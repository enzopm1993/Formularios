
var weekdays = ["Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado"];

$(document).ready(function () {
   // ConsultarSolicitudes();
    $("#selectLinea").select2({
        'width': '100%'
    });

    $("#selectArea").select2({
        'width': '100%'
    });

  

    if ($('#Garita').val() == true) {
        $("#selectEstado").prop("disabled", true);
    } 
});

function Nuevo() {
    $("#selectLinea").prop("selectedIndex", 0).change();
    $("#selectArea").empty();
    $("#selectArea").append("<option value='' >-- Seleccionar Opción--</option>");
}

function CambioLinea(valor) {
    $("#selectArea").empty();
    $("#selectArea").append("<option value='' >-- Seleccionar Opción--</option>");
    if ($("#selectLinea").val() != '') {
        $.ajax({
            url: "../SolicitudPermiso/ConsultaListadoAreas",
            type: "Get",
            data:
            {
                CodLinea: valor
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                if (!$.isEmptyObject(resultado)) {
                    $.each(resultado, function (create, row) {
                        $("#selectArea").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                    });
                } else {
                    MensajeAdvertencia("La linea seleccionado no tiene areas asignadas", false);
                }
            },
            error: function (resultado) {
                MensajeError("Error: Comuníquese con sistemas", false);
            }
        });
    }
}


