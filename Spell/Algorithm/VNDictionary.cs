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
        public Dictionary<string, string> SyllableDict;
        //từ điển từ ghép
        public List<string> CompoundDict;
        //dùng từ điển chưa có key
        private string compound = @"Resources\sortedCompoundWordDict.txt";
        private string syll = @"Resources\SyllableDictByViet39K.txt";
        private string syllPath;
        private string compoundPath;
        private VNDictionary()
        {
            syllPath = AppDomain.CurrentDomain.BaseDirectory + syll;
            compoundPath = AppDomain.CurrentDomain.BaseDirectory + compound;

            this.SyllableDict = new Dictionary<string, string>();
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
        public Dictionary<string, string> readSyllableDict()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                //properties vào fileName, chọn copy always
                string[] dictArr = File.ReadAllLines(syllPath);
                result = dictArr.ToDictionary(x => x, x => "");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            return result;
        }
        /// <summary>
        /// đọc từ điển từ ghép tiếng Việt
        /// </summary>
        /// <returns></returns>
        public List<string> readCompoundWordDict()
        {
            List<string> result = new List<string>();
            try
            {
                string[] dictArr = File.ReadAllLines(compoundPath);
                result = dictArr.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
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
            return this.SyllableDict.ContainsKey(token.ToLower()); // Neu am tiet nay ko co trong tu dien 
        }
        /// <summary>
        /// Tìm X: trả về từ ghép liền trước token dạng X X+1
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        public HashSet<string> findCompoundVNWord_Xx(string next)
        {
            HashSet<string> hSetResult = new HashSet<string>();
            if (next.Length > 0)
            {
                //duyệt qua tất cả trường hợp, với value là token
                foreach (KeyValuePair<string, List<string>> pair in CompoundWordVn.Instance.compoundWordVnDict)
                    if (pair.Value.Contains(next))
                        hSetResult.Add(pair.Key);
            }
            else
                hSetResult.Add("");
            return hSetResult;
        }
        /// <summary>
        /// trả về từ ghép liền sau token dạng X-1 X
        /// </summary>
        /// <param name="pre"></param>
        /// <returns></returns>
        public HashSet<string> findCompoundVNWord_xX(string pre)
        {
            HashSet<string> hSetResult = new HashSet<string>();
            if (pre.Length > 0 && CompoundWordVn.Instance.compoundWordVnDict.ContainsKey(pre.Trim().ToLower()))
                //duyệt qua List<string> là value với key là token
                foreach (string i in CompoundWordVn.Instance.compoundWordVnDict[pre.Trim().ToLower()])
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
        public HashSet<string> findCompoundVNWord_xxX(string prepre, string pre)
        {
            HashSet<string> hSetResult = new HashSet<string>();
            if (prepre.Length > 0 && pre.Length > 0 && CompoundWordVn.Instance.compoundWordVnDict.ContainsKey(prepre.Trim().ToLower()))
                //duyệt qua List<string> là value với key là token
                foreach (string i in CompoundWordVn.Instance.compoundWordVnDict[prepre.Trim().ToLower()])
                {
                    string[] iArr = i.Trim().Split(' ');
                    //từ ghép có 3 âm tiết dạng: w_2 w_1 iArr[1]
                    if (iArr.Length == 2
                        && iArr[0].Equals(pre)
                        && iArr[1].Length > 0)
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
        public HashSet<string> findCompoundVNWord_xXx(string pre, string next)
        {
            HashSet<string> hSetResult = new HashSet<string>();
            if (next.Length > 0 && pre.Length > 0 && CompoundWordVn.Instance.compoundWordVnDict.ContainsKey(pre.Trim().ToLower()))
                //duyệt qua List<string> là value với key là token
                foreach (string i in CompoundWordVn.Instance.compoundWordVnDict[pre.Trim().ToLower()])
                {
                    string[] iArr = i.Trim().Split(' ');
                    //từ ghép có 3 âm tiết dạng: w_1 iArr[0] _w_1
                    if (iArr.Length == 2 && iArr[1].Equals(next) && iArr[0].Length > 0)
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
        public HashSet<string> findCompoundVNWord_Xxx(string next, string nextnext)
        {
            HashSet<string> hSetResult = new HashSet<string>();
            if (next.Length > 0 && nextnext.Length > 0)
                //duyệt qua tất cả trường hợp, với value là token
                foreach (KeyValuePair<string, List<string>> pair in CompoundWordVn.Instance.compoundWordVnDict)
                    foreach (string i in pair.Value)
                    {
                        string[] iArr = i.Trim().Split(' ');
                        //từ ghép có 3 âm tiết dạng: key next nextnexxt
                        if (iArr.Length == 2 && iArr[0].Equals(next) && iArr[1].Equals(nextnext))
                            hSetResult.Add(pair.Key);
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
