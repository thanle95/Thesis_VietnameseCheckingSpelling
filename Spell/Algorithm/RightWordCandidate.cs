using System;
using System.Collections.Generic;
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

        public HashSet<string> createCandidate(string prepre, string pre, string token, string next,string nextnext, bool isMajuscule)
        {
            HashSet<string> hSetCandidate = new HashSet<string>();
            hSetCandidate = WrongWordCandidate.getInstance.createCandidate(prepre,pre, token, next,nextnext, isMajuscule);
            return hSetCandidate;
        }
    }
}
