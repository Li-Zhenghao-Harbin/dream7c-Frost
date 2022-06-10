using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace 柒幻_霜降
{
    public partial class Frm_Rename : Form
    {
        public Frm_Rename()
        {
            InitializeComponent();
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Btn_Confirm_Click(object sender, EventArgs e)
        {
            try
            {
                string previous_file = Frm_Main.full_name[Frm_Main.treeview_selected_index];
                string new_file = Frm_Main.folder_path + "\\" + Tx_New_Name.Text + ".txt";
                File.Move(previous_file, new_file);
            }
            catch
            {
                MessageBox.Show("命名重复或不合法", "错误");
            }
            Close();
        }

        private void Frm_Rename_Load(object sender, EventArgs e)
        {
            Tx_New_Name.Text = Frm_Main.current_file_name;
            Tx_New_Name.SelectAll();
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
