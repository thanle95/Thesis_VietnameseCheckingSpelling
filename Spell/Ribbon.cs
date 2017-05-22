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
using System.Diagnostics;
using System.IO;

namespace Spell
{
    public partial class Ribbon
    {
        Microsoft.Office.Tools.CustomTaskPane myCustomTaskPane;
        private static int typeFindError = 0;
        private static int typeError = 0;
        private const int LIFE_CORPUS = 0;
        private const int POLITIC_CORPUS = 1;
        private const int LITERRATY_CORPUS = 2;

        private const int WHOLE_DOCUMENT_SELECTION = 0;
        private const int APART_DOCUMENT_SELECTION = 1;

        private const int DOCK_RIGHT = 0;
        private const int DOCK_LEFT = 1;

        private static bool isAutoChange = false;
        Thread threadFindError;
        ThreadStart threadStartFindError;
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            myCustomTaskPane = Globals.ThisAddIn.CustomTaskPanes.Add(UserControl.Instance, "Spelling");
            myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionFloating;

            myCustomTaskPane.Width = 300;
            myCustomTaskPane.Height = 350;
            myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionRight;
            myCustomTaskPane.Width = 300;
            typeFindError = dropTypeFindError.SelectedItemIndex;
            //Ngram.Instance.runFirst();
            FindError.Instance.createValue(typeFindError, typeError, isAutoChange);
            threadStartFindError = new ThreadStart(check);

         
        }

        /// <summary>
        /// Khi nút check được click
        /// Kiểm tra nếu có tick vào check box gợi ý, thì hiện task pane gợi ý bên phải
        /// còn không thì chỉ highLight những lỗi trong văn bản
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckError_Click(object sender, RibbonControlEventArgs e)
        {
            if (File.ReadAllText(FileManager.Instance.ShowAgain).Equals("0"))
            {
                Usage usage = new Usage();
                usage.ShowDialog();
            }
            threadFindError = new Thread(threadStartFindError);
            threadFindError.Priority = ThreadPriority.Highest;
            //if (!threadFindError.IsAlive)
            threadFindError.Start();
        }
        private void check()
        {
            DocumentHandling.Instance.DeHighLight_All_Mistake(Globals.ThisAddIn.Application.ActiveDocument.Characters);
            btnCheckError.Enabled = false;
            btnPauseResume.Enabled = true;
            btnDeleteFormat.Enabled = false;
            btnStop.Enabled = true;
            FindError.Instance.StopFindError = false;
            tbtnShowTaskpane.Enabled = false;

            typeFindError = dropTypeFindError.SelectedItemIndex;
            typeError = dropTypeError.SelectedItemIndex;
            isAutoChange = chkbAutoChange.Checked;

            FindError.Instance.createValue(typeFindError, typeError, isAutoChange);

            myCustomTaskPane.Visible = false;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            FindError.Instance.startFindError();
            stopwatch.Stop();

            int count = FindError.Instance.CountError;
            //int count = UserControl.Instance.startFindError(typeFindError);
            if (count > 0)
            {
                btnDeleteFormat.Enabled = true;
                string message = SysMessage.Instance.Message_Notify_Fix_Error(count);
                string caption = SysMessage.Instance.Caption_Notify_Fix_Error;
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);

                if (result == DialogResult.Yes)
                {
                    UserControl.Instance.showCandidateInTaskPaneWithCountWord();
                }
                myCustomTaskPane.Visible = true;
            }
            else
            {
                MessageBox.Show(SysMessage.Instance.No_error);
                btnDeleteFormat.Enabled = false;
            }
            TimeSpan ts = stopwatch.Elapsed;
            string elapseTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                ts.Hours, ts.Minutes, ts.Seconds,
                                ts.Milliseconds / 10);
            MessageBox.Show(elapseTime);
            btnPauseResume.Enabled = false;
            btnStop.Enabled = false;
            btnCheckError.Enabled = true;
            tbtnShowTaskpane.Enabled = true;
        }
        private void dropDockPosition_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            if (dropDockPosition.SelectedItemIndex == DOCK_RIGHT)
                myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionRight;
            else if (dropDockPosition.SelectedItemIndex == DOCK_LEFT)
                myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionLeft;
            else {
                myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionFloating;
                Office.CommandBar cb = Globals.ThisAddIn.Application.CommandBars[myCustomTaskPane.Title];
                cb.Left = 1000;
                cb.Top = 300;
            }
        }

        private void dropCorpus_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            if (dropCorpus.SelectedItemIndex == POLITIC_CORPUS)
            {
                MessageBox.Show(SysMessage.Instance.Feature_is_updating);
                dropCorpus.SelectedItemIndex = LIFE_CORPUS;
            }
            else if (dropCorpus.SelectedItemIndex == LITERRATY_CORPUS)
            {
                MessageBox.Show(SysMessage.Instance.Feature_is_updating);
                dropCorpus.SelectedItemIndex = LIFE_CORPUS;
            }
        }

        private void dropTypeFindError_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            //
            //if (dropTypeFindError.SelectedItemIndex == APART_DOCUMENT_SELECTION)
            //{
            //    MessageBox.Show(SysMessage.Instance.Feature_is_updating);
            //    dropTypeFindError.SelectedItemIndex = WHOLE_DOCUMENT_SELECTION;
            //}
        }

        private void btnDeleteFormat_Click(object sender, RibbonControlEventArgs e)
        {
            DocumentHandling.Instance.DeHighLight_All_Mistake(Globals.ThisAddIn.Application.ActiveDocument.Characters);
            btnDeleteFormat.Enabled = false;
        }

        private void btnShowTaskpane_Click(object sender, RibbonControlEventArgs e)
        {
            //string text = Globals.ThisAddIn.Application.Selection.Sentences[1].Text;
            //MessageBox.Show(text + ": " + text.Length);
            myCustomTaskPane.Visible = true;
        }

        private void tbtnShowTaskpane_Click(object sender, RibbonControlEventArgs e)
        {
            if (tbtnShowTaskpane.Checked)
            {
                myCustomTaskPane.Visible = true;
                FindError.Instance.FirstError_Context = FindError.Instance.lstErrorRange.First().Key;
                FindError.Instance.lstErrorRange[FindError.Instance.FirstError_Context].Select();
                UserControl.Instance.showCandidateInTaskPaneWithCountWord();
            }
            else
                myCustomTaskPane.Visible = false;
        }

        private void btnPauseResume_Click(object sender, RibbonControlEventArgs e)
        {
            if (btnPauseResume.Label.Equals("Tạm dừng"))
            {
                threadFindError.Suspend();
                btnPauseResume.Label = "Tiếp tục";
                tbtnShowTaskpane.Enabled = true;
            }
            else
            {
                threadFindError.Resume();
                btnPauseResume.Label = "Tạm dừng";
                tbtnShowTaskpane.Enabled = false;
            }
        }

        private void btnStop_Click(object sender, RibbonControlEventArgs e)
        {
            //threadFindError.Suspend();
            //threadFindError.Abort();
            FindError.Instance.StopFindError = true;
            btnCheckError.Enabled = true;
            btnStop.Enabled = false;
            btnPauseResume.Enabled = false;
            tbtnShowTaskpane.Enabled = true;
        }

        private void btnUsage_Click(object sender, RibbonControlEventArgs e)
        {
            Usage usage = new Usage();
            usage.ShowDialog();
        }

    }
}
