var dtMainOnDemand;
var dtTaskTable
var typeVal;
var run = false;
$(document).ready(function () {
    $(document).on('click', "#action", function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $(document).on('focusout', "#action", function () {
        $(".actionDrop").fadeOut();
    });
    $(document).find("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '#dismiss, .overlay', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).find('.select2picker').select2({});

    $(document).on('click', '#drpDwnLink', function (e) {
        e.preventDefault();
        $(document).find("#drpDwn").slideToggle();
    });
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
    $(".actionBar").fadeIn();
    $("#MaintenanceOnDemandGridAction :input").attr("disabled", "disabled");
    generateMaintanceOndemandDataTable();
});
$(document).on('click', "#sidebarCollapse", function () {
    $('#renderOnDemandLibrary').find('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "Tasks":
            GenerateTaskGrid();
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
    if ($(this).find('#drpDwnLink').length > 0) {
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
//#region GenerateDT func()
function generateMaintanceOndemandDataTable() {
    var printCounter = 0;
    if ($(document).find('#maintananceOnDemandSearch').hasClass('dataTable')) {
        dtMainOnDemand.destroy();
    }
    dtMainOnDemand = $("#maintananceOnDemandSearch").DataTable({
        colReorder: {
            fixedColumnsLeft:0
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: true,
        "stateSaveCallback": function (settings, data) {
            // Send an Ajax request to the server with the state object
            if (run == true) {
                $.ajax({
                    "url": gridStateSaveUrl,//"/Base/CreateUpdateState",
                    "data": {
                        GridName: "MaintenanceOnDemandProcedures_Search",
                        LayOutInfo: JSON.stringify(data)
                    },
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
            run = false;
        },
        "stateLoadCallback": function (settings, callback) {
            var o;
            $.ajax({
                "url": gridStateLoadUrl,//"/Base/GetState",
                "data": {
                    GridName: "MaintenanceOnDemandProcedures_Search",
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    if (json) {
                        callback(JSON.parse(json));
                    }
                    else {
                        callback(json);
                    }

                }
            });
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Maintenance On-Demand'
            },
            {
                extend: 'print',
                title: 'Maintenance On-Demand'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Maintenance On-Demand',
                extension: '.csv',
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                orientation: 'landscape',
                pageSize: 'A3',
                title: 'Maintenance On-Demand'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/MaintenanceOnDemandLibrary/GetMOnDemandGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.MaintOnDemandMasterId = LRTrim($("#txtOnDemandId").val());
                d.Description = LRTrim($("#txtDescription").val());
                d.Type = LRTrim($("#ddlType").val());
                d.CreateDate = LRTrim($("#txtCreated").val());
                d.srcData = LRTrim($("#txtsearchbox").val());
            },
            "dataSrc": function (result) {
                $("#ddlType").empty();
                $("#ddlType").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < result.typeList.length; i++) {
                    var id = result.typeList[i];
                    var name = result.typeList[i];
                    $("#ddlType").append("<option value='" + id + "'>" + name + "</option>");
                }
                if (typeVal) {
                    $("#ddlType").val(typeVal);
                }
                if (result.data.length == "0") {
                    $(document).find('.import-export').attr('disabled', 'disabled');
                }
                else {
                    $(document).find('.import-export').removeAttr('disabled');
                }

                return result.data;
            },
            global: true
        },
        "columns":
        [
            {
                "data": "ClientLookUpId",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "className": "text-left",
                "name": "0",
                "mRender": function (data, type, row) {
                    return '<a class=lnk_onDetails href="javascript:void(0)">' + data + '</a>';
                }
            },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap'>" + data + "</div>";
                }
            },
            { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2", },
            { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date ", "name": "3", }
        ],
        "columnDefs": [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            $("#MaintenanceOnDemandGridAction :input").removeAttr("disabled");
            $("#MaintenanceOnDemandGridAction :button").removeClass("disabled");
            DisableExportButton($("#maintananceOnDemandSearch"), $(document).find('.import-export'));
        }
    });
}
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            dtTable = $("#maintananceOnDemandSearch").DataTable();
            var currestsortedcolumn = $('#maintananceOnDemandSearch').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#maintananceOnDemandSearch').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#maintananceOnDemandSearch').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var jsonResult = $.ajax({
                "url": "/MaintenanceOnDemandLibrary/GetMOnDemandPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    OrderCol: "0",
                    OrderDir: "asc",
                    MaintOnDemandMasterId: LRTrim($("#txtOnDemandId").val()),
                    Description: LRTrim($("#txtDescription").val()),
                    Type: LRTrim($("#ddlType").val()),
                    CreateDate: LRTrim($("#txtCreated").val()),
                    srcData: LRTrim($("#txtsearchbox").val()),
                    colname: colname,
                    coldir: coldir
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#maintananceOnDemandSearch thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.ClientLookupId != null) {
                    item.ClientLookupId = item.ClientLookupId;
                }
                else {
                    item.ClientLookupId = "";
                }
                if (item.Description != null) {
                    item.Description = item.Description;
                }
                else {
                    item.Description = "";
                }
                if (item.Type != null) {
                    item.Type = item.Type;
                }
                else {
                    item.Type = "";
                }
                if (item.CreateDate != null) {
                    item.CreateDate = item.CreateDate;
                }
                else {
                    item.CreateDate = "";
                }
                var fData = [];
                $.each(visiblecolumnsIndex, function (index, inneritem) {
                    var key = Object.keys(item)[inneritem];
                    var value = item[key]
                    fData.push(value);
                });
                d.push(fData);
            })
            return {
                body: d,
                header: $("#maintananceOnDemandSearch thead tr th").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
})
//#endregion
//#region Search
$(document).on('click', '#liPdf', function () {
    $(".buttons-pdf")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liPrint', function () {
    $(".buttons-print")[0].click();
    funcCloseExportbtn();
});
//$('#btnDropdownSearch').on('click', function () {
//    clearAdvanceSearch();
//    dtMainOnDemand.page('first').draw('page');
//});
$("#btnPRDataAdvSrch").on('click', function (e) {
    run = true;
    $("#txtsearchbox").val("");
    typeVal = $("#ddlType").val();
    PRAdvSearch();
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    dtMainOnDemand.page('first').draw('page');
});
function PRAdvSearch() {
    var InactiveFlag = false;
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#advsearchfilteritems").html(searchitemhtml);
    $(".filteritemcount").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    if (searchtxtId == "ddlType") {
        typeVal = null;
    }
    PRAdvSearch();
    dtMainOnDemand.page('first').draw('page');
});
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    $("#txtsearchbox").val("");
    clearAdvanceSearch();
    dtMainOnDemand.page('first').draw('page');
});
function clearAdvanceSearch() {
    selectCount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    $('#advsearchsidebar').find("select").val("").trigger('change');
    typeVal = $("#ddlType").val();
    $(".filteritemcount").text(selectCount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
$(document).on('click', '.lnk_onDetails', function (e) {
    e.preventDefault();
    var index_row = $('#maintananceOnDemandSearch tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtMainOnDemand.row(row).data();
    var MasterId = data.MaintOnDemandMasterId;
    $.ajax({
        url: "/MaintenanceOnDemandLibrary/OndemandDetail",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { MaintOnDemandMasterId: MasterId },
        success: function (data) {
            $('#renderOnDemandLibrary').html(data);
        },
        complete: function () {
            CloseLoader();
        }
    });
});
//#endregion
//#region Task
function GenerateTaskGrid() {
    var rCount = 0;
    var showAddBtn = false;
    var showDeleteBtn = false;
    var showEditBtn = false;
    var showDeleteBtn = false;
    var MasterId = $(document).find('#maintenanceOnDemanModel_MaintOnDemandMasterId').val();
    if ($(document).find('#tasksTable').hasClass('dataTable')) {
        dtTaskTable.destroy();
    }
    dtTaskTable = $("#tasksTable").DataTable({
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
            "url": "/MaintenanceOnDemandLibrary/PopulateTasks?MaintOnDemandMasterId=" + MasterId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                showAddBtn = response.showAddBtn;
                showEditBtn = response.showEditBtn;
                showDeleteBtn = response.showDeleteBtn;
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [2], render: function (a, b, data, d) {
                    if (showAddBtn && showEditBtn && showDeleteBtn) {
                        return '<a class="btn btn-outline-primary addtaskBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editTaskBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger deltaskBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else if (showAddBtn && showEditBtn && showDeleteBtn == false) {
                        return '<a class="btn btn-outline-primary addtaskBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editTaskBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                    }
                    else if (showAddBtn && showEditBtn == false && showDeleteBtn == false) {
                        return '<a class="btn btn-outline-primary addtaskBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>';
                    }
                    else if (showAddBtn == false && showEditBtn == false && showDeleteBtn == false) {
                        return '';
                    }
                    else if (showAddBtn == false && showEditBtn == false && showDeleteBtn == true) {
                        return '<a class="btn btn-outline-danger deltaskBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else if (showAddBtn == false && showEditBtn == true && showDeleteBtn == true) {
                        return '<a class="btn btn-outline-success editTaskBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger deltaskBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else if (showAddBtn == true && showEditBtn == false && showDeleteBtn == true) {
                        return '<a class="btn btn-outline-primary addtaskBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-danger deltaskBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else if (showAddBtn == true && showEditBtn == false && showDeleteBtn == false) {
                        return '<a class="btn btn-outline-primary addtaskBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>';
                    }
                    else if (showAddBtn == false && showEditBtn == true && showDeleteBtn == false) {
                        return '<a class="btn btn-outline-success editTaskBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';                            
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
            if (rCount > 0 || showAddBtn == false) {
                $('#btnAddTask').hide();
            }
            else {
                $('#btnAddTask').show();
            }
            if (showAddBtn == false && showEditBtn == false && showDeleteBtn == false) {
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
$(document).on('click', '.deltaskBttn', function () {
    var data = dtTaskTable.row($(this).parents('tr')).data();
    var MasterId = $(document).find('#maintenanceOnDemanModel_MaintOnDemandMasterId').val();
    var MaintOnDemandMasterTaskId = data.MaintOnDemandMasterTaskId;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/MaintenanceOnDemandLibrary/DeleteTasks',
            data: {
                MasterId: MasterId, MasterTaskId: MaintOnDemandMasterTaskId
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
    var MasterId = $(document).find('#maintenanceOnDemanModel_MaintOnDemandMasterId').val();
    var ClientLookUpId = $(document).find('#maintenanceOnDemanModel_ClientLookUpId').val();

    $.ajax({
        url: "/MaintenanceOnDemandLibrary/AddTask",
        type: "GET",
        dataType: 'html',
        data: { MasterId: MasterId, ClientLookUpId: ClientLookUpId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderOnDemandLibrary').html(data);
        },
        complete: function () {
            SetOnDemmandControl();
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
    var MasterId = $(document).find('#taskModel_MaintOnDemandMasterId').val();
    RedirectToSureOncancel(MasterId, "task");
});
function EditTask(data) {
    var MasterId = $(document).find('#maintenanceOnDemanModel_MaintOnDemandMasterId').val();
    var ClientLookUpId = $(document).find('#maintenanceOnDemanModel_ClientLookUpId').val();
    $.ajax({
        url: "/MaintenanceOnDemandLibrary/EditTask",
        type: "GET",
        dataType: 'html',
        data: { MasterId: MasterId, ClientLookUpId: ClientLookUpId, MaintOnDemandMasterTaskId: data.MaintOnDemandMasterTaskId, TaskId: data.TaskId, Description: data.Description },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderOnDemandLibrary').html(data);
        },
        complete: function () {
            SetOnDemmandControl();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function TaskAddOnSuccess(data) {
    CloseLoader();
    var MaintOnDemandMasterId = data.MaintOnDemandMasterId;
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("TaskAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("TaskUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToonDemandDetail(MaintOnDemandMasterId, "task");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region On-Demand Add/Edit
$(document).on('click', ".AddOndemand", function () {
    var ClientLookUpId = $(document).find('#maintenanceOnDemanModel_ClientLookUpId').val();
    var MasterId = $(document).find('#maintenanceOnDemanModel_MaintOnDemandMasterId').val();
    $.ajax({
        url: "/MaintenanceOnDemandLibrary/AddOndemand",
        type: "GET",
        dataType: 'html',
        data: { ClientLookUpId: ClientLookUpId, MasterId: MasterId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderOnDemandLibrary').html(data);
        },
        complete: function () {
            SetOnDemmandControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btnEditOnDemand", function () {
    var MasterId = $(document).find('#maintenanceOnDemanModel_MaintOnDemandMasterId').val();
    var ClientLookUpId = $(document).find('#maintenanceOnDemanModel_ClientLookUpId').val();
    var Description = $(document).find('#maintenanceOnDemanModel_Description').val();
    var Type = $(document).find('#maintenanceOnDemanModel_Type').val();

    $.ajax({
        url: "/MaintenanceOnDemandLibrary/EditOndemand",
        type: "GET",
        dataType: 'html',
        data: { MasterId: MasterId, ClientLookUpId: ClientLookUpId, Description: Description, Type: Type },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderOnDemandLibrary').html(data);
        },
        complete: function () {
            SetOnDemmandControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btnCancelOndemand", function () {
    var MasterId = $(document).find('#maintenanceOnDemanModel_MaintOnDemandMasterId').val();
    if (!MasterId) {
        MasterId = $(document).find('#maintenanceOnDemanModel_MasterIdForCancel').val();
    }
    RedirectToSureOncancel(MasterId, "");
});
function OnDemandAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.Command == "save") {
            if (data.mode == "add") {
                SuccessAlertSetting.text = getResourceValue("MaintenanceOnDemandaddedsuccessfullyAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("MaintenanceOnDemandupdatedsuccessfullyAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToonDemandDetail(data.MaintOnDemandMasterId, "");
            });
        }
        else {
            SuccessAlertSetting.text = getResourceValue("MaintenanceOnDemandaddedsuccessfullyAlert");
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change');
                $(document).find('form').find("#purchaseRequestModel_VendorId").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};
$(document).on('click', "#brdondemand", function () {
    var MasterId = $(this).attr('data-val');
    RedirectToonDemandDetail(MasterId, "");
});
//#endregion
//#region Common
function RedirectToonDemandDetail(MaintOnDemandMasterId, mode) {
    if (MaintOnDemandMasterId == 0) {
        window.location.href = "/MaintenanceOnDemandLibrary/index?page=MaintenanceOnDemand";
    }
    else {
        $.ajax({
            url: "/MaintenanceOnDemandLibrary/OndemandDetail",
            type: "POST",
            dataType: "html",
            data: { MaintOnDemandMasterId: MaintOnDemandMasterId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#renderOnDemandLibrary').html(data);
            },
            complete: function () {
                CloseLoader();
                if (mode === "task") {
                    $('#onDemamdTask').trigger('click');
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
function RedirectToSureOncancel(MasterId, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToonDemandDetail(MasterId, mode);
    });
}
function SetOnDemmandControl() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if (areaddescribedby) {
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
        }
    });
    $(document).find('.select2picker').select2({});
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    });
}
//#endregion
$(document).on('click', '#maintananceOnDemandSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#maintananceOnDemandSearch_length .searchdt-menu', function () {
    run = true;
});
$(document).on('click', '#maintananceOnDemandSearch_wrapper th', function () {
    run = true;
});



//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtMainOnDemand, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(dtMainOnDemand, colOrder);
    run = true;
    dtMainOnDemand.state.save(run);
});
//#endregion