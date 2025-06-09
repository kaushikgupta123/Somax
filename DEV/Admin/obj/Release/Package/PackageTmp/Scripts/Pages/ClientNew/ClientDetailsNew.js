//#region Event Log
var order_forEventLog = '1';
var orderDir_forEventLog = 'asc';
var ClientEventLogTable;

function GenerateCEEventLogGrid() {
    var ClientId = $(document).find('#ClientId').val();

    if ($(document).find('#ClientEventLogTable').hasClass('dataTable')) {
        ClientEventLogTable.destroy();
    }
    ClientEventLogTable = $("#ClientEventLogTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/Admin/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Admin/Clients/PopulateEventLog?ClientId=" + ClientId,
            "type": "POST",
            "datatype": "json"
        },
        "columns":
            [
                {
                    "data": "TransactionDate",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "type": "date"
                },
                { "data": "EventLogId", "autoWidth": false, "bSearchable": true, "bSortable": true, "width": "20%" },
                { "data": "SiteName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Event", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Comments", "autoWidth": true, "bSearchable": true, "bSortable": true },
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}

//#endregion

function openCity(evt, cityName) {
    evt.preventDefault();
    $(document).find(".tabsArea").hide();
    switch (cityName) {
        case "ClientSites":
            GenerateSiteTableGrid();
            $('#SiteDetail').html('');
            $('#SiteSubscription').html('');
            break;
        case "ClientOverview":
            $('#ClientEventLogItem').show();
            $('#ClientActivity').show();
            $('#btnIdentification').addClass('active');
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
}

//#region Activity
var order_forActivity = '1';
var orderDir_forActivity = 'asc';
var ActivityTable;
function GenerateActivityTableGrid() {
    var ClientId = $(document).find('#ClientId').val();

    if ($(document).find('#ActivityTable').hasClass('dataTable')) {
        ActivityTable.destroy();
    }
    ActivityTable = $("#ActivityTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/Admin/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Admin/Clients/PopulateActivityTableGrid?ClientId=" + ClientId,
            "type": "POST",
            "datatype": "json"
        },
        "columns":
            [
                {
                    "data": "CreateDate",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "type": "date"
                },
                { "data": "UserInfoId", "autoWidth": false, "bSearchable": true, "bSortable": true, "width": "20%" },
                { "data": "SessionId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Browser", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "IPAddress", "autoWidth": true, "bSearchable": true, "bSortable": true },
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
//#endregion

//#region Site Grid
var order_forSite = '1';
var orderDir_forSite = 'asc';
var ClientSiteTable;
function GenerateSiteTableGrid() {
    var ClientId = $(document).find('#ClientId').val();

    if ($(document).find('#ClientSiteTable').hasClass('dataTable')) {
        ClientSiteTable.destroy();
    }
    ClientSiteTable = $("#ClientSiteTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/Admin/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Admin/Clients/PopulateSiteTableGrid?ClientId=" + ClientId,
            "type": "POST",
            "datatype": "json"
        },
        "columns":
            [
                {
                    "data": "SiteId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "0",
                    "mRender": function (data, type, full, row) {
                        return '<div  class="width-100"><a class="lnk_site" href="javascript:void(0)">' + data + '</a></div>'
                    }
                },
                {
                    "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-left", "name": "1"
                },
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-left", "name": "2" },
                {
                    "data": "APM", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3", "className": "text-center",
                    "mRender": function (data, type, row) {
                        if (data == false) {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" class="status" onclick="return false"><span></span></label>';
                        }
                        else {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" checked="checked" class="status" onclick="return false"><span></span></label>';
                        }
                    }
                },
                {
                    "data": "CMMS", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4", "className": "text-center",
                    "mRender": function (data, type, row) {
                        if (data == false) {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" class="status" onclick="return false"><span></span></label>';
                        }
                        else {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" checked="checked" class="status" onclick="return false"><span></span></label>';
                        }
                    }
                },
                {
                    "data": "Sanitation", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5", "className": "text-center",
                    "mRender": function (data, type, row) {
                        if (data == false) {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" class="status" onclick="return false"><span></span></label>';
                        }
                        else {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" checked="checked" class="status" onclick="return false"><span></span></label>';
                        }
                    }
                },
                {
                    "data": "SiteId", "autoWidth": true, "bSearchable": true, "bSortable": false, "className": "text-center",
                    "mRender": function (data, type, row) {
                        var status = '';
                        if (row.Status === "Active") {
                            status = '<a class="btn btn-blue siteActiveInactiveBtn gridinnerbutton" style="margin-left: 0;" title="Inactive"">Inactive</a>';
                        }
                        else {
                            status = '<a class="btn btn-blue siteActiveInactiveBtn gridinnerbutton" style="margin-left: 0;" title="Active"">Active</a>';
                        }

                        return '<button type="button" class="btn btn-blue actionbtns" style="border:0;outline:0" data-id="' + row.SiteId + '"><strong>...</strong></button>' +
                            '<div id="' + row.SiteId + '" class="actionbtndiv" style="display:none;">' +
                            '<a class="btn btn-blue addSiteBtn gridinnerbutton" style="margin-left: 0;" title="Add">Add Site</a>' +
                            status +
                            '</div >' +
                            '<div class="maskaction ' + row.SiteId + '" data-id="' + row.SiteId + '"  onclick="funcCloseActionbtn()"></div>';
                    }
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}

function funcCloseActionbtn() {
    var btnid = $(this).attr("data-id");
    $(document).find(".actionbtndiv").hide();
    $(document).find(".maskaction").hide();
}

$(document).on('click', '.actionbtns', function (e) {
    var row = $(this).parents('tr');
    var data = ClientSiteTable.row(row).data();
    $(document).find("#" + data.SiteId + "").show();
    $(document).find("." + data.SiteId).show();
});
$(document).on('click', '.lnk_site', function (e) {
    var siteId;
    var clientId = $(document).find('#ClientId').val();

    var row = $(this).parents('tr');
    var data = ClientSiteTable.row(row).data();
    siteId = data.SiteId;


    GetSiteDetailsByClientIdSiteId(clientId, siteId);
    GetSiteBillingDetailsByClientIdSiteId(clientId, siteId);
});

$(document).on('click', '#actinctivateclient', function (e) {
    var ClientId = $(document).find("#ClientId").val();
    var Status = $(document).find("#ClientStatus").val();
    var InActiveFlag = Status == "Active" ? true : false;
    $.ajax({
        url: "/Admin/Clients/MakeClientActiveInactive",
        type: "POST",
        dataType: "json",
        data: { InActiveFlag: InActiveFlag, ClientId: ClientId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                if (InActiveFlag == true) {
                    SuccessAlertSetting.text = /*getResourceValue("AccountActivateSuccessAlert")*/"Client inactivated successfully";
                    titleText = getResourceValue("AlertInactive");
                }
                else {
                    SuccessAlertSetting.text = /*getResourceValue("AccountInactivateSuccessAlert")*/"Client activated successfully";
                    titleText = getResourceValue("AlertActive");
                }
                swal(SuccessAlertSetting, function () {
                    RedirectToClientDetail(ClientId, "client", 0);
                });
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
//#endregion

//#region Site Edit
$(document).on('click', "#btnEditSite", function () {
    var ClientId = LRTrim($(document).find('#ClientId').val());

    var SiteId = LRTrim($(document).find('#SiteId').val());

    $.ajax({
        url: '/Admin/Clients/EditSiteByClientIdSiteId',
        data: { ClientId: ClientId, SiteId: SiteId },
        type: "POST",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#clientmaincontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: '.ignorevalidation' });
            $.validator.unobtrusive.parse(document);
            $('.select2picker, form').change(function () {
                var areaddescribedby = $(this).attr('aria-describedby');
                if (typeof areaddescribedby != 'undefined') {
                    $('#' + areaddescribedby).show();
                }
            });
            $(document).find('.select2picker').select2({});

            $("form").submit(function () {
                var activetagid = $('.vtabs li.active').attr('id');
            });
        }
    });
});

function SiteEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("SiteUpdateSuccessAlert");
        var ClientId = $(document).find('#ClientId').val();
        swal(SuccessAlertSetting, function () {
            RedirectToClientDetail(ClientId, "site", data.SiteId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion

//#region Site Details By SiteId
function GetSiteDetailsByClientIdSiteId(clientId, siteid) {
    $.ajax({
        url: "/Admin/Clients/GetSiteDetailsByClientIdSiteId",
        type: "POST",
        data: { clientId: clientId, siteid: siteid },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#SiteDetail').html(data);
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion

//#region Site Billing Details By SiteId
function GetSiteBillingDetailsByClientIdSiteId(clientId, siteid) {
    $.ajax({
        url: "/Admin/Clients/GetSiteBillingDetailsByClientIdSiteId",
        type: "POST",
        data: { clientId: clientId, siteid: siteid },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#SiteSubscription').html(data);
        },
        complete: function () {
            CloseLoader();

        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion

//#region SiteBilling Edit
$(document).on('click', "#btnEditSiteBilling", function () {
    var ClientId = LRTrim($(document).find('#ClientId').val());

    var SiteId = LRTrim($(document).find('#SiteId').val());

    $.ajax({
        url: '/Admin/Clients/EditSiteBillingByClientIdSiteId',
        data: { ClientId: ClientId, SiteId: SiteId },
        type: "POST",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#clientmaincontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            $(document).find('.dtpicker').datepicker({
                changeMonth: true,
                changeYear: true,
                beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
                "dateFormat": "mm/dd/yy",
                autoclose: true
            }).inputmask('mm/dd/yyyy');

        }
    });
});

function SiteBillingAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("SiteBillingUpdateAlert");
        var ClientId = $(document).find('#ClientId').val();
        swal(SuccessAlertSetting, function () {
            RedirectToClientDetail(ClientId, "site", data.SiteId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', "#btnCancelEditSite,#btnCancelEditSiteBilling", function () {
    var clientid = $(document).find('#ClientId').val();
    var siteid = $(document).find('#SiteId').val();
    swal(CancelAlertSetting, function () {
        RedirectToClientDetail(clientid, "site", siteid);
    });
});
$(document).on('click', '.siteActiveInactiveBtn', function (e) {
    var row = $(this).parents('tr');
    var data = ClientSiteTable.row(row).data();
    var ClientId = $(document).find("#ClientId").val();
    var InActiveFlag = data.Status == "Active" ? true : false;
    var UpdateIndex = data.UpdateIndex;
    var SiteId = data.SiteId;
    $.ajax({
        url: "/Admin/Clients/MakeSiteActiveInactive",
        type: "POST",
        dataType: "json",
        data: { InActiveFlag: InActiveFlag, ClientId: ClientId, SiteId: SiteId, UpdateIndex: UpdateIndex },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                if (InActiveFlag == true) {
                    SuccessAlertSetting.text = getResourceValue("SiteInactivateSuccessAlert");
                }
                else {
                    SuccessAlertSetting.text = getResourceValue("SiteActivateSuccessAlert");
                }
                swal(SuccessAlertSetting, function () {
                    RedirectToClientDetail(ClientId, "site", 0);
                });
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
//#endregion

//#region Add Site
$(document).on('click', '.addSiteBtn', function () {
    var clientid = $(document).find('#ClientId').val();
    $.ajax({
        url: "/Admin/Clients/AddSite",
        type: "GET",
        dataType: 'html',
        data: { ClientId: clientid },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#clientmaincontainer').html(data);
        },
        complete: function () {
            SetClientControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btnCancelAddSite", function () {
    var clientid = $(document).find('#ClientId').val();
    swal(CancelAlertSetting, function () {
        RedirectToClientDetail(clientid, "site", 0);
    });
});

function SiteAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("SiteAddSuccessAlert");
        if (data.Command == "save") {
            swal(SuccessAlertSetting, function () {
                RedirectToClientDetail(data.ClientId, "site", 0);
            });
        }
        else {
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").not("#colorselector").val("").trigger('change.select2');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion

$(document).on('click', '.lithisclient', function () {
    var ClientId = $(this).attr('data-val');
    RedirectToClientDetail(ClientId, "client");
});

//#region V2-994

//#region PopupGridWebSiteMaintainenceMessageDetail - V2-994
$(document).on('click', '#actionSiteMaintainence', function () {
    RedirectToSiteMaintainenceDetail();
});
function RedirectToSiteMaintainenceDetail() {
    $.ajax({
        url: "/Admin/Clients/SiteMaintainenceDetails",
        type: "POST",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#WebSiteMainteneceMessageGrid').html(data);
            $(document).find('#WebSiteMainteneceMessageGridModal').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            generateSiteMaintenanceTable();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

var SiteMaintenanceTable;
function generateSiteMaintenanceTable() {
    var rCount = 0;
    if ($(document).find('#tblSiteMaintenanceList').hasClass('dataTable')) {
        SiteMaintenanceTable.destroy();
    }
    SiteMaintenanceTable = $('#tblSiteMaintenanceList').DataTable({
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
            url: "/Admin/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Admin/Clients/GetSiteMaintainanceGridData",
            "type": "post",
            "datatype": "json",
            "dataSrc": function (result) {
                let colOrder = SiteMaintenanceTable.order();
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
                {
                    "data": "SiteMaintenanceId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "name": "1", "class": "text-center"
                },
                {
                    "data": "LoginPageMessage", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "name": "2",
                },
                {
                    "data": "DowntimeStart", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "name": "3",
                },
                {
                    "data": "DowntimeEnd", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "name": "4",
                },
                {
                    "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "name": "5",
                },
                { "data": "SiteMaintenanceId", "bSearchable": false, "bSortable": false, className: "text-center" },

            ],
        columnDefs: [
            {
                "data": null,
                targets: [5], render: function (a, b, data, d) {
                    if (true) {
                        return '<div class="text-wrap"><a class="btn btn-outline-primary gridbtnAddSiteMaintenance gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success gridbtnEditSiteMaintenance gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        initComplete: function () {
            if (rCount > 0) {
                $("#btnAddSiteMaintenance").hide();
            }
            else {
                $("#btnAddSiteMaintenance").show();
            }
            SetPageLengthMenu();
        }
    });
};
//#endregion

//#region Add EDit

$(document).on('click', '#btnAddSiteMaintenance', function () {
    AddEditSiteMaintence(0);
});
$(document).on('click', '.gridbtnAddSiteMaintenance', function () {
    AddEditSiteMaintence(0);
   
});
$(document).on('click', '.gridbtnEditSiteMaintenance', function () {
    var data = SiteMaintenanceTable.row($(this).parents('tr')).data();
    AddEditSiteMaintence(data.SiteMaintenanceId);
});
function AddEditSiteMaintence(SiteMaintenanceId) {
    $.ajax({
        url: "/Admin/Clients/AddEditSiteMaintenanceView",
        type: "POST",
        dataType: 'html',
        data: {
            "SiteMaintenanceId": SiteMaintenanceId,
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddEditSiteMaintenanceModalDialog').html(data);
            $('#AddEditSiteMaintenancerModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
            $(document).find(".dtpicker").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                minDate:new Date(),
                buttonImage: '/Admin/Images/calender.png',
            });
            if (SiteMaintenanceId == 0) {
                $('.dtpicker').datepicker('setDate', new Date());
            }
            var timerVal = moment().format('hh:mm A');
            $(document).find('.timerId').timepicker(
                {
                    template: 'dropdown',
                    minuteStep: 1,
                    showMeridian: true,
                    defaultTime: timerVal,
                });
        },
        complete: function () {
            SetClientControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SiteMaintenceOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("alertWebSiteMaintenanceMsgSave");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("alertWebSiteMaintenanceMsgUpdated");
        }
        swal(SuccessAlertSetting, function () {
            SiteMaintenanceTable.page('first').draw('page');
        });
        $('#AddEditSiteMaintenancerModalpopup').modal('hide');
        $("#btnAddSiteMaintenance").hide();
    }
    else {
        ShowErrorAlert(data);
    }
};
function chkValidDate() {
    var startDate = $(document).find('#DowntimeStartDate').val();
    var startTime = $(document).find('#DowntimeStartTime').val();
    var startDateMarge = new Date(startDate + ' ' + startTime);
    var endDate = $(document).find('#DowntimeEndDate').val();
    var endTime = $(document).find('#DowntimeEndTime').val();
    var endDateMarge = new Date(endDate + ' ' + endTime);
    var startDateTime = new Date(startDateMarge);
    var endDateTime = new Date(endDateMarge);
    if (startDateTime > endDateTime) {
        ErrorAlertSetting.text = getResourceValue("alertDowntimeEndAndStartDateGtrErrMsg");
        swal(ErrorAlertSetting, function () {
        });
        return false;
    }
    return true;

}
//#endregion

//#endregion


//#region V2-993

//#region PopupGridMessageSelectedClientDetail
$(document).on('click', '#actionMessageSelectedClient', function () {
    var ClientId = $("#ClientId").val();
    RedirectToMessageSelectedClientDetail(ClientId, "client");
});
function RedirectToMessageSelectedClientDetail(ClientId) {
    $.ajax({
        url: "/Admin/Clients/MessageSelectedClientDetails",
        type: "POST",
        dataType: 'html',
        data: {
            "ClientCustomId": ClientId,
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#MessageSelectedClientMessageGrid').html(data);
            $(document).find('#MessageSelectedClientMessageGridModal').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            generateMessageSelectedClientTable();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

var MessageSelectedClientTable;
var rCount = 0;
function generateMessageSelectedClientTable() {
    if ($(document).find('#tblMessageSelectedClientList').hasClass('dataTable')) {
        MessageSelectedClientTable.destroy();
    }
    MessageSelectedClientTable = $('#tblMessageSelectedClientList').DataTable({
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
            url: "/Admin/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Admin/Clients/GetMessageSelectedClientGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.ClientCustomId = LRTrim($("#ClientId").val());
            },
            "dataSrc": function (result) {
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
                {
                    "data": "ClientMessageId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "name": "1", "class": "text-center"
                },
                {
                    "data": "Message", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "name": "2",
                },
                {
                    "data": "StartDate", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "name": "3",
                },
                {
                    "data": "EndDate", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "name": "4",
                },
                {
                    "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "name": "5",
                },
                { "data": "ClientMessageId", "bSearchable": false, "bSortable": false, className: "text-center" },

            ],
        columnDefs: [
            {
                "data": null,
                targets: [5], render: function (a, b, data, d) {
                    if (true) {

                        return '<div class="text-wrap"><a class="btn btn-outline-primary gridbtnAddMessageSelectedClient gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success gridbtnEditMessageSelectedClient gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        initComplete: function () {
            if (rCount > 0) {
                $("#btnAddClientMessage").hide();
            }
            else {
                $("#btnAddClientMessage").show();
            }
            SetPageLengthMenu();
        }
    });
};

//#endregion

//#region Add EDit MessageSelectedClient
$(document).on('click', '.gridbtnAddMessageSelectedClient', function () {
    var ClientId = $("#ClientId").val();
    AddEditMessageSelectedClients(0, ClientId);
});
$(document).on('click', '.gridbtnEditMessageSelectedClient', function () {
    var data = MessageSelectedClientTable.row($(this).parents('tr')).data();
    AddEditMessageSelectedClients(data.ClientMessageId, data.ClientId);
});
$(document).on('click', '#btnAddClientMessage', function () {
    var ClientId = $("#ClientId").val();
    AddEditMessageSelectedClients(0, ClientId);
});
function AddEditMessageSelectedClients(ClientMessageId, ClientId) {
    $.ajax({
        url: "/Admin/Clients/AddEditMessageSelectedClientView",
        type: "POST",
        dataType: 'html',
        data: {
            "ClientMessageId": ClientMessageId,
            "ClientId": ClientId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddEditMessageSelectedClientModalDialog').html(data);
            $('#AddEditMessageSelectedClientModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
            $(document).find(".dtpicker").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                minDate:new Date(),
                buttonImage: '/Admin/Images/calender.png',
            });
            if (ClientMessageId == 0) {
                $('.dtpicker').datepicker('setDate', new Date());
            }
            var timerVal = moment().format('hh:mm A');
            $(document).find('.timerId').timepicker(
                {
                    template: 'dropdown',
                    minuteStep: 1,
                    showMeridian: true,
                    defaultTime: timerVal,
                });
        },
        complete: function () {
            SetClientControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function MessageSelectedClientOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("alertClientMessageSave");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("alertClientMessageUpdated");
        }
        swal(SuccessAlertSetting, function () {
            MessageSelectedClientTable.page('first').draw('page');
        });
        $('#AddEditMessageSelectedClientModalpopup').modal('hide');
        $("#btnAddClientMessage").hide();
    }
    else {
        ShowErrorAlert(data);
    }
};
function chkValidDate() {
    var startDate = $(document).find('#CMStartDate').val();
    var startTime = $(document).find('#CMStartTime').val();
    var startDateMarge = new Date(startDate + ' ' + startTime);
    var endDate = $(document).find('#CMEndDate').val();
    var endTime = $(document).find('#CMEndTime').val();
    var endDateMarge = new Date(endDate + ' ' + endTime);
    var startDateTime = new Date(startDateMarge);
    var endDateTime = new Date(endDateMarge);
    if (startDateTime > endDateTime) {
        ErrorAlertSetting.text = getResourceValue("alertEndDateGtrStartDateErrMsg");
        swal(ErrorAlertSetting, function () {
        });
        return false;
    }
    return true;
}
//#endregion



//#region PopupGridMessageAllClientDetail
$(document).on('click', '#actionMessageAllClients', function () {
    var ClientId = 0;
    RedirectToMessageAllClientDetail(ClientId, "client");
});
function RedirectToMessageAllClientDetail(ClientId) {
    $.ajax({
        url: "/Admin/Clients/MessageSelectedClientDetails",
        type: "POST",
        dataType: 'html',
        data: {
            "ClientCustomId": ClientId,
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#MessageAllClientMessageGrid').html(data);
            $(document).find('#MessageAllClientMessageGridModal').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            generateMessageAllClientTable();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

var MessageAllClientTable;
var rCount = 0;
function generateMessageAllClientTable() {
    if ($(document).find('#tblMessageAllClientList').hasClass('dataTable')) {
        MessageAllClientTable.destroy();
    }
    MessageAllClientTable = $('#tblMessageAllClientList').DataTable({
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
            url: "/Admin/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Admin/Clients/GetMessageAllClientGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.ClientCustomId = 0;
            },

            "dataSrc": function (result) {
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
                {
                    "data": "ClientMessageId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "name": "1", "class": "text-center"
                },
                {
                    "data": "Message", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "name": "2",
                },
                {
                    "data": "StartDate", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "name": "3",
                },
                {
                    "data": "EndDate", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "name": "4",
                },
                {
                    "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "name": "5",
                },
                { "data": "ClientMessageId", "bSearchable": false, "bSortable": false, className: "text-center" },

            ],
        columnDefs: [
            {
                "data": null,
                targets: [5], render: function (a, b, data, d) {
                    if (true) {
                        return '<div class="text-wrap"><a class="btn btn-outline-primary gridbtnAddMessageAllClient gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success gridbtnEditMessageAllClient gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        initComplete: function () {
            if (rCount > 0) {
                $("#btnAddAllClientMessage").hide();
            }
            else {
                $("#btnAddAllClientMessage").show();
            }
            SetPageLengthMenu();

        }
    });
};
//#endregion

//#region Add EDit MessageAllClient

$(document).on('click', '.gridbtnAddMessageAllClient', function () {
    var data = MessageAllClientTable.row($(this).parents('tr')).data();
    AddEditMessageAllClients(0, data.ClientId);
})

$(document).on('click', '#btnAddAllClientMessage', function () {
    var ClientId = $("#ClientId").val();
    AddEditMessageAllClients(0, ClientId);
});
$(document).on('click', '.gridbtnEditMessageAllClient', function () {
    var data = MessageAllClientTable.row($(this).parents('tr')).data();
    AddEditMessageAllClients(data.ClientMessageId, data.ClientId);
});

$(document).on('click', '#btnAddClientMessage', function () {
    var ClientId = $("#ClientId").val();
    AddEditMessageAllClients(0, ClientId);
});
function AddEditMessageAllClients(ClientMessageId, ClientId) {
    $.ajax({
        url: "/Admin/Clients/AddEditMessageAllClientView",
        type: "POST",
        dataType: 'html',
        data: {
            "ClientMessageId": ClientMessageId,
            "ClientId": ClientId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddEditMessageAllClientModalDialog').html(data);
            $('#AddEditMessageAllClientModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
            $(document).find(".dtpicker").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '/Admin/Images/calender.png',
                minDate:new Date()
            });
            if (ClientMessageId == 0) {
                $('.dtpicker').datepicker('setDate', new Date());
            }
            var timerVal = moment().format('hh:mm A');
            $(document).find('.timerId').timepicker(
                {
                    template: 'dropdown',
                    minuteStep: 1,
                    showMeridian: true,
                    defaultTime: timerVal,
                });
        },
        complete: function () {
            SetClientControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function MessageAllClientOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("alertClientMessageSave");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("alertClientMessageUpdated");
        }
        swal(SuccessAlertSetting, function () {
            MessageAllClientTable.page('first').draw('page');
        });
        $('#AddEditMessageAllClientModalpopup').modal('hide');
        $("#btnAddAllClientMessage").hide();
    }
    else {
        ShowErrorAlert(data);
    }
};
function chkValidDate() {
    var startDate = $(document).find('#CMStartDate').val();
    var startTime = $(document).find('#CMStartTime').val();
    var startDateMarge = new Date(startDate + ' ' + startTime);
    var endDate = $(document).find('#CMEndDate').val();
    var endTime = $(document).find('#CMEndTime').val();
    var endDateMarge = new Date(endDate + ' ' + endTime);
    var startDateTime = new Date(startDateMarge);
    var endDateTime = new Date(endDateMarge);
    if (startDateTime > endDateTime) {
        ErrorAlertSetting.text = getResourceValue("alertEndDateGtrStartDateErrMsg");
        swal(ErrorAlertSetting, function () {
        });
        return false;
    }
    return true;
}
//#endregion

//#endregion