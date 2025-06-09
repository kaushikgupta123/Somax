var fleetissueTable;
var FIDate = "";
var FIDescription = "";
var FIStatus = "";
var FIDefects = "";

//$(document).on('click', '#openFIgrid', function () {    
//    var index = $(this).data('attr');
//    generateSOFleetIssueTable(index);
//});
function generateSOFleetIssueTable(ServiceOrderLineItemId, index) {
   
    var EquipmentId = $('#ServiceOrderequipId_'+ index).val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#FleetIssuePopupTable').hasClass('dataTable')) {
        fleetissueTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    fleetissueTable = $("#FleetIssuePopupTable").DataTable({
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
            "url": "/Base/FleetIssueLookupList",
            data: function (d) {
                d.EquipmentId = EquipmentId;
                d.Date = FIDate;
                d.Description = FIDescription;
                d.Status = FIStatus;
                d.Defects = FIDefects;
               
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
                "data": "Readingdate", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {                    
                    return '<a class=link_SOFleetIssue_detail href="javascript:void(0)" data-attr='+index+'>' + moment(row.Readingdate).format('MM/DD/YYYY')+ '</a>'
                }
            },
            { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true
            },
            { "data": "Defects", "autoWidth": true, "bSearchable": true, "bSortable": true },            
        ],
        "rowCallback": function (row, data, index, full) {
            var colType = this.api().column(3).index('visible');
        },
        initComplete: function () {
            $(document).find('#tblFleetIssuefooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#ServiceOrderFleetIssueModal').hasClass('show')) {
                $(document).find('#ServiceOrderFleetIssueModal').modal("show");
            }

            $('#FleetIssuePopupTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#FleetIssuePopupTable thead th').eq($(this).index()).text();
                if (title === "Record Date")
                { $(this).html('<input type="text" class="tfootsearchtxt dtpickerNew " id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>'); }                                
                else
                { $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');}                
                if (FIDate) { $('#colindex_0').val(FIDate ); }
                if (FIDescription) { $('#colindex_1').val(FIDescription ); }
                if (FIStatus) { $('#colindex_2').val(FIStatus ); }
                if (FIDefects) { $('#colindex_3').val(FIDefects); } 
            });

            $('#FleetIssuePopupTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];                  
                    var searchText = LRTrim($(this).val());
                    FIDate = $('#colindex_0').val();
                    FIDescription = $('#colindex_1').val();
                    FIStatus = $('#colindex_2').val();
                    FIDefects = $('#colindex_3').val();                    
                    fleetissueTable.page('first').draw('page');                    
                }
               
            });

            $(document).find(".dtpickerNew").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                maxDate: new Date(),
                buttonImage: '/Images/calender.png'
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
$(document).on('click', '.link_SOFleetIssue_detail', function (e) {
    var index_row = $('#FleetIssuePopupTable tr').index($(this).closest('tr'))-1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = fleetissueTable.row(row).data();    
    var description = $(document).find('#FIDescription').val();
    var index = $(this).data('attr');    
    var s = $(document).find('#FIDescription_' + index).val(33);
    $(document).find('#FIDescription_' + index).val(data.Description);
    $(document).find('#hdnFleetIssueId_' + index).val(data.FleetIssuesId);
    $(document).find("#ServiceOrderFleetIssueModal").modal('hide');    
    $(document).find('.reset').prop("checked", false);

    CloseLoader();
});