var DTaccountSearchTable;
var DTnoteTable;
var DTattachmentTable;
var run = false;
var clientLookupId;
var name;
var inactiveFlag;
var siteid;
//#region OnPageLoadJs
$(document).ready(function () {
  
    if (localStorage.getItem("sitevalue") == null || localStorage.getItem("sitevalue") == 0) {
        siteid = 0
        $("#ddlsitename").val("");
    }
    else {
        siteid = localStorage.getItem("sitevalue");
        $("#ddlsitename").val(localStorage.getItem("sitevalue"));
    }
    $(document).find('.select2picker').select2({});
    $(document).on('click', "#action", function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionBar").fadeIn();
    $("#AccountGridAction :input").attr("disabled", "disabled");
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $(document).on('focusout', "#action", function () {
        $(".actionDrop").fadeOut();
    });
    $(document).find("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '#dismiss, .overlay', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', '#drpDwnLink', function (e) {
        e.preventDefault();
        $(document).find("#drpDwn").slideToggle();
    });
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
    $('#advsearchsidebar').find('select').val("").trigger('change');
    generateAccountDataTable();
});
$(document).find('#sidebarCollapse').on('click', function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$(document).on('click', "ul.vtabs li", function () {
    $("ul.vtabs li").removeClass("active");
    $(this).addClass("active");
    $(".tabsArea").hide();
    var activeTab = $(this).find("a").attr("href");
    $(activeTab).fadeIn();
    return false;
});
function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "Notes":
            GenerateNoteGrid();
            break;
        case "Attachment":
            GenerateAttachmentGrid();
            break;
    }
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

