using System;
using System.Linq;
using System.Windows.Forms;
using Spell.Algorithm;
using Word = Microsoft.Office.Interop.Word;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Generic;

namespace Spell
{
    public partial class UserControl : System.Windows.Forms.UserControl
    {
        // Ngữ cảnh trước khi sửa lỗi
        private string _oldContextString = "",
            // Ngữ cảnh sau sửa lỗi
            _newContextString = "";

        // Cờ đánh dấu đang ở chế độ tự động sửa lỗi
        private bool _IsFixAll { get; set; }

        // Lỗi dư khoảng trắng
        private const string _ERROR_SPACE = "\"Lỗi dư khoảng trắng\"";

        // Dòng hiện tại đang chọn trong gridlog
        private int _SelectedRowGridLog { get; set; }

        // Cờ đánh dấu sửa hết lỗi
        private bool _IsOutOfError { get; set; }

        // Range hiện tại đang sửa lỗi
        private Word.Range _curRange = null;

        // Lỗi hiện tại đang sửa
        private string _ErrorString { get; set; }

        // Thể hiện của UserControl khi dùng design pattern Singleton
        private static UserControl _instance = new UserControl();

        // Cờ đánh dấu có tiếp tục sửa lỗi hay không
        private bool _IsPause { get; set; }

        // Gán nhãn cho lblWrongContext
        public static string WRONG_TEXT
        {
            get
            {
                return "\"Từ sai\"";
            }
        }
        private UserControl()
        {
            InitializeComponent();
        }
        public static UserControl Instance
        {
            get
            {
                return _instance;
            }
        }
        /// <summary>
        /// Khởi tạo giao diện UserControl
        /// </summary>
        /// <param name="isFixAll"></param>
        public void Start(bool isFixAll)
        {
            _IsFixAll = isFixAll;
            if (_IsFixAll)
            {
                changeUI_IsAutoFix();
                _IsPause = false;
            }
            else {
                changeUI_IsManuallyFix();
                _IsPause = true;
            }
        }
        /// <summary>
        /// Xóa gridlog
        /// </summary>
        public void Clear()
        {
            SynchronizedInvoke(gridLog, delegate ()
            {
                gridLog.Rows.Clear();
                gridLog.Size = new System.Drawing.Size(282, 42);
            });
        }

        /// <summary>
        /// hiện gợi ý sữa lỗi lên taskpane, tự duyệt ngữ cảnh
        /// </summary>
        public void showCandidateInTaskPane(Word.Words words, Word.Sentences sentences)
        {
            FixError fixError = new FixError();
            FindError.Instance.GetSeletedContext(words, sentences);
            fixError.getCandidatesWithContext(FindError.Instance.SelectedError_Context, FindError.Instance.lstErrorRange);

            if (fixError.hSetCandidate.Count > 0)
            {
                _oldContextString = FindError.Instance.ToString().Trim();
                _newContextString = fixError.ToString().Trim();
                lblWrong.Text = fixError.Token;
                lstbCandidate.Items.Clear();
                foreach (string item in fixError.hSetCandidate)
                {
                    if (!item.ToLower().Equals(fixError.Token.ToLower()))
                        if (item.Length > 1)
                            lstbCandidate.Items.Add(item.Trim());
                    if (lstbCandidate.Items.Count > 0)
                        lstbCandidate.SetSelected(0, true);
                    btnChange.Focus();
                }
            }
            else
            {
                //MessageBox.Show(SysMessage.Instance.IsNotError(FindError.Instance.SelectedError_Context.TOKEN));
            }
        }
        public void showCandidateInTaskPane()
        {
            FindError.Instance.lstErrorRange[FindError.Instance.FirstError_Context].Select();
            while (FindError.Instance.lstErrorRange.Count > 0)
            {
                _IsOutOfError = false;
                FixError fixError = new FixError();

                fixError.getCandidatesWithContext(FindError.Instance.FirstError_Context, FindError.Instance.lstErrorRange);
                _curRange = FindError.Instance.lstErrorRange[FindError.Instance.FirstError_Context];
                _ErrorString = fixError.Token;
                //MessageBox.Show(string.Format("\"{0}\"-\"{1}\"", range.Text, fixError.Token));
                _oldContextString = FindError.Instance.ToString().Trim();
                _newContextString = fixError.ToString().Trim();
                if (!_IsPause)
                {
                    Globals.ThisAddIn.CustomTaskPanes[0].Visible = true;
                    change(fixError.Token.ToLower(), fixError.hSetCandidate.ElementAt(0), false);
                    //CurRannge.Select();
                }
                else
                {
                    SynchronizedInvoke(lblWrong, delegate ()
                    {
                        lblWrong.Text = fixError.Token;
                    });
                    
                    Globals.ThisAddIn.CustomTaskPanes[0].Visible = true;
                    SynchronizedInvoke(lstbCandidate, delegate () { lstbCandidate.Items.Clear(); });

                    foreach (string item in fixError.hSetCandidate)
                        if (!item.ToLower().Equals(fixError.Token.ToLower()))
                            if (item.Length > 1)
                                SynchronizedInvoke(lstbCandidate, delegate () { lstbCandidate.Items.Add(item.Trim()); });
                    if (lstbCandidate.Items.Count > 0)
                        SynchronizedInvoke(lstbCandidate, delegate () { lstbCandidate.SetSelected(0, true); });
                    SynchronizedInvoke(btnChange, delegate () { btnChange.Focus(); });
                    return;
                }

            }
        }

