function FijarHora() {
    if ($('#FijarHora').val() == "") {
        $('#msgerrorfijarhora').show();
        return false;
    } else {
        $('#msgerrorfijarhora').hide();
    }
    $("#ModalHora").modal("hide");
    var numerofilas = ($('#TableCuchillos tr').length) - 3;
    for (var i = 0; i <= numerofilas; i++) {
        $('#ControlAsistencia_' + i + '__Hora').val
        if ($('#ControlAsistencia_' + i + '__EstadoAsistencia').val() == "3") {
            $('#ControlAsistencia_' + i + '__Hora').val($('#FijarHora').val());
        }

    }

}
function SetearHora() {
    $('#btnhora').prop('disabled', 'disabled');
    $.ajax({
        //contentType: "application/json; charset=utf-8",
        url: '../Asistencia/ModalHora',
        type: "GET",
        success: function (resultado) {
            $('#modalhoraasis').empty();
            $('#modalhoraasis').html(resultado);
            $("#ModalHora").modal("show");
            $('#btnhora').removeAttr('disabled');
        },
        error: function (result) {
            $('#btnhora').removeAttr('disabled');
            //Console.log(result);
            //MensajeError(result, false);
        }
    });
}
function Nuevo() {
    $('#GenerarAsistencia').hide();
    $('#TurnoGen').removeAttr('disabled');
    $('#txtFecha').removeAttr('disabled');
    $('#ConsultaAsistencia').removeAttr('disabled');
    $('#TurnoGen').prop('selectedIndex', 0);
    $('#PartialAsistencia').empty();


    var d = new Date();

    var dia = d.getDate();

    var mes = (d.getMonth() + 1) < 10 ? ("0" + (d.getMonth() + 1)) : d.getMonth() + 1;
    var anio = d.getFullYear();

    var fechatotal = anio + "-" + mes + "-" + dia
    $('#txtFecha').val(fechatotal);


}
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
        $('#mensajeturno').show();
        return false;
    } else {
        $('#mensajeturno').hide();
    }
    if ($('#txtFecha').val() == "") {
        $('#mensajefecha').show();
        return false;
    } else {
        $('#mensajefecha').hide();
    }
    $('#PartialAsistencia').empty();
        $('#PartialAsistencia').empty();
        $.ajax({
            //contentType: "application/json; charset=utf-8",
            url: '../Asistencia/ConsultarExistenciaAsistenciaGeneral',
            type: "POST",
            data: {
                Turno: $('#TurnoGen').val(),
                Fecha: $('#txtFecha').val()
            },
            success: function (resultado) {
                $('#Existe').val(resultado);

                if (resultado == 0) {
                    $('#GenerarAsistencia').show();
                    $('#TurnoGen').prop('disabled', 'disabled');
                    $('#txtFecha').prop('disabled', 'disabled');
                    $('#ConsultaAsistencia').prop('disabled', 'disabled');
                }
                if (resultado == 1) {
                    GenerarAsistenciaDiariaGeneral($('#CodLinea').val(), resultado);
                    $('#GenerarAsistencia').hide();
                }
            },
            error: function (result) {
                //Console.log(result);
                //MensajeError(result, false);
            }
        });
 
}
function GenerarAsistenciaDiariaGeneral(IdLinea, bandera) {
    $("#spinnerCargando").prop("hidden", false);
    //console.log("hola");
    var turno;
    if (bandera == 0) {
        $('#GenerarAsistenciaMovidos').prop("disabled", true);
        $('#GenerarAsistencia').hide();
    }
    turno = $('#TurnoGen').val();
    $.ajax({
        url: '../Asistencia/AsistenciaGeneralPartial',
        type: 'POST',
        data: {
            CodLinea: IdLinea,
            BanderaExiste: bandera,
            Turno: turno,
            Fecha: $('#txtFecha').val()
        },
        success: function (resultado) {
            //MensajeCorrecto(resultado, true);
            $("#spinnerCargando").prop("hidden", true);
            $('#PartialAsistencia').html(resultado);
            $('#GenerarAsistenciaMovidos').hide();

            if (bandera == 0) {
                $('#GenerarAsistenciaMovidos').prop("disabled", false);
            }
        }
        ,
        error: function (result) {

            CerrarModalCargando();
            //Console.log(result);
            //MensajeError(result, false);
            if (bandera == 0) {
                $('#GenerarAsistenciaMovidos').prop("disabled", false);
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

    if (ComboOCheck != 'change') {
    //**
    $('#CheckAsistencia-' + indice).prop("disabled", true);
    $('#ControlAsistencia_' + valor + '__EstadoAsistencia').prop("disabled", true);

        //**
    }
    //**
    if (ComboOCheck == 'check') {
        if ($('#TurnoGen').val() == '1') {
            if (hora > toDate("07:00", "h:m")){
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
    if (ComboOCheck == 'combo') {
        if ($('#ControlAsistencia_' + valor + '__EstadoAsistencia').val() == '3') {
            $("#LabelAsistencia-" + fila).css("background", "transparent");
            $("#CheckAsistencia-" + fila).prop('checked', false);
        } else {
            PintarCHeck(fila);
        }

    }
    //***
    if (ComboOCheck == 'change' && $('#ControlAsistencia_' + valor + '__EstadoAsistencia').val() != '3')//para saber si se dispara el evento onchange de hora u observacion
    {
        $("#CheckAsistencia-" + fila).prop('checked', false);
    }

    if (ComboOCheck == 'change' && $('#ControlAsistencia_' + valor + '__EstadoAsistencia').val() == '3') {
        banderaChangesinCheck = true;
    }
    //**
    if ($('#CheckAsistencia-' + fila).prop('checked')) {
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
                estado: $('#ControlAsistencia_' + fila + '__EstadoAsistencia').val(),
                Fecha: $('#txtFecha').val()
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
                $("#CheckAsistencia-" + indice).prop('checked', false);
            }
        });
    } else if (($('#CheckAsistencia-' + fila).prop('checked') == false) && banderaChangesinCheck == false){

        $("#LabelAsistencia-" + fila).css("background", "transparent");
        fila -= 1;
        $('#ControlAsistencia_' + fila + '__EstadoAsistencia').val('3');
        $.ajax({
            url: '../Asistencia/CambiarAsistenciaEmpleadoFalta',
            type: 'POST',
            dataType: "json",
            data: {
                cedula: $('#ControlAsistencia_' + fila + '__Cedula').val(),
                Fecha: $('#txtFecha').val()
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
//LAST AAGREGADOS
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
            if (resultado) {
                $("#" + fila).css("background", "transparent");
                $('#' + fila + ' :input').prop("disabled", false);
                $('#actualizar-' + fila).hide();
                $('#ControlAsistencia_' + indice + '__Observacion').val("");

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
    $('#' + fila + ' :input').prop("disabled", true);
}


//METODOS PARA CUCHILLOS
function check(id, color, cedula) {
    // alert(id);
    console.log(id);
    console.log(color);
    console.log(cedula);
    //7b8a8b
    var estado = "1";
    if (id == "") {
        GuardarControlCuchillo(cedula, color, id, estado, true);
    } else {
        GuardarControlCuchillo(cedula, color, id, estado, false);
    }
}
function GuardarControlCuchillo(cedula, color, numero, estado, check) {
    $.ajax({
        url: "../Asistencia/GuardarControlCuchillo",
        type: "GET",
        data: {
            dsCedula: cedula,
            dsColor: color,
            dsNumero: numero,
            dsEstado: estado,
            dbCheck: check
        },
        success: function (resultado) {
            //alert(resultado);
            //if (resultado = "No es posible asignar el cuchillo, por que ya ha sido prestado")
            //{
            //    MensajeError(resultado, false);
            //}
        },
        error: function (resultado) {
            //console.log(resultado.responseJSON);
            MensajeError(resultado.responseJSON + "", false);
            if (color == "B") {
                $('#Blanco' + cedula).prop('selectedIndex', 0);
            }
            if (color == "R") {
                $('#Rojo' + cedula).prop('selectedIndex', 0);
            }
            if (color == "N") {
                $('#Rojo' + cedula).prop('selectedIndex', 0);
            }

        }
    });
}
//FIN METODOS PARA CUCHILLOS