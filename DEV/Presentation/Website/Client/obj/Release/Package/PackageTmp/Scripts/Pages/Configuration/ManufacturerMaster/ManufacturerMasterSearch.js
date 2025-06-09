//#region Common
var manMasterTable;
var run = false;
var selectCount = 0;
var activeStatus = false;
//#endregion
//#region Search
$(document).ready(function () {
    activeStatus = localStorage.getItem("MANUFACTURERMASTERSEARCHGRIDDISPLAYSTATUS");
    if (activeStatus) {
        if (activeStatus == "false") {
            activeStatus = false;
            $('#manMasterDropdown').val("1").trigger('change.select2');
        }
        else {
            activeStatus = true;
            $('#manMasterDropdown').val("2").trigger('change.select2');
        }
    }
    else {
        activeStatus = false;
    }
  generateManufacturerMasterDataTable();

  ShowbtnLoaderclass("LoaderDrop");
  ShowbtnLoader("btnsortmenu");
  $("#manufacturerMasterAction :input").attr("disabled", "disabled");
  $(".actionBar").fadeIn();
  $(document).find('.select2picker').select2({});
  $("#sidebar").mCustomScrollbar({
    theme: "minimal"
  });
  $('#dismiss, .overlay').on('click', function () {
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
  });
  $('#sidebarCollapse').on('click', function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
  });
});
function generateManufacturerMasterDataTable() {
  var showAddBtn = false;
  var showEditBtn = false;
  var showDeleteBtn = false;
  var printCounter = 0;
  if ($(document).find('#manufacturerMasterSearch').hasClass('dataTable')) {
    manMasterTable.destroy();
  }
  manMasterTable = $("#manufacturerMasterSearch").DataTable({
    colReorder: {
      fixedColumnsLeft: 1,
      fixedColumnsRight: 1
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
      if (run == true) {
        $.ajax({
          "url": "/Base/CreateUpdateState",
          "data": {
            GridName: "ManuFacturerMaster_Search",
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
      $.ajax({
        "url": "/Base/GetState",
        "data": {
          GridName: "ManuFacturerMaster_Search",
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
        title: 'Manufacturer Master'
      },
      {
        extend: 'print',
        title: 'Manufacturer Master',
      },
      {
        text: 'Export CSV',
        extend: 'csv',
        filename: 'Manufacturer Master',
        extension: '.csv'
      },
      {
        text: 'Print',
        extend: 'pdfHtml5',
        title: 'Manufacturer Master',
        orientation: 'landscape',
        pageSize: 'A3'
      }
    ],
    "orderMulti": true,
    "ajax": {
      "url": "/ManufacturerMaster/GetGridDataforManufacturerMaster",
      "type": "post",
      "datatype": "json",
      data: function (d) {
        d.ClientLookupId = LRTrim($('#ManufacturerID').val());
        d.Name = LRTrim($('#Name').val());
        d.Inactive = activeStatus;//LRTrim($('#Inactive').val());
      },
      "dataSrc": function (result) {
        showAddBtn = result.showAddBtn;
        showEditBtn = result.showEditBtn;
        showDeleteBtn = result.showDeleteBtn;
        HidebtnLoader("btnsortmenu");
        HidebtnLoaderclass("LoaderDrop");
        $("#advstatus").empty();
        $("#advstatus").append("<option value=''>" + "--Select--" + "</option>");
        for (var key in result.StatusList) {
          var id = key;
          var name = key;
          $("#advstatus").append("<option value='" + id + "'>" + getStatusValue(name) + "</option>");

        }
        if (result.data.length == "0") {
            $(document).find('.import-export').attr('disabled', 'disabled');
        }
        else {
            $(document).find('.import-export').removeAttr('disabled');
        }

        return result.data;
      },
      complete: function () {
          CloseLoader();
          $("#manufacturerMasterAction :input").not('.import-export').removeAttr("disabled");
          $("#manufacturerMasterAction :button").not('.import-export').removeClass("disabled");
      },
      global: true
    },
    columnDefs: [
      {
        targets: [2], render: function (a, b, data, d) {
          if (showEditBtn) {
            if (showAddBtn) {
              if (showDeleteBtn) {
                return '<a class="btn btn-outline-primary addBtnMmanMaster gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                  '<a class="btn btn-outline-success editBtnMmanMaster gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                  '<a class="btn btn-outline-danger deleteBtnMmanMaster gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
              }
              else {
                return '<a class="btn btn-outline-primary addBtnMmanMaster gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                  '<a class="btn btn-outline-success editBtnMmanMaster gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
              }
            }
            else {
              if (showDeleteBtn) {
                return '<a class="btn btn-outline-success editBtnMmanMaster gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a> ' +
                  '<a class="btn btn-outline-danger deleteBtnMmanMaster gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
              }
              else {
                return '<a class="btn btn-outline-success editBtnMmanMaster gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
              }
            }
          }
          else {
            if (showAddBtn) {
              if (showDeleteBtn) {
                return '<a class="btn btn-outline-primary addBtnMmanMaster gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                  '<a class="btn btn-outline-danger deleteBtnMmanMaster gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
              }
              else {
                return '<a class="btn btn-outline-primary addBtnMmanMaster gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>';
              }
            }
            else {
              if (showDeleteBtn) {
                return '<a class="btn btn-outline-danger deleteBtnMmanMaster gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
              }
              else {
                return "";
              }
            }
          }
        }
      },
      {
        targets: [0, 2],
        className: 'noVis'
      }
    ],
    "columns":
      [
        {
          "data": "ClientLookupId",
          "autoWidth": true,
          "bSearchable": true,
          "bSortable": true,
          "className": "text-left",
          "name": "0"
        },
        { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
        { "className": "text-center", "bSortable": false }
      ],

    initComplete: function () {
      var actionColumn = this.api().column(2);
      if (showAddBtn === false && showEditBtn === false && showDeleteBtn === false) {
        actionColumn.visible(false);
      }
      else {
        actionColumn.visible(true);
      }
      $(document).on('click', '.status', function (e) {
        e.preventDefault();
      });
      SetPageLengthMenu();
    }
  });
};
$(document).on('change', '#manMasterDropdown', function () {
  run = true;
  ShowbtnLoaderclass("LoaderDrop");
  var optionVal = $(document).find("#manMasterDropdown").val();
  if (optionVal == "1") {
    activeStatus = false;
    $('#btnActiveMain').addClass("active");
    $('#btnInactiveMain').removeClass("active");
    localStorage.setItem("MANUFACTURERMASTERSEARCHGRIDDISPLAYSTATUS", false);
    clearAdvanceSearch();
   // manMasterTable.page('first').draw('page');
  //  generateManufacturerMasterDataTable();
  }
  else {
    activeStatus = true;
    $('#btnActiveMain').removeClass("active");
    $('#btnInactiveMain').addClass("active");
    localStorage.setItem("MANUFACTURERMASTERSEARCHGRIDDISPLAYSTATUS", true);
    var searchOption = LRTrim($("#txtVendorDataSrch").val());
    if (searchOption.trim() == null || searchOption.trim() == "") {
      $("#spnControlCounter").text(selectCount);
      $('#liSelectCount').text(selectCount + ' filters applied');
    }
    else {
      clearAdvanceSearch();
    }
   // manMasterTable.page('first').draw('page');
    //generateManufacturerMasterDataTable();
  }
  manMasterTable.page('first').draw('page');
});
$(document).on('click', '#manufacturerMasterSearch_paginate .paginate_button', function () {
  run = true;
});
$(document).on('change', '#manufacturerMasterSearch_length .searchdt-menu', function () {
  run = true;
});
$(document).on('click', '#manufacturerMasterSearch_wrapper th', function () {
  run = true;
});
//#endregion
//#region Add/Edit

$(document).on('click', '.addNewManMaster', function () {
  var data = manMasterTable.row($(this).parents('tr')).data();
  AddManMaster();
});

$(document).on('click', '.addBtnMmanMaster', function () {
  var data = manMasterTable.row($(this).parents('tr')).data();
  AddManMaster();
});

$(document).on('click', '.editBtnMmanMaster', function () {
  var data = manMasterTable.row($(this).parents('tr')).data();
  EditManMaster(data.ManufacturerID, data.ClientLookupId, data.Name, data.Inactive, "update");
});

$(document).on('click', '.deleteBtnMmanMaster', function () {
  var data = manMasterTable.row($(this).parents('tr')).data();
  DeleteManMaster(data.ManufacturerID, data.ClientLookupId, data.Name, data.Inactive);
});

function DeleteManMaster(ManufacturerID, ClientLookupId, Name, Inactive) {
    run = true;
  swal(CancelAlertSetting, function () {
    $.ajax({
      url: '/ManufacturerMaster/DeleteManufacturerMaster',
      data: {
        ManufacturerID: ManufacturerID
      },
      type: "POST",
      datatype: "json",
      beforeSend: function () {
        ShowLoader();
      },
      success: function (data) {
        if (data.Result == "success") {
          //manMasterTable.state.clear();
          ShowDeleteAlert(getResourceValue("ManufacturerMasterDeleteAlert"));
        }
      },
      complete: function () {
        manMasterTable.page('first').draw('page');
        CloseLoader();
      }
    });
  });
}

function EditManMaster(ManufacturerID, ClientLookupId, Name, Inactive) {
  var ClientLookupId = ClientLookupId;
  $.ajax({
    url: "/ManufacturerMaster/EditManMaster",
    type: "GET",
    dataType: 'html',
    data: { ManufacturerID: ManufacturerID, Name: Name, Inactive: activeStatus, ClientLookupId: ClientLookupId },
    beforeSend: function () {
      ShowLoader();
    },
    success: function (data) {
      CloseLoader();
      $('#renderManmaster').html(data);
    },
    complete: function () {
      $.validator.unobtrusive.parse(document);
    },
    error: function () {
      CloseLoader();
    }
  });
}

function AddManMaster() {
  $.ajax({
    url: "/ManufacturerMaster/AddManMaster",
    type: "GET",
    dataType: 'html',
    beforeSend: function () {
      ShowLoader();
    },
    success: function (data) {
      $('#renderManmaster').html(data);
    },
    complete: function () {
      CloseLoader();
      $.validator.setDefaults({ ignore: null });
      $.validator.unobtrusive.parse(document);
    },
    error: function (jqXHR, exception) {
      CloseLoader();
    }
  });
}

$(document).on('click', "#btnCancelAddMM", function () {
  swal(CancelAlertSetting, function () {
    window.location.href = "../ManufacturerMaster/Index?page=Manufacturer_Master";
  });
});

function ManMasterAddOnSuccess(data) {
  CloseLoader();
  if (data.Result == "success") {
    var message;
    if (data.mode == "add") {
      if (data.Command == "save") {
        SuccessAlertSetting.text = getResourceValue("ManufacturerMasterAddAlert");
        swal(SuccessAlertSetting, function () {
          ResetErrorDiv();
          window.location.href = "../ManufacturerMaster/Index?page=Manufacturer_Master";
        });
      }
      else {
        SuccessAlertSetting.text = getResourceValue("ManufacturerMasterAddAlert");
        ResetErrorDiv();
        swal(SuccessAlertSetting, function () {
          $(document).find('form').trigger("reset");
          $(document).find('form').find("input").removeClass("input-validation-error");
        });
      }
    }
    else {
      SuccessAlertSetting.text = getResourceValue("ManufacturerMasterUpdateAlert");
      swal(SuccessAlertSetting, function () {
        ResetErrorDiv();
        window.location.href = "../ManufacturerMaster/Index?page=Manufacturer_Master";
      });
    }

  }
  else {
    ShowGenericErrorOnAddUpdate(data);
  }
}

//#endregion
//#region Export
$(document).on('click', '#liPrint', function () {
  $(".buttons-print")[0].click();
  funcCloseExportbtn();
});
$(document).on('click', '#liCsv', function () {
  $(".buttons-csv")[0].click();
  funcCloseExportbtn();
});
$(document).on('click', '#liPdf', function () {
  $(".buttons-pdf")[0].click();
  funcCloseExportbtn();
});
$(document).on('click', "#liExcel", function () {
  $(".buttons-excel")[0].click();
  funcCloseExportbtn();
});
//#endregion
//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
  funCustomizeBtnClick(manMasterTable, true);
});
$(document).on('click', '.saveConfig', function () {
  var colOrder = [0];
  funCustozeSaveBtn(manMasterTable, colOrder);
  run = true;
  manMasterTable.state.save(run);
});
//#endregion
//#region Advanced Search
$("#btnManMasterDataAdvSrch").on('click', function (e) {
    run = true;
  ManIdVal = $("#advstatus").val();
  ManMasterAdvSearch();
  $('#sidebar').removeClass('active');
  $('.overlay').fadeOut();
  manMasterTable.page('first').draw('page');
});
function ManMasterAdvSearch() {
  var InactiveFlag = false;
  var searchitemhtml = "";
  selectCount = 0;
  $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
    if ($(this).val()) {
      selectCount++;
      searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
    }
  });
  searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
  $('#liSelectCount').text(selectCount + ' filters applied');
  $("#dvFilterSearchSelect2").html(searchitemhtml);
  $("#spnControlCounter").text(selectCount);
}
$(document).on('click', '#liClearAdvSearchFilter', function () {
  run = true;
  clearAdvanceSearch();
  manMasterTable.page('first').draw('page');
});
function clearAdvanceSearch() {
  selectCount = 0;
  $('#advsearchsidebar').find('input:text').val('');
  selectCount = 0;
  $("#spnControlCounter").text(selectCount);
  $('#liSelectCount').text(selectCount + ' filters applied');
  $('#dvFilterSearchSelect2').find('span').html('');
  $('#dvFilterSearchSelect2').find('span').removeClass('tagTo');
}
$(document).on('click', '.btnCross', function () {
    run = true;
  var btnCrossed = $(this).parent().attr('id');
  var searchtxtId = btnCrossed.split('_')[1];
  $('#' + searchtxtId).val('').trigger('change');
  $(this).parent().remove();
  selectCount--;
  if (searchtxtId == "ManufacturerID") {
        $("#ManufacturerID").val('');
  }
  if (searchtxtId == "Name") {
        $("#Name").val('');
  }
  ManMasterAdvSearch();
  manMasterTable.page('first').draw('page');
});
//#endregion Advanced Search
//#region Print
$(function () {
  jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
    if (this.context.length) {
      var currestsortedcolumn = $('#manufacturerMasterSearch').dataTable().fnSettings().aaSorting[0][0];
      var coldir = $('#manufacturerMasterSearch').dataTable().fnSettings().aaSorting[0][1];
      var colname = $('#manufacturerMasterSearch').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
      var valManufacturerID = LRTrim($('#ManufacturerID').val());
      var valName = LRTrim($('#Name').val());
      var valInactiveFlag = activeStatus;
      dtTable = $("#manufacturerMasterSearch").DataTable();
      var currestsortedcolumn = $('#manufacturerMasterSearch').dataTable().fnSettings().aaSorting[0][0];
      var coldir = $('#manufacturerMasterSearch').dataTable().fnSettings().aaSorting[0][1];
      var colname = $('#manufacturerMasterSearch').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
      var jsonResult = $.ajax({
        "url": "/ManufacturerMaster/GetManufacturerMasterPrintData",
        "type": "get",
        "datatype": "json",
        data: {
          colname: colname,
          coldir: coldir,
          _manufacturerID: valManufacturerID,
          _name: valName,
          _inactiveFlag: valInactiveFlag,
        },
        success: function (result) {
        },
        async: false
      });
      var thisdata = JSON.parse(jsonResult.responseText).data;
      var visiblecolumnsIndex = $("#manufacturerMasterSearch thead tr th").not(':eq(2)').map(function (key) {
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
        if (item.Name != null) {
          item.Name = item.Name;
        }
        else {
          item.Name = "";
        }
        if (item.ManufacturerID != null) {
          item.ManufacturerID = item.ManufacturerID;
        }
        else {
          item.ManufacturerID = "";
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
        header: $("#manufacturerMasterSearch thead tr th").not(":eq(2)").find('div').map(function (key) {
          return this.innerHTML;
        }).get()
      };
    }
  });
})
//#endregion Print