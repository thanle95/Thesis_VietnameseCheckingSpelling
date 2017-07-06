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
        private VNDictionary()
        {
            this.SyllableDict = new Dictionary<string, string>();
            this.CompoundDict = new List<string>();
        }
        public void runFirst()
        {
            this.SyllableDict = readSyllableDict();
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
                result = Properties.Resources.SyllableDictByViet39K.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList().ToDictionary(x => x, x => "");
                string[] uniArr;
                if (result.Count == 1)
                {
                    uniArr = result.Keys.First().Split('\n');
                    result = uniArr.ToDictionary(x => x, x => "");
                }
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
                result = Properties.Resources.sortedCompoundWordDict.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string[] uniArr;
                if (result.Count == 1)
                {
                    uniArr = result[0].Split('\n');
                    result = uniArr.ToList();
                }
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
        public HashSet<string> findCompoundVNWord_Xx(Context context)
        {
            HashSet<string> hSetResult = new HashSet<string>();
            if (context.NEXT.Length > 0)
            {
                //duyệt qua tất cả trường hợp, với value là token
                foreach (KeyValuePair<string, List<string>> pair in CompoundWordVn.Instance.compoundWordVnDict)
                    if (!pair.Key.ToLower().Equals(context.TOKEN.ToLower()) && pair.Value.Contains(context.NEXT) && Candidate.getInstance.IsLikeLy(context.TOKEN, pair.Key))
                        hSetResult.Add(pair.Key);
            }
            return hSetResult;
        }
        /// <summary>
        /// trả về từ ghép liền sau token dạng X-1 X
        /// </summary>
        /// <param name="pre"></param>
        /// <returns></returns>
        public HashSet<string> findCompoundVNWord_xX(Context context)
        {
            HashSet<string> hSetResult = new HashSet<string>();
            if (context.PRE.Trim().Length > 0 && CompoundWordVn.Instance.compoundWordVnDict.ContainsKey(context.PRE.Trim().ToLower()))
                //duyệt qua List<string> là value với key là token
                foreach (string i in CompoundWordVn.Instance.compoundWordVnDict[context.PRE.Trim().ToLower()])
                {
                    string[] iArr = i.Trim().Split(' ');
                    //từ ghép có 2 âm tiết dạng: token iArr[0]
                    if (iArr.Length == 1)
                        if (!iArr[0].ToLower().Equals(context.TOKEN.ToLower()) && iArr[0].Length > 0 && Candidate.getInstance.IsLikeLy(context.TOKEN, iArr[0]))
                            hSetResult.Add(iArr[0]);
                }
            return hSetResult;
        }
        /// <summary>
        /// trả về từ ghép dạng X-2 X-1 X
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HashSet<string> findCompoundVNWord_xxX(Context context)
        {
            HashSet<string> hSetResult = new HashSet<string>();
            if (context.PREPRE.Trim().Length > 0 && context.PRE.Trim().Length > 0 && CompoundWordVn.Instance.compoundWordVnDict.ContainsKey(context.PREPRE.Trim().ToLower()))
                //duyệt qua List<string> là value với key là token
                foreach (string i in CompoundWordVn.Instance.compoundWordVnDict[context.PREPRE.Trim().ToLower()])
                {
                    string[] iArr = i.Trim().Split(' ');
                    //từ ghép có 3 âm tiết dạng: w_2 w_1 iArr[1]
                    if (iArr.Length == 2)
                        if (!iArr[1].ToLower().Equals(context.TOKEN.ToLower())
                            && iArr[0].Equals(context.PRE.Trim())
                            && iArr[1].Length > 0
                            && Candidate.getInstance.IsLikeLy(context.TOKEN, iArr[1]))
                            hSetResult.Add(iArr[1]);
                }
            return hSetResult;
        }
        /// <summary>
        /// trả về từ ghép dạng X-1 X X+1
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HashSet<string> findCompoundVNWord_xXx(Context context)
        {
            HashSet<string> hSetResult = new HashSet<string>();
            if (context.NEXT.Trim().Length > 0 && context.PRE.Trim().Length > 0 && CompoundWordVn.Instance.compoundWordVnDict.ContainsKey(context.PRE.Trim().ToLower()))
                //duyệt qua List<string> là value với key là token
                foreach (string i in CompoundWordVn.Instance.compoundWordVnDict[context.PRE.Trim().ToLower()])
                {
                    string[] iArr = i.Trim().Split(' ');
                    //từ ghép có 3 âm tiết dạng: w_1 iArr[0] _w_1
                    if (iArr.Length == 2)
                        if (!iArr[0].ToLower().Equals(context.TOKEN.ToLower()) && iArr[1].Equals(context.NEXT.Trim()) && iArr[0].Length > 0 && Candidate.getInstance.IsLikeLy(context.TOKEN, iArr[0]))
                            hSetResult.Add(iArr[0]);
                }
            return hSetResult;
        }
        /// <summary>
        /// trả về từ ghép dạng X X+1 X+2
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HashSet<string> findCompoundVNWord_Xxx(Context context)
        {
            HashSet<string> hSetResult = new HashSet<string>();
            if (context.NEXT.Trim().Length > 0 && context.NEXTNEXT.Trim().Length > 0)
                //duyệt qua tất cả trường hợp, với value là token
                foreach (KeyValuePair<string, List<string>> pair in CompoundWordVn.Instance.compoundWordVnDict)
                    foreach (string i in pair.Value)
                    {
                        string[] iArr = i.Trim().Split(' ');
                        //từ ghép có 3 âm tiết dạng: key next nextnexxt
                        if (iArr.Length == 2)
                            if (!pair.Key.ToLower().Equals(context.TOKEN.ToLower()) && iArr[0].Equals(context.NEXT.Trim()) && iArr[1].Equals(context.NEXTNEXT.Trim()) && Candidate.getInstance.IsLikeLy(context.TOKEN, pair.Key))
                                hSetResult.Add(pair.Key);
                    }
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
