$(document).on('click', '.openchargetolookupgrid', function () {
    $(document).find('.chargetolookup').removeClass('chargeto-ele-lookup');
    $(document).find('.chargetolookupval').removeClass('chargeto-ele-lookup-hidden');
    $(this).siblings('.chargetolookup').addClass('chargeto-ele-lookup');
    $(this).siblings('.chargetolookupval').addClass('chargeto-ele-lookup-hidden');

    var chargetype = $(this).parent('div').parent('td').prev('td').find('.select-chargetype').val();

    switch (chargetype) {
        case "Account":
            generateShoppingCartAccountDataTable();
            break;
        case "WorkOrder":
            generateShoppingCartWorkOrderDataTable();
            break;
        case "Equipment":
            generateShoppingCartEquipDataTable();
            break;

    }

});
//#region Account
var dtAccountTable;
var accClientLookupId = "";
var accName = "";

function generateShoppingCartAccountDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#AccountPRTable').hasClass('dataTable')) {
        dtAccountTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtAccountTable = $("#AccountPRTable").DataTable({
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
            "url": "/Base/GetAccountLookupList",
            data: function (d) {
                d.ClientLookupId = accClientLookupId;
                d.Name = accName;

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
            {
                "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<a class=link_PRAccount href="javascript:void(0)">' + data + '</a>'
                }
            },

            {
                "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<span class="m-badge--custom">' + data + '</span>'
                }
            },
        ],
        "rowCallback": function (row, data, index, full) {
        },
        initComplete: function () {
            $(document).find('#tblAccountPRfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#AccountPRModal').hasClass('show')) {
                $(document).find('#AccountPRModal').modal("show");
            }

            $(document).find('#AccountPRTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#AccountPRTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="praccount tfootsearchtxt" id="praccountcolindex_' + colIndex + '"  /><i class="fa fa-search dropSearchIcon"></i>');
                if (accClientLookupId) { $('#praccountcolindex_0').val(accClientLookupId); }
                if (accName) { $('#praccountcolindex_1').val(accName); }

            });

            $('#AccountPRTable tfoot th').find('.praccount').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    accClientLookupId = $('#praccountcolindex_0').val();
                    accName = $('#praccountcolindex_1').val();
                    dtAccountTable.page('first').draw('page');
                }
            });
        }
    });
}
$(document).on('click', '.link_PRAccount', function (e) {
    var index_row = $('#AccountPRTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtAccountTable.row(row).data();
    $(document).find('.chargeto-ele-lookup').val(data.ClientLookupId);
    $(document).find('.chargeto-ele-lookup-hidden').val(data.AccountId); //---change ID   
    $(document).find('#ChargeToClientLookupId').val(data.ClientLookupId).css("display", "block");
    $(document).find('#ChargeToId').val(data.AccountId).removeClass('input-validation-error').css("display", "none");
    $(document).find('#ChargeToId').parent().find('div > button.ClearChargeToModalPopupGridData').css('display', 'block');
    //$(document).find('#ChargeTo_Name').val(data.Name);
    $(document).find("#AccountPRModal").modal('hide');
    $(document).find('.chargetolookup').removeClass('chargeto-ele-lookup');
    $(document).find('.chargetolookupval').removeClass('chargeto-ele-lookup-hidden');
});
$(document).on('click', '.ClearChargeToModalPopupGridData', function () {
    $(document).find('#' + $(this).data('textfield')).val('').css("display", "none");
    $(document).find('#' + $(this).data('valuefield')).val('').css("display", "block");
    $(this).css('display', 'none');
});
//#endregion

//#region workorder
var dtWorkOrderTable;
var woClientLookupId = "";
var woDescription = "";
var woChargeTo = "";
var woWorkAssigned = "";
var woRequestor = "";
var woStatus = "";

function generateShoppingCartWorkOrderDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#WorkOrderPRTable').hasClass('dataTable')) {
        dtWorkOrderTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtWorkOrderTable = $("#WorkOrderPRTable").DataTable({
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
            "url": "/Base/GetWorkOrderLookupList",
            data: function (d) {
                d.ClientLookupId = woClientLookupId;
                d.Description = woDescription;
                d.ChargeTo = woChargeTo;
                d.WorkAssigned = woWorkAssigned;
                d.Requestor = woRequestor;
                d.Status = woStatus;
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
            {
                "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<a class=link_PRWorkOrder href="javascript:void(0)">' + data + '</a>';
                }
            },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-300'>" + data + "</div>";
                }
            },
            {
                "data": "ChargeTo", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<span class="m-badge--custom">' + data + '</span>';
                }
            },
            {
                "data": "WorkAssigned", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<span class="m-badge--custom">' + data + '</span>';
                }
            },
            {
                "data": "Requestor", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<span class="m-badge--custom">' + data + '</span>';
                }
            },
            {
                "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<span class="m-badge--custom">' + data + '</span>';
                }
            },
        ],
        "rowCallback": function (row, data, index, full) {
        },
        initComplete: function () {
            $(document).find('#tblWorkOrderPRfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#WorkOrderPRModal').hasClass('show')) {
                $(document).find('#WorkOrderPRModal').modal("show");
            }


            $(document).find('#WorkOrderPRTable tfoot th').each(function (i, v) {
                var colIndex = i;
                if (colIndex !== 3 && colIndex !== 4) {
                    $(this).html('<input type="text" style="width:100%" class="prwo tfootsearchtxt" id="prwocolindex_' + colIndex + '"  /><i class="fa fa-search dropSearchIcon"></i>');
                    if (woClientLookupId) { $('#prwocolindex_0').val(woClientLookupId); }
                    if (woDescription) { $('#prwocolindex_1').val(woDescription); }
                    if (woChargeTo) { $('#prwocolindex_2').val(woChargeTo); }
                    if (woWorkAssigned) { $('#prwocolindex_3').val(woWorkAssigned); }
                    if (woRequestor) { $('#prwocolindex_4').val(woRequestor); }
                    if (woStatus) { $('#prwocolindex_5').val(woStatus); }

                }
                else {
                    $(this).html('');
                }
            });

            $('#WorkOrderPRTable tfoot th').find('.prwo').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    woClientLookupId = $('#prwocolindex_0').val();
                    woDescription = $('#prwocolindex_1').val();
                    woChargeTo = $('#prwocolindex_2').val();
                    woWorkAssigned = $('#prwocolindex_3').val();
                    woRequestor = $('#prwocolindex_4').val();
                    woStatus = $('#prwocolindex_5').val();
                    dtWorkOrderTable.page('first').draw('page');
                }
            });
        }
    });
}
$(document).on('click', '.link_PRWorkOrder', function (e) {
    var index_row = $('#WorkOrderPRTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtWorkOrderTable.row(row).data();
    $(document).find('.chargeto-ele-lookup').val(data.ClientLookupId);
    $(document).find('.chargeto-ele-lookup-hidden').val(data.WorkOrderId); //---change ID   
    $(document).find('#ChargeToClientLookupId').val(data.ClientLookupId).css("display", "block");
    $(document).find('#ChargeToId').val(data.WorkOrderId).removeClass('input-validation-error').css("display", "none");
    //$(document).find('#ChargeTo_Name').val(data.Name);
    $(document).find('#ChargeToId').parent().find('div > button.ClearChargeToModalPopupGridData').css('display', 'block');
    $(document).find("#WorkOrderPRModal").modal('hide');
    $(document).find('.chargetolookup').removeClass('chargeto-ele-lookup');
    $(document).find('.chargetolookupval').removeClass('chargeto-ele-lookup-hidden');    
});
//#endregion

//#region equipment

var equipTable;
var eqClientLookupId = "";
var eqName = "";
var eqModel = "";
var eqType = "";
var eqSerialNumber = "";

function generateShoppingCartEquipDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#EquipmentPRTable').hasClass('dataTable')) {
        equipTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    equipTable = $("#EquipmentPRTable").DataTable({
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
            "url": "/Base/GetEquipmentLookupList",
            data: function (d) {
                d.ClientLookupId = eqClientLookupId;
                d.Name = eqName;
                d.Model = eqModel;
                d.Type = eqType;
                d.SerialNumber = eqSerialNumber;
                d.InactiveFlag = false;
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
            {
                "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<a class=link_preqp_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "Model", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<span class="m-badge--custom">' + data + '</span>'
                }
            },
            { "data": "SerialNumber", "autoWidth": true, "bSearchable": true, "bSortable": true },
        ],
        "rowCallback": function (row, data, index, full) {
            var colType = this.api().column(3).index('visible');
            if (data.Type) {
                var color = "#" + intToARGB(hashCode(LRTrim(data.Type)));
                $('td', row).eq(colType).find('.m-badge--custom').css('background-color', color).css('color', '#fff');
            }
        },
        initComplete: function () {
            $(document).find('#tblEquipmentPRfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#EquipmentPRModal').hasClass('show')) {
                $(document).find('#EquipmentPRModal').modal("show");
            }

            $('#EquipmentPRTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#EquipmentPRTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="prequipment tfootsearchtxt" id="prequipmentcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (eqClientLookupId) { $('#prequipmentcolindex_0').val(eqClientLookupId); }
                if (eqName) { $('#prequipmentcolindex_1').val(eqName); }
                if (eqModel) { $('#prequipmentcolindex_2').val(eqModel); }
                if (eqType) { $('#prequipmentcolindex_3').val(eqType); }
                if (eqSerialNumber) { $('#prequipmentcolindex_4').val(eqSerialNumber); }
            });

            $('#EquipmentPRTable tfoot th').find('.prequipment').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    eqClientLookupId = $('#prequipmentcolindex_0').val();
                    eqName = $('#prequipmentcolindex_1').val();
                    eqModel = $('#prequipmentcolindex_2').val();
                    eqType = $('#prequipmentcolindex_3').val();
                    eqSerialNumber = $('#prequipmentcolindex_4').val();
                    equipTable.page('first').draw('page');
                }
            });
        }
    });
}
$(document).on('click', '.link_preqp_detail', function (e) {
    var index_row = $('#EquipmentPRTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = equipTable.row(row).data();
    $(document).find('.chargeto-ele-lookup').val(data.ClientLookupId);
    $(document).find('.chargeto-ele-lookup-hidden').val(data.EquipmentId); //---change ID   
    $(document).find("#EquipmentPRModal").modal('hide');
    $(document).find('.chargetolookup').removeClass('chargeto-ele-lookup');
    $(document).find('.chargetolookupval').removeClass('chargeto-ele-lookup-hidden');
});
$(document).ready(function () {
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });

});



