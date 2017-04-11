using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using System.Windows.Forms;
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

        public Word.Range HighLight_Mistake(string wrongText, Word.Words wordList, Word.WdColorIndex colorIndex, Word.WdColor color, int countWord)
        {
            Word.Range range = null;
            Word.Words words = wordList;
            for (int i = countWord; i <= words.Count; i++)
            {
                if (words[i].Text.ToLower().Trim().Equals(wrongText.Trim().ToLower()))
                {
                    int start = words[i].Start;
                    int end = words[i].End;
                    if (words[i].Text.Contains(" "))
                        end--;
                    range = Globals.ThisAddIn.Application.ActiveDocument.Range(start, end);
                    range.HighlightColorIndex = colorIndex;
                    range.Font.Color = color;
                    break;
                }
            }
            return range;
        }
        public Word.Range HighLight_MistakeWrongWord(string wrongText, Word.Words wordList, int countWord)
        {
            return HighLight_Mistake(wrongText, wordList, Word.WdColorIndex.wdRed, Word.WdColor.wdColorYellow, countWord);
        }
        public Word.Range HighLight_MistakeRightWord(string wrongText, Word.Words wordList, int countWord)
        {
            return HighLight_Mistake(wrongText, wordList, Word.WdColorIndex.wdYellow, Word.WdColor.wdColorAutomatic,countWord);
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

                string[] phraseArr = new Regex(StringConstant.Instance.patternMiddleSymbol).Split(text);

                foreach (string iPharse in phraseArr)
                {
                    Regex r = new Regex(StringConstant.Instance.patternEndSentenceCharacter);
                    Match m = r.Match(iPharse);
                    if (m.Success)
                    //nếu chứa ký tự kết thúc câu
                    {
                        //bỏ dấu, vì dấu ở đằng sau, nên lấy phần tử đầu tiên
                        string tmp = new Regex(StringConstant.Instance.patternEndSentenceCharacter).Split(iPharse.Trim())[0];
                        if (tmp != "")
                            ret.Add(tmp);
                    }
                    else
                    {
                        ret.Add(iPharse);
                    }
                }
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


    }
}
