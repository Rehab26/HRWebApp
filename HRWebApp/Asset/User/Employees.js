$(document).ready(function () {
    $('#EmployeesTable').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": "http://localhost:44323/api/user/getall?id=123"
    });
});