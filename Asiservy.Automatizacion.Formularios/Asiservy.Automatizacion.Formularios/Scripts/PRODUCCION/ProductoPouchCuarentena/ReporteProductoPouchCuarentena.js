var Error = 0;
function ConsultarReportePouchCuarentena() {
    Error = 0;
    let params = {
        Fecha: $('#txtFechaProduccion').val(),
        Turno: $('#cmbTurno').val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../ProductoPouchCuarentena/PartialReporteProductoPouchCuarentena?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            if (!respuesta.ok) {
                //MensajeError(respuesta.statusText);
                MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
                Error = 1;
            }
            return respuesta.text();
        })
        .then(function (resultado) {
            if (resultado == '"101"') {
                window.location.reload();
            }
            if (Error == 0) {
                if (resultado != '"0"') {
                    $('#DivReporte').html(resultado);
                    $('#MsjRegistros').prop('hidden', true);
                } else {
                    $('#MsjRegistros').prop('hidden', false);
                }
                
              
            }
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}

function imprimirw() {
    window.print();
}