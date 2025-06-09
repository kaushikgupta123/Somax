
//#region Commonn 
var dtVendorRequest;
var selectCount = 0;
var gridname = "VendorRequests_Search";
var activeStatus;
var run = false;
//#endregion
//#region Search
$(document).ready(function () {
    $("#ActionGridBar :input").attr("disabled", "disabled");
    ShowbtnLoader("btnsortmenu");
    ShowbtnLoaderclass("LoaderDrop");
    $(document).on('click', '#sidebarCollapse', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    $("#btnVendorDataAdvSrch").on('click', function (e) {
        run = true;
        $("#txtVendorDataSrch").val('');
        AWBAdvSearch();
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();

        dtVendorRequest.page('first').draw('page');
    });
    //#region Load Grid With Status
    var displayState = localStorage.getItem("VENDORREQUESTSEARCHGRIDDISPLAYSTATUS");
    if (displayState) {
        activeStatus = displayState;
        if (activeStatus == 1) {
            $(document).find('#vendorsearchtitle').text($('#vendorsearchListul').find('li').eq(0).text());
            $("#vendorsearchListul li").eq(0).addClass('activeState');
        }
        else if (activeStatus == 2) {
            $(document).find('#vendorsearchtitle').text($('#vendorsearchListul').find('li').eq(1).text());
            $("#vendorsearchListul li").eq(1).addClass('activeState');
        }
        else if (activeStatus == 3) {
            $(document).find('#vendorsearchtitle').text($('#vendorsearchListul').find('li').eq(2).text());
            $("#vendorsearchListul li").eq(2).addClass('activeState');
        }
    }
    else {
        activeStatus = 1;
        $(document).find('#vendorsearchtitle').text($('#vendorsearchListul').find('li').eq(0).text());
        $("#vendorsearchListul li").eq(0).addClass('activeState');
    }
    $('#advsearchsidebar').find('select').val("").trigger('change');
    generateVendorRequestTable();
    //#endregion

    $(document).on('click', '#liPdf', function () {
        $(".buttons-pdf")[0].click();
        $('#mask').trigger('click');
    });
    $(document).on('click', '#liCsv', function () {
        $(".buttons-csv")[0].click();
        $('#mask').trigger('click');
    });
    $(document).on('click', "#liExcel", function () {
        $(".buttons-excel")[0].click();
        $('#mask').trigger('click');
    });
    $(document).on('click', '#liPrint', function () {
        $(".buttons-print")[0].click();
        $('#mask').trigger('click');
    });
    
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    if (searchtxtId == "ddlType") {
        $(document).find("#ddlType").val("").trigger('change.select2');
    }
    AWBAdvSearch();
    dtVendorRequest.page('first').draw('page');
});
var titleArray = [];
var classNameArray = [];
var order = '0';//Vendor rq Sorting
var orderDir = 'asc';//Vendor rq Sorting
var IsVendorRequestCreateAccessSecurity = "False";
var IsVendorRequestApprovalAccessSecurity ="False";
function generateVendorRequestTable() {
    IsVendorRequestCreateAccessSecurity = $("#VendorRequestCreateAccess").val();
    IsVendorRequestApprovalAccessSecurity = $("#VendorRequestApprovalAccess").val();
    if ($(document).find('#VendorRequestSearch').hasClass('dataTable')) {
        dtVendorRequest.destroy();
    }
    dtVendorRequest = $("#VendorRequestSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 1
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }//Vendor rq Sorting
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
                    selectCount = 0;
                    if (json.LayoutInfo !== '') {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        order = LayoutInfo.order[0][0];
                        orderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo !== '') {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#spnControlCounter"), $("#dvFilterSearchSelect2"));
                        }
                    }
                    else {
                        callback(json.LayoutInfo);
                    }
                }
            });
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        scrollX: true,
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'VendorRequest'
            },
            {
                extend: 'print',
                title: 'VendorRequest'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'VendorRequest',
                extension: '.csv',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                orientation: 'portrait',
                pageSize: 'A4',
                title: 'VendorRequest'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/VendorRequest/GetVendorRequestchunkGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                activeStatus = localStorage.getItem("VENDORREQUESTSEARCHGRIDDISPLAYSTATUS");
                d._addresscity = LRTrim($('#AddressCity').val());
                d._addressstate = LRTrim($('#AddressState').val());
                d._name = LRTrim($('#Name').val());
                d._type = LRTrim($('#ddlType').val());
                d._status = LRTrim($('#Status').val());
                d.customDisplay = activeStatus;
                d.srcData = LRTrim($("#txtColumnSearch").val());
                d.Order = order;//Vendor rq Sorting
            },
            "dataSrc": function (result) {
                let colOrder = dtVendorRequest.order();
                orderDir = colOrder[0][1];

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
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        "columns":
            [
                {
                    "data": "Name",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "0",
                    
                },
                {
                    "data": "City", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                    
                }, {
                    "data": "State", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2",
                    
                },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4",
                    render: function (data, type, row, meta) {
                        if (data == statusCode.Approved) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--yellow m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Open) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Denied) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--purple m-badge--wide'>" + getStatusValue(data) + "</span >";
                        } else {
                            return getStatusValue(data);
                        }
                    }
                },
                {
                    "data": "VendorRequestId", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "5", "className": "text-center",
                    "mRender": function (data, type, row) {
                       
                        if (row.Status == "Open") {
                            var editVendorRequestBttnhtml = "";
                            var delVendorRequestBttnhml = "";
                            var VRApprovalBttnhml = "";
                            var VRDenyBttnhm = "";
                            if (IsVendorRequestCreateAccessSecurity == "True" && IsVendorRequestApprovalAccessSecurity == "True") {
                                editVendorRequestBttnhtml = '<a class="btn btn-blue editVendorRequestBttn gridinnerbutton" style="margin-left: 0;" title="Edit">Edit</a>';
                                delVendorRequestBttnhml = '<a class="btn btn-blue delVendorRequestBttn gridinnerbutton" style="margin-left: 0;" title="Delete">Delete</a>';
                                VRApprovalBttnhml = '<a class="btn btn-blue VRApprovalBttn gridinnerbutton" style="margin-left: 0;" title="Approve">Approve</a>';
                                VRDenyBttnhm = '<a class="btn btn-blue VRDenyBttn gridinnerbutton" style="margin-left: 0;" title="Deny">Deny</a>';
                            }
                            else {
                                if (IsVendorRequestCreateAccessSecurity == "True" && IsVendorRequestApprovalAccessSecurity == "False") {
                                    editVendorRequestBttnhtml = '<a class="btn btn-blue editVendorRequestBttn gridinnerbutton" style="margin-left: 0;" title="Edit">Edit</a>';
                                    delVendorRequestBttnhml = '<a class="btn btn-blue delVendorRequestBttn gridinnerbutton" style="margin-left: 0;" title="Delete">Delete</a>';

                                } else {
                                    if (IsVendorRequestCreateAccessSecurity == "False" && IsVendorRequestApprovalAccessSecurity == "True") {
                                        VRApprovalBttnhml = '<a class="btn btn-blue VRApprovalBttn gridinnerbutton" style="margin-left: 0;" title="Approve">Approve</a>';
                                        VRDenyBttnhm = '<a class="btn btn-blue VRDenyBttn gridinnerbutton" style="margin-left: 0;" title="Deny">Deny</a>';

                                    }
                                }
                            }

                            return '<button type="button" class="btn btn-blue actionbtns" style="border:0;outline:0" data-id="' + row.VendorRequestId + '"><i style="font-size:15px" class="fa">&#xf142;</i></button>' +
                                '<div id="' + row.VendorRequestId + '" class="actionbtndiv" style="display:none;">'
                                +
                                editVendorRequestBttnhtml
                                + delVendorRequestBttnhml
                                + VRApprovalBttnhml
                                + VRDenyBttnhm +
                                '</div >' +
                                '<div class="maskaction ' + row.VendorRequestId + '" data-id="' + row.VendorRequestId + '"  onclick="funcCloseActionbtn()"></div>';

                        } else {
                            return '';
                        }
                    }
                },
            ],
        "columnDefs": [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function (settings, json) {
            SetPageLengthMenu();
            $("#ActionGridBar :input").removeAttr("disabled");
            $("#ActionGridBar :button").removeClass("disabled");
            DisableExportButton($("#VendorRequestSearch"), $(document).find('.import-export'));
        }
    });
}
$(document).on('click', '#VendorRequestSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#VendorRequestSearch_length .searchdt-menu', function () {
    run = true;
});
$('#VendorRequestSearch').find('th').click(function () {
    run = true;
    order = $(this).data('col');
});
function clearAdvanceSearch() {
    $('#advsearchsidebar').find('input:text').val('');
    $(document).find("#ddlType").val("").trigger('change.select2');
    selectCount = 0;
    $("#spnControlCounter").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
    $('#dvFilterSearchSelect2').find('span').html('');
    $('#dvFilterSearchSelect2').find('span').removeClass('tagTo');
}

