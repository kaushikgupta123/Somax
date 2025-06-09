var sanitationJobSearchdt;
var statussearchval;
var typeValStatus;
var typeValCreatedBy;
var typeValAssigned;
var typeValVerified;
var run = false;


$(function () {
    $(".updateArea").hide();
    ShowbtnLoaderclass("LoaderDrop");
    ShowbtnLoader("sanbtnsortmenu");
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
    $('#sansidebarCollapse').on('click', function () {
        $('.sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    var tabtype = localStorage.getItem("TabType");
    if (tabtype == "SaitationRequest" || ($('#Sanitation').is(':visible') == true || $('#Maintenance').length == 0)) {
        var sanitationjobstatus = localStorage.getItem("SANITATIONJOBSTATUSONLY");
        if (sanitationjobstatus || tabtype == "SaitationRequest") {
            generateSJDataTable();
            statussearchval = sanitationjobstatus;
            $(document).find('#Sanitationtab').addClass('active');
            $(document).find('#Maintenancetab').removeClass('active');
            $('#SanitationList').val(sanitationjobstatus).trigger('change.select2');
        }
        else {
            if (!$("#IsJobAddFromDashboard").val()) {
                statussearchval = 0;
                generateSJDataTable();
            }
        }      
        $('#Maintenance').hide();
        $('#Sanitation').show();
        
    }

   
    $(document).find('.select2picker').select2({});
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



    //V2-1100
    mobiscroll.settings = {
        lang: 'en',                                       // Specify language like: lang: 'pl' or omit setting to use default
        theme: 'ios',                                     // Specify theme like: theme: 'ios' or omit setting to use default
        themeVariant: 'light'                             // More info about themeVariant: https://docs.mobiscroll.com/4-10-9/javascript/popup#opt-themeVariant
    };

    CustomQueryDropdown = $('#SanCustomQueryDropdownDiv').mobiscroll().popup({
        display: 'bubble',
        anchor: '#ShowCustomQueryDropdown',
        buttons: [],
        cssClass: 'mbsc-no-padding md-vertical-list'
    }).mobiscroll('getInst');
    SortByDropdown = $('#SortByDropdownDiv').mobiscroll().popup({
        display: 'bubble',
        anchor: '#ShowSortByDropdown',
        buttons: [],
        cssClass: 'mbsc-no-padding md-vertical-list'
    }).mobiscroll('getInst');

    $('#SanCustomQueryDropdown').mobiscroll().listview({
        enhance: true,
        swipe: false,
    });
    $('#SortByDropdown').mobiscroll().listview({
        enhance: true,
        swipe: false
    });

});

//#region Search
$(document).on('change', '#SanitationList', function () {
    run = true;
    var optionval = $('#SanitationList option:selected').val();
    typeValStatus = $("#Status").val();
    typeValAssigned = $("#Assigned").val();
    localStorage.setItem("SANITATIONJOBSTATUSONLY", optionval)
    statussearchval = optionval;
    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        sanitationJobSearchdt.page('first').draw('page');
    }

});

$("#btnSJDataAdvSrch").on('click', function (e) {
    searchresult = [];
    typeValStatus = $("#Status").val();
    typeValCreatedBy = $("#CreateBy").val();
    typeValAssigned = $("#Assigned").val();
    typeValVerified = $("#VeryfiedBy").val();
    SJAdvSearch();
    sanitationJobSearchdt.page('first').draw('page');
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
});
function SJAdvSearch() {
    var InactiveFlag = false;
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
    $("#sanadvsearchfilteritems").html(searchitemhtml);
    $(".sanfilteritemcount").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}
$(document).on('click', '#sanliClearAdvSearchFilter', function () {
    run = true;
    $("#SanitationList").val(0).trigger('change.select2');
    statussearchval = 0;
    $(document).find('#Extracted').val("").trigger('change');
    localStorage.removeItem("SANITATIONJOBSTATUSONlY");
    clearAdvanceSearch();
    SJAdvSearch();
    sanitationJobSearchdt.page('first').draw('page');
});
$(document).on('click', '.btnCross', function () {
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    $(".sanfilteritemcount").text(selectCount);
    if (searchtxtId == "Status") {
        typeValStatus = null;
    }
    if (searchtxtId == "CreateBy") {
        typeValCreatedBy = null;
    }
    if (searchtxtId == "Assigned") {
        typeValAssigned = null;
    }
    if (searchtxtId == "VeryfiedBy") {
        typeValVerified = null;
    }
    sanitationJobSearchdt.page('first').draw('page');
});
function clearAdvanceSearch() {
    selectCount = 0;
    $("#JobId").val("");
    $("#Description").val("");
    $("#ChargeTo").val("");
    $("#ChargeToName").val("");
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
    $("#sanadvsearchfilteritems").html('');
    $(".sanfilteritemcount").text(selectCount);
    typeValStatus = $("#Status").val();
    typeValCreatedBy = $("#CreateBy").val();
    typeValAssigned = $("#Assigned").val();
    typeValVerified = $("#VeryfiedBy").val();
}
var order_sanitation = '1';
var orderDir_sanitation = 'asc';
function generateSJDataTable() {
    
    var printCounter = 0;
    if ($(document).find('#sanitationJobSearchTable').hasClass('dataTable')) {
        sanitationJobSearchdt.destroy();
    }
    sanitationJobSearchdt = $('#sanitationJobSearchTable').DataTable({
        colReorder: {
            fixedColumnsLeft: 1
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "desc"]],
        stateSave: true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = order_sanitation;
                    data.order[0][1] = orderDir_sanitation;
                }
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "SanitationJob_Dashboard_Search",
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
            if (localStorage.getItem("TabType")) {
                callback("");
            }
            else {
                $.ajax({
                    "url": gridStateLoadUrl,
                    "data": {
                        GridName: "SanitationJob_Dashboard_Search"
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
            }
            localStorage.removeItem("TabType");
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
                title: 'Data Export'
            },
            {
                extend: 'print',
                title: 'Data Export'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Data Export',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Data Export',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Dashboard/GetSantGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.CustomQueryDisplayId = statussearchval;
                d.ClientLookupId = LRTrim($("#JobId").val());
                d.Description = LRTrim($("#Description").val());
                d.ChargeTo_ClientLookupId = LRTrim($("#ChargeTo").val());
                d.ChargeTo_Name = LRTrim($('#ChargeToName').val());
                d.Status = LRTrim($("#Status").val());
                d.CreateDate = ValidateDate($("#Created").val());
                d.Assigned = LRTrim($("#Assigned").val());
                d.CompleteDate = LRTrim(ValidateDate($("#CompleteDate").val()));
            },
            "dataSrc": function (result) {
                let colOrder = sanitationJobSearchdt.order();
                orderDir_sanitation = colOrder[0][1];
                $("#Status").empty();
                $("#Status").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < result.statusList.length; i++) {
                    var id = result.statusList[i];
                    var name = result.statusList[i];
                    $("#Status").append("<option value='" + id + "'>" + name + "</option>");
                }
                if (typeValStatus) {
                    $("#Status").val(typeValStatus);
                }                

                $("#Assigned").empty();
                $("#Assigned").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < result.assignedList.length; i++) {
                    var id = result.assignedList[i];
                    var name = result.assignedList[i];
                    $("#Assigned").append("<option value='" + id + "'>" + name + "</option>");

                }
                if (typeValAssigned) {
                    $("#Assigned").val(typeValAssigned);
                }              
                /*HidebtnLoader("sanbtnsortmenu");*/
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
                "data": "ClientLookupId",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "className": "text-left",
                "name": "0"

            },
            {
                "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": true, "name": "1",
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-300'>" + data + "</div>";
                }
            },
            { "data": "ChargeTo_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2", },
                { "data": "ChargeTo_Name", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
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
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date", "name": "5" },

            {
                "data": "Assigned", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6",
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
                "name": "7"
            }
        ],
        //"columnDefs": [
        //    {
        //        targets: [0],
        //        className: 'noVis'
        //    }

        //],
        initComplete: function () {
            SetPageLengthMenu();
            //var extSanit = $(document).find('#sanitationJobSearchModel_ExternalSanitation').val();
            //if (extSanit == "False") {
            //    sanitationJobSearchdt.column('colExtracted:name').visible(false);
            //    sanitationJobSearchdt.column('colScheduledDate:name').visible(true);                         
            //}
            //else {
            //    sanitationJobSearchdt.column('colExtracted:name').visible(true);
            //    sanitationJobSearchdt.column('colScheduledDate:name').visible(false);
            //               }
            //var currestsortedcolumn = $('#sanitationJobSearchTable').dataTable().fnSettings().aaSorting[0][0];
            //var column = this.api().column(currestsortedcolumn);
            //var columnId = $(column.header()).attr('id');
            //switch (columnId) {
            //    case "thJobId":
            //        EnableJobIdColumnSorting();
            //        break;
            //    case "thJobDesc":
            //        EnableJobDescColumnSorting();
            //        break;
            //    case "thJobChargeTo":
            //        EnableJobChargeToColumnSorting();
            //        break;
            //    case "thJobStatus":
            //        EnableJobStatusColumnSorting();
            //        break;
            //}
           /* $('#sanbtnsortmenu').text(getResourceValue("spnSorting") + " : " + column.header().innerHTML);*/
            $("#SanitationGridAction :input").removeAttr("disabled");
            $("#SanitationGridAction :button").removeClass("disabled");
            //V2-834
            _isLoggedInFromMobile = CheckLoggedInFromMob();
            if (_isLoggedInFromMobile === true) {
                $(".import-export").remove();
                $("#AddSanitationRequestMbl").css('display', '');
            }
        }
    });
}

