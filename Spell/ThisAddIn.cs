using Spell.Algorithm;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using System;

namespace Spell
{
    public partial class ThisAddIn
    {
        Word.Application myApplication;
        private string TAG = "CANDIDATE";
        private int count = 1;
        private Office.CommandBarButton myControl;
        private string WrongWord
        {
            get; set;
        }
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            VNDictionary.getInstance.runFirst();
            Ngram.Instance.runFirst();
            myApplication = this.Application;
            //AddMenuItem();
            myApplication.WindowBeforeRightClick +=
                new Word.ApplicationEvents4_WindowBeforeRightClickEventHandler(application_WindowBeforeRightClick);

        }
        private void RemoveExistingMenuItem()
        {
            Office.CommandBar contextMenu = myApplication.CommandBars["Text"];
            //myApplication.CustomizationContext = customTemplate;
            while (true)
            {
                Office.CommandBarButton control =
                    (Office.CommandBarButton)contextMenu.FindControl
                    (Office.MsoControlType.msoControlButton, missing,
                    TAG, true, true);
                if (control == null)
                    return;
                else
                    control.Delete(true);
            }

        }
        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {

        }

        void myControl_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            UserControl.Instance.Start(false);
            UserControl.Instance.change(WrongWord, Ctrl.Caption, true);
        }
        public void application_WindowBeforeRightClick(Word.Selection selection, ref bool Cancel)
        {
            RemoveExistingMenuItem();
            Word.Words words = Globals.ThisAddIn.Application.Selection.Words;
            Word.Sentences sentences = Globals.ThisAddIn.Application.Selection.Sentences;
            FixError fixError = new FixError();
            FindError.Instance.GetSeletedContext(words, sentences);
            fixError.getCandidatesWithContext(FindError.Instance.SelectedError_Context, FindError.Instance.lstErrorRange);
            WrongWord = fixError.Token.ToLower();
            string[] candidateArr = new string[fixError.hSetCandidate.Count];
            int i = 0;
            string candidate = "";
            if (fixError.hSetCandidate.Count > 0)
            {
                foreach (string item in fixError.hSetCandidate)

                    candidateArr[i++] = item;
                for (i = fixError.hSetCandidate.Count - 1; i >= 0; i--)
                {
                    candidate = candidateArr[i];
                    if (!candidate.ToLower().Equals(fixError.Token.ToLower()))
                        if (candidate.Length > 1)
                            addCandidate(candidate.Trim());
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(SysMessage.Instance.IsNotError(FindError.Instance.SelectedError_Context.TOKEN));
            }
            //for (int i = 0; i < 3; i++)
            //{

            //}
        }
        private void addCandidate(string candidate)
        {

            Office.MsoControlType menuItem =
                       Office.MsoControlType.msoControlButton;

            myControl =
                (Office.CommandBarButton)myApplication.CommandBars["Text"].Controls.Add
                (menuItem, missing, missing, 1, true);
            myControl.Style = Office.MsoButtonStyle.msoButtonCaption;
            myControl.Caption = candidate;
            myControl.Tag = TAG;

            myControl.Click +=
                new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler
                    (myControl_Click);

            //customTemplate.Saved = true;

            GC.Collect();
        }
        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion

    }
}
