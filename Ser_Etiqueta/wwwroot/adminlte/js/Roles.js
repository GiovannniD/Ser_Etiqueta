
$(function () {
    $(document).ready(function () {
        $('#Roles').DataTable({
            "destroy": true, "scrollX": "200%",
            "scrollY": "350px",
            "lengthMenu": [[10, 20, 50, 100, 1000, -1], [10, 20, 50, 100, 1000, "All"]],
            "responsive": false, "lengthChange": false, "autoWidth": false, "searching": true
        });
    });
});

