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
        data: {
            Fecha: $('#txtFecha').val(),
            Turno: $("#txtTurno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#spinnerCargando").prop("hidden", true);
            if (resultado == '0') {
                MensajeAdvertencia("No existen registros");
                return;            }

            $('#DivTableReporte').html(resultado);
            
            config.opcionesDT.pageLength = 50;
            config.opcionesDT.order = false;
            config.opcionesDT.ordering = false;
            config.opcionesDT.paging = false;
            $('#tblDataTable').DataTable(config.opcionesDT);

        },
        error: function (resultado) {
            MensajeError(resultado.responseText);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}