using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spell.Algorithm
{
    public class VNDictionary
    {
        //từ điển âm tiết
        public List<string> SyllableDict;
        //từ điển từ ghép
        public List<string> CompoundDict;

        private VNDictionary()
        {
            this.SyllableDict = new List<string>();
            this.SyllableDict = readSyllableDict();
            this.CompoundDict = new List<string>();
            this.CompoundDict = readCompoundWordDict();
        }

        private static VNDictionary instance = new VNDictionary();
        public static VNDictionary getInstance
        {
            get
            {
                return instance;
            }
        }


        /// <summary>
        /// Đọc từ điển tiếng âm tiết lên
        /// </summary>
        public List<string> readSyllableDict()
        {
            List<string> result = new List<string>();
            try
            {
                //properties vào fileName, chọn copy always
                string[] dictArr = File.ReadAllLines(@"Resources\SyllableDictByViet39K.txt");
                result = dictArr.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return result;
        }
        /// <summary>
        /// đọc từ điển từ ghép tiếng Việt
        /// </summary>
        /// <returns></returns>
        public List<string> readCompoundWordDict()
        {
            List<string> result = new List<string>(); try
            {
                string[] dictArr = File.ReadAllLines(@"Resources\newCompoundWordByViet39K.txt");
                result = dictArr.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return result;
        }
        /// <summary>
        /// Kiểm tra một token có là âm tiết tiếng Việt hay không
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool isSyllableVN(string token)
        {
            return this.SyllableDict.BinarySearch(token.ToLower()) >= 0; // Neu am tiet nay ko co trong tu dien 
        }
        /// <summary>
        /// Tìm X: trả về từ ghép liền trước token dạng X X+1
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HashSet<string> findCompoundVNWord_Xx(string token)
        {
            HashSet<string> hSetResult = new HashSet<string>();
            if (token.Length > 0)
                //duyệt qua tất cả trường hợp, với value là token
                foreach (KeyValuePair<string, List<string>> pair in CompoundWordVn.Instance.compoundWordVnDict)
                    //foreach (string i in pair.Value)
                    //{
                        //string[] iArr = i.Trim().Split(' ');
                        //if (iArr.Length == 1 && iArr[0].Equals(token))
                        if(pair.Value.Contains(token))
                            hSetResult.Add(pair.Key);
                    //}
            else
                hSetResult.Add("");
            return hSetResult;
        }
        /// <summary>
        /// trả về từ ghép liền sau token dạng X-1 X
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HashSet<string> findCompoundVNWord_xX(string token)
        {
            HashSet<string> hSetResult = new HashSet<string>();
            if (token.Length > 0 && CompoundWordVn.Instance.compoundWordVnDict.ContainsKey(token.Trim().ToLower()))
                //duyệt qua List<string> là value với key là token
                foreach (string i in CompoundWordVn.Instance.compoundWordVnDict[token.Trim().ToLower()])
                {
                    string[] iArr = i.Trim().Split(' ');
                    //từ ghép có 2 âm tiết dạng: token iArr[0]
                    if (iArr.Length == 1 && iArr[0].Length > 0)
                        hSetResult.Add(iArr[0]);
                }
            else
                hSetResult.Add("");
            return hSetResult;
        }
        /// <summary>
        /// trả về từ ghép dạng X-2 X-1 X
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HashSet<string> findCompoundVNWord_xxX(string w_2, string w_1)
        {
            HashSet<string> hSetResult = new HashSet<string>();
            if (w_2.Length > 0 && w_1.Length > 0 && CompoundWordVn.Instance.compoundWordVnDict.ContainsKey(w_2.Trim().ToLower()))
                //duyệt qua List<string> là value với key là token
                foreach (string i in CompoundWordVn.Instance.compoundWordVnDict[w_2.Trim().ToLower()])
                {
                    string[] iArr = i.Trim().Split(' ');
                    //từ ghép có 3 âm tiết dạng: w_2 w_1 iArr[1]
                    if (iArr.Length == 2 && iArr[0].Equals(w_1) && iArr[1].Length > 0)
                        hSetResult.Add(iArr[1]);
                }
            else
                hSetResult.Add("");

            return hSetResult;
        }
        /// <summary>
        /// trả về từ ghép dạng X-1 X X+1
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HashSet<string> findCompoundVNWord_xXx(string w_1, string _w_1)
        {
            HashSet<string> hSetResult = new HashSet<string>();
            if (_w_1.Length > 0 && w_1.Length > 0 && CompoundWordVn.Instance.compoundWordVnDict.ContainsKey(w_1.Trim().ToLower()))
                //duyệt qua List<string> là value với key là token
                foreach (string i in CompoundWordVn.Instance.compoundWordVnDict[w_1.Trim().ToLower()])
                {
                    string[] iArr = i.Trim().Split(' ');
                    //từ ghép có 3 âm tiết dạng: w_1 iArr[0] _w_1
                    if (iArr.Length == 2 && iArr[1].Equals(_w_1) && iArr[0].Length > 0)
                        hSetResult.Add(iArr[0]);
                }
            else
                hSetResult.Add("");
            return hSetResult;
        }
        /// <summary>
        /// trả về từ ghép dạng X X+1 X+2
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HashSet<string> findCompoundVNWord_Xxx(string _w_1, string _w_2)
        {
            HashSet<string> hSetResult = new HashSet<string>();
            if (_w_1.Length > 0 && _w_2.Length > 0)
                //duyệt qua tất cả trường hợp, với value là token
                foreach (KeyValuePair<string, List<string>> pair in CompoundWordVn.Instance.compoundWordVnDict)
                {
                    foreach (string i in pair.Value)
                    {
                        string[] iArr = i.Trim().Split(' ');
                        if (iArr.Length == 2 && iArr[0].Equals(_w_1) && iArr[1].Equals(_w_2))
                            hSetResult.Add(pair.Key);
                    }
                    //if (pair.Value.BinarySearch(token)!= -1)
                    //    hSetResult.Add(pair.Key);
                }
            else
                hSetResult.Add("");
            return hSetResult;
        }
        /// <summary>
        /// kiểm tra một từ có là từ láy tiếng Việt hay không
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public bool isReduplicativeWordsVN(string words)
        {
            return false;
        }
    }
}
