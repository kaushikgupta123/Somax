using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using System.Data.SqlClient;
using Database.StoredProcedure;

namespace Database.Client.Custom.Transactions
{
    public class EstimatedCostsTransactions :EstimatedCosts_TransactionBaseClass
    {
       

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
           
        }
    }

    public class EstimatedCosts_RetrieveByObjectId :EstimatedCosts_TransactionBaseClass
    {
        public List<b_EstimatedCosts> EstimatatedCostsList { get; set; }

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
            List<b_EstimatedCosts> tmpArray = null;
            

            EstimatedCosts.RetrieveByObjectIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            EstimatatedCostsList = new List<b_EstimatedCosts>();
            foreach (b_EstimatedCosts tmpObj in tmpArray)
            {
                EstimatatedCostsList.Add(tmpObj);
            }
        }
    }

    public class EstimatedCosts_SummeryRetrieveByObjectId : EstimatedCosts_TransactionBaseClass
    {
        public List<b_EstimatedCosts> EstimatatedCostsList { get; set; }
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
            List<b_EstimatedCosts> tmpArray = null;


            EstimatedCosts.SummeryRetrieveByObjectIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            EstimatatedCostsList = new List<b_EstimatedCosts>();
            foreach (b_EstimatedCosts tmpObj in tmpArray)
            {
                EstimatatedCostsList.Add(tmpObj);
            }
        }

    }

    /*ADDED BY INDISNET TECHNOLOGIES*/
    public class EstimatedCosts_RetrieveForPrevMaintFromDatabase : EstimatedCosts_TransactionBaseClass
    {
        public List<b_EstimatedCosts> EstimatatedCostsList { get; set; }
        public long PrevMaintMasterId { get; set; }
        public string Category { get; set; }
        public long Client { get; set; }

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
            List<b_EstimatedCosts> tmpArray = null;


            EstimatedCosts.RetrieveForPrevMaintFromDatabase(this.Connection, this.Transaction, CallerUserInfoId
                , CallerUserName, Client, PrevMaintMasterId, Category, ref tmpArray);

            EstimatatedCostsList = new List<b_EstimatedCosts>();
            foreach (b_EstimatedCosts tmpObj in tmpArray)
            {
                EstimatatedCostsList.Add(tmpObj);
            }
        }

    }

  public class EstimatedCost_ValidateAddTransaction : EstimatedCosts_TransactionBaseClass
  {
    public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();

    }
    public override void PerformWorkItem()
    {
      List<b_StoredProcValidationError> errors = null;
      EstimatedCosts.EstimateCostValidateAdd(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
      StoredProcValidationErrorList = errors;
    }

    public override void Postprocess()
    {

    }
  }


  public class EstimatedCost_ValidateUpdateTransaction : EstimatedCosts_TransactionBaseClass
  {
    public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();

    }
    public override void PerformWorkItem()
    {
      List<b_StoredProcValidationError> errors = null;
      EstimatedCosts.EstimateCostValidateUpdate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
      StoredProcValidationErrorList = errors;
    }

    public override void Postprocess()
    {

    }
  }

    #region V2-691
    public class EstimatedCosts_RetrieveForChildGridByObjectId : EstimatedCosts_TransactionBaseClass
    {
        public List<b_EstimatedCosts> EstimatedCostsList { get; set; }

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
            List<b_EstimatedCosts> tmpArray = null;

            EstimatedCosts.EstimatedCosts_RetrieveForChildGridByObjectId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            EstimatedCostsList = new List<b_EstimatedCosts>();
            foreach (b_EstimatedCosts tmpObj in tmpArray)
            {
                EstimatedCostsList.Add(tmpObj);
            }
        }
    }
    #endregion
    #region V2-690
    public class EstimatedCosts_CreateFromShoppingCart : EstimatedCosts_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (EstimatedCosts.EstimatedCostsId > 0)
            {
                string message = "WorkOrderLineItem has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            EstimatedCosts.CreateFromShoppingCart(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(EstimatedCosts.EstimatedCostsId > 0);
        }
    }
    #endregion
    #region V2-732
    public class EstimatedCosts_CreateFromShoppingCartMultiStoreroom : EstimatedCosts_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (EstimatedCosts.EstimatedCostsId > 0)
            {
                string message = "WorkOrderLineItem has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            EstimatedCosts.CreateFromShoppingCartMultiStoreroom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(EstimatedCosts.EstimatedCostsId > 0);
        }
    }
    #endregion

    #region V2-1204
    public class EstimatedCosts_RetrieveByObjectId_V2 : EstimatedCosts_TransactionBaseClass
    {
        public List<b_EstimatedCosts> EstimatatedCostsList { get; set; }

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
            List<b_EstimatedCosts> tmpArray = null;


            EstimatedCosts.RetrieveByObjectId_V2FromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            EstimatatedCostsList = new List<b_EstimatedCosts>();
            foreach (b_EstimatedCosts tmpObj in tmpArray)
            {
                EstimatatedCostsList.Add(tmpObj);
            }
        }
    }
    #endregion
}




