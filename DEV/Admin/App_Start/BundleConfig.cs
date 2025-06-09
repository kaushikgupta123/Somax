using System.Web;
using System.Web.Optimization;

namespace Admin
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
               "~/Scripts/Metronic/assets/demo/default/base/responsive.css",
               "~/Content/user-notification.css"
               ));
            #endregion

            #region Dashboard
            bundles.Add(new StyleBundle("~/Content/dashboardStyle").Include(
               "~/Content/Dashboard/dashboard.css"
               ));
            #endregion

            #region Common
            bundles.Add(new StyleBundle("~/Content/datatable").Include(
                "~/Scripts/GridLib/jquery.dataTables.min.css",
                "~/Scripts/GridLib/buttons.dataTables.min.css",
                "~/Scripts/GridLib/fixedColumns.dataTables.min.css",
                "~/Scripts/GridLib/customDataTable.css",
                "~/Scripts/css/bootstrap-multiselect.css"
                ));

            bundles.Add(new StyleBundle("~/Content/Common").Include(
              "~/Scripts/css/sweetalert.css",
              "~/Scripts/css/tootltipcustom.css",
              "~/Scripts/css/jquery-ui.css",
              "~/Scripts/css/custom-dropdown-style.css"
              ));
            bundles.Add(new StyleBundle("~/Content/daterangepickercustom").Include(
             "~/Scripts/css/daterangepicker_theme.css",
             "~/Scripts/css/daterangepicker_custom.css"
             ));
            #endregion

            #region Login
            bundles.Add(new StyleBundle("~/Content/passwordcreate").Include(
                 "~/Scripts/Metronic/assets/login/vendors/base/vendors.bundle.css",
                 "~/Scripts/Metronic/assets/login/demo/default/base/responsive.css",
                 "~/Scripts/Metronic/assets/login/demo/default/base/style.bundle.css"
              ));

            bundles.Add(new StyleBundle("~/Content/AdminLogin").Include(
                 "~/Scripts/css/vendors.bundle.css",
                 "~/Scripts/css/style.bundle.css",
                 "~/Scripts/css/admin_login.css"
              ));
            #endregion

            #endregion

            #region Script-Bundle

            #region CreatePassword
            bundles.Add(new ScriptBundle("~/bundles/CreatePassword").Include(
             "~/Scripts/Metronic/assets/vendors/base/vendors.bundle.min.js",
             "~/Scripts/Metronic/assets/demo/demo3/base/scripts.bundle.js",
             "~/Scripts/js/CreateNewPassword.js"
             ));
            #endregion

            #region Layout
            bundles.Add(new ScriptBundle("~/bundles/LayoutScript").Include(
              "~/Scripts/mcx-dialog.js",
              "~/Scripts/Metronic/assets/vendors/base/vendors.bundle.min.js",
              "~/Scripts/Metronic/assets/demo/demo3/base/scripts.bundle.js",
              "~/Scripts/Metronic/assets/login/demo/default/base/jquery.mCustomScrollbar.concat.min.js",
              "~/Scripts/Metronic/assets/demo/default/custom/components/forms/widgets/input-mask.js",
              "~/Scripts/Somax/AdminSomax_main.js",
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
                "~/Scripts/GridLib/jquery.dataTables.min.js",
                "~/Scripts/GridLib/dataTables.colReorder.min.js",
                "~/Scripts/GridLib/dataTables.bootstrap4.min.js",
                "~/Scripts/GridLib/dataTables.buttons.min.js",
                "~/Scripts/GridLib/pdfmake.min.js",
                "~/Scripts/GridLib/vfs_fonts.js",
                //"~/Scripts/Metronic/GridLib/buttons.flash.min.js",
                "~/Scripts/GridLib/buttons.html5.min.js",
                "~/Scripts/GridLib/buttons.print.min.js",
                "~/Scripts/GridLib/dataTables.fixedColumns.min.js",
                 // "~/Scripts/buttons.colVis.min.js",
                 "~/Scripts/GridLib/jszip.min.js",
                 "~/Scripts/Somax/jquery.selectlistactions.js"
                //"~/Scripts/Pages/equipment/jquery.selectlistactions.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/imagezoom").Include(
             "~/Scripts/ImageZoom/jquery.elevateZoom.js", // don't replace this file as it is customized (line no 260-266 and 326-332)
             "~/Scripts/dropzone/dropzone.js"
             ));
            bundles.Add(new ScriptBundle("~/bundles/QRPrint").Include(
              "~/Scripts/jspdf.min.js",
              "~/Scripts/html2canvas.min.js"
              ));
            #endregion

            #region Login
            bundles.Add(new ScriptBundle("~/bundles/Login").Include(
             "~/Scripts/Metronic/assets/vendors/base/vendors.bundle.min.js",
             "~/Scripts/Metronic/assets/demo/demo3/base/scripts.bundle.js",
             "~/Scripts/Metronic/assets/snippets/pages/user/login.js"
             ));

            bundles.Add(new ScriptBundle("~/bundles/AdminLogin").Include(
             "~/Scripts/js/vendors.bundle.js",
             "~/Scripts/js/scripts.bundle.js",
             "~/Scripts/js/login.js"
             ));
            #endregion

            #region Client
            bundles.Add(new ScriptBundle("~/bundles/Client").Include(
             "~/Scripts/treeTable.js",
             "~/Scripts/Pages/Client/ClientSearch.js",
             "~/Scripts/Pages/Client/ClientDetails.js"

             ));
            #region V2-964
            bundles.Add(new ScriptBundle("~/bundles/ClientNew").Include(
             "~/Scripts/treeTable.js",
             "~/Scripts/Pages/ClientNew/ClientNew.js",
             "~/Scripts/Pages/ClientNew/ClientDetailsNew.js"
             ));
            #endregion
            #endregion

            #region Site
            bundles.Add(new ScriptBundle("~/bundles/Site").Include(
            "~/Scripts/Pages/Site/SiteSearch.js"

             ));
            #endregion

            #region Knowledgebase
            bundles.Add(new ScriptBundle("~/bundles/Knowledgebase").Include(
               "~/Scripts/treeTable.js",
                "~/Scripts/Pages/Knowledgebase/KnowledgebaseSearchNew.js",
               "~/Scripts/Pages/Knowledgebase/KnowledgebaseDetails.js"
               ));
            #endregion

            #region SupportTicket
            bundles.Add(new ScriptBundle("~/bundles/SupportTicket").Include(
             "~/Scripts/treeTable.js",
             "~/Scripts/Pages/SupportTicket/SupportTicket.js"
             ));
            #endregion
            #region UserManagement
            bundles.Add(new ScriptBundle("~/bundles/UserManagement").Include(
               "~/Scripts/Pages/UserManagement/UserManagement.js",
               "~/Scripts/Pages/UserManagement/UserManagementDetails.js"
               ));
            #endregion
            #endregion

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = false;
#endif

        }
    }
}
