
function CambioLinea(valor) {
    $("#selectArea").empty();
    $("#selectArea").append("<option value='' >-- Seleccionar Opción--</option>");

    $.ajax({
        url: "../SolicitudPermiso/ConsultaListadoAreas",
        type: "Get",
        data:
        {
            CodLinea: valor
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#selectArea").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("La linea seleccionado no tiene areas asignadas", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}


function MarcarSalida(IdSolicitudPermiso,fecha) {
    console.log(IdSolicitudPermiso);
    console.log(fecha);
    $.ajax({
        type: "POST",
        url: '../SolicitudPermiso/MarcarSalidaSolicitudPermiso',
        data: {
            IdSolicitudPermiso: IdSolicitudPermiso,
            FechaBiometrico: fecha
        },
        success: function (Resultado) {
            MensajeCorrecto(Resultado,false);
            ConsultarSolicitudes();
        },
        error: function (Resultado) {
            MensajeError(Resultado);
        }
    });
   

}

function ConsultarSolicitudes() {
     $.ajax({
        type: "GET",
        url: '../SolicitudPermiso/ConsultaSolicitudes',
        data: {
            dsLinea: $('#selectLinea').val(),
            dsArea: $('#selectArea').val(),
            dsEstado: $('#selectEstado').val(),
            dsGarita: $('#Garita').val()
        },
        success: function (data) {
            $('#RptSolicitudes').html(data);
        }
    });

}


$(document).ready(function () {
    $('#TableReporteSolicitud').DataTable({
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
        "pageLength": 5,
        "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
        "pagingType": "full_numbers"
    });

    
});
