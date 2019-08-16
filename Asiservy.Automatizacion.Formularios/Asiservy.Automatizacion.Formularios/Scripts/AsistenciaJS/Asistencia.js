
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

    if (valor != 0) {
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
    }

    if (SelectEstado == 2) {
        CheckAsistencia.checked = true;
        label.style.backgroundColor = "yellow";
        var Hora = new Date().getHours();
        var Minuto = new Date().getMinutes();
        observacion.value = "Ingreso: " + Hora.toString() + ":" + Minuto.toString() + " AM ";

    }

    if (SelectEstado == 3) {
        label.style.backgroundColor = "red";
        CheckAsistencia.checked = true;
        observacion.value = "";

    }


}
