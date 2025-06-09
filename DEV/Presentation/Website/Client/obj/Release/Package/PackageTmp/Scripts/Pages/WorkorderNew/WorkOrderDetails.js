//#region Tasks
var woTaskTable;
$(document).on('click', '.addTaskBttn', function () {
    var data = woTaskTable.row($(this).parents('tr')).data();
    AddWoTask();
});
$(document).on('click', '#btnwoAddtask', function () {
    var data = woTaskTable.row($(this).parents('tr')).data();
    AddWoTask();
});
$(document).on('click', '.editTaskBttn', function () {
    var data = woTaskTable.row($(this).parents('tr')).data();
    EditWoTask(data.WorkOrderTaskId);
});
$(document).on('click', '.delTaskBttn', function () {
    var data = woTaskTable.row($(this).parents('tr')).data();
    DeleteWoTask(data.WorkOrderTaskId);
});
$(document).on('click', "#brdwotask", function () {
    var workOrderID = $(this).attr('data-val');
    RedirectToPmDetail(workOrderID);
});
function generateWoTaskGrid() {
    var IsActionOpen = false;
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var rCount = 0;
    var visibility;
    $(document).find('#example-select-all-sensor').prop('checked', false);
    if ($(document).find('#woTaskTable').hasClass('dataTable')) {
        woTaskTable.destroy();
    }
    woTaskTable = $("#woTaskTable").DataTable({
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
        dom: 'Btlipr',
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        buttons: [],
        "filter": true,
        "orderMulti": true,
        "ajax": {
            "url": "/WorkOrder/PopulateTask?workOrderId=" + workOrderID,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                IsActionOpen = response.ActionSecurity;
                return response.data;
            }
        },
        columnDefs: [{
            "data": "WorkOrderTaskId",
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
            targets: [5], render: function (a, b, data, d) {

                if (IsActionOpen) {
                    return '<div class="text-wrap"><a class="btn btn-outline-primary addTaskBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
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
                { "data": "TaskNumber", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-150'>" + data + "</div>";
                    }
                },
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ChargeToClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "WorkOrderTaskId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            if (rCount > 0 || IsActionOpen == false) {
                $("#btnwoAddtask").hide();
                $("#btnwoCompletetask").show();
                $("#btnwoCanceltask").show();
                $("#btnwoReOpentask").show();
            }
            else {
                $("#btnwoAddtask").show();
                $("#btnwoCompletetask").hide();
                $("#btnwoCanceltask").hide();
                $("#btnwoReOpentask").hide();
            }
            if (IsActionOpen == false) {
                var column = this.api().column(0);
                column.visible(false);
                var column = this.api().column(5);
                column.visible(false);
                $(document).find('#tskBttns').hide();
            }
            else {
                var column = this.api().column(0);
                column.visible(true);
                var column = this.api().column(5);
                column.visible(true);
                $(document).find('#tskBttns').show();
            }
            SetPageLengthMenu();
            CloseLoader();
        }
    });
}
var TaskIdsToupdate = [];
var TaskIdsToupdateReOpen = [];
var equiToClientLookupIdbarcode = [];
$(document).on('change', '.isSelect', function () {
    var data = woTaskTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        var el = $('#example-select-all-sensor').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        var index = TaskIdsToupdate.indexOf(data.WorkOrderTaskId);
        TaskIdsToupdate.splice(index, 1);

        var indexOpen = TaskIdsToupdateReOpen.indexOf(data.WorkOrderTaskId + ',' + data.Status);
        TaskIdsToupdateReOpen.splice(indexOpen, 1);
    }
    else {
        TaskIdsToupdate.push(data.WorkOrderTaskId);
        TaskIdsToupdateReOpen.push(data.WorkOrderTaskId + ',' + data.Status);
    }
});
$(document).on('click', '#example-select-all-sensor', function () {
    var checked = this.checked;
    if (checked) {
        var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
        TaskIdsToupdate = [];
        TaskIdsToupdateReOpen = [];
        $.ajax({
            "url": "/WorkOrder/PopulateTaskIds?workOrderId=" + workOrderID,
            async: true,
            type: "GET",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data) {
                    $.each(data, function (index, item) {
                        TaskIdsToupdate.push(item.WorkOrderTaskId);
                        TaskIdsToupdateReOpen.push(item.WorkOrderTaskId + ',' + item.Status);
                    });
                }
                else {
                    TaskIdsToupdate = [];
                    TaskIdsToupdateReOpen = [];
                }
            },
            complete: function () {
                woTaskTable.column(0).nodes().to$().each(function (index, item) {
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
$(document).on('click', '#btnwoCompletetask', function () {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var taskIds = null;
    taskIds = TaskIdsToupdate.join();
    if (taskIds !== "") {
        $.ajax({
            url: '/WorkOrder/CompleteTask',
            data: { taskList: taskIds, workOrderId: workOrderID },
            type: "GET",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "success" && data.successcount > 0) {
                    var msg = data.successcount + getResourceValue("taskCompleteAlert");
                    ShowSuccessAlert(msg);
                    TaskIdsToupdateReOpen = [];
                    TaskIdsToupdate = [];
                    generateWoTaskGrid();
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
                    TaskIdsToupdateReOpen = [];
                    TaskIdsToupdate = [];
                }
                $(document).find('.isSelect').prop('checked', false);
                $(document).find('#example-select-all-sensor').prop('checked', false);
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
var taskIDscancel;
$(document).on('click', '#btnwoCanceltask', function () {
    $(document).find('#woTaskModel_CancelReason').val('').trigger('change');
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    taskIDscancel = null;
    taskIDscancel = TaskIdsToupdate.join();
    if (taskIDscancel !== "") {
        $.ajax({
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#cancelModal').modal('show');
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
        $(document).find('.isSelect').prop('checked', false);
        $(document).find('#example-select-all-sensor').prop('checked', false);
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
$(document).on('click', '#btnwoReOpentask', function () {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var taskIdsReopen = null;
    taskIdsReopen = TaskIdsToupdateReOpen.join();
    if (taskIdsReopen !== "") {
        $.ajax({
            url: '/WorkOrder/ReOpenTask',
            data: { taskList: taskIdsReopen, workOrderId: workOrderID },
            type: "GET",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "success") {
                    var msg = data.successcount + getResourceValue("taskReopenAlert");
                    ShowSuccessAlert(msg);
                    TaskIdsToupdateReOpen = [];
                    TaskIdsToupdate = [];
                    generateWoTaskGrid();

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
                TaskIdsToupdateReOpen = [];
                TaskIdsToupdate = [];
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
$(document).on('click', '#btnWoTaskCancelOk', function () {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var cancelreason = $(document).find('#woTaskModel_CancelReason').val();
    $.ajax({
        url: '/WorkOrder/CancelTask',
        data: { taskList: taskIDscancel, cancelReason: cancelreason },
        type: "GET",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#cancelModal').modal('hide');
            if (data.data == "success") {
                var msg = data.successcount + getResourceValue("taskCancelAlert");
                ShowSuccessAlert(msg);
                TaskIdsToupdateReOpen = [];
                TaskIdsToupdate = [];
                generateWoTaskGrid();
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
            TaskIdsToupdateReOpen = [];
            TaskIdsToupdate = [];
        },
        complete: function () {
            CloseLoader();
            $('#cancelModal').modal('hide');
        },
        error: function () {
            CloseLoader();
        }
    });

});
$(document).on('click', '#btnWoTaskCancelCancel,.cancel-modal-close', function () {
    $('#cancelModal').modal('hide');
    $(document).find('.isSelect').prop('checked', false);
    $(document).find('#example-select-all-sensor').prop('checked', false);
});
function AddWoTask() {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $('#workOrderModel_Status').val();
    var description = $('#workOrderModel_Description').val();
    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $(document).find('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $(document).find('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $(document).find('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    var projectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    //v2-463
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    //v2-847
    var AssetGroup1Name = $(document).find('#workOrderSummaryModel_AssetGroup1Name').val();
    var AssetGroup2Name = $(document).find('#workOrderSummaryModel_AssetGroup2Name').val();
    var AssetGroup1ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup1ClientlookupId').val();
    var AssetGroup2ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup2ClientlookupId').val();
    //
    $.ajax({
        url: "/WorkOrder/AddTasks",
        type: "GET",
        dataType: 'html',
        data: { workOrderId: workOrderID, ClientLookupId: clientLookupId, Status: status, Description: description, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: projectClientLookupId, AssetLocation: assetLocation, AssetGroup1Name: AssetGroup1Name, AssetGroup2Name: AssetGroup2Name, AssetGroup1ClientlookupId: AssetGroup1ClientlookupId, AssetGroup2ClientlookupId: AssetGroup2ClientlookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));

        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function EditWoTask(taskid) {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $('#workOrderModel_Status').val();
    var description = $('#workOrderModel_Description').val();
    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $(document).find('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $(document).find('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $(document).find('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    //v2-463
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var projectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    //v2-847
    var AssetGroup1Name = $(document).find('#workOrderSummaryModel_AssetGroup1Name').val();
    var AssetGroup2Name = $(document).find('#workOrderSummaryModel_AssetGroup2Name').val();
    var AssetGroup1ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup1ClientlookupId').val();
    var AssetGroup2ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup2ClientlookupId').val();
    //
    $.ajax({
        url: "/WorkOrder/EditTasks",
        type: "GET",
        dataType: 'html',
        data: { workOrderId: workOrderID, _taskId: taskid, ClientLookupId: clientLookupId, Status: status, Description: description, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: projectClientLookupId, AssetLocation: assetLocation, AssetGroup1Name: AssetGroup1Name, AssetGroup2Name: AssetGroup2Name, AssetGroup1ClientlookupId: AssetGroup1ClientlookupId, AssetGroup2ClientlookupId: AssetGroup2ClientlookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));
        },
        complete: function () {
            SetControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function WoTaskAddOnSuccess(data) {
    CloseLoader();
    var workOrderId = data.workOrderId;
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("TaskAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("TaskUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(workOrderId, "tasks")
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function DeleteWoTask(taskId) {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/WorkOrder/DeleteTasks',
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
                    ShowDeleteAlert(getResourceValue("taskDeleteSuccessAlert"));
                    woTaskTable.state.clear();
                }
            },
            complete: function () {
                generateWoTaskGrid();
            }
        });
    });
}
$(document).on('click', "#btnwotaskcancel", function () {
    var workOrderID = $(document).find('#woTaskModel_WorkOrderId').val();
    woTaskTable = undefined;
    RedirectToDetailOncancel(workOrderID, "tasks");
});
$(document).on('click', "#openwotaskchargetogrid", function () {
    var textChargeToId = $("#woTaskModel_ChargeType option:selected").val();
    if (textChargeToId == "Equipment") { generateEquipmentDataTableWO(); }
    else if (textChargeToId == "Location") { generateLocationDataTable(); }
});
$(document).on("change", "#woTaskModel_ChargeType", function () {
    $(document).find('#txtChargeTo').val('');
    var textChargeToId = $("#woTaskModel_ChargeType option:selected").val();
});
var woAssignTable;
$(document).on('click', '.addAssignmentBttn', function () {
    AddAssignment();
});
$(document).on('click', '#btnAddwoAssignment', function () {
    var data = woAssignTable.row($(this).parents('tr')).data();
    AddAssignment();
});
$(document).on('click', '.editAssignmentBttn', function () {
    var data = woAssignTable.row($(this).parents('tr')).data();
    EditAssignment(data.WorkOrderSchedId, data.AssignedTo_PersonnelClientLookupId);
});
$(document).on('click', '.delAssignmentBttn', function () {
    var data = woAssignTable.row($(this).parents('tr')).data();
    DeleteAssignment(data.WorkOrderSchedId);
});
function generateWoAssignmentGrid() {
    var IsActionDelBtnOpen = false;
    var IsActionEditBtnOpen = false;
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var workOrderStatus = $(document).find('#workOrderModel_Status').val();
    var rCount = 0;
    if ($(document).find('#woAssignmentTable').hasClass('dataTable')) {
        woAssignTable.destroy();
    }
    woAssignTable = $("#woAssignmentTable").DataTable({
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
            "url": "/WorkOrder/PopulateAssignment?workOrderId",
            "type": "POST",
            data: { workOrderId: workOrderID, workOrderStatus: workOrderStatus },
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                IsActionDelBtnOpen = response.IsActionDelBtnShow;
                IsActionEditBtnOpen = response.IsActionEditBtnShow;
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    if (IsActionEditBtnOpen == true) {
                        return '<a class="btn btn-outline-primary addAssignmentBttn gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editAssignmentBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delAssignmentBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return '';
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "AssignedTo_PersonnelClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "WorkAssigned_Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "ScheduledStartDate",
                    "type": "date "
                },
                {
                    "data": "ScheduledHours", defaultContent: "", "bSearchable": true, "bSortable": true
                },
                {
                    "data": "WorkOrderSchedId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            if (IsActionEditBtnOpen == false) {
                var column = this.api().column(4);
                column.visible(false);
            }
            else {
                var column = this.api().column(4);
                column.visible = true;
            }
            if (rCount > 0 || IsActionEditBtnOpen == false) {
                $("#btnAddwoAssignment").hide();
            }
            else {
                $("#btnAddwoAssignment").show();
            }
            SetPageLengthMenu();
        }
    });
}
function AddAssignment() {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $('#workOrderModel_Status').val();
    var description = $('#workOrderModel_Description').val();
    var projectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    $.ajax({
        url: "/WorkOrder/AddAssignment",
        type: "GET",
        dataType: 'html',
        data: { WorkOrderID: workOrderID, ClientLookupId: clientLookupId, Status: status, Description: description, ProjectClientLookupId: projectClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function WoAssignmentAddOnSuccess(data) {
    CloseLoader();
    var workOrderId = data.workOrderId;
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AssignmentAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("AssignmentUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(workOrderId, "assignments");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditAssignment(woScheduleId, AssignedTo_PersonnelClientLookupId) {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $('#workOrderModel_Status').val();
    var description = $('#workOrderModel_Description').val();
    var completedate = $(document).find("#spnwocompletedate").text();
    var projectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    $.ajax({
        url: "/WorkOrder/EditAssignment",
        type: "GET",
        dataType: 'html',
        data: { WorkOrderID: workOrderID, _assignmentId: woScheduleId, ClientLookupId: clientLookupId, AssignedTo_PersonnelClientLookupId: AssignedTo_PersonnelClientLookupId, Status: status, Description: description, ProjectClientLookupId: projectClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
        },
        complete: function () {
            SetControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function DeleteAssignment(woScheduleId) {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/WorkOrder/DeleteAssignment',
            data: {
                WorkOrderID: workOrderID, _assignmentId: woScheduleId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("assignmentDeleteSuccessAlert"));
                    woAssignTable.state.clear();
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                generateWoAssignmentGrid();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#brdwoassignment", function () {
    var workorderId = $(document).find('#woAssignmentModel_WorkOrderId').val();
    RedirectToPmDetail(workorderId);

});
$(document).on('click', "#btnwoassignmentcancel", function () {
    var woID = $(document).find('#woAssignmentModel_WorkOrderId').val();
    woAssignTable = undefined;
    RedirectToDetailOncancel(woID, "assignments");
});

//#endregion
//#region Attachments
var woAttachmentTable;
$(document).on('click', "#brdwoattachment", function () {
    var woID = $(this).attr('data-val');
    RedirectToPmDetail(woID);
});
function generateWoAttachmentsGrid() {
    var attchCount = 0;
    if ($(document).find('#woAttachmentTable').hasClass('dataTable')) {
        woAttachmentTable.destroy();
    }
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    woAttachmentTable = $("#woAttachmentTable").DataTable({
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
            "url": "/WorkOrder/PopulateAttachment?workOrderId=" + workOrderID,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                attchCount = response.recordsTotal;
                if (attchCount > 0) {
                    $(document).find('#woAttachmentCount').show();
                    $(document).find('#woAttachmentCount').html(attchCount);
                }
                else {
                    $(document).find('#woAttachmentCount').hide();
                }
                return response.data;
            }
        },

        columnDefs: [
            {
                targets: [6], render: function (a, b, data, d) {
                    {
                        return '<a class="btn btn-outline-success editAttchBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delAttchBttn gridinnerbutton" title = "Delete" > <i class="fa fa-trash"></i></a> ';
                    }

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
                        return '<a class=lnk_sensor_1 href="javascript:void(0)"  target="_blank">' + row.FullName + '</a>';
                    }
                },
                { "data": "FileSizeWithUnit", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "CreateDate",
                    "type": "date "
                },
                /*V2-949*/
                {
                    "data": "PrintwithForm", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center",
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
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.lnk_sensor_1', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = woAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/WorkOrder/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });
});
$(document).on('click', "#btnAddWoAttachment", function () {
    var woID = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $("#workOrderModel_Status").val();
    var description = $('#workOrderModel_Description').val();
    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    //v2-463
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var ProjectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    //v2-847
    var AssetGroup1Name = $(document).find('#workOrderSummaryModel_AssetGroup1Name').val();
    var AssetGroup2Name = $(document).find('#workOrderSummaryModel_AssetGroup2Name').val();
    var AssetGroup1ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup1ClientlookupId').val();
    var AssetGroup2ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup2ClientlookupId').val();
    //
    $.ajax({
        url: "/WorkOrder/AddAttachments",
        type: "GET",
        dataType: 'html',
        data: { workOrderID: woID, ClientLookupId: clientLookupId, Status: status, Description: description, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: ProjectClientLookupId, AssetLocation: assetLocation, AssetGroup1Name: AssetGroup1Name, AssetGroup2Name: AssetGroup2Name, AssetGroup1ClientlookupId: AssetGroup1ClientlookupId, AssetGroup2ClientlookupId: AssetGroup2ClientlookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));
        },
        complete: function () {
            SetControls();

        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('submit', "#frmwoattachmentadd", function (e) {
    e.preventDefault();
    var form = document.querySelector('#frmwoattachmentadd');
    if (!$('form').valid()) {
        return;
    }
    var data = new FormData(form);
    $.ajax({
        type: "POST",
        url: "/WorkOrder/AddAttachments",
        data: data,
        processData: false,
        contentType: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                var workOrderId = data.workOrderId;
                SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
                swal(SuccessAlertSetting, function () {
                    RedirectToPmDetail(workOrderId, "attachments")
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
$(document).on('click', '.delAttchBttn', function () {
    var data = woAttachmentTable.row($(this).parents('tr')).data();
    DeleteWoAttachment(data.FileAttachmentId);
});
function DeleteWoAttachment(fileAttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/WorkOrder/DeleteAttachments',
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
                    woAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                generateWoAttachmentsGrid();
                CloseLoader();
            }
        });
    });
}

//#region V2-949
$(document).on('click', '.editAttchBttn', function () {
    var data = woAttachmentTable.row($(this).parents('tr')).data();
    EditWOAttachment(data.FileAttachmentId);
});
function EditWOAttachment(FileAttachmentId) {
    var WorkOrderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $("#workOrderModel_Status").val();
    var description = $('#workOrderModel_Description').val();
    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var ProjectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    var AssetGroup1Name = $(document).find('#workOrderSummaryModel_AssetGroup1Name').val();
    var AssetGroup2Name = $(document).find('#workOrderSummaryModel_AssetGroup2Name').val();
    var AssetGroup1ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup1ClientlookupId').val();
    var AssetGroup2ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup2ClientlookupId').val();

    $.ajax({
        url: "/WorkOrder/EditAttachment",
        type: "GET",
        dataType: 'html',
        data: { fileAttachmentId: FileAttachmentId, workOrderID: WorkOrderId, ClientLookupId: clientLookupId, Status: status, Description: description, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: ProjectClientLookupId, AssetLocation: assetLocation, AssetGroup1Name: AssetGroup1Name, AssetGroup2Name: AssetGroup2Name, AssetGroup1ClientlookupId: AssetGroup1ClientlookupId, AssetGroup2ClientlookupId: AssetGroup2ClientlookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));
        },
        complete: function () {
            SetControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
//#endregion

$(document).on('click', "#btnwoattachmentcancel", function () {
    var woID = $(document).find('#attachmentModel_WorkOrderId').val();
    woAttachmentTable = undefined;
    RedirectToDetailOncancel(woID, "attachments");
});
var dtEstimatesPartTable;
$(document).on('click', "#btnWospartcancel", function () {
    var workorderId = $(document).find('#estimatePart_WorkOrderId').val();
    dtEstimatesPartTable = undefined;
    RedirectToDetailOncancel(workorderId, "estimatespart");
});
$(document).on('click', "#brdWoestimateparts", function () {
    var workorderId = $(document).find('#estimatePart_WorkOrderId').val();
    RedirectToPmDetail(workorderId);

});
$(document).on('click', '.editWoEstimatesPartBttn', function () {
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var data = dtEstimatesPartTable.row($(this).parents('tr')).data();
    var EstimatedCostsId = data.EstimatedCostsId;
    var CategoryId = data.CategoryId;
    if (CategoryId > 0) {
        $.ajax({
            url: "/WorkOrder/EditPartInInventory",
            type: "GET",
            dataType: 'html',
            data: { EstimatedCostsId: EstimatedCostsId, WorkOrderId: workorderId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#AddEstimatedParts').hide();
                $('#PartNotInInventoryPopUp').html(data);
                $('#AddPartNotInInventoryModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
            },
            complete: function () {
                SetControls();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
    else {
        var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
        var mainClientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
        var status = $("#workOrderModel_Status").val();
        var description = $('#workOrderModel_Description').val();
        var data = dtEstimatesPartTable.row($(this).parents('tr')).data();
        var PartclientLookupId = data.ClientLookupId;
        var EstimatedCostsId = data.EstimatedCostsId;
        var Description = data.Description;
        var UnitCost = data.UnitCost;
        var Unit = data.Unit;
        var AccountId = data.AccountId;
        var AccountClientLookupId = data.AccountClientLookupId;
        var VendorId = data.VendorId;
        var VendorClientLookupId = data.VendorClientLookupId;
        var PartCategoryMasterId = data.PartCategoryMasterId;
        var PartCategoryClientLookupId = data.PartCategoryClientLookupId;
        var Quantity = data.Quantity;
        var TotalCost = data.TotalCost;

        var type = $(document).find('#spnwotype').text();
        var priority = $(document).find("#spnwopriority").text();
        var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
        var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
        var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
        var completedate = $(document).find("#spnwocompletedate").text();
        var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
        var assignedFullName = $('#workOrderSummaryModel_AssignedFullName').val();
        var assigned = $('#workOrderSummaryModel_Assigned').val();
        var workAssigned_PersonnelId = $('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
        //v2-463
        var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
        var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
        var ProjectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
        var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
        $.ajax({
            url: "/WorkOrder/EditEstimatesPart",
            type: "GET",
            dataType: 'html',
            data: { WorkOrderId: workorderId, MainClientLookupId: mainClientLookupId, PartclientLookupId: PartclientLookupId, EstimatedCostsId: EstimatedCostsId, Description: Description, Status: status, UnitCost: UnitCost, Quantity: Quantity, Unit: Unit, AccountClientLookupId: AccountClientLookupId, AccountId: AccountId, VendorClientLookupId: VendorClientLookupId, VendorId: VendorId, PartCategoryClientLookupId: PartCategoryClientLookupId, PartCategoryMasterId: PartCategoryMasterId, TotalCost: TotalCost, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: ProjectClientLookupId, AssetLocation: assetLocation, CategoryId: CategoryId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#AddEstimatedParts').hide();
                $('#PartNotInInventoryPopUp').html(data);
                $('#AddPartNotInInventoryModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
            },
            complete: function () {
                SetControls();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
});
$(document).on('click', '.delWoEstimatesPartBttn', function () {
    var data = dtEstimatesPartTable.row($(this).parents('tr')).data();
    var EstimatedCostsId = data.EstimatedCostsId;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/WorkOrder/DeleteEstimatesPart',
            data: {
                EstimatedCostsId: EstimatedCostsId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("MaterialRequestDeletedAlert"));
                    dtEstimatesPartTable.state.clear();
                }
            },
            complete: function () {
                generateWoEstimatesPartGrid();
                CloseLoader();
            }
        });
    });
});
function AddEstimatePart() {
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $('#workOrderModel_Status').val();
    var description = $('#workOrderModel_Description').val();

    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $(document).find('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $(document).find('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $(document).find('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    //v2-463
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var ProjectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    $.ajax({
        url: "/WorkOrder/AddEstimatesPart",
        type: "GET",
        dataType: 'html',
        data: { WorkOrderId: workorderId, ClientLookupId: clientLookupId, Status: status, Description: description, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: ProjectClientLookupId, AssetLocation: assetLocation },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
var dtEstimatesPartTable;
var EstimatedCostsIdsToupdate = [];
function generateWoEstimatesPartGrid() {
    var rCount = 0;
    var initiated = false;
    var visibility = prventivemaintancesecurity_Estimate;
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var EstimatedCostsId = $(document).find('#workOrderModel_EstimatedCostsId').val();
    if ($(document).find('#tblWoEstimatesPart').hasClass('dataTable')) {
        dtEstimatesPartTable.destroy();
    }
    $(document).find('#EstimatedCostsGenerationSearch-select-all').prop('checked', false);
    dtEstimatesPartTable = $("#tblWoEstimatesPart").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        pagingType: "full_numbers",
        bProcessing: true,
        bDeferRender: true,
        order: [[1, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        orderMulti: true,
        ajax: {
            url: "/WorkOrder/PopulateEstimatePart?workOrderId=" + workorderId,
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.EstimatedCostsId = EstimatedCostsId;
            },
            dataSrc: function (response) {
                rCount = response.data.length;
                initiated = response.IsInitiated;
                return response.data;
            }
        },
        columnDefs: [
            {
                data: "EstimatedCostsId",
                orderable: false,
                className: 'select-checkbox dt-body-center',
                targets: 0,
                render: function (data, type, full, meta) {
                    if ($('#EstimatedCostsGenerationSearch-select-all').is(':checked')) {
                        return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="partisSelect" value="' + $('<div/>').text(data).html() + '">';
                    } else {
                        if (EstimatedCostsIdsToupdate.indexOf(data) != -1) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="partisSelect ' + data + '" value="' + $('<div/>').text(data).html() + '">';
                        } else {
                            return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="partisSelect" value="' + $('<div/>').text(data).html() + '">';
                        }
                    }
                }
            },
            {
                data: null,
                targets: [8],
                render: function (a, b, data, d) {
                    if (visibility == "True") {
                        var ismaterialreq = $("#ApprovalRouteModel_IsMaterialRequest").val();
                        if ($("#workOrderModel_Status").val() != "Complete" && $("#workOrderModel_Status").val() != "Denied") {
                            if (data.Status == "Initiated" && ismaterialreq == "True") {
                                if (data.PurchaseRequestId != 0) {
                                    return '<div class="text-wrap"><a data-toggle="modal" data-target="#AddEstimatedParts" class="btn btn-outline-primary addWoEstimatesPartBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>'
                                } else {
                                    return '<div class="text-wrap"><a data-toggle="modal" data-target="#AddEstimatedParts" class="btn btn-outline-primary addWoEstimatesPartBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                                        '<a class="btn btn-outline-success editWoEstimatesPartBttn gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>' +
                                        '<a class="btn btn-outline-danger delWoEstimatesPartBttn gridinnerbutton" title="Delete"><i class="fa fa-trash"></i></a>';
                                }
                            }
                            else if (data.Status == "Approved" && ismaterialreq == "False") {
                                if (data.PurchaseRequestId != 0) {
                                    return '<div class="text-wrap"><a data-toggle="modal" data-target="#AddEstimatedParts" class="btn btn-outline-primary addWoEstimatesPartBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>';
                                }
                                else {
                                    return '<div class="text-wrap"><a data-toggle="modal" data-target="#AddEstimatedParts" class="btn btn-outline-primary addWoEstimatesPartBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                                        '<a class="btn btn-outline-success editWoEstimatesPartBttn gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>' +
                                        '<a class="btn btn-outline-danger delWoEstimatesPartBttn gridinnerbutton" title="Delete"><i class="fa fa-trash"></i></a>';
                                }
                            }
                        }
                    } else {
                        return "";
                    }
                }
            }
        ],
        columns: [
            {},
            { data: "ClientLookupId", autoWidth: true, bSearchable: true, bSortable: true },
            {
                data: "Description",
                autoWidth: true,
                bSearchable: true,
                bSortable: true,
                mRender: function (data, type, row) {
                    return "<div class='text-wrap width-300'>" + data + "</div>";
                }
            },
            { data: "UnitCost", autoWidth: true, bSearchable: true, bSortable: true, className: "text-right" },
            { data: "Quantity", autoWidth: true, bSearchable: true, bSortable: true, className: "text-right" },
            { data: "Unit", autoWidth: true, bSearchable: true, bSortable: true, className: "text-right" },
            { data: "TotalCost", autoWidth: true, bSearchable: true, bSortable: true, className: "text-right" },
            { data: "Status", autoWidth: true, bSearchable: true, bSortable: true, className: "text-left" },
            { data: "ObjectId", defaultContent: "", bSearchable: false, bSortable: false, className: "text-center" }
        ],
        initComplete: function () {
            if (visibility == "False") {
                this.api().column(8).visible(false);
            } else {
                this.api().column(8).visible(true);
            }
            if (rCount > 0 || visibility == "False") {
                $("#btnWoAddEstimatedPart").hide();
                if (initiated) {
                    $("#sendmaterialrequestitemsforapproval").show();
                } else {
                    $("#sendmaterialrequestitemsforapproval").hide();
                    $("#btnWoAddEstimatedPart").show();
                }
            } else {
                if ($("#workOrderModel_Status").val() != "Complete" && $("#workOrderModel_Status").val() != "Denied") {
                    $("#btnWoAddEstimatedPart").show();
                } else {
                    $("#btnWoAddEstimatedPart").hide();
                }
            }
            SetPageLengthMenu();
        }
    });
    $('#EstimatedCostsGenerationSearch-select-all').on('click', function () {
        $('.partisSelect').prop('checked', this.checked);
    });
}
function EstimapePartAddOnSuccess(data) {
    var workOrderId = data.workorderid;
    if (data.Result == "success") {
        $(document).find('#AddPartNotInInventoryModalpopup').modal("hide");
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("MaterialRequestAddedAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("MaterialRequestUpdatedAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(workOrderId, "estimatespart")
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region Estimating - Purchased
var dtEstimatesPurchaseTable;
var purchasegridfilteritemcount = 0;
$(document).on('click', '#btnmainsearch', function () {
    PGridClearAdvanceSearch();
    generateWoEstimatePurchaseGrid();
    var searchText = LRTrim($('#txtsearchbox').val());
});
$(document).on('click', '#btnPurchaseDataAdvSrch', function () {
    dtEstimatesPurchaseTable.state.clear();
    var searchitemhtml = "";
    purchasegridfilteritemcount = 0
    $('#txtsearchbox').val('');
    $('#purchasegridadvsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val() && $(this).val() != "0") {
            purchasegridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times pbtnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#purchaseadvsearchfilteritems').html(searchitemhtml);
    $(document).find('#purchasetadvsearchcontainer').find(".sidebar").removeClass('active');
    $('.overlay').fadeOut();
    PGridAdvanceSearch();
});
function PGridAdvanceSearch() {
    generateWoEstimatePurchaseGrid();
    $('.purchasefilteritemcount').text(purchasegridfilteritemcount);
}
function PGridClearAdvanceSearch() {
    var purchasegridfilteritemcount = 0;
    $('#purchasegridadvsearchsidebar').find('input:text').val('');
    $('.purchasefilteritemcount').text(purchasegridfilteritemcount);
    $('#purchaseadvsearchfilteritems').find('span').html('').removeClass('tagTo');
}
$(document).on('click', '#liClearAdvSearchFilterpurchase', function () {
    $(document).find("#txtsearchbox").val("");
    PGridClearAdvanceSearch();
    generateWoEstimatePurchaseGrid();
});
$(document).on('click', '.pbtnCross', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    purchasegridfilteritemcount--;
    PGridAdvanceSearch();
});
function generateWoEstimatePurchaseGrid() {
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var srcData = LRTrim($("#txtsearchbox").val());
    var clientLookupId = LRTrim($("#advsearchPurchaseOrder").val());
    var lineNo = LRTrim($("#advsearchLineNo").val());
    var description = LRTrim($("#advsearchDescription").val());
    var orderQty = LRTrim($("#advsearchOrderQty").val());
    var unitofMeasure = LRTrim($("#advsearchUOM").val());
    var status = LRTrim($("#advsearchStatus").val());
    var estimatedDelivery = LRTrim($("#advsearchEstimatedDelivery").val());
    var receivequantity = LRTrim($("#advsearchReceivedQuantity").val());
    if ($(document).find('#tblWOEstimatesPurchase').hasClass('dataTable')) {
        dtEstimatesPurchaseTable.destroy();
    }
    dtEstimatesPurchaseTable = $("#tblWOEstimatesPurchase").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/WorkOrder/PopulateEstimatePurchase",
            "type": "POST",
            data: function (d) {
                d.workorderId = workorderId;
                d.clientLookupId = clientLookupId;
                d.description = description;
                d.unitofMeasure = unitofMeasure;
                d.status = status;
                d.estimatedDelivery = estimatedDelivery;
                d.orderQty = orderQty;
                d.lineNo = lineNo;
                d.receivequantity = receivequantity;
                d.srcData = srcData;
            },
            "datatype": "json"
        },
        "columns":
            [
                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "LineNumber", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OrderQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "UnitOfMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "EstimatedDelivery", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "type": "date "
                },
                { "data": "ReceivedQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#openwopartidgrid , #openActualpartgrid", function () {
    generateWoPartsXrefDataTable();
});
//#endregion
//#region Estimating - Labor
var dtEstimatesLaborTable;
$(document).on('click', '#btnWoAddEstimatedLabor', function () {
    AddEstimateLabor();
});
$(document).on('click', "#btnslaborcancel", function () {
    dtEstimatesLaborTable = undefined;
    var workorderId = $(document).find('#estimateLabor_workOrderId').val();
    RedirectToDetailOncancel(workorderId, "estimateslabor");
});
$(document).on('click', "#brdWoestimatelabor", function () {
    var workorderId = $(document).find('#estimateLabor_workOrderId').val();
    RedirectToPmDetail(workorderId);

});
$(document).on('click', '.addWoEstimatesLaborBttn', function () {
    AddEstimateLabor();
});
$(document).on('click', '.editWoEstimatesLaborBttn', function () {
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $('#workOrderModel_Status').val();
    var description = $('#workOrderModel_Description').val();
    var data = dtEstimatesLaborTable.row($(this).parents('tr')).data();
    var EstimatedCostsId = data.EstimatedCostsId;
    var duration = data.Duration;
    var categoryId = data.CategoryId;
    var quantity = data.Quantity;

    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    //v2-463
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var ProjectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    //v2-847
    var AssetGroup1Name = $(document).find('#workOrderSummaryModel_AssetGroup1Name').val();
    var AssetGroup2Name = $(document).find('#workOrderSummaryModel_AssetGroup2Name').val();
    var AssetGroup1ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup1ClientlookupId').val();
    var AssetGroup2ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup2ClientlookupId').val();
    //
    $.ajax({
        url: "/WorkOrder/EditEstimatesLabor",
        type: "GET",
        dataType: 'html',
        data: {
            WorkOrderId: workorderId, ClientLookupId: clientLookupId, EstimatedCostsId: EstimatedCostsId, duration: duration, categoryId: categoryId, quantity: quantity, Status: status, Description: description, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: ProjectClientLookupId, AssetLocation: assetLocation, AssetGroup1Name: AssetGroup1Name, AssetGroup2Name: AssetGroup2Name, AssetGroup1ClientlookupId: AssetGroup1ClientlookupId, AssetGroup2ClientlookupId: AssetGroup2ClientlookupId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delWoEstimatesLaborBttn', function () {
    var data = dtEstimatesLaborTable.row($(this).parents('tr')).data();
    var EstimatedCostsId = data.EstimatedCostsId;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/WorkOrder/DeleteEstimatesLabor',
            data: {
                EstimatedCostsId: EstimatedCostsId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("laborDeleteSuccessAlert"));
                    dtEstimatesLaborTable.state.clear();
                }
            },
            complete: function () {
                generateWoEstimatesLaborGrid();
                CloseLoader();
            }
        });
    });
});
function AddEstimateLabor() {
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $('#workOrderModel_Status').val();
    var description = $('#workOrderModel_Description').val();
    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    //v2-463
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var ProjectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    //v2-847
    var AssetGroup1Name = $(document).find('#workOrderSummaryModel_AssetGroup1Name').val();
    var AssetGroup2Name = $(document).find('#workOrderSummaryModel_AssetGroup2Name').val();
    var AssetGroup1ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup1ClientlookupId').val();
    var AssetGroup2ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup2ClientlookupId').val();
    //
    $.ajax({
        url: "/WorkOrder/AddEstimatesLabor",
        type: "GET",
        dataType: 'html',
        data: { WorkOrderId: workorderId, ClientLookupId: clientLookupId, Status: status, Description: description, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: ProjectClientLookupId, AssetLocation: assetLocation, AssetGroup1Name: AssetGroup1Name, AssetGroup2Name: AssetGroup2Name, AssetGroup1ClientlookupId: AssetGroup1ClientlookupId, AssetGroup2ClientlookupId: AssetGroup2ClientlookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function generateWoEstimatesLaborGrid() {
    var rCount = 0;
    var visibility = prventivemaintancesecurity_Estimate;
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();

    if ($(document).find('#tblWOEstimatesLabor').hasClass('dataTable')) {
        dtEstimatesLaborTable.destroy();
    }
    dtEstimatesLaborTable = $("#tblWOEstimatesLabor").DataTable({
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
            "url": "/WorkOrder/PopulateEstimateLabor?workOrderId=" + workorderId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [6], render: function (a, b, data, d) {
                    if (visibility == "True") {
                        return '<div class="text-wrap"><a class="btn btn-outline-primary addWoEstimatesLaborBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editWoEstimatesLaborBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delWoEstimatesLaborBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "Duration", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            if (visibility == "False") {
                var column = this.api().column(5);
                column.visible(false);
            }
            else {
                var column = this.api().column(5);
                column.visible(true);
            }
            if (rCount > 0 || visibility == "False") {
                $("#btnWoAddEstimatedLabor").hide();
            }
            else {
                $("#btnWoAddEstimatedLabor").show();
            }
            SetPageLengthMenu();
        }
    });
}
function EstimateLaborAddOnSuccess(data) {
    var workOrderId = data.workorderid;
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("LaborAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("LaborUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(workOrderId, "estimateslabor")
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region Estimating - Other
var dtEstimatesOtherTable;
function generateWoEstimatesOtherGrid() {
    var rCount = 0;
    var visibility = prventivemaintancesecurity_Estimate;
    var IsVendorcolShow = true;
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    if ($(document).find('#tblWoEstimatesOther').hasClass('dataTable')) {
        dtEstimatesOtherTable.destroy();
    }
    dtEstimatesOtherTable = $("#tblWoEstimatesOther").DataTable({
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
            "url": "/WorkOrder/PopulateEstimateOther?workOrderId=" + workorderId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                IsVendorcolShow = response.isVendorcolShow;
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [6], render: function (a, b, data, d) {
                    if (visibility == "True") {
                        return '<div class="text-wrap"><a class="btn btn-outline-primary addWoEstimatesOthBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editWoEstimatesOthBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delWoEstimatesOthBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "Source", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            if (visibility == "False") {
                var column = this.api().column(5);
                column.visible(false);
            }
            else {
                var column = this.api().column(5);
                column.visible(true);
            }
            if (IsVendorcolShow) {
                var column = this.api().column(1);
                column.visible(false);
            }
            else {
                var column = this.api().column(1);
                column.visible(true);
            }

            if (rCount > 0) {
                $("#btnAddWoEstimatesOther").hide();
            }
            else {
                if (visibility == "True") {
                    $("#btnAddWoEstimatesOther").show();
                }
                else {
                    $("#btnAddWoEstimatesOther").hide();
                }
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '#btnAddWoEstimatesOther', function () {
    AddEstimateOther();
});
$(document).on('click', '.addWoEstimatesOthBttn', function () {
    AddEstimateOther();
});
$(document).on('click', '.editWoEstimatesOthBttn', function () {
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $(document).find('#workOrderModel_Status').val();
    var summarydescription = $('#workOrderModel_Description').val();
    var data = dtEstimatesOtherTable.row($(this).parents('tr')).data();
    var EstimatedCostsId = data.EstimatedCostsId;
    var description = data.Description;
    var source = data.Source;
    var unitCost = data.UnitCost;
    var quantity = data.Quantity;
    var totalCost = data.TotalCost;
    var updateIndex = data.UpdateIndex;
    var vendorId = data.VendorId;
    var vendorClientLookupId = data.VendorClientLookupId;

    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    //v2-463
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var projectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    //v2-847
    var AssetGroup1Name = $(document).find('#workOrderSummaryModel_AssetGroup1Name').val();
    var AssetGroup2Name = $(document).find('#workOrderSummaryModel_AssetGroup2Name').val();
    var AssetGroup1ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup1ClientlookupId').val();
    var AssetGroup2ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup2ClientlookupId').val();
    //
    $.ajax({
        url: "/WorkOrder/EditEstimatesOther",
        type: "GET",
        dataType: 'html',
        data: { WorkOrderId: workorderId, ClientLookupId: clientLookupId, EstimatedCostsId: EstimatedCostsId, description: description, source: source, unitCost: unitCost, quantity: quantity, totalCost: totalCost, updateIndex: updateIndex, vendorId: vendorId, VendorClientLookupId: vendorClientLookupId, Status: status, summarydescription: summarydescription, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: projectClientLookupId, AssetLocation: assetLocation, AssetGroup1Name: AssetGroup1Name, AssetGroup2Name: AssetGroup2Name, AssetGroup1ClientlookupId: AssetGroup1ClientlookupId, AssetGroup2ClientlookupId: AssetGroup2ClientlookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delWoEstimatesOthBttn', function () {
    var data = dtEstimatesOtherTable.row($(this).parents('tr')).data();
    var EstimatedCostsId = data.EstimatedCostsId;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/WorkOrder/DeleteEstimatesOther',
            data: {
                EstimatedCostsId: EstimatedCostsId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    dtEstimatesOtherTable.state.clear();
                    ShowDeleteAlert(getResourceValue("otherDeleteSuccessAlert"));
                }
            },
            complete: function () {
                generateWoEstimatesOtherGrid();
                CloseLoader();
            }
        });
    });
});
$(document).on('click', "#btnWOestothercancel", function () {
    dtEstimatesOtherTable = undefined;
    var workorderId = $(document).find('#estimateOtherModel_workOrderId').val();
    RedirectToDetailOncancel(workorderId, "estimatesother");
});
$(document).on('click', "#brdWoestimateother", function () {
    var workorderId = $(document).find('#estimateOtherModel_workOrderId').val();
    RedirectToPmDetail(workorderId);

});
function AddEstimateOther() {
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $('#workOrderModel_Status').val();
    var description = $('#workOrderModel_Description').val();
    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    //v2-463
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var ProjectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    //v2-847
    var AssetGroup1Name = $(document).find('#workOrderSummaryModel_AssetGroup1Name').val();
    var AssetGroup2Name = $(document).find('#workOrderSummaryModel_AssetGroup2Name').val();
    var AssetGroup1ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup1ClientlookupId').val();
    var AssetGroup2ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup2ClientlookupId').val();
    //
    $.ajax({
        url: "/WorkOrder/AddEstimatesOther",
        type: "GET",
        dataType: 'html',
        data: {
            WorkOrderId: workorderId, ClientLookupId: clientLookupId, Status: status, Description: description, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: ProjectClientLookupId, AssetLocation: assetLocation, AssetGroup1Name: AssetGroup1Name, AssetGroup2Name: AssetGroup2Name, AssetGroup1ClientlookupId: AssetGroup1ClientlookupId, AssetGroup2ClientlookupId: AssetGroup2ClientlookupId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function WoEstOtherAddOnSuccess(data) {
    var workOrderId = data.workorderid;
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("OtherEstimateAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("OtherEstimateUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(workOrderId, "estimatesother");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#wovopengrid', function () {
    generateWoVendorDataTable();
});
//#endregion

//#region Actual - Parts
var woActualPartsTable;
function generateWoActualPartsGrid() {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    if ($(document).find('#woActualPartTable').hasClass('dataTable')) {
        woActualPartsTable.destroy();
    }
    woActualPartsTable = $("#woActualPartTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/WorkOrder/PopulateActualPart?workOrderId=" + workOrderID,
            "type": "POST",
            "datatype": "json"
        },

        'rowCallback': function (row, data, dataIndex) {
            var found = SelectedActualPartId.some(function (el) {
                return el.PartHistoryId == data.PartHistoryId;
            });
            if (found) {
                $(row).find('input[type="checkbox"]').prop('checked', true);
            }
        },
        "columns":
            [
                {
                    "data": "PartHistoryId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    targets: 0,
                    'render': function (data, type, full, meta) {
                        return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="chksearchactualpart"  value="'
                            + $('<div/>').text(data).html() + '">';
                    }
                },
                { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-300'>" + data + "</div>";
                    }
                },
                { "data": "TransactionQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "Cost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "UnitofMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "TransactionDate",
                    "type": "date "

                },
                { "data": "PerformBy", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });


}
$(document).on('change', '#StoreroomId', function () {
    if ($('#StoreroomId').val() == '') {
        $(document).find('#AddActualPartIssueModal').find('#openActualpartgrid').hide();
        $(document).find('#AddActualPartIssueModal').find('#btnQrScannerActual').hide();
    }
    else {
        $(document).find('#AddActualPartIssueModal').find('#openActualpartgrid').show();
        $(document).find('#AddActualPartIssueModal').find('#btnQrScannerActual').show();
    }
    $(document).find('#txtpartid,#PartIssueAddModel_PartId').val('');
});
$(document).on('hidden.bs.modal', '#AddActualPartIssueModal', function () {
    $('#divAddParts').html('');
});
//#endregion
//#region Actual - Labor
var woActualLabourTable;
function generateWoActualLaborGrid() {
    var rCount = 0;
    var visibility = prventivemaintancesecurity_Estimate;
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var workOrderModel_Status = $(document).find('#workOrderModel_Status').val();
    if ($(document).find('#woActualLabourTable').hasClass('dataTable')) {
        woActualLabourTable.destroy();
    }
    woActualLabourTable = $("#woActualLabourTable").DataTable({
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
            "url": "/WorkOrder/PopulateActualLabor?workOrderId=" + workOrderID,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    if (visibility == "True" && workOrderModel_Status != "Canceled" && workOrderModel_Status != "Denied") {
                        return '<a class="btn btn-outline-primary addLaborBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editLaborBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delLaborBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return '';
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "PersonnelClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "NameFull", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "StartDate", "type": "date " },
                { "data": "Hours", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "TCValue", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "className": "text-center" }
            ],
        initComplete: function () {
            if (visibility == "False") {
                var column = this.api().column(4);
                column.visible(false);
            }
            else {
                var column = this.api().column(4);
                column.visible(true);
            }
            if (rCount > 0) {
                $("#AddLaborbtn").hide();
            }
            else {
                if (visibility == "True" && workOrderModel_Status != "Canceled" && workOrderModel_Status != "Denied") {
                    $("#AddLaborbtn").show();
                }
                else {
                    $("#AddLaborbtn").hide();
                }
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#AddLaborbtn , .addLaborBttn", function (e) {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $('#workOrderModel_Status').val();
    var description = $('#workOrderModel_Description').val();
    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    //v2-463
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var ProjectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    //v2-847
    var AssetGroup1Name = $(document).find('#workOrderSummaryModel_AssetGroup1Name').val();
    var AssetGroup2Name = $(document).find('#workOrderSummaryModel_AssetGroup2Name').val();
    var AssetGroup1ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup1ClientlookupId').val();
    var AssetGroup2ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup2ClientlookupId').val();
    //
    $.ajax({
        url: "/WorkOrder/AddLabor",
        type: "GET",
        dataType: 'html',
        data: { WorkOrderID: workOrderID, ClientLookupId: clientLookupId, Status: status, Description: description, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: ProjectClientLookupId, AssetLocation: assetLocation, AssetGroup1Name: AssetGroup1Name, AssetGroup2Name: AssetGroup2Name, AssetGroup1ClientlookupId: AssetGroup1ClientlookupId, AssetGroup2ClientlookupId: AssetGroup2ClientlookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));
        },
        complete: function () {
            SetControls();
            ActualLabordtpickerdate();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.editLaborBttn', function () {
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $('#workOrderModel_Status').val();
    var description = $('#workOrderModel_Description').val();

    var data = woActualLabourTable.row($(this).parents('tr')).data();
    var timeCardId = data.TimecardId;
    var personnelID = data.PersonnelID;
    var hours = data.Hours;
    var startDate = data.StartDate;
    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    //v2-463
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var ProjectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    //v2-847
    var AssetGroup1Name = $(document).find('#workOrderSummaryModel_AssetGroup1Name').val();
    var AssetGroup2Name = $(document).find('#workOrderSummaryModel_AssetGroup2Name').val();
    var AssetGroup1ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup1ClientlookupId').val();
    var AssetGroup2ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup2ClientlookupId').val();
    //
    $.ajax({
        url: "/WorkOrder/EditActualLabor",
        type: "GET",
        dataType: 'html',
        data: {
            WorkOrderId: workorderId, ClientLookupId: clientLookupId, timeCardId: timeCardId, personnelID: personnelID, hours: hours, startDate: startDate, Status: status, Description: description, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: ProjectClientLookupId, AssetLocation: assetLocation, AssetGroup1Name: AssetGroup1Name, AssetGroup2Name: AssetGroup2Name, AssetGroup1ClientlookupId: AssetGroup1ClientlookupId, AssetGroup2ClientlookupId: AssetGroup2ClientlookupId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));
        },
        complete: function () {
            SetControls();
            ActualLabordtpickerdate();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delLaborBttn', function () {
    var data = woActualLabourTable.row($(this).parents('tr')).data();
    var TimeCardId = data.TimecardId;

    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/WorkOrder/DeleteActualLabor',
            data: {
                TimeCardId: TimeCardId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    woActualLabourTable.state.clear();
                    ShowDeleteAlert(getResourceValue("laborDeleteSuccessAlert"));
                }
            },
            complete: function () {
                generateWoActualLaborGrid();
                CloseLoader();
            }
        });
    });
});
//V2-1010
function ActualLabordtpickerdate() {
    $(document).find('#woLaborModel_StartDate').datepicker({
        endDate: "today",
        maxDate: "today",
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true,
        onSelect: function (dateText) {
            ValidatePast30DaysDateFromDatePicker(this.value, getResourceValue("ValidatePast30DaysDateFromDatePickerAlert"));
            $(document).find('#woLaborModel_StartDate').removeClass('input-validation-error').css("display", "block");
        }
    }).inputmask('mm/dd/yyyy');
}
function ActualLaborAddOnSuccess(data) {
    var workOrderId = data.workorderid;
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("LaborAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("LaborUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(workOrderId, "actuallabor")
        });

    }
    else {
        ShowGenericErrorOnAddUpdate(data);

    }
}
$(document).on('click', "#btnActualLaborcancel", function () {
    woActualLabourTable = undefined;
    var workorderId = $(document).find('#woLaborModel_workOrderId').val();
    RedirectToDetailOncancel(workorderId, "actuallabor");
});
$(document).on('click', "#brdfollowupwo", function () {
    var workorderId = $(this).attr('data-val');
    RedirectToPmDetail(workorderId);

});
//#endregion
//#region Actual - Other
var woActualOtherTable;
$(document).on('click', '#btnAddWoActualOther', function () {
    AddActualOther();
});
$(document).on('click', '.addWoActualOtherBttn', function () {
    AddActualOther();
});
$(document).on('click', '.editWoActualOtherBttn', function () {
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $('#workOrderModel_Status').val();
    var summarydescription = $('#workOrderModel_Description').val();
    var data = woActualOtherTable.row($(this).parents('tr')).data();
    var otherCostsId = data.OtherCostsId;
    var description = data.Description;
    var source = data.Source;
    var unitCost = data.UnitCost;
    var quantity = data.Quantity;
    var totalCost = data.TotalCost;
    var vendorId = data.VendorId;
    var vendorName = data.VendorClientLookupId;
    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    //v2-463
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var ProjectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    //v2-847
    var AssetGroup1Name = $(document).find('#workOrderSummaryModel_AssetGroup1Name').val();
    var AssetGroup2Name = $(document).find('#workOrderSummaryModel_AssetGroup2Name').val();
    var AssetGroup1ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup1ClientlookupId').val();
    var AssetGroup2ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup2ClientlookupId').val();
    //
    $.ajax({
        url: "/WorkOrder/EditActualOther",
        type: "GET",
        dataType: 'html',
        data: { WorkOrderId: workorderId, ClientLookupId: clientLookupId, VendorName: vendorName, otherCostsId: otherCostsId, description: description, source: source, unitCost: unitCost, quantity: quantity, totalCost: totalCost, vendorId: vendorId, Status: status, summarydescription: summarydescription, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: ProjectClientLookupId, AssetLocation: assetLocation, AssetGroup1Name: AssetGroup1Name, AssetGroup2Name: AssetGroup2Name, AssetGroup1ClientlookupId: AssetGroup1ClientlookupId, AssetGroup2ClientlookupId: AssetGroup2ClientlookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#brdWoactualother", function () {
    var workorderId = $(document).find('#actualOther_workOrderId').val();
    RedirectToPmDetail(workorderId);

});
$(document).on('click', "#btnWOActualothercancel", function () {
    var workorderId = $(document).find('#actualOther_workOrderId').val();
    woActualOtherTable = undefined;
    RedirectToDetailOncancel(workorderId, "actualother");
});
$(document).on('click', '.delWoActualOtherBttn', function () {
    var data = woActualOtherTable.row($(this).parents('tr')).data();
    var OtherCostsId = data.OtherCostsId;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/WorkOrder/DeleteActualOther',
            data: {
                OtherCostsId: OtherCostsId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "success") {
                    woActualOtherTable.state.clear();
                    ShowDeleteAlert(getResourceValue("otherDeleteSuccessAlert"));
                }
            },
            complete: function () {
                generateWoActualOtherGrid();
                CloseLoader();
            }
        });
    });
});
function AddActualOther() {
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $('#workOrderModel_Status').val();
    var description = $('#workOrderModel_Description').val();
    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    //v2-463
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var ProjectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    //v2-847
    var AssetGroup1Name = $(document).find('#workOrderSummaryModel_AssetGroup1Name').val();
    var AssetGroup2Name = $(document).find('#workOrderSummaryModel_AssetGroup2Name').val();
    var AssetGroup1ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup1ClientlookupId').val();
    var AssetGroup2ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup2ClientlookupId').val();
    //
    $.ajax({
        url: "/WorkOrder/AddActualOther",
        type: "GET",
        dataType: 'html',
        data: { WorkOrderId: workorderId, ClientLookupId: clientLookupId, Status: status, Description: description, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: ProjectClientLookupId, AssetLocation: assetLocation, AssetGroup1Name: AssetGroup1Name, AssetGroup2Name: AssetGroup2Name, AssetGroup1ClientlookupId: AssetGroup1ClientlookupId, AssetGroup2ClientlookupId: AssetGroup2ClientlookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));
        },
        complete: function () {
            $('#actualOther_Source').val($('#actualOther_Source > option:eq(1)').val()).trigger('change');
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function generateWoActualOtherGrid() {
    var rCount = 0;
    var visibility = prventivemaintancesecurity_Estimate;
    var IsVendorcolShow = true;
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    if ($(document).find('#tblwoActualOther').hasClass('dataTable')) {
        woActualOtherTable.destroy();
    }
    woActualOtherTable = $("#tblwoActualOther").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/WorkOrder/PopulateActualOther?workOrderId=" + workOrderID,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                IsVendorcolShow = response.isVendorcolShow;
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [6], render: function (a, b, data, d) {
                    if (visibility == "True") {
                        return '<div class="text-wrap"><a class="btn btn-outline-primary addWoActualOtherBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editWoActualOtherBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delWoActualOtherBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "Source", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "className": "text-center" }

            ],
        initComplete: function () {
            if (visibility == "False") {
                var column = this.api().column(5);
                column.visible(false);
            }
            else {
                var column = this.api().column(5);
                column.visible(true);
            }
            if (IsVendorcolShow) {
                var column = this.api().column(1);
                column.visible(false);
            }
            else {
                var column = this.api().column(1);
                column.visible(true);
            }
            if (rCount > 0) {
                $("#btnAddWoActualOther").hide();
            }
            else {
                if (visibility == "True") {
                    $("#btnAddWoActualOther").show();
                }
                else {
                    $("#btnAddWoActualOther").hide();
                }
            }
            SetPageLengthMenu();
        }
    });
}
function WoActualOtherAddOnSuccess(data) {
    var workOrderId = data.workorderid;
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("OtherActualAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("OtherActualUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(workOrderId, "actualother")
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion


//#region AddFollowUpWorkOrder
$(document).on('click', "#menuFollowModalDetailsPage", function (e) {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    e.preventDefault();
    GetFollowUpWO(workOrderID, clientLookupId);
    $('#EmergencyUnplannedWo').modal('hide');
    $('.modal-backdrop').remove();
});
function GetFollowUpWO(workOrderID, clientLookupId) {
    $.ajax({
        url: "/WorkOrder/FollowUpWO",
        type: "POST",
        dataType: "html",
        data: { WorkoderId: workOrderID, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#btnCancelAddFollowUp", function () {
    var workorderId = $(document).find('#woRequestModel_WorkOrderId').val();
    if (workorderId == 0) {
        swal(CancelAlertSetting, function () {
            window.location.href = "../WorkOrder/Index?page=Maintenance_Work_Order_Search";
        });
    } else {
        RedirectToDetailOncancel(workorderId, "overview");
    }
});
$(document).on('change', "#woFollowupChargeTo", function () {
    $(document).find('#txtChargeTo').val('');
    var type = $(this).val();
    if (type == "") {
        $("#imgChargeToTree").css("display", "none");
    }
    else {
        if (type == "Equipment") {
            $("#imgChargeToTree").css("display", "block");
        }
        else {
            $("#imgChargeToTree").css("display", "none");
        }
    }

});
$(document).on('click', "#openfollowwogrid", function () {
    // commented for v2-608
    //var textChargeToId = $("#woFollowupChargeTo option:selected").val();
    //if (textChargeToId === "Equipment") { generateEquipmentDataTableWO(); }
    //else if (textChargeToId === "Location") { generateLocationDataTable(); }
    generateEquipmentDataTableWO();
});
$(document).on('click', "#brdactualLabour", function () {
    var workorderId = $(this).attr('data-val');
    RedirectToPmDetail(workorderId);
});
function AddFollowUpOnSuccess(data) {
    if (data.Issuccess) {
        localStorage.setItem("workorderstatus", '3');
        localStorage.setItem("workorderstatustext", getResourceValue("spnOpenWorkOrder"));
        SuccessAlertSetting.text = getResourceValue("spnFollowUpAddedSuccessfully");
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(data.WorkOrderId, "overview");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, '#menuFollowModalDetailsPage');
    }
    CloseLoader();
}
//#endregion

//#region RequestOrder
var reqOrderTable;
function generateRequestOrderGrid(workOrderID) {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    if ($(document).find('#woReqOrderTable').hasClass('dataTable')) {
        reqOrderTable.destroy();
    }
    reqOrderTable = $("#woReqOrderTable").DataTable({
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
        dom: 'Btlipr',
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        buttons: [],
        "filter": true,
        "orderMulti": true,
        "ajax": {
            "url": "/WorkOrder/PopulateRequestOrder?workOrderId=" + workOrderID,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                return response.data;
            }
        },
        columnDefs: [{

        }
        ],
        "columns":
            [
                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Vendor", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Created", "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            CloseLoader();
        }
    });
}
//#endregion

//#region Common
function RedirectToDetailOncancel(woId, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToPmDetail(woId, mode);
    });
}

function AddDowntimeOnSuccess(data) {
    CloseLoader();
    if (data.Issuccess) {
        $('#downtimeModalDetailsPage').modal('hide');
        $('#ui-datepicker-div').removeClass('ui-downtime');
        $('.modal-backdrop').remove();
        //v2-463 Values Assign From DowntimeModel To Summary Header
        $("#Summary_EquipDownDate").html(moment(data.Downdate).format("MM/DD/YYYY"));
        $("#Summary_EquipDownHours").html(" " + data.Minutes.toFixed(2) + " mins");
        $("#workOrderSummaryModel_EquipDownDate").val(moment(data.Downdate).format('DD-MMM-YYYY HH:mm:ss a'));
        $("#workOrderSummaryModel_EquipDownHours").val(data.Minutes.toFixed(2));
        SuccessAlertSetting.text = getResourceValue(data.msg);
        swal(SuccessAlertSetting, function () {
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, '#downtimeModalDetailsPage');
    }
}
$(document).on('click', '#ReopenWO', function () {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    swal({
        title: "Are you sure?",
        text: "Please confirm to reopen.",
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        ShowLoader();
        $.ajax({
            url: '/WorkOrder/ReopenWO',
            data: {
                workorderId: workOrderID
            },
            type: "GET",
            datatype: "json",
            success: function (data) {
                if (data.data == "success") {
                    SuccessAlertSetting.text = getResourceValue("WorkorderSuccessfullyReopenAlert");
                    swal(SuccessAlertSetting, function () {
                        RedirectToPmDetail(data.workorderId, "overview");
                    }
                    );
                }
                else if (data.data == "unsuccess") {
                    var msg = getResourceValue("CannotReopenWorkOrderAlert");
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
                else {
                    GenericSweetAlertMethod(data.error);
                }
            }
            , complete: function () {
                CloseLoader();
            },
            error: function () {
                swal.close();
                CloseLoader();
            }
        });
    });
});
$(document).on('click', '#ApproveWO,#ApproveDetailsWO', function () {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    swal({
        title: getResourceValue("spnAreyousure"),
        text: getResourceValue("spnPleaseConfirmToApprove"),
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        ShowLoader();
        $.ajax({
            url: '/WorkOrder/ApproveWO',
            data: {
                workorderId: workOrderID
            },
            type: "GET",
            datatype: "json",
            success: function (data) {
                if (data.data == "success") {
                    SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyApprovedAlert");
                    swal(SuccessAlertSetting, function () {
                        RedirectToPmDetail(data.workorderId, "overview");
                    }
                    );
                }
                else {
                    GenericSweetAlertMethod(data.error);
                }
            }
            , complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    });
});


$(document).on('click', '#DenyDetailsWO', function () {
    $(document).find('#denyModalDetailsPage').modal('show');
});
$(document).on('click', '#denyWorkOrderJob', function () {
    var workOrderID;
    if (layoutType == 1) {
        workOrderID = workOrderIdCard;
    }
    else {
        workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    }

    var comments = $('#txtdenycomments').val();
    if (comments === "") {
        errormsgtitle = getResourceValue("spnNoDenyComments");
        errormsgtext = getResourceValue("spnPleaseEnterDenyComments");
    }
    if (comments !== "") {
        $.ajax({
            url: '/WorkOrder/DenyWoJob',
            data: { WorkorderId: workOrderID, Comments: comments },
            type: "GET",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#denyModalDetailsPage').modal('hide');
                if (data.data == "success") {
                    SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyDeniedAlert");
                    swal(SuccessAlertSetting, function () {
                        if (layoutType == 2) {
                            RedirectToPmDetail(data.WoId, "overview");
                        }
                        else {
                            if ($(document).find('.summaryview').length > 0) {
                                RedirectToPmDetail(data.WoId, "overview");
                            }
                            else {
                                //imagecardviewstartvalue = 0;
                                //imagegrdcardcurrentpage = 1;
                                ShowCardView();
                            }
                        }
                    });
                }
                else {
                    GenericSweetAlertMethod(data.error);
                }
            },
            complete: function () {
                $(document).find('#txtdenycomments').val("").trigger('change');
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
    else {
        swal({
            title: errormsgtitle,
            text: errormsgtext,
            type: "warning",
            showCancelButton: false,
            confirmButtonClass: "btn-sm btn-danger",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("SaveAlertOk"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        });
    }
});

$(document).on('click', '#WorkorderPlanner', function () {

    $.ajax({
        url: '/WorkOrder/PopulatePersonDropdown',
        type: "GET",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $("#personnelselect").empty();
            $("#personnelselect").append("<option value=''>" + "--Select--" + "</option>");
            for (var i = 0; i < data.plannerList.length; i++) {
                var id = data.plannerList[i].Value;
                var name = data.plannerList[i].Text;
                $("#personnelselect").append("<option value='" + id + "'>" + name + "</option>");
            }
        },
        complete: function () {
            $(document).find('.select2picker').select2({});
            $(document).find('#planningWorkOrderListJob').attr('id', 'btnworkorderPlanner');
            $(document).find('#planningModalIndexPage').modal({ backdrop: 'static', keyboard: false });
            $(document).find('#planningModalIndexPage').modal('show');
            CloseLoader();
        }
    });

});

$(document).on('click', "#btnworkorderPlanner", function (e) {
    workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse('#planningForm');
    $('#planningForm').validate();
    var isvalid = $('#planningForm').valid();
    if (!isvalid) {
        $('#planningForm').submit();
    }

    if (isvalid) {

        var jsonResult =
        {
            WorkorderId: workOrderID,
            Planner_PersonnelId: $('#personnelselect').val()
        }

        $.ajax({
            url: '/WorkOrder/PlanningWo',
            type: "POST",
            datatype: "json",
            data: JSON.stringify(jsonResult),
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "success") {
                    SuccessAlertSetting.text = getResourceValue("spnWorkorderPlannerUpdatedAlert");
                    swal(SuccessAlertSetting, function () {
                        $(document).find('#planningModalIndexPage').modal('hide');
                        RedirectToPmDetail(workOrderID, "overview");
                    });
                }
                else {
                    GenericSweetAlertMethod(data.data);
                }
            },
            complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
});



$(document).on('click', ".EmergencyWoPopup", function (e) {
    $('#EmergencyUnplannedWo').modal('hide');
});
$(document).on('click', "#emergencyOnDemandPopUpPageID", function (e) {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    e.preventDefault();
    GoEmergencyOnDemand(workOrderID, clientLookupId);
    $('#EmergencyUnplannedWo').modal('hide');
    $('.modal-backdrop').remove();
});
function GoEmergencyOnDemand(workOrderID, clientLookupId) {
    $.ajax({
        url: "/WorkOrder/EmergencyOnDemand",
        type: "POST",
        dataType: "html",
        data: { WorkoderId: workOrderID, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
        },
        complete: function () {
            SetControls();
            $("#imgChargeToTree").css("display", "none");
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '#btnCancelEmergencyOnDemand,#btnCancelEmergencyDescribe', function (e) {
    var workOrderID = $(this).attr('data-val');
    RedirectToDetailOncancel(workOrderID, "overview");
});
$(function () {
    $("#woEmergencyOnDemandModel_ChargeTo").attr('disabled', 'disabled');
    $(document).on('change', "#woEmergencyOnDemandModel_ChargeTo", function () {
        $(document).find('#woEmergencyOnDemandModel_ChargeToClientLookupId').val($("#woEmergencyOnDemandModel_ChargeTo option:selected").text());
    });
    // commented for v2-608
    //$(document).on('change', "#woEmergencyOnDemandModel_ChargeType", function () {
    //    chargeTypeText = $('option:selected', this).text();
    //    var type = $(this).val();
    //    $("#txtChargeTo").val("");
    //    chargeTypeSelected = type;
    //    if (type == "") {
    //        $("#imgChargeToTree").css("display", "none");
    //        $("#woEmergencyOnDemandModel_ChargeTo").attr('disabled', 'disabled');
    //    }
    //    else {
    //        $("#woEmergencyOnDemandModel_ChargeTo").attr('disabled', false);
    //        if (type == "Equipment") {
    //            $("#imgChargeToTree").css("display", "block");
    //        }
    //        else {
    //            $("#imgChargeToTree").css("display", "none");
    //        }
    //        $("#woEmergencyOnDemandModel_ChargeTo").removeAttr('disabled');
    //    }
    //});
    $(document).on('click', "#btnCancelAddWorkRequest", function () {
        RedirectToDetailOncancelForDashboard();
    });
    function RedirectToDetailOncancelForDashboard() {
        swal(CancelAlertSetting, function () {
            window.location.href = '/Dashboard/Dashboard';
        });
    }
});
$(document).on('click', "#emergencyDescribePopUpPageID", function (e) {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    e.preventDefault();
    GoEmergencyDescribe(workOrderID, clientLookupId);
    $('#EmergencyUnplannedWo').modal('hide');
    $('.modal-backdrop').remove();
});
function GoEmergencyDescribe(workOrderID, clientLookupId) {
    $.ajax({
        // url: "/WorkOrder/EmergencyDescribe",
        url: "/WorkOrder/EmergencyDescribeDynamic", //V2-1067
        type: "POST",
        dataType: "html",
        data: { WorkoderId: workOrderID, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
        },
        complete: function () {
            SetControls();
            $("#imgChargeToTree").css("display", "none");
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(function () {
    $(document).on('change', "#woEmergencyDescribeModel_ChargeTo", function () {
        $(document).find('#woEmergencyDescribeModel_ChargeToClientLookupId').val($("#woEmergencyDescribeModel_ChargeTo option:selected").text());
    });
    // commented for v2-608
    //$(document).on('change', "#woEmergencyDescribeModel_ChargeType", function () {
    //    $(document).find('#txtChargeTo').val("");
    //    chargeTypeText = $('option:selected', this).text();

    //    var type = $(this).val();
    //    chargeTypeSelected = type;

    //    if (type == "") {
    //        $("#imgChargeToTree").css("display", "none");
    //        $("#woEmergencyDescribeModel_ChargeTo").attr('disabled', 'disabled');
    //    }
    //    else {
    //        if (type == "Equipment") {
    //            $("#imgChargeToTree").css("display", "block");
    //        }
    //        else {
    //            $("#imgChargeToTree").css("display", "none");
    //        }
    //        $("#woEmergencyDescribeModel_ChargeTo").removeAttr('disabled');
    //    }
    //});
    $(document).on('click', "#btnCancelAddWorkRequest", function () {
        RedirectToDetailOncancelForDashboard();
    });
    function RedirectToDetailOncancelForDashboard() {
        swal(CancelAlertSetting, function () {
            window.location.href = '/Dashboard/Dashboard';
        });
    }
});
function WorksEmergencyOnSuccess(data) {

    if (data.data == "success") {
        {
            localStorage.setItem("workorderstatus", '1');
            localStorage.setItem("workorderstatustext", (getStatusValue("Scheduled") + " " + getResourceValue("spnWorkOrder")));
            SuccessAlertSetting.text = getResourceValue("spnUnplanedWorkorderAddedSuccessfully");
            swal(SuccessAlertSetting, function () {
                if (data.IsToSanitationPage) {
                    var message = getResourceValue("DoesSanitationNeedToInspectAlert");
                    swal({
                        title: "",
                        text: message,
                        html: true,
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonClass: "btn-sm btn-primary",
                        cancelButtonClass: "btn-sm",
                        confirmButtonText: getResourceValue("CancelAlertYes"),
                        cancelButtonText: getResourceValue("CancelAlertNo")
                    }, function (value) {
                        if (value == true) {
                            GoSanitationDescribe(data.workOrderID);
                        }
                        else {
                            RedirectToPmDetail(data.workOrderID, "overview");
                        }
                    });
                }
                else {
                    RedirectToPmDetail(data.workOrderID, "overview");
                }
            });

        }
    }
    else {
        CloseLoader();
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '.btnCancelDataval', function (e) {
    var workOrderID = $(this).attr('data-val');
    RedirectToDetailOncancel(workOrderID, "overview");
});
$(document).on('click', "#sanitationWoOnDemandPageID", function (e) {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    e.preventDefault();
    GoSanitationOnDemand(workOrderID);
    $('#SanitationAddWO').modal('hide');
    $('.modal-backdrop').remove();
});
function GoSanitationOnDemand(workOrderID) {
    $.ajax({
        url: "/WorkOrder/SanitationOnDemand",
        type: "POST",
        dataType: "html",
        data: { WorkoderId: workOrderID },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '#imgChargeToTreesanitation', function (e) {
    $('#woSanitationTreeModal').modal('show');
    $(this).blur();
    generateTreeForSanitation(-1);
});
function generateTreeForSanitation(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/PlantLocationEquipmentTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: false,
        cache: false,
        beforeSend: function () {
            ShowLoader();
            $(document).find(".cntTree").html("<b>Processing...</b>");
        },
        success: function (data) {

            $(document).find(".cntTree").html(data);
        }
        ,
        complete: function () {
            CloseLoader();
            treeTable($(document).find('#tblTree'));
            $(document).find('.radSelect').each(function () {
                if ($(this).data('plantlocationid') == -1)
                    $(this).attr('checked', true);
            });
        },
        error: function (xhr) {
            alert('error');
        }
    });
}
$(document).on('click', "#sanitationWoDescribePageID", function (e) {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    e.preventDefault();
    GoSanitationDescribe(workOrderID);
    $('#SanitationAddWO').modal('hide');
    $('.modal-backdrop').remove();
});
function GoSanitationDescribe(workOrderID) {
    $.ajax({
        url: "/WorkOrder/SanitationDescribe",
        type: "POST",
        dataType: "html",
        data: { WorkoderId: workOrderID },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SanitationAddOnSuccess(data) {
    CloseLoader();
    if (data.data == "success") {
        if (data.Command == "save") {
            SuccessAlertSetting.text = getResourceValue("spnSanitationAddedSuccessfully");
            swal(SuccessAlertSetting, function () {
                window.location.href = "../SanitationJob/Index?page=Sanitation_Jobs_Search";
            });
        }
        else {
            ResetErrorDiv();
            $('#identificationtab').addClass('active').trigger('click');
            SuccessAlertSetting.text = getResourceValue("AddPartsAlerts");
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change');
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
$(document).on('click', "#openemerwogrid", function () {
    // Commented for V2 - 608
    //var textChargeToId = $("#woEmergencyOnDemandModel_ChargeType option:selected").val();
    //if (textChargeToId == "Equipment") { generateEquipmentDataTable(); }
    //else if (textChargeToId == "Location") { generateLocationDataTable(); }
    generateEquipmentDataTableWO();
});
$(document).on('click', "#woreqopengrid", function () {
    // Commented for V2-608
    //var textChargeToId = $("#woRequestModel_ChargeType option:selected").val();
    //if (textChargeToId == "Equipment") { generateEquipmentDataTable(); }
    //else if (textChargeToId == "Location") { generateLocationDataTable(); }
    generateEquipmentDataTableWO();
});
$(document).on('click', "#opendescribewogrid", function () {
    //// Commented for V2-608
    //var textChargeToId = $("#woEmergencyDescribeModel_ChargeType option:selected").val();
    //if (textChargeToId == "Equipment") { generateEquipmentDataTable(); }
    //else if (textChargeToId == "Location") { generateLocationDataTable(); }
    generateEquipmentDataTableWO();
});
$(document).on('click', "#openwogrid", function () {
    // Commented for V2-608
    //var textChargeToId = $("#workOrderModel_ChargeType option:selected").val();
    //if (textChargeToId == "Equipment") { generateEquipmentDataTable(); }
    //else if (textChargeToId == "Location") { generateLocationDataTable(); }
    generateEquipmentDataTableWO();
});
// Commented for V2-608
//$(document).on("change", "#workOrderModel_ChargeType", function () {
//    $(document).find('#txtChargeTo').val('');
//    var textChargeToId = $("#workOrderModel_ChargeType option:selected").val();

//    if (textChargeToId == "Equipment") {
//        $("#imgChargeToTreeLineItem").css("display", "block");
//        $('#workOrderModel_ChargeToClientLookupId').css("width", "90%");
//    }
//    else {
//        $("#imgChargeToTreeLineItem").css("display", "none");
//        $('#workOrderModel_ChargeToClientLookupId').css("width", "100%");
//    }
//});
// Commented for V2-608
//$(document).on("change", "#woOnDemandModel_ChargeType", function () {
//    $(document).find('#txtChargeTo').val('');
//    var textChargeToId = $("#woOnDemandModel_ChargeType option:selected").val();
//    if (textChargeToId === "Equipment") {
//        $("#imgChargeToTreeLineItem").css("display", "block");
//        $('#woOnDemandModel_ChargeToClientLookupId').css("width", "90%");
//    }
//    else {
//        $("#imgChargeToTreeLineItem").css("display", "none");
//        $('#woOnDemandModel_ChargeToClientLookupId').css("width", "100%");
//    }
//});
$(document).on('click', "#openwogrid_OnDemandModel", function () {
    // Commented for V2-608
    //var textChargeToId = $("#woOnDemandModel_ChargeType option:selected").val();
    //if (textChargeToId === "Equipment") { generateEquipmentDataTable(); }
    //else if (textChargeToId === "Location") { generateLocationDataTable(); }
    generateEquipmentDataTableWO();
});
// Commented for V2-608
//$(document).on("change", "#woDescriptionModel_ChargeType", function () {
//    $(document).find('#txtChargeTo').val('');
//    var textChargeToId = $("#woDescriptionModel_ChargeType option:selected").val();
//    if (textChargeToId === "Equipment") {
//        $("#imgChargeToTreeLineItem").css("display", "block");
//        $('#woDescriptionModel_ChargeToClientLookupId').css("width", "90%");
//    }
//    else {
//        $("#imgChargeToTreeLineItem").css("display", "none");
//        $('#woDescriptionModel_ChargeToClientLookupId').css("width", "100%");
//    }
//});
$(document).on('click', "#openwogrid_WorkOrderDesc", function () {
    // Commented for V2-608
    //var textChargeToId = $("#woDescriptionModel_ChargeType option:selected").val();
    //if (textChargeToId === "Equipment") { generateEquipmentDataTable(); }
    //else if (textChargeToId === "Location") { generateLocationDataTable(); }
    generateEquipmentDataTableWO();
});
//#endregion

$(document).on('click', "#downtimeModel_Downdate", function () {
    if ($('#ui-datepicker-div.ui-datepicker.ui-widget.ui-widget-content.ui-downtime').length == 0) {
        $('#ui-datepicker-div').addClass('ui-downtime');
    }
});


function DowntimeCalenderOpen() {
    if ($('#ui-datepicker-div.ui-datepicker.ui-widget.ui-widget-content.ui-downtime').length == 0) {
        $('#ui-datepicker-div').addClass('ui-downtime');
    }
    return true;
}
function ScheduleCalenderOpen() {
    if ($('#ui-datepicker-div').hasClass('ui-downtime')) {
        $('#ui-datepicker-div').removeClass('ui-downtime');
    }
    return true;
}
//#region Down Time
$(document).on('click', "#downtimeset", function () {
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');

    if ($('#downtimeModel_Minutes').val() != "0.00") {

    } else {
        $('#downtimeModel_Minutes').val(0);
    }

    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.dtpicker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if ($(this).valid()) {
            if (typeof areaddescribedby != 'undefined') {
                $('#' + areaddescribedby).hide();
            }
        }
        else {
            if (typeof areaddescribedby != 'undefined') {
                $('#' + areaddescribedby).show();
            }
        }
    });
    $(".modaDismiss").on("click", function () {
        $('.errormessage').html('').hide();
        $("#downtimeModalDetailsPage").find("select").each(function () {
            //V2 463
            if ($('#downtimeModel_Minutes').val() != "0.00") {

            } else {
                $(this).val('').trigger('change');
            }
            if ($(this).hasClass('input-validation-error')) {
                $(this).removeClass('input-validation-error');
            }
        });
        $("#downtimeModalDetailsPage").find('input[type = text]').each(function () {
            if ($('#downtimeModel_Minutes').val() != "0.00") {

            } else {
                $(this).val('');
            }

            if ($(this).hasClass('input-validation-error')) {
                $(this).removeClass('input-validation-error');
            }
        });
        //V2 463
        if ($('#downtimeModel_Minutes').val() != "0.00") {

        } else {
            $('#downtimeModel_Minutes').val(0);
        }
    });
});

var instructionEditor = "";
$(document).on('click', '#instructionDetail', function () {
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    $.ajax({
        url: '/WorkOrder/LoadIntructions',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
            ClearEditorById('intructionDetail');
        },
        data: {
            WorkOrderId: workorderId,
        },
        success: function (data) {
            LoadCkEditorById('intructionDetail', function (editor) {
                if (data.data) {
                    SetDataById('intructionDetail', data.data.Content);
                    $(document).find('#InstructionContent').val(data.data.Content);
                    $(document).find('#hdnInstruction').val(data.data.InstructionId);
                }
                else {
                    $(document).find('#InstructionContent').val("");
                    $(document).find('#hdnInstruction').val("0");
                }
            });
            $('#instructionDetailModal').modal('show');
        },
        error: function (xhr) {
            console.log(xhr)
        },
        complete: function () {
            CloseLoader();
        }
    });
});

$(document).on('click', '#btnSaveIntro', function () {
    const data = getDataFromEditorById('intructionDetail');
    var requiremessage = getResourceValue("InstructionRequired");

    var instructionId = $(document).find('#hdnInstruction').val();
    if (Number(instructionId) <= 0 && (!data || LRTrim(data) == "")) {
        ShowErrorAlert(requiremessage);
        return false;
    }
    $(document).find('#InstructionContent').val(data);
});
function WOInstructionsOnSuccess(data) {
    if (data && data.Result == "success") {
        var alertmessage = '';
        if (data.mode == "add") {
            alertmessage = getResourceValue("InstructionsAddAlert");
        }
        else {
            alertmessage = getResourceValue("InstructionsUpdateAlert");
        }
        ShowSuccessAlert(alertmessage);
        $('#btnCancelIntro').click();
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
$(document).on('click', '#btnCancelIntro', function () {
    ClearEditorById('intructionDetail');
    $('#instructionDetailModal').modal('hide');
});
//#endregion
//#region send for approval

$(document).on('click', '#actionSendForApprovalWO', function () {
    $(document).find('.select2picker').select2({});
    $(document).find('#SendForApprovalModal').modal('show');

});

function WoSendForApprovalOnSuccess(data) {
    $(document).find('#SendForApprovalModal').modal('hide');
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    if (data.data === "success") {
        SuccessAlertSetting.text = getResourceValue("SendApprovalSuccessAlert");
        swal(SuccessAlertSetting, function () {
            CloseLoader();
            if (layoutType == 2) {
                RedirectToPmDetail(workOrderID, "overview");
            }
            else {
                if ($(document).find('.summaryview').length > 0) {
                    RedirectToPmDetail(workOrderIdCard, "overview");
                }
                else {
                    ShowCardView();
                }
            }
        });

    }
    else {
        CloseLoader();
        GenericSweetAlertMethod(data.data);
        pageno = workOrdersSearchdt.page.info().page;
        workOrdersSearchdt.page(pageno).draw('page');
    }
    $('#ddlSendForApprovalUser').val(null).trigger("change.select2");

}
//#endregion

//#region Add actual Part issue
$(document).on('click', '#AddActualPartsIssue', function () {
    $.ajax({
        url: '/WorkOrder/AddActualPartIssue',
        type: 'POST',
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data != '') {
                $('#divAddParts').html(data);
            }
        },
        complete: function () {
            if ($('#divAddParts').html() != '') {
                SetControls();
                $(document).find("#PartIssueAddModel_IsPartIssue").val(true);
                $(document).find('#AddActualPartIssueModal').modal("show");
            }
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function ActualPartIssueAddOnSuccess(data) {
    $("#AddActualPartIssueModal").find("input,select").val('').end().modal("hide");
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("PartIssueAddAlert");
        swal(SuccessAlertSetting, function () {
            ShowLoader();
            woActualPartsTable.page('first').draw('page');
            CloseLoader();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region Add QR scanner
//#region Parts
$(document).on('click', '#btnQrScannerActual', function () {
    $(document).find('#txtpartid').val('');
    $(document).find('#PartIssueAddModel_PartId').val('');
    if (!$(document).find('#QrCodeReaderModal').hasClass('show')) {
        $(document).find('#QrCodeReaderModal').modal("show");
        StartQRReaderActualParts();
    }
});
function StartQRReaderActualParts() {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 150,
                //aspectRatio: aspectratio //1.7777778
            }, success => {
                onScanSuccessActualParts(success);
            }, error => {
            });
        } else {
            if ($(document).find('#QrCodeReaderModal').hasClass('show')) {
                $(document).find('#QrCodeReaderModal').modal("hide");
            }
        }
    }).catch(e => {
        if ($(document).find('#QrCodeReaderModal').hasClass('show')) {
            $(document).find('#QrCodeReaderModal').modal("hide");
        }
        if (e && e.startsWith('NotReadableError')) {
            ShowErrorAlert(getResourceValue("cameraIsBeingUsedByAnotherAppAlert"));
        }
        else if (e && e.startsWith('NotFoundError')) {
            ShowErrorAlert(getResourceValue("cameraDeviceNotFoundAlert"));
        }

    });
}
function onScanSuccessActualParts(decodedText) {
    var url = "/WorkOrder/GetPartIdByClientLookUpId?clientLookUpId=" + decodedText;
    if ($(document).find('#UseMultiStoreroom').val() == "True") {
        var StoreroomId = $(document).find('#StoreroomId').val();
        url = "/Base/GetPartIdByStoreroomIdAndClientLookUpforMultiStoreroom?ClientLookupId=" + decodedText + "&StoreroomId=" + StoreroomId;
    }
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if ($(document).find('#QrCodeReaderModal').hasClass('show')) {
                $(document).find('#QrCodeReaderModal').modal("hide");
            }
            if (data.PartId > 0) {
                var WOid = $(document).find('#workOrderModel_WorkOrderId').val();
                var WOLookupid = $(document).find('#workOrderModel_ClientLookupId').val();
                $(document).find('#PartIssueAddModel_WorkOrderId').val(WOid);
                $(document).find('#PartIssueAddModel_WorkOrderClientLookupId').val(WOLookupid);
                $(document).find('#txtpartid').val(decodedText).removeClass('input-validation-error');
                $(document).find('#PartIssueAddModel_PartId').val(data.PartId);
            }
            else if (data.MultiStoreroomError == true) {
                var msg = getResourceValue('spnInvalidQrCodeMsgforMultiStoreroom');
                ShowErrorAlert(msg.replace('${decodedText}', decodedText));
            }
            else {
                //Show Error Swal
                ShowErrorAlert(getResourceValue('spnInvalidQrCodeMsg').replace('${decodedText}', decodedText));
            }

        },
        complete: function () {
            StopCamera();
            CloseLoader();
        },
        error: function (xhr) {
            ShowErrorAlert(getResourceValue("somethingWentWrongAlert"));
            CloseLoader();
        }
    });
}
//#endregion
//#endregion Add QR scanner

//#region add returnpart v2-624
var SelectedActualPartId = [];
function ActualPartSelectedItem(PartHistoryId, PartId, PartClientLookupId, Description, TransactionQuantity, TotalCost, UPCCode, StoreroomId) {

    this.PartHistoryId = PartHistoryId;
    this.PartId = PartId;
    this.PartClientLookupId = PartClientLookupId;
    this.Description = Description;
    this.TransactionQuantity = TransactionQuantity;
    this.Cost = TotalCost;
    this.UPCCode = UPCCode;
    this.StoreroomId = StoreroomId;
};
$(document).on('change', '.chksearchactualpart', function () {
    var data = woActualPartsTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        SelectedActualPartId = SelectedActualPartId.filter(function (el) {
            return el.PartHistoryId !== data.PartHistoryId;
        });
    }
    else {
        var item = new ActualPartSelectedItem(data.PartHistoryId, data.PartId, data.PartClientLookupId, data.Description,
            data.TransactionQuantity, data.TotalCost, data.UPCCode, data.StoreroomId);

        SelectedActualPartId.push(item);

    }
    if (SelectedActualPartId.length > 0) {
        $("#AddActualReturnParts").show();
    }
    else {
        $("#AddActualReturnParts").hide();
        SelectedActualPartId = []
    }
})

$(document).on('click', "#AddActualReturnParts", function () {
    var jsonResult = {
        "WoPart": SelectedActualPartId,
        "PartIssueAddModel.WorkOrderId": $(document).find("#workOrderModel_WorkOrderId").val(),
        "PartIssueAddModel.WorkOrderClientLookupId": $(document).find("#workOrderModel_ClientLookupId").val()
    }
    $.ajax({
        url: '/WorkOrder/ReturnActualPartSelectedList',
        type: "POST",
        datatype: "json",
        data: JSON.stringify(jsonResult),
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.data == "success") {
                SuccessAlertSetting.text = getResourceValue("CheckinProcessCompleteSuccessAlert");
                swal(SuccessAlertSetting, function () {

                });
            }
            else {
            }
            $("#AddActualReturnParts").hide();
            $(document).find('.chksearchactualpart').prop('checked', false);
            SelectedActualPartId = [];
            pageno = woActualPartsTable.page.info().page;
            woActualPartsTable.page(pageno).draw('page');
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
})

//#endregion

//#region Show Qrcode
$(document).on('click', '#QrCodeDetail', function () {
    var workorderClientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    $.ajax({
        url: '/Base/LoadQRCode',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        data: {
            WorkorderClientLookupId: workorderClientLookupId
        },
        success: function (data) {
            if (data) {
                DrawQRCode($('#divQrCodeDetail'), data);
                $('#QrCodeDetailModal').modal('show');
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function (xhr) {
            console.log(xhr)
        },
    });
});

//#endregion

//#region Show BarCode
$(document).on('click', '#BarCodeDetails', function () {
    var WOClientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    DrawBarCode('#wocode39barcode', WOClientLookupId)
    $('#barCodeDetailModal').modal('show');

})
//#endregion

//#region material request V2-690
$(document).on('click', "#selectidpartininventory", function (e) {
    e.preventDefault();
    var WorkOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    $('.modal-backdrop').remove();
    if ($(document).find('#workOrderModel_IsUseMultiStoreroom').val() == "True") {
        PopulateStorerooms();
    }
    else {
        GoToAddPartInInventory(WorkOrderID);
    }
});

function GoToAddPartInInventory(WorkOrderID) {
    var ClientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var vendorId = 0;
    var StoreroomId = 0;
    if ($(document).find('#StoreroomId').val() != "undefined" && $(document).find('#StoreroomId').val() > 0) {
        StoreroomId = $(document).find('#StoreroomId').val();
    }
    $.ajax({
        url: "/WorkOrder/AddPartInInventory",
        type: "POST",
        dataType: "html",
        data: { WorkOrderID: WorkOrderID, ClientLookupId: ClientLookupId, vendorId: vendorId, StoreroomId: StoreroomId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            SetFixedHeadStyle();
        },
        complete: function () {
            CloseLoader();
            ShowCardViewWO();
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', "#selectidpartnotininventory", function (e) {
    $('.modal-backdrop').remove();
    AddEstimatePartNotInInventory();
});

function AddEstimatePartNotInInventory() {
    var workorderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $('#workOrderModel_Status').val();
    var description = $('#workOrderModel_Description').val();

    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $(document).find('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $(document).find('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $(document).find('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var ProjectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    $.ajax({
        url: "/WorkOrder/AddEstimatesPartNotInInventory",
        type: "GET",
        dataType: 'html',
        data: { WorkOrderId: workorderId, ClientLookupId: clientLookupId, Status: status, Description: description, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: ProjectClientLookupId, AssetLocation: assetLocation },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddEstimatedParts').hide();
            $('#PartNotInInventoryPopUp').html(data);
            $('#AddPartNotInInventoryModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            CloseLoader();
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '.clearstate', function () {
    $(document).find('#AddPartNotInInventoryModalpopup select').each(function (i, item) {
        $('#' + $(document).find("#" + item.getAttribute('id')).attr('aria-describedby')).hide();
    });
})
function EstimatePartNotInInventoryAddOnSuccess(data) {
    var workOrderId = data.workorderid;
    if (data.Result == "success") {
        $(document).find('#AddPartNotInInventoryModalpopup').modal("hide");
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("MaterialRequestAddedAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("MaterialRequestUpdatedAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(workOrderId, "estimatespart")
        });

    }
    else {
        ShowGenericErrorOnAddUpdate(data);

    }
}
function EditLineItemOnSuccess(data) {
    CloseLoader();
    var workOrderId = data.workOrderId;
    if (data.Result == "success") {
        $(document).find('#AddPartNotInInventoryModalpopup').modal("hide");
        SuccessAlertSetting.text = getResourceValue("MaterialRequestUpdatedAlert");
        swal(SuccessAlertSetting, function () {
            $('.modal-backdrop').remove();
            RedirectToPmDetail(workOrderId, "estimatespart");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

//V2-695************************
var dtDownTimeTable;
function generateWODowntimeGrid() {
    var workOrderID = $('#workOrderModel_WorkOrderId').val();
    var WCount = 0;
    var visibilityCreate;
    var visibilityEdit;
    var visibilityDelete;
    if ($(document).find('#woDowntimeTable').hasClass('dataTable')) {
        dtDownTimeTable.destroy();
    }
    dtDownTimeTable = $("#woDowntimeTable").DataTable({
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
            "url": "/WorkOrder/GetWorkOrder_Downtime",
            "type": "POST",
            data: { workOrderID: workOrderID },
            "datatype": "json",
            "dataSrc": function (json) {
                WCount = json.data.length;
                visibilityCreate = json.secDownTimeAdd;
                visibilityEdit = json.secDownTimeEdit;
                visibilityDelete = json.secDownTimeDelete;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [3], render: function (a, b, data, d) {
                    var actionButtonhtml = "";
                    if (visibilityCreate == true) {
                        actionButtonhtml = '<a class="btn btn-outline-primary addWorkOrderDownTimeBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>';
                        if (visibilityEdit == true) {
                            actionButtonhtml = actionButtonhtml + '<a class="btn btn-outline-success editWorkOrderDownTimeBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                        }
                    }
                    if (visibilityDelete == true) {
                        actionButtonhtml = actionButtonhtml + '<a class="btn btn-outline-danger deleteWorkOrderDownTimeBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    return actionButtonhtml;
                }
            }
        ],
        "columns":
            [
                {
                    "data": "Downdate",
                    "type": "date "
                },
                { "data": "MinutesDown", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": "text-right" },
                { "data": "ReasonForDownDescription", "autoWidth": true, "bSearchable": true, "bSortable": true },//V2-695
                { "className": "text-center" }
            ],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api();
            var rows = api.rows().nodes();
            var getData = api.rows({ page: 'current' }).data();
            // Total over all pages
            total = api.column(1).data().reduce(function (a, b) {
                return parseFloat(a) + parseFloat(b);
            }, 0);
            // Update footer
            $("#woDowntimeTablefoot").empty();
            if (data.length != 0) {
                var footer = "";
                if (end == getData[0].TotalCount) {
                    footer = '<tr><th style="text-align: left !important; font-weight: 500 !important; color: #0b0606 !important">Grand Total</th><th style="text-align: right !important; font-weight: 500 !important; color: #0b0606 !important; padding: 0px 10px 0px 0px !important">' + getData[0].TotalMinutesDown.toFixed(4) + '</th><th></th> <th></th></tr>'
                    $("#woDowntimeTablefoot").empty().append(footer);
                }
            }

        },
        initComplete: function () {
            if (visibilityCreate == false && visibilityDelete == false) {
                var column = this.api().column(3);
                column.visible(false);
            }
            else {
                var column = this.api().column(3);
                column.visible(true);
            }
            if (WCount > 0 || visibilityCreate == false) { $("#btnAddWoDowntime").hide(); }
            else {
                $("#btnAddWoDowntime").show();
            }
            SetPageLengthMenu();
        }
    });
}

//#region Down Time
$(document).on('click', '.addWorkOrderDownTimeBttn', function () {
    AddDowntime();
});
$(document).on('click', '.editWorkOrderDownTimeBttn', function () {
    var row = $(this).parents('tr');
    var data = dtDownTimeTable.row(row).data();
    var DowntimeId = data.DowntimeId;
    EditDowntime(DowntimeId);
});
$(document).on('click', '.deleteWorkOrderDownTimeBttn', function () {
    var row = $(this).parents('tr');
    var data = dtDownTimeTable.row(row).data();
    var DowntimeId = data.DowntimeId;
    DeleteDowntime(DowntimeId);
});
$(document).on('click', "#btnAddWoDowntime", function () {
    AddDowntime();
});
function AddDowntime() {
    var ChargeToId = $(document).find('#workOrderModel_ChargeToId').val();
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $('#workOrderModel_Status').val();
    var description = $('#workOrderModel_Description').val();
    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $(document).find('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $(document).find('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $(document).find('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    var projectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    //v2-847
    var AssetGroup1Name = $(document).find('#workOrderSummaryModel_AssetGroup1Name').val();
    var AssetGroup2Name = $(document).find('#workOrderSummaryModel_AssetGroup2Name').val();
    var AssetGroup1ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup1ClientlookupId').val();
    var AssetGroup2ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup2ClientlookupId').val();
    //
    $.ajax({
        url: '/WorkOrder/RedirectDowntime',
        data: { ChargeToId: ChargeToId, workOrderID: workOrderID, ClientLookupId: clientLookupId, Status: status, Description: description, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: projectClientLookupId, AssetLocation: assetLocation, AssetGroup1Name: AssetGroup1Name, AssetGroup2Name: AssetGroup2Name, AssetGroup1ClientlookupId: AssetGroup1ClientlookupId, AssetGroup2ClientlookupId: AssetGroup2ClientlookupId },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function DownTimeAddOnSuccess(data) {
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("alertDownTimeAdded");
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(data.workOrderid, "downtime")
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnAddDowntimeCancel,#btnEditDowntimeCancel", function () {
    swal(CancelAlertSetting, function () {
        var workOrderID = $(document).find('#wodowntimeModel_WorkOrderId').val();
        RedirectToPmDetail(workOrderID, "downtime")
    });
});
function EditDowntime(DowntimeId) {
    var ChargeToId = $(document).find('#workOrderModel_ChargeToId').val();
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var status = $('#workOrderModel_Status').val();
    var description = $('#workOrderModel_Description').val();
    var type = $(document).find('#spnwotype').text();
    var priority = $(document).find("#spnwopriority").text();
    var chargeto = $(document).find("#spnwoChargeToClientLookupId").text();
    var chargetoname = $(document).find("#spnwoChargeTo_Name").text();
    var scheduleddate = $(document).find("#spnwoscheduledstartdate").text();
    var completedate = $(document).find("#spnwocompletedate").text();
    var ScheduledDuration = $('#workOrderModel_ScheduledDuration').val();
    var assignedFullName = $(document).find('#workOrderSummaryModel_AssignedFullName').val();
    var assigned = $(document).find('#workOrderSummaryModel_Assigned').val();
    var workAssigned_PersonnelId = $(document).find('#workOrderSummaryModel_WorkAssigned_PersonnelId').val();
    var projectClientLookupId = $(document).find('#workOrderModel_ProjectClientLookupId').val();
    var Downdate = $('#workOrderSummaryModel_EquipDownDate').val();
    var DownMinutes = $('#workOrderSummaryModel_EquipDownHours').val();
    var assetLocation = $(document).find('#workOrderModel_AssetLocation').val();
    var DowntimeId = DowntimeId;
    //----------------
    //v2-847
    var AssetGroup1Name = $(document).find('#workOrderSummaryModel_AssetGroup1Name').val();
    var AssetGroup2Name = $(document).find('#workOrderSummaryModel_AssetGroup2Name').val();
    var AssetGroup1ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup1ClientlookupId').val();
    var AssetGroup2ClientlookupId = $(document).find('#workOrderSummaryModel_AssetGroup2ClientlookupId').val();
    //
    $.ajax({
        url: '/WorkOrder/ShowDownTimeEdit',
        data: { ChargeToId: ChargeToId, workOrderID: workOrderID, ClientLookupId: clientLookupId, Status: status, Description: description, Type: type, Priority: priority, ChargeTo: chargeto, ChargeToName: chargetoname, ScheduledDate: scheduleddate, Assigned: assigned, CompleteDate: completedate, ScheduledDuration: ScheduledDuration, AssignedFullName: assignedFullName, WorkAssigned_PersonnelId: workAssigned_PersonnelId, Downdate: Downdate, DownMinutes: DownMinutes, ProjectClientLookupId: projectClientLookupId, AssetLocation: assetLocation, DowntimeId: DowntimeId, AssetGroup1Name: AssetGroup1Name, AssetGroup2Name: AssetGroup2Name, AssetGroup1ClientlookupId: AssetGroup1ClientlookupId, AssetGroup2ClientlookupId: AssetGroup2ClientlookupId },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderworkorder').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function DeleteDowntime(DowntimeId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/WorkOrder/DownTimeDelete',
            data: {
                _DowntimeId: DowntimeId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $("#woDowntimeTablefoot").empty();
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("downtimeDeleteSuccessAlert"));
                    dtDownTimeTable.state.clear;
                }
                else {
                    dtDownTimeTable.destroy();
                    generateWODowntimeGrid();
                    return;
                }
            },
            complete: function () {
                generateWODowntimeGrid();
                CloseLoader();
            }
        });
    });
}
function DownTimeEditOnSuccess(data) {
    SuccessAlertSetting.text = getResourceValue("UpdateDowntimeAlerts");
    if (data.Result == "success") {
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(data.workOrderid, "downtime")
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion

//***end V2-695*****************

//#endregion

//#region Multiple Photo Upload
function CompressImage(files, imageName) {
    new Compressor(files, {
        quality: 0.6,
        convertTypes: ['image/png'],
        convertSize: 100000,
        // The compression process is asynchronous,
        // which means you have to access the `result` in the `success` hook function.
        success(result) {
            if (result.size < files.size) {
                SaveCompressedImage(result, imageName);
            }
            else {
                SaveCompressedImage(files, imageName);
            }
            console.log('file name ' + result.name + ' after compress ' + result.size);
        },
        error(err) {
            console.log(err.message);
        },
    });
}
function SaveCompressedImage(data, imageName) {
    var AddPhotoFileData = new FormData();
    AddPhotoFileData.append('file', data, imageName);

    $.ajax({
        url: '../base/SaveUploadedFile',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: AddPhotoFileData,
        success: function (result) {
            //alert(result);
            SaveMultipleUploadedFileToServer(imageName);
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}
$(document).on('change', '#files', function () {
    var _isMobile = CheckLoggedInFromMob();
    var val = $(this).val();
    var imageName = val.replace(/^.*[\\\/]/, '')
    //console.log(imageName);
    var fileUpload = $("#files").get(0);
    var files = fileUpload.files;
    var fileExt = imageName.substr(imageName.lastIndexOf('.') + 1).toLowerCase();
    if (fileExt != 'jpeg' && fileExt != 'jpg' && fileExt != 'png' && fileExt != 'JPEG' && fileExt != 'JPG' && fileExt != 'PNG') {
        ShowErrorAlert(getResourceValue("spnValidImage"));
        $('#files').val('');
        //e.preventDefault();
        return false;
    }
    else if (this.files[0].size > (1024 * 1024 * 10)) {
        ShowImageSizeExceedAlert();
        $('#files').val('');
        //e.preventDefault();
        return false;
    }
    else {
        //alert('Hello');
        swal(AddImageAlertSetting, function () {
            if (window.FormData !== undefined) {
                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    console.log('file name ' + files[i].name + ' before compress ' + files[i].size);
                    if (_isMobile == true) {
                        var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
                        var imgname = workOrderID + "_" + Math.floor((new Date()).getTime() / 1000);
                        CompressImage(files[i], imgname + "." + fileExt);
                    }
                    else {
                        CompressImage(files[i], imageName);
                    }
                }
            } else {
                //alert("FormData is not supported.");
            }
            $('#files').val('');

        });
    }
});
//endregion

//#region Show Images
var imagecardviewstartvalue = 0;
var imagecardviewlwngth = 10;
var imagegrdcardcurrentpage = 1;
var imagecurrentorderedcolumn = 1;
var layoutTypeWO = 1;
function LoadImages(WorkOrderId) {
    $.ajax({
        url: '/WorkOrder/GetImages',
        type: 'POST',
        data: {
            currentpage: imagegrdcardcurrentpage,
            start: imagecardviewstartvalue,
            length: imagecardviewlwngth,
            WorkOrderId: WorkOrderId
        },
        beforeSend: function () {
            $(document).find('#imagedataloader').show();
        },
        success: function (data) {
            //console.log(data.imageAttachmentModels);
            //if (data.TotalCount > 0) {
            $(document).find('#ImageGrid').show();
            $(document).find('#WorkOrderImages').html(data).show();
            $(document).find('#tblimages_paginate li').each(function (index, value) {
                $(this).removeClass('active');
                if ($(this).data('currentpage') == imagegrdcardcurrentpage) {
                    $(this).addClass('active');
                }
            });
            //}
            //else {
            //    $(document).find('#ImageGrid').hide();
            //}
        },
        complete: function () {
            $(document).find('#imagedataloader').hide();
            $(document).find('#imagecardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(imagecardviewlwngth).trigger('change.select2');
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('change', '#imagecardviewpagelengthdrp', function () {
    var WorkOrderId = $(document).find('#workOrderModel_WorkOrderId').val();
    imagecardviewlwngth = $(this).val();
    imagegrdcardcurrentpage = parseInt(imagecardviewstartvalue / imagecardviewlwngth) + 1;
    imagecardviewstartvalue = parseInt((imagegrdcardcurrentpage - 1) * imagecardviewlwngth) + 1;
    //GetAndSaveState();
    LoadImages(WorkOrderId);

});
$(document).on('click', '#tblimages_paginate .paginate_button', function () {
    var WorkOrderId = $(document).find('#workOrderModel_WorkOrderId').val();
    if (layoutTypeWO == 1) {
        var currentselectedpage = parseInt($(document).find('#tblimages_paginate .pagination').find('.active').text());
        imagecardviewlwngth = $(document).find('#imagecardviewpagelengthdrp').val();
        imagecardviewstartvalue = imagecardviewlwngth * (parseInt($(this).find('.page-link').text()) - 1);
        var lastpage = parseInt($(this).prev('li').data('currentpage'));

        if ($(this).attr('id') == 'tbl_previous') {
            if (currentselectedpage == 1) {
                return false;
            }
            imagecardviewstartvalue = imagecardviewlwngth * (currentselectedpage - 2);
            imagegrdcardcurrentpage = imagegrdcardcurrentpage - 1;
        }
        else if ($(this).attr('id') == 'tbl_next') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            imagecardviewstartvalue = imagecardviewlwngth * (currentselectedpage);
            imagegrdcardcurrentpage = imagegrdcardcurrentpage + 1;
        }
        else if ($(this).attr('id') == 'tbl_first') {
            if (currentselectedpage == 1) {
                return false;
            }
            imagegrdcardcurrentpage = 1;
            imagecardviewstartvalue = 0;
        }
        else if ($(this).attr('id') == 'tbl_last') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            imagegrdcardcurrentpage = parseInt($(this).prevAll('li').eq(1).text());
            imagecardviewstartvalue = imagecardviewlwngth * (imagegrdcardcurrentpage - 1);
        }
        else {
            imagegrdcardcurrentpage = $(this).data('currentpage');
        }
        //GetAndSaveState();
        LoadImages(WorkOrderId);

    }
    else {
        run = true;
    }
});
//#endregion
//#region Set As Default
$(document).on('click', '#selectidSetAsDefault', function () {
    var AttachmentId = $(this).attr('dataid');
    $('#' + AttachmentId).hide();
    var WorkOrderId = $(document).find('#workOrderModel_WorkOrderId').val();
    $('.modal-backdrop').remove();
    $.ajax({
        url: '../base/SetImageAsDefault',
        type: 'POST',

        data: { AttachmentId: AttachmentId, objectId: WorkOrderId, TableName: "WorkOrder" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result === "success") {
                //$('.modal-backdrop').remove();
                //$('#' + AttachmentId).hide();
                $('#EquipZoom').attr('src', data.imageurl);
                $('#EquipZoom').attr('data-zoom-image', data.imageurl);
                $('.equipImg').attr('src', data.imageurl);
                //$(document).find('#AzureImage').append('<a href="javascript:void(0)" id="deleteImg" class="trashIcon" title="Delete"><i class="fa fa-trash"></i></a>');
                $('#EquipZoom').data('zoomImage', data.imageurl).elevateZoom(
                    {
                        zoomType: "window",
                        lensShape: "round",
                        lensSize: 1000,
                        zoomWindowFadeIn: 500,
                        zoomWindowFadeOut: 500,
                        lensFadeIn: 100,
                        lensFadeOut: 100,
                        easing: true,
                        scrollZoom: true,
                        zoomWindowWidth: 450,
                        zoomWindowHeight: 450
                    });
                $("#EquipZoom").on('load', function () {
                    //LoadImages(EquimentId);
                    CloseLoader();
                    ShowImageSetSuccessAlert();
                });
                //RedirectToEquipmentDetail(EquimentId, "OnPremiseImageReload");
                //ShowImageSaveSuccessAlert();
            }
            //else {
            //    CloseLoader();
            //    //var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
            //    //ShowErrorAlert(errorMessage);

            //}
        },
        complete: function () {
            //CloseLoader();
            LoadImages(WorkOrderId);
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion
//#region Delete Image
$(document).on('click', '#selectidDelete', function () {
    var AttachmentId = $(this).attr('dataid');
    $('#' + AttachmentId).hide();
    var WorkOrderId = $(document).find('#workOrderModel_WorkOrderId').val();
    var ClientOnPremise = $('#workOrderModel_ClientOnPremise').val();
    $('.modal-backdrop').remove();
    if (ClientOnPremise == 'True') {
        DeleteOnPremiseMultipleImage(WorkOrderId, AttachmentId);
    }
    else {
        DeleteAzureMultipleImage(WorkOrderId, AttachmentId);
    }
});

function DeleteOnPremiseMultipleImage(WorkOrderId, AttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '../base/DeleteMultipleImageFromOnPremise',
            type: 'POST',
            data: { AttachmentId: AttachmentId, objectId: WorkOrderId, TableName: "WorkOrder" },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToPmDetail(WorkOrderId, "OnPremiseImageReload");
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function DeleteAzureMultipleImage(WorkOrderId, AttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '../base/DeleteMultipleImageFromAzure',
            type: 'POST',
            data: { AttachmentId: AttachmentId, objectId: WorkOrderId, TableName: "WorkOrder" },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    //LoadImages(EquimentId);
                    RedirectToPmDetail(WorkOrderId, "AzureImageReload");
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
//#endregion
//region Save Multiple Image
function SaveMultipleUploadedFileToServer(imageName) {
    var WorkOrderId = $(document).find('#workOrderModel_WorkOrderId').val();
    $.ajax({
        url: '../base/SaveMultipleUploadedFileToServer',
        type: 'POST',

        data: { 'fileName': imageName, objectId: WorkOrderId, TableName: "WorkOrder", AttachObjectName: "WorkOrder" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {

                CloseLoader();
                ShowImageSaveSuccessAlert();
            }
            else if (data.result == "1") {
                CloseLoader();
                var errorMessage = getResourceValue("ImageExistAlert");
                ShowErrorAlert(errorMessage);

            }
            else {
                CloseLoader();
                var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
                ShowErrorAlert(errorMessage);

            }
        },
        complete: function () {
            LoadImages(WorkOrderId);
        },
        error: function () {
            CloseLoader();
        }
    });
}
//endregion

//#region V2-719
$(document).on('click', '#AwaitApprovalBtnWoDetail,#AwaitApprovalWoDetail', function () {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    swal({
        title: getResourceValue("spnAreyousure"),
        text: getResourceValue("spnPleaseConfirmToApprove"),
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        ShowLoader();
        $.ajax({
            url: '/WorkOrder/ApproveWO',
            data: {
                workorderId: workOrderID
            },
            type: "GET",
            datatype: "json",
            success: function (data) {
                if (data.data == "success") {
                    SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyApprovedAlert");
                    swal(SuccessAlertSetting, function () {
                        RedirectToPmDetail(data.workorderId, "overview");
                    }
                    );
                }
                else {
                    GenericSweetAlertMethod(data.error);
                }
            }
            , complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    });
});


$(document).on('click', '#AwaitDenyBtnWoDetail,#AwaitDenyWoDetail', function () {
    $(document).find('#txtdenycomments').val('');
    $(document).find('#denyModalDetailsPage').modal('show');
});
//#endregion

//#region V2-726

$(document).on('click', "#sendmaterialrequestitemsforapproval", function (e) {
    e.preventDefault();
    $('.modal-backdrop').remove();
    GoToSendForApproval();
});

function GoToSendForApproval() {
    var WorkOrderId = $(document).find('#workOrderModel_WorkOrderId').val();
    $.ajax({
        url: "/WorkOrder/SendForApproval",
        type: "GET",
        dataType: 'html',
        data: { WorkOrderId: WorkOrderId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#SendForApprovalPopup').html(data);
            $('#SendForApprovalModalPopup').modal({ backdrop: 'static', keyboard: false, show: true });
            if ($('#ApprovalRouteModel_ApproverCount').val() != 1) {
                $('#Approver').val(null).trigger("change.select2");
            }
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

function SendForApprovalOnSuccess(data) {
    $(document).find('#SendForApprovalModalPopup').modal('hide');
    var WorkOrderId = data.WorkOrderId;
    var ApprovalGroupId = data.ApprovalGroupId;
    if (data.data === "success") {
        if (ApprovalGroupId >= 0) {
            SuccessAlertSetting.text = getResourceValue("SendApprovalSuccessAlert");
            swal(SuccessAlertSetting, function () {
                CloseLoader();
                RedirectToPmDetail(WorkOrderId, "estimatespart")
            });
        }
        else {
            ErrorAlertSetting.text = "You have been not assigned any Approval Group";
            swal(ErrorAlertSetting, function () {
                CloseLoader();
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
    //$('#Approver').val(null).trigger("change.select2");

}

function SendWRForApprovalOnSuccess(data) {
    $(document).find('#SendForApprovalModalDetailsPage').modal('hide');
    var WorkOrderId = data.WorkOrderId;
    var ApprovalGroupId = data.ApprovalGroupId;
    if (data.data === "success") {
        if (ApprovalGroupId >= 0) {
            SuccessAlertSetting.text = getResourceValue("SendApprovalSuccessAlert");
            swal(SuccessAlertSetting, function () {
                CloseLoader();
                RedirectToPmDetail(WorkOrderId, "overview");
            });
        }
        //else {
        //    ErrorAlertSetting.text = "You have been not assigned any Approval Group";
        //    swal(ErrorAlertSetting, function () {
        //        CloseLoader();
        //    });
        //  }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
    //$('#Approver').val(null).trigger("change.select2");
}
$(document).on('click', '#btncancelsendForApproval,.clearstate', function () {
    var areaChargeToId = "";
    $(document).find('#SendForApprovalModalPopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });

});
$(document).on('click', '#btncancelsendWRForApproval,.clearstate1', function () {
    var areaChargeToId = "";
    $(document).find('#SendForApprovalModalDetailsPage select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });

});
//#endregion

//#region V2-732
function PopulateStorerooms() {
    $(document).find('#AddEstimatedParts').modal('hide');
    $.ajax({
        url: "/WorkOrder/PopulateStorerooms",
        type: "GET",
        dataType: 'html',
        //data: { MaterialRequestId: MaterialRequestId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#StoreroomListPopUp').html(data);
            $('#StoreroomListModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
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

function SelctStoreroomOnSuccess(data) {
    var WorkOrderId = $(document).find('#workOrderModel_WorkOrderId').val();
    $('.modal-backdrop').remove();
    if (data.data === "success") {
        $(document).find('#StoreroomListModalpopup').hide();
        GoToAddPartInInventory(WorkOrderId);
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#btnSubmitStoreroomcancel,.clearstate', function () {
    var areaChargeToId = "";
    $(document).find('#StoreroomListModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });

});
//#endregion
$(document).on('click', '#linkToEquipment', function (e) {
    clearDropzone();
    var EquipmentId = $(document).find('#workOrderModel_ChargeToId').val();
    window.location.href = "../Equipment/DetailFromWorkOrder?EquipmentId=" + EquipmentId;
});
//#endregion

//#region V2-730
$(document).on('click', '#MultiLevelApproveWO', function () {
    swal({
        title: getResourceValue("CancelAlertSure"),
        text: getResourceValue("spnPleaseConfirmToApprove"),
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: true,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
       
    }, function () {
        var WorkOrderId = $(document).find('#workOrderModel_WorkOrderId').val();
        var ApprovalGroupId = $(document).find('#ApprovalRouteModelByObjectId_ApprovalGroupId').val();
        var clientLookupId = $(document).find("#workOrderModel_ClientLookupId").val();
        MultiLevelApproveWR(WorkOrderId, ApprovalGroupId, clientLookupId);
    });
});

function MultiLevelApproveWR(WorkOrderId, ApprovalGroupId, clientLookupId) {
    $.ajax({
        url: '/WorkOrder/MultiLevelApproveWR',
        data: {
            WorkRequestId: WorkOrderId, ApprovalGroupId: ApprovalGroupId, ClientLookupId: clientLookupId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('.sweet-overlay').fadeOut();
            $('.showSweetAlert').fadeOut();
            if (data.data == "success") {
                if (data.ApproverList.length > 0) {
                    $.ajax({
                        url: "/WorkOrder/SendWRForMultiLevelApproval",
                        type: "POST",
                        dataType: 'html',
                        data: { Approvers: data.ApproverList, WorkOrderId: WorkOrderId, ApprovalGroupId: ApprovalGroupId },
                        beforeSend: function () {
                            ShowLoader();
                        },
                        success: function (data) {
                            $('#MultiLevelApproverListPopUp').html(data);
                            $('#MultiLevelApproverListModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
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
                else {
                    $.ajax({
                        url: "/WorkOrder/MultiLevelFinalApprove",
                        type: "GET",
                        datatype: "json",
                        data: { WorkOrderId: WorkOrderId, ApprovalGroupId: ApprovalGroupId },
                        beforeSend: function () {
                            ShowLoader();
                        },
                        success: function (data) {
                            var WorkOrderId = data.WorkOrderId;
                            if (data.data === "success") {
                                if (data.ApprovalGroupId >= 0) {
                                    SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyApprovedAlert");
                                    swal(SuccessAlertSetting, function () {
                                        CloseLoader();

                                        RedirectToPmDetail(WorkOrderId);
                                    });
                                }
                            }
                            else {
                                ShowGenericErrorOnAddUpdate(data);
                            }
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
            }
            else {
                GenericSweetAlertMethod(data.data);
            }
        },
        complete: function () {
            CloseLoader();
            SetControls();
        }
    });
};

function SendWRForMultiLevelApprovalOnSuccess(data) {
    $(document).find('#MultiLevelApproverListModalpopup').modal('hide');
    var WorkOrderId = data.WorkOrderId;
    if (data.data === "success") {
        if (data.ApprovalGroupId >= 0) {
            SuccessAlertSetting.text = getResourceValue("SendApprovalSuccessAlert");
            swal(SuccessAlertSetting, function () {
                CloseLoader();
                RedirectToPmDetail(WorkOrderId);
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', '#denyMultiLevelWR', function () {
    swal({
        title: getResourceValue("CancelAlertSure"),
        text: getResourceValue("spnPleaseConfirmToDeny"),
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        var WorkOrderId = $(document).find('#workOrderModel_WorkOrderId').val();
        var ApprovalGroupId = $(document).find('#ApprovalRouteModelByObjectId_ApprovalGroupId').val();
        var clientLookupId = $(document).find("#workOrderModel_ClientLookupId").val();
        MultiLevelDenyWR(WorkOrderId, ApprovalGroupId, clientLookupId);
    });
});
function MultiLevelDenyWR(WorkOrderId, ApprovalGroupId, clientLookupId) {
    $.ajax({
        url: '/WorkOrder/MultiLevelDenyWOJob',
        data: {
            WorkOrderId: WorkOrderId, ApprovalGroupId: ApprovalGroupId, ClientLookupId: clientLookupId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.data == "success") {
                SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyDeniedAlert");
                swal(SuccessAlertSetting, function () {
                    RedirectToPmDetail(WorkOrderId);
                });
            }
            else {
                ErrorAlertSetting.text = getResourceValue("FailedAlert");
                swal(ErrorAlertSetting, function () {
                    RedirectToPmDetail(WorkOrderId);
                });
            }
        },
        complete: function () {
            CloseLoader();
            SetControls();
        }
    });
}
//#endregion

//#region V2-1006
$(document).on('change', '#attachmentModel_FileContent', function () {
    var val = $(this).val();
    var fileName = val.replace(/^.*[\\\/]/, '');
    var fileExt = fileName.substr(fileName.lastIndexOf('.') + 1).toLowerCase();
    if (fileExt == 'PDF' || fileExt == 'pdf') {
        $('#attachmentModel_PrintwithForm').removeAttr('disabled');
    }
    else {
        $('#attachmentModel_PrintwithForm').prop('checked', false);
        $('#attachmentModel_PrintwithForm').attr("disabled", true);
    }
});
//#endregion

//#region V2-1056 AddSanitationRequestWO
$(document).on('click', "#AddSanitationRequestBtn", function (e) {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var clientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    $.ajax({
        url: "/WorkOrder/AddSanitationRequestWO",
        type: "GET",
        dataType: 'html',
        data: {
            'WorkoderId': workOrderID, 'ClientLookupId': clientLookupId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddSanitationRequestWOPopUp').html(data);
            $('#AddSanitationRequestWOModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});

$(document).on('click', '#btnCancelAddSanitationRequestWO', function () {
    $("#AddSanitationRequestWOModalpopup").modal('hide');
});
$(document).on('hidden.bs.modal', '#AddSanitationRequestWOModalpopup', function () {
    $('#AddSanitationRequestWOPopUp').html('');
});

function AddSanitationRequestWOOnSuccess(data) {
    if (data.Issuccess) {
        $("#AddSanitationRequestWOModalpopup").modal('hide');
        SuccessAlertSetting.text = getResourceValue("SanitationRequestAddAlert");
        swal(SuccessAlertSetting, function () {

        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, '#AddSanitationRequestWOModalpopup');
    }
    CloseLoader();



}
//#endregion
//#region V2-1051
$(document).on('click', '#btnWOModel', function () {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    $.ajax({
        url: '/WorkOrder/WOMOdel',
        data: {
            WorkOrderID: workOrderID
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.data == "success") {
                localStorage.setItem("workorderstatustext", getResourceValue("spnOpenWorkOrder"));
                localStorage.setItem("workorderstatus", '3');
                SuccessAlertSetting.text = getResourceValue("WorkOrderAddAlert");
                swal(SuccessAlertSetting, function () {
                    RedirectToPmDetail(data.CreatedWorkOrderId, 'overview');
                });
            }
        },
        complete: function () {
            CloseLoader();
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("workorderstatustext"));
        }
    });
});
//#endregion