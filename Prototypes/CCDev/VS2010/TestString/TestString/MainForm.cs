using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TestString
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        const string _tableAllowedCharacters = " 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz=+-*/<>()[]{}@#~\\|';:.?!`¬¦£$%^&_";
        const int _tableValueMaximumLength = 15;

        /// <summary>
        /// Function to generate a new string from the supplied string with the new string
        /// containing only those characters that are acceptable as Table contents
        /// </summary>
        /// <param name="stringToFilter">The string whose characters are to be scanned for unacceptable content</param>
        /// <returns>A string containing only characters that are acceptable as table data</returns>
        public static string FilterData(string stringToFilter)
        {
            StringBuilder stringBuilder = new StringBuilder("", _tableValueMaximumLength);

            int charIndex;
            for (charIndex = 0; (charIndex < stringToFilter.Length) && (stringBuilder.Length < _tableValueMaximumLength); ++charIndex)
            {
                char character = stringToFilter[charIndex];
                if (_tableAllowedCharacters.IndexOf(character) != -1)
                    // This character is acceptable
                    stringBuilder.Append(character);
            }

            // Remove leading and trailing spaces whilst allowing the internal contents to contain spaces
            return stringBuilder.ToString().Trim();
        }

        private void TestSplit()
        {
            string[] requesterApplicationVersionParts = "4.10.01.01".Split('.');

            if (requesterApplicationVersionParts.Length == 4)
            {
                int requesterApplicationSystemVersion = System.Convert.ToInt32(requesterApplicationVersionParts[0]);
                int requesterApplicationMajorVersion = System.Convert.ToInt32(requesterApplicationVersionParts[1]);
                int requesterApplicationMinorVersion = System.Convert.ToInt32(requesterApplicationVersionParts[2]);
                int requesterApplicationPatchVersion = System.Convert.ToInt32(requesterApplicationVersionParts[3]);
            }

        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            TestSplit();

            txtResult.Text = FilterData(txtValue.Text);

            if ( Clipboard.ContainsText(TextDataFormat.Text))
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Clipboard.GetText(TextDataFormat.Text));
                MemoryStream ms = new MemoryStream(byteArray);

                string clipboardData = "";
                using (BinaryReader binaryReader = new BinaryReader(ms))
                {
                    try
                    {
                        for(;;) clipboardData += binaryReader.ReadChar();
                    }
                    catch (Exception eek)
                    {
                    }
                }

                MemoryStream memoryStream = new MemoryStream();
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream,Encoding.Unicode))
                {
                    foreach (char thisChar in clipboardData)
                    {
                        binaryWriter.Write(thisChar);
                    }

                    byte [] byteArrayWrite = memoryStream.ToArray();

                    Encoding encoding = new UnicodeEncoding(false,true);

                    string theString = encoding.GetString(byteArrayWrite);
                }

                txtMultiline.Text = clipboardData;
            }
        }
    }
}
