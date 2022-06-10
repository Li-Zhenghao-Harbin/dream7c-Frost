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
    public partial class Frm_About : Form
    {
        public Frm_About()
        {
            InitializeComponent();
        }

        private void Btn_Confirm_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
