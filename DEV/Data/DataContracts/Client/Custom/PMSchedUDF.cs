using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Database;
using Database.Business;
using Database.Client.Custom.Transactions;
using Newtonsoft.Json;
using Common.Constants;
using Common.Extensions;

namespace DataContracts
{
    public partial class PMSchedUDF : DataContractBase
    {
        #region Variables
        public string Select1ClientLookupId { get; set; }
        public string Select2ClientLookupId { get; set; }
        public string Select3ClientLookupId { get; set; }
        public string Select4ClientLookupId { get; set; }
        #endregion

        public PMSchedUDF RetrivePMSchedUDFByPrevMaintSchedId(DatabaseKey dbKey)
        {
            PMSchedUDF_RetrieveByPrevMaintSchedId trans = new PMSchedUDF_RetrieveByPrevMaintSchedId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PMSchedUDF =this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            PMSchedUDF objPMSchedUDF = new PMSchedUDF();
            objPMSchedUDF.UpdateFromDatabaseObjectExtended(trans.PMSchedUDFs);
            return objPMSchedUDF;
        }
        public void UpdateFromDatabaseObjectExtended(b_PMSchedUDF dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.PMSchedUDFId = dbObj.PMSchedUDFId;
            this.Text1 = dbObj.Text1;
            this.Text2 = dbObj.Text2;
            this.Text3 = dbObj.Text3;
            this.Text4 = dbObj.Text4;
            this.Date1 = dbObj.Date1;
            this.Date2 = dbObj.Date2;
            this.Date3 = dbObj.Date3;
            this.Date4 = dbObj.Date4;
            this.Bit1 = dbObj.Bit1;
            this.Bit2 = dbObj.Bit2;
            this.Bit3 = dbObj.Bit3;
            this.Bit4 = dbObj.Bit4;
            this.Numeric1 = dbObj.Numeric1;
            this.Numeric2 = dbObj.Numeric2;
            this.Numeric3 = dbObj.Numeric3;
            this.Numeric4 = dbObj.Numeric4;
            this.Select1 = dbObj.Select1;
            this.Select2 = dbObj.Select2;
            this.Select3 = dbObj.Select3;
            this.Select4 = dbObj.Select4;
            this.PrevMaintSchedId = dbObj.PrevMaintSchedId;

            this.Select1ClientLookupId = dbObj.Select1ClientLookupId;
            this.Select2ClientLookupId = dbObj.Select2ClientLookupId;
            this.Select3ClientLookupId = dbObj.Select3ClientLookupId;
            this.Select4ClientLookupId = dbObj.Select4ClientLookupId;

            AuditEnabled = true;
        }
    }
}
