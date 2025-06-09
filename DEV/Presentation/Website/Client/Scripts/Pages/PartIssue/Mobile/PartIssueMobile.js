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
$(document).ready(function () {
    mobiscroll.settings = {
        lang: 'en',                                       // Specify language like: lang: 'pl' or omit setting to use default
        theme: 'ios',                                     // Specify theme like: theme: 'ios' or omit setting to use default
        themeVariant: 'light'                             // More info about themeVariant: https://docs.mobiscroll.com/4-10-9/javascript/popup#opt-themeVariant
    };
    $(document).find('.select2picker').select2({});

    $("#partIssueModel_ChargeToId").attr('disabled', 'disabled');
    $(document).find("#imgChargeToTree").hide();
    $(document).on('change', '.ddlStoreroom', function () {
        $(document).find('#txtPartId').val('');
        $(document).find('#hdnPartId').val('');
        if ($(this).val() != '') {
            $(document).find('#openpartgrid').closest('.mbsc-col-12').show();
            $(document).find('#btnQrScanner').closest('.mbsc-col-12').show();
        } else {
            $(document).find('#openpartgrid').closest('.mbsc-col-12').hide();
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

    loadIssueParts();
});

$(document).on("change", "#partIssueModel_ChargeType", function () {


    chargeTypeText = $('option:selected', this).text();
    var type = $(this).val();
    chargeTypeSelected = type;
    $(document).find("#txtChargeToId").val("");
    $(document).find("#hdnChargeToId").val("");
    if (type == "") {
        $(document).find("#imgChargeToTree").hide();
        $("#hdnChargeToId").attr('disabled', 'disabled');
        $(document).find("#opengrid").hide();
    }
    else {
        if ($(document).find("#opengrid").hasClass('OpenWOModalPopupGridWorkorderModal')) {
            $(document).find("#opengrid").removeClass("OpenWOModalPopupGridWorkorderModal");
        }
        if ($(document).find("#opengrid").hasClass('EquipmentTableModalPopup_Mobile')) {
            $(document).find("#opengrid").removeClass("EquipmentTableModalPopup_Mobile");
        }
        if (type == "Equipment") {
            $(document).find("#imgChargeToTree").show();
            $(document).find("#opengrid").addClass("EquipmentTableModalPopup_Mobile");
            $(document).find(".btnEquipmentQrScannerCls").show();
            $(document).find(".btnWorkOrderQrScannerCls").hide();
        }
        else {

            $(document).find("#opengrid").addClass("OpenWOModalPopupGridWorkorderModal");
            $(document).find("#imgChargeToTree").hide();
            $(document).find(".btnWorkOrderQrScannerCls").show();
            $(document).find(".btnEquipmentQrScannerCls").hide();
        }
        $("#hdnChargeToId").removeAttr('disabled');
        $(document).find("#opengrid").show();

    }
    var tlen = $(document).find("#partIssueModel_ChargeType").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#partIssueModel_ChargeType").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#partIssueModel_ChargeType").removeClass("input-validation-error");
    }
    else {
        var areaddescribedby = $(document).find("#partIssueModel_ChargeType").attr('aria-describedby');
        $('#' + areaddescribedby).show();
        $(document).find('form').find("#partIssueModel_ChargeType").addClass("input-validation-error");
    }
});
$(document).on("change", "#partIssueModel_ChargeToId", function () {
    chargeToSelected = $(this).val();
    chargeToText = $('option:selected', this).text();
    var tlen = $(document).find("#partIssueModel_ChargeToId").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#partIssueModel_ChargeToId").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#partIssueModel_ChargeToId").removeClass("input-validation-error");
    }
    else {
        var areaddescribedby = $(document).find("#partIssueModel_ChargeToId").attr('aria-describedby');
        $('#' + areaddescribedby).show();
        $(document).find('form').find("#partIssueModel_ChargeToId").addClass("input-validation-error");
    }
});
$(document).on('change', '.ddlStoreroom', function () {
    var IssueFormExist = $(document).find("#hdnIssueToId");
    var ReturnFormExist = $(document).find("#hdnReturnToId");
    if ($(this).val() != '') {
        $(document).find('#openpartgrid').show();
        if (IssueFormExist.length > 0) {
            $(document).find('#btnQrScanner').show();
        }
        if (ReturnFormExist.length > 0) {
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
function SetControls_Mobile() {
    var errClass = 'mobile-validation-error';
    CloseLoader();
    $.validator.setDefaults({
        ignore: null,
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
    $('#IssueParts').trigger('mbsc-enhance');
    $(document).find('.mobiscrollselect:not(:disabled)').mobiscroll().select({
        display: 'bubble',
        filter: true,
        group: {
            groupWheel: false,
            header: false
        },
    });
    $(document).find('.mobiscrollselect:disabled').mobiscroll().select({
        disabled: true
    });
    $('#partIssueModel_Quantity').mobiscroll().numpad({
        max: 99999999.99,
        maxScale: 2,
        preset: 'decimal',
        thousandsSeparator: '',
        entryMode: 'freeform'
    });
    var x = parseFloat($('#partIssueModel_Quantity').val()) == 0 ? '' : $('#partIssueModel_Quantity').val();
    $('#partIssueModel_Quantity').mobiscroll('setVal', x);

    $('#partIssueReturnModel_Quantity').mobiscroll().numpad({
        maxScale: 2,
        preset: 'decimal',
        thousandsSeparator: '',
        entryMode: 'freeform'
    });
    var x = parseFloat($('#partIssueReturnModel_Quantity').val()) == 0 ? '' : $('#partIssueReturnModel_Quantity').val();
    $('#partIssueReturnModel_Quantity').mobiscroll('setVal', x);
    SetFixedHeadStyle();
}

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
        personnelId = $(document).find("#hdnIssueToId").val();
        quantitySelected = $(document).find("#partIssueModel_Quantity").val();
        chargeToSelected = $(document).find("#hdnChargeToId").val();
        chargeToClientLookupId = $(document).find("#txtChargeToId").val();
        partidSelected = $(document).find("#hdnPartId").val();
        comments = $(document).find("#partIssueModel_Comments").val();
        if ($(document).find(".ddlStoreroom").length > 0) {
            StoreroomId = $(document).find("#StoreroomId").val();
            StoreroomName = $(document).find(".ddlStoreroom").find("option:selected").text()
        } else {
            StoreroomId = 0;
            StoreroomName = "";
        }
        $(document).find("#finselectcontainer").show();
        $(document).find("#dvInventorySelectTable").show();
        $(document).find("#dvIdBttns").show();
        $.ajax({
            url: "/PartIssue/PopulateIssuePartSelectTable",
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
                    var item = new ItemsToAddToTemporaryTable(IssueToClientLookupId, chargeTypeSelected, data.chargeToClientLookupId, data.objPart.ClientLookupId, data.objPart.Description, data.objPart.UPCCode, quantitySelected, errMsg, personnelId, data.PartStoreroomId, chargeToSelected, data.objPart.PartId, chargeTypeText, data.Comments, data.StoreroomId, data.StoreroomName);
                    FinalGridSelectedItemArray.push(item);
                    var areaddescribedby;
                    if (typeof finalSelectInventoryTable != 'undefined') {
                        finalSelectInventoryTable.destroy();
                    }
                    GenerateFinalSelectInventoryTable(FinalGridSelectedItemArray);
                    $(document).find("#txtPartId").val("");
                    $(document).find("#hdnPartId").val("");
                    areaddescribedby = $(document).find("#txtPartId").attr('aria-describedby');
                    $('#' + areaddescribedby).hide();
                    $(document).find('form').find("#txtPartId").removeClass("input-validation-error");
                    $(document).find("#partIssueModel_Quantity").val("");
                    $('#partIssueModel_Quantity').mobiscroll('setVal', '');
                    $(document).find("#partIssueModel_Comments").val("");
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
            url: "/PartIssue/GetDataTableLanguageJson?nGrid=" + true,
        },
        "order": [[0, "asc"]],
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        data: data,
        columnDefs: [
            {
                targets: [8], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-danger delItem gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                },
            },
            {
                "targets": [9],
                "visible": false,
                "searchable": false
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
            }
            ,
            {
                "targets": [13],
                "visible": false,
                "searchable": false
            }
        ],
        "columns":
            [
                { "data": "IssueToClentLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
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

//#endregion
//#region Return ConfirmItems
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
        url: "/PartIssue/ReturnConfirmInventorydata",
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
            var areaddescribedby = $(document).find("#partIssueReturnModel_ChargeType").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find("#partIssueReturnModel_ChargeType").removeClass("input-validation-error");
            $(document).find("#txtReturnChargeToId").val("");
            $(document).find("#txtPartReturnId").val("");
            $(document).find("#txtReturnToId").val("");
            var areaddescribedby = $(document).find("#txtReturnChargeToId").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            var areaddescribedby = $(document).find("#txtPartReturnId").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            var areaddescribedby = $(document).find("#txtReturnToId").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find("#partIssueReturnModel_Quantity").val("");
            $('#partIssueReturnModel_Quantity').mobiscroll('setVal', '');
            $(document).find("#partIssueReturnModel_Comments").val("");
            if ($(document).find('#MultiStoreroom').val() == "True") {
                var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
                $('#' + areaddescribedby).hide();
                $(document).find('form').find(".ddlStoreroom").removeClass("input-validation-error");
            }
            $(document).find('form').find("input").removeClass("input-validation-error");

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
//#region Issue ConfirmItems
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
        url: "/PartIssue/ConfirmInventorydata",
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
            var areaddescribedby = $(document).find("#partIssueModel_ChargeType").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find("#partIssueModel_ChargeType").removeClass("input-validation-error");
            $(document).find("#txtChargeToId").val("");
            $(document).find("#txtPartId").val("");
            $(document).find("#txtIssueToId").val("");
            var areaddescribedby = $(document).find("#txtChargeToId").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            var areaddescribedby = $(document).find("#txtPartId").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            var areaddescribedby = $(document).find("#txtIssueToId").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find("#partIssueModel_Quantity").val("");
            $('#partIssueModel_Quantity').mobiscroll('setVal', '');
            $(document).find("#partIssueModel_Comments").val("");
            if ($(document).find('#MultiStoreroom').val() == "True") {
                var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
                $('#' + areaddescribedby).hide();
                $(document).find('form').find(".ddlStoreroom").removeClass("input-validation-error");
            }
            $(document).find('form').find("input").removeClass("input-validation-error");
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
function generateEquipmentTree(assignedid) {
    $.ajax({
        url: '/PlantLocationTree/EquipmentHierarchyTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        cache: false,
        beforeSend: function () {
            ShowLoader();
            $(document).find(".cntTree").html("<b>Processing...</b>");
        },
        success: function (data) {
            $(document).find(".cntTree").html('');
            $(document).find(".cntTree").html(data);
        },
        complete: function () {
            CloseLoader();
            treeTable($(document).find(".cntTree").find('#tblTree'));
            $(document).find('.radSelect').each(function () {


                if ($(this).data('equipmentid') == $(document).find('#hdnChargeToId').val()) {
                    $(this).attr('checked', true);
                }
            });
            $.each($(document).find(".cntTree").find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,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"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find(".cntTree").find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element           
            $('#commonWOTreeModal').addClass('slide-active');//.trigger('mbsc-enhance');

            $('#tblTree tr td').removeAttr('style');// code to remove the style applied from treetable.js -- white-space: nowrap;
        },
        error: function (xhr) {
            alert('error');
        }
    });
}
$(document).on('change', '.radSelect', function () {
    equipmentid = $(this).data('equipmentid');
    var clientlookupid = $(this).data('clientlookupid').split("(")[0];
    $('#commonWOTreeModal').removeClass('slide-active');
    $(document).find('#hdnId').val(equipmentid);
    if (typeof activeTab !== 'undefined' && activeTab == "#Returnpart") {
        $(document).find('#hdnReturnChargeToId').val(equipmentid);
        $(document).find('#txtReturnChargeToId').val(clientlookupid).trigger('change');
        $(document).find('#txtReturnChargeToId').closest('form').valid();
        $(document).find('form').find("#txtReturnChargeToId").removeClass("input-validation-error");

    }
    else {
        $(document).find('#hdnChargeToId').val(equipmentid);
        $(document).find('#txtChargeToId').val(clientlookupid).trigger('change');
        $(document).find('#txtChargeToId').closest('form').valid();
        $(document).find('form').find("#txtChargeToId").removeClass("input-validation-error");

    }
});
$(document).on('click', "#commonWOTreeModalHide", function () {
    $('#commonWOTreeModal').removeClass('slide-active');
});
//#endregion


//#region QR Reader
$(document).on('click', '#btnQrScanner', function () {
    $(document).find('form').find("#txtPartId").removeClass("input-validation-error");
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
    var url = "/PartIssue/GetPartIdByClientLookUpId?clientLookUpId=" + decodedText;
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
                    $(document).find('#txtPartReturnId').val(decodedText).trigger('mbsc-enhance');;
                    $(document).find('#hdnPartReturnId').val(data.PartId);
                }
                else {
                    $(document).find('#txtPartId').val(decodedText).trigger('mbsc-enhance');;
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

$(document).on('click', '#btnIssueToQrScanner', function () {
    $(document).find('form').find("#txtChargeToId").removeClass("input-validation-error");
    $(document).find('#txtIssueToId').val('');
    $(document).find('#hdnIssueToId').val('');
    if (!$(document).find('#QrCodeReaderModal').hasClass('show')) {
        $(document).find('#QrCodeReaderModal').modal("show");
        StartQRReaderIssueToCheckout();
    }
});
function StartQRReaderIssueToCheckout() {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 150,
            }, success => {
                onScanSuccessIssueToCheckoutMobile(success);
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
function onScanSuccessIssueToCheckoutMobile(decodedText) {
    var url = "/PartIssue/GetPersonnelIdByClientLookUpId?clientLookUpId=" + decodedText;
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
            if (data.PersonnelId > 0) {
                if (typeof activeTab !== 'undefined' && activeTab == "#Returnpart") {
                    $(document).find('#txtReturnChargeToId').val(decodedText).trigger('mbsc-enhance');;
                    $(document).find('#hdnReturnChargeToId').val(data.PersonnelId);
                }
                else {
                    $(document).find('#txtIssueToId').val(decodedText).trigger('mbsc-enhance');;
                    $(document).find('#hdnIssueToId').val(data.PersonnelId);
                }


            }
            else {
                //Show Error Swal
                ShowErrorAlert(getResourceValue('spnInvalidQrCodePersonnelMsg').replace('${decodedText}', decodedText));
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

$(document).on('click', '#btnEquipmentQrScanner', function () {
    $(document).find('form').find("#txtChargeToId").removeClass("input-validation-error");
    $(document).find('#txtChargeToId').val('');
    $(document).find('#hdnChargeToId').val('');
    if (!$(document).find('#QrCodeReaderModal').hasClass('show')) {
        $(document).find('#QrCodeReaderModal').modal("show");
        StartQRReaderEquipmentCheckout();
    }
});
function StartQRReaderEquipmentCheckout() {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 150,
            }, success => {
                onScanSuccessEquipmentCheckoutMobile(success);
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
function onScanSuccessEquipmentCheckoutMobile(decodedText) {
    var url = "/PartIssue/GetEquipmentIdByClientLookUpId?clientLookUpId=" + decodedText;
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
            if (data.EquipmentId > 0) {
                $(document).find('#hdnId').val(data.EquipmentId);
                if (typeof activeTab !== 'undefined' && activeTab == "#Returnpart") {
                    $(document).find('#txtReturnChargeToId').val(decodedText).trigger('mbsc-enhance');;
                    $(document).find('#hdnReturnChargeToId').val(data.EquipmentId);
                }
                else {
                    $(document).find('#txtChargeToId').val(decodedText).trigger('mbsc-enhance');;
                    $(document).find('#hdnChargeToId').val(data.EquipmentId);
                }

            }
            else {
                //Show Error Swal
                ShowErrorAlert(getResourceValue('spnInvalidQrCodeAssetMsg').replace('${decodedText}', decodedText));
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
$(document).on('click', '#btnWorkOrderQrScanner', function () {
    $(document).find('form').find("#txtChargeToId").removeClass("input-validation-error");
    $(document).find('#txtChargeToId').val('');
    $(document).find('#hdnChargeToId').val('');
    if (!$(document).find('#QrCodeReaderModal').hasClass('show')) {
        $(document).find('#QrCodeReaderModal').modal("show");
        StartQRReaderWorkOrderCheckout();
    }
});
function StartQRReaderWorkOrderCheckout() {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 150,
            }, success => {
                onScanSuccessWorkorderCheckoutMobile(success);
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
function onScanSuccessWorkorderCheckoutMobile(decodedText) {
    var url = "/PartIssue/GetWorkorderIdByClientLookUpId?clientLookUpId=" + decodedText;
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
            if (data.WorkOrderId > 0) {
                if (typeof activeTab !== 'undefined' && activeTab == "#Returnpart") {
                    $(document).find('#txtReturnChargeToId').val(decodedText).trigger('mbsc-enhance');;
                    $(document).find('#hdnReturnChargeToId').val(data.WorkOrderId);
                }
                else {
                    $(document).find('#txtChargeToId').val(decodedText).trigger('mbsc-enhance');;
                    $(document).find('#hdnChargeToId').val(data.WorkOrderId);
                }

            }
            else {
                //Show Error Swal
                ShowErrorAlert(getResourceValue('spnInvalidQrCodeWorkOrderMsg').replace('${decodedText}', decodedText));
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


//#endregion


function loadReturnParts() {
    $("#partIssueReturnModel_ChargeToId").attr('disabled', 'disabled');
    $(document).find("#imgChargeToTreereturn").hide();
    $(document).find('#IssueParts').html();
    $(document).find('#finselectIssuecontainer').html();
    $.ajax({
        url: '/PartIssue/LoadReturnPartMobile',
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
            $(document).find("#partIssueReturnModel_ChargeType").val("WorkOrder").trigger('change');
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $(document).find('.select2picker').select2({});
            if (ReturnFinalGridSelectedItemArray.length != '0') {
                $(document).find("#finselectReturncontainer").show();
                $(document).find("#dvInventoryReturnSelectTable").show();
                $(document).find("#dvIdReturnBttns").show();
                GenerateReturnFinalSelectInventoryTable(ReturnFinalGridSelectedItemArray);
            }
            var MultiStoreroom = $(document).find('#MultiStoreroom').val();
            var StoreroomId = $(document).find('#StoreroomId').val();
            if (MultiStoreroom == "True" && StoreroomId == 0) {
                $(document).find('#openpartgrid').hide();
                $(document).find('#btnQrScannerreturn').hide();
            }
            $(document).find('#ReturnParts').trigger('mbsc-enhance');
            SetControls_Mobile();

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
        url: '/PartIssue/LoadIssuePartsMobile',
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
            $(document).find("#partIssueModel_ChargeType").val("WorkOrder").trigger('change');
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $(document).find('.select2picker').select2({});
            $(document).find("#imgChargeToTree").hide();
            if (FinalGridSelectedItemArray.length != '0') {
                $(document).find("#finselectcontainer").show();
                $(document).find("#dvInventorySelectTable").show();
                $(document).find("#dvIdBttns").show();
                GenerateFinalSelectInventoryTable(FinalGridSelectedItemArray);
            }
            if ($(document).find('#MultiStoreroom').val() == "True" && $(document).find('#StoreroomId').val() == "0") {
                $(document).find('#openpartgrid').hide();
                $(document).find('#btnQrScanner').hide();
            }

            $(document).find('#IssueParts').trigger('mbsc-enhance');
            SetControls_Mobile();

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


$(document).on('click', '#btnClearIssuePartAllItems', function () {
    var areaddescribedby = $(document).find("#partIssueModel_ChargeType").attr('aria-describedby');
    $('#' + areaddescribedby).hide();
    $(document).find('form').find("#partIssueModel_ChargeType").removeClass("input-validation-error");
    $(document).find("#txtChargeToId").val("");
    $(document).find("#txtPartId").val("");
    $(document).find("#txtIssueToId").val("");
    var areaddescribedby = $(document).find("#txtChargeToId").attr('aria-describedby');
    $('#' + areaddescribedby).hide();
    var areaddescribedby = $(document).find("#txtPartId").attr('aria-describedby');
    $('#' + areaddescribedby).hide();
    var areaddescribedby = $(document).find("#txtIssueToId").attr('aria-describedby');
    $('#' + areaddescribedby).hide();
    $(document).find("#partIssueModel_Quantity").val("");
    $('#partIssueModel_Quantity').mobiscroll('setVal', '');
    $(document).find("#partIssueModel_Comments").val("");
    if ($(document).find('#MultiStoreroom').val() == "True") {
        var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find(".ddlStoreroom").removeClass("input-validation-error");
    }
    $(document).find('form').find("input").removeClass("input-validation-error");
});




//#region  Return Part
//#region FillGrid
var ReturnFinalGridSelectedItemArray = [];

$(document).on('click', '#btnReturnToQrScanner', function () {
    $(document).find('form').find("#txtReturnToId").removeClass("input-validation-error");
    $(document).find('#txtReturnToId').val('');
    $(document).find('#hdnReturnToId').val('');
    if (!$(document).find('#QrCodeReaderModal').hasClass('show')) {
        $(document).find('#QrCodeReaderModal').modal("show");
        StartQRReaderReturnToCheckout();
    }
});
function StartQRReaderReturnToCheckout() {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 150,
            }, success => {
                onScanSuccessReturnToCheckoutMobile(success);
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
function onScanSuccessReturnToCheckoutMobile(decodedText) {
    var url = "/PartIssue/GetPersonnelIdByClientLookUpId?clientLookUpId=" + decodedText;
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
            if (data.PersonnelId > 0) {
                if (typeof activeTab !== 'undefined' && activeTab == "#Returnpart") {
                    $(document).find('#txtReturnToId').val(decodedText).trigger('mbsc-enhance');;
                    $(document).find('#hdnReturnToId').val(data.PersonnelId);
                }
                else {
                    $(document).find('#txtIssueToId').val(decodedText).trigger('mbsc-enhance');;
                    $(document).find('#hdnIssueToId').val(data.PersonnelId);
                }


            }
            else {
                //Show Error Swal
                ShowErrorAlert(getResourceValue('spnInvalidQrCodePersonnelMsg').replace('${decodedText}', decodedText));
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
        personnelId = $(document).find("#hdnReturnToId").val();
        quantitySelected = $(document).find("#partIssueReturnModel_Quantity").val();
        chargeToSelected = $(document).find("#hdnReturnChargeToId").val();
        chargeToClientLookupId = $(document).find("#txtReturnChargeToId").val();
        partidSelected = $(document).find("#hdnPartReturnId").val();
        comments = $(document).find("#partIssueReturnModel_Comments").val();
        if ($(document).find(".ddlStoreroom").length > 0) {
            StoreroomId = $(document).find("#StoreroomId").val();
            StoreroomName = $(document).find(".ddlStoreroom").find("option:selected").text()
        } else {
            StoreroomId = 0;
            StoreroomName = "";
        }
        $(document).find("#finselectReturncontainer").show();
        $(document).find("#dvInventoryReturnSelectTable").show();
        $(document).find("#dvIdReturnBttns").show();
        $.ajax({
            url: "/PartIssue/PopulateIssuePartSelectTable",
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
                    $(document).find("#txtPartReturnId").val("");
                    $(document).find("#hdnPartReturnId").val("");
                    areaddescribedby = $(document).find("#txtPartReturnId").attr('aria-describedby');
                    $('#' + areaddescribedby).hide();
                    $(document).find('form').find("#txtPartReturnId").removeClass("input-validation-error");
                    $(document).find("#partIssueReturnModel_Quantity").val("");
                    $('#partIssueReturnModel_Quantity').mobiscroll('setVal', '');
                    $(document).find("#partIssueReturnModel_Comments").val("");
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
                targets: [8], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-danger delReturnItem gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                },
            },
            {
                "targets": [9],
                "visible": false,
                "searchable": false
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
        ],
        "columns":
            [
                { "data": "IssueToClentLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
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
        window.location.href = "../LogIn/LogOut";
    });
})

$(document).on('click', '#btnReturnClearAllItems', function () {
    var areaddescribedby = $(document).find("#partIssueReturnModel_ChargeType").attr('aria-describedby');
    $('#' + areaddescribedby).hide();
    $(document).find('form').find("#partIssueReturnModel_ChargeType").removeClass("input-validation-error");
    $(document).find("#txtReturnChargeToId").val("");
    $(document).find("#txtPartReturnId").val("");
    $(document).find("#txtReturnToId").val("");
    var areaddescribedby = $(document).find("#txtReturnChargeToId").attr('aria-describedby');
    $('#' + areaddescribedby).hide();
    var areaddescribedby = $(document).find("#txtPartReturnId").attr('aria-describedby');
    $('#' + areaddescribedby).hide();
    var areaddescribedby = $(document).find("#txtReturnToId").attr('aria-describedby');
    $('#' + areaddescribedby).hide();
    $(document).find("#partIssueReturnModel_Quantity").val("");
    $('#partIssueReturnModel_Quantity').mobiscroll('setVal', '');
    $(document).find("#partIssueReturnModel_Comments").val("");
    if ($(document).find('#MultiStoreroom').val() == "True") {
        var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find(".ddlStoreroom").removeClass("input-validation-error");
    }
    $(document).find('form').find("input").removeClass("input-validation-error");
});

$(document).on('click', '#btnEquipmentQrScannerReturn', function () {
    $(document).find('form').find("#txtReturnChargeToId").removeClass("input-validation-error");
    $(document).find('#txtReturnChargeToId').val('');
    $(document).find('#hdnReturnChargeToId').val('');
    if (!$(document).find('#QrCodeReaderModal').hasClass('show')) {
        $(document).find('#QrCodeReaderModal').modal("show");
        StartQRReaderEquipmentCheckoutReturn();
    }
});
function StartQRReaderEquipmentCheckoutReturn() {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 150,
            }, success => {
                onScanSuccessEquipmentCheckoutReturnMobile(success);
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
function onScanSuccessEquipmentCheckoutReturnMobile(decodedText) {
    var url = "/PartIssue/GetEquipmentIdByClientLookUpId?clientLookUpId=" + decodedText;
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
            if (data.EquipmentId > 0) {
                $(document).find('#hdnId').val(data.EquipmentId);
                if (typeof activeTab !== 'undefined' && activeTab == "#Returnpart") {
                    $(document).find('#txtReturnChargeToId').val(decodedText).trigger('mbsc-enhance');;
                    $(document).find('#hdnReturnChargeToId').val(data.EquipmentId);
                }
                else {
                    $(document).find('#txtChargeToId').val(decodedText).trigger('mbsc-enhance');;
                    $(document).find('#hdnChargeToId').val(data.EquipmentId);
                }


            }
            else {
                //Show Error Swal
                ShowErrorAlert(getResourceValue('spnInvalidQrCodeAssetMsg').replace('${decodedText}', decodedText));
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
$(document).on('click', '#btnWorkOrderQrScannerReturn', function () {
    $(document).find('form').find("#txtReturnChargeToId").removeClass("input-validation-error");
    $(document).find('#txtReturnChargeToId').val('');
    $(document).find('#hdnReturnChargeToId').val('');
    if (!$(document).find('#QrCodeReaderModal').hasClass('show')) {
        $(document).find('#QrCodeReaderModal').modal("show");
        StartQRReaderWorkOrderCheckoutReturn();
    }
});
function StartQRReaderWorkOrderCheckoutReturn() {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 150,
            }, success => {
                onScanSuccessWorkorderCheckoutReturnMobile(success);
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
function onScanSuccessWorkorderCheckoutReturnMobile(decodedText) {
    var url = "/PartIssue/GetWorkorderIdByClientLookUpId?clientLookUpId=" + decodedText;
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
            if (data.WorkOrderId > 0) {
                if (typeof activeTab !== 'undefined' && activeTab == "#Returnpart") {
                    $(document).find('#txtReturnChargeToId').val(decodedText).trigger('mbsc-enhance');;
                    $(document).find('#hdnReturnChargeToId').val(data.WorkOrderId);
                }
                else {
                    $(document).find('#txtChargeToId').val(decodedText).trigger('mbsc-enhance');;
                    $(document).find('#hdnChargeToId').val(data.WorkOrderId);
                }


            }
            else {
                //Show Error Swal
                ShowErrorAlert(getResourceValue('spnInvalidQrCodeWorkOrderMsg').replace('${decodedText}', decodedText));
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

//#endregion
$(document).on('click', '#btnQrScannerreturn', function () {
    $(document).find('form').find("#txtPartReturnId").removeClass("input-validation-error");
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

$(document).on('click', '#btnCancelIssuePartAllItems', function () {
    swal(CancelAlertSetting, function () {
        finalSelectInventoryTable.clear().draw();
        FinalGridSelectedItemArray = [];
        window.location.href = "../LogIn/LogOut";
    });
});


//#region returnpart tab


$(document).on('click', '#btnCancelReturnPartAllItems', function () {
    $(document).find("#partIssueReturnModel_ChargeType").val("").trigger('change');
    var areaddescribedby = $(document).find("#partIssueReturnModel_ChargeType").attr('aria-describedby');
    $('#' + areaddescribedby).hide();
    $(document).find('form').find("#partIssueReturnModel_ChargeType").removeClass("input-validation-error");
    $(document).find("#txtReturnChargeToId").val("");
    $(document).find("#txtPartReturnId").val("");
    $(document).find("#txtReturnToId").val("");
    $(document).find('form').find("#txtReturnChargeToId").removeClass("input-validation-error");
    $(document).find('form').find("#txtPartReturnId").removeClass("input-validation-error");
    $(document).find('form').find("#txtReturnToId").removeClass("input-validation-error");
    $(document).find("#partIssueReturnModel_Quantity").val("");
    $(document).find("#partIssueReturnModel_Comments").val("");
    if ($(document).find('#MultiStoreroom').val() == "True") {
        $(document).find("#partIssueReturnModel_StoreroomId").val("").trigger('change');
        var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find(".ddlStoreroom").removeClass("input-validation-error");
    }
});


$(document).on("change", "#partIssueReturnModel_ChargeType", function () {


    chargeTypeText = $('option:selected', this).text();
    var type = $(this).val();
    chargeTypeSelected = type;
    $(document).find("#txtReturnChargeToId").val("");
    $(document).find("#hdnReturnChargeToId").val("");
    if (type == "") {
        $(document).find("#imgChargeToTreereturn").hide();
        $("#hdnReturnChargeToId").attr('disabled', 'disabled');
        $(document).find("#opengridreturn").hide();
    }
    else {
        if ($(document).find("#opengridreturn").hasClass('OpenWOModalPopupGridWorkorderModal')) {
            $(document).find("#opengridreturn").removeClass("OpenWOModalPopupGridWorkorderModal");
        }
        if ($(document).find("#opengridreturn").hasClass('EquipmentTableModalPopup_Mobile')) {
            $(document).find("#opengridreturn").removeClass("EquipmentTableModalPopup_Mobile");
        }
        if (type == "Equipment") {
            $(document).find("#imgChargeToTreereturn").show();
            $(document).find("#opengridreturn").addClass("EquipmentTableModalPopup_Mobile");
            $(document).find(".btnEquipmentQrScannerReturnCls").show();
            $(document).find(".btnWorkOrderQrScannerReturnCls").hide();
        }
        else {

            $(document).find("#opengridreturn").addClass("OpenWOModalPopupGridWorkorderModal");
            $(document).find("#imgChargeToTreereturn").hide();
            $(document).find(".btnWorkOrderQrScannerReturnCls").show();
            $(document).find(".btnEquipmentQrScannerReturnCls").hide();
        }
        $("#hdnReturnChargeToId").removeAttr('disabled');
        $(document).find("#opengridreturn").show();

    }
    var tlen = $(document).find("#partIssueReturnModel_ChargeType").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#partIssueReturnModel_ChargeType").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#partIssueReturnModel_ChargeType").removeClass("input-validation-error");
    }
    else {
        var areaddescribedby = $(document).find("#partIssueReturnModel_ChargeType").attr('aria-describedby');
        $('#' + areaddescribedby).show();
        $(document).find('form').find("#partIssueReturnModel_ChargeType").addClass("input-validation-error");
    }
});


$("#returnparttab").on('click', function () {
    $.ajax({
        url: '/PartIssue/LoadReturnPart',
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
//#region  mobile Part
$(document).on('click', '#openpartgrid', function () {
    generatePartScrollViewForMobileMobiscroll();
});
function generatePartScrollViewForMobileMobiscroll() {
    PartListlength = 0;
    $.ajax({
        "url": "/Dashboard/PartLookupListView_Mobile",
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
    $('#partIdModal').addClass('slide-active');
}
$(document).on('click', '#btnPartLoadMore', function () {
    $(this).hide();
    PartListlength += PartPageLength;
    InitializePartListView_Mobile();
});
function BindPartDataForListView() {
    var Search = $(document).find('#txtPartSearch_Mobile').val();
    $.ajax({
        "url": "/Dashboard/GetPartLookupListchunksearch_Mobile",
        data: {
            Search: Search,
            Start: PartListlength,
            Length: PartPageLength,
            Storeroomid: ($(document).find('#MultiStoreroom').val() == 'True' ? $('#StoreroomId').val() : '')
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
$(document).on('click', '#partIdModalHide', function () {
    $(document).find('#partIdModal').removeClass("slide-active"); //..modal("hide");
    $('#txtPartSearch_Mobile').val('');
    $('#DivPartSearchScrollViewModal').html('');
});

$(document).on("keyup", '#txtPartSearch_Mobile', function (e) {
    if (e.keyCode == 13) {
        generatePartScrollViewForMobileMobiscroll();
    }
});
$(document).on('click', '.scrollview-partsearch', function (e) {
    if ($(document).find('#txtPartId').length > 0) {
        $(document).find('#txtPartId').val($(this).data('clientlookupid')).trigger('mbsc-enhance');
        $(document).find('#hdnPartId').val($(this).data('id'));
        $(document).find('#txtPartId').closest('form').valid();
    }
    else if ($(document).find('#txtPartReturnId').length > 0) {
        $(document).find('#txtPartReturnId').val($(this).data('clientlookupid')).trigger('mbsc-enhance');
        $(document).find('#hdnPartReturnId').val($(this).data('id'));
        $(document).find('#txtPartReturnId').closest('form').valid();
    }
    $('#partIdModalHide').trigger('click');
});
//#endregion



