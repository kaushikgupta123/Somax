using System.Collections.Generic;

namespace InterfaceAPI.Models.Account
{
    public class AccountImportResponseModel
    {
        public AccountImportResponseModel()
        {
            errMessageList = new List<AccountErrorResponseModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<AccountErrorResponseModel> errMessageList { get; set; }
    }
}