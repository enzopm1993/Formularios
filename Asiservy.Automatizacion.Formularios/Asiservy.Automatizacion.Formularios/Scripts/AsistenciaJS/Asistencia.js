
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
    var desspanAsistencia = "spanAsistencia";
    var desObservacion = "Observacion"
    if (valor != 0) {
        desSelectEstado +=  valor;
        desCheckAsistencia += valor;
        desspanAsistencia += valor;
        desObservacion += valor;
    }
   // console.log(desCheckAsistencia);
    var SelectEstado = document.getElementById(desSelectEstado);
    var chexkAsistencia = document.getElementById(desCheckAsistencia).checked;
    var span = document.getElementById(desspanAsistencia);
    var observacion = document.getElementById(desObservacion);

    var Hora = new Date().getHours();
    var Minuto = new Date().getMinutes();

    //console.log(Hora);
    if (chexkAsistencia) {
        if (Hora >= 10) {
            SelectEstado.selectedIndex = 2;
            span.style.backgroundColor = "yellow";
            observacion.value = "Ingreso: "+Hora.toString() + ":" + Minuto.toString() + " AM ";
        } else {
            SelectEstado.selectedIndex = 1;
            span.style.backgroundColor = "greenyellow";

        }
    }
    else {
        SelectEstado.selectedIndex = 0;
        span.style.backgroundColor = "#ccc";
        observacion.value = "";
    }
}

function CambioEstado(valor) {
    var desSelectEstado = "SelectEstado";
    var desCheckAsistencia = "CheckAsistencia";
    var desspanAsistencia = "spanAsistencia";
    var desObservacion = "Observacion"

    if (valor != 0) {
        desSelectEstado += valor;
        desCheckAsistencia += valor;
        desspanAsistencia += valor;
        desObservacion += valor;
    }


    var SelectEstado = document.getElementById(desSelectEstado).value;
    var span = document.getElementById(desspanAsistencia);
    var CheckAsistencia = document.getElementById(desCheckAsistencia);
    var observacion = document.getElementById(desObservacion);


    if (SelectEstado == 1) {
        CheckAsistencia.checked = true;
        span.style.backgroundColor = "greenyellow";
        observacion.value = "";
    }

    if (SelectEstado == 2) {
        CheckAsistencia.checked = true;
        span.style.backgroundColor = "yellow";
        var Hora = new Date().getHours();
        var Minuto = new Date().getMinutes();
        observacion.value = "Ingreso: " + Hora.toString() + ":" + Minuto.toString() + " AM ";

    }

    if (SelectEstado == 3) {
        span.style.backgroundColor = "red";
        CheckAsistencia.checked = true;
        observacion.value = "";

    }


}
