//#region Common 
var dtTable;
var statussearchval;
var selectCount = 0;
var gridname = "SanitationJob_Search";
var run = false;
var equipmentid = -1;
var CustomQueryDisplayId = "0";
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
var CreateStartDateVw = '';
var CreateEndDateVw = '';
var CompleteStartDateVw = '';
var CompleteEndDateVw = '';
var FailedStartDateVw = '';
var FailedEndDateVw = '';
var PassedStartDateVw = '';
var PassedEndDateVw = '';
var sjStatus = '';
var today = $.datepicker.formatDate('mm/dd/yy', new Date());
SanitationAllowedPrintNumber = 50;
//#endregion

function SanitationSelectedItem(SanitationJobId, ClientLookupId, Status) {
    this.SanitationJobId = SanitationJobId;
    this.ClientLookupId = ClientLookupId;
    this.Status = Status;
};
var SelectedSanitationCancel = [];
$(function () {
    $(".updateArea").hide();
    ShowbtnLoaderclass("LoaderDrop");
    ShowbtnLoader("btnsortmenu");
    $(document).on('click', "#action", function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $(document).on('focusout', "#action", function () {
        $(".actionDrop").fadeOut();
    });
    $(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $('#sidebarCollapse').on('click', function () {
        $('.sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    //#region for removeclass
    if ($("#AssetTree").val() == "False") {
        $('ul li span').removeClass('wthAdjstNew2');
    }
        //#endregion
    var strCreateStartDateVw = localStorage.getItem('SJCreateStartDateVw');
    if (strCreateStartDateVw) {
        CreateStartDateVw = strCreateStartDateVw;
    }
    var endCreateEndDateVw = localStorage.getItem('SJCreateEndDateVw');
    if (endCreateEndDateVw) {
        CreateEndDateVw = endCreateEndDateVw;
    }

    var strCompleteStartDateVw = localStorage.getItem('SJCompleteStartDateVw');
    if (strCompleteStartDateVw) {
        CompleteStartDateVw = strCompleteStartDateVw;
    }
    var endCompleteEndDateVw = localStorage.getItem('SJCompleteEndDateVw');
    if (endCompleteEndDateVw) {
        CompleteEndDateVw = endCompleteEndDateVw;
    }

    var strFailedStartDateVw = localStorage.getItem('SJFailedStartDateVw');
    if (strFailedStartDateVw) {
        FailedStartDateVw = strFailedStartDateVw;
    }
    var endFailedEndDateVw = localStorage.getItem('SJFailedEndDateVw');
    if (endFailedEndDateVw) {
        FailedEndDateVw = endFailedEndDateVw;
    }

    var strPassedStartDateVw = localStorage.getItem('SJPassedStartDateVw');
    if (strPassedStartDateVw) {
        PassedStartDateVw = strPassedStartDateVw;
    }
    var endPassedEndDateVw = localStorage.getItem('SJPassedEndDateVw');
    if (endPassedEndDateVw) {
        PassedEndDateVw = endPassedEndDateVw;
    }

    var sanitationjobstatus = localStorage.getItem("SANITATIONJOBSTATUS");
    if (sanitationjobstatus) {
        var text = "";
        CustomQueryDisplayId = sanitationjobstatus;

        if (sanitationjobstatus === '11' || sanitationjobstatus === '12' || sanitationjobstatus === '13' ||
            sanitationjobstatus === '14' || sanitationjobstatus === '15' || sanitationjobstatus === '16') {
            $('#cmbcreateview').val(sanitationjobstatus).trigger('change');
            $("#sanjobsearchListul li").removeClass("activeState");
            $("#sanjobsearchListul li[id='0']").addClass("activeState");
            text = $("#sanjobsearchListul li[id='0']").text();
            $('#sanjobsearchtitle').text(text);
        }
        else if (sanitationjobstatus === '17' || sanitationjobstatus === '18' || sanitationjobstatus === '19' ||
            sanitationjobstatus === '20' || sanitationjobstatus === '21' || sanitationjobstatus === '22') {
            $('#cmbcompleteview').val(sanitationjobstatus).trigger('change');
            $("#sanjobsearchListul li").removeClass("activeState");
            $("#sanjobsearchListul li[id='9']").addClass("activeState");
            text = $("#sanjobsearchListul li[id='9']").text();
            $('#sanjobsearchtitle').text(text);
        }
        else if (sanitationjobstatus === '23' || sanitationjobstatus === '24' || sanitationjobstatus === '25' ||
            sanitationjobstatus === '26' || sanitationjobstatus === '27' || sanitationjobstatus === '28') {
            $('#cmbfailedview').val(sanitationjobstatus).trigger('change');
            $("#sanjobsearchListul li").removeClass("activeState");
            $("#sanjobsearchListul li[id='8']").addClass("activeState");
            text = $("#sanjobsearchListul li[id='8']").text();
            $('#sanjobsearchtitle').text(text);
        }
        else if (sanitationjobstatus === '29' || sanitationjobstatus === '30' || sanitationjobstatus === '31' ||
            sanitationjobstatus === '32' || sanitationjobstatus === '33' || sanitationjobstatus === '34') {
            $('#cmbpassedview').val(sanitationjobstatus).trigger('change');
            $("#sanjobsearchListul li").removeClass("activeState");
            $("#sanjobsearchListul li[id='10']").addClass("activeState");
            text = $("#sanjobsearchListul li[id='10']").text();
            $('#sanjobsearchtitle').text(text);
        }

        generateSJDataTable();

        $('#sanitationJobSearchModel_TextSearchList').val(sanitationjobstatus).trigger('change.select2');

        $('#sanjobsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId) {
                //if ($(this).attr('id') != '0')
                //{
                //    $('#sanjobsearchtitle').text($(this).text());
                //}
                //else
                //{
                //    $('#sanjobsearchtitle').text(getResourceValue("SanitationJobsAlert"));
                //}   
                text = $(this).text();
                $('#sanjobsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
        if (sanitationjobstatus === '11' || sanitationjobstatus === '12' || sanitationjobstatus === '13' ||
            sanitationjobstatus === '14' || sanitationjobstatus === '15' || sanitationjobstatus === '16') {
            if (sanitationjobstatus === '16')
                text = text + " - " + $('#createdaterange').val();
            else
                text = text + " - " + $(document).find('#cmbcreateview option[value=' + sanitationjobstatus + ']').text();
            $('#sanjobsearchtitle').text(text);
        }
        else if (sanitationjobstatus === '17' || sanitationjobstatus === '18' || sanitationjobstatus === '19' ||
            sanitationjobstatus === '20' || sanitationjobstatus === '21' || sanitationjobstatus === '22') {
            if (sanitationjobstatus === '22')
                text = text + " - " + $('#completedaterange').val();
            else
                text = text + " - " + $(document).find('#cmbcompleteview option[value=' + sanitationjobstatus + ']').text();
            $('#sanjobsearchtitle').text(text);
        }
        else if (sanitationjobstatus === '23' || sanitationjobstatus === '24' || sanitationjobstatus === '25' ||
            sanitationjobstatus === '26' || sanitationjobstatus === '27' || sanitationjobstatus === '28') {
            if (sanitationjobstatus === '28')
                text = text + " - " + $('#faileddaterange').val();
            else
                text = text + " - " + $(document).find('#cmbfailedview option[value=' + sanitationjobstatus + ']').text();
            $('#sanjobsearchtitle').text(text);
        }
        else if (sanitationjobstatus === '29' || sanitationjobstatus === '30' || sanitationjobstatus === '31' ||
            sanitationjobstatus === '32' || sanitationjobstatus === '33' || sanitationjobstatus === '34') {
            if (sanitationjobstatus === '34')
                text = text + " - " + $('#passedaterange').val();
            else
                text = text + " - " + $(document).find('#cmbpassedview option[value=' + sanitationjobstatus + ']').text();
            $('#sanjobsearchtitle').text(text);
        }

    }
    else {
        if (!$("#IsJobAddFromDashboard").val()) {
            CustomQueryDisplayId = 1;
            generateSJDataTable();
            // $('#sanjobsearchtitle').text(getResourceValue("SanitationJobsAlert"));
            $('#sanjobsearchtitle').text(getResourceValue("OpenSanitationJobsAlert"));
            $("#sanjobsearchListul li").eq(1).addClass('activeState');
        }
    }

    $(document).find('.select2picker').select2({});
    $('#searchTable').DataTable();
    $('#searchTable2').DataTable();
    $(document).find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '#dismiss, .overlay', function () {
        $('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', '#drpDwnLink', function (e) {
        e.preventDefault();
        $(document).find("#drpDwn").slideToggle();
    });
    $(document).on('click', '#drpDwnLink2', function (e) {
        e.preventDefault();
        $(document).find("#drpDwn2").slideToggle();
    });
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        var a = $(this).val();
        if ($(this).val() === 'SanitationMenuOverview') {
            opendiv(evt, 'ChargeTo');
        }
        else {
            openCity(evt, $(this).val());
        }
        $('#' + $(this).val()).show();
    });
    $(document).on('click', "#sanitationMenuSidebar", function () {
        $(document).find('#btnrequestChargeto').addClass('active');
        $(document).find('#ChargeTo').show();
    });
});
$(document).on('click', "ul.vtabs li", function () {
    if ($(this).find('#drpDwnLink').length > 0 || $(this).find('#drpDwnLink2').length > 0) {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        return false;
    }
    else {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        $(".tabsArea").hide();
        var activeTab = $(this).find("a").attr("href");
        $(activeTab).fadeIn();
        return false;
    }
});
function openCity(evt, cityName) {
    evt.preventDefault();    
    switch (cityName) {
        case "Photos":
            LoadImages($('#JobDetailsModel_SanitationJobId').val());
            break;
        case "SJTasks":
            generateSjTaskGrid(0);
            break;
        case "SJLabor":
            generateSJLaborGrid();
            break;
        case "SJNotes":
            GenerateSJNotesGrid();
            break;
        case "SJAttachments":
            GenerateSJAttachmentsGrid();
            break;
        case "SJEventLog":
            GenerateSJEventLogGrid();
            break;
        case "SJAssignments":
            generateSJAssignmentGrid();
            break;
        case "SJTools":
            generateSJToolsGrid();
            break;
        case "SJChemicalSupplies":
            generateSJChemicalSuppliesGrid();
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
function opendiv(evt, cityName) {
    $("#SanitationMenuOverview").find("#btnrequestChargeto").addClass('active');
    document.getElementById(cityName).style.display = "block";
}
//#region Search
$("#btnSJDataAdvSrch").on('click', function (e) {
    run = true;
    searchresult = [];
    SJAdvSearch();
    dtTable.page('first').draw('page');
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
});
function SJAdvSearch() {
    var InactiveFlag = false;
    var searchitemhtml = "";
    $(document).find('#txtColumnSearch').val('');
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
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    $("#sanitationJobSearchModel_TextSearchList").val(0).trigger('change.select2');
    CustomQueryDisplayId = 1; selectCount
    $(document).find('#Extracted').val("").trigger('change');
    localStorage.removeItem("SANITATIONJOBSTATUS");
    clearAdvanceSearch();
    SJAdvSearch();
    dtTable.page('first').draw('page');
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    if (selectCount > 0) selectCount--;

    $(".filteritemcount").text(selectCount);
    if (searchtxtId == "Status") {
        $(document).find("#Status").val("").trigger('change.select2');
    }
    if (searchtxtId == "CreateBy") {
        $(document).find("#CreateBy").val("").trigger('change.select2');
    }
    if (searchtxtId == "Assigned") {
        $(document).find("#Assigned").val("").trigger('change.select2');
    }
    if (searchtxtId == "VeryfiedBy") {
        $(document).find("#VeryfiedBy").val("").trigger('change.select2');
    }
    if (searchtxtId == "Shift") {
        $(document).find("#Shift").val("").trigger('change.select2');
    }
    dtTable.page('first').draw('page');
});
function clearAdvanceSearch() {
    selectCount = 0;
    $("#JobId").val("");
    $("#Description").val("");
    $("#ChargeTo").val("");
    $("#ChargeToName").val("");
    $("#AssetGroup1_ClientLookUpId").val("");
    $("#AssetGroup2_ClientLookUpId").val("");
    $("#AssetGroup3_ClientLookUpId").val("");
    $("#Status").val("").trigger('change.select2');
    $('#Shift').val("").trigger('change.select2');
    $("#Created").val("");
    $('#CreateBy').val("").trigger('change.select2');
    $('#Assigned').val("").trigger('change.select2');
    $("#CompleteDate").val("");
    $('#VeryfiedBy').val("").trigger('change.select2');
    $("#VeryfiedDate").val("");
    if ($("#Extracted").length > 0) {
        $("#Extracted").val("").trigger('change.select2');
    }
    if ($("#ScheduledDate").length > 0) {
        $("#ScheduledDate").val("");
    }
    $("#advsearchfilteritems").html('');
    $(".filteritemcount").text(selectCount);
}
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

var order = '1';//V2-398 Sorting
var orderDir = 'asc';
function generateSJDataTable() {
    var printCounter = 0;
    if ($(document).find('#sanitationJobSearchTable').hasClass('dataTable')) {
        dtTable.destroy();
    }
    dtTable = $("#sanitationJobSearchTable").DataTable({
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
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }//V2-398 Sorting   
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
                        order = LayoutInfo.order[0][0];
                        orderDir = LayoutInfo.order[0][1];                        //V2-398 Sorting
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));

                        }
                    }
                    else {
                        callback(json);
                    }
                }
            });

        },
        scrollX: true,
        fixedColumns: {
            leftColumns: 2
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Sanitation List'
            },
            {
                extend: 'print',
                title: 'Sanitation List'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Sanitation List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Sanitation List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/SanitationJob/GetSantGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.CustomQueryDisplayId = CustomQueryDisplayId;
                d.ClientLookupId = LRTrim($("#JobId").val());
                d.Description = LRTrim($("#Description").val());
                d.ChargeTo_ClientLookupId = LRTrim($("#ChargeTo").val());
                d.ChargeTo_Name = LRTrim($('#ChargeToName').val());
                d.AssetLocation = LRTrim($('#AssetLocation').val());
                d.Status = LRTrim($("#Status").val());
                d.Shift = LRTrim($("#Shift").val());
                d.AssetGroup1_ClientLookUpId = LRTrim($('#AssetGroup1ClientLookUpId').val());
                d.AssetGroup2_ClientLookUpId = LRTrim($("#AssetGroup2ClientLookUpId").val());
                d.AssetGroup3_ClientLookUpId = LRTrim($("#AssetGroup3ClientLookUpId").val());
                d.CreateDate = ValidateDate($("#Created").val());
                d.CreateBy = LRTrim($("#CreateBy").val());
                d.Assigned = LRTrim($("#Assigned").val());
                d.CompleteDate = LRTrim(ValidateDate($("#CompleteDate").val()));
                d.VerifiedBy = LRTrim($("#VeryfiedBy").val());
                d.VerifiedDate = LRTrim(ValidateDate($("#VeryfiedDate").val()));
                d.Extracted = LRTrim($("#Extracted").val());
                d.ScheduledDate = LRTrim(ValidateDate($("#ScheduledDate").val()));
                //V2-398
                d.CreateStartDateVw = ValidateDate(CreateStartDateVw);
                d.CreateEndDateVw = ValidateDate(CreateEndDateVw);
                d.CompleteStartDateVw = ValidateDate(CompleteStartDateVw);
                d.CompleteEndDateVw = ValidateDate(CompleteEndDateVw);
                d.FailedStartDateVw = ValidateDate(FailedStartDateVw);
                d.FailedEndDateVw = ValidateDate(FailedEndDateVw);
                d.PassedStartDateVw = ValidateDate(PassedStartDateVw);
                d.PassedEndDateVw = ValidateDate(PassedEndDateVw);
                d.Order = order;//V2-398 Sorting
                // d.orderDir = orderDir;//V2-398 Sorting

            },
            "dataSrc": function (result) {
                let colOrder = dtTable.order();
                orderDir = colOrder[0][1];
                if (result.data.length < 1) {
                    $(document).find('.import-export').prop('disabled', true);
                }
                else {
                    $(document).find('.import-export').prop('disabled', false);
                }

                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
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
                    "data": "SanitationJobId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    targets: 0,

                    'render': function (data, type, full, meta) {
                        var found = SelectedSanitationCancel.some(function (el) {
                            return el.SanitationJobId === data;
                        });
                        if (!found) {
                            return '<input type="checkbox" data-eqid="' + data + '" class="chksearch"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                        else {
                            return '<input type="checkbox" checked="checked" data-eqid="' + data + '" class="chksearch"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                    }
                },
                {
                    "data": "ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "0",
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_psearch href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": true, "name": "1",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-300'>" + data + "</div>";
                    }
                },
                { "data": "ChargeTo_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2", },
                { "data": "ChargeTo_Name", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                { "data": "AssetLocation", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4",
                    mRender: function (data, type, full, meta) {
                        if (data == statusCode.Approved) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--yellow m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Canceled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--orange m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Complete) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--teal m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Denied) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--purple m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Open) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Scheduled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--grey m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.JobRequest) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--light-blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Pass) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Fail) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--red m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else {
                            return getStatusValue(data);
                        }
                    }
                },
                {
                    "data": "ShiftDescription", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5"
                },
                {
                    "data": "AssetGroup1_ClientLookUpId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6"
                },
                {
                    "data": "AssetGroup2_ClientLookUpId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7"
                },
                {
                    "data": "AssetGroup3_ClientLookUpId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "8"
                },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date ", "name": "9" },
                {
                    "data": "CreateByName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "10",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-100'>" + data + "</div>";
                    }
                },
                {
                    "data": "Assigned", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "11",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-100'>" + data + "</div>";
                    }
                },
                {
                    "data": "CompleteDate",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "type": "date",
                    "name": "12"
                },
                { "data": "VerifiedBy", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "13" },
                { "data": "VerifiedDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date", "name": "14" },
                {
                    "data": "Extracted", "autoWidth": true, "bSearchable": true, "bSortable": true, name: "colExtracted", "name": "15",
                    'mRender': function (data, type, full, meta) {
                        if (data == true) {
                            return 'Yes';
                        }
                        else {
                            return 'No';
                        }
                    }
                },
                { "data": "ScheduledDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date", name: "colScheduledDate", "name": "16" }
            ],
        "columnDefs": [
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            var extSanit = $(document).find('#sanitationJobSearchModel_ExternalSanitation').val();
            if (extSanit == "False") {
                dtTable.column('colExtracted:name').visible(false);
                dtTable.column('colScheduledDate:name').visible(true);
            }
            else {
                dtTable.column('colExtracted:name').visible(true);
                dtTable.column('colScheduledDate:name').visible(false);
            }

            $("#SanitationGridAction :input").removeAttr("disabled");
            $("#SanitationGridAction :button").removeClass("disabled");
            DisableExportButton($("#sanitationJobSearchTable"), $(document).find('.import-export'));
        }
    });
}

