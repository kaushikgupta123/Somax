var dtUserAccessTable;
var securityProfileId;
var securityProfileName = "";
var securityProfileDescription = "";
var APM = "";
var CMMS = "";
var Sanitation = "";
var Fleet = "";
$(document).on('click', '#openuseraccessgrid', function () {   
    generateUserAccessTable();
});
function generateUserAccessTable() {
    var rCount = 0;
    if ($(document).find('#UserAccessTable').hasClass('dataTable')) {
        dtUserAccessTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtUserAccessTable = $("#UserAccessTable").DataTable({
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        "pageLength": 10,
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Base/GetUserAccessList",
            data: function (d) {
                d.SecurityProfileName = securityProfileName;
                d.SecurityProfileDescription = securityProfileDescription;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                return json.data;
            }
        },
        "columns":
        [
            {
                "data": "SecurityProfileName", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<a class=link_eqpvendor_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            {
                "data": "SecurityProfileDescription", "autoWidth": true, "bSearchable": true, "bSortable": true,
            }
        ],
        initComplete: function () {
            $(document).find('#tbluseraccessfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#useraccessModal').hasClass('show')) {
                $(document).find('#useraccessModal').modal("show");
            }
            $('#UserAccessTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#UserAccessTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="eqp_vendor_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (securityProfileName) { $('#eqp_vendor_colindex_0').val(securityProfileName); }
                if (securityProfileDescription) { $('#eqp_vendor_colindex_1').val(securityProfileDescription); }
            });
            $(document).ready(function () {
                $(window).keydown(function (event) {
                    if (event.keyCode === 13) {
                        event.preventDefault();
                        return false;
                    }
                });
                $('#UserAccessTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                    if (e.keyCode === 13) {
                        var thisId = $(this).attr('id');
                        var colIdx = thisId.split('_')[1];
                        var searchText = LRTrim($(this).val());
                        securityProfileName = $('#eqp_vendor_colindex_0').val();
                        securityProfileDescription = $('#eqp_vendor_colindex_1').val();
                        dtUserAccessTable.page('first').draw('page');
                    }
                });
            });
        }
    });
}


$(document).on('click', '.link_eqpvendor_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtUserAccessTable.row(row).data();
    $(document).find('#userModels_SecurityProfileName,.securityprofileid').val(data.SecurityProfileName).removeClass('input-validation-error');
    $(document).find('#userModels_SecurityProfileId').val(data.SecurityProfileId);
    $(document).find('#UserType').val(data.UserType);
    $(document).find('#userModels_CMMSUser').val(data.CMMSUser);
    $(document).find('#userModels_SanitationUser').val(data.SanitationUser);
    $(document).find('#userModels_ProductGrouping').val(data.ProductGrouping);
    $(document).find('#userModels_PackageLevel').val(data.PackageLevel);
    $(document).find("#useraccessModal").modal('hide');
    UserTypeSelect(data.UserType);
});


$(document).on('click', '#openuserchangeaccessgrid', function () {
    generateChangeUserAccessTable();
});
function generateChangeUserAccessTable() {
    APM = $(document).find('#userChangeAccessModel_APM').val();
    CMMS = $(document).find('#userChangeAccessModel_CMMS').val();
    Sanitation = $(document).find('#userChangeAccessModel_Sanitation').val();
    Fleet = $(document).find('#userChangeAccessModel_Fleet').val();
    Production = $(document).find('#userChangeAccessModel_Production').val();
    
    if ($(document).find('#UserAccessTable').hasClass('dataTable')) {
        dtUserAccessTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtUserAccessTable = $("#UserAccessTable").DataTable({
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        "pageLength": 10,
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Base/GetChangeUserAccessList",
            data: function (d) {
                d.SecurityProfileName = securityProfileName;
                d.SecurityProfileDescription = securityProfileDescription;
                d.APM = APM;
                d.CMMS = CMMS;
                d.Sanitation = Sanitation;
                d.Fleet = Fleet;
                d.Production = Production;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                return json.data;
            }
        },
        "columns":
        [
            {
                "data": "SecurityProfileName", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<a class=link_securityprofile_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            {
                "data": "SecurityProfileDescription", "autoWidth": true, "bSearchable": true, "bSortable": true,
            }
        ],
        initComplete: function () {
            $(document).find('#tbluseraccessfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#useraccessModal').hasClass('show')) {
                $(document).find('#useraccessModal').modal("show");
            }
            $('#UserAccessTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#UserAccessTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="eqp_vendor_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (securityProfileName) { $('#eqp_vendor_colindex_0').val(securityProfileName); }
                if (securityProfileDescription) { $('#eqp_vendor_colindex_1').val(securityProfileDescription); }
            });
            $(document).ready(function () {
                $(window).keydown(function (event) {
                    if (event.keyCode === 13) {
                        event.preventDefault();
                        return false;
                    }
                });
                $('#UserAccessTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                    if (e.keyCode === 13) {
                        var thisId = $(this).attr('id');
                        var colIdx = thisId.split('_')[1];
                        var searchText = LRTrim($(this).val());
                        securityProfileName = $('#eqp_vendor_colindex_0').val();
                        securityProfileDescription = $('#eqp_vendor_colindex_1').val();
                        dtUserAccessTable.page('first').draw('page');
                    }
                });
            });
        }
    });
}


$(document).on('click', '.link_securityprofile_detail', function (e) { 
    var row = $(this).parents('tr');
    var data = dtUserAccessTable.row(row).data();
    $(document).find('#userChangeAccessModel_SecurityProfileName,.securityprofileid').val(data.SecurityProfileName).removeClass('input-validation-error');
    $(document).find('#userChangeAccessModel_SecurityProfileId').val(data.SecurityProfileId);
    $(document).find('#userChangeAccessModel_UserType').val(data.UserType);   
    $(document).find('#userChangeAccessModel_ProductGrouping').val(data.ProductGrouping);
    $(document).find('#userChangeAccessModel_PackageLevel').val(data.PackageLevel);
    $(document).find("#useraccessModal").modal('hide');
    UserTypeSelect(data.UserType);
});

