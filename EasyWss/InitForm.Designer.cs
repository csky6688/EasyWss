namespace EasyWss
{
    partial class InitForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitForm));
            this.button_Register = new System.Windows.Forms.Button();
            this.button_UnRegister = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_Register
            // 
            this.button_Register.Location = new System.Drawing.Point(12, 21);
            this.button_Register.Name = "button_Register";
            this.button_Register.Size = new System.Drawing.Size(95, 34);
            this.button_Register.TabIndex = 0;
            this.button_Register.Text = "注册程序";
            this.button_Register.UseVisualStyleBackColor = true;
            this.button_Register.Click += new System.EventHandler(this.button_Register_Click);
            // 
            // button_UnRegister
            // 
            this.button_UnRegister.Location = new System.Drawing.Point(214, 21);
            this.button_UnRegister.Name = "button_UnRegister";
            this.button_UnRegister.Size = new System.Drawing.Size(95, 34);
            this.button_UnRegister.TabIndex = 1;
            this.button_UnRegister.Text = "卸载程序";
            this.button_UnRegister.UseVisualStyleBackColor = true;
            this.button_UnRegister.Click += new System.EventHandler(this.button_UnRegister_Click);
            // 
            // InitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 80);
            this.Controls.Add(this.button_UnRegister);
            this.Controls.Add(this.button_Register);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "InitForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EasyWss";
            this.Load += new System.EventHandler(this.InitForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Register;
        private System.Windows.Forms.Button button_UnRegister;
    }
}