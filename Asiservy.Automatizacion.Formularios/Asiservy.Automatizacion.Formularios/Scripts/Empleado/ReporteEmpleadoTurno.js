


//$(document).ready(function () {
//    CargarEmpleadoTurno("0");
//    $('SelectTurno').val("0");
//    //  Nuevo();
//});
function Consultar() {
  //  console.log($("#selectLinea").val());
    if ($("#selectLinea").val()=='' ) {
        $("#validacionLineas").prop("hidden", false);
        return;
    } else {
        $("#validacionLineas").prop("hidden", true);
    }


    CargarEmpleadoTurno();
}

function CargarEmpleadoTurno() {
    var bitacora = $('#TableEmpleadoTurno');
    bitacora.html('');
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../Empleado/ReporteEmpleadoTurnoPartial",
        type: "GET",
        data: {
            dsTurno: $("#SelectTurno").val(),
            dsLinea: $("#selectLinea").val()
        
        },
        success: function (resultado) {
            var bitacora = $('#TableEmpleadoTurno');
            if (resultado=="0")
                bitacora.html('<div class="text-center"><h4>No Existen Registros</h4></div>');
            else
                bitacora.html(resultado);

            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);

            MensajeError(resultado, false);
        }
    });
}
