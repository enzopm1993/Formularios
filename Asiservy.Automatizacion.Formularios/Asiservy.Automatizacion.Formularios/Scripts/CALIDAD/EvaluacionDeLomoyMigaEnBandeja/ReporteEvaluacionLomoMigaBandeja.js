var Error = 0;
var ParametrosLomo =
{
    Limpieza1: {
        Venas: 8,
        Espinas: 10,
        Moretones: 9,
        Escamas: 3,
        Piel: 5,
        Total: 35
    },
    Limpieza2: {
        Venas: 6,
        Espinas: 8,
        Moretones: 5,
        Escamas: 2,
        Piel: 4,
        Total: 25
    },
    Limpieza3: {
        Venas: 1,
        Espinas: 3,
        Moretones: 3,
        Escamas: 0,
        Piel: 0,
        Total: 7
    }
}
var ParametrosMiga =
{
    Limpieza1: {
        Venas: 7,
        Espinas: 10,
        Moretones: 7,
        Escamas: 10,
        Piel: 6,
        Total: 40
    },
    Limpieza2: {
        Venas: 4,
        Espinas: 10,
        Moretones: 3,
        Escamas: 5,
        Piel: 3,
        Total: 25
    },
    Limpieza3: {
        Venas: 0,
        Espinas: 2,
        Moretones: 2,
        Escamas: 0,
        Piel: 0,
        Total: 4
    }
}
function MostrarReporte(data) {
    $('#cargac').show();
    Error = 0;
    let params = {
        IdEvaluacionDeLomosYMigasEnBandejas: data.IdEvaluacionDeLomosYMigasEnBandejas
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EvaluacionDeLomoyMigaEnBandeja/PartialReporteEvaluacionLomosMigasBandeja?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.text();
        })
        .then(function (resultado) {
            if (resultado == '"101"') {
                window.location.reload();
            }
            if (resultado != '"0"') {
                $('#DivBotonImpr').prop('hidden', false);
                $('#DivReporte').prop('hidden', false);
                $('#DivCabReportes').prop('hidden', true);
                $('#DivReporte').empty();
                $('#DivReporte').html(resultado);
                $('#mensajeRegistros').prop('hidden', true);
                //config.opcionesDT.pageLength = 30;
                //$('#tblReporte').DataTable(config.opcionesDT);
                //LimpiarDetalleControles();
            } else {
                $('#DivReporte').empty();
                $('#mensajeRegistros').prop('hidden', false);
            }
            $('#cargac').hide();
            //console.log(resultado);
        })
        .then(function () {
            ConsultarFotos(data.IdEvaluacionDeLomosYMigasEnBandejas);
        })
        .catch(function (resultado) {
            console.log(resultado);
            MensajeError(resultado, false);
            //$('#btnCargando').prop('hidden', true);
            //$('#btnConsultar').prop('hidden', false);
        })
    
}
function imprimirw() {
    window.print();
}
function ConsultarFotos(idCabecera) {
    let params = {
        IdCabecera: idCabecera
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EvaluacionDeLomoyMigaEnBandeja/PartialReporteFotos?' + query;

    fetch(url)
        //,body: data
        .then(function (respuesta) {
            if (!respuesta.ok) {

                MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
                Error = 1;
            }
            return respuesta.text();
        })
        .then(function (resultado) {
            if (resultado == '"101"') {
                window.location.reload();
            }
            if (Error == 0) {
                if (resultado == '"0"') {
                    //$("#divTableDetalle2").html("<div class='text-center'>No existen registros</div>");
                    //$("#spinnerCargandoDetalle2").prop("hidden", true);
                } else {
                    //$("#spinnerCargandoDetalle2").prop("hidden", true);
                    $("#divTableDetalle2").html(resultado);

                }


            }
        })
        .catch(function (resultado) {

            MensajeError(resultado.responseText, false);

        })
}
function validarImg(rotacion, id, imagen) {

    $('#' + id).rotate(rotacion);
    //document.getElementById(id).style.height = "0px";
    //document.getElementById(id).style.width = "0px";

    var img = new Image();
    img.onload = function () {
        //  alert(this.width + 'x' + this.height);
        var ancho = this.width;
        var alto = this.height;
        if (ancho < alto) {
            document.getElementById(id).style.height = "250px";
            document.getElementById(id).style.width = "150px";
        } else {
            document.getElementById(id).style.height = "150px";
            document.getElementById(id).style.width = "250px";
        }

    }
    img.src = "../ImagenSiaa/" + imagen;

}
function Atras() {
    $('#DivBotonImpr').prop('hidden', true);
    $('#DivReporte').prop('hidden', true);
    $('#DivCabReportes').prop('hidden', false);
}
function CargarCabReportes() {
    Atras();
    $('#cargac').show();
    var table = $("#tblDataTableReporte");
//    table.DataTable().destroy();
//    table.DataTable().clear();
    let params = {
        FechaDesde: $('#fechaDesde').val(),
        FechaHasta: $('#fechaHasta').val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EvaluacionDeLomoyMigaEnBandeja/ConsultarCabecerasReporte?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.json();
        })
        .then(function (resultado) {
            if (resultado == '101') {
                window.location.reload();
            }
            if (resultado != '0') {
                $('#mensajeRegistros').prop('hidden', true);
                $("#tblDataTableReporte tbody").empty();
                $('#DivCabReportes').prop('hidden', false);
                config.opcionesDT.columns = [
                    { data: 'IdEvaluacionDeLomosYMigasEnBandejas' },
                    { data: 'FechaProduccion' },
                    { data: 'OrdenFabricacion' },
                    { data: 'Turno' },
                    { data: 'Cliente' },
                    { data: 'Lomo' },
                    { data: 'Miga' },
                    { data: 'Empaque' },
                    { data: 'Enlatado' },
                    { data: 'Pouch' },
                    { data: 'NivelLimpiezaDescripcion' },
                    { data: 'NivelLimpieza' },
                    { data: 'Observacion' },
                    { data: 'EstadoRegistro' },
                    { data: 'FechaIngresoLog' },
                    { data: 'UsuarioIngresoLog' },
                    { data: 'TerminalIngresoLog' },
                    { data: 'FechaModificacionLog' },
                    { data: 'UsuarioModificacionLog' },
                    { data: 'TerminalModificacionLog' },
                    { data: 'EstadoControl' },
                    { data: 'AprobadoPor' },
                    { data: 'FechaAprobacion' }
                ];
                
                resultado.forEach(function (row) {
                    row.FechaProduccion = moment(row.FechaProduccion).format('DD-MM-YYYY');
                    if (row.FechaAprobacion != null) {
                        row.FechaAprobacion = moment(row.FechaAprobacion).format('DD-MM-YYYY HH:mm');
                    }
                    if (row.FechaIngresoLog != null) {
                        row.FechaIngresoLog = moment(row.FechaIngresoLog).format('DD-MM-YYYY HH:mm');
                    }
                    if (row.FechaModificacionLog != null) {
                        row.FechaModificacionLog = moment(row.FechaModificacionLog).format('DD-MM-YYYY HH:mm');
                    }
                    var estiloTrue ='<i class="fas fa-check-square" style="color:green"></i>'
                    var estiloClass = 'badge badge-danger';
                    var estado = 'PENDIENTE';
                    if (row.ObservacionDet!=null) {
                        row.Observacion = row.Observacion.toUpperCase();
                    }
                    if (row.EstadoControl == true) {
                        estiloClass = 'badge badge-success';
                        estado = 'APROBADO';
                    }
                    if (row.Lomo == true) {
                        row.Lomo = estiloTrue;
                    } else {
                        row.Lomo = '';
                    }
                    if (row.Miga == true) {
                        row.Miga = estiloTrue;
                    } else {
                        row.Miga = '';
                    }
                    if (row.Empaque == true) {
                        row.Empaque = estiloTrue;
                    } else {
                        row.Empaque = '';
                    }
                    if (row.Enlatado == true) {
                        row.Enlatado = estiloTrue;
                    } else {
                        row.Enlatado = '';
                    }
                    if (row.Pouch == true) {
                        row.Pouch = estiloTrue;
                    } else {
                        row.Pouch = '';
                    }
                    row.EstadoControl = '<span class="' + estiloClass + '">' + estado + '</span>';
                });
                config.opcionesDT.columnDefs = [
                {
                    "targets": [0,11,13,16,19],
                    "visible": false,
                    "searchable": false
                }
                ];
                //config.opcionesDT.scrollX = false;
                table.DataTable().destroy();

                table.DataTable(config.opcionesDT);
                table.DataTable().clear();
                table.DataTable().rows.add(resultado);
                table.DataTable().draw();

                $('#tblDataTableReporte tbody').on('click', 'tr', function () {
                    var table = $('#tblDataTableReporte').DataTable();
                    var dataDetalle = table.row(this).data();

                    MostrarReporte(dataDetalle);
                });
            } else {
           
                $('#DivReporte').empty();
                $('#DivCabReportes').prop('hidden', true);
                $('#mensajeRegistros').text(Mensajes.SinRegistrosRangoFecha);
                $('#mensajeRegistros').prop('hidden', false);
            }
            $('#cargac').hide();
            //console.log(resultado);
        })
        .catch(function (resultado) {
            $('#cargac').hide();
            console.log(resultado);
            MensajeError(resultado, false);
            //$('#btnCargando').prop('hidden', true);
            //$('#btnConsultar').prop('hidden', false);
        })
}

