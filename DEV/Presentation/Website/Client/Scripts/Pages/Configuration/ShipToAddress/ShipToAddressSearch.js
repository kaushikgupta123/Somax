//#region Common
var shipToAddressTable;
var run = false;
var selectCount = 0;
var activeStatus = false;
var order = '0';
var orderDir = 'asc';
var filterinfoarray = [];
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
//#endregion
//#region Search
$(document).ready(function () {
    generateShipToAddressDataTable();
    ShowbtnLoader("btnsortmenu");
    $("#shipToAddressAction :input").attr("disabled", "disabled");
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
});
function generateShipToAddressDataTable() {
    var showAddBtn = false;
    var showEditBtn = false;
    var showDeleteBtn = false;
    var printCounter = 0;
    if ($(document).find('#shipToAddressSearch').hasClass('dataTable')) {
        shipToAddressTable.destroy();
    }
    shipToAddressTable = $("#shipToAddressSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 1,
            fixedColumnsRight: 1
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
                var filterinfoarray = getfilterinfoarray($('#advsearchsidebar'));
                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: "ShipToAddress_Search",
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
            $.ajax({
                "url": "/Base/GetLayout",
                "data": {
                    GridName: "ShipToAddress_Search",
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    if (json.LayoutInfo !== '') {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        order = LayoutInfo.order[0][0];
                        orderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo !== '') {
                            setsearchui(JSON.parse(json.FilterInfo), $("#spnControlCounter"), $("#dvFilterSearchSelect2"));
                        }
                    }
                    else {
                        callback(json.LayoutInfo);
                    }
                    //if (json) {
                    //    callback(JSON.parse(json));
                    //}
                    //else {
                    //    callback(json);
                    //}
                }
            });
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
                title: 'Ship To Address'
            },
            {
                extend: 'print',
                title: 'Ship To Address',
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Ship To Address',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                title: 'Ship To Address',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/ShipToAddress/GetGridDataforShipToAddress",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.ClientLookupId = LRTrim($('#ShipToId').val());
                d.Address1 = LRTrim($('#Address1').val());
                d.AddressCity = LRTrim($('#AddressCity').val());
                d.AddressState = LRTrim($('#AddressState').val());                
            },
            "dataSrc": function (result) {
                let colOrder = shipToAddressTable.order();
                orderDir = colOrder[0][1];
                order = colOrder[0][0];
                showAddBtn = result.showAddBtn;
                showEditBtn = result.showEditBtn;
                showDeleteBtn = result.showDeleteBtn;          
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
                $("#shipTopoAddressAction :input").not('.import-export').removeAttr("disabled");
                $("#shipTopoAddressAction :button").not('.import-export').removeClass("disabled");
            },
            global: true
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    if (showEditBtn) {
                        if (showAddBtn) {
                            if (showDeleteBtn) {
                                return '<a class="btn btn-outline-primary addBtnShipToAddress gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                                    '<a class="btn btn-outline-success editBtnShipToAddress gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                                    '<a class="btn btn-outline-danger deleteBtnShipToAddress gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                            }
                            else {
                                return '<a class="btn btn-outline-primary addBtnShipToAddress gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                                    '<a class="btn btn-outline-success editBtnShipToAddress gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                            }
                        }
                        else {
                            if (showDeleteBtn) {
                                '<a class="btn btn-outline-success editBtnShipToAddress gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                                    '<a class="btn btn-outline-danger deleteBtnShipToAddress gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                            }
                            else {
                                '<a class="btn btn-outline-success editBtnShipToAddress gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>'
                            }
                        }
                    }
                    else {
                        if (showAddBtn) {
                            if (showDeleteBtn) {
                                return '<a class="btn btn-outline-primary addBtnShipToAddress gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                                    '<a class="btn btn-outline-danger deleteBtnShipToAddress gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                            }
                            else {
                                return '<a class="btn btn-outline-primary addBtnShipToAddress gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>';
                            }
                        }
                        else {
                            if (showDeleteBtn) {
                                return '<a class="btn btn-outline-danger deleteBtnShipToAddress gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                            }
                            else {
                                return "";
                            }
                        }
                    }
                }
            },
            {
                targets: [0, 4],
                className: 'noVis'
            }
        ],
        "columns":
            [
                {
                    "data": "ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "0",
                },
                { "data": "Address1", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
                { "data": "AddressCity", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "AddressState", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                { "className": "text-center", "bSortable": false }
            ],

        initComplete: function () {
            var actionColumn = this.api().column(4);
            if (showAddBtn === false && showEditBtn === false && showDeleteBtn === false) {
                actionColumn.visible(false);
            }
            else {
                actionColumn.visible(true);
            }
            $(document).on('click', '.status', function (e) {
                e.preventDefault();
            });
            SetPageLengthMenu();
        }
    });
};

$(document).on('click', '#shipToAddressSearch_paginate .paginate_button', function () {   
    run = true;
});
$(document).on('change', '#shipToAddressSearch_length .searchdt-menu', function () {  
    run = true;
});

$(document).on('click', '#shipToAddressSearch_wrapper th', function () {
    run = true;
});
//#endregion
//#region Add/Edit

$(document).on('click', '.addNewShipToAddress', function () {
    AddShipToAddress();
});

$(document).on('click', '.addBtnShipToAddress', function () {
    AddShipToAddress();
});

