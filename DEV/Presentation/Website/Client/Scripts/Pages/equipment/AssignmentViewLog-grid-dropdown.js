var dtAssignmentViewLogTable;
var AvlTransactionDate="";
var AvlStatus="";
var AvlPersonnelId;
var AvlAssigned;
var AvlLocation;
var AvlParentId;
var AvlAssetGroup1;
var AvlAssetGroup2;
var AvlAssetGroup3;

function generateAssignmentViewLogDataTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#AssignmentViewLogTable').hasClass('dataTable')) {
        dtAssignmentViewLogTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtAssignmentViewLogTable = $("#AssignmentViewLogTable").DataTable({
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
            "url": "/Equipment/AssignmentViewLogLookupList",
            data: function (d) {
                d.ObjectId = EquipmentId;
                d.TransactionDate = AvlTransactionDate;
                d.Status = AvlStatus;
                d.PersonnelId = AvlPersonnelId;
                d.Assigned = AvlAssigned;
                d.Location = AvlLocation;
                d.ParentId = AvlParentId;
                d.AssetGroup1 = AvlAssetGroup1;
                d.AssetGroup2 = AvlAssetGroup2;
                d.AssetGroup3 = AvlAssetGroup3;

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
                    "data": "TransactionDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date",
                    "mRender": function (data, type, row) {
                        return "<div>" + data + "</div>";
                    }
                },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return '<div class="m - badge--custom">' + data + '</div>';
                    }
                },
                {
                    "data": "PersonnelName", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>';
                    }
                },
                {
                    "data": "AssignedClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>';
                    }
                },
                {
                    "data": "Location", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap'>" + data + "</div>";
                    }
                },
                {
                    "data": "ParentClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>';
                    }
                },
                {
                    "data": "AssetGroup1Name", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>';
                    }
                },
                {
                    "data": "AssetGroup2Name", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>';
                    }
                },
                {
                    "data": "AssetGroup3Name", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>';
                    }
                },

            ],

        initComplete: function () {
            $(document).find('#tblAssignmentLogfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#AssignmentViewLogModal').hasClass('show')) {
                $(document).find('#AssignmentViewLogModal').modal("show");
            }
            $(document).find('#AssignmentViewLogTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#AssignmentViewLogTable thead th').eq($(this).index()).text();
                if (colIndex == 0) {
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt dtpicker" id="assign_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                }
                else {
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="assign_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                }
                if (AvlTransactionDate) { $('#assign_colindex_0').val(AvlTransactionDate); }
                if (AvlStatus) { $('#assign_colindex_1').val(AvlStatus); }
                if (AvlPersonnelId) { $('#assign_colindex_2').val(AvlPersonnelId); }
                if (AvlAssigned) { $('#assign_colindex_3').val(AvlAssigned); }
                if (AvlLocation) { $('#assign_colindex_4').val(AvlLocation); }
                if (AvlParentId) { $('#assign_colindex_5').val(AvlParentId); }
                if (AvlAssetGroup1) { $('#assign_colindex_6').val(AvlAssetGroup1); }
                if (AvlAssetGroup2) { $('#assign_colindex_7').val(AvlAssetGroup2); }
                if (AvlAssetGroup3) { $('#assign_colindex_8').val(AvlAssetGroup3); }


            });
            $(document).find('.dtpicker').datepicker({
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true,
                onSelect: function () {
                    this.focus();
                }
                
            }).inputmask('mm/dd/yyyy');

            $('#AssignmentViewLogTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());

                    AvlTransactionDate= $('#assign_colindex_0').val(); 
                    AvlStatus= $('#assign_colindex_1').val(); 
                    AvlPersonnelId= $('#assign_colindex_2').val();
                    AvlAssigned= $('#assign_colindex_3').val(); 
                    AvlLocation= $('#assign_colindex_4').val(); 
                    AvlParentId= $('#assign_colindex_5').val(); 
                    AvlAssetGroup1= $('#assign_colindex_6').val();
                    AvlAssetGroup2= $('#assign_colindex_7').val(); 
                    AvlAssetGroup3= $('#assign_colindex_8').val(); 
                    dtAssignmentViewLogTable.page('first').draw('page');
                    this.blur();
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
