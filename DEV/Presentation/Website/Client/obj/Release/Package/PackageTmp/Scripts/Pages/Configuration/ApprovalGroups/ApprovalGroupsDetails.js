//#region Common
function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "ApprovalGroupDetails":
            generateAppGroupApprovalTable();
            break;
        case "ApprovalGroupRequestors":
            generateAppGroupRequestorsTable();
            break;
    }
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";

    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
}
$(document).on('click', "ul.vtabs li", function () {
    $("ul.vtabs li").removeClass("active");
    $(this).addClass("active");
    $(".tabsArea").hide();
    var activeTab = $(this).find("a").attr("href");
    $(activeTab).fadeIn();
    return false;
});
//#endregion

//#region Details tab

//#region Grid
var AppGroupApprovalDetailTable;
var PurchaseRequestConst = "PurchaseReq";
function generateAppGroupApprovalTable() {
    //var printCounter = 0;
    var rCount = 0;
    if ($(document).find('#AppGroupApprovalDetail').hasClass('dataTable')) {
        AppGroupApprovalDetailTable.destroy();
    }
    AppGroupApprovalDetailTable = $('#AppGroupApprovalDetail').DataTable({
        //colReorder: {
        //    fixedColumnsLeft: 1,
        //    fixedColumnsRight: 1
        //},
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        //"stateSaveCallback": function (settings, data) {
        //    if (run == true) {
        //        $.ajax({
        //            "url": "/Base/CreateUpdateState",
        //            "data": {
        //                GridName: "ApprovalGroup_Search",
        //                LayOutInfo: JSON.stringify(data)
        //            },
        //            "dataType": "json",
        //            "type": "POST",
        //            "success": function () { return; }
        //        });
        //    }
        //    run = false;
        //},
        //"stateLoadCallback": function (settings, callback) {
        //    $.ajax({
        //        "url": "/Base/GetState",
        //        "data": {
        //            GridName: "ApprovalGroup_Search",
        //        },
        //        "async": false,
        //        "dataType": "json",
        //        "success": function (json) {
        //            if (json) {
        //                callback(JSON.parse(json));
        //            }
        //            else {
        //                callback(json);
        //            }
        //        }
        //    });
        //},
        scrollX: true,
        //fixedColumns: {
        //    leftColumns: 1
        //},
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Approval Group'
            },
            {
                extend: 'print',
                title: 'Approval Group',
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Approval Group',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                title: 'Approval Group',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/ApprovalGroups/AppGroupApproverForDetailsPage",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                //d.RequestType = LRTrim($('#RequestType').val());
                //d.Description = LRTrim($('#Description').val());
                d.ApprovalGroupId = $('#ApprovalGroupsModel_ApprovalGroupId').val();
            },
            "dataSrc": function (result) {
                let colOrder = ApprovalGroupTable.order();
                orderDir = colOrder[0][1];
                order = colOrder[0][0];

                //if (result.data.length == "0") {
                //    $(document).find('.import-export').attr('disabled', 'disabled');
                //}
                //else {
                //    $(document).find('.import-export').removeAttr('disabled');
                //}
                rCount = result.data.length;
                if (rCount > 0) {
                    $('#btnAddAppGroupApproval').hide();
                }
                else {
                    $('#btnAddAppGroupApproval').show();
                }
                return result.data;
            },
            complete: function () {
                CloseLoader();
                //$("#ApprovalGroupSearchAction :input").not('.import-export').removeAttr("disabled");
                //$("#ApprovalGroupSearchAction :button").not('.import-export').removeClass("disabled");
            },
            global: true
        },
        "columns":
            [
                { "data": "ApproverName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1", },
                { "data": "Limit", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2", "class": "text-right" },
                { "data": "LevelName", "autoWidth": true, "bSearchable": true, "bSortable": false },
                { "data": "", "bSearchable": false, "bSortable": false, "class": "text-center" },
            ],
        columnDefs: [
            {
                "data": null,
                targets: [3], render: function (a, b, data, d) {
                    if ($('#ApprovalGroupsModel_RequestType').val().toLowerCase() == PurchaseRequestConst.toLowerCase()) {
                        return '<div class="text-wrap"><a class="btn btn-outline-primary gridbtnAddAppGroupApproval gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success gridbtnEditAppGroupApproval gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger gridbtnDeleteAppGroupApproval gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return '<div class="text-wrap"><a class="btn btn-outline-primary gridbtnAddAppGroupApproval gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-danger gridbtnDeleteAppGroupApproval gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }

                }
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
        },
        drawCallback: function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;
            var groupColumn = 2;//for level name group
            api.column(groupColumn).visible(false);
            api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                if (last !== group) {
                    $(rows).eq(i).before('<tr class="group"><td colspan="4">' + group + '</td></tr>');

                    last = group;
                }
            });
        },
    });
};
//#endregion

