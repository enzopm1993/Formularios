model = [];
modelEliminar = [];
modelEditar = [];
$(document).ready(function () {
    ValidaEstadoReporte($("#txtFecha").val());
    $("#txtFechaOrden").val($("#txtFecha").val());
    CargarOrdenFabricacion();
});


function ValidaEstadoReporte(Fecha) {
    $.ajax({
        url: "../AnalisisSensorial/ValidaEstadoReporte",
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
    $("#txtFechaOrden").val($("#txtFecha").val());
    model = [];
    ValidaEstadoReporte($("#txtFecha").val());
    MostrarModalCargando();
  
    $("#h4Mensaje").html("");
    $("#tblPartial").html("");
    $.ajax({
        url: "../AnalisisSensorial/ProtocoloMateriaPrimaPartial",
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
            if (resultado == "0") {
                $("#h4Mensaje").html(Mensajes.SinRegistros);
            } else {
                $("#tblPartial").html(resultado);
            }
            CerrarModalCargando();
            nuevoControl();

        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            CerrarModalCargando();
        }
    });
}

function MostrarModal() {
    $("#modalControl").modal("show");
}

function nuevoControl() {
    $("#selectParametro").prop("selectedIndex", 0).change();
    $("#txtValor").val('');
}


function EditarControl() {

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
        $("#selectEquipo").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #FA8072');
        });

        valida = false;
    } else {
        $("#selectEquipo").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #ced4da');
        });
    }

    if ($("#selectParametro").val() == "") {

        $("#selectParametro").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #FA8072');
        });

        valida = false;
    } else {
        $("#selectParametro").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #ced4da');
        });

    }

    if ($("#txtValor").val() == "") {
        $("#txtValor").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtValor").css('borderColor', '#ced4da');
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

    $.ajax({
        url: "../AnalisisSensorial/ProtocoloMateriaPrima",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),
            IdParametro: $("#selectParametro").val(),
            IdEquipo: $("#selectEquipo").val(),
            Valor: parseFloat($("#txtValor").val()).toFixed(2)
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            }
            if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
                nuevoControl();
            } else {
                ConsultarControl();
            }
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });

    //alert("generado");
}


function EditarControl(model) {
    $("#modalEditarControl").modal('show');
    $("#txtParametro").val(model.Parametro);
    $("#txtEquipo").val(model.Equipo);
    $("#txtValorModal").val(model.Valor);
    modelEditar = model;
}

function ValidaEditar() {
    var valida = true;
    if ($("#txtValorModal").val() == "") {
        $("#txtValorModal").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtValorModal").css('borderColor', '#ced4da');
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
        url: "../AnalisisSensorial/AnalisisSensorial",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),
            IdParametro: modelEditar.IdParametro,
            IdEquipo: modelEditar.IdEquipo,
            Valor: $("#txtValorModal").val()
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
    console.log(model);
    $.ajax({
        url: "../AnalisisSensorial/EliminarAnalisisSensorial",
        type: "POST",
        data: {
            IdAnalisisSensorialDetalle: modelEliminar.IdAnalisisSensorialDetalle,
            IdAnalisisSensorial: modelEliminar.IdAnalisisSensorial,
            IdEquipo: modelEliminar.IdEquipo,
            IdParametro: modelEliminar.IdParametro,
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            } else if (resultado == "1") {
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

function EliminarControl(model) {
    $("#modalEliminarControlDetalle").modal('show');
    modelEliminar = model;
}

$("#modal-detalle-si").on("click", function () {
    InactivarControl();
    $("#modalEliminarControlDetalle").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});






$("#btnOrden").on("click", function () {
    $("#ModalOrdenes").modal('show');
});


$("#modal-orden-si").on("click", function () {
    if ($("#SelectOrdenFabricacion").val() == '') {
        $('#validaOrden').prop("hidden", false);
        return;
    }
    $("#txtOrdenFabricacion").val($("#SelectOrdenFabricacion").val());
    $("#ModalOrdenes").modal('hide');
    $('#validaOrden').prop("hidden", true);
    ConsultaLotes();
});

$("#modal-orden-no").on("click", function () {
    $("#ModalOrdenes").modal('hide');
});

function CargarOrdenFabricacion() {
    valor = $("#txtFechaOrden").val();
    if (valor == '' || valor == null)
        return;
    $("#SelectOrdenFabricacion").empty();
    $("#SelectOrdenFabricacion").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../AnalisisSensorial/ConsultarOrdenesFabricacion",
        type: "GET",
        data: {
            Fecha: valor
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#SelectOrdenFabricacion").empty();
                $("#SelectOrdenFabricacion").append("<option value='' >-- Error de servicio--</option>");
                return;
            }
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectOrdenFabricacion").append("<option value='" + row.Orden + "'>" + row.Orden + "</option>")
                });
                $('#validaFecha').prop("hidden", true);

            } else {
                $('#validaFecha').prop("hidden", false);
            }
            //CargarControlHueso();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}




function ConsultaLotes() {
    valor = $("#SelectOrdenFabricacion").val();
    if (valor == '' || valor == null)
        return;
    $("#selectLote").empty();
    $("#selectLote").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../AnalisisSensorial/ConsultaLotes",
        type: "GET",
        data: {
            Of: valor
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#selectLote").empty();
                $("#selectLote").append("<option value='' >-- Error de servicio--</option>");
                return;
            }
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#selectLote").append("<option value='" + row.Lote + "'>" + row.Lote + "</option>")
                });
            } 
            
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}
