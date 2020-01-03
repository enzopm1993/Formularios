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
function MoverEmpleados() {
    i = 0; 
    var result = new Array();
    //ConsultarEmpleado = "ConsultarEmpleado";
    if ($('#selectLinea').val() != "" && $('#SelectArea').val() != "" && $('#SelectRecurso').val() != "" && $('#SelectCargo').val() != "") {
        $("input[type=checkbox]:checked").each(function (resultado) {
            id = $(this).attr("id");
            this.id = id.replace('Empleado-', '');
            result.push(this.id);
            i++;
        });
    } else {
        MensajeAdvertencia("Centro de Costos, Recurso, Línea,Cargo,fecha y hora son obligatorios", false);
    }
        MostrarModalCargando();
        $.ajax({
            url: "../Nomina/ActualizaEmpleadosArea",
            type: "POST",
            data:
            {
                //pslinea: $('#SelectLineaOrigen').val(),
                //psarea: $('#SelectAreaOrigen').val(),
                //pscargo: $('#SelectCargoOrigen').val(),
                //tipo: $('#optcambiaremp').val()
                Cedula: result,
                CentroCostos: $('#SelectArea').val(),
                Recurso: $('#SelectRecurso').val(),
                Linea: $('#SelectLinea').val(),
                Cargo: $('#SelectCargo').val(),
            },
            success: function (data) {
                //$('#DivEmpleados').html(data);
                //$('#btnGuardarCambioEmp').show();
                //$('#Guardar').show();
                //$('#Guardar').val('Mover Empleados');
                CerrarModalCargando();
                //MensajeCorrecto("Empleados movidos con éxito", true);
                console.log(resultado);
                $('#BodyMensajeCp').html(resultado);
                $('#ModalMensajeCP').modal('show');
                

            },
            error: function (resultado) {
                //MensajeError("No se pudieron mover", false);
                MensajeError(resultado, false);
            }
        });
        $('#contempleados').show();
   
}