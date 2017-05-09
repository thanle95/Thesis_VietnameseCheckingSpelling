namespace Spell
{
    partial class Ribbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ribbon));
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl1 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl2 = this.Factory.CreateRibbonDropDownItem();
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.box1 = this.Factory.CreateRibbonBox();
            this.tbtnCheck = this.Factory.CreateRibbonToggleButton();
            this.chkSuggest = this.Factory.CreateRibbonCheckBox();
            this.separator1 = this.Factory.CreateRibbonSeparator();
            this.dropTypeFindError = this.Factory.CreateRibbonDropDown();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.box1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.box1);
            this.group1.Items.Add(this.separator1);
            this.group1.Items.Add(this.dropTypeFindError);
            this.group1.Label = "Spell checking";
            this.group1.Name = "group1";
            // 
            // box1
            // 
            this.box1.BoxStyle = Microsoft.Office.Tools.Ribbon.RibbonBoxStyle.Vertical;
            this.box1.Items.Add(this.tbtnCheck);
            this.box1.Items.Add(this.chkSuggest);
            this.box1.Name = "box1";
            // 
            // tbtnCheck
            // 
            this.tbtnCheck.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.tbtnCheck.Image = ((System.Drawing.Image)(resources.GetObject("tbtnCheck.Image")));
            this.tbtnCheck.Label = "Check spelling";
            this.tbtnCheck.Name = "tbtnCheck";
            this.tbtnCheck.ShowImage = true;
            this.tbtnCheck.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.tbtnCheck_Click);
            // 
            // chkSuggest
            // 
            this.chkSuggest.Label = "Hiện gợi ý";
            this.chkSuggest.Name = "chkSuggest";
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            // 
            // dropTypeFindError
            // 
            ribbonDropDownItemImpl1.Label = "Toàn bộ văn bản";
            ribbonDropDownItemImpl2.Label = "Bôi đen đoạn văn bản";
            this.dropTypeFindError.Items.Add(ribbonDropDownItemImpl1);
            this.dropTypeFindError.Items.Add(ribbonDropDownItemImpl2);
            this.dropTypeFindError.Label = "Kiểu kiểm lỗi";
            this.dropTypeFindError.Name = "dropTypeFindError";
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.box1.ResumeLayout(false);
            this.box1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonBox box1;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox chkSuggest;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton tbtnCheck;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator1;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropTypeFindError;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon Ribbon1
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
