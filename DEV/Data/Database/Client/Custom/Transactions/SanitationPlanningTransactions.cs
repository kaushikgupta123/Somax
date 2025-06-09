using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using Common.Enumerations;

namespace Database
{
    public class SanitationPlanningTransactions_RetrieveByMasterId : SanitationPlanning_TransactionBaseClass
    {
        public SanitationPlanningTransactions_RetrieveByMasterId ()
	    {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
	    }
        
        public List<b_SanitationPlanning> SanitationPlanningList { get; set; }
        public long SanitationMasterId { get; set; }
        public long ClientId { get; set; }
        public string Category { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_SanitationPlanning> tmpArray = null;
            b_SanitationPlanning o = new b_SanitationPlanning();
			
			  
            // Explicitly set tenant id from dbkey
               o.ClientId = this.dbKey.Client.ClientId;


               o.RetrieveBySanitationMasterId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName,ClientId,SanitationMasterId,Category, ref tmpArray);

               SanitationPlanningList = tmpArray;
        }
    }
    public class SanitationPlanningTransactions_RetrieveBySanitationJobID : SanitationPlanning_TransactionBaseClass
    {
        public SanitationPlanningTransactions_RetrieveBySanitationJobID()
        {

            UseDatabase = DatabaseTypeEnum.Client;
        }

        public List<b_SanitationPlanning> SanitationPlanningList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_SanitationPlanning> tmpArray = null;

            SanitationPlanning.RetrieveBy_SanitationJobId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SanitationPlanningList = tmpArray;
        }
    }
}
