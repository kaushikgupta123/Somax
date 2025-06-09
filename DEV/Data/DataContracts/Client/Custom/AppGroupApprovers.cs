using Database;
using Database.Business;

using System.Collections.Generic;

namespace DataContracts
{
    public partial class AppGroupApprovers : DataContractBase, IStoredProcedureValidation
    {
        #region property
        public List<AppGroupApprovers> listOfAppGroupApprovers { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string ApproverName { get; set; }
        public int TotalCount { get; set; }
        public string LevelName { get; set; }

        public string ValidateFor = string.Empty;
        #region V2-726
        public long RequestorId { get; set; }
        public long SiteId { get; set; }
        public string RequestType { get; set; }
        #endregion
        #region V2-730
        public long ObjectId { get; set; }
        #endregion
        #endregion

        #region Child Grid
        public AppGroupApprovers AppGroupRetrieveByApprovalGroupIdV2(DatabaseKey dbKey, string TimeZone)
        {
            AppGroupApprovers_RetrieveByApprovalGroupIdV2 trans = new AppGroupApprovers_RetrieveByApprovalGroupIdV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.AppGroupApprovers = this.ToDateBaseObjectForChildGrid();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfAppGroupApprovers = new List<AppGroupApprovers>();


            List<AppGroupApprovers> AppGroupApproverslist = new List<AppGroupApprovers>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_AppGroupApprovers ApprovalGroup in trans.AppGroupApprovers.listOfAppGroupApprovers)
            {
                AppGroupApprovers tmpAppGroupApprovers = new AppGroupApprovers();

                tmpAppGroupApprovers.UpdateFromDatabaseObjectForChildGrid(ApprovalGroup);
                AppGroupApproverslist.Add(tmpAppGroupApprovers);
            }
            this.listOfAppGroupApprovers.AddRange(AppGroupApproverslist);
            return this;
        }
        public b_AppGroupApprovers ToDateBaseObjectForChildGrid()
        {
            b_AppGroupApprovers dbObj = new b_AppGroupApprovers();
            dbObj.ClientId = this.ClientId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ApprovalGroupId = this.ApprovalGroupId;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForChildGrid(b_AppGroupApprovers dbObj)
        {
            this.AppGroupApproversId = dbObj.AppGroupApproversId;
            this.Limit = dbObj.Limit;
            this.Level = dbObj.Level;
            this.ApproverName = dbObj.ApproverName;
            this.TotalCount = dbObj.TotalCount;

            AuditEnabled = true;
        }
        #endregion

        #region Details
        public List<AppGroupApprovers> AppGroupApproverDetailsChunkSearch(DatabaseKey dbKey, string TimeZone)
        {
            AppGroupApprovers_RetrieveChunkSearchFromDetails trans = new AppGroupApprovers_RetrieveChunkSearchFromDetails()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.AppGroupApprovers = ToDateBaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();


            List<AppGroupApprovers> AppGroupApproverslist = new List<AppGroupApprovers>();
            foreach (b_AppGroupApprovers ApprovalGroup in trans.appGroupApproversList)// .AppGroupApprovers.listOfAppGroupApprovers)
            {
                AppGroupApprovers tmpAppGroupApprovers = new AppGroupApprovers();

                tmpAppGroupApprovers.UpdateFromDatabaseObjectExtended(ApprovalGroup);
                AppGroupApproverslist.Add(tmpAppGroupApprovers);
            }
            return AppGroupApproverslist;
        }
        public b_AppGroupApprovers ToDateBaseObjectExtended()
        {
            b_AppGroupApprovers dbObj = this.ToDatabaseObject();
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            
            return dbObj;
        }

        public void UpdateFromDatabaseObjectExtended(b_AppGroupApprovers dbObj)
        {
            UpdateFromDatabaseObject(dbObj);

            this.ApproverName = dbObj.ApproverName;
            this.TotalCount = dbObj.TotalCount;
            this.LevelName = dbObj.LevelName;
        }

        public void RetrieveById_V2(DatabaseKey dbKey)
        {
            AppGroupApprovers_RetrieveById_V2 trans = new AppGroupApprovers_RetrieveById_V2();
            trans.AppGroupApprovers = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectById(trans.AppGroupApprovers);
        }

        public void UpdateFromDatabaseObjectById(b_AppGroupApprovers dbObj)
        {
            UpdateFromDatabaseObject(dbObj);
            this.ApproverName = dbObj.ApproverName;
            this.LevelName = dbObj.LevelName;
        }

        #endregion

        

        #region V2-726
        public List<AppGroupApprovers> RetrieveApproversForApproval(DatabaseKey dbKey)
        {
            AppGroupApprovers_RetrieveApproversForApproval trans = new AppGroupApprovers_RetrieveApproversForApproval()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.AppGroupApprovers = this.ToDatabaseObjectExtendedByRequestorIdAndRequestType();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            return (UpdateFromDatabaseObjectlist(trans.ApproverList));
        }
        public b_AppGroupApprovers ToDatabaseObjectExtendedByRequestorIdAndRequestType()
        {
            b_AppGroupApprovers dbObj = this.ToDatabaseObject();
            dbObj.RequestorId = this.RequestorId;
            dbObj.SiteId = this.SiteId;
            dbObj.RequestType = this.RequestType;
            dbObj.Level=this.Level;
            return dbObj;
        }
        public List<AppGroupApprovers> UpdateFromDatabaseObjectlist(List<b_AppGroupApprovers> dbObjlist)
        {
            List<AppGroupApprovers> temp = new List<AppGroupApprovers>();

            AppGroupApprovers objPer;

            foreach (b_AppGroupApprovers per in dbObjlist)
            {
                objPer = new AppGroupApprovers();
                //objPer.UpdateFromDatabaseObject(per);
                objPer.ApproverId= per.ApproverId;
                objPer.ApproverName= per.ApproverName;
                temp.Add(objPer);
            }

            return (temp);


        }
        #endregion

        #region V2-720
        public void CheckvalidateApproverId(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateApproverId";
            Validate<AppGroupApprovers>(dbKey);
        }
        public void CheckValidateUpperLevelExists(DatabaseKey dbKey)
        {
            Validate<AppGroupApprovers>(dbKey);
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            if (ValidateFor == "ValidateApproverId")
            {

                AppGroupApprovers_ValidateApproverId trans = new AppGroupApprovers_ValidateApproverId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.AppGroupApprovers = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);

            }
            else
            {
                AppGroupApprovers_ValidateUpperLevelExists_V2 trans = new AppGroupApprovers_ValidateUpperLevelExists_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.AppGroupApprovers = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);
            }
            return errors;
        }
        #endregion

