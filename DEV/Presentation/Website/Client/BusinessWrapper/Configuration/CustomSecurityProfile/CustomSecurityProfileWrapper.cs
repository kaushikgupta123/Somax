using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Models.Configuration.CustomSecurityProfile;
using Common.Constants;
using DataContracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.BusinessWrapper.Configuration.CustomSecurityProfile
{
    public class CustomSecurityProfileWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();

        public CustomSecurityProfileWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search grid
        public List<CustomSecurityProfileSearchModel> GetCustomSecurityProfiles(string orderbycol = "", string orderDir = "", int? skip = 0, int? length = 0,
            string SearchText = "", string Name = "", string Description = "")
        {
            List<CustomSecurityProfileSearchModel> CustomsecurityProfileSearchModelList = new List<CustomSecurityProfileSearchModel>();
            CustomSecurityProfileSearchModel CustomsecurityProfileSearchModel;

            DataContracts.SecurityProfile secprof = new DataContracts.SecurityProfile()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                OrderColumn = orderbycol,
                OrderDirection = orderDir,
                Page = skip ?? 0,
                ResultsPerPage = length ?? 0,
                SearchText = SearchText,
                Name = Name,
                Description = Description
            };
            List<DataContracts.SecurityProfile> splist = new List<DataContracts.SecurityProfile>();
            splist = secprof.CustomSecurityProfileRetrieveChunkSearchV2(this.userData.DatabaseKey);

            foreach (var item in splist)
            {
                CustomsecurityProfileSearchModel = new CustomSecurityProfileSearchModel();
                CustomsecurityProfileSearchModel.SecurityProfileId = item.SecurityProfileId;
                CustomsecurityProfileSearchModel.Name = item.Name;
                CustomsecurityProfileSearchModel.Description = item.Description;
                CustomsecurityProfileSearchModel.TotalCount = item.TotalCount;
                CustomsecurityProfileSearchModelList.Add(CustomsecurityProfileSearchModel);
            }

            return CustomsecurityProfileSearchModelList;
        }
        #endregion

        public CustomSecurityProfileModel GetEditSecurityProfileDetailsById(long SecurityProfileId)
        {
            CustomSecurityProfileModel objcspModel = new CustomSecurityProfileModel();
            DataContracts.SecurityProfile securityprofile = new DataContracts.SecurityProfile()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SecurityProfileId = SecurityProfileId

            };
            securityprofile.Retrieve(_dbKey);
            objcspModel = initializeDetailsControls(securityprofile);
            return objcspModel;
        }
        public CustomSecurityProfileModel initializeDetailsControls(DataContracts.SecurityProfile obj)
        {
            CustomSecurityProfileModel customsecurityprofile = new CustomSecurityProfileModel();
            customsecurityprofile.SecurityProfileId = obj.SecurityProfileId;
            customsecurityprofile.Name = obj.Name;
            customsecurityprofile.Description = obj.Description;

            return customsecurityprofile;
        }

        #region Add or edit securityprofile
        public DataContracts.SecurityProfile AddOrEditSecurityProfile(CustomSecurityProfileVM objCSP)
        {
            DataContracts.SecurityProfile securityprofile;
            securityprofile = new DataContracts.SecurityProfile()
            { 
                ClientId = _dbKey.Client.ClientId,
                Name= objCSP.customSecurityProfileModel.Name.Trim(),
                SecurityProfileId = objCSP.customSecurityProfileModel.SecurityProfileId
            };
           
            securityprofile.ValidateProfileName(this.userData.DatabaseKey);
            List<string> errList = new List<string>();
            if (securityprofile.ErrorMessages == null || securityprofile.ErrorMessages.Count == 0)
            {
                if (objCSP.customSecurityProfileModel.SecurityProfileId == 0)
                {
                    #region Insert in Security profile Table
                    #region get ProductGrouping and Package Level
                    int Productgrouping = 0;
                    string packagelevel = "";                   
                    Productgrouping = GetProductGrouping();
                    packagelevel = _dbKey.Client.PackageLevel;
                    #endregion
                    securityprofile = new DataContracts.SecurityProfile();
                    securityprofile.ClientId = this.userData.DatabaseKey.User.ClientId;
                    securityprofile.Name = objCSP.customSecurityProfileModel.Name.Trim();
                    securityprofile.Description = objCSP.customSecurityProfileModel.Description.Trim();
                    securityprofile.SortOrder = 0;
                    securityprofile.Protected = false;
                    securityprofile.UserType = UserTypeConstants.Full;
                    securityprofile.ProductGrouping = Productgrouping;
                    securityprofile.PackageLevel = packagelevel;
                    securityprofile.CMMSUser = false;
                    securityprofile.SanitationUser = false;
                    securityprofile.APMUser = false;
                    securityprofile.Create(this.userData.DatabaseKey);

                    if (securityprofile.ErrorMessages == null)
                    {
                        securityprofile.SecurityItemAdd(this.userData.DatabaseKey);
                    }
                    #endregion
                }
                else
                {
                    #region Update Security profile Table
                    securityprofile = new DataContracts.SecurityProfile()
                    {
                        ClientId = _dbKey.Client.ClientId,
                        SecurityProfileId = objCSP.customSecurityProfileModel.SecurityProfileId
                    };
                    securityprofile.Retrieve(_dbKey);

                    securityprofile.Name = objCSP.customSecurityProfileModel.Name.Trim();
                    securityprofile.Description = objCSP.customSecurityProfileModel.Description.Trim();                   
                    securityprofile.Update(this.userData.DatabaseKey);
                    #endregion
                }

            }
            return securityprofile;
        }
        #endregion

        public List<CustomSecurityItemModel> GetCustomAllSecurityItemV2List(long SecurityProfileId, string SecurityItemTab)
        {
            List<CustomSecurityItemModel> CustomSecurityItemModelList = new List<CustomSecurityItemModel>();
            CustomSecurityItemModel objCustomSecurityItemModel;
            string securityLocalizedName = string.Empty;
            List<DataContracts.SecurityItem> securityitemlist = new List<DataContracts.SecurityItem>();
            DataContracts.SecurityItem securityitem = new DataContracts.SecurityItem()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SecurityProfileId = SecurityProfileId,
                SecurityItemTab = SecurityItemTab,
                PackageLevel = userData.DatabaseKey.Client.PackageLevel,
                ProductGrouping = GetProductGrouping()

            };
            securityitemlist = securityitem.CustomRetrieveAllV2(this.userData.DatabaseKey);

            foreach (var item in securityitemlist)
            {
                if (!string.IsNullOrEmpty(item.ItemName))
                {
                    securityLocalizedName = UtilityFunction.GetMessageFromResource(item.ItemName.Replace("-", ""), LocalizeResourceSetConstants.SecurityProfileItemsDetails);
                }
                objCustomSecurityItemModel = new CustomSecurityItemModel();
                objCustomSecurityItemModel.SingleItem = item.SingleItem;
                objCustomSecurityItemModel.ReportItem = item.ReportItem;
                objCustomSecurityItemModel.SecurityItemId = item.SecurityItemId;
                objCustomSecurityItemModel.SecurityProfileId = item.SecurityProfileId;
                objCustomSecurityItemModel.SortOrder = item.SortOrder;
                objCustomSecurityItemModel.SecurityLocalizedName = !string.IsNullOrEmpty(securityLocalizedName) ? securityLocalizedName : item.ItemName;
                objCustomSecurityItemModel.Protected = item.Protected;
                objCustomSecurityItemModel.ItemName = item.ItemName;
                objCustomSecurityItemModel.ItemAccess = item.ItemAccess;
                objCustomSecurityItemModel.ItemCreate = item.ItemCreate;
                objCustomSecurityItemModel.ItemEdit = item.ItemEdit;
                objCustomSecurityItemModel.ItemDelete = item.ItemDelete;
                objCustomSecurityItemModel.UpdateIndex = item.UpdateIndex;
                CustomSecurityItemModelList.Add(objCustomSecurityItemModel);
            }
            return CustomSecurityItemModelList;

        }

        private int GetProductGrouping()
        {
            // RKL - 2020-11-17 - Included the Fleet settings 
            // New product groupings
            int productGrouping = 0;
            var siteData = userData.Site;
            //-------Product Grouping Values----//
            // CMMS Only 
            if (siteData.CMMS == true && siteData.APM == false && siteData.Sanitation == false && siteData.Fleet == false)      //---These 4 conditions denote product grouping=1
            {
                productGrouping = 1;
            }
            // Sanitation Only
            else if (siteData.CMMS == false && siteData.APM == false && siteData.Sanitation == true && siteData.Fleet == false) //---These 4 conditions denote product grouping=2
            {
                productGrouping = 2;
            }
            // APM Only
            else if (siteData.CMMS == false && siteData.APM == true && siteData.Sanitation == false && siteData.Fleet == false) //---These 4 conditions denote product grouping=3
            {
                productGrouping = 3;
            }
            // CMMS and Sanitation
            else if (siteData.CMMS == true && siteData.APM == false && siteData.Sanitation == true && siteData.Fleet == false)  //---These 4 conditions denote product grouping=4
            {
                productGrouping = 4;
            }
            // CMMS and APM
            else if (siteData.CMMS == true && siteData.APM == true && siteData.Sanitation == false && siteData.Fleet == false)  //---These 4 conditions denote product grouping=5
            {
                productGrouping = 5;
            }
            // CMMS and APM and Sanitation
            else if (siteData.CMMS == true && siteData.APM == true && siteData.Sanitation == true && siteData.Fleet == false)   //---These 4 conditions denote product grouping=6
            {
                productGrouping = 6;
            }
            // Fleet Only 
            else if (siteData.CMMS == false && siteData.APM == false && siteData.Sanitation == false && userData.Site.Fleet == true)
            {
                productGrouping = 7;
            }
            // CMMS and Fleet
            else if (siteData.CMMS == true && siteData.APM == false && siteData.Sanitation == false && userData.Site.Fleet == true)
            {
                productGrouping = 8;
            }
            // Fleet and APM
            else if (siteData.CMMS == false && siteData.APM == true && siteData.Sanitation == false && userData.Site.Fleet == true)
            {
                productGrouping = 9;
            }
            // CMMS and APM and FLEET
            else if (siteData.CMMS == true && siteData.APM == true && siteData.Sanitation == false && userData.Site.Fleet == true)
            {
                productGrouping = 10;
            }
            return productGrouping;
        }

        internal void UpdateSecurityItem(List<CustomSecurityItemModel> objlist)
        {
            List<DataContracts.SecurityItem> securityitemlists = new List<DataContracts.SecurityItem>();
            DataContracts.SecurityItem securityitem;
            Parallel.ForEach(objlist, item =>
            {
                securityitem = new DataContracts.SecurityItem()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SecurityProfileId = item.SecurityProfileId,
                    SingleItem = item.SingleItem,
                    ReportItem = item.ReportItem,
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

            if (securityitemlists.Count > 0)
            {
                foreach (DataContracts.SecurityItem obj in securityitemlists)
                {
                        obj.Update(this.userData.DatabaseKey);
                }
            }
        }

    }
}