//FECHA DataRangePicker
$(function () {
    var start = moment();
    var end = moment();
    var mesesLetras = {
        '01': "Enero",
        '02': "Febrero",
        '03': "Marzo",
        '04': "Abril",
        '05': "Mayo",
        '06': "Junio",
        '07': "Julio",
        '08': "Agosto",
        '09': "Septiembre",
        '10': "Octubre",
        '11': "Noviembre",
        '12': "Diciembre"
    }

    function cb(start, end) {
        var fechaMuestraDesde = mesesLetras[start.format('MM')] + ' ' + start.format('D') + ', ' + start.format('YYYY');
        var fechaMuestraHasta = mesesLetras[end.format('MM')] + ' ' + end.format('D') + ', ' + end.format('YYYY');
        $("#fechaDesde").val(start.format('YYYY-MM-DD'));
        $("#fechaHasta").val(end.format('YYYY-MM-DD'));

        $('#reportrange span').html(fechaMuestraDesde + ' - ' + fechaMuestraHasta);
    }

    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        maxSpan: {
            "days": 61
        },
        minDate: moment("01/10/2019", "DD/MM/YYYY"),
        maxDate: moment(),
        ranges: {
            'Hoy': [moment(), moment()],
            'Ayer': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Últimos 7 días': [moment().subtract(6, 'days'), moment()],
            'Últimos 30 días': [moment().subtract(29, 'days'), moment()],
            'Mes actual (hasta hoy)': [moment().startOf('month'), moment()],
            'Último mes': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        },
        "locale": {
            "format": "DD/MM/YYYY",
            "separator": " - ",
            "applyLabel": "Aplicar",
            "cancelLabel": "Cancelar",
            "fromLabel": "De",
            "toLabel": "a",
            "customRangeLabel": "Personalizada",
            "weekLabel": "W",
            "daysOfWeek": [
                "Do",
                "Lu",
                "Ma",
                "Mi",
                "Ju",
                "Vi",
                "Sa"
            ],
            "monthNames": [
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Septiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            ],
            "firstDay": 1
        }
    }, cb);
    cb(start, end);
});