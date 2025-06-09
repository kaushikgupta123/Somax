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
//var titleText = '';
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
    CaseNo = localStorage.getItem("USERMANAGEMENTSTATUS");
    if (CaseNo && $('#usersearchListul #' + CaseNo).length > 0) {
        $('#usersearchListul #' + CaseNo).addClass('activeState');
        $('#UserManagmentTitle').text($('#usersearchListul #' + CaseNo).text());
        localStorage.setItem('USERMANAGEMENTTITLE', $('#usersearchListul #' + CaseNo).text());
        //if (CaseNo == '3') {
        //    siteList = localStorage.getItem('SITELIST');
        //    if (siteList != null && siteList.length > 0) {
        //        selectedSiteNames = localStorage.getItem('SITELIST').split(',');
        //    }
        //    if (selectedSiteNames != null && selectedSiteNames.length > 0) {
        //        var selectedValues = new Array();
        //        for (var i = 0; i < selectedSiteNames.length; i++) {
        //            selectedValues[i] = selectedSiteNames[i];
        //        }
        //        $("#ddlSiteId").val(selectedValues).trigger('change');
        //    }
        //    //if site id's not exists in the dropdown for the user
        //    if ($("#ddlSiteId").val().length === 0) {
        //        CaseNo = '1';
        //        $('.usersearchdrpbox').removeClass('activeState');
        //        $('#usersearchListul #1').addClass('activeState');
        //        $('#UserManagmentTitle').text($('#usersearchListul #1').text());
        //        localStorage.setItem("USERMANAGEMENTSTATUS", CaseNo);
        //        localStorage.setItem('USERMANAGEMENTTITLE', $('#usersearchListul #1').text());
        //        localStorage.removeItem('SITELIST');
        //    }
        //}
    }
    else {
        CaseNo = '1';
        $('#usersearchListul #1').addClass('activeState');
        $('#UserManagmentTitle').text($('#usersearchListul #1').text());
        localStorage.setItem("USERMANAGEMENTSTATUS", CaseNo);
        localStorage.setItem('USERMANAGEMENTTITLE', $('#usersearchListul #1').text());
        localStorage.removeItem('SITELIST');
    }

    if ($('#PackageLevel').val().toUpperCase() === PackageLevel && $('#IsSuperUser').val() === 'True') {
        order = '1';
        orderJson = [[1, "asc"]];
        LoaduserManagmentTableGrid_ForEnterprise();
    }
    else {
        order = '0';
        orderJson = [[0, "asc"]];
        LoaduserManagmentTableGrid_ForBasicProfessional();
    }
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
function LoaduserManagmentTableGrid_ForEnterprise() {
    if ($(document).find('#userManagmentTable').hasClass('dataTable')) {
        userManagmentTable.destroy();
    }
    userManagmentTable = $("#userManagmentTable").DataTable({
        colReorder: {
            fixedColumnsLeft: 2
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": orderJson,
        stateSave: true,
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
                        GridName: "User_Search_Enterprise",
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
                    GridName: "User_Search_Enterprise"
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
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
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
            "url": "/UserManagement/PopulateuserManagment",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.CaseNo = CaseNo;
                d.UserName = LRTrim($("#UserName").val());
                d.LastName = LRTrim($("#LastName").val());
                d.FirstName = LRTrim($("#FirstName").val());
                d.Email = LRTrim($("#Email").val());
                //v2 332
                d.CraftId = $("#CraftId").val() == "" ? 0 : $("#CraftId").val();
                d.order = order;
                /*d.orderDir = orderDir;*/
                d.orderDir=userManagmentTable.order()[0][1];
                //d.SelectedSites = siteList;
                d.Sites = $('#Sites option:selected').toArray().map(item => item.value).toString();
                d.SearchText = LRTrim($("#txtColumnSearch").val());
                d.SecurityProfileIds = $('#SecurityProfileId option:selected').toArray().map(item => item.value).toString();
                d.UserTypes = $('#UserType option:selected').toArray().map(item => item.value).toString();
                d.Shifts = $('#Shift option:selected').toArray().map(item => item.value).toString();
                d.IsActive = $("#IsActive").val();
                d.EmployeeId = $("#EmployeeId").val();
                
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
                            return '<img id="' + data + '" src="../../Images/details_open.png" alt="expand/collapse" rel="' + data + '" style="cursor: pointer;"/>';
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
                { "data": "Email", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                { "data": "EmployeeId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" }, //V2-1160
                // v2 332
                //{ "data": "CraftDescription", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                { "data": "SecurityProfileDescription", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" }
            ],
        columnDefs: [
            {
                targets: [0, 1],
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

$('#userManagmentTable').find('th').click(function () {
    if ($(this).data('col') !== undefined && $(this).data('col') !== '') {
        run = true;
        order = $(this).data('col');
    }
});

function LoaduserManagmentTableGrid_ForBasicProfessional() {
    if ($(document).find('#userManagmentTable').hasClass('dataTable')) {
        userManagmentTable.destroy();
    }
    userManagmentTable = $("#userManagmentTable").DataTable({
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
                }
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: "User_Search",
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
                    GridName: "User_Search"
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
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
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
            "url": "/UserManagement/PopulateuserManagment",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.CaseNo = CaseNo;
                d.UserName = LRTrim($("#UserName").val());
                d.LastName = LRTrim($("#LastName").val());
                d.FirstName = LRTrim($("#FirstName").val());
                d.Email = LRTrim($("#Email").val());
                //v2 332
                d.CraftId = $("#CraftId").val() == "" ? 0 : $("#CraftId").val();
                d.order = order;
                /* d.orderDir = orderDir;*/
                d.orderDir = userManagmentTable.order()[0][1];
                //d.SelectedSites = siteList;
                //d.Sites = $('#Site option:selected').toArray().map(item => item.value).toString();
                d.SearchText = LRTrim($("#txtColumnSearch").val());
                d.SecurityProfileIds = $('#SecurityProfileId option:selected').toArray().map(item => item.value).toString();
                d.Sites = $('#Sites option:selected').toArray().map(item => item.value).toString();
                d.UserTypes = $('#UserType option:selected').toArray().map(item => item.value).toString();
                d.Shifts = $('#Shift option:selected').toArray().map(item => item.value).toString();
                d.IsActive = $("#IsActive").val();
                d.EmployeeId = $("#EmployeeId").val();
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
                    "data": "UserName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "0",
                    "mRender": function (data, type, row) {
                        return '<a class=link_um_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "LastName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
                { "data": "FirstName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "Email", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                { "data": "EmployeeId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" }, //V2-1160
                // v2 332
                { "data": "CraftDescription", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
                { "data": "SecurityProfileDescription", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" }
            ],
        columnDefs: [
            {
                targets: [0],
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
    if (this.src.match('details_close')) {
        this.src = "../../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        this.src = "../../Images/details_close.png";
        var UserInfoId = $(this).attr("rel");
        $.ajax({
            url: "/UserManagement/PopulateuserManagmentCraftDetails",
            data: {
                UserInfoId: UserInfoId
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
                            url: "/base/GetDataTableLanguageJson?nGrid=" + true
                        },
                        buttons: [],
                        //"columnDefs": [
                        //    { className: 'text-right', targets: [3, 5, 6] }
                        //],
                        //"footerCallback": function (row, data, start, end, display) {
                        //    var api = this.api(),
                        //        // Total over all pages
                        //        //total = api.column(6).data().reduce(function (a, b) {
                        //        //    return parseFloat(a) + parseFloat(b);
                        //        //}, 0);
                        //    // Update footer
                        //    //$(api.column(6).footer()).html(total.toFixed(2));
                        //},
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
    localStorage.setItem('USERMANAGEMENTTITLE', titleText);
    var index_row = $('#userManagmentTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = userManagmentTable.row(row).data();
    $.ajax({
        url: "/UserManagement/UserManagementDetails",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { UserId: data.UserInfoId },
        success: function (data) {
            $('#usermanagementcontainer').empty().html(data);
            $(document).find('#spnlinkToSearch').text(titleText);
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
    $("#Password").val("");
    $("#Email").val("");
    $('.adv-item').val("");
    $('#CraftId').val('').trigger('change');
    $('#SecurityProfileId').val('').trigger('change');
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
            if ($(this).attr('id') == "SecurityProfileId") {
                if ($(this).val() != "" && $(this).val() != "0") {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                    }
            }
            else if ($(this).attr('id') == "Sites") {
                if ($(this).val() != "" && $(this).val() != "0") {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
            else if ($(this).attr('id') == "Shift") {
                if ($(this).val() != "" && $(this).val() != "0") {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
            else if ($(this).attr('id') == "UserType") {
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
    if ($('#PackageLevel').val().toUpperCase() === PackageLevel && $('#IsSuperUser').val() === 'True') {
        CustomPDFGenerator_ForEnterprise('liPrint');
    }
    else {
        $(".buttons-print")[0].click();
    }
    funcCloseExportbtn();
});
$(document).on('click', '#liPdf', function () {
    if ($('#PackageLevel').val().toUpperCase() === PackageLevel && $('#IsSuperUser').val() === 'True') {
        CustomPDFGenerator_ForEnterprise('liPdf');
    }
    else {
        $(".buttons-pdf")[0].click();
    }
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
        CraftId: $("#CraftId").val() == "" ? 0 : $("#CraftId").val(),
        order: order,
        orderDir: orderDir,
        SelectedSites: $('#Sites option:selected').toArray().map(item => item.value).toString(),
        SearchText: LRTrim($("#txtColumnSearch").val()),
        SecurityProfileIds: $('#SecurityProfileId option:selected')
            .toArray().map(item => item.value).toString(),
        UserTypes: $('#UserType option:selected').toArray().map(item => item.value).toString(),
        Shifts: $('#Shift option:selected').toArray().map(item => item.value).toString(),
        IsActive: $("#IsActive").val(),
        EmployeeId: $("#EmployeeId").val(),
    };
    objPrintParams = JSON.stringify({ 'objPrintParams': params });
    $.ajax({
        "url": "/UserManagement/SetPrintData",
        "data": objPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            if (thisid == 'liPdf') {
                window.open('/UserManagement/ExportASPDF?d=d', '_self');
            }
            else if (thisid == 'liPrint') {
                window.open('/UserManagement/ExportASPDF', '_blank');
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
    localStorage.setItem("USERMANAGEMENTSTATUS", activeStatus);
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
});
//$.validator.setDefaults({ ignore: null });
$(document).ready(function () {
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
        case "Notes":
            generateNotesGrid();
            break;
        case "Attachment":
            generateAttachmentsGrid();
            break;
        case "Contacts":
            generateContactsGrid();
            break;        
        case "Sites":
            generatePersonnelSitesGrid();
            break;
        case "Storerooms":
            generatePersonnelStoreroomsGrid();
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
    $(document).find('#btndetails').addClass('active');
    $(document).find('#Usertab').show();
    $(document).find('#Details').show();
});
$(document).on('click', "#edituser", function () {
    var UserInfoId = $('#UserInfoId').val();
    $.ajax({
        url: "/UserManagement/UserEdit",
        type: "GET",
        dataType: 'html',
        data: { UserInfoId: UserInfoId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#usermanagementcontainer').empty().html(data);
        },
        complete: function () {
            CloseLoader();
            //Setmandatory();
            $.validator.setDefaults({ ignore: '.cls-user-name' });
            $.validator.unobtrusive.parse(document);
            $(document).find('.select2picker').select2({});
            $('input, form').blur(function () {
                $(this).valid();
            });
            CloseAreasetup();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btneditcancel,#brdUser", function () {
    var UserInfoId = $('#UserInfoId').val();
    swal(CancelAlertSetting, function () {
        RedirectTouserDetail(UserInfoId);
    });
});
function RedirectTouserDetail(UserInfoId, mode) {
    $.ajax({
        url: "/UserManagement/UserManagementDetails",
        type: "POST",
        dataType: 'html',
        data: { UserId: UserInfoId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#usermanagementcontainer').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem('USERMANAGEMENTTITLE'));
        },
        complete: function () {
            CloseLoader();
            if (mode === "notes") {
                $('#linotes').trigger('click');
                $('#colorselector').val('Notes');
                generateNotesGrid();
            }
            if (mode === "attachment") {
                $('#liattachment').trigger('click');
                $('#colorselector').val('Attachment');
                generateAttachmentsGrid();
            }
            if (mode === "contacts") {
                $('#licontacts').trigger('click');
                $('#colorselector').val('Contacts');
                generateContactsGrid();
            } 
            if (mode === "sites") {
                $('#lisites').trigger('click');
                $('#colorselector').val('Sites');
                generatePersonnelSitesGrid();
            }
            if (mode === "storerooms") {
                $('#listorerooms').trigger('click');
                $('#colorselector').val('Storerooms');
                generatePersonnelStoreroomsGrid();
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
function UserEditOnSuccess(data) {
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("UserUpdateAlert");
        swal(SuccessAlertSetting, function () {
            $.ajax({
                url: "/UserManagement/UserManagementDetails",
                type: "POST",
                dataType: 'html',
                data: { UserId: data.UserInfoId },
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (data) {
                    $('#usermanagementcontainer').html(data);
                    $(document).find('#spnlinkToSearch').text(localStorage.getItem('USERMANAGEMENTTITLE'));
                },
                complete: function () {

                    CloseLoader();
                },
                error: function () {
                    CloseLoader();
                }
            });
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', ".adduser", function () {
    $.ajax({
        url: "/UserManagement/AddUser",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#usermanagementcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            $(document).find("#UserName").val("");
            $(document).find("#Password").val("");
            $.validator.unobtrusive.parse(document);
            SetUMControls();
            //$(document).find('.select2picker').select2({
            //});
            //$('input, form').blur(function () {
            //    $(this).valid();
            //});
            //CloseAreasetup();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function UserAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Command == "save") {
            SuccessAlertSetting.text = getResourceValue("UserAddAlert");
            swal(SuccessAlertSetting, function () {
                RedirectTouserDetail(data.UserInfoId, "userinformation");
            });
        }
        else {
            ResetErrorDiv();
            SuccessAlertSetting.text = getResourceValue("UserAddAlert");
            swal(SuccessAlertSetting, function () {
                $(document).find('#Userform').trigger("reset");
                $(document).find('#Userform').find("select").removeClass("input-validation-error");
                $(document).find('#Userform').find("input").removeClass("input-validation-error");
                $(document).find('#Userform').find('#userModels_CraftId').val("").trigger('change.select2');
                $(document).find('#Userform').find('#userModels_SiteId').val("").trigger('change.select2');
                $(document).find('#userModels_SecurityProfileName').val('');
                $(document).find('#userModels_SecurityProfileId').val('');
                $(document).find('#UserType').val('');
                $(document).find('#userModels_CMMSUser').val('');
                $(document).find('#userModels_SanitationUser').val('');
                $(document).find('#userModels_ProductGrouping').val('');
                $(document).find('#userModels_PackageLevel').val('');
                $(document).find('#Userform').find('#Email').val('');
                $(document).find('#Userform').find('#userModels_Buyer').prop('checked', false);
                $(document).find("#liCraft").hide();
                $(document).find("#liEmail").hide();
                $(document).find("#liBuyer").hide();
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnCancelAddUser", function () {
    swal(CancelAlertSetting, function () {
        window.location.href = "../UserManagement/Index?page=User_Management";
    });
});
function CloseAreasetup() {
    $(document).find('.select2picker, form').change(function () {
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
$(document).on('click', "#btnuserEdit", function () {
    if ($(document).find("form").valid()) {
        return;
    }
    else {
        var errorTab = $(".input-validation-error").parents('div:eq(0)').attr('id');
        if (errorTab === 'Details') {
            $('#btndetails').trigger('click');
        }
        else if (errorTab === 'SingleSignOn') {
            $('#userSingleSignOntabedit').trigger('click');
        }
        else {
            $('#useraddresstabedit').trigger('click');
        }
    }
});
//#region print
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var jsonResult = $.ajax({
                "url": "/UserManagement/UserDetailsPrintData",
                "type": "get",
                "datatype": "json",
                data: {                  
                    "CaseNo": CaseNo,
                    "UserName": LRTrim($("#UserName").val()),
                    "LastName": LRTrim($("#LastName").val()),
                    "FirstName": LRTrim($("#FirstName").val()),
                    "Email": LRTrim($("#Email").val()),
                    "CraftId": $("#CraftId").val() == "" ? 0 : $("#CraftId").val(),
                    "order": order,
                    "orderDir": orderDir,
                    "SelectedSites": $('#Sites option:selected').toArray().map(item => item.value).toString(),
                    "SearchText": LRTrim($("#txtColumnSearch").val()),
                   "SecurityProfileIds": $('#SecurityProfileId option:selected')
                        .toArray().map(item => item.value).toString(),
                    "UserTypes":$('#UserType option:selected').toArray().map(item => item.value).toString(),
                    "Shifts": $('#Shift option:selected').toArray().map(item => item.value).toString(),
                    "IsActive":$("#IsActive").val(),
                    "EmployeeId": $("#EmployeeId").val()
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = [];

            if ($('#PackageLevel').val().toUpperCase() === PackageLevel && $('#IsSuperUser').val() === 'True') {
                visiblecolumnsIndex = $("#userManagmentTable thead tr th").not(":eq(0)").map(function (key) {
                    return this.getAttribute('data-th-index');
                }).get();
            }
            else {
                visiblecolumnsIndex = $("#userManagmentTable thead tr th").map(function (key) {
                    return this.getAttribute('data-th-index');
                }).get();
            }

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
                if (item.EmployeeId != null) {
                    item.EmployeeId = item.EmployeeId;
                }
                else {
                    item.EmployeeId = "";
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
            if ($('#PackageLevel').val().toUpperCase() === PackageLevel && $('#IsSuperUser').val() === 'True') {
                header = $("#userManagmentTable thead tr th").not(":eq(0)").map(function (key) {
                    if ($(this).parents('.userManagementinnerDataTable').length == 0 && this.innerHTML) {
                        return this.innerHTML;
                    }
                }).get();
            }
            else {
                header = $("#userManagmentTable thead tr th").map(function (key) {
                    return this.innerHTML;
                }).get();
            }
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
    var colOrder = [];
    if ($('#PackageLevel').val().toUpperCase() === PackageLevel && $('#IsSuperUser').val() === 'True') 
        colOrder = [0, 1];
    else
        colOrder = [0];
    funCustozeSaveBtn(userManagmentTable, colOrder);
    run = true;
    userManagmentTable.state.save(run);
});
//#endregion

//#region V2-332
function UserTypeSelect(userType) {
    if (userType) {
        userType = userType.toLowerCase();
        switch (userType) {
            case 'full':
                $(document).find("#liCraft").show();
                $(document).find("#liEmail").show();
                $(document).find("#liBuyer").show();
                $(document).find("#EmailValidShow").hide();
                break;
            case 'admin':
                $(document).find("#liCraft").show();
                $(document).find("#liEmail").show();
                $(document).find("#liBuyer").show();
                $(document).find("#EmailValidShow").show();
                break;
            case 'workrequest':
                $(document).find("#liCraft").show();
                $(document).find("#liEmail").show();
                $(document).find("#liBuyer").hide();
                $(document).find("#userModels_Buyer").prop("checked", false);
                $(document).find("#EmailValidShow").hide();
                break;
            case 'enterprise':
                $(document).find("#liCraft").show();
                $(document).find("#liEmail").show();
                $(document).find("#liBuyer").hide();
                $(document).find("#userModels_Buyer").prop("checked", false);
                $(document).find("#EmailValidShow").hide();
                break;
            case 'reference':
                $(document).find("#liCraft").show();
                $(document).find("#liEmail").hide();
                $(document).find("#liEmail").val('');
                $(document).find("#liBuyer").hide();
                $(document).find("#userModels_Buyer").prop("checked", false);
                $(document).find("#EmailValidShow").hide();
                break;
            case 'production':
                $(document).find("#liCraft").show();
                $(document).find("#liEmail").show();
                $(document).find("#liBuyer").hide();
                $(document).find("#userModels_Buyer").prop("checked", false);
                $(document).find("#EmailValidShow").hide();
                break;
            default:
                $(document).find("#liCraft").hide();
                $(document).find("#liEmail").hide();
                $(document).find("#liBuyer").hide();
                $(document).find("#EmailValidShow").hide();
                $(document).find("#MiddleNameValidShow").hide();
                $(document).find("#LastNameValidShow").show();
        }
    }
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
    //if ($(this).attr('id') == '3') {
    //    siteList = localStorage.getItem('SITELIST');
    //    if (siteList) {
    //        selectedSiteNames = siteList.split(',');
    //        if (selectedSiteNames != null && selectedSiteNames.length > 0) {
    //            var selectedValues = new Array();
    //            for (var i = 0; i < selectedSiteNames.length; i++) {
    //                selectedValues[i] = selectedSiteNames[i];
    //            }
    //            $("#ddlSiteId").val(selectedValues).trigger('change.select2');
    //        }
    //    }
    //    $(document).find('#searcharea').hide("slide");
    //    $(document).find('#SitesUsersModal').modal('show');
    //    return;
    //}
    //else {
    //    selectedSiteNames = [];
    //    localStorage.removeItem('SITELIST');
    //    siteList = "";
    //    $("#ddlSiteId").val('').trigger("change.select2");
    //}

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
    localStorage.setItem("USERMANAGEMENTSTATUS", CaseNo);
    localStorage.setItem('USERMANAGEMENTTITLE', $('#usersearchListul #' + CaseNo).text());
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
    localStorage.setItem("USERMANAGEMENTSTATUS", CaseNo);
    localStorage.setItem('USERMANAGEMENTTITLE', $('#usersearchListul #' + CaseNo).text());

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
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'UserManagement' },
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
        data: { tableName: 'UserManagement', searchText: txtSearchval, isClear: isClear },
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
        else if (item.key == 'SecurityProfileId' && item.value)
        {
            $("#SecurityProfileId").val(item.value).trigger("change.select2");
            if (item.value) {
                if (item.value != "") {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
        }
        else if (item.key == 'Sites' && item.value) {
            $("#Sites").val(item.value).trigger("change.select2");
            if (item.value) {
                if (item.value != "") {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
        }
        else if (item.key == 'Shift' && item.value) {
            $("#Shift").val(item.value).trigger("change.select2");
            if (item.value) {
                if (item.value != "") {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
        }
        else if (item.key == 'UserType' && item.value) {
            $("#UserType").val(item.value).trigger("change.select2");
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

//#region V2-417 Inactivate and Active Users
$(document).on('click', '#ActivateInactivateUser', function () {
    var _userInfoid = $(this).attr('data-id');
    var inactiveFlag = $(document).find('#UserStatus').val();
    $.ajax({
        url: "/UserManagement/ValidateUserStatusChange",
        type: "POST",
        dataType: "json",
        data: { _userInfoid: _userInfoid, inactiveFlag: inactiveFlag },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.validationStatus == true) {
                if (inactiveFlag == "True") {
                    CancelAlertSetting.text = getResourceValue("InActivateUserAlert");
                }
                else {
                    CancelAlertSetting.text = getResourceValue("ActivateUserAlert");                  
                }
                swal(CancelAlertSetting, function (isConfirm) {
                    if (isConfirm == true) {
                        $.ajax({
                            url: "/UserManagement/UpdateUserStatus",
                            type: "POST",
                            dataType: "json",
                            data: {
                                _userInfoid: _userInfoid,
                                inactiveFlag: inactiveFlag
                            },
                            beforeSend: function () {
                                ShowLoader();
                            },
                            success: function (data) {
                                if (data.result == 'success') {
                                    if (inactiveFlag == "True") {
                                        SuccessAlertSetting.text = getResourceValue("UserInActiveSuccessAlert");
                                        localStorage.setItem("USERMANAGEMENTSTATUS", "2");                                       
                                        titleText = getResourceValue("AlertInactive");
                                    }
                                    else {
                                        SuccessAlertSetting.text = getResourceValue("UserActiveSuccessAlert");  
                                        localStorage.setItem("USERMANAGEMENTSTATUS", "1");
                                        titleText = getResourceValue("AlertActive");
                                    }
                                    swal(SuccessAlertSetting, function () {
                                        $.ajax({
                                            url: "/UserManagement/UserManagementDetails",
                                            type: "POST",
                                            dataType: 'html',
                                            data: { UserId: _userInfoid },
                                            beforeSend: function () {
                                                ShowLoader();
                                            },
                                            success: function (data) {
                                                $('#usermanagementcontainer').html(data);
                                                $(document).find('#spnlinkToSearch').text(titleText);
                                            },
                                            complete: function () {
                                                CloseLoader();
                                                
                                            },
                                            error: function () {
                                                CloseLoader();
                                            }
                                        });                                   
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
//#endregion

//#region V2-491 Unlock Account
$(document).on('click', '#unlockaccount', function () {
    var _userInfoid = $(this).attr('data-infoid'); 
    var _defaultSiteid = $(this).attr('data-siteid');
    var _personnelId = $(this).attr('data-personnelid');
    $.ajax({
        url: "/UserManagement/UnlockAccount",
        type: "POST",
        dataType: "json",
        data: { _userInfoid: _userInfoid, Siteid: _defaultSiteid, PersonnelId: _personnelId},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == 'success') {
                SuccessAlertSetting.text = "Account has been unlocked";
                swal(SuccessAlertSetting, function () {
                    RedirectTouserDetail(data.userInfoId, "userinformation");
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
//#endregion