﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
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
            getContext(iWord, words);
        }
        public Context(Word.Words words, Word.Sentences sentences)
        {
            string[] wordArr = sentences[1].Text.Split(' ');
            for (int i = 0; i < wordArr.Length; i++)
                if (wordArr[i].Trim().Equals(words[1].Text.Trim()))
                {
                    getContext(i, wordArr);
                    return;
                }
        }
        public void GetSeletedContext(Word.Words words, Word.Sentences sentences)
        {
            string[] wordArr = sentences[1].Text.Split(' ');
            for (int i = 0; i < wordArr.Length; i++)
                if (wordArr[i].Trim().Equals(words[1].Text.Trim()))
                {
                    getContext(i, wordArr);
                    return;
                }
        }
        public Context()
        {
            PREPRE = PRE = TOKEN = NEXT = NEXTNEXT = "";
        }

        public void getContext(int iWord, string[] words)
        {
            Regex regexSpecialChar = new Regex(StringConstant.Instance.patternCheckSpecialChar);
            Regex regexEndSentenceChar = new Regex(StringConstant.Instance.patternEndSentenceCharacter);
            PREPRE = PRE = NEXT = NEXTNEXT = "";
            int length = words.Length;
            if (iWord == 0)
                PRE = Ngram.Instance.START_STRING;
            if (iWord > 0)
                PRE = regexSpecialChar.Replace(words[iWord - 1].Trim(), "");
            if (iWord > 1)
                PREPRE = regexSpecialChar.Replace(words[iWord - 2].Trim(), "");
            if (iWord == length - 1)
                NEXT = Ngram.Instance.END_STRING;
            if (iWord < length - 1)
                NEXT = regexSpecialChar.Replace(words[iWord + 1].Trim(), "");
            if (iWord < length - 2)
                NEXTNEXT = regexSpecialChar.Replace(words[iWord + 2].Trim(), "");
            TOKEN = words[iWord].Trim();
            Regex regexEndMidSym = new Regex(StringConstant.Instance.patternEndMiddleSymbol);
            Match mEndMidSym = regexEndMidSym.Match(TOKEN);
            if (mEndMidSym.Success)
            {
                NEXT = Ngram.Instance.END_STRING;
                NEXTNEXT = "";
            }
            if (PRE.Equals(""))
            {
                PRE = Ngram.Instance.START_STRING;
                PREPRE = "";
            }
            if (NEXT.Equals(""))
            {
                NEXT = Ngram.Instance.START_STRING;
                NEXTNEXT = "";
            }
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
