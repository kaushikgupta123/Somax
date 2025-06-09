Dropzone.autoDiscover = false;
var zoomConfig = {
    zoomType: "window",
    lensShape: "round",
    lensSize: 1000,
    zoomWindowFadeIn: 500,
    zoomWindowFadeOut: 500,
    lensFadeIn: 100,
    lensFadeOut: 100,
    easing: true,
    scrollZoom: true,
    zoomWindowWidth: 450,
    zoomWindowHeight: 450
};
$(function () {
    $(document).find('.select2picker').select2({});
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });   
    SetUIEnvironment();
    //V2-728 ***start
    var WOCompCriteriaTab = document.getElementById("woCompletionSettingsModel_WOCompCriteriaTab").checked;
    if (WOCompCriteriaTab) {
        $(document).find("#btnCriteriaSetup").prop('disabled', false)
    } else {
        $(document).find("#btnCriteriaSetup").prop('disabled', true); // Disable it.
    }
    //V2-728 **end
});
function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {       
        case "Passwordsettings":            
            $('#Passwordsettings').show();
            break;
        //V2-634
        case "CompletionSettings":
            $('#CompletionSettings').show();
            break;
        //V2-944
        case "FormConfigurationSettings":
            $('#FormConfigurationSettings').show();
            break;
        default:
            $('#Overview').show();
    }
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
}
$(document).on('click', "ul.vtabs li", function () {
    var id = $(this).attr('id');
    if (id != undefined && id == 'photot') {
        $(document).find('#btnSaveWO').hide();
        $(document).find('#btnSavePS').hide();
        $(document).find('#btnSaveWOCompletion').hide();
        $(document).find('#btnSaveWOFormConfiguration').hide();
    }
    else if (id != undefined && id == 'Passwordsetting')
    {
        $(document).find('#btnSaveWO').hide();
        $(document).find('#btnSavePS').show();
        $(document).find('#btnSaveWOCompletion').hide();
        $(document).find('#btnSaveWOFormConfiguration').hide();
    }
    //v2-634
    else if (id != undefined && id == 'CompletionSetting')
    {
        $(document).find('#btnSavePS').hide();
        $(document).find('#btnSaveWO').hide();
        $(document).find('#btnSaveWOCompletion').show();
        $(document).find('#btnSaveWOFormConfiguration').hide();
    }
    //v2-944
    else if (id != undefined && id == 'FormConfigurationSetting') {
        $(document).find('#btnSavePS').hide();
        $(document).find('#btnSaveWO').hide();
        $(document).find('#btnSaveWOCompletion').hide();
        $(document).find('#btnSaveWOFormConfiguration').show();
    }
    else
    {
        $(document).find('#btnSavePS').hide();
        $(document).find('#btnSaveWO').show();
        $(document).find('#btnSaveWOCompletion').hide();
        $(document).find('#btnSaveWOFormConfiguration').hide();
    }
    $("ul.vtabs li").removeClass("active");
    $(this).addClass("active");
    $(".tabsArea").hide();
    var activeTab = $(this).find("a").attr("href");
    $(activeTab).fadeIn();
    return false;
});
function SetUIEnvironment() {
    CloseLoader();
    if ($(document).find('#dropzoneForm').length > 0) {
        var myDropzone = new Dropzone("div#dropzoneForm", {
            url: "../Base/SaveUploadedFile",
            addRemoveLinks: true,
            paramName: 'file',
            maxFilesize: 10, // MB
            maxFiles: 4,
            dictDefaultMessage: 'Drag files here to upload, or click to select one',
            acceptedFiles: ".jpeg,.jpg,.png",
            init: function () {
                this.on("removedfile", function (file) {
                    if (file.type != 'image/jpeg' && file.type != 'image/jpg' && file.type != 'image/png') {
                        ShowErrorAlert(getResourceValue("spnValidImage"));
                    }
                });
            },
            autoProcessQueue: true,
            dictRemoveFile: "X",
            uploadMultiple: true,
            dictRemoveFileConfirmation: "Do you want to remove this file ?",
            dictCancelUpload: "X",
            parallelUploads: 1,
            dictMaxFilesExceeded: "You can not upload any more files.",
            success: function (file, response) {
                var imgName = response;
                file.previewElement.classList.add("dz-success");
                var radImage = '<a class="lnkContainer setImage" data-toggle="tooltip" title="Upload a selective Image!" data-image="' + file.name + '" style="cursor:pointer;"><i class="fa fa-upload" style="cursor:pointer;"></i></a>';
                $(file.previewElement).append(radImage);
            },
            error: function (file, response) {
                if (file.size > (1024 * 1024 * 10)) // not more than 10mb
                {
                    ShowImageSizeExceedAlert();
                }
                file.previewElement.classList.add("dz-error");
                var _this = this;
                _this.removeFile(file);
            }
        });
    }
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if ($(this).valid()) {
            if (typeof areaddescribedby !== 'undefined') {
                $('#' + areaddescribedby).hide();
            }
        }
        else {
            if (typeof areaddescribedby !== 'undefined') {
                $('#' + areaddescribedby).show();
            }
        }
    });
}
function ZoomImage(element, dataurl, noimage) {
    $.removeData(element, 'elevateZoom');
    $('.zoomContainer').remove();
    if (noimage == true) {
        element.data('zoom-image', "../Scripts/ImageZoom/images/NoImage.jpg").elevateZoom(zoomConfig);
    }
    else if (!dataurl) {
        element.elevateZoom(zoomConfig);
    }
    else {
        //element.data('zoom-image', "data:image/jpg;base64," + dataurl).elevateZoom(zoomConfig); 
        element.data('zoom-image', dataurl).elevateZoom(zoomConfig);
    }
}

