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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnIgnore = new System.Windows.Forms.Button();
            this.lblWrong = new System.Windows.Forms.Label();
            this.lstbCandidate = new System.Windows.Forms.ListBox();
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
            this.btnResume = new System.Windows.Forms.Button();
            this.btnPauseResumeAutoFix = new System.Windows.Forms.Button();
            this.lblPauseResumeAutoFix = new System.Windows.Forms.Label();
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
            this.btnIgnore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIgnore.AutoSize = true;
            this.btnIgnore.BackColor = System.Drawing.Color.White;
            this.btnIgnore.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIgnore.ForeColor = System.Drawing.Color.Black;
            this.btnIgnore.Location = new System.Drawing.Point(195, 0);
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
            this.lstbCandidate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstbCandidate.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.lstbCandidate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstbCandidate.FormattingEnabled = true;
            this.lstbCandidate.ItemHeight = 16;
            this.lstbCandidate.Location = new System.Drawing.Point(0, 34);
            this.lstbCandidate.Name = "lstbCandidate";
            this.lstbCandidate.Size = new System.Drawing.Size(285, 100);
            this.lstbCandidate.TabIndex = 4;
            this.lstbCandidate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstbCandidate_KeyDown);
            // 
            // btnChange
            // 
            this.btnChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
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
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Aqua;
            this.gridLog.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gridLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.gridLog.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.gridLog.BackgroundColor = System.Drawing.Color.SkyBlue;
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
            this.STT.Width = 40;
            // 
            // wrongContext
            // 
            this.wrongContext.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.wrongContext.HeaderText = "Ngữ cảnh sai";
            this.wrongContext.Name = "wrongContext";
            this.wrongContext.ReadOnly = true;
            // 
            // rightContext
            // 
            this.rightContext.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
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
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
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
            this.pnlSequenceFix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSequenceFix.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSequenceFix.Controls.Add(this.btnResume);
            this.pnlSequenceFix.Controls.Add(this.lstbCandidate);
            this.pnlSequenceFix.Controls.Add(this.btnIgnore);
            this.pnlSequenceFix.Controls.Add(this.lblWrong);
            this.pnlSequenceFix.Controls.Add(this.btnChange);
            this.pnlSequenceFix.Location = new System.Drawing.Point(14, 7);
            this.pnlSequenceFix.Name = "pnlSequenceFix";
            this.pnlSequenceFix.Size = new System.Drawing.Size(285, 176);
            this.pnlSequenceFix.TabIndex = 25;
            // 
            // btnResume
            // 
            this.btnResume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResume.AutoSize = true;
            this.btnResume.BackColor = System.Drawing.Color.White;
            this.btnResume.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResume.ForeColor = System.Drawing.Color.Black;
            this.btnResume.Location = new System.Drawing.Point(93, 0);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(90, 30);
            this.btnResume.TabIndex = 20;
            this.btnResume.Text = "Trở lại";
            this.btnResume.UseVisualStyleBackColor = false;
            this.btnResume.Visible = false;
            this.btnResume.Click += new System.EventHandler(this.btnResume_Click);
            // 
            // btnPauseResumeAutoFix
            // 
            this.btnPauseResumeAutoFix.BackgroundImage = global::Spell.Properties.Resources.pause;
            this.btnPauseResumeAutoFix.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPauseResumeAutoFix.Location = new System.Drawing.Point(117, 3);
            this.btnPauseResumeAutoFix.Name = "btnPauseResumeAutoFix";
            this.btnPauseResumeAutoFix.Size = new System.Drawing.Size(50, 50);
            this.btnPauseResumeAutoFix.TabIndex = 26;
            this.btnPauseResumeAutoFix.UseVisualStyleBackColor = true;
            this.btnPauseResumeAutoFix.Click += new System.EventHandler(this.btnPauseResumeAutoFix_Click);
            // 
            // lblPauseResumeAutoFix
            // 
            this.lblPauseResumeAutoFix.AutoSize = true;
            this.lblPauseResumeAutoFix.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPauseResumeAutoFix.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblPauseResumeAutoFix.Location = new System.Drawing.Point(101, 60);
            this.lblPauseResumeAutoFix.Name = "lblPauseResumeAutoFix";
            this.lblPauseResumeAutoFix.Size = new System.Drawing.Size(82, 18);
            this.lblPauseResumeAutoFix.TabIndex = 28;
            this.lblPauseResumeAutoFix.Text = "Tạm dừng";
            this.lblPauseResumeAutoFix.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblPauseResumeAutoFix.Click += new System.EventHandler(this.lblPauseResumeAutoFix_Click);
            // 
            // pnlAutoFix
            // 
            this.pnlAutoFix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAutoFix.Controls.Add(this.pnlButtonAutoFix);
            this.pnlAutoFix.Controls.Add(this.pnlShowMore);
            this.pnlAutoFix.Controls.Add(this.gridLog);
            this.pnlAutoFix.Location = new System.Drawing.Point(14, 194);
            this.pnlAutoFix.Name = "pnlAutoFix";
            this.pnlAutoFix.Size = new System.Drawing.Size(285, 400);
            this.pnlAutoFix.TabIndex = 30;
            this.pnlAutoFix.Visible = false;
            // 
            // pnlButtonAutoFix
            // 
            this.pnlButtonAutoFix.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlButtonAutoFix.Controls.Add(this.btnPauseResumeAutoFix);
            this.pnlButtonAutoFix.Controls.Add(this.lblPauseResumeAutoFix);
            this.pnlButtonAutoFix.Location = new System.Drawing.Point(0, 0);
            this.pnlButtonAutoFix.Name = "pnlButtonAutoFix";
            this.pnlButtonAutoFix.Size = new System.Drawing.Size(285, 90);
            this.pnlButtonAutoFix.TabIndex = 30;
            // 
            // pnlShowMore
            // 
            this.pnlShowMore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
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
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.pnlAutoFix);
            this.Controls.Add(this.pnlSequenceFix);
            this.MinimumSize = new System.Drawing.Size(320, 600);
            this.Name = "UserControl";
            this.Size = new System.Drawing.Size(320, 600);
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

        public System.Windows.Forms.Button btnIgnore;
        public System.Windows.Forms.Label lblWrong;
        public System.Windows.Forms.ListBox lstbCandidate;
        public System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.DataGridView gridLog;
        private System.Windows.Forms.Label lblWrongContext;
        private System.Windows.Forms.Label lblRightContext;
        private System.Windows.Forms.Label lblRightArrow;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Panel pnlSequenceFix;
        private System.Windows.Forms.Button btnPauseResumeAutoFix;
        private System.Windows.Forms.Label lblPauseResumeAutoFix;
        private System.Windows.Forms.Panel pnlAutoFix;
        private System.Windows.Forms.Panel pnlShowMore;
        private System.Windows.Forms.Panel pnlButtonAutoFix;
        public System.Windows.Forms.Button btnResume;
        private System.Windows.Forms.DataGridViewTextBoxColumn STT;
        private System.Windows.Forms.DataGridViewTextBoxColumn wrongContext;
        private System.Windows.Forms.DataGridViewTextBoxColumn rightContext;
    }
}

