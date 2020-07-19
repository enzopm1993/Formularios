
$(document).ready(function () {
    CargarReporteAvance();
});


function CargarReporteAvance() {
    var txtFecha = $('#txtFecha').val();
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }
    if ($("#selectTurno").val() == "") {
        $("#selectTurno").css('borderColor', '#FA8072');
        return;
    } else {
        $("#selectTurno").css('borderColor', '#ced4da');
    }
    $('#btnConsultar').prop("disabled", true);
    $('#DivTableReporteControlAvance').html("");
    $('#divMensaje').html("");

    MostrarModalCargando();
    $.ajax({
        url: "../Avance/ReporteRendimientoLineaPartial",
        type: "GET",
        data: {
            ddFecha: txtFecha,
            Turno: $("#selectTurno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $('#btnConsultar').prop("disabled", false);
            if (resultado == "1") {
                $('#divMensaje').html("No existen Registros");
                $("#spinnerCargando").prop("hidden", true);
                $("#divChart").prop("hidden", true);
                CerrarModalCargando();
                $("#btnImprimir").prop("hidden", true);

            } else {
                $("#spinnerCargando").prop("hidden", true);
                $('#DivTableReporteControlAvance').html(resultado);
                //config.opcionesDT.pageLength = 50;
                //config.opcionesDT.order = [[0, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);
                $("#divChart").prop("hidden", false);
                $("#btnImprimir").prop("hidden", false);

                CerrarModalCargando();
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            CerrarModalCargando();
        }
    });
}


function ModalDetalleLimpiadoras(resultado) {
    //console.log(resultado);
    $("#lblTitulo").html("LIMPIADORAS");
    var table = $('#tblTable');
    table.DataTable().clear();    
    $("#tblTable tbody").empty();
    config.opcionesDT.order = [];
    config.opcionesDT.columns = [
        { data: 'Linea' },
        { data: 'Hora' },
        { data: 'Limpiadoras' }
    ];


    resultado.forEach(function (row, i) {
        row.Hora = moment(row.Hora).format('DD-MM-YYYY HH:mm');
    });
    config.opcionesDT.pageLength = -1;
    table.DataTable().destroy();
    table.DataTable(config.opcionesDT);
    table.DataTable().rows.add(resultado);
    table.DataTable().draw();

    $("#ModalDetalle").modal("show");
}


function ModalDetalleDespellejadores(resultado) {
    $("#lblTitulo").html("DESPELLEJADORES");

    var table = $('#tblTable');
    table.DataTable().clear();
    $("#tblTable tbody").empty();
    config.opcionesDT.order = [];
    config.opcionesDT.columns = [
        { data: 'Linea' },
        { data: 'Hora' },
        { data: 'Despellejadores' }
    ];


    resultado.forEach(function (row, i) {
        row.Hora = moment(row.Hora).format('DD-MM-YYYY HH:mm');
    });
    config.opcionesDT.pageLength = -1;
    table.DataTable().destroy();
    table.DataTable(config.opcionesDT);
    table.DataTable().rows.add(resultado);
    table.DataTable().draw();

    $("#ModalDetalle").modal("show");
}



//function printDiv() {
//    window.print();
//}