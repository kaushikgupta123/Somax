using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Database;
using Database.Business;
using Common.Structures;
using Database.SqlClient;
namespace DataContracts
{
    public partial class MeterReading : DataContractBase, IStoredProcedureValidation
    {
        #region Private Variables
        private bool m_validateProcess;
        private bool m_validateAdd;
        private bool m_validateMeterReadingAdd;
        private bool m_validateMeterReadingProcess;        
        #endregion
        #region Properties
        [DataMember]
        public long SiteId { get; set; }
        public decimal ReadingCurrent { get; set; }        
        public bool CreateMode { get; set; }
        public string ErrorMessagerow { get; set; }
        public string ReadByClientLookupId { get; set; }
        public string DateRead { get; set; }
        public string Validate_For { get; set; }
        public string meter_clientlookupid { get; set; }
        public List<MeterReading> MeterReadingList { get; set; }
      
        #endregion

       
        public static List<MeterReading> UpdateFromDatabaseObjectList(List<b_MeterReading> dbObjs)
        {
            List<MeterReading> result = new List<MeterReading>();

            foreach (b_MeterReading dbObj in dbObjs)
            {
                MeterReading tmp = new MeterReading();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromExtendedDatabaseObject(b_MeterReading dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.DateRead = dbObj.DateRead;
            this.ReadByClientLookupId = dbObj.ReadByClientLookupId;
            this.SiteId = dbObj.SiteId;
            this.meter_clientlookupid = dbObj.meter_clientlookupid;
        }

        public static List<MeterReading> RetriveByMeterId(DatabaseKey dbKey, MeterReading reading)
        {
            MeterReading_RetrieveByMeterId trans = new MeterReading_RetrieveByMeterId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.MeterReading = reading.ToDatabaseObject();
            trans.MeterReading = reading.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return MeterReading.UpdateFromDatabaseObjectList(trans.MeterReadingList);
        }

        public void Meter_Reading(DatabaseKey dbKey)
        {
          MeterReading_CreateReading trans = new MeterReading_CreateReading();
          trans.MeterReading = this.ToExtendedDatabaseObject();
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();
          // Validating before calling - this is not necessary here
          /*
          this.ValidateMeterReading(dbKey, loc);
          if (this.ErrorMessages.Count == 0)
          {
            MeterReading_CreateReading trans = new MeterReading_CreateReading();
            trans.MeterReading = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
          }
          */
          /*
          List<MeterReading> newReadingList = new List<MeterReading>();

            foreach (MeterReading meterreading in MeterReadingList)
            {
                meterreading.ValidateMeterReading(dbKey, loc);
            }

            newReadingList = MeterReadingList.FindAll(P => P.ErrorMessages.Count == 0);

            if (newReadingList.Count > 0)
            {
                MeterReading_CreateReading trans = new MeterReading_CreateReading();
                trans.MeterReading = this.ToExtendedDatabaseObject();
                trans.MeterReadingList = this.ToDatabaseObjectList(newReadingList);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
            }

            this.MeterReadingList = MeterReadingList.FindAll(P => P.ErrorMessages.Count > 0);
          */
        }

        public void ResetMeter(DatabaseKey dbKey)
        {
          ResetMeter_Create trans = new ResetMeter_Create();
          trans.MeterReading = this.ToExtendedDatabaseObject();
          trans.MeterReading.meter_clientlookupid = this.meter_clientlookupid;
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();
          /*
          List<MeterReading> ResetMeterList = new List<MeterReading>();

            foreach (MeterReading resetMeter in MeterReadingList)
            {
                resetMeter.ValidateMeterReading(dbKey, loc);
            }

            ResetMeterList = MeterReadingList.FindAll(P => P.ErrorMessages.Count == 0);

            if (ResetMeterList.Count > 0)
            {
                ResetMeter_Create trans = new ResetMeter_Create();
                trans.MeterReading = this.ToDatabaseObject();
                trans.resetMeterList = this.ToDatabaseObjectList(ResetMeterList);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
            }

            this.MeterReadingList = MeterReadingList.FindAll(P => P.ErrorMessages.Count > 0);
           */

        }
        public b_MeterReading ToExtendedDatabaseObject()
        {
            b_MeterReading dbObj = this.ToDatabaseObject();
            dbObj.ReadByClientLookupId = this.ReadByClientLookupId;
            dbObj.SiteId = this.SiteId;
            return dbObj;
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
          List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
          if (Validate_For == "MeterReadingAdd")
          {
            MeterReading_ValidateProcess trans = new MeterReading_ValidateProcess()
            {
              CallerUserInfoId = dbKey.User.UserInfoId,
              CallerUserName = dbKey.UserName,
            };
            trans.MeterReading = this.ToDatabaseObject();
            trans.MeterReading.MeterId = this.MeterId;
            trans.MeterReading.meter_clientlookupid = this.meter_clientlookupid;
            trans.MeterReading.SiteId = this.SiteId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();



            if (trans.StoredProcValidationErrorList != null)
            {
              foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
              {
                StoredProcValidationError tmp = new StoredProcValidationError();
                tmp.UpdateFromDatabaseObject(error);
                errors.Add(tmp);
              }
            }
          }
          else if (Validate_For == "ResetMeterAdd")
          {
            ResetMeter_ValidateProcess trans = new ResetMeter_ValidateProcess()
            {
              CallerUserInfoId = dbKey.User.UserInfoId,
              CallerUserName = dbKey.UserName,
            };
            trans.MeterReading = this.ToDatabaseObject();
            trans.MeterReading.meter_clientlookupid = this.meter_clientlookupid;
            trans.MeterReading.MeterReadingId = this.MeterReadingId;

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            if (trans.StoredProcValidationErrorList != null)
            {
              foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
              {
                StoredProcValidationError tmp = new StoredProcValidationError();
                tmp.UpdateFromDatabaseObject(error);
                errors.Add(tmp);
              }
            }
          }

          return errors;
        }

      public void ValidateMeterReadingAdd(DatabaseKey dbKey)
        {
            m_validateProcess = false;
            m_validateAdd = false;
            m_validateMeterReadingAdd = true;
            m_validateMeterReadingProcess = false;       

            Validate<MeterReading>(dbKey);
            //if (this.TransactionDate.HasValue && this.TransactionDate.Value.Date.CompareTo(DateTime.Now.AddDays(1).Date) > 0)
            //{
            //    SOMAX.G4.Business.Localization.ValidationError locValidationError =
            //        loc.StoredProcValidation.ValidationError.Find(v => v.Code == ISSUE_DATE_ERROR_CODE);
            //    if (locValidationError != null) { ErrorMessages.Add(locValidationError.Message); }
            //}
        }
        public void ValidateMeterReading(DatabaseKey dbKey)
        {
            m_validateProcess = false;
            m_validateAdd = false;
            m_validateMeterReadingAdd = false;
            m_validateMeterReadingProcess = true;


            Validate<MeterReading>(dbKey);
            //if (this.TransactionDate.HasValue && this.TransactionDate.Value.Date.CompareTo(DateTime.Now.AddDays(1).Date) > 0)
            //{
            //    SOMAX.G4.Business.Localization.ValidationError locValidationError =
            //        loc.StoredProcValidation.ValidationError.Find(v => v.Code == ISSUE_DATE_ERROR_CODE);
            //    if (locValidationError != null) { ErrorMessages.Add(locValidationError.Message); }
            //}
        }
       public void ValidateMeterReadingProcess(DatabaseKey dbKey)
        {

            Validate_For = "MeterReadingAdd";
            Validate<MeterReading>(dbKey);
            
        }

      public void ValidateResetMeterAdd(DatabaseKey dbKey)
        {
            Validate_For = "ResetMeterAdd";
            Validate<MeterReading>(dbKey);
        }

      public void Meter_Reading_Process(DatabaseKey dbKey)
      {
        List<MeterReading> newMeterReadingList = new List<MeterReading>();

        foreach (MeterReading meterreading in MeterReadingList)
        {
          meterreading.ValidateMeterReadingProcess(dbKey);
        }

        newMeterReadingList = MeterReadingList.FindAll(P => P.ErrorMessages.Count == 0);

        if (newMeterReadingList.Count > 0)
        {

          MeterReadingProcess_Create trans = new MeterReadingProcess_Create();
          trans.MeterReading = this.ToDatabaseObject();
          trans.PartHistoryList = this.ToDatabaseObjectList(newMeterReadingList);
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();
        }

        this.MeterReadingList = MeterReadingList.FindAll(P => P.ErrorMessages.Count > 0);

      }

      public List<b_MeterReading> ToDatabaseObjectList(List<MeterReading> partHistoryList)
      {
        List<b_MeterReading> dbObjs = new List<b_MeterReading>();

        foreach (MeterReading mr in partHistoryList)
        {
          dbObjs.Add(mr.ToExtendedDatabaseObject());
        }

        return dbObjs;
      }

    }
}
