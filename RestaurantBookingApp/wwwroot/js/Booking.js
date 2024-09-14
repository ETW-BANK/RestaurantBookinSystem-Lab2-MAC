﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#tblData').DataTable({
        "ajax": {
            "url": '/Bookings/GetAllBookings',
            "type": "GET",
            "datatype": "json",
            "dataSrc": "data" // This should match the key in your JSON response
        },
        "columns": [
            { "data": "customer.firstName", "width": "10%" },
            { "data": "customer.lasttName", "width": "10%" },
            { "data": "customer.email", "width": "10%" },
            { "data": "customer.phone", "width": "10%" },
            { "data": "bookingDate", "width": "15%", "render": function (data) { return new Date(data).toLocaleDateString(); } },
            { "data": "tables.tableNumber", "width": "10%" },
            { "data": "tables.numberOfSeats", "width": "10%" },
            { "data": "numberOfGuests", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                           <a href="/Bookings/Create?id=${data}" class="btn btn-primary mx-2">
                 <i class="bi bi-pencil-square"></i> Edit
                        </a>
                            <a href="/Bookings/Delete?id=${data}" class="btn btn-danger mx-2">
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>
                        </div>
                    `;
                },
                "width": "20%"
            }
        ]
    });
}
