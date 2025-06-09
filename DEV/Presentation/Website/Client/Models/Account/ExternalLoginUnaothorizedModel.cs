using System;

namespace Client.Models.Account
{
    [Serializable]
    public class ExternalLoginUnaothorizedModel
    {
        public string Provider { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ErrorMessage { get; set; }
    }
}