using Data.Database.Business;
using Data.Database;
using Data.DataContracts;

using System;
using System.Collections.Generic;
using Database.Business;
using Database;
namespace DataContracts
{
    public partial class ShipTo : DataContractBase,IStoredProcedureValidation
    {
        #region Properties
        string ValidateFor = string.Empty;
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public int totalCount { get; set; }
        public string SearchText { get; set; }

        #endregion
        public List<ShipTo> RetrieveChunkSearch(DatabaseKey dbKey)
        {
            ShipTo_RetrieveChunkSearch trans = new ShipTo_RetrieveChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.ShipTo = this.ToDatabaseObject();
            trans.ShipTo.orderbyColumn = this.orderbyColumn;
            trans.ShipTo.orderBy = this.orderBy;
            trans.ShipTo.offset1 = this.offset1;
            trans.ShipTo.nextrow = this.nextrow;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<ShipTo> list = new List<ShipTo>();

            foreach (b_ShipTo v in trans.ShipToList)
            {
                ShipTo tmp = new ShipTo();
                tmp.UpdateFromDatabaseObject(v);
                tmp.totalCount = v.totalCount;
                list.Add(tmp);
            }
            return list;

        }

        
        public void Add(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateForClientlookupId";
            Validate<ShipTo>(dbKey);
            if (IsValid)
            {
                ShipTo_Create trans = new ShipTo_Create()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.ShipTo = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.ShipTo);
            }

        }

        public void UpdateCustom(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateForClientlookupId";
            Validate<ShipTo>(dbKey);
            if (IsValid)
            {
                ShipTo_Update trans = new ShipTo_Update();
                trans.ShipTo = this.ToDatabaseObject();
                trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.ShipTo);
            }

        }

        #region Validation Methods
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            if (ValidateFor == "ValidateForClientlookupId")
            {
                ShipTo_ValidateByClientlookupId trans = new ShipTo_ValidateByClientlookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.ShipTo = this.ToDatabaseObject();

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

        #endregion
    }
}
