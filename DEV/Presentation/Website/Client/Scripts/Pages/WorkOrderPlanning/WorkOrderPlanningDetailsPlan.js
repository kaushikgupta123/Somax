//#region Commonn 
var run = false;
var woDatatTable;
var WOPlan_GridSearchtotalcount = 0;
var SelectWOPlan_GridSearchWorkOrderId = [];
var SelectWOPlan_GridSearchWorkOrderIdDetails = [];
var selectCountWO = 0;
var orderForWO = '1';
var orderDirForWO = 'asc';
//#endregion

//#region New Search button
$(document).on('click', "#WOSrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'WorkOrderLineItem' },
        beforeSend: function () {
            ShowbtnLoader("WOSrchBttnNew");
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)" id= "mem_' + i + '"' + '><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchListWO.innerHTML = str;
            $(document).find('#WOsearchBttnNewDrop').show("slideToggle");
        },
        complete: function () {
            HidebtnLoader("WOSrchBttnNew");
        },
        error: function () {
            HidebtnLoader("WOSrchBttnNew");
        }
    });
});
function GenerateSearchWOList(txtSearchval, isClear) {
    $.ajax({
        url: '/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'WorkOrderLineItem', searchText: txtSearchval, isClear: isClear },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)"><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchListWO.innerHTML = str;
        },
        complete: function () {
            if (isClear == false) {
                woDatatTable.page('first').draw('page');
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
$(document).on('keyup', '#txtColumnWOSearch', function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == 13) {
        WOTextSearch();
    }
    else {
        event.preventDefault();
    }
});
$(document).on('click', '.txtSearchWOClick', function () {
    WOTextSearch();
});
function WOTextSearch() {
    
    run = true;
    clearWoAdvanceSearch();
    $("#gridadvsearchstatus").val('');
    var txtSearchval = LRTrim($(document).find('#txtColumnWOSearch').val());
    if (txtSearchval) {
        GenerateSearchWOList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnWOSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossWO" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritemsWO").html(searchitemhtml);
    }
    else {
        generateWorkOrderDataTableForWorkOrderPlan();
    }
    var container = $(document).find('#WOsearchBttnNewDrop');
    container.hide("slideToggle");
}
$(document).on('click', '#UlSearchListWO li', function () {
    var v = LRTrim($(this).text());
    $(document).find('#txtColumnWOSearch').val(v);
    WOTextSearch();
});
$(document).on('click', '#cancelText', function () {
    $(document).find('#txtColumnWOSearch').val('');
});
$(document).on('click', '#clearTextWO', function () {
    GenerateSearchWOList('', true);
});

function clearWoAdvanceSearch() {
    selectCountWO = 0;
    $("#EquipmentClientLookupId").val("");
    $('#Name').val("");
    $("#Description").val("");
    $("#RequiredDate").val("").trigger('change');
    $("#advsearchfilteritemsWO").html('');
    $(".filteritemcountWO").text(selectCountWO);

}
//#endregion

//#region Dropdown toggle   
$(document).mouseup(function (e) {
    var container = $(document).find('#WOsearchBttnNewDrop');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slideToggle");
    }
});


//#endregion

//#region Advance Search
function LoadWOAdvancedSearchComponents() {
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();

    });
    $("#sidebarWOCollapse").on('click', function () {
   
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    $("#btnWODataAdvSrch").on('click', function (e) {
        run = true;
        $(document).find('#txtColumnWOSearch').val('');
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
        WOAdvSearch();
        woDatatTable.page('first').draw('page');
    });
    function WOAdvSearch() {
        $('#txtColumnWOSearch').val('');
        var searchitemhtml = "";
        selectCountWO = 0;
        $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
            if ($(this).hasClass('dtpicker')) {
                $(this).val(ValidateDate($(this).val()));
            }
            if ($(this).attr('id') != 'advrequiredDatedaterange') {
                if ($(this).val()) {
                    selectCountWO++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossWO" aria-hidden="true"></a></span>';
                }
            }
            else {
                if ($(this).val()) {
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossWO" aria-hidden="true"></a></span>';
                }
            }

        });
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritemsWO").html(searchitemhtml);
       // $('#_advrequiredDatedaterange').hide();
        $(".filteritemcountWO").text(selectCountWO);
    }
   

    $(document).on('click', '.btnCrossWO', function () {
       
        run = true;
        var btnCrossed = $(this).parent().attr('id');
        var searchtxtId = btnCrossed.split('_')[1];
        $('#' + searchtxtId).val('').trigger('change');
        $(this).parent().remove();
        selectCountWO--;
        WOAdvSearch();
        woDatatTable.page('first').draw('page');
    });
}
//#endregion

