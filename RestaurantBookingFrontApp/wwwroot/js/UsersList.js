$(document).ready(function () {
    $('#tblData').DataTable({
        "ajax": {
            "url": 'Users/GetAllUser', // API endpoint
            "type": "GET",
            "datatype": "json",
            "dataSrc": "data" // Map to the "data" property in the response
        },
        "columns": [
            { "data": "id", "width": "10%" }, // Maps to "id"
            { "data": "name", "width": "15%" }, // Maps to "name"
            { "data": "email", "width": "20%" }, // Maps to "email"
            { "data": "streetAddress", "width": "20%" }, // Maps to "streetAddress"
            { "data": "city", "width": "10%" }, // Maps to "city"
            { "data": "postalCode", "width": "10%" }, // Maps to "postalCode"
            { "data": "phoneNumber", "width": "15%" }, // Maps to "phoneNumber"
            { "data": "role", "width": "10%" } // Maps to "role"
        ],
        "processing": true, // Display processing indicator
        "paging": true, // Enable pagination
        "responsive": true, // Make table responsive
        "language": {
            "emptyTable": "No data available in table", // Message for empty table
            "processing": "Processing..." // Message while loading data
        }
    });
});
