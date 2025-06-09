//#region Global variables
var selectedcountPTL = 0;
var selectCountPTL = 0;
var StartDateEditArray = [];
var EndDateEditArray = [];
var ProjectId = 0;
var ProjecttaskIdsArray = [];
var ProjecttaskWOidArray = [];
var totalCountTaskGrid = 0;
var ProgressEditArray = [];
//#endregion

function LoadProjectTab()
{
    var ProjectId = $(document).find("#projectCostingModel_ProjectId").val();
    LoadProjectCostingHeaderData(ProjectId);
}
function LoadProjectCostingHeaderData(ProjectId) {
    $.ajax({
        url: '/ProjectCosting/Details',
        type: 'POST',
        dataType: 'html',
        data: {
            ProjectId: ProjectId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data) {
                $(document).find('#projectmaincontainer').html("");
                $(document).find('#projectmaincontainer').html(data);
            }
           
        },
        complete: function () {
            LoadActivity(ProjectId)
            CloseLoader();
           
        },
        error: function (err) {
            CloseLoader();
        }
    });
}

// #region Activity & Comments
var selectedUsers = [];
var selectedUnames = [];
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
function LoadActivity(ProjectId) {
    $.ajax({
        "url": "/ProjectCosting/LoadActivity",
        data: { ProjectId: ProjectId },
        type: "POST",
        datatype: "json",
        success: function (data) {
            $(document).find('#activityitems').html(data);
            $(document).find("#activityList").mCustomScrollbar({
                theme: "minimal"
            });
        },
        complete: function () {
            $(document).find('#activitydataloader').hide();
            var ftext = '';
            var bgcolor = '';
            $(document).find('#activityitems').find('.activity-header-item').each(function () {
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
                $(this).attr('style', 'color:' + bgcolor + ' !important;border:1px solid ' + bgcolor + ' !important;');
                ftext = thistext;
            });
            LoadComments(ProjectId);
        }
    });
}
function LoadComments(ProjectId) {
    $.ajax({
        "url": "/ProjectCosting/LoadComments",
        data: { ProjectId: ProjectId },
        type: "POST",
        datatype: "json",
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
        }
    });
}
//#endregion

