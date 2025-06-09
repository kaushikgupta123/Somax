using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Database;
using Database.Business;


namespace DataContracts
{
    public partial class WOCompletionSettings
    {
        public void RetrieveByClientId(DatabaseKey dbKey)
        {
            WoCompletionSettings_RetrieveByClientId trans = new WoCompletionSettings_RetrieveByClientId();
            trans.WOCompletionSettings = this.ToDatabaseObjectByClientId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectByClientId(trans.WOCompletionSettings);
        }

        public b_WOCompletionSettings ToDatabaseObjectByClientId()
        {
            b_WOCompletionSettings dbObj = new b_WOCompletionSettings();
            dbObj.WOCompletionSettingsId = this.WOCompletionSettingsId;
            dbObj.ClientId = this.ClientId;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectByClientId(b_WOCompletionSettings dbObj)
        {
            this.WOCompletionSettingsId = dbObj.WOCompletionSettingsId;
            this.ClientId = dbObj.ClientId;
            this.UseWOCompletionWizard = dbObj.UseWOCompletionWizard;
            this.WOCommentTab = dbObj.WOCommentTab;
            this.TimecardTab = dbObj.TimecardTab;
            this.AutoAddTimecard = dbObj.AutoAddTimecard;
            //V2-728
            this.WOCompCriteriaTab = dbObj.WOCompCriteriaTab;
            this.WOCompCriteriaTitle = dbObj.WOCompCriteriaTitle;
            this.WOCompCriteria = dbObj.WOCompCriteria;
            // Turn on auditing
            AuditEnabled = true;
        }
    }
}
