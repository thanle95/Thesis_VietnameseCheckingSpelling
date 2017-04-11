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

namespace Spell
{
    //phục vụ cho việc thêm vào từ điển
    public enum Position { xxX, xXx, Xxx, xX, Xx, X };
    public partial class UserControl : System.Windows.Forms.UserControl
    {
        private Word.Range curRangeTextShowInTaskPane;
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
        public static string StartSent
        {
            get
            {
                return "<s>";
            }
        }
        public static string EndSent
        {
            get
            {
                return "</s>";
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
                Word.Words words = Globals.ThisAddIn.Application.ActiveDocument.Words;

                Word.Range tokenRange = findErrorRangeByStartIndex(startIndex);
                for (int iWord = 1; iWord <= words.Count; iWord++)
                {
                    string token = tokenRange.Text.Trim().ToLower();
                    string word = words[iWord].Text.Trim().ToLower();
                    if (word.Equals(token))
                    {
                        string[] gramAroundIWord = getGramArroundIWord(iWord, words.Count, words);
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

                    } //end if compare to find token
                } // end for
            }
            //}
        }

        private Word.Range findErrorRangeByStartIndex(int startIndex)
        {
            List<Word.Range> temp = new List<Word.Range>();
            temp.AddRange(lstErrorRange);
            int count = temp.Count;
            for(int i = 0; i < count; i ++)
            {
                if (temp[i].Start <= startIndex)
                {
                    temp.Remove(temp[i]);
                    count--;
                    i--;
                }
                else
                    break;
            }
            return temp.First();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iWord">word thứ i</param>
        /// <param name="lengthSentence">độ dài câu</param>
        /// <param name="words">toàn bộ words trong document</param>
        /// <returns></returns>
        public string[] getGramArroundIWord(int iWord, int lengthSentence, Word.Words words)
        {
            string[] ret = new string[4];
            //0: prepre
            //1: pre
            //2: next
            //3: nextnexxt
            string prepre = "", pre = "", next = "", nextnext = "";
            if (iWord == 1)
            {
                pre = StartSent;
                if (lengthSentence > 1)
                    next = words[iWord + 1].Text.Trim().ToLower();
                if (lengthSentence > 2)
                    nextnext = words[iWord + 2].Text.Trim().ToLower();
            }
            else if (iWord == 2)
            {
                if (lengthSentence > 1)
                    pre = words[iWord - 1].Text.Trim().ToLower();
                if (lengthSentence > 2)
                    next = words[iWord + 1].Text.Trim().ToLower();
                if (lengthSentence > 3)
                    nextnext = words[iWord + 2].Text.Trim().ToLower();
            }
            else if (iWord == 3)
            {
                if (lengthSentence > 2)
                {
                    prepre = words[iWord - 2].Text.Trim().ToLower();
                    pre = words[iWord - 1].Text.Trim().ToLower();
                }
                if (lengthSentence > 3)
                    next = words[iWord + 1].Text.Trim().ToLower();
                if (lengthSentence > 4)
                    nextnext = words[iWord + 2].Text.Trim().ToLower();
            }
            else if (iWord > 3 && iWord < lengthSentence - 1)
            {
                if (lengthSentence > 5)
                {
                    prepre = words[iWord - 2].Text.Trim().ToLower();
                    pre = words[iWord - 1].Text.Trim().ToLower();
                    next = words[iWord + 1].Text.Trim().ToLower();
                    nextnext = words[iWord + 2].Text.Trim().ToLower();
                }
            }
            else if (iWord == lengthSentence - 1)
            {
                if (lengthSentence > 2)
                {
                    pre = words[iWord - 1].Text.Trim().ToLower();
                    next = words[iWord + 1].Text.Trim().ToLower();
                }
                if (lengthSentence > 3)
                    prepre = words[iWord - 2].Text.Trim().ToLower();
            }
            else if (iWord == lengthSentence)
            {
                next = EndSent;
                if (lengthSentence > 1)
                    pre = words[iWord - 1].Text.Trim().ToLower();
                if (lengthSentence > 2)
                    prepre = words[iWord - 2].Text.Trim().ToLower();
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
        /// HighLight lỗi hiện tại và hiện gợi ý
        /// </summary>
        public int showWrongWithSuggest()
        {
            try
            {
                //dehightlight tất cả những lỗi trước đó
                DocumentHandling.Instance.DeHighLight_All_Mistake(Globals.ThisAddIn.Application.ActiveDocument.Characters);
                //lấy toàn bộ danh sách các từ trong Active Document, để lấy được ngữ cảnh
                Word.Words globalWords = Globals.ThisAddIn.Application.ActiveDocument.Words;
                //lấy những câu trong Active Document
                Word.Sentences sentences = Globals.ThisAddIn.Application.ActiveDocument.Sentences;
                //với mỗi câu, tách thành từng cụm có liên quan mật thiết với nhau, như "", (),...
                List<string> mySentences = DocumentHandling.Instance.getPhrase(sentences);
                //Xử lý từng cụm từ, vì mỗi cụm từ có liên quan mật thiết với nhau
                int countWord = 0;
                foreach (string mySentence in mySentences)
                {
                    string[] words =mySentence.Trim().Split(' ');
                    //số lượng các từ trong cụm
                    int length = words.Length;
                    //duyệt qua từng từ trong cụm
                    for (int i = 0; i < length; i++)
                    {
                        string token = words[i].Trim().ToLower();
                        countWord++;
                        //Kiểm tra nếu không phải là từ Việt Nam
                        //Thì highLight
                        if (!VNDictionary.getInstance.isSyllableVN(token))
                        {
                            lstErrorRange.Add((DocumentHandling.Instance.HighLight_MistakeWrongWord(token, globalWords, countWord)));
                            continue;
                        }
                        else
                        {
                            //tìm vị trí của token trong globalWords để xác định ngữ cảnh
                            for (int iWord = countWord; iWord <= globalWords.Count; iWord++)
                            {
                                string word = globalWords[iWord].Text.Trim().ToLower();
                                //tìm được vị trí của token
                                if (word.Equals(token))
                                {
                                    //xác định ngữ cảnh
                                    string[] gramAroundIWord = getGramArroundIWord(iWord, globalWords.Count, globalWords);
                                    string prepre = gramAroundIWord[0], pre = gramAroundIWord[1], next = gramAroundIWord[2], nextnext = gramAroundIWord[3];
                                    if (i == 0)
                                    {
                                        pre = StartSent;
                                        prepre = "";
                                    }
                                    if (i == length - 1)
                                    {
                                        next = EndSent;
                                        nextnext = "";
                                    }
                                    //kiểm tra token có khả năng sai hay k
                                    if (!RightWordCandidate.getInstance.checkRightWord(prepre, pre, token, next, nextnext))
                                    {
                                        lstErrorRange.Add((DocumentHandling.Instance.HighLight_MistakeRightWord(token, globalWords, countWord)));
                                        continue;
                                    }
                                    break;
                                }
                            }
                        }

                    }//end for: duyệt từ từng trong cụm
                    //if (isFault)
                    //    break;
                }//end for: duyệt từ cụm
                //showCandidateInTaskPane();
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
            DocumentHandling.Instance.DeHighLight_All_Mistake(curRangeTextShowInTaskPane.Start, curRangeTextShowInTaskPane.End);
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
        public int startFindError()
        {
            MessageBox.Show("dang kiem tra loi");
            lstErrorRange = new List<Word.Range>();
            int count = showWrongWithSuggest();
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
            foreach(Word.Range range in lstErrorRange)
                if(range.Text.Equals(lblWrong.Text.ToLower()))
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