        public void SynchronizedInvoke(ISynchronizeInvoke sync, Action action)
        {
            // If the invoke is not required, then invoke here and get out.
            if (!sync.InvokeRequired)
            {
                // Execute action.
                action();

                // Get out.
                return;
            }

            // Marshal to the required context.
            sync.Invoke(action, new object[] { });
        }
        /// <summary>
        /// HighLight tất cả những lỗi mà không hiện gợi ý
        /// </summary>
        /// <summary>
        /// HighLight những lỗi hiện tại và đưa vào danh sách lỗi
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>

        // button Ignore
        // unhighlight all word 
        private void btnIgnore_Click(object sender, EventArgs e)
        {
            ignore(); // Goi phuong thuc ignore de thuc hien deHighlight
        }

        private void ignore()
        {
            int startIndex = 0;
            int endIndex = 0;
            string wrong = lblWrong.Text.Trim().ToLower();
            //2 trường hợp bỏ qua
            //khi người dùng không chỉnh sửa, tức range.text không đổi, dùng selection.words.first
            Word.Range range = Globals.ThisAddIn.Application.Selection.Words.First;
            string text = range.Text.Trim().ToLower();
            if (text.Equals(wrong))
            {
                foreach (var item in FindError.Instance.lstErrorRange)
                {
                    if (range.Start == item.Value.Start)
                    {
                        FindError.Instance.lstErrorRange.Remove(item.Key);
                        FindError.Instance.lstError.Remove(item.Key);
                        startIndex = item.Value.Start;
                        endIndex = item.Value.End;
                        DocumentHandling.Instance.RemoveUnderline_Mistake(item.Value.Text, startIndex, endIndex);
                        break;
                    }
                }
                //không tìm thấy
            }
            //khi người dùng chỉnh sửa, kiểm tra trong những range.value.text của lstErrorRange
            //nếu có range không bằng với phần tử trong lstError
            //thì bỏ qua lỗi tại đó
            else
            {
                string iErrorRange;
                string iError;
                for (int i = 0; i < FindError.Instance.CountError; i++)
                {
                    var item = FindError.Instance.lstErrorRange.ElementAt(i);
                    iErrorRange = FindError.Instance.lstErrorRange.ElementAt(i).Value.Text;
                    iError = FindError.Instance.lstError.ElementAt(i).Value;
                    if (iErrorRange == null || !iErrorRange.Equals(iError))
                    {
                        FindError.Instance.lstErrorRange.Remove(item.Key);
                        FindError.Instance.lstError.Remove(item.Key);
                        startIndex = item.Value.Start;
                        endIndex = item.Value.End;
                        DocumentHandling.Instance.RemoveUnderline_Mistake(startIndex, endIndex);
                        break;
                    }

                }
            }
            CheckOutOfError_ShowCandidateNextTime();
        }

        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    ignore();
        //    Ngram.Instance.addToDictionary(lblWrong.Text, null, null, Position.X);
        //}

