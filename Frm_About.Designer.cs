namespace 柒幻_霜降
{
    partial class Frm_About
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
            this.Btn_Confirm = new System.Windows.Forms.Button();
            this.Lbl_Title = new System.Windows.Forms.Label();
            this.Lbl_Copyright = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Lbl_Background = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Btn_Confirm
            // 
            this.Btn_Confirm.AutoSize = true;
            this.Btn_Confirm.Location = new System.Drawing.Point(44, 316);
            this.Btn_Confirm.Name = "Btn_Confirm";
            this.Btn_Confirm.Size = new System.Drawing.Size(170, 40);
            this.Btn_Confirm.TabIndex = 1;
            this.Btn_Confirm.Text = "确认";
            this.Btn_Confirm.UseVisualStyleBackColor = true;
            this.Btn_Confirm.Click += new System.EventHandler(this.Btn_Confirm_Click);
            // 
            // Lbl_Title
            // 
            this.Lbl_Title.AutoSize = true;
            this.Lbl_Title.BackColor = System.Drawing.Color.White;
            this.Lbl_Title.Font = new System.Drawing.Font("宋体", 20F);
            this.Lbl_Title.Location = new System.Drawing.Point(35, 30);
            this.Lbl_Title.Name = "Lbl_Title";
            this.Lbl_Title.Size = new System.Drawing.Size(266, 54);
            this.Lbl_Title.TabIndex = 2;
            this.Lbl_Title.Text = "柒幻 霜降";
            this.Lbl_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Lbl_Copyright
            // 
            this.Lbl_Copyright.AutoSize = true;
            this.Lbl_Copyright.Location = new System.Drawing.Point(40, 144);
            this.Lbl_Copyright.Name = "Lbl_Copyright";
            this.Lbl_Copyright.Size = new System.Drawing.Size(322, 144);
            this.Lbl_Copyright.TabIndex = 4;
            this.Lbl_Copyright.Text = "v 3.0.9.3\r\n\r\n总设计师 | 李正浩\r\n\r\n版权所有 © 2021 柒幻工作室\r\nwww.dream7c.com";
            this.Lbl_Copyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::柒幻_霜降.Properties.Resources.dream7c_FR_logo_icon_pre;
            this.pictureBox1.Location = new System.Drawing.Point(414, 135);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(309, 221);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // Lbl_Background
            // 
            this.Lbl_Background.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Lbl_Background.Location = new System.Drawing.Point(-1, -3);
            this.Lbl_Background.Name = "Lbl_Background";
            this.Lbl_Background.Size = new System.Drawing.Size(778, 109);
            this.Lbl_Background.TabIndex = 6;
            // 
            // Frm_About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 391);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Lbl_Copyright);
            this.Controls.Add(this.Lbl_Title);
            this.Controls.Add(this.Btn_Confirm);
            this.Controls.Add(this.Lbl_Background);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_About";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "关于 柒幻 霜降";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Btn_Confirm;
        private System.Windows.Forms.Label Lbl_Title;
        private System.Windows.Forms.Label Lbl_Copyright;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label Lbl_Background;
    }
}