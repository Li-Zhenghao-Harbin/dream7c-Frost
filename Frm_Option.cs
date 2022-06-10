using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 柒幻_霜降
{
    public partial class Frm_Option : Form
    {
        public Frm_Option()
        {
            InitializeComponent();
        }

        protected string folder_path;

        private void Btn_Select_Folder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = "选择文件夹"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                folder_path = dialog.SelectedPath;
                Tx_Folder_Path.Text = folder_path;
            }
            dialog.Dispose();
        }

        private void GetFontNamesFromSystem()
        {
            FontFamily[] fontFamilies;
            Combo_Font.Items.Clear();
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();
            fontFamilies = installedFontCollection.Families;
            int count = fontFamilies.Length;
            for (int j = 0; j < count; ++j)
            {
                Combo_Font.Items.Add(fontFamilies[j].Name);
            }
        }

        private void Frm_Option_Load(object sender, EventArgs e)
        {
            GetFontNamesFromSystem();
            Tx_Folder_Path.Text = Frm_Main.folder_path;
            Combo_Font.SelectedItem = Frm_Main.font_name;
            Num_Font_Size.Value = Frm_Main.font_size;
            Btn_Forecolor.BackColor = ColorTranslator.FromHtml(Frm_Main.forecolor);
            Btn_Backcolor.BackColor = ColorTranslator.FromHtml(Frm_Main.backcolor);
            Check_ExtendContent.Checked = Frm_Main.extend_file_content;
            Check_ExtendPairDocument.Checked = Frm_Main.extend_pair_document;
            Tx_Generate_Spliter.Text = Frm_Main.generate_separator.ToString();
            Num_DecimalPlace.Value = Frm_Main.decimal_place;
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Btn_Confirm_Click(object sender, EventArgs e)
        {
            if (Tx_Generate_Spliter.Text == "")
            {
                TabControl.SelectedIndex = 3;
                MessageBox.Show("生成分隔符不能为空", "错误");
                return;
            }
            Frm_Main.folder_path = Tx_Folder_Path.Text;
            Frm_Main.font_name = Combo_Font.SelectedItem.ToString();
            Frm_Main.font_size = (int)Num_Font_Size.Value;
            Frm_Main.forecolor = ColorTranslator.ToHtml(Btn_Forecolor.BackColor);
            Frm_Main.backcolor = ColorTranslator.ToHtml(Btn_Backcolor.BackColor);
            Frm_Main.extend_file_content = Check_ExtendContent.Checked;
            Frm_Main.extend_pair_document = Check_ExtendPairDocument.Checked;
            Frm_Main.generate_separator = Tx_Generate_Spliter.Text.ToCharArray()[0];
            Frm_Main.decimal_place = (int)Num_DecimalPlace.Value;
            for (int i = 0; i < 4; i++)
            {
                Frm_Main.Rtxs[i].Font = new Font(Frm_Main.font_name, Frm_Main.font_size);
                Frm_Main.Rtxs[i].ForeColor = Btn_Forecolor.BackColor;
                Frm_Main.Rtxs[i].BackColor = Btn_Backcolor.BackColor;
            }
            Frm_Main frm_Main = new Frm_Main();
            Frm_Main.Rtx_solstice[0].Font = Frm_Main.Rtx_output[0].Font = Frm_Main.Rtxs[0].Font;
            Frm_Main.Rtx_solstice[0].ForeColor = Frm_Main.Rtx_output[0].ForeColor = Frm_Main.Rtxs[0].ForeColor;
            Frm_Main.Rtx_solstice[0].BackColor = Frm_Main.Rtx_output[0].BackColor = Frm_Main.Rtxs[0].BackColor;
            frm_Main.SaveConfig();
            Frm_Main.change_folder_path = true;
            Close();
            if (Frm_Main.Tabcontrols[0].TabPages[1].Text.Substring(Frm_Main.Tabcontrols[0].TabPages[1].Text.Length - Frm_Main.save_tag_length) == "（未保存）")
            {
                Frm_Main.Tabcontrols[0].TabPages[1].Text = Frm_Main.Tabcontrols[0].TabPages[1].Text.Substring(0, Frm_Main.Tabcontrols[0].TabPages[1].Text.Length - Frm_Main.save_tag_length);
            }
            if (Frm_Main.Tabcontrols[1].TabPages[1].Text.Substring(Frm_Main.Tabcontrols[1].TabPages[1].Text.Length - Frm_Main.save_tag_length) == "（未保存）")
            {
                Frm_Main.Tabcontrols[1].TabPages[1].Text = Frm_Main.Tabcontrols[1].TabPages[1].Text.Substring(0, Frm_Main.Tabcontrols[1].TabPages[1].Text.Length - Frm_Main.save_tag_length);
            }
        }

        private void SetColor(object sender)
        {
            Button btn = (Button)sender;
            ColorDialog colordialog = new ColorDialog();
            if (colordialog.ShowDialog() == DialogResult.OK)
            {
                btn.BackColor = colordialog.Color;
            }
            colordialog.Dispose();
        }

        private void Btn_Forecolor_Click(object sender, EventArgs e)
        {
            SetColor(Btn_Forecolor);
        }

        private void Btn_Backcolor_Click(object sender, EventArgs e)
        {
            SetColor(Btn_Backcolor);
        }
    }
}
