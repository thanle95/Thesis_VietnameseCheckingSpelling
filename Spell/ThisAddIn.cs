using Spell.Algorithm;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using System;
using Microsoft.Office.Tools.Word;
using System.IO;
using System.Collections.Generic;

namespace Spell
{

    public partial class ThisAddIn
    {
        Word.Application myApplication;
        private string TAG = "CANDIDATE";
        private Office.CommandBarButton myControl;
        private int PreSelectedRangeStart = 0;
        private string PreSelectedRangeText = "";
        private string WrongWord
        {
            get; set;
        }
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            //khởi tạo dữ liệu
            VNDictionary.getInstance.runFirst();
            Ngram.Instance.runFirst();

            myApplication = Application;
            //thêm sự kiện click chuột phải
            myApplication.WindowBeforeRightClick +=
                new Word.ApplicationEvents4_WindowBeforeRightClickEventHandler(application_WindowBeforeRightClick);
            //thêm sự kiện đổi vùng được chọn
            myApplication.WindowSelectionChange +=
                  new Word.ApplicationEvents4_WindowSelectionChangeEventHandler(ThisDocument_SelectionChange);
        }
        private void ThisDocument_SelectionChange(Word.Selection selection)
        {
            //Nếu văn bản có lỗi
            //và đã kết thúc kiểm lỗi
            if (FindError.Instance.CountError > 0 && FindError.Instance.IsStopFindError)
            {
                //Lấy từ đang chọn
                Word.Range selectedRange = DocumentHandling.Instance.GetWordByCursorSelection();
                //Trường hợp người dùng tự sửa lỗi
                //Xử lý remove underline lỗi hiện tại
                //Bằng việc so sánh với từ được chọn lần trước
                {
                    if (!selectedRange.Text.Equals(PreSelectedRangeText) && selectedRange.Start == PreSelectedRangeStart)
                        DocumentHandling.Instance.RemoveUnderline_Mistake(selectedRange.Text, selectedRange.Start, selectedRange.End);
                    PreSelectedRangeText = selectedRange.Text;
                    PreSelectedRangeStart = selectedRange.Start;
                }

                //Truy cập vào label lblWrong
                UserControl.Instance.SynchronizedInvoke(UserControl.Instance.lblWrong, delegate ()
                {
                    //Sửa lỗi hiện tại
                    if (selectedRange.Text.Trim().Equals(UserControl.Instance.lblWrong.Text))
                        EnableFixError(true);
                    else {
                        foreach (var item in FindError.Instance.dictContext_ErrorRange.Keys)
                            //Sửa lỗi bất kỳ khác
                            if (selectedRange.Text.Trim().Equals(item.TOKEN))
                            {
                                EnableFixError(true);
                                Word.Words words = Globals.ThisAddIn.Application.Selection.Words;
                                Word.Sentences sentences = Globals.ThisAddIn.Application.Selection.Sentences;
                                UserControl.Instance.startFixError(words, sentences);
                                return;
                            }
                        //Không phải là lỗi
                        EnableFixError(false);
                    }
                });
            }
        }

        private void EnableFixError(bool enable)
        {
            UserControl.Instance.SynchronizedInvoke(UserControl.Instance.lstbCandidate, delegate ()
            {
                UserControl.Instance.lstbCandidate.Enabled = enable;
            });
            UserControl.Instance.SynchronizedInvoke(UserControl.Instance.pnlTxtManualFix, delegate ()
            {
                UserControl.Instance.pnlTxtManualFix.Visible = enable;
            });
            UserControl.Instance.SynchronizedInvoke(UserControl.Instance.btnResume, delegate ()
            {
                UserControl.Instance.btnResume.Visible = !enable;
            });
        }
        /// <summary>
        /// Bỏ những item có tag là CANDIDATE trước đó
        /// </summary>
        private void RemoveExistingMenuItem()
        {
            Office.CommandBar contextMenu = myApplication.CommandBars["Text"];
            while (true)
            {
                Office.CommandBarButton control =
                    (Office.CommandBarButton)contextMenu.FindControl
                    (Office.MsoControlType.msoControlButton, missing,
                    TAG, true, true);
                if (control == null)
                    return;
                else
                    control.Delete(true);
            }

        }
        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {

        }
        /// <summary>
        /// Xử lý sự kiện chọn CANDIDATE khi chuột phải
        /// </summary>
        /// <param name="Ctrl"></param>
        /// <param name="CancelDefault"></param>
        void myControl_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            UserControl.Instance.Start(false);
            UserControl.Instance.change(WrongWord, Ctrl.Caption, true);
        }
        /// <summary>
        /// Xử lý sự kiện chuột phải
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="Cancel"></param>
        public void application_WindowBeforeRightClick(Word.Selection selection, ref bool Cancel)
        {
            //bỏ những MenuItem tồn tại trước đó
            RemoveExistingMenuItem();

            //Nếu văn bản có lỗi
            if (FindError.Instance.CountError > 0)
            {
                Word.Words words = Globals.ThisAddIn.Application.Selection.Words;

                Context context = new Context();
                context.getContext();

                //Tìm lỗi trong danh sách
                if (FindError.Instance.IsContainError(context, words.First.Start)){
                    //Sửa lỗi đã tìm được
                    FixError fixError = new FixError();
                    fixError.getCandidatesWithContext(context, FindError.Instance.dictContext_ErrorRange);
                    WrongWord = fixError.Token.ToLower();

                    //dùng List để reverse hashSet
                    List<string> candidates = new List<string>();
                    if (fixError.hSetCandidate.Count > 0)
                    {
                        foreach (string item in fixError.hSetCandidate)
                            candidates.Add(item);

                        candidates.Reverse();

                        foreach (string candidate in candidates)
                            if (!candidate.ToLower().Equals(fixError.Token.ToLower()))
                                if (candidate.Length > 1)
                                    addCandidate(candidate.Trim());
                    }
                    else;
                }
                //System.Windows.Forms.MessageBox.Show(SysMessage.Instance.IsNotError(FindError.Instance.SelectedError_Context.TOKEN));
            }
        }
        /// <summary>
        /// Thêm một candidate vào MenuItem
        /// </summary>
        /// <param name="candidate"></param>
        private void addCandidate(string candidate)
        {
            Office.MsoControlType menuItem =
                       Office.MsoControlType.msoControlButton;
            myControl =
                (Office.CommandBarButton)myApplication.CommandBars["Text"].Controls.Add
                (menuItem, missing, missing, 1, true);
            myControl.Style = Office.MsoButtonStyle.msoButtonCaption;
            myControl.Caption = candidate;
            myControl.Tag = TAG;
            myControl.Click +=
                new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler
                    (myControl_Click);
            GC.Collect();
        }
        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion

    }
}
