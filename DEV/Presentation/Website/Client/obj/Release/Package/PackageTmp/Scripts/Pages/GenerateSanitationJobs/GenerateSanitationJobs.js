var generateSanitationJobsDt;
var run = false;
var selectedcount = 0;
var totalcount = 0;
var SaniBatchEntryToupdate = [];
var SaniBatchEntrySelectedItemArray = [];
var searchcount = 0;
var searchresult = [];
var select = 0;
var gridname = "GenerateSanitationJobs_Search";
var SaniGenOder = 1;
var SaniGenDir = 'asc';
var SaniGenAllowedPrintNumber = 50;
var ReturnSanMasterBatchHeaderId = 0;
$(function () {
    $(document).find('.select2picker').select2({});
    $(document).find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '.dismiss, .overlay', function () {
        $('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', '#SaniJobGensidebarCollapse', function () {
        $('.sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    $(document).find('.dtpicker1').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true,
        minDate: 1
    }).inputmask('mm/dd/yyyy');
    $(document).find('.mulselectclass').show();
    /*GetSanitationGeneratedList();*/
});
$.validator.setDefaults({ ignore: null });
$('input, form').blur(function () {
    if ($(this).closest('form').length > 0) {
        $(this).valid();
    }
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
//#region Sani Generated Add 
$(document).on('change', '#ddlscheduletype', function () {
    var type = $(this).val();
    if (type == "OnDemand") {
        $(document).find('#divOnDemand').show();
    }
    else {
        $(document).find('#divOnDemand').hide();
    }
    if (type) {
        $(this).removeClass("input-validation-error");
    }

});
$(document).on('change', '#dtgeneratedthrough', function () {
    var generatedthroughid = $(this).val();
    if (generatedthroughid) {
        $(this).removeClass("input-validation-error");
    }

});
function CreateSanitationJobsGenerationOnSuccess(data) {
    CloseLoader();
    if (data.Previewval) {
        if (data.Result == "success") {
            clearAdvanceSearch();
            SaniBatchEntryToupdate = [];
            SaniBatchEntrySelectedItemArray = [];
            $(document).find('#GenSani-select-all').prop('checked', false);
            ReturnSanMasterBatchHeaderId = 0;
                GetSanitationGeneratedList();
        }
        else {
            ResetErrorDiv();
            ShowGenericErrorOnAddUpdate(data.Msg);
        }

    }
    else {
        if (data.Msg > 0) {
            CloseLoader();
            ResetErrorDiv();
            var rowCount = data.Msg;
            var prnChkIdval = $(document).find('#prnChkId').is(':checked');
           
           SuccessAlertSetting.text = getResourceValue("alertMasterSanitationJobGenerationCompleted") + ' ' + rowCount;
            swal(SuccessAlertSetting, function () {
                if (data.SanitationjobList.length > 0 && prnChkIdval == true) {
                    ShowLoader();
                    $.ajax({
                        url: '/MasterSanitationSchedule/PrintSaniList',
                        data: {
                            listOfSanitation: data.SanitationjobList
                        },
                        type: "POST",
                        datatype: "json",
                        responseType: 'arraybuffer',
                        success: function (result) {
                            if (result.success) {
                                PdfPrintAllSanitationList(result.pdf);
                            }
                        },
                        complete: function () {
                            $(document).find('#prnChkId').prop('checked', false);
                            ResetErrorDiv();
                            $(document).find('#ddlscheduletype').val("").trigger('change');
                            $(document).find('#dtgeneratedthrough').val("");
                            $(document).find('#ondemandgroup').val("").trigger('change');
                            $("#ddlAssetGroup1 option[value]").remove();
                            $("#ddlAssetGroup2 option[value]").remove();
                            $("#ddlAssetGroup3 option[value]").remove();
                            CloseLoader();
                        }
                    });

                }
            });
        }
        else {
            ResetErrorDiv();
            ShowGenericErrorOnAddUpdate(data.Msg);
        }
    }
}
//#endregion
function GetSanitationGeneratedList() {
    var ScheduleType = $(document).find("#ddlscheduletype").val();
    var GeneratedThroughDate = ValidateDate($(document).find("#dtgeneratedthrough").val());
    var OnDemandGroup = $(document).find("#ondemandgroup").val();
    var AssetGroup1Ids = $(document).find('#ddlAssetGroup1').val();
    var AssetGroup2Ids = $(document).find("#ddlAssetGroup2").val();
    var AssetGroup3Ids = $(document).find("#ddlAssetGroup3").val();
    var SanMasterGenDueDate = ValidateDate($(document).find("#SanGenDueDate").val());
    var EquipmentClientLookupId = LRTrim($(document).find("#SanGEquipmentClientLookupId").val());
    var EquipmentName = LRTrim($(document).find("#SanGEquipmentName").val());
    var SanMasterGenShift = LRTrim($(document).find("#SanGenShift").val());
    var SanMasterGenDesc = LRTrim($(document).find("#SanGenDesc").val());

    $('#GenWOcontainer').show();
    if ($(document).find('#tblGenerateSanitationJobsGrid').hasClass('dataTable')) {
        generateSanitationJobsDt.destroy();
    }
    generateSanitationJobsDt = $("#tblGenerateSanitationJobsGrid").DataTable({
        colReorder: {
            fixedColumnsLeft: 2
        },
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        "stateSave": true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = SaniGenOder;
                    data.order[0][1] = SaniGenDir;
                }
                //Search Retention
                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: gridname,
                        LayOutInfo: JSON.stringify(data),
                        FilterInfo: ''
                    },
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
                //Search Retention
            }
            run = false;
        },
        "stateLoadCallback": function (settings, callback) {

            //Search Retention
            $.ajax({
                "url": "/Base/GetLayout",
                "data": {
                    GridName: gridname
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {

                    if (json.LayoutInfo !== '') {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        LayoutInfo.start = 0;
                        SaniGenOder = LayoutInfo.order[0][0];
                        SaniGenDir = LayoutInfo.order[0][1];
                        callback(LayoutInfo);
                    }
                    else {
                        callback(json.LayoutInfo);
                    }
                }
            });
            //Search Retention
        },
        scrollX: true,
        //fixedColumns: {
        //    leftColumns: 2
        //},
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Generate Sanitation Jobs List'
            },
            {
                extend: 'print',
                title: 'Generate Sanitation Jobs List'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Generate Sanitation Jobs List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Generate Sanitation Jobs List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            url: "/GenerateSanitationJobs/GetSanitationJobGenGrid",
            "type": "post",
            "datatype": "json",
            cache: false,
            data: function (d) {
                d.ScheduleType = ScheduleType;
                d.GeneratedThroughDate = GeneratedThroughDate;
                d.OnDemandGroup = OnDemandGroup;
                d.AssetGroup1Ids = AssetGroup1Ids;
                d.AssetGroup2Ids = AssetGroup2Ids;
                d.AssetGroup3Ids = AssetGroup3Ids;
                d.SanMasterGenDueDate = SanMasterGenDueDate;
                d.EquipmentClientLookupId = EquipmentClientLookupId;
                d.EquipmentName = EquipmentName;
                d.SanMasterGenShift = SanMasterGenShift;
                d.SanMasterGenDesc = SanMasterGenDesc;
                d.ReturnSanMasterBatchHeaderId = ReturnSanMasterBatchHeaderId;
            },
            "dataSrc": function (result) {
                if (result.data.length < 1) {
                    $(document).find('#btnGenerateSanitationExport,#GenSani-select-all').prop('disabled', true);
                }
                else {
                    $(document).find('#btnGenerateSanitationExport,#GenWO-select-alll').prop('disabled', false);
                }
                let SaniGenOder = generateSanitationJobsDt.order();
                SaniGenDir = SaniGenOder[0][1];
                totalcount = result.recordsTotal;
                ReturnSanMasterBatchHeaderId = result.ReturnSanMasterBatchHeaderId;
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "SanMasterBatchEntryId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    'render': function (data, type, full, meta) {
                        if ($('#GenSani-select-all').is(':checked') && totalcount == selectedcount) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-sanMasterBatchEntryId="' + data + '" class="chkGenWOsearch ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        } else {
                            if (SaniBatchEntryToupdate.indexOf(data) != -1) {
                                return '<input type="checkbox" checked="checked" name="id[]" data-sanMasterBatchEntryId="' + data + '" class="chkGenWOsearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                            else {
                                return '<input type="checkbox" name="id[]" data-sanMasterBatchEntryId="' + data + '" class="chkGenWOsearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                        }
                    }
                },
                { "data": "DueDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
                { "data": "EquipmentClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "EquipmentName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                { "data": "Shift", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" }

            ],
        columnDefs: [
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            DisableExportButton($("#tblGenerateSanitationJobsGrid"), $(document).find('#btnGenerateSanitationExport,#GenSani-select-all,#prnChkBEId,#btnCreateSanitationJobGenPreview'));
        }
    });
}

//#region export

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
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var ScheduleType = $(document).find("#ddlscheduletype").val();
            var GeneratedThroughDate = ValidateDate($(document).find("#dtgeneratedthrough").val());
            var OnDemandGroup = $(document).find("#ondemandgroup").val();
            var AssetGroup1Ids = $(document).find('#ddlAssetGroup1').val();
            var AssetGroup2Ids = $(document).find("#ddlAssetGroup2").val();
            var AssetGroup3Ids = $(document).find("#ddlAssetGroup3").val();
            var SanMasterGenDueDate = ValidateDate($(document).find("#SanGenDueDate").val());
            var EquipmentClientLookupId = LRTrim($(document).find("#SanGEquipmentClientLookupId").val());
            var EquipmentName = LRTrim($(document).find("#SanGEquipmentName").val());
            var SanMasterGenShift = LRTrim($(document).find("#SanGenShift").val());
            var SanMasterGenDesc = LRTrim($(document).find("#SanGenDesc").val());
            var colname = SaniGenOder;
            var coldir = SaniGenDir;
            var jsonResult = $.ajax({
                "url": "/GenerateSanitationJobs/GetSanitationJobGenGridPrintData",
                "type": "POST",
                "datatype": "json",
                data: {
                    colname: colname,
                    coldir: coldir,
                    ScheduleType: ScheduleType,
                    GeneratedThroughDate: GeneratedThroughDate,
                    OnDemandGroup: OnDemandGroup,
                    AssetGroup1Ids: AssetGroup1Ids,
                    AssetGroup2Ids: AssetGroup2Ids,
                    AssetGroup3Ids: AssetGroup3Ids,
                    SanMasterGenDueDate: SanMasterGenDueDate,
                    EquipmentClientLookupId: EquipmentClientLookupId,
                    EquipmentName: EquipmentName,
                    SanMasterGenShift: SanMasterGenShift,
                    SanMasterGenDesc: SanMasterGenDesc,
                    ReturnSanMasterBatchHeaderId : ReturnSanMasterBatchHeaderId
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#tblGenerateSanitationJobsGrid thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.DueDate != null) {
                    item.DueDate = item.DueDate;
                }
                else {
                    item.DueDate = "";
                }
                if (item.EquipmentClientLookupId != null) {
                    item.EquipmentClientLookupId = item.EquipmentClientLookupId;
                }
                else {
                    item.EquipmentClientLookupId = "";
                }
                if (item.EquipmentName != null) {
                    item.EquipmentName = item.EquipmentName;
                }
                else {
                    item.EquipmentName = "";
                }
                if (item.Shift != null) {
                    item.Shift = item.Shift;
                }
                else {
                    item.Shift = "";
                }
                if (item.Description != null) {
                    item.Description = item.Description;
                }
                else {
                    item.Description = "";
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
                header: $("#tblGenerateSanitationJobsGrid thead tr th").not(":eq(0)").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});
//#endregion
//#region Advance Search
$(document).on('click', "#btnSanGeneratedDataAdvSrch", function (e) {
    generateSanitationJobsDt.state.clear();
    var searchitemhtml = "";
    hGridfilteritemcount = 0
    $('#advsearchsidebarSanGenerated').find('.adv-item').each(function (index, item) {
        if ($(this).val() && $(this).val() != "0") {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossHistory" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('.sidebar').removeClass('active');
    $('#SaniJobGenerateadvsearchcontainer').find('#sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    GridAdvanceSearch();
});
function GridAdvanceSearch() {
    GetSanitationGeneratedList();
    $('.filteritemcount').text(hGridfilteritemcount);
}
$(document).on('click', '.btnCrossHistory', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    GridAdvanceSearch();
});
$(document).on('click', '#liClearAdvSearchFilterAVPMG', function () {
    clearAdvanceSearch();
    GetSanitationGeneratedList();
});
function clearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebarSanGenerated').find('input:text').val('');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
//#endregion


//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(generateSanitationJobsDt, true);
});
$(document).on('click', '.saveConfig', function () {

    var colOrder = [0, 1];
    funCustozeSaveBtn(generateSanitationJobsDt, colOrder);
    run = true;
    generateSanitationJobsDt.state.save(run);
});


//#endregion

//#region PM Generated Batch Entry Search

$(document).on('click', '#GenSani-select-all', function (e) {
    SaniBatchEntryToupdate = [];
    SaniBatchEntrySelectedItemArray = [];

    var ScheduleType = $(document).find("#ddlscheduletype").val();
    var GeneratedThroughDate = ValidateDate($(document).find("#dtgeneratedthrough").val());
    var OnDemandGroup = $(document).find("#ondemandgroup").val();
    var AssetGroup1Ids = $(document).find('#ddlAssetGroup1').val();
    var AssetGroup2Ids = $(document).find("#ddlAssetGroup2").val();
    var AssetGroup3Ids = $(document).find("#ddlAssetGroup3").val();
    var SanMasterGenDueDate = ValidateDate($(document).find("#SanGenDueDate").val());
    var EquipmentClientLookupId = LRTrim($(document).find("#SanGEquipmentClientLookupId").val());
    var EquipmentName = LRTrim($(document).find("#SanGEquipmentName").val());
    var SanMasterGenShift = LRTrim($(document).find("#SanGenShift").val());
    var SanMasterGenDesc = LRTrim($(document).find("#SanGenDesc").val());
    var colname = SaniGenOder;
    var coldir = SaniGenDir;
    var checked = this.checked;
    $.ajax({
        url: "/GenerateSanitationJobs/GetSearchAllSanitationJobGenGrid",
        data: {
            colname: colname,
            coldir: coldir,
            ScheduleType: ScheduleType,
            GeneratedThroughDate: GeneratedThroughDate,
            OnDemandGroup: OnDemandGroup,
            AssetGroup1Ids: AssetGroup1Ids,
            AssetGroup2Ids: AssetGroup2Ids,
            AssetGroup3Ids: AssetGroup3Ids,
            SanMasterGenDueDate: SanMasterGenDueDate,
            EquipmentClientLookupId: EquipmentClientLookupId,
            EquipmentName: EquipmentName,
            SanMasterGenShift: SanMasterGenShift,
            SanMasterGenDesc: SanMasterGenDesc,
            ReturnSanMasterBatchHeaderId: ReturnSanMasterBatchHeaderId

        },
        async: true,
        type: "GET",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    if (checked) {
                        if (SaniBatchEntryToupdate.indexOf(item.SanMasterBatchEntryId) == -1) {
                            SaniBatchEntryToupdate.push(item.SanMasterBatchEntryId);
                            var itemBE = new SaniBatchEntrySelectedItem(item.SanMasterBatchEntryId, item.SanMasterBatchHeaderId);

                            var found = SaniBatchEntrySelectedItemArray.some(function (el) {
                                return el.SanMasterBatchEntryId === item.SanMasterBatchEntryId;
                            });
                            if (!found) {
                                SaniBatchEntrySelectedItemArray.push(itemBE);
                            }
                        }
                    } else {
                        var i = SaniBatchEntryToupdate.indexOf(item.SanMasterBatchEntryId);
                        SaniBatchEntryToupdate.splice(i, 1);
                        SaniBatchEntrySelectedItemArray.splice(i, 1);

                    }
                });
            }
        },
        complete: function () {
            if (checked) {
                $(document).find('#tblGenerateSanitationJobsGrid').find('.chkGenWOsearch').prop('checked', 'checked');
            } else {
                $(document).find('#tblGenerateSanitationJobsGrid').find('.chkGenWOsearch').prop('checked', false);
            }
            CloseLoader();
        }
    });
});
$(document).on('change', '.chkGenWOsearch', function () {
    var data = generateSanitationJobsDt.row($(this).parents('tr')).data();
    if (!this.checked) {
        selectedcount--;
        var index = SaniBatchEntryToupdate.indexOf(data.SanMasterBatchEntryId);
        SaniBatchEntryToupdate.splice(index, 1);
        var el = $('#GenSani-select-all').get(0);
        //if (el && el.checked && ('indeterminate' in el)) {
        //    el.indeterminate = true;
        //}
        if (el && el.checked) {
            el.checked = false;
        }
        SaniBatchEntrySelectedItemArray = SaniBatchEntrySelectedItemArray.filter(function (el) {
            return el.SanMasterBatchEntryId !== data.SanMasterBatchEntryId;
        });
    }
    else {
        SaniBatchEntryToupdate.push(data.SanMasterBatchEntryId);
        selectedcount = selectedcount + SaniBatchEntryToupdate.length;
        var item = new SaniBatchEntrySelectedItem(data.SanMasterBatchEntryId, data.SanMasterBatchHeaderId);
        var found = SaniBatchEntrySelectedItemArray.some(function (el) {
            return el.SanMasterBatchEntryId === data.SanMasterBatchEntryId;
        });
        if (!found) { SaniBatchEntrySelectedItemArray.push(item); }

    }
    if (totalcount == SaniBatchEntryToupdate.length) {
        $(document).find('.dt-body-center').find('#GenSani-select-all').prop('checked', 'checked');
    }
    else {
        $(document).find('.dt-body-center').find('#GenSani-select-all').prop('checked', false);
    }
});

$(document).on('click', "#btnCreateSanitationJobGenPreview", function () {
    var prnChkBEId = $(document).find('#prnChkBEId').is(':checked');
    if (SaniBatchEntryToupdate.length <= 0) {
        ShowGridItemSelectionAlert();
        return false;
    }
    //else if (SaniBatchEntryToupdate.length > SaniGenAllowedPrintNumber && prnChkBEId == true) {
    //    var errorMessage = "You can select maximum " + SaniGenAllowedPrintNumber + " records to proceed.";
    //    ShowErrorAlert(errorMessage);
    //    return false;
    //}
    else {
        var jsonResult = {
            "list": SaniBatchEntrySelectedItemArray,
            "AssetGroup1Ids": $(document).find('#ddlAssetGroup1').val(),
            "AssetGroup2Ids": $(document).find("#ddlAssetGroup2").val(),
            "AssetGroup3Ids": $(document).find("#ddlAssetGroup3").val(),
            "chkPrintSan": $(document).find('#prnChkBEId').is(':checked'),
            "ScheduleType": $(document).find("#ddlscheduletype").val(),
            "OnDemand": $(document).find("#ondemandgroup").val(),
            "GenerateThrough" : ValidateDate($(document).find("#dtgeneratedthrough").val()),
        }


        var dataList = JSON.stringify(jsonResult);
        $.ajax({
            url: "/GenerateSanitationJobs/CreateSanitationJobGenPreview",
            type: "POST",
            dataType: "json",
            data: dataList,
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Msg > 0) {
                    var rowCount = data.Msg;
                     ReturnSanMasterBatchHeaderId = data.ReturnSanMasterBatchHeaderId;
                    SuccessAlertSetting.text = getResourceValue("alertMasterSanitationJobGenerationCompleted") + ' ' + rowCount;
                    swal(SuccessAlertSetting, function () {
                        if (data.SanitationjobList.length > 0 && prnChkBEId == true) {
                            ShowLoader();
                            $.ajax({
                                url: '/MasterSanitationSchedule/SetPrintSanitationJobListFromIndex', 
                                data: {
                                    listOfSanitation: data.SanitationjobList
                                },
                                type: "POST",
                                datatype: "json",
                                responseType: 'arraybuffer',
                                success: function (result) {
                                    if (result.success) {
                                        window.open("/MasterSanitationSchedule/GenerateSanitationJobPrint", "_blank");
                                    }
                                },
                                complete: function () {
                                    $(document).find('#prnChkBEId').prop('checked', false); selectedcount = 0;
                                    //totalcount = 0;
                                    //SaniBatchEntryToupdate = [];
                                    //SaniBatchEntrySelectedItemArray = [];
                                    $(document).find('#GenSani-select-all').prop('checked', false);
                                    /*$(document).find('#btnCreateSanitationJobGen').trigger('click');*/
                                    GetSanitationGeneratedList();
                                    CloseLoader();
                                }
                            });
                        } else {
                            GetSanitationGeneratedList();
                        }
                        //V2-955
                        totalcount = 0;
                        SaniBatchEntryToupdate = [];
                        SaniBatchEntrySelectedItemArray = [];
                    });
                }
                else {
                    ResetErrorDiv();
                    ShowGenericErrorOnAddUpdate(data.Msg);
                }
                CloseLoader();

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
function PdfPrintAllSanitationList(pdf) {
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

function SaniBatchEntrySelectedItem(SanMasterBatchEntryId, SanMasterBatchHeaderId) {
    this.SanMasterBatchEntryId = SanMasterBatchEntryId;
    this.SanMasterBatchHeaderId = SanMasterBatchHeaderId;

}


//#endregion

//#region V2-963
$(document).on('click', "#btnCreateSanitationJobGen", function (event) {
    var res = true;
    $('.modal-col-wrap').find('.form-control').each(function (index, item) {
        if ($(this).hasClass("input-validation-error") && ($(this).val() == undefined || $(this).val() == '')) {
            res = false;
            return false;
        }
    });
    if (res) {
        var dtgenerated = $('#dtgeneratedthrough').val();
        date1 = new Date(dtgenerated);
        date2 = new Date();
        var milli_secs = date1.getTime() - date2.getTime();

        // Convert the milli seconds to Days 
        var days = parseInt(milli_secs / (1000 * 3600 * 24));
        if (days > 30 && days <= 60) {
            var msg = getResourceValue("GenerateThroughMorethan30Alert");
            CancelAlertSetting.text = msg.replace('{0}', days);
            swal(CancelAlertSetting, function () {
                $('#SanitationJobsGenerationForm').submit();
                event.preventDefault();
            });
            return false;
        } else if (days > 60) {
            ErrorAlertSetting.text = getResourceValue("GenerateThroughMorethan60Alert");
            swal(ErrorAlertSetting, function () {

            });
            return false;
        }
    }
});
//#endregion