var RecieptSelectedItemArray = [];
var dtReceiptDataTable;
var count = 0;
var deletedRow;
var thisElement;
var StoreroomId = "";
var StoreroomName = "";
$(function () {
    $(document).find('.select2picker').select2({});
    $('#receiptModel_UnitCost').val(0);
    $('#receiptModel_ReceiptQuantity').val(1);
   /* LoadGetPartLookDropDown();*/   //Unused code 
    if ($(document).find('#MultiStoreroom').val() == "True")
    {
        $(document).find('#openpartgrid').hide();
    }
    $(document).on('change', '.ddlStoreroom', function () {
        $(document).find('#txtPartId').val('');
        if ($(this).val() != '') {
            $(document).find('#openpartgrid').show();
        } else {
            $(document).find('#openpartgrid').hide();
        }
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
//function LoadGetPartLookDropDown() {
//    $.ajax({
//        url: "/InventoryReceipts/GetThisPartLookUpList",
//        type: "GET",
//        dataType: 'json',
//        beforeSend: function () {
//            ShowLoader();
//        },
//        success: function (data) {
//            var partLookUpDropDown = $(document).find('#receiptModel_PartClientLookupId');
//            partLookUpDropDown.empty();
//            partLookUpDropDown.append("<option value=''>" + "--Select--" + "</option>");
//            if (data.length > 0) {
//                for (var i = 0; i < data.length; i++) {
//                    partLookUpDropDown.append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
//                }
//            }
//        },
//        complete: function () {
//            CloseLoader();
//        },
//        error: function () {
//            CloseLoader();
//        }
//    });
//}
/*$(document).on('change', "#receiptModel_PartClientLookupId", function () {
    var tlen = $(document).find("#receiptModel_PartClientLookupId").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#receiptModel_PartClientLookupId").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#receiptModel_PartClientLookupId").removeClass("input-validation-error");
    }
    else {
        var areaddescribedby = $(document).find("#receiptModel_PartClientLookupId").attr('aria-describedby');
        $('#' + areaddescribedby).show();
        $(document).find('form').find("#receiptModel_PartClientLookupId").addClass("input-validation-error");
    }
});*/
function ValidationOnSuccess(data) {   
    if (data.data === "success") {
        ResetErrorDiv();
        count++;
        var _PartClientLookupId = LRTrim($("#txtPartId").val());
        var _UnitCost = LRTrim($("#receiptModel_UnitCost").val());
        var _ReceiptQuantity = LRTrim($("#receiptModel_ReceiptQuantity").val());
        if ($(document).find(".ddlStoreroom").length > 0) {
            StoreroomId = $(document).find(".ddlStoreroom").val();
            StoreroomName = $(document).find(".ddlStoreroom").find("option:selected").text();
        } else {
            StoreroomId = 0;
            StoreroomName = "";
        }
        $.ajax({
            url: "/InventoryReceipts/PopulateGrid",
            type: "POST",
            dataType: "json",
            data: { PartClientLookupId: _PartClientLookupId, UnitCost: _UnitCost, ReceiptQuantity: _ReceiptQuantity, count: count, StoreroomId: StoreroomId, StoreroomName: StoreroomName },
            success: function (data) {
                if (!data.data.Errormsg) {
                    data.data.errorListInString = "";
                }
                var item = new PartNotInInventorySelectedItem(
                    count,
                    data.data.PartId,
                    data.data.PartClientLookupId,
                    data.data.PartUPCCode,
                    data.data.Description,
                    data.data.UnitCost,
                    data.data.PartAverageCost,
                    data.data.ReceiptQuantity,
                    /*data.data.errorListInString,*/
                    data.data.StoreroomId,
                    data.data.StoreroomName
                );
                RecieptSelectedItemArray.push(item);
                var trHTML = '';
                trHTML +=
                    '<tr><td>' + count +
                    '</td><td>' + data.data.PartId +
                    '</td><td>' + data.data.StoreroomName +
                    '</td><td>' + data.data.PartClientLookupId +
                    '</td><td>' + data.data.PartUPCCode +
                    '</td><td>' + data.data.Description +
                    '</td><td>' + data.data.UnitCost +
                    '</td><td>' + data.data.PartAverageCost +
                    '</td><td>' + data.data.ReceiptQuantity +
                    '</td><td>' + '<a class="btn btn-outline-danger btndeletereceipt gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>' +
                    '</td><td>' + data.data.errorListInString +
                    '</td><td>' + data.data.StoreroomId +
                    '</td></tr>';
                $("#tblfinalSelectReceiptsGrid").dataTable().fnDestroy();
                $('#tblfinalSelectReceiptsGrid').append(trHTML);
            },
            complete: function () {
                ScrollToId("btnselectreceipt");
                dtReceiptDataTable = $("#tblfinalSelectReceiptsGrid").DataTable({
                    colReorder: true,
                    rowGrouping: true,
                    searching: true,
                    "pagingType": "full_numbers",
                    "bProcessing": true,
                    language: {
                        url: "/base/GetDataTableLanguageJson?nGrid=" + true,
                    },
                    "bDeferRender": true,
                    "order": [[0, "asc"]],
                    sDom: 'Btipr',
                    buttons: [],
                    "orderMulti": true,
                    "columnDefs": [
                        {
                            "targets": [0, 1,11],
                            "visible": false,
                        },
                        {
                            "targets": [9],
                            "orderable": false,
                            "className": "text-center"
                        },
                        //{ className: 'text-center', targets: [8] },
                    ],
                    initComplete: function () {
                        if ($(document).find('#MultiStoreroom').val() != "True") {
                            dtReceiptDataTable.columns(2).visible(false);
                        }
                    }
                });
                $('#finselectcontainer').show();
               /* $('#receiptModel_PartClientLookupId').val($('#receiptModel_PartClientLookupId > option:first').val()).trigger('change');*/
                $('#receiptModel_UnitCost').val(0);
                $('#receiptModel_ReceiptQuantity').val(1);
                $(document).find("#receiptModel_PartClientLookupId").val("");
                //var areaddescribedby = $(document).find("#receiptModel_PartClientLookupId").attr('aria-describedby');
                //$('#' + areaddescribedby).hide();
                //$(document).find('form').find("#receiptModel_PartClientLookupId").removeClass("input-validation-error");
               
                if ($(document).find('#MultiStoreroom').val() == "True") {
                    $(document).find("#receiptModel_StoreroomId").val("").trigger('change');
                    var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
                    $('#' + areaddescribedby).hide();
                    $(document).find('form').find(".ddlStoreroom").removeClass("input-validation-error");
                }
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
    else {
        CloseLoader();
        ShowGenericErrorOnAddUpdate(data);      
    }
}
function PartNotInInventorySelectedItem(count, PartId, PartClientLookupId, PartUPCCode, Description, UnitCost, PartAverageCost, ReceiptQuantity, StoreroomId, StoreroomName) {
    this.count = count;
    this.PartId = PartId;
    this.PartClientLookupId = PartClientLookupId;
    this.PartUPCCode = PartUPCCode;
    this.Description = Description;
    this.UnitCost = UnitCost;
    this.PartAverageCost = PartAverageCost;
    this.ReceiptQuantity = ReceiptQuantity;
    this.StoreroomId = StoreroomId;
    this.StoreroomName = StoreroomName;
};
$(document).on('click', "#btnconfirm", function () {
    if (RecieptSelectedItemArray.length < 1) {
        swal({
            title: getResourceValue("CommonErrorAlert"),
            text: getResourceValue("additeningrid"),
            type: "error",
            showCancelButton: false,
            confirmButtonClass: "btn-sm btn-danger",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("SaveAlertOk"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        }, function () {

        });
        return false;
    }
    else
    {
        GeneratedfinalSelectPartsTable(RecieptSelectedItemArray);
    }
});
function GeneratedfinalSelectPartsTable(datasource1) {
    RecieptSelectedItemArray = [];
    $.each(datasource1, function (index, item) {
        var count1 = item.count;
        var PartId = item.PartId;
        var PartClientLookupId = item.PartClientLookupId;
        var PartUPCCode = item.PartUPCCode;
        var Description = item.Description;
        var UnitCost = item.UnitCost;
        var PartAverageCost = item.PartAverageCost;
        var ReceiptQuantity = item.ReceiptQuantity;
        var _StoreroomId = item.StoreroomId;
        var _StoreroomName = item.StoreroomName;
        var item = new PartNotInInventorySelectedItem(count1, PartId, PartClientLookupId, PartUPCCode, Description, UnitCost, PartAverageCost, ReceiptQuantity, _StoreroomId, _StoreroomName);
        RecieptSelectedItemArray.push(item);
    });
    var list = JSON.stringify({ 'list': RecieptSelectedItemArray });
    $.ajax({
        url: "/InventoryReceipts/SaveListRecieptFromGrid",
        type: "POST",
        dataType: "json",
        data: list,
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                RecieptSelectedItemArray = [];
                FinalGridSelectedItemArray = [];
                SuccessAlertSetting.text = getResourceValue("ReceiptAddAlert");
                swal(SuccessAlertSetting, function () {
                    funcRefreshPage();
                });
                $("#txtPartId").val("");
                if ($(document).find('#MultiStoreroom').val() == "True") {
                    $(document).find("#receiptModel_StoreroomId").val("").trigger('change');
                    var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
                    $('#' + areaddescribedby).hide();
                    $(document).find("#receiptModel_StoreroomId").removeClass("input-validation-error");
                   
                }
            }
            else {
                funcErrorGridPage(data);
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
$(document).on('click', '.btndeletereceipt', function () {
    thisElement = dtReceiptDataTable.row($(this).parents('tr'));
    deletedRow = dtReceiptDataTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        dtReceiptDataTable.row(thisElement).remove().draw();
        RecieptSelectedItemArray = RecieptSelectedItemArray.filter(function (el) {
            return el.count != deletedRow[0];
        });
    });
});
$(document).on('click', '#btncancel', function () {
    swal(CancelAlertSetting, function () {
        funcRefreshPage()
    });
});
function funcRefreshPage() {
    count = 0;
    $('#finselectcontainer').hide();
    RecieptSelectedItemArray = [];
    $('#receiptModel_PartClientLookupId').val("");//val($('#receiptModel_PartClientLookupId > option:first').val()).trigger('change');
    $('#receiptModel_UnitCost').val(0);
    $('#receiptModel_ReceiptQuantity').val(1);
    if ($(document).find('#MultiStoreroom').val() == "True") {
        $(document).find("#receiptModel_StoreroomId").val("").trigger('change');
        var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find("#receiptModel_StoreroomId").removeClass("input-validation-error");
    }
    dtReceiptDataTable.clear().draw();
    
    //var areaddescribedby = $(document).find("#receiptModel_PartClientLookupId").attr('aria-describedby');
    //$('#' + areaddescribedby).hide();
    //$(document).find('form').find("#receiptModel_PartClientLookupId").removeClass("input-validation-error");
}
function funcErrorGridPage(data) {
    $.each(data.Result, function (index, el) {
        if (index == 0) {
            $.each(el.SuccListId, function (index, el2) {
                var filteredData = dtReceiptDataTable
                    .rows()
                    .indexes()
                    .filter(function (value, index) {
                        return dtReceiptDataTable.row(value).data()[1] == el2.SuccpartId;
                    });
                dtReceiptDataTable.rows(filteredData)
                    .remove()
                    .draw();

                RecieptSelectedItemArray = RecieptSelectedItemArray.filter(function (el) {
                    return el.PartId != el2.SuccpartId;
                });
            });
        }
    });
    dtReceiptDataTable
        .column(1)
        .data()
        .each(function (value, index) {
            $.each(data.Result, function (index1, el) {
                if (el.PartId == value) {
                    $("#tblfinalSelectReceiptsGrid tbody").parent().find('tr').eq(index + 1).find("td:last-child").text(el.Errormsg);
                }
            });
        });
}

