var globalNotificationTab;
$('#m_topbar_notification_icon').on('click', function () {
    var newNotification = $('#NewNotificationCount').val();
    if (parseInt(newNotification) == 0) {
        $('#m_topbar_notification_icon .m-nav__link-icon').removeClass('m-animate-shake');
        $('#m_topbar_notification_icon .m-nav__link-badge').removeClass('m-animate-blink');
        $('#notification-dot').removeClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
    }
    // Fetching Record by Active tab
    var Type = $('ul.m-tabs-line--brand').find('li').find('a.active').attr('type');
    if (Type == "Inventory") {
        GetInventoryNotification();
    }
    else if (Type == "Procurement") {
        GetProcurementNotification();
    }
    else if (Type == "System") {
        GetSystemNotification();
    }
    else if (Type == "APM") {
        GetAPMNotification();
    }
    else {
        GetMaintenanceNotification();
    }

});
$('#NotificationMaintenancetab').on('click', function () {
    GetMaintenanceNotification();
});
$('#NotificationInventorytab').on('click', function () {
    GetInventoryNotification();
});
$('#NotificationProcurementtab').on('click', function () {
    GetProcurementNotification();
});
$('#NotificationSystemtab').on('click', function () {
    GetSystemNotification();
});
$('#NotificationAPMtab').on('click', function () {
    GetAPMNotification();
});
function GetMaintenanceNotification() {
    ResetNotificationTab();
    RemoveNotificationUnread();
    globalNotificationTab = 'Maintenance';
    $.ajax({
        url: '/Base/GetMaintenanceNotification',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        beforeSend: function () {
            $(document).find('#maintnotificationbody').html('');
            $('#notificationloader').show();
        },
        type: "GET",
        success: function (data) {
            $('#topbar_maintenence_notifications').html(data);
        },
        complete: function () {
            ResetTopBarNotification();
            $(document).find("#maintnotificationbody").mCustomScrollbar({
                theme: "minimal-dark"
            });
            var newNotification = $('#NewNotificationCount').val();
            $('#notificationCount').text(newNotification + ' New');
            var newNotificationSelectedtabCount = $('#NewNotificationSelectedtabCount').val();
            if (parseInt(newNotificationSelectedtabCount) == 0) {
                $('#notificationMaintenance-dot').removeClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
            }
            else if (parseInt(newNotificationSelectedtabCount) == 1) {
                $('#notificationMaintenance-dot').addClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
            }
        },
        error: function (xhr) {
        }
    });

}

