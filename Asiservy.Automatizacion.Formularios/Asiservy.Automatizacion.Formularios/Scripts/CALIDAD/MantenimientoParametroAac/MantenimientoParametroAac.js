﻿var itemEditar = [];
$(document).ready(function () {
    ConsultarReporte();
});

function ConsultarReporte() {
    $("#chartCabecera2").html('');
    $("#spinnerCargandoMantenimiento").prop("hidden", false);
    $.ajax({
        url: "../MantenimientoParametroAac/MantenimientoParametroAacPartial",
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            //$("#divCabecera2").prop("hidden", false);
            if (resultado == "0") {
                $("#chartCabecera2").html("No existen registros");
                $("#spinnerCargandoMantenimiento").prop("hidden", true);
            } else {
                $("#spinnerCargandoMantenimiento").prop("hidden", true);
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
            $("#spinnerCargandoMantenimiento").prop("hidden", true);
        }
    });
}


function GuardarControl() {

    //if ($("#SelectTipoLimpieza").val() == "") {
    //    $("#SelectTipoLimpieza").css('borderColoor', '#FA8072');
    //    return;
    //} else {
    //    $("#SelectTipoLimpieza").css('borderColoor', '#ced4da');
    //}

    if ($("#txtDescripcion").val() == "") {
        $("#txtDescripcion").css('borderColoor', '#FA8072');
        return;
    } else {
        $("#txtDescripcion").css('borderColoor', '#ced4da');
    }

    if ($("#txtAbreviatura").val() == "") {
        $("#txtAbreviatura").css('borderColoor', '#FA8072');
        return;
    } else {
        $("#txtAbreviatura").css('borderColoor', '#ced4da');
    }

    var estado = 'A';
    //if (!$("#CheckEstadoRegistro").prop("checked")) {
    //    estado = 'I'
    //}

    $.ajax({
        url: "../MantenimientoParametroAac/MantenimientoParametroAac",
        type: "POST",
        data: {
            IdParametro: $("#txtIdControl").val(),
            Descripcion: $("#txtDescripcion").val(),
            Abreviatura: $("#txtAbreviatura").val(),
            MaximoPermitido: $('#txtMaximoPermitido').val(),
            EstadoRegistro: estado
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } else {
                NuevoControl();
                ConsultarReporte();
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });

    //alert("generado");
}

function CambioEstado(valor) {
    //  console.log(valor);
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');

}

function NuevoControl() {
    $("#txtIdControl").val('0');
    $("#txtDescripcion").val('');
    $("#txtAbreviatura").val('');
    $("#txtMaximoPermitido").val('');
    
    $("#CheckEstadoRegistro").prop("checked", true);
    $('#LabelEstado').text('Activo');
}


function ActualizarCabecera(model) {
    $("#txtIdControl").val(model.IdParametroAac);
    $("#txtDescripcion").val(model.Descripcion);
    $("#txtAbreviatura").val(model.Abreviatura)
    $("#txtMaximoPermitido").val(model.Abreviatura)
}

function InactivarConfirmar(jdata) {
    $("#modalEliminarControl").modal("show");
    itemEditar = jdata;
    $("#myModalLabel").text("¿Desea inactivar el registro?");
    itemEditar.EstadoRegistro = 'I';
}

function ActivarConfirmar(jdata) {
    $("#modalEliminarControl").modal("show");
    $("#myModalLabel").text("¿Desea activar el registro?");
    itemEditar = jdata;
    itemEditar.EstadoRegistro = 'A';
}


function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}




function EliminarCabeceraSi() {
    MostrarModalCargando();
    $.ajax({
        url: "../MantenimientoParametroAac/EliminarMantenimientoParametroAac",
        type: "POST",
        data: {
            IdParametro: itemEditar.IdParametro,
            EstadoRegistro: itemEditar.EstadoRegistro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                ConsultarReporte();
                MensajeCorrecto("Registro Actualizado con Éxito");
                CerrarModalCargando();
                itemEditar = 0;
            }
        },
        error: function (resultado) {
            CerrarModalCargando();
            MensajeError(resultado.responseText, false);
        }
    });
}