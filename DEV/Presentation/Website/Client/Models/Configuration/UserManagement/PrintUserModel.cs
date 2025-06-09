namespace Client.Models.Configuration.UserManagement
{
    public class PrintUserModel
    {
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }        
        public string Email { get; set; }
        public string EmployeeId { get; set; } //V2-1160
        public string Craft { get; set; }
        public string SecurityProfile { get; set; }
        public long PersonnelCraftId { get; set; }
    }
}