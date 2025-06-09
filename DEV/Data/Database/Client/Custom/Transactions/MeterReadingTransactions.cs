using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Database;
using Common.Structures;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
  public class MeterReading_RetrieveByMeterId : MeterReading_TransactionBaseClass
  {
    public List<b_MeterReading> MeterReadingList { get; set; }

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
      List<b_MeterReading> tmpArray = null;

      MeterReading.RetrieveByMeterIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

      MeterReadingList = new List<b_MeterReading>();
      foreach (b_MeterReading tmpObj in tmpArray)
      {
        MeterReadingList.Add(tmpObj);
      }
    }
  }

  public class MeterReading_CreateReading : MeterReading_TransactionBaseClass
  {
    //  public List<b_MeterReading> MeterReadingList { get; set; }
    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
      if (MeterReading.MeterReadingId > 0)
      {
        string message = "MeterReading has an invalid ID.";
        throw new Exception(message);
      }
    }
    public override void PerformWorkItem()
    {
      MeterReading.CreateReading(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    }

    public override void Postprocess()
    {
      base.Postprocess();
      System.Diagnostics.Debug.Assert(MeterReading.MeterReadingId > 0);
    }
  }
  public class ResetMeter_Create : MeterReading_TransactionBaseClass
  {
    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
      if (MeterReading.MeterReadingId > 0)
      {
        string message = "Meter Reading has an invalid ID.";
        throw new Exception(message);
      }
    }
    public override void PerformWorkItem()
    {
      MeterReading.ResetDataIntoDataBase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    }

    public override void Postprocess()
    {
      //base.Postprocess();
      //System.Diagnostics.Debug.Assert(MeterReading.MeterReadingId > 0);
    }
  }
  public class MeterReading_ValidateProcess : MeterReading_TransactionBaseClass
  {
    public MeterReading_ValidateProcess()
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

        MeterReading.ValidateMeterReadingProcessFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
                ref errors);

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
    public long SiteId { get; set; }
  }
  public class ResetMeter_ValidateProcess : MeterReading_TransactionBaseClass
  {
    public ResetMeter_ValidateProcess()
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

        MeterReading.ValidateResetMeterProcessFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
                ref errors);

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
  public class MeterReadingProcess_Create : MeterReading_TransactionBaseClass
  {
    public List<b_MeterReading> PartHistoryList { get; set; }
    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
      if (MeterReading.MeterReadingId > 0)
      {
        string message = "Meter Reading has an invalid ID.";
        throw new Exception(message);
      }
    }
    public override void PerformWorkItem()
    {
      foreach (b_MeterReading parthistory in PartHistoryList)
      {
        MeterReading.ClientId = dbKey.Client.ClientId;
        MeterReading.MeterId = parthistory.MeterId;
        MeterReading.Reading = parthistory.Reading;
        MeterReading.ReadingDate = parthistory.ReadingDate;
        MeterReading.ReadByClientLookupId = parthistory.ReadByClientLookupId;
        MeterReading.Reset = parthistory.Reset;
        MeterReading.CreateReading(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
      }
    }

    public override void Postprocess()
    {
      //base.Postprocess();
      //System.Diagnostics.Debug.Assert(MeterReading.MeterReadingId > 0);
    }
  }
}





