$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": 'Users/GetAllUsers', 
            "type": "GET",
            "datatype": "json",
            "dataSrc": "data",
        },
        "columns": [
            { "data": 'name', "width": "15%" },
            { "data": 'email', "width": "20%" },
            { "data": 'streetAddress', "width": "20%" },
            { "data": 'city', "width": "15%" },
            { "data": 'state', "width": "10%" },
            { "data": 'postalCode', "width": "7%" },
            { "data": 'phoneNumber', "width": "15%" },
            { "data": 'role', "width": "8%" },
            {
                "data": 'id',
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="Users/Create?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a href="Users/Delete?id=${data}" class="btn btn-danger mx-2">
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
