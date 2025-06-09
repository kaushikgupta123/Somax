$(document).ready(function () {
    if ($('#IsDetailFromWorkOrder').val() != undefined && $('#IsDetailFromWorkOrder').val().toLowerCase() === 'true') {
        var equipmentSearchstatus = localStorage.getItem("EquipmentViewstatusTextMobile");
        $(document).find('#spnlinkToSearch').text(equipmentSearchstatus);
        $(document).find('#divImageGrid').hide();
        var EquipmentId = $('#EquipData_EquipmentId').val();
        SetEquipmentDetailEnvironment();
        SetFixedHeadStyle();
        LoadAssetDetails(EquipmentId)
        SetControls_Mobile();
        titleText = equipmentSearchstatus;
    }
});

//#region Details
$(document).on('click', '#linkToSearch, .btnCancelAddEquipment', function () {
    window.location.href = "../Equipment/Index?page=Maintenance_Assets";
});
var tab = "Details";
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
        case "Comments":
            LoadCommentsTab();
            break;
        case "Photos":
            var id = $('#EquipData_EquipmentId').val();
            $(document).find('#divImageGrid').show();
            LoadImages(id);
            break;
        case "Details":
            var id = $('#EquipData_EquipmentId').val();
            LoadAssetDetails(id);
            break;
        case "Attachment":
            LoadAttachments();
            break;
        case "TechSpecs":
            LoadTechSpecs();
            break;
        case "Parts":
            LoadParts();
            break;
        case "Downtime":
            LoadDowntime();
            break;
        case "PMList":
            LoadPMList();
            break;
        case "WOActive":
            LoadWOActive();
            break;
        case "WOComplete":
            LoadWOComplete();
            break;
        case "PartIssues":
            LoadPartIssues();
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
    $(document).find('#Comments').html('');
    $(document).find('#Photos').html('');
    $(document).find('#divImageGrid').hide();
    $(document).find('#Attachment').html('');
    $(document).find('#TechSpecs').html('');
    $(document).find('#Parts').html('');
    $(document).find('#Downtime').html('');
    $(document).find('#PMList').html('');
    $(document).find('#WOActive').html('');
    $(document).find('#WOComplete').html('');
    $(document).find('#PartIssues').html('');
}
//#endregion
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
            //alert(result);
            var EquipmentId = $('#EquipData_EquipmentId').val();
            SaveMultipleUploadedFileToServer(EquipmentId, imageName);
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
        //alert('Hello');
        swal(AddImageAlertSetting, function () {
            if (window.FormData !== undefined) {
                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    console.log('file name ' + files[i].name + ' before compress ' + files[i].size);
                    if (_isMobile == true) {
                        var EquipmentId = $('#EquipData_EquipmentId').val();
                        var imgname = EquipmentId + "_" + Math.floor((new Date()).getTime() / 1000);
                        CompressImage(files[i], imgname + "." + fileExt);
                    }
                    else {
                        CompressImage(files[i], imageName);
                    }

                }
            } else {
                //alert("FormData is not supported.");
            }
            $('#files').val('');

        });
    }
});
function SaveMultipleUploadedFileToServer(EquimentId, imageName) {
    $.ajax({
        url: '../base/SaveMultipleUploadedFileToServer',
        type: 'POST',

        data: { 'fileName': imageName, objectId: EquimentId, TableName: "Equipment", AttachObjectName: "Equipment" },
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
            LoadImages(EquimentId);
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion
//#region Show Images
var cardviewstartvalue = 0;
var cardviewlwngth = 10;
var grdcardcurrentpage = 1;
var currentorderedcolumn = 1;
var layoutTypeWO = 1;
function LoadImages(EquipmentId) {
    $.ajax({
        url: '/Equipment/GetImages_Mobile ',
        type: 'POST',
        data: {
            currentpage: grdcardcurrentpage,
            start: cardviewstartvalue,
            length: cardviewlwngth,
            EquipmentId: EquipmentId
        },
        beforeSend: function () {
            $(document).find('#imagedataloader').show();
        },
        success: function (data) {
            /*if (data.TotalCount > 0) {*/
            $(document).find('#ImageGrid').show();
            $(document).find('#EquipmentImages').html(data).show();
            $(document).find('#tblimages_paginate li').each(function (index, value) {
                $(this).removeClass('active');
                if ($(this).data('currentpage') == grdcardcurrentpage) {
                    $(this).addClass('active');
                }
            });
            //}
            //else {
            //    $(document).find('#ImageGrid').hide();
            //}
        },
        complete: function () {
            $(document).find('#imagedataloader').hide();
            $(document).find('#imagecardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(cardviewlwngth).trigger('change.select2');
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('change', '#imagecardviewpagelengthdrp', function () {
    var EquimentId = $('#EquipData_EquipmentId').val();
    cardviewlwngth = $(this).val();
    grdcardcurrentpage = parseInt(cardviewstartvalue / cardviewlwngth) + 1;
    cardviewstartvalue = parseInt((grdcardcurrentpage - 1) * cardviewlwngth) + 1;
    LoadImages(EquimentId);

});
$(document).on('click', '#tblimages_paginate .paginate_button', function () {
    var EquimentId = $('#EquipData_EquipmentId').val();
    if (layoutTypeWO == 1) {
        var currentselectedpage = parseInt($(document).find('#tblimages_paginate .pagination').find('.active').text());
        cardviewlwngth = $(document).find('#imagecardviewpagelengthdrp').val();
        cardviewstartvalue = cardviewlwngth * (parseInt($(this).find('.page-link').text()) - 1);
        var lastpage = parseInt($(this).prev('li').data('currentpage'));

        if ($(this).attr('id') == 'tbl_previous') {
            if (currentselectedpage == 1) {
                return false;
            }
            cardviewstartvalue = cardviewlwngth * (currentselectedpage - 2);
            grdcardcurrentpage = grdcardcurrentpage - 1;
        }
        else if ($(this).attr('id') == 'tbl_next') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            cardviewstartvalue = cardviewlwngth * (currentselectedpage);
            grdcardcurrentpage = grdcardcurrentpage + 1;
        }
        else if ($(this).attr('id') == 'tbl_first') {
            if (currentselectedpage == 1) {
                return false;
            }
            grdcardcurrentpage = 1;
            cardviewstartvalue = 0;
        }
        else if ($(this).attr('id') == 'tbl_last') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            grdcardcurrentpage = parseInt($(this).prevAll('li').eq(1).text());
            cardviewstartvalue = cardviewlwngth * (grdcardcurrentpage - 1);
        }
        else {
            grdcardcurrentpage = $(this).data('currentpage');
        }
        LoadImages(EquimentId);

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
    var EquipmentId = $(document).find('#EquipData_EquipmentId').val();
    $('.modal-backdrop').remove();
    $.ajax({
        url: '../base/SetImageAsDefault',
        type: 'POST',

        data: { AttachmentId: AttachmentId, objectId: EquipmentId, TableName: "Equipment" },
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
            LoadImages(EquipmentId);
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
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var ClientOnPremise = $('#EquipModel_ClientOnPremise').val();
    $('.modal-backdrop').remove();
    if (ClientOnPremise == 'True') {
        DeleteOnPremiseMultipleImage(EquipmentId, AttachmentId);
    }
    else {
        DeleteAzureMultipleImage(EquipmentId, AttachmentId);
    }
});

function DeleteOnPremiseMultipleImage(EquipmentId, AttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '../base/DeleteMultipleImageFromOnPremise',
            type: 'POST',
            data: { AttachmentId: AttachmentId, objectId: EquipmentId, TableName: "Equipment" },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectionToDetails_Mobile(EquipmentId);
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function DeleteAzureMultipleImage(EquipmentId, AttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '../base/DeleteMultipleImageFromAzure',
            type: 'POST',
            data: { AttachmentId: AttachmentId, objectId: EquipmentId, TableName: "Equipment" },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    //LoadImages(EquimentId);
                    RedirectionToDetails_Mobile(EquipmentId);
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
//#region Comments
var colorarray = [];
function colorobject(string, color) {
    this.string = string;
    this.color = color;
}
function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}
function LoadCommentsTab() {
    var Equipmentid = $(document).find('#EquipData_EquipmentId').val();
    $.ajax({
        url: '/Equipment/GetCommentsDetails_Mobile',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Comments').html('');
            $(document).find('#Comments').html(data);
            LoadComments(Equipmentid);
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

}
function LoadComments(EquipmentId) {
    $.ajax({
        "url": "/Equipment/LoadComments_Mobile",
        data: { EquipmentId: EquipmentId },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var getTexttoHtml = textToHTML(data);
            $(document).find('#commentstems').html(getTexttoHtml);
            $(document).find("#commentsList").mCustomScrollbar({
                theme: "minimal"
            });
        },
        complete: function () {
            var ftext = '';
            var bgcolor = '';
            $(document).find('#commentsdataloader').hide();
            $(document).find('#commentstems').find('.comment-header-item').each(function () {
                var thistext = LRTrim($(this).text());
                if (ftext == '' || ftext != thistext) {
                    var bgcolorarr = colorarray.filter(function (a) {
                        return a.string == thistext;
                    });
                    if (bgcolorarr.length == 0) {
                        bgcolor = getRandomColor();
                        var thisval = new colorobject(thistext, bgcolor);
                        colorarray.push(thisval);
                    }
                    else {
                        bgcolor = bgcolorarr[0].color;
                    }
                }
                $(this).attr('style', 'color:' + bgcolor + '!important;border:1px solid' + bgcolor + '!important;');
                ftext = LRTrim($(this).text());
            });
            var loggedinuserinitial = LRTrim($('#hdr-comments-add').text());
            var avlcolor = colorarray.filter(function (a) {
                return a.string == loggedinuserinitial;
            });
            if (avlcolor.length == 0) {
                $('#hdr-comments-add').attr('style', 'border:1px solid #264a7c !important;').show();
            }
            else {
                $('#hdr-comments-add').attr('style', 'color:' + avlcolor[0].color + ' !important;border:1px solid ' + avlcolor[0].color + '!important;').show();
            }
            $('.kt-notes__body a').attr('target', '_blank');
            CloseLoader();
        }
    });
}
$(document).on("focus", "#wotxtcommentsnew", function () {
    $(document).find('.ckeditorarea').show();
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.ckeditorarea').hide();
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    //ClearEditor();
    LoadCkEditor('wotxtcomments');
    $("#wotxtcommentsnew").hide();
    $(".ckeditorfield").show();
});
$(document).on('click', '#btnsavecommands', function () {
    var selectedUsers = [];
    const data = getDataFromTheEditor();
    if (!data) {
        return false;
    }
    var EquipmentId = $(document).find('#EquipData_EquipmentId').val();
    var ClientLookupId = $(document).find('#EquipData_ClientLookupId').val();
    var noteId = 0;
    if (LRTrim(data) == "") {
        return false;
    }
    else {
        $(document).find('.ck-content').find('.mention').each(function (item, index) {
            selectedUsers.push($(index).data('user-id'));
        });
        $.ajax({
            url: '/Equipment/AddOrUpdateComment_Mobile',
            type: 'POST',
            beforeSend: function () {
                ShowLoader();
            },
            data: {
                EquipmentId: EquipmentId,
                content: data,
                EqClientLookupId: ClientLookupId,
                userList: selectedUsers,
                noteId: noteId,
            },
            success: function (data) {
                if (data.Result == "success") {
                    var message;
                    if (data.mode == "add") {
                        SuccessAlertSetting.text = getResourceValue("CommentAddAlert");
                    }
                    else {
                        SuccessAlertSetting.text = getResourceValue("CommentUpdateAlert");
                    }
                    swal(SuccessAlertSetting, function () {
                        LoadCommentsTab();
                    });
                }
                else {
                    ShowGenericErrorOnAddUpdate(data);
                    CloseLoader();
                }
            },
            complete: function () {
                ClearEditor();
                $("#wotxtcommentsnew").show();
                $(".ckeditorfield").hide();
                selectedUsers = [];
                selectedUnames = [];
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
});
$(document).on('click', '#commandCancel', function () {
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    ClearEditor();
    $("#wotxtcommentsnew").show();
    $(".ckeditorfield").hide();
});
$(document).on('click', '.editcomments', function () {
    $(document).find(".ckeditorarea").each(function () {
        $(this).html('');
    });
    $("#wotxtcommentsnew").show();
    $(".ckeditorfield").hide();
    var notesitem = $(this).parents('.kt-notes__item').eq(0);
    notesitem.find('.ckeditorarea').html(CreateEditorHTML('wotxtcommentsEdit'));
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    var rawHTML = $.parseHTML($(this).parents('.kt-notes__item').find('.kt-notes__body').find('.originalContent').html());
    LoadCkEditorEdit('wotxtcommentsEdit', rawHTML);
    $(document).find('.ckeditorarea').hide();
    notesitem.find('.ckeditorarea').show();
    notesitem.find('.kt-notes__body').hide();
    notesitem.find('.commenteditdelearea').hide();
});

$(document).on('click', '.deletecomments', function () {
    DeleteWoNote($(this).attr('id'));
});
$(document).on('click', '.btneditcomments', function () {
    var data = getDataFromTheEditor();
    var noteId = $(this).parents('.kt-notes__item').find('.editcomments').attr('id');
    var EquipmentId = $(document).find('#EquipData_EquipmentId').val();
    var ClientLookupId = $(document).find('#EquipData_ClientLookupId').val();
    var updatedindex = $(this).parents('.kt-notes__item').find('.hdnupdatedindex').val();
    $.ajax({
        url: '/Equipment/AddOrUpdateComment_Mobile',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        data: { EquipmentId: EquipmentId, content: LRTrim(data), EqClientLookupId: ClientLookupId, noteId: noteId, updatedindex: updatedindex },
        success: function (data) {
            if (data.Result == "success") {
                var message;
                if (data.mode == "add") {
                    SuccessAlertSetting.text = getResourceValue("CommentAddAlert");
                }
                else {
                    SuccessAlertSetting.text = getResourceValue("CommentUpdateAlert");
                }
                swal(SuccessAlertSetting, function () {
                    LoadCommentsTab();
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
                CloseLoader();
            }
        },
        complete: function () {
            // ClearEditorEdit();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

});
$(document).on('click', '.btncommandCancel', function () {
    ClearEditorEdit();
    $(document).find('.ckeditorarea').hide();
    $(this).parents('.kt-notes__item').find('.kt-notes__body').show();
    $(this).parents('.kt-notes__item').find('.commenteditdelearea').show();
});
function DeleteWoNote(notesId) {
    swal(CancelAlertSettingForCallback, function () {
        $.ajax({
            url: '/Base/DeleteComment',
            data: {
                _notesId: notesId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    SuccessAlertSetting.text = getResourceValue("CommentDeleteAlert");
                    swal(SuccessAlertSetting, function () {
                        LoadCommentsTab();
                    });
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}

//#endregion 

function LoadAssetDetails(EquipmentId) {
    $.ajax({
        url: '/Equipment/LoadViewAssetDetails_Mobile ',
        type: 'POST',
        data: {
            EquipmentId: EquipmentId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#AssetDetails').html(data);
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', '#showAddEquip_Mobile', function () {
    addAssetMobile();
});
function addAssetMobile() {
    $.ajax({
        url: "/Equipment/AddEquipmentDynamicView_Mobile",
        type: "POST",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddEquipmentDiv').html(data);
        },
        complete: function () {
            SetControls_Mobile();
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
                //min: 0.01,
                //max: 99999999.99,
                scale: 2,
                preset: 'decimal',
                thousandsSeparator: ''
            });
            $('#AddEquipmentModalDialog').addClass('slide-active').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
}
function EquipmentAddDynamicOnSuccess_Mobile(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Command == "save") {
            $('#AddEquipmentModalDialog').removeClass('slide-active');
            var message;
            if (data.mode == "add") {
                /*localStorage.setItem("PARTMOBILESTATUS", '1');*/
                //localStorage.setItem("partstatustext", getResourceValue("AlertActive"));
                SuccessAlertSetting.text = getResourceValue("EquipmentSaveAlerts");
            }
            //else {
            //    SuccessAlertSetting.text = getResourceValue("UpdatePartsAlerts");
            //}
            swal(SuccessAlertSetting, function () {
                RedirectionToDetails_Mobile(data.EquipmentId);
            });
        }
        else {
            ResetErrorDiv();
            $('#identificationtab').addClass('active').trigger('click');
            SuccessAlertSetting.text = getResourceValue("EquipmentSaveAlerts");
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change.select2');
                $('#wrEquipTreeModalHide').trigger('click');
                $(document).find('.mbsc-err').removeClass('mbsc-err');
                $(document).find('.mbsc-err-msg').html('');
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '.btnCancelAddEquipment', function () {
    //$("#AddEquipmentModalDialog").modal('hide');
    $('#AddEquipmentModalDialog').removeClass('slide-active');
});
function SetControls_Mobile() {
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

//#region Add QR scanner

//#region Parts QR Reader

var Equipment_textFieldID_Mobile = '', Equipment_valueFieldID_Mobile = '';
function QrScannerEquipment_Mobile(txtID, ValID) {
    Equipment_textFieldID_Mobile = '#' + txtID;
    Equipment_valueFieldID_Mobile = '';
    if (ValID != '') {
        Equipment_valueFieldID_Mobile = '#' + ValID;
    }
    if (!$(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').addClass("slide-active");
        StartQRReader_Mobile('Equipment');
    }
}
$(document).on('click', '#closeQrScanner_Mobile', function () {
    if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').removeClass('slide-active');
    }
    if (!$(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').removeClass('slide-active');
        StopCamera(); // using same method from somax_main.js
    }
});
function StartQRReader_Mobile(Module) {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 150,
                //aspectRatio: aspectratio //1.7777778
            }, success => {

                onScanSuccessEquipment_Mobile(success);

            }, error => {
            });
        } else {
            if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
                $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
            }
        }
    }).catch(e => {
        if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
            $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
        }
        if (e && e.startsWith('NotReadableError')) {
            ShowErrorAlert(getResourceValue("cameraIsBeingUsedByAnotherAppAlert"));
        }
        else if (e && e.startsWith('NotFoundError')) {
            ShowErrorAlert(getResourceValue("cameraDeviceNotFoundAlert"));
        }

    });
}


function detectMob() {
    var isMobile = false;
    if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
        || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) {
        return true;
    }
    return isMobile;
}

//#endregion

//#region Equipment qr scanner
function onScanSuccessEquipment_Mobile(decodedText) {
    $.ajax({
        url: "/WorkOrder/GetEquipmentIdByClientLookUpId?clientLookUpId=" + decodedText,
        type: "GET",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
                $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
            }
            if (data.EquipmentId > 0) {
                $(document).find(Equipment_textFieldID_Mobile).val('');
                $(document).find(Equipment_textFieldID_Mobile).val(decodedText).trigger('mbsc-enhance');//.removeClass('input-validation-error');
                $(document).find(Equipment_textFieldID_Mobile).closest('form').valid();
                $(document).find(Equipment_textFieldID_Mobile).parents('div').eq(0).css('display', 'block');
                if (Equipment_valueFieldID_Mobile != '') {
                    $(document).find(Equipment_valueFieldID_Mobile).val('');
                    $(document).find(Equipment_valueFieldID_Mobile).val(data.EquipmentId).trigger('change');
                    if ($(document).find(Equipment_valueFieldID_Mobile).css('display') == 'block') {
                        $(document).find(Equipment_valueFieldID_Mobile).parents('div').eq(0).css('display', 'none');
                    }
                    if ($(document).find(Equipment_valueFieldID_Mobile).parent().find('div > button.ClearAssetModalPopupGridData').length > 0) {
                        //Dynamic Pages Cross button to clear the textbox Value
                        $(document).find(Equipment_valueFieldID_Mobile).parent().find('div > button.ClearAssetModalPopupGridData').css('display', 'block');
                    }
                }
            }
            else {
                //Show Error Swal
                ShowErrorAlert(getResourceValue('spnInvalidQrCodeMsgforEquipment').replace('${decodedText}', decodedText));
            }
        },
        complete: function () {
            StopCamera();
            CloseLoader();
        },
        error: function (xhr) {
            ShowErrorAlert(getResourceValue("somethingWentWrongAlert"));
            CloseLoader();
        }
    });
}
//#endregion
//#endregion Add QR scanner

//#region Equipment Hierarchy ModalTree in Add Work Request Modal
var TextFieldId_ChargeTo = "";
var HdnfieldId_ChargeTo = "";
$(document).on('click', '#imgChargeToTreeGridOverWorkReqModal', function (e) {
    TextFieldId_ChargeTo = $(this).data('textfield');
    HdnfieldId_ChargeTo = $(this).data('valuefield');
    $(this).blur();
    generateWREquipmentTreeDynamic(-1);
});
function generateWREquipmentTreeDynamic(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/WorkOrderEquipmentHierarchyTreeDynamic',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        cache: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find(".cntTreeWR").html(data);
        },
        complete: function () {
            CloseLoader();
            $('#wrEquipTreeModal').addClass('slide-active');
            treeTable($(document).find(".cntTreeWR").find('#tblTree'));
            $(document).find('.radSelectWoDynamic').each(function () {
                if ($(document).find('#' + HdnfieldId_ChargeTo).val() == '0' || $(document).find('#' + HdnfieldId_ChargeTo) == '') {

                    if ($(this).data('equipmentid') === equipmentid) {
                        $(this).attr('checked', true);
                    }

                }
                else {

                    if ($(this).data('equipmentid') == $(document).find('#' + HdnfieldId_ChargeTo).val()) {
                        $(this).attr('checked', true);
                    }

                }

            });
            //-- V2-518 collapse all element
            // looking for the collapse icon and triggered click to collapse
            $.each($(document).find(".cntTreeWR").find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAAsTAAALEwEAmpwYAAAKT2lDQ1BQaG90b3Nob3AgSUNDIHByb2ZpbGUAAHjanVNnVFPpFj333vRCS4iAlEtvUhUIIFJCi4AUkSYqIQkQSoghodkVUcERRUUEG8igiAOOjoCMFVEsDIoK2AfkIaKOg6OIisr74Xuja9a89+bN/rXXPues852zzwfACAyWSDNRNYAMqUIeEeCDx8TG4eQuQIEKJHAAEAizZCFz/SMBAPh+PDwrIsAHvgABeNMLCADATZvAMByH/w/qQplcAYCEAcB0kThLCIAUAEB6jkKmAEBGAYCdmCZTAKAEAGDLY2LjAFAtAGAnf+bTAICd+Jl7AQBblCEVAaCRACATZYhEAGg7AKzPVopFAFgwABRmS8Q5ANgtADBJV2ZIALC3AMDOEAuyAAgMADBRiIUpAAR7AGDIIyN4AISZABRG8lc88SuuEOcqAAB4mbI8uSQ5RYFbCC1xB1dXLh4ozkkXKxQ2YQJhmkAuwnmZGTKBNA/g88wAAKCRFRHgg/P9eM4Ors7ONo62Dl8t6r8G/yJiYuP+5c+rcEAAAOF0ftH+LC+zGoA7BoBt/qIl7gRoXgugdfeLZrIPQLUAoOnaV/Nw+H48PEWhkLnZ2eXk5NhKxEJbYcpXff5nwl/AV/1s+X48/Pf14L7iJIEyXYFHBPjgwsz0TKUcz5IJhGLc5o9H/LcL//wd0yLESWK5WCoU41EScY5EmozzMqUiiUKSKcUl0v9k4t8s+wM+3zUAsGo+AXuRLahdYwP2SycQWHTA4vcAAPK7b8HUKAgDgGiD4c93/+8//UegJQCAZkmScQAAXkQkLlTKsz/HCAAARKCBKrBBG/TBGCzABhzBBdzBC/xgNoRCJMTCQhBCCmSAHHJgKayCQiiGzbAdKmAv1EAdNMBRaIaTcA4uwlW4Dj1wD/phCJ7BKLyBCQRByAgTYSHaiAFiilgjjggXmYX4IcFIBBKLJCDJiBRRIkuRNUgxUopUIFVIHfI9cgI5h1xGupE7yAAygvyGvEcxlIGyUT3UDLVDuag3GoRGogvQZHQxmo8WoJvQcrQaPYw2oefQq2gP2o8+Q8cwwOgYBzPEbDAuxsNCsTgsCZNjy7EirAyrxhqwVqwDu4n1Y8+xdwQSgUXACTYEd0IgYR5BSFhMWE7YSKggHCQ0EdoJNwkDhFHCJyKTqEu0JroR+cQYYjIxh1hILCPWEo8TLxB7iEPENyQSiUMyJ7mQAkmxpFTSEtJG0m5SI+ksqZs0SBojk8naZGuyBzmULCAryIXkneTD5DPkG+Qh8lsKnWJAcaT4U+IoUspqShnlEOU05QZlmDJBVaOaUt2ooVQRNY9aQq2htlKvUYeoEzR1mjnNgxZJS6WtopXTGmgXaPdpr+h0uhHdlR5Ol9BX0svpR+iX6AP0dwwNhhWDx4hnKBmbGAcYZxl3GK+YTKYZ04sZx1QwNzHrmOeZD5lvVVgqtip8FZHKCpVKlSaVGyovVKmqpqreqgtV81XLVI+pXlN9rkZVM1PjqQnUlqtVqp1Q61MbU2epO6iHqmeob1Q/pH5Z/YkGWcNMw09DpFGgsV/jvMYgC2MZs3gsIWsNq4Z1gTXEJrHN2Xx2KruY/R27iz2qqaE5QzNKM1ezUvOUZj8H45hx+Jx0TgnnKKeX836K3hTvKeIpG6Y0TLkxZVxrqpaXllirSKtRq0frvTau7aedpr1Fu1n7gQ5Bx0onXCdHZ4/OBZ3nU9lT3acKpxZNPTr1ri6qa6UbobtEd79up+6Ynr5egJ5Mb6feeb3n+hx9L/1U/W36p/VHDFgGswwkBtsMzhg8xTVxbzwdL8fb8VFDXcNAQ6VhlWGX4YSRudE8o9VGjUYPjGnGXOMk423GbcajJgYmISZLTepN7ppSTbmmKaY7TDtMx83MzaLN1pk1mz0x1zLnm+eb15vft2BaeFostqi2uGVJsuRaplnutrxuhVo5WaVYVVpds0atna0l1rutu6cRp7lOk06rntZnw7Dxtsm2qbcZsOXYBtuutm22fWFnYhdnt8Wuw+6TvZN9un2N/T0HDYfZDqsdWh1+c7RyFDpWOt6azpzuP33F9JbpL2dYzxDP2DPjthPLKcRpnVOb00dnF2e5c4PziIuJS4LLLpc+Lpsbxt3IveRKdPVxXeF60vWdm7Obwu2o26/uNu5p7ofcn8w0nymeWTNz0MPIQ+BR5dE/C5+VMGvfrH5PQ0+BZ7XnIy9jL5FXrdewt6V3qvdh7xc+9j5yn+M+4zw33jLeWV/MN8C3yLfLT8Nvnl+F30N/I/9k/3r/0QCngCUBZwOJgUGBWwL7+Hp8Ib+OPzrbZfay2e1BjKC5QRVBj4KtguXBrSFoyOyQrSH355jOkc5pDoVQfujW0Adh5mGLw34MJ4WHhVeGP45wiFga0TGXNXfR3ENz30T6RJZE3ptnMU85ry1KNSo+qi5qPNo3ujS6P8YuZlnM1VidWElsSxw5LiquNm5svt/87fOH4p3iC+N7F5gvyF1weaHOwvSFpxapLhIsOpZATIhOOJTwQRAqqBaMJfITdyWOCnnCHcJnIi/RNtGI2ENcKh5O8kgqTXqS7JG8NXkkxTOlLOW5hCepkLxMDUzdmzqeFpp2IG0yPTq9MYOSkZBxQqohTZO2Z+pn5mZ2y6xlhbL+xW6Lty8elQfJa7OQrAVZLQq2QqboVFoo1yoHsmdlV2a/zYnKOZarnivN7cyzytuQN5zvn//tEsIS4ZK2pYZLVy0dWOa9rGo5sjxxedsK4xUFK4ZWBqw8uIq2Km3VT6vtV5eufr0mek1rgV7ByoLBtQFr6wtVCuWFfevc1+1dT1gvWd+1YfqGnRs+FYmKrhTbF5cVf9go3HjlG4dvyr+Z3JS0qavEuWTPZtJm6ebeLZ5bDpaql+aXDm4N2dq0Dd9WtO319kXbL5fNKNu7g7ZDuaO/PLi8ZafJzs07P1SkVPRU+lQ27tLdtWHX+G7R7ht7vPY07NXbW7z3/T7JvttVAVVN1WbVZftJ+7P3P66Jqun4lvttXa1ObXHtxwPSA/0HIw6217nU1R3SPVRSj9Yr60cOxx++/p3vdy0NNg1VjZzG4iNwRHnk6fcJ3/ceDTradox7rOEH0x92HWcdL2pCmvKaRptTmvtbYlu6T8w+0dbq3nr8R9sfD5w0PFl5SvNUyWna6YLTk2fyz4ydlZ19fi753GDborZ752PO32oPb++6EHTh0kX/i+c7vDvOXPK4dPKy2+UTV7hXmq86X23qdOo8/pPTT8e7nLuarrlca7nuer21e2b36RueN87d9L158Rb/1tWeOT3dvfN6b/fF9/XfFt1+cif9zsu72Xcn7q28T7xf9EDtQdlD3YfVP1v+3Njv3H9qwHeg89HcR/cGhYPP/pH1jw9DBY+Zj8uGDYbrnjg+OTniP3L96fynQ89kzyaeF/6i/suuFxYvfvjV69fO0ZjRoZfyl5O/bXyl/erA6xmv28bCxh6+yXgzMV70VvvtwXfcdx3vo98PT+R8IH8o/2j5sfVT0Kf7kxmTk/8EA5jz/GMzLdsAAAAgY0hSTQAAeiUAAICDAAD5/wAAgOkAAHUwAADqYAAAOpgAABdvkl/FRgAAAHFJREFUeNpi/P//PwMlgImBQsA44C6gvhfa29v3MzAwOODRc6CystIRbxi0t7fjDJjKykpGYrwwi1hxnLHQ3t7+jIGBQRJJ6HllZaUUKYEYRYBPOB0gBShKwKGA////48VtbW3/8clTnBIH3gCKkzJgAGvBX0dDm0sCAAAAAElFTkSuQmCC"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find(".cntTreeWR").find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element

            $('#tblTree tr td').removeAttr('style');// code to remove the style applied from treetable.js -- white-space: nowrap;
        },
        error: function (xhr) {
            alert('error');
        }
    });
}

var equipmentid = 0;
$(document).on('change', ".radSelectWoDynamic", function () {

    var s = $(this).data;
    $(document).find('#' + HdnfieldId_ChargeTo).val('0');
    equipmentid = $(this).data('equipmentid');
    var clientlookupid = $(this).data('clientlookupid');
    var chargetoname = $(this).data('itemname');
    chargetoname = chargetoname.substring(0, chargetoname.length - 1);

 

    $(document).find('#' + TextFieldId_ChargeTo).val(clientlookupid).trigger('mbsc-enhance');
    $(document).find('#' + TextFieldId_ChargeTo).parents('div').eq(0).css("display", "block");
    $(document).find('#' + HdnfieldId_ChargeTo).val(equipmentid);//.removeClass('input-validation-error').trigger('change');
    $(document).find('#' + HdnfieldId_ChargeTo).closest('form').valid();
    $(document).find('#' + HdnfieldId_ChargeTo).parents('div').eq(0).css("display", "none");
    $(document).find('#' + HdnfieldId_ChargeTo).parent().find('div > button.ClearAssetModalPopupGridData').css('display', 'block');
    $('#wrEquipTreeModalHide').trigger('click');
    //    }
    //});
    //commented ajax call not sure why it is called
});
$(document).on('click', '#wrEquipTreeModalHide', function () {
    $('#wrEquipTreeModal').removeClass('slide-active');
    TextFieldId_ChargeTo = "";
    HdnfieldId_ChargeTo = "";
});
//#endregion

//region Edit equipment
$(document).on('click', "#equipmentedit", function () {
    var equipmentid = LRTrim($(document).find('#EquipData_EquipmentId').val());
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var Name = $('#EquipData_Name').val();
    var Status = $('#EquipData_Status').val();
    var isRemoveFromService = $('#EquipData_RemoveFromService').val();
    $.ajax({
        url: '/Equipment/EditEquipmentDynamic_Mobile',
        data: { EquipmentId: equipmentid, ClientlookUpId: ClientlookUpId, Name: Name, isRemoveFromService: isRemoveFromService, Status: Status },
        type: "POST",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#MobileEquipmentmaincontainer').html(data);

        },
        complete: function () {
            CloseLoader();
            SetControls_Mobile();
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
                //min: 0.01,
                //max: 99999999.99,
                scale: 2,
                preset: 'decimal',
                thousandsSeparator: ''
            });
            $('#EditEquipmentModalDialog').addClass('slide-active').trigger('mbsc-enhance');
        }
    });
});


function EquipmentEditDynamicOnSuccess_Mobile(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("EquipmentUpdateAlert");
        swal(SuccessAlertSetting, function () {
            RedirectionToDetails_Mobile(data.equipmentid);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', '.btnCancelEditEquipment', function () {
    var equipmentid = LRTrim($(document).find('#EditEquipment_EquipmentId').val());
    //$("#AddEquipmentModalDialog").modal('hide');
    /*$('#EditEquipmentModalDialog').removeClass('slide-active');*/
    RedirectionToDetails_Mobile(equipmentid);
});

//endregion

//#region V2-919
//#region Attachment
function LoadAttachments() {
    $.ajax({
        url: '/Equipment/LoadAttachments_Mobile ',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Attachment').html(data);
            generateAttachmentDataTable();
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '.delAttchBttn', function () {
    var data = dtAttachmentTable.row($(this).parents('tr')).data();
    DeleteEquipmentAttachment(data.FileAttachmentId);
});
function DeleteEquipmentAttachment(fileAttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Equipment/DeleteAttachment',
            data: {
                _fileAttachmentId: fileAttachmentId
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "POST",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    dtAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                generateAttachmentDataTable();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnAddAttachment", function () {
    var equipmentid = $(document).find('#EquipData_EquipmentId').val();
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var Name = $('#EquipData_Name').val();
    var Status = $('#EquipData_Status').val();
    var isRemoveFromService = $('#EquipData_RemoveFromService').val();
    $.ajax({
        url: "/Equipment/ShowAddAttachment_Mobile",
        type: "GET",
        dataType: 'html',
        data: { EquipmentId: equipmentid, ClientlookUpId: ClientlookUpId, Name: Name, isRemoveFromService: isRemoveFromService, Status: Status },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AttachmentPopUp').html(data);
        },
        complete: function () {
            SetControls_Mobile();
            BindMobiscrollControlForAttachmentTab();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function BindMobiscrollControlForAttachmentTab() {
    $('#AddAttachmentModalpopup').parent().addClass("slide-active").trigger('mbsc-enhance');
}
$(document).on('click', ".Attachmentclearstate", function () {
    $('#AddAttachmentModalpopup').parent().removeClass("slide-active");
});
function generateAttachmentDataTable() {
    var equipmentid = $(document).find('#EquipData_EquipmentId').val();
    if ($(document).find('#AttachTable').hasClass('dataTable')) {
        dtAttachmentTable.destroy();
    }
    var visibility;
    var attchCount = 0;
    dtAttachmentTable = $("#AttachTable").DataTable({
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

            "url": "/Equipment/PopulateAttachment_Mobile?EquipmentId=" + equipmentid,
            "type": "post",
            "datatype": "json",
            "dataSrc": function (response) {
                attchCount = response.recordsTotal;
                visibility = response.Attachsecurity;
                if (attchCount > 0) {
                    $(document).find('#asstAttachmentCount').show();
                    $(document).find('#asstAttachmentCount').html(attchCount);
                }
                else {
                    $(document).find('#asstAttachmentCount').hide();
                }
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    if (visibility == true) {
                        return '<a class="btn btn-outline-danger delAttchBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                }
            }
        ],
        "columns":
            [
                {
                    "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "FileName",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_download_attachment href="javascript:void(0)"  target="_blank">' + row.FullName + '</a>';
                    }
                },
                { "data": "FileSizeWithUnit", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CreateDate", "type": "date " },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            if (visibility == false) {
                $("#btnAddAttachment").hide();
                var column = this.api().column(5);
                column.visible(false);
            }
            else {
                $("#btnAddAttachment").show();
                var column = this.api().column(5);
                column.visible(true);
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('submit', "#frmeqpattachmentadd", function (e) {
    e.preventDefault();
    var form = document.querySelector('#frmeqpattachmentadd');
    if (!$('#frmeqpattachmentadd').valid()) {
        return;
    }
    var data = new FormData(form);
    $.ajax({
        type: "POST",
        url: "/Equipment/AddAttachment",
        data: data,
        processData: false,
        contentType: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                var EquipmentId = data.equipmentid;
                if (data.IsduplicateAttachmentFileExist) {
                    ShowErrorAlert(getResourceValue("AttachmentFileExistAlerts"));
                }
                else {
                    SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
                    swal(SuccessAlertSetting, function () {
                        $('#AddAttachmentModalpopup').parent().removeClass("slide-active");
                        generateAttachmentDataTable();
                    });
                }

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
$(document).on('click', '.lnk_download_attachment', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = dtAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/Equipment/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });

});
//#endregion

//#region Tech Spec
function LoadTechSpecs() {
    $.ajax({
        url: '/Equipment/LoadTechSpecs_Mobile ',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#TechSpecs').html(data);
            generateTechSpecsTable();
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function generateTechSpecsTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var rCount = 0;
    var visibility;
    var techSpecsSecurity;
    if ($(document).find('#techSpecsTable').hasClass('dataTable')) {
        techSpecsTable.destroy();
    }
    techSpecsTable = $("#techSpecsTable").DataTable({
        responsive: true,
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
            "url": "/Equipment/GetEquipment_TechSpecs_Mobile",
            "type": "POST",
            data: { EquipmentId: EquipmentId },
            "datatype": "json",
            "cache": false,
            "dataSrc": function (json) {
                rCount = json.data.length;
                techSpecsSecurity = json.techSpecsSecurity;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    if (techSpecsSecurity == true) {
                        return '<a class="btn btn-outline-primary addBtnTechSpecs gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editBttnTechSpecs gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delBtnTechSpecs gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "SpecValue", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "Comments", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    data: "TechSpecId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            if (techSpecsSecurity == true) {
                var column = this.api().column(4);
                column.visible(true);
            }
            else {
                var column = this.api().column(4);
                column.visible(false);
            }
            if (rCount > 0 || techSpecsSecurity == false) { $("#btnAddTechSpecs").hide(); }
            else {
                $("#btnAddTechSpecs").show();
            }
            SetPageLengthMenu();
        }
    });
}
function BindMobiscrollControlForTechSpecsTab() {
    $('#AddTechSpecsModalpopup').parent().addClass("slide-active").trigger('mbsc-enhance');
}
$(document).on('click', ".TechSpecsclearstate", function () {
    $('#AddTechSpecsModalpopup').parent().removeClass("slide-active");
});
$(document).on('click', '#btnAddTechSpecs', function (e) {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    AddTechSpecs(0, EquipmentId, 0, 0, 'add');
});
$(document).on('click', '.addBtnTechSpecs', function () {
    var data = techSpecsTable.row($(this).parents('tr')).data();
    AddTechSpecs(data.TechSpecId, data.EquipmentId, data.Equipment_TechSpecsId, data.updatedindex, "add");
});
$(document).on('click', '.editBttnTechSpecs', function () {
    var data = techSpecsTable.row($(this).parents('tr')).data();
    AddTechSpecs(data.TechSpecId, data.EquipmentId, data.Equipment_TechSpecsId, data.updatedindex, "update");
});
$(document).on('click', '.delBtnTechSpecs', function () {
    var data = techSpecsTable.row($(this).parents('tr')).data();
    DeleteTechSpecs(data.Equipment_TechSpecsId);
});
function TechSpecAddOnSuccess(data) {
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AddTechspecsAlerts");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("UpdateTechspecsAlerts");
        }
        swal(SuccessAlertSetting, function () {
            $('#AddTechSpecsModalpopup').parent().removeClass("slide-active");
            generateTechSpecsTable();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function AddTechSpecs(techSpecId, eqid, equipTechSpecId, updatedindex, mode) {
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var Name = $('#EquipData_Name').val();
    var Status = $('#EquipData_Status').val();
    var isRemoveFromService = $('#EquipData_RemoveFromService').val();
    $.ajax({
        url: '/Equipment/AddTechSpecs_Mobile',
        data: { EquipmentId: eqid, TechMode: mode, TechSpecId: equipTechSpecId, ClientlookUpId: ClientlookUpId, Name: Name, isRemoveFromService: isRemoveFromService, Status: Status },
        type: "GET",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#TechSpecsPopUp').html(data);
        },
        complete: function () {
            SetControls_Mobile();
            BindMobiscrollControlForTechSpecsTab();
        },
        error: function () {
        }
    });
}
function DeleteTechSpecs(eqid) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Equipment/TechSpecsDelete',
            data: {
                _EquipmentTechSpecsId: eqid
            },
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    techSpecsTable.state.clear;
                    ShowDeleteAlert(getResourceValue("techspecsDeleteSuccessAlert"));
                }
                else {
                    techSpecsTable.state.clear;
                    generateTechSpecsTable();
                    return;
                }
            },
            complete: function () {
                generateTechSpecsTable();
                CloseLoader();
            }
        });
    });
}
//#endregion

//#region Parts
function LoadParts() {
    $.ajax({
        url: '/Equipment/LoadParts_Mobile ',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Parts').html(data);
            generatePartsDataTable();
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function generatePartsDataTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#partsTable').hasClass('dataTable')) {
        dtPartsTable.destroy();
    }
    dtPartsTable = $("#partsTable").DataTable({
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
            "url": "/Equipment/GetEquipment_Parts_Mobile",
            data: function (d) {
                d.EquipmentId = EquipmentId;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                visibility = json.partSecurity;
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [5],
                render: function (a, b, data, d) {
                    if (visibility == true) {
                        return '<a class="btn btn-outline-primary addBtnParts gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editBttnParts gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delBtnParts gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                {
                    "data": "Part_ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=link_part_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Part_Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "QuantityNeeded", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "QuantityUsed", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Comment", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "EquipmentId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            var column = this.api().column(5);
            if (visibility == false) {
                column.visible(false);
            }
            else {
                column.visible(true);
            }
            if (rCount > 0 || visibility == false) { $("#btnAddParts").hide(); }
            else {
                $("#btnAddParts").show();
            }
            SetPageLengthMenu();
        }
    });
}
function BindMobiscrollControlForPartsTab() {
    $('#AddPartsModalpopup').parent().addClass("slide-active").trigger('mbsc-enhance');
    $('#partsSessionData_QuantityNeeded').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 999999999.999999,
        //fill: 'ltr',
        maxScale: 6,
        preset: 'decimal',
        thousandsSeparator: '',
        entryMode: 'freeform'
    });
    var x = parseFloat($('#partsSessionData_QuantityNeeded').val()) == 0 ? '' : $('#partsSessionData_QuantityNeeded').val();
    $('#partsSessionData_QuantityNeeded').mobiscroll('setVal', x);
    $('#partsSessionData_QuantityUsed').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 999999999.999999,
        //fill: 'ltr',
        maxScale: 6,
        preset: 'decimal',
        thousandsSeparator: '',
        entryMode: 'freeform'
    });
    var x = parseFloat($('#partsSessionData_QuantityUsed').val()) == 0 ? '' : $('#partsSessionData_QuantityUsed').val();
    $('#partsSessionData_QuantityUsed').mobiscroll('setVal', x);
}
$(document).on('click', ".Partsclearstate", function () {
    $('#AddPartsModalpopup').parent().removeClass("slide-active");
});
$(document).on('click', '#btnAddParts', function (e) {
    var equipmentid = $(document).find('#EquipData_EquipmentId').val();
    AddParts(equipmentid);
});
$(document).on('click', '.addBtnParts', function () {
    var data = dtPartsTable.row($(this).parents('tr')).data();
    AddParts(data.EquipmentId);
});
$(document).on('click', '.editBttnParts', function () {
    var data = dtPartsTable.row($(this).parents('tr')).data();
    EditParts(data.EquipmentId, data.Equipment_Parts_XrefId, encodeURIComponent(data.Part_ClientLookupId), data.QuantityNeeded, data.QuantityUsed, (encodeURIComponent(data.Comment)).replace(/%20/g, "&#32;"), data.UpdatedIndex, "update");
});
$(document).on('click', '.delBtnParts', function () {
    var data = dtPartsTable.row($(this).parents('tr')).data();
    DeleteParts(data.Equipment_Parts_XrefId);
});
function AddParts(eqid) {
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var Name = $('#EquipData_Name').val();
    var Status = $('#EquipData_Status').val();
    var isRemoveFromService = $('#EquipData_RemoveFromService').val();
    $.ajax({
        url: '/Equipment/PartsAdd_Mobile',
        data: { EquipmentId: eqid, ClientlookUpId: ClientlookUpId, Name: Name, isRemoveFromService: isRemoveFromService, Status: Status },
        type: "GET",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#PartsPopUp').html(data);
        },
        complete: function () {
            SetControls_Mobile();
            BindMobiscrollControlForPartsTab();
        }
    });
}
function PartAddOnSuccess(data) {
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("AddPartsAlerts");
        swal(SuccessAlertSetting, function () {
            $('#AddPartsModalpopup').parent().removeClass("slide-active");
            generatePartsDataTable();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditParts(eqid, Equipment_Parts_XrefId, Part_ClientLookupId, QuantityNeeded, QuantityUsed, Comment, UpdatedIndex) {
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var Name = $('#EquipData_Name').val();
    var Status = $('#EquipData_Status').val();
    var isRemoveFromService = $('#EquipData_RemoveFromService').val();
    $.ajax({
        url: '/Equipment/PartsEdit_Mobile',
        data: { EquipmentId: eqid, Equipment_Parts_XrefId: Equipment_Parts_XrefId, ClientlookUpId: ClientlookUpId, Name: Name, isRemoveFromService: isRemoveFromService, Status: Status },
        type: "GET",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#PartsPopUp').html(data);
        },
        complete: function () {
            SetControls_Mobile();
            BindMobiscrollControlForPartsTab();
        }
    });
};
function PartsEditOnSuccess(data) {
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("UpdatePartsAlerts");
        swal(SuccessAlertSetting, function () {
            $('#AddPartsModalpopup').parent().removeClass("slide-active");
            generatePartsDataTable();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function DeleteParts(eqid) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Equipment/PartsDelete',
            data: { _EquipmentPartSpecsId: eqid },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("partDeleteSuccessAlert"));
                }
            },
            complete: function () {
                dtPartsTable.state.clear;
                generatePartsDataTable();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', '.link_part_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtPartsTable.row(row).data();
    var partId = data.PartId;
    var equipmentId = data.EquipmentId; //V2-1007
    clearDropzone();
    var UseMultiStoreroom = $(document).find('#UseMultiStoreroom').val();
    if (UseMultiStoreroom == 'False')
        window.location.href = "../Parts/PartsDetailFromEquipment?partId=" + partId + '&equipmentId=' + equipmentId; //V2-1007
    else
        window.location.href = "../MultiStoreroomPart/MultiStoreroomPartsDetailFromEquipment?partId=" + partId + '&equipmentId=' + equipmentId; //V2-1007
});
//#region Part List
$(document).on('click', '#openpartgrid', function () {
    generatePartScrollViewForMobileMobiscroll();
});
function generatePartScrollViewForMobileMobiscroll() {
    PartListlength = 0;
 
    $.ajax({
        "url": "/Equipment/PartLookupListView_Mobile",
        type: "POST",
        dataType: "html",
       
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#DivPartSearchScrollViewModal').html(data);
        },
        complete: function () {
         
            InitializePartListView_Mobile();
         
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
var PartListView,
    PartListlength = 0,
    PartPageLength = 100;
function InitializePartListView_Mobile() {
    PartListView = $('#PartListViewForSearch').mobiscroll().listview({
        theme: 'ios',
        themeVariant: 'light',
        animateAddRemove: false,
        striped: true,
        swipe: false
    }).mobiscroll('getInst');
    BindPartDataForListView();
    $('#maintenancepartIdModal').addClass('slide-active');
}
$(document).on('click', '#btnPartLoadMore', function () {
    $(this).hide();
    PartListlength += PartPageLength;
    InitializePartListView_Mobile();
});
function BindPartDataForListView() {
    var Search = $(document).find('#txtPartSearch_Mobile').val();
    $.ajax({
        "url": "/Equipment/GetPartLookupListchunksearch_Mobile",
        data: {
            Search: Search,
            Start: PartListlength,
            Length: PartPageLength,
           
        },
        type: 'POST',
        dataType: 'JSON',
        beforeSend: function () {
            ShowLoader();
            PartListView.showLoading();
        },
        success: function (data) {
            var i, item, lihtml;
            for (i = 0; i < data.data.length; ++i) {
                item = data.data[i];
                lihtml = '';
                lihtml = '<li class="scrollview-partsearch" data-id="' + item.PartId + '"; data-clientlookupid="' + item.ClientLookUpId + '">';
                lihtml = lihtml + "" + item.ClientLookUpId + ' (' + item.Description + ')</li>';
                PartListView.add(null, mobiscroll.$(lihtml));
            }
            if ((PartListlength + PartPageLength) < data.recordsTotal) {
                $('#btnPartLoadMore').show();
            }
            else {
                $('#btnPartLoadMore').hide();
            }
        },
        complete: function () {
            PartListView.hideLoading();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', '#maintenancepartIdModalHide', function () {
    $(document).find('#maintenancepartIdModal').removeClass("slide-active");
    $('#txtPartSearch_Mobile').val('');
    $('#DivPartSearchScrollViewModal').html('');
});
$(document).on("keyup", '#txtPartSearch_Mobile', function (e) {
    if (e.keyCode == 13) {
        generatePartScrollViewForMobileMobiscroll();
    }
});

$(document).on('click', '.scrollview-partsearch', function (e) {
    $(document).find('#txtpartid').val($(this).data('clientlookupid')).trigger('mbsc-enhance');
    $(document).find('#txtpartid').closest('form').valid();
    $(document).find('#partsSessionData_Part').val($(this).data('clientlookupid'));
    $(document).find('#maintenancepartIdModal').removeClass("slide-active");
});
//#endregion
//#endregion

//#region Down Time
function LoadDowntime() {
    $.ajax({
        url: '/Equipment/LoadDowntime_Mobile ',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Downtime').html(data);
            generateDowntimeDataTable();
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function generateDowntimeDataTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var rCount = 0;
    var visibilityCreate;
    var visibilityEdit;
    var visibilityDelete;
    if ($(document).find('#downtimeTable').hasClass('dataTable')) {
        dtDownTimeTable.destroy();
    }
    dtDownTimeTable = $("#downtimeTable").DataTable({
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
            "url": "/Equipment/GetEquipment_Downtime_Mobile",
            "type": "POST",
            data: { EquipmentId: EquipmentId },
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                visibilityCreate = json.secDownTimeAdd;
                visibilityEdit = json.secDownTimeEdit;
                visibilityDelete = json.secDownTimeDelete;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    var actionButtonhtml = "";
                    if (visibilityCreate == true) {
                        actionButtonhtml = '<a class="btn btn-outline-primary addEquipmentDownTimeBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>';
                        if (visibilityEdit == true) {
                            actionButtonhtml = actionButtonhtml + '<a class="btn btn-outline-success editEquipmentDownTimeBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                        }
                    }
                    if (visibilityDelete == true) {
                        actionButtonhtml = actionButtonhtml + '<a class="btn btn-outline-danger deleteEquipmentDownTimeBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    return actionButtonhtml;
                }
            }
        ],
        "columns":
            [
                { "data": "WorkOrderClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "DateDown",
                    "type": "date "
                },
                { "data": "MinutesDown", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": "text-right" },
                { "data": "ReasonForDownDescription", "autoWidth": true, "bSearchable": true, "bSortable": true },//V2-695
                { "data": "EquipmentId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        "footerCallback": function (row, data, start, end, display) {

            var api = this.api();
            var rows = api.rows().nodes();
            var getData = api.rows({ page: 'current' }).data();
            // Total over all pages
            total = api.column(2).data().reduce(function (a, b) {
                return parseFloat(a) + parseFloat(b);
            }, 0);
            // Update footer
            $("#downtimeTablefoot").empty();
            if (data.length != 0) {
                var footer = "";
                if (end == getData[0].TotalCount) {
                    footer = '<tr><th></th><th style="text-align: left !important; font-weight: 500 !important; color: #0b0606 !important">Grand Total</th><th style="text-align: right !important; font-weight: 500 !important; color: #0b0606 !important; padding: 0px 10px 0px 0px !important">' + getData[0].TotalMinutesDown.toFixed(4) + '</th><th></th> <th></th></tr>'
                    $("#downtimeTablefoot").empty().append(footer);
                }
            }
        },
        initComplete: function () {
            if (visibilityCreate == false && visibilityDelete == false) {
                var column = this.api().column(4);
                column.visible(false);
            }
            else {
                var column = this.api().column(4);
                column.visible(true);
            }
            if (rCount > 0 || visibilityCreate == false) { $("#btnAddDownTime").hide(); }
            else {
                $("#btnAddDownTime").show();
            }
            SetPageLengthMenu();
        },

    });
}
function BindMobiscrollControlForDowntimeTab() {
    $('#AddDowntimeModalpopup').parent().addClass("slide-active").trigger('mbsc-enhance');
    $('#downTimeModel_MinutesDown').mobiscroll().numpad({
        //touchUi: true,
        min: 0.0001,
        max: 99999999999.9999,
        //fill: 'ltr',
        maxScale: 4,
        preset: 'decimal',
        thousandsSeparator: '',
        entryMode: 'freeform'
    });
    var x = parseFloat($('#downTimeModel_MinutesDown').val()) == 0 ? '' : $('#downTimeModel_MinutesDown').val();
    $('#downTimeModel_MinutesDown').mobiscroll('setVal', x);
    $('#downTimeModel_DateDown').mobiscroll().calendar({
        display: 'bottom',
        touchUi: true,
        weekDays: 'min',
        yearChange: false,
        min: new Date(),
        months: 1
    }).inputmask('mm/dd/yyyy');
}
$(document).on('click', ".Downtimeclearstate", function () {
    $('#AddDowntimeModalpopup').parent().removeClass("slide-active");
});
$(document).on('click', '.addEquipmentDownTimeBttn', function () {
    var data = dtDownTimeTable.row($(this).parents('tr')).data();
    AddDowntime(data.EquipmentId);
});
$(document).on('click', '#btnAddDownTime', function (e) {
    var equipmentid = $(document).find('#EquipData_EquipmentId').val();
    AddDowntime(equipmentid);
});
$(document).on('click', '.editEquipmentDownTimeBttn', function () {
    var data = dtDownTimeTable.row($(this).parents('tr')).data();
    EditDowntime(data.DowntimeId);
});
$(document).on('click', '.deleteEquipmentDownTimeBttn', function () {
    var data = dtDownTimeTable.row($(this).parents('tr')).data();
    DeleteDowntime(data.DowntimeId);
});
function AddDowntime(eqid) {
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var Name = $('#EquipData_Name').val();
    var Status = $('#EquipData_Status').val();
    var isRemoveFromService = $('#EquipData_RemoveFromService').val();
    $.ajax({
        url: '/Equipment/DownTimeAdd_Mobile',
        data: { EquipmentId: eqid, ClientlookUpId: ClientlookUpId, Name: Name, isRemoveFromService: isRemoveFromService, Status: Status },
        type: "GET",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#DowntimePopUp').html(data);
        },
        complete: function () {
            SetControls_Mobile();
            BindMobiscrollControlForDowntimeTab();
        }
    });
}
function DownTimeAddOnSuccess(data) {
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("alertDownTimeAdded");
        swal(SuccessAlertSetting, function () {
            $('#AddDowntimeModalpopup').parent().removeClass("slide-active");
            generateDowntimeDataTable();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditDowntime(DowntimeId) {
    var EquimentId = $('#EquipData_EquipmentId').val();
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var Name = $('#EquipData_Name').val();
    var Status = $('#EquipData_Status').val();
    var isRemoveFromService = $('#EquipData_RemoveFromService').val();
    $.ajax({
        url: '/Equipment/ShowDownTimeEdit_Mobile',
        data: { EquipmentId: EquimentId, DownTimeId: DowntimeId, ClientlookUpId: ClientlookUpId, Name: Name, isRemoveFromService: isRemoveFromService, Status: Status },
        type: "POST",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#DowntimePopUp').html(data);
        },
        complete: function () {
            SetControls_Mobile();
            BindMobiscrollControlForDowntimeTab();
        },
        error: function (jqXHR, exception) {
        }
    });
}
function DeleteDowntime(DowntimeId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Equipment/DownTimeDelete',
            data: {
                _DowntimeId: DowntimeId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("downtimeDeleteSuccessAlert"));
                    dtDownTimeTable.state.clear;
                }
                else {
                    dtDownTimeTable.destroy();
                    generateDowntimeDataTable();
                    return;
                }
            },
            complete: function () {
                generateDowntimeDataTable();
                CloseLoader();
            }
        });
    });
}
function DownTimeEditOnSuccess(data) {
    SuccessAlertSetting.text = getResourceValue("UpdateDowntimeAlerts");
    if (data.Result == "success") {
        swal(SuccessAlertSetting, function () {
            $('#AddDowntimeModalpopup').parent().removeClass("slide-active");
            generateDowntimeDataTable();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion

//#region PMList
function LoadPMList() {
    $.ajax({
        url: '/Equipment/LoadPMList_Mobile ',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#PMList').html(data);
            generatePMDataTable();
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function generatePMDataTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var visibility;
    if ($(document).find('#pmTable').hasClass('dataTable')) {
        dtPMTable.destroy();
    }
    dtPMTable = $("#pmTable").DataTable({
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
            "url": "/Equipment/GetEquipment_PMList_Mobile",
            "type": "POST",
            data: { EquipmentId: EquipmentId },
            "datatype": "json",
            "dataSrc": function (json) {
                visibility = json.pmSecurity;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    if (visibility == true) {
                        return '<a class="btn btn-outline-success btnPmListEdit gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                {
                    "data": "ClientLookupId",
                    "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return '<a class=lnk_preventivemaintenancedetails href="javascript:void(0)">' + data + "</a>";
                    }
                },
                {
                    "data": "Description",
                    "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-400'>" + data + "</div>";
                    }
                },
                { "data": "LastScheduled", "type": "date " },
                { "data": "LastPerformed", "type": "date " },
                { "data": "AssignedTo_PersonnelName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PrevMaintMasterId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            if (visibility == true) {
                var column = this.api().column(5);
                column.visible(true);
            }
            else {
                var column = this.api().column(5);
                column.visible(false);
            }
            SetPageLengthMenu();
        }
    });
}
//#region V2-1008 PM List Link Details
$(document).on('click', '.lnk_preventivemaintenancedetails', function (e) {
    var row = $(this).parents('tr');
    var data = dtPMTable.row(row).data();
    var equipmentid = LRTrim($(document).find('#EquipData_EquipmentId').val());
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var PrevMasterId = data.PrevMaintMasterId;
    window.location.href = "../PreventiveMaintenance/DetailFromEquipment?PrevMaintMasterId=" + PrevMasterId + "&EquipmentId=" + equipmentid + "&EquipmentClientLookupId=" + ClientlookUpId;
});
//#endregion
$(document).on('click', '.btnPmListEdit', function () {
    var equipmentid = LRTrim($(document).find('#EquipData_EquipmentId').val());
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var row = $(this).parents('tr');
    var data = dtPMTable.row(row).data();
    var PrevMasterId = data.PrevMaintMasterId;
    clearDropzone();
    window.location.href = "../PreventiveMaintenance/DetailFromEquipment?PrevMaintMasterId=" + PrevMasterId + "&EquipmentId=" + equipmentid + "&EquipmentClientLookupId=" + ClientlookUpId;
});
//#endregion

//#region WOActive
function LoadWOActive() {
    $.ajax({
        url: '/Equipment/LoadWOActive_Mobile ',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#WOActive').html(data);
            generateWOActiveDataTable();
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function generateWOActiveDataTable() {
    var srcData = LRTrim($("#txtWTAsearchbox").val());
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var ClientLookupId = LRTrim($("#wgridActiveadvsearchWorkOrderId").val());
    var Description = LRTrim($("#wgridActiveadvsearchDescription").val());
    var WorkAssigned_PersonnelClientLookupId = LRTrim($("#wgridActiveadvsearchWorkAssigned").val());
    var Status_Display = LRTrim($("#wgridActiveadvsearchStatus").val());
    var Type = LRTrim($("#wgridActiveadvsearchType").val());
    var CreateDate = LRTrim($('#wgridActiveadvsearchCreateDate').val());
    if ($(document).find('#woActiveTable').hasClass('dataTable')) {
        dtWOActiveTable.destroy();
    }
    dtWOActiveTable = $("#woActiveTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Equipment/GetEquipment_WOActive_Mobile",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.EquipmentId = EquipmentId;
            },
            "dataSrc": function (json) {

                $("#wgridActiveadvsearchStatus").empty();
                $("#wgridActiveadvsearchStatus").append("<option value=''>" + "--Select--" + "</option>");
                var status = [];
                for (var key in json.data) {
                    if (status.indexOf(json.data[key].Status_Display) == -1) {
                        status.push(json.data[key].Status_Display);
                    }
                }
                for (statusVal in status) {
                    var name = status[statusVal];
                    $("#wgridActiveadvsearchStatus").append("<option value='" + name + "'>" + getStatusValue(name) + "</option>");
                }
                if (Status_Display) {
                    $("#wgridActiveadvsearchStatus").val(Status_Display);
                }

                return json.data;
            }
        },
        "columns":
            [
                {
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_workorderactive href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-400'>" + data + "</div>";
                    }
                },
                { "data": "WorkAssigned_PersonnelClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Status_Display", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    render: function (data, type, row, meta) {

                        if (data == statusCode.Approved) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--yellow m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Canceled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--orange m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Complete) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Denied) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--purple m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Scheduled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.WorkRequest) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--red m-badge--wide' style='width:95px;' >" + getStatusValue(data) + "</span >";
                        }
                        else {
                            return getStatusValue(data);
                        }
                    }
                },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}

$(document).on('click', '.lnk_workorderactive', function (e) {
    var row = $(this).parents('tr');
    var data = dtWOActiveTable.row(row).data();
    var workorderId = data.WorkOrderId;
    clearDropzone();
    localStorage.setItem("equipmentSearchstatus", titleText);
    localStorage.setItem("workorderstatus", '0');
    localStorage.setItem("workorderstatustext", getResourceValue("spnWorkOrder"));
    window.location.href = "../WorkOrder/DetailFromEquipment?workOrderId=" + workorderId;
});
//#endregion

//#region WOComplete
function LoadWOComplete() {
    $.ajax({
        url: '/Equipment/LoadWOComplete_Mobile ',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#WOComplete').html(data);
            generateWOCompleteTable();
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function generateWOCompleteTable() {
    var srcData = LRTrim($("#txtWTOsearchbox").val());
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var ClientLookupId = LRTrim($("#wgridCompleteadvsearchWorkOrderId").val());
    var Description = LRTrim($("#wgridCompleteadvsearchDescription").val());
    var WorkAssigned_PersonnelClientLookupId = LRTrim($("#wgridCompleteadvsearchWorkAssigned").val());
    var Status_Display = LRTrim($("#wgridCompleteadvsearchStatus").val());
    var Type = LRTrim($("#wgridCompleteadvsearchType").val());
    var CreateDate = LRTrim($('#wgridCompleteadvsearchCreateDate').val());
    if ($(document).find('#woCompleteTable').hasClass('dataTable')) {
        dtWOCompleteTable.destroy();
    }
    dtWOCompleteTable = $("#woCompleteTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Equipment/GetEquipment_WOComplete_Mobile",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.EquipmentId = EquipmentId;
            },
            "dataSrc": function (json) {
                $("#wgridCompleteadvsearchStatus").empty();
                $("#wgridCompleteadvsearchStatus").append("<option value=''>" + "--Select--" + "</option>");
                var status = [];
                for (var key in json.data) {
                    if (status.indexOf(json.data[key].Status_Display) == -1) {
                        status.push(json.data[key].Status_Display);
                    }
                }
                for (statusVal in status) {
                    var name = status[statusVal];
                    $("#wgridCompleteadvsearchStatus").append("<option value='" + name + "'>" + getStatusValue(name) + "</option>");
                }
                if (Status_Display) {
                    $("#wgridCompleteadvsearchStatus").val(Status_Display);
                }
                return json.data;
            }
        },
        "columns":
            [
                {
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_workordercomplete href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-400'>" + data + "</div>";
                    }
                },
                { "data": "WorkAssigned_PersonnelClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Status_Display", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    render: function (data, type, row, meta) {

                        if (data == statusCode.Approved) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--yellow m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Canceled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--orange m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Complete) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Denied) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--purple m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Scheduled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.WorkRequest) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--red m-badge--wide' style='width:95px;' >" + getStatusValue(data) + "</span >";
                        }
                        else {
                            return getStatusValue(data);
                        }
                    }
                },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}

$(document).on('click', '.lnk_workordercomplete', function (e) {
    var row = $(this).parents('tr');
    var data = dtWOCompleteTable.row(row).data();
    var workorderId = data.WorkOrderId;
    localStorage.setItem("equipmentSearchstatus", titleText);
    localStorage.setItem("workorderstatus", '4');
    localStorage.setItem("workorderstatustext", getResourceValue("spnCompleteWorkOrder"));
    window.location.href = "../WorkOrder/DetailFromEquipment?workOrderId=" + workorderId;
});
//#endregion

//#region PartIssues
function LoadPartIssues() {
    $.ajax({
        url: '/Equipment/LoadPartIssues_Mobile ',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#PartIssues').html(data);
            generatePartIssueTable();
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function generatePartIssueTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var srcData = LRTrim($("#txtPartIssueSearchbox").val());
    var PartClientLookupId = LRTrim($("#piGridadvsearchPartId").val());
    var Description = LRTrim($("#piGridadvsearchDescription").val());
    var ChargeToClientLookupId = LRTrim($("#piGridadvsearchChargeTo").val());
    var UnitofMeasure = LRTrim($("#piGridadvsearchUnit").val());
    var IssuedTo = LRTrim($("#piGridadvsearchIssuedTo").val());
    var TransactionDate = LRTrim($("#piGridadvsearchDate").val());
    var TransactionQuantity = LRTrim($("#piGridadvsearchQuantity").val());
    var Cost = LRTrim($("#piGridadvsearchCosts").val());
    if ($(document).find('#partIssuesTable').hasClass('dataTable')) {
        dtPartIssueTable.destroy();
    }
    dtPartIssueTable = $("#partIssuesTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Equipment/GetEquipment_PartIssued_Mobile",
            "type": "POST",
            data: function (d) {
                d.EquipmentId = EquipmentId;
            },
            "datatype": "json",
            "dataSrc": function (json) {
                return json.data;
            }
        },
        "columns":
            [
                { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-400'>" + data + "</div>";
                    }
                },
                { "data": "TransactionDate", "type": "date ", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ChargeToClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "TransactionQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "UnitofMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Cost", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "IssuedTo", "autoWidth": true, "bSearchable": true, "bSortable": true },
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}

//#endregion
//#endregion