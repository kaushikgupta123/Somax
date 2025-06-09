//#region V2-1203
$(document).on('click', '#addPartModel', function () {
    var partId = $(this).data('id');
    $.ajax({
        url: "/Parts/PartModelWizard",
        type: "POST",
        data: { 'PartId': partId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#addPartModelPopupDivContainer').html(data);
        },
        complete: function () {
            SetPartControls();
            addPartModelWizard.init();
            $('#PartModelWizardModal').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        error: function () {
            CloseLoader();
        }
    });
});

var addPartModelWizard = function () {

    var wizardEl = $('#m_wizard');
    var formEl = $('#formPartModel');
    var wizard;

    var initWizard = function () {
        wizard = wizardEl.mWizard({
            startStep: 1
        });

        wizard.on('beforeNext', function (wizard) {
            if ($('.m-wizard__form-step--current').attr('id') == 'PartInformationStep') {
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
            formEl = $('#formPartModel');

            initWizard();
        }
    };
}();

function AddPartModelOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $('#PartModelWizardModal').modal('hide');
        SuccessAlertSetting.text = getResourceValue("PartAddAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPartDetail(data.PartId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', '.close', function () {
    // If you use tooltips for errors, hide them
    $(document).find('[role="tooltip"]').css('display', 'none');
});
//#endregion