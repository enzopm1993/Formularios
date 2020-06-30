var itemEditar = [];
$(document).ready(function () {
    ConsultarReporte();
});

function ConsultarReporte() {
    $("#chartCabecera2").html('');
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../ParametroCalidad/MantenimientoParametroCalidadPartial",
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
       
    if ($("#txtDescripcion").val() == "") {
        $("#txtDescripcion").css('borderCParametroCalidad', '#FA8072');
        return;
    } else {
        $("#txtDescripcion").css('borderCParametroCalidad', '#ced4da');
    }

   

    $.ajax({
        url: "../ParametroCalidad/MantenimientoParametroCalidad",
        type: "POST",
        data: {
            IdParametro: $("#txtIdControl").val(),
            Nombre: $("#txtDescripcion").val(),
            Minimo: $("#txtMinimo").val(),
            Maximo: $("#txtMaximo").val(),
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } else {
                $("#ModalEditarControl").modal("hide");
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

//function CambioEstado(valor) {
//    //  console.log(valor);
//    if (valor)
//        $('#LabelEstado').text('Activo');
//    else
//        $('#LabelEstado').text('Inactivo');

//}

function NuevoControl() {
    $("#txtIdControl").val('0');
    $("#txtDescripcion").val('');
    $("#txtMinimo").val('');
    $("#txtMaximo").val('');
    $("#txtObservacion").val('');
    //$("#CheckEstadoRegistro").prop("checked", true);
    //$('#LabelEstado').text('Activo');
}


function ActualizarCabecera(model) {
    $("#ModalEditarControl").modal("show");
    $("#txtIdControl").val(model.IdParametro);
    $("#txtDescripcion").val(model.Nombre);
    $("#txtMinimo").val(model.Minimo)
    $("#txtMaximo").val(model.Maximo)
    $("#txtObservacion").val(model.Observacion)
}
