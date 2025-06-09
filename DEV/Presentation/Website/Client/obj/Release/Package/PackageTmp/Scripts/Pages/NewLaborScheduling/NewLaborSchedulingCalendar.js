//#region global variable
var calendar;
var selectCount = 0;
var PersonnelList = [];
var CustomQueryDisplayId = 1;
var StartRequiredDate = '';
var EndRequiredDate = '';
var today = $.datepicker.formatDate('mm/dd/yy', new Date());
var StartScheduledDate = today;
var EndScheduledDate = today;
var LaborAvailableSelectedItemArray = [];
var AWOGridTotalGridItem = [];
var WorkOrderIds = [];
var LaborAvailabledt;
var SelectedLookupIdToAssigned = [];
var SelectedStatusAssigned = [];
var SelectedWoIdToAssigned = [];
var hGridfilteritemcount = 0;
var flag;
var ModelOpened = false;
var OldCustomQueryDisplayId = "1";
var gridname = "CalendarLaborScheduling_Search";
var PersonnelColor = [];
//#endregion

//#region calendar bind
$(document).ready(function () {
    $(".actionBar").fadeIn();
    $("#lsGridAction :input").attr("disabled", "disabled");
    $(document).find('.select2picker').select2({});
    GetUserAssignedDropdown();//V2-562 
    $(document).on('click', "#action", function () {
        $(".actionDrop").slideToggle();
    });
    $(document).on('focusout', "#action", function () {
        $(".actionDrop").fadeOut();
    });
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $('.overlay2').on('click', function () {
        $('#ApproveWOadvsearchcontainer').find('.sidebar').removeClass('active');
        $(document).find('.overlay2').fadeOut();
    });
    //$('#sidebarCollapse').on('click', function () {
    //    $('#sidebar').addClass('active');
    //    $('.overlay').fadeIn();
    //    $('.collapse.in').toggleClass('in');
    //    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    //});
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    //$('#ScheduledDateCalendar').val(CustomQueryDisplayId).select2();
    //ReloadAdvSearchFilterInfo();
    $(document).find(".assignedBlock").css("display", "inline-block");
    //$(document).find(".scheduledBlock").css("display", "inline-block");
    generateLaborSchedulingCalendar();
});
//#region Multiple Select Drowpdown  With CheckBox V2-562
var Select2MultiCheckBoxObj = [];
var id_selectElement = 'ddlUser';
var staticWordInID = 'state_';
var Totalchecked = 0;
var Allselected = false;
function GetUserAssignedDropdown() {
    $.map($('#' + id_selectElement + ' option'), function (option) {
        AddItemInSelect2MultiCheckBoxObj(option.value, false);
    });

    function formatResult(state) {
        if (Select2MultiCheckBoxObj.length > 0) {
            var stateId = staticWordInID + state.id;
            let index = Select2MultiCheckBoxObj.findIndex(x => x.id == state.id);
            if (index > -1) {
                var checkbox = "";
                if (index == 0) {
                    checkbox = $('<div class="checkbox"><input class="select2Checkbox checkboxAssignedUser " id="state_0" type="checkbox" ' + (Select2MultiCheckBoxObj[index]["IsChecked"] ? 'checked' : '') +
                        '><label for="checkboxstate_0 "> ' + state.text + '</label></div>', { id: 'state_0' });
                }
                else {
                    checkbox = $('<div class="checkbox"><input class="select2Checkbox checkboxAssignedUser" id="' + stateId + '" type="checkbox" ' + (Select2MultiCheckBoxObj[index]["IsChecked"] ? 'checked' : '') +
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

    let $select2 = $(document).find("#" + id_selectElement).select2(optionSelect2);
    $("#" + id_selectElement).find("option[value='']").attr("disabled", 'disabled');
    getCalMultiplecheck();
    $select2.on('select2:close', function () {

    });
    $select2.on("select2:select", function (event) {
        $("#" + staticWordInID + event.params.data.id).prop("checked", true);
        AddItemInSelect2MultiCheckBoxObj(event.params.data.id, true);
        //If all options are slected then selectAll option would be also selected.

        if (Select2MultiCheckBoxObj.filter(x => x.IsChecked === false).length === 1) {
            AddItemInSelect2MultiCheckBoxObj(0, true);
            $("#" + staticWordInID + "0").prop("checked", true);
        }
        Totalchecked = Totalchecked + 1;
        getCalMultiplecheck();
    });

    $select2.on("select2:unselect", function (event) {
        $("#" + staticWordInID + "0").prop("checked", false);
        AddItemInSelect2MultiCheckBoxObj(0, false);
        $("#" + staticWordInID + event.params.data.id).prop("checked", false);
        AddItemInSelect2MultiCheckBoxObj(event.params.data.id, false);
        Totalchecked = Totalchecked - 1;
        getCalMultiplecheck();
    });

    $(document).on("click", "#" + staticWordInID + "0", function () {
        var b = $("#" + staticWordInID + "0").is(':checked');
        IsCheckedAllOption(b);
        if (b == true) {
            Totalchecked = $('.checkboxAssignedUser:checked').length - 1;
        } else {
            Totalchecked = $('.checkboxAssignedUser:checked').length;
        }
        getCalMultiplecheck();
        $("#" + id_selectElement).select2("close");
    });
    $(document).on("click", ".checkboxAssignedUser", function (event) {
        let selector = "#" + this.id;
        let isChecked = false;
        if (this.id == "state_0") {
            isChecked = Select2MultiCheckBoxObj[Select2MultiCheckBoxObj.findIndex(x => x.id == "")]['IsChecked'] ? true : false;
        } else {
            isChecked = Select2MultiCheckBoxObj[Select2MultiCheckBoxObj.findIndex(x => x.id == this.id.replaceAll(staticWordInID, ''))]['IsChecked'] ? true : false;
        }
        $(selector).prop("checked", isChecked);
        getCalMultiplecheck();
    });
    $(document).on("click", ".assignedBlock span  .select2-selection__rendered", function (event) {
        $(".assignedBlock span ul li .select2-search__field").val("");
    });
    $(document).on("click", ".assignedBlock span  .select2-selection__rendered .select2-selection__clear", function (event) {
        IsCheckedAllOption(false);
        Totalchecked = $('.checkboxAssignedUser:checked').length;
        getCalMultiplecheck();
        $("#" + id_selectElement).select2("close");

    });
    $(document).on('focusout', '.assignedBlock span ul li .select2-search__field', function (e) {
        getCalMultiplecheck();
    });
    $(document).on('keypress', '.assignedBlock span ul li .select2-search__field', function (e) {
        var tcount = Totalchecked;
        var ftcount = "";
        if (tcount == 0) {
            ftcount = "All Personnel";
        }
        else {
            ftcount = tcount.toString() + " People";
        }
        var text = $(".assignedBlock span ul li .select2-search__field").val();
        $(".assignedBlock span ul li .select2-search__field").val(text.replace(ftcount, ''));
        $('.assignedBlock span ul li .select2-search__field[placeholder=""]').attr('style', 'width:100%');
    });
}
function AddItemInSelect2MultiCheckBoxObj(id, IsChecked) {
    if (Select2MultiCheckBoxObj.length > 0) {
        let index = Select2MultiCheckBoxObj.findIndex(x => x.id == id);
        if (index > -1) {
            Select2MultiCheckBoxObj[index]["IsChecked"] = IsChecked;
        }
        else {
            Select2MultiCheckBoxObj.push({ "id": id, "IsChecked": IsChecked });
        }
    }
    else {
        Select2MultiCheckBoxObj.push({ "id": id, "IsChecked": IsChecked });
    }
}
function IsCheckedAllOption(trueOrFalse) {
    $.map($('#' + id_selectElement + ' option'), function (option) {
        AddItemInSelect2MultiCheckBoxObj(option.value, trueOrFalse);
    });
    $('#' + id_selectElement + " > option").not(':first').prop("selected", trueOrFalse);
    //This will select all options and adds in Select2
    $("#" + id_selectElement).trigger("change");//This will effect the changes
    /* $(".select2-results__option").not(':first').attr("aria-selected", trueOrFalse);*/
    //This will make grey color of selected options

    $("input[id^='" + staticWordInID + "']").prop("checked", trueOrFalse);
}
function getCalMultiplecheck() {
    $('.assignedBlock span ul li .select2-search__field[placeholder=""]').attr('style', 'width:100%');
    $(".assignedBlock span  .select2-selection__rendered").find('.select2-selection__choice').remove();
    Allselected = $("#" + staticWordInID + "0").prop('checked') ? true : false;
    var tcount = Totalchecked;
    var ftcount = "";
    if (tcount == 0) {
        ftcount = "All Personnel";
    }
    else {
        ftcount = tcount.toString() + " People";
    }
    $(".assignedBlock span ul li .select2-search__field").val(ftcount);
    //alert($('#' + id_selectElement).val());
}
//#endregion Multiple Select Drowpdown  With CheckBox V2-562
function ISODateToDateformat(strDate) {
    try {
        //if (strDate && strDate.length > 0) {
        //    let dt = strDate.substring(0, 10).split('-');
        //    return dt[1] + '/' + dt[2] + '/' + dt[0];
        //}
        //var d=new Date("2015-03-25T12:00:00-06:00");
        var d = new Date(strDate);
        return $.datepicker.formatDate('mm/dd/yy', d);
    } catch (e) {
        return null;
    }
}
function getRandomColor(PersonnelId, count, total) {
    //var letters = '0123456789ABCDEF';
    //var color = '#';
    //for (var i = 0; i < 6; i++) {
    //    color += letters[Math.floor(Math.random() * 16)];
    //}
    //return color;
    //var color = "hsl(" + Math.random() * 360 + ", 100%, 75%)";
    //return color;
    //light
    //const hue = Math.floor(Math.random() * 360);
    //const saturation = Math.floor(Math.random() * (100 + 1)) + "%";
    //const lightness = Math.floor((1 + Math.random()) * (100 / 2 + 1)) + "%";
    //return "hsl(" + hue + ", " + saturation + ", " + lightness + ")";
    //dark


    //var RowCount = PersonnelColor.filter(function (obj) { return obj.PersonnelId === PersonnelId }).length;
    //var bgcolor = "";
    //if (RowCount === 0) {
    //    const hue = Math.floor(Math.random() * 360);
    //    const saturation = Math.floor(Math.random() * (100 + 1)) + "%";
    //    const lightness = Math.floor(Math.random() * (100 / 2 + 1)) + "%";
    //    //return "hsl(" + hue + ", " + saturation + ", " + lightness + ")";
    //    bgcolor = "hsl(" + hue + ", " + saturation + ", " + lightness + ")";
    //    PersonnelColor.push(new ColorObj(bgcolor, PersonnelId));
    //    return bgcolor;
    //}
    //else {
    //    bgcolor = PersonnelColor.filter(function (obj) { return obj.PersonnelId === PersonnelId })[0].bgcolor;
    //    return bgcolor;
    //}

    //const h = Math.floor(Math.random() * 360),
    //    s = Math.floor(Math.random() * 100) + '%',
    //    l = Math.floor(Math.random() * 60) + '%';
    //return "hsl(" + h + ", " + s + ", " + l + ")";

    //--------------start
    var lum = -0.1; //-0.25; -0.1
    var hex = String('#' + Math.random().toString(16).slice(2, 8).toUpperCase()).replace(/[^0-9a-f]/gi, '');
    if (hex.length < 6) {
        hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
    }
    var rgb = "#",
        c, i;
    for (i = 0; i < 3; i++) {
        c = parseInt(hex.substr(i * 2, 2), 16);
        c = Math.round(Math.min(Math.max(0, c + (c * lum)), 255)).toString(16);
        rgb += ("00" + c).substr(c.length);
    }
    //return rgb;
    //--------------end



    //var bgcolor = "hsl(" + (count * (360 / total) % 360) + ",100%,30%)";
    //var bgcolor = "hsl(" + (count * 137.508) + ",100%,30%)";
    var bgcolor = rgb;

    //var RowCount = PersonnelColor.filter(function (obj) { return obj.PersonnelId === PersonnelId }).length;

    //const hue = Math.floor(Math.random() * 360);
    //const saturation = Math.floor(Math.random() * (100 + 1)) + "%";
    //const lightness = Math.floor(Math.random() * (100 / 2 + 1)) + "%";
    //bgcolor = "hsl(" + hue + ", " + saturation + ", " + lightness + ")";
    //if (RowCount === 0) {
    //    PersonnelColor.push(new ColorObj(bgcolor, PersonnelId));
    //}
    return bgcolor;

    //return Math.floor(Math.random() * 16777215).toString(16).padStart(6, '0');
}
function distinctColors(count, PersonnelList) {
    PersonnelColor = [];
    var i = 0;
    for (hue = 0; hue < 360; hue += 360 / count) {
        PersonnelColor.push(new ColorObj(HSVtoRGB(hue, 100, 100), PersonnelList[i]["PersonnelId"]));
        i++;
    }
}
function HSVtoRGB(h, s, v) {
    var r, g, b, i, f, p, q, t;
    if (arguments.length === 1) {
        s = h.s, v = h.v, h = h.h;
    }
    i = Math.floor(h * 6);
    f = h * 6 - i;
    p = v * (1 - s);
    q = v * (1 - f * s);
    t = v * (1 - (1 - f) * s);
    switch (i % 6) {
        case 0: r = v, g = t, b = p; break;
        case 1: r = q, g = v, b = p; break;
        case 2: r = p, g = v, b = t; break;
        case 3: r = p, g = q, b = v; break;
        case 4: r = t, g = p, b = v; break;
        case 5: r = v, g = p, b = q; break;
    }
    //return {
    //    r: Math.round(r * 255),
    //    g: Math.round(g * 255),
    //    b: Math.round(b * 255)
    //};
    return "rgb(" + Math.round(r * 255) + "," + Math.round(g * 255) + "," + Math.round(b * 255) + ")";
}
function CalendarEvent(WorkOrderId, WorkOrderScheduleId, Title, Description, PersonnelId, PersonnelFull, StartDate, Hours, bgcolor, Partonorder, Tooltip) {
    this.id = WorkOrderScheduleId;
    this.WorkOrderId = WorkOrderId;
    this.WorkOrderScheduleId = WorkOrderScheduleId;
    this.title = Title;
    this.description = Description;
    this.PersonnelId = PersonnelId;
    this.PersonnelFull = PersonnelFull;
    this.start = StartDate;
    this.Hours = Hours;
    this.backgroundColor = bgcolor;
    this.allDay = true;
    //V2-838
    this.PartOnOrder = Partonorder;
    this.Tooltip = Tooltip;
}
function ColorObj(bgcolor, personnelId) {
    this.bgcolor = bgcolor;
    this.personnelId = personnelId;
}
function BindPersonnelList(ListPersonnel) {
    var PersonnelList = $('#PersonnelList > tbody');
    PersonnelList.html('');
    //distinctColors(data.ListPersonnel.length, data.ListPersonnel);
    $.each(ListPersonnel, function (i, item) {
        //var bgcolor = PersonnelColor.filter(function (color) { return color.personnelId === item.PersonnelId; })[0].bgcolor;                        
        //var bgcolor = getRandomColor(item.PersonnelId, i, ListPersonnel.length);
        var bgcolor = PersonnelColor.filter(function (x) { return x.personnelId === item.PersonnelId })[0].bgcolor;
        //data.ListPersonnel[i]["bgcolor"] = bgcolor;
        PersonnelList.append('<tr><td><i class="fa fa-circle" style="color:' + bgcolor + ';"></i></td>' +
            '<td style="max-width:153px;"><p style = "padding:0; margin:0; word-break:break-word;"> ' + item.PersonnelFull + ' </p></td >' +
            '<td style="text-align:right !important;">' + item.ScheduledHours + '</td></tr>');
    });
}
function RetrieveCalendarData(startStr, endStr, successCallback, failureCallback) {
    $.ajax({
        url: '/LaborScheduling/GetLaborSchedulingCalendarData',
        type: 'POST',
        dataType: 'json',
        data: {
            StartDt: ISODateToDateformat(startStr),
            EndDt: ISODateToDateformat(endStr),
            "PersonnelList": PersonnelList
        },
        beforeSend: function () {
        },
        //contentType: 'json',
        success: function (data) {
            let CalendarEventData = [];
            if (data.LaborSchedulingList && data.LaborSchedulingList.length > 0) {
                PersonnelColor = [];
                if (data.ListPersonnel && data.ListPersonnel.length > 0) {
                    GetPersonnelColorFromStorage(data.ListPersonnel.map(function (x) { return x.PersonnelId }));
                    BindPersonnelList(data.ListPersonnel);
                    SetPersonnelColorToStorage(JSON.stringify(PersonnelColor));
                }
                $(document).find("#PersonnelList > tbody").css("visibility", "visible");
                $.each(data.LaborSchedulingList, function (i, d) {
                    var bgcolor = PersonnelColor.filter(function (color) { return color.personnelId === d.PersonnelId; })[0].bgcolor;
                    
                    var obj = new CalendarEvent(d.WorkOrderId, d.WorkOrderScheduleId, d.Title, d.Description, d.PersonnelId, d.PersonnelFull,
                        d.ScheduledStartDate, d.ScheduledHours, bgcolor, d.PartOnOrder, d.Tooltip);
                    CalendarEventData.push(obj);
                });
                successCallback(CalendarEventData);
            }
            else {
                successCallback(CalendarEventData);
                $('#PersonnelList > tbody').html('<tr><td colspan="3" style="text-align: center">No data available in table<td></tr>');
                SetPersonnelColorToStorage();
            }
        },
        complete: function () {
        },
        failure: function (err) {
            failureCallback(err);
        }
    });
}
function generateLaborSchedulingCalendar() {
    var calendarEl = document.getElementById('m_calendar');

    calendar = new FullCalendar.Calendar(calendarEl, {
        plugins: ['interaction', 'dayGrid'],
        header: {
            //left: 'today prev next',
            left: 'prev next',
            center: 'title',
            right: 'dayGridMonth,dayGridWeek'
        },
        buttonText: {
            //next: '>',
            //nextYear: '>>',
            //prev: '<',
            //prevYear: '<<',
            //today: moment().locale('de', { months: "Januar_Februar_M&#228;rz_April_Mai_Juni_Juli_August_September_Oktober_November_Dezember".split("_") }).format("MMMM YYYY")
            today: 'Today',
            dayGridMonth: 'Month',
            dayGridWeek: 'Week',
        },
        //buttonIcons: {

        //},
        defaultDate: new Date(),
        navLinks: false, // no navigations to the new window in browser
        editable: true, // for both drag and resize event
        eventDurationEditable: false, // Determines whether the user can resize event between dates
        eventResourceEditable: true, // Determines whether the user can drag events between resources
        eventLimit: true, // allow "more" link when too many events
        views: {
            dayGridMonth: {
                eventLimit: 4 // only 4 event on a date block in month view
            }
        },
        loading: function (isLoading, view) {
            if (isLoading) {// isLoading gives boolean value                
                $('#calendarloader').show();
                $('.calenderoverlay').show();
            } else {
                $("#lsGridAction :input").removeAttr("disabled");
                $('#calendarloader').hide();
                $('.calenderoverlay').hide();
            }
        },
        events: function (info, successCallback, failureCallback) {
            RetrieveCalendarData(info.startStr, info.endStr, successCallback, failureCallback);
        },
        lazyFetching: false, // it will refresh for every calendar interaction
        eventRender: function (info) {
            var tooltip = new Tooltip(info.el, {
                title: info.event.extendedProps.Tooltip,
                placement: 'top',
                trigger: 'hover',
                container: 'body'
            });
            //V2-838
            if (info.event.extendedProps.PartOnOrder >0)
                info.el.querySelector('.fc-title').innerHTML = "<span>" + info.event.title + '<span  class="number-badge m - badge m - badge--purple">' + info.event.extendedProps.PartOnOrder +'</span>'+ "</span>";
        },
        eventDrop: function (info) {// drag and drop to an element         
            var CurrentDate = new Date().setHours(0, 0, 0, 0);
            var GivenDate = new Date(info.event.start.toISOString()).setHours(0, 0, 0, 0);

            if (GivenDate >= CurrentDate) {
                let returnStatus = DragScheduleCalendar(info.event.start.toISOString(), info.event.extendedProps.WorkOrderId, info.event.extendedProps.WorkOrderScheduleId);
                if (returnStatus !== "success") {
                    info.revert();
                }
            }
            else {
                ErrorAlertSetting.text = getResourceValue('ValidateScheduleValueNotPastDateJS');
                swal(ErrorAlertSetting, function () { });
                info.revert();
            }
        },
        selectable: false,
        dateClick: function (info) {//click on an empty area of a date               
            var CurrentDate = new Date().setHours(0, 0, 0, 0);
            var GivenDate = new Date(info.date).setHours(0, 0, 0, 0);

            if (GivenDate >= CurrentDate) {
                AddScheduleCalendar(info.date);
            }
            else {
                ErrorAlertSetting.text = getResourceValue('ValidateScheduleValueNotPastDateJS');
                swal(ErrorAlertSetting, function () { });
            }
        },
        eventClick: function (info) { //clicking on a data element            
            EditScheduleCalendar(info.event.extendedProps.WorkOrderScheduleId, info.event.extendedProps.WorkOrderId, info.event.extendedProps.PersonnelId);
        }
    });
    calendar.render();
}
function GetPersonnelColorFromStorage(PersonnelIdList) {
    var colArraystring = localStorage.getItem("LaborSchedulingColArray");
    var colArray = [];
    if (colArraystring && colArraystring !== null) {
        colArray = JSON.parse(colArraystring);
    }
    var bgcol = '';
    $.each(PersonnelIdList, function (i, p) {
        if (colArray && colArray !== null) {
            var color = colArray.filter(col => col.personnelId === p);
            if (color && color.length > 0) {
                PersonnelColor.push(new ColorObj(color[0].bgcolor, p));
            }
            else {
                bgcol = AssignColor();
                PersonnelColor.push(new ColorObj(bgcol, p));
            }
        }
    });
}
function AssignColor() {
    var len = 0, bgcol = '';
    do {
        bgcol = getRandomColor();
        len = PersonnelColor.filter(function (x) { x.bgcolor === bgcol }).length;
    }
    while (len > 0);
    return bgcol;
}
function SetPersonnelColorToStorage(colorArray) {
    localStorage.removeItem("LaborSchedulingColArray");
    if (colorArray && colorArray.length > 0) {
        localStorage.setItem("LaborSchedulingColArray", colorArray);
    }
}
$(document).on('click', '#idListView', function () {
    if (!$(this).hasClass('active')) {
        window.location = '/LaborScheduling/ListView';
    }

});
$(document).on('click', '#idCalendarView', function () {
    if (!$(this).hasClass('active')) {
        window.location = '/LaborScheduling/CalendarView';
    }
});
//#endregion

//#region Additional Search
//var filterinfoarray = [];
//function filterinfo(id, value) {
//    this.key = id;
//    this.value = value;
//}
//function getfilterinfoarray(txtsearchelement, advsearchcontainer) {
//    var filterinfoarray = [];
//    var f;
//    if (txtsearchelement !== "") {
//        f = new filterinfo('searchstring', LRTrim(txtsearchelement.val()));
//    }
//    else {
//        f = new filterinfo('searchstring', "");
//    }
//    filterinfoarray.push(f);
//    advsearchcontainer.find('.adv-item').each(function (index, item) {
//        if ($(this).parent('div').is(":visible")) {
//            f = new filterinfo($(this).attr('id'), $(this).val());
//            filterinfoarray.push(f);
//        }
//    });
//    return filterinfoarray;
//}
//function ReloadAdvSearchFilterInfo() {
//    $.ajax({
//        "url": "/Base/GetLayout",
//        "data": {
//            GridName: gridname
//        },
//        "async": false,
//        "dataType": "json",
//        "success": function (json) {
//            selectCount = 0;
//            if (json.FilterInfo !== '') {
//                setsearchui(JSON.parse(json.FilterInfo), "", $(".filteritemcount"), $("#advsearchfilteritems"));
//            }
//        }
//    });
//}
//function SaveAdvSearchFilterInfo() {
//    var filterinfoarray = getfilterinfoarray("", $('#advsearchsidebarcalendar'));
//    $.ajax({
//        "url": "/Base/CreateUpdateState",
//        "data": {
//            GridName: gridname,
//            LayOutInfo: "",
//            FilterInfo: JSON.stringify(filterinfoarray)
//        },
//        "async": false,
//        "dataType": "json",
//        "type": "POST",
//        "success": function () { return; }
//    });
//}
//function setsearchui(data, txtsearchelement, advcountercontainer, searchstringcontainer) {
//    var searchitemhtml = '';
//    $.each(data, function (index, item) {
//        if (item.key == 'searchstring' && item.value) {
//            var txtSearchval = item.value;
//            if (item.value) {
//                txtsearchelement.val(txtSearchval);
//                searchitemhtml = "";
//                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + txtsearchelement.attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
//            }
//            return false;
//        }
//        else {
//            if ($('#' + item.key).parent('div').is(":visible")) {
//                $('#' + item.key).val(item.value);
//                if (item.value) {
//                    selectCount++;
//                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
//                }
//            }
//            else if (item.key == 'advrequiredDatedaterange' && item.value !== '') {
//                $('#' + item.key).val(item.value);
//                if (item.value) {
//                    searchitemhtml = searchitemhtml + '<span style="display:none;" class="label label-primary tagTo" id="_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
//                }
//            }
//            if (item.key == 'RequiredDate') {
//                $("#RequiredDate").trigger('change.select2');
//            }
//            advcountercontainer.text(selectCount);
//        }
//    });
//    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
//    searchstringcontainer.html(searchitemhtml);
//}

//$(document).on('change', '#RequiredDate', function () {
//    var thisval = $(this).val();
//    CustomQueryDisplayId = thisval;
//    if (thisval == 12) {
//        $('#RequiredDatetimeperiodcontainer').show();
//    }
//    else {
//        $('#RequiredDatetimeperiodcontainer').hide();
//    }
//    switch (thisval) {
//        case '7':
//            $('#RequiredDatetimeperiodcontainer').hide();
//            StartRequiredDate = today;
//            EndRequiredDate = today;
//            break;
//        case '8':
//            $('#RequiredDatetimeperiodcontainer').hide();
//            StartRequiredDate = PreviousDateByDay(7);
//            EndRequiredDate = today;
//            break;
//        case '9':
//            $('#RequiredDatetimeperiodcontainer').hide();
//            StartRequiredDate = PreviousDateByDay(30);
//            EndRequiredDate = today;
//            break;
//        case '10':
//            $('#RequiredDatetimeperiodcontainer').hide();
//            StartRequiredDate = PreviousDateByDay(60);
//            EndRequiredDate = today;
//            break;
//        case '11':
//            $('#RequiredDatetimeperiodcontainer').hide();
//            StartRequiredDate = PreviousDateByDay(90);
//            EndRequiredDate = today;
//            break;
//        case '12':
//            $('#RequiredDatetimeperiodcontainer').show();
//            StartRequiredDate = today;
//            EndRequiredDate = today;
//            $('#advrequiredDatedaterange').val(StartRequiredDate + ' - ' + EndRequiredDate);
//            $(document).find('#advrequiredDatedaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
//                StartRequiredDate = start.format('MM/DD/YYYY');
//                EndRequiredDate = end.format('MM/DD/YYYY');
//            });
//            break;
//        default:
//            $('#RequiredDatetimeperiodcontainer').hide();
//            $(document).find('#advrequiredDatedaterange').daterangepicker({
//                format: 'MM/DD/YYYY'
//            });
//            StartRequiredDate = '';
//            EndRequiredDate = '';
//            break;
//    }
//});
//$(document).on('change', '#ScheduledDateCalendar', function () {
//    var thisval = $(this).val();
//    CustomQueryDisplayId = thisval;
//    if (thisval == 6) {
//        $('#ScheduledDatetimeperiodcontainercalendar').show();
//    }
//    else {
//        $('#ScheduledDatetimeperiodcontainercalendar').hide();
//    }
//    switch (thisval) {
//        case '1':
//            $('#ScheduledDatetimeperiodcontainercalendar').hide();
//            StartScheduledDate = today;
//            EndScheduledDate = today;
//            break;
//        case '2':
//            $('#ScheduledDatetimeperiodcontainercalendar').hide();
//            StartScheduledDate = PreviousDateByDay(7);
//            EndScheduledDate = today;
//            break;
//        case '3':
//            $('#ScheduledDatetimeperiodcontainercalendar').hide();
//            StartScheduledDate = PreviousDateByDay(30);
//            EndScheduledDate = today;
//            break;
//        case '4':
//            $('#ScheduledDatetimeperiodcontainercalendar').hide();
//            StartScheduledDate = PreviousDateByDay(60);
//            EndScheduledDate = today;
//            break;
//        case '5':
//            $('#ScheduledDatetimeperiodcontainercalendar').hide();
//            StartScheduledDate = PreviousDateByDay(90);
//            EndScheduledDate = today;
//            break;
//        case '6':
//            $('#ScheduledDatetimeperiodcontainercalendar').show();
//            if (ModelOpened === false) {
//                StartScheduledDate = today;
//                EndScheduledDate = today;
//                $('#advscheduledDatedaterangecalendar').val(StartScheduledDate + ' - ' + EndScheduledDate);
//                $(document).find('#advscheduledDatedaterangecalendar').daterangepicker(daterangepickersetting, function (start, end, label) {
//                    StartScheduledDate = start.format('MM/DD/YYYY');
//                    EndScheduledDate = end.format('MM/DD/YYYY');
//                });
//            }
//            else {
//                var dpicker = $('#advscheduledDatedaterangecalendar').val();
//                StartScheduledDate = dpicker.split(' - ')[0];
//                EndScheduledDate = dpicker.split(' - ')[1];
//            }

//            break;
//        default:
//            $('#ScheduledDatetimeperiodcontainercalendar').hide();
//            $(document).find('#advscheduledDatedaterangecalendar').daterangepicker({
//                format: 'MM/DD/YYYY'
//            });
//            StartScheduledDate = '';
//            EndScheduledDate = '';
//            break;
//    }

//    if (thisval != 6) {
//        ModelOpened = false;
//        ChangeListLSHeaderInfo();
//    }
//    else {
//        $(document).find('#DateRangeScheduledModalcalendar').modal('show');
//    }
//});
//$(document).on('click', '#btntimeperiod', function (e) {
//    ModelOpened = true;
//    OldCustomQueryDisplayId = '6';
//    $(document).find('#DateRangeScheduledModalcalendar').modal('hide');
//    ChangeListLSHeaderInfo();
//});
//$("#btnLSDataAdvSrchCalendar").on('click', function (e) {
//    run = true;
//    $(document).find('#txtColumnSearch').val('');
//    $('#sidebar').removeClass('active');
//    $('.overlay').fadeOut();
//    LSAdvSearch();
//    SaveAdvSearchFilterInfo();
//    calendar.refetchEvents();
//});
//$(document).on('click', '.btnCross', function () {
//    run = true;
//    var btnCrossed = $(this).parent().attr('id');
//    var searchtxtId = btnCrossed.split('_')[1];
//    $('#' + searchtxtId).val('').trigger('change');
//    $(this).parent().remove();
//    if (selectCount > 0) selectCount--;
//    LSAdvSearch();
//    SaveAdvSearchFilterInfo();
//    calendar.refetchEvents();
//});

function ChangeListLSHeaderInfo() {
    calendar.refetchEvents();
}
//function LSAdvSearch() {
//    $('#txtColumnSearch').val('');
//    var searchitemhtml = "";
//    selectCount = 0;
//    $('#advsearchsidebarcalendar').find('.adv-item').each(function (index, item) {
//        if ($(this).hasClass('dtpicker')) {
//            $(this).val(ValidateDate($(this).val()));
//        }
//        if ($(this).attr('id') != 'advrequiredDatedaterange') {
//            if ($(this).val()) {
//                selectCount++;
//                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
//            }
//        }
//        else {
//            if ($(this).val()) {
//                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
//            }
//        }

//    });
//    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
//    $("#advsearchfilteritems").html(searchitemhtml);
//    $('#_advrequiredDatedaterange').hide();
//    $(".filteritemcount").text(selectCount);
//    // $('#liSelectCount').text(selectCount + ' filters applied');
//}
//function clearAdvanceSearch() {
//    selectCount = 0;
//    $("#EquipmentId").val("");
//    $('#Name').val("");
//    $("#Description").val("");
//    $("#RequiredDate").val("").trigger('change');
//    StartRequiredDate = '';
//    EndRequiredDate = '';
//    $("#advsearchfilteritems").html('');
//    $(".filteritemcount").text(selectCount);
//}
//#endregion

//#region Drop down start date and personnel list

//var SelectTimePeriod = '';
//$('#ScheduledDateCalendar').on('select2:open', function (e) {

//    SelectTimePeriod = $(this).val();
//});

//$('#ScheduledDateCalendar').on('select2:close', function (e) {

//    if (SelectTimePeriod == $(this).val()) {
//        $('#ScheduledDateCalendar').val($(this).val()).trigger("change");
//    }
//});

//$(".canceldtpicker").on('click', function (e) {
//    $('#ScheduledDateCalendar').val(OldCustomQueryDisplayId).trigger("change.select2");
//});
//$(document).on('change', '#ScheduledDateCalendar', function () {
//    var thisval = $(this).val();
//    if (CustomQueryDisplayId != '6') {
//        OldCustomQueryDisplayId = CustomQueryDisplayId;
//    }

//    CustomQueryDisplayId = thisval;
//    if (thisval == 6) {
//        $('#ScheduledDatetimeperiodcontainer').show();
//    }
//    else {
//        $('#ScheduledDatetimeperiodcontainer').hide();
//    }
//    switch (thisval) {
//        case '1':
//            $('#ScheduledDatetimeperiodcontainer').hide();
//            StartScheduledDate = today;
//            EndScheduledDate = today;
//            break;
//        case '2':
//            $('#ScheduledDatetimeperiodcontainer').hide();
//            StartScheduledDate = PreviousDateByDay(7);
//            EndScheduledDate = today;
//            break;
//        case '3':
//            $('#ScheduledDatetimeperiodcontainer').hide();
//            StartScheduledDate = PreviousDateByDay(30);
//            EndScheduledDate = today;
//            break;
//        case '4':
//            $('#ScheduledDatetimeperiodcontainer').hide();
//            StartScheduledDate = PreviousDateByDay(60);
//            EndScheduledDate = today;
//            break;
//        case '5':
//            $('#ScheduledDatetimeperiodcontainer').hide();
//            StartScheduledDate = PreviousDateByDay(90);
//            EndScheduledDate = today;
//            break;
//        case '6':
//            $('#ScheduledDatetimeperiodcontainer').show();
//            if (ModelOpened === false) {
//                StartScheduledDate = today;
//                EndScheduledDate = today;
//                $('#advscheduledDatedaterangecalendar').val(StartScheduledDate + ' - ' + EndScheduledDate);
//                $(document).find('#advscheduledDatedaterangecalendar').daterangepicker(daterangepickersetting, function (start, end, label) {
//                    StartScheduledDate = start.format('MM/DD/YYYY');
//                    EndScheduledDate = end.format('MM/DD/YYYY');
//                });
//            }
//            else {
//                var dpicker = $('#advscheduledDatedaterangecalendar').val();
//                StartScheduledDate = dpicker.split(' - ')[0];
//                EndScheduledDate = dpicker.split(' - ')[1];
//            }
//            break;
//        default:
//            $('#ScheduledDatetimeperiodcontainer').hide();
//            $(document).find('#advscheduledDatedaterangecalendar').daterangepicker({
//                format: 'MM/DD/YYYY'
//            });
//            StartScheduledDate = '';
//            EndScheduledDate = '';
//            break;
//    }

//    if (thisval != 6) {
//        ChangeListLSHeaderInfo();
//    }
//    else {
//        //$(document).find('#DateRangeScheduledModal').modal('show');
//        $(document).find('#DateRangeScheduledModal').modal({ backdrop: 'static', keyboard: false, show: true });
//    }
//});
$(document).on('change', '#ddlUser', function () {
    PersonnelList = $(this).val();
    //if (PersonnelList != "") {
    //    $('#ddlUser option[value=""]').remove();
    //}
    //else {
    //    $('#ddlUser option[value=""]').remove();
    //    $('#ddlUser')
    //        .prepend('<option value="" selected="selected">All Personnel</option>');
    //}

    calendar.refetchEvents();
});
//#endregion

//#region Add schedule calendar
function AddScheduleCalendar(dateStr) {
    var NeedToExecComplete = true;
    $.ajax({
        url: "/LaborScheduling/AddScheduleCalendar",
        type: "GET",
        //dataType: 'JSON',
        //data: {
        //    'ScheduleDate': ISODateToDateformat(dateStr)
        //},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data["WorkOrderListCount"] !== undefined && data["WorkOrderListCount"] !== null && data["WorkOrderListCount"] == 0) {
                NeedToExecComplete = false;
                CloseLoader();
                var ShowAlertSetting = {
                    title: '',
                    text: getResourceValue("CancelAlertLostMsg"),
                    type: "warning",
                    showCancelButton: false,
                    confirmButtonClass: "btn-sm btn-primary",
                    cancelButtonClass: "btn-sm",
                    confirmButtonText: getResourceValue("CancelAlertOk")
                };
                ShowAlertSetting.text = getResourceValue('spnNoApprovedWorkOrder');
                swal(ShowAlertSetting, function () { });
            }
            else {                
                $('#AddScheduleCalendarPopUp').html(data);
                $('#AddScheduleCalendar').modal({ backdrop: 'static', keyboard: false, show: true });                
            }            
        },
        complete: function () {
            if (NeedToExecComplete === true) {
                SetControls();
                $('#AddSchedlingCalendarModal_ScheduleDate').val(ISODateToDateformat(dateStr));
            }            
        },
        error: function () {
            CloseLoader();
        }
    });
}
function AddScheduleCalendarOnSuccess(data) {
    if (data.data === "success") {
        SuccessAlertSetting.text = getResourceValue("ScheduledWOAddAlert");
        $("#AddScheduleCalendar").modal('hide');
        swal(SuccessAlertSetting, function () {
            calendar.refetchEvents();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
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
    SetFixedHeadStyle();
}
$(document).on('click', '.closeAddcheduleModal', function () {
    var areaChargeToId = "";
    $(document).find('#AddScheduleCalendarPopUp select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });
});
//#endregion

//#region Edit and remove schedule calendar
$(document).on('hide.bs.modal', '#EditScheduleCalendar', function (event) {
    $('#EditScheduleCalendarPopUp').empty();
});
function EditScheduleCalendar(WorkOrderSchedId, Workorderid, PersonnelId) {
    $.ajax({
        url: "/LaborScheduling/EditScheduleCalendar",
        type: "GET",
        dataType: 'html',
        data: {
            'WorkOrderSchedId': WorkOrderSchedId,
            'Workorderid': Workorderid
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#EditScheduleCalendarPopUp').html(data);
            $('#EditScheduleCalendar').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            var color = PersonnelColor.filter(function (obj) { return obj.personnelId === PersonnelId })[0].bgcolor;
            $('#PersonnelColor').css("color", color);
            SetControls();
            addControls();            
        },
        error: function () {
            CloseLoader();
        }
    });
}
function EditScheduleCalendarOnSuccess(data) {
    if (data.data === "success") {
        //SuccessAlertSetting.text = getResourceValue("ScheduledWOAddAlert");
        SuccessAlertSetting.text = getResourceValue("ScheduledWorkOrderUpdateSuccessAlert");
        $("#EditScheduleCalendar").modal('hide');
        swal(SuccessAlertSetting, function () {
            calendar.refetchEvents();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on("click", ".RemoveSchedule", function () {
    var WOId = $(this).data('workorderid');
    var WorkOrderScheduledID = $(this).data('workorderscheduledid');
    CancelAlertSettingForCallback.text = getResourceValue("ConfirmRemoveScheduleAlert");
    swal(CancelAlertSettingForCallback, function () {
        $.ajax({
            url: "/LaborScheduling/RemoveScheduleCalendar",
            type: "POST",
            dataType: 'JSON',
            data: {
                'WOId': WOId,
                'WOSchedId': WorkOrderScheduledID
            },
            async: false,
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data === "success") {
                    $("#EditScheduleCalendar").modal('hide');
                    SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyRemovedScheduled");
                    swal(SuccessAlertSetting, function () {
                        calendar.refetchEvents();
                    });
                }
            },
            complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    });
});
//$(document).on('click', '.closeEditcheduleModal', function () {
//    debugger;
//    var areaChargeToId = "";
//    $(document).find('#EditScheduleCalendarPopUp select').each(function (i, item) {
//        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
//        $('#' + areaChargeToId).hide();
//    });
//});
//#endregion

//#region drag and drop schedule calendar
function DragScheduleCalendar(dateStr, WOId, WorkOrderScheduledID) {
    var returnStatus = "";
    $.ajax({
        url: "/LaborScheduling/DragScheduleCalendar",
        type: "POST",
        dataType: 'json',
        data: {
            'ScheduleDate': ISODateToDateformat(dateStr),
            'WOId': WOId,
            'WorkOrderScheduledID': WorkOrderScheduledID
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.data === "success") {
                returnStatus = "success";
                calendar.refetchEvents();
            }
            else {
                returnStatus = "failed";
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            returnStatus = "failed";
            CloseLoader();
        }
    });
    return returnStatus;
}
//#endregion

//#region Available Work
$(document).on('click', '#btnAvailableWork', function () {
    //$(document).find('#AvailableWorkModal').modal('show');
    LaborAvailableSelectedItemArray = [];
    AWOGridTotalGridItem = [];
    WorkOrderIds = [];
    $.ajax({
        url: "/LaborScheduling/AvailableWorkOrdersCalendar",
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AvailableWork').html(data);
            // $(document).find('#AvailableWorkModal').modal('show');
            $(document).find('#AvailableWorkModal').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetAdvSearchControl();
            GenerateAvailableLabor();
            CloseLoader();
            $("#ApproveWOadvsearchcontainer .sidebar").mCustomScrollbar({
                theme: "minimal"
            });
        },
        error: function () {
            CloseLoader();
        }
    });
});

//#region V2-984
function SetAdvSearchControl() {
    $("#sidebar2").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismissAW, .overlay2').on('click', function () {
        $('#sidebar2').removeClass('active');
        $('.overlay2').fadeOut();
    });
    $(document).find('#AvailablesidebarCollapse').on('click', function () {
        $('#sidebar2').addClass('active');
        $('.overlay2').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    $(document).find('.select2picker').select2({});
}
//#endregion
function GenerateAvailableLabor(flag) {
    if ($(document).find('#tblAvailGrid').hasClass('dataTable')) {
        LaborAvailabledt.destroy();
    }
    LaborAvailabledt = $(document).find("#tblAvailGrid").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[1, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/LaborScheduling/GetAvailableWorkOrderCalendarMainGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.ClientLookupId = LRTrim($("#AWOWorkOrder").val());
                d.ChargeTo = LRTrim($("#AWOChargeTo").val());
                d.ChargeToName = LRTrim($("#AWOChargeToName").val());
                d.Description = LRTrim($("#AWODescription").val());
                d.Status = $("#AWOStatus").val();
                d.Priority = $('#AWOPriority').val();
                //d.DownRequired = LRTrim($('#AWODownRequired').val());
                d.Assigned = $('#AWOAssigned').val();
                d.Type = $("#AWOWOType").val();
                //d.StartDate = LRTrim($("#AWOStartDate").val());
                //d.Duration = LRTrim($("#AWODuration").val());
                d.RequiredDate = LRTrim($('#AWORequiredDate').val());
                d.flag = flag;
            },
            "dataSrc": function (result) {
                AWOGridTotalGridItem = result.data.length;
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "WorkOrderId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    targets: 0,
                    'render': function (data, type, full, meta) {
                        if ($('#labavlidselectall').is(':checked')) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-LSid="' + data + '" class="chksearchAvl ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        } else {
                            var found = LaborAvailableSelectedItemArray.some(function (el) {
                                return el.WorkOrderId === data;
                            });
                            if (found) {
                                return '<input type="checkbox" checked="checked" name="id[]" data-LSid="' + data + '" class="chksearchAvl ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                            else {
                                return '<input type="checkbox" name="id[]" data-LSid="' + data + '" class="chksearchAvl ' + data + '"  value="'
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
                    //V2-838
                    "className": "text-left",
                    "mRender": function (data, type, full, row) {
                        if (full['PartsOnOrder'] > 0) {
                            return '<div  class="width-100"><a class="lnk_workorder" style="text-decoration: none; cursor:default;color:black" href="javascript:void(0)">' + data + '<span style="margin-left:8px"  class="m-badge m-badge--purple">' + full['PartsOnOrder'] + '</a></div>';
                        } else {
                            return '<div  class="width-100"><a class="lnk_workorder" style="text-decoration: none; cursor:default;color:black" href="javascript:void(0)">' + data + '</a></div>'
                        }
                    }
                },
                { "data": "ChargeTo", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ChargeToName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    render: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "Assigned", "autoWidth": true, "bSearchable": true, "bSortable": true, "sClass": "ghover",
                    mRender: function (data, type, full, meta) {
                        if (full.WorkAssigned_PersonnelId == -1) {
                            return "<span>" + data + "</span><span class='tooltipgrid' id=" + full.WorkOrderId + "></span><span class='loadingImg' style='display:none !important;'><img src='/Images/lineLoader.gif' style='width:55px;height:auto;position:absolute;left:6px;top:30px;'></span>";

                        }
                        else {
                            return "<span>" + data + "</span>";
                        }
                    }
                },
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Priority", "autoWidth": true, "bSearchable": true, "bSortable": true },
                //{ "data": "DownRequired", "autoWidth": true, "bSearchable": true, "bSortable": true },
                //{ "data": "Assigned", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
                //{ "data": "StartDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                //{ "data": "Duration", "autoWidth": true, "bSearchable": true, "bSortable": true, className: 'text-right' },
                { "data": "RequiredDate", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        },
        'rowCallback': function (row, data, index) {
            if (data.Status == "Scheduled") {
                $(row).find('td').css('background-color', '#36a3f7b3');
                $(row).find('td').css('color', '#fff');
            }
        }
    });
}

$(document).on('click', '#AlreadyScheduledId', function (e) {
    LaborAvailabledt.state.clear();
    if (!this.checked) {
        GenerateAvailableLabor(0);
    }
    else {
        GenerateAvailableLabor(1);
    }
});
$(document).on('click', '#labavlidselectall', function (e) {
    var IsChecked = $(document).find('#AlreadyScheduledId').is(":checked");
    if (IsChecked) {
        flag = 1;
    }
    else {
        flag = 0;
    }
    WorkOrderIds = [];
    var checked = this.checked;
    $.ajax({
        url: "/LaborScheduling/GetLaborAvailableCalendar",
        async: true,
        type: "GET",
        datatype: "json",
        data:  {
            flag:flag
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    var found = LaborAvailableSelectedItemArray.some(function (el) {
                        return el.WorkOrderId === item.WorkOrderId;
                    });
                    if (checked) {
                        if (WorkOrderIds.indexOf(item.WorkOrderId) == -1)
                            WorkOrderIds.push(item.WorkOrderId);

                        var itemLS = new LaborAvailableSelectedItem(item.WorkOrderId, item.ClientLookupId, item.ChargeTo, item.ChargeToName, item.Description, item.Status, item.Priority,
                            item.DownRequired, item.Assigned, item.Type, item.StartDate, item.Duration, item.RequiredDate);
                        if (!found) { LaborAvailableSelectedItemArray.push(itemLS); }
                    } else {
                        var i = WorkOrderIds.indexOf(item.WorkOrderId);
                        WorkOrderIds.splice(i, 1);
                        if (found) {
                            LaborAvailableSelectedItemArray = LaborAvailableSelectedItemArray.filter(function (el) {
                                return el.WorkOrderId !== item.WorkOrderId;
                            });
                        }
                    }
                });
            }
        },
        complete: function () {
            LaborAvailabledt.column(0).nodes().to$().each(function (index, item) {
                if (checked) {
                    $(this).find('.chksearchAvl').prop('checked', 'checked');
                } else {
                    $(this).find('.chksearchAvl').prop('checked', false);
                }
            });
            CloseLoader();
        }
    });
});
$(document).on('change', '.chksearchAvl', function () {
    var data = LaborAvailabledt.row($(this).parents('tr')).data();
    if (!this.checked) {
        var index = WorkOrderIds.indexOf(data.WorkOrderId);
        WorkOrderIds.splice(index, 1);
        var el = $('#labavlidselectall').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        LaborAvailableSelectedItemArray = LaborAvailableSelectedItemArray.filter(function (el) {
            return el.WorkOrderId !== data.WorkOrderId;
        });
    }
    else {
        WorkOrderIds.push(data.WorkOrderId);
        var item = new LaborAvailableSelectedItem(data.WorkOrderId, data.ClientLookupId, data.ChargeTo, data.ChargeToName, data.Description, data.Status, data.Priority,
            data.DownRequired, data.Assigned, data.Type, data.StartDate, data.Duration, data.RequiredDate);
        var found = LaborAvailableSelectedItemArray.some(function (el) {
            return el.WorkOrderId === data.WorkOrderId;
        });
        if (!found) { LaborAvailableSelectedItemArray.push(item); }
    }
    if (AWOGridTotalGridItem == LaborAvailableSelectedItemArray.length) {
        $('#labavlidselectall').prop('checked', 'checked');
    }
    else {
        $('#labavlidselectall').prop('checked', false);
    }
});
function LaborAvailableSelectedItem(WorkOrderId, ClientLookupId, ChargeTo, ChargeToName, Description, Status, Priority, Type, RequiredDate) {
    this.WorkOrderId = WorkOrderId;
    this.ClientLookupId = ClientLookupId;
    this.ChargeTo = ChargeTo;
    this.ChargeToName = ChargeToName;
    this.Description = Description;
    this.Status = Status;
    this.Priority = Priority;
    this.Type = Type;
    this.RequiredDate = RequiredDate;
}
$(document).on('click', '#btnLSAddAvailableWO', function (e) {
    var WorkOrderVals = WorkOrderIds;
    if (WorkOrderVals.length <= 0) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        for (var i = 0; i < LaborAvailableSelectedItemArray.length; i++) {
            SelectedWoIdToAssigned.push(LaborAvailableSelectedItemArray[i].WorkOrderId);
            SelectedLookupIdToAssigned.push(LaborAvailableSelectedItemArray[i].ClientLookupId);
            SelectedStatusAssigned.push(LaborAvailableSelectedItemArray[i].Status);
        }
        //$(document).find('#Schedulestartdate').val("").trigger('change');
        //$('#ddlSchUser').val("").trigger("change.select2");
        //$(document).find('#woRescheduleModel_WorkOrderIds').val(SelectedWoIdToAssigned);
        //$(document).find('#woRescheduleModel_ClientLookupIds').val(SelectedLookupIdToAssigned);
        //$(document).find('#woRescheduleModel_Status').val(SelectedStatusAssigned);
        //$(document).find('.select2picker').select2({});
        //$(document).find('#ScheduleModal').modal('show');

        $(document).find('#Assignstartdate').val("").trigger('change');
        $('#ddlAssUser').val("").trigger("change.select2");
        $('#availableWorkAssignCalendarModel_ScheduledDuration').val("0");
        $(document).find('#availableWorkAssignCalendarModel_WorkOrderIds').val(SelectedWoIdToAssigned);
        $(document).find('#availableWorkAssignCalendarModel_ClientLookupIds').val(SelectedLookupIdToAssigned);
        $(document).find('#availableWorkAssignCalendarModel_Status').val(SelectedStatusAssigned);
        $(document).find('.select2picker').select2({});
        // $(document).find('#AvailableWorkAssignModal').modal('show');
        $(document).find('#AvailableWorkAssignModal').modal({ backdrop: 'static', keyboard: false, show: true });
        //SetNewLaborScheduleControls();
        SetControls();
        addControls();
        $(document).find('form').find("#Assignstartdate").removeClass("input-validation-error");
        $(document).find('form').find("#ddlAssUser").removeClass("input-validation-error");
        $(document).find('form').find("#availableWorkAssignCalendarModel_ScheduledDuration").removeClass("input-validation-error");
    }
});

function addControls() {
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true,
        minDate: new Date()
    }).inputmask('mm/dd/yyyy');
}
function AvailableWorkAddOnSuccess(data) {
    //$('#ScheduleModal').modal('hide');
    if (data.data === "success") {
        SuccessAlertSetting.text = getResourceValue("AvailableWorkAssignSuccessAlert");
        swal(SuccessAlertSetting, function () {
            CloseLoader();
            LaborAvailabledt.page('first').draw('page');
            $(document).find('#Assignstartdate').val("").trigger('change');
            $('#ddlAssUser').val("").trigger("change.select2");
            calendar.refetchEvents();
        });
    }
    else {
        CloseLoader();
        GenericSweetAlertMethod(data.data);
    }
    //RemoveUpdateAreaInfo();
    //$(document).find('.chksearch').prop('checked', false);
    //$(document).find('.itemcount').text(0);
    LaborAvailableSelectedItemArray = [];
    AWOGridTotalGridItem = [];
    SelectedLookupIdToAssigned = [];
    SelectedStatusAssigned = [];
    SelectedWoIdToAssigned = [];
    WorkOrderIds = [];
    if ($('#labavlidselectall').is(':checked')) {
        $('#labavlidselectall').prop('checked', false);
    }
    $('.chksearchAvl').prop('checked', false);
    $(document).find('#Assignstartdate').val("").trigger('change');
    $('#ddlAssUser').val(null).trigger("change.select2");
    $('#availableWorkAssignCalendarModel_ScheduledDuration').val("0");
    $(document).find('#AvailableWorkAssignModal').modal('hide');
    //$(document).find('#AvailableWorkModal').modal('hide');

}
$(document).on('click', '.btnAsscancelmod', function () {
    var areaddescribedby = $(document).find("#ddlAssUser").attr('aria-describedby');
    if (typeof areaddescribedby !== 'undefined') {
        $('#' + areaddescribedby).hide();
    }
    $(document).find('form').find("#ddlAssUser").removeClass("input-validation-error");
    $(document).find('form').find("#Assignstartdate").removeClass("input-validation-error");
    $(document).find('form').find("#availableWorkAssignCalendarModel_ScheduledDuration").removeClass("input-validation-error");
});

$(document).on('click', "#btnAvailableLaborDataAdvSrch", function (e) {
    var searchitemhtml = "";
    hGridfilteritemcount = 0;
    $('#txtLAsearchbox').val('');
    $('#advsearchsidebarAvailableLabor').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        //V2-984
        //if ($(this).val()) {
        //    hGridfilteritemcount++;
        //    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossHistory" aria-hidden="true"></a></span>';
        //}
        if ([].concat($(this).val()).filter(function (valueOfFilter) { return valueOfFilter != ''; }).length) {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossHistory" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritemsAWO').html(searchitemhtml);
    $('#ApproveWOadvsearchcontainer').find('.sidebar').removeClass('active');
    $(document).find('.overlay2').fadeOut();
    GridAdvanceSearch();
});
function GridAdvanceSearch() {
    var IsChecked = $(document).find('#AlreadyScheduledId').is(":checked");
    if (IsChecked) {
        flag = 1;
    }
    else {
        flag = 0;
    }
    LaborAvailabledt.page('first').draw('page');
    $('.AWOfilteritemcount').text(hGridfilteritemcount);
}

$(document).on('click', '#liClearAdvSearchFilterAVlLabAWO', function () {
    var IsChecked = $(document).find('#AlreadyScheduledId').is(":checked");
    if (IsChecked) {
        flag = 1;
    }
    else {
        flag = 0;
    }
    clearAdvanceSearchAWO();
    LaborAvailabledt.page('first').draw('page');
});
function clearAdvanceSearchAWO() {
    var filteritemcount = 0;
    $('#advsearchsidebarAvailableLabor').find('input:text').val('');
    $('.AWOfilteritemcount').text(filteritemcount);
    $('#advsearchfilteritemsAWO').find('span').html('');
    $('#advsearchfilteritemsAWO').find('span').removeClass('tagTo');


}
$(document).on('click', '.btnCrossHistory', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    GridAdvanceSearch();
});
//$(document).on('click', '.btnLSAWOCancel', function () {
//    $('.overlay2').fadeOut();
//});
$(document).on('hide.bs.modal', '#AvailableWorkModal', function (event) {
    $('#AvailableWorkModal').empty();
    $(document).find('#AvailableWork').html('');
});
//$('#AvailablesidebarCollapse').on('click', function () {
$(document).on('click', '#AvailablesidebarCollapse', function () {
    $('#ApproveWOadvsearchcontainer .sidebar').addClass('active');
    $('.overlay2').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    //addControls();
    
});
//$("#ApproveWOadvsearchcontainer .sidebar").mCustomScrollbar({
//    theme: "minimal"
//});
$(document).on('click', '#dismissAW', function () {
    $(document).find('#ApproveWOadvsearchcontainer .sidebar').removeClass('active');
    $(document).find('.overlay2').fadeOut();

});
//#endregion

