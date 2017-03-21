using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spell.Algorithm
{


    class NewUtility
    {
        private const double ALPHA = 0.00001;
        private static NewUtility instance = new NewUtility();
        private NewUtility()
        {

        }

        public static NewUtility Instance
        {
            get { return instance; }
        }
        private class Result
        {
            public List<List<string>> ListError
            {
                get; set;
            }
            public string Sentence { get; set; }

            public Result()
            {
                ListError = new List<List<string>>();
                Sentence = "";
            }
        }

        private class Probility
        {
            public double Prob
            {
                get; set;
            }
            public List<string> Key
            {
                get; set;
            }

            public Probility()
            {
                Prob = 0;
                Key = new List<string>();
            }
        }
        class ErrorAndNoGroupError
        {

            public string Error
            {
                get; set;
            }
            public int NoGroupError
            {
                get; set;
            }

            public ErrorAndNoGroupError()
            {
                NoGroupError = -1;
                Error = "";
            }

        }
        /// <summary>
        /// tách dấu tiếng Việt theo kiểu TeLex: đã --> ddax
        /// </summary>
        /// <param name="word">Từ cần tách dấu</param>
        /// <returns>Từ đã tách dấu tiếng Việt theo kiểu Telex</returns>
        private string extractSign(string word)
        {
            string ret = "";
            char textSign = ' ';
            bool haveSign = false;
            int iPos; // vị trí của kí tự thứ i trong Source
            int iPosPos; // vị trí của iPos trong vnCharacter

            for (int i = 0; i < word.Length; i++)
            {
                //indexOf: trả về vị trí đầu tiên tìm được
                iPos = StringConstant_old.Instance.source.IndexOf(word.ElementAt(i));
                if ((iPos >= 0) && (iPos < StringConstant_old.Instance._length))
                {
                    iPosPos = StringConstant_old.Instance.vnCharacter.IndexOf(StringConstant_old.Instance.dest
                            .ElementAt(iPos));
                    if ((iPosPos >= 0) && (iPosPos < StringConstant_old.Instance._length))
                    {
                        // bước chuyển â => aa
                        ret += StringConstant_old.Instance.vnCharacterExtractsign[iPosPos];
                    }
                    else {
                        ret += StringConstant_old.Instance.dest.ElementAt(iPos);
                    }
                    // bước gắn dấu
                    textSign = StringConstant_old.Instance.sign.ElementAt(iPos);
                    haveSign = true;
                }
                else {
                    iPosPos = StringConstant_old.Instance.vnCharacter.IndexOf(word.ElementAt(i));
                    if ((iPosPos >= 0) && (iPosPos < StringConstant_old.Instance._length))
                    {
                        ret += StringConstant_old.Instance.vnCharacterExtractsign[iPosPos];
                    }
                    else {
                        ret += word.ElementAt(i);
                    }
                }
            }
            if (haveSign)
                ret += textSign;
            return ret;
        }
        /// <summary>
        /// chuyển từ dạng Telex thô sang tiếng Việt: ddax --> đã
        /// </summary>
        /// <param name="word">từ gần gộp dấu</param>
        /// <returns></returns>
        private string makeCohere(string word)
        {
            string tmp = word;
            string ret = "", tmp2 = "";
            bool flag = false;
            int k, iSign;
            iSign = StringConstant_old.Instance.sign.IndexOf(tmp.ElementAt(tmp.Length - 1));
            if (iSign >= 0 && iSign < 5)
                tmp = tmp.Substring(0, tmp.Length - 1);
            for (int iTmp = 0; iTmp < tmp.Length; iTmp++)
            {
                //
                if (iTmp + 2 < tmp.Length)
                {
                    tmp2 = tmp.Substring(iTmp, 2);
                    for (int iVNCharacterExtractSign = 0; iVNCharacterExtractSign < StringConstant_old.Instance.vnCharacterExtractsign.Length; iVNCharacterExtractSign++)
                    {
                        if (tmp2
                                .Equals(StringConstant_old.Instance.vnCharacterExtractsign[iVNCharacterExtractSign]))
                        {
                            if (iVNCharacterExtractSign != 6)
                            { // khác "đ"
                                if (iSign >= 0 && iSign < 5)
                                {

                                    ret += StringConstant_old.Instance.source
                                            .ElementAt(StringConstant_old.Instance.dest
                                                    .IndexOf(StringConstant_old.Instance.vnCharacter
                                                            .ElementAt(iVNCharacterExtractSign))
                                                    + iSign);
                                    flag = true;
                                }
                                else
                                    ret += StringConstant_old.Instance.vnCharacter.ElementAt(iVNCharacterExtractSign);
                            }
                            else
                                ret += StringConstant_old.Instance.vnCharacter.ElementAt(6);
                            iTmp++;

                            break;
                        }
                    }
                }
                if (!flag)
                {
                    k = StringConstant_old.Instance.vowel.IndexOf(tmp.ElementAt(iTmp));
                    if (k >= 0 && k < StringConstant_old.Instance.vowel.Length)
                    {
                        if (iSign >= 0 && iSign < 5)
                            ret += StringConstant_old.Instance.source.ElementAt(k * 5 + iSign);
                        else
                            ret += tmp.ElementAt(iTmp);
                    }
                    else
                        ret += tmp.ElementAt(iTmp);
                }
                flag = false;

            }
            return ret;
        }
        /// <summary>
        /// Hàm sinh lỗi theo dấu tiếng Việt
        /// </summary>
        /// <param name="w">từ cần sinh lỗi</param>
        /// <returns>danh sách từ</returns>
        private List<string> gererateErrorSign(string w)
        {
            List<string> ret = new List<string>();
            string tmp = extractSign(w);
            string sign = tmp.ElementAt(tmp.Length - 1) + "";
            int t = StringConstant_old.Instance.sign.IndexOf(sign);
            if (t >= 0 && t < 5)
            {
                tmp = tmp.Substring(0, tmp.Length - 1);
                for (int i = 0; i < 5; i++)
                {
                    //MessageBox.Show(temp + StringConstant.Instance.sign.ElementAt(i));
                    string word = makeCohere(tmp + StringConstant_old.Instance.sign.ElementAt(i));
                    ret.Add(word);
                }
            }
            return ret;
        }

        private List<string> gererateErrorByTyping(string w)
        {
            List<string> result = new List<string>();
            List<string> insert = new List<string>();
            List<string> delete = new List<string>();
            List<string> replace = new List<string>();
            List<string> permute = new List<string>();

            string temp = extractSign(w);

            insert = errorByInsert(temp);
            delete = errorByDelete(temp);
            replace = errorByReplace(temp);
            permute = errorByPermute(temp);

            if (insert != null)
                for (int i = 0; i < insert.Capacity; i++)
                    result.Add(insert.ElementAt(i));
            if (delete != null)
                for (int i = 0; i < delete.Capacity; i++)
                    result.Add(delete.ElementAt(i));
            if (replace != null)
                for (int i = 0; i < replace.Capacity; i++)
                    result.Add(replace.ElementAt(i));
            if (permute != null)
                for (int i = 0; i < permute.Capacity; i++)
                    result.Add(permute.ElementAt(i));

            for (int i = 0; i < temp.Length; i++)
            {
                string haft = temp.Substring(0, i);
                string secondHaft = temp.Substring(i + 1, temp.Length);
                for (int j = 0; j < 2; j++)
                {
                    int k = StringConstant_old.Instance.keyBoard[j].IndexOf(temp.ElementAt(i));
                    if (k >= 0 && k < StringConstant_old.Instance.keyBoard[j].Length)
                    {
                        if (k > 0)
                            result.Add(makeCohere(haft
                                    + StringConstant_old.Instance.keyBoard[j].ElementAt(k - 1)
                                    + secondHaft));
                        if (k < StringConstant_old.Instance.keyBoard[j].Length - 1)
                            result.Add(makeCohere(haft
                                    + StringConstant_old.Instance.keyBoard[j].ElementAt(k + 1)
                                    + secondHaft));
                        break;
                    }
                }
            }

            return result;
        }

        private List<string> errorByPermute(string temp)
        {
            return null;
        }

        private List<string> errorByReplace(string temp)
        {
            return null;
        }

        private List<string> errorByDelete(string w)
        {
            List<string> r = new List<string>();
            // Giữ lại ký tự cuối
            string tmp = w;
            char endCh = tmp.ElementAt(tmp.Length - 1);

            /*
             * Xóa ký tự cuối
             */
            // Ký tự cuối cùng là dấu
            if (StringConstant_old.Instance.signError.IndexOf(endCh) >= 0)
            {
                r.Add(tmp.Substring(0, tmp.Length - 2) + endCh);
            }
            // Ký tự cuối cùng không phải là dấu
            else {
                r.Add(tmp.Substring(0, tmp.Length - 1));
            }
            return r;
        }

        private List<string> errorByInsert(string temp)
        {
            return null;
        }
        /// <summary>
        /// sinh ra cac candidate cho một chuỗi
        /// </summary>
        /// <param name="s"> chuỗi cần sinh</param>
        /// <returns></returns>
        private List<string> generateCandidates(string[] wordArr)
        {
            List<string> ret = new List<string>();
            ret = generateCandidatePerWord(wordArr[0]);
            return ret;
        }
        /// <summary>
        /// Sinh candidate cho một từ
        /// </summary>
        /// <param name="w"></param>
        /// <returns>Danh sách các phần tử string mà mỗi phần tử là một candidate</returns>
        public List<string> generateCandidatePerWord(string w)
        {
            List<string> result = new List<string>();
            Dictionary<string, int> d = Ngram.Instance.DictionaryForNgram;
            List<string> errorSign = gererateErrorSign(w);
            foreach (string i in errorSign)
                foreach (KeyValuePair<string, int> pair in d)
                {
                    if (pair.Key.ToLower().Equals(i.ToLower()))
                    {
                        result.Add(i);
                        break;
                    }
                }

            List<string> startErr = new List<string>();

            List<string> endErr = new List<string>();
            // for (int i = 0; i < MAX; i++) {
            // result[i] = startErr[i] = endErr[i] = " ";
            // }
            ErrorAndNoGroupError start = startError(w);

            ErrorAndNoGroupError end = endError(w, start.Error.Length);
            // Nếu phần đầu và phần cuối của w sau khi tách có độ dài nhỏ hơn w
            // Phải giữ lại phần ở giữa đó
            // Giữ mid: start
            String midW = "";
            StringConstant_old stringConstant = StringConstant_old.Instance;
            int startErrorLength = stringConstant.initConsonant.Length;
            int endErrorLength = stringConstant.endError.Length;
            if (start.NoGroupError != -1 && end.NoGroupError != -1)
            {
                int head = 0, tail = 0;

                for (int i = 0; i < startErrorLength; i++)
                {
                    if (stringConstant.initConsonant[i].Equals(start.Error))
                    {
                        head = stringConstant.initConsonant[i].Length;
                        break;
                    }
                }
                for (int i = 0; i < endErrorLength; i++)
                {
                    if (stringConstant.endError[i].Equals(end.Error))
                    {
                        tail = stringConstant.endError[i].Length;
                        break;
                    }
                }

                if ((head + tail) < w.Length)
                {
                    int mid = w.Length - (head + tail);
                    midW = w.Substring(head, mid + head);
                }
            }
            // Giữ mid: end
            if (start.NoGroupError == -1)
            {
                // startErr[0] = start.error;
                startErr.Add(start.Error);
            }
            else {
                // int j = 0;
                for (int i = 0; i < startErrorLength; i++)
                {
                    if (start.NoGroupError == stringConstant.initConsonantCheck[i])
                    {
                        // Xử lý sự kiện tìm thấy nhóm gây lỗi
                        // startErr[j++] = stringConstant.startError[i];
                        startErr.Add(stringConstant.initConsonant[i]);
                    }
                }
            }
            if (end.NoGroupError == -1)
            {
                // endErr[0] = end.error;
                endErr.Add(end.Error);
            }
            else {
                // int j = 0;
                for (int i = 0; i < endErrorLength; i++)
                {
                    if (end.NoGroupError == stringConstant.endErrorCheck[i])
                    {
                        // Xử lý sự kiện tìm thấy nhóm gây lỗi
                        // endErr[j++] = stringConstant.end_error[i];
                        endErr.Add(stringConstant.endError[i]);
                    }
                }
            }
            // System.out.println(startErr[0]);
            // int k = 0;
            // String[] temp = new String[5];
            string word = "";
            foreach (string i in startErr)
            {
                foreach (string j in endErr)
                {
                    if (!i.Equals(" ") && !j.Equals(" "))
                    {
                        word = i + midW + j;
                        // for (String iWord :
                        // this.stringConstant.getDanhSachTuGoc()) {
                        // if (word.Equals(iWord)) {
                        // result[k++] = word;
                        // }
                        // }
                        // temp = signError(word);
                        // for (int z = 0; z < 5; z++) {
                        // // result[k++] = temp[z];
                        // }
                        foreach (KeyValuePair<string, int> pair in Ngram.Instance.DictionaryForNgram)
                        {
                            if (pair.Key.Trim().Equals(word.Trim().ToLower()))
                            {
                                result.Add(word);
                                break;
                            }
                        }
                        // result[k++] = word;
                    }
                }
            }

            ////List<string> errorTyping = gererateErrorByTyping(w);
            ////foreach (string i in errorTyping)
            ////    foreach (string u in Ngram.getInstance().DictionaryForNgram.Keys)
            ////    {
            ////        if (u.Equals(i))
            ////        {
            ////            result.Add(i);
            ////            break;
            ////        }
            ////    }
            //foreach(string i in endErr)
            //{

            //}
            return result;
        }
        /// <summary>
        /// Sinh lỗi phụ âm đầu dựa vào những trường hợp thường gặp khi mắc lỗi, như tiếng địa phương
        /// </summary>
        /// <param name="word"></param>
        /// <returns>phụ âm đầu có khả năng sinh lỗi và số nhóm lỗi của nó trong startErrorCheck</returns>
        private ErrorAndNoGroupError startError(string word)
        {
            ErrorAndNoGroupError result = new ErrorAndNoGroupError();
            int l = word.Length;
            string temp = "";
            for (int i = 0; i < StringConstant_old.Instance.initConsonantCheck.Length; i++)
            {
                if (l >= 3)
                {
                    temp = word.Substring(0, 3);
                    if (temp.Equals(StringConstant_old.Instance.initConsonant[i]))
                    {
                        result.Error = temp;
                        result.NoGroupError = StringConstant_old.Instance.initConsonantCheck[i];
                        return result;
                    }
                }
                if (l >= 2)
                {
                    temp = word.Substring(0, 2);
                    if (temp.Equals(StringConstant_old.Instance.initConsonant[i]))
                    {
                        result.Error = temp;
                        result.NoGroupError = StringConstant_old.Instance.initConsonantCheck[i];
                        return result;
                    }
                }
                if (l >= 1)
                {
                    temp = word.Substring(0, 1);
                    if (temp.Equals(StringConstant_old.Instance.initConsonant[i]))
                    {
                        result.Error = temp;
                        result.NoGroupError = StringConstant_old.Instance.initConsonantCheck[i];
                        return result;
                    }
                }
                temp = "";
                if (temp.Equals(StringConstant_old.Instance.initConsonant[i]))
                {
                    result.Error = temp;
                    result.NoGroupError = StringConstant_old.Instance.initConsonantCheck[i];
                    return result;
                }

            }
            return result;

        }
        /// <summary>
        /// Sinh lỗi phụ âm cuối, dựa vào những trường hợp thường gặp, như tiếng địa phương
        /// </summary>
        /// <param name="word"></param>
        /// <param name="n">độ dài phụ âm đầu, có được từ phương thức startError</param>
        /// <returns>vần hay phụ âm cuối có khả năng sinh lỗi, và số nhóm lỗi trong endErrorCheck</returns>
        private ErrorAndNoGroupError endError(string word, int n)
        {
            ErrorAndNoGroupError result = new ErrorAndNoGroupError();
            int l = word.Length;
            string tmp = "";
            if (n == 0)
                tmp = word;
            else
                tmp = word.Substring(n, l - n);
            for (int i = 0; i < StringConstant_old.Instance.endErrorCheck.Length; i++)
            {

                if (tmp.Equals(StringConstant_old.Instance.endError[i]))
                {
                    result.Error = tmp;
                    result.NoGroupError = StringConstant_old.Instance.endErrorCheck[i];
                    return result;
                }

                else if (tmp.Length == 2)
                {
                    // ví dụ: tmp = "an", tmp1 = "n"
                    string tmp1 = tmp[1] + "";
                    if (tmp.Equals(StringConstant_old.Instance.endError[i]))
                    {
                        result.Error = tmp;
                        result.NoGroupError = StringConstant_old.Instance.endErrorCheck[i];
                        return result;
                    }
                }
                else if (tmp.Length == 3)
                {
                    //ví dụ tmp = "iêt", tmp1 = "t"
                    string tmp1 = tmp[2] + "";
                    if (tmp.Equals(StringConstant_old.Instance.endError[i]))
                    {
                        result.Error = tmp;
                        result.NoGroupError = StringConstant_old.Instance.endErrorCheck[i];
                        return result;
                    }
                    else
                    {
                        //ví dụ tmp = "ang", tmp1 = "ng"
                        tmp1 = tmp.Substring(1, 2);
                        if (tmp.Equals(StringConstant_old.Instance.endError[i]))
                        {
                            result.Error = tmp;
                            result.NoGroupError = StringConstant_old.Instance.endErrorCheck[i];
                            return result;
                        }
                    }
                }
                else if (tmp.Length == 4)
                {
                    //ví dụ tmp = "uyên", tmp1 = "n"
                    string tmp1 = tmp[3] + "";
                    if (tmp.Equals(StringConstant_old.Instance.endError[i]))
                    {
                        result.Error = tmp;
                        result.NoGroupError = StringConstant_old.Instance.endErrorCheck[i];
                        return result;
                    }
                    else
                    {
                        //ví dụ tmp = "oanh", tmp1 = "nh"
                        tmp1 = tmp.Substring(2, 2);
                        if (tmp.Equals(StringConstant_old.Instance.endError[i]))
                        {
                            result.Error = tmp;
                            result.NoGroupError = StringConstant_old.Instance.endErrorCheck[i];
                            return result;
                        }
                        else
                        {
                            //ví dụ tmp = "oanh", tmp1 = "anh"
                            tmp1 = tmp.Substring(1, 3);
                            if (tmp.Equals(StringConstant_old.Instance.endError[i]))
                            {
                                result.Error = tmp;
                                result.NoGroupError = StringConstant_old.Instance.endErrorCheck[i];
                                return result;
                            }
                        }
                    }
                }
            }
            return result;
        }
        /**
     * 
     * @param word
     *            : từ cần lọc có kèm theo ký tự như là ký tự cuối câu
     * @return từ sau khi lọc
     */
        private string purifyWord(string word)
        {
            string result = word;
            if (result.Length > 0)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    char ch = result.ElementAt(i);
                    if (isEndSymbol(ch + "") || isMiddleSymbol(ch) || isSymbol(ch))
                    {
                        result = result.Replace("" + ch, "");
                        i--;
                    }
                }
            }
            return result;
        }

        private bool isSymbol(char ch)
        {
            throw new NotImplementedException();
        }

        private bool isMiddleSymbol(char ch)
        {
            throw new NotImplementedException();
        }

        private bool isEndSymbol(string w)
        {
            Regex r = new Regex(StringConstant_old.Instance.patternSymbolstring);
            Match m = r.Match(w);
            if (m.Success)
                return true;
            return false;
        }

        /**
         *
         * @param p
         *            : văn bản đầu vào
         * @return văn bản sau khi lọc những thành phần như: email, địa chỉ web,...
         */
        private string purifyText(string p)
        {
            string result = "";
            string[] temp = new Regex("[" + " |," + StringConstant_old.Instance.emailCheck + "|"
                    + StringConstant_old.Instance.webCheck + "|" + StringConstant_old.Instance.patternSymbolstring
                    + "|" + StringConstant_old.Instance.patternEndSentenceCharacter + "|"
                    + StringConstant_old.Instance.patternMiddleSymbol + "|" + StringConstant_old.Instance.date + "|"
                     //+ StringConstant. + "|" + StringConstant.numbercheck
                     + "]").Split(p);


            int length = temp.Length;
            for (int i = 0; i < length; i++)
            {
                if (!temp[i].Equals(""))
                    result += temp[i] + " ";
            }
            return result;
        }
        /// <summary>
        /// Kiểm tra một từ có nằm trong từ điển tiếng việt hay không
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool isVietNameseWithoutContext(string word)
        {
            Dictionary<string, int> d = Ngram.Instance.DictionaryForNgram;
            //Nếu là symbol sẽ bỏ qua kiểm tra
            if (isEndSymbol(word))
                return true;
            //
            //cần cải tiến, sắp xếp lại dictionary để dùng binarySearch
            //
            foreach (KeyValuePair<string, int> pair in d)
            {
                if (pair.Key.Trim().ToLower().Equals(word.Trim().ToLower()))
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Kiểm tra một từ khi đứng cạnh những từ khác có nghĩa hay không
        /// từ đang xét nằm ở giữa
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public bool isVietNameseWithContext_midSentence(string w1, string wi, string w3)
        {
            List<string> candidates = generateCandidatePerWord(wi.ToLower());
            string key1 = (w1 + " " + wi).ToLower();
            string key2 = (wi + " " + w3).ToLower();
            double max = 0;
            if (Ngram.Instance.BiGramCount.ContainsKey(key1))
                max = Ngram.Instance.BiGramCount[key1];
            if (Ngram.Instance.BiGramCount.ContainsKey(key2))
                max += Ngram.Instance.BiGramCount[key2];
            foreach (string iCandidate in candidates)
            {
                string iKey1 = (w1 + " " + wi).ToLower();
                string iKey2 = (wi + " " + w3).ToLower();
                double tmp = 0;
                if (Ngram.Instance.BiGramCount.ContainsKey(iKey1))
                    tmp = Ngram.Instance.BiGramCount[iKey1];
                if (Ngram.Instance.BiGramCount.ContainsKey(iKey2))
                    tmp += Ngram.Instance.BiGramCount[iKey2];
                if (tmp > max)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Kiểm tra một từ khi đứng cạnh những từ khác có nghĩa hay không
        /// từ đang xét nằm ở cuối câu
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public bool isVietNameseWithContext_endSentence(string w1, string wi)
        {
            List<string> candidates = generateCandidatePerWord(wi.ToLower());
            string key = (w1 + " " + wi).ToLower();
            double max = 0;
            if (Ngram.Instance.BiGramCount.ContainsKey(key))
                max = Ngram.Instance.BiGramCount[key];
            foreach (string iCandidate in candidates)
            {
                string iKey = (w1 + " " + iCandidate).ToLower();
                double tmp = 0;
                if (Ngram.Instance.BiGramCount.ContainsKey(iKey))
                    tmp = Ngram.Instance.BiGramCount[iKey];
                if (tmp > max)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Kiểm tra một từ khi đứng cạnh những từ khác có nghĩa hay không
        /// từ đang xét nằm ở đầu câu
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public bool isVietNameseWithContext_beginSentence(string wi, string w2)
        {
            List<string> candidates = generateCandidatePerWord(wi.ToLower());
            string key = (wi + " " + w2).ToLower();
            double max = 0;
            if (Ngram.Instance.BiGramCount.ContainsKey(key))
                max = Ngram.Instance.BiGramCount[key];
            foreach (string iCandidate in candidates)
            {
                string iKey = (iCandidate + " " + w2).ToLower();
                double tmp = 0;
                if (Ngram.Instance.BiGramCount.ContainsKey(iKey))
                    tmp = Ngram.Instance.BiGramCount[iKey];
                if (tmp > max)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// In ra từng lỗi trên task pane
        /// </summary>
        /// <param name="sentence">câu hiện tại</param>
        /// <returns></returns>
        public List<string> runWithSuggestFixes(string sentence)
        {
            List<string> ret = new List<string>(), tmp= new List<string>();
            //
            //Chạy phương thức kiểm lỗi
            //
            string[] wordArr = new Regex("[ ,.!?;:…]").Split(sentence);
            tmp = generateCandidates(wordArr);
            foreach (string i in tmp)
                ret.Add(i);
            return ret;
        }
    }
}
