﻿$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": 'Tables/GetAllTables',  // Ensure this URL matches your controller's route
            "type": "GET",
            "datatype": "json",
            "dataSrc": ""  // Since the response is an array directly, no need for 'dataSrc': "data"
        },
        "columns": [
            { "data": 'tableNumber', "width": "20%" },
            { "data": 'numberOfSeats', "width": "20%" },
            { "data": 'isAvailable', "width": "20%" }
            {
                "data": 'id',
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="Tables/Edit?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a href="Tables/Delete?id=${data}" class="btn btn-danger mx-2">
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>
                        </div>
                    `;
                },
                "width": "15%"
            }
        ]
    });
}