function AWBAdvSearch() {
    var searchitemhtml = "";
    $(document).find('#txtColumnSearch').val('');
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#dvFilterSearchSelect2").html(searchitemhtml);

    $("#spnControlCounter").text(selectCount);
}
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
       
       
        if (this.context.length) {
            var _addresscity = LRTrim($('#AddressCity').val());
            var _addressstate = LRTrim($('#AddressState').val());
            var _name = LRTrim($('#Name').val());
            var _type = LRTrim($('#ddlType').val());
            var _status = LRTrim($('#Status').val());
            var customDisplay = localStorage.getItem("VENDORREQUESTSEARCHGRIDDISPLAYSTATUS");
            var srcData = LRTrim($("#txtColumnSearch").val());
            var colname = order;
            var coldir = orderDir;
            var jsonResult = $.ajax({
                "url": "/VendorRequest/GetVendorRequestPrintData",
                data: {
                    _name: _name,
                    _addresscity: _addresscity,
                    _addressstate: _addressstate,
                    _type: _type,
                    _status: _status,
                    customDisplay: customDisplay,
                    srcData: srcData,
                    colname: colname,
                    coldir: coldir
                },
                success: function (result) {
                },
                async: false
            });
           
                var thisdata = JSON.parse(jsonResult.responseText).data;
                var visiblecolumnsIndex = $("#VendorRequestSearch thead tr th").map(function (key) {
                    return this.getAttribute('data-th-index');
                }).get();
                var d = [];
                $.each(thisdata, function (index, item) {
                   
                    if (item.Name != null) {
                        item.Name = item.Name;
                    }
                    else {
                        item.Name = "";
                    }
                    if (item.City != null) {
                        item.City = item.City;
                    }
                    else {
                        item.City = "";
                    }
                    if (item.State != null) {
                        item.State = item.State;
                    }
                    else {
                        item.State = "";
                    }
                    if (item.Type != null) {
                        item.Type = item.Type;
                    }
                    else {
                        item.Type = "";
                    }
                    if (item.Status != null) {
                        item.Status = item.Status;
                    }
                    else {
                        item.Status = "";
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
                    header: $("#VendorRequestSearch thead tr th div").map(function (key) {
                        return this.innerHTML;
                    }).get()
                };
           
        }
    });
})
//#region 
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
            advcountercontainer.text(selectCount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);

}
//#endregion
//#endregion

