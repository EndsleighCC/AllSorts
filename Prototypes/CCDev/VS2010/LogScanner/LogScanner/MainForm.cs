using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;

namespace LogScanner
{
    public partial class MainForm : Form
    {

        private enum ApplicationPageEnum
        {
            FileContents,
            FileFiltered,
            FilePairs
        } ;

        private string _logFileName = null;

        private string _logFileLoadedName ;

        private string _logFileStartOffsetPercentageText;
        private int _logFileWindowLengthByteCount;

        private const int _minimumDisplaySize = 1;
        private const int _maximumDisplaySize = 50000000;
        private const string _fileOffsetFromEnd = "E";

        public MainForm()
        {
            InitializeComponent();

            openFileDialog.FileName = @"C:\Program Files\insurance.net\EIS\Log\EISDBG.LOG";
            txtFileName.Text = openFileDialog.FileName;
            _logFileName = openFileDialog.FileName;
            btnExecute.Text = "Open";
        }

        private ApplicationPageEnum CurrentApplicationPage
        {
            get
            {
                ApplicationPageEnum applicationPage = ApplicationPageEnum.FileContents;

                if (tabDisplayType.SelectedTab == tabFileContents)
                {
                    applicationPage = ApplicationPageEnum.FileContents;
                }
                else if (tabDisplayType.SelectedTab == tabContentsFilter)
                {
                    applicationPage = ApplicationPageEnum.FileFiltered;
                }
                else if (tabDisplayType.SelectedTab == tabContentsPairs)
                {
                    applicationPage = ApplicationPageEnum.FilePairs;
                }

                return applicationPage;
            }
        }

