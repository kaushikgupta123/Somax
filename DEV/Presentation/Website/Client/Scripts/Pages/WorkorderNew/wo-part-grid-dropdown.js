var dtWoXrefPartsTable;
var PrtClientLookupId = "";
var PrtDescription = "";
var PrtUPCcode = "";
var PrtManufacturer = "";
var PrtManufacturerId = "";
var PrtStockType = "";
function generateWoPartsXrefDataTable() {
    //Clear Text box
    PrtClientLookupId = "";
    PrtDescription = "";
    PrtUPCcode = "";
    PrtManufacturer = "";
    PrtManufacturerId = "";
    PrtStockType = "";
    //Clear Text box
    var EquipmentId = $('#partsSessionData_EquipmentId').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#WoXrefPartsTable').hasClass('dataTable')) {
        dtWoXrefPartsTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtWoXrefPartsTable = $("#WoXrefPartsTable").DataTable({
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
                d.UPCcode = PrtUPCcode;
                d.Manufacturer = PrtManufacturer;
                d.ManufacturerId = PrtManufacturerId;
                d.StockType = PrtStockType;
                d.StoreroomId = ($('#UseMultiStoreroom').val() == 'True' ? $('#StoreroomId').val() : "")
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
                        return '<a class=link_xrefwopart_detail href="javascript:void(0)">' + data + '</a>'
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
                }
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
            $('#WoXrefPartsTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#WoXrefPartsTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (PrtClientLookupId) { $('#colindex_0').val(PrtClientLookupId); }
                if (PrtDescription) { $('#colindex_1').val(PrtDescription); }
                if (PrtUPCcode) { $('#colindex_2').val(PrtUPCcode); }
                if (PrtManufacturer) { $('#colindex_3').val(PrtManufacturer); }
                if (PrtManufacturerId) { $('#colindex_4').val(PrtManufacturerId); }
                if (PrtStockType) { $('#colindex_5').val(PrtStockType); }

            });
            $('#WoXrefPartsTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    PrtClientLookupId = $('#colindex_0').val();
                    PrtDescription = $('#colindex_1').val();
                    PrtUPCcode = $('#colindex_2').val();
                    PrtManufacturer = $('#colindex_3').val();
                    PrtManufacturerId = $('#colindex_4').val();
                    PrtStockType = $('#colindex_5').val();
                    dtWoXrefPartsTable.page('first').draw('page');
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
$(document).on('click', '.link_xrefwopart_detail', function () {

    var index_row = $('#WoXrefPartsTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtWoXrefPartsTable.row(row).data();
    $(document).find('#txtpartid').val(data.ClientLookUpId).removeClass('input-validation-error');
    if ($(document).find('#PartIssueAddModel_IsPartIssue').val()) { //V2-615
        var WOid = $(document).find('#workOrderModel_WorkOrderId').val();
        var WOLookupid = $(document).find('#workOrderModel_ClientLookupId').val();
        $(document).find('#PartIssueAddModel_PartId').val(data.PartId);
        $(document).find('#PartIssueAddModel_WorkOrderId').val(WOid);
        $(document).find('#PartIssueAddModel_WorkOrderClientLookupId').val(WOLookupid);
    }

    $(document).find('#partModal').modal("hide");
});
