
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#tblData').DataTable({
        "ajax": {
            "url": 'Users/GetAllUser',
            "type": "GET",
            "datatype": "json",
            "dataSrc": "data" 
        },
        "columns": [
            { "data": "id", "width": "20%" },
            { "data": "name", "width": "20%" },
            { "data": "streetAddress", "width": "20%" },
            { "data": "city", "width": "20%" },
            { "data": "postalCode", "width": "20%" },
            { "data": "phoneNumber", "width": "20%" },
            { "data": "role", "width": "20%" },
            {
                "data": "id",
                "render": function (data, type, row) {
                    return '<div class="w-75 btn-group" role="group">' +
                        '<a href="Tables/Edit?id=' + row.id + '" class="btn btn-primary mx-2">' +
                        '<i class="bi bi-pencil-square"></i> Edit' +
                        '</a>' +
                        '<a href="Tables/Delete?id=' + row.id + '" class="btn btn-danger mx-2">' +
                        '<i class="bi bi-trash-fill"></i> Delete' +
                        '</a>' +
                        '</div>';
                },
                "width": "15%"
            }
        ]
    });
}
