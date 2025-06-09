using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Reflection;

using Database;
using Database.Business;

namespace DataContracts
{

  public partial class SanitationRequest : DataContractBase, IStoredProcedureValidation
  {
    #region Constructors
    /// <summary>
    /// Default constructor.
    /// </summary>
    public SanitationRequest()
    {
      Initialize();
    }
    #endregion
    public void Clear()
    {
      Initialize();
    }


    #region Private Variables
    private int m_ValidateType;   // 1 - Add, 2 - Save
    #endregion

    #region constants
    private const int Validate_Add = 1;
    private const int Validate_Save = 2;
    #endregion

    #region Supporting method Properties

    string ValidateFor = string.Empty;

    public bool CreateMode { get; set; }
    #endregion

    #region Properties
    public string ChargeToClientLookupId { get; set; }
    public long PlantLocationId { get; set; }
    public string WOExClientLookupId { get; set; }


    public string Creator_PersonnelClientLookupId { get; set; }
    public int FlagSourceType { get; set; }

    public long ClientId { get; set; }

    public long SanitationJobId { get; set; }

    public long SanitationMasterId { get; set; }

    public long AreaId { get; set; }

    public long SiteId { get; set; }

    public long DepartmentId { get; set; }

    public long StoreroomId { get; set; }

    public string ClientLookupId { get; set; }

    public string SourceType { get; set; }
    public long SourceId { get; set; }
    public decimal ActualDuration { get; set; }

    public long AssignedTo_PersonnelId { get; set; }

    public string CancelReason { get; set; }

    public long ChargeToId { get; set; }

    public string ChargeType { get; set; }

    public string ChargeTo_Name { get; set; }

    public long CompleteBy_PersonnelId { get; set; }

    public string CompleteComments { get; set; }

    public DateTime? CompleteDate { get; set; }

    public string Description { get; set; }

    public string Shift { get; set; }

    public bool DownRequired { get; set; }

    public DateTime? ScheduledDate { get; set; }

    public decimal ScheduledDuration { get; set; }

    public string Status { get; set; }

    public long Creator_PersonnelId { get; set; }

    public int UpdateIndex { get; set; }
    //SOM-1334
    public long SanOnDemandMasterId { get; set; }
    public DateTime? RequiredDate { get; set; }
        #endregion

        private void Initialize()
    {
      b_SanitationJob dbObj = new b_SanitationJob();
      UpdateFromDatabaseObject(dbObj);

      // Turn off auditing for object initialization
      AuditEnabled = false;
    }

    //===========================================================================================================

    #region Transaction Methods


    public void Add_SanitationRequest(DatabaseKey dbKey)
    {
      Validate<SanitationRequest>(dbKey);

      if (IsValid)
      {
        SanitationRequest_Create trans = new SanitationRequest_Create()
        {
          CallerUserInfoId = dbKey.User.UserInfoId,
          CallerUserName = dbKey.UserName,
        };
        trans.SanitationJob = this.ToDatabaseObjectSanitationJob();
        trans.SanitationJob.FlagSourceType = this.FlagSourceType;
        trans.dbKey = dbKey.ToTransDbKey();
        trans.Execute();

        // The create procedure may have populated an auto-incremented key. 
        UpdateFromDatabaseObject(trans.SanitationJob);
      }
    }

