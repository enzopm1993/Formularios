﻿Datos = [];
$(document).ready(function () {
    $("#selectCondicion").select2();
    $('#selectCondicion').select2({
        width: '100%'
    });
    ConsultarControl();
});

function ValidaEstadoReporte(Fecha) {
    $.ajax({
        url: "../CondicionPersonal/ValidaEstadoReporte",
        type: "GET",
        data: {
            Fecha: Fecha,
            Turno: $("#selectTurno").val()
            
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            // console.log(resultado == 0);
            if (resultado == 0) {
                $("#lblAprobadoPendiente").html("");

            } else if (resultado == 1) {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);

            } else {
                $("#lblAprobadoPendiente").removeClass("badge-info").addClass("badge-danger");
                $("#lblAprobadoPendiente").html(Mensajes.Pendiente);
            }
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}


function ConsultarControl() {
    $("#chartCabecera2").html('');
    if ($("#txtFecha").val() == '' || $("#selectTurno").val()=='') {
        return;
    }
    ValidaEstadoReporte($("#txtFecha").val());
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../CondicionPersonal/CondicionPersonalPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val(),
            Turno: $("#selectTurno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#chartCabecera2").html('<div class="text-center"><h4 class="text-warning">'+Mensajes.SinRegistros+'</h4></div>');
                $("#spinnerCargando").prop("hidden", true);
            } else {
                $("#spinnerCargando").prop("hidden", true);
                $("#chartCabecera2").html(resultado);
                config.opcionesDT.pageLength = -1;
                config.opcionesDT.order = false;
                config.opcionesDT.ordering = false;
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            console.log(resultado);
            MensajeError("Error: Comuníquese con sistemas", false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function ValidarEmpleado() {
    var valida = true;
  
    if ($("#Lineas").val() == "") {
        $("#Lineas").css("border-color", "#DC143C");
        valida = false;
    } else {
        $("#Lineas").css("border-color", "#d1d3e2");
    }

    return valida;
}
function CargarEmpleados(formulario) {

    if (!ValidarEmpleado()) {
        return;
    }
    MostrarModalCargando();
    $('#' + formulario).attr("disabled", true);
    $.ajax({
        url: "../General/EmpleadoBuscar",
        type: "Get",
        data:
        {
            dsLinea: $('#Lineas').val()
        },
        success: function (resultado) {
            $('#ModelCargarEmpleados').html(resultado);
            $("#ModalEmpleado").modal("show");
            $('#' + formulario).attr("disabled", false);
            CerrarModalCargando();
        },
        error: function (resultado) {
            MensajeError("Se ha generado un error, Comuniquese con sistemas.", false);
            $('#' + formulario).remove("disabled");
            CerrarModalCargando();

        }
    });

}

function SeleccionarControl(model) {
    //console.log(model);
    $("#CargarEmpleadoAS").prop("disabled",true);
    $("#txtFecha").prop("disabled", true);
    $("#Lineas").prop("disabled", true);
    $("#Cedula").val(model.Cedula);
    $("#Nombre").val(model.Nombre);
    $("#txtIdCondicionPersonal").val(model.IdCondicionPersonal);
    $("#txtHora").val(model.Hora);
    $("#txtObservacion").val(model.Observacion);
    $("#selectCondicion").val(model.CodCondicion).change();
    $("#btnEliminar").prop("hidden", false);
    Datos = model;
}

function NuevoControl() {
    $("#CargarEmpleadoAS").prop("disabled", false);
    $("#txtFecha").prop("disabled", false);
    $("#Lineas").prop("disabled", false);
    $("#Lineas").css("border-color", "#d1d3e2");
    $("#Cedula").val('');
    $("#Nombre").val('');
    $("#txtIdCondicionPersonal").val('0');
    $("#txtHora").val(moment().format("YYYY-MM-DDTHH:mm"));
    $("#txtObservacion").val('');
    $("#selectCondicion").prop("selectedIndex", 0).change();
    $("#Lineas").prop("selectedIndex", 0);
    $("#btnEliminar").prop("hidden", true);
    Datos = [];


}

function Validar() {
    var valida = true;
    if ($("#Cedula").val() == "" && $("#selectCondicion").val()!='1') {
        $("#Nombre").css('borderColor', '#FA8072');
        valida=false;
    } else {
        $("#Nombre").css('borderColor', '#ced4da');
    }
    if ($("#txtHora").val() == "") {
        $("#txtHora").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHora").css('borderColor', '#ced4da');
    }

    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }

    if ($("#selectTurno").val() == "") {
        $("#selectTurno").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectTurno").css('borderColor', '#ced4da');
    }
    if ($("#selectCondicion").val() == "") {
        //$("#SelectTextura").css('borderColor', '#FA8072');
        $("#selectCondicion").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #FA8072');
        });
        valida = false;
    } else {
        $("#selectCondicion").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #ced4da');
        });
    }

    //if ($("#selectCondicion").val() == "") {
    //    $("#selectCondicion").css('borderColor', '#FA8072');
    //    valida = false;
    //} else {
    //    $("#selectCondicion").css('borderColor', '#ced4da');
    //}
    return valida;
}


function GuardarControl() {
    if (!Validar()) {
        return;
    }
    
    $.ajax({
        url: "../CondicionPersonal/CondicionPersonal",
        type: "POST",
        data: {
            IdCondicionPersonal: $("#txtIdCondicionPersonal").val(),
            IdCondicionPersonalControl: Datos.IdCondicionPersonalControl,
            Fecha: $("#txtFecha").val(),
            Turno: $("#selectTurno").val(),
            Hora: $("#txtHora").val(),
            Cedula: $("#Cedula").val(),
            CodCondicion: $("#selectCondicion").val(),
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } else if (resultado == "1") {
                MensajeAdvertencia(Mensajes.ControlAprobado);
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
            }
            else {
                MensajeCorrecto(resultado);
                NuevoControl();
                ConsultarControl();
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });

    //alert("generado");
}



function InactivarControl() {
    $.ajax({
        url: "../CondicionPersonal/EliminarCondicionPersonal",
        type: "POST",
        data: {
            IdCondicionPersonal: $("#txtIdCondicionPersonal").val(),
              Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == 1) {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
                return;
            } 
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            ConsultarControl();
            NuevoControl();
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function EliminarControl() {
  //  $("#txtEliminarDetalle").val($("#txtIdCondicionPersonal").val());
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    if ($("#txtIdCondicionPersonal").val() > 0) {
        $("#modalEliminarControlDetalle").modal('show');
    } else {
        MensajeAdvertencia("Seleccione un control.");
    }
}

$("#modal-detalle-si").on("click", function () {
    InactivarControl();
    $("#modalEliminarControlDetalle").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});
