var Mainorder = '1';
var MainorderDir = 'asc';
var dtWOPTable;
var SelectWOPId = [];
var gridname = "WorkOrderPlan_Search";
var run = false;
var CustomQueryDisplayId = 0;
var WorkOrderPlanId = 0;

$(document).ready(function () {
    var WOPstatus = localStorage.getItem("WOPstatus");
    if (WOPstatus) {
        var text = "";
        CustomQueryDisplayId = WOPstatus;
        $('#WorkOrderPlansearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId) {
                text = $(this).text();
                $('#WoPlanningsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
    }
    else {
        WOPstatus = "2";
        CustomQueryDisplayId = WOPstatus;
        localStorage.setItem("WOPstatus", "2");
        $('#WorkOrderPlansearchListul li#2').addClass('activeState');
        $('#WoPlanningsearchtitle').text(getResourceValue("spnOpenWorkOrderPlans"));
    }
    generateWOPDataTable();


});

function generateWOPDataTable() {

    var printCounter = 0;
    if ($(document).find('#WoPlanningSearch').hasClass('dataTable')) {
        dtWOPTable.destroy();
    }
    dtWOPTable = $("#WoPlanningSearch").DataTable({
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
                    data.order[0][0] = Mainorder;
                    data.order[0][1] = MainorderDir;
                }
                
                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: gridname,
                        LayOutInfo: JSON.stringify(data),
                        FilterInfo: ''
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
                        Mainorder = LayoutInfo.order[0][0];
                        MainorderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));                        
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
                title: 'Work Order Plan',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'main-search'
                }

            },
            {
                extend: 'print',
                title: 'Work Order Plan',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'main-search'
                }

            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Work Order Plan',
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
                title: 'Work Order Plan',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'main-search'
                }
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/WorkOrderPlanning/GetWoPlanningGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.CustomQueryDisplayId = localStorage.getItem("WOPstatus");
                d.txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
                d.Order = Mainorder;
                //d.orderDir = orderDir;
            },
            "dataSrc": function (result) {
                let colOrder = dtWOPTable.order();
                MainorderDir = colOrder[0][1];

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
                    "data": "WorkOrderPlanId",
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
                    "data": "PlanID",
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
                { "data": "StartDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2", },
                { "data": "EndDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5",
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "Created", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
                { "data": "Completed", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date ", "name": "7" }

            ],
        initComplete: function (settings, json) {
            SetPageLengthMenu();

            $("#WoPlanningGridAction :input").removeAttr("disabled");
            $("#WoPlanningGridAction :button").removeClass("disabled");
            DisableExportButton($("#WoPlanningSearch"), $(document).find('.import-export'));
        }
    });
}

