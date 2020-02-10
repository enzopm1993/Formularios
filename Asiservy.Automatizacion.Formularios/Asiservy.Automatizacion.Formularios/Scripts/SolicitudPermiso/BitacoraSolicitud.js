
function ConsultarBitacora() {
    $("#spinnerCargando").prop("hidden", false);
    $('#Bitacora').html('');
    $.ajax({
        url: "../SolicitudPermiso/BitacoraSolicitudPartial",
        type: "GET",
        data: {
            dsCedula: $('#Identificacion').val(),
            dsIdSolicitud: $('#txtNumeroSolicitud').val(),
            ddFechaDesde: $('#dateDesde').val(),
            ddFechaHasta: $('#dateHasta').val()
        },
        success: function (resultado) {
            //console.log(JSON.stringify(resultado));
            if (resultado == "101") {
                window.location.reload();
            }
            var bitacora = $('#Bitacora');
            $("#spinnerCargando").prop("hidden", true);

            if (resultado.Failed) {
                MensajeAdvertencia(resultado.Mensaje, false);
            } else {
                bitacora.html(resultado);
            }
        },
        error: function (result) {
            $("#spinnerCargando").prop("hidden", true);
            MensajeError(result.responseText, false);

        }
    });
}


function ActivarFechas() {
    var check = document.getElementById("SwitchFechas").checked;
   
    var dateDesde = document.getElementById("dateDesde");
    var dateHasta = document.getElementById("dateHasta");
    if (check) {
        dateDesde.removeAttribute("disabled");
        dateHasta.removeAttribute("disabled");
        dateDesde.value = moment().format("YYYY-MM-DD");
        dateHasta.value = moment().format("YYYY-MM-DD");

    } else {        
        dateDesde.setAttribute("disabled", true)
        dateHasta.setAttribute("disabled", true)
        dateDesde.value = "";
        dateHasta.value = "";
    }
}

