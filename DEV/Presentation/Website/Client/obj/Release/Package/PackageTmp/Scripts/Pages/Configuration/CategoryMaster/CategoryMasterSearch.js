//#region Common
var categoryMasterTable;
var run = false;
var selectCount = 0;
var activeStatus = false;
var order = '0';
var orderDir = 'asc';
//#endregion
//#region Search
$(document).ready(function () {
    activeStatus = localStorage.getItem("CATEGORYMASTERSEARCHGRIDDISPLAYSTATUS");
    if (activeStatus) {
        if (activeStatus == "false") {
            activeStatus = false;
            $('#categoryMasterDropdown').val("1").trigger('change.select2');
        }
        else {
            activeStatus = true;
            $('#categoryMasterDropdown').val("2").trigger('change.select2');
        }
    }
    else {
        activeStatus = false;
    }
    generateCategoryMasterDataTable();

    ShowbtnLoaderclass("LoaderDrop");
    ShowbtnLoader("btnsortmenu");
    $("#categoryMasterAction :input").attr("disabled", "disabled");
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
function generateCategoryMasterDataTable() {
    var showAddBtn = false;
    var showEditBtn = false;
    var showDeleteBtn = false;
    var printCounter = 0;
    if ($(document).find('#categoryMasterSearch').hasClass('dataTable')) {
        categoryMasterTable.destroy();
    }
    categoryMasterTable = $("#categoryMasterSearch").DataTable({
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
                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: "CategoryMaster_Search",
                        LayOutInfo: JSON.stringify(data)
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
                "url": "/Base/GetState",
                "data": {
                    GridName: "CategoryMaster_Search",
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    if (json) {
                        callback(JSON.parse(json));
                    }
                    else {
                        callback(json);
                    }
                }
            });
        },
        scrollX: true,
        fixedColumns: {
            leftColumns: 1
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Category Master'
            },
            {
                extend: 'print',
                title: 'Category Master',
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Category Master',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                title: 'Category Master',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/CategoryMaster/GetGridDataforCategoryMaster",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.ClientLookupId = LRTrim($('#PartCategoryMasterId').val());
                d.Description = LRTrim($('#Description').val());
                d.Inactive = activeStatus;                
            },
            "dataSrc": function (result) {
                let colOrder = categoryMasterTable.order();
                orderDir = colOrder[0][1];
                order = colOrder[0][0];
                showAddBtn = result.showAddBtn;
                showEditBtn = result.showEditBtn;
                showDeleteBtn = result.showDeleteBtn;
                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");             
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
                $("#categoryMasterAction :input").not('.import-export').removeAttr("disabled");
                $("#categoryMasterAction :button").not('.import-export').removeClass("disabled");
            },
            global: true
        },
        columnDefs: [
            {
                targets: [2], render: function (a, b, data, d) {
                    if (showEditBtn) {
                        if (showAddBtn) {
                            if (showDeleteBtn) {
                                return '<a class="btn btn-outline-primary addBtnCategoryMaster gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                                    '<a class="btn btn-outline-success editBtnCategoryMaster gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                                    '<a class="btn btn-outline-danger deleteBtnCategoryMaster gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                            }
                            else {
                                return '<a class="btn btn-outline-primary addBtnCategoryMaster gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                                    '<a class="btn btn-outline-success editBtnCategoryMaster gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                            }
                        }
                        else {
                            if (showDeleteBtn) {
                                return '<a class="btn btn-outline-success editBtnCategoryMaster gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a> ' +
                                    '<a class="btn btn-outline-danger deleteBtnCategoryMaster gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                            }
                            else {
                                return '<a class="btn btn-outline-success editBtnCategoryMaster gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                            }
                        }
                    }
                    else {
                        if (showAddBtn) {
                            if (showDeleteBtn) {
                                return '<a class="btn btn-outline-primary addBtnCategoryMaster gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                                    '<a class="btn btn-outline-danger deleteBtnCategoryMaster gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                            }
                            else {
                                return '<a class="btn btn-outline-primary addBtnCategoryMaster gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>';
                            }
                        }
                        else {
                            if (showDeleteBtn) {
                                return '<a class="btn btn-outline-danger deleteBtnCategoryMaster gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                            }
                            else {
                                return "";
                            }
                        }
                    }
                }
            },
            {
                targets: [0, 2],
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
                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
                { "className": "text-center", "bSortable": false }
            ],

        initComplete: function () {
            var actionColumn = this.api().column(2);
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
$(document).on('change', '#categoryMasterDropdown', function () {
    run = true;
    ShowbtnLoaderclass("LoaderDrop");
    var optionVal = $(document).find("#categoryMasterDropdown").val();
    if (optionVal == "1") {
        activeStatus = false;
        $('#btnActiveMain').addClass("active");
        $('#btnInactiveMain').removeClass("active");
        localStorage.setItem("CATEGORYMASTERSEARCHGRIDDISPLAYSTATUS", false);
        clearAdvanceSearch();
        // categoryMasterTable.page('first').draw('page');
        //  generateManufacturerMasterDataTable();
    }
    else {
        activeStatus = true;
        $('#btnActiveMain').removeClass("active");
        $('#btnInactiveMain').addClass("active");
        localStorage.setItem("CATEGORYMASTERSEARCHGRIDDISPLAYSTATUS", true);
        var searchOption = LRTrim($("#txtVendorDataSrch").val());
        if (searchOption.trim() == null || searchOption.trim() == "") {
            $("#spnControlCounter").text(selectCount);
            $('#liSelectCount').text(selectCount + ' filters applied');
        }
        else {
            clearAdvanceSearch();
        }
        // categoryMasterTable.page('first').draw('page');
        //generateManufacturerMasterDataTable();
    }
    categoryMasterTable.page('first').draw('page');
});
$(document).on('click', '#categoryMasterSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#categoryMasterSearch_length .searchdt-menu', function () {
    run = true;
});
$(document).on('click', '#categoryMasterSearch_wrapper th', function () {
    run = true;
});
//#endregion
//#region Add/Edit

$(document).on('click', '.addNewCategoryMaster', function () {
    var data = categoryMasterTable.row($(this).parents('tr')).data();
    AddCategoryMaster();
});

$(document).on('click', '.addBtnCategoryMaster', function () {
    var data = categoryMasterTable.row($(this).parents('tr')).data();
    AddCategoryMaster();
});

$(document).on('click', '.editBtnCategoryMaster', function () {
    var data = categoryMasterTable.row($(this).parents('tr')).data();
    EditCategoryMaster(data.PartCategoryMasterId, data.ClientLookupId, data.Description, data.Inactive, "update");
});

$(document).on('click', '.deleteBtnCategoryMaster', function () {
    var data = categoryMasterTable.row($(this).parents('tr')).data();
    DeleteCategoryMaster(data.PartCategoryMasterId, data.ClientLookupId, data.Description, data.Inactive);
});

function DeleteCategoryMaster(PartCategoryMasterId, ClientLookupId, Description, Inactive) {
    run = true;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/CategoryMaster/DeleteCategoryMaster',
            data: {
                PartCategoryMasterId: PartCategoryMasterId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("CategoryMasterDeleteAlert"));                   
                }
            },
            complete: function () {
                categoryMasterTable.page('first').draw('page');
                CloseLoader();
            }
        });
    });
}

function EditCategoryMaster(PartCategoryMasterId, ClientLookupId, Description, Inactive) {
    var ClientLookupId = ClientLookupId;
    $.ajax({
        url: "/CategoryMaster/EditCategoryMaster",
        type: "GET",
        dataType: 'html',
        data: { PartCategoryMasterId: PartCategoryMasterId, Description: Description, Inactive: activeStatus, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            CloseLoader();
            $('#renderCategoryMaster').html(data);
        },
        complete: function () {
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            CloseLoader();
        }
    });
}

function AddCategoryMaster() {
    $.ajax({
        url: "/CategoryMaster/AddCategoryMaster",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderCategoryMaster').html(data);
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

$(document).on('click', "#btnCancelAddMM", function () {
    swal(CancelAlertSetting, function () {
        window.location.href = "../CategoryMaster/Index?page=Category_Master";
    });
});

function CategoryMasterAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            if (data.Command == "save") {
                SuccessAlertSetting.text = getResourceValue("CategoryMasterAddAlert");
                swal(SuccessAlertSetting, function () {
                    ResetErrorDiv();
                    window.location.href = "../CategoryMaster/Index?page=Category_Master";
                });
            }
            else {
                SuccessAlertSetting.text = getResourceValue("CategoryMasterAddAlert");
                ResetErrorDiv();
                swal(SuccessAlertSetting, function () {
                    $(document).find('form').trigger("reset");
                    $(document).find('form').find("input").removeClass("input-validation-error");
                });
            }
        }
        else {
            SuccessAlertSetting.text = getResourceValue("CategoryMasterUpdateAlert");
            swal(SuccessAlertSetting, function () {
                ResetErrorDiv();
                window.location.href = "../CategoryMaster/Index?page=Category_Master";
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
//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(categoryMasterTable, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(categoryMasterTable, colOrder);
    run = true;
    categoryMasterTable.state.save(run);
});
//#endregion
//#region Advanced Search
$("#btnCategoryMasterDataAdvSrch").on('click', function (e) {
    run = true;
    ManIdVal = $("#advstatus").val();
    CategoryMasterAdvSearch();
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    categoryMasterTable.page('first').draw('page');
});
function CategoryMasterAdvSearch() {
    var InactiveFlag = false;
    var searchitemhtml = "";
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
    categoryMasterTable.page('first').draw('page');
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
    if (searchtxtId == "PartCategoryMasterId") {
        $("#PartCategoryMasterId").val('');
    }
    if (searchtxtId == "Description") {
        $("#Description").val('');
    }
    CategoryMasterAdvSearch();
    categoryMasterTable.page('first').draw('page');
});
//#endregion Advanced Search
//#region Print
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            //var currestsortedcolumn = $('#categoryMasterSearch').dataTable().fnSettings().aaSorting[0][0];
            //var coldir = $('#categoryMasterSearch').dataTable().fnSettings().aaSorting[0][1];
            //var colname = $('#categoryMasterSearch').dataTable().fnSettings().aoColumns[currestsortedcolumn].Name;
            var valPartCategoryMasterId = LRTrim($('#PartCategoryMasterId').val());
            var valDescription = LRTrim($('#Description').val());
            var valInactiveFlag = activeStatus;
           // dtTable = $("#categoryMasterSearch").DataTable();
           // var currestsortedcolumn = $('#categoryMasterSearch').dataTable().fnSettings().aaSorting[0][0];
            //var coldir = $('#categoryMasterSearch').dataTable().fnSettings().aaSorting[0][1];
            //var colname = $('#categoryMasterSearch').dataTable().fnSettings().aoColumns[currestsortedcolumn].Name;

            var colname = order;
            var coldir = orderDir;
            var jsonResult = $.ajax({
                "url": "/CategoryMaster/GetCategoryMasterPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    _colname: colname,
                    _coldir: coldir,
                    _partCategoryMasterId: valPartCategoryMasterId,
                    _description: valDescription,
                    _inactiveFlag: valInactiveFlag,
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#categoryMasterSearch thead tr th").not(':eq(2)').map(function (key) {
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
                if (item.Description != null) {
                    item.Description = item.Description;
                }
                else {
                    item.Description = "";
                }
                if (item.PartCategoryMasterId != null) {
                    item.PartCategoryMasterId = item.PartCategoryMasterId;
                }
                else {
                    item.PartCategoryMasterId = "";
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
                header: $("#categoryMasterSearch thead tr th").not(":eq(2)").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
})
//#endregion Print