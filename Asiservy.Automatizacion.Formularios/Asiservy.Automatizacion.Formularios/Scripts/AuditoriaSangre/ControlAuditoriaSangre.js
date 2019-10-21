function CargarEmpleados(formulario) {
    //console.log($('#selectLinea').val());   
    //console.log($('#selectArea').val());   
    //console.log($('#selectCargo').val());  
    if ($('#selectLinea').val() != '') {
        $('#' + formulario).attr("disabled", true);
        $.ajax({
            url: "../AuditoriaSangre/EmpleadoBuscar",
            type: "Get",
            data:
            {
                dsLinea: $('#selectLinea').val(),
                dsArea: $('#selectArea').val(),
                dsCargo: $('#selectCargo').val()
            },
            success: function (resultado) {
                $('#ModelCargarEmpleados').html(resultado);
                $("#ModalEmpleado").modal("show");
                $('#' + formulario).attr("disabled", false);
            },
            error: function (resultado) {
                MensajeError(JSON.stringify(resultado), false);
                $('#' + formulario).remove("disabled");
            }
        });
    } else {
        MensajeAdvertencia("Seleccione una LINEA", false)
    }
}
function IngresarAuditoriaSangre() {
    var estado;
    if ($('#Estado').prop('checked')) {
        estado = "A";
    } else {
        estado = "I";
    }
    if ($('#Cedula').val() == "" || $('#Porcentaje').val() == "") {
        MensajeError("Los campos Cédula y Porcentaje son obligatorios", false);
    } else {
        $.ajax({
            url: "../AuditoriaSangre/ControlAuditoriaSangrePartial",
            type: "POST",
            data:
            {
                Cedula: $('#Cedula').val(),
                Porcentaje: $('#Porcentaje').val(),
                Fecha: $('#FechaAuditoria').val(),
                Estado: estado
            },
            success: function (resultado) {
                $('#Cedula').val("");
                $('#Porcentaje').val("");
                $('#ControlAuditoriaSangre').html(resultado);
                MensajeCorrecto("Registro ingresado con éxito", true);
            },
            error: function (resultado) {
                MensajeError(JSON.stringify(resultado), false);

            }
        });
    }
    

}
function CambiarLableEstado() {
    if ($('#Estado').prop('checked')) {
        //console.log("checked");
        $('#EstadoLabel').text("Activo");
    } else {
        //console.log("No checked");
        $('#EstadoLabel').text("Inactivo");
    }
}
//CargarAuditoria('@item.Fecha', '@item.Cedula', '@item.Porcentaje')
function CargarAuditoria(fecha,cedula,porcentaje) {
    $('#FechaAuditoria').val(fecha);
    $('#Cedula').val(cedula);
    $('#Porcentaje').val(porcentaje);
    
    $('#Estado').prop('checked', true)
    $('#EstadoLabel').text("Activo");
}
$('#TablaAuditoriaSangre').DataTable({
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