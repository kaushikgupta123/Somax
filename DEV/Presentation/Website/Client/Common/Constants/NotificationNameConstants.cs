using Database;

namespace Client.Common
{
    public class NotificationNameConstants
    {
        #region Maintenance      
        internal const string WorkRequestApprovalNeeded = "WorkRequestApprovalNeeded";
        internal const string WorkRequestApproved = "WorkRequestApproved";
        internal const string WorkRequestDenied = "WorkRequestDenied";
        internal const string WorkOrderComplete = "WorkOrderComplete";
        internal const string SensorReadingAlertWO = "SensorReadingAlertWO";
        internal const string WorkOrderCommentMention = "WorkOrderCommentMention";
        internal const string AssetCommentMention = "AssetCommentMention";        
        internal const string WorkOrderAssign = "WorkOrderAssign";
        internal const string WorkOrderApprovalNeeded = "WorkOrderApprovalNeeded";
        internal const string WOPlanCommentMention = "WOPlanCommentMention";
        internal const string ProjectCommentMention = "ProjectCommentMention";
        #region V2-720       
        internal const string WRApprovalRouting = "WRApprovalRouting";
        #endregion
        #region V2-823
        internal const string KPIReOpened = "KPIReOpened";
        internal const string KPISubmitted = "KPISubmitted";
        #endregion
        #endregion
        #region Inventory

        internal const string PartMasterRequestApprovalNeeded = "PartMasterRequestApprovalNeeded";
        internal const string PartMasterRequestApproved = "PartMasterRequestApproved";
        internal const string PartMasterRequestDenied = "PartMasterRequestDenied";
        internal const string PartMasterRequestReturned = "PartMasterRequestReturned";
        internal const string PartMasterRequestProcessed = "PartMasterRequestProcessed";
        internal const string PartMasterRequestSiteApprovalNeeded = "PartMasterRequestSiteApprovalNeeded";
        internal const string PartMasterRequestSiteApproved = "PartMasterRequestSiteApproved";
        internal const string PartMasterRequestSiteApprovalNeededLocal = "PartMasterRequestSiteApprovalNeededLocal";
        internal const string PartMasterRequestAdditionProcessed = "PartMasterRequestAdditionProcessed";
        internal const string PartMasterRequestReplaceProcessed = "PartMasterRequestReplaceProcessed";
        internal const string PartMasterRequestInactivationProcessed = "PartMasterRequestInactivationProcessed";
        internal const string PartMasterRequestSXReplaceProcessed = "PartMasterRequestSXReplaceProcessed";
        internal const string PartMasterRequestECO_ReplaceProcessed = "PartMasterRequestECO_ReplaceProcessed";
        internal const string PartMasterRequestECO_SX_ReplaceProcessed = "PartMasterRequestECO_SX_ReplaceProcessed";
        internal const string PartTransferCreated = "PartTransferCreated";
        internal const string PartTransferIssue = "PartTransferIssue";
        internal const string PartTransferReceipt = "PartTransferReceipt";
        internal const string PartTransferDenied = "PartTransferDenied";
        internal const string PartTransferCanceled = "PartMasterRequestApprovalNeeded";
        internal const string PartTransferForceCompPend = "PartTransferDenied";
        internal const string PartCommentMention = "PartCommentMention";
        internal const string MaterialRequestApprovalNeeded = "MaterialRequestApprovalNeeded";
        #endregion
        #region Procurement
        internal const string PurchaseRequestApprovalNeeded = "PurchaseRequestApprovalNeeded";
        internal const string PurchaseRequestApproved = "PurchaseRequestApproved";
        internal const string PurchaseRequestConverted = "PurchaseRequestConverted";
        internal const string PurchaseOrderReceipt = "PurchaseOrderReceipt";
        internal const string PurchaseRequestDenied = "PurchaseRequestDenied";
        internal const string PurchaseRequestReturned = "PurchaseRequestReturned";       
        internal const string POEmailToVendor = "POEmailToVendor";
        internal const string POImportedReviewRequired = "POImportedReviewRequired";       
        #endregion
        #region System
        internal const string NewUserAdded = "NewUserAdded";
        internal const string ResetPassword = "ResetPassword";
        #endregion
        #region APM
        internal const string APMMeterEvent = "APMMeterEvent";
        internal const string APMWarningEvent = "APMWarningEvent";
        internal const string APMCriticalEvent = "APMCriticalEvent";
        #endregion
        #region V2-1077
        internal const string WorkOrderPlanner = "WorkOrderPlanner";
        #endregion
    }
}