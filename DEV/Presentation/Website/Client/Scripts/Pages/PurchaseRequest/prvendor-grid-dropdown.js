var dtPRVendorTable;
var VenClientLookupId = "";
var VenName = "";
var TextField = "";
var ValueField = "";
var NameField = "";

$(document).on('click', '#vopengrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    NameField = $(this).data('namefield');
    var IsPunchOut = $(document).find('#purchaseRequestModel_IsPunchOut').val();
    if (IsPunchOut == 'True')
    {
        generateVendorDataTable("/Base/GetPunchOutVendorLookupList");
    }
    else
    {
        generateVendorDataTable("/Base/GetVendorLookupListchunksearch");
    }
    
});
function generateVendorDataTable(url) {
    var rCount = 0;
    var visibility;
    if ($(document).find('#PRVendorsTable').hasClass('dataTable')) {
        dtPRVendorTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtPRVendorTable = $("#PRVendorsTable").DataTable({
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
                d.ClientLookupId = VenClientLookupId;
                d.Name = VenName;
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
                    return '<a class=link_prvendor_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            {
                "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true,
            }
        ],
        initComplete: function () {
            $(document).find('#tblvendorfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#partModal').hasClass('show')) {
                $(document).find('#partModal').modal("show");
            }

            $('#PRVendorsTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#PRVendorsTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="vcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (VenClientLookupId) { $('#vcolindex_0').val(VenClientLookupId); }
                if (VenName) { $('#vcolindex_1').val(VenName); }
            });

            $(window).keydown(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });
            $('#PRVendorsTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    VenClientLookupId = $('#vcolindex_0').val();
                    VenName = $('#vcolindex_1').val();
                    dtPRVendorTable.page('first').draw('page');
                }
            });
        }
    });
}

//$(document).on('click', '.link_prvendor_detail', function (e) {    
//    var index_row = $('#PRVendorsTable tr').index($(this).closest('tr')) - 1;
//    var row = $(this).parents('tr');
//    var td = $(this).parents('tr').find('td');
//    var data = dtPRVendorTable.row(row).data();
//    $(document).find('#purchaseRequestModel_VendorClientLookupId').val(data.ClientLookupId).removeClass('input-validation-error');   
//    $(document).find('#purchaseRequestModel_VendorName').val(data.Name);
//    $(document).find('#purchaseRequestModel_VendorId').val(data.VendorID);
//    $(document).find("#partModal").modal('hide');
//});

$(document).on('click', '.link_prvendor_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtPRVendorTable.row(row).data();
    $(document).find('#' + TextField).val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('#' + ValueField).val(data.VendorID).removeClass('input-validation-error');
    $(document).find('#' + NameField).val(data.Name);
    $(document).find("#partModal").modal('hide');
    $(document).find('#' + ValueField).parent().find('div > button.ClearVendorModalPopupGridData').css('display', 'block');

    if ($(document).find('#' + TextField).css('display') == 'none') {
        $(document).find('#' + TextField).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).css('display') == 'block') {
        $(document).find('#' + ValueField).css('display', 'none');
    }
});

$(document).on('click', '.ClearVendorModalPopupGridData', function () {
    $(document).find('#' + $(this).data('textfield')).val('');
    $(document).find('#' + $(this).data('valuefield')).val('');
    $(document).find('#' + $(this).data('namefield')).val('');

    if ($(document).find('#' + $(this).data('textfield')).css('display') == 'block') {
        $(document).find('#' + $(this).data('textfield')).css('display', 'none');
    }
    if ($(document).find('#' + $(this).data('valuefield')).css('display') == 'none') {
        $(document).find('#' + $(this).data('valuefield')).css('display', 'block');
    }
    $(this).css('display', 'none');
});