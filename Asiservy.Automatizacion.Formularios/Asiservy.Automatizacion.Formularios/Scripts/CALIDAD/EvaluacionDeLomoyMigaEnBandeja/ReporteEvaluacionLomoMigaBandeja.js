var Error = 0;
function ConsultarReporte() {
    $('#cargac').show();
    Error = 0;
    let params = {
        Fecha: $('#txtFechaProduccion').val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EvaluacionDeLomoyMigaEnBandeja/PartialReporteEvaluacionLomosMigasBandeja?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.text();
        })
        .then(function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado != '"0"') {
                $('#DivReporte').empty();
                $('#DivReporte').html(resultado);
                $('#mensajeRegistros').prop('hidden', true);
                //config.opcionesDT.pageLength = 30;
                //$('#tblReporte').DataTable(config.opcionesDT);
                //LimpiarDetalleControles();
            } else {
                $('#DivReporte').empty();
                $('#mensajeRegistros').prop('hidden', false);
            }
            $('#cargac').hide();
            //console.log(resultado);
        })
        .catch(function (resultado) {
            console.log(resultado);
            MensajeError(resultado.responseText, false);
            //$('#btnCargando').prop('hidden', true);
            //$('#btnConsultar').prop('hidden', false);
        })
}