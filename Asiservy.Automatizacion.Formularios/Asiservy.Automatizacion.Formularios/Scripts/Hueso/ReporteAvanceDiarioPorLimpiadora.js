var configModal = {
    wsUrl: 'http://192.168.0.31:8870',
    baseUrl: '@Url.Content("~/")',
    opcionesDT: {
        "language": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            },
            "buttons": {
                "pageLength": "<img style='width:100%' src='../../Content/icons/show24.png' />"
            }
        },
        "pageLength": 15,
        "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "Todos"]],
        "pagingType": "full_numbers",
        "dom": 'Bfrtip',
        //"scrollX": "auto",
        //"scrollY": "auto",
        "order": [[0, "desc"], [1, "desc"]],
        "buttons": [
            {
                extend: 'pageLength',
                text: ' <img style="width:100%" src="../../Content/icons/show24.png" />',
                titleAttr: 'Mostrar'
            }, {
                extend: 'copyHtml5',
                text: ' <img style="width:100%" src="../../Content/icons/copy24.png" />',
                titleAttr: 'Copiar'
            },
            {
                extend: 'excelHtml5',
                text: '<img style="width:100%" src="../../Content/icons/excel24.png" />',
                titleAttr: 'Excel'
            },
            {
                extend: 'print',
                text: '<img style="width:100%" src="../../Content/icons/print24.png" />',
                titleAttr: 'Imprimir'
            },
            {
                text: '<img id="imgBtnColumnas"  style="width:100%" src="../../Content/icons/justificar24.png" />',
                titleAttr: 'Ajustar Columnas',
                action: MostarModalColumns
            }
        ]
    }
}


function CargarReporteAvanceLimpiadora() {
    var txtFecha = $('#txtFecha').val();
    var selectLinea = $('#selectLinea').val();
    $('#btnConsultar').prop("disabled", true);
    $("#spinnerCargando").prop("hidden", false);
    $('#DivTableReporteControlAvancePorLimpiadora').html('');
    $.ajax({
        url: "../Hueso/ReporteAvanceDiarioPorLimpiadoraPartial",
        type: "GET",
        data: {
            ddFecha: txtFecha,
            dsLinea: selectLinea,
            dsTurno: $("#selectTurno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $('#DivTableReporteControlAvancePorLimpiadora').html(resultado);
            config.opcionesDT.pageLength = 15;
            config.opcionesDT.order = [[1, "asc"]];
            $('#tblDataTable').DataTable(config.opcionesDT);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);

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
    var table = $('#tblTable');
    table.DataTable().clear();    
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

           //console.log(resultado)

            $("#tblTable tbody").empty();
            configModal.opcionesDT.order = [];
            configModal.opcionesDT.columns = [
                { data: 'Hora' },
                { data: 'OrdenFabricacion' },
                { data: 'Especie' },
                { data: 'Talla' },
                { data: 'Promedio' },
                { data: 'HuesoReal' },
                { data: 'HuesoTeorico' },
                { data: 'KiloReal' },
                { data: 'KiloTeoricoLimpiadora' },
                { data: 'Avance' }
            ];

            var Horas = [];
            var Avance = [];

            resultado.forEach(function (row, i) {
                var poHora = row.Hora;
                row.Hora = moment(row.Hora).format('DD-MM-YYYY HH:mm');
                Horas[i] = moment(poHora).format('HH:mm');
                Avance[i] = row.Avance;
                var estilo = 'badge-danger';
                if (row.Avance < 60) {
                    estilo = "#ff0000";
                } else
                    if (row.Avance < 80) {
                        estilo = "#ffd800";
                    } else {
                        estilo = "#4cff00";
                    }
                row.Avance = row.Avance + ' <i class="fas fa-arrow-circle-up" style="background:' + estilo + '; border-radius:10px"></i>';

            });
            configModal.opcionesDT.pageLength = 5;
            table.DataTable().destroy();
            table.DataTable(configModal.opcionesDT);
            table.DataTable().rows.add(resultado);
            table.DataTable().draw();
           // table.DataTable().destroy();

            //console.log(Avance);
            var options = {
                series: [{
                    type: 'line',
                    name: "Avance",
                    data: Avance
                     
                }],
               
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
                markers: {
                    size: 2,
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
                    title: {
                        text: 'Horas'
                    }
                },
                yaxis: {
                    title: {
                        text: 'Avance'
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

            var chart = new ApexCharts(document.querySelector("#divKpi"), options);
            chart.render();


            var options = {
                series: [{
                    name: "Avance",
                    data: Avance
                }],
                chart: {
                    type: 'bar',
                    height: 350
                },
                colors: [function ({ value, seriesIndex, w }) {
                    if (value < 60) {
                        return '#ff0000'
                    } if (value < 80) {
                        return '#ffd800'
                    } else {
                        return '#4cff00'
                    }
                }],
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
                    title: {
                        text: 'Avance'
                    }
                },
                yaxis: {
                    title: {
                        text: 'Hotas'
                    }
                    
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