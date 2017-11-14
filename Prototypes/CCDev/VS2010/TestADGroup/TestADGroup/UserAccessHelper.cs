using System.Collections.Generic;
using System.Security.Principal;

namespace TestADGroup
{
    public static class UserAccessHelper
    {
        private static readonly UserAccessDetail UserAccessDetail = new UserAccessDetail();

        /// <summary>
        /// Returns the TMS Permission of the user with the supplied WindowsIdentity
        /// </summary>
        /// <param name="windowsIdentity">The WindowsIdentity for the user which will either come from System.Security.Principal or System.ServiceModel.ServiceSecurityContext.Current</param>
        /// <returns>User Access Permission for the specified User</returns>
        public static Enumerations.PermissionType UserAccessPermission(WindowsIdentity windowsIdentity)
        {
            Enumerations.PermissionType userAccessPermission = Enumerations.PermissionType.None;

                // A Chassis User Permission Override does not exist

                string userName = windowsIdentity.Name;

                int backslashPos = userName.LastIndexOf(@"\");

                string simpleUserName = backslashPos == -1 ? userName : userName.Substring(backslashPos + 1);

                // For speed, cache the User's Group membership
                Collections.CaseIgnoringSortedSetType userNetworkGroupCollection = UserAccessDetail.NetworkGroupsForUserName(simpleUserName);
                // Determine which Group of which the User is a member starting with the lowest Permission
                if (userNetworkGroupCollection.Contains(TmsPermissionNetworkGroupName[Enumerations.PermissionType.Read]))
                    userAccessPermission = Enumerations.PermissionType.Read;
                else if (
                    userNetworkGroupCollection.Contains(
                        TmsPermissionNetworkGroupName[Enumerations.PermissionType.Full]))
                    userAccessPermission = Enumerations.PermissionType.Full;
             // A Chassis User Permission Override does not exist

            return userAccessPermission;
        }

        public static string ConvertPermissionTypeToGroupName(Enumerations.PermissionType permissionType)
        {
            return TmsPermissionNetworkGroupName[permissionType];
        }

        private readonly static SortedDictionary<Enumerations.PermissionType, string /* Network Group Name */>
            TmsPermissionNetworkGroupName
                = new SortedDictionary<Enumerations.PermissionType, string>
                      {
                          {Enumerations.PermissionType.None, string.Empty},
                          {Enumerations.PermissionType.Read, "TMS Viewers"},
                          {Enumerations.PermissionType.Full, "TMS Developers"}
                      };

    }
}
