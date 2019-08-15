

function fillBook(valor) {
    var desSelectEstado = "SelectEstado";
    var desCheckAsistencia = "CheckAsistencia";
    var desspanAsistencia = "spanAsistencia";
    if (valor != 0) {
        desSelectEstado +=  valor;
        desCheckAsistencia += valor;
        desspanAsistencia +=valor;
    }
    console.log(desCheckAsistencia);
    var SelectEstado = document.getElementById(desSelectEstado);
    var chexkAsistencia = document.getElementById(desCheckAsistencia).checked;
    var span = document.getElementById(desspanAsistencia);



    var Hora = new Date().getHours();
   

//    console.log(chexkAsistencia);
    if (chexkAsistencia) {
        if (Hora >= 10) {
            SelectEstado.selectedIndex = 2;
            span.style.backgroundColor = "yellow";
        } else {
            SelectEstado.selectedIndex = 1;
            span.style.backgroundColor = "greenyellow";
        }
    }
    else {
        SelectEstado.selectedIndex = 0;
        span.style.backgroundColor = "#ccc";
    }
}

function CambioEstado(valor) {
    var desSelectEstado = "SelectEstado";
    var desCheckAsistencia = "CheckAsistencia";
    var desspanAsistencia = "spanAsistencia";
    if (valor != 0) {
        desSelectEstado += valor;
        desCheckAsistencia += valor;
        desspanAsistencia += valor;
    }

    var SelectEstado = document.getElementById(desSelectEstado).value;
    var span = document.getElementById(desspanAsistencia);
    var CheckAsistencia = document.getElementById(desCheckAsistencia);


    if (SelectEstado == 1) {
        CheckAsistencia.checked = true;
        span.style.backgroundColor = "greenyellow";
    }

    if (SelectEstado == 2) {
        CheckAsistencia.checked = true;
        span.style.backgroundColor = "yellow";
    }

    if (SelectEstado == 3) {
        span.style.backgroundColor = "red";
        CheckAsistencia.checked = true;
    }


}
