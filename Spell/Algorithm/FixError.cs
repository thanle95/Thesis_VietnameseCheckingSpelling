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
        public void getCandidatesWithStartIndex(int startIndex, Dictionary<int, Word.Range> dictError, List<string> mySentences)
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
                int countWord = dictError.First(kvp => kvp.Value == tokenRange).Key;
                int count = 0;
                foreach (string mySentence in mySentences)
                {
                    string[] words = mySentence.Trim().Split(' ');
                    int i = 0;
                    foreach (string word in words)
                    {
                        if (word.Length > 0)
                            count++;
                        if (countWord == count)
                        {
                            string wordInWords = regexEndSentenceChar.Replace(word, "");
                            if (wordInWords.Trim().ToLower().Equals(Token))
                            {
                                Context context = new Context(i, words);
                                hSetCandidate= Candidate.getInstance.selectiveCandidate(context);
                                return;
                            }
                        }
                        i++;
                    } //end if compare to find token
                } // end for
            }
        }
        public void getCandidatesWithCountWord(int countWord, Dictionary<int, Word.Range> dictError, List<string> mySentences)
        {
            hSetCandidate = new HashSet<string>();
            //nếu có lỗi trong danh sách
            if (dictError.Count > 0)
            {
                //lấy lỗi đầu tiên tìm được với startIndex
                Token = dictError[countWord].Text.ToLower().Trim() ;

                //if(token.Length == 0)
                //{
                //    lblWrong.Text = ERROR_SPACE;
                //    lstbCandidate.Items.Add("");
                //    return;
                //}

                Regex regexEndSentenceChar = new Regex(StringConstant.Instance.patternSignSentence);
                int count = 0;
                foreach (string mySentence in mySentences)
                {
                    string[] words = mySentence.Trim().Split(' ');
                    int i = 0;
                    foreach (string word in words)
                    {
                        if (word.Length > 0)
                            count++;
                        if (countWord == count)
                        {
                            string wordInWords = regexEndSentenceChar.Replace(word, "");
                            if (wordInWords.Trim().ToLower().Equals(Token))
                            {
                                Context context = new Context(i, words);
                                hSetCandidate = Candidate.getInstance.selectiveCandidate(context);
                                return;
                            }
                        }
                        i++;
                    } //end if compare to find token
                } // end for
            }
        }
        private Word.Range findErrorRangeByStartIndex(int startIndex, Dictionary<int, Word.Range> dictError)
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
