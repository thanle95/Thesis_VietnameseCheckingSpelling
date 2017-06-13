using System;
using System.Linq;
using System.Windows.Forms;
using Spell.Algorithm;
using Word = Microsoft.Office.Interop.Word;
using System.Threading;
using System.ComponentModel;

namespace Spell
{
    //phục vụ cho việc thêm vào từ điển
    public enum Position { xxX, xXx, Xxx, xX, Xx, X };

    public partial class UserControl : System.Windows.Forms.UserControl
    {
        private Word.Range curRangeTextShowInTaskPane;
        private string oldString = "", newString = "";
        public bool _isFixAll { get; set; }
        public int Count { get; set; }
        private static UserControl instance = new UserControl();
        private const string ERROR_SPACE = "\"Lỗi dư khoảng trắng\"";
        private int SELECTED_ERROR { get; set; }
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
        public void Start()
        {
            //SynchronizedInvoke(lblWrongContext, delegate ()
            //{
            //    lblWrongContext.Visible = false;
            //});
            //SynchronizedInvoke(lblRightContext, delegate ()
            //{
            //    lblRightContext.Visible = false;
            //});
            //SynchronizedInvoke(lblRightArrow, delegate ()
            //{
            //    lblRightArrow.Visible = false;
            //});
            //SynchronizedInvoke(btnGo, delegate ()
            //{
            //    btnGo.Visible = false;
            //});
            SynchronizedInvoke(gridLog, delegate ()
            {
                gridLog.Rows.Clear();
                gridLog.Size = new System.Drawing.Size(287, 85);
            });
            //changeUI_IsFixAll();
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
        public void showCandidateInTaskPane(bool isFixAll)
        {
            _isFixAll = isFixAll;
            if (_isFixAll)
                changeUI_IsFixAll();
            else
                changeUI_IsNotFixAll();
            while (FindError.Instance.lstErrorRange.Count > 0)
            {
                FixError fixError = new FixError();

                fixError.getCandidatesWithContext(FindError.Instance.FirstError_Context, FindError.Instance.lstErrorRange);
                Word.Range range = FindError.Instance.lstErrorRange[FindError.Instance.FirstError_Context];
                range.Select();

                oldString = FindError.Instance.ToString().Trim();
                newString = fixError.ToString().Trim();
                if (_isFixAll)
                {
                    change(fixError.Token.ToLower(), fixError.hSetCandidate.ElementAt(0));
                }
                else {
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
            showCandidateInTaskPane(_isFixAll);
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
            change(lblWrong.Text.ToLower(), lstbCandidate.SelectedItem.ToString());
        }
        private void change(string wrongText, string fixText)
        {
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
            if (!_isFixAll)
            {
                lblWrong.Text = "\"Từ sai\"";
                lstbCandidate.Items.Clear();
            }
            DocumentHandling.Instance.DeHighLight_Mistake(startIndex, endIndex);
            curRangeTextShowInTaskPane.Select();

            if (FindError.Instance.lstErrorRange.Count == 0)
            {
                MessageBox.Show(SysMessage.Instance.No_error);
                changeUI_IsNotFixAll_OutOfError();
                //SynchronizedInvoke(this, delegate () { this.Visible = false; });
                return;
            }
            //
            //sửa lỗi tiếp theo
            //
            FindError.Instance.FirstError_Context = FindError.Instance.lstErrorRange.First().Key;
            FindError.Instance.lstErrorRange[FindError.Instance.FirstError_Context].Select();
            if (!_isFixAll)
                showCandidateInTaskPane(_isFixAll);

        }
        private void btnChange_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                change(lblWrong.Text.ToLower(), lstbCandidate.SelectedItem.ToString());
        }

        private void lstbCandidate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnChange.Focus();
                change(lblWrong.Text.ToLower(), lstbCandidate.SelectedItem.ToString());
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
                if (_isFixAll)
                {
                    if (gridLog.Location.Y == 5)
                        changeUI_ShowMoreInfo_IsFixAll();
                }
                else {
                    if (gridLog.Location.Y == 195)
                        changeUI_ShowMoreInfo_IsNotFixAll();
                }
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
                if (gridLog.RowCount >= SELECTED_ERROR + 1)
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

        private void changeUI_IsFixAll()
        {
            changeUI_FixSequenceGroup(false);
            SynchronizedInvoke(gridLog, delegate () { gridLog.Location = new System.Drawing.Point(18, 5); });
            changeUI_ShowMore(false);
        }
        //Sửa lỗi tuần tự
        private void changeUI_IsNotFixAll()
        {
            changeUI_FixSequenceGroup(true);
            changeUI_ShowMore(false);
            SynchronizedInvoke(gridLog, delegate ()
            {
                if (gridLog.Location.Y > 195)
                    for (int i = gridLog.Location.Y; i >= 195; i--)
                    {
                        gridLog.Location = new System.Drawing.Point(18, i);
                        Thread.Sleep(5);
                    }
                gridLog.Location = new System.Drawing.Point(18, 195);
            });

        }
        private void changeUI_IsNotFixAll_OutOfError()
        {
            _isFixAll = true;
            changeUI_FixSequenceGroup(false);

            changeUI_ShowMore(false);
            SynchronizedInvoke(gridLog, delegate ()
            {
                for (int i = gridLog.Location.Y; i >= 5; i--)
                {
                    gridLog.Location = new System.Drawing.Point(18, i);
                    Thread.Sleep(1);
                }
            });
        }
        private void changeUI_FixSequenceGroup(bool visible)
        {
            SynchronizedInvoke(lblWrong, delegate ()
            {
                lblWrong.Visible = visible;
            });
            SynchronizedInvoke(btnIgnore, delegate ()
            {
                btnIgnore.Visible = visible;
            });
            SynchronizedInvoke(lstbCandidate, delegate ()
            {
                lstbCandidate.Visible = visible;
            });
            SynchronizedInvoke(btnStart, delegate ()
            {
                btnStart.Visible = visible;
            });
            SynchronizedInvoke(btnChange, delegate ()
            {
                btnChange.Visible = visible;
            });
        }
        private void changeUI_ShowMore(bool visible)
        {
            SynchronizedInvoke(lblWrongContext, delegate ()
            {
                lblWrongContext.Visible = visible;
            });
            SynchronizedInvoke(lblRightArrow, delegate ()
            {
                lblRightArrow.Visible = visible;
            });
            SynchronizedInvoke(lblRightContext, delegate ()
            {
                lblRightContext.Visible = visible;
            });
            SynchronizedInvoke(btnGo, delegate ()
            {
                btnGo.Visible = visible;
            });
        }
        private void addRowGridLog()
        {
            SynchronizedInvoke(gridLog, delegate ()
            {
                gridLog.Visible = true;
                if (_isFixAll)
                {

                    if (gridLog.Size.Height <= 500)
                        gridLog.Size = new System.Drawing.Size(gridLog.Size.Width, gridLog.Size.Height + 22);
                }
                else
                if (gridLog.Size.Height <= 250)
                {

                    gridLog.Size = new System.Drawing.Size(gridLog.Size.Width, gridLog.Size.Height + 22);
                    if (gridLog.Location.Y > 195)
                        for (int i = gridLog.Location.Y; i >= 195; i--)
                        {
                            gridLog.Location = new System.Drawing.Point(18, i);
                            Thread.Sleep(5);
                        }
                    gridLog.Location = new System.Drawing.Point(18, 195);
                }
                gridLog.Rows.Add(++Count, oldString, newString);
            });

        }
        private void changeUI_ShowMoreInfo_IsFixAll()
        {
            changeUI_FixSequenceGroup(false);
            SynchronizedInvoke(gridLog, delegate ()
            {
                if (gridLog.Location.Y != 263)
                {
                    for (int i = 5; i <= 50; i++)
                    {
                        gridLog.Location = new System.Drawing.Point(18, i);
                        Thread.Sleep(5);
                    }
                }
            });
            changeUI_ShowMoreTop();
            changeUI_ShowMore(true);

        }
        private void changeUI_ShowMoreInfo_IsNotFixAll()
        {
            changeUI_FixSequenceGroup(true);
            SynchronizedInvoke(gridLog, delegate ()
            {
                for (int i = 195; i <= 263; i++)
                {
                    gridLog.Location = new System.Drawing.Point(18, i);
                    Thread.Sleep(5);
                }
            });
            changeUI_ShowMoreBottom();
            changeUI_ShowMore(true);
        }
        private void changeUI_ShowMoreTop()
        {
            SynchronizedInvoke(lblWrongContext, delegate ()
            {
                lblWrongContext.Location = new System.Drawing.Point(18, 5);
            });
            SynchronizedInvoke(lblRightArrow, delegate ()
            {
                lblRightArrow.Location = new System.Drawing.Point(22, 16);
            });
            SynchronizedInvoke(lblRightContext, delegate ()
            {
                lblRightContext.Location = new System.Drawing.Point(58, 22);
            });
            SynchronizedInvoke(btnGo, delegate ()
            {
                btnGo.Location = new System.Drawing.Point(214, 15);
            });
        }
        private void changeUI_ShowMoreBottom()
        {
            SynchronizedInvoke(lblWrongContext, delegate ()
            {
                lblWrongContext.Location = new System.Drawing.Point(18, 195);
            });
            SynchronizedInvoke(lblRightArrow, delegate ()
            {
                lblRightArrow.Location = new System.Drawing.Point(22, 216);
            });
            SynchronizedInvoke(lblRightContext, delegate ()
            {
                lblRightContext.Location = new System.Drawing.Point(58, 222);
            });
            SynchronizedInvoke(btnGo, delegate ()
            {
                btnGo.Location = new System.Drawing.Point(214, 215);
            });
        }

    }
}