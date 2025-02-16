$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": 'Categories/GetAllCategories',
            "type": "GET",
            "dataSrc": "data",
        },
        "columns": [
            { "data": 'id', "width": "20%" },
            { "data": 'name', "width": "20%" },
            { "data": 'description', "width": "20%" },
            {
                "data": "imageUrl",
                "render": function (data) {
                    return `<img src="${data}" class="img-fluid rounded" style="width: 70px; height: 70px;" alt="Category Image"/>`;
                },
                "width": "15%"
            },
            {
                "data": 'id',
                "render": function (data, type, row) {
                    return `
        <div class="w-75 btn-group" role="group">
            <a href="Categories/Edit?id=${row.id}" class="btn btn-primary mx-2">
                <i class="bi bi-pencil-square"></i> Edit
            </a>
            <a href="Categories/Delete?id=${row.id}" class="btn btn-danger mx-2">
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