﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using System.Threading;

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
        private const int IS_TYPING_TYPE = 0;

        private const int WRONG_RIGHT_ERROR = 0;
        private const int WRONG_ERROR = 1;
        private const int RIGHT_ERROR = 2;
        public Context FirstError_Context { get; set; }
        public Context SelectedError_Context { get; set; }
        public bool StopFindError { get; set; }
        private static FindError instance = new FindError();
        private int _typeFindError = 0;
        private int _typeError = 0;
        private bool _isAutoChange = false;
        private bool isNoChange = false;
        public override string ToString()
        {
            string pp = FirstError_Context.PREPRE.Equals(Ngram.Instance.START_STRING) ?
                "" : FirstError_Context.PREPRE;
            string p = FirstError_Context.PRE.Equals(Ngram.Instance.START_STRING) ?
                "" : FirstError_Context.PRE;
            string n = FirstError_Context.NEXT.Equals(Ngram.Instance.END_STRING) ?
                "" : FirstError_Context.NEXT;
            string nn = FirstError_Context.NEXTNEXT.Equals(Ngram.Instance.END_STRING) ?
                "" : FirstError_Context.NEXTNEXT;
            return string.Format("{0} {1} {2} {3} {4}", pp, p, FirstError_Context.TOKEN, n, nn);
        }
        private FindError()
        {
            StopFindError = false;
            lstErrorRange = new Dictionary<Context, Word.Range>();
        }
        public static FindError Instance
        {
            get
            {
                return instance;
            }
        }
        public void createValue(int typeFindError, int typeError, bool isAutoChange)
        {
            _typeFindError = typeFindError;
            _typeError = typeError;
            _isAutoChange = isAutoChange;
        }
        public int CountError
        {
            get
            {
                return lstErrorRange.Count;
            }
        }
        public void GetSeletedContext(Word.Words words, Word.Sentences sentences)
        {
            SelectedError_Context = new Context(words, sentences);
            foreach (Context context in lstErrorRange.Keys)
            {
                if (context.Equals(SelectedError_Context))
                {
                    SelectedError_Context = context;
                    return;
                }
            }
        }
        public void startFindError()
        {
            showWrongWithSuggest(_typeFindError, _typeError, _isAutoChange);
        }
        public void showWrongWithSuggest(int typeFindError, int typeError, bool isAutoChange)
        {
            try
            {
                //sửa lỗi kiểm tra lần 2 không hiện được gợi ý
                lstErrorRange.Clear();
                FirstError_Context = null;
                //lấy toàn bộ danh sách các từ trong Active Document, để lấy được ngữ cảnh
                while (true)
                {
                    if (StopFindError)
                    {
                        break;
                    }
                    if (typeFindError == IS_TYPING_TYPE)
                        //chọn toàn bộ văn bản
                        curSentences = Globals.ThisAddIn.Application.Selection.Sentences;

                    else
                        //lấy danh sách câu dựa trên vùng được bôi đen
                        curSentences = Globals.ThisAddIn.Application.ActiveDocument.Sentences;
                    int start = 0, end = 0;
                    string iWord = "";
                    string token = "";
                    string wordInWords = "";
                    string[] words;
                    int length;
                    string prepre = "", pre = "", next = "", nextnext = "";
                    HashSet<string> hSetCand = new HashSet<string>();
                    Word.Range range = null;
                    for (int iSentence = 1; iSentence <= curSentences.Count; iSentence++)
                    {

                        if (StopFindError)
                        {
                            break;
                        }
                        words = curSentences[iSentence].Text.TrimEnd().Split(' ');
                        start = curSentences[iSentence].Start;
                        end = curSentences[iSentence].End;
                        range = Globals.ThisAddIn.Application.ActiveDocument.Range(start, end);
                        if (typeFindError != IS_TYPING_TYPE)
                            range.Select();
                        //số lượng các từ trong cụm
                        length = words.Length;
                        //duyệt qua từng từ trong cụm
                        for (int i = 0; i < length; i++)
                        {

                            if (StopFindError)
                            {
                                break;
                            }
                            iWord = words[i];
                            token = iWord.Trim().ToLower();
                            if (token.Length < 1)
                            {
                                start += iWord.Length + 1;
                                continue;
                            }
                            //Kiểm tra các kí tự đặc biệt, mail, số, tên riêng, viết tắt
                            Regex r = new Regex(StringConstant.Instance.patternCheckSpecialChar);
                            Match m = r.Match(token);
                            if (m.Success)
                            {
                                start += iWord.Length + 1;
                                continue;
                            }
                            //viết hoa giữa câu
                            if (char.IsUpper(iWord.Trim()[0]) && i != 0)
                            {
                                start += iWord.Length + 1;
                                continue;
                            }
                            else
                            {
                                //xác định ngữ cảnh
                                Context context = new Context(i, words);
                                wordInWords = Regex.Replace(context.TOKEN, StringConstant.Instance.patternSignSentence, "");
                                //nếu loại bỏ ký tự đặc biệt nằm giữa hay đầu từ, ví dụ email, thì bắt đầu vòng lặp sau
                                if (!iWord.Contains(wordInWords)
                                    //nếu loại bỏ ký tự đặc biệt xong, độ dài của từ bằng 0, thì bắt đầu vòng lặp sau
                                    || wordInWords.Length == 0)
                                {
                                    start += words[i].Length + 1;
                                    continue;
                                }
                                if (words[i].Length != wordInWords.Length)
                                    context.TOKEN = wordInWords;
                                if (!iWord.Contains("\r"))
                                    start += iWord.Length - wordInWords.Length;
                                end = start + wordInWords.Length;
                                prepre = context.PREPRE;
                                pre = context.PRE;
                                next = context.NEXT;
                                nextnext = context.NEXTNEXT;
                                //Kiểm tra nếu không phải là từ Việt Nam
                                //Thì highLight
                                isNoChange = false;
                                if (typeFindError == IS_TYPING_TYPE)
                                {
                                    foreach (Context iContext in lstErrorRange.Keys)
                                        if (iContext.Equals(context))
                                        {
                                            isNoChange = true;
                                            hSetCand = Candidate.getInstance.selectiveCandidate(context);
                                            if (hSetCand.Count > 0)
                                                words[i] = hSetCand.ElementAt(0);
                                            break;
                                        }
                                }
                                if (isNoChange)
                                    continue;
                                if ((typeError == WRONG_RIGHT_ERROR || typeError == WRONG_ERROR) && !VNDictionary.getInstance.isSyllableVN(wordInWords))
                                {

                                    if (isAutoChange)
                                    {
                                        hSetCand.Clear();
                                        hSetCand = WrongWordCandidate.getInstance.createCandidate(context, false);
                                        if (hSetCand.Count > 0)
                                        {
                                            //tự động thay thế bằng candidate tốt nhất
                                            //tránh làm sai những gram phía sau
                                            words[i] = hSetCand.ElementAt(0);
                                            if (FirstError_Context == null)
                                                FirstError_Context = context;

                                            lstErrorRange.Add(context, (DocumentHandling.Instance.HighLight_MistakeWrongWord(start, end)));
                                        }
                                    }
                                    else {
                                        if (FirstError_Context == null)
                                            FirstError_Context = context;
                                        lstErrorRange.Add(context, (DocumentHandling.Instance.HighLight_MistakeWrongWord(start, end)));
                                    }
                                }//end if wrong word
                                 //kiểm tra token có khả năng sai ngữ cảnh hay k

                                else if ((typeError == WRONG_RIGHT_ERROR || typeError == RIGHT_ERROR) && !RightWordCandidate.getInstance.checkRightWord(context))
                                {
                                    if (isAutoChange)
                                    {
                                        context.PRE = token;
                                        context.TOKEN = next;
                                        context.NEXT = nextnext;
                                        string tmpNext = "";
                                        //
                                        //thay words[i+1] bằng candidate tốt nhất
                                        hSetCand.Clear();
                                        hSetCand = Candidate.getInstance.selectiveCandidate(context);
                                        if (hSetCand.Count > 0)
                                            tmpNext = hSetCand.ElementAt(0);
                                        context.PRE = pre;
                                        context.TOKEN = token;
                                        context.NEXT = tmpNext;
                                        //kiểm tra words[i] bị sai có do ảnh hưởng của words[i+1] hay không
                                        if (!RightWordCandidate.getInstance.checkRightWord(context))
                                        {

                                            context.PRE = pre;
                                            context.TOKEN = token;
                                            context.NEXT = next;
                                            hSetCand.Clear();
                                            hSetCand = Candidate.getInstance.selectiveCandidate(context);
                                            if (hSetCand.Count > 0)
                                            {
                                                //tự động thay thế bằng candidate tốt nhất
                                                //tránh làm sai những gram phía sau
                                                words[i] = hSetCand.ElementAt(0);
                                                if (FirstError_Context == null)
                                                    FirstError_Context = context;
                                                lstErrorRange.Add(context, (DocumentHandling.Instance.HighLight_MistakeRightWord(start, end)));
                                            }
                                        }
                                    }
                                    else {
                                        if (FirstError_Context == null)
                                            FirstError_Context = context;
                                        lstErrorRange.Add(context, (DocumentHandling.Instance.HighLight_MistakeRightWord(start, end)));
                                    }
                                }// end else if right word
                            }
                            start += iWord.Length + 1;
                        }//end for: duyệt từng từ trong câu
                    }//end for: duyệt từng câu
                    if (typeFindError == IS_TYPING_TYPE)
                        Thread.Sleep(1000);
                    else
                        break;
                }//end while true
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

    }
}
