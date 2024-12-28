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
            { "data": 'bookingId', "width": "5%" },
            { "data": 'bookingDate', "width": "10%" },
            { "data": 'bookingTime', "width": "5%" },
            { "data": 'tableNumber', "width": "5%" },
            { "data": 'numberOfGuests', "width": "5%" },
            { "data": 'applicationUserId', "width": "20%" },
            { "data": 'name', "width": "10%" },
            { "data": 'phone', "width": "10%" },
            { "data": 'email', "width": "15%" },
          

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