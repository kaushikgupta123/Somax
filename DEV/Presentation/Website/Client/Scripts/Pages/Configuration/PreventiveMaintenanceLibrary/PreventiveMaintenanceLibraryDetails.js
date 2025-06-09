//#region Tasks
var dtTaskTable;
function GenerateTaskGrid() {
    var RecordCount = 0;
    var IsActionAddBtnOpen = false;
    var IsActionDelBtnOpen = false;
    var IsActionEditBtnOpen = false;
    var pmlId = $('#preventiveMaintenanceLibraryModel_PrevMaintLibraryId').val();
    if ($(document).find('#tasksPMLTable').hasClass('dataTable')) {
        dtTaskTable.destroy();
    }
    dtTaskTable = $("#tasksPMLTable").DataTable({
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
            "url": "/PreventiveMaintenanceLibrary/PopulateTasks",
            "data": { PrevMaintLibraryId: pmlId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {   
                RecordCount = response.data.length;
                IsActionAddBtnOpen = response.isActionAddBtnShow;
                IsActionDelBtnOpen = response.isActionDelBtnShow;
                IsActionEditBtnOpen = response.IsActionEditBtnShow;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [2], render: function (a, b, data, d) {
                    if (IsActionEditBtnOpen) {
                        if (IsActionAddBtnOpen) {
                            if (IsActionDelBtnOpen) {
                                return '<a class="btn btn-outline-primary addtaskBttn gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                                    '<a class="btn btn-outline-success editTaskBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                                    '<a class="btn btn-outline-danger deltaskBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                            }
                            else {
                                return '<a class="btn btn-outline-primary addtaskBttn gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                                    '<a class="btn btn-outline-success editTaskBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                            }
                        }
                        else {
                            if (IsActionDelBtnOpen) {
                                return '<a class="btn btn-outline-success editTaskBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a> ' +
                                    '<a class="btn btn-outline-danger deltaskBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                            }
                            else {
                                return '<a class="btn btn-outline-success editTaskBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                            }
                        }
                    }
                    else {
                        if (IsActionAddBtnOpen) {
                            if (IsActionDelBtnOpen) {
                                return '<a class="btn btn-outline-primary addtaskBttn gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                                    '<a class="btn btn-outline-danger deltaskBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                            }
                            else {
                                return '<a class="btn btn-outline-primary addtaskBttn gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>';
                            }
                        }
                        else {
                            if (IsActionDelBtnOpen) {
                                return '<a class="btn btn-outline-danger deltaskBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                            }
                            else {
                                return "";
                            }
                        }
                    }
                }
            }
        ],
        "columns":
        [
            { "data": "TaskId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            { "bSortable": false, "className": "text-center" }
        ],
        initComplete: function () {       
            if (IsActionAddBtnOpen == false && IsActionEditBtnOpen == false && IsActionDelBtnOpen == false) {
                var column = this.api().column(2);
                column.visible(false);
            }  
            else {
                var column = this.api().column(2);
                column.visible(true);
            }
            if (RecordCount > 0 || IsActionAddBtnOpen==false ) {
                $(document).find('#btnAddTaskContainer').hide();
            }
            else {
                $(document).find('#btnAddTaskContainer').show();
            }
        }
    });
}
$(document).on('click', '.deltaskBttn', function () {
    var data = dtTaskTable.row($(this).parents('tr')).data();
    var pmlId = $('#preventiveMaintenanceLibraryModel_PrevMaintLibraryId').val();
    var PrevMaintLibraryTaskId = data.PrevMaintLibraryTaskId;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/PreventiveMaintenanceLibrary/DeleteTasks',
            data: {
                PrevMaintLibraryId: pmlId,
                PrevMaintLibraryTaskId: PrevMaintLibraryTaskId
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "post",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    CloseLoader();
                    ShowDeleteAlert(getResourceValue("taskDeleteSuccessAlert"));
                }
            },
            complete: function () {
                dtTaskTable.state.clear();
                GenerateTaskGrid();
            }
        });
    });
});
$(document).on('click', "#btnAddTask,.addtaskBttn", function () {
    var pmlId = $('#preventiveMaintenanceLibraryModel_PrevMaintLibraryId').val();
    var ClientLookUpId = $(document).find('#preventiveMaintenanceLibraryModel_ClientLookupId').val();
    $.ajax({
        url: "/PreventiveMaintenanceLibrary/AddTask",
        type: "GET",
        dataType: 'html',
        data: { PrevMaintLibraryId: pmlId, ClientLookUpId: ClientLookUpId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventiveLibrary').html(data);
        },
        complete: function () {
            SetPMLControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.editTaskBttn', function () {
    var data = dtTaskTable.row($(this).parents('tr')).data();
    EditTask(data);
});
$(document).on('click', "#btnTaskcancel", function () {
    var pmlId = $('#taskModel_PrevMaintLibraryId').val();
    RedirectToSureOncancel(pmlId, "Tasks");
});
function RedirectToSureOncancel(pmlId, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToPmlDetail(pmlId, mode);
    });
}
$(document).on('click', "#brdpmltask", function () {
    var pmlId = $(this).attr('data-val');
    RedirectToPmlDetail(pmlId,"");
});
function EditTask(data) {
    var pmlId = $('#preventiveMaintenanceLibraryModel_PrevMaintLibraryId').val();
    var ClientLookUpId = $(document).find('#preventiveMaintenanceLibraryModel_ClientLookupId').val();
    $.ajax({
        url: "/PreventiveMaintenanceLibrary/EditTask",
        type: "GET",
        dataType: 'html',
        data:
        {
            PrevMaintLibraryId: pmlId,
            ClientLookUpId: ClientLookUpId,
            PrevMaintLibraryTaskId: data.PrevMaintLibraryTaskId,
            TaskId: data.TaskId,
            Description: data.Description
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventiveLibrary').html(data);
        },
        complete: function () {
            SetPMLControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function TaskAddOnSuccess(data) {
    CloseLoader();
    var pmlId = data.PrevMaintLibraryId;
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("TaskAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("TaskUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmlDetail(pmlId, "Tasks");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

//#endregion

//#region Action
$(document).on('click', '#ChangePrevenMaintLibId', function (e) {
    var clientlookupid = $(document).find('#hiddenclientlookupid').val();
    $(document).find('#txtPreventiveId').val(clientlookupid).removeClass('input-validation-error');
    $('#changePreventiveIDModalDetailsPage').modal('show');
    $(this).blur();
    $.validator.unobtrusive.parse(document);
});

function PrevMaintLibOnSuccess(data) {
    $('#changePreventiveIDModalDetailsPage').modal('hide');
    var prevMasterLibID = $(document).find('#hiddenprevmaintLibid').val();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("UpdatePMAlerts");
        swal(SuccessAlertSetting, function () {            
            RedirectToPmlDetail(prevMasterLibID, "overview");
        });
    }
    else {
        GenericSweetAlertMethod(data);
    }
}
    //#endregion