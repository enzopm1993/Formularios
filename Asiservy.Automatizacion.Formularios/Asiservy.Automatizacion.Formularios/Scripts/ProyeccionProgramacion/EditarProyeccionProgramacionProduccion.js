$(document).ready(function () {
    ValidaProyeccion();
});



function ValidaProyeccion() {
    if ($('#txtFechaProduccion').val() == "") {
        return;
    }
    $("#DivTablePreparacion").html('');
    $("#DivMensaje").html("");
    $.ajax({
        url: "../ProyeccionProgramacion/ValidarProyeccionProgramacionProduccion",
        type: "GET",
        data:
        {
            Fecha: $('#txtFechaProduccion').val()
        },
        success: function (resultado) {

            $("#btnEliminar").prop("hidden", true);
            $("#btnFinalizar").prop("hidden", true);
            $("#btnHabilitar").prop("hidden", true);
            $("#DivTablePreparacion").html('');
           // $("#DivMensaje").html("");
            if (resultado.Codigo == 0) //no se existen registros
            {
                $("#DivButtons").prop("hidden", true);
                $("#btnGenerarProyecion").prop("hidden", false);
                $("#IdProyeccion").val(0);
                $("#DivMensaje").html("<h3 class'text-center'>" + resultado.Mensaje + " </h3> ");
                $("#txtValidaEditar").val("1");

            } else if (resultado.Codigo == 4) //proyeccion se encuentra en estado finalzado
            {
                $("#DivButtons").prop("hidden", true);
                $("#btnGenerarProyecion").prop("hidden", true);
                $("#IdProyeccion").val(resultado.Observacion);
                //MensajeAdvertencia(resultado.Mensaje);
                $("#DivMensaje").html("<h3 class'text-center'>" + resultado.Mensaje + " </h3> ");
                $("#btnHabilitar").prop("hidden", true);
                $("#txtValidaEditar").val("0");                
                CargarProyeccionProgramacion();
            } else if (resultado.Codigo == 2) //Esta siendo Editado
            {
                $("#DivButtons").prop("hidden", true);
                $("#btnGenerarProyecion").prop("hidden", true);
                $("#IdProyeccion").val(resultado.Observacion);
               // MensajeAdvertencia(resultado.Mensaje);
                $("#DivMensaje").html("<h3 class'text-center'>" + resultado.Mensaje + " </h3> ");               
                $("#txtValidaEditar").val("0");
                CargarProyeccionProgramacion();
            } else {
                $("#DivButtons").prop("hidden", false);
                $("#btnGenerarProyecion").prop("hidden", true);
                $("#IdProyeccion").val(resultado.Observacion);
                $("#btnEliminar").prop("hidden", false);
                $("#btnFinalizar").prop("hidden", false);
                CargarProyeccionProgramacion();
                $("#txtValidaEditar").val("1");
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);

        }
    });
}



function CargarProyeccionProgramacion() {

    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../ProyeccionProgramacion/ProyeccionProgramacionDetallePartial",
        type: "GET",
        data:
        {
            IdProgramacion: $('#IdProyeccion').val(),
            proceso:2
        },
        success: function (resultado) {
            if (resultado == 0) {
                $("#DivMensaje").html("<h3 class'text-center'> No existen registros </h3> ");
            } else {
                $("#DivTableProyecion").html(resultado);
                $('#tblDataTable').DataTable(config.opcionesDT);

            }
            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}

function SeleccionarProyeccionProgramacion( idDetalle,lineas,horainicio,horafin,observacion)
{
    if ($("#txtValidaEditar").val() == "1") {
        limpiarModal();
        $("#ModalEditarProyeccion").modal("show");
        $("#txtIdProgramacion").val(idDetalle);
        $("#txtHoraInicio").val(horainicio);
        $("#txtHoraFin").val(horafin);
        $("#observacionedit").val(observacion);
        $("#btnGuardar").prop("disabled", false);

        var ArrayLineas = lineas.split(",");
        $("#form input").each(function () {
            if (ArrayLineas.some(x => x == this.value)) {
                this.checked = true;                
            }
            label = this.id.split("-");
            idLabel = "Label-" + label[1];
            PintarLinea(this.id, idLabel);
        }) 

    }
}

function limpiarModal()
{
    $("#form input").each(function () {
        if (this.checked) {
            this.checked = false;
        }
    }) 
    $("#txtIdProgramacion").val("0");
    $("#txtHoraInicio").val("");
    $("#txtHoraFin").val("");
}

function GuardarProyeccionDetalle() {
    var lineas = new Array(); 
    $("#form input").each(function () {       
        if (this.checked) {
            lineas.push(this.value);
        }
    }) 
    var todasLineas = "";
    for (i = 0; i < lineas.length; i++){
        todasLineas += lineas[i];
        if (i != lineas.length - 1) {
            todasLineas += ",";
        }
    }
    if (!Validar(lineas)) {
        return;
    }

    $("#btnGuardar").prop("disabled",true);
    $.ajax({
        url: "../ProyeccionProgramacion/GuardarModificarProyeccionProgramacionDetalle",
        type: "Post",
        data:
        {
            IdProyeccionProgramacionDetalle: $('#txtIdProgramacion').val(),
            Lineas: todasLineas,
            HoraProcesoInicio: $("#txtHoraInicio").val(),
            HoraProcesoFin: $("#txtHoraFin").val(),
            Observacion: $("#observacionedit").val(),
            proceso: 2

        },
        success: function (resultado) {
            $("#ModalEditarProyeccion").modal("hide");
            CargarProyeccionProgramacion();
            MensajeCorrecto(resultado);

        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#ModalEditarProyeccion").modal("hide");

        }
    });

}

function Validar(lineas) {
    var valida = true;  
    if (lineas.length  > 0) {
        $("#msgerrorLineas").prop("hidden", true);
    } else {
        $("#msgerrorLineas").prop("hidden", false);
        valida = false;
    }
    if ($("#txtHoraInicio").val() != "") {
        $("#msgerrorHoraInicio").prop("hidden", true);
    } else {
        $("#msgerrorHoraInicio").prop("hidden", false);
        valida = false;
    }
    if ($("#txtHoraFin").val() != "") {
        $("#msgerrorHoraFin").prop("hidden", true);
    } else {
        $("#msgerrorHoraFin").prop("hidden", false);
        valida = false;
    }
    return valida;
}




function FinalizarProyeccionProgramacion() {
    $.ajax({
        url: "../ProyeccionProgramacion/FinalizarIngresoProyeccionProgramacion",
        type: "GET",
        data:
        {
            id: $('#IdProyeccion').val(),
            proceso:2
        },
        success: function (resultado) {          
            ValidaProyeccion();
            MensajeCorrecto(resultado);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}

function HabilitarProyeccionProgramacion() {
    $.ajax({
        url: "../ProyeccionProgramacion/HabilitarIngresoProyeccionProgramacion",
        type: "GET",
        data:
        {
            id: $('#IdProyeccion').val(),
            proceso:2
        },
        success: function (resultado) {            
            ValidaProyeccion();
            MensajeCorrecto(resultado);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}


function PintarLinea(id, idLabel) {
    if ($("#" + id).prop("checked")) {
        $("#" + idLabel).css("background", "#00FF00");
    } else {
        $("#" + idLabel).css("background", "#5a5c69");

    }
}