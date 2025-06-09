var dt_PartCategoryMasterTable;
var PartCategoryClientLookupId = "";
var PartCategoryDescription = "";
var TextField = "";
var ValueField = "";
/*var NameField = "";*/

$(document).on('click', '.OpenPartCategoryMasterModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    /*NameField = $(this).data('namefield');*/
    generatePartCategoryMasterDataTable("/Base/GetPartCategoryMasterLookupListchunksearch");

});
function generatePartCategoryMasterDataTable(url) {
    var rCount = 0;
    var visibility;
    if ($(document).find('#tblPartCategoryMasterModalPopup').hasClass('dataTable')) {
        dt_PartCategoryMasterTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dt_PartCategoryMasterTable = $("#tblPartCategoryMasterModalPopup").DataTable({
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
                d.ClientLookupId = PartCategoryClientLookupId;
                d.Description = PartCategoryDescription;
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
                        return '<a class=link_PartCategoryMaster_detail href="javascript:void(0)">' + data + '</a>'
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                }
            ],
        initComplete: function () {
            $(document).find('#tblPartCategoryMasterModalPopupFooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#PartCategoryMasterTableModalPopup').hasClass('show')) {
                $(document).find('#PartCategoryMasterTableModalPopup').modal("show");
            }

            $('#tblPartCategoryMasterModalPopup tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#tblPartCategoryMasterModalPopup thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="vcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (PartCategoryClientLookupId) { $('#vcolindex_0').val(PartCategoryClientLookupId); }
                if (PartCategoryDescription) { $('#vcolindex_1').val(PartCategoryDescription); }
            });

            $(window).keydown(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });
            $('#tblPartCategoryMasterModalPopup tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    PartCategoryClientLookupId = $('#vcolindex_0').val();
                    PartCategoryDescription = $('#vcolindex_1').val();
                    dt_PartCategoryMasterTable.page('first').draw('page');
                }
            });
        }
    });
}


$(document).on('click', '.link_PartCategoryMaster_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dt_PartCategoryMasterTable.row(row).data();
    $(document).find('#' + TextField).val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('#' + ValueField).val(data.PartCategoryMasterId).removeClass('input-validation-error');
   /* $(document).find('#' + NameField).val(data.Description);*/
    $(document).find("#PartCategoryMasterTableModalPopup").modal('hide');
    $(document).find('#' + ValueField).parent().find('div > button.ClearPartCategoryMasterModalPopupGridData').css('display', 'block');

    if ($(document).find('#' + TextField).css('display') == 'none') {
        $(document).find('#' + TextField).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).css('display') == 'block') {
        $(document).find('#' + ValueField).css('display', 'none');
    }
});

$(document).on('click', '.ClearPartCategoryMasterModalPopupGridData', function () {
    $(document).find('#' + $(this).data('textfield')).val('');
    $(document).find('#' + $(this).data('valuefield')).val('');
 /*   $(document).find('#' + $(this).data('namefield')).val('');*/

    if ($(document).find('#' + $(this).data('textfield')).css('display') == 'none') {
        $(document).find('#' + $(this).data('textfield')).css('display', 'block');
    }
    if ($(document).find('#' + $(this).data('valuefield')).css('display') == 'block') {
        $(document).find('#' + $(this).data('valuefield')).css('display', 'none');
    }
    $(this).css('display', 'none');
});