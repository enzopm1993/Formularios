



$(document).ready(function () {
    $("#DivHora").hide();   
    //CargarAreas();
});


function CargarAreas() {
    $.ajax({
        url: "../SolicitudPermiso/ConsultaListadoAreas",
        type: "Get",
        data:
        {
            CodLinea: $('#selectLinea').val()
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#selectArea").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("La linea seleccionado no tiene areas asignadas", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}
function CambioHoraFecha() {
    var HoraDesde = document.getElementById("timeHoraSalida");
    var HoraHasta = document.getElementById("timeHoraRegreso");
    var FechaSalidaRegreso = document.getElementById("dateSalidaRegreso");
    var FechaDesde = document.getElementById("dateSalida");
    var FechaHasta = document.getElementById("dateRegreso");
    var check = document.getElementById("switchHoraFecha").checked
   

    if (check) {
        $("#LabelFecha").text("Hora");
        $("#DivHora").show();
        $("#DivFecha").hide();
        FechaDesde.value = null;
        FechaHasta.value = null;
    } else {
        $('#LabelFecha').text("Fecha");
        $("#DivHora").hide();
        $("#DivFecha").show();       
        FechaSalidaRegreso.value = null;
        HoraDesde.value = null;
        HoraHasta.value = null;
    }
}