//#region Add,Edit delete
$(document).on('click', '#btnAddAppGroupApproval', function () {
    AddEditAppGroupApproval(0);
});
$(document).on('click', '.gridbtnAddAppGroupApproval', function () {
    AddEditAppGroupApproval(0);
});
$(document).on('click', '.gridbtnEditAppGroupApproval', function () {
    var data = AppGroupApprovalDetailTable.row($(this).parents('tr')).data();
    AddEditAppGroupApproval(data.AppGroupApproversId);
});
function AddEditAppGroupApproval(AppGroupApproversId) {
    $.ajax({
        url: "/ApprovalGroups/AddEditAppGroupApprovalView",
        type: "POST",
        dataType: 'html',
        data: {
            "ApprovalGroupId": $('#ApprovalGroupsModel_ApprovalGroupId').val(),
            "RequestType": $('#ApprovalGroupsModel_RequestType').val(),
            "AppGroupApproversId": AppGroupApproversId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddEditApproverModalDialog').html(data);
            $('#AddEditApproverModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            //$('#AddEditApproverModalpopup').modal('show');
            SetAGControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('hidden.bs.modal', '#AddEditApproverModalDialog', function () {
    $('.ui-tooltip1').hide();
    $('#AddEditApproverModalpopup').html('');
});
function AppGroupApprovalOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("approverSaveAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("approverUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            AppGroupApprovalDetailTable.page('first').draw('page');
        });
        $('#AddEditApproverModalpopup').modal('hide');

    }
    else {
    /*    ShowGenericErrorOnAddUpdate(data);*/
        ShowErrorAlert(data);
    }
};

$(document).on('click', '.gridbtnDeleteAppGroupApproval', function () {
    var data = AppGroupApprovalDetailTable.row($(this).parents('tr')).data();
    DeleteAppGroupApproval(data.AppGroupApproversId);
});
function DeleteAppGroupApproval(AppGroupApproversId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: "/ApprovalGroups/DeleteAppGroupApproval",
            type: "POST",
            dataType: 'json',
            data: {
                "ApprovalGroupId": $('#ApprovalGroupsModel_ApprovalGroupId').val(),
                "AppGroupApproversId": AppGroupApproversId
            },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowSuccessAlert(getResourceValue("approverDeleteAlert"));
                    AppGroupApprovalDetailTable.page('first').draw('page');
                }
                else {
                    ShowErrorAlert(data);
                }
            },
            complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    });
}

$(document).on('click', ".btncancel", function (e) {
    var Id = "";
    $(document).find('#AddEditApproverModalpopup select').each(function (i, item) {
        Id = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + Id).hide();
    });
})
$(document).on("change", "#appGroupApproverModel_Level", function () {
    var tlen = $(document).find("#appGroupApproverModel_Level").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#appGroupApproverModel_Level").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#appGroupApproverModel_Level").removeClass("input-validation-error");
    }
    else {
        var areaddescribedby = $(document).find("#appGroupApproverModel_Level").attr('aria-describedby');
        $('#' + areaddescribedby).show();
        $(document).find('form').find("#appGroupApproverModel_Level").addClass("input-validation-error");
    }
});
$(document).on("change", "#appGroupApproverModel_ApproverId", function () {
    var tlen = $(document).find("#appGroupApproverModel_ApproverId").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#appGroupApproverModel_ApproverId").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#appGroupApproverModel_ApproverId").removeClass("input-validation-error");
    }
    else {
        var areaddescribedby = $(document).find("#appGroupApproverModel_ApproverId").attr('aria-describedby');
        $('#' + areaddescribedby).show();
        $(document).find('form').find("#appGroupApproverModel_ApproverId").addClass("input-validation-error");
    }
});
//#endregion

