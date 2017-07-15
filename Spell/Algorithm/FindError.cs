using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;

namespace Spell.Algorithm
{
    class FindError
    {
        // Từ điển với key là ngữ cảnh, value là range lỗi
        public Dictionary<Context, Word.Range> dictContext_ErrorRange;

        // Từ điển với key là ngữ cảnh, value là từ lỗi
        // dùng để bỏ qua lỗi khi người dùng đã chỉnh sửa lỗi đó. 
        public Dictionary<Context, string> dictContext_ErrorString;

        // Danh sách câu được select trong document
        private Word.Sentences _SelectionSentences;

        // Toàn bộ câu trong document
        private Word.Sentences _AllSentences = Globals.ThisAddIn.Application.ActiveDocument.Sentences;

        // Cờ đánh dấu có phải chế độ "Đang đánh máy" hay không
        private const int IS_TYPING_TYPE = 0;

        // Hằng số đánh dấu đang ở chế độ kiểm lỗi chính tả và ngữ cảnh
        private const int WRONG_RIGHT_ERROR = 0;
        // Hằng số đánh dấu đang ở chế độ kiểm lỗi chính tả
        private const int WRONG_ERROR = 1;
        // Hằng số đánh dấu đang ở chế độ kiểm lỗi ngữ cảnh
        private const int RIGHT_ERROR = 2;

        // Ngữ cảnh chứa lỗi đầu tiên trong danh sách
        public Context FirstError_Context
        {
            get
            {
                return FindFirstError_Context();
            }
        }
        // Tìm ngữ cảnh đầu tiên trong danh sách
        private Context FindFirstError_Context()
        {
            int min = dictContext_ErrorRange.First().Value.Start;
            Context ret = dictContext_ErrorRange.First().Key;

            // Ngữ cảnh đầu tiên là ngữ cảnh có range.Start nhỏ nhất
            foreach(var item in dictContext_ErrorRange)
                if(item.Value.Start < min)
                {
                    min = item.Value.Start;
                    ret = item.Key;
                }
            return ret;
        }
        //public Context SelectedError_Context { get; set; }
        public bool StopFindError { get; set; }
        private static FindError instance = new FindError();
        private int _typeFindError = 0;
        private int _typeError = 0;
        private bool _isResume = false;
        public int ISentence { get; set; }
        private int Start { get; set; }
        private int End { get; set; }
        private string _Sentence { get; set; }
        private string iWord { get; set; }
        private string iWordReplaced { get; set; }
        private string[] words { get; set; }
        private string[] originWords { get; set; }
        private int length { get; set; }
        private bool isError { get; set; }
        private Context tmpContext { get; set; }
        private HashSet<string> hSetCand { get; set; }
        private Word.Range range { get; set; }
        private Regex r = new Regex(StringConstant.Instance.patternCheckSpecialChar);
        private Regex regexHasWord = new Regex(StringConstant.Instance.patternHasWord);
        private int _countSentence;
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
            dictContext_ErrorRange = new Dictionary<Context, Word.Range>();
            dictContext_ErrorString = new Dictionary<Context, string>();
            tmpContext = new Context();
            hSetCand = new HashSet<string>();
        }
        public static FindError Instance
        {
            get
            {
                return instance;
            }
        }
        public void Clear()
        {
            StopFindError = false;
            if (dictContext_ErrorRange.Count > 0)
            {
                dictContext_ErrorRange.Clear();
                dictContext_ErrorString.Clear();
            }
        }
        public void createValue(int typeFindError, int typeError)
        {
            if (dictContext_ErrorRange.Count > 0)
            {
                dictContext_ErrorRange.Clear();
                dictContext_ErrorString.Clear();
            }
            _typeFindError = typeFindError;
            _typeError = typeError;
            _isResume = false;
            StopFindError = false;
        }
        public void setResume()
        {
            _isResume = true;
        }
        public int CountError
        {
            get
            {
                return dictContext_ErrorRange.Count;
            }
        }
        /// <summary>
        /// Kiểm tra dictErrorRange có chứa context và có range.Start == start hay không
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public bool IsContainError(Context context, int start)
        {
            foreach(var item in dictContext_ErrorRange)
                if (item.Value.Start == start)
                    if (item.Key.Equals(context))
                        return true;
            return false;            
        }
        public void startFindError()
        {
            Find(_typeFindError, _typeError);
        }

