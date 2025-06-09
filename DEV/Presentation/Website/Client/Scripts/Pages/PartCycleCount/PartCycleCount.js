var partCycleCountdt;
var gridname = "PartCycleCount_Search";
var PartCountorder = '0';
var partCountDir = 'asc';
var run = false;
var EditPartCountArray = [];
var rowcount = 0;
var _StoreroomId = 0;
$(document).find('.select2picker').select2({});
$(document).find('.dtpicker1').datepicker({
    changeMonth: true,
    changeYear: true,
    "dateFormat": "mm/dd/yy",
    autoclose: true,
}).inputmask('mm/dd/yyyy');
$(function () {
    $(document).find('.mulselectclass').show();
    $(document).on('change', '.ddlStoreroom', function () {
        var tlen = $(document).find(".ddlStoreroom").val();
        if (tlen.length > 0) {
            var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find(".ddlStoreroom").removeClass("input-validation-error");
        }
        else {
            var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
            $('#' + areaddescribedby).show();
            $(document).find('form').find(".ddlStoreroom").addClass("input-validation-error");
        }
    });
});
function ValidationOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        GetPartCycleCountGenerateList();
    }
    else {
        ResetErrorDiv();
        ShowGenericErrorOnAddUpdate(data.Msg);
    }
}
function GetPartCycleCountGenerateList() {
    EditPartCountArray = [];
    ShowLoader();
    var count = 0;
    var rowcount = 0;
    if ($(document).find('#tblpartcyclecount').hasClass('dataTable')) {
        partCycleCountdt.destroy();
    }
    if ($(document).find('#MultiStoreroom').val() == "True") {
        _StoreroomId = $(document).find(".ddlStoreroom").val();
    }
    partCycleCountdt = $("#tblpartcyclecount").DataTable({
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        scrollX: true,
        fixedColumns: {
            leftColumns: 1
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Part Cycle Count List'
            },
            {
                extend: 'print',
                title: 'Part Cycle Count List'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Part Cycle Count List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Part Cycle Count List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            url: "/PartCycleCount/GetPartGridData",
            "type": "post",
            "datatype": "json",
            cache: false,
            data: function (d) {
                d.Area = LRTrim($('#partCycleCountModel_Area').val());
                d.Row = LRTrim($('#partCycleCountModel_Row').val());
                d.Shelf = LRTrim($('#partCycleCountModel_Shelf').val());
                d.Bin = LRTrim($('#partCycleCountModel_Bin').val());
                d.StockType = $('#partCycleCountModel_StockType').val();
                d.Critical = $('#partCycleCountModel_CriticalFlag').is(":checked") ? true : false;
                d.Consignment = $('#partCycleCountModel_Consignment').is(":checked") ? true : false;
                d.GenerateThrough = ValidateDate($('#partCycleCountModel_GenerateThrough').val());
                d.PartClientLookupId = LRTrim($('#PartID').val());
                d.PartDescription = LRTrim($('#Description').val());
                d.Section = LRTrim($('#partCycleCountModel_Section').val());
                d.Order = PartCountorder;
                d.StoreroomId = _StoreroomId;
            },
            "dataSrc": function (result) {
                if (result.data.length < 1) {
                    $(document).find('#btnPartCountExport').prop('disabled', true);
                }
                else {
                    $(document).find('#btnPartCountExport').prop('disabled', false);
                }
                let PartCountorder = partCycleCountdt.order();
                partCountDir = PartCountorder[0][1];
                TotalCount = result.recordsTotal;
                return result.data;
            },
            global: true
        },
        "columns":
            [

                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
                { "data": "PartDescription", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                //V2-765
                /*{ "data": "QtyOnHand", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3", "className": "text-right" },*/
                {
                    "data": "QuantityCount", "autoWidth": true, "bSearchable": true, "bSortable": false,
                    'render': function (data, type, row) {

                        var EditPartArrray = EditPartCountArray.filter(function (el) {
                            return el.PartId == row.PartId;
                        });
                        if (EditPartArrray.length > 0) {
                            return "<div style='width:100px !important; position:relative;'><input type='text' style='width:90px !important;text-align:right;' class='duration  dt-inline-text decimalinputupto2places grd-count' autocomplete='off' value='" + EditPartArrray[0].QuantityCount + "' maskedFormat='6,2'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-3px; color:green;display:block;' title='success'></i></div>";
                        } else {
                            return "<div style='width:100px !important; position:relative;'><input type='text' style='width:90px !important;text-align:right;' class='duration  dt-inline-text decimalinputupto2places grd-count' autocomplete='off' value='" + data + "' maskedFormat='6,2'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-3px; color:green;display:none;' title='success'></i></div>";
                        }
                    }
                },
                { "data": "Area", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
                { "data": "Section", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
                { "data": "Row", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7" },
                { "data": "Shelf", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "8" },
                { "data": "Bin", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "9" },
                {
                    "data": "Variance", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "10", "className": "text-right",
                    'render': function (data, type, row) {

                        var EditPartArrray = EditPartCountArray.filter(function (el) {
                            return el.PartId == row.PartId;
                        });

                        var CountQuantity = 0;
                        if (EditPartArrray.length > 0) {
                            CountQuantity = EditPartArrray[0].QuantityCount;
                        }
                        var variance = CountQuantity - row.QtyOnHand;
                        return variance;
                    }
                },
                {
                    "data": "", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "11", "className": "text-center",
                    'render': function (data, type, row) {
                        return "<input class='resetcount' id='" + (rowcount++) + "' type='button' value='Reset'>";
                    }
                }
            ],
        initComplete: function () {
            $("#finselectcontainer").show();
            SetPageLengthMenu();
            /*V2 - 765*/
            var column = this.api().column(8);
            column.visible(false);

            CloseLoader();
        }
    });


}

$(document).on('click', '.paginate_button', function () {
    run = true;
});
$(document).on('change', '.searchdt-menu', function () {
    run = true;
});
$('#tblpartcyclecount').find('th').click(function () {
    if ($(this).data('col')) {
        run = true;
        PartCountorder = $(this).data('col');
    }
});
$("#sidebar").mCustomScrollbar({
    theme: "minimal"
});
$("#PartBulksidebar").mCustomScrollbar({
    theme: "minimal"
});
$('#dismiss, .overlay').on('click', function () {
    $('#sidebar,#Bulksidebar').removeClass('active');
    $('.overlay').fadeOut();
});
$('#sidebarCollapse').on('click', function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});


$("#btnPartsDataAdvSrch").on('click', function (e) {
    partCycleCountdt.state.clear();
    var searchitemhtml = "";
    filteritemcount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            filteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    AdvanceSearch();
});
function AdvanceSearch() {
    partCycleCountdt.page('first').draw('page');
    $('.filteritemcount').text(filteritemcount);
}
$(document).on('click', '.btnCross', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    filteritemcount--;
    AdvanceSearch()
});

$(document).on('click', '#liClearAdvSearchFilterAVPMG', function () {
    clearAdvanceSearch();
    partCycleCountdt.page('first').draw('page');
});
function clearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
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
            var Area = $(document).find('#partCycleCountModel_Area').val();
            var Section = $(document).find('#partCycleCountModel_Section').val();
            var Row = $(document).find('#partCycleCountModel_Row').val();
            var Shelf = $(document).find('#partCycleCountModel_Shelf').val();
            var Bin = $(document).find('#partCycleCountModel_Bin').val();
            var StockType = $(document).find('#partCycleCountModel_StockType').val();
            var Critical = false;
            if ($('#partCycleCountModel_CriticalFlag').is(":checked")) {
                Critical = true;
            }
            var Consignment = false;
            if ($('#partCycleCountModel_Consignment').is(":checked")) {
                Consignment = true;
            }
            if ($(document).find('#MultiStoreroom').val() == "True") {
                _StoreroomId = $(document).find(".ddlStoreroom").val();
            }
            var GenerateThrough = ValidateDate($(document).find('#partCycleCountModel_GenerateThrough').val());
            var colname = PartCountorder;
            var coldir = partCountDir;
            var PartClientLookupId = $("#PartID").val();
            var PartDescription = $("#Description").val();
            var jsonResult = $.ajax({
                "url": "/PartCycleCount/GetPartCycleCountGridPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    colname: colname,
                    coldir: coldir,
                    Area: LRTrim(Area),
                    Row: LRTrim(Row),
                    Shelf: LRTrim(Shelf),
                    Bin: LRTrim(Bin),
                    StockType: StockType,
                    Critical: Critical,
                    Consignment: Consignment,
                    GenerateThrough: GenerateThrough,
                    PartClientLookupId: LRTrim(PartClientLookupId),
                    PartDescription: LRTrim(PartDescription),
                    Section: Section,
                    StoreroomId: _StoreroomId
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#tblpartcyclecount thead tr th").not(":eq(8)").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
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
                header: $("#tblpartcyclecount thead tr th").not(":eq(8)").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});
//#endregion


//#region Count edit

var enterhit = 0;
var Count = 0;
var oldScheduledDate = '';
function PartCycleCountSelectedItem(PartId, ClientLookupId, PartDescription, QtyOnHand, Variance, Countval) {
    this.PartId = PartId;
    this.ClientLookupId = ClientLookupId;
    this.Description = PartDescription;
    this.QtyOnHand = QtyOnHand;
    this.Variance = Variance;
    this.QuantityCount = Countval;
};

$(document).on('blur', '.grd-count', function (event) {
    $('.grd-count').prop("disabled", true);
    var row = $(this).parents('tr');
    var data = partCycleCountdt.row(row).data();
    var thstextbox = $(this);
    var Countval = $(this).val();

    if (Countval == '') {
        $(this).val(0);
        Countval = 0;
    }
    var pageno = partCycleCountdt.page.info().page;
    var Variance = Countval - data.QtyOnHand;
    data.Variance = Variance;
    thstextbox.siblings('.is-saved-check').show();
    var item = new PartCycleCountSelectedItem(
        data.PartId,
        data.ClientLookupId,
        data.PartDescription,
        data.QtyOnHand,
        Variance,
        Countval
    );
    EditPartCountArray = EditPartCountArray.filter(function (el) {
        return el.PartId != data.PartId;
    });
    EditPartCountArray.push(item);
    if (EditPartCountArray.length > 0) {
        $(document).find('#btnconfirm').removeAttr('disabled');
    }
    else { $(document).find('#btnconfirm').attr('disabled', 'disabled'); }
    partCycleCountdt.page(pageno).draw('page');
});


$(document).on('click', '.resetcount', function (event) {
    var row = $(this).parents('tr');
    var data = partCycleCountdt.row(row).data();
    row.find('td:eq(2)').find('input[type="text"]').val('0');
    row.find('td:eq(2)').find('.is-saved-check').hide();
    EditPartCountArray = EditPartCountArray.filter(function (el) {
        return el.PartId != data.PartId;
    });
    if (EditPartCountArray.length > 0) {
        $(document).find('#btnconfirm').removeAttr('disabled');
    }
    else { $(document).find('#btnconfirm').attr('disabled', 'disabled'); }
    var info = partCycleCountdt.page.info();
    var pageclicked = info.page;
    partCycleCountdt.page(pageclicked).draw('page');
});


$(document).on('click', "#btnconfirm", function () {
    if (EditPartCountArray.length < 1) {
        swal({
            title: getResourceValue("CommonErrorAlert"),
            text: getResourceValue("additeningrid"),
            type: "error",
            confirmButtonText: getResourceValue("SaveAlertOk"),
        }, function () {
        });
        return false;
    }
    else {
        UpdateSelectedPartsStoreroomTable(EditPartCountArray);
    }
});
function UpdateSelectedPartsStoreroomTable(EditPartCountArray) {
    if ($(document).find('#MultiStoreroom').val() == "True") {
        _StoreroomId = $(document).find(".ddlStoreroom").val();
    }
    var list = JSON.stringify({ 'list': EditPartCountArray, StoreroomId: _StoreroomId});
    $.ajax({
        url: "/PartCycleCount/SaveListPartCountFromGrid",
        type: "POST",
        dataType: "json",
        data: list,
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                EditPartCountArray = [];
                SuccessAlertSetting.text = getResourceValue("RecordsUpdateAlert");
                swal(SuccessAlertSetting, function () {
                    $(document).find('#partCycleCountModel_Area').val("");
                    $(document).find('#partCycleCountModel_Row').val("");
                    $(document).find('#partCycleCountModel_Shelf').val("");
                    $(document).find('#partCycleCountModel_Bin').val("");
                    $(document).find('#partCycleCountModel_StockType').val("").trigger('change');
                    $(document).find('#partCycleCountModel_Section').val("");
                    $(document).find('#partCycleCountModel_GenerateThrough').val("");
                    if ($(document).find('#MultiStoreroom').val() == "True") {
                        $(document).find(".ddlStoreroom").val("").trigger('change');
                        var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
                        $('#' + areaddescribedby).hide();
                        $(document).find('form').find(".ddlStoreroom").removeClass("input-validation-error");
                    }
                    $('#finselectcontainer').hide();
                    EditPartCountArray = [];
                    partCycleCountdt.clear().draw();

                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
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


//#endregion