
$(document).ready(function () {
    $(document).find('.select2picker').select2({});
    fxActiveTabSetting();   
});
function fxActiveTabSetting() {
    var activeTab = "";
    if ($('button[data-tab="PurchaseRequest"]').length > 0) {
        if ($('button[data-tab="PurchaseRequest"]').hasClass("active")) {
            activeTab = "PurchaseRequest";
        }
    }
    if ($('button[data-tab="WorkRequest"]').length > 0) {
        if ($('button[data-tab="WorkRequest"]').hasClass("active")) {
            activeTab = "WorkRequest";
        }
    }
    if ($('button[data-tab="MaterialRequest"]').length > 0) {
        if ($('button[data-tab="MaterialRequest"]').hasClass("active")) {
            activeTab = "MaterialRequest";
        }
    }
    fxloadcurrentTab(activeTab);
}

function fxloadcurrentTab(activeTab) {
    $(".tabcontent").removeAttr("style");
    $("#" + activeTab).css("display", "block");
    $(".Approval-det-tab").removeClass("active");
    $('button[data-tab="' + activeTab+'"]').addClass("active");
    fxLoadGridSwitchTab(activeTab);
};
//#region SwitchTab
function fxLoadGridSwitchTab(Tab) {
    switch (Tab) {
        case "PurchaseRequest":
            LoadPurchaseRequestTab();
            break;
        case "WorkRequest":
            LoadWorkRequestTab();
            break;
        case "MaterialRequest":
            LoadMaterialRequestTab();
            break;
    }
}

$(document).on('click', '.Approval-det-tab', function (e) {
    ResetOtherTabs();
    var tab = $(this).data('tab');
    fxLoadGridSwitchTab(tab);
    SwitchTab(e, tab);
});
function SwitchTab(evt, tab) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(tab).style.display = "block";

    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
}
function ResetOtherTabs() {
    $(document).find('#PurchaseRequestDiv').html('');
    $(document).find('#WorkRequestDiv').html('');
    $(document).find('#MaterialRequestDiv').html('');
}
//#endregion

