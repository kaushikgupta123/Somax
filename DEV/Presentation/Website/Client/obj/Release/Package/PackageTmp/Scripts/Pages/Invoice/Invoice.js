var invoiceSearchdt;
var invoListSearchdt
var invoicerecieptList;
var statussearchval;
var selectedcount = 0;
var totalcount = 0;
var searchcount = 0;
var searchresult = [];
var IsCheckAllTrue;
var argValStat;
var run = false;
$(function () {
  ShowbtnLoader("btnsortmenu");
  ShowbtnLoaderclass("LoaderDrop");
  $(document).on('click', "#action", function () {
    $(".actionDrop").slideToggle();
  });
  $(".actionDrop ul li a").click(function () {
    $(".actionDrop").fadeOut();
  });
  $(document).on('focusout', "#action", function () {
    $(".actionDrop").fadeOut();
  });
  $(document).find("#sidebar").mCustomScrollbar({
    theme: "minimal"
  });
  $(document).on('click', '#dismiss, .overlay', function () {
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
  });
  $(document).find('.select2picker').select2({});

  $(document).on('click', '#drpDwnLink', function (e) {
    e.preventDefault();
    $(document).find("#drpDwn").slideToggle();
  });
  $(document).on('change', '#colorselector', function (evt) {
    $(document).find('.tabsArea').hide();
    openCity(evt, $(this).val());
    $('#' + $(this).val()).show();
  });
});

$(document).ready(function () {
  $(".actionBar").fadeIn();
  $("#InvoiceGridAction :input").attr("disabled", "disabled");
});

