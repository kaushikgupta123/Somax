var assetTable;
var assetClientLookupId = "";
var assetName = "";
var assetType = "";
var deptClientLookupId = "";
var lineClientLookupId = "";
var systemClientLookupId = "";

function generateAssetPopupTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#assetGridTable').hasClass('dataTable')) {
        assetTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    assetTable = $("#assetGridTable").DataTable({
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
            "url": "/Base/GetAssetLookupList",
            data: function (d) {
                d.clientLookupId = assetClientLookupId;
                d.name = assetName;
                d.assetType = assetType;
                d.department = deptClientLookupId;
                d.line = lineClientLookupId;
                d.system = systemClientLookupId;
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
                "data": "ClientLookUpId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<a class=link_asset_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "AssetType", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center",
                 "mRender": function (data, type, row) {
                    return '<span class="m-badge--custom">' + data + '</span>'
                }
            },
            { "data": "DeptClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "LineClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "SystemClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true }
        ],
        "rowCallback": function (row, data, index, full) {
            var colType = this.api().column(2).index('visible');
            if (data.AssetType) {
                var color = "#" + intToARGB(hashCode(LRTrim(data.Type)));
                $('td', row).eq(colType).find('.m-badge--custom').css('background-color', color).css('color', '#fff');
            }
        },
        initComplete: function () {
            $(document).find('#tblAssetfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#AssetPopupModal').hasClass('show')) {
                $(document).find('#AssetPopupModal').modal("show");
            }
            $('#assetGridTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#assetGridTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="woequipment tfootsearchtxt" id="assetcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (assetClientLookupId) { $('#assetcolindex_0').val(assetClientLookupId); }
                if (assetName) { $('#assetcolindex_1').val(assetName); }
                if (assetType) { $('#assetcolindex_2').val(assetType); }
                if (deptClientLookupId) { $('#assetcolindex_3').val(deptClientLookupId); }
                if (lineClientLookupId) { $('#assetcolindex_4').val(lineClientLookupId); }
                if (systemClientLookupId) { $('#assetcolindex_5').val(systemClientLookupId); }
            });
            $('#assetGridTable tfoot th').find('.woequipment').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    assetClientLookupId = $('#assetcolindex_0').val();
                    assetName = $('#assetcolindex_1').val();
                    assetType = $('#assetcolindex_2').val();
                    deptClientLookupId = $('#assetcolindex_3').val();
                    lineClientLookupId = $('#assetcolindex_4').val();
                    systemClientLookupId = $('#assetcolindex_5').val();
                    assetTable.page('first').draw('page');
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
$(document).on('click', '.link_asset_detail', function (e) {
    var index_row = $('#assetGridTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = assetTable.row(row).data();
    var ChargeType = 'Equipment';
    $(document).find('#hdnId').val(data.EquipmentId);
    if ($(document).find('#DemandModel_PlantLocationDescription').length > 0) {
        $(document).find('#DemandModel_PlantLocationDescription').val(data.ClientLookUpId + "(" + data.Name + ")");
        $(document).find('#DemandModel_PlantLocationDescription').removeClass('input-validation-error');
    }
    if ($(document).find('#TchargeType').length > 0) {
        $(document).find('#TchargeType').val(ChargeType);
    }
    if ($(document).find('#TplantLocationId').length > 0) {
        $(document).find('#TplantLocationId').val(data.EquipmentId);
    }
    if ($(document).find('#TplantLocationDescription').length > 0) {
        $(document).find('#TplantLocationDescription').val(data.ClientLookUpId);
    }
    if ($(document).find('#JobDetailsModel_PlantLocationDescription').length > 0) {
        $(document).find('#JobDetailsModel_PlantLocationDescription').val(data.ClientLookUpId + "(" + data.Name + ")");
    }
    if ($(document).find('#ODescribeModel_PlantLocationDescription').length > 0) {
        $(document).find('#ODescribeModel_PlantLocationDescription').val(data.ClientLookUpId + "(" + data.Name + ")");
        $(document).find('#ODescribeModel_PlantLocationDescription').removeClass('input-validation-error');
    }
    if ($(document).find('#MasterSanitModel_ChargeToDescription').length > 0) {
        $(document).find('#MasterSanitModel_ChargeToDescription').val(data.ClientLookUpId);
        $(document).find('#MasterSanitModel_ChargeToDescription').removeClass('input-validation-error');
        if ($(document).find('#MasterSanitModel_ChargeToName').length > 0) {
            $(document).find('#MasterSanitModel_ChargeToName').val(data.Name);
        }
    }
    if ($(document).find('#MasterSanitModel_ChargeType').length > 0) {
        $(document).find('#MasterSanitModel_ChargeType').val(ChargeType);
    }
    if ($(document).find('#MasterSanitModel_PlantLocationId').length > 0) {
        $(document).find('#MasterSanitModel_PlantLocationId').val(data.EquipmentId);
    }
    if ($(document).find('#MasterSanitModel_ChargeToClientLookupId').length > 0) {
        $(document).find('#MasterSanitModel_ChargeToClientLookupId').val(data.ClientLookUpId);
    }
    $(document).find("#AssetPopupModal").modal('hide');   
});