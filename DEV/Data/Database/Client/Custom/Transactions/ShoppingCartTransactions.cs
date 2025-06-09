using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
    public class ShoppingCart_Validate_ByClientlookupIdAndPersonnelId : ShoppingCart_TransactionBaseClass
    {

        public ShoppingCart_Validate_ByClientlookupIdAndPersonnelId()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;
                ShoppingCart.ShoppingCart_ValidateByClientLookupId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

                StoredProcValidationErrorList = errors;
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }

    }

        public class ShoppingCart_RetrieveAllData : ShoppingCart_TransactionBaseClass
        {

        public ShoppingCart_RetrieveAllData()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_ShoppingCart> ShpingCartList { get; set; }

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
            b_ShoppingCart[] tmpArray = null;
            // Explicitly set tenant id from dbkey
            this.ShoppingCart.ClientId = this.dbKey.Client.ClientId;
            this.ShoppingCart.RetrieveAll(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            ShpingCartList = new List<b_ShoppingCart>(tmpArray);
        }
    }
    public class ShoppingCart_UpdateByPKForeignKeys : ShoppingCart_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ShoppingCart.ShoppingCartId == 0)
            {
                string message = "ShoppingCartId has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            ShoppingCart.UpdateByPKForeignKeysInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    public class ShoppingCart_ConvertToPurchaseRequest : ShoppingCart_TransactionBaseClass
    {

        public ShoppingCart_ConvertToPurchaseRequest()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;
                ShoppingCart.ShoppingCart_ConvertToPurchaseRequest(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName);

                StoredProcValidationErrorList = errors;
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }

    }

    public class ShoppingCart_RetrieveForReviewWorkBenchSearch : ShoppingCart_TransactionBaseClass
    {

        public List<b_ShoppingCart> ShoppingCartList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            
        }
        public override void PerformWorkItem()
        {
            List<b_ShoppingCart> tmpList = null;
            ShoppingCart.ReviewWorkbenchSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            ShoppingCartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

        public string Created { get; set; }

        public string StatusDrop { get; set; }

        public long UserInfoId { get; set; }
    }
    public class ShoppingCart_RetrieveByPK : ShoppingCart_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ShoppingCart.ShoppingCartId == 0)
            {
                string message = "ShoppingCart has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            ShoppingCart.RetrieveByShoppingCartId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class ShoppingCart_RetrieveAllWorkbenchSearch : ShoppingCart_TransactionBaseClass
    {
        public List<b_ShoppingCart> ShoppingCartList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            
        }
        public override void PerformWorkItem()
        {
            List<b_ShoppingCart> tmpList = null;
            ShoppingCart.RetrieveAllWorkbenchSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            ShoppingCartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

        public string Created { get; set; }

        public string StatusDrop { get; set; }

        public long UserInfoId { get; set; }
    }
    public class ShoppingCart_RetrieveForNotification : ShoppingCart_TransactionBaseClass
    {

      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();
        if (ShoppingCart.ShoppingCartId == 0)
        {
          string message = "ShoppingCart has an invalid ID.";
          throw new Exception(message);
        }
      }

      public override void PerformWorkItem()
      {
        base.UseTransaction = false;
        ShoppingCart.RetrieveForNotification(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
      }
    }
    public class ShoppingCart_AutoGeneration : ShoppingCart_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            
        }
        public override void PerformWorkItem()
        {
            ShoppingCart.AutoGeneration(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
           
        }
    }
    //public class ShoppingCart_RetrieveForAutoGenNotification : ShoppingCart_TransactionBaseClass
    //{

    //    public b_Shopping Shop { get; set; }
    //    public override void PerformLocalValidation()
    //    {
    //        base.PerformLocalValidation();
    //    }
    //    public override void PerformWorkItem()
    //    {
    //        b_Shopping tmpList = null;
    //        b_Shopping shop=new b_Shopping();
    //        shop.Cart = Shop.Cart;
    //        shop.RetrieveForAutoGenNotification(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref shop);
    //        Shop = shop;
           
    //    }

    //    public override void Postprocess()
    //    {
    //        base.Postprocess();
    //    }
    //}

}
