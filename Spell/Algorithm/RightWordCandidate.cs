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
            double L = Candidate.getInstance.calScore_Ngram(prepre, pre, token, next, nextnext);
            using (FileStream aFile = new FileStream((rightScorePath), FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(aFile))
            {
                sw.WriteLine();
                sw.WriteLine(token + "-----" + L);
                sw.WriteLine("**********************************************************************");
            }
            if (L > Candidate.getInstance.LIM_LANGUAGEMODEL)
                return true;
            return false;
        }
        /// <summary>
        /// +
        /// n
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
            HashSet<string> result = new HashSet<string>();
            //giữ cặp <candidate, điểm> để so sánh
            Dictionary<string, double> candidatesWithScore = new Dictionary<string, double>(),
                //giữ cặp <candidate, điểm> với những candidate là từ ghép 3 âm tiết
                prioritizedCandidatesWithScore = new Dictionary<string, double>();
            //candidate chưa chọn lọc dựa vào số điểm
            HashSet<string> hSetCandidate = new HashSet<string>();

            hSetCandidate.UnionWith(Candidate.getInstance.createCandidateByNgram(prepre, pre, token, next, nextnext, isMajuscule));
            hSetCandidate.UnionWith(Candidate.getInstance.createCandByCompoundWord(prepre, pre, token, next, nextnext, isMajuscule));
            //giá trị lamda có được do thống kê
            double lamda1 = 0.2;
            double lamda2 = 0.4;
            double lamda3 = 0.4;
            double score = 0;
            //Dictionary
            double D = 0;
            //Language model
            double L = 0.0;
            //Similarity
            double S = 0;
            string text_writeFile = "";
            foreach (string candidate in hSetCandidate)
            {
                S = Candidate.getInstance.calScore_Similarity(token, candidate);
                if (S > Candidate.getInstance.LIM_SIMILARITY)
                {
                    D = Candidate.getInstance.calScore_CompoundWord(prepre, pre, candidate, next, nextnext);
                    L = Candidate.getInstance.calScore_Ngram(prepre, pre, candidate, next, nextnext);

                    score = lamda1 * D + lamda2 * L + lamda3 * S;
                    if (score > Candidate.getInstance.MAX_SCORE)
                        score = Candidate.getInstance.MAX_SCORE;
                    //ngưỡng để chọn candidate có được do thống kê
                    if (L > Candidate.getInstance.LIM_LANGUAGEMODEL)
                    {
                        //là từ ghép 3 âm tiết, hoặc rất giống với token
                        if (D == Candidate.getInstance.MAX_SCORE || S == Candidate.getInstance.MAX_SCORE)
                        {
                            //nếu số lượng phần tử còn nhỏ hơn 5
                            if (prioritizedCandidatesWithScore.Count < 5)
                            {
                                prioritizedCandidatesWithScore.Add(candidate, score);
                                prioritizedCandidatesWithScore = Candidate.getInstance.sortDict(prioritizedCandidatesWithScore);
                            }
                            //nếu phần tử cuối cùng có số điểm thấp hơn candidate hiện tại
                            else if (prioritizedCandidatesWithScore.Last().Value < score)
                            {
                                prioritizedCandidatesWithScore.Remove(prioritizedCandidatesWithScore.Last().Key);
                                prioritizedCandidatesWithScore.Add(candidate, score);
                                prioritizedCandidatesWithScore = Candidate.getInstance.sortDict(prioritizedCandidatesWithScore);
                            }
                        }
                        //không phải từ ghép 3 âm tiết
                        else
                        {
                            //nếu số lượng phần tử còn nhỏ hơn 5
                            if (candidatesWithScore.Count < 5)
                            {
                                candidatesWithScore.Add(candidate, score);
                                candidatesWithScore = Candidate.getInstance.sortDict(candidatesWithScore);
                            }
                            //nếu phần tử cuối cùng có số điểm thấp hơn candidate hiện tại
                            else if (candidatesWithScore.Last().Value < score)
                            {
                                candidatesWithScore.Remove(candidatesWithScore.Last().Key);
                                candidatesWithScore.Add(candidate, score);
                                candidatesWithScore = Candidate.getInstance.sortDict(candidatesWithScore);
                            }
                        }
                        text_writeFile += String.Format("{0}: [{1};{2};{3}] = {4}", candidate, D, L, S, score) + "\n";
                    }
                }
            }
            //nếu có từ ghép 3 âm tiết
            if (prioritizedCandidatesWithScore.Count > 0)
                foreach (string key in prioritizedCandidatesWithScore.Keys)
                    result.Add(key);
            //không có từ ghép 3 âm tiết
            else
                foreach (string key in candidatesWithScore.Keys)
                    result.Add(key);

            //ghi đè file
            using (FileStream aFile = new FileStream((rightCandPath), FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(aFile))
            {
                sw.WriteLine(text_writeFile);
                sw.WriteLine("**********************************************************************");
            }
            return result;
        }
        
    }
}
