var activeStatus = true;
var totalcount = 0;
var searchcount = 0;
var searchresult = [];
var userToupdate = [];
var run = false;
var PackageLevel = 'ENTERPRISE';
var order = '';
var orderDir = 'asc';
var orderJson = [];
var CaseNo = '1';
var siteList = '';
var selectedSiteNames = [];
$(function () {
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', '#sidebarCollapse', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    ShowbtnLoaderclass("LoaderDrop");
    activeStatus = true;
    $(".searchList li").removeClass("activeState");
    CaseNo = localStorage.getItem("ADMINUSERMANAGEMENTSTATUS");
    if (CaseNo && $('#usersearchListul #' + CaseNo).length > 0) {
        $('#usersearchListul #' + CaseNo).addClass('activeState');
        $('#UserManagmentTitle').text($('#usersearchListul #' + CaseNo).text());
        localStorage.setItem('ADMINUSERMANAGEMENTTITLE', $('#usersearchListul #' + CaseNo).text());
       
    }
    else {
        CaseNo = '1';
        $('#usersearchListul #1').addClass('activeState');
        $('#UserManagmentTitle').text($('#usersearchListul #1').text());
        localStorage.setItem("ADMINUSERMANAGEMENTSTATUS", CaseNo);
        localStorage.setItem('ADMINUSERMANAGEMENTTITLE', $('#usersearchListul #1').text());
        localStorage.removeItem('SITELIST');
    }

        order = '1';
        orderJson = [[1, "asc"]];
        LoaduserManagmentTableGrid();
    
});
$(document).on('change', '#colorselector', function (evt) {
    $(document).find('.tabsArea').hide();
    openCity(evt, $(this).val());
    $('#' + $(this).val()).show();
});
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../UserManagement/index?page=User_Management";
});
//#region Search
var userManagmentTable;
var securityProfileId = "";

$('#userManagmentTable').find('th').click(function () {
    if ($(this).data('col') !== undefined && $(this).data('col') !== '') {
        run = true;
        order = $(this).data('col');
    }
});

