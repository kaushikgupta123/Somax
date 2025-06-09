//#region variable
var reporttable;
var grpname = "";
var reportname = '';
var rlistid = '';
var rlistuserreport = false;
var IncludePrompt = '';
var hasChild = false;
var IsGrouped = false;
var GroupColumn = '';
var prom1source = '';
var prom2source = '';
var prom2type = '';
var multiselectedval1 = '';
var multiselectedval2 = '';
var case1 = 0;
var case2 = 0;
var daterangetext1 = '';
var daterangetext2 = '';
var strdate1 = '';
var enddate1 = '';
var strdate2 = '';
var enddate2 = '';
var promp1label = '';
var promp2label = '';
var Favorites = 'Favorites';
var changefavorite = false;
var labelending = ' :';
var cola = [];
var today = $.datepicker.formatDate('mm/dd/yy', new Date());
var dtformat = 'MM/DD/YYYY';
var dtrangelocal = {
    "applyLabel": getResourceValue("JsApply"),
    "cancelLabel": getResourceValue("CancelAlert")
};
var footerhtmlth = '<th style="font-weight:600 !important;"></th>';
var plain = "PLAIN";
var number = "NUMBER";
var currency = "CURRENCY";
var percentage = "PERCENTAGE";
var FilterElements = [];
var ConfigArray = [];
var FilterArray = [];
var IsDevExpressRpt = '';
var DevExpressRptName = '';
//#endregion

