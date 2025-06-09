using Database.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    #region PurchaseRequest
    public class ApprovalRoute_RetrieveForPurchaseRequest_V2 : ApprovalRoute_TransactionBaseClass
    {
        public List<b_ApprovalRoute> ApprovalRouteList { get; set; }

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
            List<b_ApprovalRoute> tmpArray = null;
            ApprovalRoute.ApprovalRoute_RetrieveForPurchaseRequest_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            ApprovalRouteList = new List<b_ApprovalRoute>();
            foreach (b_ApprovalRoute tmpObj in tmpArray)
            {
                ApprovalRouteList.Add(tmpObj);
            }
        }
    }
    #endregion
    #region WorkRequest
    public class ApprovalRoute_RetrieveForWorkRequest_V2 : ApprovalRoute_TransactionBaseClass
    {
        public List<b_ApprovalRoute> ApprovalRouteList { get; set; }

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
            List<b_ApprovalRoute> tmpArray = null;
            ApprovalRoute.ApprovalRoute_RetrieveForWorkRequest_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            ApprovalRouteList = new List<b_ApprovalRoute>();
            foreach (b_ApprovalRoute tmpObj in tmpArray)
            {
                ApprovalRouteList.Add(tmpObj);
            }
        }
    }
    #endregion
    #region V2-730
    public class ApprovalRoute_RetrievebyObjectId_V2 : ApprovalRoute_TransactionBaseClass
    {
        public List<b_ApprovalRoute> ApprovalRouteList { get; set; }

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
            List<b_ApprovalRoute> tmpArray = null;
            ApprovalRoute.ApprovalRoute_RetrievebyObjectId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            ApprovalRouteList = new List<b_ApprovalRoute>();
            foreach (b_ApprovalRoute tmpObj in tmpArray)
            {
                ApprovalRouteList.Add(tmpObj);
            }
        }
    }

    public class ApprovalRoute_UpdateByForeignKeys_V2 : ApprovalRoute_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ApprovalRoute.ObjectId == 0)
            {
                string message = "ApprovalRoute has an invalid Object ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            ApprovalRoute.UpdateByObjectIdInDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    #endregion
   
    #region MaterialRequest

    public class ApprovalRoute_RetrieveForMaterialRequest_V2 : ApprovalRoute_TransactionBaseClass
    {
        public List<b_ApprovalRoute> ApprovalRouteListForMaterialRequest { get; set; }

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
            List<b_ApprovalRoute> tmpArray = null;
            ApprovalRoute.ApprovalRoute_RetrieveForMaterialRequest_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            ApprovalRouteListForMaterialRequest = new List<b_ApprovalRoute>();
            foreach (b_ApprovalRoute tmpObj in tmpArray)
            {
                ApprovalRouteListForMaterialRequest.Add(tmpObj);
            }
        }
    }

    #endregion
}
