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
    //phục vụ cho việc thêm vào từ điển
    public enum Position { xxX, xXx, Xxx, xX, Xx, X };

    public partial class UserControl : System.Windows.Forms.UserControl
    {
        private Word.Range curRangeTextShowInTaskPane;
        private string oldString = "", newString = "";
        public bool IsFixAll { get; set; }
        public int grigLogCount { get; set; }
        private static UserControl instance = new UserControl();
        private const string ERROR_SPACE = "\"Lỗi dư khoảng trắng\"";
        private int SELECTED_ERROR { get; set; }
        private bool IsOutOfError { get; set; }
        private UserControl()
        {
            InitializeComponent();
        }
        public static UserControl Instance
        {
            get
            {
                return instance;
            }
        }
        private int Index { get; set; }
        private int TotalError { get; set; }
        public void Start(bool isFixAll)
        {
            if (TotalError == 0)
                TotalError = FindError.Instance.lstErrorRange.Count;
            IsFixAll = isFixAll;
           
            if (IsFixAll)
                changeUI_IsAutoFix();
            else
                changeUI_IsSequenceFix();
        }
        public void Clear()
        {
            TotalError = 0;
            SynchronizedInvoke(gridLog, delegate ()
            {
                gridLog.Rows.Clear();
                gridLog.Size = new System.Drawing.Size(287, 85);
            });
        }
        public static string WRONG_TEXT
        {
            get
            {
                return "\"Từ sai\"";
            }
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
                MessageBox.Show(SysMessage.Instance.IsNotError(FindError.Instance.SelectedError_Context.TOKEN));
            }
        }
        public void showCandidateInTaskPane()
        {
            while (FindError.Instance.lstErrorRange.Count > 0)
            {
                IsOutOfError = false;
                FixError fixError = new FixError();

                fixError.getCandidatesWithContext(FindError.Instance.FirstError_Context, FindError.Instance.lstErrorRange);
                Word.Range range = FindError.Instance.lstErrorRange[FindError.Instance.FirstError_Context];
                range.Select();

                oldString = FindError.Instance.ToString().Trim();
                newString = fixError.ToString().Trim();
                if (IsFixAll)
                {
                    change(fixError.Token.ToLower(), fixError.hSetCandidate.ElementAt(0), false);
                }
                else
                {
                    SynchronizedInvoke(lblWrong, delegate () { lblWrong.Text = fixError.Token; });
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

        private void SynchronizedInvoke(ISynchronizeInvoke sync, Action action)
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

            foreach (Word.Range range in FindError.Instance.lstErrorRange.Values)
                if (range.Text.Trim().ToLower().Equals(lblWrong.Text.ToLower()))
                {
                    startIndex = range.Start;
                    endIndex = range.End;
                    var item = FindError.Instance.lstErrorRange.First(kvp => kvp.Value == range);
                    FindError.Instance.lstErrorRange.Remove(item.Key);
                    break;
                }

            DocumentHandling.Instance.DeHighLight_Mistake(startIndex, endIndex);
            if (FindError.Instance.lstErrorRange.Count == 0)
            {
                MessageBox.Show(SysMessage.Instance.No_error);
                this.Visible = false;
                return;
            }

            //
            //sửa lỗi tiếp theo
            //
            FindError.Instance.FirstError_Context = FindError.Instance.lstErrorRange.First().Key;
            FindError.Instance.lstErrorRange[FindError.Instance.FirstError_Context].Select();
            showCandidateInTaskPane();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ignore();
            Ngram.Instance.addToDictionary(lblWrong.Text, null, null, Position.X);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Word.Words words = Globals.ThisAddIn.Application.Selection.Words;
            Word.Sentences sentences = Globals.ThisAddIn.Application.Selection.Sentences;

            startFixError(words, sentences);
        }
        public void startFixError(Word.Words words, Word.Sentences sentences)
        {
            showCandidateInTaskPane(words, sentences);
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            change(lblWrong.Text.ToLower(), lstbCandidate.SelectedItem.ToString(), false);
        }
        public void change(string wrongText, string fixText, bool isRightClick)
        {
            //if (isRightClick)
            //{
            //    FixError fixError = new FixError();

            //    fixError.getCandidatesWithContext(FindError.Instance.FirstError_Context, FindError.Instance.lstErrorRange);
            //    Word.Range range = FindError.Instance.lstErrorRange[FindError.Instance.FirstError_Context];
            //    range.Select();

            //    oldString = FindError.Instance.ToString().Trim();
            //    newString = fixError.ToString().Trim();
            //}
            addRowGridLog();
            int startIndex = 0;
            int endIndex = 0;
            if (lblWrong.Text.Equals(ERROR_SPACE))
                wrongText = " ";
            bool isMajuscule = false;
            foreach (Word.Range range in FindError.Instance.lstErrorRange.Values)
                if (range.Text.ToLower().Equals(wrongText))
                {
                    if (!range.Text.Equals(wrongText))
                        isMajuscule = true;
                    startIndex = range.Start;
                    curRangeTextShowInTaskPane = range;
                    var item = FindError.Instance.lstErrorRange.First(kvp => kvp.Value == range);

                    FindError.Instance.lstErrorRange.Remove(item.Key);
                    break;
                }
            if (isMajuscule)
                curRangeTextShowInTaskPane.Text = fixText[0].ToString().ToUpper() + fixText.Substring(1);
            else curRangeTextShowInTaskPane.Text = fixText;

            endIndex = startIndex + curRangeTextShowInTaskPane.Text.Length;
            if (!IsFixAll)
            {
                lblWrong.Text = "\"Từ sai\"";
                lstbCandidate.Items.Clear();
            }
            DocumentHandling.Instance.DeHighLight_Mistake(startIndex, endIndex);
            curRangeTextShowInTaskPane.Select();
            Index++;
            //UpdateProgressBar();
            if (Index == TotalError)
            {
                IsOutOfError = true;
                Thread.Sleep(500);
                MessageBox.Show(SysMessage.Instance.No_error);
                changeUI_OutOfError();
                Index = 0;
                return;
            }
            //
            //sửa lỗi tiếp theo
            //
            //if (!isRightClick)
            {
                FindError.Instance.FirstError_Context = FindError.Instance.lstErrorRange.First().Key;
                FindError.Instance.lstErrorRange[FindError.Instance.FirstError_Context].Select();
                if (!IsFixAll)
                    showCandidateInTaskPane();
            }
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

        private void gridLog_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            showMoreInfoContext();
        }
        private void showMoreInfoContext()
        {
            SynchronizedInvoke(gridLog, delegate ()
            {
                changeUI_ShowMoreInfo();

            });

            DataGridViewRow row = gridLog.SelectedRows[0];
            SELECTED_ERROR = row.Index;
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
            //Word.Find findObject = Globals.ThisAddIn.Application.Selection.Find;
            //findObject.ClearFormatting();
            //SynchronizedInvoke(lblRightContext, delegate () {
            //    findObject.Text = lblRightContext.Text;
            //});
            Word.Document oWordDoc = Globals.ThisAddIn.Application.ActiveDocument;
            Word.Range rng = oWordDoc.Content;
            rng.Find.ClearFormatting();
            object findText = "";
            SynchronizedInvoke(lblRightContext, delegate ()
            {
                DataGridViewRow rowNext = null;
                findText = lblRightContext.Text;
                int count = 0;
                if (gridLog.RowCount > SELECTED_ERROR + 1)
                {
                    rowNext = gridLog.Rows[SELECTED_ERROR + 1];
                    string wrongRowNext = rowNext.Cells[1].Value.ToString();
                    string[] wrongRowNextArr = wrongRowNext.Split(' ');

                    foreach (string i in wrongRowNextArr)
                    {
                        if (lblRightContext.Text.ToLower().Contains(i.ToLower()))
                        {
                            if (++count == 2)
                            {
                                findText = rowNext.Cells[2].Value.ToString();
                                break;
                            }

                        }
                    }
                }

            });
            object oTrue = true;
            object oFalse = false;
            object oFindStop = Word.WdFindWrap.wdFindStop;
            rng.Find.Execute(ref findText, ref oTrue, ref oFalse, ref oTrue,
                    ref oFalse, ref oFalse, ref oTrue, ref oFindStop, ref oFalse,
                    null, null, null, null, null, null);
            rng.Select();

        }

        private void changeUI_IsAutoFix()
        {
            SynchronizedInvoke(pnlSequenceFix, delegate () { pnlSequenceFix.Visible = false; });
            SynchronizedInvoke(pnlAutoFix, delegate () { pnlAutoFix.Location = new System.Drawing.Point(14, 5); });
            SynchronizedInvoke(gridLog, delegate () { gridLog.Location = new System.Drawing.Point(0, 90); });
            SynchronizedInvoke(pnlButtonAutoFix, delegate ()
            {
                pnlButtonAutoFix.Location = new System.Drawing.Point(200, 0);
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
        private void changeUI_IsSequenceFix()
        {
            SynchronizedInvoke(pnlAutoFix, delegate ()
            {
                pnlAutoFix.Location = new System.Drawing.Point(14, 200);
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
                for (int i = pnlAutoFix.Location.Y; i > 5; i--)
                {
                    pnlAutoFix.Location = new System.Drawing.Point(14, i);
                    Thread.Sleep(1);
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
                if (IsFixAll)
                {

                    if (gridLog.Size.Height <= 310)
                        gridLog.Size = new System.Drawing.Size(gridLog.Size.Width, gridLog.Size.Height + 22);
                }
                else
                    if (gridLog.Size.Height <= 250)
                {

                    gridLog.Size = new System.Drawing.Size(gridLog.Size.Width, gridLog.Size.Height + 22);
                    for (int i = gridLog.Location.Y; i >= 90; i--)
                    {
                        gridLog.Location = new System.Drawing.Point(0, i);
                        Thread.Sleep(5);
                    }
                    gridLog.Location = new System.Drawing.Point(0, 90);
                }
                gridLog.Rows.Add(++grigLogCount, oldString, newString);
            });

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
                    IsFixAll = false;
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

                    IsFixAll = true;
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

        private void changeUI_ShowMoreInfo()
        {
            int yGridLog, yShowMore;
            if (IsOutOfError)
            {
                yGridLog = 60;
                yShowMore = 0;
            }
            else
            {
                yGridLog = 150;
                yShowMore = 90;
            }
            SynchronizedInvoke(gridLog, delegate ()
            {

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