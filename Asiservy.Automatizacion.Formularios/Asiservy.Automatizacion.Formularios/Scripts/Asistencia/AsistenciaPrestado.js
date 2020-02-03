function CerrarModalPrestadoInfo() {
    $("#modalprestados").modal("hide");
}
function GenerarAsistenciaPrestadosOk() {
    $("#modalprestados").modal("hide");
    $("#horaservidor").hide();
    $("#lblHoraServidor").hide();

    GenerarAsistenciaDiariaMovidos($('#LineaPres').val(), $('#banderapres').val());
    console.log($('#LineaPres').val());
    console.log($('#banderapres').val());
}
function ActualizaObservacionHide() {
    var posiciono = $('#PosicionHide').val();
    $('#ControlAsistencia_' + posiciono + '__Observacion').val($('#areaobservacion').val());
    //"GuardarPersona(" + @cont + ",'" + @item.NOMBRES + "','change');"
    //var posicionint =parseInt(posiciono,10) + 1;
    //alert(parseInt(posiciono, 10));
    GuardarPersona(parseInt(posiciono, 10) + 1, '', 'change');
    $('#ModalObservacion').modal('hide');
}
function CargarObservacion(Posicion, bloquear) {
    $('#PosicionHide').val(Posicion);
    //console.log("ojo");
    //console.log(Posicion);
    //console.log($('#ControlAsistencia_' + Posicion + '__Observacion').val());
    $('#areaobservacion').val($('#ControlAsistencia_' + Posicion + '__Observacion').val());
    if (bloquear == 1) {
        $('#areaobservacion').prop('disabled', 'disabled');
        $('#GuardarObservacion').prop('disabled', 'disabled');
    } else {
        $('#areaobservacion').removeAttr('disabled');
        $('#GuardarObservacion').removeAttr('disabled');
    }
}

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

        //$('#ControlAsistencia_' + i + '__Hora').val
        if ($('#ControlAsistencia_' + i + '__EstadoAsistencia').val() == "3") {
            $('#ControlAsistencia_' + i + '__Hora').val($('#FijarHora').val());
        }

    }

}
function SetearHora() {
    $('#btnhora').prop('disabled', 'disabled');
    $('#ModalHora').modal('show');
    $('#btnhora').removeAttr('disabled');
}
function Nuevo() {


    $('#horaservidor').hide();
    $('#lblHoraServidor').hide();
    $('#mensajepersonal').hide();
    $('#GenerarAsistencia').hide();
    $('#TurnoGen').removeAttr('disabled');
    $('#txtFecha').removeAttr('disabled');
    $('#ConsultaAsistencia').removeAttr('disabled');
    $('#TurnoGen').prop('selectedIndex', 0);
    $('#PartialAsistencia').empty();
    //$('#TurnoGen').prop('disabled', false);
    //$('#txtFecha').prop('disabled', false);

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
        $('#horaservidor').hide();
        $('#lblHoraServidor').hide();
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
    $('#TurnoGen').prop('disabled', true);
    $('#txtFecha').prop('disabled', true);

    $("#spinnerCargando").prop("hidden", false);
    $('#PartialAsistencia').empty();
    $.ajax({
        //contentType: "application/json; charset=utf-8",
        url: '../Asistencia/ConsultarExistenciaAsistenciaPrestados',
        type: "POST",
        data: {
            Turno: $('#TurnoGen').val(),
            Fecha: $('#txtFecha').val()
        },
        success: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            if (resultado == '333') {
                MensajeAdvertencia('La fecha no puede ser mayor a hoy', false);
                $('#TurnoGen').prop('disabled', false);
                $('#txtFecha').prop('disabled', false);
                return false;
            }
            $('#Existe').val(resultado);

            if (resultado == 0) {
                $('#horaservidor').show();
                $('#lblHoraServidor').show();
                $('#GenerarAsistencia').show();
                $('#TurnoGen').prop('disabled', 'disabled');
                $('#txtFecha').prop('disabled', 'disabled');
                $('#ConsultaAsistencia').prop('disabled', 'disabled');

            }
            if (resultado == 1) {
                $('#horaservidor').hide();
                $('#lblHoraServidor').hide();
                GenerarAsistenciaDiariaMovidos($('#CodLinea').val(), resultado);
                $('#GenerarAsistencia').hide();
            }
        },
        error: function (result) {
            Console.log(result);
            //MensajeError(result, false);
        }
    });

}
function VerificarMovidosAMiLinea(IdLinea, bandera) {

    $('#mensajepersonal').hide();
    $('#LineaPres').val(IdLinea);
    if ($('#horaservidor').val() == '') {
        MensajeAdvertencia('Debe ingresar la hora');
        return false;
    }
    $("#spinnerCargando").prop("hidden", false);
    $('#GenerarAsistencia').hide();
    $('#horaservidor').hide();
    $('#lblHoraServidor').hide();
    $('#banderapres').val(bandera);
    $.ajax({
        //url: '../Asistencia/VerificarPrestados',
        url: '../Asistencia/ModalMovidosaMiLinea',
        type: 'GET',
        data: {
            //CodLinea: IdLinea,
            //BanderaExiste: bandera,
            //Turno: turno,
            Fecha: $('#txtFecha').val(),
            Hora: $('#horaservidor').val()

        },
        success: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            $('#GenerarAsistencia').show();
            $('#horaservidor').show();
            $('#lblHoraServidor').show();
            $('#divmodalprestados').html(resultado);

            if ($('#txtPrestado').val() == 'true') {
                $('#modalprestados').modal("show");
                $('#LineaPres').val(IdLinea);
                $('#banderapres').val(bandera);

            } else {
                //GenerarAsistenciaDiariaMovidos(IdLinea, bandera);
                $('#MensajeModalPrestado').html('No existe personal prestado a otra línea');
                $('#mensajepersonal').show();
                $('#LineaPres').val(IdLinea);
                $('#banderapres').val(bandera);
            }
            //if (resultado) {
            //    $('#modalprestados').modal("show");
            //    $('#LineaPres').val(IdLinea);
            //    $('#banderapres').val(bandera);

            //} else {
            //    GenerarAsistenciaDiaria(IdLinea, bandera);
            //}

        }
        ,
        error: function (result) {
            MensajeError(result.responseText, false);
        }
    });
}
function GenerarAsistenciaDiariaMovidos(IdLinea, bandera) {
    $("#spinnerCargando").prop("hidden", false);
    //console.log("hola");
    if (bandera == 0) {
        $('#GenerarAsistenciaMovidos').prop("disabled", true);
        $('#GenerarAsistencia').hide();
    }
    turno = $('#TurnoGen').val();
    $.ajax({
        url: '../Asistencia/AsistenciaPrestadoPartial',
        type: 'POST',
        data: {
            CodLinea: IdLinea,
            BanderaExiste: bandera,
            Turno: turno,
            Fecha: $('#txtFecha').val(),
            Hora: $('#horaservidor').val()
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

            $("#spinnerCargando").prop("hidden", true);
            //Console.log(result);
            //MensajeError(result, false);
            if (bandera == 0) {
                $('#GenerarAsistenciaMovidos').prop("disabled", false);
            }
        }
    });
}

