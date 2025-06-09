var dtAttachmentTable;
var dtEventTable;
var dtLaborTable
var dtAvailabilityTable;
var dtAttendanceTable;

//#region OnPageLoadJs
$(document).ready(function () {
    $(document).on('change', '#mobilesidemenuselector', function (evt) {
        $(document).find('.tabsArea').hide();
        GoToTab(evt, $(this).val());
        $('#' + $(this).val()).show();
    });

});
$(document).on('click', "ul.vtabs li", function () {
    $("ul.vtabs li").removeClass("active");
    $(this).addClass("active");
    $(".tabsArea").hide();
    var activeTab = $(this).find("a").attr("href");
    $(activeTab).fadeIn();
    return false;
});
function GoToTab(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "Attachments":
            GenerateAttachmentGrid();
            break;
        case "Events":
            GenerateEventsGrid();
            break;
        case "Labor":
            GenerateLaborsGrid();
            break;
        case "Availability":
            GenerateAvailabilityGrid();
            break;
        case "Attendance":
            GenerateAttendanceGrid();
            break;
    }
}
//#endregion

//#region Edit Personnel
$(document).on('click', "#editpersonnel", function () {
    var personId = $('#personnelModel_PersonnelId').val();
    $.ajax({
        url: "/Personnel/EditPersonnel",
        type: "GET",
        dataType: 'html',
        data: { PersonnelId: personId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#personnelcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            SetPersonnelEnvironmentPage();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function OnPersonnelEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("UpdatePersonnelAlert");
        swal(SuccessAlertSetting, function () {
            RedirectTopersonnelDetail(data.PersonnelId, "");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnCancelPersonnel", function () {
    var PersonnelId = $(document).find('#personnelModel_PersonnelId').val();
    swal(CancelAlertSetting, function () {
        RedirectTopersonnelDetail(PersonnelId, "");
    });
});
//#endregion

//#region Attachment
function GenerateAttachmentGrid() {
    $('#btnAddAttachment').hide();
    var PersonnelId = $('#personnelModel_PersonnelId').val();
    if ($(document).find('#attachTable').hasClass('dataTable')) {
        dtAttachmentTable.destroy();
    }
    dtAttachmentTable = $("#attachTable").DataTable({
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
            "url": "/Personnel/PopulateAttachment",
            "data": { PersonnelId: PersonnelId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [5], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-danger delattachment gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "FullName",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_download_attachment href="javascript:void(0)"  target="_blank">' + row.FullName + '</a>';
                    }
                },

                { "data": "FileSizeWithUnit", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "CreateDate",
                    "type": "date "
                },
                {
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            $('#btnAddAttachment').show();
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnAddAttachment", function () {
    var PersonnelId = $('#personnelModel_PersonnelId').val();
    var ClientLookupId = $('#personnelModel_ClientLookupId').val();
    $.ajax({
        url: "/Personnel/AddAttachment",
        type: "GET",
        dataType: 'html',
        data: { PersonnelId: PersonnelId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#personnelcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            SetPersonnelEnvironmentPage();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('submit', "#frmpersonnelattachmentadd", function (e) {
    e.preventDefault();
    var form = document.querySelector('#frmpersonnelattachmentadd');
    if (!$('form').valid()) {
        return;
    }
    var data = new FormData(form);
    $.ajax({
        type: "POST",
        url: "/Personnel/AddAttachment",
        data: data,
        processData: false,
        contentType: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            AttachmentAddOnSuccess(data);
        },
        complete: function () {
            CloseLoader();
        },
        error: function (xhr) {
            console.log(xhr);
        }
    });
});
function AttachmentAddOnSuccess(data) {
    var PersonnelId = data.PersonnelId;
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
        swal(SuccessAlertSetting, function () {
            RedirectTopersonnelDetail(PersonnelId, "Attachment");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
$(document).on('click', '.lnk_download_attachment', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = dtAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/Personnel/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });
});
$(document).on('click', '.delattachment', function () {
    var data = dtAttachmentTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Personnel/DeleteAttachment',
            data: {
                fileAttachmentId: data.FileAttachmentId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                DeleteAttachment();
                if (data.Result == "success") {
                    dtAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("assignmentDeleteSuccessAlert"));
                    GenerateAttachmentGrid();
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
});
function DeleteAttachment() {
    SuccessAlertSetting.text = getResourceValue("assignmentDeleteSuccessAlert");
    swal(SuccessAlertSetting, function () {
    });
}
$(document).on('click', "#btnAttachcancel", function () {
    var PersonnelId = $('#attachmentModel_PersonnelId').val();
    swal(CancelAlertSetting, function () {
        RedirectTopersonnelDetail(PersonnelId, "Attachment");
    });
});
//#endregion

//#region Events
function GenerateEventsGrid() {
    $(document).find("#btnAddEvents").hide();
    var rCount = 0;
    var PersonnelId = $('#personnelModel_PersonnelId').val();
    if ($(document).find('#eventTable').hasClass('dataTable')) {
        dtEventTable.destroy();
    }
    dtEventTable = $("#eventTable").DataTable({
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
            "url": "/Personnel/PopulateEvents",
            "data": { PersonnelId: PersonnelId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                EventSecurity = response.security;
                rCount = response.data.length;
                if (rCount <= 0) {
                    $(document).find("#btnAddEvents").show();
                }
                else {
                    $(document).find("#btnAddEvents").hide();
                }
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                visible: false,
                "className": "text-center",
                targets: [4], render: function (a, b, data, d) {
                    var buttonHtml = '';
                    buttonHtml += EventSecurity.Create ? '<a class="btn btn-outline-primary addEventBttn gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' : '';
                    buttonHtml += EventSecurity.Edit ? '<a class="btn btn-outline-success editEventBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' : '';
                    buttonHtml += EventSecurity.Delete ? '<a class="btn btn-outline-danger delEventBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>' : '';
                    return buttonHtml;
                }

            }
        ],
        "columns":
            [
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "CompleteDate",
                    "type": "date "
                },
                {
                    "data": "ExpireDate",
                    "type": "date "
                },
            ],
        initComplete: function () {
            SetPageLengthMenu();

            if (EventSecurity.Create || EventSecurity.Edit || EventSecurity.Delete)
                this.api().column(4).visible(true);
        }
    });
}
$(document).on('click', ".addEventBttn,#btnAddEvents", function () {
    var PersonnelId = $('#personnelModel_PersonnelId').val();
    var ClientLookupId = $('#personnelModel_ClientLookupId').val();
    $(document).find('#eventmodel_CompleteDate').val("").trigger('change');
    $(document).find('#eventmodel_ExpireDate').val("").trigger('change');
    $.ajax({
        url: "/Personnel/AddOrEditEvents",
        type: "GET",
        dataType: 'html',
        data: { PersonnelId: PersonnelId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#personnelcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            SetPersonnelEnvironmentPage();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function AddOnSuccessEvents(data) {
    if (data.Result == "success") {
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("EventsAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("EventsUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectTopersonnelDetail(data.PersonnelId, "Events");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
$(document).on('click', ".editEventBttn", function () {
    var PersonnelId = $('#personnelModel_PersonnelId').val();
    var ClientLookupId = $('#personnelModel_ClientLookupId').val();
    var data = dtEventTable.row($(this).parents('tr')).data();
    $.ajax({
        url: "/Personnel/AddOrEditEvents",
        type: "GET",
        dataType: 'html',
        data: { EventId: data.EventsId, PersonnelId: PersonnelId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#personnelcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            SetPersonnelEnvironmentPage();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delEventBttn', function () {

    var data = dtEventTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        DeleteEvent(data);
    });
});
function DeleteEvent(data) {
    $.ajax({
        url: '/Personnel/DeleteEvent',
        data: {
            eventId: data.EventsId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                dtEventTable.state.clear();
                ShowDeleteAlert(getResourceValue("EventsDeleteAlert"));
                GenerateEventsGrid();
            }
        },
        complete: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#btneventcancel", function () {
    var PersonnelId = $('#eventmodel_PersonnelId').val();
    swal(CancelAlertSetting, function () {
        RedirectTopersonnelDetail(PersonnelId, "Events")
    });
});
//#endregion

//#region Labor
function GenerateLaborsGrid() {
    var PersonnelId = $('#personnelModel_PersonnelId').val();
    if ($(document).find('#LaborTable').hasClass('dataTable')) {
        dtLaborTable.destroy();
    }
    dtLaborTable = $("#LaborTable").DataTable({
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
            "url": "/Personnel/PopulateLabor",
            "data": { PersonnelId: PersonnelId },
            "type": "POST",
            "datatype": "json",
        },

        "columns":
            [
                { "data": "ChargeTo", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Date", "type": "date ", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Hours", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Cost", "autoWidth": true, "bSearchable": true, "bSortable": true },
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
//#endregion

//#region Availability 
$(document).on('click', "#btnAvailabilitycancel", function () {
    var PersonnelId = $('#AvailabilityPersonnelId').val();
    swal(CancelAlertSetting, function () {
        RedirectTopersonnelDetail(PersonnelId, "Availability");
    });
});
function GenerateAvailabilityGrid() {
    var AvailablilitySecurity;
    var PersonnelId = $('#personnelModel_PersonnelId').val();
    if ($(document).find('#availabilityTable').hasClass('dataTable')) {
        dtAvailabilityTable.destroy();
    }
    dtAvailabilityTable = $("#availabilityTable").DataTable({
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
            "url": "/Personnel/PopulateAvailability",
            "data": { PersonnelId: PersonnelId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                AvailablilitySecurity = response.security;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                visible: false,
                targets: [3], render: function (a, b, data, d) {
                    var buttonHtml = '';
                    buttonHtml += AvailablilitySecurity.Create ? '<a class="btn btn-outline-primary addAvailabilityBttn gridinnerbutton" title="Add"  onclick="EditAvailability(null);"><i class="fa fa-plus"></i></a>' : '';
                    buttonHtml += AvailablilitySecurity.Edit ? '<a class="btn btn-outline-success editAvailabilityBttn gridinnerbutton" title= "Edit" onclick="EditAvailability(' + data.PersonnelAvailabilityId + ');"> <i class="fa fa-pencil"></i></a>' : '';
                    buttonHtml += AvailablilitySecurity.Delete ? '<a class="btn btn-outline-danger delAvailabilityBttn gridinnerbutton" title="Delete" onclick="DeleteAvailability(' + data.PersonnelAvailabilityId + ');"> <i class="fa fa-trash"></i></a>' : '';
                    return buttonHtml;
                }
            }
        ],
        "columns":
            [
                { "data": "PersonnelAvailabilityDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PAHours", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PAShift", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            var rows = this.api().data().toArray().length;
            if (rows <= 0) {
                $('#btnAddAvailability').show();
            } else {
                $('#btnAddAvailability').hide();
            }

            if (AvailablilitySecurity.Create || AvailablilitySecurity.Edit || AvailablilitySecurity.Delete)
                this.api().column(3).visible(true);
        }
    });
}
$(document).on('click', "#btnAddAvailability", function () {
    EditAvailability(null);
});
function EditAvailability(PersonnelAvailabilityId) {
    var PersonnelId = $('#personnelModel_PersonnelId').val();
    var clientLookUpId = $('#personnelModel_ClientLookupId').val();
    $.ajax({
        url: "/Personnel/AddAvailability",
        type: "GET",
        dataType: 'html',
        data: { PersonnelId: PersonnelId, PersonnelAvailabilityId: PersonnelAvailabilityId, ClientLookupId: clientLookUpId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#personnelcontainer').html(data);
            $(document).find('.dtpicker').datepicker({
                minDate: new Date(),
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true
            }).inputmask('mm/dd/yyyy');
        },
        complete: function () {
            CloseLoader();
            SetPersonnelEnvironmentPage();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function DeleteAvailability(PersonnelAvailabilityId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Personnel/DeleteAvailability',
            data: {
                PersonnelAvailabilityId: PersonnelAvailabilityId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    dtAvailabilityTable.state.clear();
                    ShowDeleteAlert(getResourceValue("AvailabilityDeleteAlert"));
                    GenerateAvailabilityGrid();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function AddOnSuccessAvailability(data) {
    if (data.Result == "success") {
        if (data.Mode == "Add") {
            SuccessAlertSetting.text = getResourceValue("AvailabilityAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("AvailabilityUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectTopersonnelDetail(data.PersonnelId, "Availability");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
//#endregion

//#region Attendance
function GenerateAttendanceGrid() {
    $(document).find("#btnAddAttendance").hide();
    var rCount = 0;
    var PersonnelId = $('#personnelModel_PersonnelId').val();
    if ($(document).find('#attendanceTable').hasClass('dataTable')) {
        dtAttendanceTable.destroy();
    }
    dtAttendanceTable = $("#attendanceTable").DataTable({
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
            "url": "/Personnel/PopulateAttendance",
            "data": { PersonnelId: PersonnelId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                if (rCount > 0) {
                    $(document).find("#btnAddAttendance").hide();
                }
                else {
                    $(document).find("#btnAddAttendance").show();
                }
                AttendanceSecurity = response.security;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                visible: false,
                "className": "text-center",
                targets: [3], render: function (a, b, data, d) {
                    var buttonHtml = '';
                    buttonHtml += AttendanceSecurity.Create ? '<a class="btn btn-outline-primary addAttendenceBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' : '';
                    buttonHtml += AttendanceSecurity.Edit ? '<a class="btn btn-outline-success editAttendenceBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' : '';
                    buttonHtml += AttendanceSecurity.Delete ? '<a class="btn btn-outline-danger delAttendenceBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>' : '';
                    return buttonHtml;
                }
            }
        ],
        "columns":
            [
                { "data": "Date", "type": "date", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Hours", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "ShiftDescription", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            if (AttendanceSecurity.Create || AttendanceSecurity.Edit || AttendanceSecurity.Delete)
                this.api().column(3).visible(true);
        }
    });
}
$(document).on('click', ".addAttendenceBttn,#btnAddAttendance", function () {
    var PersonnelId = $('#personnelModel_PersonnelId').val();
    var ClientLookupId = $('#personnelModel_ClientLookupId').val();
    //$(document).find('#eventmodel_CompleteDate').val("").trigger('change');
    $.ajax({
        url: "/Personnel/AddOrEditAttendance",
        type: "GET",
        dataType: 'html',
        data: { PersonnelId: PersonnelId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#personnelcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            SetPersonnelEnvironmentPage();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function AddOnSuccessAttendance(data) {
    if (data.Result == "success") {
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AttendanceAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("AttendanceUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectTopersonnelDetail(data.PersonnelId, "Attendance");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
$(document).on('click', ".editAttendenceBttn", function () {
    var PersonnelId = $('#personnelModel_PersonnelId').val();
    var ClientLookupId = $('#personnelModel_ClientLookupId').val();
    var data = dtAttendanceTable.row($(this).parents('tr')).data();
    $.ajax({
        url: "/Personnel/AddOrEditAttendance",
        type: "GET",
        dataType: 'html',
        data: { PersonnelAttendanceId: data.PersonnelAttendanceId, PersonnelId: PersonnelId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#personnelcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            SetPersonnelEnvironmentPage();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delAttendenceBttn', function () {

    var data = dtAttendanceTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        DeleteAttendance(data);
    });
});
function DeleteAttendance(data) {
    $.ajax({
        url: '/Personnel/DeleteAttendance',
        data: {
            PersonnelAttendanceId: data.PersonnelAttendanceId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                dtAttendanceTable.state.clear();
                ShowDeleteAlert(getResourceValue("AttendanceDeleteAlert"));
                GenerateAttendanceGrid();
            }
        },
        complete: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#btnattendancecancel", function () {
    var PersonnelId = $('#personnelAttendanceModel_PersonnelId').val();
    swal(CancelAlertSetting, function () {
        RedirectTopersonnelDetail(PersonnelId, "Attendance");
    });
});
//#endregion

//#region Auxiliary - Information
$(document).on('click', '#addAuxInformation', function () {
    $('#AuxiliaryInformationModal').find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    $.validator.unobtrusive.parse(document);

    var strdate = $('#hiddenstrdate').val();
    var LastSalaryReview = $('#hiddenLastSalaryReview').val();
    var InitialPay = $('#hiddenInitialPay').val();
    var BasePay = $('#hiddenBasePay').val();

    $('#StartDate').val(strdate);
    $('#LastSalaryReview').val(LastSalaryReview);
    $('#auxiliaryInformation_InitialPay').val(InitialPay);
    $('#auxiliaryInformation_BasePay').val(BasePay);

    $('#AuxiliaryInformationModal').modal('show');
});
function AuxiliaryInformationUpdateOnSuccess(data) {
    if (data.Result == "success") {
        $('#AuxiliaryInformationModal').modal('hide');
        SuccessAlertSetting.text = getResourceValue("UpdateAuxiliaryInformationAlert");
        swal(SuccessAlertSetting, function () {
            RedirectTopersonnelDetail(data.PersonnelId, "");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion

//#region Common
function SetPersonnelEnvironmentPage() {
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

    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');

    $(document).find('.dtpicker_1').datepicker({
        minDate: new Date(),
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');

}
$(document).on('click', '.brdPersonnel', function () {
    var PersonnelId = $(this).attr('data-val');
    RedirectTopersonnelDetail(PersonnelId, "");
});
function RedirectTopersonnelDetail(PersonnelId, mode) {
    $.ajax({
        url: "/Personnel/PersonnelDetails",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { PersonnelId: PersonnelId },
        success: function (data) {
            $('#personnelcontainer').html(data);
        },
        complete: function () {
            if (mode === "Attachment") {
                $('#liAttachment').trigger('click');
                $('#mobilesidemenuselector').val('Attachments');
                GenerateAttachmentGrid();
            }
            if (mode == 'Events') {
                $('#liEvent').trigger('click');
                $('#mobilesidemenuselector').val('Events');
                GenerateEventsGrid();
            }
            if (mode == 'Labor') {
                $('#liLabor').trigger('click');
                $('#mobilesidemenuselector').val('Labor');
                GenerateLaborGrid();
            }
            if (mode == 'Availability') {
                $('#liAvailability').trigger('click');
                $('#mobilesidemenuselector').val('Availability');
                GenerateAvailabilityGrid();
            }
            if (mode == 'Attendance') {
                $('#liAttendance').trigger('click');
                $('#mobilesidemenuselector').val('Attendance');
                GenerateAttendanceGrid();
            }
            CloseLoader();
        }
    });
}
//#endregion

//#region Asset group master query
$(document).on('click', '#OpenAssetGroupMasterQueryModal', function () {
    AssignSelect2MultipleValue($('#hdnAssetGroup1Ids'), $('#AssetGroup1Ids'));
    AssignSelect2MultipleValue($('#hdnAssetGroup2Ids'), $('#AssetGroup2Ids'));
    AssignSelect2MultipleValue($('#hdnAssetGroup3Ids'), $('#AssetGroup3Ids'));
    $('#AssetGroupMasterQueryModal').modal('show');
    SetPersonnelEnvironmentPage();
});
function AssignSelect2MultipleValue(hdnAssetGroupId, Select2hdnAssetGroupId) {
    var data = new Array();
    $.each(hdnAssetGroupId.val().split(','), function (i, elem) {
        data[i] = elem;
    });
    Select2hdnAssetGroupId.val(data).trigger("change.select2");
}
function AssetGroupMasterQueryUpdateOnSuccess(data) {
    if (data.Result == "success") {
        $('#AssetGroupMasterQueryModal').modal('hide');
        SuccessAlertSetting.text = getResourceValue("UpdateMasterQuerySetup");
        swal(SuccessAlertSetting, function () {
            RedirectTopersonnelDetail(data.PersonnelId, "");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion