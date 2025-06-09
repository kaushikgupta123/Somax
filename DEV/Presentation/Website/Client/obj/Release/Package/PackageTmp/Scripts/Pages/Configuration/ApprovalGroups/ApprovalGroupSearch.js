//#region Common
var ApprovalGroupTable;
var run = false;
var selectCount = 0;
var activeStatus = false;
var order = '1';
var orderDir = 'asc';
var gridname = "ApprovalGroup_Search";
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
//#endregion
//#region Search
$(document).ready(function () {
    generateApprovalGroupTable();

    //ShowbtnLoaderclass("LoaderDrop");
    //ShowbtnLoader("btnsortmenu");
    $("#ApprovalGroupSearchAction :input").attr("disabled", "disabled");
    $(".actionBar").fadeIn();
    $(document).find('.select2picker').select2({});
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        var validity = false;
        if ($(this).closest('form').length > 0) {
            validity = $(this).valid();
        }
        if (validity == true) {
            if (typeof areaddescribedby != 'undefined') {
                $('#' + areaddescribedby).hide();
            }
        }
        else {
            if (typeof areaddescribedby != 'undefined') {
                $('#' + areaddescribedby).show();
            }
        }
    });
});
function generateApprovalGroupTable() {
    var printCounter = 0;
    
    if ($(document).find('#ApprovalGroupSearch').hasClass('dataTable')) {
        ApprovalGroupTable.destroy();
    }
    ApprovalGroupTable = $("#ApprovalGroupSearch").DataTable({
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
        //"stateSaveCallback": function (settings, data) {
        //    if (run == true) {
        //        $.ajax({
        //            "url": "/Base/CreateUpdateState",
        //            "data": {
        //                GridName: "ApprovalGroup_Search",
        //                LayOutInfo: JSON.stringify(data),
        //                FilterInfo: JSON.stringify(filterinfoarray)
        //            },
        //            "dataType": "json",
        //            "type": "POST",
        //            "success": function () { return; }
        //        });
        //    }
        //    run = false;
        //},

        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: gridname,
                        LayOutInfo: JSON.stringify(data),
                        FilterInfo: JSON.stringify(filterinfoarray)
                    },
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
            run = false;
        },
        //"stateLoadCallback": function (settings, callback) {
        //    $.ajax({
        //        "url": "/Base/Getlayout",
        //        "data": {
        //            GridName: "ApprovalGroup_Search",
        //        },
        //        "async": false,
        //        "dataType": "json",
        //        //"success": function (json) {
        //        //    if (json) {
        //        //        callback(JSON.parse(json));
        //        //    }
        //        //    else {
        //        //        callback(json);
        //        //    }
        //        //}
        //        "success": function (json) {
        //            if (json.LayoutInfo) {
        //                var LayoutInfo = JSON.parse(json.LayoutInfo);
        //                order = LayoutInfo.order[0][0];
        //                orderDir = LayoutInfo.order[0][1];
        //                callback(JSON.parse(json.LayoutInfo));
        //                if (json.FilterInfo) {
        //                    setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#dvFilterSearchSelect2"));
        //                }
        //            }
        //            else {
        //                callback(json);
        //            }

        //        }
        //    });
        //},
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
                        order = LayoutInfo.order[0][0];
                        orderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#dvFilterSearchSelect2"));
                        }
                    }
                    else {
                        callback(json);
                    }

                }
            });
            //return o;
        },
        scrollX: true,
        //fixedColumns: {
        //    leftColumns: 1
        //},
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Approval Group'
            },
            {
                extend: 'print',
                title: 'Approval Group',
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Approval Group',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                title: 'Approval Group',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/ApprovalGroups/GetGridDataforApprovalGroup",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.Order = order;
                d.RequestType = LRTrim($('#RequestType').val());
                d.Description = LRTrim($('#Description').val());
                d.ApprovalGroupId = LRTrim($('#ApprovalGroupId').val());
                d.AssetGroup1Id = $(document).find('#AssetGroup1Id').val();
                d.AssetGroup2Id = $(document).find('#AssetGroup2Id').val();
                d.AssetGroup3Id = $(document).find('#AssetGroup3Id').val();
                d.txtSearchval = $(document).find('#txtColumnSearch').val();
            },
            "dataSrc": function (result) {
                let colOrder = ApprovalGroupTable.order();
                orderDir = colOrder[0][1];
                //order = colOrder[0][0];
              
                if (result.data.length == "0") {
                    $(document).find('.import-export').attr('disabled', 'disabled');
                }
                else {
                    $(document).find('.import-export').removeAttr('disabled');
                }

                return result.data;
            },
            complete: function () {
                CloseLoader();
                $("#ApprovalGroupSearchAction :input").not('.import-export').removeAttr("disabled");
                $("#ApprovalGroupSearchAction :button").not('.import-export').removeClass("disabled");
            },
            global: true
        },
     
        "columns":
            [
                {
                    "data": "ApprovalGroupId",
                    "bVisible": true,
                    "bSortable": false,
                    "autoWidth": false,
                    "bSearchable": false,
                    "mRender": function (data, type, row) {
                        if (row.ChildCount > 0) {
                            return '<img id="' + data + '" src="../../Images/details_open.png" alt="expand/collapse" rel="' + data + '" style="cursor: pointer;"/>';
                        }
                        else {
                            return '';
                        }
                    }
                },
                {
                    "data": "ApprovalGroupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "0",//
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_psearch href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "RequestType", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1", },
                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "AssetGroup1ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3", },
                { "data": "AssetGroup2ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                { "data": "AssetGroup3ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5", }
             
               
            ],
        columnDefs: [
                {
                    targets: [0, 1],
                    className: 'noVis'
                }],

        initComplete: function () {
         
            SetPageLengthMenu();
        }
    });
};
$(document).find('#ApprovalGroupSearch').on('click', 'tbody td img', function (e) {
    var tr = $(this).closest('tr');
    var row = ApprovalGroupTable.row(tr);
    if (this.src.match('details_close')) {
        this.src = "../../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        this.src = "../../Images/details_close.png";
        var ApprovalGroupId = $(this).attr("rel");
        $.ajax({
            url: "/ApprovalGroups/GetApprovalGroupsInnerGrid",
            data: {
                ApprovalGroupId: ApprovalGroupId
            },
            beforeSend: function () {
                ShowLoader();
            },
            dataType: 'html',
            success: function (json) {
                row.child(json).show();
               
                dtinnerGrid = row.child().find('.ApprovalGroupsinnerDataTable').DataTable(
                    {
                        "order": [[0, "asc"]],
                        paging: false,
                        searching: false,
                        "bProcessing": true,
                        responsive: true,
                        scrollY: 300,
                        "scrollCollapse": true,
                        sDom: 'Btlipr',
                        language: {
                            url: "/base/GetDataTableLanguageJson?nGrid=" + true
                        },
                        buttons: [],

                        initComplete: function () {
                            tr.addClass('shown');
                            row.child().find('.dataTables_scroll').addClass('tblchild-scroll');
                            CloseLoader();
                        }
                    });

            }
        });

    }
});
$(document).on('click', '#ApprovalGroupSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#ApprovalGroupSearch_length .searchdt-menu', function () {
    run = true;
});
$(document).on('click', '#ApprovalGroupSearch_wrapper th', function () {
    if ($(this).data('col') !== undefined && $(this).data('col') !== '') {
        run = true;
        order = $(this).data('col');
    }
});


