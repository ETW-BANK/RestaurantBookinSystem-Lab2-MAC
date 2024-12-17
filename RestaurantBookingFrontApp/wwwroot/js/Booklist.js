$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": 'Bookings/GetAllBookings',
            "type": "GET",
            "datatype": "json",
            "dataSrc": "data",
        },
        "columns": [
            { "data": 'bookingId', "width": "20%" },
            { "data": 'bookingDate', "width": "20%" },
            { "data": 'bookingTime', "width": "20%" },
            { "data": 'tableNumber', "width": "20%" },
            { "data": 'numberOfGuests', "width": "20%" },
            { "data": 'applicationUserId', "width": "20%" },
            { "data": 'name', "width": "20%" },
            { "data": 'phone', "width": "20%" },
            { "data": 'email', "width": "20%" },

            {
                "data": 'id',
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