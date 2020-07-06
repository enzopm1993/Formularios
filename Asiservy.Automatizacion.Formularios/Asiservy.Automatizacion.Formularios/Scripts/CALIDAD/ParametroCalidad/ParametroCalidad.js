var itemEditar = [];
$(document).ready(function () {
    ConsultarReporte();
    $('#txtMinimo').inputmask({
        'alias': 'decimal',
        'groupSeparator': ',',
        'digits': 2,
        'autoGroup': true,
        'digitsOptional': true,
        'max': '999.99'
    });
    $('#txtMaximo').inputmask({
        'alias': 'decimal',
        'groupSeparator': ',',
        'digits': 2,
        'autoGroup': true,
        'digitsOptional': true,
        'max': '999.99'
    });
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
        $("#txtDescripcion").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtDescripcion").css('borderColor', '#ced4da');
    }

    if ($("#txtMinimo").val() > $("#txtMaximo").val()) {
        $("#txtMinimo").css('borderColor', '#FA8072');
        $("#txtMaximo").css('borderColor', '#FA8072');
        MensajeAdvertencia("Valor mínimo no debe ser mayor que valor máximo.")
        return;
    } else {
        $("#txtMinimo").css('borderColor', '#ced4da');
        $("#txtMaximo").css('borderColor', '#ced4da');

    }

   

    $.ajax({
        url: "../ParametroCalidad/MantenimientoParametroCalidad",
        type: "POST",
        data: {
            CodParametro: $("#txtIdControl").val(),
            Nombre: $("#txtDescripcion").val(),
            Minimo: $("#txtMinimo").val(),
            Maximo: $("#txtMaximo").val(),
            ColorDentroRango: $("#selectColor1").val(),
            ColorFueraRango: $("#selectColor2").val(),
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
    $("#selectColor2").css("color", 'black');
    //$("#CheckEstadoRegistro").prop("checked", true);
    //$('#LabelEstado').text('Activo');
}


function ActualizarCabecera(model) {
    $("#ModalEditarControl").modal("show");
    $("#txtIdControl").val(model.CodParametro);
    $("#txtDescripcion").val(model.Nombre);
    $("#txtMinimo").val(model.Minimo)
    $("#txtMaximo").val(model.Maximo)
    if (model.ColorDentroRango != "") {
        $("#selectColor1").css("color", model.ColorDentroRango);
        $("#selectColor1").val(model.ColorDentroRango)

    }
    if (model.ColorFueraRango != "") {
        $("#selectColor2").css("color", model.ColorFueraRango);
        $("#selectColor2").val(model.ColorFueraRango)

    }
    $("#txtObservacion").val(model.Observacion)
}


function CambioColor(id) {
    //console.log($("#" + id).val());
    if ($("#" + id).val() != "") {
        $("#" + id).css("color", $("#" + id).val());
    } else {
        $("#" + id).css("color",'black');
    }
}