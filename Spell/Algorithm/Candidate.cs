using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                return 0.3;
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
                return 0.6;
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
        public HashSet<string> createCandidate(Context context)
        {
            bool isMajuscule = Check_Majuscule(context.TOKEN);
            if (VNDictionary.getInstance.isSyllableVN(context.TOKEN))
                return RightWordCandidate.getInstance.createCandidate(context, isMajuscule);
            return WrongWordCandidate.getInstance.createCandidate(context, isMajuscule);
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
        public double calScore_NgramForFixError(Context context, string candidate)
        {
            double calBiGram_PreCand = Ngram.Instance.calBigram(context.PRE, candidate);
            double calBigram_CandNext = Ngram.Instance.calBigram(candidate, context.NEXT);
            double calBiGram_PreToken = Ngram.Instance.calBigram(context.PRE, context.TOKEN);
            double calBigram_TokenNext = Ngram.Instance.calBigram(context.TOKEN, context.NEXT);

            if (calBiGram_PreCand == 0 && calBigram_CandNext == 0)
                return 0;
            if (calBiGram_PreToken == 0 && calBigram_TokenNext == 0)
                return 1;
            double lamda1 = 0, lamda2 = 0;
            lamda1 = calBiGram_PreCand / calBiGram_PreToken;
            lamda2 = calBigram_CandNext / calBigram_TokenNext;
            if (lamda1 > 100 || lamda2 > 100)
                return 1;
            if (lamda1 > 80 || lamda2 > 80)
                return 0.8;
            if (lamda1 > 50 || lamda2 > 50)
                return 0.5;
            return 0;
        }
        public double calScore_NgramForFindError(Context context)
        {
            double calBiGram_PreToken = Ngram.Instance.calBigram(context.PRE, context.TOKEN);
            double calBigram_TokenNext = Ngram.Instance.calBigram(context.TOKEN, context.NEXT);


            double ret = (0.5 * calBiGram_PreToken + 0.5 * calBigram_TokenNext) * 1E5;
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
        public double calScore_Ngram(Context context, string candidate)
        {
            double calBiGram_PreCand = Ngram.Instance.calBigram(context.PRE, candidate);
            double calBigram_CandNext = Ngram.Instance.calBigram(candidate, context.NEXT);
            double calBiGram_PreToken = Ngram.Instance.calBigram(context.PRE, context.TOKEN);
            double calBigram_TokenNext = Ngram.Instance.calBigram(candidate, context.TOKEN);

            if (calBiGram_PreCand == 0 && calBigram_CandNext == 0)
                return 0;
            if (calBiGram_PreToken == 0 && calBigram_TokenNext == 0)
                return 1;
            double lamda1 = 0, lamda2 = 0;
            lamda1 = calBiGram_PreCand / calBiGram_PreToken;
            lamda2 = calBigram_CandNext / calBigram_TokenNext;
            if (lamda1 > 100 || lamda2 > 100)
                return 1;
            if (lamda1 > 80 || lamda2 > 80)
                return 0.8;
            if (lamda1 > 50 || lamda2 > 50)
                return 0.5;
            return 0;
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
        public double calScore_CompoundWord(Context context, string candidate)
        {
            string _3SyllComWord1 = String.Format("{0} {1} {2}", context.PREPRE, context.PRE, candidate).Trim().ToLower();
            string _3SyllComWord2 = String.Format("{0} {1} {2}", context.PRE, candidate, context.NEXT).Trim().ToLower();
            string _3SyllComWord3 = String.Format("{0} {1} {2}", candidate, context.NEXT, context.NEXTNEXT).Trim().ToLower();
            string _2SyllComWord1 = String.Format("{0} {1}", context.PRE, candidate).Trim().ToLower();
            string _2SyllComWord2 = String.Format("{0} {1}", candidate, context.NEXT).Trim().ToLower();
            if (context.PREPRE.Length > 0 && context.PRE.Length > 0 && VNDictionary.getInstance.CompoundDict.Contains(_3SyllComWord1))
                return MAX_SCORE;
            else if (context.PRE.Length > 0 && context.NEXT.Length > 0 && VNDictionary.getInstance.CompoundDict.Contains(_3SyllComWord2))
                return MAX_SCORE;
            else if (context.NEXT.Length > 0 && context.NEXTNEXT.Length > 0 && VNDictionary.getInstance.CompoundDict.Contains(_3SyllComWord3))
                return MAX_SCORE;
            else if (context.PRE.Length > 0 && VNDictionary.getInstance.CompoundDict.Contains(_2SyllComWord1))
                return 0.7;
            else if (context.NEXTNEXT.Length > 0 && VNDictionary.getInstance.CompoundDict.Contains(_2SyllComWord2))
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
            if (token.Equals("côngn") && candidate.Equals("công"))
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
            if (signToken.Equals(signCandidate))
                return 0;
            if (signToken.Equals("s") && signCandidate.Equals("x") ||
                signToken.Equals("x") && signCandidate.Equals("s") ||
                signToken.Equals("j") && signCandidate.Equals("x") ||
                signToken.Equals("x") && signCandidate.Equals("j"))
                return 0.1;
            return 0.5;
        }

        private int calDenominatorForStringDiff(string extToken, string signToken, string extCandidate, string signCandidate)
        {
            return extToken.Length + signToken.Length + extCandidate.Length + signCandidate.Length;
        }

        private double calNumeratorForStringDiff(string extX, string extY)
        {

            double numerator = 0;
            try
            {
                int lengthExtX = extX.Length;
                int lengthExtY = extY.Length;
                int j = 0;
                bool isRegion = false;
                for (int i = 0; i < lengthExtX; i++, j++)
                    if (j < lengthExtY)
                    {
                        if (extX[i] == extY[j])
                            continue;
                        if (isRegion)
                        {
                            numerator += 1;
                            continue;
                        }
                        else if (i + 1 < lengthExtX && i + 1 < lengthExtY)
                        {
                            if (isRegionMistake(extX[i], extX[i + 1], extY[j], extY[j + 1]))
                            {
                                numerator += 0.1;
                                i++;
                                j++;
                                isRegion = true;
                                continue;
                            }

                        }
                        if (isRegionMistake(extX[i], extY[j]))
                        {
                            numerator += 0.1;
                            continue;
                        }
                        int index;
                        if (j > 0)
                            index = extY.IndexOf(extX[i], j - 1);
                        else
                            index = extY.IndexOf(extX[i], j);
                        if (index != -1)
                        {
                            if (Math.Abs(i - index) == 1)
                            {
                                numerator += 0.1;
                                continue;
                            }
                            else if (Math.Abs(i - index) == 2)
                            {
                                numerator += 0.2;
                                continue;
                            }
                        }

                        else if (isVowelVNMistake(extX[i], extY[j]))
                            numerator += 0.3;
                        else if (isKeyboardMistake(extX[i], extY[j]))
                        {
                            j--;
                            numerator += 0.5;
                        }
                        else {
                            j--;
                            numerator += 1;
                        }
                    }

                    else
                        numerator += 1;

            }
            catch
            {
                MessageBox.Show(extX + " " + extY);
            }
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
                        if (Math.Abs(iC1 - iC2) == 0 || Math.Abs(jC1 - jC2) == 0)
                            return true;
                        else return false;
                }
            return false;
        }
        private bool isRegionMistake(char c1, char c2)
        {
            int iC1 = -1, iC2 = -1;
            bool isFoundC1 = false, isFoundC2 = false;
            string c = "";
            for (int i = 0; i < StringConstant.MAXGROUP_REGION_CONFUSED; i++)
                for (int j = 0; j < StringConstant.MAXCASE_REGION_CONFUSED; j++)
                {
                    c = StringConstant.Instance.VNRegion_Confused_Matrix_LowerCase[i, j];
                    if (!isFoundC1)
                        if (c.Equals(c1 + ""))
                        {
                            isFoundC1 = true;
                            iC1 = i;
                        }
                    if (!isFoundC2)
                        if (c.Equals(c2 + ""))
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
        public HashSet<string> createCandByCompoundWord(Context context, bool isMajuscule)
        {
            HashSet<string> hset = new HashSet<string>();
            //tìm X
            hset.UnionWith(VNDictionary.getInstance.findCompoundVNWord_Xxx(context));
            hset.UnionWith(VNDictionary.getInstance.findCompoundVNWord_xXx(context));
            hset.UnionWith(VNDictionary.getInstance.findCompoundVNWord_xxX(context));
            if (hset.Count > 0)
                return hset;
            hset.UnionWith(VNDictionary.getInstance.findCompoundVNWord_Xx(context));
            hset.UnionWith(VNDictionary.getInstance.findCompoundVNWord_xX(context));
            

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
        public HashSet<string> createCandidateByNgram(Context context, bool isMajuscule)
        {
            HashSet<string> lstCandidate = new HashSet<string>();
            List<string> bi = Ngram.Instance._biAmount.Keys.Where(key => IsLikeLy(context.TOKEN, key) && (key.Contains(context.PRE) || key.Contains(context.NEXT))).ToList();
            foreach (string key in bi)
            {
                if (key.Contains(Ngram.Instance.START_STRING) || key.Contains(Ngram.Instance.END_STRING))
                    continue;
                string[] word = key.Split(' ');
                if (!word[1].ToLower().Equals(context.TOKEN.ToLower()) && word[0].Equals(context.PRE) && word[1].Length > 0)
                    lstCandidate.Add(word[1]);
                else if (!word[0].ToLower().Equals(context.TOKEN.ToLower()) && word[1].Equals(context.NEXT) && word[0].Length > 0)
                    lstCandidate.Add(word[0]);
                if (lstCandidate.Count > 50)
                    return lstCandidate;
            }
            return lstCandidate;
        }
        public HashSet<string> createCandidateByNgram_NoUseLamdaExp(Context context, bool isMajuscule)
        {
            HashSet<string> lstCandidate = new HashSet<string>();
            foreach (KeyValuePair<string, int> pair  in Ngram.Instance._biAmount)
            {
                if (pair.Key.Contains(Ngram.Instance.START_STRING) || pair.Key.Contains(Ngram.Instance.END_STRING))
                    continue;
                if (IsLikeLy(context.TOKEN, pair.Key) &&(pair.Key.Contains(context.PRE) || pair.Key.Contains(context.NEXT)))
                {
                    string[] word = pair.Key.Split(' ');
                    if (word[0].Equals(context.PRE) && word[1].Length > 0)
                        lstCandidate.Add(word[1]);
                    else if (word[1].Equals(context.NEXT) && word[0].Length > 0)
                        lstCandidate.Add(word[0]);
                }
            }
            return lstCandidate;
        }
        public bool IsLikeLy(string syll, string cand)
        {
            int lenght = syll.Length;
            bool isLongWord = lenght > 3 ? true : false;
            int count = 0;
            foreach (char s in syll)
                foreach (char c in cand)
                {
                    if (c == s)
                    {
                        count++;
                        if ((isLongWord && count == lenght - 2) || (!isLongWord && count == lenght - 1))
                            return true;
                        break;
                    }
                }
            return false;
        }
    }

}
