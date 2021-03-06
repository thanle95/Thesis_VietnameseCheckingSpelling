﻿using System;
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

        private static RightWordCandidate instance = new RightWordCandidate();
        private RightWordCandidate()
        {
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
        public bool checkRightWord(Context context)
        {
            double D = Candidate.getInstance.calScore_CompoundWord(context, context.TOKEN);
            double L = Candidate.getInstance.calScore_NgramForFindError(context);
            //using (FileStream aFile = new FileStream((FileManager.Instance.RightWordScore), FileMode.Open, FileAccess.Write))
            //using (StreamWriter sw = new StreamWriter(aFile))
            //{
            //    sw.WriteLine();
            //    sw.WriteLine(String.Format("{0}: [{1};{2}]", context.TOKEN, L, D));
            //    sw.WriteLine("**********************************************************************");
            //}
            if ( D >= Candidate.getInstance.LIM_COMPOUNDWORD)
                return true;
            
            if (L >= Candidate.getInstance.LIM_LANGUAGEMODEL)
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
        public HashSet<string> createCandidate(Context context)
        {
            HashSet<string> result = new HashSet<string>();
            //giữ cặp <candidate, điểm> để so sánh
            Dictionary<string, double> candidatesWithScore = new Dictionary<string, double>(),
                //giữ cặp <candidate, điểm> với những candidate là từ ghép 3 âm tiết
                prioritizedCandidatesWithScore = new Dictionary<string, double>();
            //candidate chưa chọn lọc dựa vào số điểm
            HashSet<string> hSetCandidate = new HashSet<string>();

            hSetCandidate.UnionWith(Candidate.getInstance.createCandidateByNgram_NoUseLamdaExp(context));
            
            hSetCandidate.UnionWith(Candidate.getInstance.createCandByCompoundWord(context));
            //giá trị lamda có được do thống kê
            double lamda1 = 0.3;
            double lamda2 = 0.3;
            double lamda3 = 0.4;
            double score = 0;
            //Dictionary
            double D = 0;
            //Language model
            double L = 0.0;
            //Similarity
            double S = 0;
            //string text_writeFile = "";
            foreach (string candidate in hSetCandidate)
            {
                S = Candidate.getInstance.calScore_Similarity(context.TOKEN, candidate);
                if (S >= Candidate.getInstance.LIM_SIMILARITY)
                {
                    D = Candidate.getInstance.calScore_CompoundWord(context, candidate);
                    L = Candidate.getInstance.calScore_Ngram(context, candidate);

                    score = lamda1 * D + lamda2 * L + lamda3 * S;
                    if (score > Candidate.getInstance.MAX_SCORE)
                        score = Candidate.getInstance.MAX_SCORE;
                    //ngưỡng để chọn candidate có được do thống kê
                    //if (L >= Candidate.getInstance.LIM_LANGUAGEMODEL || D >= Candidate.getInstance.LIM_COMPOUNDWORD)
                    if (score >= Candidate.getInstance.LIM_SCORE)
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
                    //text_writeFile += String.Format("{0}: [{1};{2};{3}] = {4}", candidate, D, L, S, score) + "\n";
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
            //using (FileStream aFile = new FileStream((FileManager.Instance.RightWordCand), FileMode.Append, FileAccess.Write))
            //using (StreamWriter sw = new StreamWriter(aFile))
            //{
            //    sw.WriteLine(text_writeFile);
            //    sw.WriteLine("**********************************************************************");
            //}
            return result;
        }

    }
}
