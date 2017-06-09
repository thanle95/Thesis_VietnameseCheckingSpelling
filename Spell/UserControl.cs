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
        private int DeltaYLocation { get { return 188; } }
        public bool _isFixAll { get; set; }
        public int Count { get; set; }
        private static UserControl instance = new UserControl();
        private const string ERROR_SPACE = "\"Lỗi dư khoảng trắng\"";
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
            SynchronizedInvoke(lblWrongContext, delegate ()
            {
                lblWrongContext.Visible = false;
            });
            SynchronizedInvoke(lblRightContext, delegate ()
            {
                lblRightContext.Visible = false;
            });
            SynchronizedInvoke(lblRightArrow, delegate ()
            {
                lblRightArrow.Visible = false;
            });
            SynchronizedInvoke(btnGo, delegate ()
            {
                btnGo.Visible = false;
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
        public void showCandidateInTaskPane(bool isFixAll)
        {
            _isFixAll = isFixAll;
            string oldString = "", newString = "";

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

                    SynchronizedInvoke(gridLog, delegate () { gridLog.Rows.Add(++Count, oldString, newString); });
                    change(fixError.Token.ToLower(), fixError.hSetCandidate.ElementAt(0));
                }
                else {
                    changeUI_IsNotFixAll();
                    SynchronizedInvoke(lblWrong, delegate () { lblWrong.Text = fixError.Token; });
                    SynchronizedInvoke(lstbCandidate, delegate () { lstbCandidate.Items.Clear(); });
                    SynchronizedInvoke(gridLog, delegate () { gridLog.Rows.Add(++Count, oldString, newString); });
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
        public void showWrongWithoutSuggest()
        {

        }
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

            SynchronizedInvoke(lblWrongContext, delegate ()
            {
                lblWrongContext.Text = row.Cells[1].Value.ToString();
                lblWrongContext.Visible = true;
            });
            SynchronizedInvoke(lblRightContext, delegate ()
            {
                lblRightContext.Text = row.Cells[2].Value.ToString();
                lblRightContext.Visible = true;
            });
            SynchronizedInvoke(lblRightArrow, delegate ()
            {
                lblRightArrow.Visible = true;
            });
            SynchronizedInvoke(btnGo, delegate ()
            {
                btnGo.Visible = true;
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
                findText = lblRightContext.Text;
            });
            object oTrue = true;
            object oFalse = false;
            object oFindStop = Word.WdFindWrap.wdFindStop;
            rng.Find.Execute(ref findText, ref oTrue, ref oFalse, ref oTrue,
                ref oFalse, ref oFalse, ref oTrue, ref oFindStop, ref oFalse,
                null, null, null, null, null, null);
            rng.Select();

        }
        public void changeUIStart()
        {
            SynchronizedInvoke(gridLog, delegate () { gridLog.Location = new System.Drawing.Point(18, 5); });
        }
        private void changeUI_IsNotFixAll()
        {
            SynchronizedInvoke(lblWrong, delegate ()
            {
                lblWrong.Visible = true;
            });
            SynchronizedInvoke(btnIgnore, delegate ()
            {
                btnIgnore.Visible = true;
            });
            SynchronizedInvoke(lstbCandidate, delegate ()
            {
                lstbCandidate.Visible = true;
            });
            SynchronizedInvoke(btnStart, delegate ()
            {
                btnStart.Visible = true;
            });
            SynchronizedInvoke(btnChange, delegate ()
            {
                btnChange.Visible = true;
            });
            SynchronizedInvoke(gridLog, delegate ()
            {
                gridLog.Location = new System.Drawing.Point(18, 195);
            });

        }
        private void changeUI_ShowMoreInfo_IsNotFixAll()
        {
            SynchronizedInvoke(gridLog, delegate ()
            {
                for (int i = 195; i <= 263; i++)
                {
                    gridLog.Location = new System.Drawing.Point(18, i);
                    Thread.Sleep(5);
                }
            });
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
        private void changeUI_ShowMoreInfo_IsFixAll()
        {
            SynchronizedInvoke(lblWrong, delegate ()
            {
                lblWrong.Visible = false;
            });
            SynchronizedInvoke(btnIgnore, delegate ()
            {
                btnIgnore.Visible = false;
            });
            SynchronizedInvoke(lstbCandidate, delegate ()
            {
                lstbCandidate.Visible = false;
            });
            SynchronizedInvoke(btnStart, delegate ()
            {
                btnStart.Visible = false;
            });
            SynchronizedInvoke(btnChange, delegate ()
            {
                btnChange.Visible = false;
            });
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
    }
}