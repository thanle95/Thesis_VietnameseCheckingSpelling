using System;
using System.Linq;
using System.Windows.Forms;
using Spell.Algorithm;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
namespace Spell
{
    //phục vụ cho việc thêm vào từ điển
    public enum Position { xxX, xXx, Xxx, xX, Xx, X };
    public partial class UserControl : System.Windows.Forms.UserControl
    {
        private Word.Range curRangeTextShowInTaskPane;
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
        public static string WRONG_TEXT
        {
            get
            {
                return "\"Lỗi\"";
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
        public void showCandidateInTaskPaneWithCountWord()
        {
            FixError fixError = new FixError();

            fixError.getCandidatesWithContext(FindError.Instance.FirstError_Context, FindError.Instance.lstErrorRange);

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
                return;
            }

            //
            //sửa lỗi tiếp theo
            //
            FindError.Instance.FirstError_Context = FindError.Instance.lstErrorRange.First().Key;
            FindError.Instance.lstErrorRange[FindError.Instance.FirstError_Context].Select();
            showCandidateInTaskPaneWithCountWord();
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
            change();
        }
        private void change()
        {
            int startIndex = 0;
            int endIndex = 0;
            string wrongText = lblWrong.Text.ToLower();
            if (lblWrong.Text.Equals(ERROR_SPACE))
                wrongText = " ";
            bool isMajuscule = false;
            string fixText = lstbCandidate.SelectedItem.ToString();
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
            lblWrong.Text = "\"Wrong Text\"";
            lstbCandidate.Items.Clear();
            DocumentHandling.Instance.DeHighLight_Mistake(startIndex, endIndex);
            curRangeTextShowInTaskPane.Select();

            if (FindError.Instance.lstErrorRange.Count == 0)
            {
                MessageBox.Show(SysMessage.Instance.No_error);
                return;
            }
            //
            //sửa lỗi tiếp theo
            //
            FindError.Instance.FirstError_Context = FindError.Instance.lstErrorRange.First().Key;
            FindError.Instance.lstErrorRange[FindError.Instance.FirstError_Context].Select();
            showCandidateInTaskPaneWithCountWord();

        }
        private void btnChange_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                change();
            }
        }

        private void lstbCandidate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnChange.Focus();
                change();
            }
        }
        private void UserControl_Layout(object sender, LayoutEventArgs e)
        {
            Office.CommandBar cb = Globals.ThisAddIn.Application.CommandBars["Spelling"];
            //MessageBox.Show(cb.Left + " " + cb.Top);

            cb.Left = 1100;
            cb.Top = 300;
            Width = 500;
            //MessageBox.Show(cb.Left + " " + cb.Top);
        }
    }
}