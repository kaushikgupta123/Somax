using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class ServiceTasks : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string SearchText { get; set; }
        public Int32 TotalCount { get; set; }
        public List<ServiceTasks> listOfServiceTask { get; set; }

        public string ValidateFor = string.Empty;
        public string Flag { get; set; }
        #endregion
        #region Transaction
        #region CheckDuplicate
        public void CheckDuplicateServiceTask(DatabaseKey dbKey)
        {
            ValidateFor = "CheckDuplicate";
            Validate<ServiceTasks>(dbKey);
        }
        public void CheckServiceTaskIsInactivateorActivate(DatabaseKey dbKey)
        {
            ValidateFor = "CheckIfInactivateorActivate";
            Validate<ServiceTasks>(dbKey);
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            //--------Check For Equipment is Parent of another-------------------------------------------------
            if (ValidateFor == "CheckDuplicate")
            {
                ServiceTasks_ValidateByClientLookupId trans = new ServiceTasks_ValidateByClientLookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.ServiceTasks = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);


            }
            else if (ValidateFor == "CheckIfInactivateorActivate")
            {

                ServiceTasks_ValidateByInactivateorActivate trans = new ServiceTasks_ValidateByInactivateorActivate()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.ServiceTasks = this.ToDatabaseObject();
                trans.ServiceTasks.Flag = this.Flag;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);

            }


            return errors;
        }

        #endregion
        #region Search
        public List<ServiceTasks> ServiceTaskRetrieveChunkSearchV2(DatabaseKey dbKey, string TimeZone)
        {
            ServiceTask_RetrieveFleetAssetChunkSearchV2 trans = new ServiceTask_RetrieveFleetAssetChunkSearchV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ServiceTasks = this.ToDateBaseObjectForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            //this.utilityAdd = new UtilityAdd();
            this.listOfServiceTask = new List<ServiceTasks>();

            
            List<ServiceTasks> ServiceTaskslist = new List<ServiceTasks>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_ServiceTasks Service in trans.ServiceTasks.listOfServiceTask)
            {
                ServiceTasks tmpServiceTask = new ServiceTasks();

                tmpServiceTask.UpdateFromDatabaseObjectForServiceTaskChunkSearch(Service, TimeZone);
                ServiceTaskslist.Add(tmpServiceTask);
            }
            return ServiceTaskslist;
        }
        public b_ServiceTasks ToDateBaseObjectForChunkSearch()
        {
            b_ServiceTasks dbObj = this.ToDatabaseObject();
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.SearchText = this.SearchText;

            dbObj.ServiceTasksId = this.ServiceTasksId;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Description = this.Description;
            dbObj.ServiceTasksId = this.ServiceTasksId;
            dbObj.InactiveFlag = this.InactiveFlag;
            dbObj.TotalCount = this.TotalCount;

            return dbObj;
        }
        public void UpdateFromDatabaseObjectForServiceTaskChunkSearch(b_ServiceTasks dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Description = dbObj.Description;
            this.ServiceTasksId = dbObj.ServiceTasksId;
            this.InactiveFlag = dbObj.InactiveFlag;
            this.TotalCount = dbObj.TotalCount;

        }
        #endregion
        #endregion

        #region GetAll ServiceTask
        public List<ServiceTasks> RetrieveAllCustom(DatabaseKey dbKey)
        {
            ServiceTask_RetrieveAllCustom trans = new ServiceTask_RetrieveAllCustom()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.ServiceTasks = new b_ServiceTasks();
            trans.Execute();                    
            return UpdateFromDatabaseObjectList(trans.ServiceTasksList);
        }
        public static List<ServiceTasks> UpdateFromDatabaseObjectList(List<b_ServiceTasks> dbObjs)
        {
            List<ServiceTasks> result = new List<ServiceTasks>();

            foreach (b_ServiceTasks dbObj in dbObjs)
            {
                ServiceTasks tmp = new ServiceTasks();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        #endregion
    }
}
