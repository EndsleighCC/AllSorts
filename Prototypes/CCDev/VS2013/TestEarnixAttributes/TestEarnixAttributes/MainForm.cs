using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestEarnixAttributes
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnLeftFilename_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;

            openFileDialog.InitialDirectory = "\\\\VINCENT\\UT00\\Logs\\earnix";
            openFileDialog.FileName = null;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtLeftFilename.Text = openFileDialog.FileName;

                EarnixAttributeReader earnixAttributeReader = new EarnixAttributeReader(txtLeftFilename.Text);

                _leftSortedDictionary = earnixAttributeReader.EarnixAttributesDictionary;

                txtLeftFile.Clear();

                if (earnixAttributeReader.Error )
                {
                    txtLeftError.Text = earnixAttributeReader.ErrorText;
                }
                else
                {
                    txtLeftError.Text = String.Empty;
                }

                foreach (KeyValuePair<string, string> keyValuePair in _leftSortedDictionary)
                {
                    txtLeftFile.AppendText( keyValuePair.Key + "=\"" + keyValuePair.Value + "\"" + Environment.NewLine);
                }

                AnalyseDifferences();

            }
            else
            {
                txtLeftFilename.Text = String.Empty;
                MessageBox.Show("A valid left filename is required", "Left Filename ErrorText");
            }
        }

        private void btnRightFilename_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;

            openFileDialog.InitialDirectory = "\\\\VINCENT\\PR00\\Logs\\earnix";
            openFileDialog.FileName = null;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtRightFilename.Text = openFileDialog.FileName;

                EarnixAttributeReader earnixAttributeReader = new EarnixAttributeReader(txtRightFilename.Text);

                _rightSortedDictionary = earnixAttributeReader.EarnixAttributesDictionary;

                txtRightFile.Clear();

                if (earnixAttributeReader.Error )
                {
                    txtRightError.Text = earnixAttributeReader.ErrorText;
                }
                else
                {
                    txtLeftError.Text = String.Empty;
                }

                foreach (KeyValuePair<string, string> keyValuePair in _rightSortedDictionary)
                {
                    txtRightFile.AppendText(keyValuePair.Key + "=\"" + keyValuePair.Value + "\"" + Environment.NewLine);
                }

                AnalyseDifferences();
            }
            else
            {
                txtRightFilename.Text = String.Empty;
                MessageBox.Show("A valid right filename is required", "Right Filename ErrorText");
            }
        }

        private void ShowDifferences(   SortedDictionary<string, string> referenceSortedDictionary,
                                        SortedDictionary<string, string> compareSortedDictionary,
                                        TextBox textBox)
        {
            foreach (KeyValuePair<string, string> keyValuePair in referenceSortedDictionary)
            {
                string difference = null;
                if ( ! compareSortedDictionary.ContainsKey(keyValuePair.Key))
                {
                    difference = " + ";
                }
                else
                {
                    if (String.Compare(keyValuePair.Value, compareSortedDictionary[keyValuePair.Key]) != 0)
                    {
                        difference = " ! ";
                    }
                }
                if (difference != null)
                {
                    textBox.AppendText(difference + keyValuePair.Key + "=" + keyValuePair.Value +
                                                    Environment.NewLine);
                }
            }
            
        }

        private void AnalyseDifferences()
        {
            txtLeftDiffFromRight.Clear();
            txtRightDiffFromLeft.Clear();
            if ((_leftSortedDictionary != null) && (_rightSortedDictionary != null))
            {

                // Look for what is in left that is not in right

                ShowDifferences( _leftSortedDictionary,_rightSortedDictionary,txtLeftDiffFromRight);
                ShowDifferences( _rightSortedDictionary, _leftSortedDictionary, txtRightDiffFromLeft);
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            // txtLeftFile.Size = new System.Drawing.Size((this.Size.Width - 50)/2,(this.Size.Height-50)/2);
            // txtRightFile.Size = new System.Drawing.Size((this.Size.Width - 50) / 2, (this.Size.Height - 50) / 2);
        }

        private SortedDictionary<string, string> _leftSortedDictionary = null;
        private SortedDictionary<string, string> _rightSortedDictionary = null;


    }
}
