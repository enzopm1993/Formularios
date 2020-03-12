


function ConsultarReporte() {
    $("#chartCabecera2").html('');
    if ($("#txtFecha").val() == '' || $("#selectArea").val() == '' ) {
        return;
    }
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../ResidualCloro/ReporteResidualCloroPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val(),
            Area: $("#selectArea").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#divCabecera2").prop("hidden", false);
            if (resultado == "0") {
                $("#chartCabecera2").html("No existen registros");
                $("#spinnerCargando").prop("hidden", true);
            } else {
                $("#spinnerCargando").prop("hidden", true);
                $("#chartCabecera2").html(resultado);
                config.opcionesDT.pageLength = -1;
                config.opcionesDT.order = false;
                config.opcionesDT.ordering = false;
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

