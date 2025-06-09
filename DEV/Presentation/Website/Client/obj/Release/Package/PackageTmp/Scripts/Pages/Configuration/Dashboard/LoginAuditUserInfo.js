////var loginAuditingRetrieveUserTable;
////$(function () {
////    LoginAuditingRetrieveUserGrid();
////}); 

////function LoginAuditingRetrieveUserGrid() { 
////    if ($(document).find('#loginAuditingRetrieveUsertable').hasClass('dataTable')) {
////        loginAuditingRetrieveUserTable.destroy();
////    }
////    loginAuditingRetrieveUserTable = $("#loginAuditingRetrieveUsertable").DataTable({
////        colReorder: true,
////        rowGrouping: true,
////        searching: true,
////        serverSide: true,
////        "pagingType": "full_numbers",
////        "bProcessing": true,
////        "bDeferRender": true,
////        "order": [[0, "asc"]],
////        stateSave: true,
////        language: {
////            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
////        },
////        sDom: 'Btlipr',
////        buttons: [],
////        "orderMulti": true,
////        "ajax": {
////            "url": "/Configuration/PopulateLoginAuditing",
////            "type": "POST",
////            "datatype": "json"
////        },
////        "columns":
////        [
////            { "data": "UserName", "autoWidth": true, "bSearchable": true, "bSortable": true },
////            { "data": "LogIn", "type": "date", "autoWidth": true, "bSearchable": true, "bSortable": true },
////            { "data": "IPAddress", "autoWidth": true, "bSearchable": true, "bSortable": true }
////        ],
////        initComplete: function () {
////            SetPageLengthMenu();
////        }
////    });
////}



