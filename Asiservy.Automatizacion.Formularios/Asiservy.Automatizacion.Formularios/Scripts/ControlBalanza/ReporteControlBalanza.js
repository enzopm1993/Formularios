


function CargarControlBalanza() {
    $("#spinnerCargando").prop("hidden", false);
    $("#DivTableControl").html('');
    $.ajax({
        url: "../ControlBalanza/ReporteControlBalanzaPartial",
        type: "GET",
        data: {
            FechaDesde: $('#txtFechaDesde').val(),
            FechaHasta: $('#txtFechaHasta').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#DivTableControl").html('');
                $("#DivTableControl").html("No existen registros");
            } else {
                $("#DivTableControl").html('');
                $("#DivTableControl").html(resultado);
                config.opcionesDT.pageLength = 5;
                config.opcionesDT.order = [[1, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);


            }
            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#DivTableControl").html('');
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}