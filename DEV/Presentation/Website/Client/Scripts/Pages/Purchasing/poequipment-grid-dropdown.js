var equipTable;
var eqClientLookupId = "";
var eqName = "";
var eqModel = "";
var eqType = "";
var eqSerialNumber = "";
var TextField = "";
var ValueField = "";
function generateEquipmentDataTable() {    
    var rCount = 0;
    var visibility;
    if ($(document).find('#EquipmentPOTable').hasClass('dataTable')) {
        equipTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    equipTable = $("#EquipmentPOTable").DataTable({
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
            "url": "/Base/GetEquipmentLookupList",
            data: function (d) {
                d.ClientLookupId = eqClientLookupId;
                d.Name = eqName;
                d.Model = eqModel;
                d.Type = eqType;
                d.SerialNumber = eqSerialNumber;
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
                    return '<a class=link_poeqp_detail href="javascript:void(0)">' + data + '</a>'
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
            $(document).find('#tblEquipmentPOfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#EquipmentPOModal').hasClass('show')) {
                $(document).find('#EquipmentPOModal').modal("show");
            }
            $('#EquipmentPOTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#EquipmentPOTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="poequipment tfootsearchtxt" id="poequipmentcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (eqClientLookupId) { $('#poequipmentcolindex_0').val(eqClientLookupId); }
                if (eqName) { $('#poequipmentcolindex_1').val(eqName); }
                if (eqModel) { $('#poequipmentcolindex_2').val(eqModel); }
                if (eqType) { $('#poequipmentcolindex_3').val(eqType); }
                if (eqSerialNumber) { $('#poequipmentcolindex_4').val(eqSerialNumber); }
            });

            $('#EquipmentPOTable tfoot th').find('.poequipment').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    eqClientLookupId = $('#poequipmentcolindex_0').val();
                    eqName = $('#poequipmentcolindex_1').val();
                    eqModel = $('#poequipmentcolindex_2').val();
                    eqType = $('#poequipmentcolindex_3').val();
                    eqSerialNumber = $('#poequipmentcolindex_4').val();
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
//$(document).on('click', '.link_poeqp_detail', function (e) {
//    var index_row = $('#EquipmentPOTable tr').index($(this).closest('tr')) - 1;
//    var row = $(this).parents('tr');
//    var td = $(this).parents('tr').find('td');
//    var data = equipTable.row(row).data();
//    $(document).find('#txtChargeToId').val(data.ClientLookupId).removeClass('input-validation-error');
//    $(document).find('#hdnChargeToId').val(data.EquipmentId);   
//    $(document).find("#EquipmentPOModal").modal('hide');
//});

$(document).on('click', '.link_poeqp_detail', function (e) {
    var index_row = $('#EquipmentPOTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = equipTable.row(row).data();
    $(document).find('#ChargeToClientLookupId').val(data.ClientLookupId).css("display", "block");
    $(document).find('#ChargeToId').val(data.EquipmentId).removeClass('input-validation-error').css("display", "none");
    $(document).find('#ChargeToId').parent().find('div > button.ClearChargeToModalPopupGridData').css('display', 'block');
    //$(document).find('#ChargeTo_Name').val(data.Name);
    if ($('#DirectBuyLineItemModalpopup').hasClass('show')) {
        $(document).find('#' + TextField).val(data.ClientLookupId).removeClass('input-validation-error');
        $(document).find('#' + ValueField).val(data.EquipmentId).removeClass('input-validation-error');
        if ($(document).find('#' + TextField).css('display') == 'none') {
            $(document).find('#' + TextField).css('display', 'block');
        }
        if ($(document).find('#' + ValueField).css('display') == 'block') {
            $(document).find('#' + ValueField).css('display', 'none');
        }
        $(document).find('#' + ValueField).parent().find('div > button.ClearEquipmentModalPopupGridData').css('display', 'block');

    }
    $(document).find("#EquipmentPOModal").modal('hide');
});
$(document).on('hidden.bs.modal', '#EquipmentPOModal', function () {
    TextField = "";
    ValueField = "";
});
