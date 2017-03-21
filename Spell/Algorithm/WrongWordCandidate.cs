﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Spell.Algorithm;
namespace Spell.Algorithm
{
    public class WrongWordCandidate
    {
        private WrongWordCandidate()
        {
        }

        private static WrongWordCandidate instance = new WrongWordCandidate();
        public static WrongWordCandidate getInstance
        {
            get
            {
                return instance;
            }
        }

        public HashSet<string> createCandidate(string prepre, string pre, string token, string next, string nextnext, bool isMajuscule)
        {
            HashSet<string> result = new HashSet<string>();
            HashSet<string> hsetNgramCand = createCandidateByNgram(prepre, pre, token, next, nextnext, isMajuscule);
            Dictionary<string, double> candidatesWithScore = new Dictionary<string, double>(), tempCandidatesWithScore = new Dictionary<string, double>();
            HashSet<string> hSetXx = VNDictionary.getInstance.findCompoundVNWord_Xx(next);
            HashSet<string> hSetxX = VNDictionary.getInstance.findCompoundVNWord_xX(pre);
            HashSet<string> hSetXxx = VNDictionary.getInstance.findCompoundVNWord_Xxx(next, nextnext);
            HashSet<string> hSetxXx = VNDictionary.getInstance.findCompoundVNWord_xXx(pre, next);
            HashSet<string> hSetxxX = VNDictionary.getInstance.findCompoundVNWord_xxX(prepre, pre);
            HashSet<string> hSetCandidate = new HashSet<string>();
            hSetCandidate.UnionWith(hSetXx);
            hSetCandidate.UnionWith(hSetxX);
            hSetCandidate.UnionWith(hSetXxx);
            hSetCandidate.UnionWith(hSetxXx);
            hSetCandidate.UnionWith(hSetxxX);
            double lamda1 = 0.04;
            double lamda2 = 0.9;
            double lamda3 = 0.06;
            double score = 0;
            int D = 0;
            double H = 0;
            double L = 0.0;
            //
            string text = "";
            foreach (string candidate in hSetCandidate)
            {
                L = calNgram(prepre, pre, candidate, next, nextnext);
                H = calDeviation(token, candidate);
                D = calMatch_pre_next_compoundWordVNDict(prepre, pre, candidate, next, nextnext);
                score = lamda1 * D + lamda2 * L + lamda3 * H;
                if (H != -1)
                    if (D == 10)
                    {
                        if (tempCandidatesWithScore.Count < 5)
                        {
                            tempCandidatesWithScore.Add(candidate, score);
                            tempCandidatesWithScore = sortDict(tempCandidatesWithScore);
                        }
                        else
                        if (tempCandidatesWithScore.Last().Value < score)
                        {
                            tempCandidatesWithScore.Remove(tempCandidatesWithScore.Last().Key);
                            tempCandidatesWithScore.Add(candidate, score);
                            tempCandidatesWithScore = sortDict(tempCandidatesWithScore);
                        }
                    }
                    else
                    {
                        if (candidatesWithScore.Count < 5)
                        {
                            candidatesWithScore.Add(candidate, score);
                            candidatesWithScore = sortDict(candidatesWithScore);
                        }
                        else
                        if (candidatesWithScore.Last().Value < score)
                        {
                            candidatesWithScore.Remove(candidatesWithScore.Last().Key);
                            candidatesWithScore.Add(candidate, score);
                            candidatesWithScore = sortDict(candidatesWithScore);
                        }
                    }
                text += String.Format("{0}: [{1};{2};{3}] = {4}", candidate, D, L, H, score) + "\n";
            }
            if (tempCandidatesWithScore.Count > 0)
                foreach (string key in tempCandidatesWithScore.Keys)
                    result.Add(key);
            else
                foreach (string key in candidatesWithScore.Keys)
                    result.Add(key);
            //HashSet<string> tuanTu = kiemtra_TuanTu(token, isMajuscule);
            //HashSet<string> xoayVong = kiemtra_XoayVong(token, isMajuscule);
            //foreach (string s in tuanTu)
            //    result.Add(s);
            //foreach (string s in xoayVong)
            //    result.Add(s);
            using (FileStream aFile = new FileStream((@"C:\Users\Kiet\OneDrive\Thesis\test.txt"), FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(aFile))
            {
                sw.WriteLine(text);
                sw.WriteLine("**********************************************************************");
            }
            
            using (FileStream aFile = new FileStream((@"C:\Users\Kiet\OneDrive\Thesis\testNgramCanđ.txt"), FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(aFile))
            {
                foreach(string temp in hsetNgramCand)
                {
                    sw.WriteLine(temp);
                }

                sw.WriteLine("**********************************************************************");
            }
            //File.WriteAllText(@"C:\Users\Kiet\OneDrive\Thesis\test.txt", text);
            return result;
        }

        public HashSet<string> createCandidateByNgram(string prepre, string pre, string token, string next, string nextnext, bool isMajuscule)
        {
            HashSet<string> lstCandidate = new HashSet<string>();
            List<string> bi = Ngram.Instance._biAmount.Keys.Where(key => key.Contains(pre) || key.Contains(next)).ToList();
            List<string> tri = Ngram.Instance._triAmount.Keys.Where(key =>( key.Contains(prepre) && key.Contains(pre))
                                                                    || (key.Contains(next) && key.Contains(nextnext))
                                                                    || (key.Contains(pre) && key.Contains(next))).ToList();

            foreach (string key in bi)
            {
                string[] word = key.Split(' ');
                if (word[0].Equals(pre) && calDeviation(token,word[1]) > 10)
                {
                    lstCandidate.Add(word[1]);
                }
                if (word[1].Equals(next) && calDeviation(token, word[0]) > 10)
                {
                    lstCandidate.Add(word[0]);
                }       
                       
            }

            foreach (string key in Ngram.Instance._triAmount.Keys)
            {
                string[] word = key.Split(' ');
                if (word[0].Equals(prepre) && word[1].Equals(pre) && calDeviation(token, word[2]) > 10)
                {
                    lstCandidate.Add(word[2]);
                }
                if (word[1].Equals(next) && word[2].Equals(nextnext) && calDeviation(token, word[0]) > 10)
                {
                    lstCandidate.Add(word[0]);
                }
                if (word[0].Equals(pre) && word[2].Equals(next) && calDeviation(token, word[1]) > 10)
                {
                    lstCandidate.Add(word[1]);
                }
            }

            return lstCandidate;
        }

        private Dictionary<string, double> sortDict(Dictionary<string, double> dict)
        {
            List<KeyValuePair<string, double>> myList = dict.OrderByDescending(pair => pair.Value).ToList();
            Dictionary<string, double> ret = new Dictionary<string, double>();
            foreach (KeyValuePair<string, double> pair in myList)
                ret.Add(pair.Key, pair.Value);
            return ret;
        }
        public double calNgram(string prepre, string pre, string candidate, string next, string nextnext)
        {
            double calBiGram_PreCand = Ngram.Instance.calBiNgram(pre, candidate);
            double calBigram_CandNext = Ngram.Instance.calBiNgram(candidate, next);
            double calTrigram1 = Ngram.Instance.calTriNgram(prepre, pre, candidate);
            double calTrigram2 = Ngram.Instance.calTriNgram(pre, candidate, next);
            double calTrigram3 = Ngram.Instance.calTriNgram( candidate, next, nextnext);
            double ret = calBiGram_PreCand + calBigram_CandNext + calTrigram1 + calTrigram2 + calTrigram3;
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="preCandidate"></param>
        /// <param name="nextCandidate"></param>
        /// <returns></returns>
        public int calMatch_pre_next_compoundWordVNDict(string prepre, string pre, string candidate, string next, string nextnext)
        {
            string _3SyllComWord1 = (prepre + " " + pre + " " + candidate).Trim().ToLower();
            string _3SyllComWord2 = (pre + " " + candidate + " " + next).Trim().ToLower();
            string _3SyllComWord3 = (candidate + " " + next + " " + nextnext).Trim().ToLower();
            string _2SyllComWord1 = (pre + " " + candidate).Trim().ToLower();
            string _2SyllComWord2 = (candidate + " " + next).Trim().ToLower();
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
        /// tính toán độ lệch giữa token với candidate dựa vào nhầm lẫn bàn phím 
        /// điểm càng thấp, thì candidate càng tốt
        /// </summary>
        /// <param name="token"></param>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public double calDeviation(string token, string candidate)
        {
            int MAX = 10;
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

            int iCandidate = -1, iToken = -1, jCandidate = -1, jToken = -1;
            double keyboardScore = 0;
            int regionScore = 0;

            if (token.Length == candidate.Length)
            {
                for (int i = 0; i < candidate.Length; i++)
                {
                    if (candidate[i] != token[i])
                        keyboardScore += calDeviationByKeyBoardMissType(candidate[i], token[i]);
                }
            }
            regionScore += calDeviationByRegionConfuse(candidate, token);
            return deltaLength + diffScore - keyboardScore + regionScore;
        }
        /// <summary>
        /// tính toán độ lệch giữa token với candidate dựa vào nhầm lẫn vùng miền
        /// điểm càng cao, thì candidate càng tốt
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private int calDeviationByRegionConfuse(string candidate, string token)
        {
            HashSet<string> candidates = create_regionConfusedCandidate(token, false);
            if (candidates.Contains(candidate)) //token là một trường hợp nhầm lẫn vùng miền của candidate
                return 5;
            return 0;
        }

        public double calDeviationByKeyBoardMissType(char token, char candidate)
        {
            int isFound = 0;
            int iCandidate = -1, iToken = -1, jCandidate = -1, jToken = -1;
            //duyệt qua từng dòng trong keyboard matrix
            for (int row = 0; row < StringConstant.MAX_KEYBOARD_ROW; row++)
            {
                for (int col = 0; col < StringConstant.MAX_KEYBOARD_COL; col++)
                {
                    if (StringConstant.getInstance.KeyBoardMatrix_LowerCase[row, col] == candidate)
                    {
                        iCandidate = row;
                        jCandidate = col;
                        isFound++;
                    }
                    if (StringConstant.getInstance.KeyBoardMatrix_LowerCase[row, col] == token)
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
        /// Sử dụng tuân tự các phương thức tạo candidate theo các trường hợp khác nhau 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HashSet<string> kiemtra_TuanTu(string token, bool isMajuscule)
        {
            HashSet<string> result = new HashSet<string>();
            HashSet<string> lstCandidateReplace = create_replaceCandidate(token, isMajuscule);
            HashSet<string> lstCandidateRegionConfused = create_regionConfusedCandidate(token, isMajuscule);
            HashSet<string> lstCandidateInsert = create_insertCandidate(token, isMajuscule);
            HashSet<string> lstCandidatePermute = create_permuteCandidate(token);
            HashSet<string> lstCandidateDelete = create_deleteCandidate(token, isMajuscule);
            foreach (string i in lstCandidateDelete)
                result.Add(i.ToLower().Trim());
            foreach (string i in lstCandidateInsert)
                result.Add(i.ToLower().Trim());
            foreach (string i in lstCandidatePermute)
                result.Add(i.ToLower().Trim());
            foreach (string i in lstCandidateRegionConfused)
                result.Add(i.ToLower().Trim());
            foreach (string i in lstCandidateReplace)
                result.Add(i.ToLower().Trim());
            return result;
        }

        public HashSet<string> kiemtra_XoayVong(string token, bool isMajuscule)
        {
            HashSet<string> result = new HashSet<string>();
            return result;
        }

        #region create candidate for error by missing word (insert case)
        /// <summary>
        /// Sinh candidate cho trường hợp từ bị thiếu
        /// Phương thức này sẽ lần lượt thêm những chữ cái khác nhau 
        /// Mỗi lần thêm chữ cái, ta sẽ kiểm tra từ này có thuộc từ điển tiếng việt 
        /// Nếu từ này có trong từ điển thì sẽ thêm nó vào danh sách candidate
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HashSet<string> create_insertCandidate(string token, bool isMajuscule)
        {

            char[] vn_AlphabetArr;
            if (isMajuscule)
            {
                vn_AlphabetArr = StringConstant.getInstance.VNAlphabetArr_UpperCase;
            }
            else
            {
                vn_AlphabetArr = StringConstant.getInstance.VNAlphabetArr_LowerCase;
            }

            HashSet<string> result = new HashSet<string>();
            for (int i = 0; i < vn_AlphabetArr.Length; i++)
            {
                for (int j = 0; j <= token.Length; j++)
                {
                    string s = checkSignPos(token.Insert(j, vn_AlphabetArr[i] + ""), isMajuscule);
                    if (VNDictionary.getInstance.isSyllableVN(s))
                        result.Add(s);
                }
            }
            return result;
        }
        /// <summary>
        /// Kiểm tra và trả về token với vị trí dấu thanh đúng
        /// </summary>
        /// <param name="token">token cần kiểm tra</param>
        /// <returns>token với dấu thanh đúng vị trí</returns>
        public string checkSignPos(string token, bool isMajuscule)
        {
            char[] VN_Vowel_Arr;
            string[] VN_Consonant_Arr;
            if (isMajuscule)
            {
                VN_Consonant_Arr = StringConstant.getInstance.VNConsonantArr_UpperCase;
                VN_Vowel_Arr = StringConstant.getInstance.VNVowelArr_UpperCase;
            }
            else
            {
                VN_Consonant_Arr = StringConstant.getInstance.VNConsonantArr_LowerCase;
                VN_Vowel_Arr = StringConstant.getInstance.VNVowelArr_LowerCase;
            }

            List<string> consonants = countingConsonant(token, isMajuscule);
            //số lượng con chữ phụ âm hay tổ hợp con chữ phụ âm có trong token
            int consonantNo = consonants.Count;
            //số lượng ký tự là phụ âm trong token
            int charConsonantNo = 0;
            if (consonantNo > 0)
                foreach (string i in consonants)
                    charConsonantNo += i.Length;
            //số lượng ký tự là nguyên âm trong token
            int charVowelNo = token.Length - charConsonantNo;
            //vị trí ký tự mang dấu thanh trong token
            int signPos = getSignPositionInToken(token, isMajuscule);
            switch (charVowelNo)
            {
                //chỉ có 1 ký tự là nguyên âm
                case 1:
                    return token;
                case 2:
                    //có 2 tổ hợp con chữ phụ âm hay 2 con chữ phụ âm
                    if (consonantNo < 3 && signPos != -1)// có dấu thanh
                        if (consonantNo == 2) //chắc chắn có phụ âm cuối
                            //tùông ---> signPos = 5- 2 - 1 = 2
                            if (signPos == token.Length - consonants[1].Length - 1) //dấu thanh ở nguyên âm chót
                                return token;
                            else
                                return changeSignPos(consonants[0].Length, consonants[0].Length + 1, token, isMajuscule);
                        //hoặc có 1 tổ hợp con chữ phụ âm hay 1 con chữ phụ âm, và đó không phải phụ âm đầu
                        else if (consonantNo == 1 && !hasFirstConsonant(token, consonants))
                        {
                            if (signPos != -1) // có dấu thanh
                                //úông ---> 
                                if (signPos == token.Length - consonants[0].Length - 1) //dấu thanh ở nguyên âm chót
                                    return token;
                                else
                                    return changeSignPos(consonants[0].Length, consonants[0].Length + 1, token, isMajuscule);
                        }
                        else// không có phụ âm cuối
                        {
                            //taị
                            if (signPos == token.Length - 2) //dấu thanh ở nguyên âm đầu
                                return token;
                            else
                            {
                                return changeSignPos(consonants[0].Length + 1, consonants[0].Length, token, isMajuscule);
                            }

                        }
                    return token;
                case 3: // có 3 nguyên âm, 3 nguyên âm phải kề nhau nên không có phụ âm cuối
                    if (consonants.Count == 1)
                    {
                        int vowelCountting = 0;
                        foreach (char temp in token)
                        {
                            if (VN_Vowel_Arr.Contains(temp))
                                vowelCountting++;
                            else if (vowelCountting != 0)
                                return token;
                        }
                        if (signPos != -1) // có dấu thanh
                        {
                            //ngoáy ----> signPos = 5 - 2 = 3
                            //hoại ----> signPos = 4 - 2 = 2
                            if (signPos == token.Length - 2) //dấu thanh ở nguyên âm giữa
                                return token;
                            else if (signPos == token.Length - 3) //dấu thanh ở nguyên âm đầu
                                //ngóay ----> 
                                return changeSignPos(token.Length - 3, consonants[0].Length + 1, token, isMajuscule);
                            else// dấu thanh ở nguyên âm cuối
                                //ngoaý ----> 
                                // i = 3
                                return changeReverseSignPos(token.Length - 2, token, isMajuscule);
                        }
                        else return token;
                    }
                    else return token;
            }
            return token;
        }
        private string changeSignPos(int i, int j, string token, bool isMajuscule)
        {
            //tùông ---> sign = {dấu huyền}
            int sign = getSign(token[i], isMajuscule);
            if (sign != -1)
            {
                //tùông ---> c = 'u'
                char c = getCharWithoutSign(token[i], isMajuscule);
                //tùông ---> v = 'ồ'
                char v = getCharWithSign(token[j], sign, isMajuscule);
                //tùông ---> ret = (tng).Insert(1, "uồ") = tuồng
                string ret = (token.Remove(i, 2)).Insert(i, c + "" + v);
                return ret;
            }
            return token;
        }
        private string changeReverseSignPos(int i, string token, bool isMajuscule)
        {
            int sign = getSign(token[i + 1], isMajuscule);
            //c = 'y'
            char c = getCharWithoutSign(token[i + 1], isMajuscule);
            //v = 'á'
            char v = getCharWithSign(token[i], sign, isMajuscule);
            //
            string ret = (token.Remove(i, 2)).Insert(i, v + "" + c);
            return ret;
        }
        /// <summary>
        /// Lấy vị trí ký tự mang dấu thanh trong token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private int getSignPositionInToken(string token, bool isMajuscule)
        {
            char[] VN_VowelWithSign_Arr;
            if (isMajuscule)
            {
                VN_VowelWithSign_Arr = StringConstant.getInstance.VNVowelWithSignArr_UpperCase;
            }
            else
            {
                VN_VowelWithSign_Arr = StringConstant.getInstance.VNVowelWithSignArr_LowerCase;
            }

            foreach (char c in StringConstant.getInstance.VNVowelWithSignArr_LowerCase)
            {
                if (token.Contains(c))
                    return token.IndexOf(c);
            }
            return -1;
        }
        /// <summary>
        /// Lấy dấu thanh dựa trên mảng 2 chiều vietnameseVowelWithSign
        /// </summary>
        /// <param name="c">ký tự mang dấu thanh</param>
        /// <returns>dấu thanh</returns>
        private int getSign(char c, bool isMajuscule)
        {
            char[,] VN_VowelWithSignMatrix;
            if (isMajuscule)
            {
                VN_VowelWithSignMatrix = StringConstant.getInstance.VNVowelWithSignMatrix_UpperCase;
            }
            else
            {
                VN_VowelWithSignMatrix = StringConstant.getInstance.VNVowelWithSignMatrix_LowerCase;
            }
            for (int i = 0; i < StringConstant.MAX_SIGN_NO; i++)
                for (int j = 0; j < StringConstant.MAX_VOWEL_NO; j++)
                    if (c == VN_VowelWithSignMatrix[i, j])
                        return i;
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">ký tự cần bỏ dấu thanh</param>
        /// <returns>ký tự sau khi bỏ dấu thanh</returns>
        private char getCharWithoutSign(char c, bool isMajuscule)
        {
            char[,] VN_VowelWithSignMatrix;
            char[] VN_Vowel_Arr;
            if (isMajuscule)
            {
                VN_Vowel_Arr = StringConstant.getInstance.VNVowelArr_UpperCase;
                VN_VowelWithSignMatrix = StringConstant.getInstance.VNVowelWithSignMatrix_UpperCase;
            }
            else
            {
                VN_Vowel_Arr = StringConstant.getInstance.VNVowelArr_LowerCase;
                VN_VowelWithSignMatrix = StringConstant.getInstance.VNVowelWithSignMatrix_LowerCase;
            }

            for (int i = 0; i < StringConstant.MAX_SIGN_NO; i++)
                for (int j = 0; j < StringConstant.MAX_VOWEL_NO; j++)
                    if (c == VN_VowelWithSignMatrix[i, j])
                        return VN_Vowel_Arr[j];
            return c;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">ký tự cần thêm dấu thanh</param>
        /// <param name="sign">dấu thanh</param>
        /// <returns>ký tự được thêm dấu thanh</returns>
        private char getCharWithSign(char c, int sign, bool isMajuscule)
        {
            char[,] VN_VowelWithSignMatrix;
            char[] VN_Vowel_Arr;
            if (isMajuscule)
            {
                VN_Vowel_Arr = StringConstant.getInstance.VNVowelArr_UpperCase;
                VN_VowelWithSignMatrix = StringConstant.getInstance.VNVowelWithSignMatrix_UpperCase;
            }
            else
            {
                VN_Vowel_Arr = StringConstant.getInstance.VNVowelArr_LowerCase;
                VN_VowelWithSignMatrix = StringConstant.getInstance.VNVowelWithSignMatrix_LowerCase;
            }

            for (int i = 0; i < StringConstant.MAX_VOWEL_NO; i++)
                if (c == VN_Vowel_Arr[i])
                    return VN_VowelWithSignMatrix[sign, i];
            return c;
        }
        /// <summary>
        /// Đếm số phụ âm trong một âm
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public List<string> countingConsonant(string token, bool isMajuscule)
        {
            string[] VN_Consonant_Arr;
            if (isMajuscule)
            {
                VN_Consonant_Arr = StringConstant.getInstance.VNConsonantArr_UpperCase;
            }
            else
            {
                VN_Consonant_Arr = StringConstant.getInstance.VNConsonantArr_LowerCase;
            }

            List<string> ret = new List<string>();
            string sTmp = token;
            foreach (string s in VN_Consonant_Arr)
            {

                if (sTmp.Contains(s))
                {
                    ret.Add(s);
                    sTmp = sTmp.Remove(sTmp.IndexOf(s), s.Length);
                }
            }
            if (ret.Count == 1)
                return ret;
            else if (ret.Count == 2)
            {
                if (!token.Substring(0, ret[0].Length).Equals(ret[0])) // là phụ âm đầu
                    ret.Reverse();
            }
            return ret;
        }
        #endregion

        /// <summary>
        /// Sinh candidate cho trường hợp từ bị dư
        /// Ta sẽ lần lượt xóa từng chữ của từ,
        /// Mỗi lần xóa, ta sẽ kiểm tra lại từ đó,
        /// Nếu thuộc từ điển thì sẽ thêm vào danh sách candidate
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HashSet<string> create_deleteCandidate(string token, bool isMajuscule)
        {
            HashSet<string> result = new HashSet<string>();
            //xóa 1 ký tự
            for (int i = 0; i < token.Length; i++)
            {
                string s = token.Remove(i, 1);
                if (VNDictionary.getInstance.isSyllableVN(checkSignPos(s, isMajuscule)))
                    result.Add(s);
            }
            //xóa 2 ký tự
            for (int i = 0; i < token.Length; i++)
            {
                string si = token.Remove(i, 1);
                for (int j = 0; j < si.Length; j++)
                {
                    string sj = si.Remove(j, 1);
                    if (VNDictionary.getInstance.isSyllableVN(checkSignPos(sj, isMajuscule)))
                        result.Add(sj);
                }
            }
            //xóa 3 ký tự
            for (int i = 0; i < token.Length; i++)
            {
                string si = token.Remove(i, 1);
                for (int j = 0; j < si.Length; j++)
                {
                    string sj = si.Remove(j, 1);
                    for (int k = 0; k < sj.Length; k++)
                    {
                        string sk = sj.Remove(k, 1);
                        if (VNDictionary.getInstance.isSyllableVN(checkSignPos(sk, isMajuscule)))
                            result.Add(sk);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Sinh candidate cho trường hợp từ bị hoán vị (đảo lộn) vị trí các chữ trong từ,
        /// Ta sẽ đảo thứ tự từng cặp chữ cái,
        /// Khi có kết quả là từ thuộc từ điển tiếng việt thì ta sẽ thêm vào danh sách candidate
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public HashSet<string> create_permuteCandidate(string token)
        {
            HashSet<string> result = new HashSet<string>();
            for (int i = 0; i < token.Length - 1; i++)
            {
                string s = permuteToken(token, i);
                if (VNDictionary.getInstance.isSyllableVN(s))
                    result.Add(s);
            }
            return result;
        }

        private string permuteToken(string token, int startIndex)
        {
            //lang -> alng, lnag, lagn
            string s = token.Remove(startIndex, 1);
            return s.Insert(startIndex + 1, token[startIndex] + "");
        }

        #region Create candidate for error by gõ nhầm và âm vùng miền ( replace )
        /// <summary>
        /// Sinh candidate cho trường hợp từ bị gõ nhầm trên bàn phím hoặc do sai âm vùng miền
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HashSet<string> create_replaceCandidate(string token, bool isMajuscule)
        {

            char[,] keyBoardMatrix, VN_VowelWithSignMatrix;
            char[] VN_Vowel_Arr;
            if (isMajuscule)
            {
                keyBoardMatrix = StringConstant.getInstance.KeyBoardMatrix_UperCase;
                VN_Vowel_Arr = StringConstant.getInstance.VNVowelArr_UpperCase;
                VN_VowelWithSignMatrix = StringConstant.getInstance.VNVowelWithSignMatrix_UpperCase;
            }
            else
            {
                keyBoardMatrix = StringConstant.getInstance.KeyBoardMatrix_LowerCase;
                VN_Vowel_Arr = StringConstant.getInstance.VNVowelArr_LowerCase;
                VN_VowelWithSignMatrix = StringConstant.getInstance.VNVowelWithSignMatrix_LowerCase;
            }
            HashSet<string> result = new HashSet<string>();
            for (int iToken = 0; iToken < token.Length; iToken++)
            {
                int i = hLookup(token[iToken], isMajuscule);
                int j = vLookup(token[iToken], isMajuscule);
                for (int ii = i - 1; ii <= i + 1; ii++)
                    if (ii < 0 || ii > 2)
                        continue;
                    else
                        for (int jj = j - 1; jj <= j + 1; jj++)
                        {
                            if (jj < 0 || jj > 9)
                                continue;
                            char c = keyBoardMatrix[ii, jj];
                            string s = (token.Remove(iToken, 1)).Insert(iToken, c + "");
                            if (VNDictionary.getInstance.isSyllableVN(s))
                            {
                                result.Add(s);
                            }
                        }
                char ch = token[iToken];
                string str = token;
                int maxVN_Vowel = StringConstant.MAX_VOWEL_NO;
                int maxVN_Sign = StringConstant.MAX_SIGN_NO;
                for (int x = 0; x < maxVN_Vowel; x++)
                    for (int y = 0; y < maxVN_Sign; y++)
                        if (ch == VN_VowelWithSignMatrix[y, x] || ch == VN_Vowel_Arr[x])
                        {
                            switch (x)
                            {
                                case 0:
                                case 1:
                                case 2: //a ă â
                                    for (int xx = 0; xx <= 2; xx++)
                                        for (int yy = 0; yy < maxVN_Sign; yy++)
                                        {
                                            str = (token.Remove(iToken, 1)).Insert(iToken, VN_VowelWithSignMatrix[yy, xx] + "");
                                            if (VNDictionary.getInstance.isSyllableVN(str))
                                                result.Add(str);
                                            str = (token.Remove(iToken, 1)).Insert(iToken, VN_Vowel_Arr[xx] + "");
                                            if (VNDictionary.getInstance.isSyllableVN(str))
                                                result.Add(str);
                                        }
                                    break;
                                case 3:
                                case 4: //e ê
                                    for (int xx = 3; xx <= 4; xx++)
                                        for (int yy = 0; yy < maxVN_Sign; yy++)
                                        {
                                            str = (token.Remove(iToken, 1)).Insert(iToken, VN_VowelWithSignMatrix[yy, xx] + "");
                                            if (VNDictionary.getInstance.isSyllableVN(str))
                                                result.Add(str);
                                            str = (token.Remove(iToken, 1)).Insert(iToken, VN_Vowel_Arr[xx] + "");
                                            if (VNDictionary.getInstance.isSyllableVN(str))
                                                result.Add(str);
                                        }
                                    break;
                                case 5:
                                case 6:
                                case 7: //o ô ơ
                                    for (int xx = 5; xx <= 7; xx++)
                                        for (int yy = 0; yy < maxVN_Sign; yy++)
                                        {
                                            str = (token.Remove(iToken, 1)).Insert(iToken, VN_VowelWithSignMatrix[yy, xx] + "");
                                            if (VNDictionary.getInstance.isSyllableVN(str))
                                                result.Add(str);
                                            str = (token.Remove(iToken, 1)).Insert(iToken, VN_Vowel_Arr[xx] + "");
                                            if (VNDictionary.getInstance.isSyllableVN(str))
                                                result.Add(str);
                                        }
                                    break;
                                case 8:  //i ---> o ô ơ u ư
                                    for (int xx = 5; xx <= 10; xx++)
                                        for (int yy = 0; yy < maxVN_Sign; yy++)
                                        {
                                            str = (token.Remove(iToken, 1)).Insert(iToken, VN_VowelWithSignMatrix[yy, xx] + "");
                                            if (VNDictionary.getInstance.isSyllableVN(str))
                                                result.Add(str);
                                            str = (token.Remove(iToken, 1)).Insert(iToken, VN_Vowel_Arr[xx] + "");
                                            if (VNDictionary.getInstance.isSyllableVN(str))
                                                result.Add(str);
                                        }
                                    break;
                                case 9:

                                case 10: //u ư ---> y i
                                    for (int xx = 8; xx <= 11; xx++)
                                        for (int yy = 0; yy < maxVN_Sign; yy++)
                                        {
                                            str = (token.Remove(iToken, 1)).Insert(iToken, VN_VowelWithSignMatrix[yy, xx] + "");
                                            if (VNDictionary.getInstance.isSyllableVN(str))
                                                result.Add(str);
                                            str = (token.Remove(iToken, 1)).Insert(iToken, VN_Vowel_Arr[xx] + "");
                                            if (VNDictionary.getInstance.isSyllableVN(str))
                                                result.Add(str);
                                        }
                                    break;
                                case 11: //y --> u ư
                                    for (int xx = 9; xx <= 11; xx++)
                                        for (int yy = 0; yy < maxVN_Sign; yy++)
                                        {
                                            str = (token.Remove(iToken, 1)).Insert(iToken, VN_VowelWithSignMatrix[yy, xx] + "");
                                            if (VNDictionary.getInstance.isSyllableVN(str))
                                                result.Add(str);
                                            str = (token.Remove(iToken, 1)).Insert(iToken, VN_Vowel_Arr[xx] + "");
                                            if (VNDictionary.getInstance.isSyllableVN(str))
                                                result.Add(str);
                                        }
                                    break;
                            }
                            break;
                            //}
                            //            }
                        }
            }

            return result;
        }

        /// <summary>
        /// Sinh candidate cho trường hợp nhầm lẫn vùng miền
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HashSet<string> create_regionConfusedCandidate(string token, bool isMajuscule)
        {

            string[,] vn_RegionConfusedMatrix;
            if (isMajuscule)
            {
                vn_RegionConfusedMatrix = StringConstant.getInstance.VNRegion_Confused_Matrix_UperCase;
            }
            else
            {
                vn_RegionConfusedMatrix = StringConstant.getInstance.VNRegion_Confused_Matrix_LowerCase;
            }
            HashSet<string> result = new HashSet<string>();
            List<string> listConsonants = countingConsonant(token, isMajuscule);
            bool isFound = false; //
            if (hasFirstConsonant(token, listConsonants))
            {
                for (int i = 0; i < StringConstant.MAXGROUP_REGION_CONFUSED; i++)
                {
                    for (int j = 0; j < StringConstant.MAXCASE_REGION_CONFUSED; j++)
                    {
                        string replacedConsonant = vn_RegionConfusedMatrix[i, j];
                        if (isFound)
                        {
                            if (replacedConsonant.Length > 0)
                            {
                                string tmp = (token.Remove(0, listConsonants[0].Length)).Insert(0, replacedConsonant);
                                if (VNDictionary.getInstance.isSyllableVN(tmp))
                                    result.Add(tmp);
                                if (j == StringConstant.MAXCASE_REGION_CONFUSED - 1)
                                    return result;
                            }
                            else
                            {
                                return result;
                            }
                        }
                        if (!isFound && listConsonants[0].Equals(vn_RegionConfusedMatrix[i, j]))
                        {
                            j = -1;
                            isFound = true;
                        }
                    }
                }
            }
            return result;
        }

        public bool hasFirstConsonant(string token, List<string> consonants)
        {
            if (consonants.Count != 0)
                if (token.Substring(0, consonants[0].Length).Equals(consonants[0]))
                {
                    return true;
                }
            return false;
        }
        private int vLookup(char c, bool isMajuscule)
        {
            char[,] keyBoardMatrix;
            if (isMajuscule)
            {
                keyBoardMatrix = StringConstant.getInstance.KeyBoardMatrix_UperCase;
            }
            else
            {
                keyBoardMatrix = StringConstant.getInstance.KeyBoardMatrix_LowerCase;
            }
            for (int i = 0; i < StringConstant.MAX_KEYBOARD_ROW; i++)
            {
                for (int j = 0; j < StringConstant.MAX_KEYBOARD_COL; j++)
                    if (c == keyBoardMatrix[i, j])
                        return j;
            }
            return -1;
        }

        private int hLookup(char c, bool isMajuscule)
        {
            char[,] keyBoardMatrix;
            if (isMajuscule)
            {
                keyBoardMatrix = StringConstant.getInstance.KeyBoardMatrix_UperCase;
            }
            else
            {
                keyBoardMatrix = StringConstant.getInstance.KeyBoardMatrix_LowerCase;
            }

            for (int i = 0; i < StringConstant.MAX_KEYBOARD_ROW; i++)
            {
                for (int j = 0; j < StringConstant.MAX_KEYBOARD_COL; j++)
                    if (c == keyBoardMatrix[i, j])
                        return i;
            }
            return -1;
        }
        #endregion

    }
}