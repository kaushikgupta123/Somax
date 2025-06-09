//#region Common
var run = false;
var StartCreateteDate = '';
var EndCreateteDate = '';
var totalcount = 0;
var StartProcessedByDate = '';
var EndProcessedByDate = '';
var today = $.datepicker.formatDate('mm/dd/yy', new Date());
var CustomQueryDisplayId = 10;
var ProcessedStartDateVw = '';
var ProcessedEndDateVw = '';
var CreateStartDateVw = '';
var CreateEndDateVw = '';
var CancelandDeniedStartDateVw = '';
var CancelandDeniedEndDateVw = '';
var prtempsearchtitle = '';
var prtempCustomdisplayId = '';

var selectCount = 0;
var gridname = "PurchaseRequest_Search";
var IsUsePartMaster = false;
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
var ConvertedToOrder = '14',
    ConvertedToOrderToday = '24',
    ConvertedToOrder7Days = '25',
    ConvertedToOrder30Days = '26',
    ConvertedToOrder60Days = '27',
    ConvertedToOrder90Days = '28',
    ConvertedToOrderDateRange = '29';
var CanceledOrDenied = '17',
    CanceledOrDeniedToday = '30',
    CanceledOrDenied7Days = '31',
    CanceledOrDenied30Days = '32',
    CanceledOrDenied60Days = '33',
    CanceledOrDenied90Days = '34',
    CanceledOrDeniedDateRange = '35';
var AllStatusValues = '18',
    AllStatusValues7Days = '19',
    AllStatusValues30Days = '20',
    AllStatusValues60Days = '21',
    AllStatusValues90Days = '22',
    AllStatusValuesDateRange = '23';
//#endregion

