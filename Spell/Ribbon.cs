using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using System.Threading;
using System.Windows.Forms;
using Spell.Algorithm;
namespace Spell
{
    public partial class Ribbon
    {
        Microsoft.Office.Tools.CustomTaskPane myCustomTaskPane;

        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            myCustomTaskPane = Globals.ThisAddIn.CustomTaskPanes.Add(UserControl.Instance, "Spelling");
            myCustomTaskPane.DockPositionRestrict = Office.MsoCTPDockPositionRestrict.msoCTPDockPositionRestrictNoChange;
            myCustomTaskPane.Width = 300;
            //Ngram.Instance.runFirst();
        }

        /// <summary>
        /// Khi nút check được click
        /// Kiểm tra nếu có tick vào check box gợi ý, thì hiện task pane gợi ý bên phải
        /// còn không thì highLight những lỗi trong văn bản
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbtnCheck_Click(object sender, RibbonControlEventArgs e)
        {
            ThreadStart t = new ThreadStart(UserControl.Instance.showWrongWithoutSuggest);
            Thread threadWithoutSuggest = new Thread(t);
            ThreadStart t1 = new ThreadStart(UserControl.Instance.showWrongWithSuggest);
            Thread threadWithSuggest = new Thread(t1);
            //nút check được checked
            if (tbtnCheck.Checked)
                //mode1: hiện gợi ý sửa lỗi
                if (chkSuggest.Checked)
                {
                    myCustomTaskPane.Visible = true;
                    //threadWithSuggest.Start();
                }
                //mode2: không hiện gợi ý 
                else {
                    myCustomTaskPane.Visible = false;
                    threadWithSuggest.Abort();
                    //threadWithoutSuggest.Start();
                }
            else
            {
                threadWithSuggest.Abort();
                threadWithoutSuggest.Abort();
                DocumentHandling.Instance.DeHighLight_All_Mistake(Globals.ThisAddIn.Application.ActiveDocument.Characters);
            }
        }
        private void chkSuggest_Click(object sender, RibbonControlEventArgs e)
        {
            if (chkSuggest.Checked)
            {
                myCustomTaskPane.Visible = true;
                //UserControl1.Instance.showCandidates();
            }
            else {
                myCustomTaskPane.Visible = false;
                DocumentHandling.Instance.DeHighLight_All_Mistake(Globals.ThisAddIn.Application.ActiveDocument.Characters);
            }

        }
    }
}
