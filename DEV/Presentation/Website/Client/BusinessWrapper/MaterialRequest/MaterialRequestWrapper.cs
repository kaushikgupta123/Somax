using Client.BusinessWrapper.Common;
using Client.Models.MaterialRequest;

using Common.Constants;
using Common.Enumerations;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.BusinessWrapper
{
    public class MaterialRequestWrapper : CommonWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        internal List<string> errorMessage = new List<string>();

        public MaterialRequestWrapper(UserData userData) : base(userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search
        public List<MaterialRequestModel> GetMaterialRequestChunkList(int CustomQueryDisplayId, int skip = 0, int length = 0, string orderbycol = "",
            string orderDir = "", long? MaterialRequestId = null, string Description = "", string Status = "", string AccountClientLookupId = "", DateTime? RequiredDate = null, DateTime? CreateDate = null, DateTime? CompleteDate = null, string SearchText = "")
        {
            MaterialRequest materialRequest = new MaterialRequest();
            MaterialRequestModel mrModel;
            List<MaterialRequestModel> MaterialRequestModelList = new List<MaterialRequestModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            materialRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            materialRequest.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            materialRequest.CustomQueryDisplayId = CustomQueryDisplayId;
            materialRequest.orderbyColumn = orderbycol;
            materialRequest.orderBy = orderDir;
            materialRequest.offset1 = Convert.ToString(skip);
            materialRequest.nextrow = Convert.ToString(length);

            materialRequest.MaterialRequestId = MaterialRequestId.HasValue ? MaterialRequestId.Value : 0;
            materialRequest.AccountClientLookupId = AccountClientLookupId;
            materialRequest.RequiredDate = RequiredDate;
            materialRequest.CreateDate = CreateDate;
            materialRequest.CompleteDate = CompleteDate;
            materialRequest.Description = Description;
            materialRequest.Status = Status;
            materialRequest.SearchText = SearchText;

            List<MaterialRequest> materialRequestList = materialRequest.RetrieveChunkSearch(this.userData.DatabaseKey);
            if (materialRequestList != null)
            {
                foreach (var item in materialRequestList)
                {
                    mrModel = new MaterialRequestModel();
                    mrModel.ClientId = item.ClientId;
                    mrModel.MaterialRequestId = item.MaterialRequestId;
                    mrModel.Description = item.Description;
                    mrModel.Status = item.Status;
                    mrModel.Account_ClientLookupId = item.AccountClientLookupId;
                    if (item.CreateDate != null && item.CreateDate == default(DateTime))
                    {
                        mrModel.CreateDate = null;
                    }
                    else
                    {
                        mrModel.CreateDate = item.CreateDate;
                    }
                    if (item.RequiredDate != null && item.RequiredDate == default(DateTime))
                    {
                        mrModel.RequiredDate = null;
                    }
                    else
                    {
                        mrModel.RequiredDate = item.RequiredDate;
                    }
                    if (item.CompleteDate != null && item.CompleteDate == default(DateTime))
                    {
                        mrModel.CompleteDate = null;
                    }
                    else
                    {
                        mrModel.CompleteDate = item.CompleteDate;
                    }
                    mrModel.ChildCount = item.ChildCount;
                    mrModel.TotalCount = item.TotalCount;
                    MaterialRequestModelList.Add(mrModel);
                }
            }
            return MaterialRequestModelList;
        }

        public List<ChildGridModel> PopulateChilditems(long MaterialRequestId)
        {
            ChildGridModel objChildGrid;
            List<ChildGridModel> ChildItemList = new List<ChildGridModel>();

            EstimatedCosts estimatedCosts = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = MaterialRequestId,
                ObjectType = MaterialRequestConstant.MeterialRequest
            };
            List<EstimatedCosts> estimatedCostsList = EstimatedCosts.EstimatedCostsRetrieveByObjectId_ForChild(this.userData.DatabaseKey, estimatedCosts);

            if (estimatedCostsList != null)
            {
                foreach (var item in estimatedCostsList)
                {
                    objChildGrid = new ChildGridModel();
                    objChildGrid.PartClientLookupId = item.PartClientLookupId;
                    objChildGrid.EstimatedCostsId = item.EstimatedCostsId;
                    objChildGrid.ObjectId = item.ObjectId;
                    objChildGrid.CategoryId = item.CategoryId;
                    objChildGrid.Description = item.Description;
                    objChildGrid.UnitCost = Math.Round(item.UnitCost, 2);
                    objChildGrid.TotalCost = Math.Round(item.TotalCost, 2);
                    objChildGrid.Quantity = item.Quantity;
                    objChildGrid.Status = item.Status;
                    ChildItemList.Add(objChildGrid);
                }
            }

            return ChildItemList;
        }
        #endregion

        #region Add/Edit

        public MaterialRequest SaveMaterialRequest(MaterialRequestModel _mrModel)
        {
            MaterialRequest objMaterialRequest = new DataContracts.MaterialRequest();
            if (_mrModel.MaterialRequestId != 0)
            {
                objMaterialRequest = EditMaterialRequest(_mrModel);
            }
            else
            {
                objMaterialRequest = AddMaterialRequest(_mrModel);
            }
            return objMaterialRequest;
        }
        public MaterialRequest AddMaterialRequest(MaterialRequestModel mrModel)
        {
            MaterialRequest materialRequest = new MaterialRequest();

            materialRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            materialRequest.SiteId = _dbKey.Personnel.SiteId;
            materialRequest.Requestor_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            materialRequest.Description = mrModel.Description ?? string.Empty;
            materialRequest.AccountId = mrModel.AccountId ?? 0;
            materialRequest.RequiredDate = mrModel.RequiredDate;
            materialRequest.Status = MaterialRequestConstant.Status;

            materialRequest.Create(_dbKey);

            return materialRequest;

        }

        public MaterialRequest EditMaterialRequest(MaterialRequestModel mrModel)
        {

            MaterialRequest materialRequest = new MaterialRequest()
            {
                MaterialRequestId = mrModel.MaterialRequestId
            };
            materialRequest.Retrieve(_dbKey);

            materialRequest.AccountId = mrModel.AccountId ?? 0;
            materialRequest.RequiredDate = mrModel.RequiredDate;
            materialRequest.Description = mrModel.Description ?? string.Empty;
            if (materialRequest.CompleteDate != null && materialRequest.CompleteDate.Value == default(DateTime))
            {
                materialRequest.CompleteDate = null;
            }

            materialRequest.Update(userData.DatabaseKey);
            if (materialRequest.ErrorMessages != null && materialRequest.ErrorMessages.Count > 0)
            {
                return materialRequest;
            }
            return materialRequest;
        }
        #endregion

        #region Deatails
        public MaterialRequestModel PopulateMaterialRequestDetails(long PartId)
        {
            MaterialRequestModel objMaterialRequest = new MaterialRequestModel();
            MaterialRequest obj = new MaterialRequest()
            {
                ClientId = _dbKey.Client.ClientId,
                MaterialRequestId = PartId
            };
            obj.RetriveByMaterialRequestId(userData.DatabaseKey);
            objMaterialRequest = initializeControls(obj);

            return objMaterialRequest;
        }
        public MaterialRequestModel initializeControls(MaterialRequest obj)
        {
            MaterialRequestModel objMaterialRequest = new MaterialRequestModel();
            objMaterialRequest.ClientId = obj.ClientId;

            objMaterialRequest.MaterialRequestId = obj.MaterialRequestId;
            objMaterialRequest.AccountId = obj?.AccountId ?? 0;
            objMaterialRequest.Account_ClientLookupId = obj?.AccountClientLookupId ?? string.Empty;
            objMaterialRequest.CreateDate = obj?.CreateDate ?? DateTime.MinValue;
            objMaterialRequest.Personnel_NameFirst = obj?.Personnel_NameFirst ?? string.Empty;
            objMaterialRequest.Personnel_NameLast = obj?.Personnel_NameLast ?? string.Empty;
            objMaterialRequest.Description = obj?.Description ?? string.Empty;
            objMaterialRequest.RequiredDate = obj?.RequiredDate ?? DateTime.MinValue;
            objMaterialRequest.CompleteDate = obj?.CompleteDate ?? DateTime.MinValue;
            objMaterialRequest.Status = obj?.Status ?? string.Empty;
            //V2-732
            //objMaterialRequest.IsUseMultiStoreroom=this._dbKey.Client.UseMultiStoreroom;

            return objMaterialRequest;
        }
        #endregion

        #region Add Part Not In Inventory
        public EstimatedCosts AddPartNotInInventory(PartNotInInventoryModel ChildModel)
        {
            EstimatedCosts estimatedCosts = new EstimatedCosts();
            //V2-726 Start
            ApprovalGroupSettings approvalGroupSettings = new ApprovalGroupSettings
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId
            };
            var approvalGroupSettingsList = approvalGroupSettings.RetrieveApprovalGroupSettings_V2(userData.DatabaseKey).FirstOrDefault();
            //V2-726 End

            estimatedCosts.ClientId = ChildModel.ClientId;
            estimatedCosts.ObjectId = ChildModel.ObjectId;
            estimatedCosts.ObjectType = MaterialRequestConstant.MeterialRequest;
            estimatedCosts.Description = ChildModel.Description;
            estimatedCosts.Category = MaterialRequestConstant.Category;
            estimatedCosts.Quantity = ChildModel.Quantity ?? 0;
            estimatedCosts.Source = MaterialRequestConstant.Source;
            //V2-726 Start
            if (approvalGroupSettingsList != null)
            {
                if (approvalGroupSettingsList.MaterialRequests == true)
                {
                    estimatedCosts.Status = MaterialRequestLineStatus.Initiated;
                }
                else
                {
                    estimatedCosts.Status = MaterialRequestLineStatus.Approved;
                }
            }
            else
            {
                estimatedCosts.Status = MaterialRequestLineStatus.Approved;
            }
            //V2-726 End

            //materialRequest.ValidateAddMultiStoreroomPart(this.userData.DatabaseKey);
            //if (materialRequest.ErrorMessages==null||materialRequest.ErrorMessages.Count==0)
            //{
            estimatedCosts.Create(_dbKey);
            //}
            return estimatedCosts;

        }
        public PartNotInInventoryModel GetLineItem(long EstimatedCostsId, long MaterialRequestId)
        {
            // Create a new instance of PartNotInInventoryModel
            PartNotInInventoryModel objPartNotInInventoryModel = new PartNotInInventoryModel();

            // Create a new instance of EstimatedCosts and set its properties
            EstimatedCosts estimatedCosts = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = MaterialRequestId,
                ObjectType = MaterialRequestConstant.MeterialRequest
            };

            // Retrieve the list of EstimatedCosts items for the given MaterialRequestId
            List<EstimatedCosts> EstmatedCostsItems = EstimatedCosts.EstimatedCostsRetrieveByObjectId_ForChild(this.userData.DatabaseKey, estimatedCosts);

            // Find the specific EstimatedCosts item by EstimatedCostsId
            var selectedEstmatedCostsItems = EstmatedCostsItems != null ? EstmatedCostsItems.Where(x => x.EstimatedCostsId == EstimatedCostsId).SingleOrDefault() : null;

            // Set the properties of objPartNotInInventoryModel based on the selected EstimatedCosts item
            objPartNotInInventoryModel.EstimatedCostsId = selectedEstmatedCostsItems.EstimatedCostsId;
            objPartNotInInventoryModel.ClientId = selectedEstmatedCostsItems.ClientId;
            objPartNotInInventoryModel.Quantity = selectedEstmatedCostsItems.Quantity;
            objPartNotInInventoryModel.ObjectId = selectedEstmatedCostsItems.ObjectId;
            objPartNotInInventoryModel.Description = selectedEstmatedCostsItems.Description;
            objPartNotInInventoryModel.CategoryId = selectedEstmatedCostsItems.CategoryId;
            objPartNotInInventoryModel.PartClientLookupId = selectedEstmatedCostsItems.PartClientLookupId;

            // V2-1148: Set additional properties
            objPartNotInInventoryModel.UnitCost = selectedEstmatedCostsItems?.UnitCost ?? 00;
            objPartNotInInventoryModel.Unit = selectedEstmatedCostsItems.Unit;
            objPartNotInInventoryModel.AccountId = selectedEstmatedCostsItems.AccountId;
            objPartNotInInventoryModel.AccountClientLookupId = selectedEstmatedCostsItems.AccountClientLookupId;
            objPartNotInInventoryModel.VendorId = selectedEstmatedCostsItems.VendorId;
            objPartNotInInventoryModel.VendorClientLookupId = selectedEstmatedCostsItems.VendorClientLookupId;
            objPartNotInInventoryModel.PartCategoryMasterId = selectedEstmatedCostsItems.UNSPSC;
            objPartNotInInventoryModel.PartCategoryClientLookupId = selectedEstmatedCostsItems.PartCategoryClientLookupId;

            // Return the populated PartNotInInventoryModel
            return objPartNotInInventoryModel;
        }

        public EstimatedCosts EditPartNotInInventory(PartNotInInventoryModel partNotInInventoryModel)
        {
            // Retrieve the EstimatedCosts object from the database using the provided EstimatedCostsId
            EstimatedCosts estimatedCosts = new EstimatedCosts()
            {
                EstimatedCostsId = partNotInInventoryModel.EstimatedCostsId
            };
            estimatedCosts.Retrieve(_dbKey);

            // Update the properties of the EstimatedCosts object with the values from the partNotInInventoryModel
            estimatedCosts.Description = partNotInInventoryModel.Description;
            estimatedCosts.Quantity = partNotInInventoryModel.Quantity ?? 0;

            #region V2-1148 Update additional properties based on the partNotInInventoryModel
            estimatedCosts.UnitOfMeasure = !string.IsNullOrEmpty(partNotInInventoryModel.Unit) ? partNotInInventoryModel.Unit : "";
            estimatedCosts.AccountId = partNotInInventoryModel?.AccountId ?? 0;
            estimatedCosts.AccountClientLookupId = partNotInInventoryModel.AccountClientLookupId;
            estimatedCosts.VendorId = partNotInInventoryModel?.VendorId ?? 0;
            estimatedCosts.VendorClientLookupId = partNotInInventoryModel.VendorClientLookupId;
            estimatedCosts.UnitCost = partNotInInventoryModel.UnitCost ?? 0;
            // Update the UNSPSC and UnitCost properties based on the ShoppingCart flag
            if (userData.Site.ShoppingCart == false)
            {
                estimatedCosts.UNSPSC = 0;
                estimatedCosts.PartClientLookupId = null;
            }
            else
            {
                estimatedCosts.UNSPSC = partNotInInventoryModel.PartCategoryMasterId ?? 0;
            }
            #endregion

            // Update the EstimatedCosts object in the database
            estimatedCosts.Update(userData.DatabaseKey);

            // Check for any error messages and return the EstimatedCosts object
            if (estimatedCosts.ErrorMessages != null && estimatedCosts.ErrorMessages.Count > 0)
            {
                return estimatedCosts;
            }
            return estimatedCosts;
        }


        internal bool DeleteLineItem(long EstimatedCostsId)
        {
            bool retValue = false;
            try
            {
                EstimatedCosts materialRequestLineItem = new EstimatedCosts()
                {
                    EstimatedCostsId = EstimatedCostsId

                };
                materialRequestLineItem.Delete(userData.DatabaseKey);
                if (materialRequestLineItem.ErrorMessages == null || materialRequestLineItem.ErrorMessages.Count == 0)
                {

                    retValue = true;
                }
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Edit Part In Inventory
        public PartInInventoryModel GetPartInInventoryItem(long EstimatedCostsId, long MaterialRequestId)
        {
            PartInInventoryModel objPartInInventoryModel = new PartInInventoryModel();
            EstimatedCosts estimatedCosts = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = MaterialRequestId,
                ObjectType = MaterialRequestConstant.MeterialRequest
            };
            List<EstimatedCosts> EstmatedCostsItems = EstimatedCosts.EstimatedCostsRetrieveByObjectId_ForChild(this.userData.DatabaseKey, estimatedCosts);
            var selectedEstmatedCostsItems = EstmatedCostsItems != null ? EstmatedCostsItems.Where(x => x.EstimatedCostsId == EstimatedCostsId).SingleOrDefault() : null;
            objPartInInventoryModel.EstimatedCostsId = selectedEstmatedCostsItems.EstimatedCostsId;
            objPartInInventoryModel.ClientId = selectedEstmatedCostsItems.ClientId;
            objPartInInventoryModel.Quantity = selectedEstmatedCostsItems.Quantity;
            objPartInInventoryModel.ObjectId = selectedEstmatedCostsItems.ObjectId;
            objPartInInventoryModel.Description = selectedEstmatedCostsItems.Description;
            objPartInInventoryModel.CategoryId = selectedEstmatedCostsItems.CategoryId;
            objPartInInventoryModel.PartClientLookupId = selectedEstmatedCostsItems.PartClientLookupId;

            if (userData.Site.ShoppingCart == true)
            {
                objPartInInventoryModel.UnitCost = selectedEstmatedCostsItems?.UnitCost ?? 00;
            }
            else
            {
                objPartInInventoryModel.UnitCostStockPart = selectedEstmatedCostsItems?.UnitCost ?? 00;
            }
            objPartInInventoryModel.Unit = selectedEstmatedCostsItems.Unit;
            objPartInInventoryModel.AccountId = selectedEstmatedCostsItems.AccountId;
            objPartInInventoryModel.AccountClientLookupId = selectedEstmatedCostsItems.AccountClientLookupId;
            objPartInInventoryModel.VendorId = selectedEstmatedCostsItems.VendorId;
            objPartInInventoryModel.VendorClientLookupId = selectedEstmatedCostsItems.VendorClientLookupId;
            objPartInInventoryModel.PartCategoryMasterId = selectedEstmatedCostsItems.UNSPSC;
            objPartInInventoryModel.PartCategoryClientLookupId = selectedEstmatedCostsItems.PartCategoryClientLookupId;
            return objPartInInventoryModel;
        }

        public EstimatedCosts EditPartInInventory(PartInInventoryModel partInInventoryModel)
        {

            EstimatedCosts estimatedCosts = new EstimatedCosts()
            {
                EstimatedCostsId = partInInventoryModel.EstimatedCostsId
            };
            estimatedCosts.Retrieve(_dbKey);
            // Update the properties of the EstimatedCosts object with the values from the partNotInInventoryModel
            estimatedCosts.Quantity = partInInventoryModel.Quantity ?? 0;
            #region 1148 Update additional properties based on the partInInventoryModel
            estimatedCosts.UnitOfMeasure = !string.IsNullOrEmpty(partInInventoryModel.Unit) ? partInInventoryModel.Unit : "";
            estimatedCosts.AccountId = partInInventoryModel?.AccountId ?? 0;
            estimatedCosts.AccountClientLookupId = partInInventoryModel.AccountClientLookupId;
            estimatedCosts.VendorId = partInInventoryModel?.VendorId ?? 0;
            estimatedCosts.VendorClientLookupId = partInInventoryModel.VendorClientLookupId;
            // Update the UNSPSC and UnitCost properties based on the ShoppingCart flag
            if (userData.Site.ShoppingCart == false)
            {
                estimatedCosts.UNSPSC = 0;
                estimatedCosts.PartClientLookupId = null;
                estimatedCosts.UnitCost = partInInventoryModel.UnitCostStockPart ?? 0;
            }
            else
            {
                estimatedCosts.UnitCost = partInInventoryModel.UnitCost ?? 0;
            }
            #endregion
            // Update the EstimatedCosts object in the database
            estimatedCosts.Update(userData.DatabaseKey);

            // Check for any error messages and return the EstimatedCosts object
            if (estimatedCosts.ErrorMessages != null && estimatedCosts.ErrorMessages.Count > 0)
            {
                return estimatedCosts;

            }
            return estimatedCosts;
        }

        #endregion

        #region V2-726

        public EstimatedCosts SendForApproval(ApprovalRouteModel arModel)
        {
            EstimatedCosts ec = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = arModel.ObjectId,
                ObjectType = MaterialRequestConstant.MeterialRequest
            };

            List<EstimatedCosts> estimatedCostsList = EstimatedCosts.EstimatedCostsRetrieveByObjectId_ForChild(this.userData.DatabaseKey, ec);
            estimatedCostsList = estimatedCostsList.Where(x => x.Status == MaterialRequestLineStatus.Initiated).ToList();
            if (estimatedCostsList != null && estimatedCostsList.Count > 0)
            {
                foreach (var item in estimatedCostsList)
                {
                    item.Retrieve(_dbKey);
                    item.Status = MaterialRequestLineStatus.Route;
                    item.Update(userData.DatabaseKey);
                    if (item.ErrorMessages != null && item.ErrorMessages.Count > 0)
                    {
                        ec.ErrorMessages = item.ErrorMessages;
                        break;
                    }

                    Task t1 = Task.Factory.StartNew(() => CreateEventLog(arModel.ApproverId, item.EstimatedCostsId, arModel.Comments, arModel.ApprovalGroupId, arModel.RequestType));
                }

                if (ec.ErrorMessages == null || ec.ErrorMessages.Count == 0)
                {
                    List<long> objectId = new List<long>() { arModel.ObjectId };
                    var UserList = new List<Tuple<long, string, string>>();
                    CommonWrapper coWrapper = new CommonWrapper(userData);
                    var PersonnelInfo = coWrapper.GetPersonnelDetailsByPersonnelId(arModel.ApproverId);
                    if (PersonnelInfo != null)
                    {
                        UserList.Add
                        (
                            Tuple.Create(Convert.ToInt64(PersonnelInfo.PersonnelId), PersonnelInfo.UserName, PersonnelInfo.Email)
                        );
                        ProcessAlert objAlert = new ProcessAlert(this.userData);
                        Task t2 = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.MaterialRequest>(AlertTypeEnum.MaterialRequestApprovalNeeded, objectId, UserList));
                    }

                    //Task t2 = Task.Factory.StartNew(() => CreateEventLog(arModel.ApproverId, arModel.ObjectId, arModel.Comments));

                }
            }
            //        wo.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);

            //wo.Status = WorkOrderStatusConstants.AwaitingApproval;
            //wo.UpdateByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);

            return ec;
        }

        private void CreateEventLog(Int64 ApproverId, Int64 ObjectId, string comment, long ApprovalGroupId, string RequestType)
        {
            ApprovalRoute log = new ApprovalRoute();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.ApproverId = ApproverId;
            log.ObjectId = ObjectId;
            log.Comments = comment;
            log.ApprovalGroupId = ApprovalGroupId;
            log.RequestType = RequestType;
            log.IsProcessed = false;
            log.ProcessResponse = String.Empty;
            log.Create(userData.DatabaseKey);
        }
        public bool RetrieveApprovalGroupMaterialRequestStaus()
        {
            bool IsMaterialRequest = false;
            ApprovalGroupSettings approvalGroupSettings = new ApprovalGroupSettings
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId
            };
            var approvalGroupSettingsList = approvalGroupSettings.RetrieveApprovalGroupSettings_V2(userData.DatabaseKey).FirstOrDefault();
            //V2-726 End
            if (approvalGroupSettingsList != null)
            {
                if (approvalGroupSettingsList.MaterialRequests == true)
                {
                    IsMaterialRequest = true;
                }
            }
            return IsMaterialRequest;
        }
        #endregion
    }
}