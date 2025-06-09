var equipTable;
var eqClientLookupId = "";
var eqName = "";
var eqModel = "";
var eqType = "";
var eqSerialNumber = "";

function generateEquipmentDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#EquipmentPRTable').hasClass('dataTable')) {
        equipTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    equipTable = $("#EquipmentPRTable").DataTable({
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
                    return '<a class=link_prequipment_detail href="javascript:void(0)">' + data + '</a>'
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
            $(document).find('#tblEquipmentPRfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#EquipmentPRModal').hasClass('show')) {
                $(document).find('#EquipmentPRModal').modal("show");
            }

            $('#EquipmentPRTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#EquipmentPRTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="prequipment tfootsearchtxt" id="prequipmentcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (eqClientLookupId) { $('#prequipmentcolindex_0').val(eqClientLookupId); }
                if (eqName) { $('#prequipmentcolindex_1').val(eqName); }
                if (eqModel) { $('#prequipmentcolindex_2').val(eqModel); }
                if (eqType) { $('#prequipmentcolindex_3').val(eqType); }
                if (eqSerialNumber) { $('#prequipmentcolindex_4').val(eqSerialNumber); }
            });

            $('#EquipmentPRTable tfoot th').find('.prequipment').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    eqClientLookupId = $('#prequipmentcolindex_0').val();
                    eqName = $('#prequipmentcolindex_1').val();
                    eqModel = $('#prequipmentcolindex_2').val();
                    eqType = $('#prequipmentcolindex_3').val();
                    eqSerialNumber = $('#prequipmentcolindex_4').val();
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
//$(document).on('click', '.link_preqp_detail', function (e) {
//    var index_row = $('#EquipmentPRTable tr').index($(this).closest('tr')) - 1;
//    var row = $(this).parents('tr');
//    var td = $(this).parents('tr').find('td');
//    var data = equipTable.row(row).data();
//    $(document).find('#ChargeToClientLookupId').val(data.ClientLookupId).css("display", "block");
//    $(document).find('#ChargeToId').val(data.EquipmentId).removeClass('input-validation-error').css("display", "none");
//    //$(document).find('#ChargeTo_Name').val(data.ChargeTo);
//    $(document).find('#ChargeToId').parent().find('div > button#ClearChargeToModalPopupGridData').css('display', 'block');
//    $(document).find("#EquipmentPRModal").modal('hide');
//});
$(document).on('click', '.link_prequipment_detail', function (e) {
    var index_row = $('#EquipmentPRTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = equipTable.row(row).data();
    $(document).find('#ChargeToClientLookupId').val(data.ClientLookupId).css("display", "block");
    $(document).find('#ChargeToId').val(data.EquipmentId).removeClass('input-validation-error').css("display", "none");
    //$(document).find('#ChargeTo_Name').val(data.ChargeTo);
    $(document).find('#ChargeToId').parent().find('div > button#ClearChargeToModalPopupGridData').css('display', 'block');
    $(document).find("#EquipmentPRModal").modal('hide');
});