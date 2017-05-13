using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;

namespace Spell.Algorithm
{
    class FindError
    {
        public Dictionary<int, Word.Range> lstErrorRange = new Dictionary<int, Word.Range>();
        private Word.Sentences curSentences;
        public List<string> MySentences
        {
            get; set;
        }
        public int FirstError_CountWord { get; set; }
        private static FindError instance = new FindError();
        private FindError()
        {

        }
        public static FindError Instance
        {
            get
            {
                return instance;
            }
        }
        public Dictionary<int, Word.Range> startFindError(int typeFindError)
        {
            lstErrorRange = new Dictionary<int, Word.Range>();
            Dictionary<int, Word.Range> ret = showWrongWithSuggest(typeFindError);
            return ret;
        }
        public Dictionary<int, Word.Range> showWrongWithSuggest(int typeFindError)
        {
            try
            {
                FirstError_CountWord = -1;
                lstErrorRange.Clear();

                ////lấy toàn bộ danh sách các từ trong Active Document, để lấy được ngữ cảnh
                //lấy danh sách câu dựa trên vùng được bôi đen
                if (typeFindError == 0)
                    curSentences = Globals.ThisAddIn.Application.ActiveDocument.Sentences;
                else
                    curSentences = Globals.ThisAddIn.Application.Selection.Sentences;
                List<Word.Sentences> curSentenceList = new List<Word.Sentences>();
                curSentenceList.Add(curSentences);
                //với mỗi câu, tách thành từng cụm có liên quan mật thiết với nhau, như "", (),...
                MySentences = DocumentHandling.Instance.getPhrase(curSentenceList.ElementAt(0));
                //Xử lý từng cụm từ, vì mỗi cụm từ có liên quan mật thiết với nhau
                int countWord = 0;
                foreach (string mySentence in MySentences)
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
                        {
                            //lstErrorRange.Add(countWord, (DocumentHandling.Instance.HighLight_MistakeWrongWord(token, curSentences, countWord)));
                            countWord--;
                            continue;
                        }
                        //Kiểm tra các kí tự đặc biệt, mail, số, tên riêng, viết tắt
                        Regex r = new Regex(StringConstant.Instance.patternCheckSpecialChar);
                        Match m = r.Match(token);
                        if (m.Success || (char.IsUpper(words[i].Trim()[0]) /*&& i != 0*/))
                            continue;
                        else
                        {
                            //xác định ngữ cảnh
                            Context context = new Context(i, words);
                            string prepre = context.PREPRE, pre = context.PRE, next = context.NEXT, nextnext = context.NEXTNEXT;

                            //Kiểm tra nếu không phải là từ Việt Nam
                            //Thì highLight
                            if (!VNDictionary.getInstance.isSyllableVN(token))
                            {
                                if (FirstError_CountWord == -1)
                                    FirstError_CountWord = countWord;
                                lstErrorRange.Add(countWord, (DocumentHandling.Instance.HighLight_MistakeWrongWord(token, curSentenceList.ElementAt(0), countWord)));
                                HashSet<string> hsetCand = Candidate.getInstance.selectiveCandidate(prepre, pre, token, next, nextnext);
                                if (hsetCand.Count > 0)
                                    //tự động thay thế bằng candidate tốt nhất
                                    //tránh làm sai những gram phía sau
                                    words[i] = hsetCand.ElementAt(0);
                            }
                            else
                            {
                                //kiểm tra token có khả năng sai ngữ cảnh hay k

                                if (!RightWordCandidate.getInstance.checkRightWord(prepre, pre, token, next, nextnext))
                                {
                                    HashSet<string> hsetCandNext = Candidate.getInstance.selectiveCandidate("", token, next, nextnext, "");
                                    string tmpNext = "";
                                    if (hsetCandNext.Count > 0)
                                        tmpNext = hsetCandNext.ElementAt(0);
                                    if (!RightWordCandidate.getInstance.checkRightWord(prepre, pre, token, tmpNext, nextnext))
                                    {
                                        if (FirstError_CountWord == -1)
                                            FirstError_CountWord = countWord;
                                        lstErrorRange.Add(countWord, (DocumentHandling.Instance.HighLight_MistakeRightWord(token, curSentenceList.ElementAt(0), countWord)));
                                        HashSet<string> hsetCand = Candidate.getInstance.selectiveCandidate(prepre, pre, token, next, nextnext);
                                        if (hsetCand.Count > 0)
                                            //tự động thay thế bằng candidate tốt nhất
                                            //tránh làm sai những gram phía sau
                                            words[i] = hsetCand.ElementAt(0);
                                    }
                                }
                            }
                        }
                    }//end for: duyệt từ từng trong cụm
                }//end for: duyệt từ cụm

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            return lstErrorRange;
        }

    }
}
