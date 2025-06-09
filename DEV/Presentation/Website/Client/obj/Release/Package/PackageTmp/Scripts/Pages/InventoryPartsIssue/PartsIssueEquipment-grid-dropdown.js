var equipTable;
var eqClientLookupId = "";
var eqName = "";
var eqModel = "";
var eqType = "";
var eqSerialNumber = "";

function generateEquipmentDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#EquipmentCHKTable').hasClass('dataTable')) {
        equipTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    equipTable = $("#EquipmentCHKTable").DataTable({
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
                    return '<a class=link_chkeqp_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "Model", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center",
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
            $(document).find('#tblEquipmentCHKfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#EquipmentCHKModal').hasClass('show')) {
                $(document).find('#EquipmentCHKModal').modal("show");
            }
            $('#EquipmentCHKTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#EquipmentCHKTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="chkequipment tfootsearchtxt" id="chkequipmentcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (eqClientLookupId) { $('#chkequipmentcolindex_0').val(eqClientLookupId); }
                if (eqName) { $('#chkequipmentcolindex_1').val(eqName); }
                if (eqModel) { $('#chkequipmentcolindex_2').val(eqModel); }
                if (eqType) { $('#chkequipmentcolindex_3').val(eqType); }
                if (eqSerialNumber) { $('#chkequipmentcolindex_4').val(eqSerialNumber); }
            });

            $('#EquipmentCHKTable tfoot th').find('.chkequipment').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    eqClientLookupId = $('#chkequipmentcolindex_0').val();
                    eqName = $('#chkequipmentcolindex_1').val();
                    eqModel = $('#chkequipmentcolindex_2').val();
                    eqType = $('#chkequipmentcolindex_3').val();
                    eqSerialNumber = $('#chkequipmentcolindex_4').val();
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
$(document).on('click', '.link_chkeqp_detail', function (e) {
    var index_row = $('#EquipmentCHKTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = equipTable.row(row).data();
    $(document).find('#hdnId').val(data.EquipmentId);
    $(document).find('#txtChargeToId').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('#hdnChargeToId').val(data.EquipmentId);
    $(document).find("#EquipmentCHKModal").modal('hide');
});