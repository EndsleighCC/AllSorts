using System;
using System.Collections;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;

namespace TestADGroup
{
    [Serializable]
    public class UserAccessDetail
    {
        #region Constructors

        public UserAccessDetail()
        {
            InitialiseDefaultNamingContext();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Determine whether the specified Network User is a member of the specified Network Group
        /// </summary>
        /// <param name="userSAMAccountName">The simple name (SAM Account Name) e.g. "corc1", of the User</param>
        /// <param name="groupCommonName">The Common Name of the Group</param>
        /// <returns>Whether the User is (true) or is not (false) a member of the specified Network Group</returns>
        internal bool UserIsNetworkGroupMember(string userSAMAccountName, string groupCommonName)
        {
            bool userIsNetworkGroupMember = false;

            if (_DefaultNamingContext != null)
            {
                // There is a Default Naming Context

                try
                {
                    // Get the Group's Distinguished Name
                    string groupDistinguishedName = DistinguishedNameFromCommonName(groupCommonName);
                    if (groupDistinguishedName != null)
                    {
                        // Got Group Distinguished Name

                        string userDistinguishedName = DistinguishedNameFromSAMAccountName(userSAMAccountName);
                        Collections.CaseIgnoringSortedSetType userGroupDistinguishedNameCollection = NetworkGroupsForUser(LdapObjectPath(userDistinguishedName), true);
                        if (userGroupDistinguishedNameCollection != null)
                        {
                            // Search returned a result
                            userIsNetworkGroupMember = userGroupDistinguishedNameCollection.Contains(groupDistinguishedName);
                        } // Search returned a result

                    } // Got Group Distinguished Name

                }
                catch (Exception eek)
                {
                }

            } // There is a Default Naming Context

            return userIsNetworkGroupMember;
        }

        /// <summary>
        /// Determine whether the Network User corresponding to the supplied
        /// Technician's Reference is a member of the specified Network Group
        /// </summary>
        /// <param name="techRef">The Underwriting Technician's Reference e.g. "CHCO" of the User</param>
        /// <param name="groupName">The Common Name of the Group</param>
        /// <returns>Whether the User is (true) or is not (false) a member of the specified Network Group</returns>
        internal bool UserWithTechRefIsNetworkGroupMember(string techRef, string groupCommonName)
        {
            bool userIsNetworkGroupMember = false;

            Collections.CaseIgnoringSortedSetType userDistinguishedNamesForGroupName = NetworkDistinguishedNamesForGroupName(groupCommonName);

            // Examine the Tech Ref for all the Users in the Group looking for a match
            for (int userIndex = 0; ( ! userIsNetworkGroupMember ) && ( userIndex < userDistinguishedNamesForGroupName.Count ) ; ++userIndex )
            {
                string userDistinguishedName = userDistinguishedNamesForGroupName.ElementAt(userIndex);
                string userTechRef = UserTechRef(userDistinguishedName);
                if ((userTechRef!=null) && (String.Compare(userTechRef, techRef, true /* ignore case */ ) == 0))
                {
                    // A user with the specified Underwriting Technician's Reference is in the group
                    userIsNetworkGroupMember = true;
                }
            }

            return userIsNetworkGroupMember;
        }

        /// <summary>
        /// Produce a Collection of the simple names (Common Names)
        /// of the Network Groups of which the specified User is a member
        /// </summary>
        /// <param name="userCommonName">The name (sAMAccountName) of the User</param>
        /// <returns>A Collection of names (Common Names) of Groups of which the User is a member</returns>
        internal Collections.CaseIgnoringSortedSetType NetworkGroupsForUserName(string userCommonName)
        {
            var userGroupsCommonNameCollection = new Collections.CaseIgnoringSortedSetType();

            if (_DefaultNamingContext != null)
            {
                // There is a Default Naming Context

                try
                {
                    string userDistinguishedName = DistinguishedNameFromSAMAccountName(userCommonName);
                    if (userDistinguishedName != null)
                    {
                        // Got User distinguished Name

                        // Get a Collection of Groups of which the User is a member
                        Collections.CaseIgnoringSortedSetType userGroupDistinguishedNameCollection = NetworkGroupsForUser(LdapObjectPath(userDistinguishedName), true);

                        // Copy suitable details of the Collection of Group Distinguished Names to a Collection of Group Common Names
                        foreach (string groupDistinguishedName in userGroupDistinguishedNameCollection)
                        {
                            userGroupsCommonNameCollection.Add(CommonNameFromDistinguishedName(groupDistinguishedName));
                        }

                    } // Got User distinguished Name
                }
                catch (Exception eek)
                {
                }

            } // There is a Default Naming Context

            return userGroupsCommonNameCollection;
        }

        /// <summary>
        /// Return a Set of the Common Names of all Users within the supplied Active Directory Group
        /// </summary>
        /// <param name="groupCommonName">The Common Name of the "group"</param>
        /// <returns>A "Case Ignoring Sorted Set" of Common Names of group members</returns>
        internal Collections.CaseIgnoringSortedSetType NetworkUsersForGroupName(string groupCommonName)
        {
            Collections.CaseIgnoringSortedSetType groupUserCommonNameCollection = new Collections.CaseIgnoringSortedSetType();

            SearchResultCollection groupSearchResultCollection =
                SearchResultCollectionFindAll("(&(cn=" + groupCommonName + ")(ObjectClass=group))");
            if (groupSearchResultCollection != null)
            {
                // The search returned at least one result

                foreach (SearchResult groupSearchResult in groupSearchResultCollection)
                {
                    DirectoryEntry groupDirectoryEntry = groupSearchResult.GetDirectoryEntry();

                    Collections.CaseIgnoringSortedSetType groupUserDistinguishedNameCollection = NetworkUsersInGroup(groupDirectoryEntry.Path, true);
                    foreach (string distinguishedName in groupUserDistinguishedNameCollection)
                    {
                        groupUserCommonNameCollection.Add(CommonNameFromDistinguishedName(distinguishedName));
                    }
                
                }
            }

            return groupUserCommonNameCollection;
        }

        /// <summary>
        /// Return a Set of the Distinguished Names of all Users within the supplied Active Directory Group
        /// </summary>
        /// <param name="groupCommonName">The Common Name of the "group"</param>
        /// <returns>A "Case Ignoring Sorted Set" of Distinguished Names of group members</returns>
        internal Collections.CaseIgnoringSortedSetType NetworkDistinguishedNamesForGroupName(string groupCommonName)
        {
            Collections.CaseIgnoringSortedSetType groupUserDistinguishedNameCollection = new Collections.CaseIgnoringSortedSetType();

            SearchResultCollection groupSearchResultCollection =
                SearchResultCollectionFindAll("(&(cn=" + groupCommonName + ")(ObjectClass=group))");
            if (groupSearchResultCollection != null)
            {
                // The search returned at least one result

                foreach (SearchResult groupSearchResult in groupSearchResultCollection)
                {
                    DirectoryEntry groupDirectoryEntry = groupSearchResult.GetDirectoryEntry();

                    Collections.CaseIgnoringSortedSetType entryGroupUserDistinguishedNameCollection = NetworkUsersInGroup(groupDirectoryEntry.Path, true);
                    foreach (string distinguishedName in entryGroupUserDistinguishedNameCollection)
                    {
                        groupUserDistinguishedNameCollection.Add(distinguishedName);
                    }

                }
            }

            return groupUserDistinguishedNameCollection;
        }

        /// <summary>
        /// Return a Set containing the names of all Active Directory Objects of type "group"
        /// </summary>
        /// <returns>A "Case Ignoring Sorted Set" of Common Names of Active Directory Groups</returns>
        internal Collections.CaseIgnoringSortedSetType NetworkGroupSet()
        {
            Collections.CaseIgnoringSortedSetType networkGroupCommonNameCollection = new Collections.CaseIgnoringSortedSetType();
            SearchResultCollection groupSearchResultCollection = SearchResultCollectionFindAll("(&(cn=*)(ObjectClass=group))");
            if (groupSearchResultCollection != null)
            {
                // The search returned at least one result

                foreach (SearchResult groupSearchResult in groupSearchResultCollection)
                {
                    DirectoryEntry groupDirectoryEntry = groupSearchResult.GetDirectoryEntry();

                    networkGroupCommonNameCollection.Add(CommonNameFromDistinguishedName(groupDirectoryEntry.Path));
                }

            } // The search returned at least one result

            return networkGroupCommonNameCollection;
        }

        /// <summary>
        /// Return the "description" Property for the supplied "user" Active Directory Object
        /// </summary>
        /// <param name="userDistinguishedName">The Distinguished Name of the required "user" Object</param>
        /// <returns>Either the contents of the "description" Property, or, null if not found</returns>
        internal string UserDescription( string userDistinguishedName )
        {
            string userDescription = null;

            SearchResultCollection userSearchResultCollection =
                SearchResultCollectionFindAll("(&(distinguishedName=" + userDistinguishedName + ")(ObjectClass=user))");
            if (userSearchResultCollection != null)
            {
                // The search returned at least one result

                foreach (SearchResult groupSearchResult in userSearchResultCollection)
                {
                    DirectoryEntry userDirectoryEntry = groupSearchResult.GetDirectoryEntry();

                    userDescription = (string)userDirectoryEntry.InvokeGet("description");
                }

            } // The search returned at least one result

            return userDescription;
        }

        /// <summary>
        /// Return the Underwriting Technician's Reference for the specified "user" Distinguished Name
        /// </summary>
        /// <param name="userDistinguishedName">The Distinguished Name of the required "user" Object</param>
        /// <returns>Either the contents of the Underwriting Technician's Reference, or, null if not found</returns>
        internal string UserTechRef(string userDistinguishedName)
        {
            string userTechRef = null;
            string userDescription = UserDescription(userDistinguishedName);
            if (userDescription != null)
            {
                string[] userDescriptionTokens = userDescription.Split(' ');
                if (userDescriptionTokens.Count() > 0 )
                {
                    // User Description has more than one token

                    // The Underwriting Technician's Reference should be the first token
                    if ((userDescriptionTokens[0].Length == TechniciansReferenceLength)
                         && (StringContainsOnlyAlphabeticCharacters(userDescriptionTokens[0])))
                        // There appears to be an Underwriting Technician's Reference
                        userTechRef = userDescriptionTokens[0].ToUpper();

                } // User Description has more than one token
            }
            return userTechRef;
        }

        /// <summary>
        /// Useful function for displaying the contents of all Properties of the Object
        /// associated with the supplied Active Directory Entry
        /// </summary>
        /// <param name="directoryEntry">The Active Directory Entry for the object</param>
        private void ShowAllProperties(DirectoryEntry directoryEntry)
        {
            Debug.WriteLine("Directory Entry Properties for Object \"" + directoryEntry.Name + "\" of class \"" + directoryEntry.SchemaClassName + "\"");

            foreach (string propertyName in directoryEntry.Properties.PropertyNames)
            {
                Debug.WriteLine("    " + propertyName + "=" + directoryEntry.Properties[propertyName]);

                Collections.CaseIgnoringSortedSetType propertyValueSet = new Collections.CaseIgnoringSortedSetType();

                propertyValueSet = AttributeValuesMultiString(propertyName, directoryEntry, propertyValueSet);

                foreach (string propertyValue in propertyValueSet)
                {
                    string valueString = "            " + propertyName + "=" + propertyValue;
                    Debug.WriteLine(valueString);
                }
            }
        }

        internal bool StringContainsOnlyAlphabeticCharacters( string checkContents )
        {
            bool stringContainsOnlyAlphabeticCharacters = true;

            for (int charIndex = 0; (stringContainsOnlyAlphabeticCharacters ) && (charIndex < checkContents.Length); ++charIndex)
            {
                if ( ! (    ( (checkContents[charIndex] >= 'A') && (checkContents[charIndex] <= 'Z') )
                         || ( (checkContents[charIndex] >= 'a') && (checkContents[charIndex] <= 'z') )
                       )
                   )
                    stringContainsOnlyAlphabeticCharacters = false;
            }

            return stringContainsOnlyAlphabeticCharacters;
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Determine the Default Naming Context e.g. "DC=eis,DC=endsleigh,DC=co,DC=uk"
        /// </summary>
        private void InitialiseDefaultNamingContext()
        {
            var rootEntry = new DirectoryEntry(LdapObjectPath("rootDSE"));

            try
            {
                _DefaultNamingContext = (string)rootEntry.Properties["defaultNamingContext"].Value;
            }
            catch (Exception)
            {
                _DefaultNamingContext = null ;
            }
        }

        /// <summary>
        /// Determine the LDAP Default Naming Context by prefixing the "Default Naming Context" with the LDAP Prefix
        /// </summary>
        /// <returns>LDAP Prefix : Default Naming Context</returns>
        private string LdapDefaultNamingContext()
        {
            return LdapPrefix + _DefaultNamingContext;
        }

        /// <summary>
        /// Generates the full LDAP Path to the supplied path
        /// </summary>
        /// <param name="objectPath">the non-LDAP Distinguished Name of the object</param>
        /// <returns>LDAP Path to the object</returns>
        private static string LdapObjectPath(string objectPath)
        {
            return LdapPrefix + objectPath;
        }

        /// <summary>
        /// Produce the Distinguished Name using the supplied search criteria
        /// </summary>
        /// <param name="lDAPsearchCriteria">LDAP Search criteria</param>
        /// <returns>the Distinguised name of the object returns by the search</returns>
        private string DistinguishedNameFromSearch(string lDAPsearchCriteria)
        {
            string distinguishedName = null;
            SearchResult searchResult = SearchResultCollectionFindOne(lDAPsearchCriteria);

            if (searchResult != null)
            {
                // The search returned a result
                string ldapPath = searchResult.Path;
                distinguishedName = ldapPath.Substring(LdapPrefix.Length);
            }
            return distinguishedName;
        }

        /// <summary>
        /// Determines the Distinguished Name for the supplied SAM Account Name
        /// e.g. corc1 -> "CN=Chris Cornelius,OU=Systems Team,OU=Users,OU=Head Office,DC=eis,DC=endsleigh,DC=co,DC=uk"
        /// </summary>
        /// <param name="sAMAccountName">The "Security Access Module", or, "logon name" of the account to be resolved</param>
        /// <returns></returns>
        private string DistinguishedNameFromSAMAccountName(string sAMAccountName)
        {
            return DistinguishedNameFromSearch( "(SAMAccountName=" + sAMAccountName + ")");
        }

        /// <summary>
        /// Determines the Distinguished Name for the supplied Common Name
        /// e.g. SHNSDW26 -> "CN=SHNSDW26,OU=Software Deployment,OU=Workstations,OU=Head Office,DC=eis,DC=endsleigh,DC=co,DC=uk"
        /// </summary>
        /// <param name="commonName"></param>
        /// <returns></returns>
        private string DistinguishedNameFromCommonName(string commonName)
        {
            return DistinguishedNameFromSearch( "(cn=" + commonName + ")");
        }

        /// <summary>
        /// Parses the Common Name from the supplied Distinguished Name
        /// </summary>
        /// <param name="distinguishedName">The Distinguished Name from which the Common Name will be parsed</param>
        /// <returns>The Common Name</returns>
        private string CommonNameFromDistinguishedName(string distinguishedName)
        {
            string commonName = null;
            int commonNameIndex = distinguishedName.IndexOf(CommonNamePrefix);
            if (commonNameIndex == -1)
                // No Common Name Prefix so just return the entire name
                commonName = distinguishedName;
            else
            {
                // Parse out the Common Name

                // Get a copy of the tail of the string without the prefix
                string distinguishedNameTail = distinguishedName.Substring(commonNameIndex + CommonNamePrefix.Length);

                // Look for the end of the Common Name
                int commaIndex = distinguishedNameTail.IndexOf(",");
                if (commaIndex == -1)
                    // No comma so just use the entire tail
                    commonName = distinguishedNameTail;
                else
                    commonName = distinguishedNameTail.Substring(0, commaIndex);

            } // Parse out the Common Name

            return commonName;
        }

        /// <summary>
        /// Generates a DirectorySearcher Object with its Filter set as specified
        /// </summary>
        /// <param name="searchFilter">An LDAP Clause that can be used to filter the Directory</param>
        /// <returns>A Directory Searcher object</returns>
        private DirectorySearcher NewDirectorySearcher(string searchFilter)
        {
            DirectoryEntry defaultEntry = new DirectoryEntry(LdapDefaultNamingContext());
            DirectorySearcher objectSearcher = new DirectorySearcher(defaultEntry);
            objectSearcher.Filter = searchFilter;
            return objectSearcher;
        }

        /// <summary>
        /// Executes an LDAP Search using the specified filter to return all matching results
        /// </summary>
        /// <param name="searchFilter">The LDAP Filter to be used for the search</param>
        /// <returns>A Search Result Collection containing the search results which will be null if no results were found</returns>
        private System.DirectoryServices.SearchResultCollection SearchResultCollectionFindAll(string searchFilter)
        {
            DirectorySearcher directorySearcher = NewDirectorySearcher(searchFilter);
            return directorySearcher.FindAll();
        }

        /// <summary>
        /// Executes an LDAP Search using the specified filter to return a single matching result
        /// </summary>
        /// <param name="searchFilter">The LDAP Filter to be used for the search</param>
        /// <returns>A Search Result containing the single search result which will be null if no results were found</returns>
        private System.DirectoryServices.SearchResult SearchResultCollectionFindOne(string searchFilter)
        {
            DirectorySearcher directorySearcher = NewDirectorySearcher(searchFilter);
            return directorySearcher.FindOne();
        }

        /// <summary>
        /// Produce a Collection of Distinguished Names representing the Network Groups
        /// of which the specified Network User is a member
        /// </summary>
        /// <param name="userLdapPath">The LDAP Path for Network User</param>
        /// <param name="recursive">Whether the search should be recursive or not</param>
        /// <returns>A Collection of Network Group Distinguished Names of which the User is a member</returns>
        private Collections.CaseIgnoringSortedSetType NetworkGroupsForUser(string userLdapPath, bool recursive)
        {
            Collections.CaseIgnoringSortedSetType groupMemberships = new Collections.CaseIgnoringSortedSetType();
            return AttributeValuesMultiString("memberOf", userLdapPath, groupMemberships, recursive);
        }

        /// <summary>
        /// Produce a Collection of Distinguished Names representing the Network Users
        /// that belong to the specified Group
        /// </summary>
        /// <param name="groupLdapPath">The LDAP Path for Network Group</param>
        /// <param name="recursive">Whether the search should be recursive or not</param>
        /// <returns>A Collection of Network User Distinguished Names that are members of the Group</returns>
        private Collections.CaseIgnoringSortedSetType NetworkUsersInGroup(string groupLdapPath, bool recursive)
        {
            Collections.CaseIgnoringSortedSetType userMemberships = new Collections.CaseIgnoringSortedSetType();
            return AttributeValuesMultiString("member", groupLdapPath, userMemberships, recursive);
        }

        /// <summary>
        /// Produce the possible multiple values of the specified Attribute for the specified Object
        /// </summary>
        /// <param name="attributeName">The name of the Attribute</param>
        /// <param name="objectDistinguishedName">The Distinguished Name of the Object</param>
        /// <param name="valuesCollection">A Collection of values of the specified Attribute</param>
        /// <param name="recursive">Whether the search should be recursive or not</param>
        /// <returns>A Collection of Values for the specified Attribute</returns>
        private Collections.CaseIgnoringSortedSetType AttributeValuesMultiString(string attributeName, string objectDistinguishedName, Collections.CaseIgnoringSortedSetType valuesCollection, bool recursive)
        {
            DirectoryEntry objectEntry = new DirectoryEntry(objectDistinguishedName);

            valuesCollection = AttributeValuesMultiString(attributeName, objectEntry, valuesCollection, recursive);

            objectEntry.Close();
            objectEntry.Dispose();

            return valuesCollection;
        }

        /// <summary>
        /// Produce the possible multiple values of the specified Attribute for the specified object
        /// without performing recursion
        /// </summary>
        /// <param name="attributeName">The name of the Attribute</param>
        /// <param name="objectDistinguishedName">The Distinguished Name of the Object</param>
        /// <param name="valuesCollection">A Collection of values of the specified Attribute</param>
        /// <returns>A Collection of Values for the specified Attribute</returns>
        private Collections.CaseIgnoringSortedSetType AttributeValuesMultiString(string attributeName, DirectoryEntry objectEntry, Collections.CaseIgnoringSortedSetType valuesCollection)
        {
            return AttributeValuesMultiString(attributeName, objectEntry, valuesCollection, false);
        }

        /// <summary>
        /// Produce the possible multiple values of the specified Attribute for the supplied object
        /// without performing recursion
        /// </summary>
        /// <param name="attributeName">The name of the Attribute</param>
        /// <param name="objectEntry">The Directory Entry for the Object</param>
        /// <param name="valuesCollection">A Collection of values of the specified Attribute</param>
        /// <param name="recursive">Whether the search should be recursive or not</param>
        /// <returns>A Collection of Values for the specified Attribute</returns>
        private Collections.CaseIgnoringSortedSetType AttributeValuesMultiString(string attributeName, DirectoryEntry objectEntry, Collections.CaseIgnoringSortedSetType valuesCollection, bool recursive)
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

        #endregion Private Methods

        #region Private member variables

        /// <summary>
        /// Store the Default Naming Context e.g. "DC=eis,DC=endsleigh,DC=co,DC=uk"
        /// </summary>
        private string _DefaultNamingContext = null;

        #endregion Private member variables

        #region Private Constants

        /// <summary>
        /// LDAP Prefix
        /// </summary>
        private const string LdapPrefix = "LDAP://";

        /// <summary>
        /// Common Name Prefix which forms part of a Distinguished Name
        /// </summary>
        private const string CommonNamePrefix = "CN=";

        /// <summary>
        /// All Underwriting Technician's References must be 4 characters
        /// </summary>
        private const int TechniciansReferenceLength = 4;

        #endregion Private Constants

    } // class UserAccessDetail

} // namespace Endsleigh.Legacy.TMS
