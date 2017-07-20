using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
namespace Spell.Algorithm
{
    public class Context
    {
        public string PREPRE
        {
            get; set;
        }
        public string PRE
        {
            get; set;
        }
        public string TOKEN
        {
            get; set;
        }
        public string NEXT
        {
            get; set;
        }
        public string NEXTNEXT
        {
            get; set;
        }
        public int WordCount
        {
            get
            {
                int count = 1;
                if (PREPRE.Length > 0)
                    count++;
                if (PRE.Length > 0)
                    count++;
                if (NEXT.Length > 0)
                    count++;
                if (NEXTNEXT.Length > 0)
                    count++;

                return count;
            }
        }
        private Regex regexSPEC = new Regex(StringConstant.Instance.patternSPEC);
        private Regex regexOPEN = new Regex(StringConstant.Instance.patternOPEN);
        private Regex regexCLOSE = new Regex(StringConstant.Instance.patternCLOSE);
        public Context(int iWord, string[] words)
        {
            getContext(iWord, words);
        }
        public void CopyForm(Context c)
        {
            this.PREPRE = c.PREPRE;
            this.PRE = c.PRE;
            this.TOKEN = c.TOKEN;
            this.NEXT = c.NEXT;
            this.NEXTNEXT = c.NEXTNEXT;
        }
        public Context(Word.Words words, Word.Sentences sentences)
        {
            string[] wordArr = sentences.First.Text.Split(' ');
            for (int i = 0; i < wordArr.Length; i++)
                if (wordArr[i].Trim().Equals(words.First.Text.Trim()))
                {
                    getContext(i, wordArr);
                    return;
                }
        }
        public void GetSeletedContext(Word.Words words, Word.Sentences sentences)
        {
            string[] wordArr = sentences[1].Text.Split(' ');
            for (int i = 0; i < wordArr.Length; i++)
                if (wordArr[i].Trim().Equals(words[1].Text.Trim()))
                {
                    getContext(i, wordArr);
                    return;
                }
        }
        public Context()
        {
            PREPRE = PRE = TOKEN = NEXT = NEXTNEXT = "";
        }
        /// <summary>
        /// getContext by selectionWord
        /// </summary>
        public void getContext()
        {
            Word.Words words = Globals.ThisAddIn.Application.Selection.Words;
            Word.Sentences sentences = Globals.ThisAddIn.Application.Selection.Sentences;

            string[] wordArray = sentences.First.Text.Trim().Split(' ');
            string iWord = words.First.Text.Trim();
            int i = 0;

            int wordStart = words.First.Start - sentences.First.Start;
            for (int iFor = 0; iFor < wordStart; iFor++)
            {
                if (sentences.First.Text[iFor] == ' ')
                    i++;
            }
            getContext(i, wordArray);

        }
        public void getContext(int iWord, string[] words)
        {
            PREPRE = PRE = TOKEN = NEXT = NEXTNEXT = "";
            string token = words[iWord].Trim();
            //1
            TOKEN = regexSPEC.Replace(token, "");
            // Không giống như lúc đầu 
            // ---> chứa kí tự đặc biệt
            //2
            if (TOKEN.Length < token.Length)
            {
                //3
                PREPRE = PRE = NEXT = NEXTNEXT = "";
                return;
            }
            else
            {
                TOKEN = regexOPEN.Replace(token, "");
                //Chứa ký tự thuộc nhóm OPEN
                //4
                if (TOKEN.Length < token.Length)
                {
                    //5
                    PREPRE = PRE = "";
                    findNext_NextNext(iWord, words);
                    return;
                }
                else
                {
                    TOKEN = regexCLOSE.Replace(token, "");
                    //6
                    if (TOKEN.Length < token.Length)
                    {
                        //7
                        NEXT = NEXTNEXT = "";
                        return;
                    }
                    else
                    {
                        //8
                        findPre_PrePre(iWord, words);
                        return;
                    }
                }
            }
        }