function getfilterinfoarray(txtsearchelement, advsearchcontainer) {
    var filterinfoarray = [];
    var f = new filterinfo('searchstring', LRTrim(txtsearchelement.val()));
    filterinfoarray.push(f);
    advsearchcontainer.find('.adv-item').each(function (index, item) {
        if ($(this).parent('div').is(":visible")) {
            f = new filterinfo($(this).attr('id'), $(this).val());
            filterinfoarray.push(f);
        }
    });
    return filterinfoarray;
}
function setsearchui(data, txtsearchelement, advcountercontainer, searchstringcontainer) {
    var searchitemhtml = '';
    $.each(data, function (index, item) {
        if (item.key == 'searchstring' && item.value) {
            var txtSearchval = item.value;
            if (item.value) {
                txtsearchelement.val(txtSearchval);
                searchitemhtml = "";
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + txtsearchelement.attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
            return false;
        }
        else {
            if ($('#' + item.key).parent('div').is(":visible")) {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }

            //if (item.key == 'advcreatedaterange' && item.value !== '') {
            //    $('#' + item.key).val(item.value);
            //    if (item.value) {
            //        searchitemhtml = searchitemhtml + '<span style="display:none;" class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            //    }
            //}
            //if (item.key == 'advprocessdedbydaterange' && item.value !== '') {
            //    $('#' + item.key).val(item.value);
            //    if (item.value) {
            //        searchitemhtml = searchitemhtml + '<span style="display:none;" class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            //    }
            //}

            //if (item.key == 'Created') {
            //    $("#Created").trigger('change.select2');
            //}
            //if (item.key == 'DateProcessed') {
            //    $("#DateProcessed").trigger('change.select2');
            //}

            //if (item.key == 'Vendor') {
            //    $("#Vendor").trigger('change.select2');
            //}

            //if (item.key == 'Status') {
            //    $("#Status").trigger('change.select2');
            //}

            advcountercontainer.text(selectCount);
            $("#liSelectCount").text(selectCount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);

}
//#endregion

//#region Export
//$(document).on('click', '#liPrint', function () {
//    $(".buttons-print")[0].click();
//    funcCloseExportbtn();
//});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    funcCloseExportbtn();
});
//$(document).on('click', '#liPdf', function () {
//    $(".buttons-pdf")[0].click();
//    funcCloseExportbtn();
//});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liPdf,#liPrint', function () {
    var thisid = $(this).attr('id');
    var TableHaederProp = [];
    function table(property, title) {
        this.property = property;
        this.title = title;
    }
    $("#ApprovalGroupSearch thead tr th").map(function (key) {
        var thisdiv = $(this).find('div');
        if ($(this).parents('.ApprovalGroupsinnerDataTable').length == 0 && thisdiv.html()) {
            if (this.getAttribute('data-th-prop')) {
                var tablearr = new table(this.getAttribute('data-th-prop'), thisdiv.html());
                TableHaederProp.push(tablearr);
            }
        }
    });
  
    var params = {
        tableHaederProps: TableHaederProp,
        colname: order,
        coldir: orderDir,
        RequestType : LRTrim($('#RequestType').val()),
        Description : LRTrim($('#Description').val()),
        ApprovalGroupId : LRTrim($('#ApprovalGroupId').val()),
        AssetGroup1Id : $(document).find('#AssetGroup1Id').val(),
        AssetGroup2Id : $(document).find('#AssetGroup2Id').val(),
        AssetGroup3Id: $(document).find('#AssetGroup3Id').val(),
        SearchText: $(document).find('#txtColumnSearch').val()
    };
    $.ajax({
        "url": "/ApprovalGroups/SetPrintData",
        "data": JSON.stringify({ 'agPrintParams': params }),
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            if (thisid == 'liPdf') {
                window.open('/ApprovalGroups/ExportASPDF?d=PDF', '_self');
            }
            else if (thisid == 'liPrint') {
                window.open('/ApprovalGroups/ExportASPDF', '_blank');
            }

            return;
        }
    });
   /* $('#mask').trigger('click');*/
});
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var jsonResult = $.ajax({
                url: '/ApprovalGroups/GetApprovalGroupsPrintData?page=all',
                data: {
                    colname: order,
                    coldir: orderDir,
                    RequestType: LRTrim($('#RequestType').val()),
                    Description: LRTrim($('#Description').val()),
                    ApprovalGroupId: LRTrim($('#ApprovalGroupId').val()),
                    AssetGroup1Id: $(document).find('#AssetGroup1Id').val(),
                    AssetGroup2Id: $(document).find('#AssetGroup2Id').val(),
                    AssetGroup3Id: $(document).find('#AssetGroup3Id').val(),
                    txtSearchval: $(document).find('#txtColumnSearch').val()
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#ApprovalGroupSearch thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.ApprovalGroupId != null) {
                    item.ApprovalGroupId = item.ApprovalGroupId;
                }
                else {
                    item.ApprovalGroupId = "";
                }
                if (item.RequestType != null) {
                    item.RequestType = item.RequestType;
                }
                else {
                    item.RequestType = "";
                }
                if (item.Description != null) {
                    item.Description = item.Description;
                }
                else {
                    item.Description = "";
                }
                if (item.AssetGroup1ClientLookupId != null) {
                    item.AssetGroup1ClientLookupId = item.AssetGroup1ClientLookupId;
                }
                else {
                    item.AssetGroup1ClientLookupId = "";
                }
                if (item.AssetGroup2ClientLookupId != null) {
                    item.AssetGroup2ClientLookupId = item.AssetGroup2ClientLookupId;
                }
                else {
                    item.AssetGroup2ClientLookupId = "";
                }
                if (item.AssetGroup3ClientLookupId != null) {
                    item.AssetGroup3ClientLookupId = item.AssetGroup3ClientLookupId;
                }
                else {
                    item.AssetGroup3ClientLookupId = "";
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
                header: $("#ApprovalGroupSearch thead tr th").find('div').map(function (key) {
                    if ($(this).parents('.ApprovalGroupsinnerDataTable').length == 0 && this.innerHTML) {
                        return this.innerHTML;
                    }
                }).get()
            };
        }
    });
});
//#endregion
//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(ApprovalGroupTable, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0,1];
    funCustozeSaveBtn(ApprovalGroupTable, colOrder);
    run = true;
    ApprovalGroupTable.state.save(run);
});
//#endregion
//#region Advanced Search
$("#btnApprovalGroupsDataAdvSrch").on('click', function (e) {
    run = true;
    ManIdVal = $("#advstatus").val();
    ApprovalGroupsAdvSearch();
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    ApprovalGroupTable.page('first').draw('page');
});
function ApprovalGroupsAdvSearch() {
    var InactiveFlag = false;
    var searchitemhtml = "";
    $("#txtColumnSearch").val("");
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#liSelectCount').text(selectCount + ' filters applied');
    $("#dvFilterSearchSelect2").html(searchitemhtml);
    $("#spnControlCounter").text(selectCount);
}
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    clearAdvanceSearch();
    ApprovalGroupTable.page('first').draw('page');
});
function clearAdvanceSearch() {
    selectCount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    selectCount = 0;
    $("#AssetGroup1Id").val("").trigger('change');
    $("#AssetGroup2Id").val("").trigger('change');
    $("#AssetGroup3Id").val("").trigger('change');
    //$("#txtColumnSearch").val("");
    $("#spnControlCounter").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
    $('#dvFilterSearchSelect2').find('span').html('');
    $('#dvFilterSearchSelect2').find('span').removeClass('tagTo');
}
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    if (searchtxtId == "RequestType") {
        $("#RequestType").val('');
    }
    if (searchtxtId == "ApprovalGroupId") {
        $("#ApprovalGroupId").val('');
    }
    if (searchtxtId == "Description") {
        $("#Description").val('');
    }
    if (searchtxtId == "AssetGroup1Id") {
        $("#AssetGroup1Id").val('');
    }
    if (searchtxtId == "AssetGroup2Id") {
        $("#AssetGroup2Id").val('');
    }
    if (searchtxtId == "AssetGroup3Id") {
        $("#AssetGroup3Id").val('');
    }
    ApprovalGroupsAdvSearch();
    ApprovalGroupTable.page('first').draw('page');
});
//#endregion Advanced Search


