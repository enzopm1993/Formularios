


function CargarReporteAvanceLimpiadora() {
    var txtFecha = $('#txtFecha').val();
    var selectLinea = $('#selectLinea').val();
    $('#btnConsultar').prop("disabled", true);
    $("#spinnerCargando").prop("hidden", false);
    var bitacora = $('#DivTableReporteControlAvancePorLimpiadora');
    bitacora.html('');
    $.ajax({
        url: "../Hueso/ReporteAvanceDiarioPorLimpiadoraPartial",
        type: "GET",
        data: {
            ddFecha: txtFecha,
            dsLinea: selectLinea
        },
        success: function (resultado) {
            var bitacora = $('#DivTableReporteControlAvancePorLimpiadora');
            bitacora.html(resultado);

            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError(resultado.responseJSON, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);

           
        }
    });

}