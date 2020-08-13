$(document).ready(function () {
    MostrarReporte();
});
function MostrarReporte() {

    $('#cargac').show();
    Error = 0;
    let params = {
        Fecha: $('#txtFecha').val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../MapeoProductoTunel/PartialReporteProduccionDiaria?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.text();
        })
        .then(function (resultado) {
            if (resultado == '"101"') {
                window.location.reload();
            }
            if (resultado != '0') {

                $('#cardreporte').prop('hidden', false);
                $('#DivReporte').empty();
                $('#DivReporte').html(resultado);
              
                //$('#mensajeRegistros').prop('hidden', true);
                $('#MensajeRegistros').prop('hidden',true);
            } else {
                $('#MensajeRegistros').prop('hidden', false);
                $('#cardreporte').prop('hidden', true);
                $('#DivReporte').empty();
                $('#mensajeRegistros').prop('hidden', false);
            }
            $('#cargac').hide();
            //console.log(resultado);
        })

        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError("Error comuníquese con el departamento de sistemas", false);
            //$('#btnCargando').prop('hidden', true);
            //$('#btnConsultar').prop('hidden', false);
        })

}