$(document).on('click', '.lnk_psearch', function (e) {
    e.preventDefault();
    //var index_row = $('#ApprovalGroupSearch tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    //var td = $(this).parents('tr').find('td');
    var data = ApprovalGroupTable.row(row).data();
    var ApprovalGroupId = data.ApprovalGroupId;
    $.ajax({
        url: "/ApprovalGroups/ApprovalGroupDetails",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { ApprovalGroupId: ApprovalGroupId },
        success: function (data) {
            $('#renderApprovalGroups').html(data);           
        },
        complete: function () {
            CloseLoader();
            generateAppGroupApprovalTable();
        }
    });
});
function RedirectToAGDetail(AGMasterId, mode) {
    if (AGMasterId == 0) {
        window.location.href = "/ApprovalGroups/Index?page=Approval_Groups";
    }
    else {
        $.ajax({
            url: "/ApprovalGroups/ApprovalGroupDetails",
            type: "GET",
            dataType: "html",
            data: { ApprovalGroupId: AGMasterId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#renderApprovalGroups').html(data);
            },
            complete: function () {
                CloseLoader();
                SetAGControls();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
}
//#region Add/Edit
$(document).on('click', ".addNewApprovalGroup", function (e) {
    $.ajax({
        url: "/ApprovalGroups/AddEditApprovalGroups",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderApprovalGroups').html(data);
        },
        complete: function () {
            SetAGControls();
        },
        error: function () {
            CloseLoader();
        }
    });
})

$(document).on('click', "#btnEditApprovalGroup", function () {
    debugger
    var approvalgroupid = $(document).find('#ApprovalGroupsModel_ApprovalGroupId').val();
    var requesttype = $(document).find('#ApprovalGroupsModel_RequestType').val();
    var description = $(document).find('#ApprovalGroupsModel_Description').val();
    var assetgroup1 = $(document).find('#ApprovalGroupsModel_AssetGroup1Id').val();
    var assetgroup2 = $(document).find('#ApprovalGroupsModel_AssetGroup2Id').val();
    var assetgroup3 = $(document).find('#ApprovalGroupsModel_AssetGroup3Id').val();
    $.ajax({
        url: "/ApprovalGroups/AddEditApprovalGroups",
        type: "GET",
        dataType: 'html',
        data: { ApprovalGroupId: approvalgroupid, Requesttype: requesttype,Description: description, AssetGroup1: assetgroup1, AssetGroup2: assetgroup2, AssetGroup3: assetgroup3 },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderApprovalGroups').html(data);
        },
        complete: function () {
            SetAGControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});

function SetAGControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if (areaddescribedby) {
            if ($(this).valid()) {
                if (typeof areaddescribedby != 'undefined') {
                    $('#' + areaddescribedby).hide();
                }
            }
            else {
                if (typeof areaddescribedby != 'undefined') {
                    $('#' + areaddescribedby).show();
                }
            }
        }
    });
    $(document).find('.select2picker').select2({});
    
}
function ApprovalGroupsAddEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Command == "save") {
            if (data.mode == "add") {
                SuccessAlertSetting.text = getResourceValue("ApprovalGroupMasterAddAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("ApprovalGroupMasterUpdateAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToAGDetail(data.ApprovalGroupMasterId, data.mode);
            });
        }
        else {
            SuccessAlertSetting.text = getResourceValue("ApprovalGroupMasterAddAlert");
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change');
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("select").removeClass("input-validation-error");
                var areaddescribedby = $(document).find("#ApprovalGroupMasterModel_RequestType").attr('aria-describedby');
                if (typeof areaddescribedby != 'undefined') {
                    $('#' + areaddescribedby).hide();
                }
                $(document).find('form').find("#ApprovalGroupMasterModel_RequestType").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};

$(document).on('click', "#btnCancelAGMaster", function () {
    //var MasterId = $(document).find('#ApprovalGroupMasterModel_ApprovalGroupId').val();
    //if (!MasterId) {
    //    MasterId = 0;
    //}
    //RedirectToAGDetail(MasterId, "");
    var ApprovalGroupId = $('#ApprovalGroupMasterModel_ApprovalGroupId').val();
    if (typeof ApprovalGroupId !== "undefined" && ApprovalGroupId != 0) {
        swal(CancelAlertSetting, function () {
            RedirectToAGDetail(ApprovalGroupId, "");
        });
    }
    else {
        swal(CancelAlertSetting, function () {
            window.location.href = "../ApprovalGroups/Index?page=Approval_Groups";
        });
    }
});
$(document).on('click', "#brdEMdetail", function () {
    var ApprovalGroupId = $(this).data('val');
    RedirectToAGDetail(ApprovalGroupId, "");
});
//#endregion

//#region New Search button
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'ApprovalGroups' },
        beforeSend: function () {
            ShowbtnLoader("SrchBttnNew");
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)" id= "mem_' + i + '"' + '><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchList.innerHTML = str;
            $(document).find('#searchBttnNewDrop').show("slideToggle");
        },
        complete: function () {
            HidebtnLoader("SrchBttnNew");
        },
        error: function () {
            HidebtnLoader("SrchBttnNew");
        }
    });
});
function GenerateSearchList(txtSearchval, isClear) {
    $.ajax({
        url: '/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'ApprovalGroups', searchText: txtSearchval, isClear: isClear },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)"><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchList.innerHTML = str;
        }
        ,
        complete: function () {
            if (isClear == false) {
                ApprovalGroupTable.page('first').draw('page');
                CloseLoader();
            }
            else {
                CloseLoader();
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '#UlSearchList li', function () {
    var v = LRTrim($(this).text());
    $(document).find('#txtColumnSearch').val(v);
    TextSearch();
});
$(document).on('click', '#cancelText', function () {
    $(document).find('#txtColumnSearch').val('');
});
$(document).on('click', '#clearText', function () {
    GenerateSearchList('', true);
});

$(document).on('keyup', '#txtColumnSearch', function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == 13) {
        TextSearch();
    }
    else {
        event.preventDefault();
    }
});
$(document).on('click', '.txtSearchClick', function () {
    TextSearch();
});
function TextSearch() {
    run = true;
    clearAdvanceSearch();
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#dvFilterSearchSelect2").html(searchitemhtml);
    }
    else {
        ApprovalGroupTable.page('first').draw('page');
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
}

//#endregion
//#region Dropdown toggle   
$(document).on('click', "#spnDropToggle", function () {
    $(document).find('#searcharea').show("slide");
});
$(document).mouseup(function (e) {
    var container = $(document).find('#searcharea');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slide");
    }
});
$(document).mouseup(function (e) {
    var container = $(document).find('#searchBttnNewDrop');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slideToggle");
    }
});
//#endregion