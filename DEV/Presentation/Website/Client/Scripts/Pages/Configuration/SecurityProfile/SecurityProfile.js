$(function () {
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
    generateSecurityProfileDataTable();
});
function openCity(evt, cityName) {
    evt.preventDefault();
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
    $("ul.vtabs li").removeClass("active");
    $(this).addClass("active");
    $(".tabsArea").hide();
    var activeTab = $(this).find("a").attr("href");
    $(activeTab).fadeIn();
    return false;
});
//#region Search
function generateSecurityProfileDataTable() {
    if ($(document).find('#tblsecurityprofile').hasClass('dataTable')) {
        dtSecurityProfile.destroy();
    }
    dtSecurityProfile = $("#tblsecurityprofile").DataTable({
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/SecurityProfile/GetMSecurityProfileGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.filter = LRTrim($("#txtsearchbox").val());
            },
            global: true
        },
        "columns":
        [
            {
                "data": "Name",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "className": "text-left",
                "name": "0",
                "mRender": function (data, type, row) {
                    return '<a class=lnk_onsecDetails href="javascript:void(0)">' + data + '</a>';
                }
            },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap'>" + data + "</div>";
                }
            },
        ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
//$(document).on('click', '#btnDropdownSearch', function () {
//    dtSecurityProfile.page('first').draw('page');
//}); 
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    $("#txtsearchbox").val('');
    dtSecurityProfile.page('first').draw('page');
});
$(document).on('click', '.lnk_onsecDetails', function (e) {
    e.preventDefault();
    var index_row = $('#tblsecurityprofile tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtSecurityProfile.row(row).data();
    ReloadSecurityProfileItems(data.SecurityProfileId, data.Description);
});
//#endregion
//#region Module
$(document).on('click', '#btnrefreshModule', function () {
    ReloadSecurityProfileItems($(document).find('#currentsecprofileid').val(), $(document).find('#currentsecprofiledes').val(), "");
});
$(document).on('click', '#btnSaveModule', function () {
    var ItemsToupdate = [];
    $("#tasksModuleTable tbody tr").each(function () {
        var securityprofileid = $(document).find('#currentsecprofileid').val();
        var securityitemid = $(this).find(".SecurityItemId").text();
        var sortorder = $(this).find('.SortOrder').text();
        var updateindex = $(this).find('.UpdateIndex').text();
        var Protected = $(this).find('.Protected').text();
        var ItemName = $(this).find(".ItemName").text();
        var access = $(this).find("td").find('.chkaccess').is(':checked');
        var create = $(this).find("td").find('.chkcreate').is(':checked');
        var edit = $(this).find("td").find('.chkedit').is(':checked');
        var del = $(this).find("td").find('.chkdel').is(':checked');
        var dataItem = new SecurityItemsObject(securityprofileid, securityitemid, sortorder, updateindex, Protected, ItemName, access, create, edit, del);
        ItemsToupdate.push(dataItem);
    })
    var objlist = JSON.stringify({ 'objlist': ItemsToupdate });
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        url: '/SecurityProfile/SaveSecurityItems',
        type: "POST",
        datatype: "json",
        data: objlist,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                ShowSuccessAlert(getResourceValue("SecurityItemUpdateAlert"));
            }
        },
        complete: function () {
            ReloadSecurityProfileItems($(document).find('#currentsecprofileid').val(), $(document).find('#currentsecprofiledes').val(), "");
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion
//#region Process
$(document).on('click', '#btnrefreshProcess', function () {
    ReloadSecurityProfileItems($(document).find('#currentsecprofileid').val(), $(document).find('#currentsecprofiledes').val(), "securityprocess");
});
$(document).on('click', '#btnSaveProcess', function () {
    var ItemsToupdate = [];
    $("#tasksProcessTable tbody tr").each(function () {
        var securityprofileid = $(document).find('#currentsecprofileid').val();
        var securityitemid = $(this).find(".SecurityItemId").text();
        var sortorder = $(this).find('.SortOrder').text();
        var updateindex = $(this).find('.UpdateIndex').text();
        var Protected = $(this).find('.Protected').text();
        var ItemName = $(this).find(".ItemName").text();
        var access = $(this).find("td").find('.chkaccess').is(':checked');
        var create = false;
        var edit = false;
        var del = false;
        var dataItem = new SecurityItemsObject(securityprofileid, securityitemid, sortorder, updateindex, Protected, ItemName, access, create, edit, del);
        ItemsToupdate.push(dataItem);
    })
    var objlist = JSON.stringify({ 'objlist': ItemsToupdate });
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        url: '/SecurityProfile/SaveSecuritySingleItems',
        type: "POST",
        datatype: "json",
        data: objlist,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                ShowSuccessAlert(getResourceValue("SecurityItemUpdateAlert"));
            }
        },
        complete: function () {
            ReloadSecurityProfileItems($(document).find('#currentsecprofileid').val(), $(document).find('#currentsecprofiledes').val(), "securityprocess");
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion
//#region Common
function SecurityItemsObject(SecurityProfileId, SecurityItemId, SortOrder, UpdateIndex, Protected, ItemName, ItemAccess, ItemCreate, ItemEdit, ItemDelete) {
    this.SecurityProfileId = SecurityProfileId;
    this.SecurityItemId = SecurityItemId;
    this.SortOrder = SortOrder;
    this.UpdateIndex = UpdateIndex;
    this.Protected = Protected;
    this.ItemName = ItemName;
    this.ItemAccess = ItemAccess;
    this.ItemCreate = ItemCreate;
    this.ItemEdit = ItemEdit;
    this.ItemDelete = ItemDelete;
};
function ReloadSecurityProfileItems(SecurityProfileId, Description, mode) {
    $.ajax({
        url: "/SecurityProfile/SecurityProfileDetail",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { SecurityProfileId: SecurityProfileId, Description: Description },
        success: function (data) {
            $('#renderSecurityprofile').empty().html(data);
            $(document).find('#currentsecprofileid').val(SecurityProfileId);
            $(document).find('#currentsecprofiledes').val(Description);
            if (mode == "securityprocess") {
                $(document).find('#liModule').removeClass('active');
                $(document).find('#Module').hide();

                $(document).find('#liProcess').addClass('active');
                $(document).find('#Process').show();
            }
        },
        complete: function () {
            $(document).on('click', '.disabled', function (e) {
                e.preventDefault();
            });
            CloseLoader();
        }
    });
}
//#endregion