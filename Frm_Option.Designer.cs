namespace 柒幻_霜降
{
    partial class Frm_Option
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
            this.Btn_Select_Folder = new System.Windows.Forms.Button();
            this.Tx_Folder_Path = new System.Windows.Forms.TextBox();
            this.Lbl_Folder_Title = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Btn_Backcolor = new System.Windows.Forms.Button();
            this.Lbl_backcolor = new System.Windows.Forms.Label();
            this.Btn_Forecolor = new System.Windows.Forms.Button();
            this.Lbl_Forecolor = new System.Windows.Forms.Label();
            this.Num_Font_Size = new System.Windows.Forms.NumericUpDown();
            this.Lbl_Font_Size = new System.Windows.Forms.Label();
            this.Combo_Font = new System.Windows.Forms.ComboBox();
            this.Lbl_Font_Name = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.Check_ExtendContent = new System.Windows.Forms.CheckBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.Tx_Generate_Spliter = new System.Windows.Forms.TextBox();
            this.Lbl_GenerateSpliter = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.Btn_Confirm = new System.Windows.Forms.Button();
            this.Lbl_DecimalPlace = new System.Windows.Forms.Label();
            this.Num_DecimalPlace = new System.Windows.Forms.NumericUpDown();
            this.Check_ExtendPairDocument = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Num_Font_Size)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Num_DecimalPlace)).BeginInit();
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
            this.flowLayoutPanel.TabIndex = 2;
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.tabPage1);
            this.TabControl.Controls.Add(this.tabPage2);
            this.TabControl.Controls.Add(this.tabPage3);
            this.TabControl.Controls.Add(this.tabPage4);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.HotTrack = true;
            this.TabControl.Location = new System.Drawing.Point(3, 3);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(783, 330);
            this.TabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.Btn_Select_Folder);
            this.tabPage1.Controls.Add(this.Tx_Folder_Path);
            this.tabPage1.Controls.Add(this.Lbl_Folder_Title);
            this.tabPage1.Location = new System.Drawing.Point(8, 39);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(767, 283);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "目录";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Btn_Select_Folder
            // 
            this.Btn_Select_Folder.AutoSize = true;
            this.Btn_Select_Folder.Location = new System.Drawing.Point(151, 55);
            this.Btn_Select_Folder.Name = "Btn_Select_Folder";
            this.Btn_Select_Folder.Size = new System.Drawing.Size(170, 40);
            this.Btn_Select_Folder.TabIndex = 9;
            this.Btn_Select_Folder.Text = "选择文件夹";
            this.Btn_Select_Folder.UseVisualStyleBackColor = true;
            this.Btn_Select_Folder.Click += new System.EventHandler(this.Btn_Select_Folder_Click);
            // 
            // Tx_Folder_Path
            // 
            this.Tx_Folder_Path.Enabled = false;
            this.Tx_Folder_Path.Location = new System.Drawing.Point(151, 12);
            this.Tx_Folder_Path.Name = "Tx_Folder_Path";
            this.Tx_Folder_Path.ReadOnly = true;
            this.Tx_Folder_Path.Size = new System.Drawing.Size(600, 35);
            this.Tx_Folder_Path.TabIndex = 8;
            // 
            // Lbl_Folder_Title
            // 
            this.Lbl_Folder_Title.AutoSize = true;
            this.Lbl_Folder_Title.Location = new System.Drawing.Point(15, 15);
            this.Lbl_Folder_Title.Name = "Lbl_Folder_Title";
            this.Lbl_Folder_Title.Size = new System.Drawing.Size(130, 24);
            this.Lbl_Folder_Title.TabIndex = 7;
            this.Lbl_Folder_Title.Text = "文件夹路径";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.Btn_Backcolor);
            this.tabPage2.Controls.Add(this.Lbl_backcolor);
            this.tabPage2.Controls.Add(this.Btn_Forecolor);
            this.tabPage2.Controls.Add(this.Lbl_Forecolor);
            this.tabPage2.Controls.Add(this.Num_Font_Size);
            this.tabPage2.Controls.Add(this.Lbl_Font_Size);
            this.tabPage2.Controls.Add(this.Combo_Font);
            this.tabPage2.Controls.Add(this.Lbl_Font_Name);
            this.tabPage2.Location = new System.Drawing.Point(8, 39);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(767, 283);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "字体和颜色";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Btn_Backcolor
            // 
            this.Btn_Backcolor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Backcolor.Location = new System.Drawing.Point(104, 162);
            this.Btn_Backcolor.Name = "Btn_Backcolor";
            this.Btn_Backcolor.Size = new System.Drawing.Size(120, 32);
            this.Btn_Backcolor.TabIndex = 8;
            this.Btn_Backcolor.UseVisualStyleBackColor = true;
            this.Btn_Backcolor.Click += new System.EventHandler(this.Btn_Backcolor_Click);
            // 
            // Lbl_backcolor
            // 
            this.Lbl_backcolor.AutoSize = true;
            this.Lbl_backcolor.Location = new System.Drawing.Point(15, 165);
            this.Lbl_backcolor.Name = "Lbl_backcolor";
            this.Lbl_backcolor.Size = new System.Drawing.Size(82, 24);
            this.Lbl_backcolor.TabIndex = 7;
            this.Lbl_backcolor.Text = "背景色";
            // 
            // Btn_Forecolor
            // 
            this.Btn_Forecolor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Forecolor.Location = new System.Drawing.Point(104, 112);
            this.Btn_Forecolor.Name = "Btn_Forecolor";
            this.Btn_Forecolor.Size = new System.Drawing.Size(120, 32);
            this.Btn_Forecolor.TabIndex = 6;
            this.Btn_Forecolor.UseVisualStyleBackColor = true;
            this.Btn_Forecolor.Click += new System.EventHandler(this.Btn_Forecolor_Click);
            // 
            // Lbl_Forecolor
            // 
            this.Lbl_Forecolor.AutoSize = true;
            this.Lbl_Forecolor.Location = new System.Drawing.Point(15, 115);
            this.Lbl_Forecolor.Name = "Lbl_Forecolor";
            this.Lbl_Forecolor.Size = new System.Drawing.Size(82, 24);
            this.Lbl_Forecolor.TabIndex = 5;
            this.Lbl_Forecolor.Text = "前景色";
            // 
            // Num_Font_Size
            // 
            this.Num_Font_Size.Location = new System.Drawing.Point(104, 62);
            this.Num_Font_Size.Name = "Num_Font_Size";
            this.Num_Font_Size.Size = new System.Drawing.Size(120, 35);
            this.Num_Font_Size.TabIndex = 4;
            // 
            // Lbl_Font_Size
            // 
            this.Lbl_Font_Size.AutoSize = true;
            this.Lbl_Font_Size.Location = new System.Drawing.Point(15, 65);
            this.Lbl_Font_Size.Name = "Lbl_Font_Size";
            this.Lbl_Font_Size.Size = new System.Drawing.Size(58, 24);
            this.Lbl_Font_Size.TabIndex = 3;
            this.Lbl_Font_Size.Text = "大小";
            // 
            // Combo_Font
            // 
            this.Combo_Font.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Combo_Font.FormattingEnabled = true;
            this.Combo_Font.Location = new System.Drawing.Point(104, 12);
            this.Combo_Font.Name = "Combo_Font";
            this.Combo_Font.Size = new System.Drawing.Size(400, 32);
            this.Combo_Font.TabIndex = 2;
            // 
            // Lbl_Font_Name
            // 
            this.Lbl_Font_Name.AutoSize = true;
            this.Lbl_Font_Name.Location = new System.Drawing.Point(15, 15);
            this.Lbl_Font_Name.Name = "Lbl_Font_Name";
            this.Lbl_Font_Name.Size = new System.Drawing.Size(58, 24);
            this.Lbl_Font_Name.TabIndex = 1;
            this.Lbl_Font_Name.Text = "字体";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.Check_ExtendPairDocument);
            this.tabPage3.Controls.Add(this.Check_ExtendContent);
            this.tabPage3.Location = new System.Drawing.Point(8, 39);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(767, 283);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "文档";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // Check_ExtendContent
            // 
            this.Check_ExtendContent.AutoSize = true;
            this.Check_ExtendContent.Checked = true;
            this.Check_ExtendContent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Check_ExtendContent.Location = new System.Drawing.Point(15, 15);
            this.Check_ExtendContent.Name = "Check_ExtendContent";
            this.Check_ExtendContent.Size = new System.Drawing.Size(306, 28);
            this.Check_ExtendContent.TabIndex = 0;
            this.Check_ExtendContent.Text = "新建临时文件时继承内容";
            this.Check_ExtendContent.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.Num_DecimalPlace);
            this.tabPage4.Controls.Add(this.Lbl_DecimalPlace);
            this.tabPage4.Controls.Add(this.Tx_Generate_Spliter);
            this.tabPage4.Controls.Add(this.Lbl_GenerateSpliter);
            this.tabPage4.Location = new System.Drawing.Point(8, 39);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(767, 283);
            this.tabPage4.TabIndex = 2;
            this.tabPage4.Text = "数据生成";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // Tx_Generate_Spliter
            // 
            this.Tx_Generate_Spliter.Location = new System.Drawing.Point(176, 62);
            this.Tx_Generate_Spliter.MaxLength = 1;
            this.Tx_Generate_Spliter.Name = "Tx_Generate_Spliter";
            this.Tx_Generate_Spliter.Size = new System.Drawing.Size(90, 35);
            this.Tx_Generate_Spliter.TabIndex = 10;
            // 
            // Lbl_GenerateSpliter
            // 
            this.Lbl_GenerateSpliter.AutoSize = true;
            this.Lbl_GenerateSpliter.Location = new System.Drawing.Point(15, 65);
            this.Lbl_GenerateSpliter.Name = "Lbl_GenerateSpliter";
            this.Lbl_GenerateSpliter.Size = new System.Drawing.Size(130, 24);
            this.Lbl_GenerateSpliter.TabIndex = 9;
            this.Lbl_GenerateSpliter.Text = "生成分隔符";
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
            this.Btn_Cancel.AutoSize = true;
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
            this.Btn_Confirm.AutoSize = true;
            this.Btn_Confirm.Location = new System.Drawing.Point(424, 3);
            this.Btn_Confirm.Name = "Btn_Confirm";
            this.Btn_Confirm.Size = new System.Drawing.Size(170, 40);
            this.Btn_Confirm.TabIndex = 1;
            this.Btn_Confirm.Text = "确认";
            this.Btn_Confirm.UseVisualStyleBackColor = true;
            this.Btn_Confirm.Click += new System.EventHandler(this.Btn_Confirm_Click);
            // 
            // Lbl_DecimalPlace
            // 
            this.Lbl_DecimalPlace.AutoSize = true;
            this.Lbl_DecimalPlace.Location = new System.Drawing.Point(15, 15);
            this.Lbl_DecimalPlace.Name = "Lbl_DecimalPlace";
            this.Lbl_DecimalPlace.Size = new System.Drawing.Size(154, 24);
            this.Lbl_DecimalPlace.TabIndex = 11;
            this.Lbl_DecimalPlace.Text = "保留有效位数";
            // 
            // Num_DecimalPlace
            // 
            this.Num_DecimalPlace.Location = new System.Drawing.Point(176, 12);
            this.Num_DecimalPlace.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.Num_DecimalPlace.Name = "Num_DecimalPlace";
            this.Num_DecimalPlace.Size = new System.Drawing.Size(120, 35);
            this.Num_DecimalPlace.TabIndex = 12;
            // 
            // Check_ExtendPairDocument
            // 
            this.Check_ExtendPairDocument.AutoSize = true;
            this.Check_ExtendPairDocument.Checked = true;
            this.Check_ExtendPairDocument.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Check_ExtendPairDocument.Location = new System.Drawing.Point(15, 65);
            this.Check_ExtendPairDocument.Name = "Check_ExtendPairDocument";
            this.Check_ExtendPairDocument.Size = new System.Drawing.Size(378, 28);
            this.Check_ExtendPairDocument.TabIndex = 1;
            this.Check_ExtendPairDocument.Text = "临时文件创建文档组时继承内容";
            this.Check_ExtendPairDocument.UseVisualStyleBackColor = true;
            // 
            // Frm_Option
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 399);
            this.Controls.Add(this.flowLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_Option";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选项";
            this.Load += new System.EventHandler(this.Frm_Option_Load);
            this.flowLayoutPanel.ResumeLayout(false);
            this.TabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Num_Font_Size)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Num_DecimalPlace)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label Lbl_Font_Name;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Button Btn_Confirm;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button Btn_Select_Folder;
        private System.Windows.Forms.TextBox Tx_Folder_Path;
        private System.Windows.Forms.Label Lbl_Folder_Title;
        private System.Windows.Forms.ComboBox Combo_Font;
        private System.Windows.Forms.NumericUpDown Num_Font_Size;
        private System.Windows.Forms.Label Lbl_Font_Size;
        private System.Windows.Forms.Label Lbl_Forecolor;
        private System.Windows.Forms.Button Btn_Forecolor;
        private System.Windows.Forms.Button Btn_Backcolor;
        private System.Windows.Forms.Label Lbl_backcolor;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox Tx_Generate_Spliter;
        private System.Windows.Forms.Label Lbl_GenerateSpliter;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox Check_ExtendContent;
        private System.Windows.Forms.Label Lbl_DecimalPlace;
        private System.Windows.Forms.NumericUpDown Num_DecimalPlace;
        private System.Windows.Forms.CheckBox Check_ExtendPairDocument;
    }
}