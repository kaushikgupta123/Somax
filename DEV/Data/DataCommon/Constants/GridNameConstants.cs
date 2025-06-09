/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID Person              Description
* =========== ======= =================== ==========================================================
* 2016-Aug-07 SOM-1049 Roger Lawton       Added new class to store/retrieve grid layout information
****************************************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Constants

{
    public class GridNameConstants
    {
        public const string AccountSearch = "Account_Search";
        public const string EquipmentSearch = "Equipment_Search";
        public const string EquipmentMasterSearch = "EquipmentMaster_Search";
        public const string GaugeSearch = "Gauge_Search";
        public const string InvoiceMatchSearch = "InvoiceMatch_Search";
        public const string LocationSearch = "Location_Search";
        public const string ManufacturerMasterSearch = "ManufacturerMaster_Search";
        public const string MaintenanceOnDemandProcedures = "MaintenanceOnDemandProcedures_Search";

        public const string MeterSearch = "Meter_Search";
        public const string PartCategoryMasterSearch = "PartCategoryMaster_Search";
        public const string PartSearch = "Part_Search";
        //SOM-1366
        public const string PartMasterSearch = "PartMaster_Search";
        public const string PrintPartLabel = "PrintPartLabel";
        public const string PrevMaintSearch = "PrevMaint_Search";
        public const string PrevMaintLibrarySearch = "PrevMaintLibrary_Search";
        public const string MasterSanitationLibrarySearch = "MasterSanitationLibrary _Search";
        public const string PurchaseOrderSearch = "PurchaseOrder_Search";
        public const string PurchaseOrderReceiptSearch = "PurchaseOrderReceipt_Search";
        public const string PurchaseRequestSearch = "PurchaseRequest_Search";
       
        //SOM-1249
        public const string SanitationJobSearch = "SanitationJob_Search";
        public const string SanOnDemandMasterSearch = "SanOnDemandMasterSearch";
        public const string SanitationMasterSearch = "SanitationMaster_Search";
        public const string SanitationScheduleMasterSearch = "SanitationScheduleMaster_Search";
        //SOM-1378
        public const string SensorAlertProcedureSearch = "SensorAlertProcedure_Search";
        //SOM-1383
        public const string SensorSearch = "Sensor_Search";
        //SOM-1367
        public const string VendorMasterSearch = "VendorMaster_Search";
        public const string VendorSearch = "Vendor_Search";
        public const string WorkOrderSearch = "WorkOrder_Search";
        public const string VendorCatalogSearch = "VendorCatalog_Search";
        //SOM-1515
        public const string PartMasterRequestSearch = "PartMasterRequest_Search";
        //public const string EventInfoSearch = "EventInfo_Search";
        public const string PartsTransferSearch = "PartsTransfer_Search";

        public const string UserSearch = "User_Search";
        public const string MultiSitePartSearch = "MultiSitePart_Search";
        public const string DeviceSearch = "Device_Search";
        public const string IoTEventSearch = "IoTEvent_Search";
    }
}
