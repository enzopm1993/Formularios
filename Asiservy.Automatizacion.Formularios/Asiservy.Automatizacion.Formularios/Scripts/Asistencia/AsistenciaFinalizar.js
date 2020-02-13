function Nuevo() {
    var now = new Date();

    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);

    var today = now.getFullYear() + "-" + (month) + "-" + (day);

    $("#txtFecha").val(today);
    $("#txtFecha").prop('disabled', false);
    $("#TurnoGen").prop('disabled', false);
    $("#TurnoGen").prop('selectedIndex', 0);
    $('#PartialAsistenciaFin').empty();
    $('#MensajeAsistenciaSalida').empty();
    
}
function ConsultarAsistenciaSalida(Linea) {
    $("#spinnerCargando").prop("hidden", false);
    if ($('#TurnoGen').prop('selectedIndex') == 0) {
        $('#mensajeturno').show();
        return false;
    } else {
        $('#mensajeturno').hide();
    }
    if ($('#txtFecha').val() == '') {
        $('#mensajefecha').show();
        return false;
    } else {
        $('#mensajefecha').hide();
    }
    
    $("#txtFecha").prop('disabled', true);
    $("#TurnoGen").prop('disabled', true);
    $.ajax({
        //contentType: "application/json; charset=utf-8",
        url: '../Asistencia/AsistenciaFinalizarPartial',
        type: "GET",
        data: {
            CodLinea: Linea,
            Turno: $('#TurnoGen').val(),
            Fecha: $('#txtFecha').val()
        },
        success: function (resultado) {
            //console.log(resultado);
            $("#spinnerCargando").prop("hidden", true);
            $('#PartialAsistenciaFin').empty();
            $('#PartialAsistenciaFin').html(resultado);
            if ($('#Nregistros').val() == 0) {
                $('#MensajeAsistenciaSalida').html('No existen registros para mostrar');
                $('#PartialAsistenciaFin').empty();
            } else {
                $('#MensajeAsistenciaSalida').html('');
            }
            if ($('#TurnoGen').val() == 2) {
                //console.log('show');
                $('#divfechafin').show();
            } else {
                $('#divfechafin').hide();
            }
        },
        error: function (result) {
            //Console.log(result);
            MensajeError(result, false);
        }
    });
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
function CheckSalida(fila) {
    //$("#LabelAsistencia-" + fila).css("background", "green");
    //console.log('heckSalida-'+fila);
    $("#CheckSalida-" + fila).prop('checked', true);
}
function FijarHora() {
    if ($('#FijarHora').val() == "") {
        $('#msgerrorfijarhora').show();
        return false;
    } else {
        $('#msgerrorfijarhora').hide();
    }
    $("#ModalHora").modal("hide");
    var numerofilas = ($('#TableCuchillos tr').length) - 2;
    for (var i = 0; i <= numerofilas; i++) {
        //console.log('#CheckSalida-' + parseInt(i + 1));
        if (!$('#CheckSalida-' + parseInt(i + 1)).is(':checked')) {
            $('#txtHorasalida'+i).val($('#FijarHora').val());
        }
    }

}

function SetearHora() {
    $('#btnhora').prop('disabled', 'disabled');
    $('#ModalHora').modal('show');
    $('#btnhora').removeAttr('disabled');
}
function GuardarSalida(Fila, Cedula, idMovimientoPersonalDiario,VieneDeHora) {
  
    //console.log(Fila);
    //console.log(Cedula);
    //console.log(idMovimientoPersonalDiario);
    //console.log(VieneDeHora);
    if (($('#TurnoGen').val() == 2) && ($('#txtFechaFin').val() == '')) {
        $('#mensajefechafin').show();
        $('#CheckSalida-' + (parseInt(Fila) + 1)).prop('checked', false);
        $('#txtFechaFin').focus();
        return false;
    } else {
        $('#mensajefechafin').hide();
    }
    var FechaFinalizacion;
    if (($('#TurnoGen').val() == 2)) {
        FechaFinalizacion = $('#txtFechaFin').val();
    } else {
        FechaFinalizacion = $('#txtFecha').val();
    }
    var psTipo = "";
    if ($('#CheckSalida-' + (parseInt(Fila)+1)).prop('checked')) {
        psTipo = "MarcarSalida";
    } else {
        psTipo = "DesmarcarSalida";
    }
    if (VieneDeHora == 'change' && ($('#CheckSalida-' + parseInt(Fila)).is(':checked'))) {
        psTipo = "DesmarcarSalida";
        $('#CheckSalida-' + parseInt(Fila)).prop('checked', false);
        //console.log(Fila);
    } else if (VieneDeHora == 'change' && !($('#CheckSalida-' + parseInt(Fila)).is(':checked'))){
        return false;
    }
    $.ajax({
        //contentType: "application/json; charset=utf-8",
        url: '../Asistencia/GuardarSalidaAsistencia',
        type: "POST",
        data: {
            Cedula: Cedula,
            Fecha: FechaFinalizacion,
            FechaGenAsistencia: $('#txtFecha').val(),
            Hora: $('#txtHorasalida' + Fila).val(),
            Tipo: psTipo,
            IdMovimiento: idMovimientoPersonalDiario,
            Turno: $('#TurnoGen').val(),
            CodLinea: $('#Linea').val()
        },
        success: function (resultado) {
            
            if (resultado == '2') {
                
                MensajeAdvertencia('La hora de salida no puede ser menor a la de ingreso', false);
                $('#CheckSalida-' + (parseInt(Fila) + 1)).prop('checked', false);
            }
        },
        error: function (result) {
            //Console.log(result);
            $('#CheckSalida-' + parseInt(Fila) + 1).prop('checked', false);
            MensajeError(result, false);

        }
    });
}