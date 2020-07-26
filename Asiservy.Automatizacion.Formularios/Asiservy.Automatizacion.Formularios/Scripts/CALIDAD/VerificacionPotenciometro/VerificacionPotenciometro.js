$(document).ready(function () {
    ConsultarControl();
    //$("#txtNaCI1").mask("9?.99");
    //$("#txtNaCI2").mask("9?.99");
    //$("#txtNaCI3").mask("9?.99");
    
    $('#txtSerie').inputmask({
        'alias': 'integer',
        'groupSeparator': '',
        'autoGroup': true,
        'digitsOptional': true,
        'max': '2000000000'
    });
    $('#txtNaCI1').inputmask({
        'alias': 'decimal',
        'groupSeparator': '',
        'digits': 2,
        'autoGroup': true,
        'digitsOptional': true,
        'max': '10.00'
    });

    $('#txtNaCI2').inputmask({
        'alias': 'decimal',
        'groupSeparator': '',
        'digits': 2,
        'autoGroup': true,
        'digitsOptional': true,
        'max': '10.00'
    });

    $('#txtNaCI3').inputmask({
        'alias': 'decimal',
        'groupSeparator': '',
        'digits': 2,
        'autoGroup': true,
        'digitsOptional': true,
        'max': '10.00'
    });


    $('#txtSerieModal').inputmask({
        'alias': 'integer',
        'groupSeparator': '',
        'autoGroup': true,
        'digitsOptional': true,
        'max': '2000000000'
    });
    $('#txtNaCI1Modal').inputmask({
        'alias': 'decimal',
        'groupSeparator': '',
        'digits': 2,
        'autoGroup': true,
        'digitsOptional': true,
        'max': '10.00'
    });

    $('#txtNaCI2Modal').inputmask({
        'alias': 'decimal',
        'groupSeparator': '',
        'digits': 2,
        'autoGroup': true,
        'digitsOptional': true,
        'max': '10.00'
    });

    $('#txtNaCI3Modal').inputmask({
        'alias': 'decimal',
        'groupSeparator': '',
        'digits': 2,
        'autoGroup': true,
        'digitsOptional': true,
        'max': '10.00'
    });
});


function ValidaEstadoReporte(Fecha) {
    $.ajax({
        url: "../VerificacionPotenciometro/ValidaEstadoReporte",
        type: "GET",
        data: {
            Fecha: Fecha
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
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
    if ($("#txtFecha").val() == '') {
        return;
    }
    ValidaEstadoReporte($("#txtFecha").val());
    MostrarModalCargando();
    $("#h4Mensaje").html("" );
    $.ajax({
        url: "../VerificacionPotenciometro/VerificacionPotenciometroPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#divCabecera2").prop("hidden", false);
            if (resultado == "0") {
                $("#h4Mensaje").html(Mensajes.SinRegistros);
                $("#btnGenerar").prop("hidden", false);
                $("#btnEditar").prop("hidden", true);
                $("#btnEliminar").prop("hidden", true);
                $("#txtModelo").val("");
                $("#txtSerie").val("");
                $("#txtNaCI1").val("");
                $("#txtNaCI2").val("");
                $("#txtNaCI3").val("");
                $("#txtCodigo").val("");
                $("#txtObservacion").val(resultado[0].Observacion);
                $("#divTable").html('');

            } else {
                $("#divTable").html(resultado);
            }
            CerrarModalCargando();
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            CerrarModalCargando();
        }
    });
}


function Validar() {
    var valida = true;

    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }

    if ($("#txtModelo").val() == "") {
        $("#txtModelo").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtModelo").css('borderColor', '#ced4da');
    }
    if ($("#txtSerie").val() == "") {
        $("#txtSerie").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtSerie").css('borderColor', '#ced4da');
    }

    if ($("#txtNaCI1").val() == "") {
        $("#txtNaCI1").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtNaCI1").css('borderColor', '#ced4da');
    }

    if ($("#txtNaCI2").val() == "") {
        $("#txtNaCI2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtNaCI2").css('borderColor', '#ced4da');
    }
    if ($("#txtNaCI3").val() == "") {
        $("#txtNaCI3").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtNaCI3").css('borderColor', '#ced4da');
    }
    if ($("#txtCodigo").val() == "") {
        $("#txtCodigo").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtCodigo").css('borderColor', '#ced4da');
    }
    return valida;
}


