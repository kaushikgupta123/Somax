//#region  Vendor Look up
var dtVendorTable;
var VenClientLookupId = "";
var VenName = "";
var VendorLookuptotalcount = 0;
var SelectVendorIDs = [];
var SelectVendorIDsDetails = [];

$(document).ready(function () {
    if ($('#IsMultistoreroomAutoPRGeneration').val() === 'True') {
        $(document).find('.select2picker').select2({});
        $('#StoreroomListModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
    }
    $(document).find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '.dismiss, .overlay', function () {
        $('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', '#AutoGenPRGensidebarCollapse', function () {
        $('.sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });

    $("#divAutoPRGenerationSearch").hide();
    $(document).find("#VendorIds").empty().select2({ multiple: true, minimumResultsForSearch: -1 });
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
   
});


$(document).on('click', '#opengrid', function () {
    VendorLookuptotalcount = 0;
     SelectVendorIDs = [];
     SelectVendorIDsDetails = [];
    generateVendorDataTable();
    $('.VendorLookupGriditemcount').text('0');
});
function generateVendorDataTable() {
    // var EquipmentId = $('#partsSessionData_EquipmentId').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#VendorsTable').hasClass('dataTable')) {
        dtVendorTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtVendorTable = $("#VendorsTable").DataTable({
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        "pageLength": 10,
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/AutoPRGeneration/VendorLookupListchunksearch_AutoPRGeneration",
            data: function (d) {
                d.ClientLookupId = LRTrim(VenClientLookupId);
                d.Name = LRTrim(VenName);
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                VendorLookuptotalcount = json.recordsTotal;
                if (SelectVendorIDsDetails.length == VendorLookuptotalcount && VendorLookuptotalcount != 0) {
                    $(document).find('.dt-body-center').find('#VendorLookupsearch-select-all').prop('checked', true);
                }
                else {
                    $(document).find('.dt-body-center').find('#VendorLookupsearch-select-all').prop('checked', false);
                }
                return json.data;
            }
        },
        "columns":
            [{
                "data": "VendorID",
                orderable: false,
                "bSortable": false,
                "autoWidth": false,
                className: 'select-checkbox dt-body-center',
                'render': function (data, type, full, meta) {
                    if ($('#VendorLookupsearch-select-all').is(':checked') && VendorLookuptotalcount == SelectVendorIDs.length) {
                        return '<input type="checkbox" checked="checked"  data-prid="' + data + '" class="chkVendorlookup ' + data + '"  value="'
                            + $('<div/>').text(data).html() + '">';
                    } else {
                        if (SelectVendorIDs.indexOf(data) != -1) {
                            return '<input type="checkbox" checked="checked" data-prid="' + data + '" class="chkVendorlookup ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                        else {
                            return '<input type="checkbox"  data-prid="' + data + '" class="chkVendorlookup ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                    }
                }
            },
                {
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    //"mRender": function (data, type, row) {
                    //    return '<a class=link_partvendor_detail href="javascript:void(0)">' + data + '</a>'
                    //}
                },
                {
                    "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true,
                }
            ],
        columnDefs: [
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            $(document).find('#tblvendorfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#partModal').hasClass('show')) {
                $(document).find('#partModal').modal("show");
            }
            $('#VendorsTable tfoot th').each(function (i, v) {
                var colIndex = i;
                if (colIndex > 0) {
                    var title = $('#VendorsTable thead th').eq($(this).index()).text();
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                    if (VenClientLookupId) { $('#colindex_1').val(VenClientLookupId); }
                    if (VenName) { $('#colindex_2').val(VenName); }
                }
            });

            $('#VendorsTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    VenClientLookupId = $('#colindex_1').val();
                    VenName = $('#colindex_2').val();
                    dtVendorTable.page('first').draw('page');
                }
            });
        }
    });
}
var order = '1';
var orderDir = 'asc';
$(document).on('click', '#VendorLookupsearch-select-all', function (e) {
    SelectVendorIDsDetails = [];
    SelectVendorIDs = [];
    var checked = this.checked;
    $.ajax({
        "url": "/AutoPRGeneration/GetVendorLookupListSelectAllData",
        data: {
            colname : order,
            coldir : orderDir,
            ClientLookupId: LRTrim(VenClientLookupId),
            Name: LRTrim(VenName)
        },
        async: true,
        type: "GET",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    if (checked) {
                        var exist = $.grep(SelectVendorIDsDetails, function (obj) {
                            return obj.id === item.VendorID;
                        });
                        if (exist.length == 0) {
                            var objVendorSelectItems = new VendorSelectItems(item.VendorID, item.ClientLookupId + ' - ' + item.Name);
                            SelectVendorIDsDetails.push(objVendorSelectItems);
                            SelectVendorIDs.push(item.VendorID);
                        }
                    }
                    else {
                        SelectVendorIDsDetails = $.grep(SelectVendorIDsDetails, function (obj) {
                            return obj.id !== item.VendorID;
                        });

                        var i = SelectVendorIDs.indexOf(item.VendorID);
                        SelectVendorIDs.splice(i, 1);
                    }

                });
            }
        },
        complete: function () {
            $('.VendorLookupGriditemcount').text(SelectVendorIDsDetails.length);

            if (checked) {

                $(document).find('.dt-body-center').find('.chkVendorlookup').prop('checked', 'checked');

            }
            else {
                $(document).find('.dt-body-center').find('.chkVendorlookup').prop('checked', false);


            }
            CloseLoader();
        }
    });
});


