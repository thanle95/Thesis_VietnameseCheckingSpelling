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
        public Dictionary<Context, Word.Range> lstErrorRange = new Dictionary<Context, Word.Range>();
        private Word.Sentences curSentences;
        public List<string> MySentences
        {
            get; set;
        }
        public Context FirstError_Context { get; set; }
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
        public Dictionary<Context, Word.Range> startFindError(int typeFindError)
        {
            lstErrorRange = new Dictionary<Context, Word.Range>();
            Dictionary<Context, Word.Range> ret = showWrongWithSuggest(typeFindError);
            return ret;
        }
        public Dictionary<Context, Word.Range> showWrongWithSuggest(int typeFindError)
        {
            try
            {
                //FirstError_Context; ;
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
                foreach (string mySentence in MySentences)
                {
                    string[] words = mySentence.Trim().Split(' ');

                    //số lượng các từ trong cụm
                    int length = words.Length;
                    //duyệt qua từng từ trong cụm
                    for (int i = 0; i < length; i++)
                    {
                        string token = words[i].Trim().ToLower();
                        if (token.Length < 1)
                        {
                            //lstErrorRange.Add(countWord, (DocumentHandling.Instance.HighLight_MistakeWrongWord(token, curSentences, countWord)));
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
                                if (FirstError_Context == null)
                                    FirstError_Context = context;
                                lstErrorRange.Add(context, (DocumentHandling.Instance.HighLight_MistakeWrongWord(context, curSentenceList.ElementAt(0))));
                                HashSet<string> hsetCand = WrongWordCandidate.getInstance.createCandidate(context, false);
                                if (hsetCand.Count > 0)
                                    //tự động thay thế bằng candidate tốt nhất
                                    //tránh làm sai những gram phía sau
                                    words[i] = hsetCand.ElementAt(0);
                            }
                            else
                            {
                                //kiểm tra token có khả năng sai ngữ cảnh hay k

                                if (!RightWordCandidate.getInstance.checkRightWord(context))
                                {
                                    context.PRE = token;
                                    context.TOKEN = next;
                                    context.NEXT = nextnext;
                                    string tmpNext = "";
                                    HashSet<string> hsetCandNext = Candidate.getInstance.selectiveCandidate(context);
                                    if (hsetCandNext.Count > 0)
                                        tmpNext = hsetCandNext.ElementAt(0);
                                    context.PRE = pre;
                                    context.TOKEN = token;
                                    context.NEXT = tmpNext;

                                    if (!RightWordCandidate.getInstance.checkRightWord(context))
                                    {
                                        if (FirstError_Context == null)
                                            FirstError_Context = context;
                                        context.PRE = pre;
                                        context.TOKEN = token;
                                        context.NEXT = next;
                                        lstErrorRange.Add(context, (DocumentHandling.Instance.HighLight_MistakeRightWord(context, curSentenceList.ElementAt(0))));
                                        HashSet<string> hsetCand = Candidate.getInstance.selectiveCandidate(context);
                                        if (hsetCand.Count > 0)
                                            //tự động thay thế bằng candidate tốt nhất
                                            //tránh làm sai những gram phía sau
                                            words[i] = hsetCand.ElementAt(0);
                                    }
                                }
                            }// end else right word
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
