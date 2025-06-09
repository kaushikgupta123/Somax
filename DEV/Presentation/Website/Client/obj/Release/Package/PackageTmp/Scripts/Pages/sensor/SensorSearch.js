$(document).ready(function () {
    var dtTable = $("#example").DataTable({
        rowGrouping: true,
        colReorder: true,
        dataSrc: 'LastReading',     
        "bProcessing": true, 
        dom: 'Bfrtip',
        buttons: [
             'excel', 'pdf', 'print'
        ],       
        "orderMulti": true, 

        "ajax": {
            "url": "/SensorSearch/GetSensors",
            "type": "GET",
            "datatype": "json"
        },
        "columns":
            [
                 {
                     "data": "SensorId",
                     "autoWidth": true,
                     "bSearchable": true,
                     "bSortable": true,
                     "mRender": function (data, type, row) {                        
                         return '<a class=lnk_sensor href="">' + data + '</a>'
                     }
                 },
                 { "data": "SensorName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                 { "data": "EquipmentClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                 { "data": "LastReading", "autoWidth": true, "bSearchable": true, "bSortable": true },
                 { "data": "EquipmentName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                 {
                     "data": "Equipment_Sensor_XrefId",
                     "autoWidth": true,
                     "bSearchable": true,
                     "bSortable": true,
                     "visible": false,
                 },
            ]

    });
    var sensorData = {
        "id": "",
        "name": "",
        "eq_ID": "",
        "lastRead": "",
        "eq_name": "",
        "eq_Xref": ""
    };
    $(document).on('click', '.lnk_sensor', function (e) {
        e.preventDefault();
        var index_row = $('#example tr').index($(this).closest('tr')) - 1;
        var td = $(this).parents('tr').find('td');
        sensorData.id = $(this).html();       
        sensorData.name = $(td[1]).html();       
        sensorData.eq_ID = $(td[2]).html();       
        sensorData.lastRead = $(td[3]).html();       
        sensorData.eq_name = $(td[4]).html();      
        sensorData.eq_Xref = dtTable.cells({ row: index_row, column: 5 }).data()[0];     
        $.ajax({
            url: "/SensorSearch/GetSelectedRow",
            type: "POST",
            dataType: "json",            
            data: { _sensorId: sensorData.id, _sensorName: sensorData.name, _equipmentClientLookupId: sensorData.eq_ID, _lastReading: sensorData.lastRead, _equipmentName: sensorData.eq_name, _equipment_Sensor_XrefId: sensorData.eq_Xref },
            success: function (data) {                
                window.location.href = '../SensorEdit/Index';
            }
        });
    });
    $(document).on('change', '#drpGroup', function () {
        var selectedIndex = $(this).prop('selectedIndex');
        var selectedText = $(this).find('option:selected').text();
        renderDataTable(selectedIndex, selectedText);
    });
});
function renderDataTable(selectedIndex, selectedText) {
    $("#example").DataTable().destroy();
    if (selectedIndex == 0) {
        $("#example").DataTable({
            rowGrouping: true,
            colReorder: true,
            dataSrc: 'LastReading',          
            "bProcessing": true, 
            dom: 'Bfrtip',
            buttons: [
                 'excel', 'pdf', 'print'
            ],          
            "orderMulti": true, 

            "ajax": {
                "url": "/SensorSearch/GetSensors",
                "type": "GET",
                "datatype": "json"
            },
            "columns":
                [
                     {
                         "data": "SensorId",
                         "autoWidth": true,
                         "bSearchable": true,
                         "bSortable": true,
                         "mRender": function (data, type, row) {                           
                             return '<a class=lnk_sensor href="">' + data + '</a>'
                         }
                     },
                     { "data": "SensorName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                     { "data": "EquipmentClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                     { "data": "LastReading", "autoWidth": true, "bSearchable": true, "bSortable": true },
                     { "data": "EquipmentName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                     {
                         "data": "Equipment_Sensor_XrefId",
                         "autoWidth": true,
                         "bSearchable": true,
                         "bSortable": true,
                         "visible": false,

                     },
                ]
        });
    }
    else {
        selectedIndex = selectedIndex - 1;
        $("#example").DataTable({
            rowGrouping: true,
            colReorder: true,
            dataSrc: 'LastReading',           
            "bProcessing": true, 
            dom: 'Bfrtip',
            buttons: [
                 'excel', 'pdf', 'print'
            ],          
            "orderMulti": true,
            "ajax": {
                "url": "/SensorSearch/GetSensors",
                "type": "GET",
                "datatype": "json"
            },
            "columnDefs": [
         { "visible": false, "targets": selectedIndex }
            ],
            "order": [[selectedIndex, 'asc']],
            "displayLength": 25,
            "drawCallback": function (settings) {
                var api = this.api();
                var rows = api.rows({ page: 'current' }).nodes();
                var last = null;
                
                api.column(selectedIndex, { page: 'current' }).data().each(function (group, i) {
                    var toShow = selectedText + " : " + group;
                    if (last !== group) {
                        $(rows).eq(i).before(
                            '<tr class="group"><td colspan="5">' + toShow + '</td></tr>'
                        );
                        last = group;
                    }
                });
            },
            "columns":
                [
                     {
                         "data": "SensorId",
                         "autoWidth": true,
                         "bSearchable": true,
                         "bSortable": true,
                         "mRender": function (data, type, row) {                          
                             return '<a class=lnk_sensor href="">' + data + '</a>'
                         }
                     },
                     { "data": "SensorName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                     { "data": "EquipmentClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                     { "data": "LastReading", "autoWidth": true, "bSearchable": true, "bSortable": true },
                     { "data": "EquipmentName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                     {
                         "data": "Equipment_Sensor_XrefId",
                         "autoWidth": true,
                         "bSearchable": true,
                         "bSortable": true,
                         "visible": false,

                     },
                ]
        });
    }
}
