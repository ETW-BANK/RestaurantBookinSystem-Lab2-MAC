var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": 'Users/GetAllUser',  // Ensure this matches your Admin controller route
            "type": "GET",
            "datatype": "json",
            "dataSrc": "data"  // Map data from the 'data' key in the response
        },
        "columns": [
            { "data": 'name', "width": "15%" },
            { "data": 'email', "width": "20%" },
            { "data": 'streetAddress', "width": "15%" },
            { "data": 'city', "width": "10%" },
           
            { "data": 'postalCode', "width": "10%" },
            { "data": 'phoneNumber', "width": "10%" },
            { "data": 'role', "width": "10%" }
            {
                "data": 'id',
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="Users/RoleManagement?userId=${data}" class="btn btn-danger text-white" style="width:100px">
                                Permission
                            </a>
                        </div>`;
                },
                "width": "15%"
            }
        ]
    });
}
