$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#tblData').DataTable({
        "ajax": {
            "url": 'Bookings/GetAllBookings',
            "type": "GET",
            "datatype": "json",
            "dataSrc": function (json) {
                console.log("Response Data:", json); // Log for debugging
                return json.data || json; // Adjust if data is wrapped in a "data" property
            }
        },
        "columns": [
            { "data": "bookingId", "width": "10%" },
            { "data": "bookingDate", "width": "10%" },
            { "data": "bookingTime", "width": "10%" },
            { "data": "numberOfGuests", "width": "10%" },
            { "data": "table.tableNumber", "width": "5%" },
            { "data": "userid", "width": "10%" },
             { "data": "userName", "width": "15%" },
            { "data": "email", "width": "10%" },
           
            {
                "data": "bookingId",
                "render": function (data, type, row) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="Bookings/Edit?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a href="Bookings/Delete?id=${data}" class="btn btn-danger mx-2">
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>
                        </div>
                    `;
                },
                "width": "10%"
            }
        ]
    });
}
