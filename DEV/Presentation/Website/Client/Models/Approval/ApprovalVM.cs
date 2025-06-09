using Client.Models.Work_Order;
using Client.Models.Work_Order.UIConfiguration;
using Client.Models.WorkOrder;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.Approval
{
    public class ApprovalVM : LocalisationBaseVM
    {
        public ApprovalVM()
        {
            FilterTypePRList = new List<SelectListItem>();
            FilterTypeWRList = new List<SelectListItem>();
            FilterTypeMRList = new List<SelectListItem>();
        }
        public bool isPurchaseRequestApproval { get; set; }
        public bool isWorkRequestApproval { get; set; }
        public bool isMaterialRequestApproval { get; set; }
        public int NumberOfPurchaseRequests { get; set; }
        public int NumberOfWorkRequests { get; set; }
        public int NumberOfMaterialRequests { get; set; }
        public List<SelectListItem> FilterTypePRList { get; set; }
        public string FilterTypePR { get; set; }
        public List<SelectListItem> FilterTypeWRList { get; set; }
        public string FilterTypeWR { get; set; }
        public List<SelectListItem> FilterTypeMRList { get; set; }
        public string FilterTypeMR { get; set; }
        public List<Client.Common.UIConfigurationDetailsForModelValidation> UIConfigurationDetails { get; set; }
        public ViewWorkOrderModelDynamic ViewWorkOrderModel { get; set; }
        public WorkOrderModel workOrderModel { get; set; }
        public WoTaskModel woTaskModel { get; set; }
        public IEnumerable<SelectListItem> CancelReasonListWo { get; set; }   
        public List<MaterialRequestModel> materialRequestModelList { get; set; }
        public PurchaseRequest.UIConfiguration.ViewPurchaseRequestModelDynamic ViewPurchaseRequest { get; set; }
        public UserData udata { get; set; }
        public long ApproverId { get; set; }
        public Models.MaterialRequest.MaterialRequestSummaryModel  materialRequestSummary { get; set; }
        public MultiLevelApproveApprovalRouteModel multiLevelApproveApprovalRouteModel { get; set; }
    }
}