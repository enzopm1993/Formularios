var Com = [];
$(document).ready(function () {
    ConsultaComunicados();
    console.log(vacacion);

    $("#TotalDiasVacciones").html(vacacion.TotalDias);
    var options = {
        series: [vacacion.DiasTomados,vacacion.Saldo],
        chart: {
            type: 'donut',
            height: 150,
        },

        fill: {
            colors: ['#FF0000', '#00FF3E']
        },
        colors: ['#FF0000', '#00FF3E'],
        labels: [ 'Días Tomados', 'Días Disponibles'],
        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    width: 200
                },
                legend: {
                    position: 'bottom'
                }
            }
        }]
    };
    var chart = new ApexCharts(document.querySelector("#divOtros"), options);
    chart.render();

});

function ConsultaComunicados() {
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../Home/ConsultaComunicados",
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divComunicados").html('')
                return;
            }
            $("#divComunicados").html(resultado);
            $("#divComunicados").css("height", "100px");
            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError("Error, comuniquese con sistemas. "+resultado.responseText, false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}

function SeleccionarComunicado(Contenido) {
    $("#modalBodyComunicados").html('');
    $("#modalBodyComunicados").html(Contenido);
    $("#ModalComunicado").modal("show");
}
