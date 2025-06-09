namespace Client.Models.Configuration.UserManagement
{
    public class PrintUserModel_Enterprise
    {
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string EmployeeId { get; set; } //V2-1160
        public string SecurityProfile { get; set; }
        
    }
}