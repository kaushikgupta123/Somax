/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2011 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== ========================================================
* 2011-Dec-09 20110019 Roger Lawton        Added ClientLookupId to search results
* 2011-Dec-14 20110039 Roger Lawton        Added Lookuplist validation
* 2014-Aug-10 SOM-280  Roger Lawton        Modified UpdateFromDataObjectList to include 
*                                          LaborAccountClientLookupId
* 2015-Mar-03 SOM-590  Roger Lawton        Removed validation on columns we do not support
* 2015-Sep-14 SOM-805  Roger Lawton        Location - Show Location.ClientLookupId if FACILITIES
***************************************************************************************************
*/
using System;
using System.Collections.Generic;
using Database.Business;
using Database;
using Common.Extensions;

namespace DataContracts
{
    public partial class MaintOnDemandMaster : DataContractBase, IStoredProcedureValidation
    {
        public long Creator_PersonneIId { get; internal set; }
        public DateTime CreateDate { get; private set; }
        public void CreateByPKForeignKeys(DatabaseKey dbKey)
        {
            Validate<MaintOnDemandMaster>(dbKey);

            if (IsValid)
            {
                MaintOnDemandMaster_Create trans = new MaintOnDemandMaster_Create();
                trans.MaintOnDemandMaster = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.MaintOnDemandMaster);
            }
        }

        public List<MaintOnDemandMaster> RetrieveAllBySiteId(DatabaseKey dbKey, string TimeZone)
        {
            RetrieveAllMaintOnDemandMaster trans = new RetrieveAllMaintOnDemandMaster()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
                
                
            };
            trans.SiteId = this.SiteId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<MaintOnDemandMaster> Mlist = new List<MaintOnDemandMaster>();

            foreach (b_MaintOnDemandMaster maintOnDemandMaster in trans.MaintOnDemandMasterList)
            {
                MaintOnDemandMaster tmp = new MaintOnDemandMaster();
                tmp.UpdateFromDatabaseObjectForRetriveAll(maintOnDemandMaster, TimeZone);
                Mlist.Add(tmp);
            }
            return Mlist;

        }
        private void UpdateFromDatabaseObjectForRetriveAll(b_MaintOnDemandMaster dbObj, string TimeZone)
        {

            this.UpdateFromDatabaseObject(dbObj);


            // SOM-706 - Convert the create date to the user's time zone
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                //this.CreateDate = dbObj.CreateDate.ConvertFromUTCToUser(TimeZone);
                this.CreateDate = dbObj.CreateDate.ConvertFromUTCToUser(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            Validate_Id vtrans = new Validate_Id()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            vtrans.MaintOnDemandMaster = this.ToDatabaseObject();
            vtrans.MaintOnDemandMaster.Creator_PersonneIId = dbKey.Personnel.PersonnelId;
            vtrans.dbKey = dbKey.ToTransDbKey();
            vtrans.Execute();
            if (vtrans.StoredProcValidationErrorList != null)
            {
                foreach (b_StoredProcValidationError error in vtrans.StoredProcValidationErrorList)
                {
                    StoredProcValidationError tmp = new StoredProcValidationError();
                    tmp.UpdateFromDatabaseObject(error);
                    errors.Add(tmp);
                }
            }
            return errors;
        }

    }
}
