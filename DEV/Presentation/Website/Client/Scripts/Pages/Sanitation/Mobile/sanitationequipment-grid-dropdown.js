﻿$(document).on('click', '.scrollview-equipmentsearch', function (e) {
    if ($(document).find('#txtChargeToId,#txtChargeTo').length > 0) {
        $(document).find('#txtChargeToId').val($(this).data('clientlookupid')).trigger('mbsc-enhance');//.removeClass('input-validation-error');
        $(document).find('#hdnChargeToId').val($(this).data('id'));
        var ChargeType = 'Equipment';
        var EquipmentId = $(this).data('id');
        var ClientLookUpId = $(this).data('clientlookupid');
        var Name = $(this).data('chargetoname');
        $(document).find('#hdnId').val(EquipmentId);
        if ($(document).find('#DemandModel_PlantLocationDescription').length > 0) {
            $(document).find('#DemandModel_PlantLocationDescription').val(ClientLookUpId + "(" + Name + ")").trigger('mbsc-enhance');
        }
        if ($(document).find('#TchargeType').length > 0) {
            $(document).find('#TchargeType').val(ChargeType);
        }
        if ($(document).find('#TplantLocationId').length > 0) {
            $(document).find('#TplantLocationId').val(EquipmentId);
        }
        if ($(document).find('#TplantLocationDescription').length > 0) {
            $(document).find('#TplantLocationDescription').val(ClientLookUpId);
        }
        if ($(document).find('#JobDetailsModel_PlantLocationDescription').length > 0) {
            $(document).find('#JobDetailsModel_PlantLocationDescription').val(ClientLookUpId + "(" + Name + ")");
        }
        if ($(document).find('#ODescribeModel_PlantLocationDescription').length > 0) {
            $(document).find('#ODescribeModel_PlantLocationDescription').val(ClientLookUpId + "(" + Name + ")");
            $(document).find('#ODescribeModel_PlantLocationDescription').removeClass('input-validation-error');
        }
        if ($(document).find('#MasterSanitModel_ChargeToDescription').length > 0) {
            $(document).find('#MasterSanitModel_ChargeToDescription').val(ClientLookUpId).trigger('mbsc-enhance');
            if ($(document).find('#MasterSanitModel_ChargeToName').length > 0) {
                $(document).find('#MasterSanitModel_ChargeToName').val(Name);
            }
        }
        if ($(document).find('#MasterSanitModel_ChargeType').length > 0) {
            $(document).find('#MasterSanitModel_ChargeType').val(ChargeType);
        }
        if ($(document).find('#MasterSanitModel_PlantLocationId').length > 0) {
            $(document).find('#MasterSanitModel_PlantLocationId').val(EquipmentId);
        }
        if ($(document).find('#MasterSanitModel_ChargeToClientLookupId').length > 0) {
            $(document).find('#MasterSanitModel_ChargeToClientLookupId').val(ClientLookUpId);
        }
        $(document).find('#txtChargeTo').val(ClientLookUpId + "(" + Name + ")").trigger('mbsc-enhance');
        //$(document).find('#txtChargeToId').closest('form').valid();
    }
    $('#SJEquipmentModalHide').trigger('click');
});
$(document).on('click', '#SJEquipmentModalHide', function () {
    AssetListlength = 0;
    $('#SJEquipmentModal').removeClass('slide-active');
    $('#txtEquipmentSearch_Mobile').val('');
});
$(document).on("keyup", '#txtEquipmentSearch_Mobile', function (e) {
    if (e.keyCode == 13) {
        $(document).find("#AssetListViewForSearch").html("");
        InitializeAssetListView_Mobile();
    }
});
var AssetListView,
    AssetListlength = 0,
    PageLength = 100;
