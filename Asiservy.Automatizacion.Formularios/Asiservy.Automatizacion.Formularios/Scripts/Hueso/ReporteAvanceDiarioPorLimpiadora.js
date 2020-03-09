


function CargarReporteAvanceLimpiadora() {
    var txtFecha = $('#txtFecha').val();
    var selectLinea = $('#selectLinea').val();
    $('#btnConsultar').prop("disabled", true);
    $("#spinnerCargando").prop("hidden", false);
    var bitacora = $('#DivTableReporteControlAvancePorLimpiadora');
    bitacora.html('');
    $.ajax({
        url: "../Hueso/ReporteAvanceDiarioPorLimpiadoraPartial",
        type: "GET",
        data: {
            ddFecha: txtFecha,
            dsLinea: selectLinea
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            var bitacora = $('#DivTableReporteControlAvancePorLimpiadora');
            bitacora.html(resultado);

            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);

            config.opcionesDT.pageLength = 15;
            config.opcionesDT.order = [[1, "asc"]];
            $('#tblDataTable').DataTable(config.opcionesDT);

        },
        error: function (resultado) {
            MensajeError(resultado.responseJSON, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);

           
        }
    });

}


var Datos = [];
function SeleccionarLimpiadora(model) {
    $("#ModalKpi").modal("show");
    $("#divKpi").html("");
    $("#divKpi2").html("");
    Datos = model;
    ConsultaKpi();
}


function ConsultaKpi() {
    var txtFecha = $('#txtFecha').val();
    $.ajax({
        url: "../Hueso/ConsultaKpiAvanceLimpiadora",
        type: "GET",
        data: {
            Fecha: txtFecha,
            Cedula: Datos.Cedula
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            var Horas = [];
            var Avance = [];
           
            $.each(resultado, function (i, item) {
                // console.log(item.Hora);
                Horas[i] = moment(item.Hora).format("HH:mm");
                Avance[i] = item.Avance;
            });
            console.log(Horas);
            console.log(Avance);
            var options = {
                series: [{
                    name: "Desktops",
                    data: Avance
                }],
                chart: {
                    height: 350,
                    type: 'line',
                    zoom: {
                        enabled: false
                    }
                },
                dataLabels: {
                    enabled: false
                },
                stroke: {
                    curve: 'straight'
                },
                title: {
                    text: Datos.Nombre,
                    align: 'left'
                },
                grid: {
                    row: {
                        colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
                        opacity: 0.5
                    },
                },
                xaxis: {
                    categories: Horas,
                }
            };

            var chart = new ApexCharts(document.querySelector("#divKpi"), options);
            chart.render();


            var options = {
                series: [{
                    data: Avance
                }],
                chart: {
                    type: 'bar',
                    height: 350
                },
                plotOptions: {
                    bar: {
                        horizontal: true,
                    }
                },
                dataLabels: {
                    enabled: false
                },
                xaxis: {
                    categories: Horas,
                }
            };

            var chart = new ApexCharts(document.querySelector("#divKpi2"), options);
            chart.render();


        },
        error: function (resultado) {
            MensajeError("Error, Comuniquese con sistemas.", false);           

        }
    });
}