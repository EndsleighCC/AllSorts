using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Ionic.Zip;

namespace TestDotNetZip
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void AddFile( ZipFile zip , string FileName )
        {
            txtDisplay.Text += "Adding \"" + FileName + "\"" + Environment.NewLine ;
            // Add with full path
            zip.AddFile(FileName);
            // Add to the root of the Archive
            // zip.AddFile(FileName,"");
        }

        List<string> RecurseDirectory(List<string> filenameList , string currentDirectory)
        {
            var directories = Directory.EnumerateDirectories(currentDirectory);

            foreach (var directory in directories)
            {
                filenameList = RecurseDirectory(filenameList, Path.Combine(currentDirectory, directory));
            }

            foreach (var filename in Directory.EnumerateFiles(currentDirectory))
            {
                filenameList.Add(Path.Combine( currentDirectory , filename ) );
            }

            return filenameList;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            txtDisplay.Clear();

            if ( String.IsNullOrEmpty( txtZipName.Text ) || String.IsNullOrWhiteSpace( txtZipName.Text) )
                MessageBox.Show( "Zip Name is empty" , "DotNetZip Test Error" ) ;
            else
            {
                txtDisplay.Text = "Beginning DotNetZip ..." + Environment.NewLine;

                using (ZipFile zip = new ZipFile())
                {

                    List<string> filenameList = new List<string>() ;

                    filenameList = RecurseDirectory( filenameList , txtFolderName.Text ) ;

                    foreach (string filename in filenameList)
                    {
                        AddFile( zip , Path.Combine( txtFolderName.Text , filename ) ) ;
                    }

                    zip.Comment = String.Format("Added by CC's DotNetZip test {0}", DateTime.Now);

                    string ZipFileName = txtZipName.Text;

                    if (String.Compare(ZipFileName.Substring(ZipFileName.Length - _ZipFileExtension.Length), _ZipFileExtension, true) != 0)
                        ZipFileName += _ZipFileExtension;

                    txtDisplay.Text += "Saving DotNetZip \"" + ZipFileName + "\"" + Environment.NewLine;
                    zip.Save(ZipFileName);
                }

                txtDisplay.Text += "Ending DotNetZip" + Environment.NewLine;
            }
        }

        const string _ZipFileExtension = ".zip" ;

        private void btnDirectory_Click(object sender, EventArgs e)
        {
            fldbFolderSelect.ShowDialog();
            txtFolderName.Text = fldbFolderSelect.SelectedPath;
        }

        private void btnZipNameDirectory_Click(object sender, EventArgs e)
        {
            fldbZipFileName.ShowDialog();
            txtZipName.Text = fldbZipFileName.SelectedPath + "\\" ;
        }
    }
}
