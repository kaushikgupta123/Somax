//#region Common
var run = false;
var dtTable;
var CustomQueryDisplayId = 0;
var SelectPOId = [];
var selectCount = 0;
var gridname = "PurchaseOrder_Search";
var CompleteStartDateVw = '';
var CompleteEndDateVw = '';
var StartCreateteDate = '';
var EndCreateteDate = '';
var StartCompleteDate = '';
var EndCompleteDate = '';
var CreateStartDateVw = '';
var CreateEndDateVw = '';
var today = $.datepicker.formatDate('mm/dd/yy', new Date());
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
var AllStatusValues = '0',
    AllStatusValuesToday = '11',
    AllStatusValues7Days = '12',
    AllStatusValues30Days = '13',
    AllStatusValues60Days = '14',
    AllStatusValues90Days = '15',
    AllStatusValuesDateRange = '16';
var Complete = '6',
    CompleteToday = '2',
    Complete7Days = '3',
    Complete30Days = '4',
    Complete60Days = '5',
    //Complete90Days = '6',
    CompleteDateRange = '10';
//#endregion

$(document).ready(function () {
    $('#comptimeperiodcontainer').hide();

    ShowbtnLoaderclass("LoaderDrop");
    ShowbtnLoader("btnsortmenu");
    $(document).find('.select2picker').select2({});
    $(document).on('click', "#action", function () {
        $(".actionDrop").slideToggle();
    });
    $(document).on('focusout', "#action", function () {
        $(".actionDrop").fadeOut();
    });
    $(".tabsArea").hide();
    $("ul.vtabs li:first").addClass("active").show();
    $(".tabsArea:first").show();
    $(".actionBar").fadeIn();
    $("#POGridAction :input").attr("disabled", "disabled");
    $(document).on('click', "ul.vtabs li", function () {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        $(".tabsArea").not('#PurchaseOrderLineItem').hide();
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
    $(document).on('click', '#poOverview', function () {
        $('#PurchasingOverview').show();
        $('#Equipmenttab').show();
        $('.tabcontent').show();
        $('#btndetails').addClass('active');
    });
    $(document).on('change', '#colorselector', function (evt) {
        $(".tabsArea").not('#PurchaseOrderLineItem').hide();
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
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    switch (cityName) {
        case "PONotes":
            GeneratePONotesGrid();
            break;
        case "POEventLog":
            GenerateSJEventLogGrid();
            break;
        case "POAttachments":
            GeneratePOAttachmentsGrid();
            break;
        case "POverview":
            $('#PurchasingOverview').show();
            $('#Detailstab').show();
            $('#btndetails').addClass('active')
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
    var val = localStorage.getItem("PURCHASEORDERSTATUS");
    if (val && val !== "0") {
        CustomQueryDisplayId = val;
        //V2-427
        var text = "";
        if (val == CompleteToday || val == Complete7Days || val == Complete30Days || val == Complete60Days || val == Complete || val == CompleteDateRange) {
            $('#cmbcompleteview').val(val).trigger('change');
            text = $('#posearchListul').find('li').eq(2).text();
            $('#posearchtitle').text(text);
            $("#posearchListul li").removeClass("activeState");
            $("#posearchListul li").eq(2).addClass('activeState');


        }
        //364
        if (val == AllStatusValuesToday || val == AllStatusValues7Days || val == AllStatusValues30Days || val == AllStatusValues60Days
            || val == AllStatusValues90Days || val == AllStatusValuesDateRange) {
            $('#cmbcreateview').val(val).trigger('change');
            text = $('#posearchListul').find('li').eq(0).text();
            $('#posearchtitle').text(text);
            $("#posearchListul li").removeClass("activeState");
            $("#posearchListul li").eq(0).addClass('activeState');
        }
        //364
        else {
            $('#posearchListul li').each(function (index, value) {
                if ($(this).attr('id') == CustomQueryDisplayId && $(this).attr('id') != '0') {
                    $('#posearchtitle').text($(this).text());
                    $(this).addClass('activeState');
                }
            });
        }
        //V2-427

        //364
        if ($('#posearchListul').find('.activeState').attr('id') == '0') {
            if ($(document).find('#cmbcreateview').val() != AllStatusValuesDateRange)
                text = text + " - " + $(document).find('#cmbcreateview option[value=' + val + ']').text();
            else
                text = text + " - " + $('#createdaterange').val();
            $('#posearchtitle').text(text);
        }
        //364

        //v2 371 Point 3
        //-------------------------------------------------------        
        if ($('#posearchListul').find('.activeState').attr('id') == '6') {
            if ($(document).find('#cmbcompleteview').val() != CompleteDateRange)
                text = text + " - " + $(document).find('#cmbcompleteview option[value=' + val + ']').text();
            else
                text = text + " - " + $('#completedaterange').val();
            $('#posearchtitle').text(text);
        }
        //-------------------------------------------------------

        //generatePODataTable();
    }
    else {

        $('#posearchListul li').each(function (index, value) {
            if ($(this).attr('id') == 1) {
                CustomQueryDisplayId = $(this).attr('id');
                $('#posearchtitle').text($(this).text());
                $("#posearchListul li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });

    }
    var IsDetailFromNotification = $("#IsDetailFromNotification").val();
    var IsRedirectFromPart = $("#IsRedirectFromPart").val();
    var PoId = $("#POId").val();
    if (IsRedirectFromPart == "True") {
        localStorage.setItem("PURCHASEORDERSTATUS", '1');
        localStorage.setItem("postatustext", getResourceValue("OpenAlert"));
        RedirectToPODetail(PoId, "overview");

    } 
   else if (IsDetailFromNotification == "True") {
        localStorage.setItem("PURCHASEORDERSTATUS", '6');
        localStorage.setItem("postatustext", getResourceValue("spnCompletedLast90Days"));
        RedirectToPODetail(PoId, "overview");
    }
    else {
        generatePODataTable();
    }


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
    });
});
$(document).on('click', '#PurchasingOrderSidebar', function () {
    $('#PurchasingOverview').show();
    $('#Detailstab').show();
    $('#btndetails').addClass('active')
});

//#region Search
$(document).on('change', '#PurchaseOrderModel_CustomFilter', function () {
    ShowbtnLoaderclass("LoaderDrop");
    run = true;
    var optionval = $('#PurchaseOrderModel_CustomFilter option:selected').val();
    CustomQueryDisplayId = optionval;
    localStorage.setItem("PURCHASEORDERSTATUS", CustomQueryDisplayId);
    dtTable.page('first').draw('page');
});
$("#btnPODataAdvSrch").on('click', function (e) {
    run = true;
    searchresult = [];
    PRAdvSearch();
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    dtTable.page('first').draw('page');
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    if (searchtxtId == "Status") {
        $(document).find("#Status").val("").trigger('change.select2');
    }

    if (searchtxtId == "CompleteDate") {
        $(document).find("#CompleteDate").val("").trigger('change.select2');
    }
    PRAdvSearch(dtTable.length);
    dtTable.page('first').draw('page');
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', '#liPrint,#liPdf', function () {
    //$(".buttons-pdf")[0].click();
    var thisid = $(this).attr('id');
    var TableHaederProp = [];
    function table(property, title) {
        this.property = property;
        this.title = title;
    }
    $("#PurchaseOrderSearch thead tr th").map(function (key) {
        var thisdiv = $(this).find('div');
        if ($(this).parents('.purchaseOrderinnerDataTable').length == 0 && thisdiv.html()) {
            var tablearr = new table(this.getAttribute('data-th-prop'), thisdiv.html());
            TableHaederProp.push(tablearr);
        }
    });
    var params = {
        tableHaederProps: TableHaederProp,
        colname: order,
        coldir: orderDir,
        CustomQueryDisplayId: CustomQueryDisplayId,
        CompleteStartDateVw: ValidateDate(CompleteStartDateVw),
        CompleteEndDateVw: ValidateDate(CompleteEndDateVw),
        CreateStartDateVw: ValidateDate(CreateStartDateVw),
        CreateEndDateVw: ValidateDate(CreateEndDateVw),
        PurchaseOrder: LRTrim($("#PurchaseOrder").val()),
        Status: LRTrim($("#Status").val()),
        VendorClientLookupId: LRTrim($("#VendorClientLookupId").val()),
        VendorName: LRTrim($('#VendorName').val()),
        StartCreateDate: ValidateDate(StartCreateteDate),
        EndCreateDate: ValidateDate(EndCreateteDate),
        Attention: LRTrim($("#Attention").val()),
        VendorPhoneNumber: LRTrim($("#VendorPhoneNumber").val()),
        StartCompleteDate: ValidateDate(StartCompleteDate),
        EndCompleteDate: ValidateDate(EndCompleteDate),
        Reason: LRTrim($("#Reason").val()),
        Buyer_PersonnelName: LRTrim($("#BuyerPersonnelName").val()),
        TotalCost: LRTrim($("#TotalCost").val()),
        FilterValue: "0",
        txtSearchval: LRTrim($(document).find('#txtColumnSearch').val()),
        Required: LRTrim($(document).find('#Required').val())
    };
    pOPrintParams = JSON.stringify({ 'pOPrintParams': params });
    $.ajax({
        "url": "/Purchasing/SetPrintData",
        "data": pOPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            if (thisid == 'liPdf') {
                window.open('/Purchasing/ExportASPDF?d=d', '_self');
            }
            else if (thisid == 'liPrint') {
                window.open('/Purchasing/ExportASPDF', '_blank');
            }

            return;
        }
    });
    $('#mask').trigger('click');
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    $('#mask').trigger('click');
});
$(document).on('change', '#CreateDate', function () {
    var thisval = $(this).val();
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
$(document).on('change', '#CompleteDate', function () {
    var thisval = $(this).val();
    if (thisval == 10) {
        $('#comptimeperiodcontainer').show();
    }
    else {
        $('#comptimeperiodcontainer').hide();
    }
    switch (thisval) {
        case '2':
            $('#comptimeperiodcontainer').hide();
            StartCompleteDate = today;
            EndCompleteDate = today;
            break;
        case '3':
            $('#comptimeperiodcontainer').hide();
            StartCompleteDate = PreviousDateByDay(7);
            EndCompleteDate = today;
            break;
        case '4':
            $('#comptimeperiodcontainer').hide();
            StartCompleteDate = PreviousDateByDay(30);
            EndCompleteDate = today;
            break;
        case '5':
            $('#comptimeperiodcontainer').hide();
            StartCompleteDate = PreviousDateByDay(60);
            EndCompleteDate = today;
            break;
        case '6':
            $('#comptimeperiodcontainer').hide();
            StartCompleteDate = PreviousDateByDay(90);
            EndCompleteDate = today;
            break;
        case '10':
            $('#comptimeperiodcontainer').show();
            StartCompleteDate = today;
            EndCompleteDate = today;
            $('#advcompletedaterange').val(StartCompleteDate + ' - ' + EndCompleteDate);
            $(document).find('#advcompletedaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                StartCompleteDate = start.format('MM/DD/YYYY');
                EndCompleteDate = end.format('MM/DD/YYYY');
            });
            break;
        default:
            $('#comptimeperiodcontainer').hide();
            $(document).find('#advcompletedaterange').daterangepicker({
                format: 'MM/DD/YYYY'
            });
            StartCompleteDate = '';
            EndCompleteDate = '';
            break;

    }
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
        if ($(this).attr('id') != 'advcompletedaterange') {
            if ($(this).val()) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }
        else {
            if ($(this).val()) {
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }

    });
    if (status != 0) {
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
        $('#_advcompletedaterange').hide();
    }
    else {
        var statusDropdown = $("Status");

    }
    $(".filteritemcount").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}
function clearAdvanceSearch() {
    selectCount = 0;
    $("#PurchaseOrder").val("");
    $("#Status").val("").trigger('change');
    $('#VendorClientLookupId').val("");
    $("#VendorName").val("");
    $("#CreateDate").val("").trigger('change');
    StartCreateteDate = '';
    EndCreateteDate = '';
    $("#Attention").val("");
    $("#VendorPhoneNumber").val("");
    $("#CompleteDate").val("").trigger('change.select2');
    StartCompleteDate = '';
    EndCompleteDate = '';
    $("#Reason").val("");
    $("#BuyerPersonnelName").val("");
    $("#TotalCost").val("");
    $("#Required").val("");
    $("#advsearchfilteritems").html('');
    $(".filteritemcount").text(selectCount);
}
var titleArray = [];
var classNameArray = [];
var order = '2';//PO Sorting
var orderDir = 'asc';//PO Sorting
function generatePODataTable() {
    if ($(document).find('#PurchaseOrderSearch').hasClass('dataTable')) {
        dtTable.destroy();
    }
    dtTable = $("#PurchaseOrderSearch").DataTable({
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
                }//PO Sorting
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
                        var LayoutInfo = JSON.parse(json.LayoutInfo);//PO Sorting
                        order = LayoutInfo.order[0][0];//PO Sorting
                        orderDir = LayoutInfo.order[0][1]; //PO Sorting
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
                            //#region CompleteDate
                            var CompleteDateme = $("#CompleteDate").val();
                            if (CompleteDateme == CompleteDateRange) {
                                $('#comptimeperiodcontainer').show();
                            }
                            else {
                                $('#comptimeperiodcontainer').hide();
                            }
                            switch (CompleteDateme) {
                                case CompleteToday:
                                    $('#comptimeperiodcontainer').hide();
                                    StartCompleteDate = today;
                                    EndCompleteDate = today;
                                    break;
                                case Complete7Days:
                                    $('#comptimeperiodcontainer').hide();
                                    StartCompleteDate = PreviousDateByDay(7);
                                    break;
                                case Complete30Days:
                                    $('#comptimeperiodcontainer').hide();
                                    StartCompleteDate = PreviousDateByDay(30);
                                    EndCompleteDate = today;
                                    break;
                                case Complete60Days:
                                    $('#comptimeperiodcontainer').hide();
                                    StartCompleteDate = PreviousDateByDay(60);
                                    EndCompleteDate = today;
                                    break;
                                case Complete:
                                    $('#comptimeperiodcontainer').hide();
                                    StartCompleteDate = PreviousDateByDay(90);
                                    EndCompleteDate = today;
                                    break;
                                case CompleteDateRange:
                                    $('#comptimeperiodcontainer').show();
                                    var advComDateRange = $("#advcompletedaterange").val();
                                    var advComDateRangeArr = advComDateRange.split("-");
                                    StartCompleteDate = LRTrim(advComDateRangeArr[0]);
                                    EndCompleteDate = LRTrim(advComDateRangeArr[1]);
                                    $('#advcompletedaterange').val(StartCompleteDate + ' - ' + EndCompleteDate);
                                    $(document).find('#advcompletedaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                                        StartCompleteDate = start.format('MM/DD/YYYY');
                                        EndCompleteDate = end.format('MM/DD/YYYY');
                                    });
                                    break;
                                default:
                                    $('#comptimeperiodcontainer').hide();
                                    $(document).find('#advcompletedaterange').daterangepicker({
                                        format: 'MM/DD/YYYY'
                                    });
                                    StartCompleteDate = '';
                                    EndCompleteDate = '';
                                    break;

                            }
                            //#endregion

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
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [
            {
                text: 'Export CSV',
                orientation: 'landscape',
                extend: 'csv',
                filename: 'Purchase Order List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                orientation: 'landscape',
                pageSize: 'A3',
                css: 'display:none',
                title: 'Purchase Order List'
            },
            {
                extend: 'excelHtml5',
                title: 'Purchase Order List'
            },
            {
                extend: 'print',
                title: 'Purchase Order List',
                orientation: 'landscape'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Purchasing/GetPOGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.CustomQueryDisplayId = CustomQueryDisplayId;
                d.CompleteStartDateVw = ValidateDate(CompleteStartDateVw);
                d.CompleteEndDateVw = ValidateDate(CompleteEndDateVw);
                d.CreateStartDateVw = ValidateDate(CreateStartDateVw);
                d.CreateEndDateVw = ValidateDate(CreateEndDateVw);
                d.PurchaseOrder = LRTrim($("#PurchaseOrder").val());
                d.Status = LRTrim($("#Status").val());
                d.VendorClientLookupId = LRTrim($("#VendorClientLookupId").val());
                d.VendorName = LRTrim($('#VendorName').val());
                d.StartCreateDate = ValidateDate(StartCreateteDate);
                d.EndCreateDate = ValidateDate(EndCreateteDate);
                d.Attention = LRTrim($("#Attention").val());
                d.VendorPhoneNumber = LRTrim($("#VendorPhoneNumber").val());
                d.StartCompleteDate = ValidateDate(StartCompleteDate);
                d.EndCompleteDate = ValidateDate(EndCompleteDate);
                d.Reason = LRTrim($("#Reason").val());
                d.Buyer_PersonnelName = LRTrim($("#BuyerPersonnelName").val());
                d.TotalCost = LRTrim($("#TotalCost").val());
                d.FilterValue = "0";
                d.Required = ValidateDate($("#Required").val());
                d.txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
                d.Order = order;//PO Sorting
                //d.orderDir = orderDir;//PO Sorting
            },
            "dataSrc": function (result) {

                let colOrder = dtTable.order();
                //  order = colOrder[0][0];
                orderDir = colOrder[0][1];

                if (result.recordsTotal == 0) {
                    $(document).find('.import-export').prop("disabled", true);
                }
                else {
                    $(document).find('.import-export').removeAttr('disabled');
                }

                // HidebtnLoader("btnsortmenu");
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
                    "data": "PurchaseOrderId",
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
                    "data": "PurchaseOrderId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    targets: 0,
                    "name": "0",
                    'render': function (data, type, full, meta) {
                        return '<input type="checkbox" name="id[]" data-poid="' + data + '" class="chkPOsearch"  value="'
                            + $('<div/>').text(data).html() + '">';
                    }
                },
                {
                    "data": "ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_poSearch href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                    mRender: function (data, type, full, meta) {
                        if (data == statusCode.Partial) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--yellow m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Complete) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Void) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--red m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Open) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else {
                            return getStatusValue(data);
                        }
                    }
                },
                { "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date" },
                { "data": "Attention", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "VendorPhoneNumber", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CompleteDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date" },
                {
                    "data": "Reason", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "Buyer_PersonnelName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, className: "text-right" },
                { "data": "Required", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date" }
            ],
        "columnDefs": [
            {
                targets: [0, 1, 2],
                className: 'noVis'
            }
        ],
        initComplete: function (settings, json) {
            SetPageLengthMenu();
            //----------conditional column hiding-------------//
            //var api = new $.fn.dataTable.Api(settings);
            //var columns = dtTable.settings().init().columns;
            //var arr = [];
            //var j = 0;
            //while (j < json.hiddenColumnList.length) {
            //    var clsname = '.' + json.hiddenColumnList[j];
            //    var title = dtTable.columns(clsname).header();
            //    titleArray.push(title[0].innerHTML);
            //    classNameArray.push(clsname);
            //    dtTable.columns(clsname).visible(false);
            //    var sortMenuItem = '.dropdown-menu' + ' ' + clsname;
            //    $(sortMenuItem).remove();
            //    //---hide adv search items---
            //    var advclsname = '.' + "PO-" + json.hiddenColumnList[j];
            //    $(document).find(advclsname).hide();
            //    j++;
            //}
            //----------------------------------------------//

            $("#POGridAction :input").removeAttr("disabled");
            $("#POGridAction :button").removeClass("disabled");
            DisableExportButton($('#PurchaseOrderSearch'), $(document).find('.import-export'));
        }
    });
}
$('#PurchaseOrderSearch').find('th').click(function () {
    run = true;
    order = $(this).data('col');
});
$(document).find('#PurchaseOrderSearch').on('click', 'tbody td img', function (e) {

    var tr = $(this).closest('tr');
    var row = dtTable.row(tr);
    if (this.src.match('details_close')) {
        this.src = "../../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        this.src = "../../Images/details_close.png";
        var PurchaseOrderID = $(this).attr("rel");
        $.ajax({
            url: "/Purchasing/GetPoInnerGrid",
            data: {
                PurchaseOrderID: PurchaseOrderID
            },
            beforeSend: function () {
                ShowLoader();
            },
            dataType: 'html',
            success: function (json) {
                row.child(json).show();
                dtinnerGrid = row.child().find('.purchaseOrderinnerDataTable').DataTable(
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
                        initComplete: function () { row.child().find('.dataTables_scroll').addClass('tblchild-scroll'); CloseLoader(); }
                    });
                tr.addClass('shown');
            }
        });

    }
});
$(document).on('click', '#PurchaseOrderSearch_paginate .paginate_button', function () {
    PRAdvSearch();
    run = true;
});
$(document).on('change', '#PurchaseOrderSearch_length .searchdt-menu', function () {
    PRAdvSearch();
    run = true;
});
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var PurchaseOrder = LRTrim($("#PurchaseOrder").val());
            var Status = $("#Status").val();
            var VendorClientLookupId = LRTrim($("#VendorClientLookupId").val());
            var VendorName = LRTrim($('#VendorName').val());
            var Attention = LRTrim($("#Attention").val());
            var VendorPhoneNumber = LRTrim($("#VendorPhoneNumber").val());
            var Reason = LRTrim($("#Reason").val());
            var Buyer_PersonnelName = LRTrim($("#BuyerPersonnelName").val());
            var TotalCost = LRTrim($("#TotalCost").val());
            var colname = order;//PO Sorting
            var coldir = orderDir;//PO Sorting
            var FilterValue = "0";
            var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
            var Required = LRTrim($('#Required').val());
            var jsonResult = $.ajax({
                url: '/Purchasing/GetPOPrintData?page=all',
                data: {
                    CustomQueryDisplayId: CustomQueryDisplayId,
                    CompleteStartDateVw: ValidateDate(CompleteStartDateVw),
                    CompleteEndDateVw: ValidateDate(CompleteEndDateVw),
                    CreateStartDateVw: ValidateDate(CreateStartDateVw),
                    CreateEndDateVw: ValidateDate(CreateEndDateVw),
                    PurchaseOrder: PurchaseOrder,
                    Status: Status,
                    VendorClientLookupId: VendorClientLookupId,
                    VendorName: VendorName,
                    StartCreateDate: ValidateDate(StartCreateteDate),
                    EndCreateDate: ValidateDate(EndCreateteDate),
                    Attention: Attention,
                    VendorPhoneNumber: VendorPhoneNumber,
                    StartCompleteDate: ValidateDate(StartCompleteDate),
                    EndCompleteDate: ValidateDate(EndCompleteDate),
                    Reason: Reason,
                    Buyer_PersonnelName: Buyer_PersonnelName,
                    TotalCost: TotalCost,
                    colname: colname,
                    coldir: coldir,
                    FilterValue: FilterValue,
                    Required: Required,
                    txtSearchval: txtSearchval
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#PurchaseOrderSearch thead tr th").map(function (key) {
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
                if (item.Status != null) {
                    item.Status = item.Status;
                }
                else {
                    item.Status = "";
                }
                if (item.VendorClientLookupId != null) {
                    item.VendorClientLookupId = item.VendorClientLookupId;
                }
                else {
                    item.VendorClientLookupId = "";
                }
                if (item.VendorName != null) {
                    item.VendorName = item.VendorName;
                }
                else {
                    item.VendorName = "";
                }
                if (item.CreateDate != null) {
                    item.CreateDate = item.CreateDate;
                }
                else {
                    item.CreateDate = "";
                }
                if (item.Attention != null) {
                    item.Attention = item.Attention;
                }
                else {
                    item.Attention = "";
                }
                if (item.VendorPhoneNumber != null) {
                    item.VendorPhoneNumber = item.VendorPhoneNumber;
                }
                else {
                    item.VendorPhoneNumber = "";
                }
                if (item.CompleteDate != null) {
                    item.CompleteDate = item.CompleteDate;
                }
                else {
                    item.CompleteDate = "";
                }
                if (item.Reason != null) {
                    item.Reason = item.Reason;
                }
                else {
                    item.Reason = "";
                }
                if (item.Buyer_PersonnelName != null) {
                    item.Buyer_PersonnelName = item.Buyer_PersonnelName;
                }
                else {
                    item.Buyer_PersonnelName = "";
                }
                if (item.TotalCost != null) {
                    item.TotalCost = item.TotalCost;
                }
                else {
                    item.TotalCost = "";
                }
                if (item.Required != null) {
                    item.Required = item.Required;
                }
                else {
                    item.Required = "";
                }
                var fData = [];
                $.each(visiblecolumnsIndex, function (index, inneritem) {
                    var key = Object.keys(item)[inneritem];
                    var value = item[key];
                    fData.push(value);
                });
                d.push(fData);
            });

            return {
                body: d,
                header: $("#PurchaseOrderSearch thead tr th").find('div').map(function (key) {
                    if ($(this).parents('.purchaseOrderinnerDataTable').length == 0 && this.innerHTML) {
                        return this.innerHTML;
                    }
                }).get()
            };
        }
    });
});
$(document).on('click', '.lnk_poSearch', function (e) {
    e.preventDefault();
    var index_row = $('#PurchaseOrderSearch tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtTable.row(row).data();
    var PurchaseOrderId = data.PurchaseOrderId;
    var titletext = $('#posearchtitle').text();
    localStorage.setItem("postatustext", titletext);
    $.ajax({
        url: "/Purchasing/Details",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { PurchaseOrderId: PurchaseOrderId },
        success: function (data) {
            $('#renderpurchasing').html(data);
            $(document).find('#spnlinkToSearch').text(titletext);
        },
        complete: function () {
            generateLineiItemdataTable(PurchaseOrderId, "");
            CloseLoader();
            SetPODetailsControls();
            if ($(document).find('.AddPO').length === 0) { $(document).find('#poactiondiv').css('margin-right', '0px'); }
        }
    });
});
$(document).on('change', '.chkPOsearch', function () {

    var data = dtTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        SelectPOId = SelectPOId.filter(function (el) {
            return el.PurchaseOrderId !== data.PurchaseOrderId;
        });
    }
    else {
        var item = new PurchaseOrderNotInSelectedItem(data.PurchaseOrderId, data.ClientLookupId, data.Status);
        var found = SelectPOId.some(function (el) {
            return el.PurchaseOrderId === data.PurchaseOrderId;
        });
        if (!found) { SelectPOId.push(item); }
    }
    if (SelectPOId.length > 0) {
        $(".actionBar").hide();
        $(".updateArea").fadeIn();
        $('#PrintPOCheckList').removeAttr("disabled");
    }
    else {
        $(".updateArea").hide();
        $(".actionBar").fadeIn();
        $('#PrintPOCheckList').prop("disabled", "disabled");

    }
    $('.itemcount').text(SelectPOId.length);
});
function PurchaseOrderNotInSelectedItem(PurchaseOrderId, ClientLookupId, Status) {
    this.PurchaseOrderId = PurchaseOrderId;
    this.ClientLookupId = ClientLookupId;
    this.Status = Status;
};


