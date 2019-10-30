function LimpiarBoton() {
    $('#Cedula').val("");
    $('#Porcentaje').val("");
    $('#Nombre').val("");
    $('#IdAuditoriaSangre').val("");

}
function ConsultarAuditoriaChange() {
    $.ajax({
        url: "../AuditoriaSangre/ControlAuditoriaSangrePartial",
        type: "POST",
        data:
        {
            Fecha: $('#FechaAuditoria').val(),
            change: 1
        },
        success: function (resultado) {
            $('#Cedula').val("");
            $('#Porcentaje').val("");
            $('#Nombre').val("");
            $('#IdAuditoriaSangre').val("");
            $('#ControlAuditoriaSangre').html(resultado);
            //MensajeCorrecto("Registro ingresado con éxito", false);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);

        }
    });
}
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
                IdAuditoria: $('#IdAuditoriaSangre').val(),
                Cedula: $('#Cedula').val(),
                Porcentaje: $('#Porcentaje').val(),
                Fecha: $('#FechaAuditoria').val(),
                Estado: estado
            },
            success: function (resultado) {
                $('#Cedula').val("");
                $('#Porcentaje').val("");
                $('#Nombre').val("");
                $('#IdAuditoriaSangre').val("");
                $('#ControlAuditoriaSangre').html(resultado);
                MensajeCorrecto("Registro ingresado con éxito", false);
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
function CargarAuditoria(fecha, cedula, porcentaje, nombres, idauditoriasangre) {
    $('#IdAuditoriaSangre').val(idauditoriasangre);
    $('#Nombre').val(nombres);
    //$('#FechaAuditoria').val(fecha);
    $('#Cedula').val(cedula);
    $('#Porcentaje').val(porcentaje);
    
    $('#Estado').prop('checked', true)
    $('#EstadoLabel').text("Activo");
}