$(document).on('change', '.chkVendorlookup', function () {
    var index_row = $('#VendorsTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtVendorTable.row(row).data();
    if (!this.checked) {
        var el = $('#VendorLookupsearch-select-all').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }

        var index = SelectVendorIDs.indexOf(data.VendorID);
        SelectVendorIDs.splice(index, 1);
        SelectVendorIDsDetails.splice(index, 1);
    }
    else {
        
        var found = SelectVendorIDsDetails.some(function (el) {
            return el.id === data.VendorID;
        });
        if (!found) {
            var objVendorSelectItems = new VendorSelectItems(data.VendorID, data.ClientLookupId + ' - ' + data.Name);
            SelectVendorIDsDetails.push(objVendorSelectItems);
            SelectVendorIDs.push(data.VendorID);
        }

    }
    if (SelectVendorIDsDetails.length == VendorLookuptotalcount) {
        $(document).find('.dt-body-center').find('#VendorLookupsearch-select-all').prop('checked', 'checked');
    }
    else {
        $(document).find('.dt-body-center').find('#VendorLookupsearch-select-all').prop('checked', false);
    }

    $('.VendorLookupGriditemcount').text(SelectVendorIDs.length);
});




$(document).on('click', '#btnVendorProcess', function (e) {
    $("#divAutoPRGenerationSearch").show();
  /*  $('.PartGridSelectitemcount').text(0);*/
    generateAutoPRGenerationSearchDataTable();
});
$(document).on('click', '#btnApply', function () {
    if (SelectVendorIDs.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        $(document).find("#VendorIds").select2('destroy').empty().select2({ data: SelectVendorIDsDetails, multiple: true, minimumResultsForSearch: -1 });
        $("#VendorIds > option").prop("selected", true);
        $(document).find("#VendorIds").trigger('change');
        if ($(document).find('#partModal').hasClass('show')) {
            $(document).find('#partModal').modal("hide");
        }
    }
});
function VendorSelectItems(id, text) {
    this.id = id;
    this.text = text;
}
//#endregion 
//#region   Auto PR GenerationSearch
var Mainorder = '1';
var MainorderDir = 'asc';
var gridname = "AutoPRGeneration_Search";
var run = false;
var dtAutoPRGenerationSearch;
var PartLookuptotalcount = 0;
var SelectPartIDs = [];
function generateAutoPRGenerationSearchDataTable() {
    ProcessSelectedItemArray = [];
    var PartClientLookupId = LRTrim($(document).find("#APRGPartClientLookupId").val());
    var Description = LRTrim($(document).find("#APRGDescription").val());
    var UnitofMeasure = LRTrim($(document).find("#APRGUnitofMeasure").val());
    var VendorClientLookupId = LRTrim($(document).find("#APRGVendorClientLookupId").val());
    var VendorName = LRTrim($(document).find("#APRGVendorName").val());
    var QtyToOrder = $(document).find("#APRGQtyToOrder").val();
    var LastPurchaseCost = $(document).find("#APRGLastPurchaseCost").val();
    $('#AutoGenPRcontainer').show();
    if ($(document).find('#tblAutoPRGenerationSearch').hasClass('dataTable')) {
        dtAutoPRGenerationSearch.destroy();
    }
    dtAutoPRGenerationSearch = $("#tblAutoPRGenerationSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 2
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        stateSave: true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = Mainorder;
                    data.order[0][1] = MainorderDir;
                }

                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: gridname,
                        LayOutInfo: JSON.stringify(data),
                        FilterInfo: ''
                    },
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
            run = false;
        },
        "stateLoadCallback": function (settings, callback) {
            var o;
            $.ajax({
                "url": "/Base/GetLayout",
                "data": {
                    GridName: gridname
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    if (json.LayoutInfo) {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        Mainorder = LayoutInfo.order[0][0];
                        MainorderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                    }
                    else {
                        callback(json);
                    }

                }
            });
            //return o;
        },
        scrollX: true,
        fixedColumns: {
            leftColumns: 2
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Generate Auto PR List'
            },
            {
                extend: 'print',
                title: 'Generate Auto PR List'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Generate Auto PR List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Generate Auto PR List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/AutoPRGeneration/GetAutoPRGenerationGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.VendorIdList = $(document).find('#VendorIds').val();
                d.Order = Mainorder;
                d.PartClientLookupId = PartClientLookupId;
                d.Description = Description;
                d.UnitofMeasure = UnitofMeasure;
                d.VendorClientLookupId = VendorClientLookupId;
                d.VendorName = VendorName;
                d.QtyToOrder = QtyToOrder;
                d.LastPurchaseCost = LastPurchaseCost;
                d.StoreroomId = $(document).find('#autoPRGenerationStoreroomID').val();
            },
            "dataSrc": function (result) {
                let colOrder = dtAutoPRGenerationSearch.order();
                MainorderDir = colOrder[0][1];

                var i = 0;
                PartLookuptotalcount = result.recordsTotal;
                if (ProcessSelectedItemArray.length == PartLookuptotalcount && PartLookuptotalcount != 0) {
                    $(document).find('.dt-body-center').find('#AutoPRGenerationSearch-select-all').prop('checked', true);
                }
                else {
                    $(document).find('.dt-body-center').find('#AutoPRGenerationSearch-select-all').prop('checked', false);
                }
                if (result.data.length < 1) {
                    $(document).find('#btnAutoPRGenExport,#AutoPRGenerationSearch-select-all').prop('disabled', true);
                }
                else {
                    $(document).find('#btnAutoPRGenExport,#AutoPRGenerationSearch-select-alll').prop('disabled', false);
                }
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "PartId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    'render': function (data, type, full, meta) {
                        if ($('#AutoPRGenerationSearch-select-all').is(':checked') && PartLookuptotalcount == ProcessSelectedItemArray.length) {
                            return '<input type="checkbox" checked="checked"  data-prid="' + data + '" class="chkAutoPRGenerationSearch ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        } else {
                            if (ProcessSelectedItemArray.indexOf(data) != -1) {
                                return '<input type="checkbox" checked="checked" data-prid="' + data + '" class="chkAutoPRGenerationSearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                            else {
                                return '<input type="checkbox"  data-prid="' + data + '" class="chkAutoPRGenerationSearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                        }
                    }
                },
                { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2", },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "QtyToOrder", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4",
                    'render': function (data, type, full, meta) {
                        return "<div style='width:110px !important;'><input type='text' style='width:90px !important; float:left;text-align:right;' class='duration  dt-inline-text decimalinputupto2places grd-hours' autocomplete='off' value='" + data + "' maskedFormat='6,2'><i class='fa fa-check-circle is-saved-check' style='float: right; position: relative; top: 8px; right:-3px; color:green;display:none;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: relative; top: 8px; right:-3px; color:red;display:none;' title='test djhgjdfd kjhfhdksh'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:relative;top: 8px; right:-3px;display:none;' /></div>";
                    }
                },
                { "data": "UnitofMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
               
                { "data": "LastPurchaseCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
                { "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7" },
                { "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "8" }

            ],
        columnDefs: [
            {
                targets: [0,1],
                className: 'noVis'
            }
        ],
        initComplete: function (settings, json) {
            SetPageLengthMenu();
            DisableExportButton($("#tblAutoPRGenerationSearch"), $(document).find('#btnAutoPRGenExport,#AutoPRGenerationSearch-select-all'));
          
        }
    });
}
$(document).on('click', '#tblAutoPRGenerationSearch .paginate_button', function () {
    run = true;
});
$(document).on('change', '#tblAutoPRGenerationSearch .searchdt-menu', function () {
    run = true;
});
$('#tblAutoPRGenerationSearch').find('th').click(function () {
    if ($(this).data('col')) {
        run = true;
        Mainorder = $(this).data('col');
    }

});

