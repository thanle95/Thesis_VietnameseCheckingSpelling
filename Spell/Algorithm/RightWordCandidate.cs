using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spell.Algorithm;

namespace Spell.Algorithm
{
    public class RightWordCandidate
    {

        private string rightScore = @"\Resources\rightWord.txt";
        private string rightCand = @"\Resources\rightWordCandidate.txt";
        private string rightScorePath;
        private string rightCandPath;
        private static RightWordCandidate instance = new RightWordCandidate();
        private RightWordCandidate()
        {
            rightScorePath = Environment.CurrentDirectory + rightScore;
            rightCandPath = Environment.CurrentDirectory + rightCand;
        }
        public static RightWordCandidate getInstance
        {
            get
            {
                return instance;
            }
        }
        /// <summary>
        /// kiểm tra 1 từ đúng âm tiết tiếng Việt có hợp ngữ cảnh hay không
        /// </summary>
        /// <param name="prepre"></param>
        /// <param name="pre"></param>
        /// <param name="token"></param>
        /// <param name="next"></param>
        /// <param name="nextnext"></param>
        /// <returns></returns>
        public bool checkRightWord(string prepre, string pre, string token, string next, string nextnext)
        {
            double L = WrongWordCandidate.getInstance.calScore_Ngram(prepre, pre, token, next, nextnext);
            using (FileStream aFile = new FileStream((rightScorePath), FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(aFile))
            {
                sw.WriteLine();
                sw.WriteLine(token + "-----" + L);
                sw.WriteLine("**********************************************************************");
            }
            if (L > WrongWordCandidate.getInstance.LIM_LANGUAGEMODEL)
                return true;
            return false;
        }
        /// <summary>
        /// tạo những candidate dựa trên ngữ cảnh và độ tương tự
        /// </summary>
        /// <param name="prepre"></param>
        /// <param name="pre"></param>
        /// <param name="token"></param>
        /// <param name="next"></param>
        /// <param name="nextnext"></param>
        /// <param name="isMajuscule"></param>
        /// <returns></returns>
        public HashSet<string> createCandidate(string prepre, string pre, string token, string next, string nextnext, bool isMajuscule)
        {
            HashSet<string> hSetCandidate = new HashSet<string>();
            Dictionary<string, double> candidatesWithScore = new Dictionary<string, double>(), tempCandidatesWithScore = new Dictionary<string, double>();
            HashSet<string> result = new HashSet<string>();
            hSetCandidate.UnionWith(WrongWordCandidate.getInstance.createCandidateByNgram(prepre, pre, token, next, nextnext, isMajuscule));
            hSetCandidate.UnionWith(WrongWordCandidate.getInstance.createCandByCompoundWord(prepre, pre, token, next, nextnext, isMajuscule));
            int D = 0;
            double S = 0;
            double L = 0.0;
            double score = 0;
            string text = "";
            double lamda1 = 0.000003;
            double lamda2 = 0.99999;
            double lamda3 = 0.000007;
            foreach (string candidate in hSetCandidate)
            {
                S = WrongWordCandidate.getInstance.calScore_Similarity(token, candidate);
                if (S > WrongWordCandidate.getInstance.LIM_SIMILARITY)
                {
                    L = WrongWordCandidate.getInstance.calScore_Ngram(prepre, pre, candidate, next, nextnext);

                    D = WrongWordCandidate.getInstance.calScore_CompoundWord(prepre, pre, candidate, next, nextnext);
                    score = lamda1 * D + lamda2 * L + lamda3 * S;
                    if (S > WrongWordCandidate.getInstance.LIM_SIMILARITY || L > WrongWordCandidate.getInstance.LIM_LANGUAGEMODEL)
                    {
                        //nếu số lượng phần tử còn nhỏ hơn 5
                        if (candidatesWithScore.Count < 5)
                        {
                            candidatesWithScore.Add(candidate, score);
                            candidatesWithScore = sortDict(candidatesWithScore);
                        }
                        //nếu phần tử cuối cùng có điểm thấp hơn candidate hiện tại
                        else if (candidatesWithScore.Last().Value < score)
                        {
                            candidatesWithScore.Remove(candidatesWithScore.Last().Key);
                            candidatesWithScore.Add(candidate, score);
                            candidatesWithScore = sortDict(candidatesWithScore);
                        }
                        text += String.Format("{0}: [{1};{2},{3}] = {3}", candidate, L, S, D, score) + "\n";
                    }
                }
            }
            foreach (string key in candidatesWithScore.Keys)
                result.Add(key);
            //ghi đè file
            using (FileStream aFile = new FileStream((rightCandPath), FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(aFile))
            {
                sw.WriteLine(text);
                sw.WriteLine("**********************************************************************");
            }
            return result;
        }
        /// <summary>
        /// sắp xếp candidate dựa trên số điểm, candidate có điểm cao nhất sẽ ở vị trí đầu tiên
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private Dictionary<string, double> sortDict(Dictionary<string, double> dict)
        {
            List<KeyValuePair<string, double>> myList = dict.OrderByDescending(pair => pair.Value).ToList();
            Dictionary<string, double> ret = new Dictionary<string, double>();
            foreach (KeyValuePair<string, double> pair in myList)
                ret.Add(pair.Key, pair.Value);
            return ret;
        }
    }
}