function GetInventoryNotification() {
    ResetNotificationTab();
    RemoveNotificationUnread();
    globalNotificationTab = 'Inventory';
    $.ajax({
        url: '/Base/GetInventoryNotification',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        beforeSend: function () {
            $('#inventorynotificationbody').html('');
            $('#notificationloader').show();
        },
        type: "GET",
        success: function (data) {
            $('#topbar_inventory_notifications').html(data);
        },
        complete: function () {
            ResetTopBarNotification();
            $(document).find("#inventorynotificationbody").mCustomScrollbar({
                theme: "minimal-dark"
            });
            var newNotification = $('#NewNotificationCount').val();
            $('#notificationCount').text(newNotification + ' New');
            var newNotificationSelectedtabCount = $('#NewNotificationSelectedtabCount').val();
            if (parseInt(newNotificationSelectedtabCount) == 0) {
                $('#notificationInventory-dot').removeClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
            }
            else if (parseInt(newNotificationSelectedtabCount) == 1) {
                $('#notificationInventory-dot').addClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
            }
        },
        error: function (xhr) {
        }
    });
}
function GetProcurementNotification() {
    ResetNotificationTab();
    RemoveNotificationUnread();
    globalNotificationTab = 'Procurement';
    $.ajax({
        url: '/Base/GetProcurementNotification',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        beforeSend: function () {
            $('#procnotificationbody').html('');
            $('#notificationloader').show();
        },
        type: "GET",
        success: function (data) {
            $('#topbar_procurement_notifications').html(data);
        },
        complete: function () {
            ResetTopBarNotification();
            $(document).find("#procnotificationbody").mCustomScrollbar({
                theme: "minimal-dark"
            });
            var newNotification = $('#NewNotificationCount').val();
            $('#notificationCount').text(newNotification + ' New');
            var newNotificationSelectedtabCount = $('#NewNotificationSelectedtabCount').val();
            if (parseInt(newNotificationSelectedtabCount) == 0) {
                $('#notificationProcurement-dot').removeClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
            }
            else if (parseInt(newNotificationSelectedtabCount) == 1) {
                $('#notificationProcurement-dot').addClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
            }
        },
        error: function (xhr) {
        }
    });
}
function GetSystemNotification() {
    ResetNotificationTab();
    RemoveNotificationUnread();
    globalNotificationTab = 'System';
    $.ajax({
        url: '/Base/GetSystemNotification',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        beforeSend: function () {
            $('#systemnotificationbody').html('');
            $('#notificationloader').show();
        },
        type: "GET",
        success: function (data) {
            $('#topbar_system_notifications').html(data);
        },
        complete: function () {
            ResetTopBarNotification();
            $(document).find("#systemnotificationbody").mCustomScrollbar({
                theme: "minimal-dark"
            });
            var newNotification = $('#NewNotificationCount').val();
            $('#notificationCount').text(newNotification + ' New');
            var newNotificationSelectedtabCount = $('#NewNotificationSelectedtabCount').val();
            if (parseInt(newNotificationSelectedtabCount) == 0) {
                $('#notificationSystem-dot').removeClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
            }
            else if (parseInt(newNotificationSelectedtabCount) == 1) {
                $('#notificationSystem-dot').addClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
            }
        },
        error: function (xhr) {
        }
    });
}
//#region V2-538
function GetAPMNotification() {
    ResetNotificationTab();
    RemoveNotificationUnread();
    globalNotificationTab = 'APM';
    $.ajax({
        url: '/Base/GetAPMNotification',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        beforeSend: function () {
            $('#apmnotificationbody').html('');
            $('#notificationloader').show();
        },
        type: "GET",
        success: function (data) {
            $('#topbar_apm_notifications').html(data);
        },
        complete: function () {
            ResetTopBarNotification();
            $(document).find("#apmnotificationbody").mCustomScrollbar({
                theme: "minimal-dark"
            });
            var newNotification = $('#NewNotificationCount').val();
            $('#notificationCount').text(newNotification + ' New');
            var newNotificationSelectedtabCount = $('#NewNotificationSelectedtabCount').val();
            if (parseInt(newNotificationSelectedtabCount) == 0) {
                $('#notificationapm-dot').removeClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
            }
            else if (parseInt(newNotificationSelectedtabCount) == 1) {
                $('#notificationapm-dot').addClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
            }
        },
        error: function (xhr) {
        }
    });
}
//#endregion
function GetUnreadNotificationCount(onload) {
    $.ajax({
        url: '/Base/GetUnreadNotificationCount',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        type: "GET",
        success: function (data) {
            $('#notificationCount').text(data.UnreadCount + ' New');
            if (parseInt(data.UnreadCount) > 0 && onload == "onload") {
                $('#m_topbar_notification_icon .m-nav__link-icon').addClass('m-animate-shake');
                $('#m_topbar_notification_icon .m-nav__link-badge').addClass('m-animate-blink');
                $('#notification-dot').addClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
            }
            if (parseInt(data.ResultMaintenanceCount) > 0) {
                $('#notificationMaintenance-dot').addClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
            }
            if (parseInt(data.ResultInventoryCount) > 0) {
                $('#notificationInventory-dot').addClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
            }
            if (parseInt(data.ResultProcurementCount) > 0) {
                $('#notificationProcurement-dot').addClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
            }
            if (parseInt(data.ResultSystemCount) > 0) {
                $('#notificationSystem-dot').addClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
            }
        },
        complete: function () {
        },
        error: function (xhr) {
        }
    });
}
$('#ClearAllNotificationSelectedTab').on('click', function () {
    var Type = $('ul.m-tabs-line--brand').find('li').find('a.active').attr('type');
    $.ajax({
        url: '/Base/ClearNotificationSelectedTab',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        type: "GET",
        data: {
            Type: Type
        },
        success: function (data) {
            if (data.Result == "success") { ReloadSelectedTabAfterClearAll(Type); }
            else { GenericSweetAlertMethod(data); }

        },
        complete: function () {

        },
        error: function (xhr) {
        }
    });

});