//#endregion

//#region Requestors tab**************
//#region Grid
var AppGroupRequestorsTable;
function generateAppGroupRequestorsTable() {
    var rCount = 0;
    if ($(document).find('#AppGroupRequestors').hasClass('dataTable')) {
        AppGroupRequestorsTable.destroy();
    }
    AppGroupRequestorsTable = $('#AppGroupRequestors').DataTable({
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        scrollX: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Approval Group'
            },
            {
                extend: 'print',
                title: 'Approval Group',
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Approval Group',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                title: 'Approval Group',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/ApprovalGroups/AppGroupRequestors",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.ApprovalGroupId = $('#ApprovalGroupsModel_ApprovalGroupId').val();
            },
            "dataSrc": function (result) {
                let colOrder = AppGroupRequestorsTable.order();
                orderDir = colOrder[0][1];
                order = colOrder[0][0];
                rCount = result.data.length;
                return result.data;
            },
            complete: function () {
                CloseLoader();
            },
            global: true
        },
        "columns":
            [
                { "data": "Requestor", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1", },
            ],
        columnDefs: [
            {
                "data": null,
                targets: [1], render: function (a, b, data, d) {
                    if (true) {
                        return '<div class="text-wrap center">' +
                            '<a class="btn btn-outline-danger gridbtnDeleteAppGroupRequestors gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return "";
                    }

                }
            }
        ],
        initComplete: function () {
            //if (rCount > 0) {
            //    $('#btnAddAppGroupApproval').hide();
            //}
            //else {
            $('#btnAddRequestor').show();
            //}
            SetPageLengthMenu();
        }
    });
};
//#endregion

