var listaDatos = [];
$(document).ready(function () {
    CargarBandeja();
});

//CARGAR BANDEJA
function CargarBandeja() {
    $('#cargac').show();
    var op = 2;
    if ($('#selectEstadoReporte').val()=='true') {
        op = 3;
    }    
    $.ajax({
        url: "../ControlCuchillosPreparacion/BandejaCuchilloPreparacionPartial",
        data: {
            fechaDesde: $('#fechaDesde').val(),
            fechaHasta: $('#fechaHasta').val(),
            idControlCuchillo: 0,
            op: op
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == '0') {
                $('#MensajeRegistros').show();
            } else {
                $('#MensajeRegistros').hide();
            }
            $("#btnPendiente").prop("hidden", true);
            $("#btnAprobado").prop("hidden", false);
            
            $('#divTablaAplrobados').empty();
            $('#divTablaAplrobados').html(resultado);
            setTimeout(function () {
                $('#cargac').hide();
            }, 200);            
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function SeleccionarBandeja(model) {
    $('#cargac').show();
    var table = $("#tblDataTableAprobar");
    table.DataTable().clear();
    table.DataTable().destroy();
    table.DataTable().clear();
    table.DataTable().draw(); 
    listaDatos = model;
    var date = new Date();
    $('#txtFechaAprobado').val(moment(date).format('YYYY-MM-DDTHH:mm'));
    $('#cargac').show();
    if (model.EstadoReporte == true) {
        $('#txtFechaAprobado').prop('hidden', true);
        $('#btnAprobado').prop('hidden', true);
        $('#btnPendiente').prop('hidden', false);
    } else {
        $('#txtFechaAprobado').prop('hidden', false);
        $('#btnPendiente').prop('hidden', true);
        $('#btnAprobado').prop('hidden', false);
    }
    $.ajax({
        url: "../ControlCuchillosPreparacion/ConsultarControlCuchilloDetalle",
        type: "GET",
        data: {
            IdCuchilloPreparacion: 0,
            IdControlCuchilloDetalle: 0,
            IdControlCuchillo: model.IdControlCuchillo,
            opcion: 1
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "102") {
                MensajeAdvertencia("No existen datos para este model.");
            }
            if (resultado.length == 0) {
                MensajeAdvertencia("No existen EMPLEADOS ingresados para este model.");                
            } else {
                $("#tblDataTableAprobar tbody").empty();
                config.opcionesDT.order = [];
                config.opcionesDT.buttons = [];
                config.opcionesDT.columns = [
                    { data: 'CodigoCuchillo' },
                    { data: 'Estado' },
                    { data: 'CedulaEmpleado' },
                    { data: 'UsuarioIngresoLog' }
                ];                
                table.DataTable().destroy();
                table.DataTable(config.opcionesDT);
                resultado.forEach(function (row) {
                    var clscolor = "badge-danger"; //Aplico estilo a la columna Estado 
                    var checked = '';
                    if (row.Estado == true) {
                        clscolor = "badge-success";
                        checked = 'checked';
                    }
                    row.Estado = '<center><span class="badge ' + clscolor + '"><input type="checkbox" ' + checked + ' disabled id="vehicle2" name="Estado" value="Estado"></span></center>';
                    var colummCedula = "";
                    colummCedula = row.CedulaEmpleado;
                    var guion = colummCedula.includes("-");//Valido si la cadena tiene algun -, El - significa que hay un Empleado asignado
                    if (guion == true) {
                        colummCedula = colummCedula.split('-');
                        row.CedulaEmpleado = colummCedula[1];
                    } else { row.CedulaEmpleado = "<center><span class='badge badge-danger' >NO ASIGNADO</span></center>"; }                   
                });
                table.DataTable().rows.add(resultado);
                table.DataTable().draw();            
            }
            setTimeout(function () {
                $('#cargac').hide();
                if (resultado.length != 0) {
                    $("#ModalApruebaPendiente").modal("show");
                }
            }, 200);
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);            
        }
    });
}

function AprobarPendiente(estadoReporte) {
    if ($("#selectEstadoReporte").val() == 'false') {
        var date = new Date();
        if (moment($('#txtFechaAprobado').val()).format('YYYY-MM-DD') < moment(listaDatos.Fecha).format('YYYY-MM-DD')) {
            MensajeAdvertencia('La fecha de APROBACION no puede ser menor a la fecha de creacion del reporte: <span class="badge badge-danger">' + moment(listaDatos.Fecha).format('DD-MM-YYYY') + '</span>');
            return;
        }
        if (moment($('#txtFechaAprobado').val()).format('YYYY-MM-DD') > moment(date).format('YYYY-MM-DD')) {
            MensajeAdvertencia('La fecha de APROBACION no puede ser mayor a la fecha actual: <span class="badge badge-danger">' + moment(date).format('DD-MM-YYYY') + '</span>');
            return;
        }
    } else { $('#txtFechaAprobado').val(''); }
        $.ajax({
            url: "../ControlCuchillosPreparacion/GuardarModificarControlCuchilloPreparacion",
            type: "POST",
            data: {
                IdControlCuchillo: listaDatos.IdControlCuchillo,
                FechaAprobado: $('#txtFechaAprobado').val(),
                siAprobar: true,
                EstadoReporte: estadoReporte
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                if (resultado == 1) {
                    MensajeCorrecto('¡Registro aprobado correctamente!');
                } else if (resultado == 2) { MensajeCorrecto('Estado actualizado correctamente'); }
                else { MensajeError('El registro no debe guardase - solo actualizarce - Controller: GuardarModificarControlCuchilloPreparacion'); }
                
                $("#ModalApruebaPendiente").modal("hide");
                CargarBandeja();                
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
            }
        });
}

function validar() {
    if ($('#txtFechaAprobado').val() == '') {
        $("#txtFechaAprobado").css('border', '1px dashed red');
        MensajeAdvertencia('Fecha invalida');
        return;
    } else {
        $("#txtFechaAprobado").css('border', '');
    }
}

function LimpiarFecha() {
    $("#txtFechaAprobado").css('border', '');
}

//DATE RANGE PICKER
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
            "days": 60
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