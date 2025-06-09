//#region Common
var run = false;
var totalcount = 0;
var CustomQueryDisplayId = 1;
var sitefilters = "";
var CreateStartDateVw = '';
var CreateEndDateVw = '';
var SubmitStartDateVw = '';
var SubmitEndDateVw = '';
var today = $.datepicker.formatDate('mm/dd/yy', new Date());
var yearweekfiltersenterprise = "";
var selectCount = 0;
var gridname = "BBUKPIEnterprise_Search";
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}

//#endregion

$(document).ready(function () {

    $(document).on('click', "#action", function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $(document).on('focusout', "#action", function () {
        $(".actionDrop").fadeOut();
    });
    $(".actionBar").fadeIn();
    $("#BBUKPIEnterpriseGridAction :input").attr("disabled", "disabled");

    var title = localStorage.getItem("mrstatustext");
    if (title) {
        $(document).find('#spnlinkToSearch').text(title);
    }
    GetSitesDropdown();
    GetYearWeeksDropdown();
    $(document).find('.select2picker').select2({});
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
$(document).ready(function () {
    $(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '.dismiss, .overlay', function () {
        $(document).find('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', '#sidebarCollapse', function () {
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
    //#region Load Grid With Status
    var enterprisecurrentstatus = localStorage.getItem("BBUKPIENTERPRISESTATUS");
    if (enterprisecurrentstatus != 'undefined' && enterprisecurrentstatus != null && enterprisecurrentstatus != "") {
        CustomQueryDisplayId = enterprisecurrentstatus;
        if (enterprisecurrentstatus == '4' || enterprisecurrentstatus == '5' || enterprisecurrentstatus == '6' || enterprisecurrentstatus == '7' || enterprisecurrentstatus == '1' || enterprisecurrentstatus == '12') {
            $('#cmbcreateview').val(enterprisecurrentstatus).trigger('change');
            text = $('#bbuenterprisesearchListul').find('li').eq(0).text();
            $('#bbuenterprisesearchtitle').text(text);
            $("#bbuenterprisesearchListul li").removeClass("activeState");
            $("#bbuenterprisesearchListul li").eq(0).addClass('activeState');
        }
        if (enterprisecurrentstatus == '8' || enterprisecurrentstatus == '9' || enterprisecurrentstatus == '10' || enterprisecurrentstatus == '11' || enterprisecurrentstatus == '3' || enterprisecurrentstatus == '13') {
            $('#cmbsubmitview').val(enterprisecurrentstatus).trigger('change');
            text = $('#bbuenterprisesearchListul').find('li').eq(2).text();
            $('#bbuenterprisesearchtitle').text(text);
            $("#bbuenterprisesearchListul li").removeClass("activeState");
            $("#bbuenterprisesearchListul li").eq(2).addClass('activeState');
        }
        else {
            $('#bbuenterprisesearchListul li').each(function (index, value) {
                if ($(this).attr('id') == CustomQueryDisplayId && $(this).attr('id') != '0') {
                    $('#bbuenterprisesearchtitle').text($(this).text());
                    $(".searchList li").removeClass("activeState");
                    $(this).addClass('activeState');
                }
            });
        }
        if ($('#bbuenterprisesearchListul').find('.activeState').attr('id') == '1') {
            if ($(document).find('#cmbcreateview').val() != '12')
                text = text + " - " + $(document).find('#cmbcreateview option[value=' + enterprisecurrentstatus + ']').text();
            else
                text = text + " - " + $('#createdaterange').val();
            $('#bbuenterprisesearchtitle').text(text);
        }
        if ($('#bbuenterprisesearchListul').find('.activeState').attr('id') == '3') {
            if ($(document).find('#cmbsubmitview').val() != '13')
                text = text + " - " + $(document).find('#cmbsubmitview option[value=' + enterprisecurrentstatus + ']').text();
            else
                text = text + " - " + $('#submitdaterange').val();
            $('#bbuenterprisesearchtitle').text(text);
        }
    }
    else {
        CustomQueryDisplayId = "2";
        $('#bbuenterprisesearchtitle').text($('#bbuenterprisesearchListul').find('li').eq(1).text());
        $("#bbuenterprisesearchListul li").eq(1).addClass("activeState");
    }
    //#endregion
    //#region Load Grid With Site Filter
    var sitefilterstatus = localStorage.getItem("sitevalue");
    if (sitefilterstatus != "" && sitefilterstatus != undefined) {
        sitefilters = sitefilterstatus;
    }
    else {
        sitefilters = "";
    }
    //#endregion
    //#region Load Grid With Year Week Filter
    var yearweekfilterstatusenterprise = localStorage.getItem("yearweekvalueenterprisetext");
    if (yearweekfilterstatusenterprise != "" && yearweekfilterstatusenterprise != undefined) {
        yearweekfiltersenterprise = yearweekfilterstatusenterprise;
    }
    else {
        yearweekfiltersenterprise = "";
    }
    //#endregion
    generateBBUKPIEnterpriseDataTable();
    if (sitefilterstatus != "" && sitefilterstatus != undefined) {
        var stringToArray = sitefilterstatus.split(',');
        $('#ddlSites').val(stringToArray).trigger('change.select2');
        SiteTotalchecked = stringToArray.length;
        if ($('#ddlSites option').length == SiteTotalchecked + 1) {
            IsCheckedAllOption(true);
        }
        else {
            $.each(stringToArray, function (index, value) {
                AddItemInSiteSelect2MultiCheckBoxObj(value, true);
            });
        }
        getSiteCalMultiplecheck();
    }
    var yearweekfilterstatusenterpriseval = localStorage.getItem("yearweekvalueenterprise");
    if (yearweekfilterstatusenterpriseval != "" && yearweekfilterstatusenterpriseval != undefined) {
        var stringToArrayenterprise = yearweekfilterstatusenterpriseval.split(',');
        $('#ddlYearWeeks').val(stringToArrayenterprise).trigger('change.select2');
        YearWeekTotalchecked = stringToArrayenterprise.length;
        if ($('#ddlYearWeeks option').length == YearWeekTotalchecked + 1) {
            IsCheckedAllOptionYearWeek(true);
        }
        else {
            $.each(stringToArrayenterprise, function (index, value) {
                AddItemInYearWeekSelect2MultiCheckBoxObj(value, true);
            });
        }
    }
    getYearWeekCalMultiplecheck();
});
//#region Multiple Select Drowpdown  With CheckBox V2-1109
var SiteSelect2MultiCheckBoxObj = [];
var Site_id_selectElement = 'ddlSites';
var SitestaticWordInID = 'site_';
var SiteTotalchecked = 0;
var SiteAllselected = false;
function GetSitesDropdown() {
    $.map($('#' + Site_id_selectElement + ' option'), function (option) {
        AddItemInSiteSelect2MultiCheckBoxObj(option.value, false);
    });

    function formatResult(state) {
        if (SiteSelect2MultiCheckBoxObj.length > 0) {
            var stateId = SitestaticWordInID + state.id;
            let index = SiteSelect2MultiCheckBoxObj.findIndex(x => x.id == state.id);
            if (index > -1) {
                var checkbox = "";
                if (index == 0) {
                    checkbox = $('<div class="checkbox"><input class="select2Checkbox checkboxSites " id="site_0" type="checkbox" ' + (SiteSelect2MultiCheckBoxObj[index]["IsChecked"] ? 'checked' : '') +
                        '><label for="checkboxsite_0 "> ' + state.text + '</label></div>', { id: 'site_0' });
                }
                else {
                    checkbox = $('<div class="checkbox"><input class="select2Checkbox checkboxSites" id="' + stateId + '" type="checkbox" ' + (SiteSelect2MultiCheckBoxObj[index]["IsChecked"] ? 'checked' : '') +
                        '><label for="checkbox' + stateId + '"> ' + state.text + '</label></div>', { id: stateId });
                }

                return checkbox;
            }
        }
    }

    let optionSelect2 = {
        templateResult: formatResult,
        closeOnSelect: false,
        allowClear: true
        /* width: '100%'*/
    };

    let $select2 = $(document).find("#" + Site_id_selectElement).select2(optionSelect2);
    $("#" + Site_id_selectElement).find("option[value='']").attr("disabled", 'disabled');
    getSiteCalMultiplecheck();
    $select2.on('select2:close', function () {

    });
    $select2.on("select2:select", function (event) {
        $("#" + SitestaticWordInID + event.params.data.id).prop("checked", true);
        AddItemInSiteSelect2MultiCheckBoxObj(event.params.data.id, true);
        //If all options are slected then selectAll option would be also selected.

        if (SiteSelect2MultiCheckBoxObj.filter(x => x.IsChecked === false).length === 1) {
            AddItemInSiteSelect2MultiCheckBoxObj(0, true);
            $("#" + SitestaticWordInID + "0").prop("checked", true);
        }
        SiteTotalchecked = SiteTotalchecked + 1;
        getSiteCalMultiplecheck();
    });

    $select2.on("select2:unselect", function (event) {
        $("#" + SitestaticWordInID + "0").prop("checked", false);
        AddItemInSiteSelect2MultiCheckBoxObj(0, false);
        $("#" + SitestaticWordInID + event.params.data.id).prop("checked", false);
        AddItemInSiteSelect2MultiCheckBoxObj(event.params.data.id, false);
        SiteTotalchecked = SiteTotalchecked - 1;
        getSiteCalMultiplecheck();
    });

    $(document).on("click", "#" + SitestaticWordInID + "0", function () {
        var b = $("#" + SitestaticWordInID + "0").is(':checked');
        IsCheckedAllOption(b);
        if (b == true) {
            SiteTotalchecked = $('.checkboxSites:checked').length - 1;
        } else {
            SiteTotalchecked = $('.checkboxSites:checked').length;
        }
        getSiteCalMultiplecheck();
        $("#" + Site_id_selectElement).select2("close");
    });
    $(document).on("click", ".checkboxSites", function (event) {
        let selector = "#" + this.id;
        let isChecked = false;
        if (this.id == "site_0") {
            isChecked = SiteSelect2MultiCheckBoxObj[SiteSelect2MultiCheckBoxObj.findIndex(x => x.id == "")]['IsChecked'] ? true : false;
        } else {
            isChecked = SiteSelect2MultiCheckBoxObj[SiteSelect2MultiCheckBoxObj.findIndex(x => x.id == this.id.replaceAll(SitestaticWordInID, ''))]['IsChecked'] ? true : false;
        }
        $(selector).prop("checked", isChecked);
        getSiteCalMultiplecheck();
    });
    $(document).on("click", ".sitesBlock span  .select2-selection__rendered", function (event) {
        $(".sitesBlock span ul li .select2-search__field").val("");
    });
    $(document).on("click", ".sitesBlock span  .select2-selection__rendered .select2-selection__clear", function (event) {
        IsCheckedAllOption(false);
        SiteTotalchecked = $('.checkboxSites:checked').length;
        getSiteCalMultiplecheck();
        $("#" + Site_id_selectElement).select2("close");

    });
    $(document).on('focusout', '.sitesBlock span ul li .select2-search__field', function (e) {
        getSiteCalMultiplecheck();
    });
    $(document).on('keypress', '.sitesBlock span ul li .select2-search__field', function (e) {
        var tcount = SiteTotalchecked;
        var ftcount = "";
        if (tcount == 0) {
            ftcount = "All Sites";
        }
        else {
            ftcount = tcount.toString() + " Site(s)";
        }
        var text = $(".sitesBlock span ul li .select2-search__field").val();
        $(".sitesBlock span ul li .select2-search__field").val(text.replace(ftcount, ''));
        $('.sitesBlock span ul li .select2-search__field[placeholder=""]').attr('style', 'width:100%');
    });


}
function AddItemInSiteSelect2MultiCheckBoxObj(id, IsChecked) {
    if (SiteSelect2MultiCheckBoxObj.length > 0) {
        let index = SiteSelect2MultiCheckBoxObj.findIndex(x => x.id == id);
        if (index > -1) {
            SiteSelect2MultiCheckBoxObj[index]["IsChecked"] = IsChecked;
        }
        else {
            SiteSelect2MultiCheckBoxObj.push({ "id": id, "IsChecked": IsChecked });
        }
    }
    else {
        SiteSelect2MultiCheckBoxObj.push({ "id": id, "IsChecked": IsChecked });
    }
}
function IsCheckedAllOption(trueOrFalse) {
    $.map($('#' + Site_id_selectElement + ' option'), function (option) {
        AddItemInSiteSelect2MultiCheckBoxObj(option.value, trueOrFalse);
    });
    $('#' + Site_id_selectElement + " > option").not(':first').prop("selected", trueOrFalse);
    //This will select all options and adds in Select2
    $("#" + Site_id_selectElement).trigger("change");//This will effect the changes
    /* $(".select2-results__option").not(':first').attr("aria-selected", trueOrFalse);*/
    //This will make grey color of selected options

    $("input[id^='" + SitestaticWordInID + "']").prop("checked", trueOrFalse);
}
function getSiteCalMultiplecheck() {
    $('.sitesBlock span ul li .select2-search__field[placeholder=""]').attr('style', 'width:100%');
    $(".sitesBlock span  .select2-selection__rendered").find('.select2-selection__choice').remove();
    SiteAllselected = $("#" + SitestaticWordInID + "0").prop('checked') ? true : false;
    var tcount = SiteTotalchecked;
    var ftcount = "";
    if (tcount == 0) {
        ftcount = "All Sites";
    }
    else {
        ftcount = tcount.toString() + " Site(s)";
    }
    $(".sitesBlock span ul li .select2-search__field").val(ftcount);

}
var YearWeekSelect2MultiCheckBoxObj = [];
var YearWeek_id_selectElement = 'ddlYearWeeks';
var YearWeekstaticWordInID = 'yearweek_';
var YearWeekTotalchecked = 0;
var YearWeekAllselected = false;
function GetYearWeeksDropdown() {
    $.map($('#' + YearWeek_id_selectElement + ' option'), function (option) {
        AddItemInYearWeekSelect2MultiCheckBoxObj(option.value, false);
    });

    function formatResult(state) {
        if (YearWeekSelect2MultiCheckBoxObj.length > 0) {
            var stateId = YearWeekstaticWordInID + state.id;
            let index = YearWeekSelect2MultiCheckBoxObj.findIndex(x => x.id == state.id);
            if (index > -1) {
                var checkbox = "";
                if (index == 0) {
                    checkbox = $('<div class="checkbox"><input class="select2Checkbox checkboxYearWeeks " id="yearweek_0" type="checkbox" ' + (YearWeekSelect2MultiCheckBoxObj[index]["IsChecked"] ? 'checked' : '') +
                        '><label for="checkboxyearweek_0 "> ' + state.text + '</label></div>', { id: 'yearweek_0' });
                }
                else {
                    checkbox = $('<div class="checkbox"><input class="select2Checkbox checkboxYearWeeks" id="' + stateId + '" type="checkbox" ' + (YearWeekSelect2MultiCheckBoxObj[index]["IsChecked"] ? 'checked' : '') +
                        '><label for="checkbox' + stateId + '"> ' + state.text + '</label></div>', { id: stateId });
                }

                return checkbox;
            }
        }
    }

    let optionSelect2 = {
        templateResult: formatResult,
        closeOnSelect: false,
        allowClear: true
        /* width: '100%'*/
    };

    let $select2 = $(document).find("#" + YearWeek_id_selectElement).select2(optionSelect2);
    $("#" + YearWeek_id_selectElement).find("option[value='']").attr("disabled", 'disabled');
    getYearWeekCalMultiplecheck();
    $select2.on('select2:close', function () {

    });
    $select2.on("select2:select", function (event) {
        $("#" + YearWeekstaticWordInID + event.params.data.id).prop("checked", true);
        AddItemInYearWeekSelect2MultiCheckBoxObj(event.params.data.id, true);
        //If all options are slected then selectAll option would be also selected.

        if (YearWeekSelect2MultiCheckBoxObj.filter(x => x.IsChecked === false).length === 1) {
            AddItemInYearWeekSelect2MultiCheckBoxObj(0, true);
            $("#" + YearWeekstaticWordInID + "0").prop("checked", true);
        }
        YearWeekTotalchecked = YearWeekTotalchecked + 1;
        getYearWeekCalMultiplecheck();
    });

    $select2.on("select2:unselect", function (event) {
        $("#" + YearWeekstaticWordInID + "0").prop("checked", false);
        AddItemInYearWeekSelect2MultiCheckBoxObj(0, false);
        $("#" + YearWeekstaticWordInID + event.params.data.id).prop("checked", false);
        AddItemInYearWeekSelect2MultiCheckBoxObj(event.params.data.id, false);
        YearWeekTotalchecked = YearWeekTotalchecked - 1;
        getYearWeekCalMultiplecheck();
    });

    $(document).on("click", "#" + YearWeekstaticWordInID + "0", function () {
        var b = $("#" + YearWeekstaticWordInID + "0").is(':checked');
        IsCheckedAllOptionYearWeek(b);
        if (b == true) {
            YearWeekTotalchecked = $('.checkboxYearWeeks:checked').length - 1;
        } else {
            YearWeekTotalchecked = $('.checkboxYearWeeks:checked').length;
        }
        getYearWeekCalMultiplecheck();
        $("#" + YearWeek_id_selectElement).select2("close");
    });
    $(document).on("click", ".checkboxYearWeeks", function (event) {
        let selector = "#" + this.id;
        let isChecked = false;
        if (this.id == "yearweek_0") {
            isChecked = YearWeekSelect2MultiCheckBoxObj[YearWeekSelect2MultiCheckBoxObj.findIndex(x => x.id == "")]['IsChecked'] ? true : false;
        } else {
            isChecked = YearWeekSelect2MultiCheckBoxObj[YearWeekSelect2MultiCheckBoxObj.findIndex(x => x.id == this.id.replaceAll(YearWeekstaticWordInID, ''))]['IsChecked'] ? true : false;
        }
        $(selector).prop("checked", isChecked);
        getYearWeekCalMultiplecheck();
    });
    $(document).on("click", ".yearweekBlock span  .select2-selection__rendered", function (event) {
        $(".yearweekBlock span ul li .select2-search__field").val("");
    });
    $(document).on("click", ".yearweekBlock span  .select2-selection__rendered .select2-selection__clear", function (event) {
        IsCheckedAllOptionYearWeek(false);
        YearWeekTotalchecked = $('.checkboxYearWeeks:checked').length;
        getYearWeekCalMultiplecheck();
        $("#" + YearWeek_id_selectElement).select2("close");

    });
    $(document).on('focusout', '.yearweekBlock span ul li .select2-search__field', function (e) {
        getYearWeekCalMultiplecheck();
    });
    $(document).on('keypress', '.yearweekBlock span ul li .select2-search__field', function (e) {
        var tcount = YearWeekTotalchecked;
        var ftcount = "";
        if (tcount == 0) {
            ftcount = "All YearWeeks";
        }
        else {
            ftcount = tcount.toString() + " YearWeek(s)";
        }
        var text = $(".yearweekBlock span ul li .select2-search__field").val();
        $(".yearweekBlock span ul li .select2-search__field").val(text.replace(ftcount, ''));
        $('.yearweekBlock span ul li .select2-search__field[placeholder=""]').attr('style', 'width:100%');
    });


}
function AddItemInYearWeekSelect2MultiCheckBoxObj(id, IsChecked) {
    if (YearWeekSelect2MultiCheckBoxObj.length > 0) {
        let index = YearWeekSelect2MultiCheckBoxObj.findIndex(x => x.id == id);
        if (index > -1) {
            YearWeekSelect2MultiCheckBoxObj[index]["IsChecked"] = IsChecked;
        }
        else {
            YearWeekSelect2MultiCheckBoxObj.push({ "id": id, "IsChecked": IsChecked });
        }
    }
    else {
        YearWeekSelect2MultiCheckBoxObj.push({ "id": id, "IsChecked": IsChecked });
    }
}
function IsCheckedAllOptionYearWeek(trueOrFalse) {
    $.map($('#' + YearWeek_id_selectElement + ' option'), function (option) {
        AddItemInYearWeekSelect2MultiCheckBoxObj(option.value, trueOrFalse);
    });
    $('#' + YearWeek_id_selectElement + " > option").not(':first').prop("selected", trueOrFalse);
    //This will select all options and adds in Select2
    $("#" + YearWeek_id_selectElement).trigger("change");//This will effect the changes
    /* $(".select2-results__option").not(':first').attr("aria-selected", trueOrFalse);*/
    //This will make grey color of selected options

    $("input[id^='" + YearWeekstaticWordInID + "']").prop("checked", trueOrFalse);
}
function getYearWeekCalMultiplecheck() {
    $('.yearweekBlock span ul li .select2-search__field[placeholder=""]').attr('style', 'width:100%');
    $(".yearweekBlock span  .select2-selection__rendered").find('.select2-selection__choice').remove();
    YearWeekAllselected = $("#" + YearWeekstaticWordInID + "0").prop('checked') ? true : false;
    var tcount = YearWeekTotalchecked;
    var ftcount = "";
    if (tcount == 0) {
        ftcount = "All Year and Weeks";
    }
    else {
        ftcount = tcount.toString() + " Year and Week(s)";
    }
    $(".yearweekBlock span ul li .select2-search__field").val(ftcount);

}
//#endregion Multiple Select Drowpdown  With CheckBox V2-1109
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
//#region Search
var dtBBUKPIEnterpriseTable;
var dtBBUKPIEnterpriseTableForExport;
$("#btnMRDataAdvSrch").on('click', function (e) {
    run = true;
    searchresult = [];
    EnterpriseAdvSearch();
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    dtBBUKPIEnterpriseTable.page('first').draw('page');
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    //EnterpriseAdvSearch(dtBBUKPIEnterpriseTable.length);
    EnterpriseAdvSearch();
    dtBBUKPIEnterpriseTable.page('first').draw('page');
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', '#liPdf', function () {
    $(".buttons-pdf")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', '#liPrint', function () {
    $(".buttons-print")[0].click();
    $('#mask').trigger('click');
});

function EnterpriseAdvSearch() {
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



    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#advsearchfilteritems").html(searchitemhtml);

    $(".filteritemcount").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}
function clearAdvanceSearch() {
    selectCount = 0;
    $("#MaterialRequestId").val("");
    $("#Description").val("");
    $("#Status").val("").trigger('change');
    $("#Created").val("");
    $('#AccountId').val("").trigger('change');
    $("#RequiredDate").val("");
    $("#Completed").val("");
    $("#advsearchfilteritems").html('');
    $(".filteritemcount").text(selectCount);

}
var titleArray = [];
var classNameArray = [];
var order = '1';
var orderDir = 'asc';
function generateBBUKPIEnterpriseDataTable() {
    if ($(document).find('#bbukpienterpriseSearch').hasClass('dataTable')) {
        dtBBUKPIEnterpriseTable.destroy();
    }
    dtBBUKPIEnterpriseTable = $("#bbukpienterpriseSearch").DataTable({
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
                var filterinfoarray = getfilterinfoarraybbukpi($("#txtColumnSearch"), $('#advsearchsidebar'));
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
                            setsearchuibbukpi(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
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
        //fixedColumns: {
        //    leftColumns: 3,
        //},
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'BBU KPI Enterprise List'
            },
            {
                extend: 'print',
                title: 'BBU KPI Enterprise List'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'BBU KPI Enterprise List',
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
                title: 'BBU KPI Enterprise List'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/BBUKPIEnterprise/GetBBUKPIEnterpriseGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.CustomQueryDisplayId = CustomQueryDisplayId;
                d.SubmitStartDateVw = ValidateDate(SubmitStartDateVw);
                d.SubmitEndDateVw = ValidateDate(SubmitEndDateVw);
                d.CreateStartDateVw = ValidateDate(CreateStartDateVw);
                d.CreateEndDateVw = ValidateDate(CreateEndDateVw);
                d.Sites = sitefilters;
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.Order = order;
                //d.orderDir = orderDir;
                d.YearWeekfilters = yearweekfiltersenterprise;
            },
            "dataSrc": function (result) {
                let colOrder = dtBBUKPIEnterpriseTable.order();
                orderDir = colOrder[0][1];

                var i = 0;
                //SetMultiSelectAction(CustomQueryDisplayId);
                totalcount = result.recordsTotal;

                //HidebtnLoader("btnsortmenu");
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
                    "data": "Year", "autoWidth": false, "bSearchable": true, "bSortable": true, "name": "0", "mRender": function (data, type, row) {
                        return '<span>' + data + '</span>';
                    }
                },
                {
                    "data": "WeekNumber",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "name": "1",
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_psearch href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "weekStart", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "weekEnd", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                /*{ "data": "Created", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4", "className": "text-right" },*/
                { "data": "Created", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                { "data": "PMPercentCompleted", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right", "name": "5" },
                { "data": "WOBacklogCount", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right", "name": "6" },
                { "data": "phyInvAccuracy", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right", "name": "7" },

                /*hiden*/
                { "data": "pMFollowUpComp", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "8" },
                { "data": "activeMechUsers", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "9" },
                { "data": "rCACount", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "10" },
                { "data": "tTRCount", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "11" },
                { "data": "invValueOverMax", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "12" },
                { "data": "cycleCountProgress", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "13" },
                { "data": "eVTrainingHrs", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "14" },
                { "data": "SiteName", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "15" },
                
            ],
        columnDefs: [
            {
                //targets: [0, 1, 2],
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function (settings, json) {
            SetPageLengthMenu();
            //----------conditional column hiding-------------//
            //var api = new $.fn.dataTable.Api(settings);
            //var columns = dtBBUKPIEnterpriseTable.settings().init().columns;
            //var arr = [];
            //var j = 0;
            //while (j < json.hiddenColumnList.length) {
            //    var clsname = '.' + json.hiddenColumnList[j];
            //    var title = dtBBUKPIEnterpriseTable.columns(clsname).header();
            //    titleArray.push(title[0].innerHTML);
            //    classNameArray.push(clsname);
            //    dtBBUKPIEnterpriseTable.columns(clsname).visible(false);
            //    var sortMenuItem = '.dropdown-menu' + ' ' + clsname;
            //    $(sortMenuItem).remove();

            //    //---hide adv search items---
            //    var advclsname = '.' + "prc-" + json.hiddenColumnList[j];
            //    $(document).find(advclsname).hide();
            //    j++;
            //}
            //----------------------------------------------//

            $("#BBUKPIEnterpriseGridAction :input").removeAttr("disabled");
            $("#BBUKPIEnterpriseGridAction :button").removeClass("disabled");
            DisableExportButton($("#bbukpienterpriseSearch"), $(document).find('.import-export'));
        }
    });
}
$('#bbukpienterpriseSearch').find('th').click(function () {
    if ($(this).data('col') !== undefined && $(this).data('col') !== '') {
        run = true;
        order = $(this).data('col');
    }

});
$(document).on('click', '#bbukpienterpriseSearch_paginate .paginate_button', function () {
    EnterpriseAdvSearch();
    run = true;
});
$(document).on('change', '#bbukpienterpriseSearch_length .searchdt-menu', function () {
    EnterpriseAdvSearch();
    run = true;
});
$("#ddlSites").change(function () {

    run = true;
    var siteid = $(this).val();
    if (siteid == "") {
        sitefilters = "";
    }
    else {
        sitefilters = siteid;
    }
    localStorage.setItem("sitevalue", siteid);
    dtBBUKPIEnterpriseTable.page('first').draw('page');

});
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            dtBBUKPIEnterpriseTableForExport = $("#bbukpienterpriseSearchForExport").DataTable();
            var SubmitStartDateVw = ValidateDate(SubmitStartDateVw);
            var SubmitEndDateVw = ValidateDate(SubmitEndDateVw);
            var CreateStartDateVw = ValidateDate(CreateStartDateVw);
            var CreateEndDateVw = ValidateDate(CreateEndDateVw);
            var Sites = $("#ddlSites").val().join();
            var SearchText = LRTrim($(document).find('#txtColumnSearch').val());
            var colname = order;
            var coldir = orderDir;

            //var txtsearchval = LRTrim($("#txtColumnSearch").val());
            var jsonResult = $.ajax({
                url: '/BBUKPIEnterprise/GetBBUKPIEnterprisePrintData?page=all',
                data: {
                    CustomQueryDisplayId: CustomQueryDisplayId,
                    SubmitStartDateVw: SubmitStartDateVw,
                    SubmitEndDateVw: SubmitEndDateVw,
                    CreateStartDateVw: CreateStartDateVw,
                    CreateEndDateVw: CreateEndDateVw,
                    Sites: Sites,
                    SearchText: SearchText,
                    colname: colname,
                    coldir: coldir,
                    YearWeekfilters: yearweekfiltersenterprise
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#bbukpienterpriseSearchForExport thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.WeekNumber != null) {
                    item.WeekNumber = item.WeekNumber;
                }
                else {
                    item.WeekNumber = "";
                }
                if (item.Year != null) {
                    item.Year = item.Year;
                }
                else {
                    item.Year = "";
                }
                if (item.SiteName != null) {
                    item.SiteName = item.SiteName;
                }
                else {
                    item.SiteName = "";
                }
                if (item.Created != null) {
                    item.Created = item.Created;
                }
                else {
                    item.Created = "";
                }
                if (item.PMPercentCompleted != null) {
                    item.PMPercentCompleted = item.PMPercentCompleted;
                }
                else {
                    item.PMPercentCompleted = "";
                }
                if (item.activeMechUsers != null) {
                    item.activeMechUsers = item.activeMechUsers;
                }
                else {
                    item.activeMechUsers = "";
                }
                if (item.pMFollowUpComp != null) {
                    item.pMFollowUpComp = item.pMFollowUpComp;
                }
                else {
                    item.pMFollowUpComp = "";
                }
                if (item.WOBacklogCount != null) {
                    item.WOBacklogCount = item.WOBacklogCount;
                }
                else {
                    item.WOBacklogCount = "";
                }
                if (item.rCACount != null) {
                    item.rCACount = item.rCACount;
                }
                else {
                    item.rCACount = "";
                }
                if (item.tTRCount != null) {
                    item.tTRCount = item.tTRCount;
                }
                else {
                    item.tTRCount = "";
                }
                if (item.invValueOverMax != null) {
                    item.invValueOverMax = item.invValueOverMax;
                }
                else {
                    item.invValueOverMax = "";
                }
                if (item.phyInvAccuracy != null) {
                    item.phyInvAccuracy = item.phyInvAccuracy;
                }
                else {
                    item.phyInvAccuracy = "";
                }
                if (item.cycleCountProgress != null) {
                    item.cycleCountProgress = item.cycleCountProgress;
                }
                else {
                    item.cycleCountProgress = "";
                }
                if (item.eVTrainingHrs != null) {
                    item.eVTrainingHrs = item.eVTrainingHrs;
                }
                else {
                    item.eVTrainingHrs = "";
                }
                if (item.weekStart != null) {
                    item.weekStart = item.weekStart;
                }
                else {
                    item.weekStart = "";
                }
                if (item.weekEnd != null) {
                    item.weekEnd = item.weekEnd;
                }
                else {
                    item.weekEnd = "";
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
                header: $("#bbukpienterpriseSearchForExport thead tr th").map(function (key) {
                    return this.innerHTML;
                    //if ($(this).parents('.materialRequestinnerDataTable').length == 0 && this.innerHTML) {
                    //    return this.innerHTML;
                    //}
                }).get()
            };
        }
    });
});
$(document).on('click', '.lnk_psearch', function (e) {
    e.preventDefault();
    var index_row = $('#bbukpienterpriseSearch tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtBBUKPIEnterpriseTable.row(row).data();
    var BBUKPIId = data.BBUKPIId;
    var titletext = $('#bbuenterprisesearchtitle').text();
    localStorage.setItem("mrstatustext", titletext);
    $.ajax({
        url: "/BBUKPIEnterprise/BBUKPIEnterpriseDetails",
        type: "POST",
        data: { BBUKPIId: BBUKPIId },
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#BBUKPIEnterpriseContainer').html(data);
            $(document).find('#spnlinkToSearch').text(titletext);
            SetFixedHeadStyle();
        },
        complete: function () {
            colorarray = [];
            Activity(BBUKPIId);
            CloseLoader();
        }
    });
});

function getfilterinfoarraybbukpi(txtsearchelement, advsearchcontainer) {
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
function setsearchuibbukpi(data, txtsearchelement, advcountercontainer, searchstringcontainer) {
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

$(document).on('click', '.bbuenterprisesearchdrpbox', function (e) {
    $(document).find('#txtColumnSearch').val('');
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $('.itemcount').text(0);
    run = true;
    if ($(this).attr('id') == '1') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("enterprisestatus");
        if (val == '1' || val == '4' || val == '5' || val == '6' || val == '7' || val == '12') {
            $('#cmbcreateview').val(val).trigger('change');
        }
        $(document).find('#EnterpriseDateRangeModalForCreateDate').modal('show');
        return;
    }
    else {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('CreateStartDateVw');
        localStorage.removeItem('CreateEndDateVw');
        $(document).find('#cmbcreateview').val('').trigger('change');
    }
    if ($(this).attr('id') == '3') {
        $(document).find('#searcharea').hide("slide");
        var val = localStorage.getItem("enterprisestatus");
        if (val == '3' || val == '8' || val == '9' || val == '10' || val == '11' || val == '13') {
            $('#cmbsubmitview').val(val).trigger('change');
        }
        $(document).find('#EnterpriseDateRangeModalForSubmitDate').modal('show');
        return;
    }
    else {
        SubmitStartDateVw = '';
        SubmitEndDateVw = '';
        localStorage.removeItem('SubmitStartDateVw');
        localStorage.removeItem('SubmitEndDateVw');
        $(document).find('#cmbsubmitview').val('').trigger('change');
    }
    var val = localStorage.getItem("BBUKPIENTERPRISESTATUS");

    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("BBUKPIENTERPRISESTATUS", optionval);
    localStorage.setItem("enterprisestatus", optionval);
    CustomQueryDisplayId = optionval;
    $(document).find('#bbuenterprisesearchtitle').text($(this).text());
    ShowbtnLoaderclass("LoaderDrop");
    EnterpriseAdvSearch();
    dtBBUKPIEnterpriseTable.page('first').draw('page');
});
$(document).on('keyup', '#bbuenterprisesearctxtbox', function (e) {
    var tagElems = $(document).find('#bbuenterprisesearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('change', '#cmbcreateview', function (e) {
    var thielement = $(this);
    CustomQueryDisplayId = thielement.val();
    if (thielement.val() == '12') {
        var strtlocal = localStorage.getItem('EnterpriseCreateStartDateVw');
        if (strtlocal) {
            CreateStartDateVw = strtlocal;
        }
        else {
            CreateStartDateVw = today;
        }
        var endlocal = localStorage.getItem('EnterpriseCreateEndDateVw');
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
        localStorage.removeItem('EnterpriseCreateStartDateVw');
        localStorage.removeItem('EnterpriseCreateEndDateVw');
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
    if (daterangeval != '12') {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('EnterpriseCreateStartDateVw');
        localStorage.removeItem('EnterpriseCreateEndDateVw');
    }
    else {
        localStorage.setItem('EnterpriseCreateStartDateVw', CreateStartDateVw);
        localStorage.setItem('EnterpriseCreateEndDateVw', CreateEndDateVw);
    }
    $(document).find('#EnterpriseDateRangeModalForCreateDate').modal('hide');
    var text = $('#bbuenterprisesearchListul').find('li').eq(0).text();

    if (daterangeval != '12')

        text = text + " - " + $(document).find('#cmbcreateview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#createdaterange').val();

    $('#bbuenterprisesearchtitle').text(text);
    $("#bbuenterprisesearchListul li").removeClass("activeState");
    $("#bbuenterprisesearchListul li").eq(0).addClass('activeState');
    //localStorage.setItem("BBUKPIENTERPRISESTATUS", daterangeval);
    localStorage.setItem("BBUKPIENTERPRISESTATUS", daterangeval);
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');

    CustomQueryDisplayId = daterangeval;
    //if (layoutType == 1) {
    //    cardviewstartvalue = 0;
    //    grdcardcurrentpage = 1;

    //    LayoutFilterinfoUpdate();
    //    ShowCardView();
    //}
    if (daterangeval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtBBUKPIEnterpriseTable.page('first').draw('page');
    }

});

$(document).on('change', '#cmbsubmitview', function (e) {
    var thielement = $(this);
    CustomQueryDisplayId = thielement.val();
    if (thielement.val() == '13') {
        var strtlocal = localStorage.getItem('EnterpriseSubmitStartDateVw');
        if (strtlocal) {
            SubmitStartDateVw = strtlocal;
        }
        else {
            SubmitStartDateVw = today;
        }
        var endlocal = localStorage.getItem('EnterpriseSubmitEndDateVw');
        if (endlocal) {
            SubmitEndDateVw = endlocal;
        }
        else {
            SubmitEndDateVw = today;
        }
        $(document).find('#timeperiodcontainerForSubmitDate').show();
        $(document).find('#submitdaterange').daterangepicker({
            format: 'MM/DD/YYYY',
            startDate: SubmitStartDateVw,
            endDate: SubmitEndDateVw,
            "locale": {
                "applyLabel": getResourceValue("JsApply"),
                "cancelLabel": getResourceValue("CancelAlert")
            }
        }, function (start, end, label) {
            SubmitStartDateVw = start.format('MM/DD/YYYY');
            SubmitEndDateVw = end.format('MM/DD/YYYY');
        });
    }
    else {
        SubmitStartDateVw = '';
        SubmitEndDateVw = '';
        localStorage.removeItem('EnterpriseSubmitStartDateVw');
        localStorage.removeItem('EnterpriseSubmitEndDateVw');
        $(document).find('#timeperiodcontainerForSubmitDate').hide();
    }
})

$(document).on('click', '#btntimeperiodForSubmitDate', function (e) {
    run = true;
    var daterangeval = $(document).find('#cmbsubmitview').val();
    if (daterangeval == '') {
        return;
    }
    CustomQueryDisplayId = daterangeval;
    if (daterangeval != '13') {
        SubmitStartDateVw = '';
        SubmitEndDateVw = '';
        localStorage.removeItem('EnterpriseSubmitStartDateVw');
        localStorage.removeItem('EnterpriseSubmitEndDateVw');
    }
    else {
        localStorage.setItem('EnterpriseSubmitStartDateVw', SubmitStartDateVw);
        localStorage.setItem('EnterpriseSubmitEndDateVw', SubmitEndDateVw);
    }
    $(document).find('#EnterpriseDateRangeModalForSubmitDate').modal('hide');
    var text = $('#bbuenterprisesearchListul').find('li').eq(2).text();

    if (daterangeval != '13')

        text = text + " - " + $(document).find('#cmbsubmitview option[value=' + daterangeval + ']').text();
    else
        text = text + " - " + $('#submitdaterange').val();

    $('#bbuenterprisesearchtitle').text(text);
    $("#bbuenterprisesearchListul li").removeClass("activeState");
    $("#bbuenterprisesearchListul li").eq(2).addClass('activeState');
    localStorage.setItem("BBUKPIENTERPRISESTATUS", daterangeval);
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');

    CustomQueryDisplayId = daterangeval;
    if (daterangeval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtBBUKPIEnterpriseTable.page('first').draw('page');
    }

});
//#endregion

//#region Details
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../BBUKPIEnterprise/Index?page=BBUKPI_Enterprise";
});
function Activity(BBUKPIId) {
    $.ajax({
        "url": "/BBUKPIEnterprise/LoadActivity",
        data: { ObjectId: BBUKPIId },
        type: "POST",
        datatype: "json",
        success: function (data) {
            $(document).find('#activityitems').html(data);
            $(document).find("#activityList").mCustomScrollbar({
                theme: "minimal"
            });
        },
        complete: function () {
            $(document).find('#activitydataloader').hide();
            var ftext = '';
            var bgcolor = '';
            $(document).find('#activityitems').find('.activity-header-item').each(function () {
                var thistext = LRTrim($(this).text());
                if (ftext == '' || ftext != thistext) {
                    var bgcolorarr = colorarray.filter(function (a) {
                        return a.string == thistext;
                    });
                    if (bgcolorarr.length == 0) {
                        bgcolor = getRandomColor();
                        var thisval = new colorobject(thistext, bgcolor);
                        colorarray.push(thisval);
                    }
                    else {
                        bgcolor = bgcolorarr[0].color;
                    }
                }
                $(this).attr('style', 'color:' + bgcolor + ' !important;border:1px solid ' + bgcolor + ' !important;');
                ftext = thistext;
            });
            LoadComments(BBUKPIId);
        }
    });
}
var colorarray = [];
function colorobject(string, color) {
    this.string = string;
    this.color = color;
}

function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}
function LoadComments(BBUKPIId) {
    $.ajax({
        "url": "/BBUKPIEnterprise/LoadComments",
        data: { ObjectId: BBUKPIId },
        type: "POST",
        datatype: "json",
        success: function (data) {
            var getTexttoHtml = textToHTML(data);
            $(document).find('#commentstems').html(getTexttoHtml);
            $(document).find("#commentsList").mCustomScrollbar({
                theme: "minimal"
            });
        },
        complete: function () {
            var ftext = '';
            var bgcolor = '';
            $(document).find('#commentsdataloader').hide();
            $(document).find('#commentstems').find('.comment-header-item').each(function () {
                var thistext = LRTrim($(this).text());
                if (ftext == '' || ftext != thistext) {
                    var bgcolorarr = colorarray.filter(function (a) {
                        return a.string == thistext;
                    });
                    if (bgcolorarr.length == 0) {
                        bgcolor = getRandomColor();
                        var thisval = new colorobject(thistext, bgcolor);
                        colorarray.push(thisval);
                    }
                    else {
                        bgcolor = bgcolorarr[0].color;
                    }
                }
                $(this).attr('style', 'color:' + bgcolor + '!important;border:1px solid' + bgcolor + '!important;');
                ftext = LRTrim($(this).text());
            });
            var loggedinuserinitial = LRTrim($('#hdr-comments-add').text());
            var avlcolor = colorarray.filter(function (a) {
                return a.string == loggedinuserinitial;
            });
            if (avlcolor.length == 0) {
                $('#hdr-comments-add').attr('style', 'border:1px solid #264a7c !important;').show();
            }
            else {
                $('#hdr-comments-add').attr('style', 'color:' + avlcolor[0].color + ' !important;border:1px solid ' + avlcolor[0].color + '!important;').show();
            }
            $('.kt-notes__body a').attr('target', '_blank');
        }
    });
}
//#region CKEditor
$(document).on("focus", "#wotxtcommentsnew", function () {
    $(document).find('.ckeditorarea').show();
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.ckeditorarea').hide();
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    //ClearEditor();
    LoadCkEditorBBUEnterprise('wotxtcomments');
    $("#wotxtcommentsnew").hide();
    $(".ckeditorfield").show();
});
$(document).on('click', '#btnsavecommands', function () {
    var selectedUsers = [];
    const data = getDataFromTheEditor();
    if (!data) {
        return false;
    }
    var BBUKPIId = $(document).find('#BBUKPIEnterpriseModel_BBUKPIId').val();
    var noteId = 0;
    if (LRTrim(data) == "") {
        return false;
    }
    else {
        $(document).find('.ck-content').find('.mention').each(function (item, index) {
            selectedUsers.push($(index).data('user-id'));
        });
        $.ajax({
            url: '/BBUKPIEnterprise/AddComments',
            type: 'POST',
            beforeSend: function () {
                ShowLoader();
            },
            data: {
                BBUKPIId: BBUKPIId,
                content: data,
                noteId: noteId,
            },
            success: function (data) {
                if (data.Result == "success") {
                    var message;
                    if (data.mode == "add") {
                        SuccessAlertSetting.text = getResourceValue("CommentAddAlert");
                    }
                    else {
                        SuccessAlertSetting.text = getResourceValue("CommentUpdateAlert");
                    }
                    swal(SuccessAlertSetting, function () {
                        RedirectToEnterpriseDetail(BBUKPIId);
                    });
                }
                else {
                    ShowGenericErrorOnAddUpdate(data);
                    CloseLoader();
                }
            },
            complete: function () {
                ClearEditor();
                $("#wotxtcommentsnew").show();
                $(".ckeditorfield").hide();
                selectedUsers = [];
                selectedUnames = [];
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
});
$(document).on('click', '#commandCancel', function () {
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    ClearEditor();
    $("#wotxtcommentsnew").show();
    $(".ckeditorfield").hide();
});
$(document).on('click', '.editcomments', function () {
    $(document).find(".ckeditorarea").each(function () {
        $(this).html('');
    });
    $("#wotxtcommentsnew").show();
    $(".ckeditorfield").hide();
    var notesitem = $(this).parents('.kt-notes__item').eq(0);
    notesitem.find('.ckeditorarea').html(CreateEditorHTML('wotxtcommentsEdit'));
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    var rawHTML = $.parseHTML($(this).parents('.kt-notes__item').find('.kt-notes__body').find('.originalContent').html());
    LoadCkEditorEditBBUEnterprise('wotxtcommentsEdit', rawHTML);
    $(document).find('.ckeditorarea').hide();
    notesitem.find('.ckeditorarea').show();
    notesitem.find('.kt-notes__body').hide();
    notesitem.find('.commenteditdelearea').hide();
});

$(document).on('click', '.deletecomments', function () {
    DeleteWoNote($(this).attr('id'));
});
$(document).on('click', '.btneditcomments', function () {
    var data = getDataFromTheEditor();
    var BBUKPIId = $(document).find('#BBUKPIEnterpriseModel_BBUKPIId').val();
    var noteId = $(this).parents('.kt-notes__item').find('.editcomments').attr('id');
    var updatedindex = $(this).parents('.kt-notes__item').find('.hdnupdatedindex').val();
    $.ajax({
        url: '/BBUKPIEnterprise/AddComments',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        data: { BBUKPIId: BBUKPIId, content: LRTrim(data), noteId: noteId, updatedindex: updatedindex },
        success: function (data) {
            if (data.Result == "success") {
                var message;
                if (data.mode == "add") {
                    SuccessAlertSetting.text = getResourceValue("CommentAddAlert");
                }
                else {
                    SuccessAlertSetting.text = getResourceValue("CommentUpdateAlert");
                }
                swal(SuccessAlertSetting, function () {
                    RedirectToEnterpriseDetail(BBUKPIId);
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
                CloseLoader();
            }
        },
        complete: function () {
            // ClearEditorEdit();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

});
$(document).on('click', '.btncommandCancel', function () {
    ClearEditorEdit();
    $(document).find('.ckeditorarea').hide();
    $(this).parents('.kt-notes__item').find('.kt-notes__body').show();
    $(this).parents('.kt-notes__item').find('.commenteditdelearea').show();
});
function DeleteWoNote(notesId) {
    swal(CancelAlertSettingForCallback, function () {
        $.ajax({
            url: '/Base/DeleteComment',
            data: {
                _notesId: notesId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    SuccessAlertSetting.text = getResourceValue("CommentDeleteAlert");
                    swal(SuccessAlertSetting, function () {
                        RedirectToEnterpriseDetail($(document).find('#BBUKPIEnterpriseModel_BBUKPIId').val());
                    });
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function LoadCkEditorBBUEnterprise(equtxtcomments) {
    $(".toolbar-container").html('');
    ClearEditor();
    DecoupledEditor
        .create(document.querySelector('#' + equtxtcomments), {
            toolbar: ['heading', '|', 'bold', 'italic', 'alignment', 'link', 'numberedList', 'bulletedList', '|', 'fontFamily', 'fontSize', 'fontColor', 'fontBackgroundColor', '|', 'removeFormat'],
            extraPlugins: [MentionCustomization],
            mediaEmbed: { previewsInData: true },
            fontSize: {
                options: [8, 9, 10, 11, 12, 13, 14, 16, 18, 24, 30, 36, 48, 60, 72, 96]
            }/*,
            mention: {
                feeds: [
                    {
                        marker: '@',
                        feed: getFeedItems,
                    }
                ]
            }*/

        })
        .then(editor => {
            //editor.destroy();
            const toolbarContainer = document.querySelector('main .toolbar-container');
            toolbarContainer.prepend(editor.ui.view.toolbar.element);
            theEditor = editor;
            editor.execute('listStyle', { type: 'decimal' });
            editor.model.document.on('change:data', () => {

            });
        })
        .catch(err => {
            console.error(err.stack);
        });
}

function LoadCkEditorEditBBUEnterprise(equtxtcomments, data) {
    $(".toolbar-containerEdit").html('');
    ClearEditorEdit();
    DecoupledEditor
        .create(document.querySelector('#' + equtxtcomments), {
            toolbar: ['heading', '|', 'bold', 'italic', 'alignment', 'link', 'numberedList', 'bulletedList', '|', 'fontFamily', 'fontSize', 'fontColor', 'fontBackgroundColor', '|', 'removeFormat'],
            extraPlugins: [MentionCustomization],
            mediaEmbed: { previewsInData: true },
            fontSize: {
                options: [8, 9, 10, 11, 12, 13, 14, 16, 18, 24, 30, 36, 48, 60, 72, 96]
            }/*,
            mention: {
                feeds: [
                    {
                        marker: '@',
                        feed: getFeedItems,
                    }
                ]

            }*/
        })
        .then(editor => {
            //editor.destroy();
            var getParseHtml = GetParseHtml(data);
            editor.setData(getParseHtml);
            const toolbarContainer = document.querySelector('main .toolbar-containerEdit');
            toolbarContainer.prepend(editor.ui.view.toolbar.element);
            theEditor = editor;
            editor.execute('listStyle', { type: 'decimal' });
            editor.model.document.on('change:data', () => {

            });
        })
        .catch(err => {
            console.error(err.stack);
        });
}
//#endregion
function RedirectToEnterpriseDetail(BBUKPIId, mode) {
    $.ajax({
        url: "/BBUKPIEnterprise/BBUKPIEnterpriseDetails",
        type: "POST",
        dataType: 'html',
        data: { BBUKPIId: BBUKPIId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#BBUKPIEnterpriseContainer').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("mrstatustext"));
            SetFixedHeadStyle();
        },
        complete: function () {
            colorarray = [];
            Activity(BBUKPIId);
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#region Reopen
$(document).on('click', '#btnReopen', function () {
    var Status = "Reopen";
    var message = getResourceValue("BBUKPIReopenAlertMessage");
    ChangeBBUKPIStatus(Status, message)
});
function ChangeBBUKPIStatus(Status, Message) {
    var BBUKPIId = $('#BBUKPIEnterpriseModel_BBUKPIId').val();
    CancelAlertSetting.text = Message;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: "/BBUKPIEnterprise/ChangeStatus",
            type: "GET",
            dataType: 'json',
            data: { BBUKPIId: BBUKPIId, Status: Status },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                var message = "";
                if (data.Result == "success") {
                    if (Status == "Reopen") {
                        SuccessAlertSetting.text = getResourceValue("BBUKPIReopenAlert");
                    }
                    swal(SuccessAlertSetting, function () {
                        RedirectToEnterpriseDetail(BBUKPIId);
                    });
                }
                else {
                    message = "";
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
}
//#endregion
//#endregion

$("#ddlYearWeeks").change(function () {

    run = true;
    var id = $('#ddlYearWeeks option:selected').toArray().map(item => item.text).join();
    var ids = $(this).val();
    if (id == "") {
        yearweekfiltersenterprise = "";
    }
    else {
        yearweekfiltersenterprise = id;
    }
    localStorage.setItem("yearweekvalueenterprise", ids);
    localStorage.setItem("yearweekvalueenterprisetext", id);
    dtBBUKPIEnterpriseTable.page('first').draw('page');

});