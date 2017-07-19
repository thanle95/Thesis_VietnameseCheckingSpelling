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
        Word.Range rangeRestore = null;
        StringBuilder textRestore = new StringBuilder();
        Stopwatch watch;
        private bool isFirst = true;
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            myCustomTaskPane = Globals.ThisAddIn.CustomTaskPanes.Add(UserControl.Instance, "Chính tả");
            myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionFloating;

            myCustomTaskPane.Width = 320;
            myCustomTaskPane.Height = 600;
            myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionRight;

            myCustomTaskPane.Width = 320;
            dropTypeFindError.SelectedItemIndex = WHOLE_DOCUMENT_SELECTION;
            typeFindError = dropTypeFindError.SelectedItemIndex;
            threadStartFindError = new ThreadStart(check);


        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckError_Click(object sender, RibbonControlEventArgs e)
        {

            //Kiểm lỗi
            if (btnCheckError.Label.Equals("Kiểm lỗi"))
            {
                threadFindError = new Thread(threadStartFindError);
                threadFindError.Priority = ThreadPriority.Highest;
                threadFindError.Start();

                btnCheckError.Label = "Dừng kiểm lỗi";
                btnCheckError.ScreenTip = "Dừng kiểm lỗi";
                btnCheckError.SuperTip = "Bạn sẽ sửa lỗi sau khi dừng kiểm lỗi";
                btnCheckError.Image = global::Spell.Properties.Resources.stop;


                //Globals.ThisAddIn.Application.StatusBar = "Đang lưu trạng thái văn bản";
            }
            //Dừng kiểm lỗi
            else
                stopFindError();

        }
        private void stopFindError()
        {
            FindError.Instance.IsStopFindError = true;

            btnCheckError.Label = "Kiểm lỗi";
            btnCheckError.ScreenTip = "Kiểm lỗi";
            btnCheckError.SuperTip = "Bôi đen vùng văn bản trước khi nhấn nút để kiểm tra vùng văn bản đó\n\nHoặc để con " +
    "trỏ tại bất cứ đâu trong văn bản, hệ thống sẽ kiểm lỗi từ đó trở về sau";
            btnCheckError.Image = global::Spell.Properties.Resources.check;

            //thiết lập kiểm lỗi lần sau
            dropCorpus.Enabled = true;
            dropTypeError.Enabled = true;
            dropTypeFindError.Enabled = true;
            btnShowTaskpane.Enabled = true;
        }
        private void PrepareRestore()
        {
            //dùng để phục hồi văn bản
            int count = Globals.ThisAddIn.Application.ActiveDocument.Characters.Count;
            rangeRestore = Globals.ThisAddIn.Application.ActiveDocument.Range(0, count);
            textRestore.Append(Globals.ThisAddIn.Application.ActiveDocument.Range(0, count).Text);
            btnRestore.Enabled = false;
        }
        private void check()
        {
            PrepareForStart();

            //Stopwatch stopwatch = new Stopwatch();

            //stopwatch.Start();
            FindError.Instance.Find();

            //stopwatch.Stop();

            int count = FindError.Instance.CountError;

            if (count > 0)
            {
                ThreadStart tsPrepareRestore = new ThreadStart(PrepareRestore);
                Thread tPrepareRestore = new Thread(tsPrepareRestore);
                tPrepareRestore.Start();
                showSuggest(count);
            }
            else
                btnDeleteFormat.Enabled = false;
            stopFindError();
        }
        private void showSuggest(int count)
        {
            btnDeleteFormat.Enabled = true;
            UserControl.Instance.Start(false);
            UserControl.Instance.showCandidateInTaskPane();
        }
        private void dropDockPosition_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            if (dropDockPosition.SelectedItemIndex == DOCK_RIGHT)
                myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionRight;
            else if (dropDockPosition.SelectedItemIndex == DOCK_LEFT)
                myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionLeft;
            else
            {
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
            //if (dropTypeFindError.SelectedItemIndex == IS_TYPING_SELECTION)
            //{
            //    MessageBox.Show(SysMessage.Instance.Feature_is_updating);
            //    dropTypeFindError.SelectedItemIndex = WHOLE_DOCUMENT_SELECTION;
            //}
            //
            //if (dropTypeFindError.SelectedItemIndex == APART_DOCUMENT_SELECTION)
            //{
            //    MessageBox.Show(SysMessage.Instance.Feature_is_updating);
            //    dropTypeFindError.SelectedItemIndex = WHOLE_DOCUMENT_SELECTION;
            //}
        }

        private void btnDeleteFormat_Click(object sender, RibbonControlEventArgs e)
        {

            Word.Range selectionRange = Globals.ThisAddIn.Application.Selection.Range;
            //nếu người dùng đang chọn một vùng nào đó
            //thì bỏ gạch dưới vùng đó
            if (selectionRange.Start < selectionRange.End)
            {
                DocumentHandling.Instance.RemoveUnderline_Mistake(selectionRange.Text, selectionRange.Start, selectionRange.End);
            }
            else if (FindError.Instance.CountError > 0)
            {
                DocumentHandling.Instance.RemoveUnderline_AllMistake();
                FindError.Instance.Clear();
                //UserControl.Instance.IsFixAll = false;
            }
            myCustomTaskPane.Visible = false;
            btnDeleteFormat.Enabled = false;
            btnDeleteFormat.Enabled = false;
            btnShowTaskpane.Enabled = false;
        }

        private void btnShowTaskpane_Click(object sender, RibbonControlEventArgs e)
        {
            myCustomTaskPane.Visible = true;
        }
        private void showSumError_Click(object sender, RibbonControlEventArgs e)
        {
            int sum = 0;
            new Thread(
                 delegate ()
                 {
                     Word.Range selectedRange = Globals.ThisAddIn.Application.Selection.Range;

                     // Kiểm tra toàn bộ văn bản
                     if (selectedRange.Start == selectedRange.End)
                     {
                         foreach (Word.Range range in Globals.ThisAddIn.Application.ActiveDocument.Words)
                             if (range.Font.Color == Word.WdColor.wdColorRed)
                             {
                                 sum++;
                                 lblSumError.Label = sum.ToString() + " lỗi";
                             }
                     }

                     // Kiểm tra một phần văn bản
                     else
                     {
                         foreach (Word.Range range in Globals.ThisAddIn.Application.Selection.Words)
                             if (range.Font.Color == Word.WdColor.wdColorRed)
                             {
                                 sum++;
                                 lblSumError.Label = sum.ToString() + " lỗi";
                             }
                     }
                 }).Start();
        }

        private void btnFixAll_Click(object sender, RibbonControlEventArgs e)
        {

        }
        private Microsoft.Office.Tools.Word.GroupContentControl groupControl1;


        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            //Globals.ThisAddIn.Application.ActiveDocument.Protect(
            // Word.WdProtectionType.wdAllowOnlyReading,
            // false, System.String.Empty, false, false);

            Microsoft.Office.Tools.Word.Document vstoDocument =
                Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveDocument);


            vstoDocument.Paragraphs[1].Range.InsertParagraphBefore();

            Word.Range range1 = vstoDocument.Paragraphs[1].Range;
            range1.Text = "You cannot edit or change the formatting of text " +
                "in this sentence, because this sentence is in a GroupContentControl.";
            range1.Select();

            groupControl1 = vstoDocument.Controls.AddGroupContentControl("groupControl1");
        }

        private void button2_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveDocument).Controls.Remove("groupControl1");
        }

        private void btnShowTaskPane_Click_1(object sender, RibbonControlEventArgs e)
        {
            //Là nút sửa tất cả
            if (btnShowTaskpane.Label.Equals("Sửa tất cả"))
            {
                btnShowTaskpane.Label = "Sửa từng lỗi";
                btnShowTaskpane.ScreenTip = "Hiện Task Pane để sửa từng lỗi có trong văn bản";
                btnShowTaskpane.Image = Properties.Resources.change;
                if (FindError.Instance.CountError > 0)
                {
                    myCustomTaskPane.Visible = true;
                    UserControl.Instance.Start(true);
                    Thread threadChangeAll;
                    ThreadStart threadStartChangeAll;
                    threadStartChangeAll = new ThreadStart(UserControl.Instance.showCandidateInTaskPane);
                    threadChangeAll = new Thread(threadStartChangeAll);
                    threadChangeAll.Start();
                }
                else
                {
                    myCustomTaskPane.Visible = false;
                    btnDeleteFormat.Enabled = false;
                    btnShowTaskpane.Enabled = false;
                }
                btnRestore.Enabled = true;
            }
            else
            {
                btnRestore.Enabled = false;
                btnShowTaskpane.Label = "Sửa tất cả";
                btnShowTaskpane.ScreenTip = "Hiện Task Pane để sửa tất cả lỗi có trong văn bản bằng gợi ý tốt nhất được chọn";
                btnShowTaskpane.Image = Properties.Resources.change_all;
                if (FindError.Instance.dictContext_ErrorRange.Count > 0)
                {
                    showSuggest(FindError.Instance.CountError);
                    //FindError.Instance.FirstError_Context = FindError.Instance.lstErrorRange.First().Key;
                    //FindError.Instance.lstErrorRange[FindError.Instance.FirstError_Context].Select();
                    //UserControl.Instance.showCandidateInTaskPane();
                }
                else {
                    myCustomTaskPane.Visible = false;
                    btnDeleteFormat.Enabled = false;
                    btnShowTaskpane.Enabled = false;
                }

            }
        }

        private void btnRestore_Click(object sender, RibbonControlEventArgs e)
        {
            PrepareForStart();
            rangeRestore.Text = textRestore.ToString();
            rangeRestore.Select();
            DocumentHandling.Instance.RemoveUnderline_AllMistake();
        }
        private void PrepareForStart()
        {
            typeFindError = dropTypeFindError.SelectedItemIndex;
            typeError = dropTypeError.SelectedItemIndex;
            FindError.Instance.createValue(typeFindError, typeError);

            btnDeleteFormat.Enabled = false;
            btnShowTaskpane.Enabled = false;
            if (!btnShowTaskpane.Label.Equals("Sửa tất cả"))
            {
                btnShowTaskpane.Label = "Sửa tất cả";
                btnShowTaskpane.ScreenTip = "Hiện Task Pane để sửa tất cả lỗi có trong văn bản bằng gợi ý tốt nhất được chọn";
                btnShowTaskpane.Image = Properties.Resources.change_all;
            }
            dropCorpus.Enabled = false;
            dropTypeError.Enabled = false;
            dropTypeFindError.Enabled = false;

            UserControl.Instance.Clear();
            myCustomTaskPane.Visible = false;


            if (isFirst)
            {
                isFirst = false;
                btnDeleteCheckedRange.Enabled = true;
            }
            else
                DocumentHandling.Instance.RemoveUnderline_AllMistake();
        }

        private void btnDeleteCheckedRange_Click(object sender, RibbonControlEventArgs e)
        {
            Word.Range selectionRange = Globals.ThisAddIn.Application.Selection.Range;
            //nếu người dùng đang chọn một vùng nào đó
            //thì bỏ gạch dưới vùng đó
            if (selectionRange.Start < selectionRange.End)
            {
                DocumentHandling.Instance.RemoveHighlighChecked(selectionRange.Start, selectionRange.End);
            }
            else
                DocumentHandling.Instance.RemoveHighlighChecked();
        }
    }
}
