using System.Collections.Generic;

namespace InterfaceAPI.Models.Account
{
    public class AccountErrorResponseModel
    {
        public AccountErrorResponseModel()
        {
            AcctErrMsgList = new List<string>();
            AcctProcessErrMsgList = new List<string>();
        }
        public int ExAccountId { get; set; }
        public List<string> AcctProcessErrMsgList { get; set; }
        public List<string> AcctErrMsgList { get; set; }
    }
}