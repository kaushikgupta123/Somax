var equipTable;
var eqClientLookupId = "";
var NameEq = "";
var Model = "";
var Type = "";
var SerialNumber = "";
var AssetGroup1ClientLookupId = "";
var AssetGroup2ClientLookupId = "";
var AssetGroup3ClientLookupId = "";

function generateEquipmentDataTableWO() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#EquipmentWOTable').hasClass('dataTable')) {
        equipTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    equipTable = $("#EquipmentWOTable").DataTable({
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

                { "data": "AssetGroup1ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
                { "data": "AssetGroup2ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true},
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
            $(document).find('#tblEquipmentWOfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#EquipmentWOModal').hasClass('show')) {
                $(document).find('#EquipmentWOModal').modal("show");
            }
            $('#EquipmentWOTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#EquipmentWOTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="woequipment tfootsearchtxt" id="woequipmentcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (eqClientLookupId) { $('#woequipmentcolindex_0').val(eqClientLookupId); }
                if (NameEq) { $('#woequipmentcolindex_1').val(NameEq); }
                if (Model) { $('#woequipmentcolindex_2').val(Model); }
                if (Type) { $('#woequipmentcolindex_3').val(Type); }
                if (AssetGroup1ClientLookupId) { $('#woequipmentcolindex_4').val(AssetGroup1ClientLookupId); }
                if (AssetGroup2ClientLookupId) { $('#woequipmentcolindex_5').val(AssetGroup2ClientLookupId); }
                if (AssetGroup3ClientLookupId) { $('#woequipmentcolindex_6').val(AssetGroup3ClientLookupId); }
                if (SerialNumber) { $('#woequipmentcolindex_7').val(SerialNumber); }
            });

            $('#EquipmentWOTable tfoot th').find('.woequipment').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    eqClientLookupId = $('#woequipmentcolindex_0').val();
                    NameEq = $('#woequipmentcolindex_1').val();
                    Model = $('#woequipmentcolindex_2').val();
                    Type = $('#woequipmentcolindex_3').val();
                    AssetGroup1ClientLookupId = $('#woequipmentcolindex_4').val();
                    AssetGroup2ClientLookupId = $('#woequipmentcolindex_5').val();
                    AssetGroup3ClientLookupId = $('#woequipmentcolindex_6').val();
                    SerialNumber = $('#woequipmentcolindex_7').val();
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
    var index_row = $('#EquipmentWOTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = equipTable.row(row).data();
    $(document).find('#txtChargeTo').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('#hdnChargeTo').val(data.EquipmentId);
    $('#workOrderModel_ChargeTo_Name').val(data.Name);
    $(document).find("#EquipmentWOModal").modal('hide');
});