        private bool LogFileLoaded
        {
            get
            {
                return _logFileLoadedName != null;
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
            if (!File.Exists(openFileDialog.FileName))
                MessageBox.Show("File \"" + openFileDialog.FileName + "\" does not exist", "Log Scanner");
            else
            {
                txtFileName.Text = openFileDialog.FileName;
                _logFileName = openFileDialog.FileName;
            }
        }

        private void GoToStartOfNextLine(StreamReader inputFileStream)
        {
            string inputLine = inputFileStream.ReadLine();
        }

        private void LoadLogFile(string fileName)
        {
            if (!File.Exists(fileName))
                MessageBox.Show("File \"" + fileName + "\" does not exist", "Log Scanner");
            else
            {
                using (StreamReader inputFileStream = new StreamReader(fileName))
                {

                    if (!String.IsNullOrEmpty(txtFileWindowLengthBytes.Text))
                    {
                        int logFileWindowLengthByteCount = System.Convert.ToInt32(txtFileWindowLengthBytes.Text);
                        if ((logFileWindowLengthByteCount < _minimumDisplaySize) || (logFileWindowLengthByteCount > _maximumDisplaySize))
                        {
                            MessageBox.Show(
                                String.Format("File Window {0} cannot be outside of the range {1} to {2}",
                                              logFileWindowLengthByteCount, _minimumDisplaySize, _maximumDisplaySize), "Log Scanner");
                        }
                        else
                        {
                            _logFileWindowLengthByteCount = logFileWindowLengthByteCount;
                        }
                    }

                    int maximumWindowLengthByteCount = (_logFileWindowLengthByteCount == 0) ? int.MaxValue : _logFileWindowLengthByteCount;

                    if ( !String.IsNullOrEmpty(txtOffsetPercentage.Text) )
                    {
                        // Look for "offset from end of file" indicator

                        long fileLength = (new FileInfo(fileName)).Length;

                        double fileStartOffsetPercentage = 0.0;

                        if (String.Compare(txtOffsetPercentage.Text, _fileOffsetFromEnd, true) == 0)
                            fileStartOffsetPercentage = 100.0;

                        if ((fileStartOffsetPercentage < 0.0) || (fileStartOffsetPercentage > 100.0))
                        {
                            MessageBox.Show(
                                String.Format("File Offset {0} cannot be outside of the range 0 to 100. Defaulting to 0.",
                                              fileStartOffsetPercentage), "Log Scanner");
                        }
                        else
                        {
                            // Reposition the file

                            long logFileStartPosition = 0 ;

                            // Remember the specified file offset
                            _logFileStartOffsetPercentageText = txtOffsetPercentage.Text;

                            if (String.Compare(txtOffsetPercentage.Text, _fileOffsetFromEnd, true) == 0)
                            {
                                // Offset is relative to the end of the file

                                if (maximumWindowLengthByteCount == int.MaxValue)
                                    // Display from the beginning of the file
                                    logFileStartPosition = 0;
                                else
                                    // Display from the specified location
                                    logFileStartPosition = fileLength - maximumWindowLengthByteCount;

                            } // Offset is relative to the end of the file
                            else
                            {
                                // Offset is relative to the start of the file

                                fileStartOffsetPercentage = System.Convert.ToDouble(txtOffsetPercentage.Text);

                                logFileStartPosition = (long)(fileLength / 100.0 * fileStartOffsetPercentage);

                            } // Offset is relative to the start of the file

                            inputFileStream.BaseStream.Position = logFileStartPosition;
                            if (logFileStartPosition != 0 )
                                GoToStartOfNextLine(inputFileStream);
                        }
                    }

                    // Where this loop starts to read from the file will depend upon whether the above code
                    // has moved the file pointer from the beginning of the file
                    List<string> inputFileLineList = new List<string>();
                    try
                    {
                        string inputLine = inputFileStream.ReadLine();
                        while (inputLine != null)
                        {
                            inputFileLineList.Add(inputLine);
                            inputLine = inputFileStream.ReadLine();
                        }
                    }
                    catch (EndOfStreamException eek)
                    {
                    }
                    txtFileContents.Clear();
                    string[] inputFileLines = inputFileLineList.ToArray();
                    txtFileContents.Lines = inputFileLines;

                    txtDisplayLineCount.Text = inputFileLineList.Count.ToString();

                    // Empty any selections
                    txtSelect.Clear();

                    _logFileLoadedName = fileName;
                }
            }
        }

        private void FilterLogFile( string pattern )
        {
            List<string> matchingLineList = new List<string>();
            foreach ( string line in txtFileContents.Lines)
            {
                MatchCollection matches = Regex.Matches(line,
                                                        pattern,
                                                        RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    matchingLineList.Add( line );
                }
            }

            string[] matchingLinesStringArray = matchingLineList.ToArray();
            txtSelect.Lines = matchingLinesStringArray;
        }

        private bool FileViewHasChanged()
        {
            bool fileViewHasChanged = false;

            // Look for file offset changes
            if ( String.IsNullOrEmpty(_logFileStartOffsetPercentageText) )
            {
                // A file offset was not previously specified
                if (!String.IsNullOrEmpty(txtOffsetPercentage.Text))
                {
                    // The File Offset is now specified
                    fileViewHasChanged = true;
                }
            }
            else
            {
                // A file offset was previously specified so decide whether it has changed
                if (String.IsNullOrEmpty(txtOffsetPercentage.Text))
                {
                    // The File Offset is now empty
                    fileViewHasChanged = true;
                }
                else if ( String.Compare(txtOffsetPercentage.Text,_logFileStartOffsetPercentageText,true) == 0 )
                {
                    // The File Offset has changed
                    fileViewHasChanged = true;
                }
            }

            // Look for file window changes
            if ( ! fileViewHasChanged )
            {
                if  ( _logFileWindowLengthByteCount == 0 )
                {
                    // A log file window length was not previously specified
                    if ( !String.IsNullOrEmpty(txtFileWindowLengthBytes.Text))
                    {
                        // A log file window is now specified
                        fileViewHasChanged = true;
                    }
                }
                else if ( _logFileWindowLengthByteCount > 0 )
                {
                    // A log file window length was previously specified
                    if (String.IsNullOrEmpty(txtFileWindowLengthBytes.Text))
                    {
                        // The file window length is now empty
                        fileViewHasChanged = true;
                    }
                    else if ( System.Convert.ToInt32(txtFileWindowLengthBytes.Text) != _logFileWindowLengthByteCount )
                    {
                        // The file window length has changed
                        fileViewHasChanged = true;
                    }
                }
            }

            return fileViewHasChanged;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if ( _logFileName != null )
            {
                // Load or re-load file
                if ((!LogFileLoaded) || (String.Compare(_logFileLoadedName, _logFileName, true) != 0))
                    LoadLogFile(_logFileName);
                // Logfile must be laoded
                else if (String.Compare(_logFileLoadedName, _logFileName, true) != 0)
                    LoadLogFile(_logFileName);
                else if ( FileViewHasChanged())
                    // File view has changed so reload
                    LoadLogFile(_logFileName);

                SetDisplayState();

                ApplicationPageEnum applicationPage = CurrentApplicationPage;

                switch (applicationPage)
                {
                    default :
                    case ApplicationPageEnum.FileContents :
                        // Nothing to do as it was all done above
                        break;
                    case ApplicationPageEnum.FileFiltered :
                        if (String.IsNullOrEmpty(txtPattern.Text) || String.IsNullOrWhiteSpace(txtPattern.Text))
                            MessageBox.Show("Search pattern is empty", "Log Scanner");
                        else
                            FilterLogFile( txtPattern.Text );
                        break;
                    case ApplicationPageEnum.FilePairs :
                        break;
                }
            }
        }

        private void SetDisplayState()
        {
            if (tabDisplayType.SelectedTab == tabFileContents)
            {
                SetContentsDisplayState();
            }
            else if (tabDisplayType.SelectedTab == tabContentsFilter)
            {
                SetContentsFilterDisplayState();
            }
            else if (tabDisplayType.SelectedTab == tabContentsPairs)
            {
                SetContentsPairDisplayState();
            }
        }

        private void SetContentsDisplayState()
        {
            btnExecute.Text = "Open";
        }

        private void SetContentsFilterDisplayState()
        {
            if (LogFileLoaded)
            {
                btnExecute.Text = "Filter";
            }
        }

        private void SetContentsPairDisplayState()
        {
            if (LogFileLoaded)
            {
                btnExecute.Text = "Filter Pairs";
            }
        }

        private void tabDisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplicationPageEnum currentApplicationPage = CurrentApplicationPage;
            switch (currentApplicationPage)
            {
                default:
                case ApplicationPageEnum.FileContents :
                    SetContentsDisplayState();
                    break;
                case ApplicationPageEnum.FileFiltered :
                    SetContentsFilterDisplayState();
                    break;
                case ApplicationPageEnum.FilePairs :
                    SetContentsPairDisplayState();
                    break;
            }
        }

