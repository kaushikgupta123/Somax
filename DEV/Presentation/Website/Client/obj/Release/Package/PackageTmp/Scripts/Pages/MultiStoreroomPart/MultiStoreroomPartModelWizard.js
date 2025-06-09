//#region V2-1203
$(document).on('click', '#addMultiStoreroomPartModel', function () {
    var partId = $(this).data('id');
    $.ajax({
        url: "/MultiStoreroomPart/MultiStoreroomPartModelWizard",
        type: "POST",
        data: { 'PartId': partId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#addMSPartModelPopupDivContainer').html(data);
        },
        complete: function () {
            SetMSPControls();
            addMSPartModelWizard.init();
            $('#MSPartModelWizardModal').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        error: function () {
            CloseLoader();
        }
    });
});

var addMSPartModelWizard = function () {

    var wizardEl = $('#m_wizard');
    var formEl = $('#formMSPartModel');
    var wizard;

    var initWizard = function () {
        wizard = wizardEl.mWizard({
            startStep: 1
        });

        wizard.on('beforeNext', function (wizard) {
            if ($('.m-wizard__form-step--current').attr('id') == 'MSPartInformationStep') {
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
            formEl = $('#formMSPartModel');

            initWizard();
        }
    };
}();

function AddMSPartModelOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $('#MSPartModelWizardModal').modal('hide');
        SuccessAlertSetting.text = getResourceValue("PartAddAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToMultiStoreroomPartDetail(data.PartId);
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