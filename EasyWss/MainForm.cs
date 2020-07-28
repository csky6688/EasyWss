using System;
using System.Threading;
using System.Windows.Forms;

namespace EasyWss
{
    public partial class MainForm : Form
    {
        string filePath = string.Empty;
        public MainForm(string fp)
        {
            InitializeComponent();
            filePath = fp;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            label_FilePath.Text = filePath;
            Thread thread = new Thread(() =>
            {
                string shareUrl = Wss.Upload(filePath, label_Progress);
                Invoke(new Action(()=>
                {
                    MessageBox.Show(this, "上传成功，已复制分享链接至剪贴板，上传记录可在 Logs 目录下查看！", "EasyWss", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
                Clipboard.SetDataObject(shareUrl, true);
                Environment.Exit(0);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
            
        }
    }
}
