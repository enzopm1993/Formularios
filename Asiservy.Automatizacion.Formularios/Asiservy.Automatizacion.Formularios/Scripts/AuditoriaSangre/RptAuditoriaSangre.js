$(document).ready(function () {
    var fecha = new Date(); //Fecha actual
    var mes = fecha.getMonth() + 1; //obteniendo mes
    var dia = fecha.getDate(); //obteniendo dia
    var ano = fecha.getFullYear(); //obteniendo año
    if (dia < 10)
        dia = '0' + dia; //agrega cero si el menor de 10
    if (mes < 10)
        mes = '0' + mes //agrega cero si el menor de 10
    document.getElementById('Fecha').value = ano + "-" + mes + "-" + dia;



});

function ConsultarReporteAuditoriaSangre() {
    if (($('#Lineas').prop('selectedIndex') == 0) || ($('#Fecha')=="")) {
        ConsultarRptAuditoriaSangre
        MensajeError("El campo Fecha y Línea son obligatorios", false);
    } else {
        $('#ConsultarRptAuditoriaSangre').attr("disabled", true);
        $.ajax({
            url: "../AuditoriaSangre/ReporteAuditoriaSangrePArtial",
            type: "GET",
            data:
            {
                CodLinea: $('#Lineas').val(),
                Fecha: $('#Fecha').val()
            },
            success: function (resultado) {
                $('#DivReporteSangre').html(resultado);
                $('#ConsultarRptAuditoriaSangre').attr("disabled", false);
            },
            error: function (resultado) {
                MensajeError(JSON.stringify(resultado), false);
                $('#ConsultarRptAuditoriaSangre').attr("disabled", false);
            }
        });
    }

    
}

