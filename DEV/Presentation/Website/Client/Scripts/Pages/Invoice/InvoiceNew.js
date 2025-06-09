//v2-389
var gridname = "InvoiceMatch_Search"; 
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
//v2-389
var selectCount = 0;
var invoiceSearchdt;
var invoListSearchdt;
var invoicerecieptList;
var selectedcount = 0;
var totalcount = 0;
var searchcount = 0;
var searchresult = [];
var IsCheckAllTrue;
var run = false;
var IMStatus = 0;

var CompleteATPStartDateVw = '';//V2-373
var CompleteATPEndDateVw = '';
var CompletePStartDateVw = '';
var CompletePEndDateVw = '';
var CreateStartDateVw = '';
var CreateEndDateVw = '';
var today = $.datepicker.formatDate('mm/dd/yy', new Date());
var mysearchText = '';//V2-373


$(function () {
    ShowbtnLoader("btnsortmenu");
    ShowbtnLoaderclass("LoaderDrop");
    $(document).on('click', "#action", function () {
        $(".actionDrop").slideToggle();
    });
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
    //V2-373
    var val = localStorage.getItem("InvoiceMatchingstatus");
    if (val && val !== "0") {
        IMStatus = val;
        var text = "";
        if (val == '17' || val == '18' || val == '19' || val == '20' || val == '21' || val == '22') {
            $('#cmbcreateview').val(val).trigger('change');
            text = $('#IMsearchListul').find('li').eq(0).text();
            $('#IMsearchtitle').text(text);
            $("#IMsearchListul li").removeClass("activeState");
            $("#IMsearchListul li").eq(0).addClass('activeState');
        }
        else if (val == '2' || val == '3' || val == '4' || val == '5' || val == '6' || val == '10') {
            $('#cmbcompleteview').val(val).trigger('change');
            text = $('#IMsearchListul').find('li').eq(2).text();
            $('#IMsearchtitle').text(text);
            $("#IMsearchListul li").removeClass("activeState");
            $("#IMsearchListul li").eq(2).addClass('activeState');
        }
        else if (val == '11' || val == '12' || val == '13' || val == '14' || val == '15' || val == '16') {
            $('#cmbcompletepaidview').val(val).trigger('change');
            text = $('#IMsearchListul').find('li').eq(3).text();
            $('#IMsearchtitle').text(text);
            $("#IMsearchListul li").removeClass("activeState");
            $("#IMsearchListul li").eq(3).addClass('activeState');
        }
        else {
            $('#IMsearchListul li').each(function (index, value) {
                if ($(this).attr('id') == IMStatus && $(this).attr('id') != '0') {
                    $('#IMsearchtitle').text($(this).text());
                    $(this).addClass('activeState');
                }
            });
        }
        if ($('#IMsearchListul').find('.activeState').attr('id') == '9') {
            if ($(document).find('#cmbcompletepaidview').val() != '16')
                text = text + " - " + $(document).find('#cmbcompletepaidview option[value=' + val + ']').text();
            else
                text = text + " - " + $('#completepaiddaterange').val();
            $('#IMsearchtitle').text(text);
        }
        //V2-373

        //V2-373 Point 3
        //-------------------------------------------------------        
        else if ($('#IMsearchListul').find('.activeState').attr('id') == '8') {
            if ($(document).find('#cmbcompleteview').val() != '10')
                text = text + " - " + $(document).find('#cmbcompleteview option[value=' + val + ']').text();
            else
                text = text + " - " + $('#completedaterange').val();
            $('#IMsearchtitle').text(text);
        }
            //-------------------------------------------------------
         //V2-1061
        else if ($('#IMsearchListul').find('.activeState').attr('id') == '0') {
            if ($(document).find('#cmbcreateview').val() != '22')
                text = text + " - " + $(document).find('#cmbcreateview option[value=' + val + ']').text();
            else
                text = text + " - " + $('#createdaterange').val();
            $('#IMsearchtitle').text(text);
        }
        //V2-1061
        generateInvoiceDataTable();
    }
    else {

        $('#IMsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == 1) {
                IMStatus = $(this).attr('id');
                $('#IMsearchtitle').text($(this).text());
                $("#IMsearchListul li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
        generateInvoiceDataTable();
    }    
    $(document).find('.select2picker').select2({});//V2-373
});

$(document).ready(function () {

    $(".actionBar").fadeIn();
    $("#InvoiceGridAction :input").attr("disabled", "disabled");
});
$(document).mouseup(function (e) {
    var container = $(document).find('#searchBttnNewDrop');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slideToggle");
    }
});
//#region  Search button

$(document).on('click', '#SrchBttnNew', function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'InvoiceMatchHeader' },
        beforeSend: function () {
            ShowbtnLoader("SrchBttnNew");
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)" id= "mem_' + i + '"' + '><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
                UlSearchList.innerHTML = str;
            }
        },
        complete: function () {
            $(document).find('#searchBttnNewDrop').show("slideToggle");
            HidebtnLoader("SrchBttnNew");
        },
        error: function () {
            HidebtnLoader("SrchBttnNew");
        }
    });

});
$(document).on('keyup', '#txtColumnSearch', function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode === 13) {
        TextSearch();
    }
    else {
        event.preventDefault();
    }
});
$(document).on('click', '.txtSearchClick', function () {
    TextSearch();
});

