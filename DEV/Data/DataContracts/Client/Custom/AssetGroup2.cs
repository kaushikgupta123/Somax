using System;
using System.Collections.Generic;
using Database.Business;
using Database.Transactions;
using Database;

namespace DataContracts
{
    /// <summary>
    /// Business object that stores a record from the Department table.
    /// </summary>
    public partial class AssetGroup2 : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
        public string ValidateFor = string.Empty;
        public string ClientLookup_Desc { get; set; }
        #endregion
        public List<AssetGroup2> RetrieveAssetGroup2ByClientIdSiteId(DatabaseKey dbKey)
        {
            RetrieveAssetGroup2ByClientIdSiteId trans = new RetrieveAssetGroup2ByClientIdSiteId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.AssetGroup2 = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.AssetGroup2);
            List<AssetGroup2> AssetGroup2List = new List<AssetGroup2>();
            foreach (b_AssetGroup2 Asset in trans.AssetGroup2List)
            {
                AssetGroup2 tmpAssetGroup2 = new AssetGroup2()
                {
                    AssetGroup2Id = Asset.AssetGroup2Id,
                    ClientId = Asset.ClientId,
                    SiteId = Asset.SiteId,
                    Description = Asset.Description,
                    InactiveFlag = Asset.InactiveFlag,
                    ClientLookupId = Asset.ClientLookupId,
                    ClientLookup_Desc = Asset.ClientLookup_Desc
                };
                AssetGroup2List.Add(tmpAssetGroup2);
            }
            return AssetGroup2List;
        }


        public AssetGroup2 RetrieveAssetGroup2ByAssetGroup2Id(DatabaseKey dbKey)
        {
            Retrieve_AssetGroup2ByAssetGroup2Id trans = new Retrieve_AssetGroup2ByAssetGroup2Id()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.AssetGroup2 = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.AssetGroup2);
            List<AssetGroup2> AssetGroup2List = new List<AssetGroup2>();
            if (trans.AssetGroup2List != null && trans.AssetGroup2List.Count > 0)
            {
                foreach (b_AssetGroup2 Asset in trans.AssetGroup2List)
                {
                    AssetGroup2 tmpAssetGroup2 = new AssetGroup2()
                    {
                        AssetGroup2Id = Asset.AssetGroup2Id,
                        ClientId = Asset.ClientId,
                        SiteId = Asset.SiteId,
                        Description = Asset.Description,
                        InactiveFlag = Asset.InactiveFlag,
                        ClientLookupId = Asset.ClientLookupId,
                        ClientLookup_Desc = Asset.ClientLookup_Desc
                    };
                    AssetGroup2List.Add(tmpAssetGroup2);
                }
                this.AssetGroup2Id = trans.AssetGroup2List[0].AssetGroup2Id;
                this.ClientId = trans.AssetGroup2List[0].ClientId;
                this.SiteId = trans.AssetGroup2List[0].SiteId;
                this.Description = trans.AssetGroup2List[0].Description;
                this.InactiveFlag = trans.AssetGroup2List[0].InactiveFlag;
                this.ClientLookupId = trans.AssetGroup2List[0].ClientLookupId;
                this.ClientLookup_Desc = trans.AssetGroup2List[0].ClientLookup_Desc;
            }
            return this;
        }


        public List<AssetGroup2> RetrieveAssetGroup2ByByInActiveFlag_V2(DatabaseKey dbKey)
        {
            Retrieve_AssetGroup2ByInActiveFlag_V2 trans = new Retrieve_AssetGroup2ByInActiveFlag_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.AssetGroup2 = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();

            trans.Execute();
            UpdateFromDatabaseObject(trans.AssetGroup2);
            List<AssetGroup2> AssetGroup2List = new List<AssetGroup2>();
            foreach (b_AssetGroup2 Asset in trans.AssetGroup2List)
            {
                AssetGroup2 tmpAssetGroup2 = new AssetGroup2()
                {
                    AssetGroup2Id = Asset.AssetGroup2Id,
                    ClientId = Asset.ClientId,
                    SiteId = Asset.SiteId,
                    Description = Asset.Description,
                    InactiveFlag = Asset.InactiveFlag,
                    ClientLookupId = Asset.ClientLookupId,
                    ClientLookup_Desc = Asset.ClientLookup_Desc
                };
                AssetGroup2List.Add(tmpAssetGroup2);
            }
            return AssetGroup2List;
        }


        public List<AssetGroup2> RetrieveAllAssetGroup2ByInActiveFlag_V2(DatabaseKey dbKey)
        {
            RetrieveAll_AssetGroup2ByInActiveFlag_V2 trans = new RetrieveAll_AssetGroup2ByInActiveFlag_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.AssetGroup2 = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();

            trans.Execute();
            UpdateFromDatabaseObject(trans.AssetGroup2);
            List<AssetGroup2> AssetGroup2List = new List<AssetGroup2>();
            foreach (b_AssetGroup2 Asset in trans.AssetGroup2List)
            {
                AssetGroup2 tmpAssetGroup2 = new AssetGroup2()
                {
                    AssetGroup2Id = Asset.AssetGroup2Id,
                    ClientId = Asset.ClientId,
                    SiteId = Asset.SiteId,
                    Description = Asset.Description,
                    InactiveFlag = Asset.InactiveFlag,
                    ClientLookupId = Asset.ClientLookupId,
                    ClientLookup_Desc = Asset.ClientLookup_Desc
                };
                AssetGroup2List.Add(tmpAssetGroup2);
            }
            return AssetGroup2List;
        }


        public bool ValidateNewClientLookupIdforAssetGroup2(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateNewClientLookupIdforAssetGroup2";
            Validate<AssetGroup2>(dbKey);
            return IsValid;
        }

        public bool ValidateOldClientLookupIdforAssetGroup2(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateOldClientLookupIdforAssetGroup2";
            Validate<AssetGroup2>(dbKey);
            return IsValid;
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            if (ValidateFor == "ValidateNewClientLookupIdforAssetGroup2")
            {
                AssetGroup2_ValidateNewClientLookupIdV2 trans = new AssetGroup2_ValidateNewClientLookupIdV2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.AssetGroup2 = this.ToDatabaseObject();
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


            if (ValidateFor == "ValidateOldClientLookupIdforAssetGroup2")
            {
                AssetGroup2_ValidateOldClientLookupIdV2 trans = new AssetGroup2_ValidateOldClientLookupIdV2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.AssetGroup2 = this.ToDatabaseObject();
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

        }
}