//#region Export
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
        debugger
        var approverID = $(document).find('#hdnApporverId').val();
        var filterType; var info; var start; var lengthMenuSetting; var Order; var length; var orderdir; var d = [];
        var visiblecolumnsIndex;
        if (this.context.length) {
            if (options.gridtoexport == 'workrequest-search') {
                // WorkRequestExport
                filterType = $(document).find('#FilterTypeWR').val();
                workRequestTable = $("#workRequestSearchTable").DataTable();
                info = workRequestTable.page.info();
                start = info.start;
                lengthMenuSetting = info.length;
                Order = order;
                length = $('#workRequestSearchTable').dataTable().length;
                orderdir = orderDir;
                var jsonResult = $.ajax({
                    "url": "/Approval/GetApprovalRouteWRPrintData",
                    "type": "get",
                    "datatype": "json",
                    data: {
                        colorder: Order,
                        coldir: orderdir,
                        start: start,
                        length: lengthMenuSetting,
                        approverId: approverID,
                        filterType: filterType
                    },
                    success: function (result) {
                    },
                    async: false
                });
                var thisdata = JSON.parse(jsonResult.responseText).data;
                 visiblecolumnsIndex = $("#workRequestSearchTable thead tr th").map(function (key) {
                    return this.getAttribute('data-th-index');
                }).get();
                d = [];
                $.each(thisdata, function (index, item) {
                    if (item.ClientLookupId != null) {
                        item.ClientLookupId = item.ClientLookupId;
                    }
                    else {
                        item.ClientLookupId = "";
                    }
                    if (item.ChargeTo != null) {
                        item.ChargeTo = item.ChargeTo;
                    }
                    else {
                        item.ChargeTo = "";
                    }
                    if (item.ChargeToName != null) {
                        item.ChargeToName = item.ChargeToName;
                    }
                    else {
                        item.ChargeToName = "";
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
                    if (item.Date != null) {
                        item.Date = item.Date;
                    }
                    else {
                        item.Date = "";
                    }
                    if (item.Comments != null) {
                        item.Comments = item.Comments;
                    }
                    else {
                        item.Comments = "";
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
                    header: $("#workRequestSearchTable thead tr th").find('div').map(function (key) {
                        return this.innerHTML;
                    }).get()
                };
            }
            else if (options.gridtoexport == 'purchaserequest-search') {
                //ExportPurchaseRequestGrid;
                filterType = $(document).find('#FilterTypePR').val();
                dtPurchaseRequestSearchTable = $("#PurchaseRequestSearchTable").DataTable();
                info = dtPurchaseRequestSearchTable.page.info();
                start = info.start;
                lengthMenuSetting = info.length;
                Order = order;
                length = $('#PurchaseRequestSearchTable').dataTable().length;
                orderdir = orderDir;
                var jsonResult = $.ajax({
                    "url": "/Approval/GetApprovalRoutePRPrintData",
                    "type": "get",
                    "datatype": "json",
                    data: {
                        colorder: Order,
                        coldir: orderDir,
                        start: start,
                        length: lengthMenuSetting,
                        approverId: approverID,
                        filterType: filterType
                    },
                    success: function (result) {
                    },
                    async: false
                });
                var thisdata = JSON.parse(jsonResult.responseText).data;
                 visiblecolumnsIndex = $("#PurchaseRequestSearchTable thead tr th").map(function (key) {
                    return this.getAttribute('data-th-index');
                }).get();
                d = [];
                $.each(thisdata, function (index, item) {
                    if (item.ClientLookupId != null) {
                        item.ClientLookupId = item.ClientLookupId;
                    }
                    else {
                        item.ClientLookupId = "";
                    }
                    if (item.VendorName != null) {
                        item.VendorName = item.VendorName;
                    }
                    else {
                        item.VendorName = "";
                    }
                    if (item.Date != null) {
                        item.Date = item.Date;
                    }
                    else {
                        item.Date = "";
                    }
                    if (item.Comments != null) {
                        item.Comments = item.Comments;
                    }
                    else {
                        item.Comments = "";
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
                    header: $("#PurchaseRequestSearchTable thead tr th").map(function (key) {
                        return this.innerHTML;
                    }).get()
                };
            }
            else {
                filterType = $(document).find('#FilterTypeMR').val();
                MaterialRequestSearchGridTable = $("#Tbl_materialRequestSearchGrid").DataTable();
                info = MaterialRequestSearchGridTable.page.info();
                start = info.start;
                lengthMenuSetting = info.length;
                Order = order;
                length = $('#Tbl_materialRequestSearchGrid').dataTable().length;
                orderdir = orderDir;
                var jsonResult = $.ajax({
                    "url": "/Approval/GetApprovalRouteMRPrintData",
                    "type": "get",
                    "datatype": "json",
                    data: {
                        colorder: Order,
                        coldir: orderDir,
                        start: start,
                        length: lengthMenuSetting,
                        filterType: filterType
                    },
                    success: function (result) {
                    },
                    async: false
                });
                var thisdata = JSON.parse(jsonResult.responseText).data;
                 visiblecolumnsIndex = $("#Tbl_materialRequestSearchGrid thead tr th").map(function (key) {
                    return this.getAttribute('data-th-index');
                }).get();
                d = [];
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
                    if (item.Quantity != null) {
                        item.Quantity = item.Quantity;
                    }
                    else {
                        item.Quantity = "";
                    }
                    if (item.TotalCost != null) {
                        item.TotalCost = item.TotalCost;
                    }
                    else {
                        item.TotalCost = "";
                    }
                    if (item.Date != null) {
                        item.Date = item.Date;
                    }
                    else {
                        item.Date = "";
                    }
                    if (item.Comments != null) {
                        item.Comments = item.Comments;
                    }
                    else {
                        item.Comments = "";
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
                    header: $("#Tbl_materialRequestSearchGrid thead tr th").map(function (key) {
                        return this.innerHTML;
                    }).get()
                };
            }
        }
    });
});
//#endregion