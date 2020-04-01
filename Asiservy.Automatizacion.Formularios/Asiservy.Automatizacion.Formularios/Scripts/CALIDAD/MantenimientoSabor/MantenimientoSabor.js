
$(document).ready(function () {
    ConsultarReporte();
});

function ConsultarReporte() {
    $("#chartCabecera2").html('');
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../MantenimientoSabor/MantenimientoSaborPartial",
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
        $("#SelectTipoLimpieza").css('borderCSabor', '#FA8072');
        return;
    } else {
        $("#SelectTipoLimpieza").css('borderCSabor', '#ced4da');
    }

    var estado = 'A';
    if (!$("#CheckEstadoRegistro").prop("checked")) {
        estado = 'I'
    }
    //  alert($("#CheckEstadoRegistro").prop("checked"));
    $.ajax({
        url: "../MantenimientoSabor/MantenimientoSabor",
        type: "POST",
        data: {
            IdSabor: $("#txtIdControl").val(),
            Descripcion: $("#txtDescripcion").val(),
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
    $("#CheckEstadoRegistro").prop("checked", true);
    $('#LabelEstado').text('Activo');
}

function SeleccionarControl(model) {
    $("#txtIdControl").val(model.IdSabor);
    $("#txtDescripcion").val(model.Descripcion);
    if (model.EstadoRegistro == 'A') {
        $("#CheckEstadoRegistro").prop("checked", true);
        $('#LabelEstado').text('Activo');
    } else {
        $("#CheckEstadoRegistro").prop("checked", false);
        $('#LabelEstado').text('Inactivo');
    }
    //console.log(model);
}