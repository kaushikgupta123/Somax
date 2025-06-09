//#region Details
$(document).ready(function () {
    var IsRedirectFromWorkorder = $(document).find("#IsRedirectFromWorkorder").val();
    if (IsRedirectFromWorkorder == 'True') {
        SetSanitationDetailEnvironment();
        SetFixedHeadStyle();
        $.validator.setDefaults({ ignore: null });
        $.validator.unobtrusive.parse(document);
    }
});
$(document).on('click', '.lnk_sanitationjob_mobile', function (e) {
    e.preventDefault();
    var SanitationJobId = $(this).attr('id');
    $.ajax({
        url: "/SanitationJob/SJobDetails_Mobile",
        type: "POST",
        data: { SanitationJobId: SanitationJobId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
            //$(document).find('#spnlinkToSearch').text(titleText);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("sanjobstatustext"));
        },
        complete: function () {
            SetSanitationDetailEnvironment();
            SetFixedHeadStyle();
        },
        error: function () {
            CloseLoader();
        }
    });

});
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../SanitationJob/Index?page=Sanitation_Jobs_Search";
});
function ZoomImage(element) {
    element.elevateZoom(ZoomConfig);
}
function SetSanitationDetailEnvironment() {
    CloseLoader();
    ZoomImage($(document).find('#EquipZoom'));

    SetFixedHeadStyle();
    $('#tabscroll').mobiscroll().nav({
        type: 'tab'
    });
}
$(document).on('click', '.sjmoreaddescription', function () {
    $(document).find('#sanitationdetaildesmodaltext').text($(this).data("des"));
    $(document).find('#sanitationdetaildesmodal').addClass('slide-active').trigger('mbsc-enhance');
});
var tab = "Overview";
function RedirectToSaDetail(SanitationJobId, mode) {
    $.ajax({
        url: "/SanitationJob/SJobDetails_Mobile",
        type: "POST",
        data: { SanitationJobId: SanitationJobId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
            //$(document).find('#spnlinkToSearch').text(titleText);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("sanjobstatustext"));
        },
        complete: function () {
            CloseLoader();

            $(document).find(".m-portlet").find("button.active").removeClass('active');
            $(document).find(".m-portlet").find("[data-tab='" + tab + "']").addClass('active');
            $(document).find("#" + tab).css("display", "block");
            if (tab != 'Overview') {
                ResetOtherTabs();
                loadTabDetails(tab);
            }
            SetSanitationDetailEnvironment();
            SetFixedHeadStyle();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '.wo-det-tab', function (e) {
    if ($(this).hasClass('active')) {
        return false;
    }
    ResetOtherTabs();
    tab = $(this).data('tab');
    loadTabDetails(tab);
    SwitchTab(e, tab);
});
function loadTabDetails(tab) {
    switch (tab) {
        case "Overview":
            LoadOverviewTab();
            break;
        case "Comments":
            LoadCommentsTab();
            break;
        case "Photos":
            LoadPhotosTab();
            break;
        case "SJTools":
            LoadSJToolsTab();
            break;
        case "SJChemicalSupplies":
            LoadSJChemicalSuppliesTab();
            break;
        case "SJAssignments":
            LoadSJAssignmentsTab();
            break;
        case "SJTask":
            LoadSJTaskTab();
            break;
        case "SJLabor":
            LoadSJLaborTab();
            break;
        case "SJNotes":
            LoadSJNotesTab();
            break;
    }
}
function SwitchTab(evt, tab) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(tab).style.display = "block";

    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
    $(document).find('.wo-det-tab').removeClass('mbsc-ms-item-sel');
    $(document).find('#' + tab).addClass('mbsc-ms-item-sel');
}
function ResetOtherTabs() {
    $(document).find('#Overview').html('');
    $(document).find('#Photos').html('');
    $(document).find('#SJTools').html('');
    $(document).find('#SJChemicalSupplies').html('');
    $(document).find('#SJAssignments').html('');
}
//#region Overview
function LoadOverviewTab() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    $.ajax({
        url: "/SanitationJob/SJobDetails_Mobile",
        type: "POST",
        data: { SanitationJobId: SanitationJobId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitation').html(data);
            //$(document).find('#spnlinkToSearch').text(titleText);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("sanjobstatustext"));
        },
        complete: function () {
            SetSanitationDetailEnvironment();
            SetFixedHeadStyle();
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion
//#region Photo
//#region Multiple Photo Upload
function CompressImage(files, imageName) {
    new Compressor(files, {
        quality: 0.6,
        convertTypes: ['image/png'],
        convertSize: 100000,
        // The compression process is asynchronous,
        // which means you have to access the `result` in the `success` hook function.
        success(result) {
            if (result.size < files.size) {
                SaveCompressedImage(result, imageName);
            }
            else {
                SaveCompressedImage(files, imageName);
            }
            console.log('file name ' + result.name + ' after compress ' + result.size);
        },
        error(err) {
            console.log(err.message);
        },
    });
}
function SaveCompressedImage(data, imageName) {
    var AddPhotoFileData = new FormData();
    AddPhotoFileData.append('file', data, imageName);
    $.ajax({
        url: '../base/SaveUploadedFile',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: AddPhotoFileData,
        success: function (result) {
            SaveMultipleUploadedFileToServer(imageName);

            $('#files').val('');
            $('#add_photos').val('');
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}
$(document).on('change', '#files', function () {
    var val = $(this).val();
    var _isMobile = CheckLoggedInFromMob();
    var imageName = val.replace(/^.*[\\\/]/, '')
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var imgname = SanitationJobId + "_" + Math.floor((new Date()).getTime() / 1000);
    var fileUpload = $("#files").get(0);
    var files = fileUpload.files;
    var fileExt = imageName.substr(imageName.lastIndexOf('.') + 1).toLowerCase();
    if (fileExt != 'jpeg' && fileExt != 'jpg' && fileExt != 'png' && fileExt != 'JPEG' && fileExt != 'JPG' && fileExt != 'PNG') {
        ShowErrorAlert(getResourceValue("spnValidImage"));
        $('#files').val('');
        //e.preventDefault();
        return false;
    }
    else if (this.files[0].size > (1024 * 1024 * 10)) {
        ShowImageSizeExceedAlert();
        $('#files').val('');
        //e.preventDefault();
        return false;
    }
    else {
        swal(AddImageAlertSetting, function () {
            if (window.FormData !== undefined) {
                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    console.log('file name ' + files[i].name + ' before compress ' + files[i].size);
                    if (_isMobile == true) {
                        CompressImage(files[i], imgname + "." + fileExt);
                    }
                    else {
                        CompressImage(files[i], imageName);
                    }

                }
            } else {
                //alert("FormData is not supported.");
            }

        });
    }
});

$(document).on('change', '#add_photos', function () {
    var val = $(this).val();
    var imageName = val.replace(/^.*[\\\/]/, '');
    //image name set
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var imgname = SanitationJobId + "_" + Math.floor((new Date()).getTime() / 1000);
    //
    var fileUpload = $("#add_photos").get(0);
    var files = fileUpload.files;
    var fileExt = imageName.substr(imageName.lastIndexOf('.') + 1);
    if (fileExt != 'jpeg' && fileExt != 'jpg' && fileExt != 'png' && fileExt != 'JPEG' && fileExt != 'JPG' && fileExt != 'PNG') {
        ShowErrorAlert(getResourceValue("spnValidImage"));
        $('#add_photos').val('');
        //e.preventDefault();
        return false;
    }
    else if (this.files[0].size > (1024 * 1024 * 10)) {
        ShowImageSizeExceedAlert();
        $('#add_photos').val('');
        //e.preventDefault();
        return false;
    }
    //var duplicate_chk = SaveUploadedFileToServer(WorkOrderId, imageName);
    //if () { }
    else {
        //alert('Hello');
        swal(AddImageAlertSetting, function () {
            if (window.FormData !== undefined) {

                // Create FormData object  
                // var fileData = new FormData();

                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    //fileData.append(imgname + "." + fileExt, files[i]);
                    CompressImage(files[i], imgname + "." + fileExt);
                }
            }
            else {

            }
        });
    }
});
//#endregion

//#region Show Images
var imgcardviewstartvalue = 0;
var imgcardviewlwngth = 10;
var imggrdcardcurrentpage = 1;
var imgcurrentorderedcolumn = 1;
var layoutTypeWO = 1;
function LoadPhotosTab() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    $.ajax({
        url: '/SanitationJob/LoadPhotos_Mobile',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Photos').html(data);
        },
        complete: function () {
            LoadImages(SanitationJobId);
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function LoadImages(SanitationJobId) {
    $.ajax({
        url: '/SanitationJob/GetImages_Mobile',
        type: 'POST',
        data: {
            currentpage: imggrdcardcurrentpage,
            start: imgcardviewstartvalue,
            length: imgcardviewlwngth,
            SanitationJobId: SanitationJobId
        },
        beforeSend: function () {
            $(document).find('#imagedataloader').show();
        },
        success: function (data) {
            $(document).find('#ImageGrid').show();
            $(document).find('#ObjectImages').html(data).show();
            $(document).find('#tblimages_paginate li').each(function (index, value) {
                $(this).removeClass('active');
                if ($(this).data('currentpage') == imggrdcardcurrentpage) {
                    $(this).addClass('active');
                }
            });
        },
        complete: function () {
            $(document).find('#imagedataloader').hide();
            $(document).find('#imgcardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(imgcardviewlwngth).trigger('change.select2');
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('change', '#imgcardviewpagelengthdrp', function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    imgcardviewlwngth = $(this).val();
    imggrdcardcurrentpage = parseInt(imgcardviewstartvalue / imgcardviewlwngth) + 1;
    imgcardviewstartvalue = parseInt((imggrdcardcurrentpage - 1) * imgcardviewlwngth) + 1;
    LoadImages(SanitationJobId);

});
$(document).on('click', '#tblimages_paginate .paginate_button', function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    if (layoutTypeWO == 1) {
        var currentselectedpage = parseInt($(document).find('#tblimages_paginate .pagination').find('.active').text());
        imgcardviewlwngth = $(document).find('#imgcardviewpagelengthdrp').val();
        imgcardviewstartvalue = imgcardviewlwngth * (parseInt($(this).find('.page-link').text()) - 1);
        var lastpage = parseInt($(this).prev('li').data('currentpage'));

        if ($(this).attr('id') == 'tbl_previous') {
            if (currentselectedpage == 1) {
                return false;
            }
            imgcardviewstartvalue = imgcardviewlwngth * (currentselectedpage - 2);
            imggrdcardcurrentpage = imggrdcardcurrentpage - 1;
        }
        else if ($(this).attr('id') == 'tbl_next') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            imgcardviewstartvalue = imgcardviewlwngth * (currentselectedpage);
            imggrdcardcurrentpage = imggrdcardcurrentpage + 1;
        }
        else if ($(this).attr('id') == 'tbl_first') {
            if (currentselectedpage == 1) {
                return false;
            }
            imggrdcardcurrentpage = 1;
            imgcardviewstartvalue = 0;
        }
        else if ($(this).attr('id') == 'tbl_last') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            imggrdcardcurrentpage = parseInt($(this).prevAll('li').eq(1).text());
            imgcardviewstartvalue = imgcardviewlwngth * (imggrdcardcurrentpage - 1);
        }
        else {
            imggrdcardcurrentpage = $(this).data('currentpage');
        }
        LoadImages(SanitationJobId);

    }
    else {
        run = true;
    }
});
$(document).on('click', ".openPictureOptions", function (e) {
    var AttachmentId = $(this).attr('dataid');
    var AttachmentURL = $(this).attr('dataurl');
    $("#imgAttachmentId").val(AttachmentId);
    $("#imgAttachmentURL").val(AttachmentURL);
    $('#OpenImgActionPopup').addClass('slide-active').trigger('mbsc-enhance');
});
$(document).on('click', ".actionpopupmobileback", function (e) {
    $('#OpenImgActionPopup').removeClass('slide-active');
});
$(document).on('click', ".selectidOpen", function (e) {
    var AttachmentURL = $("#imgAttachmentURL").val();
    $("#SelectedImg").attr('src', AttachmentURL);
    $('#ShowImgPopup').addClass('slide-active').trigger('mbsc-enhance');
});
$(document).on('click', ".openimgback", function (e) {
    $("#SelectedImg").attr('src', '');
    $('#ShowImgPopup').removeClass('slide-active');
});
//#endregion
//#region Set As Default
$(document).on('click', '#selectidSetAsDefault', function () {
    var AttachmentId = $("#imgAttachmentId").val();
    $('#OpenImgActionPopup').removeClass('slide-active');
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    $.ajax({
        url: '../base/SetImageAsDefault',
        type: 'POST',

        data: { AttachmentId: AttachmentId, objectId: SanitationJobId, TableName: "Sanitation" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result === "success") {
                $('#EquipZoom').attr('src', data.imageurl);
                $('#EquipZoom').attr('data-zoom-image', data.imageurl);
                $('.equipImg').attr('src', data.imageurl);
                $('#EquipZoom').data('zoomImage', data.imageurl).elevateZoom(
                    {
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
                    });
                $("#EquipZoom").on('load', function () {
                    CloseLoader();
                    ShowImageSetSuccessAlert();
                });
            }
        },
        complete: function () {
            //CloseLoader();
            LoadImages(SanitationJobId);
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion
//#region Delete Image
$(document).on('click', '#selectidDelete', function () {
    var AttachmentId = $("#imgAttachmentId").val();
    $('#' + AttachmentId).hide();
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientOnPremise = $('#JobDetailsModel_ClientOnPremise').val();
    if (ClientOnPremise == 'True') {
        DeleteOnPremiseMultipleImage(SanitationJobId, AttachmentId);
    }
    else {
        DeleteAzureMultipleImage(SanitationJobId, AttachmentId);
    }
});

function DeleteOnPremiseMultipleImage(SanitationJobId, AttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '../base/DeleteMultipleImageFromOnPremise',
            type: 'POST',
            data: { AttachmentId: AttachmentId, objectId: SanitationJobId, TableName: "Sanitation" },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    $('#OpenImgActionPopup').removeClass('slide-active');
                    //LoadImages(PartId);
                    RedirectToSaDetail(SanitationJobId);
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function DeleteAzureMultipleImage(SanitationJobId, AttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '../base/DeleteMultipleImageFromAzure',
            type: 'POST',
            data: { AttachmentId: AttachmentId, objectId: SanitationJobId, TableName: "Sanitation" },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToSaDetail(SanitationJobId);
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
//#endregion
//#region Save Multiple Image
function SaveMultipleUploadedFileToServer(imageName) {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    $.ajax({
        url: '../base/SaveMultipleUploadedFileToServer',
        type: 'POST',

        data: { 'fileName': imageName, objectId: SanitationJobId, TableName: "Sanitation", AttachObjectName: "Sanitation" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {

                CloseLoader();
                ShowImageSaveSuccessAlert();
            }
            else if (data.result == "1") {
                CloseLoader();
                var errorMessage = getResourceValue("ImageExistAlert");
                ShowErrorAlert(errorMessage);

            }
            else {
                CloseLoader();
                var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
                ShowErrorAlert(errorMessage);

            }
        },
        complete: function () {
            LoadImages(SanitationJobId);
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion
//#endregion
//#region Tool
var SJToolsTable;
function LoadSJToolsTab() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    $.ajax({
        url: '/SanitationJob/LoadSJTools_Mobile',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#SJTools').html(data);
        },
        complete: function () {
            generateSJToolsGrid();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function generateSJToolsGrid() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var rCount = 0;

    var IsAddOrEdit = false;
    var IsDelelte = false;
    if ($(document).find('#SJToolsTable').hasClass('dataTable')) {
        SJToolsTable.destroy();
    }
    SJToolsTable = $("#SJToolsTable").DataTable({
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
            "url": "/SanitationJob/PopulateTools?SanitationJobId=" + SanitationJobId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                IsAddOrEdit = response.IsAddOrEdit;
                IsDelelte = response.IsDelelte;
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {

                    if (IsAddOrEdit && IsDelelte) {
                        return '<a class="btn btn-outline-success editToolsBttn gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delToolsBttn gridinnerbutton" title="Delete"><i class="fa fa-trash"></i></a>';
                    } else if (IsDelelte) {
                        return '<a class="btn btn-outline-danger delToolsBttn gridinnerbutton" title="Delete"><i class="fa fa-trash"></i></a>';
                    }
                    else if (IsAddOrEdit) {
                        return '<a class="btn btn-outline-success editToolsBttn gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "CategoryValue", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "Instructions"
                },
                {
                    "data": "Quantity", defaultContent: "", "bSearchable": true, "bSortable": true
                },
                {
                    "data": "SanitationPlanningId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            if (IsAddOrEdit) {
                $("#btnAddSJTools").show();
            }
            else {
                $("#btnAddSJTools").hide();
            }
        }
    });
}
$(document).on('click', '.addToolsBttn,#btnAddSJTools', function () {
    AddTools();
});
function AddTools() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    $.ajax({
        url: "/SanitationJob/AddTools_Mobile",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: SanitationJobId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#SJToolPopUp').html(data);
        },
        complete: function () {
            SetSJControls();
            BindMobiscrollControlForSJToolTab();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SetSJControls() {
    var errClass = 'mobile-validation-error';
    CloseLoader();
    $.validator.setDefaults({
        ignore: null,
        //errorClass: "mobile-validation-error", // default values is input-validation-error
        //validClass: "valid", // default values is valid
        highlight: function (element, errorClass, validClass) { //for the elements having error
            $(element).addClass(errClass).removeClass(validClass);
            $(element.form).find("#" + element.id).parent().parent().addClass("mbsc-err");
            var elemName = $(element.form).find("#" + element.id).attr('name');
            $(element.form).find('[data-valmsg-for="' + elemName + '"]').addClass("mbsc-err-msg");
        },
        unhighlight: function (element, errorClass, validClass) { //for the elements having not any error
            $(element).removeClass(errClass).addClass(validClass);
            $(element.form).find("#" + element.id).parent().parent().removeClass("mbsc-err");
            var elemName = $(element.form).find("#" + element.id).attr('name');
            $(element.form).find('[data-valmsg-for="' + elemName + '"]').removeClass("mbsc-err-msg");
        },
    });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        if ($(this).closest('form').length > 0) {
            $(this).valid();
        }
    });
    //$('.mobiscrollselect, form').change(function () {
    //    debugger;
    //    $(this).find('label.mbsc-err').removeClass('mbsc-err');
    //    if ($(this).valid() == false) {
    //        $(this).find('.input-validation-error').each(function () {
    //            $(this).parents('label').eq(0).addClass('mbsc-err');
    //        });
    //    }
    //});
    //$('.select2picker, form').change(function () {
    //$('form').change(function () {
    //    $(this).valid();
    //});
    $(document).find('.select2picker').select2({});
    $(document).find('.mobiscrollselect:not(:disabled)').mobiscroll().select({
        display: 'bubble',
        //touchUi: false,
        /*data: remoteData,*/
        filter: true,
        group: {
            groupWheel: false,
            header: false
        },
        //width: 400,
        //placeholder: 'Please Select...'

    });
    $(document).find('.mobiscrollselect:disabled').mobiscroll().select({
        disabled: true
    });
    SetFixedHeadStyle();
}
function SJToolsAddOnSuccess(data) {
    CloseLoader();
    var SanitationJobId = data.SanitationJobId;
    if (data.data == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("ToolAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("ToolUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToSaDetail(SanitationJobId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function BindMobiscrollControlForSJToolTab() {
    $('#AddSJToolModalpopup').parent().addClass("slide-active").trigger('mbsc-enhance');
    $('#toolModel_Quantity').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 999999999.999999,
        //scale: 2,
        maxScale: 6,
        entryMode: 'freeform',
        preset: 'decimal',
        thousandsSeparator: ''
    });
    var x = parseFloat($('#toolModel_Quantity').val()) == 0 ? '' : $('#toolModel_Quantity').val();
    $('#toolModel_Quantity').mobiscroll('setVal', x);
}
$(document).on('click', '.editToolsBttn', function () {
    var data = SJToolsTable.row($(this).parents('tr')).data();
    EditTools(data);
});
function EditTools(fullRecord) {
    var sanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var clientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    var categoryValue = fullRecord.CategoryValue;
    var description = fullRecord.Description;
    var instructions = fullRecord.Instructions;
    var quantity = fullRecord.Quantity;
    var sanitationPlanningId = fullRecord.SanitationPlanningId;
    $.ajax({
        url: "/SanitationJob/EditTools_Mobile",
        type: "GET",
        dataType: 'html',
        data: { ClientLookupId: clientLookupId, SanitationJobId: sanitationJobId, categoryValue: categoryValue, description: description, instructions: instructions, quantity: quantity, sanitationPlanningId: sanitationPlanningId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#SJToolPopUp').html(data);
        },
        complete: function () {
            SetSJControls();
            BindMobiscrollControlForSJToolTab();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', '.delToolsBttn', function () {
    var data = SJToolsTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SanitationJob/DeleteTools',
            data: {
                _SanitationPlanningId: data.SanitationPlanningId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    SJToolsTable.state.clear();
                    ShowDeleteAlert(getResourceValue("toolsDeleteSuccessAlert"));
                    generateSJToolsGrid();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
});
$(document).on('click', ".SJToolclearstate", function () {
    $('#AddSJToolModalpopup').parent().removeClass("slide-active");
});
$(function () {
    $(document).on('change', "#toolModel_CategoryValue", function () {
        $(document).find('#toolModel_hdnDropdownDescription').val($("#toolModel_CategoryValue option:selected").text());
    });
});
//#endregion
//#region ChemicalSupplies
var SJChemicalSuppliesTable;
function LoadSJChemicalSuppliesTab() {
    $.ajax({
        url: '/SanitationJob/LoadSJChemicalSupplies_Mobile',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#SJChemicalSupplies').html(data);
        },
        complete: function () {
            generateSJChemicalSuppliesGrid();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '.addChemicalSuppliesBttn,#btnAddSJChemicalSupplies', function () {
    AddChemicalSupplies();
});
$(document).on('click', '.editChemicalSuppliesBttn', function () {
    var data = SJChemicalSuppliesTable.row($(this).parents('tr')).data();
    EditChemicalSupplies(data);
});
function EditChemicalSupplies(fullRecord) {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    var sanitationPlanningId = fullRecord.SanitationPlanningId;
    var categoryValue = fullRecord.CategoryValue;
    var description = fullRecord.Description;
    var instructions = fullRecord.Instructions;
    var dilution = fullRecord.Dilution;
    var quantity = fullRecord.Quantity;
    $.ajax({
        url: "/SanitationJob/EditChemicalSupplies_Mobile",
        type: "GET",
        dataType: 'html',
        data: { ClientLookupId: ClientLookupId, SanitationJobId: SanitationJobId, sanitationPlanningId: sanitationPlanningId, categoryValue: categoryValue, description: description, instructions: instructions, dilution: dilution, quantity: quantity },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#SJChemicalSuppliesPopUp').html(data);
        },
        complete: function () {
            SetSJControls();
            BindMobiscrollControlForSJChemicalSuppliesTab();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', '.delChemicalSuppliesBttn', function () {
    var data = SJChemicalSuppliesTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SanitationJob/DeleteChemicalSupplies',
            data: {
                _SanitationPlanningId: data.SanitationPlanningId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    SJChemicalSuppliesTable.state.clear();
                    ShowDeleteAlert(getResourceValue("chemicalsDeleteSuccessAlert"));
                    generateSJChemicalSuppliesGrid();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
});
function generateSJChemicalSuppliesGrid() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var rCount = 0;
    var IsAddOrEditOrDel = false;
    if ($(document).find('#SJChemicalSuppliesTable').hasClass('dataTable')) {
        SJChemicalSuppliesTable.destroy();
    }
    SJChemicalSuppliesTable = $("#SJChemicalSuppliesTable").DataTable({
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
            "url": "/SanitationJob/PopulateChemicalSupplies?SanitationJobId=" + SanitationJobId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                IsAddOrEditOrDel = response.IsAddOrEditOrDel;
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    if (IsAddOrEditOrDel) {
                        return '<a class="btn btn-outline-success editChemicalSuppliesBttn gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delChemicalSuppliesBttn gridinnerbutton" title="Delete"><i class="fa fa-trash"></i></a>';
                    } else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "CategoryValue", "autoWidth": false, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "Instructions"
                },
                {
                    "data": "Quantity", defaultContent: "", "bSearchable": true, "bSortable": true
                },
                {
                    "data": "SanitationPlanningId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            if (IsAddOrEditOrDel) {
                $("#btnAddSJChemicalSupplies").show();
            }
            else {
                $("#btnAddSJChemicalSupplies").hide();
            }

        }
    });
}
function AddChemicalSupplies() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    $.ajax({
        url: "/SanitationJob/AddChemicalSupplies_Mobile",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: SanitationJobId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#SJChemicalSuppliesPopUp').html(data);
        },
        complete: function () {
            SetSJControls();
            BindMobiscrollControlForSJChemicalSuppliesTab();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function BindMobiscrollControlForSJChemicalSuppliesTab() {
    $('#AddSJChemicalSuppliesModalpopup').parent().addClass("slide-active").trigger('mbsc-enhance');
    $('#chemicalSuppliesModel_Quantity').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 999999999.999999,
        //scale: 2,
        maxScale: 6,
        entryMode: 'freeform',
        preset: 'decimal',
        thousandsSeparator: ''
    });
    var x = parseFloat($('#chemicalSuppliesModel_Quantity').val()) == 0 ? '' : $('#chemicalSuppliesModel_Quantity').val();
    $('#chemicalSuppliesModel_Quantity').mobiscroll('setVal', x);
}
function SJChemicalSuppliesAddOnSuccess(data) {
    CloseLoader();
    var SanitationJobId = data.SanitationJobId;
    if (data.data == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("ChemicalSupplyAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("ChemicalSupplyUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToSaDetail(SanitationJobId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', ".SJChemicalSuppliesclearstate", function () {
    $('#AddSJChemicalSuppliesModalpopup').parent().removeClass("slide-active");
});
$(function () {
    $(document).on('change', "#chemicalSuppliesModel_CategoryValue", function () {
        $(document).find('#chemicalSuppliesModel_hdnDropdownDescription').val($("#chemicalSuppliesModel_CategoryValue option:selected").text());
    });
});
//#endregion
//#region Assignment
var SJAssignTable;
function LoadSJAssignmentsTab() {
    $.ajax({
        url: '/SanitationJob/LoadSJAssignment_Mobile',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#SJAssignments').html(data);
        },
        complete: function () {
            generateSJAssignmentGrid();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '.addAssignmentBttn,#btnAddSJAssignment', function () {
    AddAssignment();
});
$(document).on('click', '.editAssignmentBttn', function () {
    var data = SJAssignTable.row($(this).parents('tr')).data();
    EditAssignment(data);
});
function EditAssignment(fullRecord) {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    var scheduledHours = fullRecord.ScheduledHours;
    var scheduledStartDate = fullRecord.ScheduledStartDate;
    var personnelId = fullRecord.PersonnelId;
    var updateIndex = fullRecord.UpdateIndex;
    var sanitationJobScheduleId = fullRecord.SanitationJobScheduleId;
    $.ajax({
        url: "/SanitationJob/EditAssignment_Mobile",
        type: "GET",
        dataType: 'html',
        data: { ClientLookupId: ClientLookupId, SanitationJobId: SanitationJobId, scheduledHours: scheduledHours, scheduledStartDate: scheduledStartDate, personnelId: personnelId, updateIndex: updateIndex, sanitationJobScheduleId: sanitationJobScheduleId, },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#SJAssignmentPopUp').html(data);
        },
        complete: function () {
            SetSJControls();
            BindMobiscrollControlForSJAssignmentTab();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', '.delAssignmentBttn', function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var data = SJAssignTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SanitationJob/DeleteAssignment',
            data: {
                SanitationJobScheduledId: data.SanitationJobScheduleId,
                SanitationJobId: SanitationJobId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                deleteConfirmSanitAssignment();
                if (data.Result == "success") {
                    SJAssignTable.state.clear();
                    ShowDeleteAlert(getResourceValue("assignmentDeleteSuccessAlert"));
                    generateSJAssignmentGrid();
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
});
function deleteConfirmSanitAssignment() {
    SuccessAlertSetting.text = getResourceValue("assignmentDeleteSuccessAlert");
    swal(SuccessAlertSetting, function () {
    });
}
function generateSJAssignmentGrid() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var IsAddOrDel = false;
    var rCount = 0;
    if ($(document).find('#SJAssignmentTable').hasClass('dataTable')) {
        SJAssignTable.destroy();
    }
    SJAssignTable = $("#SJAssignmentTable").DataTable({
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
            "url": "/SanitationJob/PopulateAssignment?SanitationJobId=" + SanitationJobId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                IsAddOrDel = response.IsAddOrDel;
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    if (IsAddOrDel) {
                        return '<a class="btn btn-outline-success editAssignmentBttn gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delAssignmentBttn gridinnerbutton" title="Delete"><i class="fa fa-trash"></i></a>';
                    } else {
                        return '<a class="btn btn-outline-danger delAssignmentBttn gridinnerbutton" title="Delete"><i class="fa fa-trash"></i></a>';
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "ScheduledStartDate",
                    "type": "date "
                },
                {
                    "data": "ScheduledHours", defaultContent: "", "bSearchable": true, "bSortable": true
                },
                {
                    "data": "SanitationJobScheduleId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            if (IsAddOrDel) {
                $("#btnAddSJAssignment").show();
            } else {
                $("#btnAddSJAssignment").hide();
            }
        }
    });
}
function AddAssignment() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    $.ajax({
        url: "/SanitationJob/AddAssignment_Mobile",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: SanitationJobId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#SJAssignmentPopUp').html(data);
        },
        complete: function () {
            SetSJControls();
            BindMobiscrollControlForSJAssignmentTab();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function BindMobiscrollControlForSJAssignmentTab() {
    $('#AddSJAssignmentModalpopup').parent().addClass("slide-active").trigger('mbsc-enhance');
    $('#assignmentModel_ScheduledStartDate').mobiscroll().calendar({
        display: 'bottom',
        touchUi: true,
        weekDays: 'min',
        yearChange: false,
        //min: new Date(),
        months: 1
    }).inputmask('mm/dd/yyyy');
    $('#assignmentModel_ScheduledHours').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 99999999.99,
        //scale: 2,
        maxScale: 2,
        entryMode: 'freeform',
        preset: 'decimal',
        thousandsSeparator: ''
    });
    var x = parseFloat($('#assignmentModel_ScheduledHours').val()) == 0 ? '' : $('#assignmentModel_ScheduledHours').val();
    $('#assignmentModel_ScheduledHours').mobiscroll('setVal', x);
}
function SJAssignmentAddOnSuccess(data) {
    var SanitationJobId = data.SanitationJobId;
    if (data.data == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AssignmentAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("AssignmentUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToSaDetail(SanitationJobId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
$(document).on('click', ".SJChemicalSuppliesclearstate", function () {
    $('#AddSJAssignmentModalpopup').parent().removeClass("slide-active");
});
//#endregion
//#region CancelSanitation
$(document).on('click', '#cancelModalCall,#btnCancelSanitations', function () {
    var thisid = $(this).attr('id');
    $.ajax({
        url: '/SanitationJob/PopulateCancelReasonDropdown',
        type: "GET",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $("#txtCancelReasonSelect").empty();
            $("#txtCancelReasonSelect").append("<option value=''>" + "--Select--" + "</option>");
            for (var i = 0; i < data.cancelReasonList.length; i++) {
                var id = data.cancelReasonList[i].Value;
                var name = data.cancelReasonList[i].Text;
                $("#txtCancelReasonSelect").append("<option value='" + id + "'>" + name + "</option>");
            }
        },
        complete: function () {
            SetSJControls();
            if (thisid == 'cancelModalCall') {
                var id = $('#JobDetailsModel_SanitationJobId').val();
                $('#sanitationCancelAndPrintListModel_SanitationJobId').val(id);
                $('#cancelModalDetailsPage').addClass("slide-active").trigger('mbsc-enhance');
            }
            else {
                $(document).find('#cancelModalSearchPage').modal('show');
            }
            CloseLoader();
        }
    });
});

function SanitationJobCancelOnSuccess(data) {

    $('#cancelModalDetailsPage').modal('hide');
    if (data.data == "success") {
        SuccessAlertSetting.text = getResourceValue("jobCancelsuccessmsg");


        swal(SuccessAlertSetting, function () {
            CloseLoader();
            RedirectToSaDetail(data.SanitationJobId);
        });
    }
    else {
        GenericSweetAlertMethod(data);
    }
}

$(document).on('click', ".CancelSJclearstate", function () {
    $('#cancelModalDetailsPage').removeClass("slide-active");
});
//#endregion
//#region Edit Sanitation
$(document).on('click', "#editsanitation", function (e) {
    e.preventDefault();
    var JobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    $.ajax({
        url: "/SanitationJob/EditSanitationJobDetails_Mobile",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: JobId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#EditSanitationPopUp').html(data);
        },
        complete: function () {
            SetSJControls();
            BindMobiscrollControlForEditSanitation();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function BindMobiscrollControlForEditSanitation() {
    $('#EditSanitationModalpopup').addClass("slide-active").trigger('mbsc-enhance');
    $('#JobDetailsModel_ScheduledDate').mobiscroll().calendar({
        display: 'bottom',
        touchUi: true,
        weekDays: 'min',
        yearChange: false,
        disabled: true,
        //min: new Date(),
        months: 1
    }).inputmask('mm/dd/yyyy');
    $('#JobDetailsModel_CompleteDate').mobiscroll().calendar({
        display: 'bottom',
        touchUi: true,
        weekDays: 'min',
        yearChange: false,
        disabled: true,
        //min: new Date(),
        months: 1
    }).inputmask('mm/dd/yyyy');
    $('#JobDetailsModel_ScheduledDuration').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 999999.99,
        //scale: 2,
        maxScale: 2,
        entryMode: 'freeform',
        preset: 'decimal',
        thousandsSeparator: ''
    });
    var x = parseFloat($('#JobDetailsModel_ScheduledDuration').val()) == 0 ? '' : $('#JobDetailsModel_ScheduledDuration').val();
    $('#JobDetailsModel_ScheduledDuration').mobiscroll('setVal', x);
    $('#JobDetailsModel_ActualDuration').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 999999.99,
        //scale: 2,
        maxScale: 2,
        entryMode: 'freeform',
        preset: 'decimal',
        thousandsSeparator: ''
    });
    var x = parseFloat($('#JobDetailsModel_ActualDuration').val()) == 0 ? '' : $('#JobDetailsModel_ActualDuration').val();
    $('#JobDetailsModel_ActualDuration').mobiscroll('setVal', x);
}
$(document).on('click', ".EditSanitationclearstate", function () {
    $('#EditSanitationModalpopup').removeClass("slide-active");
});
function SanitationUpdateOnSuccess(data) {
    CloseLoader();
    if (data.data === "success") {
        var SanitationJobId = data.SanitationJobId;
        if (data.Command === "save" || data.Command === "complete") {
            var message;
            if (data.mode === "add") {
                SuccessAlertSetting.text = getResourceValue("SanitationAddAlert");
            }
            else if (data.mode === "complete") {
                SuccessAlertSetting.text = getResourceValue("SanitationCompleteAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("SanitationUpdateAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToSaDetail(SanitationJobId);
            });
        }
        else {
            ResetErrorDiv();
            $('#identificationtab').addClass('active').trigger('click');
            SuccessAlertSetting.text = getResourceValue("SanitationAddAlert");

            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        if (data.Command === "complete") {
            message = getResourceValue(data.Result);
            ShowGenericErrorOnAddUpdate(message);
        }
        else {
            ShowGenericErrorOnAddUpdate(data.Result);
        }
    }
}
//#region Asset popup
$(document).on('click', "#openOJobAssetgrid", function () {
    $('#SJEquipmentModal').addClass('slide-active');
    $(document).find("#AssetListViewForSearch").html("");
    InitializeAssetListView_Mobile();
});
//#endregion
//#endregion
//#endregion

//#region V2-983 Task
var TaskIdsToupdate = [];
var TaskIdsToupdateReOpen = [];
var taskIDscancel;
var SJTaskTable;
function LoadSJTaskTab() {
    $.ajax({
        url: '/SanitationJob/LoadSJTask_Mobile',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#SJTask').html(data);
        },
        complete: function () {
            generateSJTaskGrid();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function generateSJTaskGrid() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ChargeToClientLookupId = $(document).find('#JobDetailsModel_ChargeTo_ClientLookupId').val();
    var rCount = 0;
    var IsForAllSecurity = false;
    $(document).find('#example-select-all-sensor').prop('checked', false);

    if ($(document).find('#SJTaskTable').hasClass('dataTable')) {
        SJTaskTable.destroy();
    }
    SJTaskTable = $("#SJTaskTable").DataTable({
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[1, "asc"]],
        stateSave: false,
        dom: 'rtlip',
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        "filter": true,
        "orderMulti": true,
        "ajax": {
            "url": "/SanitationJob/PopulateTask",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.sanitationJobId = SanitationJobId;
                d.ChargeToClientLookupId = ChargeToClientLookupId;
            },
            "dataSrc": function (response) {
                rCount = response.data.length;
                IsForAllSecurity = response.IsForAllSecurity;
                return response.data;
            }
        },
        columnDefs: [{
            "data": "SanitationJobTaskId",
            orderable: false,
            className: 'select-checkbox dt-body-center',
            targets: 0,
            'render': function (data, type, full, meta) {
                if ($('#example-select-all-sensor').is(':checked')) {
                    return '<input type="checkbox"  checked="checked" name="id[]" data-eqid="' + data + '" class="isSelect" value="' + $('<div/>').text(data).html() + '">';
                }
                else {
                    if (TaskIdsToupdate.indexOf(data) != -1) {
                        return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="isSelect ' + data + '"  value="'
                            + $('<div/>').text(data).html() + '">';
                    }
                    else {
                        return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="isSelect" value="' + $('<div/>').text(data).html() + '">';
                    }
                }
            }
        },
        {
            "data": null,
            targets: [4], render: function (a, b, data, d) {
                if (IsForAllSecurity) {
                    return '<a  class="btn btn-outline-primary addTaskBttnMbl gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                        '<a class="btn btn-outline-success editTaskBttnMbl gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delTaskBttnMbl gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
                else {
                    return "";
                }
            }
        }
        ],
        "columns":
            [
                {},
                { "data": "TaskId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    render: function (data, type, row, meta) {
                        if (data == statusCode.Cancel) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--orange m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Complete) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Open) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else {
                            return getStatusValue(data);
                        }
                    }
                },
                { "data": "SanitationJobTaskId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }

            ],
        initComplete: function () {
            SetPageLengthMenu();
            if (rCount > 0 && IsForAllSecurity) {
                $("#btnsjAddtaskMbl").hide();
                $("#btnsjCompletetaskMbl").show();
                $("#btnsjCanceltaskMbl").show();
                $("#btnsjReOpentaskMbl").show();
            }
            else {
                {
                    if (IsForAllSecurity) {
                        $("#btnsjAddtaskMbl").show();
                    } else {
                        $("#btnsjAddtaskMbl").hide();
                    }
                    $("#btnsjCompletetaskMbl").hide();
                    $("#btnsjCanceltaskMbl").hide();
                    $("#btnsjReOpentaskMbl").hide();
                }
            }
        }
    });
}
$(document).on('change', '.isSelect', function () {
    var data = SJTaskTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        var el = $('#example-select-all-sensor').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        var index = TaskIdsToupdate.indexOf(data.SanitationJobTaskId);
        TaskIdsToupdate.splice(index, 1);

        var indexOpen = TaskIdsToupdateReOpen.indexOf(data.SanitationJobTaskId + ',' + data.Status);
        TaskIdsToupdateReOpen.splice(indexOpen, 1);
    }
    else {
        TaskIdsToupdate.push(data.SanitationJobTaskId);
        TaskIdsToupdateReOpen.push(data.SanitationJobTaskId + ',' + data.Status);
    }
});
$(document).on('click', '#example-select-all-sensor', function (e) {
    var checked = this.checked;
    if (checked) {
        var sanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
        var ChargeToClientLookupId = $(document).find('#JobDetailsModel_ChargeTo_ClientLookupId').val();
        TaskIdsToupdate = [];
        TaskIdsToupdateReOpen = [];
        $.ajax({
            "url": "/SanitationJob/PopulateTaskIds?sanitationJobId=" + sanitationJobId + '&ChargeToClientLookupId=' + ChargeToClientLookupId,
            async: true,
            type: "GET",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data) {
                    $.each(data, function (index, item) {
                        TaskIdsToupdate.push(item.SanitationJobTaskId);
                        TaskIdsToupdateReOpen.push(item.SanitationJobTaskId + ',' + item.Status);
                    });
                }
                else {
                    TaskIdsToupdate = [];
                    TaskIdsToupdateReOpen = [];
                }
            },
            complete: function () {
                SJTaskTable.column(0).nodes().to$().each(function (index, item) {
                    $(this).find('.isSelect').prop('checked', 'checked');
                });
                CloseLoader();
            }
        });
    }
    else {
        $(document).find('.isSelect').prop('checked', false);
        TaskIdsToupdate = [];
        TaskIdsToupdateReOpen = [];
    }
});

$(document).on('click', '#btnsjCompletetaskMbl', function () {
    var sjid = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var taskIds = null;
    taskIds = TaskIdsToupdate.join();
    if (taskIds !== "") {
        $.ajax({
            url: '/SanitationJob/CompleteTask',
            data: { taskList: taskIds, sanitationJobId: sjid },
            type: "GET",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "success" && data.successcount > 0) {
                    var msg = data.successcount + getResourceValue("taskCompleteAlert");
                    ShowSuccessAlert(msg);
                    TaskIdsToupdate = [];
                    TaskIdsToupdateReOpen = [];
                    taskIds = null;
                    generateSJTaskGrid();
                }
                else {
                    $(document).find('.isSelect').prop('checked', false);
                    $(document).find('#example-select-all-sensor').prop('checked', false);
                    var msg = getResourceValue("taskNotCompleteAlert");
                    swal({
                        title: getResourceValue("taskNotCompleteAlert"),
                        text: msg,
                        type: "error",
                        showCancelButton: false,
                        confirmButtonClass: "btn-sm btn-danger",
                        cancelButtonClass: "btn-sm",
                        confirmButtonText: getResourceValue("SaveAlertOk"),
                        cancelButtonText: getResourceValue("CancelAlertNo")
                    });
                }
                TaskIdsToupdate = [];
                TaskIdsToupdateReOpen = [];
                taskIds = null;
            },
            complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
    else {
        swal({
            title: getResourceValue("taskNotSelectedAlert"),
            text: getResourceValue("taskSelectAlert"),
            type: "warning",
            confirmButtonClass: "btn-sm btn-primary",
            confirmButtonText: getResourceValue("SaveAlertOk"),
        });
        return false;
    }
});
$(document).on('click', '#btnsjReOpentaskMbl', function () {
    var sjid = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var taskIds = null;
    taskIds = TaskIdsToupdateReOpen.join();
    if (taskIds !== "") {
        $.ajax({
            url: '/SanitationJob/ReOpenTask',
            data: { taskList: taskIds, sanitationJobId: sjid },
            type: "GET",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "success") {
                    var msg = data.successcount + getResourceValue("taskReopenAlert");
                    ShowSuccessAlert(msg);
                    TaskIdsToupdate = [];
                    TaskIdsToupdateReOpen = [];
                    taskIds = null;
                    generateSJTaskGrid();
                    $(document).find('.isSelect').prop('checked', false);
                }
                else {
                    $(document).find('.isSelect').prop('checked', false);
                    $(document).find('#example-select-all-sensor').prop('checked', false);
                    var msg = getResourceValue("taskNotReopenAlert");
                    swal({
                        title: getResourceValue("taskCantReopenAlert"),
                        text: msg,
                        type: "error",
                        showCancelButton: false,
                        confirmButtonClass: "btn-sm btn-danger",
                        cancelButtonClass: "btn-sm",
                        confirmButtonText: getResourceValue("SaveAlertOk")
                    });
                }
                TaskIdsToupdate = [];
                TaskIdsToupdateReOpen = [];
                taskIds = null;
            },
            complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
    else {
        swal({
            title: getResourceValue("taskNotSelectedAlert"),
            text: getResourceValue("taskSelectAlert"),
            type: "warning",
            confirmButtonClass: "btn-sm btn-primary",
            confirmButtonText: getResourceValue("SaveAlertOk"),
        });
        return false;
    }
});
$(document).on('click', '#btnsjCanceltaskMbl', function () {
    taskIDscancel = null;
    taskIDscancel = TaskIdsToupdateReOpen.join();
    if (taskIDscancel !== "") {
        $.ajax({
            url: '/SanitationJob/PopulateCancelReasonDropdown',
            type: "GET",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $("#taskCancelReasonSelect").empty();
                $("#taskCancelReasonSelect").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < data.cancelReasonList.length; i++) {
                    var id = data.cancelReasonList[i].Value;
                    var name = data.cancelReasonList[i].Text;
                    $(document).find("#taskCancelReasonSelect").append("<option value='" + id + "'>" + name + "</option>");
                }
            },
            complete: function () {
                SetSJControls();
                $('#sjCancelTaskModal').addClass("slide-active").trigger('mbsc-enhance');
                CloseLoader();
            }
        });
    }
    else {
        $('#sjCancelTaskModal').removeClass("slide-active").trigger('mbsc-enhance');
        swal({
            title: getResourceValue("taskNotSelectedAlert"),
            text: getResourceValue("taskSelectAlert"),
            type: "warning",
            confirmButtonClass: "btn-sm btn-primary",
            confirmButtonText: getResourceValue("SaveAlertOk"),
        });
        return false;
    }
});
$(document).on('click', '#btnSjTaskCancelMbl', function () {
    var sjid = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var cancelreason = $(document).find('#taskCancelReasonSelect').val();
    if (cancelreason != "" && cancelreason != "--Select--") {
        $.ajax({
            url: '/SanitationJob/CancelTask',
            data: { taskList: taskIDscancel, cancelReason: cancelreason, sanitationJobId: sjid },
            type: "GET",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "success") {
                    var msg = data.successcount + getResourceValue("taskCancelAlert");
                    ShowSuccessAlert(msg);
                    TaskIdsToupdate = [];
                    TaskIdsToupdateReOpen = [];
                    taskIDscancel = null;
                    generateSJTaskGrid();
                }
                else {
                    $(document).find('.isSelect').prop('checked', false);
                    $(document).find('#example-select-all-sensor').prop('checked', false);
                    var msg = getResourceValue("taskNotCancelAlert");
                    swal({
                        title: getResourceValue("taskCancelFailedAlert"),
                        text: msg,
                        type: "error",
                        showCancelButton: false,
                        confirmButtonClass: "btn-sm btn-danger",
                        cancelButtonClass: "btn-sm",
                        confirmButtonText: getResourceValue("SaveAlertOk"),
                        cancelButtonText: getResourceValue("CancelAlertNo")
                    });
                }
                TaskIdsToupdate = [];
                TaskIdsToupdateReOpen = [];
                taskIDscancel = null;
            },
            complete: function () {
                CloseLoader();
                $('#sjCancelTaskModal').removeClass("slide-active").trigger('mbsc-enhance');
            },
            error: function () {
                CloseLoader();
            }
        });
    }
    else {
        swal({
            title: getResourceValue("taskNoCancelReasonAlert"),
            text: getResourceValue("taskSelectCancelReasonAlert"),
            type: "warning",
            showCancelButton: false,
            confirmButtonClass: "btn-sm btn-danger",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("SaveAlertOk"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        });
    }

});
$(document).on('click', '.sjTaskCancelModal', function () {
    TaskIdsToupdate = [];
    TaskIdsToupdateReOpen = [];
    taskIDscancel = null;
    $(document).find('.isSelect').prop('checked', false);
    $(document).find('#example-select-all-sensor').prop('checked', false);
    $('#sjCancelTaskModal').removeClass("slide-active").trigger('mbsc-enhance');
});

//#region Add,Edit,Delete
$(document).on('click', '.addTaskBttnMbl', function () {
    AddSjTaskMbl();
});
$(document).on('click', '.editTaskBttnMbl', function () {
    var data = SJTaskTable.row($(this).parents('tr')).data();
    EditSjTaskMbl(data.SanitationJobTaskId);
});
$(document).on('click', '.delTaskBttnMbl', function () {
    var data = SJTaskTable.row($(this).parents('tr')).data();
    DeleteSjTaskMbl(data.SanitationJobTaskId);
});
$(document).on('click', '.sjAddEditTaskCancel', function () {
    $('#SJTaskModalpopup').parent().removeClass("slide-active").trigger('mbsc-enhance');
});
function AddSjTaskMbl() {
    var sjID = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ChargeToClientLookupId = $(document).find('#JobDetailsModel_ChargeTo_ClientLookupId').val();
    var ChargeType = $(document).find('#JobDetailsModel_ChargeType').val();
    $.ajax({
        url: "/SanitationJob/AddTasksMbl",
        type: "GET",
        dataType: 'html',
        data: { sanitationJobId: sjID, ChargeToClientLookupId: ChargeToClientLookupId, ChargeType: ChargeType },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#SJTaskPopUp').html(data);
            $(document).find('#SJTaskModalpopup').parent().addClass("slide-active").trigger('mbsc-enhance');
        },
        complete: function () {
            SetSJControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SjTaskAddEditOnSuccessMbl(data) {
    CloseLoader();
    var sjid = data.sjid;
    var sjid = $(document).find('#sanitationJobTaskModel_SanitationJobId').val();
    if (data.data == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("TaskAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("TaskUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            $('#SJTaskModalpopup').parent().removeClass("slide-active").trigger('mbsc-enhance');
            generateSJTaskGrid();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditSjTaskMbl(taskid) {
    var sjID = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    $.ajax({
        url: "/SanitationJob/EditTasksMbl",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: sjID, _taskId: taskid, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#SJTaskPopUp').html(data);
            $(document).find('#SJTaskModalpopup').parent().addClass("slide-active").trigger('mbsc-enhance');
        },
        complete: function () {
            SetSJControls();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function DeleteSjTaskMbl(taskId) {
    var sjid = $(document).find('#sanitationJobTaskModel_SanitationJobId').val();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SanitationJob/DeleteTasks',
            data: {
                taskNumber: taskId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    SJTaskTable.state.clear();
                    ShowDeleteAlert(getResourceValue("taskDeleteSuccessAlert"));
                    generateSJTaskGrid();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}

//#endregion


//#endregion

//#region V2-912
$(document).on('click', '#SanitationJobApprove_Mobile', function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();

    $.ajax({
        url: '/SanitationJob/SanitationJobApprove_Mobile',
        data: {
            SanitationJobId: SanitationJobId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                SuccessAlertSetting.text = getResourceValue("alertSanitationJobApproved");
                swal(SuccessAlertSetting, function () {
                    CloseLoader();
                    RedirectToSaDetail(data.SanitationJobId, "overview");
                });
            } else {
                alert('error');
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});

$(document).on('click', '#SanitationJobComplete_Mobile', function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();

    $.ajax({
        url: '/SanitationJob/SanitationJobComplete_Mobile',
        data: {
            SanitationJobId: SanitationJobId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                SuccessAlertSetting.text = getResourceValue("alertSanitationJobCompleted");
                swal(SuccessAlertSetting, function () {
                    CloseLoader();
                    RedirectToSaDetail(data.SanitationJobId, "overview");
                });
            } else {
                alert('error');
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});

$(document).on('click', '#SanitationJobPass_Mobile', function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();

    $.ajax({
        url: '/SanitationJob/SanitationJobPass_Mobile',
        data: {
            SanitationJobId: SanitationJobId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                SuccessAlertSetting.text = getResourceValue("alertSanitationJobPassed");
                swal(SuccessAlertSetting, function () {
                    CloseLoader();
                    RedirectToSaDetail(data.SanitationJobId, "overview");
                });
            } else {
                alert('error');
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#region Fail Sanitation
$(document).on('click', '#FailModalCall_Mobile', function () {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();

    $.ajax({
        url: "/SanitationJob/GetFailVarificationSanitationJob_Mobile",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: SanitationJobId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#FailSanitationpopup').html('');
            $('#FailSanitationpopup').html(data);

            /*   $(document).find('#FailSanitationModalpopup').parent().addClass("slide-active").trigger('mbsc-enhance');*/
        },
        complete: function () {
            $(document).find('#FailSanitationModalpopup').parent().addClass("slide-active").trigger('mbsc-enhance');
            SetSJControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});

function SanitationJobFailVarificationOnSuccess_Mobile(data) {
    $('#FailSanitationModalpopup').parent().removeClass("slide-active").trigger('mbsc-enhance');

    if (data.data == "success") {
        SuccessAlertSetting.text = getResourceValue("alertSVFailedReasonVarification");


        swal(SuccessAlertSetting, function () {
            CloseLoader();
            RedirectToSaDetail(data.SanitationJobId, "overview");
        });
    }
    else {
        GenericSweetAlertMethod(data);
    }
}


$(document).on('click', '.FailJobBtn', function () {

    var areaddescribedby = $(document).find("#txtfailReasonSelect").attr('aria-describedby');
    if (typeof areaddescribedby !== 'undefined') {
        $('#' + areaddescribedby).hide();
    }
});

$(document).on('click', ".FailSJclearstate", function () {
    $('#FailSanitationModalpopup').parent().removeClass("slide-active").trigger('mbsc-enhance');
});

//#endregion
//endregion

//#region Add Work Request V2-1055
$(document).on('click', "#AddWorkRequestBtn_Mobile", function (e) {
    var SJId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var SJChargeToId = $(document).find('#SJChargeToId').val();
    var SJChargeTo_ClientLookupId = $(document).find('#JobDetailsModel_ChargeTo_ClientLookupId').val();
    $.ajax({
        url: "/SanitationJob/AddWoRequestDynamicInSanitationJobDeatails_Mobile",
        type: "GET",
        dataType: 'html',
        data: {
            'SJChargeToId': SJChargeToId,
            'SJChargeTo_ClientLookupId': SJChargeTo_ClientLookupId,
            'SJId': SJId,
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddWorkRequestDiv').html(data);
        },
        complete: function () {
            SetControls();
            $('.dtpicker').mobiscroll().calendar({
                display: 'bottom',
                touchUi: true,
                weekDays: 'min',
                yearChange: false,
                min: new Date(),
                months: 1
            }).inputmask('mm/dd/yyyy');
            $('.decimal').mobiscroll().numpad({
                touchUi: true,
                scale: 2,
                preset: 'decimal',
                thousandsSeparator: ''
            });
            $('#AddWorkRequestModalDialog').addClass('slide-active').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
});
function WorkRequestDynamicAddOnSuccess(data) {
    if (data.data === "success") {
        if (data.Command === "save") {
            if (fileExtAddProccess != "") {
                var imgname = data.workOrderID + "_" + Math.floor((new Date()).getTime() / 1000);
                CompressImageAddProccess(FilesAddProccess[0], imgname + "." + fileExtAddProccess, data.workOrderID);
                fileExtAddProccess = "";
            }
            SuccessAlertSetting.text = getResourceValue("spnWoRequestAddSuccessfully");
            swal(SuccessAlertSetting, function () {
                var DashboardId = $('#DashboardlistingId').val();
                if (DashboardId == null || DashboardId == "") {
                    DashboardId = 0;
                }
                $(document).find('#AddWorkRequestModalDialog').removeClass('slide-active');
            });
        }
    }
    else {
        CloseLoader();
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '.btnCancelAddWorkRequest', function () {
    $('#AddWorkRequestModalDialog').removeClass('slide-active');
    fileExtAddProccess = "";
});

var FilesAddProccess;
var fileExtAddProccess = "";
$(document).on('change', '.addphotoWorkorder', function () {
    var id = $(this).attr('id');
    var val = $(this).val();
    var previewid = $(this).closest(".takePic").find("img").attr('id');
    var imageName = val.replace(/^.*[\\\/]/, '');
    var fileUpload = $("#" + id).get(0);
    var fileExt = imageName.substr(imageName.lastIndexOf('.') + 1);

    if (fileExt != 'jpeg' && fileExt != 'jpg' && fileExt != 'png' && fileExt != 'JPEG' && fileExt != 'JPG' && fileExt != 'PNG') {
        ShowErrorAlert(getResourceValue("spnValidImage"));
        $("#" + id).val('');
        return false;
    }
    else if (this.files[0].size > (1024 * 1024 * 10)) {
        ShowImageSizeExceedAlert();
        $("#" + id).val('');
        return false;
    }
    else {
        if (window.FormData !== undefined) {
            var url = window.URL.createObjectURL(this.files[0]);
            $('#' + previewid).attr('src', url);
            FilesAddProccess = fileUpload.files;
            fileExtAddProccess = fileExt;
        }
        else {
        }
    }
});
function CompressImageAddProccess(files, imageName, WorkOrderId) {
    new Compressor(files, {
        quality: 0.6,
        convertTypes: ['image/png'],
        convertSize: 100000,
        // The compression process is asynchronous,
        // which means you have to access the `result` in the `success` hook function.
        success(result) {
            if (result.size < files.size) {
                SaveCompressedImageAddProccess(result, imageName, WorkOrderId);
            }
            else {
                SaveCompressedImageAddProccess(files, imageName, WorkOrderId);
            }
            console.log('file name ' + result.name + ' after compress ' + result.size);
        },
        error(err) {
            console.log(err.message);
        },
    });
}
function SaveCompressedImageAddProccess(data, imageName, WorkOrderId) {
    var AddPhotoFileData = new FormData();
    AddPhotoFileData.append('file', data, imageName);

    $.ajax({
        url: '../base/SaveUploadedFile',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: AddPhotoFileData,
        success: function (result) {
            SaveUploadedFileToServerAddProccess(WorkOrderId, imageName);
            $('#add_photosWR').val('');
        }
    });
}
function SaveUploadedFileToServerAddProccess(WorkOrderId, imageName) {
    $.ajax({
        url: '../base/SaveMultipleUploadedFileToServer',
        type: 'POST',

        data: { 'fileName': imageName, objectId: WorkOrderId, TableName: "WorkOrder", AttachObjectName: "WorkOrder" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {
                CloseLoader();
            }
            else if (data.result == "1") {
                CloseLoader();
                var errorMessage = getResourceValue("ImageExistAlert");
                ShowErrorAlert(errorMessage);
            }
            else {
                CloseLoader();
                var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
                ShowErrorAlert(errorMessage);
            }
        },
        complete: function () {
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion
//#region V2-1118
//#region Labor
var SJLaborTable;
function LoadSJLaborTab() {
    $.ajax({
        url: '/SanitationJob/LoadSJLabor_Mobile',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#SJLabor').html(data);
        },
        complete: function () {
            generateSJLaborGrid();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function generateSJLaborGrid() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var rCount = 0;
    if ($(document).find('#SJLaborTable').hasClass('dataTable')) {
        SJLaborTable.destroy();
    }
    SJLaborTable = $("#SJLaborTable").DataTable({
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
            "url": "/SanitationJob/PopulateLabor_Mobile?SanitationJobId=" + SanitationJobId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-success editLaborBttn gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delLaborBttn gridinnerbutton" title="Delete"><i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                { "data": "PersonnelClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "StartDate",
                    "type": "date "
                },
                {
                    "data": "Hours", defaultContent: "", "bSearchable": true, "bSortable": true
                },
                { "data": "Value", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "TimecardId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.addLaborBttn,#btnAddSJLabor', function () {
    AddLabor();
});
function AddLabor() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    $.ajax({
        url: "/SanitationJob/AddLabor_Mobile",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: SanitationJobId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#SJLaborPopUp').html(data);
        },
        complete: function () {
            SetSJControls();
            BindMobiscrollControlForSJLaborTab();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SJLaborAddOnSuccess(data) {
    CloseLoader();
    var SanitationJobId = data.SanitationJobId;
    if (data.data == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("LaborAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("LaborUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToSaDetail(SanitationJobId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function BindMobiscrollControlForSJLaborTab() {
    $('#AddSJLaborModalpopup').parent().addClass("slide-active").trigger('mbsc-enhance');
    $('#laborModel_StartDate').mobiscroll().calendar({
        display: 'bottom',
        touchUi: true,
        weekDays: 'min',
        yearChange: false,
        //min: new Date(),
        months: 1
    }).inputmask('mm/dd/yyyy');
    $('#laborModel_Hours').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 99999999.99,
        //scale: 2,
        maxScale: 2,
        entryMode: 'freeform',
        preset: 'decimal',
        thousandsSeparator: ''
    });
    var x = parseFloat($('#laborModel_Hours').val()) == 0 ? '' : $('#laborModel_Hours').val();
    $('#laborModel_Hours').mobiscroll('setVal', x);
}
$(document).on('click', '.editLaborBttn', function () {
    var data = SJLaborTable.row($(this).parents('tr')).data();
    EditLabor(data);
});
function EditLabor(fullRecord) {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    var chargeToId_Primary = fullRecord.ChargeToId_Primary;
    var hours = fullRecord.Hours;
    var startDate = fullRecord.StartDate;
    var personnelId = fullRecord.PersonnelId;
    var updateIndex = fullRecord.UpdateIndex;
    var timecardId = fullRecord.TimecardId;
    $.ajax({
        url: "/SanitationJob/EditLabor_Mobile",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: SanitationJobId, ClientLookupId: ClientLookupId, chargeToId_Primary: chargeToId_Primary, hours: hours, startDate: startDate, personnelId: personnelId, updateIndex: updateIndex, timecardId: timecardId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#SJLaborPopUp').html(data);
        },
        complete: function () {
            SetSJControls();
            BindMobiscrollControlForSJLaborTab();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', '.delLaborBttn', function () {
    var data = SJLaborTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SanitationJob/DeleteLabor',
            data: {
                timecardId: data.TimecardId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    SJLaborTable.state.clear();
                    ShowDeleteAlert(getResourceValue("laborDeleteSuccessAlert"));
                    generateSJLaborGrid();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
});
$(document).on('click', ".SJLaborclearstate", function () {
    $('#AddSJLaborModalpopup').parent().removeClass("slide-active");
});
//#endregion
//#region Notes
var SJNotesTable;
function LoadSJNotesTab() {
    $.ajax({
        url: '/SanitationJob/LoadSJNotes_Mobile',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#SJNotes').html(data);
        },
        complete: function () {
            generateSJNotesGrid();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function generateSJNotesGrid() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var rCount = 0;
    if ($(document).find('#SJNotesTable').hasClass('dataTable')) {
        SJNotesTable.destroy();
    }
    SJNotesTable = $("#SJNotesTable").DataTable({
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
            "url": "/SanitationJob/PopulateNotes_Mobile?SanitationJobId=" + SanitationJobId,
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                targets: [3], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-success editSJnote gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delSJnote gridinnerbutton" title="Delete"><i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "ModifiedDate",
                    "type": "date "
                },
                {
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.addNoteBttn,#btnSJAddNote', function () {
    AddNotes();
});
function AddNotes() {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    $.ajax({
        url: "/SanitationJob/AddNotes_Mobile",
        type: "GET",
        dataType: 'html',
        data: { SanitationJobId: SanitationJobId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#SJNotesPopUp').html(data);
        },
        complete: function () {
            SetSJControls();
            BindMobiscrollControlForSJNotesTab();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SJNotesAddOnSuccess(data) {
    CloseLoader();
    var SanitationJobId = data.SanitationJobId;
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AddNoteAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("UpdateNoteAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToSaDetail(SanitationJobId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function BindMobiscrollControlForSJNotesTab() {
    $('#AddSJNotesModalpopup').parent().addClass("slide-active").trigger('mbsc-enhance');
}
$(document).on('click', '.editSJnote', function () {
    var data = SJNotesTable.row($(this).parents('tr')).data();
    EditNotes(data);
});
function EditNotes(fullrecord) {
    var SanitationJobId = $(document).find('#JobDetailsModel_SanitationJobId').val();
    var ClientLookupId = $(document).find('#JobDetailsModel_ClientLookupId').val();
    var notesid = fullrecord.NotesId;
    var subject = fullrecord.Subject;
    var content = fullrecord.Content;
    var updatedindex = fullrecord.updatedindex;
    $.ajax({
        url: "/SanitationJob/EditNote_Mobile",
        type: "GET",
        dataType: 'html',
        data: { ClientLookupId: ClientLookupId, SanitationJobId: SanitationJobId, notesId: notesid, subject: subject, content: content, updatedindex: updatedindex },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#SJNotesPopUp').html(data);
        },
        complete: function () {
            SetSJControls();
            BindMobiscrollControlForSJNotesTab();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', '.delSJnote', function () {
    var data = SJNotesTable.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/SanitationJob/DeleteNotes',
            data: {
                _notesId: data.NotesId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    SJNotesTable.state.clear();
                    ShowDeleteAlert(getResourceValue("noteDeleteSuccessAlert"));
                    generateSJNotesGrid();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
});
$(document).on('click', ".SJNotesclearstate", function () {
    $('#AddSJNotesModalpopup').parent().removeClass("slide-active");
});
//#endregion
//#endregion