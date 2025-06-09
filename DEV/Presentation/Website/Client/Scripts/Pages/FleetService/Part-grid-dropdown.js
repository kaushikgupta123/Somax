var dtXrefPartsTable;
var PrtClientLookupId = "";
var PrtDescription = "";
var PrtUPCcode = "";
var PrtManufacturer = "";
var PrtManufacturerId = "";
var PrtStockType = "";
$(document).on('click', '#openpartgrid', function () {
    generatePartsXrefDataTable();
});
function generatePartsXrefDataTable() {
    var EquipmentId = $('#partsSessionData_EquipmentId').val();
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
            "url": "/Base/GetPartLookupList",
            data: function (d) {
                d.ClientLookupId = PrtClientLookupId;
                d.Description = PrtDescription;
                d.UPCcode = PrtUPCcode;
                d.Manufacturer = PrtManufacturer;
                d.ManufacturerId = PrtManufacturerId;
                d.StockType = PrtStockType;
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
                    return '<a class=link_xrefpart_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-300'>" + data + "</div>";
                }
            },
            {
                "data": "UPCcode", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<span class="m-badge--custom">' + data + '</span>';
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
            if (!$(document).find('#partchkoutModal').hasClass('show')) {
                $(document).find('#partchkoutModal').modal("show");
            }
            $(document).find('#XrefPartsTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#XrefPartsTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="eqp_part_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (PrtClientLookupId) { $('#eqp_part_colindex_0').val(PrtClientLookupId); }
                if (PrtDescription) { $('#eqp_part_colindex_1').val(PrtDescription); }
                if (PrtUPCcode) { $('#eqp_part_colindex_2').val(PrtUPCcode); }
                if (PrtManufacturer) { $('#eqp_part_colindex_3').val(PrtManufacturer); }
                if (PrtManufacturerId) { $('#eqp_part_colindex_4').val(PrtManufacturerId); }
                if (PrtStockType) { $('#eqp_part_colindex_5').val(PrtStockType); }

            });

            $('#XrefPartsTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    PrtClientLookupId = $('#eqp_part_colindex_0').val();
                    PrtDescription = $('#eqp_part_colindex_1').val();
                    PrtUPCcode = $('#eqp_part_colindex_2').val();
                    PrtManufacturer = $('#eqp_part_colindex_3').val();
                    PrtManufacturerId = $('#eqp_part_colindex_4').val();
                    PrtStockType = $('#eqp_part_colindex_5').val();
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
    //var index_row = $('#XrefPartsTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    //var td = $(this).parents('tr').find('td');
    var data = dtXrefPartsTable.row(row).data();
    $(document).find('#IssuePartModel_PartClientLookupId').val(data.ClientLookUpId).removeClass('input-validation-error');
    $(document).find('#IssuePartModel_PartClientLookupId').val(data.ClientLookUpId);
    $(document).find('#IssuePartModel_PartId').val(data.PartId);
    $(document).find('#IssuePartModel_Description').val(data.Description);
    $(document).find('#IssuePartModel_UPCcode').val(data.UPCcode);
    $(document).find("#partchkoutModal").modal('hide');
});