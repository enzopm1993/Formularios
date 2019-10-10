function ConsultarCambioPersonal() {
    $.ajax({
        url: "../Asistencia/ReporteCambioPersonalPartial",
        type: "GET",
        data:
        {
            CodLinea: $('#Linea').val(),
            
        },
        success: function (resultado) {
           
            $('#resultadoreporte').html(resultado);
            
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);

        }
    });
}