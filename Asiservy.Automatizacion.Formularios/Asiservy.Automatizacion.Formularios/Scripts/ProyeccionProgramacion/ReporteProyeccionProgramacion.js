

function CargarReporte() {
    MostrarModalCargando();
    $.ajax({
        url: "../ProyeccionProgramacion/ReporteProyeccionProgramacionPartial",
        type: "GET",
        data: { Fecha: $('#txtFecha').val() },
        success: function (resultado) {            
            var m = document.getElementById("DivTableReporteProyeccion");
            m.innerHTML = resultado;
            CerrarModalCargando();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText);
            CerrarModalCargando(); 
        }
    });
}