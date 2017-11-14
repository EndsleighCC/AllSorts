using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkDetail;

namespace TestADfunctions
{
    class Program
    {
        enum FunctionRequired
        {
            None,
            SingleTechRefLookup ,
            GroupTechRefLookup
        }

        private const int TechniciansReferenceLength = 4;

        static string ParseDescriptionForUserTechRef( string userDescription )
        {
            string possibleUserTechRef = null;

            if (!String.IsNullOrEmpty(userDescription) && !String.IsNullOrWhiteSpace(userDescription))
            {
                string[] userDescriptionTokens = userDescription.Split(new char[] {' '},
                                                                       StringSplitOptions.RemoveEmptyEntries);
                if (userDescriptionTokens.Count() > 0)
                {
                    // User Description has more than one token

                    if (userDescriptionTokens[0].Length == TechniciansReferenceLength)
                        possibleUserTechRef = userDescriptionTokens[0].ToUpper();
                }
            }
            return possibleUserTechRef;
        }

        static void ValidateUserTechRef( string userCommonName , string userTechRef , bool displayFormatIsOk)
        {
            if (!String.IsNullOrEmpty(userTechRef) && !String.IsNullOrWhiteSpace(userTechRef))
            {
                if (userTechRef.Length != 4)
                    Console.WriteLine("    ****** User \"{0}\" : Tech. Ref. is not 4 characters long", userCommonName);
                else
                {
                    // Check the format

                    bool validFormat = true;
                    foreach (char ch in userTechRef)
                    {
                        if ((ch < 'A') || (ch > 'Z'))
                            validFormat = false;
                    }

                    if (!validFormat)
                        Console.WriteLine("    ****** User \"{0}\" : Format of Tech. Ref. \"{1}\" is invalid", userCommonName, userTechRef);
                    else if (displayFormatIsOk)
                        Console.WriteLine("    ****** User \"{0}\" : Format of Tech. Ref. \"{1}\" is correct", userCommonName, userTechRef);

                } // Check the format
            }
        }

        static void ShowGroupMembershipDetails(UserAccessDetail userAccessDetail, string userAccountName, string userTechRef, string groupName )
        {
            Console.WriteLine();
            if (userAccessDetail.UserWithTechRefIsNetworkGroupMember(userTechRef, groupName))
                Console.WriteLine("Tech. Ref. \"{0}\" for User \"{1}\" is a member of \"{2}\"", userTechRef,
                                  userAccountName, groupName);
            else
                Console.WriteLine("Tech. Ref. \"{0}\" for User \"{1}\" is NOT a member of \"{2}\"", userTechRef,
                                  userAccountName, groupName);
            
        }

        static void Main(string[] args)
        {
            string userAccountName = Environment.UserName;

            bool accountNameSpecified = false;

            if (args.Count() > 0)
            {
                userAccountName = args[0];
                accountNameSpecified = true;
            }

            NetworkDetail.UserAccessDetail userAccessDetail = new UserAccessDetail();

            Console.WriteLine();
            Console.WriteLine("Distinguished Name for Account \"{0}\" is:", userAccountName);
            Console.WriteLine("    \"{0}\"",userAccessDetail.DistinguishedNameFromSAMAccountName(userAccountName));

            string userDescription =
                userAccessDetail.UserDescriptionFromDistinguishedName(
                    userAccessDetail.DistinguishedNameFromSAMAccountName(userAccountName));

            Console.WriteLine();
            Console.WriteLine("User Description for Account \"{0}\" is \"{1}\"",userAccountName,userDescription);

            string userTechRef = userAccessDetail.UserTechRefFromAccountName(userAccountName);

            Console.WriteLine();
            if (userTechRef != null)
            {
                Console.WriteLine("User \"{0}\" Tech. Ref. is \"{1}\"", userAccountName, userTechRef);
                ValidateUserTechRef(userAccountName, userTechRef, true);
            }
            else
            {
                string potentialTechRef = ParseDescriptionForUserTechRef(userDescription);
                if (potentialTechRef == null)
                    Console.WriteLine("User \"{0}\" does not have a Tech. Ref.", userAccountName);
                else
                {
                    Console.WriteLine("User \"{0}\" has a potential Tech. Ref. of \"{1}\"", userAccountName,
                                      potentialTechRef);
                    userTechRef = potentialTechRef;
                }
            }

            FunctionRequired functionRequired = FunctionRequired.None;
            if (accountNameSpecified)
                functionRequired = FunctionRequired.SingleTechRefLookup;
            else
                functionRequired = FunctionRequired.GroupTechRefLookup;
            switch (functionRequired)
            {
                case FunctionRequired.SingleTechRefLookup:
                    {
                        ShowGroupMembershipDetails(userAccessDetail, userAccountName, userTechRef, "System Builders");
                        ShowGroupMembershipDetails(userAccessDetail, userAccountName, userTechRef, "Zurich Home Underwriting Specialist");
                    }
                    break;
                case FunctionRequired.GroupTechRefLookup:
                    {
                        Console.WriteLine();
                        Console.WriteLine("Looking up all AD Users");
                        Console.WriteLine();
                        Collections.CaseIgnoringSortedSetType networkUserCommonNames = userAccessDetail.NetworkUserSet();
                        Console.WriteLine("Total Count of AD Users is {0}",networkUserCommonNames.Count);
                        Console.WriteLine();
                        int userIndex = -1;
                        foreach (string userCommonName in networkUserCommonNames)
                        {
                            userIndex += 1;
                            string userDistinguishedName =
                                userAccessDetail.DistinguishedNameFromCommonName(userCommonName);
                            string thisUserDescription =
                                userAccessDetail.UserDescriptionFromDistinguishedName(userDistinguishedName);
                            string thisUserTechRef =
                                userAccessDetail.UserTechRefFromUserDistinguishedName(userAccessDetail.DistinguishedNameFromCommonName(userCommonName));
                            Console.WriteLine("{0,4} : User \"{1}\". Tech. Ref. = \"{2}\". Description = \"{3}\"",
                                                    userIndex,
                                                    userCommonName,
                                                    ( String.IsNullOrEmpty(thisUserTechRef) ? "Not known" : thisUserTechRef),
                                                    thisUserDescription);
                            ValidateUserTechRef(userCommonName,ParseDescriptionForUserTechRef(thisUserDescription), false);
                        }
                    }
                    break;
            }

            Collections.CaseIgnoringSortedSetType networkGroupsForUser = userAccessDetail.NetworkGroupsForUserAccountName(userAccountName);
            Console.WriteLine();
            Console.WriteLine("Network Group Membership for \"{0}\"", userAccountName);
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            foreach (string networkGroupName in networkGroupsForUser)
            {
                Console.WriteLine("    \"{0}\"",networkGroupName);
            }

        }
    }
}
