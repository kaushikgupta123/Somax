var locationTable;
var locClientLookupId = "";
var locName = "";
var locModel = "";
var locType = "";
var locSerialNumber = "";


function generateLocationDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#LocationTable').hasClass('dataTable')) {
        locationTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    locationTable = $("#LocationTable").DataTable({
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
            "url": "/Base/GetLocationLookupList",
            data: function (d) {
                d.ClientLookupId = locClientLookupId;
                d.Name = locName;
                d.Model = locModel;
                d.Type = locType;
                d.SerialNumber = locSerialNumber;
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
                    return '<a class=link_loc_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "Complex", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center",
                "mRender": function (data, type, row) {
                    return '<span class="m-badge--custom">' + data + '</span>';
                }
            }
            //,
            //{ "data": "SerialNumber", "autoWidth": true, "bSearchable": true, "bSortable": true },
        ],
        "rowCallback": function (row, data, index, full) {
            var colType = this.api().column(3).index('visible');
            if (data.Type) {
                var color = "#" + intToARGB(hashCode(LRTrim(data.Type)));
                $('td', row).eq(colType).find('.m-badge--custom').css('background-color', color).css('color', '#fff');
            }
        },
        initComplete: function () {
            $(document).find('#tblLocationfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#LocationModal').hasClass('show')) {
                $(document).find('#LocationModal').modal("show");
            }

            $('#LocationTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#LocationTable thead th').eq($(this).index()).text();

                if (colIndex !== 2 && colIndex !== 3) {
                    $(this).html('<input type="text" style="width:100%" class="chklocation tfootsearchtxt" id="chklocationcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                    if (locClientLookupId) { $('#chklocationcolindex_0').val(locClientLookupId); }
                    if (locName) { $('#chklocationcolindex_1').val(locName); }
                    if (locSerialNumber) { $('#chklocationcolindex_4').val(locSerialNumber); }

                }
            });

            $('#LocationTable tfoot th').find('.chklocation').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    locClientLookupId = $('#chklocationcolindex_0').val();
                    locName = $('#chklocationcolindex_1').val();
                    //Model = $('#chklocationcolindex_2').val();
                    //Type = $('#chklocationcolindex_3').val();
                    locSerialNumber = $('#chklocationcolindex_4').val();
                    locationTable.page('first').draw('page');
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
$(document).on('click', '.link_loc_detail', function (e) {
    var index_row = $('#LocationTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = locationTable.row(row).data();
    if (typeof activeTab !== 'undefined' && activeTab == "#Returnpart")
    {
        $(document).find('#txtReturnChargeToId').val(data.ClientLookupId).removeClass('input-validation-error');
        $(document).find('#hdnReturnChargeToId').val(data.LocationId);
    }
    else {
        $(document).find('#txtChargeToId').val(data.ClientLookupId).removeClass('input-validation-error');
        $(document).find('#hdnChargeToId').val(data.LocationId);
    }
   
    $(document).find("#LocationModal").modal('hide');
});