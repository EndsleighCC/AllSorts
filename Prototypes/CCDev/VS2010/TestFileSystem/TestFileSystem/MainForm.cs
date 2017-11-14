using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TestFileSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if ((String.IsNullOrEmpty(txtPath.Text)) || (String.IsNullOrWhiteSpace(txtPath.Text))
                )
            {
                MessageBox.Show("Path is empty", "TestFileSystem");
            }
            else if ((String.IsNullOrEmpty(txtPattern.Text)) || (String.IsNullOrWhiteSpace(txtPattern.Text)))
            {
                MessageBox.Show("Pattern is empty", "TestFileSystem");
            }
            else
            {
                IEnumerable<string> fileNames = Directory.EnumerateFiles(txtPath.Text, txtPattern.Text,
                                                                         SearchOption.AllDirectories);

                foreach (string filename in fileNames)
                {
                    lstboxFilenames.Items.Add(filename);
                }
            }
        }
    }
}
