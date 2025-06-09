using System.Collections.Generic;
using Client.Models;
using Common.Constants;
using DataContracts;
using Client.Common;
using System;
using Common.Extensions;
using Client.Models.Approval;
using System.Globalization;
using System.Threading.Tasks;
using Client.BusinessWrapper.Common;
using Common.Enumerations;

namespace Client.BusinessWrapper
{
    public class ApprovalWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        internal List<string> errorMessage = new List<string>();
        public ApprovalWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        public List<ApprovalRouteModel> GetApprovalRouteWRGriddata(long approverId, string FilterType, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0)
        {
            List<ApprovalRouteModel> alllist = new List<ApprovalRouteModel>();
            ApprovalRoute approvalroute = new ApprovalRoute()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.Site.SiteId,
                ApproverId = approverId,
                FilterTypeCase = FilterType,
                OrderbyColumn = orderbycol,
                OrderBy = orderDir,
                Offset = skip,
                Nextrow = length
            };
            List<ApprovalRoute> approvalRouteWRlist = ApprovalRoute.ApprovalRoute_RetrieveForWorkRequest_V2(this.userData.DatabaseKey, approvalroute);

            foreach (var item in approvalRouteWRlist)
            {
                ApprovalRouteModel data = new ApprovalRouteModel();
                data.ClientLookupId = item.ClientLookupId;
                data.ChargeTo = item.ChargeTo;
                data.ChargeToName = item.ChargeToName;
                data.Type = item.Type;
                data.Comments = item.Comments;
                data.WorkOrderId = item.WorkOrderId;
                data.Description = item.Description;
                data.Comments = item.Comments;
                data.ApprovalGroupId = item.ApprovalGroupId;
                if (item.Date != null && item.Date == default(DateTime))
                {
                    data.Date = null;
                }
                else
                {
                    data.Date = item.Date;
                }
                data.TotalCount = item.TotalCount;
                alllist.Add(data);
            }
            return alllist;
        }

        #region Purchase rquest
        internal List<PRApprovalSearchModel> GetApprovalRoutePRGriddata(long approverId, string FilterType, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0)
        {
            var alllist = new List<PRApprovalSearchModel>();
            ApprovalRoute approvalroute = new ApprovalRoute()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.Site.SiteId,
                ApproverId = approverId,
                FilterTypeCase = FilterType,
                OrderbyColumn = orderbycol,
                OrderBy = orderDir,
                Offset = skip,
                Nextrow = length
            };
            List<ApprovalRoute> approvalRoutePRlist = ApprovalRoute.ApprovalRoute_RetrieveForPurchaseRequest_V2(this.userData.DatabaseKey, approvalroute);

            foreach (var item in approvalRoutePRlist)
            {
                var data = new PRApprovalSearchModel();
                data.ApprovalGroupId = item.ApprovalGroupId;
                data.ApprovalRouteId = item.ApprovalRouteId;
                data.PurchaseRequestId = item.PurchaseRequestId;

                data.ClientLookupId = item.ClientLookupId;
                data.VendorName = item.VendorName;
                data.Comments = item.Comments;
                data.ApprovalGroupId = item.ApprovalGroupId;
                if (item.Date != null && item.Date == default(DateTime))
                {
                    data.Date = null;
                }
                else
                {
                    data.Date = item.Date.Value.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                data.TotalCount = item.TotalCount;
                alllist.Add(data);
            }
            return alllist;
        }
        #endregion


        #region MaterialRequest Search
        public List<MaterialRequestModel> GetMaterialRequestList(string orderbycol = "", int length = 0, string orderDir = "", int skip = 0,string FilterTypeCase="")
        {
            List<MaterialRequestModel> MaterialRequestModelList = new List<MaterialRequestModel>();
            
            MaterialRequestModel materialRequestModel;
            ApprovalRoute approvalRoute = new ApprovalRoute();
            approvalRoute.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            approvalRoute.ClientId = userData.DatabaseKey.Client.ClientId;
            approvalRoute.OrderbyColumn = orderbycol;
            approvalRoute.OrderBy = orderDir;
            approvalRoute.Offset = skip;
            approvalRoute.Nextrow = length;
            approvalRoute.FilterTypeCase = FilterTypeCase;
            approvalRoute.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
            var mList = approvalRoute.ApprovalRoute_RetrieveForMaterialRequest(userData.DatabaseKey, approvalRoute);
            foreach (var item in mList)
            {
                materialRequestModel = new MaterialRequestModel();
                materialRequestModel.ApprovalRouteId = item.ApprovalRouteId;
                materialRequestModel.EstimatedCostsId = item.EstimatedCostsId;
                materialRequestModel.MaterialRequestId= item.MaterialRequestId;

                materialRequestModel.ClientLookupId = item.ClientLookupId;
                materialRequestModel.Description = item.Description;
                materialRequestModel.SiteId = item.SiteId;
                materialRequestModel.UnitCost = item.UnitCost;
                materialRequestModel.Quantity = item.Quantity;
                materialRequestModel.TotalCost = item.TotalCost;
                materialRequestModel.Date = item.Date;
                materialRequestModel.Comments = item.Comments;
                materialRequestModel.ApprovalGroupId = item.ApprovalGroupId;
                materialRequestModel.TotalCount = item.TotalCount;
                MaterialRequestModelList.Add(materialRequestModel);
            }
            return MaterialRequestModelList;
        }

        #endregion

        #region V2-769
        private void CreateEventLog(Int64 ApproverId, Int64 ObjectId, string comment, long ApprovalGroupId, string RequestType)
        {
            ApprovalRoute log = new ApprovalRoute();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.ApproverId = ApproverId;
            log.ObjectId = ObjectId;
            log.ApprovalGroupId = ApprovalGroupId;
            log.RequestType = RequestType;
            log.Comments = comment;
            log.IsProcessed = false;
            log.ProcessResponse = String.Empty;
            log.Create(userData.DatabaseKey);
        }
        #region Work Request
        public WorkOrder MultiLevelApproveWR(MultiLevelApproveApprovalRouteModel arModel)
        {
            WorkOrder workOrder = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = arModel.ObjectId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            workOrder.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);

            workOrder.Status = WorkOrderStatusConstants.AwaitingApproval;
            workOrder.Update(userData.DatabaseKey);

            ApprovalRoute AR = new ApprovalRoute();
            AR.ClientId = this.userData.DatabaseKey.Client.ClientId;
            AR.ApprovalGroupId = arModel.ApprovalGroupId;
            AR.ObjectId = arModel.ObjectId;
            AR.ProcessResponse = WorkOrderStatusConstants.Approved;
            AR.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
            AR.UpdateByObjectId_V2(userData.DatabaseKey);

            if (AR.ErrorMessages == null || AR.ErrorMessages.Count == 0)
            {
                Task t1 = Task.Factory.StartNew(() => CreateEventLog(arModel.ApproverId, arModel.ObjectId, arModel.Comments, arModel.ApprovalGroupId, arModel.RequestType));
                List<long> workorderid = new List<long>() { workOrder.WorkOrderId };
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
                    Task t2 = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WRApprovalRouting, workorderid, UserList));
                }
            }
            return workOrder;
        }
        public WorkOrder MultiLevelFinalApproveWR(long workorderId, long ApprovalGroupId)
        {
            WorkOrder workOrder = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workorderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            workOrder.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            if (workOrder.Status == WorkOrderStatusConstants.AwaitingApproval)
            {
                // Approve the work order
                workOrder.Status = WorkOrderStatusConstants.Approved;
                workOrder.ApproveDate = DateTime.UtcNow;
                workOrder.ApproveBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                workOrder.Update(userData.DatabaseKey);

                ApprovalRoute AR = new ApprovalRoute();
                AR.ClientId = this.userData.DatabaseKey.Client.ClientId;
                AR.ApprovalGroupId = ApprovalGroupId;
                AR.ObjectId = workorderId;
                AR.ProcessResponse = WorkOrderStatusConstants.Approved;
                AR.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
                AR.UpdateByObjectId_V2(userData.DatabaseKey);

                if (AR.ErrorMessages == null || AR.ErrorMessages.Count == 0)
                {
                    //Task t1 = Task.Factory.StartNew(() => CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Approved, "Work Order Approved"));

                    List<long> listWO = new List<long>();
                    listWO.Add(workOrder.WorkOrderId);
                    ProcessAlert objAlert = new ProcessAlert(this.userData);
                    Task t2 = Task.Factory.StartNew(() => objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkRequestApproved, listWO));
                }
            }
            return workOrder;

        }

        public WorkOrder MultiLevelDenyWO(long workorderId, long ApprovalGroupId, ref string Statusmsg)
        {
            WorkOrder workOrder = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workorderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            workOrder.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            string strWOStatus = workOrder.Status;
            workOrder.Status = WorkOrderStatusConstants.Denied;
            workOrder.Update(userData.DatabaseKey);
            ApprovalRoute AR = new ApprovalRoute();
            AR.ClientId = this.userData.DatabaseKey.Client.ClientId;
            AR.ApprovalGroupId = ApprovalGroupId;
            AR.ObjectId = workorderId;
            AR.ProcessResponse = WorkOrderStatusConstants.Denied;
            AR.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
            AR.UpdateByObjectId_V2(userData.DatabaseKey);
            if (workOrder.ErrorMessages == null || workOrder.ErrorMessages.Count == 0)
            {
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                List<long> listWO = new List<long>();
                listWO.Add(workOrder.WorkOrderId);
                objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkRequestDenied, listWO);
                Statusmsg = "success";
            }
            else
            {
                Statusmsg = "error";
            }
            return workOrder;
        }
        #endregion

        #region Purchase Request
        public PurchaseRequest MultiLevelApprovePR(MultiLevelApproveApprovalRouteModel arModel)
        {
            List<long> PRlist = new List<long>();
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = arModel.ObjectId;

            purchaseRequest.Retrieve(userData.DatabaseKey);
            PurchaseRequest pr = new PurchaseRequest();
            pr.ClientId = userData.DatabaseKey.Client.ClientId;
            pr.PurchaseRequestId = arModel.ObjectId;
            pr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.DatabaseKey.Client.DefaultTimeZone);
            pr.Status = PurchaseRequestStatusConstants.AwaitApproval;
            pr.UpdateIndex = purchaseRequest.UpdateIndex;
            //V2-832
            pr.ProcessBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            pr.Processed_Date = System.DateTime.UtcNow;
            pr.Process_Comments = arModel.Comments ?? string.Empty;
            //V2-832
            pr.Update(userData.DatabaseKey);
            ApprovalRoute AR = new ApprovalRoute();
            AR.ClientId = userData.DatabaseKey.Client.ClientId;
            AR.ApprovalGroupId = arModel.ApprovalGroupId;
            AR.ObjectId = arModel.ObjectId;
            AR.ProcessResponse = PurchaseRequestStatusConstants.Approved;
            AR.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
            AR.UpdateByObjectId_V2(userData.DatabaseKey);
            CreateEventLog(arModel.ApproverId, arModel.ObjectId, arModel.Comments, arModel.ApprovalGroupId, arModel.RequestType);
            // Send Notification
            List<long> purchaserequestid = new List<long>() { arModel.ObjectId };
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
                Task t2 = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.PurchaseRequest>(AlertTypeEnum.PurchaseRequestApprovalNeeded, purchaserequestid, UserList));
            }

            return pr;
        }
        public PurchaseRequest MultiLevelFinalApprovePR(long PurchaseRequestId, long ApprovalGroupId)
        {
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            List<long> PRlist = new List<long>();
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = PurchaseRequestId;

            purchaseRequest.Retrieve(userData.DatabaseKey);
            PurchaseRequest pr = new PurchaseRequest();
            pr.ClientId = userData.DatabaseKey.Client.ClientId;
            pr.PurchaseRequestId = PurchaseRequestId;
            pr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.DatabaseKey.Client.DefaultTimeZone);
            pr.Status = PurchaseRequestStatusConstants.Approved;
            pr.Approved_Date = DateTime.UtcNow;
            pr.ApprovedBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            pr.UpdateIndex = purchaseRequest.UpdateIndex;
            pr.Update(userData.DatabaseKey);
            ApprovalRoute AR = new ApprovalRoute();
            AR.ClientId = userData.DatabaseKey.Client.ClientId;
            AR.ApprovalGroupId = ApprovalGroupId;
            AR.ObjectId = PurchaseRequestId;
            AR.ProcessResponse = PurchaseRequestStatusConstants.Approved;
            AR.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
            AR.UpdateByObjectId_V2(userData.DatabaseKey);
            PRlist.Add(pr.PurchaseRequestId);
            objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestApproved, PRlist);

            return pr;
        }


        public PurchaseRequest MultiLevelDenyPR(long PurchaseRequestId, long ApprovalGroupId)
        {
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            List<long> PRlist = new List<long>();
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = PurchaseRequestId;

            purchaseRequest.Retrieve(userData.DatabaseKey);
            var PrevStatus = purchaseRequest.Status;
            purchaseRequest.Status = PurchaseRequestStatusConstants.Denied;
            purchaseRequest.Update(userData.DatabaseKey);
            if (purchaseRequest.ErrorMessages == null || purchaseRequest.ErrorMessages.Count == 0)
            {
                ApprovalRoute AR = new ApprovalRoute();
                AR.ClientId = userData.DatabaseKey.Client.ClientId;
                AR.ApprovalGroupId = ApprovalGroupId;
                AR.ObjectId = PurchaseRequestId;
                AR.ProcessResponse = PurchaseRequestStatusConstants.Denied;
                AR.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
                AR.UpdateByObjectId_V2(userData.DatabaseKey);
                PRlist.Add(purchaseRequest.PurchaseRequestId);
                objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestDenied, PRlist);
            }
            return purchaseRequest;
        }
        public PurchaseRequest ReturnToRequestorPR(long PurchaseRequestId, long ApprovalGroupId,string Comments)
        {
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            List<long> PRlist = new List<long>();
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = PurchaseRequestId;

            purchaseRequest.Retrieve(userData.DatabaseKey);
            var PrevStatus = purchaseRequest.Status;
            purchaseRequest.Status = PurchaseRequestStatusConstants.Resubmit;
            //V2-832
            purchaseRequest.ProcessBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            purchaseRequest.Processed_Date = System.DateTime.UtcNow;
            //
            purchaseRequest.Return_Comments = Comments;
            purchaseRequest.Update(userData.DatabaseKey);
            if (purchaseRequest.ErrorMessages == null || purchaseRequest.ErrorMessages.Count == 0)
            {
                ApprovalRoute AR = new ApprovalRoute();
                AR.ClientId = userData.DatabaseKey.Client.ClientId;
                AR.ApprovalGroupId = ApprovalGroupId;
                AR.ObjectId = PurchaseRequestId;
                AR.ProcessResponse = PurchaseRequestStatusConstants.Resubmit;
                AR.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
                AR.UpdateByObjectId_V2(userData.DatabaseKey);
                PRlist.Add(purchaseRequest.PurchaseRequestId);
                objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestReturned, PRlist);
            }
            return purchaseRequest;
        }
        #endregion

        #region Material Request
        public EstimatedCosts MultiLevelApproveMR(MultiLevelApproveApprovalRouteModel arModel)
        {
            EstimatedCosts ec = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EstimatedCostsId = arModel.ObjectId
            };
            ec.Retrieve(userData.DatabaseKey);

            ec.Status = MaterialRequestLineStatus.Route;
            ec.Update(userData.DatabaseKey);

            ApprovalRoute AR = new ApprovalRoute();
            AR.ClientId = this.userData.DatabaseKey.Client.ClientId;
            AR.ApprovalGroupId = arModel.ApprovalGroupId;
            AR.ObjectId = arModel.ObjectId;
            AR.ProcessResponse = MaterialRequestLineStatus.Approved;
            AR.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
            AR.UpdateByObjectId_V2(userData.DatabaseKey);

            if (AR.ErrorMessages == null || AR.ErrorMessages.Count == 0)
            {
                Task t1 = Task.Factory.StartNew(() => CreateEventLog(arModel.ApproverId, arModel.ObjectId, arModel.Comments, arModel.ApprovalGroupId, arModel.RequestType));
                List<long> workorderid = new List<long>() { ec.EstimatedCostsId };
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
                    Task t2 = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.MaterialRequest>(AlertTypeEnum.MaterialRequestApprovalNeeded, workorderid, UserList));
                }
            }
            return ec;
        }
        public EstimatedCosts MultiLevelFinalApproveMR(long estimatedCostsId, long ApprovalGroupId)
        {
            EstimatedCosts ec = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EstimatedCostsId = estimatedCostsId
            };
            ec.Retrieve(userData.DatabaseKey);
            if (ec.Status == MaterialRequestLineStatus.Route)
            {
                ec.Status = MaterialRequestLineStatus.Approved;
                //ec.ApproveDate = DateTime.UtcNow;
                //ec.ApproveBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                ec.Update(userData.DatabaseKey);

                ApprovalRoute AR = new ApprovalRoute();
                AR.ClientId = this.userData.DatabaseKey.Client.ClientId;
                AR.ApprovalGroupId = ApprovalGroupId;
                AR.ObjectId = estimatedCostsId;
                AR.ProcessResponse = MaterialRequestLineStatus.Approved;
                AR.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
                AR.UpdateByObjectId_V2(userData.DatabaseKey);

                //if (AR.ErrorMessages == null || AR.ErrorMessages.Count == 0)
                //{
                //    //Task t1 = Task.Factory.StartNew(() => CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Approved, "Work Order Approved"));

                //    List<long> listWO = new List<long>();
                //    listWO.Add(ec.EstimatedCostsId);
                //    ProcessAlert objAlert = new ProcessAlert(this.userData);
                //    Task t2 = Task.Factory.StartNew(() => objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkRequestApproved, listWO));
                //}
            }
            return ec;

        }

        public EstimatedCosts MultiLevelDenyMR(long estimatedCostsId, long ApprovalGroupId, ref string Statusmsg)
        {
            EstimatedCosts ec = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EstimatedCostsId = estimatedCostsId
            };
            ec.Retrieve(userData.DatabaseKey);
            string strWOStatus = ec.Status;
            ec.Status = MaterialRequestLineStatus.Denied;
            ec.Update(userData.DatabaseKey);
            ApprovalRoute AR = new ApprovalRoute();
            AR.ClientId = this.userData.DatabaseKey.Client.ClientId;
            AR.ApprovalGroupId = ApprovalGroupId;
            AR.ObjectId = estimatedCostsId;
            AR.ProcessResponse = MaterialRequestLineStatus.Denied;
            AR.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
            AR.UpdateByObjectId_V2(userData.DatabaseKey);
            if (ec.ErrorMessages == null || ec.ErrorMessages.Count == 0)
            {
                //ProcessAlert objAlert = new ProcessAlert(this.userData);
                //List<long> listWO = new List<long>();
                //listWO.Add(workOrder.WorkOrderId);
                //objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkRequestDenied, listWO);
                Statusmsg = "success";
            }
            else
            {
                Statusmsg = "error";
            }
            return ec;
        }
        #endregion
        #endregion
    }
}