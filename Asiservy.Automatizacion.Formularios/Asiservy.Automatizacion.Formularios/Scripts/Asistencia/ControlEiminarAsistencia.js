function LimpiarControles() {
    $('#Fecha').val('');
    $('#PartialEliminarAsistencia').empty();
    $('#btnLimpiar').prop('hidden', true);
    $('#btnConsultar').prop('hidden', false);
    $('#Fecha').prop('disabled', false);
}
function ConfirmarEliminarRegistro(data) {
   
    $('#MensajeEliminarControl').html('Está seguro que desea eliminar toda la asistencia de la línea: ' + data.Linea + '-' + data.Generado+', Turno: '+data.Turno);
    $('#ModalEliminarControl').modal('show');
    $('#Lineahide').val(data.CodLinea);
    $('#Turnohide').val(data.Turno);
    $('#Generadohide').val(data.Generado);
}
function EliminarAsistencia() {
    $('#ModalEliminarControl').modal('hide');
    $('#cargac').show();
    $.ajax({
        url: "../Asistencia/EliminarAsistenciaTotal",
        type: "POST",
        data: {
            LineaCod: $('#Lineahide').val(),
            Turno: $('#Turnohide').val(),
            Fecha: $('#Fecha').val(),
            Generado:$('#Generadohide').val()
        },
        success: function (resultado) {
            $('#cargac').hide();
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);
            ConsultarAsistenciaGenerada();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
            
        }
    });
}
function ConsultarAsistenciaGenerada() {
    if ($('#Fecha').val() == '') {
        $('#msjFecha').prop('hidden', false);
        return false;
    } else {
        $('#msjFecha').prop('hidden', true);
    }
    $('#cargac').show();
    $.ajax({
        url: "../Asistencia/PartialEliminarAsistencia",
        type: "GET",
        data: {
            Fecha: $('#Fecha').val()
        },
        success: function (resultado) {
            $('#cargac').hide();
            $('#Fecha').prop('disabled', true);
            $('#btnConsultar').prop('hidden', true);
            $('#btnLimpiar').prop('hidden', false);
            if (resultado == "101") {
                window.location.reload();
            }
            $('#PartialEliminarAsistencia').html(resultado);
            
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);

        }
    });
}