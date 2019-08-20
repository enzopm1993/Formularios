function ConsultarEmpleados() {
    $.ajax({
        type: "GET",
        url: "/Asistencia/ConsultarEmpleado",
        success: function (data) {
            $('#DivEmpleados').html(data);
        }
    });
}
$('#btnGuardarCambioEmp').hide();
$('#ConsultarEmpleados').on("click", function () {
    $('#btnGuardarCambioEmp').show();
});
$('#trlinea').hide();
$('#trlineao').hide();
$('#comboareao').change(function () {
    if ($(this).val() == 'procesos') {
        $('#trlineao').show();
    } else {
        $('#trlineao').hide();
    }
});
$('#comboarea').change(function () {
    if ($(this).val() == 'procesos') {
        $('#trlinea').show();
    } else {
        $('#trlinea').hide();
    }
});