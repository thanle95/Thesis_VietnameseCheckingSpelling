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
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl9 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl10 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl11 = this.Factory.CreateRibbonDropDownItem();
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.box1 = this.Factory.CreateRibbonBox();
            this.separator1 = this.Factory.CreateRibbonSeparator();
            this.dropTypeFindError = this.Factory.CreateRibbonDropDown();
            this.dropTypeError = this.Factory.CreateRibbonDropDown();
            this.dropCorpus = this.Factory.CreateRibbonDropDown();
            this.chkbAutoChange = this.Factory.CreateRibbonCheckBox();
            this.separator2 = this.Factory.CreateRibbonSeparator();
            this.dropDockPosition = this.Factory.CreateRibbonDropDown();
            this.btnCheckError = this.Factory.CreateRibbonButton();
            this.btnDeleteFormat = this.Factory.CreateRibbonButton();
            this.btnPauseResume = this.Factory.CreateRibbonButton();
            this.tbtnShowTaskpane = this.Factory.CreateRibbonToggleButton();
            this.btnStop = this.Factory.CreateRibbonButton();
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
            this.group1.Items.Add(this.dropTypeError);
            this.group1.Items.Add(this.dropCorpus);
            this.group1.Items.Add(this.chkbAutoChange);
            this.group1.Items.Add(this.separator2);
            this.group1.Items.Add(this.dropDockPosition);
            this.group1.Label = "Spell checking";
            this.group1.Name = "group1";
            // 
            // box1
            // 
            this.box1.BoxStyle = Microsoft.Office.Tools.Ribbon.RibbonBoxStyle.Vertical;
            this.box1.Items.Add(this.btnCheckError);
            this.box1.Items.Add(this.btnDeleteFormat);
            this.box1.Items.Add(this.btnPauseResume);
            this.box1.Items.Add(this.tbtnShowTaskpane);
            this.box1.Items.Add(this.btnStop);
            this.box1.Name = "box1";
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            // 
            // dropTypeFindError
            // 
            ribbonDropDownItemImpl1.Label = "Toàn bộ văn bản";
            ribbonDropDownItemImpl2.Label = "Từng câu";
            this.dropTypeFindError.Items.Add(ribbonDropDownItemImpl1);
            this.dropTypeFindError.Items.Add(ribbonDropDownItemImpl2);
            this.dropTypeFindError.Label = "Kiểu kiểm lỗi";
            this.dropTypeFindError.Name = "dropTypeFindError";
            this.dropTypeFindError.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dropTypeFindError_SelectionChanged);
            // 
            // dropTypeError
            // 
            ribbonDropDownItemImpl3.Label = "Chính tả và ngữ cảnh";
            ribbonDropDownItemImpl4.Label = "Chỉ chính tả";
            ribbonDropDownItemImpl5.Label = "Chỉ ngữ cảnh";
            this.dropTypeError.Items.Add(ribbonDropDownItemImpl3);
            this.dropTypeError.Items.Add(ribbonDropDownItemImpl4);
            this.dropTypeError.Items.Add(ribbonDropDownItemImpl5);
            this.dropTypeError.Label = "Loại lỗi";
            this.dropTypeError.Name = "dropTypeError";
            // 
            // dropCorpus
            // 
            ribbonDropDownItemImpl6.Label = "Đời sống";
            ribbonDropDownItemImpl7.Label = "Chính trị";
            ribbonDropDownItemImpl8.Label = "Văn học";
            this.dropCorpus.Items.Add(ribbonDropDownItemImpl6);
            this.dropCorpus.Items.Add(ribbonDropDownItemImpl7);
            this.dropCorpus.Items.Add(ribbonDropDownItemImpl8);
            this.dropCorpus.Label = "Loại ngữ liệu";
            this.dropCorpus.Name = "dropCorpus";
            this.dropCorpus.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dropCorpus_SelectionChanged);
            // 
            // chkbAutoChange
            // 
            this.chkbAutoChange.Checked = true;
            this.chkbAutoChange.Label = "Tránh sai do từ lân cận";
            this.chkbAutoChange.Name = "chkbAutoChange";
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            // 
            // dropDockPosition
            // 
            ribbonDropDownItemImpl9.Label = "Bên phải";
            ribbonDropDownItemImpl10.Label = "Bên trái";
            ribbonDropDownItemImpl11.Label = "Không neo";
            this.dropDockPosition.Items.Add(ribbonDropDownItemImpl9);
            this.dropDockPosition.Items.Add(ribbonDropDownItemImpl10);
            this.dropDockPosition.Items.Add(ribbonDropDownItemImpl11);
            this.dropDockPosition.Label = "Vị trí neo";
            this.dropDockPosition.Name = "dropDockPosition";
            this.dropDockPosition.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dropDockPosition_SelectionChanged);
            // 
            // btnCheckError
            // 
            this.btnCheckError.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnCheckError.Image = global::Spell.Properties.Resources.check;
            this.btnCheckError.Label = "Kiểm lỗi";
            this.btnCheckError.Name = "btnCheckError";
            this.btnCheckError.ShowImage = true;
            this.btnCheckError.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnCheckError_Click);
            // 
            // btnDeleteFormat
            // 
            this.btnDeleteFormat.Label = "Xóa đánh dấu lỗi";
            this.btnDeleteFormat.Name = "btnDeleteFormat";
            this.btnDeleteFormat.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnDeleteFormat_Click);
            // 
            // btnPauseResume
            // 
            this.btnPauseResume.Enabled = false;
            this.btnPauseResume.Label = "Tạm dừng";
            this.btnPauseResume.Name = "btnPauseResume";
            this.btnPauseResume.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnPauseResume_Click);
            // 
            // tbtnShowTaskpane
            // 
            this.tbtnShowTaskpane.Enabled = false;
            this.tbtnShowTaskpane.Label = "Xem gợi ý";
            this.tbtnShowTaskpane.Name = "tbtnShowTaskpane";
            this.tbtnShowTaskpane.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.tbtnShowTaskpane_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Label = "Kết thúc";
            this.btnStop.Name = "btnStop";
            this.btnStop.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnStop_Click);
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
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator1;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropTypeFindError;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator2;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDockPosition;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropCorpus;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDeleteFormat;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropTypeError;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox chkbAutoChange;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton tbtnShowTaskpane;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnPauseResume;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCheckError;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnStop;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon Ribbon1
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
