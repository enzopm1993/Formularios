//guardar con check
function GuardarPersona(fila) {
    
    
    if ($('#CheckAsistencia-' + fila).prop('checked'))
    {
        fila -= 1;
        $.ajax({
            url: '../Asistencia/GrabarAsistenciaEmpleado',
            type: 'POST',
            dataType: "json",
            data: {
                cedula: $('#ControlAsistencia_' + fila + '__Cedula').val(),
                nombre: $('#ControlAsistencia_' + fila + '__Nombres').val(),
                Hora: $('#ControlAsistencia_' + fila + '__Hora').val(),
                observacion: $('#ControlAsistencia_' + fila + '__Observacion').val()
            },
            success: function (resultado) {
                //MensajeCorrecto(resultado, true);

            }
            ,
            error: function () {
                MensajeError("No se pudieron mover", false);
            }
        });
    }
    
}

//Bloqueo de el chceck de cuchillos negros
var i = 0;
$("tr").each(function () {
    var desCheck = "CheckCuchilloNegro";
    if (i > 1)
        desCheck += i;
    var x = document.getElementById(desCheck);
    if (x != null)
        x.disabled = true;
    i++;
});


//check de asistencia validar, si es atraso
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

//Seleccion de estados, validacion por estado
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

//bloquea o desbloquea los check de cuchillos dependiendo de los parametros
function LimpiarBloquearCheckCuchillo(valor,bool) {
    var desCheckCuchilloRojo = "CheckCuchilloRojo";
    var desCheckCuchilloBlanco = "CheckCuchilloBlanco";
    var desCheckCuchilloNegro = "CheckCuchilloNegro";

    var desLabelCuchilloRojo = "labelCuchilloRojo";
    var desLabelCuchilloBlanco = "labelCuchilloBlanco";
    var desLabelCuchilloNegro = "labelCuchilloNegro";
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
    label2.style.background = "#ccc";
   // label3.style.background = "#ccc";
    //console.log(desLabelCuchilloBlanco);
    //console.log(label1);
    //console.log(label2);
    //console.log(label3);
    if (bool) {
        cuchilloRojo.checked = !bool;
     //   cuchilloNegro.checked = !bool;
        cuchilloBlanco.checked = !bool;
    }
    cuchilloRojo.disabled = bool;
    //cuchilloNegro.disabled = bool;
    cuchilloBlanco.disabled = bool;
}

//pintan celda de check de cuchillo
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