        private void FindISentence()
        {
            int min = 1;
            int max = _AllSentences.Count;
            int key = _SelectionSentences[1].Start;
            while (min <= max)
            {
                ISentence = (min + max) / 2;
                if (key == _AllSentences[ISentence].Start)
                {
                    return;
                }
                else if (key < _AllSentences[ISentence].Start)
                {
                    max = ISentence - 1;
                }
                else
                {
                    min = ISentence + 1;
                }
            }
        }
        public void Find_Typing(int typeError)
        {
            //Lấy danh sách câu đang được chọn
            _SelectionSentences = Globals.ThisAddIn.Application.Selection.Sentences;
            //Trong những câu đang được chọn, lấy câu đầu tiên
            //------
            //......
            //Kiểm tra cho đến khi gặp từ còn đang đánh máy dang dở
            //Tức là cho đến khi gặp ký tự xuống dòng

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeFindError"></param>
        /// <param name="typeError"></param>
        /// <param name="isResume"></param>
        public void Find(int typeFindError, int typeError)
        {
            try
            {
                // Cờ dùng để đánh dấu có select range câu đang kiểm tra hay không
                bool isSelected = true;
                Word.Range selectionRange = Globals.ThisAddIn.Application.Selection.Range;
                //dùng để tìm ISentence
                _SelectionSentences = Globals.ThisAddIn.Application.Selection.Sentences;

                //nếu bắt đầu và kết thúc bằng nhau
                //kiểm tra từ câu chứa vị trí con trỏ đến cuối văn bản
                if (selectionRange.Start == selectionRange.End)
                {
                    FindISentence();
                    isSelected = true;
                    _SelectionSentences = _AllSentences;
                }
                else {
                    //ngược lại
                    //kiểm tra những câu được chọn
                    //curSentences = Globals.ThisAddIn.Application.Selection.Sentences;
                    isSelected = false;
                    ISentence = 1;
                }
                isError = false;
                _countSentence = _SelectionSentences.Count;
                //lấy toàn bộ danh sách các từ trong Active Document, để lấy được ngữ cảnh
                while (true)
                {
                    if (StopFindError)
                        break;
                    for (; ISentence <= _countSentence; ISentence++)
                    {
                        if (StopFindError)
                            break;
                        range = _SelectionSentences[ISentence];
                        _Sentence = range.Text.TrimEnd();

                        // Kiểm tra trường hợp thiếu khoảng trắng giữa 2 dấu câu liên tiếp
                        // Tôi có bút chì, bút bi...; tất cả chúng đều được mua tháng trước

                        Match mHasWord = regexHasWord.Match(_Sentence);
                        if (!mHasWord.Success)
                        {
                            // Kiểm lỗi từ đầu, vì có thể không cắt được câu đang có lỗi

                            range.Text = " " + range.Text;
                            if (isSelected)
                                range.Select();
                            MessageBox.Show(SysMessage.Instance.Message_Space_Expected);
                            ISentence = 0;
                            continue;
                        }
                        Start = range.Start;
                        words = _Sentence.Split(' ');
                        originWords = _Sentence.Split(' ');
                        if (typeFindError != IS_TYPING_TYPE && isSelected)
                            range.Select();
                        //số lượng các từ trong cụm
                        length = words.Length;
                        //duyệt qua từng từ trong cụm
                        for (int i = 0; i < length; i++)
                        {
                            if (StopFindError)
                                break;
                            iWord = words[i];
                            tmpContext.TOKEN = iWord.Trim().ToLower();
                            if (tmpContext.TOKEN.Length < 1)
                            {
                                Start += iWord.Length + 1;
                                continue;
                            }
                            //Kiểm tra các kí tự đặc biệt, mail, số, tên riêng, viết tắt

                            Match m = r.Match(tmpContext.TOKEN);
                            if (m.Success)
                            {
                                Start += iWord.Length + 1;
                                continue;
                            }
                            //viết hoa giữa câu
                            if (char.IsUpper(iWord.Trim()[0]) && i != 0)
                            {
                                Start += iWord.Length + 1;
                                continue;
                            }
                            else
                            {
                                //xác định ngữ cảnh
                                Context context = new Context(i, words);

                                iWordReplaced = Regex.Replace(context.TOKEN, StringConstant.Instance.patternSignSentence, "");
                                //nếu loại bỏ ký tự đặc biệt nằm giữa hay đầu từ, ví dụ email, thì bắt đầu vòng lặp sau
                                if (!iWord.Contains(iWordReplaced)
                                    //nếu loại bỏ ký tự đặc biệt xong, độ dài của từ bằng 0, thì bắt đầu vòng lặp sau
                                    || iWordReplaced.Length == 0)
                                {
                                    Start += words[i].Length + 1;
                                    continue;
                                }
                                if (words[i].Length != iWordReplaced.Length)
                                    context.TOKEN = iWordReplaced;
                                if (!iWord.Contains("\r"))
                                    Start += iWord.Length - iWordReplaced.Length;
                                End = Start + iWordReplaced.Length;

                                tmpContext.CopyForm(context);
                                //Kiểm tra trên từ điển âm tiết
                                if ((typeError == WRONG_RIGHT_ERROR || typeError == WRONG_ERROR) && !VNDictionary.getInstance.isSyllableVN(iWordReplaced))
                                {
                                    hSetCand.Clear();
                                    hSetCand = WrongWordCandidate.getInstance.createCandidate(context, false);
                                    if (hSetCand.Count > 0)
                                    {
                                        //tự động thay thế bằng candidate tốt nhất
                                        //tránh làm sai những gram phía sau
                                        words[i] = hSetCand.ElementAt(0);

                                        ////lấy ngữ cảnh gốc
                                        //context.getContext(i, originWords);
                                        isError = true;
                                        dictContext_ErrorRange.Add(context, (DocumentHandling.Instance.UnderlineWrongWord(context.TOKEN, Start, End)));
                                    }
                                }//end if wrong word

                                //kiểm tra token có khả năng sai ngữ cảnh hay k

                                else if ((typeError == WRONG_RIGHT_ERROR || typeError == RIGHT_ERROR) && !RightWordCandidate.getInstance.checkRightWord(context))
                                {
                                    if (i < length - 1)
                                    {
                                        context.getContext(i + 1, words);
                                        string tmpNext = "";
                                        //
                                        //thay words[i+1] bằng candidate tốt nhất
                                        hSetCand.Clear();
                                        hSetCand = RightWordCandidate.getInstance.createCandidate(context, false);
                                        if (hSetCand.Count > 0)
                                            tmpNext = hSetCand.ElementAt(0);
                                        context.CopyForm(tmpContext);
                                        context.NEXT = tmpNext;

                                        //kiểm tra words[i] bị sai có do ảnh hưởng của words[i+1] hay không
                                        if (!RightWordCandidate.getInstance.checkRightWord(context))
                                        {
                                            context.CopyForm(tmpContext);
                                            hSetCand.Clear();
                                            hSetCand = RightWordCandidate.getInstance.createCandidate(context, false);
                                            if (hSetCand.Count > 0)
                                            {
                                                //tự động thay thế bằng candidate tốt nhất
                                                //tránh làm sai những gram phía sau
                                                words[i] = hSetCand.ElementAt(0);
                                                ////lấy ngữ cảnh gốc
                                                //context.getContext(i, originWords);
                                                isError = true;
                                                dictContext_ErrorRange.Add(context, (DocumentHandling.Instance.UnderlineRightWord(context.TOKEN, Start, End)));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        hSetCand.Clear();
                                        hSetCand = RightWordCandidate.getInstance.createCandidate(context, false);
                                        if (hSetCand.Count > 0)
                                        {
                                            //tự động thay thế bằng candidate tốt nhất
                                            //tránh làm sai những gram phía sau
                                            words[i] = hSetCand.ElementAt(0);
                                            ////lấy ngữ cảnh gốc
                                            //context.getContext(i, originWords);
                                            isError = true;
                                            dictContext_ErrorRange.Add(context, (DocumentHandling.Instance.UnderlineRightWord(context.TOKEN, Start, End)));
                                        }
                                    }

                                }// end else if right word
                                if (typeFindError == IS_TYPING_TYPE)
                                {
                                    if (!isError)
                                    {
                                        if (dictContext_ErrorRange.ContainsKey(context))
                                        {
                                            dictContext_ErrorRange.Remove(context);
                                            DocumentHandling.Instance.RemoveUnderline_Mistake(context.TOKEN, Start, End);
                                        }
                                    }
                                }


                            }
                            Start += iWord.Length + 1;
                        }//end for: duyệt từng từ trong câu
                    }//end for: duyệt từng câu
                    if (typeFindError == IS_TYPING_TYPE)
                    {
                        Thread.Sleep(1000);
                    }
                    else {
                        StopFindError = true;
                        break; // break while true
                    }
                }//end while true

                // Sắp xếp lại danh sách lỗi theo Range.Start
                dictContext_ErrorRange.OrderBy(x => x.Value.Start).ToDictionary(x => x.Key, x => x.Value);

                foreach (var item in dictContext_ErrorRange)
                    dictContext_ErrorString.Add(item.Key, item.Value.Text);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

    }
}
