var PrentiveMaintenanceGenerateWOdt;
var run = false;
var selectedcount = 0;
var totalcount = 0;
var PrevBatchEntryToupdate = [];
var PrevBatchEntrySelectedItemArray = [];
var searchcount = 0;
var searchresult = [];
var select = 0;
var gridname = "PMGenerateWorkOrders_Search";
var PMGenWOorder = 1;
var PMGenWOColOrder = "";
var PMGenWODir = 'asc';
var WOAllowedPrintNumber = 50;
var ReturnPrevMaintBatchHeaderId = 0;
$(function () {
    $(document).find('.select2picker').select2({});
    $(document).find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '.dismiss, .overlay', function () {
        $('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', '#PMGsidebarCollapse', function () {
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
    ProgressbarDynamicStatus();
});
$.validator.setDefaults({ ignore: null });
$('input, form').blur(function () {
    if ($(this).closest('form').length > 0) {
        $(this).valid();
    }
});
$('.select2picker, form').change(function () {
    var areaddescribedby = $(this).attr('aria-describedby');
    if ($(this).closest('form').length > 0) {
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
    }
});
$(document).find('.select2picker').select2({});

//#region Print Work Order
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
//#region PM Generated Add Work Order
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
function CreatePMWorkOrderOnSuccess(data) {
    CloseLoader();
    if (data.Previewval) {
        if (data.Result == "success") {
            clearAdvanceSearch();
            PrevBatchEntryToupdate = [];
            PrevBatchEntrySelectedItemArray = [];
            $(document).find('#GenWO-select-all').prop('checked', false);
            ReturnPrevMaintBatchHeaderId = 0;
            GetPMGeneratedWOList();
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
            SuccessAlertSetting.text = getResourceValue("woGenAlert") + ' ' + rowCount;
            swal(SuccessAlertSetting, function () {
                if (data.WoList.length > 0 && prnChkIdval == true) {
                    $.ajax({
                        url: '/Workorder/PrintWoList',
                        data: {
                            listwo: data.WoList
                        },
                        type: "POST",
                        datatype: "json",
                        responseType: 'arraybuffer',
                        success: function (result) {
                            if (result.success) {
                                window.open("/WorkOrder/GenerateWorkOrderPrint", "_blank");
                            }
                        },
                        complete: function () {
                            $(document).find('#prnChkId').prop('checked', false);
                            ResetErrorDiv();
                            $(document).find('#ddlscheduletype').val("").trigger('change');
                            $(document).find('#dtgeneratedthrough').val("");
                            $(document).find('#ondemandgroup').val("").trigger('change');
                            $(document).find('#ddlAssetGroup1').val("").trigger('change');
                            $(document).find('#ddlAssetGroup2').val("").trigger('change');
                            $(document).find('#ddlAssetGroup3').val("").trigger('change');
                            $(document).find('#ddlWOType').val("").trigger('change');
                            $(document).find('#ddlPMType').val("").trigger('change');
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

//#region PM Generated Batch Entry Search

$(document).on('click', '#GenWO-select-all', function (e) {
    PrevBatchEntryToupdate = [];
    PrevBatchEntrySelectedItemArray = [];

    var ScheduleType = $(document).find("#ddlscheduletype").val();
    var GeneratedThroughDate = ValidateDate($(document).find("#dtgeneratedthrough").val());
    var OnDemandGroup = $(document).find("#ondemandgroup").val();
    var AssetGroup1Ids = $(document).find('#ddlAssetGroup1').val();
    var AssetGroup2Ids = $(document).find("#ddlAssetGroup2").val();
    var AssetGroup3Ids = $(document).find("#ddlAssetGroup3").val();
    var WOType = $(document).find("#ddlWOType").val();
    var PMType = $(document).find("#ddlPMType").val();
    var PrevBEDueDate = ValidateDate($(document).find("#PMGBEDueDate").val());
    var EquipmentClientLookupId = LRTrim($(document).find("#PMGEquipmentClientLookupId").val());
    var EquipmentName = LRTrim($(document).find("#PMGEquipmentName").val());
    var PrevMaintMasterClientLookupId = LRTrim($(document).find("#PMGMAssetJobID").val());
    var PrevMaintMasterDescription = LRTrim($(document).find("#PMGPMDesc").val());
    var colname = PMGenWOorder;
    var coldir = PMGenWODir;
    var checked = this.checked;
    $.ajax({
        url: "/PMGenerateWorkOrders/GetSearchAllPreventiveMainGenWOGrid",
        data: {
            colname: colname,
            coldir: coldir,
            ScheduleType: ScheduleType,
            GeneratedThroughDate: GeneratedThroughDate,
            OnDemandGroup: OnDemandGroup,
            AssetGroup1Ids: AssetGroup1Ids,
            AssetGroup2Ids: AssetGroup2Ids,
            AssetGroup3Ids: AssetGroup3Ids,
            WOType: WOType,
            PMType: PMType,
            PrevBEDueDate: PrevBEDueDate,
            EquipmentClientLookupId: EquipmentClientLookupId,
            EquipmentName: EquipmentName,
            PrevMaintMasterClientLookupId: PrevMaintMasterClientLookupId,
            PrevMaintMasterDescription: PrevMaintMasterDescription,
            ReturnPrevMaintBatchHeaderId: ReturnPrevMaintBatchHeaderId,
            //V2-1082
            downRequired :$("#txtDownRequired").val(),
            shifts : $("#gridadvsearchshift").val()

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
                        if (PrevBatchEntryToupdate.indexOf(item.PrevMaintBatchEntryId) == -1) {
                            PrevBatchEntryToupdate.push(item.PrevMaintBatchEntryId);
                            var itemBE = new PrevBatchEntrySelectedItem(item.PrevMaintBatchEntryId, item.PrevMaintBatchHeaderId, item.PrevMaintSchedId, item.PlanningRequired);

                            var found = PrevBatchEntrySelectedItemArray.some(function (el) {
                                return el.PrevMaintBatchEntryId === item.PrevMaintBatchEntryId;
                            });
                            if (!found) {
                                PrevBatchEntrySelectedItemArray.push(itemBE);
                            }
                        }
                    } else {
                        var i = PrevBatchEntryToupdate.indexOf(item.PrevMaintBatchEntryId);
                        PrevBatchEntryToupdate.splice(i, 1);
                        PrevBatchEntrySelectedItemArray.splice(i, 1);

                    }
                });
            }
        },
        complete: function () {
            if (checked) {
                $(document).find('#tblPMGenerateWorkOrdersGrid').find('.chkGenWOsearch').prop('checked', 'checked');
            } else {
                $(document).find('#tblPMGenerateWorkOrdersGrid').find('.chkGenWOsearch').prop('checked', false);
            }
            CloseLoader();
        }
    });
});
$(document).on('change', '.chkGenWOsearch', function () {
    var data = PrentiveMaintenanceGenerateWOdt.row($(this).parents('tr')).data();
    if (!this.checked) {
        selectedcount--;
        var index = PrevBatchEntryToupdate.indexOf(data.PrevMaintBatchEntryId);
        PrevBatchEntryToupdate.splice(index, 1);
        var el = $('#GenWO-select-all').get(0);        
        if (el && el.checked) {
            el.checked = false;
        }
        PrevBatchEntrySelectedItemArray = PrevBatchEntrySelectedItemArray.filter(function (el) {
            return el.PrevMaintBatchEntryId !== data.PrevMaintBatchEntryId;
        });
    }
    else {
        PrevBatchEntryToupdate.push(data.PrevMaintBatchEntryId);
        selectedcount = selectedcount + PrevBatchEntryToupdate.length;
        var item = new PrevBatchEntrySelectedItem(data.PrevMaintBatchEntryId, data.PrevMaintBatchHeaderId, data.PrevMaintSchedId, data.PlanningRequired);
        var found = PrevBatchEntrySelectedItemArray.some(function (el) {
            return el.PrevMaintBatchEntryId === data.PrevMaintBatchEntryId;
        });
        if (!found) { PrevBatchEntrySelectedItemArray.push(item); }

    }
    if (totalcount == PrevBatchEntryToupdate.length) {
        $(document).find('.dt-body-center').find('#GenWO-select-all').prop('checked', 'checked');
    }
    else {
        $(document).find('.dt-body-center').find('#GenWO-select-all').prop('checked', false);
    }
});

$(document).on('click', "#btnCreatePmWorkOrdersPreview", function () {
    var prnChkBEId = $(document).find('#prnChkBEId').is(':checked');
    if (PrevBatchEntryToupdate.length <= 0) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else if (PrevBatchEntryToupdate.length > WOAllowedPrintNumber && prnChkBEId == true) {
        var errorMessage = "You can select maximum " + WOAllowedPrintNumber + " records to proceed.";
        ShowErrorAlert(errorMessage);
        return false;
    }
    else {
        var jsonResult = {
            "list": PrevBatchEntrySelectedItemArray,
            "AssetGroup1Ids": $(document).find('#ddlAssetGroup1').val(),
            "AssetGroup2Ids": $(document).find("#ddlAssetGroup2").val(),
            "AssetGroup3Ids": $(document).find("#ddlAssetGroup3").val(),
            "PrevMaintSchedType": $(document).find("#ddlWOType").val(),
            "PrevMaintMasterType": $(document).find("#ddlPMType").val(),
            "chkPrintWorkOrder": $(document).find('#prnChkBEId').is(':checked')
        }


        var dataList = JSON.stringify(jsonResult);
        $.ajax({
            url: "/PMGenerateWorkOrders/CreatePMWorkOrderPreview",
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
                    ReturnPrevMaintBatchHeaderId = data.ReturnPrevMaintBatchHeaderId;
                    SuccessAlertSetting.text = getResourceValue("woGenAlert") + ' ' + rowCount;
                    swal(SuccessAlertSetting, function () {
                        if (data.WoList.length > 0 && prnChkBEId == true) {
                            $.ajax({
                                url: '/Workorder/PrintWoList',
                                data: {
                                    listwo: data.WoList
                                },
                                type: "POST",
                                datatype: "json",
                                responseType: 'arraybuffer',
                                success: function (result) {
                                    if (result.success) {
                                        window.open("/WorkOrder/GenerateWorkOrderPrint", "_blank");
                                    }
                                },
                                complete: function () {
                                    $(document).find('#prnChkBEId').prop('checked', false); selectedcount = 0;                                   
                                    $(document).find('#GenWO-select-all').prop('checked', false);                                    
                                    GetPMGeneratedWOList();
                                    CloseLoader();
                                }
                            });
                        } else {
                            GetPMGeneratedWOList();
                        }
                        //V2-955
                        totalcount = 0;
                        PrevBatchEntryToupdate = [];
                        PrevBatchEntrySelectedItemArray = [];
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

function PrevBatchEntrySelectedItem(PrevMaintBatchEntryId, PrevMaintBatchHeaderId, PrevMaintSchedId, PlanningRequired) {
    this.PrevMaintBatchEntryId = PrevMaintBatchEntryId;
    this.PrevMaintBatchHeaderId = PrevMaintBatchHeaderId;
    this.PrevMaintSchedId = PrevMaintSchedId;
    this.PlanningRequired = PlanningRequired;

}

function GetPMGeneratedWOList() {
    var ScheduleType = $(document).find("#ddlscheduletype").val();
    var GeneratedThroughDate = ValidateDate($(document).find("#dtgeneratedthrough").val());
    var OnDemandGroup = $(document).find("#ondemandgroup").val();
    var AssetGroup1Ids = $(document).find('#ddlAssetGroup1').val();
    var AssetGroup2Ids = $(document).find("#ddlAssetGroup2").val();
    var AssetGroup3Ids = $(document).find("#ddlAssetGroup3").val();
    var WOType = $(document).find("#ddlWOType").val();
    var PMType = $(document).find("#ddlPMType").val();
    var PrevBEDueDate = ValidateDate($(document).find("#PMGBEDueDate").val());
    var EquipmentClientLookupId = LRTrim($(document).find("#PMGEquipmentClientLookupId").val());
    var EquipmentName = LRTrim($(document).find("#PMGEquipmentName").val());
    var PrevMaintMasterClientLookupId = LRTrim($(document).find("#PMGMAssetJobID").val());
    var PrevMaintMasterDescription = LRTrim($(document).find("#PMGPMDesc").val());

    $('#GenWOcontainer').show();
    if ($(document).find('#tblPMGenerateWorkOrdersGrid').hasClass('dataTable')) {
        PrentiveMaintenanceGenerateWOdt.destroy();
    }
    PrentiveMaintenanceGenerateWOdt = $("#tblPMGenerateWorkOrdersGrid").DataTable({
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
                    data.order[0][0] = PMGenWOorder;
                    data.order[0][1] = PMGenWODir;
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
                        PMGenWOorder = LayoutInfo.order[0][0];
                        PMGenWODir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                    }
                    else {
                        callback(json.LayoutInfo);
                    }
                }
            });
            //Search Retention
        },
        scrollX: true,        
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Generate Work Orders List'
            },
            {
                extend: 'print',
                title: 'Generate Work Orders List'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Generate Work Orders List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Generate Work Orders List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            url: "/PMGenerateWorkOrders/GetPreventiveMainGenWOGrid",
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
                d.WOType = WOType;
                d.PMType = PMType;
                d.PrevBEDueDate = PrevBEDueDate;
                d.EquipmentClientLookupId = EquipmentClientLookupId;
                d.EquipmentName = EquipmentName;
                d.PrevMaintMasterClientLookupId = PrevMaintMasterClientLookupId;
                d.PrevMaintMasterDescription = PrevMaintMasterDescription;
                d.ReturnPrevMaintBatchHeaderId = ReturnPrevMaintBatchHeaderId;
                //V2-1082
                d.downRequired = $("#txtDownRequired").val();
                d.shifts = $("#gridadvsearchshift").val();

            },
            "dataSrc": function (result) {
                if (result.data.length < 1) {
                    $(document).find('#btnPMGenerateWOExport,#GenWO-select-all').prop('disabled', true);
                }
                else {
                    $(document).find('#btnPMGenerateWOExport,#GenWO-select-alll').prop('disabled', false);
                }
                let PMGenWOorder = PrentiveMaintenanceGenerateWOdt.order();
                PMGenWOColOrder = PMGenWOorder[0][0];
                PMGenWODir = PMGenWOorder[0][1];
                totalcount = result.recordsTotal;
                ReturnPrevMaintBatchHeaderId = result.ReturnPrevMaintBatchHeaderId;
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "PrevMaintBatchEntryId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    'render': function (data, type, full, meta) {
                        if ($('#GenWO-select-all').is(':checked') && totalcount == selectedcount) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-prevmaintbatchEntryid="' + data + '" class="chkGenWOsearch ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        } else {
                            if (PrevBatchEntryToupdate.indexOf(data) != -1) {
                                return '<input type="checkbox" checked="checked" name="id[]" data-prevmaintbatchEntryid="' + data + '" class="chkGenWOsearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                            else {
                                return '<input type="checkbox" name="id[]" data-prevmaintbatchEntryid="' + data + '" class="chkGenWOsearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                        }
                    }
                },
                { "data": "DueDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
                { "data": "EquipmentClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "EquipmentName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                { "data": "PrevMaintMasterClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                { "data": "PrevMaintMasterDescription", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
                { "data": "PMRequiredDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
                {
                    "data": "AssignedTo_Name", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7", "sClass": "ghover",
                    "mRender": function (data, type, full, meta) {
                        return "<span>" + full.AssignedMultiple + "</span><span class='tooltipgrid' id=" + full.PrevMaintBatchEntryId + ">" + data + "</span><span class='loadingImg' style='display:none !important;'><img src='/Images/lineLoader.gif' style='width:55px;height:auto;position:absolute;left:6px;top:30px;'></span>";
                    }
                },
                {
                    "data": "DownRequired", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center",
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
                    "data": "Shift", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        if (data == null) {
                            data = "";
                        } else {
                            data = data;
                        }
                        return "<div class='text-wrap width-150'>" + data + "</div>";
                    }
                }
            ],
        columnDefs: [
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            DisableExportButton($("#tblPMGenerateWorkOrdersGrid"), $(document).find('#btnPMGenerateWOExport,#GenWO-select-all,#prnChkBEId,#btnCreatePmWorkOrdersPreview'));
        }
    });
}
//#endregion
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
            var WOType = $(document).find("#ddlWOType").val();
            var PMType = $(document).find("#ddlPMType").val();
            var PrevBEDueDate = ValidateDate($(document).find("#PMGBEDueDate").val());
            var EquipmentClientLookupId = LRTrim($(document).find("#PMGEquipmentClientLookupId").val());
            var EquipmentName = LRTrim($(document).find("#PMGEquipmentName").val());
            var PrevMaintMasterClientLookupId = LRTrim($(document).find("#PMGMAssetJobID").val());
            var PrevMaintMasterDescription = LRTrim($(document).find("#PMGPMDesc").val());
            var colname = PMGenWOColOrder;
            var coldir = PMGenWODir;
            var jsonResult = $.ajax({
                "url": "/PMGenerateWorkOrders/GetPreventiveMainGenWOGridPrintData",
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
                    WOType: WOType,
                    PMType: PMType,
                    PrevBEDueDate: PrevBEDueDate,
                    EquipmentClientLookupId: EquipmentClientLookupId,
                    EquipmentName: EquipmentName,
                    PrevMaintMasterClientLookupId: PrevMaintMasterClientLookupId,
                    PrevMaintMasterDescription: PrevMaintMasterDescription,
                    ReturnPrevMaintBatchHeaderId: ReturnPrevMaintBatchHeaderId,
                    //V2-1082
                    downRequired: $("#txtDownRequired").val(),
                    shifts: $("#gridadvsearchshift").val()
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#tblPMGenerateWorkOrdersGrid thead tr th").map(function (key) {
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
                if (item.PrevMaintMasterClientLookupId != null) {
                    item.PrevMaintMasterClientLookupId = item.PrevMaintMasterClientLookupId;
                }
                else {
                    item.PrevMaintMasterClientLookupId = "";
                }
                if (item.PrevMaintMasterDescription != null) {
                    item.PrevMaintMasterDescription = item.PrevMaintMasterDescription;
                }
                else {
                    item.PrevMaintMasterDescription = "";
                }
                if (item.RequiredDate != null) {
                    item.RequiredDate = item.RequiredDate;
                }
                else {
                    item.RequiredDate = "";
                }
                if (item.AssignedToName != null) {
                    item.AssignedToName = item.AssignedToName;
                }
                else {
                    item.AssignedToName = "";
                }
                if (item.DownRequired != null) {
                    item.DownRequired = item.DownRequired;
                }
                if (item.Shift != null) {
                    item.Shift = item.Shift;
                }
                else {
                    item.Shift = "";
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
                header: $("#tblPMGenerateWorkOrdersGrid thead tr th").not(":eq(0)").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});
//#endregion
//#region Advance Search
$(document).on('click', "#btnPMGeneratedDataAdvSrch", function (e) {
    PrentiveMaintenanceGenerateWOdt.state.clear();
    var searchitemhtml = "";
    hGridfilteritemcount = 0
    $('#advsearchsidebarPMGenerated').find('.adv-item').each(function (index, item) {
        if ([].concat($(this).val()).filter(function (valueOfFilter) { return valueOfFilter != ''; }).length) {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossHistory" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('.sidebar').removeClass('active');
    $('#PMGeneratedadvsearchcontainer').find('#sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    GridAdvanceSearch();
});
function GridAdvanceSearch() {
    GetPMGeneratedWOList();
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
    GetPMGeneratedWOList();
});
function clearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebarPMGenerated').find('input:text').val('');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
    $(document).find("#txtDownRequired").val("").trigger('change.select2');
    $(document).find("#gridadvsearchshift").val("").trigger('change.select2');
}
//#endregion


//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(PrentiveMaintenanceGenerateWOdt, true);
});
$(document).on('click', '.saveConfig', function () {

    var colOrder = [0, 1];
    funCustozeSaveBtn(PrentiveMaintenanceGenerateWOdt, colOrder);
    run = true;
    PrentiveMaintenanceGenerateWOdt.state.save(run);
});


//#endregion

//#region V2-963
$(document).on('click', "#btnCreatePmWorkOrders", function (event) {
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
                $('#PMGenWOForm').submit();
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

//#region V2-1014
$('#tblPMGenerateWorkOrdersGrid').on('mouseenter', '.ghover', function (e) {
    var rowData = PrentiveMaintenanceGenerateWOdt.row(this).data();
    if (rowData != undefined) {
        var ChildCount = rowData.ChildCount;
        var thise = $(this);
        if (LRTrim(thise.find('.tooltipgrid').text()).length > 0 && ChildCount > 1) {
            thise.find('.tooltipgrid').attr('style', 'display :block !important;');
            return;
        }
    }
});
$('#tblPMGenerateWorkOrdersGrid').on('mouseleave', '.ghover', function (e) {
    $(this).find('.tooltipgrid').attr('style', 'display :none !important;');
});
//#endregion