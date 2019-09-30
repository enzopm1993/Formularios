




function CargarReporteControlCuchillo() {
    var fecha = $('#txtFecha').val();
    var linea = $('#selectLinea').val();
    if (fecha == '' || linea == '') {

        MensajeAdvertencia("Ingrese filtros de busqueda");
        return;
    }
    $.ajax({
        url: "../Asistencia/ReporteControlCuchilloPartial",
        type: "GET",
        data: {
            Fecha: fecha,
            Linea: linea
        },
        success: function (resultado) {

            var bitacora = $('#DivTableControlCuchillo');
            bitacora.html(resultado);
        },
        error: function (resultado) {
            MensajeError(resultado, false);
            var bitacora = $('#DivTableControlCuchillo');
            bitacora.html('');
        }
    });
}