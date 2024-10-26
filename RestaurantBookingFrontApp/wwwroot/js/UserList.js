
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
            { "data": 'name', "width": "10%"  },
            { "data": 'email', "width": "15%" },
            { "data": 'streetAddress', "width": "15%" },
            { "data": 'city', "width": "10%" },
            { "data": 'state', "width": "10%" },
            { "data": 'postalCode', "width": "7%" },
            { "data": 'phoneNumber', "width": "15%" },
            { "data": 'role', "width": "8%" },
            {
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        return `
                            <div class="text-center">
                              <a onClick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px">
                                    <i class="bi bi-lock-fill"></i> Lock
                                </a>
                               
                                <a href="Users/UpdateRole?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:150px">
                                    <i class="bi bi-pencil-square"></i> Permission
                                </a>
                            </div>
                        `;
                    } else {
                        return `
                            <div class="text-center">
                               <a onClick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:100px">
                                    <i class="bi bi-unlock-fill"></i> Unlock
                                </a>
                                <a href="Users/UpdateRole?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:150px">
                                    <i class="bi bi-pencil-square"></i> Permission
                                </a>
                            </div>
                        `;
                    }
                },
                "width": "25%"
            }
        ]
    });
}

function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Users/LockUnlock', // Ensure the URL matches your controller route
        data: JSON.stringify(id), // Wrap in an object for proper deserialization
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            } else {
                toastr.error(data.message || "An error occurred.");
            }
        },
        error: function () {
            toastr.error("An error occurred while processing the request.");
        }
    });
}