function ReloadSelectedTabAfterClearAll(type) {
    if (type == "Maintenance") {
        GetMaintenanceNotification();
    }
    else if (type == "Inventory") {
        GetInventoryNotification();
    }
    else if (type == "Procurement") {
        GetProcurementNotification();
    }
    else if (type == "System"){
        GetSystemNotification();
    }
    else if (type == "APM") {
        GetAPMNotification();
    }
}
function ResetNotificationTab() {
    $('#topbar_inventory_notifications').html('');
    $('#topbar_maintenence_notifications').html('');
    $('#topbar_procurement_notifications').html('');
    $('#topbar_system_notifications').html('');
}

function RemoveNotificationUnread() {
    if (globalNotificationTab == 'Maintenance') {
        if ($('#notificationMaintenance-dot').hasClass('m-badge--dot')) {
            $('#notificationMaintenance-dot').removeClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
        }
    }
    else if (globalNotificationTab == 'Inventory') {
        if ($('#notificationInventory-dot').hasClass('m-badge--dot')) {
            $('#notificationInventory-dot').removeClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
        }
    }
    else if (globalNotificationTab == 'Procurement') {
        if ($('#notificationProcurement-dot').hasClass('m-badge--dot')) {
            $('#notificationProcurement-dot').removeClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
        }
    }
    else if (globalNotificationTab == 'System') {
        if ($('#notificationSystem-dot').hasClass('m-badge--dot')) {
            $('#notificationSystem-dot').removeClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
        }
    }

}

function ResetTopBarNotification() {
    var newNotification = $('#NewNotificationCount').val();
    if (parseInt(newNotification) == 0) {
        $('#m_topbar_notification_icon .m-nav__link-icon').removeClass('m-animate-shake');
        $('#m_topbar_notification_icon .m-nav__link-badge').removeClass('m-animate-blink');
        $('#notification-dot').removeClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
    }
    $(document).find('#notificationloader').hide();
}
function cuteHide(el) {
    el.animate({ opacity: '0' }, 150, function () {
        el.animate({ height: '0px' }, 150, function () {
            el.remove();
        });
    });
}
$(document).on('click', '.closeIcon', function () {
    var alertUser = $(this).attr('data-alertuser');
    DeleteNotification(alertUser);
    var el = $(this).closest('.m-list-timeline__item');
    cuteHide(el);
});

function DeleteNotification(AlertUserId) {
    $.ajax({
        url: '/Base/DeleteNotification',
        data: {
            AlertUserId: AlertUserId,
        },
        type: "POST",
        datatype: "json",
        success: function (data) { }
    });
}
//#endregion

