var equipTable;
var eqClientLookupId = "";
var eqName = "";
var eqModel = "";
var eqType = "";
var eqSerialNumber = "";


function generateEquipmentDataTable() {
    var rCount = 0;    
    if ($(document).find('#EquipmentPMTable').hasClass('dataTable')) {
        equipTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    equipTable = $("#EquipmentPMTable").DataTable({
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        "pageLength": 10,
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Base/GetEquipmentLookupListchunksearch",
            data: function (d) {
                d.ClientLookupId = eqClientLookupId;
                d.Name = eqName;
                d.Model = eqModel;
                d.Type = eqType;
                d.SerialNumber = eqSerialNumber;
                d.InactiveFlag = false;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                return json.data;
            }
        },
        "columns":
        [
            {
                "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<a class=link_woeqp_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "Model", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<span class="m-badge--custom">' + data + '</span>'
                }
            },
            { "data": "SerialNumber", "autoWidth": true, "bSearchable": true, "bSortable": true },
        ],
        "rowCallback": function (row, data, index, full) {
            var colType = this.api().column(3).index('visible');
            if (data.Type) {
                var color = "#" + intToARGB(hashCode(LRTrim(data.Type)));
                $('td', row).eq(colType).find('.m-badge--custom').css('background-color', color).css('color', '#fff');
            }
        },
        initComplete: function () {
            $(document).find('#tblEquipmentWOfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#EquipmentPMModal').hasClass('show')) {
                $(document).find('#EquipmentPMModal').modal("show");
            }
            $('#EquipmentPMTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#EquipmentPMTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="woequipment tfootsearchtxt" id="woequipmentcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (eqClientLookupId) { $('#woequipmentcolindex_0').val(eqClientLookupId); }
                if (eqName) { $('#woequipmentcolindex_1').val(eqName); }
                if (eqModel) { $('#woequipmentcolindex_2').val(eqModel); }
                if (eqType) { $('#woequipmentcolindex_3').val(eqType); }
                if (eqSerialNumber) { $('#woequipmentcolindex_4').val(eqSerialNumber); }
            });

            $('#EquipmentPMTable tfoot th').find('.woequipment').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    eqClientLookupId = $('#woequipmentcolindex_0').val();
                    eqName = $('#woequipmentcolindex_1').val();
                    eqModel = $('#woequipmentcolindex_2').val();
                    eqType = $('#woequipmentcolindex_3').val();
                    eqSerialNumber = $('#woequipmentcolindex_4').val();
                    equipTable.page('first').draw('page');
                }
            });
        }
    });
}

$(document).ready(function () {
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
});
$(document).on('click', '.link_woeqp_detail', function (e) {
    //if ($(document).find('#scheduleRecords_InactiveFlag').length > 0)
    //{
    //    $(document).find('#scheduleRecords_InactiveFlag').prop('readonly', false);
    //    $(document).on('click', '#scheduleRecords_InactiveFlag', function () { return true});
    //}
    var index_row = $('#EquipmentPMTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = equipTable.row(row).data();
    $(document).find('#txtChargeTo').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('#hdnChargeTo').val(data.EquipmentId);
    $(document).find('#scheduleRecords_ChargeToName').val(data.Name);
    $(document).find("#EquipmentPMModal").modal('hide');
});

//#region V2-950
var Model = "";
var NameEq = "";
var Type = "";
var SerialNumber = "";
var AssetGroup1ClientLookupId = "";
var AssetGroup2ClientLookupId = "";
var AssetGroup3ClientLookupId = "";
var TextField = "";
var ValueField = "";
var Equipid = "";
$(document).on('click', '.OpenAssetModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    Equipid = $(this).data('equipmentid');
    generateAssetDataTable();
});

