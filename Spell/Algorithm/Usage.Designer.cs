namespace Spell.Algorithm
{
    partial class Usage
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGotIt = new System.Windows.Forms.Button();
            this.chkbNoShowAgain = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnGotIt
            // 
            this.btnGotIt.Location = new System.Drawing.Point(197, 226);
            this.btnGotIt.Name = "btnGotIt";
            this.btnGotIt.Size = new System.Drawing.Size(75, 23);
            this.btnGotIt.TabIndex = 0;
            this.btnGotIt.Text = "Tôi đã hiểu";
            this.btnGotIt.UseVisualStyleBackColor = true;
            this.btnGotIt.Click += new System.EventHandler(this.btnGotIt_Click);
            // 
            // chkbNoShowAgain
            // 
            this.chkbNoShowAgain.AutoSize = true;
            this.chkbNoShowAgain.Location = new System.Drawing.Point(25, 230);
            this.chkbNoShowAgain.Name = "chkbNoShowAgain";
            this.chkbNoShowAgain.Size = new System.Drawing.Size(131, 17);
            this.chkbNoShowAgain.TabIndex = 1;
            this.chkbNoShowAgain.Text = "Không hiển thị lần sau";
            this.chkbNoShowAgain.UseVisualStyleBackColor = true;
            // 
            // Usage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.chkbNoShowAgain);
            this.Controls.Add(this.btnGotIt);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Usage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Usage";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Usage_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGotIt;
        private System.Windows.Forms.CheckBox chkbNoShowAgain;
    }
}