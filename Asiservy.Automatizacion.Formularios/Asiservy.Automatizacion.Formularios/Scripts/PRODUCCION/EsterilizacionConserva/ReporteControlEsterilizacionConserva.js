﻿function ConsultarReporte() {
    if ($('#FechaProduccion').val() == '') {
        $('#msjerrorfecha').prop('hidden', false);
        return false;
    } else {
        $('#msjerrorfecha').prop('hidden', true);
    }
    if ($('#Turno').prop('selectedIndex') == 0) {
        $('#msjerrorturno').prop('hidden', false);
        return false;
    } else {
        $('#msjerrorturno').prop('hidden', true);
    }
    if ($('#Linea').prop('selectedIndex') == 0) {
        $('#msjerrorLinea').prop('hidden', false);
        return false;
    } else {
        $('#msjerrorLinea').prop('hidden', true);
    }
    $('#btnCargando').prop('hidden', false);
    $('#btnConsultar').prop('hidden', true);

    //const data = new FormData();
    //data.append('Fecha', $("#FechaProduccion").val());
    //data.append('Turno', $("#Turno").val());
    //data.append('Linea', $("#Linea").val());
    //var url = new URL('../EsterilizacionConserva/PartialReporteControl')
    let params = {
        Fecha:$("#FechaProduccion").val(),
        Turno:$("#Turno").val(),
        Linea: $("#Linea").val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EsterilizacionConserva/PartialReporteControl?' + query;
    fetch(url)
        //,body: data
    .then(function (respuesta) {
        return respuesta.text();
        })
    .then(function (resultado) {
        //console.log(resultado);
        if (resultado == '"101"') {
            window.location.reload();
        }
        if (resultado == "0") {
            $('#DivReporte').html('');
            $('#mensajegeneral').prop('hidden', false);
            $('#btnimprimir').prop('hidden', true)
        } else {
            $('#mensajegeneral').prop('hidden', true);
            $('#DivReporte').html(resultado);
            $('#btnimprimir').prop('hidden', false);
            $('#lblFechap').text($('#FechaProduccion').val());
        }
        $('#btnCargando').prop('hidden', true);
        $('#btnConsultar').prop('hidden', false);

        //if ($('#RegPartial').val() == 0) {
        //    $('#mensajegeneral').prop('hidden', false);
        //    $('#btnimprimir').prop('hidden', true)
        //} else {
        //    $('#lblFechap').text($('#FechaProduccion').val());
        //    $('#btnimprimir').prop('hidden',false)
        //    $('#mensajegeneral').prop('hidden', true);
        //}
    })
    .catch(function (resultado) {
        MensajeError(resultado.responseText, false);
        $('#btnCargando').prop('hidden', true);
        $('#btnConsultar').prop('hidden', false);
    })
}
//function printDiv(nombreDiv) {
//    var contenido = document.getElementById(nombreDiv).innerHTML;
//    var contenidoOriginal = document.body.innerHTML;

//    document.body.innerHTML = contenido;

//    window.print();

//    document.body.innerHTML = contenidoOriginal;
//}
//function ImprimeDiv() {
//    var divToPrint = document.getElementById('DivReporte');
//    //var divToPrint = $('DivReporte').html();
//    var newWin = window.open('', 'Print-Window', 'width=1000,height=600');
//    newWin.document.open();
//    newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
    
//    newWin.document.close();
//    setTimeout(function () { newWin.close(); }, 10);
//}
//function imprimir3() {
//    $('#DivReporte').printThis();
//}
function imprimirElemento(elemento) {
    var ventana = window.open('', 'PRINT', 'height=768,width=1024');
    ventana.document.write('<html><head><title>' + document.title + '</title>');
    ventana.document.write('<link rel="stylesheet" href="~/Content/Site.css">');
    ventana.document.write('</head><body >');
    ventana.document.write(elemento.innerHTML);
    ventana.document.write('</body></html>');
    ventana.document.close();
    ventana.focus();
    ventana.onload = function () {
        ventana.print();
        ventana.close();
    };
    return true;
}
function imprime4() {
    //var div = document.querySelector("#DivReporte");
    //imprimirElemento(div);
    window.print();
}

//function imprimirCanvas() {

//    var canvas = html2canvas($("#DivReporte"));
//    var img = canvas.toDataUrl("image/png");
//    doc = new jsPDF("px", "a4");
//    doc.addImage(img, "png", 50, 50);
//    doc.save("demo.pdf");
//}
//function PrintDiv() {
//    var divContents = document.getElementById("DivReporte").innerHTML;
//    var printWindow = window.open('', '', 'height=200,width=400');
//    printWindow.document.write('<html><head><title>DIV Contents</title>');
//    printWindow.document.write('</head><body >');
//    printWindow.document.write(divContents);
//    printWindow.document.write('</body></html>');
//    printWindow.document.close();
//    printWindow.print();
//}
function imprimirw() {
    window.print();
}