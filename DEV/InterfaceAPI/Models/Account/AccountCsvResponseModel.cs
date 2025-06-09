using System.Collections.Generic;

namespace InterfaceAPI.Models.Account
{
    public class AccountCsvResponseModel
    {
        public AccountCsvResponseModel()
        {
            AccountImportResponseModelList = new List<AccountImportResponseModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<AccountImportResponseModel> AccountImportResponseModelList { get; set; }
    }
}