function LoadPlanListTab(WorkOrderPlanID)
{
    PlanHeaderData(WorkOrderPlanID);
}
function PlanHeaderData(WorkOrderPlanID)
{
    $.ajax({
        url: '/WorkOrderPlanning/PlanHeaderList',
        type: 'POST',
        dataType: 'html',
        data: {
            WorkOrderPlanID: WorkOrderPlanID
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data) {
                $(document).find('#Plan').html(data);
            }
        },
        complete: function () {
            CloseLoader();
            generateWorkOrderDataTableForWorkOrderPlan();
            LoadWOAdvancedSearchComponents();
            PreventEventForWoLookupGridSearch();
            LoadActivity(WorkOrderPlanID);
            LookupGridResetTextboxValue();
            SelectWOPlan_WorkOrderId = [];
            SelectWOPlan_WorkOrderIdDetails = [];

        },
        error: function (err) {
            CloseLoader();
        }
    });
}

//#region Work order grid section for work order plan



function generateWorkOrderDataTableForWorkOrderPlan() {
    selectCountWO = 0;
    SelectWOPlan_GridSearchWorkOrderId = [];
    SelectWOPlan_GridSearchWorkOrderIdDetails = [];
    var workOrderPlanId = $("#workorderPlanningModel_WorkOrderPlanId").val();
    if ($(document).find('#WoPlanningDetailsSearch').hasClass('dataTable')) {
        woDatatTable.destroy();
    }
    woDatatTable = $("#WoPlanningDetailsSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 2
        },
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        "stateSave": true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = orderForWO;
                    data.order[0][1] = orderDirForWO;
                }
                var filterinfoarrayForWO = getfilterinfoarrayForWO($("#txtColumnWOSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: "WorkOrderForWorkOrderPlan_Search",
                        LayOutInfo: JSON.stringify(data),
                        FilterInfo: JSON.stringify(filterinfoarrayForWO)
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
                    GridName: "WorkOrderForWorkOrderPlan_Search"
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    
                    /*selectCountWO = 0;*/
                    if (json.LayoutInfo !== '') {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        orderForWO = LayoutInfo.order[0][0];
                        orderDirForWO = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo !== '') {
                            setsearchuiWO(JSON.parse(json.FilterInfo), $("#txtColumnWOSearch"), $(".filteritemcountWO"), $("#advsearchfilteritemsWO"));
                        }
                    }
                    else {
                        callback(json.LayoutInfo);
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
        "orderMulti": true,
        "ajax": {
            "url": "/WorkOrderPlanning/GetWorkOrderGridDataForWorkOrderPlan",
            "type": "post",
            "datatype": "json",
            cache: false,
            data: function (d) {
                d.WorkOrderPlanID = workOrderPlanId;
                d.equipmentClientLookupId = LRTrim($("#EquipmentClientLookupId").val());
                d.chargeToName = LRTrim($("#Name").val());
                d.description = LRTrim($("#Description").val());
                d.requiredDate = LRTrim($("#RequiredDate").val());
                d.type = LRTrim($("#Type").val());
                d.searchText = LRTrim($(document).find('#txtColumnWOSearch').val());
                d.orderForWO = orderForWO;
                //d.orderDir = orderDir;
            },
            "dataSrc": function (result) {
                let colOrder = woDatatTable.order();
                orderForWO = colOrder[0][0];
                orderDirForWO = colOrder[0][1];
                WOPlan_GridSearchtotalcount = result.recordsTotal;
                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                //if ($('input[id="woforplansearch-select-all"]').is(':checked')) {
                //    $('input[id="woforplansearch-select-all"]').prop('checked', false);
                //}
                if (SelectWOPlan_GridSearchWorkOrderIdDetails.length == WOPlan_GridSearchtotalcount && WOPlan_GridSearchtotalcount!=0) {
                    $('input[id="woforplansearch-select-all"]').prop('checked', true);
                }
                else {
                    $('input[id="woforplansearch-select-all"]').prop('checked', false);
                }
                return result.data;
            }
        },
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        "columns":
            [
                {
                    "data": "WOPlanLineItemId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    'render': function (data, type, full, meta) {
                        if ($('#woforplansearch-select-all').is(':checked') && WOPlan_GridSearchtotalcount == SelectWOPlan_GridSearchWorkOrderId.length) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="chkWOPlan_GridSearch ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        } else {
                            if (SelectWOPlan_GridSearchWorkOrderId.indexOf(data) != -1) {
                                return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="chkWOPlan_GridSearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                            else {
                                return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="chkWOPlan_GridSearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                        }
                    }
                },
                {
                    "data": "WorkOrderClientLookupId",
                    "orderable": true,
                    "bSortable": true,
                    "autoWidth": false,
                    "bSearchable": true,
                    className: 'text-left',
                    "name": "0"
                },
                { "data": "Description", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true, "name": "1" },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true, "name": "2" },
                { "data": "RequiredDate", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true, "name": "3" },
                { "data": "EquipmentClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true, "name": "4" },
                { "data": "ChargeTo_Name", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true, "name": "5" },
                { "data": "WOPlanLineItemType", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true, "name": "6" },
            ],
        columnDefs: [
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();

            $(document).find('.dtpicker').datepicker({
                minDate: 0,
                "dateFormat": "mm/dd/yy",
                autoclose: true,
                changeMonth: true,
                changeYear: true
            });
            $(document).find('#RequiredDate').datepicker({
                //minDate: 0,
                "dateFormat": "mm/dd/yy",
                autoclose: true,
                changeMonth: true,
                changeYear: true
            });
        }
    });

    $('#WoPlanningDetailsSearch').find('th').click(function () {
        if ($(this).data('col')) {
            run = true;
            orderForWO = $(this).data('col');
        }        
    });

    $(document).on('change', '#WoPlanningDetailsSearch_length .searchdt-menu', function () {
        run = true;
    });

    $(document).on('click', '#WoPlanningDetailsSearch .paginate_button', function () {
        run = true;
    });
}

