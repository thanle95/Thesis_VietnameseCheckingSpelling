using System;
using System.Collections.Generic;
using System.Linq;
using Word = Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using Spell.Algorithm;
namespace Spell
{
    public class DocumentHandling
    {
        private static DocumentHandling instance = new DocumentHandling();
        private DocumentHandling()
        {

        }
        public static DocumentHandling Instance
        {
            get { return instance; }
        }
        public Word.Range HighLight_Mistake(Context context, Word.Sentences sentencesList, Word.WdColorIndex colorIndex, Word.WdColor color)
        {
            Word.Range range = null;
            //Word.Lines lines = Globals.ThisAddIn.Application.ActiveDocument.Words.l;
            Word.Sentences sentences = sentencesList;

            //start và end để chọn range highLight cho từ bị lỗi.
            int start = 0;
            int end = 0;
            for (int i = 1; i <= sentences.Count; i++)
            {

                if (!sentences[i].Text.Contains(context.TOKEN))
                    continue;
                string[] words = sentences[i].Text.Trim().Split(' ');
                start = sentences[i].Start;
                //if (sentences[i].Text.Length < sentences[i].Text.TrimEnd().Length)
                //    start++;
                end = 0;
                for (int j = 0; j < words.Length; j++)
                {
                    string word = words[j].Trim().ToLower();
                    if (words.Length < 1)
                        continue;
                    Regex r = new Regex(StringConstant.Instance.patternCheckSpecialChar);
                    Match m = r.Match(context.TOKEN);
                    if (m.Success)
                        continue;
                    else {
                        Context jContext = new Context(j, words);
                        //if (count == countWordInSentence - 1)
                        //{
                        //    endRange = Globals.ThisAddIn.Application.ActiveDocument.Range(start, start);
                        //    endRange.Select();
                        //}
                        //nếu từ có chứa những ký tự đặc biệt thì loại bỏ ký tự đó
                        string wordInArr = Regex.Replace(words[j], StringConstant.Instance.patternSignSentence, "");

                        end = start + wordInArr.Length;

                        if (words[j].Length != 0 && wordInArr.Length == 0)
                            end += 1;
                        if (context.Equals(jContext))
                        {
                            range = Globals.ThisAddIn.Application.ActiveDocument.Range(start, end);
                            range.HighlightColorIndex = colorIndex;
                            range.Font.Color = color;
                            range.Select();
                            return range;
                        }

                        start = end + 1 + Math.Abs(wordInArr.Length - words[j].Length); // bỏ qua khoảng trắng
                        if (words[j].Length != 0 && wordInArr.Length == 0)
                            start -= 1;

                    }

                }
            }
            return range;
        }
        public Word.Range HighLight_Mistake(int start, int end, Word.WdColorIndex colorIndex, Word.WdColor color)
        {
            Word.Range range = null;
            range = Globals.ThisAddIn.Application.ActiveDocument.Range(start, end);
            range.HighlightColorIndex = colorIndex;
            range.Font.Color = color;
            return range;
        }

        public Word.Range HighLight_MistakeWrongWord(Context context, Word.Sentences sentencesList)
        {
            return HighLight_Mistake(context, sentencesList, Word.WdColorIndex.wdRed, Word.WdColor.wdColorYellow);
        }
        public Word.Range HighLight_MistakeWrongWord(int start, int end)
        {
            return HighLight_Mistake(start, end, Word.WdColorIndex.wdRed, Word.WdColor.wdColorYellow);
        }
        public Word.Range HighLight_MistakeRightWord(Context context, Word.Sentences sentencesList)
        {
            return HighLight_Mistake(context, sentencesList, Word.WdColorIndex.wdYellow, Word.WdColor.wdColorAutomatic);
        }
        public Word.Range HighLight_MistakeRightWord(int start, int end)
        {
            return HighLight_Mistake(start, end , Word.WdColorIndex.wdYellow, Word.WdColor.wdColorAutomatic);
        }

        public void DeHighLight_All_Mistake(Word.Characters characters)
        {
            //int count = Globals.ThisAddIn.Application.ActiveDocument.Characters.Count;
            int count = characters.Count;
            Word.Range range = Globals.ThisAddIn.Application.ActiveDocument.Range(0, count);
            range.HighlightColorIndex = Word.WdColorIndex.wdNoHighlight;
            range.Font.Color = Word.WdColor.wdColorAutomatic;
        }
        public void DeHighLight_Mistake(int startIndex, int endIndex)
        {
            Word.Range range = Globals.ThisAddIn.Application.ActiveDocument.Range(startIndex, endIndex);
            range.HighlightColorIndex = Word.WdColorIndex.wdNoHighlight;
            range.Font.Color = Word.WdColor.wdColorAutomatic;
        }


        public string getActiveDocument(Word.Words words, int start, int end)
        {
            string ret = "";
            for (int i = start; i < end; i++)
                ret += words[i].Text + " ";
            return ret;
        }

        //
        //---Kiet Start
        //       

        // overload dehighlight
        public void DeHighLight_All_Mistake(int start, int end)
        {
            //int count = Globals.ThisAddIn.Application.ActiveDocument.Characters.Count;
            //int count = characters.Count;
            Word.Range range = Globals.ThisAddIn.Application.ActiveDocument.Range(start, end);
            range.HighlightColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdNoHighlight;
        }
        //
        //---Kiet End
        //
        public List<string> getSentence(int startIndex, int endIndex, Word.Sentences sentences)
        {
            List<string> ret = new List<string>();
            for (int i = 1; i <= sentences.Count; i++)
            {
                if (sentences[i].Start <= endIndex && sentences[i].End >= startIndex)
                {
                    ret.Add(sentences[i].Text);
                }
            }
            return ret;
        }
        /// <summary>
        /// trả về danh sách câu hiện hành
        /// </summary>
        /// <param name="sentences"></param>
        /// <returns></returns>
        public List<string> getPhrase(Word.Sentences sentences)
        {
            List<string> ret = new List<string>();
            for (int i = 1; i <= sentences.Count; i++)
            {
                string text = sentences[i].Text;

                string[] phraseArr = new Regex(StringConstant.Instance.patternSignSentence).Split(text);

                foreach (string iPharse in phraseArr)
                {
                    if (iPharse.Trim().Length > 0)
                        ret.Add(iPharse);
                }
            }
            return ret;
        }
        public List<string> getPhrase(Word.Range sentences)
        {
            List<string> ret = new List<string>();
            string sentence = sentences.Text;
            string[] phraseArr = new Regex(StringConstant.Instance.patternSignSentence).Split(sentence);

            foreach (string iPharse in phraseArr)
            {
                if (iPharse.Trim().Length > 0)
                    ret.Add(iPharse);
            }

            return ret;
        }

        public List<string> getWords(Word.Words words)
        {
            List<string> ret = new List<string>();
            for (int i = 1; i <= words.Count; i++)
                ret.Add(words[i].Text);
            return ret;
        }

        public bool checkEng(string token)
        {
            string extractedToken = Candidate.getInstance.extractSignVN(token);
            bool result = false;
            if (!VNDictionary.getInstance.isSyllableVN(token))
                foreach (char sign in StringConstant.Instance.VNSign)
                {
                    if (!extractedToken.Contains(sign))
                        result = true;
                }
            return result;
        }

    }
}
