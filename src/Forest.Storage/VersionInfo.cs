using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;

namespace Forest.Storage
{
    public class VersionInfo
    {
        public VersionInfo()
        {
            DateCreated = CurrentDateTime;
            AuthorCreated = CurrentUser;
        }

        public static string CurrentDateTime => DateTime.Now.ToString("yyyy-MM-dd : HH:mm:ss");

        public static string CurrentUser
        {
            get
            {
                UserPrincipal userPrincipal = null;
                try
                {
                    userPrincipal = UserPrincipal.Current;
                }
                catch
                {
                    // No user principal. Not possible to retrieve the display name of the user.
                }

                return userPrincipal == null
                    ? $"({WindowsIdentity.GetCurrent().Name.Split('\\').Last()})"
                    : $"{UserPrincipal.Current.DisplayName} ({WindowsIdentity.GetCurrent().Name.Split('\\').Last()})";
            }
        }

        public static int Year => 24;

        public static int MajorVersion => 1;

        public static int MinorVersion => 1;

        public string DateCreated { get; set; }

        public string AuthorCreated { get; set; }
    }
}