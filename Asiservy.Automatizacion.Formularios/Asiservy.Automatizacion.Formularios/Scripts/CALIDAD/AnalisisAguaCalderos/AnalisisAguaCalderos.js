﻿model = [];
$(document).ready(function () {
   ValidaEstadoReporte($("#txtFecha").val());
});


function ValidaEstadoReporte(Fecha) {
    $.ajax({
        url: "../AnalisisAguaCalderos/ValidaEstadoReporte",
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
                $("#h4Mensaje").html(Mensajes.SinRegistros);
               

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
    model = [];
    ValidaEstadoReporte($("#txtFecha").val());
    MostrarModalCargando();
    $("#h4Mensaje").html("");
    $.ajax({
        url: "../AnalisisAguaCalderos/AnalisisAguaCalderosPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val(),
            IdEquipo: $("#selectEquipo").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#divCabecera2").prop("hidden", false);
            //console.log(resultado);
            if (resultado == "0") {
                parametros.forEach(function (x) {
                    $("#txt-" + x).val("");
                    $("#txt-" + x).prop("disabled", false);
                });
                $("#btnGenerar").prop("hidden", false);
                $("#btnEditar").prop("hidden", true);
                $("#btnEliminar").prop("hidden", true);
            } else {
                model = resultado;
                parametros.forEach(function (x) {
                    $("#txt-" + x).val("");
                    $("#txt-" + x).prop("disabled", true);
                    resultado.forEach(function (y) {
                        if (x = y.IdParametro) {
                            $("#txt-" + x).val(y.Valor);
                        }
                    });
                });

                //console.log(resultado);
                $("#btnGenerar").prop("hidden", true);
                $("#btnEditar").prop("hidden", false);
                $("#btnEliminar").prop("hidden", false);


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

function EditarControl() {
    parametros.forEach(function (x) {
        //data here
        $("#txt-" + x).prop("disabled", false);
        //console.log(object);
    });
    $("#btnGenerar").prop("hidden", false);
    $("#btnEditar").prop("hidden", true);
    $("#btnEliminar").prop("hidden", false);
}


function Validar() {
    var valida = true;

    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }

    if ($("#selectEquipo").val() == "") {
        $("#selectEquipo").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectEquipo").css('borderColor', '#ced4da');
    }

    var contador = 0;
    parametros.forEach(function (x) {
        if ($("#txt-" + x).val() > 9999.9999) {
            $("#txt-" + x).css('borderColor', '#FA8072');
            valida = false;
        } else {
            $("#txt-" + x).css('borderColor', '#ced4da');
        }

        if ($("#txt-" + x).val() != "") {
            contador += 1;
        }


    });
    if (contador == 0) {
        MensajeAdvertencia("Ingrese al menos un componente.");
        valida = false;
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

    var formdata = new FormData();
    formdata.append("Fecha", $("#txtFecha").val());

    var obj = [];
    parametros.forEach(function (x) {
        if ($("#txt-" + x).val() > 0) {
            obj.push({ IdParametro: x, Valor: $("#txt-" + x).val(), IdEquipo: $("#selectEquipo").val()});
        }
    });

    formdata.append("detalle", obj);
     //console.log(obj);
     //return;
    $.ajax({
        url: "../AnalisisAguaCalderos/AnalisisAguaCalderos",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),
            Detalle: obj,
           

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
            } else {
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



function InactivarControl() {
    console.log(model);
    $.ajax({
        url: "../AnalisisAguaCalderos/EliminarAnalisisAguaCalderos",
        type: "POST",
        data: {
            IdAnalisisAguaCalderos: model[0].IdAnalisisAguaCalderos,
            IdEquipo: $("#selectEquipo").val(),
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
            } else {
                ConsultarControl();
            }
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}

function EliminarControl() {
    $("#modalEliminarControlDetalle").modal('show');

}

$("#modal-detalle-si").on("click", function () {
    InactivarControl();
    $("#modalEliminarControlDetalle").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});