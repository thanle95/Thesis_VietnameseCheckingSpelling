using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spell.Algorithm
{
    public class Candidate
    {
        public double LIM_SIMILARITY
        {
            get
            {
                return 3.5;
            }
        }
        public double LIM_LANGUAGEMODEL
        {
            get
            {
                return 1E-6;
            }
        }

        private Candidate()
        {

        }
        private static Candidate instance = new Candidate();
        public static Candidate getInstance { get { return instance; } }
        /// <summary>
        /// Kiểm tra token là in hoa hay thường
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool Check_Majuscule(string token)
        {
            foreach (char c in StringConstant.Instance.VNAlphabetArr_UpperCase)
                if (token.Contains(c))
                    return true;
            return false;
        }

        public string toString(HashSet<string> hset)
        {
            string ret = "";
            foreach (string i in hset)
                ret += i + "\n";
            return ret;
        }
        public string toString(List<string> lst)
        {
            string ret = "";
            foreach (string i in lst)
                ret += i + "\n";
            return ret;
        }

        /// <summary>
        /// Sinh candidate cho token
        /// </summary>
        public HashSet<string> createCandidate(string prepre, string pre, string token, string next, string nextnext)
        {
            bool isMajuscule = Check_Majuscule(token);
            if (VNDictionary.getInstance.isSyllableVN(token))
                return RightWordCandidate.getInstance.createCandidate(prepre, pre, token,next, nextnext, isMajuscule);
            return WrongWordCandidate.getInstance.createCandidate(prepre,pre, token, next,nextnext, isMajuscule);
        }

        public HashSet<string> selectiveCandidate(string prepre, string pre, string token, string next, string nextnext)
        {
            return createCandidate(prepre, pre, token, next, nextnext);
        }

        /// <summary>
        /// sắp xếp candidate dựa trên số điểm, candidate có điểm cao nhất sẽ ở vị trí đầu tiên
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public Dictionary<string, double> sortDict(Dictionary<string, double> dict)
        {
            List<KeyValuePair<string, double>> myList = dict.OrderByDescending(pair => pair.Value).ToList();
            Dictionary<string, double> ret = new Dictionary<string, double>();
            foreach (KeyValuePair<string, double> pair in myList)
                ret.Add(pair.Key, pair.Value);
            return ret;
        }

        /// <summary>
        /// tính điểm cho candidate dựa vào ngữ cảnh
        /// </summary>
        /// <param name="prepre"></param>
        /// <param name="pre"></param>
        /// <param name="candidate"></param>
        /// <param name="next"></param>
        /// <param name="nextnext"></param>
        /// <returns></returns>
        public double calScore_Ngram(string prepre, string pre, string candidate, string next, string nextnext)
        {
            double calBiGram_PreCand = Ngram.Instance.calBigram(pre, candidate);
            double calBigram_CandNext = Ngram.Instance.calBigram(candidate, next);
            //double calTrigram1 = Ngram.Instance.calTrigram(prepre, pre, candidate);
            //double calTrigram2 = Ngram.Instance.calTrigram(pre, candidate, next);
            //double calTrigram3 = Ngram.Instance.calTrigram(candidate, next, nextnext);
            double lamda1 = 0.5;
            double lamda2 = 0.5;
            //double lamda3 = 0.1;
            //double lamda4 = 0.1;
            //double lamda5 = 0.1;
            double ret = lamda1 * calBiGram_PreCand + lamda2 * calBigram_CandNext;// + lamda3 * calTrigram1 + lamda4 * calTrigram2 + lamda5 * calTrigram3;
            return ret;
        }
        /// <summary>
        /// tính điểm cho candidate dựa vào từ ghép
        /// </summary>
        /// <param name="prepre"></param>
        /// <param name="pre"></param>
        /// <param name="candidate"></param>
        /// <param name="next"></param>
        /// <param name="nextnext"></param>
        /// <returns></returns>
        public int calScore_CompoundWord(string prepre, string pre, string candidate, string next, string nextnext)
        {
            string _3SyllComWord1 = String.Format("{0} {1} {2}", prepre, pre, candidate).Trim().ToLower();
            string _3SyllComWord2 = String.Format("{0} {1} {2}", pre, candidate, next).Trim().ToLower();
            string _3SyllComWord3 = String.Format("{0} {1} {2}", candidate, next, nextnext).Trim().ToLower();
            string _2SyllComWord1 = String.Format("{0} {1}", pre, candidate).Trim().ToLower();
            string _2SyllComWord2 = String.Format("{0} {1}", candidate, next).Trim().ToLower();
            if (prepre.Length > 0 && pre.Length > 0 && VNDictionary.getInstance.CompoundDict.Contains(_3SyllComWord1))
                return 10;
            else if (pre.Length > 0 && next.Length > 0 && VNDictionary.getInstance.CompoundDict.Contains(_3SyllComWord2))
                return 10;
            else if (next.Length > 0 && nextnext.Length > 0 && VNDictionary.getInstance.CompoundDict.Contains(_3SyllComWord3))
                return 10;
            else if (pre.Length > 0 && VNDictionary.getInstance.CompoundDict.Contains(_2SyllComWord1))
                return 5;
            else if (next.Length > 0 && VNDictionary.getInstance.CompoundDict.Contains(_2SyllComWord2))
                return 5;
            return 0;
        }
        /// <summary>
        /// tính điểm cho Candidate dựa vào độ tương tự với token
        /// điểm càng cao, thì candidate càng tốt
        /// </summary>
        /// <param name="token"></param>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public double calScore_Similarity(string token, string candidate)
        {
            int MAX = 8;
            token = token.ToLower();
            candidate = candidate.ToLower();
            //dựa trên số lượng ký tự
            int deltaLength = MAX - Math.Abs(token.Length - candidate.Length); //độ dài càng gần bằng nhau, điểm càng cao

            //dưạ trên số lượng ký tự khác nhau
            int diffScore = MAX;
            if (token.Length > candidate.Length)
            {
                //token = vin
                //candidate = vi
                for (int i = 0; i < token.Length; i++)
                    if (!candidate.Contains(token[i]))
                        diffScore -= 2;
            }
            else
            {
                for (int i = 0; i < candidate.Length; i++)
                    if (!token.Contains(candidate[i]))
                        diffScore -= 2;
            }
            if (diffScore < (MAX - token.Length))
                return -1;
            //dựa trên nhầm lẫn bàn phím
            //candidate sẽ được tăng điểm cao nếu candidate có khả năng do nhầm lẫn bàn phím mà tạo thành token
            double keyboardScore = 0;
            int regionScore = 0;

            if (token.Length == candidate.Length)
            {
                for (int i = 0; i < candidate.Length; i++)
                {
                    if (candidate[i] != token[i])
                        keyboardScore += calScore_Similarity_Keyboard(candidate[i], token[i]);
                }
            }
            //dựa trên nhầm lẫn vùng miền
            regionScore += calScore_Similarity_Region(candidate, token);
            double lamda1 = 0.35;
            double lamda2 = 0.35;
            double lamda3 = 0.1;
            double lamda4 = 0.2;
            double ret = lamda1 * deltaLength + lamda2 * diffScore - lamda3 * keyboardScore + lamda4 * regionScore;
            return ret;
        }
        /// <summary>
        /// tính toán độ tương tự giữa token với candidate dựa vào nhầm lẫn vùng miền
        /// điểm càng cao, thì candidate càng tốt
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private int calScore_Similarity_Region(string candidate, string token)
        {
            HashSet<string> candidates = WrongWordCandidate.getInstance.create_regionConfusedCandidate(token, false);
            if (candidates.Contains(candidate)) //token là một trường hợp nhầm lẫn vùng miền của candidate
                return 5;
            return 0;
        }
        /// <summary>
        /// tính độ tương tự giữa token với candidate dựa trên nhầm lẫn bàn phím
        /// </summary>
        /// <param name="token"></param>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public double calScore_Similarity_Keyboard(char token, char candidate)
        {
            int isFound = 0;
            int iCandidate = -1, iToken = -1, jCandidate = -1, jToken = -1;
            //duyệt qua từng dòng trong keyboard matrix
            for (int row = 0; row < StringConstant.MAX_KEYBOARD_ROW; row++)
            {
                //duyệt qua từng cột trong keyboard matrix
                for (int col = 0; col < StringConstant.MAX_KEYBOARD_COL; col++)
                {
                    if (StringConstant.Instance.KeyBoardMatrix_LowerCase[row, col] == candidate)
                    {
                        iCandidate = row;
                        jCandidate = col;
                        isFound++;
                    }
                    if (StringConstant.Instance.KeyBoardMatrix_LowerCase[row, col] == token)
                    {
                        iToken = row;
                        jToken = col;
                        isFound++;
                    }
                    if (isFound == 2)
                        break;
                }

                if (isFound == 2)
                    break;

            }
            int rowScore = /*StringConstant.MAX_KEYBOARD_ROW -*/ Math.Abs(iCandidate - iToken);
            int colScore = /*StringConstant.MAX_KEYBOARD_COL -*/ Math.Abs(jCandidate - jToken);
            return Math.Round(Math.Sqrt(Math.Pow(rowScore, 2) + Math.Pow(colScore, 2)), 2);
        }

        /// <summary>
        /// tạo candidate dựa trên từ ghép
        /// </summary>
        /// <param name="prepre"></param>
        /// <param name="pre"></param>
        /// <param name="token"></param>
        /// <param name="next"></param>
        /// <param name="nextnext"></param>
        /// <param name="isMajuscule"></param>
        /// <returns></returns>
        public HashSet<string> createCandByCompoundWord(string prepre, string pre, string token, string next, string nextnext, bool isMajuscule)
        {
            HashSet<string> hset = new HashSet<string>();
            //tìm X
            hset.UnionWith(VNDictionary.getInstance.findCompoundVNWord_Xx(next));
            hset.UnionWith(VNDictionary.getInstance.findCompoundVNWord_xX(pre));
            hset.UnionWith(VNDictionary.getInstance.findCompoundVNWord_Xxx(next, nextnext));
            hset.UnionWith(VNDictionary.getInstance.findCompoundVNWord_xXx(pre, next));
            hset.UnionWith(VNDictionary.getInstance.findCompoundVNWord_xxX(prepre, pre));

            return hset;
        }
        /// <summary>
        /// tạo candidate dựa trên ngữ cảnh
        /// </summary>
        /// <param name="prepre"></param>
        /// <param name="pre"></param>
        /// <param name="token"></param>
        /// <param name="next"></param>
        /// <param name="nextnext"></param>
        /// <param name="isMajuscule"></param>
        /// <returns></returns>
        public HashSet<string> createCandidateByNgram(string prepre, string pre, string token, string next, string nextnext, bool isMajuscule)
        {
            HashSet<string> lstCandidate = new HashSet<string>();
            List<string> bi = Ngram.Instance._biAmount.Keys.Where(key => key.Contains(pre) || key.Contains(next)).ToList();
            List<string> tri = Ngram.Instance._triAmount.Keys.Where(key => (key.Contains(prepre) && key.Contains(pre))
                                                                    || (key.Contains(next) && key.Contains(nextnext))
                                                                    || (key.Contains(pre) && key.Contains(next))).ToList();

            foreach (string key in bi)
            {
                string[] word = key.Split(' ');
                if (word[0].Equals(pre) && calScore_Similarity(token, word[1]) > LIM_SIMILARITY)
                    lstCandidate.Add(word[1]);
                else if (word[1].Equals(next) && calScore_Similarity(token, word[0]) > LIM_SIMILARITY)
                    lstCandidate.Add(word[0]);
            }
            //
            //bigram
            //
            //foreach (string key in Ngram.Instance._triAmount.Keys)
            //{
            //    string[] word = key.Split(' ');
            //    if (word[0].Equals(prepre) && word[1].Equals(pre) && calScore_Similarity(token, word[2]) > 10)
            //    {
            //        lstCandidate.Add(word[2]);
            //    }
            //    if (word[1].Equals(next) && word[2].Equals(nextnext) && calScore_Similarity(token, word[0]) > 10)
            //    {
            //        lstCandidate.Add(word[0]);
            //    }
            //    if (word[0].Equals(pre) && word[2].Equals(next) && calScore_Similarity(token, word[1]) > 10)
            //    {
            //        lstCandidate.Add(word[1]);
            //    }
            //}
            return lstCandidate;
        }
    }

}