$(document).on('click', '#AutoPRGenerationSearch-select-all', function (e) {
    ProcessSelectedItemArray = [];
    var checked = this.checked;
    var vendoridsArr = $(document).find('#VendorIds').val();
    var PartClientLookupId = LRTrim($(document).find("#APRGPartClientLookupId").val());
    var Description = LRTrim($(document).find("#APRGDescription").val());
    var UnitofMeasure = LRTrim($(document).find("#APRGUnitofMeasure").val());
    var VendorClientLookupId = LRTrim($(document).find("#APRGVendorClientLookupId").val());
    var VendorName = LRTrim($(document).find("#APRGVendorName").val());
    var QtyToOrder = $(document).find("#APRGQtyToOrder").val();
    var LastPurchaseCost = $(document).find("#APRGLastPurchaseCost").val();
    $.ajax({
        "url": "/AutoPRGeneration/GetAutoPRGenerationSearchSelectAllData",
        data: {
            VendorIdLists: vendoridsArr,
            colname: Mainorder,
            coldir: MainorderDir,
            PartClientLookupId : PartClientLookupId,
            Description : Description,
            UnitofMeasure : UnitofMeasure,
            VendorClientLookupId : VendorClientLookupId,
            VendorName : VendorName,
            QtyToOrder : QtyToOrder,
            LastPurchaseCost: LastPurchaseCost,
            StoreroomId : $(document).find('#autoPRGenerationStoreroomID').val()
        },
        async: true,
        type: "POST",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    if (checked) {
                            var partData = new funcPartTable(item.PartId, item.QtyToOrder);
                            ProcessSelectedItemArray.push(partData);
                    }
                    else {
                        var i = ProcessSelectedItemArray.indexOf(item.PartId);
                        ProcessSelectedItemArray.splice(i, 1);
                    }

                });
            }
        },
        complete: function () {
         /*   $('.PartGridSelectitemcount').text(SelectPartIDs.length);*/

            if (checked) {

                $(document).find('.dt-body-center').find('.chkAutoPRGenerationSearch').prop('checked', 'checked');

            }
            else {
                $(document).find('.dt-body-center').find('.chkAutoPRGenerationSearch').prop('checked', false);


            }
            CloseLoader();
        }
    });
});


