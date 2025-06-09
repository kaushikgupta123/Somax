var dtProjMainTable;
var run = false;
var gridname = "Project_Search_new";
var PrjMainorder = '1';
var PrjMainorderDir = 'asc';
var dtinnerGrid;

var order = '1';
var orderDir = 'asc';
var CreateStartDateVw = '';
var CreateEndDateVw = '';
var CompleteStartDateVw = '';
var CompleteEndDateVw = '';
var CloseStartDateVw = '';
var CloseEndDateVw = '';
var CustomQueryDisplayId = 8;
var today = $.datepicker.formatDate('mm/dd/yy', new Date());
var selectCount = 0;
var status;

$(document).ready(function () {
    var strCreateStartDateVw = localStorage.getItem('ProCreateStartDateVw');
    if (strCreateStartDateVw) {
        CreateStartDateVw = strCreateStartDateVw;
    }
    var endCreateEndDateVw = localStorage.getItem('ProCreateEndDateVw');
    if (endCreateEndDateVw) {
        CreateEndDateVw = endCreateEndDateVw;
    }

    var strCompleteStartDateVw = localStorage.getItem('ProCompleteStartDateVw');
    if (strCompleteStartDateVw) {
        CompleteStartDateVw = strCompleteStartDateVw;
    }
    var endCompleteEndDateVw = localStorage.getItem('ProCompleteEndDateVw');
    if (endCompleteEndDateVw) {
        CompleteEndDateVw = endCreateEndDateVw;
    }

    var strCloseStartDateVw = localStorage.getItem('ProCloseStartDateVw');
    if (strCloseStartDateVw) {
        CloseStartDateVw = strCloseStartDateVw;
    }
    var endCloseEndDateVw = localStorage.getItem('ProCloseEndDateVw');
    if (endCloseEndDateVw) {
        CloseEndDateVw = endCloseEndDateVw;
    }

    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    var Projstatus = localStorage.getItem("Projstatus");
    if (Projstatus) {
        var text = "";
        CustomQueryDisplayId = Projstatus;

        if (Projstatus === '1' || Projstatus === '2' || Projstatus === '3' ||
            Projstatus === '4' || Projstatus === '5' || Projstatus === '6') {
            $('#cmbcreateview').val(Projstatus).trigger('change');
            $("#projectsearchListul li").removeClass("activeState");
            $("#projectsearchListul li[id='1']").addClass("activeState");
            text = $("#projectsearchListul li[id='1']").text();
            $('#projectsearchtitle').text(text);
        }
        else if (Projstatus === '9' || Projstatus === '10' || Projstatus === '11' ||
            Projstatus === '12' || Projstatus === '13' || Projstatus === '14') {
            $('#cmbcompletedview').val(Projstatus).trigger('change');
            $("#projectsearchListul li").removeClass("activeState");
            $("#projectsearchListul li[id='9']").addClass("activeState");
            text = $("#projectsearchListul li[id='9']").text();
            $('#projectsearchtitle').text(text);
        }
        else if (Projstatus === '15' || Projstatus === '16' || Projstatus === '17' ||
            Projstatus === '18' || Projstatus === '19' || Projstatus === '20') {
            $('#cmbclosedview').val(Projstatus).trigger('change');
            $("#projectsearchListul li").removeClass("activeState");
            $("#projectsearchListul li[id='15']").addClass("activeState");
            text = $("#projectsearchListul li[id='15']").text();
            $('#projectsearchtitle').text(text);
        }

        $('#projectsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId) {
                text = $(this).text();
                $('#projectsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });

        if (Projstatus === '1' || Projstatus === '2' || Projstatus === '3' ||
            Projstatus === '4' || Projstatus === '5' || Projstatus === '6') {
            if (Projstatus === '6')
                text = text + " - " + $('#createdaterange').val();
            else
                text = text + " - " + $(document).find('#cmbcreateview option[value=' + Projstatus + ']').text();
            $('#projectsearchtitle').text(text);
        }
        else if (Projstatus === '9' || Projstatus === '10' || Projstatus === '11' ||
            Projstatus === '12' || Projstatus === '13' || Projstatus === '14') {
            if (Projstatus === '14')
                text = text + " - " + $('#completeddaterange').val();
            else
                text = text + " - " + $(document).find('#cmbcompletedview option[value=' + Projstatus + ']').text();
            $('#projectsearchtitle').text(text);
        }
        else if (Projstatus === '15' || Projstatus === '16' || Projstatus === '17' ||
            Projstatus === '18' || Projstatus === '19' || Projstatus === '20') {
            if (Projstatus === '20')
                text = text + " - " + $('#closeddaterange').val();
            else
                text = text + " - " + $(document).find('#cmbclosedview option[value=' + Projstatus + ']').text();
            $('#projectsearchtitle').text(text);
        }
    }
    else {
        Projstatus = "8";
        CustomQueryDisplayId = Projstatus;
        localStorage.setItem("Projstatus", "8");
        $('#projectsearchListul li#8').addClass('activeState');
        $('#projectsearchtitle').text(getResourceValue("OpenProjectsAlert"));
    }
    generateProjDataTable();

});

//#region Generate Search Grid
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
function generateProjDataTable() {

    var printCounter = 0;
    if ($(document).find('#projectCostingSearch').hasClass('dataTable')) {
        dtProjMainTable.destroy();
    }
    dtProjMainTable = $("#projectCostingSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 2
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
                    data.order[0][0] = PrjMainorder;
                    data.order[0][1] = PrjMainorderDir;
                }
                var filterinfoarray = getfilterinfoarrayproj($("#txtColumnSearch"), $('#advsearchsidebarProj'));
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
                        PrjMainorder = LayoutInfo.order[0][0];
                        PrjMainorderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchuiproj(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#spnControlCounter"), $("#advsearchfilteritems"));
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
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Project Costing',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'main-search'
                }

            },
            {
                extend: 'print',
                title: 'Project Costing',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'main-search'
                }

            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Project Costing',
                extension: '.csv',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'main-search',
                    customizeData: function (d) {
                        var exportBody = ExportMainGrid1();
                        d.body.length = 0;
                        d.body.push.apply(d.body, exportBody);
                    }

                }
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                orientation: 'landscape',
                pageSize: 'A4',
                title: 'Project Costing',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'main-search'
                }
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/ProjectCosting/GetProjectGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.CustomQueryDisplayId = localStorage.getItem("Projstatus");
                d.ClientlookupId = LRTrim($("#ProjectID").val());
                d.Description = LRTrim($("#Description").val());
                d.CreateStartDateVw = ValidateDate(CreateStartDateVw);
                d.CreateEndDateVw = ValidateDate(CreateEndDateVw);
                d.CompleteStartDateVw = ValidateDate(CompleteStartDateVw);
                d.CompleteEndDateVw = ValidateDate(CompleteEndDateVw);
                d.CloseStartDateVw = ValidateDate(CloseStartDateVw);
                d.CloseEndDateVw = ValidateDate(CloseEndDateVw);
                d.Status = $("#gridadvsearchstatus").val().join();
                d.AssignedAssetGroup1 = LRTrim($("#AssignedAssetGroup1").val());
                d.AssignedAssetGroup2 = LRTrim($("#AssignedAssetGroup2").val());
                d.AssignedAssetGroup3 = LRTrim($("#AssignedAssetGroup3").val());
                d.txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
                d.Order = PrjMainorder;
            },
            "dataSrc": function (result) {
                let colOrder = dtProjMainTable.order();
                PrjMainorderDir = colOrder[0][1];

                var i = 0;
                totalcount = result.recordsTotal;

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
                    "data": "ClientlookupId",
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
                { "data": "ActualStart", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2", },
                { "data": "ActualFinish", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5",
                   
                },
                {
                    "data": "Budget", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "8", "class": " text-right",
                    
                },
                {
                    "data": "AG1ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "9",
                    
                },
                {
                    "data": "AG2ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "10",
                    
                },
                {
                    "data": "AG3ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "11",
                    
                },
                { "data": "Created", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
                { "data": "CompleteDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date ", "name": "7" },
                {
                    "data": "Coordinator", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "12",
                   
                }

            ],

        initComplete: function (settings, json) {
            SetPageLengthMenu();
            //----------conditional column hiding-------------//
            //var api = new $.fn.dataTable.Api(settings);
            //var columns = dtProjMainTable.settings().init().columns;
            //var arr = [];
            //var j = 0;

            //----------------------------------------------//

            $("#ProjectGridAction :input").removeAttr("disabled");
            $("#ProjectGridAction :button").removeClass("disabled");
            DisableExportButton($("#projectCostingSearch"), $(document).find('.import-export'));
        }
    });
}
function getfilterinfoarrayproj(txtsearchelement, advsearchcontainer) {
    var filterinfoarray = [];
    var f = new filterinfo('searchstringproject', LRTrim(txtsearchelement.val()));
    filterinfoarray.push(f);
    advsearchcontainer.find('.adv-item').each(function (index, item) {
        if ($(this).parent('div').is(":visible")) {
            f = new filterinfo($(this).attr('id'), $(this).val());
            filterinfoarray.push(f);
        }
        if ($(this).parent('div').find('div').hasClass('range-timeperiod')) {
            if ($(this).parent('div').find('input').val() !== '' && $(this).val() == '10') {
                f = new filterinfo('this-' + $(this).attr('id'), $(this).parent('div').find('input').val());
                filterinfoarray.push(f);
            }
        }
    });
    return filterinfoarray;
}
function setsearchuiproj(data, txtsearchelement, advcountercontainer, searchstringcontainer) {
    var searchitemhtml = '';
    $.each(data, function (index, item) {
        if (item.key == 'searchstringproject' && item.value) {
            var txtSearchval = item.value;
            if (item.value) {
                txtsearchelement.val(txtSearchval);
                searchitemhtml = "";
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + txtsearchelement.attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossProj" aria-hidden="true"></a></span>';
            }
            return false;
        }
        else {
            if ($('#' + item.key).parent('div').is(":visible")) {
                $('#' + item.key).val(item.value);
                if (item.value && item.value.length > 0) {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossProj" aria-hidden="true"></a></span>';
                }
            }
            advcountercontainer.text(selectCount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
}
//#endregion

$(document).on('click', '.lnk_psearch', function (e) {
    e.preventDefault();
    var index_row = $('#projectCostingSearch tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtProjMainTable.row(row).data();
    var ProjectId = data.ProjectId;
    var clientLookupId = data.ClientlookupId;
    var titletext = $('#projectsearchtitle').text();
    localStorage.setItem("Projstatustext", titletext);
    $.ajax({
        url: "/ProjectCosting/Details",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { ProjectId: ProjectId, ClientLookupId: clientLookupId },
        success: function (data) {
            $('#projectmaincontainer').html(data);
            $(document).find('#spnlinkToSearch').text(titletext);
        },
        complete: function () {
            SetFixedHeadStyle();
            LoadActivity(ProjectId);
            CloseLoader();
        }
    });
});
$(document).on('click', '#projectCostingSearch .paginate_button', function () {
    run = true;
});
$(document).on('change', '#projectCostingSearch .searchdt-menu', function () {
    run = true;
});
$('#projectCostingSearch').find('th').click(function () {
    if ($(this).data('col')) {
        run = true;
        PrjMainorder = $(this).data('col');
    }

});

//#region Advance filter
$(document).on('click', '#sidebarCollapse', function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$("#btnProjectDataAdvSrch").on('click', function (e) {
    run = true;
    $(document).find('#txtColumnSearch').val('');
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    status = $("#gridadvsearchstatus").val();
    ProjectAdvSearch();
    dtProjMainTable.page('first').draw('page');
});
function ProjectAdvSearch() {
    var InactiveFlag = false;
    $('#txtColumnSearch').val('');
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebarProj').find('.adv-item').each(function (index, item) {

        if ($(this).val() && $(this).val().length > 0) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossProj" aria-hidden="true"></a></span>';
        }
        if ($(this).attr('id') == "gridadvsearchstatus") {
            if ($(this).val() == null && status != null) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossProj" aria-hidden="true"></a></span>';
            }
        }

    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
   
    $("#advsearchfilteritems").html(searchitemhtml);
    $("#spnControlCounter").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}

function clearAdvanceSearch() {
    selectCount = 0;
    $("#spnControlCounter").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
    $("#ProjectID").val("");
    $("#Description").val("");
    $("#WorkOrderId").val("");
    $("#AssignedAssetGroup1").val("");
    $("#AssignedAssetGroup2").val("");
    $("#AssignedAssetGroup3").val("");
    $("#gridadvsearchstatus").val("").trigger('change');
    status = $('#gridadvsearchstatus').val();

}
//#endregion

//#region Search
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'Project_Search' },
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
        data: { tableName: 'Project_Search', searchText: txtSearchval, isClear: isClear },
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
                dtProjMainTable.page('first').draw('page');
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
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossProj" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        dtProjMainTable.page('first').draw('page');
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
}
$(document).on('click', '.btnCrossProj', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    if (searchtxtId == "gridadvsearchstatus") {
        $(document).find("#gridadvsearchstatus").val("").trigger('change.select2');
        ServiceOrderShift = "";
    }
   
    ProjectAdvSearch();
    dtProjMainTable.page('first').draw('page');
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
//#endregion

//#region DetailPageSwitchTab
$(document).on('click', '.wop-det-tab', function (e) {
    ResetOtherTabs();
    var tab = $(this).data('tab');
    switch (tab) {
        case "Project":
            // Loading code
            LoadProjectTab();
            break;
        case "WorkOrders":
            LoadWorkOrdersTab();
            break;
        case "Purchasing":
            LoadPurchasingTab();
            break;
        case "Dashboard":
            LoadDashboardTab();
            break;
    }
    SwitchTab(e, tab);
});
function SwitchTab(evt, tab) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(tab).style.display = "block";

    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
}
function ResetOtherTabs() {
    //DestroyGanttChart();
    $(document).find('#Project').html('');
    $(document).find('#WorkOrders').html('');
    $(document).find('#Purchasing').html('');
    $(document).find('#Dashboard').html('');
}
//#endregion

//#region Side View click
$(document).on('click', '.Projectsearchdrpbox', function (e) {
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $(document).find('.chksearch').prop('checked', false);
    $('.itemcount').text(0);
    run = true;


    if ($(this).attr('id') == '1') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("Projstatus");
        if (val == '1' || val == '2' || val == '3' || val == '4' || val == '5' || val == '6') {
            $('#cmbcreateview').val(val).trigger('change');
        }
        $(document).find('#ProjDateRangeModalForAllStatus').modal('show');
        return;
    }
    else {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('ProCreateStartDateVw');
        localStorage.removeItem('ProCreateEndDateVw');
        $(document).find('#cmbcreateview').val('').trigger('change');
    }

    if ($(this).attr('id') == '9') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("Projstatus");
        if (val == '9' || val == '10' || val == '11' || val == '12' || val == '13' || val == '14') {
            $('#cmbcompletedview').val(val).trigger('change');
        }
        $(document).find('#ProjDateRangeModalForCompletedProject').modal('show');
        return;
    }
    else {
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        localStorage.removeItem('ProCompleteStartDateVw');
        localStorage.removeItem('ProCompleteEndDateVw');
        $(document).find('#cmbcompletedview').val('').trigger('change');
    }

    if ($(this).attr('id') == '15') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("Projstatus");
        if (val == '15' || val == '16' || val == '17' || val == '18' || val == '19' || val == '20') {
            $('#cmbclosedview').val(val).trigger('change');
        }
        $(document).find('#ProjDateRangeModalForClosedProject').modal('show');
        return;
    }
    else {
        CloseStartDateVw = '';
        CloseEndDateVw = '';
        localStorage.removeItem('ProCloseStartDateVw');
        localStorage.removeItem('ProCloseEndDateVw');
        $(document).find('#cmbclosedview').val('').trigger('change');
    }


    if ($(this).attr('id') != '8') {
        $('#projectsearchtitle').text($(this).text());
        localStorage.setItem("Projstatustext", $(this).text());
    }
    else {
        $('#projectsearchtitle').text(getResourceValue("OpenProjectsAlert"));
        localStorage.setItem("Projstatustext", getResourceValue("OpenProjectsAlert"));
    }

    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("Projstatus", optionval);
    CustomQueryDisplayId = optionval;


    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');


    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtProjMainTable.page('first').draw('page');
    }

});

