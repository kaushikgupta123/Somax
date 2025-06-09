//#region Common 
var dtReceiptDataTable;
var dtinnerGrid;
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../PurchaseOrderReceipt/Index?page=Procurement_Receipts";
});//V2-331
//#endregion
function GenerateReceiptHistoryGrid(PurchaseOderId) {
    ShowLoader();
    if ($(document).find('#tblLineItemHistory').hasClass('dataTable')) {
        dtReceiptDataTable.destroy();
    }
    dtReceiptDataTable = $(document).find("#tblLineItemHistory").DataTable({
        searching: false,
        paging: false,
        info: false,
        "bProcessing": true,
        "bDeferRender": true,
        language: {
            url: dataTableLocalisationUrl
        },
        sDom: 'Btlipr',
        "order": [[1, "asc"]],
        "ajax": {
            "url": "/PurchaseOrderReceipt/PopulateReceiptHistoryGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.PurchaseOderId = PurchaseOderId;
            }
        },
        "columns":
            [
                {
                    "data": "PurchaseOrderLineItemId",
                    "bVisible": true,
                    "bSortable": false,
                    "autoWidth": false,
                    "bSearchable": false,

                    "mRender": function (data, type, row) {
                        return '<img id="' + data + '" src="../../Images/details_open.png" alt="expand/collapse" rel="' + data + '" style="cursor: pointer;"/>';
                    }
                },
                { "data": "LineNumber", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-100'>" + data + "</div>";
                    }
                },
                { "data": "OrderQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "UnitOfMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "QuantityToDate", "autoWidth": true, "bSearchable": true, "bSortable": false },
                { "data": "Status_Display", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "QuantityBackOrdered", "autoWidth": true, "bSearchable": true, "bSortable": false }
            ],
        "columnDefs": [
            { width: "10px", targets: 0 },
            { className: 'text-right', targets: [4, 6, 8] }
        ],
        initComplete: function () {
            $('.dt-buttons').html('');
            $(document).find('#tblLineItemHistory').on('click', 'tbody td img', function (e) {
                var tr = $(this).closest('tr');
                var row = dtReceiptDataTable.row(tr);
                if (this.src.match('details_close')) {
                    this.src = "../../Images/details_open.png";
                    row.child.hide();
                    tr.removeClass('shown');
                }
                else {
                    this.src = "../../Images/details_close.png";
                    var purchaseOrderLineItemId = $(this).attr("rel");
                    ShowLoader();
                    $.get("/PurchaseOrderReceipt/POInnerGrid/?PurchaseOrderLineItemId=" + purchaseOrderLineItemId, function (InnerGridLineItemModel) {
                        row.child(InnerGridLineItemModel).show();
                        dtinnerGrid = row.child().find('.tblLineItemHistory-child').DataTable(
                            {
                                "order": [[1, "asc"]],
                                paging: false,
                                searching: false,
                                "bProcessing": true,
                                responsive: true,
                                scrollY: 300,                               
                                scrollX: true,//V2-331
                                "scrollCollapse": true,
                                sDom: 'tlipr',
                                language: {
                                    url: "/base/GetDataTableLanguageJson?nGrid=" + true
                                },
                                buttons: [],
                                "columnDefs": [                                
                                    {
                                        "targets": [13, 14, 15, 16, 17],
                                        "visible": false,
                                    }
                                    ,
                                    {
                                        "targets": [0, 7, 9],
                                        "orderable": false,
                                    },
                                    { className: 'text-left', targets: [0, 1, 2, 3, 4, 5, 8, 10, 11] },
                                    { className: 'text-right', targets: [6, 7, 12] },
                                    { className: 'text-center', targets: [9] }

                                ],
                                initComplete: function () {
                                    var IsExternal = $(document).find('#PurchaseOrderModel_IsExternal').val();
                                    if (IsExternal == "True") {
                                        this.api().column(0).visible(false);
                                    }
                                    else {
                                        this.api().column(0).visible(true);
                                    }
                                    CloseLoader();
                                },
                                select: {
                                    style: 'os',
                                    selector: 'td:nth-child(6)'
                                }
                            });

                        tr.addClass('shown');
                    });
                }
            });
        }
    });
    CloseLoader();
}
$(document).on('click', '#btnSaveReversedComments', function () {
    var BodyComments = $('#txtReversedcomments').val();
    if (!BodyComments) {
        var message = getResourceValue("EnterCommentsAlert");
        ShowTextMissingCommonAlert(message);
        return false;
    }
    UpdatePurchaseRequestStatus(BodyComments, RecieptSelectedItemArray);
    $('#txtReversedcomments').val('');
});
function UpdatePurchaseRequestStatus(BodyComments, datasource1) {
    var GlobalReceiptNumber;
    RecieptSelectedItemArray = [];
    $.each(datasource1, function (index, item) {
        var POReceiptItemId = item.POReceiptItemId;
        var POReceiptHeaderId = item.POReceiptHeaderId;
        var PurchaseOrderLineItemId = item.PurchaseOrderLineItemId;
        GlobalReceiptNumber = item.ReceiptNumber;
        var ReceiveBy_PersonnelID = item.ReceiveBy_PersonnelID;
        var QuantityReceived = item.QuantityReceived;
        var UOMConversion = item.UOMConversion;
        var ChargeType = item.ChargeType;
        var ChargeToId = item.ChargeToId;
        var comments = BodyComments;
        var item = new PartNotInInventorySelectedItem(GlobalReceiptNumber, POReceiptItemId,
            POReceiptHeaderId,
            PurchaseOrderLineItemId,
            ReceiveBy_PersonnelID,
            QuantityReceived,
            comments,
            UOMConversion,
            ChargeType,
            ChargeToId
        );
        RecieptSelectedItemArray.push(item);
    });
    var list = JSON.stringify({ 'list': RecieptSelectedItemArray });
    $.ajax({
        url: "/PurchaseOrderReceipt/UpdateRevisedComments",
        type: "POST",
        dataType: "json",
        data: list,
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('.modal').modal('hide');
            $('.modal-backdrop').hide();
            if (data == "success") {
                SuccessAlertSetting.text = getResourceValue("alertupdatedsuccessfully");
                swal(SuccessAlertSetting, function () {
                    RedirectDetail($(document).find('#PurchaseOrderModel_PurchaseOrderId').val(), "receipthistory");
                    //$(document).find('#tabReceiptHistorySidebar').trigger('click');
                    //RefreshLineGrid(GlobalReceiptNumber, BodyComments);
                });
            } else {
                GenericSweetAlertMethod(data);
            }
        },
        complete: function () {
            CloseLoader();
        }
    });
};
function RefreshLineGrid(GlobalReceiptNumber, BodyComments) {
    dtinnerGrid
        .column(1)
        .data()
        .each(function (value, index) {
            if (value == GlobalReceiptNumber) {
                $(".currenttr tbody").parent().find('tr').eq(index + 1).find("td:nth-child(7)").find('input[type="checkbox"]').prop('checked', true);
                $(".currenttr tbody").parent().find('tr').eq(index + 1).find("td:nth-child(8)").text(BodyComments);
                var appLink = '<td class="text-center sorting_1"><a  class="nolink" href="javascript:void(0)">Reverse</a></td>';
                $(".currenttr tbody").parent().find('tr').eq(index + 1).find("td:nth-child(1)").replaceWith(appLink);
            }
        });
    $('#tblLineItemHistory').find('.currenttr').removeClass('currenttr');
}
var RecieptSelectedItemArray = [];
$(document).on("click", ".binddataClass", function () {   
    RecieptSelectedItemArray = [];
    var ReceiptNumber = $(this).data('todo').ReceiptNumber;
    var POReceiptItemId = $(this).data('todo').POReceiptItemId;
    var POReceiptHeaderId = $(this).data('todo').POReceiptHeaderId;
    var PurchaseOrderLineItemId = $(this).data('todo').PurchaseOrderLineItemId;
    var ReceiveBy_PersonnelID = $(this).data('todo').ReceiveBy_PersonnelID;
    var QuantityReceived = $(this).data('todo').QuantityReceived;
    var UOMConversion = $(this).data('todo').UOMConversion;
    var ChargeType = $(this).data('todo').ChargeType;
    var ChargeToId = $(this).data('todo').ChargeToId;
    var comments = '';
    var item = new PartNotInInventorySelectedItem(
        ReceiptNumber,
        POReceiptItemId,
        POReceiptHeaderId,
        PurchaseOrderLineItemId,
        ReceiveBy_PersonnelID,
        QuantityReceived,
        comments,
        UOMConversion,
        ChargeType,
        ChargeToId,
        "currenttr"
    );
    $(this).parents('table').eq(0).addClass('currenttr');
    RecieptSelectedItemArray.push(item);
});
function PartNotInInventorySelectedItem(ReceiptNumber, POReceiptItemId,
    POReceiptHeaderId,
    PurchaseOrderLineItemId,
    ReceiveBy_PersonnelID,
    QuantityReceived, comments, UOMConversion, ChargeType, ChargeToId,
    currentparenttr) {   
    this.ReceiptNumber = ReceiptNumber;
    this.POReceiptItemId = POReceiptItemId;
    this.POReceiptHeaderId = POReceiptHeaderId;
    this.PurchaseOrderLineItemId = PurchaseOrderLineItemId;
    this.ReceiveBy_PersonnelID = ReceiveBy_PersonnelID;
    this.QuantityReceived = QuantityReceived;
    this.Comments = comments;
    this.UOMConversion = UOMConversion;
    this.ChargeType = ChargeType;
    this.ChargeToId = ChargeToId;
    this.currentparenttr = currentparenttr;
};

function POReceiptQROnSuccess(data) {
    CloseLoader();
    if (data.success === 0) {
        var smallLabel = $('#SmallLabel').prop('checked');
        //window.open('/PurchaseOrderReceipt/QRCodeGenerationUsingRotativa?SmallLabel=' + encodeURI(smallLabel), '_blank');
        window.open('/PurchaseOrderReceipt/QRCodeGenerationUsingDevExpress?SmallLabel=' + encodeURI(smallLabel), '_blank');

        $('#printPOReceiptQrCodeModal').modal('hide');
    }
}