$(document).on('click', '#sanitationJobSearchTable_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#sanitationJobSearchTable_length .searchdt-menu', function () {
    run = true;
});
//function EnableJobIdColumnSorting() {
//    $('.DTFC_LeftWrapper').find('#thJobId').css('pointer-events', 'auto');
//    document.getElementById('thJobDesc').style.pointerEvents = 'none';
//    document.getElementById('thJobChargeTo').style.pointerEvents = 'none';
//    document.getElementById('thJobStatus').style.pointerEvents = 'none';
//}
//function EnableJobDescColumnSorting() {
//    $(document).find('.th-JobId').css('pointer-events', 'none');
//    document.getElementById('thJobDesc').style.pointerEvents = 'auto';
//    document.getElementById('thJobChargeTo').style.pointerEvents = 'none';
//    document.getElementById('thJobStatus').style.pointerEvents = 'none';
//}
//function EnableJobChargeToColumnSorting() {
//    $(document).find('.th-JobId').css('pointer-events', 'none');
//    document.getElementById('thJobDesc').style.pointerEvents = 'none';
//    document.getElementById('thJobChargeTo').style.pointerEvents = 'auto';
//    document.getElementById('thJobStatus').style.pointerEvents = 'none';
//}
//function EnableJobStatusColumnSorting() {
//    $(document).find('.th-JobId').css('pointer-events', 'none');
//    document.getElementById('thJobDesc').style.pointerEvents = 'none';
//    document.getElementById('thJobChargeTo').style.pointerEvents = 'none';
//    document.getElementById('thJobStatus').style.pointerEvents = 'auto';
//}
//$(document).find('.srtsanitationcolumn').click(function () {
//    ShowbtnLoader("sanbtnsortmenu");
//    var col = $(this).data('col');
//    switch (col) {
//        case 0:
//            EnableJobIdColumnSorting();
//            $('.DTFC_LeftWrapper').find('#thJobId').trigger('click');
//            break;
//        case 1:
//            EnableJobDescColumnSorting();
//            $('#thJobDesc').trigger('click');
//            break;
//        case 2:
//            EnableJobChargeToColumnSorting();
//            $('#thJobChargeTo').trigger('click');
//            break;
//        case 4:
//            EnableJobStatusColumnSorting();
//            $('#thJobStatus').trigger('click');
//            break;
//    }
//    $('#sanbtnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());
//    $(document).find('.srtsanitationcolumn').removeClass('sort-active');
//    $(this).addClass('sort-active');
//    run = true;
//});
$(function () {

});
$(document).on('change', '.chksearch', function () {
    var data = sanitationJobSearchdt.row($(this).parents('tr')).data();
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
            sanitationJobSearchdt.page('first').draw('page');
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
});
//#region ColumnVisibility
$(document).on('click', '#sanliCustomize', function () {
    funCustomizeBtnClick(sanitationJobSearchdt);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(sanitationJobSearchdt, colOrder);
    run = true;
    sanitationJobSearchdt.state.save(run);
});
//#endregion

