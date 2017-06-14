using Spell.Algorithm;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
namespace Spell
{
    public partial class ThisAddIn
    {
        Word.Application application;
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            VNDictionary.getInstance.runFirst();
            Ngram.Instance.runFirst();
            application = this.Application;
            application.WindowBeforeRightClick +=
                new Word.ApplicationEvents4_WindowBeforeRightClickEventHandler(application_WindowBeforeRightClick);

        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }
        public void application_WindowBeforeRightClick(Word.Selection selection, ref bool Cancel)
        {
            Word.Application applicationObject =
       Globals.ThisAddIn.Application as Word.Application;
            Office.CommandBarButton commandBarButton =
        applicationObject.CommandBars.FindControl
        (Office.MsoControlType.msoControlButton, missing, "HELLO_TAG", missing)
        as Office.CommandBarButton;
            if (commandBarButton != null)
            {
                return;
            }
            Office.CommandBar popupCommandBar = applicationObject.CommandBars["Text"];
            bool isFound = false;
            foreach (object _object in popupCommandBar.Controls)
            {
                Office.CommandBarButton _commandBarButton = _object as Office.CommandBarButton;
                if (_commandBarButton == null) continue;
                if (_commandBarButton.Tag.Equals("HELLO_TAG"))
                {
                    isFound = true;
                }
            }
            if (!isFound)
            {
                commandBarButton = (Office.CommandBarButton)popupCommandBar.Controls.Add
        (Office.MsoControlType.msoControlButton, missing, missing, missing, true);
                commandBarButton.Caption = "Hello !!!";
                commandBarButton.FaceId = 356;
                commandBarButton.Tag = "HELLO_TAG";
                commandBarButton.BeginGroup = true;
            }
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
