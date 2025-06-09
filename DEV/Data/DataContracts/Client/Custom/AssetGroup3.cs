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
    public partial class AssetGroup3 : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
        public string ValidateFor = string.Empty;
        public string ClientLookup_Desc { get; set; }
        #endregion
        public List<AssetGroup3> RetrieveAssetGroup3ByClientIdSiteId(DatabaseKey dbKey)
        {
            RetrieveAssetGroup3ByClientIdSiteId trans = new RetrieveAssetGroup3ByClientIdSiteId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.AssetGroup3 = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.AssetGroup3);
            List<AssetGroup3> AssetGroup3List = new List<AssetGroup3>();
            foreach (b_AssetGroup3 Asset in trans.AssetGroup3List)
            {
                AssetGroup3 tmpAssetGroup3 = new AssetGroup3()
                {
                    AssetGroup3Id = Asset.AssetGroup3Id,
                    ClientId = Asset.ClientId,
                    SiteId = Asset.SiteId,
                    Description = Asset.Description,
                    InactiveFlag = Asset.InactiveFlag,
                    ClientLookupId = Asset.ClientLookupId,
                    ClientLookup_Desc = Asset.ClientLookup_Desc
                };
                AssetGroup3List.Add(tmpAssetGroup3);
            }
            return AssetGroup3List;
        }


        public AssetGroup3 RetrieveAssetGroup3ByAssetGroup3Id(DatabaseKey dbKey)
        {
            Retrieve_AssetGroup3ByAssetGroup3Id trans = new Retrieve_AssetGroup3ByAssetGroup3Id()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.AssetGroup3 = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.AssetGroup3);
            List<AssetGroup3> AssetGroup3List = new List<AssetGroup3>();
            if (trans.AssetGroup3List != null && trans.AssetGroup3List.Count > 0)
            {
                foreach (b_AssetGroup3 Asset in trans.AssetGroup3List)
                {
                    AssetGroup3 tmpAssetGroup3 = new AssetGroup3()
                    {
                        AssetGroup3Id = Asset.AssetGroup3Id,
                        ClientId = Asset.ClientId,
                        SiteId = Asset.SiteId,
                        Description = Asset.Description,
                        InactiveFlag = Asset.InactiveFlag,
                        ClientLookupId = Asset.ClientLookupId,
                        ClientLookup_Desc = Asset.ClientLookup_Desc
                    };
                    AssetGroup3List.Add(tmpAssetGroup3);
                }
                this.AssetGroup3Id = trans.AssetGroup3List[0].AssetGroup3Id;
                this.ClientId = trans.AssetGroup3List[0].ClientId;
                this.SiteId = trans.AssetGroup3List[0].SiteId;
                this.Description = trans.AssetGroup3List[0].Description;
                this.InactiveFlag = trans.AssetGroup3List[0].InactiveFlag;
                this.ClientLookupId = trans.AssetGroup3List[0].ClientLookupId;
                this.ClientLookup_Desc = trans.AssetGroup3List[0].ClientLookup_Desc;
            }
            return this;
        }


        public List<AssetGroup3> RetrieveAssetGroup3ByByInActiveFlag_V2(DatabaseKey dbKey)
        {
            Retrieve_AssetGroup3ByInActiveFlag_V2 trans = new Retrieve_AssetGroup3ByInActiveFlag_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.AssetGroup3 = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();

            trans.Execute();
            UpdateFromDatabaseObject(trans.AssetGroup3);
            List<AssetGroup3> AssetGroup3List = new List<AssetGroup3>();
            foreach (b_AssetGroup3 Asset in trans.AssetGroup3List)
            {
                AssetGroup3 tmpAssetGroup3 = new AssetGroup3()
                {
                    AssetGroup3Id = Asset.AssetGroup3Id,
                    ClientId = Asset.ClientId,
                    SiteId = Asset.SiteId,
                    Description = Asset.Description,
                    InactiveFlag = Asset.InactiveFlag,
                    ClientLookupId = Asset.ClientLookupId,
                    ClientLookup_Desc = Asset.ClientLookup_Desc
                };
                AssetGroup3List.Add(tmpAssetGroup3);
            }
            return AssetGroup3List;
        }

        public List<AssetGroup3> RetrieveAllAssetGroup3ByInActiveFlag_V2(DatabaseKey dbKey)
        {
            RetrieveAll_AssetGroup3ByInActiveFlag_V2 trans = new RetrieveAll_AssetGroup3ByInActiveFlag_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.AssetGroup3 = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();

            trans.Execute();
            UpdateFromDatabaseObject(trans.AssetGroup3);
            List<AssetGroup3> AssetGroup3List = new List<AssetGroup3>();
            foreach (b_AssetGroup3 Asset in trans.AssetGroup3List)
            {
                AssetGroup3 tmpAssetGroup3 = new AssetGroup3()
                {
                    AssetGroup3Id = Asset.AssetGroup3Id,
                    ClientId = Asset.ClientId,
                    SiteId = Asset.SiteId,
                    Description = Asset.Description,
                    InactiveFlag = Asset.InactiveFlag,
                    ClientLookupId = Asset.ClientLookupId,
                    ClientLookup_Desc = Asset.ClientLookup_Desc
                };
                AssetGroup3List.Add(tmpAssetGroup3);
            }
            return AssetGroup3List;
        }

        public bool ValidateNewClientLookupIdforAssetGroup3(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateNewClientLookupIdforAssetGroup3";
            Validate<AssetGroup3>(dbKey);
            return IsValid;
        }

        public bool ValidateOldClientLookupIdforAssetGroup3(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateOldClientLookupIdforAssetGroup3";
            Validate<AssetGroup3>(dbKey);
            return IsValid;
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            if (ValidateFor == "ValidateNewClientLookupIdforAssetGroup3")
            {
                AssetGroup3_ValidateNewClientLookupIdV2 trans = new AssetGroup3_ValidateNewClientLookupIdV2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.AssetGroup3 = this.ToDatabaseObject();
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

            if (ValidateFor == "ValidateOldClientLookupIdforAssetGroup3")
            {
                AssetGroup3_ValidateOldClientLookupIdV2 trans = new AssetGroup3_ValidateOldClientLookupIdV2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.AssetGroup3 = this.ToDatabaseObject();
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