//#region CKEditor
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
$(document).on('click', '#btnsavecommand', function () {
    var selectedUsers = [];
    const data = getDataFromTheEditor();
    if (!data) {
        return false;
    }
    var ProjectId = $(document).find("#projectCostingModel_ProjectId").val();
    var ClientLookupId = $(document).find("#projectHeaderSummaryModel_ClientlookupId").val();
    var noteId = 0;
    if (LRTrim(data) == "") {
        return false;
    }
    else {
        /*  alert("ajax");*/
        $(document).find('.ck-content').find('.mention').each(function (item, index) {
            selectedUsers.push($(index).data('user-id'));
        });
        $.ajax({
            url: '/ProjectCosting/AddComments',
            type: 'POST',
            beforeSend: function () {
                ShowLoader();
            },
            data: {
                ProjectId: ProjectId,
                content: data,
                ClientLookupId: ClientLookupId,
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

                        LoadProjectTab();
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
    var ProjectId = $(document).find("#projectCostingModel_ProjectId").val();
    var ClientLookupId = $(document).find("#projectHeaderSummaryModel_ClientlookupId").val();
    var noteId = $(this).parents('.kt-notes__item').find('.editcomments').attr('id');
    var updatedindex = $(this).parents('.kt-notes__item').find('.hdnupdatedindex').val();
    $.ajax({
        url: '/ProjectCosting/AddComments',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        data: { ProjectId: ProjectId, content: LRTrim(data), ClientLookupId: ClientLookupId, noteId: noteId, updatedindex: updatedindex },
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
                    LoadProjectTab();

                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
                CloseLoader();
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

                        LoadProjectTab();
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
function ProjectStatusUpdate(status) {

    var confrimtext = status == "Complete" ? getResourceValue("ConfirmToCompleteProjectAlert") : status == "Canceled" ? getResourceValue("ConfirmToCancelProjectAlert") : status == "Open" ? getResourceValue("ConfirmToReopenProjectAlert") : "";

    if (status != "") {

        var ProjectId = $(document).find("#projectCostingModel_ProjectId").val();
        swal({
            title: getResourceValue("spnAreyousure"),
            text: confrimtext,
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: false,
            confirmButtonClass: "btn-sm btn-primary",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("CancelAlertYes"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        }, function () {


            {
                $.ajax({
                    url: "/ProjectCosting/UpdatingProjectStatus",
                    type: "POST",
                    data: { ProjectId: ProjectId, Status: status },
                    datatype: "json",
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (data) {

                        var Successtext = status == "Complete" ? getResourceValue("ProjectCompletedSuccessfullyAlert") : status == "Canceled" ? getResourceValue("ProjectCanceledSuccessfullyAlert") : status == "Open" ? getResourceValue("ProjectReopenedSuccessfullyAlert") : "";

                        SuccessAlertSetting.text = Successtext;
                        CloseLoader();
                        swal(SuccessAlertSetting, function () {
                            //LoadProjectTab();
                            RedirectToProjectCostingDetail(ProjectId, "");
                        });
                    },
                    complete: function () {

                    },
                    error: function () {
                        CloseLoader();
                    }
                });
            }
        });

    }


}

//#region Edit ProjectCosting
$(document).on('click', '#editProjectCosting', function () {
    var ProjectId = $(document).find("#projectCostingModel_ProjectId").val();
    $.ajax({
        url: "/ProjectCosting/EditProjectCosting",
        type: "GET",
        dataType: 'html',
        data: { ProjectId: ProjectId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#projectmaincontainer').html(data);
        },
        complete: function () {
            SetProjControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btnCancelEditProject", function () {
    var ProjectId = $(document).find("#EditProject_ProjectId").val();
    var ClientLookupId = $(document).find("#EditProject_ClientLookupId").val();
    swal(CancelAlertSetting, function () {
        RedirectToProjectCostingDetail(ProjectId, ClientLookupId);
    });
});

function ProjectCostingEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        localStorage.setItem("Projstatus", '8')
        localStorage.setItem("Projstatustext", getResourceValue("OpenProjectsAlert"));
        SuccessAlertSetting.text = getResourceValue("ProjectUpdatedSuccessAlert");
        
        ResetErrorDiv();
        swal(SuccessAlertSetting, function () {
            $(document).find('form').trigger("reset");
            $(document).find('form').find("select").val("").trigger('change');
            $(document).find('form').find("select").removeClass("input-validation-error");
            $(document).find('form').find("input").removeClass("input-validation-error");
            $(document).find('form').find("textarea").removeClass("input-validation-error");
            RedirectToProjectCostingDetail(data.projectId, data.clientLookupId);
        });
        //}
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};

//#endregion
$(document).on('click', '#ProjectCostreaddescription', function () {
    $(document).find('#Projectdetaildesmodaltext').text($(this).data("des"));
    $(document).find('#Projectcostingdetaildesmodal').modal('show');
});

$(document).on('click', '#brdProjectCosting', function () {
    var ProjectId = $(document).find("#EditProject_ProjectId").val();
    var ClientLookupId = $(document).find("#EditProject_ClientLookupId").val();
    RedirectToProjectCostingDetail(ProjectId, ClientLookupId);
});

//#region Add projectCosting
$(document).on('click', '.addNewProjectCosting', function () {
    $.ajax({
        url: "/ProjectCosting/AddProjectCosting",
        type: "GET",
        dataType: 'html',
        data: { ProjectId: 0 },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#projectmaincontainer').html(data);
        },
        complete: function () {
            SetProjControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});

function ProjectCostingAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.Command == "save") {
            localStorage.setItem("Projstatus", '8')
            localStorage.setItem("Projstatustext", getResourceValue("OpenProjectsAlert"));
            SuccessAlertSetting.text = getResourceValue("ProjectAddedSuccessAlert");
            swal(SuccessAlertSetting, function () {
                RedirectToProjectCostingDetail(data.projectId, data.clientLookupId);
            });
        }
        else {
            SuccessAlertSetting.text = getResourceValue("ProjectAddedSuccessAlert");
        ResetErrorDiv();
        swal(SuccessAlertSetting, function () {
            $(document).find('form').trigger("reset");
            $(document).find('form').find("select").val("").trigger('change');
            $(document).find('form').find("select").removeClass("input-validation-error");
            $(document).find('form').find("input").removeClass("input-validation-error");
            $(document).find('form').find("textarea").removeClass("input-validation-error");
            RedirectToProjectCostingDetail(data.projectId, data.clientLookupId);
        });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};

$(document).on('click', "#btnCancelAddProject", function () {
    swal(CancelAlertSetting, function () {
        window.location.href = "../ProjectCosting/Index?page=ProjectCosting";
    });
});

//#endregion