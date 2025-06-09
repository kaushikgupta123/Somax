using Client.BusinessWrapper.Common;
using Client.Models.Configuration.SiteSetUp;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Data;
using Client.Common;
using Common.Constants;
namespace Client.BusinessWrapper.Configuration.SiteSetUp
{
    public class SiteSetUpWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public SiteSetUpWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        internal Site SiteDetails()
        {
            Site site = new Site()
            {
                SiteId = userData.DatabaseKey.Personnel.SiteId
            };
            site.Retrieve(this.userData.DatabaseKey);
            return site;
        }

        internal List<CustomSequentialId> CustomIdDetails(string key)
        {
            CustomSequentialId custid = new CustomSequentialId();

            List<CustomSequentialId> custList = new List<CustomSequentialId>();
            custid.KeyList = key;
            custList = custid.RetrieveByClientIdandSiteIdandKey_V2(userData.DatabaseKey);
            return custList;
        }




        internal List<Personnel> CreatorList()
        {
            Personnel personnel = new Personnel()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            List<Personnel> PersonnelList = personnel.RetrieveForLookupList(userData.DatabaseKey);
            return PersonnelList;
        }
        internal string PrintType()
        {
            string printType = string.Empty;
            INTDataLayer.BAL.SiteBAL sBal = new INTDataLayer.BAL.SiteBAL();
            DataTable dt = new DataTable();
            dt = sBal.GetSettingBySettingName(this.userData.DatabaseKey.Client.ClientId, this.userData.Site.SiteId, "PrintLabelSize", this.userData.DatabaseKey.ClientConnectionString);
            if (dt.Rows.Count > 0)
            {
                printType = dt.Rows[0]["SettingValue"].ToString();
            }
            else
            {
                printType = "SmallQRCode";
                sBal.UpdateSettingBySettingName(this.userData.DatabaseKey.Client.ClientId, this.userData.Site.SiteId, "PrintLabelSize", printType, this.userData.DatabaseKey.ClientConnectionString);
            }
            return printType;
        }
        internal Site RetriveSiteDetailsByClientAndSite(long ClientId, long SiteId)
        {
            Site site = new Site()
            {
                ClientId = ClientId,
                SiteId = SiteId
            };
            site.Retrieve(this.userData.DatabaseKey);
            return site;
        }
        internal List<String> UpdateSiteSetup(SiteSetUpModel objM, ref string ErrorMsg)
        {
            Site site = new Site()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.Personnel.SiteId
            };
            site.Retrieve(this.userData.DatabaseKey);
            bool shoppingcart = userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UsePartMaster;
            INTDataLayer.BAL.SiteBAL sBal = new INTDataLayer.BAL.SiteBAL();
            if (site.IsValid)
            {
                //if (objM.tabType != "ApprovalGroupSettings")
                //{
                List<string> returnObj = new List<string>();
                List<Site> siteList = site.RetrieveAll(this.userData.DatabaseKey);
                List<Site> obj = siteList.FindAll(x => (x.Name.ToLower() == objM.Name.Trim().ToLower()) && (x.SiteId != userData.DatabaseKey.Personnel.SiteId));
                if (userData.DatabaseKey.Personnel.SiteId == -1)
                {
                    obj = siteList.FindAll(x => (x.Name.ToLower() == objM.Name.Trim().ToLower()) && (x.SiteId != userData.DatabaseKey.Personnel.SiteId));
                }
                if (obj != null && obj.Count > 0)
                {
                    returnObj.Add("Site Name already available to this client");
                    return returnObj;

                }
                site = RetriveFromModel(objM, site);


                if (shoppingcart && site.ShoppingCartIncludeBuyer)
                {
                    long peid = site.ShoppingCartReviewDefault;
                    if (peid > 0)
                    {
                        Personnel p = new Personnel();
                        p.PersonnelId = site.ShoppingCartReviewDefault;
                        p.Retrieve(this.userData.DatabaseKey);
                        if (p.Buyer == true)
                        {
                            site.UpdateWithProcessSystemTreeValidation(this.userData.DatabaseKey);
                            sBal.UpdateSettingBySettingName(this.userData.DatabaseKey.Client.ClientId, this.userData.Site.SiteId, "PrintLabelSize", objM.PrintType, this.userData.DatabaseKey.ClientConnectionString);
                            string updateIndex = site.UpdateIndex.ToString();
                        }
                        else
                        {
                            returnObj.Add("You must select a user that is a Buyer for the Default Reviewer / Approver");
                            return returnObj;
                        }
                    }
                    else
                    {
                        returnObj.Add("You must select a user that is a Buyer for the Default Reviewer / Approver");
                        return returnObj;
                    }
                }

                else
                {
                    site.UpdateWithProcessSystemTreeValidation(this.userData.DatabaseKey);
                    sBal.UpdateSettingBySettingName(this.userData.DatabaseKey.Client.ClientId, this.userData.Site.SiteId, "PrintLabelSize", objM.PrintType, this.userData.DatabaseKey.ClientConnectionString);
                    string updateIndex = site.UpdateIndex.ToString();
                    CustomSequentialId custid = new CustomSequentialId();

                    if (!string.IsNullOrEmpty(objM.PRPrefix) || !string.IsNullOrEmpty(objM.POPrefix))
                    {
                        custid.ClientId = this.userData.DatabaseKey.Client.ClientId;
                        custid.SiteId = this.userData.Site.SiteId;
                        custid.PRPrefix = objM.PRPrefix;
                        custid.POPrefix = objM.POPrefix;
                        custid.UpdatePrefixbyKey_V2(this.userData.DatabaseKey);

                    }
                }
                //}
                //else if ((site.ErrorMessages == null) || (site.ErrorMessages.Count == 0))
                //{
                //ApprovalGroupSettings
                if (site.ClientId > 0 && site.SiteId > 0)
                {
                    ApprovalGroupSettings approvalGroupSettings = new ApprovalGroupSettings();
                    approvalGroupSettings.ClientId = this.userData.DatabaseKey.Client.ClientId;
                    approvalGroupSettings.SiteId = this.userData.Site.SiteId;
                    approvalGroupSettings.MaterialRequests = objM.approvalGroupSettingsModel.MaterialRequests;
                    approvalGroupSettings.PurchaseRequests = objM.approvalGroupSettingsModel.PurchaseRequests;
                    approvalGroupSettings.SanitationRequests = objM.approvalGroupSettingsModel.SanitationRequests;
                    approvalGroupSettings.WorkRequests = objM.approvalGroupSettingsModel.WorkRequests;
                    approvalGroupSettings.ApprovalGroupSettingsId = objM.approvalGroupSettingsModel.ApprovalGroupSettingsId;
                    if (approvalGroupSettings.ApprovalGroupSettingsId == 0)
                    {
                        approvalGroupSettings.Create(this.userData.DatabaseKey);
                    }
                    else
                    {
                        approvalGroupSettings.Update(this.userData.DatabaseKey);
                    }
                }

                //}


            }
            return site.ErrorMessages;
        }
        internal Site RetriveFromModel(SiteSetUpModel objM, Site site)
        {
            #region OverView
            site.Name = objM.Name ?? string.Empty;
            site.Description = objM.Name ?? string.Empty;
            site.TimeZone = objM.TimeZone ?? string.Empty;
            site.MaintOnDemand = objM.IsondemandWorkorderaccess;
            site.MobileWOTimer = objM.MobileWOTimer ?? string.Empty;
            site.PMLibrary = objM.PMLibrary;

            #region Asset Group
            site.AssetGroup1Name = String.IsNullOrEmpty(objM.AssetGroup1Name) ? "Asset Group 1" : objM.AssetGroup1Name;
            site.AssetGroup2Name = String.IsNullOrEmpty(objM.AssetGroup2Name) ? "Asset Group 2" : objM.AssetGroup2Name;
            site.AssetGroup3Name = String.IsNullOrEmpty(objM.AssetGroup3Name) ? "Asset Group 3" : objM.AssetGroup3Name;
            #endregion
            #endregion
            #region Ship To
            site.ShipToName = objM.ShipToName ?? string.Empty;
            site.Address1 = objM.ShipToAddress1 ?? string.Empty;
            site.Address2 = objM.ShipToAddress2 ?? string.Empty;
            site.Address3 = objM.ShipToAddress3 ?? string.Empty;
            site.AddressCity = objM.ShipToCity ?? string.Empty;
            site.AddressState = objM.ShipToState ?? string.Empty;
            site.AddressCountry = objM.ShipToCountry ?? string.Empty;
            site.AddressPostCode = objM.ShipToPostalCode ?? string.Empty;
            #endregion
            #region Bill To
            site.BillToName = objM.BillToName ?? string.Empty;
            site.BillToAddress1 = objM.BillToAddress1 ?? string.Empty;
            site.BillToAddress2 = objM.BillToAddress2 ?? string.Empty;
            site.BillToAddress3 = objM.BillToAddress3 ?? string.Empty;
            site.BillToAddressCity = objM.BillToCity ?? string.Empty;
            site.BillToAddressCountry = objM.BillToCountry ?? string.Empty;
            site.BillToAddressPostCode = objM.BillToPostalCode ?? string.Empty;
            site.BillToAddressState = objM.BillToState ?? string.Empty;
            site.BillToComment = objM.BillToComment ?? string.Empty;
            #endregion

            #region AddPurchasing
            site.AutoPurch = objM.Isincludeinautopurchasing;
            site.AutoPurch_Prefix = objM.AutoPurchPrefix ?? string.Empty;
            site.AutoPurch_CreatorId = objM.CreatorID ?? 0;
            site.NonStockAccountId = objM.NonStockDefaultAccount ?? 0;
            #region V2-820
            site.IncludePRReview = objM.IsIncludePRPreview;
            site.ShoppingCartReviewDefault = objM.ShoppingCartReviewDefault??0;
            site.ShoppingCartIncludeBuyer = objM.IsShoppingCartIncludeBuyer;
            site.DefaultBuyer = objM.DefaultBuyer??0;
            #endregion
            #endregion
            #region V2-894
            site.OnOrderCheck = objM.IsOnOrderCheck;
            #endregion
            #region V2-1032
            site.SingleStockLineItem = objM.SingleStockLineItem;
            #endregion
            site.UpdateIndex = site.UpdateIndex;
            //changes for  V2-576
            site.UsePlanning = objM.IsUsePlanning;
            //V2-676
            site.WOBarcode = objM.WOBarCode;
            #region V2-948
            site.SourceAssetAccount = objM.SourceAssetAccount;
            #endregion
            #region V2-1061
           site.InvoiceVarianceCheck = objM.InvoiceVarianceCheck;
            site.InvoiceVariance = objM.InvoiceVariance??0;
            #endregion
            return site;
        }
        internal bool UpdatePhoto(string rtrData)
        {
            Site site = new Site()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.Personnel.SiteId
            };
            site.Retrieve(userData.DatabaseKey);
            site.Logo = rtrData;
            site.Update(userData.DatabaseKey);
            if (site.ErrorMessages != null && site.ErrorMessages.Count > 0)
            {
                return false;
            }
            return true;
        }

        #region AssetGroup1

        public List<AssetGroup1Model> PopulateAssetGroup1()
        {

            List<AssetGroup1Model> AssetGroup1ModelList = new List<AssetGroup1Model>();
            AssetGroup1Model objAssetGroup1Model;
            AssetGroup1 assetGroup1 = new AssetGroup1()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            List<AssetGroup1> AssetGroup1List = assetGroup1.RetrieveAssetGroup1ByClientIdSiteId(userData.DatabaseKey);

            foreach (var p in AssetGroup1List)
            {
                objAssetGroup1Model = new AssetGroup1Model();
                objAssetGroup1Model.AssetGroup1Id = p.AssetGroup1Id;
                objAssetGroup1Model.Description = p.Description ?? string.Empty;
                objAssetGroup1Model.InactiveFlag = p.InactiveFlag;
                objAssetGroup1Model.ClientLookupId = p.ClientLookupId;
                AssetGroup1ModelList.Add(objAssetGroup1Model);
            }
            return AssetGroup1ModelList;
        }

        public List<String> AddOrEditAssetGroup1(SiteSetUpVM objVM, ref string Mode)
        {
            AssetGroup1 assetGroup1 = new AssetGroup1();
            if (objVM.assetGroup1Model.AssetGroup1Id == 0)
            {
                Mode = "add";
                assetGroup1.ClientId = userData.DatabaseKey.Client.ClientId;
                assetGroup1.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                assetGroup1.Description = objVM.assetGroup1Model.Description.Trim() ?? string.Empty;
                assetGroup1.InactiveFlag = objVM.assetGroup1Model.InactiveFlag;
                assetGroup1.ClientLookupId = objVM.assetGroup1Model.ClientLookupId;
                if (assetGroup1.ValidateNewClientLookupIdforAssetGroup1(userData.DatabaseKey) == true)
                {
                    assetGroup1.Create(userData.DatabaseKey);
                }
            }
            else
            {
                Mode = "update";
                assetGroup1.ClientId = userData.DatabaseKey.Client.ClientId;
                assetGroup1.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                assetGroup1.AssetGroup1Id = objVM.assetGroup1Model.AssetGroup1Id;
                var assetgroup1 = assetGroup1.RetrieveAssetGroup1ByAssetGroup1Id(userData.DatabaseKey);
                if (objVM.assetGroup1Model.InactiveFlag == true && assetGroup1.InactiveFlag == false)
                {
                    DataContracts.Equipment eq = new DataContracts.Equipment();
                    eq.ClientId = userData.DatabaseKey.Client.ClientId;
                    eq.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                    eq.ClientLookupId = assetGroup1.ClientLookupId;
                    eq.AssetGroup1 = assetGroup1.AssetGroup1Id;
                    eq.CheckIfAssetGroup1UsedInEquipment(userData.DatabaseKey);
                    if (eq.ErrorMessages != null && eq.ErrorMessages.Count > 0)
                    {
                        return eq.ErrorMessages;
                    }
                }
                assetGroup1.ClientLookupId = objVM.assetGroup1Model.ClientLookupId;
                assetGroup1.Description = objVM.assetGroup1Model.Description.Trim() ?? string.Empty;
                assetGroup1.InactiveFlag = objVM.assetGroup1Model.InactiveFlag;
                assetGroup1.UpdateIndex = objVM.assetGroup1Model.UpdateIndex;
                if (assetGroup1.ValidateOldClientLookupIdforAssetGroup1(userData.DatabaseKey) == true)
                {
                    assetGroup1.Update(userData.DatabaseKey);
                }

            }
            return assetGroup1.ErrorMessages;
        }
        public List<string> ActivateAssetGroup1ById(long AssetGroup1Id)
        {
            AssetGroup1 assetGroup1 = new AssetGroup1();
            assetGroup1.ClientId = userData.DatabaseKey.Client.ClientId;
            assetGroup1.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            assetGroup1.AssetGroup1Id = AssetGroup1Id;
            assetGroup1.RetrieveAssetGroup1ByAssetGroup1Id(userData.DatabaseKey);
            assetGroup1.InactiveFlag = false;
            if (assetGroup1.ValidateOldClientLookupIdforAssetGroup1(userData.DatabaseKey) == true)
            {
                assetGroup1.Update(userData.DatabaseKey);
            }
            return assetGroup1.ErrorMessages;
        }
        public AssetGroup1Model GetAssetGroup1ByAssetGroup1Id(long AssetGroup1Id)
        {
            AssetGroup1Model objAssetGroup1Model = new AssetGroup1Model();
            objAssetGroup1Model.AssetGroup1Id = AssetGroup1Id;
            AssetGroup1 assetGroup1 = new AssetGroup1()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                AssetGroup1Id = AssetGroup1Id
            };
            assetGroup1.Retrieve(userData.DatabaseKey);
            if (assetGroup1 != null)
            {
                objAssetGroup1Model.ClientLookupId = assetGroup1.ClientLookupId;
                objAssetGroup1Model.Description = assetGroup1.Description;
                objAssetGroup1Model.AssetGroup1Id = assetGroup1.AssetGroup1Id;
                objAssetGroup1Model.UpdateIndex = assetGroup1.UpdateIndex;
                objAssetGroup1Model.InactiveFlag = assetGroup1.InactiveFlag;
            }
            return objAssetGroup1Model;
        }

        internal DataContracts.AssetGroup1 DeleteAssetGroup1(long AssetGroup1Id)
        {
            try
            {
                DataContracts.AssetGroup1 assetGroup1 = new DataContracts.AssetGroup1()
                {
                    AssetGroup1Id = AssetGroup1Id,
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.DatabaseKey.User.DefaultSiteId
                };
                DataContracts.Equipment eq = new DataContracts.Equipment();
                eq.ClientId = userData.DatabaseKey.Client.ClientId;
                eq.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                eq.ObjectId = assetGroup1.AssetGroup1Id;
                eq.ObjectName = "AssetGroup1";
                eq.CheckForeignFieldToDelete(userData.DatabaseKey);
                if (eq.ErrorMessages == null || eq.ErrorMessages.Count == 0)
                {
                    assetGroup1.Delete(userData.DatabaseKey);
                }
                else
                {
                    assetGroup1.ErrorMessages = eq.ErrorMessages;
                }
                return assetGroup1;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion
        #region AssetGroup2

        public List<AssetGroup2Model> PopulateAssetGroup2()
        {

            List<AssetGroup2Model> AssetGroup2ModelList = new List<AssetGroup2Model>();
            AssetGroup2Model objAssetGroup2Model;
            AssetGroup2 assetGroup2 = new AssetGroup2()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            List<AssetGroup2> AssetGroup2List = assetGroup2.RetrieveAssetGroup2ByClientIdSiteId(userData.DatabaseKey);

            foreach (var p in AssetGroup2List)
            {
                objAssetGroup2Model = new AssetGroup2Model();

                objAssetGroup2Model.AssetGroup2Id = p.AssetGroup2Id;
                objAssetGroup2Model.Description = p.Description ?? string.Empty;
                objAssetGroup2Model.InactiveFlag = p.InactiveFlag;
                objAssetGroup2Model.ClientLookupId = p.ClientLookupId;
                AssetGroup2ModelList.Add(objAssetGroup2Model);
            }
            return AssetGroup2ModelList;
        }

        public List<String> AddOrEditAssetGroup2(SiteSetUpVM objVM, ref string Mode)
        {
            AssetGroup2 assetGroup2 = new AssetGroup2();
            if (objVM.assetGroup2Model.AssetGroup2Id == 0)
            {
                Mode = "add";
                assetGroup2.ClientId = userData.DatabaseKey.Client.ClientId;
                assetGroup2.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                assetGroup2.Description = objVM.assetGroup2Model.Description.Trim() ?? string.Empty;
                assetGroup2.InactiveFlag = objVM.assetGroup2Model.InactiveFlag;
                assetGroup2.ClientLookupId = objVM.assetGroup2Model.ClientLookupId;
                if (assetGroup2.ValidateNewClientLookupIdforAssetGroup2(userData.DatabaseKey) == true)
                {
                    assetGroup2.Create(userData.DatabaseKey);
                }
            }
            else
            {
                Mode = "update";
                assetGroup2.ClientId = userData.DatabaseKey.Client.ClientId;
                assetGroup2.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                assetGroup2.AssetGroup2Id = objVM.assetGroup2Model.AssetGroup2Id;
                assetGroup2.RetrieveAssetGroup2ByAssetGroup2Id(userData.DatabaseKey);
                if (objVM.assetGroup2Model.InactiveFlag == true && assetGroup2.InactiveFlag == false)
                {
                    DataContracts.Equipment eq = new DataContracts.Equipment();
                    eq.ClientId = userData.DatabaseKey.Client.ClientId;
                    eq.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                    eq.ClientLookupId = assetGroup2.ClientLookupId;
                    eq.AssetGroup2 = assetGroup2.AssetGroup2Id;
                    eq.CheckIfAssetGroup2UsedInEquipment(userData.DatabaseKey);
                    if (eq.ErrorMessages != null && eq.ErrorMessages.Count > 0)
                    {
                        return eq.ErrorMessages;
                    }
                }
                assetGroup2.ClientLookupId = objVM.assetGroup2Model.ClientLookupId;
                assetGroup2.Description = objVM.assetGroup2Model.Description.Trim() ?? string.Empty;
                assetGroup2.InactiveFlag = objVM.assetGroup2Model.InactiveFlag;
                assetGroup2.UpdateIndex = objVM.assetGroup2Model.UpdateIndex;
                if (assetGroup2.ValidateOldClientLookupIdforAssetGroup2(userData.DatabaseKey) == true)
                {
                    assetGroup2.Update(userData.DatabaseKey);
                }

            }
            return assetGroup2.ErrorMessages;
        }
        public List<string> ActivateAssetGroup2ById(long AssetGroup2Id)
        {
            AssetGroup2 assetGroup2 = new AssetGroup2();
            assetGroup2.ClientId = userData.DatabaseKey.Client.ClientId;
            assetGroup2.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            assetGroup2.AssetGroup2Id = AssetGroup2Id;
            assetGroup2.RetrieveAssetGroup2ByAssetGroup2Id(userData.DatabaseKey);
            assetGroup2.InactiveFlag = false;
            if (assetGroup2.ValidateOldClientLookupIdforAssetGroup2(userData.DatabaseKey) == true)
            {
                assetGroup2.Update(userData.DatabaseKey);
            }
            return assetGroup2.ErrorMessages;
        }
        public AssetGroup2Model GetAssetGroup2ByAssetGroup2Id(long AssetGroup2Id)
        {
            AssetGroup2Model objAssetGroup2Model = new AssetGroup2Model();
            objAssetGroup2Model.AssetGroup2Id = AssetGroup2Id;
            AssetGroup2 assetGroup2 = new AssetGroup2()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                AssetGroup2Id = AssetGroup2Id
            };
            assetGroup2.Retrieve(userData.DatabaseKey);
            if (assetGroup2 != null)
            {
                objAssetGroup2Model.ClientLookupId = assetGroup2.ClientLookupId;
                objAssetGroup2Model.Description = assetGroup2.Description;
                objAssetGroup2Model.AssetGroup2Id = assetGroup2.AssetGroup2Id;
                objAssetGroup2Model.UpdateIndex = assetGroup2.UpdateIndex;
                objAssetGroup2Model.InactiveFlag = assetGroup2.InactiveFlag;
            }
            return objAssetGroup2Model;
        }

        internal DataContracts.AssetGroup2 DeleteAssetGroup2(long AssetGroup2Id)
        {
            try
            {
                DataContracts.AssetGroup2 assetGroup2 = new DataContracts.AssetGroup2()
                {
                    AssetGroup2Id = AssetGroup2Id,
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.DatabaseKey.User.DefaultSiteId
                };
                DataContracts.Equipment eq = new DataContracts.Equipment();
                eq.ClientId = userData.DatabaseKey.Client.ClientId;
                eq.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                eq.ObjectId = assetGroup2.AssetGroup2Id;
                eq.ObjectName = "AssetGroup2";
                eq.CheckForeignFieldToDelete(userData.DatabaseKey);
                if (eq.ErrorMessages == null || eq.ErrorMessages.Count == 0)
                {
                    assetGroup2.Delete(userData.DatabaseKey);
                }
                else
                {
                    assetGroup2.ErrorMessages = eq.ErrorMessages;
                }
                return assetGroup2;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region AssetGroup3

        public List<AssetGroup3Model> PopulateAssetGroup3()
        {

            List<AssetGroup3Model> AssetGroup3ModelList = new List<AssetGroup3Model>();
            AssetGroup3Model objAssetGroup3Model;
            AssetGroup3 assetGroup3 = new AssetGroup3()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            List<AssetGroup3> AssetGroup3List = assetGroup3.RetrieveAssetGroup3ByClientIdSiteId(userData.DatabaseKey);

            foreach (var p in AssetGroup3List)
            {
                objAssetGroup3Model = new AssetGroup3Model();

                objAssetGroup3Model.AssetGroup3Id = p.AssetGroup3Id;
                objAssetGroup3Model.Description = p.Description ?? string.Empty;
                objAssetGroup3Model.InactiveFlag = p.InactiveFlag;
                objAssetGroup3Model.ClientLookupId = p.ClientLookupId;
                AssetGroup3ModelList.Add(objAssetGroup3Model);
            }
            return AssetGroup3ModelList;
        }

        public List<String> AddOrEditAssetGroup3(SiteSetUpVM objVM, ref string Mode)
        {
            AssetGroup3 assetGroup3 = new AssetGroup3();
            if (objVM.assetGroup3Model.AssetGroup3Id == 0)
            {
                Mode = "add";
                assetGroup3.ClientId = userData.DatabaseKey.Client.ClientId;
                assetGroup3.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                assetGroup3.Description = objVM.assetGroup3Model.Description.Trim() ?? string.Empty;
                assetGroup3.InactiveFlag = objVM.assetGroup3Model.InactiveFlag;
                assetGroup3.ClientLookupId = objVM.assetGroup3Model.ClientLookupId;
                if (assetGroup3.ValidateNewClientLookupIdforAssetGroup3(userData.DatabaseKey) == true)
                {
                    assetGroup3.Create(userData.DatabaseKey);
                }
            }
            else
            {
                Mode = "update";
                assetGroup3.ClientId = userData.DatabaseKey.Client.ClientId;
                assetGroup3.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                assetGroup3.AssetGroup3Id = objVM.assetGroup3Model.AssetGroup3Id;
                assetGroup3.RetrieveAssetGroup3ByAssetGroup3Id(userData.DatabaseKey);
                if (objVM.assetGroup3Model.InactiveFlag == true && assetGroup3.InactiveFlag == false)
                {
                    DataContracts.Equipment eq = new DataContracts.Equipment();
                    eq.ClientId = userData.DatabaseKey.Client.ClientId;
                    eq.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                    eq.ClientLookupId = assetGroup3.ClientLookupId;
                    eq.AssetGroup3 = assetGroup3.AssetGroup3Id;
                    eq.CheckIfAssetGroup3UsedInEquipment(userData.DatabaseKey);
                    if (eq.ErrorMessages != null && eq.ErrorMessages.Count > 0)
                    {
                        return eq.ErrorMessages;
                    }
                }
                assetGroup3.ClientLookupId = objVM.assetGroup3Model.ClientLookupId;
                assetGroup3.Description = objVM.assetGroup3Model.Description.Trim() ?? string.Empty;
                assetGroup3.InactiveFlag = objVM.assetGroup3Model.InactiveFlag;
                assetGroup3.UpdateIndex = objVM.assetGroup3Model.UpdateIndex;
                if (assetGroup3.ValidateOldClientLookupIdforAssetGroup3(userData.DatabaseKey) == true)
                {
                    assetGroup3.Update(userData.DatabaseKey);
                }

            }
            return assetGroup3.ErrorMessages;
        }
        public List<string> ActivateAssetGroup3ById(long AssetGroup3Id)
        {
            AssetGroup3 assetGroup3 = new AssetGroup3();
            assetGroup3.ClientId = userData.DatabaseKey.Client.ClientId;
            assetGroup3.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            assetGroup3.AssetGroup3Id = AssetGroup3Id;
            assetGroup3.RetrieveAssetGroup3ByAssetGroup3Id(userData.DatabaseKey);
            assetGroup3.InactiveFlag = false;
            if (assetGroup3.ValidateOldClientLookupIdforAssetGroup3(userData.DatabaseKey) == true)
            {
                assetGroup3.Update(userData.DatabaseKey);
            }
            return assetGroup3.ErrorMessages;
        }
        public AssetGroup3Model GetAssetGroup3ByAssetGroup3Id(long AssetGroup3Id)
        {
            AssetGroup3Model objAssetGroup3Model = new AssetGroup3Model();
            objAssetGroup3Model.AssetGroup3Id = AssetGroup3Id;
            AssetGroup3 assetGroup3 = new AssetGroup3()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                AssetGroup3Id = AssetGroup3Id
            };
            assetGroup3.Retrieve(userData.DatabaseKey);
            if (assetGroup3 != null)
            {
                objAssetGroup3Model.ClientLookupId = assetGroup3.ClientLookupId;
                objAssetGroup3Model.Description = assetGroup3.Description;
                objAssetGroup3Model.AssetGroup3Id = assetGroup3.AssetGroup3Id;
                objAssetGroup3Model.UpdateIndex = assetGroup3.UpdateIndex;
                objAssetGroup3Model.InactiveFlag = assetGroup3.InactiveFlag;
            }
            return objAssetGroup3Model;
        }

        internal DataContracts.AssetGroup3 DeleteAssetGroup3(long AssetGroup3Id)
        {
            try
            {
                DataContracts.AssetGroup3 assetGroup3 = new DataContracts.AssetGroup3()
                {
                    AssetGroup3Id = AssetGroup3Id,
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.DatabaseKey.User.DefaultSiteId
                };
                DataContracts.Equipment eq = new DataContracts.Equipment();
                eq.ClientId = userData.DatabaseKey.Client.ClientId;
                eq.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                eq.ObjectId = assetGroup3.AssetGroup3Id;
                eq.ObjectName = "AssetGroup3";
                eq.CheckForeignFieldToDelete(userData.DatabaseKey);
                if (eq.ErrorMessages == null || eq.ErrorMessages.Count == 0)
                {
                    assetGroup3.Delete(userData.DatabaseKey);
                }
                else
                {
                    assetGroup3.ErrorMessages = eq.ErrorMessages;
                }
                return assetGroup3;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region V2-720 Approval Group Settings
        //binding
        internal ApprovalGroupSettingsModel ApprovalGroupSettingsDetails()
        {
            ApprovalGroupSettingsModel approvalGroupSettingsModel = new ApprovalGroupSettingsModel();
            DataContracts.ApprovalGroupSettings approvalGroupSettings = new DataContracts.ApprovalGroupSettings();
            approvalGroupSettings.ClientId = userData.DatabaseKey.Client.ClientId;
            approvalGroupSettings.SiteId = this.userData.Site.SiteId;
            var cList = approvalGroupSettings.RetrieveApprovalGroupSettings_V2(this.userData.DatabaseKey);
            foreach (var item in cList)
            {
                approvalGroupSettingsModel = new ApprovalGroupSettingsModel();
                approvalGroupSettingsModel.ApprovalGroupSettingsId = item.ApprovalGroupSettingsId;
                approvalGroupSettingsModel.MaterialRequests = item.MaterialRequests;
                approvalGroupSettingsModel.PurchaseRequests = item.PurchaseRequests;
                approvalGroupSettingsModel.WorkRequests = item.WorkRequests;
                approvalGroupSettingsModel.SanitationRequests = item.SanitationRequests;
            }
            return approvalGroupSettingsModel;
        }
        #endregion
    }
}