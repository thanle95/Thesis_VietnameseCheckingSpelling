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
        public Word.Range GetWordByCursorSelection()
        {
            Word.Words words = Globals.ThisAddIn.Application.Selection.Words;
       
            Word.Range range = null;
            
            range = Globals.ThisAddIn.Application.ActiveDocument.Range(words.First.Start, words.First.End);
            return range;
        }
        public Word.Range UnderlineWord(Context context, Word.Sentences sentencesList, Word.WdColorIndex colorIndex, Word.WdColor color)
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
        public Word.Range UnderlineWord(string token, int start, int end, Word.WdColor color)
        {
            Word.Range range = null;
            range = Globals.ThisAddIn.Application.ActiveDocument.Range(start, end);
            while (!range.Text.Equals(token))
            {
                range = Globals.ThisAddIn.Application.ActiveDocument.Range(++start, ++end);
            }
            //range.HighlightColorIndex = colorIndex;
            //range.Font.Color = color;
            range.Underline = Word.WdUnderline.wdUnderlineWavy;
            range.Font.UnderlineColor = color;
            return range;
        }

        public Word.Range UnderlineWrongWord(Context context, Word.Sentences sentencesList)
        {
            return UnderlineWord(context, sentencesList, Word.WdColorIndex.wdRed, Word.WdColor.wdColorYellow);
        }
        public Word.Range UnderlineWrongWord(string token, int start, int end)
        {
            return UnderlineWord(token, start, end, Word.WdColor.wdColorRed);
        }
        public Word.Range UnderlineRightWord(Context context, Word.Sentences sentencesList)
        {
            return UnderlineWord(context, sentencesList, Word.WdColorIndex.wdYellow, Word.WdColor.wdColorAutomatic);
        }
        public Word.Range UnderlineRightWord(string token, int start, int end)
        {
            return UnderlineWord(token, start, end ,Word.WdColor.wdColorBlue);
        }

        public void RemoveUnderline_AllMistake()
        {
            int count = Globals.ThisAddIn.Application.ActiveDocument.Characters.Count;
            Word.Range range = Globals.ThisAddIn.Application.ActiveDocument.Range(0, count);
            range.Underline = Word.WdUnderline.wdUnderlineNone;
        }
        public void RemoveUnderline_Mistake(Word.Range range)
        {
            range.Underline = Word.WdUnderline.wdUnderlineNone;
        }
        public void RemoveUnderline_Mistake(string fixText, int startIndex, int endIndex)
        {
            Word.Range range = Globals.ThisAddIn.Application.ActiveDocument.Range(startIndex, endIndex);
            while (!range.Text.Equals(fixText))
            {
                range = Globals.ThisAddIn.Application.ActiveDocument.Range(++startIndex, ++endIndex);
            }
            //range.HighlightColorIndex = Word.WdColorIndex.wdNoHighlight;
            //range.Font.Color = Word.WdColor.wdColorAutomatic;
            range.Underline = Word.WdUnderline.wdUnderlineNone;
        }
        public void RemoveUnderline_Mistake( int startIndex, int endIndex)
        {
            Word.Range range = Globals.ThisAddIn.Application.ActiveDocument.Range(startIndex, endIndex);
            range.Underline = Word.WdUnderline.wdUnderlineNone;
        }
        public void HighLightCheckedRange(Word.Range range)
        {
            range.HighlightColorIndex = Word.WdColorIndex.wdGray25;
        }
        public void RemoveHighlighChecked()
        {
            Word.Range range = Globals.ThisAddIn.Application.ActiveDocument.Range(0, Globals.ThisAddIn.Application.ActiveDocument.Characters.Count);
            range.HighlightColorIndex = Word.WdColorIndex.wdNoHighlight;
        }
        public void RemoveHighlighChecked(int start, int end)
        {
            Word.Range range = Globals.ThisAddIn.Application.ActiveDocument.Range(start, end);
            range.HighlightColorIndex = Word.WdColorIndex.wdNoHighlight;
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
        public void RemoveHighLight(int start, int end)
        {
            Word.Range range = Globals.ThisAddIn.Application.ActiveDocument.Range(start, end);
            range.HighlightColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdNoHighlight;
        }
        public void HighLight(int start, int end)
        {
            Word.Range range = Globals.ThisAddIn.Application.ActiveDocument.Range(start, end);
            range.HighlightColorIndex = Word.WdColorIndex.wdYellow;
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
