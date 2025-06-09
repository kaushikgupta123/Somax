
using Database;
using Database.Business;
using System.Collections.Generic;
namespace DataContracts
{
    partial class EPMInvImpHdr : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
   
        #endregion
        #region Validation Methods
        public string ValidateFor = string.Empty;

        public void CheckEPMInvImpHdrValidate(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateEPMInvImpHdrImport";
            Validate<EPMInvImpHdr>(dbKey);
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();


            if (ValidateFor == "ValidateEPMInvImpHdrImport")
            {
                EPMInvImpHdr_ValidateImport trans = new EPMInvImpHdr_ValidateImport()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.EPMInvImpHdr = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                //trans.EPMInvImpHdr.ClientId = this.ClientId;
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
        public List<EPMInvImpHdr> EPMInvImpHdrImportRetrieveAll(DatabaseKey dbKey)
        {
            EPMInvImpHdr_RetrieveAll trans = new EPMInvImpHdr_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.EPMInvImpHdrList);
        }
        public static List<EPMInvImpHdr> UpdateFromDatabaseObjectList(List<b_EPMInvImpHdr> dbObjs)
        {
            List<EPMInvImpHdr> result = new List<EPMInvImpHdr>();

            foreach (b_EPMInvImpHdr dbObj in dbObjs)
            {
                EPMInvImpHdr tmp = new EPMInvImpHdr();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
       
    }
}
