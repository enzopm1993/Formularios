﻿//$('#GuardarSolicitudGeneral').click(function () {
//    $('#GuardarSolicitudGeneral').prop('readonly', true);
//}
//);
function CambioHoraFecha() {
    var HoraDesde = document.getElementById("timeHoraSalida");
    var HoraHasta = document.getElementById("timeHoraRegreso");
    var FechaSalidaRegreso = document.getElementById("dateSalidaRegreso");
    var FechaDesde = document.getElementById("dateSalida");
    var FechaHasta = document.getElementById("dateRegreso");
    var check = document.getElementById("switchHoraFecha").checked
    console.log(check);

    if (check) {
        HoraDesde.removeAttribute("readonly");
        HoraHasta.removeAttribute("readonly");
        FechaSalidaRegreso.removeAttribute("readonly");
        FechaDesde.setAttribute("readonly", true);
        FechaHasta.setAttribute("readonly", true);
        console.log(FechaDesde);
        FechaDesde.value = null;
        FechaHasta.value = null;
    } else {

        HoraDesde.setAttribute("readonly", true);
        HoraHasta.setAttribute("readonly", true);
        FechaSalidaRegreso.setAttribute("readonly", true);
        FechaDesde.removeAttribute("readonly");
        FechaHasta.removeAttribute("readonly");
        FechaSalidaRegreso.value = null;
        HoraDesde.value = null;
        HoraHasta.value = null;
    }
}

document.getElementById("selectArea").options[0].disabled = true;
document.getElementById("CodigoLinea").options[0].disabled = true;
document.getElementById("selectCargo").options[0].disabled = true;
document.getElementById("selectMotivo").options[0].disabled = true;

//$('#CargarEmpleadoPG').prop('readonly', true);
//$('#CargarEmpleadoPG').attr("disabled", true);



