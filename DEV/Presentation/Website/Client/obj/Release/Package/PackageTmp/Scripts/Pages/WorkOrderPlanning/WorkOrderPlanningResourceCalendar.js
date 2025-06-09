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
        if (Totalchecked == Select2MultiCheckBoxObj.length - 1 && b == false && $('.checkboxAssignedUser:checked').length < 1) {
            IsCheckedAllOption(true);
            $("#" + staticWordInID + "0").prop("checked", true);
            Totalchecked = Select2MultiCheckBoxObj.length - 1;

        } else {
           
            IsCheckedAllOption(b);
            if (b == true) {
                Totalchecked = $('.checkboxAssignedUser:checked').length - 1;
            } else {
                Totalchecked = $('.checkboxAssignedUser:checked').length;
            }
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
        var d = new Date(strDate);
        return $.datepicker.formatDate('mm/dd/yy', d);
    } catch (e) {
        return null;
    }
}
function getRandomColor(PersonnelId, count, total) {   
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
    return rgb;
    //--------------end    
}

function CalendarEvent(WorkOrderId, WorkOrderScheduleId, Title, Description, PersonnelId, PersonnelFull, StartDate, Hours, bgcolor) {
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
        url: '/WorkOrderPlanning/GetResourceCalendarData',
        type: 'POST',
        dataType: 'json',
        data: {
            StartDt: ISODateToDateformat(startStr),
            EndDt: ISODateToDateformat(endStr),
            "PersonnelList": PersonnelList,
            WorkOrderPlanId: $('#workorderPlanningModel_WorkOrderPlanId').val()
        },
        beforeSend: function () {
        },
        //contentType: 'json',
        success: function (data) {
            let CalendarEventData = [];
            if (data.CalendarList && data.CalendarList.length > 0) {
                PersonnelColor = [];
                if (data.ListPersonnel && data.ListPersonnel.length > 0) {
                    GetPersonnelColorFromStorage(data.ListPersonnel.map(function (x) { return x.PersonnelId }));
                    BindPersonnelList(data.ListPersonnel);
                    SetPersonnelColorToStorage(JSON.stringify(PersonnelColor));
                }
                $(document).find("#PersonnelList > tbody").css("visibility", "visible");
                $.each(data.CalendarList, function (i, d) {
                    var bgcolor = PersonnelColor.filter(function (color) { return color.personnelId === d.PersonnelId; })[0].bgcolor;
                    var obj = new CalendarEvent(d.WorkOrderId, d.WorkOrderScheduleId, d.Title, d.Description, d.PersonnelId, d.PersonnelFull,
                        d.ScheduledStartDate, d.ScheduledHours, bgcolor);
                    CalendarEventData.push(obj);
                });
                successCallback(CalendarEventData);
            }
            else {
                successCallback(CalendarEventData);
                $('#PersonnelList > tbody')
                    .css("visibility", "visible")
                    .html('<tr><td colspan="3" style="text-align: center">' + getResourceValue("spnEmptyTable") + '<td></tr>');
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
function generateResourceCalendar() {
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
                title: info.event.title,
                placement: 'top',
                trigger: 'hover',
                container: 'body'
            });
        },
        eventDrop: function (info) {// drag and drop to an element     
            if (isFunctionAllowed()) {
                var dtValue = ISODateToDateformat(info.event.start.toISOString());
                if (isScheduledDateBetweenStartAndEndDate(dtValue)) {
                    let returnStatus = DragScheduleCalendar(info.event.start.toISOString(), info.event.extendedProps.WorkOrderId, info.event.extendedProps.WorkOrderScheduleId);
                    if (returnStatus !== "success") {
                        info.revert();
                    }
                }
                else {
                    ErrorAlertSetting.text = getResourceValue("ValidateScheduleDateOnlyBetweenSelectedWOPStartAndEndDate");
                    swal(ErrorAlertSetting, function () { });
                    info.revert();
                }
            }
            else {
                info.revert();
            }
        },
        selectable: false,
        dateClick: function (info) {//click on an empty area of a date       
            if (isFunctionAllowed()) {
                var dtValue = ISODateToDateformat(info.dateStr);
                if (isScheduledDateBetweenStartAndEndDate(dtValue)) {
                    AddScheduleCalendar(info.dateStr);
                }
                else {
                    ErrorAlertSetting.text = getResourceValue("ValidateScheduleDateOnlyBetweenSelectedWOPStartAndEndDate");
                    swal(ErrorAlertSetting, function () { });
                }
            }
        },
        eventClick: function (info) { //clicking on a data element      
            if (isFunctionAllowed()) {
                EditScheduleCalendar(info.event.extendedProps.WorkOrderScheduleId, info.event.extendedProps.WorkOrderId, info.event.extendedProps.PersonnelId);
            }
        }
    });
    calendar.render();
}
function GetPersonnelColorFromStorage(PersonnelIdList) {
    var colArraystring = localStorage.getItem("WorkOrderPlanningColArray");
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
    localStorage.removeItem("WorkOrderPlanningColArray");
    if (colorArray && colorArray.length > 0) {
        localStorage.setItem("WorkOrderPlanningColArray", colorArray);
    }
}
function isFunctionAllowed() {
    var Locked = LockPlan,
        Status = PlanStatus;

    if (Status === 'Locked') {
        if (Locked === 'True') {
            return true;
        }
    }
    return false;
}
function isScheduledDateBetweenStartAndEndDate(SelectedDate) {
    var dtValue = new Date(moment(SelectedDate, 'MM/DD/YYYY'));
    var StartDate = new Date(moment(PlanStartDate, 'MM/DD/YYYY'));
    var EndDate = new Date(moment(PlanEndDate, 'MM/DD/YYYY'));

    if (dtValue < StartDate || dtValue > EndDate) {
        return false;
    }
    return true;
}
function LoadCalendarTab() {
    $.ajax({
        url: '/WorkOrderPlanning/ResourceCalendar',
        type: 'POST',
        dataType: 'html',
        data: {

        },
        beforeSend: function () {
        },
        //contentType: 'html',
        success: function (data) {
            if (data) {
                $(document).find('#ResourceCalenderDiv').html(data);
            }
        },
        complete: function () {
            Totalchecked = 0;     
            generateResourceCalendar();
            $(document).find(".assignedBlock").css("display", "inline-block");
            $(document).find(".scheduledBlock").css("display", "inline-block");
            $(".actionBar").fadeIn();
            $("#lsGridAction :input").attr("disabled", "disabled");
            $(document).find('.select2picker').select2({});
            GetUserAssignedDropdown();//V2-562 
            PersonnelList = [];                
            if (isFunctionAllowed()) {
                $(document).find("#btnAvailableWorkRL").css("display", "inline-block");
            }
            else {
                $(document).find("#btnAvailableWorkRL").css("display", "none");
            }
        },
        error: function (err) {
            CloseLoader();
        }
    });
}
//#endregion