function InitializeAssetListView_Mobile() {
    AssetListView = $('#AssetListViewForSearch').mobiscroll().listview({
        theme: 'ios',
        themeVariant: 'light',
        animateAddRemove: false,
        striped: true,
        swipe: false
    }).mobiscroll('getInst');
    BindAssetDataForListView();
    $('#EquipmentWOModal').addClass('slide-active');
}
$(document).on('click', '#btnAssetLoadMore', function () {
    $(this).hide();
    AssetListlength += PageLength;
    InitializeAssetListView_Mobile();
});
function BindAssetDataForListView() {
    var Search = $(document).find('#txtEquipmentSearch_Mobile').val();
    $.ajax({
        "url": "/SanitationJob/GetEquipmentLookupListchunksearch_Mobile",
        data: {
            Search: Search,
            Start: AssetListlength,
            Length: PageLength
        },
        type: 'POST',
        dataType: 'JSON',
        beforeSend: function () {
            ShowLoader();
            AssetListView.showLoading();
        },
        success: function (data) {
            var i, item, lihtml;
            for (i = 0; i < data.data.length; ++i) {
                item = data.data[i];
                lihtml = '';
                lihtml = '<li class="scrollview-equipmentsearch" data-chargetoname="' + item.Name + '" data-id="' + item.EquipmentId + '"; data-clientlookupid="' + item.ClientLookupId + '">';
                lihtml = lihtml + "" + item.ClientLookupId + ' (' + item.Name + ')</li>';
                AssetListView.add(null, mobiscroll.$(lihtml));
            }
            if ((AssetListlength + PageLength) < data.recordsTotal) {
                $('#btnAssetLoadMore').show();
            }
            else {
                $('#btnAssetLoadMore').hide();
            }
        },
        complete: function () {
            AssetListView.hideLoading();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', '#imgChargeToTree', function (e) {
    $(this).blur();
    generateEquipmentTree($(this).data('assignedid'));
});

function generateEquipmentTree(assignedid) {
    $.ajax({
        url: '/PlantLocationTree/SanitationEquipmentHierarchyTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        cache: false,
        beforeSend: function () {
            ShowLoader();
            $(document).find(".cntTree").html("<b>Processing...</b>");
        },
        success: function (data) {
            $(document).find(".cntTree").html(data);
        },
        complete: function () {
            CloseLoader();
            treeTable($(document).find('#tblTree'));
            $(document).find('.radSelectSanitation').each(function () {
                var c_id = $(this).data('clientlookupid');
                console.log($(document).find('#' + assignedid).val());
                if (c_id != null && c_id.toString().trim() == $(document).find('#' + assignedid).val().trim()) {
                    $(this).attr('checked', true);
                }
                else if (c_id != null && c_id.toString().trim() == $(document).find('#' + assignedid).val().split('(')[0]) {
                    $(this).attr('checked', true);
                }
            });
            $.each($(document).find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,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"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element
            $('#commonSanitationTreeModal').addClass('slide-active');

            $('#tblTree tr td').removeAttr('style');// code to remove the style applied from treetable.js -- white-space: nowrap;
        },
        error: function (xhr) {
            alert('error');
        }
    });
}

$(document).on('change', '.radSelectSanitation', function () {
    equipmentid = $(this).data('equipmentid');
    var clientlookupid = $(this).data('clientlookupid');
    var chargetoname = $(this).data('itemname');
    chargetoname = clientlookupid + '(' + (chargetoname.substring(0, chargetoname.length - 1)).trim() + ')';
    $(document).find("#TchargeType").val("Equipment");
    $(document).find("#TplantLocationId").val(equipmentid);

    $(document).find('#DemandModel_PlantLocationDescription').val(chargetoname).removeClass('input-validation-error').trigger('change');
    $(document).find('#TplantLocationDescription').val(clientlookupid);
    $(document).find('#JobDetailsModel_PlantLocationDescription').val(chargetoname);
    $(document).find('#ODescribeModel_PlantLocationDescription').val(chargetoname).removeClass('input-validation-error').trigger('change');
    if ($(document).find('#JobDetailsModel_PlantLocationDescription').length > 0) {
        var ChargeTo_Name = ($(this).data('itemname').substring(0, $(this).data('itemname').length - 1)).trim();
        $(document).find('#JobDetailsModel_ChargeTo_Name').val(ChargeTo_Name);
    }
    $('#commonSanitationTreeModal').removeClass('slide-active');
    $(document).find('#txtChargeToId').val(clientlookupid).trigger('mbsc-enhance');
    $(document).find('#txtChargeTo').val(chargetoname).trigger('mbsc-enhance');
});
$(document).on('click', "#commonSanitationTreeModalHide", function () {
    $('#commonSanitationTreeModal').removeClass('slide-active');
});