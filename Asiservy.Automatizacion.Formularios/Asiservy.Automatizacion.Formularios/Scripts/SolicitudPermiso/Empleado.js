function CargarEmpleados(formulario) {    
    if ($('#selectLinea').val() != '') {
        $('#' + formulario).attr("disabled", true);
        $.ajax({
            url: "../SolicitudPermiso/EmpleadoBuscar",
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