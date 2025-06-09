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
$(document).on('click', '#brdpm', function () {
    var MasterId = $(this).attr('data-val');
    RedirectToEMDetail(MasterId, "");
});
$(document).on('change', '#colorselector', function (evt) {
    $(document).find('.tabsArea').hide();
    openCity(evt, $(this).val());
    $('#' + $(this).val()).show();
});
//#region PM
function GeneratePMGrid() {
    var rCount = 0;
    var showBtn = false;
    var MasterId = $(document).find('#EquipmentMasterModel_EquipmentMasterId').val();
    if ($(document).find('#pmTable').hasClass('dataTable')) {
        dtPmTable.destroy();
    }
    dtPmTable = $("#pmTable").DataTable({
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
            "url": "/EquipmentMaster/PopulatePM",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.equipmentMasterId = MasterId
            },
            "dataSrc": function (response) {
                showBtn = response.showBtn;
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [4], render: function (a, b, data, d) {
                    if (showBtn) {
                        return '<a class="btn btn-outline-primary addpmBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editpmBttn gridinnerbutton" title= "Edit" style="display:none"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delpmBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return '';
                    }

                }
            }
        ],
        "columns":
        [
            { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            { "data": "Frequency", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "FrequencyType", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "bSortable": false, "className": "text-center" }
        ],
        initComplete: function () {
            if (rCount > 0 || showBtn == false) {
                $('#btnAddPreventive').hide();
            }
            else {
                $('#btnAddPreventive').show();
            }
            if (showBtn == false ) {
                var column = this.api().column(4);
                column.visible(false);
            }
            else {
                var column = this.api().column(4);
                column.visible(true);
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.delpmBttn', function () {
    var data = dtPmTable.row($(this).parents('tr')).data();
    var eQMaster_PMLibraryId = data.EQMaster_PMLibraryId;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/EquipmentMaster/DeletePm',
            data: {
                eQMaster_PMLibraryId: eQMaster_PMLibraryId
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "post",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    CloseLoader();
                    ShowDeleteAlert("Preventive Maintenance deleted successfully");
                }
            },
            complete: function () {
                dtPmTable.state.clear();
                GeneratePMGrid();
            }
        });
    });
});
$(document).on('click', ".addpmBttn,#btnAddPreventive", function () {
    var masterId = $(document).find('#EquipmentMasterModel_EquipmentMasterId').val();
    var name = $(document).find('#EquipmentMasterModel_Name').val();
    $.ajax({
        url: "/EquipmentMaster/AddPM",
        type: "GET",
        dataType: 'html',
        data: { eQMasterId: masterId, name: name },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderequipmentmaster').html(data);
        },
        complete: function () {
            SetEqpMasterControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function PmAddOnSuccess(data) {
    CloseLoader();
    var EQMasterId = data.EQMasterId;
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("PreventiveMaintenanceAddAlert");
        }       
        swal(SuccessAlertSetting, function () {
            RedirectToEMDetail(EQMasterId, "Preventive");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnPMcancel", function () {
    var EQMasterId = $(document).find('#EquipmentMasterPmModel_EQMasterId').val();
    RedirectToSureOncancel(EQMasterId, "Preventive");
});
$(document).on('click', '.editpmBttn', function () {
    var data = dtPmTable.row($(this).parents('tr')).data();
    EditPm(data);
});
function EditPm(data) {
    var eQMasterId = $(document).find('#EquipmentMasterModel_EquipmentMasterId').val();
    var name = $(document).find('#EquipmentMasterModel_Name').val();
    $.ajax({
        url: "/EquipmentMaster/EditPm",
        type: "GET",
        dataType: 'html',
        data: { eQMasterId: eQMasterId, name: name, eQMaster_PMLibraryId: data.EQMaster_PMLibraryId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderequipmentmaster').html(data);
        },
        complete: function () {
            SetEqpMasterControl();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
//#endregion PM