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
            MensajeError("Error, comuniquese con sistemas. " + result, false);
        }
    });
}

function NuevoControl() {
    $("#txtIdControl").val('0');
    $("#txtNombre").val('');
    $("#txtCodigo").val('');
    $("#txtVersion").val('');

    $("#txtIdControlModal").val('0');
    $("#txtNombreModal").val('');
    $("#txtCodigoModal").val('');
    $("#txtVersionModal").val('');

}

function SeleccionarReporteMaestro(model) {
   // alert(model);
    $("#hMensaje").prop("hidden", false);
    $("#hMensaje").html(model.Nombre);

    $("#txtIdControl").val(model.IdReporteMaestro);
    $("#txtNombre").val(model.Nombre);
    $("#txtCodigo").val(model.Codigo);
    $("#txtVersion").val(model.UltimaVersion);

    $("#txtIdControlModal").val(model.IdReporteMaestro);
    $("#txtNombreModal").val(model.Nombre);
    $("#txtCodigoModal").val(model.Codigo);
    $("#txtVersionModal").val(model.UltimaVersion);

    $("#divCabecera").prop("hidden", true);
    $("#btnGenerar").prop("hidden", true);
    $("#divCabeceraPartial").prop("hidden", true);

    $("#btnEditar").prop("hidden", false);
    $("#btnAtras").prop("hidden", false);
    $("#divDetalle").prop("hidden", false);


    ConsultarReporteDetalle(model.IdReporteMaestro);

}
function Atras() {

    $("#hMensaje").prop("hidden", true);
    $("#hMensaje").html('');

    $("#divCabecera").prop("hidden", false);
    $("#btnGenerar").prop("hidden", false);
    $("#divCabeceraPartial").prop("hidden", false);

    $("#btnEditar").prop("hidden", true);
    $("#btnAtras").prop("hidden", true);
    $("#divDetalle").prop("hidden", true);
    NuevoControl();
    ConsultarReporteMaestros();
}


function EditarReporte() {
    $("#ModalEditarControl").modal("show");
}

function EditarReporteMaestro() {
    if ($("#txtIdControlModal").val() == "") {
        $("#txtIdControlModal").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtIdControlModal").css('borderColor', '#ced4da');
    }
    if ($("#txtNombreModal").val() == "") {
        $("#txtNombreModal").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtNombreModal").css('borderColor', '#ced4da');
    }
    if ($("#txtCodigoModal").val() == "") {
        $("#txtCodigoModal").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtCodigoModal").css('borderColor', '#ced4da');
    }
    if ($("#txtVersionModal").val() == "") {
        $("#txtVersionModal").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtVersionModal").css('borderColor', '#ced4da');
    }
    $.ajax({
        url: "../ReporteMaestro/ReporteMaestro",
        type: "POST",
        data: {
            IdReporteMaestro: $("#txtIdControlModal").val(),
            Nombre: $("#txtNombreModal").val(),
            Codigo: $("#txtCodigoModal").val(),
            UltimaVersion: $("#txtVersionModal").val()

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == "0") {
                MensajeAdvertencia("Faltan parametros");
                return;
            }
            //      NuevaReporteMaestro();
            $("#ModalEditarControl").modal("hide");
            MensajeCorrecto("Registro Exitoso");
            //ConsultarReporteMaestros();
        },
        error: function (result) {
           /* console.log(JSON.stringify(result.responseText))*/;
            MensajeError("Error, comuniquese con sistemas. " + result, false);
        }
    });
}



//////////////////////////////////////DETALLE///////////////////////////////////////////////

function ConsultarReporteDetalle(Id) {

    $("#spinnerCargandoDetalle").prop("hidden", false);
    $.ajax({
        url: "../ReporteMaestro/ReporteDetallePartial",
        type: "GET",
        data: {
            idControl: Id
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#chartDetalle").html("No existen registros");
                $("#spinnerCargandoDetalle").prop("hidden", true);
            } else {
                $("#spinnerCargandoDetalle").prop("hidden", true);
                $("#chartDetalle").html(resultado);
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


function GenerarReporteDetalle() {
    $("#ModalControlDetalle").modal("show");
}



function readFile(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            var filePreview = document.createElement('img');
            filePreview.id = 'file-preview';
            //e.target.result contents the base64 data from the image uploaded
            filePreview.src = e.target.result;
            console.log(e.target.result);

            var previewZone = document.getElementById('file-preview-zone');
            previewZone.appendChild(filePreview);
            document.getElementById("file-preview").style.width = "200px";
            document.getElementById("file-preview").style.height = "300px";
}

        reader.readAsDataURL(input.files[0]);
    }
}

var fileUpload = document.getElementById('file-upload');
fileUpload.onchange = function (e) {
    readFile(e.srcElement);
}
