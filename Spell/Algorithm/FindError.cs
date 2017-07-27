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

        // Cờ đánh dấu dừng kiểm lỗi
        public bool IsStopFindError { get; set; }

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

        // Kiểu kiểm lỗi
        private int _typeFindError = 0;
        // Kiểu lỗi
        private int _typeError = 0;

        // Vị trí câu kiểm lỗi trong document
        private int _iSentence;
        // Giữ range.Start
        private int _start;
        // Giữ range.End
        private int _end;
        // Giữ câu hiện tại đang kiểm lỗi
        private string _sentence;
        // Giữ từ đang kiểm lỗi trong câu
        private string _iWord;
        // Giữ từ đang kiểm lỗi sau khi bỏ dấu câu
        private string _iWordReplaced;

        // Mảng chứa những từ trong câu đang kiểm lỗi
        // có thể thay đổi trong quá trình kiểm tra
        private string[] _words;
        // Mảng chứa những từ trong câu đang kiểm lỗi
        // không thay đổi trong quá trình kiểm tra
        private string[] _originWords;
        // Độ dài câu hiện tại
        private int _length;

        // Ngữ cảnh lỗi hiện tại
        // không thay đổi trong quá trình kiểm tra lỗi đó
        private Context _originalContext;
        // Hashset chứa những gợi ý cho lỗi đang kiểm tra
        private HashSet<string> _hSetCand;

        // Range lỗi hiện tại
        private Word.Range _range;
        // Regex chứa những ký tự đặc biệt
        // như email, link, ...
        private Regex _regexCheckSpecial = new Regex(StringConstant.Instance.patternCheckSpecialChar);
        // Regex kiểm tra có chứa từ hay không
        private Regex _regexHasWord = new Regex(StringConstant.Instance.patternHasWord);
        // Số câu cần kiểm tra
        private int _countSentence;

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
            foreach (var item in dictContext_ErrorRange)
                if (item.Value.Start < min)
                {
                    min = item.Value.Start;
                    ret = item.Key;
                }
            return ret;
        }
        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        private FindError()
        {
            IsStopFindError = false;
            dictContext_ErrorRange = new Dictionary<Context, Word.Range>();
            dictContext_ErrorString = new Dictionary<Context, string>();
            _originalContext = new Context();
            _hSetCand = new HashSet<string>();
        }
        /// <summary>
        /// Dùng property để tạo singleton
        /// </summary>
        public static FindError Instance
        {
            get
            {
                return instance;
            }
        }
        private static FindError instance = new FindError();
        /// <summary>
        /// Xóa dữ liệu khi nhấn nút xóa đánh dấu lỗi trên Ribbon
        /// </summary>
        public void Clear()
        {
            IsStopFindError = false;
            if (dictContext_ErrorRange.Count > 0)
            {
                dictContext_ErrorRange.Clear();
                dictContext_ErrorString.Clear();
            }
        }
        /// <summary>
        /// Tạo dữ liệu trước khi kiểm lỗi
        /// </summary>
        /// <param name="typeFindError"></param>
        /// <param name="typeError"></param>
        public void createValue(int typeFindError, int typeError)
        {
            if (dictContext_ErrorRange.Count > 0)
            {
                dictContext_ErrorRange.Clear();
                dictContext_ErrorString.Clear();
            }
            _typeFindError = typeFindError;
            _typeError = typeError;
            IsStopFindError = false;
        }
        // Trả về số lỗi có trong danh sách
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
            foreach (var item in dictContext_ErrorRange)
                if (item.Value.Start == start)
                    if (item.Key.Equals(context))
                        return true;
            return false;
        }

        /// <summary>
        /// Tìm vị trí câu đầu tiên để kiểm lỗi
        /// </summary>
        private int FindISentence(int key, bool isStart)
        {
            // Dùng binary sort để tìm kiếm dựa trên range.Start
            int min = 1;
            int max = _AllSentences.Count;
            int i = 1;
            int start, end;
            while (min <= max)
            {
                i = (min + max) / 2;

                if (isStart)
                {
                    start = _AllSentences[i].Start;
                    if (key == start)
                        return i;
                    else if (key < start)
                        max = i - 1;
                    else
                        min = i + 1;
                }
                else {
                    end = _AllSentences[i].End;
                    if (key == end)
                        return i;
                    else if (key < end)
                        max = i - 1;
                    else
                        min = i + 1;
                }
            }
            // Trường hợp key > [i].start và key < [i].end
            if (isStart && i - 1 > 0)
                if (_AllSentences[i - 1].End > key)
                    return i - 1;
            if (!isStart && i + 1 < _AllSentences.Count)
                if (_AllSentences[i + 1].Start < key)
                    return i + 1;
            return i;
        }
        public void Find_Typing(int typeError)
        {
            //Trong những câu đang được chọn, lấy câu đầu tiên
            //------
            //......
            //Kiểm tra cho đến khi gặp từ còn đang đánh máy dang dở
            //Tức là cho đến khi gặp ký tự xuống dòng

        }
        /// <summary>
        /// Tìm lỗi trong chế độ toàn văn bản
        /// </summary>
        public void Find()
        {
            try
            {
                Word.Range selectionRange = Globals.ThisAddIn.Application.Selection.Range;
                // Tìm ISentence
                _iSentence = FindISentence(selectionRange.Start, true);

                //nếu bắt đầu và kết thúc bằng nhau
                //kiểm tra từ câu chứa vị trí con trỏ đến cuối văn bản

                if (selectionRange.Start == selectionRange.End)
                {

                    _countSentence = _AllSentences.Count;
                }
                else {
                    //ngược lại
                    //kiểm tra những câu được chọn
                    //curSentences = Globals.ThisAddIn.Application.Selection.Sentences;
                    _countSentence = FindISentence(selectionRange.End, false);
                }

                for (; _iSentence <= _countSentence; _iSentence++)
                {
                    if (IsStopFindError)
                        break;
                    _range = _AllSentences[_iSentence];
                    _sentence = _range.Text.TrimEnd();

                    // Kiểm tra trường hợp thiếu khoảng trắng giữa 2 dấu câu liên tiếp
                    // Tôi có bút chì, bút bi...; tất cả chúng đều được mua tháng trước

                    Match mHasWord = _regexHasWord.Match(_sentence);
                    if (!mHasWord.Success)
                    {
                        // Kiểm lỗi từ đầu, vì có thể không cắt được câu đang có lỗi

                        //_range.Text = " " + _range.Text;
                        ////if (isSelected)
                        //    _range.Select();
                        //MessageBox.Show(SysMessage.Instance.Message_Space_Expected);
                        //_iSentence --;
                        continue;
                    }
                    _start = _range.Start;
                    _words = _sentence.Split(' ');
                    _originWords = _sentence.Split(' ');

                    // Nếu không phải chế độ bôi đen để kiểm tra văn bản
                    // và chế độ đang đánh máy
                    // thì select range đang chọn.
                    _range.Select();
                    //if (isSelected)
                    //    DocumentHandling.Instance.HighLight(_range.Start, _range.End);
                    //else
                    //    //if (_typeFindError != IS_TYPING_TYPE && !isSelected)


                    _length = _words.Length;
                    // Duyệt qua từng từ trong câu.
                    for (int i = 0; i < _length; i++)
                    {
                        if (IsStopFindError)
                            break;
                        _iWord = _words[i];
                        _originalContext.TOKEN = _iWord.Trim().ToLower();
                        if (_originalContext.TOKEN.Length == 0)
                        {
                            _start += _iWord.Length + 1;
                            continue;
                        }
                        // Kiểm tra các ký tự đặc biệt, mail, số, tên riêng, viết tắt
                        // Nếu có chứa những ký tự đó thì bỏ qua
                        Match m = _regexCheckSpecial.Match(_originalContext.TOKEN);
                        if (m.Success)
                        {
                            _start += _iWord.Length + 1;
                            continue;
                        }
                        // Viết hoa giữa câu thì bỏ qua vì là danh từ riêng
                        if (char.IsUpper(_iWord.Trim()[0]) && i != 0)
                        {
                            _start += _iWord.Length + 1;
                            continue;
                        }
                        else
                        {
                            // Xác định ngữ cảnh
                            Context context = new Context(i, _words);

                            _iWordReplaced = Regex.Replace(context.TOKEN, StringConstant.Instance.patternSignSentence, "");
                            //nếu loại bỏ ký tự đặc biệt nằm giữa hay đầu từ, ví dụ email, thì bắt đầu vòng lặp sau
                            if (!_iWord.Contains(_iWordReplaced)
                                //nếu loại bỏ ký tự đặc biệt xong, độ dài của từ bằng 0, thì bắt đầu vòng lặp sau
                                || _iWordReplaced.Length == 0)
                            {
                                _start += _words[i].Length + 1;
                                continue;
                            }
                            if (_words[i].Length != _iWordReplaced.Length)
                                context.TOKEN = _iWordReplaced;
                            if (!_iWord.Contains("\r"))
                                _start += _iWord.Length - _iWordReplaced.Length;
                            _end = _start + _iWordReplaced.Length;

                            _originalContext.CopyForm(context);
                            // Kiểm tra trên từ điển âm tiết
                            if ((_typeError == WRONG_RIGHT_ERROR || _typeError == WRONG_ERROR) && !VNDictionary.getInstance.isSyllableVN(_iWordReplaced))
                            {
                                // TODO: kiểm tra lại việc thay thế bằng candidate tốt nhất

                                // Kiểm tra từ hiện tại sai có phải do từ sau hay không
                                if (i < _length - 1)
                                {
                                    CheckError(context, i, false);
                                }
                                else
                                {
                                    // Là từ cuối câu nên không ảnh hưởng từ phía sau
                                    if (hasCandidate(context, false))
                                        addError(context, false);

                                }
                            }
                            // End if wrong word

                            // Kiểm tra token có khả năng sai ngữ cảnh hay k

                            else if ((_typeError == WRONG_RIGHT_ERROR || _typeError == RIGHT_ERROR) && !RightWordCandidate.getInstance.checkRightWord(context))
                            {
                                if (i < _length - 1)
                                {
                                    CheckError(context, i, true);
                                }
                                else
                                {
                                    // Là từ cuối câu nên không ảnh hưởng từ phía sau
                                    if (hasCandidate(context, true))
                                        addError(context, true);
                                }

                            }// end else if right word
                        }
                        _start += _iWord.Length + 1;
                    }//end for: duyệt từng từ trong câu
                    //DocumentHandling.Instance.HighLightCheckedRange(_range);
                }//end for: duyệt từng câu

                // Kết thúc kiểm lỗi
                IsStopFindError = true;

                //// Sắp xếp lại danh sách lỗi theo Range.Start
                //dictContext_ErrorRange.OrderBy(x => x.Value.Start).ToDictionary(x => x.Key, x => x.Value);

                foreach (var item in dictContext_ErrorRange)
                    dictContext_ErrorString.Add(item.Key, item.Value.Text);
            }
            catch
            {

            }
        }

        private bool hasCandidate(Context context, bool isRightError)
        {
            _hSetCand.Clear();
            if (isRightError)
                _hSetCand = RightWordCandidate.getInstance.createCandidate(context);
            else
                _hSetCand = WrongWordCandidate.getInstance.createCandidate(context);
            if (_hSetCand.Count > 0)
                return true;
            return false;
        }

        private void addError(Context context, bool isRightError)
        {
            if (isRightError)
                dictContext_ErrorRange.Add(context, (DocumentHandling.Instance.UnderlineRightWord(context.TOKEN, _start, _end)));
            else
                dictContext_ErrorRange.Add(context, (DocumentHandling.Instance.UnderlineWrongWord(context.TOKEN, _start, _end)));
        }

        private void CheckError(Context context, int i, bool isRightError)
        {
            // Ngữ cảnh chỉ có một từ nhưng thuộc trường hợp sai do ngữ cảnh
            if (context.WordCount == 1 && isRightError)
                return;
            if (_words[i + 1].Length > 0)
            {
                if (char.IsUpper(_words[i + 1][0]))
                {
                    if (hasCandidate(context, isRightError))
                        addError(context, isRightError);
                    return;
                }
                // Kiểm tra từ hiện tại có sai do từ tiếp theo hay không
                context.getContext(i + 1, _words);

                // Nếu ngữ cảnh của từ tiếp theo, có chứa từ hiện tại thì kiểm tra
                if (context.ToString().Contains(_originalContext.TOKEN))
                {
                    _hSetCand.Clear();
                    _hSetCand = Candidate.getInstance.createCandidate(context, isRightError);

                    // Từ thứ i + 1 có candidate thay thế
                    // nếu không, thì từ hiện tại là sai
                    if (_hSetCand.Count > 0)
                    {
                        context.CopyForm(_originalContext);


                        context.NEXT = _hSetCand.ElementAt(0);
                        // Dùng candidate tốt nhất làm ngữ cảnh
                        // kiểm tra từ hiện tại có sai do từ sau hay không

                        if (hasCandidate(context, isRightError))
                        {
                            // Từ hiện tại sai mà không phải do từ phía sau
                            // Tránh làm sai những gram phía sau
                            _words[i] = _hSetCand.ElementAt(0);

                            context.CopyForm(_originalContext);
                            addError(context, isRightError);
                        }
                        else
                        {
                            context.CopyForm(_originalContext);
                            // Nếu từ hiện tại có candidate thay thế thì thêm vào đó thành một lỗi
                            if (hasCandidate(context, isRightError))
                                addError(context, isRightError);
                        }
                    }
                    else
                    {
                        context.CopyForm(_originalContext);
                        // Nếu từ hiện tại có candidate thay thế thì thêm vào đó thành một lỗi
                        if (hasCandidate(context, isRightError))
                            addError(context, isRightError);
                    }
                }
                else
                {
                    context.CopyForm(_originalContext);
                    // Nếu từ hiện tại có candidate thay thế thì thêm vào đó thành một lỗi
                    if (hasCandidate(context, isRightError))
                        addError(context, isRightError);
                }
            }

        }
    }
}
