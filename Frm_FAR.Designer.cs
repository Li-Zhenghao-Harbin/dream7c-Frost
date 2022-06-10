namespace 柒幻_霜降
{
    partial class Frm_FAR
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
            this.Check_Reverse = new System.Windows.Forms.CheckBox();
            this.Check_UL = new System.Windows.Forms.CheckBox();
            this.Tx_Find = new System.Windows.Forms.TextBox();
            this.Lbl_Find_1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Check_ReplaceAll = new System.Windows.Forms.CheckBox();
            this.Tx_Replace_2 = new System.Windows.Forms.TextBox();
            this.Lbl_Replace_2 = new System.Windows.Forms.Label();
            this.Tx_Replace_1 = new System.Windows.Forms.TextBox();
            this.Lbl_Replace_1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.Btn_Confirm = new System.Windows.Forms.Button();
            this.Btn_Count = new System.Windows.Forms.Button();
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
            this.flowLayoutPanel.TabIndex = 3;
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
            this.TabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.Check_Reverse);
            this.tabPage1.Controls.Add(this.Check_UL);
            this.tabPage1.Controls.Add(this.Tx_Find);
            this.tabPage1.Controls.Add(this.Lbl_Find_1);
            this.tabPage1.Location = new System.Drawing.Point(8, 39);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(767, 283);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "查找";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Check_Reverse
            // 
            this.Check_Reverse.AutoSize = true;
            this.Check_Reverse.Location = new System.Drawing.Point(295, 65);
            this.Check_Reverse.Name = "Check_Reverse";
            this.Check_Reverse.Size = new System.Drawing.Size(138, 28);
            this.Check_Reverse.TabIndex = 15;
            this.Check_Reverse.Text = "反向查找";
            this.Check_Reverse.UseVisualStyleBackColor = true;
            // 
            // Check_UL
            // 
            this.Check_UL.AutoSize = true;
            this.Check_UL.Checked = true;
            this.Check_UL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Check_UL.Location = new System.Drawing.Point(127, 65);
            this.Check_UL.Name = "Check_UL";
            this.Check_UL.Size = new System.Drawing.Size(162, 28);
            this.Check_UL.TabIndex = 14;
            this.Check_UL.Text = "区分大小写";
            this.Check_UL.UseVisualStyleBackColor = true;
            // 
            // Tx_Find
            // 
            this.Tx_Find.BackColor = System.Drawing.SystemColors.Window;
            this.Tx_Find.Location = new System.Drawing.Point(127, 12);
            this.Tx_Find.Name = "Tx_Find";
            this.Tx_Find.Size = new System.Drawing.Size(624, 35);
            this.Tx_Find.TabIndex = 8;
            // 
            // Lbl_Find_1
            // 
            this.Lbl_Find_1.AutoSize = true;
            this.Lbl_Find_1.Location = new System.Drawing.Point(15, 15);
            this.Lbl_Find_1.Name = "Lbl_Find_1";
            this.Lbl_Find_1.Size = new System.Drawing.Size(106, 24);
            this.Lbl_Find_1.TabIndex = 7;
            this.Lbl_Find_1.Text = "查找目标";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.Check_ReplaceAll);
            this.tabPage2.Controls.Add(this.Tx_Replace_2);
            this.tabPage2.Controls.Add(this.Lbl_Replace_2);
            this.tabPage2.Controls.Add(this.Tx_Replace_1);
            this.tabPage2.Controls.Add(this.Lbl_Replace_1);
            this.tabPage2.Location = new System.Drawing.Point(8, 39);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(767, 283);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "替换";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Check_ReplaceAll
            // 
            this.Check_ReplaceAll.AutoSize = true;
            this.Check_ReplaceAll.Location = new System.Drawing.Point(127, 115);
            this.Check_ReplaceAll.Name = "Check_ReplaceAll";
            this.Check_ReplaceAll.Size = new System.Drawing.Size(138, 28);
            this.Check_ReplaceAll.TabIndex = 13;
            this.Check_ReplaceAll.Text = "全部替换";
            this.Check_ReplaceAll.UseVisualStyleBackColor = true;
            this.Check_ReplaceAll.CheckedChanged += new System.EventHandler(this.Check_ReplaceAll_CheckedChanged);
            // 
            // Tx_Replace_2
            // 
            this.Tx_Replace_2.BackColor = System.Drawing.SystemColors.Window;
            this.Tx_Replace_2.Location = new System.Drawing.Point(127, 62);
            this.Tx_Replace_2.Name = "Tx_Replace_2";
            this.Tx_Replace_2.Size = new System.Drawing.Size(624, 35);
            this.Tx_Replace_2.TabIndex = 12;
            // 
            // Lbl_Replace_2
            // 
            this.Lbl_Replace_2.AutoSize = true;
            this.Lbl_Replace_2.Location = new System.Drawing.Point(15, 65);
            this.Lbl_Replace_2.Name = "Lbl_Replace_2";
            this.Lbl_Replace_2.Size = new System.Drawing.Size(82, 24);
            this.Lbl_Replace_2.TabIndex = 11;
            this.Lbl_Replace_2.Text = "替换为";
            // 
            // Tx_Replace_1
            // 
            this.Tx_Replace_1.BackColor = System.Drawing.SystemColors.Window;
            this.Tx_Replace_1.Location = new System.Drawing.Point(127, 12);
            this.Tx_Replace_1.Name = "Tx_Replace_1";
            this.Tx_Replace_1.Size = new System.Drawing.Size(624, 35);
            this.Tx_Replace_1.TabIndex = 10;
            // 
            // Lbl_Replace_1
            // 
            this.Lbl_Replace_1.AutoSize = true;
            this.Lbl_Replace_1.Location = new System.Drawing.Point(15, 15);
            this.Lbl_Replace_1.Name = "Lbl_Replace_1";
            this.Lbl_Replace_1.Size = new System.Drawing.Size(106, 24);
            this.Lbl_Replace_1.TabIndex = 9;
            this.Lbl_Replace_1.Text = "查找目标";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.Btn_Cancel);
            this.flowLayoutPanel2.Controls.Add(this.Btn_Confirm);
            this.flowLayoutPanel2.Controls.Add(this.Btn_Count);
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
            this.Btn_Confirm.Text = "查找";
            this.Btn_Confirm.UseVisualStyleBackColor = true;
            this.Btn_Confirm.Click += new System.EventHandler(this.Btn_Confirm_Click);
            // 
            // Btn_Count
            // 
            this.Btn_Count.Location = new System.Drawing.Point(248, 3);
            this.Btn_Count.Name = "Btn_Count";
            this.Btn_Count.Size = new System.Drawing.Size(170, 40);
            this.Btn_Count.TabIndex = 2;
            this.Btn_Count.Text = "计数";
            this.Btn_Count.UseVisualStyleBackColor = true;
            this.Btn_Count.Visible = false;
            this.Btn_Count.Click += new System.EventHandler(this.Btn_Count_Click);
            // 
            // Frm_FAR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 399);
            this.Controls.Add(this.flowLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_FAR";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Frm_FAR_Load);
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
        private System.Windows.Forms.TextBox Tx_Find;
        private System.Windows.Forms.Label Lbl_Find_1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Button Btn_Confirm;
        private System.Windows.Forms.TextBox Tx_Replace_1;
        private System.Windows.Forms.Label Lbl_Replace_1;
        private System.Windows.Forms.TextBox Tx_Replace_2;
        private System.Windows.Forms.Label Lbl_Replace_2;
        private System.Windows.Forms.CheckBox Check_ReplaceAll;
        private System.Windows.Forms.CheckBox Check_UL;
        private System.Windows.Forms.CheckBox Check_Reverse;
        private System.Windows.Forms.Button Btn_Count;
    }
}