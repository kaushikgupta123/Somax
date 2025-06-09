using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Models.Configuration.SecurityProfile;
using Common.Constants;
using DataContracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.BusinessWrapper.Configuration.SecurityProfile
{
    public class SecurityProfileWrapper 
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();

        public SecurityProfileWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        public List<SecurityProfileModel> GetProfiles()
        {
            List<SecurityProfileModel> securityProfileModelList = new List<SecurityProfileModel>();
            SecurityProfileModel securityProfileModel;

            DataContracts.SecurityProfile secprof = new DataContracts.SecurityProfile()
            {
                ClientId = userData.DatabaseKey.Client.ClientId
            };
            List<DataContracts.SecurityProfile> splist = new List<DataContracts.SecurityProfile>();
            
            ////-----Test 342 profile list-----
            //secprof.PackageLevel = "basic";
            //secprof.ProductGrouping = 1;
            //splist = secprof.RetrieveByPackageLevel(userData.DatabaseKey);
            ////----- END -----
            // RKL - 2020-11-17 - V2 is not using any profiles < 100
            //     - Modified the stored procedure to not return these profiles
            //     - Currently not supporting Custom Profiles - will have to modify this later
            // 
            if (this.userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise.ToUpper() && this.userData.DatabaseKey.User.IsSuperUser == true)
            {

                secprof.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;
                splist = secprof.RetrieveAllProfilesForEnterPrisePackagelevel(this.userData.DatabaseKey);                 
                List<DataContracts.SecurityProfile> splist_sorted = splist.AsQueryable().OrderBy(x => x.SortOrder).ToList();

                foreach (var item in splist_sorted)
                {
                    if(item.SecurityProfileId>=6 && item.SecurityProfileId<=18)
                    {
                        securityProfileModel = new SecurityProfileModel();
                        securityProfileModel.SecurityProfileId = item.SecurityProfileId;
                        securityProfileModel.Name = item.Name;
                        securityProfileModel.Description = item.Description;//UtilityFunction.GetMessageFromResource(item.Name, LocalizeResourceSetConstants.UserDetails);
                        securityProfileModelList.Add(securityProfileModel);
                    }                    
                }
            }
            else
            {
                splist = secprof.RetrieveAllProfiles(this.userData.DatabaseKey);
                List<DataContracts.SecurityProfile> splist_sorted = splist.AsQueryable().OrderBy(x => x.SortOrder).ToList();
               // Site is Fleet Only and Client Package Level is Professional  408
                if (userData.Site.CMMS==false && userData.Site.Sanitation == false && userData.Site.APM == false && userData.Site.Fleet == true  && this.userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Professional.ToUpper())
                {                   
                        splist_sorted = splist.FindAll(x => x.ProductGrouping == 7 && x.PackageLevel.ToUpper() == PackageLevelConstant.Professional.ToUpper());
                }
                //Site is Fleet Only and Client Package Level is Enterprise  408
                else if (userData.Site.CMMS == false && userData.Site.Sanitation == false && userData.Site.APM == false && userData.Site.Fleet == true && this.userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise.ToUpper())
                {
                    splist_sorted = splist.FindAll(x => x.ProductGrouping == 7 && x.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise.ToUpper());

                }
                //Site is CMMS and Fleet and Client Package Level is Professional 408
                else if (userData.Site.CMMS == true && userData.Site.Sanitation == false && userData.Site.APM == false && userData.Site.Fleet == true && this.userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Professional.ToUpper())
                {
                    splist_sorted = splist.FindAll(x => x.ProductGrouping == 8 && x.PackageLevel.ToUpper() == PackageLevelConstant.Professional.ToUpper());
                }
                //Site is CMMS and Fleet and Client Package Level is Enterprise  408
                else if (userData.Site.CMMS == true && userData.Site.Sanitation == false && userData.Site.APM == true && this.userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise.ToUpper())
                {
                    splist_sorted = splist.FindAll(x => x.ProductGrouping == 8 && x.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise.ToUpper());

                }
                else
                {
                    if (userData.DatabaseKey.Client.ClientId != 4 || userData.DatabaseKey.Client.ClientId != 6)
                    {
                        splist_sorted = splist.FindAll(x => x.Name != SecurityProfileConstants.SOMAX_Inventory);
                    }
                }
                foreach (var item in splist_sorted)
                {
                    securityProfileModel = new SecurityProfileModel();
                    securityProfileModel.SecurityProfileId = item.SecurityProfileId;
                    securityProfileModel.Name = item.Name;
                    securityProfileModel.Description = item.Description;//UtilityFunction.GetMessageFromResource(item.Name, LocalizeResourceSetConstants.UserDetails);
                    securityProfileModelList.Add(securityProfileModel);
                }

            }
        
            return securityProfileModelList;
        }
        public List<DataContracts.SecurityItem> GridSource(long SecurityProfileId)
        {
            List<DataContracts.SecurityItem> securityitemlist = new List<DataContracts.SecurityItem>();
            DataContracts.SecurityItem securityitem = new DataContracts.SecurityItem()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SecurityProfileId = SecurityProfileId,
                AccessingClientId = userData.DatabaseKey.Client.ClientId
            };
            securityitemlist = securityitem.RetrieveAllByClientAndSecurityProfile(this.userData.DatabaseKey);
            if (userData.DatabaseKey.Client.ClientId != 6 && userData.DatabaseKey.Client.ClientId != 4)
            {
                securityitemlist.RemoveAll(x => x.ItemName == SecurityConstants.BBUMaintStats || x.ItemName == SecurityConstants.BBUMaintStats_Extract);
            }
            //securityitemlist.RemoveAll(x => x.ItemName == SecurityConstants.Gauges
            //                             || x.ItemName == SecurityConstants.Gauge_Reading);
            securityitemlist.RemoveAll(x => x.ItemName == SecurityConstants.ShoppingCart_AutoGen);
            if (userData.DatabaseKey.Client.PackageLevel.ToUpper() ==PackageLevelConstant.Enterprise)
            {
                if (!userData.Site.UsePartMaster)
                {
                    securityitemlist.RemoveAll(x => x.ItemName == SecurityConstants.PartMaster
                                                  || x.ItemName == SecurityConstants.ManufacturerMaster
                                                  || x.ItemName == SecurityConstants.PartCategoryMaster
                                                  //|| x.ItemName == SecurityConstants.ShoppingCart
                                                  //|| x.ItemName == SecurityConstants.ShoppingCart_Approve
                                                  //|| x.ItemName == SecurityConstants.ShoppingCart_Review
                                                  );
                }
                if (!userData.Site.ShoppingCart)
                {
                    securityitemlist.RemoveAll(x => x.ItemName == SecurityConstants.ShoppingCart
                                                 || x.ItemName == SecurityConstants.ShoppingCart_Approve
                                                 || x.ItemName == SecurityConstants.ShoppingCart_Review
                                                 || x.ItemName == SecurityConstants.ShoppingCart_AutoGen);
                }



                if (!userData.Site.UseVendorMaster)
                {
                    securityitemlist.RemoveAll(x => x.ItemName == SecurityConstants.VendorMaster
                                                  || x.ItemName == SecurityConstants.VendorCatalog);

                }
                if (userData.DatabaseKey.Client.BusinessType.ToUpper() != BusinessTypeConstants.FoodServices)
                {
                    securityitemlist.RemoveAll(x => x.ItemName == SecurityConstants.Sanitation
                                                 || x.ItemName == SecurityConstants.SanitationJob
                                                 || x.ItemName == SecurityConstants.SanitationJob_ApprovalWorkbench
                                                 || x.ItemName == SecurityConstants.SanitationJob_CreateRequest
                                                 || x.ItemName == SecurityConstants.Sanitation_Job_Gen
                                                 || x.ItemName == SecurityConstants.Sanitation_OnDemand
                                                 || x.ItemName == SecurityConstants.Sanitation_Verification
                                                 || x.ItemName == SecurityConstants.Sanitation_WB);
                }
            }
            // Remove non-enterprise stuff 
            else
            {
                securityitemlist.RemoveAll(x => x.ItemName == SecurityConstants.PartMaster
                                              || x.ItemName == SecurityConstants.ManufacturerMaster
                                              || x.ItemName == SecurityConstants.PartCategoryMaster
                                              || x.ItemName == SecurityConstants.VendorMaster
                                              || x.ItemName == SecurityConstants.VendorCatalog
                                              //|| x.ItemName == SecurityConstants.ShoppingCart
                                              //|| x.ItemName == SecurityConstants.ShoppingCart_Approve
                                              //|| x.ItemName == SecurityConstants.ShoppingCart_Review
                                              );
                //----Shopping cart items are removed from Site.UsePartmaster to Site.Shoppingcart for Som-1604---//
                if (!userData.Site.ShoppingCart)
                {
                    securityitemlist.RemoveAll(x => x.ItemName == SecurityConstants.ShoppingCart
                                                 || x.ItemName == SecurityConstants.ShoppingCart_Approve
                                                 || x.ItemName == SecurityConstants.ShoppingCart_Review
                                                 || x.ItemName == SecurityConstants.ShoppingCart_AutoGen);
                }

                if (userData.DatabaseKey.Client.BusinessType.ToUpper() != BusinessTypeConstants.FoodServices)
                {
                    securityitemlist.RemoveAll(x => x.ItemName == SecurityConstants.Sanitation
                                                 || x.ItemName == SecurityConstants.SanitationJob
                                                 || x.ItemName == SecurityConstants.SanitationJob_ApprovalWorkbench
                                                 || x.ItemName == SecurityConstants.SanitationJob_CreateRequest
                                                 || x.ItemName == SecurityConstants.Sanitation_Job_Gen
                                                 || x.ItemName == SecurityConstants.Sanitation_OnDemand
                                                 || x.ItemName == SecurityConstants.Sanitation_Verification
                                                 || x.ItemName == SecurityConstants.Sanitation_WB);
                }

            }
          
            return securityitemlist;
        }
        public List<SecurityItemModel> GetSecurityItemList(long SecurityProfileId)
        {
            List<SecurityItemModel> SecurityItemModelList = new List<SecurityItemModel>();
            SecurityItemModel objSecurityItemModel;
            string securityLocalizedName = string.Empty;
            List<DataContracts.SecurityItem> securityitemlist = new List<DataContracts.SecurityItem>();
            securityitemlist = GridSource(SecurityProfileId);
            foreach (var item in securityitemlist)
            {
                if (!string.IsNullOrEmpty(item.ItemName))
                {
                    securityLocalizedName = UtilityFunction.GetMessageFromResource(item.ItemName.Replace("-", ""), LocalizeResourceSetConstants.SecurityProfileItemsDetails);
                }
                objSecurityItemModel = new SecurityItemModel();
                objSecurityItemModel.SingleItem = item.SingleItem;
                objSecurityItemModel.SecurityItemId = item.SecurityItemId;
                objSecurityItemModel.SecurityProfileId = item.SecurityProfileId;
                objSecurityItemModel.SortOrder = item.SortOrder;
                objSecurityItemModel.SecurityLocalizedName = !string.IsNullOrEmpty(securityLocalizedName) ? securityLocalizedName : item.ItemName;
                objSecurityItemModel.Protected = item.Protected;
                objSecurityItemModel.ItemName = item.ItemName;
                objSecurityItemModel.ItemAccess = item.ItemAccess;
                objSecurityItemModel.ItemCreate = item.ItemCreate;
                objSecurityItemModel.ItemEdit = item.ItemEdit;
                objSecurityItemModel.ItemDelete = item.ItemDelete;
                objSecurityItemModel.UpdateIndex = item.UpdateIndex;
                SecurityItemModelList.Add(objSecurityItemModel);
            }
            return SecurityItemModelList;
            //}

        }
        protected void GetSecuritySingleItemList()
        {
            List<DataContracts.SecurityItem> securityitemlist = new List<DataContracts.SecurityItem>();
            securityitemlist = GridSource(1);

            securityitemlist = securityitemlist.FindAll(x => x.SingleItem.Equals(true));

            //GridViewDataColumn colProtected = (GridViewDataColumn)grdSecurityItemList.Columns[1];
            //ASPxCheckBox chkProfileProtectd = (ASPxCheckBox)grdSecurityItemList.FindHeaderTemplateControl(colProtected, "chkProfileProtected");
            //if (chkProfileProtectd != null)
            //{
            //    chkProfileProtectd.Checked = securityitemlist.Count > 0 ? securityitemlist[0].Protected : false;
            //    chkProfileProtectd.Enabled = false;// !chkProfileProtectd.Checked;
            //}

        }

        internal void UpdateSecurityItem(List<SecurityItemModel> objlist, bool SingleItem)
        {
            List<DataContracts.SecurityItem> securityitemlists = new List<DataContracts.SecurityItem>();
            DataContracts.SecurityItem securityitem;
            Parallel.ForEach(objlist, item =>
            {
                securityitem = new DataContracts.SecurityItem()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SecurityProfileId = item.SecurityProfileId,
                    SingleItem = SingleItem,
                    SecurityItemId = item.SecurityItemId,
                    Protected = item.Protected,
                    ItemName = item.ItemName,
                    SortOrder = item.SortOrder,
                    ItemAccess = item.ItemAccess,
                    ItemCreate = item.ItemCreate,
                    ItemEdit = item.ItemEdit,
                    ItemDelete = item.ItemDelete,
                    UpdateIndex = item.UpdateIndex
                };
                securityitemlists.Add(securityitem);
            });
            //foreach (var item in objlist)
            //{

            //    securityitem = new DataContracts.SecurityItem()
            //    {
            //        ClientId = userData.DatabaseKey.Client.ClientId,
            //        SecurityProfileId = item.SecurityProfileId,
            //        SingleItem = SingleItem,
            //        SecurityItemId = item.SecurityItemId,
            //        //tar age protected pathate hobe html thke
            //        Protected = item.Protected,
            //        ItemName = item.ItemName,
            //        SortOrder = item.SortOrder,
            //        ItemAccess = item.ItemAccess,
            //        ItemCreate = item.ItemCreate,
            //        ItemEdit = item.ItemEdit,
            //        ItemDelete = item.ItemDelete,
            //        UpdateIndex = item.UpdateIndex
            //    };
            //    securityitemlists.Add(securityitem);
            //}
            if (securityitemlists.Count > 0)
            {
                foreach (DataContracts.SecurityItem obj in securityitemlists)
                {
                    if (!obj.Protected)
                    {
                        obj.Update(this.userData.DatabaseKey);
                    }
                }
            }
        }
    }
}