////#region V2-1100 AddSanitationRequestMbl
$(document).on('click', "#AddSanitationRequestMbl", function (e) {
    e.preventDefault();
    var id = this.id;
    GoSanitationRequestDemandAndDescribe(id);
});
function GoSanitationRequestDemandAndDescribe(id) {
    $.ajax({
        url: "/Dashboard/AddSanitationRequestWR_Mobile",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddSanitationModal').html(data);
        },
        complete: function () {
            SetControls();
            $('#AddSanitationModalpopup').addClass('slide-active').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '.btnCancelSR', function () {

    var areaChargeToId = "";
    $(document).find('#AddSanitationModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });
    $('#AddSanitationModalpopup').removeClass('slide-active');
    $('#AddSanitationModal').html('');
});
// adding an error class and valid class should be done manually
function SetControls() {
    var errClass = 'mobile-validation-error';
    CloseLoader();
    $.validator.setDefaults({
        ignore: null,
        //errorClass: "mobile-validation-error", // default values is input-validation-error
        //validClass: "valid", // default values is valid
        highlight: function (element, errorClass, validClass) { //for the elements having error
            $(element).addClass(errClass).removeClass(validClass);
            $(element.form).find("#" + element.id).parent().parent().addClass("mbsc-err");
            var elemName = $(element.form).find("#" + element.id).attr('name');
            $(element.form).find('[data-valmsg-for="' + elemName + '"]').addClass("mbsc-err-msg");
        },
        unhighlight: function (element, errorClass, validClass) { //for the elements having not any error
            $(element).removeClass(errClass).addClass(validClass);
            $(element.form).find("#" + element.id).parent().parent().removeClass("mbsc-err");
            var elemName = $(element.form).find("#" + element.id).attr('name');
            $(element.form).find('[data-valmsg-for="' + elemName + '"]').removeClass("mbsc-err-msg");
        },
    });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        if ($(this).closest('form').length > 0) {
            $(this).valid();
        }
    });

    $(document).find('.select2picker').select2({});
    $(document).find('.mobiscrollselect:not(:disabled)').mobiscroll().select({
        display: 'bubble',
        filter: true,
        group: {
            groupWheel: false,
            header: false
        },
    });
    $(document).find('.mobiscrollselect:disabled').mobiscroll().select({
        disabled: true
    });
    SetFixedHeadStyle();
}
////#endregion












