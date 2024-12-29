$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": 'Bookings/GetAllBookings',
            "type": "GET",
            "datatype": "json",
            "dataSrc": function (json) {
                
                json.data.forEach(function (item) {
                    
                    if (item.bookingStatus === 0) {
                        item.bookingStatus = 'Active';
                    } else if (item.bookingStatus === 1) {
                        item.bookingStatus = 'Cancelled';
                    }
                });

                return json.data;
            }
        },
        "columns": [
            { "data": 'bookingId', "width": "5%" },
            { "data": 'bookingDate', "width": "15%" },
            { "data": 'bookingTime', "width": "5%" },
            { "data": 'tableNumber', "width": "5%" },
            { "data": 'numberOfGuests', "width": "5%" },
            { "data": 'applicationUserId', "width": "20%" },
            { "data": 'name', "width": "10%" },
            { "data": 'phone', "width": "10%" },
            { "data": 'email', "width": "15%" },
            {
                "data": 'bookingStatus',  
                "render": function (data, type, row) {
                    return `<span class="small-text">${data}</span>`;  
                },
                "width": "15%"
            },
            {
                "data": 'bookingId',  
                "render": function (data, type, row) {
                  
                    if (row.bookingStatus === 'Cancelled') {
                        return `
                            <div class="w-75 btn-group" role="group">
                                <button class="btn btn-danger mx-2" disabled>
                                    <i class="bi bi-trash-fill"></i> <span class="small-text">Cancelled</span>
                                </button>
                            </div>
                        `;
                    } else {
                        return `
                            <div class="w-75 btn-group" role="group">
                                <a href="Bookings/ConfirmCancelBooking?bookingId=${row.bookingId}" class="btn btn-success mx-2">
                                    <i class="bi bi-trash-fill"></i> <span class="small-text">Cancel</span>
                                </a>
                            </div>
                        `;
                    }
                },
                "width": "15%"
            }
        ]
    });
}
