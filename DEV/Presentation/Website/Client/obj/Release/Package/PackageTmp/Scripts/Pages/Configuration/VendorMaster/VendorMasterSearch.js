var DTvMasterSearchTable;
var run = false;

//#region OnPageLoadJs
var activeStatus = false;
$(document).ready(function () {
    $(document).find('.select2picker').select2({});
    ShowbtnLoaderclass("LoaderDrop");
    $(".actionBar").fadeIn();
    $("#vendormasterGridAction :input").attr("disabled", "disabled");
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
    //#region Load Grid With Status
    var displayState = localStorage.getItem("VENDORMASTERGRIDDISPLAYSTATUS");
    if (displayState)
    {
        if (displayState == "false") {
            $('#vMasterDropdown').val('1').trigger('change.select2');
        }
        else {
            $('#vMasterDropdown').val('2').trigger('change.select2');
        }
        activeStatus = displayState;
    }
    else
    {
        activeStatus = false;
    }
    $('#advsearchsidebar').find('select').val("").trigger('change');
    generatevMasterDataTable();
     //#endregion
});
$(document).on('click', '#sidebarCollapse', function () {
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
function openCity(evt, cityName) {
    evt.preventDefault();
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";

    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
}
//#endregion

//#region Generate VendorMasterGrid func()
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
var titleArray = [];
var classNameArray = [];
function generatevMasterDataTable() {
    var printCounter = 0;
    if ($(document).find('#vMasterSearchTBL').hasClass('dataTable')) {
        DTvMasterSearchTable.destroy();
    }
    DTvMasterSearchTable = $("#vMasterSearchTBL").DataTable({
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
                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: "VendorMaster_Search",
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
                    GridName: "VendorMaster_Search",
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
                title: 'Vendor Master'
            },
            {
                extend: 'print',
                title: 'Vendor Master',
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Vendor Master',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/VendorMaster/GetVendorMasterGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.clientLookupId = LRTrim($('#txtClientLookupId').val());
                d.name = LRTrim($("#txtName").val());
                d.exVendorSiteCode = LRTrim($("#txtExVendorSiteCode").val());
                d.addressCity = LRTrim($('#txtAddressCity').val());
                d.addressState = LRTrim($("#txtAddressState").val());
                d.type = LRTrim($("#txtType").val());
                d.inactiveFlag = activeStatus;
                d.IsExternal = LRTrim($("#IsExternal").val());
            },
            "dataSrc": function (result) {
                HidebtnLoaderclass("LoaderDrop");
                return result.data;
            },
            global: true
        },
        "columns":
        [
            {
                "data": "ClientLookupId",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "className": "text-left",
                "name": "0",
                "mRender": function (data, type, row) {
                    return '<a class=lnk_vendormaster href="javascript:void(0)">' + data + '</a>';
                }
            },
            { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
            { "data": "ExVendorSiteCode", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
            { "data": "AddressCity", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
            { "data": "AddressState", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
            { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
            {
                "data": "IsExternal", "autoWidth": true, "bSearchable": false, "bSortable": false, className: 'text-center', "name": "6",
                "mRender": function (data, type, row) {
                    if (data == true) {
                        return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" checked="checked" class="status" onclick="return false"><span></span></label>';
                    }
                    else {

                        return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" class="status" onclick="return false"><span></span></label>';
                    }

                }
            }
        ],
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function (settings, json) {
            SetPageLengthMenu();
            //----------conditional column hiding-------------//
            var api = new $.fn.dataTable.Api(settings);
            var columns = DTvMasterSearchTable.settings().init().columns;
            var arr = [];
            var titleCols;
            var j = 0;
            while (j < json.hiddenColumnList.length) {
                var clsname = '.' + json.hiddenColumnList[j];
                var title = DTvMasterSearchTable.columns(clsname).header();
                if (clsname == '.ExVendorSiteCode') {
                    titleCols = 'Site Code';
                }
                else if (clsname == '.AddressCity') {
                    titleCols = 'City';
                }
                else if (clsname == '.AddressState') {
                    titleCols = 'State';
                }
                else {
                    titleCols = (title.selector.cols).slice(1);
                }
                titleArray.push(titleCols);
                classNameArray.push(clsname);
                DTvMasterSearchTable.columns(clsname).visible(false);
                //var sortMenuItem = '.dropdown-menu' + ' ' + clsname;
                //$(sortMenuItem).remove();

                //---hide adv search items---
                var advclsname = '.' + "vndm-" + json.hiddenColumnList[j];
                $(document).find(advclsname).hide();
                j++;
            }
            //----------------------------------------------//
            $("#vendormasterGridAction :input").removeAttr("disabled");
            $("#vendormasterGridAction :button").removeClass("disabled");
        }
    });
};

$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var currestsortedcolumn = $('#vMasterSearchTBL').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#vMasterSearchTBL').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#vMasterSearchTBL').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var jsonResult = $.ajax({
                "url": "/VendorMaster/GetVendorMasterPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    colname: colname,
                    coldir: coldir,
                    clientLookupId : LRTrim($('#txtClientLookupId').val()),
                    name : LRTrim($("#txtName").val()),
                    exVendorSiteCode : LRTrim($("#txtExVendorSiteCode").val()),
                    addressCity : LRTrim($('#txtAddressCity').val()),
                    addressState : LRTrim($("#txtAddressState").val()),
                    type : LRTrim($("#txtType").val()),
                    inactiveFlag: activeStatus,
                    IsExternal: LRTrim($("#IsExternal").val())
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#vMasterSearchTBL thead tr th").map(function (key) {
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
                if (item.Name != null) {
                    item.Name = item.Name;
                }
                else {
                    item.Name = "";
                }
                if (item.ExVendorSiteCode != null) {
                    item.ExVendorSiteCode = item.ExVendorSiteCode;
                }
                else {
                    item.ExVendorSiteCode = "";
                }
                if (item.AddressCity != null) {
                    item.AddressCity = item.AddressCity;
                }
                else {
                    item.AddressCity = "";
                }
                if (item.AddressState != null) {
                    item.AddressState = item.AddressState;
                }
                else {
                    item.AddressState = "";
                }
                if (item.Type != null) {
                    item.Type = item.Type;
                }
                else {
                    item.Type = "";
                }
                if (item.IsExternal != null) {
                    if (item.IsExternal == true) {
                        item.IsExternal = getResourceValue("CancelAlertYes");
                    }
                    else if (item.IsExternal == false) {
                        item.IsExternal = getResourceValue("CancelAlertNo");
                    }
                }
                else {
                    item.IsExternal = "";
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
                header: $("#vMasterSearchTBL thead tr th div").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
})
//#endregion

//#region Search
$(document).on('change', '#vMasterDropdown', function () {
run=true;
    var optionVal = $(this).val();
    ShowLoader();
    ShowbtnLoaderclass("LoaderDrop");
   
    if (optionVal == "1") {
        localStorage.setItem("VENDORMASTERGRIDDISPLAYSTATUS", false);
        activeStatus = false;
    }
    else {
        localStorage.setItem("VENDORMASTERGRIDDISPLAYSTATUS", true);
        activeStatus = true;
    }
    DTvMasterSearchTable.page('first').draw('page');
    CloseLoader();
});

var filteritemcount = 0;
$(document).on('click', "#btnvMasterDataAdvSrch", function (e) {
    DTvMasterSearchTable.state.clear();
    var searchitemhtml = "";
    filteritemcount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val() && $(this).val() != "0") {
            filteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossSODL" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('#sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    GridAdvanceSearch();
});
function GridAdvanceSearch() {
    run=true;
    DTvMasterSearchTable.page('first').draw('page');
    $('.filteritemcount').text(filteritemcount);
}
$(document).on('click', '.btnCrossSODL', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    filteritemcount--;
    GridAdvanceSearch();
});
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    clearAdvanceSearch();
    DTvMasterSearchTable.page('first').draw('page');
});
function clearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    $('#advsearchsidebar').find('select').val('').trigger('change');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
//#endregion AdvSearch

//#region btn_click

$(document).on('click', '.lnk_vendormaster', function (e) {
    e.preventDefault();
    var index_row = $('#vMasterSearchTBL tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = DTvMasterSearchTable.row(row).data();
    var vendorMasterId = data.VendorMasterId;
    $.ajax({
        url: "/VendorMaster/VendorMasterDetails",
        type: "GET",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { vendorMasterId: vendorMasterId },
        success: function (data) {
            $('#rendervendormaster').html(data);
        },
        complete: function (data) {
            CloseLoader();
        },
        error: function (xhr, error) {
        }
    });
});
$(document).on('click', ".btnEditVendorMaster", function () {
    var vMasterId = $(document).find('#changeVendorModel_VendorMasterId').val();
    $.ajax({
        url: "/VendorMaster/EditVendorMaster",
        type: "GET",
        dataType: 'html',
        data: { vendorMasterId: vMasterId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendervendormaster').html(data);
        },
        complete: function () {
            SetVendorMasterControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', ".addPlusbtnVendor", function () {
    $.ajax({
        url: "/VendorMaster/AddVendorMaster",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendervendormaster').html(data);
        },
        complete: function () {
            SetVendorMasterControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});

//#endregion

//#region Gear funC()
$(document).on('click', '#vMasterSearchTBL_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#vMasterSearchTBL_paginate .searchdt-menu', function () {
    run = true;
});
$(document).on('click', '#liCustomize', function () {
    //funCustomizeBtnClick(DTvMasterSearchTable,true);
    funCustomizeBtnClick(DTvMasterSearchTable, true, titleArray);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(DTvMasterSearchTable, colOrder);
    run = true;
    DTvMasterSearchTable.state.save(run);
    if (classNameArray != null && classNameArray.length > 0) {
        var j = 0;
        while (j < classNameArray.length) {
            DTvMasterSearchTable.columns(classNameArray[j]).visible(false);
            j++;
        }
    }
});
//#endregion

//#region On-Success
function ChangeVendorOnSuccess(data) {
    CloseLoader();
    if (data.Issuccess) {
        $('#changeVendorIdPage').modal('hide');
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("VendorIDUpdateAlert");
         swal(SuccessAlertSetting, function () {
            //$("#vendorMasterModel_ClientLookupId").text(data.ClientLookUpId);
             RedirectToDetail($('#changeVendorModel_VendorMasterId').val(), "");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

function OnVendorMasterAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.Command == "save") {
            if (data.mode == "add")
            {
                SuccessAlertSetting.text = getResourceValue("VendorMasterAddAlert");
            }
            else
            {
                SuccessAlertSetting.text = getResourceValue("VendorMasterUpdateAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToDetail(data.VendorMasterId, "");
            });
        }
        else {
            SuccessAlertSetting.text = "Vendor Master added successfully.";
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                if (data.mode == "add")
                {
                    $(document).find('form').trigger("reset");
                    $(document).find('form').find("select").val("").trigger('change');
                    $(document).find('form').find("#accountDetails_ClientLookupId").removeClass("input-validation-error");
                    $(document).find('form').find("#accountDetails_Name").removeClass("input-validation-error");
                } else {
                    $.ajax({
                        url: "/VendorMaster/AddVendorMaster",
                        type: "GET",
                        dataType: 'html',
                        beforeSend: function () {
                            ShowLoader();
                        },
                        success: function (data) {
                            $('#rendervendormaster').html(data);
                        },
                        complete: function () {
                            SetVendorMasterControl();
                        },
                        error: function () {
                            CloseLoader();
                        }
                    });
                }
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

//#endregion

//#region Common
function SetVendorMasterControl() {
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
$(document).on('click', "#brdVendor", function () {
    var vMasterId = $(this).attr('data-val');
    RedirectToDetail(vMasterId);
});
$(document).on('click', "#btnCancelEditVendor", function () {
    var vMasterId = $(this).attr('data-val');
    swal(CancelAlertSetting, function () {
        RedirectToDetail(vMasterId);
    });
});
$(document).on('click', "#btnCancelAddVendor", function () {
    var vMasterId = $(this).attr('data-val');
    swal(CancelAlertSetting, function () {
        window.location.href = "../VendorMaster/Index?page=Masters_Vendor_Vendor_Master";
    });
});
function RedirectToDetail(vendorMasterId) {
    $.ajax({
        url: "/VendorMaster/VendorMasterDetails",
        type: "GET",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { vendorMasterId: vendorMasterId },
        success: function (data) {
            $('#rendervendormaster').html(data);
        },
        complete: function (data) {
            CloseLoader();
        },
        error: function (xhr, error) {
        }
    });
}
//#endregion


$(document).on('click', '#btncreatedlastupdated', function () {
    var vMasterId = $(document).find('#changeVendorModel_VendorMasterId').val();
       $.ajax({
           url: '/VendorMaster/CreateandLastUpdate',
            type: "GET",
            datatype: "json",
            data: { vendorMasterId: vMasterId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#lblusercreateBydP').text(data.Createby);
                $('#lblcreateDateP').text(data.CreateDate);
                $('#lbluserupdatedByP').text(data.ModifyBy);
                $('#lblupdateDateP').text(data.ModifyDate);
            },
            complete: function () {
                $(document).find('#createdlastupdated').modal('show');
                CloseLoader();
            }
        });
    
});

$(document).on('change', '#vendorMasterModel_RemitUseBusiness', function () {
   
    if ($(this).is(':checked')) {
        $('#vendorMasterModel_RemitAddress1').val($('#vendorMasterModel_Address1').val()).attr("disabled",true);
        $('#vendorMasterModel_RemitAddress2').val($('#vendorMasterModel_Address2').val()).attr("disabled", true);
        $('#vendorMasterModel_RemitAddress3').val($('#vendorMasterModel_Address3').val()).attr("disabled", true);
        $('#vendorMasterModel_RemitAddressCity').val($('#vendorMasterModel_AddressCity').val()).attr("disabled", true);
        $('#vendorMasterModel_RemitAddressState').val($('#vendorMasterModel_AddressState').val()).attr("disabled", true);
        $('#vendorMasterModel_RemitAddressPostCode').val($('#vendorMasterModel_AddressPostCode').val()).attr("disabled", true);
        $('#vendorMasterModel_RemitAddressCountry').val($('#vendorMasterModel_AddressCountry').val()).attr("disabled", true);
    }
    else {
        $('#vendorMasterModel_RemitAddress1').val("").removeAttr("disabled", false);
        $('#vendorMasterModel_RemitAddress2').val("").removeAttr("disabled", false);
        $('#vendorMasterModel_RemitAddress3').val("").removeAttr("disabled", false);
        $('#vendorMasterModel_RemitAddressCity').val("").removeAttr("disabled", false);
        $('#vendorMasterModel_RemitAddressState').val("").removeAttr("disabled", false);
        $('#vendorMasterModel_RemitAddressPostCode').val("").removeAttr("disabled", false);
        $('#vendorMasterModel_RemitAddressCountry').val("").removeAttr("disabled", false);
    }
});