$(document).on('change', '.chkAutoPRGenerationSearch', function () {
    var index_row = $('#tblAutoPRGenerationSearch tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtAutoPRGenerationSearch.row(row).data();
    var exist = $.grep(ProcessSelectedItemArray, function (obj) {
        return obj.PartId === data.PartId;
    });
    if (!this.checked) {
        var el = $('#AutoPRGenerationSearch-select-all').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
      
        if (ProcessSelectedItemArray.length > 0 && exist.length > 0) {
            var index = -1;
            for (var i = 0; i < ProcessSelectedItemArray.length; ++i) {
                if (ProcessSelectedItemArray[i].PartId === data.PartId) {
                    index = i;
                    break;
                }
            }
            ProcessSelectedItemArray.splice(index, 1);
            
        }
    }
    else {
        /*  SelectPartIDs.push(data.PartId);*/

        if (ProcessSelectedItemArray.length > 0 && exist.length > 0) {
            //no entry

        } else {
            var partData = new funcPartTable(data.PartId, data.QtyToOrder);
            ProcessSelectedItemArray.push(partData);

        }

    }

    
    if (ProcessSelectedItemArray.length == PartLookuptotalcount) {
        $(document).find('.dt-body-center').find('#AutoPRGenerationSearch-select-all').prop('checked', 'checked');
    }
    else {
        $(document).find('.dt-body-center').find('#AutoPRGenerationSearch-select-all').prop('checked', false);
    }

   /* $('.PartGridSelectitemcount').text(SelectPartIDs.length);*/
});
//#endregion
//#region  Create Purchase Request
$(document).on('click', '#btnOkPurchaseRequest', function () {
    $(document).find('.dt-body-center').find('#AutoPRGenerationSearch-select-all').prop('checked', false);
    $(document).find('.chkAutoPRGenerationSearch').prop('checked', false);
   /* $('.PartGridSelectitemcount').text(0);*/
    generateAutoPRGenerationSearchDataTable();
    $(document).find('#_PurchaseRequestAutoGeneration').modal('hide');
});
$(document).on('click', '#btnCreatePurchaseRequest', function (e) {
    
    if (ProcessSelectedItemArray.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var STData = ProcessSelectedItemArray;
        if (ProcessSelectedItemArray.length == 0)
            return;
        var STData = ProcessSelectedItemArray;
        var validqty = true;
        finalSelectedItemArray = [];

        $.each(STData, function (index, item) {
            if (item.OrderQuantity != "" && isNaN(item.OrderQuantity) == true) {
                validqty = false;
            }
            var existqty = $.grep(QtySelectedItemArray, function (obj) {
                return obj.PartId === item.PartId;
            });
            if (QtySelectedItemArray.length > 0 && existqty.length > 0) {
                var partData = new funcPartTable(item.PartId, existqty[0].OrderQuantity);
                finalSelectedItemArray.push(partData);
            } else {
                var partData = new funcPartTable(item.PartId,item.OrderQuantity);
                finalSelectedItemArray.push(partData);
            }
        });
        if (validqty == false) {
            ShowErrorAlert("Please enter valid Order Qty.");
            return false;
        }
        var jsonResult = JSON.stringify({ 'partTableLists': finalSelectedItemArray });
        
        {
            $.ajax({
                url: '/AutoPRGeneration/CreateAutoPRGeneration',
                data: jsonResult,
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (data) {
                    $(document).find('#lblPartsSelectedCount').html(data.ItemsReviewed);
                    $(document).find('#lblPurchaseRequestCreatedCount').html(data.HeadersCreated);
                    $(document).find('#lblLineItemsCreatedCount').html(data.DetailsCreated);
                    $(document).find('#_PurchaseRequestAutoGeneration').modal('show');
                },
                complete: function () {
                    CloseLoader();
                }
            });
        }
    }

});
//#endregion 

//#region export

$(document).on('click', '#liPdf', function () {
    $(".buttons-pdf")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liPrint', function () {
    $(".buttons-print")[0].click();
    funcCloseExportbtn();
});
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var vendoridsArr = $(document).find('#VendorIds').val();
            var PartClientLookupId = LRTrim($(document).find("#APRGPartClientLookupId").val());
            var Description = LRTrim($(document).find("#APRGDescription").val());
            var UnitofMeasure = LRTrim($(document).find("#APRGUnitofMeasure").val());
            var VendorClientLookupId = LRTrim($(document).find("#APRGVendorClientLookupId").val());
            var VendorName = LRTrim($(document).find("#APRGVendorName").val());
            var QtyToOrder = $(document).find("#APRGQtyToOrder").val();
            var LastPurchaseCost = $(document).find("#APRGLastPurchaseCost").val();
            var jsonResult = $.ajax({
                "url": "/AutoPRGeneration/GetAutoPRGenerationGridPrintData",
                "type": "POST",
                "datatype": "json",
                data: {
                    VendorIdLists: vendoridsArr,
                    colname: Mainorder,
                    coldir: MainorderDir,
                    PartClientLookupId: PartClientLookupId,
                    Description: Description,
                    UnitofMeasure: UnitofMeasure,
                    VendorClientLookupId: VendorClientLookupId,
                    VendorName: VendorName,
                    QtyToOrder: QtyToOrder,
                    LastPurchaseCost: LastPurchaseCost,
                    StoreroomId : $(document).find('#autoPRGenerationStoreroomID').val()
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#tblAutoPRGenerationSearch thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.PartClientLookupId != null) {
                    item.PartClientLookupId = item.PartClientLookupId;
                }
                else {
                    item.PartClientLookupId = "";
                }
                if (item.Description != null) {
                    item.Description = item.Description;
                }
                else {
                    item.Description = "";
                }
                if (item.QtyToOrder != null) {
                    item.QtyToOrder = item.QtyToOrder;
                }
                else {
                    item.QtyToOrder = "";
                }
                if (item.UnitofMeasure != null) {
                    item.UnitofMeasure = item.UnitofMeasure;
                }
                else {
                    item.UnitofMeasure = "";
                }
                if (item.LastPurchaseCost != null) {
                    item.LastPurchaseCost = item.LastPurchaseCost;
                }
                else {
                    item.LastPurchaseCost = "";
                }
                if (item.VendorClientLookupId != null) {
                    item.VendorClientLookupId = item.VendorClientLookupId;
                }
                else {
                    item.VendorClientLookupId = "";
                }
                if (item.VendorName != null) {
                    item.VendorName = item.VendorName;
                }
                else {
                    item.VendorName = "";
                }

                var fData = [];
                $.each(visiblecolumnsIndex, function (index, inneritem) {
                    var key = Object.keys(item)[inneritem];
                    var value = item[key]
                    fData.push(value);
                });
                d.push(fData);
            })
            return {
                body: d,
                header: $("#tblAutoPRGenerationSearch thead tr th").not(":eq(0)").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});