$(document).on('click', '#sanitationJobSearchTable_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#sanitationJobSearchTable_length .searchdt-menu', function () {
    run = true;
});
$('#sanitationJobSearchTable').find('th').click(function () {
    run = true;
    order = $(this).data('col');
});
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var JobId = LRTrim($("#JobId").val());
            var Description = LRTrim($("#Description").val());
            var ChargeTo = LRTrim($("#ChargeTo").val());
            var ChargeToName = LRTrim($('#ChargeToName').val());
            var AssetLocation = LRTrim($('#AssetLocation').val());
            var Status = LRTrim($("#Status").val());
            var Shift = LRTrim($("#Shift").val());
            var AssetGroup1_ClientLookUpId = LRTrim($('#AssetGroup1ClientLookUpId').val());
            var AssetGroup2_ClientLookUpId = LRTrim($("#AssetGroup2ClientLookUpId").val());
            var AssetGroup3_ClientLookUpId = LRTrim($("#AssetGroup3ClientLookUpId").val());
            var Created = LRTrim($("#Created").val());
            var CreateBy = LRTrim($("#CreateBy").val());
            var Assigned = LRTrim($("#Assigned").val());
            var CompleteDate = LRTrim($("#CompleteDate").val());
            var VeryfiedBy = LRTrim($("#VeryfiedBy").val());
            var VeryfiedDate = LRTrim($("#VeryfiedDate").val());

            var colname = order;//V2-398 Sorting
            var coldir = orderDir;//V2-398 Sorting
            var txtsearchval = LRTrim($("#txtColumnSearch").val());
            var jsonResult = $.ajax({
                "url": "/SanitationJob/GetSantPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    CustomQueryDisplayId: CustomQueryDisplayId,
                    ClientLookupId: JobId,
                    Description: Description,
                    ChargeTo_ClientLookupId: ChargeTo,
                    ChargeTo_Name: ChargeToName,
                    AssetLocation: AssetLocation,
                    Status: Status,
                    Shift: Shift,
                    AssetGroup1_ClientLookUpId: AssetGroup1_ClientLookUpId,
                    AssetGroup2_ClientLookUpId: AssetGroup2_ClientLookUpId,
                    AssetGroup3_ClientLookUpId: AssetGroup3_ClientLookUpId,
                    CreateDate: Created,
                    CreateBy: CreateBy,
                    Assigned: Assigned,
                    CompleteDate: CompleteDate,
                    VerifiedBy: VeryfiedBy,
                    VerifiedDate: VeryfiedDate,
                    colname: colname,
                    coldir: coldir,
                    txtSearchval: txtsearchval,
                    CreateStartDateVw: ValidateDate(CreateStartDateVw),
                    CreateEndDateVw: ValidateDate(CreateEndDateVw),
                    CompleteStartDateVw: ValidateDate(CompleteStartDateVw),
                    CompleteEndDateVw: ValidateDate(CompleteEndDateVw),
                    FailedStartDateVw: ValidateDate(FailedStartDateVw),
                    FailedEndDateVw: ValidateDate(FailedEndDateVw),
                    PassedStartDateVw: ValidateDate(PassedStartDateVw),
                    PassedEndDateVw: ValidateDate(PassedEndDateVw)
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#sanitationJobSearchTable thead tr th").not(":eq(0)").map(function (key) {
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

                if (item.ChargeTo_ClientLookupId != null) {
                    item.ChargeTo_ClientLookupId = item.ChargeTo_ClientLookupId;
                }
                else {
                    item.ChargeTo_ClientLookupId = "";
                }
                if (item.ChargeTo_Name != null) {
                    item.ChargeTo_Name = item.ChargeTo_Name;
                }
                else {
                    item.ChargeTo_Name = "";
                }
                if (item.AssetLocation != null) {
                    item.AssetLocation = item.AssetLocation;
                }
                else {
                    item.AssetLocation = "";
                }
                if (item.Status != null) {
                    item.Status = item.Status;
                }
                else {
                    item.Status = "";
                }
                if (item.Shift != null) {
                    item.Shift = item.Shift;
                }
                else {
                    item.Shift = "";
                }

                if (item.AssetGroup1_ClientLookUpId != null) {
                    item.AssetGroup1_ClientLookUpId = item.AssetGroup1_ClientLookUpId;
                }
                else {
                    item.AssetGroup1_ClientLookUpId = "";
                }

                if (item.AssetGroup2_ClientLookUpId != null) {
                    item.AssetGroup2_ClientLookUpId = item.AssetGroup2_ClientLookUpId;
                }
                else {
                    item.AssetGroup2_ClientLookUpId = "";
                }

                if (item.AssetGroup3_ClientLookUpId != null) {
                    item.AssetGroup3_ClientLookUpId = item.AssetGroup3_ClientLookUpId;
                }
                else {
                    item.AssetGroup3ClientLookUpId = "";
                }

                if (item.CreateDate != null) {
                    item.CreateDate = item.CreateDate;
                }
                else {
                    item.CreateDate = "";
                }
                if (item.CreateBy != null) {
                    item.CreateBy = item.CreateBy;
                }
                else {
                    item.CreateBy = "";
                }
                if (item.Assigned != null) {
                    item.Assigned = item.Assigned;
                }
                else {
                    item.Assigned = "";
                }
                if (item.CompleteDate != null) {
                    item.CompleteDate = item.CompleteDate;
                }
                else {
                    item.CompleteDate = "";
                }
                if (item.VerifiedBy != null) {
                    item.VerifiedBy = item.VerifiedBy;
                }
                else {
                    item.VerifiedBy = "";
                }
                if (item.VerifiedDate != null) {
                    item.VerifiedDate = item.VerifiedDate;
                }
                else {
                    item.VerifiedDate = "";
                }
                if (item.Extracted != null) {
                    if (item.Extracted == true) {
                        item.Extracted = "Yes"
                    }
                    else {
                        item.Extracted = "No";
                    }
                }
                else {
                    item.Extracted = "";
                }
                if (item.ScheduledDate != null) {
                    item.ScheduledDate = item.ScheduledDate;
                }
                else {
                    item.ScheduledDate = "";
                }
                if (item.CompleteComments != null) {
                    item.CompleteComments = item.CompleteComments;
                }
                else {
                    item.CompleteComments = "";
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
                header: $("#sanitationJobSearchTable thead tr th").not(":eq(0)").find('div').map(function (key) {
                    if (this.innerHTML) {
                        return this.innerHTML;
                    }
                }).get()
            };
        }
    });
});
$(document).on('change', '.chksearch', function () {
    var data = dtTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        SelectedSanitationCancel = SelectedSanitationCancel.filter(function (el) {
            return el.SanitationJobId !== data.SanitationJobId;
        });
    }
    else {
        var item = new SanitationSelectedItem(data.SanitationJobId, data.ClientLookupId, data.Status);
        var found = SelectedSanitationCancel.some(function (el) {
            return el.SanitationJobId === data.SanitationJobId;
        });
        if (!found) { SelectedSanitationCancel.push(item); }
    }

    if (SelectedSanitationCancel.length > 0) {
        $(".actionBar").hide();
        $(".updateArea").fadeIn();
    }
    else {
        $(".updateArea").hide();
        $(".actionBar").fadeIn();
    }
    $('.itemcount').text(SelectedSanitationCancel.length);
});
$(document).on('click', '#cancelSatinationsJob', function () {
    run = true;
    if ($(document).find("#frmcancelsanitation").valid() == false) {
        return;
    }
    var cancelreason = $('#txtCancelReasonSelect').val();
    var comments = $('#txtcancelcomments').val();
    var jsonResult = {
        "list": SelectedSanitationCancel,
        "cancelreason": cancelreason,
        "comments": comments
    };
    $.ajax({
        url: '/SanitationJob/CancelSanitationList',
        type: "POST",
        datatype: "json",
        data: JSON.stringify(jsonResult),
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.data == "success") {
                SuccessAlertSetting.text = getResourceValue("jobCancelsuccessmsg");
                swal(SuccessAlertSetting, function () {
                    CloseLoader();
                });
            }
            else {
                // GenericSweetAlertMethod(data.data);
                swal({
                    title: getResourceValue("CommonErrorAlert"),
                    text: data.data,
                    type: "error",
                    showCancelButton: false,
                    confirmButtonClass: "btn-sm btn-danger",
                    cancelButtonClass: "btn-sm",
                    confirmButtonText: getResourceValue("SaveAlertOk"),
                    cancelButtonText: getResourceValue("CancelAlertNo")
                });
            }
            $(".updateArea").hide();
            $(".actionBar").fadeIn();
            $(document).find('.chksearch').prop('checked', false);
            $(document).find('.itemcount').text(0);
            SelectedSanitationCancel = [];
            dtTable.page('first').draw('page');
        },
        complete: function () {
            CloseLoader();
            $('#cancelModalSearchPage').modal('hide');
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('change', '#txtCancelReasonSelect', function () {
    if ($(this).val()) {
        $(this).removeClass('input-validation-error');
    }
    else {
        $(this).addClass('input-validation-error');
    }
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
function PdfPrintAllWoList(pdf) {
    var blob = b64StrtoBlob(pdf, 'application/pdf');
    var blobUrl = URL.createObjectURL(blob);
    window.open(blobUrl, "_blank");
}
function b64StrtoBlob(b64Data, contentType, sliceSize) {
    contentType = contentType || '';
    sliceSize = sliceSize || 512;
    var byteCharacters = atob(b64Data);
    var byteArrays = [];
    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        var slice = byteCharacters.slice(offset, offset + sliceSize);
        var byteNumbers = new Array(slice.length);
        for (var i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }
        var byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
    }
    var blob = new Blob(byteArrays, { type: contentType });
    return blob;
}
$(document).on('click', '#PrintSelectedSanitation', function () {
    if (SelectedSanitationCancel.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    //else if (SelectedSanitationCancel.length > SanitationAllowedPrintNumber) {
    //    var errorMessage = "You can select maximum " + SanitationAllowedPrintNumber + " records to proceed.";
    //    ShowErrorAlert(errorMessage);
    //    return false;
    //}
    else {
        var jsonResult = {
            "list": SelectedSanitationCancel,
            "cancelreason": "",
            "comments": "",
            "PrintingCountConnectionID": PrintingCountConnectionID
        }
        {
            $.ajax({
                url: '/SanitationJob/SetPrintSanitationJobListFromIndex', 
                data: JSON.stringify(jsonResult),
                type: "POST",
                datatype: "json",
                contentType: 'application/json; charset=utf-8',
                responseType: 'arraybuffer',
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (result) {
                    if (result.success) {
                        window.open("/SanitationJob/GenerateSanitationJobPrint", "_blank");
                    }
                },
                complete: function () {
                    CloseLoader();
                    $(".updateArea").hide();
                    $(".actionBar").fadeIn();
                    $(document).find('.chksearch').prop('checked', false);
                    $('.itemcount').text(0);
                    SelectedSanitationCancel = [];
                }
            });
        }
    }
});
//V2-1071

$(document).on('click', '#printSJ', function () {
    var SJArray = [];
    var jobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var clientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    var status = $(document).find('#SJStatus').val(); 
    var item = new SanitationSelectedItem(jobId, clientLookupId, status);
    SJArray.push(item);
    var jsonResult = {
        "list": SJArray,
        "cancelreason": "",
        "comments": "",
        "PrintingCountConnectionID": ""
    }
    $.ajax({
        url: '/SanitationJob/SetPrintSanitationJobListFromIndex',
        data: JSON.stringify(jsonResult),
        type: "POST",
        datatype: "json",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (result) {
            if (result.success) {
                window.open("/SanitationJob/GenerateSanitationJobPrint", "_blank");
            }
        },
        complete: function () {
            CloseLoader();
            WOArray = [];
        }
    });
    //$.ajax({
    //    url: '/WorkOrder/Print',
    //    data: { 'workOrderId': workorderId },

    //    type: "POST",
    //    datatype: "json",
    //    beforeSend: function () {
    //        ShowLoader();
    //    },
    //    success: function (result) {
    //        if (result.success && result.jsonStringExceed == false) {
    //            PdfPrintAllWoList(result.pdf);
    //        }
    //        else {
    //            CloseLoader();
    //            var errorMessage = getResourceValue("PdfFileSizeExceedAlert");
    //            ShowErrorAlert(errorMessage);
    //            return false;
    //        }
    //    },
    //    complete: function () {
    //        CloseLoader();

    //    }
    //});
});
//$(document).on('click', '#printSJ', function () {
//        var jsonResult = {
//            "list": SelectedSanitationCancel,
//            "cancelreason": "",
//            "comments": "",
//            "PrintingCountConnectionID": PrintingCountConnectionID
//        }
//        {
//            $.ajax({
//                url: '/SanitationJob/SetPrintSanitationJobListFromIndex',
//                data: JSON.stringify(jsonResult),
//                type: "POST",
//                datatype: "json",
//                contentType: 'application/json; charset=utf-8',
//                responseType: 'arraybuffer',
//                beforeSend: function () {
//                    ShowLoader();
//                },
//                success: function (result) {
//                    if (result.success) {
//                        window.open("/SanitationJob/GenerateSanitationJobPrint", "_blank");
//                    }
//                },
//                complete: function () {
//                    CloseLoader();
//                    $(".updateArea").hide();
//                    $(".actionBar").fadeIn();
//                    $(document).find('.chksearch').prop('checked', false);
//                    $('.itemcount').text(0);
//                    SelectedSanitationCancel = [];
//                }
//            });
//        }
//});

//
//#endregion
$(document).on('click', '.lnk_psearch', function (e) {
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();
    var titletext = $('#sanjobsearchtitle').text();
    localStorage.setItem("sanjobstatustext", titletext);
    $.ajax({
        url: "/SanitationJob/SJobDetails",
        type: "POST",
        data: { SanitationJobId: data.SanitationJobId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
            $(document).find('#spnlinkToSearch').text(titletext);
        },
        complete: function () {
            SetSanitationDetailEnvironment();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#editsanitation", function (e) {
    e.preventDefault();
    var JobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    $.ajax({
        url: "/SanitationJob/EditSanitationJobDetails",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: JobId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
            //#region for removeclass
            if ($("#AssetTree").val() == "False") {
                $('ul li span').removeClass('wthAdjstNew2');
            }
        //#endregion
        },
        complete: function () {
            CloseLoader();
            SetSJControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});

$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../SanitationJob/Index?page=Sanitation_Jobs_Search";
});
$(document).on('click', "#btnSaveSanitation,#btnCompleteSanitation", function () {
    if ($(document).find("#frmeditsanitation").valid()) {
        return;
    }
    else {
        //var errorTab = $(document).find('#frmeditsanitation').find(".input-validation-error").closest('.tabsArea').attr('id');
        //if (errorTab === 'RequestCharge') {
        //    $(document).find('#requesttab').trigger('click');
        //}
        //else {
        //    $(document).find('#statustab').trigger('click');
        //}
    }
});
$(document).on('click', "#btnCancelEditSanitation", function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    RedirectToDetailOncancel(SanitationJobId, "overview");
});
$(document).on('click', "#btnCancelAddSanitation,#BackToDetailsSanitation", function () {
    var SanitationJobId = $(this).attr('data-val');
    RedirectToDetailOncancel(SanitationJobId, "overview");
});
function RedirectToSaDetail(sanitationJobId, mode) {
    $.ajax({
        url: "/SanitationJob/SJobDetails",
        type: "POST",
        dataType: 'html',
        data: { SanitationJobId: sanitationJobId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("sanjobstatustext"));
        },
        complete: function () {
            SetSanitationDetailEnvironment();
            if (mode === "overview") {
                $('#lioverview').trigger('click');
            }
            if (mode === "tasks") {
                $('#sjtask').trigger('click');
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '#ImgShowAccount', function (e) {
    $("#ImgCloseAccount").show();
    $("#dvAccountContainer").show();
    $("#ImgShowAccount").hide();
});
$(document).on('click', '#ImgCloseAccount', function (e) {
    $("#dvAccountContainer").hide();
    $("#ImgShowAccount").show();
    $("#ImgCloseAccount").hide();
});
var PlantLocationId = -1;
$(document).on('click', '.txtTreeView', function () {
    $(this).blur();
    if ($(document).find('#SanitationPlantLocation').val() == 'True') {
        generateSanitationPlantLocationTree(-1);
    }
    else {
        generateTree(-1);
    }

});
$(document).on('click', '#pldArray', function (e) {
    $(this).blur();
    //if ($(document).find('#SanitationPlantLocation').val() == 'True') {
    //    generateSanitationPlantLocationTree(-1);
    //}
    //else {
    //    generateTree(-1);
    //}
    //V2-609
    generateSanitationAssetTree();
});

$(document).on('click', '.txtSanTreeView', function () {
    $(this).blur();
    if ($(document).find('#SanitationPlantLocation').val() == 'True') {
        generateSanitationPlantLocationTree(-1);
    }
    else {
        generateTree(-1);
    }

});
$(document).on('click', '#pldSanArray', function (e) {
    $(this).blur();
    //if ($(document).find('#SanitationPlantLocation').val() == 'True') {
    //    generateSanitationPlantLocationTree(-1);
    //}
    //else {
    //    generateTree(-1);
    //}
    //V2-609
    generateSanitationAssetTree();
});
$(document).ready(function () {
    if (localStorage.getItem("sanjobstatustext") === null) {
        localStorage.setItem("sanjobstatustext", getResourceValue("SanitationJobsAlert"));
    }
    $(".actionBar").fadeIn();
    $("#SanitationGridAction :input").attr("disabled", "disabled");
});

function generateSanitationPlantLocationTree(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/SanitationPlantLocationTreeLookup',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        cache: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find(".cntTree").html(data);
        },
        complete: function () {
            CloseLoader();
            $('#sanitationTreeModal').modal('show');
            treeTable($(document).find('#tblTree'));
            $(document).find('.radSelectSanitPl').each(function () {
                if ($(this).data('equipmentid') === equipmentid)
                    $(this).attr('checked', true);
            });
            // looking for the collapse icon and triggered click to collapse
            $.each($(document).find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,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"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element
        },
        error: function (xhr) {
            alert('error');
        }
    });
}
function generateTree(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/PlantLocationEquipmentTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        cache: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find(".cntTree").html(data);
        }
        ,
        complete: function () {
            CloseLoader();
            treeTable($(document).find('#tblTree'));
            $(document).find('.radSelect').each(function () {
                if ($(this).data('plantlocationid') === PlantLocationId)
                    $(this).attr('checked', true);
            });
            $('#sanitationTreeModal').modal('show');
            // looking for the collapse icon and triggered click to collapse
            $.each($(document).find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,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"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element
            CloseLoader();
        },
        error: function (xhr) {
            alert('error');
        }
    });
}
$(document).on('change', '.radSelectSanitPl', function () {
    equipmentid = $(this).data('equipmentid');
    var Description = $(this).data('description');
    var ChargeType = 'Equipment';
    var clientlookupid = $(this).data('clientlookupid');
    var SanitationId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    $(document).find('#TchargeType').val(ChargeType);
    $(document).find('#TplantLocationId').val(equipmentid);
    $(document).find('#TplantLocationDescription').val(Description);
    $(document).find('#JobDetailsModel_PlantLocationDescription').val(clientlookupid);
    $(document).find('#DemandModel_PlantLocationDescription').val(clientlookupid).trigger('change');
    $(document).find('#ODescribeModel_PlantLocationDescription').val(clientlookupid);
    $(document).find('#ODescribeModel_PlantLocationDescription').val(clientlookupid).trigger('change');
    $('#sanitationTreeModal').modal('hide');
});

$(document).on('change', '.radSelect', function () {

    PlantLocationId = $(this).data('plantlocationid');
    var Description = $(this).data('description');
    var ChargeType = $(this).data('chargetype');
    var SanitationId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    $(document).find('#TchargeType').val(ChargeType);
    $(document).find('#TplantLocationId').val(PlantLocationId);
    $(document).find('#TplantLocationDescription').val(Description);
    $(document).find('#JobDetailsModel_PlantLocationDescription').val(Description);
    $(document).find('#DemandModel_PlantLocationDescription').val(Description).trigger('change');
    $(document).find('#ODescribeModel_PlantLocationDescription').val(Description);
    $(document).find('#ODescribeModel_PlantLocationDescription').val(Description).trigger('change');
    $('#sanitationTreeModal').modal('hide');
});
function SanitationUpdateOnSuccess(data) {
    CloseLoader();
    if (data.data === "success") {
        if (data.Command === "save" || data.Command === "complete") {
            var message;
            if (data.mode === "add") {
                //CustomQueryDisplayId = "0";
                //localStorage.setItem("SANITATIONJOBSTATUS", CustomQueryDisplayId);
                //localStorage.setItem("sanjobstatustext", getResourceValue("SanitationJobsAlert")); 
                SuccessAlertSetting.text = getResourceValue("SanitationAddAlert");
            }
            else if (data.mode === "complete") {
                //CustomQueryDisplayId = "4";
                //localStorage.setItem("SANITATIONJOBSTATUS", CustomQueryDisplayId);
                //localStorage.setItem("sanjobstatustext", getResourceValue("CompletedLast30DaysAlert")); 
                SuccessAlertSetting.text = getResourceValue("SanitationCompleteAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("SanitationUpdateAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToSaDetail(data.SanitationJobId, "overview");
            });
        }
        else {
            ResetErrorDiv();
            $('#identificationtab').addClass('active').trigger('click');
            SuccessAlertSetting.text = getResourceValue("SanitationAddAlert");

            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        if (data.Command === "complete") {
            message = getResourceValue(data.Result);
            ShowGenericErrorOnAddUpdate(message);
        }
        else {
            ShowGenericErrorOnAddUpdate(data.Result);
        }
    }
}
$(document).on('click', "#AddORequestDemand", function (e) {
    var JobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var clientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    e.preventDefault();
    GoORequestDemand(JobId, clientLookupId);
    $('#AddSanitationRequest').modal('hide');
    $('.modal-backdrop').remove();
});
function GoORequestDemand(JobId, clientLookupId) {
    $.ajax({
        url: "/SanitationJob/AddRequestDemand",
        type: "POST",
        dataType: "html",
        data: { SanitationJobId: JobId, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
            //#region for removeclass
            if ($(document).find("#AssetTree").val() == "False") {
                $('ul li span').removeClass('wthAdjstNew2');
            }
        //#endregion
        },
        complete: function () {
            CloseLoader();
            SetSJControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#AddORequestDescribe", function (e) {
    var JobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var clientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    e.preventDefault();
    GoORequestDescribe(JobId, clientLookupId);
    $('#AddSanitationRequest').modal('hide');
    $('.modal-backdrop').remove();
  
});
function GoORequestDescribe(JobId, clientLookupId) {
    $.ajax({
        url: "/SanitationJob/AddRequestDescribe",
        type: "POST",
        dataType: "html",
        data: { SanitationJobId: JobId, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
            //#region for removeclass
            if ($(document).find("#AssetTree").val() == "False") {
                $('ul li span').removeClass('wthAdjstNew2');
            }
        //#endregion
        },
        complete: function () {
            CloseLoader();
            SetSJControls();

        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#AddOJobDemand", function (e) {
    var JobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var clientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    e.preventDefault();
    GoOJobDemand(JobId, clientLookupId);
    $('#AddSanitationJob').modal('hide');
    $('.modal-backdrop').remove();
   
});
function GoOJobDemand(JobId, clientLookupId) {
    $.ajax({
        url: "/SanitationJob/AddJobDemand",
        type: "POST",
        dataType: "html",
        data: { SanitationJobId: JobId, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
            //#region for removeclass
            if ($(document).find("#AssetTree").val() == "False") {
                $('ul li span').removeClass('wthAdjstNew2');
            }
        //#endregion
        },
        complete: function () {
            CloseLoader();
            SetSJControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#AddOJobDescribe", function (e) {
    var JobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var clientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    e.preventDefault();
    GoOJobDescribe(JobId, clientLookupId);
    $('#AddSanitationJob').modal('hide');
    $('.modal-backdrop').remove();
    //#region for removeclass
    if ($(document).find("#AssetTree").val() == "False") {
        $('ul li span').removeClass('wthAdjstNew2');
    }
        //#endregion
});
function GoOJobDescribe(JobId, clientLookupId) {
    $.ajax({
        url: "/SanitationJob/AddJobDescribe",
        type: "POST",
        dataType: "html",
        data: { SanitationJobId: JobId, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
            //#region for removeclass
            if ($(document).find("#AssetTree").val() == "False") {
                $('ul li span').removeClass('wthAdjstNew2');
            }
        //#endregion
        },
        complete: function () {
            CloseLoader();
            SetSJControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SetSJControls() {
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
    ZoomImage($(document).find('#EquipZoom'));
    $(document).find('.select2picker').select2({});
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    SetFixedHeadStyle();
}
$(document).on('click', "#btnCancelAddSanitationDashboard", function () {
    var returnToIndex = $(this).attr('data-val');
    RedirectToDetailOncancelForDashboard(returnToIndex);
});
function RedirectToDetailOncancelForDashboard(returnToIndex) {
    swal(CancelAlertSetting, function () {
        if (returnToIndex) {
            window.location.href = "../SanitationJob/Index?page=Sanitation_Jobs_Search";
        }
        else {
            window.location.href = '/Dashboard/Dashboard';
        }
    });
}
function ZoomImage(element) {
    element.elevateZoom(
        {
            zoomType: "window",
            lensShape: "round",
            lensSize: 1000,
            zoomWindowFadeIn: 500,
            zoomWindowFadeOut: 500,
            lensFadeIn: 100,
            lensFadeOut: 100,
            easing: true,
            scrollZoom: true,
            zoomWindowWidth: 450,
            zoomWindowHeight: 450
        });
}
function clearDropzone() {
    deleteServer = false;
    if ($(document).find('#dropzoneForm').length > 0) {
        Dropzone.forElement("div#dropzoneForm").destroy();
    }
}
function RedirectToSJDetail(SanitationJobId, mode) {
    $.ajax({
        url: "/SanitationJob/SJobDetails",
        type: "POST",
        dataType: 'html',
        data: { SanitationJobId: SanitationJobId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("sanjobstatustext"));
        },
        complete: function () {
            CloseLoader();
            LoadImages(SanitationJobId);
            SetSanitationDetailEnvironment();
            if (mode === "AzureImageReload" || mode === "OnPremiseImageReload") {
                $('#overviewcontainer').hide();
                $('#SanitationMenuOverview').hide();
                $('.tabcontent2').hide();
                $('#auditlogcontainer').hide();
                $('.imageDropZone').show();
                $(document).find('#btnnblock').removeClass("col-xl-6");
                $(document).find('#btnnblock').addClass("col-xl-12");
                $(document).find('#partst').removeClass("active");
                $(document).find('#photot').addClass("active");
            }
            if (mode === "overview") {
                $('#SanitationMenuOverview').trigger('click');
                $('#Detailstab').trigger('click');
                $('#colorselector').val('SanitationMenuOverview');
            }
            if (mode === "notes") {
                $('#SJNotestab').trigger('click');
                $('#colorselector').val('SJNotes');
            }
            if (mode === "attachment") {
                $('#SJAttachmentstab').trigger('click');
                $('#colorselector').val('SJAttachments');
            }
            if (mode === "labors") {
                $('#SJlabortab').trigger('click');
                $('#colorselector').val('SJLabor');
            }
            if (mode === "assignments") {
                $('#SJassignmenttab').trigger('click');
                $(document).find("#drpDwn").slideToggle();
                $('#colorselector').val('SJAssignments');
            }
            if (mode === "tasks") {
                $('#sjtask').trigger('click');
                $('#colorselector').val('SJTasks');
            }
            if (mode === "tools") {
                $('#SJtoolstab').trigger('click');
                $(document).find("#drpDwn").slideToggle();
                $('#colorselector').val('SJTools');
            }
            if (mode === "ChemicalSupplies") {
                $('#SJChemicalSuppliestab').trigger('click');
                $(document).find("#drpDwn").slideToggle();
                $('#colorselector').val('SJChemicalSupplies');
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SetSanitationDetailEnvironment() {
    CloseLoader();
    ZoomImage($(document).find('#EquipZoom'));
    Dropzone.autoDiscover = false;
    if ($(document).find('#dropzoneForm').length > 0) {
        var myDropzone = new Dropzone("div#dropzoneForm", {
            url: "../Base/SaveUploadedFile",
            addRemoveLinks: true,
            paramName: 'file',
            maxFilesize: 10, // MB
            maxFiles: 4,
            dictDefaultMessage: getResourceValue("FileUploadAlert"),
            acceptedFiles: ".jpeg,.jpg,.png",
            init: function () {
                this.on("removedfile", function (file) {
                    if (file.type != 'image/jpeg' && file.type != 'image/jpg' && file.type != 'image/png') {
                        ShowErrorAlert(getResourceValue("spnValidImage"));
                    }
                });
            },
            autoProcessQueue: true,
            dictRemoveFile: "X",
            uploadMultiple: true,
            dictRemoveFileConfirmation: getResourceValue("CancelAlertSure"),
            dictCancelUpload: "X",
            parallelUploads: 1,
            dictMaxFilesExceeded: "You can not upload any more files.",
            success: function (file, response) {
                var imgName = response;
                file.previewElement.classList.add("dz-success");
                var radImage = '<a class="lnkContainer setImage" data-toggle="tooltip" title="Upload a selective Image!" data-image="' + file.name + '" style="cursor:pointer;"><i class="fa fa-upload" style="cursor:pointer;"></i></a>';
                $(file.previewElement).append(radImage);
            },
            error: function (file, response) {
                if (file.size > (1024 * 1024 * 10)) // not more than 10mb
                {
                    ShowImageSizeExceedAlert();
                }
                file.previewElement.classList.add("dz-error");
                var _this = this;
                _this.removeFile(file);

            }
        });
    }
    SetFixedHeadStyle();
}
$(document).on('click', '.setImage', function () {
    var SanitationJobId = $('#JobDetailsModel_SanitationJobId').val();
    var imageName = $(this).data('image');
    $.ajax({
        url: '../Base/SaveUploadedFileToServer',
        type: 'POST',
        data: { 'fileName': imageName, objectId: SanitationJobId, TableName: "SanitationJob", AttachObjectName: "Sanitation" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {
            $('#EquipZoom').attr('src', data.imageurl);
            $('.equipImg').attr('src', data.imageurl);
            $(document).find('#AzureImage').append('<a href="javascript:void(0)" id="deleteImg" class="trashIcon" title="Delete"><i class="fa fa-trash"></i></a>');
            $('#EquipZoom').data('zoomImage', data.imageurl).elevateZoom(
                {
                    zoomType: "window",
                    lensShape: "round",
                    lensSize: 1000,
                    zoomWindowFadeIn: 500,
                    zoomWindowFadeOut: 500,
                    lensFadeIn: 100,
                    lensFadeOut: 100,
                    easing: true,
                    scrollZoom: true,
                    zoomWindowWidth: 450,
                    zoomWindowHeight: 450
                });
            $("#EquipZoom").on('load', function () {
                CloseLoader();
                ShowImageSaveSuccessAlert();
            });
            }
            else {
                CloseLoader();
                var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");    
                ShowErrorAlert(errorMessage);

            }
        },
        error: function () {
            CloseLoader();
        }
        //complete: function () { }
    });
});

//#region Photos
$(document).on('click', '#deleteImg', function () {
    var SanitationJobId = $('#JobDetailsModel_SanitationJobId').val();
    var ClientOnPremise = $('#JobDetailsModel_ClientOnPremise').val();
    if (ClientOnPremise == 'True') {
        DeleteOnPremiseImage(SanitationJobId);
    }
    else {
        DeleteAzureImage(SanitationJobId);
    }
  
});

function DeleteAzureImage(SanitationJobId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SanitationJob/DeleteImageFromAzure',
            type: 'POST',
            data: { _SanitationJobId: SanitationJobId, TableName: "SanitationJob", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToSJDetail(SanitationJobId, "AzureImageReload");
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}

function DeleteOnPremiseImage(SanitationJobId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SanitationJob/DeleteImageFromOnPremise',
            type: 'POST',
            data: { _SanitationJobId: SanitationJobId, TableName: "SanitationJob", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToSJDetail(SanitationJobId, "OnPremiseImageReload");
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
//#endregion
$(document).on('click', '#anchPhoto', function (e) {
    clearDropzone();
    var _SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();

   
    LoadImages(_SanitationJobId); //V2 - 841
   

    //$.ajax({
    //    url: "/SanitationJob/SJobDetails",
    //    type: "POST",
    //    data: { SanitationJobId: _SanitationJobId },
    //    dataType: 'html',
    //    beforeSend: function () {
    //    },
    //    success: function (data) {
    //    },
    //    complete: function () {
    //        SetSanitationDetailEnvironment();
    //    },
    //    error: function () {
    //    }
    //});

   

});

//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtTable,true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0, 1];
    funCustozeSaveBtn(dtTable, colOrder);
    run = true;
    dtTable.state.save(run);
});

//$(document).find('.srtsanjobcolumn').click(function () {
//    ShowbtnLoader("btnsortmenu");
//    order = $(this).data('col');//V2-398 Sorting
//    var txtColumnSearch = LRTrim($(document).find('#txtColumnSearch').val());
//    if (txtColumnSearch != "") {
//        TextSearch();//SJ Sorting 
//    }
//    else {
//        $('#sanitationJobSearchTable').DataTable().draw();//V2-398 Sorting
//    }    
//    $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());
//    $(document).find('.srtsanjobcolumn').removeClass('sort-active');
//    $(this).addClass('sort-active');
//    run = true;

//});
//$(document).find('.srtsanjoborder').click(function () {
//    ShowbtnLoader("btnsortmenu");  
//    orderDir = $(this).data('mode');//V2-398 Sorting
//    var txtColumnSearch = LRTrim($(document).find('#txtColumnSearch').val());
//    if (txtColumnSearch != "") {
//        TextSearch();//SJ Sorting 
//    }
//    else {
//        $('#sanitationJobSearchTable').DataTable().draw();//V2-398 Sorting
//    } 
//    $(document).find('.srtsanjoborder').removeClass('sort-active');
//    $(this).addClass('sort-active');
//    run = true;

//});



$(document).on('keyup', '#sanjobsearctxtbox', function (e) {
    var tagElems = $(document).find('#sanjobsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.sanjobsearchdrpbox', function (e) {
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#advsearchfilteritems").html('');
    $(document).find('#txtColumnSearch').val('');
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $(document).find('.chksearch').prop('checked', false);
    $('.itemcount').text(0);
    SelectedSanitationCancel = [];
    SelectPRId = [];
    run = true;
    if ($(this).attr('id') == '0') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("SANITATIONJOBSTATUS");
        if (val == '11' || val == '12' || val == '13' || val == '14' || val == '15' || val == '16') {
            $('#cmbcreateview').val(val).trigger('change');
        }
        $(document).find('#SJDateRangeModalForCreateDate').modal('show');

        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        FailedStartDateVw = '';
        FailedEndDateVw = '';
        PassedStartDateVw = '';
        PassedEndDateVw = '';
        return;
    }

    else {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('SJCreateStartDateVw');
        localStorage.removeItem('SJCreateEndDateVw');
        $(document).find('#cmbcreateview').val('').trigger('change');
    }
    if ($(this).attr('id') == '8') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("SANITATIONJOBSTATUS");
        if (val == '23' || val == '24' || val == '25' || val == '26' || val == '27' || val == '28') {
            $('#cmbfailedview').val(val).trigger('change');
        }
        $(document).find('#SJFailedDateRangeModal').modal('show');

        CreateStartDateVw = '';
        CreateEndDateVw = '';
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        PassedStartDateVw = '';
        PassedEndDateVw = '';
        return;
    }

    else {
        FailedStartDateVw = '';
        FailedEndDateVw = '';
        localStorage.removeItem('SJFailedStartDateVw');
        localStorage.removeItem('SJFailedEndDateVw');
        $(document).find('#cmbfailedview').val('').trigger('change');
    }
    if ($(this).attr('id') == '9') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("SANITATIONJOBSTATUS");
        if (val == '17' || val == '18' || val == '19' || val == '20' || val == '21' || val == '22') {
            $('#cmbcompleteview').val(val).trigger('change');
        }
        $(document).find('#SJDateRangeModal').modal('show');
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        FailedStartDateVw = '';
        FailedEndDateVw = '';
        PassedStartDateVw = '';
        PassedEndDateVw = '';
        return;
    }

    else {
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        localStorage.removeItem('SJCompleteStartDateVw');
        localStorage.removeItem('SJCompleteEndDateVw');
        $(document).find('#cmbcompleteview').val('').trigger('change');
    }
    if ($(this).attr('id') == '10') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("SANITATIONJOBSTATUS");
        if (val == '29' || val == '30' || val == '31' || val == '32' || val == '33' || val == '34') {
            $('#cmbpassedview').val(val).trigger('change');
        }
        $(document).find('#SJPassedDateRangeModal').modal('show');
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        FailedStartDateVw = '';
        FailedEndDateVw = '';

        return;
    }

    else {
        PassedStartDateVw = '';
        PassedEndDateVw = '';
        localStorage.removeItem('SJPassedStartDateVw');
        localStorage.removeItem('SJPassedEndDateVw');
        $(document).find('#cmbpassedview').val('').trigger('change');
    }

    if ($(this).attr('id') != '0') {
        $('#sanjobsearchtitle').text($(this).text());
        localStorage.setItem("sanjobstatustext", $(this).text());
    }
    else {
        $('#sanjobsearchtitle').text(getResourceValue("OpenSanitationJobsAlert"));
        localStorage.setItem("sanjobstatustext", getResourceValue("OpenSanitationJobsAlert"));
    }
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("SANITATIONJOBSTATUS", optionval);
    CustomQueryDisplayId = optionval;
    ShowbtnLoaderclass("LoaderDrop");
    dtTable.page('first').draw('page');
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
    var sanitationjobcurrentstatus = localStorage.getItem("SANITATIONJOBSTATUS");
    if (sanitationjobcurrentstatus) {
        CustomQueryDisplayId = sanitationjobcurrentstatus;
        $('#sanjobsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId && $(this).attr('id') != '0') {
                $('#sanjobsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
    }
    else { $('#sanjobsearchtitle').text(getResourceValue("SanitationJobsAlert")); }
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        dtTable.page('first').draw('page');
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
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
//#endregion

//#region New Search button
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'SanitationJob' },
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
        data: { tableName: 'SanitationJob', searchText: txtSearchval, isClear: isClear },
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
                dtTable.page('first').draw('page');
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
//#endregion


//#region V2-389
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
//#region V2-398
$(document).on('click', '#btntimeperiodForCreateDate', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbcreateview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != '16') {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('SJCreateStartDateVw');
        localStorage.removeItem('SJCreateEndDateVw');
    }
    else {
        localStorage.setItem('SJCreateStartDateVw', CreateStartDateVw);
        localStorage.setItem('SJCreateEndDateVw', CreateEndDateVw);
    }
    $(document).find('#SJDateRangeModalForCreateDate').modal('hide');
    var text = $('#sanjobsearchListul').find('li').eq(0).text();

    if (daterangeval != '16')

        text = text + " - " + $(document).find('#cmbcreateview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#createdaterange').val();

    $('#sanjobsearchtitle').text(text);
    localStorage.setItem("sanjobstatustext", text);
    $("#sanjobsearchListul li").removeClass("activeState");
    $("#sanjobsearchListul li").eq(0).addClass('activeState');
    localStorage.setItem("SANITATIONJOBSTATUS", daterangeval);
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');

    CustomQueryDisplayId = daterangeval;
    localStorage.setItem("SANITATIONJOBSTATUS", CustomQueryDisplayId);
    if (daterangeval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtTable.page('first').draw('page');
    }

});

$(document).on('click', '#btntimeperiod', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbcompleteview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != '22') {
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        localStorage.removeItem('SJCompleteStartDateVw');
        localStorage.removeItem('SJCompleteEndDateVw');
    }
    else {
        localStorage.setItem('SJCompleteStartDateVw', CompleteStartDateVw);
        localStorage.setItem('SJCompleteEndDateVw', CompleteEndDateVw);
    }
    $(document).find('#SJDateRangeModal').modal('hide');
    var text = $('#sanjobsearchListul').find('li').eq(5).text();

    //-------------------------------------------------------
    if (daterangeval != '22')
        text = text + " - " + $(document).find('#cmbcompleteview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#completedaterange').val();
    //-------------------------------------------------------

    $('#sanjobsearchtitle').text(text);
    localStorage.setItem("sanjobstatustext", text);
    $("#sanjobsearchListul li").removeClass("activeState");
    $("#sanjobsearchListul li").eq(5).addClass('activeState');
    localStorage.setItem("SANITATIONJOBSTATUS", daterangeval);
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');
    CustomQueryDisplayId = daterangeval;
    localStorage.setItem("SANITATIONJOBSTATUS", CustomQueryDisplayId);
    if (daterangeval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtTable.page('first').draw('page');
    }
});

$(document).on('click', '#btnfailedtimeperiod', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbfailedview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != '28') {
        FailedStartDateVw = '';
        FailedEndDateVw = '';
        localStorage.removeItem('SJFailedStartDateVw');
        localStorage.removeItem('SJFailedEndDateVw');
    }
    else {
        localStorage.setItem('SJFailedStartDateVw', FailedStartDateVw);
        localStorage.setItem('SJFailedEndDateVw', FailedEndDateVw);
    }
    $(document).find('#SJFailedDateRangeModal').modal('hide');
    var text = $('#sanjobsearchListul').find('li').eq(4).text();

    //-------------------------------------------------------
    if (daterangeval != '28')
        text = text + " - " + $(document).find('#cmbfailedview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#faileddaterange').val();
    //-------------------------------------------------------

    $('#sanjobsearchtitle').text(text);
    localStorage.setItem("sanjobstatustext", text);
    $("#sanjobsearchListul li").removeClass("activeState");
    $("#sanjobsearchListul li").eq(4).addClass('activeState');
    localStorage.setItem("SANITATIONJOBSTATUS", daterangeval);
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');
    CustomQueryDisplayId = daterangeval;
    localStorage.setItem("SANITATIONJOBSTATUS", CustomQueryDisplayId);
    if (daterangeval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtTable.page('first').draw('page');
    }
});

$(document).on('click', '#btnpassedtimeperiod', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbpassedview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != '34') {
        PassedStartDateVw = '';
        PassedEndDateVw = '';
        localStorage.removeItem('SJPassedStartDateVw');
        localStorage.removeItem('SJPassedEndDateVw');
    }
    else {
        localStorage.setItem('SJPassedStartDateVw', PassedStartDateVw);
        localStorage.setItem('SJPassedEndDateVw', PassedEndDateVw);
    }
    $(document).find('#SJPassedDateRangeModal').modal('hide');
    var text = $('#sanjobsearchListul').find('li').eq(6).text();

    //-------------------------------------------------------
    if (daterangeval != '34')
        text = text + " - " + $(document).find('#cmbpassedview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#passedaterange').val();
    //-------------------------------------------------------

    $('#sanjobsearchtitle').text(text);
    localStorage.setItem("sanjobstatustext", text);
    $("#sanjobsearchListul li").removeClass("activeState");
    $("#sanjobsearchListul li").eq(6).addClass('activeState');
    localStorage.setItem("SANITATIONJOBSTATUS", daterangeval);
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');
    CustomQueryDisplayId = daterangeval;
    localStorage.setItem("SANITATIONJOBSTATUS", CustomQueryDisplayId);
    if (daterangeval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtTable.page('first').draw('page');
    }
});

$(document).on('change', '#cmbcreateview', function (e) {
    var thielement = $(this);
    CustomQueryDisplayId = thielement.val();
    if (thielement.val() == '16') {
        var strtlocal = localStorage.getItem('SJCreateStartDateVw');
        if (strtlocal) {
            CreateStartDateVw = strtlocal;
        }
        else {
            CreateStartDateVw = today;
        }
        var endlocal = localStorage.getItem('SJCreateEndDateVw');
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
        localStorage.removeItem('SJCreateStartDateVw');
        localStorage.removeItem('SJCreateEndDateVw');
        $(document).find('#timeperiodcontainerForCreateDate').hide();
    }
});

$(document).on('change', '#cmbcompleteview', function (e) {
    var thielement = $(this);
    sjStatus = thielement.val();

    if (thielement.val() == '22') {
        CompleteStartDateVw = today;
        CompleteEndDateVw = today;
        var strtlocal = localStorage.getItem('SJCompleteStartDateVw');
        if (strtlocal) {
            CompleteStartDateVw = strtlocal;
        }
        else {
            CompleteStartDateVw = today;
        }
        var endlocal = localStorage.getItem('SJCompleteEndDateVw');
        if (endlocal) {
            CompleteEndDateVw = endlocal;
        }
        else {
            CompleteEndDateVw = today;
        }
        $(document).find('#timeperiodcontainer').show();
        $(document).find('#completedaterange').daterangepicker({
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
            console.log(CompleteStartDateVw);
        });
    }
    else {
        $(document).find('#timeperiodcontainer').hide();
    }
});

$(document).on('change', '#cmbfailedview', function (e) {
    var thielement = $(this);
    sjStatus = thielement.val();

    if (thielement.val() == '28') {
        FailedStartDateVw = today;
        FailedEndDateVw = today;
        var strtlocal = localStorage.getItem('SJFailedStartDateVw');
        if (strtlocal) {
            FailedStartDateVw = strtlocal;
        }
        else {
            FailedStartDateVw = today;
        }
        var endlocal = localStorage.getItem('SJFailedEndDateVw');
        if (endlocal) {
            FailedEndDateVw = endlocal;
        }
        else {
            FailedEndDateVw = today;
        }
        $(document).find('#failedtimeperiodcontainer').show();
        $(document).find('#faileddaterange').daterangepicker({
            format: 'MM/DD/YYYY',
            startDate: FailedStartDateVw,
            endDate: FailedEndDateVw,
            "locale": {
                "applyLabel": getResourceValue("JsApply"),
                "cancelLabel": getResourceValue("CancelAlert")
            }
        }, function (start, end, label) {
            FailedStartDateVw = start.format('MM/DD/YYYY');
            FailedEndDateVw = end.format('MM/DD/YYYY');
        });
    }
    else {
        $(document).find('#failedtimeperiodcontainer').hide();
    }
});

$(document).on('change', '#cmbpassedview', function (e) {
    var thielement = $(this);
    sjStatus = thielement.val();

    if (thielement.val() == '34') {
        PassedStartDateVw = today;
        PassedEndDateVw = today;
        var strtlocal = localStorage.getItem('SJPassedStartDateVw');
        if (strtlocal) {
            PassedStartDateVw = strtlocal;
        }
        else {
            PassedStartDateVw = today;
        }
        var endlocal = localStorage.getItem('SJPassedEndDateVw');
        if (endlocal) {
            PassedEndDateVw = endlocal;
        }
        else {
            PassedEndDateVw = today;
        }
        $(document).find('#passedtimeperiodcontainer').show();
        $(document).find('#passedaterange').daterangepicker({
            format: 'MM/DD/YYYY',
            startDate: PassedStartDateVw,
            endDate: PassedEndDateVw,
            "locale": {
                "applyLabel": getResourceValue("JsApply"),
                "cancelLabel": getResourceValue("CancelAlert")
            }
        }, function (start, end, label) {
            PassedStartDateVw = start.format('MM/DD/YYYY');
            PassedEndDateVw = end.format('MM/DD/YYYY');
        });
    }
    else {
        $(document).find('#passedtimeperiodcontainer').hide();
    }
});
//#endregion

//#region Sanitation AssetTree
function generateSanitationAssetTree(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/SanitationEquipmentHierarchyTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        cache: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find(".cntTree").html(data);
        }
        ,
        complete: function () {
            CloseLoader();
            treeTable($(document).find('#tblTree'));

            $(document).find('.radSelectSanitation').each(function () {
                if ($(document).find('#hdnId').val() == '0' || $(document).find('#hdnId').val() == '') {

                    if ($(this).data('equipmentid') === equipmentid) {
                        $(this).attr('checked', true);
                    }

                }
                else {

                    if ($(this).data('equipmentid') == $(document).find('#hdnId').val()) {
                        $(this).attr('checked', true);
                    }

                }

            });
            $('#sanitationTreeModal').modal('show');
            //---------------------------------------------------
            // looking for the collapse icon and triggered click to collapse
            $.each($(document).find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,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"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element
            CloseLoader();
        },
        error: function (xhr) {
            alert('error');
        }
    });
}


$(document).on('change', ".radSelectSanitation", function () {
    $(document).find('#hdnId').val('0');
    equipmentid = $(this).data('equipmentid');
    var clientlookupid = $(this).data('clientlookupid');
    var chargetoname = $(this).data('itemname');
    chargetoname = clientlookupid + '(' + (chargetoname.substring(0, chargetoname.length - 1)).trim()+')';
    $(document).find("#TchargeType").val("Equipment");
    $(document).find("#TplantLocationId").val(equipmentid);
   
    $(document).find('#DemandModel_PlantLocationDescription').val(chargetoname).removeClass('input-validation-error').trigger('change');
    $(document).find('#TplantLocationDescription').val(clientlookupid);
    $(document).find('#JobDetailsModel_PlantLocationDescription').val(chargetoname);
    $(document).find('#ODescribeModel_PlantLocationDescription').val(chargetoname).removeClass('input-validation-error').trigger('change');
    if ($(document).find('#JobDetailsModel_PlantLocationDescription').length > 0) {
        var ChargeTo_Name = ($(this).data('itemname').substring(0, $(this).data('itemname').length - 1)).trim();
        $(document).find('#JobDetailsModel_ChargeTo_Name').val(ChargeTo_Name);
    }
    $('#sanitationTreeModal').modal('hide');

});
//#endregion

//#region Show Images  V2-841
var cardviewstartvalue = 0;
var cardviewlwngth = 10;
var grdcardcurrentpage = 1;
var currentorderedcolumn = 1;
var layoutTypeWO = 1;
function LoadImages(_SanitationJobId) {
    $.ajax({
        url: '/SanitationJob/GetImages',
        type: 'POST',
        data: {
            currentpage: grdcardcurrentpage,
            start: cardviewstartvalue,
            length: cardviewlwngth,
            SanitationJobId: _SanitationJobId
        },
        beforeSend: function () {
            $(document).find('#imagedataloader').show();
        },
        success: function (data) {
            //console.log(data.imageAttachmentModels);
            //if (data.TotalCount > 0) {
            $(document).find('#ImageGrid').show();
            $(document).find('#SanitationImages').html(data).show();
            $(document).find('#tblimages_paginate li').each(function (index, value) {
                $(this).removeClass('active');
                if ($(this).data('currentpage') == grdcardcurrentpage) {
                    $(this).addClass('active');
                }
            });
            //}
            //else {
            //    $(document).find('#ImageGrid').hide();
            //}
        },
        complete: function () {
            $(document).find('#imagedataloader').hide();
            $(document).find('#cardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(cardviewlwngth).trigger('change.select2');
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('change', '#cardviewpagelengthdrp', function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val()
    cardviewlwngth = $(this).val();
    grdcardcurrentpage = parseInt(cardviewstartvalue / cardviewlwngth) + 1;
    cardviewstartvalue = parseInt((grdcardcurrentpage - 1) * cardviewlwngth) + 1;
    //GetAndSaveState();
    LoadImages(SanitationJobId);

});
$(document).on('click', '#tblimages_paginate .paginate_button', function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    if (layoutTypeWO == 1) {
        var currentselectedpage = parseInt($(document).find('#tblimages_paginate .pagination').find('.active').text());
        cardviewlwngth = $(document).find('#cardviewpagelengthdrp').val();
        cardviewstartvalue = cardviewlwngth * (parseInt($(this).find('.page-link').text()) - 1);
        var lastpage = parseInt($(this).prev('li').data('currentpage'));

        if ($(this).attr('id') == 'tbl_previous') {
            if (currentselectedpage == 1) {
                return false;
            }
            cardviewstartvalue = cardviewlwngth * (currentselectedpage - 2);
            grdcardcurrentpage = grdcardcurrentpage - 1;
        }
        else if ($(this).attr('id') == 'tbl_next') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            cardviewstartvalue = cardviewlwngth * (currentselectedpage);
            grdcardcurrentpage = grdcardcurrentpage + 1;
        }
        else if ($(this).attr('id') == 'tbl_first') {
            if (currentselectedpage == 1) {
                return false;
            }
            grdcardcurrentpage = 1;
            cardviewstartvalue = 0;
        }
        else if ($(this).attr('id') == 'tbl_last') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            grdcardcurrentpage = parseInt($(this).prevAll('li').eq(1).text());
            cardviewstartvalue = cardviewlwngth * (grdcardcurrentpage - 1);
        }
        else {
            grdcardcurrentpage = $(this).data('currentpage');
        }
        //GetAndSaveState();
        LoadImages(SanitationJobId);

    }
    else {
        run = true;
    }
});
//#endregion

//#region Multiple Photo Upload
function CompressImage(files, imageName) {
    new Compressor(files, {
        quality: 0.6,
        convertTypes: ['image/png'],
        convertSize: 100000,
        // The compression process is asynchronous,
        // which means you have to access the `result` in the `success` hook function.
        success(result) {
            if (result.size < files.size) {
                SaveCompressedImage(result, imageName);
            }
            else {
                SaveCompressedImage(files, imageName);
            }
            console.log('file name ' + result.name + ' after compress ' + result.size);
        },
        error(err) {
            console.log(err.message);
        },
    });
}
function SaveCompressedImage(data, imageName) {
    var AddPhotoFileData = new FormData();
    AddPhotoFileData.append('file', data, imageName);

    $.ajax({
        url: '../base/SaveUploadedFile',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: AddPhotoFileData,
        success: function (result) {
            //alert(result);
            SaveMultipleUploadedFileToServer(imageName);
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}
$(document).on('change', '#files', function () {
    var val = $(this).val();
    var _isMobile = CheckLoggedInFromMob();
    var imageName = val.replace(/^.*[\\\/]/, '')
    //console.log(imageName);
    var fileUpload = $("#files").get(0);
    var files = fileUpload.files;
    var fileExt = imageName.substr(imageName.lastIndexOf('.') + 1).toLowerCase();
    if (fileExt != 'jpeg' && fileExt != 'jpg' && fileExt != 'png' && fileExt != 'JPEG' && fileExt != 'JPG' && fileExt != 'PNG') {
        ShowErrorAlert(getResourceValue("spnValidImage"));
        $('#files').val('');
        //e.preventDefault();
        return false;
    }
    else if (this.files[0].size > (1024 * 1024 * 10)) {
        ShowImageSizeExceedAlert();
        $('#files').val('');
        //e.preventDefault();
        return false;
    }
    else {
        //alert('Hello');
        swal(AddImageAlertSetting, function () {
            if (window.FormData !== undefined) {
                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    console.log('file name ' + files[i].name + ' before compress ' + files[i].size);
                    if (_isMobile == true) {
                        var sanitationJobId=$(document).find('#JobDetailsModel_SanitationJobId').val();
                        var imgname = sanitationJobId + "_" + Math.floor((new Date()).getTime() / 1000);
                        CompressImage(files[i], imgname + "." + fileExt);
                    }
                    else {
                        CompressImage(files[i], imageName);
                    }
                    
                }
            } else {
                //alert("FormData is not supported.");
            }
            $('#files').val('');

        });
    }
});
//#endregion

//#region Save Multiple Image
function SaveMultipleUploadedFileToServer(imageName) {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    $.ajax({
        url: '../base/SaveMultipleUploadedFileToServer',
        type: 'POST',

        data: { 'fileName': imageName, objectId: SanitationJobId, TableName: "Sanitation", AttachObjectName: "Sanitation" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {

                CloseLoader();
                ShowImageSaveSuccessAlert();
            }
            else if (data.result == "1") {
                CloseLoader();
                var errorMessage = getResourceValue("ImageExistAlert");
                ShowErrorAlert(errorMessage);

            }
            else {
                CloseLoader();
                var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
                ShowErrorAlert(errorMessage);

            }
        },
        complete: function () {
            LoadImages(SanitationJobId);
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion




//#region Set As Default
$(document).on('click', '#selectidSetAsDefault', function () {
    var AttachmentId = $(this).attr('dataid');
    $('#' + AttachmentId).hide();
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    $('.modal-backdrop').remove();
    $.ajax({
        url: '../base/SetImageAsDefault',
        type: 'POST',

        data: { AttachmentId: AttachmentId, objectId: SanitationJobId, TableName: "Sanitation" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result === "success") {
                //$('.modal-backdrop').remove();
                //$('#' + AttachmentId).hide();
                $('#EquipZoom').attr('src', data.imageurl);
                $('#EquipZoom').attr('data-zoom-image', data.imageurl);
                $('.equipImg').attr('src', data.imageurl);
                //$(document).find('#AzureImage').append('<a href="javascript:void(0)" id="deleteImg" class="trashIcon" title="Delete"><i class="fa fa-trash"></i></a>');
                $('#EquipZoom').data('zoomImage', data.imageurl).elevateZoom(
                    {
                        zoomType: "window",
                        lensShape: "round",
                        lensSize: 1000,
                        zoomWindowFadeIn: 500,
                        zoomWindowFadeOut: 500,
                        lensFadeIn: 100,
                        lensFadeOut: 100,
                        easing: true,
                        scrollZoom: true,
                        zoomWindowWidth: 450,
                        zoomWindowHeight: 450
                    });
                $("#EquipZoom").on('load', function () {
                    //LoadImages(EquimentId);
                    CloseLoader();
                    ShowImageSetSuccessAlert();
                });
                //RedirectToEquipmentDetail(EquimentId, "OnPremiseImageReload");
                //ShowImageSaveSuccessAlert();
            }
            //else {
            //    CloseLoader();
            //    //var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
            //    //ShowErrorAlert(errorMessage);

            //}
        },
        complete: function () {
            //CloseLoader();
            LoadImages(SanitationJobId);
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion

//#region Delete Image
$(document).on('click', '#selectidDelete', function () {
    var AttachmentId = $(this).attr('dataid');
    $('#' + AttachmentId).hide();
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientOnPremise = $('#JobDetailsModel_ClientOnPremise').val();
    $('.modal-backdrop').remove();
    if (ClientOnPremise == 'True') {
        DeleteOnPremiseMultipleImage(SanitationJobId, AttachmentId);
    }
    else {
        DeleteAzureMultipleImage(SanitationJobId, AttachmentId);
    }
});

function DeleteOnPremiseMultipleImage(SanitationJobId, AttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '../base/DeleteMultipleImageFromOnPremise',
            type: 'POST',
            data: { AttachmentId: AttachmentId, objectId: SanitationJobId, TableName: "SanitationJob" },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToSJDetail(SanitationJobId, "OnPremiseImageReload");
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function DeleteAzureMultipleImage(SanitationJobId, AttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '../base/DeleteMultipleImageFromAzure',
            type: 'POST',
            data: { AttachmentId: AttachmentId, objectId: SanitationJobId, TableName: "SanitationJob" },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToSJDetail(SanitationJobId, "AzureImageReload");
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
//#endregion

//#region V2-1080 Reset Grid
$('#liResetGridClearBtn').click(function (e) {
    CancelAlertSetting.text = getResourceValue("ResetGridAlertMessage");
    swal(CancelAlertSetting, function () {
        var localstorageKeys = [];
        localstorageKeys.push("sanjobstatustext");
        localstorageKeys.push("SJCreateStartDateVw");
        localstorageKeys.push("SJCreateEndDateVw");
        localstorageKeys.push("SJCompleteStartDateVw");
        localstorageKeys.push("SJCompleteEndDateVw");
        localstorageKeys.push("SJFailedStartDateVw");
        localstorageKeys.push("SJFailedEndDateVw");
        localstorageKeys.push("SJPassedStartDateVw");
        localstorageKeys.push("SJPassedEndDateVw");
        localstorageKeys.push("SANITATIONJOBSTATUS");
        DeleteGridLayout('SanitationJob_Search', dtTable, localstorageKeys);
        GenerateSearchList('', true);
        window.location.href = "../SanitationJob/Index?page=Sanitation_Jobs_Search";
    });
});
//#endregion
