



function SelectEstado(valor) {
    if (valor < 1 && $("#EstadoControlCuchillo").val() != "undefined")
        valor = $("#EstadoControlCuchillo").val();

    CargarControlCuchillo(valor);
}

function CargarControlCuchillo(estado) {
    if (estado < 1)
        return;

    if ($("#txtFecha").val() == '' || $("#txtFecha").val() == null) {

        MensajeAdvertencia("Ingrese una Fecha");
        return;
    }
    $('#TablaControlCuchillo').html('');
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../ControlCuchillo/ControlCuchilloPartial",
        type: "GET",
        data: {
            dsEstado: estado,
            ddFecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            var bitacora = $('#TablaControlCuchillo');
            $("#spinnerCargando").prop("hidden", true);
            if (resultado == "0") {
                bitacora.html("<div class='text-center'><h4>No se ha generado la asistencia</h4></div>");

            } else {
                bitacora.html(resultado);
            }

        },
        error: function (resultado) {

            MensajeError(resultado, false);
            $("#spinnerCargando").prop("hidden", true);


        }
    });
}




function check(id, color, cedula) {

    var Observacion = "#txtObservacion-" + cedula;
    var numero = 0;
    var estado = $('#EstadoControlCuchillo').val();
    if (color == 'B') {
        numero = id.substring(9, 6);
    } if (color == 'R') {
        numero = id.substring(8, 4);
    } if (color == 'N') {
        numero = id.substring(9, 5);
    }
    id = "#" + id;
    $(id).prop('disabled', true);
    if ($(id).prop('checked')) {
        // $(label).css("background", "#28B463");
        $(Observacion).prop('readonly', true);
        GuardarControlCuchillo(cedula, color, numero, estado, true, id, Observacion);
    } else {
        // $(label).css("background", "#7b8a8b");
        $(Observacion).prop('readonly', false);
        GuardarControlCuchillo(cedula, color, numero, estado, false, id, Observacion);
    }
}



function GuardarControlCuchillo(cedula, color, numero, estado, check, idCheck, Observacion) {

    $.ajax({
        url: "../ControlCuchillo/GuardarControlCuchillo",
        type: "GET",
        data: {
            dsCedula: cedula,
            dsColor: color,
            dsNumero: numero,
            dsEstado: estado,
            dbCheck: check,
            ddFecha: $("#txtFecha").val(),
            Observacion: $(Observacion).val()
        },
        success: function (resultado) {
            if (resultado.codigo == 1) {
                MensajeAdvertencia(resultado.descripcion)
                $(idCheck).prop('checked', false);
                //  $(Observacion).val()
                // $(idLabel).css("background", "#7b8a8b");               
            }
            $(idCheck).prop('disabled', false);

        },
        error: function (resultado) {
            //console.log(resultado.responseJSON);
            //console.log(resultado);
            MensajeError(resultado.responseText + "", false);
            // MensajeError(resultado + "", false);
            $(idCheck).prop('checked', false);
            // $(idLabel).css("background", "#7b8a8b");
            $(idCheck).prop('disabled', false);


        }
    });
}