function generateAssetDataTable() {
    var rCount = 0;    
    if ($(document).find('#tblAssetLookup').hasClass('dataTable')) {
        equipTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    equipTable = $("#tblAssetLookup").DataTable({
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        "pageLength": 10,
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Base/GetEquipmentLookupListchunksearch",
            data: function (d) {
                d.ClientLookupId = eqClientLookupId;
                d.Name = NameEq;
                d.Model = Model;
                d.Type = Type;
                d.AssetGroup1ClientLookupId = AssetGroup1ClientLookupId;
                d.AssetGroup2ClientLookupId = AssetGroup2ClientLookupId;
                d.AssetGroup3ClientLookupId = AssetGroup3ClientLookupId;
                d.SerialNumber = SerialNumber;
                d.InactiveFlag = false;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                return json.data;
            }
        },
        "columns":
            [
                {
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=link_pmseqp_detail href="javascript:void(0)">' + data + '</a>'
                    }
                },
                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Model", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>'
                    }
                },

                { "data": "AssetGroup1ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
                { "data": "AssetGroup2ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
                { "data": "AssetGroup3ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
                { "data": "SerialNumber", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        "rowCallback": function (row, data, index, full) {
            var colType = this.api().column(3).index('visible');
            if (data.Type) {
                var color = "#" + intToARGB(hashCode(LRTrim(data.Type)));
                $('td', row).eq(colType).find('.m-badge--custom').css('background-color', color).css('color', '#fff');
            }
        },
        initComplete: function () {
            $(document).find('#tblAssetLookupfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#AssetLookupModal').hasClass('show')) {
                $(document).find('#AssetLookupModal').modal("show");
            }
            $('#tblAssetLookup tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#tblAssetLookup thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="woequipment tfootsearchtxt" id="equipmentcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (eqClientLookupId) { $('#equipmentcolindex_0').val(eqClientLookupId); }
                if (NameEq) { $('#equipmentcolindex_1').val(NameEq); }
                if (Model) { $('#equipmentcolindex_2').val(Model); }
                if (Type) { $('#equipmentcolindex_3').val(Type); }
                if (AssetGroup1ClientLookupId) { $('#equipmentcolindex_4').val(AssetGroup1ClientLookupId); }
                if (AssetGroup2ClientLookupId) { $('#equipmentcolindex_5').val(AssetGroup2ClientLookupId); }
                if (AssetGroup3ClientLookupId) { $('#equipmentcolindex_6').val(AssetGroup3ClientLookupId); }
                if (SerialNumber) { $('#equipmentcolindex_7').val(SerialNumber); }
            });

            $('#tblAssetLookup tfoot th').find('.woequipment').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    eqClientLookupId = $('#equipmentcolindex_0').val();
                    NameEq = $('#equipmentcolindex_1').val();
                    Model = $('#equipmentcolindex_2').val();
                    Type = $('#equipmentcolindex_3').val();
                    AssetGroup1ClientLookupId = $('#equipmentcolindex_4').val();
                    AssetGroup2ClientLookupId = $('#equipmentcolindex_5').val();
                    AssetGroup3ClientLookupId = $('#equipmentcolindex_6').val();
                    SerialNumber = $('#equipmentcolindex_7').val();
                    equipTable.page('first').draw('page');
                }
            });
        }
    });
}
$(document).on('click', '.link_pmseqp_detail', function (e) {
    var index_row = $('#tblAssetLookup tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = equipTable.row(row).data();
    $(document).find('#' + TextField).val(data.ClientLookupId).css("display", "block");
    $(document).find('#' + ValueField).val(data.EquipmentId).removeClass('input-validation-error').css("display", "none");
    $(document).find('#' + ValueField).parent().find('div > button.ClearAssetModalPopupGridData').css('display', 'block');
    $(document).find("#AssetLookupModal").modal('hide');
    $('#ChargeToName').val(data.Name);
});
$(document).on('hidden.bs.modal', '#AssetLookupModal', function () {
    TextField = "";
    ValueField = "";
});
$(document).on('click', '.ClearAssetModalPopupGridData', function () {
    $(document).find('#' + $(this).data('textfield')).val('').css("display", "none");
    $(document).find('#' + $(this).data('valuefield')).val('').css("display", "block");
    $(this).css('display', 'none');
    $('#ChargeToName').val('');
});
//#region Equipment Hierarchy ModalTree 
var TextFieldId_ChargeTo = "";
var HdnfieldId_ChargeTo = "";
$(document).on('click', '#imgChargeToTreeLineItemDynamic', function (e) {
    TextFieldId_ChargeTo = $(this).data('textfield');
    HdnfieldId_ChargeTo = $(this).data('valuefield');
    $(this).blur();
    generateWoEquipmentTreeDynamic(-1);
});
function generateWoEquipmentTreeDynamic(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/WorkOrderEquipmentHierarchyTreeDynamic',
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
        },
        complete: function () {
            CloseLoader();
            $('#woEquipTreeModal').modal('show');
            treeTable($(document).find('#tblTree'));
            $(document).find('.radSelectWoDynamic').each(function () {
                if ($(document).find('#' + HdnfieldId_ChargeTo).val() == '0' || $(document).find('#' + HdnfieldId_ChargeTo) == '') {

                    if ($(this).data('equipmentid') === equipmentid) {
                        $(this).attr('checked', true);
                    }

                }
                else {

                    if ($(this).data('equipmentid') == $(document).find('#' + HdnfieldId_ChargeTo).val()) {
                        $(this).attr('checked', true);
                    }

                }

            });
            //-- V2-518 collapse all element
            // looking for the collapse icon and triggered click to collapse
            $.each($(document).find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,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"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element
        },
        error: function (xhr) {
            alert('error');
        }
    });
}


$(document).on('change', ".radSelectWoDynamic", function () {

    var s = $(this).data;
    $(document).find('#' + HdnfieldId_ChargeTo).val('0');
    equipmentid = $(this).data('equipmentid');
    var clientlookupid = $(this).data('clientlookupid');
    var chargetoname = $(this).data('itemname');
    chargetoname = chargetoname.substring(0, chargetoname.length - 1);
    $.ajax({
        url: '/PlantLocationTree/MapEquipmentHierarchyTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: false,
        cache: false,
        data: { _EquipmentId: equipmentid },
        success: function (data) {
            $('#commonWOTreeModal').modal('hide');

            $(document).find('#' + TextFieldId_ChargeTo).val(clientlookupid).css("display", "block");
            $(document).find('#' + HdnfieldId_ChargeTo).val(equipmentid).removeClass('input-validation-error').css("display", "none").trigger('change');
            $(document).find('#' + HdnfieldId_ChargeTo).parent().find('div > button.ClearAssetModalPopupGridData').css('display', 'block');
            $('#woEquipTreeModal').modal('hide');
            $('#ChargeToName').val(chargetoname);
        }
    });

});
$(document).on('hidden.bs.modal', '#woEquipTreeModal', function () {
    TextFieldId_ChargeTo = "";
    HdnfieldId_ChargeTo = "";
});
//#endregion
//#endregion