//#region System Unavailable Message
function GetMaintenanceMessage() {
    $.ajax({
        url: "/Base/GetSiteMaintenanceMessage",
        type: "GET",
        datatype: "json",
        success: function (data) {
            if (data != '') {
                $.notify({
                    // options
                    icon: 'glyphicon glyphicon-warning-sign',
                    //title: 'System Under Maintenance',
                    title: '',
                    message: data,
                    url: '#'
                   /*url: '/SiteMaintenance/UnderConstruction'*/,
                    target: '_blank'
                }, {
                    // settings
                    element: 'body',
                    position: null,
                    //type: "info",
                    type: "danger",
                    allow_dismiss: true,
                    newest_on_top: false,
                    showProgressbar: false,
                    placement: {
                        from: "top",
                        align: "right"
                    },
                    offset: 20,
                    spacing: 10,
                    z_index: 1031,
                    delay: 5000,
                    timer: 5000,
                    url_target: '_blank',
                    mouse_over: null,
                    animate: {
                        enter: 'animated fadeInDown',
                        exit: 'animated fadeOutUp'
                    },
                    onShow: null,
                    onShown: null,
                    onClose: null,
                    onClosed: null,
                    icon_type: 'class',
                    template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                        '<button type="button" aria-hidden="true" class="close" data-notify="dismiss"></button>' +
                        '<span data-notify="icon"></span> ' +
                        '<span data-notify="title">{1}</span> ' +
                        '<span data-notify="message"><i class="fa fa-exclamation-circle" style="font-size:13px"></i>&nbsp; {2}</span>' +
                        '<div class="progress" data-notify="progressbar">' +
                        '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                        '</div>' +
                        '<a href="{3}" target="{4}" data-notify="url"></a>' +
                        '</div>'

                });
            }

        },
        complete: function () {

        }
    });

}
function GetPasswordExpirationMessage() {
    $.ajax({
        url: "/Base/GetPasswordExpirationMessage",
        type: "GET",
        datatype: "json",
        success: function (data) {
            if (data != '') {
                $.notify({
                    // options
                    icon: 'glyphicon glyphicon-warning-sign',
                    //title: 'System Under Maintenance',
                    title: '',
                    message: data,
                    url: '#'
                   /*url: '/SiteMaintenance/UnderConstruction'*/,
                    target: '_blank'
                }, {
                    // settings
                    element: 'body',
                    position: null,
                    //type: "info",
                    type: "danger",
                    allow_dismiss: true,
                    newest_on_top: false,
                    showProgressbar: false,
                    placement: {
                        from: "top",
                        align: "right"
                    },
                    offset: 20,
                    spacing: 10,
                    z_index: 1031,
                    delay: 5000,
                    timer: 5000,
                    url_target: '_blank',
                    mouse_over: null,
                    animate: {
                        enter: 'animated fadeInDown',
                        exit: 'animated fadeOutUp'
                    },
                    onShow: null,
                    onShown: null,
                    onClose: null,
                    onClosed: null,
                    icon_type: 'class',
                    template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                        '<button type="button" aria-hidden="true" class="close" data-notify="dismiss"></button>' +
                        '<span data-notify="icon"></span> ' +
                        '<span data-notify="title">{1}</span> ' +
                        '<span data-notify="message"><i class="fa fa-exclamation-circle" style="font-size:13px"></i>&nbsp; {2}</span>' +
                        '<div class="progress" data-notify="progressbar">' +
                        '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                        '</div>' +
                        '<a href="{3}" target="{4}" data-notify="url"></a>' +
                        '</div>'

                });
            }

        },
        complete: function () {

        }
    });

}

