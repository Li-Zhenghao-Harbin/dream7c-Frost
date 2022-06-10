namespace 柒幻_霜降
{
    partial class Frm_Add
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
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.Tx_New_Name = new System.Windows.Forms.TextBox();
            this.Lbl_New_Name = new System.Windows.Forms.Label();
            this.Btn_New_Paste = new System.Windows.Forms.Button();
            this.Rtx_New_Content = new System.Windows.Forms.RichTextBox();
            this.Lbl_New_Content = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Btn_Import_SelectFile = new System.Windows.Forms.Button();
            this.Tx_Import_Path = new System.Windows.Forms.TextBox();
            this.Lbl_Import_Path = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.Btn_Confirm = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.flowLayoutPanel.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Controls.Add(this.TabControl);
            this.flowLayoutPanel.Controls.Add(this.flowLayoutPanel2);
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(789, 399);
            this.flowLayoutPanel.TabIndex = 1;
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.tabPage1);
            this.TabControl.Controls.Add(this.tabPage2);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Location = new System.Drawing.Point(3, 3);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(783, 330);
            this.TabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.Tx_New_Name);
            this.tabPage1.Controls.Add(this.Lbl_New_Name);
            this.tabPage1.Controls.Add(this.Btn_New_Paste);
            this.tabPage1.Controls.Add(this.Rtx_New_Content);
            this.tabPage1.Controls.Add(this.Lbl_New_Content);
            this.tabPage1.Location = new System.Drawing.Point(8, 39);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(767, 283);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "新建";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Tx_New_Name
            // 
            this.Tx_New_Name.Location = new System.Drawing.Point(80, 224);
            this.Tx_New_Name.Name = "Tx_New_Name";
            this.Tx_New_Name.Size = new System.Drawing.Size(500, 35);
            this.Tx_New_Name.TabIndex = 4;
            this.Tx_New_Name.Text = "新建文件";
            this.Tx_New_Name.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tx_New_Name_KeyDown);
            // 
            // Lbl_New_Name
            // 
            this.Lbl_New_Name.AutoSize = true;
            this.Lbl_New_Name.Location = new System.Drawing.Point(15, 227);
            this.Lbl_New_Name.Name = "Lbl_New_Name";
            this.Lbl_New_Name.Size = new System.Drawing.Size(58, 24);
            this.Lbl_New_Name.TabIndex = 3;
            this.Lbl_New_Name.Text = "标题";
            // 
            // Btn_New_Paste
            // 
            this.Btn_New_Paste.Location = new System.Drawing.Point(587, 15);
            this.Btn_New_Paste.Name = "Btn_New_Paste";
            this.Btn_New_Paste.Size = new System.Drawing.Size(170, 40);
            this.Btn_New_Paste.TabIndex = 2;
            this.Btn_New_Paste.Text = "从剪切板粘贴";
            this.Btn_New_Paste.UseVisualStyleBackColor = true;
            this.Btn_New_Paste.Click += new System.EventHandler(this.Btn_New_Paste_Click);
            // 
            // Rtx_New_Content
            // 
            this.Rtx_New_Content.DetectUrls = false;
            this.Rtx_New_Content.Location = new System.Drawing.Point(80, 15);
            this.Rtx_New_Content.Name = "Rtx_New_Content";
            this.Rtx_New_Content.Size = new System.Drawing.Size(500, 200);
            this.Rtx_New_Content.TabIndex = 1;
            this.Rtx_New_Content.Text = "";
            this.Rtx_New_Content.WordWrap = false;
            // 
            // Lbl_New_Content
            // 
            this.Lbl_New_Content.AutoSize = true;
            this.Lbl_New_Content.Location = new System.Drawing.Point(15, 15);
            this.Lbl_New_Content.Name = "Lbl_New_Content";
            this.Lbl_New_Content.Size = new System.Drawing.Size(58, 24);
            this.Lbl_New_Content.TabIndex = 0;
            this.Lbl_New_Content.Text = "内容";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.Btn_Import_SelectFile);
            this.tabPage2.Controls.Add(this.Tx_Import_Path);
            this.tabPage2.Controls.Add(this.Lbl_Import_Path);
            this.tabPage2.Location = new System.Drawing.Point(8, 39);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(767, 283);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "导入";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Btn_Import_SelectFile
            // 
            this.Btn_Import_SelectFile.Location = new System.Drawing.Point(79, 55);
            this.Btn_Import_SelectFile.Name = "Btn_Import_SelectFile";
            this.Btn_Import_SelectFile.Size = new System.Drawing.Size(170, 40);
            this.Btn_Import_SelectFile.TabIndex = 6;
            this.Btn_Import_SelectFile.Text = "选择文件";
            this.Btn_Import_SelectFile.UseVisualStyleBackColor = true;
            this.Btn_Import_SelectFile.Click += new System.EventHandler(this.Btn_Import_SelectFile_Click);
            // 
            // Tx_Import_Path
            // 
            this.Tx_Import_Path.Enabled = false;
            this.Tx_Import_Path.Location = new System.Drawing.Point(79, 12);
            this.Tx_Import_Path.Name = "Tx_Import_Path";
            this.Tx_Import_Path.Size = new System.Drawing.Size(668, 35);
            this.Tx_Import_Path.TabIndex = 5;
            // 
            // Lbl_Import_Path
            // 
            this.Lbl_Import_Path.AutoSize = true;
            this.Lbl_Import_Path.Location = new System.Drawing.Point(15, 15);
            this.Lbl_Import_Path.Name = "Lbl_Import_Path";
            this.Lbl_Import_Path.Size = new System.Drawing.Size(58, 24);
            this.Lbl_Import_Path.TabIndex = 1;
            this.Lbl_Import_Path.Text = "路径";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.Btn_Cancel);
            this.flowLayoutPanel2.Controls.Add(this.Btn_Confirm);
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 339);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.flowLayoutPanel2.Size = new System.Drawing.Size(783, 46);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.Location = new System.Drawing.Point(600, 3);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(170, 40);
            this.Btn_Cancel.TabIndex = 0;
            this.Btn_Cancel.Text = "取消";
            this.Btn_Cancel.UseVisualStyleBackColor = true;
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // Btn_Confirm
            // 
            this.Btn_Confirm.Location = new System.Drawing.Point(424, 3);
            this.Btn_Confirm.Name = "Btn_Confirm";
            this.Btn_Confirm.Size = new System.Drawing.Size(170, 40);
            this.Btn_Confirm.TabIndex = 1;
            this.Btn_Confirm.Text = "确认";
            this.Btn_Confirm.UseVisualStyleBackColor = true;
            this.Btn_Confirm.Click += new System.EventHandler(this.Btn_Confirm_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // Frm_Add
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 399);
            this.Controls.Add(this.flowLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Frm_Add";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加文件";
            this.Activated += new System.EventHandler(this.Frm_Add_Activated);
            this.Load += new System.EventHandler(this.Frm_Add_Load);
            this.flowLayoutPanel.ResumeLayout(false);
            this.TabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Button Btn_Confirm;
        private System.Windows.Forms.Button Btn_New_Paste;
        private System.Windows.Forms.RichTextBox Rtx_New_Content;
        private System.Windows.Forms.Label Lbl_New_Content;
        private System.Windows.Forms.TextBox Tx_New_Name;
        private System.Windows.Forms.Label Lbl_New_Name;
        private System.Windows.Forms.TextBox Tx_Import_Path;
        private System.Windows.Forms.Label Lbl_Import_Path;
        private System.Windows.Forms.Button Btn_Import_SelectFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}