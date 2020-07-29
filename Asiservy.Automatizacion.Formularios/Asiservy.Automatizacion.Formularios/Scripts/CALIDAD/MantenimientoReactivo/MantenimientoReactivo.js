var itemEditar = [];
$(document).ready(function () {
    ConsultarReporte();
});

function ConsultarReporte() {
    //alert();
   $("#chartCabecera2").html('');
   $("#divCabecera2").prop("hidden", false);
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../MantenimientoReactivo/MantenimientoReactivoPartial",
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#chartCabecera2").html("No existen registros");
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


function GuardarModificarControl() {
    // alert();
    //console.log($("#txtDescripcion").val());
    if ($("#txtDescripcion").val() == "") {
        $("#txtDescripcion").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtDescripcion").css('borderColor', '#ced4da');
    }

    if ($("#txtAbreviatura").val() == "") {
        $("#txtAbreviatura").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtAbreviatura").css('borderColor', '#ced4da');
    }

    var estado = 'A';
    //if (!$("#CheckEstadoRegistro").prop("checked")) {
    //    estado = 'I'
    //}
    MostrarModalCargando();
    $.ajax({
        url: "../MantenimientoReactivo/MantenimientoReactivo",
        type: "POST",
        data: {
            IdReactivo: $("#txtIdControl").val(),
            Descripcion: $("#txtDescripcion").val(),
            Abreviatura: $("#txtAbreviatura").val(),
            ValorPredeterminado: $("#txtValorPredeterminado").val(),
            EstadoRegistro: estado
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CerrarModalCargando();
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
            CerrarModalCargando();

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
    $("#txtValorPredeterminado").val('');
    $("#CheckEstadoRegistro").prop("checked", true);
    $('#LabelEstado').text('Activo');
}


function ActualizarCabecera(model) {
    $("#txtIdControl").val(model.IdReactivo);
    $("#txtDescripcion").val(model.Descripcion);
    $("#txtValorPredeterminado").val(model.ValorPredeterminado);
    $("#txtAbreviatura").val(model.Abreviatura)
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
        url: "../MantenimientoReactivo/EliminarMantenimientoReactivo",
        type: "POST",
        data: {
            IdReactivo: itemEditar.IdReactivo,
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