//$(document).on('click', '#PrintPOCheckList', function () {
//    if (SelectPOId.length < 1) {
//        ShowGridItemSelectionAlert();
//        return false;
//    }
//    else {
//        var jsonResult = {
//            "list": SelectPOId
//        };
//        $.ajax({
//            url: '/Purchasing/PrintPOListFromIndex',
//            data: JSON.stringify(jsonResult),
//            type: "POST",
//            datatype: "json",
//            contentType: 'application/json; charset=utf-8',
//            responseType: 'arraybuffer',
//            beforeSend: function () {
//                ShowLoader();
//            },
//            success: function (result) {
//                if (result.success && result.jsonStringExceed == false) {
//                    PdfPrintAllPOList(result.pdf);
//                }
//                else {
//                    CloseLoader();
//                    var errorMessage = getResourceValue("PdfFileSizeExceedAlert");
//                    ShowErrorAlert(errorMessage);
//                    return false;
//                }

//            },
//            complete: function () {
//                CloseLoader();
//                $(".updateArea").hide();
//                $(".actionBar").fadeIn();
//                $(document).find('.chkPOsearch').prop('checked', false);
//                $('.itemcount').text(0);
//                SelectPOId = [];
//            }
//        });

//    }
//});
function PdfPrintAllPOList(pdf) {
    var blob = b64StrtoBlob(pdf, 'application/pdf');
    var blobUrl = URL.createObjectURL(blob);
    var htmlbody = '<iframe id="iframeid" src=' + blobUrl + ' style="position:fixed; top:10; left:0; bottom:0; right:0; width:100%; height:100%; border:none; margin:0; padding:0; overflow:hidden; z-index:999999;" style="display:none" > </iframe>';
    var winhtml = window.open("", "PdfWndow", "width=800,height=800");
    if (winhtml) {
        winhtml.document.write(htmlbody);
        winhtml.document.getElementById('iframeid').contentWindow.print();
    }
    else {
        var errorMessage = getResourceValue("CheckPopUpEnabledAlert");
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
var dtLineItemTable;
var lItemfilteritemcount = 0;
var typeValPoLiStatus;
var GrandTotalCost = 0;
var recordsTotalcount = 0;
function generateLineiItemdataTable(PurchaseOrderId, searchtext) {
    var rCount = 0;
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
    var PurchaseUOM = $('#UOM').val();
    if (PurchaseUOM) {
        PurchaseUOM = LRTrim(PurchaseUOM);
    }
    var UnitCost = $("#UnitCost").val();
    if (UnitCost) {
        UnitCost = LRTrim(UnitCost);
    }
    var TotalCost = $("input[id='TotalCost']").val();
    if (TotalCost) {
        TotalCost = LRTrim(TotalCost);
    }
    var Status = $("#PoLiStatus").val();
    if (Status) {
        Status = LRTrim(Status);
    }
    else {
        Status = "";
    }
    var Account = $('#liAccount').val();
    if (Account) {
        Account = LRTrim(Account);
    }
    var ChargeToClientLookupId = $('#ChargeToClientLookupId').val();
    if (ChargeToClientLookupId) {
        ChargeToClientLookupId = LRTrim(ChargeToClientLookupId);
    }
    var StoreroomId = $(document).find("#PurchaseOrderModel_StoreroomId").val();
    var visibility = lineNumberGridSecurity;
    var visibilityAddLineitem = VisibilityAddLineItem;
    var LineItemStaus = lineNumberGridStatus;
    var PunchoutFunctionality = isPunchout;
    var isExternalStatus = isExternal;
    if ($(document).find('#tblLineItem').hasClass('dataTable')) {
        dtLineItemTable.destroy();
    }
    dtLineItemTable = $("#tblLineItem").DataTable({
        colReorder: true,
        rowGrouping: true,
        serverSide: true,
        searching: true,
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
            "url": "/Purchasing/PopulateLineItem",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.PurchaseOrderId = PurchaseOrderId;
                d.searchText = searchtext;
                d.LineNumber = LineNumber;
                d.PartId = PartId;
                d.Description = Description;
                d.Quantity = Quantity;
                d.PurchaseUOM = PurchaseUOM;
                d.UnitCost = UnitCost;
                d.TotalCost = TotalCost;
                d.Status = Status;
                d.Account = Account;
                d.ChargeToClientLookupId = ChargeToClientLookupId;
                d.StoreroomId = StoreroomId;
            },
            "dataSrc": function (result) {
                rCount = result.data.length;
                $("#PoLiStatus").empty();
                GrandTotalCost = result.GrandTotalCost;
                recordsTotalcount = result.recordsTotal;
                $("#PoLiStatus").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < result.statuslist.length; i++) {
                    var id = result.statuslist[i];
                    var name = result.statuslist[i];
                    $("#PoLiStatus").append("<option value='" + id + "'>" + name + "</option>");
                }
                $("#UOM").empty();
                $("#UOM").append("<option value=''>" + "--Select--" + "</option>");
                var unitOfMeasure = [];
                for (var key in result.data) {
                    if (unitOfMeasure.indexOf(result.data[key].PurchaseUOM) == -1) {
                        unitOfMeasure.push(result.data[key].PurchaseUOM);
                    }
                }
                for (unitVal in unitOfMeasure) {
                    var name = unitOfMeasure[unitVal];
                    $("#UOM").append("<option value='" + name + "'>" + name + "</option>");
                }
                if (PurchaseUOM && $("#UOM option[value='" + PurchaseUOM + "']").length > 0) {
                    $("#UOM").val(PurchaseUOM).trigger('change.select2');
                }
                else {
                    $("#UOM").val("").trigger('change.select2');
                }
                $(document).find('.select2picker').select2({});
                if (typeValPoLiStatus && $("#PoLiStatus option[value='" + typeValPoLiStatus + "']").length > 0) {
                    $(document).find("#PoLiStatus").val(typeValPoLiStatus).trigger('change.select2');
                }
                else {
                    $(document).find("#PoLiStatus").val("").trigger('change.select2');
                }
                return result.data;
            },
            global: true
        },
        columnDefs: [
            {
                "data": null,
                targets: [10], render: function (a, b, data, d) {
                    if (LineItemStaus == "Void") {
                        return "";
                    }
                    else if (visibility == "True" && visibilityAddLineitem == "False" && isExternalStatus == "False") {
                        return '<a class="btn btn-outline-success editPOLineItemBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delPOLineItemBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else if (visibility == "False" && visibilityAddLineitem == "True" && PunchoutFunctionality == "False" && isExternalStatus == "False") {
                        return '<a data-toggle="modal" href="#AddLineItems" class="btn btn-outline-primary addPOLineItemBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>'
                    }
                    else if (visibility == "True" && visibilityAddLineitem == "True" && PunchoutFunctionality == "False" && isExternalStatus == "False") {
                        return '<a data-toggle="modal" href="#AddLineItems" class="btn btn-outline-primary addPOLineItemBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editPOLineItemBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delPOLineItemBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a></div>';
                    }
                    else if (visibility == "True" && visibilityAddLineitem == "True" && PunchoutFunctionality == "True" && isExternalStatus == "False") {
                        return '<a class="btn btn-outline-success editPOLineItemBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delPOLineItemBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a></div>';
                    }
                    else {
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
                { "data": "UnitOfMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },
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
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "AccountClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ChargeToClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }

            ],
        "footerCallback": function (row, data, start, end, display) {
            var info = dtLineItemTable.page.info();
            var api = this.api();
            var rows = api.rows().nodes();
            var getData = api.rows({ page: 'current' }).data();
            // Total over all pages
            //total = api.column(6).data().reduce(function (a, b) {
            //    return parseFloat(a) + parseFloat(b);
            //}, 0);
            total = GrandTotalCost;
            // Update footer
            var start = info.start;
            var end = info.end;
            $("#tblLineItemfoot").empty();
            if (data.length != 0 && recordsTotalcount == end) {
                var footer = "";
                if ((LineItemStaus == "Void") || (isExternalStatus == "True") || (visibility == "False" && visibilityAddLineitem == "False")) {
                    footer = '<tr><th></th><th></th><th></th><th></th><th></th><th style="text-align: left!important; font-weight: 500!important; color:#0b0606!important">Grand Total</th><th style = "text-align: right!important; font-weight: 500!important; color: #0b0606!important; padding: 0px 10px 0px 0px!important" >' + total.toFixed(2) + '</th><th></th><th></th><th></th></tr>'
                }
                else {
                    footer = '<tr><th></th><th></th><th></th><th></th><th></th><th style="text-align: left!important; font-weight: 500!important; color:#0b0606!important">Grand Total</th><th style = "text-align: right!important; font-weight: 500!important; color: #0b0606!important; padding: 0px 10px 0px 0px!important" >' + total.toFixed(2) + '</th><th></th><th></th><th></th><th></th></tr>'
                }

                $("#tblLineItemfoot").empty().append(footer);
            } else {
                $("#tblLineItemfoot").empty().append("");
            }
        },
        initComplete: function (settings, json) {
            //----------conditional column hiding-------------//

            //var j = 0;
            //while (j < json.hiddenColumnList.length) {
            //    var clsname = '.' + json.hiddenColumnList[j];
            //    dtLineItemTable.columns(clsname).visible(false);
            //    var advclsname = '.' + "POLI-" + json.hiddenColumnList[j];
            //    $(document).find(advclsname).hide();
            //    j++;
            //}
            //----------------------------------------------//

            var column = this.api().column(10);
            if (rCount > 0) {
                $("#addLineItem").hide();
            }
            else {
                if (LineItemStaus == "Void") {
                    $("#addLineItem").hide();
                }
                if (isExternalStatus == "True") {
                    $("#addLineItem").hide();
                }
                else if (visibilityAddLineitem == "True") {
                    $("#addLineItem").show();
                }
                else {
                    $("#addLineItem").hide();
                }
            }
            if (PunchoutFunctionality == "True") {
                $("#addLineItem").hide();
            }
            if (LineItemStaus == "Void") {
                column.visible(false);
            }
            else if (isExternalStatus == "True") {
                column.visible(false);
            }
            else if (visibility == "False" && visibilityAddLineitem == "False") {
                column.visible(false);
            }
            else if (visibility == "True" || visibilityAddLineitem == "True") {
                column.visible(true);
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnLitemSearch", function () {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    clearLineItemAdvanceSearch();
    dtLineItemTable.state.clear();
    var searchText = LRTrim($('#txtsearchbox').val());
    typeValPoLiStatus = $("#PoLiStatus").val();
    generateLineiItemdataTable(PurchaseOrderId, searchText);
});
$(document).on('click', '#lineitemClearAdvSearchFilter', function () {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    $("#txtsearchbox").val("");
    clearLineItemAdvanceSearch();
    generateLineiItemdataTable(PurchaseOrderId, "");
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
    typeValPoLiStatus = $("#PoLiStatus").val();
    $('#lineitemadvsearchfilteritems').html(searchitemhtml);
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    LineItemAdvanceSearch();
    $('.lifilteritemcount').text(lItemfilteritemcount);
});
function clearLineItemAdvanceSearch() {
    var filteritemcount = 0;
    $('#litemadvsearchsidebar').find('input:text').val('');
    $('.lifilteritemcount').text(filteritemcount);
    $('#lineitemadvsearchfilteritems').html('');
    $(document).find("#PoLiStatus,#UOM").val("").trigger('change.select2');
    typeValPoLiStatus = $("#PoLiStatus").val();
}
function LineItemAdvanceSearch() {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    generateLineiItemdataTable(PurchaseOrderId, "");
}
$(document).on('click', '.litembtnCross', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId + '.adv-item').val('').trigger('change');
    $(this).parent().remove();
    lItemfilteritemcount--;
    if (searchtxtId == "PoLiStatus") {
        typeValPoLiStatus = null;
    }
    LineItemAdvanceSearch();
    $('.lifilteritemcount').text(lItemfilteritemcount);

});
//$(document).on('click', '.editPOLineItemBttn', function () {
//    var data = dtLineItemTable.row($(this).parents('tr')).data();
//    EditPOLineItem(data.PurchaseOrderLineItemId);
//});
//function EditPOLineItem(lineitemid) {
//    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
//    var POClientLookupId = $(document).find('#PurchaseOrderModel_ClientLookupId').val();;
//    var IsPunchout = isPunchout;
//    $.ajax({
//        url: "/Purchasing/EditLineItem",
//        type: "POST",
//        dataType: 'html',
//        data: { LineItemId: lineitemid, PurchaseOrderId: PurchaseOrderId, POClientLookupId: POClientLookupId, IsPunchout: IsPunchout },
//        beforeSend: function () {
//            ShowLoader();
//        },
//        success: function (data) {
//            $('#renderpurchasing').html(data);
//        },
//        complete: function () {
//            SetPOControls();
//        },
//        error: function (jqXHR, exception) {
//            CloseLoader();
//        }
//    });
//}
//#region V2-653
$(document).on('click', '.editPOLineItemBttn', function () {
    var data = dtLineItemTable.row($(this).parents('tr')).data();
    var IsSingleStock = $(document).find('#PurchaseOrderModel_SingleStockLineItem').val();
    if (IsSingleStock == "True" && data.PartId > 0) {
        EditPOLineItem(data.PurchaseOrderLineItemId, "/Purchasing/EditPOPartInInventorySingleStockDynamic");
    }
    else {
        if (data.PartId > 0) {
            EditPOLineItem(data.PurchaseOrderLineItemId, "/Purchasing/EditPOPartInInventoryDynamic");
            return;
        }
        EditPOLineItem(data.PurchaseOrderLineItemId, "/Purchasing/EditPOPartNotInInventoryDynamic");
    }

});
function EditPOLineItem(lineitemid, url) {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    var POClientLookupId = $(document).find('#PurchaseOrderModel_ClientLookupId').val();
    var StoreroomId = $(document).find('#PurchaseOrderModel_StoreroomId').val();
    var IsPunchout = isPunchout;
    $.ajax({
        url: url,
        type: "POST",
        dataType: 'html',
        data: { LineItemId: lineitemid, PurchaseOrderId: PurchaseOrderId, POClientLookupId: POClientLookupId, IsPunchout: IsPunchout, StoreroomId: StoreroomId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchasing').html(data);
        },
        complete: function () {
            SetPOControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
//#endregion
$(document).on('click', "#btnpolineitemcancel", function () {
    var PurchaseOrderId = $(document).find('#lineItem_PurchaseOrderId').val();
    RedirectToDetailOncancel(PurchaseOrderId, "overview");
});
//$(document).on('click', "#btnpoNonPartIncancel", function () {
//    var PurchaseOrderId = $(document).find('#lineItem_PurchaseOrderId').val();
//    RedirectToDetailOncancel(PurchaseOrderId, "overview");
//});
$(document).on('click', "#btnPOLineItemcancel", function () {
    var PurchaseOrderId = $(document).find('#PurchaseOrderId').val();
    RedirectToDetailOncancel(PurchaseOrderId, "overview");
});
function EditLineItemOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        SuccessAlertSetting.text = getResourceValue("LineItemUpdatedAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPODetail(data.PurchaseOrderId, "overview");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function AddPartNonInInventoryOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        SuccessAlertSetting.text = getResourceValue("LineItemAddedAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPODetail(data.PurchaseOrderId, "overview");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '.delPOLineItemBttn', function () {
    var data = dtLineItemTable.row($(this).parents('tr')).data();
    var PurchaseOrderId = data.PurchaseOrderId;
    var PurchaseOrderLineItemId = data.PurchaseOrderLineItemId;
    DeletePRLineItem(PurchaseOrderId, PurchaseOrderLineItemId)
});
function DeletePRLineItem(PurchaseOrderId, PurchaseOrderLineItemId) {
    CancelAlertSetting.closeOnConfirm = false;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: "/Purchasing/DeleteLineItem",
            type: "POST",
            dataType: 'json',
            data: { _PurchaseOrderId: PurchaseOrderId, _PurchaseOrderLineItemId: PurchaseOrderLineItemId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                var message = "";
                if (data == "success") {
                    SuccessAlertSetting.text = getResourceValue("LineItemDeletedAlert");
                    swal(SuccessAlertSetting, function () {
                        if (data == "success") {
                            dtLineItemTable.destroy();
                            generateLineiItemdataTable(PurchaseOrderId, "");
                        }
                    });
                }
                else {
                    message = getResourceValue("LineItemDeleteFailedAlert");
                    swal({
                        title: "Alert",
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
$(document).on('change', '#lineItem_ChargeType', function () {
    $(document).find('#txtChargeToId').val('');
});
$(document).on('change', '#lineItem_ChargeToId', function () {
    var Id = $(this).val();
    var type = $("#lineItem_ChargeType option:selected").val();
    var textChargeToId = $("#lineItem_ChargeToId option:selected").text();
    if (textChargeToId == "--Select--") {
        $("#lineItem_ChargeTo_Name").val('');
        return false;
    }
    else {
        var Name = '';
        $.ajax({
            url: "/Purchasing/GetChargeToName",
            type: "GET",
            dataType: 'json',
            data: { _Id: Id, _type: type },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (type == "Equipment") {
                    Name = data.Name
                }
                else if (type == "WorkOrder") {
                    Name = data.ChargeTo_Name
                }
                else if (type == "Account") {
                    Name = data.Name
                }
            },
            complete: function () {
                CloseLoader();
                $("#lineItem_ChargeTo_Name").val(Name);
                $(document).find('.select2picker').select2({});
            },
            error: function () {
                CloseLoader();
            }
        });
    }
});
$(document).on('click', "#selectidpartnotininventory", function (e) {
    e.preventDefault();
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    $('.modal-backdrop').remove();
    GoToAddNonPartInInventory(PurchaseOrderId);
});
$(document).on('click', "#selectidpartininventory", function (e) {
    e.preventDefault();
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    var IsSingleStock = $(document).find('#PurchaseOrderModel_SingleStockLineItem').val();
    $('.modal-backdrop').remove();
    if (IsSingleStock == "True")
        GoToAddPartInInventorySingleStock(PurchaseOrderId)
    else
        GoToAddPartInInventory(PurchaseOrderId);
});
function GoToAddNonPartInInventory(PurchaseOrderId) {
    $.ajax({
        //  url: "/Purchasing/AddNonPartInInventory",
        url: "/Purchasing/AddNonPartInInventoryDynamic",
        type: "POST",
        dataType: "html",
        data: { PurchaseOrderId: PurchaseOrderId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchasing').html(data);
        },
        complete: function () {
            CloseLoader();
            SetPOControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion
//#region Common
function SetPOControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if ($(this) && $(this).valid()) {
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
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    SetFixedHeadStyle();
}
function SetPODetailsControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
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
//#endregion

//#region Work Flow History -  Removed as JIRA V2-30
//#endregion
//#region SelectPartgrid
var spartgridselectCount = 0;
var PartNotInInventorySelectedItemArray = [];
var FinalGridSelectedItemArray = [];
var finalSelectPartsTable;
var SelectPartsTable;
function PartNotInInventorySelectedItem(PartId, ClientLookupId, Description, Manufacture, Quantity) {
    this.PartId = PartId;
    this.ClientLookupId = ClientLookupId;
    this.Description = Description;
    this.Manufacture = Manufacture;
    this.Quantity = Quantity;
};
function PartNotInInventoryProcessdata(PartId, ClientLookupId, Description, OrderQuantity, PurchaseOrderId) {
    this.PartId = PartId;
    this.ClientLookupId = ClientLookupId;
    this.Description = Description;
    this.OrderQuantity = OrderQuantity;
    this.PurchaseOrderId = PurchaseOrderId;
};
function GoToAddPartInInventory(PurchaseOrderId) {
    var ClientLookupId = $(document).find('#PurchaseOrderModel_ClientLookupId').val();
    var vendorId = $(document).find("#PurchaseOrderModel_VendorId").val();
    var StoreroomId = $(document).find("#PurchaseOrderModel_StoreroomId").val();
    $.ajax({
        url: "/Purchasing/AddPartInInventory",
        type: "POST",
        dataType: "html",
        data: { PurchaseOrderId: PurchaseOrderId, ClientLookupId: ClientLookupId, vendorId: vendorId, StoreroomId: StoreroomId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchasing').html(data);
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
$(document).on('click', '#brdpolineitem', function () {
    var PurchaseOrderId = $(this).attr('data-val');
    PartNotInInventorySelectedItemArray = [];
    RedirectToPODetail(PurchaseOrderId);
});
$(document).on('click', "#btnAddInventorycancel", function () {
    var PurchaseOrderId = $(document).find('#partInInventoryModel_PurchaseOrderId').val();
    PartNotInInventorySelectedItemArray = [];
    swal(CancelAlertSetting, function () {
        RedirectToPODetail(PurchaseOrderId);
    });

});
//#endregion

//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    //funPoCustomizeBtnClick(dtTable, null, titleArray);
    funCustomizeBtnClick(dtTable, true, titleArray);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0, 1, 2];
    funCustozeSaveBtn(dtTable, colOrder);
    run = true;
    dtTable.state.save(run);
    if (classNameArray != null && classNameArray.length > 0) {
        var j = 0;
        while (j < classNameArray.length) {
            dtTable.columns(classNameArray[j]).visible(false);
            j++;
        }
    }
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

$(document).on('keyup', '#posearctxtbox', function (e) {
    var tagElems = $(document).find('#posearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.posearchdrpbox', function (e) {
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $(document).find('.chksearch').prop('checked', false);
    $('.itemcount').text(0);
    SelectPOId = [];
    run = true;
    //V2-364
    if ($(this).attr('id') == '0') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("PURCHASEORDERSTATUS");
        if (val == AllStatusValuesToday || val == AllStatusValues7Days || val == AllStatusValues30Days || val == AllStatusValues60Days ||
            val == AllStatusValues90Days || val == AllStatusValuesDateRange) {
            $('#cmbcreateview').val(val).trigger('change');
        }
        //Add on 02/07/2020 Problem with Searching DateRange Modal PopUP For All Status & Complete Dropdown Item
        else {
            CreateStartDateVw = '';
            CreateEndDateVw = '';
            localStorage.removeItem('POCreateStartDateVw');
            localStorage.removeItem('POCreateEndDateVw');
            $(document).find('#cmbcreateview').val('').trigger('change');
        }
        //Add on 02/07/2020 Problem with Searching DateRange Modal PopUP For All Status & Complete Dropdown Item
        $(document).find('#DateRangeModalForCreateDate').modal('show');
        return;
    }

    else {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('POCreateStartDateVw');
        localStorage.removeItem('POCreateEndDateVw');
        $(document).find('#cmbcreateview').val('').trigger('change');
    }
    //V2-364
    if ($(this).attr('id') == '6') {
        $(document).find('#searcharea').hide("slide");
        var val2 = localStorage.getItem("PURCHASEORDERSTATUS");
        if (val2 == CompleteToday || val2 == Complete7Days || val2 == Complete30Days || val2 == Complete60Days || val2 == Complete || val2 == CompleteDateRange) {
            $('#cmbcompleteview').val(val2).trigger('change');
        }
        //Add on 02/07/2020 Problem with Searching DateRange Modal PopUP For All Status & Complete Dropdown Item
        else {
            CompleteStartDateVw = '';
            CompleteEndDateVw = '';
            localStorage.removeItem('POCompleteStartDateVw');
            localStorage.removeItem('POCompleteEndDateVw');
            $(document).find('#cmbcompleteview').val('').trigger('change');
        }
        //Add on 02/07/2020 Problem with Searching DateRange Modal PopUP For All Status & Complete Dropdown Item
        $(document).find('#DateRangeModal').modal('show');
        return;
    }
    else {
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        localStorage.removeItem('POCompleteStartDateVw');
        localStorage.removeItem('POCompleteEndDateVw');
        $(document).find('#cmbcompleteview').val('').trigger('change');
    }
    if ($(this).attr('id') != '0') {
        $('#posearchtitle').text($(this).text());
    }
    else {
        $('#posearchtitle').text(getResourceValue("OpenAlert"));
    }
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("PURCHASEORDERSTATUS", optionval);
    CustomQueryDisplayId = optionval;
    ShowbtnLoaderclass("LoaderDrop");
    PRAdvSearch();
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#advsearchfilteritems").html('');
    $(document).find('#txtColumnSearch').val('');
    dtTable.page('first').draw('page');
});
//V2-364
$(document).on('change', '#cmbcreateview', function (e) {
    var thielement = $(this);
    CustomQueryDisplayId = thielement.val();
    if (thielement.val() == AllStatusValuesDateRange) {
        var strtlocal = localStorage.getItem('POCreateStartDateVw');
        if (strtlocal) {
            CreateStartDateVw = strtlocal;
        }
        else {
            CreateStartDateVw = today;
        }
        var endlocal = localStorage.getItem('POCreateEndDateVw');
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
        localStorage.removeItem('POCreateStartDateVw');
        localStorage.removeItem('POCreateEndDateVw');
        $(document).find('#timeperiodcontainerForCreateDate').hide();
    }
})

$(document).on('click', '#btntimeperiodForCreateDate', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbcreateview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != AllStatusValuesDateRange) {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('POCreateStartDateVw');
        localStorage.removeItem('POCreateEndDateVw');
    }
    else {
        localStorage.setItem('POCreateStartDateVw', CreateStartDateVw);
        localStorage.setItem('POCreateEndDateVw', CreateEndDateVw);
    }
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#advsearchfilteritems").html('');
    $(document).find('#txtColumnSearch').val('');
    dtTable.page('first').draw('page');
    $(document).find('#DateRangeModalForCreateDate').modal('hide');
    var text = $('#posearchListul').find('li').eq(0).text();

    if (daterangeval != AllStatusValuesDateRange)

        text = text + " - " + $(document).find('#cmbcreateview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#createdaterange').val();

    $('#posearchtitle').text(text);
    $("#posearchListul li").removeClass("activeState");
    $("#posearchListul li").eq(0).addClass('activeState');
    localStorage.setItem("PURCHASEORDERSTATUS", daterangeval);
});
//V2-364
$(document).on('change', '#cmbcompleteview', function (e) {
    var thielement = $(this);
    CustomQueryDisplayId = thielement.val();
    if (thielement.val() == CompleteDateRange) {
        var strtlocal = localStorage.getItem('POCompleteStartDateVw');
        if (strtlocal) {
            CompleteStartDateVw = strtlocal;
        }
        else {
            CompleteStartDateVw = today;
        }
        var endlocal = localStorage.getItem('POCompleteEndDateVw');
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
        });
    }
    else {
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        localStorage.removeItem('POCompleteStartDateVw');
        localStorage.removeItem('POCompleteEndDateVw');
        $(document).find('#timeperiodcontainer').hide();
    }
});
$(document).on('click', '#btntimeperiod', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbcompleteview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != CompleteDateRange) {
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        localStorage.removeItem('POCompleteStartDateVw');
        localStorage.removeItem('POCompleteEndDateVw');
    }
    else {
        localStorage.setItem('POCompleteStartDateVw', CompleteStartDateVw);
        localStorage.setItem('POCompleteEndDateVw', CompleteEndDateVw);
    }
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#advsearchfilteritems").html('');
    $(document).find('#txtColumnSearch').val('');
    dtTable.page('first').draw('page');
    $(document).find('#DateRangeModal').modal('hide');
    var text = $('#posearchListul').find('li').eq(2).text();

    //v2 371 Point 3
    //-------------------------------------------------------
    if (daterangeval != CompleteDateRange)
        text = text + " - " + $(document).find('#cmbcompleteview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#completedaterange').val();
    //-------------------------------------------------------

    $('#posearchtitle').text(text);
    $("#posearchListul li").removeClass("activeState");
    $("#posearchListul li").eq(2).addClass('activeState');
    localStorage.setItem("PURCHASEORDERSTATUS", daterangeval);
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
    var data = localStorage.getItem("PURCHASEORDERSTATUS");
    //#comented as search is not working when localStorage.getItem("PURCHASEORDERSTATUS") is null
    //if ($("#posearchListul > #6").hasClass('activeState') == false)
    //    $(document).find('#cmbcompleteview').val('').trigger('change');
    //if ($("#posearchListul > #0").hasClass('activeState') == false)
    //    $(document).find('#cmbcreateview').val('').trigger('change');
    //#endregion
    //$('#posearchtitle').text(getResourceValue("PurchaseOrderAlert"));
    //$(".searchList li").removeClass("activeState");
    //$("#posearchListul li").eq(data).addClass('activeState');
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        //Add on 02/07/2020 Problem with Searching DateRange Modal PopUP For All Status & Complete Dropdown Item
        run = true;
        CustomQueryDisplayId = data;
        //Add on 02/07/2020 Problem with Searching DateRange Modal PopUP For All Status & Complete Dropdown Item
        dtTable.page('first').draw('page');
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
}
//#endregion
//#region New Search button
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'PurchaseOrder' },
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
    var data = localStorage.getItem("PURCHASEORDERSTATUS");
    if (data) {
        CustomQueryDisplayId = data;
    }
    $.ajax({
        url: '/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'PurchaseOrder', searchText: txtSearchval, isClear: isClear },
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
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
            else if (item.key == 'advcompletedaterange' && item.value !== '') {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    searchitemhtml = searchitemhtml + '<span style="display:none;" class="label label-primary tagTo" id="_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
            if (item.key == 'CompleteDate') {
                $("#CompleteDate").trigger('change.select2');
            }


            advcountercontainer.text(selectCount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);

}
//#endregion


//#region V2 667
//$(document).on('click', '#btnPOPartNotInventorySave', function (e) {
//    var form = $(document).find('#frmPOPartNotInInventoryId');
//    if (form.valid() === false) {
//        return;
//    }
//    e.preventDefault();
//    var ChargeType = $(document).find('#lineItem_ChargeType').val();
//    if (ChargeType != 'WorkOrder') {
//        CancelAlertSetting.text = getResourceValue("ChargeItemNotWorkOrderContinueAlert");

//        swal(CancelAlertSetting, function (isConfirm) {
//            if (isConfirm == true) {
//                $(document).find('#frmPOPartNotInInventoryId').submit();
//            }
//            else {
//                return false;
//            }
//        });
//    }
//    else {
//        $(document).find('#frmPOPartNotInInventoryId').submit();
//    }
//});
//#region V2-653
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
$(document).on('click', '#btnPOPartNotInventorySave', function (e) {
    var form = $(document).find('#frmPOPartNotInInventoryId');
    if (form.valid() === false) {
        return;
    }
    e.preventDefault();
    var ChargeType = $(document).find('#ChargeType').val();
    if (ChargeType != 'WorkOrder') {
        //CancelAlertSetting.text = getResourceValue("ChargeItemNotWorkOrderContinueAlert");

        ChargeTypeCancelAlertSetting.closeOnConfirm = false;
        swal(ChargeTypeCancelAlertSetting, function (isConfirm) {
            if (isConfirm == true) {
                $(document).find('#frmPOPartNotInInventoryId').submit();
            }
            else {
                ChargeTypeCancelAlertSetting.closeOnConfirm = true;
                return false;
            }
        });
        ChargeTypeCancelAlertSetting.closeOnConfirm = true;
    }
    else {
        $(document).find('#frmPOPartNotInInventoryId').submit();
    }
});
//#endregion
//$(document).on('click', '#btnPOEditLineItemSave', function (e) {
//    var form = $(document).find('#frmEditPOLineItemId');
//    if (form.valid() === false) {
//        return;
//    }
//    e.preventDefault();
//    var ChargeType = $(document).find('#lineItem_ChargeType').val();
//    if (ChargeType != 'WorkOrder') {
//        CancelAlertSetting.text = getResourceValue("ChargeItemNotWorkOrderContinueAlert");

//        swal(CancelAlertSetting, function (isConfirm) {
//            if (isConfirm == true) {
//                $(document).find('#frmEditPOLineItemId').submit();
//            }
//            else {
//                return false;
//            }
//        });
//    }
//    else {
//        $(document).find('#frmEditPOLineItemId').submit();
//    }
//});
//#region V2-653
// 
$(document).on('click', '#btnPOEditLineItemSave', function (e) {
    if ($(document).find('#ChargeType').length > 0) {
        // this code will be executed only for Part not in inventory 
        // but will not be executed for Part in inventory
        var form = $(document).find('#frmPOLineItemEditDynamic');
        if (form.valid() === false) {
            return;
        }
        e.preventDefault();
        var ChargeType = $(document).find('#ChargeType').val();
        if (ChargeType != 'WorkOrder') {
            //CancelAlertSetting.text = getResourceValue("ChargeItemNotWorkOrderContinueAlert");

            ChargeTypeCancelAlertSetting.closeOnConfirm = false;
            swal(ChargeTypeCancelAlertSetting, function (isConfirm) {
                if (isConfirm == true) {
                    $(document).find('#frmPOLineItemEditDynamic').submit();
                }
                else {
                    ChargeTypeCancelAlertSetting.closeOnConfirm = true;
                    return false;
                }
            });
            ChargeTypeCancelAlertSetting.closeOnConfirm = true;
        }
        else {
            $(document).find('#frmPOLineItemEditDynamic').submit();
        }
    }
});
//#endregion
//#endregion
//#region V2-853 Reset Grid
$('#liResetGridClearBtn').click(function (e) {
    CancelAlertSetting.text = getResourceValue("ResetGridAlertMessage");
    swal(CancelAlertSetting, function () {
        var localstorageKeys = [];
        localstorageKeys.push("PURCHASEORDERSTATUS");
        localstorageKeys.push("postatustext");
        localstorageKeys.push("POCreateStartDateVw");
        localstorageKeys.push("POCreateEndDateVw");
        localstorageKeys.push("POCompleteStartDateVw");
        localstorageKeys.push("POCompleteEndDateVw");
        DeleteGridLayout('PurchaseOrder_Search', dtTable, localstorageKeys);
        GenerateSearchList('', true);
        window.location.href = "../Purchasing/Index?page=Procurement_Orders";
    });
});
//#endregion

//#region V2-946
$(document).on('click', '#PrintPOCheckList', function () {
    if (SelectPOId.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var jsonResult = {
            "list": SelectPOId
        };
        if ($("#ClientIdForPrint").val() == 4) {
            $.ajax({
                url: '/Purchasing/PrintPOListFromIndex',
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
                        PdfPrintAllPOList(result.pdf);
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
                    $(document).find('.chkPOsearch').prop('checked', false);
                    $('.itemcount').text(0);
                    SelectPOId = [];
                }
            });
        }
        else {
            $.ajax({
                url: '/Purchasing/SetPrintPoListFromIndex',
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
                        window.open("/Purchasing/GeneratePurchaseOrderPrint", "_blank");
                    }

                },
                complete: function () {
                    CloseLoader();
                    $(".updateArea").hide();
                    $(".actionBar").fadeIn();
                    $(document).find('.chkPOsearch').prop('checked', false);
                    $('.itemcount').text(0);
                    SelectPOId = [];
                }
            });
        }
    }
});
$(document).on('click', '#printPO', function () {
    var SelectPOId = [];
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    var ClientLookupId = $(document).find('#PurchaseOrderModel_ClientLookupId').val();
    var Status = $(document).find('#PurchaseOrderModel_Status').val();
    var item = new PurchaseOrderNotInSelectedItem(PurchaseOrderId, ClientLookupId, Status);
    SelectPOId.push(item);
    var jsonResult = {
        "list": SelectPOId
    };
    $.ajax({
        url: '/Purchasing/SetPrintPoListFromIndex',
        data: JSON.stringify(jsonResult),
        type: "POST",
        datatype: "json",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (result) {
            if (result.success) {
                window.open("/Purchasing/GeneratePurchaseOrderPrint", "_blank");
            }
        },
        complete: function () {
            CloseLoader();
            SelectPOId = [];
        }
    });
});
//#endregion
//#Region V2-1047
$(document).on('click', '#Modelconfirm', function () {
    var PurchaseOrderId = $(document).find('#PurchaseOrderModel_PurchaseOrderId').val();
    var StoreroomId = $(document).find("#PurchaseOrderModel_StoreroomId").val();
    $.ajax({
        url: '/Purchasing/Modelconfirm',
        data: { PurchaseOrderId: PurchaseOrderId },
        type: "POST",
        datatype: "html JSON",
        /* contentType: 'application/json; charset=utf-8',*/
        beforeSend: function (data) {
            ShowLoader();
        },
        success: function (data) {
            if (data.success) {
                SuccessAlertSetting.text = getResourceValue("SuccessAlertConvertPOtoPR");
                swal(SuccessAlertSetting, function () {
                    window.location.href = "../PurchaseRequest/DetailFromPurchaseOrder?PurchaseRequestId=" + data.purchaseRequestId;
                });
            } else {
                $('#DirectBuyLineItemPopUp').html(data);
                GenerateDirectBuyLineItemTableGrid(PurchaseOrderId);
                $(document).find('.select2picker').select2({});
                $('#DirectBuyLineItemModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
            }
        },
        complete: function () {
            CloseLoader();

        }
    });
});

function DirectBuyLineItemSuccess(data) {
    $('.modal-backdrop').remove();
    CloseLoader();
    if (data.Result === "success") {
        $(document).find('#DirectBuyLineItemModalpopup').hide();
        SuccessAlertSetting.text = getResourceValue("SuccessAlertConvertPOtoPR");
        swal(SuccessAlertSetting, function () {
            window.location.href = "../PurchaseRequest/DetailFromPurchaseOrder?PurchaseRequestId=" + data.purchaseRequestId;
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
var DirectBuyLineItemTable;
function GenerateDirectBuyLineItemTableGrid(PurchaseOrderId) {
    var chargeTypeList;
    if ($(document).find('#tblDirectBuyLineItem').hasClass('dataTable')) {
        DirectBuyLineItemTable.destroy();
    }
    DirectBuyLineItemTable = $("#tblDirectBuyLineItem").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: false,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "bPaginate": false,
        "order": [[1, "asc"]],
        stateSave: false,

        //language: {
        //    url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        //},
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Purchasing/DirectBuyLineItemsModelconfirm",
            "data": { PurchaseOrderId: PurchaseOrderId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                chargeTypeList = response.ChargeTypeList;
                return response.data;
            }
        },
        "columns":
            [

                {
                    "data": "Description", "autoWidth": true, "bSearchable": false, "bSortable": false,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-100'>" + data + "</div>";
                    }
                },
                { "data": "OrderQuantity", "autoWidth": true, "bSearchable": false, "bSortable": false, "className": "text-right" },
                { "data": "PurchaseUOM", "autoWidth": true, "bSearchable": false, "bSortable": false },
                {
                    "data": "UnitCost", "autoWidth": true, "bSearchable": false, "bSortable": false, "className": "text-right",
                    "mRender": function (data, type, row) {
                        return data.toFixed(2);
                    }
                },

                {
                    "data": "ChargeType", "autoWidth": true, "bSearchable": false, "bSortable": false,
                    "mRender": function (data, type, row, meta) {

                        var ChargeTypefieldIdName = "POLineItemList[" + meta.row + "].ChargeType"
                        var ChargeTypefieldId = "ChargeType_" + meta.row
                        var ChargeTypeIdDropDown = "<option value=''>" + "--Select--" + "</option>";
                        if (chargeTypeList.length > 0) {
                            for (var i = 0; i < chargeTypeList.length; i++) {
                                if (chargeTypeList[i].Value == row.ChargeType) {
                                    ChargeTypeIdDropDown = ChargeTypeIdDropDown + "<option selected value='" + chargeTypeList[i].Value + "'>" + chargeTypeList[i].Text + "</option>";
                                }
                                else {
                                    ChargeTypeIdDropDown = ChargeTypeIdDropDown + "<option value='" + chargeTypeList[i].Value + "'>" + chargeTypeList[i].Text + "</option>";
                                }
                            }
                        }
                        var ChargeTypeIdDropDownhtml = ' <select style="min-width: 117px;" name="' + ChargeTypefieldIdName + '" id="' + ChargeTypefieldId + '" class="form-control search adv-item select2picker search ddlChangeTypeForDirectbuy">"' + ChargeTypeIdDropDown + '"</select>'


                        return '<span class="inputText">' + ChargeTypeIdDropDownhtml + '</span>';
                    }
                },

                {
                    "data": "ChargeToClientLookupId", "autoWidth": true, "bSearchable": false, "bSortable": false,
                    "mRender": function (data, type, row, meta) {
                        var chargeTypeVal = row.ChargeType;
                        var TextFieldId = "ChargeToClientLookupId_" + meta.row;
                        var fieldId = "ChargeToId_" + meta.row;
                        var openChargeToModalCssClassName = "";
                        var clearChargeToModalCssClassName = "";
                        var fieldIdName = "POLineItemList[" + meta.row + "].ChargeToId"
                        var hdnNamePurchaseOrderLineItemId = "POLineItemList[" + meta.row + "].PurchaseOrderLineItemId";
                        var hdnidPurchaseOrderLineItemId = "PurchaseOrderLineItemId_" + meta.row;
                        var hdnNamePurchaseOrderId = "POLineItemList[" + meta.row + "].PurchaseOrderId";
                        var hdnidPurchaseOrderId = "PurchaseOrderId_" + meta.row;
                        var TextFieldIdName = "POLineItemList[" + meta.row + "].ChargeToClientLookupId";
                        var innerHtmlAssetTree = '<div class="input-group-btn tree-full n-tree clschargeToidTree"> <i class="fa fa-tree chargeTree TreeAdjustNew imgChargeToTreeLineItemDynamic" id="imgChargeToTreeLineItemDynamic" data-imgChargeToBtn="imgChargeToBtnModalPopupGridData_' + meta.row + '" data-textfield="' + TextFieldId + '" data-valuefield="' + fieldId + '"></i></div >'
                        if (chargeTypeVal == "Account") {
                            openChargeToModalCssClassName = "OpenAccountModalPopupGrid";
                            clearChargeToModalCssClassName = "ClearAccountModalPopupGridData";
                        }
                        if (chargeTypeVal == "WorkOrder") {
                            openChargeToModalCssClassName = "OpenWorkOrderModalPopupGrid";
                            clearChargeToModalCssClassName = "ClearWorkOrderModalPopupGridData";
                        }
                        if (chargeTypeVal == "Equipment") {
                            openChargeToModalCssClassName = "OpenEquipmentModalPopupGrid";
                            clearChargeToModalCssClassName = "ClearEquipmentModalPopupGridData";
                            innerHtmlAssetTree = "";
                            innerHtmlAssetTree = '<div class="input-group-btn tree-full n-tree clschargeToidTree"> <i class="fa fa-tree chargeTree TreeAdjustNew imgChargeToTreeLineItemDynamic" id="imgChargeToTreeLineItemDynamic" data-imgChargeToBtn="imgChargeToBtnModalPopupGridData_' + meta.row + '" style="display: block;" data-textfield="' + TextFieldId + '" data-valuefield="' + fieldId + '"></i></div >'
                        }
                        var innerhtmlOpen = ' <div class="input-group-btn"><button id = "OpenChargeToModalPopupGrid" class="btn btn-white btntxtInputGroup ' + openChargeToModalCssClassName + '"  type = "button" data-OpenChargeToBtn="OpenBtnToBtnModalPopupGridData_' + meta.row + '" data-textfield="' + TextFieldId + '" data-valuefield="' + fieldId + '" >  <i id="srcicon" class="fa fa-search"></i> </button ></div >'
                        var innerhtmlclear = "";
                        var innerhtmlInputText = "";
                        if (row.ChargeToClientLookupId != "") {
                            innerhtmlclear = '<div class="input-group-btn"><button id = "ClearChargeToModalPopupGridData"  class="btn btn-white btntxtInputGroup  ' + clearChargeToModalCssClassName + ' " type = "button" data-clearcrossbtn="ClearCrossBtnModalPopupGridData_' + meta.row + '"  data-valuefield="' + fieldId + '" data-textfield="' + TextFieldId + '" ><i id="srcicon" class="fa fa-close"></i></button ></div >'
                            innerhtmlInputText = ' <input style="width: 100%;min-width: 60px;display:none" type="text" value="' + row.ChargeToId + '" name="' + fieldIdName + '" autocomplete = "off" readonly = "readonly" class="form-control search dropbtn txtclientlookupcls readonly" id=' + fieldId + ' ></input>   <input style="width: 100%;min-width: 60px" type="text" name="' + TextFieldIdName + '" value="' + row.ChargeToClientLookupId + '" autocomplete = "off" readonly = "readonly" class="form-control search dropbtn txtclientlookupcls readonly"  id="' + TextFieldId + '" ></input> '
                        } else {
                            innerhtmlclear = '<div class="input-group-btn"><button id = "ClearChargeToModalPopupGridData" class="btn btn-white btntxtInputGroup  ' + clearChargeToModalCssClassName + ' " type = "button" data-clearcrossbtn="ClearCrossBtnModalPopupGridData_' + meta.row + '"  data-textfield="' + TextFieldId + '" data-valuefield="' + fieldId + '" ><i id="srcicon" class="fa fa-close"></i></button ></div >'
                            innerhtmlInputText = ' <input style="width: 100%;min-width: 60px" type="text" autocomplete = "off" readonly = "readonly"  class="form-control search dropbtn txtclientlookupcls readonly" id="' + fieldId + '"></input>   <input type="text" style="width: 100%;min-width: 60px;display:none" autocomplete = "off"  readonly = "readonly" class="form-control search dropbtn txtclientlookupcls readonly"  id="' + TextFieldId + '" ></input> '

                        }
                        return '<input type="hidden" value="' + row.PurchaseOrderLineItemId + '" name="' + hdnNamePurchaseOrderLineItemId + '" class="form-control search dropbtn txtclientlookupcls readonly" id="' + hdnidPurchaseOrderLineItemId + '" ></input> <input type="hidden" value="' + row.PurchaseOrderId + '" name="' + hdnNamePurchaseOrderId + '" class="form-control search dropbtn txtclientlookupcls readonly" id="' + hdnidPurchaseOrderId + '" ></input>  <span class="inputText"><div class="dropdown dropTableOuter tooltip-box"><div class="tooltipForDirectBuyLineItem" id="tooltip' + TextFieldId + '"></div><div class="input-group four-col-btn">' + innerhtmlInputText + innerhtmlclear + innerhtmlOpen + innerHtmlAssetTree + '</div></div></span>';
                    }
                },

                {
                    "data": "AccountClientLookupId", "autoWidth": true, "bSearchable": false, "bSortable": false,
                    "mRender": function (data, type, row, meta) {
                        var TextFieldId = "AccountClientLookupId_" + meta.row;
                        var fieldId = "AccountId_" + meta.row;
                        var fieldIdName = "POLineItemList[" + meta.row + "].AccountId"
                        var TextFieldIdName = "POLineItemList[" + meta.row + "].AccountClientLookupId";

                        var innerhtmlOpen = ' <div class="input-group-btn"><button id = "OpenAccountModalPopupGrid" class="btn btn-white btntxtInputGroup OpenAccountModalPopupGrid"  type = "button" data-OpenAccountToBtn="OpenBtnToBtnModalPopupGridData_' + meta.row + '" data-textfield="' + TextFieldId + '" data-valuefield="' + fieldId + '" >  <i id="srcicon" class="fa fa-search"></i> </button ></div >'
                        var innerhtmlclear = "";
                        var innerhtmlInputText = "";
                        if (row.AccountClientLookupId != "") {
                            innerhtmlclear = '<div class="input-group-btn"><button id = "ClearAccountModalPopupGridData" class="btn btn-white btntxtInputGroup ClearAccountModalPopupGridData" type = "button" data-valuefield="' + fieldId + '" data-textfield="' + TextFieldId + '" ><i id="srcicon" class="fa fa-close"></i></button ></div >'
                            innerhtmlInputText = ' <input style="width: 100%;min-width: 60px;display:none" type="text"  value="' + row.AccountId + '" name="' + fieldIdName + '" autocomplete = "off" readonly = "readonly" class="form-control search dropbtn txtclientlookupcls readonly" id=' + fieldId + ' ></input>   <input style="width: 100%;min-width: 60px" type="text" name="' + TextFieldIdName + '" value="' + row.AccountClientLookupId + '" autocomplete = "off" readonly = "readonly" class="form-control search dropbtn txtclientlookupcls readonly"  id="' + TextFieldId + '" ></input> '
                        } else {
                            innerhtmlclear = '<div class="input-group-btn"><button id = "ClearAccountModalPopupGridData" class="btn btn-white btntxtInputGroup  ClearAccountModalPopupGridData " type = "button" data-textfield="' + TextFieldId + '" data-valuefield="' + fieldId + '" ><i id="srcicon" class="fa fa-close"></i></button ></div >'
                            innerhtmlInputText = ' <input style="width: 100%;min-width: 60px" type="text" autocomplete = "off" readonly = "readonly"  class="form-control search dropbtn txtclientlookupcls readonly" id="' + fieldId + '"></input>   <input type="text"  autocomplete = "off" style="width: 100%;min-width: 60px;display:none" readonly = "readonly" class="form-control search dropbtn txtclientlookupcls readonly"  id="' + TextFieldId + '" ></input> '

                        }
                        return '<span class="inputText"><div class="dropdown dropTableOuter tooltip-box"><div class="tooltipForDirectBuyLineItem" id="tooltip' + TextFieldId + '"></div><div class="input-group four-col-btn">' + innerhtmlInputText + innerhtmlclear + innerhtmlOpen + '</div></div></span>';

                    }
                }


            ],
        "info": false,
        initComplete: function () {
            SetPageLengthMenu();
            $(document).find('.ddlChangeTypeForDirectbuy').select2({});
        }
    });
}
$(document).on('change', '.ddlChangeTypeForDirectbuy', function () {
    var ids = this.id.split('_');
    var textfield = "#ChargeToClientLookupId_" + ids[1];
    var valuefield = "#ChargeToId_" + ids[1];
    $(textfield).val('');
    $(valuefield).val('');
    $('[data-ClearCrossBtn=ClearCrossBtnModalPopupGridData_' + ids[1] + ']').css('display', 'none');
    $('[data-imgChargeToBtn=imgChargeToBtnModalPopupGridData_' + ids[1] + ']').css('display', 'none');
    if ($('[data-OpenChargeToBtn=OpenBtnToBtnModalPopupGridData_' + ids[1] + ']').hasClass('OpenAccountModalPopupGrid')) {
        $('[data-OpenChargeToBtn=OpenBtnToBtnModalPopupGridData_' + ids[1] + ']').removeClass('OpenAccountModalPopupGrid');
    }
    if ($('[data-ClearCrossBtn=ClearCrossBtnModalPopupGridData_' + ids[1] + ']').hasClass('ClearAccountModalPopupGridData')) {
        $('[data-ClearCrossBtn=ClearCrossBtnModalPopupGridData_' + ids[1] + ']').removeClass('ClearAccountModalPopupGridData');
    }

    if ($('[data-OpenChargeToBtn=OpenBtnToBtnModalPopupGridData_' + ids[1] + ']').hasClass('OpenWorkOrderModalPopupGrid')) {
        $('[data-OpenChargeToBtn=OpenBtnToBtnModalPopupGridData_' + ids[1] + ']').removeClass('OpenWorkOrderModalPopupGrid');
    }
    if ($('[data-ClearCrossBtn=ClearCrossBtnModalPopupGridData_' + ids[1] + ']').hasClass('ClearWorkOrderModalPopupGridData')) {
        $('[data-ClearCrossBtn=ClearCrossBtnModalPopupGridData_' + ids[1] + ']').removeClass('ClearWorkOrderModalPopupGridData');
    }

    if ($('[data-OpenChargeToBtn=OpenBtnToBtnModalPopupGridData_' + ids[1] + ']').hasClass('OpenEquipmentModalPopupGrid')) {
        $('[data-OpenChargeToBtn=OpenBtnToBtnModalPopupGridData_' + ids[1] + ']').removeClass('OpenEquipmentModalPopupGrid');
    }
    if ($('[data-ClearCrossBtn=ClearCrossBtnModalPopupGridData_' + ids[1] + ']').hasClass('ClearEquipmentModalPopupGridData')) {
        $('[data-ClearCrossBtn=ClearCrossBtnModalPopupGridData_' + ids[1] + ']').removeClass('ClearEquipmentModalPopupGridData');
    }

    var type = $(this).val();
    if (type == "") {
        $('[data-imgChargeToBtn=imgChargeToBtnModalPopupGridData_' + ids[1] + ']').hide();
        return;
    }
    if (type == 'Account') {
        $('[data-OpenChargeToBtn=OpenBtnToBtnModalPopupGridData_' + ids[1] + ']').addClass('OpenAccountModalPopupGrid');
        $('[data-ClearCrossBtn=ClearCrossBtnModalPopupGridData_' + ids[1] + ']').addClass('ClearAccountModalPopupGridData');
        return;
    }
    if (type == 'WorkOrder') {
        $('[data-OpenChargeToBtn=OpenBtnToBtnModalPopupGridData_' + ids[1] + ']').addClass('OpenWorkOrderModalPopupGrid');
        $('[data-ClearCrossBtn=ClearCrossBtnModalPopupGridData_' + ids[1] + ']').addClass('ClearWorkOrderModalPopupGridData');
        return;
    }
    if (type == "Equipment") {
        $('[data-OpenChargeToBtn=OpenBtnToBtnModalPopupGridData_' + ids[1] + ']').addClass('OpenEquipmentModalPopupGrid');
        $('[data-ClearCrossBtn=ClearCrossBtnModalPopupGridData_' + ids[1] + ']').addClass('ClearEquipmentModalPopupGridData');
        $('[data-imgChargeToBtn=imgChargeToBtnModalPopupGridData_' + ids[1] + ']').css('display', 'block');
        return;
    }


});
var tooltip = "";
$(document).on('mouseover', '.txtclientlookupcls', function () {

    var inputField = document.getElementById(this.id);
    tooltip = document.getElementById("tooltip" + this.id);
    showTooltip(inputField);


});
$(document).on('mouseout', '.txtclientlookupcls', function () {
    hideTooltip();

});
function showTooltip(inputField) {
    // Get the input text value
    if (inputField.value.length > 4) {
        var text = inputField.value;

        // Set the tooltip content to the input text value
        tooltip.innerText = text;

        // Position the tooltip relative to the input field
        var inputRect = inputField.getBoundingClientRect();
        tooltip.style.left = inputRect.left + "px";
        tooltip.style.top = (inputRect.top - tooltip.offsetHeight - 5) + "px";

        // Show the tooltip
        tooltip.style.display = "block";
    }

}

function hideTooltip() {
    // Hide the tooltip
    tooltip.style.display = "none";
}

//#region V2-1032 SingleStockLineItem
var lineitemlookupListPOTable;
//#region Add
function GoToAddPartInInventorySingleStock(PurchaseOrderId) {
    var StoreroomId = $(document).find("#PurchaseOrderModel_StoreroomId").val();
    $.ajax({
        url: "/Purchasing/AddPartInInventorySingleStockLineItemDynamic",
        type: "POST",
        dataType: "html",
        data: { PurchaseOrderId: PurchaseOrderId, StoreroomId: StoreroomId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpurchasing').html(data);
        },
        complete: function () {
            CloseLoader();
            SetPOControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}


$(document).on('click', '#btnPOPartInInventorySingleStockSave', function (e) {
    var form = $(document).find('#frmPOPartInInventorySingleStockDynamic');
    if (form.valid() === false) {
        return;
    }
    e.preventDefault();
    var currentPartId = $(document).find('#PartId').val();
    var currentPartClientLookupId = $(document).find('#PartClientLookupId').val();
    var currentPartDesc = $(document).find('#PartClientLookupId').text();
    var isOnOrderCheck = $(document).find('#AddPOLineItemPartInInventory_IsOnOderCheck').val();
    if (isOnOrderCheck=="True") {
        generateChecklineitemlookupListPOTable(currentPartClientLookupId, currentPartDesc, currentPartId, Action = "Save")
    }
    else {
        $(document).find('#frmPOPartInInventorySingleStockDynamic').submit();
    }

});

function AddPartInInventorySingleStockOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        SuccessAlertSetting.text = getResourceValue("LineItemAddedAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPODetail(data.PurchaseOrderId, "overview");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

//#endregion
//#region Edit
$(document).on('click', '#btnPOEditLineItemSingleStockSave', function (e) {
    var form = $(document).find('#frmPOLineItemEditSingleStockDynamic');
        if (form.valid() === false) {
            return;
        }
        e.preventDefault();
    var currentPartId = $(document).find('#PartId').val();
    var currentPartClientLookupId = $(document).find('#PartClientLookupId').val();
    var currentPartDesc = $(document).find('#PartClientLookupId').text();
    var isOnOrderCheck = $(document).find('#EditPOLineItemPartInInventorySingleStock_IsOnOderCheck').val();
    if (isOnOrderCheck=="True") {
        generateChecklineitemlookupListPOTable(currentPartClientLookupId, currentPartDesc, currentPartId, Action = "Update")
    }
    else {
        $(document).find('#frmPOLineItemEditSingleStockDynamic').submit();
    }
    
});

//#endregion
function generateChecklineitemlookupListPOTable(PartClientLookupId, PartDesc, PartId, Action) {
    if ($(document).find('#LineitemlookupListPOTable').hasClass('dataTable')) {
        lineitemlookupListPOTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    lineitemlookupListPOTable = $("#LineitemlookupListPOTable").DataTable({
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
                $(document).find('#popLineitemlookupListPOHeader').text("");
                var ExistingPRPOForPartMessage = getResourceValue("globalExistingPRPOForPart");
                $(document).find('#popLineitemlookupListPOHeader').text(ExistingPRPOForPartMessage + PartClientLookupId + " - " + PartDesc)
                $(document).find('#LineitemlookupListPOModal').modal({ backdrop: 'static', keyboard: false, show: true });
                /* }*/
            }
            else {
                if (Action == "Save")
                    $(document).find('#frmPOPartInInventorySingleStockDynamic').submit();
                else
                    $(document).find('#frmPOLineItemEditSingleStockDynamic').submit();

            }
        }
    });
}

$(document).on('click', '#addPOPartfromLineItemSSPopUp', function () {
    var formid=$(document).find('form')[0].id;
    $(document).find('#LineitemlookupListPOModal').modal('hide');
    if (formid =="frmPOPartInInventorySingleStockDynamic")
        $(document).find('#frmPOPartInInventorySingleStockDynamic').submit();
    else 
        $(document).find('#frmPOLineItemEditSingleStockDynamic').submit();

});

$(document).on('click', '#btncancelfromLineItemPOPopUp', function () {
    $(document).find('#LineitemlookupListPOModal').modal('hide');
})

//#endregion

//#region V2-1124 setting description, issueunit, appliedcost according to the selected part
$(document).on('click', '.link_xrefpart_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtXrefPartsTable.row(row).data();
    $(document).find('#Description').val(data.Description);
    $(document).find('#UnitCost').val(data.AppliedCost);
    $(document).find('#UnitOfMeasure').val(data.IssueUnit).trigger('change');
});
//#endregion