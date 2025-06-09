//#region Wizard
var WOCompletionWizard = function () {
    //== Base elements
    var wizardEl = $('#m_wizard');
    var formEl = $('#formCompleteDynamic');
    var wizard;

    //== Private functions
    var initWizard = function () {
        //== Initialize form wizard
        wizard = wizardEl.mWizard({
            startStep: 1
        });

        if ($('#WoCriteriacompleteChk').length > 0) {
            formEl.find('#wizardBtnnext').addClass('disabled');
        }
        //== Validation before going to next page
        wizard.on('beforeNext', function (wizard) {
            //if (validator.form() !== true) {
            //    return false;  // don't go to the next step
            //}
            if ($('.m-wizard__form-step--current').attr('id') == 'WoCompletionCriteriaStep') {
                
                if ($('#WoCriteriacompleteChk').is(':checked')) {
                    formEl.find('#wizardBtnnext').removeClass('disabled');
                        //formEl.submit();
                    } else {
                        return false;
                    }
                
            } else {
                var isvalid = formEl.valid();
                if (!isvalid) {
                    if (formEl.find('.select2picker').length > 0) {
                        formEl.submit();
                    }
                    return false;
                }
            }
            
            //}
            //$.validator.setDefaults({ ignore: null });
            //$.validator.unobtrusive.parse('#formCompleteDynamic');
            //$('#formCompleteDynamic').validate();

            $('#WorkOrderCompletionWizard_CompletionComments').val('');
            if ($('#CommentEditorFromWizard').length > 0) {
                //if (theEditor != null && theEditor != undefined) {
                commentUserList();
                var commentEditor = getDataFromEditorById('CommentEditorFromWizard')
                $('#WorkOrderCompletionWizard_CompletionComments').val(commentEditor);
                //}
            }

            $('#WorkOrderCompletionWizard_WOLaborsString').val('');
            if ($('#LaborWizardTable').length > 0) {
                if (dtLaborWizardTable != null && dtLaborWizardTable != undefined && dtLaborWizardTable.rows().data().length > 0) {
                    $('#WorkOrderCompletionWizard_WOLaborsString').val(JSON.stringify(dtLaborWizardTable.rows().data().toArray()));
                }
            }

        });

        //== Change event
        wizard.on('change', function (wizard) {
            //mApp.scrollTop();
        });
    }

    return {
        // public functions
        init: function () {
            wizardEl = $('#m_wizard');
            formEl = $('#formCompleteDynamic');

            initWizard();
        }
    };
}();
//#endregion Wizard