//#region Delete Requestor
$(document).on('click', '.gridbtnDeleteAppGroupRequestors', function () {
    var row = $(this).parents('tr');
    var data = AppGroupRequestorsTable.row(row).data();
    var AppGroupRequestorsId = data.AppGroupRequestorsId;
    DeleteRequestor(AppGroupRequestorsId);
});
function DeleteRequestor(AppGroupRequestorsId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/ApprovalGroups/AppGroupDeleteRequestor',
            data: {
                AppGroupRequestorsId: AppGroupRequestorsId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("requestoeDeleteAlert"));
                    AppGroupRequestorsTable.page('first').draw('page');
                }
                else {
                    ShowErrorAlert(data);
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
//#endregion delete requestor

//#region Add Requestors Popup
var Requestorstotalcount = 0;
$(document).on('click', '#btnAddRequestor', function () {
    Requestorstotalcount = 0;
    SelectPersonalIDs = [];
    SelectPersonalIDsDetails = [];
    generateAddRequestorsDataTable();
    $('.PersonnelLookupGriditemcount').text('0');
});
var RequestorTable;
var PersonnelClientLookupID = "";
var Name = "";
var AssetGroup1 = "";
var AssetGroup2 = "";
var AssetGroup3 = "";
var SelectPersonalIDs = [];
var SelectPersonalIDsDetails = [];
var appGrpLookuporder = '1';
var appGrpLookuporderDir = 'asc';
function generateAddRequestorsDataTable() {
    var RequestorCount = 0;
    var visibility;
    if ($(document).find('#AddRequestorTable').hasClass('dataTable')) {
        RequestorTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    RequestorTable = $("#AddRequestorTable").DataTable({
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        "pageLength": 10,
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/ApprovalGroups/AddrequestorListchunksearch_AutoAddRequestorGeneration",
            data: function (d) {
                d.clientLookupId = LRTrim(PersonnelClientLookupID);
                d.name = LRTrim(Name);
                d.AssetGroup1 = LRTrim(AssetGroup1);
                d.AssetGroup2 = LRTrim(AssetGroup2);
                d.AssetGroup3 = LRTrim(AssetGroup3);
                d.requestType = $('#ApprovalGroupsModel_RequestType').val();
                d.ApprovalGroupId = $('#ApprovalGroupsModel_ApprovalGroupId').val();//added for filter
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                RequestorCount = json.data.length;
                Requestorstotalcount = json.recordsTotal;
                if (SelectPersonalIDsDetails.length == Requestorstotalcount && Requestorstotalcount != 0) {
                    $(document).find('.dt-body-center').find('#PersonnelIDsearch-select-all').prop('checked', true);
                }
                else {
                    $(document).find('.dt-body-center').find('#PersonnelIDsearch-select-all').prop('checked', false);
                }
                return json.data;
            }
        },
        "columns":
            [{
                "data": "PersonnelId",
                orderable: false,
                "bSortable": false,
                "autoWidth": false,
                className: 'select-checkbox dt-body-center',
                'render': function (data, type, full, meta) {
                    if ($('#PersonnelIDsearch-select-all').is(':checked') && Requestorstotalcount == SelectPersonalIDs.length) {
                        return '<input type="checkbox" checked="checked"  data-prid="' + data + '" class="chkPersonalIDs ' + data + '"  value="'
                            + $('<div/>').text(data).html() + '">';
                    } else {
                        if (SelectPersonalIDs.indexOf(data) != -1) {
                            return '<input type="checkbox" checked="checked" data-prid="' + data + '" class="chkPersonalIDs ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                        else {
                            return '<input type="checkbox"  data-prid="' + data + '" class="chkPersonalIDs ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                    }
                }
            },
            {
                "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,

            },
            {
                "data": "FullName", "autoWidth": true, "bSearchable": true, "bSortable": true,
            },
            {
                //"data": "AssetGroup1", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "data": "AssetGroup1ClientlookUpId", "autoWidth": true, "bSearchable": true, "bSortable": true,
            },
            {
                //"data": "AssetGroup2", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "data": "AssetGroup2ClientlookUpId", "autoWidth": true, "bSearchable": true, "bSortable": true,
            },
            {
               //"data": "AssetGroup3", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "data": "AssetGroup3ClientlookUpId", "autoWidth": true, "bSearchable": true, "bSortable": true,
            }
            ],
        columnDefs: [
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            $(document).find('#tblAddRequestorTablefooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#PersonnelApprovalPopupModal').hasClass('show')) {
                $(document).find('#PersonnelApprovalPopupModal').modal("show");
            }
            $('#AddRequestorTable tfoot th').each(function (i, v) {
                var colIndex = i;
                if (colIndex > 0) {
                    var title = $('#AddRequestorTable thead th').eq($(this).index()).text();
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                    if (PersonnelClientLookupID) { $('#colindex_1').val(PersonnelClientLookupID); }
                    if (Name) { $('#colindex_2').val(Name); }
                    if (AssetGroup1) { $('#colindex_3').val(AssetGroup1); }
                    if (AssetGroup2) { $('#colindex_4').val(AssetGroup2); }
                    if (AssetGroup3) { $('#colindex_5').val(AssetGroup3); }
                }
            });

            $('#AddRequestorTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    PersonnelClientLookupID = $('#colindex_1').val();
                    Name = $('#colindex_2').val();
                    AssetGroup1 = $('#colindex_3').val();
                    AssetGroup2 = $('#colindex_4').val();
                    AssetGroup3 = $('#colindex_5').val();

                    RequestorTable.page('first').draw('page');
                }
            });
        }
    });
}
//#endregion end add requestors

//#region Process Requestors To Add
$(document).on('click', '#btnProcessRequestorsToAdd', function () {

    if (SelectPersonalIDs.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var approvalGroupId = $("#ApprovalGroupsModel_ApprovalGroupId").val();
        var jsonResult = {
            "SelectPersonalIDsItem": SelectPersonalIDs, "approvalGroupId": approvalGroupId
        }
        {
            $.ajax({
                url: '/ApprovalGroups/ProcessRequestorsToAddItem',
                data: jsonResult,
                type: "POST",
                datatype: "json",
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (result) {
                    if (result.success == true) {
                        $(document).find('#PersonnelApprovalPopupModal').modal("hide");
                        SuccessAlertSetting.text = getResourceValue("RequestorAddedAlert");
                        swal(SuccessAlertSetting, function () {
                            //RedirectToAGDetail(data.MaterialRequestId);
                        });
                    }
                    else {
                        CloseLoader();
                        return false;
                    }

                },
                complete: function () {
                    CloseLoader();
                    $(document).find('.dt-body-center').find('#PersonnelIDsearch-select-all').prop('checked', false);
                    $(document).find('.chkPersonalIDs').prop('checked', false);
                    $('.PersonnelLookupGriditemcount').text(0);
                    SelectPersonalIDs = [];
                    SelectPersonalIDsDetails = [];
                    AppGroupRequestorsTable.page('first').draw('page');
                }
            });
        }
    }
});
//#endregion end Process

//#region check box
$(document).on('change', '.chkPersonalIDs', function () {
    $(".actionBar").hide();
    var data = RequestorTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        var el = $('#PersonnelIDsearch-select-all').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        var index = SelectPersonalIDs.indexOf(data.PersonnelId);
        SelectPersonalIDs.splice(index, 1);
        SelectPersonalIDsDetails.splice(index, 1);
    }
    else {
        var item = data.PersonnelId;
        var found = SelectPersonalIDsDetails.some(function (el) {
            return el.PersonnelId === data.PersonnelId;
        });
        if (!found) {
            SelectPersonalIDs.push(item);
            SelectPersonalIDsDetails.push(item);
        }

    }
    if (SelectPersonalIDsDetails.length == Requestorstotalcount) {
        $(document).find('.dt-body-center').find('#PersonnelIDsearch-select-all').prop('checked', 'checked');
    }
    else {
        $(document).find('.dt-body-center').find('#PersonnelIDsearch-select-all').prop('checked', false);
    }

    $('.PersonnelLookupGriditemcount').text(SelectPersonalIDs.length);

});
//#endregion end check box

//#region Select All check box
$(document).on('click', '#PersonnelIDsearch-select-all', function (e) {
    SelectPersonalIDs = [];
    SelectPersonalIDsDetails = [];

    RequestorTable = $("#AddRequestorTable").DataTable();
    var colname = appGrpLookuporder;
    var coldir = appGrpLookuporderDir;
    var checked = this.checked;
    $.ajax({
        "url": "/ApprovalGroups/ProcessRequestorsToAddItemsSelectAll",
        data:
        {
            colname: colname,
            coldir: coldir,
            clientLookupId: LRTrim(PersonnelClientLookupID),
            name: LRTrim(Name),
            AssetGroup1: LRTrim(AssetGroup1),
            AssetGroup2: LRTrim(AssetGroup2),
            AssetGroup3: LRTrim(AssetGroup3),
            requestType: $('#ApprovalGroupsModel_RequestType').val(),
            ApprovalGroupId: $('#ApprovalGroupsModel_ApprovalGroupId').val()//added for filter
        },
        async: true,
        type: "GET",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    if (checked) {
                        var exist = $.grep(SelectPersonalIDsDetails, function (obj) {
                            return obj.PersonnelId === item.PersonnelId;
                        });
                        if (exist.length == 0) {
                            SelectPersonalIDsDetails.push(item.PersonnelId);
                            SelectPersonalIDs.push(item.PersonnelId);
                        }
                    }
                    else {
                        SelectPersonalIDsDetails = $.grep(SelectPersonalIDsDetails, function (obj) {
                            return obj.WorkOrderId !== item.PersonnelId;
                        });

                        var i = SelectPersonalIDs.indexOf(item.PersonnelId);
                        SelectPersonalIDs.splice(i, 1);
                    }

                });
            }
        },
        complete: function () {
            $('.PersonnelLookupGriditemcount').text(SelectPersonalIDsDetails.length);

            if (checked) {

                $(document).find('.dt-body-center').find('.chkPersonalIDs').prop('checked', 'checked');

            }
            else {
                $(document).find('.dt-body-center').find('.chkPersonalIDs').prop('checked', false);


            }
            CloseLoader();
        }
    });
});
//#endregion Select All check box

//#endregion Requestors tab********
//#region Description popup
$(document).on('click', '#approvalGroupDescription', function () {
    $(document).find('#ApprovalGroupsDetaildesmodaltext').text($(this).data("des"));
    $(document).find('#ApprovalGroupsDetaildesmodal').modal('show');
});
//#endregion