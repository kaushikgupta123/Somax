var dtPurchaseRequestSearchTable;
var dtLineItemTable;
var order = '1';
var orderDir = 'asc';
function LoadPurchaseRequestTab() {
    PurchaseRequestTab();
}
function PurchaseRequestTab() {
    $.ajax({
        url: '/Approval/LoadPurchaseRequest',
        type: 'POST',
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data) {
                $(document).find('#PurchaseRequestDiv').html(data);
            }
        },
        complete: function () {
            GeneratePRApprovalDatatable();
            CloseLoader();
        },
        error: function (err) {
            CloseLoader();
        }
    });
}
function GeneratePRApprovalDatatable() {
    var approverID = $(document).find('#hdnApporverId').val();
    if ($(document).find('#PurchaseRequestSearchTable').hasClass('dataTable')) {
        dtPurchaseRequestSearchTable.destroy();
    }
    dtPurchaseRequestSearchTable = $("#PurchaseRequestSearchTable").DataTable({
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
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Purchase Request List',
                orientation: 'landscape',
                pageSize: 'A2',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'purchaserequest-search'
                }
            },
            {
                extend: 'print',
                title: 'Purchase Request List',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'purchaserequest-search'
                }
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Purchase Request List',
                extension: '.csv',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'purchaserequest-search'
                }
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                customize: function (doc) {
                    doc.defaultStyle.alignment = 'left';
                    doc.styles.tableHeader.alignment = 'left';
                },
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'purchaserequest-search'
                },
                orientation: 'landscape',
                pageSize: 'A4',
                title: 'Purchase Request List'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Approval/GetPRApprovalGridData",
            "type": "POST",
            data: {
                ApproverID: approverID,
                FilterType: $('#FilterTypePR').val()
            },
            "datatype": "json",
            "dataSrc": function (json) {
                let colOrder = dtPurchaseRequestSearchTable.order();
                orderDir = colOrder[0][1];
                order = colOrder[0][0];
                if (json.data.length < 1) {
                    $(document).find('.import-export').prop('disabled', true);
                    $(document).find('#spnNumberOfPurchaseRequests').removeClass("badge badge-light rounded-circle tabBadge").text("");
                }
                else {
                    $(document).find('.import-export').prop('disabled', false);
                    if ($(document).find('#spnNumberOfPurchaseRequests').hasClass("tabBadge")) {
                        $(document).find('#spnNumberOfPurchaseRequests').text(json.recordsTotal);
                    } else {
                        $(document).find('#spnNumberOfPurchaseRequests').addClass("badge badge-light rounded-circle tabBadge").text(json.recordsTotal);
                    }

                }

                return json.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, full, row) {
                        return '<div  class="width-100"><a class="lnk_puchaserequest" href="javascript:void(0)">' + data + '</a></div>'
                    }
                },
                { "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Date", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Comments", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        },
    });
}
$(document).on('click', '.lnk_puchaserequest', function (e) {
    var PurchaseRequestId;
    var row = $(this).parents('tr');
    var data = dtPurchaseRequestSearchTable.row(row).data();
    PurchaseRequestId = data.PurchaseRequestId;
    var PurchaseRequestClientlookup = data.ClientLookupId;
    $(".lnk_puchaserequest").parents('tr').removeAttr("style");
    row.css("background-color", "#eceff1");
    var ApprovalGroupId = data.ApprovalGroupId;
    $(document).find('#hdnPurchaseRequestId').val(PurchaseRequestId);
    $.ajax({
        url: "/Approval/PurchaseRequestViewDetails",
        type: "POST",
        data: { PurchaseRequestId: PurchaseRequestId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('.PurchaseRequestView').html(data);
            $("#txtPRclientlookup").text(PurchaseRequestClientlookup);
            $(document).find('#hdnApprovalGroupId').val(ApprovalGroupId);
            $(document).find('#hdnClientLookupId').val(PurchaseRequestClientlookup);
            generateLineiItemdataTable(PurchaseRequestId);
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});

function generateLineiItemdataTable(PurchaseRequestId) {
    var rCount = 0;
    var srcData = 0;
    var IsShoppingCart = false;

    if ($(document).find('#tblLineItem').hasClass('dataTable')) {
        dtLineItemTable.destroy();
    }
    dtLineItemTable = $("#tblLineItem").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "bProcessing": true,
        "order": [[0, "asc"]],
        stateSave: false,
        "pagingType": "full_numbers",
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Approval/PopulateLineItem",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.PurchaseRequestId = PurchaseRequestId;
            },
            "dataSrc": function (result) {
                rCount = result.data.length;
                IsShoppingCart = result.IsShoppingCart;
                return result.data;
            },
            global: true
        },
        "columns":
            [
                { "data": "LineNumber", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "OrderQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "PurchaseUOM", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right",
                    "mRender": function (data, type, row) {
                        return data.toFixed(2);
                    }
                },
                {
                    "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right",
                    "mRender": function (data, type, row) {
                        return data.toFixed(2);
                    }
                },
                { "data": "Account_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "RequiredDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date",
                    render: function (data, type, row, meta) {
                        if (data == null) {
                            return '';
                        } else {
                            return data;
                        }
                    }
                },
                { "data": "ChargeToClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true }
                /*{ "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }*/

            ],
        "footerCallback": function (row, data, start, end, display) {

            var api = this.api();
            var rows = api.rows().nodes();
            var getData = api.rows({ page: 'current' }).data();
            // Total over all pages
            total = api.column(6).data().reduce(function (a, b) {
                return parseFloat(a) + parseFloat(b);
            }, 0);
            // Update footer
            if (data.length != 0) {
                var footer = "";
                if (IsShoppingCart == true)
                    footer = '<tr><th></th><th></th><th></th><th></th><th style="text-align: left!important; font-weight: 500!important; color:#0b0606!important">Grand Total</th><th style = "text-align: right!important; font-weight: 500!important; color: #0b0606!important; padding: 0px 10px 0px 0px!important" >' + total.toFixed(2) + '</th><th></th><th></th><th></th></tr>'
                else
                    footer = '<tr><th></th><th></th><th></th><th></th><th style="text-align: left!important; font-weight: 500!important; color:#0b0606!important">Grand Total</th><th style = "text-align: right!important; font-weight: 500!important; color: #0b0606!important; padding: 0px 10px 0px 0px!important" >' + total.toFixed(2) + '</th><th></th><th></th></tr>'
                $("#tblLineItemfoot").empty().append(footer);
            }
        },
        initComplete: function (settings, json) {
            /* var column = this.api().column(10);*/
            var api = new $.fn.dataTable.Api(settings);
            api.column(8).visible(IsShoppingCart);

            SetPageLengthMenu();
            //----------conditional column hiding-------------//
            var api = new $.fn.dataTable.Api(settings);
            var columns = dtLineItemTable.settings().init().columns;
            var arr = [];
            var j = 0;
            while (j < json.hiddenColumnList.length) {
                var clsname = '.G' + json.hiddenColumnList[j];
                dtLineItemTable.columns(clsname).visible(false);
                //---hide adv search items---
                var advclsname = '.' + "prcli-" + json.hiddenColumnList[j];
                $(document).find(advclsname).hide();
                j++;
            }
            //----------------------------------------------//
        }
    });
}

