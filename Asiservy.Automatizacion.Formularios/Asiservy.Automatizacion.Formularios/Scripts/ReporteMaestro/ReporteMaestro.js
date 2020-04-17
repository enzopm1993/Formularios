$(document).ready(function () {
    ConsultarReporteMaestros();
  
});


function ConsultarReporteMaestros() {

    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../ReporteMaestro/ReporteMaestroPartial",
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#chartCabecera").html("No existen registros");
                $("#spinnerCargando").prop("hidden", true);
            } else {
                $("#spinnerCargando").prop("hidden", true);
                $("#chartCabecera").html(resultado);
                config.opcionesDT.pageLength = -1;
                config.opcionesDT.order = [];
                $('#tblDataTable2').DataTable(config.opcionesDT);
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}



function GenerarReporteMaestro() {
    if ($("#txtIdControl").val() == "") {
        $("#txtIdControl").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtIdControl").css('borderColor', '#ced4da');
    }
    if ($("#txtNombre").val() == "") {
        $("#txtNombre").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtNombre").css('borderColor', '#ced4da');
    }
    if ($("#txtCodigo").val() == "") {
        $("#txtCodigo").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtCodigo").css('borderColor', '#ced4da');
    }
    if ($("#txtVersion").val() == "") {
        $("#txtVersion").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtVersion").css('borderColor', '#ced4da');
    }
    $.ajax({
        url: "../ReporteMaestro/ReporteMaestro",
        type: "POST",
        data: {
            IdReporteMaestro: $("#txtIdControl").val(),
            Nombre: $("#txtNombre").val(),
            Codigo: $("#txtCodigo").val(),
            UltimaVersion: $("#txtVersion").val()

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == "0") {
                MensajeAdvertencia("Faltan parametros");
                return;
            }
      //      NuevaReporteMaestro();
            MensajeCorrecto("Registro Exitoso");
            ConsultarReporteMaestros();
        },
        error: function (result) {
           /* console.log(JSON.stringify(result.responseText))*/;
            MensajeError(resultado.Mensaje, false);
        }
    });
}


function SeleccionarReporteMaestro(model) {
    alert(model);
}