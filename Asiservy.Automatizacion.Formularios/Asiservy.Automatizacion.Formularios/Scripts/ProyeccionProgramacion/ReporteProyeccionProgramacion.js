

function CargarReporte() {
    MostrarModalCargando();
    $.ajax({
        url: "../ProyeccionProgramacion/ReporteProyeccionProgramacionPartial",
        type: "GET",
        data: { Fecha: $('#txtFecha').val() },
        success: function (resultado) {     
            $('#DivTableReporteProyeccion').html(resultado);
            CerrarModalCargando();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText);
            CerrarModalCargando(); 
        }
    });
}

