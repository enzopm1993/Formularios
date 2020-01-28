
$(document).ready(function () {
    ConsultarEntregaUniformes();
});

function NuevaEntregaUniforme() {
    $("#NombreEmpleado").val('');
    $("#Identificacion").val('');
    $("#selectLinea").prop("selectedIndex", 0);
}

function AgregarEntregaUniforme() {
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }
    if ($("#NombreEmpleado").val() == "") {
        $("#NombreEmpleado").css('borderColor', '#FA8072');
        return;
    } else {
        $("#NombreEmpleado").css('borderColor', '#ced4da');
    }

    $("#ModalEntregaUniforme").modal("show");
}

function GenerarEntregaUniforme() {    
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }
    if ($("#NombreEmpleado").val() == "") {
        $("#NombreEmpleado").css('borderColor', '#FA8072');
        return;
    } else {
        $("#NombreEmpleado").css('borderColor', '#ced4da');
    }
    $.ajax({
        url: "../EntregaUniforme/EntregaUniforme",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),
            Cedula: $("#Identificacion").val()
           
        },
        success: function (resultado) {
            $("#ModalEntregaUniforme").modal("hide");
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == "0") {
                MensajeAdvertencia("Faltan parametros");
                return;
            } else if (!resultado.Respuesta) {
                MensajeAdvertencia(resultado.Mensaje);
                return;
            }
            NuevaEntregaUniforme();
            MensajeCorrecto("Registro Exitoso");
            ConsultarEntregaUniformes();
        },
        error: function (result) {
           /* console.log(JSON.stringify(result.responseText))*/;
            MensajeError(resultado.Mensaje, false);   
        }
    });
}

function ConsultarEntregaUniformes() {
    $("#spinnerCargando").prop("hidden", false);
    if ($("#txtFecha").val() == "") {
        return;
    }
    $('#divEntregaUniforme').html('');
    $.ajax({
        url: "../EntregaUniforme/EntregaUniformePartial",
        type: "GET",
        data: {
           Fecha:$("#txtFecha").val()
        },
        success: function (resultado) {
            //console.log(JSON.stringify(resultado));
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == "0") {
                var bitacora = $('#divEntregaUniforme');
                bitacora.html('No existen registros');
                $("#spinnerCargando").prop("hidden", true);
                return;
            }
            var bitacora = $('#divEntregaUniforme');
            $("#spinnerCargando").prop("hidden", true);            
            bitacora.html(resultado);
            config.opcionesDT.pageLength = -1;
            config.opcionesDT.order = [4, "desc"];
            $('#tblDataTable').DataTable(config.opcionesDT);

           
        },
        error: function (result) {
           /* console.log(JSON.stringify(result.responseText))*/;
            MensajeError(resultado.Mensaje, false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function InactivarEntregaUniforme() {
    $.ajax({
        url: "../EntregaUniforme/EliminarEntregaUniforme",
        type: "POST",
        data: {
            IdEntregaUniforme: $("#txtEliminarEntregaUniforme").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            ConsultarEntregaUniformes();
            //   MensajeCorrecto("Registro Eliminado con Éxito");
            $("#modalEliminarProcesoDetalle").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarEntregaUniforme(model) {
    $("#txtEliminarEntregaUniforme").val(model.IdEntregaUniforme);
    $("#pModalDetalle").html("Empleado: " + model.Nombres);
    $("#modalEliminarEntregaUniforme").modal('show');
}


$("#modal-Detalle-btn-si").on("click", function () {  
    InactivarEntregaUniforme();   
    $("#txtEliminarEntregaUniforme").val('0');
    $("#modalEliminarEntregaUniforme").modal('hide');
});

$("#modal-Detalle-btn-no").on("click", function () {
    $("#txtEliminarEntregaUniforme").val('0');
    $("#modalEliminarEntregaUniforme").modal('hide');
});