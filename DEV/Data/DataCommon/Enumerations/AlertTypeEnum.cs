/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2017 by SOMAX Inc.
* Alert Type Enum
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2017-Feb-11 SOM-801  Roger Lawton       Added PurchaseRequestDenied 
****************************************************************************************************
 */

namespace Common.Enumerations
{

    public enum AlertTypeEnum
    {
        // Implemented Types
        //Purchase Request
        PurchaseRequestApprovalNeeded,
        PurchaseRequestApproved,
        PurchaseRequestConverted,
        PurchaseRequestDenied,
        PurchaseRequestReturned,
        //Sensor
        SensorReadingAlert,//SOM-1351
        SensorReadingAlertWO,
        // Purchase Order
        PurchaseOrderReceipt,
        // PO Import
        POEmailToVendor,
        POImportedReviewRequired,
        // Work Request
        WorkRequestApprovalNeeded,
        WorkRequestApproved,
        WorkRequestDenied,
        // Work Order 
        WorkOrderComplete,
        // Types to go - not yet implemented in updated process alert class
        WorkOrderCancel,
        WorkOrderSchedule,
        WorkOrderScheduleAssignedUser,
        // Meter Generated Work Order 
        MeterWOGenerated,
        MeterWOGenerated_Assigned,
        // Shopping Cart
        ShoppingCartApprovalNeeded,
        ShoppingCartApproved,
        ShoppingCartDenied,
        ShoppingCartReturned,
        // Part Master Requests
        PartMasterRequestApprovalNeeded,
        PartMasterRequestSiteApprovalNeeded,
        PartMasterRequestApproved,
        PartMasterRequestSiteApproved,
        PartMasterRequestDenied,
        PartMasterRequestReturned,
        PartMasterRequestProcessed,
        PartMasterRequestAdditionProcessed,
        PartMasterRequestReplaceProcessed,
        PartMasterRequestInactivationProcessed,
        PartMasterRequestSXReplaceProcessed,
        PartMasterRequestECO_ReplaceProcessed,
        PartMasterRequestECO_SX_ReplaceProcessed,
        ShoppingCart_AutoGeneration,
        PartTransferSend,
        PartTransferIssue,
        PartTransferReceipt,
        PartTransferDenied,
        PartTransferForceCompPend,
        PartTransferCanceled,
        WorkOrderCommentMention,
        AssetCommentMention,
        PartCommentMention,
        WorkOrderAssign,
        NewUserAdded,
        ResetPassword,
        ServiceOrderCommentMention,
        ServiceOrderAssign,
        ServiceOrderCancel,
        ServiceOrderComplete,
        WorkOrderApprovalNeeded,
        WorkOrderAssigned,
        WOPlanCommentMention,//V2-592
        ProjectCommentMention, //V-594
        MaterialRequestApprovalNeeded,//V2-726
        WRApprovalRouting,//V2-726
        KPIReOpened, //V2-823
        KPISubmitted,  //V2-823
        APMMeterEvent,  //V2-538
        APMWarningEvent,  //V2-538
        APMCriticalEvent,  //V2-538
        WorkOrderPlanner  //V2-1077
    }
}
