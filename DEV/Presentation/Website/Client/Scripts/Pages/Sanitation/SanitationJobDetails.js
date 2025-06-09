var dtpNotesTable;
//#region Notes
function GenerateSJNotesGrid() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    if ($(document).find('#SJnotesTable').hasClass('dataTable')) {
        dtpNotesTable.destroy();
    }
    dtpNotesTable = $("#SJnotesTable").DataTable({
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
            "url": "/SanitationJob/PopulateNotes?SanitationJobId=" + SanitationJobId,
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                targets: [3], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-success editSJnote gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delSJnote gridinnerbutton" title="Delete"><i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "ModifiedDate",
                    "type": "date "
                },
                {
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            CloseLoader();
        }
    });
}

$(document).on('click', "#btnSJAddNote", function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    $.ajax({
        url: "/SanitationJob/AddNotes",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: SanitationJobId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
        },
        complete: function () {
            SetSJControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});

$(document).on('click', '.editSJnote', function () {
    var data = dtpNotesTable.row($(this).parents('tr')).data();
    EditSJNote(data);
});

$(document).on('click', '.delSJnote', function () {
    var data = dtpNotesTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SanitationJob/DeleteNotes',
            data: {
                _notesId: data.NotesId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    dtpNotesTable.state.clear();
                    ShowDeleteAlert(getResourceValue("noteDeleteSuccessAlert"));
                    GenerateSJNotesGrid();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
});
$(document).on('click', '#brdSanitation', function () {
    var SanitationJobId = $(this).attr('data-val');
    RedirectToSJDetail(SanitationJobId);
});
$(document).on('click', "#btnSJnotescancel", function () {

    var SanitationJobId = $(document).find('#notesModel_SanitationJobId').val();
    RedirectToDetailOncancel(SanitationJobId, "notes");
});
function SanitationNotesAddOnSuccess(data) {
    var SanitationJobId = data.SanitationJobId;
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AddNoteAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("UpdateNoteAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToSJDetail(SanitationJobId, "notes");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
function EditSJNote(fullrecord) {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    var notesid = fullrecord.NotesId;
    var subject = fullrecord.Subject;
    var content = fullrecord.Content;
    var updatedindex = fullrecord.updatedindex;
    $.ajax({
        url: "/SanitationJob/EditNote",
        type: "GET",
        dataType: 'html',
        data: { ClientLookupId: ClientLookupId, SanitationJobId: SanitationJobId, notesId: notesid, subject: subject, content: content, updatedindex: updatedindex },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
        },
        complete: function () {
            SetSJControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
//#endregion
//#region Event Log
var SJEventLogTable;
function GenerateSJEventLogGrid() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    if ($(document).find('#SJEventLogTable').hasClass('dataTable')) {
        SJEventLogTable.destroy();
    }
    SJEventLogTable = $("#SJEventLogTable").DataTable({
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
            "url": "/SanitationJob/PopulateEventLog?SanitationJobId=" + SanitationJobId,
            "type": "POST",
            "datatype": "json"
        },
        "columns":
            [
                { "data": "Events", "autoWidth": false, "bSearchable": true, "bSortable": true, "width": "20%" },
                { "data": "Personnel", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "TransactionDate",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "type": "date"
                },
                { "data": "Comments", "autoWidth": true, "bSearchable": true, "bSortable": true },
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
//#endregion
//#region Attachments
var SJAttachmentTable;
function GenerateSJAttachmentsGrid() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var attchCount = 0;
    if ($(document).find('#SJAttachmentTable').hasClass('dataTable')) {
        SJAttachmentTable.destroy();
    }
    SJAttachmentTable = $("#SJAttachmentTable").DataTable({
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
            "url": "/SanitationJob/PopulateAttachments?SanitationJobId=" + SanitationJobId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                attchCount = response.recordsTotal;
                if (attchCount > 0) {
                    $(document).find('#sanitAttachmentCount').show();
                    $(document).find('#sanitAttachmentCount').html(attchCount);
                }
                else {
                    $(document).find('#sanitAttachmentCount').hide();
                }
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-danger delAttchBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "FileName",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_sensor_1 href="javascript:void(0)"  target="_blank">' + row.FullName + '</a>'
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
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnSJAddAttachment", function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    $.ajax({
        url: "/SanitationJob/AddAttachments",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: SanitationJobId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
        },
        complete: function () {
            SetSJControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delAttchBttn', function () {
    var data = SJAttachmentTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SanitationJob/DeleteAttachments',
            data: {
                _fileAttachmentId: data.FileAttachmentId
            },
            type: "GET",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    SJAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {

                GenerateSJAttachmentsGrid();
            }
        });
    });
});
function deleteConfirmSanitAttachment() {
    SuccessAlertSetting.text = getResourceValue("attachmentDeleteSuccessAlert");
    swal(SuccessAlertSetting, function () {
    });
}
$(document).on('click', '.lnk_sensor_1', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = SJAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/SanitationJob/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });
});
$(document).on('click', '#brdSJattachment', function () {
    var SanitationJobId = $(this).attr('data-val');
    RedirectToDetailOncancel(SanitationJobId);
});
$(document).on('click', "#btnSJattachmentcancel", function () {
    var SanitationJobId = $(document).find('#attachmentModel_SanitationJobId').val();
    RedirectToDetailOncancel(SanitationJobId, "attachment");
});

$(document).on('submit', "#frmSJattachmentadd", function (e) {
    e.preventDefault();
    var form = document.querySelector('#frmSJattachmentadd');
    if (!$('form').valid()) {
        return;
    }
    var data = new FormData(form);
    $.ajax({
        type: "POST",
        url: "/SanitationJob/AddAttachments",
        data: data,
        processData: false,
        contentType: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {

            if (data.data == "success") {
                var SanitationJobId = data.SanitationJobId;
                SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
                swal(SuccessAlertSetting, function () {
                    RedirectToSJDetail(SanitationJobId, "attachment");
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function (xhr) {
            CloseLoader();
        }
    });
});
//#endregion
//#region Labor
var SJLaborTable;
$(document).on('click', '.addLaborBttn', function () {
    AddLabor();
});
$(document).on('click', '#btnAddSJLabor', function () {
    var data = SJLaborTable.row($(this).parents('tr')).data();
    AddLabor();
});
$(document).on('click', '.editLaborBttn', function () {
    var data = SJLaborTable.row($(this).parents('tr')).data();
    EditLabor(data);
});
function EditLabor(fullRecord) {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    var chargeToId_Primary = fullRecord.ChargeToId_Primary;
    var hours = fullRecord.Hours;
    var startDate = fullRecord.StartDate;
    var personnelId = fullRecord.PersonnelId;
    var updateIndex = fullRecord.UpdateIndex;
    var timecardId = fullRecord.TimecardId;
    $.ajax({
        url: "/SanitationJob/EditLabor",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: SanitationJobId, ClientLookupId: ClientLookupId, chargeToId_Primary: chargeToId_Primary, hours: hours, startDate: startDate, personnelId: personnelId, updateIndex: updateIndex, timecardId: timecardId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
        },
        complete: function () {
            SetSJControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', '.delLaborBttn', function () {
    var data = SJLaborTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SanitationJob/DeleteLabor',
            data: {
                timecardId: data.TimecardId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    SJLaborTable.state.clear();
                    ShowDeleteAlert(getResourceValue("laborDeleteSuccessAlert"));
                    generateSJLaborGrid();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
});
function generateSJLaborGrid() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var rCount = 0;

    if ($(document).find('#SJLaborTable').hasClass('dataTable')) {
        SJLaborTable.destroy();
    }
    SJLaborTable = $("#SJLaborTable").DataTable({
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
            "url": "/SanitationJob/PopulateLabor?SanitationJobId=" + SanitationJobId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-success editLaborBttn gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delLaborBttn gridinnerbutton" title="Delete"><i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                { "data": "PersonnelClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "StartDate",
                    "type": "date "
                },
                {
                    "data": "Hours", defaultContent: "", "bSearchable": true, "bSortable": true
                },
                { "data": "Value", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "TimecardId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
function AddLabor() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    $.ajax({
        url: "/SanitationJob/AddLabor",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: SanitationJobId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
        },
        complete: function () {
            SetSJControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SJLaborAddOnSuccess(data) {
    var SanitationJobId = data.SanitationJobId;
    if (data.data == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("LaborAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("LaborUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToSJDetail(SanitationJobId, "labors");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#brdSJlabor", function () {

    var SanitationJobId = $(document).find('#laborModel_ChargeToId_Primary').val();
    RedirectToDetailOncancel(SanitationJobId);
});
$(document).on('click', "#btnSJlaborcancel", function () {
    var SanitationJobId = $(document).find('#laborModel_ChargeToId_Primary').val();

    RedirectToDetailOncancel(SanitationJobId, "labors");
});
//#endregion
//#region Assignment
var SJAssignTable;
$(document).on('click', '.addAssignmentBttn', function () {
    AddAssignment();
});
$(document).on('click', '#btnAddSJAssignment', function () {
    var data = SJAssignTable.row($(this).parents('tr')).data();
    AddAssignment();
});
$(document).on('click', '.editAssignmentBttn', function () {
    var data = SJAssignTable.row($(this).parents('tr')).data();
    EditAssignment(data);
});
function EditAssignment(fullRecord) {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    var scheduledHours = fullRecord.ScheduledHours;
    var scheduledStartDate = fullRecord.ScheduledStartDate;
    var personnelId = fullRecord.PersonnelId;
    var updateIndex = fullRecord.UpdateIndex;
    var sanitationJobScheduleId = fullRecord.SanitationJobScheduleId;
    $.ajax({
        url: "/SanitationJob/EditAssignment",
        type: "GET",
        dataType: 'html',
        data: { ClientLookupId: ClientLookupId, SanitationJobId: SanitationJobId, scheduledHours: scheduledHours, scheduledStartDate: scheduledStartDate, personnelId: personnelId, updateIndex: updateIndex, sanitationJobScheduleId: sanitationJobScheduleId, },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
        },
        complete: function () {
            SetSJControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', '.delAssignmentBttn', function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var data = SJAssignTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SanitationJob/DeleteAssignment',
            data: {
                SanitationJobScheduledId: data.SanitationJobScheduleId,
                SanitationJobId: SanitationJobId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                deleteConfirmSanitAssignment();
                if (data.Result == "success") {
                    SJAssignTable.state.clear();
                    ShowDeleteAlert(getResourceValue("assignmentDeleteSuccessAlert"));
                    generateSJAssignmentGrid();
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
function deleteConfirmSanitAssignment() {
    SuccessAlertSetting.text = getResourceValue("assignmentDeleteSuccessAlert");
    swal(SuccessAlertSetting, function () {
    });
}
$(document).on('click', "#btnSJAssignmentcancel", function () {
    var SanitationJobId = $(document).find('#assignmentModel_SanitationJobId').val();
    RedirectToDetailOncancel(SanitationJobId, "assignments");
});
function generateSJAssignmentGrid() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var IsAddOrDel = false;
    var rCount = 0;
    if ($(document).find('#SJAssignmentTable').hasClass('dataTable')) {
        SJAssignTable.destroy();
    }
    SJAssignTable = $("#SJAssignmentTable").DataTable({
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
            "url": "/SanitationJob/PopulateAssignment?SanitationJobId=" + SanitationJobId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                IsAddOrDel = response.IsAddOrDel;
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    if (IsAddOrDel) {
                        return '<a class="btn btn-outline-success editAssignmentBttn gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delAssignmentBttn gridinnerbutton" title="Delete"><i class="fa fa-trash"></i></a>';
                    } else {
                        return '<a class="btn btn-outline-danger delAssignmentBttn gridinnerbutton" title="Delete"><i class="fa fa-trash"></i></a>';
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "ScheduledStartDate",
                    "type": "date "
                },
                {
                    "data": "ScheduledHours", defaultContent: "", "bSearchable": true, "bSortable": true
                },
                {
                    "data": "SanitationJobScheduleId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            if (IsAddOrDel) {
                $("#btnAddSJAssignment").show();
            } else {
                $("#btnAddSJAssignment").hide();
            }
        }
    });
}
function AddAssignment() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    $.ajax({
        url: "/SanitationJob/AddAssignment",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: SanitationJobId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
        },
        complete: function () {
            SetSJControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SJAssignmentAddOnSuccess(data) {
    var SanitationJobId = data.SanitationJobId;
    if (data.data == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AssignmentAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("AssignmentUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToSJDetail(SanitationJobId, "assignments");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
$(document).on('click', "#brdSJassignment", function () {
    var SanitationJobId = $(document).find('#assignmentModel_SanitationJobId').val();
    RedirectToDetailOncancel(SanitationJobId);
});
$(document).on('click', "#btnSJassignmentcancel", function () {
    var SanitationJobId = $(document).find('#assignmentModel_SanitationJobId').val();
    SJAssignTable = undefined;
    RedirectToDetailOncancel(SanitationJobId, "assignments");
});
//#endregion
$(document).on('change', "#DemandModel_PlantLocationDescription", function () {
    var tlen = $(document).find("#DemandModel_PlantLocationDescription").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#DemandModel_PlantLocationDescription").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
    }
});
$(document).on('change', "#ODescribeModel_PlantLocationDescription", function () {
    var tlen = $(document).find("#ODescribeModel_PlantLocationDescription").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#ODescribeModel_PlantLocationDescription").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
    }
});
function SetControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $(document).find('#DemandModel_PlantLocationDescription').change(function () {
        $(this).valid();
    });
    $(document).find('#ODescribeModel_PlantLocationDescription').change(function () {
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
    SetFixedHeadStyle();
};
//#region Task
$(document).on('click', '.addTaskBttn', function () {
    var data = sjTaskTable.row($(this).parents('tr')).data();
    AddSjTask();
});
$(document).on('click', '#btnsjAddtask', function () {
    var data = sjTaskTable.row($(this).parents('tr')).data();
    AddSjTask();
});
$(document).on('click', '.editTaskBttn', function () {
    var data = sjTaskTable.row($(this).parents('tr')).data();
    EditSjTask(data.SanitationJobTaskId);
});
$(document).on('click', '.delTaskBttn', function () {
    var data = sjTaskTable.row($(this).parents('tr')).data();
    DeleteSjTask(data.SanitationJobTaskId);
});
$(document).on('click', "#brdsjtask", function () {
    var sjid = $(this).attr('data-val');
    RedirectToSJDetail(sjid);
});
function generateSjTaskGrid() {
    var sanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ChargeToClientLookupId = $(document).find('#JobDetailsModel_ChargeTo_ClientLookupId').val();
    var rCount = 0;
    var IsForAllSecurity = false;
    var visibility;
    $(document).find('#example-select-all-sensor').prop('checked', false);
    if ($(document).find('#sjTaskTable').hasClass('dataTable')) {
        sjTaskTable.destroy();
    }
    sjTaskTable = $("#sjTaskTable").DataTable({
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[1, "asc"]],
        stateSave: false,
        dom: 'rtlip',
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        "filter": true,
        "orderMulti": true,
        "ajax": {
            "url": "/SanitationJob/PopulateTask",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.sanitationJobId = sanitationJobId;
                d.ChargeToClientLookupId = ChargeToClientLookupId;
            },
            "dataSrc": function (response) {
                rCount = response.data.length;
                IsForAllSecurity = response.IsForAllSecurity;
                return response.data;
            }
        },
        columnDefs: [{
            "data": "SanitationJobTaskId",
            orderable: false,
            className: 'select-checkbox dt-body-center',
            targets: 0,
            'render': function (data, type, full, meta) {
                if ($('#example-select-all-sensor').is(':checked')) {
                    return '<input type="checkbox"  checked="checked" name="id[]" data-eqid="' + data + '" class="isSelect" value="' + $('<div/>').text(data).html() + '">';
                }
                else {
                    if (TaskIdsToupdate.indexOf(data) != -1) {
                        return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="isSelect ' + data + '"  value="'
                            + $('<div/>').text(data).html() + '">';
                    }
                    else {
                        return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="isSelect" value="' + $('<div/>').text(data).html() + '">';
                    }
                }
            }
        },
        {
            "data": null,
            targets: [4], render: function (a, b, data, d) {
                if (IsForAllSecurity) {
                    return '<a class="btn btn-outline-primary addTaskBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                        '<a class="btn btn-outline-success editTaskBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delTaskBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
                else {
                    return "";
                }
            }
        }
        ],
        "columns":
            [
                {},
                { "data": "TaskId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    render: function (data, type, row, meta) {
                        if (data == statusCode.Cancel) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--orange m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Complete) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Open) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else {
                            return getStatusValue(data);
                        }
                    }
                },
                { "data": "SanitationJobTaskId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }

            ],
        initComplete: function () {
            SetPageLengthMenu();
            if (rCount > 0 && IsForAllSecurity) {
                $("#btnsjAddtask").hide();
                $("#btnsjCompletetask").show();
                $("#btnsjCanceltask").show();
                $("#btnsjReOpentask").show();
            }
            else {
                {
                    if (IsForAllSecurity) {
                        $("#btnsjAddtask").show();
                    } else {
                        $("#btnsjAddtask").hide();
                    }
                    $("#btnsjCompletetask").hide();
                    $("#btnsjCanceltask").hide();
                    $("#btnsjReOpentask").hide();
                }
            }
        }
    });
}
var TaskIdsToupdate = [];
var TaskIdsToupdateReOpen = [];
var taskIDscancel;
$(document).on('change', '.isSelect', function () {
    var data = sjTaskTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        var el = $('#example-select-all-sensor').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        var index = TaskIdsToupdate.indexOf(data.SanitationJobTaskId);
        TaskIdsToupdate.splice(index, 1);

        var indexOpen = TaskIdsToupdateReOpen.indexOf(data.SanitationJobTaskId + ',' + data.Status);
        TaskIdsToupdateReOpen.splice(indexOpen, 1);
    }
    else {
        TaskIdsToupdate.push(data.SanitationJobTaskId);
        TaskIdsToupdateReOpen.push(data.SanitationJobTaskId + ',' + data.Status);
    }
});
$(document).on('click', '#example-select-all-sensor', function (e) {
    var checked = this.checked;
    if (checked) {
        var sanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
        var ChargeToClientLookupId = $(document).find('#JobDetailsModel_ChargeTo_ClientLookupId').val();
        TaskIdsToupdate = [];
        TaskIdsToupdateReOpen = [];
        $.ajax({
            "url": "/SanitationJob/PopulateTaskIds?sanitationJobId=" + sanitationJobId + '&ChargeToClientLookupId=' + ChargeToClientLookupId,
            async: true,
            type: "GET",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data) {
                    $.each(data, function (index, item) {
                        TaskIdsToupdate.push(item.SanitationJobTaskId);
                        TaskIdsToupdateReOpen.push(item.SanitationJobTaskId + ',' + item.Status);
                    });
                }
                else {
                    TaskIdsToupdate = [];
                    TaskIdsToupdateReOpen = [];
                }
            },
            complete: function () {
                sjTaskTable.column(0).nodes().to$().each(function (index, item) {
                    $(this).find('.isSelect').prop('checked', 'checked');
                });
                CloseLoader();
            }
        });
    }
    else {
        $(document).find('.isSelect').prop('checked', false);
        TaskIdsToupdate = [];
        TaskIdsToupdateReOpen = [];
    }
});
$(document).on('click', '#btnsjCompletetask', function () {
    var sjid = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var taskIds = null;
    taskIds = TaskIdsToupdate.join();
    if (taskIds !== "") {
        $.ajax({
            url: '/SanitationJob/CompleteTask',
            data: { taskList: taskIds, sanitationJobId: sjid },
            type: "GET",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "success" && data.successcount > 0) {
                    var msg = data.successcount + getResourceValue("taskCompleteAlert");
                    ShowSuccessAlert(msg);
                    TaskIdsToupdate = [];
                    TaskIdsToupdateReOpen = [];
                    taskIds = null;
                    generateSjTaskGrid();
                }
                else {
                    $(document).find('.isSelect').prop('checked', false);
                    $(document).find('#example-select-all-sensor').prop('checked', false);
                    var msg = getResourceValue("taskNotCompleteAlert");
                    swal({
                        title: getResourceValue("taskNotCompleteAlert"),
                        text: msg,
                        type: "error",
                        showCancelButton: false,
                        confirmButtonClass: "btn-sm btn-danger",
                        cancelButtonClass: "btn-sm",
                        confirmButtonText: getResourceValue("SaveAlertOk"),
                        cancelButtonText: getResourceValue("CancelAlertNo")
                    });
                }
                TaskIdsToupdate = [];
                TaskIdsToupdateReOpen = [];
                taskIds = null;
            },
            complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
    else {
        swal({
            title: getResourceValue("taskNotSelectedAlert"),
            text: getResourceValue("taskSelectAlert"),
            type: "warning",
            confirmButtonClass: "btn-sm btn-primary",
            confirmButtonText: getResourceValue("SaveAlertOk"),
        });
        return false;
    }
});
$(document).on('click', '#btnsjReOpentask', function () {
    var sjid = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var taskIds = null;
    taskIds = TaskIdsToupdateReOpen.join();
    if (taskIds !== "") {
        $.ajax({
            url: '/SanitationJob/ReOpenTask',
            data: { taskList: taskIds, sanitationJobId: sjid },
            type: "GET",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "success") {
                    var msg = data.successcount + getResourceValue("taskReopenAlert");
                    ShowSuccessAlert(msg);
                    TaskIdsToupdate = [];
                    TaskIdsToupdateReOpen = [];
                    taskIds = null;
                    generateSjTaskGrid();
                    $(document).find('.isSelect').prop('checked', false);
                }
                else {
                    $(document).find('.isSelect').prop('checked', false);
                    $(document).find('#example-select-all-sensor').prop('checked', false);
                    var msg = getResourceValue("taskNotReopenAlert");
                    swal({
                        title: getResourceValue("taskCantReopenAlert"),
                        text: msg,
                        type: "error",
                        showCancelButton: false,
                        confirmButtonClass: "btn-sm btn-danger",
                        cancelButtonClass: "btn-sm",
                        confirmButtonText: getResourceValue("SaveAlertOk")
                    });
                }
                TaskIdsToupdate = [];
                TaskIdsToupdateReOpen = [];
                taskIds = null;
            },
            complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
    else {
        swal({
            title: getResourceValue("taskNotSelectedAlert"),
            text: getResourceValue("taskSelectAlert"),
            type: "warning",
            confirmButtonClass: "btn-sm btn-primary",
            confirmButtonText: getResourceValue("SaveAlertOk"),
        });
        return false;
    }
});
$(document).on('click', '#btnsjCanceltask', function () {
    var sanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    taskIDscancel = null;
    taskIDscancel = TaskIdsToupdateReOpen.join();
    if (taskIDscancel !== "") {
        $.ajax({
            url: '/SanitationJob/PopulateCancelReasonDropdown',
            type: "GET",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $("#sanitationJobTaskModel_CancelReason").empty();
                $("#sanitationJobTaskModel_CancelReason").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < data.cancelReasonList.length; i++) {
                    var id = data.cancelReasonList[i].Value;
                    var name = data.cancelReasonList[i].Text;
                    $("#sanitationJobTaskModel_CancelReason").append("<option value='" + id + "'>" + name + "</option>");
                }
                $('#cancelModal').modal('show');
            },
            complete: function () {
                CloseLoader();
                $(document).find('.select2picker').select2({});
                $('#cancelModal').modal('hide');
            },
            error: function () {
                CloseLoader();
            }
        });
    }
    else {
        $('#cancelModal').modal('hide');
        swal({
            title: getResourceValue("taskNotSelectedAlert"),
            text: getResourceValue("taskSelectAlert"),
            type: "warning",
            confirmButtonClass: "btn-sm btn-primary",
            confirmButtonText: getResourceValue("SaveAlertOk"),
        });
        return false;
    }
});
$(document).on('click', '#btnSjTaskCancelOk', function () {
    var sjid = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var cancelreason = $(document).find('#sanitationJobTaskModel_CancelReason').val();
    if (cancelreason != "" && cancelreason != "--Select--") {
        $.ajax({
            url: '/SanitationJob/CancelTask',
            data: { taskList: taskIDscancel, cancelReason: cancelreason, sanitationJobId: sjid },
            type: "GET",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "success") {
                    var msg = data.successcount + getResourceValue("taskCancelAlert");
                    ShowSuccessAlert(msg);
                    TaskIdsToupdate = [];
                    TaskIdsToupdateReOpen = [];
                    taskIDscancel = null;
                    generateSjTaskGrid();
                }
                else {
                    $(document).find('.isSelect').prop('checked', false);
                    $(document).find('#example-select-all-sensor').prop('checked', false);
                    var msg = getResourceValue("taskNotCancelAlert");
                    swal({
                        title: getResourceValue("taskCancelFailedAlert"),
                        text: msg,
                        type: "error",
                        showCancelButton: false,
                        confirmButtonClass: "btn-sm btn-danger",
                        cancelButtonClass: "btn-sm",
                        confirmButtonText: getResourceValue("SaveAlertOk"),
                        cancelButtonText: getResourceValue("CancelAlertNo")
                    });
                }
                TaskIdsToupdate = [];
                TaskIdsToupdateReOpen = [];
                taskIDscancel = null;
            },
            complete: function () {
                CloseLoader();
                $('#cancelModal').modal('hide');
            },
            error: function () {
                CloseLoader();
            }
        });
    }
    else {
        swal({
            title: getResourceValue("taskNoCancelReasonAlert"),
            text: getResourceValue("taskSelectCancelReasonAlert"),
            type: "warning",
            showCancelButton: false,
            confirmButtonClass: "btn-sm btn-danger",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("SaveAlertOk"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        });
    }

});
$(document).on('click', '#btnSjTaskCancelCancel,.cancel-modal-close', function () {
    TaskIdsToupdate = [];
    TaskIdsToupdateReOpen = [];
    taskIDscancel = null;
    $(document).find('.isSelect').prop('checked', false);
    $(document).find('#example-select-all-sensor').prop('checked', false);
    $('#cancelModal').modal('hide');
});
function AddSjTask() {
    var sjID = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    var ChargeToClientLookupId = $(document).find('#JobDetailsModel_ChargeTo_ClientLookupId').val();
    var ChargeType = $(document).find('#JobDetailsModel_ChargeType').val();
    $.ajax({
        url: "/SanitationJob/AddTasks",
        type: "GET",
        dataType: 'html',
        data: { sanitationJobId: sjID, ClientLookupId: ClientLookupId, ChargeToClientLookupId: ChargeToClientLookupId, ChargeType: ChargeType },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
        },
        complete: function () {
            SetControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SjTaskAddOnSuccess(data) {
    CloseLoader();
    var sjid = data.sjid;
    var sjid = $(document).find('#sanitationJobTaskModel_SanitationJobId').val();
    if (data.data == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("TaskAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("TaskUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToSJDetail(sjid, "tasks");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditSjTask(taskid) {
    var sjID = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    $.ajax({
        url: "/SanitationJob/EditTasks",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: sjID, _taskId: taskid, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
        },
        complete: function () {
            SetControls();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function DeleteSjTask(taskId) {
    var sjid = $(document).find('#sanitationJobTaskModel_SanitationJobId').val();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SanitationJob/DeleteTasks',
            data: {
                taskNumber: taskId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    sjTaskTable.state.clear();
                    ShowDeleteAlert(getResourceValue("taskDeleteSuccessAlert"));
                    generateSjTaskGrid();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnsjtaskcancel", function () {
    var sjid = $(document).find('#sanitationJobTaskModel_SanitationJobId').val();
    sjTaskTable = undefined;
    RedirectToDetailOncancel(sjid, "tasks");
});
function RedirectToDetailOncancel(sjid, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToSJDetail(sjid, mode);
    });
}
//#endregion
//#region tools
var SJToolsTable;
$(document).on('click', '.addToolsBttn', function () {
    AddTools();
});
$(document).on('click', '#btnAddSJTools', function () {
    var data = SJToolsTable.row($(this).parents('tr')).data();
    AddTools();
});
$(document).on('click', '.editToolsBttn', function () {
    var data = SJToolsTable.row($(this).parents('tr')).data();
    EditTools(data);
});
function EditTools(fullRecord) {
    var sanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var clientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    var categoryValue = fullRecord.CategoryValue;
    var description = fullRecord.Description;
    var instructions = fullRecord.Instructions;
    var quantity = fullRecord.Quantity;
    var sanitationPlanningId = fullRecord.SanitationPlanningId;
    $.ajax({
        url: "/SanitationJob/EditTools",
        type: "GET",
        dataType: 'html',
        data: { ClientLookupId: clientLookupId, SanitationJobId: sanitationJobId, categoryValue: categoryValue, description: description, instructions: instructions, quantity: quantity, sanitationPlanningId: sanitationPlanningId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
        },
        complete: function () {
            SetSJControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', '.delToolsBttn', function () {
    var data = SJToolsTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SanitationJob/DeleteTools',
            data: {
                _SanitationPlanningId: data.SanitationPlanningId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    SJToolsTable.state.clear();
                    ShowDeleteAlert(getResourceValue("toolsDeleteSuccessAlert"));
                    generateSJToolsGrid();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
});
$(document).on('click', "#btnSJToolscancel", function () {
    var SanitationJobId = $(document).find('#toolsModel_SanitationJobId').val();
    RedirectToDetailOncancel(SanitationJobId, "tools");
});
function generateSJToolsGrid() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var rCount = 0;

    var IsAddOrEdit = false;
    var IsDelelte = false;
    if ($(document).find('#SJToolsTable').hasClass('dataTable')) {
        SJToolsTable.destroy();
    }
    SJToolsTable = $("#SJToolsTable").DataTable({
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
            "url": "/SanitationJob/PopulateTools?SanitationJobId=" + SanitationJobId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                IsAddOrEdit = response.IsAddOrEdit;
                IsDelelte = response.IsDelelte;
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {

                    if (IsAddOrEdit && IsDelelte) {
                        return '<a class="btn btn-outline-success editToolsBttn gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delToolsBttn gridinnerbutton" title="Delete"><i class="fa fa-trash"></i></a>';
                    } else if (IsDelelte) {
                        return '<a class="btn btn-outline-danger delToolsBttn gridinnerbutton" title="Delete"><i class="fa fa-trash"></i></a>';
                    }
                    else if (IsAddOrEdit) {
                        return '<a class="btn btn-outline-success editToolsBttn gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "CategoryValue", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "Instructions"
                },
                {
                    "data": "Quantity", defaultContent: "", "bSearchable": true, "bSortable": true
                },
                {
                    "data": "SanitationPlanningId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            if (IsAddOrEdit) {
                $("#btnAddSJTools").show();
            }
            else {
                $("#btnAddSJTools").hide();
            }
        }
    });
}
function AddTools() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    $.ajax({
        url: "/SanitationJob/AddTools",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: SanitationJobId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
        },
        complete: function () {
            SetSJControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SJToolsAddOnSuccess(data) {
    CloseLoader();
    var SanitationJobId = data.SanitationJobId;
    if (data.data == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("ToolAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("ToolUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToSJDetail(SanitationJobId, "tools");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#brdSJtools", function () {

    var SanitationJobId = $(document).find('#toolModel_SanitationJobId').val();
    RedirectToDetailOncancel(SanitationJobId);
});
$(document).on('click', "#btnSJtoolscancel", function () {
    var SanitationJobId = $(document).find('#toolModel_SanitationJobId').val();
    SJToolsTable = undefined;
    RedirectToDetailOncancel(SanitationJobId, "tools");
});
$(function () {
    $(document).on('change', "#toolModel_CategoryValue", function () {
        $(document).find('#toolModel_hdnDropdownDescription').val($("#toolModel_CategoryValue option:selected").text());
    });
});
//#endregion
//#region ChemicalSupplies
var SJChemicalSuppliesTable;
$(document).on('click', '.addChemicalSuppliesBttn', function () {
    AddChemicalSupplies();
});
$(document).on('click', '#btnAddSJChemicalSupplies', function () {
    var data = SJChemicalSuppliesTable.row($(this).parents('tr')).data();
    AddChemicalSupplies();
});
$(document).on('click', '.editChemicalSuppliesBttn', function () {
    var data = SJChemicalSuppliesTable.row($(this).parents('tr')).data();
    EditChemicalSupplies(data);
});
function EditChemicalSupplies(fullRecord) {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    var sanitationPlanningId = fullRecord.SanitationPlanningId;
    var categoryValue = fullRecord.CategoryValue;
    var description = fullRecord.Description;
    var instructions = fullRecord.Instructions;
    var dilution = fullRecord.Dilution;
    var quantity = fullRecord.Quantity;
    $.ajax({
        url: "/SanitationJob/EditChemicalSupplies",
        type: "GET",
        dataType: 'html',
        data: { ClientLookupId: ClientLookupId, SanitationJobId: SanitationJobId, sanitationPlanningId: sanitationPlanningId, categoryValue: categoryValue, description: description, instructions: instructions, dilution: dilution, quantity: quantity },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
        },
        complete: function () {
            SetSJControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', '.delChemicalSuppliesBttn', function () {
    var data = SJChemicalSuppliesTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SanitationJob/DeleteChemicalSupplies',
            data: {
                _SanitationPlanningId: data.SanitationPlanningId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    SJChemicalSuppliesTable.state.clear();
                    ShowDeleteAlert(getResourceValue("chemicalsDeleteSuccessAlert"));
                    generateSJChemicalSuppliesGrid();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
});
$(document).on('click', "#btnSJChemicalSuppliescancel", function () {
    var SanitationJobId = $(document).find('#ChemicalSuppliesModel_SanitationJobId').val();
    RedirectToDetailOncancel(SanitationJobId, "ChemicalSupplies");
});
function generateSJChemicalSuppliesGrid() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var rCount = 0;
    var IsAddOrEditOrDel = false;
    if ($(document).find('#SJChemicalSuppliesTable').hasClass('dataTable')) {
        SJChemicalSuppliesTable.destroy();
    }
    SJChemicalSuppliesTable = $("#SJChemicalSuppliesTable").DataTable({
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
            "url": "/SanitationJob/PopulateChemicalSupplies?SanitationJobId=" + SanitationJobId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                IsAddOrEditOrDel = response.IsAddOrEditOrDel;
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    if (IsAddOrEditOrDel) {
                        return '<a class="btn btn-outline-success editChemicalSuppliesBttn gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delChemicalSuppliesBttn gridinnerbutton" title="Delete"><i class="fa fa-trash"></i></a>';
                    } else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "CategoryValue", "autoWidth": false, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "Instructions"
                },
                {
                    "data": "Quantity", defaultContent: "", "bSearchable": true, "bSortable": true
                },
                {
                    "data": "SanitationPlanningId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            if (IsAddOrEditOrDel) {
                $("#btnAddSJChemicalSupplies").show();
            }
            else {
                $("#btnAddSJChemicalSupplies").hide();
            }

        }
    });
}
function AddChemicalSupplies() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    $.ajax({
        url: "/SanitationJob/AddChemicalSupplies",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: SanitationJobId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
        },
        complete: function () {
            SetSJControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SJChemicalSuppliesAddOnSuccess(data) {
    CloseLoader();
    var SanitationJobId = data.SanitationJobId;
    if (data.data == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("ChemicalSupplyAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("ChemicalSupplyUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToSJDetail(SanitationJobId, "ChemicalSupplies");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#brdSJChemicalSupplies", function () {
    var SanitationJobId = $(document).find('#chemicalSuppliesModel_SanitationJobId').val();
    RedirectToDetailOncancel(SanitationJobId);
});
$(document).on('click', "#btnSJChemicalSuppliescancel", function () {
    var SanitationJobId = $(document).find('#chemicalSuppliesModel_SanitationJobId').val();
    SJChemicalSuppliesTable = undefined;
    RedirectToDetailOncancel(SanitationJobId, "ChemicalSupplies");
});
$(function () {
    $(document).on('change', "#chemicalSuppliesModel_CategoryValue", function () {
        $(document).find('#chemicalSuppliesModel_hdnDropdownDescription').val($("#chemicalSuppliesModel_CategoryValue option:selected").text());
    });
});
//#endregion

//#region CancelSanitation
$(document).on('click', '#cancelModalCall,#btnCancelSanitations', function () {
    var thisid = $(this).attr('id');
    if (thisid === 'btnCancelSanitations') {
        var found = false;
        for (var i = 0; i < SelectedSanitationCancel.length; i++) {
            if (SelectedSanitationCancel[i].Status !== 'Canceled' && SelectedSanitationCancel[i].Status !== 'Complete' && SelectedSanitationCancel[i].Status !== 'Passed' && SelectedSanitationCancel[i].Status !== 'Fail') {
                found = true;
                break;
            }
        }
        if (found === false) {
            var errorMessage = getResourceValue("CancelCompleteSanitationAlert");
            ShowErrorAlert(errorMessage);
            return false;
        }
    }
    $.ajax({
        url: '/SanitationJob/PopulateCancelReasonDropdown',
        type: "GET",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $("#txtCancelReasonSelect").empty();
            $("#txtCancelReasonSelect").append("<option value=''>" + "--Select--" + "</option>");
            for (var i = 0; i < data.cancelReasonList.length; i++) {
                var id = data.cancelReasonList[i].Value;
                var name = data.cancelReasonList[i].Text;
                $("#txtCancelReasonSelect").append("<option value='" + id + "'>" + name + "</option>");
            }
        },
        complete: function () {
            $.validator.unobtrusive.parse(document);
            $(document).find('#txtCancelReasonSelect').val('').trigger('change.select2').removeClass('input-validation-error');
            $(document).find('#txtcancelcomments').val('').removeClass('input-validation-error');
            $(document).find('.select2picker').select2({});
            if (thisid == 'cancelModalCall') {
                var id = $('#JobDetailsModel_SanitationJobId').val();
                $('#sanitationCancelAndPrintListModel_SanitationJobId').val(id);
                $(document).find('#cancelModalDetailsPage').modal('show');
            }
            else {
                $(document).find('#cancelModalSearchPage').modal('show');
            }
            CloseLoader();
        }
    });
});

function SanitationJobCancelOnSuccess(data) {

    $('#cancelModalDetailsPage').modal('hide');
    if (data.data == "success") {
        SuccessAlertSetting.text = getResourceValue("jobCancelsuccessmsg");


        swal(SuccessAlertSetting, function () {
            CloseLoader();
            RedirectToSJDetail(data.SanitationJobId, "overview");
        });
    }
    else {
        GenericSweetAlertMethod(data);
    }
}


$(document).on('click', '.cancelJobBtn', function () {

    var areaddescribedby = $(document).find("#txtCancelReasonSelect").attr('aria-describedby');
    if (typeof areaddescribedby !== 'undefined') {
        $('#' + areaddescribedby).hide();
    }
});


//#endregion

//#region Complete-From-Grid
function CompleteModelList(ClientLookupId, SanitationJobId, Status) {
    this.ClientLookupId = ClientLookupId;
    this.SanitationJobId = SanitationJobId;
    this.Status = Status;
}
$(document).on('click', '#btnCompleteSanitations', function () {
    var found = true;
    var status;
    for (var i = 0; i < SelectedSanitationCancel.length; i++) {
        status = SelectedSanitationCancel[i].Status;
        if (status == statusCode.Approved || status == statusCode.Scheduled || status == statusCode.JobRequest) {
            found = false;
            break;
        }
    }
    if (found === true) {
        var errorMessage = getResourceValue("SanitJobBatchCompletionAlert");
        ShowErrorAlert(errorMessage);
        return false;
    }
    else {
        $(document).find('#txtcompletecomments').val('').removeClass('input-validation-error');
        $(document).find('#completeModalSearchPage').modal('show');
    }

});
$(document).on('click', '#completeSatinationJob', function () {
    run = true;
    var completeModelArray = [];
    var comments = $(document).find('#txtcompletecomments').val();
    for (var j = 0; j < SelectedSanitationCancel.length; j++) {
        status = SelectedSanitationCancel[j].Status;
        if (status == 'Approved' || status == 'Scheduled' || status == 'JobRequest') {
            var thisArray = new CompleteModelList(SelectedSanitationCancel[j].ClientLookupId, SelectedSanitationCancel[j].SanitationJobId, SelectedSanitationCancel[j].Status);
            completeModelArray.push(thisArray);
        }
    }
    var jsonResult = {
        "list": completeModelArray,
        "comments": comments
    };
    var List = JSON.stringify(jsonResult);
    $.ajax({
        url: '/SanitationJob/CompleteSanitationJobList',
        type: "POST",
        datatype: "json",
        data: List,
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var dataItems = "<ul style='list-style:none;padding: 0;margin: 0;text-align:left;'>";
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                dataItems += "<li><i class='fa fa-circle bull'></i>" + data[i].ClientLookupId + " " + data[i].Status + " " + data[i].ErrMsg + "</li>";
            }
            dataItems += "</ul>";
            swal({
                title: getResourceValue("SanitJobCompletionStatAlert"),
                text: dataItems,
                html: true,
                type: "info",
                confirmButtonClass: "btn-sm btn-primary",
                confirmButtonText: getResourceValue("SaveAlertOk")
            }, function () {
                $(document).find('#txtcompletecomments').val('');
                $(".updateArea").hide();
                $(".actionBar").fadeIn();
                $(document).find('.chksearch').prop('checked', false);
                $(document).find('.itemcount').text(0);
                SelectedSanitationCancel = [];
                dtTable.page('first').draw('page');
            });
        },
        complete: function () {
            CloseLoader();

            $('#completeModalSearchPage').modal('hide');
        }
    });
});
//#endregion

//#region Asset popup
$(document).on('click', "#openOJobAssetgrid", function () {
    generateAssetPopupTable();
});
//#endregion
//#region V2-912
$(document).on('click', '#SanitationJobApprove', function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();

    $.ajax({
        url: '/SanitationJob/SanitationJobApprove',
        data: {
            SanitationJobId: SanitationJobId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                SuccessAlertSetting.text = getResourceValue("alertSanitationJobApproved");
                swal(SuccessAlertSetting, function () {
                    CloseLoader();
                    RedirectToSJDetail(data.SanitationJobId, "overview");
                });
            } else {
                alert('error');
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

$(document).on('click', '#SanitationJobComplete', function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();

    $.ajax({
        url: '/SanitationJob/SanitationJobComplete',
        data: {
            SanitationJobId: SanitationJobId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                SuccessAlertSetting.text = getResourceValue("alertSanitationJobCompleted");
                swal(SuccessAlertSetting, function () {
                    CloseLoader();
                    RedirectToSJDetail(data.SanitationJobId, "overview");
                });
            } else {
                alert('error');
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

$(document).on('click', '#SanitationJobPass', function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();

    $.ajax({
        url: '/SanitationJob/SanitationJobPass',
        data: {
            SanitationJobId: SanitationJobId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                SuccessAlertSetting.text = getResourceValue("alertSanitationJobPassed");
                swal(SuccessAlertSetting, function () {
                    CloseLoader();
                    RedirectToSJDetail(data.SanitationJobId, "overview");
                });
            } else {
                alert('error');
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
//#region Fail Sanitation
$(document).on('click', '#FailModalCall', function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();

    $.ajax({
        url: "/SanitationJob/GetFailVarificationSanitationJob",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: SanitationJobId},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#FailSanitationpopup').html('');
            $('#FailSanitationpopup').html(data);
            $('#FailSanitationModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            // $('#FailSanitationModalpopup').modal('show');
            SetSJControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});

function SanitationJobFailVarificationOnSuccess(data) {
    $('.modal-backdrop').remove();
    $('#FailSanitationModalpopup').modal('hide');
    if (data.data == "success") {
        SuccessAlertSetting.text = getResourceValue("alertSVFailedReasonVarification");


        swal(SuccessAlertSetting, function () {
            CloseLoader();
            RedirectToSJDetail(data.SanitationJobId, "overview");
        });
    }
    else {
        GenericSweetAlertMethod(data);
    }
}


$(document).on('click', '.FailJobBtn', function () {

    var areaddescribedby = $(document).find("#txtfailReasonSelect").attr('aria-describedby');
    if (typeof areaddescribedby !== 'undefined') {
        $('#' + areaddescribedby).hide();
    }
});


//#endregion
//endregion

//#region V2-988
$(document).on('click', '.sjreadmoredescription', function () {
    $(document).find('#sjdetaildesmodaltext').text($(this).data("des"));
    $(document).find('#sjdetaildesmodal').modal('show');
});
//#endregion

//#region Add Work Request V2-1055
$(document).on('click', "#AddWorkRequestBtn", function (e) {
    var SJId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var SJChargeToId = $(document).find('#SJChargeToId').val();
    var SJChargeTo_ClientLookupId = $(document).find('#JobDetailsModel_ChargeTo_ClientLookupId').val();
    $.ajax({
        url: "/SanitationJob/AddWoRequestDynamicInSanitationJobDeatails",
        type: "GET",
        dataType: 'html',
        data: {
            'SJChargeToId': SJChargeToId,
            'SJChargeTo_ClientLookupId': SJChargeTo_ClientLookupId,
            'SJId': SJId,
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddWorkRequestDiv').html(data);
            $('#AddWorkRequestModalDialog').modal("show");
        },
        complete: function () {
            SetControls();
            $(document).find('.dtpicker').datepicker({
                minDate: 0,
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true
            }).inputmask('mm/dd/yyyy');
        },
        error: function () {
            CloseLoader();
        }
    });
});
function WorkRequestDynamicAddOnSuccess(data) {
    if (data.data === "success") {
        $("#AddWorkRequestModalDialog").modal('hide');
        if (data.Command === "save") {
            SuccessAlertSetting.text = getResourceValue("spnWoRequestAddSuccessfully");
            swal(SuccessAlertSetting, function () {
                CloseLoader();
               
            });
        }
    }
    else {
        CloseLoader();
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#btnCancelAddWorkRequest', function () {
    $("#AddWorkRequestModalDialog").modal('hide');
});
$(document).on('hidden.bs.modal', '#AddWorkRequestModalDialog', function () {

});


//#endregion