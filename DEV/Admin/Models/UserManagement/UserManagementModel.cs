using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models.UserManagement
{
    public class UserManagementModel
    {
        public long ClientId { get; set; }
        public long UserInfoId { get; set; }
        public long LoginInfoId { get; set; }
        public string CompanyName { get; set; }
        public string PersonnelClientLookupId { get; set; }
        public long PersonnelCraftId { get; set; }
        public string PersonnelShift { get; set; }
        public string CraftDescription { get; set; }
        [Display(Name = "UserName|" + LocalizeResourceSetConstants.Global)]
        public string UserName { get; set; }
        [Display(Name = "LastName|" + LocalizeResourceSetConstants.UserDetails)]
        public string LastName { get; set; }
        [Display(Name = "FirstName|" + LocalizeResourceSetConstants.UserDetails)]
        public string FirstName { get; set; }
        [Display(Name = "Email|" + LocalizeResourceSetConstants.UserDetails)]
        public string Email { get; set; }
        [Display(Name = "globalActive|" + LocalizeResourceSetConstants.Global)]
        public bool IsActive { get; set; }
        [Display(Name = "Administrator|" + LocalizeResourceSetConstants.UserDetails)]
        public bool IsSuperUser { get; set; }
        [Display(Name = "spnPersonnelID|" + LocalizeResourceSetConstants.UserDetails)]
        public long PersonnelID { get; set; }
        [Display(Name = "spnSecurityProfile|" + LocalizeResourceSetConstants.Global)]
        public string SecurityProfile { get; set; }
        public long TotalCount { get; set; }
        public int SiteCount { get; set; }
        public string SecurityProfileDescription { get; set; }

    }
}