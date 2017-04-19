using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spell.Algorithm;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

namespace Spell
{
    //phục vụ cho việc thêm vào từ điển
    public enum Position { xxX, xXx, Xxx, xX, Xx, X };
    public partial class UserControl : System.Windows.Forms.UserControl
    {
        private Word.Range curRangeTextShowInTaskPane;
        private Word.Sentences curSentences;
        private List<Word.Range> lstErrorRange = new List<Word.Range>();
        private static UserControl instance = new UserControl();
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
                return "\"Wrong text\"";
            }
        }
        private void lstbCandidate_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblFix.Text = lstbCandidate.SelectedItem.ToString();
        }
        /// <summary>
        /// hiện gợi ý sửa lỗi lên task pane
        /// </summary>
        /// <param name="prepre"></param>
        /// <param name="pre"></param>
        /// <param name="range"></param>
        /// <param name="next"></param>
        /// <param name="nextnext"></param>
        public void showCandidateInTaskPane(string prepre, string pre, Word.Range range, string next, string nextnext)
        {
            string token = range.Text;
            if (token.Length > 0)
            {
                //chọn ra những ứng cử viên dựa vào ngữ cảnh
                HashSet<string> items = Candidate.getInstance.selectiveCandidate(prepre, pre, token, next, nextnext);
                //hiện lỗi lên taskpane và label
                lblWrong.Text = token;
                lstbCandidate.Items.Clear();
                foreach (string item in items)
                    if (!item.ToLower().Equals(token.ToLower()))
                        if (item.Length > 1)
                            lstbCandidate.Items.Add(item.Trim());
            }
        }
        /// <summary>
        /// hiện gợi ý sữa lỗi lên taskpane, tự duyệt ngữ cảnh
        /// </summary>
        public void showCandidateInTaskPane(int startIndex)
        {
            if (lstErrorRange.Count > 0)
            {

                Word.Range tokenRange = findErrorRangeByStartIndex(startIndex);
                string token = tokenRange.Text.Trim().ToLower();
                for (int iWord = 1; iWord <= curSentences.Count; iWord++)
                {
                    string[] words = curSentences[iWord].Text.Trim().Split(' ');
                    int i = 0;
                    foreach (string word in words)
                    {
                        if (word.Trim().ToLower().Equals(token))
                        {
                            string[] gramAroundIWord = getGramArroundIWord(i, words);
                            string prepre = gramAroundIWord[0], pre = gramAroundIWord[1], next = gramAroundIWord[2], nextnext = gramAroundIWord[3];

                            if (token.Length > 0)
                            {
                                HashSet<string> items = Candidate.getInstance.selectiveCandidate(prepre, pre, token, next, nextnext);
                                lblWrong.Text = token;
                                lstbCandidate.Items.Clear();
                                //lstbCandidate.Items.Add(token);
                                foreach (string item in items)
                                {
                                    if (!item.ToLower().Equals(token.ToLower()))
                                        if (item.Length > 1)
                                            lstbCandidate.Items.Add(item.Trim());
                                    if (lstbCandidate.Items.Count > 0)
                                        lstbCandidate.SetSelected(0, true);
                                    btnChange.Focus();
                                }
                                break;
                            }
                        }
                        i++;
                    } //end if compare to find token
                } // end for
            }
            //}
        }

        private Word.Range findErrorRangeByStartIndex(int startIndex)
        {
            List<Word.Range> temp = new List<Word.Range>();
            //temp.AddRange(lstErrorRange);
            //int count = temp.Count;
            //for (int i = 0; i < count; i++)
            //{
            //    if (temp[i].Start <= startIndex)
            //    {
            //        temp.Remove(temp[i]);
            //        count--;
            //        i--;
            //    }i
            //    else
            //        break;
            //}

            foreach (Word.Range range in lstErrorRange)
            {
                if (range != null)
                    if (range.Start <= startIndex && startIndex <= range.End)
                    {
                        temp.Add(range);
                        break;
                    }
            }
            if (temp.Count == 0)
                temp.Add(lstErrorRange.First());
            return temp.First();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iWord">word thứ i</param>
        /// <param name="lengthSentence">độ dài câu</param>
        /// <param name="words">toàn bộ words trong document</param>
        /// <returns></returns>
        public string[] getGramArroundIWord(int iWord, string[] words)
        {
            string[] ret = new string[4];
            //0: prepre
            //1: pre
            //2: next
            //3: nextnexxt
            string prepre = "", pre = "", next = "", nextnext = "";
            int length = words.Length;
            if (iWord == 0)
                pre = Ngram.Instance.START_STRING;
            if (iWord > 0)
                pre = words[iWord - 1];
            if (iWord > 1)
                prepre = words[iWord - 2];
            if (iWord == length - 1)
                next = Ngram.Instance.END_STRING;
            if (iWord < length - 1)
                next = words[iWord + 1];
            if (iWord < length - 2)
                nextnext = words[iWord + 2];
            Regex r = new Regex(StringConstant.Instance.patternCheckSpecialChar);
            if (pre.Length > 0 && iWord != 1) //pre không phải từ đầu câu
            {
                Match m = r.Match(pre);
                if (m.Success | char.IsUpper(pre.Trim()[0]))
                {
                    pre = Ngram.Instance.START_STRING;
                    prepre = "";
                }
            }
            if (next.Length > 0)
            {
                Match m = r.Match(next);
                if (m.Success | char.IsUpper(next.Trim()[0]))
                {
                    next = Ngram.Instance.END_STRING;
                    nextnext = "";
                }
            }
            if (prepre.Length > 0)
            {
                Match m = r.Match(prepre);
                if (m.Success | char.IsUpper(prepre.Trim()[0]))
                {
                    prepre = "";
                }
            }
            if (nextnext.Length > 0)
            {
                Match m = r.Match(nextnext);
                if (m.Success | char.IsUpper(nextnext.Trim()[0]))
                {
                    nextnext = "";
                }
            }
            ret[0] = prepre;
            ret[1] = pre;
            ret[2] = next;
            ret[3] = nextnext;
            return ret;
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
        public int showWrongWithSuggest(int startIndex, int endIndex)
        {
            try
            {
                //dehightlight tất cả những lỗi trước đó
                DocumentHandling.Instance.DeHighLight_All_Mistake(Globals.ThisAddIn.Application.ActiveDocument.Characters);
                lblWrong.Text = WRONG_TEXT;
                lstbCandidate.Items.Clear();
                lstErrorRange.Clear();
                //lấy toàn bộ danh sách các từ trong Active Document, để lấy được ngữ cảnh
                Word.Words globalWords = Globals.ThisAddIn.Application.ActiveDocument.Words;
                //lấy những câu trong Active Document
                curSentences = Globals.ThisAddIn.Application.Selection.Sentences;
                //lấy danh sách câu dựa trên vùng được bôi đen
                List<string> sentencesGetByRange = DocumentHandling.Instance.getSentence(startIndex, endIndex, curSentences);
                //với mỗi câu, tách thành từng cụm có liên quan mật thiết với nhau, như "", (),...
                List<string> mySentences = DocumentHandling.Instance.getPhrase(sentencesGetByRange);
                //Xử lý từng cụm từ, vì mỗi cụm từ có liên quan mật thiết với nhau
                int countWord = 0;
                foreach (string mySentence in mySentences)
                {
                    string[] words = mySentence.Trim().Split(' ');

                    //số lượng các từ trong cụm
                    int length = words.Length;
                    //duyệt qua từng từ trong cụm
                    for (int i = 0; i < length; i++)
                    {
                        countWord++;
                        string token = words[i].Trim().ToLower();
                        if (token.Length < 1)
                            continue;
                        //Kiểm tra các kí tự đặc biệt, mail, số, tên riêng, viết tắt
                        // string[] phraseArr = new Regex(StringConstant.Instance.patternCheckWord).Split(text);

                        //foreach (string iPharse in phraseArr)
                        //{
                        Regex r = new Regex(StringConstant.Instance.patternCheckSpecialChar);
                        Match m = r.Match(token);
                        // !(char.IsUpper(token[0])) | DocumentHandling.Instance.checkEng(token)
                        if (m.Success || (char.IsUpper(words[i].Trim()[0]) && i != 0))
                            continue;
                        else
                        {
                            //Kiểm tra nếu không phải là từ Việt Nam
                            //Thì highLight
                            if (!VNDictionary.getInstance.isSyllableVN(token))
                                lstErrorRange.Add((DocumentHandling.Instance.HighLight_MistakeWrongWord(token, curSentences, countWord)));
                            else
                            {
                                //tìm vị trí của token trong globalWords để xác định ngữ cảnh
                                string[] gramAroundIWord = getGramArroundIWord(i, words);
                                string prepre = gramAroundIWord[0], pre = gramAroundIWord[1], next = gramAroundIWord[2], nextnext = gramAroundIWord[3];
                                //kiểm tra token có khả năng sai hay k
                                if (!RightWordCandidate.getInstance.checkRightWord(prepre, pre, token, next, nextnext))
                                    lstErrorRange.Add((DocumentHandling.Instance.HighLight_MistakeRightWord(token, curSentences, countWord)));
                            }
                        }//end for: duyệt từ từng trong cụm
                    }//end for: duyệt từ cụm
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            return lstErrorRange.Count;
        }

        //
        //---Kiet Start
        //

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
            foreach (Word.Range range in lstErrorRange)
                if (range.Text.Equals(lblWrong.Text.ToLower()))
                {
                    startIndex = range.Start;
                    endIndex = range.End;
                    lstErrorRange.Remove(range);
                    break;
                }
            DocumentHandling.Instance.DeHighLight_Mistake(startIndex, endIndex);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ignore();
            Ngram.Instance.addToDictionary(lblWrong.Text, null, null, Position.X);
        }



        private void UserControl_Load(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            int startIndex = Globals.ThisAddIn.Application.Selection.Start;
            startFixError(startIndex);
        }
        public void startFixError(int startIndex)
        {
            showCandidateInTaskPane(startIndex);
        }
        public int startFindError(int startInex, int endIndex)
        {
            lstErrorRange = new List<Word.Range>();
            int count = showWrongWithSuggest(startInex, endIndex);
            return count;
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            change();
        }
        private void change()
        {
            //lstErrorRange.Remove(lstErrorRange.Where(x => x.Text.Equals(lblWrong.Text.ToLower())).Single());
            int startIndex = 0;
            int endIndex = 0;
            foreach (Word.Range range in lstErrorRange)
                if (range.Text.Equals(lblWrong.Text.ToLower()))
                {
                    startIndex = range.Start;
                    curRangeTextShowInTaskPane = range;
                    lstErrorRange.Remove(range);
                    break;
                }

            curRangeTextShowInTaskPane.Text = lstbCandidate.SelectedItem.ToString();
            endIndex = startIndex + curRangeTextShowInTaskPane.Text.Length;
            lblWrong.Text = "\"Wrong Text\"";
            lblFix.Text = "\"Fix Text\"";
            lstbCandidate.Items.Clear();
            DocumentHandling.Instance.DeHighLight_Mistake(startIndex, endIndex);
            //Globals.ThisAddIn.Application.Selection.GoTo(Word.WdGoToItem.wdGoToLine, Word.WdGoToDirection.wdGoToAbsolute, 3);
            curRangeTextShowInTaskPane.Select();
            //lstErrorRange.Remove(lstErrorRange.First());
            //if (lstErrorRange.Count > 0)
            //showCandidateInTaskPane();

            //------------------
            //startFindError();
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


        //
        //---Kiet End
        //
    }
}