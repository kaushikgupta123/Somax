//#region Task
$(document).on('click', '#brdMsTask', function () {
    var msid = $(this).attr('data-val');
    RedirectToMSDetail(msid);
});
var msTaskTable;
function generateMsTaskGrid() {
    var sanitationMasterId = $(document).find('#MasterSanitModel_SanitationMasterId').val();
    var rCount = 0;
    var visibility;
    var tskSecurity;
    if ($(document).find('#msTaskTable').hasClass('dataTable')) {
        msTaskTable.destroy();
    }
    msTaskTable = $("#msTaskTable").DataTable({
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
        "order": [[0, "asc"]],
        stateSave: false,
        dom: 'rtlip',
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        "filter": true,
        "orderMulti": true,
        "ajax": {
            "url": "/MasterSanitationSchedule/PopulateMastSanitTask",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.sanitationMasterId = sanitationMasterId;
            },
            "dataSrc": function (response) {
                tskSecurity = response.tskSecurity;
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [2], render: function (a, b, data, d) {
                    if (tskSecurity) {
                        return '<a class="btn btn-outline-primary addTaskBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editTaskBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delTaskBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            },
        ],
        "columns":
        [
            { "data": "TaskId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-600'>" + data + "</div>";
                }
            },
            { "data": "SanitationMasterTaskId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            if (rCount || tskSecurity == false > 0) {
                $("#btnMsTaskAdd").hide();
            }
            else {
                $("#btnMsTaskAdd").show();
            }
        }
    });
}
$(document).on('click', '.addTaskBttn,#btnMsTaskAdd', function () {
    var data = msTaskTable.row($(this).parents('tr')).data();
    AddMsTask();
});
function AddMsTask() {
    var sanitationMasterId = $(document).find('#MasterSanitModel_SanitationMasterId').val();
    var msDescription = $(document).find('#MasterSanitModel_Description').val();
    $.ajax({
        url: "/MasterSanitationSchedule/AddOrEditTasks",
        type: "GET",
        dataType: 'html',
        data: { sanitationMasterId: sanitationMasterId, msDescription: msDescription },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermasterschedule').html(data);
            CloseLoader();
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
function MsTaskAddOnSuccess(data) {
    CloseLoader();
    var msid = $(document).find('#MasterSanTaskModel_SanitationMasterId').val();
    if (data.data == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("TaskAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("TaskUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToMSDetail(msid, "SOTasks");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnmstaskcancel", function () {
    var msid = $(document).find('#MasterSanTaskModel_SanitationMasterId').val();
    msTaskTable = undefined;
    RedirectToMSDetailOnCancel(msid, "SOTasks");
});
$(document).on('click', '.editTaskBttn', function () {
    var data = msTaskTable.row($(this).parents('tr')).data();
    EditMsTask(data.SanitationMasterTaskId);
});
$(document).on('click', '.delTaskBttn', function () {
    var data = msTaskTable.row($(this).parents('tr')).data();
    DeleteMsTask(data.SanitationMasterTaskId);
});
function EditMsTask(taskid) {
    var msID = $(document).find('#MasterSanitModel_SanitationMasterId').val();
    var desc = $(document).find('#MasterSanitModel_Description').val(); 
    $.ajax({
        url: "/MasterSanitationSchedule/AddOrEditTasks",
        type: "GET",
        dataType: 'html',
        data: { sanitationMasterId: msID, msDescription: desc, sanitationMasterTaskId: taskid },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermasterschedule').html(data);
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
function DeleteMsTask(taskId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/MasterSanitationSchedule/DeleteTasks',
            data: {
                sanitationMasterTaskId: taskId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    msTaskTable.state.clear();
                    ShowDeleteAlert(getResourceValue("taskDeleteSuccessAlert"));
                    generateMsTaskGrid();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
//#endregion Task

//#region Notes
$(document).on('click', '#brdMsNote', function () {
    var msid = $(this).attr('data-val');
    RedirectToMSDetail(msid);
});
var soNotesTable;
function GenerateMsNotesGrid() {
    var SanitationMasterId = $(document).find('#MasterSanitModel_SanitationMasterId').val();
    if ($(document).find('#tblSONotes').hasClass('dataTable')) {
        soNotesTable.destroy();
    }
    soNotesTable = $("#tblSONotes").DataTable({
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
            "url": "/MasterSanitationSchedule/PopulateNotes",
            data: function (d) {
                d.SanitationMasterId = SanitationMasterId;
            },
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                targets: [3], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-success editNoteBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delNoteBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
        [
            { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "ModifiedDate", "type": "date " },
            { "bSortable": false, "className": "text-center" }
        ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnMsAddNote", function () {
    var sanitationMasterId = $(document).find('#MasterSanitModel_SanitationMasterId').val();
    var msDescription = $(document).find('#MasterSanitModel_Description').val();
    $.ajax({
        url: "/MasterSanitationSchedule/AddNotes",
        type: "GET",
        dataType: 'html',
        data: { sanitationMasterId: sanitationMasterId, msDescription: msDescription },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermasterschedule').html(data);
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function MsNoteAddOnSuccess(data) {
    CloseLoader();
    var msid = $(document).find('#MasterSanNotesModel_SanitationMasterId').val();
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AddNoteAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("UpdateNoteAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToMSDetail(msid, "SONotes");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '.editNoteBttn', function () {
    var data = soNotesTable.row($(this).parents('tr')).data();
    EditMsNote(data);
});
function EditMsNote(fullrecord) {
    var sanitationMasterId = $(document).find('#MasterSanitModel_SanitationMasterId').val();
    var ClientLookupId = $(document).find('#MasterSanitModel_Description').val();
    var notesid = fullrecord.NotesId;
    $.ajax({
        url: "/MasterSanitationSchedule/EditNote",
        type: "GET",
        dataType: 'html',
        data: { sanitationMasterId: sanitationMasterId, notesId: notesid, msDescription: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermasterschedule').html(data);
        },
        complete: function () {
            SetControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', "#btnnotescancel", function () {
    var msid = $(document).find('#MasterSanNotesModel_SanitationMasterId').val();
    soNotesTable = undefined;
    RedirectToMSDetailOnCancel(msid, "SONotes");
});
$(document).on('click', '.delNoteBttn', function () {
    var data = soNotesTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/MasterSanitationSchedule/DeleteNotes',
            data: {
                notesId: data.NotesId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    soNotesTable.state.clear();
                    ShowDeleteAlert(getResourceValue("noteDeleteSuccessAlert"));
                    GenerateMsNotesGrid();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
});
//#endregion

//#region Attachment
$(document).on('click', '#brdMsAttach', function () {
    var msid = $(this).attr('data-val');
    RedirectToMSDetail(msid);
});
var msAttachmentTable
function generateMsAttachmentsGrid() {
    var SanitationMasterId = $(document).find('#MasterSanitModel_SanitationMasterId').val();
    var attchCount = 0;
    if ($(document).find('#tblSOAttachment').hasClass('dataTable')) {
        msAttachmentTable.destroy();
    }
    msAttachmentTable = $("#tblSOAttachment").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "bProcessing": true,
        serverSide: true,
        "pagingType": "full_numbers",
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
            "url": "/MasterSanitationSchedule/PopulateMSAttachments?SanitationMasterId=" + SanitationMasterId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                attchCount = response.recordsTotal;
                if (attchCount > 0) {
                    $(document).find('#mastsanitAttachmentCount').show();
                    $(document).find('#mastsanitAttachmentCount').html(attchCount);
                }
                else {
                    $(document).find('#mastsanitAttachmentCount').hide();
                }
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-danger delMSAttachment gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
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
                    return '<a class=lnk_download_attachment href="' + '/MasterSanitationSchedule/DowloadUmAttachment?fileinfoId=' + row.FullName + '"  target="_blank">' + row.FullName + '</a>'
                }
            },
            { "data": "FileSizeWithUnit", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "CreateDate", "type": "date " },
            { "bSortable": false, "className": "text-center" }
        ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnMsAddAttachment", function () {
    var sanitationMasterId = $(document).find('#MasterSanitModel_SanitationMasterId').val();
    var description = $(document).find('#MasterSanitModel_Description').val();
    $.ajax({
        url: "/MasterSanitationSchedule/AddMsAttachments",
        type: "GET",
        dataType: 'html',
        data: { sanitationMasterId: sanitationMasterId, description: description },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermasterschedule').html(data);
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btnMsAtchCancel", function () {
    var msid = $(document).find('#MasterSanAttachmentModel_SanitationMasterId').val();
    RedirectToMSDetailOnCancel(msid, "SOAttachments");
});
$(document).on('submit', "#frmMsAttachmentadd", function (e) {
    e.preventDefault();
    var form = document.querySelector('#frmMsAttachmentadd');
    if (!$('form').valid()) {
        return;
    }
    var data = new FormData(form);
    $.ajax({
        type: "POST",
        url: "/MasterSanitationSchedule/AddMsAttachments",
        data: data,
        processData: false,
        contentType: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                var msid = data.sanitId;
                SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
                swal(SuccessAlertSetting, function () {
                    RedirectToMSDetail(msid, "SOAttachments");
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
$(document).on('click', '.lnk_download_attachment', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = msAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/MasterSanitationSchedule/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });

});
$(document).on('click', ".delMSAttachment", function () {
    var data = msAttachmentTable.row($(this).parents('tr')).data();
    var FileAttachmentId = data.FileAttachmentId;
    DeleteMsAttachment(FileAttachmentId);
});
function DeleteMsAttachment(fileAttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/MasterSanitationSchedule/DeleteMsAttachments',
            data: {
                _fileAttachmentId: fileAttachmentId
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "POST",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    msAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                generateMsAttachmentsGrid();
                CloseLoader();
            }
        });
    });
}
//#endregion

//#region Tools 
$(document).on('click', '#brdmsTools', function () {
    var msid = $(this).attr('data-val');
    RedirectToMSDetail(msid);
});
var msToolsTable;
function generateMsToolsGrid() {
    var sanitationMasterId = $(document).find('#MasterSanitModel_SanitationMasterId').val();
    var rCount = 0;
    var visibility;
    var toolSecurity;
    if ($(document).find('#tblmsTools').hasClass('dataTable')) {
        msToolsTable.destroy();
    }
    msToolsTable = $("#tblmsTools").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[0, "asc"]],
        stateSave: false,
        dom: 'rtlip',
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        "filter": true,
        "orderMulti": true,
        "ajax": {
            "url": "/MasterSanitationSchedule/PopulateMastSanitTools",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.sanitationMasterId = sanitationMasterId;
            },
            "dataSrc": function (response) {
                toolSecurity = response.toolSecurity;
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [4], render: function (a, b, data, d) {
                    if (toolSecurity) {
                        return '<a class="btn btn-outline-primary addToolBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editToolBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delToolBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            },
        ],
        "columns":
        [
            { "data": "CategoryValue", "autoWidth": false, "bSearchable": true, "bSortable": true },
            {
                "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            {
                "data": "Instructions", "autoWidth": true, "bSearchable": true, "bSortable": true,
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-100'>" + data + "</div>";
                }
            },
            { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "bSortable": false, "className": "text-center" }
        ],
        initComplete: function () {
            if (rCount > 0 || toolSecurity == false) {
                $(document).find('#btnAddmsTool').hide();
            }
            else {
                $(document).find('#btnAddmsTool').show();
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.addToolBttn,#btnAddmsTool', function () {
    AddTool();
});
$(document).on('click', '.editToolBttn', function () {
    var sanitationMasterId = $(document).find('#MasterSanitModel_SanitationMasterId').val();
    var msDescription = $(document).find('#MasterSanitModel_Description').val();
    var data = msToolsTable.row($(this).parents('tr')).data();
    var SanitationPlanningId = data.SanitationPlanningId;
    $.ajax({
        url: "/MasterSanitationSchedule/AddorEditTool",
        type: "GET",
        dataType: 'html',
        data: { sanitationMasterId: sanitationMasterId, msDescription: msDescription, SanitationPlanningId: SanitationPlanningId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermasterschedule').html(data);
            CloseLoader();
        },
        complete: function () {
            SetMSSControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delToolBttn', function () {
    var data = msToolsTable.row($(this).parents('tr')).data();
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
                    msToolsTable.state.clear();
                    ShowDeleteAlert(getResourceValue("toolsDeleteSuccessAlert"));
                    generateMsToolsGrid();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
});
function AddTool() {
    var sanitationMasterId = $(document).find('#MasterSanitModel_SanitationMasterId').val();
    var msDescription = $(document).find('#MasterSanitModel_Description').val();
    $.ajax({
        url: "/MasterSanitationSchedule/AddorEditTool",
        type: "GET",
        dataType: 'html',
        data: { sanitationMasterId: sanitationMasterId, msDescription: msDescription },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermasterschedule').html(data);
            CloseLoader();
        },
        complete: function () {
            SetMSSControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#btnToolsCancel", function () {
    var msid = $(document).find('#MasterSanPlanningModel_SanitationMasterId').val();
    RedirectToMSDetailOnCancel(msid, "SOTools");
});
function MSScheduleToolAddEditOnSuccess(data) {
    CloseLoader();
    var SanitationMasterId = data.SanitationMasterId;
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("ToolAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("ToolUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToMSDetail(SanitationMasterId, "SOTools");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('change', '#MasterSanPlanningModel_CategoryValue', function () {
    var Description;
    var thisText = $("#MasterSanPlanningModel_CategoryValue option:selected").text();
    if (thisText) {
        var splittedText = thisText.split('|');
        if (splittedText.length > 1) {
            Description = splittedText[1].trim();
            $(document).find('#MasterSanPlanningModel_Description').val(Description);
        }
    }
});
//#endregion

//#region Chemicals
var msChemicalTable;
function generateMsChemicalGrid() {
    var sanitationMasterId = $(document).find('#MasterSanitModel_SanitationMasterId').val();
    var rCount = 0;
    var visibility;
    var chemSecurity;
    if ($(document).find('#tblSOChemicals').hasClass('dataTable')) {
        msChemicalTable.destroy();
    }
    msChemicalTable = $("#tblSOChemicals").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[0, "asc"]],
        stateSave: false,
        dom: 'rtlip',
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        "filter": true,
        "orderMulti": true,
        "ajax": {
            "url": "/MasterSanitationSchedule/PopulateMastSanitChemicals",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.sanitationMasterId = sanitationMasterId;
            },
            "dataSrc": function (response) {
                chemSecurity = response.chemSecurity;
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [4], render: function (a, b, data, d) {
                    if (chemSecurity) {
                        return '<a class="btn btn-outline-primary addChemicalBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editChemicalBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delChemicalBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return '';
                    }
                }
            },
        ],
        "columns":
        [
            { "data": "CategoryValue", "autoWidth": false, "bSearchable": true, "bSortable": true },
            {
                "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            {
                "data": "Instructions", "autoWidth": false, "bSearchable": true, "bSortable": true,
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            { "data": "Quantity", "autoWidth": false, "bSearchable": true, "bSortable": true },
            { "bSortable": false, "className": "text-center" }
        ],
        initComplete: function () {
            if (rCount > 0 || chemSecurity == false) {
                $("#btnAddChemicals").hide();
            } else {
                $("#btnAddChemicals").show();
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', ".editChemicalBttn", function () {
    var data = msChemicalTable.row($(this).parents('tr')).data();
    var masterSaniID = $(document).find('#MasterSanitModel_SanitationMasterId').val();
    var masterSaniDesc = $(document).find('#MasterSanitModel_Description').val();
    $.ajax({
        url: "/MasterSanitationSchedule/AddOrEditChemical",
        type: "GET",
        dataType: 'html',
        data: { sanitationMasterId: masterSaniID, msDescription: masterSaniDesc, SanitationPlanningId: data.SanitationPlanningId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermasterschedule').html(data);
        },
        complete: function () {
            SetMSSControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', ".addChemicalBttn,#btnAddChemicals", function () {
    var sanitationMasterId = $(document).find('#MasterSanitModel_SanitationMasterId').val();
    var msDescription = $(document).find('#MasterSanitModel_Description').val();
    $.ajax({
        url: "/MasterSanitationSchedule/AddOrEditChemical",
        type: "GET",
        dataType: 'html',
        data: { sanitationMasterId: sanitationMasterId, msDescription: msDescription },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermasterschedule').html(data);
            CloseLoader();
        },
        complete: function () {
            SetMSSControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#brdChemical", function () {
    var userInfoId = $(this).attr('data-val');
    msChemicalTable = undefined;
    RedirectToMSDetail(userInfoId, "SOChemicals");
});
$(document).on('click', "#btnChemicalscancel", function () {
    var msid = $(document).find('#MasterSanPlanningModel_SanitationMasterId').val();
    msChemicalTable = undefined;
    RedirectToMSDetailOnCancel(msid, "SOChemicals");
});
$(document).on('click', "#brdChemical", function () {
    var userInfoId = $(this).attr('data-val');
    msChemicalTable = undefined;
    RedirectToMSDetail(userInfoId, "SOChemicals");
});
$(document).on('click', "#btnChemicalscancel", function () {
    var msid = $(document).find('#MasterSanPlanningModel_SanitationMasterId').val();
    msChemicalTable = undefined;
    RedirectToMSDetailOnCancel(msid, "SOChemicals");
});
function ChemicalAddOnSuccess(data) {
    if (data.data == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("ChemicaladdAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("ChemicalUpdateAlert") ;
        }
        swal(SuccessAlertSetting, function () {
            RedirectToMSDetail(data.SanitationMasterId, "SOChemicals");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
$(document).on('click', '.delChemicalBttn', function () {
    var data = msChemicalTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        DeleteChemicals(data);
    });
});
function DeleteChemicals(data) {
    $.ajax({
        url: '/MasterSanitationSchedule/DeleteChemicals',
        data: {
            SanitationPlanningId: data.SanitationPlanningId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                msChemicalTable.state.clear();
                ShowDeleteAlert(getResourceValue("ChemicalDeleteAlert")); 
                generateMsChemicalGrid();
            }
        },
        complete: function () {
            CloseLoader();
        }
    });
}
//#endregion

//#region job generation
$(function () {
    $(".CalendarDiv").hide();
    $("#RadioButton").val("OnDemand");
    $('input[name="radio_1"]:radio').change(function () {
        var radioValue = $("input[name='radio_1']:checked").val();
        if (radioValue == "OnDemand") {
            $(".OnDemandDiv").show();
            $(".CalendarDiv").hide();
        } else {
            $(".OnDemandDiv").hide();
            $(".CalendarDiv").show();

            $("#sanitationJobGenerationModel_OnDemandGroup").val($("#sanitationJobGenerationModel_OnDemandGroup option:first").val()).change();
            $(document).find('#sanitationJobGenerationModel_ScheduledDate').val("");

            $(document).find('#sanitationJobGenerationModel_OnDemandGroup').removeClass('input-validation-error').addClass('valid');
            $(document).find('#sanitationJobGenerationModel_ScheduledDate').removeClass('input-validation-error').addClass('valid');
        }

        $("#RadioButton").val(radioValue);
    });

    $(document).on("change", "#sanitationJobGenerationModel_ScheduledDate", function () {
        var tlen = $(document).find("#sanitationJobGenerationModel_ScheduledDate").val();
        if (tlen.length > 0) {
            var areaddescribedby = $(document).find("#sanitationJobGenerationModel_ScheduledDate").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find("#sanitationJobGenerationModel_ScheduledDate").removeClass("input-validation-error");
        }
        else {
            var areaddescribedby = $(document).find("#sanitationJobGenerationModel_ScheduledDate").attr('aria-describedby');
            $('#' + areaddescribedby).show();
            $(document).find('form').find("#sanitationJobGenerationModel_ScheduledDate").addClass("input-validation-error");
        }
    });
    $(document).on("change", "#sanitationJobGenerationModel_OnDemandGroup", function () {
        var tlen = $(document).find("#sanitationJobGenerationModel_OnDemandGroup").val();
        if (tlen.length > 0) {
            var areaddescribedby = $(document).find("#sanitationJobGenerationModel_OnDemandGroup").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find("#sanitationJobGenerationModel_OnDemandGroup").removeClass("input-validation-error");
        }
        else {
            var areaddescribedby = $(document).find("#sanitationJobGenerationModel_OnDemandGroup").attr('aria-describedby');
            $('#' + areaddescribedby).show();
            $(document).find('form').find("#sanitationJobGenerationModel_OnDemandGroup").addClass("input-validation-error");
        }
    });
    $(document).find(".modal").on("hidden.bs.modal", function () {
        $(document).find('.ui-tooltip').hide();
    });

});

$('#ModalPopUp').on('click', function () {
    $.ajax({
        url: '/MasterSanitationSchedule/GetOnDemandList',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        beforeSend: function () {
            ShowLoader();
        },
        type: "GET",
        success: function (data) {
            var option = "";
            var statusList = data;
            if (statusList) {
                option += '<option value="">--Select--</option>';
                for (var i = 0; i < statusList.length; i++) {
                    option += '<option value="' + statusList[i].ListValue + '">' + statusList[i].ListValue + " | " + statusList[i].Description + '</option>';
                }
            }
            $(document).find('#sanitationJobGenerationModel_OnDemandGroup').empty().html(option);
            $(document).find('#sanitationJobGenerationModel_OnDemandGroup').val(data.ChangeSiteSiteId).trigger('change');
            ResetErrorDiv();
            $("#genJobModal").find("select").each(function () {
                $(this).val('').trigger('change');
                if ($(this).hasClass('input-validation-error')) {
                    $(this).removeClass('input-validation-error');
                }
            });
            $("#genJobModal").find('input[type = text]').each(function () {
                $(this).val('');
                if ($(this).hasClass('input-validation-error')) {
                    $(this).removeClass('input-validation-error');
                }
            });
            $(document).find('.ui-tooltip').hide();

            $('#genJobModal').modal('show');
        },
        complete: function () {
            $(document).find('.select2picker').select2({});
            $.validator.unobtrusive.parse(document);
            $('input, form').blur(function () {
                $(this).valid();
            });
            CloseLoader();
        }
    });
});
function ResetErrorDiv() {
    $(document).find('.errormessage').html('').hide();
}
function CloseWOModal() {
    var option = '<option value="">--Select--</option>';

    $(document).find('#sanitationJobGenerationModel_OnDemandGroup').val("").trigger('change');
    $(document).find('#sanitationJobGenerationModel_ScheduledDate').val("");
    $(document).find('#sanitationJobGenerationModel_IsPrint').prop('checked', false);
    $('#genJobModal').modal('hide');
}
function GenerationProcessOnSuccess(data) {
    CloseLoader();
    if (data.data == "success") {
        CloseWOModal();
        var TotalMsg = "Sanitation Job Generation Complete" + "<span style='color: #212529; display: block; font-weight: 400; text-decoration: underline;'>" + data.TopMessage + "</span>" + "Sanitation Masters :" + data.SanitationMasterCount + "</br>" + "Sanitation Jobs Created :" + data.SanitationJobCount;
        if (data.SanitationJobCount && data.SanitationJobCount > 0 && data.SanitationJobCount > 0) {
            SuccessAlertSetting.text = TotalMsg;
            swal({
                title: getResourceValue("SaveAlertSuccess"),
                text: TotalMsg,
                html: true,
                type: "success",
                showCancelButton: false,
                confirmButtonClass: "btn-sm btn-success",
                cancelButtonClass: "btn-sm",
                confirmButtonText: getResourceValue("SaveAlertOk"),
                cancelButtonText: getResourceValue("CancelAlertNo")
            }, function () {
                if (data.IsPrint && data.listOfSanitation.length > 0) {
                    $.ajax({
                        url: '/MasterSanitationSchedule/PrintSaniList',
                        data: {
                            listOfSanitation: data.listOfSanitation
                        },
                        type: "POST",
                        datatype: "json",
                        responseType: 'arraybuffer',
                        beforeSend: function () {
                            ShowLoader();
                        },
                        success: function (result) {
                            if (result.success) {
                                PdfPrintAllWoList(result.pdf);
                            }
                        },
                        complete: function () {
                            CloseLoader();
                        }
                    });
                }

            });
        }
        else {
            swal({
                title: getResourceValue("CommonErrorAlert"),
                text: TotalMsg,
                html: true,
                type: "error",
                showCancelButton: false,
                confirmButtonClass: "btn-sm btn-danger",
                cancelButtonClass: "btn-sm",
                confirmButtonText: getResourceValue("SaveAlertOk"),
                cancelButtonText: getResourceValue("CancelAlertNo")
            });
        }

    }
    else {
        ResetErrorDiv();
        ShowGenericErrorOnAddUpdate(data, '#genJobModal');
    }
}
function PdfPrintAllWoList(pdf) {
    var blob = b64StrtoBlob(pdf, 'application/pdf');
    var blobUrl = URL.createObjectURL(blob);
    window.open(blobUrl, "_blank");
}
function b64StrtoBlob(b64Data, contentType, sliceSize) {
    contentType = contentType || '';
    sliceSize = sliceSize || 512;
    var byteCharacters = atob(b64Data);
    var byteArrays = [];
    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        var slice = byteCharacters.slice(offset, offset + sliceSize);
        var byteNumbers = new Array(slice.length);
        for (var i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }
        var byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
    }
    var blob = new Blob(byteArrays, { type: contentType });
    return blob;
}


//#endregion

//#region Asset popup
$(document).on('click', "#openOJobAssetgrid", function () {
    generateAssetPopupTable();
});
//#endregion