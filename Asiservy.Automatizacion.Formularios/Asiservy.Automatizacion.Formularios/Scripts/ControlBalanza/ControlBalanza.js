

$(document).ready(function () {
    CargarControlBalanza();
});


function Limpiar() {
    $("#NombreEmpleado").val('');
    $("#Identificacion").val('');   
    $("#txtCodigo").val('');
    $("#txtObservacion").val('');
}

function CargarControlBalanza() {
    $("#spinnerCargando").prop("hidden", false);
    $("#DivTableControl").html('');
    $.ajax({
        url: "../ControlBalanza/ControlBalanzaPartial",
        type: "GET",
        data: {
            Fecha: $('#txtFecha').val(),
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#DivTableControl").html('');
                $("#DivTableControl").html("No existen registros");
            } else {
                $("#DivTableControl").html('');
                $("#DivTableControl").html(resultado);
                config.opcionesDT.pageLength = 5;
                config.opcionesDT.order = [[1, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);


            }           
            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#DivTableControl").html('');
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}


function Validar() {
    var valida = true;
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        valida=false;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    } 
    if ($("#txtCodigo").val() == "") {
        $("#txtCodigo").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtCodigo").css('borderColor', '#ced4da');
    } 

    if ($("#NombreEmpleado").val() == "") {
        $("#NombreEmpleado").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#NombreEmpleado").css('borderColor', '#ced4da');
    } 
    return valida;
}

function GuardarControl() {

    if (!Validar()) {
        return;
    }

    $.ajax({
        url: "../ControlBalanza/ControlBalanza",
        type: "POST",
        data: {
            Fecha: $('#txtFecha').val(),
            Cedula: $("#Identificacion").val(),
            Codigo: $("#txtCodigo").val(),
            Observacion: $("#txtObservacion").val(),
            Linea: $("#selectLinea").val()
        },
        success: function (resultado) {
            MensajeCorrecto(resultado);
            CargarControlBalanza();
            Limpiar();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function EditarControlBalanza(model) {
    $("#NombreEmpleado").val(model.Nombre);
    $("#Identificacion").val(model.Cedula);
    $("#txtCodigo").val(model.Codigo);
    $("#txtObservacion").val(model.Observacion);
}



function InactivarControlConsumoDetalle() {
    $.ajax({
        url: "../ControlBalanza/EliminarControlBalanza",
        type: "POST",
        data: {
            IdControlBalanza: $("#txtEliminarDetalle").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            CargarControlBalanza();
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function EliminarControlBalanza(model) {
    $("#txtEliminarDetalle").val(model.IdControlBalanza);
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    $("#modalEliminarControlDetalle").modal('show');
}

$("#modal-detalle-si").on("click", function () {
    InactivarControlConsumoDetalle();
    $("#modalEliminarControlDetalle").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});
