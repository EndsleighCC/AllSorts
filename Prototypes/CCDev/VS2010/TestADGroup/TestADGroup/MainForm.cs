using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TestADGroup
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Update the Combo Box asynchronously because the AD search takes a little while
            ThreadPool.QueueUserWorkItem(new WaitCallback(UpdateComboThread),this);
        }

        static void UpdateComboThread( Object obj )
        {
            MainForm mainForm = (MainForm) obj;
            UserAccessDetail userAccessDetail = new UserAccessDetail();
            Collections.CaseIgnoringSortedSetType networkGroupSet = userAccessDetail.NetworkGroupSet();
            mainForm.UpdateCombo(networkGroupSet);
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(comboNetworkGroupList.Text))
            {
                UserAccessDetail userAccessDetailTest = new UserAccessDetail();
                if (userAccessDetailTest.UserWithTechRefIsNetworkGroupMember("CHCO", "System builders"))
                    Debug.WriteLine( "Tech Ref is in Group");
                else
                    Debug.WriteLine( "Tech Ref is NOT in Group");

                lstADGroupMembers.Items.Clear();

                UserAccessDetail userAccessDetail = new UserAccessDetail();
                Collections.CaseIgnoringSortedSetType groupUserCollection = userAccessDetail.NetworkUsersForGroupName(comboNetworkGroupList.Text);

                foreach ( string userName in groupUserCollection)
                {
                    lstADGroupMembers.Items.Add(userName);
                }
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            string clipboardData = "";
            foreach ( string userName in lstADGroupMembers.Items)
            {
                clipboardData += userName + Environment.NewLine;
            }
            SetClipboardData(clipboardData);
        }

        private string GetClipboardData()
        {
            string clipboardData = "";

            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Clipboard.GetText(TextDataFormat.Text));
                MemoryStream ms = new MemoryStream(byteArray);

                using (BinaryReader binaryReader = new BinaryReader(ms))
                {
                    try
                    {
                        for (; ; ) clipboardData += binaryReader.ReadChar();
                    }
                    catch (Exception eek)
                    {
                    }
                }

            }

            return clipboardData;
        }

        private void SetClipboardData(string dataForClipboard)
        {
            Clipboard.SetText(dataForClipboard);
        }

        delegate void UpdateComboDelegate( Collections.CaseIgnoringSortedSetType groupNameSet );

        private void UpdateCombo(Collections.CaseIgnoringSortedSetType groupNameSet)
        {
            // Check whether an Invoke is required by checking any control for an Invoke being Required
            if (!comboNetworkGroupList.InvokeRequired)
            {
                foreach (string networkGroupName in groupNameSet)
                {
                    comboNetworkGroupList.Items.Add(networkGroupName);
                }

                comboNetworkGroupList.Text = "";
                comboNetworkGroupList.Enabled = true;
                comboNetworkGroupList.Select();
            }
            else
            {
                UpdateComboDelegate updateComboDelegate = new UpdateComboDelegate(UpdateCombo);
                this.Invoke(updateComboDelegate, groupNameSet);
            }
        }


    }
}
