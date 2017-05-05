﻿using System;
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

        
        public Word.Range HighLight_Mistake(string wrongText, Word.Sentences sentencesList, int countWord, Word.WdColorIndex colorIndex, Word.WdColor color)
        {
            Word.Range range = null;

            Word.Sentences sentences = sentencesList;
            int count = 0;
            for (int i = 1; i <= sentences.Count; i++)
            {
                string[] words = sentences[i].Text.Trim().Split(' ');
                int start = sentences[i].Start;
                //if (sentences[i].Text.Length < sentences[i].Text.TrimEnd().Length)
                //    start++;
                int end = 0;
                foreach (string word in words)
                {
                    count++;

                    string wordInArr = Regex.Replace(word, StringConstant.Instance.patternSignSentence, "");

                    end = start + wordInArr.Length;
                    if (word.Length == 0)
                        end += 1;
                    if (count == countWord)
                    {
                        if (wordInArr.ToLower().Trim().Equals(wrongText.Trim().ToLower()))
                        {
                            range = Globals.ThisAddIn.Application.ActiveDocument.Range(start, end);
                            range.HighlightColorIndex = colorIndex;
                            range.Font.Color = color;
                            return range;
                        }
                    }
                    start = end + 1 + Math.Abs(wordInArr.Length - word.Length); // bỏ qua khoảng trắng
                    if (word.Length == 0)
                        start -= 1;
                }
            }
            return range;
        }
        
        public Word.Range HighLight_MistakeWrongWord(string wrongText, Word.Sentences sentencesList, int countWord)
        {
            return HighLight_Mistake(wrongText, sentencesList, countWord, Word.WdColorIndex.wdRed, Word.WdColor.wdColorYellow);
        }
       
        public Word.Range HighLight_MistakeRightWord(string wrongText, Word.Sentences sentencesList, int countWord)
        {
            return HighLight_Mistake(wrongText, sentencesList, countWord, Word.WdColorIndex.wdYellow, Word.WdColor.wdColorAutomatic);
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
