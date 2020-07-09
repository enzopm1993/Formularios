var ListadoGeneral = [];

$(document).ready(function () {
    CargarReporteAvance();
});


function CargarReporteAvance() {
    $("#selectLinea").prop("selectedIndex", 0);
    var txtFecha = $('#txtFecha').val();
    if (txtFecha == "") {
        MensajeAdvertencia("Igrese una Fecha.");
        return;
    }
    if ($("#selectTurno").val() == "") {
        MensajeAdvertencia("Seleccione un turno.");
        return;
    }
    $('#btnConsultar').prop("disabled", true);
    $("#spinnerCargando").prop("hidden", false);
    $('#DivTableReporteControlAvance').html("");
    $.ajax({
        url: "../Hueso/ReporteRendimientoLotePartial",
        type: "GET",
        data: {
            ddFecha: txtFecha,
            Turno: $("#selectTurno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $('#btnConsultar').prop("disabled", false);
            if (resultado == "1") {
                $('#DivTableReporteControlAvance').html("No existen Registros");
                $("#spinnerCargando").prop("hidden", true);
            } else {
                $("#spinnerCargando").prop("hidden", true);
                $('#DivTableReporteControlAvance').html(resultado);
                config.opcionesDT.pageLength = 50;
                config.opcionesDT.order = [[0, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}