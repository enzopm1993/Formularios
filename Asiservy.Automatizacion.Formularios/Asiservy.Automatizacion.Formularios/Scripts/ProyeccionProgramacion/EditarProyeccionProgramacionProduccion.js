$(document).ready(function () {
    ValidaProyeccion();
});



function ValidaProyeccion() {
    if ($('#txtFechaProduccion').val() == "") {
        return;
    }
    $("#DivTableProyecion").html('');
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
            $("#DivTableProyecion").html('');
           // $("#DivMensaje").html("");
            if (resultado.Codigo == 0) //no se existen registros
            {
                $("#DivButtons").prop("hidden", true);                
                $("#IdProyeccion").val(0);
                $("#DivMensaje").html("<h3 class'text-center'>" + resultado.Mensaje + " </h3> ");
                $("#txtValidaEditar").val("1");

            } else if (resultado.Codigo == 4) //proyeccion se encuentra en estado finalzado
            {
                $("#DivButtons").prop("hidden", true);              
                $("#IdProyeccion").val(resultado.Observacion);
                //MensajeAdvertencia(resultado.Mensaje);
                $("#DivMensaje").html("<h3 class'text-center'>" + resultado.Mensaje + " </h3> ");
                $("#btnHabilitar").prop("hidden", true);
                $("#txtValidaEditar").val("0");                
                CargarProyeccionProgramacion();
            } else if (resultado.Codigo == 2) //Esta siendo ingresado por preparacion
            {
                $("#DivButtons").prop("hidden", true);                
                $("#IdProyeccion").val(resultado.Observacion);
               // MensajeAdvertencia(resultado.Mensaje);
                $("#DivMensaje").html("<h3 class'text-center'>" + resultado.Mensaje + " </h3> ");               
                $("#txtValidaEditar").val("0");
                CargarProyeccionProgramacion();
            } else if (resultado.Codigo == 3) //Esta siendo Editado por preparacion
            {
                $("#DivButtons").prop("hidden", true);               
                $("#IdProyeccion").val(resultado.Observacion);
                // MensajeAdvertencia(resultado.Mensaje);
                $("#DivMensaje").html("<h3 class'text-center'>" + resultado.Mensaje + " </h3> ");
                $("#txtValidaEditar").val("0");
                $("#btnHabilitar").prop("hidden", false);

                CargarProyeccionProgramacion();
            } else {
                $("#DivButtons").prop("hidden", false);               
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
                config.opcionesDT.pageLength = 50;
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

function SeleccionarProyeccionProgramacion(model)
{
    if ($("#txtValidaEditar").val() == "1") {
        limpiarModal();
        $("#ModalEditarProyeccion").modal("show");
        $("#txtIdProgramacion").val(model.IdProyeccionProgramacionDetalle);
        $("#txtHoraInicio").val(model.HoraProcesoInicio);
        $("#txtHoraFin").val(model.HoraProcesoFin);
        $("#observacionedit").val(model.Observacion);
        $("#txtLote").val(model.Lote);
        $("#txtPeso").val(model.Toneladas);
        $("#txtDestino").val(model.Destino);
        $("#btnGuardar").prop("disabled", false);

        var ArrayLineas = model.Lineas.split(",");
    //    console.log(ArrayLineas);
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
        label = this.id.split("-");
        idLabel = "Label-" + label[1];
        $("#" + idLabel).css("background", "#5a5c69");
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
        $("#" + idLabel).css("background", "#4682B4");
    } else {
        $("#" + idLabel).css("background", "#5a5c69");

    }
}