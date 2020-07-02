


$(document).ready(function () {
    CargarTablaPeriodo();
    $('#IdParametro').val(0);
});


function Valida() {
    var bol = true;
    if ($("#txtCodigo").val() == "") {
        $("#txtCodigo").css('borderColor', '#FA8072');
        bol=false;
    } else {
        $("#txtCodigo").css('borderColor', '#ced4da');
    }
    if ($("#txtDescripcion").val() == "") {
        $("#txtDescripcion").css('borderColor', '#FA8072');
        bol = false;
    } else {
        $("#txtDescripcion").css('borderColor', '#ced4da');
    }
    if ($("#SelectEstado").val() == "") {
        $("#SelectEstado").css('borderColor', '#FA8072');
        bol = false;
    } else {
        $("#SelectEstado").css('borderColor', '#ced4da');
    }
    if ($("#txtFechaDesde").val() == "") {
        $("#txtFechaDesde").css('borderColor', '#FA8072');
        bol = false;
    } else {
        $("#txtFechaDesde").css('borderColor', '#ced4da');
    }

    if ($("#txtFechaHasta").val() == "") {
        $("#txtFechaHasta").css('borderColor', '#FA8072');
        bol = false;
    } else {
        $("#txtFechaHasta").css('borderColor', '#ced4da');
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
    $('#divTablePeriodos').html('');
    $.ajax({
        url: "../Periodo/PeriodoPartial",
        type: "GET",
        success: function (resultado) {

            $('#divTablePeriodos').html(resultado);
            $("#SpinnerCargando").prop("hidden", true);
            Nuevo();
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
    $('#SelectEstado').val();
    $("#txtFechaDesde").val('');
    $("#txtFechaHasta").val('');

    $("#txtDescripcion").css('borderColor', '#ced4da');
    $("#SelectEstado").css('borderColor', '#ced4da');
    $("#txtFechaDesde").css('borderColor', '#ced4da');
    $("#txtFechaHasta").css('borderColor', '#ced4da');

    }

function SeleccionPeriodo(id, descripcion, estado, FechaDesde, FechaHasta) {
 
   
    $('#IdPeriodo').val(id);
    $('#txtFechaDesde').val(FechaDesde);
    $('#txtDescripcion').val(descripcion);
    $('#txtFechaHasta').val(FechaHasta);
    $('#SelectEstado').val(estado);
   
}