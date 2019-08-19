
var i = 0;
$("tr").each(function () {
    var desCheck = "CheckCuchilloNegro";
    if(i>1)
        desCheck += i;
    var x = document.getElementById(desCheck);
    if (x != null)
        x.disabled = true;
    i++;
});


function Cuchillo(color, fila) {

    var desLabel = "labelCuchillo";
    var desCheck = "CheckCuchillo";

    if (color == 1) { desLabel += "Rojo"; desCheck += "Rojo"; }
    if (color == 2) { desLabel += "Negro"; desCheck += "Negro"; }
    if (color == 3) { desLabel += "Blanco"; desCheck += "Blanco"; }

    if (fila != 1) {
        desLabel += fila;
        desCheck += fila;
    }
    //console.log(desCheck);
    var label = document.getElementById(desLabel);
    var check = document.getElementById(desCheck).checked;
    //console.log(check);

    if (check) {
        label.style.background = "#27D5C3";
        document.getElementById(desCheck).checked = true;
    } else {
        label.style.background = "#ccc";
        document.getElementById(desCheck).checked = false;
    }

}

function Guardar() {
    var Estado = document.getElementById("SelectEstado");
   // console.log(Estado);
    //console.log(Estado.selectedIndex);
    if (Estado.selectedIndex == 0) {
        Mensaje("Seleccione un estado..");
        //   alert("Seleccione un Estado..");
    } else {
        Mensaje("Registro Guardado Exitosamente");
    }

}

function prueba() {

    var checkRojo = document.getElementById("CheckCuchilloRojo").checked;
    var checkBlanco = document.getElementById("CheckCuchilloBlanco").checked;
    var checkNegro = document.getElementById("CheckCuchilloNegro").checked;

    console.log(checkRojo, checkNegro, checkBlanco);

}