function GetClientMessage() {
    $.ajax({
        url: "/Base/GetClientMessage",
        type: "GET",
        datatype: "json",
        success: function (data) {
            // Combine messages from data array
            var combinedMessage = data.join(',<br>');
            var selectedclientmsg = "";
            var allclientmsg = "";
            var msghtml = "";
            if (data.length > 1) {
                selectedclientmsg = data[0].Message;
                allclientmsg = data[1].Message;
                msghtml = '<span data-notify="icon"></span> ' +
                    '<span data-notify="title" style="color:#000">Selected Client Message</span> ' +
                    '<span data-notify="message" style="color:#000"><i class="fa fa-exclamation-circle" style="font-size:13px;color:#000"></i>&nbsp; ' + selectedclientmsg + '</span>' +
                    '<span data-notify="icon"></span> ' +
                    '<span data-notify="title" style="color:#000">All Client Message</span> ' +
                    '<span data-notify="message" style="color:#000"><i class="fa fa-exclamation-circle" style="font-size:13px;color:#000"></i>&nbsp; ' + allclientmsg + '</span>';
            }
            else if (data.length == 1) {
                if (data[0].ClientId > 0) {
                    selectedclientmsg = data[0].Message;
                    msghtml = '<span data-notify="icon"></span> ' +
                        '<span data-notify="title" style="color:#000">Selected Client Message</span> ' +
                        '<span data-notify="message" style="color:#000"><i class="fa fa-exclamation-circle" style="font-size:13px;color:#000"></i>&nbsp; ' + selectedclientmsg + '</span>';
                }
                else {
                    allclientmsg = data[0].Message;
                    msghtml = '<span data-notify="icon"></span> ' +
                        '<span data-notify="title" style="color:#000">All Client Message</span> ' +
                        '<span data-notify="message" style="color:#000"><i class="fa fa-exclamation-circle" style="font-size:13px;color:#000"></i>&nbsp; ' + allclientmsg + '</span>';
                }
            }


            // Show notification if there are messages
            if (combinedMessage !== '') {
                $.notify({
                    icon: 'glyphicon glyphicon-warning-sign',
                    title: 'Client Message Notification',
                    message: combinedMessage,
                    url: '#',
                    target: '_blank'
                }, {
                    element: 'body',
                    position: null,
                    type: "warning",
                    allow_dismiss: true,
                    newest_on_top: false,
                    showProgressbar: false,
                    placement: {
                        from: "top",
                        align: "right"
                    },
                    offset: 20,
                    spacing: 10,
                    z_index: 1031,
                    delay: 5000,
                    timer: 5000,
                    url_target: '_blank',
                    mouse_over: null,
                    animate: {
                        enter: 'animated fadeInDown',
                        exit: 'animated fadeOutUp'
                    },
                    onShow: null,
                    onShown: null,
                    onClose: null,
                    onClosed: null,
                    icon_type: 'class',
                    template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                        '<button type="button" aria-hidden="true" class="close" data-notify="dismiss"></button>' +
                        msghtml +
                        '<div class="progress" data-notify="progressbar">' +
                        '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                        '</div>' +
                        '<a href="{3}" target="{4}" data-notify="url"></a>' +
                        '</div>'
                });
            }
        },
        complete: function () {
        }
    });
}

$(function () {
    GetMaintenanceMessage();
    GetPasswordExpirationMessage();
    GetClientMessage();
});
//#endregion
//#region V2-1136 Work order Link Details
$(document).on('click', '.lnk_WorkoderdetailsFromNotification', function (e) {
    var workOrderId = $(this).data('woid');
    var AlertName = $(this).data('alertname');
    ShowLoader();
    window.location.href = '../WorkOrder/DetailFromNotification?workOrderId=' + workOrderId + '&alertName=' + AlertName;
});
//#endregion
//#region V2-1147 Link Details
$(document).on('click', '.lnk_EquipmentdetailsFromNotification', function (e) {
    var equipmentid = $(this).data('equipmentid');
    var AlertName = $(this).data('alertname');
    ShowLoader();
    window.location.href = '../Equipment/DetailFromNotification?EquipmentId=' + equipmentid + '&alertName=' + AlertName;
});
$(document).on('click', '.lnk_PartdetailsFromNotification', function (e) {
    var partid = $(this).data('partid');
    var AlertName = $(this).data('alertname');
    ShowLoader();
    window.location.href = '../Parts/DetailFromNotification?partId=' + partid + '&alertName=' + AlertName;
});
$(document).on('click', '.lnk_PurchaseOrderdetailsFromNotification', function (e) {
    var polineitemid = $(this).data('polineitemid');
    var AlertName = $(this).data('alertname');
    ShowLoader();
    window.location.href = '../Purchasing/DetailFromNotification?PurchaseOrderLineItemId=' + polineitemid + '&alertName=' + AlertName;
});
$(document).on('click', '.lnk_PurchaseRequestdetailsFromNotification', function (e) {
    var prid = $(this).data('prid');
    var AlertName = $(this).data('alertname');
    ShowLoader();
    window.location.href = '../PurchaseRequest/DetailFromNotification?PurchaseRequestId=' + prid + '&alertName=' + AlertName;
});
//#endregion