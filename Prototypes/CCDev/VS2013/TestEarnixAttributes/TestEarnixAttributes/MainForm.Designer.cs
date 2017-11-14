namespace TestEarnixAttributes
{
    partial class frmMain
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLeftFilename = new System.Windows.Forms.TextBox();
            this.btnLeftFilename = new System.Windows.Forms.Button();
            this.txtRightFilename = new System.Windows.Forms.TextBox();
            this.btnRightFilename = new System.Windows.Forms.Button();
            this.txtLeftFile = new System.Windows.Forms.TextBox();
            this.txtRightFile = new System.Windows.Forms.TextBox();
            this.txtLeftDiffFromRight = new System.Windows.Forms.TextBox();
            this.txtRightDiffFromLeft = new System.Windows.Forms.TextBox();
            this.txtLeftError = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRightError = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Left filename";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(635, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Right filename";
            // 
            // txtLeftFilename
            // 
            this.txtLeftFilename.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtLeftFilename.Location = new System.Drawing.Point(132, 36);
            this.txtLeftFilename.Name = "txtLeftFilename";
            this.txtLeftFilename.Size = new System.Drawing.Size(365, 20);
            this.txtLeftFilename.TabIndex = 2;
            // 
            // btnLeftFilename
            // 
            this.btnLeftFilename.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnLeftFilename.Location = new System.Drawing.Point(517, 38);
            this.btnLeftFilename.Name = "btnLeftFilename";
            this.btnLeftFilename.Size = new System.Drawing.Size(40, 17);
            this.btnLeftFilename.TabIndex = 3;
            this.btnLeftFilename.Text = "...";
            this.btnLeftFilename.UseVisualStyleBackColor = true;
            this.btnLeftFilename.Click += new System.EventHandler(this.btnLeftFilename_Click);
            // 
            // txtRightFilename
            // 
            this.txtRightFilename.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtRightFilename.Location = new System.Drawing.Point(715, 36);
            this.txtRightFilename.Name = "txtRightFilename";
            this.txtRightFilename.Size = new System.Drawing.Size(365, 20);
            this.txtRightFilename.TabIndex = 4;
            // 
            // btnRightFilename
            // 
            this.btnRightFilename.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRightFilename.Location = new System.Drawing.Point(1100, 37);
            this.btnRightFilename.Name = "btnRightFilename";
            this.btnRightFilename.Size = new System.Drawing.Size(40, 18);
            this.btnRightFilename.TabIndex = 5;
            this.btnRightFilename.Text = "...";
            this.btnRightFilename.UseVisualStyleBackColor = true;
            this.btnRightFilename.Click += new System.EventHandler(this.btnRightFilename_Click);
            // 
            // txtLeftFile
            // 
            this.txtLeftFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtLeftFile.Location = new System.Drawing.Point(12, 100);
            this.txtLeftFile.Multiline = true;
            this.txtLeftFile.Name = "txtLeftFile";
            this.txtLeftFile.ReadOnly = true;
            this.txtLeftFile.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLeftFile.Size = new System.Drawing.Size(562, 433);
            this.txtLeftFile.TabIndex = 6;
            // 
            // txtRightFile
            // 
            this.txtRightFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRightFile.Location = new System.Drawing.Point(593, 100);
            this.txtRightFile.Multiline = true;
            this.txtRightFile.Name = "txtRightFile";
            this.txtRightFile.ReadOnly = true;
            this.txtRightFile.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRightFile.Size = new System.Drawing.Size(591, 433);
            this.txtRightFile.TabIndex = 7;
            // 
            // txtLeftDiffFromRight
            // 
            this.txtLeftDiffFromRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtLeftDiffFromRight.Location = new System.Drawing.Point(12, 568);
            this.txtLeftDiffFromRight.Multiline = true;
            this.txtLeftDiffFromRight.Name = "txtLeftDiffFromRight";
            this.txtLeftDiffFromRight.ReadOnly = true;
            this.txtLeftDiffFromRight.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLeftDiffFromRight.Size = new System.Drawing.Size(562, 264);
            this.txtLeftDiffFromRight.TabIndex = 8;
            // 
            // txtRightDiffFromLeft
            // 
            this.txtRightDiffFromLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRightDiffFromLeft.Location = new System.Drawing.Point(593, 568);
            this.txtRightDiffFromLeft.Multiline = true;
            this.txtRightDiffFromLeft.Name = "txtRightDiffFromLeft";
            this.txtRightDiffFromLeft.ReadOnly = true;
            this.txtRightDiffFromLeft.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRightDiffFromLeft.Size = new System.Drawing.Size(591, 264);
            this.txtRightDiffFromLeft.TabIndex = 9;
            // 
            // txtLeftError
            // 
            this.txtLeftError.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtLeftError.Location = new System.Drawing.Point(132, 70);
            this.txtLeftError.Name = "txtLeftError";
            this.txtLeftError.ReadOnly = true;
            this.txtLeftError.Size = new System.Drawing.Size(365, 20);
            this.txtLeftError.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "ErrorText";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(635, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "ErrorText";
            // 
            // txtRightError
            // 
            this.txtRightError.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtRightError.Location = new System.Drawing.Point(715, 70);
            this.txtRightError.Name = "txtRightError";
            this.txtRightError.ReadOnly = true;
            this.txtRightError.Size = new System.Drawing.Size(365, 20);
            this.txtRightError.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 543);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(165, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Differences of left relative to right:";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(590, 543);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(165, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Differences of right relative to left:";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1197, 848);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtRightError);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLeftError);
            this.Controls.Add(this.txtRightDiffFromLeft);
            this.Controls.Add(this.txtLeftDiffFromRight);
            this.Controls.Add(this.txtRightFile);
            this.Controls.Add(this.txtLeftFile);
            this.Controls.Add(this.btnRightFilename);
            this.Controls.Add(this.txtRightFilename);
            this.Controls.Add(this.btnLeftFilename);
            this.Controls.Add(this.txtLeftFilename);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmMain";
            this.Text = "Earnix Attribute Reader";
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLeftFilename;
        private System.Windows.Forms.Button btnLeftFilename;
        private System.Windows.Forms.TextBox txtRightFilename;
        private System.Windows.Forms.Button btnRightFilename;
        private System.Windows.Forms.TextBox txtLeftFile;
        private System.Windows.Forms.TextBox txtRightFile;
        private System.Windows.Forms.TextBox txtLeftDiffFromRight;
        private System.Windows.Forms.TextBox txtRightDiffFromLeft;
        private System.Windows.Forms.TextBox txtLeftError;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRightError;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