    public void UpdateFromDatabaseObject(b_SanitationJob dbObj)
    {
      this.ClientId = dbObj.ClientId;
      this.SanitationJobId = dbObj.SanitationJobId;
      this.SanitationMasterId = dbObj.SanitationMasterId;
      this.AreaId = dbObj.AreaId;
      this.SiteId = dbObj.SiteId;
      this.DepartmentId = dbObj.DepartmentId;
      this.StoreroomId = dbObj.StoreroomId;
      this.ClientLookupId = dbObj.ClientLookupId;
      this.SourceType = dbObj.SourceType;
      this.SourceId = dbObj.SourceId;
      this.ActualDuration = dbObj.ActualDuration;
      this.AssignedTo_PersonnelId = dbObj.AssignedTo_PersonnelId;
      this.CancelReason = dbObj.CancelReason;
      this.ChargeToId = dbObj.ChargeToId;
      this.ChargeType = dbObj.ChargeType;
      this.ChargeTo_Name = dbObj.ChargeTo_Name;
      this.CompleteBy_PersonnelId = dbObj.CompleteBy_PersonnelId;
      this.CompleteComments = dbObj.CompleteComments;
      this.CompleteDate = dbObj.CompleteDate;
      this.Description = dbObj.Description;
      this.Shift = dbObj.Shift;
      this.DownRequired = dbObj.DownRequired;
      this.ScheduledDate = dbObj.ScheduledDate;
      this.ScheduledDuration = dbObj.ScheduledDuration;
      this.Status = dbObj.Status;
      this.Creator_PersonnelId = dbObj.Creator_PersonnelId;
      this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
      this.PlantLocationId = dbObj.PlantLocationId;
      this.SanOnDemandMasterId = dbObj.SanOnDemandMasterId;
      this.UpdateIndex = dbObj.UpdateIndex;



      //this.ApproveBy_PersonnelId = dbObj.ApproveBy_PersonnelId;
      //this.ApproveDate = dbObj.ApproveDate;
      //this.DeniedBy_PersonnelId = dbObj.DeniedBy_PersonnelId;
      //this.DeniedDate = dbObj.DeniedDate;
      //this.DeniedReason = dbObj.DeniedReason;
      //this.DeniedComment = dbObj.DeniedComment;
      //this.PassBy_PersonnelId = dbObj.PassBy_PersonnelId;
      //this.PassDate = dbObj.PassDate;
      //this.FailBy_PersonnelId = dbObj.FailBy_PersonnelId;
      //this.FailDate = dbObj.FailDate;
      //this.FailReason = dbObj.FailReason;
      //this.FailComment = dbObj.FailComment;
      //this.Extracted = dbObj.Extracted;
      //this.ExportLogId = dbObj.ExportLogId;



      // Turn on auditing
      AuditEnabled = true;
    }

    private b_SanitationJob ToDatabaseObjectSanitationJob()
    {
            b_SanitationJob dbObj = new b_SanitationJob();
            dbObj.ClientId = this.ClientId;
            dbObj.SanitationJobId = this.SanitationJobId;
            dbObj.AreaId = this.AreaId;
            dbObj.SiteId = this.SiteId;
            dbObj.DepartmentId = this.DepartmentId;
            dbObj.StoreroomId = this.StoreroomId;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.SourceType = this.SourceType;
            dbObj.SourceId = this.SourceId;
            dbObj.ActualDuration = this.ActualDuration;
            dbObj.AssignedTo_PersonnelId = this.AssignedTo_PersonnelId;
            dbObj.CancelReason = this.CancelReason;
            dbObj.ChargeToId = this.ChargeToId;
            dbObj.ChargeType = this.ChargeType;
            dbObj.ChargeTo_Name = this.ChargeTo_Name;
            if (this.WOExClientLookupId == null || this.WOExClientLookupId == "")
            {
                dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
            }
            else
            {
                dbObj.ChargeToClientLookupId = this.WOExClientLookupId;
            }
            dbObj.PlantLocationId = this.PlantLocationId;
            dbObj.Creator_PersonnelClientLookupId = this.Creator_PersonnelClientLookupId;
            dbObj.CompleteBy_PersonnelId = this.CompleteBy_PersonnelId;
            dbObj.CompleteComments = this.CompleteComments;
            dbObj.CompleteDate = this.CompleteDate;
            dbObj.Description = this.Description;
            dbObj.Shift = this.Shift;
            dbObj.DownRequired = this.DownRequired;
            dbObj.ScheduledDate = this.ScheduledDate;
            dbObj.ScheduledDuration = this.ScheduledDuration;
            dbObj.Status = this.Status;
            dbObj.Creator_PersonnelId = this.Creator_PersonnelId;
            dbObj.UpdateIndex = this.UpdateIndex;
            dbObj.SanOnDemandMasterId = this.SanOnDemandMasterId;//SOM-1334
            dbObj.RequiredDate = this.RequiredDate;
            return dbObj;

        }
    private b_SanitationJob ToDatabaseObjectSanitationJobValid()
    {
      b_SanitationJob dbObj = new b_SanitationJob();
      dbObj.ClientId = this.ClientId;
      dbObj.SanitationJobId = this.SanitationJobId;
      dbObj.AreaId = this.AreaId;
      dbObj.SiteId = this.SiteId;
      dbObj.DepartmentId = this.DepartmentId;
      dbObj.StoreroomId = this.StoreroomId;
      dbObj.ClientLookupId = this.ClientLookupId;
      dbObj.SourceType = this.SourceType;
      dbObj.SourceId = this.SourceId;
      dbObj.ActualDuration = this.ActualDuration;
      dbObj.AssignedTo_PersonnelId = this.AssignedTo_PersonnelId;
      dbObj.CancelReason = this.CancelReason;
      dbObj.ChargeToId = this.ChargeToId;
      dbObj.ChargeType = this.ChargeType;
      dbObj.ChargeTo_Name = this.ChargeTo_Name;
      if (this.WOExClientLookupId == null || this.WOExClientLookupId == "")
      {
        dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;

      }
      else
      {
        dbObj.WOExClientLookupId = this.WOExClientLookupId;

      }
      dbObj.PlantLocationId = this.PlantLocationId;
      dbObj.Creator_PersonnelClientLookupId = this.Creator_PersonnelClientLookupId;
      dbObj.CompleteBy_PersonnelId = this.CompleteBy_PersonnelId;
      dbObj.CompleteComments = this.CompleteComments;
      dbObj.CompleteDate = this.CompleteDate;
      dbObj.Description = this.Description;
      dbObj.Shift = this.Shift;
      dbObj.DownRequired = this.DownRequired;
      dbObj.ScheduledDate = this.ScheduledDate;
      dbObj.ScheduledDuration = this.ScheduledDuration;
      dbObj.Status = this.Status;
      dbObj.Creator_PersonnelId = this.Creator_PersonnelId;
      dbObj.UpdateIndex = this.UpdateIndex;
      dbObj.SanOnDemandMasterId = this.SanOnDemandMasterId;//SOM-1334
      return dbObj;

    }


