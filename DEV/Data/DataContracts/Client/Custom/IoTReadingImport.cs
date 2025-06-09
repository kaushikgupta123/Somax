using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    partial class IoTReadingImport : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
        public long PersonnelId { get; set; }
        public long IoTEventId { get; set; }
        public string AlertName { get; set; }
        #endregion
        #region Validation Methods
        public string ValidateFor = string.Empty;

        public void CheckIoTReadingImportValidate(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateImport";
            Validate<IoTReadingImport>(dbKey);
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();


            if (ValidateFor == "ValidateImport")
            {
                IoTReadingImport_ValidateImport trans = new IoTReadingImport_ValidateImport()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.IoTReadingImport = this.ToDatabaseObject();

                trans.dbKey = dbKey.ToTransDbKey();
                trans.IoTReadingImport.ClientId = this.ClientId;
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
        #endregion
        public List<IoTReadingImport> IoTReadingImportRetrieveAll(DatabaseKey dbKey)
        {
            IoTReadingImport_RetrieveAll trans = new IoTReadingImport_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.IoTReadingImportList);
        }
        public static List<IoTReadingImport> UpdateFromDatabaseObjectList(List<b_IoTReadingImport> dbObjs)
        {
            List<IoTReadingImport> result = new List<IoTReadingImport>();

            foreach (b_IoTReadingImport dbObj in dbObjs)
            {
                IoTReadingImport tmp = new IoTReadingImport();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        #region process 
        public void GetIoTReadingImportProcess(DatabaseKey dbKey)
        {
            IoTReadingImport_ProcessImport trans = new IoTReadingImport_ProcessImport()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.IoTReadingImport = this.ToDatabaseObject();
            trans.IoTReadingImport.PersonnelId = this.PersonnelId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.IoTReadingImport);
            this.AlertName = trans.IoTReadingImport.AlertName;
            this.IoTEventId = trans.IoTReadingImport.IoTEventId;
        }
        #endregion
    }
}