$(document).on('click', '#pinIRsidebarCollapse,#sidebarCollapse', function () {
  $('#renderinvoice').find('.sidebar').addClass('active');
  $('.overlay').fadeIn();
  $('.collapse.in').toggleClass('in');
  $('a[aria-expanded=true]').attr('aria-expanded', 'false');
  $(document).find('.dtpicker').datepicker({
    changeMonth: true,
    changeYear: true,
    "dateFormat": "mm/dd/yy",
    autoclose: true
  }).inputmask('mm/dd/yyyy');
});
function openCity(evt, cityName) {
  evt.preventDefault();
  switch (cityName) {
    case "InvoiceOverview":
      var InvID = $(document).find('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
      RedirectInvoiveDetail(InvID);
      break;
    case "INVNotes":
      generateInvNotesGrid();
      break;
    case "INVAttachments":
      generateInvAttachmentGrid();
      break;
  }
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
$(document).on('click', "#InvoiceOverViewSidebar", function () {
  $(document).find('#Active').css('display', 'block');
  generateInvoiceLineItemListDataTable();
});
$(document).on('click', "ul.vtabs li", function () {
  if ($(this).find('#drpDwnLink').length > 0) {
    $("ul.vtabs li").removeClass("active");
    $(this).addClass("active");
    return false;
  }
  else {
    $("ul.vtabs li").removeClass("active");
    $(this).addClass("active");
    $(".tabsArea").hide();
    var activeTab = $(this).find("a").attr("href");
    $(activeTab).fadeIn();
    return false;
  }
});
$(function () {
  var dropkey = localStorage.getItem("DROPKEY");
  if (dropkey) {
    argValStat = dropkey;
    generateInvoiceDataTable();
    $('#scheduleInvoiceList').val(dropkey).trigger('change.select2');
  }
  else {
    argValStat = 0;
    generateInvoiceDataTable();
  }
});
$(document).on('click', ".addInvoice", function (e) {
  $.ajax({
    url: "/Invoice/AddInvoiceMatchHeader",
    type: "GET",
    dataType: 'html',
    beforeSend: function () {
      ShowLoader();
    },
    success: function (data) {
      $('#renderinvoice').html(data);
    },
    complete: function () {
      SetControls();
    },
    error: function () {
      CloseLoader();
    }
  });
});
function SetControls() {
  CloseLoader();
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
  $(document).find('.select2picker').select2({});
  $(document).find('.dtpicker').datepicker({
    changeMonth: true,
    changeYear: true,
    "dateFormat": "mm/dd/yy",
    autoclose: true
  }).inputmask('mm/dd/yyyy');
};
var selectedStatus;
$(document).on('click', '#liPdf', function () {
  $(".buttons-pdf")[0].click();
  $('#mask').trigger('click');
});
$(document).on('click', '#liCsv', function () {
  $(".buttons-csv")[0].click();
  $('#mask').trigger('click');
});
$(document).on('click', "#liExcel", function () {
  $(".buttons-excel")[0].click();
  $('#mask').trigger('click');
});
$(document).on('click', '#liPrint', function () {
  $(".buttons-print")[0].click();
  $('#mask').trigger('click');
});

function generateInvoiceDataTable() {
  var printCounter = 0;
  if ($(document).find('#tblinvoice').hasClass('dataTable')) {
    invoiceSearchdt.destroy();
  }
  invoiceSearchdt = $("#tblinvoice").DataTable({
    colReorder: {
      fixedColumnsLeft: 1
    },
    rowGrouping: true,
    searching: true,
    serverSide: true,
    "pagingType": "full_numbers",
    "bProcessing": true,
    "bDeferRender": true,
    "order": [[0, "asc"]],
    stateSave: true,
    "stateSaveCallback": function (settings, data) {
      // Send an Ajax request to the server with the state object
      if (run == true) {
        $.ajax({
          "url": gridStateSaveUrl,//"/Base/CreateUpdateState",
          "data": {
            GridName: "InvoiceMatch_Search",
            LayOutInfo: JSON.stringify(data)
          },
          "dataType": "json",
          "type": "POST",
          "success": function () { return; }
        });
      }
      run = false;
    },
    "stateLoadCallback": function (settings, callback) {
      var o;
      $.ajax({
        "url": gridStateLoadUrl,
        "data": {
          GridName: "InvoiceMatch_Search",
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {
          if (json) {
            callback(JSON.parse(json));
          }
          else {
            callback(json);
          }

        }
      });
    },
    scrollX: true,
    fixedColumns: {
      leftColumns: 1
    },
    language: {
      url: "/base/GetDataTableLanguageJson?nGrid=" + true,
    },
    sDom: 'Btlipr',
    buttons: [
      {
        extend: 'excelHtml5',
        title: 'Invoice List'
      },
      {
        extend: 'print',
        title: 'Invoice List'
      },

      {
        text: 'Export CSV',
        extend: 'csv',
        filename: 'Invoice List',
        extension: '.csv'
      },
      {
        text: 'Print',
        extend: 'pdfHtml5',
        exportOptions: {
          columns: ':visible'
        },
        css: 'display:none',
        title: 'Invoice List',
        orientation: 'landscape',
        pageSize: 'A3'
      }
    ],
    "orderMulti": true,
    "ajax": {
      "url": "/Invoice/GetInvoiceMaintGrid",
      "type": "post",
      "datatype": "json",
      data: function (d) {
        d.StatusId = argValStat;
        d.invoice = LRTrim($("#GAinvoice").val());
        d.status = $("#GAstatus").val();
        d.vendor = LRTrim($("#GAvendor").val());
        d.vendorname = LRTrim($("#GAvendorname").val());
        d.receiptdate = ValidateDate($("#GAreceiptdate").val());
        d.purchaseorder = LRTrim($("#GApurchaseorder").val());
        d.invoicedate = ValidateDate($("#GAinvoicedate").val());
      },
      "dataSrc": function (result) {
          if (result.data.length < 1) {
              $(document).find('.import-export').prop('disabled', true);
          }
          else {
              $(document).find('.import-export').prop('disabled', false);
          }
        var option = "";
        var statusList = result.statuslist;
        if (statusList) {
          option += '<option value="">--Select--</option>';

          for (var i = 0; i < statusList.length - 1; i++) {
            option += '<option value="' + statusList[i] + '">' + getStatusValue(statusList[i]) + '</option>';
          }
        }
        $(document).find('#GAstatus').empty().html(option);
        if (selectedStatus) {
          $(document).find('#GAstatus').val(selectedStatus);
        }
        HidebtnLoaderclass("LoaderDrop");
        HidebtnLoader("btnsortmenu");
        return result.data;
      },
      global: true
    },
    "columns":
      [
        {
          "data": "ClientLookupId",
          "autoWidth": true,
          "bSearchable": true,
          "bSortable": true,
          "className": "text-left",
          "name": "0",
          "mRender": function (data, type, row) {
            return '<a class=lnk_invoice href="javascript:void(0)">' + data + '</a>';
          }
        },
        {
          "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
          render: function (data, type, row, meta) {
            if (data == statusCode.Paid) {
              return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
            }
            else if (data == statusCode.Open) {
              return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
            }
            else if (data == statusCode.AuthorizedToPay) {
              return "<span class='m-badge m-badge-grid-cell m-badge--yellow m-badge--wide'>" + getStatusValue(data) + "</span >";
            }
            else {
              return getStatusValue(data);
            }
          }
        },
        { "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
        { "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
        { "data": "ReceiptDate", "autoWidth": true, "bSearchable": true, "bSortable": false, "type": "date" },
        { "data": "InvoiceDate", "autoWidth": true, "bSearchable": true, "bSortable": false, "type": "date" },
        { "data": "POClientLookUpId", "autoWidth": true, "bSearchable": true, "bSortable": false }
      ],
    "columnDefs": [
      {
        render: function (data, type, full, meta) {
          return "<div class='text-wrap width-100'>" + data + "</div>";
        },
        targets: [2]
      },
      {
        targets: [0],
        className: 'noVis'
      }

    ],
    initComplete: function () {
      SetPageLengthMenu();
      if (statussearchval) {
        $("#GAstatus").val(statussearchval).trigger('change.select2');
      }
      var currestsortedcolumn = $('#tblinvoice').dataTable().fnSettings().aaSorting[0][0];
      var column = this.api().column(currestsortedcolumn);
      var columnId = $(column.header()).attr('id');
      switch (columnId) {
        case "thIdInvoice":
          EnableInVoiceColumnSorting();
          break;
        case "thIdStatus":
          EnableStatusColumnSorting();
          break;
        case "thIdVendor":
          EnableVendorColumnSorting();
          break;
        case "thIdVendorName":
          EnableVendorNameColumnSorting();
          break;
      }
      $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + column.header().innerHTML);

      $("#InvoiceGridAction :input").removeAttr("disabled");
      $("#InvoiceGridAction :button").removeClass("disabled");
    }
  });
}

$(document).on('click', '#tblinvoice_paginate .paginate_button', function () {
  run = true;
});
$(document).on('change', '#tblinvoice_length .searchdt-menu', function () {
  run = true;
});
function EnableInVoiceColumnSorting() {
  $('.DTFC_LeftWrapper').find('#thIdInvoice').css('pointer-events', 'auto');
  if ($(document).find('#thIdStatus').length > 0) {
    document.getElementById('thIdStatus').style.pointerEvents = 'none';
  }
  if ($(document).find('#thIdVendor').length > 0) {
    document.getElementById('thIdVendor').style.pointerEvents = 'none';
  }
  if ($(document).find('#thIdVendorName').length > 0) {
    document.getElementById('thIdVendorName').style.pointerEvents = 'none';
  }
}
function EnableStatusColumnSorting() {
  if ($(document).find('.th-IdInvoice').length > 0) {
    document.getElementById('thIdStatus').style.pointerEvents = 'auto';
  }
  $('.DTFC_LeftWrapper').find('#thIdInvoice').css('pointer-events', 'none');
  if ($(document).find('#thIdVendor').length > 0) {
    document.getElementById('thIdVendor').style.pointerEvents = 'none';
  }
  if ($(document).find('#thIdVendorName').length > 0) {
    document.getElementById('thIdVendorName').style.pointerEvents = 'none';
  }
}
function EnableVendorColumnSorting() {
  if ($(document).find('.th-IdInvoice').length > 0) {
    document.getElementById('thIdVendor').style.pointerEvents = 'auto';
  }
  $('.DTFC_LeftWrapper').find('#thIdInvoice').css('pointer-events', 'none');
  if ($(document).find('#thIdStatus').length > 0) {
    document.getElementById('thIdStatus').style.pointerEvents = 'none';
  }
  if ($(document).find('#thIdVendorName').length > 0) {
    document.getElementById('thIdVendorName').style.pointerEvents = 'none';
  }
}
function EnableVendorNameColumnSorting() {
  if ($(document).find('.th-IdInvoice').length > 0) {
    document.getElementById('thIdVendorName').style.pointerEvents = 'auto';
  }
  $('.DTFC_LeftWrapper').find('#thIdInvoice').css('pointer-events', 'none');
  if ($(document).find('#thIdStatus').length > 0) {
    document.getElementById('thIdStatus').style.pointerEvents = 'none';
  }
  if ($(document).find('#thIdVendor').length > 0) {
    document.getElementById('thIdVendor').style.pointerEvents = 'none';
  }
}
$(document).find('.srtInvMcolumn').click(function () {
  ShowbtnLoader("btnsortmenu");
  var col = $(this).data('col');
  switch (col) {
    case 0:
      EnableInVoiceColumnSorting();
      $('.DTFC_LeftBodyWrapper').find('#thIdInvoice').trigger('click');
      break;
    case 1:
      EnableStatusColumnSorting();
      $('#thIdStatus').trigger('click');
      break;
    case 2:
      EnableVendorColumnSorting();
      $('#thIdVendor').trigger('click');
      break;
    case 3:
      EnableVendorNameColumnSorting();
      $('#thIdVendorName').trigger('click');
      break;
  }

  $('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + $(this).text());
  $(document).find('.srtInvMcolumn').removeClass('sort-active');
  $(this).addClass('sort-active');
  run = true;
});
$(function () {
  jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
    if (this.context.length) {
      var optionval = $('#scheduleInvoiceList option:selected').val();
      var Ginvoice = LRTrim($("#GAinvoice").val());
      var Gstatus = $("#GAstatus").val();
      var Gvendor = LRTrim($("#GAvendor").val());
      var Gvendorname = LRTrim($("#GAvendorname").val());
      var Greceiptdate = LRTrim($("#GAreceiptdate").val());
      var Gpurchaseorder = LRTrim($("#GApurchaseorder").val());
      var Ginvoicedate = LRTrim($("#GAinvoicedate").val());
      dtTable = $("#tblinvoice").DataTable();
      var currestsortedcolumn = $('#tblinvoice').dataTable().fnSettings().aaSorting[0][0];
      var coldir = $('#tblinvoice').dataTable().fnSettings().aaSorting[0][1];
      var colname = $('#tblinvoice').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
      var jsonResult = $.ajax({
        url: '/Invoice/GetPRPrintData?page=all',
        data: {
          CustomQueryDisplayId: optionval,
          invoice: Ginvoice,
          status: Gstatus,
          vendor: Gvendor,
          vendorname: Gvendorname,
          receiptdate: Greceiptdate,
          purchaseorder: Gpurchaseorder,
          invoicedate: Ginvoicedate,
          colname: colname,
          coldir: coldir
        },
        success: function (result) {
        },
        async: false
      });
      var thisdata = JSON.parse(jsonResult.responseText).data;
      var visiblecolumnsIndex = $("#tblinvoice thead tr th").map(function (key) {
        return this.getAttribute('data-th-index');
      }).get();

      var d = [];
      $.each(thisdata, function (index, item) {
        if (item.ClientLookupId != null) {
          item.ClientLookupId = item.ClientLookupId;
        }
        else {
          item.ClientLookupId = "";
        }
        if (item.Status != null) {
          item.Status = item.Status;
        }
        else {
          item.Status = "";
        }
        if (item.VendorClientLookupId != null) {
          item.VendorClientLookupId = item.VendorClientLookupId;
        }
        else {
          item.VendorClientLookupId = "";
        }
        if (item.VendorName != null) {
          item.VendorName = item.VendorName;
        }
        else {
          item.VendorName = "";
        }
        if (item.ReceiptDate != null) {
          item.ReceiptDate = item.ReceiptDate;
        }
        else {
          item.ReceiptDate = "";
        }
        if (item.InvoiceDate != null) {
          item.InvoiceDate = item.InvoiceDate;
        }
        else {
          item.InvoiceDate = "";
        }
        if (item.POClientLookupId != null) {
          item.POClientLookupId = item.POClientLookupId;
        }
        else {
          item.POClientLookupId = "";
        }
        var fData = [];
        $.each(visiblecolumnsIndex, function (index, inneritem) {
          var key = Object.keys(item)[inneritem];
          var value = item[key]
          fData.push(value);
        });
        d.push(fData);
      })
      return {
        body: d,
        header: $("#tblinvoice thead tr th div").map(function (key) {
          return this.innerHTML;
        }).get()
      };
    }
  });
})
$(document).on('change', '#scheduleInvoiceList', function () {
  ShowbtnLoaderclass("LoaderDrop");
  run = true;

  var optionval = $('#scheduleInvoiceList option:selected').val();
  argValStat = optionval;
  localStorage.setItem("DROPKEY", optionval);
  if (optionval.length !== 0) {
    invoiceSearchdt.page('first').draw('page');
  }

});
$("#btnDataAdvSrchInvoice").on('click', function (e) {
  searchresult = [];
  statussearchval = $("#GAstatus").val();
  selectedStatus = statussearchval;
  InvoiceAdvSearch();
  $('.sidebar').removeClass('active');
  $('.overlay').fadeOut();
  invoiceSearchdt.page('first').draw('page');
});
function InvoiceAdvSearch() {
  var InactiveFlag = false;
  $("#purchaseRequestModel_ScheduleWorkList").val(0);
  var searchitemhtml = "";
  selectCount = 0;
  $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
    if ($(this).hasClass('dtpicker')) {
      $(this).val(ValidateDate($(this).val()));
    }
    if ($(this).val()) {
      selectCount++;
      searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
    }
  });
  searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
  $("#advsearchfilteritems").html(searchitemhtml);
  $(".filteritemcount").text(selectCount);
  $('#liSelectCount').text(selectCount + ' filters applied');
}
$(document).on('click', '.btnCross', function () {
  var btnCrossed = $(this).parent().attr('id');
  var searchtxtId = btnCrossed.split('_')[1];
  $('#' + searchtxtId).val('').trigger('change');
  $(this).parent().remove();
  selectCount--;
  $(".filteritemcount").text(selectCount);
  if (searchtxtId == "GAstatus") {
    $("#GAstatus").val("").trigger('change.select2');
    selectedStatus = $("#GAstatus").val();
  }
  invoiceSearchdt.page('first').draw('page');
});
function clearAdvanceSearch() {
  $("#GAinvoice").val("");
  $("#GAstatus").val("");
  $("#GAvendor").val("");
  $("#GAvendorname").val("");
  $('#GAreceiptdate').val("");
  $("#GApurchaseorder").val("");
  $("#GAstatus").val("").trigger('change.select2');
  selectedStatus = $("#GAstatus").val();
}
function hGridclearAdvanceSearch() {
  selectCount = 0;
  $(".filteritemcount").text(selectCount);
  $('#advsearchsidebar').find('input:text').val('');
  $('#advsearchfilteritems').find('span').html('');
  $('#advsearchfilteritems').find('span').removeClass('tagTo');
  $('#GAstatus').val('').trigger('change.select2');
}
$(document).on('click', '#liClearAdvSearchFilter', function () {
  run = true;
  argValStat = 0;
  $("#scheduleInvoiceList").val(0).trigger('change.select2');
  $(".filteritemcount").text("0");
  localStorage.removeItem("DROPKEY");
  statussearchval = null;
  clearAdvanceSearch();
  hGridclearAdvanceSearch();
  invoiceSearchdt.page('first').draw('page');
});
$(document).on('click', '#dismiss, .overlay', function () {
  $('.sidebar').removeClass('active');
  $('.overlay').fadeOut();
});
$(document).on('click', '.lnk_invoice', function (e) {
  var row = $(this).parents('tr');
  var data = invoiceSearchdt.row(row).data();
  $.ajax({
    url: "/Invoice/InvoiceDetails",
    type: "POST",
    data: { invoiceId: data.InvoiceMatchHeaderId },
    dataType: 'html',
    beforeSend: function () {
      ShowLoader();
    },
    success: function (data) {
      $('#renderinvoice').html(data);
      CloseLoader();
    },
    complete: function () {
      generateInvoiceLineItemListDataTable();
    },
    error: function () {
      CloseLoader();
    }
  });
});

$(document).on('click', '#ChangeInvoiceDrop', function () {
  $(document).find('#ChangeInvoice').modal('show');
});

function generateInvoiceLineItemListDataTable() {
  var visibility = "True";
  var srcData = LRTrim($('#txtitemsearchbox').val());
  var InvoiceMatchHeaderId = $("#InvoiceMatchHeaderModel_InvoiceMatchHeaderId").val();
  var GAlinenumber = LRTrim($("#GAlinenumber").val());
  var GAdescription = LRTrim($("#GAdescription").val());
  var GAquantity = LRTrim($("#GAquantity").val());
  var GAunitofmeasure = LRTrim($("#GAunitofmeasure").val());
  var GAunitcost = LRTrim($("#GAunitcost").val());
  var GAtotalcost = LRTrim($("#GAtotalcost").val());
  var GApurchaseOrder = LRTrim($("#GApurchaseOrder").val());
  var GAaccount = LRTrim($("#GAaccount").val());
  if ($(document).find('#tblInvoiceItemList').hasClass('dataTable')) {
    invoListSearchdt.destroy();
  }
  invoListSearchdt = $("#tblInvoiceItemList").DataTable({
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
      "url": "/Invoice/GetInvoiceLineItemsGrid",
      "type": "post",
      data: function (d) {
        d.srcData = srcData;
        d.invoiceMatchHeaderId = InvoiceMatchHeaderId;
        d.linenumber = GAlinenumber;
        d.description = GAdescription;
        d.quantity = GAquantity;
        d.unitofmeasure = GAunitofmeasure;
        d.unitcost = GAunitcost;
        d.totalcost = GAtotalcost;
        d.purchaseOrder = GApurchaseOrder;
        d.account = GAaccount;
      },
      "datatype": "json"
    },
    columnDefs: [
      {
        "data": null,
        targets: [8], render: function (a, b, data, d) {
          if (visibility == "True") {
            return '<a class="btn btn-outline-success editinvoiceitembtn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
              '<a class="btn btn-outline-danger delinvoiceitemBtn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
          }
          else {
            return "";
          }
        }
      }
    ],
    "columns":
      [
        { "data": "LineNumber", "autoWidth": true, "bSearchable": true, "bSortable": true },
        {
          "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
          "mRender": function (data, type, row) {
            return "<div class='text-wrap width-200'>" + data + "</div>";
          }
        },
        { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true },
        { "data": "UnitOfMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },
        { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true },
        { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true },
        { "data": "PurchaseOrder", "autoWidth": true, "bSearchable": true, "bSortable": true },
        { "data": "Account", "autoWidth": true, "bSearchable": true, "bSortable": true },
        { "bSortable": false, "className": "text-center" }
      ],
    initComplete: function () {
      SetPageLengthMenu();
    }
  });
}
$(document).on('click', "#ListsidebarCollapse", function () {
  $('#renderinvoice').find('.sidebar').addClass('active');
  $('.overlay').fadeIn();
  $('.collapse.in').toggleClass('in');
  $('a[aria-expanded=true]').attr('aria-expanded', 'false');
  $(document).find('.dtpicker').datepicker({
    changeMonth: true,
    changeYear: true,
    "dateFormat": "mm/dd/yy",
    autoclose: true
  }).inputmask('mm/dd/yyyy');
});
$(document).on('click', '#dismiss, .overlay', function () {
  $('.sidebar').removeClass('active');
  $('.overlay').fadeOut();
});
$(document).on('click', '#btnInvoiceitemSearch', function () {
  $('#advsearchsidebarListItemInvoice').find('input:text').each(function () { $(this).val(''); });
  generateInvoiceLineItemListDataTable();
});
$(document).on('click', '#btnDataAdvSrchListInvoiceItem', function () {
  searchresult = [];
  invoListSearchdt.state.clear();
  InvoiceListAdvSearch();
  $('.sidebar').removeClass('active');
  $('.overlay').fadeOut();
});
function InvoiceListAdvSearch() {
  var InactiveFlag = false;
  var searchitemhtmlListInvo = "";
  selectCount = 0;
  $('#advsearchsidebarListItemInvoice').find('.adv-item').each(function (index, item) {
    if ($(this).val()) {
      selectCount++;
      searchitemhtmlListInvo = searchitemhtmlListInvo + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossListItem" aria-hidden="true"></a></span>';
    }
  });
  generateInvoiceLineItemListDataTable();
  $("#advsearchfilteritems").html(searchitemhtmlListInvo);
  $(".filteritemcount").text(selectCount);
}
$(document).on('click', '.btnCrossListItem', function () {
  var btnCrossed = $(this).parent().attr('id');
  var searchtxtId = btnCrossed.split('_')[1];
  $('#' + searchtxtId).val('').trigger('change');
  $(this).parent().remove();
  selectCount--;
  InvoiceListAdvSearch();
});
$(document).on('click', '#liClearAdvSearchFilterListInvoice', function () {
  $('#txtitemsearchbox').val('');
  invoListSearchdt.state.clear();
  $('#advsearchsidebarListItemInvoice').find('input:text').each(function () { $(this).val(''); });
  hGridclearAdvanceSearch();
  generateInvoiceLineItemListDataTable();
});
$(document).on('click', '.editinvoiceitembtn', function () {
  var data = invoListSearchdt.row($(this).parents('tr')).data();
  var invoiceMatchItemId = data.InvoiceMatchItemId;
  var InvoiceMatchHeaderId = $("#InvoiceMatchHeaderModel_InvoiceMatchHeaderId").val();
  var clientLookupId = $("#InvoiceMatchHeaderModel_ClientLookupId").val();
  $.ajax({
    url: "/Invoice/EditInvoiceListItem",
    type: "GET",
    dataType: 'html',
    data: { InvoiceMatchItemId: invoiceMatchItemId, InvoiceId: InvoiceMatchHeaderId, ClientLookupId: clientLookupId },
    beforeSend: function () {
      ShowLoader();
    },
    success: function (data) {
      $('#renderinvoice').html(data);
    },
    complete: function () {
      SetControls();
    },
    error: function () {
      CloseLoader();
    }
  });
});
function ListItemEditOnSuccess(data) {
  CloseLoader();
  if (data.data === "success") {
    var message;
    SuccessAlertSetting.text = getResourceValue("InvoiceMatchLineItemUpdateSuccessAlert");
    swal(SuccessAlertSetting, function () {
      RedirectInvoiveDetail(data.InvoiceMatchHeaderId);
    });
  }
  else {
    ShowGenericErrorOnAddUpdate(data);
  }
}
function RedirectInvoiveDetail(invoiceMatchHeaderId) {
  $.ajax({
    url: "/Invoice/InvoiceDetails",
    type: "POST",
    dataType: 'html',
    data: { invoiceId: invoiceMatchHeaderId },
    beforeSend: function () {
      ShowLoader();
    },
    success: function (data) {
      $('#renderinvoice').html(data);
      CloseLoader();
    },
    complete: function () {
      generateInvoiceLineItemListDataTable();
    },
    error: function () {
      CloseLoader();
    }
  });
}
$(document).on('click', '.delinvoiceitemBtn', function () {
  var data = invoListSearchdt.row($(this).parents('tr')).data();
  var headerId = data.InvoiceMatchHeaderId;
  var ItemId = data.InvoiceMatchItemId;
  DeleteInvoiceItem(headerId, ItemId);
});
function DeleteInvoiceItem(headerId, ItemId) {
  swal(CancelAlertSetting, function () {
    $.ajax({
      url: '/Invoice/DeleteInvoiceitem',
      data: {
        MatchHeaderId: headerId, MatchItemId: ItemId
      },
      type: "POST",
      datatype: "json",
      beforeSend: function () {
        ShowLoader();
      },
      success: function (data) {
        if (data.Result == "success") {
          RedirectInvoiveDetail(headerId);
          ShowDeleteAlert(getResourceValue("InvoiceMatchLineItemDeleteSuccessAlert"));
        }
        else {
          if (data == "spnCandeltNotOpenItem") {
            var msg = getResourceValue(data);
            GenericSweetAlertMethod(msg);
          }
          else {
            GenericSweetAlertMethod(data);
          }
        }
      },
      complete: function () {
        CloseLoader();
      }
    });
  });
}
$(document).on('click', '#AddInvoiceReceipt', function () {
  var invoiceMatchItemId = $(document).find('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
  var clientLookupId = $(document).find('#InvoiceMatchHeaderModel_ClientLookupId').val();
  $.ajax({
    url: "/Invoice/AddInvoiceReceipt",
    type: "GET",
    dataType: 'html',
    data: { InvoiceMatchItemId: invoiceMatchItemId, ClientLookupId: clientLookupId },
    beforeSend: function () {
      ShowLoader();
    },
    success: function (data) {
      $('#renderinvoice').html(data);
    },
    complete: function () {
      SetControls();
    },
    error: function () {
      CloseLoader();
    }
  });
});
function OnSuccessAddReceipts(data) {
  CloseLoader();
  if (data.data == "success") {
    if (data.Command == "save") {
      var message;
      SuccessAlertSetting.text = getResourceValue("InvoiceMatchLineItemAddSuccessAlert");
      swal(SuccessAlertSetting, function () {
        RedirectInvoiveDetail(data.InvoiceMatchHeaderId);
      });
    }
    else {
      ResetErrorDiv();
      $('#InvoiceOverview').addClass('active').trigger('click');
      SuccessAlertSetting.text = getResourceValue("AddPartsAlerts");
      swal(SuccessAlertSetting, function () {
        $(document).find('form').trigger("reset");
        $(document).find('form').find("select").val("").trigger('change.select2');
        $(document).find('form').find("select").removeClass("input-validation-error");
        $(document).find('form').find("input").removeClass("input-validation-error");
        $(document).find('form').find("textarea").removeClass("input-validation-error");
      });
    }
  }
  else {
    ShowGenericErrorOnAddUpdate(data);
  }
}
$(document).on('click', '#selectReceiptGrid', function () {
  var vendorId = $(document).find('#InvoiceMatchHeaderModel_VendorId').val();
  var invoiceMatchHeaderId = $('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
  var clientLookupId = $('#InvoiceMatchHeaderModel_ClientLookupId').val();
  $.ajax({
    url: "/Invoice/RenderReceiptView",
    type: "POST",
    dataType: "html",
    data: { VendorId: vendorId, InvoiceMatchHeaderId: invoiceMatchHeaderId, ClientLookupId: clientLookupId },
    beforeSend: function () {
      ShowLoader();
    },
    success: function (data) {
      $('#renderinvoice').html(data);
    },
    complete: function () {
      CloseLoader();
      GenerateReceiptGrid();
    },
    error: function () {
      CloseLoader();
    }
  });
});
function GenerateReceiptGrid() {
  var vendorId = $(document).find('#InvoiceMatchHeaderModel_VendorId').val();
  var _srcData = LRTrim($("#SPGPopuptxtSearch").val());
  var _PurchaseOrder = LRTrim($('#txtPurchaseOrder').val());
  var _ReceivedDate = LRTrim($('#txtReceivedDate').val());
  var _PartID = LRTrim($('#txtPartID').val());
  var _Description = LRTrim($('#txtDescription').val());
  var _QuantityReceived = LRTrim($('#txtQuantityReceived').val());
  var _UnitofMeasure = LRTrim($('#txtUnitofMeasure').val());
  var _UnitCost = LRTrim($('#txtUnitCost').val());
  var _TotalCost = LRTrim($('#txtTotalCost').val());
  IsCheckAllTrue = $("#fgidselectall").prop("checked");
  if ($(document).find('#tblSelectReceiptGrid').hasClass('dataTable')) {
    invoicerecieptList.destroy();
  }
  invoicerecieptList = $("#tblSelectReceiptGrid").DataTable({
    colReorder: true,
    rowGrouping: true,
    searching: true,
    serverSide: true,
    "pagingType": "full_numbers",
    "bProcessing": true,
    "bDeferRender": true,
    "order": [[1, "asc"]],
    stateSave: true,
    language: {
      url: "/base/GetDataTableLanguageJson?nGrid=" + true,
    },
    sDom: 'Btlipr',
    buttons: [],
    "orderMulti": true,
    "ajax": {
      "url": "/Invoice/PopulateRecieptData",
      "type": "post",
      "datatype": "json",
      data: function (d) {
        d.VendorId = vendorId;
        d.srcData = _srcData;
        d.PurchaseOrder = _PurchaseOrder;
        d.ReceivedDate = _ReceivedDate;
        d.PartID = _PartID;
        d.Description = _Description;
        d.QuantityReceived = _QuantityReceived;
        d.UnitofMeasure = _UnitofMeasure;
        d.UnitCost = _UnitCost;
        d.TotalCost = _TotalCost;
      },
      "dataSrc": function (result) {
        searchcount = result.recordsTotal;
        $.each(result.data, function (index, item) {
          searchresult.push(item.PurchaseRequestId);
        });
        if (totalcount < result.recordsTotal)
          totalcount = result.recordsTotal;
        if (totalcount != result.recordsTotal)
          selectedcount = result.recordsTotal;
        return result.data;
      },
      global: true
    },
    "columns":
      [
        {
          "data": "POReceiptItemId",
          orderable: false,
          "bSortable": false,
          className: 'select-checkbox dt-body-center',
          targets: 0,
          'render': function (data, type, full, meta) {
            var found = InvoiceRecieptSelectedItemArray.some(function (el) {
              return el.POReceiptItemID === data;
            });
            if (found) {
              return '<input type="checkbox" data-eqid="' + data + '" class="chksearch"  checked  value="' + $('<div/>').text(data).html() + '">';
            }
            else {
              return '<input type="checkbox" data-eqid="' + data + '" class="chksearch"  value="' + $('<div/>').text(data).html() + '">';
            }
          }
        },
        { "data": "POClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
        { "data": "ReceivedDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
        { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
        {
          "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
          mRender: function (data, type, full, meta) {
            return "<div class='text-wrap width-400'>" + data + "</div>";
          }
        },
        { "data": "QuantityReceived", "autoWidth": true, "bSearchable": true, "bSortable": true },
        { "data": "UnitOfMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },
        { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true },
        { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true },
        { "data": "AccountId", "autoWidth": true, "bSearchable": true, "bSortable": true }
      ],
    "columnDefs": [
      {
        "targets": [9],
        "visible": false
      }
    ]
  });
}
var InvoiceRecieptSelectedItemArray = [];
$(document).on('change', '.chksearch', function () {
  var el = $('#fgidselectall').get(0);
  if (el && el.checked && ('indeterminate' in el)) {
    el.indeterminate = true;
  }
  var invoiceMatchHeaderId = $('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
  var data = invoicerecieptList.row($(this).parents('tr')).data();
  if (!this.checked) {
    InvoiceRecieptSelectedItemArray = InvoiceRecieptSelectedItemArray.filter(function (el) {
      return el.POReceiptItemID !== data.POReceiptItemId;
    });
  }
  else {
    var item = new PartNotInInventorySelectedItem(invoiceMatchHeaderId, data.POReceiptItemId, data.POClientLookupId,
      data.PartClientLookupId, data.Description, data.QuantityReceived,
      data.UnitOfMeasure, data.UnitCost, data.TotalCost, data.AccountId
    );
    var found = InvoiceRecieptSelectedItemArray.some(function (el) {
      return el.POReceiptItemID === data.POReceiptItemId;
    });
    if (!found) { InvoiceRecieptSelectedItemArray.push(item); }
  }
});
function PartNotInInventorySelectedItem(InvoiceMatchHeaderId, POReceiptItemId, POClientLookupId, PartClientLookupId, Description, QuantityReceived, UnitOfMeasure, UnitCost, TotalCost, AccountId) {
  this.InvoiceMatchHeaderId = InvoiceMatchHeaderId;
  this.POReceiptItemID = POReceiptItemId;
  this.POClientLookupId = POClientLookupId;
  this.PartClientLookupId = PartClientLookupId;
  this.Description = Description;
  this.Quantity = QuantityReceived;
  this.UnitOfMeasure = UnitOfMeasure;
  this.UnitCost = UnitCost;
  this.TotalCost = TotalCost;
  this.AccountId = AccountId;
};
$(document).on('click', "#btnprocessReceipt", function (e) {
  if (InvoiceRecieptSelectedItemArray.length < 1) {
    ShowGridItemSelectionAlert();
    e.preventDefault();
    return false;
  }
  else {
    GeneratedfinalSelectPartsTable(InvoiceRecieptSelectedItemArray);
  }
});
function GeneratedfinalSelectPartsTable(datasource1) {
  InvoiceRecieptSelectedItemArray = [];
  var InvoiceMatchHeaderId = $('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
  $.each(datasource1, function (index, item) {
    var POReceiptItemId = item.POReceiptItemID;
    var POClientLookupId = item.POClientLookupId;
    var ReceivedDate = item.ReceivedDate;
    var PartClientLookupId = item.PartClientLookupId;
    var Description = item.Description;
    var QuantityReceived = item.QuantityReceived;
    var UnitOfMeasure = item.UnitOfMeasure;
    var UnitCost = item.UnitCost;
    var TotalCost = item.TotalCost;
    var AccountId = item.AccountId;
    var item = new PartNotInInventorySelectedItem(InvoiceMatchHeaderId, POReceiptItemId, POClientLookupId,
      PartClientLookupId, Description, QuantityReceived,
      UnitOfMeasure, UnitCost, TotalCost, AccountId
    );
    InvoiceRecieptSelectedItemArray.push(item);
  });
  var list = JSON.stringify({ 'list': InvoiceRecieptSelectedItemArray });
  $.ajax({
    url: "/Invoice/SaveListRecieptFromGrid",
    type: "POST",
    dataType: "json",
    data: list,
    contentType: 'application/json; charset=utf-8',
    beforeSend: function () {
      ShowLoader();
    },
    success: function (data) {
      if (data.Result == "success") {
        InvoiceRecieptSelectedItemArray = [];
        FinalGridSelectedItemArray = [];
        SuccessAlertSetting.text = getResourceValue("LineItemAddedAlert");
        swal(SuccessAlertSetting, function () {
          invoicerecieptList.state.clear();
          RedirectInvoiveDetail(data.invoiceMatchHeaderId);
        });
      }
      else {
        var msgEror = getResourceValue("UpdateAlert");
        ShowGenericErrorOnAddUpdate(msgEror);
      }
    },
    complete: function () {
      CloseLoader();

    },
    error: function () {
      CloseLoader();
    }
  });
}
$(document).on('click', '#fgidselectall', function (e) {
  var checked = this.checked;
  IsCheckAllTrue = checked;
  if (checked) {
    var _VendorId = $(document).find('#InvoiceMatchHeaderModel_VendorId').val();
    var _PurchaseOrder = LRTrim($('#txtPurchaseOrder').val());
    var _ReceivedDate = LRTrim($('#txtReceivedDate').val());
    var _PartID = LRTrim($('#txtPartID').val());
    var _Description = LRTrim($('#txtDescription').val());
    var _QuantityReceived = LRTrim($('#txtQuantityReceived').val());
    var _UnitofMeasure = LRTrim($('#txtUnitofMeasure').val());
    var _UnitCost = LRTrim($('#txtUnitCost').val());
    var _TotalCost = LRTrim($('#txtTotalCost').val());
    $.ajax({
      "url": "/Invoice/GetAllRecieptData",
      data: {
        VendorId: _VendorId,
        PurchaseOrder: _PurchaseOrder,
        ReceivedDate: _ReceivedDate,
        PartID: _PartID,
        Description: _Description,
        QuantityReceived: _QuantityReceived,
        UnitofMeasure: _UnitofMeasure,
        UnitCost: _UnitCost,
        TotalCost: _TotalCost
      },
      async: true,
      type: "post",
      beforeSend: function () {
        ShowLoader();
      },
      datatype: "json",
      success: function (data) {
        if (data) {
          InvoiceRecieptSelectedItemArray = [];
          $(document).find('.chksearch').prop('checked', 'checked');
          $.each(data, function (index, item) {
            var invoiceMatchHeaderId = $('#InvoiceMatchHeaderModel_InvoiceMatchHeaderId').val();
            var item = new PartNotInInventorySelectedItem(invoiceMatchHeaderId, item.POReceiptItemId, item.POClientLookupId,
              item.PartClientLookupId, item.Description, item.QuantityReceived,
              item.UnitOfMeasure, item.UnitCost, item.TotalCost, item.AccountId
            );
            InvoiceRecieptSelectedItemArray.push(item);
          });
        }
      },
      complete: function () {
        CloseLoader();
      }
    });
  }
  else {
    $(document).find('.chksearch').prop('checked', false);
    InvoiceRecieptSelectedItemArray = [];
  }
});
$(document).on('click', '#brdprlineitem', function () {
  var HeaderId = $(this).attr('data-val');
  RedirectInvoiveDetail(HeaderId);
});
var spartgridselectCount = 0;
$(document).on('click', '#btnSPGAdvanceSearch', function () {
  searchresult = [];
  invoicerecieptList.state.clear();
  SPGAdvSearch();
  $('.sidebar').removeClass('active');
  $('.overlay').fadeOut();
});
function SPGAdvSearch() {
  $("#SPGPopuptxtSearch").val("");
  var searchitemhtml = "";
  spartgridselectCount = 0;
  $('#SPGadvsearchsidebar').find('.adv-item').each(function (index, item) {
    if ($(this).hasClass('dtpicker')) {
      $(this).val(ValidateDate($(this).val()));
    }
    if ($(this).val()) {
      spartgridselectCount++;
      searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times SPGbtnCross" aria-hidden="true"></a></span>';
    }
  });
  GenerateReceiptGrid();
  searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
  $("#SPGadvsearchfilteritems").html(searchitemhtml);
  $('#renderinvoice').find('.sidebar').removeClass('active');
  $(".pinvgridfilteritemcount").text(spartgridselectCount);
  $('.overlay').fadeOut();
}
$(document).on('click', '#ReceiptClearSearch', function () {
  $('#SPGPopuptxtSearch').val('');
  hGridPopUpclearAdvanceSearch();
  GenerateReceiptGrid();
  $(".pinvgridfilteritemcount").text(0);
});
$(document).on('click', '.SPGbtnCross', function () {
  var btnCrossedId = $(this).parent().attr('id');
  var searchtxtId = btnCrossedId.split('_')[1];
  $('#' + searchtxtId).val('').trigger('change');
  $(this).parent().remove();
  spartgridselectCount--;
  SPGAdvSearch();
});
$(document).on('click', '#btnSPGSearch', function () {
  hGridPopUpclearAdvanceSearch();
  invoicerecieptList.state.clear();
  GenerateReceiptGrid();
  $(".pinvgridfilteritemcount").text(0);
});
function hGridPopUpclearAdvanceSearch() {
  selectCount = 0;
  $(".filteritemcount").text(selectCount);
  $('#SPGadvsearchsidebar').find('input:text,select').val('');
  $('#SPGadvsearchsidebar').find('select').val('').trigger('change.select2');
  $('#SPGadvsearchfilteritems').find('span').html('');
  $('#SPGadvsearchfilteritems').find('span').removeClass('tagTo');
}
//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
  funCustomizeBtnClick(invoiceSearchdt);
});
$(document).on('click', '.saveConfig', function () {
  var colOrder = [0];
  funCustozeSaveBtn(invoiceSearchdt, colOrder);
  run = true;
  invoiceSearchdt.state.save(run);
});
//#endregion


