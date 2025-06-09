$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../BBUKPISIte/Index?page=BBU-KPI_Site";
});
function Activity(BBUKPIId) {
    $.ajax({
        "url": "/BBUKPISite/LoadActivity",
        data: { ObjectId: BBUKPIId },
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
            LoadComments(BBUKPIId);
        }
    });
}
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
function LoadComments(BBUKPIId) {
    $.ajax({
        "url": "/BBUKPISite/LoadComments",
        data: { ObjectId: BBUKPIId },
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
//#region CKEditor
$(document).on("focus", "#txtcommentsnew", function () {
    $(document).find('.ckeditorarea').show();
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.ckeditorarea').hide();
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    //ClearEditor();
    LoadCkEditorBBUSite('txtcomments');
    $("#txtcommentsnew").hide();
    $(".ckeditorfield").show();
});
$(document).on('click', '#btnsavecommands', function () {
    var selectedUsers = [];
    const data = getDataFromTheEditor();
    if (!data) {
        return false;
    }
    var BBUKPIId = $(document).find('#BBUKPISiteModel_BBUKPIId').val();
    //var woClientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var noteId = 0;
    if (LRTrim(data) == "") {
        return false;
    }
    else {
        $(document).find('.ck-content').find('.mention').each(function (item, index) {
            selectedUsers.push($(index).data('user-id'));
        });
        $.ajax({
            url: '/BBUKPISite/AddComments',
            type: 'POST',
            beforeSend: function () {
                ShowLoader();
            },
            data: {
                BBUKPIId: BBUKPIId,
                content: data,
                //woClientLookupId: woClientLookupId,
                //userList: selectedUsers,
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
                        //personnelList: personnelList,(BBUKPIId);
                        RedirectToSiteDetail(BBUKPIId);
                    });
                }
                else {
                    ShowGenericErrorOnAddUpdate(data);
                    CloseLoader();
                }
            },
            complete: function () {
                ClearEditor();
                $("#txtcommentsnew").show();
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
    $("#txtcommentsnew").show();
    $(".ckeditorfield").hide();
});
$(document).on('click', '.editcomments', function () {
    $(document).find(".ckeditorarea").each(function () {
        $(this).html('');
    });
    $("#txtcommentsnew").show();
    $(".ckeditorfield").hide();
    var notesitem = $(this).parents('.kt-notes__item').eq(0);
    notesitem.find('.ckeditorarea').html(CreateEditorHTML('wotxtcommentsEdit'));
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    var rawHTML = $.parseHTML($(this).parents('.kt-notes__item').find('.kt-notes__body').find('.originalContent').html());
    LoadCkEditorEditBBUSite('wotxtcommentsEdit', rawHTML);
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
    var BBUKPIId = $(document).find('#BBUKPISiteModel_BBUKPIId').val();
    var noteId = $(this).parents('.kt-notes__item').find('.editcomments').attr('id');
    //var woClientLookupId = $(document).find('#workOrderModel_ClientLookupId').val();
    var updatedindex = $(this).parents('.kt-notes__item').find('.hdnupdatedindex').val();
    $.ajax({
        url: '/BBUKPISite/AddComments',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        data: { BBUKPIId: BBUKPIId, content: LRTrim(data), noteId: noteId, updatedindex: updatedindex },
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
                    RedirectToSiteDetail(BBUKPIId);
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
                        RedirectToSiteDetail($(document).find('#BBUKPISiteModel_BBUKPIId').val());
                    });
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function LoadCkEditorBBUSite(equtxtcomments) {
    $(".toolbar-container").html('');
    ClearEditor();
    DecoupledEditor
        .create(document.querySelector('#' + equtxtcomments), {
            toolbar: ['heading', '|', 'bold', 'italic', 'alignment', 'link', 'numberedList', 'bulletedList', '|', 'fontFamily', 'fontSize', 'fontColor', 'fontBackgroundColor', '|', 'removeFormat'],
            extraPlugins: [MentionCustomization],
            mediaEmbed: { previewsInData: true },
            fontSize: {
                options: [8, 9, 10, 11, 12, 13, 14, 16, 18, 24, 30, 36, 48, 60, 72, 96]
            }/*,
            mention: {
                feeds: [
                    {
                        marker: '@',
                        feed: getFeedItems,
                    }
                ]
            }*/

        })
        .then(editor => {
            //editor.destroy();
            const toolbarContainer = document.querySelector('main .toolbar-container');
            toolbarContainer.prepend(editor.ui.view.toolbar.element);
            theEditor = editor;
            editor.execute('listStyle', { type: 'decimal' });
            editor.model.document.on('change:data', () => {

            });
        })
        .catch(err => {
            console.error(err.stack);
        });
}

function LoadCkEditorEditBBUSite(equtxtcomments, data) {
    $(".toolbar-containerEdit").html('');
    ClearEditorEdit();
    DecoupledEditor
        .create(document.querySelector('#' + equtxtcomments), {
            toolbar: ['heading', '|', 'bold', 'italic', 'alignment', 'link', 'numberedList', 'bulletedList', '|', 'fontFamily', 'fontSize', 'fontColor', 'fontBackgroundColor', '|', 'removeFormat'],
            extraPlugins: [MentionCustomization],
            mediaEmbed: { previewsInData: true },
            fontSize: {
                options: [8, 9, 10, 11, 12, 13, 14, 16, 18, 24, 30, 36, 48, 60, 72, 96]
            }/*,
            mention: {
                feeds: [
                    {
                        marker: '@',
                        feed: getFeedItems,
                    }
                ]

            }*/
        })
        .then(editor => {
            //editor.destroy();
            var getParseHtml = GetParseHtml(data);
            editor.setData(getParseHtml);
            const toolbarContainer = document.querySelector('main .toolbar-containerEdit');
            toolbarContainer.prepend(editor.ui.view.toolbar.element);
            theEditor = editor;
            editor.execute('listStyle', { type: 'decimal' });
            editor.model.document.on('change:data', () => {

            });
        })
        .catch(err => {
            console.error(err.stack);
        });
}
//#endregion
function RedirectToSiteDetail(BBUKPIId) {
    var title = localStorage.getItem("bbukpititle");
    $.ajax({
        url: "/BBUKPISite/BBUKPISiteDetails",
        type: "POST",
        dataType: 'html',
        data: { BBUKPIId: BBUKPIId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#BBUKPISiteContainer').html(data);
            $(document).find('#spnlinkToSearch').text(title);
            SetFixedHeadStyle();
        },
        complete: function () {
            colorarray = [];
            //generateRequestOrderGrid(workorderid);
            //SetWorkworderDetailEnvironment();
            //LoadCost(workorderid, 'Actual');
            //LoadImages(workorderid);
            Activity(BBUKPIId);
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '.link_bbukpi_detail', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();
    var BBUKPIId = data.BBUKPIId;
    var titletext = $('#sitesearchtitle').text();
    //localStorage.setItem("mrstatustext", titletext);
    $.ajax({
        url: "/BBUKPISite/BBUKPISiteDetails",
        type: "POST",
        data: { BBUKPIId: BBUKPIId },
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#BBUKPISiteContainer').html(data);
            $(document).find('#spnlinkToSearch').text(titletext);
            localStorage.setItem("bbukpititle", titletext);
            SetFixedHeadStyle();
            //if ($(document).find('.AddMrequest').length === 0) { $(document).find('#prdetailactiondiv').css('margin-right', '0px'); }
        },
        complete: function () {
            colorarray = [];
            //var dt = $("#tblLineItem").DataTable();
            //dt.state.clear();
            //dt.destroy();
            //generateLineiItemdataTable(MRequestId);
            Activity(BBUKPIId);
            //SetMRControls();
            CloseLoader();
        }
    });
});

//#region Edit
$(document).on('click', '#ReturnToDetail', function () {
    var BBUKPIId = $(document).find('#BBUKPISiteEditModel_BBUKPIId').val();
    RedirectToSiteDetail(BBUKPIId);
});
$(document).on('click', '#btnCancelEdit', function () {
    var BBUKPIId = $(document).find('#BBUKPISiteEditModel_BBUKPIId').val();
    swal(CancelAlertSetting, function () {
        RedirectToSiteDetail(BBUKPIId);
    });
});
$(document).on('click', '#btnEditBBUKPISite', function (e) {
    var BBUKPIId = $('#BBUKPISiteModel_BBUKPIId').val();
    $.ajax({
        url: "/BBUKPISite/BBUKPISiteEdit",
        type: "POST",
        data: { BBUKPIId: BBUKPIId },
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#BBUKPISiteContainer').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            SetFixedHeadStyle();
        }
    });
});
function BBUKPISiteEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("BBUKPIUpdateAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToSiteDetail(data.BBUKPIId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion

//#region Submit
$(document).on('click', '#btnSubmitBBUKPISite', function (e) {
    var BBUKPIId = $('#BBUKPISiteModel_BBUKPIId').val();
    var ClientId = $('#BBUKPISiteModel_ClientId').val();
    $.ajax({
        url: "/BBUKPISite/BBUKPISiteSubmit",
        type: "POST",
        data: { BBUKPIId: BBUKPIId, ClientId: ClientId },
        dataType: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                SuccessAlertSetting.text = getResourceValue("BBUKPISubmittedAlert");
                swal(SuccessAlertSetting, function () {
                    RedirectToSiteDetail(data.BBUKPIId);
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
            }
        },
        complete: function () {
            CloseLoader();
        }
    });
});
//#endregion 