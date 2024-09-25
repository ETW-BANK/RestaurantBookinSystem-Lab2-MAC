

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": 'Customers/GetAllCustomer',
            "type": "GET",
            "datatype": "json",
            "dataSrc": "data",

        },
        "columns": [
            { "data": 'firstName', "width": "20%" },
            { "data": 'lasttName', "width": "20%" },
            { "data": 'email', "width": "20%" },
            { "data": 'phone', "width": "15%" },
            {
                "data": 'id',
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="Customers/Edit?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a href="Customers/Delete?id=${data}" class="btn btn-danger mx-2">
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