$(document).on('click', '.editBtnShipToAddress', function () {
    var data = shipToAddressTable.row($(this).parents('tr')).data();
    EditShipToAddress(data);
});

$(document).on('click', '.deleteBtnShipToAddress', function () {
    var data = shipToAddressTable.row($(this).parents('tr')).data();
    DeleteShipToAddress(data);
});

function DeleteShipToAddress(data) {
    run = true;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/ShipToAddress/DeleteShipToAddress',
            data: {
                ShipToAddressId: data.ShipToId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("ShipToAddressDeleteAlert"));                   
                }
            },
            complete: function () {
                shipToAddressTable.page('first').draw('page');
                CloseLoader();
            }
        });
    });
}

function EditShipToAddress(data) {    
    $.ajax({
        url: "/ShipToAddress/EditShipToAddress",
        type: "GET",
        dataType: 'html',
        data: { ShipToId: data.ShipToId},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            CloseLoader();
            $('#renderShipToAddress').html(data);
        },
        complete: function () {
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            CloseLoader();
        }
    });
}

function AddShipToAddress() {
    $.ajax({
        url: "/ShipToAddress/AddShipToAddress",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderShipToAddress').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}

$(document).on('click', "#btnCancelAddSTA", function () {
    swal(CancelAlertSetting, function () {
        window.location.href = "../ShipToAddress/Index?page=Ship_To_Address";
    });
});

function ShipToAddressAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.mode == "add") {
            if (data.Command == "save") {
                SuccessAlertSetting.text = getResourceValue("ShipToAddressAddAlert");
                swal(SuccessAlertSetting, function () {
                    ResetErrorDiv();
                    window.location.href = "../ShipToAddress/Index?page=Ship_To_Address";
                });
            }
            else {
                SuccessAlertSetting.text = getResourceValue("ShipToAddressAddAlert");
                ResetErrorDiv();
                swal(SuccessAlertSetting, function () {
                    $(document).find('form').trigger("reset");
                    $(document).find('form').find("input").removeClass("input-validation-error");
                });
            }
        }
        else {
            SuccessAlertSetting.text = getResourceValue("ShipToAddressUpdateAlert");
            swal(SuccessAlertSetting, function () {
                ResetErrorDiv();
                window.location.href = "../ShipToAddress/Index?page=Ship_To_Address";
            });
        }

    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

//#endregion
//#region Export
$(document).on('click', '#liPrint', function () {
    $(".buttons-print")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liPdf', function () {
    $(".buttons-pdf")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    funcCloseExportbtn();
});
//#endregion

//#region Advanced Search
$("#btnShipToAddressDataAdvSrch").on('click', function (e) {
    run = true;
    ShipToAddressAdvSearch();
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    shipToAddressTable.page('first').draw('page');
});
function ShipToAddressAdvSearch() {
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#liSelectCount').text(selectCount + ' Filters Applied');
    $("#dvFilterSearchSelect2").html(searchitemhtml);
    $("#spnControlCounter").text(selectCount);
}
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    clearAdvanceSearch();
    shipToAddressTable.page('first').draw('page');
});
function clearAdvanceSearch() {
    selectCount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    selectCount = 0;
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
    if (searchtxtId == "ShipToId") {
        $("#ShipToId").val('');
    }
    if (searchtxtId == "Address1") {
        $("#Address1").val('');
    }
    if (searchtxtId == "AddressCity") {
        $("#AddressCity").val('');
    }
    if (searchtxtId == "AddressState") {
        $("#AddressState").val('');
    }
    ShipToAddressAdvSearch();
    shipToAddressTable.page('first').draw('page');
});
//#endregion Advanced Search

//#region Print
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var colname = order;
            var coldir = orderDir;
            var jsonResult = $.ajax({
                "url": "/ShipToAddress/GetShipToAddressPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    _colname: colname,
                    _coldir: coldir,
                    _clientLookupId : LRTrim($('#ShipToId').val()),
                    _address1 : LRTrim($('#Address1').val()),
                    _addressCity : LRTrim($('#AddressCity').val()),
                    _ddressState : LRTrim($('#AddressState').val()),
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data; 
            var visiblecolumnsIndex = $("#shipToAddressSearch thead tr th").not(':eq(4)').map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.ClientLookupId != null) {
                    item.ClientLookupId = item.ClientLookupId;
                }
                else {
                    item.ClientLookupId = "";
                }
                if (item.Address1 != null) {
                    item.Address1 = item.Address1;
                }
                else {
                    item.Address1 = "";
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
                header: $("#shipToAddressSearch thead tr th").not(":eq(4)").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
})
//#endregion Print
//#region Grid data retain

function getfilterinfoarray(advsearchcontainer) {
    var filterinfoarray = [];
    var f = new filterinfo('searchstring');
    filterinfoarray.push(f);
    advsearchcontainer.find('.adv-item').each(function (index, item) {
        if ($(this).parent('div').is(":visible")) {
            f = new filterinfo($(this).attr('id'), $(this).val());
            filterinfoarray.push(f);
        }
    });
    return filterinfoarray;
}
function setsearchui(data, advcountercontainer, searchstringcontainer) {
    var searchitemhtml = '';
    $.each(data, function (index, item) {        
       
            if ($('#' + item.key).parent('div').is(":visible")) {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
            advcountercontainer.text(selectCount);
       
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
    $('#liSelectCount').text(selectCount + ' Filters Applied');
}
//#endregion Grid data retain