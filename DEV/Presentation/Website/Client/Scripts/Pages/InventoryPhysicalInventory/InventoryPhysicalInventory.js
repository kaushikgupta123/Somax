var InventorySelectedItemArray = [];
var dtInventoryDataTable;
var count = 0;
var deletedRow;
var thisElement;
var StoreroomId = "";
var StoreroomName = "";
$(function () {
    $(document).find('.select2picker').select2({});
    //LoadGetPartLookDropDown();
    if ($(document).find('#MultiStoreroom').val() == "True") {
        $(document).find('#openinvpartgrid').hide();
        $(document).find('#btnQrScanner').hide();
    }
    $(document).on('change', '.ddlStoreroom', function () {
        $(document).find('#txtPartId').val('');
        $(document).find('#hdnPartId').val('');
        if ($(this).val() != '') {
            $(document).find('#openinvpartgrid').show();
            $(document).find('#btnQrScanner').show();
        } else {
            $(document).find('#openinvpartgrid').hide();
            $(document).find('#btnQrScanner').hide();
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
/*function LoadGetPartLookDropDown() {
    $.ajax({
        url: "/InventoryPhysicalInventory/GetThisPartLookUpList",
        type: "GET",
        dataType: 'json',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var partLookUpDropDown = $(document).find('#inventoryModel_PartClientLookupId');
            partLookUpDropDown.empty();
            partLookUpDropDown.append("<option value=''>" + "--Select--" + "</option>");
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    partLookUpDropDown.append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
                }
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}*/
/*$(document).on('change', "#inventoryModel_PartClientLookupId", function () {
    var tlen = $(document).find("#inventoryModel_PartClientLookupId").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#inventoryModel_PartClientLookupId").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#inventoryModel_PartClientLookupId").removeClass("input-validation-error");
    }
    else {
        var areaddescribedby = $(document).find("#inventoryModel_PartClientLookupId").attr('aria-describedby');
        $('#' + areaddescribedby).show();
        $(document).find('form').find("#inventoryModel_PartClientLookupId").addClass("input-validation-error");
    }
});*/
function ValidationOnSuccess(data) {
    if (data.data == "success") {
        count++;
        var _PartClientLookupId = $("#txtPartId").val().trim();
        var _QuantityCounted = $("#inventoryModel_ReceiptQuantity").val().trim();
        if ($(document).find(".ddlStoreroom").length > 0) {
            StoreroomId = $(document).find(".ddlStoreroom").val();
            StoreroomName = $(document).find(".ddlStoreroom").find("option:selected").text();
        } else {
            StoreroomId = 0;
            StoreroomName = "";
        }
        $.ajax({
            url: "/InventoryPhysicalInventory/PopulateGrid",
            type: "POST",
            dataType: "json",
            data: { PartClientLookupId: _PartClientLookupId, QuantityCounted: _QuantityCounted, count: count, StoreroomId: StoreroomId, StoreroomName: StoreroomName},
            success: function (data) {
                if (data.Result == "success") {
                    var item = new PartNotInInventorySelectedItem(
                        count,
                        data.data.PartId,
                        data.data.PartClientLookupId,
                        data.data.Description,
                        data.data.PartUPCCode,
                        data.data.QtyOnHand,
                        data.data.QuantityCount,
                        data.data.PartStoreroomUpdateIndex,
                        data.data.StoreroomId,
                        data.data.StoreroomName
                    );
                    InventorySelectedItemArray.push(item);
                    var trHTML = '';
                    trHTML +=
                        '<tr><td>' + count +
                        '</td><td>' + data.data.StoreroomName +
                        '</td><td>' + data.data.PartClientLookupId +
                        '</td><td>' + data.data.Description +
                        '</td><td>' + data.data.PartUPCCode +
                        '</td><td>' + data.data.QtyOnHand +
                        '</td><td>' + data.data.QuantityCount +
                        '</td><td>' + data.data.Section +
                        '</td><td>' + data.data.Row +
                        '</td><td>' + data.data.Shelf +
                        '</td><td>' + data.data.Bin +
                        '</td><td>' + '<a class="btn btn-outline-danger btndeletereceipt gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>' +
                        '</td><td>' + data.data.StoreroomId +
                        '</td></tr>';
                    $("#tblfinalSelectReceiptsGrid").dataTable().fnDestroy();
                    ScrollToId("btnselectinventory");
                    $('#finselectcontainer').show();
                    dtInventoryDataTable = $("#tblfinalSelectReceiptsGrid").append(trHTML).DataTable({
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
                                "targets": [0,12],
                                "visible": false,
                            }
                            ,
                            {
                                "targets": [10],
                                "orderable": false,
                            },
                            { className: 'text-center', targets: [10] },
                        ],
                        initComplete: function () {
                            if ($(document).find('#MultiStoreroom').val() != "True") {
                                dtInventoryDataTable.columns(1).visible(false);
                            }
                        }
                    });
                /*    $('#txtPartId').val($('#txtPartId > option:first').val()).trigger('change');*/
                    $('#inventoryModel_ReceiptQuantity').val("");
                    ResetErrorDiv();
                    $(document).find("#txtPartId").val("");
                    //var areaddescribedby = $(document).find("#txtPartId").attr('aria-describedby');
                    //$('#' + areaddescribedby).hide();
                    $(document).find('form').find("#txtPartId").removeClass("input-validation-error");
                    if ($(document).find('#MultiStoreroom').val() == "True") {
                        $(document).find("#inventoryModel_StoreroomId").val("").trigger('change');
                        var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
                        $('#' + areaddescribedby).hide();
                        $(document).find('form').find(".ddlStoreroom").removeClass("input-validation-error");
                  }
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
    else {
        CloseLoader();
        ShowGenericErrorOnAddUpdate(data);
    }
}
function PartNotInInventorySelectedItem(count, PartId, PartClientLookupId, Description, PartUPCCode, QtyOnHand, QuantityCount, PartStoreroomUpdateIndex, StoreroomId, StoreroomName) {
    this.count = count;
    this.PartId = PartId;
    this.PartClientLookupId = PartClientLookupId;
    this.Description = Description;
    this.PartUPCCode = PartUPCCode;
    this.QtyOnHand = QtyOnHand;
    this.QuantityCount = QuantityCount;
    this.PartStoreroomUpdateIndex = PartStoreroomUpdateIndex;
    this.StoreroomId = StoreroomId;
    this.StoreroomName = StoreroomName;
};
$(document).on('click', "#btnconfirm", function () {
    if (InventorySelectedItemArray.length < 1) {
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
    else {
        GeneratedfinalSelectPartsTable(InventorySelectedItemArray);
    }
});
function GeneratedfinalSelectPartsTable(datasource1) {
    InventorySelectedItemArray = [];
    $.each(datasource1, function (index, item) {
        var count1 = count;
        var PartId = item.PartId;
        var PartClientLookupId = item.PartClientLookupId;
        var Description = item.Description;
        var PartUPCCode = item.PartUPCCode;
        var QtyOnHand = item.QtyOnHand;
        var QuantityCount = item.QuantityCount;
        var PartStoreroomUpdateIndex = item.PartStoreroomUpdateIndex;
        var _StoreroomId = item.StoreroomId;
        var _StoreroomName = item.StoreroomName;
        var item = new PartNotInInventorySelectedItem(count1, PartId, PartClientLookupId, Description, PartUPCCode, QtyOnHand, QuantityCount, PartStoreroomUpdateIndex , _StoreroomId, _StoreroomName);
        InventorySelectedItemArray.push(item);
    });
    var list = JSON.stringify({ 'list': InventorySelectedItemArray });
    $.ajax({
        url: "/InventoryPhysicalInventory/SaveListPhysicalInventoryFromGrid",
        type: "POST",
        dataType: "json",
        data: list,
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                InventorySelectedItemArray = [];
                SuccessAlertSetting.text = getResourceValue("RecordsUpdateAlert");
                swal(SuccessAlertSetting, function () {
                    funcRefreshPage();
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
            }
        },
        complete: function () {
            CloseLoader();
            //var areaddescribedby = $(document).find("#txtPartId").attr('aria-describedby');
            //$('#' + areaddescribedby).hide();
            /*$(document).find('form').find("#txtPartId").removeClass("input-validation-error");*/
            $('#inventoryModel_ReceiptQuantity').val("");
            $("#txtPartId").val("");
            if ($(document).find('#MultiStoreroom').val() == "True") {
                $(document).find("#inventoryModel_StoreroomId").val("").trigger('change');
                var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
                $('#' + areaddescribedby).hide();
                $(document).find("#inventoryModel_StoreroomId").removeClass("input-validation-error");

            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '.btndeletereceipt', function () {
    thisElement = dtInventoryDataTable.row($(this).parents('tr'));
    deletedRow = dtInventoryDataTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        dtInventoryDataTable.row(thisElement).remove().draw();
        InventorySelectedItemArray = InventorySelectedItemArray.filter(function (el) {
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
    InventorySelectedItemArray = [];
    $('#txtPartId').val("");//($('#inventoryModel_PartClientLookupId > option:first').val()).trigger('change');
    /* $('#inventoryModel_UnitCost').val(0);*/
    $('#inventoryModel_ReceiptQuantity').val("");
    if ($(document).find('#MultiStoreroom').val() == "True") {
        $(document).find("#inventoryModel_StoreroomId").val("").trigger('change');
        var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find(".ddlStoreroom").removeClass("input-validation-error");
    }
    dtInventoryDataTable.clear().draw();
    ResetErrorDiv();
    //var areaddescribedby = $(document).find("#txtPartId").attr('aria-describedby');
    //$('#' + areaddescribedby).hide();
    //$(document).find('form').find("#txtPartId").removeClass("input-validation-error");
}


//#region QR Reader
$(document).on('click', '#btnQrScanner', function () {
    $(document).find('#txtPartId').val('');
    $(document).find('#hdnPartId').val('');
    if ($(document).find('#MultiStoreroom').val() == "True") {
        if ($(document).find('.ddlStoreroom').val() == "") {
            return false;
        }
    }
    if (!$(document).find('#QrCodeReaderModal').hasClass('show')) {
        $(document).find('#QrCodeReaderModal').modal("show");
        StartQRReaderPartCheckout();
    }
});
function StartQRReaderPartCheckout() {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 150,
                //aspectRatio: aspectratio //1.7777778
            }, success => {
                onScanSuccessPartCheckout(success);
            }, error => {
            });
        } else {
            if ($(document).find('#QrCodeReaderModal').hasClass('show')) {
                $(document).find('#QrCodeReaderModal').modal("hide");
            }
        }
    }).catch(e => {
        if ($(document).find('#QrCodeReaderModal').hasClass('show')) {
            $(document).find('#QrCodeReaderModal').modal("hide");
        }
        if (e && e.startsWith('NotReadableError')) {
            ShowErrorAlert("Unable to start camera. It seems that another app is using the camera.");
        }
        else if (e && e.startsWith('NotFoundError')) {
            ShowErrorAlert("Camera device not found.");
        }

    });
}
function onScanSuccessPartCheckout(decodedText) {
    var url = "/Parts/GetPartIdByClientLookUpId?clientLookUpId=" + decodedText;
    if ($(document).find('#MultiStoreroom').val() == "True") {
        var StoreroomId = $(document).find('.ddlStoreroom').val();
        url = "/Base/GetPartIdByStoreroomIdAndClientLookUpforMultiStoreroom?ClientLookupId=" + decodedText + "&StoreroomId=" + StoreroomId;

    }
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if ($(document).find('#QrCodeReaderModal').hasClass('show')) {
                $(document).find('#QrCodeReaderModal').modal("hide");
            }
            if (data.PartId > 0) {
                if (typeof activeTab !== 'undefined' && activeTab == "#Returnpart") {
                    $(document).find('#txtPartReturnId').val(decodedText);
                    $(document).find('#hdnPartReturnId').val(data.PartId);
                }
                else {
                    $(document).find('#txtPartId').val(decodedText);
                    $(document).find('#hdnPartId').val(data.PartId);
                }


            } else if ($(document).find('#MultiStoreroom').val() == "True") {
                var msg = getResourceValue('spnInvalidQrCodeMsgforMultiStoreroom');
                ShowErrorAlert(msg.replace('${decodedText}', decodedText));
            }
            else {
                //Show Error Swal
                ShowErrorAlert(getResourceValue('spnInvalidQrCodeMsg').replace('${decodedText}', decodedText));
            }

        },
        complete: function () {
            StopCamera();
            CloseLoader();
        },
        error: function (xhr) {
            ShowErrorAlert("Something went wrong. Please try again.");
            CloseLoader();
        }
    });
}
function detectMob() {
    var isMobile = false;
    if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
        || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) {
        return true;
    }
    return isMobile;
}

//#endregion
