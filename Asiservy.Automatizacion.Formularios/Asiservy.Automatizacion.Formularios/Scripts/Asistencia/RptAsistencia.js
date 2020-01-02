function ConsultarAsistencia() {
    if ($('#FechaDesde').val() == '') {
        $('#msjerrorFecha1').show();
        return false;
    } else {
        $('#msjerrorFecha1').hide();
    }
    $.ajax({
        url: "../Asistencia/RptAsistenciaPartial",
        type: "GET",
        data:
        {
            FechaInicio: $('#FechaDesde').val(),
            FechaFin: $('#FechaHasta').val(),
            Linea: $('#Linea').val(),
            Turno: $('#Turno').val()
        },
        success: function (resultado) {

            $('#divRptAsistencia').html(resultado);
            //MensajeCorrecto("Registro ingresado con éxito", false);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);

        }
    });
}