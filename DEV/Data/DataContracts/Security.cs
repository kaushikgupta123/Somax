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
* 2014-Nov-21 SOM-446  Roger Lawton     Correct issue with purchasing security
* 2015-Feb-08 SOM-536  Roger Lawton     Removed the redundant "Edit" property of Sec_PurchaseRequest
*                                       Edit is one of the properties that are in the base class
* 2014-Feb-16 SOM-558  Roger Lawton     Purchase Request Not correct       
* 2015-Apr-11 SOM-643  Roger Lawton     Changes to purchasing and purchase request  
* 2015-Jun-02 SOM-684  Roger Lawton     Add Purchasing.ForceComplete 
* 2016-Aug-02 SOM-1044 Roger Lawton     Added Meters.MeterReading filling 
* 2016-Oct-06 SOM-1124 Roger Lawton     Added PurchaseRequest.EditAwaitApproval 
* 2017-May-01 SOM-1279 Roger Lawton     Added Part
* 2017-Dec-16 SOM-1532 Roger Lawton     Add Purchase
* 2019-Feb-20 SOM-1655 Roger Lawton     Added PartTransfer - Cleaned up and organized 
* 2020-Nov-03 V2-418   Roger Lawton     Added Support for V2SecurityProfileId
*                                       No change in this method - just uses the V2SecurityProfileId
*                                       that is loaded into the UserData object 
* 2021-Dec-12 V2-622   Roger Lawton     Review and change to put in alpha order  
*                                       Document and separate into "Modules" 
* 2022-Jan-06 V2       Roger Lawton     Add WO Completion WB Items (Supplied by IndusNet                                      
****************************************************************************************************
*/

using Common.Constants;

using System;
using System.Collections.Generic;

namespace DataContracts
{
    [Serializable]
    public class Security
    {
        #region Properties
        #region Module Level 
        //
        // Security Modules 
        //  - Module Level 
        //  - Multiple Item Column Values (ItemAccess, ItemCreate, ItemDelete, ItemEdit) Used
        //
        public Sec_Accounts Accounts { get; set; }
        public Sec_BBUMaintStats BBUMaintStats { get; set; }
        public Sec_Dashboards Dashboards { get; set; }    // Not sure where this is used 
        public Sec_Downtime Downtime { get; set; }
        public Sec_Equipment Equipment { get; set; }
        public Sec_Asset_Repairable_Spare Asset_RepairableSpare { get; set; } //V2-637
        public Sec_EquipmentMaster EquipmentMaster { get; set; }
        public Sec_Fleet_Assets Fleet_Assets { get; set; }
        public Sec_Fleet_Fuel_Tracking Fleet_FuelTracking { get; set; }   // Set as Module but only using Access
        public Sec_Fleet_Issues Fleet_Issues { get; set; }
        public Sec_Fleet_Meter_History Fleet_MeterHistory { get; set; }   // Set as Module but only using Access
        public Sec_Scheduled_Service_Entry Fleet_Scheduled { get; set; }  // Set as Module but only using Access -- V2-406
        public Sec_Fleet_Service_Order Fleet_ServiceOrder { get; set; }   //V2-388
        public Sec_Fleet_Service_Task Fleet_ServiceTask { get; set; }     // Set as Module but only using Access -- V2-406
        public Sec_InvoiceMatching InvoiceMatching { get; set; }
        public Sec_ManufacturerMaster ManufacturerMaster { get; set; }
        public Sec_MasterSanitationLibrary MasterSanitation { get; set; }
        public Sec_Meters Meters { get; set; }
        public Sec_OnDemandLibrary OnDemandLibrary { get; set; }
        public Sec_PartCategoryMaster PartCategoryMaster { get; set; }
        public Sec_PartMaster PartMaster { get; set; }
        public Sec_PartMasterRequest PartMasterRequest { get; set; }
        public Sec_Parts Parts { get; set; }
        public Sec_PartTransfers PartTransfers { get; set; }
        public Sec_Personnel Personnel { get; set; } // V2-575
        public Sec_Personnel_Attendance Personnel_Attendance { get; set; } // V2-575
        public Sec_Personnel_Availability Personnel_Availability { get; set; } // V2-575
        public Sec_Personnel_Events Personnel_Events { get; set; } // V2-575
        public Sec_Personnel_Schedule Personnel_Schedule { get; set; }    // Does not appear to be used yet
        public Sec_PrevMaint PrevMaint { get; set; }
        public Sec_PrevMaintLibrary PrevMaintLibrary { get; set; }
        public Sec_Project Project { get; set; }//V2-594
        public Sec_PurchaseRequest PurchaseRequest { get; set; }
        public Sec_Purchasing Purchasing { get; set; }
        public Sec_Sanitation Sanitation { get; set; }
        public Sec_SanitationJob SanitationJob { get; set; }
        public Sec_Sensor Sensors { get; set; }
        public Sec_VendorCatalog VendorCatalog { get; set; }
        public Sec_VendorMaster VendorMaster { get; set; }
        public Sec_Vendors Vendors { get; set; }
        public Sec_WorkOrder WorkOrders { get; set; }
        // Module Level - Not Used in V2
        public Sec_Locations Locations { get; set; }                      // Not used in V2 - No Locations Module
        public Sec_PlantLocations PlantLocations { get; set; }            // Not used in V2 - No PlantLocations
        public Sec_PlantSetup PlantSetup { get; set; }                    // Not used in V2 - No Plant Setup
        public Sec_Procedures Procedures { get; set; }                    // Not used in V2 - No Plant Setup
        public Sec_ProcessSetup ProcessSetup { get; set; }                // Not used in V2 - No Plant Setup
        public Sec_ShoppingCart ShoppingCart { get; set; }                // Not used in V2 - No Plant Setup

        public Sec_Asset_Downtime Asset_Downtime { get; set; }                            //V2-695
        public Sec_WorkOrder_Downtime WorkOrder_Downtime { get; set; }                         //V2-695
        public Sec_MaintenanceCompletionWorkbenchWidget_Downtime MaintenanceCompletionWorkbenchWidget_Downtime { get; set; }        //V2-695
        public Sec_ApprovalGroupsConfiguration ApprovalGroupsConfiguration { get; set; } //720
        public Sec_MaterialRequest_Approve MaterialRequest_Approve { get; set; }   //720
        public Sec_PurchaseRequest_Review PurchaseRequest_Review { get; set; }   //820
        public Sec_BBUKPI_Site BBUKPI_Site { get; set; }   //823
        public Sec_BBUKPI_Enterprise BBUKPI_Enterprise { get; set; }   //823
        public Sec_SanitationJob_Photos SanitationJob_Photos { get; set; }   //841
        public Sec_Parts_UpdatePart_Costs Parts_UpdatePart_Costs { get; set; }   //906
        public Sec_Vendor_Create_Vendor_Request Vendor_Create_Vendor_Request { get; set; } //915
        public Sec_Vendor_Approve_Vendor_Request Vendor_Approve_Vendor_Request { get; set; } //915
        public Sec_Vendor_Insurance Vendor_Insurance { get; set; } //929

        public Sec_Vendor_AssetMgt Vendor_AssetMgt { get; set; } //933
        #endregion Module Level 
        #region Functional Level
        //
        // Security Modules 
        //  - Function/Single Item Only Level 
        //  - Separate Modules with only one Item Column Value (ItemAccess) Used
        //
        public Sec_Access_Enterprise_Maintenance_Dashboard Access_Enterprise_Maintenance_Dashboard { get; set; } //V2-552
        public Sec_AccessAPMDashboard AccessAPMDashboard { get; set; } //V2-552
        public Sec_AccessFleetDashboard AccessFleetDashboard { get; set; } //V2-552
        public Sec_AccessMaintenanceDashboard AccessMaintenanceDashboard { get; set; } //V2-552
        public Sec_AccessMaintenanceTechnicianDashboard AccessMaintenanceTechnicianDashboard { get; set; }//V2-610
        public Sec_AccessSanitationDashboard AccessSanitationDashboard { get; set; } //V2-552
        public Sec_Asset_Availability Asset_Availability { get; set; }
        public Sec_EventInfo EventInfo { get; set; }
        public Sec_Fleet_Record_Fuel_Entry Fleet_RecordFuelEntry { get; set; }
        public Sec_Fleet_Record_Meter_Reading Fleet_RecordMeterReading { get; set; }
        public Sec_ImportData ImportData { get; set; }
        public Sec_MaintenanceCompletionWorkbenchWidget_AddFollowUpWO MaintenanceCompletionWorkbenchWidget_AddFollowUpWO { get; set; }//V2-610
        public Sec_MaintenanceCompletionWorkbenchWidget_AddUnplannedWO MaintenanceCompletionWorkbenchWidget_AddUnplannedWO { get; set; }//V2-610
        public Sec_MaintenanceCompletionWorkbenchWidget_AddWorkRequest MaintenanceCompletionWorkbenchWidget_AddWorkRequest { get; set; } //V2-652
        public Sec_MaintenanceCompletionWorkbenchWidget_Cancel MaintenanceCompletionWorkbenchWidget_Cancel { get; set; }//V2-610
        public Sec_MaintenanceCompletionWorkbenchWidget_Complete MaintenanceCompletionWorkbenchWidget_Complete { get; set; }//V2-610
        public Sec_MaintenanceCompletionWorkbenchWidget_PartIssue MaintenanceCompletionWorkbenchWidget_PartIssue { get; set; }//V2-610
        public Sec_Personnel_Auxiliary_Information Personnel_Auxiliary_Information { get; set; } // V2-575
        public Sec_MaintenanceCompletionWorkbenchWidget_MaterialRequest MaintenanceCompletionWorkbenchWidget_MaterialRequest { get; set; } // V2-690
        public Sec_WorkOrder_MaterialRequest WorkOrder_MaterialRequest { get; set; } // V2-690
        public Sec_MaintenanceCompletionWorkbenchWidget_Photos MaintenanceCompletionWorkbenchWidget_Photos { get; set; } // V2-716
        public Sec_Asset_Photos Asset_Photos { get; set; } // V2-716
        public Sec_WorkOrder_Photos WorkOrder_Photos { get; set; } // V2-716
        public Sec_Parts_Photos Parts_Photos { get; set; } // V2-716
        public Sec_Vendor_Insurance_OverrideInsurance Vendor_Insurance_OverrideInsurance { get; set; } // V2-929
        public Sec_Vendor_Insurance_RemoveOverride Vendor_Insurance_RemoveOverride { get; set; } // V2-929
        public Sec_WorkOrder_FormConfiguration WorkOrder_FormConfiguration { get; set; } // V2-944
        public Sec_Vendor_AssetMgt_OverrideAssetMgt Vendor_AssetMgt_OverrideAssetMgt { get; set; } // V2-933
        public Sec_Vendor_AssetMgt_RemoveOverride Vendor_AssetMgt_RemoveOverride { get; set; } // V2-933

        public Sec_SanitationJob_Approve SanitationJob_Approve { get; set; } // V2-912
        public Sec_SanitationJob_Complete SanitationJob_Complete { get; set; } // V2-912
        public Sec_PurchaseOrder_Model PurchaseOrder_Model { get; set; } // V2-1047

        public Sec_SanitationJob_AddMaintenanceWorkRequest SanitationJob_AddMaintenanceWorkRequest { get; set; } // V2-1055
        public Sec_SanitationJob_Sanitation_Request_From_Maintenance SanitationJob_Sanitation_Request_From_Maintenance { get; set; } // V2-1056

        public Sec_Parts_Issue Parts_Issue { get; set; } // V2-1031
        public Sec_ShipToAddress ShipToAddress { get; set; } //V2-1086

        public Sec_PartTransfer_Auto_Transfer_Generation PartTransfer_Auto_Transfer_Generation { get; set; } // V2-1031
        public Sec_Convert_To_PurchaseOrder Convert_To_PurchaseOrder { get; set; } // V2-1186

        public Sec_Analytics Analytics { get; set; } //V2-1177
        //
        // Functional Level - Not Used in V2
        //
        #endregion Functional Level
        #region Report Level 
        // Security Modules 
        //  - Report Item Only Level 
        //  - Separate Modules with only one Item Column Value (ItemAccess) Used
        //
        public Sec_APM_Reports APM_Reports { get; set; }
        public Sec_Asset_Reports Asset_Reports { get; set; }
        public Sec_Configuration_Reports Configuration_Reports { get; set; }
        public Sec_Create_Private_Reports Create_Private_Reports { get; set; } //V2-413
        public Sec_Create_Public_Reports Create_Public_Reports { get; set; } //V2-413
        public Sec_Create_Site_Reports Create_Site_Reports { get; set; } //V2-413
        public Sec_Enterprise_CMMS Enterprise_CMMS { get; set; } //V2-413
        public Sec_Enterprise_Sanitation Enterprise_Sanitation { get; set; } //V2-413
        public Sec_Enterprise_APM Enterprise_APM { get; set; } //V2-413
        public Sec_Enterprise_Fleet Enterprise_Fleet { get; set; } //V2-413
        public Sec_Inventory_Reports Inventory_Reports { get; set; }
        public Sec_Preventive_Maintenance_Reports Preventive_Maintenance_Reports { get; set; }
        public Sec_Purchasing_Reports Purchasing_Reports { get; set; }
        public Sec_Reports Reports { get; set; }
        public Sec_Reports_Fleet Reports_Fleet { get; set; }
        public Sec_Sanitation_Reports Sanitation_Reports { get; set; }
        public Sec_Work_Order_Reports Work_Order_Reports { get; set; }
        //
        // Report Level - Not used in V2
        //
        public Sec_BusinessIntelligence BusinessIntelligence { get; set; }
        #endregion Report Level 
        #endregion Properties

        #region Constructors
        // Default Constructor
        public Security(UserData userdata)
        {
            List<DataContracts.SecurityItem> items = new List<DataContracts.SecurityItem>();
            DataContracts.SecurityItem securityitem = new DataContracts.SecurityItem()
            {
                ClientId = userdata.DatabaseKey.Client.ClientId,
                SecurityProfileId = userdata.DatabaseKey.User.SecurityProfileId,    // V2SecurityProfileId
                //AccessingClientId = userdata.DatabaseKey.Client.ClientId,
                AccessingClientId = 0,                                              // V2-418
            };
            items = securityitem.RetrieveAllByClientAndSecurityProfile(userdata.DatabaseKey);
            // Module Level
            this.Accounts = new Sec_Accounts();
            this.BBUMaintStats = new Sec_BBUMaintStats();
            this.Dashboards = new Sec_Dashboards();                         // Not sure where this is used????
            this.Downtime = new Sec_Downtime();
            this.Equipment = new Sec_Equipment();
            this.Asset_RepairableSpare = new Sec_Asset_Repairable_Spare(); //V2-637
            this.EquipmentMaster = new DataContracts.Sec_EquipmentMaster();
            this.Fleet_Assets = new Sec_Fleet_Assets();
            this.Fleet_FuelTracking = new Sec_Fleet_Fuel_Tracking();
            this.Fleet_Issues = new Sec_Fleet_Issues();
            this.Fleet_MeterHistory = new Sec_Fleet_Meter_History();
            this.Fleet_Scheduled = new Sec_Scheduled_Service_Entry();   //V2-406
            this.Fleet_ServiceOrder = new Sec_Fleet_Service_Order();
            this.Fleet_ServiceTask = new Sec_Fleet_Service_Task();
            this.InvoiceMatching = new Sec_InvoiceMatching();
            this.ManufacturerMaster = new Sec_ManufacturerMaster();
            this.MasterSanitation = new Sec_MasterSanitationLibrary();
            this.Meters = new Sec_Meters();
            this.OnDemandLibrary = new Sec_OnDemandLibrary();
            this.PartCategoryMaster = new Sec_PartCategoryMaster();
            this.PartMaster = new Sec_PartMaster();//SOM-1366
            this.PartMasterRequest = new Sec_PartMasterRequest();
            this.Parts = new Sec_Parts();
            this.PartTransfers = new Sec_PartTransfers();
            this.Personnel = new Sec_Personnel();  //V2  575
            this.Personnel_Attendance = new Sec_Personnel_Attendance();  //V2  575
            this.Personnel_Availability = new Sec_Personnel_Availability(); //V2 575
            this.Personnel_Events = new Sec_Personnel_Events();  //V2  575
            this.Personnel_Schedule = new Sec_Personnel_Schedule();  //V2  575
            this.PrevMaint = new Sec_PrevMaint();
            this.PrevMaintLibrary = new Sec_PrevMaintLibrary();
            this.Project = new Sec_Project(); //V2-594
            this.PurchaseRequest = new Sec_PurchaseRequest();
            this.Purchasing = new Sec_Purchasing();
            this.Sanitation = new Sec_Sanitation();
            this.SanitationJob = new Sec_SanitationJob();
            this.Sensors = new Sec_Sensor();
            this.VendorCatalog = new Sec_VendorCatalog();
            this.VendorMaster = new Sec_VendorMaster();
            this.Vendors = new Sec_Vendors();
            this.WorkOrders = new Sec_WorkOrder();
            // Module - not used in V2
            this.Locations = new Sec_Locations();
            this.PlantLocations = new Sec_PlantLocations();
            this.PlantSetup = new Sec_PlantSetup();
            this.Procedures = new Sec_Procedures();
            this.ProcessSetup = new Sec_ProcessSetup();
            this.ShoppingCart = new Sec_ShoppingCart();
            this.Asset_Downtime = new Sec_Asset_Downtime();  //V2-695
            this.WorkOrder_Downtime = new Sec_WorkOrder_Downtime();  //V2-695
            this.MaintenanceCompletionWorkbenchWidget_Downtime = new Sec_MaintenanceCompletionWorkbenchWidget_Downtime();  //V2-695
            //
            // Functional Level Modules - Separate Module - Only Use Item Access
            //
            this.Access_Enterprise_Maintenance_Dashboard = new Sec_Access_Enterprise_Maintenance_Dashboard();//V2-552
            this.AccessAPMDashboard = new Sec_AccessAPMDashboard();//V2-552
            this.AccessFleetDashboard = new Sec_AccessFleetDashboard();//V2-552
            this.AccessMaintenanceDashboard = new Sec_AccessMaintenanceDashboard(); //V2-552
            this.AccessMaintenanceTechnicianDashboard = new Sec_AccessMaintenanceTechnicianDashboard(); //V2-610
            this.AccessSanitationDashboard = new Sec_AccessSanitationDashboard(); //V2-552
            this.Asset_Availability = new Sec_Asset_Availability();
            this.EventInfo = new DataContracts.Sec_EventInfo();
            this.Fleet_RecordFuelEntry = new Sec_Fleet_Record_Fuel_Entry();
            this.Fleet_RecordMeterReading = new Sec_Fleet_Record_Meter_Reading();
            this.ImportData = new DataContracts.Sec_ImportData();
            this.MaintenanceCompletionWorkbenchWidget_AddUnplannedWO = new Sec_MaintenanceCompletionWorkbenchWidget_AddUnplannedWO(); //V2-621
            this.MaintenanceCompletionWorkbenchWidget_AddFollowUpWO = new Sec_MaintenanceCompletionWorkbenchWidget_AddFollowUpWO(); //V2-621
            this.MaintenanceCompletionWorkbenchWidget_AddWorkRequest = new Sec_MaintenanceCompletionWorkbenchWidget_AddWorkRequest(); //V2-652
            this.MaintenanceCompletionWorkbenchWidget_Complete = new Sec_MaintenanceCompletionWorkbenchWidget_Complete(); //V2-610
            this.MaintenanceCompletionWorkbenchWidget_Cancel = new Sec_MaintenanceCompletionWorkbenchWidget_Cancel(); //V2-610
            this.MaintenanceCompletionWorkbenchWidget_PartIssue = new Sec_MaintenanceCompletionWorkbenchWidget_PartIssue(); //V2-610
            this.Personnel_Auxiliary_Information = new Sec_Personnel_Auxiliary_Information();  //V2  575
            // Function Level - Not Used in V2
            this.BusinessIntelligence = new DataContracts.Sec_BusinessIntelligence();
            // Report Level Modules - Separate Module - Only Use Item Access 
            this.APM_Reports = new Sec_APM_Reports();
            this.Asset_Reports = new Sec_Asset_Reports();
            this.Configuration_Reports = new Sec_Configuration_Reports();
            this.Create_Private_Reports = new Sec_Create_Private_Reports();
            this.Create_Public_Reports = new Sec_Create_Public_Reports();
            this.Create_Site_Reports = new Sec_Create_Site_Reports();
            this.Enterprise_APM = new Sec_Enterprise_APM();
            this.Enterprise_CMMS = new Sec_Enterprise_CMMS();
            this.Enterprise_Fleet = new Sec_Enterprise_Fleet();
            this.Enterprise_Sanitation = new Sec_Enterprise_Sanitation();
            this.Inventory_Reports = new Sec_Inventory_Reports();
            this.Preventive_Maintenance_Reports = new Sec_Preventive_Maintenance_Reports();
            this.Purchasing_Reports = new Sec_Purchasing_Reports();
            this.Reports = new Sec_Reports();
            this.Reports_Fleet = new Sec_Reports_Fleet();
            this.Sanitation_Reports = new Sec_Sanitation_Reports();
            this.Work_Order_Reports = new Sec_Work_Order_Reports();
            this.MaintenanceCompletionWorkbenchWidget_MaterialRequest = new Sec_MaintenanceCompletionWorkbenchWidget_MaterialRequest();
            this.WorkOrder_MaterialRequest = new Sec_WorkOrder_MaterialRequest();
            this.MaintenanceCompletionWorkbenchWidget_Photos = new Sec_MaintenanceCompletionWorkbenchWidget_Photos();
            this.Asset_Photos = new Sec_Asset_Photos();
            this.WorkOrder_Photos = new Sec_WorkOrder_Photos();
            this.Parts_Photos = new Sec_Parts_Photos();
            this.ApprovalGroupsConfiguration = new Sec_ApprovalGroupsConfiguration(); //720
            this.MaterialRequest_Approve = new Sec_MaterialRequest_Approve(); //720
            this.PurchaseRequest_Review = new Sec_PurchaseRequest_Review(); //820
            this.BBUKPI_Site = new Sec_BBUKPI_Site(); //823
            this.BBUKPI_Enterprise = new Sec_BBUKPI_Enterprise(); //823
            this.SanitationJob_Photos = new Sec_SanitationJob_Photos(); //841
            this.Parts_UpdatePart_Costs = new Sec_Parts_UpdatePart_Costs(); //906
            this.Vendor_Create_Vendor_Request = new Sec_Vendor_Create_Vendor_Request(); //915
            this.Vendor_Approve_Vendor_Request = new Sec_Vendor_Approve_Vendor_Request(); //915
            this.Vendor_Insurance = new Sec_Vendor_Insurance(); //929
            this.Vendor_Insurance_OverrideInsurance = new Sec_Vendor_Insurance_OverrideInsurance(); //929
            this.Vendor_Insurance_RemoveOverride = new Sec_Vendor_Insurance_RemoveOverride(); //929
            this.WorkOrder_FormConfiguration = new Sec_WorkOrder_FormConfiguration(); //944
            this.Vendor_AssetMgt = new Sec_Vendor_AssetMgt(); //933
            this.Vendor_AssetMgt_OverrideAssetMgt = new Sec_Vendor_AssetMgt_OverrideAssetMgt(); //933
            this.Vendor_AssetMgt_RemoveOverride = new Sec_Vendor_AssetMgt_RemoveOverride(); //933
            this.SanitationJob_Complete = new Sec_SanitationJob_Complete(); //V2-912
            this.SanitationJob_Approve = new Sec_SanitationJob_Approve(); //V2-912
            this.PurchaseOrder_Model = new Sec_PurchaseOrder_Model(); //V2-1047
            this.SanitationJob_AddMaintenanceWorkRequest = new Sec_SanitationJob_AddMaintenanceWorkRequest(); //V2-1055
            this.SanitationJob_Sanitation_Request_From_Maintenance = new Sec_SanitationJob_Sanitation_Request_From_Maintenance(); //V2-1056
            this.Parts_Issue = new Sec_Parts_Issue(); //V2-1031
            this.ShipToAddress = new Sec_ShipToAddress();//V2-1086
            this.PartTransfer_Auto_Transfer_Generation = new Sec_PartTransfer_Auto_Transfer_Generation(); //V2-1059
            this.Convert_To_PurchaseOrder = new Sec_Convert_To_PurchaseOrder(); //V2-1186
            this.Analytics = new Sec_Analytics(); //V2-1177
            // Report Level - Not Used in V2
            // No Modules Found
            // Load the items from the data
            foreach (SecurityItem item in items)
            {
                string type = item.ItemName;
                #region switch 
                switch (type)
                {
                    //
                    // Module Level
                    //
                    // Accounts
                    case SecurityConstants.Accounts:
                        this.Accounts.Access = item.ItemAccess;
                        this.Accounts.Create = item.ItemCreate;
                        this.Accounts.Delete = item.ItemDelete;
                        this.Accounts.Edit = item.ItemEdit;
                        break;
                    // Dashboards
                    case SecurityConstants.Dashboards:
                        this.Dashboards.Access = item.ItemAccess;
                        this.Dashboards.Create = item.ItemCreate;
                        this.Dashboards.Delete = item.ItemDelete;
                        this.Dashboards.Edit = item.ItemEdit;
                        break;
                    // Downtime
                    case SecurityConstants.Downtime:
                        this.Downtime.Access = item.ItemAccess;
                        this.Downtime.Create = item.ItemCreate;
                        this.Downtime.Delete = item.ItemDelete;
                        this.Downtime.Edit = item.ItemEdit;
                        break;
                    // Equipment
                    case SecurityConstants.Equipment:
                        this.Equipment.Access = item.ItemAccess;
                        this.Equipment.Create = item.ItemCreate;
                        this.Equipment.Delete = item.ItemDelete;
                        this.Equipment.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.Equipment_Change_ClientLookupId:
                        this.Equipment.ChangeClientLookupId = item.ItemAccess;
                        break;
                    case SecurityConstants.Asset_ScrapAsset:
                        this.Equipment.Asset_ScrapAsset = item.ItemAccess;
                        break;
                    // Asset Repairable Spares
                    case SecurityConstants.Asset_RepairableSpare:
                        this.Asset_RepairableSpare.ItemName = item.ItemName;
                        this.Asset_RepairableSpare.Create = item.ItemCreate;
                        this.Asset_RepairableSpare.Delete = item.ItemDelete;
                        this.Asset_RepairableSpare.Edit = item.ItemEdit;
                        this.Asset_RepairableSpare.Access = item.ItemAccess;
                        break;
                    //V2-637
                    case SecurityConstants.Asset_RepairableSpare_Assignment:
                        this.Asset_RepairableSpare.Assignment = item.ItemAccess;
                        break;
                    // EquipmentMaster
                    case SecurityConstants.EquipmentMaster:
                        this.EquipmentMaster.Access = item.ItemAccess;
                        this.EquipmentMaster.Create = item.ItemCreate;
                        this.EquipmentMaster.Delete = item.ItemDelete;
                        this.EquipmentMaster.Edit = item.ItemEdit;
                        break;
                    // Fleet_Assets
                    case SecurityConstants.Fleet_Assets:
                        this.Fleet_Assets.Access = item.ItemAccess;
                        this.Fleet_Assets.Create = item.ItemCreate;
                        this.Fleet_Assets.Delete = item.ItemDelete;
                        this.Fleet_Assets.Edit = item.ItemEdit;
                        break;
                    // Fleet Fuel Tracking is set up as a module but only the Access is used
                    case SecurityConstants.Fleet_Fuel_Tracking:
                        this.Fleet_FuelTracking.Access = item.ItemAccess;
                        this.Fleet_FuelTracking.Create = item.ItemCreate;
                        this.Fleet_FuelTracking.Delete = item.ItemDelete;
                        this.Fleet_FuelTracking.Edit = item.ItemEdit;
                        break;
                    // Fleet Issues
                    case SecurityConstants.Fleet_Issues:
                        this.Fleet_Issues.Access = item.ItemAccess;
                        this.Fleet_Issues.Create = item.ItemCreate;
                        this.Fleet_Issues.Delete = item.ItemDelete;
                        this.Fleet_Issues.Edit = item.ItemEdit;
                        break;
                    // Fleet Meter History is set up as a module but only the Access is used
                    case SecurityConstants.Fleet_Meter_History:
                        this.Fleet_MeterHistory.Access = item.ItemAccess;
                        this.Fleet_MeterHistory.Create = item.ItemCreate;
                        this.Fleet_MeterHistory.Delete = item.ItemDelete;
                        this.Fleet_MeterHistory.Edit = item.ItemEdit;
                        break;
                    // Fleet_Scheduled
                    case SecurityConstants.Fleet_Scheduled_Service:
                        this.Fleet_Scheduled.Access = item.ItemAccess;
                        this.Fleet_Scheduled.Create = item.ItemCreate;
                        this.Fleet_Scheduled.Delete = item.ItemDelete;
                        this.Fleet_Scheduled.Edit = item.ItemEdit;
                        break;
                    // Fleet_ServiceOrder
                    case SecurityConstants.Fleet_Service_Order:
                        this.Fleet_ServiceOrder.Access = item.ItemAccess;
                        this.Fleet_ServiceOrder.Create = item.ItemCreate;
                        this.Fleet_ServiceOrder.Delete = item.ItemDelete;
                        this.Fleet_ServiceOrder.Edit = item.ItemEdit;
                        break;
                    // Fleet_ServiceTask
                    case SecurityConstants.Fleet_Service_Task:
                        this.Fleet_ServiceTask.Access = item.ItemAccess;
                        this.Fleet_ServiceTask.Create = item.ItemCreate;
                        this.Fleet_ServiceTask.Delete = item.ItemDelete;
                        this.Fleet_ServiceTask.Edit = item.ItemEdit;
                        break;
                    // InvoiceMatching
                    case SecurityConstants.InvoiceMatching:
                        this.InvoiceMatching.Access = item.ItemAccess;
                        this.InvoiceMatching.Create = item.ItemCreate;
                        this.InvoiceMatching.Delete = item.ItemDelete;
                        this.InvoiceMatching.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.InvoiceMatching_AuthorizeToPay:
                        this.InvoiceMatching.AuthorizeToPay = item.ItemAccess;
                        break;
                    case SecurityConstants.InvoiceMatching_ChangeInvoiceID:
                        this.InvoiceMatching.ChangeInvoiceId = item.ItemAccess;
                        break;
                    case SecurityConstants.InvoiceMatching_InvoicePaid:
                        this.InvoiceMatching.InvoicePaid = item.ItemAccess;
                        break;
                    case SecurityConstants.InvoiceMatching_Reopen:
                        this.InvoiceMatching.ReOpen = item.ItemAccess;
                        break;
                    // ManufacturerMaster
                    case SecurityConstants.ManufacturerMaster:
                        this.ManufacturerMaster.Access = item.ItemAccess;
                        this.ManufacturerMaster.Create = item.ItemCreate;
                        this.ManufacturerMaster.Delete = item.ItemDelete;
                        this.ManufacturerMaster.Edit = item.ItemEdit;
                        break;
                    // MasterSanitation
                    case SecurityConstants.MasterSanitation_Library:
                        this.MasterSanitation.Access = item.ItemAccess;
                        this.MasterSanitation.Create = item.ItemCreate;
                        this.MasterSanitation.Delete = item.ItemDelete;
                        this.MasterSanitation.Edit = item.ItemEdit;
                        break;
                    // Meters
                    case SecurityConstants.Meters:
                        this.Meters.Access = item.ItemAccess;
                        this.Meters.Create = item.ItemCreate;
                        this.Meters.Delete = item.ItemDelete;
                        this.Meters.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.Meter_Reading:
                        this.Meters.RecordReading = item.ItemAccess;
                        break;
                    // OnDemandLibrary
                    case SecurityConstants.OnDemand_Library:
                        this.OnDemandLibrary.Access = item.ItemAccess;
                        this.OnDemandLibrary.Create = item.ItemCreate;
                        this.OnDemandLibrary.Delete = item.ItemDelete;
                        this.OnDemandLibrary.Edit = item.ItemEdit;
                        break;
                    // PartCategoryMaster
                    case SecurityConstants.PartCategoryMaster:
                        this.PartCategoryMaster.Access = item.ItemAccess;
                        this.PartCategoryMaster.Create = item.ItemCreate;
                        this.PartCategoryMaster.Delete = item.ItemDelete;
                        this.PartCategoryMaster.Edit = item.ItemEdit;
                        break;
                    // PartMaster
                    case SecurityConstants.PartMaster:
                        this.PartMaster.Access = item.ItemAccess;
                        this.PartMaster.Create = item.ItemCreate;
                        this.PartMaster.Delete = item.ItemDelete;
                        this.PartMaster.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.PartMaster_AssignPart:
                        this.PartMaster.AssignPart = item.ItemAccess;
                        break;
                    // PartMasterRequest
                    case SecurityConstants.PartMasterRequest:
                        this.PartMasterRequest.Access = item.ItemAccess;
                        this.PartMasterRequest.Create = item.ItemCreate;
                        this.PartMasterRequest.Delete = item.ItemDelete;
                        this.PartMasterRequest.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.PartMasterRequest_Approve:
                        this.PartMasterRequest.Approve = item.ItemAccess;
                        break;
                    case SecurityConstants.PartMasterRequest_ApproveEnterprise:
                        this.PartMasterRequest.ApproveEnterprise = item.ItemAccess;
                        break;
                    case SecurityConstants.PartMasterRequest_Review:
                        this.PartMasterRequest.Review = item.ItemAccess;
                        break;
                    // Parts
                    case SecurityConstants.Parts:
                        this.Parts.Access = item.ItemAccess;
                        this.Parts.Create = item.ItemCreate;
                        this.Parts.Delete = item.ItemDelete;
                        this.Parts.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.Parts_Change_ClientLookupId:
                        this.Parts.ChangeClientLookupId = item.ItemAccess;
                        break;
                    case SecurityConstants.Parts_Checkout:
                        this.Parts.Checkout = item.ItemAccess;
                        break;
                    case SecurityConstants.Parts_CycleCount:
                        this.Parts.CycleCount = item.ItemAccess;
                        break;
                    case SecurityConstants.Parts_Multi_Site_Search:
                        this.Parts.MultiSiteSearch = item.ItemAccess;
                        break;
                    case SecurityConstants.Parts_Equipment_Xref:
                        this.Parts.Part_Equipment_XRef = item.ItemAccess;
                        break;
                    case SecurityConstants.Parts_Vendor_Xref:
                        this.Parts.Part_Vendor_XRef = item.ItemAccess;
                        break;
                    case SecurityConstants.Parts_Physical:
                        this.Parts.Physical = item.ItemAccess;
                        break;
                    case SecurityConstants.Parts_Receipt:
                        this.Parts.Receipt = item.ItemAccess;
                        break;
                    case SecurityConstants.Parts_SiteReview:
                        this.Parts.SiteReview = item.ItemAccess;
                        break;
                    // PartTransfers
                    case SecurityConstants.PartTransfers:
                        this.PartTransfers.Access = item.ItemAccess;
                        this.PartTransfers.Create = item.ItemCreate;
                        this.PartTransfers.Delete = item.ItemDelete;
                        this.PartTransfers.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.PartTransfers_Process:
                        this.PartTransfers.PartTransfers_Process = item.ItemAccess;
                        break;
                    // Personnel 
                    case SecurityConstants.Personnel:
                        this.Personnel.ItemName = item.ItemName;
                        this.Personnel.Access = item.ItemAccess;
                        this.Personnel.Create = item.ItemCreate;
                        this.Personnel.Delete = item.ItemDelete;
                        this.Personnel.Edit = item.ItemEdit;
                        break;
                    // Person
                    case SecurityConstants.Personnel_MasterQuerySetup:
                        this.Personnel.MasterQuerySetup = item.ItemAccess;
                        break;
                    // Personnel_Attendance
                    case SecurityConstants.Personnel_Attendance:
                        this.Personnel_Attendance.ItemName = item.ItemName;
                        this.Personnel_Attendance.Access = item.ItemAccess;
                        this.Personnel_Attendance.Create = item.ItemCreate;
                        this.Personnel_Attendance.Delete = item.ItemDelete;
                        this.Personnel_Attendance.Edit = item.ItemEdit;
                        break;
                    // Personnel_Availability
                    case SecurityConstants.Personnel_Availability:
                        this.Personnel_Availability.ItemName = item.ItemName;
                        this.Personnel_Availability.Create = item.ItemCreate;
                        this.Personnel_Availability.Delete = item.ItemDelete;
                        this.Personnel_Availability.Edit = item.ItemEdit;
                        this.Personnel_Availability.Access = item.ItemAccess;
                        break;
                    // Personnel_Events
                    case SecurityConstants.Personnel_Events:
                        this.Personnel_Events.ItemName = item.ItemName;
                        this.Personnel_Events.Access = item.ItemAccess;
                        this.Personnel_Events.Create = item.ItemCreate;
                        this.Personnel_Events.Delete = item.ItemDelete;
                        this.Personnel_Events.Edit = item.ItemEdit;
                        break;
                    // Personnel_Schedule
                    case SecurityConstants.Personnel_Schedule: // Not actually used yet
                        this.Personnel_Schedule.ItemName = item.ItemName;
                        this.Personnel_Schedule.Access = item.ItemAccess;
                        this.Personnel_Schedule.Create = item.ItemCreate;
                        this.Personnel_Schedule.Delete = item.ItemDelete;
                        this.Personnel_Schedule.Edit = item.ItemEdit;
                        break;
                    // PrevMaint
                    case SecurityConstants.PrevMaint:
                        this.PrevMaint.Access = item.ItemAccess;
                        this.PrevMaint.Create = item.ItemCreate;
                        this.PrevMaint.Delete = item.ItemDelete;
                        this.PrevMaint.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.PM_Change_ClientLookupId:
                        this.PrevMaint.ChangeClientLookupId = item.ItemAccess;
                        break;
                    case SecurityConstants.PrevMaint_WO_Gen:
                        this.PrevMaint.Generate_WorkOrders = item.ItemAccess;
                        break;
                    case SecurityConstants.PrevMaint_PMForecast:
                        this.PrevMaint.PrevMaintPMForecast = item.ItemAccess;
                        break;
                    // PrevMaint_Library 
                    case SecurityConstants.PrevMaint_Library:
                        this.PrevMaintLibrary.Access = item.ItemAccess;
                        this.PrevMaintLibrary.Create = item.ItemCreate;
                        this.PrevMaintLibrary.Delete = item.ItemDelete;
                        this.PrevMaintLibrary.Edit = item.ItemEdit;
                        break;
                    //Project
                    case SecurityConstants.Project:
                        this.Project.ItemName = item.ItemName;
                        this.Project.Create = item.ItemCreate;
                        this.Project.Delete = item.ItemDelete;
                        this.Project.Edit = item.ItemEdit;
                        this.Project.Access = item.ItemAccess;
                        break;
                    case SecurityConstants.Project_Cancel:
                        this.Project.Cancel = item.ItemAccess;
                        break;
                    case SecurityConstants.Project_Complete:
                        this.Project.Complete = item.ItemAccess;
                        break;
                    // PurchaseRequest
                    case SecurityConstants.PurchaseRequest:
                        this.PurchaseRequest.Access = item.ItemAccess;
                        this.PurchaseRequest.Create = item.ItemCreate;
                        this.PurchaseRequest.Delete = item.ItemDelete;
                        this.PurchaseRequest.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.PurchaseRequest_Approve:
                        this.PurchaseRequest.Approve = item.ItemAccess;
                        break;
                    case SecurityConstants.PurchaseRequest_AutoGen:
                        this.PurchaseRequest.AutoGeneration = item.ItemAccess;
                        break;
                    case SecurityConstants.PurchaseRequest_EditAwaitApprove:
                        this.PurchaseRequest.EditAwaitApprove = item.ItemAccess;
                        break;
                    case SecurityConstants.PurchaseRequest_EditApproved:
                        this.PurchaseRequest.EditApproved = item.ItemAccess;
                        break;
                    case SecurityConstants.PurchaseRequest_UsePunchout:
                        this.PurchaseRequest.UsePunchout = item.ItemAccess;
                        break;
                    // Purchasing
                    case SecurityConstants.Purchasing:
                        this.Purchasing.Access = item.ItemAccess;
                        this.Purchasing.Create = item.ItemCreate;
                        this.Purchasing.Delete = item.ItemDelete;
                        this.Purchasing.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.Purchasing_Approve:
                        this.Purchasing.Approve = item.ItemAccess;
                        break;
                    case SecurityConstants.Purchasing_ForceComplete:
                        this.Purchasing.ForceComplete = item.ItemAccess;
                        break;
                    case SecurityConstants.Purchasing_Receive:
                        this.Purchasing.Receive = item.ItemAccess;
                        break;
                    case SecurityConstants.Purchasing_ReceiveAccess:
                        this.Purchasing.ReceiveAccess = item.ItemAccess;
                        break;
                    case SecurityConstants.PurchaseOrder_SendPunchoutPO:
                        this.Purchasing.SendPunchoutPO = item.ItemAccess;
                        break;
                    case SecurityConstants.Purchasing_Void:
                        this.Purchasing.Void = item.ItemAccess;
                        break;
                    // Sanitation
                    case SecurityConstants.Sanitation:
                        this.Sanitation.Access = item.ItemAccess;
                        this.Sanitation.Create = item.ItemCreate;
                        this.Sanitation.Delete = item.ItemDelete;
                        this.Sanitation.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.SanitationJob_ApprovalWorkbench:
                        this.Sanitation.Approve = item.ItemAccess;
                        break;
                    case SecurityConstants.Sanitation_Job_Gen:
                        this.Sanitation.JobGeneration = item.ItemAccess;
                        break;
                    case SecurityConstants.Sanitation_OnDemand:
                        this.Sanitation.OnDemand = item.ItemAccess;
                        break;
                    case SecurityConstants.Sanitation_Verification:
                        this.Sanitation.Verification = item.ItemAccess;
                        break;
                    case SecurityConstants.Sanitation_WB:
                        this.Sanitation.Workbench = item.ItemAccess;
                        break;
                    // SanitationJob
                    case SecurityConstants.SanitationJob:
                        this.SanitationJob.Access = item.ItemAccess;
                        this.SanitationJob.Create = item.ItemCreate;
                        this.SanitationJob.Delete = item.ItemDelete;
                        this.SanitationJob.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.SanitationJob_CreateRequest:
                        this.SanitationJob.CreateRequest = item.ItemAccess;
                        break;
                    // Sensors
                    case SecurityConstants.Sensors:
                        this.Sensors.Access = item.ItemAccess;
                        this.Sensors.Create = item.ItemCreate;
                        this.Sensors.Delete = item.ItemDelete;
                        this.Sensors.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.AlertProcedures:
                        this.Sensors.AlertProcedures = item.ItemAccess;
                        break;
                    case SecurityConstants.SensorSearch:
                        this.Sensors.Search = item.ItemAccess;
                        break;
                    // Vendors
                    case SecurityConstants.Vendors:
                        this.Vendors.Access = item.ItemAccess;
                        this.Vendors.Create = item.ItemCreate;
                        this.Vendors.Delete = item.ItemDelete;
                        this.Vendors.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.Vendor_ChangeClientLookupID:
                        this.Vendors.ChangeClientLookupId = item.ItemAccess;
                        break;
                    case SecurityConstants.Vendor_ConfigurePunchout:
                        this.Vendors.ConfigurePunchout = item.ItemAccess;
                        break;
                    // VendorCatalog
                    case SecurityConstants.VendorCatalog:
                        this.VendorCatalog.Access = item.ItemAccess;
                        this.VendorCatalog.Create = item.ItemCreate;
                        this.VendorCatalog.Delete = item.ItemDelete;
                        this.VendorCatalog.Edit = item.ItemEdit;
                        break;
                    // VendorMaster
                    case SecurityConstants.VendorMaster:
                        this.VendorMaster.Access = item.ItemAccess;
                        this.VendorMaster.Create = item.ItemCreate;
                        this.VendorMaster.Delete = item.ItemDelete;
                        this.VendorMaster.Edit = item.ItemEdit;
                        break;
                    // WorkOrders
                    case SecurityConstants.WorkOrders:
                        this.WorkOrders.Access = item.ItemAccess;
                        this.WorkOrders.Create = item.ItemCreate;
                        this.WorkOrders.Delete = item.ItemDelete;
                        this.WorkOrders.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.WorkOrder_Approve:
                        this.WorkOrders.Approve = item.ItemAccess;
                        break;
                    case SecurityConstants.WorkOrder_Cancel:
                        this.WorkOrders.Cancel = item.ItemAccess;
                        break;
                    case SecurityConstants.WorkOrder_Complete:
                        this.WorkOrders.Complete = item.ItemAccess;
                        break;
                    case SecurityConstants.WorkOrder_CreateEmergency:
                        this.WorkOrders.CreateEmergency = item.ItemAccess;
                        break;
                    case SecurityConstants.WorkOrder_CreateFollowUp:
                        this.WorkOrders.CreateFollowUp = item.ItemAccess;
                        break;
                    case SecurityConstants.WorkOrder_CreateWR:
                        this.WorkOrders.CreateRequest = item.ItemAccess;
                        break;
                    case SecurityConstants.WorkOrder_LaborScheduling:
                        this.WorkOrders.LaborScheduling = item.ItemAccess;
                        break;
                    case SecurityConstants.WorkOrder_PartIssue:
                        this.WorkOrders.PartIssue = item.ItemAccess;
                        break;
                    case SecurityConstants.WorkOrder_Planning:
                        this.WorkOrders.Planning = item.ItemAccess;
                        break;
                    case SecurityConstants.WorkOrder_PlanningRequired:
                        this.WorkOrders.PlanningRequired = item.ItemAccess;
                        break;
                    case SecurityConstants.WorkOrder_Scheduling:
                        this.WorkOrders.Scheduling = item.ItemAccess;
                        break;
                    case SecurityConstants.WorkOrder_SendForApproval:
                        this.WorkOrders.SendForApproval = item.ItemAccess;
                        break;
                    case SecurityConstants.WorkOrder_CompletionWizardConfiguration:
                        this.WorkOrders.CompletionWizardConfiguration = item.ItemAccess;
                        break;
                    // Maintenance Completion Workbench
                    //V2-621
                    case SecurityConstants.MaintenanceCompletionWorkbenchWidget_AddFollowUpWO:
                        this.MaintenanceCompletionWorkbenchWidget_AddFollowUpWO.ItemName = item.ItemName;
                        this.MaintenanceCompletionWorkbenchWidget_AddFollowUpWO.Access = item.ItemAccess;
                        break;
                    //V2-621
                    case SecurityConstants.MaintenanceCompletionWorkbenchWidget_AddUnplannedWO:
                        this.MaintenanceCompletionWorkbenchWidget_AddUnplannedWO.ItemName = item.ItemName;
                        this.MaintenanceCompletionWorkbenchWidget_AddUnplannedWO.Access = item.ItemAccess;
                        break;
                    //V2-652
                    case SecurityConstants.MaintenanceCompletionWorkbenchWidget_AddWorkRequest:
                        this.MaintenanceCompletionWorkbenchWidget_AddWorkRequest.ItemName = item.ItemName;
                        this.MaintenanceCompletionWorkbenchWidget_AddWorkRequest.Access = item.ItemAccess;
                        break;
                    //V2-610
                    case SecurityConstants.MaintenanceCompletionWorkbenchWidget_Complete:
                        this.MaintenanceCompletionWorkbenchWidget_Complete.ItemName = item.ItemName;
                        this.MaintenanceCompletionWorkbenchWidget_Complete.Access = item.ItemAccess;
                        break;
                    //V2-610
                    case SecurityConstants.MaintenanceCompletionWorkbenchWidget_Cancel:
                        this.MaintenanceCompletionWorkbenchWidget_Cancel.ItemName = item.ItemName;
                        this.MaintenanceCompletionWorkbenchWidget_Cancel.Access = item.ItemAccess;
                        break;
                    //V2-610
                    case SecurityConstants.MaintenanceCompletionWorkbenchWidget_PartIssue:
                        this.MaintenanceCompletionWorkbenchWidget_PartIssue.ItemName = item.ItemName;
                        this.MaintenanceCompletionWorkbenchWidget_PartIssue.Access = item.ItemAccess;
                        break;
                    //V2-626
                    case SecurityConstants.WorkOrder_AddProjecttoWorkOrder:
                        this.WorkOrders.AddProjecttoWorkOrder = item.ItemAccess;
                        break;


                    // Module Level- Client Specific - BBU
                    // BBUMaintStats
                    case SecurityConstants.BBUMaintStats:
                        this.BBUMaintStats.Access = item.ItemAccess;
                        this.BBUMaintStats.Create = item.ItemCreate;
                        this.BBUMaintStats.Edit = item.ItemEdit;
                        break;
                    // Module Level - Not Used in V2
                    // Locations
                    case SecurityConstants.Locations:
                        this.Locations.Access = item.ItemAccess;
                        this.Locations.Create = item.ItemCreate;
                        this.Locations.Delete = item.ItemDelete;
                        this.Locations.Edit = item.ItemEdit;
                        break;
                    // PlantLocations
                    case SecurityConstants.PlantLocations:
                        this.PlantLocations.Access = item.ItemAccess;
                        this.PlantLocations.Create = item.ItemCreate;
                        this.PlantLocations.Delete = item.ItemDelete;
                        this.PlantLocations.Edit = item.ItemEdit;
                        break;
                    // PlantSetup
                    case SecurityConstants.PlantSetup:
                        this.PlantSetup.Access = item.ItemAccess;
                        this.PlantSetup.Create = item.ItemCreate;
                        this.PlantSetup.Delete = item.ItemDelete;
                        this.PlantSetup.Edit = item.ItemEdit;
                        break;
                    // Procedures
                    case SecurityConstants.Procedures:
                        this.Procedures.Access = item.ItemAccess;
                        this.Procedures.Create = item.ItemCreate;
                        this.Procedures.Delete = item.ItemDelete;
                        this.Procedures.Edit = item.ItemEdit;
                        break;
                    // ProcessSetup
                    case SecurityConstants.ProcessSetup:
                        this.ProcessSetup.Access = item.ItemAccess;
                        this.ProcessSetup.Create = item.ItemCreate;
                        this.ProcessSetup.Delete = item.ItemDelete;
                        this.ProcessSetup.Edit = item.ItemEdit;
                        break;
                    // ShoppingCart 
                    case SecurityConstants.ShoppingCart:
                        this.ShoppingCart.Access = item.ItemAccess;
                        this.ShoppingCart.Create = item.ItemCreate;
                        this.ShoppingCart.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.ShoppingCart_Approve:
                        this.ShoppingCart.Approve = item.ItemAccess;
                        break;
                    case SecurityConstants.ShoppingCart_AutoGen:
                        this.ShoppingCart.AutoGen = item.ItemAccess;
                        break;
                    case SecurityConstants.ShoppingCart_Review:
                        this.ShoppingCart.Review = item.ItemAccess;
                        break;
                    //
                    // Functional Level - Single Item Modules 
                    //
                    case SecurityConstants.Access_Enterprise_Maintenance_Dashboard:
                        this.Access_Enterprise_Maintenance_Dashboard.ItemName = item.ItemName;
                        this.Access_Enterprise_Maintenance_Dashboard.Access = item.ItemAccess;
                        break;
                    case SecurityConstants.AccessMaintenanceTechnicianDashboard:
                        this.AccessMaintenanceTechnicianDashboard.ItemName = item.ItemName;
                        this.AccessMaintenanceTechnicianDashboard.Access = item.ItemAccess;
                        break;
                    case SecurityConstants.AccessAPMDashboard:
                        this.AccessAPMDashboard.ItemName = item.ItemName;
                        this.AccessAPMDashboard.Access = item.ItemAccess;
                        break;
                    case SecurityConstants.AccessFleetDashboard:
                        this.AccessFleetDashboard.ItemName = item.ItemName;
                        this.AccessFleetDashboard.Access = item.ItemAccess;
                        break;
                    case SecurityConstants.AccessMaintenanceDashboard:
                        this.AccessMaintenanceDashboard.ItemName = item.ItemName;
                        this.AccessMaintenanceDashboard.Access = item.ItemAccess;
                        break;
                    case SecurityConstants.AccessSanitationDashboard:
                        this.AccessSanitationDashboard.ItemName = item.ItemName;
                        this.AccessSanitationDashboard.Access = item.ItemAccess;
                        break;
                    case SecurityConstants.Asset_Availability:
                        this.Asset_Availability.ItemName = item.ItemName;
                        this.Asset_Availability.Access = item.ItemAccess;
                        break;
                    // EventInfo
                    case SecurityConstants.Events:
                        this.EventInfo.Events = item.ItemAccess;
                        break;
                    case SecurityConstants.Events_Process:
                        this.EventInfo.EventsProcess = item.ItemAccess;
                        break;
                    case SecurityConstants.Fleet_Record_Fuel_Entry:
                        this.Fleet_RecordFuelEntry.ItemName = item.ItemName;
                        this.Fleet_RecordFuelEntry.Access = item.ItemAccess;
                        break;
                    case SecurityConstants.Fleet_Record_Meter_Reading:
                        this.Fleet_RecordMeterReading.ItemName = item.ItemName;
                        this.Fleet_RecordMeterReading.Access = item.ItemAccess;
                        break;
                    case SecurityConstants.ImportData_Equipment:
                        this.ImportData.Equipment = item.ItemAccess;
                        break;
                    case SecurityConstants.ImportData_Part:
                        this.ImportData.Part = item.ItemAccess;
                        break;
                    case SecurityConstants.Personnel_Auxiliary_Information:
                        this.Personnel_Auxiliary_Information.ItemName = item.ItemName;
                        this.Personnel_Auxiliary_Information.Access = item.ItemAccess;
                        break;
                    // Client Specific - BBU 
                    case SecurityConstants.BBUMaintStats_Extract:
                        this.BBUMaintStats.Extract = item.ItemAccess;
                        break;
                    //
                    // Functional Level - Not Used in V2 (Yet)
                    //
                    case SecurityConstants.BusinessIntelligence:
                        this.BusinessIntelligence.Access = item.ItemAccess;
                        break;
                    //
                    // Reports Level
                    //
                    case SecurityConstants.APM_Reports:
                        this.APM_Reports.ItemName = item.ItemName;
                        this.APM_Reports.Access = item.ItemAccess;      // V2-622
                                                                        //this.APM_Reports.ReportItem = item.ReportItem;
                        break;
                    case SecurityConstants.Asset_Reports:
                        this.Asset_Reports.ItemName = item.ItemName;
                        this.Asset_Reports.Access = item.ItemAccess;  // V2-622
                        //this.Asset_Reports.ReportItem = item.ReportItem;
                        break;
                    case SecurityConstants.Configuration_Reports:
                        this.Configuration_Reports.ItemName = item.ItemName;
                        this.Configuration_Reports.Access = item.ItemAccess;
                        //this.Configuration_Reports.ReportItem = item.ReportItem;
                        break;
                    case SecurityConstants.Create_Private_Reports:
                        this.Create_Private_Reports.Access = item.ItemAccess;
                        //this.Create_Private_Reports.Create = item.ItemCreate; // Not Used
                        //this.Create_Private_Reports.Delete = item.ItemDelete; // Not Used
                        //this.Create_Private_Reports.Edit = item.ItemEdit;     // Not Used
                        break;
                    case SecurityConstants.Create_Public_Reports:
                        this.Create_Public_Reports.Access = item.ItemAccess;
                        //this.Create_Public_Reports.Create = item.ItemCreate;  // Not Used
                        //this.Create_Public_Reports.Delete = item.ItemDelete;  // Not Used
                        //this.Create_Public_Reports.Edit = item.ItemEdit;      // Not Used
                        break;
                    case SecurityConstants.Create_Site_Reports:
                        this.Create_Site_Reports.Access = item.ItemAccess;
                        //this.Create_Site_Reports.Create = item.ItemCreate;    // Not Used
                        //this.Create_Site_Reports.Delete = item.ItemDelete;    // Not Used
                        //this.Create_Site_Reports.Edit = item.ItemEdit;        // Not Used
                        break;
                    case SecurityConstants.Enterprise_APM:
                        this.Enterprise_APM.ItemName = item.ItemName;
                        this.Enterprise_APM.Access = item.ItemAccess;
                        //this.Enterprise_APM.Create = item.ItemCreate;         // Not Used
                        //this.Enterprise_APM.Delete = item.ItemDelete;         // Not Used
                        //this.Enterprise_APM.Edit = item.ItemEdit;             // Not Used
                        //this.Enterprise_APM.ReportItem = item.ReportItem;     // Not Used
                        break;
                    case SecurityConstants.Enterprise_CMMS:
                        this.Enterprise_CMMS.ItemName = item.ItemName;
                        this.Enterprise_CMMS.Access = item.ItemAccess;
                        //this.Enterprise_CMMS.Create = item.ItemCreate;        // Not Used
                        //this.Enterprise_CMMS.Delete = item.ItemDelete;        // Not Used
                        //this.Enterprise_CMMS.Edit = item.ItemEdit;            // Not Used
                        //this.Enterprise_CMMS.ReportItem = item.ReportItem;    // Not Used
                        break;
                    case SecurityConstants.Enterprise_Fleet:
                        this.Enterprise_Fleet.Access = item.ItemAccess;
                        //this.Enterprise_Fleet.Create = item.ItemCreate;
                        //this.Enterprise_Fleet.Delete = item.ItemDelete;
                        //this.Enterprise_Fleet.Edit = item.ItemEdit;
                        //this.Enterprise_Fleet.ItemName = item.ItemName;
                        //this.Enterprise_Fleet.ReportItem = item.ReportItem;
                        break;
                    case SecurityConstants.Enterprise_Sanitation:
                        this.Enterprise_Sanitation.Access = item.ItemAccess;
                        //this.Enterprise_Sanitation.Create = item.ItemCreate;
                        //this.Enterprise_Sanitation.Delete = item.ItemDelete;
                        //this.Enterprise_Sanitation.Edit = item.ItemEdit;
                        //this.Enterprise_Sanitation.ItemName = item.ItemName;
                        //this.Enterprise_Sanitation.ReportItem = item.ReportItem;
                        break;
                    case SecurityConstants.Inventory_Reports:
                        this.Inventory_Reports.ItemName = item.ItemName;
                        this.Inventory_Reports.Access = item.ItemAccess;
                        //this.Inventory_Reports.ReportItem = item.ReportItem;
                        break;
                    case SecurityConstants.Preventive_Maintenance_Reports:
                        this.Preventive_Maintenance_Reports.ItemName = item.ItemName;
                        this.Preventive_Maintenance_Reports.Access = item.ItemAccess;
                        break;
                    case SecurityConstants.Purchasing_Reports:
                        this.Purchasing_Reports.ItemName = item.ItemName;
                        this.Purchasing_Reports.Access = item.ItemAccess;
                        break;
                    case SecurityConstants.Reports:
                        this.Reports.Access = item.ItemAccess;
                        break;
                    case SecurityConstants.Reports_Fleet:
                        this.Reports_Fleet.ItemName = item.ItemName;
                        this.Reports_Fleet.Access = item.ItemAccess;
                        break;
                    case SecurityConstants.Reports_MakePublic:
                        this.Reports.MakePublic = item.ItemAccess;
                        break;
                    case SecurityConstants.Sanitation_Reports:
                        this.Sanitation_Reports.ItemName = item.ItemName;
                        this.Sanitation_Reports.Access = item.ItemAccess;
                        break;
                    case SecurityConstants.Work_Order_Reports:
                        this.Work_Order_Reports.ItemName = item.ItemName;
                        this.Work_Order_Reports.Access = item.ItemAccess;
                        break;
                    //V2-691
                    case SecurityConstants.Parts_MaterialRequest:
                        this.Parts.MaterialRequest = item.ItemAccess;
                        break;
                    //V2-690
                    case SecurityConstants.MaintenanceCompletionWorkbenchWidget_MaterialRequest:
                        this.MaintenanceCompletionWorkbenchWidget_MaterialRequest.ItemName = item.ItemName;
                        this.MaintenanceCompletionWorkbenchWidget_MaterialRequest.Access = item.ItemAccess;
                        break;
                    //V2-690
                    case SecurityConstants.WorkOrder_MaterialRequest:
                        this.WorkOrder_MaterialRequest.ItemName = item.ItemName;
                        this.WorkOrder_MaterialRequest.Access = item.ItemAccess;
                        break;
                    // 
                    // Asset Downtime V2-695
                    case SecurityConstants.Asset_Downtime:
                        this.Asset_Downtime.Access = item.ItemAccess;
                        this.Asset_Downtime.Create = item.ItemCreate;
                        this.Asset_Downtime.Delete = item.ItemDelete;
                        this.Asset_Downtime.Edit = item.ItemEdit;
                        break;
                    // Workorder Downtime V2-695
                    case SecurityConstants.WorkOrder_Downtime:
                        this.WorkOrder_Downtime.Access = item.ItemAccess;
                        this.WorkOrder_Downtime.Create = item.ItemCreate;
                        this.WorkOrder_Downtime.Delete = item.ItemDelete;
                        this.WorkOrder_Downtime.Edit = item.ItemEdit;
                        break;
                    // Maintenance Completion Workbench Widget Downtime V2-695
                    case SecurityConstants.MaintenanceCompletionWorkbenchWidget_Downtime:
                        this.MaintenanceCompletionWorkbenchWidget_Downtime.Access = item.ItemAccess;
                        this.MaintenanceCompletionWorkbenchWidget_Downtime.Create = item.ItemCreate;
                        this.MaintenanceCompletionWorkbenchWidget_Downtime.Delete = item.ItemDelete;
                        this.MaintenanceCompletionWorkbenchWidget_Downtime.Edit = item.ItemEdit;
                        break;
                    // Maintenance Completion Workbench Widget Photos V2-716
                    case SecurityConstants.MaintenanceCompletionWorkbenchWidget_Photos:
                        this.MaintenanceCompletionWorkbenchWidget_Photos.ItemName = item.ItemName;
                        this.MaintenanceCompletionWorkbenchWidget_Photos.Access = item.ItemAccess;
                        this.MaintenanceCompletionWorkbenchWidget_Photos.Create = item.ItemCreate;
                        this.MaintenanceCompletionWorkbenchWidget_Photos.Delete = item.ItemDelete;
                        this.MaintenanceCompletionWorkbenchWidget_Photos.Edit = item.ItemEdit;
                        break;
                    // WorkOrder Photos V2-716
                    case SecurityConstants.WorkOrder_Photos:
                        this.WorkOrder_Photos.ItemName = item.ItemName;
                        this.WorkOrder_Photos.Access = item.ItemAccess;
                        this.WorkOrder_Photos.Create = item.ItemCreate;
                        this.WorkOrder_Photos.Delete = item.ItemDelete;
                        this.WorkOrder_Photos.Edit = item.ItemEdit;
                        break;
                    // Asset Photos V2-716
                    case SecurityConstants.Asset_Photos:
                        this.Asset_Photos.ItemName = item.ItemName;
                        this.Asset_Photos.Access = item.ItemAccess;
                        this.Asset_Photos.Create = item.ItemCreate;
                        this.Asset_Photos.Delete = item.ItemDelete;
                        this.Asset_Photos.Edit = item.ItemEdit;
                        break;
                    // Parts Photos V2-716
                    case SecurityConstants.Parts_Photos:
                        this.Parts_Photos.ItemName = item.ItemName;
                        this.Parts_Photos.Access = item.ItemAccess;
                        this.Parts_Photos.Create = item.ItemCreate;
                        this.Parts_Photos.Delete = item.ItemDelete;
                        this.Parts_Photos.Edit = item.ItemEdit;
                        break;
                    //Approval Groups V2-720
                    case SecurityConstants.ApprovalGroupsConfiguration:
                        this.ApprovalGroupsConfiguration.ItemName = item.ItemName;
                        this.ApprovalGroupsConfiguration.Access = item.ItemAccess;
                        break;
                    //Approval Groups V2-720
                    case SecurityConstants.MaterialRequest_Approve:
                        this.MaterialRequest_Approve.ItemName = item.ItemName;
                        this.MaterialRequest_Approve.Access = item.ItemAccess;
                        break;
                    //User Review in PR Approval Process V2-820
                    case SecurityConstants.PurchaseRequest_Review:
                        this.PurchaseRequest_Review.ItemName = item.ItemName;
                        this.PurchaseRequest_Review.Access = item.ItemAccess;
                        break;
                    // Reports Level - Not Used In V2
                    //
                    //BBUKPI_Site V2-823
                    case SecurityConstants.BBUKPI_Site:
                        this.BBUKPI_Site.ItemName = item.ItemName;
                        this.BBUKPI_Site.Access = item.ItemAccess;
                        this.BBUKPI_Site.Create = item.ItemCreate;
                        this.BBUKPI_Site.Delete = item.ItemDelete;
                        this.BBUKPI_Site.Edit = item.ItemEdit;
                        break;
                    //BBUKPI_Enterprise V2-823
                    case SecurityConstants.BBUKPI_Enterprise:
                        this.BBUKPI_Enterprise.ItemName = item.ItemName;
                        this.BBUKPI_Enterprise.Access = item.ItemAccess;
                        this.BBUKPI_Enterprise.Create = item.ItemCreate;
                        this.BBUKPI_Enterprise.Delete = item.ItemDelete;
                        this.BBUKPI_Enterprise.Edit = item.ItemEdit;
                        break;
                    //SanitationJob_Photos V2-841
                    case SecurityConstants.SanitationJob_Photos:
                        this.SanitationJob_Photos.ItemName = item.ItemName;
                        this.SanitationJob_Photos.Access = item.ItemAccess;
                        this.SanitationJob_Photos.Create = item.ItemCreate;
                        this.SanitationJob_Photos.Delete = item.ItemDelete;
                        this.SanitationJob_Photos.Edit = item.ItemEdit;
                        break;
                    //V2-906
                    case SecurityConstants.Parts_UpdatePart_Costs:
                        this.Parts_UpdatePart_Costs.Access = item.ItemAccess;
                        break;
                    //V2-915
                    case SecurityConstants.Vendor_Approve_Vendor_Request:
                        this.Vendor_Approve_Vendor_Request.ItemName = item.ItemName;
                        this.Vendor_Approve_Vendor_Request.Access = item.ItemAccess;
                        break;
                    //V2-915
                    case SecurityConstants.Vendor_Create_Vendor_Request:
                        this.Vendor_Create_Vendor_Request.ItemName = item.ItemName;
                        this.Vendor_Create_Vendor_Request.Access = item.ItemAccess;
                        break;
                    //V2-929
                    case SecurityConstants.Vendor_Insurance:
                        this.Vendor_Insurance.ItemName = item.ItemName;
                        this.Vendor_Insurance.Access = item.ItemAccess;
                        this.Vendor_Insurance.Create = item.ItemCreate;
                        this.Vendor_Insurance.Delete = item.ItemDelete;
                        this.Vendor_Insurance.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.Vendor_Insurance_OverrideInsurance:
                        this.Vendor_Insurance_OverrideInsurance.ItemName = item.ItemName;
                        this.Vendor_Insurance_OverrideInsurance.Access = item.ItemAccess;
                        break;
                    case SecurityConstants.Vendor_Insurance_RemoveOverride:
                        this.Vendor_Insurance_RemoveOverride.ItemName = item.ItemName;
                        this.Vendor_Insurance_RemoveOverride.Access = item.ItemAccess;
                        break;
                    //V2-944
                    case SecurityConstants.WorkOrder_FormConfiguration:
                        this.WorkOrder_FormConfiguration.ItemName = item.ItemName;
                        this.WorkOrder_FormConfiguration.Access = item.ItemAccess;
                        break;
                    //V2-933
                    case SecurityConstants.Vendor_AssetMgt:
                        this.Vendor_AssetMgt.ItemName = item.ItemName;
                        this.Vendor_AssetMgt.Access = item.ItemAccess;
                        this.Vendor_AssetMgt.Create = item.ItemCreate;
                        this.Vendor_AssetMgt.Delete = item.ItemDelete;
                        this.Vendor_AssetMgt.Edit = item.ItemEdit;
                        break;
                    case SecurityConstants.Vendor_AssetMgt_OverrideAssetMgt:
                        this.Vendor_AssetMgt_OverrideAssetMgt.ItemName = item.ItemName;
                        this.Vendor_AssetMgt_OverrideAssetMgt.Access = item.ItemAccess;
                        break;
                    case SecurityConstants.Vendor_AssetMgt_RemoveOverride:
                        this.Vendor_AssetMgt_RemoveOverride.ItemName = item.ItemName;
                        this.Vendor_AssetMgt_RemoveOverride.Access = item.ItemAccess;
                        break;
                    //V2-912
                    case SecurityConstants.SanitationJob_Approve:
                        this.SanitationJob_Approve.ItemName = item.ItemName;
                        this.SanitationJob_Approve.Access = item.ItemAccess;
                        break;
                    //V2-912
                    case SecurityConstants.SanitationJob_Complete:
                        this.SanitationJob_Complete.ItemName = item.ItemName;
                        this.SanitationJob_Complete.Access = item.ItemAccess;
                        break;
                    //V2-1055
                    case SecurityConstants.SanitationJob_AddMaintenanceWorkRequest:
                        this.SanitationJob_AddMaintenanceWorkRequest.ItemName = item.ItemName;
                        this.SanitationJob_AddMaintenanceWorkRequest.Access = item.ItemAccess;
                        break;
                    //V2-1056
                    case SecurityConstants.SanitationJob_Sanitation_Request_From_Maintenance:
                        this.SanitationJob_Sanitation_Request_From_Maintenance.ItemName = item.ItemName;
                        this.SanitationJob_Sanitation_Request_From_Maintenance.Access = item.ItemAccess;
                        break;
                    //V2-1046
                    case SecurityConstants.PurchaseRequest_Consolidate:
                        this.PurchaseRequest.Consolidate = item.ItemAccess;
                        break;
                    //V2-1047
                    case SecurityConstants.PurchaseOrder_Model:
                        this.PurchaseOrder_Model.ItemName = item.ItemName;
                        this.PurchaseOrder_Model.Access = item.ItemAccess;
                        break;
                    //V2-1051
                    case SecurityConstants.WorkOrder_Model:
                        this.WorkOrders.Model = item.ItemAccess;
                        break;
                    //V2-1059
                    case SecurityConstants.PartTransfer_Auto_Transfer_Generation:
                        this.PartTransfer_Auto_Transfer_Generation.ItemName = item.ItemName;
                        this.PartTransfer_Auto_Transfer_Generation.Access = item.ItemAccess;
                        break;

                    //V2-1031
                    case SecurityConstants.Parts_Issue:
                        this.Parts_Issue.ItemName = item.ItemName;
                        this.Parts_Issue.Access = item.ItemAccess;
                        break;

                    //V2-1063
                    case SecurityConstants.PurchaseRequest_MaterialRequestItems:
                        this.PurchaseRequest.MaterialRequestItems = item.ItemAccess;
                        break;
                    //V2-1086
                    case SecurityConstants.ShipToAddress:
                        this.ShipToAddress.ItemName = item.ItemName;
                        this.ShipToAddress.Access = item.ItemAccess;
                        this.ShipToAddress.Create = item.ItemCreate;
                        this.ShipToAddress.Delete = item.ItemDelete;
                        this.ShipToAddress.Edit = item.ItemEdit;
                        break;
                    //V2-1103
                    case SecurityConstants.Sensors_Record_Reading:
                        this.Sensors.RecordReading = item.ItemAccess;
                        break;
                    //V2-1079
                    case SecurityConstants.PurchaseOrder_SendEDI_PO:
                        this.Purchasing.SendEDIPO = item.ItemAccess;
                        break;
                    //V2-1186
                    case SecurityConstants.ConvertToPurchaseOrder:
                        this.Convert_To_PurchaseOrder.Access = item.ItemAccess;
                        this.Convert_To_PurchaseOrder.ItemName = item.ItemName;
                        break;
                    //V2-1203
                    case SecurityConstants.Parts_Model:
                        this.Parts.Model = item.ItemAccess;
                        break;
                    //V2-1202
                    case SecurityConstants.Asset_Model:
                        this.Equipment.Model = item.ItemAccess;
                        break;
                    //V2-1177
                    case SecurityConstants.Analytics_WorkOrderStatus:
                        this.Analytics.WorkOrderStatus = item.ItemAccess;
                        break;
                    //V2-1196
                    case SecurityConstants.Parts_ConfigureAutoPurchasing:
                        this.Parts.ConfigureAutoPurchasing = item.ItemAccess;
                        break;
                    //V2-1204
                    case SecurityConstants.PrevMaint_Model:
                        this.PrevMaint.Model = item.ItemAccess;
                        break;
                    default:
                        break;
                    
                }
                #endregion switch 

            }
        }
        #endregion
    }
    #region Abstract Classes
    [Serializable]
    public abstract class Sec_base
    {
        #region Private Properties
        private bool access = false;
        private bool create = false;
        private bool edit = false;
        private bool delete = false;
        private bool reportItem = false;
        private string itemName = string.Empty;
        #endregion Private Properties

        #region Public Properties
        public bool Access
        {
            get { return this.access; }
            set { this.access = value; }
        }
        public bool Create
        {
            get { return this.create; }
            set { this.create = value; }
        }
        public bool Edit
        {
            get { return this.edit; }
            set { this.edit = value; }
        }
        public bool Delete
        {
            get { return this.delete; }
            set { this.delete = value; }
        }

        public bool ReportItem
        {
            get { return this.reportItem; }
            set { this.reportItem = value; }
        }

        public string ItemName
        {
            get { return this.itemName; }
            set { this.itemName = value; }
        }
        #endregion Public Properties

    }
    #endregion
    #region Maintenance Classes
    [Serializable]
    public class Sec_Accounts : Sec_base
    {
        public Sec_Accounts()
        {
        }
    }
    [Serializable]
    public class Sec_Equipment : Sec_base
    {
        //SOM-1677
        private bool changeclientlookupid;
        // V2-639 
        private bool asset_scrapasset;
        private bool assetmodel = false;//V2-1202
        public Sec_Equipment()
        {
        }
        //SOM-1677
        public bool ChangeClientLookupId
        {
            get { return this.changeclientlookupid; }
            set { this.changeclientlookupid = value; }
        }
        // V2-639
        public bool Asset_ScrapAsset
        {
            get { return this.asset_scrapasset; }
            set { this.asset_scrapasset = value; }
        }
        // V2-1202
        public bool Model
        {
            get { return this.assetmodel; }
            set { this.assetmodel = value; }
        }
    }
    [Serializable]
    public class Sec_Asset_Repairable_Spare : Sec_base
    {
        private bool assignment;
        public Sec_Asset_Repairable_Spare()
        {
        }
        public bool Assignment
        {
            get { return this.assignment; }
            set { this.assignment = value; }
        }
    }

    [Serializable]
    public class Sec_Locations : Sec_base
    {
        public Sec_Locations()
        {
        }
    }

    [Serializable]
    public class Sec_Meters : Sec_base
    {
        private bool recordReading = false;
        public Sec_Meters()
        {
        }
        public bool RecordReading
        {
            get { return this.recordReading; }
            set { this.recordReading = value; }
        }
    }

    [Serializable]
    public class Sec_Gauges : Sec_base
    {
        private bool recordReading = false;
        public Sec_Gauges()
        {
        }
        public bool RecordReading
        {
            get { return this.recordReading; }
            set { this.recordReading = value; }
        }
    }

    [Serializable]
    public class Sec_PrevMaint : Sec_base
    {
        private bool generate_wo = false;
        private bool prevMaintPMForecast = false;
        private bool changeclientlookupid = false;
        private bool model = false;//V2-1204
        public Sec_PrevMaint()
        {
            // Default Constructor
        }

        public bool Generate_WorkOrders
        {
            get { return this.generate_wo; }
            set { this.generate_wo = value; }
        }
        public bool PrevMaintPMForecast
        {
            get { return this.prevMaintPMForecast; }
            set { this.prevMaintPMForecast = value; }
        }

        public bool ChangeClientLookupId
        {
            get { return this.changeclientlookupid; }
            set { this.changeclientlookupid = value; }
        }
        //V2-1204
        public bool Model
        {
            get { return this.model; }
            set { this.model = value; }
        }
    }

    [Serializable]
    public class Sec_Procedures : Sec_base
    {
        public Sec_Procedures()
        {
        }
    }
    //SOM-1378
    [Serializable]
    public class Sec_Sensor : Sec_base
    {
        private bool alertprocedures = false;
        private bool sensorSearch = false;
        private bool recordreading = false;
        public Sec_Sensor()
        {
            // Default Constructor    
        }

        public bool AlertProcedures
        {
            get { return this.alertprocedures; }
            set { this.alertprocedures = value; }
        }
        public bool Search
        {
            get { return this.sensorSearch; }
            set { this.sensorSearch = value; }
        }
        public bool RecordReading
        {
            get { return this.recordreading; }
            set { this.recordreading = value; }
        }

    }



    [Serializable]
    public class Sec_Sanitation : Sec_base
    {
        private bool jobgen = false;
        private bool wb = false;
        private bool approve = false;
        private bool ondemad = false;//SOM-1333
        private bool verification = false;//SOM-1271

        public Sec_Sanitation()
        {
            // Default Constructor    
        }
        public bool JobGeneration
        {
            get { return this.jobgen; }
            set { this.jobgen = value; }
        }
        public bool Workbench
        {
            get { return this.wb; }
            set { this.wb = value; }
        }
        public bool Approve
        {
            get { return this.approve; }
            set { this.approve = value; }
        }
        //SOM-1333
        public bool OnDemand
        {
            get { return this.ondemad; }
            set { this.ondemad = value; }
        }
        public bool Verification
        {
            get { return this.verification; }
            set { this.verification = value; }
        }
    }
    //public class Sec_ShoppingCart : Sec_base
    //{

    //    public Sec_ShoppingCart()
    //    {
    //        // Default Constructor
    //    }

    //}
    [Serializable]
    public class Sec_SanitationJob : Sec_base
    {
        private bool createRequest = false;//SOM-1249
        public Sec_SanitationJob()
        {
            // Default Constructor    
        }
        public bool CreateRequest
        {
            get { return this.createRequest; }
            set { this.createRequest = value; }
        }
    }

    [Serializable]
    public class Sec_WorkOrder : Sec_base
    {
        private bool addProjecttoWorkOrder = false;
        private bool approve = false;
        private bool cancel = false;//--SOM-905--//
        private bool complete = false;
        private bool completionwizardconfiguration = false;
        private bool createemergency = false;
        private bool createfollowup = false;
        private bool createrequest = false;
        private bool laborsched = false;
        private bool partissue = false;
        private bool planning = false;
        private bool planningRequired = false;
        private bool scheduling = false;
        private bool sendForApproval = false;
        private bool model = false;

        public Sec_WorkOrder()
        {
            // Default Constructor
        }

        public bool CreateRequest
        {
            get { return this.createrequest; }
            set { this.createrequest = value; }
        }

        public bool Approve
        {
            get { return this.approve; }
            set { this.approve = value; }
        }
        public bool Complete
        {
            get { return this.complete; }
            set { this.complete = value; }
        }
        public bool CompletionWizardConfiguration
        {
            get { return this.completionwizardconfiguration; }
            set { this.completionwizardconfiguration = value; }
        }
        public bool LaborScheduling
        {
            get { return this.laborsched; }
            set { this.laborsched = value; }
        }
        public bool CreateFollowUp
        {
            get { return this.createfollowup; }
            set { this.createfollowup = value; }
        }
        public bool CreateEmergency
        {
            get { return this.createemergency; }
            set { this.createemergency = value; }
        }
        //--SOM-905--//
        public bool Cancel
        {
            get { return this.cancel; }
            set { this.cancel = value; }
        }

        public bool Scheduling
        {
            get { return this.scheduling; }
            set { this.scheduling = value; }
        }

        public bool PlanningRequired
        {
            get { return this.planningRequired; }
            set { this.planningRequired = value; }
        }
        public bool PartIssue
        {
            get { return this.partissue; }
            set { this.partissue = value; }
        }
        public bool Planning
        {
            get { return this.planning; }
            set { this.planning = value; }
        }
        public bool AddProjecttoWorkOrder
        {
            get { return this.addProjecttoWorkOrder; }
            set { this.addProjecttoWorkOrder = value; }
        }


        public bool SendForApproval
        {
            get { return this.sendForApproval; }
            set { this.sendForApproval = value; }
        }

        public bool Model
        {
            get { return this.model; }
            set { this.model = value; }
        }
    }
    [Serializable]
    public class Sec_Fleet_Assets : Sec_base
    {
        public Sec_Fleet_Assets()
        {
        }
    }
    [Serializable]
    public class Sec_Reports_Fleet : Sec_base
    {
        public Sec_Reports_Fleet()
        {
        }
    }

    [Serializable]
    public class Sec_Fleet_Meter_History : Sec_base
    {
        public Sec_Fleet_Meter_History()
        {
        }
    }
    [Serializable]
    public class Sec_Fleet_Record_Meter_Reading : Sec_base
    {
        public Sec_Fleet_Record_Meter_Reading()
        {
        }
        private bool recordMeterReading = false;
        public bool RecordMeterReading
        {
            get { return this.recordMeterReading; }
            set { this.recordMeterReading = value; }
        }

    }
    [Serializable]
    public class Sec_Fleet_Fuel_Tracking : Sec_base
    {
        public Sec_Fleet_Fuel_Tracking()
        {
        }
    }
    [Serializable]
    public class Sec_Fleet_Record_Fuel_Entry : Sec_base
    {
        public Sec_Fleet_Record_Fuel_Entry()
        {
        }
    }
    [Serializable]
    public class Sec_Fleet_Service_Task : Sec_base
    {
        public Sec_Fleet_Service_Task()
        {
        }
    }
    //V2-406
    [Serializable]
    public class Sec_Scheduled_Service_Entry : Sec_base //V2-406
    {
        public Sec_Scheduled_Service_Entry()
        {
        }
    }
    //V2-406
    [Serializable]
    public class Sec_Fleet_Issues : Sec_base
    {
        public Sec_Fleet_Issues()
        {
        }
    }

    //V2-388
    [Serializable]
    public class Sec_Fleet_Service_Order : Sec_base //V2-406
    {
        public Sec_Fleet_Service_Order()
        {
        }
    }

    //V2-413
    [Serializable]
    public class Sec_Create_Private_Reports : Sec_base
    {
        public Sec_Create_Private_Reports()
        {
        }
    }


    //V2-413
    [Serializable]
    public class Sec_Create_Site_Reports : Sec_base
    {
        public Sec_Create_Site_Reports()
        {
        }
    }


    //V2-413
    [Serializable]
    public class Sec_Create_Public_Reports : Sec_base
    {
        public Sec_Create_Public_Reports()
        {
        }
    }


    //V2-413
    [Serializable]
    public class Sec_Enterprise_CMMS : Sec_base
    {
        public Sec_Enterprise_CMMS()
        {
        }
    }


    //V2-413
    [Serializable]
    public class Sec_Enterprise_Sanitation : Sec_base
    {
        public Sec_Enterprise_Sanitation()
        {
        }
    }

    //V2-413
    [Serializable]
    public class Sec_Enterprise_APM : Sec_base
    {
        public Sec_Enterprise_APM()
        {
        }
    }

    //V2-413
    [Serializable]
    public class Sec_Enterprise_Fleet : Sec_base
    {
        public Sec_Enterprise_Fleet()
        {
        }
    }
    //V2-485
    [Serializable]
    public class Sec_Asset_Availability : Sec_base
    {
        public Sec_Asset_Availability()
        {
        }
    }

    //V2-552
    [Serializable]
    public class Sec_AccessMaintenanceDashboard : Sec_base
    {
        public Sec_AccessMaintenanceDashboard()
        {
        }
    }

    //V2-552
    [Serializable]
    public class Sec_AccessSanitationDashboard : Sec_base
    {
        public Sec_AccessSanitationDashboard()
        {
        }
    }

    //V2-552
    [Serializable]
    public class Sec_AccessAPMDashboard : Sec_base
    {
        public Sec_AccessAPMDashboard()
        {
        }
    }

    //V2-552
    [Serializable]
    public class Sec_AccessFleetDashboard : Sec_base
    {
        public Sec_AccessFleetDashboard()
        {
        }
    }

    //V2-552
    [Serializable]
    public class Sec_Access_Enterprise_Maintenance_Dashboard : Sec_base
    {
        public Sec_Access_Enterprise_Maintenance_Dashboard()
        {
        }
    }
    //public class Sec_WorkRequest : Sec_base
    //{
    //    private bool create = false;


    //    public Sec_WorkRequest()
    //    {
    //        // Default Constructor
    //    }

    //    public bool Create
    //    {
    //        get { return this.create; }
    //        set { this.create = value; }
    //    }


    //}
    #endregion Maintenance Classes
    #region Inventory Classes
    [Serializable]
    public class Sec_Parts : Sec_base
    {
        private bool siteReview = false;
        private bool checkout = false;
        private bool receipt = false;
        private bool physical = false;
        private bool mssearch = false;
        private bool eqxref = false;
        private bool vexref = false;
        private bool changeclientlookupid;  //SOM-1677
        private bool cyclecount = false;
        private bool materialrequest = false;//V2-691
        private bool model = false;//V2-1203
        private bool parts_configureautopurchasing = false;//V2-1196
        public Sec_Parts()
        {
            // Default Constructor
        }
        public bool SiteReview
        {
            get { return this.siteReview; }
            set { this.siteReview = value; }
        }
        public bool Checkout
        {
            get { return this.checkout; }
            set { this.checkout = value; }
        }
        public bool CycleCount
        {
            get { return this.cyclecount; }
            set { this.cyclecount = value; }
        }
        public bool Receipt
        {
            get { return this.receipt; }
            set { this.receipt = value; }
        }
        public bool Physical
        {
            get { return this.physical; }
            set { this.physical = value; }
        }
        public bool MultiSiteSearch
        {
            get { return this.mssearch; }
            set { this.mssearch = value; }
        }
        // SOM-1279
        public bool Part_Equipment_XRef
        {
            get { return this.eqxref; }
            set { this.eqxref = value; }
        }
        public bool Part_Vendor_XRef
        {
            get { return this.vexref; }
            set { this.vexref = value; }
        }
        // SOM-1677
        public bool ChangeClientLookupId
        {
            get { return this.changeclientlookupid; }
            set { this.changeclientlookupid = value; }
        }
        //V2-691
        public bool MaterialRequest
        {
            get { return this.materialrequest; }
            set { this.materialrequest = value; }
        }
        //V2-1203
        public bool Model
        {
            get { return this.model; }
            set { this.model = value; }
        }
        //V2-1196
        public bool ConfigureAutoPurchasing
        {
            get { return this.parts_configureautopurchasing; }
            set { this.parts_configureautopurchasing = value; }
        }
    }

    [Serializable]
    public class Sec_Vendors : Sec_base
    {
        private bool changeclientlookupid;
        private bool configurePunchout;
        public Sec_Vendors()
        {
            // Default Constructor
        }
        public bool ChangeClientLookupId
        {
            get { return this.changeclientlookupid; }
            set { this.changeclientlookupid = value; }
        }
        public bool ConfigurePunchout
        {
            get { return this.configurePunchout; }
            set { this.configurePunchout = value; }
        }
    }
    #endregion
    #region Purchasing Classes

    [Serializable]
    public class Sec_Purchasing : Sec_base
    {
        private bool approve = false;
        private bool receive = false;
        private bool secvoid = false;
        private bool secfcomp = false;
        private bool receiveaccess = false; // SOM-1534 - Receipts Access 
        private bool sendPunchoutPO = false;
        private bool sendEDIPO = false;//V2-1079
        public Sec_Purchasing()
        {
            // Default Constructor
        }

        public bool Approve
        {
            get { return this.approve; }
            set { this.approve = value; }
        }
        public bool Receive
        {
            get { return this.receive; }
            set { this.receive = value; }
        }
        public bool ReceiveAccess
        {
            get { return this.receiveaccess; }
            set { this.receiveaccess = value; }
        }
        public bool Void
        {
            get { return this.secvoid; }
            set { this.secvoid = value; }
        }
        // SOM-684
        public bool ForceComplete
        {
            get { return this.secfcomp; }
            set { this.secfcomp = value; }
        }
        public bool SendPunchoutPO
        {
            get { return this.sendPunchoutPO; }
            set { this.sendPunchoutPO = value; }
        }
        //V2-1079
        public bool SendEDIPO
        {
            get { return this.sendEDIPO; }
            set { this.sendEDIPO = value; }
        }
    }
    #endregion
    #region PurchaseRequest Classes
    [Serializable]
    public class Sec_PurchaseRequest : Sec_base
    {
        private bool approve = false;
        private bool autogeneration = false;
        private bool editawaitapprove = false;
        private bool editapproved = false;    // 2022-11-03
        private bool usePunchout = false;
        // private bool shoppingcart = false;
        // private bool shoppingcartapprove = false;
        // private bool shoppingCartReview = false;
        private bool consolidate = false;
        private bool materialrequest = false;


        public Sec_PurchaseRequest()
        {
            // Default Constructor
        }
        public bool Approve
        {
            get { return this.approve; }
            set { this.approve = value; }
        }

        public bool AutoGeneration
        {
            get { return this.autogeneration; }
            set { this.autogeneration = value; }
        }
        public bool EditAwaitApprove
        {
            get { return this.editawaitapprove; }
            set { this.editawaitapprove = value; }
        }
        // RKL - 2022-11-03 - change from editawaitapprove to editapproved
        public bool EditApproved
        {
            get { return this.editapproved; }
            set { this.editapproved = value; }
        }
        public bool UsePunchout
        {
            get { return this.usePunchout; }
            set { this.usePunchout = value; }
        }

        //public bool ShoppingCart
        // {
        //  get { return this.shoppingcart; }
        //  set { this.shoppingcart = value; }
        // }
        //  public bool ShoppingCartApprove
        // {
        //   get { return this.shoppingcartapprove; }
        //    set { this.shoppingcartapprove = value; }
        //  }
        //  public bool ShoppingCartReview
        //  {
        //      get { return this.shoppingCartReview; }
        //      set { this.shoppingCartReview = value; }
        //  }
        public bool Consolidate
        {
            get { return this.consolidate; }
            set { this.consolidate = value; }
        }
        public bool MaterialRequestItems
        {
            get { return this.materialrequest; }
            set { this.materialrequest = value; }
        }
    }
    #endregion
    #region BBUMaintStats

    [Serializable]
    public class Sec_BBUMaintStats : Sec_base
    {
        private bool secbbuextract = false;
        public Sec_BBUMaintStats()
        {
            // Default Constructor
        }
        public bool Extract
        {
            get { return this.secbbuextract; }
            set { this.secbbuextract = value; }
        }
    }
    #endregion
    #region Invoice Matching Classes

    [Serializable]
    public class Sec_InvoiceMatching : Sec_base
    {
        private bool _invoicePaid = false;
        private bool _authorizeToPay = false;
        private bool _reOpen = false;
        private bool _changeInvoiceId = false;
        public Sec_InvoiceMatching()
        {
        }
        public bool InvoicePaid
        {
            get { return this._invoicePaid; }
            set { this._invoicePaid = value; }
        }
        public bool AuthorizeToPay
        {
            get { return this._authorizeToPay; }
            set { this._authorizeToPay = value; }
        }
        public bool ReOpen
        {
            get { return this._reOpen; }
            set { this._reOpen = value; }
        }
        public bool ChangeInvoiceId
        {
            get { return this._changeInvoiceId; }
            set { this._changeInvoiceId = value; }
        }
    }


    #endregion
    #region ProcessSetup Classes

    [Serializable]
    public class Sec_ProcessSetup : Sec_base
    {
        public Sec_ProcessSetup()
        {
        }
    }


    #endregion
    #region Downtime Classes
    [Serializable]
    public class Sec_Downtime : Sec_base
    {
        public Sec_Downtime()
        {
        }
    }


    #endregion
    #region Reports  Classes
    [Serializable]
    public class Sec_Reports : Sec_base
    {
        private bool makepublic = false;//Som-1193
        public Sec_Reports()
        {
        }
        public bool MakePublic
        {
            get { return this.makepublic; }
            set { this.makepublic = value; }
        }

    }
    [Serializable]
    public class Sec_Asset_Reports : Sec_base
    {
        private bool makepublic = false;//Som-1193
        public Sec_Asset_Reports()
        {
        }
        public bool MakePublic
        {
            get { return this.makepublic; }
            set { this.makepublic = value; }
        }

    }

    [Serializable]
    public class Sec_Work_Order_Reports : Sec_base
    {
        private bool makepublic = false;//Som-1193
        public Sec_Work_Order_Reports()
        {
        }
        public bool MakePublic
        {
            get { return this.makepublic; }
            set { this.makepublic = value; }
        }

    }
    [Serializable]
    public class Sec_Preventive_Maintenance_Reports : Sec_base
    {
        private bool makepublic = false;//Som-1193
        public Sec_Preventive_Maintenance_Reports()
        {
        }
        public bool MakePublic
        {
            get { return this.makepublic; }
            set { this.makepublic = value; }
        }

    }

    [Serializable]
    public class Sec_Inventory_Reports : Sec_base
    {
        private bool makepublic = false;//Som-1193
        public Sec_Inventory_Reports()
        {
        }
        public bool MakePublic
        {
            get { return this.makepublic; }
            set { this.makepublic = value; }
        }

    }

    [Serializable]
    public class Sec_Purchasing_Reports : Sec_base
    {
        private bool makepublic = false;//Som-1193
        public Sec_Purchasing_Reports()
        {
        }
        public bool MakePublic
        {
            get { return this.makepublic; }
            set { this.makepublic = value; }
        }

    }

    [Serializable]
    public class Sec_Sanitation_Reports : Sec_base
    {
        private bool makepublic = false;
        public Sec_Sanitation_Reports()
        {
        }
        public bool MakePublic
        {
            get { return this.makepublic; }
            set { this.makepublic = value; }
        }

    }
    [Serializable]
    public class Sec_APM_Reports : Sec_base
    {
        private bool makepublic = false;
        public Sec_APM_Reports()
        {
        }
        public bool MakePublic
        {
            get { return this.makepublic; }
            set { this.makepublic = value; }
        }

    }
    [Serializable]
    public class Sec_Configuration_Reports : Sec_base
    {
        private bool makepublic = false;
        public Sec_Configuration_Reports()
        {
        }
        public bool MakePublic
        {
            get { return this.makepublic; }
            set { this.makepublic = value; }
        }

    }
    #endregion
    #region Dashboards Classes
    [Serializable]
    public class Sec_Dashboards : Sec_base
    {
        public Sec_Dashboards()
        {
        }
    }


    #endregion
    #region Plant Setup Classes
    [Serializable]
    public class Sec_PlantSetup : Sec_base
    {
        public Sec_PlantSetup()
        {
        }
    }

    #endregion
    #region PartMaster
    [Serializable]
    public class Sec_PartMaster : Sec_base
    {
        public Sec_PartMaster()
        {
            // Default Constructor
        }
        private bool assignpart = false;
        public bool AssignPart
        {
            get { return this.assignpart; }
            set { this.assignpart = value; }
        }
    }

    #endregion
    #region ManufacturerMaster
    [Serializable]
    public class Sec_ManufacturerMaster : Sec_base
    {

        public Sec_ManufacturerMaster()
        {
            // Default Constructor
        }

    }

    #endregion
    #region Plant Locations Classes
    [Serializable]
    public class Sec_PlantLocations : Sec_base
    {
        public Sec_PlantLocations()
        {
        }
    }

    #endregion
    #region VendorMaster
    [Serializable]
    public class Sec_VendorMaster : Sec_base
    {
        public Sec_VendorMaster()
        {
            // Default Constructor
        }

    }
    #endregion
    #region VendorCatalog
    [Serializable]
    public class Sec_VendorCatalog : Sec_base
    {
        public Sec_VendorCatalog()
        {
            // Default Constructor
        }
    }
    #endregion
    #region PartCategoryMaster
    [Serializable]
    public class Sec_PartCategoryMaster : Sec_base
    {

        public Sec_PartCategoryMaster()
        {
            // Default Constructor
        }

    }
    #endregion
    #region PartMasterRequest
    [Serializable]
    public class Sec_PartMasterRequest : Sec_base
    {
        public Sec_PartMasterRequest()
        {
            // Default Constructor
        }
        private bool review = false;
        private bool approve = false;
        private bool approve2 = false;
        public bool Review
        {
            get { return this.review; }
            set { this.review = value; }
        }
        public bool Approve
        {
            get { return this.approve; }
            set { this.approve = value; }
        }
        public bool ApproveEnterprise
        {
            get { return this.approve2; }
            set { this.approve2 = value; }
        }
    }

    #endregion
    #region Maintenance On-Demand
    [Serializable]
    public class Sec_OnDemandLibrary : Sec_base
    {
        public Sec_OnDemandLibrary()
        {
            // Default Constructor
        }
    }
    [Serializable]
    public class Sec_PrevMaintLibrary : Sec_base
    {
        public Sec_PrevMaintLibrary()
        {
            // Default Constructor
        }
    }
    [Serializable]
    public class Sec_MasterSanitationLibrary : Sec_base
    {
        public Sec_MasterSanitationLibrary()
        {
            // Default Constructor
        }
    }


    #endregion
    #region BusinessIntelligence
    [Serializable]
    public class Sec_BusinessIntelligence : Sec_base
    {
        public Sec_BusinessIntelligence()
        {
            // Default Constructor
        }

    }
    #endregion
    #region EquipmentMaster
    [Serializable]
    public class Sec_EquipmentMaster : Sec_base
    {
        public Sec_EquipmentMaster()
        {
            // Default Constructor
        }

    }
    #endregion
    #region ShoppingCart
    [Serializable]
    public class Sec_ShoppingCart : Sec_base
    {
        private bool review = false;
        private bool approve = false;
        private bool autogen = false;
        public Sec_ShoppingCart()
        {
            // Default Constructor
        }
        public bool Review
        {
            get { return this.review; }
            set { this.review = value; }
        }
        public bool Approve
        {
            get { return this.approve; }
            set { this.approve = value; }
        }
        public bool AutoGen
        {
            get { return this.autogen; }
            set { this.autogen = value; }
        }
    }
    #endregion
    #region ImportData
    [Serializable]
    public class Sec_ImportData : Sec_base
    {
        public Sec_ImportData()
        {
            // Default Constructor
        }
        private bool importdataEquipment = false;
        private bool importdataPart = false;
        public bool Equipment
        {
            get { return this.importdataEquipment; }
            set { this.importdataEquipment = value; }
        }
        public bool Part
        {
            get { return this.importdataPart; }
            set { this.importdataPart = value; }
        }
    }
    #endregion
    #region EventInfo
    [Serializable]
    public class Sec_EventInfo : Sec_base
    {
        public Sec_EventInfo()
        {
            // Default Constructor
        }
        private bool events = false;
        private bool eventsprocess = false;
        public bool Events
        {
            get { return this.events; }
            set { this.events = value; }
        }
        public bool EventsProcess
        {
            get { return this.eventsprocess; }
            set { this.eventsprocess = value; }
        }
    }
    #endregion
    #region PartTransfers
    [Serializable]
    public class Sec_PartTransfers : Sec_base
    {
        public Sec_PartTransfers()
        {
            // Default Constructor
        }
        private bool process = false;
        public bool PartTransfers_Process
        {
            get { return this.process; }
            set { this.process = value; }
        }
    }
    #endregion
    #region Personnel Classes
    [Serializable]
    public class Sec_Personnel : Sec_base
    {
        private bool masterquerysetup;

        public Sec_Personnel()
        {
        }
        public bool MasterQuerySetup
        {
            get { return this.masterquerysetup; }
            set { this.masterquerysetup = value; }
        }
    }

    [Serializable]
    public class Sec_Personnel_Events : Sec_base
    {
        public Sec_Personnel_Events()
        {
        }
    }

    [Serializable]
    public class Sec_Personnel_Schedule : Sec_base
    {
        public Sec_Personnel_Schedule()
        {
        }
    }

    [Serializable]
    public class Sec_Personnel_Attendance : Sec_base
    {
        public Sec_Personnel_Attendance()
        {
        }
    }

    [Serializable]
    public class Sec_Personnel_Auxiliary_Information : Sec_base
    {
        public Sec_Personnel_Auxiliary_Information()
        {
        }
    }

    [Serializable]
    public class Sec_Personnel_Availability : Sec_base
    {
        public Sec_Personnel_Availability()
        {
        }
    }

    #endregion
    #region Project Classes
    [Serializable]
    public class Sec_Project : Sec_base
    {
        public Sec_Project()
        {
        }
        private bool cancel = false;
        private bool complete = false;
        public bool Cancel
        {
            get { return this.cancel; }
            set { this.cancel = value; }
        }
        public bool Complete
        {
            get { return this.complete; }
            set { this.complete = value; }
        }
    }
    [Serializable]
    public class Sec_AccessMaintenanceTechnicianDashboard : Sec_base
    {
        public Sec_AccessMaintenanceTechnicianDashboard()
        {
        }
    }
    [Serializable]
    public class Sec_MaintenanceCompletionWorkbenchWidget_Complete : Sec_base
    {
        public Sec_MaintenanceCompletionWorkbenchWidget_Complete()
        {
        }
    }
    [Serializable]
    public class Sec_MaintenanceCompletionWorkbenchWidget_Cancel : Sec_base
    {
        public Sec_MaintenanceCompletionWorkbenchWidget_Cancel()
        {
        }
    }
    [Serializable]
    public class Sec_MaintenanceCompletionWorkbenchWidget_PartIssue : Sec_base
    {
        public Sec_MaintenanceCompletionWorkbenchWidget_PartIssue()
        {
        }
    }
    [Serializable]
    public class Sec_MaintenanceCompletionWorkbenchWidget_AddUnplannedWO : Sec_base
    {
        public Sec_MaintenanceCompletionWorkbenchWidget_AddUnplannedWO()
        {
        }
    }
    [Serializable]
    public class Sec_MaintenanceCompletionWorkbenchWidget_AddWorkRequest : Sec_base
    {
        public Sec_MaintenanceCompletionWorkbenchWidget_AddWorkRequest()
        {
        }
    }
    [Serializable]
    public class Sec_MaintenanceCompletionWorkbenchWidget_AddFollowUpWO : Sec_base
    {
        public Sec_MaintenanceCompletionWorkbenchWidget_AddFollowUpWO()
        {
        }
    }
    [Serializable]
    public class Sec_MaintenanceCompletionWorkbenchWidget_MaterialRequest : Sec_base
    {
        public Sec_MaintenanceCompletionWorkbenchWidget_MaterialRequest()
        {
        }
    }
    [Serializable]
    public class Sec_WorkOrder_MaterialRequest : Sec_base
    {
        public Sec_WorkOrder_MaterialRequest()
        {
        }
    }
    #endregion
    #region Asset Downtime Classes
    [Serializable]
    public class Sec_Asset_Downtime : Sec_base
    {
        public Sec_Asset_Downtime()
        {
        }
    }
    #endregion
    #region Workorder Downtime Classes
    [Serializable]
    public class Sec_WorkOrder_Downtime : Sec_base
    {
        public Sec_WorkOrder_Downtime()
        {
        }
    }
    #endregion
    #region MaintenanceCompletionWorkbenchWidget Downtime Classes
    [Serializable]
    public class Sec_MaintenanceCompletionWorkbenchWidget_Downtime : Sec_base
    {
        public Sec_MaintenanceCompletionWorkbenchWidget_Downtime()
        {
        }
    }
    #endregion
    #region Multiple Photos Upload V2-716
    [Serializable]
    public class Sec_MaintenanceCompletionWorkbenchWidget_Photos : Sec_base
    {
        public Sec_MaintenanceCompletionWorkbenchWidget_Photos()
        {
        }
    }
    [Serializable]
    public class Sec_Asset_Photos : Sec_base
    {
        public Sec_Asset_Photos()
        {
        }
    }
    [Serializable]
    public class Sec_WorkOrder_Photos : Sec_base
    {
        public Sec_WorkOrder_Photos()
        {
        }
    }
    [Serializable]
    public class Sec_Parts_Photos : Sec_base
    {
        public Sec_Parts_Photos()
        {
        }
    }
    #endregion

    #region ApprovalGroupsConfiguration Classes V2-720
    [Serializable]
    public class Sec_ApprovalGroupsConfiguration : Sec_base
    {
        public Sec_ApprovalGroupsConfiguration()
        {
        }
    }
    #endregion
    #region MaterialRequest_Approve Classes V2-720
    [Serializable]
    public class Sec_MaterialRequest_Approve : Sec_base
    {
        public Sec_MaterialRequest_Approve()
        {
        }
    }
    #endregion
    #region PurchaseRequest_Review Classes V2-820
    [Serializable]
    public class Sec_PurchaseRequest_Review : Sec_base
    {
        public Sec_PurchaseRequest_Review()
        {
        }
    }
    #endregion
    #region BBUKPI_Site Classes V2-823
    [Serializable]
    public class Sec_BBUKPI_Site : Sec_base
    {
        public Sec_BBUKPI_Site()
        {
        }
    }
    #endregion
    #region BBUKPI_Enterprise Classes V2-823
    [Serializable]
    public class Sec_BBUKPI_Enterprise : Sec_base
    {
        public Sec_BBUKPI_Enterprise()
        {
        }
    }
    #endregion

    #region SanitationJob_Photos Classes V2-841
    [Serializable]
    public class Sec_SanitationJob_Photos : Sec_base
    {
        public Sec_SanitationJob_Photos()
        {
        }
    }
    #endregion

    #region Parts_UpdatePart_Costs Classes V2-906
    [Serializable]
    public class Sec_Parts_UpdatePart_Costs : Sec_base
    {
        public Sec_Parts_UpdatePart_Costs()
        {
        }
    }
    #endregion

    #region Vendor_Approve_Vendor_Request Classes V2-915
    [Serializable]
    public class Sec_Vendor_Approve_Vendor_Request : Sec_base
    {
        public Sec_Vendor_Approve_Vendor_Request()
        {
        }
    }
    #endregion
    #region Vendor_Create_Vendor_Request Classes V2-915
    [Serializable]
    public class Sec_Vendor_Create_Vendor_Request : Sec_base
    {
        public Sec_Vendor_Create_Vendor_Request()
        {
        }
    }
    #endregion

    #region Vendor_Insurance Classes V2-929
    [Serializable]
    public class Sec_Vendor_Insurance : Sec_base
    {
        public Sec_Vendor_Insurance()
        {
        }
    }
    #endregion
    #region Vendor_Insurance_OverrideInsurance Classes V2-929
    [Serializable]
    public class Sec_Vendor_Insurance_OverrideInsurance : Sec_base
    {
        public Sec_Vendor_Insurance_OverrideInsurance()
        {
        }
    }
    #endregion
    #region Vendor_Insurance_RemoveOverride Classes V2-929
    [Serializable]
    public class Sec_Vendor_Insurance_RemoveOverride : Sec_base
    {
        public Sec_Vendor_Insurance_RemoveOverride()
        {
        }
    }
    #endregion
    #region WorkOrder Form Configuration Classes V2-944
    [Serializable]
    public class Sec_WorkOrder_FormConfiguration : Sec_base
    {
        public Sec_WorkOrder_FormConfiguration()
        {
        }
    }
    #endregion

    #region Vendor_AssetMgt Classes V2-933
    [Serializable]
    public class Sec_Vendor_AssetMgt : Sec_base
    {
        public Sec_Vendor_AssetMgt()
        {
        }
    }
    #endregion
    #region Sec_Vendor_AssetMgt_OverrideAssetMgt Classes V2-933
    [Serializable]
    public class Sec_Vendor_AssetMgt_OverrideAssetMgt : Sec_base
    {
        public Sec_Vendor_AssetMgt_OverrideAssetMgt()
        {
        }
    }
    #endregion
    #region Vendor_AssetMgt_RemoveOverride Classes V2-933
    [Serializable]
    public class Sec_Vendor_AssetMgt_RemoveOverride : Sec_base
    {
        public Sec_Vendor_AssetMgt_RemoveOverride()
        {
        }
    }
    #endregion
    #region SanitationJob Complete Classes V2-912
    [Serializable]
    public class Sec_SanitationJob_Complete : Sec_base
    {
        public Sec_SanitationJob_Complete()
        {
        }
    }
    #endregion
    #region SanitationJob Approve Classes V2-912
    [Serializable]
    public class Sec_SanitationJob_Approve : Sec_base
    {
        public Sec_SanitationJob_Approve()
        {
        }
    }
    #endregion
    #region PurchaseOrder Model Classes V2-1047
    [Serializable]
    public class Sec_PurchaseOrder_Model : Sec_base
    {
        public Sec_PurchaseOrder_Model()
        {
        }
    }
    #endregion

    #region SanitationJob Add Maintenance WorkRequest Classes V2-1055
    [Serializable]
    public class Sec_SanitationJob_AddMaintenanceWorkRequest : Sec_base
    {
        public Sec_SanitationJob_AddMaintenanceWorkRequest()
        {
        }
    }
    #endregion
    #region Maintenance Add Sanitation Request Classes V2-1056
    [Serializable]
    public class Sec_SanitationJob_Sanitation_Request_From_Maintenance : Sec_base
    {
        public Sec_SanitationJob_Sanitation_Request_From_Maintenance()
        {
        }
    }
    #endregion

    #region Parts Issue V2-1031
    [Serializable]
    public class Sec_Parts_Issue : Sec_base
    {
        public Sec_Parts_Issue()
        {
        }
    }
    #endregion
    #region PartTransfer Auto Transfer Generation V2-1059
    [Serializable]
    public class Sec_PartTransfer_Auto_Transfer_Generation : Sec_base
    {
        public Sec_PartTransfer_Auto_Transfer_Generation()
        {
        }
    }
    #endregion

    #region V2-1086
    [Serializable]
    public class Sec_ShipToAddress : Sec_base
    {
        public Sec_ShipToAddress()
        {
                
        }
      
    }
    #endregion
    #region Convert To PurchaseOrder V2-1186
    [Serializable]
    public class Sec_Convert_To_PurchaseOrder : Sec_base
    {
        public Sec_Convert_To_PurchaseOrder()
        {
        }
    }
    #endregion

    #region V2-1177 Analytics Dashboard
    [Serializable]
    public class Sec_Analytics : Sec_base
    {
        private bool workorderstatus = false;
        public Sec_Analytics()
        {
        }
        public bool WorkOrderStatus
        {
            get { return this.workorderstatus; }
            set { this.workorderstatus = value; }
        }
    }
    #endregion

}

