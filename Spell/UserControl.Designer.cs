namespace Spell
{

    partial class UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnIgnore = new System.Windows.Forms.Button();
            this.lblWrong = new System.Windows.Forms.Label();
            this.lstbCandidate = new System.Windows.Forms.ListBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnChange = new System.Windows.Forms.Button();
            this.gridLog = new System.Windows.Forms.DataGridView();
            this.STT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wrongContext = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rightContext = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblWrongContext = new System.Windows.Forms.Label();
            this.lblRightContext = new System.Windows.Forms.Label();
            this.lblRightArrow = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.pnlSequenceFix = new System.Windows.Forms.Panel();
            this.btnAutoFix = new System.Windows.Forms.Button();
            this.btnStopAutoFix = new System.Windows.Forms.Button();
            this.lblAutoFix = new System.Windows.Forms.Label();
            this.lblStopAutoFix = new System.Windows.Forms.Label();
            this.pnlAutoFix = new System.Windows.Forms.Panel();
            this.pnlButtonAutoFix = new System.Windows.Forms.Panel();
            this.pnlShowMore = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gridLog)).BeginInit();
            this.pnlSequenceFix.SuspendLayout();
            this.pnlAutoFix.SuspendLayout();
            this.pnlButtonAutoFix.SuspendLayout();
            this.pnlShowMore.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnIgnore
            // 
            this.btnIgnore.AutoSize = true;
            this.btnIgnore.BackColor = System.Drawing.Color.White;
            this.btnIgnore.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIgnore.ForeColor = System.Drawing.Color.Black;
            this.btnIgnore.Location = new System.Drawing.Point(196, -2);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(90, 30);
            this.btnIgnore.TabIndex = 0;
            this.btnIgnore.Text = "Bỏ qua";
            this.btnIgnore.UseVisualStyleBackColor = false;
            this.btnIgnore.Click += new System.EventHandler(this.btnIgnore_Click);
            // 
            // lblWrong
            // 
            this.lblWrong.AutoSize = true;
            this.lblWrong.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWrong.ForeColor = System.Drawing.Color.Black;
            this.lblWrong.Location = new System.Drawing.Point(-4, 1);
            this.lblWrong.Name = "lblWrong";
            this.lblWrong.Size = new System.Drawing.Size(71, 21);
            this.lblWrong.TabIndex = 3;
            this.lblWrong.Text = "\"Từ sai\"";
            // 
            // lstbCandidate
            // 
            this.lstbCandidate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstbCandidate.FormattingEnabled = true;
            this.lstbCandidate.ItemHeight = 16;
            this.lstbCandidate.Location = new System.Drawing.Point(0, 34);
            this.lstbCandidate.Name = "lstbCandidate";
            this.lstbCandidate.Size = new System.Drawing.Size(285, 100);
            this.lstbCandidate.TabIndex = 4;
            this.lstbCandidate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstbCandidate_KeyDown);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.White;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(0, 147);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(90, 30);
            this.btnStart.TabIndex = 18;
            this.btnStart.Text = "Xem gợi ý";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnChange
            // 
            this.btnChange.AutoSize = true;
            this.btnChange.BackColor = System.Drawing.Color.White;
            this.btnChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChange.ForeColor = System.Drawing.Color.Black;
            this.btnChange.Location = new System.Drawing.Point(196, 147);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(90, 30);
            this.btnChange.TabIndex = 19;
            this.btnChange.Text = "Sửa";
            this.btnChange.UseVisualStyleBackColor = false;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            this.btnChange.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnChange_KeyDown);
            // 
            // gridLog
            // 
            this.gridLog.AllowUserToAddRows = false;
            this.gridLog.AllowUserToDeleteRows = false;
            this.gridLog.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Aqua;
            this.gridLog.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.gridLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.STT,
            this.wrongContext,
            this.rightContext});
            this.gridLog.Location = new System.Drawing.Point(0, 170);
            this.gridLog.MultiSelect = false;
            this.gridLog.Name = "gridLog";
            this.gridLog.ReadOnly = true;
            this.gridLog.Size = new System.Drawing.Size(287, 90);
            this.gridLog.TabIndex = 20;
            this.gridLog.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridLog_CellDoubleClick);
            // 
            // STT
            // 
            this.STT.HeaderText = "STT";
            this.STT.MaxInputLength = 10000;
            this.STT.Name = "STT";
            this.STT.ReadOnly = true;
            this.STT.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.STT.Width = 50;
            // 
            // wrongContext
            // 
            this.wrongContext.HeaderText = "Ngữ cảnh sai";
            this.wrongContext.Name = "wrongContext";
            this.wrongContext.ReadOnly = true;
            // 
            // rightContext
            // 
            this.rightContext.HeaderText = "Ngữ cảnh đúng";
            this.rightContext.Name = "rightContext";
            this.rightContext.ReadOnly = true;
            // 
            // lblWrongContext
            // 
            this.lblWrongContext.AutoSize = true;
            this.lblWrongContext.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWrongContext.ForeColor = System.Drawing.Color.Red;
            this.lblWrongContext.Location = new System.Drawing.Point(0, 3);
            this.lblWrongContext.Name = "lblWrongContext";
            this.lblWrongContext.Size = new System.Drawing.Size(51, 16);
            this.lblWrongContext.TabIndex = 21;
            this.lblWrongContext.Text = "label1";
            // 
            // lblRightContext
            // 
            this.lblRightContext.AutoSize = true;
            this.lblRightContext.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRightContext.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblRightContext.Location = new System.Drawing.Point(40, 30);
            this.lblRightContext.Name = "lblRightContext";
            this.lblRightContext.Size = new System.Drawing.Size(51, 16);
            this.lblRightContext.TabIndex = 22;
            this.lblRightContext.Text = "label1";
            // 
            // lblRightArrow
            // 
            this.lblRightArrow.AutoSize = true;
            this.lblRightArrow.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRightArrow.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblRightArrow.Location = new System.Drawing.Point(4, 24);
            this.lblRightArrow.Name = "lblRightArrow";
            this.lblRightArrow.Size = new System.Drawing.Size(30, 24);
            this.lblRightArrow.TabIndex = 23;
            this.lblRightArrow.Text = "→";
            // 
            // btnGo
            // 
            this.btnGo.AutoSize = true;
            this.btnGo.BackColor = System.Drawing.Color.White;
            this.btnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.ForeColor = System.Drawing.Color.Black;
            this.btnGo.Location = new System.Drawing.Point(196, 23);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(90, 30);
            this.btnGo.TabIndex = 24;
            this.btnGo.Text = "Đi đến lỗi...";
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // pnlSequenceFix
            // 
            this.pnlSequenceFix.Controls.Add(this.lstbCandidate);
            this.pnlSequenceFix.Controls.Add(this.btnIgnore);
            this.pnlSequenceFix.Controls.Add(this.lblWrong);
            this.pnlSequenceFix.Controls.Add(this.btnStart);
            this.pnlSequenceFix.Controls.Add(this.btnChange);
            this.pnlSequenceFix.Location = new System.Drawing.Point(14, 7);
            this.pnlSequenceFix.Name = "pnlSequenceFix";
            this.pnlSequenceFix.Size = new System.Drawing.Size(285, 176);
            this.pnlSequenceFix.TabIndex = 25;
            // 
            // btnAutoFix
            // 
            this.btnAutoFix.BackgroundImage = global::Spell.Properties.Resources.fix;
            this.btnAutoFix.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAutoFix.Location = new System.Drawing.Point(30, 3);
            this.btnAutoFix.Name = "btnAutoFix";
            this.btnAutoFix.Size = new System.Drawing.Size(50, 50);
            this.btnAutoFix.TabIndex = 26;
            this.btnAutoFix.UseVisualStyleBackColor = true;
            // 
            // btnStopAutoFix
            // 
            this.btnStopAutoFix.BackgroundImage = global::Spell.Properties.Resources.stop;
            this.btnStopAutoFix.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStopAutoFix.Location = new System.Drawing.Point(181, 3);
            this.btnStopAutoFix.Name = "btnStopAutoFix";
            this.btnStopAutoFix.Size = new System.Drawing.Size(50, 50);
            this.btnStopAutoFix.TabIndex = 27;
            this.btnStopAutoFix.UseVisualStyleBackColor = true;
            // 
            // lblAutoFix
            // 
            this.lblAutoFix.AutoSize = true;
            this.lblAutoFix.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutoFix.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblAutoFix.Location = new System.Drawing.Point(-2, 66);
            this.lblAutoFix.Name = "lblAutoFix";
            this.lblAutoFix.Size = new System.Drawing.Size(107, 20);
            this.lblAutoFix.TabIndex = 28;
            this.lblAutoFix.Text = "Sửa tự động";
            // 
            // lblStopAutoFix
            // 
            this.lblStopAutoFix.AutoSize = true;
            this.lblStopAutoFix.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStopAutoFix.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblStopAutoFix.Location = new System.Drawing.Point(129, 66);
            this.lblStopAutoFix.Name = "lblStopAutoFix";
            this.lblStopAutoFix.Size = new System.Drawing.Size(152, 20);
            this.lblStopAutoFix.TabIndex = 29;
            this.lblStopAutoFix.Text = "Dừng sửa tự động";
            // 
            // pnlAutoFix
            // 
            this.pnlAutoFix.Controls.Add(this.pnlButtonAutoFix);
            this.pnlAutoFix.Controls.Add(this.pnlShowMore);
            this.pnlAutoFix.Controls.Add(this.gridLog);
            this.pnlAutoFix.Location = new System.Drawing.Point(14, 194);
            this.pnlAutoFix.Name = "pnlAutoFix";
            this.pnlAutoFix.Size = new System.Drawing.Size(285, 311);
            this.pnlAutoFix.TabIndex = 30;
            this.pnlAutoFix.Visible = false;
            // 
            // pnlButtonAutoFix
            // 
            this.pnlButtonAutoFix.Controls.Add(this.btnStopAutoFix);
            this.pnlButtonAutoFix.Controls.Add(this.btnAutoFix);
            this.pnlButtonAutoFix.Controls.Add(this.lblStopAutoFix);
            this.pnlButtonAutoFix.Controls.Add(this.lblAutoFix);
            this.pnlButtonAutoFix.Location = new System.Drawing.Point(2, 0);
            this.pnlButtonAutoFix.Name = "pnlButtonAutoFix";
            this.pnlButtonAutoFix.Size = new System.Drawing.Size(285, 90);
            this.pnlButtonAutoFix.TabIndex = 30;
            // 
            // pnlShowMore
            // 
            this.pnlShowMore.Controls.Add(this.btnGo);
            this.pnlShowMore.Controls.Add(this.lblWrongContext);
            this.pnlShowMore.Controls.Add(this.lblRightArrow);
            this.pnlShowMore.Controls.Add(this.lblRightContext);
            this.pnlShowMore.Location = new System.Drawing.Point(0, 90);
            this.pnlShowMore.Name = "pnlShowMore";
            this.pnlShowMore.Size = new System.Drawing.Size(285, 55);
            this.pnlShowMore.TabIndex = 25;
            // 
            // UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.pnlAutoFix);
            this.Controls.Add(this.pnlSequenceFix);
            this.MinimumSize = new System.Drawing.Size(300, 350);
            this.Name = "UserControl";
            this.Size = new System.Drawing.Size(320, 530);
            ((System.ComponentModel.ISupportInitialize)(this.gridLog)).EndInit();
            this.pnlSequenceFix.ResumeLayout(false);
            this.pnlSequenceFix.PerformLayout();
            this.pnlAutoFix.ResumeLayout(false);
            this.pnlButtonAutoFix.ResumeLayout(false);
            this.pnlButtonAutoFix.PerformLayout();
            this.pnlShowMore.ResumeLayout(false);
            this.pnlShowMore.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnIgnore;
        private System.Windows.Forms.Label lblWrong;
        private System.Windows.Forms.ListBox lstbCandidate;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.DataGridView gridLog;
        private System.Windows.Forms.Label lblWrongContext;
        private System.Windows.Forms.Label lblRightContext;
        private System.Windows.Forms.Label lblRightArrow;
        private System.Windows.Forms.DataGridViewTextBoxColumn STT;
        private System.Windows.Forms.DataGridViewTextBoxColumn wrongContext;
        private System.Windows.Forms.DataGridViewTextBoxColumn rightContext;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Panel pnlSequenceFix;
        private System.Windows.Forms.Button btnAutoFix;
        private System.Windows.Forms.Label lblAutoFix;
        private System.Windows.Forms.Label lblStopAutoFix;
        private System.Windows.Forms.Panel pnlAutoFix;
        private System.Windows.Forms.Button btnStopAutoFix;
        private System.Windows.Forms.Panel pnlShowMore;
        private System.Windows.Forms.Panel pnlButtonAutoFix;
    }
}

