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
   /* $(document).find('.select2picker').select2({});*/
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
    $(document).find("#opengrid").hide();
    $(document).find("#imgChargeToTree").hide();
    
   
    SetControls();
});
$(document).on("change", "#partsIssue_ChargeType", function () {

    var type = $(this).val();
    $(document).find("#txtChargeTo").val("");
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
        if ($(document).find("#opengrid").hasClass('OpenAssetModalPopupGridoverEquipmentModal')) {
            $(document).find("#opengrid").removeClass("OpenAssetModalPopupGridoverEquipmentModal");
        }
        if (type == "Equipment") {
            $(document).find("#imgChargeToTree").show();
            $(document).find("#opengrid").addClass("OpenAssetModalPopupGridoverEquipmentModal");
        }
        else {

            $(document).find("#opengrid").addClass("OpenWOModalPopupGridWorkorderModal");
            $(document).find("#imgChargeToTree").hide();
        }
        $("#hdnChargeToId").removeAttr('disabled');
        $(document).find("#opengrid").show();

    }

   
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
    $('#MobilePhInvntPartIssue').trigger('mbsc-enhance');
    $(document).find('.mobiscrollselect:not(:disabled)').mobiscroll().select({
        display: 'bubble',
        //touchUi: false,
        /*data: remoteData,*/
        filter: true,
        group: {
            groupWheel: false,
            header: false
        }
        //width: 400,
        //placeholder: 'Please Select...'

    });
    $(document).find('.mobiscrollselect:disabled').mobiscroll().select({
        disabled: true
    });
    $('#partsIssue_Quantity').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 99999999.99,
        //fill: 'ltr',
        maxScale: 2,
        preset: 'decimal',
        thousandsSeparator: '',
        entryMode: 'freeform'
    });
    var x = parseFloat($('#partsIssue_Quantity').val()) == 0 ? '' : $('#partsIssue_Quantity').val();
    $('#partsIssue_Quantity').mobiscroll('setVal', x);
    SetFixedHeadStyle();
}


$(document).on('click', '#openinvpartgrid', function () {
    $('#txtPartSearch_Mobile').val('');
    $(document).find("#PartListViewForSearch").html("");
    InitializePartListView_Mobile();
});


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
        "url": "/InventoryPartsIssue/GetPartLookupListchunksearch_Mobile",
        data: {
            Search: Search,
            Start: PartListlength,
            Length: PartPageLength,
            Storeroomid: ($('#MultiStoreroom').val() == 'True' ? $('#StoreroomId').val() : '')
        },
        type: 'POST',
        dataType: 'JSON',
        beforeSend: function () {
           /* ShowLoader();*/
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
   /* $('#DivPartSearchScrollViewModal').html('');*/
});
$(document).on("keyup", '#txtPartSearch_Mobile', function (e) {
    if (e.keyCode == 13) {
        $(document).find("#PartListViewForSearch").html("");
        InitializePartListView_Mobile();
    }
});
$(document).on('click', '.scrollview-partsearch', function (e) {
    $(document).find('#txtPartId').val($(this).data('clientlookupid')).trigger('mbsc-enhance');//.removeClass('input-validation-error');
    $(document).find('#txtPartId').closest('form').valid();
    $(document).find('#hdnPartId').val($(this).data('id'));
    $(document).find('#maintenancepartIdModal').removeClass("slide-active");
});



