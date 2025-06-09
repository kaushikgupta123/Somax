using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constants
{
    public class FailureReasonConstant
    {
        public const string UserNotFound = "USERNOTFOUND";
        public const string AccountInactive = "ACCOUNTINACTIVE";
        public const string MaxAttemptExceed = "MAXATTEMPTEXCEED";
        public const string PasswordNotMatched = "PASSWORDNOTMATCHED";
        public const string TemporaryPasswordExpired = "TEMPORARYPASSWORDEXPIRED";
        public const string UserNameIncorrect = "USERNAMEINCORRECT";
        public const string UnAuthorisedMsg = "UNAUTHORISEDMSG";
        public const string ClientInactiveMsg = "CLIENTINACTIVE";//V2-858
        public const string SiteInactiveMsg = "SITEINACTIVEMSG";//V2-858
    }
}