        delegate void WriteContentsTextDelegate(string textLine);

        private void WriteContentsText(string textLine)
        {
            // Check whether an Invoke is required by checking any control for an Invoke being Required
            if (!txtFileContents.InvokeRequired)
            {
                txtFileContents.Text += textLine + Environment.NewLine;
            }
            else
            {
                WriteContentsTextDelegate writeContentsTextDelegate = new WriteContentsTextDelegate(WriteContentsText);
                this.Invoke(writeContentsTextDelegate, textLine);
            }
        }

        private void btnRemovePairs_Click(object sender, EventArgs e)
        {
            if ( String.IsNullOrEmpty( txtPairPattern.Text ) || String.IsNullOrWhiteSpace( txtPairPattern.Text ) )
                MessageBox.Show("Pair pattern is empty", "Log Scanner");
            else
            {
                // Pair Pattern supplied

                List<string> linesWithPairsRemoved = txtSelect.Lines.ToList();

                for (int lineIndexFirst = 0; lineIndexFirst < linesWithPairsRemoved.Count ; ++lineIndexFirst)
                {
                    string lineFirst = linesWithPairsRemoved[lineIndexFirst];
                    // Find the first Line that matches
                    MatchCollection matches = Regex.Matches(lineFirst,
                                                            txtPairPattern.Text,
                                                            RegexOptions.IgnoreCase);
                    if ( matches.Count > 0 )
                    {
                        // Found the first of a pair

                        string dataToMatch = lineFirst.Substring(matches[0].Index, matches[0].Length);

                        const int indexNotFound = -1;
                        int indexSecondFound = indexNotFound;

                        for (int lineIndexSecond = lineIndexFirst + 1;
                                (indexSecondFound == indexNotFound) && (lineIndexSecond < linesWithPairsRemoved.Count);
                                ++lineIndexSecond)
                        {
                            string lineSecond = linesWithPairsRemoved[lineIndexSecond];

                            if (lineSecond.IndexOf(dataToMatch) != indexNotFound)
                            {
                                indexSecondFound = lineIndexSecond;
                            }
                        }

                        if ( (indexSecondFound != indexNotFound) )
                        {
                            // Remove the first and last of the pair

                            linesWithPairsRemoved.RemoveAt(lineIndexFirst);
                            // Index will have reduced by one
                            linesWithPairsRemoved.RemoveAt(indexSecondFound - 1);

                            // Decement the loop counter so that the loop increment returns to the correct value
                            lineIndexFirst -= 1;

                        } // Remove the first and last of the pair

                    } // Found the first of a pair

                } // for

                txtSelect.Lines = linesWithPairsRemoved.ToArray();

            } // Pair Pattern supplied

        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            _logFileName = txtFileName.Text;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtSearchText.Text))
            {
                List<string> linesToSearch = txtFileContents.Lines.ToList();

                int scannedCharacterCount = 0;

                bool found = false;
                int startLine = txtFileContents.SelectionStart == 0
                                    ? 0
                                    : txtFileContents.GetLineFromCharIndex(txtFileContents.SelectionStart) + 1;
                // Point to the start of the start line
                scannedCharacterCount = txtFileContents.GetFirstCharIndexFromLine(startLine);
                for (int lineIndex = startLine; (!found) && (lineIndex < linesToSearch.Count); ++lineIndex)
                {
                    string line = linesToSearch[lineIndex];
                    // Find the first Line that matches
                    MatchCollection matches = Regex.Matches(line,
                                                            txtSearchText.Text,
                                                            RegexOptions.IgnoreCase);
                    if (matches.Count == 0)
                        scannedCharacterCount += line.Length + Environment.NewLine.Length ;
                    else
                    {
                        // Found the first line of matching text

                        found = true;

                        txtFileContents.Select(scannedCharacterCount, line.Length);

                        txtFileContents.ScrollToCaret();

                        string selectedText = txtFileContents.SelectedText;

                    } // Found the first line of matching text
                }

                if (!found)
                    MessageBox.Show("String \"" + txtSearchText.Text + "\" was not found", "Log Scanner");
            }
        }
    }
}