//#region Labor
var dtLaborWizardTable;
function generateLaborWizardTable() {
    var visibility = ActualLaborSecurityForWizard;
    if ($(document).find('#LaborWizardTable').hasClass('dataTable')) {
        dtLaborWizardTable.destroy();
    }
    dtLaborWizardTable = $("#LaborWizardTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        //serverSide: true,
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
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    if (visibility == "True") {
                        return '<a class="btn btn-outline-primary addLaborBttnWizard gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editLaborBttnWizard gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delLaborBttnWizard gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return '';
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "PersonnelClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "NameFull", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "StartDate", "type": "date " },
                { "data": "Hours", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "TCValue", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "className": "text-center", "bSortable": false }
            ],
        initComplete: function () {
            if (visibility == "False") {
                var column = this.api().column(4);
                column.visible(false);
            }
            else {
                var column = this.api().column(4);
                column.visible(true);
            }
            if (visibility == "True") {
                $("#AddWizardLaborbtn").show();
            }
            else {
                $("#AddWizardLaborbtn").hide();
            }
            SetPageLengthMenu();
        }
    });
}
var editIndex = -1;
$(document).on('click', '.btnWizardLaborcancel', function () {
    editIndex = -1;
});
$(document).on('hidden.bs.modal', '#WorkorderCompletionLaborModal', function (e) {
    $('#WorkorderCompletionLaborDiv').html('');
});
$(document).on('click', "#AddWizardLaborbtn , .addLaborBttnWizard", function (e) {
    editIndex = -1;
    $.ajax({
        url: "/WorkOrder/AddLaborFromCompletionWizard",
        type: "POST",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#WorkorderCompletionLaborDiv').html(data);
        },
        complete: function () {
            SetControls();
            //if (globalPageType == "CompletionWorkbench") {
            //    Labordtpickerdate();
            //}
            Labordtpickerdate();
            $('#WorkorderCompletionLaborModal').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        error: function () {
            CloseLoader();
        }
    });
});
function CompletionLaborAddOnSuccess(data) {
    var LaborData = {
        "PersonnelID": $('#CompletionLaborWizard_PersonnelID').val(),
        "StartDate": $('#CompletionLaborWizard_StartDate').val(),
        "Hours": $('#CompletionLaborWizard_Hours').val(),
        "PersonnelClientLookupId": data.ClientLookupId,
        "NameFull": data.FullName,
        "TCValue": data.Value
    };
    if (editIndex == -1) {
        dtLaborWizardTable.row.add(LaborData).draw();
    }
    else {
        dtLaborWizardTable.row(editIndex).data(LaborData).draw();
        editIndex = -1;
    }

    if (dtLaborWizardTable.rows().data().length == 0) {
        $("#AddWizardLaborbtn").show();
    }
    else {
        $("#AddWizardLaborbtn").hide();
    }
    $('#WorkorderCompletionLaborModal').modal('hide');
}
$(document).on('click', '.editLaborBttnWizard', function () {
    var data = dtLaborWizardTable.row($(this).parents('tr')).data();
    $.ajax({
        url: "/WorkOrder/EditLaborFromCompletionWizard",
        type: "POST",
        dataType: 'html',
        data: { PersonnelID: data.PersonnelID, Hours: data.Hours, StartDate: data.StartDate },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            editIndex = dtLaborWizardTable.row($(this).parents('tr')).index();
            $('#WorkorderCompletionLaborDiv').html(data);
        },
        complete: function () {
            SetControls();
            //if (globalPageType == "CompletionWorkbench") {
            //    Labordtpickerdate();
            //}
            Labordtpickerdate();
            $('#WorkorderCompletionLaborModal').modal('show');
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delLaborBttnWizard', function () {
    swal(CancelAlertSetting, function () {
        var idx = dtLaborWizardTable.row($(this).parents('tr')).index();
        dtLaborWizardTable.row(idx).remove().draw();
        if (dtLaborWizardTable.rows().data().length == 0) {
            $("#AddWizardLaborbtn").show();
        }
        else {
            $("#AddWizardLaborbtn").hide();
        }
    });
});
//#endregion

//#region Work Order completion Wizard
$(document).on('click', '#actionCompleteWOWizard, #btnCompleteDetailWizard', function () {
    var item = new PartNotInInventorySelectedItem($(document).find('#workOrderModel_WorkOrderId').val(), $(this).data('clientlookupid'), $(this).data('status'));
    LoadWorkOrderCompletionWizardModal($('#WorkorderCompletionWizardDiv'), [item], "WorkOrderDetails");
});
$(document).on('click', '.wobtngrdcompleteWizard', function () {
    var item = new PartNotInInventorySelectedItem($(this).find('span').attr('id'), $(this).data('clientlookupid'), $(this).data('status'));
    LoadWorkOrderCompletionWizardModal($('#WorkorderCompletionWizardDiv'), [item], "WorkOrderSearchSingleCardView");
});

$(document).on('click', '#CompleteWoCheckListWizard', function () {
    if (SelectedWoIdToCancel.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var found = false;
        for (var i = 0; i < SelectedWoIdToCancel.length; i++) {
            if (SelectedWoIdToCancel[i].Status === 'Approved' || SelectedWoIdToCancel[i].Status === 'Scheduled') /*SelectedWoIdToCancel[i].Status === 'WorkRequest' v2-313*/ {
                found = true;
                break;
            }

        }
        if (found === false) {
            var errorMessage = getResourceValue("WorkOrderBatchCompletionAlert");
            ShowErrorAlert(errorMessage);
            return false;
        }
        else {
            LoadWorkOrderCompletionWizardModal($('#WorkorderCompletionWizardDiv'), SelectedWoIdToCancel, "WorkOrderSearchMultiple");
        }
    }
});
var globalPageType = "";
function LoadWorkOrderCompletionWizardModal(divElement, WorkOrderIds, PageType) {
    $.ajax({
        url: "/WorkOrder/CompleteWorkOrderFromWizard",
        type: "POST",
        data: { WorkOrderIds: WorkOrderIds },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            divElement.html(data);
        },
        complete: function () {
            SetControls();
            WOCompletionWizard.init();
            if ($('#LaborWizardTable').length > 0) {
                generateLaborWizardTable();
            }
            if ($('#CommentEditorFromWizard').length > 0) {
                LoadCkEditorById('CommentEditorFromWizard', function (editor) { });
            }
            $('#WorkOrderCompletionWizardModal').modal({ backdrop: 'static', keyboard: false, show: true });
            globalPageType = PageType;
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('hidden.bs.modal', '#WorkOrderCompletionWizardModal', function (e) {
    if ($('#CommentEditorFromWizard').length > 0) {
        ClearEditorById('CommentEditorFromWizard');
    }
    $('#WorkorderCompletionWizardDiv').html('');
});
function WorkOrderDynamicCompleteOnSuccess(data) {
    if (globalPageType == "WorkOrderSearchMultiple") {
        if (data.data == "success") {
            SuccessAlertSetting.text = getResourceValue("WorkOrderCompleteAlert");
            swal(SuccessAlertSetting, function () { });
        }
        else {
            GenericSweetAlertMethod(data);
        }
        if (layoutType !== 2) {
            ShowCardView();
        }
        SelectedWoIdToCancel = [];
        WoIdListToComplete = [];
        $(".updateArea").hide();
        $(".actionBar").fadeIn();
        $(document).find('.chksearch').prop('checked', false);
        $(document).find('.itemcount').text(0);
        pageno = workOrdersSearchdt.page.info().page;
        workOrdersSearchdt.page(pageno).draw('page');

        SelectedWoIdToCancel = [];
        WoIdListToComplete = [];
    }
    else if (globalPageType == "WorkOrderSearchSingleCardView") {
        if (data.data == "success") {
            SuccessAlertSetting.text = getResourceValue("WorkOrderCompleteAlert");
            swal(SuccessAlertSetting, function () {
                if (layoutType !== 2) {
                    ShowCardView();
                }
            });
        }
        else {
            GenericSweetAlertMethod(data);
        }
    }
    else if (globalPageType == "WorkOrderDetails") {
        if (data.data == "success") {
            SuccessAlertSetting.text = getResourceValue("WorkOrderCompleteAlert");
            swal(SuccessAlertSetting, function () {
                RedirectToPmDetail($(document).find('#workOrderModel_WorkOrderId').val(), "overview");
            });
        }
        else {
            GenericSweetAlertMethod(data);
        }
    }
    else if (globalPageType == "CompletionWorkbench") {
        if (data.data == "success") {
            var WorkOrderId = $(document).find('#WorkOrderId').val();
            SuccessAlertSetting.text = getResourceValue("WorkOrderCompleteAlert");
            swal(SuccessAlertSetting, function () {
                RedirectToMaintenanceWorkBenchDetail(WorkOrderId, "");
            });
        }
        else {
            GenericSweetAlertMethod(data);
        }
    }
    $(document).find('#WorkOrderCompletionWizardModal').modal('hide');
    globalPageType = "";
}
//#endregion

//#region Dashboard WorkOrder Completion Wizard
$(document).on('click', '#CompleteWoCheckListWizard_Workbench', function () {
    var WorkOrderId = $(document).find('#WorkOrderId').val();
    var workstatus = $(this).data("workbench_wostatus");
    var WorkOrder_ClientLookupId = $(this).data("workbench_workorder_clientlookupid");
    var item = new DashboardWorkBenchInSelectedItem(WorkOrderId, WorkOrder_ClientLookupId, workstatus);
    LoadWorkOrderCompletionWizardModal($('#WorkorderCompletionWizardDiv'), [item], "CompletionWorkbench");
});
function DashboardWorkBenchInSelectedItem(WorkOrderId, ClientLookupId, Status) {
    this.WorkOrderId = WorkOrderId;
    this.ClientLookupId = ClientLookupId;
    this.Status = Status;
};
function Labordtpickerdate() {
    $(document).find('#CompletionLaborWizard_StartDate').datepicker({
        endDate: "today",
        maxDate: "today",
        changeMonth: true,
        changeYear: true,
     /*   beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },*/
        "dateFormat": "mm/dd/yy",
        autoclose: true,
        onSelect: function (dateText) {
            ValidatePast30DaysDateFromDatePicker(this.value, getResourceValue("ValidatePast30DaysDateFromDatePickerAlert"));
            $(document).find('#CompletionLaborWizard_StartDate').removeClass('input-validation-error').css("display", "block");
        }
    }).inputmask('mm/dd/yyyy');
}
function commentUserList() {
    $('#WorkOrderCompletionWizard_CommentUserIds').val('');
    if ($('#CommentEditorFromWizard').length > 0) {
        var selectedUsers = [];
        $(document).find('.ck-content').find('.mention').each(function (item, index) {
            selectedUsers.push($(index).data('user-id'));
        });
        if (selectedUsers.length > 0) {
            $('#WorkOrderCompletionWizard_CommentUserIds').val(selectedUsers.toString());
        }
    }
}
//#endregion

//#region V2-728
$(document).on('click', '#WoCriteriacompleteChk', function (e) {
    var checked = this.checked;
    if (checked) {
        $("#formCompleteDynamic").find('#wizardBtnnext').removeClass('disabled');
    } else {
        $("#formCompleteDynamic").find('#wizardBtnnext').addClass('disabled');
    }
});
//#endregion

