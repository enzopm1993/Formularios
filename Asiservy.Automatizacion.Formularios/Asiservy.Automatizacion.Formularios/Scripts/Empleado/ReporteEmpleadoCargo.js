

function CargarEmpleadoCargo() {
   // console.log($("#selectLinea").val());
    $("#divCard").prop("hidden", true);

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
            config.opcionesDT.pageLength = 15;
            //   config.opcionesDT.order = [[1, "asc"]];
            $('#tblDataTable').DataTable(config.opcionesDT);
            $("#divCard").prop("hidden", false);

        },
        error: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            MensajeError(resultado.responseText, false);
        }
    });
}