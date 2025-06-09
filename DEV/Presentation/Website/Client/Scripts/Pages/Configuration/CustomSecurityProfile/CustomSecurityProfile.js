var selectCount = 0;
var selectedcount = 0;
var totalcount = 0;
var searchcount = 0;
var run = false;
var orderbycol = 0;
var orderDir = 'asc';
var dtSecurityProfile;
var gridname = "CustomSecurityProfile_Search";

$(document).ready(function () {
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });   
    generateCustomSecurityProfileDataTable();
});
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
//#region for search grid
function generateCustomSecurityProfileDataTable() {
    var printCounter = 0;
    if ($(document).find('#tblcustomsecurityprofile').hasClass('dataTable')) {
        dtSecurityProfile.destroy();
    }
    dtSecurityProfile = $("#tblcustomsecurityprofile").DataTable({
        colReorder: {
            fixedColumnsLeft: 1
        },
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        "stateSave": true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order)
                    data.order[0][0] = $(document).find('.srtspcolumn.sort-active').data('col');
                //Search Retention
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
                //Search Retention
            }
            run = false;
        },
        "stateLoadCallback": function (settings, callback) {

            //Search Retention
            $.ajax({
                "url": "/Base/GetLayout",
                "data": {
                    GridName: gridname
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    if (json.LayoutInfo) {
                        callback(JSON.parse(json.LayoutInfo));
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        orderbycol = LayoutInfo.order[0][0];
                        orderDir = LayoutInfo.order[0][1];
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#dvFilterSearchSelect2"));
                        }
                    }
                    else {
                        callback(json);
                    }
                }
            });
            //Search Retention
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
                title: 'Custom Security Profile List'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Custom Security Profile List',
                extension: '.csv'
            },
            {
                extend: 'print',
                title: 'Custom Security Profile List'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Custom Security Profile List'
                //orientation: 'landscape',
               // pageSize: 'A4'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/CustomSecurityProfile/GetCustomSecurityProfileGridData",
            "type": "post",
            "datatype": "json",
            cache: false,
            data: function (d) {
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.Name = LRTrim($(document).find('#txtName').val());
                d.Description = LRTrim($(document).find('#txtDescription').val());
            },
            "dataSrc": function (result) {
                searchcount = result.recordsTotal;
                if (result.data.length < 1) {
                    $(document).find('#btnCustomSecurityProfileExport').prop('disabled', true);
                }
                else {
                    $(document).find('#btnCustomSecurityProfileExport').prop('disabled', false);
                }
                if (totalcount < result.recordsTotal)
                    totalcount = result.recordsTotal;
                if (totalcount != result.recordsTotal)
                    selectedcount = result.recordsTotal;

                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                return result.data;
            }
        },
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
       
        "columns":
            [
                {
                    "data": "SecurityProfileId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": false,
                    "className": "text-left",
                    "name": "0",
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_onsecDetails href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "1",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap'>" + data + "</div>";
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "2",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap'>" + data + "</div>";
                    }
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            orderbycol = $('#tblcustomsecurityprofile').dataTable().fnSettings().aaSorting[0][0];
            orderDir = $('#tblcustomsecurityprofile').dataTable().fnSettings().aaSorting[0][1];
            switch (orderbycol) {
                case 0:
                    $(document).find('.srtspcolumn').eq(0).addClass('sort-active');
                    sorttext = $(document).find('.srtspcolumn').eq(0).text();
                    break;
                case 1:
                    $(document).find('.srtspcolumn').eq(1).addClass('sort-active');
                    sorttext = $(document).find('.srtspcolumn').eq(1).text();
                    break;
                case 2:
                    $(document).find('.srtspcolumn').eq(2).addClass('sort-active');
                    sorttext = $(document).find('.srtspcolumn').eq(2).text();
                    break;
                default:
                    $(document).find('.srtspcolumn').eq(0).addClass('sort-active');
                    sorttext = $(document).find('.srtspcolumn').eq(0).text();
                    break;
            }
            switch (orderDir) {
                case "asc":
                    $(document).find('.srtsporder').eq(0).addClass('sort-active');
                    break;
                case "desc":
                    $(document).find('.srtsporder').eq(1).addClass('sort-active');
                    break;
            }
            $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + sorttext);
            $("#CustomSecurityProfileGridAction :input").removeAttr("disabled");
            $("#CustomSecurityProfileGridAction :button").removeClass("disabled");
            DisableExportButton($("#tblcustomsecurityprofile"), $(document).find('.import-export'));
        }
    });
}

$(document).on('click', '#tblcustomsecurityprofile_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#tblcustomsecurityprofile_length .searchdt-menu', function () {
    run = true;
});

