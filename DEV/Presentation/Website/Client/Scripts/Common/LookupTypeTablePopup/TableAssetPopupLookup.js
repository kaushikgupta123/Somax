var equipTable;
var eqClientLookupId = "";
var NameEq = "";
var Model = "";
var Type = "";
var SerialNumber = "";
var AssetGroup1ClientLookupId = "";
var AssetGroup2ClientLookupId = "";
var AssetGroup3ClientLookupId = "";
var TextField = "";
var ValueField = "";
var Equipid = "";

$(document).on('click', '.OpenAssetModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    Equipid = $(this).data('equipmentid');
    generateAssetDataTable();
});

function generateAssetDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#tblAssetLookup').hasClass('dataTable')) {
        equipTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    equipTable = $("#tblAssetLookup").DataTable({
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
            "url": "/Base/GetEquipmentLookupListchunksearch",
            data: function (d) {
                d.ClientLookupId = eqClientLookupId;
                d.Name = NameEq;
                d.Model = Model;
                d.Type = Type;
                d.AssetGroup1ClientLookupId = AssetGroup1ClientLookupId;
                d.AssetGroup2ClientLookupId = AssetGroup2ClientLookupId;
                d.AssetGroup3ClientLookupId = AssetGroup3ClientLookupId;
                d.SerialNumber = SerialNumber;
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
                        return '<a class=link_woeqp_detail_common href="javascript:void(0)">' + data + '</a>'
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

                { "data": "AssetGroup1ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
                { "data": "AssetGroup2ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
                { "data": "AssetGroup3ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
                { "data": "SerialNumber", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        "rowCallback": function (row, data, index, full) {
            var colType = this.api().column(3).index('visible');
            if (data.Type) {
                var color = "#" + intToARGB(hashCode(LRTrim(data.Type)));
                $('td', row).eq(colType).find('.m-badge--custom').css('background-color', color).css('color', '#fff');
            }
        },
        initComplete: function () {
            $(document).find('#tblAssetLookupfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#AssetLookupModal').hasClass('show')) {
                $(document).find('#AssetLookupModal').modal("show");
            }
            $('#tblAssetLookup tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#tblAssetLookup thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="woequipment tfootsearchtxt" id="equipmentcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (eqClientLookupId) { $('#equipmentcolindex_0').val(eqClientLookupId); }
                if (NameEq) { $('#equipmentcolindex_1').val(NameEq); }
                if (Model) { $('#equipmentcolindex_2').val(Model); }
                if (Type) { $('#equipmentcolindex_3').val(Type); }
                if (AssetGroup1ClientLookupId) { $('#equipmentcolindex_4').val(AssetGroup1ClientLookupId); }
                if (AssetGroup2ClientLookupId) { $('#equipmentcolindex_5').val(AssetGroup2ClientLookupId); }
                if (AssetGroup3ClientLookupId) { $('#equipmentcolindex_6').val(AssetGroup3ClientLookupId); }
                if (SerialNumber) { $('#equipmentcolindex_7').val(SerialNumber); }
            });

            $('#tblAssetLookup tfoot th').find('.woequipment').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    eqClientLookupId = $('#equipmentcolindex_0').val();
                    NameEq = $('#equipmentcolindex_1').val();
                    Model = $('#equipmentcolindex_2').val();
                    Type = $('#equipmentcolindex_3').val();
                    AssetGroup1ClientLookupId = $('#equipmentcolindex_4').val();
                    AssetGroup2ClientLookupId = $('#equipmentcolindex_5').val();
                    AssetGroup3ClientLookupId = $('#equipmentcolindex_6').val();
                    SerialNumber = $('#equipmentcolindex_7').val();
                    equipTable.page('first').draw('page');
                }
            });
        }
    });
}

$(document).ready(function () {
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
});
$(document).on('click', '.link_woeqp_detail_common', function (e) {
    var index_row = $('#tblAssetLookup tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = equipTable.row(row).data();
    //$(document).find('#' + TextField).val(data.ClientLookupId).removeClass('input-validation-error');
    //$(document).find('#' + ValueField).val(data.EquipmentId);
    $(document).find('#' + TextField).val(data.ClientLookupId).css("display", "block");
    $(document).find('#' + ValueField).val(data.EquipmentId).removeClass('input-validation-error').css("display", "none");
    $(document).find('#' + ValueField).parent().find('div > button.ClearAssetModalPopupGridData').css('display', 'block');
    //$('#workOrderModel_ChargeTo_Name').val(data.Name);
    $(document).find("#AssetLookupModal").modal('hide');
    //V2-948
    var SourceAssetAccount = $(document).find("#SourceAssetAccount").val();
    if (SourceAssetAccount != undefined && SourceAssetAccount == "True") {
        getlaboraccount(data.EquipmentId);
        
    }
    //V2-948
});
$(document).on('hidden.bs.modal', '#AssetLookupModal', function () {
    TextField = "";
    ValueField = "";
});
$(document).on('click', '.ClearAssetModalPopupGridData', function () {
    $(document).find('#' + $(this).data('textfield')).val('').css("display", "none");
    $(document).find('#' + $(this).data('valuefield')).val('').css("display", "block");
    $(this).css('display', 'none');
});
//#region V2-948
function getlaboraccount(EquipmentId) {
    $.ajax({
        url: "../base/GetAccountByEquipmentId",
        type: 'GET',
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        data: { EquipmentId: EquipmentId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            CloseLoader();
            $(document).find("#Labor_AccountId").css("display", "none");
            $(document).find("#Labor_AccountId").val(data.data.Labor_AccountId);
            $(document).find("#AccountClientLookupId").css("display", "block");
            if (data.data.LaborAccountClientLookupId != "") {
                $(document).find(".ClearAccountModalPopupGridData").css("display", "block");
            }
            $(document).find("#AccountClientLookupId").val(data.data.LaborAccountClientLookupId);
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion