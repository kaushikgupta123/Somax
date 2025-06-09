
var dtXrefPartsTable;
var PrtClientLookupId = "";
var PrtDescription = "";
var PrtUPCcode = "";
var PrtManufacturer = "";
var PrtManufacturerId = "";
var PrtStockType = "";
var TextField = "";
var ValueField = "";
var Part_Storeroomid = "";
$(document).on('click', '.OpenPartModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    var MultiStoreroom = $(this).data('multistoreroomvalue');
    if (MultiStoreroom == "True") {
        Part_Storeroomid = $(this).data('storeroomid');
        if (Part_Storeroomid == "") {
            return false;
        }
    }
    generatePartsXrefDataTable();
});
$(document).on('click', '.ClearPartModalPopupGridData', function () {
    $(document).find('#' + $(this).data('textfield')).val('');
    $(document).find('#' + $(this).data('valuefield')).val('');

    if ($(document).find('#' + $(this).data('textfield')).css('display') == 'block') {
        $(document).find('#' + $(this).data('textfield')).css('display', 'none');
    }
    if ($(document).find('#' + $(this).data('valuefield')).css('display') == 'none') {
        $(document).find('#' + $(this).data('valuefield')).css('display', 'block');
    }
    $(this).css('display', 'none');
});
function generatePartsXrefDataTable() {
    var params = {
        ClientLookupId: PrtClientLookupId,
        Description: PrtDescription,
        UPCcode: PrtUPCcode,
        Manufacturer: PrtManufacturer,
        ManufacturerId: PrtManufacturerId,
        StockType: PrtStockType,
        Storeroomid: Part_Storeroomid
    };
    var url = "/Base/GetPartLookupListchunksearch";
    if ($(document).find('#AddPRLineItemPartInInventory_PartStoreroomId').length > 0
        || $(document).find('#EditPRLineItemPartInInventorySingleStock_PartStoreroomId').length > 0
        || $(document).find('#AddPOLineItemPartInInventory_PartStoreroomId').length > 0
        || $(document).find('#EditPOLineItemPartInInventorySingleStock_PartStoreroomId').length > 0) {
        var newParamKey = "VendorId";
        var newParamValue = $(document).find('#PartInInventoryVendorId').val();
        params[newParamKey] = newParamValue;
        url = "/Base/GetPartLookupListchunksearchForSingleStockLineItem";
    }
    var rCount = 0;
    var visibility;
    if ($(document).find('#XrefPartsTable').hasClass('dataTable')) {
        dtXrefPartsTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtXrefPartsTable = $("#XrefPartsTable").DataTable({
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
            "url": url,
            data: function (d) {
                $.extend(d, params);
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
                        return '<a class=link_xrefpart_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-400'>" + data + "</div>";
                    }
                },
                {
                    "data": "UPCcode", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>'
                    }
                },
                {
                    "data": "Manufacturer", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--dot-custom">' +
                            '</span > &nbsp; <span class="m--font-custom" >' + data + '</span>';
                    }
                },
                { "data": "ManufacturerID", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "StockType", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>'
                    }
                },
            ],
        "rowCallback": function (row, data, index, full) {
            var colManufacturer = this.api().column(2).index('visible');
            if (data.Manufacturer) {
                var color = "#" + intToARGB(hashCode(LRTrim(data.Manufacturer)));
                $('td', row).eq(colManufacturer).find('.m-badge--dot-custom').css('background-color', color).css('color', '#fff');
                $('td', row).eq(colManufacturer).find('.m--font-custom').css('color', color);
            }
            var colStockType = this.api().column(4).index('visible');
            if (data.StockType) {
                var color = "#" + intToARGB(hashCode(LRTrim(data.StockType)));
                $('td', row).eq(colStockType).find('.m-badge--custom').css('background-color', color).css('color', '#fff');
            }
        },
        initComplete: function () {
            $(document).find('#tblpartfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#partModal').hasClass('show')) {
                $(document).find('#partModal').modal("show");
            }
            $(document).find('#XrefPartsTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#XrefPartsTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="eqp_part_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (params.ClientLookupId) { $('#eqp_part_colindex_0').val(params.ClientLookupId); }
                if (params.Description) { $('#eqp_part_colindex_1').val(params.Description); }
                if (params.UPCcode) { $('#eqp_part_colindex_2').val(params.UPCcode); }
                if (params.Manufacturer) { $('#eqp_part_colindex_3').val(params.Manufacturer); }
                if (params.ManufacturerId) { $('#eqp_part_colindex_4').val(params.ManufacturerId); }
                if (params.StockType) { $('#eqp_part_colindex_5').val(params.StockType); }
            });
            $('#XrefPartsTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    params.ClientLookupId = $('#eqp_part_colindex_0').val();
                    params.Description = $('#eqp_part_colindex_1').val();
                    params.UPCcode = $('#eqp_part_colindex_2').val();
                    params.Manufacturer = $('#eqp_part_colindex_3').val();
                    params.ManufacturerId = $('#eqp_part_colindex_4').val();
                    params.StockType = $('#eqp_part_colindex_5').val();
                    dtXrefPartsTable.page('first').draw('page');
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
$(document).on('click', '.link_xrefpart_detail', function (e) {
    var index_row = $('#XrefPartsTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtXrefPartsTable.row(row).data();
    $(document).find('#' + TextField).val(data.ClientLookUpId).removeClass('input-validation-error');
    $(document).find('#' + ValueField).val(data.PartId).removeClass('input-validation-error');
    $(document).find('#' + TextField).text(data.Description)
    /*// RKL-MAIL-Label Printing from Receipts*/
    if ($(document).find('#AddPRLineItemPartInInventory_PartStoreroomId').length > 0) {
        $(document).find('#AddPRLineItemPartInInventory_PartStoreroomId').val(data.PartStoreroomId);
    }
    if ($(document).find('#EditPRLineItemPartInInventorySingleStock_PartStoreroomId').length > 0) {
        $(document).find('#EditPRLineItemPartInInventorySingleStock_PartStoreroomId').val(data.PartStoreroomId);
    }
    if ($(document).find('#AddPOLineItemPartInInventory_PartStoreroomId').length > 0) {
        $(document).find('#AddPOLineItemPartInInventory_PartStoreroomId').val(data.PartStoreroomId);
    }
    if ($(document).find('#EditPOLineItemPartInInventorySingleStock_PartStoreroomId').length > 0) {
        $(document).find('#EditPOLineItemPartInInventorySingleStock_PartStoreroomId').val(data.PartStoreroomId);
    }
    $(document).find("#partModal").modal('hide');
    $(document).find('#' + ValueField).parent().find('div > button.ClearPartModalPopupGridData').css('display', 'block');

    if ($(document).find('#' + TextField).css('display') == 'none') {
        $(document).find('#' + TextField).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).css('display') == 'block') {
        $(document).find('#' + ValueField).css('display', 'none');
    }
});