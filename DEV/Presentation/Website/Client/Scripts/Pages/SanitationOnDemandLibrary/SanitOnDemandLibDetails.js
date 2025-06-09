function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "Task":
            GenerateSanitTaskGrid();
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
//#region Task
function GenerateSanitTaskGrid() {
    var rCount = 0;
    var showAddEditBtn = false;
    var showDeleteBtn = false;
    var MasterId = $(document).find('#sanitationOnDemandLibModel_SanOnDemandMasterId').val();
    if ($(document).find('#sanitTaskTable').hasClass('dataTable')) {
        dtTaskTable.destroy();
    }
    dtTaskTable = $("#sanitTaskTable").DataTable({
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
            "url": "/SanitationOnDemandLibrary/GetSanitOnDemandLibTaskGrid",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.SanOnDemandMasterId = MasterId
            },
            "dataSrc": function (response) {
                showAddEditBtn = response.showAddEditBtn;
                showDeleteBtn = response.showDeleteBtn;
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [2], render: function (a, b, data, d) {
                    if (showAddEditBtn && showDeleteBtn) {
                        return '<a class="btn btn-outline-primary addtaskBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editTaskBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger deltaskBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else if (showAddEditBtn && showDeleteBtn == false) {
                        return '<a class="btn btn-outline-primary addtaskBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editTaskBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                    }
                    else if (showAddEditBtn == false && showDeleteBtn == true) {
                        return '<a class="btn btn-outline-danger deltaskBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    if (showAddEditBtn == false && showDeleteBtn == false) {
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
            if (rCount > 0 || showAddEditBtn == false) {
                $('#btnAddTask').hide();
            }
            else {
                $('#btnAddTask').show();
            }
            if (showAddEditBtn == false && showDeleteBtn == false)
            {
                var column = this.api().column(2);
                column.visible(false);
            }
            else
            {
                var column = this.api().column(2);
                column.visible(true);
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnAddTask,.addtaskBttn", function () {
    var masterId = $(document).find('#sanitationOnDemandLibModel_SanOnDemandMasterId').val();
    var tastId = $(document).find('#sanitOnDemandLibTaskModel_TaskId').val();
    var clientLookupId = $(document).find('#sanitationOnDemandLibModel_ClientLookUpId').val();
    $.ajax({
        url: "/SanitationOnDemandLibrary/AddOrEditSanitTask",
        type: "GET",
        dataType: 'html',
        data: { sanOnDemandMasterId: masterId, taskId: tastId, clientLookUpId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitationondemandLibrary').html(data);
        },
        complete: function () {
            SetOnDemmandControl();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.deltaskBttn', function () {
    var data = dtTaskTable.row($(this).parents('tr')).data();
    var sanOnDemandMasterTaskId = data.SanOnDemandMasterTaskId;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SanitationOnDemandLibrary/DeleteSanitTask',
            data: {
                sanOnDemandMasterTaskId: sanOnDemandMasterTaskId
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
                GenerateSanitTaskGrid();
            }
        });
    });
});
$(document).on('click', '.editTaskBttn', function () {
    var data = dtTaskTable.row($(this).parents('tr')).data();
    EditTask(data);
});
$(document).on('click', "#btnTaskcancel", function () {
    var masterId = $(document).find('#sanitOnDemandLibTaskModel_SanOnDemandMasterId').val();
    RedirectToSureOncancel(masterId, "task");
});
function EditTask(data) {
    var masterId = $(document).find('#sanitationOnDemandLibModel_SanOnDemandMasterId').val();
    var clientLookupId = $(document).find('#sanitationOnDemandLibModel_ClientLookUpId').val();
    $.ajax({
        url: "/SanitationOnDemandLibrary/AddOrEditSanitTask",
        type: "GET",
        dataType: 'html',
        data: {
            taskId: data.TaskId,
            sanOnDemandMasterId: masterId,
            sanitOnDemandMasterTaskId: data.SanOnDemandMasterTaskId,
            description: data.Description,
            clientLookUpId: clientLookupId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitationondemandLibrary').html(data);
        },
        complete: function () {
            SetOnDemmandControl();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function TaskAddOnSuccess(data) {
    CloseLoader();
    var OnDemandMasterId = data.SanOnDemandMasterId;
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("TaskAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("TaskUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToonDemandDetail(OnDemandMasterId, "task");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion task
//#region Common
function RedirectToSureOncancel(MasterId, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToonDemandDetail(MasterId, mode);
    });
}
function RedirectToonDemandDetail(OnDemandMasterId, mode) {
    if (OnDemandMasterId == 0) {
        window.location.href = "/SanitationOnDemandLibrary/index?page=Sanitation_Jobs_Sanitation_-_On_Demand";
    }
    else {
        $.ajax({
            url: "/SanitationOnDemandLibrary/SanitOnDemandLibDetails",
            type: "GET",
            dataType: "html",
            data: { sanOnDemandMasterId: OnDemandMasterId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#rendersanitationondemandLibrary').html(data);
            },
            complete: function () {
                CloseLoader();
                if (mode === "task") {
                    $('#litask').trigger('click');
                    $('#colorselector').val('Tasks');
                }
                SetOnDemmandControl();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
}

//#endregion Common