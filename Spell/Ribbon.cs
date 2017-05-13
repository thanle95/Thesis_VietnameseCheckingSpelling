using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using System.Threading;
using System.Windows.Forms;
using Spell.Algorithm;
namespace Spell
{
    public partial class Ribbon
    {
        Microsoft.Office.Tools.CustomTaskPane myCustomTaskPane;
        private int typeFindError = 0;
        private const int LIFE_CORPUS = 0;
        private const int POLITIC_CORPUS = 1;
        private const int LITERRATY_CORPUS = 2;

        private const int WHOLE_DOCUMENT_SELECTION = 0;
        private const int APART_DOCUMENT_SELECTION = 1;

        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            myCustomTaskPane = Globals.ThisAddIn.CustomTaskPanes.Add(UserControl.Instance, "Spelling");
            myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionFloating;
            myCustomTaskPane.Width = 300;
            myCustomTaskPane.Height = 350;
            myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionRight;
            myCustomTaskPane.Width = 300;
            typeFindError = dropTypeFindError.SelectedItemIndex;
            //Ngram.Instance.runFirst();
        }

        /// <summary>
        /// Khi nút check được click
        /// Kiểm tra nếu có tick vào check box gợi ý, thì hiện task pane gợi ý bên phải
        /// còn không thì highLight những lỗi trong văn bản
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbtnCheck_Click(object sender, RibbonControlEventArgs e)
        {
            
            //ThreadStart t = new ThreadStart(UserControl.Instance.showWrongWithoutSuggest);
            //Thread threadWithoutSuggest = new Thread(t);
            //ThreadStart t1 = new ThreadStart(UserControl.Instance.showWrongWithSuggest);
            //Thread threadWithSuggest = new Thread(t1);
            //nút check được checked
            if (tbtnCheck.Checked)
                //mode1: hiện gợi ý sửa lỗi
                if (chkSuggest.Checked)
                {
                    //dehightlight tất cả những lỗi trước đó
                    DocumentHandling.Instance.DeHighLight_All_Mistake(Globals.ThisAddIn.Application.ActiveDocument.Characters);
                    int startIndex = Globals.ThisAddIn.Application.Selection.Start;
                    int endIndex = Globals.ThisAddIn.Application.Selection.End;
                    Dictionary<int, Word.Range> ret = FindError.Instance.startFindError(typeFindError);
                    int count = ret.Count;
                    //int count = UserControl.Instance.startFindError(typeFindError);
                    if (count > 0)
                    {

                        string message = SysMessage.Instance.Message_Notify_Fix_Error(count);
                        string caption = SysMessage.Instance.Caption_Notify_Fix_Error;
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                        DialogResult result;

                        // Displays the MessageBox.

                        result = MessageBox.Show(message, caption, buttons);

                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            UserControl.Instance.showCandidateInTaskPaneWithCountWord();
                        }
                        myCustomTaskPane.Visible = true;
                    }
                    //    myCustomTaskPane.Visible = true;
                    if (count == 0)
                        MessageBox.Show(SysMessage.Instance.No_error);
                    //threadWithSuggest.Start();
                }
                //mode2: không hiện gợi ý 
                else {
                    myCustomTaskPane.Visible = false;
                    //threadWithSuggest.Abort();
                    //threadWithoutSuggest.Start();
                }
            else
            {
                //threadWithSuggest.Abort();
                //threadWithoutSuggest.Abort();
                DocumentHandling.Instance.DeHighLight_All_Mistake(Globals.ThisAddIn.Application.ActiveDocument.Characters);
            }
        }

        private void dropDockPosition_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            if (dropDockPosition.SelectedItemIndex == 0)
                myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionRight;
            else if (dropDockPosition.SelectedItemIndex == 1)
                myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionLeft;
            else {
                myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionFloating;
                Office.CommandBar cb = Globals.ThisAddIn.Application.CommandBars[myCustomTaskPane.Title];
                MessageBox.Show(cb.Left + " " + cb.Top);
                
                cb.Left = 700;
                cb.Top = 500;
                MessageBox.Show(cb.Left + " " + cb.Top);
            }
        }

        private void dropCorpus_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            if (dropCorpus.SelectedItemIndex == POLITIC_CORPUS)
            {
                MessageBox.Show(SysMessage.Instance.Feature_is_updating);
                dropCorpus.SelectedItemIndex = LIFE_CORPUS;
            }
            else if (dropCorpus.SelectedItemIndex == LITERRATY_CORPUS)
            {
                MessageBox.Show(SysMessage.Instance.Feature_is_updating);
                dropCorpus.SelectedItemIndex = LIFE_CORPUS;
            }
        }

        private void dropTypeFindError_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            if (dropTypeFindError.SelectedItemIndex == APART_DOCUMENT_SELECTION)
            {
                MessageBox.Show(SysMessage.Instance.Feature_is_updating);
                dropTypeFindError.SelectedItemIndex = WHOLE_DOCUMENT_SELECTION;
            }
        }
    }
}
