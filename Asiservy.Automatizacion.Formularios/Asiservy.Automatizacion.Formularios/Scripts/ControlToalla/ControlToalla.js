$(document).ready(function () {
    cargarpartial();
});
function cargarpartial() {
    $('#spinnerCargando').prop("hidden", false);
    $.ajax({
        url: "../ControlToalla/PartialControlToalla",
        type: "POST",
        data: {
            Turno: $('#TurnoGen').val(),
            Fecha: $("#txtFecha").val(),
            Hora: $('#txtHora').val(),
            Linea: $("#txtLinea").val(),
            Observacion: $("#txtObservacion").val(),
        },
        success: function (resultado) {
            //CargarControlCoche();
            //MensajeCorrecto(resultado);
            $('#spinnerCargando').prop("hidden", true);
            $('#DivControl').html(resultado);
            //Nuevo();
            //$("#btnGuardar").prop("disabled", false);

        },
        error: function (resultado) {

            //CargarControlCoche();
            //$("#btnGuardar").prop("disabled", false);
            $('#spinnerCargando').prop("hidden", true);

            MensajeError(resultado.responseText, false);
            //Nuevo();

        }
    });
}
function GuardarControl() {
    $("#btnGuardar").prop("disabled", true);
    $('#spinnerCargando').prop("hidden", false);
    //var DivControl = $('#DivTableControlCoche');
    $('#DivControl').empty();
    $.ajax({
        url: "../ControlToalla/GuardarControlToalla",
        type: "POST",
        data: {
            Turno: $('#TurnoGen').val(),
            Fecha: $("#txtFecha").val(),
            Hora: $('#txtHora').val(),
            Linea: $("#txtLinea").val(),
            Observacion: $("#txtObservacion").val(),
            estadoreg:'A'
        },
        success: function (resultado) {
            //CargarControlCoche();
            //MensajeCorrecto(resultado);
            //Nuevo();
            $("#btnGuardar").prop("disabled", false);
            $('#spinnerCargando').prop("hidden", true);
            if (resultado == '999') {
                MensajeAdvertencia("Ya existe un registro en la hora indicada", false);
            } 
                cargarpartial();
            

        },
        error: function (resultado) {

            //CargarControlCoche();
            $("#btnGuardar").prop("disabled", false);
            $('#spinnerCargando').prop("hidden", true);

            MensajeError(resultado.responseText, false);
            //Nuevo();

        }
    });
}