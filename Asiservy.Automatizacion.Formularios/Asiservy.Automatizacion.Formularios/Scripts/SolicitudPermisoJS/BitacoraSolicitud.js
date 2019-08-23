
function ActivarFechas() {
    var check = document.getElementById("SwitchFechas").checked;
    var textSolicitud = document.getElementById("textSolicitud");
    var textCedula = document.getElementById("textCedula");
    var dateDesde = document.getElementById("dateDesde");
    var dateHasta = document.getElementById("dateHasta");
    if (check) {
        dateDesde.removeAttribute("disabled");
        dateHasta.removeAttribute("disabled");
        textSolicitud.setAttribute("disabled",true)
        textCedula.setAttribute("disabled", true)
        textSolicitud.value = "";
        textCedula.value = "";

    } else {

        textSolicitud.removeAttribute("disabled");
        textCedula.removeAttribute("disabled");
        dateDesde.setAttribute("disabled", true)
        dateHasta.setAttribute("disabled", true)

        dateDesde.value = "";
        dateHasta.value = "";

    }


}
