//#region Notes
var umNotesTable;
var activeStatus = false;


function generateNotesGrid() {
    var personnelId = $(document).find('#userModels_PersonnelId').val();
    if ($(document).find('#notesTable').hasClass('dataTable')) {
        umNotesTable.destroy();
    }
    umNotesTable = $("#notesTable").DataTable({
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
            "url": "/UserManagement/PopulateNotes",
            data: function (d) {
                d.personnelId = personnelId;
            },
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                targets: [3], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-success editNoteBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delNoteBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ModifiedDate", "type": "date " },
                {
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnAddNote", function () {
    AddUMNotes();
});
$(document).on('click', '.editNoteBttn', function () {
    var data = umNotesTable.row($(this).parents('tr')).data();
    EditUmNote(data.NotesId);
});
$(document).on('click', '.delNoteBttn', function () {
    var data = umNotesTable.row($(this).parents('tr')).data();
    DeleteUmNote(data.NotesId);
});
$(document).on('click', "#brdumnotes", function () {
    var userInfoId = $(this).attr('data-val');
    RedirectTouserDetail(userInfoId, "notes");
});
function AddUMNotes() {
    var UserInfoId = $(document).find('#UserInfoId').val();
    var PersonnelId = $(document).find('#userModels_PersonnelId').val();
    var clientLookupId = $(document).find('#userModels_UserName').val();
    var ownerName = $(document).find('#userModels_FirstName').val() + ' ' + $(document).find('#userModels_LastName').val();
    $.ajax({
        url: "/UserManagement/AddUMNotes",
        type: "POST",
        dataType: 'html',
        data: { userInfoId: UserInfoId, personnelId: PersonnelId, clientLookupId: clientLookupId, ownerName: ownerName },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#usermanagementcontainer').html(data);
        },
        complete: function () {
            SetUMControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function UMNotesAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AddNoteAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("UpdateNoteAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectTouserDetail(data.UserInfoId, "notes");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditUmNote(notesid) {
    var UserInfoId = $(document).find('#UserInfoId').val();
    var PersonnelId = $(document).find('#userModels_PersonnelId').val();
    var clientLookupId = $(document).find('#userModels_UserName').val();
    $.ajax({
        url: "/UserManagement/EditUMNotes",
        type: "POST",
        dataType: 'html',
        data: { userInfoId: UserInfoId, personnelId: PersonnelId, notesId: notesid, clientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#usermanagementcontainer').html(data);
        },
        complete: function () {
            SetUMControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', "#btnumnotescancel", function () {
    var UserInfoId = $(document).find('#notesModel_UserInfoId').val();
    swal(CancelAlertSetting, function () {
        RedirectTouserDetail(UserInfoId, "notes");
    });
});
function DeleteUmNote(notesId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/UserManagement/DeleteUMNotes',
            data: {
                notesId: notesId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    umNotesTable.state.clear();
                    ShowDeleteAlert(getResourceValue("noteDeleteSuccessAlert"));
                }
            },
            complete: function () {
                generateNotesGrid();
                CloseLoader();
            }
        });
    });
}
//#endregion Notes
//#region Attachments
var dtumAttachmentTable;
function generateAttachmentsGrid() {
    var personnelId = $(document).find('#userModels_PersonnelId').val();
    if ($(document).find('#attachTable').hasClass('dataTable')) {
        dtumAttachmentTable.destroy();
    }
    dtumAttachmentTable = $("#attachTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "bProcessing": true,
        serverSide: true,
        "pagingType": "full_numbers",
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
            "url": "/UserManagement/PopulateUmAttachments?personnelId=" + personnelId,
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-danger delUmAttachment gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "FileName",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_download_attachment href="' + '/UserManagement/DowloadUmAttachment?fileinfoId=' + row.FileInfoId + '"  target="_blank">' + row.FullName + '</a>'
                    }
                },
                { "data": "FileSizeWithUnit", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CreateDate", "type": "date " },
                {
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.lnk_download_attachment', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = dtumAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/UserManagement/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });

});
$(document).on('click', "#btnAddumAttachment", function () {
    var UserInfoId = $(document).find('#UserInfoId').val();
    var PersonnelId = $(document).find('#userModels_PersonnelId').val();
    var UserName = $(document).find('#userModels_UserName').val();
    $.ajax({
        url: "/UserManagement/AddUmAttachments",
        type: "GET",
        dataType: 'html',
        data: { userInfoId: UserInfoId, personnelId: PersonnelId, userName: UserName },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#usermanagementcontainer').html(data);
        },
        complete: function () {
            SetUMControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function AttachmentUmAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
        swal(SuccessAlertSetting, function () {
            RedirectTouserDetail(data.umid, "attachment");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', ".delUmAttachment", function () {
    var data = dtumAttachmentTable.row($(this).parents('tr')).data();
    var FileAttachmentId = data.FileAttachmentId;
    DeleteUmAttachment(FileAttachmentId);
});
function DeleteUmAttachment(fileAttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/UserManagement/DeleteUmAttachments',
            data: {
                fileAttachmentId: fileAttachmentId
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "POST",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    dtumAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                generateAttachmentsGrid();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnumattachmentcancel", function () {
    var UserInfoId = $(document).find('#UserManagementModel_UserInfoId').val();
    swal(CancelAlertSetting, function () {
        RedirectTouserDetail(UserInfoId, "attachment");
    });
});
$(document).on('click', "#brdumattachment", function () {
    var UserInfoId = $(this).attr('data-val');
    RedirectTouserDetail(UserInfoId, "attachment");
});
//#endregion
//#region Contact
$(document).on('click', "#brdumcontact", function () {
    var userInfoId = $(this).attr('data-val');
    RedirectTouserDetail(userInfoId, "contacts");
});
var dtContactTable;
function generateContactsGrid() {
    var personnelId = $(document).find('#userModels_PersonnelId').val();
    if ($(document).find('#contactTable').hasClass('dataTable')) {
        dtContactTable.destroy();
    }
    dtContactTable = $("#contactTable").DataTable({
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
            "url": "/UserManagement/PopulateUmContacts?personnelId=" + personnelId,
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-success editUmContact gridinnerbutton" title="Edit"> <i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delUmContact gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Phone1", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Email1", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.editUmContact', function () {
    var data = dtContactTable.row($(this).parents('tr')).data();
    var UserName = $(document).find('#userModels_UserName').val();
    var userInfoId = $(document).find('#UserInfoId').val();
    var personelId = $(document).find('#userModels_PersonnelId').val();
    var country = data.AddressCountry;
    EditUmContact(userInfoId, data.ContactId, data.UpdateIndex, UserName, personelId, country);
});
$(document).on('click', '.delUmContact', function () {
    var data = dtContactTable.row($(this).parents('tr')).data();
    var userInfoId = $(document).find('#UserInfoId').val();
    DeleteUmContact(data.ContactId);
});
$(document).on('click', "#btnAddContact", function () {
    var userInfoId = $(document).find('#UserInfoId').val();
    var userName = $(document).find('#userModels_UserName').val();
    var ownerName = $(document).find('#userModels_FirstName').val() + ' ' + $(document).find('#userModels_LastName').val();
    var personnelId = $(document).find('#userModels_PersonnelId').val();
    $.ajax({
        url: "/UserManagement/AddUmContact",
        type: "GET",
        dataType: 'html',
        data: { userInfoId: userInfoId, userName: userName, ownerName: ownerName, personnelId: personnelId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#usermanagementcontainer').html(data);
            
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $(document).find('.select2picker').select2({});
            $('input, form').blur(function () {
                $(this).valid();
            });
            //$(document).find('#userManagementContactModel_OwnerName').val(ownerName);
            //$(document).find('#userManagementContactModel_OwnerId').val(userInfoId);
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btncontactcancel", function () {
    var UserInfoId = $(document).find('#userManagementContactModel_UserInfoId').val();
    swal(CancelAlertSetting, function () {
        RedirectTouserDetail(UserInfoId, "contacts");
    });
});
function EditUmContact(userInfoId, contactid, updatedindex, UserName, personelId, cntry) {
    $.ajax({
        url: "/UserManagement/EditUmContact",
        type: "GET",
        dataType: 'html',
        data: { userInfoId: userInfoId, contactid: contactid, updatedindex: updatedindex, userName: UserName, personelId: personelId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#usermanagementcontainer').html(data);
        },
        complete: function () {
            if (cntry == "USA") {
                $(document).find('#listateddl').show();
                $(document).find('#listatetext').hide();
            }
            else {
                $(document).find('#listateddl').hide();
                $(document).find('#listatetext').show();
            }
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $(document).find('.select2picker').select2({});
            $('input, form').blur(function () {
                $(this).valid();
            });
        },
        error: function () {
            CloseLoader();
        }
    });
}
function DeleteUmContact(contactid) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/UserManagement/DeleteUmContacts',
            data: {
                contactId: contactid
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("contactDeleteSuccessAlert"));
                    dtContactTable.state.clear();
                }
            },
            complete: function () {
                generateContactsGrid();
                CloseLoader();
            }
        });
    });
}
function UmContactAddOnSuccess(data) {
    if (data.Result == "success") {
        var message = "";
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("ContactAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("ContactUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectTouserDetail(data.userInfoId, "contacts");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('change', '#AddressCountry', function () {
    var cntry = $(this).val();
    if (cntry == "USA") {
        $(document).find('#listateddl').show();
        $(document).find('#listatetext').hide();
    }
    else {
        $(document).find('#listateddl').hide();
        $(document).find('#listatetext').show();
    }
});
//#endregion Contact


//#region Reset-Password
//$(document).on('click', '#openresetpasswordmodal', function () {
//    var UserInfoId = $(document).find('#UserInfoId').val();
//    $.ajax({
//        url: '/UserManagement/GetUserDetailForResetPassWord',
//        contentType: 'application/json; charset=utf-8',
//        datatype: 'json',
//        data: {
//            UserInfoId: UserInfoId
//        },
//        beforeSend: function () {
//            ShowLoader();
//        },
//        type: "GET",
//        success: function (data) {
//            $(document).find('#resetPasswordModel_UserName').val(data.UserName);
//            $(document).find('#resetPasswordModel_FirstName').val(data.FirstName);
//            $(document).find('#resetPasswordModel_MiddleName').val(data.MiddleName);
//            $(document).find('#resetPasswordModel_LastName').val(data.LastName);
//            $(document).find('#resetPasswordModel_UserInfoId').val(data.UserInfoId);
//            $(document).find('#resetPasswordModel_EmailAddress').val(data.EmailAddress);
//            $(document).find('#resetPasswordModel_Password').val('');
//            $(document).find('#resetPasswordModel_ConfirmPassword').val('');
//            $(document).find('#UMResetPasswordModal').modal('show');

//        },
//        complete: function () {
//            CloseLoader();
//        }
//    });
//});
//#endregion
//#region Common
function SetUMControls() {
    CloseLoader();
    $(document).find('.select2picker').select2({});
    $.validator.setDefaults({ ignore: null });
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
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
}
//#endregion Common

//#region V2-332

$(document).on('click', '#openresetpasswordmodal', function () {
    var UserInfoId = $(document).find('#UserInfoId').val();
    $.ajax({
        url: '/UserManagement/GetUserDetailsForResetPassWord',
        type: "GET",
        datatype: "json",
        data: {
            UserInfoId: UserInfoId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#resetPasswordModel_UserName').val(data.UserName);
            $(document).find('#resetPasswordModel_FirstName').val(data.FirstName);
            $(document).find('#resetPasswordModel_MiddleName').val(data.MiddleName);
            $(document).find('#resetPasswordModel_LastName').val(data.LastName);
            $(document).find('#resetPasswordModel_UserInfoId').val(data.UserInfoId);
            $(document).find('#resetPasswordModel_EmailAddress').val(data.EmailAddress);
            $(document).find('#resetPasswordModel_Password').val('');
            $(document).find('#resetPasswordModel_ConfirmPassword').val('');
            $(document).find('#UMResetPasswordModal').modal('show');
        },
        complete: function () {
            CloseLoader();
        }
    });
});
function CreateTempPasswordOnSuccess(data) {
    CloseLoader();
    if (data.result == "success") {
        SuccessAlertSetting.text = data.message;
        swal(SuccessAlertSetting, function () {
            $(document).find('#UMResetPasswordModal').modal('hide');
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}


//#endregion

//#region V2-487

//$(document).on('click', '#changeuseraccessmodal', function () {
//    $('.errormessage').html('').hide();
//    //ResetErrorDiv($(document).find('#frmchangeuseraccess'));    
//    //  $.validator.unobtrusive.parse(document);
//    var SecurityProfileName = $(document).find('#userModels_SecurityProfileName').val();
//    $(document).find('#userChangeAccessModel_SecurityProfileName').val(SecurityProfileName)
//    $(document).find('#UMchangeUserAccessModal').modal('show');

//});

$(document).on('click', '#changeuseraccessmodal', function () {
    $('.errormessage').html('').hide();
    var UserInfoId = $(document).find('#UserInfoId').val();
    $.ajax({
        url: '/UserManagement/ChangeUserAccess',
        type: "GET",
        dataType: 'html',
        data: {
            UserInfoId: UserInfoId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#UMchangeUserAccessModalPopup').html(data);
        },
        complete: function () {
            $('#UMchangeUserAccessModal').modal('show');
            $('#UMchangeUserAccessModal').modal({ backdrop: 'static', keyboard: false, show: true });
            var SecurityProfileName = $(document).find('#userModels_SecurityProfileName').val();
            $(document).find('#userChangeAccessModel_SecurityProfileName').val(SecurityProfileName)
            CloseLoader();
        }
    });
});

function UserAccessOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("UserAccessChangeSuccessAlert");
        swal(SuccessAlertSetting, function () {
            RedirectTouserDetail(data.userInfoId);
            $(document).find('#UMchangeUserAccessModal').modal('hide');
        });
    }
    else {
        $(document).find('#userChangeAccessModel_SecurityProfileName').val('');
        ShowGenericErrorOnAddUpdate(data);
    }
}

//#endregion
//#region V2-419 Enterprise User Management - Add/Remove Sites
var dtPersonnelSiteTable;
function generatePersonnelSitesGrid() {
    var userInfoId = $(document).find('#UserInfoId').val();
    var userType = $(document).find('#userModels_UserType').val();
    var usertypeFull = getResourceValue("UserTypeFull").toUpperCase();
    var usertypeAdmin = getResourceValue("UserTypeAdmin").toUpperCase();
    var usertypeEnter = getResourceValue("UserTypeEnterprise").toUpperCase();
    var usertypeWorkRequest = getResourceValue("UserTypeWorkRequest").toUpperCase();
    if ($(document).find('#siteTable').hasClass('dataTable')) {
        dtPersonnelSiteTable.destroy();
    }
    dtPersonnelSiteTable = $("#siteTable").DataTable({
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
            "url": "/UserManagement/PopulatePersonnelSites?userInfoId=" + userInfoId,
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
            targets: [3], render: function (a, b, data, d) {
                      if (userType.toUpperCase() == usertypeFull || userType.toUpperCase() == usertypeAdmin || userType.toUpperCase() == usertypeEnter || userType.toUpperCase() == usertypeWorkRequest.toUpperCase()) {
                        return '<a class="btn btn-outline-primary addUmUserSite gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-danger delUmUserSite gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return '';
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "SiteId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "UserSiteName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CraftDescription", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }

            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', ".addUmUserSite", function () {
    AddUserNewSite();
});

function AddUserNewSite() {
    var userInfoId = $(document).find('#UserInfoId').val();
    var userName = $(document).find('#userModels_UserName').val();
    var userFirstName = $(document).find('#userModels_FirstName').val();
    var userMiddleName = $(document).find('#userModels_MiddleName').val();
    var userLastName = $(document).find('#userModels_LastName').val();
    var userType = $(document).find('#userModels_UserType').val();
    var isSuperUser = $(document).find('#userModels_IsSuperUser').val();
    var siteControlled = $(document).find('#userModels_SiteControlled').val();
    $.ajax({
        url: "/UserManagement/AddUmSite",
        type: "GET",
        dataType: 'html',
        data: { userInfoId: userInfoId, userName: userName, userFirstName: userFirstName, userMiddleName: userMiddleName, userLastName: userLastName, userType: userType, isSuperUser: isSuperUser, siteControlled: siteControlled },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#usermanagementcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $(document).find('.select2picker').select2({});
            $('input, form').blur(function () {
                $(this).valid();
            });
        },
        error: function () {
            CloseLoader();
        }
    });
}

function UmSiteAddOnSuccess(data) {
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("UserSiteAddAlert");
        swal(SuccessAlertSetting, function () {
            RedirectTouserDetail(data.userInfoId, "sites");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', "#btnusersitecancel", function () {
    var UserInfoId = $(document).find('#userSiteModel_UserInfoId').val();
    swal(CancelAlertSetting, function () {
        RedirectTouserDetail(UserInfoId, "sites");
    });
});

$(document).on('click', "#btnSiteUserSubmit", function (e) {
    var form = $(document).find('#frmusersiteAdd');
    if (form.valid() === false) {
        return;
    }
    e.preventDefault();
    var _userInfoid = $(document).find('#userSiteModel_UserInfoId').val();
    var _siteid = $("#userSiteModel_SiteId").val();
    var _siteControlled = $("#userSiteModel_SiteControlled").val();
    var _userType = $(document).find('#userSiteModel_UserType').val();
    $.ajax({
        url: "/UserManagement/ValidateAddSite",
        type: "POST",
        dataType: "json",
        data: { _userInfoid: _userInfoid, _siteid: _siteid, _siteControlled: _siteControlled, _userType: _userType },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.validationStatus == true) {
                CancelAlertSetting.text = getResourceValue("UserAddForTheSiteAlert");

                swal(CancelAlertSetting, function (isConfirm) {
                    if (isConfirm == true) {
                        $(document).find('#frmusersiteAdd').submit();
                    }
                    else {
                        return false;
                    }
                });
            }
            else {
                GenericSweetAlertMethod(data);
                return false;
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function (jqxhr) {
            CloseLoader();
        }
    });
});

$(document).on('click', '.delUmUserSite', function () {
    var data = dtPersonnelSiteTable.row($(this).parents('tr')).data();
    var userInfoId = $(document).find('#UserInfoId').val();
    var siteControlled = $("#userModels_SiteControlled").val();
    var userType = $(document).find('#userModels_UserType').val();
    var isSuperUser = $(document).find('#userModels_IsSuperUser').val();
    var defaultSiteId = $(document).find('#userModels_DefaultSiteId').val();
    DeleteUmSite(data.PersonnelId, userInfoId, data.SiteId, siteControlled, userType, isSuperUser, defaultSiteId);
});

$(document).on('click', "#brdumSite", function () {
    var userInfoId = $(this).attr('data-val');
    RedirectTouserDetail(userInfoId, "sites");
});

function DeleteUmSite(personnelid, userInfoId, siteId, siteControlled, userType, isSuperUser, defaultSiteId) {
    $.ajax({
        url: "/UserManagement/ValidateRemoveSite",
        type: "POST",
        dataType: "json",
        data: { _userInfoid: userInfoId, _siteId: siteId, _siteControlled: siteControlled, _userType: userType },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.validationStatus == true) {
                CancelAlertSetting.text = getResourceValue("UserSiteDeleteAlert");
                swal(CancelAlertSetting, function () {
                    $.ajax({
                        url: '/UserManagement/DeleteUmSites',
                        data: {
                            personnelid: personnelid, userInfoId: userInfoId, siteId: siteId, userType: userType, isSuperUser: isSuperUser, defaultSiteId: defaultSiteId
                        },
                        type: "POST",
                        datatype: "json",
                        beforeSend: function () {
                            ShowLoader();
                        },
                        success: function (data) {
                            if (data.Result == "success") {
                                ShowDeleteAlert(getResourceValue("userSiteDeleteSuccessAlert"));
                                dtPersonnelSiteTable.state.clear();
                            }
                        },
                        complete: function () {
                            generatePersonnelSitesGrid();
                            CloseLoader();
                        }
                    });
                });
            }
            else {
                GenericSweetAlertMethod(data);
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function (jqxhr) {
            CloseLoader();
        }
    });
}

$(document).on("change", "#userSiteModel_SiteId", function () {
    var userInfoId = $("#userSiteModel_UserInfoId").val();
    var siteId = $("#userSiteModel_SiteId").val();
    $.ajax({
        url: "/UserManagement/GetDefaultBuyerCounts",
        type: "POST",
        dataType: "json",
        data: { userInfoId: userInfoId, siteId: siteId },
        success: function (data) {
            if (data > 0) {
                $("#userSiteModel_Buyer").prop('checked', true);
            }
            else {
                $("#userSiteModel_Buyer").prop('checked', false);
            }
        },
        error: function (jqxhr) {
        }
    });

    var tlen = $(document).find("#userSiteModel_SiteId").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#userSiteModel_SiteId").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#userSiteModel_SiteId").removeClass("input-validation-error");
    }
    else {
        var arectypeaddescribedby = $(document).find("#userSiteModel_SiteId").attr('aria-describedby');
        $('#' + arectypeaddescribedby).show();
        $(document).find('form').find("#userSiteModel_SiteId").addClass("input-validation-error");
    }
});
$(document).on("change", "#userSiteModel_CraftId", function () {
    chargeToSelected = $(this).val();
    chargeToText = $('option:selected', this).text();
    var tlen = $(document).find("#userSiteModel_CraftId").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#userSiteModel_CraftId").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#userSiteModel_CraftId").removeClass("input-validation-error");
    }
    else {
        var arectoaddescribedby = $(document).find("#userSiteModel_CraftId").attr('aria-describedby');
        $('#' + arectoaddescribedby).show();
        $(document).find('form').find("#userSiteModel_CraftId").addClass("input-validation-error");
    }
});

//#endregion

//#region V2-491 Manual Reset Password
$(document).on('click', '#manualPassReset', function () {
    var UserInfoId = $(document).find('#UserInfoId').val();
    var siteid = $(this).attr('data-siteid');
    var personnelid = $(this).attr('data-personnelid');
    $.ajax({
        url: '/UserManagement/ManualResetPassWord',
        type: "GET",
        dataType: 'html',
        data: {
            UserInfoId: UserInfoId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#ManualResetPasswordModal').html(data);
            $('#ManualResetPasswordModalPopup').modal({ backdrop: 'static', keyboard: false, show: true });
            $(document).find('#SiteId').val(siteid);
            $(document).find('#PersonnelId').val(personnelid);
        },
        complete: function () {
            CloseLoader();
        }
    });
});
function ManualCreateTempPasswordOnSuccess(data) {
    CloseLoader();
    if (data.result == "success") {
        SuccessAlertSetting.text = getResourceValue("PasswordResetAlert");
        swal(SuccessAlertSetting, function () {
            $(document).find('#ManualResetPasswordModalPopup').modal('hide');
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregionManualResetPasswordModal

//#region ChangeUserName V2-629
$(document).on('click', '#changeusername', function () {
    var UserInfoId = $(document).find('#UserInfoId').val();
    var userName = $('#userModels_UserName').val();
    var userDefaultSiteId = $('#userModels_DefaultSiteId').val();
    var userDefaultPersonnelId = $('#userModels_DefaultPersonnelId').val();
    $.ajax({
        url: '/UserManagement/ChangeUserName',
        type: "GET",
        dataType: 'html',
        data: {
            UserInfoId: UserInfoId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {           
            $('#UMchangeUserNameModalPopup').html(data);
        },
        complete: function () {
            $('#UMchangeUserNameModal').modal('show');
            $('#UMchangeUserNameModal').modal({ backdrop: 'static', keyboard: false, show: true });
            $('#userNameChangeModel_UserName').val(userName);
            $('#userNameChangeModel_UserInfoId').val(UserInfoId);  
            $('#userNameChangeModel_DefaultSiteId').val(userDefaultSiteId);
            $('#userNameChangeModel_DefaultPersonnelId').val(userDefaultPersonnelId);  
            CloseLoader();
        }
    });
});


function UserNameChangeOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("UserNameChangeSuccessAlert");
        swal(SuccessAlertSetting, function () {
            RedirectTouserDetail(data.userInfoId);
            $(document).find('#UMchangeUserNameModal').modal('hide');          
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#btnchangeUserName', function (e) {
    e.preventDefault();
    if ($('#userModels_UserName').val() !== $('#userNameChangeModel_UserName').val()) {
        $(document).find('#frmchangeusername').submit();
    }
    else {
        ShowErrorAlert(getResourceValue("PleaseChangeUserNameBeforeProceedAlert"));
    }
});
//#endregion

//#region V2-680 User Management - Add/Remove Storerooms
var dtPersonnelStoreroomTable;
function generatePersonnelStoreroomsGrid() {
    var personnelId = $(document).find('#userModels_PersonnelId').val();
    var rCount = 0;
    if ($(document).find('#storeroomTable').hasClass('dataTable')) {
        dtPersonnelStoreroomTable.destroy();
    }
    
    dtPersonnelStoreroomTable = $("#storeroomTable").DataTable({
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
            "url": "/UserManagement/PopulatePersonnelStorerooms?personnelId=" + personnelId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [2], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-primary addUmUserStoreroom gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                        '<a class="btn btn-outline-success editUserStoreroom gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delUmUserStoreroom gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                
                { "data": "SiteName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "0" },
                { "data": "StoreroomName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
                {
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }

            ],
        initComplete: function () {
            if (rCount > 0) { $("#btnAddStoreroom").hide(); }
            else {
                $("#btnAddStoreroom").show();
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', ".addUmUserStoreroom", function () {
    AddUserNewStoreroom();
});
$(document).on('click', "#btnAddStoreroom", function () {
    AddUserNewStoreroom();
});

function AddUserNewStoreroom() {
    var userInfoId = $(document).find('#UserInfoId').val();
    var userName = $(document).find('#userModels_UserName').val();
    $.ajax({
        url: "/UserManagement/AddUmStoreroom",
        type: "GET",
        dataType: 'html',
        data: { userInfoId: userInfoId, userName: userName },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#usermanagementcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $(document).find('.select2picker').select2({});
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
            $('input, form').blur(function () {
                $(this).valid();
            });
        },
        error: function () {
            CloseLoader();
        }
    });
}

function UmStoreroomAddOnSuccess(data) {
    if (data.Result == "success") {
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("UserStoreroomAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("UserUpdateStoreroomAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectTouserDetail(data.userInfoId, "storerooms");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', "#btnStoreroomUserCancel", function () {
    var UserInfoId = $(document).find('#userStoreroomModel_UserInfoId').val();
    swal(CancelAlertSetting, function () {
        RedirectTouserDetail(UserInfoId, "storerooms");
    });
});


$(document).on('click', '.delUmUserStoreroom', function () {
    var data = dtPersonnelStoreroomTable.row($(this).parents('tr')).data();
    DeleteUmStoreroom(data.StoreroomAuthId);
});

$(document).on('click', "#brdumStoreroom", function () {
    var userInfoId = $(this).attr('data-val');
    RedirectTouserDetail(userInfoId, "storerooms");
});

function DeleteUmStoreroom(storeroomAuthId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/UserManagement/DeleteUmStorerooms',
            data: {
                storeroomAuthId: storeroomAuthId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("UserStoreroomDeleteAlert"));
                    dtPersonnelStoreroomTable.state.clear();
                }
            },
            complete: function () {
                generatePersonnelStoreroomsGrid();
                CloseLoader();
            }
        });
    });
}
$(document).on('change', '#UserSiteId', function () {
    $("#StoreroomId").html("");
    var SiteId = $(this).val();
    if (SiteId) {
        $(document).find('#StoreroomId').removeAttr('disabled');
        $.ajax({
            url: '/UserManagement/RetrieveAllStoreroomBySiteId?SiteId=' + SiteId,
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $("#StoreroomId").append($("<option />").val('').text("--Select--"));               
                $.each(data.StoreroomList, function () {
                    $("#StoreroomId").append($("<option />").val(this.Value).text(this.Text));
                });
            },
            complete: function () {              
                CloseLoader();
            },
            error: function () {
                $("#StoreroomId").html("");
            }
        });
    }
    else {
        $(document).find('#StoreroomId').val('').trigger('change').attr('disabled', 'disabled');
        

        var areaddescribedby = $(document).find("#StoreroomId").attr('aria-describedby');
        if (typeof areaddescribedby !== 'undefined') {
            $('#' + areaddescribedby).hide();
        }
        $(document).find('form').find("#StoreroomId").removeClass("input-validation-error");

    }
});

$(document).on('click', '.editUserStoreroom', function () {
    var data = dtPersonnelStoreroomTable.row($(this).parents('tr')).data();
    EditUmStoreroom(data.StoreroomAuthId);
});
function EditUmStoreroom(storeroomAuthId) {
    var userInfoId = $(document).find('#UserInfoId').val();
    var clientLookupId = $(document).find('#userModels_UserName').val();
    $.ajax({
        url: "/UserManagement/EditUMStorerooms",
        type: "POST",
        dataType: 'html',
        data: { userInfoId: userInfoId, storeroomAuthId: storeroomAuthId, clientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#usermanagementcontainer').html(data);
        },
        complete: function () {
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
//#endregion


//#region add reference User V2-547
$(document).on('click', '#AddReferenceUser', function () {
    var UserInfoId = $(document).find('#UserInfoId').val();
    $.ajax({
        url: '/UserManagement/AddReferenceUser',
        type: "GET",
        dataType: 'html',
        //data: {
        //    UserInfoId: UserInfoId
        //},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddReferenceUserModalPopup').html(data);
        },
        complete: function () {
            SetUMControls();
            $('#AddReferenceUserModal').modal('show');
            $('#AddReferenceUserModal').modal({ backdrop: 'static', keyboard: false, show: true });
            $(document).find('#referenceUserModel_UserInfoId').val(UserInfoId);
            CloseLoader();
        }
    });
});


function AddReferenceUserOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $(document).find('#AddReferenceUserModal').modal('hide');
        SuccessAlertSetting.text = getResourceValue("AddReferenceUserAlert");
        swal(SuccessAlertSetting, function () {
            RedirectTouserDetail(data.userInfoId);
        });
        
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

//#region V2-887 Show hide password and ConfirmPasswword
function ShowHidePassword(PasswordId, EyeId) {
    if ($('#' + EyeId).hasClass('fa-eye-slash')) {
        $('#' + EyeId).removeClass('fa-eye-slash');
        $('#' + EyeId).addClass('fa-eye');
        $('#' + PasswordId).attr('type', 'text');
    } else {
        $('#' + EyeId).removeClass('fa-eye');
        $('#' + EyeId).addClass('fa-eye-slash');
        $('#' + PasswordId).attr('type', 'password');
    }
}
//#endregion