function GenerateSearchList(txtSearchval, isClear) {   
    var data = localStorage.getItem("InvoiceMatchingstatus"); //V2-373
    IMStatus = data; //V2-373
    run = true;
    $.ajax({
        url: '/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'InvoiceMatchHeader', searchText: txtSearchval, isClear: isClear },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {

            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)"><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchList.innerHTML = str;
        },
        complete: function () {

            if (isClear == false) {
                invoiceSearchdt.page('first').draw('page');
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
};
function TextSearch() {
    hGridclearAdvanceSearch();
    //V2-373
    var data = localStorage.getItem("InvoiceMatchingstatus");
    if ($("#IMsearchListul > #8").hasClass('activeState') == false)
        $(document).find('#cmbcompleteview').val('').trigger('change');
    if ($("#IMsearchListul > #9").hasClass('activeState') == false)
        $(document).find('#cmbcompletepaidview').val('').trigger('change');
    clearAdvanceSearch();
    $("#gridadvsearchstatus").val('');

    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {     
        mysearchText = $(document).find('#txtColumnSearch').val();
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        run = true;
        IMStatus = data; //V2-373       
        invoiceSearchdt.page('first').draw('page');
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
$(document).on('keyup', '#IMsearctxtbox', function (e) {
    var tagElems = $(document).find('#IMsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.IMsearchdrpbox', function (e) {
    run = true;
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#advsearchfilteritems").html('');
    $(document).find('#txtColumnSearch').val('');
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $(document).find('.chksearch').prop('checked', false);
    $('.itemcount').text(0);
    SelectedWoIdToCancel = [];
    if ($(this).attr('id') == '0') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("InvoiceMatchingstatus");
        if (val == '17' || val == '18' || val == '19' || val == '20' || val == '21' || val == '22') {
            $('#cmbcreateview').val(val).trigger('change');
        }
        $(document).find('#IMDateRangeModalForCreateDate').modal('show');

        CompleteATPStartDateVw = '';
        CompleteATPStartDateVw = '';
        CompletePStartDateVw = '';
        CompletePEndDateVw = '';        
        return;
    }

    else {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('IMCreateStartDateVw');
        localStorage.removeItem('IMCreateEndDateVw');
        $(document).find('#cmbcreateview').val('').trigger('change');
    }
    //V2-373
    if ($(this).attr('id') == '8') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("InvoiceMatchingstatus");
        if (val == '2' || val == '3' || val == '4' || val == '5' || val == '6' || val == '10') {
            $('#cmbcompleteview').val(val).trigger('change');
        }
        //V2-373
        //else {
        //    CompleteATPStartDateVw = '';
        //    CompleteATPEndDateVw = '';           
        //    localStorage.removeItem('IMHCompleteATPStartDateVw');
        //    localStorage.removeItem('IMHCompleteATPEndDateVw');
        //    $(document).find('#cmbcompleteview').val('').trigger('change');
        //}
        $(document).find('#DateRangeModal').modal('show');//V2-373

        CompletePStartDateVw = '';
        CompletePEndDateVw = '';
        CompletePStartDateVw = '';
        CompletePEndDateVw = '';       
        return;
    }
    else {
        CompleteATPStartDateVw = '';
        CompleteATPEndDateVw = '';
        localStorage.removeItem('IMHCompleteATPStartDateVw');
        localStorage.removeItem('IMHCompleteATPEndDateVw');
        $(document).find('#cmbcompleteview').val('').trigger('change');
    }
       
        if ($(this).attr('id') == '9')//V2-373
        {
        $(document).find('#searcharea').hide("slide");
        var val2 = localStorage.getItem("InvoiceMatchingstatus");
        if (val2 == '11' || val2 == '12' || val2 == '13' || val2== '14' || val2 == '15' || val2 == '16') {
            $('#cmbcompletepaidview').val(val2).trigger('change');
        }
        //else {
        //    CompletePStartDateVw = '';
        //    CompletePEndDateVw = '';
        //    localStorage.removeItem('IMHCompletePStartDateVw');
        //    localStorage.removeItem('IMHCompletePEndDateVw');
        //    $(document).find('#cmbcompletepaidview').val('').trigger('change');
        //}
            $(document).find('#DateRangePaidModal').modal('show');//V2-373
            CompleteATPStartDateVw = '';
            CompleteATPEndDateVw = '';
            CreateStartDateVw = '';
            CreateEndDateVw = '';
        return;
    }
    else {
        CompletePStartDateVw = '';
        CompletePEndDateVw = '';
        localStorage.removeItem('IMHCompletePStartDateVw');
        localStorage.removeItem('IMHCompletePEndDateVw');
        $(document).find('#cmbcompletepaidview').val('').trigger('change');
    }
 

    //V2-373
    if ($(this).attr('id') != '0') {
        $('#IMsearchtitle').text($(this).text());
        localStorage.setItem("InvoiceMatchingstatustext", $(this).text());
    }
    else {
        $(document).find('#IMDateRangeModalForCreateDate').modal('show');
        //var text = getResourceValue("InvoiceMatchingAlert");
        //$('#IMsearchtitle').text(text);
        //localStorage.setItem("InvoiceMatchingstatustext", text);
        return;
    }
    //V2-373

    localStorage.setItem("InvoiceMatchingstatus", $(this).attr('id'));
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    IMStatus = $(this).attr('id');
    if (IMStatus.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        invoiceSearchdt.page('first').draw('page');
    }

});
//#region Select Time Period  V2-373
$(document).on('click', '#btntimeperiodForCreateDate', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbcreateview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != '22') {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('IMCreateStartDateVw');
        localStorage.removeItem('IMCreateEndDateVw');
    }
    else {
        localStorage.setItem('IMCreateStartDateVw', CreateStartDateVw);
        localStorage.setItem('IMCreateEndDateVw', CreateEndDateVw);
    }
    $(document).find('#IMDateRangeModalForCreateDate').modal('hide');
    var text = $('#IMsearchListul').find('li').eq(0).text();

    if (daterangeval != '22')

        text = text + " - " + $(document).find('#cmbcreateview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#createdaterange').val();

    $('#IMsearchtitle').text(text);
    localStorage.setItem("InvoiceMatchingstatustext", text);
    $("#IMsearchListul li").removeClass("activeState");
    $("#IMsearchListul li").eq(0).addClass('activeState');
    localStorage.setItem("InvoiceMatchingstatus", daterangeval);
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');

    IMStatus = daterangeval;
    localStorage.setItem("InvoiceMatchingstatus", IMStatus);
    if (daterangeval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        invoiceSearchdt.page('first').draw('page');
    }

});
$(document).on('change', '#cmbcreateview', function (e) {
    var thielement = $(this);
    IMStatus = thielement.val();
    if (thielement.val() == '22') {
        var strtlocal = localStorage.getItem('IMCreateStartDateVw');
        if (strtlocal) {
            CreateStartDateVw = strtlocal;
        }
        else {
            CreateStartDateVw = today;
        }
        var endlocal = localStorage.getItem('IMCreateEndDateVw');
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
        localStorage.removeItem('IMCreateStartDateVw');
        localStorage.removeItem('IMCreateEndDateVw');
        $(document).find('#timeperiodcontainerForCreateDate').hide();
    }
});
$(document).on('change', '#cmbcompletepaidview', function (e) {
    var thielement = $(this);
    IMStatus = thielement.val();
    if (thielement.val() == '16') {
        var strtlocal = localStorage.getItem('IMHCompletePStartDateVw');
        if (strtlocal) {
            CompletePStartDateVw = strtlocal;
        }
        else {
            CompletePStartDateVw = today;
        }
        var endlocal = localStorage.getItem('IMHCompletePEndDateVw');
        if (endlocal) {
            CompletePEndDateVw = endlocal;
        }
        else {
            CompletePEndDateVw = today;
        }
       
        $(document).find('#timeperiodcontainerForPaid').show();
        $(document).find('#completepaiddaterange').daterangepicker({
            format: 'MM/DD/YYYY',
            startDate: CompletePStartDateVw,
            endDate: CompletePEndDateVw,
            "locale": {
                "applyLabel": getResourceValue("JsApply"),
                "cancelLabel": getResourceValue("CancelAlert")
            }
        }, function (start, end, label) {
            CompletePStartDateVw = start.format('MM/DD/YYYY');
            CompletePEndDateVw = end.format('MM/DD/YYYY');
        });
    }
    else {
        CompletePStartDateVw = '';
        CompletePEndDateVw = '';
        localStorage.removeItem('IMHCompletePStartDateVw');
        localStorage.removeItem('IMHCompletePEndDateVw');
        $(document).find('#timeperiodcontainerForPaid').hide();
    }
});
$(document).on('change', '#cmbcompleteview', function (e) {
    var thielement = $(this);
    IMStatus = thielement.val();
    if (thielement.val() == '10') {
        var strtlocal = localStorage.getItem('IMHCompleteATPStartDateVw');
        if (strtlocal) {
            CompleteATPStartDateVw = strtlocal;
        }
        else {
            CompleteATPStartDateVw = today;
        }
        var endlocal = localStorage.getItem('IMHCompleteATPEndDateVw');
        if (endlocal) {
            CompleteATPEndDateVw = endlocal;
        }
        else {
            CompleteATPEndDateVw = today;
        }
        $(document).find('#timeperiodcontainer').show();
        $(document).find('#completedaterange').daterangepicker({
            format: 'MM/DD/YYYY',
            startDate: CompleteATPStartDateVw,
            endDate: CompleteATPEndDateVw,
            "locale": {
                "applyLabel": getResourceValue("JsApply"),
                "cancelLabel": getResourceValue("CancelAlert")
            }
        }, function (start, end, label) {
            CompleteATPStartDateVw = start.format('MM/DD/YYYY');
            CompleteATPEndDateVw = end.format('MM/DD/YYYY');
        });
    }
    else {
        CompleteATPStartDateVw = '';
        CompleteATPEndDateVw = '';
        localStorage.removeItem('IMHCompleteATPStartDateVw');
        localStorage.removeItem('IMHCompleteATPEndDateVw');
        $(document).find('#timeperiodcontainer').hide();
    }
});
$(document).on('click', '#btntimeperiod', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbcompleteview').val();
    if (daterangeval == '') {
        return;
    }
    IMStatus = daterangeval;
    if (daterangeval != '10') {
        CompleteATPStartDateVw = '';
        CompleteATPEndDateVw = '';
        localStorage.removeItem('IMHCompleteATPStartDateVw');
        localStorage.removeItem('IMHCompleteATPEndDateVw');
    }
    else {       
        localStorage.setItem('IMHCompleteATPStartDateVw', CompleteATPStartDateVw);
        localStorage.setItem('IMHCompleteATPEndDateVw', CompleteATPEndDateVw);
    }
    invoiceSearchdt.page('first').draw('page');    
    $(document).find('#DateRangeModal').modal('hide');
    var text = $('#IMsearchListul').find('li').eq(2).text();

    if (daterangeval != '10')
        text = text + " - " + $(document).find('#cmbcompleteview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#completedaterange').val();

    $('#IMsearchtitle').text(text);
    $("#IMsearchListul li").removeClass("activeState");
    $("#IMsearchListul li").eq(2).addClass('activeState');
    localStorage.setItem("InvoiceMatchingstatus", daterangeval);
});
$(document).on('click', '#btntimeperiodForPaid', function (e) {
    run = true;
    var paiddaterangeval = $(document).find('#cmbcompletepaidview').val();
    if (paiddaterangeval == '') {
        return;
    }
    IMStatus = paiddaterangeval;
    if (paiddaterangeval != '16') {
        CompletePStartDateVw = '';
        CompletePEndDateVw = '';
        localStorage.removeItem('IMHCompletePStartDateVw');
        localStorage.removeItem('IMHCompletePEndDateVw');
    }
    else {      
        localStorage.setItem('IMHCompletePStartDateVw', CompletePStartDateVw);
        localStorage.setItem('IMHCompletePEndDateVw', CompletePEndDateVw);
    }
    invoiceSearchdt.page('first').draw('page');  
    $(document).find('#DateRangePaidModal').modal('hide');
    var text = $('#IMsearchListul').find('li').eq(3).text(); 
    if (paiddaterangeval != '16')
        text = text + " - " + $(document).find('#cmbcompletepaidview option[value=' + paiddaterangeval + ']').text();
    else
        text = text + " - " + $('#completepaiddaterange').val();

    $('#IMsearchtitle').text(text);
    $("#IMsearchListul li").removeClass("activeState");
    $("#IMsearchListul li").eq(3).addClass('activeState');
    localStorage.setItem("InvoiceMatchingstatus", paiddaterangeval);

});
//V2-373
//#endregion

//#endregion

$(document).on('click', '#pinIRsidebarCollapse,#sidebarCollapse', function () {
    $('#renderinvoice').find('.sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
});
function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "InvoiceOverview":
            var InvID = $(document).find('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
            RedirectInvoiveDetail(InvID);
            break;
        case "INVNotes":
            generateInvNotesGrid();
            break;
        case "INVAttachments":
            generateInvAttachmentGrid();
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
$(document).on('click', "#InvoiceOverViewSidebar", function () {
   
    $(document).find('#Active').css('display', 'block');
    generateInvoiceLineItemListDataTable();
});
$(document).on('click', "ul.vtabs li", function () {
    if ($(this).find('#drpDwnLink').length > 0) {
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
$(document).on('click', ".addInvoice", function (e) {
    $.ajax({
        url: "/Invoice/AddInvoiceMatchHeader",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderinvoice').html(data);
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
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
}
var selectedStatus;
$(document).on('click', '#liPdf', function () {
    $(".buttons-pdf")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', '#liPrint', function () {
    $(".buttons-print")[0].click();
    $('#mask').trigger('click');
});
var order = '1';//Invoice Match Sorting
var orderDir = 'asc';//Invoice Match Sorting
function generateInvoiceDataTable() {
    if ($(document).find('#tblinvoice').hasClass('dataTable')) {
        invoiceSearchdt.destroy();
    }
    invoiceSearchdt = $("#tblinvoice").DataTable({
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
                }//Invoice Match Sorting
                //V2-389
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: gridname, //V2-389
                        LayOutInfo: JSON.stringify(data),
                        FilterInfo: JSON.stringify(filterinfoarray)//V2-389
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
                //V2-389
                "url": "/Base/GetLayout",
                //V2-389
                "data": {
                    GridName: gridname //V2-389
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    //V2-389
                    selectCount = 0;
                    if (json.LayoutInfo) {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);//Invoice Match Sorting
                        order = LayoutInfo.order[0][0];//Invoice Match Sorting
                        orderDir = LayoutInfo.order[0][1]; //Invoice Match Sorting
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
                        }
                    }
                    else {
                        callback(json.LayoutInfo);
                    }
                    //V2-389
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
                title: 'Invoice List'
            },
            {
                extend: 'print',
                title: 'Invoice List'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Invoice List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Invoice List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Invoice/GetInvoiceMaintGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.CustomQueryDisplayId = IMStatus;               
                d.CompleteATPStartDateVw = ValidateDate(CompleteATPStartDateVw);//V2-373
                d.CompleteATPEndDateVw = ValidateDate(CompleteATPEndDateVw);//V2-373
                d.CompletePStartDateVw = ValidateDate(CompletePStartDateVw);//V2-373
                d.CompletePEndDateVw = ValidateDate(CompletePEndDateVw);//V2-373
                d.CreateStartDateVw = ValidateDate(CreateStartDateVw);//V2-1061
                d.CreateEndDateVw = ValidateDate(CreateEndDateVw);//V2-1061
                d.invoice = LRTrim($("#GAinvoice").val());
                d.status = LRTrim($("#GAstatus").val());
                d.vendor = LRTrim($("#GAvendor").val());
                d.vendorname = LRTrim($("#GAvendorname").val());
                d.receiptdate = ValidateDate($("#GAreceiptdate").val());
                d.purchaseorder = LRTrim($("#GApurchaseorder").val());
                d.invoicedate = ValidateDate($("#GAinvoicedate").val());
                d.txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
                d.Order = order;//Invoice Match Sorting
              //  d.orderDir = orderDir;//Invoice Match Sorting
             
            },
            "dataSrc": function (result) {
                let colOrder = invoiceSearchdt.order();
                orderDir = colOrder[0][1];
                if (result.data.length < 1) {
                    $(document).find('.import-export').prop('disabled', true);
                }
                else {
                    $(document).find('.import-export').prop('disabled', false);
                }
                HidebtnLoaderclass("LoaderDrop");
                HidebtnLoader("btnsortmenu");
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "InvoiceMatchHeaderId",
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
                    "data": "ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_invoice href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    render: function (data, type, row, meta) {
                        if (data == statusCode.Paid) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide fixedStatusSmallboxwidth'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Open) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide fixedStatusSmallboxwidth'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.AuthorizedToPay) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--yellow m-badge--wide fixedStatusBigboxwidth'>" + getStatusValue(data) + "</span >";
                        }
                        else {
                            return getStatusValue(data);
                        }
                    }
                },
                { "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ReceiptDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date" },
                { "data": "InvoiceDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date" },
                { "data": "POClientLookUpId", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        "columnDefs": [
            {
                render: function (data, type, full, meta) {
                    return "<div class='text-wrap width-100'>" + data + "</div>";
                },
                targets: [2]    
            },
            {
                targets: [0, 1],
                className: 'noVis'
            }

        ],
        initComplete: function () {
            SetPageLengthMenu();
            //var currestsortedcolumn = $('#tblinvoice').dataTable().fnSettings().aaSorting[0][0];
            //var currestsortedorder = $('#tblinvoice').dataTable().fnSettings().aaSorting[0][1];
            //var column = this.api().column(currestsortedcolumn);
            //var columnId = $(column.header()).attr('id');
            //$(document).find('.srtInvMcolumn').removeClass('sort-active');
            //$(document).find('.srtInvMorder').removeClass('sort-active');
            //switch (columnId) {
            //    case "thIdInvoice":
            //        $(document).find('.srtInvMcolumn').eq(0).addClass('sort-active');
            //        break;
            //    case "thIdStatus":
            //        $(document).find('.srtInvMcolumn').eq(1).addClass('sort-active');
            //        break;
            //    case "thIdVendor":
            //        $(document).find('.srtInvMcolumn').eq(2).addClass('sort-active');
            //        break;
            //    case "thIdVendorName":
            //        $(document).find('.srtInvMcolumn').eq(3).addClass('sort-active');
            //        break;
            //}
            //switch (currestsortedorder) {
            //    case "asc":
            //        $(document).find('.srtInvMorder').eq(0).addClass('sort-active');
            //        break;
            //    case "desc":
            //        $(document).find('.srtInvMorder').eq(1).addClass('sort-active');
            //        break;
            //}
            //$('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + column.header().innerHTML);
            $("#InvoiceGridAction :input").removeAttr("disabled");
            $("#InvoiceGridAction :button").removeClass("disabled");
            DisableExportButton($("#tblinvoice"), $(document).find('.import-export'));
        }
    });
}
$(document).find('#tblinvoice').on('click', 'tbody td img', function (e) {
    var tr = $(this).closest('tr');
    var row = invoiceSearchdt.row(tr);
    if (this.src.match('details_close')) {
        this.src = "../../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        this.src = "../../Images/details_close.png";
        var invoiceMatchHeaderId = $(this).attr("rel");
        $.ajax({
            url: "/Invoice/GetInvInnerGrid",
            data: {
                invoiceMatchHeaderId: invoiceMatchHeaderId
            },
            beforeSend: function () {
                ShowLoader();
            },
            dataType: 'html',
            success: function (json) {
                row.child(json).show();
                dtinnerGrid = row.child().find('.invMatchinnerDataTable').DataTable(
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
                            { className: 'text-right', targets: [2, 4, 5] }
                        ],
                        "footerCallback": function (row, data, start, end, display) {
                            var api = this.api(),
                                // Total over all pages
                                total = api.column(5).data().reduce(function (a, b) {
                                    return parseFloat(a) + parseFloat(b);
                                }, 0);
                            // Update footer
                            $(api.column(5).footer()).html(total.toFixed(2));
                        },
                        initComplete: function () { row.child().find('.dataTables_scroll').addClass('tblchild-scroll'); CloseLoader(); }
                    });
                tr.addClass('shown');
            }
        });

    }
});
$(document).on('click', '#tblinvoice_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#tblinvoice_length .searchdt-menu', function () {
    run = true;
});
$('#tblinvoice').find('th').click(function () {
    run = true;
    order = $(this).data('col');
});
//$(document).find('.srtInvMcolumn').click(function () {
//    ShowbtnLoader("btnsortmenu");
//    order = $(this).data('col');//Invoice Match Sorting
//    var txtColumnSearch = LRTrim($(document).find('#txtColumnSearch').val());
//    if (txtColumnSearch != "") {
//        TextSearch();//Invoice Match Sorting
//    }
//    else {
//        $('#tblinvoice').DataTable().draw();//Invoice Match Sorting
//    }  
//    $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());
//    $(document).find('.srtInvMcolumn').removeClass('sort-active');
//    $(this).addClass('sort-active');
//    run = true;
//});
//$(document).find('.srtInvMorder').click(function () {
//    ShowbtnLoader("btnsortmenu");
//    orderDir = $(this).data('mode');//Invoice Match Sorting
//    var txtColumnSearch = LRTrim($(document).find('#txtColumnSearch').val());
//    if (txtColumnSearch != "") {
//        TextSearch();//Invoice Match Sorting
//    }
//    else {
//        $('#tblinvoice').DataTable().draw();//Invoice Match Sorting
//    }  
//    $(document).find('.srtInvMorder').removeClass('sort-active');
//    $(this).addClass('sort-active');
//    run = true;
//});
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            dtTable = $("#tblinvoice").DataTable();
            var colname = order;//Invoice Match Sorting
            var coldir = orderDir;//Invoice Match Sorting
            var jsonResult = $.ajax({
                url: '/Invoice/GetINVPrintData?page=all',
                data: {
                    CustomQueryDisplayId: IMStatus,                   
                    CompleteATPStartDateVw: ValidateDate(CompleteATPStartDateVw),//V2-373
                    CompleteATPEndDateVw: ValidateDate(CompleteATPEndDateVw),//V2-373
                    CompletePStartDateVw: ValidateDate(CompletePStartDateVw),//V2-373
                    CompletePEndDateVw: ValidateDate(CompletePEndDateVw),//V2-373
                    CreateStartDateVw:ValidateDate(CreateStartDateVw),//V2-1061
                    CreateEndDateVw:ValidateDate(CreateEndDateVw),//V2-1061
                    invoice: LRTrim($("#GAinvoice").val()),
                    status: $("#GAstatus").val(),
                    vendor: LRTrim($("#GAvendor").val()),
                    vendorname: LRTrim($("#GAvendorname").val()),
                    receiptdate: ValidateDate($("#GAreceiptdate").val()),
                    purchaseorder: LRTrim($("#GApurchaseorder").val()),
                    invoicedate: ValidateDate($("#GAinvoicedate").val()),
                    colname: colname,
                    coldir: coldir,
                    txtSearchval: LRTrim($(document).find('#txtColumnSearch').val())
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#tblinvoice thead tr th").map(function (key) {
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
                if (item.ReceiptDate != null) {
                    item.ReceiptDate = item.ReceiptDate;
                }
                else {
                    item.ReceiptDate = "";
                }
                if (item.InvoiceDate != null) {
                    item.InvoiceDate = item.InvoiceDate;
                }
                else {
                    item.InvoiceDate = "";
                }
                if (item.POClientLookupId != null) {
                    item.POClientLookupId = item.POClientLookupId;
                }
                else {
                    item.POClientLookupId = "";
                }
                var fData = [];
                $.each(visiblecolumnsIndex, function (index, inneritem) {
                    var key = Object.keys(item)[inneritem];
                    var value = item[key];
                    fData.push(value);
                });
                d.push(fData);
            })
            return {
                body: d,
                header: $("#tblinvoice thead tr th").find('div').map(function (key) {
                    if ($(this).parents('.invMatchinnerDataTable').length == 0 && this.innerHTML) {
                        return this.innerHTML;
                    }
                }).get()
            };
        }
    });
});
$("#btnDataAdvSrchInvoice").on('click', function (e) {
    run = true;
    searchresult = [];
    InvoiceAdvSearch();
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    invoiceSearchdt.page('first').draw('page');
});
function InvoiceAdvSearch() {
    var InactiveFlag = false;
    $("#purchaseRequestModel_ScheduleWorkList").val(0);
    var searchitemhtml = "";
    //Add on 09/07/2020 A
    $(document).find('#txtColumnSearch').val('');
    //Add on 09/07/2020 A
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
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    $(".filteritemcount").text(selectCount);
    if (searchtxtId == "GAstatus") {
        $(document).find("#GAstatus").val("").trigger('change.select2');
    }
    InvoiceAdvSearch();
    invoiceSearchdt.page('first').draw('page');
});
function clearAdvanceSearch() {
    $("#GAinvoice").val("");
    $("#GAvendor").val("");
    $("#GAvendorname").val("");
    $('#GAreceiptdate').val("");
    $("#GApurchaseorder").val("");
    $("#GAstatus").val("").trigger('change.select2');
    selectCount = 0;
    $(".filteritemcount").text(selectCount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
function hGridclearAdvanceSearch() {
    selectCount = 0;
    $(".filteritemcount").text(selectCount);
    $('#advsearchsidebar').find('input:text').val('');
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}

$(document).on('click', '#dismiss, .overlay', function () {
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
});
$(document).on('click', '.lnk_invoice', function (e) {
    var row = $(this).parents('tr');
    var data = invoiceSearchdt.row(row).data();
    var titletext = $('#IMsearchtitle').text();
    localStorage.setItem("InvoiceMatchingstatustext", titletext);
    $.ajax({
        url: "/Invoice/InvoiceDetails",
        type: "POST",
        data: { invoiceId: data.InvoiceMatchHeaderId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderinvoice').html(data);
            $(document).find('#spnlinkToSearch').text(titletext);
            CloseLoader();
        },
        complete: function () {
            generateInvoiceLineItemListDataTable();
            SetInvoiceDetailsControls();//V2-331
        },
        error: function () {
            CloseLoader();
        }
    });
});

$(document).on('click', '#ChangeInvoiceDrop', function () {
    $(document).find('#ChangeInvoice').modal('show');
});

function generateInvoiceLineItemListDataTable() {
    var visibility = "True";
    var srcData = LRTrim($('#txtitemsearchbox').val());
    var InvoiceMatchHeaderId = $("#InvoiceMatchHeaderModel_InvoiceMatchHeaderId").val();
    var GAlinenumber = LRTrim($("#GAlinenumber").val());
    var GAdescription = LRTrim($("#GAdescription").val());
    var GAquantity = LRTrim($("#GAquantity").val());
    var GAunitofmeasure = LRTrim($("#GAunitofmeasure").val());
    var GAunitcost = LRTrim($("#GAunitcost").val());
    var GAtotalcost = LRTrim($("#GAtotalcost").val());
    var GApurchaseOrder = LRTrim($("#GApurchaseOrder").val());
    var GAaccount = LRTrim($("#GAaccount").val());
    if ($(document).find('#tblInvoiceItemList').hasClass('dataTable')) {
        invoListSearchdt.destroy();
    }
    invoListSearchdt = $("#tblInvoiceItemList").DataTable({
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
            "url": "/Invoice/GetInvoiceLineItemsGrid",
            "type": "post",
            data: function (d) {
                d.srcData = srcData;
                d.invoiceMatchHeaderId = InvoiceMatchHeaderId;
                d.linenumber = GAlinenumber;
                d.description = GAdescription;
                d.quantity = GAquantity;
                d.unitofmeasure = GAunitofmeasure;
                d.unitcost = GAunitcost;
                d.totalcost = GAtotalcost;
                d.purchaseOrder = GApurchaseOrder;
                d.account = GAaccount;
            },
            "datatype": "json"
        },
        columnDefs: [
            {
                "data": null,
                targets: [8], render: function (a, b, data, d) {
                    if (visibility == "True") {
                        return '<a class="btn btn-outline-success editinvoiceitembtn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delinvoiceitemBtn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
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
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "UnitOfMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PurchaseOrder", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Account", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#ListsidebarCollapse", function () {
    $('#renderinvoice').find('.sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
});
$(document).on('click', '#dismiss, .overlay', function () {
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
});
$(document).on('click', '#btnInvoiceitemSearch', function () {
    $('#advsearchsidebarListItemInvoice').find('input:text').each(function () { $(this).val(''); });
    generateInvoiceLineItemListDataTable();
});
$(document).on('click', '#btnDataAdvSrchListInvoiceItem', function () {
    searchresult = [];
    invoListSearchdt.state.clear();
    InvoiceListAdvSearch();
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
});
function InvoiceListAdvSearch() {
    var InactiveFlag = false;
    var searchitemhtmlListInvo = "";
    selectCount = 0;
    $('#advsearchsidebarListItemInvoice').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            selectCount++;
            searchitemhtmlListInvo = searchitemhtmlListInvo + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossListItem" aria-hidden="true"></a></span>';
        }
    });
    generateInvoiceLineItemListDataTable();
    $("#advsearchfilteritems").html(searchitemhtmlListInvo);
    $(".filteritemcount").text(selectCount);
}
$(document).on('click', '.btnCrossListItem', function () {
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    InvoiceListAdvSearch();
});
$(document).on('click', '#liClearAdvSearchFilterListInvoice', function () {
    $('#txtitemsearchbox').val('');
    invoListSearchdt.state.clear();
    $('#advsearchsidebarListItemInvoice').find('input:text').each(function () { $(this).val(''); });
    hGridclearAdvanceSearch();
    generateInvoiceLineItemListDataTable();
});
$(document).on('click', '.editinvoiceitembtn', function () {
    var data = invoListSearchdt.row($(this).parents('tr')).data();
    var invoiceMatchItemId = data.InvoiceMatchItemId;
    var InvoiceMatchHeaderId = $("#InvoiceMatchHeaderModel_InvoiceMatchHeaderId").val();
    var clientLookupId = $("#InvoiceMatchHeaderModel_ClientLookupId").val();
    $.ajax({
        url: "/Invoice/EditInvoiceListItem",
        type: "GET",
        dataType: 'html',
        data: { InvoiceMatchItemId: invoiceMatchItemId, InvoiceId: InvoiceMatchHeaderId, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderinvoice').html(data);
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function ListItemEditOnSuccess(data) {
    CloseLoader();
    if (data.data === "success") {
        var message;
        SuccessAlertSetting.text = getResourceValue("InvoiceMatchLineItemUpdateSuccessAlert");
        swal(SuccessAlertSetting, function () {
            RedirectInvoiveDetail(data.InvoiceMatchHeaderId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function RedirectInvoiveDetail(invoiceMatchHeaderId) {
    $.ajax({
        url: "/Invoice/InvoiceDetails",
        type: "POST",
        dataType: 'html',
        data: { invoiceId: invoiceMatchHeaderId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {

            $('#renderinvoice').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("InvoiceMatchingstatustext"));
            CloseLoader();
        },
        complete: function () {
            generateInvoiceLineItemListDataTable();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '.delinvoiceitemBtn', function () {
    var data = invoListSearchdt.row($(this).parents('tr')).data();
    var headerId = data.InvoiceMatchHeaderId;
    var ItemId = data.InvoiceMatchItemId;
    DeleteInvoiceItem(headerId, ItemId);
});
function DeleteInvoiceItem(headerId, ItemId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Invoice/DeleteInvoiceitem',
            data: {
                MatchHeaderId: headerId, MatchItemId: ItemId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    RedirectInvoiveDetail(headerId);
                    ShowDeleteAlert(getResourceValue("InvoiceMatchLineItemDeleteSuccessAlert"));
                }
                else {
                    if (data == "spnCandeltNotOpenItem") {
                        var msg = getResourceValue(data);
                        GenericSweetAlertMethod(msg);
                    }
                    else {
                        GenericSweetAlertMethod(data);
                    }
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
$(document).on('click', '#AddInvoiceReceipt', function () {
    var invoiceMatchItemId = $(document).find('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
    var clientLookupId = $(document).find('#InvoiceMatchHeaderModel_ClientLookupId').val();
    $.ajax({
        url: "/Invoice/AddInvoiceReceipt",
        type: "GET",
        dataType: 'html',
        data: { InvoiceMatchItemId: invoiceMatchItemId, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderinvoice').html(data);
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function OnSuccessAddReceipts(data) {
    CloseLoader();
    if (data.data == "success") {
        if (data.Command == "save") {
            var message;
            SuccessAlertSetting.text = getResourceValue("InvoiceMatchLineItemAddSuccessAlert");
            swal(SuccessAlertSetting, function () {
                RedirectInvoiveDetail(data.InvoiceMatchHeaderId);
            });
        }
        else {
            ResetErrorDiv();
            $('#InvoiceOverview').addClass('active').trigger('click');
            SuccessAlertSetting.text = getResourceValue("AddPartsAlerts");
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change.select2');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#selectReceiptGrid', function () {
    var vendorId = $(document).find('#InvoiceMatchHeaderModel_VendorId').val();
    var invoiceMatchHeaderId = $('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
    var clientLookupId = $('#InvoiceMatchHeaderModel_ClientLookupId').val();
    $.ajax({
        url: "/Invoice/RenderReceiptView",
        type: "POST",
        dataType: "html",
        data: { VendorId: vendorId, InvoiceMatchHeaderId: invoiceMatchHeaderId, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderinvoice').html(data);
        },
        complete: function () {
            CloseLoader();
            GenerateReceiptGrid();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function GenerateReceiptGrid() {
    var vendorId = $(document).find('#InvoiceMatchHeaderModel_VendorId').val();
    var _srcData = LRTrim($("#SPGPopuptxtSearch").val());
    var _PurchaseOrder = LRTrim($('#txtPurchaseOrder').val());
    var _ReceivedDate = LRTrim($('#txtReceivedDate').val());
    var _PartID = LRTrim($('#txtPartID').val());
    var _Description = LRTrim($('#txtDescription').val());
    var _QuantityReceived = LRTrim($('#txtQuantityReceived').val());
    var _UnitofMeasure = LRTrim($('#txtUnitofMeasure').val());
    var _UnitCost = LRTrim($('#txtUnitCost').val());
    var _TotalCost = LRTrim($('#txtTotalCost').val());
    IsCheckAllTrue = $("#fgidselectall").prop("checked");
    if ($(document).find('#tblSelectReceiptGrid').hasClass('dataTable')) {
        invoicerecieptList.destroy();
    }
    invoicerecieptList = $("#tblSelectReceiptGrid").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Invoice/PopulateRecieptData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.VendorId = vendorId;
                d.srcData = _srcData;
                d.PurchaseOrder = _PurchaseOrder;
                d.ReceivedDate = _ReceivedDate;
                d.PartID = _PartID;
                d.Description = _Description;
                d.QuantityReceived = _QuantityReceived;
                d.UnitofMeasure = _UnitofMeasure;
                d.UnitCost = _UnitCost;
                d.TotalCost = _TotalCost;
            },
            "dataSrc": function (result) {
                searchcount = result.recordsTotal;
                $.each(result.data, function (index, item) {
                    searchresult.push(item.PurchaseRequestId);
                });
                if (totalcount < result.recordsTotal)
                    totalcount = result.recordsTotal;
                if (totalcount != result.recordsTotal)
                    selectedcount = result.recordsTotal;
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "POReceiptItemId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    targets: 0,
                    'render': function (data, type, full, meta) {
                        var found = InvoiceRecieptSelectedItemArray.some(function (el) {
                            return el.POReceiptItemID === data;
                        });
                        if (found) {
                            return '<input type="checkbox" data-eqid="' + data + '" class="chksearch"  checked  value="' + $('<div/>').text(data).html() + '">';
                        }
                        else {
                            return '<input type="checkbox" data-eqid="' + data + '" class="chksearch"  value="' + $('<div/>').text(data).html() + '">';
                        }
                    }
                },
                { "data": "POClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ReceivedDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-400'>" + data + "</div>";
                    }
                },
                { "data": "QuantityReceived", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "UnitOfMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "AccountId", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        "columnDefs": [
            {
                "targets": [9],
                "visible": false
            }
        ]
    });
}
var InvoiceRecieptSelectedItemArray = [];
$(document).on('change', '.chksearch', function () {
    var el = $('#fgidselectall').get(0);
    if (el && el.checked && ('indeterminate' in el)) {
        el.indeterminate = true;
    }
    var invoiceMatchHeaderId = $('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
    var data = invoicerecieptList.row($(this).parents('tr')).data();
    if (!this.checked) {
        InvoiceRecieptSelectedItemArray = InvoiceRecieptSelectedItemArray.filter(function (el) {
            return el.POReceiptItemID !== data.POReceiptItemId;
        });
    }
    else {
        var item = new PartNotInInventorySelectedItem(invoiceMatchHeaderId, data.POReceiptItemId, data.POClientLookupId,
            data.PartClientLookupId, data.Description, data.QuantityReceived,
            data.UnitOfMeasure, data.UnitCost, data.TotalCost, data.AccountId
        );
        var found = InvoiceRecieptSelectedItemArray.some(function (el) {
            return el.POReceiptItemID === data.POReceiptItemId;
        });
        if (!found) { InvoiceRecieptSelectedItemArray.push(item); }
    }
});
function PartNotInInventorySelectedItem(InvoiceMatchHeaderId, POReceiptItemId, POClientLookupId, PartClientLookupId, Description, QuantityReceived, UnitOfMeasure, UnitCost, TotalCost, AccountId) {
    this.InvoiceMatchHeaderId = InvoiceMatchHeaderId;
    this.POReceiptItemID = POReceiptItemId;
    this.POClientLookupId = POClientLookupId;
    this.PartClientLookupId = PartClientLookupId;
    this.Description = Description;
    this.QuantityReceived = QuantityReceived;
    this.UnitOfMeasure = UnitOfMeasure;
    this.UnitCost = UnitCost;
    this.TotalCost = TotalCost;
    this.AccountId = AccountId;
};
$(document).on('click', "#btnprocessReceipt", function (e) {
    if (InvoiceRecieptSelectedItemArray.length < 1) {
        ShowGridItemSelectionAlert();
        e.preventDefault();
        return false;
    }
    else {
        GeneratedfinalSelectPartsTable(InvoiceRecieptSelectedItemArray);
    }
});
function GeneratedfinalSelectPartsTable(datasource1) {
    InvoiceRecieptSelectedItemArray = [];
    var InvoiceMatchHeaderId = $('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
    $.each(datasource1, function (index, item) {
        var POReceiptItemId = item.POReceiptItemID;
        var POClientLookupId = item.POClientLookupId;
        var ReceivedDate = item.ReceivedDate;
        var PartClientLookupId = item.PartClientLookupId;
        var Description = item.Description;
        var QuantityReceived = item.QuantityReceived;
        var UnitOfMeasure = item.UnitOfMeasure;
        var UnitCost = item.UnitCost;
        var TotalCost = item.TotalCost;
        var AccountId = item.AccountId;
        var item = new PartNotInInventorySelectedItem(InvoiceMatchHeaderId, POReceiptItemId, POClientLookupId,
            PartClientLookupId, Description, QuantityReceived,
            UnitOfMeasure, UnitCost, TotalCost, AccountId
        );
        InvoiceRecieptSelectedItemArray.push(item);
    });
    var list = JSON.stringify({ 'list': InvoiceRecieptSelectedItemArray });
    $.ajax({
        url: "/Invoice/SaveListRecieptFromGrid",
        type: "POST",
        dataType: "json",
        data: list,
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                InvoiceRecieptSelectedItemArray = [];
                FinalGridSelectedItemArray = [];
                SuccessAlertSetting.text = getResourceValue("LineItemAddedAlert");
                swal(SuccessAlertSetting, function () {
                    invoicerecieptList.state.clear();
                    RedirectInvoiveDetail(data.invoiceMatchHeaderId);
                });
            }
            else {
                var msgEror = getResourceValue("UpdateAlert");
                ShowGenericErrorOnAddUpdate(msgEror);
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
$(document).on('click', '#fgidselectall', function (e) {
    var checked = this.checked;
    IsCheckAllTrue = checked;
    if (checked) {
        var _VendorId = $(document).find('#InvoiceMatchHeaderModel_VendorId').val();
        var _PurchaseOrder = LRTrim($('#txtPurchaseOrder').val());
        var _ReceivedDate = LRTrim($('#txtReceivedDate').val());
        var _PartID = LRTrim($('#txtPartID').val());
        var _Description = LRTrim($('#txtDescription').val());
        var _QuantityReceived = LRTrim($('#txtQuantityReceived').val());
        var _UnitofMeasure = LRTrim($('#txtUnitofMeasure').val());
        var _UnitCost = LRTrim($('#txtUnitCost').val());
        var _TotalCost = LRTrim($('#txtTotalCost').val());
        $.ajax({
            "url": "/Invoice/GetAllRecieptData",
            data: {
                VendorId: _VendorId,
                PurchaseOrder: _PurchaseOrder,
                ReceivedDate: _ReceivedDate,
                PartID: _PartID,
                Description: _Description,
                QuantityReceived: _QuantityReceived,
                UnitofMeasure: _UnitofMeasure,
                UnitCost: _UnitCost,
                TotalCost: _TotalCost
            },
            async: true,
            type: "post",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data) {
                    InvoiceRecieptSelectedItemArray = [];
                    $(document).find('.chksearch').prop('checked', 'checked');
                    $.each(data, function (index, item) {
                        var invoiceMatchHeaderId = $('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
                        var item = new PartNotInInventorySelectedItem(invoiceMatchHeaderId, item.POReceiptItemId, item.POClientLookupId,
                            item.PartClientLookupId, item.Description, item.QuantityReceived,
                            item.UnitOfMeasure, item.UnitCost, item.TotalCost, item.AccountId
                        );
                        InvoiceRecieptSelectedItemArray.push(item);
                    });
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    }
    else {
        $(document).find('.chksearch').prop('checked', false);
        InvoiceRecieptSelectedItemArray = [];
    }
});
$(document).on('click', '#brdprlineitem', function () {
    var HeaderId = $(this).attr('data-val');
    RedirectInvoiveDetail(HeaderId);
});
var spartgridselectCount = 0;
$(document).on('click', '#btnSPGAdvanceSearch', function () {
    searchresult = [];
    invoicerecieptList.state.clear();
    SPGAdvSearch();
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
});
function SPGAdvSearch() {
    $("#SPGPopuptxtSearch").val("");
    var searchitemhtml = "";
    spartgridselectCount = 0;
    $('#SPGadvsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val()) {
            spartgridselectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times SPGbtnCross" aria-hidden="true"></a></span>';
        }
    });
    GenerateReceiptGrid();
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#SPGadvsearchfilteritems").html(searchitemhtml);
    $('#renderinvoice').find('.sidebar').removeClass('active');
    $(".pinvgridfilteritemcount").text(spartgridselectCount);
    $('.overlay').fadeOut();
}
$(document).on('click', '#ReceiptClearSearch', function () {
    $('#SPGPopuptxtSearch').val('');
    hGridPopUpclearAdvanceSearch();
    GenerateReceiptGrid();
    $(".pinvgridfilteritemcount").text(0);
});
$(document).on('click', '.SPGbtnCross', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    spartgridselectCount--;
    SPGAdvSearch();
});
$(document).on('click', '#btnSPGSearch', function () {
    hGridPopUpclearAdvanceSearch();
    invoicerecieptList.state.clear();
    GenerateReceiptGrid();
    $(".pinvgridfilteritemcount").text(0);
});
function hGridPopUpclearAdvanceSearch() {
    selectCount = 0;
    $(".filteritemcount").text(selectCount);
    $('#SPGadvsearchsidebar').find('input:text,select').val('');
    $('#SPGadvsearchsidebar').find('select').val('').trigger('change.select2');
    $('#SPGadvsearchfilteritems').find('span').html('');
    $('#SPGadvsearchfilteritems').find('span').removeClass('tagTo');
}
//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(invoiceSearchdt,true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0,1];
    funCustozeSaveBtn(invoiceSearchdt, colOrder);
    run = true;
    invoiceSearchdt.state.save(run);
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

//#region V2-373
function SetInvoiceDetailsControls() {
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
