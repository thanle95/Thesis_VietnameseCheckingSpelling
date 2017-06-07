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
            this.btnIgnore = new System.Windows.Forms.Button();
            this.lblWrong = new System.Windows.Forms.Label();
            this.lstbCandidate = new System.Windows.Forms.ListBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnChange = new System.Windows.Forms.Button();
            this.gridLog = new System.Windows.Forms.DataGridView();
            this.wrongContext = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rightContext = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridLog)).BeginInit();
            this.SuspendLayout();
            // 
            // btnIgnore
            // 
            this.btnIgnore.AutoSize = true;
            this.btnIgnore.BackColor = System.Drawing.Color.White;
            this.btnIgnore.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIgnore.ForeColor = System.Drawing.Color.Black;
            this.btnIgnore.Location = new System.Drawing.Point(181, 4);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(80, 30);
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
            this.lblWrong.Location = new System.Drawing.Point(14, 7);
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
            this.lstbCandidate.Location = new System.Drawing.Point(18, 40);
            this.lstbCandidate.Name = "lstbCandidate";
            this.lstbCandidate.Size = new System.Drawing.Size(243, 100);
            this.lstbCandidate.TabIndex = 4;
            this.lstbCandidate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstbCandidate_KeyDown);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.White;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(18, 153);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(80, 30);
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
            this.btnChange.Location = new System.Drawing.Point(181, 153);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(80, 30);
            this.btnChange.TabIndex = 19;
            this.btnChange.Text = "Sửa";
            this.btnChange.UseVisualStyleBackColor = false;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            this.btnChange.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnChange_KeyDown);
            // 
            // gridLog
            // 
            this.gridLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.wrongContext,
            this.rightContext});
            this.gridLog.Location = new System.Drawing.Point(18, 208);
            this.gridLog.Name = "gridLog";
            this.gridLog.Size = new System.Drawing.Size(243, 295);
            this.gridLog.TabIndex = 20;
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
            // UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.gridLog);
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lstbCandidate);
            this.Controls.Add(this.lblWrong);
            this.Controls.Add(this.btnIgnore);
            this.MinimumSize = new System.Drawing.Size(300, 350);
            this.Name = "UserControl";
            this.Size = new System.Drawing.Size(300, 529);
            ((System.ComponentModel.ISupportInitialize)(this.gridLog)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnIgnore;
        private System.Windows.Forms.Label lblWrong;
        private System.Windows.Forms.ListBox lstbCandidate;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.DataGridView gridLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn wrongContext;
        private System.Windows.Forms.DataGridViewTextBoxColumn rightContext;
    }
}