//guardar con check
function GuardarPersona(fila, nombre, ComboOCheck, CentroCostos, Recurso, Linea, Cargo) {
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
            if (hora > toDate($('#HoraControlAsistencia').val(), "h:m")) {
                $('#ControlAsistencia_' + valor + '__EstadoAsistencia').val('2');
            }
            if (hora <= toDate($('#HoraControlAsistencia').val(), "h:m")) {
                $('#ControlAsistencia_' + valor + '__EstadoAsistencia').val('1');
            }
        }
        if ($('#TurnoGen').val() == '2') {
            if (hora > toDate($('#HoraControlAsistencia').val(), "h:m")) {
                $('#ControlAsistencia_' + valor + '__EstadoAsistencia').val('2');
            }
            if (hora <= toDate($('#HoraControlAsistencia').val(), "h:m")) {
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
                Fecha: $('#txtFecha').val(),
                CentroCostos: CentroCostos,
                Recurso: Recurso,
                Linea: Linea,
                Cargo: Cargo,
                Turno: $('#TurnoGen').val()
            },
            success: function (resultado) {
                //MensajeCorrecto(resultado, true);
                $('#CheckAsistencia-' + indice).prop("disabled", false);
                //$('#ControlAsistencia_' + valor + '__EstadoAsistencia').prop("disabled", false);
            }
            ,
            error: function (resultado) {
                //MensajeError(resultado, false);

                $('#CheckAsistencia-' + indice).prop("disabled", false);
                //$('#ControlAsistencia_' + valor + '__EstadoAsistencia').prop("disabled", false);
                $("#LabelAsistencia-" + indice).css("background", "transparent");
                $("#CheckAsistencia-" + indice).prop('checked', false);
            }
        });
    } else if (($('#CheckAsistencia-' + fila).prop('checked') == false) && banderaChangesinCheck == false) {

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
                //$('#ControlAsistencia_' + valor + '__EstadoAsistencia').prop("disabled", false);
            }
            ,
            error: function (resultado) {
                $('#CheckAsistencia-' + indice).prop("disabled", false);
                //$('#ControlAsistencia_' + valor + '__EstadoAsistencia').prop("disabled", false);
                PintarCHeck(indice);
                MensajeError(resultado, false);
            }
        });
    }

}
//LAST AAGREGADOS
function ConsultarBiometrico(fila, cedula) {
    //alert("hola");
    $('#actualizar-' + fila).prop('hidden', true);
    $('#actualizar2-' + fila).prop('hidden', false);
    var indice = fila - 1;
    $.ajax({
        url: '../Asistencia/ConsultarBiometrico',
        type: 'POST',
        dataType: "json",
        data: {
            Cedula: cedula,
            Fecha: $('#txtFecha').val()
        },
        success: function (resultado) {

            $('#actualizar-' + fila).prop('hidden', false);
            $('#actualizar2-' + fila).prop('hidden', true);
            if (resultado.Marcacion && $('#ControlAsistencia_' + indice + '__Hora').val() != '' && $('#CheckAsistencia-' + fila).prop('checked') == false) {
                $("#" + fila).css("background", "white");
                $('#' + fila + ' :input').prop("disabled", false);
                //$('#actualizar-' + fila).hide();
                $('#ControlAsistencia_' + indice + '__Observacion').val("");
                $('#ControlAsistencia_' + indice + '__Hora').val(moment(resultado.Hora).format("hh:mm"));
            }
            $('#ControlAsistencia_' + indice + '__EstadoAsistencia').prop('disabled', true)
            $('#Rojo0').prop('disabled', true);
            $('#Blanco0').prop('disabled', true);

        }
        ,
        error: function (result) {
            $('#Actualizar-' + fila).prop('hidden', false);
            $('#Actualizar2-' + fila).prop('hidden', true);
            //Console.log(result);
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
function GuardarModificarCuchilloEmpleadoPrestado(NumeroCuchillo, Color, Cedula,id, cont) {

    var sPath = window.location.pathname;
    var sPage = sPath.substring(sPath.lastIndexOf('/') + 1);
    //alert(sPage);
    if (sPage == "AsistenciaPrestado") {
        if (!$('#CheckAsistencia-' + cont).prop('checked')) {
            $('#' + id).prop('selectedIndex', 0);
            return false;
        }
    }
    //if (!Validar()) {
    //    return;
    //}
    console.log(Color);
    //var NumeroCuchilloBlanco=null;
    //var NumeroCuchilloRojo=null;
    //var NumeroCuchilloNegro=null;
    //if (Color == 'B') {
    //    NumeroCuchilloBlanco = NumeroCuchillo;
    //}
    //if (Color == 'R') {
    //    NumeroCuchilloRojo = NumeroCuchillo;
    //}
    //if (Color == 'N') {
    //    NumeroCuchilloNegro = NumeroCuchillo;
    //}

    $.ajax({
        url: "../ControlCuchillo/EmpleadoCuchilloPrestado",
        type: "POST",
        data:
        {
            //Cedula: $("#selectEmpleado").val(),
            Cedula: Cedula,
            Fecha: $("#txtFecha").val(),
            //CuchilloBlanco: $("#txtCuchilloBlanco").val(),
            //CuchilloRojo: $("#txtCuchilloRojo").val(),
            //CuchilloNegro: $("#txtCuchilloNegro").val()
            CuchilloBlanco: $('#Blanco' + Cedula).val(),
            CuchilloRojo: $('#Rojo' + Cedula).val(),
            CuchilloNegro: $('#Negro' + Cedula).val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == '0') {
                MensajeAdvertencia("No se ha encontrado al empleado");
                return;
            }
            if (resultado == '1') {
                MensajeAdvertencia("Cuchillo ya ha sido asignado a otro empleado");
                if (Color = 'B') {
                    $('#Blanco' + Cedula).prop('selectedIndex', 0);
                }
                if (Color = 'R') {
                    $('#Rojo' + Cedula).prop('selectedIndex', 0);
                }
                if (Color = 'N') {
                    $('#Negro' + Cedula).prop('selectedIndex', 0);
                }
                return;
            }
            //CargarEmpleadoCuchilloPrestado();
            console.log(resultado);
            //MensajeCorrecto(resultado);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}
function check(id, color, cedula) {
    // alert(id);
    console.log(id);
    console.log(color);
    console.log(cedula);
    //7b8a8b
    var estado = "1";
    if (id == "") {
        //parametros cuchillo victor
        //  alert("seleccione");
        GuardarControlCuchillo(cedula, color, 1, estado, false);
        //***

        //GuardarControlCuchillo(cedula, color, id, estado, true);
    } else {
        //parametros cuchillo victor
        //     alert("cuchillo seleccionado");
        GuardarControlCuchillo(cedula, color, id, estado, true);
        //**

        //GuardarControlCuchillo(cedula, color, id, estado, false);
    }
}
//function GuardarControlCuchillo(cedula, color, numero, estado, check,idCheck,Observacion)
function GuardarControlCuchillo(cedula, color, numero, estado, check) {
    $.ajax({
        url: "../ControlCuchillo/GuardarControlCuchillo",
        type: "GET",
        data: {
            dsCedula: cedula,
            dsColor: color,
            dsNumero: numero,
            dsEstado: estado,
            dbCheck: check,
            ddFecha: $('#txtFecha').val(),
            dbTipo: true
        },
        success: function (resultado) {
            if (resultado.codigo == "1") {
                MensajeAdvertencia(resultado.descripcion);
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
            //console.log(resultado);
        },
        error: function (resultado) {
            //console.log(resultado.responseJSON);
            MensajeError(resultado.responseJSON + "", false);


        }
    });
}
//FIN METODOS PARA CUCHILLOS