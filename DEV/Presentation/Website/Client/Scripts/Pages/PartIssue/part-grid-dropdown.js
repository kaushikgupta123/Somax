var dtXrefPartsTable;
var PrtClientLookupId = "";
var PrtDescription = "";
var PrtManufacturer = "";
var PrtManufacturerId = "";
var PrtStockType = "";
var Part_Storeroomid = "";
$(document).on('click', '#openpartgrid', function () {
    var MultiStoreroom = $(this).data('multistoreroomvalue');
    if (MultiStoreroom == "True") {
        Part_Storeroomid = $(document).find('.ddlStoreroom').val();
        if (Part_Storeroomid == "") {
            return false;
        }
    }
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
            "url": "/Base/GetPartLookupListchunksearch",
            data: function (d) {
                d.ClientLookupId = PrtClientLookupId;
                d.Description = PrtDescription;
                d.Manufacturer = PrtManufacturer;
                d.ManufacturerId = PrtManufacturerId;
                d.StockType = PrtStockType;
                d.Storeroomid = Part_Storeroomid;
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
                if (PrtManufacturer) { $('#eqp_part_colindex_2').val(PrtManufacturer); }
                if (PrtManufacturerId) { $('#eqp_part_colindex_3').val(PrtManufacturerId); }
                if (PrtStockType) { $('#eqp_part_colindex_4').val(PrtStockType); }

            });

            $('#XrefPartsTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    PrtClientLookupId = $('#eqp_part_colindex_0').val();
                    PrtDescription = $('#eqp_part_colindex_1').val();
                    PrtManufacturer = $('#eqp_part_colindex_2').val();
                    PrtManufacturerId = $('#eqp_part_colindex_3').val();
                    PrtStockType = $('#eqp_part_colindex_4').val();
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
    $(document).find('#reader__dashboard_section_swaplink').css("display", "none");
});
$(document).on('click', '.link_xrefpart_detail', function (e) {
    var index_row = $('#XrefPartsTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtXrefPartsTable.row(row).data();
    if (typeof activeTab !== 'undefined' && activeTab == "#Returnpart") {
        $(document).find('#txtPartReturnId').val(data.ClientLookUpId).removeClass('input-validation-error');
        $(document).find('#txtPartReturnId').val(data.ClientLookUpId);
        $(document).find('#hdnPartReturnId').val(data.PartId);
    }
    else {
        $(document).find('#txtPartId').val(data.ClientLookUpId).removeClass('input-validation-error');
        $(document).find('#txtPartId').val(data.ClientLookUpId);
        $(document).find('#hdnPartId').val(data.PartId);
    }

    $(document).find("#partchkoutModal").modal('hide');
});


