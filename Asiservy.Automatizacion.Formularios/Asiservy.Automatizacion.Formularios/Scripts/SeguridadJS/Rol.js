$.ajax({
    url: "../Seguridad/ConsultaRoles",
    type: "Get",
    success: function (resultado) {
        $('#DetalleRoles').html(resultado);
        //$("#ModalEmpleado").modal("show");

    },
    error: function (resultado) {
        MensajeError(JSON.stringify(resultado), false);
        // $('#' + formulario).remove("disabled");
    }
});

function EditarFilaRol(id, nombre, estado) {

    $('#Descripcion').val(nombre);
    $('#IdRol').val(id);

    if (estado == 'A') {
        $("input[name=EstadoRegistro][value='A']").prop("checked", true);
    }
    if (estado == 'I') {
        $("input[name=EstadoRegistro][value='I']").prop("checked", true);
    }

}
$('#Nuevo').click(function () {
    $('#Descripcion').val("");
    $('#IdRol').val(0);
    $("input[name=EstadoRegistro][value='A']").prop("checked", false);
    $("input[name=EstadoRegistro][value='I']").prop("checked", false);
});