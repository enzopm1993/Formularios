$(document).ready(function () {
    //$('#IdRol').select2({
    //    width: 'resolve'
    //});
    //$('#IdOpcion').select2({
    //    width: 'resolve'
    //});
    $("#IdRol").select2({
        width: '100%' // need to override the changed default
    });
    $("#IdOpcion").select2({
        width: '100%' // need to override the changed default
    });
    //$("#IdRol").select2();
    //$("#IdOpcion").select2();
});
$.ajax({
    url: "../Seguridad/ConsultaOpcionRol",
    type: "Get",
    success: function (resultado) {
        $('#DetalleOpcionesRoles').html(resultado);
        //$("#ModalEmpleado").modal("show");

    },
    error: function (resultado) {
        MensajeError(JSON.stringify(resultado), false);
        // $('#' + formulario).remove("disabled");
    }
});

function EditarFilaOpRol(IdOr, IdRol, IdOpcion, estado) {

    $('#IdRolh').val(IdRol);
    $('#IdOpcionh').val(IdOpcion);
    $('#IdOpcionRol').val(IdOr);

    $('#IdRol').val(IdRol).trigger('change');
   // $("#IdRol").val(IdRol)
    $('#IdRol').attr("disabled", true);

    $("#IdOpcion").val(IdOpcion).trigger('change');
    $('#IdOpcion').attr("disabled", true);
    if (estado == 'A') {
        $("input[name=EstadoRegistro][value='A']").prop("checked", true);
    }
    if (estado == 'I') {
        $("input[name=EstadoRegistro][value='I']").prop("checked", true);
    }

}
$('#NuevoRegistro').click(function () {
    $("#IdRol").prop('selectedIndex', 0).change();
    $("#IdOpcion").prop('selectedIndex', 0).change();

    $('#IdRolh').val("");
    $('#IdOpcionh').val("");
    $('#IdOpcionRol').val(0);
    $("input[name=EstadoRegistro][value='A']").prop("checked", false);
    $("input[name=EstadoRegistro][value='I']").prop("checked", false);
   // $("#IdRol").val("");
   // $("#IdOpcion").val("");
    $('#IdRol').attr("disabled", false);
    $('#IdOpcion').attr("disabled", false);
});
$('#IdOpcion').change(function () {
 
    $('#IdOpcionh').val($('#IdOpcion').val());
});
$('#IdRol').change(function () {
   
    $('#IdRolh').val($('#IdRol').val());

});
