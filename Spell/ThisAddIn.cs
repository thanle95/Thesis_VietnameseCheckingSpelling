﻿using Spell.Algorithm;
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
        private void AddMenuItem()
        {
            //myApplication.CustomizationContext = customTemplate;
            Office.MsoControlType menuItem =
                Office.MsoControlType.msoControlButton;

            myControl =
                (Office.CommandBarButton)myApplication.CommandBars["Text"].Controls.Add
                (menuItem, missing, missing, 1, true);

            myControl.Style = Office.MsoButtonStyle.msoButtonCaption;
            myControl.Caption = "My Menu Item";
            myControl.Tag = "MyMenuItem";

            myControl.Click +=
                new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler
                    (myControl_Click);

            //customTemplate.Saved = true;

            GC.Collect();

        }
        void myControl_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            System.Windows.Forms.MessageBox.Show("My Menu Item clicked");
        }
        public void application_WindowBeforeRightClick(Word.Selection selection, ref bool Cancel)
        {
            //     Word.Application applicationObject =
            //Globals.ThisAddIn.Application as Word.Application;
            //     Office.CommandBarButton commandBarButton =
            // applicationObject.CommandBars.FindControl
            // (Office.MsoControlType.msoControlButton, missing, "HELLO_TAG", missing)
            // as Office.CommandBarButton;
            //     if (commandBarButton != null)
            //     {
            //         return;
            //     }
            //     Office.CommandBar popupCommandBar = applicationObject.CommandBars["Text"];
            //     bool isFound = false;
            //     foreach (object _object in popupCommandBar.Controls)
            //     {
            //         Office.CommandBarButton _commandBarButton = _object as Office.CommandBarButton;
            //         if (_commandBarButton == null) continue;
            //         if (_commandBarButton.Tag.Equals("HELLO_TAG"))
            //         {
            //             isFound = true;
            //         }
            //     }
            //     if (!isFound)
            //     {
            //         commandBarButton = (Office.CommandBarButton)popupCommandBar.Controls.Add
            // (Office.MsoControlType.msoControlButton, missing, missing, missing, true);
            //         commandBarButton.Caption = "Hello !!!";
            //         commandBarButton.FaceId = 356;
            //         commandBarButton.Tag = "HELLO_TAG";
            //         commandBarButton.BeginGroup = true;
            //     }

            //--------------------------------------

            RemoveExistingMenuItem();
            //for (int i = 0; i < 3; i++)
            //{
                Office.MsoControlType menuItem =
                        Office.MsoControlType.msoControlButton;

                myControl =
                    (Office.CommandBarButton)myApplication.CommandBars["Text"].Controls.Add
                    (menuItem, missing, missing, 1, true);

                myControl.Style = Office.MsoButtonStyle.msoButtonCaption;
                myControl.Caption = "My Menu Item" + count++;
                myControl.Tag = TAG;

                myControl.Click +=
                    new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler
                        (myControl_Click);

                //customTemplate.Saved = true;

                GC.Collect();
            //}
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
