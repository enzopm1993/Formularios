


$(document).ready(function () {
    CargarEmpleadoTurno("0");
    $('SelectTurno').val("0");
    //  Nuevo();
});


function CargarEmpleadoTurno(turno) {
    $.ajax({
        url: "../Empleado/ReporteEmpleadoTurnoPartial",
        type: "GET",
        data: { dsTurno: turno },
        success: function (resultado) {
            var bitacora = $('#TableEmpleadoTurno');
            bitacora.html(resultado);
        },
        error: function (resultado) {

            MensajeError(resultado, false);
        }
    });
}
