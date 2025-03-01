$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": 'Categories/GetCategories',
            "type": "GET",
            "datatype": "json",
            "dataSrc": "data",
        },
        "columns": [
            { "data": 'category.id', "width": "20%" },
            { "data": 'category.name', "width": "20%" },
            { "data": 'category.description', "width": "20%" },
            {
                "data": 'category.id',
                "render": function (data, type, row) {
                    return `
        <div class="w-75 btn-group" role="group">
            <a href="Tables/Edit?id=${row.id}" class="btn btn-primary mx-2">
                <i class="bi bi-pencil-square"></i> Edit
            </a>
            <a href="Tables/Delete?id=${row.id}" class="btn btn-danger mx-2">
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