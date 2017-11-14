namespace TestCQSAnalyse
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
            this.dataGridViewBase = new System.Windows.Forms.DataGridView();
            this.dataGridViewCompare = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBaseFilename = new System.Windows.Forms.TextBox();
            this.txtCompareFilename = new System.Windows.Forms.TextBox();
            this.btnSelectBaseFilename = new System.Windows.Forms.Button();
            this.btnSelectCompareFilename = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCompare)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewBase
            // 
            this.dataGridViewBase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewBase.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBase.Location = new System.Drawing.Point(12, 49);
            this.dataGridViewBase.Name = "dataGridViewBase";
            this.dataGridViewBase.Size = new System.Drawing.Size(813, 212);
            this.dataGridViewBase.TabIndex = 0;
            // 
            // dataGridViewCompare
            // 
            this.dataGridViewCompare.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewCompare.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCompare.Location = new System.Drawing.Point(12, 312);
            this.dataGridViewCompare.Name = "dataGridViewCompare";
            this.dataGridViewCompare.Size = new System.Drawing.Size(813, 214);
            this.dataGridViewCompare.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Base File Name";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 281);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Compare File Name";
            // 
            // txtBaseFilename
            // 
            this.txtBaseFilename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBaseFilename.Location = new System.Drawing.Point(102, 16);
            this.txtBaseFilename.Name = "txtBaseFilename";
            this.txtBaseFilename.Size = new System.Drawing.Size(663, 20);
            this.txtBaseFilename.TabIndex = 4;
            // 
            // txtCompareFilename
            // 
            this.txtCompareFilename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCompareFilename.Location = new System.Drawing.Point(117, 278);
            this.txtCompareFilename.Name = "txtCompareFilename";
            this.txtCompareFilename.Size = new System.Drawing.Size(648, 20);
            this.txtCompareFilename.TabIndex = 5;
            // 
            // btnSelectBaseFilename
            // 
            this.btnSelectBaseFilename.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSelectBaseFilename.Location = new System.Drawing.Point(783, 16);
            this.btnSelectBaseFilename.Name = "btnSelectBaseFilename";
            this.btnSelectBaseFilename.Size = new System.Drawing.Size(42, 20);
            this.btnSelectBaseFilename.TabIndex = 6;
            this.btnSelectBaseFilename.Text = "...";
            this.btnSelectBaseFilename.UseVisualStyleBackColor = true;
            this.btnSelectBaseFilename.Click += new System.EventHandler(this.btnSelectBaseFilename_Click);
            // 
            // btnSelectCompareFilename
            // 
            this.btnSelectCompareFilename.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSelectCompareFilename.Location = new System.Drawing.Point(781, 278);
            this.btnSelectCompareFilename.Name = "btnSelectCompareFilename";
            this.btnSelectCompareFilename.Size = new System.Drawing.Size(43, 20);
            this.btnSelectCompareFilename.TabIndex = 7;
            this.btnSelectCompareFilename.Text = "...";
            this.btnSelectCompareFilename.UseVisualStyleBackColor = true;
            this.btnSelectCompareFilename.Click += new System.EventHandler(this.btnSelectCompareFilename_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 538);
            this.Controls.Add(this.btnSelectCompareFilename);
            this.Controls.Add(this.btnSelectBaseFilename);
            this.Controls.Add(this.txtCompareFilename);
            this.Controls.Add(this.txtBaseFilename);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewCompare);
            this.Controls.Add(this.dataGridViewBase);
            this.Name = "MainForm";
            this.Text = "Test CQS Analyser Tool";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCompare)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewBase;
        private System.Windows.Forms.DataGridView dataGridViewCompare;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBaseFilename;
        private System.Windows.Forms.TextBox txtCompareFilename;
        private System.Windows.Forms.Button btnSelectBaseFilename;
        private System.Windows.Forms.Button btnSelectCompareFilename;
    }
}

