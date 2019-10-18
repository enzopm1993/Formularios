//var ListaIdsHora = new Array();
//function PushHoras(id) {
//    ListaIdsHora.push(id);
//}
//function mueveReloj() {
//    momentoActual = new Date()
//    hora = momentoActual.getHours()
//    minuto = momentoActual.getMinutes()
//    segundo = momentoActual.getSeconds()
//    hora = hora < 10 ? '0' + hora : hora;
//    segundo = segundo < 10 ? '0' + segundo : segundo;
//    minuto = minuto < 10 ? '0' + minuto : minuto;
//    horaImprimible = hora + ":" + minuto + ":" + segundo

//    //$('#ControlAsistencia_3__Hora').val(horaImprimible); 
//    //$('#' + id).val(horaImprimible); 
//    ListaIdsHora.forEach(function (elemento, indice, array) {
//        //console.log(elemento, indice);
//        $('#' + elemento).val(horaImprimible); 
//    });
//    setTimeout("mueveReloj()", 1000)
//}
//$(window).on('load', function () {

//});
function toDate(dStr, format) {
    //**
    var pieces = dStr.split(':'),
    hour, minute, second;

   
        hour = parseInt(pieces[0], 10);
        minute = parseInt(pieces[1], 10);
        //second = parseInt(pieces[2], 10);
  
   
    var fecha = new Date(2019, 10, 12, hour, minute);
    //alert(fecha);
    //**
   
    if (format == "h:m") {
        return fecha;
    } else
        return "Invalid Format";
}
function buscarenTabla() {
    // Declare variables
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("busqueda");
    filter = input.value.toUpperCase();
    table = document.getElementById("TableCuchillos");
    tr = table.getElementsByTagName("tr");

    // Loop through all table rows, and hide those who don't match the search query
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[2];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}
function ConsultarSiExisteAsistencia() {
    
    if ($('#TurnoGen').prop('selectedIndex') == 0) {
        $('#GenerarAsistencia').hide();
        MensajeError("Debe seleccionar un turno", false);
    } else {

    
    $('#PartialAsistencia').empty();
    $.ajax({
        //contentType: "application/json; charset=utf-8",
        url: '../Asistencia/ConsultarExistenciaAsistencia',
        type: "POST",
        data: {
            Turno: $('#TurnoGen').val()
        },
        success: function (resultado) {
            $('#Existe').val(resultado);

            if (resultado == 0)
            {
                $('#GenerarAsistencia').show();
                
            }
            if (resultado == 1)
            {
                GenerarAsistenciaDiaria($('#CodLinea').val(), resultado);
                $('#GenerarAsistencia').hide();
            }
        },
        error: function (result) {
            Console.log(result);
            //MensajeError(result, false);
        }
        });
    }
}
function ConsultarBiometrico(fila, cedula) {
    //alert("hola");
    var indice = fila - 1;
    $.ajax({
        url: '../Asistencia/ConsultarBiometrico',
        type: 'POST',
        dataType: "json",
        data: {
            Cedula: cedula
        },
        success: function (resultado) {
            if (resultado)
            {
                $("#" + fila).css("background", "transparent");
                $('#' + fila + ' :input').prop("disabled", false);
                $('#actualizar-' + fila).hide();
                $('#ControlAsistencia_'+indice+'__Observacion').val("");
                
            }
            
        }
        ,
        error: function (result) {
            Console.log(result);
            //MensajeError(result, false);
        }
    });
}
function PintarCHeck(fila) {
    $("#LabelAsistencia-" + fila).css("background", "green");
    $("#CheckAsistencia-" + fila).prop('checked', true);
}
function DeshabilitarControles(fila) {
    $('#'+fila+' :input').prop("disabled", true);
}
function GenerarAsistenciaDiaria(IdLinea, bandera) {
    MostrarModalCargando();
    //console.log("hola");
    var turno;
    if (bandera == 0) {
        $('#GenerarAsistencia').prop("disabled", true);
        
    }
    turno = $('#TurnoGen').val();
    //else {
    //    turno = $('#TurnoCons').val();
    //}
    $.ajax({
        url: '../Asistencia/AsistenciaPartial',
        type: 'POST',
        data: {
            CodLinea: IdLinea,
            BanderaExiste: bandera,
            Turno: turno
        },
        success: function (resultado) {
            //MensajeCorrecto(resultado, true);
            CerrarModalCargando();
            $('#PartialAsistencia').html(resultado);
            $('#GenerarAsistencia').hide();
           
            if (bandera == 0) {
                $('#GenerarAsistencia').prop("disabled", false);
            }
        }
        ,
        error: function (result) {
            console.log(result);
            CerrarModalCargando();
            
            MensajeError(result.responseJSON, false);
            if (bandera == 0) {
                $('#GenerarAsistencia').prop("disabled", false);
            }
        }
    });
}
//guardar con check
function GuardarPersona(fila, nombre, ComboOCheck) {
    //**
    //console.log('change');
    var banderaChangesinCheck = false;
    //**
    var valor = fila - 1;
    var d = new Date();
    var indice = fila;
    //var hora = d.getHours();
    var hora = toDate($('#ControlAsistencia_' + valor + '__Hora').val(), "h:m");
    //alert(hora);  LabelAsistencia-
    //** 
   
    if (ComboOCheck != 'change')
    { 
    //**
    $('#CheckAsistencia-' + indice).prop("disabled", true);
    $('#ControlAsistencia_' + valor + '__EstadoAsistencia').prop("disabled", true);
    //**
    }
    //**
    if (ComboOCheck == 'check')
    {
        if ($('#TurnoGen').val() == '1') {
            if (hora > toDate("07:00", "h:m")) {
                $('#ControlAsistencia_' + valor + '__EstadoAsistencia').val('2');
            }
            if (hora <= toDate("07:00", "h:m")) {
                $('#ControlAsistencia_' + valor + '__EstadoAsistencia').val('1');
            }
        }
        if ($('#TurnoGen').val() == '2') {
                if (hora > toDate("07:00", "h:m")) {
                    $('#ControlAsistencia_' + valor + '__EstadoAsistencia').val('2');
                }
                if (hora <= toDate("07:00", "h:m")) {
                    $('#ControlAsistencia_' + valor + '__EstadoAsistencia').val('1');
                }
         }
    }
    if (ComboOCheck == 'combo')
    {
        if ($('#ControlAsistencia_' + valor + '__EstadoAsistencia').val() == '3')
        {
            $("#LabelAsistencia-" + fila).css("background", "transparent");
            $("#CheckAsistencia-" + fila).prop('checked', false);
        } else
        {
            PintarCHeck(fila);
        }
        
    }
    //***
    if (ComboOCheck == 'change' && $('#ControlAsistencia_' + valor + '__EstadoAsistencia').val()!='3')//para saber si se dispara el evento onchange de hora u observacion
    {
        $("#CheckAsistencia-" + fila).prop('checked', false);
    }
    
    if (ComboOCheck == 'change' && $('#ControlAsistencia_' + valor + '__EstadoAsistencia').val() == '3')
    {
        banderaChangesinCheck = true;
    }
    //**
    if ($('#CheckAsistencia-' + fila).prop('checked'))
    {
        $("#LabelAsistencia-" + fila).css("background", "green");
        fila -= 1;
        $.ajax({
            url: '../Asistencia/GrabarAsistenciaEmpleado',
            type: 'POST',
            dataType: "json",
            data: {
                cedula: $('#ControlAsistencia_' + fila + '__Cedula').val(),
                //nombre: $('#ControlAsistencia_' + fila + '__NOMBRES').val(),
                nombre: nombre,
                Hora: $('#ControlAsistencia_' + fila + '__Hora').val(),
                observacion: $('#ControlAsistencia_' + fila + '__Observacion').val(),
                estado: $('#ControlAsistencia_' + fila + '__EstadoAsistencia').val()
            },
            success: function (resultado) {
                //MensajeCorrecto(resultado, true);
                $('#CheckAsistencia-' + indice).prop("disabled", false);
                $('#ControlAsistencia_' + valor + '__EstadoAsistencia').prop("disabled", false);
            }
            ,
            error: function (resultado) {
                //MensajeError(resultado, false);

                $('#CheckAsistencia-' + indice).prop("disabled", false);
                $('#ControlAsistencia_' + valor + '__EstadoAsistencia').prop("disabled", false);
                $("#LabelAsistencia-" + indice).css("background", "transparent");
                $("#CheckAsistencia-" + indice  ).prop('checked', false);
            }
        });
    } else if (($('#CheckAsistencia-' + fila).prop('checked') == false) && banderaChangesinCheck==false) {

        $("#LabelAsistencia-" + fila).css("background", "transparent");
        fila -= 1;
        $('#ControlAsistencia_' + fila + '__EstadoAsistencia').val('3');
        $.ajax({
            url: '../Asistencia/CambiarAsistenciaEmpleadoFalta',
            type: 'POST',
            dataType: "json",
            data: {
                cedula: $('#ControlAsistencia_' + fila + '__Cedula').val()
            },
            success: function (resultado) {
                //MensajeCorrecto(resultado, true);
                $('#CheckAsistencia-' + indice).prop("disabled", false);
                $('#ControlAsistencia_' + valor + '__EstadoAsistencia').prop("disabled", false);
            }
            ,
            error: function (resultado) {
                $('#CheckAsistencia-' + indice).prop("disabled", false);
                $('#ControlAsistencia_' + valor + '__EstadoAsistencia').prop("disabled", false);
                PintarCHeck(indice);
                MensajeError(resultado, false);
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

