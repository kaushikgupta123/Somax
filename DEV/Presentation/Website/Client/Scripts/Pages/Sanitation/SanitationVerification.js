var SanitationVerificationdt;
var selectCount = 0;
var selectedcount = 0;
var totalcount = 0;
var searchcount = 0;
var searchresult = [];
var PassSelectedItemArray = [];
var FailSelectedItemArray = [];
$(document).on('click', '#sidebarCollapse', function () {
    $('.sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$(document).on('click', '#btnSanitationAdvSrch', function () {
    searchresult = [];
    SanitationVerificationdt.state.clear();
    AWBAdvSearch();
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
});
function AWBAdvSearch() {
    var InactiveFlag = false;
    $("#txtEqpDataSrch").val("");
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    var optionvalStat = $('#schStatusId option:selected').val();
    localStorage.setItem("dropKey", optionvalStat);
    if (optionvalStat.length !== 0) {
        generateSVDataTable();
    }
    $("#dvFilterSearchSelect2").html(searchitemhtml);
    $(".spnControlCounter").text(selectCount);
}
$(document).on('click', '.btnCross', function () {
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    AWBAdvSearch();
});
function clearAdvanceSearches() {
    $('#adv-JobId').val("");
    $('#adv-Description').val("");
    $('#adv-ChargeTo').val("");
    $('#adv-ChargeToName').val("");
   // $('#adv-WorkAssigned').val("");

    $("#adv-WorkAssigned").val("").trigger('change.select2');
    $('#adv-shift').val("").trigger('change.select2');

   // $('#adv-shift').val("");
    selectCount = 0;
    $("#dvFilterSearchSelect2").html('');
    $(".spnControlCounter").text(selectCount);
}
$(document).on('click', '#liclearadvsearch', function () {
    $('#schStatusId,#schCreateddate').val(0).trigger('change.select2');
    localStorage.removeItem("PurchaseApprovaldropKey");
    SanitationVerificationdt.state.clear();
    clearAdvanceSearches();
    SanitationVerificationdt.page('first').draw('page');
});
function GetVerificationSelectedItem(SanitationJobId, FailReason, FailComment) {
    this.SanitationJobId = SanitationJobId;
    this.FailReason = FailReason;
    this.FailComment = FailComment;
};
$(function () {
    ShowbtnLoaderclass("LoaderDrop");
    $(document).find('.select2picker').select2({});
    $(document).find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '#dismiss, .overlay', function () {
        $('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    var dropkey = localStorage.getItem("dropKeySV");
    
    if (dropkey) {
        var optionvalStat = $('#schStatusId option:selected').val();
        localStorage.setItem("dropKey", optionvalStat);
        if (optionvalStat.length !== 0) {
            generateSVDataTable();
        }
    }
    else {
        generateSVDataTable(0, 0);
    }
});
$(document).on('change', '#schStatusId', function () {  
        var optionvalStat = $('#schStatusId option:selected').val();
        localStorage.setItem("dropKeySV", optionvalStat);
        if (optionvalStat.length !== 0) {
            ShowbtnLoaderclass("LoaderDrop");
            SanitationVerificationdt.state.clear();
            generateSVDataTable();
        }
});

$(document).on('change', '#schCreateddate', function () {    
        var optionvalStat = $('#schStatusId option:selected').val();
        localStorage.setItem("dropKeySV", optionvalStat);
        if (optionvalStat.length !== 0) {
            ShowbtnLoaderclass("LoaderDrop");
            SanitationVerificationdt.state.clear();
            generateSVDataTable();
        }   
});

function generateSVDataTable() {
    var shift = $('#adv-shift').val();
    if ($(document).find('#tbSanitationVerification').hasClass('dataTable')) {
        SanitationVerificationdt.destroy();
    }  
    SanitationVerificationdt = $("#tbSanitationVerification").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/SanitationVerification/GetSanitationVerificationData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.StatusTypeId = $('#schStatusId option:selected').val();
                d.CreatedDatesId = $('#schCreateddate option:selected').val();
                d.JobId = LRTrim($('#adv-JobId').val());
                d.description = LRTrim($('#adv-Description').val());
                d.chargeToLookUpId = LRTrim($('#adv-ChargeTo').val());
                d.Chargetoname = LRTrim($('#adv-ChargeToName').val());
                d.workassigned = $('#adv-WorkAssigned').val();
                d.shift = shift;
            },
            "dataSrc": function (result) {
                searchcount = result.recordsTotal;
                $.each(result.data, function (index, item) {
                    searchresult.push(item.SanitationJobId);
                });

                if (totalcount < result.recordsTotal)
                    totalcount = result.recordsTotal;
                if (totalcount != result.recordsTotal)
                    selectedcount = result.recordsTotal;
                HidebtnLoaderclass("LoaderDrop");
                return result.data;
            },
            global: true
        },
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        "columns":
        [
            {
                "data": "SanitationJobId",
                orderable: false,
                "bSortable": false,
                className: 'select-checkbox',
                targets: 0,
                'className': 'dt-body-center',
                'render': function (data, type, full, meta) {

                    if ($('#SVsearchsSelectAll').is(':checked') && totalcount == selectedcount) {

                        return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="chksearch ' + data + '"  value="'
                            + $('<div/>').text(data).html() + '">';
                    } else {

                        if (PassSelectedItemArray.indexOf(data) != -1) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="chksearch ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                        else {
                            return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="chksearch ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                    }
                }
            },
            {
                "data": "ClientLookupId",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "mRender": function (data, type, row) {                   
                    return data;
                }
            },
            {
                "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": true,
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-400'>" + data + "</div>";
                }
            },
            { "data": "ChargeTo_ClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true, "width": "100px" },
            { "data": "ChargeTo_Name", "autoWidth": false, "bSearchable": true, "bSortable": true, "width": "100px" },
            { "data": "CompleteDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "width": "100px" },
            {
                "data": "FailReason", "autoWidth": true, "bSearchable": true,  "bSortable": true, 
                'render': function (data, type, full, meta) {
                    var result = "";
                    var exist = $.grep(FailSelectedItemArray, function (obj) {
                        return obj.SanitationJobId === full.SanitationJobId;
                    });

                    result = "<select class='selectFailReason dt-inline-dropdown'>"
                    result += "<option value='0' selected>--Select--</option>"
                    $(full.FailReasonList).each(function (index, value) {
                        if (exist.length > 0 && value.value == exist[0].FailReason) {
                            result += "<option value='" + exist[0].FailReason + "' selected>" + exist[0].FailReason + "</option>"
                        }
                        else if (value.value == data) {
                            result += "<option value='" + value.value + "' selected>" + value.label + "</option>"
                        }
                        else {
                            result += "<option value='" + value.value + "'>" + value.label + "</option>"
                        }
                    });
                    result += "</select>";
                    return result;
                }
            },
            {
                "data": "FailComment", "autoWidth": true, "bSearchable": true, "bSortable": true,
                'render': function (data, type, full, meta) {
                    var thisdate = data;
                    var exist = $.grep(FailSelectedItemArray, function (obj) {
                        return obj.SanitationJobId === full.SanitationJobId;
                    });
                    if (exist.length > 0) {
                        data = exist[0].FailComment;
                    }
                    return "<input type='text' class='dnyFailcomment dt-inline-text' maxlength='500' autocomplete='off' value='" + data + "'>";
                }
            },
            { "data": "Assigned_PersonnelClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date " },
            { "data": "Shift", "autoWidth": true, "bSearchable": true, "bSortable": true }
        ],
        "columnDefs": [
            {
                render: function (data, type, full, meta) {
                    return "<div class='text-wrap width-50'>" + data + "</div>";
                },
                targets: [1, 2]
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            if (totalcount != 0 && (totalcount == PassSelectedItemArray.length || (searchcount != totalcount && arrayContainsArray(PassSelectedItemArray, searchresult) == true))) {
                $('#SVsearchsSelectAll').prop('checked', true);
            } else {
                $('#SVsearchsSelectAll').prop('checked', false);
            }
            $(document).find('.selectFailReason,.select-shift').select2({
                minimumResultsForSearch: -1
            });
        },
        'rowCallback': function (row, data, index) {
            if (FailSelectedItemArray.indexOf(data.SanitationJobId) != -1) {
                $(row).addClass("checked");
            }
            var exist = $.grep(FailSelectedItemArray, function (obj) {
                return obj.SanitationJobId === data.SanitationJobId;
            });
            if (exist.length > 0 && exist[0].FailReason != "0" && data.FailReason != exist[0].FailReason) {
                $('td:eq(6)', row).css('background-color', '#d7f9c7');
                $('td:eq(6)', row).find('.selectFailReason').css('background-color', '#d7f9c7');
                $('td:eq(6)', row).css('color', '#fff');
            }
            if (exist.length > 0 && data.FailComment != exist[0].FailComment) {
                $('td:eq(7)', row).css('background-color', '#d7f9c7');
                $('td:eq(7)', row).find('.dnyFailcomment').css('background-color', '#d7f9c7');
            }
        },
    });
}
$(document).on('click', '#SVsearchsSelectAll', function (e) {
    PassSelectedItemArray = [];
    var checked = this.checked;
    var optionvalStat = $('#schStatusId option:selected').val();
    var optionvalCreateDate = $('#schCreateddate option:selected').val();
    var JobNumber = LRTrim($('#adv-JobId').val());
    var description = LRTrim($('#adv-Description').val());
    var chargeto = LRTrim($('#adv-ChargeTo').val());
    var chargetoname = LRTrim($('#adv-ChargeToName').val());
    var workassigned = $('#adv-WorkAssigned').val();
    var shift = $('#adv-shift').val();
    searchresult = [];
    var checked = this.checked;
    $.ajax({
        url: '/SanitationVerification/GetSanitVerification',
        data: {
            StatusTypeId: optionvalStat,
            CreatedDatesId: optionvalCreateDate,
            JobId: JobNumber,
            description: description,
            chargeto: chargeto,
            Chargetoname: chargetoname,
            workassigned: workassigned,
            shift: shift
        },
        async: true,
        type: "GET",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    searchresult.push(item.SanitationJobId);
                    if (checked) {
                        if (PassSelectedItemArray.indexOf(item.SanitationJobId) == -1)
                            PassSelectedItemArray.push(item.SanitationJobId);
                        var exist = $.grep(FailSelectedItemArray, function (obj) {
                            return obj.SanitationJobId === item.SanitationJobId;
                        });
                        if (exist.length == 0) {
                            var item1 = new GetVerificationSelectedItem(item.SanitationJobId, item.FailReason, item.FailComment);
                            FailSelectedItemArray.push(item1);
                        }

                    } else {
                        var i = PassSelectedItemArray.indexOf(item.SanitationJobId);
                        PassSelectedItemArray.splice(i, 1);

                        FailSelectedItemArray = $.grep(FailSelectedItemArray, function (obj) {
                            return obj.SanitationJobId !== item.SanitationJobId;
                        });
                    }
                });
            }
        },
        complete: function () {
            $('.itemcount').text(FailSelectedItemArray.length);
            SanitationVerificationdt.column(0).nodes().to$().each(function (index, item) {
                if (checked) {
                    $('#btnPass').removeAttr("disabled");
                    $('#btnFail').removeAttr("disabled");
                    $(this).find('.chksearch').prop('checked', 'checked');
                } else {
                    $('#btnPass').prop("disabled", "disabled");
                    $('#btnFail').prop("disabled", "disabled");
                    $(this).find('.chksearch').prop('checked', false);
                }
            });
            CloseLoader();
        }
    });
});
$(document).on('change', '.chksearch', function () {
    var thisTr = $(this).closest("tr");
    var data = SanitationVerificationdt.row($(this).parents('tr')).data();
    if (!this.checked) {
        var el = $('#SVsearchsSelectAll').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        selectedcount--;
        var index = PassSelectedItemArray.indexOf(data.SanitationJobId);
        PassSelectedItemArray.splice(index, 1);
        thisTr.removeClass("checked");
        FailSelectedItemArray = $.grep(FailSelectedItemArray, function (obj) {
            return obj.SanitationJobId !== data.SanitationJobId;
        });
    }
    else {
        selectedcount = selectedcount + FailSelectedItemArray.length;
        var item = new GetVerificationSelectedItem(data.SanitationJobId, data.FailReason, data.FailComment);
        PassSelectedItemArray.push(data.SanitationJobId);
        thisTr.addClass("checked");
        var data = SanitationVerificationdt.row($(this).parents('tr')).data();
        var SanitationJobId = data.SanitationJobId;
        var exist = $.grep(FailSelectedItemArray, function (obj) {
            return obj.SanitationJobId === SanitationJobId;
        });
        if (FailSelectedItemArray.length > 0 && exist.length > 0) {
        }
        else {
            var thisSanitation = new GetVerificationSelectedItem(SanitationJobId, "", "");
            FailSelectedItemArray.push(thisSanitation);
        }
    }
    if (PassSelectedItemArray.length > 0) {
        $('#btnPass').removeAttr("disabled");
        $('#btnFail').removeAttr("disabled");
    }
    else {
        $('#btnPass').prop("disabled", "disabled");
        $('#btnFail').prop("disabled", "disabled");
    }
});
$(document).on('change', '.dnyFailcomment', function () {
    $(this).parent('td').css('background-color', '#d7f9c7');
    $(this).css('background-color', '#d7f9c7');
    var data = SanitationVerificationdt.row($(this).parents('tr')).data();
    var SanitationJobId = data.SanitationJobId;
    var FailReason = $(this).parents('tr').find('.selectFailReason').val();
    var FailComment = $(this).val();
    var exist = $.grep(FailSelectedItemArray, function (obj) {
        return obj.SanitationJobId === SanitationJobId;
    });
    if (FailSelectedItemArray.length > 0 && exist.length > 0) {
        var index = -1;
        for (var i = 0; i < FailSelectedItemArray.length; ++i) {
            if (FailSelectedItemArray[i].SanitationJobId == SanitationJobId) {
                index = i;
                break;
            }
        }
        FailSelectedItemArray[index].FailComment = FailComment;
    }
    else {
        var item = new GetVerificationSelectedItem(SanitationJobId, FailReason, FailComment);
        FailSelectedItemArray.push(item);
    }
});
$(document).on('change', '.selectFailReason', function () {
    $(this).parent('td').css('background-color', '#d7f9c7');
    $(this).css('background-color', '#d7f9c7');
    var data = SanitationVerificationdt.row($(this).parents('tr')).data();
    var SanitationJobId = data.SanitationJobId;
    var FailReasonval = $(this).val();
    var FailReasontext = $(this).find('option:selected').text();
    var Comment = $(this).parents('tr').find('.dnyFailcomment').val();
    var exist = $.grep(FailSelectedItemArray, function (obj) {
        return obj.SanitationJobId === SanitationJobId;
    });
    if (FailSelectedItemArray.length > 0 && exist.length > 0) {
        var index = -1;
        for (var i = 0; i < FailSelectedItemArray.length; ++i) {
            if (FailSelectedItemArray[i].SanitationJobId == SanitationJobId) {
                index = i;
                break;
            }
        }
        FailSelectedItemArray[index].FailReason = FailReasonval;
    }
    else {
        var item = new GetVerificationSelectedItem(SanitationJobId, FailReasonval, Comment);
        FailSelectedItemArray.push(item);
    }
});
$(document).on('click', "#btnPass", function () {
    var list = FailSelectedItemArray;
    list = FailSelectedItemArray.filter(function (item) {
        return PassSelectedItemArray.indexOf(item.SanitationJobId) > -1;
    });
    list = JSON.stringify({ 'list': list });
    PassSelectedItemArray = [];
    FailSelectedItemArray = [];
    $.ajax({
        url: '/SanitationVerification/SavePassListFromGrid',
        data: list,
        contentType: 'application/json; charset=utf-8',
        type: "POST",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data.Result == "success") {
                $('#SVsearchsSelectAll').prop('checked', false);
                $('.itemcount').text(0);

                SuccessAlertSetting.text = getResourceValue("spnPassAlert");
                swal(SuccessAlertSetting, function () {
                    var optionvalStat = $('#schStatusId option:selected').val();

                    SanitationVerificationdt.state.clear();
                    if (optionvalStat.length !== 0) {
                        generateSVDataTable();
                        $('#btnPass').prop("disabled", "disabled");
                        $('#btnFail').prop("disabled", "disabled");
                        FailSelectedItemArray.clear;
                    }
                });
            }
            else {
                swal({
                    title: getResourceValue("CommonErrorAlert"),
                    text: getResourceValue("UpdateAlert"),
                    type: "error",
                    showCancelButton: false,
                    confirmButtonClass: "btn-sm btn-danger",
                    cancelButtonClass: "btn-sm",
                    confirmButtonText: getResourceValue("SaveAlertOk")
                }, function () {
                    return false;
                });
            }
        },
        complete: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btnFail", function () {
    var valid = true;
    $.each(FailSelectedItemArray, function (index, item) {
        if (item.FailReason == "") {
            valid = false;
        }
    });
    if (valid == false) {
        swal({
            title: getResourceValue("CommonErrorAlert"),
            text: getResourceValue("alertSVFailedReason"),
            type: "error",
            showCancelButton: false,
            confirmButtonClass: "btn-sm btn-danger",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("SaveAlertOk"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        }, function () {
        });
        return false;
    }
    var list = FailSelectedItemArray;
    list = FailSelectedItemArray.filter(function (item) {
        return PassSelectedItemArray.indexOf(item.SanitationJobId) > -1;
    });
    list = JSON.stringify({ 'list': list });
    PassSelectedItemArray = [];
    FailSelectedItemArray = [];
    $.ajax({
        url: "/SanitationVerification/SaveFailListFromGrid",
        type: "POST",
        dataType: "json",
        data: list,
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                $('#btnFail').prop("disabled", "disabled");
                $('#btnPass').prop("disabled", "disabled");
                $('#SVsearchsSelectAll').prop('checked', false);
                $('.itemcount').text(0);

                SuccessAlertSetting.text = getResourceValue("spnFailAlert");
                swal(SuccessAlertSetting, function () {
                    var optionvalStat = $('#schStatusId option:selected').val();

                    SanitationVerificationdt.state.clear();
                    if (optionvalStat.length !== 0) {
                        generateSVDataTable();
                        FailSelectedItemArray.clear;
                    }
                });
            }
            else {
                swal({
                    title: getResourceValue("CommonErrorAlert"),
                    text: getResourceValue("UpdateAlert"),
                    type: "error",
                    showCancelButton: false,
                    confirmButtonClass: "btn-sm btn-danger",
                    cancelButtonClass: "btn-sm",
                    confirmButtonText: getResourceValue("SaveAlertOk")
                }, function () {
                    return false;
                });
            }
        },
        complete: function () {
            CloseLoader();
        }
    });
});