function getfilterinfoarrayForWO(txtsearchelement, advsearchcontainer) {
    var filterinfoarrayForWO = [];
    var f = new filterinfo('searchstring', LRTrim(txtsearchelement.val()));
    filterinfoarrayForWO.push(f);
    advsearchcontainer.find('.adv-item').each(function (index, item) {
        if ($(this).parent('div').is(":visible")) {
            f = new filterinfoForWO($(this).attr('id'), $(this).val());
            filterinfoarrayForWO.push(f);
        }
    });
    return filterinfoarrayForWO;
}
function filterinfoForWO(id, value) {
    this.key = id;
    this.value = value;
}

$(document).on('click', '#woforplansearch-select-all', function (e) {
    var workOrderPlanId = $("#workorderPlanningModel_WorkOrderPlanId").val();
     SelectWOPlan_GridSearchWorkOrderId = [];
    SelectWOPlan_GridSearchWorkOrderIdDetails = [];
    var colname = orderForWO;
    var coldir = orderDirForWO;
    var checked = this.checked;
    $.ajax({
        "url": "/WorkOrderPlanning/GetWorkOrderGridDataForWorkOrderPlanSelectAllData",
        data:
        {
            colname: colname,
            coldir: coldir,
            WorkOrderPlanID : workOrderPlanId,
            equipmentClientLookupId : LRTrim($("#EquipmentClientLookupId").val()),
            chargeToName : LRTrim($("#Name").val()),
            description : LRTrim($("#Description").val()),
            requiredDate : LRTrim($("#RequiredDate").val()),
            type : LRTrim($("#Type").val()),
            searchText : LRTrim($(document).find('#txtColumnWOSearch').val()),
            orderForWO: orderForWO
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
                        var exist = $.grep(SelectWOPlan_GridSearchWorkOrderIdDetails, function (obj) {
                            return obj.WorkOrderId === item.WOPlanLineItemId;
                        });
                        if (exist.length == 0) {
                            SelectWOPlan_GridSearchWorkOrderIdDetails.push(item.WOPlanLineItemId);
                            SelectWOPlan_GridSearchWorkOrderId.push(item.WOPlanLineItemId);
                        }
                    }
                    else {
                        SelectWOPlan_WorkOrderIdDetails = $.grep(SelectWOPlan_GridSearchWorkOrderIdDetails, function (obj) {
                            return obj.WorkOrderId !== item.WOPlanLineItemId;
                        });

                        var i = SelectWOPlan_GridSearchWorkOrderId.indexOf(item.WOPlanLineItemId);
                        SelectWOPlan_GridSearchWorkOrderId.splice(i, 1);
                    }

                });
            }
        },
        complete: function () {
            $('.WoPlanningDetailsSearchitemcount').text(SelectWOPlan_GridSearchWorkOrderIdDetails.length);
            if (SelectWOPlan_GridSearchWorkOrderIdDetails.length == 0) {
                $("#DivWoPlanningDetailsSearchitemcount").hide();
            } else {
                $("#DivWoPlanningDetailsSearchitemcount").show();
            }
            if (checked) {
                $(".actionBar").hide();
                $(document).find('.dt-body-center').find('.chkWOPlan_GridSearch').prop('checked', 'checked');

            }
            else {
                $(".actionBar").show();
                $(document).find('.dt-body-center').find('.chkWOPlan_GridSearch').prop('checked', false);


            }
            CloseLoader();
        }
    });
});

