using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestCQSAnalyse
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            dataGridViewBase.ColumnCount = 2;
            dataGridViewCompare.ColumnCount = 2;
        }

        private void btnSelectBaseFilename_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;

            openFileDialog.InitialDirectory = "C:\\ST00\\Temp";
            openFileDialog.FileName = "TestBase.csv";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                txtBaseFilename.Text = openFileDialog.FileName;
            else
            {
                txtBaseFilename.Text = String.Empty;
                MessageBox.Show("A valid base filename is required", "Base Filename Error");
            }
        }

        private void btnSelectCompareFilename_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;

            openFileDialog.InitialDirectory = "C:\\ST00\\Temp";
            openFileDialog.FileName = "TestComapre.csv";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                txtCompareFilename.Text = openFileDialog.FileName;
            else
            {
                txtBaseFilename.Text = String.Empty;
                MessageBox.Show("A valid compare filename is required", "Compare Filename Error");
            }
        }

    } // MainForm
}
