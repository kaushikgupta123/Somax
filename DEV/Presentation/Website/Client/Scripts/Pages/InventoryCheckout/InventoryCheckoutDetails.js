var issueToSelected = "";
var chargeTypeSelected = "";
var chargeTypeText = "";
var chargeToSelected = "";
var chargeToClientLookuopId = '';
var chargeToId = "";
var chargeToText = "";
var partidSelected = "";
var partClientLookupId = "";
var partIdText = "";
var description = "";
var upcCode = "";
var quantitySelected = "";
var errMsg = "";
var finalSelectInventoryTable;
var finalReturnSelectInventoryTable;
var personnelId = "";
var delSingle = "";
var equipmentid = -1;
var activeTab = "#Issuepart";
var comments = "";
var StoreroomId = "";
var StoreroomName = "";
$(function () {
    $(document).find('.select2picker').select2({});
 
    $("#inventoryCheckoutModel_ChargeToId").attr('disabled', 'disabled');
    $(document).find("#imgChargeToTree").hide();
    loadIssueParts();
    $(document).on("change", "#inventoryCheckoutModel_ChargeType", function () {
        var option = '';
        chargeTypeText = $('option:selected', this).text();
        var type = $(this).val();
        chargeTypeSelected = type;
        if (type == "") {
            $(document).find("#imgChargeToTree").hide();
            option = "--Select--";
            $(document).find("#inventoryCheckoutModel_ChargeToId").val("").trigger('change');
            $("#inventoryCheckoutModel_ChargeToId").attr('disabled', 'disabled');
            $(document).find("#opengrid").hide();
        }
        else {
            if (type == "Equipment") {
                $(document).find("#imgChargeToTree").show();
               /* $(document).find("#txtChargeToId").parents('.inputText').addClass('wthAdjst2');*/
             
            }
            else {
                $(document).find("#imgChargeToTree").hide();
                /*$(document).find("#txtChargeToId").parents('.inputText').removeClass('wthAdjst2');  */             
            }           
            $("#inventoryCheckoutModel_ChargeToId").removeAttr('disabled');
            $(document).find("#opengrid").show();

        }
        $(document).find("#txtChargeToId").val("");
        var tlen = $(document).find("#inventoryCheckoutModel_ChargeType").val();
        if (tlen.length > 0) {
            var areaddescribedby = $(document).find("#inventoryCheckoutModel_ChargeType").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find("#inventoryCheckoutModel_ChargeType").removeClass("input-validation-error");
        }
        else {
            var areaddescribedby = $(document).find("#inventoryCheckoutModel_ChargeType").attr('aria-describedby');
            $('#' + areaddescribedby).show();
            $(document).find('form').find("#inventoryCheckoutModel_ChargeType").addClass("input-validation-error");
        }
    });
    $(document).on("change", "#inventoryCheckoutModel_ChargeToId", function () {
        chargeToSelected = $(this).val();
        chargeToText = $('option:selected', this).text();
        var tlen = $(document).find("#inventoryCheckoutModel_ChargeToId").val();
        if (tlen.length > 0) {
            var areaddescribedby = $(document).find("#inventoryCheckoutModel_ChargeToId").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find("#inventoryCheckoutModel_ChargeToId").removeClass("input-validation-error");
        }
        else {
            var areaddescribedby = $(document).find("#inventoryCheckoutModel_ChargeToId").attr('aria-describedby');
            $('#' + areaddescribedby).show();
            $(document).find('form').find("#inventoryCheckoutModel_ChargeToId").addClass("input-validation-error");
        }
    });
    $(document).on('change', '.ddlStoreroom', function () {
        var IssueFormExist = $(document).find("#inventoryCheckoutModel_selectedPersonnelId");
        var ReturnFormExist = $(document).find("#inventoryCheckoutReturnModel_selectedPersonnelId");
        if ($(this).val() != '') {
            $(document).find('#openpartgrid').show();
            if (IssueFormExist.length > 0) {
                $(document).find('#btnQrScanner').show();
            }
            if (ReturnFormExist.length > 0)
            {
                $(document).find('#btnQrScannerreturn').show();
            }
        } else {
            $(document).find('#openpartgrid').hide();
            if (IssueFormExist.length > 0) {
                $(document).find('#btnQrScanner').hide();
            }
            if (ReturnFormExist.length > 0) {
                $(document).find('#btnQrScannerreturn').hide();
            }
        }
        if (IssueFormExist.length > 0) {
            $(document).find('#hdnPartId').val('');
            $(document).find('#txtPartId').val('');
        }
        if (ReturnFormExist.length > 0) {
            $(document).find('#hdnPartReturnId').val('');
            $(document).find('#txtPartReturnId').val('');
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
//#region FillGrid
var FinalGridSelectedItemArray = [];
function ItemAddToGridSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var eachdRow;
        var thisData = [];
        var i;
        var rowCount = 0;
        if (typeof finalSelectInventoryTable != 'undefined' && delSingle == 1) {
            delSingle = 0;
            if (finalSelectInventoryTable.data().count() > 0) {
                rowCount = finalSelectInventoryTable.data().count();
            }
            if (rowCount > 0) {
                for (i = 0; i < rowCount; i++) {
                    eachRow = finalSelectInventoryTable.row(i).data();
                    if (eachRow.PersonnelId) {
                        var item = new ItemsToAddToTemporaryTable(eachRow.IssueToClentLookupId, eachRow.ChargeType, eachRow.ChargeToClientLookupId, eachRow.PartClientLookupId, eachRow.PartDescription, eachRow.UPCCode, eachRow.Quantity, eachRow.ErrorMessagerow, eachRow.PersonnelId, eachRow.PartStoreroomId, eachRow.ChargeToId, eachRow.PartId, eachRow.Comments, eachRow.StoreroomId, eachRow.StoreroomName);
                        FinalGridSelectedItemArray.push(item);
                    }
                }
            }
        }
        personnelId = $(document).find("#inventoryCheckoutModel_selectedPersonnelId").val();
        quantitySelected = $(document).find("#inventoryCheckoutModel_Quantity").val();
        chargeToSelected = $(document).find("#hdnChargeToId").val();
        chargeToClientLookupId = $(document).find("#txtChargeToId").val();
        partidSelected = $(document).find("#hdnPartId").val();
        comments = $(document).find("#inventoryCheckoutModel_Comments").val();
        if ($(document).find(".ddlStoreroom").length > 0) {
            StoreroomId = $(document).find("#inventoryCheckoutModel_StoreroomId").val();
            StoreroomName = $(document).find(".ddlStoreroom").find("option:selected").text()
        } else {
            StoreroomId = 0;
            StoreroomName = "";
        }
        $(document).find("#finselectcontainer").show();
        $(document).find("#dvInventorySelectTable").show();
        $(document).find("#dvIdBttns").show();
        $.ajax({
            url: "/InventoryCheckout/PopulateInventorySelectTable",
            type: "GET",
            dataType: 'json',
            async: false,
            data: { _personnelId: personnelId, _TransactionQuantity: quantitySelected, _partId: partidSelected, _chargeToId: chargeToSelected, _chargeToClientLookupId: chargeToClientLookupId, _chargeType: chargeTypeSelected, _comments: comments, _StoreroomId: StoreroomId, _StoreroomName: StoreroomName},
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.ErrorMsg == null) {
                    var IssueToClientLookupId = data.IssueToClientLookupId;
                    var item = new ItemsToAddToTemporaryTable(IssueToClientLookupId, chargeTypeSelected, data.chargeToClientLookupId, data.objPart.ClientLookupId, data.objPart.Description, data.objPart.UPCCode, quantitySelected, errMsg, personnelId, data.PartStoreroomId, chargeToSelected, data.objPart.PartId, chargeTypeText, data.Comments, data.StoreroomId,data.StoreroomName);
                    FinalGridSelectedItemArray.push(item);
                    var areaddescribedby;
                    if (typeof finalSelectInventoryTable != 'undefined') {
                        finalSelectInventoryTable.destroy();
                    }
                    GenerateFinalSelectInventoryTable(FinalGridSelectedItemArray);
                    $(document).find("#txtChargeToId").val("");
                    $(document).find("#hdnChargeToId").val("");
                    $(document).find("#txtPartId").val("");
                    $(document).find("#hdnPartId").val("");
                    areaddescribedby = $(document).find("#txtPartId").attr('aria-describedby');
                    $('#' + areaddescribedby).hide();
                    $(document).find('form').find("#txtPartId").removeClass("input-validation-error");
                    areaddescribedby = $(document).find("#inventoryCheckoutModel_ChargeType").attr('aria-describedby');
                    $('#' + areaddescribedby).hide();
                    $(document).find('form').find("#inventoryCheckoutModel_ChargeType").removeClass("input-validation-error");
                    areaddescribedby = $(document).find("#txtChargeToId").attr('aria-describedby');
                    $('#' + areaddescribedby).hide();
                    $(document).find('form').find("#txtChargeToId").removeClass("input-validation-error");
                    $(document).find("#inventoryCheckoutModel_Quantity").val("1");
                    $(document).find("#inventoryCheckoutModel_Comments").val("");
                    ScrollToId("btnConfirmAllItems");
                }
                else {
                    swal({
                        title: getResourceValue("CommonErrorAlert"),
                        text: data.ErrorMsg,
                        type: "error",
                        showCancelButton: false,
                        confirmButtonClass: "btn-sm btn-danger",
                        cancelButtonClass: "btn-sm",
                        confirmButtonText: getResourceValue("SaveAlertOk"),
                        cancelButtonText: getResourceValue("CancelAlertNo")
                    }, function () {
                        $(document).find("#finselectcontainer").hide();
                    });
                }
            },
            complete: function () {
                if ($('#finselectcontainer').is(":hidden")) {
                    $('#finselectcontainer').show();
                }
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
function ItemsToAddToTemporaryTable(IssueToClentLookupId, ChargeType, ChargeToClientLookupId, PartClientLookupId, PartDescription, UPCCode, Quantity, ErrorMessagerow, PersonnelId, PartStoreroomId, ChargeToId, PartId, ChargeTypeText, Comments, StoreroomId, StoreroomName) {
    this.IssueToClentLookupId = IssueToClentLookupId;
    this.ChargeType = ChargeType;
    this.ChargeToClientLookupId = ChargeToClientLookupId;
    this.PartClientLookupId = PartClientLookupId;
    this.PartDescription = PartDescription;
    this.UPCCode = UPCCode;
    this.Quantity = Quantity;
    this.ErrorMessagerow = ErrorMessagerow;
    this.PersonnelId = PersonnelId;
    this.PartStoreroomId = PartStoreroomId;
    this.ChargeToId = ChargeToId;
    this.PartId = PartId;
    this.ChargeTypeText = ChargeTypeText;
    this.Comments = Comments;
    this.StoreroomId = StoreroomId;
    this.StoreroomName = StoreroomName;
    
};
function GenerateFinalSelectInventoryTable(datasource1) {
    var data = datasource1;
    finalSelectInventoryTable = $("#inventorySelectTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        "order": [[0, "asc"]],
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        data: data,
        columnDefs: [
            {
                targets: [9], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-danger delItem gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                },
            },
            {
                "targets": [10],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [11],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [12],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [13],
                "visible": false,
                "searchable": false
            }
            ,
            {
                "targets": [14],
                "visible": false,
                "searchable": false
            }
        ],
        "columns":
            [
                { "data": "IssueToClentLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                //{ "data": "ChargeType", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ChargeTypeText", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ChargeToClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "StoreroomName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "PartDescription", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "UPCCode", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ErrorMessagerow", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PersonnelId", "bSearchable": false, "bSortable": false, "className": "text-center" },
                { "data": "PartStoreroomId", "autoWidth": true, "bSearchable": false, "bSortable": false },
                { "data": "ChargeToId", "autoWidth": true, "bSearchable": false, "bSortable": false },
                { "data": "PartId", "autoWidth": true, "bSearchable": false, "bSortable": false },
                { "data": "Comments", "autoWidth": true, "bSearchable": false, "bSortable": false },
                { "data": "StoreroomId", "autoWidth": true, "bSearchable": false, "bSortable": false }
            ],
        initComplete: function () {
            if ($(document).find('#MultiStoreroom').val() != "True") {
                finalSelectInventoryTable.columns(3).visible(false);
            }
        }
    });
};
$(document).on('click', '.delItem', function () {
    delSingle = 1;
    var thisElement = finalSelectInventoryTable.row($(this).parents('tr'));
    var deletedRow = finalSelectInventoryTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        finalSelectInventoryTable.row(thisElement).remove().draw();
        FinalGridSelectedItemArray = FinalGridSelectedItemArray.filter(function (el) {
            return el.personnelId !== deletedRow.personnelId;
        });
        var rowCount = finalSelectInventoryTable.data().count();
        if (rowCount < 1) {
            $(document).find("#finselectcontainer").hide();
        }
    });
});
$(document).on('click', '#btnCancelAllItems', function () {
    swal(CancelAlertSetting, function () {
        finalSelectInventoryTable.clear().draw();
        FinalGridSelectedItemArray = [];
        $(document).find("#finselectcontainer").hide();
    });
})

//#endregion
//#region ConfirmItems
$(document).on('click', "#btnConfirmAllItems", function () {
    var PersonnelId = personnelId;
    var thisData = [];
    var toReturn;
    var thisElement;
    var eachdRow;
    var totalRows = finalSelectInventoryTable.data().count();
    var i;
    for (i = 0; i < totalRows; i++) {
        eachRow = finalSelectInventoryTable.row(i).data();
        if (!$.isNumeric(eachRow.Quantity)) {
            $(this).find('.decimalinput ').addClass('input-validation-error');
            toReturn = false;
        }
        else {
            $(this).find('.decimalinput ').removeClass('input-validation-error');
        }
        if (eachRow.PersonnelId) {
            var item = new ItemsToAddToTemporaryTable(eachRow.IssueToClentLookupId, eachRow.ChargeType, eachRow.ChargeToClientLookupId, eachRow.PartClientLookupId, eachRow.PartDescription, eachRow.UPCCode, eachRow.Quantity, eachRow.ErrorMessagerow, eachRow.PersonnelId, eachRow.PartStoreroomId, eachRow.ChargeToId, eachRow.PartId, '', eachRow.Comments, eachRow.StoreroomId, eachRow.StoreroomName);
            thisData.push(item);
        }
    }
    if (toReturn == false) {
        return;
    }
    var dataList = JSON.stringify({ 'dataList': thisData });
    $.ajax({
        url: "/InventoryCheckout/ConfirmInventorydata",
        type: "POST",
        dataType: "json",
        data: dataList,
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.length > 0) {
                var msgEror = getResourceValue("UpdateAlert");
                ShowGenericErrorOnAddUpdate(msgEror);
                FinalGridSelectedItemArray = [];
                $.each(data, function (index, item) {
                    if (item.ErrorMessagerow != null) {
                        var item = new ItemsToAddToTemporaryTable(issueToSelected, chargeTypeSelected, chargeToClientLookuopId, data.objPart.ClientLookupId, data.objPart.Description, data.objPart.UPCCode, quantitySelected, errMsg, personnelId, ChargeToId, PartId, Comments, StoreroomId, StoreroomName);
                        FinalGridSelectedItemArray.push(item);
                    }
                });
                if (typeof finalSelectInventoryTable != 'undefined') {
                    finalSelectInventoryTable.destroy();
                }
                GenerateFinalSelectInventoryTable(FinalGridSelectedItemArray);
                CloseLoader();
            }
            else {
                finalSelectInventoryTable.clear().draw();
                FinalGridSelectedItemArray = [];
                SuccessAlertSetting.text = getResourceValue("ConfirmedSuccessfullyAlert");
                swal(SuccessAlertSetting, function () {
                    $(document).find("#finselectcontainer").hide();
                });
            }
            $(document).find("#inventoryCheckoutModel_ChargeType").val("").trigger('change');
            var areaddescribedby = $(document).find("#inventoryCheckoutModel_ChargeType").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find("#inventoryCheckoutModel_ChargeType").removeClass("input-validation-error");
            $(document).find("#txtChargeToId").val("");
            $(document).find("#txtPartId").val("");
            $(document).find('form').find("#txtChargeToId").removeClass("input-validation-error");
            $(document).find('form').find("#txtPartId").removeClass("input-validation-error");
            $(document).find("#inventoryCheckoutModel_Quantity").val("1");
            $(document).find("#inventoryCheckoutModel_Comments").val(" ");
            if ($(document).find('#MultiStoreroom').val() == "True") {
                $(document).find("#inventoryCheckoutModel_StoreroomId").val("").trigger('change');
                var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
                $('#' + areaddescribedby).hide();
                $(document).find('form').find(".ddlStoreroom").removeClass("input-validation-error");
            }
           
          
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion
//#region ModalTree
$(document).on('click', '#imgChargeToTree,#imgChargeToTreereturn', function (e) {
    $(this).blur();
    generateEquipmentTree(-1);
});
function generateEquipmentTree(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/EquipmentHierarchyTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        cache: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find(".cntTree").html(data);
            $('#inventoryCheckoutTreeModal').modal('show');
        },
        complete: function () {
            CloseLoader();
            treeTable($(document).find('#tblTree'));
            //V2-624
            $(document).find('.radSelect').each(function () {
                if ($(document).find('#hdnId').val() == '0' || $(document).find('#hdnId').val() == '') {

                    if ($(this).data('equipmentid') === equipmentid) {
                        $(this).attr('checked', true);
                    }

                }
                else {

                    if ($(this).data('equipmentid') == $(document).find('#hdnId').val()) {
                        $(this).attr('checked', true);
                    }

                }

            });
        },
        error: function (xhr) {
            alert('error');
        }
    });
}
$(document).on('change', '.radSelect', function () {
    equipmentid = $(this).data('equipmentid');
    var clientlookupid = $(this).data('clientlookupid').split(' ')[0];
    $('#inventoryCheckoutTreeModal').modal('hide');
    if (typeof activeTab !== 'undefined' && activeTab == "#Returnpart")
    {
        $(document).find('#txtReturnChargeToId').val(clientlookupid);
        $(document).find('#hdnReturnChargeToId').val(equipmentid);
    }
    else
    {
        $(document).find('#txtChargeToId').val(clientlookupid);
        $(document).find('#hdnChargeToId').val(equipmentid);
    }
    

});
//#endregion

$(document).on('click', "#opengrid,#opengridreturn", function () {
    if (typeof activeTab !== 'undefined' && activeTab == "#Returnpart") {
        var textChargeToReturnId = $("#inventoryCheckoutReturnModel_ChargeType option:selected").val();
        if (textChargeToReturnId == "WorkOrder") { generateWorkOrderDataTable(); }
        else if (textChargeToReturnId == "Equipment") { generateEquipmentDataTable(); }
        else { generateLocationDataTable(); }
    }
    else {
        var textChargeToId = $("#inventoryCheckoutModel_ChargeType option:selected").val();
        if (textChargeToId == "WorkOrder") { generateWorkOrderDataTable(); }
        else if (textChargeToId == "Equipment") { generateEquipmentDataTable(); }
        else { generateLocationDataTable(); }
    }

});

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
    if ($(document).find('#MultiStoreroom').val() == "True")
    {
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
              if (typeof activeTab !== 'undefined' && activeTab == "#Returnpart")
                {
                    $(document).find('#txtPartReturnId').val(decodedText);
                    $(document).find('#hdnPartReturnId').val(data.PartId);
                }
                else
                {
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

//#region  tab V2-624
function loadReturnParts() {
    $("#inventoryCheckoutReturnModel_ChargeToId").attr('disabled', 'disabled');
    $(document).find("#imgChargeToTreereturn").hide();
    $(document).find('#IssueParts').html();
    $(document).find('#finselectIssuecontainer').html();
    $.ajax({
        url: '/InventoryCheckout/LoadReturnPart',
        type: 'POST',
        data: {},
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#ReturnParts').html(data);

        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $(document).find('.select2picker').select2({});
            $(document).find("#opengridreturn").hide();
            if (ReturnFinalGridSelectedItemArray.length != '0')
            {
                $(document).find("#finselectReturncontainer").show();
                $(document).find("#dvInventoryReturnSelectTable").show();
                $(document).find("#dvIdReturnBttns").show();
                GenerateReturnFinalSelectInventoryTable(ReturnFinalGridSelectedItemArray);
            }        
            var MultiStoreroom = $(document).find('#MultiStoreroom').val();
            if (MultiStoreroom == "True") {
                $(document).find('#openpartgrid').hide();
                $(document).find('#btnQrScannerreturn').hide();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            CloseLoader();
        }
    });
}
function loadIssueParts() {
    $(document).find('#ReturnParts').html();
    $(document).find('#finselectReturncontainer').html();
    $.ajax({
        url: '/InventoryCheckout/LoadIssueParts',
        type: 'POST',
        data: {},
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#IssueParts').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $(document).find('.select2picker').select2({});
            $(document).find("#opengrid").hide();
            if (FinalGridSelectedItemArray.length != '0') {
                $(document).find("#finselectcontainer").show();
                $(document).find("#dvInventorySelectTable").show();
                $(document).find("#dvIdBttns").show();
                GenerateFinalSelectInventoryTable(FinalGridSelectedItemArray);
            }   
            if ($(document).find('#MultiStoreroom').val() == "True") {
                $(document).find('#openpartgrid').hide();
                $(document).find('#btnQrScanner').hide();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            CloseLoader();
        }
    });
}

$(document).on('click', '.IC-det-tab', function (e) {
    ResetOtherTabs();
    var tab = $(this).data('tab');
    switch (tab) {
        case "IssueParts":
            $(document).find('#hdnId').val('');
            loadIssueParts();
            activeTab = "#Issuepart";
            break;
        case "ReturnParts":
            $(document).find('#hdnId').val('');
            loadReturnParts();
            activeTab = "#Returnpart";
            break;

    }
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
    $(document).find('#IssueParts').html('');
    $(document).find('#ReturnParts').html('');
}

//#endregion

//#region  Return Part
//#region FillGrid
var ReturnFinalGridSelectedItemArray = [];
function ReturnItemAddToGridSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var eachdRow;
        var thisData = [];
        var i;
        var rowCount = 0;
        if (typeof finalReturnSelectInventoryTable != 'undefined' && delSingle == 1) {
            delSingle = 0;
            if (finalReturnSelectInventoryTable.data().count() > 0) {
                rowCount = finalReturnSelectInventoryTable.data().count();
            }
            if (rowCount > 0) {
                for (i = 0; i < rowCount; i++) {
                    eachRow = finalReturnSelectInventoryTable.row(i).data();
                    if (eachRow.PersonnelId) {
                        var item = new ReturnItemsToAddToTemporaryTable(eachRow.IssueToClentLookupId, eachRow.ChargeType, eachRow.ChargeToClientLookupId, eachRow.PartClientLookupId, eachRow.PartDescription, eachRow.UPCCode, eachRow.Quantity, eachRow.ErrorMessagerow, eachRow.PersonnelId, eachRow.PartStoreroomId, eachRow.ChargeToId, eachRow.PartId, eachRow.Comments, eachRow.StoreroomId, eachRow.StoreroomName);
                        ReturnFinalGridSelectedItemArray.push(item);
                    }
                }
            }
        }
        personnelId = $(document).find("#inventoryCheckoutReturnModel_selectedPersonnelId").val();
        quantitySelected = $(document).find("#inventoryCheckoutReturnModel_Quantity").val();
        chargeToSelected = $(document).find("#hdnReturnChargeToId").val();
        chargeToClientLookupId = $(document).find("#txtReturnChargeToId").val();
        partidSelected = $(document).find("#hdnPartReturnId").val();
        comments = $(document).find("#inventoryCheckoutReturnModel_Comments").val();
        if ($(document).find(".ddlStoreroom").length > 0) {
            StoreroomId = $(document).find("#inventoryCheckoutReturnModel_StoreroomId").val();
            StoreroomName = $(document).find(".ddlStoreroom").find("option:selected").text()
        } else {
            StoreroomId = 0;
            StoreroomName = "";
        }
        $(document).find("#finselectReturncontainer").show();
        $(document).find("#dvInventoryReturnSelectTable").show();
        $(document).find("#dvIdReturnBttns").show();
        $.ajax({
            url: "/InventoryCheckout/PopulateInventorySelectTable",
            type: "GET",
            dataType: 'json',
            async: false,
            data: { _personnelId: personnelId, _TransactionQuantity: quantitySelected, _partId: partidSelected, _chargeToId: chargeToSelected, _chargeToClientLookupId: chargeToClientLookupId, _chargeType: chargeTypeSelected, _comments: comments, _StoreroomId: StoreroomId, _StoreroomName: StoreroomName },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.ErrorMsg == null) {
                    var IssueToClientLookupId = data.IssueToClientLookupId;
                    var item = new ReturnItemsToAddToTemporaryTable(IssueToClientLookupId, chargeTypeSelected, data.chargeToClientLookupId, data.objPart.ClientLookupId, data.objPart.Description, data.objPart.UPCCode, quantitySelected, errMsg, personnelId, data.PartStoreroomId, chargeToSelected, data.objPart.PartId, chargeTypeText, data.Comments, data.StoreroomId, data.StoreroomName);
                    ReturnFinalGridSelectedItemArray.push(item);
                    var areaddescribedby;
                    if (typeof finalReturnSelectInventoryTable != 'undefined') {
                        finalReturnSelectInventoryTable.destroy();
                    }
                    GenerateReturnFinalSelectInventoryTable(ReturnFinalGridSelectedItemArray);
                    $(document).find("#txtReturnChargeToId").val("");
                    $(document).find("#hdnReturnChargeToId").val("");
                    $(document).find("#txtPartReturnId").val("");
                    $(document).find("#hdnPartReturnId").val("");
                    areaddescribedby = $(document).find("#txtPartReturnId").attr('aria-describedby');
                    $('#' + areaddescribedby).hide();
                    $(document).find('form').find("#txtPartReturnId").removeClass("input-validation-error");
                    areaddescribedby = $(document).find("#inventoryCheckoutReturnModel_ChargeType").attr('aria-describedby');
                    $('#' + areaddescribedby).hide();
                    $(document).find('form').find("#inventoryCheckoutReturnModel_ChargeType").removeClass("input-validation-error");
                    areaddescribedby = $(document).find("#txtReturnChargeToId").attr('aria-describedby');
                    $('#' + areaddescribedby).hide();
                    $(document).find('form').find("#txtReturnChargeToId").removeClass("input-validation-error");
                    $(document).find("#inventoryCheckoutReturnModel_Quantity").val("1");
                    $(document).find("#inventoryCheckoutReturnModel_Comments").val("");
                    ScrollToId("btnReturnConfirmAllItems");
                }
                else {
                    swal({
                        title: getResourceValue("CommonErrorAlert"),
                        text: data.ErrorMsg,
                        type: "error",
                        showCancelButton: false,
                        confirmButtonClass: "btn-sm btn-danger",
                        cancelButtonClass: "btn-sm",
                        confirmButtonText: getResourceValue("SaveAlertOk"),
                        cancelButtonText: getResourceValue("CancelAlertNo")
                    }, function () {
                        $(document).find("#finselectReturncontainer").hide();
                    });
                }
            },
            complete: function () {
                if ($('#finselectReturncontainer').is(":hidden")) {
                    $('#finselectReturncontainer').show();
                }
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
function ReturnItemsToAddToTemporaryTable(IssueToClentLookupId, ChargeType, ChargeToClientLookupId, PartClientLookupId, PartDescription, UPCCode, Quantity, ErrorMessagerow, PersonnelId, PartStoreroomId, ChargeToId, PartId, ChargeTypeText, Comments, StoreroomId, StoreroomName) {
    this.IssueToClentLookupId = IssueToClentLookupId;
    this.ChargeType = ChargeType;
    this.ChargeToClientLookupId = ChargeToClientLookupId;
    this.PartClientLookupId = PartClientLookupId;
    this.PartDescription = PartDescription;
    this.UPCCode = UPCCode;
    this.Quantity = Quantity;
    this.ErrorMessagerow = ErrorMessagerow;
    this.PersonnelId = PersonnelId;
    this.PartStoreroomId = PartStoreroomId;
    this.ChargeToId = ChargeToId;
    this.PartId = PartId;
    this.ChargeTypeText = ChargeTypeText;
    this.Comments = Comments;
    this.StoreroomId = StoreroomId;
    this.StoreroomName = StoreroomName;
};
function GenerateReturnFinalSelectInventoryTable(datasource1) {
    var data = datasource1;
    finalReturnSelectInventoryTable = $("#inventoryReturnSelectTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        "order": [[0, "asc"]],
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        data: data,
        columnDefs: [
            {
                targets: [9], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-danger delReturnItem gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                },
            },
            {
                "targets": [10],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [11],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [12],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [13],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [14],
                "visible": false,
                "searchable": false
            }
        ],
        "columns":
        [
            { "data": "IssueToClentLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            //{ "data": "ChargeType", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "ChargeTypeText", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "ChargeToClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "StoreroomName", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "PartDescription", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            { "data": "UPCCode", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "ErrorMessagerow", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "PersonnelId", "bSearchable": false, "bSortable": false, "className": "text-center" },
            { "data": "PartStoreroomId", "autoWidth": true, "bSearchable": false, "bSortable": false },
            { "data": "ChargeToId", "autoWidth": true, "bSearchable": false, "bSortable": false },
            { "data": "PartId", "autoWidth": true, "bSearchable": false, "bSortable": false },
            { "data": "Comments", "autoWidth": true, "bSearchable": false, "bSortable": false },
            { "data": "StoreroomId", "autoWidth": true, "bSearchable": false, "bSortable": false }
            ],
        initComplete: function () {
            if ($(document).find('#MultiStoreroom').val() != "True") {
                finalReturnSelectInventoryTable.columns(3).visible(false);
            }
        }
    });
};
$(document).on('click', '.delReturnItem', function () {
    delSingle = 1;
    var thisElement = finalReturnSelectInventoryTable.row($(this).parents('tr'));
    var deletedRow = finalReturnSelectInventoryTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        finalReturnSelectInventoryTable.row(thisElement).remove().draw();
        ReturnFinalGridSelectedItemArray = ReturnFinalGridSelectedItemArray.filter(function (el) {
            return el.personnelId !== deletedRow.personnelId;
        });
        var rowCount = finalReturnSelectInventoryTable.data().count();
        if (rowCount < 1) {
            $(document).find("#finselectReturncontainer").hide();
        }
    });
});
$(document).on('click', '#btnReturnCancelAllItems', function () {
    swal(CancelAlertSetting, function () {
        finalReturnSelectInventoryTable.clear().draw();
        ReturnFinalGridSelectedItemArray = [];
        $(document).find("#finselectReturncontainer").hide();
    });
})
//#endregion
$(document).on('click', '#btnQrScannerreturn', function () {
    $(document).find('#txtPartReturnId').val('');
    $(document).find('#hdnPartReturnId').val('');
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
//#region ConfirmItems
$(document).on('click', "#btnReturnConfirmAllItems", function () {
    var PersonnelId = personnelId;
    var thisData = [];
    var toReturn;
    var thisElement;
    var eachdRow;
    var totalRows = finalReturnSelectInventoryTable.data().count();
    var i;
    for (i = 0; i < totalRows; i++) {
        eachRow = finalReturnSelectInventoryTable.row(i).data();
        if (!$.isNumeric(eachRow.Quantity)) {
            $(this).find('.decimalinput ').addClass('input-validation-error');
            toReturn = false;
        }
        else {
            $(this).find('.decimalinput ').removeClass('input-validation-error');
        }
        if (eachRow.PersonnelId) {
            var item = new ReturnItemsToAddToTemporaryTable(eachRow.IssueToClentLookupId, eachRow.ChargeType, eachRow.ChargeToClientLookupId, eachRow.PartClientLookupId, eachRow.PartDescription, eachRow.UPCCode, eachRow.Quantity, eachRow.ErrorMessagerow, eachRow.PersonnelId, eachRow.PartStoreroomId, eachRow.ChargeToId, eachRow.PartId, '', eachRow.Comments, eachRow.StoreroomId, eachRow.StoreroomName);
            thisData.push(item);
        }
    }
    if (toReturn == false) {
        return;
    }
    var dataList = JSON.stringify({ 'dataList': thisData });
    $.ajax({
        url: "/InventoryCheckout/ReturnConfirmInventorydata",
        type: "POST",
        dataType: "json",
        data: dataList,
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.length > 0) {
                var msgEror = getResourceValue("UpdateAlert");
                ShowGenericErrorOnAddUpdate(msgEror);
                ReturnFinalGridSelectedItemArray = [];
                $.each(data, function (index, item) {
                    if (item.ErrorMessagerow != null) {
                        var item = new ReturnItemsToAddToTemporaryTable(issueToSelected, chargeTypeSelected, chargeToClientLookuopId, data.objPart.ClientLookupId, data.objPart.Description, data.objPart.UPCCode, quantitySelected, errMsg, personnelId, ChargeToId, PartId, Comments, StoreroomId, StoreroomName);
                        ReturnFinalGridSelectedItemArray.push(item);
                    }
                });
                if (typeof finalReturnSelectInventoryTable != 'undefined') {
                    finalReturnSelectInventoryTable.destroy();
                }
                GenerateReturnFinalSelectInventoryTable(ReturnFinalGridSelectedItemArray);
                CloseLoader();
            }
            else {
                finalReturnSelectInventoryTable.clear().draw();
                ReturnFinalGridSelectedItemArray = [];
                SuccessAlertSetting.text = getResourceValue("CheckinProcessCompleteSuccessAlert");
                swal(SuccessAlertSetting, function () {
                    $(document).find("#finselectReturncontainer").hide();
                });
            }
            $(document).find("#inventoryCheckoutReturnModel_ChargeType").val("").trigger('change');
            var areaddescribedby = $(document).find("#inventoryCheckoutReturnModel_ChargeType").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find("#inventoryCheckoutReturnModel_ChargeType").removeClass("input-validation-error");
            $(document).find("#txtReturnChargeToId").val("");
            $(document).find("#txtPartReturnId").val("");
            $(document).find('form').find("#txtPartReturnId").removeClass("input-validation-error");
            $(document).find('form').find("#txtReturnChargeToId").removeClass("input-validation-error");
            $(document).find("#inventoryCheckoutReturnModel_Quantity").val("1");
            $(document).find("#inventoryCheckoutReturnModel_Comments").val(" ");
            if ($(document).find('#MultiStoreroom').val() == "True") {
                $(document).find("#inventoryCheckoutReturnModel_StoreroomId").val("").trigger('change');
                var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
                $('#' + areaddescribedby).hide();
                $(document).find('form').find(".ddlStoreroom").removeClass("input-validation-error");
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion

//#region returnpart tab

$(document).on("change", "#inventoryCheckoutReturnModel_ChargeType", function () {
    var option = '';
    chargeTypeText = $('option:selected', this).text();
    var type = $(this).val();
    chargeTypeSelected = type;
    if (type == "") {
        $(document).find("#opengridreturn").hide();
        option = "--Select--";
        $(document).find("#inventoryCheckoutReturnModel_ChargeToId").val("").trigger('change');
        $("#inventoryCheckoutReturnModel_ChargeToId").attr('disabled', 'disabled');
        $(document).find("#imgChargeToTreereturn").hide();
    }
    else {
        if (type == "Equipment") {
            $(document).find("#imgChargeToTreereturn").show();
            /*$(document).find('#txtReturnChargeToId').parents('.inputText').addClass('wthAdjst2');*/
        }
        else {
            $(document).find("#imgChargeToTreereturn").hide();
           /* $(document).find('#txtReturnChargeToId').parents('.inputText').removeClass('wthAdjst2');*/
        }
        $("#inventoryCheckoutReturnModel_ChargeToId").removeAttr('disabled');
        $(document).find("#opengridreturn").show();
    }
    $(document).find("#txtReturnChargeToId").val("");
    var tlen = $(document).find("#inventoryCheckoutReturnModel_ChargeType").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#inventoryCheckoutReturnModel_ChargeType").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#inventoryCheckoutReturnModel_ChargeType").removeClass("input-validation-error");
    }
    else {
        var areaddescribedby = $(document).find("#inventoryCheckoutReturnModel_ChargeType").attr('aria-describedby');
        $('#' + areaddescribedby).show();
        $(document).find('form').find("#inventoryCheckoutReturnModel_ChargeType").addClass("input-validation-error");
    }
});
$(document).on("change", "#inventoryCheckoutReturnModel_ChargeToId", function () {
    chargeToSelected = $(this).val();
    chargeToText = $('option:selected', this).text();
    var tlen = $(document).find("#inventoryCheckoutReturnModel_ChargeToId").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#inventoryCheckoutReturnModel_ChargeToId").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#inventoryCheckoutReturnModel_ChargeToId").removeClass("input-validation-error");
    }
    else {
        var areaddescribedby = $(document).find("#inventoryCheckoutReturnModel_ChargeToId").attr('aria-describedby');
        $('#' + areaddescribedby).show();
        $(document).find('form').find("#inventoryCheckoutReturnModel_ChargeToId").addClass("input-validation-error");
    }
});
$("#returnparttab").on('click', function () {
    $.ajax({
        url: '/InventoryCheckout/LoadReturnPart',
        type: 'POST',
        data: {},
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#ReturnPart').html(data);
        },
        complete: function () {
            CloseLoader();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            CloseLoader();
        }
    });
})
//#endregion
//#endregion
