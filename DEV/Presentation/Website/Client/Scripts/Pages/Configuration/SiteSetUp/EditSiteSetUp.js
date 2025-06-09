Dropzone.autoDiscover = false;
$(function () {   
    $(document).find('.select2picker').select2({});
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
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
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
            openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
    SetWorkworderDetailEnvironment();
});
function openCity(evt, cityName) {
    if (cityName == "ApprovalGroupSettings") {
        $(document).find("#tabType").val("ApprovalGroupSettings");
    } else {
        $(document).find("#tabType").val("Other");
    }
    evt.preventDefault();
    switch (cityName) {
        case "BillTo":
            $('#BillTo').show();
            break;
        case "AddMaintenance":
            $('#AddMaintenance').show();
            break;
        case "AddPurchasing":
            $('#AddPurchasing').show();
            break;
        case "InvoiceMatch": //1061
            $('#InvoiceMatch').show();
            break;
        case "ShipTo":
            $('#ShipTo').show();
            break;
        case "AssetGroup1":
            GenerateAssetGroup1Grid();
            break;
        case "AssetGroup2":
            GenerateAssetGroup2Grid();
            break;
        case "AssetGroup3":
            GenerateAssetGroup3Grid();
            break;
        case "ApprovalGroupSettings"://V2-720
            $('#ApprovalGroupSettings').show();
            break;
      
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
$.validator.setDefaults({ ignore: null });
$(document).on('click', "#btnSaveSite", function () {
    if ($(document).find("form").valid()) {
        return;
    }
    else {
        var errorTab = $(".input-validation-error").parents('div:eq(0)').attr('id');
        if (errorTab === 'Overview') {
            $('#idOverview').trigger('click');
        }        
    }
});
$(document).on('click', "ul.vtabs li", function () {
    var id = $(this).attr('id');
    if (id != undefined && (id == 'photot' || id == 'idAssetGroup1' || id == 'idAssetGroup2' || id == 'idAssetGroup3')) {
        $(document).find('#btnSaveSite').hide();
        ZoomImage($(document).find('#EquipZoom'));
    } 
    else {
       $(document).find('#btnSaveSite').show();
    }
    if ($(this).find('#drpDwnLink').length > 0 || $(this).find('#drpDwnLink2').length > 0) {
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
function SetWorkworderDetailEnvironment() {
    $.validator.setDefaults({ ignore: null });
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
                if (file.size > (1024 * 1024 * 10)) 
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
    var SiteId = $('#SiteId').val();
    $.ajax({
        url: "../base/SaveUploadedFileToServer",
        type: 'POST',
        data: { 'fileName': imageName, objectId: SiteId, TableName: "Site", AttachObjectName: "Site" },
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
        }
    });
});

$(document).on('click', '#deleteImg', function () {  
    var ClientOnPremise = $('#ClientOnPremise').val();
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
            url: '/SiteSetup/DeleteImageFromAzure',
            type: 'POST',
            data: { },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data == "success" || data == "not found") {                   
                    $('#EquipZoom').attr('src', '../Scripts/ImageZoom/images/NoImage.jpg');
                    $('.tabcontent2').hide();
                    $('#auditlogcontainer').hide();
                    $('.imageDropZone').show();
                    $(document).find('#btnnblock').removeClass("col-xl-6");
                    $(document).find('#btnnblock').addClass("col-xl-12");
                    $(document).find('#workorderSidebar').removeClass("active");
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
            url: '/SiteSetup/DeleteImageFromOnPremise',
            type: 'POST',
            data: {},
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data == "success" || data == "not found") {
                    $('#EquipZoom').attr('src', '../Scripts/ImageZoom/images/NoImage.jpg');
                    $('.tabcontent2').hide();
                    $('#auditlogcontainer').hide();
                    $('.imageDropZone').show();
                    $(document).find('#btnnblock').removeClass("col-xl-6");
                    $(document).find('#btnnblock').addClass("col-xl-12");
                    $(document).find('#workorderSidebar').removeClass("active");
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
function SiteUpdateAddOnSuccess(data) {
    CloseLoader();
    if (data.Result === "success") {      
        SuccessAlertSetting.text = getResourceValue("SitesetupupdatedsuccessfullyAlert");         
        swal(SuccessAlertSetting, function () {  
            RedirectToSiteSearch();
            });        
    }
    else
    {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function RedirectToSiteSearch() {
    window.location.href = "/SiteSetUp/index?page=Site";
}