$(document).find('.srtspcolumn').click(function () {
    ShowbtnLoader("btnsortmenu");
    $(document).find('.srtspcolumn').removeClass('sort-active');
    $(this).addClass('sort-active');
    orderbycol = $(this).data('col');
    orderDir = $('li.srtsporder.sort-active').data('mode');
    $('#tblcustomsecurityprofile').DataTable().order([orderbycol, orderDir]).draw();
    $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());
    run = true;
});
$(document).find('.srtsporder').click(function () {
    ShowbtnLoader("btnsortmenu");
    $(document).find('.srtsporder').removeClass('sort-active');
    $(this).addClass('sort-active');
    orderbycol = $(this).parent('ul').find('li.srtspcolumn.sort-active').data('col');
    orderDir = $(this).data('mode');
    $('#tblcustomsecurityprofile').DataTable().order([orderbycol, orderDir]).draw();
    run = true;
});


//#endregion
$(document).on('click', '.lnk_onsecDetails', function (e) {
    e.preventDefault();
    var index_row = $('#tblcustomsecurityprofile tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtSecurityProfile.row(row).data();
    ReloadCustomSecurityProfileItems(data.SecurityProfileId, data.Description,data.Name,'ModulesTab');
});
$(document).on('click', '#customSecurityProfiledesc', function () {
    $(document).find('#customsecprofiledesmodal').modal('show');
    $(document).find('#customsecprofiledesmodaltext').text($(this).data("des"));
});
function openCity(evt, cityName) {
    evt.preventDefault();

    ReloadCustomSecurityProfileItems($(document).find('#currentsecprofileid').val(), $(document).find('#currentsecprofiledes').val(), $(document).find('#currentsecprofilename').val(), cityName); 
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
$(document).on('click', "ul.vtabs li", function () {
    $("ul.vtabs li").removeClass("active");
    $(this).addClass("active");
    $(".tabsArea").hide();
    var activeTab = $(this).find("a").attr("href");
    $(activeTab).fadeIn();
    return false;
});
$(document).on('click', '.AddCustomSecurityProfile', function (e) {
    var SecurityProfileId=$(this).attr('data-profileid');
    $.ajax({
        url: "/CustomSecurityProfile/SecurityProfileAddOrEdit",
        type: "GET",
        dataType: 'html',
        data: { SecurityProfileId: SecurityProfileId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#CustomSecurityPopup').html(data);
            $('#CustomSecurityModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});

function SetControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    SetFixedHeadStyle();
}
function CustomSecurityProfileAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Mode == "Add") {
            SuccessAlertSetting.text = getResourceValue("SecurityProfileAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("SecurityProfileUpdateAlert");	
        }
        swal(SuccessAlertSetting, function () {
            $("#CustomSecurityModalpopup").modal('hide');
            ReloadCustomSecurityProfileItems(data.SecurityProfileId, data.Description, data.Name, 'ModulesTab');
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);

    }
}

//#region Common
function SecurityItemsObject(SecurityProfileId, SecurityItemId, SortOrder, UpdateIndex, Protected, ItemName, ItemAccess, ItemCreate, ItemEdit, ItemDelete, SingleItem, ReportItem) {
    this.SecurityProfileId = SecurityProfileId;
    this.SecurityItemId = SecurityItemId;
    this.SortOrder = SortOrder;
    this.UpdateIndex = UpdateIndex;
    this.Protected = Protected;
    this.ItemName = ItemName;
    this.ItemAccess = ItemAccess;
    this.ItemCreate = ItemCreate;
    this.ItemEdit = ItemEdit;
    this.ItemDelete = ItemDelete;
    this.SingleItem = SingleItem;
    this.ReportItem = ReportItem;
};
function ReloadCustomSecurityProfileItems(SecurityProfileId, Description,Name, mode) {
    $.ajax({
        url: "/CustomSecurityProfile/CustomSecurityProfileDetail",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { SecurityProfileId: SecurityProfileId, Description: Description, Name: Name, mode: mode },
        success: function (data) {
            $('#renderCustomSecurityprofile').empty().html(data);
            $(document).find('#currentsecprofileid').val(SecurityProfileId);
            $(document).find('#currentsecprofiledes').val(Description);
            $(document).find('#currentsecprofilename').val(Name);
            if (mode == "FunctionsTab") {
                $(document).find('#liReportsTab').removeClass('active');
                $(document).find('#ReportsTab').hide();

                $(document).find('#liModulesTab').removeClass('active');
                $(document).find('#ModulesTab').hide();

                $(document).find('#liFunctionsTab').addClass('active');
                $(document).find('#FunctionsTab').show();
            }
            else if (mode == "ReportsTab") {
                $(document).find('#liFunctionsTab').removeClass('active');
                $(document).find('#FunctionsTab').hide();

                $(document).find('#liModulesTab').removeClass('active');
                $(document).find('#ModulesTab').hide();

                $(document).find('#liReportsTab').addClass('active');
                $(document).find('#ReportsTab').show();
            }
            else
            {
                $(document).find('#liReportsTab').removeClass('active');
                $(document).find('#ReportsTab').hide();

                $(document).find('#liFunctionsTab').removeClass('active');
                $(document).find('#FunctionsTab').hide();

                $(document).find('#liModulesTab').addClass('active');
                $(document).find('#ModulesTab').show();
            }
        },
        complete: function () {
            $(document).on('click', '.disabled', function (e) {
                e.preventDefault();
            });
            $(document).find('#colorselector').val(mode);
            CloseLoader();
        }
    });
}
//#endregion


//#region Module
$(document).on('click', '#btnRefreshModule', function () {
    ReloadCustomSecurityProfileItems($(document).find('#currentsecprofileid').val(), $(document).find('#currentsecprofiledes').val(), $(document).find('#currentsecprofilename').val(),"ModulesTab");
});
$(document).on('click', '#btnSaveModule', function () {
    var ItemsToupdate = [];
    $("#tasksModuleTable tbody tr").each(function () {
        var securityprofileid = $(document).find('#currentsecprofileid').val();
        var securityitemid = $(this).find(".SecurityItemId").text();
        var sortorder = $(this).find('.SortOrder').text();
        var updateindex = $(this).find('.UpdateIndex').text();
        var Protected = $(this).find('.Protected').text();
        var ItemName = $(this).find(".ItemName").text();
        var access = $(this).find("td").find('.chkaccess').is(':checked');
        var create = $(this).find("td").find('.chkcreate').is(':checked');
        var edit = $(this).find("td").find('.chkedit').is(':checked');
        var del = $(this).find("td").find('.chkdel').is(':checked');
        var single = $(this).find('.single').text();
        var report = $(this).find('.report').text();
        var dataItem = new SecurityItemsObject(securityprofileid, securityitemid, sortorder, updateindex, Protected, ItemName, access, create, edit, del, single, report);
        ItemsToupdate.push(dataItem);
    })
    var objlist = JSON.stringify({ 'objlist': ItemsToupdate });
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        url: '/CustomSecurityProfile/SaveSecurityItems',
        type: "POST",
        datatype: "json",
        data: objlist,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                ShowSuccessAlert(getResourceValue("SecurityItemUpdateAlert"));
            }
        },
        complete: function () {
            ReloadCustomSecurityProfileItems($(document).find('#currentsecprofileid').val(), $(document).find('#currentsecprofiledes').val(), $(document).find('#currentsecprofilename').val(), "ModulesTab");
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion

//#region Functions
$(document).on('click', '#btnRefreshFunctions', function () {
    ReloadCustomSecurityProfileItems($(document).find('#currentsecprofileid').val(), $(document).find('#currentsecprofiledes').val(), $(document).find('#currentsecprofilename').val(), "FunctionsTab");
});
$(document).on('click', '#btnSaveFunctions', function () {
    var ItemsToupdate = [];
    $("#tasksFunctionsTable tbody tr").each(function () {      
        var securityprofileid = $(document).find('#currentsecprofileid').val();
        var securityitemid = $(this).find(".SecurityItemId").text();
        var sortorder = $(this).find('.SortOrder').text();
        var updateindex = $(this).find('.UpdateIndex').text();
        var Protected = $(this).find('.Protected').text();
        var ItemName = $(this).find(".ItemName").text();
        var access = $(this).find("td").find('.chkaccess').is(':checked');
        //var access = $(this).find(".access").text();
        var create = $(this).find(".create").text();
        var edit = $(this).find(".edit").text();
        var del = $(this).find(".del").text();
        var single = $(this).find(".single").text();
        //var single = $(this).find("td").find('.chkaccess').is(':checked');
        var report = $(this).find(".report").text();
        var dataItem = new SecurityItemsObject(securityprofileid, securityitemid, sortorder, updateindex, Protected, ItemName, access, create, edit, del, single, report);
        ItemsToupdate.push(dataItem);
    })
    var objlist = JSON.stringify({ 'objlist': ItemsToupdate });
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        url: '/CustomSecurityProfile/SaveSecurityItems',
        type: "POST",
        datatype: "json",
        data: objlist,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                ShowSuccessAlert(getResourceValue("SecurityItemUpdateAlert"));
            }
        },
        complete: function () {
            ReloadCustomSecurityProfileItems($(document).find('#currentsecprofileid').val(), $(document).find('#currentsecprofiledes').val(), $(document).find('#currentsecprofilename').val(), "FunctionsTab");
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion
//#region Reports
$(document).on('click', '#btnRefreshReports', function () {
    ReloadCustomSecurityProfileItems($(document).find('#currentsecprofileid').val(), $(document).find('#currentsecprofiledes').val(), $(document).find('#currentsecprofilename').val(), "ReportsTab");
});
$(document).on('click', '#btnSaveReports', function () {
    var ItemsToupdate = [];
    $("#tasksReportsTable tbody tr").each(function () {
        var securityprofileid = $(document).find('#currentsecprofileid').val();
        var securityitemid = $(this).find(".SecurityItemId").text();
        var sortorder = $(this).find('.SortOrder').text();
        var updateindex = $(this).find('.UpdateIndex').text();
        var Protected = $(this).find('.Protected').text();
        var ItemName = $(this).find(".ItemName").text();
        //var access = $(this).find(".access").text();
        var access = $(this).find("td").find('.chkaccess').is(':checked');
        var create = $(this).find(".create").text();
        var edit = $(this).find(".edit").text();
        var del = $(this).find(".del").text();
        var single = $(this).find(".single").text();
        var report = $(this).find(".report").text();
        //var report = $(this).find("td").find('.chkaccess').is(':checked');
        var dataItem = new SecurityItemsObject(securityprofileid, securityitemid, sortorder, updateindex, Protected, ItemName, access, create, edit, del, single, report);
        ItemsToupdate.push(dataItem);
    });
    var objlist = JSON.stringify({ 'objlist': ItemsToupdate });
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        url: '/CustomSecurityProfile/SaveSecurityItems',
        type: "POST",
        datatype: "json",
        data: objlist,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                ShowSuccessAlert(getResourceValue("SecurityItemUpdateAlert"));
            }
        },
        complete: function () {
            ReloadCustomSecurityProfileItems($(document).find('#currentsecprofileid').val(), $(document).find('#currentsecprofiledes').val(), $(document).find('#currentsecprofilename').val(), "ReportsTab");
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion

//#region Search and Advance Search
function CustomSecurityProfileAdvSearch() {
    $('#txtColumnSearch').val('');
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#dvFilterSearchSelect2").html(searchitemhtml);
    $(".filteritemcount").text(selectCount);
}
$("#btnCustomSecurityProfileAdvSrch").on('click', function (e) {
    run = true;
    CustomSecurityProfileAdvSearch();
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    dtSecurityProfile.page('first').draw('page');
});
function clearAdvanceSearch() {
    selectCount = 0;
    $("#txtName").val("");
    $("#txtDescription").val("");
    $(".filteritemcount").text(selectCount);
    $("#dvFilterSearchSelect2").html('');
}
$(document).on('click', '#sidebarCollapse', function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$(document).on('click', '#dismiss, .overlay', function () {
    $(document).find('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    CustomSecurityProfileAdvSearch();
    dtSecurityProfile.page('first').draw('page');
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
            advcountercontainer.text(selectCount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
}
//#endregion

//#region New Search button
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'CustomSecurityProfile' },
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
        data: { tableName: 'CustomSecurityProfile', searchText: txtSearchval, isClear: isClear },
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
                dtSecurityProfile.page('first').draw('page');
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
    //activeStatus = 0;
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#dvFilterSearchSelect2").html(searchitemhtml);
    }
    else {
        dtSecurityProfile.page('first').draw('page');
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
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

//#region Print
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var name = LRTrim($('#txtName').val());
            var description = LRTrim($("#txtDescription").val());
            var searchText = LRTrim($(document).find('#txtColumnSearch').val());

            dtSecurityProfile = $("#tblcustomsecurityprofile").DataTable();
            var currestsortedcolumn = $('#tblcustomsecurityprofile').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#tblcustomsecurityprofile').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#tblcustomsecurityprofile').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var jsonResult = $.ajax({
                "url": "/CustomSecurityProfile/GetCustomSecurityProfileGridPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    name: name,
                    description: description,
                    colname: colname,
                    coldir: coldir,
                    searchText: searchText
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#tblcustomsecurityprofile thead tr th").map(function (key) {
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
                if (item.Description != null) {
                    item.Description = item.Description;
                }
                else {
                    item.Description = "";
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
                header: $("#tblcustomsecurityprofile thead tr th").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});
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
//#endregion Print

//#region Dropdown toggle   
$(document).mouseup(function (e) {
    var container = $(document).find('#searchBttnNewDrop');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slideToggle");
    }
});
//#endregion