//#endregion
//#region Advance Search
$(document).on('click', "#btnAutoPRGenDataAdvSrch", function (e) {
    dtAutoPRGenerationSearch.state.clear();
    var searchitemhtml = "";
    hGridfilteritemcount = 0
    $('#advsearchsidebarAutoPRGenerated').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossHistory" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('.sidebar').removeClass('active');
    $('#AutoPRGeneratedadvsearchcontainer').find('#sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    GridAdvanceSearch();
});
function GridAdvanceSearch() {
    generateAutoPRGenerationSearchDataTable();
    $('.filteritemcount').text(hGridfilteritemcount);
}
$(document).on('click', '.btnCrossHistory', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    GridAdvanceSearch();
});
$(document).on('click', '#liClearAdvSearchFilterAVAutoPRGen', function () {
    clearAdvanceSearch();
    generateAutoPRGenerationSearchDataTable();
});
function clearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebarAutoPRGenerated').find('input:text,input[type="number"]').val('');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
//#endregion


//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtAutoPRGenerationSearch, true);
});
$(document).on('click', '.saveConfig', function () {

    var colOrder = [0,1];
    funCustozeSaveBtn(dtAutoPRGenerationSearch, colOrder);
    run = true;
    dtAutoPRGenerationSearch.state.save(run);
});
//#endregion

