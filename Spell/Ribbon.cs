using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Office = Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;
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

        private const int DOCK_RIGHT = 0;
        private const int DOCK_LEFT = 1;
        private const int IS_TYPING_SELECTION = 0;
        private const int WHOLE_DOCUMENT_SELECTION = 1;
        Thread threadFindError;
        ThreadStart threadStartFindError;
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            myCustomTaskPane = Globals.ThisAddIn.CustomTaskPanes.Add(UserControl.Instance, "Spelling");
            myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionFloating;

            myCustomTaskPane.Width = 320;
            myCustomTaskPane.Height = 600;
            myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionRight;
            myCustomTaskPane.Width = 320;
            dropTypeFindError.SelectedItemIndex = WHOLE_DOCUMENT_SELECTION;
            typeFindError = dropTypeFindError.SelectedItemIndex;
            //Ngram.Instance.runFirst();
            FindError.Instance.createValue(typeFindError, typeError);
            threadStartFindError = new ThreadStart(check);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckError_Click(object sender, RibbonControlEventArgs e)
        {
            //if (Properties.Resources.ShowAgain.Equals("0"))
            //{
            //    Usage usage = new Usage();
            //    usage.ShowDialog();
            //}
            if (btnCheckError.Label.Equals("Kiểm lỗi"))
            {
                threadFindError = new Thread(threadStartFindError);
                threadFindError.Priority = ThreadPriority.Highest;
                //if (!threadFindError.IsAlive)
                threadFindError.Start();
                btnCheckError.Label = "Tạm dừng";
                btnCheckError.Image = global::Spell.Properties.Resources.pause;
            }
            else if (btnCheckError.Label.Equals("Tạm dừng"))
            {
                threadFindError.Suspend();
                tbtnShowTaskpane.Enabled = true;
                btnDeleteFormat.Enabled = true;
                btnCheckError.Label = "Tiếp tục";
                btnCheckError.Image = global::Spell.Properties.Resources.check;
            }
            else
            {
                threadFindError.Resume();
                tbtnShowTaskpane.Enabled = false;
                btnCheckError.Label = "Tạm dừng";
                btnCheckError.Image = global::Spell.Properties.Resources.pause;
            }
        }
        private void check()
        {
            DocumentHandling.Instance.DeHighLight_All_Mistake(Globals.ThisAddIn.Application.ActiveDocument.Characters);

            typeFindError = dropTypeFindError.SelectedItemIndex;
            typeError = dropTypeError.SelectedItemIndex;
            btnDeleteFormat.Enabled = false;
            btnStop.Enabled = true;
            dropCorpus.Enabled = false;
            dropTypeError.Enabled = false;
            dropTypeFindError.Enabled = false;
            FindError.Instance.StopFindError = false;
            tbtnShowTaskpane.Enabled = false;

            FindError.Instance.createValue(typeFindError, typeError);
            UserControl.Instance.grigLogCount = 0;
            myCustomTaskPane.Visible = false;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            FindError.Instance.startFindError();
            stopwatch.Stop();
            int count = FindError.Instance.CountError;
            if (count > 0)
                showSuggest(count);
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
            btnStop.Enabled = false;
            dropCorpus.Enabled = true;
            dropTypeError.Enabled = true;
            dropTypeFindError.Enabled = true;
        }
        private void showSuggest(int count)
        {
            btnDeleteFormat.Enabled = true;
            string message = SysMessage.Instance.Message_Notify_Fix_Error(count);
            string caption = SysMessage.Instance.Caption_Notify_Fix_Error;
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            // Displays the MessageBox.

            result = MessageBox.Show(message, caption, buttons);
            //UserControl.Instance.changeUIStart();
            myCustomTaskPane.Visible = true;
            if (result == DialogResult.Yes)
            {
                UserControl.Instance.Start(true);
                UserControl.Instance.showCandidateInTaskPane();
            }
            else {
                UserControl.Instance.Start(false);
                UserControl.Instance.showCandidateInTaskPane();
            }
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
            if (dropTypeFindError.SelectedItemIndex == IS_TYPING_SELECTION)
            {
                MessageBox.Show(SysMessage.Instance.Feature_is_updating);
                dropTypeFindError.SelectedItemIndex = WHOLE_DOCUMENT_SELECTION;
            }
            //
            //if (dropTypeFindError.SelectedItemIndex == APART_DOCUMENT_SELECTION)
            //{
            //    MessageBox.Show(SysMessage.Instance.Feature_is_updating);
            //    dropTypeFindError.SelectedItemIndex = WHOLE_DOCUMENT_SELECTION;
            //}
        }

        private void btnDeleteFormat_Click(object sender, RibbonControlEventArgs e)
        {
            string message = SysMessage.Instance.Message_Notify_Delete_Format;
            string caption = SysMessage.Instance.Caption_Notify_Fix_Error;
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            // Displays the MessageBox.

            result = MessageBox.Show(message, caption, buttons);
            //UserControl.Instance.changeUIStart();
            myCustomTaskPane.Visible = true;
            if (result == DialogResult.Yes)
            {
                DocumentHandling.Instance.DeHighLight_All_Mistake(Globals.ThisAddIn.Application.ActiveDocument.Characters);
                FindError.Instance.Clear();
                UserControl.Instance._isFixAll = false;
                myCustomTaskPane.Visible = false;

                btnDeleteFormat.Enabled = false;
            }
           
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
                showSuggest(FindError.Instance.CountError);
                //FindError.Instance.FirstError_Context = FindError.Instance.lstErrorRange.First().Key;
                //FindError.Instance.lstErrorRange[FindError.Instance.FirstError_Context].Select();
                //UserControl.Instance.showCandidateInTaskPane();
            }
            else
                myCustomTaskPane.Visible = false;
        }

        private void btnStop_Click(object sender, RibbonControlEventArgs e)
        {
            if (btnCheckError.Label.Equals("Tiếp tục"))
            {
                threadFindError.Resume();
            }
            FindError.Instance.StopFindError = true;
            btnStop.Enabled = false;
            tbtnShowTaskpane.Enabled = true;

            btnCheckError.Label = "Kiểm lỗi";
            btnCheckError.Image = global::Spell.Properties.Resources.check;
        }

        private void showSumError_Click(object sender, RibbonControlEventArgs e)
        {
            int sum = 0;
            Thread a = new Thread(
                delegate ()
                {
                    foreach (Word.Range range in Globals.ThisAddIn.Application.ActiveDocument.Words)
                        if (range.Font.Color == Word.WdColor.wdColorRed)
                        {
                            sum++;
                            lblSumError.Label = sum.ToString() + " lỗi";
                        }
                });
            a.Start();
        }
    }
}
