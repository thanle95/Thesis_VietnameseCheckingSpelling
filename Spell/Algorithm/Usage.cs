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

namespace Spell.Algorithm
{
    public partial class Usage : Form
    {
        public Usage()
        {
            InitializeComponent();
            if (File.ReadAllText(FileManager.Instance.ShowAgain).Equals("1"))
            {
                chkbNoShowAgain.Checked = true;
            }
        }

        private void Usage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (chkbNoShowAgain.Checked)
                File.WriteAllText(FileManager.Instance.ShowAgain, "1");
            else
                File.WriteAllText(FileManager.Instance.ShowAgain, "0");
        }

        private void btnGotIt_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