        private void findPre_PrePre(int iWord, string[] words)
        {
            if (iWord > 0)
            {
                //8
                PRE = words[iWord - 1].Trim();
                string preSPEC, preClose, preOpen;
                preSPEC = regexSPEC.Replace(PRE, "");
                preClose = regexCLOSE.Replace(PRE, "");
                preOpen = regexOPEN.Replace(PRE, "");

                bool isUpper = char.IsUpper(PRE[0]) ? true : false;
                //9
                // Theo chũ trương
                if (preSPEC.Length < PRE.Length || preClose.Length < PRE.Length || (isUpper && iWord != 1))
                {

                    PRE = PREPRE = "";
                    findNext_NextNext(iWord, words);
                    return;
                }
                else
                {
                    //10
                    if (preOpen.Length < PRE.Length)
                    {
                        PRE = preOpen;
                        //11
                        PREPRE = "";
                        //12
                        checkTokenContainsClose_FindNext(iWord, words);
                    }
                    //13
                    else
                    {
                        if (iWord > 1)
                        {
                            PREPRE = words[iWord - 2].Trim();
                            string prepreSPEC, prepreClose, prepreOpen;
                            prepreSPEC = regexSPEC.Replace(PREPRE, "");
                            prepreClose = regexCLOSE.Replace(PREPRE, "");
                            prepreOpen = regexOPEN.Replace(PREPRE, "");

                            isUpper = char.IsUpper(PREPRE[0]) ? true : false;
                            //14
                            if (prepreSPEC.Length < PREPRE.Length || prepreClose.Length < PREPRE.Length || (isUpper && iWord != 2))
                            {

                                //5
                                PREPRE = "";
                                //12
                                checkTokenContainsClose_FindNext(iWord, words);
                            }
                            else if (prepreOpen.Length < PREPRE.Length)
                            {
                                PREPRE = prepreOpen;
                                //11
                                //PREPRE = "";
                                //12
                                //checkTokenContainsClose_FindNext(iWord, words);
                            }
                            //12
                            checkTokenContainsClose_FindNext(iWord, words);
                        }
                        else
                        {
                            PREPRE = "";
                            //12
                            checkTokenContainsClose_FindNext(iWord, words);
                        }
                    }
                }
            }
            else
            {
                PRE = PREPRE = "";
                findNext_NextNext(iWord, words);
                return;
            }
        }

        private void checkTokenContainsClose_FindNext(int iWord, string[] words)
        {
            string tokenClose = regexCLOSE.Replace(words[iWord].Trim(), "");

            if (tokenClose.Length < words[iWord].Trim().Length)
                return;
            else
            {
                findNext_NextNext(iWord, words);
                return;
            }
        }

        private void findNext_NextNext(int iWord, string[] words)
        {
            if (iWord < words.Length - 1)
            {
                //15
                NEXT = words[iWord + 1].Trim();
                string nextSPEC, nextClose, nextOpen;
                nextSPEC = regexSPEC.Replace(NEXT, "");
                nextClose = regexCLOSE.Replace(NEXT, "");
                nextOpen = regexOPEN.Replace(NEXT, "");

                bool isUpper = char.IsUpper(NEXT[0]) ? true : false;
                //16
                if (nextSPEC.Length < NEXT.Length || nextOpen.Length < NEXT.Length || isUpper)
                {

                    //17

                    NEXT = NEXTNEXT = "";
                    return;
                }
                else
                {
                    //18
                    if (nextClose.Length < NEXT.Length)
                    {
                        NEXT = nextClose;
                        //19
                        NEXTNEXT = "";
                        return;
                    }
                    else
                    {
                        //20
                        if (iWord < words.Length - 2)
                        {
                            NEXTNEXT = words[iWord + 2].Trim();
                            string nextnextSPEC, nextnextOpen, nextnextClose;
                            nextnextSPEC = regexSPEC.Replace(NEXTNEXT, "");
                            nextnextOpen = regexOPEN.Replace(NEXTNEXT, "");
                            nextnextClose = regexCLOSE.Replace(NEXTNEXT, "");
                            isUpper = char.IsUpper(NEXTNEXT[0]) ? true : false;
                            //21
                            if (nextnextSPEC.Length < NEXTNEXT.Length || nextnextOpen.Length < NEXTNEXT.Length || isUpper)
                            {
                                //17
                                NEXTNEXT = "";
                                //22
                                return;
                            }
                            else if (nextnextClose.Length < NEXTNEXT.Length)
                            {
                                NEXTNEXT = nextnextClose;
                                //19
                                return;
                            }
                        }
                        else
                        {
                            NEXTNEXT = "";
                            return;
                        }
                    }
                }
            }
            else
            {
                NEXT = NEXTNEXT = "";
                return;
            }
        }
        public override string ToString()
        {
            if (PREPRE == null)
                PREPRE = "";
            if (PRE == null)
                PRE = "";
            if (NEXT == null)
                NEXT = "";
            if (NEXTNEXT == null)
                NEXTNEXT = "";
            string pp = PREPRE.Equals(Ngram.Instance.START_STRING) ?
                 "" : PREPRE;
            string p = PRE.Equals(Ngram.Instance.START_STRING) ?
                "" : PRE;
            string n = NEXT.Equals(Ngram.Instance.END_STRING) ?
                "" : NEXT;
            string nn = NEXTNEXT.Equals(Ngram.Instance.END_STRING) ?
                "" : NEXTNEXT;
            return string.Format("{0} {1} {2} {3} {4}", pp, p, TOKEN, n, nn).Trim();
        }
        public override bool Equals(object obj)
        {
            Context c;
            if (obj.GetType() != this.GetType())
                return false;
            else c = (Context)obj;
            //if (!PREPRE.Equals(c.PREPRE))
            //    return false;
            //if (!PRE.Equals(c.PRE))
            //    return false;
            //if (!TOKEN.Equals(c.TOKEN))
            //    return false;
            //if (!NEXT.Equals(c.NEXT))
            //    return false;
            //if (!NEXTNEXT.Equals(c.NEXTNEXT))
            //    return false;
            //return true;
            if (TOKEN.Equals(c.TOKEN))
                return true;
            return false;
        }
    }
}
