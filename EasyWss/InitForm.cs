using EasyWss.Utils;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace EasyWss
{
    public partial class InitForm : Form
    {
        public InitForm()
        {
            InitializeComponent();
        }

        private void InitForm_Load(object sender, EventArgs e)
        {
            if (!OsHelper.IsAdministrator())
            {
                MessageBox.Show(this,"请以管理员身份运行！","EasyWss",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Environment.Exit(0);

            }
            
        }

        private void button_Register_Click(object sender, EventArgs e)
        {
            if (OsHelper.AddFileContextMenuItem("EasyWss", "发送到文叔叔", Process.GetCurrentProcess().MainModule.FileName))
            {
                MessageBox.Show(this,"注册成功！\r\n使用：右键单击需要上传的文件，点击“发送到文叔叔”即可。", "EasyWss", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
            }
            else
            {
                MessageBox.Show(this,"注册失败！","EasyWss",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void button_UnRegister_Click(object sender, EventArgs e)
        {
            if (OsHelper.DelFileContextMenuItem("EasyWss"))
            {
                MessageBox.Show(this, "卸载成功！", "EasyWss", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "卸载失败！", "EasyWss", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
