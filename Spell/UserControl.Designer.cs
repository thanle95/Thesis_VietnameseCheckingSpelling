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
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblWrong = new System.Windows.Forms.Label();
            this.lstbCandidate = new System.Windows.Forms.ListBox();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnChange = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnIgnore
            // 
            this.btnIgnore.AutoSize = true;
            this.btnIgnore.BackColor = System.Drawing.Color.White;
            this.btnIgnore.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIgnore.ForeColor = System.Drawing.Color.Black;
            this.btnIgnore.Location = new System.Drawing.Point(18, 32);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(77, 27);
            this.btnIgnore.TabIndex = 0;
            this.btnIgnore.Text = "Bỏ qua";
            this.btnIgnore.UseVisualStyleBackColor = false;
            this.btnIgnore.Click += new System.EventHandler(this.btnIgnore_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.AutoSize = true;
            this.btnAdd.BackColor = System.Drawing.Color.White;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(133, 32);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(128, 27);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Thêm vào từ điển";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblWrong
            // 
            this.lblWrong.AutoSize = true;
            this.lblWrong.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWrong.ForeColor = System.Drawing.Color.Black;
            this.lblWrong.Location = new System.Drawing.Point(18, 2);
            this.lblWrong.Name = "lblWrong";
            this.lblWrong.Size = new System.Drawing.Size(114, 21);
            this.lblWrong.TabIndex = 3;
            this.lblWrong.Text = "\"Wrong Text\"";
            // 
            // lstbCandidate
            // 
            this.lstbCandidate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstbCandidate.FormattingEnabled = true;
            this.lstbCandidate.ItemHeight = 16;
            this.lstbCandidate.Location = new System.Drawing.Point(18, 68);
            this.lstbCandidate.Name = "lstbCandidate";
            this.lstbCandidate.Size = new System.Drawing.Size(243, 100);
            this.lstbCandidate.TabIndex = 4;
            this.lstbCandidate.SelectedIndexChanged += new System.EventHandler(this.lstbCandidate_SelectedIndexChanged);
            this.lstbCandidate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstbCandidate_KeyDown);
            // 
            // btnUndo
            // 
            this.btnUndo.BackColor = System.Drawing.Color.White;
            this.btnUndo.Enabled = false;
            this.btnUndo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUndo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnUndo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUndo.Location = new System.Drawing.Point(205, 223);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(56, 27);
            this.btnUndo.TabIndex = 15;
            this.btnUndo.Text = "Lùi lại";
            this.btnUndo.UseVisualStyleBackColor = false;
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.White;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(18, 182);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(84, 25);
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
            this.btnChange.Location = new System.Drawing.Point(184, 181);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(77, 27);
            this.btnChange.TabIndex = 19;
            this.btnChange.Text = "Sửa";
            this.btnChange.UseVisualStyleBackColor = false;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            this.btnChange.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnChange_KeyDown);
            // 
            // UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.lstbCandidate);
            this.Controls.Add(this.lblWrong);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnIgnore);
            this.MinimumSize = new System.Drawing.Size(300, 350);
            this.Name = "UserControl";
            this.Size = new System.Drawing.Size(300, 350);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnIgnore;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblWrong;
        private System.Windows.Forms.ListBox lstbCandidate;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnChange;
    }
}

