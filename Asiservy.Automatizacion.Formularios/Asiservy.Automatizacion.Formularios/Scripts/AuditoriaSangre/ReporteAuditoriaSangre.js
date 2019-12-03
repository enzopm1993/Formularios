$(document).ready(function () {
    var fecha = new Date(); //Fecha actual
    var mes = fecha.getMonth() + 1; //obteniendo mes
    var dia = fecha.getDate(); //obteniendo dia
    var ano = fecha.getFullYear(); //obteniendo año
    if (dia < 10)
        dia = '0' + dia; //agrega cero si el menor de 10
    if (mes < 10)
        mes = '0' + mes //agrega cero si el menor de 10
    document.getElementById('Fecha').value = ano + "-" + mes + "-" + dia;
    ConsultarReporteAuditoriaSangre();


});

function ConsultarReporteAuditoriaSangre() {
    if ($('#Fecha').val() == "") {       
        $("#Fecha").css("border-color", "#DC143C");
        return;
    } else {
        $("#Fecha").css('border-color', '#d1d3e2');

        $("#spinnerCargando").prop("hidden", false);
        $('#DivReporteSangre').html('');

        $('#ConsultarRptAuditoriaSangre').attr("disabled", true);
        $.ajax({
            url: "../AuditoriaSangre/ReporteAuditoriaSangrePArtial",
            type: "GET",
            data:
            {
                CodLinea: $('#Lineas').val(),
                Fecha: $('#Fecha').val(),
                Tipo: $("#TipoAuditoria").val()
            },
            success: function (resultado) {
                if (resultado == "0") {
                    $('#DivReporteSangre').html('<div class="text-center"><h4>No Existen Registros</h4></div>');
                } else {
                    $('#DivReporteSangre').html(resultado);
                    
                    config.opcionesDT.pageLength = 15;
                    config.opcionesDT.order = [[1, "asc"]];
                    $('#tblDataTable').DataTable(config.opcionesDT);
                }
                $("#spinnerCargando").prop("hidden", true);
                $('#ConsultarRptAuditoriaSangre').attr("disabled", false);

            },
            error: function (resultado) {
                MensajeError(JSON.stringify(resultado), false);
                $("#spinnerCargando").prop("hidden", true);
                $('#ConsultarRptAuditoriaSangre').attr("disabled", false);
            }
        });
    }

    
}