$(document).on('keyup', '#projectsearctxtbox', function (e) {
    var tagElems = $(document).find('#projectsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});

//All status start
$(document).on('change', '#cmbcreateview', function (e) {
    var thielement = $(this);
    CustomQueryDisplayId = thielement.val();
    if (thielement.val() == '6') {
        var strtlocal = localStorage.getItem('ProCreateStartDateVw');
        if (strtlocal) {
            CreateStartDateVw = strtlocal;
        }
        else {
            CreateStartDateVw = today;
        }
        var endlocal = localStorage.getItem('ProCreateEndDateVw');
        if (endlocal) {
            CreateEndDateVw = endlocal;
        }
        else {
            CreateEndDateVw = today;
        }
        $(document).find('#timeperiodcontainerForCreateDate').show();
        $(document).find('#createdaterange').daterangepicker({
            format: 'MM/DD/YYYY',
            startDate: CreateStartDateVw,
            endDate: CreateEndDateVw,
            "locale": {
                "applyLabel": getResourceValue("JsApply"),
                "cancelLabel": getResourceValue("CancelAlert")
            }
        }, function (start, end, label) {
            CreateStartDateVw = start.format('MM/DD/YYYY');
            CreateEndDateVw = end.format('MM/DD/YYYY');
        });
    }
    else {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('ProCreateStartDateVw');
        localStorage.removeItem('ProCreateEndDateVw');
        $(document).find('#timeperiodcontainerForCreateDate').hide();
    }
});
$(document).on('click', '#btntimeperiodForCreateDate', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbcreateview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != '6') {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('ProCreateStartDateVw');
        localStorage.removeItem('ProCreateEndDateVw');
    }
    else {
        localStorage.setItem('ProCreateStartDateVw', CreateStartDateVw);
        localStorage.setItem('ProCreateEndDateVw', CreateEndDateVw);
    }
    $(document).find('#ProjDateRangeModalForAllStatus').modal('hide');
    var text = $('#projectsearchListul').find('li').eq(0).text();

    if (daterangeval != '6')

        text = text + " - " + $(document).find('#cmbcreateview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#createdaterange').val();

    $('#projectsearchtitle').text(text);
    $("#projectsearchListul li").removeClass("activeState");
    $("#projectsearchListul li").eq(0).addClass('activeState');
    localStorage.setItem("Projstatus", daterangeval);
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');

    CustomQueryDisplayId = daterangeval;
    localStorage.setItem("Projstatus", CustomQueryDisplayId);
    if (daterangeval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtProjMainTable.page('first').draw('page');
    }

});
//All status End

//Completed project start
$(document).on('change', '#cmbcompletedview', function (e) {
    var thielement = $(this);
    CustomQueryDisplayId = thielement.val();
    if (thielement.val() == '14') {
        var strtlocal = localStorage.getItem('ProCompleteStartDateVw');
        if (strtlocal) {
            CompleteStartDateVw = strtlocal;
        }
        else {
            CompleteStartDateVw = today;
        }
        var endlocal = localStorage.getItem('ProCompleteEndDateVw');
        if (endlocal) {
            CompleteEndDateVw = endlocal;
        }
        else {
            CompleteEndDateVw = today;
        }
        $(document).find('#timeperiodcontainerForcompletedDate').show();
        $(document).find('#completeddaterange').daterangepicker({
            format: 'MM/DD/YYYY',
            startDate: CompleteStartDateVw,
            endDate: CompleteEndDateVw,
            "locale": {
                "applyLabel": getResourceValue("JsApply"),
                "cancelLabel": getResourceValue("CancelAlert")
            }
        }, function (start, end, label) {
            CompleteStartDateVw = start.format('MM/DD/YYYY');
            CompleteEndDateVw = end.format('MM/DD/YYYY');
        });
    }
    else {
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        localStorage.removeItem('ProCompleteStartDateVw');
        localStorage.removeItem('ProCompleteEndDateVw');
        $(document).find('#timeperiodcontainerForcompletedDate').hide();
    }
});
$(document).on('click', '#btntimeperiodForcompleteDate', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbcompletedview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != '14') {
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        localStorage.removeItem('ProCompleteStartDateVw');
        localStorage.removeItem('ProCompleteEndDateVw');
    }
    else {
        localStorage.setItem('ProCompleteStartDateVw', CompleteStartDateVw);
        localStorage.setItem('ProCompleteEndDateVw', CompleteEndDateVw);
    }
    $(document).find('#ProjDateRangeModalForCompletedProject').modal('hide');
    var text = $('#projectsearchListul').find('li').eq(4).text();

    if (daterangeval != '14')

        text = text + " - " + $(document).find('#cmbcompletedview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#completeddaterange').val();

    $('#projectsearchtitle').text(text);
    $("#projectsearchListul li").removeClass("activeState");
    $("#projectsearchListul li").eq(4).addClass('activeState');
    localStorage.setItem("Projstatus", daterangeval);
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');

    CustomQueryDisplayId = daterangeval;
    localStorage.setItem("Projstatus", CustomQueryDisplayId);
    if (daterangeval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtProjMainTable.page('first').draw('page');
    }

});
//Completed project End


