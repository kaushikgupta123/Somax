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

    public class ShoppingCartLineItem_UpdateLineItem : ShoppingCartLineItem_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ShoppingCartLineItem.ShoppingCartLineItemId == 0)
            {
                string message = "ShoppingCartLineItem has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            ShoppingCartLineItem.UpdateLineItem(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class ShoppingCartLineItem_CreateByFK : ShoppingCartLineItem_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ShoppingCartLineItem.ShoppingCartLineItemId > 0)
            {
                string message = "ShoppingCartLineItems has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            ShoppingCartLineItem.InsertByForeignKeysIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(ShoppingCartLineItem.ShoppingCartLineItemId > 0);
        }
    }
    public class ShoppingCartLineItem_RetrieveByShoppingCartLineItemId : ShoppingCartLineItem_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            ShoppingCartLineItem.ShoppingCartLineItem_RetrieveByShoppingCartLineItemId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class ShoppingCartLineItem_ReOrderLineNumber : ShoppingCartLineItem_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            ShoppingCartLineItem.ReOrderLineNumber(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(ShoppingCartLineItem.ShoppingCartLineItemId > 0);
        }
    }
    public class ShoppingCart_Retrieve_ForShoppingCart : ShoppingCartLineItem_TransactionBaseClass
    {
        public List<b_ShoppingCartLineItem> ShoppingCartLineItemList { get; set; }
        public override void Preprocess()
        {
        }

        public override void Postprocess()
        {
        }

        public override void PerformWorkItem()
        {
            b_ShoppingCartLineItem[] tmpArray = null;
            ShoppingCartLineItem.ClientId = this.dbKey.Client.ClientId;
            ShoppingCartLineItem.RetrieveShoppingCartLineItem(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            ShoppingCartLineItemList = new List<b_ShoppingCartLineItem>(tmpArray);
        }
    }
    //SOM-1513
    public class ShoppingCart_RetrieveShoppingCartListForPart : ShoppingCartLineItem_TransactionBaseClass
    {
        public List<b_ShoppingCartLineItem> ShoppingCartLineItemList { get; set; }
        public override void Preprocess()
        {
        }

        public override void Postprocess()
        {
        }

        public override void PerformWorkItem()
        {
            b_ShoppingCartLineItem[] tmpArray = null;
            ShoppingCartLineItem.RetrieveShoppingCartListForPart(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            ShoppingCartLineItemList = new List<b_ShoppingCartLineItem>(tmpArray);
        }
    }
    public class ShoppingCartLineItem_RetrieveByPurchaseOrderLineItem : ShoppingCartLineItem_TransactionBaseClass
    {
        public List<b_ShoppingCartLineItem> ShoppingCartLineList { get; set; }

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
            List<b_ShoppingCartLineItem> tmpArray = null;

            ShoppingCartLineItem.RetrieveByShoppingCartLineItemFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            ShoppingCartLineList = new List<b_ShoppingCartLineItem>();
            foreach (b_ShoppingCartLineItem tmpObj in tmpArray)
            {
                ShoppingCartLineList.Add(tmpObj);
            }
        }
    }
    public class ShoppingCartLineItem_Validate_NonStockItem : ShoppingCartLineItem_TransactionBaseClass
    {
        public ShoppingCartLineItem_Validate_NonStockItem()
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
                ShoppingCartLineItem.ValidateNonStock(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);
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

  public class ShoppingCartLineItem_Validate_Catalog : ShoppingCartLineItem_TransactionBaseClass
  {
    public ShoppingCartLineItem_Validate_Catalog()
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
        ShoppingCartLineItem.ValidateCatalog(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);
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

  public class ShoppingCartLineItem_Validate_NonCatalog : ShoppingCartLineItem_TransactionBaseClass
  {
    public ShoppingCartLineItem_Validate_NonCatalog()
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
        ShoppingCartLineItem.ValidateNonCatalog(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);
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

}

