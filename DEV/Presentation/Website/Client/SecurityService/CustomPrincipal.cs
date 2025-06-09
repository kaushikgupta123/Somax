using System.Linq;
using System.Security.Principal;

namespace Client.SecurityService
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity
        {
            get;
            private set;
        }

        public bool IsInRole(string role)
        {
            return Roles.Any(r => role.Contains(r));
        }
        public bool IsInRole(string[] roles)
        {
            var isInRole = Roles.Any(r => roles.Contains(r));
            return isInRole;
        }

        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public int UserID { get; set; }
        public string UserName { get; set; }
        public string[] Roles { get; set; }

    }
}