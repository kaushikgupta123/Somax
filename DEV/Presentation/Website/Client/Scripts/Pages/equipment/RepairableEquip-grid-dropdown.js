var RepequipTable;
var EqClientLookupId = "";
var EqName = "";
var EqModel = "";
var EqType = "";
var EqMake = "";
var TextField = "";
var ValueField = "";
$(document).on('click', '.OpenRepSpareModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    var Assigned = $(document).find("#hdnIsAssigned").val();
    generateRepSpareAssetTable(Assigned);
});
$(document).on('click', '.ClearRepSpareModalPopupGrid', function () {
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
function generateRepSpareAssetTable(IsAssigned) {
    if ($(document).find('#RepEquipPopupTable').hasClass('dataTable')) {
        RepequipTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    RepequipTable = $("#RepEquipPopupTable").DataTable({
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
            "url": "/Base/GetRepSpareAssetLookupListchunksearch",
            data: function (d) {
                d.ClientLookupId = EqClientLookupId;
                d.Name = EqName;
                d.Make = EqMake;
                d.Model = EqModel;
                d.Type = EqType;
                d.IsAssigned = IsAssigned;
                
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
                        return '<a class=link_repeqp_detail href="javascript:void(0)">' + data + '</a>'
                    }
                },
                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Make", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Model", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>'
                    }
                },
                
            ],
        "rowCallback": function (row, data, index, full) {
            var colType = this.api().column(4).index('visible');
            if (data.Type) {
                var color = "#" + intToARGB(hashCode(LRTrim(data.Type)));
                $('td', row).eq(colType).find('.m-badge--custom').css('background-color', color).css('color', '#fff');
            }
        },
        initComplete: function () {
            $(document).find('#tbleqpfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#RepSpareModal').hasClass('show')) {
                $(document).find('#RepSpareModal').modal("show");
            }

            $('#RepEquipPopupTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#RepEquipPopupTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (EqClientLookupId) { $('#colindex_0').val(EqClientLookupId); }
                if (EqName) { $('#colindex_1').val(EqName); }
                if (EqMake) { $('#colindex_2').val(EqMake); }
                if (EqModel) { $('#colindex_3').val(EqModel); }
                if (EqType) { $('#colindex_4').val(EqType); }
               

            });

            $('#RepEquipPopupTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    //var colIdx = thisId.split('_')[1];
                    //var searchText = LRTrim($(this).val());
                    EqClientLookupId = $('#colindex_0').val();
                    EqName = $('#colindex_1').val();
                    EqMake = $('#colindex_2').val();
                    EqModel = $('#colindex_3').val();
                    EqType = $('#colindex_4').val();
                    RepequipTable.page('first').draw('page');
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
$(document).on('click', '.link_repeqp_detail', function (e) {
    var row = $(this).parents('tr');
    var data = RepequipTable.row(row).data();
    $(document).find('#' + TextField).val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('#' + ValueField).val(data.EquipmentId).removeClass('input-validation-error');
    $(document).find("#RepSpareModal").modal('hide');
    $(document).find('#' + ValueField).parent().find('div > button.ClearRepSpareModalPopupGrid').css('display', 'block');

    if ($(document).find('#' + TextField).css('display') == 'none') {
        $(document).find('#' + TextField).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).css('display') == 'block') {
        $(document).find('#' + ValueField).css('display', 'none');
    }
});
$(document).on('hidden.bs.modal', '#RepSpareModal', function () {
    TextField = "";
    ValueField = "";
});