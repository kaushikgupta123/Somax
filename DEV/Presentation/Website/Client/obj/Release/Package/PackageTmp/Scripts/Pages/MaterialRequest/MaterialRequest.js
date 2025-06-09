//#region Common
var run = false;
var totalcount = 0;
var CustomQueryDisplayId = 0;

var selectCount = 0;
var gridname = "MaterialRequest_Search";
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}

//#endregion

$(document).ready(function () {

    $(document).on('click', "#action", function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $(document).on('focusout', "#action", function () {
        $(".actionDrop").fadeOut();
    });
    $(".actionBar").fadeIn();
    $("#MaterialRequestGridAction :input").attr("disabled", "disabled");

    var title = localStorage.getItem("mrstatustext");
    if (title) {
        $(document).find('#spnlinkToSearch').text(title);
    }
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
$(document).ready(function () {
    $(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '.dismiss, .overlay', function () {
        $(document).find('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', '#sidebarCollapse', function () {
        $('.sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
        $(document).find('.dtpicker').datepicker({
            changeMonth: true,
            changeYear: true,
            "dateFormat": "mm/dd/yy",
            autoclose: true
        });
    });
    $(document).on('click', '#lineitemsidebarCollapse', function () {
        $('.sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    //#region Load Grid With Status
    var mrcurrentstatus = localStorage.getItem("MATERIALREQUESTSTATUS");
    if (mrcurrentstatus != 'undefined' && mrcurrentstatus != null && mrcurrentstatus != "") {
        CustomQueryDisplayId = mrcurrentstatus;
        $('#mrsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId && $(this).attr('id') != '0') {
                $('#mrsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
    }
    else {
        CustomQueryDisplayId = "1";
        $('#mrsearchtitle').text(getResourceValue("spnAllStatus"));
        $("#mrsearchListul li").first().addClass("activeState");
    }
    //#endregion
    generateMRDataTable()


});
//#region Search
var dtMrTable;
var dtPrShoppingCartTable;
var setFirstPage;
var SelectPRDetails = [];
var SelectPRId = [];
$("#btnMRDataAdvSrch").on('click', function (e) {
    run = true;
    searchresult = [];
    MRAdvSearch();
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    dtMrTable.page('first').draw('page');
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    //MRAdvSearch(dtMrTable.length);
    MRAdvSearch();
    dtMrTable.page('first').draw('page');
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', '#liPdf,#liPrint', function () {
    var thisid = $(this).attr('id');
    var TableHaederProp = [];
    function table(property, title) {
        this.property = property;
        this.title = title;
    }
    $("#materialrequestSearch thead tr th").map(function (key) {
        var thisdiv = $(this).find('div');
        if ($(this).parents('.materialRequestinnerDataTable').length == 0 && thisdiv.html()) {
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
        CustomQueryDisplayId: CustomQueryDisplayId,
        MaterialRequestId: LRTrim($("#MaterialRequestId").val()),
        Description: LRTrim($("#Description").val()),
        AccountClientLookupId: LRTrim($("#AccountId").val()),
        Status: LRTrim($("#Status").val()),
        CreateDate: ValidateDate($("#Created").val()),
        RequiredDate: ValidateDate($("#RequiredDate").val()),
        CompleteDate: ValidateDate($("#Completed").val()),
        txtsearchval: LRTrim($("#txtColumnSearch").val())
    };
    mrPrintParams = JSON.stringify({ 'mrPrintParams': params });
    $.ajax({
        "url": "/MaterialRequest/SetPrintData",
        "data": mrPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            if (thisid == 'liPdf') {
                window.open('/MaterialRequest/ExportASPDF?d=PDF', '_self');
            }
            else if (thisid == 'liPrint') {
                window.open('/MaterialRequest/ExportASPDF', '_blank');
            }

            return;
        }
    });
    $('#mask').trigger('click');
});
function MRAdvSearch() {
    var searchitemhtml = "";
    $(document).find('#txtColumnSearch').val('');
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).attr('id') == 'advcreatedaterange' || $(this).attr('id') == 'advprocessdedbydaterange') {
            if ($(this).val()) {
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }
        else {
            if ($(this).val()) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }

    });



    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#advsearchfilteritems").html(searchitemhtml);
    
    $(".filteritemcount").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}
function clearAdvanceSearch() {
    selectCount = 0;
    $("#MaterialRequestId").val("");
    $("#Description").val("");
    $("#Status").val("").trigger('change');
    $("#Created").val("");
    $('#AccountId').val("").trigger('change');
    $("#RequiredDate").val("");
    $("#Completed").val("");
    $("#advsearchfilteritems").html('');
    $(".filteritemcount").text(selectCount);

}
var titleArray = [];
var classNameArray = [];
var order = '1';
var orderDir = 'asc';
function generateMRDataTable() {
    if ($(document).find('#materialrequestSearch').hasClass('dataTable')) {
        dtMrTable.destroy();
    }
    dtMrTable = $("#materialrequestSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 3
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[2, "asc"]],
        stateSave: true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }
                var filterinfoarray = getfilterinfoarraymr($("#txtColumnSearch"), $('#advsearchsidebar'));
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
                            setsearchuimr(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
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
        //    leftColumns: 3,
        //},
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Material Request List'
            },
            {
                extend: 'print',
                title: 'Material Request List'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Material Request List',
                extension: '.csv',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                orientation: 'landscape',
                pageSize: 'A3',
                title: 'Material Request List'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/MaterialRequest/GetMRGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.CustomQueryDisplayId = CustomQueryDisplayId;
                
                d.MaterialRequestId = LRTrim($("#MaterialRequestId").val());
                d.Description = LRTrim($("#Description").val());
                d.AccountClientLookupId = LRTrim($("#AccountId").val());
                d.Status = LRTrim($("#Status").val());
                d.CreateDate = ValidateDate($("#Created").val());
                d.RequiredDate = ValidateDate($("#RequiredDate").val());
                d.CompleteDate = ValidateDate($("#Completed").val());
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.Order = order;
                //d.orderDir = orderDir;
            },
            "dataSrc": function (result) {
                let colOrder = dtMrTable.order();
                orderDir = colOrder[0][1];

                var i = 0;
                //SetMultiSelectAction(CustomQueryDisplayId);
                totalcount = result.recordsTotal;

                //HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                if (result.data.length == "0") {
                    $(document).find('.import-export').attr('disabled', 'disabled');
                }
                else {
                    $(document).find('.import-export').removeAttr('disabled');
                }

                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "MaterialRequestId",
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
                    "data": "MaterialRequestId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "1",//
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_psearch href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "RequiredDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                { "data": "Account_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5", /*"name": "3",*/
                    mRender: function (data, type, full, meta) {
                        //if (data == statusCode.Approved) {
                        //    return "<span class='m-badge m-badge-grid-cell m-badge--yellow m-badge--wide'>" + getStatusValue(data) + "</span >";
                        //}
                        //else if (data == statusCode.Canceled) {
                        //    return "<span class='m-badge m-badge-grid-cell m-badge--orange m-badge--wide'>" + getStatusValue(data) + "</span >";
                        //}
                        //else
                            if (data == statusCode.Complete) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        //else if (data == statusCode.Denied) {
                        //    return "<span class='m-badge m-badge-grid-cell m-badge--red m-badge--wide'>" + getStatusValue(data) + "</span >";
                        //}
                        else if (data == statusCode.Open) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        //else if (data == statusCode.AwaitApproval) {
                        //    return "<span class='m-badge m-badge-grid-cell m-badge--light-blue m-badge--wide' style='width:105px;' >" + getStatusValue(data) + "</span >";
                        //}
                        //else if (data == statusCode.Order) {
                        //    return "<span class='m-badge m-badge-grid-cell m-badge--teal m-badge--wide'>" + getStatusValue(data) + "</span >";
                        //}
                        //else if (data == statusCode.Resubmit) {
                        //    return "<span class='m-badge m-badge-grid-cell m-badge--purple m-badge--wide'>" + getStatusValue(data) + "</span >";
                        //}
                        //else if (data == statusCode.Extracted) {
                        //    return "<span class='m-badge m-badge-grid-cell m-badge--grey m-badge--wide'>" + getStatusValue(data) + "</span >";
                        //}
                        else {
                            return getStatusValue(data);
                        }
                    }
                },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date ", "name": "6" }, //--previous name : 6
                { "data": "CompleteDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7" } //--previous name : 9
            ],
        columnDefs: [
            {
                targets: [0, 1, 2],
                className: 'noVis'
            }
        ],
        initComplete: function (settings, json) {
            SetPageLengthMenu();
            //----------conditional column hiding-------------//
            //var api = new $.fn.dataTable.Api(settings);
            //var columns = dtMrTable.settings().init().columns;
            //var arr = [];
            //var j = 0;
            //while (j < json.hiddenColumnList.length) {
            //    var clsname = '.' + json.hiddenColumnList[j];
            //    var title = dtMrTable.columns(clsname).header();
            //    titleArray.push(title[0].innerHTML);
            //    classNameArray.push(clsname);
            //    dtMrTable.columns(clsname).visible(false);
            //    var sortMenuItem = '.dropdown-menu' + ' ' + clsname;
            //    $(sortMenuItem).remove();

            //    //---hide adv search items---
            //    var advclsname = '.' + "prc-" + json.hiddenColumnList[j];
            //    $(document).find(advclsname).hide();
            //    j++;
            //}
            //----------------------------------------------//

            $("#MaterialRequestGridAction :input").removeAttr("disabled");
            $("#MaterialRequestGridAction :button").removeClass("disabled");
            DisableExportButton($("#materialrequestSearch"), $(document).find('.import-export'));
        }
    });
}
$('#materialrequestSearch').find('th').click(function () {
    if ($(this).data('col')) {
        run = true;
        order = $(this).data('col');
    }

});


$(document).find('#materialrequestSearch').on('click', 'tbody td img', function (e) {
    var tr = $(this).closest('tr');
    var row = dtMrTable.row(tr);
    if (this.src.match('details_close')) {
        this.src = "../../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        this.src = "../../Images/details_close.png";
        var MaterialRequestID = $(this).attr("rel");
        $.ajax({
            url: "/MaterialRequest/GetMRInnerGrid",
            data: {
                MaterialRequestID: MaterialRequestID
            },
            beforeSend: function () {
                ShowLoader();
            },
            dataType: 'html',
            success: function (json) {
                row.child(json).show();
                dtinnerGrid = row.child().find('.materialRequestinnerDataTable').DataTable(
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
                        "columnDefs": [
                            { className: 'text-right', targets: [2, 3, 4] },
                            {
                                "render": function (data, type, row) {
                                    return "<div class='text-wrap'>" + data + "</div>";
                                }
                                , targets: [2]
                            }
                        ],
                        "footerCallback": function (row, data, start, end, display) {
                            var api = this.api(),
                                // Total over all pages
                                total = api.column(4).data().reduce(function (a, b) {
                                    return parseFloat(a) + parseFloat(b);
                                }, 0);
                            // Update footer
                            $(api.column(4).footer()).html(total.toFixed(2));
                        },
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
$(document).on('click', '#materialrequestSearch_paginate .paginate_button', function () {
    MRAdvSearch();
    run = true;
});
$(document).on('change', '#materialrequestSearch_length .searchdt-menu', function () {
    MRAdvSearch();
    run = true;
});

$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var MaterialRequestId = LRTrim($("#MaterialRequestId").val());
            var Description = LRTrim($("#Description").val());
            var Status = LRTrim($("#Status").val());
            var CreateDate = ValidateDate($("#Created").val());
            var RequiredDate = ValidateDate($("#RequiredDate").val());
            var CompleteDate = ValidateDate($("#Completed").val());
            var AccountClientLookupId = LRTrim($("#AccountId").val());
            var colname = order;
            var coldir = orderDir;

            var txtsearchval = LRTrim($("#txtColumnSearch").val());
            var jsonResult = $.ajax({
                url: '/MaterialRequest/GetMRPrintData?page=all',
                data: {
                    CustomQueryDisplayId: CustomQueryDisplayId,
                    MaterialRequestId: MaterialRequestId,
                    Description: Description,
                    CreateDate: CreateDate,
                    RequiredDate: RequiredDate,
                    CompleteDate: CompleteDate,
                    AccountClientLookupId: AccountClientLookupId,
                    Status: Status,
                    colname: colname,
                    coldir: coldir,
                    SearchText: txtsearchval
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#materialrequestSearch thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.MaterialRequestId != null) {
                    item.MaterialRequestId = item.MaterialRequestId;
                }
                else {
                    item.MaterialRequestId = "";
                }
                if (item.Description != null) {
                    item.Description = item.Description;
                }
                else {
                    item.Description = "";
                }
                if (item.RequiredDate != null) {
                    item.RequiredDate = item.RequiredDate;
                }
                else {
                    item.RequiredDate = "";
                }
                if (item.Account_ClientLookupId != null) {
                    item.Account_ClientLookupId = item.Account_ClientLookupId;
                }
                else {
                    item.Account_ClientLookupId = "";
                }
                if (item.Status != null) {
                    item.Status = item.Status;
                }
                else {
                    item.Status = "";
                }
                if (item.CreateDate != null) {
                    item.CreateDate = item.CreateDate;
                }
                else {
                    item.CreateDate = "";
                }
                if (item.CompleteDate != null) {
                    item.CompleteDate = item.CompleteDate;
                }
                else {
                    item.CompleteDate = "";
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
                header: $("#materialrequestSearch thead tr th").find('div').map(function (key) {
                    if ($(this).parents('.materialRequestinnerDataTable').length == 0 && this.innerHTML) {
                        return this.innerHTML;
                    }
                }).get()
            };
        }
    });
});
$(document).on('click', '.lnk_psearch', function (e) {
    e.preventDefault();
    var index_row = $('#materialrequestSearch tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtMrTable.row(row).data();
    var MRequestId = data.MaterialRequestId;
    var titletext = $('#mrsearchtitle').text();
    localStorage.setItem("mrstatustext", titletext);
    $.ajax({
        url: "/MaterialRequest/MaterialRequestDetails",
        type: "POST",
        data: { MaterialRequestId: MRequestId },
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermaterialrequest').html(data);
            $(document).find('#spnlinkToSearch').text(titletext);
            SetFixedHeadStyle();
            //if ($(document).find('.AddMrequest').length === 0) { $(document).find('#prdetailactiondiv').css('margin-right', '0px'); }
        },
        complete: function () {
            var dt = $("#tblLineItem").DataTable();
            dt.state.clear();
            dt.destroy();
            generateLineiItemdataTable(MRequestId);
            SetMRControls();
            CloseLoader();
        }
    });
});

$(document).on('click', '#mrreaddescription', function () {
    $(document).find('#mrdetaildesmodaltext').text($(this).data("des"));
    $(document).find('#mrdetaildesmodal').modal('show');
});

//#endregion


//#region common
function SetMRControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var validstate = "";
        try {
            validstate = $(this).valid();
        } catch (e) {
            validstate = undefined;
        }
        var areaddescribedby = $(this).attr('aria-describedby');
        if (areaddescribedby) {
            if (validstate) {
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
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');

    $(document).find('.dtpickernextseven').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true,
        minDate: '0',
    }).inputmask('mm/dd/yyyy');
    SetFixedHeadStyle();
}
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../MaterialRequest/Index?page=Inventory_MaterialRequests";
});

function RedirectToMRequestDetail(MaterialRequestId, mode) {
    $.ajax({
        url: "/MaterialRequest/MaterialRequestDetails",
        type: "POST",
        dataType: "html",
        data: { MaterialRequestId: MaterialRequestId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermaterialrequest').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("mrstatustext"));
            //if ($(document).find('.AddMrequest').length === 0) { $(document).find('#prdetailactiondiv').css('margin-right', '0px'); }
        },
        complete: function () {
            CloseLoader();
            generateLineiItemdataTable(MaterialRequestId);
            SetMRControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', '#brdmrlineitem', function () {
    var MaterialRequestId = $(this).attr('data-val');
    //PartNotInInventorySelectedItemArray = [];
    RedirectToMRequestDetail(MaterialRequestId);
});
function RedirectToDetailOncancel(MaterialRequestId) {
    swal(CancelAlertSetting, function () {
        RedirectToMRequestDetail(MaterialRequestId);
    });
}
$(document).on('click', '.ClearAccountClientLookupIdModalPopupGridData', function () {
    $(document).find('#' + $(this).data('textfield')).val('');
    $(document).find('#' + $(this).data('valuefield')).val('');

    //if ($(document).find('#' + $(this).data('textfield')).css('display') == 'block') {
    //    $(document).find('#' + $(this).data('textfield')).css('display', 'none');
    //}
    //if ($(document).find('#' + $(this).data('valuefield')).css('display') == 'none') {
    //    $(document).find('#' + $(this).data('valuefield')).css('display', 'block');
    //}
    $(this).css('display', 'none');
});
$(document).on('click', '.link_Account_detail', function (e) {
    $(document).find('#' + ValueField).parent().find('div > button.ClearAccountClientLookupIdModalPopupGridData').css('display', 'block');
});
//#endregion


//#region ColumnVisibility

$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtMrTable, true, titleArray);
    //funPrCustomizeBtnClick(dtMrTable,null,titleArray);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0, 1, 2];
    funCustozeSaveBtn(dtMrTable, colOrder);
    run = true;
    dtMrTable.state.save(run);
    if (classNameArray != null && classNameArray.length > 0) {
        var j = 0;
        while (j < classNameArray.length) {
            dtMrTable.columns(classNameArray[j]).visible(false);
            j++;
        }
    }
});
$(document).on('click', '.mrsearchdrpbox', function (e) {
    $(document).find('#txtColumnSearch').val('');
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $('.itemcount').text(0);
    SelectPRDetails = [];
    SelectPRId = [];
    run = true;
    var val = localStorage.getItem("MATERIALREQUESTSTATUS");

    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("MATERIALREQUESTSTATUS", optionval);
    CustomQueryDisplayId = optionval;
    $(document).find('#mrsearchtitle').text($(this).text());
    ShowbtnLoaderclass("LoaderDrop");
    MRAdvSearch();
    dtMrTable.page('first').draw('page');
});
$(document).on('keyup', '#mrsearctxtbox', function (e) {
    var tagElems = $(document).find('#mrsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
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
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        dtMrTable.page('first').draw('page');
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

//#region New Search button
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'MaterialRequest' },
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
        data: { tableName: 'MaterialRequest', searchText: txtSearchval, isClear: isClear },
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
                dtMrTable.page('first').draw('page');
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
//#endregion



//#region V2-389
function getfilterinfoarraymr(txtsearchelement, advsearchcontainer) {
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
function setsearchuimr(data, txtsearchelement, advcountercontainer, searchstringcontainer) {
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
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);

}



//#endregion

//#region Add-Edit
$(document).find('.select2picker').select2({});
$.validator.setDefaults({ ignore: null });
$(document).on('click', ".AddMrequest", function (e) {
    $.ajax({
        url: "/MaterialRequest/AddMaterialRequest",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermaterialrequest').html(data);
        },
        complete: function () {
            SetMRControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#materialrequestedit", function (e) {
    e.preventDefault();
    var MaterialRequestId = $('#MaterialRequestModel_MaterialRequestId').val();
    $.ajax({
        url: "/MaterialRequest/EditMaterialRequest",
        type: "GET",
        dataType: 'html',
        data: { MaterialRequestId: MaterialRequestId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermaterialrequest').html(data);
            SetFixedHeadStyle();
        },
        complete: function () {
            SetMRControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btnCancelAddMaterialRequest", function () {
    var MaterialRequestId = $('#MaterialRequestModel_MaterialRequestId').val();
    if (typeof MaterialRequestId !== "undefined" && MaterialRequestId != 0) {
        swal(CancelAlertSetting, function () {
            RedirectToMRequestDetail(MaterialRequestId);
        });
    }
    else {
        swal(CancelAlertSetting, function () {
            window.location.href = "../MaterialRequest/Index?page=Inventory_MaterialRequests";
        });
    }
});
$(document).on('click', "#brdmaterialrequest", function () {
    var MaterialRequestId = $(this).attr('data-val');
    RedirectToMRequestDetail(MaterialRequestId)
});
function MaterialRequestAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        //var message;
        if (data.Command == "save") {
            if (data.mode == "add") {
                localStorage.setItem("MATERIALREQUESTSTATUS", '1')
                localStorage.setItem("mrstatustext", getResourceValue("spnAllStatus"));
                SuccessAlertSetting.text = getResourceValue("MaterialRequestAddedAlert");

            }
            else {
                SuccessAlertSetting.text = getResourceValue("MaterialRequestUpdatedAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToMRequestDetail(data.MaterialRequestId);
            });
        }
        else {
            SuccessAlertSetting.text = getResourceValue("MaterialRequestAddedAlert");
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
                $(document).find('form').find(".ClearAccountModalPopupGridData").css("display", "none");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};

//#endregion

//#region ChildGrid

var dtLineItemTable;
var lItemfilteritemcount = 0;
var typeValPoLiStatus;
//$(document).on('click', "#btnLitemSearch", function () {
//    var PurchaseOrderId = $(document).find('#MaterialRequestModel_MaterialRequestId').val();
//    clearLineItemAdvanceSearch();
//    dtLineItemTable.state.clear();
//    var searchText = LRTrim($('#txtsearchbox').val());
//    typeValPoLiStatus = $("#PoLiStatus").val();
//    generateLineiItemdataTable(PurchaseOrderId, searchText);
//});
$(document).on('click', '#lineitemClearAdvSearchFilter', function () {
    var MaterialRequestId = $(document).find('#MaterialRequestModel_MaterialRequestId').val();
    $("#txtsearchbox").val("");
    clearLineItemAdvanceSearch();
    generateLineiItemdataTable(MaterialRequestId);
});
$(document).on('click', "#btnLItemDataAdvSrch", function () {
    dtLineItemTable.state.clear();
    var searchitemhtml = "";
    filteritemcount = 0;
    lItemfilteritemcount = 0;
    $('#txtsearchbox').val('');
    $('#litemadvsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            lItemfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times litembtnCross" aria-hidden="true"></a></span>';
        }
    });
    $('#lineitemadvsearchfilteritems').html(searchitemhtml);
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    LineItemAdvanceSearch();
    $('.lifilteritemcount').text(lItemfilteritemcount);
});
function clearLineItemAdvanceSearch() {
    var filteritemcount = 0;
    $('#litemadvsearchsidebar').find('input:text').val('');
    $('.lifilteritemcount').text(filteritemcount);
    $('#lineitemadvsearchfilteritems').html('');
}
function LineItemAdvanceSearch() {
    var MaterialRequestId = $(document).find('#MaterialRequestModel_MaterialRequestId').val();
    generateLineiItemdataTable(MaterialRequestId);
}
$(document).on('click', '.litembtnCross', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId + '.adv-item').val('').trigger('change');
    $(this).parent().remove();
    lItemfilteritemcount--;
    LineItemAdvanceSearch();
    $('.lifilteritemcount').text(lItemfilteritemcount);

});
function generateLineiItemdataTable(MaterialRequestId) {
    var rCount = 0;
    var initiated = false;
    var PartId = $("#PartID").val();
    if (PartId) {
        PartId = LRTrim(PartId);
    }
    var Description = $("#Description").val();
    if (Description) {
        Description = LRTrim(Description);
    }
    var Quantity = $("#Quantity").val();
    if (Quantity) {
        Quantity = LRTrim(Quantity);
    }
    var PurchaseUOM = $('#UOM').val();
    if (PurchaseUOM) {
        PurchaseUOM = LRTrim(PurchaseUOM);
    }
    var UnitCost = $("#UnitCost").val();
    if (UnitCost) {
        UnitCost = LRTrim(UnitCost);
    }
    var TotalCost = $("input[id='TotalCost']").val();
    if (TotalCost) {
        TotalCost = LRTrim(TotalCost);
    }
    if ($(document).find('#tblLineItem').hasClass('dataTable')) {
        dtLineItemTable.destroy();
    }
    dtLineItemTable = $("#tblLineItem").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "bProcessing": true,
        "order": [[0, "asc"]],
        stateSave: false,
        "pagingType": "full_numbers",
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/MaterialRequest/PopulateMaterialRequestDetailsGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.MaterialRequestId = MaterialRequestId;
                //d.searchText = searchtext;
                d.PartId = PartId;
                d.Description = Description;
                d.Quantity = Quantity;
                d.UnitCost = UnitCost;
                d.TotalCost = TotalCost;
            },
            "dataSrc": function (result) {
                rCount = result.data.length;
                initiated = result.IsInitiated;
                IsShoppingCart = result.IsShoppingCart;
                return result.data;
            },
            global: true
        },
        columnDefs: [
            {
                "data": null,
                targets: [6], render: function (a, b, data, d) {
                    var ismaterialreq = $("#ApprovalRouteModel_IsMaterialRequest").val();
                    if (data.Status == "Initiated" && ismaterialreq == "True") {
                        return '<a data-toggle="modal" href="#AddLineItems" class="btn btn-outline-primary addMRLineItemBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' + '<a class="btn btn-outline-success editMRLineItemBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delMRLineItemBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else if (data.Status == "Approved" && ismaterialreq=="False") {
                        return '<a data-toggle="modal" href="#AddLineItems" class="btn btn-outline-primary addMRLineItemBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' + '<a class="btn btn-outline-success editMRLineItemBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delMRLineItemBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-left" },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }

            ],
        initComplete: function (settings, json) {
            //var column = this.api().column(10);
            //var api = new $.fn.dataTable.Api(settings);
            //api.column(8).visible(IsShoppingCart);
            if (rCount > 0) {
                $("#addLineItem").hide();
                if (initiated) {
                    $("#sendmaterialrequestitemsforapproval").show();
                }
                else {
                    $("#sendmaterialrequestitemsforapproval").hide();
                    $("#addLineItem").show();
                }
            }
            else {
                $("#addLineItem").show();
            }
            SetPageLengthMenu();
            //----------conditional column hiding-------------//
            //var api = new $.fn.dataTable.Api(settings);
            //var columns = dtLineItemTable.settings().init().columns;
            //var arr = [];
            //var j = 0;
            //while (j < json.hiddenColumnList.length) {
            //    var clsname = '.G' + json.hiddenColumnList[j];
            //    dtLineItemTable.columns(clsname).visible(false);
            //    //---hide adv search items---
            //    var advclsname = '.' + "prcli-" + json.hiddenColumnList[j];
            //    $(document).find(advclsname).hide();
            //    j++;
            //}
            //----------------------------------------------//
        }
    });
}
//#endregion

//#region Add Part Not In Inventory
$(document).on('click', "#selectidpartnotininventory", function (e) {
    e.preventDefault();
    var MaterialRequestId = $(document).find('#MaterialRequestModel_MaterialRequestId').val();
    $('.modal-backdrop').remove();
    GoToAddPartNotInInventory(MaterialRequestId);
});

function GoToAddPartNotInInventory(MaterialRequestId) {
    $.ajax({
        url: "/MaterialRequest/AddNonPartInInventory",
        type: "GET",
        dataType: 'html',
        data: { MaterialRequestId: MaterialRequestId},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            //$('#rendermaterialrequest').html(data);
            $('#AddLineItems').hide();
            $('#PartNotInInventoryPopUp').html(data);
            $('#AddPartNotInInventoryModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetMRControls();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}

$(document).on('click', "#btncan", function () {
    var MaterialRequestId = $(document).find('#PartNotInInventoryModel_ObjectId').val();
    RedirectToDetailOncancel(MaterialRequestId);
});

function AddPartNonInInventoryOnSuccess(data) {
    CloseLoader();
    var MaterialRequestId = data.MaterialRequestId;
    if (data.Result == "success") {
        $(document).find('#AddPartNotInInventoryModalpopup').modal("hide");
        SuccessAlertSetting.text = getResourceValue("MaterialRequestItemAddedAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToMRequestDetail(MaterialRequestId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}


$(document).on('click', '.editMRLineItemBttn', function () {
    var data = dtLineItemTable.row($(this).parents('tr')).data();
    if (data.CategoryId > 0) {
        EditMRLineItemPartInInventory(data.EstimatedCostsId);
    }
    else {
        EditMRLineItemPartNotInInventory(data.EstimatedCostsId);
    }
});
function EditMRLineItemPartNotInInventory(EstimatedCostsId) {
    var MaterialRequestId = $(document).find('#MaterialRequestModel_MaterialRequestId').val();
    $.ajax({
        url: "/MaterialRequest/EditPartNotInInventory",
        type: "GET",
        dataType: 'html',
        data: { EstimatedCostsId: EstimatedCostsId, MaterialRequestId: MaterialRequestId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            //$('#rendermaterialrequest').html(data);
            $('#PartNotInInventoryPopUp').html(data);
            $('#AddPartNotInInventoryModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetMRControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}


function EditMRLineItemPartInInventory(EstimatedCostsId) {
    var MaterialRequestId = $(document).find('#MaterialRequestModel_MaterialRequestId').val();
    $.ajax({
        url: "/MaterialRequest/EditPartInInventory",
        type: "GET",
        dataType: 'html',
        data: { EstimatedCostsId: EstimatedCostsId, MaterialRequestId: MaterialRequestId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            //$('#rendermaterialrequest').html(data);
            $('#PartNotInInventoryPopUp').html(data);
            $('#AddPartNotInInventoryModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetMRControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', '.clearstate', function () {
    $(document).find('#AddPartNotInInventoryModalpopup select').each(function (i, item) {
        $('#' + $(document).find("#" + item.getAttribute('id')).attr('aria-describedby')).hide();
    });
})
function EditLineItemOnSuccess(data) {
    CloseLoader();
    var MaterialRequestId = data.MaterialRequestId;
    if (data.Result == "success") {
        $(document).find('#AddPartNotInInventoryModalpopup').modal("hide");
        SuccessAlertSetting.text = getResourceValue("MaterialRequestItemUpdatedAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToMRequestDetail(MaterialRequestId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

//$(document).on('click', "#btnprlineitemcancel", function () {
//    var MaterialRequestId = $(document).find('#PartNotInInventoryModel_ObjectId').val();
//    RedirectToDetailOncancel(MaterialRequestId);
//});

$(document).on('click', '.delMRLineItemBttn', function () {

    var data = dtLineItemTable.row($(this).parents('tr')).data();
    var MaterialRequestId = $(document).find('#MaterialRequestModel_MaterialRequestId').val();
    var EstimatedCostsId = data.EstimatedCostsId;
    //var Status = $(document).find('#purchaseRequestModel_Status').val();
    DeleteMRLineItem(EstimatedCostsId, MaterialRequestId)
});
function DeleteMRLineItem(EstimatedCostsId, MaterialRequestId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: "/MaterialRequest/DeleteLineItem",
            type: "GET",
            dataType: 'json',
            data: { EstimatedCostsId: EstimatedCostsId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                var message = "";
                if (data.Result == "success") {
                    SuccessAlertSetting.text = getResourceValue("MaterialRequestItemDeletedAlert");
                    swal(SuccessAlertSetting, function () {
                        if (data.Result == "success") {
                            RedirectToMRequestDetail(MaterialRequestId);
                        }
                    });
                }
                else {
                    message = getResourceValue("LineItemDeleteFailedAlert");
                    swal({
                        title: getResourceValue("CommonErrorAlert"),
                        text: message,
                        type: "warning",
                        showCancelButton: false,
                        confirmButtonClass: "btn-sm btn-primary",
                        cancelButtonClass: "btn-sm",
                        confirmButtonText: getResourceValue("SaveAlertOk"),
                        cancelButtonText: getResourceValue("CancelAlertNo")
                    }, function () {
                    });
                }
            },
            complete: function () {
                CloseLoader();
            },
            error: function (jqXHR, exception) {
                CloseLoader();
            }
        });
    });
}
//#endregion

//#region Part In Inventory

$(document).on('click', "#selectidpartininventory", function (e) {
    e.preventDefault();
    var MaterialRequestId = $(document).find('#MaterialRequestModel_MaterialRequestId').val();
    $('.modal-backdrop').remove();
    if ($(document).find('#MaterialRequestModel_IsUseMultiStoreroom').val()=="True") {
        PopulateStorerooms();
    }
    else {
        GoToAddPartInInventory(MaterialRequestId);
    }
});
function GoToAddPartInInventory(MaterialRequestId) {
    var vendorId = 0;
    var StoreroomId = 0;
    if ($(document).find('#StoreroomId').val() != "undefined" && $(document).find('#StoreroomId').val() > 0) {
        StoreroomId = $(document).find('#StoreroomId').val();
    }
    $.ajax({
        url: "/MaterialRequest/AddPartInInventory",
        type: "POST",
        dataType: "html",
        data: { MaterialRequestId: MaterialRequestId, vendorId: vendorId, StoreroomId: StoreroomId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendermaterialrequest').html(data);
            SetFixedHeadStyle();
        },
        complete: function () {
            CloseLoader();
            ShowCardViewWO();
            //GenerateSelectPartsGrid();
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion
//#region V2-726

//$(document).find('.select2picker').select2({});
//$.validator.setDefaults({ ignore: null });
$(document).on('click', "#sendmaterialrequestitemsforapproval", function (e) {
    e.preventDefault();
    //var MaterialRequestId = $(document).find('#MaterialRequestModel_MaterialRequestId').val();
    $('.modal-backdrop').remove();
    GoToSendForApproval();
});

function GoToSendForApproval() {
    var MaterialRequestId = $(document).find('#MaterialRequestModel_MaterialRequestId').val();
    $.ajax({
        url: "/MaterialRequest/SendForApproval",
        type: "GET",
        dataType: 'html',
        data: { MaterialRequestId: MaterialRequestId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            //$('#rendermaterialrequest').html(data);
            //$('#AddLineItems').hide();
            $('#SendForApprovalPopup').html(data);
            $('#SendForApprovalModalPopup').modal({ backdrop: 'static', keyboard: false, show: true });
            if ($('#ApprovalRouteModel_ApproverCount').val() != 1) {
                $('#Approver').val(null).trigger("change.select2");
            }
        },
        complete: function () {
            SetMRControls();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}

function SendForApprovalOnSuccess(data) {
    $(document).find('#SendForApprovalModalPopup').modal('hide');
    var MaterialRequestId = data.MaterialRequestId;
    if (data.data === "success") {
        if (data.ApprovalGroupId >= 0) {
            SuccessAlertSetting.text = getResourceValue("SendApprovalSuccessAlert");
            swal(SuccessAlertSetting, function () {
                CloseLoader();
                RedirectToMRequestDetail(MaterialRequestId);
            });
        }
        //else {
        //    ErrorAlertSetting.text = "You have been not assigned any Approval Group";
        //    swal(ErrorAlertSetting, function () {
        //        CloseLoader();
        //    });
        //}
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
    //$('#Approver').val(null).trigger("change.select2");

}
$(document).on('click', '#btncancelsendForApproval,.clearstate', function () {
    var areaChargeToId = "";
    $(document).find('#SendForApprovalModalPopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });

});
//#endregion

//#region V2-732
function PopulateStorerooms() {
    $(document).find('#AddLineItems').modal('hide');
    $.ajax({
        url: "/MaterialRequest/PopulateStorerooms",
        type: "GET",
        dataType: 'html',
        //data: { MaterialRequestId: MaterialRequestId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#StoreroomListPopUp').html(data);
            $('#StoreroomListModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetMRControls();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function SelctStoreroomOnSuccess(data) {
    var MaterialRequestId = $(document).find('#MaterialRequestModel_MaterialRequestId').val();
    $('.modal-backdrop').remove();
    if (data.data === "success") {
        $(document).find('#StoreroomListModalpopup').hide();
        GoToAddPartInInventory(MaterialRequestId);
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#btnSubmitStoreroomcancel,.clearstate', function () {
    var areaChargeToId = "";
    $(document).find('#StoreroomListModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });

});
//Endregion