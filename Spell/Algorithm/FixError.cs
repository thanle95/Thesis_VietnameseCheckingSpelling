using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public void getCandidatesWithContext(Context context, Dictionary<Context, Word.Range> dictError)
        {
            hSetCandidate = new HashSet<string>();
            if (dictError.Count > 0)
                if (dictError.ContainsKey(context))
                {
                    //nếu có lỗi trong danh sách
                    if (dictError.Count > 0)
                    {
                        //lấy lỗi đầu tiên tìm được với startIndex
                        //Token = dictError[context].Text.ToLower().Trim();
                        Token = context.TOKEN;
                        hSetCandidate = Candidate.getInstance.selectiveCandidate(context);
                    }
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
