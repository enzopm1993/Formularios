
n = new Date();
//Año
y = n.getFullYear();
//Mes
m = n.getMonth() + 1;
//Día
d = n.getDate();

//Lo ordenas a gusto.
document.getElementById("date").innerHTML = d + "/" + m + "/" + y;

function fillBook(valor) {
    var desSelectEstado = "SelectEstado";
    var desCheckAsistencia = "CheckAsistencia";
    var desLabelAsistencia = "LabelAsistencia";
    var desObservacion = "Observacion"
    if (valor != 0) {
        desSelectEstado +=  valor;
        desCheckAsistencia += valor;
        desLabelAsistencia += valor;
        desObservacion += valor;
    }
   // console.log(desCheckAsistencia);
    var SelectEstado = document.getElementById(desSelectEstado);
    var chexkAsistencia = document.getElementById(desCheckAsistencia).checked;
    var label = document.getElementById(desLabelAsistencia);
    var observacion = document.getElementById(desObservacion);

    var Hora = new Date().getHours();
    var Minuto = new Date().getMinutes();

    //console.log(Hora);
    if (chexkAsistencia) {
        if (Hora >= 10) {
            SelectEstado.selectedIndex = 2;
            label.style.backgroundColor = "yellow";
            observacion.value = "Ingreso: "+Hora.toString() + ":" + Minuto.toString() + " AM ";
        } else {
            SelectEstado.selectedIndex = 1;
            label.style.backgroundColor = "greenyellow";
        }
    }
    else {
        SelectEstado.selectedIndex = 0;
        label.style.backgroundColor = "#ccc";
        observacion.value = "";
    }
}

function CambioEstado(valor) {
    var desSelectEstado = "SelectEstado";
    var desCheckAsistencia = "CheckAsistencia";
    var desLabelAsistencia = "LabelAsistencia";
    var desObservacion = "Observacion"
    if (valor != 1) {
        desSelectEstado += valor;
        desCheckAsistencia += valor;
        desLabelAsistencia += valor;
        desObservacion += valor;
    }
    var SelectEstado = document.getElementById(desSelectEstado).value;
    var label = document.getElementById(desLabelAsistencia);
    var CheckAsistencia = document.getElementById(desCheckAsistencia);
    var observacion = document.getElementById(desObservacion);

    if (SelectEstado == 1) {
        CheckAsistencia.checked = true;
        label.style.backgroundColor = "greenyellow";
        observacion.value = "";
        LimpiarBloquearCheckCuchillo(valor, false);

    }
    if (SelectEstado == 2) {
        CheckAsistencia.checked = true;
        label.style.backgroundColor = "yellow";
        var Hora = new Date().getHours();
        var Minuto = new Date().getMinutes();
        observacion.value = "Ingreso: " + Hora.toString() + ":" + Minuto.toString() + " AM ";
        LimpiarBloquearCheckCuchillo(valor,false);

    }
    if (SelectEstado == 3) {
        label.style.backgroundColor = "red";
        CheckAsistencia.checked = true;
        CheckAsistencia.disabled = true;
        observacion.value = "";
        LimpiarBloquearCheckCuchillo(valor, true);
    }
}

function LimpiarBloquearCheckCuchillo(valor,bool) {
    var desCheckCuchilloRojo = "CheckCuchilloRojo";
    var desCheckCuchilloBlanco = "CheckCuchilloBlanco";
    var desCheckCuchilloNegro = "CheckCuchilloNegro";

    var desLabelCuchilloRojo = "labelCuchilloRojo";
    var desLabelCuchilloBlanco = "LabelCuchilloBlanco";
    var desLabelCuchilloNegro = "LabelCuchilloNegro";
    if (valor > "1" ) {
        desCheckCuchilloRojo += valor;
        desCheckCuchilloBlanco += valor;
        desCheckCuchilloNegro += valor;
        desLabelCuchilloRojo += valor;
        desLabelCuchilloBlanco += valor;
        desLabelCuchilloNegro += valor;
    }
    var cuchilloRojo = document.getElementById(desCheckCuchilloRojo);
    var cuchilloBlanco = document.getElementById(desCheckCuchilloBlanco);
    var cuchilloNegro = document.getElementById(desCheckCuchilloNegro);

    var label1 = document.getElementById(desLabelCuchilloRojo);
    var label2 = document.getElementById(desLabelCuchilloBlanco);
    var label3 = document.getElementById(desLabelCuchilloNegro);

    label1.style.background = "#ccc";
  //  label2.style.background = "#ccc";
    // label3.style.background = "#ccc";
    console.log(desLabelCuchilloBlanco);
    console.log(label1);
    console.log(label2);
    console.log(label3);
    if (bool) {
        cuchilloRojo.checked = !bool;
        cuchilloNegro.checked = !bool;
        cuchilloBlanco.checked = !bool;
    }
    cuchilloRojo.disabled = bool;
    cuchilloNegro.disabled = bool;
    cuchilloBlanco.disabled = bool;



}
