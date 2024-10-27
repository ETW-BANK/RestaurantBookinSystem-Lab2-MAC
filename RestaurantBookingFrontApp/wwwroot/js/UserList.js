var dataTable;

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
            { "data": 'name', "width": "10%" },
            { "data": 'email', "width": "15%" },
            { "data": 'streetAddress', "width": "15%" },
            { "data": 'city', "width": "10%" },
            { "data": 'state', "width": "10%" },
            { "data": 'postalCode', "width": "7%" },
            { "data": 'phoneNumber', "width": "15%" },
            { "data": 'role', "width": "8%" },
            {
                data: { id: "id" },
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="Users/RoleManagement?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:150px">
                                <i class="bi bi-pencil-square"></i> Permission
                            </a>
                        </div>
                    `;
                },
                "width": "25%"
            }
        ]
    });
}
