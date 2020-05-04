
function GuardarAsistencia() {
    if (!$('#txtHora').val() > 0 && $('#SelectEstado').val() != "3") {
        $("#validacionHora").prop("hidden", false);
        return;
    }

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
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);
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
    $("#validacionHora").prop("hidden", true);

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
        var bitacora = $('#DivTableControlAsistencia');
        bitacora.html('');
        $("#spinnerCargando").prop("hidden", false);
        $.ajax({
            url: "../Asistencia/EditarAsistenciaPartial",
            type: "GET",
            data: {
                dsLinea: Linea,
                ddFecha: Fecha
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                var bitacora = $('#DivTableControlAsistencia');

                if (resultado == "0") {

                    bitacora.html('<div class="text-center"><h3> No existen Registros </h3></div>');
                }
                else {
                    bitacora.html(resultado);
                }
                $(btnConsultar).prop("disabled", false);
                $("#spinnerCargando").prop("hidden", true);

                
            },
            error: function (resultado) {
                var bitacora = $('#DivTableControlAsistencia');
                bitacora.html('');
                MensajeError(resultado, false);
                $(btnConsultar).prop("disabled", false);
                $("#spinnerCargando").prop("hidden", true);
            }
        });
    }
}