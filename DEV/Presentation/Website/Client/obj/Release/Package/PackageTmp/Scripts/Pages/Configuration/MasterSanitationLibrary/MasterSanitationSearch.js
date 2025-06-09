var dtSaniLibrary;
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
    $("#MasterSanitOnDemandGridAction :input").attr("disabled", "disabled");
    generateSaniLibraryDataTable();
});
$(document).on('click', "#sidebarCollapse", function () {
    $('#renderSaniLibrary').find('#sidebar').addClass('active');
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
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        $(".tabsArea").hide();
        var activeTab = $(this).find("a").attr("href");
        $(activeTab).fadeIn();
        return false;

});
//#region GenerateDT func()
function generateSaniLibraryDataTable() {
    var printCounter = 0;
    if ($(document).find('#saniLibrarySearch').hasClass('dataTable')) {
        dtSaniLibrary.destroy();
    }
    dtSaniLibrary = $("#saniLibrarySearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 0
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
                        GridName: "SanitationMaster_Search",
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
                    GridName: "SanitationMaster_Search",
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
                title: 'Master Sanitation'
            },
            {
                extend: 'print',
                title: 'Master Sanitation'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Master Sanitation',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                css: 'display:none',
                title: 'Master Sanitation',
                orientation: 'landscape',
                pageSize: 'A2'
            }

        ],
        "orderMulti": true,
        "ajax": {
            "url": "/MasterSanitationLibrary/GetSanitationLibraryGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.MasterSanLibraryId = LRTrim($("#txtSanitationMasterID").val());
                d.Description = LRTrim($("#txtDescription").val());
                d.JobDuration = LRTrim($("#txtDuration").val());
                d.FrequencyType = LRTrim($("#ddlFrequencyType").val());
                d.Frequency = LRTrim($("#txtFrequency").val());
                d.CreateDate = LRTrim($("#txtCreated").val());
                d.srcData = LRTrim($("#txtsearchbox").val());
            },
            "dataSrc": function (result) {
                $("#ddlFrequencyType").empty();
                $("#ddlFrequencyType").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < result.frequencyTypeList.length; i++) {
                    var id = result.frequencyTypeList[i];
                    var name = result.frequencyTypeList[i];
                    $("#ddlFrequencyType").append("<option value='" + id + "'>" + name + "</option>");
                }
                if (typeVal) {
                    $("#ddlFrequencyType").val(typeVal);
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
                    return '<a class=lnk_saniLibraryDetails href="javascript:void(0)">' + data + '</a>';
                }
            },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            { "data": "JobDuration", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
            { "data": "FrequencyType", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
            { "data": "Frequency", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
            { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date ", "name": "5" }
        ],
        "columnDefs": [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            $("#MasterSanitOnDemandGridAction :input").removeAttr("disabled");
            $("#MasterSanitOnDemandGridAction :button").removeClass("disabled");
        }
    });
}
$(document).on('click', '#liPrint', function () {
    $(".buttons-print")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liPdf', function () {
    $(".buttons-pdf")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    funcCloseExportbtn();
});
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            dtTable = $("#saniLibrarySearch").DataTable();
            var currestsortedcolumn = $('#saniLibrarySearch').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#saniLibrarySearch').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#saniLibrarySearch').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var jsonResult = $.ajax({
                "url": "/MasterSanitationLibrary/GetMasterSanitationPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    OrderCol: "0",
                    OrderDir: "asc",
                    MasterSanLibraryId: LRTrim($("#txtOnDemandId").val()),
                    Description: LRTrim($("#txtDescription").val()),
                    Type: LRTrim($("#ddlFrequencyType").val()),
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
            var visiblecolumnsIndex = $("#saniLibrarySearch thead tr th").map(function (key) {
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
                header: $("#saniLibrarySearch thead tr th").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
})
//#endregion
//#region Search
//$('#btnDropdownSearch').on('click', function () {
//    clearAdvanceSearch();
//    dtSaniLibrary.page('first').draw('page');
//});
$("#btnPRDataAdvSrch").on('click', function (e) {
    run = true;
    $("#txtsearchbox").val("");
    typeVal = $("#ddlFrequencyType").val();
    PRAdvSearch();
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    dtSaniLibrary.page('first').draw('page');
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
    if (searchtxtId == "ddlFrequencyType") {
        typeVal = null;
    }
    PRAdvSearch();
    dtSaniLibrary.page('first').draw('page');
});
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    $("#txtsearchbox").val("");
    clearAdvanceSearch();
    dtSaniLibrary.page('first').draw('page');
});
function clearAdvanceSearch() {
    selectCount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    $('#advsearchsidebar').find("select").val("").trigger('change');
    typeVal = $("#ddlFrequencyType").val();
    $(".filteritemcount").text(selectCount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
$(document).on('click', '.lnk_saniLibraryDetails', function (e) {
    e.preventDefault();
    var index_row = $('#saniLibrarySearch tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtSaniLibrary.row(row).data();
    var MasterId = data.MasterSanLibraryId;
    $.ajax({
        url: "/MasterSanitationLibrary/SaniLibraryDetail",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { MasterSanLibraryId: MasterId },
        success: function (data) {
            $('#renderSaniLibrary').html(data);
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
    var MasterId = $(document).find('#masterSanitationModel_MasterSanLibraryId').val();
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
            "url": "/MasterSanitationLibrary/PopulateTasks",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.MasterSanLibraryId = MasterId
            },
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
    var MasterId = $(document).find('#masterSanitationModel_MasterSanLibraryId').val();
    var MasterSanLibraryTaskId = data.MasterSanLibraryTaskId;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/MasterSanitationLibrary/DeleteTasks',
            data: {
                MasterId: MasterId, MasterTaskId: MasterSanLibraryTaskId
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
    var MasterId = $(document).find('#masterSanitationModel_MasterSanLibraryId').val();
    var ClientLookUpId = $(document).find('#masterSanitationModel_ClientLookUpId').val();
    $.ajax({
        url: "/MasterSanitationLibrary/AddTask",
        type: "GET",
        dataType: 'html',
        data: { MasterId: MasterId, ClientLookUpId: ClientLookUpId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderSaniLibrary').html(data);
        },
        complete: function () {
            SetMSanitatiomControl();
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
    var MasterId = $(document).find('#taskModel_MasterSanLibraryId').val();
    RedirectToSureOncancel(MasterId, "task");
});
function EditTask(data) {
    var MasterId = $(document).find('#masterSanitationModel_MasterSanLibraryId').val();
    var ClientLookUpId = $(document).find('#masterSanitationModel_ClientLookUpId').val();
    $.ajax({
        url: "/MasterSanitationLibrary/EditTask",
        type: "GET",
        dataType: 'html',
        data: { MasterId: MasterId, ClientLookUpId: ClientLookUpId, MasterSanLibraryTaskId: data.MasterSanLibraryTaskId, TaskId: data.TaskId, Description: data.Description },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderSaniLibrary').html(data);
        },
        complete: function () {
            SetMSanitatiomControl();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function TaskAddOnSuccess(data) {
    CloseLoader();
    var MasterSanLibraryId = data.MasterSanLibraryTaskId;
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("TaskAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("TaskUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToMSanitationDetail(MasterSanLibraryId, "task");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region On-Demand Add/Edit
$(document).on('click', ".AddSanitationMaster", function () {
    var ClientLookUpId = $(document).find('#maintenanceOnDemanModel_ClientLookUpId').val();
    var MasterId = $(document).find('#maintenanceOnDemanModel_MasterSanLibraryId').val();
    $.ajax({
        url: "/MasterSanitationLibrary/AddSanitationMaster",
        type: "GET",
        dataType: 'html',
        data: { ClientLookUpId: ClientLookUpId, MasterId: MasterId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderSaniLibrary').html(data);
        },
        complete: function () {
            SetMSanitatiomControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btneditSanitationMaster", function () {

    var MasterId = $(document).find('#masterSanitationModel_MasterSanLibraryId').val();
    var ClientLookUpId = $(document).find('#masterSanitationModel_ClientLookUpId').val();
    var Description = $(document).find('#masterSanitationModel_Description').val();
    var InactiveFlag = $(document).find('#masterSanitationModel_InactiveFlag').val();
    var FrequencyType = $(document).find('#masterSanitationModel_FrequencyType').val();
    var Frequency = $(document).find('#masterSanitationModel_Frequency').val();
    var ScheduleType = $(document).find('#masterSanitationModel_ScheduleType').val();
    var ScheduleMethod = $(document).find('#masterSanitationModel_ScheduleMethod').val();
    $.ajax({
        url: "/MasterSanitationLibrary/EditSanitationMaster",
        type: "GET",
        dataType: 'html',
        data: { MasterId: MasterId, ClientLookUpId: ClientLookUpId, Description: Description, InactiveFlag: InactiveFlag, FrequencyType: FrequencyType, Frequency: Frequency, ScheduleType: ScheduleType, ScheduleMethod: ScheduleMethod },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderSaniLibrary').html(data);
        },
        complete: function () {
            SetMSanitatiomControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btnCancelMSanitation", function () {
    var MasterId = $(document).find('#masterSanitationModel_MasterSanLibraryId').val();
    if (!MasterId) {
        MasterId = $(document).find('#masterSanitationModel_MasterIdForCancel').val();
    }
    RedirectToSureOncancel(MasterId, "");
});
function OnMasterSanitationAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.Command == "save") {
            if (data.mode == "add") {
                SuccessAlertSetting.text = getResourceValue("MasterSanitationaddedsuccessfullyAlert"); 
            }
            else {
                SuccessAlertSetting.text = getResourceValue("MasterSanitationupdatedsuccessfullyAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToMSanitationDetail(data.MasterSanLibraryId, "");
            });
        }
        else {
            message = getResourceValue("MasterSanitationaddedsuccessfullyAlert");
            SuccessAlertSetting.text = getResourceValue("MasterSanitationaddedsuccessfullyAlert");
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
    RedirectToMSanitationDetail(MasterId);
   
});
//#endregion
//#region Common
function RedirectToMSanitationDetail(MasterSanLibraryId, mode) {
    if (MasterSanLibraryId == 0) {
        window.location.href = "/MasterSanitationLibrary/index?page=Master_Sanitation";
    }
    else {
        $.ajax({
            url: "/MasterSanitationLibrary/SaniLibraryDetail",
            type: "POST",
            dataType: "html",
            data: { MasterSanLibraryId: MasterSanLibraryId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#renderSaniLibrary').html(data);
            },
            complete: function () {
                CloseLoader();
                if (mode === "task") {
                    $('#onDemamdTask').trigger('click');
                    $('#colorselector').val('Tasks');
                }
                SetMSanitatiomControl();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
}
function RedirectToSureOncancel(MasterId, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToMSanitationDetail(MasterId, mode);
    });
}
function SetMSanitatiomControl() {
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

$(document).on('click', '#saniLibrarySearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#saniLibrarySearch_length .searchdt-menu', function () {
    run = true;
});

$(document).on('click', '#saniLibrarySearch_wrapper th', function () {
    run = true;
});



//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtSaniLibrary, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(dtSaniLibrary, colOrder);
    run = true;
    dtSaniLibrary.state.save(run);
});
//#endregion