using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TestRegularExpressions
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private const string _applicationName = "Regular Expression Tester";

        private void ReplaceTableNames()
        {
            // Set up a cascade replacement possibility
            SortedDictionary<string, string> tableNameMapCurrentToNew = new SortedDictionary<string, string> { { "ABCDEFTT.001", "ABCDEFRR.002" }, { "ABCDEFRR.002", "ABCDEFRR.003" } };
            string Contents = txtSearchIn.Text;
            var matches = Regex.Matches(Contents, txtPattern.Text, RegexOptions.IgnoreCase);

            // In case the item being replaced is not the same length as the
            // original item keep track of the position as replacement occurs
            int positionChange = 0;
            foreach (Match match in matches)
            {
                // Work out what to replace with
                int itemPosition = match.Index + positionChange;
                string tableNameToReplace = Contents.Substring(itemPosition, match.Length);
                if (tableNameMapCurrentToNew.Keys.Contains(tableNameToReplace))
                {
                    string tableNameReplacement = tableNameMapCurrentToNew[tableNameToReplace];
                    Contents = Contents.Remove(itemPosition, match.Length);
                    Contents = Contents.Insert(itemPosition, tableNameReplacement);
                    positionChange += tableNameReplacement.Length - tableNameToReplace.Length;
                }

            } // foreach

            txtDisplay.Text += "Table Name Replace = \"" + Contents + "\"" + Environment.NewLine;
        }


        private void ReplaceItems()
        {
            // Set up a cascade replacement possibility
            SortedDictionary<string, string> tableNameMapCurrentToNew = new SortedDictionary<string, string> { { "ABC", "WXYZ" }, { "WXYZ", "MNOPQ" } };
            string Contents = txtSearchIn.Text ;
            var matches = Regex.Matches(Contents, "ABC" , RegexOptions.IgnoreCase);

            // In case the item being replaced is not the same length as the
            // original item keep track of the position as replacement occurs
            int positionChange = 0;
            foreach (Match match in matches)
            {
                // Work out what to replace with
                int itemPosition = match.Index + positionChange;
                string tableNameToReplace = Contents.Substring(itemPosition, match.Length);
                if (tableNameMapCurrentToNew.Keys.Contains(tableNameToReplace))
                {
                    string tableNameReplacement = tableNameMapCurrentToNew[tableNameToReplace];
                    Contents = Contents.Remove(itemPosition, match.Length);
                    Contents = Contents.Insert(itemPosition, tableNameReplacement);
                    positionChange += tableNameReplacement.Length - tableNameToReplace.Length;
                }

            } // foreach

            txtDisplay.Text += "Item Replace = \"" + Contents + "\"" + Environment.NewLine;
        }

        private void TestVersion()
        {
            string versionComma = "4,28,1,3";
            string versionDots = "4.28.1.3";

            string[] versionParts = versionComma.Split(',');
            versionParts = versionComma.Split('.');
            versionParts = versionDots.Split('.');

            string PRODUCT_VERSION = "PRODUCTVERSION";
            string BuildPath = "C:\\ST00";

            // Read the inet version file.
            StreamReader inetVersionReader =
                new StreamReader(Path.Combine(BuildPath, @"INETBase\Code\ECOMINETVersion\ECOMINETVersion.rc"));
            string inetVersionText = inetVersionReader.ReadToEnd();
            string version = String.Empty;

            inetVersionReader.Close();

            // Establish the location of the .NET version files.
            string dotNetVersionPath = Path.Combine(BuildPath, "Endsleigh");

            // Start by trying to find the current version by using a regular expression to parse the inetVersionFile.
            Regex regexVersionToFind = new Regex(String.Format("({0}[ ])([0-9,]*)", PRODUCT_VERSION));
            Match versionToFindMatch = regexVersionToFind.Match(inetVersionText);

            if (versionToFindMatch.Success)
            {
                // We've found the version text.
                version = versionToFindMatch.Groups[2].Value;

                versionParts = version.Split(',');

            }

        }

        void TestResourceVersion()
        {
		    string ENDSLEIGH_VERSION_FILE = "\\INETBase\\Bin\\ECOMINETVersion.dll";

			FileVersionInfo inetVersion = FileVersionInfo.GetVersionInfo(@"C:\Program Files\insurance.net"
                                            + ENDSLEIGH_VERSION_FILE);

            string productVersion = inetVersion.ProductVersion;
            string[] versionNumber;
            if (inetVersion.ProductVersion.IndexOf('.') != -1)
            {
                // Visual Studio 2010 Product Version
                versionNumber = inetVersion.ProductVersion.Split(new char[] { '.' });
                string [] otherVersionNumber = inetVersion.ProductVersion.Split(new char[] { ',' });
            }
            else
            {
                // Visual Studio 6 Product Version
                versionNumber = inetVersion.ProductVersion.Split(new char[] { ',' });
            }
            char pad = ('0');
            string versionString = versionNumber[0].Trim().PadLeft(2, pad) + "." + versionNumber[1].Trim().PadLeft(2, pad) + "." + versionNumber[2].Trim().PadLeft(2, pad) + "." + versionNumber[3].Trim().PadLeft(2, pad);
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            // TestVersion();
            TestResourceVersion();

            if (String.IsNullOrEmpty(txtSearchIn.Text) || String.IsNullOrWhiteSpace(txtSearchIn.Text))
                MessageBox.Show("\"Search In\" text is empty", _applicationName);
            else if (String.IsNullOrEmpty(txtPattern.Text) || String.IsNullOrWhiteSpace(txtPattern.Text))
                MessageBox.Show("\"Pattern\" text is empty", _applicationName);
            else
            {
                // Perform Regular Expression Processing

                txtDisplay.Clear();

                var matches = Regex.Matches(txtSearchIn.Text, txtPattern.Text,RegexOptions.IgnoreCase) ;

                if (matches.Count == 0)
                    txtDisplay.Text = "No matches found";
                else
                {

                    foreach (Match match in matches)
                        txtDisplay.Text += String.Format( "Found '{0}' at position {1}", match.Value, match.Index)+Environment.NewLine;

                } // foreach

                ReplaceTableNames();
                ReplaceItems();

            } // Perform Regular Expression Processing

        }
    }
}
