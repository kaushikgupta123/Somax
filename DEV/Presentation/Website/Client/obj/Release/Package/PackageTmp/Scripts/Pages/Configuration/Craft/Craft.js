var run = false;
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

$(document).ready(function () {
    $(".actionBar").fadeIn();
    $("#CraftGridAction :input").attr("disabled", "disabled");
});

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
    $(document).find('.select2picker').select2({});
    $('#txtInactiveFlag').val('').trigger('change.select2');
    GenerateCraftGrid();
});
//#region Craft Search
var dtcraftSearch;
var selectCount = 0;
function GenerateCraftGrid() {
    var rCount = 0;
    var printCounter = 0;
    if ($(document).find('#tblcraftSearch').hasClass('dataTable')) {
        dtcraftSearch.destroy();
    }

    dtcraftSearch = $("#tblcraftSearch").DataTable({
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
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "Craft_Search",
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
                "url": gridStateLoadUrl,
                "data": {
                    GridName: "Craft_Search",
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
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Craft List'
            },
            {
                extend: 'print',
                title: 'Craft List',
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Craft List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                title: 'Craft List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Craft/GetCraftGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.srcData = LRTrim($("#txtsearchbox").val());
                d.ClientLookUpId = LRTrim($("#txtCraft").val());
                d.Description = LRTrim($("#txtDescription").val());
                d.ChargeRate = LRTrim($("#txtRate").val());
                d.InactiveFlag = $('#txtInactiveFlag').val();
            },
            "dataSrc": function (result) {
                if (result.data.length == "0") {
                    $(document).find('.import-export').attr('disabled', 'disabled');
                }
                else {
                    $(document).find('.import-export').removeAttr('disabled');
                }

                rCount = result.data.length;
                return result.data;
            },
            global: true
        },

        "columns":
        [
                {
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-left", "name": "0", "mRender": function (data, type, row) {
                        return '<a class=lnk_craftdetails href="javascript:void(0)">' + data + '</a>';
                    } },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap'>" + data + "</div>";
                }
            },
            { "data": "ChargeRate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
            {
                "data": "InactiveFlag", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date", "className": "text-center", "name": "3",
                "mRender": function (data, type, row) {
                    if (data == true) {
                        return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" class="disabled" checked="checked"><span></span></label>';
                    }
                    else {
                        return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" class="disabled"><span></span></label>';
                    }
                }
            },
            //{ "bSortable": false, "className": "text-center" }
        ],
        //columnDefs: [
        //    {
        //        targets: [4], render: function (a, b, data, d) {
        //            return '<a class="btn btn-outline-success editcraft gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                       
                        
        //        }
        //    }
        //],
        initComplete: function () {
            $(document).on('click', '.disabled', function (e) {
                e.preventDefault();
            });
            if (rCount > 0) {
                $("#btnaddcraft").hide();
            }
            else {
                $("#btnaddcraft").show();
            }
            SetPageLengthMenu();
            $("#CraftGridAction :input").removeAttr("disabled");
            $("#CraftGridAction :button").removeClass("disabled");
            DisableExportButton($("#tblcraftSearch"), $(document).find('.import-export'));
        }
    });
}
$('#btntextSearch').on('click', function () {
    run = true;
    clearAdvanceSearch();
    dtcraftSearch.page('first').draw('page');
});
$(document).on('click', '#btnCraftAdvSrch', function () {
    run = true;
    $('#txtsearchbox').val('');
    AWBAdvSearch();
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    dtcraftSearch.page('first').draw('page');
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    AWBAdvSearch();
    dtcraftSearch.page('first').draw('page');
});
function AWBAdvSearch() {
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
    $("#advsearchfilteritems").html(searchitemhtml);
    $(".filteritemcount").text(selectCount);
}
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    $('#txtsearchbox').val('');
    clearAdvanceSearch();
    dtcraftSearch.page('first').draw('page');
});
function clearAdvanceSearch() {
    $('#advsearchsidebar').find('input:text').val('');
    $('#advsearchsidebar').find("select").val("").trigger('change');
    selectCount = 0;
    $(".filteritemcount").text(selectCount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
};
//#endregion
//#region AddEdit
$(document).on('click', ".addPlusbtnCraft,.addCraftFromIndex", function () {
    $.ajax({
        url: "/Craft/AddCraft",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderCraft').html(data);
        },
        complete: function () {
            SetUIControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});

$(document).on('click', '.lnk_craftdetails', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = dtcraftSearch.row(row).data();
    var craftid = data.CraftId;
    $.ajax({
        url: "/Craft/CraftDetails",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { CraftId: craftid },

        success: function (data) {
            $('#renderCraft').html(data);
        },
        complete: function () {
            CloseLoader();
            SetUIControls();
        },
        error: function () {
            CloseLoader();
        }
    });   

    
});


$(document).on('click', '#editcraft ', function () {
    
    var craftid = $('#crafthiddenid').val();   
    $.ajax({
        url: "/Craft/EditCraft",
        type: "GET",
        dataType: 'html',
        data: { craftID: craftid },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderCraft').html(data);
        },
        complete: function () {
            SetUIControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function CraftAddEditOnSuccess(data) {
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("CraftAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("CraftUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToCraftSearch();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#btncancel', function () {
    swal(CancelAlertSetting, function () {
        RedirectToCraftSearch();
    });
});
function RedirectToCraftSearch() {
    window.location.href = "/Craft/Index?page=Crafts";
}
//#endregion
//#region Delete
$(document).on('click', '.delcraft', function () {
    run = true;
    var data = dtcraftSearch.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        var CraftId = data.CraftId;
        DeleteCraft(CraftId);
    });
});
function DeleteCraft(CraftId) {  
    run = true;
    $.ajax({
        url: '/craft/DeleteCraft',
        data: {
            CraftId: CraftId
        },
        beforeSend: function () {
            ShowLoader();
        },
        type: "post",
        datatype: "json",
        success: function (data) {
            ShowDeleteAlert(data[0]);
        },
        complete: function () {
            CloseLoader();
            dtcraftSearch.page('first').draw('page');
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion
//#region Commom
function SetUIControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $(document).find('.select2picker').select2({});
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
}
//#endregion

//#region Print
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var currestsortedcolumn = $('#tblcraftSearch').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#tblcraftSearch').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#tblcraftSearch').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var valCraft = LRTrim($('#txtCraft').val());
            var valDescription = LRTrim($('#txtDescription').val());
            var valRate = LRTrim($('#txtRate').val());
            var valInactive = LRTrim($("#txtInactiveFlag").val())
            dtTable = $("#tblcraftSearch").DataTable();
            var currestsortedcolumn = $('#tblcraftSearch').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#tblcraftSearch').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#tblcraftSearch').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var jsonResult = $.ajax({
                "url": "/Craft/GetCraftPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    colname: colname,
                    coldir: coldir,
                    _craft: valCraft,
                    _description: valDescription,
                    _rate: valRate,
                    _inactiveFlag: valInactive,
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#tblcraftSearch thead tr th").not(':eq(4)').map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.Craft != null) {
                    item.Craft = item.Craft;
                }
                else {
                    item.Craft = "";
                }
                if (item.Description != null) {
                    item.Description = item.Description;
                }
                else {
                    item.Description = "";
                }
                if (item.Rate != null) {
                    item.Rate = item.Rate;
                }
                else {
                    item.Rate = "";
                }
                if (item.Inactive != null) {
                    item.Inactive = item.Inactive;
                }
                else {
                    item.Inactive = "";
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
                header: $("#tblcraftSearch thead tr th").not(":eq(4)").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
})
//#endregion Print


$(document).on('click', '#tblcraftSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#tblcraftSearch_paginate .searchdt-menu', function () {
    run = true;
});