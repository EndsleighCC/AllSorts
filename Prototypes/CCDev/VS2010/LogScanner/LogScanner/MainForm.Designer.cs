namespace LogScanner
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
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.tabDisplayType = new System.Windows.Forms.TabControl();
            this.tabFileContents = new System.Windows.Forms.TabPage();
            this.txtFileContents = new System.Windows.Forms.TextBox();
            this.tabContentsFilter = new System.Windows.Forms.TabPage();
            this.btnRemovePairs = new System.Windows.Forms.Button();
            this.txtPairPattern = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSelect = new System.Windows.Forms.TextBox();
            this.txtPattern = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabContentsPairs = new System.Windows.Forms.TabPage();
            this.datagridviewLogEntries = new System.Windows.Forms.DataGridView();
            this.columnEventDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnThreadId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOffsetPercentage = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtFileWindowLengthBytes = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDisplayLineCount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSearchText = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tabDisplayType.SuspendLayout();
            this.tabFileContents.SuspendLayout();
            this.tabContentsFilter.SuspendLayout();
            this.tabContentsPairs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridviewLogEntries)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File name";
            // 
            // txtFileName
            // 
            this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileName.Location = new System.Drawing.Point(86, 22);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(852, 20);
            this.txtFileName.TabIndex = 1;
            this.txtFileName.TextChanged += new System.EventHandler(this.txtFileName_TextChanged);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "EISDBG.LOG";
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectFile.Location = new System.Drawing.Point(944, 20);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(35, 23);
            this.btnSelectFile.TabIndex = 2;
            this.btnSelectFile.Text = "...";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnExecute.Location = new System.Drawing.Point(464, 644);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 3;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // tabDisplayType
            // 
            this.tabDisplayType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabDisplayType.Controls.Add(this.tabFileContents);
            this.tabDisplayType.Controls.Add(this.tabContentsFilter);
            this.tabDisplayType.Controls.Add(this.tabContentsPairs);
            this.tabDisplayType.Location = new System.Drawing.Point(31, 85);
            this.tabDisplayType.Name = "tabDisplayType";
            this.tabDisplayType.SelectedIndex = 0;
            this.tabDisplayType.Size = new System.Drawing.Size(948, 553);
            this.tabDisplayType.TabIndex = 4;
            this.tabDisplayType.SelectedIndexChanged += new System.EventHandler(this.tabDisplayType_SelectedIndexChanged);
            // 
            // tabFileContents
            // 
            this.tabFileContents.Controls.Add(this.txtFileContents);
            this.tabFileContents.Location = new System.Drawing.Point(4, 22);
            this.tabFileContents.Name = "tabFileContents";
            this.tabFileContents.Padding = new System.Windows.Forms.Padding(3);
            this.tabFileContents.Size = new System.Drawing.Size(940, 527);
            this.tabFileContents.TabIndex = 0;
            this.tabFileContents.Text = "File Contents";
            this.tabFileContents.UseVisualStyleBackColor = true;
            // 
            // txtFileContents
            // 
            this.txtFileContents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileContents.HideSelection = false;
            this.txtFileContents.Location = new System.Drawing.Point(20, 21);
            this.txtFileContents.Multiline = true;
            this.txtFileContents.Name = "txtFileContents";
            this.txtFileContents.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFileContents.Size = new System.Drawing.Size(901, 487);
            this.txtFileContents.TabIndex = 0;
            this.txtFileContents.WordWrap = false;
            // 
            // tabContentsFilter
            // 
            this.tabContentsFilter.Controls.Add(this.btnRemovePairs);
            this.tabContentsFilter.Controls.Add(this.txtPairPattern);
            this.tabContentsFilter.Controls.Add(this.label3);
            this.tabContentsFilter.Controls.Add(this.txtSelect);
            this.tabContentsFilter.Controls.Add(this.txtPattern);
            this.tabContentsFilter.Controls.Add(this.label2);
            this.tabContentsFilter.Location = new System.Drawing.Point(4, 22);
            this.tabContentsFilter.Name = "tabContentsFilter";
            this.tabContentsFilter.Padding = new System.Windows.Forms.Padding(3);
            this.tabContentsFilter.Size = new System.Drawing.Size(940, 527);
            this.tabContentsFilter.TabIndex = 1;
            this.tabContentsFilter.Text = "Filter";
            this.tabContentsFilter.UseVisualStyleBackColor = true;
            // 
            // btnRemovePairs
            // 
            this.btnRemovePairs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemovePairs.Location = new System.Drawing.Point(602, 417);
            this.btnRemovePairs.Name = "btnRemovePairs";
            this.btnRemovePairs.Size = new System.Drawing.Size(71, 41);
            this.btnRemovePairs.TabIndex = 9;
            this.btnRemovePairs.Text = "Remove Pairs";
            this.btnRemovePairs.UseVisualStyleBackColor = true;
            this.btnRemovePairs.Click += new System.EventHandler(this.btnRemovePairs_Click);
            // 
            // txtPairPattern
            // 
            this.txtPairPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPairPattern.Location = new System.Drawing.Point(84, 429);
            this.txtPairPattern.Name = "txtPairPattern";
            this.txtPairPattern.Size = new System.Drawing.Size(501, 20);
            this.txtPairPattern.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 432);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Pair Pattern";
            // 
            // txtSelect
            // 
            this.txtSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSelect.Location = new System.Drawing.Point(13, 53);
            this.txtSelect.Multiline = true;
            this.txtSelect.Name = "txtSelect";
            this.txtSelect.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSelect.Size = new System.Drawing.Size(661, 358);
            this.txtSelect.TabIndex = 6;
            this.txtSelect.WordWrap = false;
            // 
            // txtPattern
            // 
            this.txtPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPattern.Location = new System.Drawing.Point(82, 18);
            this.txtPattern.Name = "txtPattern";
            this.txtPattern.Size = new System.Drawing.Size(592, 20);
            this.txtPattern.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Filter Pattern";
            // 
            // tabContentsPairs
            // 
            this.tabContentsPairs.Controls.Add(this.datagridviewLogEntries);
            this.tabContentsPairs.Location = new System.Drawing.Point(4, 22);
            this.tabContentsPairs.Name = "tabContentsPairs";
            this.tabContentsPairs.Padding = new System.Windows.Forms.Padding(3);
            this.tabContentsPairs.Size = new System.Drawing.Size(940, 527);
            this.tabContentsPairs.TabIndex = 2;
            this.tabContentsPairs.Text = "Pairs";
            this.tabContentsPairs.UseVisualStyleBackColor = true;
            // 
            // datagridviewLogEntries
            // 
            this.datagridviewLogEntries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagridviewLogEntries.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnEventDateTime,
            this.columnThreadId,
            this.columnText});
            this.datagridviewLogEntries.Location = new System.Drawing.Point(6, 6);
            this.datagridviewLogEntries.Name = "datagridviewLogEntries";
            this.datagridviewLogEntries.ReadOnly = true;
            this.datagridviewLogEntries.Size = new System.Drawing.Size(677, 452);
            this.datagridviewLogEntries.TabIndex = 0;
            // 
            // columnEventDateTime
            // 
            this.columnEventDateTime.HeaderText = "Date/Time";
            this.columnEventDateTime.Name = "columnEventDateTime";
            this.columnEventDateTime.ReadOnly = true;
            // 
            // columnThreadId
            // 
            this.columnThreadId.HeaderText = "Thread Id";
            this.columnThreadId.Name = "columnThreadId";
            this.columnThreadId.ReadOnly = true;
            // 
            // columnText
            // 
            this.columnText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnText.HeaderText = "Text";
            this.columnText.Name = "columnText";
            this.columnText.ReadOnly = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Offset [%]";
            // 
            // txtOffsetPercentage
            // 
            this.txtOffsetPercentage.Location = new System.Drawing.Point(87, 50);
            this.txtOffsetPercentage.Name = "txtOffsetPercentage";
            this.txtOffsetPercentage.Size = new System.Drawing.Size(100, 20);
            this.txtOffsetPercentage.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(200, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Length [bytes]";
            // 
            // txtFileWindowLengthBytes
            // 
            this.txtFileWindowLengthBytes.Location = new System.Drawing.Point(273, 50);
            this.txtFileWindowLengthBytes.Name = "txtFileWindowLengthBytes";
            this.txtFileWindowLengthBytes.Size = new System.Drawing.Size(100, 20);
            this.txtFileWindowLengthBytes.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(391, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Line Count";
            // 
            // txtDisplayLineCount
            // 
            this.txtDisplayLineCount.Location = new System.Drawing.Point(453, 51);
            this.txtDisplayLineCount.Name = "txtDisplayLineCount";
            this.txtDisplayLineCount.ReadOnly = true;
            this.txtDisplayLineCount.Size = new System.Drawing.Size(100, 20);
            this.txtDisplayLineCount.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(584, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Search";
            // 
            // txtSearchText
            // 
            this.txtSearchText.Location = new System.Drawing.Point(631, 51);
            this.txtSearchText.Name = "txtSearchText";
            this.txtSearchText.Size = new System.Drawing.Size(261, 20);
            this.txtSearchText.TabIndex = 12;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(907, 51);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(67, 21);
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnExecute;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 682);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearchText);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtDisplayLineCount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtFileWindowLengthBytes);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtOffsetPercentage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tabDisplayType);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Log Scanner";
            this.tabDisplayType.ResumeLayout(false);
            this.tabFileContents.ResumeLayout(false);
            this.tabFileContents.PerformLayout();
            this.tabContentsFilter.ResumeLayout(false);
            this.tabContentsFilter.PerformLayout();
            this.tabContentsPairs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datagridviewLogEntries)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.TabControl tabDisplayType;
        private System.Windows.Forms.TabPage tabFileContents;
        private System.Windows.Forms.TabPage tabContentsFilter;
        private System.Windows.Forms.TextBox txtFileContents;
        private System.Windows.Forms.TabPage tabContentsPairs;
        private System.Windows.Forms.TextBox txtSelect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView datagridviewLogEntries;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnEventDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnThreadId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnText;
        private System.Windows.Forms.TextBox txtPattern;
        private System.Windows.Forms.Button btnRemovePairs;
        private System.Windows.Forms.TextBox txtPairPattern;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtOffsetPercentage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtFileWindowLengthBytes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDisplayLineCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSearchText;
        private System.Windows.Forms.Button btnSearch;
    }
}

