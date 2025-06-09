//#region Task
function GenerateSaTaskGrid() {
    var rCount = 0;
    var showActionButtons = false;
    var sensorAlertProcedureId = $(document).find('#sensorAlertModel_SensorAlertProcedureId').val();
    if ($(document).find('#saTaskTable').hasClass('dataTable')) {
        dtTaskTable.destroy();
    }
    dtTaskTable = $("#saTaskTable").DataTable({
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
            "url": "/SensorAlert/GetSensorAlertTaskGrid",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.sensorAlertProcedureId = sensorAlertProcedureId
            },
            "dataSrc": function (response) {
                showActionButtons = response.showActionButtons;
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [2], render: function (a, b, data, d) {
                    if (showActionButtons) {
                        return '<a class="btn btn-outline-primary addtaskBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editTaskBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger deltaskBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return '';
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
                    return "<div class='text-wrap width-400'>" + data + "</div>";
                }
            },
            { "bSortable": false, "className": "text-center" }
        ],
        initComplete: function () {
            if (rCount > 0 || showActionButtons == false) {
                $('#btnAddTask').hide();
            }
            else {
                $('#btnAddTask').show();
            }
            if (showActionButtons == false) {
                var column = this.api().column(2);
                column.visible(false);
            }
            else {
                var column = this.api().column(2);
                column.visible(true);
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnAddTask,.addtaskBttn", function () {
    var sensorAlertProcedureId = $(document).find('#sensorAlertModel_SensorAlertProcedureId').val();
    var clientLookupId = $(document).find('#sensorAlertModel_ClientLookUpId').val();
    $.ajax({
        url: "/SensorAlert/AddOrEditSaTask",
        type: "GET",
        dataType: 'html',
        data: { sensorAlertProcedureId: sensorAlertProcedureId, clientLookUpId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderSensorAlert').html(data);
        },
        complete: function () {
            SetSaControl();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function TaskAddOnSuccess(data) {
    CloseLoader();
    var sensorAlertProcedureId = data.SensorAlertProcedureId;
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("TaskAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("TaskUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToSaDetail(sensorAlertProcedureId, "task");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnTaskcancel", function () {
    var saId = $(document).find('#sensorAlertTaskModel_SensorAlertProcedureId').val();
    RedirectToSureOncancel(saId, "task");
});
$(document).on('click', '.editTaskBttn', function () {
    var data = dtTaskTable.row($(this).parents('tr')).data();
    EditTask(data);
});
function EditTask(data) {
    var saId = $(document).find('#sensorAlertModel_SensorAlertProcedureId').val();
    var clientLookupId = $(document).find('#sensorAlertModel_ClientLookUpId').val();
    $.ajax({
        url: "/SensorAlert/AddOrEditSaTask",
        type: "GET",
        dataType: 'html',
        data: {
            taskId: data.TaskId,
            sensorAlertProcedureId: saId,
            sensorAlertProcedureTaskId: data.SensorAlertProcedureTaskId,
            description: data.Description,
            clientLookUpId: clientLookupId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderSensorAlert').html(data);
        },
        complete: function () {
            SetSaControl();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', '.deltaskBttn', function () {
    var data = dtTaskTable.row($(this).parents('tr')).data();
    var sensorAlertProcedureTaskId = data.SensorAlertProcedureTaskId;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SensorAlert/DeleteSaTask',
            data: {
                sensorAlertProcedureTaskId: sensorAlertProcedureTaskId
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
                GenerateSaTaskGrid();
            }
        });
    });
});
//#endregion Task