$(document).on('change', '.chkWOPlan_GridSearch', function () {
    $(".actionBar").hide();
    var data = woDatatTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        var el = $('#woforplansearch-select-all').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }

        var index = SelectWOPlan_GridSearchWorkOrderId.indexOf(data.WOPlanLineItemId);
        SelectWOPlan_GridSearchWorkOrderId.splice(index, 1);
        SelectWOPlan_GridSearchWorkOrderIdDetails.splice(index, 1);
    }
    else {
        var item = data.WOPlanLineItemId;
        var found = SelectWOPlan_GridSearchWorkOrderIdDetails.some(function (el) {
            return el.WOPlanLineItemId === data.WOPlanLineItemId;
        });
        if (!found) {
            SelectWOPlan_GridSearchWorkOrderId.push(item);
            SelectWOPlan_GridSearchWorkOrderIdDetails.push(item);
        }

    }
    if (SelectWOPlan_GridSearchWorkOrderIdDetails.length == WOPlan_GridSearchtotalcount) {
        $(document).find('.dt-body-center').find('#woforplansearch-select-all').prop('checked', 'checked');
    }
    else {
        $(document).find('.dt-body-center').find('#woforplansearch-select-all').prop('checked', false);
    }

    $('.WoPlanningDetailsSearchitemcount').text(SelectWOPlan_GridSearchWorkOrderId.length);
    if (SelectWOPlan_GridSearchWorkOrderId.length == 0) {
        $("#DivWoPlanningDetailsSearchitemcount").hide();
        $(".actionBar").show();
    } else
    {
        $("#DivWoPlanningDetailsSearchitemcount").show();
        $(".actionBar").hide();
    }
});
$(document).on('click', '#RemoveWorkOderPlan', function () {
    if (SelectWOPlan_GridSearchWorkOrderId.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    } else {
        swal({
            title: getResourceValue("spnAreyousure"),
            text:  getResourceValue("ConfirmRemoveWorkOrderAlert"),
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: false,
            confirmButtonClass: "btn-sm btn-primary",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("CancelAlertYes"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        }, function () {
            ShowLoader();
            var jsonResult = {
                "WOPlanLineItem": SelectWOPlan_GridSearchWorkOrderId
            };
            {
                $.ajax({
                    url: '/WorkOrderPlanning/RemoveWOPlanLineItem',
                    data: jsonResult,
                    type: "POST",
                    datatype: "json",
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (result) {
                        if (result.success == true) {
                           
                            SuccessAlertSetting.text = getResourceValue("WorkOrderRemoveAlert");
                            swal(SuccessAlertSetting, function () {
                                SearchgridValuesReset();
                            });
                        }
                        else {
                            CloseLoader();
                            return false;
                        }

                    },
                    complete: function () {
                        CloseLoader();
                    }
                });
            }
        });
    }
    
});

function SearchgridValuesReset() {
    CloseLoader();
    generateWorkOrderDataTableForWorkOrderPlan();
    if (SelectWOPlan_GridSearchWorkOrderId.length == 0) {
        $("#DivWoPlanningDetailsSearchitemcount").hide();
        $(".actionBar").show();
    } else {
        $("#DivWoPlanningDetailsSearchitemcount").show();
        $(".actionBar").hide();
    }
    $(document).find('.dt-body-center').find('#woforplansearch-select-all').prop('checked', false);
    $(document).find('.chkWOPlan_GridSearch').prop('checked', false);
    $('.WoPlanningDetailsSearchitemcount').text(0);
    SelectWOPlan_GridSearchWorkOrderIdDetails = [];
    SelectWOPlan_GridSearchWorkOrderId = [];
}

function setsearchuiWO(data, txtsearchelement, advcountercontainer, searchstringcontainer) {
    var searchitemhtml = '';
    $.each(data, function (index, item) {
        if (item.key == 'searchstring' && item.value) {
            var txtSearchval = item.value;
            if (item.value) {
                txtsearchelement.val(txtSearchval);
                searchitemhtml = "";
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + txtsearchelement.attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossWO" aria-hidden="true"></a></span>';
            }
            return false;
        }
        else {
            if ($('#' + item.key).parent('div').is(":visible")) {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    selectCountWO++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossWO" aria-hidden="true"></a></span>';
                }
            }
            if (item.key == 'RequiredDate') {
                $("#RequiredDate").trigger('change.select2');
            }
            advcountercontainer.text(selectCountWO);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
}

//#endregion

//#region  Look up List WO
var WoWorkplanPopupTable;
var WoClientLookupId = "";
var WoChargeTo = "";
var WoChargeTo_Name = "";
var WoDescription = "";
var WoStatus = "";
var WoPriority = "";
var WoType = "";
var woLookuptotalcount = 0;
var SelectWOPlan_WorkOrderId = [];
var SelectWOPlan_WorkOrderIdDetails = [];
var woLookuporder = '1';
var woLookuporderDir = 'asc';
function generateWoWorkOrderPlanLookupTable() {
  
   
    var rCount = 0;
    var visibility;
    if ($(document).find('#PlanWoWorkOrderPlanLookupTable').hasClass('dataTable')) {
        WoWorkplanPopupTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    WoWorkplanPopupTable = $("#PlanWoWorkOrderPlanLookupTable").DataTable({
        colReorder: {
            fixedColumnsLeft: 2
        },
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        "pageLength": 10,
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true,
        },
     
        sDom: 'Btlipr',
        "orderMulti": true,
        "ajax": {
            "url": "/WorkOrderPlanning/GetWOWorkOrderPLannLookupListchunksearch",
          
            data: function (d) {
                d.clientLookupId = WoClientLookupId;
                d.ChargeTo = WoChargeTo;
                d.ChargeTo_Name = WoChargeTo_Name;
                d.Description = WoDescription;
                d.Status = WoStatus;
                d.Priority = WoPriority;
                d.Type = WoType;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                woLookuptotalcount = json.recordsTotal;
                let woLookuporder = WoWorkplanPopupTable.order();
                woLookuporderDir = woLookuporder[0][1];
                //if ($('#woforplanLookupsearch-select-all').is(':checked')) {
                //    $(document).find('.dt-body-center').find('#woforplanLookupsearch-select-all').prop('checked', false);
                //}
                if (SelectWOPlan_WorkOrderIdDetails.length == woLookuptotalcount && woLookuptotalcount!=0) {
                    $(document).find('.dt-body-center').find('#woforplanLookupsearch-select-all').prop('checked', true);
                }
                else {
                    $(document).find('.dt-body-center').find('#woforplanLookupsearch-select-all').prop('checked', false);
                }
                
                return json.data;
            }
        },
        //fixedColumns: {
        //    leftColumns: 2
        //},
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        "columns":
            [
                {
                    "data": "WorkOrderId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    'render': function (data, type, full, meta) {
                        if ($('#woforplanLookupsearch-select-all').is(':checked') && woLookuptotalcount == SelectWOPlan_WorkOrderId.length) {
                            return '<input type="checkbox" checked="checked"  data-prid="' + data + '" class="chkWOPlan_WorkOrder ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        } else {
                            if (SelectWOPlan_WorkOrderId.indexOf(data) != -1) {
                                return '<input type="checkbox" checked="checked" data-prid="' + data + '" class="chkWOPlan_WorkOrder ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                            else {
                                return '<input type="checkbox"  data-prid="' + data + '" class="chkWOPlan_WorkOrder ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                        }
                    }
                },
                {
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true
                   
                },
                { "data": "ChargeTo", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "ChargeTo_Name", "autoWidth": true, "bSearchable": true, "bSortable": true
                },
                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Priority", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "RequiredDate", "autoWidth": true, "bSearchable": false, "bSortable": false }
            ],
        "rowCallback": function (row, data, index, full) {
            var colType = this.api().column(3).index('visible');
        },
        columnDefs: [
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            $(document).find('#tbleqpfooter').show();
            mcxDialog.closeLoading();
           SetPageLengthMenu();
            if (!$(document).find('#PlanWoWorkOrderPlanLookupModal').hasClass('show')) {
                $(document).find('#PlanWoWorkOrderPlanLookupModal').modal("show");
            }
           
            $('#PlanWoWorkOrderPlanLookupTable tfoot th').each(function (i, v) {
                var colIndex = i;
                if (colIndex > 0 && colIndex < 8) {
                    var title = $('#PlanWoWorkOrderPlanLookupTable thead th').eq($(this).index()).text();
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                    if (WoClientLookupId) { $('#colindex_1').val(WoClientLookupId); }
                    if (WoChargeTo) { $('#colindex_2').val(WoChargeTo); }
                    if (WoChargeTo_Name) { $('#colindex_3').val(WoChargeTo_Name); }
                    if (WoDescription) { $('#colindex_4').val(WoDescription); }
                    if (WoStatus) { $('#colindex_5').val(WoStatus); }
                    if (WoPriority) { $('#colindex_6').val(WoPriority); }
                    if (WoType) { $('#colindex_7').val(WoType); }
                }
               

            });

            $('#PlanWoWorkOrderPlanLookupTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    WoClientLookupId = $('#colindex_1').val();
                    WoChargeTo = $('#colindex_2').val();
                    WoChargeTo_Name = $('#colindex_3').val();
                    WoDescription = $('#colindex_4').val();
                    WoStatus = $('#colindex_5').val();
                    WoPriority = $('#colindex_6').val();
                    WoType = $('#colindex_7').val();
                    WoWorkplanPopupTable.page('first').draw('page');
                }
            });
           
        }
    });
}

$(document).on('click', '#woforplanLookupsearch-select-all', function (e) {
    SelectWOPlan_WorkOrderIdDetails = [];
    SelectWOPlan_WorkOrderId = [];
  
    WoWorkplanPopupTable = $("#PlanWoWorkOrderPlanLookupTable").DataTable();
    var colname = woLookuporder;
    var coldir = woLookuporderDir;
    var checked = this.checked;
    $.ajax({
        "url": "/WorkOrderPlanning/GetWOWorkOrderPLanLookupListSelectAllData",
        data:
        {
            colname : colname,
            coldir : coldir,
            clientLookupId : WoClientLookupId,
            ChargeTo : WoChargeTo,
            ChargeTo_Name : WoChargeTo_Name,
            Description : WoDescription,
            Status : WoStatus,
            Priority : WoPriority,
            Type : WoType
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
                        var exist = $.grep(SelectWOPlan_WorkOrderIdDetails, function (obj) {
                            return obj.WorkOrderId === item.WorkOrderId;
                        });
                        if (exist.length == 0) {
                            SelectWOPlan_WorkOrderIdDetails.push(item.WorkOrderId);
                            SelectWOPlan_WorkOrderId.push(item.WorkOrderId);
                        }
                        }
                    else {
                        SelectWOPlan_WorkOrderIdDetails = $.grep(SelectWOPlan_WorkOrderIdDetails, function (obj) {
                            return obj.WorkOrderId !== item.WorkOrderId;
                        });

                        var i = SelectWOPlan_WorkOrderId.indexOf(item.WorkOrderId);
                        SelectWOPlan_WorkOrderId.splice(i, 1);
                    }

                });
            }
        },
        complete: function () {
            $('.WoLookupGriditemcount').text(SelectWOPlan_WorkOrderIdDetails.length);
         
            if (checked) {

                $(document).find('.dt-body-center').find('.chkWOPlan_WorkOrder').prop('checked', 'checked');
                
            }
            else {
                $(document).find('.dt-body-center').find('.chkWOPlan_WorkOrder').prop('checked', false);
              

            }
            CloseLoader();
        }
    });
});


$(document).on('change', '.chkWOPlan_WorkOrder', function () {
    var data = WoWorkplanPopupTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        var el = $('#woforplanLookupsearch-select-all').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
      
        var index = SelectWOPlan_WorkOrderId.indexOf(data.WorkOrderId);
        SelectWOPlan_WorkOrderId.splice(index, 1);
        SelectWOPlan_WorkOrderIdDetails.splice(index, 1);
    }
    else {
        var item = data.WorkOrderId;
        var found = SelectWOPlan_WorkOrderIdDetails.some(function (el) {
            return el.WorkOrderId === data.WorkOrderId;
        });
        if (!found) {
            SelectWOPlan_WorkOrderIdDetails.push(item);
            SelectWOPlan_WorkOrderId.push(data.WorkOrderId);
        }

    }
    if (SelectWOPlan_WorkOrderIdDetails.length == woLookuptotalcount) {
        $(document).find('.dt-body-center').find('#woforplanLookupsearch-select-all').prop('checked', 'checked');
    }
    else {
        $(document).find('.dt-body-center').find('#woforplanLookupsearch-select-all').prop('checked', false);
    }
   
    $('.WoLookupGriditemcount').text(SelectWOPlan_WorkOrderId.length);
});

$(document).on('click', '#AddPopupWo', function () {
    if (SelectWOPlan_WorkOrderId.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var type = "";
        if ($(document).find("#PlanAddWorkOrders").length == 0) {
            type = "BreakIn";
        } else {
            type = "Plan";
        }
        var workOrderPlanId = $("#workorderPlanningModel_WorkOrderPlanId").val();
        var jsonResult = {
            "WOPlanLineItem": SelectWOPlan_WorkOrderId, "workOrderPlanId": workOrderPlanId, Type:type
        }
        {
            $.ajax({
                url: '/WorkOrderPlanning/AddWOPlanLineItem',
                data: jsonResult,
                type: "POST",
                datatype: "json",
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (result) {
                    if (result.success == true) {
                        $(document).find('#PlanWoWorkOrderPlanLookupModal').modal("hide");
                        LookupGridResetTextboxValue();
                        SearchgridValuesReset();
                    }
                    else {
                        CloseLoader();
                        return false;
                    }

                },
                complete: function () {
                    CloseLoader();
                    $(document).find('.dt-body-center').find('#woforplanLookupsearch-select-all').prop('checked', false);
                    $(document).find('.chkWOPlan_WorkOrder').prop('checked', false);
                    $('.WoLookupGriditemcount').text(0);
                    SelectWOPlan_WorkOrderIdDetails = [];
                    SelectWOPlan_WorkOrderId = [];

                }
            });
        }
    }
});

