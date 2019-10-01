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
            IdProyeccionProgramacion: $('#idproyeccionmodal').val()
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

$('#tablaProyeccion').DataTable({
    "language": {
        "sProcessing": "Procesando...",
        "sLengthMenu": "Mostrar _MENU_ registros",
        "sZeroRecords": "No se encontraron resultados",
        "sEmptyTable": "Ningún dato disponible en esta tabla",
        "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
        "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
        "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
        "sInfoPostFix": "",
        "sSearch": "Buscar:",
        "sUrl": "",
        "sInfoThousands": ",",
        "sLoadingRecords": "Cargando...",
        "oPaginate": {
            "sFirst": "Primero",
            "sLast": "Último",
            "sNext": "Siguiente",
            "sPrevious": "Anterior"
        },
        "oAria": {
            "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sSortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    },
    "pageLength": 10,
    "pagingType": "full_numbers"

});