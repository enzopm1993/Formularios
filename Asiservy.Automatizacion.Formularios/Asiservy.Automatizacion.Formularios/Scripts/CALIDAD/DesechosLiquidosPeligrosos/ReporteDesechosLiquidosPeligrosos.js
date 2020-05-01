$(document).ready(function () {
    ComboAnio();
    CargarCabecera(3);    
});

function ComboAnio() {
    var date = new Date();
    document.getElementById("selectMonth").selectedIndex = moment(date).format('MM');
    var n = date.getFullYear();
    var select = document.getElementById("selectAnio");
    for (var i = n; i >= 2015; i--) {
        select.options.add(new Option(i, i));
    }
}

function CargarCabecera() {
    var op = 0;
    if ($("#selectMonth").val() == '13') {
        op = 3;
    } else {
        op = 1;
    }
    $('#cargac').show();
    $.ajax({
        url: "../DesechosLiquidosPeligrosos/ReporteDesechosLiquidosPeligrososPartial",
        data: {
            anioBusqueda: $("#selectAnio").val(),
            mesBusqueda: $("#selectMonth").val(),
            idDesechosLiquidos: 0,
            op: op
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMostarTablaCabecera").html("No existen registros");
            } else {
                $("#divMostarTablaCabecera").html(resultado);
            }
            itemEditar = 0;
            setTimeout(function () {
                $('#cargac').hide();
            }, 200);
        },
        error: function (resultado) {
            CerrarModalCargando();
            MensajeError(resultado.responseText, false);
        }
    });
}

function PrintReport() {
    var op = 0;
    if ($("#selectMonth").val() == '13') {
        op = 3;
    } else {
        op = 1;
    }
    var url = $("#RedirectTo").val() + '?' + 'anioBusqueda=' + $("#selectAnio").val() + '&mesBusqueda=' + $("#selectMonth").val() + '&idDesechosLiquidos=' + 0 + '&op=' + op;
    var win = window.open(url, '_blank');
}

