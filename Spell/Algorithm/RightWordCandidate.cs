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
        private RightWordCandidate()
        {
        }

        private static RightWordCandidate instance = new RightWordCandidate();
        public static RightWordCandidate getInstance
        {
            get
            {
                return instance;
            }
        }

        public bool checkRightWord(string prepre, string pre, string token, string next, string nextnext)
        {
            double calBiGram_PreCand = Ngram.Instance.calBiNgram(pre, token);
            double calBigram_CandNext = Ngram.Instance.calBiNgram(token, next);
            double calTrigram1 = Ngram.Instance.calTriNgram(prepre, pre, token);
            double calTrigram2 = Ngram.Instance.calTriNgram(pre, token, next);
            double calTrigram3 = Ngram.Instance.calTriNgram(token, next, nextnext);
            double ret = calBiGram_PreCand + calBigram_CandNext + calTrigram1 + calTrigram2 + calTrigram3;

            using (FileStream aFile = new FileStream((@"C:\Users\Kiet\OneDrive\Thesis\rightWord.txt"), FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(aFile))
            {
                //foreach (string temp in hsetNgramCand)
                //{
                sw.WriteLine();
                    sw.WriteLine(token + "-----" +ret);
                //}

                sw.WriteLine("**********************************************************************");
            }
            //File.WriteAllText(@"C:\Users\Kiet\OneDrive\Thesis\test.txt", text);

            if (ret >  1)
                return true;
            return false;
        }

        public HashSet<string> createCandidate(string prepre, string pre, string token, string next,string nextnext, bool isMajuscule)
        {
            HashSet<string> hSetCandidate = new HashSet<string>();
            Dictionary<string, double> candidatesWithScore = new Dictionary<string, double>(), tempCandidatesWithScore = new Dictionary<string, double>();
            HashSet<string> result = new HashSet<string>();
            hSetCandidate = WrongWordCandidate.getInstance.createCandidate(prepre, pre, token, next, nextnext, isMajuscule);
            hSetCandidate.UnionWith(WrongWordCandidate.getInstance.createCandidateByNgram(prepre, pre, token, next, nextnext, isMajuscule));
            int D = 0;
            double H = 0;
            double L = 0.0;
            double score = 0;
            string text = "";
            foreach (string candidate in hSetCandidate)
            {
                L = WrongWordCandidate.getInstance.calNgram(prepre, pre, candidate, next, nextnext);
                H = WrongWordCandidate.getInstance.calDeviation(token, candidate);
                D = WrongWordCandidate.getInstance.calMatch_pre_next_compoundWordVNDict(prepre, pre, candidate, next, nextnext);
                score = D + L +  H;
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

            return hSetCandidate;
        }

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
