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
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl1 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl2 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl3 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl4 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl5 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl6 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl7 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl8 = this.Factory.CreateRibbonDropDownItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ribbon));
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.box1 = this.Factory.CreateRibbonBox();
            this.chkSuggest = this.Factory.CreateRibbonCheckBox();
            this.separator1 = this.Factory.CreateRibbonSeparator();
            this.dropTypeFindError = this.Factory.CreateRibbonDropDown();
            this.dropCorpus = this.Factory.CreateRibbonDropDown();
            this.separator2 = this.Factory.CreateRibbonSeparator();
            this.dropDockPosition = this.Factory.CreateRibbonDropDown();
            this.tbtnCheck = this.Factory.CreateRibbonToggleButton();
            this.btnDeleteFormat = this.Factory.CreateRibbonButton();
            this.btnShowLength = this.Factory.CreateRibbonButton();
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
            this.group1.Items.Add(this.dropCorpus);
            this.group1.Items.Add(this.separator2);
            this.group1.Items.Add(this.dropDockPosition);
            this.group1.Label = "Spell checking";
            this.group1.Name = "group1";
            // 
            // box1
            // 
            this.box1.BoxStyle = Microsoft.Office.Tools.Ribbon.RibbonBoxStyle.Vertical;
            this.box1.Items.Add(this.tbtnCheck);
            this.box1.Items.Add(this.chkSuggest);
            this.box1.Items.Add(this.btnDeleteFormat);
            this.box1.Items.Add(this.btnShowLength);
            this.box1.Name = "box1";
            // 
            // chkSuggest
            // 
            this.chkSuggest.Checked = true;
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
            this.dropTypeFindError.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dropTypeFindError_SelectionChanged);
            // 
            // dropCorpus
            // 
            ribbonDropDownItemImpl3.Label = "Đời sống";
            ribbonDropDownItemImpl4.Label = "Chính trị";
            ribbonDropDownItemImpl5.Label = "Văn học";
            this.dropCorpus.Items.Add(ribbonDropDownItemImpl3);
            this.dropCorpus.Items.Add(ribbonDropDownItemImpl4);
            this.dropCorpus.Items.Add(ribbonDropDownItemImpl5);
            this.dropCorpus.Label = "Loại ngữ liệu";
            this.dropCorpus.Name = "dropCorpus";
            this.dropCorpus.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dropCorpus_SelectionChanged);
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            // 
            // dropDockPosition
            // 
            ribbonDropDownItemImpl6.Label = "Bên phải";
            ribbonDropDownItemImpl7.Label = "Bên trái";
            ribbonDropDownItemImpl8.Label = "Không neo";
            this.dropDockPosition.Items.Add(ribbonDropDownItemImpl6);
            this.dropDockPosition.Items.Add(ribbonDropDownItemImpl7);
            this.dropDockPosition.Items.Add(ribbonDropDownItemImpl8);
            this.dropDockPosition.Label = "Vị trí neo";
            this.dropDockPosition.Name = "dropDockPosition";
            this.dropDockPosition.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dropDockPosition_SelectionChanged);
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
            // btnDeleteFormat
            // 
            this.btnDeleteFormat.Label = "Xóa đánh dấu lỗi";
            this.btnDeleteFormat.Name = "btnDeleteFormat";
            this.btnDeleteFormat.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnDeleteFormat_Click);
            // 
            // btnShowLength
            // 
            this.btnShowLength.Label = "Xem độ dài câu";
            this.btnShowLength.Name = "btnShowLength";
            this.btnShowLength.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnShowLength_Click);
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
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator2;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDockPosition;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropCorpus;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDeleteFormat;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnShowLength;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon Ribbon1
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
