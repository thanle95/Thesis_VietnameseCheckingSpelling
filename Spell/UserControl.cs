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

    public enum Position { xxX, xXx, Xxx, xX, Xx, X };
    public partial class UserControl : System.Windows.Forms.UserControl
    {
        private Word.Range curRangeTextShowInTaskPane;
        private List<Word.Range> lstErrorRange = new List<Word.Range>();
        private static UserControl instance = new UserControl();
        private Dictionary<string, List<string>> globalsCandidates;
        private UserControl()
        {
            globalsCandidates = new Dictionary<string, List<string>>();
            InitializeComponent();
        }

        public static UserControl Instance
        {
            get
            {
                return instance;
            }
        }

        private void lstbCandidate_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblFix.Text = lstbCandidate.SelectedItem.ToString();
        }


        /// <summary>
        /// hiện gợi ý lên task pane
        /// </summary>
        /// <param name="w">từ lỗi</param>
        public void showCandidateInTaskPane(string prepre, string pre, Word.Range range, string next, string nextnext)
        {
            //if (Globals.ThisAddIn.Application.Selection.Text.Length > 1)
            //{
            //    curRangeTextShowInTaskPane = Globals.ThisAddIn.Application.Selection.Range;

            //    string w = curRangeTextShowInTaskPane.Text.Trim();

            //    if (w.Length > 0)
            //    {
            //        List<string> items = NewUtility.Instance.generateCandidatePerWord(w);
            //        lblWrong.Text = w;
            //        lstbCandidate.Items.Clear();
            //        lstbCandidate.Items.Add(w);
            //        foreach (string item in items)
            //            if (!item.ToLower().Equals(w.ToLower()))
            //                if(item.Length > 1)
            //                lstbCandidate.Items.Add(item);
            //    }
            //}
            //while (true)
            //{
            string token = range.Text;
            if (token.Length > 0)
            {
                HashSet<string> items = Candidate.getInstance.selectiveCandidate(prepre, pre, token, next, nextnext);
                lblWrong.Text = token;
                lstbCandidate.Items.Clear();
                //lstbCandidate.Items.Add(token);
                foreach (string item in items)
                    if (!item.ToLower().Equals(token.ToLower()))
                        if (item.Length > 1)
                            lstbCandidate.Items.Add(item.Trim());
            }
            //}
        }
        public void showCandidateInTaskPane()
        {
            //hiện lỗi đầu tiên lên task pane
            if (lstErrorRange.Count > 0)
            {
                Word.Words words = Globals.ThisAddIn.Application.ActiveDocument.Words;

                Word.Range tokenRange = lstErrorRange.First(); //0: temp
                for (int j = 1, i = j - 1; j <= words.Count; j++, i++)
                {
                    if (words[j].Text.ToLower().Trim().Equals(tokenRange.Text.Trim().ToLower()))
                    {
                        string word = words[j].Text.Trim().ToLower();
                        int length = words.Count;
                        string prepre = "", pre = "", next = "", nextnext = "";
                        if (i == 0)
                        {
                            if (length > 1)
                                next = words[j + 1].Text.Trim().ToLower();
                            if (length > 2)
                                nextnext = words[j + 2].Text.Trim().ToLower();
                        }
                        else if (i == 1)
                        {
                            if (length > 1)
                                pre = words[j - 1].Text.Trim().ToLower();
                            if (length > 2)
                                next = words[j + 1].Text.Trim().ToLower();
                            if (length > 3)
                                nextnext = words[j + 2].Text.Trim().ToLower();
                        }
                        else if (i == 2)
                        {
                            if (length > 2)
                            {
                                prepre = words[j - 2].Text.Trim().ToLower();
                                pre = words[j - 1].Text.Trim().ToLower();
                            }
                            if (length > 3)
                                next = words[j + 1].Text.Trim().ToLower();
                            if (length > 4)
                                nextnext = words[j + 2].Text.Trim().ToLower();
                        }
                        else if (i > 2 && i < length - 2)
                        {
                            if (length > 5)
                            {
                                prepre = words[j - 2].Text.Trim().ToLower();
                                pre = words[j - 1].Text.Trim().ToLower();
                                next = words[j + 1].Text.Trim().ToLower();
                                nextnext = words[j + 2].Text.Trim().ToLower();
                            }
                        }
                        else if (i == length - 2)
                        {
                            if (length > 2)
                            {
                                pre = words[j - 1].Text.Trim().ToLower();
                                next = words[j + 1].Text.Trim().ToLower();
                            }
                            if (length > 3)
                                prepre = words[j - 2].Text.Trim().ToLower();
                        }
                        else if (i == length - 1)
                        {
                            if (length > 1)
                                pre = words[j - 1].Text.Trim().ToLower();
                            if (length > 2)
                                prepre = words[j - 2].Text.Trim().ToLower();
                        }
                        string token = tokenRange.Text.Trim().ToLower();
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
                                if(lstbCandidate.Items.Count > 0)
                                    lstbCandidate.SetSelected(0, true);
                                btnChange.Focus();
                            }
                            break;
                        }

                    }
                }
            }
            //}
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
        public void showWrongWithSuggest()
        {
            try
            {
                //while (true)
                //{
                globalsCandidates.Clear();
                DocumentHandling.Instance.DeHighLight_All_Mistake(Globals.ThisAddIn.Application.ActiveDocument.Characters);
                Word.Sentences sentences = Globals.ThisAddIn.Application.ActiveDocument.Sentences;
                List<string> mySentences = DocumentHandling.Instance.getPhrase(sentences);
                Word.Words globalWords = Globals.ThisAddIn.Application.ActiveDocument.Words;
                bool isFault = false;
                //Xử lý từng cụm từ, vì mỗi cụm từ có liên quan mật thiết với nhau
                foreach (string sentence in mySentences)
                {
                    string[] words = sentence.Trim().Split(' ');
                    int length = words.Length;

                    for (int i = 0; i < length; i++)
                    {
                        string word = words[i].Trim();

                        //Kiểm tra nếu không phải là từ Việt Nam
                        //Thì highLight
                        if (!VNDictionary.getInstance.isSyllableVN(word))
                        {
                            lstErrorRange.Add((DocumentHandling.Instance.HighLight_Mistake(word, globalWords)));
                            isFault = true;
                            break;
                        }


                    }//end for
                    if (isFault)
                        break;
                }
                //Thread.Sleep(5000);
                //}
                //foreach (Word.Range range in lstErrorRange)
                //    MessageBox.Show(range.Text + ": " + range.Start);
                showCandidateInTaskPane();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
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

        private void btnTest_Click(object sender, EventArgs e)
        {

        }

        private void UserControl_Load(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            startFindError();
        }
        private void startFindError()
        {
            lstErrorRange = new List<Word.Range>();
            showWrongWithSuggest();
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            change();
        }
        private void change()
        {
            curRangeTextShowInTaskPane = lstErrorRange.First();
            curRangeTextShowInTaskPane.Text = lstbCandidate.SelectedItem.ToString();
            lblWrong.Text = "\"Wrong Text\"";
            lblFix.Text = "\"Fix Text\"";
            lstbCandidate.Items.Clear();
            DocumentHandling.Instance.DeHighLight_All_Mistake(Globals.ThisAddIn.Application.ActiveDocument.Characters);
            //Globals.ThisAddIn.Application.Selection.GoTo(Word.WdGoToItem.wdGoToLine, Word.WdGoToDirection.wdGoToAbsolute, 3);
            curRangeTextShowInTaskPane.Select();
            lstErrorRange.Remove(lstErrorRange.First());
            if (lstErrorRange.Count > 0)
                showCandidateInTaskPane();

            //------------------
            startFindError();
        }
        private void btnChange_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
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
