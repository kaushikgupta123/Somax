

using Client.Common;
using Client.Models.Configuration.ApprovalGroups;

using Common.Constants;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Client.BusinessWrapper.Configuration.ApprovalGroups
{
    public class ApprovalGroupsWrapper
    {
        private DatabaseKey _dbKey;
        private UserData _userData;

        public ApprovalGroupsWrapper(UserData userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search
        public List<ApprovalGroupsModel> getApprovalGroupsearchData(string orderbycol = "", int length = 0, string orderDir = "", int skip = 0, string RequestType = "", string Description = "", string ApprovalGroupId = "", long AssetGroup1Id = 0, long AssetGroup2Id = 0, long AssetGroup3Id = 0, string SearchText = "")
        {
            List<ApprovalGroupsModel> approvalGroupsModelList = new List<ApprovalGroupsModel>();
            ApprovalGroupsModel approvalGroupsModel;
            ApprovalGroup approvalGroup = new ApprovalGroup();
            approvalGroup.ClientId = _userData.DatabaseKey.Client.ClientId;
            approvalGroup.SiteId = _userData.Site.SiteId;
            approvalGroup.OrderbyColumn = orderbycol;
            approvalGroup.OrderBy = orderDir;
            approvalGroup.OffSetVal = skip;
            approvalGroup.NextRow = length;
            approvalGroup.Description = Description;
            approvalGroup.RequestType = RequestType;
            approvalGroup.AssetGroup1Id = AssetGroup1Id;
            approvalGroup.AssetGroup2Id = AssetGroup2Id;
            approvalGroup.AssetGroup3Id = AssetGroup3Id;
            approvalGroup.ApprovalGroupId = String.IsNullOrEmpty(ApprovalGroupId) ? 0 : Convert.ToInt32(ApprovalGroupId);
            approvalGroup.SearchText = SearchText;

            var approvalGroupRecordList = approvalGroup.ApprovalGroupRetrieveChunkSearchV2(_userData.DatabaseKey, _userData.Site.TimeZone);

            foreach (var item in approvalGroupRecordList.listOfApprovalGroup)
            {
                approvalGroupsModel = new ApprovalGroupsModel();
                approvalGroupsModel.ApprovalGroupId = item.ApprovalGroupId;

                if (item.RequestType == ApprovalGroupRequestTypes.MaterialRequest)
                {
                    approvalGroupsModel.RequestType = UtilityFunction.GetMessageFromResource("spnMaterialRequest",
                        LocalizeResourceSetConstants.Global);
                }
                else if (item.RequestType == ApprovalGroupRequestTypes.PurchaseRequest)
                {
                    approvalGroupsModel.RequestType = UtilityFunction.GetMessageFromResource("spnPurchaseRequest",
                        LocalizeResourceSetConstants.Global);
                }
                else if (item.RequestType == ApprovalGroupRequestTypes.WorkRequest)
                {
                    approvalGroupsModel.RequestType = UtilityFunction.GetMessageFromResource("spnWorkRequest",
                        LocalizeResourceSetConstants.Global);
                }
                else if (item.RequestType == ApprovalGroupRequestTypes.SanitationRequest)
                {
                    approvalGroupsModel.RequestType = UtilityFunction.GetMessageFromResource("spnSanitationRequest", 
                        LocalizeResourceSetConstants.SanitationDetails);
                }
                approvalGroupsModel.Description = item.Description;
                approvalGroupsModel.TotalCount = item.TotalCount;
                approvalGroupsModel.ChildCount = item.ChildCount;
                approvalGroupsModel.AssetGroup1ClientLookupId = item.AssetGroup1ClientLookupId;
                approvalGroupsModel.AssetGroup2ClientLookupId = item.AssetGroup2ClientLookupId;
                approvalGroupsModel.AssetGroup3ClientLookupId = item.AssetGroup3ClientLookupId;
                approvalGroupsModel.AssetGroup1Id = item.AssetGroup1Id;
                approvalGroupsModel.AssetGroup2Id = item.AssetGroup2Id;
                approvalGroupsModel.AssetGroup3Id = item.AssetGroup3Id;
                approvalGroupsModelList.Add(approvalGroupsModel);
            }

            return approvalGroupsModelList;
        }

        #endregion
        #region Line Item
        internal List<LineItemModel> PopulateLineitems(long ApprovalGroupId)
        {
            LineItemModel objLineItem;
            List<LineItemModel> LineItemList = new List<LineItemModel>();

            AppGroupApprovers appGroupApprovers = new AppGroupApprovers()
            {
                ClientId = this._userData.DatabaseKey.Client.ClientId,
                ApprovalGroupId = ApprovalGroupId,
                OffSetVal = 0,
                NextRow = 100000
            };
            var appGroupApproverslist = appGroupApprovers.AppGroupRetrieveByApprovalGroupIdV2(this._userData.DatabaseKey, _userData.Site.TimeZone);

            if (appGroupApproverslist != null)
            {
                foreach (var item in appGroupApproverslist.listOfAppGroupApprovers)
                {
                    objLineItem = new LineItemModel();
                    objLineItem.AppGroupApproversId = item.AppGroupApproversId;
                    objLineItem.Level = item.Level;
                    objLineItem.Approver = item.ApproverName;
                    objLineItem.ApproverId = item.ApproverId;
                    LineItemList.Add(objLineItem);
                }
            }

            return LineItemList;
        }
        #endregion

        #region Details
        public ApprovalGroupsModel ApprovalGroupsDetails(long ApprovalGroupId)
        {
            ApprovalGroupsModel approvalGroupsModel = new ApprovalGroupsModel();
            ApprovalGroup approvalGroup = new ApprovalGroup()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                ApprovalGroupId = ApprovalGroupId,
                SiteId = _userData.DatabaseKey.User.DefaultSiteId
        };
            approvalGroup.RetrieveById_V2(_dbKey);
            if (approvalGroup != null)
            {
                approvalGroupsModel.ApprovalGroupId = approvalGroup.ApprovalGroupId;
                approvalGroupsModel.Description = approvalGroup.Description;
                approvalGroupsModel.RequestType = approvalGroup.RequestType;
                approvalGroupsModel.AssetGroup1Id = approvalGroup.AssetGroup1;
                approvalGroupsModel.AssetGroup2Id = approvalGroup.AssetGroup2;
                approvalGroupsModel.AssetGroup3Id = approvalGroup.AssetGroup3;
                approvalGroupsModel.AssetGroup1ClientLookupId = approvalGroup.AssetGroup1ClientLookupId;
                approvalGroupsModel.AssetGroup2ClientLookupId = approvalGroup.AssetGroup2ClientLookupId;
                approvalGroupsModel.AssetGroup3ClientLookupId = approvalGroup.AssetGroup3ClientLookupId;
            }
            return approvalGroupsModel;
        }



        public List<AppGroupApproverModel> AppGroupApproverDetailsChunkSearch(string orderbycol = "", int length = 0, string orderDir = "", int skip = 0, long ApprovalGroupId = 0)
        {
            List<AppGroupApproverModel> appGroupApproverModelList = new List<AppGroupApproverModel>();

            AppGroupApproverModel appGroupApproverModel;
            AppGroupApprovers appGroupApprovers = new AppGroupApprovers();
            appGroupApprovers.ClientId = _userData.DatabaseKey.Client.ClientId;
            appGroupApprovers.OrderbyColumn = orderbycol;
            appGroupApprovers.OrderBy = orderDir;
            appGroupApprovers.OffSetVal = skip;
            appGroupApprovers.NextRow = length;
            appGroupApprovers.ApprovalGroupId = ApprovalGroupId;

            var appGroupApproversList = appGroupApprovers.AppGroupApproverDetailsChunkSearch(_userData.DatabaseKey, _userData.Site.TimeZone);

            foreach (var item in appGroupApproversList)
            {
                appGroupApproverModel = new AppGroupApproverModel();
                appGroupApproverModel.AppGroupApproversId = item.AppGroupApproversId;
                appGroupApproverModel.ApproverId = item.ApproverId;
                appGroupApproverModel.ApproverName = item.ApproverName;
                appGroupApproverModel.Limit = item.Limit;
                appGroupApproverModel.Level = item.Level;
                appGroupApproverModel.LevelName = item.LevelName;
                appGroupApproverModel.TotalCount = item.TotalCount;
                appGroupApproverModelList.Add(appGroupApproverModel);
            }
            return appGroupApproverModelList;
        }
        #endregion

        #region Add/Edit
        public ApprovalGroup AddOrEditApprovalGroupMaster(ApprovalGroupMasterModel approvalGroupMasterModel, ref string Mode, ref long approvalGroupId)
        {
            DataContracts.ApprovalGroup ap = new DataContracts.ApprovalGroup();
            if (approvalGroupMasterModel.ApprovalGroupId == 0)
            {
                Mode = "add";
                ap.ClientId = this._userData.DatabaseKey.Client.ClientId;
                ap.SiteId = this._userData.Site.SiteId;
                ap.Description = approvalGroupMasterModel.Description ?? string.Empty;
                ap.RequestType = approvalGroupMasterModel.RequestType ?? string.Empty;
                ap.AssetGroup1 = approvalGroupMasterModel.AssetGroup1 ?? 0;
                ap.AssetGroup2 = approvalGroupMasterModel.AssetGroup2 ?? 0;
                ap.AssetGroup3 = approvalGroupMasterModel.AssetGroup3 ?? 0;
                ap.Create(this._userData.DatabaseKey);

                approvalGroupId = ap.ApprovalGroupId;
                return ap;
            }
            else
            {
                Mode = "update";
                DataContracts.ApprovalGroup apEdit = new DataContracts.ApprovalGroup()
                {
                    ClientId = this._userData.DatabaseKey.Client.ClientId,
                    ApprovalGroupId = approvalGroupMasterModel.ApprovalGroupId,
                };
                apEdit.Retrieve(_userData.DatabaseKey);
                apEdit.Description = approvalGroupMasterModel.Description;
                apEdit.AssetGroup1 = approvalGroupMasterModel.AssetGroup1 ?? 0;
                apEdit.AssetGroup2 = approvalGroupMasterModel.AssetGroup2 ?? 0;
                apEdit.AssetGroup3 = approvalGroupMasterModel.AssetGroup3 ?? 0;
                apEdit.Update(_userData.DatabaseKey);

                approvalGroupId = apEdit.ApprovalGroupId;
                return apEdit;
            }
        }
        #endregion

        #region AppGroupApproval add edit and delete
        public AppGroupApproverModel RetrieveAppGroupApprovers(long AppGroupApproversId)
        {
            var appGroupApproverModel = new AppGroupApproverModel();
            AppGroupApprovers appGroupApprovers = new AppGroupApprovers()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                AppGroupApproversId = AppGroupApproversId,
            };

            appGroupApprovers.RetrieveById_V2(_userData.DatabaseKey);
            if (appGroupApprovers != null)
            {
                appGroupApproverModel.ApprovalGroupId = appGroupApprovers.ApprovalGroupId;
                appGroupApproverModel.AppGroupApproversId = appGroupApprovers.AppGroupApproversId;
                appGroupApproverModel.ApproverId = appGroupApprovers.ApproverId;
                appGroupApproverModel.ApproverName = appGroupApprovers.ApproverName ?? "";
                appGroupApproverModel.Level = appGroupApprovers.Level;
                appGroupApproverModel.LevelName = appGroupApprovers.LevelName;
                appGroupApproverModel.Limit = appGroupApprovers.Limit;
            }
            return appGroupApproverModel;
        }
        public AppGroupApprovers AddAppGroupApprovers(AppGroupApproverModel model)
        {
            AppGroupApprovers validation = new AppGroupApprovers()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                ApprovalGroupId = model.ApprovalGroupId,
                ApproverId = model.ApproverId
            };
            validation.CheckvalidateApproverId(this._userData.DatabaseKey);

            if (validation.ErrorObj == null || validation.ErrorMessages.Count == 0)
            {
                AppGroupApprovers appGroupApprovers = new AppGroupApprovers()
                {
                    ClientId = _userData.DatabaseKey.Client.ClientId,
                    AppGroupApproversId = model.AppGroupApproversId,
                    ApprovalGroupId = model.ApprovalGroupId,
                    ApproverId = model.ApproverId,
                    Limit = model.Limit ?? 0,
                    Level = model.Level,
                };
                appGroupApprovers.Create(this._userData.DatabaseKey);
                return appGroupApprovers;
            }
            return validation;
        }
        public AppGroupApprovers EditAppGroupApprovers(AppGroupApproverModel model)
        {
            AppGroupApprovers appGroupApprovers = new AppGroupApprovers()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                AppGroupApproversId = model.AppGroupApproversId,
            };

            appGroupApprovers.Retrieve(_userData.DatabaseKey);

            appGroupApprovers.Limit = model.Limit ?? 0;

            appGroupApprovers.Update(_userData.DatabaseKey);
            return appGroupApprovers;
        }
        public AppGroupApprovers DeleteAppGroupApprovers(long ApprovalGroupId, long AppGroupApproversId)
        {
            AppGroupApprovers validation = new AppGroupApprovers()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                AppGroupApproversId = AppGroupApproversId
            };

            validation.CheckValidateUpperLevelExists(this._userData.DatabaseKey);
            if (validation.ErrorMessages == null || validation.ErrorMessages.Count == 0)
            {
                AppGroupApprovers appGroupApprovers = new AppGroupApprovers()
                {
                    ClientId = _userData.DatabaseKey.Client.ClientId,
                    AppGroupApproversId = AppGroupApproversId,
                };

                appGroupApprovers.Delete(this._userData.DatabaseKey);
                return appGroupApprovers;
            }
            return validation;
        }
        #endregion

        #region V2-720 Requestors Grid
        public List<AppGroupRequestorsModel> AppGroupRequestorsChunkSearch(string orderbycol = "", int length = 0, string orderDir = "", int skip = 0, long ApprovalGroupId = 0)
        {
            List<AppGroupRequestorsModel> appGroupRequestorsModelList = new List<AppGroupRequestorsModel>();

            AppGroupRequestorsModel appGroupRequestorsModel;
            AppGroupRequestors appGroupRequestors = new AppGroupRequestors();
            appGroupRequestors.ClientId = _userData.DatabaseKey.Client.ClientId;
            appGroupRequestors.OrderByColumn = orderbycol;
            appGroupRequestors.OrderBy = orderDir;
            appGroupRequestors.Offset = skip;
            appGroupRequestors.Nextrow = length;
            appGroupRequestors.ApprovalGroupId = ApprovalGroupId;
            var appGroupApproversList = appGroupRequestors.RetrieveChunkSearchForDetailsById(_dbKey);
            foreach (var item in appGroupApproversList)
            {
                appGroupRequestorsModel = new AppGroupRequestorsModel();
                appGroupRequestorsModel.AppGroupRequestorsId = item.AppGroupRequestorsId;
                appGroupRequestorsModel.Requestor = item.RequestorName;
                appGroupRequestorsModel.TotalCount = item.TotalCount;
                appGroupRequestorsModelList.Add(appGroupRequestorsModel);
            }
            return appGroupRequestorsModelList;
        }

        public AppGroupRequestors DeleteRequestor(long AppGroupRequestorsId)
        {
            AppGroupRequestors appGroupRequestors = new AppGroupRequestors()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                AppGroupRequestorsId = AppGroupRequestorsId,
            };

            appGroupRequestors.Delete(this._userData.DatabaseKey);
            return appGroupRequestors;
        }
        #endregion

        #region Retrive for Approval Group

        public List<PersonnelApprovalModel> PopulatePersonnelLookupListForApprovalGroup(string orderbycol = "", string orderDir = "",
            int skip = 0, int length = 0, string clientLookupId = "", string Name = "", string AssetGroup1 = "", string AssetGroup2 = "",
            string AssetGroup3 = "", string requestType = "", long ApprovalGroupId = 0)
        {

            List<PersonnelApprovalModel> personnelAppList = new List<PersonnelApprovalModel>();
            Personnel personnel = new Personnel()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
            };
            personnel.SiteId = _userData.DatabaseKey.User.DefaultSiteId;
            personnel.OffSetVal = skip;
            personnel.NextRow = length;
            personnel.OrderbyColumn = orderbycol;
            personnel.OrderBy = orderDir;
            personnel.ClientLookupId = clientLookupId;
            personnel.FullName = Name;
            personnel.AssetGroup1 = AssetGroup1;
            personnel.AssetGroup2 = AssetGroup2;
            personnel.AssetGroup3 = AssetGroup3;
            personnel.requestType = requestType;
            personnel.ApprovalGroupId = ApprovalGroupId;
            var Personneldata = personnel.RetrievePersonnelLookupListForApprovalGroup(_userData.DatabaseKey);
            foreach (var item in Personneldata)
            {
                PersonnelApprovalModel obj = new PersonnelApprovalModel();
                obj.PersonnelId = item.PersonnelId;
                obj.ClientLookupId = item.ClientLookupId;
                obj.FullName = item.FullName;
                obj.AssetGroup1 = item.AssetGroup1;
                obj.AssetGroup2 = item.AssetGroup2;
                obj.AssetGroup3 = item.AssetGroup3;
                obj.AssetGroup1ClientlookUpId=item.AssetGroup1Names;
                obj.AssetGroup2ClientlookUpId=item.AssetGroup2Names;
                obj.AssetGroup3ClientlookUpId=item.AssetGroup3Names;
                obj.TotalCount = item.TotalCount;
                personnelAppList.Add(obj);
            }
            return personnelAppList;
        }

        #endregion

        #region Select requestors to add to the approval group
        public List<string> AddRequestorsToApproval(string[] SelectPersonalIDsItem, long approval_GroupId)
        {
            var errorList = new List<string>();
            if (SelectPersonalIDsItem.Length > 0)
            {
                for (int i = 0; i < SelectPersonalIDsItem.Length; i++)
                {
                    if (!string.IsNullOrEmpty(SelectPersonalIDsItem[i]))
                    {
                        AppGroupRequestors appGroupRequestors = new AppGroupRequestors()
                        {
                            ApprovalGroupId = approval_GroupId,
                            RequestorId = Convert.ToInt64(SelectPersonalIDsItem[i].ToString()),
                        };
                        appGroupRequestors.Create(this._dbKey);
                        if (appGroupRequestors.ErrorMessages != null && appGroupRequestors.ErrorMessages.Count > 0)
                        {
                            errorList = appGroupRequestors.ErrorMessages;
                            break;
                        }

                    }

                }
            }
            return errorList;


        }
        #endregion

    }
}