function GuardarControl() {
    if (!Validar()) {
        return;
    }
    if (moment($("#txtFecha").val()).format("YYYY-MM-DD") > moment().format("YYYY-MM-DD")) {
        $("#txtFecha").val("");
        MensajeAdvertencia("Fecha no permitida");
        return;
    }
    $.ajax({
        url: "../VerificacionPotenciometro/VerificacionPotenciometro",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),
            Modelo: $("#txtModelo").val(),
            Serie: $("#txtSerie").val(),
            NaCI1: $("#txtNaCI1").val(),
            NaCI2: $("#txtNaCI2").val(),
            NaCI3: $("#txtNaCI3").val(),
            Codigo: $("#txtCodigo").val(),
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
            }
            else {
                ConsultarControl();
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });

    //alert("generado");
}


function EditarControl(model) {
    $("#modalEditarControl").modal('show');
    $("#txtModeloModal").val(model.Modelo);
    $("#txtSerieModal").val(model.Serie);
    $("#txtNaCI1Modal").val(model.NaCI1);
    $("#txtNaCI2Modal").val(model.NaCI2);
    $("#txtNaCI3Modal").val(model.NaCI3);
    $("#txtCodigoModal").val(model.Codigo);
    $("#txtObservacionModal").val(model.Observacion);
    modelEditar = model;
}

function ValidaEditar() {
    var valida = true;
    if ($("#txtModeloModal").val() == "") {
        $("#txtModeloModal").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtModeloModal").css('borderColor', '#ced4da');
    }
    if ($("#txtSerieModal").val() == "") {
        $("#txtSerieModal").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtSerieModal").css('borderColor', '#ced4da');
    }

    if ($("#txtNaCI1Modal").val() == "") {
        $("#txtNaCI1Modal").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtNaCI1Modal").css('borderColor', '#ced4da');
    }

    if ($("#txtNaCI2Modal").val() == "") {
        $("#txtNaCI2Modal").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtNaCI2Modal").css('borderColor', '#ced4da');
    }
    if ($("#txtNaCI3Modal").val() == "") {
        $("#txtNaCI3Modal").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtNaCI3Modal").css('borderColor', '#ced4da');
    }
    if ($("#txtCodigoModal").val() == "") {
        $("#txtCodigoModal").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtCodigoModal").css('borderColor', '#ced4da');
    }
    return valida;
}


function ModificarControl() {
    if (!ValidaEditar()) {
        return;
    }
    if (moment($("#txtFecha").val()).format("YYYY-MM-DD") > moment().format("YYYY-MM-DD")) {
        $("#txtFecha").val("");
        MensajeAdvertencia("Fecha no permitida");
        return;
    }

    $.ajax({
        url: "../VerificacionPotenciometro/VerificacionPotenciometro",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),
            IdVerificacionPotenciometroControl: modelEditar.IdVerificacionPotenciometroControl,
            Modelo: $("#txtModeloModal").val(),
            Serie: $("#txtSerieModal").val(),
            NaCI1: $("#txtNaCI1Modal").val(),
            NaCI2: $("#txtNaCI2Modal").val(),
            NaCI3: $("#txtNaCI3Modal").val(),
            Codigo: $("#txtCodigoModal").val(),
            Observacion: $("#txtObservacionModal").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } else if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
            } else {
                ConsultarControl();
            }
            $("#modalEditarControl").modal('hide');

        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });

    //alert("generado");
}




function InactivarControl() {
    $.ajax({
        url: "../VerificacionPotenciometro/EliminarVerificacionPotenciometro",
        type: "POST",
        data: {
            IdVerificacionPotenciometroControl: modelEditar.IdVerificacionPotenciometroControl
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            } if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
            } else {
                ConsultarControl();
                NuevoControl();
            }
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}

function EliminarControl(model) {
    //  $("#txtEliminarDetalle").val($("#txtIdVerificacionPotenciometro").val());
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    modelEditar = model;
    $("#modalEliminarControlDetalle").modal('show');

}

$("#modal-detalle-si").on("click", function () {
    InactivarControl();
    $("#modalEliminarControlDetalle").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});
