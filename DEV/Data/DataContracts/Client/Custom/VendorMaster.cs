using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Extensions;

using Database;
using Database.Business;

namespace DataContracts
{
    //
    public partial class VendorMaster : DataContractBase, IStoredProcedureValidation
    {
        #region Property
        public string Mode { get; set; }
        public string Inactive { get; set; }

        public string ValidateFor = string.Empty;
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public long SiteId { get; set; }
        private bool m_validateClientLookupId;
        #endregion

        #region Vendor Master Search Page
        public List<VendorMaster> VendorMaster_RetrieveAll_ByInactiveFlag(DatabaseKey dbKey, string Timezone)
        {
            VendorMasterRetrieveAll_ByInactiveFlag trans = new VendorMasterRetrieveAll_ByInactiveFlag()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.VendorMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<VendorMaster> VendorMasterList = new List<VendorMaster>();
            foreach (b_VendorMaster VendorMaster in trans.VendorMasterList)
            {
                VendorMaster tmpVendorMaster = new VendorMaster();

                tmpVendorMaster.UpdateFromDatabaseObjectForRetriveByInactiveFlag(VendorMaster);
                VendorMasterList.Add(tmpVendorMaster);
            }
            return VendorMasterList;
        }
        public void UpdateFromDatabaseObjectForRetriveByInactiveFlag(b_VendorMaster dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.CreateBy = dbObj.CreateBy;
            this.CreateDate = dbObj.CreateDate;
            this.ModifyBy = dbObj.ModifyBy;
            this.ModifyDate = dbObj.ModifyDate;

            switch (dbObj.InactiveFlag)
            {
                case true:
                    Inactive = "Inactive";
                    break;
                case false:
                    Inactive = "Active";
                    break;
                default:
                    break;
            }
        }
        public void RetrieveByFK(DatabaseKey dbKey)
        {
            VendorMaster_RetrieveByFK trans = new VendorMaster_RetrieveByFK();
            trans.VendorMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectForRetriveByInactiveFlag(trans.VendorMaster);
        }

        #endregion Part Master Search Page

        #region Add New Vendor Master
        public void Add_Vendor(DatabaseKey dbKey)
        {
            if (ValidateVendorClientLookUpId(dbKey))
            {
                Vendor_CreateVendorFromVM trans = new Vendor_CreateVendorFromVM()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                    SiteId = this.SiteId
                };
                trans.VendorMaster = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.VendorMaster);
            }
        }
        public void Add_VendorMaster(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateClientIdVendorAndMaster";
            Validate<VendorMaster>(dbKey);
            if (IsValid)
            {
                VendorMaster_Create trans = new VendorMaster_Create()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.VendorMaster = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.VendorMaster);
            }
        }

        #endregion

        #region Populate VM for Adding Vendor from Vendor Master
        public List<VendorMaster> RetrieveVendorFromVM(DatabaseKey dbKey)
        {
            RetrieveVendorFromVM trans = new RetrieveVendorFromVM()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                SiteId = this.SiteId
            };
            trans.VendorMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<VendorMaster> VendorMasterList = new List<VendorMaster>();
            foreach (b_VendorMaster VendorMaster in trans.VendorList)
            {
                VendorMaster tmpVendorMaster = new VendorMaster();

                tmpVendorMaster.UpdateFromDatabaseObjectForRetriveByInactiveFlag(VendorMaster);
                VendorMasterList.Add(tmpVendorMaster);
            }
            return VendorMasterList;
        }

        #endregion

        #region Details Page Work
        public void Update_VendorMasterDetails(DatabaseKey dbKey)
        {
            //It Shold be Commented For Future Use As SPEC
            //ValidateFor = "Update";
            //Validate<VendorMaster>(dbKey);
            //if (IsValid)
            //{

            VendorMaster_Update trans = new VendorMaster_Update();
            trans.VendorMaster = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.VendorMaster);
            //}

        }

        public void ChangeClientLookupId(DatabaseKey dbKey)
        {

            //ValidateFor = "ChangeLookupId";
            ValidateFor = "ValidateClientIdVendorAndMaster";
            Validate<VendorMaster>(dbKey);
            if (IsValid)
            {

                VendorMaster_ChangeClientLookupId trans = new VendorMaster_ChangeClientLookupId();
                trans.VendorMaster = this.ToDatabaseObject();
                trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                //The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.VendorMaster);
            }

        }

        #endregion

        #region Validation Methods

        public bool ValidateVendorClientLookUpId(DatabaseKey dbKey)
        {
            m_validateClientLookupId = true;
            Validate<VendorMaster>(dbKey);
            return IsValid;
        }
        public b_VendorMaster ToDatabaseExtendObject()
        {
            b_VendorMaster dbObj = this.ToDatabaseObject();
            dbObj.Mode = this.Mode;
            return dbObj;
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            if (ValidateFor == "Create")
            {
                VendorMaster_ValidateByClientlookupId trans = new VendorMaster_ValidateByClientlookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.VendorMaster = this.ToDatabaseExtendObject();
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
            if (ValidateFor == "Update")
            {
                VendorMaster_ValidateByClientlookupId trans = new VendorMaster_ValidateByClientlookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.VendorMaster = this.ToDatabaseExtendObject();
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
            if (ValidateFor == "ChangeLookupId")
            {
                VendorMaster_ValidateByClientlookupIdForChange trans = new VendorMaster_ValidateByClientlookupIdForChange()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.VendorMaster = this.ToDatabaseExtendObject();
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
            if (m_validateClientLookupId)
            {
                Vendor v = new Vendor();
                v.ClientId = this.ClientId;
                v.SiteId = this.SiteId;
                v.ClientLookupId = this.ClientLookupId;

                ProcedureVendorCreateValidationTransaction trans = new ProcedureVendorCreateValidationTransaction()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.Vendor = v.ToDatabaseObject();
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
            if (ValidateFor == "ValidateClientIdVendorAndMaster")
            {
                VendorMaster_ValidateClientIdVendorAndMaster trans = new VendorMaster_ValidateClientIdVendorAndMaster()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.VendorMaster = this.ToDatabaseExtendObject();
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