//#region Quantity 

var enterhit = 0;

$(document).on('change', '.grd-hours', function () {
    var row = $(this).parents('tr');
    var data = dtAutoPRGenerationSearch.row(row).data();
    var keycode = (event.keyCode ? event.keyCode : event.which);
    var thstextbox = $(this);
    var PartId = data.PartId;
    var OrderQuantity = thstextbox.val();
    thstextbox.siblings('.is-saved-check').show();
    enterhit = 1;
    if (thstextbox.val() == "" || thstextbox.val() == "0") {
        thstextbox.siblings('.is-saved-check').hide();
    }
    var exist = $.grep(QtySelectedItemArray, function (obj) {
        return obj.PartId === PartId;
    });
    if (QtySelectedItemArray.length > 0 && exist.length > 0) {
        var index = -1;
        for (var i = 0; i < QtySelectedItemArray.length; ++i) {
            if (QtySelectedItemArray[i].PartId === PartId) {
                index = i;
                break;
            }
        }
        if (OrderQuantity == "" || OrderQuantity == "0") {
            QtySelectedItemArray.splice(index, 1);
        }
        else {
            QtySelectedItemArray[index].OrderQuantity = OrderQuantity;
        }
    }
    else {
        var partData = new funcPartTable(PartId, OrderQuantity);
        QtySelectedItemArray.push(partData);
    }
    event.stopPropagation();
});
//#endregion
//#region Estimated hour edit
var ProcessSelectedItemArray = [];
var QtySelectedItemArray = [];
var finalSelectedItemArray = [];
function funcPartTable(PartId, OrderQuantity) {
    this.PartId = PartId;
    this.OrderQuantity = OrderQuantity;
}

//#endregion


function LoadAutoPRGeneration() {
    $(document).find('#StoreroomListModalpopup').modal('hide');
    $.ajax({
        url: "/AutoPRGeneration/LoadAutoPRGeneration",
        type: "GET",
        dataType: 'html',
        //data: { MaterialRequestId: MaterialRequestId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AutoPRGeneration').html(data);
            $(document).find("#VendorIds").empty().select2({ multiple: true, minimumResultsForSearch: -1 });
            
        },
        complete: function () {
            
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}

function ValidateStoreroomSelectionOnSuccess(data) {
    $('.modal-backdrop').remove();
    if (data.data === "success") {
        $(document).find('#autoPRGenerationStoreroomID').val(data.StoreroomId);
        $(document).find('#StoreroomListModalpopup').hide();
        ShowLoader();
         LoadAutoPRGeneration();
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#btnSubmitStoreroomcancel,.clearstate', function () {
    window.location.href = "../Dashboard/Dashboard";

});
$(document).on("change", "#StoreroomId", function () {
    chargeToSelected = $(this).val();
    chargeToText = $('option:selected', this).text();
    var tlen = $(document).find("#StoreroomId").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#StoreroomId").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#StoreroomId").removeClass("input-validation-error");
    }
    else {
        var areaddescribedby = $(document).find("#StoreroomId").attr('aria-describedby');
        $('#' + areaddescribedby).show();
        $(document).find('form').find("#StoreroomId").addClass("input-validation-error");
    }
});
