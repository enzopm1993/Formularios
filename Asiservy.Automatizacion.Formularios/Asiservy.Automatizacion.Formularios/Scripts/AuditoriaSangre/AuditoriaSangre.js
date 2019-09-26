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
function IngresarAuditoriaSangre () {
    $.ajax({
        url: "../AuditoriaSangre/ControlAuditoriaSangrePartial",
        type: "POST",
        data:
        {
            Cedula: $('#Cedula').val(),
            Porcentaje: $('#Porcentaje').val(),
            Fecha: $('#FechaAuditoria').val()
        },
        success: function (resultado) {
            $('#Cedula').val("");
            $('#Porcentaje').val("");
            $('#ControlAuditoriaSangre').html(resultado);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
         
        }
    });

}