namespace TestDotNetZip
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
            this.btnExecute = new System.Windows.Forms.Button();
            this.txtDisplay = new System.Windows.Forms.TextBox();
            this.txtZipName = new System.Windows.Forms.TextBox();
            this.txtFolderName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDirectory = new System.Windows.Forms.Button();
            this.fldbFolderSelect = new System.Windows.Forms.FolderBrowserDialog();
            this.btnZipNameDirectory = new System.Windows.Forms.Button();
            this.fldbZipFileName = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(216, 431);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 0;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // txtDisplay
            // 
            this.txtDisplay.Location = new System.Drawing.Point(30, 113);
            this.txtDisplay.Multiline = true;
            this.txtDisplay.Name = "txtDisplay";
            this.txtDisplay.ReadOnly = true;
            this.txtDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDisplay.Size = new System.Drawing.Size(452, 301);
            this.txtDisplay.TabIndex = 1;
            // 
            // txtZipName
            // 
            this.txtZipName.Location = new System.Drawing.Point(88, 32);
            this.txtZipName.Name = "txtZipName";
            this.txtZipName.Size = new System.Drawing.Size(320, 20);
            this.txtZipName.TabIndex = 2;
            // 
            // txtFolderName
            // 
            this.txtFolderName.Location = new System.Drawing.Point(88, 72);
            this.txtFolderName.Name = "txtFolderName";
            this.txtFolderName.Size = new System.Drawing.Size(320, 20);
            this.txtFolderName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Zip Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Folder";
            // 
            // btnDirectory
            // 
            this.btnDirectory.Location = new System.Drawing.Point(423, 71);
            this.btnDirectory.Name = "btnDirectory";
            this.btnDirectory.Size = new System.Drawing.Size(58, 21);
            this.btnDirectory.TabIndex = 6;
            this.btnDirectory.Text = "Directory";
            this.btnDirectory.UseVisualStyleBackColor = true;
            this.btnDirectory.Click += new System.EventHandler(this.btnDirectory_Click);
            // 
            // fldbFolderSelect
            // 
            this.fldbFolderSelect.SelectedPath = "C:\\ST00\\TEST";
            // 
            // btnZipNameDirectory
            // 
            this.btnZipNameDirectory.Location = new System.Drawing.Point(423, 32);
            this.btnZipNameDirectory.Name = "btnZipNameDirectory";
            this.btnZipNameDirectory.Size = new System.Drawing.Size(58, 21);
            this.btnZipNameDirectory.TabIndex = 7;
            this.btnZipNameDirectory.Text = "Directory";
            this.btnZipNameDirectory.UseVisualStyleBackColor = true;
            this.btnZipNameDirectory.Click += new System.EventHandler(this.btnZipNameDirectory_Click);
            // 
            // fldbZipFileName
            // 
            this.fldbZipFileName.SelectedPath = "C:\\ST00\\TestCode";
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnExecute;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 486);
            this.Controls.Add(this.btnZipNameDirectory);
            this.Controls.Add(this.btnDirectory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFolderName);
            this.Controls.Add(this.txtZipName);
            this.Controls.Add(this.txtDisplay);
            this.Controls.Add(this.btnExecute);
            this.Name = "MainForm";
            this.Text = "DotNetZip Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.TextBox txtDisplay;
        private System.Windows.Forms.TextBox txtZipName;
        private System.Windows.Forms.TextBox txtFolderName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDirectory;
        private System.Windows.Forms.FolderBrowserDialog fldbFolderSelect;
        private System.Windows.Forms.Button btnZipNameDirectory;
        private System.Windows.Forms.FolderBrowserDialog fldbZipFileName;
    }
}

