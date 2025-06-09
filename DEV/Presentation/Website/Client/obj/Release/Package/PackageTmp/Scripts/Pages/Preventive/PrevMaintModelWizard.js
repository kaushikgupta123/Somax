//#region V2-1204
$(document).on('click', '#addPrevMaintModel', function () {
    var prevMaintMasterId = $(this).data('id');
    $.ajax({
        url: "/PreventiveMaintenance/PrevMaintModelWizard",
        type: "POST",
        data: { 'PrevMaintMasterId': prevMaintMasterId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#addPrevMaintModelPopupDivContainer').html(data);
        },
        complete: function () {
            SetPreventiveControls();
            addPrevMaintModelWizard.init();
            $('#PrevMaintModelWizardModal').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        error: function () {
            CloseLoader();
        }
    });
});

var addPrevMaintModelWizard = function () {

    var wizardEl = $('#m_wizard');
    var formEl = $('#formPrevMaintModel');
    var wizard;

    var initWizard = function () {
        wizard = wizardEl.mWizard({
            startStep: 1
        });

        wizard.on('beforeNext', function (wizard) {
            if ($('.m-wizard__form-step--current').attr('id') == 'PrevMaintInformationStep') {
                var isvalid = formEl.valid();
                if (isvalid) {
                    return true;
                } else {
                    formEl.submit();
                    return false;
                }
            }
        });


        wizard.on('change', function (wizard) {

        });
    }

    return {
        // public functions
        init: function () {
            wizardEl = $('#m_wizard');
            formEl = $('#formPrevMaintModel');

            initWizard();
        }
    };
}();

function AddPrevMaintModelOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $('#PrevMaintModelWizardModal').modal('hide');
        SuccessAlertSetting.text = getResourceValue("AddPMAlerts");
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(data.PrevMaintMasterId)
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion