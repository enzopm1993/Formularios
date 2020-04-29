var itemEditar = [];
$(document).ready(function () {
    ConsultarEstadoReporte(0);
    CargarCabecera(1);
});

function ConsultarEstadoReporte(op) {
    $('#cargac').show();
    $.ajax({
        url: "../DesechosLiquidosPeligrosos/ConsultarEstadoReporte",
        data: {
            fechaDesde: $("#txtFecha").val(),
            fechaHasta: $("#txtFecha").val(),
            idDesechosLiquidos: 0,
            op: op
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMostarTablaCabecera").html("No existen registros");
            } else {
                if (resultado.EstadoReporte == true) {
                    $("#lblAprobadoPendiente").text("APROBADO");
                    $("#lblAprobadoPendiente").removeClass('badge-danger');
                    $("#lblAprobadoPendiente").addClass('badge badge-success');
                } else {
                    $("#lblAprobadoPendiente").text("PENDIENTE");
                    $("#lblAprobadoPendiente").removeClass('badge-success');
                    $("#lblAprobadoPendiente").addClass('badge badge-danger');
                }
            }
            itemEditar = 0;
            setTimeout(function () {
                $('#cargac').hide();
            }, 200);
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function CargarCabecera(op) {
    $('#cargac').show();
    $.ajax({
        url: "../DesechosLiquidosPeligrosos/DesechosLiquidosPeligrososPartial",
        data: {
            fechaDesde: $("#txtFecha").val(),
            fechaHasta: $("#txtFecha").val(),
            idDesechosLiquidos: 0,
            op: op
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMostarTablaCabecera").html("No existen registros");
            } else {
                $("#divMostarTablaCabecera").html(resultado);
            }
            itemEditar = 0;
            setTimeout(function () {
                $('#cargac').hide();
            }, 200);
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function GuardarCabecera() {
    $('#cargac').show();
    $.ajax({
        url: "../DesechosLiquidosPeligrosos/GuardarModificarDesechosLiquidos",
        type: "POST",
        data: {
            IdDesechosLiquidos: itemEditar.IdDesechosLiquidos,
            IdDesechosLiquidosDetalle: itemEditar.IdDesechosLiquidosDetalle,
            FechaMES: $("#txtFechaCabecera").val(),
            FechaDIA: $("#txtFechaCabecera").val(),
            Laboratorio: $("#txtLaboratorio").val(),
            Observaciones: $("#txtObservacion").val(),
            Otros: $("#txtOtros").val(),
            siAprobar: 0,
            fechaDesde: $("#txtFechaCabecera").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == '2') {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
            }
            if (resultado == '3') {
                MensajeAdvertencia('¡Ya existe un registro con ese DIA: <span class="badge badge-danger">' + moment($("#txtFechaCabecera").val()).format('DD') + '</span>!');
                $('#cargac').hide();
                return;
            }
            $('#ModalIngresoCabecera').modal('hide');
            setTimeout(function () {
                LimpiarCabecera();
                itemEditar = 0;
                $('#cargac').hide();
                CargarCabecera(1);
            }, 200);
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function ActualizarCabecera(jdata) {
    if (jdata.EstadoReporte == true) {
        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        return;
    } else {
        $("#txtFechaCabecera").prop('disabled', true);
        $("#txtFechaCabecera").val(moment(jdata.FechaDIA).format("YYYY-MM-DD"));
        //$("#txtFechaCabecera").val(date[0].defaultValue);
        $("#txtLaboratorio").val(jdata.Laboratorio);
        $("#txtOtros").val(jdata.Otros);
        $("#txtObservacion").val(jdata.Observaciones);
        $('#ModalIngresoCabecera').modal('show');
        itemEditar = jdata;
    }
}

function ModalIngresoCabecera() {
    LimpiarCabecera();
    $("#txtFechaCabecera").prop('disabled', false);
    $('#ModalIngresoCabecera').modal('show');
    $("#txtFechaCabecera").val(moment($("#txtFecha").val()).format("YYYY-MM-DD"));
}

function LimpiarCabecera() {
    $('#txtFechaCabecera').val('');
    $('#txtOtros').val('');
    $('#txtObservacion').val('');
    $('#txtLaboratorio').val('');
    $("#txtFechaCabecera").css('border', '');
    $("#txtOtros").css('border', '');
    $("#txtLaboratorio").css('border', '');
    $("#txtObservacion").css('border', '');
}

function ValidarDatosVacios() {
    var vacio = OnChangeTextBox();
    if (vacio == 1) {
        MensajeAdvertencia('¡Ingrese todo los datos requeridos!');
        return;
    } else GuardarCabecera();
}

function OnChangeTextBox() {  
    var con = 0;
    if ($('#txtFechaCabecera').val() == '') {
        $("#txtFechaCabecera").css('border', '1px dashed red');
        con = 1;
    } else $("#txtFechaCabecera").css('border', '');
    if ($('#txtLaboratorio').val() == '') {
        $("#txtLaboratorio").css('border', '1px dashed red');
        con = 1;
    } else $("#txtLaboratorio").css('border', '');
    return con;
}

function EliminarConfirmar(jdata) {
    if (jdata.EstadoReporte == true) {
        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        return;
    } else {
        $("#modalEliminarControl").modal("show");
        itemEditar = jdata;
        $("#myModalLabel").text("¿Desea Eliminar el registro?");
    }
}

function EliminarCabeceraSi() {
    $('#cargac').show();
    $.ajax({
        url: "../DesechosLiquidosPeligrosos/EliminarDesechosLiquidosDetalle",
        type: "POST",
        data: {
            IdDesechosLiquidosDetalle: itemEditar.IdDesechosLiquidosDetalle,
            fechaDesde: $('#txtFecha').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdDesechosLiquidosDetalle");
                $("#modalEliminarControl").modal("hide");
                $('#cargac').hide();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                CargarCabecera(1);
                MensajeCorrecto("Registro eliminado con Éxito");
                setTimeout(function () {
                    $('#cargac').hide();
                }, 200);
            } else if (resultado == '2') {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                $('#cargac').hide();
                return;
            }
            itemEditar = 0;
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}