var equipTable;
var EqClientLookupId = "";
var EqName = "";
var EqModel = "";
var EqMake = "";
var EqVIN = "";
$(document).on('click', '#opengrid', function () {
    generateFleetServiceEquipTable();
});
function generateFleetServiceEquipTable() {
    var EquipmentId = $('#partsSessionData_EquipmentId').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#EquipPopupTable').hasClass('dataTable')) {
        equipTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    equipTable = $("#EquipPopupTable").DataTable({
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
            "url": "/Base/FleetAssetLookupList",
            data: function (d) {
                d.clientLookupId = EqClientLookupId;
                d.name = EqName;
                d.model = EqModel;
                d.make = EqMake;
                d.vin = EqVIN;
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
                    return '<a class=link_Issueeqp_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Make", "autoWidth": true, "bSearchable": true, "bSortable": true
            },
            { "data": "Model", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "VIN", "autoWidth": true, "bSearchable": true, "bSortable": true },
        ],
        "rowCallback": function (row, data, index, full) {
            var colType = this.api().column(3).index('visible');
        },
        initComplete: function () {
            $(document).find('#tbleqpfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#FleetServiceModal').hasClass('show')) {
                $(document).find('#FleetServiceModal').modal("show");
            }

            $('#EquipPopupTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#EquipPopupTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (EqClientLookupId) { $('#colindex_0').val(EqClientLookupId); }
                if (EqName) { $('#colindex_1').val(EqName); }
                if (EqModel) { $('#colindex_3').val(EqModel); }
                if (EqMake) { $('#colindex_2').val(EqMake); }
                if (EqVIN) { $('#colindex_4').val(EqVIN); }

            });

            $('#EquipPopupTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    EqClientLookupId = $('#colindex_0').val();
                    EqName = $('#colindex_1').val();
                    EqModel = $('#colindex_3').val();
                    EqMake = $('#colindex_2').val();
                    EqVIN = $('#colindex_4').val();
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
$(document).on('click', '.link_Issueeqp_detail', function (e) {
    var index_row = $('#EquipPopupTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = equipTable.row(row).data();
    $(document).find('#FleetServiceModel_EquipmentClientLookupId').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find("#FleetServiceModal").modal('hide');
    $(document).find('form').find("select").removeClass("input-validation-error");
    $(document).find('form').find("input").removeClass("input-validation-error");
    $(document).find('form').find("textarea").removeClass("input-validation-error");
    $(document).find('.reset').prop("checked", false);
    CloseLoader();
});