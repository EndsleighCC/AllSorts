namespace TestADGroup
{
    partial class MainForm
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
            this.lstADGroupMembers = new System.Windows.Forms.ListBox();
            this.btnGroupShowMembers = new System.Windows.Forms.Button();
            this.btnCopyEntries = new System.Windows.Forms.Button();
            this.comboNetworkGroupList = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "AD Group Name";
            // 
            // lstADGroupMembers
            // 
            this.lstADGroupMembers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstADGroupMembers.FormattingEnabled = true;
            this.lstADGroupMembers.Location = new System.Drawing.Point(31, 125);
            this.lstADGroupMembers.Name = "lstADGroupMembers";
            this.lstADGroupMembers.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lstADGroupMembers.Size = new System.Drawing.Size(475, 329);
            this.lstADGroupMembers.TabIndex = 4;
            this.lstADGroupMembers.TabStop = false;
            // 
            // btnGroupShowMembers
            // 
            this.btnGroupShowMembers.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnGroupShowMembers.Location = new System.Drawing.Point(208, 80);
            this.btnGroupShowMembers.Name = "btnGroupShowMembers";
            this.btnGroupShowMembers.Size = new System.Drawing.Size(101, 23);
            this.btnGroupShowMembers.TabIndex = 2;
            this.btnGroupShowMembers.Text = "Show Members";
            this.btnGroupShowMembers.UseVisualStyleBackColor = true;
            this.btnGroupShowMembers.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // btnCopyEntries
            // 
            this.btnCopyEntries.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCopyEntries.Location = new System.Drawing.Point(185, 481);
            this.btnCopyEntries.Name = "btnCopyEntries";
            this.btnCopyEntries.Size = new System.Drawing.Size(156, 23);
            this.btnCopyEntries.TabIndex = 3;
            this.btnCopyEntries.Text = "Copy Entries To Clipboard";
            this.btnCopyEntries.UseVisualStyleBackColor = true;
            this.btnCopyEntries.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // comboNetworkGroupList
            // 
            this.comboNetworkGroupList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboNetworkGroupList.Enabled = false;
            this.comboNetworkGroupList.FormattingEnabled = true;
            this.comboNetworkGroupList.Location = new System.Drawing.Point(119, 43);
            this.comboNetworkGroupList.Name = "comboNetworkGroupList";
            this.comboNetworkGroupList.Size = new System.Drawing.Size(387, 21);
            this.comboNetworkGroupList.TabIndex = 1;
            this.comboNetworkGroupList.Text = "Working ...";
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnGroupShowMembers;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 543);
            this.Controls.Add(this.comboNetworkGroupList);
            this.Controls.Add(this.btnCopyEntries);
            this.Controls.Add(this.btnGroupShowMembers);
            this.Controls.Add(this.lstADGroupMembers);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Test Active Directory Group";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstADGroupMembers;
        private System.Windows.Forms.Button btnGroupShowMembers;
        private System.Windows.Forms.Button btnCopyEntries;
        private System.Windows.Forms.ComboBox comboNetworkGroupList;
    }
}

