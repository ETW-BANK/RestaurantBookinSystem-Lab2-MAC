$(document).ready(function () {
    loadDataTable();
});

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": 'Categories/GetCategories',
            "type": "GET",
            "datatype": "json",
            "dataSrc": "data" // Corrected to match array response
        },
        "columns": [
            { "data": 'id', "width": "10%" },
            { "data": 'name', "width": "20%" },
            { "data": 'description', "width": "20%" },
            {
                "data": 'imageUrl',
                "render": function (data, type, row) {
                    // Ensure proper image URL handling
                    let imageUrl = data && data.startsWith("http") ? data : (data ? "/" + data.replace(/^\/+/, '') : "/images/category/default.jpg");
                    return `<img src="${imageUrl}" alt="${row.name}" style="width:100px; height:auto; border-radius:5px; border:solid double;" />`;
                },
                "width": "20%"
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