$(document).ready(function () {
    $('#Createtimeperiodcontainer').hide();
    $('#processedbytimeperiodcontainer').hide();

    if ($(document).find('.AddPrequest').length === 0) { $(document).find('#prsearchactiondiv').css('margin-right', '0px'); }
    if ($(document).find("#detmaintab").length === 0) {
        ShowbtnLoaderclass("LoaderDrop");
        ShowbtnLoader("btnsortmenu");
    }
    $(document).on('click', "#action", function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $(document).on('focusout', "#action", function () {
        $(".actionDrop").fadeOut();
    });
    $(".tabsArea").hide();
    $("ul.vtabs li:first").addClass("active").show();
    $(".tabsArea:first").show();
    $(".actionBar").fadeIn();
    $("#PurchaseRequestGridAction :input").attr("disabled", "disabled");

    $(document).on('click', "ul.vtabs li", function () {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        $(".tabsArea").not('#PurchaseRequestLineItem').hide();
        var activeTab = $(this).find("a").attr("href");
        $(activeTab).fadeIn();
        return false;
    });

    $('#tabselector').change(function () {
        $('.tabcontent').hide();
        $('#' + $(this).val()).show();
    });
    $('#tabselector2').change(function () {
        $('.tabcontent').hide();
        $('#' + $(this).val()).show();
    });
    $('[data-toggle="tooltip"]').tooltip();
    $(document).on('click', '#proverviewt', function () {
        $('#PurchasingOverview').show();
        $('#Detailstab').show();
        $('#btndetails').addClass('active');
    });
    $(document).on('change', '#colorselector', function (evt) {
        $(".tabsArea").not('#PurchaseRequestLineItem').hide();
        if ($(this).val() === 'PurchasingOverview') {
            opendiv(evt, 'Detailstab');
        }
        else {
            openCity(evt, $(this).val());
        }
        $('#' + $(this).val()).show();
    });
    function opendiv(evt, cityName) {
        $("#PurchasingOverview").find("#btndetails").addClass('active');
        document.getElementById(cityName).style.display = "block";
    }
    var title = localStorage.getItem("prstatustext");
    if (title) {
        $(document).find('#spnlinkToSearch').text(title);
    }
    //V2-653 
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        var validity = false;
        if ($(this).closest('form').length > 0) {
            validity = $(this).valid();
        }
        if (validity == true) {
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
});
function openCity(evt, cityName) {
    switch (cityName) {
        case "PRNotes":
            GeneratePNotesGrid();
            break;
        case "PRAttachments":
            GeneratePAttachmentsGrid();
            break;
        case "PurchasingOverview":
            $('#PurchasingOverview').show();
            $('#Detailstab').show();
            $('#btndetails').addClass('active');
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
$(document).ready(function () {
    $(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '.dismiss, .overlay', function () {
        $(document).find('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', '#pinvsidebarCollapse,#sidebarCollapse', function () {
        $('.sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
        $(document).find('.dtpicker').datepicker({
            changeMonth: true,
            changeYear: true,
            "dateFormat": "mm/dd/yy",
            autoclose: true
        });
    });

    var IsPunchOutCheckOut = $(document).find('#IsPunchOutCheckOut').val();
    if (IsPunchOutCheckOut == "True") {
        //#region RKL Mail for close the vendor punchout website and send user back to the original tab
        if ($(document).find('#IsPunchOutCheckOutTab').val() == "False") {
            // Redirect the original tab (SOMAX)
            if (window.opener) {
                window.opener.location.href = '../PurchaseRequest/RedirectForShoppingCart';
            }
            // Close the current tab (vendor's punchout website)
            window.close();
        }
        //#endregion
        else {
            generatePRShoppingCartDataTable();
        }
    }
    else {

        var strProcessedStartDateVw = localStorage.getItem('ProcessedStartDateVw');
        if (strProcessedStartDateVw) {
            ProcessedStartDateVw = strProcessedStartDateVw;
        }
        var endProcessedEndDateVw = localStorage.getItem('ProcessedEndDateVw');
        if (endProcessedEndDateVw) {
            ProcessedEndDateVw = endProcessedEndDateVw;
        }

        var strCreateStartDateVw = localStorage.getItem('CreateStartDateVw');
        if (strCreateStartDateVw) {
            CreateStartDateVw = strCreateStartDateVw;
        }
        var endCreateEndDateVw = localStorage.getItem('CreateEndDateVw');
        if (endCreateEndDateVw) {
            CreateEndDateVw = endCreateEndDateVw;
        }


        var strCancelandDeniedStartDateVw = localStorage.getItem('CancelandDeniedStartDateVw');
        if (strCancelandDeniedStartDateVw) {
            CancelandDeniedStartDateVw = strCancelandDeniedStartDateVw;
        }
        var endCancelandDeniedEndDateVw = localStorage.getItem('CancelandDeniedEndDateVw');
        if (endCancelandDeniedEndDateVw) {
            CancelandDeniedEndDateVw = endCancelandDeniedEndDateVw;
        }

        var purchaserequeststatus = localStorage.getItem("PURCHASEREQUESTSTATUS");
        if (purchaserequeststatus) {
            var text = "";
            $('#purchaseRequestModel_ScheduleWorkList').val(purchaserequeststatus).trigger('change.select2');
            CustomQueryDisplayId = purchaserequeststatus;

            //V2-523
            if (purchaserequeststatus === ConvertedToOrderToday || purchaserequeststatus === ConvertedToOrder7Days || purchaserequeststatus === ConvertedToOrder30Days ||
                purchaserequeststatus === ConvertedToOrder60Days || purchaserequeststatus === ConvertedToOrder90Days || purchaserequeststatus === ConvertedToOrderDateRange) {
                $('#cmbPROrderview').val(purchaserequeststatus).trigger('change');
                $("#prsearchListul li").removeClass("activeState");
                $("#prsearchListul li[id='14']").addClass("activeState");
                text = $("#prsearchListul li[id='14']").text();
                $('#prsearchtitle').text(text);
            }
            else if (purchaserequeststatus === CanceledOrDeniedToday || purchaserequeststatus === CanceledOrDenied7Days || purchaserequeststatus === CanceledOrDenied30Days ||
                purchaserequeststatus === CanceledOrDenied60Days || purchaserequeststatus === CanceledOrDenied90Days || purchaserequeststatus === CanceledOrDeniedDateRange) {
                $('#cmbPRCancelDenyview').val(purchaserequeststatus).trigger('change');
                $("#prsearchListul li").removeClass("activeState");
                $("#prsearchListul li[id='17']").addClass("activeState");
                text = $("#prsearchListul li[id='17']").text();
                $('#prsearchtitle').text(text);
            }
            //V2-523
            else if (purchaserequeststatus === AllStatusValues || purchaserequeststatus === AllStatusValues7Days || purchaserequeststatus === AllStatusValues30Days ||
                purchaserequeststatus === AllStatusValues60Days || purchaserequeststatus === AllStatusValues90Days || purchaserequeststatus === AllStatusValuesDateRange) {
                $('#cmbPRview').val(purchaserequeststatus).trigger('change');
                $("#prsearchListul li").removeClass("activeState");
                $("#prsearchListul li[id='18']").addClass("activeState");
                text = $("#prsearchListul li[id='18']").text();
                $('#prsearchtitle').text(text);
            }
            $('#prsearchListul li').each(function (index, value) {
                if ($(this).attr('id') == CustomQueryDisplayId && $(this).attr('id') != '0') {
                    text = $(this).text();
                    $('#prsearchtitle').text($(this).text());
                    $(".searchList li").removeClass("activeState");
                    $(this).addClass('activeState');
                }
            });
            //375

            //V2-523
            if (purchaserequeststatus === ConvertedToOrderToday || purchaserequeststatus === ConvertedToOrder7Days || purchaserequeststatus === ConvertedToOrder30Days ||
                purchaserequeststatus === ConvertedToOrder60Days || purchaserequeststatus === ConvertedToOrder90Days || purchaserequeststatus === ConvertedToOrderDateRange) {

                if (purchaserequeststatus === ConvertedToOrderDateRange)
                    text = text + " - " + $('#PROrderdaterange').val();
                else
                    text = text + " - " + $(document).find('#cmbPROrderview option[value=' + purchaserequeststatus + ']').text();
                $('#prsearchtitle').text(text);
            }
            else if (purchaserequeststatus === CanceledOrDeniedToday || purchaserequeststatus === CanceledOrDenied7Days || purchaserequeststatus === CanceledOrDenied30Days ||
                purchaserequeststatus === CanceledOrDenied60Days || purchaserequeststatus === CanceledOrDenied90Days || purchaserequeststatus === CanceledOrDeniedDateRange) {
                if (purchaserequeststatus === CanceledOrDeniedDateRange)
                    text = text + " - " + $('#PRCancelDenydaterange').val();
                else
                    text = text + " - " + $(document).find('#cmbPRCancelDenyview option[value=' + purchaserequeststatus + ']').text();
                $('#prsearchtitle').text(text);
            }
            //V2-523
            else if (purchaserequeststatus === AllStatusValues || purchaserequeststatus === AllStatusValues7Days || purchaserequeststatus === AllStatusValues30Days ||
                purchaserequeststatus === AllStatusValues60Days || purchaserequeststatus === AllStatusValues90Days || purchaserequeststatus === AllStatusValuesDateRange) {
                if (purchaserequeststatus === AllStatusValuesDateRange)
                    text = text + " - " + $('#PRdaterange').val();
                else
                    text = text + " - " + $(document).find('#cmbPRview option[value=' + purchaserequeststatus + ']').text();
                $('#prsearchtitle').text(text);
            }
        }
        else {
            purchaserequeststatus = 10;
            CustomQueryDisplayId = purchaserequeststatus;
            $('#prsearchtitle').text(getResourceValue("MyOpenRequestsAlert"));
            $("#prsearchListul li").first().addClass("activeState");
        }
        var IsRedirectFromPart = $("#IsRedirectFromPart").val();
        var PrId = $("#PRId").val();
        var PRStatus = $("#PRStatus").val();
        if (IsRedirectFromPart == "True") {

            if (PRStatus == "Approved") {
                localStorage.setItem("PURCHASEREQUESTSTATUS", "15");
                localStorage.setItem("prstatustext", getResourceValue("ApprovedAlert"));
            }
            else if (PRStatus == "AwaitApproval") {
                localStorage.setItem("PURCHASEREQUESTSTATUS", "12");
                localStorage.setItem("prstatustext", getResourceValue("AwaitingApprovalAlert"));
            }
            else if (PRStatus == "Extracted") {
                localStorage.setItem("PURCHASEREQUESTSTATUS", "16");
                localStorage.setItem("prstatustext", getResourceValue("ExtractedAlert"));
            }
            else {
                localStorage.setItem("PURCHASEREQUESTSTATUS", "10");
                localStorage.setItem("prstatustext", getResourceValue("MyOpenRequestAlert"));
            }
            RedirectToPRequestDetail(PrId, "PurchasingOverview");

        }
        //V2-1047
        var IsRedirectFromDetailPurchaseOrder = $("#IsRedirectFromDetailPurchaseOrder").val();
        if (IsRedirectFromDetailPurchaseOrder == "True") {
            localStorage.setItem("PURCHASEREQUESTSTATUS", "10");
            localStorage.setItem("prstatustext", getResourceValue("MyOpenRequestAlert"));
            RedirectToPRequestDetail(PrId, "PurchasingOverview");

        }
        var IsRedirectFromNotification = $("#IsRedirectFromNotification").val();
        //v2-548
        var IsRedirectFromPlusMenu = $("#IsRedirectFromPlusMenu").val();
        if (IsRedirectFromPlusMenu == "True") {
            localStorage.setItem("PURCHASEREQUESTSTATUS", "10");
            localStorage.setItem("prstatustext", getResourceValue("MyOpenRequestAlert"));
            RedirectToPRequestDetail(PrId, "PurchasingOverview");

        }
        //V2-1147
        else if (IsRedirectFromNotification == "True") {
            var AlertNameRedirectFromNotification = $("#AlertNameRedirectFromNotification").val();
            if (AlertNameRedirectFromNotification == "PurchaseRequestApprovalNeeded" || AlertNameRedirectFromNotification == "PurchaseRequestApproved" || AlertNameRedirectFromNotification == "PurchaseRequestConverted" || AlertNameRedirectFromNotification == "PurchaseRequestDenied" || AlertNameRedirectFromNotification == "PurchaseRequestReturned") {
                localStorage.setItem("PURCHASEREQUESTSTATUS", "22");
                localStorage.setItem("prstatustext", getResourceValue("spnAllStatusesLast90Days"));
                RedirectToPRequestDetail(PrId, "PurchasingOverview");
            }
        }
        else {
            generatePRDataTable();
        }

    }



});
//#region Search
var dtPrTable;
var dtPrShoppingCartTable;
var setFirstPage;
var SelectPRDetails = [];
var SelectPRId = [];
$("#purchaseRequestModel_ScheduleWorkList").change(function () {
    ShowbtnLoaderclass("LoaderDrop");
    run = true;
    var optionval = $('#purchaseRequestModel_ScheduleWorkList').val();
    localStorage.setItem("PURCHASEREQUESTSTATUS", optionval);
    if (optionval.length !== 0) {
        CustomQueryDisplayId = optionval;
    }
    else {
        CustomQueryDisplayId = 0;
    }
    dtPrTable.page('first').draw('page');
    typeValStatus = $("#Status").val();
    typeValVendor = $("#Vendor").val();
});
$("#btnPRDataAdvSrch").on('click', function (e) {
    run = true;
    searchresult = [];
    PRAdvSearch();
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    dtPrTable.page('first').draw('page');
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    if (searchtxtId == "Status") {
        $(document).find("#CreateBy").val("").trigger('change.select2');
    }
    if (searchtxtId == "Vendor") {
        $(document).find("#Vendor").val("").trigger('change.select2');
    }
    PRAdvSearch(dtPrTable.length);
    dtPrTable.page('first').draw('page');
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', '#liPdf,#liPrint', function () {
    var thisid = $(this).attr('id');
    var TableHaederProp = [];
    function table(property, title) {
        this.property = property;
        this.title = title;
    }
    $("#purchaserequestSearch thead tr th").map(function (key) {
        var thisdiv = $(this).find('div');
        if ($(this).parents('.purchaseRequestinnerDataTable').length == 0 && thisdiv.html()) {
            if (this.getAttribute('data-th-prop')) {
                var tablearr = new table(this.getAttribute('data-th-prop'), thisdiv.html());
                TableHaederProp.push(tablearr);
            }
        }
    });
    var params = {
        tableHaederProps: TableHaederProp,
        colname: order,
        coldir: orderDir,
        CustomQueryDisplayId: CustomQueryDisplayId,
        ProcessedStartDateVw: ValidateDate(ProcessedStartDateVw),
        ProcessedEndDateVw: ValidateDate(ProcessedEndDateVw),
        CreateStartDateVw: ValidateDate(CreateStartDateVw),
        CreateEndDateVw: ValidateDate(CreateEndDateVw),
        CancelandDeniedStartDateVw: ValidateDate(CancelandDeniedStartDateVw),
        CancelandDeniedEndDateVw: ValidateDate(CancelandDeniedEndDateVw),
        PurchaseRequest: LRTrim($("#PurchaseRequest").val()),
        Reason: LRTrim($("#Reason").val()),
        Status: LRTrim($("#Status").val()),
        CreatedBy: LRTrim($("#CreatedBy").val()),
        Vendor: LRTrim($('#Vendor').val()),
        VendorName: LRTrim($("#VendorName").val()),
        CreateStartDate: ValidateDate(StartCreateteDate),
        CreateEndDate: ValidateDate(EndCreateteDate),
        PONumber: LRTrim($("#PONumber").val()),
        ProcessedBy: LRTrim($("#ProcessedBy").val()),
        ProcessedStartDate: ValidateDate(StartProcessedByDate),
        ProcessedEndDate: ValidateDate(EndProcessedByDate),
        txtsearchval: LRTrim($("#txtColumnSearch").val()),
        Creator_PersonnelName: "",
        Processed_PersonnelName: ""
    };
    pRPrintParams = JSON.stringify({ 'pRPrintParams': params });
    $.ajax({
        "url": "/PurchaseRequest/SetPrintData",
        "data": pRPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            if (thisid == 'liPdf') {
                window.open('/PurchaseRequest/ExportASPDF?d=d', '_self');
            }
            else if (thisid == 'liPrint') {
                window.open('/PurchaseRequest/ExportASPDF', '_blank');
            }

            return;
        }
    });
    $('#mask').trigger('click');
});
$(document).on('change', '#Created', function () {
    var thisval = $(this).val();
    if (thisval == 10) {
        $('#Createtimeperiodcontainer').show();
    }
    else {
        $('#Createtimeperiodcontainer').hide();
    }
    switch (thisval) {
        case '2':
            $('#Createtimeperiodcontainer').hide();
            StartCreateteDate = today;
            EndCreateteDate = today;
            break;
        case '3':
            $('#Createtimeperiodcontainer').hide();
            StartCreateteDate = PreviousDateByDay(7);
            EndCreateteDate = today;
            break;
        case '4':
            $('#Createtimeperiodcontainer').hide();
            StartCreateteDate = PreviousDateByDay(30);
            EndCreateteDate = today;
            break;
        case '5':
            $('#Createtimeperiodcontainer').hide();
            StartCreateteDate = PreviousDateByDay(60);
            EndCreateteDate = today;
            break;
        case '6':
            $('#Createtimeperiodcontainer').hide();
            StartCreateteDate = PreviousDateByDay(90);
            EndCreateteDate = today;
            break;
        case '10':
            $('#Createtimeperiodcontainer').show();
            StartCreateteDate = today;
            EndCreateteDate = today;
            $('#advcreatedaterange').val(StartCreateteDate + ' - ' + EndCreateteDate);
            $(document).find('#advcreatedaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                StartCreateteDate = start.format('MM/DD/YYYY');
                EndCreateteDate = end.format('MM/DD/YYYY');
            });
            break;
        default:
            $('#Createtimeperiodcontainer').hide();
            $(document).find('#advcreatedaterange').daterangepicker({
                format: 'MM/DD/YYYY'
            });
            StartCreateteDate = '';
            EndCreateteDate = '';
            break;
    }
});
$(document).on('change', '#DateProcessed', function () {
    var thisval = $(this).val();
    if (thisval == 10) {
        $('#processedbytimeperiodcontainer').show();
    }
    else {
        $('#processedbytimeperiodcontainer').hide();
    }
    switch (thisval) {
        case '2':
            $('#processedbytimeperiodcontainer').hide();
            StartProcessedByDate = today;
            EndProcessedByDate = today;
            break;
        case '3':
            $('#processedbytimeperiodcontainer').hide();
            StartProcessedByDate = PreviousDateByDay(7);
            EndProcessedByDate = today;
            break;
        case '4':
            $('#processedbytimeperiodcontainer').hide();
            StartProcessedByDate = PreviousDateByDay(30);
            EndProcessedByDate = today;
            break;
        case '5':
            $('#processedbytimeperiodcontainer').hide();
            StartProcessedByDate = PreviousDateByDay(60);
            EndProcessedByDate = today;
            break;
        case '6':
            $('#processedbytimeperiodcontainer').hide();
            StartProcessedByDate = PreviousDateByDay(90);
            EndProcessedByDate = today;
            break;
        case '10':
            $('#processedbytimeperiodcontainer').show();
            StartProcessedByDate = today;
            EndProcessedByDate = today;
            $('#advprocessdedbydaterange').val(StartProcessedByDate + ' - ' + EndProcessedByDate);
            $(document).find('#advprocessdedbydaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                StartProcessedByDate = start.format('MM/DD/YYYY');
                EndProcessedByDate = end.format('MM/DD/YYYY');
            });
            break;
        default:
            $('#processedbytimeperiodcontainer').hide();
            $(document).find('#advprocessdedbydaterange ').daterangepicker({
                format: 'MM/DD/YYYY'
            });
            StartProcessedByDate = '';
            EndProcessedByDate = '';
            break;
    }
});
$(document).on('click', '#resetgrid', function () {
    clearAdvanceSearch();
    localStorage.removeItem("PURCHASEREQUESTSTATUS");
    CustomQueryDisplayId = 0;
    dtPrTable.state.clear();
    generatePRDataTable();
});
function PRAdvSearch(status) {
    var InactiveFlag = false;
    var searchitemhtml = "";
    $(document).find('#txtColumnSearch').val('');
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).attr('id') == 'advcreatedaterange' || $(this).attr('id') == 'advprocessdedbydaterange') {
            if ($(this).val()) {
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }
        else {
            if ($(this).val()) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }

    });
    if (status != 0) {
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
        $('#sp_advprocessdedbydaterange').hide();
        $('#sp_advcreatedaterange').hide();
    }
    else {

    }
    $(".filteritemcount").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}
function clearAdvanceSearch() {
    selectCount = 0;
    $("#PurchaseRequest").val("");
    $("#Reason").val("");
    $("#Status").val("").trigger('change');
    $("#CreatedBy").val("");
    $('#Vendor').val("").trigger('change');
    $("#VendorName").val("");
    $("#Created").val("").trigger('change');
    StartCreateteDate = '';
    EndCreateteDate = '';
    $("#PONumber").val("");
    $("#ProcessedBy").val("");
    $("#DateProcessed").val("").trigger('change');
    StartProcessedByDate = '';
    EndProcessedByDate = '';
    $("#advsearchfilteritems").html('');
    $(".filteritemcount").text(selectCount);

}
var titleArray = [];
var classNameArray = [];
var order = '2';
var orderDir = 'asc';
function generatePRDataTable() {
    var printCounter = 0;
    if ($(document).find('#purchaserequestSearch').hasClass('dataTable')) {
        dtPrTable.destroy();
    }
    dtPrTable = $("#purchaserequestSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 3
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[2, "asc"]],
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
                        order = LayoutInfo.order[0][0];
                        orderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
                            GetCreateDateRangeFilterData();
                            GetProcessedDateRangeFilterData();

                        }
                    }
                    else {
                        callback(json);
                    }

                }
            });
        },
        scrollX: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Purchase Request List'
            },
            {
                extend: 'print',
                title: 'Purchase Request List'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Purchase Request List',
                extension: '.csv',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                orientation: 'landscape',
                pageSize: 'A3',
                title: 'Purchase Request List'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/PurchaseRequest/GetPRGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.CustomQueryDisplayId = CustomQueryDisplayId;
                d.ProcessedStartDateVw = ValidateDate(ProcessedStartDateVw);
                d.ProcessedEndDateVw = ValidateDate(ProcessedEndDateVw);
                d.CreateStartDateVw = ValidateDate(CreateStartDateVw);
                d.CreateEndDateVw = ValidateDate(CreateEndDateVw);
                d.CancelandDeniedStartDateVw = ValidateDate(CancelandDeniedStartDateVw);
                d.CancelandDeniedEndDateVw = ValidateDate(CancelandDeniedEndDateVw);
                d.PurchaseRequest = LRTrim($("#PurchaseRequest").val());
                d.Reason = LRTrim($("#Reason").val());
                d.Status = LRTrim($("#Status").val());
                d.CreatedBy = LRTrim($("#CreatedBy").val());
                d.Creator_PersonnelName = LRTrim($("#CreatedBy").val());
                d.Vendor = LRTrim($('#Vendor').val());
                d.VendorName = LRTrim($("#VendorName").val());
                d.CreateStartDate = ValidateDate(StartCreateteDate);
                d.CreateEndDate = ValidateDate(EndCreateteDate);
                d.PONumber = LRTrim($("#PONumber").val());
                d.ProcessedBy = LRTrim($("#ProcessedBy").val());
                d.Processed_PersonnelName = LRTrim($("#ProcessedBy").val());
                d.ProcessedStartDate = ValidateDate(StartProcessedByDate);
                d.ProcessedEndDate = ValidateDate(EndProcessedByDate);
                d.txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
                d.Order = order;
            },
            "dataSrc": function (result) {
                let colOrder = dtPrTable.order();
                orderDir = colOrder[0][1];

                var i = 0;
                SetMultiSelectAction(CustomQueryDisplayId);
                totalcount = result.recordsTotal;
                HidebtnLoaderclass("LoaderDrop");
                if (result.data.length == "0") {
                    $(document).find('.import-export,#purchaserequest-select-all').attr('disabled', 'disabled');
                }
                else {
                    $(document).find('.import-export,#purchaserequest-select-all').removeAttr('disabled');
                }

                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "PurchaseRequestId",
                    "bVisible": true,
                    "bSortable": false,
                    "autoWidth": false,
                    "bSearchable": false,
                    "mRender": function (data, type, row) {
                        if (row.ChildCount > 0) {
                            return '<img id="' + data + '" src="../../Images/details_open.png" alt="expand/collapse" rel="' + data + '" style="cursor: pointer;"/>';
                        }
                        else {
                            return '';
                        }
                    }
                },
                {
                    "data": "PurchaseRequestId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    targets: 0,
                    "name": "0",
                    'render': function (data, type, full, meta) {
                        if ($('#purchaserequest-select-all').is(':checked') && totalcount == SelectPRId.length) {
                            return '<input type="checkbox" checked="checked"  data-prid="' + data + '" class="chkPRsearch ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        } else {
                            if (SelectPRId.indexOf(data) != -1) {
                                return '<input type="checkbox" checked="checked" data-prid="' + data + '" class="chkPRsearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                            else {
                                return '<input type="checkbox"  data-prid="' + data + '" class="chkPRsearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                        }

                    }
                },
                {
                    "data": "ClientLookupId",
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
                    "data": "Reason", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3", /*"name": "2",*/
                    mRender: function (data, type, full, meta) {
                        if (data == statusCode.Approved) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--yellow m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Canceled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--orange m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Complete) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Denied) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--red m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Open) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.AwaitApproval) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--light-blue m-badge--wide' style='width:105px;' >" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Order) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--teal m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Resubmit) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--purple m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Extracted) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--grey m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Consolidated) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--red m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else {
                            return getStatusValue(data);
                        }
                    }
                },
                { "data": "Creator_PersonnelName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4", }, //
                { "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },  //--previous name : 4
                { "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" }, //--previous name : 5
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date ", "name": "7" }, //--previous name : 6
                { "data": "PurchaseOrderClientLookupId", "autoWidth": true, "bSearchable": false, "bSortable": true, "name": "8" }, //--previous name : 7
                { "data": "Processed_PersonnelName", "autoWidth": true, "bSearchable": false, "bSortable": true, "name": "9" }, //--previous name : 8
                { "data": "ProcessedDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "10" } //--previous name : 9
            ],
        columnDefs: [
            {
                targets: [0, 1, 2],
                className: 'noVis'
            }
        ],
        initComplete: function (settings, json) {
            SetPageLengthMenu();
            //----------conditional column hiding-------------//
            //var api = new $.fn.dataTable.Api(settings);
            //var columns = dtPrTable.settings().init().columns;
            //var arr = [];
            //var j = 0;
            //while (j < json.hiddenColumnList.length) {
            //    var clsname = '.' + json.hiddenColumnList[j];
            //    var title = dtPrTable.columns(clsname).header();
            //    titleArray.push(title[0].innerHTML);
            //    classNameArray.push(clsname);
            //    dtPrTable.columns(clsname).visible(false);
            //    var sortMenuItem = '.dropdown-menu' + ' ' + clsname;
            //    $(sortMenuItem).remove();

            //    //---hide adv search items---
            //    var advclsname = '.' + "prc-" + json.hiddenColumnList[j];
            //    $(document).find(advclsname).hide();
            //    j++;
            //}
            //----------------------------------------------//

            $("#PurchaseRequestGridAction :input").removeAttr("disabled");
            $("#PurchaseRequestGridAction :button").removeClass("disabled");
            DisableExportButton($("#purchaserequestSearch"), $(document).find('.import-export'));
        }
    });
}
$('#purchaserequestSearch').find('th').click(function () {
    if ($(this).data('col')) {
        run = true;
        order = $(this).data('col');
    }

});


