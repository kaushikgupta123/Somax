/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014-2022 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID  Person           Description
* =========== ======== ================ ============================================================
* 2014-Nov-20 SOM-446  Roger Lawton     Added WorkOrder-LaborScheduling
*                                            Purchasing_Approve
*                                            Purchasing_Receive
*                                            Purchasing_Void
* 2015-Feb-16 SOM-558  Roger Lawton     Purchase Request Approve not here                                               
* 2015-Apr-10 SOM-643  Roger Lawton     Purchase Request AutoGenerate not here
* 2015-Jun-02 SOM-684  Roger Lawton     Add Purchasing.ForceComplete 
* 2016-Oct-06 SOM-1124 Roger Lawton     Added PurchaseRequest_EditAwaitApprove  
* 2021-Dec-11 V2-622   Roger Lawton     Review and Categorized the Constants
* 2022-Jan-06 V2-      Roger Lawton     Add WO Complete WB and WO Add Project Constants 
*                                       (Supplied by IndusNet)
****************************************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Constants
{
    public class SecurityConstants
    {
        //
        // Module Level Constants
        // Uses all four items (ItemAccess, ItemEdit, ItemCreate, ItemDelete)
        //
        public const string Accounts = "Accounts";
        public const string BBUMaintStats = "BBUMaintStats";                        // SOM-729 -- Not sure if we need
        public const string Dashboards = "Dashboards";                              // SOM-957                                   
        public const string Downtime = "Downtime";                                  // SOM-906                                       
        public const string Equipment = "Equipment";
        public const string EquipmentMaster = "EquipmentMaster";
        public const string Fleet_Assets = "Fleet-Assets";                          //V2-385
        public const string Fleet_Fuel_Tracking = "Fleet-Fuel Tracking";            //V2-391
        public const string Fleet_Issues = "Fleet-Issues";                          //V2-411
        public const string Fleet_Meter_History = "Fleet-Meter History";            //V2-392
        public const string Fleet_Scheduled_Service = "Fleet-Scheduled Service";    //V2-406
        public const string Fleet_Service_Order = "Fleet-Service Order";            //V2-388
        public const string Fleet_Service_Task = "Fleet-Service Tasks";             //V2-405
        public const string InvoiceMatching = "InvoiceMatching";                    // SOM-822                         
        public const string ManufacturerMaster = "ManufacturerMaster";
        public const string MasterSanitation_Library = "MasterSanitation-Library";  // SOM-1605
        public const string Meters = "Meters";
        public const string OnDemand_Library = "OnDemand-Library";                  // SOM-1444
        public const string PartCategoryMaster = "PartCategoryMaster";
        public const string PartMaster = "PartMaster";                              // SOM-1366 // SOM-1044
        public const string PartMasterRequest = "PartMasterRequest";
        public const string Parts = "Parts";
        public const string PartTransfers = "PartTransfers";
        public const string Personnel = "Personnel";                                        //V2-575
        public const string Personnel_Attendance = "Personnel-Attendance";                  //V2-575
        public const string Personnel_Availability = "Personnel-Availability";              //V2-575
        public const string Personnel_Events = "Personnel-Events";                          //V2-575
        public const string Personnel_MasterQuerySetup = "Personnel - Master Query Setup";  //V2-630
        public const string Personnel_Schedule = "Personnel-Schedule";                      //V2-575
        public const string PrevMaint = "PrevMaint";
        public const string PrevMaint_Library = "PrevMaint-Library";                        // SOM-1605
        public const string Project = "Project";                                            //V2-594
        public const string PurchaseRequest = "PurchaseRequest";
        public const string Purchasing = "Purchasing";
        public const string Sanitation = "Sanitation";
        public const string SanitationJob = "SanitationJob";  //Som-1249
        public const string Sensors = "Sensors";
        public const string VendorCatalog = "VendorCatalog";
        public const string VendorMaster = "VendorMaster";
        public const string Vendors = "Vendors";
        public const string WorkOrders = "WorkOrders";
        // Module Level - Not Used in V2
        public const string Locations = "Locations";
        public const string PlantLocations = "PlantLocations";                      //--Som-1259                                        
        public const string PlantSetup = "PlantSetup";                              //--Som-1259        
        public const string Procedures = "Procedures";
        public const string ProcessSetup = "ProcessSetup";                          // SOM-827                               
        public const string ShoppingCart = "ShoppingCart";
        //
        // Functional Level Constants
        // SingleItem = True 
        // Uses ItemAccess
        //
        public const string Access_Enterprise_Maintenance_Dashboard = "Access Enterprise - Maintenance Dashboard";//V2-552
        public const string AccessMaintenanceTechnicianDashboard = "Access Maintenance Technician Dashboard";//V2-610
        public const string AccessAPMDashboard = "Access APM Dashboard";//V2-552
        public const string AccessFleetDashboard = "Access Fleet Dashboard";//V2-552
        public const string AccessMaintenanceDashboard = "Access Maintenance Dashboard";//V2-552
        public const string AccessSanitationDashboard = "Access Sanitation Dashboard";//V2-552
        public const string Asset_Availability = "Asset-Availability";//V2-485
        public const string Asset_ScrapAsset = "Asset-Scrap Asset";//V2-639
        public const string Asset_RepairableSpare = "Asset - Repairable Spare";//V2-637
        public const string Asset_RepairableSpare_Assignment = "Asset - Repairable Spare - Assignment";//V2-637
        public const string AlertProcedures = "AlertProcedures";  // Sensors 
        public const string BBUMaintStats_Extract = "BBUMaintStats-Extract";          // SOM-741 - Not Sure if we need           
        public const string CustomSecurityProfile = "CustomSecurityProfile";//V2-500 - NO ENTRY IN THE SECURITY TABLE
        public const string Equipment_Change_ClientLookupId = "Equipment-ChangeClientLookupId";  // SOM-1677
        public const string Events = "Events";
        public const string Events_Process = "Events-Process";
        public const string Fleet_Record_Fuel_Entry = "Fleet-Record Fuel Entry";//V2-391
        public const string Fleet_Record_Meter_Reading = "Fleet-Record Meter Reading";//V2-392
        public const string ImportData_Equipment = "ImportData-Equipment";
        public const string ImportData_Part = "ImportData-Part";
        public const string InvoiceMatching_InvoicePaid = "InvoiceMatching-InvoicePaid";
        public const string InvoiceMatching_AuthorizeToPay = "Invoice Matching-Authorize To Pay";
        public const string InvoiceMatching_Reopen = "Invoice Matching-Reopen";
        public const string InvoiceMatching_ChangeInvoiceID = "Invoice Matching-Change Invoice ID";
        public const string MaintenanceCompletionWorkbenchWidget_Complete = "Maintenance Completion Workbench Widget - Complete";//V2-610
        public const string MaintenanceCompletionWorkbenchWidget_Cancel = "Maintenance Completion Workbench Widget - Cancel";//V2-610
        public const string MaintenanceCompletionWorkbenchWidget_PartIssue = "Maintenance Completion Workbench Widget - Part Issue";//V2-610
        public const string MaintenanceCompletionWorkbenchWidget_AddUnplannedWO = "Maintenance Completion Workbench Widget - Add Unplanned WO";//V2-621
        public const string MaintenanceCompletionWorkbenchWidget_AddFollowUpWO = "Maintenance Completion Workbench Widget - Add Follow Up WO";//V2-621
        public const string MaintenanceCompletionWorkbenchWidget_AddWorkRequest = "Maintenance Completion Workbench Widget - Add Work Request";//V2-652
        public const string Master_Schedule_Forecast = "Master-Schedule-Forecast";
        public const string Master_Schedule_Search = "Master-Schedule-Search";
        public const string Meter_Reading = "Meter-Reading";
        public const string PartMaster_AssignPart = "PartMaster-AssignPart";
        public const string PartMasterRequest_Review = "PartMasterRequest-Review";
        public const string PartMasterRequest_Approve = "PartMasterRequest-Approve";
        public const string PartMasterRequest_ApproveEnterprise = "PartMasterRequest-ApproveEnterprise";
        public const string Parts_Change_ClientLookupId = "Parts-ChangeClientLookupId";  // SOM-1677
        public const string Parts_Checkout = "Parts-CheckOut";
        public const string Parts_CycleCount = "Parts - Cycle Count"; //V2-650
        public const string Parts_Equipment_Xref = "Parts-Equipment-XRef";            // SOM-1279
        public const string Parts_Multi_Site_Search = "Parts-Multi-Site-Search";
        public const string Parts_Physical = "Parts-Physical";
        public const string Parts_Receipt = "Parts-Receipt";
        public const string Parts_SiteReview = "Parts-SiteReview";
        public const string Parts_Vendor_Xref = "Part-Vendor-XRef";                   // SOM-1279
        public const string PartTransfers_Process = "PartTransfers-Process";
        public const string Personnel_Auxiliary_Information = "Personnel-Auxiliary Information";//V2-575
        public const string PM_Change_ClientLookupId = "PM-ChangeClientLookupId";//V2-294
        public const string PrevMaint_PMForecast = "PrevMaintPMForecast";             // SOM-957                
        public const string PrevMaint_WO_Gen = "PrevMaint-WO-Gen";
        public const string Project_Cancel = "Project-Cancel";                      //V2-594
        public const string Project_Complete = "Project-Complete";                //V2-594
        public const string PurchaseOrder_SendPunchoutPO = "Purchase Order - Send Punch Out PO";//V2-427
        public const string PurchaseRequest_Approve = "PurchaseRequest-Approve";
        public const string PurchaseRequest_AutoGen = "PurchaseRequest-AutoGen";
        public const string PurchaseRequest_AutoGeneration = "PurchaseRequest-AutoGeneration"; //V2-643
        public const string PurchaseRequest_EditAwaitApprove = "PurchaseRequest-EditAwaitApprove";
        public const string PurchaseRequest_EditApproved = "PurchaseRequest-EditApproved"; //V2-454
        public const string PurchaseRequest_UsePunchout = "Purchase Request - Use Punch Out";//V2-427
        public const string Purchasing_Approve = "Purchasing-Approve";
        public const string Purchasing_ForceComplete = "Purchasing-ForceComplete";    // SOM-684    
        public const string Purchasing_Receive = "Purchasing-Receive";
        public const string Purchasing_ReceiveAccess = "Purchasing-ReceiveAccess";
        public const string Purchasing_Void = "Purchasing-Void";
        public const string Sanitation_Job_Gen = "Sanitation-Job-Gen";
        public const string Sanitation_OnDemand = "Sanitation-OnDemand";  //Som-1333
        public const string Sanitation_Verification = "Sanitation-Verification";  //Som-1271
        public const string Sanitation_WB = "Sanitation-WB";
        public const string SanitationJob_CreateRequest = "SanitationJob-CreateRequest";  //Som-1249                                              
        public const string SanitationJob_ApprovalWorkbench = "SanitationJob-ApprovalWorkbench";  //Som-1265
        public const string SensorSearch = "SensorSearch";
        public const string Vendor_ChangeClientLookupID = "Vendor-ChangeClientLookupID";//V2-404
        public const string Vendor_ConfigurePunchout = "Vendor - Configure Punch Out";//V2-427
        public const string WorkOrder_AddProjecttoWorkOrder = "Work Order - Add Project to Work Order";//V2-626
        public const string WorkOrder_Approve = "WorkOrder-Approve";
        public const string WorkOrder_Cancel = "WorkOrder-Cancel"; // SOM-906                       
        public const string WorkOrder_Complete = "WorkOrder-Complete";
        public const string WorkOrder_CompletionWizardConfiguration = "Work Order Completion Wizard Configuration";//V2-634
        public const string WorkOrder_CreateEmergency = "WorkOrder-CreateEmergency";
        public const string WorkOrder_CreateFollowUp = "WorkOrder-CreateFollowUp";
        public const string WorkOrder_CreateWR = "WorkOrder-CreateWR";
        public const string WorkOrder_LaborScheduling = "WorkOrder-LaborScheduling";  //SOM-446   
        public const string WorkOrder_PartIssue = "Work Order - Part Issue";//V2-615
        public const string WorkOrder_Planning = "WorkOrder-Planning";//V2-592
        public const string WorkOrder_PlanningRequired = "WorkOrder-PlanningRequired";//V2-576
        public const string WorkOrder_Scheduling = "WorkOrder-Scheduling";//V2-576
        public const string WorkOrder_SendForApproval = "WorkOrder-SendForApproval";//V2-576
                                                                                    //
                                                                                    // Function Level - Not Used In V2
                                                                                    //
        public const string ShoppingCart_Review = "ShoppingCart-Review";
        public const string ShoppingCart_Approve = "ShoppingCart-Approve";
        public const string ShoppingCart_AutoGen = "ShoppingCart-AutoGen";
        //
        // Report Related Constants 
        // SingleItem = True and Reporting Related
        // Uses ItemAccess
        public const string APM_Reports = "APM Reports";
        public const string Asset_Reports = "Asset Reports";
        public const string Configuration_Reports = "Configuration";
        public const string Create_Private_Reports = "Create Private Reports";//V2-413
        public const string Create_Public_Reports = "Create Public Reports";//V2-413
        public const string Create_Site_Reports = "Create Site Reports";//V2-413
        public const string Enterprise_APM = "Enterprise - APM";//V2-413
        public const string Enterprise_CMMS = "Enterprise - CMMS";//V2-413
        public const string Enterprise_Fleet = "Enterprise - Fleet";//V2-413
        public const string Enterprise_Sanitation = "Enterprise - Sanitation";//V2-413
        public const string Inventory_Reports = "Inventory Reports";
        public const string Preventive_Maintenance_Reports = "Preventive Maintenance Reports";
        public const string Purchasing_Reports = "Purchasing Reports";
        public const string Reports = "Reports"; // SOM-907 
        public const string Reports_Fleet = "Reports-Fleet";//V2-385
        public const string Reports_MakePublic = "Reports-MakePublic";//Som-1193                                          
        public const string Sanitation_Reports = "Sanitation Reports";
        public const string Work_Order_Reports = "Work Order Reports";
        // Report Level - Not Used in V2
        public const string BusinessIntelligence = "BusinessIntelligence";
        public const string Storeroom = "Storeroom";//V2-671
        public const string MaintenanceCompletionWorkbenchWidget_MaterialRequest = "Maintenance Completion Workbench Widget-Material Request";//V2-690
        public const string WorkOrder_MaterialRequest = "Work Order-Material Request";//V2-690
        public const string Parts_MaterialRequest = "Parts-Material Request";//V2-691
        public const string Asset_Downtime = "Asset - Downtime";//V2-695
        public const string WorkOrder_Downtime = "Work Order - Downtime";//V2-695
        public const string MaintenanceCompletionWorkbenchWidget_Downtime = "Maintenance Completion Workbench Widget- Downtime";//V2-695
        public const string MaintenanceCompletionWorkbenchWidget_Photos = "Maintenance Completion Workbench Widget-Photos";//V2-716
        public const string Asset_Photos = "Asset-Photos";//V2-716
        public const string WorkOrder_Photos = "Work Order-Photos";//V2-716
        public const string Parts_Photos = "Parts-Photos";//V2-716
        public const string ApprovalGroupsConfiguration = "Approval Groups Configuration";//V2-720
        public const string MaterialRequest_Approve = "MaterialRequest-Approve";//V2-720
        public const string StoreroomTransfer = "StoreroomTransfer";//V2-671

        public const string WorkOrder_ApprovalPage = "WorkOrder-ApprovalPage"; //V2-730
        public const string PurchaseRequest_ApprovalPage = "PurchaseRequest-ApprovalPage"; //V2-730
        public const string Approval = "Approval"; //V2-759
        public const string PurchaseRequest_Review = "PurchaseRequest-Review"; //V2-820
        public const string BBUKPI_Site = "BBUKPI-Site"; //V2-823
        public const string BBUKPI_Enterprise = "BBUKPI-Enterprise"; //V2-823
        public const string SanitationJob_Photos = "SanitationJob-Photos"; //V2-841
        public const string Parts_UpdatePart_Costs = "Parts-Update Part Costs"; //V2-906
        public const string Vendor_Create_Vendor_Request = "Vendor-Create Vendor Request"; //V2-915
        public const string Vendor_Approve_Vendor_Request = "Vendor-Approve Vendor Request"; //V2-915
        public const string Vendor_Insurance = "Vendor-Insurance"; //V2-929
        public const string Vendor_Insurance_OverrideInsurance = "Vendor-Insurance-OverrideInsurance"; //V2-929
        public const string Vendor_Insurance_RemoveOverride = "Vendor-Insurance-RemoveOverride"; //V2-929
        public const string WorkOrder_FormConfiguration = "Form Configuration";//V2-944
        public const string Vendor_AssetMgt = "Vendor-AssetMgt"; //V2-933
        public const string Vendor_AssetMgt_OverrideAssetMgt = "Vendor-AssetMgt-OverrideAssetMgt"; //V2-933
        public const string Vendor_AssetMgt_RemoveOverride = "Vendor-AssetMgt-RemoveOverride"; //V2-933
        public const string SanitationJob_Approve = "SanitationJob-Approve"; //V2-912
        public const string SanitationJob_Complete = "SanitationJob-Complete"; //V2-912
        public const string PurchaseRequest_Consolidate = "Purchase Request - Consolidate"; //V2-912
        public const string PurchaseOrder_Model  = "Purchase Order - Model"; //V2-1047
        public const string SanitationJob_AddMaintenanceWorkRequest  = "SanitationJob-AddMaintenanceWorkRequest"; //V2-1055
        public const string SanitationJob_Sanitation_Request_From_Maintenance = "SanitationJob - Sanitation Request From Maintenance"; //V2-1056
        public const string WorkOrder_Model = "Work Order - Model"; //V2-1051
        public const string Parts_Issue = "Parts - Issue"; //V2-1031
        public const string PartTransfer_Auto_Transfer_Generation = "PartTransfer - Auto Transfer Generation"; //V2-1059
        public const string PurchaseRequest_MaterialRequestItems = "Purchase Request - Add Material Request Items"; //V2-1063
        public const string ShipToAddress = "Ship To Address";//V2-1086
        public const string PurchaseOrder_SendEDI_PO = "Purchase Order - Send EDI PO";//V2-1079
        public const string Sensors_Record_Reading = "Sensors - Record Reading";//V2-1103
        public const string ConvertToPurchaseOrder = "Convert to Purchase Order";//V2-1186
        public const string Parts_Model = "Parts - Model";//V2-1203
        public const string Asset_Model = "Asset - Model"; //V2-1202
        public const string Analytics_WorkOrderStatus = "Analytics-Work Order Status"; //V2-1177
        public const string PrevMaint_Model = "PrevMaint - Model";//V2-1204
        public const string Parts_ConfigureAutoPurchasing = "Parts - Configure Auto Purchasing"; //V2-1196

    }
}
