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
        private FixError()
        {
            hSetCandidate = new HashSet<string>();
        }
        // 
        public static FixError Instance
        {
            get
            {
                return _instance;
            }
        }
        private static FixError _instance = new FixError();

        public HashSet<string> hSetCandidate
        {
            get; set;
        }
        public string Token
        {
            get; set;
        }
        public int Count
        {
            get
            {
                return hSetCandidate.Count;
            }
        }
        private Context _context;
        private string _candidate = "";
        public void getCandidatesWithContext(Context context, Dictionary<Context, Word.Range> dictError)
        {
            _context = context;
            hSetCandidate.Clear();
            if (dictError.Count > 0)
            {
                foreach (var key in dictError.Keys)
                    if (key.Equals(context))
                    {
                        //nếu có lỗi trong danh sách
                        if (dictError.Count > 0)
                        {
                            //lấy lỗi đầu tiên tìm được với startIndex
                            //Token = dictError[context].Text.ToLower().Trim();
                            Token = context.TOKEN;
                            hSetCandidate = Candidate.getInstance.createCandidate(context);
                            if (hSetCandidate.Count > 0)
                                _candidate = hSetCandidate.ElementAt(0);
                        }
                        return;
                    }
            }
        }
        public override string ToString()
        {
            string pp = _context.PREPRE.Equals(Ngram.Instance.START_STRING) ?
                "" : _context.PREPRE;
            string p = _context.PRE.Equals(Ngram.Instance.START_STRING) ?
                "" : _context.PRE;
            string n = _context.NEXT.Equals(Ngram.Instance.END_STRING) ?
                "" : _context.NEXT;
            string nn = _context.NEXTNEXT.Equals(Ngram.Instance.END_STRING) ?
                "" : _context.NEXTNEXT;
            return string.Format("{0} {1} {2} {3} {4}", pp, p, _candidate, n, nn).Trim();
        }
        public string getStringNewContext(string candidate)
        {
            string pp = _context.PREPRE.Equals(Ngram.Instance.START_STRING) ?
                "" : _context.PREPRE;
            string p = _context.PRE.Equals(Ngram.Instance.START_STRING) ?
                "" : _context.PRE;
            string n = _context.NEXT.Equals(Ngram.Instance.END_STRING) ?
                "" : _context.NEXT;
            string nn = _context.NEXTNEXT.Equals(Ngram.Instance.END_STRING) ?
                "" : _context.NEXTNEXT;
            return string.Format("{0} {1} {2} {3} {4}", pp, p, candidate, n, nn);
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
