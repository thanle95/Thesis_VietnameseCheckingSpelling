using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            hSetCandidate = WrongWordCandidate.getInstance.createCandidate(prepre,pre, token, next,nextnext, isMajuscule);
            return hSetCandidate;
        }
    }
}
