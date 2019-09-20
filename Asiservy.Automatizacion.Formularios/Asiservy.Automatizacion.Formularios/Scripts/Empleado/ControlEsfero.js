//$(function () {
//    var d = new Date(),
//        h = d.getHours(),
//        m = d.getMinutes();
//    if (h < 10) h = '0' + h;
//    if (m < 10) m = '0' + m;
//    $('input[type="time"][value="now"]').each(function () {
//        $(this).attr({ 'value': h + ':' + m });
//    });
//});

function SeleccionTipo(valor) {
    if (valor != "0") {
        CargarTablaControlEsfero(valor);
    } else {
        var bitacora = $('#DivTableControlEsfero');
        bitacora.html("");
    }
}

function CargarTablaControlEsfero(tipo) {
    $.ajax({
        url: "../Empleado/ControlEsferoPartial",
        type: "GET",
        data: { dsTipo: tipo },
        success: function (resultado) {
            var bitacora = $('#DivTableControlEsfero');
            bitacora.html(resultado);
        },
        error: function (resultado) {
            MensajeError(resultado.responseJSON, false);
        }
    });
}

function GuardarMoficarControl(id,cedula) {
    var tipo = $('#SelectTipo').val();
    var idHora = '#Hora' + cedula;
    var hora = $(idHora).val();
    var label = "#label-" + cedula;
    id = "#" + id;
    if (hora == "") {
        MensajeAdvertencia("Ingrese una hora", false);
        $(id).prop('checked', false);
    } else {
        if ($(id).prop('checked')) {
            $(label).css("background", "#28B463");

        } else {
            $(label).css("background", "#7b8a8b");
            hora = null;

        }
        $(id).prop("disabled", true);
        $.ajax({
            url: "../Empleado/ControlEsfero",
            type: "POST",
            data: {
                Cedula: cedula,
                Hora: hora,
                dsTipo: tipo
            },
            success: function (resultado) {
                $(id).prop("disabled", false);

            },
            error: function (resultado) {
                MensajeError(resultado.responseJSON, false);
                $(id).prop("disabled", false);

            }
        });
    }
}