$(document).on('click', '.setImage', function () {
    var imageName = $(this).data('image');
    var ClientId = $('#clientSetUpModel_ClientId').val();
    $.ajax({
        url: '../base/SaveUploadedFileToServer',
        type: 'POST',
        data: { 'fileName': imageName, objectId: ClientId, TableName: "Client", AttachObjectName: "Client" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {
            $('#EquipZoom').attr('src', data.imageurl);
            $('.equipImg').attr('src', data.imageurl);
            $(document).find('#AzureImage').append('<a href="javascript:void(0)" id="deleteImg" class="trashIcon" title="Delete"><i class="fa fa-trash"></i></a>');
            ZoomImage($('#EquipZoom'), data.imageurl);
            $("#EquipZoom").on('load', function () {
                CloseLoader();
                ShowImageSaveSuccessAlert();
            });
            }
            else {
                CloseLoader();
                var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");    
                ShowErrorAlert(errorMessage);

            }
        },
        error: function ()
        {
            CloseLoader();
        },
        complete: function () {
           // CloseLoader();
        }
    });
});
$(document).on('click', '#deleteImg', function () {

    var ClientOnPremise = $('#clientSetUpModel_ClientOnPremise').val();
    if (ClientOnPremise == 'True') {
        DeleteOnPremiseImage();
    }
    else {
        DeleteAzureImage();
    }
 
});
function DeleteAzureImage() {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/ClientSetUp/DeleteImageFromAzure',
            type: 'POST',
            data: {},
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data == "success" || data=="not found") {
                    $('#EquipZoom').attr('src', '../Scripts/ImageZoom/images/NoImage.jpg');
                    $('.imageDropZone').show();
                    $(document).find('#btnnblock').removeClass("col-xl-6");
                    $(document).find('#btnnblock').addClass("col-xl-12");
                    $(document).find('#photot').addClass("active");
                    ZoomImage($('#EquipZoom'), '', true);
                    $(document).find('#deleteImg').remove();
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function DeleteOnPremiseImage() {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/ClientSetUp/DeleteImageFromOnPremise',
            type: 'POST',
            data: {},
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data == "success" || data == "not found") {
                    $('#EquipZoom').attr('src', '../Scripts/ImageZoom/images/NoImage.jpg');
                    $('.imageDropZone').show();
                    $(document).find('#btnnblock').removeClass("col-xl-6");
                    $(document).find('#btnnblock').addClass("col-xl-12");
                    $(document).find('#photot').addClass("active");
                    ZoomImage($('#EquipZoom'), '', true);
                    $(document).find('#deleteImg').remove();
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function AddDeleteIcon() {
    if ($(document).find('#deleteImg').length == 0) {
        $(document).find('#AzureImage').append('<div id="imgdelcontailer"><a href="javascript:void(0)" id="deleteImg" class="trashIcon" title="Delete"><i class="fa fa-trash"></i></a></div>');
    }
};
function ClientUpdateAddOnSuccess(data) {
    CloseLoader();
    if (data.Result === "success") {      
        $(document).find('#UpdateIndex').val(data.UpdateIndex);
        SuccessAlertSetting.text = getResourceValue("ClientSetupUpdateAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToClientSearch();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function RedirectToClientSearch() {
    window.location.href = "/ClientSetUp/index?page=Company";
}

function PasswordSettingsOnSuccess (data) {
    CloseLoader();
    if (data.Result === "success") {
        $('#Passwordsettings').show();
        SuccessAlertSetting.text = getResourceValue("PasswordSettingSuccessAlert");
        swal(SuccessAlertSetting, function () {
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', '#PWReqMinLength', function (e) { 
    $(document).find('#passwordSettingsModel_PWMinLength').val('');
    var checked = this.checked;
    if (checked) {
        $('#passwordSettingsModel_PWMinLength').prop("readonly", false).removeClass('readonly');
        $(document).find("#MinLenPassReqId").css('display', 'Inline');     

    } else {
        $('#passwordSettingsModel_PWMinLength').prop("readonly", true).addClass('readonly');
        $(document).find("#MinLenPassReqId").css('display', 'none');       
    }
});


$(document).on('click', '#PWReqExpiration', function (e) {  
    $(document).find('#passwordSettingsModel_PWExpiresDays').val('');
    var checked = this.checked;
    if (checked) {      
        $('#passwordSettingsModel_PWExpiresDays').prop("readonly", false).removeClass('readonly');
        $(document).find("#PassAgeLimitReqId").css('display', 'Inline');

    } else {
        $('#passwordSettingsModel_PWExpiresDays').prop("readonly", true).addClass('readonly');       
        $(document).find("#PassAgeLimitReqId").css('display', 'none');
    }
});
//#region V2-634
function CompletionSettingsOnSuccess(data) {
    CloseLoader();
    if (data.Result === "success") {
        $('#CompletionSettings').show();
        SuccessAlertSetting.text = getResourceValue("WoCompletionConfigurationSettingSuccessAlert");
        swal(SuccessAlertSetting, function () {
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion

//#region V2-728****

$(document).on('click', '#woCompletionSettingsModel_WOCompCriteriaTab', function (e) {  
    var checked = this.checked;
    if (checked) {
        $(document).find("#btnCriteriaSetup").prop('disabled', false); //
    } else {
        $(document).find("#btnCriteriaSetup").prop('disabled', true); // Disable it.
    }
});

//*****Load Completion Criteria Popup with CkEditor*******
$(document).on('click', '#btnCriteriaSetup', function () { 
    if (!$(document).find('#CompletionCriteriaPopup').hasClass('show')) {
        $('#CompletionCriteriaEditor').html('');
      
        LoadCkEditorById('CompletionCriteriaEditor', function (editor) { });//***for ckeditor***
        var Criteria = $('#woCompletionCriteriaSetupModel_WOCompCriteria').val();

        $('#CompletionCriteriaEditor').html(Criteria);
        
        $(document).find('#CompletionCriteriaPopup').modal("show");       
    }
});
$(document).on('click', '.hideCompletionCriteria', function () {
    $(document).find('#CompletionCriteriaPopup').modal("hide");
    ClearEditorById('CompletionCriteriaEditor');
});
$('#FormWOCompletionCriteria').on('keyup keypress', function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
        e.preventDefault();
        return false;
    }
});
$("#btnCompletionCriteriaSave").click(function () {
    var CompletionCriteriaEditor = getDataFromEditorById('CompletionCriteriaEditor')
    $('#woCompletionCriteriaSetupModel_WOCompCriteria').val(CompletionCriteriaEditor);

    var WOCompCriteriaTab = $('#woCompletionSettingsModel_WOCompCriteriaTab').is(":checked");
    $('#woCompletionCriteriaSetupModel_WOCompCriteriaTab').val(WOCompCriteriaTab);

    var isvalid = $("#FormWOCompletionCriteria").valid();
   
    if (isvalid) {
        if (CompletionCriteriaEditor != "") {
            $(document).find('#CompletionCriteriaPopup').modal("hide");
            $("#FormWOCompletionCriteria").submit();
        }
        else (CompletionCriteriaEditor == "")
        {
            $(document).find('#CompletionCriteriaEditor').css('border', '2px solid #f39494');
        }
        
        return false;
    } else {
        if (CompletionCriteriaEditor == "") {
            $(document).find('#CompletionCriteriaEditor').css('border', '2px solid #f39494');
        }
        return false;
    }
    
});

function CompletionCriteriaSaveOnSuccess(data) {
    CloseLoader();
    if (data.Result === "success") {
        $('#CompletionSettings').show();
        SuccessAlertSetting.text = getResourceValue("CriteriaSetupUpdatedSuccessfully");//"Criteria Setup updated successfully"
        swal(SuccessAlertSetting, function () {
            //***
            var CompletionCriteriaEditor = getDataFromEditorById('CompletionCriteriaEditor')
            $('#woCompletionCriteriaSetupModel_WOCompCriteria').val(CompletionCriteriaEditor);

            ClearEditorById('CompletionCriteriaEditor');
            
            //***
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//****

//#endregion V2-728 end****

//#region V2-944
function FormConfigurationOnSuccess(data) {
    CloseLoader();
    if (data.Result === "success") {
        $('#FormConfigurationSettings').show();
        SuccessAlertSetting.text = getResourceValue("FormConfigurationSettingSuccessAlert");
        swal(SuccessAlertSetting, function () {
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion V2-944
// #region V2-945
function openCity(evt, cityName) {
    evt.preventDefault();
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent2");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
    if (cityName == "POFormSettings") {
        $(document).find('#woFormSettingsSetupModel_FileContent').removeAttr("accept");
        $(document).find('#woFormSettingsSetupModel_FileContent').attr("accept", "application/pdf");
        POTandCUploadSection();
    }
    else {
        $(document).find('#IsFileRequired').val(false);
    }
}
$(document).on('click', "#FormConfigurationSetting", function (e) {
    $(document).find('#btnwosettings').addClass('active');
    $(document).find('#WOFormSettings').show();
});
//#endregion
//#region V2-946
$(document).on('click', "#POTandC", function () {
    POTandCUploadSection();
});

function SetControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
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
    ZoomImage($(document).find('#EquipZoom'));
    $(document).find('.select2picker').select2({});
    $(document).find('.dtpicker:not(.readonly)').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    SetFixedHeadStyle();
}
function POTandCUploadSection() {
    SetControls();
    var TCURL = $(document).find('#POTandCURL').val();
    if ($("#POTandC").prop("checked")) {
        if (TCURL == "") {
            $(document).find('#IsFileRequired').val(true);
            $(document).find('#spnFileReq').css('display', 'block');
            $(document).find('#spnFileNotReq').css('display', 'none');
        }
        else {
            $(document).find('#IsFileRequired').val(false);
            $(document).find('#spnFileReq').css('display', 'none');
            $(document).find('#spnFileNotReq').css('display', 'block');
        }
        $(document).find('#liPOTCURL').show();
    }
    else {
        $(document).find('#IsFileRequired').val(false);
        $(document).find('#spnFileReq').css('display', 'none');
        $(document).find('#spnFileNotReq').css('display', 'block');
        $(document).find('#liPOTCURL').hide();
    }
    
}

$(document).on('submit', "#frmpoformconfig", function (e) {
    e.preventDefault();
    var form = document.querySelector('#frmpoformconfig');
    if (!$('#frmpoformconfig').valid()) {
        return;
    }
    var data = new FormData(form);
    $.ajax({
        type: "POST",
        url: "/ClientSetUp/UpdatePOFormConfiguration",
        data: data,
        processData: false,
        contentType: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                var ClientOnPremise = $('#clientSetUpModel_ClientOnPremise').val();
                $('#FormConfigurationSettings').show();
                SuccessAlertSetting.text = getResourceValue("FormConfigurationSettingSuccessAlert");
                swal(SuccessAlertSetting, function () {
                    $(document).find("#woFormSettingsSetupModel_FileContent").val('');
                    $(document).find('#POTandCURL').val(data.AttachURL);
                    if ( data.AttachURL != null && data.AttachURL != "") {
                        $(document).find("#POTandCFileName").text('');
                     
                     
                        if (ClientOnPremise == 'True') {
                            choosedFileName = data.AttachURL.replace(/\\/g, "/");
                            files = choosedFileName.split('/')
                            $(document).find("#POTandCFileName").text(files[files.length - 1]);
                        } else {
                            var arr = data.AttachURL.split('/');
                            var fileName = arr[arr.length - 1]
                            $(document).find("#POTandCFileName").text(fileName);
                        }
                     
                    }
                    POTandCUploadSection();
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function (xhr) {
            CloseLoader();
        }
    });
});
//#endregion
