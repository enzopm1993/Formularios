function PintarLinea(id) {
    if ($('#Linea-' + id).prop('checked')) {
        $("#Label-" + id).removeClass("btn-dark");
        $("#Label-" + id).addClass("btn-info");
    } else {
        $("#Label-" + id).removeClass("btn-info");
        $("#Label-" + id).addClass("btn-dark");
    }
}
function ActualizarProyeccion() {
    var selected = '';
    $('#DivBodyEditar input[type=checkbox]').each(function () {
        if (this.checked) {
            selected += $(this).val() + ',';
        }
    }); 
    selected = selected.slice(0, -1);
    //alert(selected)
    if (selected == "") {
        //MensajeError("Debe seleccionar al menos una línea", false);
        $('#msgerrorLineas').show();
        return false;
    }
    if ($('#HoraInicio').val() == "") {
        //MensajeError("Debe ingresar la hora de inicio", false);
        
        $('#msgerrorHoraInicio').show();
        $('#HoraInicio').focus();
        return false;
    }
    if ($('#HoraFin').val() == "") {
        //MensajeError("Debe ingresar la hora de Fin", false);
        $('#msgerrorHoraFin').show();
        $('#HoraFin').focus();
        return false;
    }
    if ($('#HoraFin').val() < $('#HoraInicio').val()) {
        $('#msgerrorHoras').show();
        return false;
    }
    
    $('#ModalEditarProyeccion').modal('hide')
    $.ajax({
        url: "../ProyeccionProgramacion/ProyeccionProgramacionEditarPartial",
        type: "POST",
        data:
        {
            Lineas: selected,
            HoraInicio: $('#HoraInicio').val(),
            HoraFin: $('#HoraFin').val(),
            IdProyeccionProgramacion: $('#idproyeccionmodal').val(),
            Observacion: $('#observacionedit').val()
            //IdProyeccionProgramacion: idPro,
            //Lote: $('#Lote').val(),
            //FechaProduccion: $('#FechaProduccion').val(),
            //Toneladas: $('#Toneladas').val(),
            //Destino: $('#Destino').val(),
            //TipoLimpieza: $('#TipoLimpieza').val(),
            //Observacion: $('#Observacion').val()
        },
        success: function (resultado) {
            //Limpiar();
            $('#DivEditarProyeccion').html(resultado);
            MensajeCorrecto("Registro ingresado con éxito", false);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);

        }
    });
}

function AbrirModal(IdProyeccion) {
    $.ajax({
        url: "../ProyeccionProgramacion/ModalEditarProyeccion",
        type: "POST",
        data: { IdProyeccion: IdProyeccion },
        success: function (resultado) {
            
            var m = document.getElementById("modaleditarpro");
            m.innerHTML = resultado;
            //var modal = document.getElementById("ModalError");
            $("#ModalEditarProyeccion").modal("show");
            //document.getElementById('mensajeCorrecto').innerHTML = mensaje;
            //console.log(r);
        }
    });
}