//#region Commonn
$(document).ready(function () {
    $("#action").click(function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $("#action").focusout(function () {
        $(".actionDrop").fadeOut();
    });
    $(document).find('.select2picker').select2({
    });
    $(".actionBar").fadeIn();
    $("#ActionGridBar :input").attr("disabled", "disabled");
});

$.validator.setDefaults({ ignore: null });

//#endregion

//#region Dropdown toggle
$("#sidebar").mCustomScrollbar({
    theme: "minimal"
});
$('#dismiss, .overlay').on('click', function () {
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
});
$(document).find('#sidebarCollapse').on('click', function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
//$(document).on('click', "ul.vtabs li", function () {
//    $("ul.vtabs li").removeClass("active");
//    $(this).addClass("active");
//    $(".tabsArea").hide();
//    var activeTab = $(this).find("a").attr("href");
//    $(activeTab).fadeIn();
//    return false;
//});
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
$(document).on('keyup', '#vendorsearctxtbox', function (e) {
    var tagElems = $(document).find('#vendorsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.vendorsearchdrpbox', function (e) {
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#dvFilterSearchSelect2").html('');
    $(document).find('#txtColumnSearch').val('');
    run = true;
    $('#vendorsearctxtbox').text($(this).text());
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("VENDORREQUESTSEARCHGRIDDISPLAYSTATUS", optionval);
    activeStatus = optionval;
    $(document).find('#vendorsearchtitle').text($(this).text());
    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtVendorRequest.page('first').draw('page');
    }
});

//#endregion

//#region New Search button
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'VendorRequest' },
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
        data: { tableName: 'VendorRequest', searchText: txtSearchval, isClear: isClear },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)"><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchList.innerHTML = str;
        },
        complete: function () {
            if (isClear == false) {
                dtVendorRequest.page('first').draw('page');
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
    $("#gridadvsearchstatus").val('');
    activeStatus = localStorage.getItem("VENDORREQUESTSEARCHGRIDDISPLAYSTATUS");
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#dvFilterSearchSelect2").html(searchitemhtml);
    }
    else {
        dtVendorRequest.page('first').draw('page');
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
    CloseLoader();
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

//#region liCustomize1
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtVendorRequest, true, titleArray);
});

