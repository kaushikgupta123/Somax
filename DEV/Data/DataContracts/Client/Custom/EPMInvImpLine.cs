using Database;
using Database.Business;
using System.Collections.Generic;
namespace DataContracts
{
    partial class EPMInvImpLine : DataContractBase, IStoredProcedureValidation
    {
        #region Properties

        #endregion
        #region Validation Methods
        public string ValidateFor = string.Empty;

        public void CheckEPMInvImpLineValidate(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateEPMInvImpLineImport";
            Validate<EPMInvImpLine>(dbKey);
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();


            if (ValidateFor == "ValidateEPMInvImpLineImport")
            {
                EPMInvImpLine_ValidateImport trans = new EPMInvImpLine_ValidateImport()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.EPMInvImpLine = this.ToDatabaseObject();

                trans.dbKey = dbKey.ToTransDbKey();
                trans.EPMInvImpLine.ClientId = this.ClientId;
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
        public List<EPMInvImpLine> EPMInvImpLineImportRetrieveAll(DatabaseKey dbKey)
        {
            EPMInvImpLine_RetrieveAll trans = new EPMInvImpLine_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.EPMInvImpLineList);
        }
        public static List<EPMInvImpLine> UpdateFromDatabaseObjectList(List<b_EPMInvImpLine> dbObjs)
        {
            List<EPMInvImpLine> result = new List<EPMInvImpLine>();

            foreach (b_EPMInvImpLine dbObj in dbObjs)
            {
                EPMInvImpLine tmp = new EPMInvImpLine();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

    }
}

