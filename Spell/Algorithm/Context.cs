using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spell.Algorithm
{
    public class Context
    {
        public string PREPRE
        {
            get; set;
        }
        public string PRE
        {
            get; set;
        }
        public string TOKEN
        {
            get; set;
        }
        public string NEXT
        {
            get; set;
        }
        public string NEXTNEXT
        {
            get; set;
        }
        public Context(int iWord, string[] words)
        {
            getGramArroundIWord(iWord, words);
        }


        private void getGramArroundIWord(int iWord, string[] words)
        {
            Regex regexSpecialChar = new Regex(StringConstant.Instance.patternCheckSpecialChar);
            Regex regexEndSentenceChar = new Regex(StringConstant.Instance.patternEndSentenceCharacter);
            PREPRE = PRE = NEXT = NEXTNEXT = "";
            int length = words.Length;
            if (iWord == 0)
                PRE = Ngram.Instance.START_STRING;
            if (iWord > 0)
                PRE = words[iWord - 1];
            if (iWord > 1)
                PREPRE = words[iWord - 2];
            if (iWord == length - 1)
                NEXT = Ngram.Instance.END_STRING;
            if (iWord < length - 1)
                NEXT = regexEndSentenceChar.Replace(words[iWord + 1], "");
            if (iWord < length - 2)
                NEXTNEXT = regexEndSentenceChar.Replace(words[iWord + 2], "");

            if (PRE.Length > 0 && iWord != 1) //pre không phải từ đầu câu
            {
                Match m = regexSpecialChar.Match(PRE);
                if (m.Success | char.IsUpper(PRE.Trim()[0]))
                {
                    PRE = Ngram.Instance.START_STRING;
                    PREPRE = "";
                }
            }
            if (NEXT.Length > 0)
            {
                Match m = regexSpecialChar.Match(NEXT);
                if (m.Success)
                {
                    NEXT = Ngram.Instance.END_STRING;
                    NEXTNEXT = "";
                }

            }
            if (PREPRE.Length > 0)
            {
                Match m = regexSpecialChar.Match(PREPRE);
                if (m.Success | char.IsUpper(PREPRE.Trim()[0]))
                {
                    PREPRE = "";
                }
            }
            if (NEXTNEXT.Length > 0)
            {
                Match m = regexSpecialChar.Match(NEXTNEXT);
                if (m.Success)
                {
                    NEXTNEXT = "";
                }
            }
            TOKEN = words[iWord];
        }
        public override bool Equals(object obj)
        {
            Context c;
            if (obj.GetType() != this.GetType())
                return false;
            else c = (Context)obj;
            if (!PREPRE.Equals(c.PREPRE))
                return false;
            if (!PRE.Equals(c.PRE))
                return false;
            if (!TOKEN.Equals(c.TOKEN))
                return false;
            if (!NEXT.Equals(c.NEXT))
                return false;
            if (!NEXTNEXT.Equals(c.NEXTNEXT))
                return false;
            return true;
        }
    }
}
