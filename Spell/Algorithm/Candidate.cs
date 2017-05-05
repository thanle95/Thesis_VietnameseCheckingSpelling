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
                return 0.8;
            }
        }
        public double LIM_LANGUAGEMODEL
        {
            get
            {
                return 0.01;
            }
        }
        public double LIM_COMPOUNDWORD
        {
            get
            {
                return 0.7;
            }
        }
        public double LIM_SCORE
        {
            get
            {
                return 0.65;
            }
        }
        public double MAX_SCORE
        {
            get
            {
                return 1;
            }
        }
        public double MIN_SCORE
        {
            get
            {
                return 0;
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

        /// <summary>region
        /// Sinh candidate cho token
        /// </summary>
        public HashSet<string> createCandidate(string prepre, string pre, string token, string next, string nextnext)
        {
            bool isMajuscule = Check_Majuscule(token);
            if (VNDictionary.getInstance.isSyllableVN(token))
                return RightWordCandidate.getInstance.createCandidate(prepre, pre, token, next, nextnext, isMajuscule);
            return WrongWordCandidate.getInstance.createCandidate(prepre, pre, token, next, nextnext, isMajuscule);
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
            double ret = (calBiGram_PreCand + calBigram_CandNext) * 1E5;
            //tang gia tri ngram
            if (ret > MAX_SCORE)
                return MAX_SCORE;
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
        public double calScore_CompoundWord(string prepre, string pre, string candidate, string next, string nextnext)
        {
            string _3SyllComWord1 = String.Format("{0} {1} {2}", prepre, pre, candidate).Trim().ToLower();
            string _3SyllComWord2 = String.Format("{0} {1} {2}", pre, candidate, next).Trim().ToLower();
            string _3SyllComWord3 = String.Format("{0} {1} {2}", candidate, next, nextnext).Trim().ToLower();
            string _2SyllComWord1 = String.Format("{0} {1}", pre, candidate).Trim().ToLower();
            string _2SyllComWord2 = String.Format("{0} {1}", candidate, next).Trim().ToLower();
            if (prepre.Length > 0 && pre.Length > 0 && VNDictionary.getInstance.CompoundDict.Contains(_3SyllComWord1))
                return MAX_SCORE;
            else if (pre.Length > 0 && next.Length > 0 && VNDictionary.getInstance.CompoundDict.Contains(_3SyllComWord2))
                return MAX_SCORE;
            else if (next.Length > 0 && nextnext.Length > 0 && VNDictionary.getInstance.CompoundDict.Contains(_3SyllComWord3))
                return MAX_SCORE;
            else if (pre.Length > 0 && VNDictionary.getInstance.CompoundDict.Contains(_2SyllComWord1))
                return 0.7;
            else if (next.Length > 0 && VNDictionary.getInstance.CompoundDict.Contains(_2SyllComWord2))
                return 0.7;
            return MIN_SCORE;
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
            double score = MAX_SCORE;
            token = token.ToLower();
            candidate = candidate.ToLower();
            double simScore = calStringSim(token, candidate);
            double diffScore = calScore_StringDiff(token, candidate);
            score = /*0.5 * simScore + 0.5 **/ diffScore;
            if (score > MAX_SCORE)
                return MAX_SCORE;
            if (score < MIN_SCORE)
                return MIN_SCORE;
            return score;
        }

        public string extractSignVN(string word)
        {
            string ret = "";
            int iSource = 0;
            char sign = ' ';
            char vnChar;
            int iVNChar = 0;
            foreach (char c in word)
            {
                iSource = StringConstant.Instance.source.IndexOf(c);
                //không mang dấu tiếng việt
                if (iSource == -1)
                {
                    iVNChar = StringConstant.Instance.vnCharacter.IndexOf(c);
                    if (iVNChar == -1)
                    {
                        ret += c;
                    }
                    else
                        ret += StringConstant.Instance.vnCharacter_Telex[iVNChar];
                }
                else
                {
                    vnChar = StringConstant.Instance.dest[iSource];
                    sign = StringConstant.Instance.VNSign[iSource % 5];
                    iVNChar = StringConstant.Instance.vnCharacter.IndexOf(vnChar);
                    if (iVNChar == -1)
                        ret += vnChar;
                    else ret += StringConstant.Instance.vnCharacter_Telex[iVNChar];
                }
            }
            if (sign != ' ')
                ret += sign;
            return ret;
        }
        public string[] extractSignVNNotFully(string word)
        {
            string extWord = "";
            string[] ret = new string[2];
            int iSource = 0;
            char vnChar;
            string sign = "";
            foreach (char c in word)
            {
                iSource = StringConstant.Instance.source.IndexOf(c);
                //không mang dấu tiếng việt
                if (iSource == -1)
                {
                    extWord += c;
                }
                else
                {
                    vnChar = StringConstant.Instance.dest[iSource];
                    sign = StringConstant.Instance.VNSign[iSource % 5] + "";
                    extWord += vnChar;
                }
            }
            ret[0] = extWord;
            ret[1] = sign;
            return ret;
        }
        /// <summary>
        /// tính điểm cho những ký tự khác nhau trong token và candidate
        /// điểm càng cao candidate càng giống với token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public double calScore_StringDiff(string token, string candidate)
        {

            double diffScore;
            if (candidate.Equals("chói"))
                diffScore = 0;
            //tách dấu ---> tachs dâus
            string[] extTokenArr = extractSignVNNotFully(token);
            string[] extCandidateArr = extractSignVNNotFully(candidate);

            string extToken = extTokenArr[0];
            string signToken = extTokenArr[1];

            string extCandidate = extCandidateArr[0];
            string signCandidate = extCandidateArr[1];

            int denominator = calDenominatorForStringDiff(extToken, signToken, extCandidate, signCandidate);
            double numerator = calNumeratorForStringDiff(extToken, extCandidate) + calNumeratorForStringDiff(extCandidate, extToken)
                + calNumeratorForStringDiff_Sign(signToken, signCandidate) + calNumeratorForStringDiff_Sign(signCandidate, signToken);
            //tử số, khác nhau càng nhiều, điểm càng cao

            diffScore = 1 - numerator / denominator;
            if (diffScore < MIN_SCORE)
                return MIN_SCORE;
            return diffScore;
        }

        private double calNumeratorForStringDiff_Sign(string signToken, string signCandidate)
        {
            if (signToken.Length == 0 && signCandidate.Length == 0)
                return 0;
            if (signToken.Equals("s") && signCandidate.Equals("x") || signToken.Equals("x") && signCandidate.Equals("s"))
                return 0.1;
            return 0.3;
        }

        private int calDenominatorForStringDiff(string extToken, string signToken, string extCandidate, string signCandidate)
        {
            return extToken.Length + signToken.Length + extCandidate.Length + signCandidate.Length;
        }

        private double calNumeratorForStringDiff(string extX, string extY)
        {
            double numerator = 0;
            int lengthExtX = extX.Length;
            int lengthExtY = extY.Length;
            for (int i = 0; i < lengthExtX; i++)
                if (i < lengthExtY)
                {
                    if (extX[i] == extY[i])
                        continue;
                    else if (i + 1 < lengthExtX && i + 1 < lengthExtY)
                    {
                        if (isRegionMistake(extX[i], extX[i + 1], extY[i], extY[i + 1]))
                        {
                            numerator += 0.1;
                            i++;
                            continue;
                        }

                    }
                    if (isRegionMistake(extX[i], extY[i]))
                        numerator += 0.1;
                    else if (isKeyboardMistake(extX[i], extY[i]))
                        numerator += 0.3;
                    else if (isVowelVNMistake(extX[i], extY[i]))
                        numerator += 0.3;

                    else numerator += 1;
                }
                else
                    numerator += 1;
            return numerator;
        }

        private bool isVowelVNMistake(char c1, char c2)
        {
            int iC1 = -1, iC2 = -1;
            bool isFoundC1 = false, isFoundC2 = false;
            for (int i = 0; i < StringConstant.MAXGROUP_VNCHARMATRIX; i++)
                for (int j = 0; j < StringConstant.MAXCASE_VNCHARMATRIX; j++)
                {
                    if (!isFoundC1)
                        if (StringConstant.Instance.vnCharacterMatrix[i, j] == c1)
                        {
                            isFoundC1 = true;
                            iC1 = i;
                        }
                    if (!isFoundC2)
                        if (StringConstant.Instance.vnCharacterMatrix[i, j] == c2)
                        {
                            isFoundC2 = true;
                            iC2 = i;
                        }
                    if (isFoundC1 && isFoundC2)
                        if (iC1 == iC2)
                            return true;
                        else return false;
                }
            return false;
        }

        private bool isKeyboardMistake(char c1, char c2)
        {
            int iC1 = -1, iC2 = -1, jC1 = -1, jC2 = -1;
            bool isFoundC1 = false, isFoundC2 = false;
            for (int i = 0; i < StringConstant.MAX_KEYBOARD_ROW; i++)
                for (int j = 0; j < StringConstant.MAX_KEYBOARD_COL; j++)
                {
                    if (!isFoundC1)
                        if (StringConstant.Instance.KeyBoardMatrix_LowerCase[i, j] == c1)
                        {
                            isFoundC1 = true;
                            iC1 = i;
                            jC1 = j;
                        }
                    if (!isFoundC2)
                        if (StringConstant.Instance.KeyBoardMatrix_LowerCase[i, j] == c2)
                        {
                            isFoundC2 = true;
                            iC2 = i;
                            jC2 = j;
                        }
                    if (isFoundC1 && isFoundC2)
                        if (Math.Abs(iC1 - iC2) <= 1 && Math.Abs(jC1 - jC2) <= 1)
                            return true;
                        else return false;
                }
            return false;
        }
        private bool isRegionMistake(char c1, char c2)
        {
            int iC1 = -1, iC2 = -1;
            bool isFoundC1 = false, isFoundC2 = false;
            for (int i = 0; i < StringConstant.MAXGROUP_REGION_CONFUSED; i++)
                for (int j = 0; j < StringConstant.MAXCASE_REGION_CONFUSED; j++)
                {
                    if (!isFoundC1)
                        if (StringConstant.Instance.VNRegion_Confused_Matrix_LowerCase[i, j].Contains(c1))
                        {
                            isFoundC1 = true;
                            iC1 = i;
                        }
                    if (!isFoundC2)
                        if (StringConstant.Instance.VNRegion_Confused_Matrix_LowerCase[i, j].Contains(c2))
                        {
                            isFoundC2 = true;
                            iC2 = i;
                        }
                    if (isFoundC1 && isFoundC2)
                        if (iC1 == iC2)
                            return true;
                        else return false;
                }
            return false;
        }
        private bool isRegionMistake(char c1, char c12, char c2, char c22)
        {
            int iC1 = -1, iC2 = -1;
            bool isFoundC1_12 = false, isFoundC2_22 = false;
            for (int i = 0; i < StringConstant.MAXGROUP_REGION_CONFUSED; i++)
                for (int j = 0; j < StringConstant.MAXCASE_REGION_CONFUSED; j++)
                {

                    if (!isFoundC1_12)
                        if (StringConstant.Instance.VNRegion_Confused_Matrix_LowerCase[i, j].Equals(c1 + "" + c12))
                        {
                            isFoundC1_12 = true;
                            iC1 = i;
                        }
                    if (!isFoundC2_22)
                        if (StringConstant.Instance.VNRegion_Confused_Matrix_LowerCase[i, j].Equals(c2 + "" + c22))
                        {
                            isFoundC2_22 = true;
                            iC2 = i;
                        }
                    //tr ~ ch, d ~ gi
                    if (isFoundC1_12 && isFoundC2_22)
                        if (iC1 == iC2)
                            return true;
                        else return false;
                }
            return false;
        }

        /// <summary>
        /// đo độ tương tự chuỗi, điểm càng cao, càng giống nhau
        /// </summary>
        /// <param name="token"></param>
        /// <param name="candidate"></param>
        /// <returns></returns>
        private double calStringSim(string token, string candidate)
        {
            double simEdit = calEditDist(token, candidate);

            double simTri = calTri(token, candidate);
            //token equals with candidate ---> 2
            return simEdit + simTri;
        }
        /// <summary>
        /// càng lớn càng tốt
        /// </summary>
        /// <param name="token"></param>
        /// <param name="candidate"></param>
        /// <returns></returns>
        private double calEditDist(string token, string candidate)
        {
            double ret = 0;
            int editDist = levenshtein(token, candidate);
            ret = (double)1 / (1 + editDist);
            return ret;
        }

        private int levenshtein(string a, string b)
        {

            if (string.IsNullOrEmpty(a))
            {
                if (!string.IsNullOrEmpty(b))
                {
                    return b.Length;
                }
                return 0;
            }

            if (string.IsNullOrEmpty(b))
            {
                if (!string.IsNullOrEmpty(a))
                {
                    return a.Length;
                }
                return 0;
            }

            int cost;
            int[,] d = new int[a.Length + 1, b.Length + 1];
            int min1;
            int min2;
            int min3;

            for (int i = 0; i <= d.GetUpperBound(0); i += 1)
            {
                d[i, 0] = i;
            }

            for (int i = 0; i <= d.GetUpperBound(1); i += 1)
            {
                d[0, i] = i;
            }

            for (int i = 1; i <= d.GetUpperBound(0); i += 1)
            {
                for (int j = 1; j <= d.GetUpperBound(1); j += 1)
                {
                    cost = Convert.ToInt32(!(a[i - 1] == b[j - 1]));

                    min1 = d[i - 1, j] + 1;
                    min2 = d[i, j - 1] + 1;
                    min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }

            return d[d.GetUpperBound(0), d.GetUpperBound(1)];

        }
        /// <summary>
        /// càng lớn càng tốt
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private double calTri(string x, string y)
        {
            double ret = 0;
            int triX = tri(x);
            int triY = tri(y);
            int triXY = triIntersection(x, y);
            ret = (double)1 / (1 + triX + triY - 2 * (triXY));
            return ret;
        }

        private int triIntersection(string x, string y)
        {
            HashSet<string> hSetTriX = getHSetTri(x);
            HashSet<string> hSetTriY = getHSetTri(y);
            IEnumerable<string> both = hSetTriX.Intersect(hSetTriY);
            return both.Count();
        }
        private HashSet<string> getHSetTri(string x)
        {
            HashSet<string> hsetTri = new HashSet<string>();
            if (x.Length < 4)
                return hsetTri;
            for (int i = 0; i < x.Length - 2; i++)
            {
                hsetTri.Add(x.Substring(i, 3));
            }
            return hsetTri;
        }
        private int tri(string x)
        {
            return getHSetTri(x).Count;
        }



        /// <summary>
        /// tính toán độ tương tự giữa token với candidate dựa vào nhầm lẫn vùng miền
        /// điểm càng cao, thì candidate càng tốt
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private double calScore_Similarity_Region(string candidate, string token)
        {
            HashSet<string> candidates = WrongWordCandidate.getInstance.create_regionConfusedCandidate(token, false);
            if (candidates.Contains(candidate)) //token là một trường hợp nhầm lẫn vùng miền của candidate
                return 0.1;
            return MIN_SCORE;
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
            return Math.Round((Math.Sqrt(Math.Pow(rowScore, 2) + Math.Pow(colScore, 2))) / 30, 2);
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
            foreach (string key in bi)
            {
                if (key.Contains(Ngram.Instance.START_STRING) || key.Contains(Ngram.Instance.END_STRING))
                    continue;
                string[] word = key.Split(' ');
                if (word[0].Equals(pre)/* && calScore_Similarity(token, word[1]) > LIM_SIMILARITY*/)
                    lstCandidate.Add(word[1]);
                else if (word[1].Equals(next) /*&& calScore_Similarity(token, word[0]) > LIM_SIMILARITY*/)
                    lstCandidate.Add(word[0]);
            }
            return lstCandidate;
        }
    }

}
