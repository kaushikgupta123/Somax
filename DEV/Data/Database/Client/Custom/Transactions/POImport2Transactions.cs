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

  public class POImport2_ValidateByPOImport2Id : POImport2_TransactionBaseClass
  {
    public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();

    }
    public override void PerformWorkItem()
    {
      List<b_StoredProcValidationError> errors = null;
      POImport2.ValidateByPOImport2Id(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
      StoredProcValidationErrorList = errors;
    }

    public override void Postprocess()
    {

    }
  }
  public class POImport2_RetrieveForImportCompare : POImport2_TransactionBaseClass
  {

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
    }

    public override void PerformWorkItem()
    {
      base.UseTransaction = false;
      POImport2.RetrieveForImportCompare(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    }
  }
  public class POImport2_ProcessInterface : POImport2_TransactionBaseClass
  {

    public POImport2_ProcessInterface()
    {
    }

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();

    }

    // Result Sets
    public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
    public long PersonnelId { get; set; }

    public override void PerformWorkItem()
    {
      SqlCommand command = null;
      string message = String.Empty;

      try
      {
        List<b_StoredProcValidationError> errors = null;
        POImport2.poImport2_ProcessInterface(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, PersonnelId);

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