function funcRefreshPage(PersonnelId) {
    $(document).find('form').trigger("reset");
    $('#MobilePhInvntPartIssue').mobiscroll('destroy');
    $('#MobilePhInvntPartIssue').mobiscroll('getInst');
    $(document).find('#partsIssue_selectedPersonnelId').mobiscroll('setVal', PersonnelId).trigger('change');
    $(document).find('#StoreroomId').mobiscroll('setVal', PersonnelId).trigger('change');
    $(document).find("#partsIssue_ChargeType").mobiscroll('setVal', "").trigger('change');
    $('#hdnChargeToId').val("");
    $('#txtChargeTo').val("");
    $('#txtPartId').val("");
    $('#hdnPartId').val("");
    $('#partsIssue_Quantity').val("").mobiscroll('clear');
    $('#partsIssue_Comments').val("");
    if ($(document).find('#MultiStoreroom').val() == "True") {
        $(document).find("#partsIssue_StoreroomId").mobiscroll('setVal', "").trigger('change');
        var areaddescribedby = $(document).find(".ddlStoreroom").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find(".ddlStoreroom").removeClass("input-validation-error");
    }
    ResetErrorDiv();
    var areaddescribedby = $(document).find("#partsIssue_ChargeType").attr('aria-describedby');
    $('#' + areaddescribedby).hide();
    $(document).find('form').find("#partsIssue_ChargeType").removeClass("input-validation-error");

    var areaddescribedbyPartid = $(document).find("#txtPartId").attr('aria-describedby');
    $('#' + areaddescribedbyPartid).hide();
    $(document).find('form').find("#txtPartId").removeClass("input-validation-error");
    $('#MobilePhInvntPartIssue').trigger('mbsc-enhance');
    SetControls();
}

//#region QR Reader

function StartQRReader_Mobile(Module) {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 230,
                //aspectRatio: aspectratio //1.7777778
            }, success => {
                    onScanSuccessPartIssue(success);
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

function onScanSuccessPartIssue(decodedText) {
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
                
                    //$(document).find('#txtPartId').val(decodedText);
                    $(document).find('#txtPartId').val(decodedText).trigger('mbsc-enhance'); //.removeClass('input-validation-error');
                    $(document).find('#txtPartId').closest('form').valid();
                    $(document).find('#hdnPartId').val(data.PartId);
                
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


//region Asset trees
$(document).on('click', '#imgChargeToTree', function (e) {
    $(this).blur();
    generateEquipmentTree($(this).data('assignedid'));
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
                
                //else {
                ////var c_id = $(this).data('clientlookupid').split("(")[0];
                ////if (c_id != null && c_id.trim() == $(document).find('#' + assignedid).val()) {
                ////    $(this).attr('checked', true);
                ////}
                //}
            });
            $.each($(document).find(".cntTree").find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,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"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find(".cntTree").find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element
            //$('#commonWOTreeModal').modal('show');
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
    $(document).find('#hdnChargeToId').val(equipmentid);
    var clientlookupid = $(this).data('clientlookupid').split("(")[0];
    //$('#commonWOTreeModal').modal('hide');
    $('#commonWOTreeModal').removeClass('slide-active');
    $(document).find('#partsIssue_ChargeToClientLookupId').val(equipmentid).trigger('change');
    $(document).find('#txtChargeTo').val(clientlookupid).trigger('mbsc-enhance');
    $(document).find('#txtChargeTo').closest('form').valid(); //.val(clientlookupid).removeClass('input-validation-error');
});
$(document).on('click', "#commonWOTreeModalHide", function () {
    $('#commonWOTreeModal').removeClass('slide-active');
});
//endregion
function OnSuccessSavePartsIssue_Mobile(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("PartIssueAddAlert");
        if (data.btnType == "btnIssueAndAddAnother") {
            swal(SuccessAlertSetting, function () {
                /*window.location.href = "../InventoryPartsIssue/Index";*/
                funcRefreshPage(data.PersonnelId);
            });
        } else {
            swal(SuccessAlertSetting, function () {
                window.location.href = "../Dashboard/Dashboard";
            });
        }
    } else if (data.Result == "failed") {
        ErrorAlertSetting.text = data.errorMsg;
        swal(ErrorAlertSetting, function () {

        });
    }
}

$(document).on('click', '.clearTextBoxValue', function () {
    var id = $(this).data('txtboxid');
    $(document).find('#' + id).val('');
    if (id == 'txtWorkOrderSearch_Mobile') {
        InitializeWOListView_Mobile();
    }
    else if (id == 'txtEquipmentSearch_Mobile') {
        generateEquipmentDataTable_Mobile();
    }
});

$(document).on('click', '#btnPartIssuecancel', function () {
    swal(CancelAlertSetting, function () {
        window.location.href = "../Dashboard/Dashboard";
    });
});