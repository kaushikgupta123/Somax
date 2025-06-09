var dtPersonnelTable;
var PersonnelClientLookupId = "";
var PersonnelNameFirst = "";
var PersonnelNameLast = "";

$(document).on('click', '#personnelopengrid', function () {
    PersonnelClientLookupId = "";
    PersonnelNameFirst = "";
    PersonnelNameLast = "";
    getPersonnelDataTable();
});
function getPersonnelDataTable() {
    if ($(document).find('#InvoicePersonnalTable').hasClass('dataTable')) {
        dtPersonnelTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtPersonnelTable = $("#InvoicePersonnalTable").DataTable({
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
            "url": "/Base/GetPersonnelLookupListGridData",
            data: function (d) {
                d.PersonnelClientLookupId = PersonnelClientLookupId;
                d.PersonnelNameFirst = PersonnelNameFirst;
                d.PersonnelNameLast = PersonnelNameLast;
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
                    "data": "PClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=link_Personnel_detail href="javascript:void(0)">' + data + '</a>'
                    }
                },
                {
                    "data": "NameFirst", "autoWidth": true, "bSearchable": true, "bSortable": true,
                },
                {
                    "data": "NameLast", "autoWidth": true, "bSearchable": true, "bSortable": true,
                }
            ],
        initComplete: function () {
            $(document).find('#tblPersonnalfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#personnalpartModal').hasClass('show')) {
                $(document).find('#personnalpartModal').modal("show");
            }
            $(window).keydown(function (event) {
                if (event.keyCode === 13) {
                    event.preventDefault();
                    return false;
                }
            });

            $('#InvoicePersonnalTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#InvoicePersonnalTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="Personnel_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (PersonnelClientLookupId) { $('#Personnel_colindex_0').val(PersonnelClientLookupId); }
                if (PersonnelNameFirst) { $('#Personnel_colindex_1').val(PersonnelNameFirst); }
                if (PersonnelNameLast) { $('#Personnel_colindex_2').val(PersonnelNameLast); }
            });

            $('#InvoicePersonnalTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode === 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    PersonnelClientLookupId = $('#Personnel_colindex_0').val();
                    PersonnelNameFirst = $('#Personnel_colindex_1').val();
                    PersonnelNameLast = $('#Personnel_colindex_2').val();
                    dtPersonnelTable.page('first').draw('page');
                }
            });
        }
    });
}

$(document).on('click', '.link_Personnel_detail', function (e) {
    var index_row = $('#InvoicePersonnalTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtPersonnelTable.row(row).data();
    $(document).find('#InvoiceMatchHeaderModel_ResponsibleWithClientLookupId').val((data.PClientLookupId ? data.PClientLookupId.trim() : '') + ' ' + (data.NameFirst ? data.NameFirst.trim() : '') + ' ' + (data.NameLast ? data.NameLast.trim() : '')).removeClass('input-validation-error')
    $(document).find('#InvoiceMatchHeaderModel_Responsible_PersonnelId').val(data.PersonnelId);

    $(document).find("#personnalpartModal").modal('hide');
    $(".InvoiceClearPersonnalModalPopupGridData").show();
});

$(document).on('click', '.InvoiceClearPersonnalModalPopupGridData', function () {
    $(document).find('#InvoiceMatchHeaderModel_ResponsibleWithClientLookupId').val('');
    $(document).find('#InvoiceMatchHeaderModel_Responsible_PersonnelId').val('');
    if ($(document).find('#InvoiceMatchHeaderModel_Responsible_PersonnelId').css('display') == 'none') {
        $(document).find('#InvoiceMatchHeaderModel_Responsible_PersonnelId').css('display', 'block');
    }
    if ($(document).find('#InvoiceMatchHeaderModel_ResponsibleWithClientLookupId').css('display') == 'none') {
        $(document).find('#InvoiceMatchHeaderModel_ResponsibleWithClientLookupId').css('display', 'block');
    }
    $(this).css('display', 'none');
});


