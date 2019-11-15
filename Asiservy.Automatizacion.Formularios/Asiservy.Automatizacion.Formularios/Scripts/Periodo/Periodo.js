


$(document).ready(function () {
    CargarTablaPeriodo();
    $('#IdParametro').val(0);
});


function Valida() {
    var bol = true;
    if ($('#txtCodigo').val() == '') {
        $("#pValidaCodigo").prop("hidden", false);
        bol = false;
    } else {
        $("#pValidaCodigo").prop("hidden", true);
    }
    if ($('#txtDescripcion').val() == '') {
        $("#pValidaDescripcion").prop("hidden", false);
        bol = false;
    } else {
        $("#pValidaDescripcion").prop("hidden", true);
    }
    if ($('#SelectEstado').val() == "0") {
        $("#pValidaEstado").prop("hidden", false);
        bol = false;
    } else {
        $("#pValidaEstado").prop("hidden", true);
    }


    if ($('#txtFechaDesde').val() == "") {
        $("#pValidaFechaDesde").prop("hidden", false);
        bol = false;
    } else {
        $("#pValidaFechaDesde").prop("hidden", true);
    }
    if ($('#txtFechaHasta').val() == "") {
        $("#pValidaFechaHasta").prop("hidden", false);
        bol = false;
    } else {
        $("#pValidaFechaHasta").prop("hidden", true);
    }

    return bol;

}

function GuardarModificarPeriodo() {

    if (!Valida())
        return;
   
    $.ajax({
        url: "../Periodo/Periodo",
        type: "POST",
        data: {
            IdPeriodo: $('#IdPeriodo').val(),
            FechaDesde: $('#txtFechaDesde').val(),
            Descripcion: $('#txtDescripcion').val(),
            FechaHasta: $('#txtFechaHasta').val(),
            Estado: $('#SelectEstado').val(),
            

        },
        success: function (resultado) {
            if (resultado.Codigo == 0)
                MensajeAdvertencia(resultado.Mensaje);
            else {
                MensajeCorrecto(resultado.Mensaje);
                CargarTablaPeriodo();
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#SpinnerCargando").prop("hidden", true);

        }
    });
}


function CargarTablaPeriodo() {
    $("#SpinnerCargando").prop("hidden", false);
    $('#divTableParametros').html('');
    $.ajax({
        url: "../Periodo/PeriodoPartial",
        type: "GET",
        success: function (resultado) {

            var bitacora = $('#divTablePeriodos');
            bitacora.html(resultado);
            $("#SpinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError(resultado, false);
            $("#SpinnerCargando").prop("hidden", true);

        }
    });
}

function Nuevo() {
    $('#IdPeriodo').val('0');    
    $('#txtDescripcion').val('');
    $('#txtObservacion').val('');
    $('#SelectEstado').val(0);
    $("#txtFechaDesde").val('');
    $("#txtFechaHasta").val('');

    }

function SeleccionPeriodo(id, descripcion, estado, FechaDesde, FechaHasta) {
 
   
    $('#IdPeriodo').val(id);
    $('#txtFechaDesde').val(FechaDesde);
    $('#txtDescripcion').val(descripcion);
    $('#txtFechaHasta').val(FechaHasta);
    $('#SelectEstado').val(estado);
   
}