$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(dtVendorRequest, colOrder);
    run = true;
    dtVendorRequest.state.save(run);
    
});
//#endregion

//#region  Reset Grid
$('#liResetGridClearBtn').click(function (e) {
    CancelAlertSetting.text = getResourceValue("ResetGridAlertMessage");
    swal(CancelAlertSetting, function () {
        var localstorageKeys = [];
        localstorageKeys.push("VENDORREQUESTSEARCHGRIDDISPLAYSTATUS");
        DeleteGridLayout('VendorRequests_Search', dtVendorRequest, localstorageKeys);
        GenerateSearchList('', true);
        window.location.href = "../VendorRequest/Index?page=Inventory_Vendor_Request";
    });
});
//#endregion
//#region Add/Edit

$(document).on('click', '.addVendorRequest', function () {
    var data = dtVendorRequest.row($(this).parents('tr')).data();
    AddVendorRequest();
});

$(document).on('click', '.editVendorRequestBttn', function () {
    var data = dtVendorRequest.row($(this).parents('tr')).data();
    EditVendorRequest(data.VendorRequestId);
});

$(document).on('click', '.delVendorRequestBttn', function () {
    var data = dtVendorRequest.row($(this).parents('tr')).data();
    DeleteVendorRequest(data.VendorRequestId);
});
$(document).on('click', '.VRApprovalBttn', function () {
    var data = dtVendorRequest.row($(this).parents('tr')).data();
    ApprovalVendorRequest(data.VendorRequestId);
});
$(document).on('click', '.VRDenyBttn', function () {
    var data = dtVendorRequest.row($(this).parents('tr')).data();
    DenyVendorRequest(data.VendorRequestId);
});
function ApprovalVendorRequest(VendorRequestId) {
    run = true;
    CancelAlertSetting.text = getResourceValue("VendorRequestApproveAlert");
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/VendorRequest/ApprovalVendorRequest',
            data: {
                VendorRequestId: VendorRequestId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    
                    ShowDeleteAlert(getResourceValue("approvedsuccessmsg"));
                    
                }
            },
            complete: function () {
                dtVendorRequest.page('first').draw('page');
                CloseLoader();
            }
        });
    });
}
function DenyVendorRequest(VendorRequestId) {
    run = true;
    CancelAlertSetting.text = getResourceValue("VendorRequestDenyAlert");
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/VendorRequest/DenyVendorRequest',
            data: {
                VendorRequestId: VendorRequestId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    
                    ShowDeleteAlert(getResourceValue("DeniedSuccessfullyAlert"));
                    
                }
            },
            complete: function () {
                dtVendorRequest.page('first').draw('page');
                CloseLoader();
            }
        });
    });
}
function DeleteVendorRequest(VendorRequestId) {
    run = true;
    CancelAlertSetting.text = getResourceValue("VendorRequestDeleteAlert");
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/VendorRequest/DeleteVendorRequest',
            data: {
                VendorRequestId: VendorRequestId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("DeleteSuccessfullyAlert"));
                }
            },
            complete: function () {
                dtVendorRequest.page('first').draw('page');
                CloseLoader();
            }
        });
    });
}
function AddEditJs() {
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $(document).find('.select2picker').select2({});
    var defaultRangeValidator = $.validator.methods.range;
    $.validator.methods.range = function (value, element, param) {
        if (element.type === 'checkbox') {
            // if it's a checkbox return true if it is checked
            return element.checked;
        } else {
            // otherwise run the default validation function
            return defaultRangeValidator.call(this, value, element, param);
        }
    }
}
function EditVendorRequest(VendorRequestID) {
    
    $.ajax({
        url: "/VendorRequest/EditVendorRequest",
        type: "GET",
        dataType: 'html',
        data: { VendorRequestID: VendorRequestID},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            CloseLoader();
            $('#renderVendorRequest').html(data);
        },
        complete: function () {
            AddEditJs();
        },
        error: function () {
            CloseLoader();
        }
    });
}

