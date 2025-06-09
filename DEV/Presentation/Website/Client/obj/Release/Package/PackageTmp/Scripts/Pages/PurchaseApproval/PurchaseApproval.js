var dataTabledt;
var selectCount = 0;
var selectedcount = 0;
var totalcount = 0;
var searchcount = 0;
var searchresult = [];
var selectionItemArray = [];
var isTrigger = false;
var tempSchStatus = 1;

function GetRequesSelectedItem(PurchaseRequestId) {
  this.PurchaseRequestId = PurchaseRequestId;
};
$(document).on('click', '#sidebarCollapse', function () {
  $('.sidebar').addClass('active');
  $('.overlay').fadeIn();
  $('.collapse.in').toggleClass('in');
  $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$(document).on('click', '#btnPurchaseApprovalAdvSrch', function () {
  searchresult = [];
  dataTabledt.state.clear();
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
    if ($(this).hasClass('dtpicker')) {
      $(this).val(ValidateDate($(this).val()));
    }
    if ($(this).val()) {
      selectCount++;
      searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
    }
  });
  searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
  var optionvalStat = $('#schStatusId option:selected').val();
  if (optionvalStat.length !== 0) {
    generatePurchaseApprovalDataTable();
  }
  $("#dvFilterSearchSelect2").html(searchitemhtml);
  $(".spnControlCounter").text(selectCount);
}
$(document).on('click', '#liclearadvsearch', function () {
  dataTabledt.state.clear();
  clearAdvanceSearches();
  $('#schStatusId').val(1).trigger('change.select2');
  $('#purchaseApprovalModel_schCreateDateId').val(0).trigger('change.select2');
  generatePurchaseApprovalDataTable();
});
$(document).on('click', '.btnCross', function () {
  var btnCrossed = $(this).parent().attr('id');
  var searchtxtId = btnCrossed.split('_')[1];
  $('#' + searchtxtId).val('').trigger('change');
  $(this).parent().remove();
  selectCount--;
  AWBAdvSearch();
});
$(function () {
  ShowbtnLoaderclass("LoaderDrop");
  $(document).find('.select2picker').select2({
  });
  $(document).find(".sidebar").mCustomScrollbar({
    theme: "minimal"
  });
  $(document).on('click', '#dismiss, .overlay', function () {
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
  });
  isTrigger = true;
  generatePurchaseApprovalDataTable();
  $('#schStatusId').val(1).trigger('change.select2');
});
function clearAdvanceSearches() {
  $('#adv-Request').val("");
  $('#adv-Reason').val("");
  $('#adv-CreatedBy').val("");
  $('#adv-Created').val("");
  $('#adv-VendorId').val("").trigger('change');
  $('#adv-Name').val("");
  $('#adv-Totalcost').val("");
  selectCount = 0;
  $("#dvFilterSearchSelect2").html('');
  $(".spnControlCounter").text(selectCount);
}
function generatePurchaseApprovalDataTable() {
  var Request = LRTrim($('#adv-Request').val());
  var Reason = LRTrim($('#adv-Reason').val());
  var CreatedBy = LRTrim($('#adv-CreatedBy').val());
  var Created = ValidateDate($('#adv-Created').val());
  var VendorId = LRTrim($('#adv-VendorId').val());
  var VendorName = LRTrim($('#adv-Name').val());
  var Totalcost = LRTrim($('#adv-Totalcost').val());
    var StatusId;
    var IncludePRReview = $('#IncludePRReview').val();
    var ShoppingCartIncludeBuyer = $('#ShoppingCartIncludeBuyer').val();
  if (isTrigger) {
    StatusId = 1;
  }
  else {
    StatusId = $('#schStatusId option:selected').val();
  }
  var CreateDates = $('#purchaseApprovalModel_schCreateDateId option:selected').val();
  if ($(document).find('#tbPurchaseApproval').hasClass('dataTable')) {
    dataTabledt.destroy();
  }
  dataTabledt = $("#tbPurchaseApproval").DataTable({
    colReorder: true,
    rowGrouping: true,
    searching: true,
    serverSide: true,
    "bProcessing": true,
    "bDeferRender": true,
    "order": [[2, "asc"]],
    stateSave: true,
    "pagingType": "full_numbers",
    language: {
      url: "/base/GetDataTableLanguageJson?nGrid=" + true,
    },
    sDom: 'Btlipr',
    buttons: [],
    "orderMulti": true,
    "ajax": {
      "url": "/PurchaseApproval/GetPurchaseApprovalData",
      "type": "post",
      "datatype": "json",
      data: function (d) {
        d.StatusTypeId = StatusId;
        d.CreatedDatesId = CreateDates;
        d.PRClientLookupId = Request;
        d.Reason = Reason;
        d.CreatedBy = CreatedBy;
        d.CreatedDate = Created;
        d.VendorClientLookupId = VendorId;
        d.VendorName = VendorName;
        d.TotalCost = Totalcost;
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
          "data": "PurchaseRequestId",
          "bVisible": true,
          "bSortable": false,
          "autoWidth": false,
          "bSearchable": false,
          "mRender": function (data, type, row) {
            return '<img id="' + data + '" src="../../Images/details_open.png" alt="expand/collapse" rel="' + data + '" style="cursor: pointer;"/>';
          }
        },
        {
          "data": "PurchaseRequestId",
          orderable: false,
          "bSortable": false,
          "autoWidth": false,
          className: 'select-checkbox dt-body-center',
          targets: 0,
          'render': function (data, type, full, meta) {
            if ($('#SVsearchsSelectAll').is(':checked') && totalcount == selectedcount) {
              return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="chksearch ' + data + '"  value="'
                + $('<div/>').text(data).html() + '">';
            } else {

              if (selectionItemArray.indexOf(data) != -1) {
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
          "data": "PRClientLookupId",
          "autoWidth": false,
          "bSearchable": true,
          "bSortable": true, "sType": "numeric-comma",
          "mRender": function (data, type, row) {
            return '<a class=lnk_psearch href="javascript:void(0)">' + data + '</a>';
          }
        },
        { "data": "Reason", "autoWidth": false, "bSearchable": true, "bSortable": true, nowrap: "wrap" },
        { "data": "CreatedBy", "autoWidth": false, "bSearchable": true, "bSortable": true, },
        { "data": "CreateDate", "autoWidth": false, "bSearchable": true, "bSortable": true, "sType": "date" },            
        { "data": "BuyerReview", "autoWidth": false, "bSearchable": true, "bSortable": true },
        { "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
        { "data": "VendorName", "autoWidth": false, "bSearchable": true, "bSortable": true },
        { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, className: 'text-right'}
      ],
    "columnDefs": [
      { width: "3%", targets: 0 },
      { width: "3%", targets: 1 },
      {
        render: function (data, type, full, meta) {
          return "<div class='text-wrap width-200'>" + data + "</div>";
        },
        targets: [3]
      }
    ],
    initComplete: function () {
      isTrigger = false;
      SetPageLengthMenu();
      if (totalcount != 0 && (totalcount == selectionItemArray.length || (searchcount != totalcount && arrayContainsArray(selectionItemArray, searchresult) == true))) {
        $('#SVsearchsSelectAll').prop('checked', true);
      } else {
        $('#SVsearchsSelectAll').prop('checked', false);
        }
        /*--V2-820--*/
        var column = this.api().column(6);
        if (IncludePRReview == "True" && ShoppingCartIncludeBuyer == "True") {
            column.visible(true);
        }
        else {
            column.visible(false);
        }
        /*--end--*/
    }
  });
}
$(document).on('click', '.lnk_psearch', function (e) {
  var row = $(this).parents('tr');
  var data = dataTabledt.row(row).data();
  var PurchaseRequestId = data.PurchaseRequestId;
  //window.location.href = "../PurchaseRequest/DetailFromApproval?PurchaseRequestId=" + PurchaseRequestId;
    window.location.href = "../PurchaseRequest/DetailFromApprovalDynamic?PurchaseRequestId=" + PurchaseRequestId;

});
$('#tbPurchaseApproval').on('click', 'tbody td img', function (e) {
  var tr = $(this).closest('tr');
  var row = dataTabledt.row(tr);
  if (this.src.match('details_close')) {
    this.src = "../../Images/details_open.png";
    row.child.hide();
    tr.removeClass('shown');
  }
  else {
    this.src = "../../Images/details_close.png";
    var Siteid = $(this).attr("rel");
    $.get("/PurchaseApproval/PRLineItem/?Siteid=" + Siteid, function (PRLineItemModel) {
      row.child(PRLineItemModel).show();
      row.child().find('.UserGridDataTable').DataTable(
        {
          "order": [[0, "asc"]],
          paging: false,
          searching: false,
          columnDefs: [
            {
              targets: [3, 5,6],
              className: 'text-right'
            }
          ]
        });
      tr.addClass('shown');
    });
  }
});

$(document).on('change', '#schStatusId', function () {
  isTrigger = false;
  ShowbtnLoaderclass("LoaderDrop");
  getSearchOption();
});
$(document).on('change', '#purchaseApprovalModel_schCreateDateId', function () {
  ShowbtnLoaderclass("LoaderDrop");
  getSearchOption();
});
function getSearchOption() {
  var optionvalStat = $('#schStatusId option:selected').val();
  var ss = $('#purchaseApprovalModel_schCreateDateId option:selected').val();
  if (optionvalStat.length !== 0) {
    dataTabledt.state.clear();
    generatePurchaseApprovalDataTable();
  }
}

$(document).on('change', '.chksearch', function () {
  var thisTr = $(this).closest("tr");
  var data = dataTabledt.row($(this).parents('tr')).data();
  if (!this.checked) {
    var el = $('#SVsearchsSelectAll').get(0);
    if (el && el.checked && ('indeterminate' in el)) {
      el.indeterminate = true;
    }
    selectedcount--;
    var index = selectionItemArray.indexOf(data.PurchaseRequestId);
    selectionItemArray.splice(index, 1);
    thisTr.removeClass("checked");
  }
  else {
    selectionItemArray.push(data.PurchaseRequestId);
    thisTr.addClass("checked");
  }
});
$(document).on('click', '#SVsearchsSelectAll', function (e) {
  var Request = LRTrim($('#adv-Request').val());
  var Reason = LRTrim($('#adv-Reason').val());
  var CreatedBy = LRTrim($('#adv-CreatedBy').val());
  var Created = $('#adv-Created').val();
  var VendorId = $('#adv-VendorId').val();
  if (VendorId) {
    VendorId = VendorId.trim();
  }
  var VendorName = LRTrim($('#adv-Name').val());
  var Totalcost = LRTrim($('#adv-Totalcost').val());
  selectionItemArray = [];
  var checked = this.checked;
  var optionvalStat = $('#schStatusId option:selected').val();
  var optionvalCreateDate = $('#purchaseApprovalModel_schCreateDateId option:selected').val();
  searchresult = [];
  var checked = this.checked;
  $.ajax({
    url: '/PurchaseApproval/GetPurchaseApprovalAllData',
    data: {
      StatusTypeId: optionvalStat,
      CreatedDatesId: optionvalCreateDate,
      PRClientLookupId: Request,
      Reason: Reason,
      CreatedBy: CreatedBy,
      CreatedDate: Created,
      VendorClientLookupId: VendorId,
      VendorName: VendorName,
      TotalCost: Totalcost
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
          searchresult.push(item.PurchaseRequestId);
          if (checked) {
            if (selectionItemArray.indexOf(item.PurchaseRequestId) == -1) {
              selectionItemArray.push(item.PurchaseRequestId);
            }

          } else {
            var i = selectionItemArray.indexOf(item.PurchaseRequestId);
            selectionItemArray.splice(i, 1);
          }
        });
      }
    },
    complete: function () {
      dataTabledt.column(1).nodes().to$().each(function (index, item) {
        if (checked) {
          $(this).find('.chksearch').prop('checked', 'checked');

        } else {
          $(this).find('.chksearch').prop('checked', false);
        }
      });
      CloseLoader();
    }
  });
});
$(document).on('click', "#btnDeny", function () {
  if (selectionItemArray.length <= 0) {
    ShowGridItemSelectionAlert();
    return false;
  }
  var list = selectionItemArray;
  list = JSON.stringify({ 'list': list });
  selectionItemArray = [];
  $.ajax({
    url: "/PurchaseApproval/SaveDenyList",
    type: "POST",
    dataType: "json",
    data: list,
    contentType: 'application/json; charset=utf-8',
    beforeSend: function () {
      ShowLoader();
    },
    success: function (data) {
      if (data.Result == "success") {
        $('#SVsearchsSelectAll').prop('checked', false);
        $('.itemcount').text(0);
        SuccessAlertSetting.text = getResourceValue("AlertDeniedSuccess");
        swal(SuccessAlertSetting, function () {
          var optionvalStat = $('#schStatusId option:selected').val();
          dataTabledt.state.clear();
          if (optionvalStat.length !== 0) {
            generatePurchaseApprovalDataTable();
            selectionItemArray = [];
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
$(document).on('click', '#btnReturnToRequester', function () {
  if (selectionItemArray.length <= 0) {
    ShowGridItemSelectionAlert();
    return false;
  }
  else {
    $('#denymodal').modal('show');
  }
});
$(document).on('click', "#btnApprove", function () {
  if (selectionItemArray.length <= 0) {
    ShowGridItemSelectionAlert();
    return false;
  }
  var list = selectionItemArray;
  list = JSON.stringify({ 'list': list });
  selectionItemArray = [];
  $.ajax({
    url: '/PurchaseApproval/SaveApprovalList',
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
          SuccessAlertSetting.text = getResourceValue("spnPuchaseRequestApprovedSuccessfully");
        swal(SuccessAlertSetting, function () {
          var optionvalStat = $('#schStatusId option:selected').val();
          dataTabledt.state.clear();
          if (optionvalStat.length !== 0) {
            generatePurchaseApprovalDataTable();
            selectionItemArray = [];
          }
        });
      }
      else {
        swal({
          title: getResourceValue("CommonErrorAlert"),
          text: ShowGenericErrorOnAddUpdate(msgEror),
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
$(document).on('click', "#btnReturn", function () {
  var DeniedComments = $('#txtComments').val();
  var wOIds = selectionItemArray;
  if (DeniedComments) {
    $.ajax({
      url: '/PurchaseApproval/UpdateReturnToRequester',
      data: {
        wOIds: wOIds,
        Comments: DeniedComments
      },
      type: "POST",
      beforeSend: function () {
        ShowLoader();
      },
      datatype: "json",
      success: function (data) {
        if (data.Result == "success") {
          selectionItemArray = [];
          $('#denymodal').modal('hide');
          SuccessAlertSetting.text = getResourceValue("spnReturnRequestProcessComplteSuccessfully");
          swal(SuccessAlertSetting, function () {
            generatePurchaseApprovalDataTable();
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
  }
  else {
    swal({
      title: getResourceValue("ttlNoReasonSelected"),
      text: getResourceValue("AlertSelectDenied"),
      type: "warning",
      confirmButtonClass: "btn-sm btn-primary",
      confirmButtonText: getResourceValue("SaveAlertOk"),
    });
  }
});