//#region Drop down start date and personnel list
$(document).on('change', '#ddlUser', function () {
    PersonnelList = $(this).val();
    calendar.refetchEvents();
});
//#endregion

//#region Add schedule calendar
function AddScheduleCalendar(dateStr) {
    $.ajax({
        url: "/WorkOrderPlanning/AddScheduleCalendarModal",
        type: "POST",
        beforeSend: function () {
            ShowLoader();
        },
        data: {
            WorkOrderPlanId: $('#workorderPlanningModel_WorkOrderPlanId').val()
        },
        success: function (data) {
            $('#AddScheduleResourceCalendarPopUp').html(data);
            $('#AddScheduleResourceCalendar').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
            $('#ResourceCalendarAddScheduleModel_Schedulestartdate').val(ISODateToDateformat(dateStr));
            $('#WorkOrderPlanStartDate').val(PlanStartDate);
            $('#WorkOrderPlanEndDate').val(PlanEndDate);
        },
        error: function () {
            CloseLoader();
        }
    });
}
function AddScheduleCalendarOnSuccess(data) {
    if (data.data === "success") {
        SuccessAlertSetting.text = getResourceValue("ScheduledWOAddAlert");
        $("#AddScheduleResourceCalendar").modal('hide');
        swal(SuccessAlertSetting, function () {
            calendar.refetchEvents();
            $('#AddScheduleResourceCalendarPopUp').html('');
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
    $('#AddScheduleResourceCalendarPopUp').html('');
});
//#endregion

//#region Edit and remove schedule calendar
$(document).on('hide.bs.modal', '#EditScheduleCalendar', function (event) {
    $('#EditScheduleResourceCalendarPopUp').empty();
});
function EditScheduleCalendar(WorkOrderSchedId, Workorderid, PersonnelId) {
    $.ajax({
        url: "/WorkOrderPlanning/EditScheduleCalendar",
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
            $('#EditScheduleResourceCalendarPopUp').html(data);
            $('#EditScheduleResourceCalendar').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            var color = PersonnelColor.filter(function (obj) { return obj.personnelId === PersonnelId })[0].bgcolor;
            $('#PersonnelColor').css("color", color);
            SetControls();
            $(document).find('.dtpicker').datepicker({
                changeMonth: true,
                changeYear: true,
                beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
                "dateFormat": "mm/dd/yy",
                autoclose: true,
                // minDate: new Date()
            }).inputmask('mm/dd/yyyy');
            $('#WorkOrderPlanStartDate').val(PlanStartDate);
            $('#WorkOrderPlanEndDate').val(PlanEndDate);
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
        $("#EditScheduleResourceCalendar").modal('hide');
        swal(SuccessAlertSetting, function () {
            calendar.refetchEvents();
            $('#EditScheduleResourceCalendarPopUp').html('');
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
            url: "/WorkOrderPlanning/RemoveScheduleCalendar",
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
                    $("#EditScheduleResourceCalendar").modal('hide');
                    SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyRemovedScheduled");
                    swal(SuccessAlertSetting, function () {
                        calendar.refetchEvents();
                        $('#EditScheduleResourceCalendarPopUp').html('');
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
$(document).on('click', '.closeEditcheduleModal', function () {
    var areaChargeToId = "";
    $(document).find('#EditScheduleCalendarPopUp select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });
    $('#EditScheduleResourceCalendarPopUp').html('');
});
//#endregion

//#region drag and drop schedule calendar
function DragScheduleCalendar(dateStr, WOId, WorkOrderScheduledID) {
    var returnStatus = "";
    $.ajax({
        url: "/WorkOrderPlanning/DragScheduleCalendar",
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