function generatePRShoppingCartDataTable() {
    IsUsePartMaster == false;
    if ($(document).find('#tblPurchaseRequestShoppingCart').hasClass('dataTable')) {
        dtPrShoppingCartTable.destroy();
    }
    dtPrShoppingCartTable = $("#tblPurchaseRequestShoppingCart").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "pagingType": "full_numbers",
        "order": [],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/PurchaseRequest/GetPRShoppingCartGrid",
            "type": "post",
            "datatype": "json",
            "dataSrc": function (result) {
                totalcount = result.recordsTotal;
                IsUsePartMaster = result.IsUsePartMaster;
                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                if (result.ErrorList.length > 0) {
                    ErrorListTable(result.ErrorList);
                }
                return result.data;
            },
            global: true
        },
        "columns":
            [
                { "data": "SupplierPartId", "autoWidth": false, "bSearchable": false, "bSortable": false, "width": "10px" },
                { "data": "SupplierPartAuxiliaryId", "autoWidth": false, "bSearchable": false, "bSortable": false, "width": "10px" },
                {
                    "data": "Description", "autoWidth": false, "bSearchable": false, "bSortable": false,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "OrderQuantity", "autoWidth": false, "bSearchable": false, "bSortable": false, "width": "10px", "className": "text-right" },
                { "data": "UnitofMeasure", "autoWidth": true, "bSearchable": false, "bSortable": false },
                {
                    "data": "OrderUnit", "autoWidth": true, "bSearchable": false, "bSortable": false,
                    'render': function (data, type, row, meta) {
                        var result = "<select class='select-OrderUnit dt-inline-dropdown' style='width: 170px !important'>";
                        result += "<option value='' disabled>--Select--</option>";
                        $(row.OrderUnitListdropDown).each(function (index, value) {
                            var selected = value.value === row.OrderUnit ? "selected" : "";
                            result += "<option value='" + value.value + "' " + selected + ">" + value.label + "</option>";
                        });
                        result += "</select>";
                        return result;
                    }
                },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": false, "bSortable": false, "className": "text-right" },
                { "data": "ManufacturerName", "autoWidth": true, "bSearchable": false, "bSortable": false },
                { "data": "ManufacturerPartID", "autoWidth": true, "bSearchable": false, "bSortable": false, "className": "text-right" },
                {
                    "data": "PartId", "autoWidth": true, "bSearchable": false, "bSortable": false,
                    'render': function (data, type, row, meta) {
                        var PartClientLookupId = row.Part_ClientLookupId;
                        if (row.Part_ClientLookupId == null || row.Part_ClientLookupId == '') {
                            PartClientLookupId = '';
                        }
                        var shopingPartId = row.PartId;
                        if (row.PartId == null || row.PartId == '' || row.PartId == 0) {
                            shopingPartId = '';
                        }
                        var result = "";
                        result = "<div class='divShopingCart'><input type='hidden' id='PartId_" + meta.row + "' value='" + shopingPartId + "'>";
                        result += "<input type='text' class='form-control search dropbtn readonly' style='width: 118px;float: left;border-radius: 0.25rem 0 0 0.25rem;' id='PartClientLookup_" + meta.row + "' readonly value='" + PartClientLookupId + "'>";
                        result += "<div class='input-group-btn' style='float: left;margin-left: -1px;'><button class='btn btn-white btntxtInputGroup openpartgrid' data-td-row='" + meta.row + "' type = 'button'>";
                        result += "<i id='srcicon' class='fa fa-search'></i>";
                        result += "</button>";
                        result += "</div></div >";
                        return result;
                    }
                },
                {
                    "data": "ChargeType", "autoWidth": true, "bSearchable": false, "bSortable": false,
                    'render': function (data, type, full, meta) {
                        var result = "";
                        result = "<select class='select-chargetype dt-inline-dropdown' style='width: 170px !important'>";
                        result += "<option value='0' selected>--Select--</option>"
                        $(full.ChargeTypeListdropDown).each(function (index, value) {
                            result += "<option value='" + value.value + "'>" + value.label + "</option>"
                        });
                        result += "</select>";
                        return result;
                    }
                },
                {
                    "data": "ChargeToID", "autoWidth": true, "bSearchable": false, "bSortable": false, "type": "date",
                    'render': function (data, type, full, meta) {

                        return "<div class='divShopingCart'><input type='text' class='form-control search chargetolookup  dropbtn readonly' style='width: 130px;float: left;border-radius: 0.25rem 0 0 0.25rem;' readonly='readonly' autocomplete='off' > " +
                            "<input type= 'text' class='chargetolookupval  dropbtn ' style= 'display:none' autocomplete= 'off' > " +
                            "<button class='btn btn-white btntxtInputGroup openchargetolookupgrid' style='float: left;margin-left: -1px !important;' type='button'>" +
                            " <i id='srcicon' class='fa fa-search'></i>" +
                            "</button></div>";
                    }
                },
                {
                    "data": "AccountId", "autoWidth": true, "bSearchable": false, "bSortable": false, "type": "date",
                    'render': function (data, type, full, meta) {
                        return "<div class='divShopingCart'><input type='text' class='form-control search accountlookup  dropbtn readonly' style='width: 130px;float: left;border-radius: 0.25rem 0 0 0.25rem;' readonly='readonly' autocomplete='off' > " +
                            "<input type= 'text' class='accountlookupval  dropbtn ' style= 'display:none' autocomplete= 'off' > " +
                            "<button class='btn btn-white btntxtInputGroup openaccountlookupgrid' style='float: left;margin-left: -1px !important;' type='button'>" +
                            "<i id='srcicon' class='fa fa-search'></i>" +
                            "</button></div>";
                    }
                },
                {
                    "data": "RequiredDate", "autoWidth": true, "bSearchable": false, "bSortable": false, "type": "date",
                    'render': function (data, type, full, meta) {
                        return "<input type='text' class='form-control search adv-item dtpicker' id='RequiredDate_" + meta.row + "'  style='width: 130px;float: left;border-radius: 0.25rem 0 0 0.25rem;' readonly='readonly' autocomplete='off' >";
                    }
                },
                {
                    "data": "UNSPSC", "autoWidth": true, "bSearchable": false, "bSortable": false,
                    'render': function (data, type, row, meta) {

                        result = "<div class='divShopingCart'><input type='hidden' id='PartCategoryMasterId_" + meta.row + "'>";
                        result += "<input type='text' class='form-control search dropbtn readonly' style='width: 118px;float: left;border-radius: 0.25rem 0 0 0.25rem;' id='PartCategoryMasterClientLookupId_" + meta.row + "' readonly value=''>";
                        result += "<button class='btn btn-white btntxtInputGroup mobBttn OpenPartCategoryMasterModalPopupGrid' type='button' data-textfield='PartCategoryMasterClientLookupId_" + meta.row + "' data-valuefield='PartCategoryMasterId_" + meta.row + "'><i id='srcicon' class='fa fa-search'></i></button></div>";
                        return result;
                    }
                },
            ],
        initComplete: function () {
            if (IsUsePartMaster == false) {
                var column = this.api().column(14);
                column.visible(false);
            }
            $(document).find('.select-chargetype').select2({
                minimumResultsForSearch: -1
            });
            $(document).find('.select-OrderUnit').select2({
                minimumResultsForSearch: -1
            });
            $(document).find('.dtpicker').datepicker({
                minDate: 1,
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true
            }).inputmask('mm/dd/yyyy');
            SetPageLengthMenu();
        },
        "drawCallback": function (oSettings) {
            $(document).find('.select-chargetype').select2({
                minimumResultsForSearch: -1
            });
            $(document).find('.select-OrderUnit').select2({
                minimumResultsForSearch: -1
            });
        }
    });
}
$(document).on('change', '.select-chargetype', function () {
    $(this).parent('td').next('td').find('.chargetolookup').val('');
    $(this).parent('td').next('td').find('.chargetolookupval').val('');
    $(this).parent('td').prev('td').find('input[type="text"]').val('');
    $(this).parent('td').prev('td').find('input[type="hidden"]').val('');
});
function ErrorListTable(ErrorList) {
    var html = '';

    html = html + '<ul class="listView nobrd divErrorMsg" >' +
        '<li class="errorTitle">Error</li>' +
        '<li>' +
        '<span class="label" style="font-weight:500;">SupplierPartAuxiliaryId</span>' +
        '<span class="inputText" style="font-weight:500;">Message</span>' +
        '<div style="clear:both;"></div>' +
        '</li>';

    $.each(ErrorList, function (i, elem) {
        html = html + '<li>' +
            '<span class="label">' + elem.text + '</span>' +
            '<span class="inputText">' + elem.value + '</span>' +
            '<div style="clear:both;"></div>' +
            '</li>';
    });
    html = html + '</ul >';

    $('#divErrorMsg').html(html);
    $('#divErrorMsg').css('display', 'block');
}
function SetMultiSelectAction(CustomQueryDisplayId) {
    $("#ConvertToPurchaseOrderListPR").removeAttr("disabled");
    $("#returnToRequestorListPR").removeAttr("disabled");
    $("#denyListPR").removeAttr("disabled");
    $("#approveListPR").removeAttr("disabled");
    if (CustomQueryDisplayId == "1") {
        $("#ConvertToPurchaseOrderListPR").attr("disabled", "disabled");
        $("#returnToRequestorListPR").attr("disabled", "disabled");

    }
    else if (CustomQueryDisplayId == "2") {
        $("#ConvertToPurchaseOrderListPR").attr("disabled", "disabled");
    }
    else if (CustomQueryDisplayId == "4") {
        $("#ConvertToPurchaseOrderListPR").attr("disabled", "disabled");
        $("#returnToRequestorListPR").attr("disabled", "disabled");
        $("#denyListPR").attr("disabled", "disabled");
        $("#approveListPR").attr("disabled", "disabled");
    }
    else if (CustomQueryDisplayId == "5") {
        $("#ConvertToPurchaseOrderListPR").attr("disabled", "disabled");
        $("#returnToRequestorListPR").attr("disabled", "disabled");
        $("#denyListPR").attr("disabled", "disabled");
        $("#approveListPR").attr("disabled", "disabled");
    }
    else if (CustomQueryDisplayId == "8") {
        $("#returnToRequestorListPR").attr("disabled", "disabled");
        $("#denyListPR").attr("disabled", "disabled");
        $("#approveListPR").attr("disabled", "disabled");
    }

}
$(document).find('#purchaserequestSearch').on('click', 'tbody td img', function (e) {
    var tr = $(this).closest('tr');
    var row = dtPrTable.row(tr);
    if (this.src.match('details_close')) {
        this.src = "../../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        this.src = "../../Images/details_close.png";
        var PurchaseRequestID = $(this).attr("rel");
        $.ajax({
            url: "/PurchaseRequest/GetPrInnerGrid",
            data: {
                PurchaseRequestID: PurchaseRequestID
            },
            beforeSend: function () {
                ShowLoader();
            },
            dataType: 'html',
            success: function (json) {
                row.child(json).show();
                dtinnerGrid = row.child().find('.purchaseRequestinnerDataTable').DataTable(
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
                        "columnDefs": [
                            { className: 'text-right', targets: [3, 5, 6] },
                            {
                                "render": function (data, type, row) {
                                    return "<div class='text-wrap'>" + data + "</div>";
                                }
                                , targets: [2]
                            }
                        ],
                        "footerCallback": function (row, data, start, end, display) {
                            var api = this.api(),
                                // Total over all pages
                                total = api.column(6).data().reduce(function (a, b) {
                                    return parseFloat(a) + parseFloat(b);
                                }, 0);
                            // Update footer
                            $(api.column(6).footer()).html(total.toFixed(2));
                        },
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
$(document).on('click', '#purchaserequestSearch_paginate .paginate_button', function () {
    PRAdvSearch();
    run = true;
});
$(document).on('change', '#purchaserequestSearch_length .searchdt-menu', function () {
    PRAdvSearch();
    run = true;
});

$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var PurchaseRequest = LRTrim($("#PurchaseRequest").val());
            var Reason = LRTrim($("#Reason").val());
            var Status = LRTrim($("#Status").val());
            var CreatedBy = LRTrim($("#CreatedBy").val());
            var Vendor = LRTrim($('#Vendor').val());
            var VendorName = LRTrim($("#VendorName").val());
            var PONumber = LRTrim($("#PONumber").val());
            var ProcessedBy = LRTrim($("#ProcessedBy").val());
            var DateProcessed = $("#DateProcessed").val();
            var colname = order;
            var coldir = orderDir;

            var txtsearchval = LRTrim($("#txtColumnSearch").val());
            var jsonResult = $.ajax({
                url: '/PurchaseRequest/GetPRPrintData?page=all',
                data: {
                    CustomQueryDisplayId: CustomQueryDisplayId,
                    ProcessedStartDateVw: ValidateDate(ProcessedStartDateVw),
                    ProcessedEndDateVw: ValidateDate(ProcessedEndDateVw),
                    CreateStartDateVw: ValidateDate(CreateStartDateVw),
                    CreateEndDateVw: ValidateDate(CreateEndDateVw),
                    CancelandDeniedStartDateVw: ValidateDate(CancelandDeniedStartDateVw),
                    CancelandDeniedEndDateVw: ValidateDate(CancelandDeniedEndDateVw),
                    PurchaseRequest: PurchaseRequest,
                    Reason: Reason,
                    Status: Status,
                    CreatedBy: CreatedBy,
                    Vendor: Vendor,
                    VendorName: VendorName,
                    CreateStartDate: ValidateDate(StartCreateteDate),
                    CreateEndDate: ValidateDate(EndCreateteDate),
                    PONumber: PONumber,
                    ProcessedStartDate: ValidateDate(StartProcessedByDate),
                    ProcessedEndDate: ValidateDate(EndProcessedByDate),
                    DateProcessed: DateProcessed,
                    colname: colname,
                    coldir: coldir,
                    txtSearchval: txtsearchval
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#purchaserequestSearch thead tr th").not(":eq(1)").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.CreateDate != null) {
                    item.CreateDate = item.CreateDate;
                }
                else {
                    item.CreateDate = "";
                }
                if (item.ProcessedDate != null) {
                    item.ProcessedDate = item.ProcessedDate;
                }
                else {
                    item.ProcessedDate = "";
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
                header: $("#purchaserequestSearch thead tr th").not(":eq(1)").find('div').map(function (key) {
                    if ($(this).parents('.purchaseRequestinnerDataTable').length == 0 && this.innerHTML) {
                        return this.innerHTML;
                    }
                }).get()
            };
        }
    });
});
$(document).on('click', '.lnk_psearch', function (e) {
    e.preventDefault();
    var index_row = $('#purchaserequestSearch tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtPrTable.row(row).data();
    var PurchaseRequestId = data.PurchaseRequestId;
    var titletext = $('#prsearchtitle').text();
    localStorage.setItem("prstatustext", titletext);
    $.ajax({
        url: "/PurchaseRequest/Details",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { PurchaseRequestId: PurchaseRequestId },
        success: function (data) {
            $('#renderpurchaserequest').html(data);
            $(document).find('#spnlinkToSearch').text(titletext);
            if ($(document).find('.AddPrequest').length === 0) { $(document).find('#prdetailactiondiv').css('margin-right', '0px'); }
        },
        complete: function () {
            var dt = $("#tblLineItem").DataTable();
            dt.state.clear();
            dt.destroy();
            generateLineiItemdataTable(PurchaseRequestId, "");
            SetPRControls();
            CloseLoader();
        }
    });
});

$(document).on('click', '#purchaserequest-select-all', function (e) {
    SelectPRDetails = [];
    SelectPRId = [];
    var PurchaseRequest = LRTrim($("#PurchaseRequest").val());
    var Reason = LRTrim($("#Reason").val());
    var Status = LRTrim($("#Status").val());
    var CreatedBy = LRTrim($("#CreatedBy").val());
    var Vendor = LRTrim($('#Vendor').val());
    var VendorName = LRTrim($("#VendorName").val());
    var PONumber = LRTrim($("#PONumber").val());
    var ProcessedBy = LRTrim($("#ProcessedBy").val());
    var DateProcessed = $("#DateProcessed").val();
    dtPrTable = $("#purchaserequestSearch").DataTable();
    var colname = order;
    var coldir = orderDir;
    var txtsearchval = LRTrim($("#txtColumnSearch").val());
    var checked = this.checked;
    $.ajax({
        url: '/PurchaseRequest/GetPRSelectAllData?page=all',
        data: {
            CustomQueryDisplayId: CustomQueryDisplayId,
            ProcessedStartDateVw: ValidateDate(ProcessedStartDateVw),
            ProcessedEndDateVw: ValidateDate(ProcessedEndDateVw),
            CreateStartDateVw: ValidateDate(CreateStartDateVw),
            CreateEndDateVw: ValidateDate(CreateEndDateVw),
            CancelandDeniedStartDateVw: ValidateDate(CancelandDeniedStartDateVw),
            CancelandDeniedEndDateVw: ValidateDate(CancelandDeniedEndDateVw),
            PurchaseRequest: PurchaseRequest,
            Reason: Reason,
            Status: Status,
            CreatedBy: CreatedBy,
            Vendor: Vendor,
            VendorName: VendorName,
            CreateStartDate: ValidateDate(StartCreateteDate),
            CreateEndDate: ValidateDate(EndCreateteDate),
            PONumber: PONumber,
            ProcessedStartDate: ValidateDate(StartProcessedByDate),
            ProcessedEndDate: ValidateDate(EndProcessedByDate),
            DateProcessed: DateProcessed,
            colname: colname,
            coldir: coldir,
            txtSearchval: txtsearchval
        },
        async: true,
        type: "GET",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    if (checked) {
                        var exist = $.grep(SelectPRDetails, function (obj) {
                            return obj.PurchaseRequestId === item.PurchaseRequestId;
                        });
                        if (exist.length == 0) {
                            var PurchaseRequestId = item.PurchaseRequestId;
                            var ClientLookupId = item.ClientLookupId;
                            var Status = item.Status;
                            var ChildCount = item.ChildCount;
                            var VendorId = item.VendorId;
                            var VendorIsExternal = item.VendorIsExternal;
                            var thisPurchaseRequest = new PurchaseRequestNotInSelectedItem(PurchaseRequestId, ClientLookupId, Status, ChildCount, VendorId, VendorIsExternal);  /*V2-375*/
                            SelectPRDetails.push(thisPurchaseRequest);
                            SelectPRId.push(PurchaseRequestId);
                        }
                    } else {
                        SelectPRDetails = $.grep(SelectPRDetails, function (obj) {
                            return obj.PurchaseRequestId !== item.PurchaseRequestId;
                        });

                        var i = SelectPRId.indexOf(item.PurchaseRequestId);
                        SelectPRId.splice(i, 1);
                    }

                });
            }
        },
        complete: function () {
            $('.itemcount').text(SelectPRDetails.length);
            selectedcount = SelectPRDetails.length;
            if (checked) {
                $(document).find('.dt-body-center').find('.chkPRsearch').prop('checked', 'checked');
                $(".actionBar").hide();
                $(".updateArea").fadeIn();
                $('#PrintPRCheckList').removeAttr("disabled");
            }
            else {
                $(document).find('.dt-body-center').find('.chkPRsearch').prop('checked', false);
                $(".updateArea").hide();
                $(".actionBar").fadeIn();
                $('#PrintPRCheckList').prop("disabled", "disabled");

            }
            CloseLoader();
        }
    });
});

$(document).on('change', '.chkPRsearch', function () {
    var data = dtPrTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        var el = $('#purchaserequest-select-all').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        SelectPRDetails = SelectPRDetails.filter(function (el) {
            return el.PurchaseRequestId !== data.PurchaseRequestId;
        });
        var index = SelectPRId.indexOf(data.PurchaseRequestId);
        SelectPRId.splice(index, 1);
    }
    else {
        var item = new PurchaseRequestNotInSelectedItem(data.PurchaseRequestId, data.ClientLookupId, data.Status, data.ChildCount, data.VendorId, data.VendorIsExternal);  /*V2-375*/
        var found = SelectPRDetails.some(function (el) {
            return el.PurchaseRequestId === data.PurchaseRequestId;
        });
        if (!found) {
            SelectPRDetails.push(item);
            SelectPRId.push(data.PurchaseRequestId);
        }

    }
    if (SelectPRDetails.length == totalcount) {
        $(document).find('.dt-body-center').find('#purchaserequest-select-all').prop('checked', 'checked');
    }
    else {
        $(document).find('.dt-body-center').find('#purchaserequest-select-all').prop('checked', false);
    }
    if (SelectPRDetails.length > 0) {
        $(".actionBar").hide();
        $(".updateArea").fadeIn();
        $('#PrintPRCheckList,#PrintPRCheckListDevExpress').removeAttr("disabled");
    }
    else {

        $(".updateArea").hide();
        $(".actionBar").fadeIn();
        $('#PrintPRCheckList,#PrintPRCheckListDevExpress').prop("disabled", "disabled");

    }
    $('.itemcount').text(SelectPRDetails.length);
});
function PurchaseRequestNotInSelectedItem(PurchaseRequestId, ClientLookupId, Status, ChildCount, VendorId, VendorIsExternal) {
    this.PurchaseRequestId = PurchaseRequestId;
    this.ClientLookupId = ClientLookupId;
    this.Status = Status;
    this.ChildCount = ChildCount;
    this.VendorId = VendorId;
    this.VendorIsExternal = VendorIsExternal;
};
$(document).on('click', '#PrintPRCheckList', function () {
    if (SelectPRDetails.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var jsonResult = {
            "list": SelectPRDetails
        }
        {
            $.ajax({
                url: '/PurchaseRequest/PrintPRListFromIndex',
                data: JSON.stringify(jsonResult),
                type: "POST",
                datatype: "json",
                contentType: 'application/json; charset=utf-8',
                responseType: 'arraybuffer',
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (result) {
                    if (result.success && result.jsonStringExceed == false) {
                        PdfPrintAllPRList(result.pdf);
                    }
                    else {
                        CloseLoader();
                        var errorMessage = getResourceValue("PdfFileSizeExceedAlert");
                        ShowErrorAlert(errorMessage);
                        return false;
                    }

                },
                complete: function () {
                    CloseLoader();
                    $(".updateArea").hide();
                    $(".actionBar").fadeIn();
                    $(document).find('.dt-body-center').find('#purchaserequest-select-all').prop('checked', false);
                    $(document).find('.chkPRsearch').prop('checked', false);
                    $('.itemcount').text(0);
                    SelectPRDetails = [];
                    SelectPRId = [];
                }
            });
        }
    }
});
function PdfPrintAllPRList(pdf) {
    var blob = b64StrtoBlob(pdf, 'application/pdf');
    var blobUrl = URL.createObjectURL(blob);
    var htmlbody = '<iframe id="iframeid" src=' + blobUrl + ' style="position:fixed; top:10; left:0; bottom:0; right:0; width:100%; height:100%; border:none; margin:0; padding:0; overflow:hidden; z-index:999999;" style="display:none" > </iframe>';
    var winhtml = window.open("", "PdfWndow", "width=800,height=800");
    if (winhtml) {
        winhtml.document.write(htmlbody);
        winhtml.document.getElementById('iframeid').contentWindow.print();
    }
    else {
        var errorMessage = "Please check if pop-up is enabled for Somax.";
        ShowErrorAlert(errorMessage);
        return false;
    }
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
    var blob = new Blob(byteArrays, { type: contentType, ContentDisposition: "inline" });
    return blob;
}
//#endregion
//#region Line Item
function PartNotInInventorySelectedItem(PartId, ClientLookupId, Description, Manufacture, Quantity) {
    this.PartId = PartId;
    this.ClientLookupId = ClientLookupId;
    this.Description = Description;
    this.Manufacture = Manufacture;
    this.Quantity = Quantity;
};
function PartNotInInventoryProcessdata(PartId, ClientLookupId, Description, OrderQuantity, PurchaseRequestId) {
    this.PartId = PartId;
    this.ClientLookupId = ClientLookupId;
    this.Description = Description;
    this.OrderQuantity = OrderQuantity;
    this.PurchaseRequestId = PurchaseRequestId;
};
var PartNotInInventorySelectedItemArray = [];
var FinalGridSelectedItemArray = [];
var dtLineItemTable;
var lItemfilteritemcount = 0;
var finalSelectPartsTable;
var SelectPartsTable;
var GrandTotalCost = 0;
var recordsTotalcount = 0;
function generateLineiItemdataTable(PurchaseRequestId, searchtext) {
    var rCount = 0;
    var srcData = 0;
    var IsShoppingCart = false;
    var LineNumber = $("#LineNo").val();
    if (LineNumber) {
        LineNumber = LRTrim(LineNumber);
    }
    var PartId = $("#PartID").val();
    if (PartId) {
        PartId = LRTrim(PartId);
    }
    var Description = $("#Description").val();
    if (Description) {
        Description = LRTrim(Description);
    }
    var Quantity = $("#OrderQty").val();
    if (Quantity) {
        Quantity = LRTrim(Quantity);
    }
    var UOM = $('#UOM').val();
    if (UOM) {
        UOM = LRTrim(UOM);
    }
    var UnitCost = $("#UnitCost").val();
    if (UnitCost) {
        UnitCost = LRTrim(UnitCost);
    }
    var TotalCost = $("input[id='TotalCost']").val();
    if (TotalCost) {
        TotalCost = LRTrim(TotalCost);
    }
    var Account = $('#liAccount').val();
    if (Account) {
        Account = LRTrim(Account);
    }
    var ChargeToClientLookupId = $("#ChargeToClientLookupId").val();
    if (ChargeToClientLookupId) {
        ChargeToClientLookupId = LRTrim(ChargeToClientLookupId);
    }
    var visibility = lineNumberGridSecurity;
    var visibilityAddLineItem = AddLineItemSecurity;
    var PunchoutFunctionality = IsPunchout;
    var actionvisiblity = false;
    if ($(document).find('#tblLineItem').hasClass('dataTable')) {
        dtLineItemTable.destroy();
    }
    dtLineItemTable = $("#tblLineItem").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "bProcessing": true,
        "order": [[0, "asc"]],
        stateSave: false,
        "pagingType": "full_numbers",
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/PurchaseRequest/PopulateLineItem",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.PurchaseRequestId = PurchaseRequestId;
                d.searchText = searchtext;
                d.LineNumber = LineNumber;
                d.PartId = PartId;
                d.Description = Description;
                d.Quantity = Quantity;
                d.UOM = UOM;
                d.UnitCost = UnitCost;
                d.TotalCost = TotalCost;
                d.Account = Account;
                d.ChargeToClientLookupId = ChargeToClientLookupId;
            },
            "dataSrc": function (result) {
                rCount = result.data.length;
                IsShoppingCart = result.IsShoppingCart;
                GrandTotalCost = result.GrandTotalCost;
                recordsTotalcount = result.recordsTotal;
                return result.data;
            },
            global: true
        },
        columnDefs: [
            {
                "data": null,
                targets: [10], render: function (a, b, data, d) {

                    if (visibility == "True" && visibilityAddLineItem == "False") {
                        actionvisiblity = true;
                        return '<a class="btn btn-outline-success editPRLineItemBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delPRLineItemBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else if (visibilityAddLineItem == "True" && visibility == "False" && PunchoutFunctionality == "False") {
                        actionvisiblity = true;
                        return '<a class="btn btn-outline-primary addPRLineItemBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>'
                    }
                    else if (visibility == "True" && visibilityAddLineItem == "True" && PunchoutFunctionality == "False") {
                        actionvisiblity = true;
                        return '<a class="btn btn-outline-primary addPRLineItemBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editPRLineItemBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delPRLineItemBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else if (visibility == "True" && visibilityAddLineItem == "True" && PunchoutFunctionality == "True") {
                        actionvisiblity = true;
                        return '<a class="btn btn-outline-success editPRLineItemBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delPRLineItemBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        actionvisiblity = false;
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "LineNumber", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "OrderQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "UnitofMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right",
                    "mRender": function (data, type, row) {
                        return data.toFixed(2);
                    }
                },
                {
                    "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right",
                    "mRender": function (data, type, row) {
                        return data.toFixed(2);
                    }
                },
                { "data": "Account_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "RequiredDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date",
                    render: function (data, type, row, meta) {
                        if (data == null) {
                            return '';
                        } else {
                            return data;
                        }
                    }
                },
                { "data": "ChargeToClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }

            ],
        "footerCallback": function (row, data, start, end, display) {
            var info = dtLineItemTable.page.info();
            var api = this.api();
            var rows = api.rows().nodes();
            var getData = api.rows({ page: 'current' }).data();
            total = GrandTotalCost;

            var start = info.start;
            var end = info.end;
            $("#tblLineItemfoot").empty().append("");
            // Update footer
            if (data.length != 0 && recordsTotalcount == end) {
                var footer = "";
                if (IsShoppingCart == false) {
                    if (actionvisiblity == true)
                        footer = '<tr><th></th><th></th><th></th><th></th><th></th><th style="text-align: left!important; font-weight: 500!important; color:#0b0606!important">Grand Total</th><th style = "text-align: right!important; font-weight: 500!important; color: #0b0606!important; padding: 0px 10px 0px 0px!important" >' + total.toFixed(2) + '</th><th></th><th></th><th></th></tr>'
                    else
                        footer = '<tr><th></th><th></th><th></th><th></th><th></th><th style="text-align: left!important; font-weight: 500!important; color:#0b0606!important">Grand Total</th><th style = "text-align: right!important; font-weight: 500!important; color: #0b0606!important; padding: 0px 10px 0px 0px!important" >' + total.toFixed(2) + '</th><th></th><th></th></tr>'

                }
                else {
                    if (actionvisiblity == true)
                        footer = '<tr><th></th><th></th><th></th><th></th><th></th><th style="text-align: left!important; font-weight: 500!important; color:#0b0606!important">Grand Total</th><th style = "text-align: right!important; font-weight: 500!important; color: #0b0606!important; padding: 0px 10px 0px 0px!important" >' + total.toFixed(2) + '</th><th></th><th></th><th></th><th></th></tr>'
                    else
                        footer = '<tr><th></th><th></th><th></th><th></th><th></th><th style="text-align: left!important; font-weight: 500!important; color:#0b0606!important">Grand Total</th><th style = "text-align: right!important; font-weight: 500!important; color: #0b0606!important; padding: 0px 10px 0px 0px!important" >' + total.toFixed(2) + '</th><th></th><th></th><th></th></tr>'

                }

                $("#tblLineItemfoot").empty().append(footer);
            }
        },
        initComplete: function (settings, json) {
            var column = this.api().column(10);
            var api = new $.fn.dataTable.Api(settings);
            api.column(8).visible(IsShoppingCart);
            if (rCount > 0) {
                $("#addLineItem").hide();
            }
            else {
                if (visibilityAddLineItem == "True" && PunchoutFunctionality == "False") {
                    $("#addLineItem").show();
                }
                else {
                    $("#addLineItem").hide();
                }
            }
            if (visibility == "False" && visibilityAddLineItem == "False") {
                column.visible(false);
            }
            else {
                column.visible(true);
            }
            SetPageLengthMenu();
            //----------conditional column hiding-------------//
            //var api = new $.fn.dataTable.Api(settings);
            //var columns = dtLineItemTable.settings().init().columns;
            //var arr = [];
            //var j = 0;
            //while (j < json.hiddenColumnList.length) {
            //    var clsname = '.G' + json.hiddenColumnList[j];
            //    dtLineItemTable.columns(clsname).visible(false);
            //    //---hide adv search items---
            //    var advclsname = '.' + "prcli-" + json.hiddenColumnList[j];
            //    $(document).find(advclsname).hide();
            //    j++;
            //}
            //----------------------------------------------//
        }
    });
}
$(document).on('click', "#btnLitemSearch", function () {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    clearLineItemAdvanceSearch();
    dtLineItemTable.state.clear();
    var searchText = LRTrim($('#txtsearchbox').val());
    generateLineiItemdataTable(PurchaseRequestId, searchText);
});
$(document).on('click', '#lineitemClearAdvSearchFilter', function () {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    $("#txtsearchbox").val("");
    clearLineItemAdvanceSearch();
    generateLineiItemdataTable(PurchaseRequestId, "");
});
$(document).on('click', "#btnLItemDataAdvSrch", function () {
    dtLineItemTable.state.clear();
    var searchitemhtml = "";
    filteritemcount = 0;
    lItemfilteritemcount = 0;
    $('#txtsearchbox').val('');
    $('#litemadvsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            lItemfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times litembtnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#lineitemadvsearchfilteritems').html(searchitemhtml);
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    LineItemAdvanceSearch();
    $('.lifilteritemcount').text(lItemfilteritemcount);
});
function clearLineItemAdvanceSearch() {
    var filteritemcount = 0;
    $('#litemadvsearchsidebar').find('input:text').val('');
    $('#UOM,#liAccount').val('').trigger('change');
    $('.lifilteritemcount').text(filteritemcount);
    $('#lineitemadvsearchfilteritems').html('');
}
function LineItemAdvanceSearch() {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    generateLineiItemdataTable(PurchaseRequestId, "");
}
$(document).on('click', '.litembtnCross', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId + '.adv-item').val('').trigger('change');
    $(this).parent().remove();
    lItemfilteritemcount--;
    LineItemAdvanceSearch();
    $('.lifilteritemcount').text(lItemfilteritemcount);
});
function EditPRLineItem(lineitemid) {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    var ClientLookupId = $(document).find('#purchaseRequestModel_ClientLookupId').val();
    var Status = $(document).find('#purchaseRequestModel_Status').val();
    $.ajax({
        url: "/PurchaseRequest/EditLineItem",
        type: "GET",
        dataType: 'html',
        data: { LineItemId: lineitemid, PurchaseRequestId: PurchaseRequestId, ClientLookupId: ClientLookupId, Status: Status },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchaserequest').html(data);
        },
        complete: function () {
            SetPRControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', '.editPRLineItemBttn', function () {
    var data = dtLineItemTable.row($(this).parents('tr')).data();
    var IsSingleStock = $(document).find('#purchaseRequestModel_SingleStockLineItem').val();
    if (IsSingleStock == "True" && data.PartId > 0) {
        EditPRLineItemPartInInventory(data.PurchaseRequestLineItemId, "/PurchaseRequest/EditPRPartInInventorySingleStockDynamic");
    }
    else {
        if (data.PartId > 0) {
            EditPRLineItemPartInInventory(data.PurchaseRequestLineItemId, "/PurchaseRequest/EditPRPartInInventoryDynamic");
        }
        else {
            EditPRLineItemPartNotInInventory(data.PurchaseRequestLineItemId);
        }
    }

});
function EditPRLineItemPartInInventory(lineitemid, url) {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    var ClientLookupId = $(document).find('#purchaseRequestModel_ClientLookupId').val();
    var StoreroomId = $(document).find('#purchaseRequestModel_StoreroomId').val();
    var Status = $(document).find('#purchaseRequestModel_Status').val();
    var VendorId = $(document).find('#purchaseRequestModel_VendorId').val();
    $.ajax({
        url: url,
        type: "GET",
        dataType: 'html',
        data: { LineItemId: lineitemid, PurchaseRequestId: PurchaseRequestId, ClientLookupId: ClientLookupId, Status: Status, StoreroomId: StoreroomId, VendorId: VendorId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchaserequest').html(data);
        },
        complete: function () {
            SetPRControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function EditPRLineItemPartNotInInventory(lineitemid) {
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    var ClientLookupId = $(document).find('#purchaseRequestModel_ClientLookupId').val();
    var Status = $(document).find('#purchaseRequestModel_Status').val();
    $.ajax({
        url: "/PurchaseRequest/EditPRPartNotInInventoryDynamic",
        type: "GET",
        dataType: 'html',
        data: { LineItemId: lineitemid, PurchaseRequestId: PurchaseRequestId, ClientLookupId: ClientLookupId, Status: Status },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchaserequest').html(data);
        },
        complete: function () {
            SetPRControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function EditLineItemOnSuccess(data) {
    CloseLoader();
    var PurchaseRequestId = data.purchaserequestid;
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("LineItemUpdatedAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPRequestDetail(PurchaseRequestId, "PurchasingOverview");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#brdprlineitem', function () {
    var PurchaseRequestId = $(this).attr('data-val');
    PartNotInInventorySelectedItemArray = [];
    RedirectToPRequestDetail(PurchaseRequestId);
});
$(document).on('click', "#btnprlineitemcancel", function () {
    var PurchaseRequestid = $(document).find('#lineItem_PurchaseRequestId').val();
    RedirectToDetailOncancel(PurchaseRequestid, "PurchasingOverview");
});
$(document).on('click', "#btnAddInventorycancel", function () {
    var PurchaseRequestid = $(document).find('#partInInventoryModel_PurchaseRequestId').val();
    PartNotInInventorySelectedItemArray = [];
    RedirectToDetailOncancel(PurchaseRequestid, "PurchasingOverview");
});
$(document).on('click', "#btncan", function () {
    var PurchaseRequestid = $(document).find('#PurchaseRequestId').val();
    RedirectToDetailOncancel(PurchaseRequestid, "PurchasingOverview");
});
$(document).on('click', "#selectidpartnotininventory", function (e) {
    e.preventDefault();
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    var Status = $(document).find('#purchaseRequestModel_Status').val();
    $('.modal-backdrop').remove();
    GoToAddPartNotInInventory(PurchaseRequestId, Status);
});
//#region PartInInventory add lineitem 
$(document).on('click', '.addPRLineItemBttn', function () {
    var data = dtLineItemTable.row($(this).parents('tr')).data();
    $('#purchaseRequestModel_PurchaseRequestLineItemId').val(data.PurchaseRequestLineItemId);
    $('#AddLineItems').modal('show');

});
//#endregion

$(document).on('click', "#selectidpartininventory", function (e) {
    e.preventDefault();
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    var PurchaseRequestLineItemId = $(document).find('#purchaseRequestModel_PurchaseRequestLineItemId').val();
    var Status = $(document).find('#purchaseRequestModel_Status').val();
    var IsSingleStock = $(document).find('#purchaseRequestModel_SingleStockLineItem').val();
    $('.modal-backdrop').remove();
    if (IsSingleStock == "True") {
        AddPartInInventorySingleStockLineItemDynamic(PurchaseRequestId, PurchaseRequestLineItemId, Status);
    }
    else {
        GoToAddPartInInventory(PurchaseRequestId);
    }

});
function GoToAddPartInInventory(PurchaseRequestId) {
    var ClientLookupId = $(document).find('#purchaseRequestModel_ClientLookupId').val();
    var vendorId = $(document).find('#purchaseRequestModel_VendorId').val();
    var StoreroomId = $(document).find('#purchaseRequestModel_StoreroomId').val();
    $.ajax({
        url: "/PurchaseRequest/AddPartInInventory",
        type: "POST",
        dataType: "html",
        data: { PurchaseRequestId: PurchaseRequestId, ClientLookupId: ClientLookupId, vendorId: vendorId, StoreroomId: StoreroomId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchaserequest').html(data);
            SetFixedHeadStyle();
        },
        complete: function () {
            CloseLoader();
            ShowCardView();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('change', '#PartNotInInventoryModel_ChargeType', function () {
    $(document).find('#txtChargeToId').val('');
});
function GoToAddPartNotInInventory(PurchaseRequestId, Status) {
    var ClientLookupId = $(document).find('#purchaseRequestModel_ClientLookupId').val();
    $.ajax({
        url: "/PurchaseRequest/ShowAddPartNotInInventoryDynamic",
        type: "GET",
        dataType: 'html',
        data: { PurchaseRequestId: PurchaseRequestId, ClientLookupId: ClientLookupId, Status: Status },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchaserequest').html(data);
        },
        complete: function () {
            SetPRControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function EditPartNotInOnSuccess(data) {
    CloseLoader();
    var PurchaseRequestId = data.purchaserequestid;
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("LineItemAddedAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPRequestDetail(PurchaseRequestId, "PurchasingOverview");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '.delPRLineItemBttn', function () {

    var data = dtLineItemTable.row($(this).parents('tr')).data();
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    var PurchaseRequestLineItemId = data.PurchaseRequestLineItemId;
    var Status = $(document).find('#purchaseRequestModel_Status').val();
    DeletePRLineItem(PurchaseRequestId, PurchaseRequestLineItemId, Status)
});
function DeletePRLineItem(PurchaseRequestId, PurchaseRequestLineItemId, Status) {
    CancelAlertSetting.closeOnConfirm = false;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: "/PurchaseRequest/DeleteLineItem",
            type: "GET",
            dataType: 'json',
            data: { PurchaseRequestLineItemId: PurchaseRequestLineItemId, PurchaseRequestId: PurchaseRequestId, Status: Status },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                var message = "";
                if (data.Result == "success") {
                    SuccessAlertSetting.text = getResourceValue("LineItemDeletedAlert");
                    swal(SuccessAlertSetting, function () {
                        if (data.Result == "success") {
                            RedirectToPRequestDetail(PurchaseRequestId, "PurchasingOverview");
                        }
                    });
                }
                else {
                    message = getResourceValue("LineItemDeleteFailedAlert");
                    swal({
                        title: getResourceValue("CommonErrorAlert"),
                        text: message,
                        type: "warning",
                        showCancelButton: false,
                        confirmButtonClass: "btn-sm btn-primary",
                        cancelButtonClass: "btn-sm",
                        confirmButtonText: getResourceValue("SaveAlertOk"),
                        cancelButtonText: getResourceValue("CancelAlertNo")
                    }, function () {
                    });
                }
            },
            complete: function () {
                CloseLoader();
            },
            error: function (jqXHR, exception) {
                CloseLoader();
            }
        });
    });
    CancelAlertSetting.closeOnConfirm = true;
}
//#endregion

//#region common
function SetPRControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var validstate = "";
        try {
            validstate = $(this).valid();
        } catch (e) {
            validstate = undefined;
        }
        var areaddescribedby = $(this).attr('aria-describedby');
        if (areaddescribedby) {
            if (validstate) {
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
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');

    //V2-563
    $(document).find('.dtpickernextseven').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true,
        minDate: '0',
    }).inputmask('mm/dd/yyyy');
    SetFixedHeadStyle();
}
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../PurchaseRequest/Index?page=Procurement_Requests";
});

function RedirectToPRequestDetail(PurchaseRequestId, mode) {
    $.ajax({
        url: "/PurchaseRequest/Details",
        type: "POST",
        dataType: "html",
        data: { PurchaseRequestId: PurchaseRequestId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchaserequest').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("prstatustext"));
            if ($(document).find('.AddPrequest').length === 0) { $(document).find('#prdetailactiondiv').css('margin-right', '0px'); }
        },
        complete: function () {
            CloseLoader();
            if (mode === "notes") {
                $('#prnotest').trigger('click');
                $('#colorselector').val('PRNotes');
            }
            if (mode === "attachment") {
                $('#prattachmentst').trigger('click');
                $('#colorselector').val('PRAttachments');
            }
            generateLineiItemdataTable(PurchaseRequestId, "");
            SetPRControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}

function RedirectToDetailOncancel(PurchaseRequestId, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToPRequestDetail(PurchaseRequestId, mode);
    });
}
//#endregion
//#region PurchaseRequestAutoGeneration
$(document).on('click', '#liAutoGenerate', function () {
    $(document).find('#_PurchaseRequestAutoGeneration').modal('show');
});
$(document).on('click', '#btnCancelPurchaseRequest', function () {
    $(document).find('#_PurchaseRequestAutoGeneration').modal('hide');
});
$(document).on('click', '#btnCreatePurchaseRequest', function () {
    $.ajax({
        url: "/PurchaseRequest/PRAutoGenerate",
        type: "POST",
        dataType: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#lblPartsSelectedCount').html(data.ItemsReviewed);
            $(document).find('#lblPurchaseRequestCreatedCount').html(data.HeadersCreated);
            $(document).find('#lblLineItemsCreatedCount').html(data.DetailsCreated);
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('hide.bs.modal', '#_PurchaseRequestAutoGeneration', function () {
    run = true;
    dtPrTable.page('first').draw('page');
});
//#endregion

//#region ColumnVisibility

$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtPrTable, true, titleArray);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0, 1, 2];
    funCustozeSaveBtn(dtPrTable, colOrder);
    run = true;
    dtPrTable.state.save(run);
    if (classNameArray != null && classNameArray.length > 0) {
        var j = 0;
        while (j < classNameArray.length) {
            dtPrTable.columns(classNameArray[j]).visible(false);
            j++;
        }
    }
});

$(document).on('keyup', '#prsearctxtbox', function (e) {
    var tagElems = $(document).find('#prsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.prsearchdrpbox', function (e) {
    $(document).find('#txtColumnSearch').val('');
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $(document).find('input[name=select_all]').prop('checked', false);
    $(document).find('.chksearch').prop('checked', false);
    $('.itemcount').text(0);
    SelectPRDetails = [];
    SelectPRId = [];
    run = true;
    var val = localStorage.getItem("PURCHASEREQUESTSTATUS");

    if ($(this).attr('id') == ConvertedToOrder || $(this).attr('id') == CanceledOrDenied || $(this).attr('id') == AllStatusValues) {
        if ($(this).attr('id') == ConvertedToOrder) {
            if (val == ConvertedToOrderToday || val == ConvertedToOrder7Days || val == ConvertedToOrder30Days || val == ConvertedToOrder60Days ||
                val == ConvertedToOrder90Days || val == ConvertedToOrderDateRange) {
                $('#cmbPROrderview').val(val).trigger('change'); //V2-523
            }
            prtempCustomdisplayId = $(this).attr('id');
            prtempsearchtitle = $(this).text();
            $(document).find('#PROrderDateRangeModal').modal('show');
            return;
        }
        else if ($(this).attr('id') == CanceledOrDenied) {
            //if (val == '17') {
            if (val == CanceledOrDeniedToday || val == CanceledOrDenied7Days || val == CanceledOrDenied30Days || val == CanceledOrDenied60Days ||
                val == CanceledOrDenied90Days || val == CanceledOrDeniedDateRange) {
                $('#cmbPRCancelDenyview').val(val).trigger('change');
            }
            prtempCustomdisplayId = $(this).attr('id');
            prtempsearchtitle = $(this).text();
            $(document).find('#PRCancelDenyDateRangeModal').modal('show');
            return;
        }

        else {
            if (val == AllStatusValues || val == AllStatusValues7Days || val == AllStatusValues30Days || val == AllStatusValues60Days ||
                val == AllStatusValues90Days || val == AllStatusValuesDateRange) {
                $('#cmbPRview').val(val).trigger('change');
            }
            prtempCustomdisplayId = $(this).attr('id');
            prtempsearchtitle = $(this).text();

            $(document).find('#PRDateRangeModal').modal('show');
            return;
        }
    }

    else {
        $('#prsearchtitle').text($(this).text());

        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('CreateStartDateVw');
        localStorage.removeItem('CreateEndDateVw');
        $(document).find('#cmbPRview').val('').trigger('change');

        CancelandDeniedStartDateVw = '';
        CancelandDeniedEndDateVw = '';
        localStorage.removeItem('CancelandDeniedStartDateVw');
        localStorage.removeItem('CancelandDeniedEndDateVw');
        $(document).find('#cmbPRCancelDenyview').val('').trigger('change');

        ProcessedStartDateVw = '';
        ProcessedEndDateVw = '';
        localStorage.removeItem('ProcessedStartDateVw');
        localStorage.removeItem('ProcessedEndDateVw');
        $(document).find('#cmbPROrderview').val('').trigger('change');
    }

    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("PURCHASEREQUESTSTATUS", optionval);
    CustomQueryDisplayId = optionval;
    ShowbtnLoaderclass("LoaderDrop");
    PRAdvSearch();
    dtPrTable.page('first').draw('page');
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
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        dtPrTable.page('first').draw('page');
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
        data: { tableName: 'PurchaseRequest' },
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
        data: { tableName: 'PurchaseRequest', searchText: txtSearchval, isClear: isClear },
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
                dtPrTable.page('first').draw('page');
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

$(document).on('change', '#cmbPRview', function (e) {
    var thielement = $(this);
    //if (thielement.val() == '10') {
    if (thielement.val() == AllStatusValuesDateRange) {
        CreateStartDateVw = today;
        CreateEndDateVw = today;
        var strtlocal = localStorage.getItem('CreateStartDateVw');
        if (strtlocal) {
            CreateStartDateVw = strtlocal;
        }
        else {
            CreateStartDateVw = today;
        }
        var endlocal = localStorage.getItem('CreateEndDateVw');
        if (endlocal) {
            CreateEndDateVw = endlocal;
        }
        else {
            CreateEndDateVw = today;
        }
        $(document).find('#timeperiodcontainer').show();
        $(document).find('#PRdaterange').daterangepicker({
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
        localStorage.removeItem('CreateStartDateVw');
        localStorage.removeItem('CreateEndDateVw');
        $(document).find('#timeperiodcontainer').hide();
    }
});
$(document).on('click', '#btntimeperiod', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbPRview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = prtempCustomdisplayId;
    if (daterangeval != AllStatusValuesDateRange) {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('CreateStartDateVw');
        localStorage.removeItem('CreateEndDateVw');
    }
    else {
        localStorage.setItem('CreateStartDateVw', CreateStartDateVw);
        localStorage.setItem('CreateEndDateVw', CreateEndDateVw);

        ProcessedStartDateVw = '';
        ProcessedEndDateVw = '';
        localStorage.removeItem('ProcessedStartDateVw');
        localStorage.removeItem('ProcessedEndDateVw');
        $(document).find('#cmbPROrderview').val('').trigger('change');

        CancelandDeniedStartDateVw = '';
        CancelandDeniedEndDateVw = '';
        localStorage.removeItem('CancelandDeniedStartDateVw');
        localStorage.removeItem('CancelandDeniedEndDateVw');
        $(document).find('#cmbPRCancelDenyview').val('').trigger('change');

    }
    $(document).find('#PRDateRangeModal').modal('hide');
    var text = prtempsearchtitle;
    if (daterangeval === AllStatusValuesDateRange)
        text = text + " - " + $('#PRdaterange').val();
    else
        text = text + " - " + $(document).find('#cmbPRview option[value=' + daterangeval + ']').text();
    $('#prsearchtitle').text(text);
    if (daterangeval.length !== 0) {
        $(".searchList li").removeClass("activeState");
        $('#prsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId) {
                $(this).addClass('activeState');
            }
        });
        CustomQueryDisplayId = daterangeval;
        $(document).find('#searcharea').hide("slide");
        localStorage.setItem("PURCHASEREQUESTSTATUS", CustomQueryDisplayId);
        ShowbtnLoaderclass("LoaderDrop");
        PRAdvSearch();
        dtPrTable.page('first').draw('page');
    }
});

$(document).on('change', '#cmbPROrderview', function (e) {
    var thielement = $(this);
    if (thielement.val() == ConvertedToOrderDateRange) {
        ProcessedStartDateVw = today;
        ProcessedEndDateVw = today;
        var strtlocal = localStorage.getItem('ProcessedStartDateVw');
        if (strtlocal) {
            ProcessedStartDateVw = strtlocal;
        }
        else {
            ProcessedStartDateVw = today;
        }
        var endlocal = localStorage.getItem('ProcessedEndDateVw');
        if (endlocal) {
            ProcessedEndDateVw = endlocal;
        }
        else {
            ProcessedEndDateVw = today;
        }
        $(document).find('#timeOrderperiodcontainer').show();
        $(document).find('#PROrderdaterange').daterangepicker({
            format: 'MM/DD/YYYY',
            startDate: ProcessedStartDateVw,
            endDate: ProcessedEndDateVw,
            "locale": {
                "applyLabel": getResourceValue("JsApply"),
                "cancelLabel": getResourceValue("CancelAlert")
            }
        }, function (start, end, label) {
            ProcessedStartDateVw = start.format('MM/DD/YYYY');
            ProcessedEndDateVw = end.format('MM/DD/YYYY');
        });
    }
    else {
        $(document).find('#timeOrderperiodcontainer').hide();
    }
});
$(document).on('click', '#btnOrdertimeperiod', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbPROrderview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = prtempCustomdisplayId;
    if (daterangeval != ConvertedToOrderDateRange) {
        ProcessedStartDateVw = '';
        ProcessedEndDateVw = '';
        localStorage.removeItem('ProcessedStartDateVw');
        localStorage.removeItem('ProcessedEndDateVw');
    }
    else {
        localStorage.setItem('ProcessedStartDateVw', ProcessedStartDateVw);
        localStorage.setItem('ProcessedEndDateVw', ProcessedEndDateVw);


        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('CreateStartDateVw');
        localStorage.removeItem('CreateEndDateVw');
        $(document).find('#cmbPRview').val('').trigger('change');

        CancelandDeniedStartDateVw = '';
        CancelandDeniedEndDateVw = '';
        localStorage.removeItem('CancelandDeniedStartDateVw');
        localStorage.removeItem('CancelandDeniedEndDateVw');
        $(document).find('#cmbPRCancelDenyview').val('').trigger('change');
    }
    $(document).find('#PROrderDateRangeModal').modal('hide');
    var text = prtempsearchtitle;
    if (daterangeval === ConvertedToOrderDateRange)
        text = text + " - " + $('#PROrderdaterange').val();
    else
        text = text + " - " + $(document).find('#cmbPROrderview option[value=' + daterangeval + ']').text();
    $('#prsearchtitle').text(text);
    if (daterangeval.length !== 0) {
        $(".searchList li").removeClass("activeState");
        $('#prsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId) {
                $(this).addClass('activeState');
            }
        });
        $(document).find('#searcharea').hide("slide");
        CustomQueryDisplayId = daterangeval;//V2-523
        localStorage.setItem("PURCHASEREQUESTSTATUS", CustomQueryDisplayId);
        ShowbtnLoaderclass("LoaderDrop");
        typeValStatus = $("#Status").val();
        typeValVendor = $("#Vendor").val();
        PRAdvSearch();
        dtPrTable.page('first').draw('page');
    }
});

$(document).on('change', '#cmbPRCancelDenyview', function (e) {
    var thielement = $(this);
    if (thielement.val() == CanceledOrDeniedDateRange) {
        CancelandDeniedStartDateVw = today;
        CancelandDeniedEndDateVw = today;
        var strtlocal = localStorage.getItem('CancelandDeniedStartDateVw');
        if (strtlocal) {
            CancelandDeniedStartDateVw = strtlocal;
        }
        else {
            CancelandDeniedStartDateVw = today;
        }
        var endlocal = localStorage.getItem('CancelandDeniedEndDateVw');
        if (endlocal) {
            CancelandDeniedEndDateVw = endlocal;
        }
        else {
            CancelandDeniedEndDateVw = today;
        }
        $(document).find('#timeCancelDenyperiodcontainer').show();
        $(document).find('#PRCancelDenydaterange').daterangepicker({
            format: 'MM/DD/YYYY',
            startDate: CancelandDeniedStartDateVw,
            endDate: CancelandDeniedEndDateVw,
            "locale": {
                "applyLabel": getResourceValue("JsApply"),
                "cancelLabel": getResourceValue("CancelAlert")
            }
        }, function (start, end, label) {
            CancelandDeniedStartDateVw = start.format('MM/DD/YYYY');
            CancelandDeniedEndDateVw = end.format('MM/DD/YYYY');
        });
    }
    else {
        $(document).find('#timeCancelDenyperiodcontainer').hide();
    }
});
$(document).on('click', '#btnCancelDenytimeperiod', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbPRCancelDenyview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = prtempCustomdisplayId;
    if (daterangeval != CanceledOrDeniedDateRange) {
        CancelandDeniedStartDateVw = '';
        CancelandDeniedEndDateVw = '';
        localStorage.removeItem('CancelandDeniedStartDateVw');
        localStorage.removeItem('CancelandDeniedEndDateVw');
    }
    else {
        localStorage.setItem('CancelandDeniedStartDateVw', CancelandDeniedStartDateVw);
        localStorage.setItem('CancelandDeniedEndDateVw', CancelandDeniedEndDateVw);

        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('CreateStartDateVw');
        localStorage.removeItem('CreateEndDateVw');
        $(document).find('#cmbPRview').val('').trigger('change');

        ProcessedStartDateVw = '';
        ProcessedEndDateVw = '';
        localStorage.removeItem('ProcessedStartDateVw');
        localStorage.removeItem('ProcessedEndDateVw');
        $(document).find('#cmbPROrderview').val('').trigger('change');
    }
    $(document).find('#PRCancelDenyDateRangeModal').modal('hide');
    var text = prtempsearchtitle;
    if (daterangeval === CanceledOrDeniedDateRange)
        text = text + " - " + $('#PRCancelDenydaterange').val();
    else
        text = text + " - " + $(document).find('#cmbPRCancelDenyview option[value=' + daterangeval + ']').text();
    $('#prsearchtitle').text(text);
    if (daterangeval.length !== 0) {
        $(".searchList li").removeClass("activeState");
        $('#prsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId) {
                $(this).addClass('activeState');
            }
        });
        $(document).find('#searcharea').hide("slide");
        CustomQueryDisplayId = daterangeval;//V2-523
        localStorage.setItem("PURCHASEREQUESTSTATUS", CustomQueryDisplayId);
        ShowbtnLoaderclass("LoaderDrop");
        typeValStatus = $("#Status").val();
        typeValVendor = $("#Vendor").val();
        PRAdvSearch();
        dtPrTable.page('first').draw('page');
    }
});


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

            if (item.key == 'advcreatedaterange' && item.value !== '') {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    searchitemhtml = searchitemhtml + '<span style="display:none;" class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
            if (item.key == 'advprocessdedbydaterange' && item.value !== '') {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    searchitemhtml = searchitemhtml + '<span style="display:none;" class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }

            if (item.key == 'Created') {
                $("#Created").trigger('change.select2');
            }
            if (item.key == 'DateProcessed') {
                $("#DateProcessed").trigger('change.select2');
            }

            if (item.key == 'Vendor') {
                $("#Vendor").trigger('change.select2');
            }

            if (item.key == 'Status') {
                $("#Status").trigger('change.select2');
            }

            advcountercontainer.text(selectCount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);

}
function GetCreateDateRangeFilterData() {
    var CreatedDateval = $("#Created").val();
    if (CreatedDateval == 10) {
        $('#Createtimeperiodcontainer').show();
    }
    else {
        $('#Createtimeperiodcontainer').hide();
    }
    switch (CreatedDateval) {
        case '2':
            $('#Createtimeperiodcontainer').hide();
            StartCreateteDate = today;
            EndCreateteDate = today;
            break;
        case '3':
            $('#Createtimeperiodcontainer').hide();
            StartCreateteDate = PreviousDateByDay(7);
            EndCreateteDate = today;
            break;
        case '4':
            $('#Createtimeperiodcontainer').hide();
            StartCreateteDate = PreviousDateByDay(30);
            EndCreateteDate = today;
            break;
        case '5':
            $('#Createtimeperiodcontainer').hide();
            StartCreateteDate = PreviousDateByDay(60);
            EndCreateteDate = today;
            break;
        case '6':
            $('#Createtimeperiodcontainer').hide();
            StartCreateteDate = PreviousDateByDay(90);
            EndCreateteDate = today;
            break;
        case '10':
            $('#Createtimeperiodcontainer').show()
            var advCreateDateRange = $("#advcreatedaterange").val();
            var advCreateDateRangeArr = advCreateDateRange.split("-");
            StartCreateteDate = LRTrim(advCreateDateRangeArr[0]);
            EndCreateteDate = LRTrim(advCreateDateRangeArr[1]);
            $('#advcreatedaterange').val(StartCreateteDate + ' - ' + EndCreateteDate);
            $(document).find('#advcreatedaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                StartCreateteDate = start.format('MM/DD/YYYY');
                EndCreateteDate = end.format('MM/DD/YYYY');
            });
            break;
        default:
            $('#Createtimeperiodcontainer').hide();
            $(document).find('#advcreatedaterange').daterangepicker({
                format: 'MM/DD/YYYY'
            });
            StartCreateteDate = '';
            EndCreateteDate = '';
            break;
    }
}

function GetProcessedDateRangeFilterData() {
    var DateProcessedDateval = $("#DateProcessed").val();
    if (DateProcessedDateval == 10) {
        $('#processedbytimeperiodcontainer').show();
    }
    else {
        $('#processedbytimeperiodcontainer').hide();
    }
    switch (DateProcessedDateval) {
        case '2':
            $('#processedbytimeperiodcontainer').hide();
            StartProcessedByDate = today;
            EndProcessedByDate = today;
            break;
        case '3':
            $('#processedbytimeperiodcontainer').hide();
            StartProcessedByDate = PreviousDateByDay(7);
            EndProcessedByDate = today;
            break;
        case '4':
            $('#processedbytimeperiodcontainer').hide();
            StartProcessedByDate = PreviousDateByDay(30);
            EndProcessedByDate = today;
            break;
        case '5':
            $('#processedbytimeperiodcontainer').hide();
            StartProcessedByDate = PreviousDateByDay(60);
            EndProcessedByDate = today;
            break;
        case '6':
            $('#processedbytimeperiodcontainer').hide();
            StartProcessedByDate = PreviousDateByDay(90);
            EndProcessedByDate = today;
            break;
        case '10':
            $('#processedbytimeperiodcontainer').show();
            var advprocessdedDateRange = $("#advprocessdedbydaterange").val();
            var advprocessdedDateRangeArr = advprocessdedDateRange.split("-");
            StartProcessedByDate = LRTrim(advprocessdedDateRangeArr[0]);
            EndProcessedByDate = LRTrim(advprocessdedDateRangeArr[1]);
            $('#advprocessdedbydaterange').val(StartProcessedByDate + ' - ' + EndProcessedByDate);
            $(document).find('#advprocessdedbydaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                StartProcessedByDate = start.format('MM/DD/YYYY');
                EndProcessedByDate = end.format('MM/DD/YYYY');
            });
            break;
        default:
            $('#processedbytimeperiodcontainer').hide();
            $(document).find('#advprocessdedbydaterange ').daterangepicker({
                format: 'MM/DD/YYYY'
            });
            StartProcessedByDate = '';
            EndProcessedByDate = '';
            break;
    }
}

//#endregion
//#region V2 667
var ChargeTypeCancelAlertSetting = {
    title: getResourceValue("CancelAlertSure"),
    text: getResourceValue("ChargeItemNotWorkOrderContinueAlert"),
    type: "warning",
    showCancelButton: true,
    confirmButtonClass: "btn-sm btn-primary",
    cancelButtonClass: "btn-sm",
    confirmButtonText: getResourceValue("CancelAlertYes"),
    cancelButtonText: getResourceValue("CancelAlertNo")
};
$(document).on('click', '#btnPRPartNotInventorySave', function (e) {
    var form = $(document).find('#frmPRPartNotInInventoryId');
    if (form.valid() === false) {
        return;
    }
    e.preventDefault();
    var ChargeType = $(document).find('#PartNotInInventoryModel_ChargeType').val();
    if (ChargeType != 'WorkOrder') {


        swal(ChargeTypeCancelAlertSetting, function (isConfirm) {
            if (isConfirm == true) {
                $(document).find('#frmPRPartNotInInventoryId').submit();
            }
            else {
                return false;
            }
        });
    }
    else {
        $(document).find('#frmPRPartNotInInventoryId').submit();
    }
});
$(document).on('click', '#btnPRPartNotInventorySaveDynamic', function (e) {
    var form = $(document).find('#frmPRPartNotInInventoryDynamic');
    if (form.valid() === false) {
        return;
    }
    e.preventDefault();
    var ChargeType = $(document).find('#ChargeType').val();
    if (ChargeType != 'WorkOrder') {


        ChargeTypeCancelAlertSetting.closeOnConfirm = false;
        swal(ChargeTypeCancelAlertSetting, function (isConfirm) {
            if (isConfirm == true) {
                $(document).find('#frmPRPartNotInInventoryDynamic').submit();
            }
            else {
                ChargeTypeCancelAlertSetting.closeOnConfirm = true;
                return false;
            }
        });
        ChargeTypeCancelAlertSetting.closeOnConfirm = true;
    }
    else {
        $(document).find('#frmPRPartNotInInventoryDynamic').submit();
    }
});
//
$(document).on('click', '#btnPREditLineItemSaveDynamic', function (e) {
    if ($(document).find('#ChargeType').length > 0) {
        // this code will be executed only for Part not in inventory 
        // but will not be executed for Part in inventory
        var form = $(document).find('#frmPRLineItemEditDynamic');
        if (form.valid() === false) {
            return;
        }
        e.preventDefault();
        var ChargeType = $(document).find('#ChargeType').val();
        if (ChargeType != 'WorkOrder') {
            ChargeTypeCancelAlertSetting.closeOnConfirm = false;
            swal(ChargeTypeCancelAlertSetting, function (isConfirm) {
                if (isConfirm == true) {
                    $(document).find('#frmPRLineItemEditDynamic').submit();
                }
                else {
                    ChargeTypeCancelAlertSetting.closeOnConfirm = true;
                    return false;
                }
            });
            ChargeTypeCancelAlertSetting.closeOnConfirm = true;
        }
        else {
            $(document).find('#frmPRLineItemEditDynamic').submit();
        }
    }
});
//#endregion
//#region V2-853 Reset Grid
$('#liResetGridClearBtn').click(function (e) {
    CancelAlertSetting.text = getResourceValue("ResetGridAlertMessage");
    swal(CancelAlertSetting, function () {
        var localstorageKeys = [];
        localstorageKeys.push("prstatustext");
        localstorageKeys.push("ProcessedStartDateVw");
        localstorageKeys.push("ProcessedEndDateVw");
        localstorageKeys.push("CreateStartDateVw");
        localstorageKeys.push("CreateEndDateVw");
        localstorageKeys.push("CancelandDeniedStartDateVw");
        localstorageKeys.push("CancelandDeniedEndDateVw");
        localstorageKeys.push("PURCHASEREQUESTSTATUS");
        DeleteGridLayout('PurchaseRequest_Search', dtPrTable, localstorageKeys);
        GenerateSearchList('', true);
        window.location.href = "../PurchaseRequest/Index?page=Procurement_Requests";
    });
});
//#endregion

//#region V2-945
$(document).on('click', '#PrintPRCheckListDevExpress', function () {
    if (SelectPRDetails.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var jsonResult = {
            "list": SelectPRDetails
        }
        {
            $.ajax({
                url: '/PurchaseRequest/SetPrintPRListFromIndex',
                data: JSON.stringify(jsonResult),
                type: "POST",
                datatype: "json",
                contentType: 'application/json; charset=utf-8',
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (result) {
                    if (result.success) {
                        window.open("/PurchaseRequest/GeneratePurchaseRequestPrint", "_blank");
                    }
                },
                complete: function () {
                    CloseLoader();
                    $(".updateArea").hide();
                    $(".actionBar").fadeIn();
                    $(document).find('.dt-body-center').find('#purchaserequest-select-all').prop('checked', false);
                    $(document).find('.chkPRsearch').prop('checked', false);
                    $('.itemcount').text(0);
                    SelectPRDetails = [];
                    SelectPRId = [];
                }
            });
        }
    }
});

$(document).on('click', '#printPR', function () {
    var PRArray = [];
    var PurchaseRequestId = $(document).find('#purchaseRequestModel_PurchaseRequestId').val();
    var ClientLookupId = $(document).find('#purchaseRequestModel_ClientLookupId').val();
    var Status = $(document).find('#purchaseRequestModel_Status').val();
    var ChildCount = $(document).find('#purchaseRequestModel_CountLineItem').val();
    var VendorIsExternal = $(document).find('#purchaseRequestModel_VendorIsExternal').val();
    var VendorId = $(document).find('#purchaseRequestModel_VendorId').val();
    var item = new PurchaseRequestNotInSelectedItem(PurchaseRequestId, ClientLookupId, Status, ChildCount, VendorId, VendorIsExternal);
    PRArray.push(item);
    var jsonResult = {
        "list": PRArray
    };
    $.ajax({
        url: '/PurchaseRequest/SetPrintPRListFromIndex',
        data: JSON.stringify(jsonResult),
        type: "POST",
        datatype: "json",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (result) {
            if (result.success) {
                window.open("/PurchaseRequest/GeneratePurchaseRequestPrint", "_blank");
            }
        },
        complete: function () {
            CloseLoader();
            PRArray = [];
        }
    });
});
//#endregion
//#region V2 - 1032
var lineitemlookupListPRTable;
var Action;
//#region Add
function AddPartInInventorySingleStockLineItemDynamic(PurchaseRequestId, PurchaseRequestLineItemId, Status) {
    var ClientLookupId = $(document).find('#purchaseRequestModel_ClientLookupId').val();
    var vendorId = $(document).find('#purchaseRequestModel_VendorId').val();
    var StoreroomId = $(document).find('#purchaseRequestModel_StoreroomId').val();
    $.ajax({
        url: "/PurchaseRequest/AddPartInInventorySingleStockLineItemDynamic",
        type: "POST",
        dataType: "html",
        data: { PurchaseRequestId: PurchaseRequestId, ClientLookupId: ClientLookupId, vendorId: vendorId, StoreroomId: StoreroomId, LineItemId: PurchaseRequestLineItemId, Status: Status },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchaserequest').html(data);
        },
        complete: function () {
            CloseLoader();
            SetPRControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '#btnPRPartInventorySingleStockSaveDynamic', function (e) {
    var form = $(document).find('#frmPRPartInInventorySingleStockLineItemDynamic');
    if (form.valid() === false) {
        return;
    }
    e.preventDefault();
    var currentPartId = $(document).find('#PartId').val();
    var currentPartClientLookupId = $(document).find('#PartClientLookupId').val();
    var currentPartDesc = $(document).find('#PartClientLookupId').text();
    var isOnOrderCheck = $(document).find('#AddPRLineItemPartInInventory_IsOnOderCheck').val();
    if (isOnOrderCheck == "True") {
        generateChecklineitemlookupListPRTable(currentPartClientLookupId, currentPartDesc, currentPartId, Action = "Save")
    }
    else {
        $(document).find('#frmPRPartInInventorySingleStockLineItemDynamic').submit();
    }

});
function AddPartInSingleStockOnSuccess(data) {
    CloseLoader();
    var PurchaseRequestId = data.purchaserequestid;
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("LineItemAddedAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPRequestDetail(PurchaseRequestId, "PurchasingOverview");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region Edit
$(document).on('click', '#btnPREditLineItemSaveSingleStockDynamic', function (e) {
    var form = $(document).find('#frmPRLineItemEditDynamic');
    if (form.valid() === false) {
        return;
    }
    e.preventDefault();
    var currentPartId = $(document).find('#PartId').val();
    var currentPartClientLookupId = $(document).find('#PartClientLookupId').val();
    var currentPartDesc = $(document).find('#PartClientLookupId').text();
    var isOnOrderCheck = $(document).find('#EditPRLineItemPartInInventorySingleStock_IsOnOderCheck').val();
    if (isOnOrderCheck == "True") {
        generateChecklineitemlookupListPRTable(currentPartClientLookupId, currentPartDesc, currentPartId, "Update")
    }
    else
        $(document).find('#frmPRLineItemEditDynamic').submit();
});

//#endregion
function generateChecklineitemlookupListPRTable(PartClientLookupId, PartDesc, PartId, Action) {
    if ($(document).find('#LineitemlookupListPRTable').hasClass('dataTable')) {
        lineitemlookupListPRTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    lineitemlookupListPRTable = $("#LineitemlookupListPRTable").DataTable({
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        "pageLength": 10,
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/PartLookup/GetLineitemLookuplist",
            data: function (d) {
                d.PartId = PartId;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                return json.data;
            }
        },
        "columns":
            [
                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center" },
                { "data": "LineNumber", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center" },
                { "data": "OrderQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center" },
                {
                    "data": "UnitofMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center"
                },
                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center" },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center" }
            ],
        "rowCallback": function (row, data, index, full) {

        },
        initComplete: function () {
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (rCount > 0) {
                /* if (!$(document).find('#LineitemlookupListPRModal').hasClass('show')) {*/
                $(document).find('#popLineitemlookupListPRTable').text("");
                var ExistingPRPOForPartMessage = getResourceValue("globalExistingPRPOForPart");
                $(document).find('#popLineitemlookupListPRTable').text(ExistingPRPOForPartMessage + PartClientLookupId + " - " + PartDesc)
                $(document).find('#LineitemlookupListPRModal').modal({ backdrop: 'static', keyboard: false, show: true });
                /* }*/
            }
            else {
                if (Action == "Save")
                    $(document).find('#frmPRPartInInventorySingleStockLineItemDynamic').submit();
                else
                    $(document).find('#frmPRLineItemEditDynamic').submit();

            }
        }
    });
}

$(document).on('click', '#addPartfromLineItemPRPopUp', function () {
    var formid = $(document).find('form')[0].id;
    $(document).find('#LineitemlookupListPRModal').modal('hide');
    if (formid == "frmPRPartInInventorySingleStockLineItemDynamic")
        $(document).find('#frmPRPartInInventorySingleStockLineItemDynamic').submit();
    else
        $(document).find('#frmPRLineItemEditDynamic').submit();

});
$(document).on('click', '#btncancelfromLineItemPopUp', function () {
    $(document).find('#LineitemlookupListPRModal').modal('hide');
})
//#endregion

//#region V2-1124 setting description, issueunit, appliedcost according to the selected part
$(document).on('click', '.link_xrefpart_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtXrefPartsTable.row(row).data();
    $(document).find('#Description').val(data.Description);
    $(document).find('#UnitCost').val(data.AppliedCost);
    $(document).find('#UnitofMeasure').val(data.IssueUnit).trigger('change');
});
//#endregion