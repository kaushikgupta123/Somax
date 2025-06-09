using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;

namespace DataContracts
{
    public partial    class PasswordSettings 
    {
        #region Property
        public int MaxAttempts { get; set; }
        #endregion
        public void UpdatePasswordSettingsByClientId(DatabaseKey dbKey)
        {
            PasswordSettings_UpdateByClientId_V2 trans = new PasswordSettings_UpdateByClientId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UseTransaction = false;
            trans.PasswordSettings = this.ToDatabaseObject();
            trans.PasswordSettings.MaxAttempts = this.MaxAttempts;          
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
           // return trans.PasswordSettings.UpdateIndex;
        }

        public void RetrieveByClientId(DatabaseKey dbKey)
        {
            PasswordSettings_RetrieveByClientId trans = new PasswordSettings_RetrieveByClientId();
            trans.PasswordSettings = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectByClientId(trans.PasswordSettings);
        }

        public void UpdateFromDatabaseObjectByClientId(b_PasswordSettings dbObj)
        {
            this.PasswordSettingsId = dbObj.PasswordSettingsId;
            this.ClientId = dbObj.ClientId;
            this.PWReqMinLength = dbObj.PWReqMinLength;
            this.PWMinLength = dbObj.PWMinLength;
            this.PWReqExpiration = dbObj.PWReqExpiration;
            this.PWExpiresDays = dbObj.PWExpiresDays;
            this.PWRequireNumber = dbObj.PWRequireNumber;
            this.PWRequireAlpha = dbObj.PWRequireAlpha;
            this.PWRequireMixedCase = dbObj.PWRequireMixedCase;
            this.PWRequireSpecialChar = dbObj.PWRequireSpecialChar;
            this.PWNoRepeatChar = dbObj.PWNoRepeatChar;
            this.PWNotEqualUserName = dbObj.PWNotEqualUserName;
            this.AllowAdminReset = dbObj.AllowAdminReset;
            this.MaxAttempts = dbObj.MaxAttempts;
            // Turn on auditing
            AuditEnabled = true;
        }
    }
}
