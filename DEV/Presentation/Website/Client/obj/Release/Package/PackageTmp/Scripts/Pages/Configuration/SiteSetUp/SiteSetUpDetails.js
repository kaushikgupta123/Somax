//#region Common
function SetOnControl() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if (areaddescribedby) {
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
        }
    });
    $(document).find('.select2picker').select2({});
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    });
}
//#endregion

//#region AssetGroup1
$(document).on('click', "#btnAddAssetGroup1,.addAssetGroup1Bttn", function () {
    var AssetGroupLabel = $('#AssetGroup1Name').val();
    $.ajax({
        url: "/SiteSetup/AddAssetGroup1",
        type: "GET",
        data: {
            AssetGroupLabel: AssetGroupLabel
        },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderSiteSetUp').html(data);
        },
        complete: function () {
            SetOnControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});
var dtAssetGroup1Table;
function GenerateAssetGroup1Grid() {   
    var RecordCount = 0;   
    if ($(document).find('#AssetGroup1Table').hasClass('dataTable')) {
        dtAssetGroup1Table.destroy();
    }
    dtAssetGroup1Table = $("#AssetGroup1Table").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/SiteSetup/PopulateAssetGroup1",
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                RecordCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [3], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-primary addAssetGroup1Bttn gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                        '<a class="btn btn-outline-success editAssetGroup1Bttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delAssetGroup1Bttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                  
                }
            }
        ],
        "columns":
        [
            { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            {
                "data": "InactiveFlag", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date", "className": "text-center", "name": "3",
                "mRender": function (data, type, row) {                

                    if (data == true) {
                        return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" class="status" checked="checked"><span></span></label>';
                    }
                    else {
                        return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" class="status"><span></span></label>';
                    }
                }
            },
            { "bSortable": false, "className": "text-center" }
        ],
        initComplete: function () {
            $(document).on('click', '.status', function (e) {
                e.preventDefault();
            });
            SetPageLengthMenu();
            if (RecordCount > 0) {
                $(document).find('#btnAddAssetGroup1Container').hide();
            }
            else {
                $(document).find('#btnAddAssetGroup1Container').show();
            }
        }
    });
}
function AssetGroup1AddEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AssetGroup1AddAlert"); 
        }
        else {
            SuccessAlertSetting.text = getResourceValue("AssetGroup1UpdateAlert"); 
        }
        swal(SuccessAlertSetting, function () {
            localStorage.setItem("SITESETUPASSETGROUP1STATUS", true);
            window.location.href = "../SiteSetUp/Index?page=Site";
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).ready(function () {
    var displayState = localStorage.getItem("SITESETUPASSETGROUP1STATUS");
    if (displayState=="true") {
        $('#idAssetGroup1').trigger('click');
        $('#colorselector').val('AssetGroup1');
        localStorage.setItem("SITESETUPASSETGROUP1STATUS", false);
    }

});
$(document).on('click', "#btncancelAssetGroup1", function () { 
    swal(CancelAlertSetting, function () {
        localStorage.setItem("SITESETUPASSETGROUP1STATUS", true);
        window.location.href = "../SiteSetUp/Index?page=Site";

    });
});
$(document).on('click', '.delAssetGroup1Bttn', function () {
    var data = dtAssetGroup1Table.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        var AssetGroup1Id = data.AssetGroup1Id;
        $.ajax({
            url: '/SiteSetup/DeleteAssetGroup1',
            data: {
                AssetGroup1Id: AssetGroup1Id
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "post",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    CloseLoader();
                    ShowDeleteAlert(getResourceValue("AssetGroup1DeleteAlert"));
                    GenerateAssetGroup1Grid();
                }
                else {
                    GenericSweetAlertMethod(data);
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
});
$(document).on('click', '.editAssetGroup1Bttn', function () {
    var data = dtAssetGroup1Table.row($(this).parents('tr')).data();   
    var AssetGroupLabel = $('#AssetGroup1Name').val();
    $.ajax({
        url: "/SiteSetUp/EditAssetGroup1",
        type: "GET",
        dataType: 'html',
        data: { AssetGroup1Id: data.AssetGroup1Id, AssetGroupLabel: AssetGroupLabel },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderSiteSetUp').html(data);
        },
        complete: function () {
            SetOnControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion

//#region AssetGroup2
$(document).on('click', "#btnAddAssetGroup2,.addAssetGroup2Bttn", function () {
    var AssetGroupLabel = $('#AssetGroup2Name').val();
    $.ajax({
        url: "/SiteSetup/AddAssetGroup2",
        type: "GET",
        data: {
            AssetGroupLabel: AssetGroupLabel
        },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderSiteSetUp').html(data);
        },
        complete: function () {
            SetOnControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});
var dtAssetGroup2Table;
function GenerateAssetGroup2Grid() {
    var RecordCount = 0;
    if ($(document).find('#AssetGroup2Table').hasClass('dataTable')) {
        dtAssetGroup2Table.destroy();
    }
    dtAssetGroup2Table = $("#AssetGroup2Table").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/SiteSetup/PopulateAssetGroup2",
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                RecordCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [3], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-primary addAssetGroup2Bttn gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                        '<a class="btn btn-outline-success editAssetGroup2Bttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delAssetGroup2Bttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
        [
            { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            {
                "data": "InactiveFlag", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date", "className": "text-center", "name": "3",
                "mRender": function (data, type, row) {

                    if (data == true) {
                        return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" class="status" checked="checked"><span></span></label>';
                    }
                    else {
                        return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" class="status"><span></span></label>';
                    }
                }
            },
            { "bSortable": false, "className": "text-center" }
        ],
        initComplete: function () {
            $(document).on('click', '.status', function (e) {
                e.preventDefault();
            });
            SetPageLengthMenu();
            if (RecordCount > 0) {
                $(document).find('#btnAddAssetGroup2Container').hide();
            }
            else {
                $(document).find('#btnAddAssetGroup2Container').show();
            }
        }
    });
}
function AssetGroup2AddEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AssetGroup2AddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("AssetGroup2UpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            localStorage.setItem("SITESETUPASSETGROUP2STATUS", true);
            window.location.href = "../SiteSetUp/Index?page=Site";
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).ready(function () {
    var displayState = localStorage.getItem("SITESETUPASSETGROUP2STATUS");
    if (displayState == "true") {
        $('#idAssetGroup2').trigger('click');
        $('#colorselector').val('AssetGroup2');
        localStorage.setItem("SITESETUPASSETGROUP2STATUS", false);
    }

});
$(document).on('click', "#btncancelAssetGroup2", function () {
    swal(CancelAlertSetting, function () {
        localStorage.setItem("SITESETUPASSETGROUP2STATUS", true);
        window.location.href = "../SiteSetUp/Index?page=Site";

    });
});
$(document).on('click', '.delAssetGroup2Bttn', function () {
    var data = dtAssetGroup2Table.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        var AssetGroup2Id = data.AssetGroup2Id;
        $.ajax({
            url: '/SiteSetup/DeleteAssetGroup2',
            data: {
                AssetGroup2Id: AssetGroup2Id
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "post",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    CloseLoader();
                    ShowDeleteAlert(getResourceValue("AssetGroup2DeleteAlert"));
                    GenerateAssetGroup2Grid();
                }
                else {
                    GenericSweetAlertMethod(data);
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
});
$(document).on('click', '.editAssetGroup2Bttn', function () {
    var data = dtAssetGroup2Table.row($(this).parents('tr')).data();
    var AssetGroupLabel = $('#AssetGroup2Name').val();
    $.ajax({
        url: "/SiteSetUp/EditAssetGroup2",
        type: "GET",
        dataType: 'html',
        data: { AssetGroup2Id: data.AssetGroup2Id, AssetGroupLabel: AssetGroupLabel},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderSiteSetUp').html(data);
        },
        complete: function () {
            SetOnControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion

//#region AssetGroup3
$(document).on('click', "#btnAddAssetGroup3,.addAssetGroup3Bttn", function () {
    var AssetGroupLabel = $('#AssetGroup3Name').val();
    $.ajax({
        url: "/SiteSetup/AddAssetGroup3",
        type: "GET",
        data: {
            AssetGroupLabel: AssetGroupLabel
        },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderSiteSetUp').html(data);
        },
        complete: function () {
            SetOnControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});
var dtAssetGroup3Table;
function GenerateAssetGroup3Grid() {
    var RecordCount = 0;
    if ($(document).find('#AssetGroup3Table').hasClass('dataTable')) {
        dtAssetGroup3Table.destroy();
    }
    dtAssetGroup3Table = $("#AssetGroup3Table").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/SiteSetup/PopulateAssetGroup3",
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                RecordCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [3], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-primary addAssetGroup3Bttn gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                        '<a class="btn btn-outline-success editAssetGroup3Bttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                        '<a class="btn btn-outline-danger delAssetGroup3Bttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
        [
            { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            {
                "data": "InactiveFlag", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date", "className": "text-center", "name": "3",
                "mRender": function (data, type, row) {

                    if (data == true) {
                        return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" class="status" checked="checked"><span></span></label>';
                    }
                    else {
                        return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" class="status"><span></span></label>';
                    }
                }
            },
            { "bSortable": false, "className": "text-center" }
        ],
        initComplete: function () {
            $(document).on('click', '.status', function (e) {
                e.preventDefault();
            });
            SetPageLengthMenu();
            if (RecordCount > 0 ) {
                $(document).find('#btnAddAssetGroup3Container').hide();
            }
            else {
                $(document).find('#btnAddAssetGroup3Container').show();
            }
        }
    });
}
function AssetGroup3AddEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AssetGroup3AddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("AssetGroup3UpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            localStorage.setItem("SITESETUPASSETGROUP3STATUS", true);
            window.location.href = "../SiteSetUp/Index?page=Site";
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).ready(function () {
    var displayState = localStorage.getItem("SITESETUPASSETGROUP3STATUS");
    if (displayState == "true") {
        $('#idAssetGroup3').trigger('click');
        $('#colorselector').val('AssetGroup3');
        localStorage.setItem("SITESETUPASSETGROUP3STATUS", false);
    }

});
$(document).on('click', "#btncancelAssetGroup3", function () {
    swal(CancelAlertSetting, function () {
        localStorage.setItem("SITESETUPASSETGROUP3STATUS", true);
        window.location.href = "../SiteSetUp/Index?page=Site";

    });
});
$(document).on('click', '.delAssetGroup3Bttn', function () {
    var data = dtAssetGroup3Table.row($(this).parents('tr')).data();
    swal(CancelAlertSetting, function () {
        var AssetGroup3Id = data.AssetGroup3Id;
        $.ajax({
            url: '/SiteSetup/DeleteAssetGroup3',
            data: {
                AssetGroup3Id: AssetGroup3Id
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "post",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    CloseLoader();
                    ShowDeleteAlert(getResourceValue("AssetGroup3DeleteAlert"));
                    GenerateAssetGroup3Grid();
                }
                else {
                    GenericSweetAlertMethod(data);
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
});
$(document).on('click', '.editAssetGroup3Bttn', function () {
    var data = dtAssetGroup3Table.row($(this).parents('tr')).data();
    var AssetGroupLabel = $('#AssetGroup3Name').val();
    $.ajax({
        url: "/SiteSetUp/EditAssetGroup3",
        type: "GET",
        dataType: 'html',
        data: { AssetGroup3Id: data.AssetGroup3Id, AssetGroupLabel: AssetGroupLabel },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderSiteSetUp').html(data);
        },
        complete: function () {
            SetOnControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion