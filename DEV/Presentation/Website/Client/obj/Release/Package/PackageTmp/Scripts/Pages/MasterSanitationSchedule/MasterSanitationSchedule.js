var run = false;
var equipmentid = -1;
$(function () {
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
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
    generateMsScheduleTable();
    $(document).find('.select2picker').select2({});
});
$(document).on('click', "#sidebarCollapse", function () {
    $(document).find('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});

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
$(document).on('click', '#drpDwnLink', function (e) {
    e.preventDefault();
    $(document).find("#drpDwn").slideToggle();
});

function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "SOTasks":
            generateMsTaskGrid(0);
            break;
        case "SONotes":
            GenerateMsNotesGrid();
            break;
        case "SOAttachments":
            generateMsAttachmentsGrid();
            break;
        case "SOTools":
            generateMsToolsGrid();
            break;
        case "SOChemicals":
            generateMsChemicalGrid();
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
function SetControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $(document).find('#MasterSanitModel_Description').change(function () {
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
};
//#region Master Sanitation Search
var dtTable;
$(document).on('click', '#liPdf', function () {
    $(".buttons-pdf")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', '#liPrint', function () {
    $(".buttons-print")[0].click();
    $('#mask').trigger('click');
});
function generateMsScheduleTable() {
    var printCounter = 0;
    if ($(document).find('#masterSanitScheduleSearch').hasClass('dataTable')) {
        dtTable.destroy();
    }
    dtTable = $("#masterSanitScheduleSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 1
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
            if (run == true) {
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "SanitationScheduleMaster_Search",
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
                "url": gridStateLoadUrl,
                "data": {
                    GridName: "SanitationScheduleMaster_Search",
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
            //return o;
        },
        scrollX: true,
        fixedColumns: {
            leftColumns: 1,
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'MasterSanitation List'
            },
            {
                extend: 'print',
                title: 'MasterSanitation List'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'MasterSanitation List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'MasterSanitation List',
                orientation: 'landscape',
                pageSize: 'A3'
            }

        ],
        "orderMulti": true,
        "ajax": {
            "url": "/MasterSanitationSchedule/GetMasterSanitGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {               
                d.Description = LRTrim($("#txtDescription").val());
                d.ChargeToClientLookupId = LRTrim($("#txtEquipment").val());
                d.Frequency = LRTrim($("#txtFrequency").val());
                d.Assigned = LRTrim($('#txtAssigned').val());
                d.Shift = LRTrim($("#txtShift").val());
                d.ScheduledDuration = LRTrim($("#txtDuration").val());
                d.NextDue = ValidateDate($("#txtNextDue").val());
                d.InactiveFlag = LRTrim($("#isInactive").val());
                d.SearchText = LRTrim($("#txtsearchbox").val());
            },
            "dataSrc": function (result) { 
                if (result.data.length < 1) {
                    $(document).find('.import-export').prop('disabled', true);
                }
                else {
                    $(document).find('.import-export').prop('disabled', false);
                }
                return result.data;
            },
            global: true
        },
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        "columns":
        [
            {
                "data": "Description",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "className": "text-left",
                "name": "0",
                "mRender": function (data, type, row) {
                    return '<a class=lnk_psearch href="javascript:void(0)">' + data + '</a>';
                }
            },
            {
                "data": "ChargeToClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true, "name": "1",
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-400'>" + data + "</div>";
                }
            },
            { "data": "Frequency", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
            { "data": "Assigned", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
            { "data": "Shift", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4"},
            { "data": "ScheduledDuration", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
            { "data": "NextDue", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date ", "name": "6" },
            {
                "data": "InactiveFlag", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center", "name": "7", 
                "mRender": function (data, type, row) {
                    if (data == false) {
                        return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" class="status"><span></span></label>';
                    }
                    else {
                        return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" checked="checked" class="status"><span></span></label>';
                    }
                }
            }            
        ],
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            $(document).on('click', '.status', function (e) {
                e.preventDefault();
            });
            SetPageLengthMenu();
            $("#MasterSanitationGridAction :input").removeAttr("disabled");
            $("#MasterSanitationGridAction :button").removeClass("disabled");
            DisableExportButton($("#masterSanitScheduleSearch"), $(document).find('.import-export'));
        }
    });
}
$('#btnTextSearch').on('click', function () {
    clearAdvanceSearch();
    dtTable.page('first').draw('page');
});
//#region Adv Search
var hGridfilteritemcount = 0;
$(document).on('click', "#btnMsAdvSrch", function (e) {
    var searchitemhtml = "";
    hGridfilteritemcount = 0;
    $('#txtsearchbox').val('');
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val() && $(this).val() != "0") {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossMs" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('#sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    GridAdvanceSearch();
    dtTable.page('first').draw('page');
});
function GridAdvanceSearch() {
    $('.filteritemcount').text(hGridfilteritemcount);
}
$(document).on('click', '.btnCrossMs', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    GridAdvanceSearch();
    dtTable.page('first').draw('page');
});
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    $("#txtsearchbox").val("");
    clearAdvanceSearch();
    dtTable.page('first').draw('page');
});
function clearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    $("#isInactive").val("").trigger('change');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
//#endregion Adv search
//#region Print
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var description = LRTrim($("#txtDescription").val());
            var chargeToClientLookupId = LRTrim($("#txtEquipment").val());
            var frequency = LRTrim($("#txtFrequency").val());
            var assigned = LRTrim($('#txtAssigned').val());
            var shift = LRTrim($("#txtShift").val());
            var scheduledDuration = LRTrim($("#txtDuration").val());
            var nextDue = ValidateDate($("#txtNextDue").val());
            var inactiveFlag = LRTrim($("#isInactive").val());
            var searchText = LRTrim($("#txtsearchbox").val());
            dtTable = $("#masterSanitScheduleSearch").DataTable();
            var currestsortedcolumn = $('#masterSanitScheduleSearch').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#masterSanitScheduleSearch').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#masterSanitScheduleSearch').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            
            var jsonResult = $.ajax({
                "url": "/MasterSanitationSchedule/GetSanitMasterPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    Description: description,
                    ChargeToClientLookupId: chargeToClientLookupId,
                    Frequency: frequency,
                    Assigned: assigned,
                    Shift: shift,
                    ScheduledDuration: scheduledDuration,
                    NextDue: nextDue,
                    InactiveFlag: inactiveFlag,
                    SearchText: searchText,
                    colname: colname,
                    coldir:coldir
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#masterSanitScheduleSearch thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.Description != null) {
                    item.Description = item.Description;
                }
                else {
                    item.Description = "";
                }
                if (item.ChargeToClientLookupId != null) {
                    item.ChargeToClientLookupId = item.ChargeToClientLookupId;
                }
                else {
                    item.ChargeToClientLookupId = "";
                }
                if (item.Frequency != null) {
                    item.Frequency = item.Frequency;
                }
                else {
                    item.Frequency = "";
                }
                if (item.Assigned != null) {
                    item.Assigned = item.Assigned;
                }
                else {
                    item.Assigned = "";
                }
                if (item.Shift != null) {
                    item.Shift = item.Shift;
                }
                else {
                    item.Shift = "";
                }
                if (item.ScheduledDuration != null) {
                    item.ScheduledDuration = item.ScheduledDuration;
                }
                else {
                    item.ScheduledDuration = "";
                }
                if (item.NextDue != null) {
                    item.NextDue = item.NextDue;
                }
                else {
                    item.NextDue = "";
                }
                if (item.InactiveFlag != null) {
                    item.InactiveFlag = item.InactiveFlag;
                }
                else {
                    item.InactiveFlag = "";
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
                header: $("#masterSanitScheduleSearch thead tr th div").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
})

//#endregion Print
//#endregion Master Sanitation Search
//#region Details
$(document).on('click', '.lnk_psearch', function (e) {
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();
    $.ajax({
        url: "/MasterSanitationSchedule/GetMasterSanitDetails",
        type: "POST",
        data: { SanitationMasterId: data.SanitationMasterId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermasterschedule').html(data);
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '#brdMsEdit', function () {
    var msid = $(this).attr('data-val');
    RedirectToMSDetail(msid);
});
//#endregion Details
//#region Add-Edit
$(document).on('click', ".AddMasterSchedule", function () {   
    $.ajax({
        url: "/MasterSanitationSchedule/AddMSSchedule",
        type: "GET",
        dataType: 'html',      
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermasterschedule').html(data);
            //#region for removeclass
            if ($("#AssetTree").val() == "False") {
                $('ul li span').removeClass('wthAdjstNew');
            }
        },
        complete: function () {           
            SetMSSControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
var PlantLocationId = -1;
$(document).on('click', '.txtTreeView', function () {
    $(this).blur();
    if ($(document).find('#MasterSanitationPlantLocation').val() == 'True') {
        generateSanitationPlantLocationTree(-1);
    }
    else {
        generateTree(-1);
    }
});
$(document).on('click', '#pldArray', function (e) {
    $(this).blur();
    //if ($(document).find('#MasterSanitationPlantLocation').val() == 'True') {
    //    generateSanitationPlantLocationTree(-1);
    //}
    //else {
    //    generateTree(-1);
    //}
    generateSanitationAssetTree();
});

function generateSanitationPlantLocationTree(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/SanitationPlantLocationTreeLookup',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        cache: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find(".cntTree").html(data);
        },
        complete: function () {
            CloseLoader();
            $('#masterSanitationTreeModal').modal('show');
            treeTable($(document).find('#tblTree'));
            $(document).find('.radSelectSanitPl').each(function () {
                if ($(this).data('equipmentid') === equipmentid)
                    $(this).attr('checked', true);
            });
            // looking for the collapse icon and triggered click to collapse
            $.each($(document).find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,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"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element
        },
        error: function (xhr) {
            alert('error');
        }
    });
}
$(document).on('change', '.radSelectSanitPl', function () { 
    equipmentid = $(this).data('equipmentid');
    var Description = $(this).data('description');
    var ChargeType = 'Equipment';
    $(document).find('#TplantLocationId').val(equipmentid);
    $(document).find('#MasterSanitModel_ChargeToDescription').val(Description);
    $(document).find('#MasterSanitModel_ChargeToClientLookupId').val(Description);
    $(document).find('#MasterSanitModel_ChargeType').val(ChargeType);
    $(document).find('#MasterSanitModel_PlantLocationId').val(equipmentid);
    var tlen = $(document).find("#MasterSanitModel_ChargeToDescription").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#MasterSanitModel_ChargeToDescription").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#MasterSanitModel_ChargeToDescription").removeClass("input-validation-error");
    }
    else {
        var areaddescribedby = $(document).find("#MasterSanitModel_ChargeToDescription").attr('aria-describedby');
        $('#' + areaddescribedby).show();
        $(document).find('form').find("#MasterSanitModel_ChargeToDescription").addClass("input-validation-error");
    }
    $('#masterSanitationTreeModal').modal('hide');

});

function generateTree(paramVal) {  
    
    $.ajax({
        url: '/PlantLocationTree/PlantLocationClientLookUpEquipmentTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        cache: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find(".cntTree").html(data);
        }
        ,
        complete: function () {
            CloseLoader();
            treeTable($(document).find('#tblTree'));
            $(document).find('.radSelect').each(function () {
                if ($(this).data('plantlocationid') === PlantLocationId)
                    $(this).attr('checked', true);
            });
            $('#masterSanitationTreeModal').modal('show');
            // looking for the collapse icon and triggered click to collapse
            $.each($(document).find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,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"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element
            CloseLoader();
        },
        error: function (xhr) {
            alert('error');
        }
    });
}
$(document).ready(function () {
    $(".actionBar").fadeIn();
    $("#MasterSanitationGridAction :input").attr("disabled", "disabled");
});
$(document).on('change', '.radSelect', function () {   
    PlantLocationId = $(this).data('plantlocationid');
    var Description = $(this).data('description');
    var ChargeType = $(this).data('chargetype');
    var ClientLookUpId = $(this).data('clientlookupid'); 
    $(document).find('#TplantLocationId').val(PlantLocationId);
    $(document).find('#MasterSanitModel_ChargeToDescription').val(ClientLookUpId);
    $(document).find('#MasterSanitModel_ChargeToClientLookupId').val(ClientLookUpId);
    $(document).find('#MasterSanitModel_ChargeType').val(ChargeType);
    $(document).find('#MasterSanitModel_PlantLocationId').val(PlantLocationId);
    var tlen = $(document).find("#MasterSanitModel_ChargeToDescription").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#MasterSanitModel_ChargeToDescription").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#MasterSanitModel_ChargeToDescription").removeClass("input-validation-error");
    }
    else {
        var areaddescribedby = $(document).find("#MasterSanitModel_ChargeToDescription").attr('aria-describedby');
        $('#' + areaddescribedby).show();
        $(document).find('form').find("#MasterSanitModel_ChargeToDescription").addClass("input-validation-error");
    }
    $('#masterSanitationTreeModal').modal('hide');
});
function MSScheduleAddEditOnSuccess(data) {
   
    CloseLoader();
    var SanitationMasterId = data.SanitationMasterId;
    if (data.Result == "success") {
        ResetErrorDiv();
        if (data.Command == "save") {
           
            if (data.mode == "add") {
                SuccessAlertSetting.text = getResourceValue("MasterSanitationScheduleaddAlert") ;
            }
            else {
                SuccessAlertSetting.text = getResourceValue("MasterSanitationScheduleupdateAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToMSDetail(SanitationMasterId, "overview");
            });
        }
        else
        {           
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").not("#colorselector").val("").trigger('change');
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
$(document).on('click', "#btnCancelAddMSSchedule", function () {
    var msid = $(document).find('#MasterSanitModel_SanitationMasterId').val();
    if (typeof msid != "undefined" && msid != 0) {
        RedirectToMSDetailOnCancel(msid);
    }
    else {
        swal(CancelAlertSetting, function () {
            window.location.href = "../MasterSanitationSchedule/Index";

        });
    }
});
$(document).on('click', "#EditMSSchedule", function (e) {    
    e.preventDefault();
    var SanitationMasterId = $(document).find('#MasterSanitModel_SanitationMasterId').val();
    $.ajax({
        url: "/MasterSanitationSchedule/EditMSSchedule",
        type: "GET",
        dataType: 'html',
        data: { SanitationMasterId: SanitationMasterId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermasterschedule').html(data);
            //#region for removeclass
            if ($("#AssetTree").val() == "False") {
                $('ul li span').removeClass('wthAdjstNew');
            }
        },
        complete: function () {
            $(document).find(".multi-select2picker").select2({

            });
            CloseLoader();
            SetMSSControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('change', '#ScheduledType', function () {
    var ddlVal = $(this).val();
    if (ddlVal =="OnDemand")
    {
        $(document).find('#txtOnDemand').text('*');
    }
    else {
        $(document).find('#txtOnDemand').text('');
    }
});
//#endregion Add-Edit

//#region common
function SetMSSControls() {    
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
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
    $(document).find('.select2picker').select2({});
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
}
function RedirectToMSDetailOnCancel(msId, mode)
{
    swal(CancelAlertSetting, function () {
        RedirectToMSDetail(msId, mode);
    });
}
function RedirectToMSDetail(msId, mode) {
    $.ajax({
        url: "/MasterSanitationSchedule/GetMasterSanitDetails",
        type: "POST",
        dataType: 'html',
        data: { SanitationMasterId: msId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermasterschedule').html(data);
        },
        complete: function () {
            CloseLoader();
            if (mode === "SOTasks") {
                $(document).find("#drpDwn").slideToggle();
                $(document).find('#sotask').trigger('click');
            }
            if (mode === "overview") {
                $(document).find('#SanitationOverview').trigger('click');
               
            }
            if (mode === "SOTools") {
                $(document).find("#drpDwn").slideToggle();
                $(document).find('#sotools').trigger('click');
            }
            if (mode === "SONotes") {
                $(document).find("#drpDwn").slideToggle();
                $(document).find('#sonotes').trigger('click');
            }
            if (mode === "SOAttachments") {
                $(document).find("#drpDwn").slideToggle();
                $(document).find('#soattachments').trigger('click');
            }
            if (mode === "SOChemicals") {
                $(document).find("#drpDwn").slideToggle();
                $(document).find('#sochemicals').trigger('click');
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion


//#region Options
$(document).on('click', '#DeleteMSSchedule', function () {   
    var MSScheduleid = $(this).attr('data-id');
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/MasterSanitationSchedule/DeleteMSSchedule',
            data: {
                MSScheduleid: MSScheduleid
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "POST",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    SuccessAlertSetting.text = getResourceValue("MasterSanitationScheduledeleteAlert");
                    swal(SuccessAlertSetting, function () {
                        window.location.href = "../MasterSanitationSchedule/Index";
                    });
                }
                else {
                    GenericSweetAlertMethod(data);
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
});
//#endregion

$(document).on('click', '#masterSanitScheduleSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#masterSanitScheduleSearch_length .searchdt-menu', function () {
    run = true;
});
$(document).on('click', '#masterSanitScheduleSearch_wrapper th', function () {
    run = true;
});
//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtTable,true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(dtTable, colOrder);
    run = true;
    dtTable.state.save(run);
});
//#endregion

$(document).on('click', '#mssmoreaddescription', function () {
    $(document).find('#mssdetaildesmodaltext').text($(this).data("des"));
    $(document).find('#mssdetaildesmodal').modal('show');
});


//#region Sanitation AssetTree
function generateSanitationAssetTree(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/SanitationEquipmentHierarchyTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        cache: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find(".cntTree").html(data);
        }
        ,
        complete: function () {
            CloseLoader();
            treeTable($(document).find('#tblTree'));

            //------------------testing----------------
            $(document).find('.radSelectSanitation').each(function () {
                if ($(document).find('#hdnId').val() == '0' || $(document).find('#hdnId').val() == '') {

                    if ($(this).data('equipmentid') === equipmentid) {
                        $(this).attr('checked', true);
                    }

                }
                else {

                    if ($(this).data('equipmentid') == $(document).find('#hdnId').val()) {
                        $(this).attr('checked', true);
                    }

                }

            });
            $('#masterSanitationTreeModal').modal('show');
            //---------------------------------------------------
            // looking for the collapse icon and triggered click to collapse
            $.each($(document).find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,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"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element
            CloseLoader();
        },
        error: function (xhr) {
            alert('error');
        }
    });
}


$(document).on('change', ".radSelectSanitation", function () {
    $(document).find('#hdnId').val('0');
    equipmentid = $(this).data('equipmentid');
    var clientlookupid = $(this).data('clientlookupid');
    var chargetoname = $(this).data('itemname');
    chargetoname = chargetoname.substring(0, chargetoname.length - 1);
    $(document).find("#MasterSanitModel_ChargeType").val("Equipment");
    $(document).find('#MasterSanitModel_ChargeToDescription').val(clientlookupid).removeClass('input-validation-error').trigger('change');
    $(document).find('#MasterSanitModel_ChargeToClientLookupId').val(clientlookupid);
    if ($(document).find('#MasterSanitModel_ChargeToName').length > 0)
    {
        $(document).find('#MasterSanitModel_ChargeToName').val(chargetoname);
    }
    $('#masterSanitationTreeModal').modal('hide');
});
//#endregion