    #endregion


    #region Validation Methods
    public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
    {
      List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

      SanitationJob_ValidateByClientlookupIdAndPersonnelId trans = new SanitationJob_ValidateByClientlookupIdAndPersonnelId()
      {
        CallerUserInfoId = dbKey.User.UserInfoId,
        CallerUserName = dbKey.UserName,
      };
      trans.SanitationJob = this.ToDatabaseObjectSanitationJobValid();

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
      return errors;
    }

    #endregion



    //SOM-1334
    public void Add_SanitationJobOnDemandjobsAndRequests(DatabaseKey dbKey)
    {
      Validate<SanitationRequest>(dbKey);

      if (IsValid)
      {
        SanitationJobOnDemandJobAndRequest_Create trans = new SanitationJobOnDemandJobAndRequest_Create()
        {
          CallerUserInfoId = dbKey.User.UserInfoId,
          CallerUserName = dbKey.UserName,
        };
        trans.SanitationJob = this.ToDatabaseObjectSanitationJob();
        trans.SanitationJob.FlagSourceType = this.FlagSourceType;
        trans.dbKey = dbKey.ToTransDbKey();
        trans.Execute();

        // The create procedure may have populated an auto-incremented key. 
        UpdateFromDatabaseObject(trans.SanitationJob);
      }
    }

        public void AddEXSanitationRequest(DatabaseKey dbKey)
        {
            if (this.FlagSourceType == 1)
            {
                SanitationRequest_EXCreate trans = new SanitationRequest_EXCreate()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.SanitationJob = this.ToDatabaseObjectSanitationJob();
                trans.SanitationJob.FlagSourceType = this.FlagSourceType;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.SanitationJob);
                this.ErrorMessages = new List<string>();

            }
            else
            {
                Validate<SanitationRequest>(dbKey);
                if (IsValid)
                {
                    SanitationRequest_EXCreate trans = new SanitationRequest_EXCreate()
                    {
                        CallerUserInfoId = dbKey.User.UserInfoId,
                        CallerUserName = dbKey.UserName,
                    };
                    trans.SanitationJob = this.ToDatabaseObjectSanitationJob();
                    trans.SanitationJob.FlagSourceType = this.FlagSourceType;
                    trans.dbKey = dbKey.ToTransDbKey();
                    trans.Execute();

                    // The create procedure may have populated an auto-incremented key. 
                    UpdateFromDatabaseObject(trans.SanitationJob);
                }
            }
        }
    }
}
