$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": 'Menus/GetAllMenu',
            "type": "GET",
            "datatype": "json",
            "dataSrc": "data",

        },
        "columns": [
            { "data": 'title', "width": "20%" },
            { "data": 'price', "width": "20%" },
            { "data": 'isAvailable', "width": "20%" },
            { "data": 'imageUrl', "width": "15%" },
            {
                "data": 'id',
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="Menus/Edit?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a href="Menus/Delete?id=${data}" class="btn btn-danger mx-2">
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
