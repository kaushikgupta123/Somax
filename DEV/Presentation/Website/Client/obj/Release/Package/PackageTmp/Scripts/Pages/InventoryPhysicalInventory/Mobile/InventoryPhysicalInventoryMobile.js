var InventorySelectedItemArray = [];
var dtInventoryDataTable;
var count = 0;
var deletedRow;
var thisElement;
var StoreroomId = "";
var StoreroomName = "";
$(document).ready(function () {
    mobiscroll.settings = {
        lang: 'en',                                       // Specify language like: lang: 'pl' or omit setting to use default
        theme: 'ios',                                     // Specify theme like: theme: 'ios' or omit setting to use default
        themeVariant: 'light'                             // More info about themeVariant: https://docs.mobiscroll.com/4-10-9/javascript/popup#opt-themeVariant
    };
    $(document).find('.select2picker').select2({});
    $(document).on('change', '.ddlStoreroom', function () {
        $(document).find('#txtPartId').val('');
        $(document).find('#hdnPartId').val('');
        if ($(this).val() != '') {
            $(document).find('#openinvpartgrid').closest('.mbsc-col-12').show();
            $(document).find('#btnQrScanner').closest('.mbsc-col-12').show();
        } else {
            $(document).find('#openinvpartgrid').closest('.mbsc-col-12').hide();
            $(document).find('#btnQrScanner').closest('.mbsc-col-12').hide();
        }
        var tlen = $(document).find(".ddlStoreroom").val();
        if (tlen.length > 0) {
            var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find(".ddlStoreroom").removeClass("mbsc-err");
        }
        else {
            var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
            $('#' + areaddescribedby).show();
            $(document).find('form').find(".ddlStoreroom").addClass("mbsc-err");
        }
    });
    $('#MobilePhInvnt').trigger('mbsc-enhance');
    SetControls();
});
function SetControls() {
    var errClass = 'mobile-validation-error';
    CloseLoader();
    $.validator.setDefaults({
        ignore: null,
        //errorClass: "mobile-validation-error", // default values is input-validation-error
        //validClass: "valid", // default values is valid
        highlight: function (element, errorClass, validClass) { //for the elements having error
            $(element).addClass(errClass).removeClass(validClass);
            $(element.form).find("#" + element.id).parent().parent().addClass("mbsc-err");
            var elemName = $(element.form).find("#" + element.id).attr('name');
            $(element.form).find('[data-valmsg-for="' + elemName + '"]').addClass("mbsc-err-msg");
        },
        unhighlight: function (element, errorClass, validClass) { //for the elements having not any error
            $(element).removeClass(errClass).addClass(validClass);
            $(element.form).find("#" + element.id).parent().parent().removeClass("mbsc-err");
            var elemName = $(element.form).find("#" + element.id).attr('name');
            $(element.form).find('[data-valmsg-for="' + elemName + '"]').removeClass("mbsc-err-msg");
        },
    });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        if ($(this).closest('form').length > 0) {
            $(this).valid();
        }
    });
    $(document).find('.select2picker').select2({});
    $(document).find('.mobiscrollselect:not(:disabled)').mobiscroll().select({
        display: 'bubble',
        //touchUi: false,
        /*data: remoteData,*/
        filter: true,
        group: {
            groupWheel: false,
            header: false
        },
        //width: 400,
        //placeholder: 'Please Select...'

    });
    $(document).find('.mobiscrollselect:disabled').mobiscroll().select({
        disabled: true
    });
    $('#inventoryModel_ReceiptQuantity').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 99999999.99,
        //fill: 'ltr',
        maxScale: 2,
        preset: 'decimal',
        thousandsSeparator: '',
        entryMode: 'freeform'
    });
    var x = parseFloat($('#inventoryModel_ReceiptQuantity').val()) == 0 ? '' : $('#inventoryModel_ReceiptQuantity').val();
    $('#inventoryModel_ReceiptQuantity').mobiscroll('setVal', x);
    SetFixedHeadStyle();
}


