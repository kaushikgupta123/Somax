namespace Client.SecurityService
{
    public class UserPrincipalModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string[] Roles { get; set; }
        public int BranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
    }
}