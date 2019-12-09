function ConsultarAsistenciaSalida(Linea) {
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
            $('#PartialAsistenciaFin').empty();
            $('#PartialAsistenciaFin').html(resultado);
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
    console.log('heckSalida-'+fila);
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
        
        if ($('#CheckSalida-' + i + 1).prop("checked", false)) {
            $('#txtHorasalida'+i).val($('#FijarHora').val());
        }
    }

}

function SetearHora() {
    $('#btnhora').prop('disabled', 'disabled');
    $('#ModalHora').modal('show');
    $('#btnhora').removeAttr('disabled');
}
function GuardarSalida(Fila,Cedula,idMovimientoPersonalDiario) {
    console.log(Fila);
    var psTipo = "";
    if ($('#CheckSalida' + (parseInt(Fila)-1)).prop('checked')) {
        psTipo = "MarcarSalida";
    } else {
        psTipo = "DesmarcarSalida";
    }
    $.ajax({
        //contentType: "application/json; charset=utf-8",
        url: '../Asistencia/GuardarSalidaAsistencia',
        type: "POST",
        data: {
            Cedula: Cedula,
            Fecha: $('#txtFecha').val(),
            Hora: $('#txtHorasalida' + Fila).val(),
            Tipo: psTipo,
            IdMovimiento: idMovimientoPersonalDiario,
            Turno: $('#TurnoGen').val(),
            CodLinea: $('#Linea').val()
        },
        success: function (resultado) {
            if (resultado == '2') {
                $('#CheckSalida' + parseInt(Fila) + 1).prop('checked', false);
                MensajeAdvertencia('La hora de salida no puede ser menor a la de ingreso', false);
            }
        },
        error: function (result) {
            //Console.log(result);
            $('#CheckSalida' + parseInt(Fila) + 1).prop('checked', false);
            MensajeError(result, false);

        }
    });
}