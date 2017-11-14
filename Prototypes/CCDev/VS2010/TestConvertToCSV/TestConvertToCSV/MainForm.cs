using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TestConvertToCSV
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            radTab.Select();
        }

        private void ShowFileSnippet( string filename )
        {
            try
            {

                txtDisplay.Text = "";

                using (StreamReader inputFile = new StreamReader(txtFilename.Text))
                {
                    string textLine = inputFile.ReadLine();
                    for (int lineIndex = 0; (textLine != null) && (lineIndex < 50); ++lineIndex)
                    {
                        txtDisplay.Text += textLine + Environment.NewLine;
                        textLine = inputFile.ReadLine();
                    }
                }

            }
            catch (Exception eek)
            {
            }
        }

        private void btnFilenameSelect_Click(object sender, EventArgs e)
        {
            ofdSelectFileDialog.Filter = "CSV Files|*.csv";
            if (ofdSelectFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFilename.Text = ofdSelectFileDialog.FileName;

                ShowFileSnippet(txtFilename.Text);
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtFilename.Text))
                MessageBox.Show("Unable to open \"" + txtFilename.Text + "\"", "Convert To CSV");
            else
            {
                // File exists

                char characterFrom = '\t';
                char characterTo = ',';

                if (radTab.Checked)
                {
                    characterFrom = '\t';
                    characterTo = ',';
                }
                else
                {
                    characterFrom = ',';
                    characterTo = '\t';
                }

                using (FileStream fileStreamRead = new FileStream(txtFilename.Text, FileMode.Open, FileAccess.Read,FileShare.Write))
                {

                    using ( StreamReader streamReader = new StreamReader( fileStreamRead ) )
                    {

                        using (FileStream fileStreamWrite = new FileStream(txtFilename.Text, FileMode.Open, FileAccess.Write))
                        {

                            using ( StreamWriter streamWriter = new StreamWriter( fileStreamWrite ) )
                            {

                                string textLineRead = streamReader.ReadLine();
                                while ( textLineRead != null )
                                {
                                    string textLineWrite = textLineRead.Replace(characterFrom, characterTo);
                                    streamWriter.WriteLine(textLineWrite);
                                    // Read the next line
                                    textLineRead = streamReader.ReadLine();
                                }

                            }

                        }

                    }

                }

                ShowFileSnippet(txtFilename.Text);

            } // File exists

        } // btnConvert_Click
    }
}
