using Client.Common;
using Client.Models.PartsManagement.PartsManagementRequest;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AzureUtil;
using Common.Enumerations;
using System.IO;
using Presentation.Common;
using Client.BusinessWrapper.Common;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Client.BusinessWrapper.PartsManagement
{
    public class PartsManagementRequestWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        bool IsAzureHost = false;
        public PartsManagementRequestWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Search
        public List<PartsManagementRequestModel> GetPartsManagementRequests(long Searchvalue = 0)
        {
            PartsManagementRequestModel partsManagementRequestModel;
            List<PartsManagementRequestModel> partsManagementRequestModelList = new List<PartsManagementRequestModel>();

            DataContracts.PartMasterRequest PartMasterRequestSearch = new DataContracts.PartMasterRequest();
            PartMasterRequestSearch.ClientId = userData.DatabaseKey.Client.ClientId;
            PartMasterRequestSearch.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            PartMasterRequestSearch.CreatedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            PartMasterRequestSearch.CustomQueryDisplayId = Searchvalue;
            // V2-710 
            // RKL - 2022-May-25
            // Adding the dictionary makes this run MUCH faster
            // It is possible that when we get Reddis Cache implemented - this will not make as much difference
            // 
            Dictionary<string,string> pmreqtypes = new Dictionary<string, string>();
            List<DataContracts.PartMasterRequest> GridSource = PartMasterRequestSearch.Retrieve_PartMasterRequests_ByFilterCriteria(this.userData.DatabaseKey, userData.Site.TimeZone);
            string locreqtype;
            foreach (var item in GridSource)
            {
                partsManagementRequestModel = new PartsManagementRequestModel();
                partsManagementRequestModel.PartMasterRequestId = item.PartMasterRequestId;
                partsManagementRequestModel.Requester = item.Requester;
                partsManagementRequestModel.Justification = item.Justification;
                partsManagementRequestModel.RequestType = item.RequestType;
                partsManagementRequestModel.Status_Display = item.Status;
                partsManagementRequestModel.Manufacturer = item.Manufacturer;
                partsManagementRequestModel.ManufacturerId = item.ManufacturerId;
                if (pmreqtypes.TryGetValue(item.RequestType,out locreqtype))
                {
                  partsManagementRequestModel.LocalizedRequestType = locreqtype;
                }
                else
                {
                  locreqtype = UtilityFunction.GetMessageFromResource(item.RequestType, LocalizeResourceSetConstants.PartManagementRequest) ?? "";
                  pmreqtypes.Add(item.RequestType,locreqtype);
                }
                partsManagementRequestModel.LocalizedRequestType = locreqtype;
                //partsManagementRequestModel.LocalizedRequestType = UtilityFunction.GetMessageFromResource(item.RequestType, LocalizeResourceSetConstants.PartManagementRequest) ?? "";
                partsManagementRequestModelList.Add(partsManagementRequestModel);
            }
            return partsManagementRequestModelList;
        }        
        #endregion Search

        #region Details
        public string CheckRequestType(string type)
        {
            if (type.Trim() == "Addition") 
                type = PartMasterRequestTypeConstants.Addition;
            if (type.Trim() == "SX_Replace")
                type = PartMasterRequestTypeConstants.SX_Replacement;
            if (type.Trim() == "ECO_New")
                type = PartMasterRequestTypeConstants.ECO_New;
            if (type.Trim() == "ECO_Replacement") 
                type = PartMasterRequestTypeConstants.ECO_Replace;
            if (type.Trim() == "ECO_SX_Replace") 
                type = PartMasterRequestTypeConstants.ECO_SX_Replace;
            if (type.Trim() == "Replace")
                type = "";
            if (type.Trim() == "Inactivate")
                type = "";
            return type;
        }

        public PartsManagementRequestModel GetPMRequestDetails(long PartMasterRequestId)
        {
            DataContracts.PartMasterRequest partMasterRequest = new DataContracts.PartMasterRequest()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PartMasterRequestId = PartMasterRequestId 
            };
            partMasterRequest.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            PartsManagementRequestModel pmRequestModel = new PartsManagementRequestModel();

            pmRequestModel = ControlsVisibilty(partMasterRequest);
            pmRequestModel.PartMasterRequestId = partMasterRequest.PartMasterRequestId;
            pmRequestModel.Description = partMasterRequest.Description;
            pmRequestModel.Manufacturer = partMasterRequest.Manufacturer;
            pmRequestModel.ManufacturerId = partMasterRequest.ManufacturerId;
            pmRequestModel.UnitOfMeasure = partMasterRequest.UnitOfMeasure;
            pmRequestModel.Requester = partMasterRequest.Requester;
            if (partMasterRequest.CreatedDate != null && partMasterRequest.CreatedDate == default(DateTime))
            {
                pmRequestModel.CreatedDate = null;
            }
            else
            {
                pmRequestModel.CreatedDate = partMasterRequest.CreatedDate;
            }
            pmRequestModel.Justification = partMasterRequest.Justification;
            pmRequestModel.Critical = partMasterRequest.Critical; 
            pmRequestModel.PurchaseFreq = partMasterRequest.PurchaseFreq;
            pmRequestModel.PurchaseLeadTime = partMasterRequest.PurchaseLeadTime; 
            pmRequestModel.PurchaseCost = partMasterRequest.PurchaseCost;
            pmRequestModel.RequestType = partMasterRequest.RequestType;
            pmRequestModel.Status = partMasterRequest.Status;
            pmRequestModel.ClientId = partMasterRequest.ClientId;
            pmRequestModel.SiteId = partMasterRequest.SiteId;
            pmRequestModel.LastReviewedBy = partMasterRequest.LastReviewedBy;
            pmRequestModel.LastReviewed_Date = partMasterRequest.LastReviewed_Date;  
            pmRequestModel.ApproveDenyBy = partMasterRequest.ApproveDenyBy; 
            pmRequestModel.ApproveDeny_Date = partMasterRequest.ApproveDeny_Date; 
            pmRequestModel.ApproveDenyBy2 = partMasterRequest.ApproveDenyBy2; 
            pmRequestModel.ApproveDeny2_Date = partMasterRequest.ApproveDeny2_Date;  
            pmRequestModel.PartMasterId = partMasterRequest.PartMasterId;
            #region V2-798
            pmRequestModel.UnitCost = partMasterRequest.UnitCost;
            pmRequestModel.Location = partMasterRequest.Location;
            pmRequestModel.QtyMinimum = partMasterRequest.QtyMinimum;
            pmRequestModel.QtyMaximum = partMasterRequest.QtyMaximum;
            #endregion
            return pmRequestModel;
        }

        private PartsManagementRequestModel ControlsVisibilty(DataContracts.PartMasterRequest partMasterRequest)
        {
            PartsManagementRequestModel pmRequestModel = new PartsManagementRequestModel();
            if (partMasterRequest.Status == PartMasterRequestStatusConstants.Canceled
                || partMasterRequest.Status == PartMasterRequestStatusConstants.Complete
                || partMasterRequest.Status == PartMasterRequestStatusConstants.Extracted
                || partMasterRequest.Status == PartMasterRequestStatusConstants.Denied)
                pmRequestModel.btnSave = false;
            else if (partMasterRequest.CreatedBy_PersonnelId == userData.DatabaseKey.Personnel.PersonnelId
                 && (partMasterRequest.Status == PartMasterRequestStatusConstants.Open
                     || partMasterRequest.Status == PartMasterRequestStatusConstants.Returned))
                pmRequestModel.btnSave = true;
            else if (userData.Security.PartMasterRequest.Edit)
                pmRequestModel.btnSave = true;
            else
                pmRequestModel.btnSave = false;

            if (partMasterRequest.Status == PartMasterRequestStatusConstants.Denied
              || partMasterRequest.Status == PartMasterRequestStatusConstants.Complete
              || partMasterRequest.Status == PartMasterRequestStatusConstants.Approved
              || partMasterRequest.Status == PartMasterRequestStatusConstants.SiteApproved
              || partMasterRequest.Status == PartMasterRequestStatusConstants.Extracted
              || partMasterRequest.Status == PartMasterRequestStatusConstants.Canceled)
                pmRequestModel.btnSendToApproval = false;
            else if (partMasterRequest.CreatedBy_PersonnelId == userData.DatabaseKey.Personnel.PersonnelId
                 && (partMasterRequest.Status == PartMasterRequestStatusConstants.Open || partMasterRequest.Status == PartMasterRequestStatusConstants.Returned))
                pmRequestModel.btnSendToApproval = true;
            //else if (userData.Security.PartMasterRequest.Approve || userData.Security.PartMasterRequest.Review)
            //    pmRequestModel.btnSendToApproval = true;
            else
                pmRequestModel.btnSendToApproval = false;
            if (partMasterRequest.Status != PartMasterRequestStatusConstants.Review)
                pmRequestModel.btnReturn2Requester = false;
            else if (userData.Security.PartMasterRequest.Approve || userData.Security.PartMasterRequest.Review)
                pmRequestModel.btnReturn2Requester = true;
            else
                pmRequestModel.btnReturn2Requester = false;
            if (partMasterRequest.CreatedBy_PersonnelId == userData.DatabaseKey.Personnel.PersonnelId
                 && userData.Security.PartMasterRequest.Approve
                 && (partMasterRequest.Status == PartMasterRequestStatusConstants.Open
                  || partMasterRequest.Status == PartMasterRequestStatusConstants.Review
                  || partMasterRequest.Status == PartMasterRequestStatusConstants.Returned))
            {
                pmRequestModel.btnApprv = true;
            }
            else if (partMasterRequest.Status == PartMasterRequestStatusConstants.Review && userData.Security.PartMasterRequest.Approve)
            {
                pmRequestModel.btnApprv = true;
                pmRequestModel.btndenied = true;
            }
            else if (partMasterRequest.Status == PartMasterRequestStatusConstants.SiteApproved && userData.Security.PartMasterRequest.ApproveEnterprise)
            {
                pmRequestModel.btnApprv = false;
                pmRequestModel.btndenied = true;
            }
            else
            {
                pmRequestModel.btnApprv = false;
                pmRequestModel.btndenied = false;
            }

            if (partMasterRequest.CreatedBy_PersonnelId == userData.DatabaseKey.Personnel.PersonnelId
               && (partMasterRequest.Status == PartMasterRequestStatusConstants.Open || partMasterRequest.Status == PartMasterRequestStatusConstants.Returned))
            {
                pmRequestModel.btnCancel = true;
            }
            else
            {
                pmRequestModel.btnCancel = false;
            }
            if (this.userData.Security.PartMasterRequest.ApproveEnterprise && partMasterRequest.Status == PartMasterRequestStatusConstants.SiteApproved)
            {
                AlertSetup setup = new AlertSetup()
                {
                    ClientId = partMasterRequest.ClientId,
                    SiteId = partMasterRequest.SiteId,
                    Alert_Name = AlertTypeEnum.PartMasterRequestApprovalNeeded.ToString()
                };
                setup.RetrieveForNotification(userData.DatabaseKey);
                AlertTarget alerttarget = new AlertTarget()
                {
                    ClientId = setup.ClientId,
                    AlertSetupId = setup.AlertSetupId
                };
                List<AlertTarget> targets = alerttarget.RetreiveTargetList(userData.DatabaseKey);
                if (targets.Count > 0 && targets.FirstOrDefault(i => i.UserInfoId == userData.DatabaseKey.Personnel.PersonnelId) != null)
                {
                    pmRequestModel.btnEnterpriseApprv = true;
                }
                // RKL - Added Isimar to this list
                // For Feb Update - Just allow anyone with the security to do enterprise approval
                else if (userData.DatabaseKey.User.UserInfoId == 2     
                  || userData.DatabaseKey.User.UserInfoId == 1512
                  || userData.DatabaseKey.User.UserInfoId == 1568 
                  || userData.DatabaseKey.User.UserInfoId == 6130
                  || userData.DatabaseKey.User.UserInfoId == 15580
                  ) 
                {
                    pmRequestModel.btnEnterpriseApprv = true;
                }
                else
                {
                    pmRequestModel.btnEnterpriseApprv = false;
                }
            }
            else
            {
                pmRequestModel.btnEnterpriseApprv = false;
            }
         
            if (partMasterRequest.Status == PartMasterRequestStatusConstants.Denied
               || partMasterRequest.Status == PartMasterRequestStatusConstants.Complete)
                pmRequestModel.DocumentsUploadControl = false;
            else
                pmRequestModel.DocumentsUploadControl = true;
            //V2-1216
            // Check if the PartMasterRequest status is "SiteApproved" and the request type is one of the specified ECO types.
            // If the conditions are met, enable the "Return to Requester" button in the UI.
            if ((partMasterRequest.Status == PartMasterRequestStatusConstants.SiteApproved) &&
                (partMasterRequest.RequestType == PartMasterRequestTypeConstants.ECO_New ||
                 partMasterRequest.RequestType == PartMasterRequestTypeConstants.ECO_Replace ||
                 partMasterRequest.RequestType == PartMasterRequestTypeConstants.ECO_SX_Replace))
            {
                pmRequestModel.btnReturn2Requester = true;
            }
            return pmRequestModel;
        }

        public PartsManagementRequestModel GetPMRequestDetailSite(long PartMasterRequestId)
        {
            DataContracts.PartMasterRequest partMasterRequest = new DataContracts.PartMasterRequest()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.Personnel.SiteId,
                PartMasterRequestId = PartMasterRequestId//ObjectId,
            };
            partMasterRequest.RetrieveByPKForPMLocalAdd(userData.DatabaseKey, userData.Site.TimeZone);

            PartsManagementRequestModel pmRequestModel = new PartsManagementRequestModel();
            pmRequestModel = ControlsVisibiltySite(partMasterRequest);
            pmRequestModel.PartMasterRequestId = partMasterRequest.PartMasterRequestId;
            pmRequestModel.Justification = partMasterRequest.Justification;
            pmRequestModel.RequestType = partMasterRequest.RequestType;
            pmRequestModel.PartMaster_ClientLookupId = partMasterRequest.PartMaster_ClientLookupId;
            pmRequestModel.PartMaster_LongDescription = partMasterRequest.PartMaster_LongDescription;
            pmRequestModel.ClientId = partMasterRequest.ClientId;
            pmRequestModel.SiteId = partMasterRequest.SiteId;
            pmRequestModel.Status = partMasterRequest.Status;
            pmRequestModel.Site_Name = partMasterRequest.Site_Name;
            pmRequestModel.Requester = partMasterRequest.Requester; 
            if (partMasterRequest.CreatedDate != null && partMasterRequest.CreatedDate == default(DateTime))
            {
                pmRequestModel.CreatedDate = null;
            }
            else
            {
                pmRequestModel.CreatedDate = partMasterRequest.CreatedDate;
            }
            pmRequestModel.LastReviewedBy = partMasterRequest.LastReviewedBy;
            pmRequestModel.LastReviewed_Date = partMasterRequest.LastReviewed_Date;
            pmRequestModel.ApproveDenyBy = partMasterRequest.ApproveDenyBy;
            pmRequestModel.ApproveDeny_Date = partMasterRequest.ApproveDeny_Date;
            pmRequestModel.CompleteBy = partMasterRequest.PartMasterCreateBy;
            pmRequestModel.CompleteDate = partMasterRequest.CompleteDate;
            #region V2-798
            pmRequestModel.UnitCost = partMasterRequest.UnitCost;
            pmRequestModel.Location = partMasterRequest.Location;
            pmRequestModel.QtyMinimum = partMasterRequest.QtyMinimum;
            pmRequestModel.QtyMaximum = partMasterRequest.QtyMaximum;
            #endregion
            #region V2-874
            pmRequestModel.Part_ClientLookupId = partMasterRequest.Part_ClientLookupId;
            pmRequestModel.Part_Description = partMasterRequest.Part_Description;
            #endregion
            return pmRequestModel;
        }

        protected PartsManagementRequestModel ControlsVisibiltySite(DataContracts.PartMasterRequest partMasterRequest)
        {
            PartsManagementRequestModel pmRequestModel = new PartsManagementRequestModel();           
            if (partMasterRequest.Status == PartMasterRequestStatusConstants.Canceled
                || partMasterRequest.Status == PartMasterRequestStatusConstants.Complete
                || partMasterRequest.Status == PartMasterRequestStatusConstants.Extracted
                || partMasterRequest.Status == PartMasterRequestStatusConstants.Denied)
                pmRequestModel.btnSave = false;           
            else if (partMasterRequest.CreatedBy_PersonnelId == userData.DatabaseKey.Personnel.PersonnelId
                 && (partMasterRequest.Status == PartMasterRequestStatusConstants.Open
                     || partMasterRequest.Status == PartMasterRequestStatusConstants.Returned))
                pmRequestModel.btnSave = true;            
            else if (userData.Security.PartMasterRequest.Edit && partMasterRequest.Status == PartMasterRequestStatusConstants.Review)
                pmRequestModel.btnSave = true;
            else               
                pmRequestModel.btnSave = false;
            if (partMasterRequest.Status == PartMasterRequestStatusConstants.Denied
              || partMasterRequest.Status == PartMasterRequestStatusConstants.Complete
              || partMasterRequest.Status == PartMasterRequestStatusConstants.Approved
              || partMasterRequest.Status == PartMasterRequestStatusConstants.Extracted
              || partMasterRequest.Status == PartMasterRequestStatusConstants.SiteApproved
              || partMasterRequest.Status == PartMasterRequestStatusConstants.Canceled)
                pmRequestModel.btnSendToApproval = false;          
            else if (partMasterRequest.CreatedBy_PersonnelId == userData.DatabaseKey.Personnel.PersonnelId
                 && (partMasterRequest.Status == PartMasterRequestStatusConstants.Open || partMasterRequest.Status == PartMasterRequestStatusConstants.Returned))
                pmRequestModel.btnSendToApproval = true;            
            //else if (userData.Security.PartMasterRequest.Approve || userData.Security.PartMasterRequest.Review)
            //    pmRequestModel.btnSendToApproval = true;
            else              
                pmRequestModel.btnSendToApproval = false;
                     
            if (partMasterRequest.Status != PartMasterRequestStatusConstants.Review)
                pmRequestModel.btnReturn2Requester = false;           
            else if (userData.Security.PartMasterRequest.Approve || userData.Security.PartMasterRequest.Review)
                pmRequestModel.btnReturn2Requester = true;
            else
                pmRequestModel.btnReturn2Requester = false;
         
            if (partMasterRequest.CreatedBy_PersonnelId == userData.DatabaseKey.Personnel.PersonnelId
                 && userData.Security.PartMasterRequest.Approve
                 && (partMasterRequest.Status == PartMasterRequestStatusConstants.Open
                  || partMasterRequest.Status == PartMasterRequestStatusConstants.Review
                  || partMasterRequest.Status == PartMasterRequestStatusConstants.Returned))
            {
                pmRequestModel.btnApprv = true;
                pmRequestModel.btndenied = true;  
            }           
            else if (partMasterRequest.Status == PartMasterRequestStatusConstants.Review && userData.Security.PartMasterRequest.Approve)
            {
                pmRequestModel.btnApprv = true;
                pmRequestModel.btndenied = true;
            }            
            else if (partMasterRequest.Status == PartMasterRequestStatusConstants.SiteApproved && userData.Security.PartMasterRequest.ApproveEnterprise)
            {
                pmRequestModel.btnApprv = false;
                pmRequestModel.btndenied = true;
            }            
            else
            {
                pmRequestModel.btnApprv = false;
                pmRequestModel.btndenied = false;
            }          
            if (partMasterRequest.CreatedBy_PersonnelId == userData.DatabaseKey.Personnel.PersonnelId
               && (partMasterRequest.Status == PartMasterRequestStatusConstants.Open || partMasterRequest.Status == PartMasterRequestStatusConstants.Returned))
            {
                pmRequestModel.btnCancel = true;
            }
            else
            {
                pmRequestModel.btnCancel = false;
            }
            return pmRequestModel;
        }

        #endregion Details

        #region AssignPart
        internal List<string> AssignPartMastertoIndusnetBakery(AssignPartMastertoIndusnetBakeryModel obj)
        {
            PartMasterRequest pmr = new PartMasterRequest();
            pmr.ClientId = userData.DatabaseKey.Client.ClientId;
            pmr.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            pmr.PartMaster_ClientLookupId = obj.PartMaster_ClientLookupId;
            pmr.Justification = obj.Justification;
            pmr.Status = PartMasterRequestStatusConstants.Open;
            pmr.RequestType = PartMasterRequestTypeConstants.Addition;
            pmr.CreatedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            #region V2-798
            pmr.UnitCost = obj.UnitCost ?? 0;
            pmr.Location = obj.Location;
            pmr.QtyMinimum = obj.QtyMinimum ?? 0;
            pmr.QtyMaximum = obj.QtyMaximum ?? 0;
            #endregion
            pmr.CreateByFKAssign(userData.DatabaseKey);
            if (pmr.ErrorMessages == null || pmr.ErrorMessages.Count == 0)
            {
                ReviewLog rlog = new ReviewLog()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    TableName = "PartMasterRequest",
                    ObjectId = pmr.PartMasterRequestId,
                    Function = ReviewLogConstants.Created,
                    PersonnelId = userData.DatabaseKey.Personnel.PersonnelId,
                    Comments = "Added",
                    ReviewDate = DateTime.UtcNow
                };
                rlog.Create(userData.DatabaseKey);
            }
            return pmr.ErrorMessages;
        }
        #endregion

        #region ReplacePart
        public List<string> ReplacePart(ReplacePartModal replacePartModal)
        {
            PartMasterRequest pmr = new PartMasterRequest();
            pmr.ClientId = userData.DatabaseKey.Client.ClientId;
            pmr.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            pmr.PartMaster_ClientLookupId = replacePartModal.ReplaceWith;
            pmr.Part_ClientLookupId = replacePartModal.PartToReplace;
            pmr.Justification = replacePartModal.Justification;
            pmr.Status = PartMasterRequestStatusConstants.Open;
            pmr.RequestType = PartMasterRequestTypeConstants.Replacement;
            pmr.CreatedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            pmr.CreateByFKReplace(userData.DatabaseKey);
            if (pmr.ErrorMessages == null || pmr.ErrorMessages.Count == 0)
            {

                ReviewLog rlog = new ReviewLog()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    TableName = "PartMasterRequest",
                    ObjectId = pmr.PartMasterRequestId,
                    Function = ReviewLogConstants.Created,
                    PersonnelId = userData.DatabaseKey.Personnel.PersonnelId,
                    Comments = "Added",
                    ReviewDate = DateTime.UtcNow
                };
                rlog.Create(userData.DatabaseKey);
            }
            return pmr.ErrorMessages;

        }
        #endregion

        #region InactivePart
        public List<string> InactivePart(InactivePartModel inactivePartModel)
        {
            PartMasterRequest pmr = new PartMasterRequest();
            pmr.ClientId = userData.DatabaseKey.Client.ClientId;
            pmr.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            pmr.Part_ClientLookupId = inactivePartModel.PartToInactivate;
            pmr.Justification = inactivePartModel.Justification;
            pmr.Status = PartMasterRequestStatusConstants.Open;
            pmr.RequestType = PartMasterRequestTypeConstants.Inactivation;
            pmr.CreatedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            pmr.CreateByFKInactivate(userData.DatabaseKey);
            if (pmr.ErrorMessages == null || pmr.ErrorMessages.Count == 0)
            {
                ReviewLog rlog = new ReviewLog()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    TableName = "PartMasterRequest",
                    ObjectId = pmr.PartMasterRequestId,
                    Function = ReviewLogConstants.Created,
                    PersonnelId = userData.DatabaseKey.Personnel.PersonnelId,
                    Comments = "Added",
                    ReviewDate = DateTime.UtcNow
                };
                rlog.Create(userData.DatabaseKey);
            }
            return pmr.ErrorMessages;
        }
        #endregion

        #region ReplaceSXPart
        public List<string> ReplaceSXPart(ReplaceSXPartModel replaceSXPartModel)
        {
            PartMasterRequest pmr = new PartMasterRequest();
            pmr.ClientId = userData.DatabaseKey.Client.ClientId;
            pmr.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            pmr.PartMaster_ClientLookupId = replaceSXPartModel.ReplaceWith;
            pmr.Part_ClientLookupId = replaceSXPartModel.SXPartToReplace;
            pmr.Justification = replaceSXPartModel.Justification;
            pmr.Status = PartMasterRequestStatusConstants.Open;
            pmr.RequestType = PartMasterRequestTypeConstants.SX_Replacement;
            pmr.CreatedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            pmr.CreateByFKReplace(userData.DatabaseKey);
            if (pmr.ErrorMessages == null || pmr.ErrorMessages.Count == 0)
            {

                ReviewLog rlog = new ReviewLog()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    TableName = "PartMasterRequest",
                    ObjectId = pmr.PartMasterRequestId,
                    Function = ReviewLogConstants.Created,
                    PersonnelId = userData.DatabaseKey.Personnel.PersonnelId,
                    Comments = "Added",
                    ReviewDate = DateTime.UtcNow
                };
                rlog.Create(userData.DatabaseKey);
            }
            return pmr.ErrorMessages;
        }
        #endregion


        #region Attachment
        public List<PartsManagementAttachmentModel> PopulatePMRAttachment(long PartMasterRequestId, string RequestType)
        {
            DataContracts.Attachment attach = new DataContracts.Attachment();
            var dataSource = attach.RetrieveAllAttachment(this.userData.DatabaseKey, userData.Site.TimeZone);
            PartsManagementAttachmentModel objPartsManagementAttachmentModel;
            List<PartsManagementAttachmentModel> PartsManagementAttachmentModelList = new List<PartsManagementAttachmentModel>();
            foreach (var v in dataSource)
            {
                objPartsManagementAttachmentModel = new PartsManagementAttachmentModel();
                objPartsManagementAttachmentModel.AttachmentId = v.AttachmentId;
                objPartsManagementAttachmentModel.FileName = v.FileName;
                objPartsManagementAttachmentModel.Description = v.Description;
                objPartsManagementAttachmentModel.UploadedByPersonnelId = v.UploadedBy_PersonnelId;
                objPartsManagementAttachmentModel.CreateDate = v.CreateDate; 
                objPartsManagementAttachmentModel.FullName = v.FullName;
                objPartsManagementAttachmentModel.UploadedBy = v.UploadedBy;                                                                           
                objPartsManagementAttachmentModel.FileSize = v.FileSize;
                objPartsManagementAttachmentModel.UpdateIndex = v.UpdateIndex;
                objPartsManagementAttachmentModel.FileType = v.FileType;
                objPartsManagementAttachmentModel.AttachmentURL = v.AttachmentURL;
                objPartsManagementAttachmentModel.IsEditable = v.IsEditable;
                objPartsManagementAttachmentModel.ModifiedBy = v.ModifiedBy;
                objPartsManagementAttachmentModel.ObjectId = v.ObjectId;
                objPartsManagementAttachmentModel.ObjectName = v.ObjectName;
                objPartsManagementAttachmentModel.RequestType = RequestType;
                objPartsManagementAttachmentModel.PartMasterRequestId = PartMasterRequestId;
                PartsManagementAttachmentModelList.Add(objPartsManagementAttachmentModel);
            }
            return PartsManagementAttachmentModelList;
        }
        public AzureBlob DownloadPMRAttachment(long PMPartMasterRequestId, int PMAttachmentId)
        {
            int AttachmentId = Convert.ToInt32(PMAttachmentId);
            AzureBlob azb = new AzureBlob();
            AzureSetup aset = new AzureSetup();        
            Attachment attach = new Attachment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                AttachmentId = PMAttachmentId
            };
            attach.Retrieve(userData.DatabaseKey);
            AzureSetup asetup = new AzureSetup();
            Uri attach_url = new Uri(attach.AttachmentURL);
            CloudBlockBlob cb = asetup.GetCloudBlockBlobURI(attach_url);
            if (cb.Exists())
            {
                SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
                {
                    Permissions = SharedAccessBlobPermissions.Read,
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1),
                };
                SharedAccessBlobHeaders headers = new SharedAccessBlobHeaders()
                {
                    ContentDisposition = string.Format("attachment;filename=\"{0}\"", attach.FileName),
                    ContentType = attach.ContentType
                };
                var sasToken = cb.GetSharedAccessSignature(policy, headers);
                attach.AttachmentURL = cb.Uri.AbsoluteUri + sasToken;
            }

            return azb;
        }

        public bool DeleteAttachment(long _AttachmentId)
        {
            try
            {
                IsAzureHost = System.Configuration.ConfigurationManager.AppSettings["ImageStorageHost"].ToString().ToLower() == "azure";

                Attachment attachment = new Attachment()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    AttachmentId = Convert.ToInt64(_AttachmentId)

                };
                attachment.Retrieve(this.userData.DatabaseKey);
                // DeleteAttachment();
                if (IsAzureHost)
                {
                    //======Image Delete From Azure==================
                    AzureSetup aset = new AzureSetup();
                    Int64 Clientid = userData.DatabaseKey.Client.ClientId;
                    Int64 Siteid = userData.DatabaseKey.User.DefaultSiteId;
                    Int64 PartMasterRequestId = Convert.ToInt64(_AttachmentId);
                    string FileName = attachment.FileName;
                    string filePath = aset.CreateFileNamebyObject("PartMasterRequest", PartMasterRequestId.ToString(), FileName);
                    aset.DeleteAttachmentFromAzure(Clientid, Siteid, filePath, false, FileName);
                }
                attachment.Delete(userData.DatabaseKey);
                return true;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<string> EditAttachment(PartsManagementAttachmentModel objPartsManagementAttachmentModel)
        {
            Attachment attachment = new Attachment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                AttachmentId = Convert.ToInt64(objPartsManagementAttachmentModel.AttachmentId)
            };
            attachment.Retrieve(this.userData.DatabaseKey);
            attachment.Description = String.IsNullOrEmpty(objPartsManagementAttachmentModel.Description) ? "" : objPartsManagementAttachmentModel.Description.Trim();
            attachment.Update(this.userData.DatabaseKey);
            return attachment.ErrorMessages;

        }
        #endregion Attachment

        #region Review Log
        public List<PartManagementReviewLog> PopulatePMRReviewLog(long PartMasterRequestId)
        {
            DataContracts.ReviewLog RLog = new DataContracts.ReviewLog()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                TableName = "PartMasterRequest",
                ObjectId = PartMasterRequestId 
            };
            var GridSource = RLog.Retrieve_LogDetailsByPMRId(this.userData.DatabaseKey, userData.Site.TimeZone);
            PartManagementReviewLog objPartManagementReviewLog;
            List<PartManagementReviewLog> PartManagementReviewLogList = new List<PartManagementReviewLog>();
            foreach (var v in GridSource)
            {
                objPartManagementReviewLog = new PartManagementReviewLog();
                objPartManagementReviewLog.Reviewed_By = v.Reviewed_By;
                objPartManagementReviewLog.Comments = v.Comments;
                objPartManagementReviewLog.ReviewDate = v.ReviewDate;
                PartManagementReviewLogList.Add(objPartManagementReviewLog);
            }
            return PartManagementReviewLogList;
        }
        #endregion Review Log

        #region AddPart
        public List<string> AddPartMr(PartsManagementRequestModel pmrModel)
        {
            DataContracts.PartMasterRequest partMasterRequest = new DataContracts.PartMasterRequest
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            partMasterRequest.Description = pmrModel.Description;
            partMasterRequest.Justification = pmrModel.Justification;
            partMasterRequest.Manufacturer = pmrModel.Manufacturer;
            partMasterRequest.UnitOfMeasure = pmrModel.UnitOfMeasure;
            partMasterRequest.ManufacturerId = pmrModel.ManufacturerId.ToUpper();
            partMasterRequest.PurchaseFreq = pmrModel.PurchaseFreq;
            partMasterRequest.Critical = pmrModel.Critical;
            partMasterRequest.CreatedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            partMasterRequest.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            partMasterRequest.Status = PartMasterRequestStatusConstants.Open;
            partMasterRequest.PurchaseLeadTime = pmrModel.PurchaseLeadTime;
            partMasterRequest.PurchaseCost = pmrModel.PurchaseCost;

            if (pmrModel.RequestType == PartMasterRequestTypeConstants.ECO_Replace || pmrModel.RequestType == PartMasterRequestTypeConstants.ECO_SX_Replace)
            {
                partMasterRequest.RequestType = pmrModel.RequestType;
                partMasterRequest.Part_ClientLookupId = pmrModel.Part_ClientLookupId;
            }
            else
            {
                partMasterRequest.RequestType = PartMasterRequestTypeConstants.ECO_New;
                partMasterRequest.Part_ClientLookupId = "";
            }
            partMasterRequest.PurchaseCost = pmrModel.PurchaseCost;
            partMasterRequest.PurchaseLeadTime = pmrModel.PurchaseLeadTime;
            #region V2-798
            partMasterRequest.UnitCost = pmrModel.UnitCost ?? 0;
            partMasterRequest.Location = pmrModel.Location;
            partMasterRequest.QtyMinimum = pmrModel.QtyMinimum ?? 0;
            partMasterRequest.QtyMaximum = pmrModel.QtyMaximum ?? 0;
            #endregion
            partMasterRequest.AddNewPartNewRequestor(this.userData.DatabaseKey);
            if (partMasterRequest.ErrorMessages == null || partMasterRequest.ErrorMessages.Count < 1)
            {
                ReviewLog rlog = new ReviewLog()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    TableName = "PartMasterRequest",
                    ObjectId = partMasterRequest.PartMasterRequestId,
                    Function = ReviewLogConstants.Created,
                    PersonnelId = userData.DatabaseKey.Personnel.PersonnelId,
                    Comments = "Added",
                    ReviewDate = DateTime.UtcNow
                };
                rlog.Create(userData.DatabaseKey);
            }

            return partMasterRequest.ErrorMessages;
        }
        #endregion AddPart

        public List<DataContracts.LookupList> GetUnitofMeasureList()
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            return AllLookUps;
        }
        #region Photos
        //public void DeleteImage(long PartMasterRequestId, string TableName, bool Profile, bool Image, ref string rtrMsg)
        //{ 

        //    PartMasterRequest pmr = new PartMasterRequest()
        //    {
        //        ClientId = userData.DatabaseKey.Client.ClientId,
        //        PartMasterRequestId = PartMasterRequestId
        //    };

        //    pmr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
        //    //======Image Delete From Azure==================
        //    AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
        //    Int64 Clientid = pmr.ClientId;
        //    Int64 Siteid = pmr.SiteId;
        //    string imgName = "";
        //    string fileName = aset.CreateFileNamebyObject("PartMasterRequestImage", pmr.PartMasterRequestId.ToString(), imgName);
        //    aset.DeleteAzureBlob(Clientid, Siteid, fileName, true);
        //    pmr.ImageURL = "";
        //    pmr.Update(this.userData.DatabaseKey);
        //    rtrMsg = "Success";
        //}

        //public string GetPartManagementRequestImageUrl(PartsManagementRequestModel pmr)
        //{
        //    string imageurl = string.Empty;
        //    bool lExternal = false;
        //    string sasToken = string.Empty;
        //    #region New Code            
        //    AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
        //    string imgName = "";
        //    string fileName = aset.CreateFileNamebyObject("PartMasterRequestImage", pmr.PartMasterRequestId.ToString(), imgName);
        //    AzureUtil.AzureBlob obj = aset.RetrieveAzureBlob(pmr.ClientId, pmr.SiteId, fileName);
        //    imageurl = obj.ImageURI;
        //    #endregion

        //    #region Not implemented
        //    //Attachment attach = new Attachment()
        //    //{
        //    //    ClientId = userData.DatabaseKey.Client.ClientId,
        //    //    ObjectName = "PartMasterRequest",
        //    //    ObjectId = PartMasterRequestId,
        //    //    Profile = true,
        //    //    Image = true
        //    //};
        //    //List<Attachment> AList = attach.RetrieveProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);
        //    //if (AList.Count > 0)
        //    //{
        //    //    lExternal = AList.First().External;
        //    //    imageurl = AList.First().AttachmentURL;
        //    //}
        //    //else
        //    //{
        //    //    imageurl = PartMasterRequest.ImageURL;
        //    //    if (imageurl == null || imageurl == "")
        //    //    {
        //    //        imageurl = "No Image";
        //    //        lExternal = true;
        //    //    }
        //    //    else if (imageurl.Contains("somaxclientstorage"))
        //    //    {
        //    //        lExternal = false;
        //    //    }
        //    //    else
        //    //    {
        //    //        lExternal = true;
        //    //    }
        //    //}
        //    //if (lExternal)
        //    //{
        //    //    if (imageurl == "No Image")
        //    //    {
        //    //        const string UploadDirectory = "../Images/DisplayImg/";
        //    //        const string ThumbnailFileName = "NoImage.jpg";
        //    //        imageurl = UploadDirectory + ThumbnailFileName;
        //    //    }
        //    //    else
        //    //    {
        //    //        imageurl = imageurl;
        //    //    }
        //    //}
        //    //else // SOMAX Storage 
        //    //{
        //    //    AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
        //    //    sasToken = aset.GetSASUrlClient(userData.DatabaseKey.Client.ClientId, imageurl);
        //    //    if (sasToken == null || sasToken == "" || imageurl.Contains("No Image"))
        //    //    {
        //    //        const string UploadDirectory = "../Images/DisplayImg/";
        //    //        const string ThumbnailFileName = "NoImage.jpg";
        //    //        imageurl = UploadDirectory + ThumbnailFileName;
        //    //    }
        //    //    else
        //    //    {
        //    //        imageurl = sasToken;
        //    //    }
        //    //}
        //    #endregion

        //    return imageurl;
        //}

        public string GetPartManagementRequestImageUrl1(long PartMasterRequestId, PartsManagementRequestModel partsManagementRequestModel = null)
        {
            string imageurl = string.Empty;
            AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
            Int64 Clientid = userData.DatabaseKey.Client.ClientId;
            Int64 Siteid = userData.DatabaseKey.User.DefaultSiteId;
            string imgName = "";
            string fileName = aset.CreateFileNamebyObject("PartMasterRequestImage", PartMasterRequestId.ToString(), imgName);

            AzureBlob obj = aset.RetrieveAzureBlob(Clientid, Siteid, fileName);

            imageurl = obj.ImageURI;
            if (imageurl == null || imageurl == "")
            {
                const string UploadDirectory = "../Images/DisplayImg/";
                const string ThumbnailFileName = "NoImage.jpg";
                imageurl = UploadDirectory + ThumbnailFileName;
            }
            return imageurl;
        }
        #endregion

        #region Buttons
        public List<string> SavePMRSendApproval(PartMRequestSendApprovalModel pmrSendApprove)
        {
            PartMasterRequest pmr = new PartMasterRequest();
            pmr.ClientId = userData.DatabaseKey.Client.ClientId;
            pmr.PartMasterRequestId = Convert.ToInt64(pmrSendApprove.PartMasterRequestId);
            pmr.Retrieve(userData.DatabaseKey);
            pmr.Status = PartMasterRequestStatusConstants.Review;
            pmr.LastReviewedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            pmr.LastReviewed_Date = DateTime.UtcNow;
            pmr.UpdateIndex = Convert.ToInt32(pmr.UpdateIndex.ToString());
            pmr.Update(userData.DatabaseKey);
            ReviewLog rlog = new ReviewLog()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                TableName = "PartMasterRequest",
                ObjectId = Convert.ToInt64(pmrSendApprove.PartMasterRequestId),
                Function = ReviewLogConstants.Reviewed,
                PersonnelId = userData.DatabaseKey.Personnel.PersonnelId,
                Comments = pmrSendApprove.Comment,
                ReviewDate = DateTime.UtcNow
            };
            rlog.Create(userData.DatabaseKey);

            //DataContracts.PartMasterRequest partMasterRequest = new DataContracts.PartMasterRequest()
            //{
            //    ClientId = userData.DatabaseKey.Client.ClientId,
            //    SiteId = userData.DatabaseKey.Personnel.SiteId,
            //    PartMasterRequestId = Convert.ToInt64(pmrSendApprove.PartMasterRequestId),
            //};
            //partMasterRequest.RetrieveByPKForPMLocalAdd(userData.DatabaseKey, userData.Site.TimeZone);
            //string Type = CheckRequestType(partMasterRequest.RequestType);
            if(pmrSendApprove.SendToId > 0) 
            {
                    Alerts alerts = new Alerts()
                    {
                        ClientId = userData.DatabaseKey.Client.ClientId,
                        ObjectId = pmr.PartMasterRequestId,
                        ObjectName = "PartMasterRequest",
                        AlertName = AlertTypeEnum.PartMasterRequestSiteApprovalNeeded.ToString(),
                        PersonnelId = userData.DatabaseKey.Personnel.PersonnelId
                    };
                    alerts.ClearAlert(userData.DatabaseKey);
                    ProcessAlert objAlert = new ProcessAlert(this.userData);
                    List<long> PRlist = new List<long>();
                    PRlist.Add(pmr.PartMasterRequestId);
                    List<long> targets = new List<long>();
                    targets.Add(pmrSendApprove.SendToId ?? 0);
                    objAlert.SetTargetList(targets);
                    objAlert.CreateAlert<PartMasterRequest>(AlertTypeEnum.PartMasterRequestSiteApprovalNeeded, PRlist);
                
            }
            //both the codes are identical in IF block and Else Block and it does not depend upon RequestType
            //if (Type == PartMasterRequestTypeConstants.Replacement
            //    || Type == PartMasterRequestTypeConstants.Inactivation
            //    || Type == PartMasterRequestTypeConstants.SX_Replacement
            //    || Type == PartMasterRequestTypeConstants.Addition)
            //{
            //    Alerts alerts = new Alerts()
            //    {
            //        ClientId = userData.DatabaseKey.Client.ClientId,
            //        ObjectId = pmr.PartMasterRequestId,
            //        ObjectName = "PartMasterRequest",
            //        AlertName = AlertTypeEnum.PartMasterRequestSiteApprovalNeeded.ToString(),
            //        PersonnelId = userData.DatabaseKey.Personnel.PersonnelId
            //    };
            //    alerts.ClearAlert(userData.DatabaseKey);
            //    ProcessAlert objAlert = new ProcessAlert(this.userData);
            //    List<long> PRlist = new List<long>();
            //    PRlist.Add(pmr.PartMasterRequestId);
            //    List<long> targets = new List<long>();
            //    targets.Add(pmrSendApprove.SendToId??0);
            //    objAlert.SetTargetList(targets);
            //    objAlert.CreateAlert<PartMasterRequest>(AlertTypeEnum.PartMasterRequestSiteApprovalNeeded, PRlist);
            //}
            //else
            //{
            //    Alerts alerts = new Alerts()
            //    {
            //        ClientId = userData.DatabaseKey.Client.ClientId,
            //        ObjectId = pmr.PartMasterRequestId,
            //        ObjectName = "PartMasterRequest",
            //        AlertName = AlertTypeEnum.PartMasterRequestSiteApprovalNeeded.ToString(),
            //        PersonnelId = userData.DatabaseKey.Personnel.PersonnelId
            //    };
            //    alerts.ClearAlert(userData.DatabaseKey);
            //    ProcessAlert objAlert = new ProcessAlert(this.userData);
            //    List<long> PRlist = new List<long>();
            //    PRlist.Add(pmr.PartMasterRequestId);
            //    List<long> targets = new List<long>();
            //    targets.Add(pmrSendApprove.SendToId??0);
            //    objAlert.SetTargetList(targets);
            //    objAlert.CreateAlert<PartMasterRequest>(AlertTypeEnum.PartMasterRequestSiteApprovalNeeded, PRlist);
            //}
            return pmr.ErrorMessages;
        }

        public List<string> SavePMRReturnRequester(PartMRequestReturnRequesterModel pmrReturnRequester)
        {
            DateTime now = DateTime.UtcNow;
            PartMasterRequest pmr = new PartMasterRequest();
            pmr.ClientId = userData.DatabaseKey.Client.ClientId;
            pmr.PartMasterRequestId = Convert.ToInt64(pmrReturnRequester.PartMasterRequestId);
            pmr.Retrieve(userData.DatabaseKey);
            pmr.Status = PartMasterRequestStatusConstants.Returned;
            pmr.ApproveDeny_Date = null;
            pmr.ApproveDenyBy_PersonnelId = 0;
            pmr.LastReviewedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            pmr.LastReviewed_Date = now;
            pmr.UpdateIndex = Convert.ToInt32(pmr.UpdateIndex.ToString());
            pmr.Update(userData.DatabaseKey);
            ReviewLog rlog = new ReviewLog()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                TableName = "PartMasterRequest",
                ObjectId = Convert.ToInt64(pmrReturnRequester.PartMasterRequestId),
                Function = ReviewLogConstants.Returned,
                PersonnelId = userData.DatabaseKey.Personnel.PersonnelId,
                Comments = pmrReturnRequester.Comment,
                ReviewDate = now
            };
            rlog.Create(userData.DatabaseKey);
            DataContracts.PartMasterRequest partMasterRequest = new DataContracts.PartMasterRequest()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.Personnel.SiteId,
                PartMasterRequestId = Convert.ToInt64(pmrReturnRequester.PartMasterRequestId),
            };
            partMasterRequest.RetrieveByPKForPMLocalAdd(userData.DatabaseKey, userData.Site.TimeZone);
            string Type = CheckRequestType(partMasterRequest.RequestType);
            if (Type == PartMasterRequestTypeConstants.Replacement
                || Type == PartMasterRequestTypeConstants.Inactivation
                || Type == PartMasterRequestTypeConstants.SX_Replacement
                || Type == PartMasterRequestTypeConstants.Addition)
            {
                Alerts alerts = new Alerts()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    ObjectId = pmr.PartMasterRequestId,
                    ObjectName = "PartMasterRequest",
                    AlertName = AlertTypeEnum.PartMasterRequestSiteApprovalNeeded.ToString(),
                };
                alerts.ClearAlert(userData.DatabaseKey);
                alerts = new Alerts()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    ObjectId = pmr.PartMasterRequestId,
                    ObjectName = "PartMasterRequest",
                    AlertName = AlertTypeEnum.PartMasterRequestApprovalNeeded.ToString(),
                };
                alerts.ClearAlert(userData.DatabaseKey);
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                List<long> PRlist = new List<long>();
                PRlist.Add(pmr.PartMasterRequestId);
                List<long> targets = new List<long>();
                targets.Add(pmr.CreatedBy_PersonnelId);
                objAlert.SetTargetList(targets);
                objAlert.CreateAlert<PartMasterRequest>(AlertTypeEnum.PartMasterRequestReturned, PRlist);
            }
            else
            {
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                List<long> PRlist = new List<long>();
                PRlist.Add(pmr.PartMasterRequestId);
                List<long> targets = new List<long>();
                targets.Add(pmr.CreatedBy_PersonnelId);
                objAlert.SetTargetList(targets);
                objAlert.CreateAlert<PartMasterRequest>(AlertTypeEnum.PartMasterRequestReturned, PRlist);
                Alerts alerts = new Alerts()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    ObjectId = pmr.PartMasterRequestId,
                    ObjectName = "PartMasterRequest",
                    AlertName = AlertTypeEnum.PartMasterRequestSiteApprovalNeeded.ToString(),
                };
                alerts.ClearAlert(userData.DatabaseKey);
                alerts = new Alerts()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    ObjectId = pmr.PartMasterRequestId,
                    ObjectName = "PartMasterRequest",
                    AlertName = AlertTypeEnum.PartMasterRequestApprovalNeeded.ToString(),
                };
                alerts.ClearAlert(userData.DatabaseKey);
            }

            return pmr.ErrorMessages;
        }

        public List<string> SavePMRDeny(PartMRequestDenyModel pmrDeny)
        {
            long PMReqId = Convert.ToInt64(pmrDeny.PartMasterRequestId);
            PartMasterRequest pmr = new PartMasterRequest();
            pmr.ClientId = userData.DatabaseKey.Client.ClientId;
            pmr.PartMasterRequestId = PMReqId;
            pmr.Retrieve(userData.DatabaseKey);
            pmr.Status = PartMasterRequestStatusConstants.Denied;
            pmr.ApproveDenyBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            pmr.ApproveDeny_Date = DateTime.UtcNow;
            pmr.UpdateIndex = Convert.ToInt32(pmr.UpdateIndex.ToString());
            pmr.Update(userData.DatabaseKey);
            if (pmr.RequestType == PartMasterRequestTypeConstants.Inactivation)
            {
                pmr.UpdateForDenied(userData.DatabaseKey);
            }
            ReviewLog rlog = new ReviewLog()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                TableName = "PartMasterRequest",
                ObjectId = PMReqId,
                Function = ReviewLogConstants.Denied,
                PersonnelId = userData.DatabaseKey.Personnel.PersonnelId,
                Comments = pmrDeny.Comment,
                ReviewDate = DateTime.UtcNow
            };
            rlog.Create(userData.DatabaseKey);
            DataContracts.PartMasterRequest partMasterRequest = new DataContracts.PartMasterRequest()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.Personnel.SiteId,
                PartMasterRequestId = PMReqId
            };
            partMasterRequest.RetrieveByPKForPMLocalAdd(userData.DatabaseKey, userData.Site.TimeZone);
            Alerts alerts = new Alerts()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectId = pmr.PartMasterRequestId,
                ObjectName = "PartMasterRequest",
                AlertName = AlertTypeEnum.PartMasterRequestSiteApprovalNeeded.ToString(),
                PersonnelId = 0
            };
            alerts.ClearAlert(userData.DatabaseKey);
           
            string Type = CheckRequestType(partMasterRequest.RequestType);
            if (Type == PartMasterRequestTypeConstants.ECO_New
               || Type == PartMasterRequestTypeConstants.ECO_Replace
               || Type == PartMasterRequestTypeConstants.ECO_SX_Replace)
            { 
                alerts = new Alerts()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    ObjectId = pmr.PartMasterRequestId,
                    ObjectName = "PartMasterRequest",
                    AlertName = AlertTypeEnum.PartMasterRequestApprovalNeeded.ToString(),
                };
                alerts.ClearAlert(userData.DatabaseKey);
            }
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            List<long> PMRList = new List<long>();
            PMRList.Add(pmr.PartMasterRequestId);
            objAlert.CreateAlert<PartMasterRequest>(AlertTypeEnum.PartMasterRequestDenied, PMRList);
            return pmr.ErrorMessages;
        }

        public List<string> SaveSiteApprove(long PartMasterRequestId)
        {
            PartMasterRequest pmr = new PartMasterRequest();
            pmr.ClientId = userData.DatabaseKey.Client.ClientId;
            pmr.PartMasterRequestId = PartMasterRequestId;
            pmr.Retrieve(userData.DatabaseKey);
            pmr.ApproveDenyBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            pmr.ApproveDeny_Date = DateTime.UtcNow;
            if (pmr.RequestType == PartMasterRequestTypeConstants.Addition || pmr.RequestType == PartMasterRequestTypeConstants.Replacement || pmr.RequestType == PartMasterRequestTypeConstants.Inactivation || pmr.RequestType == PartMasterRequestTypeConstants.SX_Replacement)
            {
                pmr.Status = PartMasterRequestStatusConstants.Approved;
            }
            else
            {
                pmr.Status = PartMasterRequestStatusConstants.SiteApproved;
            }
            pmr.UpdateIndex = Convert.ToInt32(pmr.UpdateIndex.ToString());
            pmr.Update(userData.DatabaseKey);
            pmr.Clear();
            pmr.ClientId = userData.DatabaseKey.Client.ClientId;
            pmr.PartMasterRequestId = PartMasterRequestId;
            pmr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            ReviewLog rlog = new ReviewLog()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                TableName = "PartMasterRequest",
                ObjectId = PartMasterRequestId,
                Function = ReviewLogConstants.SiteApprove,
                PersonnelId = userData.DatabaseKey.Personnel.PersonnelId,
                Comments = "Site Approved Part Master Request",
                ReviewDate = DateTime.UtcNow
            };
            rlog.Create(userData.DatabaseKey);
            try
            {
            }
            catch (Exception ex)
            {

            }
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            List<long> PRlist = new List<long>();
            PRlist.Add(pmr.PartMasterRequestId);
            try
            {
                objAlert.CreateAlert<PartMasterRequest>(AlertTypeEnum.PartMasterRequestSiteApproved, PRlist);
            }
            catch (Exception ex)
            {

            }
            if (pmr.RequestType != PartMasterRequestTypeConstants.Addition && pmr.RequestType != PartMasterRequestTypeConstants.Replacement && pmr.RequestType != PartMasterRequestTypeConstants.Inactivation && pmr.RequestType != PartMasterRequestTypeConstants.SX_Replacement)
            {
                objAlert = new ProcessAlert(this.userData);
                try
                {
                    objAlert.CreateAlert<PartMasterRequest>(AlertTypeEnum.PartMasterRequestApprovalNeeded, PRlist);
                }
                catch (Exception ex)
                {

                }
            }
            Alerts alerts = new Alerts()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectId = pmr.PartMasterRequestId,
                ObjectName = "PartMasterRequest",
                AlertName = AlertTypeEnum.PartMasterRequestSiteApprovalNeeded.ToString(),
                PersonnelId = 0
            };
            try
            {
                alerts.ClearAlert(userData.DatabaseKey);
            }
            catch (Exception ex)
            {

            }
            return pmr.ErrorMessages;
        }

        public List<string> Cancel(long PartMasterRequestId)
        {
            PartMasterRequest pmr = new PartMasterRequest();
            pmr.ClientId = userData.DatabaseKey.Client.ClientId;
            pmr.PartMasterRequestId = PartMasterRequestId;
            pmr.Retrieve(userData.DatabaseKey);
            pmr.Status = PartMasterRequestStatusConstants.Canceled;
            pmr.UpdateIndex = Convert.ToInt32(pmr.UpdateIndex.ToString());
            pmr.Update(userData.DatabaseKey);
            ReviewLog rlog = new ReviewLog()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                TableName = "PartMasterRequest",
                ObjectId = PartMasterRequestId,
                Function = ReviewLogConstants.Canceled,
                PersonnelId = userData.DatabaseKey.Personnel.PersonnelId,
                Comments = "Canceled by Creator",
                ReviewDate = DateTime.UtcNow
            };
            rlog.Create(userData.DatabaseKey);
            pmr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            return pmr.ErrorMessages;
        }


        public List<string> SaveEnterpriseApprove(long PartMasterRequestId)
        {
            PartMasterRequest pmr = new PartMasterRequest();
            pmr.ClientId = userData.DatabaseKey.Client.ClientId;
            pmr.PartMasterRequestId = PartMasterRequestId;
            pmr.Retrieve(userData.DatabaseKey);
            pmr.ApproveDenyBy2_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            pmr.ApproveDeny2_Date = DateTime.UtcNow;
            pmr.Status = PartMasterRequestStatusConstants.Approved;
            pmr.UpdateIndex = Convert.ToInt32(pmr.UpdateIndex.ToString());
            pmr.Update(userData.DatabaseKey);
            pmr.Clear();
            pmr.ClientId = userData.DatabaseKey.Client.ClientId;
            pmr.PartMasterRequestId = PartMasterRequestId;
            pmr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            ReviewLog rlog = new ReviewLog()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                TableName = "PartMasterRequest",
                ObjectId = PartMasterRequestId,
                Function = ReviewLogConstants.EnterpriseApprove,
                PersonnelId = userData.DatabaseKey.Personnel.PersonnelId,
                Comments = "Enterprise Approved Part Master Request",
                ReviewDate = DateTime.UtcNow
            };
            rlog.Create(userData.DatabaseKey);
            try
            {
                CreateMailTemplate mt = new CreateMailTemplate(pmr, this.userData);
              bool send_email = !String.IsNullOrEmpty(mt.email_address);
              
                if (send_email)
                {
                    string body = mt.createEmailBody();
                    mt.SendHtmlFormattedEmail(mt.Subject, body);
                }
            }
            catch (Exception ex)
            {

            }
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            List<long> PRlist = new List<long>();
            PRlist.Add(pmr.PartMasterRequestId);
            try
            {
                objAlert.CreateAlert<PartMasterRequest>(AlertTypeEnum.PartMasterRequestApproved, PRlist);
            }
            catch (Exception ex)
            {

            }
            Alerts alerts = new Alerts()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectId = pmr.PartMasterRequestId,
                ObjectName = "PartMasterRequest",
                AlertName = AlertTypeEnum.PartMasterRequestApprovalNeeded.ToString(),
                PersonnelId = 0
            };
            try
            {
                alerts.ClearAlert(userData.DatabaseKey);
            }
            catch (Exception ex)
            {

            }
            return pmr.ErrorMessages;
        }

        public PartMasterRequest EditPartManagementRequest(PartsManagementRequestDetailModel _PartsManagementRequestModel)
        {

            PartMasterRequest partMasterRequest = new PartMasterRequest()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.Personnel.SiteId,
                PartMasterRequestId = _PartsManagementRequestModel.PartMasterRequestId
            };
            partMasterRequest.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            partMasterRequest.Justification = _PartsManagementRequestModel.Justification;
            partMasterRequest.Part_ClientLookupId = _PartsManagementRequestModel.Part_ClientLookupId;
            partMasterRequest.PartMaster_ClientLookupId = _PartsManagementRequestModel.PartMaster_ClientLookupId;
            partMasterRequest.UpdateByPk(this.userData.DatabaseKey);
            return partMasterRequest;
        }

        public PartMasterRequest EditPartManagementRequestSite(PartsManagementRequestModel _PartsManagementRequestModel)
        {
            PartMasterRequest partMasterRequest = new PartMasterRequest()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PartMasterRequestId = _PartsManagementRequestModel.PartMasterRequestId
            };
            partMasterRequest.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            partMasterRequest.Description = _PartsManagementRequestModel.Description;                                                                                 
            partMasterRequest.Justification = _PartsManagementRequestModel.Justification;                                                                                      
            partMasterRequest.Manufacturer = _PartsManagementRequestModel.Manufacturer;                                                                                      
            partMasterRequest.UnitOfMeasure = _PartsManagementRequestModel.UnitOfMeasure;                                                                                          
            partMasterRequest.ManufacturerId = _PartsManagementRequestModel.ManufacturerId;                                                                                         
            partMasterRequest.PurchaseFreq = _PartsManagementRequestModel.PurchaseFreq;                                                                                    
            partMasterRequest.Critical = _PartsManagementRequestModel.Critical;
            partMasterRequest.PurchaseLeadTime = _PartsManagementRequestModel.PurchaseLeadTime;
            partMasterRequest.PurchaseCost = _PartsManagementRequestModel.PurchaseCost;
            #region V2-798
            partMasterRequest.UnitCost = _PartsManagementRequestModel.UnitCost ?? 0;
            partMasterRequest.Location = _PartsManagementRequestModel.Location;
            partMasterRequest.QtyMinimum = _PartsManagementRequestModel.QtyMinimum ?? 0;
            partMasterRequest.QtyMaximum = _PartsManagementRequestModel.QtyMaximum ?? 0;
            #endregion
            string MFG_ClientLookupId = partMasterRequest.ManufacturerId.ToUpper().Trim();
            partMasterRequest.ManufacturerId = MFG_ClientLookupId;          
            try
            {            
                partMasterRequest.ImageURL = partMasterRequest.ImageURL;
                partMasterRequest.Update(this.userData.DatabaseKey);
            }
            catch (Exception ex)
            {
                throw ex;               
            }
            return partMasterRequest;
        }
        #endregion

        #region V2-798
        public List<PartsManagementRequestModel> GetPartsManagementRequestsChunkList(long Searchvalue = 0, int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", long? PartMasterRequestId = 0, long? Requestor = 0, string Justification = "", string RequestType = "", string Status = "", string Manufacturer = "", string ManufacturerId = "")
        {
            PartsManagementRequestModel partsManagementRequestModel;
            List<PartsManagementRequestModel> partsManagementRequestModelList = new List<PartsManagementRequestModel>();

            DataContracts.PartMasterRequest PartMasterRequestSearch = new DataContracts.PartMasterRequest();
            PartMasterRequestSearch.ClientId = userData.DatabaseKey.Client.ClientId;
            PartMasterRequestSearch.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            PartMasterRequestSearch.CreatedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            PartMasterRequestSearch.CustomQueryDisplayId = Searchvalue;
            PartMasterRequestSearch.orderbyColumn = orderbycol;
            PartMasterRequestSearch.orderBy = orderDir;
            PartMasterRequestSearch.offset1 = Convert.ToString(skip);
            PartMasterRequestSearch.nextrow = Convert.ToString(length);

            PartMasterRequestSearch.PartMasterRequestId = PartMasterRequestId.HasValue ? PartMasterRequestId.Value : 0;
            PartMasterRequestSearch.CreateById = Requestor.HasValue ? Requestor.Value : 0;
            PartMasterRequestSearch.Justification = Justification;
            PartMasterRequestSearch.RequestType = RequestType;
            PartMasterRequestSearch.ManufacturerId = ManufacturerId;
            PartMasterRequestSearch.Manufacturer = Manufacturer;
            PartMasterRequestSearch.Status = Status;
      //List<DataContracts.PartMasterRequest> GridSource = PartMasterRequestSearch.Retrieve_SanitationJobSearch_ByFilterCriteria(this.userData.DatabaseKey, userData.Site.TimeZone);
      List<DataContracts.PartMasterRequest> GridSource = PartMasterRequestSearch.Retrieve_PartMasterRequests_ByFilterCriteria(this.userData.DatabaseKey, userData.Site.TimeZone);
            foreach (var item in GridSource)
            {
                partsManagementRequestModel = new PartsManagementRequestModel();
                partsManagementRequestModel.PartMasterRequestId = item.PartMasterRequestId;
                partsManagementRequestModel.Requester = item.Requester;
                partsManagementRequestModel.Justification = item.Justification;
                partsManagementRequestModel.RequestType = item.RequestType;
                partsManagementRequestModel.Status_Display = item.Status;
                partsManagementRequestModel.Manufacturer = item.Manufacturer;
                partsManagementRequestModel.ManufacturerId = item.ManufacturerId;
                partsManagementRequestModel.TotalCount = item.TotalCount;
                partsManagementRequestModel.LocalizedRequestType = UtilityFunction.GetMessageFromResource(item.RequestType, LocalizeResourceSetConstants.PartManagementRequest) ?? "";
                partsManagementRequestModelList.Add(partsManagementRequestModel);
            }
            return partsManagementRequestModelList;
        }
        public PartMasterRequest EditPartManagementRequestForAssign(AssignPartMastertoIndusnetBakeryModel _PartsManagementRequestModel)
        {

            PartMasterRequest partMasterRequest = new PartMasterRequest()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.Personnel.SiteId,
                PartMasterRequestId = _PartsManagementRequestModel.PartMasterRequestId
            };
            partMasterRequest.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            partMasterRequest.Justification = _PartsManagementRequestModel.Justification;
            partMasterRequest.UnitCost = _PartsManagementRequestModel.UnitCost ?? 0;
            partMasterRequest.Location = _PartsManagementRequestModel.Location;
            partMasterRequest.QtyMinimum = _PartsManagementRequestModel.QtyMinimum ?? 0;
            partMasterRequest.QtyMaximum = _PartsManagementRequestModel.QtyMaximum ?? 0;
            partMasterRequest.Part_ClientLookupId = _PartsManagementRequestModel.Part_ClientLookupId;
            partMasterRequest.PartMaster_ClientLookupId = _PartsManagementRequestModel.PartMaster_ClientLookupId;
            partMasterRequest.UpdateByPk(this.userData.DatabaseKey);
            return partMasterRequest;
        }
        #endregion
    }
    public class CreateMailTemplate
    {
        #region Properties
        public string PartMasterRequestId { get; set; }
        public string RequestType { get; set; }
        public string Critical { get; set; }
        public string PurchFreq { get; set; }
        public string LeadTime { get; set; }
        public string ItemCost { get; set; }
        public string ItemDesc { get; set; }
        public string Manu { get; set; }
        public string ManuPartNo { get; set; }
        public string UOM { get; set; }
        public string PhotoURL { get; set; }
        public string chrYES { get; set; }
        public string chrNO { get; set; }
        public DateTime  approved_date { get; set; }
        public string email_address { get; set; }
        public string Subject { get; set; }
        private UserData userData;
        private DatabaseKey _dbKey;
        #endregion

        #region Constructor
        public CreateMailTemplate(string subject, string body)
        {
            SendHtmlFormattedEmail(subject, body);
        }
        public CreateMailTemplate(PartMasterRequest pmr, UserData ud)
        {         
            string unitom;
            LookupList lu = new LookupList();

            List<LookupList> lulist = lu.RetrieveList(ud.DatabaseKey, "UNIT_OF_MEASURE"
                                                                  , string.Empty
                                                                  , pmr.SiteId
                                                                  , pmr.UnitOfMeasure);


            if (lulist.Count > 0)
            {
                unitom = lulist[0].Description;
            }
            else
            {
                unitom = pmr.UnitOfMeasure;
            }
            this.userData = ud;           
            this.chrYES = "&#9989;";
            this.chrNO = "&#10060;";
            this.Critical = pmr.Critical == true ? chrYES : chrNO;
            this.PurchFreq = pmr.PurchaseFreq.ToString().ToLower().Contains("monthly") ? chrYES : chrNO;
            this.LeadTime = pmr.PurchaseLeadTime.ToString().ToLower().Contains("fivedays") == true ? chrNO : chrYES;
            this.ItemCost = pmr.PurchaseCost.ToString().ToLower().Contains("le200") == true || pmr.PurchaseCost.ToString().ToLower().Contains("le100") == true ? chrNO : chrYES;
            this.ItemDesc = pmr.Description;
            this.PartMasterRequestId = pmr.PartMasterRequestId.ToString();
            if (pmr.RequestType == PartMasterRequestTypeConstants.ECO_SX_Replace)
            {
                this.RequestType = "ECO_SX_REPLACEMENT";
            }
            else
            {
                this.RequestType = pmr.RequestType.ToUpper();
            }
            this.Manu = pmr.Manufacturer;
            this.ManuPartNo = pmr.ManufacturerId;
           
            this.UOM = unitom;
           
            this.PhotoURL = pmr.ImageURL;
            this.approved_date = pmr.CreatedDate ?? DateTime.MinValue;
            string approved_dte = string.Empty;
            if (approved_date > DateTime.MinValue)
            {
                approved_dte = approved_date.ToString("g");
            }           
            Site pmr_site = new Site()
            {
                ClientId = pmr.ClientId,
                SiteId = pmr.SiteId
            };
            pmr_site.Retrieve(userData.DatabaseKey);
            email_address = String.IsNullOrEmpty(pmr_site.PartMasterReqEmail.ToString()) == false ? pmr_site.PartMasterReqEmail.ToString() : "";
            string subj = "{0}:{1}:SOMAX Part Request No:{2}:Date:{3}";
            Subject = string.Format(subj, pmr_site.Name, pmr.Requester + " " + pmr.Requester_Email, pmr.PartMasterRequestId.ToString(), approved_dte);
          
        }

        public CreateMailTemplate(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #endregion

        #region MailBody
        public string createEmailBody()
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Views/PartsManagementRequest/PartmasterRequestTemplate.html")))
            {
                body = reader.ReadToEnd();
            }
            if (Critical == chrYES)
            {
                body = body.Replace("#CY1", Critical);
                body = body.Replace("#CN1", chrNO);
            }
            else
            {
                body = body.Replace("#CY1", Critical);
                body = body.Replace("#CN1", chrYES);
            }

            if (PurchFreq == chrYES)
            {
                body = body.Replace("#CY2", PurchFreq);
                body = body.Replace("#CN2", chrNO);
            }
            else
            {
                body = body.Replace("#CY2", PurchFreq);
                body = body.Replace("#CN2", chrYES);
            }
            if (LeadTime == chrYES)
            {
                body = body.Replace("#CY3", LeadTime);
                body = body.Replace("#CN3", chrNO);
            }
            else
            {
                body = body.Replace("#CY3", LeadTime);
                body = body.Replace("#CN3", chrYES);
            }
            if (ItemCost == chrYES)
            {
                body = body.Replace("#CY4", ItemCost);
                body = body.Replace("#CN4", chrNO);
            }
            else
            {
                body = body.Replace("#CY4", ItemCost);
                body = body.Replace("#CN4", chrYES);
            }

            string Critical1 = "Is this item critical for the production line ? If unsure, please contact your plant engineer";
            string PurchFreq1 = "Will this item be purchased more than once per month?";
            string LeadTime1 = "Is the lead time greater than (5) business days?";
            string ItemCost1 = "Is the item cost greater than $200 per unit?";
            string PartMasterIDLabel = "SOMAX Part Master Request No";
            string RequestTypeLabel = "Part Master Request Type";
            string ItemDesc1 = "Item Description";
            string Manu1 = "Manufacturer";
            string ManuPartNo1 = "Manufacturer's Part Number";
            string UOM1 = "Unit of Measure";
            string PhotoURL1 = "Photo URL";
            string PhotoImage1 = "Photo";
            string PartRequest1 = "Part Request";
            string PartRequestNo1 = "Part Request No";
            string RequestDate1 = "Request Date";
            body = body.Replace("#Critical", Critical1);
            body = body.Replace("#PurchFreq", PurchFreq1);
            body = body.Replace("#LeadTime", LeadTime1);
            body = body.Replace("#ItemCost", ItemCost1);
            body = body.Replace("#PartMasterId", PartMasterIDLabel);
            body = body.Replace("#RequestType", RequestTypeLabel);
            body = body.Replace("#ItemDesc", ItemDesc1);
            body = body.Replace("#Manufac", Manu1);
            body = body.Replace("#ManuPartNo", ManuPartNo1);
            body = body.Replace("#UOM", UOM1);
            body = body.Replace("#PhotoURL", PhotoURL1);
            body = body.Replace("#PhotoImage", PhotoImage1);
            body = body.Replace("#ValPartMasterId", PartMasterRequestId);
            body = body.Replace("#PartRequest", PartRequest1);
            body = body.Replace("#PartRequestNo", PartRequestNo1);
            body = body.Replace("#RequestDate", RequestDate1);
            if (approved_date != null && approved_date == default(DateTime))
            {
                body = body.Replace("#ValPartMasterRequestDate", "");
            }
            else
            {
                body = body.Replace("#ValPartMasterRequestDate", Convert.ToString(approved_date));
            }
            body = body.Replace("#ValReqType", RequestType);
            body = body.Replace("#ValDesc", ItemDesc);
            body = body.Replace("#ValManufac", Manu);
            body = body.Replace("#ValManuPartNo", ManuPartNo);
            body = body.Replace("#ValUOM", UOM);

            if (string.IsNullOrWhiteSpace(PhotoURL))
            {
                PhotoURL = commonWrapper.GetNoImageUrl();
            }
            body = body.Replace("#ValPhotoURL", PhotoURL);
            return body;

        }
        public string createEmailBody(string TemplatePath)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(TemplatePath)))
            {
                body = reader.ReadToEnd();
            }
            if (Critical == chrYES)
            {
                body = body.Replace("#CY1", "<i class='fa fa-check' style='color:green'></i>");
                body = body.Replace("#CN1", "<i class='fa fa-times' style='color:red'></i>");
            }
            else
            {
                body = body.Replace("#CY1", chrNO);
                body = body.Replace("#CN1", Critical);
            }

            if (PurchFreq == chrYES)
            {
                body = body.Replace("#CY2", PurchFreq);
                body = body.Replace("#CN2", chrNO);
            }
            else
            {
                body = body.Replace("#CY2", chrNO);
                body = body.Replace("#CN2", PurchFreq);
            }
            if (LeadTime == chrYES)
            {
                body = body.Replace("#CY3", LeadTime);
                body = body.Replace("#CN3", chrNO);
            }
            else
            {
                body = body.Replace("#CY3", chrNO);
                body = body.Replace("#CN3", LeadTime);
            }
            if (ItemCost == chrYES)
            {
                body = body.Replace("#CY4", ItemCost);
                body = body.Replace("#CN4", chrNO);
            }
            else
            {
                body = body.Replace("#CY4", chrNO);
                body = body.Replace("#CN4", ItemCost);
            }
            body = body.Replace("#ValDesc", ItemDesc);
            body = body.Replace("#ValManu", Manu);
            body = body.Replace("#ValManuPartNo", ManuPartNo);
            if (approved_date != null && approved_date == default(DateTime))
            {
                body = body.Replace("#ValPartMasterRequestDate", "");
            }
            else
            {
                body = body.Replace("#ValPartMasterRequestDate", Convert.ToString(approved_date));
            }           
            body = body.Replace("#ValUOM", UOM);
           
            body = body.Replace("#ValPhotoURL", PhotoURL);
            return body;

        }
        #endregion

        #region SendMail
        public void SendHtmlFormattedEmail(string subject, string body)
        {           
            EmailModule emailModule = new Presentation.Common.EmailModule();
            emailModule.ToEmailAddress = email_address;
            //emailModule.MailSubject = subject;
            emailModule.Subject = subject;
            emailModule.MailBody = body;
            bool IsSent = emailModule.SendEmail();
        }
        #endregion


    }
}