function MarcarSalida(IdSolicitudPermiso,Cedula,fechaSalida) {
   //console.log(IdSolicitudPermiso);
   // console.log(Cedula);
   // console.log(moment(fechaSalida).format("DD-MM-YYYY"));
   // //if (fecha == "" || fecha == null ) {

    //    MensajeAdvertencia("No ha marcado en el biometríco");
    //    return;
    //}
    $("#txtMarcarSalida-" + IdSolicitudPermiso).prop("disabled", true);
    $.ajax({
        type: "POST",
        url: '../SolicitudPermiso/MarcarSalidaSolicitudPermiso',
        data: {
            IdSolicitudPermiso: IdSolicitudPermiso,
            Cedula: Cedula,
            FechaSalida: fechaSalida
        },
        success: function (Resultado) {
            if (Resultado == "101") {
                window.location.reload();
            }
            if (Resultado == "102") {
                MensajeAdvertencia("NO HA MARCADO SALIDA");
                return;
            }
            if (Resultado == "1") {
                var horaSalida = moment(fechaSalida).format('HH:mm');
                MensajeAdvertencia("Su hora de salida es a las: " + horaSalida);
                return;
            }
            MensajeCorrecto(Resultado,false);
            ConsultarSolicitudes();
        },
        error: function (Resultado) {
           MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
   

}

function ConsultarSolicitudes() {
    MostrarModalCargando();
    $('#RptSolicitudes').html('');
    $.ajax({
        type: "GET",
        url: '../SolicitudPermiso/ConsultaSolicitudes',
        data: {
            dsLinea: $('#selectLinea').val(),
            dsArea: $('#selectArea').val(),
            dsEstado: $('#selectEstado').val(),
            dsGarita: $('#Garita').val(),
            ddFechaDesde: $("#fechaDesde").val(),
            ddFechaHasta: $("#fechaHasta").val(),
        },
        success: function (data) {
           // console.log(data);
            console.log(data);
            DevExpress.localization.locale(navigator.language);
            if ($('#Garita').val() == "true") {
                var opciosGrid = {
                    dataSource: data,
                    loadPanel: {
                        enabled: true
                    },
                    keyExpr: "IdSolicitudPermiso",
                    selection: {
                        mode: "single"
                    },
                    hoverStateEnabled: true,
                    showColumnLines: true,
                    showRowLines: true,
                    rowAlternationEnabled: true,
                    showBorders: true,
                    showBorders: true,
                    allowColumnResizing: true,
                    columnResizingMode: "nextColumn",
                    columnMinWidth: 50,
                    columnAutoWidth: true,
                    columnFixing: {
                        enabled: true
                    },
                    showBorders: true,
                    showRowLines: true,
                    filterRow: {
                        visible: true,
                        applyFilter: "auto"
                    },
                   
                    paging: {
                        pageSize: 0
                    },
                   
                    searchPanel: {
                        visible: true,
                      //  highlightCaseSensitive: true,
                        width: 240,
                        placeholder: "Buscar..."
                    },
                    headerFilter: {
                        visible: false
                    },
                  columns: [
                      { dataField: "IdSolicitudPermiso", caption: "# Solicitud", sortOrder: "desc", alignment: 'center'},
                        { caption: "Linea", dataField: "Linea", area: "column" },
                       
                        "Nombre",
                      {
                          caption: "Estado",
                            dataField: "EstadoSolicitud", dataType: "string",
                            cellTemplate: function (container, options) {
                                var estiloClass = 'badge';
                                if (options.data.CodEstadoSolicitud == "002") {
                                    var estiloClass = 'badge badge-success';
                                } else if (options.data.CodEstadoSolicitud == "001") {
                                    var estiloClass = 'badge badge-warning';
                                } else if (options.data.CodEstadoSolicitud == "003") {
                                    var estiloClass = 'badge badge-secondary';
                                } else {
                                    var estiloClass = 'badge badge-secondary';
                                }
                                $("<div>")
                                    .append($('<span class="' + estiloClass + '">' + options.value + '</span>'))
                                    .appendTo(container);

                            }
                        },
                      { caption:"Biometrico",dataField: "FechaBiometrico", dataType: "string" },
                      {
                          dataField: "FechaSalida", dataType: "date", cellTemplate: function (container, options) {
                              var dia = weekdays[moment(options.data.FechaSalida2).isoWeekday()];
                              container.append("<div>" + dia + ", " + options.data.FechaSalida + "</div>")
                          }
                      },
                        { caption: "Hora Salida", dataField: "HoraSalida", dataType: "time" },
                        {
                            caption: "Medico", alignment: "center", cellTemplate: function (container, options) {
                                if (options.data.Origen == "M") {
                                    $("<div>")
                                    .append($('<i class="fas fa-briefcase-medical" style="color:blue"></i>'))
                                    .appendTo(container);
                                }
                             
                            }
                        }
                        ,
                        {
                            caption: "Acción", type: "buttons", cellTemplate: function (container, options) {
                               // console.log(options.data);
                                var Fecha = moment(options.data.FechaSalida2).format("YYYY-MM-DD HH:mm");
                                //console.log(Fecha);
                                $("<div>")
                                    .append($('<input type="button" name="name" value="Marcar Salida" class="btn btn-outline-info" onclick="MarcarSalida(' + "'" + options.data.IdSolicitudPermiso + "'" + ',' + "'" + options.data.Identificacion + "'" + ',' + "'" + Fecha + "'" +')"/>'))
                                    .appendTo(container);

                            }
                        }

                    ],
                    summary: {
                        totalItems: [{
                            column: "IdSolicitudPermiso",
                            summaryType: "count",
                            displayFormat: "Total: {0}",
                            alignment: 'center'
                        }

                        ]
                    }
                }
            } else {
                var opciosGrid = {
                    dataSource: data,
                    loadPanel: {
                        enabled: true
                    },
                    keyExpr: "IdSolicitudPermiso",
                    selection: {
                        mode: "single"
                    },
                    hoverStateEnabled: true,
                    showColumnLines: true,
                    showRowLines: true,
                    rowAlternationEnabled: true,
                    showBorders: true,
                    showBorders: true,
                    allowColumnResizing: true,
                    columnResizingMode: "nextColumn",
                    columnMinWidth: 50,
                    columnAutoWidth: true,
                    columnFixing: {
                        enabled: true
                    },
                    showBorders: true,
                    showRowLines: true,
                    filterRow: {
                        visible: true,
                        applyFilter: "auto"
                    },
                    headerFilter: {
                        visible: true
                    },
                    paging: {
                        pageSize: 10
                    },
                    pager: {
                        showPageSizeSelector: true,
                        allowedPageSizes: [10, 20, 50, 100],
                        showInfo: true
                    },
                    searchPanel: {
                        visible: true,
                        highlightCaseSensitive: true,
                        width: 240,
                        placeholder: "Buscar..."
                    },
                    groupPanel: { visible: true },
                    grouping: {
                        autoExpandAll: false
                    },

                    columns: [
                        { dataField: "IdSolicitudPermiso", caption: "# Solicitud", sortOrder: "desc" },
                        { caption: "Linea", dataField: "Linea", area: "column" },
                        { dataField: "Area", width: 180 },
                        "Nombre",
                        {
                            caption: "Estado",
                            dataField: "EstadoSolicitud", dataType: "string",
                            cellTemplate: function (container, options) {
                                var estiloClass = 'badge';
                                if (options.data.CodEstadoSolicitud == "002") {
                                    var estiloClass = 'badge badge-success';
                                } else if (options.data.CodEstadoSolicitud == "001") {
                                    var estiloClass = 'badge badge-warning';
                                } else if (options.data.CodEstadoSolicitud == "003") {
                                    var estiloClass = 'badge badge-secondary';
                                } else {
                                    var estiloClass = 'badge badge-secondary';
                                }
                                $("<div>")
                                    .append($('<span class="' + estiloClass + '">' + options.value + '</span>'))
                                    .appendTo(container);

                            }
                        },
                        { dataField: "FechaBiometrico", dataType: "string" },
                        {
                            dataField: "FechaSalida", dataType: "date", cellTemplate: function (container, options) {
                                var dia = weekdays[moment(options.data.FechaSalida2).isoWeekday()];
                                container.append("<div>" + dia+", " +options.data.FechaSalida + "</div>")
                            }
                        },
                        { caption: "Hora Salida", dataField: "HoraSalida", dataType: "time" },
                        {
                            dataField: "FechaRegreso", dataType: "date", cellTemplate: function (container, options) {
                                var dia = weekdays[moment(options.data.FechaSalida2).isoWeekday()];
                                container.append("<div>" + dia + ", " + options.data.FechaRegreso + "</div>")
                            } },
                        { caption: "Hora Regreso", dataField: "HoraRegreso", dataType: "time" },
                        "Motivo",
                        "Observacion",
                        {
                            caption: "Fecha ingreso log",
                            dataField: "FechaIngresoLog",
                            area: "column",
                            dataType: "datetime"
                        }
                        , {
                            caption: "Usuario ingreso log",
                            dataField: "UsuarioIngresoLog",
                            area: "column",
                            dataType: "string"
                        }
                        , {
                            caption: "Terminal ingreso log",
                            dataField: "TerminalIngresoLog",
                            area: "column",
                            dataType: "string"
                        }
                        , {
                            caption: "Usuario modificación log",
                            dataField: "UsuarioModificacionLog",
                            area: "column",
                            dataType: "string"
                        }
                        , {
                            caption: "Fecha modificación log",
                            dataField: "FechaModificacionLog",
                            area: "column",
                            dataType: "datetime"
                        }
                        , {
                            caption: "Terminal modificación log",
                            dataField: "TerminalModificacionLog",
                            area: "column",
                            dataType: "string"
                        },
                        {
                            caption: "", cellTemplate: function (container, options) {
                                if (options.data.CodEstadoSolicitud == "003") {
                                    $("<divxº>")
                                        //.append($('<a class="badge badge-danger btnReversa" onclick="ReversarSolicitud(' + options.data.IdSolicitudPermiso + ')"> Reversar</span>'))
                                        .append($('<input type="button" name="name" value="Reversar" class="btn btn-outline-info" onclick="ReversarSolicitud(' + "'" + options.data.IdSolicitudPermiso + "'" +')"/>'))
                                        .appendTo(container);
                                }
                            }
                        }

                    ],
                    summary: {
                        totalItems: [{
                            column: "IdSolicitudPermiso",
                            summaryType: "count",
                            displayFormat: "Total: {0}",
                            alignment: 'right'
                        },

                        ], groupItems: [{
                            column: "IdSolicitudPermiso",
                            displayFormat: "Total: {0}",
                            summaryType: "count"
                        }]
                    },
                    //onSelectionChanged: function (selectedItems) {
                    //    var data = selectedItems.selectedRowsData[0];
                    //    if (data) {
                    //        MostrarReporte(data);
                    //        //$(".employeeNotes").text(data.Notes);
                    //        //$(".employeePhoto").attr("src", data.Picture);
                    //    }
                    //},
                    selection: {
                        mode: 'multiple'
                    }, export: {
                        enabled: true,
                        allowExportSelectedData: true
                    }, 
                    onExporting: function (e) {
                        var workbook = new ExcelJS.Workbook();
                        var worksheet = workbook.addWorksheet('Reporte');



                        DevExpress.excelExporter.exportDataGrid({
                            component: e.component,
                            worksheet: worksheet,
                            autoFilterEnabled: true
                        }).then(function () {
                            workbook.xlsx.writeBuffer().then(function (buffer) {
                                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'SolicitudesPermisos.xlsx');
                            });
                        });
                        e.cancel = true;
                    }
                }
            }
           
            $("#gridContainer").dxDataGrid(opciosGrid);
           CerrarModalCargando();


        },
        error: function (result)
        {
           MensajeError("Error: Comuníquese con sistemas", false);
           CerrarModalCargando();
        }
    });

}


function ReversarSolicitud(IdSolicitudPermiso) {

    $(".btnReversa").prop("disabled", true);
    $.ajax({
        type: "POST",
        url: '../SolicitudPermiso/ReversarSolicitudPermiso',
        data: {
            IdSolicitudPermiso: IdSolicitudPermiso
        },
        success: function (Resultado) {
            if (Resultado == "101") {
                window.location.reload();
            }
            if (Resultado == "102") {
                MensajeAdvertencia("OCURRIÓ UN ERROR EN EL PROCESO");
                $(".btnReversa").prop("disabled", false);
                return;
            }         
            MensajeCorrecto(Resultado, false);
            ConsultarSolicitudes();
        },
        error: function (Resultado) {
           MensajeError("Error: Comuníquese con sistemas", false);
        }
    });


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
        ConsultarSolicitudes();
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
    //ConsultarSolicitudes();
});



