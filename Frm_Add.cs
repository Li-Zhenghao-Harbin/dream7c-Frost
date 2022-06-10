using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 柒幻_霜降
{
    public partial class Frm_Add : Form
    {
        public Frm_Add()
        {
            InitializeComponent();
        }

        protected string f_content;
        private string file_name;
        private string file_content;

        private void Btn_New_Paste_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
            {
                Clipboard.SetDataObject(new RichTextBox
                {
                    Font = new Font("宋体", 12),
                    ForeColor = Color.Black,
                    BackColor = Color.Black,
                    Text = Clipboard.GetText()
                }.Text, true); // Reset text format
            }
            Rtx_New_Content.Paste();
        }

        private void Btn_Import_SelectFile_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "打开文件";
            openFileDialog.Filter = "文本文档|*.txt";
            openFileDialog.FileName = "";
            if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName.Length > 0)
            {
                StreamReader sr = new StreamReader(openFileDialog.FileName, Encoding.GetEncoding("gb2312"));
                f_content = sr.ReadToEnd();
                sr.Close();
                Tx_Import_Path.Text = openFileDialog.FileName;
            }
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Add_To_Treeview()
        {
            RichTextBox t = new RichTextBox();
            Controls.Add(t);
            t.Text = file_content;
            t.SaveFile(Frm_Main.folder_path + @"\" + file_name + ".txt", RichTextBoxStreamType.PlainText);
            t.Dispose(); 
            Controls.Remove(t);
            Frm_Main.file_name_from_add = Frm_Main.folder_path + @"\" + file_name + ".txt";
        }

        private void Btn_Confirm_Click(object sender, EventArgs e)
        {
            try
            {
                switch (TabControl.SelectedIndex)
                {
                    case 0:
                        if (Tx_New_Name.Text != "")
                        {
                            file_name = Tx_New_Name.Text;
                            file_content = Rtx_New_Content.Text;
                            Add_To_Treeview();
                            Frm_Main.temporty_text[Frm_Main.target_tab] = false;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("未输入标题", "错误");
                        }
                        break;
                    case 1:
                        if(Tx_Import_Path.Text != "")
                        {
                            file_name = Tx_Import_Path.Text.Substring(Tx_Import_Path.Text.LastIndexOf('\\'), Tx_Import_Path.Text.IndexOf('.') - Tx_Import_Path.Text.LastIndexOf('\\'));
                            file_content = f_content;
                            Add_To_Treeview();
                            Frm_Main.temporty_text[Frm_Main.target_tab] = false;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("未导入文件", "错误");
                        }
                        break;
                }
            }
            catch
            {
                MessageBox.Show("当前文件夹无写入权限", "错误");
            }
        }

        private void Frm_Add_Activated(object sender, EventArgs e)
        {
            if(TabControl.SelectedIndex == 0)
            {
                Tx_New_Name.Focus();
            }
        }

        private void Frm_Add_Load(object sender, EventArgs e)
        {
            Frm_Main.file_name_from_add = "";
        }

        private void Tx_New_Name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                Btn_Confirm_Click(this, new EventArgs());
            }
        }
    }
}
