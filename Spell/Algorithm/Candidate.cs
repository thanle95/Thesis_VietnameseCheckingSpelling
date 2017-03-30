using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spell.Algorithm
{
    class Candidate
    {
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
    }

}
