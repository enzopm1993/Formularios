


$(document).ready(function () {
    $("#DivHora").hide();
   
});

function CambioHoraFecha() {
    var HoraDesde = document.getElementById("timeHoraSalida");
    var HoraHasta = document.getElementById("timeHoraRegreso");
    var FechaSalidaRegreso = document.getElementById("dateSalidaRegreso");
    var FechaDesde = document.getElementById("dateSalida");
    var FechaHasta = document.getElementById("dateRegreso");
    var check = document.getElementById("switchHoraFecha").checked


    if (check) {
        $("#LabelFecha").text("Hora");
        $("#DivHora").slideUp(300).fadeIn(1000);
        $("#DivFecha").slideUp(300).fadeOut(1000);
        FechaDesde.value = null;
        FechaHasta.value = null;
    } else {
        $('#LabelFecha').text("Fecha");
        $("#DivHora").slideUp(300).fadeOut(1000);
        $("#DivFecha").slideUp(300).fadeIn(1000);
        FechaSalidaRegreso.value = null;
        HoraDesde.value = null;
        HoraHasta.value = null;
    }
}


function Guardar() {
    var CodigoMotivo = $("#selectMotivo").val();
    var Observacion = $("#Observacion").val();
    var FechaSalidaEntrada = $("#dateSalidaRegreso").val();
    var HoraSalida = $("#timeHoraSalida").val();
    var HoraRegreso = $("#timeHoraRegreso").val();
    var FechaSalida = $("#dateSalida").val();
    var FechaRegreso = $("#dateRegreso").val();
    var CodLinea = $("#txtCodLinea").val();

    $("#spinnerCargando").prop("hidden", false);
    $("#btnGuardar").prop("hidden", true);
    $.ajax({
        url: "../SolicitudPermiso/SolicitudPermisoMasivo",
        type: "POST",
        data: {
            CodigoMotivo: CodigoMotivo,
            Observacion: Observacion,
            FechaSalidaEntrada: FechaSalidaEntrada,
            HoraSalida: HoraSalida,
            HoraRegreso: HoraRegreso,
            FechaSalida: FechaSalida,
            FechaRegreso: FechaRegreso,
            CodigoLinea: CodLinea
        },
        success: function (resultado) {
            if (resultado.Codigo == 0) {
                MensajeAdvertencia(resultado.Mensaje);
            } else {
                MensajeCorrecto(resultado.Mensaje)
                $("#spinnerCargando").prop("hidden", true);
                $("#h3Mensaje").html(resultado.Mensaje);
            }
        },
        error: function (resultado) {
            //console.log(resultado);
            $("#spinnerCargando").prop("hidden", true);
            $("#btnGuardar").prop("hidden", false);
            MensajeError(resultado.responseText, false);
        }
    });



}