//Closed project start
$(document).on('change', '#cmbclosedview', function (e) {
    var thielement = $(this);
    CustomQueryDisplayId = thielement.val();
    if (thielement.val() == '20') {
        var strtlocal = localStorage.getItem('ProCloseStartDateVw');
        if (strtlocal) {
            CloseStartDateVw = strtlocal;
        }
        else {
            CloseStartDateVw = today;
        }
        var endlocal = localStorage.getItem('ProCloseEndDateVw');
        if (endlocal) {
            CloseEndDateVw = endlocal;
        }
        else {
            CloseEndDateVw = today;
        }
        $(document).find('#timeperiodcontainerForclosedDate').show();
        $(document).find('#closeddaterange').daterangepicker({
            format: 'MM/DD/YYYY',
            startDate: CloseStartDateVw,
            endDate: CloseEndDateVw,
            "locale": {
                "applyLabel": getResourceValue("JsApply"),
                "cancelLabel": getResourceValue("CancelAlert")
            }
        }, function (start, end, label) {
            CloseStartDateVw = start.format('MM/DD/YYYY');
            CloseEndDateVw = end.format('MM/DD/YYYY');
        });
    }
    else {
        CloseStartDateVw = '';
        CloseEndDateVw = '';
        localStorage.removeItem('ProCloseStartDateVw');
        localStorage.removeItem('ProCloseEndDateVw');
        $(document).find('#timeperiodcontainerForclosedDate').hide();
    }
});
$(document).on('click', '#btntimeperiodForcloseDate', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbclosedview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != '20') {
        CloseStartDateVw = '';
        CloseEndDateVw = '';
        localStorage.removeItem('ProCloseStartDateVw');
        localStorage.removeItem('ProCloseEndDateVw');
    }
    else {
        localStorage.setItem('ProCloseStartDateVw', CloseStartDateVw);
        localStorage.setItem('ProCloseEndDateVw', CloseEndDateVw);
    }
    $(document).find('#ProjDateRangeModalForClosedProject').modal('hide');
    var text = $('#projectsearchListul').find('li').eq(1).text();

    if (daterangeval != '20')

        text = text + " - " + $(document).find('#cmbclosedview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#closeddaterange').val();

    $('#projectsearchtitle').text(text);
    $("#projectsearchListul li").removeClass("activeState");
    $("#projectsearchListul li").eq(1).addClass('activeState');
    localStorage.setItem("Projstatus", daterangeval);
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');

    CustomQueryDisplayId = daterangeval;
    localStorage.setItem("Projstatus", CustomQueryDisplayId);
    if (daterangeval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtProjMainTable.page('first').draw('page');
    }

});
//Closed project End
//#endregion

