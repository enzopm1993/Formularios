


function CargarReporteAvanceLimpiadora() {
    var txtFecha = $('#txtFecha').val();
    var selectLinea = $('#selectLinea').val();
    $('#btnConsultar').prop("disabled", true);
    MostrarModalCargando();
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
            CerrarModalCargando();
        },
        error: function (resultado) {
            MensajeError(resultado.responseJSON, false);
            $('#btnConsultar').prop("disabled", false);
            CerrarModalCargando();

        }
    });

}