//#region Generate Account func()
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
function generateAccountDataTable() {
    
    var printCounter = 0;
    if ($(document).find('#accountSearchTBL').hasClass('dataTable')) {
        DTaccountSearchTable.destroy();
    }
    DTaccountSearchTable = $("#accountSearchTBL").DataTable({
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
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "Account_Search",
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
                "url": gridStateLoadUrl,
                "data": {
                    GridName: "Account_Search",
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    if (json.LayoutInfo !== '') {
                        callback(json);
                        if (json.FilterInfo !== '') {
                            setsearchui(json.FilterInfo, $(".filteritemcount"), $("#advsearchfilteritems"));
                        }
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

            'excelHtml5',
            {
                extend: 'print'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Accounts List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                css: 'display:none',
                title: 'Accounts List',
                orientation: 'landscape',
                pageSize: 'A4'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Account/GetAccountGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.ClientLookupId = LRTrim($("#txtAccountNumber").val());
                d.Name = LRTrim($("#txtName").val());
                d.Siteid = siteid;
                d.InactiveFlag = LRTrim($("#isInactive").val());
                d.IsExternal = LRTrim($("#isExternal").val());
            },
            "dataSrc": function (result) {
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
                "data": "ClientLookupId",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "name": "0",
                "mRender": function (data, type, row) {
                    return '<a class=lnk_accountdetails href="javascript:void(0)">' + data + '</a>';
                }
            },
            { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1", "className": "text-left", },
            { "data": "SiteName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2", "className": "text-left", "visible": false, },
            { "data": "InactiveFlag", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3", "className": "text-center", 
                "mRender": function (data, type, row) {
                    if (data == false) {
                        return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" class="status" onclick="return false"><span></span></label>';
                    }
                    else {
                        return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" checked="checked" class="status" onclick="return false"><span></span></label>';
                    }
                }
            },
     
            {
                "data": "IsExternal", "autoWidth": true, "bSearchable": true, "bSortable": false, className: 'text-center', "name": "4",
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
            },
        ],
        
        initComplete: function (settings, json) {
            SetPageLengthMenu();
            //----------conditional column hiding-------------//
            var api = new $.fn.dataTable.Api(settings);
            var columns = DTaccountSearchTable.settings().init().columns;
            var arr = [];
            var j = 0;
            if (json.AllowSiteNameColumn == true) {
                DTaccountSearchTable.column(2).visible(true);
            }
            else {
                DTaccountSearchTable.column(2).visible(false);
            }
            while (j < json.hiddenColumnList.length) {
                var clsname = '.' + json.hiddenColumnList[j];
                DTaccountSearchTable.columns(clsname).visible(false);
                var sortMenuItem = '.dropdown-menu' + ' ' + clsname;
                $(sortMenuItem).remove();

                //---hide adv search items---
                var advclsname = '.' + "acc-" + json.hiddenColumnList[j];
                $(document).find(advclsname).hide();
                j++;
            }
          
            $("#AccountGridAction :input").removeAttr("disabled");
            $("#AccountGridAction :button").removeClass("disabled");
            DisableExportButton($("#accountSearchTBL"), $(document).find('.import-export'));
        }
    });
};

function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
function getfilterinfoarray(advsearchcontainer) {
    var filterinfoarray = [];
    advsearchcontainer.find('.adv-item').each(function (index, item) {
        if ($(this).parent('div').is(":visible")) {
            f = new filterinfo($(this).attr('id'), $(this).val());
            filterinfoarray.push(f);

            if ($(this).parent('div').find('div').hasClass('range-timeperiod')) {
                if ($(this).parent('div').find('input').val() !== '' && $(this).val() == '10') {
                    f = new filterinfo('this-' + $(this).attr('id'), $(this).parent('div').find('input').val());
                    filterinfoarray.push(f);
                }
            }

        }
    });
    return filterinfoarray;
}
function setsearchui(data, advcountercontainer, searchstringcontainer) {
    var searchitemhtml = '';
    $.each(data, function (index, item) {
        if ($('#' + item.key).parent('div').is(":visible")) {
            $('#' + item.key).val(item.value).trigger('change');
            if (item.value) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }
        advcountercontainer.text(selectCount);
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
}

$(function () {
    
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            dtTable = $("#accountSearchTBL").DataTable();
            var currestsortedcolumn = $('#accountSearchTBL').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#accountSearchTBL').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#accountSearchTBL').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var jsonResult = $.ajax({
                "url": "/Account/GetAccountPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    ClientLookupId: LRTrim($("#txtAccountNumber").val()),
                    Name: LRTrim($("#txtName").val()),
                    Siteid:siteid,
                    InactiveFlag: LRTrim($("#isInactive").val()),
                    isExternal: LRTrim($("#isExternal").val()),
                    colname: colname,
                    coldir: coldir
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#accountSearchTBL thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.AccountNumber != null) {
                    item.AccountNumber = item.AccountNumber;
                }
                else {
                    item.AccountNumber = "";
                }
                if (item.Name != null) {
                    item.Name = item.Name;
                }
                else {
                    item.Name = "";
                }

                if (item.SiteName != null) {
                    item.SiteName = item.SiteName;
                }
                else {
                    item.SiteName = "";
                }
            
                if (item.InactiveFlag != null) {
                    if (item.InactiveFlag == true) {
                        item.InactiveFlag = getResourceValue("CancelAlertYes");
                    }
                    else if (item.InactiveFlag == false) {
                        item.InactiveFlag = getResourceValue("CancelAlertNo");
                    }
                }
                else {
                    item.InactiveFlag = "";
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
                    item.isExternal = "";
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
                header: $("#accountSearchTBL thead tr th").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
})
//#endregion

//#region Search
$('#btntextSearch').on('click', function () {
    run = true;
    clearAdvanceSearch();
    DTaccountSearchTable.page('first').draw('page');
});

$("#ddlsitename").change(function () {
    
    run = true;
    siteid = $(this).val();
    if (siteid == "")
    {
        siteid = 0;
    }
    localStorage.setItem("sitevalue", siteid);
    DTaccountSearchTable.page('first').draw('page');
    
});
$("#btnAccountDataAdvSrch").on('click', function (e) {
    run = true;
    AcountAdvSearch();
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    DTaccountSearchTable.page('first').draw('page');
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    AcountAdvSearch();
    DTaccountSearchTable.page('first').draw('page');
});
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    $("#txtsearchbox").val("");
    clearAdvanceSearch();
    DTaccountSearchTable.page('first').draw('page');
});
function clearAdvanceSearch() {
    selectCount = 0;
    $('#advsearchsidebar').find('select').val("").trigger('change');
    $(document).find('#txtAccountNumber').val("");
    $(document).find('#txtName').val("");
    $(".filteritemcount").text(selectCount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
function AcountAdvSearch() {
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#advsearchfilteritems").html(searchitemhtml);
    $(".filteritemcount").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}
$(document).on('click', '.lnk_accountdetails', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = DTaccountSearchTable.row(row).data();
    var accountId = data.AccountId;
    $.ajax({
        url: "/Account/AccountDetail",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { AccountId: accountId },
        success: function (data) {
            $('#renderAccountPage').html(data);
            if ($(document).find('#addPlusbtnAccount').length === 0) { $(document).find('#accsearchactiondiv').css('margin-right', '0px'); }
        },
        complete: function () {
            CloseLoader();
        }
    });
});
//#endregion

//#region Notes
function GenerateNoteGrid() {
    var rCount = 0;
    var accId = $('#accountDetails_AccountId').val();
    if ($(document).find('#notesTable').hasClass('dataTable')) {
        DTnoteTable.destroy();
    }
    DTnoteTable = $("#notesTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Account/PopulateNotes",
            "data": { accountId: accId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                if (rCount > 0) {
                    $(document).find("#btnAddNote").hide();
                }
                else {
                    $(document).find("#btnAddNote").show();
                }
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [3], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-primary addNoteBttn gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                        '<a class="btn btn-outline-success editNoteBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delNoteBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
        [
            { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "ModifiedDate",
                "type": "date "
            },
            {
                "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}

$(document).on('click', ".editNoteBttn", function () {
    var accId = $('#accountDetails_AccountId').val();
    var ClientLookupId = $('#accountDetails_ClientLookupId').val();
    var data = DTnoteTable.row($(this).parents('tr')).data();
    $.ajax({
        url: "/Account/AddOrEditNotes",
        type: "GET",
        dataType: 'html',
        data: { NoteId: data.NotesId, AccountId: accId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderAccountPage').html(data);
        },
        complete: function () {
            CloseLoader();
            SetAccountEnvironmentPage();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', ".addNoteBttn,#btnAddNote", function () {
    var accId = $('#accountDetails_AccountId').val();
    var ClientLookupId = $('#accountDetails_ClientLookupId').val();
    $.ajax({
        url: "/Account/AddOrEditNotes",
        type: "GET",
        dataType: 'html',
        data: { AccountId: accId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderAccountPage').html(data);
        },
        complete: function () {
            CloseLoader();
            SetAccountEnvironmentPage();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#brdumnotes", function () {
    var userInfoId = $(this).attr('data-val');
    RedirectToDetail(userInfoId, "Notes");
});
$(document).on('click', "#btnumnotescancel", function () {
    var accID = $('#noteModel_AccountID').val();
    swal(CancelAlertSetting, function () {
        RedirectToDetail(accID, "Notes");
    });
});
function AddOnSuccessNotes(data) {
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("NotesAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("NotesUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToDetail(data.accountID, "Notes");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
$(document).on('click', '.delNoteBttn', function () {

    var data = DTnoteTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        DeleteNote(data);
    });
});
function DeleteNote(data) {
    $.ajax({
        url: '/Account/DeleteNote',
        data: {
            noteId: data.NotesId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                DTnoteTable.state.clear();
                ShowDeleteAlert(getResourceValue("NoteDeleteAlert"));
                GenerateNoteGrid();
            }
        },
        complete: function () {
            CloseLoader();
        }
    });
}
//#endregion

//#region Attachment

function GenerateAttachmentGrid() {
    var accId = $('#accountDetails_AccountId').val();
    if ($(document).find('#attachTable').hasClass('dataTable')) {
        DTattachmentTable.destroy();
    }
    DTattachmentTable = $("#attachTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Account/PopulateAttachment",
            "data": { accountId: accId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [5], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-danger delattachment gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            },
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        "columns":
        [
            { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "FullName",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<a class=lnk_download_attachment href="' + '/PreventiveMaintenance/DowloadPmAttachment?_fileinfoId=' + row.FileInfoId + '"  target="_blank">' + row.FullName + '</a>'
                }
            },

            { "data": "FileSizeWithUnit", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "CreateDate",
                "type": "date "
            },
            {
                "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}

$(document).on('click', "#btnAddAttachment", function () {
    var accId = $('#accountDetails_AccountId').val();
    var ClientLookupId = $('#accountDetails_ClientLookupId').val();
    $.ajax({
        url: "/Account/AddAttachments",
        type: "GET",
        dataType: 'html',
        data: { AccountId: accId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderAccountPage').html(data);
        },
        complete: function () {
            CloseLoader();
            SetAccountEnvironmentPage();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function AttachmentAddOnSuccess(data) {
    var accountID = data.accountID;
    if (data.Result == "success") {
        var message;
        SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
        swal(SuccessAlertSetting, function () {
            RedirectToDetail(accountID, "Attachment");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
$(document).on('click', '.lnk_download_attachment', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = DTattachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/Account/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {                
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });
});
$(document).on('click', '.delattachment', function () {
    var data = DTattachmentTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Account/DeleteAttachments',
            data: {
                fileAttachmentId: data.FileAttachmentId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    DTattachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("assignmentDeleteSuccessAlert"));
                    GenerateAttachmentGrid();
                }
                else
                {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
});
$(document).on('click', "#btnAttachcancel", function () {
    var accID = $('#attachmentModel_AccountID').val();

    swal(CancelAlertSetting, function () {
        RedirectToDetail(accID, "Attachment");
    });
});
$(document).on('click', "#brdAttach", function () {
    var userInfoId = $(this).attr('data-val');
    RedirectToDetail(userInfoId, "Attachment");
});
//#endregion

//#region Common
function SetAccountEnvironmentPage() {
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if ($(this).valid()) {
            if (typeof areaddescribedby !== 'undefined') {
                $('#' + areaddescribedby).hide();
            }
        }
        else {
            if (typeof areaddescribedby !== 'undefined') {
                $('#' + areaddescribedby).show();
            }
        }
    });
    $(document).find('.select2picker').select2({});
}
function RedirectToDetail(accID, mode) {
    $.ajax({
        url: "/Account/AccountDetail",
        type: "POST",
        dataType: 'html',
        data: { AccountId: accID },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderAccountPage').html(data);
        },
        complete: function () {
            CloseLoader();
            if (mode === "Notes") {
                $('#linote').trigger('click');
                $('#colorselector').val('Notes');
                GenerateNoteGrid();
            }
            if (mode === "Attachment") {
                $('#liattachment').trigger('click');
                $('#colorselector').val('Attachment');
                GenerateAttachmentGrid();
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion

//#region UiConfig Custom Validator
function UIConfigVal(ViewName, ColumnName, Required, Hide, Disable) {
    this.ViewName = ViewName;
    this.ColumnName = ColumnName;
    this.Required = Required;
    this.Hide = Hide;
    this.Disable = Disable;
}
var validateControls = [];
function getAllViews(viewname, isextern) {    
    $.ajax({
        url: "/Base/UiConfigAllColumns",
        type: "POST",
        dataType: "json",
        async: false,
        beforeSend: function () {
            ShowLoader();
        },
        data: { viewName: viewname, isExternal: isextern },
        success: function (data) {            
            $.each(data.vList, function (index, value) {
                var UIConfigValobj = new UIConfigVal(value.ViewName, value.ColumnName, value.Required, value.Hide, value.Disable);
                validateControls.push(UIConfigValobj);
            });
        },
        complete: function () {
            CloseLoader();
        }
    });
}
//#endregion
//#region Account
$(document).on('click', "#editaccount", function () {
    var accId = $('#accountDetails_AccountId').val();
    $.ajax({
        url: "/Account/AddOrEditAccount",
        type: "GET",
        dataType: 'html',
        data: { AccountId: accId, IsAddFromDetails: false },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderAccountPage').html(data);
        },
        complete: function () {
            CloseLoader();
            SetAccountEnvironmentPage();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#addPlusbtnAccount,#addAccountFromIndex", function () {
    var accId = $('#accountDetails_AccountId').val();
    $.ajax({
        url: "/Account/AddOrEditAccount",
        type: "GET",
        dataType: 'html',
        data: { AccountId: accId, IsAddFromDetails: true },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderAccountPage').html(data);
        },
        complete: function () {
            CloseLoader();
            SetAccountEnvironmentPage();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btnCancelAccount", function () {
    var AccountId = $(document).find('#accountDetails_AccountId').val();
    var IsAddFromIndex = $(document).find('#accountDetails_IsAddFromIndex').val();
    swal(CancelAlertSetting, function () {
        if (AccountId == 0) {
            window.location.href = "/Account/Index?page=Account";
        }
        else {
            RedirectToDetail(AccountId, "");
        }
    });
});
$(document).on('click', "#brdAccount", function () {
    var accountId = $(this).attr('data-val');
    RedirectToDetail(accountId, "");
});
function OnAccountAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.Command == "save") {
            if (data.mode == "add") {
                SuccessAlertSetting.text = getResourceValue("AccountAddAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("AccountUpdateAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToDetail(data.AccountID, "");
            });
        }
        else {
            message = getResourceValue("AccountAddAlert");
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change');
                $(document).find('form').find("#accountDetails_Name").removeClass("input-validation-error");
                $(document).find('form').find("#accountDetails_ClientLookupId").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function ChangeAccountOnSuccess(data) {
    CloseLoader();
    if (data.Issuccess) {
        $('#changeAccountPage').modal('hide');
        $('.modal-backdrop').remove();
        var message = getResourceValue("AccountIdChangedAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToDetail(data.AccountId, "");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion


$(document).on('click', '#ChangeAccountId', function () {
    $(document).find('#changeAccountPage').modal('show');
});
$(document).on('click', '#changeacoountactiveinactive', function () {
    var accountid = $('#accountDetails_AccountId').val();
    var InActiveFlag = $(document).find('#accounthiddeninactiveflag').val();
    $.ajax({
        url: "/Account/ValidateForActiveInactive",
        type: "POST",
        dataType: "json",
        data: { InActiveFlag: InActiveFlag, AccountId: accountid },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.validationStatus == true) {
                if (InActiveFlag == "True") {
                    CancelAlertSetting.text = getResourceValue("AccountActivateAlert");
                }
                else {
                    CancelAlertSetting.text = getResourceValue("AccountInactiveAlert");
                }
                swal(CancelAlertSetting, function (isConfirm) {
                    if (isConfirm == true) {
                        var updateindex = $(document).find('#accounthiddenupdateindex').val();
                        $.ajax({
                            url: "/Account/MakeActiveInactive",
                            type: "POST",
                            dataType: "json",
                            data: { InActiveFlag: InActiveFlag, AccountId: accountid, UpdateIndex: updateindex },
                            beforeSend: function () {
                                ShowLoader();
                            },
                            success: function (data) {

                                if (data.Result == 'success') {
                                    if (InActiveFlag == "True") {
                                        SuccessAlertSetting.text = getResourceValue("AccountActivateSuccessAlert");
                                    }
                                    else {
                                        SuccessAlertSetting.text = getResourceValue("AccountInactivateSuccessAlert");
                                    }
                                    swal(SuccessAlertSetting, function () {
                                        RedirectToDetail(accountid, "");
                                    });
                                }
                                else {
                                    ShowGenericErrorOnAddUpdate(data);
                                }
                            },
                            complete: function () {
                                CloseLoader();
                            },
                            error: function () {
                                CloseLoader();
                            }
                        });
                    }
                });
            }
            else {
                GenericSweetAlertMethod(data);
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function (jqxhr) {
            CloseLoader();
        }
    });
});

$(document).on('click', '#accountSearchTBL_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#accountSearchTBL_length .searchdt-menu', function () {
    run = true;
});

