var ListadoGeneral = [];

var options = {
    series: [
        {
            name: "High - 2013",
            data: [28, 29, 33, 36, 32, 32, 33]
        },
        {
            name: "Low - 2013",
            data: [12, 11, 14, 18, 17, 13, 13]
        }
    ],
    chart: {
        height: 350,
        type: 'line',
        dropShadow: {
            enabled: true,
            color: '#000',
            top: 18,
            left: 7,
            blur: 10,
            opacity: 0.2
        },
        toolbar: {
            show: false
        }
    },
    colors: ['#005FFF', '#70F7D7'],
    dataLabels: {
        enabled: true,
    },
    stroke: {
        curve: 'smooth'
    },
    
    grid: {
        borderColor: '#e7e7e7',
        row: {
            colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
            opacity: 0.5
        },
    },
    markers: {
        size: 1
    },
    xaxis: {
        categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
        title: {
            text: 'Month'
        }
    },
    yaxis: {
        title: {
            text: 'Temperature'
        }
    },
    legend: {
        position: 'top',
        horizontalAlign: 'right',
        floating: true,
        offsetY: -25,
        offsetX: -5
    }
};


var chartRendimiento = new ApexCharts(document.querySelector("#chartRendimiento"), options);
chartRendimiento.render();


$(document).ready(function () {
    CargarReporteAvance();
});


function CargarReporteAvance() {
    $("#selectLinea").prop("selectedIndex", 0);
    var txtFecha = $('#txtFecha').val();
    if (txtFecha == "") {
        MensajeAdvertencia("Igrese una Fecha.");
        return;
    }
    if ($("#selectTurno").val() == "") {
        MensajeAdvertencia("Seleccione un turno.");
        return;
    }
    $('#btnConsultar').prop("disabled", true);
    $('#DivTableReporteControlAvance').html("");
    $('#divMensaje').html("");
    
    MostrarModalCargando();
    $.ajax({
        url: "../Hueso/ReporteRendimientoLotePartial",
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


            } else {
                $("#spinnerCargando").prop("hidden", true);
                $('#DivTableReporteControlAvance').html(resultado);
                config.opcionesDT.pageLength = 50;
                config.opcionesDT.order = [[0, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);
                $("#divChart").prop("hidden", false);

                var Real=[];
                var Estandar = [];
                var Lote = [];
                //console.log(ListadoGeneral);
                ListadoGeneral.forEach(function (row, i) {
                    Real[i] = row.KiloRealLomoPorcentaje + row.KiloRealMigaPorcentaje;
                    Estandar[i] = row.KiloStdLomoPorcentaje + row.KiloStdMigaPorcentaje;
                    Lote[i] = row.Lote;


                });
                //console.log(Real);
                //console.log(Estandar);

                var _serie = [{
                        name: "Real",
                        data: Real
                    },
                    {
                        name: "Estandar",
                        data: Estandar
                    }];

                chartRendimiento.updateSeries(_serie)
                chartRendimiento.updateOptions({
                     xaxis: {
                    categories: Lote,
                    title: {
                        text: 'Lote'
                    }
                    },
                    yaxis: {
                        title: {
                            text: 'Rendimiento %'
                        }
                    }
                    })


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