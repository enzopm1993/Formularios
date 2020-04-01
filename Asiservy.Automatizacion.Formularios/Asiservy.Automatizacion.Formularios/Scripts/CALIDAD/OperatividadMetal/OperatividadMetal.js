$(document).ready(function () {
    ConsultarControl();

});


function ConsultarControl() {
    $("#divMensaje").html('');
    $("#divDetalle").prop("hidden", true);
    $("#btnGenerar").prop("hidden", false);
    $("#btnEditar").prop("hidden", true);
    $("#btnEliminar").prop("hidden", true);

    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }
    $.ajax({
        url: "../OperatividadMetal/OperatividadMetalPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {               
                $("#txtPcc").val('');
                $("#txtIdControl").val('0');
                $("#chkLomo").prop("checked", false);
                $("#chkLata").prop("checked", false);
                $("#txtFerroso").val('');
                $("#txtNoFerroso").val('');
                $("#txtAceroInoxidable").val('');
                $("#txtCodDetectorMetal").val('');
                $("#txtObservacion").val('');
                $("#divMensaje").html('NO SE HA GENERADO EL CONTROL');
            } else {
                //$("#txtPcc").prop("disabled", true);
                //$("#txtCodDetectorMetal").prop("disabled", true);
                //$("#chkLomo").prop("disabled", true);
                //$("#chkLata").prop("disabled", true);
                $("#divDetalle").prop("hidden", false);
                $("#btnEditar").prop("hidden", false);
                $("#btnEliminar").prop("hidden", false);
                $("#btnGenerar").prop("hidden", true);
                //console.log(resultado);
                $("#txtPcc").val(resultado.Pcc);
                $("#txtIdControl").val(resultado.IdOperatividadMetal);
                $("#chkLomo").prop("checked",resultado.Lomos);
                $("#chkLata").prop("checked",resultado.Latas);
                $("#txtFerroso").val(resultado.Ferroso);
                $("#txtNoFerroso").val(resultado.NoFerroso);
                $("#txtAceroInoxidable").val(resultado.AceroInoxidable);
                $("#txtCodDetectorMetal").val(resultado.DetectorMetal);
                $("#txtObservacion").val(resultado.Observacion);
                CargarControlDetalle();
              //  console.log(resultado);

            }
               
        },
            error: function (resultado) {
                MensajeError("Error: Comuníquese con sistemas," + resultado, false);
        }
    });
}

function AbrirModal() {
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
        $("#ModalCabecera").modal("show");

    }
}



function Validar() {
    var valida = true;
    if ($("#txtPcc").val() == "") {
        $("#txtPcc").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtPcc").css('borderColor', '#ced4da');
    }
   
    if ($("#txtFerroso").val() == "") {
        $("#txtFerroso").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFerroso").css('borderColor', '#ced4da');
    }

    if ($("#txtNoFerroso").val() == "") {
        $("#txtNoFerroso").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtNoFerroso").css('borderColor', '#ced4da');
    }

    if ($("#txtAceroInoxidable").val() == "") {
        $("#txtAceroInoxidable").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtAceroInoxidable").css('borderColor', '#ced4da');
    }


    if ($("#txtCodDetectorMetal").val() == "") {
        $("#txtCodDetectorMetal").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtCodDetectorMetal").css('borderColor', '#ced4da');
    }
    return valida;
}
function GenerarControl() {
    if (!Validar()) {
        return;
    }
    //alert("ok");
    $.ajax({
        url: "../OperatividadMetal/OperatividadMetal",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),
            IdOperatividadMetal: $("#txtIdControl").val(),
            Pcc: $("#txtPcc").val(),
            Lomos: $("#chkLomo").prop("checked"),
            Latas: $("#chkLata").prop("checked"),
            Ferroso: $("#txtFerroso").val(),
            NoFerroso: $("#txtNoFerroso").val(),
            AceroInoxidable: $("#txtAceroInoxidable").val(),
            DetectorMetal: $("#txtCodDetectorMetal").val(),
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }else {
                $("#ModalCabecera").modal("hide");
                MensajeCorrecto("Registro Exitoso");
                ConsultarControl();
            }

        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas," + resultado, false);
        }
    });
}