        #region V2-730
        public List<AppGroupApprovers> RetrieveApproversForMultiLevelApproval(DatabaseKey dbKey)
        {
            AppGroupApprovers_RetrieveApproversForMultiLevelApproval trans = new AppGroupApprovers_RetrieveApproversForMultiLevelApproval()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.AppGroupApprovers = this.ToDatabaseObjectExtendedForMultiLevel();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            return (UpdateFromDatabaseObjectlistForMultiLevel(trans.ApproverList));
        }
        public b_AppGroupApprovers ToDatabaseObjectExtendedForMultiLevel()
        {
            b_AppGroupApprovers dbObj = this.ToDatabaseObject();
            dbObj.ObjectId = this.ObjectId;
            dbObj.ApproverId = this.ApproverId;
            dbObj.ApprovalGroupId = this.ApprovalGroupId;
            dbObj.RequestType = this.RequestType;
            return dbObj;
        }
        public List<AppGroupApprovers> UpdateFromDatabaseObjectlistForMultiLevel(List<b_AppGroupApprovers> dbObjlist)
        {
            List<AppGroupApprovers> temp = new List<AppGroupApprovers>();

            AppGroupApprovers objPer;

            foreach (b_AppGroupApprovers per in dbObjlist)
            {
                objPer = new AppGroupApprovers();
                //objPer.UpdateFromDatabaseObject(per);
                objPer.ApproverId = per.ApproverId;
                objPer.ApproverName = per.ApproverName;
                temp.Add(objPer);
            }

            return (temp);


        }
        #endregion
    }
}
