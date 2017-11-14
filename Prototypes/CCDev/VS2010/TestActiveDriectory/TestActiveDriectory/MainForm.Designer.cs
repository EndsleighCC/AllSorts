namespace TestActiveDriectory
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
            this.chklbComputers = new System.Windows.Forms.CheckedListBox();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblObjectCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.lstResults = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // chklbComputers
            // 
            this.chklbComputers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chklbComputers.FormattingEnabled = true;
            this.chklbComputers.Location = new System.Drawing.Point(21, 84);
            this.chklbComputers.Name = "chklbComputers";
            this.chklbComputers.Size = new System.Drawing.Size(357, 124);
            this.chklbComputers.Sorted = true;
            this.chklbComputers.TabIndex = 0;
            this.chklbComputers.SelectedIndexChanged += new System.EventHandler(this.chklbComputers_SelectedIndexChanged);
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnExecute.Location = new System.Drawing.Point(160, 376);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 1;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(160, 50);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblObjectCount
            // 
            this.lblObjectCount.AutoSize = true;
            this.lblObjectCount.Location = new System.Drawing.Point(122, 225);
            this.lblObjectCount.Name = "lblObjectCount";
            this.lblObjectCount.Size = new System.Drawing.Size(141, 13);
            this.lblObjectCount.TabIndex = 3;
            this.lblObjectCount.Text = "Count of objects is unknown";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(67, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Active Directory Filter";
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(212, 18);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(100, 20);
            this.txtFilter.TabIndex = 5;
            // 
            // lstResults
            // 
            this.lstResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstResults.FormattingEnabled = true;
            this.lstResults.Location = new System.Drawing.Point(21, 254);
            this.lstResults.Name = "lstResults";
            this.lstResults.ScrollAlwaysVisible = true;
            this.lstResults.Size = new System.Drawing.Size(357, 108);
            this.lstResults.TabIndex = 6;
            // 
            // frmMain
            // 
            this.AcceptButton = this.btnLoad;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 422);
            this.Controls.Add(this.lstResults);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblObjectCount);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.chklbComputers);
            this.Name = "frmMain";
            this.Text = "Test Active Directory Queries";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox chklbComputers;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblObjectCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.ListBox lstResults;
    }
}

