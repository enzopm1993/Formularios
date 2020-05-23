
$(document).ready(function () {
    ConsultarReporte();
});

function ConsultarReporte() {
    $("#chartCabecera2").html('');
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../MantenimientoTextura/MantenimientoTexturaPartial",
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#divCabecera2").prop("hidden", false);
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


function GuardarControl() {

    if ($("#SelectTipoLimpieza").val() == "") {
        $("#SelectTipoLimpieza").css('borderCTextura', '#FA8072');
        return;
    } else {
        $("#SelectTipoLimpieza").css('borderCTextura', '#ced4da');
    }
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
    if (!$("#CheckEstadoRegistro").prop("checked")) {
        estado = 'I'
    }
    //  alert($("#CheckEstadoRegistro").prop("checked"));
    $.ajax({
        url: "../MantenimientoTextura/MantenimientoTextura",
        type: "POST",
        data: {
            IdTextura: $("#txtIdControl").val(),
            Descripcion: $("#txtDescripcion").val(),
            Abreviatura: $("#txtAbreviatura").val(),
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
    $("#CheckEstadoRegistro").prop("checked", true);
    $('#LabelEstado').text('Activo');
}

function ActualizarCabecera(model) {
    $("#txtIdControl").val(model.IdTextura);
    $("#txtDescripcion").val(model.Descripcion);
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
        url: "../MantenimientoTextura/EliminarMantenimientoTextura",
        type: "POST",
        data: {
            IdTextura: itemEditar.IdTextura,
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
