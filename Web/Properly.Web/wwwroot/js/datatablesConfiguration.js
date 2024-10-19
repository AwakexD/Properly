$(document).ready(function () {
    $('#usersTable').DataTable({
        "paging": true,
        "searching": true,
        "info": true,
        "responsive": true
    });

    $('#propertyTypesTable').DataTable({
        "paging": true,
        searching: true,
        info: true,
        responsive: true,
    });
});