function PreventEventForWoLookupGridSearch() {
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
}
function LookupGridResetTextboxValue() {

     WoClientLookupId = "";
     WoChargeTo = "";
     WoChargeTo_Name = "";
     WoDescription = "";
     WoStatus = "";
     WoPriority = "";
     WoType = "";
}

$(document).on('click', '#PlanAddWorkOrders', function () {
    generateWoWorkOrderPlanLookupTable();
});
//#endregion



//#region Edit new WOP
$(document).on('click', '#editworkorderplan', function () {
    var workOrderPlanId = $("#workorderPlanningModel_WorkOrderPlanId").val();
    $.ajax({
        url: "/WorkOrderPlanning/EditWorkOderPlan",
        type: "GET",
        data: { WorkOrderPlanID: workOrderPlanId},
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#workorderplanningmaincontainer').html("");
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
function WOPEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.Command == "Edit") {
            SuccessAlertSetting.text = getResourceValue("WorkOrderPlanUpdateAlert");
            swal(SuccessAlertSetting, function () {
                RedirectToWOPDetail(data.workOrderPlanId);
            });
        }
        else {
            SuccessAlertSetting.text = getResourceValue("WorkOrderPlanUpdateAlert");;
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
              /*  $('#workorderPlanningModel_PersonnelId').val('').change();*/
                $(document).find('form').find("input").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};

$(document).on('click', "#btnCancelEditWOP", function () {
    var workOrderPlanId = $("#workorderPlanningModel_WorkOrderPlanId").val();
    swal(CancelAlertSetting, function () {
        RedirectToWOPDetail(workOrderPlanId);
    });
});


//Endregion Wop Edit
//#region  WOP Status
function WOPStatusUpdate(status)
{
    if (status != "") {
        var workOrderPlanId = $("#workorderPlanningModel_WorkOrderPlanId").val();
        $.ajax({
            url: "/WorkOrderPlanning/UpdatingStatusPlanWorkOderPlan",
            type: "POST",
            data: { WorkOrderPlanID: workOrderPlanId, Status: status },
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {

            },
            complete: function () {
                RedirectToWOPDetail(workOrderPlanId);
            },
            error: function () {
                CloseLoader();
            }
        });
    }
  

}
$(document).on('click', '#btnbreakinworkOrders', function () {
    generateWoWorkOrderPlanLookupTable();
});

//#endregion
$(document).on('click', '#PlanAddSchedule', function () {
    if (SelectWOPlan_GridSearchWorkOrderId.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    } else {
        AddScheduleWOP();
    }
});
$(document).on('click', '#PlanRemoveSchedule', function () {
    if (SelectWOPlan_GridSearchWorkOrderId.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    } else {
       
        RemoveScheduleWOP();
    }
});
function RemoveScheduleWOP() {

    var workOrderPlanId = $("#workorderPlanningModel_WorkOrderPlanId").val();
    CancelAlertSettingForCallback.text = getResourceValue("ConfirmRemoveScheduleAlert");
    swal(CancelAlertSettingForCallback, function () {
        $.ajax({
            url: "/WorkOrderPlanning/RemoveScheduleWOP",
            type: "POST",
            dataType: 'JSON',
            data: {
                'WorkOrderPlanID': workOrderPlanId, "WOPlanLineItem": SelectWOPlan_GridSearchWorkOrderId
              
            },
        /*    async: false,*/
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data === "success") {
                 
                    SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyRemovedScheduled");
                    swal(SuccessAlertSetting, function () {
                     
                    });
                }
            },
            complete: function () {
                SearchgridValuesReset();
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    });
}
//#region Add schedule 
function AddScheduleWOP() {
    $.ajax({
        url: "/WorkOrderPlanning/AddSchedulePlanModal",
        type: "POST",
        beforeSend: function () {
            ShowLoader();
        },
        data: {
            WorkOrderPlanId: $('#workorderPlanningModel_WorkOrderPlanId').val()
        },
        success: function (data) {
            $('#AddScheduleWOPPopUp').html(data);
            $('#AddScheduleWOP').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            WOPSetControls();
            $('#WOPStartDate').val(PlanStartDate);
            $('#WOPEndDate').val(PlanEndDate);
            $('#WOPScheduleModel_WorkOrderPlanId').val($("#workorderPlanningModel_WorkOrderPlanId").val());
            
        },
        error: function () {
            CloseLoader();
        }
    });
}
function WOPScheduleAddOnSuccess(data) {
    if (data.data === "success") {
        SuccessAlertSetting.text = getResourceValue("ScheduledWOAddAlert");
        $("#AddScheduleWOP").modal('hide');
        swal(SuccessAlertSetting, function () {
            $('#AddScheduleWOPPopUp').html('');
            SearchgridValuesReset();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function WOPSetControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true,
        // minDate: new Date()
    }).inputmask('mm/dd/yyyy');

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
$(document).on('click', '.WopcloseAddcheduleModal', function () {
    var areaChargeTo = "";
    $(document).find('#AddScheduleWOP select').each(function (i, item) {
        areaChargeTo = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeTo).hide();
    });
    $('#AddScheduleWOPPopUp').html('');
});
$(document).on('click', '#btnSubmitWOP', function () {
    $('#WOPScheduleModel_WorkOrderIds').val(SelectWOPlan_GridSearchWorkOrderId);
});
//#endregion
//Activity & Comments
var selectedUsers = [];
var selectedUnames = [];
var colorarray = [];
function colorobject(string, color) {
    this.string = string;
    this.color = color;
};
function LoadActivity(WorkOrderPlanId) {
    $.ajax({
        "url": "/WorkOrderPlanning/LoadActivity",
        data: { WorkOrderPlanId: WorkOrderPlanId },
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
            LoadComments(WorkOrderPlanId);
        }
    });
}
function LoadComments(WorkOrderPlanId) {
    $.ajax({
        "url": "/WorkOrderPlanning/LoadComments",
        data: { WorkOrderPlanId: WorkOrderPlanId},
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
    LoadCkEditor('wotxtcomments');
    $("#wotxtcommentsnew").hide();
    $(".ckeditorfield").show();
});
$(document).on('click', '#btnsavecommandwop', function () {
    var selectedUsers = [];
    const data = getDataFromTheEditor();
    if (!data) {
        return false;
    }
    var workorderId = $(document).find('#workorderPlanningModel_WorkOrderPlanId').val();
    var woClientLookupId = $(document).find('#workorderPlanningModel_WorkOrderPlanId').val();
    var noteId = 0;
    if (LRTrim(data) == "") {
        return false;
    }
    else {
      /*  alert("ajax");*/
        $(document).find('.ck-content').find('.mention').each(function (item, index) {
            selectedUsers.push($(index).data('user-id'));
        });
        $.ajax({
            url: '/WorkOrderPlanning/AddComments',
            type: 'POST',
            beforeSend: function () {
                ShowLoader();
            },
            data: {
                WorkOrderPlanId: workorderId,
                content: data,
                woClientLookupId: woClientLookupId,
                userList: selectedUsers,
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
                        RedirectToWOPDetail(workorderId);
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
    LoadCkEditorEdit('wotxtcommentsEdit', rawHTML);
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
    var workorderId = $(document).find('#workorderPlanningModel_WorkOrderPlanId').val();
    var noteId = $(this).parents('.kt-notes__item').find('.editcomments').attr('id');
    var woClientLookupId = $(document).find('#workorderPlanningModel_WorkOrderPlanId').val();
    var updatedindex = $(this).parents('.kt-notes__item').find('.hdnupdatedindex').val();
    $.ajax({
        url: '/WorkOrderPlanning/AddComments',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        data: { WorkOrderPlanId: workorderId, content: LRTrim(data), woClientLookupId: woClientLookupId,noteId: noteId, updatedindex: updatedindex },
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
                    RedirectToWOPDetail(workorderId);
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
                        RedirectToWOPDetail($(document).find('#workorderPlanningModel_WorkOrderPlanId').val());
                    });
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
//#endregion
$(document).on('click', '#brdWOPlan', function () {
    var WorkOrderPlanId = $(this).attr('data-val');
    RedirectToWOPDetail(WorkOrderPlanId);
});

$(document).on('click', '#WOPreaddescription', function () {
    $(document).find('#WOPdetaildesmodaltext').text($(this).data("des"));
    $(document).find('#WOPdetaildesmodal').modal('show');
});


//#region  WOP Reopen Status Update (V2-1026)
function WOPReopenStatusUpdate(status) {
    if (status != "") {
        var workOrderPlanId = $("#workorderPlanningModel_WorkOrderPlanId").val();
        $.ajax({
            url: "/WorkOrderPlanning/UpdatingStatusReopenWorkOderPlan",
            type: "POST",
            data: { WorkOrderPlanID: workOrderPlanId, Status: status },
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {

            },
            complete: function () {
                RedirectToWOPDetail(workOrderPlanId);
            },
            error: function () {
                CloseLoader();
            }
        });
    }


}
$(document).on('click', '#btnbreakinworkOrders', function () {
    generateWoWorkOrderPlanLookupTable();
});

//#endregion