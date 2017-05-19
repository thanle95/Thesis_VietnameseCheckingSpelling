using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace Spell.Algorithm
{
    class FixError
    {
        public FixError()
        {
            //getCandidatesWithStartIndex(startIndex, dictError, mySentences);
        }
        public HashSet<string> hSetCandidate
        {
            get; set;
        }
        public string Token
        {
            get; set;
        }
        public void getCandidatesWithStartIndex(int startIndex, Dictionary<Context, Word.Range> dictError, List<string> mySentences)
        {
            hSetCandidate = new HashSet<string>();
            //nếu có lỗi trong danh sách
            if (dictError.Count > 0)
            {
                //lấy lỗi đầu tiên tìm được với startIndex
                Word.Range tokenRange = findErrorRangeByStartIndex(startIndex, dictError);
                Token = tokenRange.Text.Trim().ToLower();

                //if(token.Length == 0)
                //{
                //    lblWrong.Text = ERROR_SPACE;
                //    lstbCandidate.Items.Add("");
                //    return;
                //}

                Regex regexEndSentenceChar = new Regex(StringConstant.Instance.patternSignSentence);
                Context context = dictError.First(kvp => kvp.Value == tokenRange).Key;
                foreach (string mySentence in mySentences)
                {
                    if (!mySentence.Contains(Token))
                        continue;
                    string[] words = mySentence.Trim().Split(' ');
                    int i = 0;
                    foreach (string word in words)
                    {
                        Context iContext = new Context(i, words);
                        if (context.Equals(iContext))
                        {
                            //string wordInWords = regexEndSentenceChar.Replace(word, "");
                            //if (wordInWords.Trim().ToLower().Equals(Token))
                            //{

                            hSetCandidate = Candidate.getInstance.selectiveCandidate(iContext);
                            return;
                            //}
                        }
                        i++;
                    } //end if compare to find token
                } // end for
            }
        }
        public void getCandidatesWithContext(Context context, Dictionary<Context, Word.Range> dictError)
        {
            hSetCandidate = new HashSet<string>();
            //nếu có lỗi trong danh sách
            if (dictError.Count > 0)
            {
                //lấy lỗi đầu tiên tìm được với startIndex
                Token = dictError[context].Text.ToLower().Trim();

                hSetCandidate = Candidate.getInstance.selectiveCandidate(context);
            }
        }
        private Word.Range findErrorRangeByStartIndex(int startIndex, Dictionary<Context, Word.Range> dictError)
        {
            List<Word.Range> temp = new List<Word.Range>();
            foreach (Word.Range range in dictError.Values)
            {
                if (range != null)
                    if (range.Start <= startIndex && startIndex <= range.End)
                    {
                        temp.Add(range);
                        break;
                    }
            }
            if (temp.Count == 0)
                temp.Add(dictError.Values.First());
            return temp.First();
        }
    }
}
