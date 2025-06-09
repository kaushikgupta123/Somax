var AlertSiteId;
var AlertSetupId;
var PackageLevelEnterprise = 'ENTERPRISE';
$(function () {
    $(document).find('.select2picker').select2({});
    var IsTargetListShow = $(document).find("#IsTargetListShow").val();
    var AlertSetupId = $(document).find("#AlertSetupId").val();
    var pakagelevel = $("#PackageLevelDef").val();
    var IsSuperUser = $("#IsSuperUser").val();
    if (pakagelevel == PackageLevelEnterprise && (IsSuperUser === 'True' || IsSuperUser === 'true' || IsSuperUser === 'TRUE') ) {
        ShowHideDetail();
    }
    else {
        if (AlertSetupId != '') {
            if (IsTargetListShow == 'True') {
                GenerateTargetGrid(AlertSetupId);
                $('#tblcontainer').show();
            }
            else {
                GenerateTargetGrid(0);
                $('#tblcontainer').hide();
            }
        }
        else {
            $('#tblcontainer').hide();
        }
    }
    var ISEmailShow = $(document).find("#IsShowEmailSend").val();
    if (ISEmailShow == true || ISEmailShow == "True") {
        $("#EmailSendShow").show();
    }
    else {
        $("#EmailSendShow").hide();
    }
});
$(document).on('click', "ul.vtabs li", function () {
    var id = $(this).attr('id');
    if (id != undefined && id == 'idShipTo') {
        $(document).find('#btnSaveWO').hide();
    } else {
        $(document).find('#btnSaveWO').show();
    }
    if ($(this).find('#drpDwnLink').length > 0 || $(this).find('#drpDwnLink2').length > 0) {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        return false;
    }
    else {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        $(".tabsArea").hide();
        var activeTab = $(this).find("a").attr("href");
        $(activeTab).fadeIn();
        return false;
    }
});
function NotificationUpdateAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("AlertNotificationsetupsuccessfullyAlert"); 
        swal(SuccessAlertSetting, function () {
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
var dtTargetListTable;
//#region Target
function GenerateTargetGrid(AlertSetupId) {
    var rCount = 0;
    if ($(document).find('#tblTargetListGrid').hasClass('dataTable')) {
        dtTargetListTable.destroy();
    }
    dtTargetListTable = $("#tblTargetListGrid").DataTable({
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
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/NotificationSetUp/PopulateTargetList?targetId=" + AlertSetupId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                if (rCount > 0) {
                    $(document).find("#AddNewTarget").hide();
                }
                else {
                    $(document).find("#AddNewTarget").show();
                }
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-primary addTargetPage gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                        '<a class="btn btn-outline-success editTargetPage gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delTargetPage gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                { "data": "Personnel_ClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "FirstName", "autoWidth": true, "bSearchable": false, "bSortable": true },
                { "data": "LastName", "autoWidth": true, "bSearchable": false, "bSortable": true },
                {
                    "data": "IsActive", "autoWidth": false, "bSearchable": true, "bSortable": true, className: 'text-center',
                "mRender": function (data, type, row) {
                    if (data == true) {
                        return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" checked="checked" class="status"><span></span></label>';
                    }
                    else {
                        return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" class="status"><span></span></label>';
                    }
                }
            },
            {
                "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
            }
        ],
        initComplete: function () {
            $(document).on('click', '.status', function (e) {
                e.preventDefault();
            });
            SetPageLengthMenu();
            CloseLoader();
        }
    });
}
$(document).on('click', '.editTargetPage', function () {
    var data = dtTargetListTable.row($(this).parents('tr')).data();
    EditTargetPage(data);
});
function EditTargetPage(fullrecord) {
    AlertSetupId = fullrecord.AlertSetupId;
    var AlertTargetId = fullrecord.AlertTargetId;
    var IsActive = fullrecord.IsActive;
    var UpdateIndex = fullrecord.UpdateIndex;
    var ClientLookupId = fullrecord.Personnel_ClientLookupId;
    var FirstName = fullrecord.FirstName;
    var LastName = fullrecord.LastName;
    AlertSiteId = $(document).find("#AlertSiteId").val();
    $.ajax({
        url: "/NotificationSetUp/AddOrEditTargetAlert",
        type: "GET",
        dataType: 'html',
        data: { AlertSetUpId: AlertSetupId, AlertTargetId: AlertTargetId, IsActive: IsActive, UpdateIndex: UpdateIndex, personalLoookupId: ClientLookupId, FirstName: FirstName, LastName: LastName },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderNotificationAlert').html(data);
        },
        complete: function () {
            SetTargetEnvironmentPage();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', '.addTargetPage', function () {
    var data = dtTargetListTable.row($(this).parents('tr')).data();
    var AlertTargetId = 0;
    var UpdateIndex = 0;   
    AlertSiteId = $(document).find("#AlertSiteId").val();
    AlertSetupId = $(document).find("#AlertSetupId").val();
    $.ajax({
        url: "/NotificationSetUp/AddOrEditTargetAlert",
        type: "GET",
        dataType: 'html',
        data: { AlertSetUpId: AlertSetupId, AlertTargetId: AlertTargetId, IsActive: false, UpdateIndex: UpdateIndex },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('.select2picker').select2({});
            $('#renderNotificationAlert').html(data);
        },
        complete: function () {
            SetTargetEnvironmentPage();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delTargetPage', function () {
    var AlertSetupId = $(document).find("#AlertSetupId").val();
    var data = dtTargetListTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        DeleteTarget(data, AlertSetupId);
    });
});
function DeleteTarget(data, AlertSetupId) {
    $.ajax({
        url: '/NotificationSetUp/DeleteTarget',
        data: {
            AlertTargetId: data.AlertTargetId,
            AlertSetupId: AlertSetupId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                dtTargetListTable.state.clear();
                ShowDeleteAlert(getResourceValue("TargetDeleteAlert"));
                GenerateTargetGrid(data.AlertSetupId);
            }
        },
        complete: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#btnSTargetcancel", function () {
    swal(CancelAlertSetting, function () {
        RedirectToNotificationDetail();
    });
});
function AddOnSuccessTarget(data) {
    if (data.Result == "success") {
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("TargetAddAlert"); 
        }
        else {
            SuccessAlertSetting.text = getResourceValue("TargetUpdateAlert"); 
        }
        swal(SuccessAlertSetting, function () {
            CloseLoader();
            RedirectToNotificationDetail();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
function SetTargetEnvironmentPage() {
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if ($(this).valid()) {
            if (typeof areaddescribedby !== 'undefined') {
                $('#' + areaddescribedby).hide();
            }
        }
        else {
            if (typeof areaddescribedby !== 'undefined') {
                $('#' + areaddescribedby).show();
            }
        }
    });
    $(document).find('.select2picker').select2({});
}
//#endregion

//#region V2-402 Notification Changes
function ShowHideDetail() {
    var SiteId = $(document).find("#AlertSiteId").val();
    var AlertSetUpId = $(document).find("#AlertSetupId").val();
    if (SiteId != '') {
        if (AlertSetUpId != '') {
            $(".alert-det").show();
        }
        else {
            $(".alert-det").hide();
        }
    }
    else {
        $(".alert-det").hide();
    }
}
function RedirectToNotificationDetail() {
    var PackageLevel = $(document).find("#PackageLevelDef").val();
    var IsSuperUser = $("#IsSuperUser").val();
    AlertSiteId = AlertSiteId;
    AlertSetupId = AlertSetupId;
    $.ajax({
        url: "/NotificationSetUp/NotificationDetails",
        type: "POST",
        dataType: 'html',
        data: { SetupId: AlertSetupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderNotificationAlert').html(data);

        },
        complete: function () {
            CloseLoader();
            if (PackageLevel.toUpperCase() === PackageLevelEnterprise && (IsSuperUser === 'True' || IsSuperUser === 'true' || IsSuperUser === 'TRUE')) {
                $(document).find('.select2picker').select2({});
                $("#AlertSiteId").val(AlertSiteId).trigger('change');
            }
            else {
                $(document).find('.select2picker').select2({});
                GenerateTargetGrid(AlertSetupId);
            }
            ShowHideDetail();
            $('#tblcontainer').show();
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            CloseLoader();
        }
    });
}



$(document).on("change", "#AlertSiteId", function () {
    var AlertSiteId = $("#AlertSiteId").val();
    if (AlertSiteId === '') {
        $('#AlertSetupId').val('').attr('disabled', true).trigger('change.select2');
    }
    else {
        $('#AlertSetupId').removeAttr('disabled');
    }
    $(".alert-det").hide();
    $('#tblcontainer').hide();  
    if (AlertSiteId.length > 0) {
        var areaddescribedby = $(document).find("#AlertSiteId").attr('aria-describedby');
        if (typeof areaddescribedby !== 'undefined') {
            $('#' + areaddescribedby).hide();
        }
        $(document).find('form').find("#AlertSiteId").removeClass("input-validation-error");
    }
    else {
        var arectoaddescribedby = $(document).find("#AlertSiteId").attr('aria-describedby');
        if (typeof arectoaddescribedby !== 'undefined') {
            $('#' + arectoaddescribedby).show();
        }

        $(document).find('form').find("#AlertSiteId").addClass("input-validation-error");
    }   
    if (AlertSiteId != '') { 
        $.ajax({
            url: '/NotificationSetUp/GetAlertNotification',
            data: {
                AlertSiteId: AlertSiteId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    var $el = $("#AlertSetupId");
                    $el.empty();
                    $el.append($('<option>', {
                        value: '',
                        text: '--Select--'
                    }));
                    $.each(data.data, function (index, value) {
                        $el.append($("<option></option>").attr("value", value.AlertSetupId).text(value.Name));
                    });
                }
                else {
                    var $myel = $("#AlertSetupId");
                    $myel.empty();
                    $myel.append($('<option>', {
                        value: '',
                        text: '--Select--'
                    }));
                    if (AlertSiteId.length > 0) {
                        var areaddescribedby = $(document).find("#AlertSiteId").attr('aria-describedby');
                        if (typeof areaddescribedby !== 'undefined') {
                            $('#' + areaddescribedby).hide();
                        }
                        $(document).find('form').find("#AlertSiteId").removeClass("input-validation-error");
                    }
                    else {
                        var arectoaddescribedby = $(document).find("#AlertSiteId").attr('aria-describedby');
                        if (typeof arectoaddescribedby !== 'undefined') {
                            $('#' + arectoaddescribedby).show();
                        }
                        $(document).find('form').find("#AlertSiteId").addClass("input-validation-error");
                    }
                }
            },
            complete: function () {
                if (AlertSetupId) {
                    $('#AlertSetupId').removeAttr('disabled');
                    $("#AlertSetupId").val(AlertSetupId).trigger('change');
                    AlertSetupId = "";
                }
                CloseLoader();
            }
        });
    }
});
$(document).on("change", "#AlertSetupId", function () {
    var DropdownId =$("#AlertSetupId").val();
    if (DropdownId.length > 0) {
        var areaddescribedby = $("#AlertSetupId").attr('aria-describedby');
        if (typeof areaddescribedby !== 'undefined') {
            $('#' + areaddescribedby).hide();
        }
        $(document).find('form').find("#AlertSetupId").removeClass("input-validation-error");
    }
    else {
        var arectoaddescribedby = $("#AlertSetupId").attr('aria-describedby');
        if (typeof arectoaddescribedby !== 'undefined') {
            $('#' + arectoaddescribedby).hide();
        }
        $(document).find('form').find("#AlertSetupId").addClass("input-validation-error");
    }   
    if (DropdownId != '') {
        $.ajax({
            url: '/NotificationSetUp/GetValueAlertDropDownChange',
            data: {
                DropDownId: DropdownId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    $(document).find("#DescriptionLabel").text(data.Description);
                    $("#IsActive").prop("checked", data.IsActive);
                    $("#IsEmailSend").prop("checked", data.IsEmailSend);
                    $("#IsShowEmailSend").val(data.IsShowMailSend);
                    $("#IsIncludeEmailAttachedment").val(data.IsIncludeEmailAttachedment);
                    if (data.IsTargetListShow) {
                        GenerateTargetGrid(data.SetupId);
                        $('#tblcontainer').show();
                    }
                    else {
                        GenerateTargetGrid(0);
                        $('#tblcontainer').hide();
                    }
                    var ISEmailShow = data.IsShowMailSend;
                    if (ISEmailShow === true) {
                        $("#EmailSendShow").show();
                    }
                    else {
                        $("#EmailSendShow").hide();
                    }
                }
            },
            complete: function () {
                ShowHideDetail();
                CloseLoader();
            }
        });
    }
    if (DropdownId == '') {
        GenerateTargetGrid(0);
    }
});
//#endregon