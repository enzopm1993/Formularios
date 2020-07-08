model = [];
modelEliminar = [];
modelEditar = [];
$(document).ready(function () {
    ValidaEstadoReporte($("#txtFecha").val());
    $("#selectParametro").select2({
        width:'100%'
    });
    $("#selectEquipo").select2({ width:'100%'});

    $('#txtValor').inputmask({
        'alias': 'decimal',
        'groupSeparator': ',',
        'digits': 2,
        'autoGroup': true,
        'digitsOptional': true,
        'max': '10000.00',
        'min':'0'
    });

    $('#txtValorModal').inputmask({
        'alias': 'decimal',
        'groupSeparator': ',',
        'digits': 2,
        'autoGroup': true,
        'digitsOptional': true,
        'max': '10000.00',
        'min': '0'

    });
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

function getParametros() {
    $.ajax({
        type: "GET",
        url: '../AnalisisAguaCalderos/ConsultaParametros',
        data: {
            Fecha: $("#txtFecha").val(),
            IdEquipo: $("#selectEquipo").val()
        },
        dataType: "json",
        success: function (data) {
            $("#selectParametro").empty().change();
            $("#selectParametro").append('<option value=""> Seleccione</option>');
            $.each(data, function (key, registro) {
                $("#selectParametro").append('<option value=' + registro.IdParametro + '>' + registro.Descripcion + '</option>');
            });
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}

function ConsultarControl() {
    if ($("#txtFecha").val() == '' || $("#selectEquipo").val() =='') {
        return;
    }
    model = [];
    ValidaEstadoReporte($("#txtFecha").val());
    MostrarModalCargando();
    getParametros();
    $("#h4Mensaje").html("");
    $("#tblPartial").html("");
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

function nuevoControl() {
    $("#selectParametro").prop("selectedIndex",0).change();
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
        url: "../AnalisisAguaCalderos/AnalisisAguaCalderos",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),
            IdParametro: $("#selectParametro").val(),
            IdEquipo: $("#selectEquipo").val(),
            Valor: $("#txtValor").val()
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
        url: "../AnalisisAguaCalderos/AnalisisAguaCalderos",
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
        url: "../AnalisisAguaCalderos/EliminarAnalisisAguaCalderos",
        type: "POST",
        data: {
            IdAnalisisAguaCalderosDetalle: modelEliminar.IdAnalisisAguaCalderosDetalle,
            IdAnalisisAguaCalderos: modelEliminar.IdAnalisisAguaCalderos,
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
            } else  if (resultado == "1") {
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
