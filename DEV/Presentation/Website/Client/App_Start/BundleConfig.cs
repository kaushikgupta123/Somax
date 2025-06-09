using System.Web;
using System.Web.Optimization;

namespace Client
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            #region Style-Bundle

            #region LayOut
            bundles.Add(new StyleBundle("~/Content/LayoutStyle").Include(
               "~/Content/dialog-mobile.css",
               "~/Scripts/Metronic/assets/vendors/base/vendors.bundle.css",
               "~/Scripts/Metronic/assets/demo/default/base/style.bundle.css",
               "~/Scripts/Metronic/assets/demo/default/base/jquery.mCustomScrollbar.css",
               "~/Scripts/Metronic/assets/demo/default/base/style_custom.css",
               "~/Scripts/Metronic/assets/demo/default/base/responsive.css"
               ));
            #endregion

            #region Dashboard
            bundles.Add(new StyleBundle("~/Content/dashboardStyle").Include(
            "~/Content/Dashboard/dashboard.css",
            "~/Content/_CustomizeGridSetup.css",
            "~/Scripts/Metronic/assets/demo/default/base/style_custom_tree.css"
            ));
            #endregion

            #region Common
            bundles.Add(new StyleBundle("~/Content/datatable").Include(
                "~/Scripts/Metronic/GridLib/jquery.dataTables.min.css",
                "~/Scripts/Metronic/GridLib/buttons.dataTables.min.css",
                "~/Scripts/Metronic/GridLib/fixedColumns.dataTables.min.css",
                "~/Scripts/Metronic/GridLib/customDataTable.css",
                "~/Content/bootstrap-multiselect.css"
                ));

            bundles.Add(new StyleBundle("~/Content/dropzone").Include(
                "~/Scripts/dropzone/dropzone.min.css",
                "~/Content/dropzone-custom.css"
                ));
            bundles.Add(new StyleBundle("~/Content/Common").Include(
              "~/Content/sweetalert.css",
              "~/Content/tootltipcustom.css",
              "~/Content/themes/base/jquery-ui.css",
              "~/Content/custom-dropdown-style.css"
              ));
            bundles.Add(new StyleBundle("~/Content/daterangepickercustom").Include(
             "~/Content/daterangepicker_theme.css",
             "~/Content/daterangepicker_custom.css"
             ));
            bundles.Add(new StyleBundle("~/Content/somaxcommentsection").Include(
            "~/Content/Ckeditor/Activity_Comments.css",
            "~/Content/Ckeditor/CkeditorCustomStyle.css"
            ));
            bundles.Add(new StyleBundle("~/Content/FullCalendar").Include(
                "~/Scripts/FullCalendar/packages/core/main.css",
                "~/Scripts/FullCalendar/packages/daygrid/main.css"
            //"~/Scripts/FullCalendar/custom/custom-main.css"
            ));
            bundles.Add(new StyleBundle("~/Content/GanttDHTMLX").Include(
                "~/Scripts/GanttDHTMLX/gantt.css",
                "~/Scripts/GanttDHTMLX/gantt_material.css"
            ));

            //bundles.Add(new StyleBundle("~/Content/Wizard").Include(
            //     "~/Content/Wizard/Wizard.css"
            //  ));

            #endregion

            #region Login
            bundles.Add(new StyleBundle("~/Content/passwordcreate").Include(
                 "~/Scripts/Metronic/assets/login/vendors/base/vendors.bundle.css",
                 "~/Scripts/Metronic/assets/login/demo/default/base/responsive.css"
              ));
            #endregion

            #region Reports
            bundles.Add(new StyleBundle("~/Content/SomaxReports").Include(
                 "~/Scripts/Metronic/assets/demo/default/base/GlobalSearchGrid.css",
                 "~/Content/Reports/Reports.css"
              ));
            #endregion

            #region Labor Scheduling
            bundles.Add(new StyleBundle("~/Content/LaborSchedulingList").Include(
                 "~/Content/LaborScheduling/LaborSchedulingList.css"
              ));
            bundles.Add(new StyleBundle("~/Content/LaborSchedulingCalendar").Include(
                 "~/Content/LaborScheduling/LaborSchedulingCalendar.css"
              ));
            #endregion

            #region Work Order Planning
            bundles.Add(new StyleBundle("~/Content/ResourceList").Include(
                 "~/Content/WorkOrderPlanning/ResourceList.css"
              ));
            bundles.Add(new StyleBundle("~/Content/ResourceCalendar").Include(
                 "~/Content/WorkOrderPlanning/ResourceCalendar.css"
              ));
            #endregion

            #region Project
            bundles.Add(new StyleBundle("~/Content/Project").Include(
                 "~/Content/Project/TimelineDetails.css"
              ));
            #endregion         

            #endregion

            #region Script-Bundle
            #region Login
            bundles.Add(new ScriptBundle("~/bundles/Login").Include(
             "~/Scripts/Metronic/assets/vendors/base/vendors.bundle.min.js",
             "~/Scripts/Metronic/assets/demo/demo3/base/scripts.bundle.js",
             "~/Scripts/Metronic/assets/snippets/pages/user/login.js"
             ));
            #endregion

            #region CreatePassword
            bundles.Add(new ScriptBundle("~/bundles/CreatePassword").Include(
             "~/Scripts/Metronic/assets/vendors/base/vendors.bundle.min.js",
             "~/Scripts/Metronic/assets/demo/demo3/base/scripts.bundle.js"
             ));
            #endregion

            #region Layout
            bundles.Add(new ScriptBundle("~/bundles/LayoutScript").Include(
              "~/Scripts/mcx-dialog.js",
              "~/Scripts/Metronic/assets/vendors/base/vendors.bundle.min.js",
              "~/Scripts/Metronic/assets/demo/demo3/base/scripts.bundle.js",
              "~/Scripts/Metronic/assets/login/demo/default/base/jquery.mCustomScrollbar.concat.min.js",
              "~/Scripts/Metronic/assets/demo/default/custom/components/forms/widgets/input-mask.js",
              "~/Scripts/aes.js",
              "~/Scripts/Pages/CommonDatatable.js",
              "~/Scripts/Pages/CommonAlert.js",
              "~/Scripts/Pages/CommonLoader.js",
              "~/Scripts/Pages/CommonMenu.js",
              "~/Scripts/Pages/CommonNotification.js",
              "~/Scripts/Pages/Somax_main.js",
              "~/Scripts/Pages/prpunchoutvendor-grid-dropdownformenu.js",
             "~/Scripts/bootstrap-multiselect.js"
              ));
            #endregion

            #region Common
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/Common/sweetalert.min.js",
                        "~/Scripts/jquery-ui-1.12.1.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/Common/tooltipvalidatormessage.js",
                        "~/Scripts/Pages/Customvalidator/customvalidator.js",
                        "~/Scripts/Pages/JsConstants.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                "~/Scripts/Metronic/GridLib/jquery.dataTables.min.js",
                "~/Scripts/Metronic/GridLib/dataTables.colReorder.min.js",
                "~/Scripts/Metronic/GridLib/dataTables.bootstrap4.min.js",
                "~/Scripts/Metronic/GridLib/dataTables.buttons.min.js",
                "~/Scripts/Metronic/GridLib/pdfmake.min.js",
                "~/Scripts/Metronic/GridLib/vfs_fonts.js",
                //"~/Scripts/Metronic/GridLib/buttons.flash.min.js",
                "~/Scripts/Metronic/GridLib/buttons.html5.min.js",
                "~/Scripts/Metronic/GridLib/buttons.print.min.js",
                "~/Scripts/Metronic/GridLib/dataTables.fixedColumns.min.js",
                 // "~/Scripts/buttons.colVis.min.js",
                 "~/Scripts/Metronic/GridLib/jszip.min.js",
                 "~/Scripts/Pages/equipment/jquery.selectlistactions.js",
                "~/Scripts/Pages/CommonTextInputNavigation.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/imagezoom").Include(
                "~/Scripts/ImageZoom/jquery.elevateZoom.js", // don't replace this file as it is customized (line no 260-266 and 326-332)
                "~/Scripts/dropzone/dropzone.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/QRPrint").Include(
               "~/Scripts/jspdf.min.js",
               "~/Scripts/html2canvas.min.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/FullCalendar").Include(
               "~/Scripts/FullCalendar/packages/core/main.js",
               "~/Scripts/FullCalendar/packages/interaction/main.js",
               "~/Scripts/FullCalendar/packages/daygrid/main.js",
               "~/Scripts/FullCalendar/popper1.16.1.js", // For tooltip
               "~/Scripts/FullCalendar/tooltip1.3.3.js"   // For tooltip
               ));

            bundles.Add(new ScriptBundle("~/bundles/GanttDHTMLX").Include(
               "~/Scripts/GanttDHTMLX/gantt.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/ImageCompressor").Include(
               "~/Scripts/ImageCompressor/Fengyuanchen_ImageCompressor.js"
               ));
            #endregion

            #region Somax

            #region Dashboard
            bundles.Add(new ScriptBundle("~/bundles/Dashboard").Include(
               //"~/Scripts/Pages/dashboard/dashboard_graph.js"
               "~/Scripts/Pages/dashboard/Widgets/Sortable.js",
               "~/Scripts/Pages/dashboard/Widgets/DashboardWidgets.js"
               ));
            #endregion

            #region Equipment
            bundles.Add(new ScriptBundle("~/bundles/Equipment").Include(
               "~/Scripts/treeTable.js",
               "~/Scripts/Pages/equipment/equipmentSearch.js",
               "~/Scripts/Pages/equipment/EquipmentDetails.js",
               "~/Scripts/Pages/equipment/part-grid-dropdown.js",
               "~/Scripts/Pages/equipment/eqpvendor-grid-dropdown.js",
               "~/Scripts/Pages/equipment/AssetAvailabilityLog-Grid-Dropdown.js",
               "~/Scripts/Pages/equipment/AssetModelWizard.js", //V2-1211 script "~/Scripts/Common/LookupTypeTablePopup/TableVendorPopup.js" removed from bundle and codes are added to "~/Scripts/Pages/equipment/eqpvendor-grid-dropdown.js"
               "~/Scripts/Common/LookupTypeTablePopup/TableAccountPoopup.js",
               "~/Scripts/Pages/equipment/RepairableEquip-grid-dropdown.js",
               "~/Scripts/Pages/equipment/AssignmentViewLog-grid-dropdown.js",
                "~/Scripts/Common/LookupTypeTablePopup/TableAssetPopupLookup.js",
                 "~/Scripts/treeTable.js",
                "~/Scripts/Common/LookupTypeTablePopup/EquipmentAssetTree.js"
               ));
            bundles.Add(new ScriptBundle("~/bundles/EquipmentSearchMobile").Include(
                "~/Scripts/Pages/equipment/Mobile/eqpvendor-grid-dropdown.js",
                "~/Scripts/Pages/equipment/Mobile/Account-Grid-Dropdown.js",
                "~/Scripts/Pages/equipment/Mobile/equipment-grid-dropdown.js",
                "~/Scripts/treeTable.js",
              "~/Scripts/mobile/js/mobiscroll.jquery.min.js",
              "~/Scripts/Pages/equipment/Mobile/EquipmentSearchMobile.js",
               "~/Scripts/Pages/equipment/Mobile/EquipmentDetailsMobile.js"
              ));
            #endregion

            #region FleetAsset
            bundles.Add(new ScriptBundle("~/bundles/FleetAsset").Include(
               "~/Scripts/treeTable.js",
               "~/Scripts/Pages/FleetAsset/FleetAssetSearch.js",
               "~/Scripts/Pages/FleetAsset/FleetAssetDetails.js",
               "~/Scripts/Pages/FleetAsset/part-grid-dropdown.js",
               "~/Scripts/Pages/FleetAsset/AssetAvailabilityLog_Grid_Dropdown.js"
               ));
            #endregion

            #region FleetFuel
            bundles.Add(new ScriptBundle("~/bundles/FleetFuel").Include(
               "~/Scripts/Pages/FleetFuel/FleetFuelSearch.js",
               "~/Scripts/Pages/FleetFuel/Equipment-Grid-Dropdown.js"
               ));
            #endregion

            #region FleetMeter
            bundles.Add(new ScriptBundle("~/bundles/FleetMeter").Include(
               "~/Scripts/Pages/FleetMeter/fleetmeter.js",
               "~/Scripts/Pages/FleetMeter/assetGridPopup.js"
               ));
            #endregion

            #region FleetIssue
            bundles.Add(new ScriptBundle("~/bundles/FleetIssue").Include(
               "~/Scripts/Pages/FleetIssue/FleetIssueSearch.js",
               "~/Scripts/Pages/FleetIssue/Equipment-Grid-Dropdown.js"
               ));
            #endregion

            #region FleetService
            bundles.Add(new ScriptBundle("~/bundles/FleetService").Include(
               "~/Scripts/treeTable.js",
               "~/Scripts/Pages/FleetService/FleetServiceSearch.js",
               "~/Scripts/Pages/FleetService/FleetServiceDetails.js",
                "~/Scripts/Pages/FleetService/Equipment-Grid-Dropdown.js",
                "~/Scripts/Pages/FleetService/Part-grid-dropdown.js",
                "~/Scripts/Pages/FleetService/so-vendor-grid-dropdown.js",
                "~/Scripts/Pages/FleetService/Fleetissues-Grid-Dropdown.js"
               ));
            #endregion

            #region PreventiveMaintenance
            bundles.Add(new ScriptBundle("~/bundles/PreventiveMaintenance").Include(
               "~/Scripts/treeTable.js",
               "~/Scripts/Pages/Preventive/Preventive.js",
               "~/Scripts/Pages/Preventive/PreventiveDetails.js",
               "~/Scripts/Pages/Preventive/PrevEquipment-grid-dropdown.js",
               "~/Scripts/Pages/Preventive/Prev-location-grid-dropdown.js",
               "~/Scripts/Pages/Preventive/Prev-meter-grid-dropdown.js",
               "~/Scripts/Pages/Preventive/PrevPersonnel-grid-dropdown.js",
               "~/Scripts/Pages/PartLookup/partlookupWO.js", //V2-1151
               "~/Scripts/Common/LookupTypeTablePopup/TableVendorPopup.js",//V2-1151
               "~/Scripts/Common/LookupTypeTablePopup/TableAccountPoopup.js",
               "~/Scripts/Pages/Preventive/PrevMaintModelWizard.js" //V2-1204
               ));
            #endregion

            #region WorkorderNew
            bundles.Add(new ScriptBundle("~/bundles/WorkorderNew").Include(
               "~/Scripts/treeTable.js",
               "~/Scripts/Pages/WorkorderNew/WorkorderNew.js",
               "~/Scripts/Pages/WorkorderNew/WorkOrderDetails.js",
               "~/Scripts/Pages/WorkorderNew/woequipment-grid-dropdown.js",
               "~/Scripts/Pages/WorkorderNew/wo-location-grid-dropdown.js",
               "~/Scripts/Pages/WorkorderNew/wo-vendor-grid-dropdown.js",
               "~/Scripts/Pages/WorkorderNew/wo-part-grid-dropdown.js",
               "~/Scripts/Pages/WorkorderNew/wo-project-grid-dropdown.js",
               "~/Scripts/Pages/WorkorderNew/WorkOrderCompletionWizard.js",
               "~/Scripts/Pages/PartLookup/partlookupWO.js", //V2-690
               "~/Scripts/Common/LookupTypeTablePopup/TablePersonnelPoopupForPlanner.js", //V2-1076
               "~/Scripts/Common/LookupTypeTablePopup/TableVendorPopup.js",
               "~/Scripts/Common/LookupTypeTablePopup/TablePartCategoryMasterPopup.js"
               ));
            #endregion

            #region Workorder Approval
            bundles.Add(new ScriptBundle("~/bundles/WorkorderApproval").Include(
               "~/Scripts/treeTable.js",
               "~/Scripts/Pages/WorkorderNew/ApprovalWorkbench.js"
               ));
            #endregion

            #region Vendor
            bundles.Add(new ScriptBundle("~/bundles/Vendor").Include(
               "~/Scripts/treeTable.js",
               // "~/Scripts/Pages/equipment/jquery.selectlistactions.js",
               "~/Scripts/Pages/vendor/vendorSearch.js",
               "~/Scripts/Pages/vendor/vendorAdd.js",
               "~/Scripts/Common/LookupTypeTablePopup/TablePersonnelPoopup.js"
               ));
            #endregion

            #region Parts
            bundles.Add(new ScriptBundle("~/bundles/Parts").Include(
               "~/Scripts/Pages/part/parts.js",
               "~/Scripts/Pages/part/PartDetails.js",
               "~/Scripts/Pages/part/receiptgrid.js",
               "~/Scripts/Pages/InventoryCheckout/chkoutworkorder-grid-dropdown.js",
               "~/Scripts/Pages/InventoryCheckout/chkoutequipment-grid-dropdown.js",
               "~/Scripts/Pages/InventoryCheckout/chkoutlocation-grid-dropdown.js",
               "~/Scripts/Pages/InventoryCheckout/chkoutpart-grid-dropdown.js",
               "~/Scripts/Pages/part/partvendor-grid-dropdown.js",
               "~/Scripts/Pages/part/PartModelWizard.js",
                "~/Scripts/Pages/part/PartVendorAutoPuchasingConfiguration.js" //V2-1196
               ));
            bundles.Add(new ScriptBundle("~/bundles/PartsMobile").Include(
               "~/Scripts/mobile/js/mobiscroll.jquery.min.js",
               "~/Scripts/Pages/part/Mobile/PartsMobile.js",
               "~/Scripts/Pages/part/Mobile/partworkorder-grid-dropdown.js",
               "~/Scripts/Pages/part/Mobile/partequipment-grid-dropdown.js",
               "~/Scripts/Pages/part/Mobile/partlocation-grid-dropdown.js",
               "~/Scripts/Pages/part/Mobile/partvendor-grid-dropdown-mobile.js",
               "~/Scripts/Pages/part/Mobile/partequip-grid-mobile.js",
               "~/Scripts/Pages/part/Mobile/PartVendorAutoPuchasingConfiguration-mobile.js"
               ));
            #endregion

            #region Purchase Request
            bundles.Add(new ScriptBundle("~/bundles/PurchaseRequest").Include(
               "~/Scripts/treeTable.js",
               "~/Scripts/Pages/PurchaseRequest/PurchaseRequest.js",
               "~/Scripts/Pages/PurchaseRequest/PurchaseRequestDetail.js",
               "~/Scripts/Pages/PurchaseRequest/prvendor-grid-dropdown.js",
                "~/Scripts/Pages/PurchaseRequest/prpunchoutvendor-grid-dropdown.js",
               "~/Scripts/Pages/PartLookup/partlookup.js",
               "~/Scripts/Pages/PurchaseRequest/part-grid-dropdown.js",
               "~/Scripts/Pages/PurchaseRequest/prshoppingcartaccount-grid-dropdown.js",
               "~/Scripts/Pages/PurchaseRequest/prchargetolookup-grid-dropdown.js",
               "~/Scripts/Common/LookupTypeTablePopup/TablePurchaseVendorPopup.js",
               "~/Scripts/Common/LookupTypeTablePopup/TableAccountPoopup.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/PREditPopUp").Include(
                "~/Scripts/Pages/PurchaseRequest/prequipment-grid-dropdown.js",
                "~/Scripts/Common/LookupTypeTablePopup/TableWorkOrderPopup.js",
                "~/Scripts/Common/LookupTypeTablePopup/TablePartCategoryMasterPopup.js",
                "~/Scripts/Common/LookupTypeTablePopup/TablePartPopupLookup.js"
             ));
            #endregion

            #region Purchase Order
            bundles.Add(new ScriptBundle("~/bundles/PurchaseOrder").Include(
               "~/Scripts/treeTable.js",//V2-810
               "~/Scripts/Pages/Purchasing/Purchasing.js",
               "~/Scripts/Pages/Purchasing/PurchasingDetails.js",
               "~/Scripts/Pages/Purchasing/povendor-grid-dropdown.js",
               "~/Scripts/Pages/PartLookup/partlookupPO.js",
                "~/Scripts/Common/LookupTypeTablePopup/TablePurchaseVendorPopup.js",
               "~/Scripts/Common/LookupTypeTablePopup/TableAccountPoopup.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/POEditPopUp").Include(
              "~/Scripts/Pages/Purchasing/poequipment-grid-dropdown.js",
               "~/Scripts/Common/LookupTypeTablePopup/TableWorkOrderPopup.js",
                "~/Scripts/Common/LookupTypeTablePopup/TablePartPopupLookup.js",
                 "~/Scripts/Common/LookupTypeTablePopup/TablePartCategoryMasterPopup.js"
              ));
            #endregion

            #region SanitationJob
            bundles.Add(new ScriptBundle("~/bundles/SanitationJob").Include(
               "~/Scripts/treeTable.js",
               "~/Scripts/Pages/Sanitation/SanitationJob.js",
               "~/Scripts/Pages/Sanitation/SanitationJobDetails.js",
               "~/Scripts/Pages/Sanitation/asset-grid-popup.js",
               "~/Scripts/Pages/Sanitation/asset-grid-popup.js",
               "~/Scripts/Common/LookupTypeTablePopup/TablePersonnelPoopupForPlanner.js" //V2-1076
               ));
            bundles.Add(new ScriptBundle("~/bundles/SanitationMobile").Include(
              "~/Scripts/mobile/js/mobiscroll.jquery.min.js",
              "~/Scripts/Pages/Sanitation/Mobile/SanitationJobMobile.js",
              "~/Scripts/Pages/Sanitation/Mobile/SanitationJobMobileDetails.js",
               "~/Scripts/Pages/Sanitation/Mobile/sanitationequipment-grid-dropdown.js",
               "~/Scripts/Pages/Sanitation/Mobile/PersonnelPlanner-Grid-Dropdown.js" //V2-1076
              ));
            #endregion

            #region MasterSanitation
            bundles.Add(new ScriptBundle("~/bundles/MasterSanitation").Include(
                "~/Scripts/treeTable.js",
                "~/Scripts/Pages/MasterSanitationSchedule/MasterSanitationSchedule.js",
                "~/Scripts/Pages/MasterSanitationSchedule/MasterSanitationScheduleDetail.js",
                "~/Scripts/Pages/Sanitation/asset-grid-popup.js"
                ));

            #endregion

            #region PurchaseOrderReceipt
            bundles.Add(new ScriptBundle("~/bundles/PurchaseOrderReceipt").Include(
               "~/Scripts/Pages/PurchaseOrderReceipt/PurchaseOrderReceipt.js",
               "~/Scripts/Pages/PurchaseOrderReceipt/PurchaseOrderReceiptDetail.js"
               ));
            #endregion

            #region InvoiceMatching
            bundles.Add(new ScriptBundle("~/bundles/InvoiceMatching").Include(
               //"~/Scripts/Pages/Invoice/Invoice.js",
               "~/Scripts/Pages/Invoice/InvoiceNew.js",
               "~/Scripts/Pages/Invoice/InvoiceDetails.js",
               "~/Scripts/Pages/Invoice/invoiceVendor-grid-dropdown.js",
               "~/Scripts/Pages/Invoice/invoicePurchaseorder-grid-dropdown.js",
               "~/Scripts/Pages/Invoice/invoicePersonnel-grid-dropdown.js"
               ));
            #endregion

            #region Inventory Checkout
            bundles.Add(new ScriptBundle("~/bundles/InventoryCheckout").Include(
               "~/Scripts/treeTable.js",
               "~/Scripts/Pages/InventoryCheckout/InventoryCheckoutDetails.js",
               "~/Scripts/Pages/InventoryCheckout/chkoutworkorder-grid-dropdown.js",
               "~/Scripts/Pages/InventoryCheckout/chkoutequipment-grid-dropdown.js",
               "~/Scripts/Pages/InventoryCheckout/chkoutlocation-grid-dropdown.js",
               "~/Scripts/Pages/InventoryCheckout/chkoutpart-grid-dropdown.js"
               ));
            #endregion

            #region Inventory Part Issue V2-1031
            bundles.Add(new ScriptBundle("~/bundles/InventoryPartsIssue").Include(
               "~/Scripts/treeTable.js",
               "~/Scripts/Pages/InventoryPartsIssue/InventoryPartsIssue.js",
               "~/Scripts/Pages/InventoryPartsIssue/PartsIssueEquipment-grid-dropdown.js",
               "~/Scripts/Pages/InventoryPartsIssue/PartsIssuePart-grid-dropdown.js",
               "~/Scripts/Pages/InventoryPartsIssue/PartsIssueWorkorder-grid-dropdown.js"
               ));
            bundles.Add(new ScriptBundle("~/bundles/InventoryPartsIssueMobile").Include(
             "~/Scripts/mobile/js/mobiscroll.jquery.min.js",
             "~/Scripts/Pages/InventoryPartsIssue/Mobile/InventoryPartsIssueMobile.js",
              "~/Scripts/Pages/InventoryPartsIssue/Mobile/equipment-grid-dropdown.js",
               "~/Scripts/Pages/InventoryPartsIssue/Mobile/partIssueworkorder-grid-dropdown.js"
             ));
            #endregion
            #region Inventory PhysicalInventory
            bundles.Add(new ScriptBundle("~/bundles/InventoryPhysicalInventory").Include(
               "~/Scripts/Pages/InventoryPhysicalInventory/InventoryPhysicalInventory.js",
               "~/Scripts/Pages/InventoryPhysicalInventory/phyinv-part-grid-dropdown.js"
               ));
            bundles.Add(new ScriptBundle("~/bundles/InventoryPhysicalInventoryMobile").Include(
               "~/Scripts/mobile/js/mobiscroll.jquery.min.js",
               "~/Scripts/Pages/InventoryPhysicalInventory/Mobile/InventoryPhysicalInventoryMobile.js"
               ));
            #endregion

            #region Inventory Receipt
            bundles.Add(new ScriptBundle("~/bundles/InventoryReceipt").Include(
               "~/Scripts/Pages/InventoryReceipt/InventoryReceipt.js",
               "~/Scripts/Pages/InventoryReceipt/receipt-part-grid-dropdown.js"
               ));
            #endregion

            #region Labor Scheduling
            bundles.Add(new ScriptBundle("~/bundles/LaborScheduling").Include(
               "~/Scripts/Pages/LaborScheduling/LaborScheduling.js"
               ));
            #endregion

            #region Sanitation Approval WB
            bundles.Add(new ScriptBundle("~/bundles/SanitationApprovalWB").Include(
               "~/Scripts/Pages/Sanitation/SanitationApprovalWB.js"
               ));
            #endregion

            #region Sanitation Verification
            bundles.Add(new ScriptBundle("~/bundles/SanitationVerification").Include(
               "~/Scripts/Pages/Sanitation/SanitationVerification.js"
               ));
            #endregion

            //#region EventInfo
            //bundles.Add(new ScriptBundle("~/bundles/EventInfo").Include(
            //   "~/Scripts/treeTable.js",
            //   "~/Scripts/Pages/EventInfo/EventInfoSearch.js",
            //   "~/Scripts/Pages/EventInfo/EventInfoDetails.js"
            //   ));
            //#endregion

            #region IotEvent
            bundles.Add(new ScriptBundle("~/bundles/IotEvent").Include(
               "~/Scripts/treeTable.js",
               "~/Scripts/Pages/IotEvent/IoTEventSearch.js",
               "~/Scripts/Pages/IotEvent/IoTEventDetails.js"

               ));
            #endregion

            #region PartTransfer
            bundles.Add(new ScriptBundle("~/bundles/PartTransfer").Include(
               "~/Scripts/Pages/PartTransfer/PartTransferSearch.js",
               "~/Scripts/Pages/PartTransfer/PartTransferDetail.js"
               ));
            #endregion

            #region Reports
            bundles.Add(new ScriptBundle("~/bundles/Reports").Include(
               "~/Scripts/Pages/Reports/reports.js"
               ));
            #endregion

            #region PartsManagementRequest
            bundles.Add(new ScriptBundle("~/bundles/PartsManagementRequest").Include(
               "~/Scripts/Pages/PartsManagement/PartsManagementRequest/PartsManagementRequest.js",
               "~/Scripts/Pages/PartsManagement/PartsManagementRequest/PartsManagementRequestDetail.js"
               ));
            #endregion

            #region Labor Scheduling
            bundles.Add(new ScriptBundle("~/bundles/LaborSchedulingList").Include(
               "~/Scripts/Pages/NewLaborScheduling/NewLaborScheduling.js"
               ));
            bundles.Add(new ScriptBundle("~/bundles/LaborSchedulingCalendar").Include(
               "~/Scripts/Pages/NewLaborScheduling/NewLaborSchedulingCalendar.js"
               ));
            #endregion

            #region Work Order Planning
            bundles.Add(new ScriptBundle("~/bundles/WorkOrderPlanning").Include(
               "~/Scripts/treeTable.js",
               "~/Scripts/Pages/WorkOrderPlanning/WorkOrderPlanningSearch.js",
               "~/Scripts/Pages/WorkOrderPlanning/WorkOrderPlanningDetailsPlan.js",
               "~/Scripts/Pages/WorkOrderPlanning/WorkOrderPlanningResourceList.js",
               "~/Scripts/Pages/WorkOrderPlanning/WorkOrderPlanningResourceCalendar.js",
               "~/Scripts/Pages/WorkOrderPlanning/WorkOrderPlanningDashboard.js",
               "~/Scripts/Pages/WorkOrderPlanning/WorkOrderPlanningAvailableWork.js"
               ));
            #endregion

            #region Project
            bundles.Add(new ScriptBundle("~/bundles/Project").Include(
               "~/Scripts/treeTable.js",
               "~/Scripts/Pages/Project/ProjectSearch.js",
               "~/Scripts/Pages/Project/ProjectDetails.js",
               "~/Scripts/Pages/Project/TimelineDetails.js",
               "~/Scripts/Pages/Project/DashboardDetails.js"
               ));
            #endregion

            #region ProjectCosting
            bundles.Add(new ScriptBundle("~/bundles/ProjectCosting").Include(
               "~/Scripts/treeTable.js",
                "~/Scripts/Pages/ProjectCosting/ProjectCostingSearch.js",
                "~/Scripts/Pages/ProjectCosting/ProjectCostingDetails.js",
                 "~/Scripts/Pages/ProjectCosting/WorkOrderDetails.js",
                 "~/Scripts/Pages/ProjectCosting/DashboardDetails.js",
                 "~/Scripts/Pages/ProjectCosting/PurchasingDetails.js"

               ));
            #endregion

            #endregion

            #region Config
            #region SiteSetUp
            bundles.Add(new ScriptBundle("~/bundles/SiteSetUp").Include(
               "~/Scripts/Pages/Configuration/SiteSetUp/EditSiteSetUp.js",
               "~/Scripts/Pages/Configuration/SiteSetUp/SiteSetUpDetails.js"
               ));
            #endregion
            #region PreventiveMaintenanceLibrary
            bundles.Add(new ScriptBundle("~/bundles/PreventiveMaintenanceLibrary").Include(
               "~/Scripts/Pages/Configuration/PreventiveMaintenanceLibrary/PreventiveMaintenanceLibrary.js",
               "~/Scripts/Pages/Configuration/PreventiveMaintenanceLibrary/PreventiveMaintenanceLibraryDetails.js"
               ));
            #endregion
            #region SanitationOnDemandLibrary
            bundles.Add(new ScriptBundle("~/bundles/SanitationOnDemandLibrary").Include(
                "~/Scripts/Pages/SanitationOnDemandLibrary/SanitOnDemandLibSearch.js",
                "~/Scripts/Pages/SanitationOnDemandLibrary/SanitOnDemandLibDetails.js"
                ));
            #endregion
            #region LookUpLists
            bundles.Add(new ScriptBundle("~/bundles/LookUpLists").Include(
               "~/Scripts/Pages/Configuration/LookUpLists/LookUpLists.js"
               ));
            #endregion
            #region UserManagement
            bundles.Add(new ScriptBundle("~/bundles/UserManagement").Include(
               "~/Scripts/Pages/Configuration/UserManagement/UserManagement.js",
               "~/Scripts/Pages/Configuration/UserManagement/UserManagementDetails.js",
               "~/Scripts/Pages/Configuration/UserManagement/UserAccessDropdown.js"
               ));
            #endregion
            #region SensorAlert
            bundles.Add(new ScriptBundle("~/bundles/SensorAlert").Include(
               "~/Scripts/Pages/SensorAlert/SensorAlert.js",
               "~/Scripts/Pages/SensorAlert/SensorAlertDetails.js"
               ));
            #endregion
            #region EquipmentMaster
            bundles.Add(new ScriptBundle("~/bundles/EquipmentMaster").Include(
               "~/Scripts/Pages/Configuration/EquipmentMaster/EquipmentMasterSearch.js",
               "~/Scripts/Pages/Configuration/EquipmentMaster/EquipmentMasterDetails.js"
               ));
            #endregion
            #region PartMaster
            bundles.Add(new ScriptBundle("~/bundles/PartMaster").Include(
               "~/Scripts/Pages/Configuration/PartMaster/PartMasterSearch.js",
               "~/Scripts/Pages/Configuration/PartMaster/PartMasterDetails.js",
               "~/Scripts/Pages/Configuration/PartMaster/mm-grid-dropdown.js"
               ));
            #endregion
            #region VendorMaster
            bundles.Add(new ScriptBundle("~/bundles/VendorMaster").Include(
               "~/Scripts/Pages/Configuration/VendorMaster/VendorMasterSearch.js",
               "~/Scripts/Pages/Configuration/VendorMaster/VendorMasterDetails.js"
               ));
            #endregion
            #region ManufacturerMaster
            bundles.Add(new ScriptBundle("~/bundles/ManufacturerMaster").Include(
               "~/Scripts/Pages/Configuration/ManufacturerMaster/ManufacturerMasterSearch.js"
               ));
            #endregion
            //V2 666
            #region CategoryMaster
            bundles.Add(new ScriptBundle("~/bundles/CategoryMaster").Include(
               "~/Scripts/Pages/Configuration/CategoryMaster/CategoryMasterSearch.js"
               ));
            #endregion
            //V2 666

            #region FleetServiceTask
            bundles.Add(new ScriptBundle("~/bundles/FleetServiceTask").Include(
               "~/Scripts/Pages/Configuration/FleetServiceTask/ServiceTaskSearch.js"
               ));
            #endregion

            //V2-720
            #region ApprovalGroups
            bundles.Add(new ScriptBundle("~/bundles/ApprovalGroups").Include(
               "~/Scripts/Pages/Configuration/ApprovalGroups/ApprovalGroupSearch.js",
               "~/Scripts/Pages/Configuration/ApprovalGroups/ApprovalGroupsDetails.js"
               ));
            #endregion
            //V2-720
            #endregion

            #region Dashboard Configuration
            bundles.Add(new ScriptBundle("~/bundles/ConfigDashboard").Include(
               "~/Scripts/Pages/Configuration/Dashboard/LoginAuditUserInfo.js"
               ));
            bundles.Add(new ScriptBundle("~/bundles/PartsManagementRequest").Include(
              "~/Scripts/Pages/PartsManagement/PartsManagementRequest/PartsManagementRequest.js",
              "~/Scripts/Pages/PartsManagement/PartsManagementRequest/PartsManagementRequestDetail.js",
              "~/Scripts/Pages/PartsManagement/PartsManagementRequest/partm-id-grid-dropdown.js",
              "~/Scripts/Pages/PartsManagement/PartsManagementRequest/partrepl-id-grid-dropdown.js",
              "~/Scripts/Pages/PartsManagement/PartsManagementRequest/manufacturer-grid-dropdown.js"
              ));

            #region APMOnlyEventCountbyDisposition
            bundles.Add(new ScriptBundle("~/bundles/widgets/APMOnlyEventCountbyDisposition").Include(
                "~/Scripts/Pages/dashboard/Widgets/APMOnlyEventCountbyDisposition/APMOnlyEventCountbyDisposition.js"
                ));
            #endregion 

            #region APMOnlyEventCountbyFaultCode
            bundles.Add(new ScriptBundle("~/bundles/widgets/APMOnlyEventCountbyFaultCode").Include(
                "~/Scripts/Pages/dashboard/Widgets/APMOnlyEventCountbyFaultCode/APMOnlyEventCountbyFaultCode.js"
                ));
            #endregion 

            #region APMOnlyLiveTiles
            bundles.Add(new ScriptBundle("~/bundles/widgets/APMOnlyLiveTiles").Include(
                "~/Scripts/Pages/dashboard/Widgets/APMOnlyLiveTiles/APMOnlyLiveTiles.js"
                ));
            #endregion 

            #region EquipmentDownTime
            bundles.Add(new ScriptBundle("~/bundles/widgets/EquipmentDownTime").Include(
                "~/Scripts/Pages/dashboard/Widgets/EquipmentDownTime/EquipmentDownTime.js"
                ));
            #endregion 

            #region FleetLiveTiles
            bundles.Add(new ScriptBundle("~/bundles/widgets/FleetLiveTiles").Include(
                "~/Scripts/Pages/dashboard/Widgets/FleetLiveTiles/FleetLiveTiles.js"
                ));
            #endregion

            #region InventoryStatisticsbySite
            bundles.Add(new ScriptBundle("~/bundles/widgets/InventoryStatisticsbySite").Include(
               "~/Scripts/Pages/dashboard/Widgets/InventoryStatisticsbySite/InventoryStatisticsbySite.js"
               ));
            #endregion 

            #region InventoryValuation
            bundles.Add(new ScriptBundle("~/bundles/widgets/InventoryValuation").Include(
               "~/Scripts/Pages/dashboard/Widgets/InventoryValuation/InventoryValuation.js"
               ));
            #endregion 

            #region MaintenanceLiveTiles
            bundles.Add(new ScriptBundle("~/bundles/widgets/MaintenanceLiveTiles").Include(
               "~/Scripts/Pages/dashboard/Widgets/MaintenanceLiveTiles/MaintenanceLiveTiles.js"
               ));
            #endregion 

            #region MaintenanceStaticsbySite
            bundles.Add(new ScriptBundle("~/bundles/widgets/MaintenanceStaticsbySite").Include(
               "~/Scripts/Pages/dashboard/Widgets/MaintenanceStaticsbySite/MaintenanceStaticsbySite.js"
               ));
            #endregion 

            #region OpenWorkOrdersbySite
            bundles.Add(new ScriptBundle("~/bundles/widgets/OpenWorkOrdersbySite").Include(
               "~/Scripts/Pages/dashboard/Widgets/OpenWorkOrdersbySite/OpenWorkOrdersbySite.js"
               ));
            #endregion 

            #region OverduePMSbySite
            bundles.Add(new ScriptBundle("~/bundles/widgets/OverduePMSbySite").Include(
               "~/Scripts/Pages/dashboard/Widgets/OverduePMsbySite/OverduePMSbySite.js"
               ));
            #endregion 

            #region PurchasingStatisticsbySite
            bundles.Add(new ScriptBundle("~/bundles/widgets/PurchasingStatisticsbySite").Include(
               "~/Scripts/Pages/dashboard/Widgets/PurchasingStatisticsbySite/PurchasingStatisticsbySite.js"
               ));
            #endregion            

            #region SanitationOnlyJobsByStatus
            bundles.Add(new ScriptBundle("~/bundles/widgets/SanitationOnlyJobsByStatus").Include(
               "~/Scripts/Pages/dashboard/Widgets/SanitationOnlyJobsByStatus/SanitationOnlyJobsByStatus.js"
               ));
            #endregion

            #region SanitationOnlyJobsPassorFail
            bundles.Add(new ScriptBundle("~/bundles/widgets/SanitationOnlyJobsPassorFail").Include(
               "~/Scripts/Pages/dashboard/Widgets/SanitationOnlyJobsPassorFail/SanitationOnlyJobsPassOrFail.js"
               ));
            #endregion

            #region SanitationOnlyLiveTiles
            bundles.Add(new ScriptBundle("~/bundles/widgets/SanitationOnlyLiveTiles").Include(
               "~/Scripts/Pages/dashboard/Widgets/SanitationOnlyLiveTiles/SanitationOnlyLiveTiles.js"
               ));
            #endregion

            #region ServiceOrderBacklog
            bundles.Add(new ScriptBundle("~/bundles/widgets/ServiceOrderBacklog").Include(
               "~/Scripts/Pages/dashboard/Widgets/ServiceOrderBacklog/ServiceOrderBacklog.js"
               ));
            #endregion

            #region ServiceOrderLaborHours
            bundles.Add(new ScriptBundle("~/bundles/widgets/ServiceOrderLaborHours").Include(
               "~/Scripts/Pages/dashboard/Widgets/ServiceOrderLaborHours/ServiceOrderLaborHours.js"
               ));
            #endregion

            #region WOLaborHours
            bundles.Add(new ScriptBundle("~/bundles/widgets/WOLaborHours").Include(
               "~/Scripts/Pages/dashboard/Widgets/WOLaborHours/WOLaborHours.js"
               ));
            #endregion

            #region WorkOrderBacklogWidget
            bundles.Add(new ScriptBundle("~/bundles/widgets/WorkOrderBacklogWidget").Include(
               "~/Scripts/Pages/dashboard/Widgets/WorkOrderBacklogWidget/WorkOrderBacklogWidget.js"
               ));
            #endregion

            #region WorkOrdersbyPriority
            bundles.Add(new ScriptBundle("~/bundles/widgets/WorkOrdersbyPriority").Include(
               "~/Scripts/Pages/dashboard/Widgets/WorkOrdersbyPriority/WorkOrdersbyPriority.js"
               ));
            #endregion

            #region WorkOrdersbyType
            bundles.Add(new ScriptBundle("~/bundles/widgets/WorkOrdersbyType").Include(
               "~/Scripts/Pages/dashboard/Widgets/WorkOrdersbyType/WorkOrdersbyType.js"
               ));
            #endregion

            #region WorkOrderSource
            bundles.Add(new ScriptBundle("~/bundles/widgets/WorkOrderSource").Include(
               "~/Scripts/Pages/dashboard/Widgets/WorkOrderSource/WorkOrderSource.js"
               ));
            #endregion

            #region WOSourceType
            bundles.Add(new ScriptBundle("~/bundles/widgets/WOSourceType").Include(
               "~/Scripts/Pages/dashboard/Widgets/WOSourceType/WOSourceType.js"
               ));
            #endregion

            #region MaintenanceCompletionWorkbench
            bundles.Add(new ScriptBundle("~/bundles/widgets/MaintenanceCompletionWorkbench").Include(
               "~/Scripts/Pages/dashboard/Widgets/MaintenanceCompletionWorkbench/MaintenanceCompletionWorkbench.js",
               "~/Scripts/Pages/dashboard/Widgets/MaintenanceCompletionWorkbench/MaintenanceCompletionWorkbenchDetails.js",
               "~/Scripts/Pages/dashboard/Widgets/MaintenanceCompletionWorkbench/dashboardequipment-grid-dropdown.js",
               "~/Scripts/Pages/WorkorderNew/WorkOrderCompletionWizard.js",
               "~/Scripts/Common/LookupTypeTablePopup/TableAccountPoopup.js",//V2-1068
               "~/Scripts/Common/LookupTypeTablePopup/TableVendorPopup.js",//V2-1068
               "~/Scripts/Common/LookupTypeTablePopup/TablePartCategoryMasterPopup.js", //V2-1068
               "~/Scripts/Pages/PartLookup/partlookupWO.js", //V2-690
               "~/Scripts/ImageCompressor/Fengyuanchen_ImageCompressor.js",
               "~/Scripts/Common/LookupTypeTablePopup/TablePersonnelPoopupForPlanner.js" //V2-1076
               ));
            bundles.Add(new ScriptBundle("~/bundles/widgets/Mobile/MaintenanceCompletionWorkbench").Include(
               "~/Scripts/Pages/dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/MaintenanceCompletionWorkbench.js",
               "~/Scripts/Pages/dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/MaintenanceCompletionWorkbenchDetails.js",
               "~/Scripts/Pages/dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/dashboardequipment-grid-dropdown.js",
               "~/Scripts/Pages/dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/WorkOrderCompletionWizard.js",
               //"~/Scripts/Pages/dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/Account-Grid-Dropdown.js",
               "~/Scripts/Pages/dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/PersonnelPlanner-Grid-Dropdown.js",//V2-1076
               "~/Scripts/Pages/dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/Account-Grid-Dropdown.js",//V2-1068
               "~/Scripts/Pages/dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/Vendor-Grid-Dropdown.js", //V2-1068
               "~/Scripts/Pages/dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/PartCategoryMaster-Grid-Dropdown.js", //V2-1068
               "~/Scripts/QRScanner/html5-qrcode.min.js",
               "~/Scripts/mobile/js/mobiscroll.jquery.min.js",
               "~/Scripts/Pages/dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/PartLookup/partlookupWO.js", //V2-690
               "~/Scripts/ImageCompressor/Fengyuanchen_ImageCompressor.js"
               ));
            #endregion

            #region MaintenanceTechnicianScheduleCompliance
            bundles.Add(new ScriptBundle("~/bundles/widgets/MaintenanceTechnicianScheduleCompliance").Include(
               "~/Scripts/Pages/dashboard/Widgets/MaintenanceTechnicianScheduleCompliance/MaintenanceTechnicianScheduleCompliance.js"
               ));
            #endregion

            #endregion

            #region Meter
            bundles.Add(new ScriptBundle("~/bundles/Meter").Include(
               "~/Scripts/Pages/Meter/meter.js",
               "~/Scripts/Pages/Meter/meterDetails.js"
               ));
            #endregion

            #region ScheduledService
            bundles.Add(new ScriptBundle("~/bundles/FleetScheduledService").Include(
               "~/Scripts/Pages/FleetScheduledService/ScheduledServiceSearch.js",
               "~/Scripts/Pages/FleetScheduledService/Equipment-Grid-Dropdown.js"
               ));
            #endregion

            #region Devices
            bundles.Add(new ScriptBundle("~/bundles/devices").Include(
               "~/Scripts/Pages/Devices/devices.js",
               "~/Scripts/Pages/Devices/deviceDetails.js",
               "~/Scripts/Pages/Devices/gauge.min.js",
               "~/Scripts/Pages/Devices/device-asset-grid-dropdown.js",
               "~/Scripts/Pages/Devices/device-meter-grid-dropdown.js",
               "~/Scripts/Pages/Devices/device-SensorAlertProcedure-grid-dropdown.js"
               ));
            #endregion

            #region Fusion Chart
            bundles.Add(new ScriptBundle("~/bundles/FusionChart").Include(
            "~/Scripts/FusionChart/js/fusioncharts.js",
            "~/Scripts/FusionChart/js/themes/fusioncharts.theme.fusion.js",
            "~/Scripts/FusionChart/js/fusioncharts.charts.js",
            "~/Scripts/FusionChart/js/fusioncharts.excelexport.js",
            "~/Scripts/FusionChart/js/fusioncharts.timeseries.js",
            "~/Scripts/FusionChart/js/fusioncharts.overlappedbar2d.js",
             "~/Scripts/FusionChart/js/fusioncharts.overlappedcolumn2d.js"
            ));

            bundles.GetBundleFor("~/bundles/FusionChart").Transforms.Clear();
            #endregion

            #region Web-Cam
            bundles.Add(new ScriptBundle("~/bundles/WebCam").Include(
                "~/Scripts/Webcam/webcam-easy.js",
                "~/Scripts/Pages/WebCam/Webcam.js"));
            #endregion

            #region QR Scanner
            bundles.Add(new ScriptBundle("~/bundles/QRScanner").Include(
                 "~/Scripts/QRScanner/html5-qrcode.min.js"));
            #endregion

            #region CkEditor
            bundles.Add(new Bundle("~/bundles/SomaxCkEditor").Include(
                "~/Scripts/ckeditor/ckeditor.js",
                "~/Scripts/Pages/CkeditorMain.js"));
            #endregion

            #region Personnel
            bundles.Add(new ScriptBundle("~/bundles/personnel").Include(
           "~/Scripts/Pages/Personnel/personnel.js",
           "~/Scripts/Pages/Personnel/personneldetails.js"
           ));
            #endregion

            #region PartCycleCount
            bundles.Add(new ScriptBundle("~/bundles/PartCycleCount").Include(
           "~/Scripts/Pages/PartCycleCount/PartCycleCount.js"
           ));
            #endregion

            //V2-671
            #region CategoryMaster
            bundles.Add(new ScriptBundle("~/bundles/StoreroomSetup").Include(
               "~/Scripts/Pages/Configuration/StoreroomSetup/StoreroomSetupSearch.js"
               ));
            #endregion
            //end V2-671

            #region V2-670 MultiStoreroomPart
            bundles.Add(new ScriptBundle("~/bundles/MultiStoreroomPart").Include(
               "~/Scripts/Pages/MultiStoreroomPart/MultiStoreroomPart.js",
               "~/Scripts/Pages/MultiStoreroomPart/MultiStoreroomPartDetails.js",
               "~/Scripts/Common/LookupTypeTablePopup/TableVendorPopup.js",
               "~/Scripts/Common/LookupTypeTablePopup/TableAccountPoopup.js",
               "~/Scripts/Pages/InventoryCheckout/chkoutworkorder-grid-dropdown.js",
               "~/Scripts/Pages/InventoryCheckout/chkoutequipment-grid-dropdown.js",
               "~/Scripts/Pages/InventoryCheckout/chkoutlocation-grid-dropdown.js",
               "~/Scripts/Pages/InventoryCheckout/chkoutpart-grid-dropdown.js",
                "~/Scripts/Pages/MultiStoreroomPart/mspequipment-grid-dropdown.js",
                "~/Scripts/Pages/MultiStoreroomPart/MultiStoreroomPartModelWizard.js",
                "~/Scripts/Pages/MultiStoreroomPart/PartVendorAutoPuchasingConfiguration.js" //V2-1196
               ));
            #endregion

            #region V2-691 MaterialRequest
            bundles.Add(new ScriptBundle("~/bundles/MaterialRequest").Include(
               "~/Scripts/Pages/MaterialRequest/MaterialRequest.js",
               "~/Scripts/Common/LookupTypeTablePopup/TableAccountPoopup.js",
               "~/Scripts/Pages/PartLookup/partlookupWO.js", //V2-690,691
               "~/Scripts/Common/LookupTypeTablePopup/TableVendorPopup.js",//V2-1148
               "~/Scripts/Common/LookupTypeTablePopup/TablePartCategoryMasterPopup.js"//V2-1148
               ));
            #endregion

            #region V2-751 StoreroomTransfer
            bundles.Add(new ScriptBundle("~/bundles/StoreroomTransferIncomingTransfer").Include(
               "~/Scripts/Pages/StoreroomTransfers/StoreroomTransfers.js",
               "~/Scripts/Pages/StoreroomTransfers/StoreroomTransferPart-grid-dropdown.js"
              ));
            bundles.Add(new ScriptBundle("~/bundles/StoreroomTransferOutgoingTransfer").Include(
               "~/Scripts/Pages/StoreroomTransfers/StoreroomTransfersOutgoingTransfer.js"
              ));
            #endregion
            #region V2-769 Approval Module --Purchase Request ,Work Request ,Material Request
            bundles.Add(new ScriptBundle("~/bundles/ApprovalRequest").Include(
              "~/Scripts/Pages/Approval/ApprovalRequest.js"
             ));
            bundles.Add(new ScriptBundle("~/bundles/ApprovalPurchaseRequest").Include(
               "~/Scripts/Pages/Approval/ApprovalPurchaseRequest.js"
              ));
            bundles.Add(new ScriptBundle("~/bundles/ApprovalWorkRequest").Include(
            "~/Scripts/Pages/Approval/ApprovalWorkRequest.js"
           ));
            bundles.Add(new ScriptBundle("~/bundles/ApprovalMaterialRequest").Include(
                "~/Scripts/Pages/Approval/ApprovalMaterialRequest.js"
               ));
            #endregion

            #region BBU KPI Site
            bundles.Add(new ScriptBundle("~/bundles/BBUKPISite").Include(
               "~/Scripts/Pages/BBUKPISite/BBUKPISiteSearch.js",
               "~/Scripts/Pages/BBUKPISite/BBUKPISiteDetails.js"
              ));
            #endregion
            #region BBU KPI Site
            bundles.Add(new ScriptBundle("~/bundles/BBUKPIEnterprise").Include(
               "~/Scripts/Pages/BBUKPIEnterprise/BBUKPIEnterprise.js"
              ));
            #endregion
            #region VendorRequest V2-915
            bundles.Add(new ScriptBundle("~/bundles/VendorRequest").Include(
               "~/Scripts/Pages/VendorRequest/VendorRequestSearch.js"
               ));
            #endregion

            #region V2-1086 ShipToAddress
            bundles.Add(new ScriptBundle("~/bundles/ShipToAddress").Include(
               "~/Scripts/Pages/Configuration/ShipToAddress/ShipToAddressSearch.js"
               ));
            #endregion



            #region WorkRequestOnly V2-1100
            bundles.Add(new ScriptBundle("~/bundles/WorkRequestOnly/Mobile").Include(
              "~/Scripts/mobile/js/mobiscroll.jquery.min.js",
              "~/Scripts/Pages/dashboard/DashboardWROnly_Mobile/DashboardWROnly_Mobile.js",
               "~/Scripts/Pages/dashboard/DashboardWROnly_Mobile/DashboardWRSanitation_Mobile.js",
              "~/Scripts/Pages/dashboard/DashboardWROnly_Mobile/dashboardequipment-grid-dropdown.js",
              "~/Scripts/Pages/dashboard/DashboardWROnly_Mobile/Account-Grid-Dropdown.js",
              "~/Scripts/Pages/dashboard/DashboardWROnly_Mobile/PersonnelPlanner-Grid-Dropdown.js",
               "~/Scripts/QRScanner/html5-qrcode.min.js",
               "~/Scripts/treeTable.js",
                "~/Scripts/ImageCompressor/Fengyuanchen_ImageCompressor.js"
              ));
            #endregion

            #region GuestWorkRequest V2-1176
            bundles.Add(new ScriptBundle("~/bundles/GuestWorkRequest").Include(
                "~/Scripts/mcx-dialog.js",
                "~/Scripts/Select2/select2.full.min.js",
                "~/Scripts/Pages/CommonAlert.js",
                "~/Scripts/Pages/CommonLoader.js",
                "~/Scripts/Pages/GuestWorkRequest/GuestWorkRequest.js"
             ));
            #endregion
            #region PartIssue V2-1178
            bundles.Add(new ScriptBundle("~/bundles/PartIssue").Include(
              "~/Scripts/mcx-dialog.js",
                "~/Scripts/Common/sweetalert.min.js",
                "~/Scripts/jquery-ui-1.12.1.min.js",
                "~/Scripts/Common/tooltipvalidatormessage.js",
                "~/Scripts/Select2/select2.full.min.js",
                "~/Scripts/Pages/CommonAlert.js",
                 "~/Scripts/Pages/CommonLoader.js",
                "~/Scripts/Pages/PartIssue/workorder-grid-dropdown.js",
               "~/Scripts/Pages/PartIssue/equipment-grid-dropdown.js",              
               "~/Scripts/Pages/PartIssue/part-grid-dropdown.js",
                "~/Scripts/Pages/PartIssue/TablePersonnelPopup.js",
             "~/Scripts/Pages/PartIssue/PartIssue.js"
             ));

            bundles.Add(new ScriptBundle("~/bundles/PartIssueMobile").Include(
             "~/Scripts/mcx-dialog.js",
               "~/Scripts/Common/sweetalert.min.js",
               "~/Scripts/jquery-ui-1.12.1.min.js",
               "~/Scripts/Common/tooltipvalidatormessage.js",
               "~/Scripts/Select2/select2.full.min.js",
               "~/Scripts/Pages/CommonAlert.js",
                "~/Scripts/Pages/CommonLoader.js",
                "~/Scripts/mobile/js/mobiscroll.jquery.min.js",
               "~/Scripts/Pages/PartIssue/Mobile/workorder-grid-mobile.js",
                "~/Scripts/treeTable.js",              
              "~/Scripts/Pages/PartIssue/Mobile/equipment-grid-mobile.js",              
              "~/Scripts/Pages/PartIssue/Mobile/part-grid-mobile.js",
               "~/Scripts/Pages/PartIssue/Mobile/personnel-grid-mobile.js",
            "~/Scripts/Pages/PartIssue/Mobile/PartIssueMobile.js"
            ));
            #endregion
            #endregion

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