function InactivarControl() {
    $.ajax({
        url: "../OperatividadMetal/EliminarOperatividadMetal",
        type: "POST",
        data: {
            IdOperatividadMetal: $("#txtIdControl").val()
        },
        success: function (resultado) {
          //  alert(resultado);
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            ConsultarControl();
            MensajeCorrecto("Control Eliminado con Exito");
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas,"+resultado.responseText, false);
        }
    });
}

function EliminarControl() {
   // $("#txtEliminarDetalle").val(model.IdControlConsumoInsumoDetalle);
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    $("#modalEliminarControlDetalle").modal('show');
}

$("#modal-detalle-si").on("click", function () {
    InactivarControl();
    $("#modalEliminarControlDetalle").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});


////////////////////////////////////////////// --DETALLE-- ////////////////////////////////////////////7

function CargarControlDetalle() {
    $("#spinnerCargandoDetalle").prop("hidden", false);
    $("#divTableDetalle").html('');
    $.ajax({
        url: "../OperatividadMetal/OperatividadMetalDetallePartial",
        type: "GET",
        data: {
            IdControl: $("#txtIdControl").val()
            //  Tipo: $("#txtLineaNegocio").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableDetalle").html("No existen registros");
                $("#spinnerCargandoDetalle").prop("hidden", true);
            } else {
                $("#spinnerCargandoDetalle").prop("hidden", true);
                $("#divTableDetalle").html(resultado);
                //config.opcionesDT.pageLength = 10;
                //      config.opcionesDT.order = [[0, "asc"]];
                //    $('#tblDataTable').DataTable(config.opcionesDT);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}

function ModalGenerarControlDetalle() {
    //$("#txtIdControlConsumoInsumoDetalle").val(0);
    //$("#txtHoraInicioDetalle").val("");
    //$("#txtHoraFinDetalle").val("");
    $("#ModalGenerarControlConsumoInsumoDetalle").modal("show");

}

function ValidarGenerarControlConsumoDetalle() {
    var valida = true;
    if ($("#txtHoraInicioDetalle").val() == "") {
        $("#txtHoraInicioDetalle").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHoraInicioDetalle").css('borderColor', '#ced4da');
    }
    if ($("#txtHoraFinDetalle").val() == "") {
        $("#txtHoraFinDetalle").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHoraFinDetalle").css('borderColor', '#ced4da');
    }

    return valida;
}
function GenerarControlConsumoDetalle() {
    if (!ValidarGenerarControlConsumoDetalle()) {
        return;
    }
    $("#spinnerCargandoDetalle").prop("hidden", false);
    $.ajax({
        url: "../ControlConsumoInsumo/ControlConsumoInsumoDetalle",
        type: "POST",
        data: {
            IdControlConsumoInsumoDetalle: $("#txtIdControlConsumoInsumoDetalle").val(),
            IdControlConsumoInsumos: ListadoControl.IdControlConsumoInsumos,
            HoraInicio: $("#txtHoraInicioDetalle").val(),
            HoraFin: $("#txtHoraFinDetalle").val(),
        },
        success: function (resultado) {
            $("#spinnerCargandoDetalle").prop("hidden", true);
            if (resultado == "101") {
                window.location.reload();
            }
            CargarControlConsumoInsumoDetalle();
            $("#ModalGenerarControlConsumoInsumoDetalle").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}


function EditarConsumoInsumoDetalle(model) {
    // console.log(model);
    $("#txtIdControlConsumoInsumoDetalle").val(model.IdControlConsumoInsumoDetalle);
    $("#txtHoraInicioDetalle").val(model.HoraInicio);
    $("#txtHoraFinDetalle").val(model.HoraFin);
    $("#ModalGenerarControlConsumoInsumoDetalle").modal("show");
    //ModalGenerarControlDetalle();
}

function InactivarControlConsumoDetalle() {
    $.ajax({
        url: "../ControlConsumoInsumo/EliminarControlConsumoInsumoDetalle",
        type: "POST",
        data: {
            IdControlConsumoInsumoDetalle: $("#txtEliminarDetalle").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            CargarControlConsumoInsumoDetalle();
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function EliminarProcesoConsumoInsumoDetalle(model) {
    $("#txtEliminarDetalle").val(model.IdControlConsumoInsumoDetalle);
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    $("#modalEliminarControlDetalle").modal('show');
}

$("#modal-detalle-si").on("click", function () {
    InactivarControlConsumoDetalle();
    $("#modalEliminarControlDetalle").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});