function AddVendorRequest() {
    $.ajax({
        url: "/VendorRequest/AddVendorRequest",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderVendorRequest').html(data);
        },
        complete: function () {
            CloseLoader();
            AddEditJs();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}

$(document).on('click', "#btnCancelAddVR", function () {
    CancelAlertSetting.text = getResourceValue("CancelAlertLostMsg");
    swal(CancelAlertSetting, function () {
        window.location.href = "../VendorRequest/Index?page=Inventory_Vendor_Request";
    });
});

function VendorRequestAddOrEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            if (data.Command == "save") {
                SuccessAlertSetting.text = getResourceValue("VendorRequestAddedSuccessfullyAlert");
                swal(SuccessAlertSetting, function () {
                    ResetErrorDiv();
                    window.location.href = "../VendorRequest/Index?page=Inventory_Vendor_Request";
                });
            }
            else {
                SuccessAlertSetting.text = getResourceValue("VendorRequestAddedSuccessfullyAlert");
                ResetErrorDiv();
                swal(SuccessAlertSetting, function () {
                    $(document).find('form').trigger("reset"); 
                    $(document).find('form').find("select").val("").trigger('change');
                    $(document).find('form').find("input").removeClass("input-validation-error");
                    $(document).find('form').find("select").removeClass("input-validation-error");
                });
            }
        }
        else {
            SuccessAlertSetting.text = getResourceValue("VendorRequestUpdatedSuccessfullyAlert");
            swal(SuccessAlertSetting, function () {
                ResetErrorDiv();
                window.location.href = "../VendorRequest/Index?page=Inventory_Vendor_Request";
            });
        }

    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('change', '#chkIsBusiness', function () {
    if (this.checked) {
        $(document).find('#vendorRequestModel_RemitAddress1').val($('#vendorRequestModel_Address1').val());
        $(document).find('#vendorRequestModel_RemitAddress2').val($('#vendorRequestModel_Address2').val());
        $(document).find('#vendorRequestModel_RemitAddress3').val($('#vendorRequestModel_Address3').val());
        $(document).find('#vendorRequestModel_RemitCity').val($('#vendorRequestModel_AddressCity').val());
        $(document).find('#vendorRequestModel_RemitState').val($('#vendorRequestModel_AddressState').val());
        $(document).find('#vendorRequestModel_RemitPostCode').val($('#vendorRequestModel_AddressPostCode').val());
        $(document).find('#vendorRequestModel_RemitCountry').val($('#vendorRequestModel_AddressCountry').val());
        $(document).find('#vendorRequestModel_RemitAddress1').attr('readonly', true);
        $(document).find('#vendorRequestModel_RemitAddress2').attr('readonly', true);
        $(document).find('#vendorRequestModel_RemitAddress3').attr('readonly', true);
        $(document).find('#vendorRequestModel_RemitCity').attr('readonly', true);
        $(document).find('#vendorRequestModel_RemitState').attr('readonly', true);
        $(document).find('#vendorRequestModel_RemitPostCode').attr('readonly', true);
        $(document).find('#vendorRequestModel_RemitCountry').attr('readonly', true);
    }
    else {
        $(document).find('#vendorRequestModel_RemitAddress1').val('');
        $(document).find('#vendorRequestModel_RemitAddress2').val('');
        $(document).find('#vendorRequestModel_RemitAddress3').val('');
        $(document).find('#vendorRequestModel_RemitCity').val('');
        $(document).find('#vendorRequestModel_RemitState').val('');
        $(document).find('#vendorRequestModel_RemitPostCode').val('');
        $(document).find('#vendorRequestModel_RemitCountry').val('');
        $(document).find('#vendorRequestModel_RemitAddress1').attr('readonly', false);
        $(document).find('#vendorRequestModel_RemitAddress2').attr('readonly', false);
        $(document).find('#vendorRequestModel_RemitAddress3').attr('readonly', false);
        $(document).find('#vendorRequestModel_RemitCity').attr('readonly', false);
        $(document).find('#vendorRequestModel_RemitState').attr('readonly', false);
        $(document).find('#vendorRequestModel_RemitPostCode').attr('readonly', false);
        $(document).find('#vendorRequestModel_RemitCountry').attr('readonly', false);
    }
});
//#endregion
function funcCloseActionbtn() {
    var btnid = $(this).attr("data-id");
    $(document).find(".actionbtndiv").hide();
    $(document).find(".maskaction").hide();
}

$(document).on('click', '.actionbtns', function (e) {
    var row = $(this).parents('tr');
    var data = dtVendorRequest.row(row).data();
    $(document).find("#" + data.VendorRequestId + "").show();
    $(document).find("." + data.VendorRequestId).show();
});