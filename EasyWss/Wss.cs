using EasyWss.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace EasyWss
{
    class Wss
    {

        static string xToken = LoginAnonymous();
        static string Bid, Ufileid, Tid, UpId;


        public static string Upload(string filePath, Label lb)
        {
            Logger log = Logger.GetLogger("Upload");
            bool isPart = false;
            long fileSize = FileHelper.GetFileSize(filePath);
            if (fileSize > 2097152)
            {
                isPart = true;
            }
            string fileName = FileHelper.GetFileName(filePath);
            string shareUrl = string.Empty;
            try
            {
                string stateCode = AddSend(fileSize.ToString());
                if (isPart)
                {
                    //大文件分块上传
                    using (FileStream rdr = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {

                        byte[] inData = new byte[2 * 1024 * 1024];
                        int bytesRead = rdr.Read(inData, 0, inData.Length);
                        int partNum = 1;
                        //已上传的字节数 
                        long offset = 0;
                        DateTime startTime = DateTime.Now;
                        while (bytesRead > 0)
                        {
                            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(PsUrl(isPart, fileName, UpId, inData.Length, partNum));
                            req.Method = "PUT";
                            req.AllowWriteStreamBuffering = true;
                            req.SendChunked = false;
                            req.KeepAlive = true;
                            Stream reqStream = req.GetRequestStream();
                            reqStream.Write(inData, 0, bytesRead);
                            offset += bytesRead;
                            TimeSpan span = DateTime.Now - startTime;
                            double second = span.TotalSeconds;
                            //Console.WriteLine("已用时：" + second.ToString("F2") + "秒");
                            //if (second > 0.001)
                            //{
                            //    Console.WriteLine(" 平均速度：" + (offset / 1024 / second).ToString("0.00") + "KB/秒");
                            //}
                            //else
                            //{
                            //    Console.WriteLine("正在连接……");
                            //}
                            UpdateProgressText((offset * 100.0 / rdr.Length).ToString("F2") + "%", lb);
                            bytesRead = rdr.Read(inData, 0, inData.Length);
                            partNum += 1;
                            reqStream.Close();
                            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                        }
                    }
                }
                else
                {
                    //小文件直接上传
                    string uploadUrl = PsUrl(isPart, fileName, UpId, fileSize, 0);
                    //Console.WriteLine(stateCode+"\n"+xToken + "\n\n" + uploadUrl);
                    HttpHelper.Put(uploadUrl, filePath);
                    UpdateProgressText("整块上传中……", lb);

                }
                shareUrl = Complete(isPart, fileName, UpId, Tid, Bid, Ufileid);
                GetProcess(UpId);
                log.Info(fileName + "：" + shareUrl);
                
            }
            catch (Exception error)
            {

                log.Error(error.Message);
            }
            return shareUrl;


        }

        private static void UpdateProgressText(string str, Label lb)
        {
            lb.BeginInvoke(new Action(delegate
            {

                lb.Text = str;

            }));
        }

        #region API
        private static string LoginAnonymous()
        {
            string postData = "{\"dev_info\":\"\"}";
            string ret = HttpHelper.Post("https://www.wenshushu.cn/ap/login/anonymous", postData, "");
            JObject o = JObject.Parse(ret);
            return (string)o["data"]["token"];

        }

        private static string AddSend(string fileSize)
        {
            string postData = "{\"sender\":\"\",\"remark\":\"\",\"isextension\":false,\"pwd\":\"\",\"expire\":2,\"recvs\":[\"social\",\"public\"],\"file_size\":" + fileSize + ",\"file_count\":1,\"notSaveTo\":false,\"trafficStatus\":0,\"downPreCountLimit\":0}";
            string ret = HttpHelper.Post("https://www.wenshushu.cn/ap/task/addsend", postData, xToken);
            JObject o = JObject.Parse(ret);
            Bid = (string)o["data"]["bid"];
            Ufileid = (string)o["data"]["ufileid"];
            Tid = (string)o["data"]["tid"];
            UpId = GetUpId(Bid, Ufileid, Tid, fileSize);
            return (string)o["code"];
        }


        private static string GetUpId(string bid, string ufileid, string tid, string fileSize)
        {
            string postData = "{\"preid\":\"" + ufileid + "\",\"boxid\":\"" + bid + "\",\"linkid\":\"" + tid + "\",\"utype\":\"sendcopy\",\"originUpid\":\"\",\"length\":" + fileSize + ",\"count\":1}";
            string ret = HttpHelper.Post("https://www.wenshushu.cn/ap/uploadv2/getupid", postData, xToken);
            JObject o = JObject.Parse(ret);
            return (string)o["data"]["upId"];
        }

        private static string PsUrl(bool isPart, string fileName, string upId, long fileSize, int partNum)
        {
            string postData;
            if (isPart)
            {
                postData = "{\"ispart\":true,\"fname\":\"" + fileName + "\",\"partnu\":" + partNum + ",\"fsize\":" + fileSize + ",\"upId\":\"" + upId + "\"}";
            }
            else
            {
                postData = "{\"ispart\":false,\"fname\":\"" + fileName + "\",\"fsize\":" + fileSize + ",\"upId\":\"" + upId + "\"}";
            }
            string ret = HttpHelper.Post("https://www.wenshushu.cn/ap/uploadv2/psurl", postData, xToken);
            JObject o = JObject.Parse(ret);
            return (string)o["data"]["url"];
        }

        private static string Complete(bool isPart, string fileName, string upId, string tid, string boxid, string preid)
        {
            string postData = "{\"ispart\":" + isPart.ToString().ToLower() + ",\"fname\":\"" + fileName + "\",\"location\":{\"boxid\":\"" + boxid + "\",\"preid\":\"" + preid + "\"},\"upId\":\"" + upId + "\"}";
            HttpHelper.Post("https://www.wenshushu.cn/ap/uploadv2/complete", postData, xToken);
            return CopySend(boxid, tid, preid);
        }

        private static string CopySend(string boxid, string taskid, string preid)
        {
            string postData = "{\"tid\":\"" + taskid + "\",\"bid\":\"" + boxid + "\",\"ufileid\":\"" + preid + "\"}";
            string ret = HttpHelper.Post("https://www.wenshushu.cn/ap/task/copysend", postData, xToken);
            JObject o = JObject.Parse(ret);
            return (string)o["data"]["public_url"];

        }

        private static void GetProcess(string upId)
        {
            while (true)
            {
                string postData = "{\"processId\":\"" + upId + "\"}";
                string ret = HttpHelper.Post("https://www.wenshushu.cn/ap/ufile/getprocess", postData, xToken);
                JObject o = JObject.Parse(ret);
                if ((string)o["data"]["rst"] == "success")
                {
                    return;
                }
                Thread.Sleep(1000);
            }
        }
        #endregion



    }
}
