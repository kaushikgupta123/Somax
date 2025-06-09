var mspEquipTable;
var EqClientLookupId = "";
var EqName = "";
var EqModel = "";
var EqType = "";
var EqSerialNumber = "";
$(document).on('click', '.OpenMSPAssetModalPopupGrid', function () {
    generateMSPEquipmentTable();
});
function generateMSPEquipmentTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#MSPEquipPopupTable').hasClass('dataTable')) {
        mspEquipTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    mspEquipTable = $("#MSPEquipPopupTable").DataTable({
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
                d.ClientLookupId = EqClientLookupId;
                d.Name = EqName;
                d.Model = EqModel;
                d.Type = EqType;
                d.SerialNumber = EqSerialNumber;
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
                        return '<a class=link_mspeqp_detail href="javascript:void(0)">' + data + '</a>'
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
            $(document).find('#tblmspeqpfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#mspEquipModal').hasClass('show')) {
                $(document).find('#mspEquipModal').modal("show");
            }

            $('#MSPEquipPopupTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#MSPEquipPopupTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (EqClientLookupId) { $('#colindex_0').val(EqClientLookupId); }
                if (EqName) { $('#colindex_1').val(EqName); }
                if (EqModel) { $('#colindex_2').val(EqModel); }
                if (EqType) { $('#colindex_3').val(EqType); }
                if (EqSerialNumber) { $('#colindex_4').val(EqSerialNumber); }

            });

            $('#MSPEquipPopupTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    EqClientLookupId = $('#colindex_0').val();
                    EqName = $('#colindex_1').val();
                    EqModel = $('#colindex_2').val();
                    EqType = $('#colindex_3').val();
                    EqSerialNumber = $('#colindex_4').val();
                    mspEquipTable.page('first').draw('page');
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
$(document).on('click', '.link_mspeqp_detail', function (e) {
    var index_row = $('#MSPEquipPopupTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = mspEquipTable.row(row).data();
    $(document).find('#MSPEquipmentXrefModel_Equipment_ClientLookupId').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find("#mspEquipModal").modal('hide');
});
