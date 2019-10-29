function SetearValoresDeModal(LineasSelec) {

    //console.log(LineasSelec.length);
    for (var i = 0; i < LineasSelec.length; i++) {

    }
    $("input[type=checkbox]").each(function () {
        for (var i = 0; i < LineasSelec.length; i++) {
            if ($(this).val() == LineasSelec[i]) {
                $(this).prop("checked", true);
                
                var idlabel = $(this).val().slice(-1);
                var numero = idlabel < 9 ? '0' + idlabel : idlabel;
                //console.log(idlabel);
                $('#Label-' + numero).removeClass("btn-dark");
                $('#Label-' + numero).addClass("btn-info");
                //PintarLinea($(this).val().slice(-1));
            }
        }
        //alert($(this).val());
    });
}
function ConsultaProyProgramacion() {
    //console.log('entro');
    $.ajax({
        url: "../ProyeccionProgramacion/ProyeccionProgramacionEditPartial",
        type: "POST",
        data:
        {
            Fecha: $('#FechaProduccion').val()
        },
        success: function (resultado) {

            $('#DivEditarProyeccion').empty();
            $('#DivEditarProyeccion').html(resultado);

        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);

        }
    });
}
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
            Observacion: $('#observacionedit').val(),
            FechaProduccion: $('#FechaProduccion').val()
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

function AbrirModal(IdProyeccion,Observacion,Lineas,HoraInicio,HoraFin) {
    $.ajax({
        url: "../ProyeccionProgramacion/ModalEditarProyeccion",
        type: "POST",
        data: {
            IdProyeccion: IdProyeccion,
            Observacion: Observacion,
            Lineas: Lineas,
            HoraInicio: HoraInicio,
            HoraFin: HoraFin
        },
        success: function (resultado) {
            $('#modaleditarpro').empty();
            $('#modaleditarpro').html(resultado);
            //var m = document.getElementById("modaleditarpro");
            //m.innerHTML = resultado;
            //var modal = document.getElementById("ModalError");
            $("#ModalEditarProyeccion").modal("show");
            //document.getElementById('mensajeCorrecto').innerHTML = mensaje;
            //console.log(r);
        }
    });
}
