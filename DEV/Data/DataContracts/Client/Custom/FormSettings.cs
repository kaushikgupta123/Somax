using Database.Business;
using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class FormSettings
    {
        public void RetrieveByClientId(DatabaseKey dbKey)
        {
            FormSettings_RetrieveByClientId trans = new FormSettings_RetrieveByClientId();
            trans.FormSettings = this.ToDatabaseObjectByClientId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectByClientId(trans.FormSettings);
        }

        public b_FormSettings ToDatabaseObjectByClientId()
        {
            b_FormSettings dbObj = new b_FormSettings();
            dbObj.FormSettingsId = this.FormSettingsId;
            dbObj.ClientId = this.ClientId;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectByClientId(b_FormSettings dbObj)
        {
            this.FormSettingsId = dbObj.FormSettingsId;
            this.ClientId = dbObj.ClientId;
            this.WOLaborRecording = dbObj.WOLaborRecording;
            this.WOUIC = dbObj.WOUIC;
            this.WOScheduling = dbObj.WOScheduling;
            this.WOSummary = dbObj.WOSummary;
            this.WOPhotos = dbObj.WOPhotos;
            this.WOComments = dbObj.WOComments;
            //V2-945
            this.PRUIC = dbObj.PRUIC;
            this.PRLine2 = dbObj.PRLine2;
            this.PRLIUIC = dbObj.PRLIUIC;
            this.PRComments = dbObj.PRComments;
            //V2-946
            this.POUIC = dbObj.POUIC;
            this.POLine2 = dbObj.POLine2;
            this.POLIUIC = dbObj.POLIUIC;
            this.POComments = dbObj.POComments;
            this.POTandC = dbObj.POTandC;
            this.POTandCURL = dbObj.POTandCURL;
            //V2-947
            this.PORHeader = dbObj.PORHeader;
            this.PORLine2 = dbObj.PORLine2;
            this.PORPrint = dbObj.PORPrint;
            //V2-947
            this.PORUIC = dbObj.PORUIC;
            this.PORComments = dbObj.PORComments;
            // Turn on auditing
            AuditEnabled = true;
        }
    }
}
