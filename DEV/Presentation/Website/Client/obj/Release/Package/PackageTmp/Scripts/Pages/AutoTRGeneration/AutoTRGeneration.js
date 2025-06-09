//#region  Storeroom Look up
var dtStoreroomTable;
var StoreroomId = "";
var Name = "";
var Description = "";
var StoreroomLookuptotalcount = 0;
var SelectStoreroomIDs = [];
var SelectStoreroomIDsDetails = [];

$(document).ready(function () {
    $(document).find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '.dismiss, .overlay', function () {
        $('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $("#divAutoTRGenerationSearch").hide();
    $(document).find("#StoreroomIds").empty().select2({ multiple: true, minimumResultsForSearch: -1 });
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });

});
$(document).on('click', '#opengrid', function () {
    StoreroomLookuptotalcount = 0;
    SelectStoreroomIDs = []; 
    SelectStoreroomIDsDetails = [];
    generateStoreroomDataTable();
    $('.StoreroomLookupGriditemcount').text('0');
});
function generateStoreroomDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#StoreroomTable').hasClass('dataTable')) {
        dtStoreroomTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtStoreroomTable = $("#StoreroomTable").DataTable({
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
            "url": "/AutoTRGeneration/StoreroomLookupListchunksearch_AutoTRGeneration",
            data: function (d) {
                d.StoreroomId = LRTrim(StoreroomId);
                d.Name = LRTrim(Name);
                d.Description = LRTrim(Description);
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                StoreroomLookuptotalcount = json.recordsTotal;
                if (SelectStoreroomIDsDetails.length == StoreroomLookuptotalcount && StoreroomLookuptotalcount != 0) {
                    $(document).find('.dt-body-center').find('#StoreroomLookupsearch-select-all').prop('checked', true);
                }
                else {
                    $(document).find('.dt-body-center').find('#StoreroomLookupsearch-select-all').prop('checked', false);
                }
                return json.data;
            }
        },
        "columns":
            [{
                "data": "StoreroomId",
                orderable: false,
                "bSortable": false,
                "autoWidth": false,
                className: 'select-checkbox dt-body-center',
                'render': function (data, type, full, meta) {
                    if ($('#StoreroomLookupsearch-select-all').is(':checked') && StoreroomLookuptotalcount == SelectStoreroomIDs.length) {
                        return '<input type="checkbox" checked="checked"  data-stid="' + data + '" class="chkStoreroomlookup ' + data + '"  value="'
                            + $('<div/>').text(data).html() + '">';
                    } else {
                        if (SelectStoreroomIDs.indexOf(data) != -1) {
                            return '<input type="checkbox" checked="checked" data-stid="' + data + '" class="chkStoreroomlookup ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                        else {
                            return '<input type="checkbox"  data-stid="' + data + '" class="chkStoreroomlookup ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                    }
                }
            },
            {
                "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true,
            },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
            }
            ],
        columnDefs: [
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            $(document).find('#tblStoreroomfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#partModal').hasClass('show')) {
                $(document).find('#partModal').modal("show");
            }
            $('#StoreroomTable tfoot th').each(function (i, v) {
                var colIndex = i;
                if (colIndex > 0) {
                    var title = $('#StoreroomTable thead th').eq($(this).index()).text();
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                    if (Name) { $('#colindex_1').val(Name); }
                    if (Description) { $('#colindex_2').val(Description); }
                }
            });

            $('#StoreroomTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    Name = $('#colindex_1').val();
                    Description = $('#colindex_2').val();
                    dtStoreroomTable.page('first').draw('page');
                }
            });
        }
    });
}
var order = '1';
var orderDir = 'asc';
$(document).on('click', '#StoreroomLookupsearch-select-all', function (e) {
    SelectStoreroomIDsDetails = [];
    SelectStoreroomIDs = [];
    var checked = this.checked;
    $.ajax({
        "url": "/AutoTRGeneration/GetStoreroomLookupListSelectAllData",
        data: {
            colname: order,
            coldir: orderDir,
            StoreroomId: LRTrim(StoreroomId),
            Name: LRTrim(Name),
            Description: LRTrim(Description)
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
                        var exist = $.grep(SelectStoreroomIDsDetails, function (obj) {
                            return obj.id === item.StoreroomId;
                        });
                        if (exist.length == 0) {
                            var objStoreroomSelectItems = new StoreroomSelectItems(item.StoreroomId, item.Name, item.Description);
                            SelectStoreroomIDsDetails.push(objStoreroomSelectItems);
                            SelectStoreroomIDs.push(item.StoreroomId);
                        }
                    }
                    else {
                        SelectStoreroomIDsDetails = $.grep(SelectStoreroomIDsDetails, function (obj) {
                            return obj.id !== item.StoreroomId;
                        });
                        var i = SelectStoreroomIDs.indexOf(item.StoreroomId);
                        SelectStoreroomIDs.splice(i, 1);
                    }

                });
            }
        },
        complete: function () {
            $('.StoreroomLookupGriditemcount').text(SelectStoreroomIDsDetails.length);
            if (checked) {
                $(document).find('.dt-body-center').find('.chkStoreroomlookup').prop('checked', 'checked');
            }
            else {
                $(document).find('.dt-body-center').find('.chkStoreroomlookup').prop('checked', false);
            }
            CloseLoader();
        }
    });
});
$(document).on('change', '.chkStoreroomlookup', function () {
    var index_row = $('#StoreroomTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtStoreroomTable.row(row).data();
    if (!this.checked) {
        var el = $('#StoreroomLookupsearch-select-all').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        var index = SelectStoreroomIDs.indexOf(data.StoreroomId);
        SelectStoreroomIDs.splice(index, 1);
        SelectStoreroomIDsDetails.splice(index, 1);
    }
    else {
        var found = SelectStoreroomIDsDetails.some(function (el) {
            return el.id === data.StoreroomId;
        });
        if (!found) {
            var objStoreroomSelectItems = new StoreroomSelectItems(data.StoreroomId, data.Name,data.Description);
            SelectStoreroomIDsDetails.push(objStoreroomSelectItems);
            SelectStoreroomIDs.push(data.StoreroomId);
        }
    }
    if (SelectStoreroomIDsDetails.length == StoreroomLookuptotalcount) {
        $(document).find('.dt-body-center').find('#StoreroomLookupsearch-select-all').prop('checked', 'checked');
    }
    else {
        $(document).find('.dt-body-center').find('#StoreroomLookupsearch-select-all').prop('checked', false);
    }

    $('.StoreroomLookupGriditemcount').text(SelectStoreroomIDs.length);
});
$(document).on('click', '#btnApply', function () {
    if (SelectStoreroomIDs.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        $(document).find("#StoreroomIds").select2('destroy').empty().select2({ data: SelectStoreroomIDsDetails, multiple: true, minimumResultsForSearch: -1 });
        $("#StoreroomIds > option").prop("selected", true);
        $(document).find("#StoreroomIds").trigger('change');
        if ($(document).find('#partModal').hasClass('show')) {
            $(document).find('#partModal').modal("hide");
        }
    }
});
function StoreroomSelectItems(id, text) {
    this.id = id;
    this.text = text;
}

//#region   Auto TR GenerationSearch
$(document).on('click', '#btnStoreroomProcess', function (e) {
    $("#divAutoTRGenerationSearch").show();
    generateAutoTRGenerationSearchDataTable();
});


var Mainorder = '1';
var MainorderDir = 'asc';
var gridname = "AutoTRGeneration_Search";
var run = false;
var dtAutoTRGenerationSearch;
var PartLookuptotalcount = 0;
function generateAutoTRGenerationSearchDataTable() {
    ProcessSelectedItemArray = [];
    SelectedItemArray = [];
    $('#AutoGenTRcontainer').show();
    if ($(document).find('#tblAutoTRGenerationSearch').hasClass('dataTable')) {
        dtAutoTRGenerationSearch.destroy();
    }
    dtAutoTRGenerationSearch = $("#tblAutoTRGenerationSearch").DataTable({
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
                title: 'Generate Auto TR List'
            },
            {
                extend: 'print',
                title: 'Generate Auto TR List'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Generate Auto TR List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Generate Auto TR List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/AutoTRGeneration/GetAutoTRGenerationGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.StoreroomIdList = $(document).find('#StoreroomIds').val();
                d.Order = Mainorder;
            },
            "dataSrc": function (result) {
                let colOrder = dtAutoTRGenerationSearch.order();
                MainorderDir = colOrder[0][1];

                var i = 0;
                PartLookuptotalcount = result.recordsTotal;
                if (ProcessSelectedItemArray.length == PartLookuptotalcount && PartLookuptotalcount != 0) {
                    $(document).find('.dt-body-center').find('#AutoTRGenerationSearch-select-all').prop('checked', true);
                }
                else {
                    $(document).find('.dt-body-center').find('#AutoTRGenerationSearch-select-all').prop('checked', false);
                }
                if (result.data.length < 1) {
                    $(document).find('#btnAutoTRGenExport,#AutoTRGenerationSearch-select-all').prop('disabled', true);
                }
                else {
                    $(document).find('#btnAutoTRGenExport,#AutoTRGenerationSearch-select-alll').prop('disabled', false);
                }
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "RowId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    'render': function (data, type, full, meta) {
                        if ($('#AutoTRGenerationSearch-select-all').is(':checked') && PartLookuptotalcount == SelectedItemArray.length) {
                            return '<input type="checkbox" checked="checked"  data-prid="' + data + '" class="chkAutoTRGenerationSearch ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        } else {
                            if (SelectedItemArray.indexOf(data) != -1) {
                                return '<input type="checkbox" checked="checked" data-prid="' + data + '" class="chkAutoTRGenerationSearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                            else {
                                return '<input type="checkbox"  data-prid="' + data + '" class="chkAutoTRGenerationSearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                        }
                    }
                },
                { "data": "PartIdClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2", },
                { "data": "RequestStr", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                { "data": "IssueStr", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                {
                    "data": "PartDescription", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "TransferQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
                { "data": "Max", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7" },
                { "data": "Min", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "8" },
                { "data": "OnHand", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "9" }
            ],
        columnDefs: [
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        initComplete: function (settings, json) {
            
            SetPageLengthMenu();
            DisableExportButton($("#tblAutoTRGenerationSearch"), $(document).find('#btnAutoTRGenExport,#AutoTRGenerationSearch-select-all'));

        }
    });
}
$(document).on('click', '#tblAutoTRGenerationSearch .paginate_button', function () {
    run = true;
});
$(document).on('change', '#tblAutoTRGenerationSearch .searchdt-menu', function () {
    run = true;
});
$('#tblAutoTRGenerationSearch').find('th').click(function () {
    if ($(this).data('col')) {
        run = true;
        Mainorder = $(this).data('col');
    }

});

$(document).on('click', '#AutoTRGenerationSearch-select-all', function (e) {
    ProcessSelectedItemArray = [];
    SelectedItemArray = [];
    var checked = this.checked;
    var StoreroomidsArr = $(document).find('#StoreroomIds').val();
    $.ajax({
        "url": "/AutoTRGeneration/GetAutoTRGenerationSearchSelectAllData",
        data: {
            StoreroomIdLists: StoreroomidsArr,
            colname: Mainorder,
            coldir: MainorderDir
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
                        var partData = new funcPartTranProcessTable(item.RowId, item.RequestPTStoreroomId, item.RequestStoreroomId, item.RequestPartId, item.IssuePTStoreroomId, item.IssueStoreroomId, item.IssuePartId, item.Creator_PersonnelId, item.TransferQuantity);
                        ProcessSelectedItemArray.push(partData);
                        SelectedItemArray.push(item.RowId);
                    }
                    else {
                        var i = ProcessSelectedItemArray.indexOf(item.RowId);
                        ProcessSelectedItemArray.splice(i, 1);

                        var i = SelectedItemArray.indexOf(item.RowId);
                        SelectedItemArray.splice(i, 1);
                    }

                });
            }
        },
        complete: function () {
            if (checked) {
                $(document).find('.dt-body-center').find('.chkAutoTRGenerationSearch').prop('checked', 'checked');
            }
            else {
                $(document).find('.dt-body-center').find('.chkAutoTRGenerationSearch').prop('checked', false);
            }
            CloseLoader();
        }
    });
});

$(document).on('change', '.chkAutoTRGenerationSearch', function () {
    var data = dtAutoTRGenerationSearch.row($(this).parents('tr')).data();
    if (!this.checked) {
        var el = $('#AutoTRGenerationSearch-select-all').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }

        var index = SelectedItemArray.indexOf(data.RowId);
        SelectedItemArray.splice(index, 1);
        ProcessSelectedItemArray.splice(index, 1);
        
    }
    else {
        var item = data.RowId;
        var found = ProcessSelectedItemArray.some(function (el) {
            return el.RowId === data.RowId;
        });
        if (!found) {
            var partData = new funcPartTranProcessTable(data.RowId, data.RequestPTStoreroomId, data.RequestStoreroomId, data.RequestPartId, data.IssuePTStoreroomId, data.IssueStoreroomId, data.IssuePartId, data.Creator_PersonnelId, data.TransferQuantity);
            ProcessSelectedItemArray.push(partData);
            SelectedItemArray.push(data.RowId);
        }

    }
    if (ProcessSelectedItemArray.length.length == PartLookuptotalcount) {
        $(document).find('.dt-body-center').find('#AutoTRGenerationSearch-select-all').prop('checked', 'checked');
    }
    else {
        $(document).find('.dt-body-center').find('#AutoTRGenerationSearch-select-all').prop('checked', false);
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
            var StoreroomidsArr = $(document).find('#StoreroomIds').val();
            var jsonResult = $.ajax({
                "url": "/AutoTRGeneration/GetAutoTRGenerationGridPrintData",
                "type": "POST",
                "datatype": "json",
                data: {
                    StoreroomIdLists: StoreroomidsArr,
                    colname: Mainorder,
                    coldir: MainorderDir
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#tblAutoTRGenerationSearch thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.PartIdClientLookupId != null) {
                    item.PartIdClientLookupId = item.PartIdClientLookupId;
                }
                else {
                    item.PartIdClientLookupId = "";
                }
                if (item.RequestStr != null) {
                    item.RequestStr = item.RequestStr;
                }
                else {
                    item.RequestStr = "";
                }
                if (item.IssueStr != null) {
                    item.IssueStr = item.IssueStr;
                }
                else {
                    item.IssueStr = "";
                }
                if (item.PartDescription != null) {
                    item.PartDescription = item.PartDescription;
                }
                else {
                    item.PartDescription = "";
                }
                if (item.TransferQuantity != null) {
                    item.TransferQuantity = item.TransferQuantity;
                }
                else {
                    item.TransferQuantity = "";
                }
                if (item.Max != null) {
                    item.Max = item.Max;
                }
                else {
                    item.Max = "";
                }
                if (item.Min != null) {
                    item.Min = item.Min;
                }
                else {
                    item.Min = "";
                }
                if (item.OnHand != null) {
                    item.OnHand = item.OnHand;
                }
                else {
                    item.OnHand = "";
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
                header: $("#tblAutoTRGenerationSearch thead tr th").not(":eq(0)").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});
//#endregion

$(document).on('click', '#btnCreateTransferRequest', function (e) {

    if (ProcessSelectedItemArray.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        if (ProcessSelectedItemArray.length == 0)
            return;
        var jsonResult = JSON.stringify({ 'partTranProcessTableLists': ProcessSelectedItemArray });
        $.ajax({
            url: '/AutoTRGeneration/CreateAutoTRGeneration',
            data: jsonResult,
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                addToStoreroomTransferOnSuccess(data)
            },
            complete: function () {
                CloseLoader();
            }
        });
    }
});
function addToStoreroomTransferOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("spnAddStoreroomTransfer");
        swal(SuccessAlertSetting, function () {
            finalSelectedItemArray = [];
            ProcessSelectedItemArray = [];
            SelectStoreroomIDs = [];
            SelectStoreroomIDsDetails = [];
            window.location.href = "../AutoTRGeneration/Index";
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data.error);
    }
}
//#endregion

//#region Estimated hour edit
var ProcessSelectedItemArray = [];
var SelectedItemArray = [];
function funcPartTranProcessTable(RowId, RequestPTStoreroomId, RequestStoreroomId, RequestPartId, IssuePTStoreroomId, IssueStoreroomId, IssuePartId, Creator_PersonnelId, TransferQuantity) {
    this.RowId = RowId;
    this.RequestPTStoreroomId = RequestPTStoreroomId;
    this.RequestStoreroomId = RequestStoreroomId;
    this.RequestPartId = RequestPartId;
    this.IssuePTStoreroomId = IssuePTStoreroomId;
    this.IssueStoreroomId = IssueStoreroomId;
    this.IssuePartId = IssuePartId;
    this.Creator_PersonnelId = Creator_PersonnelId;
    this.TransferQuantity = TransferQuantity;
}
//#endregion

//#endregion 