function LoaduserManagmentTableGrid() {
    var ClienId = $("#ClientId").val() == "" ? 0 : $("#ClientId").val();
    if ($(document).find('#userManagmentTable').hasClass('dataTable')) {
        userManagmentTable.destroy();
    }
    userManagmentTable = $("#userManagmentTable").DataTable({
        colReorder: {
            fixedColumnsLeft:2
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
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": "/Admin/base/CreateUpdateState",
                    "data": {
                        GridName: "User_Search_Admin",
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
                "url": "/Admin/base/GetLayout",
                "data": {
                    GridName: "User_Search_Admin"
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
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
                        }
                    }
                    else {
                        callback(json.LayoutInfo);
                    }

                }
            });
        },
        fixedColumns: {
            leftColumns: 2
        },
        language: {
            url: "/Admin/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [

            {
                extend: 'excelHtml5',
                title: 'UserDetails'
            },
            {
                extend: 'print',
                title: 'UserDetails'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'UserDetails',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                css: 'display:none',
                title: 'UserDetails',
                orientation: 'landscape',
                pageSize: 'A4'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Admin/UserManagement/PopulateuserManagment",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.CaseNo = CaseNo;
                d.UserName = LRTrim($("#UserName").val());
                d.LastName = LRTrim($("#LastName").val());
                d.FirstName = LRTrim($("#FirstName").val());
                d.Email = LRTrim($("#Email").val());
                d.order = order;
                d.orderDir = userManagmentTable.order()[0][1];
                d.SearchText = LRTrim($("#txtColumnSearch").val());
                d.ClientId = $("#ClientId").val() == "" ? 0 : $("#ClientId").val();
                d.SiteId = $("#SiteId").val() == "" ? 0 : $("#SiteId").val();
            },
            "dataSrc": function (result) {
                
                let colOrder = userManagmentTable.order();              
                orderDir = colOrder[0][1];
                  
                searchcount = result.recordsTotal;
                $.each(result.data, function (index, item) {
                    searchresult.push(item.UserInfoId);
                });
                if (totalcount < result.recordsTotal)
                    totalcount = result.recordsTotal;
                if (totalcount != result.recordsTotal)
                    selectedcount = result.recordsTotal;
                //HidebtnLoaderclass("LoaderDrop");
                if (result.data.length == "0") {
                    $(document).find('.import-export').attr('disabled', 'disabled');
                }
                else {
                    $(document).find('.import-export').removeAttr('disabled');
                }
                HidebtnLoaderclass("LoaderDrop");
                return result.data;
            },
            global: true
        },
        //select: {
        //    style: 'os',
        //    selector: 'td:first-child'
        //},
        "columns":
            [
                {
                    "data": "UserInfoId",
                    "bVisible": true,
                    "bSortable": false,
                    "autoWidth": false,
                    "bSearchable": true,
                    "name": "0",
                    "mRender": function (data, type, row) {
                        if (row.SiteCount > 0) {
                            return '<img id="' + data + '" src="../Images/details_open.png" alt="expand/collapse" style="cursor: pointer;"/>';
                        }
                        else {
                            return '';
                        }
                    }
                },
                {
                    "data": "UserName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                    "mRender": function (data, type, row) {
                        return '<a class=link_um_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "LastName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "FirstName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                
                { "data": "CompanyName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                { "data": "Email", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },

                { "data": "SecurityProfile", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
                { "data": "IsActive", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7" }
            ],
        columnDefs: [
            {
                targets: [0,1],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            $(document).on('click', '.status', function (e) {
                e.preventDefault();
            });
            SetPageLengthMenu();           

            $("#UserManagementGridAction :input").removeAttr("disabled");
            $("#UserManagementGridAction :button").removeClass("disabled");
            DisableExportButton($("#userManagmentTable"), $(document).find('.import-export'));
        }
    });
}
$(document).find('#userManagmentTable').on('click', 'tbody td img', function (e) {
    var tr = $(this).closest('tr');
    var row = userManagmentTable.row(tr);
    var data = userManagmentTable.row(row).data();
    if (this.src.match('details_close')) {
        this.src = "../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        this.src = "../Images/details_close.png";
    
        $.ajax({
            url: "/Admin/UserManagement/PopulateuserManagmentInnerGridDetails",
            data: {
                UserInfoId: data.UserInfoId,
                ClientId: data.ClientId
            },
            beforeSend: function () {
                ShowLoader();
            },
            dataType: 'html',
            success: function (json) {
                row.child(json).show();
                dtinnerGrid = row.child().find('.userManagementinnerDataTable').DataTable(
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
                            url: "/Admin/base/GetDataTableLanguageJson?nGrid=" + true
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
$(document).on('click', '.link_um_detail', function (e) {
    e.preventDefault();
    var titleText = $('#UserManagmentTitle').text();
    localStorage.setItem('ADMINUSERMANAGEMENTTITLE', titleText);
    var index_row = $('#userManagmentTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = userManagmentTable.row(row).data();
    var ClientId= data.ClientId
    $.ajax({
        url: "/Admin/UserManagement/UserManagementDetails",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { UserId: data.UserInfoId, ClientId: data.ClientId},
        success: function (data) {
            $('#usermanagementcontainer').empty().html(data);
            $(document).find('#spnlinkToSearch').text(titleText);
            GenerateActivityTableGrid(ClientId);
        },
        complete: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '#userClearAdvSearchFilter', function () {
    run = true;
    clearAdvanceSearch();
    userManagmentTable.page('first').draw('page');
});
function clearAdvanceSearch() {
    $("#FirstName").val("");
    $("#LastName").val("");
    $("#UserName").val("");
  
    $('#ClientId').val('').trigger('change');
    $('#SiteId').val('').trigger('change');
    selectCount = 0;
    $("#spnControlCounter").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
    $(".filteritemcount").text(selectCount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    userAdvSearch();
    userManagmentTable.page('first').draw('page');
});
function userAdvSearch() {
    var InactiveFlag = false;
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            if ($(this).attr('id') == "SiteId") {
                if ($(this).val() != "" && $(this).val() != "0") {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
          
            else if ($(this).attr('id') == "ClientId") {
                if ($(this).val() != "" && $(this).val() != "0") {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
            else {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';

            }
       }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    if (!$('#btnActiveMain').hasClass('active')) {
        InactiveFlag = true;
    }
    $("#advsearchfilteritems").html(searchitemhtml);
    $(".filteritemcount").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}
$(document).on('click', '#liPrint', function () {
    CustomPDFGenerator_ForEnterprise('liPrint');
    //if ($('#PackageLevel').val().toUpperCase() === PackageLevel && $('#IsSuperUser').val() === 'True') {
        
    //}
    //else {
    //    $(".buttons-print")[0].click();
    //}
    funcCloseExportbtn();
});
$(document).on('click', '#liPdf', function () {
    //if ($('#PackageLevel').val().toUpperCase() === PackageLevel && $('#IsSuperUser').val() === 'True') {
    //    CustomPDFGenerator_ForEnterprise('liPdf');
    //}
    //else {
    /* $(".buttons-pdf")[0].click();*/
    CustomPDFGenerator_ForEnterprise('liPdf');
    /*}*/
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
function CustomPDFGenerator_ForEnterprise(thisid) {
    var TableHaederProp = [];
    function table(property, title) {
        this.property = property;
        this.title = title;
    }
    $("#userManagmentTable thead tr th").map(function (key) {
        if ($(this).parents('.userManagementinnerDataTable').length == 0 && $(this).html()) {
            if (this.getAttribute('data-th-prop')) {
                var tablearr = new table(this.getAttribute('data-th-prop'), $(this).html());
                TableHaederProp.push(tablearr);
            }
        }
    });
    var params = {
        tableHaederProps: TableHaederProp,
        CaseNo: CaseNo,
        UserName: LRTrim($("#UserName").val()),
        LastName: LRTrim($("#LastName").val()),
        FirstName: LRTrim($("#FirstName").val()),
        Email: LRTrim($("#Email").val()),
        SiteId: $("#SiteId").val() == "" ? 0 : $("#SiteId").val(),
        order: order,
        orderDir: orderDir,
        SearchText: LRTrim($("#txtColumnSearch").val()),
        "ClientId": $("#ClientId").val() == "" ? 0 : $("#ClientId").val()
    };
    objPrintParams = JSON.stringify({ 'objPrintParams': params });
    $.ajax({
        "url": "/Admin/UserManagement/SetPrintData",
        "data": objPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            if (thisid == 'liPdf') {
                window.open('/Admin/UserManagement/ExportASPDF?d=d', '_self');
            }
            else if (thisid == 'liPrint') {
                window.open('/Admin/UserManagement/ExportASPDF', '_blank');
            }

            return;
        }
    });
}
$(document).on('change', '#userDropdown', function () {
    ShowbtnLoaderclass("LoaderDrop");
    run = true;
    var optionVal = $(document).find("#userDropdown").val();
    userToupdate = [];
    selectedcount = 0;
    totalcount = 0;
    searchcount = 0;
    searchresult = [];
    var searchOption = $(this).val();
    if (optionVal === "1") {
        activeStatus = true;
    }
    else {
        activeStatus = false;
    }
    $(this).addClass("active");
    localStorage.setItem("ADMINUSERMANAGEMENTSTATUS", activeStatus);
    $('.itemcount').text(userToupdate.length);
    if (searchOption) {
        clearAdvanceSearch();
    }
    userManagmentTable.page('first').draw('page');
});
$("#btnuserDataAdvSrch").on('click', function (e) {
    run = true;
    searchresult = [];
    userAdvSearch();
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    $('#txtColumnSearch').val('');
    userManagmentTable.page('first').draw('page');
});
//#endregion

//#region Commonn
$(document).ready(function () {
    $("#action").click(function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionBar").fadeIn();
    $("#UserManagementGridAction :input").attr("disabled", "disabled");
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $("#action").focusout(function () {
        $(".actionDrop").fadeOut();
    });
    $(document).find('.select2picker').select2({
    });

    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', "ul.vtabs li", function () {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        $(".tabsArea").hide();
        var activeTab = $(this).find("a").attr("href");
        $(activeTab).fadeIn();
        return false;
    });
});
function openCity(evt, cityName) {

    evt.preventDefault();
    switch (cityName) {
        case "Details":
            break;
        case "Sites":
            generatePersonnelSitesGrid();
            $(document).find('#ClientActivity').css('display', 'none');
            $(document).find('#SiteTab').css('display', 'block');
            break;
    }
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent2");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}
$(document).on('click', "#liuser", function (e) {
    $(document).find('#Usertab').show();
    $(document).find('#Details').show();
    $(document).find('#ClientActivity').css('display', 'block');
    $(document).find('#SiteTab').css('display', 'none');
    $("#siteTable").empty();
});

$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var jsonResult = $.ajax({
                "url": "/Admin/UserManagement/UserDetailsPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    "CaseNo": CaseNo,
                    "UserName": LRTrim($("#UserName").val()),
                    "LastName": LRTrim($("#LastName").val()),
                    "FirstName": LRTrim($("#FirstName").val()),
                    "Email": LRTrim($("#Email").val()),
                    "ClientId": $("#ClientId").val() == "" ? 0 : $("#ClientId").val(),
                    "order": order,
                    "orderDir": orderDir,
                    "SearchText": LRTrim($("#txtColumnSearch").val()),
                    "SiteId": $("#SiteId").val() == "" ? 0 : $("#SiteId").val(),
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = [];

           
                visiblecolumnsIndex = $("#userManagmentTable thead tr th").map(function (key) {
                    return this.getAttribute('data-th-index');
                }).get();
            

            var d = [];
            $.each(thisdata, function (index, item) {

                if (item.UserName != null) {
                    item.UserName = item.UserName;
                }
                else {
                    item.UserName = "";
                }
                if (item.LastName != null) {
                    item.LastName = item.LastName;
                }
                else {
                    item.LastName = "";
                }
                if (item.FirstName != null) {
                    item.FirstName = item.FirstName;
                }
                else {
                    item.FirstName = "";
                }
                if (item.Email != null) {
                    item.Email = item.Email;
                }
                else {
                    item.Email = "";
                }
                if (item.SecurityProfile != null) {
                    item.SecurityProfile = item.SecurityProfile;
                }
                else {
                    item.SecurityProfile = "";
                }
                if (item.Client != null) {
                    item.Client = item.Client;
                }
                else {
                    item.Client = "";
                }
                if (item.Active != null) {
                    item.Active = item.Active;
                }
                else {
                    item.Active = "";
                }
                var fData = [];
                $.each(visiblecolumnsIndex, function (index, inneritem) {
                    var key = Object.keys(item)[inneritem];
                    var value = item[key];
                    fData.push(value);
                });
                d.push(fData);
            });

            var header = [];
                header = $("#userManagmentTable thead tr th").not(":eq(0)").map(function (key) {
                    if ($(this).parents('.userManagementinnerDataTable').length == 0 && this.innerHTML) {
                        return this.innerHTML;
                    }
                }).get();
            
            return {

                body: d,
                header: header
            };
        }
    });
});
//#endregion
$(document).on('click', '#userManagmentTable_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#userManagmentTable_length .searchdt-menu', function () {
    run = true;
});

$(document).on('click', '#userManagmentTable_wrapper th', function () {
    run = true;
});
//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(userManagmentTable, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0, 1];
    funCustozeSaveBtn(userManagmentTable, colOrder);
    run = true;
    userManagmentTable.state.save(run);
});   
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
$(document).on('keyup', '#usersearchtxtbox', function (e) {
    var tagElems = $(document).find('#usersearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.usersearchdrpbox', function (e) {
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#advsearchfilteritems").html('');
    $(document).find('#txtColumnSearch').val('');
    run = true;
    if ($(this).attr('id') != '0') {
        $('#UserManagmentTitle').text($(this).text());
    }
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    CaseNo = $(this).attr('id');
    localStorage.setItem("ADMINUSERMANAGEMENTSTATUS", CaseNo);
    localStorage.setItem('ADMINUSERMANAGEMENTTITLE', $('#usersearchListul #' + CaseNo).text());
    ShowbtnLoaderclass("LoaderDrop");
    userManagmentTable.page('first').draw('page');
});

$(document).on('change', '#ddlSiteId', function () {
    var val = $(this).val();
    if (val && val.length > 0) {
        $('#btnSelectSite').removeAttr('disabled');
    }
    else {
        $('#btnSelectSite').attr('disabled', 'disabled');
    }
});
$(document).on('click', '#btnSelectSite', function () {
    run = true;
    siteList = '';
    selectedSiteNames = [];
    selectedSiteNames = $(document).find('#ddlSiteId').val();
    if (selectedSiteNames.length == 0) {
        return false;
    }
    localStorage.setItem('SITELIST', selectedSiteNames);
    for (var i = 0; i < selectedSiteNames.length; i++) {
        siteList = siteList + selectedSiteNames[i] + ',';
    }
    siteList = siteList.slice(0, -1);
    $(document).find('#SitesUsersModal').modal('hide');

    var text = $('#usersearchListul').find('li').eq(2).text();
    $('#UserManagmentTitle').text(text);
    $("#usersearchListul li").removeClass("activeState");
    $("#usersearchListul li").eq(2).addClass('activeState');

    CaseNo = $("#usersearchListul li").eq(2).attr('id');
    localStorage.setItem("ADMINUSERMANAGEMENTSTATUS", CaseNo);
    localStorage.setItem('ADMINUSERMANAGEMENTTITLE', $('#usersearchListul #' + CaseNo).text());

    if (selectedSiteNames.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        userManagmentTable.page('first').draw('page');
    }
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');
});

//#endregion

//#region New Search button
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Admin/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'AdminUserManagement' },
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
        url: '/Admin/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'AdminUserManagement', searchText: txtSearchval, isClear: isClear },
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
                userManagmentTable.page('first').draw('page');
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
    var partcurrentstatus = localStorage.getItem("CURRENTTABSTATUS");
    if (partcurrentstatus) {
        CustomQueryDisplayId = partcurrentstatus;
        $('#usersearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId && $(this).attr('id') != '0') {
                $('#partsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
    }
    else { $('#partsearchtitle').text(getResourceValue("AlertActive")); }
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        userManagmentTable.page('first').draw('page');
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

//#region Sorting
$(document).find('.srtusercolumn').click(function () {
    ShowbtnLoader("btnsortmenu");
    order = $(this).data('col');
    $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());
    $(document).find('.srtusercolumn').removeClass('sort-active');
    $(this).addClass('sort-active');
    run = true;
    $('#userManagmentTable').DataTable().draw();
});
$(document).find('.srtuserorder').click(function () {
    ShowbtnLoader("btnsortmenu");
    orderDir = $(this).data('mode');

    $(document).find('.srtuserorder').removeClass('sort-active');
    $(this).addClass('sort-active');
    run = true;
    $('#userManagmentTable').DataTable().draw();
});
//#endregion

//#region Additional Search
var filterinfoarray = [];
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
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
       
        else if (item.key == 'SiteId') {
            $("#SiteId").val(item.value).trigger("change.select2");
            if (item.value) {
                if (item.value != "") {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
        }
        else if (item.key == 'ClientId') {
            $("#ClientId").val(item.value).trigger("change.select2");
            if (item.value) {
                if (item.value != "") {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
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