//#region Export project
$(document).on('click', '#liPROPdf,#liPROPrint,#liPROExcel,#liPROCsv', function () {
    var thisid = $(this).attr('id');
    var TableHaederProp = [];
    function table(property, title) {
        this.property = property;
        this.title = title;
    }
    $("#projectCostingSearch thead tr th").map(function (key) {
        var thisdiv = $(this).find('div');
        if (thisdiv.html()) {
            if (this.getAttribute('data-th-prop')) {
                var tablearr = new table(this.getAttribute('data-th-prop'), thisdiv.html());
                TableHaederProp.push(tablearr);
            }
        }
    });
    var params = {
        tableHaederProps: TableHaederProp,
        colname: PrjMainorder,
        coldir: PrjMainorderDir,
        CustomQueryDisplayId: localStorage.getItem("Projstatus"),
        ClientlookupId: LRTrim($("#ProjectID").val()),
        Description: LRTrim($("#Description").val()),
        WorkOrderClientLookupId: LRTrim($("#WorkOrderId").val()),
        CreateStartDateVw: ValidateDate(CreateStartDateVw),
        CreateEndDateVw: ValidateDate(CreateEndDateVw),
        CompleteStartDateVw: ValidateDate(CompleteStartDateVw),
        CompleteEndDateVw: ValidateDate(CompleteEndDateVw),
        CloseStartDateVw: ValidateDate(CloseStartDateVw),
        CloseEndDateVw: ValidateDate(CloseEndDateVw),
        Status: $("#gridadvsearchstatus").val().join(),
        txtSearchval: LRTrim($(document).find('#txtColumnSearch').val()),
    };
    var ProjPrintParams = JSON.stringify({ 'projCostPrintParams': params });
    $.ajax({
        "url": "/ProjectCosting/ProjectSetPrintData",
        "data": ProjPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            if (thisid == 'liPROPdf') {
                window.open('/ProjectCosting/ProjExportASPDF?d=d', '_self');
            }
            else if (thisid == 'liPROPrint') {
                window.open('/ProjectCosting/ProjExportASPDF', '_blank');
            }
            else if (thisid == 'liPROExcel') {
                window.open('/ProjectCosting/ProjExportASPDF?d=excel', '_self');
            }
            else if (thisid == 'liPROCsv') {
                window.open('/ProjectCosting/ProjExportASPDF?d=csv', '_self');
            }
            return;
        }
    });
    $('#mask').trigger('click');
});
//#endregion



//#region common
function SetProjControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
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
    });
    $(document).find('.select2picker').select2({});
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true,
        minDate: new Date()
    }).inputmask('mm/dd/yyyy');


    SetFixedHeadStyle();
}


//#endregion

//#region Redirect to details
function RedirectToProjectCostingDetail(ProjectId, clientLookupId) {
    $.ajax({
        url: "/ProjectCosting/Details",
        type: "POST",
        dataType: "html",
        data: { ProjectId: ProjectId, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#projectmaincontainer').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("Projstatustext"));
        },
        complete: function () {
            LoadActivity(ProjectId);
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../ProjectCosting/Index?page=ProjectCosting";
});
//#endregion
