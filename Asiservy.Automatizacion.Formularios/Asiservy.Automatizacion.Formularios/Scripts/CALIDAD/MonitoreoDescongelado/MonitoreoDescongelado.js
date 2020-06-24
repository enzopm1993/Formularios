var DatosCabecera = [];
var model = [];
$(document).ready(function () {
    ConsultarMonitoreoDescongelado();
  //  $("#txtTemperaturaAgua").mask("99?.9");
    //$("#txtMuestra1").mask("99?.9");
    //$("#txtMuestra2").mask("99?.9");
   // $("#txtMuestra3").mask("99?.9");
    $.mask.definitions['~'] = '[+-]';
    $("#txtTemperaturaAgua").mask("~99?.9");
    $.mask.definitions['~'] = '[+-]';
    $("#txtMuestra1").mask("~99?.9");
    $.mask.definitions['~'] = '[+-]';
    $("#txtMuestra2").mask("~99?.9");
    $.mask.definitions['~'] = '[+-]';
    $("#txtMuestra3").mask("~99?.9");
});

function ValidaEstadoReporte(Fecha){
    $.ajax({
        url: "../MonitoreoDescongelado/ValidaEstadoReporte",
        type: "GET",
        data: {
            Fecha: Fecha,
            Turno: $("#selectTurno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
           // console.log(resultado);
           // console.log(resultado == 0);
            if (resultado == 0) {
                $("#lblAprobadoPendiente").html("");

            } else if (resultado==1) {
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

function ConsultarMonitoreoDescongelado() {
    $("#chartCabecera2").html('');
    if ($("#txtFecha").val() == '' || $("#selectTurno").val()=='') {
        $("#divCabecera2").prop("hidden", true);
        return;
    }
    if (moment($("#txtFecha").val()).format("YYYY-MM-DD") > moment().format("YYYY-MM-DD")) {
        $("#txtFecha").val("");
        MensajeAdvertencia("Fecha no permitida");
        return;
    }
    ValidaEstadoReporte($("#txtFecha").val());
    $("#divCabecera2").prop("hidden", false);
    $("#spinnerCargando").prop("hidden", false);
  //  ConsultarPeliduvios();
    $.ajax({
        url: "../MonitoreoDescongelado/MonitoreoDescongeladoPartial",
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
                $("#chartCabecera2").html('<div class="text-center"><h4 class="text-warning">' + Mensajes.SinRegistros + '</h4></div>');
                $("#spinnerCargando").prop("hidden", true);
            } else {
                $("#spinnerCargando").prop("hidden", true);
                $("#chartCabecera2").html(resultado);
                config.opcionesDT.pageLength = -1;
                config.opcionesDT.order = [[2, "asc"]];;

                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function nuevoControl(){
    $("#txtHora").css('borderColor', '#ced4da');
    $("#txtTemperaturaAgua").css('borderColor', '#ced4da');
    $("#txtMuestra1").css('borderColor', '#ced4da');
    $("#txtMuestra2").css('borderColor', '#ced4da');
    $("#txtMuestra3").css('borderColor', '#ced4da');

}

function SeleccionarControl(model) {
    nuevoControl();
    DatosCabecera = model;
    //console.log(DatosCabecera);
    $("#txtTanque").val(DatosCabecera.U_SYP_TANQUE);
    $("#txtLote").val(DatosCabecera.U_SYP_LOTE);
    $("#txtHora").val(moment().format("YYYY-MM-DDTHH:mm"));    
    $("#txtTipo").val($("#selectTipo option:selected").text());
    if ($("#selectTipo").val() == "C") {
        $("#divTemperaturaAgua").prop("hidden", true);
    } else {
        $("#divTemperaturaAgua").prop("hidden", false);
    }
    if ($("#selectTurno").val() == "") {
        $("#selectTurno").css('borderColor', '#FA8072');
        return;
    } else {
        $("#selectTurno").css('borderColor', '#ced4da');
    }

   
    ConsultarMonitoreoDetalle();
    
}

function ConsultarMonitoreoDetalle() {
    $.ajax({
        url: "../MonitoreoDescongelado/MonitoreoDescongeladoDetallePartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val(),
            Turno: $("#selectTurno").val(),
            Tanque: DatosCabecera.U_SYP_TANQUE,
            Lote: DatosCabecera.U_SYP_LOTE,
            Tipo: $("#selectTipo").val()
        },
        success: function (resultado) {
            //console.log(resultado);
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#txtIdControl").val(0);
                $("#txtHora").val(moment().format("YYYY-MM-DDTHH:mm"));
                $("#txtTemperaturaAgua").val('');
                $("#txtMuestra1").val('');
                $("#txtMuestra2").val('');
                $("#txtMuestra3").val('');
                $("#ModalMonitoreo").modal("show");
                $("#btnEliminar").prop("hidden", true);
                model = [];
            } else {
                $("#txtHora").val(moment(resultado.Hora).format("YYYY-MM-DDTHH:mm"));
                $("#txtTemperaturaAgua").val(resultado.TemperaturaAgua);
                $("#txtMuestra1").val(resultado.Muestra1);
                $("#txtMuestra2").val(resultado.Muestra2);
                $("#txtMuestra3").val(resultado.Muestra3);
                $("#txtIdControl").val(resultado.IdMonitoreoDescongelado);
                $("#ModalMonitoreo").modal("show");
                $("#btnEliminar").prop("hidden", false);
                model = resultado;
            }
            $("#btnGuardarMonitoreo").prop("disabled", false);
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            //console.log(resultado);
            MensajeError("Error: Comuníquese con sistemas", false);         
        }
    });
}


function Validar() {
    var valida = true;
    if ($("#txtHora").val() == "") {
        $("#txtHora").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHora").css('borderColor', '#ced4da');
    }
    if ($("#selectTipo").val() != 'C') {
        if ($("#txtTemperaturaAgua").val() == "") {
            $("#txtTemperaturaAgua").css('borderColor', '#FA8072');
            valida = false;
        } else {
            $("#txtTemperaturaAgua").css('borderColor', '#ced4da');
        }
    }
    if ($("#txtMuestra1").val() == "") {
        $("#txtMuestra1").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtMuestra1").css('borderColor', '#ced4da');
    }

    if ($("#txtMuestra2").val() == "") {
        $("#txtMuestra2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtMuestra2").css('borderColor', '#ced4da');
    }

    if ($("#txtMuestra3").val() == "") {
        $("#txtMuestra3").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtMuestra3").css('borderColor', '#ced4da');
    }   
    return valida;
}

function GuardarMonitoreoDescongelado() {
    if (!Validar()) {
        return;
    }

    $.ajax({
        url: "../MonitoreoDescongelado/MonitoreoDescongelado",
        type: "POST",
        data: {
            IdMonitoreoDescongelado: $("#txtIdControl").val(),
            Fecha: $("#txtFecha").val(),
            Turno: $("#selectTurno").val(),
            Tanque: DatosCabecera.U_SYP_TANQUE,
            Lote: DatosCabecera.U_SYP_LOTE,
            Especie: DatosCabecera.U_SYP_ESPECIE,
            Talla: DatosCabecera.U_SYP_TALLA,
            Hora: $("#txtHora").val(),
            Tipo: $("#selectTipo").val(),
            TemperaturaAgua: $("#txtTemperaturaAgua").val(),
            Muestra1: $("#txtMuestra1").val(),
            Muestra2: $("#txtMuestra2").val(),
            Muestra3: $("#txtMuestra3").val()

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
            } else {
                MensajeCorrecto(resultado);    
                ConsultarMonitoreoDescongelado();
            }
            $("#ModalMonitoreo").modal("hide");
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}


function EliminarControl() {
    $("#ModalMonitoreo").modal("hide");
    $("#modalEliminarControl").modal("show");
   // console.log(model);
    //$("#txtModalTipo").val(model.Tipo=='D'?'Descongelado':model.tipo=='E'?'Emparrillado':'Ingreso a Cocina');
    if (model.Tipo == 'D') {
        $("#txtModalTipo").val('Descongelado');
    } else if (model.Tipo=='E') {
        $("#txtModalTipo").val('Emparrillado');

    } else {
        $("#txtModalTipo").val('Ingreso a Cocina');

    }

    $("#txtModalTanque").val(DatosCabecera.U_SYP_TANQUE);
    $("#txtModalLote").val(DatosCabecera.U_SYP_LOTE);
    $("#txtModalHora").val(moment(model.Hora).format("HH:mm"));
}


$("#modal-detalle-si").on("click", function () {
    InactivarControl();
    $("#modalEliminarControl").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControl").modal('hide');
});


function InactivarControl() {
    $.ajax({
        url: "../MonitoreoDescongelado/EliminarMonitoreoDescongelado",
        type: "POST",
        data: {
            IdMonitoreoDescongelado: $("#txtIdControl").val(),
            Fecha: $("#txtFecha").val(),
            Turno: $("#selectTurno").val()
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
            if (resultado == 0) {
                MensajeAdvertencia("Faltan Parametros");
            } else {
                MensajeCorrecto(resultado);
            }
            ConsultarMonitoreoDescongelado();
            NuevoControl();
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}