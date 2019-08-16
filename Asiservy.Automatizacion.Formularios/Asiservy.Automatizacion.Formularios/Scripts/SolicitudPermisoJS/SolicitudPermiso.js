
    $("body").on("click", ".Grid tfoot a", function () {
        $('#WebGridForm').attr('action', $(this).attr('href')).submit();
    return false;
});

$('table tbody tr  td').on('click', function () {
    $("#myModal").modal("show");
    $("#txtSolicitud").val($(this).closest('tr').children()[0].textContent);
    $("#txtFecha").val($(this).closest('tr').children()[1].textContent);
    $("#txtMotivo").val($(this).closest('tr').children()[2].textContent);
    $("#txtArea").val($(this).closest('tr').children()[3].textContent);
    $("#txtEmpleado").val($(this).closest('tr').children()[4].textContent);

    $("#txtSolicitud").prop('disabled', true);
    $("#txtFecha").prop('disabled', true);
    $("#txtMotivo").prop('disabled', true);
    $("#txtArea").prop('disabled', true);
    $("#txtEmpleado").prop('disabled', true);
});