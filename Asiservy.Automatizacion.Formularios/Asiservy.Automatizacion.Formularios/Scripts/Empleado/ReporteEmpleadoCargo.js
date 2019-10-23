

function CargarEmpleadoCargo() {
    console.log($("#selectLinea").val());
    if ($("#selectLinea").val() < 1) {
        MensajeAdvertencia("Seleccione una Linea");
        return;
    }
    $("#divTableEmpleadoCargo").html('');
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../Empleado/ReporteEmpleadoCargoPartial",
        type: "GET",
        data: {
            Linea: $("#selectLinea").val()
        },
        success: function (resultado) {
            var div = $("#divTableEmpleadoCargo");
            div.html("");
            $("#spinnerCargando").prop("hidden", true);

            if (resultado == "0") {
                MensajeAdvertencia("No se encontraron registros");
                return;
            }
            div.html(resultado);

        },
        error: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            MensajeError(resultado.responseText, false);
        }
    });
}