$(document).on('click', '#openinvpartgrid', function () {
    generatePartScrollViewForMobileMobiscroll();
});
function generatePartScrollViewForMobileMobiscroll() {
    PartListlength = 0;
    $.ajax({
        "url": "/InventoryPhysicalInventory/PartLookupListView_Mobile",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#DivPartSearchScrollViewModal').html(data);
        },
        complete: function () {
            InitializePartListView_Mobile();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

var PartListView,
    PartListlength = 0,
    PartPageLength = 100;
function InitializePartListView_Mobile() {
    PartListView = $('#PartListViewForSearch').mobiscroll().listview({
        theme: 'ios',
        themeVariant: 'light',
        animateAddRemove: false,
        striped: true,
        swipe: false
    }).mobiscroll('getInst');
    BindPartDataForListView();
    $('#maintenancepartIdModal').addClass('slide-active');
}
$(document).on('click', '#btnPartLoadMore', function () {
    $(this).hide();
    PartListlength += PartPageLength;
    InitializePartListView_Mobile();
});
function BindPartDataForListView() {
    var Search = $(document).find('#txtPartSearch_Mobile').val();
    $.ajax({
        "url": "/InventoryPhysicalInventory/GetPartLookupListchunksearch_Mobile",
        data: {
            Search: Search,
            Start: PartListlength,
            Length: PartPageLength,
            Storeroomid: ($('#MultiStoreroom').val() == 'True' ? $('#StoreroomId').val() : '')
        },
        type: 'POST',
        dataType: 'JSON',
        beforeSend: function () {
            ShowLoader();
            PartListView.showLoading();
        },
        success: function (data) {
            var i, item, lihtml;
            for (i = 0; i < data.data.length; ++i) {
                item = data.data[i];
                lihtml = '';
                lihtml = '<li class="scrollview-partsearch" data-id="' + item.PartId + '"; data-clientlookupid="' + item.ClientLookUpId + '">';
                lihtml = lihtml + "" + item.ClientLookUpId + ' (' + item.Description + ')</li>';
                PartListView.add(null, mobiscroll.$(lihtml));
            }
            if ((PartListlength + PartPageLength) < data.recordsTotal) {
                $('#btnPartLoadMore').show();
            }
            else {
                $('#btnPartLoadMore').hide();
            }
        },
        complete: function () {
            PartListView.hideLoading();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '#maintenancepartIdModalHide', function () {
    $(document).find('#maintenancepartIdModal').removeClass("slide-active"); //..modal("hide");
    $('#txtPartSearch_Mobile').val('');
    $('#DivPartSearchScrollViewModal').html('');
});
$(document).on("keyup", '#txtPartSearch_Mobile', function (e) {
    if (e.keyCode == 13) {
        generatePartScrollViewForMobileMobiscroll();
    }
});
$(document).on('click', '.scrollview-partsearch', function (e) {
    $(document).find('#txtPartId').val($(this).data('clientlookupid')).trigger('mbsc-enhance');//.removeClass('input-validation-error');
    $(document).find('#txtPartId').closest('form').valid();
    $(document).find('#hdnPartId').val($(this).data('id'));
    $(document).find('#maintenancepartIdModal').removeClass("slide-active");
});

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
            $('#inventoryModel_ReceiptQuantity').mobiscroll('clear');
            $("#txtPartId").val("");
            if ($(document).find('#MultiStoreroom').val() == "True") {
                $(document).find("#StoreroomId").val("").trigger('change');
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
    $('#inventoryModel_ReceiptQuantity').mobiscroll('clear');
    if ($(document).find('#MultiStoreroom').val() == "True") {
        $(document).find("#StoreroomId").val("").trigger('change');
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

function StartQRReader_Mobile(Module) {
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
            if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
                $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
            }
        }
    }).catch(e => {
        if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
            $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
        }
        if (e && e.startsWith('NotReadableError')) {
            ShowErrorAlert(getResourceValue("cameraIsBeingUsedByAnotherAppAlert"));
        }
        else if (e && e.startsWith('NotFoundError')) {
            ShowErrorAlert(getResourceValue("cameraDeviceNotFoundAlert"));
        }

    });
}

$(document).on('click', '#btnQrScanner', function () {
    $(document).find('#txtPartId').val('');
    if (!$(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').addClass("slide-active");
        StartQRReader_Mobile('');
    }
});
$(document).on('click', '#closeQrScanner_Mobile', function () {
    if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').removeClass('slide-active');
    }
    if (!$(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').removeClass('slide-active');
        StopCamera(); // using same method from somax_main.js
    }
});
function ValidationOnSuccess(data) {
    if (data.data == "success") {
        count++;
        var _PartClientLookupId = $(document).find("#txtPartId").val().trim();
        var _QuantityCounted = $(document).find("#inventoryModel_ReceiptQuantity").val().trim();
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
            data: { PartClientLookupId: _PartClientLookupId, QuantityCounted: _QuantityCounted, count: count, StoreroomId: StoreroomId, StoreroomName: StoreroomName },
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
                                "targets": [0, 12],
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
                    $(document).find('#inventoryModel_ReceiptQuantity').val("");
                    $('#inventoryModel_ReceiptQuantity').mobiscroll('clear');
                    ResetErrorDiv();
                    $(document).find("#txtPartId").val("");
                    //var areaddescribedby = $(document).find("#txtPartId").attr('aria-describedby');
                    //$('#' + areaddescribedby).hide();
                    $(document).find('form').find("#txtPartId").removeClass("input-validation-error");
                    if ($(document).find('#MultiStoreroom').val() == "True") {
                        $(document).find("#StoreroomId").val("").trigger('change');
                        var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
                        $('#' + areaddescribedby).hide();
                        $(document).find('form').find(".ddlStoreroom").trigger('mbsc-enhance');//.removeClass("input-validation-error");
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
            if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
                $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
            }
            if (data.PartId > 0) {
                if (typeof activeTab !== 'undefined' && activeTab == "#Returnpart") {
                    $(document).find('#txtPartReturnId').val(decodedText);
                    $(document).find('#hdnPartReturnId').val(data.PartId);
                }
                else {
                    //$(document).find('#txtPartId').val(decodedText);
                    $(document).find('#txtPartId').val(decodedText).trigger('mbsc-enhance'); //.removeClass('input-validation-error');
                    $(document).find('#txtPartId').closest('form').valid();
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
