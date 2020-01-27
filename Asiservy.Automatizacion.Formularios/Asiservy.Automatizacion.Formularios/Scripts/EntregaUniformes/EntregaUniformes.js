
$(document).ready(function () {
    ConsultarEntregaUniformes();
});

function AgregarEntregaUniforme() {
    $("#ModalEntregaUniforme").modal("show");
}

function GenerarEntregaUniforme() {
    $.ajax({
        url: "../EntregaUniforme/EntregaUniforme",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            //console.log(JSON.stringify(resultado));
            if (resultado == "101") {
                window.location.reload();
            }
            var bitacora = $('#divEntregaUniforme');
            $("#spinnerCargando").prop("hidden", true);

            if (resultado.Failed) {
                MensajeAdvertencia(resultado.Mensaje, false);
            } else {
                bitacora.html(resultado);
            }
        },
        error: function (result) {
           /* console.log(JSON.stringify(result.responseText))*/;
            MensajeError(resultado.Mensaje, false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}

function ConsultarEntregaUniformes() {
    $("#spinnerCargando").prop("hidden", false);
    if ($("#txtFecha").val() == "") {
        return;
    }
    $('#divEntregaUniforme').html('');
    $.ajax({
        url: "../EntregaUniforme/EntregaUniformePartial",
        type: "GET",
        data: {
           Fecha:$("#txtFecha").val()
        },
        success: function (resultado) {
            //console.log(JSON.stringify(resultado));
            if (resultado == "101") {
                window.location.reload();
            }
            var bitacora = $('#divEntregaUniforme');
            $("#spinnerCargando").prop("hidden", true);

            if (resultado.Failed) {
                MensajeAdvertencia(resultado.Mensaje, false);
            } else {
                bitacora.html(resultado);
            }
        },
        error: function (result) {
           /* console.log(JSON.stringify(result.responseText))*/;
            MensajeError(resultado.Mensaje, false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}