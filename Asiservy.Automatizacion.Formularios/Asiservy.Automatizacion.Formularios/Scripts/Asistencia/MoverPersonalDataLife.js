function ConsultarEmpleados() {
    //ConsultarEmpleado = "ConsultarEmpleado";

    if ($('#SelectAreaOrigen').val() != "") {
        MostrarModalCargando();
        $.ajax({
            url: "../Asistencia/MoverPersonalDataLifePartial",
            type: "GET",
            data:
            {
                //pslinea: $('#SelectLineaOrigen').val(),
                //psarea: $('#SelectAreaOrigen').val(),
                //pscargo: $('#SelectCargoOrigen').val(),
                //tipo: $('#optcambiaremp').val()
                psCentroCosto: $('#SelectAreaOrigen').val(),
                psRecurso: $('#SelectRecursoOrigen').val(),
                psLinea: $('#SelectLineaOrigen').val(),
                psCargo: $('#SelectCargoOrigen').val(),
                tipo: $('#optcambiaremp').val()
            },
            success: function (data) {
                $('#DivEmpleados').html(data);
                $('#btnGuardarCambioEmp').show();
                $('#Guardar').show();
                $('#Guardar').val('Mover Empleados');
                CerrarModalCargando();

            }
        });
        $('#contempleados').show();
    } else {
        MensajeAdvertencia("Debe seleccionar al menos el centro de costos a consultar", false);

    }


}