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
            this.btnCheckError = this.Factory.CreateRibbonButton();
            this.btnDeleteFormat = this.Factory.CreateRibbonButton();
            this.btnDeleteCheckedRange = this.Factory.CreateRibbonButton();
            this.separator1 = this.Factory.CreateRibbonSeparator();
            this.dropTypeFindError = this.Factory.CreateRibbonDropDown();
            this.dropTypeError = this.Factory.CreateRibbonDropDown();
            this.dropCorpus = this.Factory.CreateRibbonDropDown();
            this.showSumError = this.Factory.CreateRibbonButton();
            this.lblSumError = this.Factory.CreateRibbonLabel();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.btnShowTaskpane = this.Factory.CreateRibbonButton();
            this.btnRestore = this.Factory.CreateRibbonButton();
            this.dropDockPosition = this.Factory.CreateRibbonDropDown();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.box1.SuspendLayout();
            this.group2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.group2);
            this.tab1.Label = "Kiểm lỗi chính tả tiếng Việt";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.box1);
            this.group1.Items.Add(this.separator1);
            this.group1.Items.Add(this.dropTypeFindError);
            this.group1.Items.Add(this.dropTypeError);
            this.group1.Items.Add(this.dropCorpus);
            this.group1.Items.Add(this.showSumError);
            this.group1.Items.Add(this.lblSumError);
            this.group1.Label = "Kiểm tra lỗi";
            this.group1.Name = "group1";
            // 
            // box1
            // 
            this.box1.BoxStyle = Microsoft.Office.Tools.Ribbon.RibbonBoxStyle.Vertical;
            this.box1.Items.Add(this.btnCheckError);
            this.box1.Items.Add(this.btnDeleteFormat);
            this.box1.Items.Add(this.btnDeleteCheckedRange);
            this.box1.Name = "box1";
            // 
            // btnCheckError
            // 
            this.btnCheckError.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnCheckError.Image = global::Spell.Properties.Resources.check;
            this.btnCheckError.Label = "Kiểm lỗi";
            this.btnCheckError.Name = "btnCheckError";
            this.btnCheckError.ScreenTip = "Kiểm lỗi";
            this.btnCheckError.ShowImage = true;
            this.btnCheckError.SuperTip = "Bôi đen vùng văn bản trước khi nhấn nút để kiểm tra vùng văn bản đó\n\nHoặc để con " +
    "trỏ tại bất cứ đâu trong văn bản, hệ thống sẽ kiểm lỗi từ đó trở về sau";
            this.btnCheckError.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnCheckError_Click);
            // 
            // btnDeleteFormat
            // 
            this.btnDeleteFormat.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnDeleteFormat.Enabled = false;
            this.btnDeleteFormat.Image = global::Spell.Properties.Resources.delete;
            this.btnDeleteFormat.Label = "Xóa đánh dấu lỗi";
            this.btnDeleteFormat.Name = "btnDeleteFormat";
            this.btnDeleteFormat.ScreenTip = "Xóa đánh dấu lỗi";
            this.btnDeleteFormat.ShowImage = true;
            this.btnDeleteFormat.SuperTip = "Bôi đen một vùng để xóa đánh dấu lỗi vùng đó\n\nHoặc không bôi đen để xóa đánh dấu " +
    "lỗi toàn văn bản";
            this.btnDeleteFormat.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnDeleteFormat_Click);
            // 
            // btnDeleteCheckedRange
            // 
            this.btnDeleteCheckedRange.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnDeleteCheckedRange.Enabled = false;
            this.btnDeleteCheckedRange.Image = global::Spell.Properties.Resources.delete_checked_range;
            this.btnDeleteCheckedRange.Label = "Xóa đánh dấu vùng kiểm lỗi";
            this.btnDeleteCheckedRange.Name = "btnDeleteCheckedRange";
            this.btnDeleteCheckedRange.ScreenTip = "Xóa đánh dấu vùng kiểm lỗi";
            this.btnDeleteCheckedRange.ShowImage = true;
            this.btnDeleteCheckedRange.SuperTip = "Bôi đen một vùng để xóa đánh dấu vùng đó\n\nHoặc không bôi đen để xóa đánh dấu vùng" +
    " đã kiểm lỗi toàn văn bản";
            this.btnDeleteCheckedRange.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnDeleteCheckedRange_Click);
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            // 
            // dropTypeFindError
            // 
            ribbonDropDownItemImpl1.Label = "Khi đánh máy";
            ribbonDropDownItemImpl2.Label = "Toàn bộ văn bản";
            this.dropTypeFindError.Items.Add(ribbonDropDownItemImpl1);
            this.dropTypeFindError.Items.Add(ribbonDropDownItemImpl2);
            this.dropTypeFindError.Label = "Kiểu kiểm lỗi";
            this.dropTypeFindError.Name = "dropTypeFindError";
            this.dropTypeFindError.ScreenTip = "Kiểu kiểm lỗi";
            this.dropTypeFindError.SuperTip = "Chọn kiểu kiểm lỗi của  bạn";
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
            this.dropTypeError.ScreenTip = "Loại lỗi";
            this.dropTypeError.SuperTip = "Chọn loại lỗi bạn muốn kiểm tra";
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
            this.dropCorpus.ScreenTip = "Loại ngữ liệu";
            this.dropCorpus.SuperTip = "Chọn loại ngữ liệu cho văn bản của bạn";
            this.dropCorpus.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dropCorpus_SelectionChanged);
            // 
            // showSumError
            // 
            this.showSumError.Label = "Xem tổng lỗi";
            this.showSumError.Name = "showSumError";
            this.showSumError.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.showSumError_Click);
            // 
            // lblSumError
            // 
            this.lblSumError.Label = " ";
            this.lblSumError.Name = "lblSumError";
            // 
            // group2
            // 
            this.group2.Items.Add(this.btnShowTaskpane);
            this.group2.Items.Add(this.btnRestore);
            this.group2.Items.Add(this.dropDockPosition);
            this.group2.Label = "Sửa lỗi";
            this.group2.Name = "group2";
            // 
            // btnShowTaskpane
            // 
            this.btnShowTaskpane.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnShowTaskpane.Enabled = false;
            this.btnShowTaskpane.Image = global::Spell.Properties.Resources.change_all;
            this.btnShowTaskpane.Label = "Sửa tất cả";
            this.btnShowTaskpane.Name = "btnShowTaskpane";
            this.btnShowTaskpane.ScreenTip = "Hiện Task Pane để sửa tất cả lỗi có trong văn bản bằng gợi ý tốt nhất được chọn";
            this.btnShowTaskpane.ShowImage = true;
            this.btnShowTaskpane.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnShowTaskPane_Click_1);
            // 
            // btnRestore
            // 
            this.btnRestore.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnRestore.Enabled = false;
            this.btnRestore.Image = global::Spell.Properties.Resources.undo;
            this.btnRestore.Label = "Phục hồi văn bản";
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.ScreenTip = "Phục hồi lại văn bản trước lần sửa lỗi gần nhất";
            this.btnRestore.ShowImage = true;
            this.btnRestore.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnRestore_Click);
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
            this.dropDockPosition.ScreenTip = "Vị trí neo";
            this.dropDockPosition.SuperTip = "Chọn vị trí bạn muốn neo taskpane";
            this.dropDockPosition.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dropDockPosition_SelectionChanged);
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
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonBox box1;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator1;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropTypeFindError;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDockPosition;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropCorpus;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDeleteFormat;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropTypeError;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCheckError;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton showSumError;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblSumError;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnShowTaskpane;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnRestore;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDeleteCheckedRange;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon Ribbon1
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