$(document).on('click', '.lnk_psearch', function (e) {
    e.preventDefault();
    var index_row = $('#WoPlanningSearch tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtWOPTable.row(row).data();
    WorkOrderPlanId = data.WorkOrderPlanId;
    var titletext = $('#WoPlanningsearchtitle').text();
    localStorage.setItem("WOPstatustext", titletext);
    $.ajax({
        url: "/WorkOrderPlanning/Details",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { WorkOrderPlanId: WorkOrderPlanId },
        success: function (data) {
            $('#workorderplanningmaincontainer').html(data);
            $(document).find('#spnlinkToSearch').text(titletext);
        },
        complete: function () {
            SetFixedHeadStyle();
            CloseLoader();
            LoadPlanListTab(WorkOrderPlanId);
          
        }
    });
});
$(document).on('click', '#WoPlanningSearch .paginate_button', function () {
    run = true;
});
$(document).on('change', '#WoPlanningSearch .searchdt-menu', function () {
    run = true;
});
$('#WoPlanningSearch').find('th').click(function () {
    if ($(this).data('col')) {
        run = true;
        Mainorder = $(this).data('col');
    }

});
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

//#region Inner Grid
$(document).find('#WoPlanningSearch').on('click', 'tbody td img', function (e) {
    var tr = $(this).closest('tr');
    var row = dtWOPTable.row(tr);
    if (this.src.match('details_close')) {
        this.src = "../../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        this.src = "../../Images/details_close.png";
        var WorkOrderPlanID = $(this).attr("rel");
        $.ajax({
            url: "/WorkOrderPlanning/GetWOPInnerGrid",
            data: {
                WorkOrderPlanID: WorkOrderPlanID
            },
            beforeSend: function () {
                ShowLoader();
            },
            dataType: 'html',
            success: function (json) {
                row.child(json).show();
                dtinnerGrid = row.child().find('.WOPinnerDataTable').DataTable(
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

//#endregion

//#region DetailPageSwitchTab
$(document).on('click', '.wop-det-tab', function (e) {
    ResetOtherTabs();    
    var tab = $(this).data('tab');
    switch (tab) {
        case "Plan":
            var WorkOrderPlanId = $("#workorderPlanningModel_WorkOrderPlanId").val();            
            LoadPlanListTab(WorkOrderPlanId);
            break;
        case "ResourceList":
            LoadResourceListTab();            
            break;
        case "ResourceCalender":
            LoadCalendarTab();
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
    $(document).find('#ResourceCalenderDiv').html('');
    $(document).find('#Plan').html('');
    $(document).find('#ResourceList').html('');
    $(document).find('#Dashboard').html("");
    //-- available work order
    RLAvailableSelectedItemArray = [];
    AWOGridTotalGridItemRL = [];
    SelectedLookupIdToAssignedRL = [];
    SelectedStatusAssignedRL = [];
    SelectedWoIdToAssignedRL = [];
    WorkOrderRLIds = [];
    //-- available work order
}
//#endregion

//#region Side View click
$(document).on('click', '.WoPlanningsearchdrpbox', function (e) {
    
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $(document).find('.chksearch').prop('checked', false);
    $('.itemcount').text(0);
    SelectedWoIdToCancel = [];
    run = true;
    if ($(this).attr('id') != '0') {
        $('#WoPlanningsearchtitle').text($(this).text());
        localStorage.setItem("WOPstatustext", $(this).text());
    }
    else {
        $('#WoPlanningsearchtitle').text(getResourceValue("spnAllStatuses"));
        localStorage.setItem("WOPstatustext", getResourceValue("spnAllStatuses"));
    }
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("WOPstatus", optionval);
    woStatus = optionval;
    CustomQueryDisplayId = optionval;
    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');

    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtWOPTable.page('first').draw('page');
    }

});

$(document).on('keyup', '#WoPlanningsearctxtbox', function (e) {
    var tagElems = $(document).find('#WorkOrderPlansearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
//#endregion

//#region Export Functionality
$(document).on('click', '#liWOPPdf,#liWOPPrint,#liWOPExcel,#liWOPCsv', function () {
    var thisid = $(this).attr('id');
    var TableHaederProp = [];
    function table(property, title) {
        this.property = property;
        this.title = title;
    }
    $("#WoPlanningSearch thead tr th").map(function (key) {
        var thisdiv = $(this).find('div');
        if ($(this).parents('.WOPinnerDataTable').length == 0 && thisdiv.html()) {
            if (this.getAttribute('data-th-prop')) {
                var tablearr = new table(this.getAttribute('data-th-prop'), thisdiv.html());
                TableHaederProp.push(tablearr);
            }
        }
    });
    var params = {
        tableHaederProps: TableHaederProp,
        colname: Mainorder,
        coldir: MainorderDir,
        CustomQueryDisplayId: CustomQueryDisplayId,

    };
    wopPrintParams = JSON.stringify({ 'wopPrintParams': params });
    $.ajax({
        "url": "/WorkOrderPlanning/WOPSetPrintData",
        "data": wopPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            if (thisid == 'liWOPPdf') {
                window.open('/WorkOrderPlanning/WOPExportASPDF?d=d', '_self');
            }
            else if (thisid == 'liWOPPrint') {
                window.open('/WorkOrderPlanning/WOPExportASPDF', '_blank');
            }
            else if (thisid == 'liWOPExcel') {
                window.open('/WorkOrderPlanning/WOPExportASPDF?d=excel', '_self');
            }
            else if (thisid == 'liWOPCsv') {
                window.open('/WorkOrderPlanning/WOPExportASPDF?d=csv', '_self');
            }
            return;
        }
    });
    $('#mask').trigger('click');
});

//#endregion

//#region Add new WOP
$(document).on('click', '.AddWoPlanning', function () {
    $.ajax({
        url: "/WorkOrderPlanning/AddNewWorkOderPlan",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#workorderplanningmaincontainer').html(data);
        },
        complete: function () {
            SetWOPControls();
        },
        error: function () {
            CloseLoader();
        }
    });

});

function WOPAddOnSuccess(data) {
    
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.Command == "save") {
            localStorage.setItem("WOPstatus", '2')
            //localStorage.setItem("WOPstatustext", getResourceValue("MyOpenRequestsAlert"));
            localStorage.setItem("WOPstatustext", getResourceValue("spnOpenWorkOrderPlans"));
            SuccessAlertSetting.text = getResourceValue("AddWorkOrderPlanningAlert");
            swal(SuccessAlertSetting, function () {
                RedirectToWOPDetail(data.workOrderPlanId);
            });
        }
        else {
            SuccessAlertSetting.text = getResourceValue("AddWorkOrderPlanningAlert");
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $('#workorderPlanningModel_PersonnelId').val('').change();
                $(document).find('form').find("input").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};

$(document).on('click', "#btnCancelAddWOP", function () {

    swal(CancelAlertSetting, function () {
        window.location.href = "../WorkOrderPlanning/Index?page=Maintenance_Work_Order_Planning";
    });
});
//#endregion

//#region common
function SetWOPControls() {
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
function RedirectToWOPDetail(WOPId) {
    $.ajax({
        url: "/WorkOrderPlanning/Details",
        type: "POST",
        dataType: "html",
        data: { WorkOrderPlanId: WOPId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#workorderplanningmaincontainer').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("WOPstatustext"));
            LoadPlanListTab(WOPId);
            WorkOrderPlanId = WOPId;
        },
        complete: function () {
            
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../WorkOrderPlanning/Index?page=Maintenance_Work_Order_Planning";
});
//#endregion
