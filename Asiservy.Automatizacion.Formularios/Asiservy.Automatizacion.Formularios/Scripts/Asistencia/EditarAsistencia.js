
function GuardarAsistencia() {
    $('#EditarAsistenciaModal').modal('hide');
    $.ajax({
        url: "../Asistencia/ModificarAsistencia",
        type: "POST",
        data: {
            IdAsistencia: $('#txtId').val(),
            Observacion: $('#txtObservacion').val(),
            Hora: $('#txtHora').val(),
            EstadoAsistencia:$('#SelectEstado').val()
        },
        success: function (resultado) {
            //MensajeCorrecto(resultado);
            ConsultarControlAsistencia("btnConsultar");
        },
        error: function (resultado) {
            MensajeError(resultado.responseJSON, false);         
    
        }
    });

}

function MostarModalEditar(id,estado,hora,observacion) {  
    
    $('#SelectEstado').val(estado);
    $('#txtId').val(id);
    $('#txtHora').val(hora);
    $('#txtObservacion').val(observacion);


    $('#EditarAsistenciaModal').modal();

}


function ConsultarControlAsistencia(id) {
    
    var btnConsultar = "#" + id;
    $(btnConsultar).prop("disabled", true);
    var Linea = $('#SelectLinea').val();
    var Fecha = $('#Fecha').val();
    if (Fecha == '') {
        MensajeAdvertencia("Ingrese una fecha");
        $(btnConsultar).prop("disabled", false);
    }
    else {
        MostrarModalCargando();
        $.ajax({
            url: "../Asistencia/EditarAsistenciaPartial",
            type: "GET",
            data: {
                dsLinea: Linea,
                ddFecha: Fecha
            },
            success: function (resultado) {
                
                var bitacora = $('#DivTableControlAsistencia');
                bitacora.html(resultado);
                $(btnConsultar).prop("disabled", false);
                setTimeout(CerrarModalCargando,1000);
                
            },
            error: function (resultado) {
                
                MensajeError(resultado, false);
                $(btnConsultar).prop("disabled", false);
                setTimeout(CerrarModalCargando, 1000);
            }
        });
    }
}