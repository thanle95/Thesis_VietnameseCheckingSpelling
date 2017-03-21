﻿using System;
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
                        string token = words[i].Trim().ToLower();

                        //Kiểm tra nếu không phải là từ Việt Nam
                        //Thì highLight
                        if (!VNDictionary.getInstance.isSyllableVN(token))
                        {
                            lstErrorRange.Add((DocumentHandling.Instance.HighLight_MistakeWrongWord(token, globalWords)));
                            isFault = true;
                            break;
                        }
                        else
                        {
                            for (int iWord = 1; iWord <= globalWords.Count; iWord++)
                            {
                                string word = globalWords[iWord].Text.Trim().ToLower();
                                if (word.Equals(token))
                                {
                                    string[] gramAroundIWord = getGramArroundIWord(iWord, globalWords.Count, globalWords);
                                    string prepre = gramAroundIWord[0], pre = gramAroundIWord[1], next = gramAroundIWord[2], nextnext = gramAroundIWord[3];

                                    if (!RightWordCandidate.getInstance.checkRightWord(prepre, pre, word, next, nextnext))
                                    {
                                        lstErrorRange.Add((DocumentHandling.Instance.HighLight_MistakeRightWord(token, globalWords)));
                                        isFault = true;
                                        break;
                                    }
                                    break;
                                }
                            }
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
