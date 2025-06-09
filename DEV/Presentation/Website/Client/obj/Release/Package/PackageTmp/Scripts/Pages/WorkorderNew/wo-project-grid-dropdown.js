var dtWoProjectTable;
var ProjectClientLookupId = "";
var ProjectDesc = "";
var TextField = "";
var ValueField = "";

$(document).on('click', '.OpenProjectModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    generateWoProjectDataTable();
});
$(document).on('click', '.ClearProjectModalPopupGridData', function () {
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
$(document).on('click', '#addProj', function () {
    ProjectClientLookupId = "";
    ProjectDesc = "";
    generateWoProjectDataTable();
});

function generateWoProjectDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#tblProjectModalPopup').hasClass('dataTable')) {
        dtWoProjectTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtWoProjectTable = $("#tblProjectModalPopup").DataTable({
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
            "url": "/Base/GetProjectLookupListchunksearch",
            data: function (d) {
                d.clientLookupId = ProjectClientLookupId;
                d.description = ProjectDesc;
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
                        return '<a class=link_woProject_detail href="javascript:void(0)">' + data + '</a>'
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                }
            ],
        initComplete: function () {
            $(document).find('#tblProjectModalPopupFooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#ProjectTableModalPopup').hasClass('show')) {
                $(document).find('#ProjectTableModalPopup').modal("show");
            }
            $('#tblProjectModalPopup tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#tblProjectModalPopup thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="wocolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (ProjectClientLookupId) { $('#wocolindex_0').val(ProjectClientLookupId); }
                if (ProjectDesc) { $('#wocolindex_1').val(ProjectDesc); }

            });
            $(window).keydown(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });
            $('#tblProjectModalPopup tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    ProjectClientLookupId = $('#wocolindex_0').val();
                    ProjectDesc = $('#wocolindex_1').val();
                    dtWoProjectTable.page('first').draw('page');
                }
            });
        }
    });
}

$(document).on('click', '.link_woProject_detail', function (e) {
    var workOrderID = $(document).find('#workOrderModel_WorkOrderId').val();
    var index_row = $('#tblProjectModalPopup tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtWoProjectTable.row(row).data();
    if ($(document).find('#EditWorkOrder_WorkOrderId').val()) {
        $(document).find('#' + TextField).val(data.ClientLookupId).removeClass('input-validation-error');
        $(document).find('#' + ValueField).val(data.ProjectID).removeClass('input-validation-error');
        $(document).find("#ProjectTableModalPopup").modal('hide');
        $(document).find('#' + ValueField).parent().find('div > button.ClearProjectModalPopupGridData').css('display', 'block');

        if ($(document).find('#' + TextField).css('display') == 'none') {
            $(document).find('#' + TextField).css('display', 'block');
        }
        if ($(document).find('#' + ValueField).css('display') == 'block') {
            $(document).find('#' + ValueField).css('display', 'none');
        }
    }
    else {
        $.ajax({
            url: '/WorkOrder/AddProjecttoWorkorder',
            type: "POST",
            data: { WorkOrderId: workOrderID, ProjectId: data.ProjectID, ProjectClientlookupId: data.ClientLookupId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                SuccessAlertSetting.text = getResourceValue("ProjectAddedSuccessAlert");;
                swal(SuccessAlertSetting, function () {
                    RedirectToPmDetail(workOrderID, "overview");
                });

            },
            complete: function () {
                $(document).find("#ProjectTableModalPopup").modal('hide');
                CloseLoader();
            }
        });
    }
});