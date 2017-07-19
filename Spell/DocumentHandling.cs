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

        public Word.Range UnderlineWrongWord(string token, int start, int end)
        {
            return UnderlineWord(token, start, end, Word.WdColor.wdColorRed);
        }
        public Word.Range UnderlineRightWord(string token, int start, int end)
        {
            return UnderlineWord(token, start, end, Word.WdColor.wdColorBlue);
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
        public void RemoveUnderline_Mistake(int startIndex, int endIndex)
        {
            Word.Range range = Globals.ThisAddIn.Application.ActiveDocument.Range(startIndex, endIndex);
            range.Underline = Word.WdUnderline.wdUnderlineNone;
        }
        public void EmphasizeCurrentError(Word.Range range)
        {
            range.Font.Size = 18;
        }
        public void RemoveEmphasizeCurrentError(Word.Range range, float fontSize)
        {
            range.Font.Size = fontSize;
        }
        public void RemoveEmphasizeCurrentError(int start, string text, float fontSize)
        {
            Word.Range range = Globals.ThisAddIn.Application.ActiveDocument.Range(start, start + text.Length);
            range.Font.Size = fontSize;
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