//#region Approve ans Deny
$(document).on('click', '#btnPurchaserequestApprove', function () {
    swal({
        title: getResourceValue("CancelAlertSure"),
        text: getResourceValue("spnPleaseConfirmToApprove"),
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        var PurchaseRequestId = $(document).find('#hdnPurchaseRequestId').val();
        var ApprovalGroupId = $(document).find('#hdnApprovalGroupId').val();
        var clientLookupId = $(document).find("#hdnClientLookupId").val();

        MultiLevelApprovePR(PurchaseRequestId, ApprovalGroupId, clientLookupId)

    });
});

function MultiLevelApprovePR(PurchaseRequestId, ApprovalGroupId, clientLookupId) {
    $.ajax({
        url: '/Approval/MultiLevelApprovePR',
        data: {
            PurchaseRequestId: PurchaseRequestId, ApprovalGroupId: ApprovalGroupId, ClientLookupId: clientLookupId
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
                    //console.log(ApproverObjList);
                    $.ajax({
                        url: "/Approval/SendPRForMultiLevelApproval",
                        type: "POST",
                        dataType: 'html',
                        data: { Approvers: data.ApproverList, PurchaseRequestId: PurchaseRequestId, ApprovalGroupId: ApprovalGroupId },
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
                        url: "/Approval/MultiLevelFinalApproveForPurchaseRequest",
                        type: "GET",
                        datatype: "json",
                        data: { PurchaseRequestId: PurchaseRequestId, ApprovalGroupId: ApprovalGroupId },
                        beforeSend: function () {
                            ShowLoader();
                        },
                        success: function (data) {
                            //$(document).find('#MultiLevelApproverListModalpopup').modal('hide');
                            var PurchaseRequestId = data.PurchaseRequestId;
                            if (data.Result === "success") {
                                if (data.ApprovalGroupId >= 0) {
                                    SuccessAlertSetting.text = getResourceValue("spnPuchaseRequestApprovedSuccessfully");
                                    swal(SuccessAlertSetting, function () {
                                        CloseLoader();

                                        fxloadcurrentTab("PurchaseRequest");
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

function SetControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
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
    SetFixedHeadStyle();
};
function SendPRForMultiLevelApprovalOnSuccess(data) {
    $(document).find('#MultiLevelApproverListModalpopup').modal('hide');
    var PurchaseRequestId = data.PurchaseRequestId;
    if (data.data === "success") {
        if (data.ApprovalGroupId >= 0) {
            SuccessAlertSetting.text = getResourceValue("SendApprovalSuccessAlert");
            swal(SuccessAlertSetting, function () {
                CloseLoader();
                fxloadcurrentTab("PurchaseRequest");
            });
        }

    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }

}

$(document).on('click', '#btnPurchaserequestDeny', function () {
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
        var PurchaseRequestId = $(document).find('#hdnPurchaseRequestId').val();
        var ApprovalGroupId = $(document).find('#hdnApprovalGroupId').val();
        var clientLookupId = $(document).find("#hdnClientLookupId").val();

        MultiLevelDenyPR(PurchaseRequestId, ApprovalGroupId, clientLookupId)

    });
});
function MultiLevelDenyPR(PurchaseRequestId, ApprovalGroupId, clientLookupId) {
    $.ajax({
        url: '/Approval/MultiLevelDenyPR',
        data: {
            PurchaseRequestId: PurchaseRequestId, ApprovalGroupId: ApprovalGroupId, ClientLookupId: clientLookupId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {

            if (data.data == "success") {
                SuccessAlertSetting.text = getResourceValue("alertPRDenied");
                swal(SuccessAlertSetting, function () {
                    fxloadcurrentTab("PurchaseRequest");
                });
            }
            else {
                //GenericSweetAlertMethod(getResourceValue("FailedAlert"));
                ErrorAlertSetting.text = getResourceValue("FailedAlert");
                swal(ErrorAlertSetting, function () {
                    fxloadcurrentTab("PurchaseRequest");
                });
            }
        },
        complete: function () {
            CloseLoader();
            SetControls();
        }
    });
}
$(document).on('click', '#btnreturntorequester', function () {
    var comments = LRTrim($('#txtPurchaseRequestbackcomments').val());
    if (!comments) {
        var message = getResourceValue("EnterCommentsAlert");
        ShowTextMissingCommonAlert(message);
        return false;
    }
    else {
        var PurchaseRequestId = $(document).find('#hdnPurchaseRequestId').val();
        var ApprovalGroupId = $(document).find('#hdnApprovalGroupId').val();
        var ClientLookupId = $(document).find('#hdnClientLookupId').val();
        var Comments = $(document).find("#txtPurchaseRequestbackcomments").val();
        ReturnToRequestorPR(PurchaseRequestId, ApprovalGroupId, Comments, ClientLookupId)
    }
});
function ReturnToRequestorPR(PurchaseRequestId, ApprovalGroupId, Comments, ClientLookupId) {
    $.ajax({
        url: '/Approval/ReturnPRToRequestor',
        data: {
            PurchaseRequestId: PurchaseRequestId, ApprovalGroupId: ApprovalGroupId, Comments: Comments, ClientLookupId: ClientLookupId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find("#returnTorequesterModal").modal('hide');
            if (data.data == "success") {
                SuccessAlertSetting.text = getResourceValue("PurchaseRequestReturnRequestorAlert");
                swal(SuccessAlertSetting, function () {
                    fxloadcurrentTab("PurchaseRequest");
                });
            }
            else {
                ErrorAlertSetting.text = getResourceValue("FailedAlert");
                swal(ErrorAlertSetting, function () {
                    fxloadcurrentTab("PurchaseRequest");
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