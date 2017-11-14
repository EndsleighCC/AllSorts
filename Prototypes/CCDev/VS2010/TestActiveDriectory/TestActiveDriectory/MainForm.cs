using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.Collections;

namespace TestActiveDriectory
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            InitialiseDefaultNamingContext();
        }

        string SimpleObjectName(string objectName)
        {
            string simpleObjectName = null ;

            int posEqual = objectName.LastIndexOf("=");
            if (posEqual == -1)
                simpleObjectName = objectName;
            else
                simpleObjectName = objectName.Substring(posEqual+1);
            return simpleObjectName;
        }

        void LoadComputerNames()
        {
            // DirectoryEntry localComputer = new DirectoryEntry("LDAP://" + Environment.MachineName + ",Computer" );

            try
            {
                DirectoryEntry domainEntry = new DirectoryEntry("GC://DC=eis,DC=endsleigh,DC=co,DC=uk");

                DirectorySearcher domainSearcher = new DirectorySearcher(domainEntry);

                // domainSearcher.Filter = "(ObjectCategory=computer)";
                string objectFilter = "(cn=" + txtFilter.Text + ")";

                domainSearcher.Filter = objectFilter;

                SearchResultCollection searchResultCollection = domainSearcher.FindAll() ;

                foreach (SearchResult domainSearchResult in searchResultCollection)
                {
                    string computerName = SimpleObjectName(domainSearchResult.GetDirectoryEntry().Name);
                    chklbComputers.Items.Add(computerName, CheckState.Unchecked);
                    Application.DoEvents();
                }

                lblObjectCount.Text = "Object count is " + System.Convert.ToString(chklbComputers.Items.Count);

            }
            catch (Exception eek)
            {
                MessageBox.Show(eek.ToString() , "Active Directory Tester");
            }

        }

        private void chklbComputers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string changedComputerName = chklbComputers.SelectedItem.ToString();

            lstResults.Items.Clear();

            lstResults.Items.Add(changedComputerName);

            SearchResultCollection computerSearchResultCollection = SearchResultCollectionFindAll("(&(cn=" + changedComputerName + ")(ObjectClass=computer))");
            if (computerSearchResultCollection != null)
            {
                // The search returned at least one result

                foreach (SearchResult computerSearchResult in computerSearchResultCollection)
                {
                    DirectoryEntry computerDirectoryEntry = computerSearchResult.GetDirectoryEntry();

                    lstResults.Items.Add("Name:" + computerDirectoryEntry.Name);
                    lstResults.Items.Add("Path:" + computerDirectoryEntry.Path);
                    lstResults.Items.Add("SchemaClassName:" + computerDirectoryEntry.SchemaClassName);
                    lstResults.Items.Add("Parent:" + computerDirectoryEntry.Parent.Name);
                    lstResults.Items.Add("Parent:" + computerDirectoryEntry.Parent.Path);

                    ShowAllProperties(computerSearchResult);

                    lstResults.Items.Add("");

                    ShowAllProperties(computerDirectoryEntry);

                    lstResults.Items.Add("");

                    ShowConfiguration(computerDirectoryEntry);

                    lstResults.Items.Add("");

                    lstResults.Items.Add("Children of Object \"" + computerSearchResult.Path + "\"");
                    bool childrenExist = false;
                    foreach (DirectoryEntry child in computerDirectoryEntry.Children)
                    {
                        lstResults.Items.Add("    " + child.Name);
                    }
                    if (!childrenExist)
                        lstResults.Items.Add("    None");

                    // CreateShareEntry( computerDirectoryEntry.Parent.Path , "CrapShare" , @"\\"+SimpleObjectName( computerDirectoryEntry.Name ) +@"\CrapShare" , "A crap Share" ) ;

                    // LookupChildren(computerDirectoryEntry.Parent.Path, "none");

                    // CaseIgnoringSortedSetType volumes = ComputerSharedVolumes(computerDirectoryEntry.Path, true);

                } // foreach

                lstResults.Items.Add("");

                ShowAllVolumes();

                lstResults.Items.Add("");

                ShowAllStorages();

                lstResults.Items.Add("");

                ShowAllConnectionPoints();

                lstResults.Items.Add("");

                ShowNetworkGroupsForUser(Environment.UserName);

                lstResults.Items.Add("");

                ShowNetworkGroupsForUser("corc1");

                lstResults.Items.Add("");

                ShowNetworkGroupsForUser("standard.finance");

                lstResults.Items.Add("");

                ShowNetworkUsersForGroup("System builders");

                lstResults.Items.Add("");

                ShowNetworkUsersForGroup("TMS Developers");

                lstResults.Items.Add("");

                ShowNetworkUsersForGroup("TMS Viewers");

                lstResults.Items.Add("");

                ShowNetworkUsersForGroup("TMS Viewers");

                lstResults.Items.Add("");

                ShowNetworkUsersForGroup("IT Release Handlers");

                lstResults.Items.Add("");

                ShowNetworkUsersForGroup("Discount-Unlimited");

                lstResults.Items.Add("");

                ShowNetworkUsersForGroup("TMS Development Administrators");

                lstResults.Items.Add("");

                string thisDistName = DistinguishedNameFromSAMAccountName(Environment.UserName);
                lstResults.Items.Add("Distinguished Name of \"" + Environment.UserName + "\" = \"" + thisDistName + "\"");
                string thisCommonName = CommonNameFromDistinguishedName(thisDistName);
                lstResults.Items.Add("Common Name of \"" + Environment.UserName + "\" = \"" + thisCommonName + "\"");

                lstResults.Items.Add("Distinguished Name of \"" + "SHNSDW26" + "\" = \"" + DistinguishedNameFromCommonName("SHNSDW26") + "\"");

                lstResults.Items.Add("");

                string selectedUserName = Environment.UserName;
                string selectedGroupName = "System builders";
                if (UserIsNetworkGroupMember(selectedUserName, selectedGroupName))
                    lstResults.Items.Add(selectedUserName + " is a member of \"" + selectedGroupName + "\"");
                else
                    lstResults.Items.Add(selectedUserName + " is a NOT member of \"" + selectedGroupName + "\"");

                selectedUserName = "crap";
                if (UserIsNetworkGroupMember(selectedUserName, selectedGroupName))
                    lstResults.Items.Add(selectedUserName + " is a member of \"" + selectedGroupName + "\"");
                else
                    lstResults.Items.Add(selectedUserName + " is a NOT member of \"" + selectedGroupName + "\"");

            } // The search returned at least one result

            SearchResultCollection userSearchResultCollection = SearchResultCollectionFindAll("(&(SAMAccountName=jorp1)(ObjectClass=user))");
            if (userSearchResultCollection != null)
            {
                // The search returned at least one result

                foreach (SearchResult userSearchResult in userSearchResultCollection)
                {
                    DirectoryEntry userDirectoryEntry = userSearchResult.GetDirectoryEntry();

                    lstResults.Items.Add("");

                    ShowAllProperties(userDirectoryEntry);

                } // foreach

            } // The search returned at least one result

        }

        private string _DefaultNamingContext = null;

        private void InitialiseDefaultNamingContext()
        {
            DirectoryEntry rootEntry = new DirectoryEntry(LdapObjectPath("rootDSE"));
            _DefaultNamingContext = (string)rootEntry.Properties["defaultNamingContext"].Value;
        }

        private const string LdapPrefix = "LDAP://";

        private const string CommonNamePrefix = "CN=";

        private string LdapDefaultNamingContext()
        {
            return LdapPrefix + _DefaultNamingContext;
        }

        private string LdapObjectPath(string objectPath)
        {
            return LdapPrefix + objectPath;
        }

        private string DistinguishedNameFromSAMAccountName(string sAMAccountName)
        {
            string distinguishedName = null;
            SearchResult searchResult = SearchResultCollectionFindOne("(SAMAccountName=" + sAMAccountName + ")");
            if (searchResult != null)
            {
                // The search returned a result
                string ldapPath = searchResult.Path;
                distinguishedName = ldapPath.Substring(LdapPrefix.Length);
            }
            return distinguishedName;
        }

        private string DistinguishedNameFromCommonName(string commonName)
        {
            string distinguishedName = null ;
            SearchResult searchResult = SearchResultCollectionFindOne("(cn=" + commonName + ")");
            if (searchResult != null)
            {
                // The search returned a result
                string ldapPath = searchResult.Path;
                distinguishedName = ldapPath.Substring(LdapPrefix.Length);
            }
            return distinguishedName ;
        }

        private string CommonNameFromDistinguishedName(string distinguishedName)
        {
            string commonName = null ;
            int commonNameIndex = distinguishedName.IndexOf(CommonNamePrefix);
            if (commonNameIndex == -1)
                commonName = distinguishedName;
            else
            {
                // Parse out the Common Name

                // Get a copy of the tail of the string without the prefix
                string distinguishedNameTail = distinguishedName.Substring( commonNameIndex+CommonNamePrefix.Length ) ;

                // Look for the end of the Common Name
                int commaIndex = distinguishedNameTail.IndexOf(",");
                if (commaIndex == -1)
                    commonName = distinguishedNameTail;
                else
                    commonName = distinguishedNameTail.Substring(0, commaIndex);

            } // Parse out the Common Name

            return commonName;
        }

        private DirectorySearcher NewDirectorySearcher(string searchFilter)
        {
            DirectoryEntry defaultEntry = new DirectoryEntry(LdapDefaultNamingContext());
            DirectorySearcher objectSearcher = new DirectorySearcher(defaultEntry);
            objectSearcher.Filter = searchFilter;
            return objectSearcher;
        }

        private SearchResultCollection SearchResultCollectionFindAll(string searchFilter)
        {
            DirectorySearcher directorySearcher = NewDirectorySearcher(searchFilter);
            return directorySearcher.FindAll();
        }

        private SearchResult SearchResultCollectionFindOne(string searchFilter)
        {
            DirectorySearcher directorySearcher = NewDirectorySearcher(searchFilter);
            return directorySearcher.FindOne();
        }

        private SearchResultCollection SearchResultCollectionFindAllComputer(string searchFilter)
        {
            return SearchResultCollectionFindAll(searchFilter);
        }

        private void ShowConfiguration(DirectoryEntry directoryEntry)
        {
            lstResults.Items.Add("Configuration for \"" + directoryEntry.Name + "\"");
            DirectoryEntryConfiguration directoryEntryConfiguration = directoryEntry.Options;
            lstResults.Items.Add("    PageSize=" + directoryEntryConfiguration.PageSize );
            lstResults.Items.Add("    PasswordEncoding=" + directoryEntryConfiguration.PasswordEncoding);
            lstResults.Items.Add("    PasswordPort=" + directoryEntryConfiguration.PasswordPort);
            lstResults.Items.Add("    Referral=" + directoryEntryConfiguration.Referral);
            lstResults.Items.Add("    SecurityMasks=" + directoryEntryConfiguration.SecurityMasks);
        }

        private void ShowAllProperties(SearchResult searchResult)
        {
            lstResults.Items.Add("Search Result Properties for Object \"" + searchResult.Path + "\" of class \"" + searchResult.GetDirectoryEntry().SchemaClassName + "\"");
            foreach (string propertyName in searchResult.Properties.PropertyNames)
            {
                lstResults.Items.Add("    " + propertyName);
                foreach (Object property in searchResult.Properties[propertyName])
                {
                    lstResults.Items.Add("        " + property.ToString() + " of type " + property.GetType());
                }

            }
        }

        private void ShowAllProperties(DirectoryEntry directoryEntry)
        {
            lstResults.Items.Add("Directory Entry Properties for Object \"" + directoryEntry.Name + "\" of class \"" + directoryEntry.SchemaClassName + "\"");

            foreach (string propertyName in directoryEntry.Properties.PropertyNames)
            {
                lstResults.Items.Add("    " + propertyName + "=" + directoryEntry.Properties[propertyName]);

                CaseIgnoringSortedSetType propertyValueSet = new CaseIgnoringSortedSetType();

                propertyValueSet = AttributeValuesMultiString(propertyName, directoryEntry, propertyValueSet);

                foreach (string propertyValue in propertyValueSet)
                {
                    string valueString = "            " + propertyName + "=" + propertyValue;
                    lstResults.Items.Add(valueString);
                }
            }
        }

        private void ShowNetworkGroupsForUser(string userName)
        {
            SearchResultCollection userSearchResultCollection = SearchResultCollectionFindAll("(&(SAMAccountName=" + userName + ")(ObjectClass=user))");
            if (userSearchResultCollection != null)
            {
                // The search returned at least one result

                foreach (SearchResult userSearchResult in userSearchResultCollection)
                {
                    DirectoryEntry userDirectoryEntry = userSearchResult.GetDirectoryEntry();

                    lstResults.Items.Add("Network Groups for User \"" + userDirectoryEntry.Name + "\" of class \"" + userDirectoryEntry.SchemaClassName + "\"");

                    CaseIgnoringSortedSetType userGroups = NetworkGroupsForUser(userDirectoryEntry.Path, true);
                    foreach (string groupName in userGroups)
                    {
                        lstResults.Items.Add("    \"" + groupName + "\"");
                    }
                }

            } // The search returned at least one result
        }

        private bool UserIsNetworkGroupMember(string userName , string groupName )
        {
            bool userIsNetworkGroupMember = false;

            try
            {
                // Get thje Group's Distinguished Name
                string groupDistinguishedName = DistinguishedNameFromCommonName(groupName);

                SearchResult userSearchResult = SearchResultCollectionFindOne("(&(SAMAccountName=" + userName + ")(ObjectClass=user))");
                if (userSearchResult != null)
                {
                    // Search returned a result
                    CaseIgnoringSortedSetType userGroupCollection = NetworkGroupsForUser(userSearchResult.GetDirectoryEntry().Path, true);
                    userIsNetworkGroupMember = userGroupCollection.Contains(groupDistinguishedName);
                } // Search returned a result
            }
            catch (Exception eek)
            {
            }

            return userIsNetworkGroupMember;
        }

        private void ShowNetworkUsersForGroup(string groupName)
        {
            SearchResultCollection groupSearchResultCollection = SearchResultCollectionFindAll("(&(cn=" + groupName + ")(ObjectClass=group))");
            if (groupSearchResultCollection != null)
            {
                // The search returned at least one result

                foreach (SearchResult groupSearchResult in groupSearchResultCollection)
                {
                    DirectoryEntry groupDirectoryEntry = groupSearchResult.GetDirectoryEntry();

                    lstResults.Items.Add("Network Users for Group \"" + groupDirectoryEntry.Name + "\" of class \"" + groupDirectoryEntry.SchemaClassName + "\"");

                    CaseIgnoringSortedSetType groupUsers = NetworkUsersInGroup(groupDirectoryEntry.Path, true);
                    foreach (string groupUserName in groupUsers)
                    {
                        lstResults.Items.Add("    \"" + groupUserName + "\"");
                    }
                }
            }
        }

        private void ShowAllVolumes()
        {
            SearchResultCollection volumeSearchResultCollection = SearchResultCollectionFindAll("(ObjectClass=volume)");
            if (volumeSearchResultCollection != null)
            {
                // The search returned at least one result

                foreach (SearchResult volumeSearchResult in volumeSearchResultCollection)
                {
                    DirectoryEntry volumeDirectoryEntry = volumeSearchResult.GetDirectoryEntry();

                    lstResults.Items.Add("Volume \"" + volumeDirectoryEntry.Name + "\" of class \"" + volumeDirectoryEntry.SchemaClassName + "\"");

                    ShowAllProperties(volumeDirectoryEntry);

                }
            } // The search returned at least one result
        }

        private void ShowAllStorages()
        {
            SearchResultCollection storageSearchResultCollection = SearchResultCollectionFindAll("(ObjectClass=storage)");
            if (storageSearchResultCollection != null)
            {
                // The search returned at least one result

                foreach (SearchResult storageSearchResult in storageSearchResultCollection)
                {
                    DirectoryEntry storageDirectoryEntry = storageSearchResult.GetDirectoryEntry();

                    lstResults.Items.Add("Storage \"" + storageDirectoryEntry.Name + "\" of class \"" + storageDirectoryEntry.SchemaClassName + "\"");

                    ShowAllProperties(storageDirectoryEntry);

                }
            } // The search returned at least one result
        }

        private void ShowAllConnectionPoints()
        {
            SearchResultCollection connectionPointSearchResultCollection = SearchResultCollectionFindAll("(ObjectClass=connection-point)");
            if (connectionPointSearchResultCollection != null)
            {
                // // The search returned at least one result

                foreach (SearchResult connectionPointSearchResult in connectionPointSearchResultCollection)
                {
                    DirectoryEntry connectionPointDirectoryEntry = connectionPointSearchResult.GetDirectoryEntry();

                    lstResults.Items.Add("connectionPoint \"" + connectionPointDirectoryEntry.Name + "\" of class \"" + connectionPointDirectoryEntry.SchemaClassName + "\"");

                    ShowAllProperties(connectionPointDirectoryEntry);

                }
            } // The search returned at least one result
        }

        //private void BotchJob()
        //{
        //    CreateShareEntry("OU=HOME,dc=baileysoft,dc=com",
        //        "Music", @"\\192.168.2.1\Music", "mp3 Server Share");
        //    Console.ReadLine();
        //}

        //Actual Method

        public void CreateShareEntry(string ldapPath,
            string shareName, string shareUncPath, string shareDescription)
        {
            string oGUID = string.Empty;
            string connectionPrefix = ldapPath;
            DirectoryEntry directoryObject = new DirectoryEntry(connectionPrefix);
            DirectoryEntry networkShare = directoryObject.Children.Add("CN=" +
                shareName, "volume");
            networkShare.Properties["uNCName"].Value = shareUncPath;
            networkShare.Properties["Description"].Value = shareDescription;
            networkShare.CommitChanges();

            directoryObject.Close();
            networkShare.Close();
        }

        void LookupChildren(string ldapPath, string shareName)
        {
            string connectionPrefix = ldapPath;
            DirectoryEntry directoryObject = new DirectoryEntry(connectionPrefix);

            foreach (DirectoryEntry child in directoryObject.Children)
            {
                lstResults.Items.Add("Child1:"+child.Name+" of type " + child.GetType() );

                bool childrenExist = false;
                foreach (DirectoryEntry child1 in child.Children)
                {
                    childrenExist = true;
                    lstResults.Items.Add("    Child2:\""+child1.Name+"\" of type \"" + child1.GetType()+"\"" );
                }

                if ( !childrenExist )
                    lstResults.Items.Add("    No children" );

            }

        }

        private class CaseIgnoringSortedSetType : SortedSet<string>
        {
            public CaseIgnoringSortedSetType() : base(StringComparer.CurrentCultureIgnoreCase) { }
        }

        private CaseIgnoringSortedSetType NetworkGroupsForUser(string userDistinguishedName, bool recursive)
        {
            CaseIgnoringSortedSetType groupMemberships = new CaseIgnoringSortedSetType();
            return AttributeValuesMultiString("memberOf", userDistinguishedName, groupMemberships, recursive);
        }

        private CaseIgnoringSortedSetType NetworkUsersInGroup(string groupDistinguishedName, bool recursive)
        {
            CaseIgnoringSortedSetType userMemberships = new CaseIgnoringSortedSetType();
            return AttributeValuesMultiString("member", groupDistinguishedName, userMemberships, recursive);
        }

        private CaseIgnoringSortedSetType ComputerSharedVolumes(string computerDistinguishedName, bool recursive)
        {
            CaseIgnoringSortedSetType volumes = new CaseIgnoringSortedSetType();
            return AttributeValuesMultiString("volumeCount", computerDistinguishedName, volumes , true);
        }

        private CaseIgnoringSortedSetType AttributeValuesMultiString(string attributeName, string objectDistinguishedName, CaseIgnoringSortedSetType valuesCollection, bool recursive)
        {
            DirectoryEntry objectEntry = new DirectoryEntry(objectDistinguishedName);

            valuesCollection = AttributeValuesMultiString(attributeName, objectEntry, valuesCollection, recursive);

            objectEntry.Close();
            objectEntry.Dispose();

            return valuesCollection;
        }

        // Non-recursive
        private CaseIgnoringSortedSetType AttributeValuesMultiString(string attributeName, DirectoryEntry objectEntry, CaseIgnoringSortedSetType valuesCollection)
        {
            return AttributeValuesMultiString(attributeName, objectEntry, valuesCollection, false);
        }

        // Recursive
        private CaseIgnoringSortedSetType AttributeValuesMultiString(string attributeName, DirectoryEntry objectEntry, CaseIgnoringSortedSetType valuesCollection, bool recursive)
        {
            PropertyValueCollection ValueCollection = objectEntry.Properties[attributeName];
            IEnumerator enumerator = ValueCollection.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current != null)
                {
                    if (!valuesCollection.Contains(enumerator.Current.ToString()))
                    {
                        valuesCollection.Add(enumerator.Current.ToString());
                        if (recursive)
                        {
                            AttributeValuesMultiString(attributeName, LdapPrefix + enumerator.Current.ToString(), valuesCollection, true);
                        }
                    }
                }
            }
            return valuesCollection;
        }

        //public ArrayList NetworkGroups(string userDn, bool recursive)
        //{
        //    ArrayList groupMemberships = new ArrayList();
        //    return AttributeValuesMultiString( "memberOf" , userDn , groupMemberships , recursive ) ;
        //}

        //public ArrayList AttributeValuesMultiString(string attributeName, string objectDn, ArrayList valuesCollection, bool recursive)
        //{
        //    DirectoryEntry objectEntry = new DirectoryEntry(objectDn);

        //    PropertyValueCollection ValueCollection = objectEntry.Properties[attributeName];
        //    IEnumerator enumerator = ValueCollection.GetEnumerator();

        //    while (enumerator.MoveNext())
        //    {
        //        if (enumerator.Current != null)
        //        {
        //            if (!valuesCollection.Contains(enumerator.Current.ToString()))
        //            {
        //                valuesCollection.Add(enumerator.Current.ToString());
        //                if (recursive)
        //                {
        //                    AttributeValuesMultiString(attributeName, "LDAP://" + enumerator.Current.ToString(), valuesCollection, true);
        //                }
        //            }
        //        }
        //    }
        //    objectEntry.Close();
        //    objectEntry.Dispose();
        //    return valuesCollection;
        //}

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if ( ! String.IsNullOrEmpty( txtFilter.Text ) && ! String.IsNullOrWhiteSpace( txtFilter.Text ) )
                LoadComputerNames();
        }
    }
}
