var dp;
var attachmentEventArray = [];
var attachmentEventId = '';


function GenerateGanttChart() {
    var mainGridConfig;
    if ($(window).width() > 375) {
        mainGridConfig = {
            columns: [
                { name: "id", label: "Task ID", align: "left", width: 70 },
                { name: "text", label: "Name", align: "left", width: 120 },
                { name: "start_date", align: "left", label: "Start", width: 75 },
                { name: "end_date_grid", align: "left", label: "End", width: 75 },
                {
                    name: "progress", align: "right", label: "Progress", width: 60, template: function (data) {
                        return RoundUpto2DecimalPlaces(data.progress);
                    }
                }
            ]
        }

        gantt.config.layout = {
            css: "ganttchart",
            rows: [
                {
                    cols: [
                        { view: "grid", id: "grid", config: mainGridConfig, scrollX: "scrollHor", scrollY: "scrollVer" },
                        { view: "timeline", id: "timeline", scrollX: "scrollHor", scrollY: "scrollVer" },
                        { view: "scrollbar", id: "scrollVer" },
                    ]
                },
                { view: "scrollbar", id: "scrollHor" }
            ]
        };

        gantt.config.grid_width = 400; // default value is 360
    }
    else {
        mainGridConfig = {
            columns: [
                { name: "id", label: "Task ID", align: "left", width: 70 },
                { name: "text", label: "Name", align: "left", width: 120 },
                { name: "start_date", align: "left", label: "Start", width: 75 },
                { name: "end_date_grid", align: "left", label: "End", width: 75 },
                {
                    name: "progress", align: "right", label: "Progress", width: 60, template: function (data) {
                        return RoundUpto2DecimalPlaces(data.progress);
                    }
                }
            ]
        }

        gantt.config.layout = {
            css: "gantt_container",
            cols: [
                {
                    width: 150,
                    //min_width: 150,
                    rows: [
                        { view: "grid", id: "grid", config: mainGridConfig, scrollX: "gridScroll", scrollable: true, scrollY: "scrollVer" },
                        { view: "scrollbar", id: "gridScroll", group: "horizontal" }
                    ]
                },
                //{ resizer: true, width: 1 },
                {
                    rows: [
                        { view: "timeline", id: "timeline", scrollX: "scrollHor", scrollY: "scrollVer" },
                        { view: "scrollbar", id: "scrollHor", group: "horizontal" }
                    ]
                },
                { view: "scrollbar", id: "scrollVer" }
            ]
        };
    }
    if ($(document).find('#securityProjectEdit').val() == 'False')
    {
        gantt.config.drag_move = false;
        gantt.config.drag_resize = false;
        gantt.config.drag_progress = false;
    }
    attachmentEventId = gantt.attachEvent("onGanttReady", function () {
        if ($(document).find('#securityProjectEdit').val() == 'True') {
            gantt.config.buttons_left = ["gantt_save_btn", "gantt_cancel_btn"];
        }
        else {
            gantt.config.buttons_left = ["gantt_cancel_btn"];
        }

        gantt.config.buttons_right = [];
    });
    attachmentEventArray.push(attachmentEventId);

    attachmentEventId = gantt.attachEvent("onLoadStart", function () {
        ShowLoaderOnChart();
    });
    attachmentEventArray.push(attachmentEventId);

    attachmentEventId = gantt.attachEvent("onLoadEnd", function () {
        HideLoaderOnChart();
    });
    attachmentEventArray.push(attachmentEventId);

    attachmentEventId = gantt.attachEvent("onLightbox", function (id) {
        var description = gantt.getLightboxSection('description');
        description.control.disabled = true;
        if ($(document).find('#securityProjectEdit').val() == 'False') {
            $('.gantt_section_time').find('input, textarea, button, select').attr('disabled', 'disabled');
        }

    });
    attachmentEventArray.push(attachmentEventId);

    attachmentEventId = gantt.attachEvent("onTaskDrag", function (id, mode, task, original) {
        var min_date = gantt.getState().min_date;
        var max_date = gantt.getState().max_date;
        if (+min_date >= +task.start_date) {
            gantt.config.start_date = min_date = gantt.date.add(min_date, -1, gantt.config.duration_unit);
            gantt.render()
        }
        if (+max_date <= +task.end_date) {
            gantt.config.end_date = max_date = gantt.date.add(max_date, 1, gantt.config.duration_unit);
            gantt.render()
        }
    });
    attachmentEventArray.push(attachmentEventId);

    attachmentEventId = gantt.attachEvent("onLightboxSave", function (id, task, is_new) {
        if (task.duration < 2) {
            gantt.alert({
                title: "",
                type: "alert-error",
                text: getResourceValue('EndDateMustBeGreaterThanStartDateAlert')
            });
            return false;
        }
        return true;
    });
    attachmentEventArray.push(attachmentEventId);

    gantt.config.min_duration = 2 * 24 * 60 * 60 * 1000; //24 * 60 * 60 * 1000 - 1 Day

    gantt.config.show_unscheduled = true; // show tasks having no start and end date
    gantt.config.fit_tasks = true; // automatically adjust start and end dates
    gantt.config.show_links = false; // hides all links in the Gantt chart
    //gantt.config.drag_links = false; // links created by dragging start and end point of task
    gantt.config.date_format = "%m/%d/%Y"; //date fields will be retrieved and send in this format
    gantt.config.date_grid = "%m/%d/%Y"; // date will be rendered in this format
    gantt.config.scales = [
        { unit: "day", step: 1, date: "%d" },
        { unit: "month", step: 1, date: "%M %Y" },
        //{ unit: "year", step: 1, date: "%Y" }
    ];
    gantt.templates.task_date = function (date) {
        return gantt.date.date_to_str(gantt.config.date_grid)(date);
    };

    gantt.templates.task_end_date = function (date) {
        return gantt.templates.task_date(new Date(date.valueOf() - 1));
    };

    gantt.templates.lightbox_header = function (start_date, end_date, task) {
        return gantt.templates.task_time(task.start_date, task.end_date, task);
    };

    gantt.init("ganttchart");
    RetrieveTasks();

    dp = gantt.createDataProcessor(function (mode, taskState, data, rowId) {
        switch (taskState) {
            case "update":
                if (data != null) {
                    data.progress = RoundUpto2DecimalPlaces(data.progress);
                }
                UpdateTask(data);
                break;
        }
    });
}
function DestroyGanttChart() {
    //Calling a destructor will:

    //clear the data loaded into a gantt instance
    //destroy the dataProcessor(if it is attached to the gantt)
    //detach the gantt from DOM
    //detach all DOM events attached via the event method
    if ($('#ganttchart').html() != undefined && $('#ganttchart').html() != null) {
        gantt.clearAll();
        if (dp != null && dp != undefined) {
            dp.destructor();
        }

        for (let i = 0; i < attachmentEventArray.length; i++) {
            var attachmentId = attachmentEventArray[i];
            gantt.detachEvent(attachmentId);
        }
        attachmentEventArray = [];
        attachmentEventId = '';
    }


}
function RetrieveTasks() {
    //gantt.clearAll();
    //gantt.load('/Project/RetrieveProjectTaskByProjectId?ProjectId=' + $('#projectModel_ProjectId').val())
    //    .then(function (xhr) {
    //        //console.log(xhr);
    //    });



    //    'json',
    //function (xhr) {
    //    console.log(xhr);
    //});
    //gantt.load('/Project/RetrieveProjectTaskByProjectId?ProjectId=' + $('#projectModel_ProjectId').val(),
    //    'json',
    //    function (a, b, c, d) { console.log(a, b, c, d) });
    $.ajax({
        url: '/Project/RetrieveProjectTaskByProjectId',
        type: 'POST',
        dataType: 'JSON',
        beforeSend: function () {
            ShowLoaderOnChart();
        },
        data: {
            'ProjectId': $('#projectModel_ProjectId').val()
        },
        success: function (res) {
            if (res.tasks != null && res.tasks.length > 0) {
                gantt.clearAll();
                gantt.parse(res);//need to call this to re render or update the data                
            }
            else {
                // to render blank data
                var blankGantt = { 'tasks': [] };
                gantt.clearAll();
                gantt.parse(blankGantt);
            }
        },
        complete: function () {
            //gantt.config.start_date = gantt.getState().min_date;
            //gantt.config.end_date = gantt.getState().max_date;
            gantt.render();
            HideLoaderOnChart();
        }
    });
}
function UpdateTask(task) {
    $.ajax({
        url: '/Project/UpdateProjectTaskFromTimeLine',
        type: 'POST',
        dataType: 'JSON',
        data: task,
        beforeSend: function () {
            ShowLoaderOnChart();
        },
        success: function (res) {
            if (res.Result === 'success') {
                SuccessAlertSetting.text = getResourceValue("ProjectUpdatedSuccessAlert");
                swal(SuccessAlertSetting, function () { });
            }
            else {
                ShowErrorAlert(res.ErrorMessages);
            }
        },
        error: function () {

        },
        complete: function () {
            HideLoaderOnChart();
            RetrieveTasks();
        }
    });
}
function RoundUpto2DecimalPlaces(data) {
    return +(Math.round(data + "e+2") + "e-2");
}
function ShowLoaderOnChart() {
    $('#ganttoverlay,#ganttloader').css('display', 'block');
}
function HideLoaderOnChart() {
    $('#ganttoverlay,#ganttloader').css('display', 'none');
}
function LoadTimelineTab() {
    $.ajax({
        url: '/Project/TimelineDetails',
        type: 'POST',
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data) {
                $(document).find('#Timeline').html(data);
            }
        },
        complete: function () {
            CloseLoader();
            GenerateGanttChart();
        },
        error: function (err) {
            CloseLoader();
        }
    });
}

/******* reference *****************
 https://snippet.dhtmlx.com/5/6c6103859?_ga=2.33262973.2103050340.1633505290-2080005884.1633505290
 https://docs.dhtmlx.com/gantt/api__gantt_task_end_date_template.html

 */

/* ------------------ NOTE -----------------
 * Here by default end date is rendering 1 less day in timeline. To maintain that
 * while retrieving the data 1 day is added in the controller and
 * at the time of saving 1 day is subtracted from the end date
 */