using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
//using Business.Localization;
using Common.Extensions;

using Database;
using Database.Business;

namespace DataContracts
{

    public partial class POImpLine : DataContractBase, IStoredProcedureValidation
    {
        public Int64 SiteId { get; set; }
        public string PONumber { get; set; }
        //public Int64 POImpHdrId { get; set; }

        #region Validation Methods
        public string ValidateFor = string.Empty;

        public void CheckPOImpLineValidate(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateImport";
            Validate<POImpLine>(dbKey);
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();


            if (ValidateFor == "ValidateImport")
            {
                POImpLine_ValidateImport trans = new POImpLine_ValidateImport()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.POImpLine = this.ToDatabaseObject();
                trans.POImpLine.SiteId = this.SiteId;
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

        #region Process Import

        public void GetPOImpLineProcessImport(DatabaseKey dbKey)
        {
            POImpLine_ProcessImport trans = new POImpLine_ProcessImport()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.POImpLine = this.ToDatabaseObject();
            trans.POImpLine.SiteId = this.SiteId;
            trans.POImpLine.POImpHdrId = this.POImpHdrId;
            trans.POImpLine.POImpLineId = this.POImpLineId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.POImpLine);
        }

        #endregion


        #region Retrive POImpLine Data


        //public void GetPOImpLineRetrievedDataByPOId(DatabaseKey dbKey)
        //{

        //    if (IsValid)
        //    {
        //        POImpLine_RetrievebyPOIdFromDatabase trans = new POImpLine_RetrievebyPOIdFromDatabase()
        //        {
        //            CallerUserInfoId = dbKey.User.UserInfoId,
        //            CallerUserName = dbKey.UserName,
        //        };
        //        trans.POImpLine = this.ToDatabaseObject();
        //        trans.POImpLine.SiteId = this.SiteId;

        //        trans.dbKey = dbKey.ToTransDbKey();
        //        trans.Execute();

        //        UpdateFromDatabaseObject(trans.POImpLine);
        //    }

        //}



        public void GetPOImpLineRetrieveByExPOEXPOLine(DatabaseKey dbKey)
        {

            POImpLine_RetrievebyEXPOEXPOLine trans = new POImpLine_RetrievebyEXPOEXPOLine()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.POImpLine = this.ToDatabaseObject();
            trans.POImpLine.SiteId = this.SiteId;

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.POImpLine);

        }



        public void GetPOImpLineRetrievedDataBySomaxPRLineId(DatabaseKey dbKey)
        {
            POImpLine_RetrievebySomaxPRLineIdFromDatabase trans = new POImpLine_RetrievebySomaxPRLineIdFromDatabase()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.POImpLine = this.ToDatabaseObject();
            trans.POImpLine.SiteId = this.SiteId;

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.POImpLine);

        }

        //public void POImpLineRetrievedDataByPKCustom(DatabaseKey dbKey)
        //{


        //    if (IsValid)
        //    {
        //        POImpLine_RetrieveCustom trans = new POImpLine_RetrieveCustom()
        //        {
        //            CallerUserInfoId = dbKey.User.UserInfoId,
        //            CallerUserName = dbKey.UserName,
        //        };
        //        trans.POImpLine = this.ToDatabaseObject();
        //        trans.POImpLine.SiteId = this.SiteId;

        //        trans.dbKey = dbKey.ToTransDbKey();
        //        trans.Execute();

        //        UpdateFromDatabaseObject(trans.POImpLine);
        //    }

        //}

        #endregion


        #region Create
        public void CreateCustom(DatabaseKey dbKey)
        {
            POImpLine_CreateCustom trans = new POImpLine_CreateCustom();
            trans.POImpLine = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.POImpLine);
        }
        #endregion


        #region Update
        public void UpdateCustom(DatabaseKey dbKey)
        {
            POImpLine_UpdateCustom trans = new POImpLine_UpdateCustom();
            trans.POImpLine = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.POImpLine);
        }
        #endregion
    }

}





