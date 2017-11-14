namespace TestConvertToCSV
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
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFilenameSelect = new System.Windows.Forms.Button();
            this.ofdSelectFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnConvert = new System.Windows.Forms.Button();
            this.txtDisplay = new System.Windows.Forms.TextBox();
            this.grpConvert = new System.Windows.Forms.GroupBox();
            this.radTab = new System.Windows.Forms.RadioButton();
            this.radComma = new System.Windows.Forms.RadioButton();
            this.grpConvert.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFilename
            // 
            this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilename.Location = new System.Drawing.Point(85, 30);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(334, 20);
            this.txtFilename.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Filename";
            // 
            // btnFilenameSelect
            // 
            this.btnFilenameSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilenameSelect.Location = new System.Drawing.Point(443, 28);
            this.btnFilenameSelect.Name = "btnFilenameSelect";
            this.btnFilenameSelect.Size = new System.Drawing.Size(39, 23);
            this.btnFilenameSelect.TabIndex = 2;
            this.btnFilenameSelect.Text = "...";
            this.btnFilenameSelect.UseVisualStyleBackColor = true;
            this.btnFilenameSelect.Click += new System.EventHandler(this.btnFilenameSelect_Click);
            // 
            // ofdSelectFileDialog
            // 
            this.ofdSelectFileDialog.FileName = "openFileDialog1";
            // 
            // btnConvert
            // 
            this.btnConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConvert.Location = new System.Drawing.Point(311, 77);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 3;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // txtDisplay
            // 
            this.txtDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDisplay.Location = new System.Drawing.Point(22, 126);
            this.txtDisplay.Multiline = true;
            this.txtDisplay.Name = "txtDisplay";
            this.txtDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDisplay.Size = new System.Drawing.Size(460, 359);
            this.txtDisplay.TabIndex = 4;
            this.txtDisplay.WordWrap = false;
            // 
            // grpConvert
            // 
            this.grpConvert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpConvert.Controls.Add(this.radComma);
            this.grpConvert.Controls.Add(this.radTab);
            this.grpConvert.Location = new System.Drawing.Point(67, 66);
            this.grpConvert.Name = "grpConvert";
            this.grpConvert.Size = new System.Drawing.Size(200, 43);
            this.grpConvert.TabIndex = 5;
            this.grpConvert.TabStop = false;
            this.grpConvert.Text = "Convert from";
            // 
            // radTab
            // 
            this.radTab.AutoSize = true;
            this.radTab.Location = new System.Drawing.Point(35, 17);
            this.radTab.Name = "radTab";
            this.radTab.Size = new System.Drawing.Size(44, 17);
            this.radTab.TabIndex = 6;
            this.radTab.TabStop = true;
            this.radTab.Text = "Tab";
            this.radTab.UseVisualStyleBackColor = true;
            // 
            // radComma
            // 
            this.radComma.AutoSize = true;
            this.radComma.Location = new System.Drawing.Point(117, 17);
            this.radComma.Name = "radComma";
            this.radComma.Size = new System.Drawing.Size(60, 17);
            this.radComma.TabIndex = 6;
            this.radComma.TabStop = true;
            this.radComma.Text = "Comma";
            this.radComma.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnFilenameSelect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 511);
            this.Controls.Add(this.grpConvert);
            this.Controls.Add(this.txtDisplay);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.btnFilenameSelect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFilename);
            this.Name = "MainForm";
            this.Text = "Convert to CSV";
            this.grpConvert.ResumeLayout(false);
            this.grpConvert.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFilenameSelect;
        private System.Windows.Forms.OpenFileDialog ofdSelectFileDialog;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.TextBox txtDisplay;
        private System.Windows.Forms.GroupBox grpConvert;
        private System.Windows.Forms.RadioButton radComma;
        private System.Windows.Forms.RadioButton radTab;
    }
}

