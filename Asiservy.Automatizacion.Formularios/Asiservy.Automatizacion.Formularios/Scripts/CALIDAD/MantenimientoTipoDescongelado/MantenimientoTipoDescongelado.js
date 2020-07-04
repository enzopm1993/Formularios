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
        url: "../MantenimientoTipoDescongelado/MantenimientoTipoDescongeladoPartial",
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
            //console.log(resultado);
            MensajeError("Error: Comuníquese con sistemas", false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}


function Validar() {
    var bool = true;

    //if ($("#txtIdControl").val() == "") {
    //    $("#txtIdControl").css('borderColor', '#FA8072');
    //    bool=false;
    //} else {
    //    $("#txtIdControl").css('borderColor', '#ced4da');
    //}

    if ($("#txtDescripcion").val() == "") {
        $("#txtDescripcion").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtDescripcion").css('borderColor', '#ced4da');
    }

    if ($("#txtAbreviatura").val() == "") {
        $("#txtAbreviatura").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtAbreviatura").css('borderColor', '#ced4da');
    }

    if ($("#selectColor1").val() == "") {
        $("#selectColor1").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#selectColor1").css('borderColor', '#ced4da');
    }

    return bool;
}

function GuardarModificarControl() {
    if (!Validar()) { return;}

    var estado = 'A';
    MostrarModalCargando();
    $.ajax({
        url: "../MantenimientoTipoDescongelado/MantenimientoTipoDescongelado",
        type: "POST",
        data: {
            IdTipoMonitoreo: $("#txtIdControl").val(),
            Descripcion: $("#txtDescripcion").val(),
            Abreviatura: $("#txtAbreviatura").val(),
            Color: $("#selectColor1").val(),
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
    $("#txtIdControl").val('');
    $("#txtDescripcion").val('');
    $("#txtAbreviatura").val('');
    $("#CheckEstadoRegistro").prop("checked", true);
    $('#LabelEstado').text('Activo');
  //  $("#txtIdControl").prop("disabled", false);
    $("#selectColor1").prop("selectedIndex", 0);
    $("#selectColor1").css("color", 'black');

}


function ActualizarCabecera(model) {
    $("#txtIdControl").val(model.IdTipoMonitoreo);
  //  $("#txtIdControl").prop("disabled",true);
    $("#txtDescripcion").val(model.Descripcion);
    $("#txtAbreviatura").val(model.Abreviatura)
    $("#selectColor1").val(model.Color)
    $("#selectColor1").css("color", model.Color);
    
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
        url: "../MantenimientoTipoDescongelado/EliminarMantenimientoTipoDescongelado",
        type: "POST",
        data: {
            IdTipoMonitoreo: itemEditar.IdTipoMonitoreo,
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



function CambioColor(id) {
    //console.log($("#" + id).val());
    if ($("#" + id).val() != "") {
        $("#" + id).css("color", $("#" + id).val());
    } else {
        $("#" + id).css("color", 'black');
    }
}