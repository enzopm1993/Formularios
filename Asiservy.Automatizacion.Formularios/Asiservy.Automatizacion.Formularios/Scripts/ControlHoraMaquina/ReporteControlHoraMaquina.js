$(document).ready(function () {
    CargarReporte();

});
function CargarReporte() {

    if ($('#txtFecha').val() == "") {        
        return;
    }

    $("#spinnerCargando").prop("hidden", false);
    $('#DivTableReporte').html('');
    $.ajax({
        url: "../ControlHoraMaquina/ReporteControlHoraMaquinaPartial",
        type: "GET",
        data: { Fecha: $('#txtFecha').val() },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $('#DivTableReporte').html(resultado);
            $("#spinnerCargando").prop("hidden", true);
            config.opcionesDT.pageLength = 50;
            $('#tblDataTable').DataTable(config.opcionesDT);

        },
        error: function (resultado) {
            MensajeError(resultado.responseText);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}