$(function () {
    $(document).on('click', "ul.vtabs li", function () {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
    });
    $('.lireports').on('click', function () {
        grpname = $(this).data('group');
        if (grpname === Favorites) {
            GetReportList('');
        }
        else {
            GetReportList(grpname);
        }
        localStorage.setItem('CURRENTGRPSELECTOR', $(this).attr('id'));
    });
    $(document).on('change', '#colorselector', function (evt) {
        grpname = $(this).val();
        if (grpname === Favorites) {
            GetReportList('');
        }
        else {
            GetReportList(grpname);
        }
        localStorage.setItem('CURRENTGRPSELECTOR', $(this).val());
    });
    var currentgrpselector = localStorage.getItem('CURRENTGRPSELECTOR');
    if (currentgrpselector == Favorites || !currentgrpselector) {
        GetReportList('');
    }
    else {
        if ($("#detmaintab").is(":visible")) {
            $('#' + currentgrpselector).trigger('click');
        } else {
            $('#colorselector').val(currentgrpselector).trigger('change');
        }
    }
    $(document).on('click', '.linotfavorites a:not(.change-report)', function () {
        var rlistid = $(this).parent('li').attr('id');
        var favoriteid = $(this).parent('li').data('favorite');
        var isuserreport = $(this).parent('li').data('isuserreport');
        CreateFavorite(rlistid, favoriteid, isuserreport);
    });
    $(document).on('click', '.lifavorites a:not(.change-report)', function () {
        var favoriteid = $(this).parent('li').data('favorite');
        DeleteFavorite(favoriteid);
    });
    $(document).on('mouseenter', '.reportheader,.reportdesc', function () {
        $('#h2reportname').text($(this).parent().find('.reportheader').text());
        $('#h3reportdes').text($(this).parent().find('.reportdesc').text());
        $(this).nextAll('.descriptionPopup').show();
    }).on('mouseleave', '.descriptionPopup', function () {
        $('.descriptionPopup').hide();
    });

    $(document).find("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '#dismiss, .overlay', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', '#sidebarCollapse', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
});

//#region ReportListing
function GetReportList(GroupName) {
    $.ajax({
        url: '/Reports/GetReportList',
        data: { GroupName: GroupName },
        type: "Get",
        datatype: "html",
        beforeSend: function () {
            if (changefavorite === false) {
                ShowLoader();
            }
        },
        success: function (data) {
            changefavorite = false;
            $('#reportsblock').html(data);
        },
        complete: function () {
            CloseLoader();
        }
    });
}
function CreateFavorite(ReportListingId, ReportFavoritesId, isuserreport) {
    changefavorite = true;
    $.ajax({
        url: '/Reports/CreateFavorite',
        type: "Post",
        datatype: "json",
        data: { ReportListingId: ReportListingId, ReportFavoritesId: ReportFavoritesId, IsUserReport: isuserreport },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result === 'success') {
                GetReportList(grpname);
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
function DeleteFavorite(ReportFavoritesId) {
    changefavorite = true;
    $.ajax({
        url: '/Reports/DeleteFavorite',
        type: "Post",
        datatype: "json",
        data: { ReportFavoritesId: ReportFavoritesId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result === 'success') {
                if (grpname === Favorites) {
                    GetReportList('');
                }
                else {
                    GetReportList(grpname);
                }
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion

//#region CreateReport
$(document).on('click', '.recentReportsBox', function () {
    rlistid = $(this).data('reportid');
    rlistuserreport = $(this).data('isuserreport');
    IncludePrompt = $(this).data('isprompt');
    IsDevExpressRpt = $(this).data('isdevexpressrpt');
    DevExpressRptName = $(this).data('reportname');
    if (IsDevExpressRpt == "True") {
        LoadDevExpressReport(DevExpressRptName);
    }
    else {
        LoadReport(rlistid, IncludePrompt, rlistuserreport);
    }
});
$(document).on('click', '.reportitem h2, .reportitem h3', function () {
    rlistid = $(this).parent('div').parent('li').attr('id');
    rlistuserreport = $(this).parent('div').parent('li').data('isuserreport');
    IncludePrompt = $(this).parent('div').parent('li').data('isprompt');
    IsDevExpressRpt = $(this).parent('div').parent('li').data('isdevexpressrpt');
    DevExpressRptName = $(this).parent('div').parent('li').data('reportname');
    if (IsDevExpressRpt == "True") {
        LoadDevExpressReport(DevExpressRptName);
    }
    else {
        LoadReport(rlistid, IncludePrompt, rlistuserreport);
    }
});
var LoadReport = function (rlistid, IncludePrompt, rlistuserreport) {
    if (IncludePrompt == "True") {
        $.ajax({
            url: '/Reports/GetPromptModal',
            data: { ReportListingId: rlistid, IsUserReport: rlistuserreport },
            type: "POST",
            datatype: "html",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#promptModalcontainer').html(data);
                promp1label = $(document).find('#lbl-prompt1').text();
                promp2label = $(document).find('#lbl-prompt2').text();
            },
            complete: function () {
                $(document).find('#promptModal').modal('show');
                if ($(document).find('.select2picker').length > 0) {
                    $(document).find('.select2picker').select2({});
                }
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
    else {
        $.ajax({
            url: '/Reports/GetTableData',
            type: "POST",
            datatype: "json",
            data: { ReportListingId: rlistid, IsUserReport: rlistuserreport },
            beforeSend: function () {
                /*  ShowLoader();*/
                swal({
                    title: "",
                    text: "The report may take up to " + ReportTimeOut +" minutes - please be patient",
                    imageUrl: "../content/Images/image_1197421.gif",
                    showConfirmButton: false
                });
            },           
            success: function (data) {
                if (data.timeoutError == "Timeout") {
                    ErrorAlertSetting.text = "The transaction is taking too long to run - please modify your selection criteria";
                    swal(ErrorAlertSetting, function () {
                    });
                }
                else {
                    swal.close();
                    ShowLoader();
                    CreateReport(data.data, data.columns, data.repdetail);
                }
            },
            error: function (xhr, status, error) {
                if (xhr.responseText.indexOf("request timed out")!=-1)
                {
                    ErrorAlertSetting.text = "The transaction is taking too long to run - please modify your selection criteria";
                    swal(ErrorAlertSetting, function () {
                    });
                }
                else { swal.close(); }
               
            }
        });
    }
};
$(document).on('click', '#prompedit', function () {
    if ($(document).find('.select2picker').length > 0) {
        $(document).find('.select2picker').select2({});
    }
    if ($('#multisectprompt1').length > 0) {
        $("#multisectprompt1").val(multiselectedval1).trigger('change.select2');
    }
    if ($('#multisectprompt2').length > 0) {
        $("#multisectprompt2").val(multiselectedval2).trigger('change.select2');
    }
    if ($('#cmbdaterange1').length > 0) {
        $("#cmbdaterange1").val(case1).trigger('change');
    }
    if ($('#cmbdaterange2').length > 0) {
        $("#cmbdaterange2").val(case2).trigger('change');
    }
    $(document).find('#promptModal').modal('show');
});
$(document).on('change', '.cmbdaterange', function (e) {
    var thiselement = $(this);
    var thisid = thiselement.attr('id');
    var tcontainer = thiselement.nextAll('.timeperiodcontainer:first');
    if ($(this).val() == 6) {
        if (thisid == 'cmbdaterange1') {
            if (!strdate1) {
                strdate1 = today;
            }
            if (!enddate1) {
                enddate1 = today;
            }
            tcontainer.find('.rptdaterange').daterangepicker({
                format: dtformat,
                startDate: strdate1,
                endDate: enddate1,
                locale: dtrangelocal
            }, function (start, end, label) {
                strdate1 = start.format(dtformat);
                enddate1 = end.format(dtformat);
            });
        }
        if (thisid == 'cmbdaterange2') {
            if (!strdate2) {
                strdate2 = today;
            }
            if (!enddate2) {
                enddate2 = today;
            }
            tcontainer.find('.rptdaterange').daterangepicker({
                format: dtformat,
                startDate: strdate2,
                endDate: enddate2,
                locale: dtrangelocal
            }, function (start, end, label) {
                strdate2 = start.format(dtformat);
                enddate2 = end.format(dtformat);
            });
        }
        tcontainer.show();
    }
    else {
        tcontainer.hide();
    }
});
$(document).on('click', '#btngetreport', function () {
    if ($('#multisectprompt1').length > 0) {
        multiselectedval1 = $("#multisectprompt1").val();
    }
    if ($('#multisectprompt2').length > 0) {
        multiselectedval2 = $("#multisectprompt2").val();
    }
    if ($('#cmbdaterange1').length > 0) {
        case1 = $("#cmbdaterange1").val();
    }
    if (case1 > 0 && case1 < 6) {
        daterangetext1 = $("#cmbdaterange1 option:selected").text();
    }
    else if (case1 == 6) {
        daterangetext1 = $('#rptdaterange1').val();
    }
    else {
        daterangetext1 = '';
    }
    if ($('#cmbdaterange2').length > 0) {
        case2 = $("#cmbdaterange2").val();
    }
    if (case2 > 0 && case2 < 6) {
        daterangetext2 = $("#cmbdaterange2 option:selected").text();
    }
    else if (case2 == 6) {
        daterangetext2 = $('#rptdaterange2').val();
    }
    else {
        daterangetext2 = '';
    }
    $(document).find('#promptModal').modal('hide');
    $.ajax({
        url: '/Reports/GetTableData',
        type: "POST",
        datatype: "json",
        data: {
            ReportListingId: rlistid,
            IsUserReport: rlistuserreport,
            MultiSelectData1: multiselectedval1 ? multiselectedval1.join(',') : '',
            MultiSelectData2: multiselectedval2 ? multiselectedval2.join(',') : '',
            CaseNo1: case1,
            CaseNo2: case2,
            StartDate1: strdate1,
            EndDate1: enddate1,
            StartDate2: strdate2,
            EndDate2: enddate2
        },
        beforeSend: function () {
            /*ShowLoader();*/
            swal({
                title: "",
                text: "The report may take up to " + ReportTimeOut +" minutes - please be patient",
                imageUrl: "../content/Images/image_1197421.gif",
                showConfirmButton: false
            });
        },
        success: function (data) {
            /*swal.close();*/
            if (data.timeoutError == "Timeout") {
                ErrorAlertSetting.text = "The transaction is taking too long to run - please modify your selection criteria";
                swal(ErrorAlertSetting, function () {
                });

            }
            else {
                swal.close();
                ShowLoader();
                cola.push(data.columns);
                CreateReport(data.data, data.columns, data.repdetail);

            }
        },
        error: function (xhr, status, error) {
            if (xhr.responseText.indexOf("request timed out") != -1) {
                ErrorAlertSetting.text = "The transaction is taking too long to run - please modify your selection criteria";
                swal(ErrorAlertSetting, function () {
                });
            }
            else { swal.close(); }
        }
    });
});
function CreateReport(data, columns, repdetail) {
    var pIncludePrompt = false;
    if (IncludePrompt === "True") {
        pIncludePrompt = true;
    }
    ReportDetail = JSON.stringify({ 'ReportDetail': repdetail });
    $.ajax({
        url: '/Reports/GetTable',
        data: ReportDetail,
        type: "POST",
        datatype: "html",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $('#maincontainer').html(data);
        },
        complete: function () {
            reportname = repdetail.ReportName;
            $(document).find('#reportname').text(reportname);

            if (pIncludePrompt === true) {
                if (repdetail.Prompt1Source) {
                    if (multiselectedval1 && multiselectedval1.length > 0) {
                        $(document).find('#lblpropmtselcted1').html('<strong>' + GetPrompt1label() + '</strong> ' + multiselectedval1.join(','));
                    }
                    else if (daterangetext1 && daterangetext1.length > 0) {
                        $(document).find('#lblpropmtselcted1').html('<strong>' + GetPrompt1label() + '</strong> ' + daterangetext1);
                    }
                    else {
                        $(document).find('#lblpropmtselcted1').html('<strong>' + GetPrompt1label() + '</strong> --');
                    }
                }

                if (repdetail.Prompt2Source) {
                    if (multiselectedval2 && multiselectedval2.length > 0) {
                        $(document).find('#lblpropmtselcted2').html('<strong>' + GetPrompt2label() + '</strong> ' + multiselectedval2.join(','));
                    }
                    else if (daterangetext2 && daterangetext2.length > 0) {
                        $(document).find('#lblpropmtselcted2').html('<strong>' + GetPrompt2label() + '</strong> ' + daterangetext2);
                    }
                    else {
                        $(document).find('#lblpropmtselcted2').html('<strong>' + GetPrompt2label() + '</strong> --');
                    }
                }
            }
            if (repdetail.NoExcel == true) {
                $(document).find('#liExcel').remove();
            }
            if (repdetail.NoCSV == true) {
              $(document).find('#liCsv').remove();
            }
            CreateAdvanceSearchRegion(columns, data);    
            GenerateGrid(data, columns, repdetail);     
        },
        error: function () {
            CloseLoader();
        }
    });
}
function GenerateGrid(data, columns, repdetail) {
    var plainformattargetarray = [];
    var numberformattargetarray = [];
    var currencyformattargetarray = [];
    var percentageformattargetarray = [];
    dataSet = JSON.parse(data);
    var totalcolumns = columns.length;
    var lastcolindex = -1;
    var hasGrandTotal = false;
    var groupedTotalColumn = [];
    if (columns.length > 0) {
        groupedTotalColumn = columns.filter(function (x) { return x.IsGroupTotaled == true });
    }
    $.each(columns, function (index, item) {
        if (item.IsGrandTotal === true) {
            hasGrandTotal = true;
            return false;
        }
    });
    if (hasGrandTotal === true) {
        $('#tblfooter').html(GetFooterHtml(totalcolumns));
    }
    else {
        $('#tblfooter').remove();
    }
    if (repdetail.IsGrouped === true && repdetail.GroupColumn) {
        IsGrouped = true;
        GroupColumn = repdetail.GroupColumn;
    }
    if (repdetail.IncludeChild === true && repdetail.ChildSourceName) {
        hasChild = true;
        $('#tblreport').addClass('tblMain haschild');
    }

    $.each(columns, function (index, item) {
        var format = item.NumericFormat;
        if (format) {
            format = format.toUpperCase();
            if (format == plain) {
                plainformattargetarray.push(index);
            }
            if (format == number) {
                numberformattargetarray.push(index);
            }
            if (format == currency) {
                currencyformattargetarray.push(index);
            }
            if (format == percentage) {
                percentageformattargetarray.push(index);
            }
        }
    });
        reporttable = $('#tblreport').DataTable({
            data: dataSet,
            "ordering": false,
            colReorder: {
                fixedColumnsLeft: 1
            },
            sDom: 'Btlipr',
            "pagingType": "full_numbers",
            language: {
                url: "/base/GetDataTableLanguageJson?nGrid=" + true
            },
            "dataSrc": function (result) {
                if (result.data.length < 1) {
                    $(document).find('#reportexport').prop('disabled', true);
                }
                else {
                    $(document).find('#reportexport').prop('disabled', false);
                }
            },
            initComplete: function () {
                if ($('#promptvalcontainer').length > 0) {
                    $('#promptvalcontainer').show();
                }
                DisableExportButton($('#tblreport'), $(document).find('#reportexport'));
                SetPageLengthMenu();
                CloseLoader();

                AddingRemovingAdvSearchItems();
                if (rlistuserreport == "True") {
                    ApplyExistingFilter(columns);
                    DisplayFilter();
                    ReportAdvSearch();
                }
                CreateReportEventLog('Run');
                //-- RKL Mail : Issue with Reports on an on-premise site
                if ($('body').hasClass('modal-open')) {
                    $('body').removeClass('modal-open');
                }
                if ($('.modal-backdrop').length > 0) {
                    $('.modal-backdrop').remove();
                }
                //-- RKL Mail : Issue with Reports on an on-premise site
            },
            "drawCallback": function (settings) {
                // retrieveing group column position from the table
                lastcolindex = -1;
                if (IsGrouped && GroupColumn) {
                    var allCol = reporttable.settings()[0].aoColumns;
                    allCol.forEach(function (item, i) {
                        if (item.data.toLowerCase() === GroupColumn.toLowerCase()) {
                            lastcolindex = i;
                        }
                    });
                }
                //--
                if (lastcolindex > -1) {
                    var api = this.api();
                    var rows = api.rows({ "search": "applied" }).nodes();
                    //var last = null;
                    var aData = new Array();
                    var info = reporttable.page.info();
                    var thisgroup = api.column(lastcolindex, { "search": "applied" }).data()[info.start];
                    var last = thisgroup;
                    //showing group name at the first row of each page 
                    $(rows).eq(info.start).before(
                        '<tr class="group"><td colspan=' + totalcolumns + '>' + thisgroup + '</td></tr>'
                    );

                    api.column(lastcolindex, { "search": "applied" }).data().each(function (group, i) {
                        //showing group name while group changes
                        if (last !== group) {
                            //need not to show group name if group changes occur at first row as it is already added 
                            if (i !== info.start) {
                                $(rows).eq(i).before(
                                    '<tr class="group"><td colspan=' + totalcolumns + '>' + group + '</td></tr>'
                                );
                            }
                            last = group;
                        }
                        if (groupedTotalColumn.length > 0) {
                            var vals = api.row(api.row($(rows).eq(i)).index()).data();
                            if (typeof aData[group] == 'undefined') {
                                aData[group] = new Array();
                                aData[group].rows = [];
                                groupedTotalColumn.forEach(function (val, i) {
                                    aData[group][val.data] = [];
                                });
                            }

                            aData[group].rows.push(i);
                            groupedTotalColumn.forEach(function (val, i) {
                                aData[group][val.data].push(vals[val.data]);
                            });
                        }

                    });

                    if (groupedTotalColumn.length > 0) {
                        var idx = 0;
                        for (var eachData in aData) {
                            var rowhtml = rowCreate(columns);
                            idx = Math.max.apply(Math, aData[eachData].rows);
                            $.each(groupedTotalColumn.map(function (x) { return x.data }), function (i, col) {
                                var sum = 0;
                                $.each(aData[eachData][col], function (k, v) {
                                    sum = sum + intVal(v);
                                });
                                var thiscolumn = groupedTotalColumn.filter(function (x) { return x.data == col });
                                rowhtml = rowhtml.replace('#' + col + '#', GetFormattednumber(thiscolumn[0], sum));
                            });
                            $(rows).eq(idx).after(rowhtml);
                        }
                    }
                }
            },
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api();
                var changedColumns = reporttable.settings()[0].aoColumns;
                $.each(changedColumns, function (i, item) {
                    if (item.IsGrandTotal === true && item.bVisible) {
                        // Total over all pages
                        total = api.column(i, { "search": "applied" }).data().reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                        //#region  DON'T DELETE THIS CODE
                        // Total over this page
                        //pageTotal = api.column(i, { page: 'current' }).data().reduce(function (a, b) {
                        //        return intVal(a) + intVal(b);
                        //    }, 0);

                        // Update footer
                        //$(api.column(i).footer()).html(
                        //    pageTotal.toFixed(4) + ' (' + total.toFixed(4) + ')'
                        //);
                        //#endregion
                        if (end == display.length) {
                            $('#tblfooter').show();
                            $(api.column(i).footer()).html(GetFormattednumber(item, total));
                        } else {
                            $('#tblfooter').hide();
                            $(api.column(i).footer()).html('');
                        }
                    }
                });
            },
            "columnDefs": [
                {
                    "targets": [1],
                    "render": function (data, type, row, meta) {
                        if (hasChild === true) {
                            if (row.ChildCount > 0) {
                                return '<img id="' + data + '" src="../../Images/details_open.png" alt="expand/collapse" rel="' + data + '" style="cursor: pointer;"/>';
                            }
                            else {
                                return '';
                            }
                        }
                        else {
                            return data;
                        }
                    }
                },
                {
                    "targets": plainformattargetarray,
                    "render": function (data, type, row, meta) {
                        var thiscolumnprop = meta.settings.aoColumns[meta.col];
                        if (thiscolumnprop && thiscolumnprop.NumofDecPlaces && thiscolumnprop.NumofDecPlaces != 0) {
                            return getFlooredFixed(data, thiscolumnprop.NumofDecPlaces);
                        }
                        return data;
                    }
                },
                {
                    "targets": numberformattargetarray,
                    "render": function (data, type, row, meta) {
                        var thiscolumnprop = meta.settings.aoColumns[meta.col];
                        if (thiscolumnprop && thiscolumnprop.NumofDecPlaces) {
                            return parseFloat(data).toLocaleString(undefined, { minimumFractionDigits: thiscolumnprop.NumofDecPlaces, maximumFractionDigits: thiscolumnprop.NumofDecPlaces });
                        }
                        return data;
                    }
                },
                {
                    "targets": currencyformattargetarray,
                    "render": function (data, type, row, meta) {
                        var decimelplaces = 0;
                        var thiscolumnprop = meta.settings.aoColumns[meta.col];
                        if (thiscolumnprop && thiscolumnprop.NumofDecPlaces && thiscolumnprop.NumofDecPlaces != 0) {
                            decimelplaces = thiscolumnprop.NumofDecPlaces;
                        }
                        if (thiscolumnprop && thiscolumnprop.CurrencyCode) {
                            return parseFloat(data).toLocaleString(thiscolumnprop.SiteLocalization, { style: 'currency', currency: thiscolumnprop.CurrencyCode, minimumFractionDigits: decimelplaces, maximumFractionDigits: decimelplaces });
                        }
                        return data;
                    }
                },
                {
                    "targets": percentageformattargetarray,
                    "render": function (data, type, row, meta) {
                        var thiscolumnprop = meta.settings.aoColumns[meta.col];
                        if (thiscolumnprop && thiscolumnprop.NumofDecPlaces) {
                            return parseFloat(data).toLocaleString(undefined, { style: 'percent', minimumFractionDigits: thiscolumnprop.NumofDecPlaces, maximumFractionDigits: thiscolumnprop.NumofDecPlaces });
                        }
                        return data;
                    }
                }
            ],
            buttons: [
                {
                    extend: 'excelHtml5',
                    title: reportname,
                    exportOptions: {
                        columns: ":visible"
                    },
                    footer: true
                },
                {
                    extend: 'print',
                    title: reportname,
                    exportOptions: {
                        columns: ":visible"
                    },
                    footer: true
                },
                {
                    text: 'Export CSV',
                    extend: 'csv',
                    exportOptions: {
                        columns: ":visible"
                    },
                    filename: reportname,
                    extension: '.csv',
                    footer: true
                },
                {
                    text: 'Excel',
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: ":visible"
                    },
                    filename: reportname,
                    footer: true
                },
                {
                    text: 'Print',
                    extend: 'pdfHtml5',
                    orientation: 'landscape',
                    title: reportname,
                    exportOptions: {
                        columns: ":visible"
                    },
                    pageSize: 'A3',
                    footer: true
                }
            ],
            "columns": columns
        });

  $(document).find('#tblreport').on('click', 'tbody td img', function (e) {
        var chplainformattargetarray = [];
        var chnumberformattargetarray = [];
        var chcurrencyformattargetarray = [];
        var chpercentageformattargetarray = [];
        var chproperties = [];
        var chtextwraptargetarray = [];

        var tr = $(this).closest('tr');
        var row = $('#tblreport').DataTable().row(tr);
        if (this.src.match('details_close')) {
            this.src = "../../Images/details_open.png";
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            var hasGrandTotalchildgrid = false;
            var childcolumns;
            var childtfootselector;
            this.src = "../../Images/details_close.png";
            var data = $(this).attr("rel");

            ReportDetail = JSON.stringify({ 'ReportDetail': repdetail, "Data": data, IsUserReport: rlistuserreport });

            $.ajax({
                url: '/Reports/GetChildTableData',
                type: "POST",
                datatype: "json",
                contentType: 'application/json; charset=utf-8',
                data: ReportDetail,
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (data) {
                    childcolumns = data.columns;
                    dataSet = JSON.parse(data.data);
                    $.each(dataSet, function (index, item) {
                        for (var key in item) {
                            if (item.hasOwnProperty(key) && typeof item[key] !== 'function' && item[key].length > 50) {
                                chproperties.push(key);
                            }
                        }
                    });

                    $.each(childcolumns, function (index, item) {
                        var format = item.NumericFormat;
                        if (format) {
                            format = format.toUpperCase();
                            if (format == plain) {
                                chplainformattargetarray.push(index);
                            }
                            if (format == number) {
                                chnumberformattargetarray.push(index);
                            }
                            if (format == currency) {
                                chcurrencyformattargetarray.push(index);
                            }
                            if (format == percentage) {
                                chpercentageformattargetarray.push(index);
                            }
                        }
                        if (chproperties && chproperties.length > 0) {
                            if ($.inArray(item.data, chproperties) != -1) {
                                chtextwraptargetarray.push(index);
                            }
                        }
                    });
                    var totalcolumns = childcolumns.length;
                    $.each(data.columns, function (index, item) {
                        if (item.IsGrandTotal === true) {
                            hasGrandTotalchildgrid = true;
                            return false;
                        }
                    });
                    $.ajax({
                        url: '/Reports/GetChildTable',
                        type: "POST",
                        datatype: "html",
                        success: function (data) {
                            row.child(data).show();
                            childtfootselector = row.child().find('.tblfooter');
                            if (hasGrandTotalchildgrid === true) {
                                childtfootselector.html(GetFooterHtml(totalcolumns));
                            }
                            else {
                                childtfootselector.remove();
                            }
                        },
                        complete: function () {
                            dtinnerGrid = row.child().find('.reportinnerDataTable').DataTable(
                                {
                                    data: dataSet,
                                    "order": [[0, "asc"]],
                                    paging: false,
                                    searching: false,
                                    "bProcessing": true,
                                    responsive: true,
                                    scrollY: 300,
                                    "scrollCollapse": true,
                                    sDom: 'Btlipr',
                                    language: {
                                        url: "/base/GetDataTableLanguageJson?nGrid=" + true
                                    },
                                    "columnDefs": [
                                        {
                                            "targets": chplainformattargetarray,
                                            "render": function (data, type, row, meta) {
                                                var thiscolumnprop = meta.settings.aoColumns[meta.col];
                                                if (thiscolumnprop.NumofDecPlaces != 0) {
                                                    return getFlooredFixed(data, thiscolumnprop.NumofDecPlaces);
                                                }
                                                return data;
                                            }
                                        },
                                        {
                                            "targets": chnumberformattargetarray,
                                            "render": function (data, type, row, meta) {
                                                var thiscolumnprop = meta.settings.aoColumns[meta.col];
                                                return parseFloat(data).toLocaleString(undefined, { minimumFractionDigits: thiscolumnprop.NumofDecPlaces, maximumFractionDigits: thiscolumnprop.NumofDecPlaces });
                                            }
                                        },
                                        {
                                            "targets": chcurrencyformattargetarray,
                                            "render": function (data, type, row, meta) {
                                                var decimelplaces = 0;
                                                var thiscolumnprop = meta.settings.aoColumns[meta.col];
                                                if (thiscolumnprop.NumofDecPlaces != 0) {
                                                    decimelplaces = thiscolumnprop.NumofDecPlaces;
                                                }
                                                return parseFloat(data).toLocaleString(thiscolumnprop.SiteLocalization, { style: 'currency', currency: thiscolumnprop.CurrencyCode, minimumFractionDigits: decimelplaces, maximumFractionDigits: decimelplaces });
                                            }
                                        },
                                        {
                                            "targets": chpercentageformattargetarray,
                                            "render": function (data, type, row, meta) {
                                                var thiscolumnprop = meta.settings.aoColumns[meta.col];
                                                return parseFloat(data).toLocaleString(undefined, { style: 'percent', minimumFractionDigits: thiscolumnprop.NumofDecPlaces, maximumFractionDigits: thiscolumnprop.NumofDecPlaces });
                                            }
                                        },
                                        {
                                            "targets": chtextwraptargetarray,
                                            "render": function (data, type, row, meta) {
                                                return "<div class='text-wrap'>" + data + "</div>";
                                            }
                                        }
                                    ],
                                    buttons: [],
                                    initComplete: function () {
                                        row.child().find('.dataTables_scroll').addClass('tblchild-scroll');
                                        row.child().find('.dataTables_scrollBody .tblfooter').hide();
                                        $($.fn.dataTable.tables(true)).DataTable().columns.adjust().fixedColumns().relayout();
                                        CloseLoader();
                                    },
                                    "columns": childcolumns,
                                    "footerCallback": function (row, data, start, end, display) {
                                        var api = this.api();
                                        if (hasGrandTotalchildgrid === true) {
                                            $.each(childcolumns, function (i, item) {
                                                if (item.IsGrandTotal === true) {
                                                    total = api.column(i).data().reduce(function (a, b) {
                                                        return intVal(a) + intVal(b);
                                                    }, 0);
                                                    if (end == data.length) {
                                                        childtfootselector.show();
                                                        $(api.column(i).footer()).html(GetFormattednumber(item, total));
                                                    } else {
                                                        childtfootselector.hide();
                                                        $(api.column(i).footer()).html('');
                                                    }
                                                }
                                            });
                                        }
                                    },
                                });
                            tr.addClass('shown');
                        },
                        error: function () {
                            CloseLoader();
                        }
                    });
                },
                error: function () {
                    CloseLoader();
                }
            });
        }
    });
}
function CreateAdvanceSearchRegion(columns, data) {
    var html = '';
    FilterElements = [];
    $.each(columns, function (index, item) {
        if (item.AvailableOnFilter) {
            if (item.FilterMethod.toLowerCase() === 'string' || item.FilterMethod.toLowerCase() == 'multiselect') {
                FilterElements.push({ 'title': item.title, 'id': item.data });
            }
            if (item.FilterMethod.toLowerCase() == 'string') {
                var label = '<label for="' + item.data + '">' + item.title + '</label>';
                var input = '<input type="text" data-column=' + index + ' class="form-control search adv-item" id="' + item.data + '" autocomplete="off" style="color:#333;">'
                var thishtml = '<div class="form-group">' + label + input + ' </div>';
                html = html + thishtml;
            }
            else if (item.FilterMethod.toLowerCase() == 'multiselect') {
                var column = item.data;
                var columndata = GetColumnData(column, data);
                if (columndata.length > 0) {
                    var options = '';
                    $.each(columndata, function (index, item) {
                        options = options + '<option value="' + item + '">' + item + '</option>';
                    });
                }
                var label = '<label for=' + item.data + '>' + item.title + '</label>';
                var input = '<Select type="select" data-column=' + index + ' class="form-control search select2picker adv-item" id=' + item.data + ' autocomplete="off" style="color:#333;" multiple = "multiple">'
                    + options
                    + '</select> ';
                var thishtml = '<div class="form-group">' + label + input + ' </div>';
                html = html + thishtml;
            }
        }
    });
    $(document).find('#advregion').html(html);
    $(document).find('.select2picker').select2({});
    $(document).find("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
}
function GetColumnData(column, data) {
    var arr = JSON.parse(data);
    var columndata = [];
    for (i = 0; i < arr.length; ++i) {
        var thisitem = arr[i];
        if (thisitem[column] && columndata.includes(thisitem[column]) === false) {
            columndata.push(thisitem[column]);
        }
    }
    return columndata;
}
$(document).on('click', '#btnReportAdvSrch', function () {
    $(document).find('#btnUserReportSave').attr('disabled', false);
    DisplayFilter();
    ReportAdvSearch();
    $(document).find('#sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    if (rlistuserreport == "True") {
        CreateFilterArray();
    }
});
$(document).on('click', '.btnCross', function () {
    $(document).find('#btnUserReportSave').attr('disabled', false);
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.substring(btnCrossed.indexOf('_') + 1);
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    if (rlistuserreport == "True") {
        CreateFilterArray();
    }
    DisplayFilter();
    ReportAdvSearch();
});
 function DisplayFilter() {
    var searchitemhtml = "";
    selectCount = 0;
    $(document).find('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        var searchVal = $(this).val();
        if (searchVal && searchVal.length > 0) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $(document).find("#advsearchfilteritems").html(searchitemhtml);
    if (selectCount > 0) {
        $(document).find("#advsearchfilteritems").show();
    }
    else {
        $(document).find("#advsearchfilteritems").hide();
    }
    $(document).find("#spnControlCounter").text(selectCount);
    $(document).find('#liSelectCount').text(selectCount + ' filters applied');
}
 function ReportAdvSearch() {
  $(document).find('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        var colname = $('label[for="' + $(this).attr('id') + '"]').text();
        var colindex = GetTableColumnIndex(colname);
        var searchVal = $(this).val();
        if (colindex !== undefined) {
            if (searchVal && searchVal.length > 0) {
                if (typeof searchVal === 'string') {
                    reporttable.columns(colindex).search(searchVal).draw();
                }
                else if (typeof searchVal === 'object') {
                    var searchString = '^' + searchVal.join('$|^') + '$';
                    reporttable.columns(colindex).search(searchString, true, false, true).draw();
                }
            }
            else {
                reporttable.columns(colindex).search('').draw();
            }
        }
    });
    DisableExportButtonReport();
}
 function ApplyExistingFilter(columns) {
   $.each(columns, function (item, elem) {
        if (elem.AvailableOnFilter === true) {
            var searchval = elem.Filter;
            if (searchval.length > 0) {
                if (IsJSONString(searchval)) {
                    searchval = JSON.parse(searchval);
                }
            }
            $(document).find('.adv-item#' + elem.data).val(searchval).trigger('change');
        }
    });
}
function IsJSONString(searchval) {
    try {
        JSON.parse(searchval);
    } catch (e) {
        return false;
    }
    return true;
}
function CreateFilterArray() {
    var d = [];
    FilterArray = [];
    $(document).find('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        var searchval = $(this).val();
        if (typeof searchval === "object") {
            if (searchval.length > 0) {
                searchval = JSON.stringify(searchval);
            }
            else {
                searchval = "";
            }
        }
        else {
            searchval = $(this).val();
        }
        var tmp = {
            ColumnName: $(this).attr('id'),
            Filter: searchval
        };
        d.push(tmp);
    });
    FilterArray = d;
}
function DisableExportButtonReport() {
    if (reporttable.page.info().recordsDisplay < 1)
        $(document).find('#reportexport').prop('disabled', true);
    else
        $(document).find('#reportexport').prop('disabled', false);
}
function GetTableColumnIndex(colname) {
    var colindex;
    var columnDetails = reporttable.settings()[0].aoColumns;
    $.each(columnDetails, function (i, item) {
        if (item.sTitle.trim() === colname.trim()) {
            colindex = item.idx;
            return false;
        }
    });
    return colindex;
}
function intVal(i) {
    return typeof i === 'string' ?
        i.replace(/[\$,]/g, '') * 1 :
        typeof i === 'number' ?
            i : 0;
};
function getFlooredFixed(v, d) {
    return (Math.floor(v * Math.pow(10, d)) / Math.pow(10, d)).toFixed(d);
}
function rowCreate(columns) {
    var changedColumns = reporttable.settings()[0].aoColumns;
    var rowstring = '<tr style="color:#575962 !important;">';
    var total = changedColumns.length - 1;
    //-- if group column also aaded as a column in data table for display
    var allColNames = reporttable.settings()[0].aoColumns.filter(function (y) { return y.bVisible === true }).map(function (x) { return x.data.toLowerCase() });
    if (allColNames.indexOf(GroupColumn.toLowerCase()) > -1) {
        total = changedColumns.length;
    }
    //--
    for (var i = 0; i < total; i++) {
        if (changedColumns[i].bVisible) {
            if (changedColumns[i].IsGroupTotaled == true)
                rowstring = rowstring + '<td class="' + changedColumns[i].className + '" align = "right" style="font-weight:500;">#' + changedColumns[i].data + '#</td>';
            else
                rowstring = rowstring + "<td></td>";
        }
    }
    rowstring = rowstring + '</tr>';
    return rowstring;
};
function GetPrompt1label() {
    if (promp1label) {
        return promp1label + labelending;
    }
    else {
        return prom1source + labelending;
    }
}
function GetPrompt2label() {
    if (promp2label) {
        return promp2label + labelending;
    }
    else {
        return prom2source + labelending;
    }
}
function GetFooterHtml(totalcolumns) {
    var footerhtml = '<tr>';
    for (var i = 0; i < totalcolumns; i++) {
        footerhtml = footerhtml + footerhtmlth;
    }
    return footerhtml + '</tr>';
}
function GetFormattednumber(column, numberToFormat) {
    var result = numberToFormat;
    var format = column.NumericFormat;
    if (format) {
        format = format.toUpperCase();
        if (format == plain) {
            if (column.NumofDecPlaces > 0) {
                return getFlooredFixed(numberToFormat, column.NumofDecPlaces);
            }
            return result;
        }
        else if (format == number && column.NumofDecPlaces) {
            return parseFloat(numberToFormat).toLocaleString(undefined, { minimumFractionDigits: column.NumofDecPlaces, maximumFractionDigits: column.NumofDecPlaces });
        }
        else if (format == currency) {
            var decimelplaces = 0;
            if (column.NumofDecPlaces != 0) {
                decimelplaces = column.NumofDecPlaces;
            }
            if (column.CurrencyCode) {
                return parseFloat(numberToFormat).toLocaleString(column.SiteLocalization, { style: 'currency', currency: column.CurrencyCode, minimumFractionDigits: decimelplaces, maximumFractionDigits: decimelplaces });
            }

        }
        else if (format == percentage) {
            if (column.NumofDecPlaces) {
                return parseFloat(numberToFormat).toLocaleString(undefined, { style: 'percent', minimumFractionDigits: column.NumofDecPlaces, maximumFractionDigits: column.NumofDecPlaces });
            }

        }
    }
    return result;
}
//#endregion

//#region CreateEvent
function CreateReportEventLog(event) {
    $.post('/Reports/CreateReportEventLog', {
        ReportListingId: rlistid,
        IsUserReports: rlistuserreport,
        Event: event
    }, function (returnedData) { });
}
//#endregion

//#region Export
$(document).on('click', '#liPdf,#liPrint,#liExcel', function () {
    var thisid = $(this).attr('id');
    if (IsGrouped === true || hasChild === true) {
        var params = {
            ReportListingId: rlistid,
            MultiSelectData1: multiselectedval1 ? multiselectedval1.join(',') : '',
            CaseNo1: case1,
            StartDate1: strdate1 ? strdate1 : '',
            EndDate1: enddate1 ? enddate1 : '',
            MultiSelectData2: multiselectedval2 ? multiselectedval2.join(',') : '',
            CaseNo2: case2,
            StartDate2: strdate2 ? strdate2 : '',
            EndDate2: enddate2 ? enddate2 : '',
            HasChildGrid: hasChild,
            IsGrouped: IsGrouped,
            IsUserReport: rlistuserreport
        };
        //ReportPrintParams = JSON.stringify({ 'ReportPrintParams': params });
        //ColumnsProps = JSON.stringify({'ColumnsProps' : reporttable.settings()[0].aoColumns });
        var FilterProps = [];
        $(document).find('#advsearchsidebar').find('.adv-item:not(.hidden)').each(function (index, item) {
            var colname = $(this).attr('id');//$('label[for="' + $(this).attr('id') + '"]').text();            
            var searchVal = $(this).val();
            var type = '';
            if (searchVal && searchVal.length > 0) {
                if (typeof searchVal === 'string') {
                    type = 'string';
                }
                else if (typeof searchVal === 'object') {
                    type = 'multiselect';
                }
                var elem = {
                    'ColumnName': colname,
                    'Type': type,
                    'Searchval': type === 'multiselect' ? JSON.stringify(searchVal) : searchVal
                };
                FilterProps.push(elem);
            }
        });
        $.ajax({
            "url": "/Reports/SetPrintData",
            "data":
                JSON.stringify({
                    'ReportPrintParams': params,
                    'ColumnsProps': reporttable.settings()[0].aoColumns.map(function (val) {
                        return {
                            'Sequence': val.Sequence,
                            'bVisible': val.bVisible,
                            'data': val.data
                        };
                    }),
                    'FilterProp': FilterProps
                }),
            contentType: 'application/json; charset=utf-8',
            "dataType": "json",
            "type": "POST",
            "success": function () {
                if (thisid == 'liPdf') {
                    window.open('/Reports/ExportASPDF?d=d', '_self');
                }
                else if (thisid == 'liPrint') {
                    window.open('/Reports/ExportASPDF', '_blank');
                }
                else if (thisid == 'liExcel') {
                    window.open('/Reports/ExportASPDF?d=excel', '_self');
                }

                return;
            }
        });
    }
    else {
        if (thisid == 'liPdf') {
            $(".buttons-pdf")[0].click();
        }
        else if (thisid == 'liPrint') {
            $(".buttons-print")[0].click();
        }
        else if (thisid == 'liExcel') {
            $(".buttons-excel")[0].click();
        }
    }
    funcCloseExportbtn();
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    funcCloseExportbtn();
});
//$(document).on('click', "#liExcel", function () {
//    //$(".buttons-excel")[0].click();
//    funcCloseExportbtn();
//});
//#endregion

//#region Customize
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClickReport(reporttable, false);
});
var rselectedCol = [];
var rnselectedCol = [];
var roCols = [];
function funCustomizeBtnClickReport(dtTable, customsort, titleArray) {
    //var allcolumns = reporttable.settings().init().columns;
    rselectedCol = [];
    rnselectedCol = [];
    roCols = [];
    colOrder = [];

    $('#StaffList').multiselect('destroy');
    var vCols = [];
    var mCols = [];
    $('#StaffList option').each(function () { $(this).remove(); });
    $('#PresenterList li').remove();
    $.each(dtTable.settings()[0].aoColumns, function (c) {
        var disabled = false;
        if (dtTable.settings()[0].aoColumns[c].bRequired == true) {
            disabled = true;
        }
        var KeyValuePair = {};
        KeyValuePair = {
            Id: dtTable.colReorder.order()[c],
            Idx: dtTable.settings()[0].aoColumns[c].idx,
            Value: dtTable.settings()[0].aoColumns[c].sTitle,
            Disabled: disabled,
            Selected: dtTable.settings()[0].aoColumns[c].bVisible,
            Order: c,
            Sequence: dtTable.settings()[0].aoColumns[c].Sequence,
            colname: dtTable.settings()[0].aoColumns[c].data,
            Required: dtTable.settings()[0].aoColumns[c].bRequired
        };

        ////------------for uiconfig------------------------

        if (titleArray != null && titleArray.length > 0) {
            if (!(titleArray.includes(KeyValuePair.Value))) {
                roCols.push(KeyValuePair);
                if (!($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
                    vCols.push(KeyValuePair);
                }
                if (dtTable.settings()[0].aoColumns[c].bVisible == true && !($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
                    mCols.push(dtTable.colReorder.order()[c]);
                }
            }
        }
        else {
            roCols.push(KeyValuePair);
            if (!($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
                vCols.push(KeyValuePair);
            }
            if (dtTable.settings()[0].aoColumns[c].bVisible == true && !($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
                mCols.push(dtTable.colReorder.order()[c]);
            }
        }
    });
    vCols = vCols.sort(function (ob1, ob2) {
        return ob1.Id - ob2.Id;
    });
    roCols = roCols.sort(function (ob1, ob2) {
        return ob1.Id - ob2.Id;
    });
    var options = [];
    $.each(vCols, function (i, v) {
        options[i] = {
            label: v.Value,
            title: v.Value,
            value: v.Id,
            selected: v.Selected,
            disabled: v.Disabled,
            attributes: {
                'order': v.Order,
                'Sequence': v.Sequence,
                'colname': v.colname,
                'required': v.Required
            }
        };
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
            if ($(optiona).data('required') === true) {
                optiona[0].disabled = false; // should not be disabled for column's order modification
            }
            if (checked) {
                $('ul').moveToList('#StaffList', '#PresenterList', optiona);
            }
            else {
                $('#PresenterList li').each(function () {
                    if ($(this).data('val').toString() === $(optiona).val())
                        $(this).remove();
                });
            }
            rselectedCol = [];
            rnselectedCol = [];
            $('#StaffList option').each(function () {
                if ($(this).is(":selected"))
                    rselectedCol.push({
                        Id: parseInt($(this).val()),
                        Name: $(this).text()
                    });
                else
                    rnselectedCol.push({
                        Id: parseInt($(this).val()),
                        Name: $(this).text()
                    });
            });
            $(document).find('#lblCounter').text(rselectedCol.length > 0 ? rselectedCol.length : "None");
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
$(document).on('click', '#PresenterList li', function (e) {
    if ($(this).data('removable') === false) {
        $('#reportbtnRemoveAvenger').hide();
    }
    else {
        $('#reportbtnRemoveAvenger').show();
    }
});
$(document).on('click', '#reportbtnRemoveAvenger', function (e) {
    var opts = $('#PresenterList li.activeCol');
    if (opts.length > 0)
        $('#StaffList').multiselect('deselect', $(opts).data('val').toString(), true);
    e.preventDefault();
});
$(document).on('click', '#reportbtnAvengerUp', function (e) {
    $('ul').moveUpDown('#PresenterList', true, false);
    e.preventDefault();
});
$(document).on('click', '#reportbtnAvengerDown', function (e) {
    $('ul').moveUpDown('#PresenterList', false, true);
    e.preventDefault();
});
$(document).on('click', '.reportSaveConfig', function () {
    //-- if all columns are unchecked then an error msg will be returned
    var colcount = $(document).find('#PresenterList li').length;
    if (colcount === 0) {
        var errorMessage = getResourceValue("SelectColumnErrMsg");
        ShowErrorAlert(errorMessage);
        return;
    }
    $("#gridcolumncustomizemodal").modal('hide');
    //------------------------------------------------------------------
    $(document).find('#btnUserReportSave').attr('disabled', false);
    ConfigArray = [];
    var allcolumns = reporttable.settings().init().columns;
    var colOrder = [];
    // Add columns to colOrder that are not part of the configuration
    $.each(allcolumns, function (index, item) {
        if (item.className.indexOf('noVis') > -1 && item.bRequired === false) {
            var tmp = {
                index: index,
                item: index
            }
            colOrder.push(tmp);
        }
    });
    funCustozeSaveBtnReport(reporttable, colOrder);
    run = true;
    reporttable.state.save(run);
    AddingRemovingAdvSearchItems();

    var currentpage = reporttable.page.info().page;
    reporttable.page(currentpage).draw('page');
});
function funCustozeSaveBtnReport(dtTable, colOrder) {
    //Maintaing colspan when columns gets added or removed in no data available state in grid
    var colOrderNew = [];
    $('.dataTables_empty').attr('colspan', '100%');
    $("#PresenterList li").each(function () {
        var name = $(this).find('span').text();
        $.each(roCols, function (k, l) {
            if (name === l.Value) {
                colOrderNew.push(l.Id);
                //dtTable.columns(l.Id).visible(true);
                dtTable.columns(l.Idx).visible(true);
            }
        });
    });
    $.each(rnselectedCol, function (o, g) {
        $.each(roCols, function (k, l) {
            if (g.Name === l.Value) {
                colOrderNew.push(l.Id);
                dtTable.columns(l.Idx).visible(false);
            }
        });
    });
    $.each(colOrder, function (index, item) {
        colOrderNew.splice(item.item, 0, item.item);
    });

    dtTable.colReorder.reset();
    dtTable.colReorder.order(colOrderNew);

    if (rlistuserreport == "True") {
        var allcolumns = reporttable.settings().init().columns;
        var allColumnsNew = [];
        $.each(colOrderNew, function (index, item) {
            allColumnsNew.push(allcolumns[item])
        });
        var d = [];
        $('#StaffList option').each(function () {
            var column = $(this).data('colname');
            var display = 0;
            var sequence = allColumnsNew.map(function (x) { return x.data; }).indexOf(column);
            if ($(this).is(":selected"))
                display = 1;
            else
                display = 0;

            var tmp = {
                ColumnName: column,
                Sequence: sequence + 1,
                Display: display
            }
            d.push(tmp);
        });
        ConfigArray = d;
    }
}
 function AddingRemovingAdvSearchItems() { 
        if (FilterElements.length > 0) {
            var isColExists = false;
            var isAnyColHide = false;

             $.each(FilterElements, function (_i, elem) {
                    isColExists = false;
  
                        $('#tblreport').find('thead > tr > th').each(function (_j, item) {
                            if (item.innerText.trim() === elem.title.trim()) {
                                isColExists = true;
                                return false;
                            }
                        });
                   
                    if (isColExists === true) {
                        if ($('#advregion').find('label[for="' + elem.id + '"]').hasClass('hidden')) {
                            $('#advregion').find('label[for="' + elem.id + '"]').removeClass('hidden');
                        }
                        if ($('#advregion').find('#' + elem.id).hasClass('select2picker')) {
                            $('#advregion').find('#' + elem.id).select2().next().show();
                        }
                        if ($('#advregion').find('#' + elem.id).hasClass('hidden')) {
                            $('#advregion').find('#' + elem.id).removeClass('hidden');
                        }

                    }
                    else {
                        isAnyColHide = true;
                        $('#advregion').find('label[for="' + elem.id + '"]').addClass('hidden');
                        if ($('#advregion').find('#' + elem.id).hasClass('select2picker')) {
                            $('#advregion').find('#' + elem.id).val('').trigger('change');
                            $('#advregion').find('#' + elem.id).select2().next().hide();
                        }
                        else {
                            $('#advregion').find('#' + elem.id).val('').trigger('change').addClass('hidden');
                        }
                    }
                });
            if (isAnyColHide) {
                ReportAdvSearch();
                DisplayFilter();
            }
        }
}
//#endregion

//#region add  or edit private report

$(document).on('click', '#AddPrivateReport', function () {
    $.ajax({
        url: "/Reports/PrivateReportAddOrEdit",
        type: "GET",
        dataType: 'html',
        data: { UserReportsId: 0, SourceId: rlistid, IsUserReport: rlistuserreport },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#PrivateReportPopup').html(data);
            $('#PrivateReportModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            CloseLoader();
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function PrivateReportAddEditOnSuccess(data) {
    CloseLoader();
    $(document).find('.threeDotDrop').css("display", "none");
    if (data.Result == "success") {
        if (data.Mode == "Add") {
            SuccessAlertSetting.text = getResourceValue('spnReportSuccess');
        }
        else {
            SuccessAlertSetting.text = getResourceValue('spnReportUpdateSuccess');
        }
        swal(SuccessAlertSetting, function () {
            $("#PrivateReportModalpopup").modal('hide');
            if (data.Mode == "Edit") {
                window.location.href = '/Reports/Index?page=Reports_Reports__beta_';
            }
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditReportPrivate(elem) {
    var UserReportsId = $(elem).data("id");

    $.ajax({
        url: "/Reports/PrivateReportAddOrEdit",
        type: "GET",
        dataType: 'html',
        data: { UserReportsId: UserReportsId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#PrivateReportPopup').html(data);
            $('#PrivateReportModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            CloseLoader();
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
    return false;
}
$(document).on('click', "#btnPrivateReportcancel", function () {
    swal(CancelAlertSetting, function () {
        $("#PrivateReportModalpopup").modal('hide');
        $(document).find('.errormessage').css("display", "none");
        $(document).find('.threeDotDrop').css("display", "none");
        $('body').removeClass('modal-open');
    });

});

//#endregion

//#region add  or edit site report

$(document).on('click', '#AddSiteReport', function () {
    $.ajax({
        url: "/Reports/SiteReportAddOrEdit",
        type: "GET",
        dataType: 'html',
        data: { UserReportsId: 0, SourceId: rlistid, IsUserReport: rlistuserreport },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#SiteReportPopup').html(data);
            $('#SiteReportModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            CloseLoader();
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function SiteReportAddEditOnSuccess(data) {
    CloseLoader();
    $(document).find('.threeDotDrop').css("display", "none");
    if (data.Result == "success") {
        if (data.Mode == "Add") {
            SuccessAlertSetting.text = getResourceValue('spnReportSuccess');
        }
        else {
            SuccessAlertSetting.text = getResourceValue('spnReportUpdateSuccess');
        }
        swal(SuccessAlertSetting, function () {
            $("#SiteReportModalpopup").modal('hide');
            if (data.Mode == "Edit") {
                window.location.href = '/Reports/Index?page=Reports_Reports__beta_';
            }
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditReportSite(elem) {
    var UserReportsId = $(elem).data("id");

    $.ajax({
        url: "/Reports/SiteReportAddOrEdit",
        type: "GET",
        dataType: 'html',
        data: { UserReportsId: UserReportsId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#SiteReportPopup').html(data);
            $('#SiteReportModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            CloseLoader();
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
    return false;
}
$(document).on('click', "#btnSiteReportcancel", function () {
    swal(CancelAlertSetting, function () {
        $("#SiteReportModalpopup").modal('hide');
        $(document).find('.errormessage').css("display", "none");
        $(document).find('.threeDotDrop').css("display", "none");
        $('body').removeClass('modal-open');
    });

});

//#endregion

//#region add  or edit public report

$(document).on('click', '#AddPublicReport', function () {
    $.ajax({
        url: "/Reports/PublicReportAddOrEdit",
        type: "GET",
        dataType: 'html',
        data: { UserReportsId: 0, SourceId: rlistid, IsUserReport: rlistuserreport },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#PublicReportPopup').html(data);
            $('#PublicReportModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            CloseLoader();
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function PublicReportAddEditOnSuccess(data) {
    CloseLoader();
    $(document).find('.threeDotDrop').css("display", "none");
    if (data.Result == "success") {
        if (data.Mode == "Add") {
            SuccessAlertSetting.text = getResourceValue('spnReportSuccess');
        }
        else {
            SuccessAlertSetting.text = getResourceValue('spnReportUpdateSuccess');
        }
        swal(SuccessAlertSetting, function () {
            $("#PublicReportModalpopup").modal('hide');
            if (data.Mode == "Edit") {
                window.location.href = '/Reports/Index?page=Reports_Reports__beta_';
            }
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditReportPublic(elem) {
    var UserReportsId = $(elem).data("id");

    $.ajax({
        url: "/Reports/PublicReportAddOrEdit",
        type: "GET",
        dataType: 'html',
        data: { UserReportsId: UserReportsId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#PublicReportPopup').html(data);
            $('#PublicReportModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: '.cls-report-name' });
            $.validator.unobtrusive.parse(document);
            $(document).find('.select2picker').select2({});
            //$('input, form').blur(function () {
            //    $(this).valid();
            //});
            //SetControls();
            $("#male").prop("checked", true);
        },
        error: function () {
            CloseLoader();
        }
    });
    return false;
}
$(document).on('click', "#btnPublicReportcancel", function () {
    swal(CancelAlertSetting, function () {
        $("#PublicReportModalpopup").modal('hide');
        $(document).find('.errormessage').css("display", "none");
        $(document).find('.threeDotDrop').css("display", "none");
        $('body').removeClass('modal-open');
    });
});

//#endregion

//#region delete

function DeleteUserReport(elem) {
    var UserReportsId = $(elem).data("id");
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: "/Reports/DeleteUserReport",
            type: "GET",
            data: { UserReportsId: UserReportsId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                var message = "";
                if (data == "success") {
                    $(document).find('.threeDotDrop').css("display", "none");
                    SuccessAlertSetting.text = getResourceValue('spnReportDeleted');
                    swal(SuccessAlertSetting, function () {
                        if (data == "success") {
                            window.location.href = '/Reports/Index?page=Reports_Reports__beta_';
                        }
                    });
                }
                else {
                    message = "Failed in report delete";
                    ShowErrorAlert(message);
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
}

//#endregion

//#region common

function ShowDot(elem) {
    var dataId = $(elem).data("id");

    if ($('#Action_' + dataId).is(":visible")) {
        $(document).find('#Action_' + dataId).css("display", "none");
    } else {
        $(document).find('.threeDotDrop').css("display", "none");
        $(document).find('#Action_' + dataId).css("display", "block");
    }
    return false;
}
function SetControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    //$('input, form').blur(function () {
    //    $(this).valid();
    //});
    //$('.select2picker, form').change(function () {
    //    var areaddescribedby = $(this).attr('aria-describedby');
    //    if ($(this).valid()) {
    //        if (typeof areaddescribedby != 'undefined') {
    //            $('#' + areaddescribedby).hide();
    //        }
    //    }
    //    else {
    //        if (typeof areaddescribedby != 'undefined') {
    //            $('#' + areaddescribedby).show();
    //        }
    //    }
    //});
    $(document).find('.select2picker').select2({});
    SetFixedHeadStyle();

}
$(document).on('click', ".clearerrdiv", function () {
    $(document).find('.errormessage').css("display", "none");
});
//#endregion

//#region Save user report settings
$(document).on('click', '#btnUserReportSave', function () {
    if (rlistuserreport == "True") {
        if (ConfigArray.length > 0 && FilterArray.length > 0) {
            ReportDetail = JSON.stringify({ 'ReportId': parseInt(rlistid), "config": ConfigArray, "filterConfig": FilterArray });
        }
        else if (ConfigArray.length > 0) {
            ReportDetail = JSON.stringify({ 'ReportId': parseInt(rlistid), "config": ConfigArray });
        }
        else if (FilterArray.length > 0) {
            ReportDetail = JSON.stringify({ 'ReportId': parseInt(rlistid), "filterConfig": FilterArray });
        }
        if (ConfigArray.length > 0 || FilterArray.length > 0) {
            $.ajax({
                url: "/Reports/SaveUsersReportSettings",
                type: "POST",
                datatype: "json",
                contentType: 'application/json; charset=utf-8',
                data: ReportDetail,
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (data) {
                    if (data == "success") {
                        SuccessAlertSetting.text = getResourceValue('spnUserReportConfigurationSave');
                        swal(SuccessAlertSetting, function () { });
                        CreateReportEventLog('Save');
                    }
                    else {
                        ShowGenericErrorOnAddUpdate(data);
                    }
                },
                complete: function () {
                    CloseLoader();
                    //SetControls();
                },
                error: function () {
                    CloseLoader();
                }
            });
        }
    }
});
//#endregion
//#region V2-1073
function LoadDevExpressReport(DevExpressRptName) {
    CreateReportEventLog('Run');
    if (DevExpressRptName == "Purchase Order by Account") {
        var form = document.createElement("form");
        form.method = "POST";
        form.action = "/Reports/PrintPOByAccount";
        form.target = "_blank"; // This will open the form submission in a new tab/window

        // Add any input data you need to send to the server
        //var input = document.createElement("input");
        //input.type = "hidden";
        //input.name = "model"; // The name of the parameter your controller expects
        //input.value = "some data"; // The data you want to send
        //form.appendChild(input);

        // Append form to body
        document.body.appendChild(form);

        // Submit the form
        form.submit();

        // Remove the form from the DOM
        document.body.removeChild(form);
    }
}
//#endregion