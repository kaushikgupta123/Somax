
//#region V2-419 Enterprise User Management - Add/Remove Sites
var dtPersonnelSiteTable;
function generatePersonnelSitesGrid() {
    var userInfoId = $(document).find('#UserInfoId').val();
    var userClientId = $(document).find('#userClientId').val();
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
            url: "/Admin/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Admin/UserManagement/PopulatePersonnelSites?userInfoId=" + userInfoId + "&ClientId=" + userClientId,
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
            targets: [3], render: function (a, b, data, d) {
                      
                        return '<a class="btn btn-outline-primary addUmUserSite gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-danger delUmUserSite gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    
                    
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
    var userClientId = $(document).find('#userClientId').val();
    var userName = $(document).find('#userModels_UserName').val();
    var userFirstName = $(document).find('#userModels_FirstName').val();
    var userMiddleName = $(document).find('#userModels_MiddleName').val();
    var userLastName = $(document).find('#userModels_LastName').val();
    var userType = $(document).find('#userModels_UserType').val();
    var isSuperUser = $(document).find('#userModels_IsSuperUser').val();
    var siteControlled = $(document).find('#userModels_SiteControlled').val();
    var defaultSiteId = $(document).find('#userModels_DefaultSiteId').val();
    $.ajax({
        url: "/Admin/UserManagement/AddUmSite",
        type: "GET",
        dataType: 'html',
        data: { userInfoId: userInfoId, userName: userName, userFirstName: userFirstName, userMiddleName: userMiddleName, userLastName: userLastName, userType: userType, isSuperUser: isSuperUser, siteControlled: siteControlled, ClientId: userClientId, DefaultSiteId: defaultSiteId  },
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
            RedirectTouserDetail(data.userInfoId, data.userClientId, "sites");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function RedirectTouserDetail(UserInfoId, ClientId, mode) {
    $.ajax({
        url: "/Admin/UserManagement/UserManagementDetails",
        type: "POST",
        dataType: 'html',
        data: { UserId: UserInfoId, ClientId: ClientId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#usermanagementcontainer').html(data);
            $(document).find('#spnlinkToSearch').text(localStorage.getItem('ADMINUSERMANAGEMENTTITLE'));
        },
        complete: function () {
            CloseLoader();
           
            if (mode === "sites") {
                $('#lisites').trigger('click');
                $('#colorselector').val('Sites');
                generatePersonnelSitesGrid();
                $(document).find('#ClientActivity').css('display', 'none');
                $(document).find('#SiteTab').css('display', 'block');
                GenerateActivityTableGrid(ClientId)
            }
           
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#btnusersitecancel", function () {
    var UserInfoId = $(document).find('#userSiteModel_UserInfoId').val();
    var userClientId = $(document).find('#userSiteModel_ClientId').val();
    swal(CancelAlertSetting, function () {
        RedirectTouserDetail(UserInfoId, userClientId, "sites");
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
    var _ClientId = $(document).find('#userSiteModel_ClientId').val();
    $.ajax({
        url: "/Admin/UserManagement/ValidateAddSite",
        type: "POST",
        dataType: "json",
        data: { _userInfoid: _userInfoid, _siteid: _siteid, _siteControlled: _siteControlled, _userType: _userType, ClientId: _ClientId },
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
    var userClientId = $(document).find('#userClientId').val();
    DeleteUmSite(data.PersonnelId, userInfoId, data.SiteId, siteControlled, userType, isSuperUser, defaultSiteId, userClientId);
});

$(document).on('click', "#brdumSite", function () {
    var userInfoId = $(this).attr('data-val');
    var userClientId = $(document).find('#userSiteModel_ClientId').val();
    RedirectTouserDetail(userInfoId, userClientId, "sites");
});

function DeleteUmSite(personnelid, userInfoId, siteId, siteControlled, userType, isSuperUser, defaultSiteId, userClientId) {
    $.ajax({
        url: "/Admin/UserManagement/ValidateRemoveSite",
        type: "POST",
        dataType: "json",
        data: { _userInfoid: userInfoId, _siteId: siteId, _siteControlled: siteControlled, _userType: userType, ClientId: userClientId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.validationStatus == true) {
                CancelAlertSetting.text = getResourceValue("UserSiteDeleteAlert");
                swal(CancelAlertSetting, function () {
                    $.ajax({
                        url: '/Admin/UserManagement/DeleteUmSites',
                        data: {
                            personnelid: personnelid, userInfoId: userInfoId, siteId: siteId, userType: userType, isSuperUser: isSuperUser, defaultSiteId: defaultSiteId, ClientId: userClientId 
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
    var userClientId = $(document).find('#userSiteModel_ClientId').val();
    $.ajax({
        url: "/Admin/UserManagement/GetDefaultBuyerCounts",
        type: "POST",
        dataType: "json",
        data: { userInfoId: userInfoId, siteId: siteId, ClientId: userClientId },
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
//#region Activity
var ActivityTable;
function GenerateActivityTableGrid(ClientId) {
    if ($(document).find('#ActivityTable').hasClass('dataTable')) {
        ActivityTable.destroy();
    }
    ActivityTable = $("#ActivityTable").DataTable({
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
            url: "/Admin/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Admin/UserManagement/PopulateActivityTableGrid",
            "type": "GET",
            "datatype": "json",
            data: function (d) {
                d.ClientId = ClientId
                d.order = ActivityTable.order()[0][0];
                d.orderDir = ActivityTable.order()[0][1];
            },
        },
        "columns":
            [
                {
                    "data": "CreateDate",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "type": "date"
                },
                
                { "data": "SessionId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Browser", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "IPAddress", "autoWidth": true, "bSearchable": true, "bSortable": true },
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
//#endregion
