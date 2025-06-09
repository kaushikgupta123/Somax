using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;


namespace DataContracts
{
    public partial class AssetAvailabilityLog : DataContractBase
    {
        public string PersonnelName { get; set; }

        public void UpdateFromDatabaseObjectAssetAvailabilityLog_V2(b_AssetAvailabilityLog dbObj)
        {            
            this.ObjectId = dbObj.ObjectId;
            this.TransactionDate = dbObj.TransactionDate;
            this.Event = dbObj.Event;          
            this.ReturnToService = dbObj.ReturnToService;
            this.Reason = dbObj.Reason;           
            this.ReasonCode = dbObj.ReasonCode;
            this.PersonnelName = dbObj.PersonnelName;
            // Turn on auditing
            AuditEnabled = true;
        }
    }
}
