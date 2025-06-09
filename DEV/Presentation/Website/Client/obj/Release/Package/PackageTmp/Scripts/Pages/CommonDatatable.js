//#region liCustomize1
var selectedCol = [];
var nselectedCol = [];
var oCols = [];
function funCustomizeBtnClick(dtTable, customsort, titleArray) {
    selectedCol = [];
    nselectedCol = [];
    oCols = [];
    colOrder = [0, 1];
    $('#StaffList').multiselect('destroy');
    var vCols = [];
    var mCols = [];
    $('#StaffList option').each(function () { $(this).remove(); });
    $('#PresenterList li').remove();
    $.each(dtTable.settings()[0].aoColumns, function (c) {
        var KeyValuePair = {};
        if (customsort) {
            KeyValuePair = {
                Id: dtTable.colReorder.order()[c],
                Idx: dtTable.settings()[0].aoColumns[c].idx,
                Value: dtTable.settings()[0].aoColumns[c].sTitle,
                Selected: dtTable.settings()[0].aoColumns[c].bVisible,
                Order: c
            };
        }
        else {
            KeyValuePair = {
                Id: dtTable.colReorder.order()[c],
                Idx: dtTable.settings()[0].aoColumns[c].idx,
                Value: dtTable.settings()[0].aoColumns[c].sTitle,
                Disabled: dtTable.settings()[0].aoColumns[c].bSortable,
                Selected: dtTable.settings()[0].aoColumns[c].bVisible,
                Order: c
            };
        }
        ////------------for uiconfig------------------------

        if (titleArray != null && titleArray.length > 0) {
            if (!(titleArray.includes(KeyValuePair.Value))) {
                oCols.push(KeyValuePair);
                if (!($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
                    vCols.push(KeyValuePair);
                }
                if (dtTable.settings()[0].aoColumns[c].bVisible == true && !($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
                    mCols.push(dtTable.colReorder.order()[c]);
                }
            }
        }
        else {
            oCols.push(KeyValuePair);
            if (!($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
                vCols.push(KeyValuePair);
            }
            if (dtTable.settings()[0].aoColumns[c].bVisible == true && !($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
                mCols.push(dtTable.colReorder.order()[c]);
            }
        }
        ////------------------------------------
        //oCols.push(KeyValuePair);
        //if (!($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
        //    vCols.push(KeyValuePair);
        //}
        //if (dtTable.settings()[0].aoColumns[c].bVisible == true && !($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
        //    mCols.push(dtTable.colReorder.order()[c]);
        //}

    });
    vCols = vCols.sort(function (ob1, ob2) {
        return ob1.Id - ob2.Id;
    });
    oCols = oCols.sort(function (ob1, ob2) {
        return ob1.Id - ob2.Id;
    });
    var options = [];
    $.each(vCols, function (i, v) {
        if (customsort) {
            options[i] = {
                label: v.Value,
                title: v.Value,
                value: v.Id,
                selected: v.Selected,

                attributes: {
                    'order': v.Order
                }
            };
        }
        else {
            options[i] = {
                label: v.Value,
                title: v.Value,
                value: v.Id,
                selected: v.Selected,
                disabled: v.Disabled,
                attributes: {
                    'order': v.Order
                }
            };
        }
    });
    $('#StaffList').multiselect({
        buttonContainer: '<div class="col-md-12 btn-group" />',
        templates: {
            button: '<button type="button" style="display:none;" class="multiselect dropdown-toggle" data-toggle="dropdown"><span class="multiselect-selected-text" ></span> <b class="caret"></b></button>',
            ul: '<ul class="multiselect-container dropdown-menu"></ul>',
            filter: '<li class="multiselect-item filter"><div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-search"></i></span><input class="form- control multiselect-search" type="text"></div></li>',
            filterClearBtn: '<span class="input-group-btn"><button class="btn btn-default multiselect-clear-filter" type="button"><i class="glyphicon glyphicon-remove-circle"></i></button></span>',
            li: '<li><a tabindex="0"><label class="m-checkbox m-checkbox--bold m-checkbox--state-success"></label></a></li>',
            divider: '<li class="multiselect-item divider"></li>',
            liGroup: '<li class="multiselect-item multiselect-group"><label></label></li>'
        },
        onChange: function (optiona, checked) {
            if (checked) {
                $('ul').moveToList('#StaffList', '#PresenterList', optiona);
            }
            else {
                $('#PresenterList li').each(function () {
                    if ($(this).data('val').toString() === $(optiona).val())
                        $(this).remove();
                });
            }
            selectedCol = [];
            nselectedCol = [];
            $('#StaffList option').each(function () {
                if ($(this).is(":selected"))
                    selectedCol.push({
                        Id: parseInt($(this).val()),
                        Name: $(this).text()
                    });
                else
                    nselectedCol.push({
                        Id: parseInt($(this).val()),
                        Name: $(this).text()
                    });
            });
            $(document).find('#lblCounter').text(selectedCol.length > 0 ? selectedCol.length : "None");
        }
    });

    $('#StaffList').multiselect("dataprovider", options, true);
    $('#StaffList').parent().find('ul li label').each(function () {
        $(this).append("<span></span>");
    });

    $.each(mCols, function (x, val) {
        $('#StaffList').multiselect('select', val, true);
    });
}

//function funCustomizeBtnClick(dtTable, customsort) {
//    selectedCol = [];
//    nselectedCol = [];
//    oCols = [];
//    colOrder = [0, 1];
//    $('#StaffList').multiselect('destroy');
//    var vCols = [];
//    var mCols = [];
//    $('#StaffList option').each(function () { $(this).remove(); });
//    $('#PresenterList li').remove();
//    $.each(dtTable.settings()[0].aoColumns, function (c) {
//        var KeyValuePair = {};
//        if (customsort) {
//            KeyValuePair = {
//                Id: dtTable.colReorder.order()[c],
//                Idx: dtTable.settings()[0].aoColumns[c].idx,
//                Value: dtTable.settings()[0].aoColumns[c].sTitle,
//                Selected: dtTable.settings()[0].aoColumns[c].bVisible,
//                Order: c
//            };
//        }
//        else {
//            KeyValuePair = {
//                Id: dtTable.colReorder.order()[c],
//                Idx: dtTable.settings()[0].aoColumns[c].idx,
//                Value: dtTable.settings()[0].aoColumns[c].sTitle,
//                Disabled: dtTable.settings()[0].aoColumns[c].bSortable,
//                Selected: dtTable.settings()[0].aoColumns[c].bVisible,
//                Order: c
//            };
//        }

//        oCols.push(KeyValuePair);
//        if (!($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
//            vCols.push(KeyValuePair);
//        }
//        if (dtTable.settings()[0].aoColumns[c].bVisible == true && !($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
//            mCols.push(dtTable.colReorder.order()[c]);
//        }

//    });
//    vCols = vCols.sort(function (ob1, ob2) {
//        return ob1.Id - ob2.Id;
//    });
//    oCols = oCols.sort(function (ob1, ob2) {
//        return ob1.Id - ob2.Id;
//    });
//    var options = [];
//    $.each(vCols, function (i, v) {
//        if (customsort) {
//            options[i] = {
//                label: v.Value,
//                title: v.Value,
//                value: v.Id,
//                selected: v.Selected,

//                attributes: {
//                    'order': v.Order
//                }
//            };
//        }
//        else {
//            options[i] = {
//                label: v.Value,
//                title: v.Value,
//                value: v.Id,
//                selected: v.Selected,
//                disabled: v.Disabled,
//                attributes: {
//                    'order': v.Order
//                }
//            };
//        }
//    });
//    $('#StaffList').multiselect({
//        buttonContainer: '<div class="col-md-12 btn-group" />',
//        templates: {
//            button: '<button type="button" style="display:none;" class="multiselect dropdown-toggle" data-toggle="dropdown"><span class="multiselect-selected-text" ></span> <b class="caret"></b></button>',
//            ul: '<ul class="multiselect-container dropdown-menu"></ul>',
//            filter: '<li class="multiselect-item filter"><div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-search"></i></span><input class="form- control multiselect-search" type="text"></div></li>',
//            filterClearBtn: '<span class="input-group-btn"><button class="btn btn-default multiselect-clear-filter" type="button"><i class="glyphicon glyphicon-remove-circle"></i></button></span>',
//            li: '<li><a tabindex="0"><label class="m-checkbox m-checkbox--bold m-checkbox--state-success"></label></a></li>',
//            divider: '<li class="multiselect-item divider"></li>',
//            liGroup: '<li class="multiselect-item multiselect-group"><label></label></li>'
//        },
//        onChange: function (optiona, checked) {
//            if (checked) {
//                $('ul').moveToList('#StaffList', '#PresenterList', optiona);
//            }
//            else {
//                $('#PresenterList li').each(function () {
//                    if ($(this).data('val').toString() === $(optiona).val())
//                        $(this).remove();
//                });
//            }
//            selectedCol = [];
//            nselectedCol = [];
//            $('#StaffList option').each(function () {
//                if ($(this).is(":selected"))
//                    selectedCol.push({
//                        Id: parseInt($(this).val()),
//                        Name: $(this).text()
//                    });
//                else
//                    nselectedCol.push({
//                        Id: parseInt($(this).val()),
//                        Name: $(this).text()
//                    });
//            });
//            $(document).find('#lblCounter').text(selectedCol.length > 0 ? selectedCol.length : "None");
//        }
//    });

//    $('#StaffList').multiselect("dataprovider", options, true);
//    $('#StaffList').parent().find('ul li label').each(function () {
//        $(this).append("<span></span>");
//    });

//    $.each(mCols, function (x, val) {
//        $('#StaffList').multiselect('select', val, true);
//    });
//}

$(document).on('click', '#PresenterList li:not(.disabled)', function () {
    $('#PresenterList li').removeClass('activeCol');
    $(this).addClass('activeCol');
});
$('#btnRemoveAvenger').click(function (e) {
    var opts = $('#PresenterList li.activeCol');
    if (opts.length > 0)
        $('#StaffList').multiselect('deselect', $(opts).data('val').toString(), true);
    e.preventDefault();
});
$('#btnAvengerUp').click(function (e) {
    $('ul').moveUpDown('#PresenterList', true, false);
    e.preventDefault();
});
$('#btnAvengerDown').click(function (e) {
    $('ul').moveUpDown('#PresenterList', false, true);
    e.preventDefault();

});
function funCustozeSaveBtn(dtTable, colOrder) {
    //Maintaing colspan when columns gets added or removed in no data available state in grid
    $('.dataTables_empty').attr('colspan', '100%');
    $("#PresenterList li").each(function () {
        var name = $(this).find('span').text();
        $.each(oCols, function (k, l) {
            if (name === l.Value) {
                colOrder.push(l.Id);
                //dtTable.columns(l.Id).visible(true);
                dtTable.columns(l.Idx).visible(true);
            }
        });
    });
    $.each(nselectedCol, function (o, g) {
        $.each(oCols, function (k, l) {
            if (g.Name === l.Value) {
                colOrder.push(l.Id);
                console.log(dtTable.settings()[0].aoColumns[l.Idx].sTitle);
                dtTable.columns(l.Idx).visible(false);
            }
        });
    });
    dtTable.colReorder.reset();
    dtTable.colReorder.order(colOrder);
}
//#endregion

//#region Datatable
function SetPageLengthMenu() {
    var pagelengthmenu = $(document).find('.searchdt-menu');
    pagelengthmenu.select2({
        minimumResultsForSearch: -1
    });
    $(document).find('.dataTables_length').show();
}
function GoToFirstPage(dtTable) {
    dtTable.page('first').draw('page');
}
$(document).on({
    mouseenter: function () {
        if ($(this).parents('table').hasClass('haschild')) {
            return;
        }

        if ($(this).parents('tbody').siblings('tfoot').length > 0) {
            trIndex = $(this).index() + 2;
        }
        else {
            trIndex = $(this).index() + 1;
        }
        $("table.dataTable").each(function (index) {
            $(this).find("tr:eq(" + trIndex + ")").addClass("tbl-row-hover");
        });
    },
    mouseleave: function () {
        if ($(this).parents('table').hasClass('haschild')) {
            return;
        }
        if ($(this).parents('tbody').siblings('tfoot').length > 0) {
            trIndex = $(this).index() + 2;
        }
        else {
            trIndex = $(this).index() + 1;
        }
        $("table.dataTable").each(function (index) {
            $(this).find("tr:eq(" + trIndex + ")").removeClass("tbl-row-hover");
        });
    }
}, ".dataTables_wrapper tr");
$(function () {
    $(document).find('.select2-not-search').select2({
        minimumResultsForSearch: -1
    });
});
function DisableExportButton(dataTbl, btnExort) {
    if (dataTbl.dataTable().fnGetData().length < 1)
        btnExort.prop('disabled', true);
    else
        btnExort.prop('disabled', false);
}
//#endregion

//#region Set Fixed Head Style
function SetFixedHeadStyle() {
    if ($(document).find('.m-content').hasClass('openMenu')) {
        if ($(document).find('.fixedDiv1').hasClass('fixupperpannel2')) {
            $(document).find('.fixedDiv1').removeClass('fixupperpannel2');
        }
        if ($(document).find('.fixedDiv2').hasClass('fixupperpannel')) {
            $(document).find('.fixedDiv2').removeClass('fixupperpannel');
        }
    }
    else if ($('.m-content').hasClass('closeMenu')) {
        if (!$(document).find('.fixedDiv1').hasClass('fixupperpannel2')) {
            $(document).find('.fixedDiv1').addClass('fixupperpannel2');
        }
        if (!$(document).find('.fixedDiv2').hasClass('fixupperpannel')) {
            $(document).find('.fixedDiv2').addClass('fixupperpannel');
        }
    }
}
//#endregion

//#region Export Button Show Hide
function funcShowExportbtn() {
    $(document).find("#popupExport").show();
    $(document).find("#mask").show();
}
function funcCloseExportbtn() {
    $(document).find("#popupExport").hide();
    $(document).find("#mask").hide();
}
//#endregion

//#region V2-853
//#region Reset Grid
function DeleteGridLayout(GridName, DataTable, localstorageKeys) {
    $.ajax({
        "url": "/Base/DeleteState",
        "data": {
            GridName: GridName
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {
            if (json.Result == 'success') {
                if (DataTable != null)
                    DataTable.destroy();
                $.each(localstorageKeys, function (Key, Value) {
                    if (localStorage.getItem(Value)) {
                        localStorage.removeItem(Value);
                    }
                });
            }
        }
    });
}
//#endregion
//#region Reset Grid for Card View where Jquery Datatable is not required
function DeleteGridLayout_Mobile(GridName, localstorageKeys) {
    $.ajax({
        "url": "/Base/DeleteState",
        "data": {
            GridName: GridName
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {
            if (json.Result == 'success') {
                $.each(localstorageKeys, function (Key, Value) {
                    if (localStorage.getItem(Value)) {
                        localStorage.removeItem(Value);
                    }
                });
            }
        }
    });
}
//#endregion
//#endregion
