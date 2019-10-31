function ConsultarAsistencia() {
    $.ajax({
        url: "../Asistencia/RptAsistenciaPartial",
        type: "GET",
        data:
        {
            FechaInicio: $('#FechaDesde').val(),
            FechaFin: $('#FechaHasta').val()
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