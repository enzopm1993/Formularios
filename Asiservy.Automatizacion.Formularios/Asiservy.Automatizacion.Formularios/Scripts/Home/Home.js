var Com = [];
var vacacion = [];
$(document).ready(function () {
    ConsultaComunicados();
    //console.log(Vacaciones);
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

var options = {
    series: [118.50, 5.00, 113.50],
    chart: {
        type: 'donut',
        height: 150,
    },

    fill: {
        colors: ['#0064FF', '#CCCAC9', '#00FF3E']
    },
    colors: ['#0064FF', '#CCCAC9', '#00FF3E'],
    labels: ['Dias Totales', 'Dias Tomados', 'Dias Disponobles'],
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
