﻿
function ConsultarBitacora() {
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../SolicitudPermiso/BitacoraSolicitudPartial",
        type: "GET",
        data: {
            dsCedula: $('#textCedula').val(),
            dsIdSolicitud: $('#textSolicitud').val(),
            ddFechaDesde: $('#dateDesde').val(),
            ddFechaHasta: $('#dateHasta').val()
        },
        success: function (resultado) {
            //console.log(JSON.stringify(resultado));
      
            var bitacora = $('#Bitacora');
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


function ActivarFechas() {
    var check = document.getElementById("SwitchFechas").checked;
   
    var dateDesde = document.getElementById("dateDesde");
    var dateHasta = document.getElementById("dateHasta");
    if (check) {
        dateDesde.removeAttribute("disabled");
        dateHasta.removeAttribute("disabled");        

    } else {        
        dateDesde.setAttribute("disabled", true)
        dateHasta.setAttribute("disabled", true)

        dateDesde.value = "";
        dateHasta.value = "";

    }
}