        public void startFixError(Word.Words words, Word.Sentences sentences)
        {
            showCandidateInTaskPane(words, sentences);
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            change(lblWrong.Text.ToLower(), lstbCandidate.SelectedItem.ToString(), false);
        }
        private void lstbCandidate_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            change(lblWrong.Text.ToLower(), lstbCandidate.SelectedItem.ToString(), false);
        }
        public void change(string wrongText, string fixText, bool isRightClick)
        {
            //if (IsFixAll)
            //{
            //    Context context = new Context();
            //    context.getContext();

            //    FixError fixError = new FixError();

            //    fixError.getCandidatesWithContext(context, FindError.Instance.lstErrorRange);
            //    Word.Range range = null;
            //    foreach (var pair in FindError.Instance.lstErrorRange)
            //        if (pair.Key.Equals(context))
            //        {
            //            range = pair.Value;
            //            range.Select();
            //            break;
            //        }

            //    //range.Select();

            //    _oldString = context.ToString();
            //    _newString = fixError.ToString().Trim();
            //}


            int startIndex = 0;
            int endIndex = 0;
            if (lblWrong.Text.Equals(_ERROR_SPACE))
                wrongText = " ";
            bool isMajuscule = false;
            foreach (Word.Range range in FindError.Instance.lstErrorRange.Values)
                if (range.Text.ToLower().Equals(wrongText))
                {
                    if (!range.Text.Equals(wrongText))
                        isMajuscule = true;
                    startIndex = range.Start;
                    _curRange = range;
                    var item = FindError.Instance.lstErrorRange.First(kvp => kvp.Value == range);

                    FindError.Instance.lstErrorRange.Remove(item.Key);
                    FindError.Instance.lstError.Remove(item.Key);
                    break;
                }

            if (txtManualFix.Text.Trim().Length > 0)
                fixText = txtManualFix.Text;
            if (isMajuscule)
                _curRange.Text = fixText[0].ToString().ToUpper() + fixText.Substring(1);
            else _curRange.Text = fixText;

            endIndex = startIndex + _curRange.Text.Length;
            if (!_IsFixAll)
            {
                lblWrong.Text = "\"Từ sai\"";
                lstbCandidate.Items.Clear();
            }
            DocumentHandling.Instance.RemoveUnderline_Mistake(_curRange.Text, startIndex, endIndex);
            _curRange.Select();

            //Lấy ngữ cảnh mới sau khi sửa lỗi
            Context context = new Context();
            context.getContext();
            _newContextString = context.ToString();

            addRowGridLog();
            //Index++;
            //UpdateProgressBar();
            CheckOutOfError_ShowCandidateNextTime();

            txtManualFix.Text = "";
        }
        private void CheckOutOfError_ShowCandidateNextTime()
        {
            if (/*Index == TotalError || */FindError.Instance.CountError == 0)
            {
                _IsOutOfError = true;
                Thread.Sleep(500);
                //MessageBox.Show(SysMessage.Instance.No_error);
                changeUI_OutOfError();
                //Index = 0;
                if (gridLog.RowCount == 0)
                    Globals.ThisAddIn.CustomTaskPanes[0].Visible = false;
                return;
            }
            //
            //sửa lỗi tiếp theo
            //
            FindError.Instance.FirstError_Context = FindError.Instance.lstErrorRange.First().Key;
            FindError.Instance.lstErrorRange[FindError.Instance.FirstError_Context].Select();
            if (!_IsFixAll)
                showCandidateInTaskPane();

        }
        private void btnChange_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                change(lblWrong.Text.ToLower(), lstbCandidate.SelectedItem.ToString(), false);
        }

        private void lstbCandidate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnChange.Focus();
                change(lblWrong.Text.ToLower(), lstbCandidate.SelectedItem.ToString(), false);
            }
        }

        private void showMoreInfoContext()
        {
            SynchronizedInvoke(gridLog, delegate ()
            {
                changeUI_ShowMoreInfo();

            });
            DataGridViewRow row = gridLog.CurrentRow;
            _SelectedRowGridLog = row.Index;
            SynchronizedInvoke(lblWrongContext, delegate ()
            {
                lblWrongContext.Text = row.Cells[1].Value.ToString();
            });
            SynchronizedInvoke(lblRightContext, delegate ()
            {
                lblRightContext.Text = row.Cells[2].Value.ToString();
            });
        }


        private void btnGo_Click(object sender, EventArgs e)
        {
            Word.Document oWordDoc = Globals.ThisAddIn.Application.ActiveDocument;
            Word.Range rng = oWordDoc.Content;
            rng.Find.ClearFormatting();
            object findText = "";
            bool isFound = false;
            int min = -1;
            object oTrue = true;
            object oFalse = false;
            object oFindStop = Word.WdFindWrap.wdFindStop;
            string rangeText = "";
            SynchronizedInvoke(lblRightContext, delegate ()
            {
                findText = lblRightContext.Text;
                rng.Find.Execute(ref findText, ref oTrue, ref oFalse, ref oTrue,
                        ref oFalse, ref oFalse, ref oTrue, ref oFindStop, ref oFalse,
                        null, null, null, null, null, null);
                rangeText = rng.Text;
                if (rangeText.Equals(findText))
                    isFound = true;
            });
            //nếu không tìm thấy được lblRightContext
            //tìm rightContext gần giống với lblRightContext nhất
            if (!isFound)
            {
                bool isInitial = true;
                int distance;
                int rowIndex = _SelectedRowGridLog;
                DataGridViewRow rowNext = null;
                foreach (DataGridViewRow row in gridLog.Rows)
                {
                    if (row.Index != _SelectedRowGridLog)
                    {
                        if (isInitial)
                        {
                            min = Levenshtein.Instance.calDistance(row.Cells[2].Value.ToString(), findText.ToString());
                            rowIndex = row.Index;
                            isInitial = false;
                            continue;
                        }
                        distance = Levenshtein.Instance.calDistance(row.Cells[2].Value.ToString(), findText.ToString());
                        if (distance <= min)
                        {
                            min = distance;
                            rowIndex = row.Index;
                        }
                    }
                }
                ////dùng kết quả sửa lỗi cuối cùng làm chuỗi tìm kiếm
                findText = gridLog.Rows[rowIndex].Cells[2].Value.ToString();
                rng.Find.Execute(ref findText, ref oTrue, ref oFalse, ref oTrue,
                            ref oFalse, ref oFalse, ref oTrue, ref oFindStop, ref oFalse,
                            null, null, null, null, null, null);
                rangeText = rng.Text;
                if (rangeText.Equals(findText))
                    isFound = true;
                if (!isFound)
                    MessageBox.Show(SysMessage.Instance.NoFound);
            }

            rng.Select();

        }

        private void changeUI_IsAutoFix()
        {
            SynchronizedInvoke(pnlSequenceFix, delegate () { pnlSequenceFix.Visible = false; });
            SynchronizedInvoke(pnlAutoFix, delegate () { pnlAutoFix.Location = new System.Drawing.Point(14, 5); });
            SynchronizedInvoke(gridLog, delegate () { gridLog.Location = new System.Drawing.Point(0, 90); });
            SynchronizedInvoke(pnlButtonAutoFix, delegate ()
            {
                pnlButtonAutoFix.Location = new System.Drawing.Point(0, 0);
                pnlButtonAutoFix.Visible = true;
            });
            //SynchronizedInvoke(pnlProgressBar, delegate ()
            //{
            //    pnlProgressBar.Size = new System.Drawing.Size(200, 90);
            //    pnlProgressBar.Visible = true;
            //});
            SynchronizedInvoke(pnlShowMore, delegate () { pnlShowMore.Visible = false; });
        }
        //Sửa lỗi tuần tự
        private void changeUI_IsManuallyFix()
        {
            SynchronizedInvoke(pnlAutoFix, delegate ()
            {
                pnlAutoFix.Location = new System.Drawing.Point(14, 194);
                pnlAutoFix.Visible = false;
            });
            SynchronizedInvoke(pnlSequenceFix, delegate () { pnlSequenceFix.Visible = true; });
            SynchronizedInvoke(pnlShowMore, delegate () { pnlShowMore.Visible = false; });
            SynchronizedInvoke(pnlButtonAutoFix, delegate () { pnlButtonAutoFix.Visible = false; });
            SynchronizedInvoke(gridLog, delegate ()
            {
                gridLog.Visible = false;
                gridLog.Location = new System.Drawing.Point(0, 0);
            });

        }
        private void changeUI_OutOfError()
        {
            //_isFixAll = true;
            SynchronizedInvoke(pnlSequenceFix, delegate () { pnlSequenceFix.Visible = false; });
            SynchronizedInvoke(pnlShowMore, delegate () { pnlShowMore.Visible = false; });
            SynchronizedInvoke(pnlButtonAutoFix, delegate () { pnlButtonAutoFix.Visible = false; });
            //SynchronizedInvoke(pnlProgressBar, delegate () { pnlProgressBar.Visible = false; });
            SynchronizedInvoke(pnlAutoFix, delegate ()
            {
                int min = 5;
                int delta = pnlAutoFix.Location.Y - min;
                for (int i = pnlAutoFix.Location.Y; i > min; i--)
                {
                    pnlAutoFix.Location = new System.Drawing.Point(14, i);
                    if (pnlAutoFix.Location.Y < (delta / 7 + min))
                        ;
                    if (pnlAutoFix.Location.Y < (delta * 3 / 7 + min))
                        i--;
                    else i = i - 2;
                    Thread.Sleep(7);
                }
            });
            SynchronizedInvoke(gridLog, delegate ()
            {
                for (int i = gridLog.Location.Y; i >= 0; i--)
                {
                    gridLog.Location = new System.Drawing.Point(0, i);
                    Thread.Sleep(1);
                }
            });
        }


        private void addRowGridLog()
        {
            //SynchronizedInvoke(pnlProgressBar, delegate ()
            //{
            //    if (!IsFixAll)
            //        if (pnlProgressBar.Size.Width != 285)
            //        {
            //            pnlProgressBar.Size = new System.Drawing.Size(285, 90);
            //            progressBar1.Width = 275;
            //            pnlProgressBar.Visible = true;
            //        }
            //});
            SynchronizedInvoke(pnlShowMore, delegate ()
            {
                pnlShowMore.Visible = false;
            });
            SynchronizedInvoke(pnlAutoFix, delegate ()
            {
                pnlAutoFix.Visible = true;
            });

            SynchronizedInvoke(gridLog, delegate ()
            {
                gridLog.Visible = true;
                if (_IsFixAll)
                {
                    if (gridLog.Size.Height <= 310)
                        gridLog.Size = new System.Drawing.Size(gridLog.Size.Width, gridLog.Size.Height + 20);
                }
                else if (gridLog.Size.Height <= 250)
                {
                    int delta = gridLog.Location.Y;
                    gridLog.Size = new System.Drawing.Size(gridLog.Size.Width, gridLog.Size.Height + 20);
                    for (int i = gridLog.Location.Y; i >= 0; i--)
                    {
                        gridLog.Location = new System.Drawing.Point(0, i);
                        if (pnlAutoFix.Location.Y < (delta / 7))
                            ;
                        if (pnlAutoFix.Location.Y < (delta * 3 / 7))
                            i--;
                        else i = i - 2;
                        Thread.Sleep(5);
                    }
                    gridLog.Location = new System.Drawing.Point(0, 0);
                }
                gridLog.Rows.Add(gridLog.RowCount + 1, _oldContextString, _newContextString);
                //
                //scroll gridlog đến lỗi cuối cùng
                scrollGridLog();
            });

        }
        private void scrollGridLog()
        {
            int rowVisible = gridLog.DisplayedRowCount(true);
            if (gridLog.FirstDisplayedScrollingRowIndex + rowVisible < gridLog.Rows.Count)
                gridLog.FirstDisplayedScrollingRowIndex++;
            else gridLog.FirstDisplayedScrollingRowIndex = 0;
        }
        private void btnPauseResumeAutoFix_Click(object sender, EventArgs e)
        {
            Pause_Resume();
        }
        private void Pause_Resume()
        {
            SynchronizedInvoke(lblPauseResumeAutoFix, delegate ()
            {
                if (lblPauseResumeAutoFix.Text.Contains("dừng"))
                {
                    _IsPause = true;
                    lblPauseResumeAutoFix.Text = "Tiếp tục";
                    SynchronizedInvoke(btnPauseResumeAutoFix, delegate ()
                    {
                        btnPauseResumeAutoFix.BackgroundImage = global::Spell.Properties.Resources.change;
                    });
                }
                else
                {
                    SynchronizedInvoke(pnlShowMore, delegate ()
                    {
                        pnlShowMore.Visible = false;
                    });
                    SynchronizedInvoke(gridLog, delegate ()
                    {
                        for (int i = gridLog.Location.Y; i >= 90; i--)
                        {
                            gridLog.Location = new System.Drawing.Point(0, i);
                            Thread.Sleep(5);
                        }
                    });

                    _IsPause = false;
                    lblPauseResumeAutoFix.Text = "Tạm dừng";
                    SynchronizedInvoke(btnPauseResumeAutoFix, delegate ()
                    {
                        btnPauseResumeAutoFix.BackgroundImage = global::Spell.Properties.Resources.pause;
                    });
                    Thread t = new Thread(new ThreadStart(showCandidateInTaskPane));
                    t.Start();
                }
            });
        }

        private void lblPauseResumeAutoFix_Click(object sender, EventArgs e)
        {
            Pause_Resume();
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            string text = _curRange.Text;
            if (text != null && text.Equals(_ErrorString))
            {
                _curRange.Select();
            }
            else {
                ignore();
                btnResume.Visible = false;
            }
        }

        private void gridLog_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            showMoreInfoContext();
        }

        //private void gridLog_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    var cell = gridLog.Rows[e.RowIndex].Cells[e.ColumnIndex];
        //    cell.ToolTipText = cell.Value.ToString();

        //}
        private void gridLog_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            showMoreInfoContext();
        }
        private void btnCloseBtnShowMore_Click(object sender, EventArgs e)
        {
            SynchronizedInvoke(pnlShowMore, delegate ()
            {
                pnlShowMore.Visible = false;
            });
            SynchronizedInvoke(gridLog, delegate ()
            {
                gridLog.Visible = true;
                int min = 0;
                if (_IsPause && _IsFixAll)
                    min = 90;
                for (int i = gridLog.Location.Y; i >= min; i--)
                {
                    gridLog.Location = new System.Drawing.Point(0, i);
                    Thread.Sleep(5);
                }
            });
        }



        private void changeUI_ShowMoreInfo()
        {
            int yGridLog, yShowMore;
            if (_IsOutOfError)
            {
                yGridLog = 60;
                yShowMore = 0;
            }
            else if (_IsFixAll)
            {
                yGridLog = 150;
                yShowMore = 90;
            }
            else
            {
                yGridLog = 60;
                yShowMore = 0;
            }
            SynchronizedInvoke(gridLog, delegate ()
            {
                int delta = yGridLog - gridLog.Location.Y;
                for (int i = gridLog.Location.Y; i <= yGridLog; i++)
                {
                    gridLog.Location = new System.Drawing.Point(0, i);
                    Thread.Sleep(5);
                }
            });
            SynchronizedInvoke(pnlShowMore, delegate ()
            {
                pnlShowMore.Location = new System.Drawing.Point(0, yShowMore);
                pnlShowMore.Visible = true;
            });
        }
        //private async Task ProcessData(IProgress<ProgressReport> progress)
        //{
        //    var progressReport = new ProgressReport();
        //    await Task.Run(() =>
        //    {
        //        progressReport.PercentComplete = Index * 100 / TotalError;
        //        progress.Report(progressReport);
        //        Thread.Sleep(20);
        //    });
        //}
        //private async void UpdateProgressBar()
        //{
        //    var progress = new Progress<ProgressReport>();

        //    progress.ProgressChanged += (o, report) =>
        //    {
        //        SynchronizedInvoke(lblStatus, delegate ()
        //        {
        //            lblStatus.Text = string.Format("{0}/{1} lỗi", Index, TotalError);
        //        });
        //        SynchronizedInvoke(progressBar1, delegate ()
        //        {
        //            progressBar1.Value = report.PercentComplete;
        //            progressBar1.Update();
        //        });
        //    };
        //    await ProcessData(progress);
        //    //lblStatus.Text = string.Format("{0}/{1} lỗi", Index, TotalError);

        //}

    }
}