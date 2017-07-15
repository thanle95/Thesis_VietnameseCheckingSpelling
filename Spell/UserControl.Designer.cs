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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridLog = new System.Windows.Forms.DataGridView();
            this.STT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wrongContext = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rightContext = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblWrongContext = new System.Windows.Forms.Label();
            this.lblRightContext = new System.Windows.Forms.Label();
            this.lblRightArrow = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnPauseResumeAutoFix = new System.Windows.Forms.Button();
            this.lblPauseResumeAutoFix = new System.Windows.Forms.Label();
            this.pnlAutoFix = new System.Windows.Forms.Panel();
            this.pnlButtonAutoFix = new System.Windows.Forms.Panel();
            this.pnlShowMore = new System.Windows.Forms.Panel();
            this.btnCloseBtnShowMore = new System.Windows.Forms.Button();
            this.btnChange = new System.Windows.Forms.Button();
            this.lblWrong = new System.Windows.Forms.Label();
            this.btnIgnore = new System.Windows.Forms.Button();
            this.btnResume = new System.Windows.Forms.Button();
            this.pnlSequenceFix = new System.Windows.Forms.Panel();
            this.pnlTxtManualFix = new System.Windows.Forms.Panel();
            this.txtManualFix = new System.Windows.Forms.TextBox();
            this.lstbCandidate = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridLog)).BeginInit();
            this.pnlAutoFix.SuspendLayout();
            this.pnlButtonAutoFix.SuspendLayout();
            this.pnlShowMore.SuspendLayout();
            this.pnlSequenceFix.SuspendLayout();
            this.pnlTxtManualFix.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridLog
            // 
            this.gridLog.AllowUserToAddRows = false;
            this.gridLog.AllowUserToDeleteRows = false;
            this.gridLog.AllowUserToResizeRows = false;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.Aqua;
            this.gridLog.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            this.gridLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.gridLog.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridLog.BackgroundColor = System.Drawing.Color.SkyBlue;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.gridLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.STT,
            this.wrongContext,
            this.rightContext});
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridLog.DefaultCellStyle = dataGridViewCellStyle15;
            this.gridLog.Location = new System.Drawing.Point(0, 170);
            this.gridLog.MultiSelect = false;
            this.gridLog.Name = "gridLog";
            this.gridLog.ReadOnly = true;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridLog.RowHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.gridLog.RowHeadersVisible = false;
            this.gridLog.Size = new System.Drawing.Size(282, 23);
            this.gridLog.TabIndex = 20;
            this.gridLog.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridLog_CellClick);
            this.gridLog.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridLog_CellContentClick);
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
            this.lblWrongContext.Size = new System.Drawing.Size(98, 16);
            this.lblWrongContext.TabIndex = 21;
            this.lblWrongContext.Text = "Ngữ cảnh sai";
            // 
            // lblRightContext
            // 
            this.lblRightContext.AutoSize = true;
            this.lblRightContext.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRightContext.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblRightContext.Location = new System.Drawing.Point(30, 30);
            this.lblRightContext.Name = "lblRightContext";
            this.lblRightContext.Size = new System.Drawing.Size(111, 16);
            this.lblRightContext.TabIndex = 22;
            this.lblRightContext.Text = "Ngữ cảnh đúng";
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
            this.btnGo.BackColor = System.Drawing.Color.White;
            this.btnGo.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.btnGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.ForeColor = System.Drawing.Color.Black;
            this.btnGo.Location = new System.Drawing.Point(189, 23);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(91, 30);
            this.btnGo.TabIndex = 24;
            this.btnGo.Text = "Đi đến lỗi...";
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnPauseResumeAutoFix
            // 
            this.btnPauseResumeAutoFix.Anchor = System.Windows.Forms.AnchorStyles.None;
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
            this.lblPauseResumeAutoFix.Anchor = System.Windows.Forms.AnchorStyles.None;
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
            this.pnlShowMore.BackColor = System.Drawing.SystemColors.Menu;
            this.pnlShowMore.Controls.Add(this.btnCloseBtnShowMore);
            this.pnlShowMore.Controls.Add(this.btnGo);
            this.pnlShowMore.Controls.Add(this.lblWrongContext);
            this.pnlShowMore.Controls.Add(this.lblRightArrow);
            this.pnlShowMore.Controls.Add(this.lblRightContext);
            this.pnlShowMore.Location = new System.Drawing.Point(0, 90);
            this.pnlShowMore.Name = "pnlShowMore";
            this.pnlShowMore.Size = new System.Drawing.Size(285, 55);
            this.pnlShowMore.TabIndex = 25;
            // 
            // btnCloseBtnShowMore
            // 
            this.btnCloseBtnShowMore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseBtnShowMore.BackColor = System.Drawing.Color.Red;
            this.btnCloseBtnShowMore.FlatAppearance.BorderSize = 0;
            this.btnCloseBtnShowMore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseBtnShowMore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseBtnShowMore.ForeColor = System.Drawing.Color.White;
            this.btnCloseBtnShowMore.Location = new System.Drawing.Point(260, 1);
            this.btnCloseBtnShowMore.Margin = new System.Windows.Forms.Padding(0);
            this.btnCloseBtnShowMore.Name = "btnCloseBtnShowMore";
            this.btnCloseBtnShowMore.Size = new System.Drawing.Size(20, 20);
            this.btnCloseBtnShowMore.TabIndex = 25;
            this.btnCloseBtnShowMore.Text = "X";
            this.btnCloseBtnShowMore.UseVisualStyleBackColor = false;
            this.btnCloseBtnShowMore.Click += new System.EventHandler(this.btnCloseBtnShowMore_Click);
            // 
            // btnChange
            // 
            this.btnChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChange.BackColor = System.Drawing.SystemColors.Menu;
            this.btnChange.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.btnChange.FlatAppearance.BorderSize = 0;
            this.btnChange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChange.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnChange.Location = new System.Drawing.Point(189, -2);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(90, 35);
            this.btnChange.TabIndex = 19;
            this.btnChange.Text = "Sửa";
            this.btnChange.UseVisualStyleBackColor = false;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            this.btnChange.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnChange_KeyDown);
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
            // btnIgnore
            // 
            this.btnIgnore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIgnore.BackColor = System.Drawing.Color.White;
            this.btnIgnore.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.btnIgnore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIgnore.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIgnore.ForeColor = System.Drawing.Color.Black;
            this.btnIgnore.Location = new System.Drawing.Point(189, 2);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(90, 30);
            this.btnIgnore.TabIndex = 0;
            this.btnIgnore.Text = "Bỏ qua";
            this.btnIgnore.UseVisualStyleBackColor = false;
            this.btnIgnore.Click += new System.EventHandler(this.btnIgnore_Click);
            // 
            // btnResume
            // 
            this.btnResume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResume.BackColor = System.Drawing.Color.White;
            this.btnResume.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.btnResume.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResume.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResume.ForeColor = System.Drawing.Color.Black;
            this.btnResume.Location = new System.Drawing.Point(93, 2);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(90, 30);
            this.btnResume.TabIndex = 20;
            this.btnResume.Text = "Trở lại";
            this.btnResume.UseVisualStyleBackColor = false;
            this.btnResume.Visible = false;
            this.btnResume.Click += new System.EventHandler(this.btnResume_Click);
            // 
            // pnlSequenceFix
            // 
            this.pnlSequenceFix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSequenceFix.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSequenceFix.Controls.Add(this.pnlTxtManualFix);
            this.pnlSequenceFix.Controls.Add(this.btnResume);
            this.pnlSequenceFix.Controls.Add(this.lstbCandidate);
            this.pnlSequenceFix.Controls.Add(this.btnIgnore);
            this.pnlSequenceFix.Controls.Add(this.lblWrong);
            this.pnlSequenceFix.Location = new System.Drawing.Point(14, 7);
            this.pnlSequenceFix.Name = "pnlSequenceFix";
            this.pnlSequenceFix.Size = new System.Drawing.Size(285, 176);
            this.pnlSequenceFix.TabIndex = 25;
            // 
            // pnlTxtManualFix
            // 
            this.pnlTxtManualFix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTxtManualFix.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.pnlTxtManualFix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTxtManualFix.Controls.Add(this.txtManualFix);
            this.pnlTxtManualFix.Controls.Add(this.btnChange);
            this.pnlTxtManualFix.Location = new System.Drawing.Point(1, 141);
            this.pnlTxtManualFix.Name = "pnlTxtManualFix";
            this.pnlTxtManualFix.Size = new System.Drawing.Size(281, 35);
            this.pnlTxtManualFix.TabIndex = 22;
            // 
            // txtManualFix
            // 
            this.txtManualFix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtManualFix.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtManualFix.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtManualFix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtManualFix.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtManualFix.Location = new System.Drawing.Point(13, 8);
            this.txtManualFix.Name = "txtManualFix";
            this.txtManualFix.Size = new System.Drawing.Size(175, 15);
            this.txtManualFix.TabIndex = 21;
            this.txtManualFix.Click += new System.EventHandler(this.txtManualFix_Click);
            this.txtManualFix.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtManualFix_KeyDown);
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
            this.lstbCandidate.Location = new System.Drawing.Point(1, 38);
            this.lstbCandidate.Name = "lstbCandidate";
            this.lstbCandidate.Size = new System.Drawing.Size(280, 100);
            this.lstbCandidate.TabIndex = 4;
            this.lstbCandidate.SelectedIndexChanged += new System.EventHandler(this.lstbCandidate_SelectedIndexChanged);
            this.lstbCandidate.SelectedValueChanged += new System.EventHandler(this.lstbCandidate_SelectedValueChanged);
            this.lstbCandidate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstbCandidate_KeyDown);
            this.lstbCandidate.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstbCandidate_MouseDoubleClick);
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
            this.pnlAutoFix.ResumeLayout(false);
            this.pnlButtonAutoFix.ResumeLayout(false);
            this.pnlButtonAutoFix.PerformLayout();
            this.pnlShowMore.ResumeLayout(false);
            this.pnlShowMore.PerformLayout();
            this.pnlSequenceFix.ResumeLayout(false);
            this.pnlSequenceFix.PerformLayout();
            this.pnlTxtManualFix.ResumeLayout(false);
            this.pnlTxtManualFix.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView gridLog;
        private System.Windows.Forms.Label lblWrongContext;
        private System.Windows.Forms.Label lblRightContext;
        private System.Windows.Forms.Label lblRightArrow;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnPauseResumeAutoFix;
        private System.Windows.Forms.Label lblPauseResumeAutoFix;
        private System.Windows.Forms.Panel pnlAutoFix;
        private System.Windows.Forms.Panel pnlShowMore;
        private System.Windows.Forms.Panel pnlButtonAutoFix;
        private System.Windows.Forms.DataGridViewTextBoxColumn STT;
        private System.Windows.Forms.DataGridViewTextBoxColumn wrongContext;
        private System.Windows.Forms.DataGridViewTextBoxColumn rightContext;
        private System.Windows.Forms.Button btnCloseBtnShowMore;
        private System.Windows.Forms.Button btnChange;
        public System.Windows.Forms.Label lblWrong;
        public System.Windows.Forms.Button btnIgnore;
        public System.Windows.Forms.Button btnResume;
        private System.Windows.Forms.Panel pnlSequenceFix;
        private System.Windows.Forms.TextBox txtManualFix;
        public System.Windows.Forms.ListBox lstbCandidate;
        public System.Windows.Forms.Panel pnlTxtManualFix;
    }
}

