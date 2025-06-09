var DTpartTransferSearchTable;
var run = false;
var typeVal;
var ptStatus;
var dropValStatus;

//#region OnPageLoadJs
$(document).ready(function () {
    ShowbtnLoaderclass("LoaderDrop");
    ShowbtnLoader("btnsortmenu");
    $("#parttransferAction :input").attr("disabled", "disabled");
    $(".actionBar").fadeIn();
    $(document).find('.select2picker').select2({});
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    //#region Load Grid With Status
    var partTransferttatus = localStorage.getItem("PARTTRANSFERGRIDDISPLAYSTATUS");
    if (partTransferttatus) {
        ptStatus = partTransferttatus;
        generatepTransferDataTable();
        $('#SearchTextDropID').val(partTransferttatus).trigger('change.select2');
    }
    else {
        ptStatus = 0;
        generatepTransferDataTable();
    }

    //#endregion
});
$("#SearchTextDropID").change(function () {
    ShowbtnLoaderclass("LoaderDrop");
    run = true;
    var optionval = $('#SearchTextDropID').val();
    localStorage.setItem("PARTTRANSFERGRIDDISPLAYSTATUS", optionval);

    if (optionval.length !== 0) {

        ptStatus = optionval;
    }
    else {
        ptStatus = 0;
    }
    dropValStatus = $("#advstatus").val();
    DTpartTransferSearchTable.page('first').draw('page');
});
$(document).on('click', '#sidebarCollapse', function () {
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
});
$(document).on('click', "ul.vtabs li", function () {
    $("ul.vtabs li").removeClass("active");
    $(this).addClass("active");
    $(".tabsArea").hide();
    var activeTab = $(this).find("a").attr("href");
    $(activeTab).fadeIn();
    return false;
});
function openCity(evt, cityName) {
    if (cityName == "tabEventLog") {
        GenerateEventLogGrid();
    }
    evt.preventDefault();
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
$(document).on('click', "#InvoiceOverViewSidebar", function () {
    $(document).find('#btnidentification').addClass('active');
    $(document).find('#PartTransferOverview').show();
});
//#endregion

//#region Search
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
function generatepTransferDataTable() {
    var printCounter = 0;
    if ($(document).find('#partTransferSearchTable').hasClass('dataTable')) {
        DTpartTransferSearchTable.destroy();
    }
    DTpartTransferSearchTable = $("#partTransferSearchTable").DataTable({
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
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: "PartTransfer_Search",
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
            $.ajax({
                "url": "/Base/GetState",
                "data": {
                    GridName: "PartTransfer_Search",
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
                title: 'Part Transfer'
            },
            {
                extend: 'print',
                title: 'Part Transfer',
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Part Transfer',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/PartTransfer/GetPartTransforGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.SearchTextDropID = ptStatus;
                d.PartTransferId = LRTrim($('#advtransferid').val());
                d.RequestSite_Name = LRTrim($("#advrequestsite").val());
                d.RequestPartId = LRTrim($("#advrequestpartid").val());
                d.Quantity = LRTrim($('#advquantity').val());
                d.Status = $("#advstatus option:selected").val();
                d.Description = LRTrim($("#advrequestdescription").val());
                d.RequestSite_Name = LRTrim($("#advissuesitename").val());
                d.IssuePartId = LRTrim($("#advissuepartid").val());
                d.Reason = LRTrim($("#advreason").val());
                d.LastEvent = LRTrim($("#advlastevent").val());
                d.LastEventDate = ValidateDate($("#advlasteventdate").val());
            },
            "dataSrc": function (result) {
                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                $("#advstatus").empty();
                $("#advstatus").append("<option value=''>" + "--Select--" + "</option>");
                $.each(result.StatusList, function (index, item) {
                    $("#advstatus").append("<option value='" + item + "'>" + getStatusValue(item) + "</option>");
                });
                if (dropValStatus) {
                    $("#advstatus").val(dropValStatus);
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
                "data": "PartTransferId",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "className": "text-left",
                "name": "0",
                "mRender": function (data, type, row, full) {
                    return '<a class=lnk_part_transfer href="javascript:void(0)">' + data + '</a>';
                }
            },
            { "data": "RequestSite_Name", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
            { "data": "RequestPart_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
            { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
            { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
            { "data": "RequestPart_Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
            { "data": "IssueSite_Name", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
            { "data": "IssuePart_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7" },
            { "data": "Reason", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "8" },
            { "data": "LastEvent", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "9" },
            {
                "data": "LastEventDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "10", "type": "date",
                render: function (data, type, row, meta) {
                    if (data == null) {
                        return '';
                    } else {
                        return data;
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
            SetPageLengthMenu();
            $("#parttransferAction :input").removeAttr("disabled");
            $("#parttransferAction :button").removeClass("disabled");
        }
    });
};
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            dtTable = $("#partTransferSearchTable").DataTable();
            var currestsortedcolumn = $('#partTransferSearchTable').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#partTransferSearchTable').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#partTransferSearchTable').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var jsonResult = $.ajax({
                "url": "/PartTransfer/PartTransferPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    colname: colname,
                    coldir: coldir,
                    SearchTextDropID: ptStatus,
                    PartTransferId: LRTrim($('#advtransferid').val()),
                    RequestSite_Name: LRTrim($("#advrequestsite").val()),
                    RequestPartId: LRTrim($("#advrequestpartid").val()),
                    Quantity: LRTrim($('#advquantity').val()),
                    Status: $("#advstatus option:selected").val(),
                    Description: LRTrim($("#advrequestdescription").val()),
                    RequestSite_Name: LRTrim($("#advissuesitename").val()),
                    IssuePartId: LRTrim($("#advissuepartid").val()),
                    Reason: LRTrim($("#advreason").val()),
                    LastEvent: LRTrim($("#advlastevent").val()),
                    LastEventDate: ValidateDate($("#advlasteventdate").val())
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#partTransferSearchTable thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.PartTransferId != null) {
                    item.PartTransferId = item.PartTransferId;
                }
                else {
                    item.PartTransferId = "";
                }
                if (item.RequestSite_Name != null) {
                    item.RequestSite_Name = item.RequestSite_Name;
                }
                else {
                    item.RequestSite_Name = "";
                }
                if (item.RequestPart_ClientLookupId != null) {
                    item.RequestPart_ClientLookupId = item.RequestPart_ClientLookupId;
                }
                else {
                    item.RequestPart_ClientLookupId = "";
                }
                if (item.Quantity != null) {
                    item.Quantity = item.Quantity;
                }
                else {
                    item.Quantity = "";
                }
                if (item.Status != null) {
                    item.Status = item.Status;
                }
                else {
                    item.Status = "";
                }
                if (item.RequestPart_Description != null) {
                    item.RequestPart_Description = item.RequestPart_Description;
                }
                else {
                    item.RequestPart_Description = "";
                }
                if (item.IssueSite_Name != null) {
                    item.IssueSite_Name = item.IssueSite_Name;
                }
                else {
                    item.IssueSite_Name = "";
                }
                if (item.IssuePart_ClientLookupId != null) {
                    item.IssuePart_ClientLookupId = item.IssuePart_ClientLookupId;
                }
                else {
                    item.IssuePart_ClientLookupId = "";
                }
                if (item.Reason != null) {
                    item.Reason = item.Reason;
                }
                else {
                    item.Reason = "";
                }
                if (item.LastEvent != null) {
                    item.LastEvent = item.LastEvent;
                }
                else {
                    item.LastEvent = "";
                }
                if (item.LastEventDate != null) {
                    item.LastEventDate = item.LastEventDate;
                }
                else {
                    item.LastEventDate = "";
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
                header: $("#partTransferSearchTable thead tr th div").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
})
//#endregion
//#region Advanced Search
$("#btnPTDataAdvSrch").on('click', function (e) {
        typeVal = $("#advstatus").val();
        PTAdvSearch();
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
        dropValStatus = $("#advstatus").val();
        DTpartTransferSearchTable.page('first').draw('page');
});
function PTAdvSearch() {

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
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    if (searchtxtId == "advstatus") {
        dropValStatus = null;
    }
    PTAdvSearch();
    DTpartTransferSearchTable.page('first').draw('page');
});
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    $('#SearchTextDropID').val('0').trigger('change.select2');
    ptStatus = 0;
    localStorage.removeItem("PARTTRANSFERGRIDDISPLAYSTATUS");
    clearAdvanceSearch();
    DTpartTransferSearchTable.page('first').draw('page');
});
function clearAdvanceSearch() {
    selectCount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    $('#advsearchsidebar').find("select").val("").trigger('change');
    dropValStatus = $("#advstatus").val();
    $(".filteritemcount").text(selectCount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
//#endregion Advanced Search
//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(DTpartTransferSearchTable, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(DTpartTransferSearchTable, colOrder);
    run = true;
    DTpartTransferSearchTable.state.save(run);
});
//#endregion
//#region Details
$(document).on('click', '.lnk_part_transfer', function (e) {
    e.preventDefault();
    var index_row = $('#partTransferSearchTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = DTpartTransferSearchTable.row(row).data();
    $.ajax({
        url: "/PartTransfer/GetPartTransferDetail",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { PartTransferId: data.PartTransferId },
        success: function (data) {
            $('#renderparttransferinfo').html(data);
        },
        complete: function () {
            SetPartTransferDetailEnvironment();
        }
    });
});
//#endregion
//#region Common
function SetPartTransferDetailEnvironment() {
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
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    $(document).find('.dtpicker1').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        autoclose: true
    });
    $(document).find('.select2picker').select2({});
    SetFixedHeadStyle();
}
//#endregion