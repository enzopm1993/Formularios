//function Guardar() {
//    Mensaje("Registro Guardado Exitosamente");
//}

//function CambioDepartamento(valor) {
//    var arrayValores = new Array(
//        new Array(1, 1, "-	Calamidad domestica"),
//        new Array(1, 2, "-	Comisión de servicios "),
//        new Array(1, 3, "-	Asuntos particulares"),
//        new Array(1, 4, "-	Vacaciones")
//    );

//    console.log(valor);
//    if (valor == 0) {
//        document.getElementById("selectMotivo").disabled = true;
//    } else {
//            document.getElementById("selectMotivo").options.length = 0;
//            document.getElementById("selectMotivo").options[0] = new Option("Selecciona una opcion", "0");
//            for (i = 0; i < arrayValores.length; i++) {
//                    document.getElementById("selectMotivo").options[document.getElementById("selectMotivo").options.length] = new Option(arrayValores[i][2], arrayValores[i][1]);
//            }
//            document.getElementById("selectMotivo").disabled = false;
//    }
//    var selectArea = document.getElementById("selectArea");
//    if (valor == 1) {
//        selectArea.selectedIndex = 0;
//        selectArea.disabled = false;
//    } else {

//        selectArea.selectedIndex = 0;
//        selectArea.disabled = true;
//    }
//    CambioArea();
//}

//function CambioArea() {
//    var e = document.getElementById("selectArea");
//    var selectValor = e.options[e.selectedIndex].value;
//    var labelLinea = document.getElementById("labelLinea");
//    var SelectLinea = document.getElementById("selectLinea");
//    if (selectValor == "1") {
//        labelLinea.removeAttribute("hidden");
//        SelectLinea.removeAttribute("hidden");
//    }
//    else {

//        labelLinea.setAttribute("hidden", true);
//        SelectLinea.setAttribute("hidden", true);
//    }
//}

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





