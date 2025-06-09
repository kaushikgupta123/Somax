namespace Admin.Models.UserManagement
{
    public class PrintUserModel
    {
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Client { get; set; }
        public string Email { get; set; }
        public string SecurityProfile { get; set; }
        public bool Active { get; set; }
    }
}