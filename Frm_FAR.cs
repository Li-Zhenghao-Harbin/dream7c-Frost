using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 柒幻_霜降
{
    public partial class Frm_FAR : Form
    {
        public Frm_FAR()
        {
            InitializeComponent();
        }

        // Find
        int find_start = 0;
        readonly int find_count = 0;
        int sun = 0;

        private void Frm_FAR_Load(object sender, EventArgs e)
        {
            TabControl.SelectedIndex = Frm_Main.FAR_type;
            Text = TabControl.TabPages[TabControl.SelectedIndex].Text;
            switch (Frm_Main.FAR_type)
            {
                case 0:
                    Tx_Find.Focus();
                    Tx_Find.Text = Frm_Main.Rtxs_file[Frm_Main.target_tab].SelectedText;
                    Tx_Find.SelectAll();
                    break;
                case 1:
                    Tx_Replace_1.Focus();
                    Tx_Replace_1.Text = Frm_Main.Rtxs_file[Frm_Main.target_tab].SelectedText;
                    Tx_Replace_1.SelectAll();
                    break;
            }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Text = TabControl.TabPages[TabControl.SelectedIndex].Text;
            switch (TabControl.SelectedIndex)
            {
                case 0:
                    Btn_Count.Visible = false;
                    Btn_Confirm.Text = "查找";
                    Tx_Find.Focus();
                    Tx_Find.Text = Tx_Replace_1.Text;
                    Tx_Find.SelectAll();
                    break;
                case 1:
                    Btn_Count.Visible = false;
                    Btn_Confirm.Text = "替换选中";
                    if (Check_ReplaceAll.Checked)
                    {
                        Btn_Confirm.Text = "替换全部";
                    }
                    Tx_Replace_1.Focus();
                    Tx_Replace_1.Text = Tx_Find.Text;
                    Tx_Replace_1.SelectAll();
                    break;
            }
        }

        private void Btn_Confirm_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = Frm_Main.Rtxs_file[Frm_Main.target_tab];
            RichTextBoxFinds find_type = RichTextBoxFinds.MatchCase;
            if (!Check_UL.Checked)
            {
                 find_type = RichTextBoxFinds.None;
            }
            switch (TabControl.SelectedIndex)
            {
                case 0:
                    if (Tx_Find.Text != "")
                    {
                        if (!Check_Reverse.Checked)
                        {
                            string str1 = Tx_Find.Text;
                            if (find_start >= rtb.Text.Length)
                            {
                                MessageBox.Show("已查至尾部或未查找到", "提示");
                                find_start = 0;
                            }
                            else
                            {
                                try
                                {
                                    find_start = rtb.Find(str1, find_start, find_type);
                                }
                                catch { }
                                if (find_start == -1)
                                {
                                    MessageBox.Show("已查至尾部或未查找到", "提示");
                                    if (find_count != 0)
                                    {
                                        find_start = 0;
                                    }
                                }
                                else
                                {
                                    find_start += str1.Length;
                                    rtb.Focus();
                                }
                            }
                        }
                        else
                        {
                            string str = Tx_Find.Text;
                            int rbox1 = rtb.SelectionStart;
                            int index = rtb.Find(str, 0, rbox1, RichTextBoxFinds.Reverse);
                            if (index > -1)
                            {
                                rtb.SelectionStart = index;
                                rtb.SelectionLength = str.Length;
                                sun++;
                                rtb.Focus();
                            }
                            else if (index < 0)
                            {
                                sun = 0;
                                MessageBox.Show("已查至顶部或未查找到", "提示");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("输入有误", "错误");
                    }
                    break;
                case 1:
                    if (!Check_ReplaceAll.Checked)
                    {
                        if(rtb.SelectedText != null)
                        {
                            if (Tx_Replace_1.Text != "")
                            {
                                rtb.SelectedText = rtb.SelectedText.Replace(Tx_Replace_1.Text, Tx_Replace_2.Text);
                                MessageBox.Show("已替换选中目标", "提示");
                            }
                            else
                            {
                                MessageBox.Show("输入有误", "错误");
                            }
                        }
                        else
                        {
                            MessageBox.Show("未选择文本", "错误");
                        }
                    }
                    else
                    {
                        if (Tx_Replace_1.Text != "")
                        {
                            rtb.Text = rtb.Text.Replace(Tx_Replace_1.Text, Tx_Replace_2.Text);
                            MessageBox.Show("已替换全部目标", "提示");
                        }
                    }
                    break;
            }
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Check_ReplaceAll_CheckedChanged(object sender, EventArgs e)
        {
            if (Check_ReplaceAll.Checked)
            {
                Btn_Confirm.Text = "替换全部";
            }
            else
            {
                Btn_Confirm.Text = "替换选中";
            }
        }

        private void Btn_Count_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = Frm_Main.Rtxs_file[Frm_Main.target_tab];
            int count = rtb.Text.Count(i => Tx_Find.Text.Contains(i));
            MessageBox.Show("共" + count + "次匹配", "提示");
        }
    }
}
