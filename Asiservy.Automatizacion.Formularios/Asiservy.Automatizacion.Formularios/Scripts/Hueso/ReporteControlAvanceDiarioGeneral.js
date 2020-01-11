var ListadoGeneral=[];

$(document).ready(function () {
    CargarReporteAvance();
});


function CargarReporteAvance() {
    $("#selectLinea").prop("selectedIndex", 0);
    var txtFecha = $('#txtFecha').val();    
    if (txtFecha == "") {
        MensajeAdvertencia("Igrese una Fecha");
        return;
    }    
    $('#btnConsultar').prop("disabled", true);
    $("#spinnerCargando").prop("hidden", false);
    var bitacora = $('#DivTableReporteControlAvance');
    bitacora.html('');
    $("#kpi").prop("hidden", true); 
    $("#kpi2").prop("hidden", true); 
    $("#DivTable").prop("hidden", true); 
    $("#chartPorLinea").html("");
    $.ajax({
        url: "../Hueso/ReporteControlAvanceDiarioGeneralPartial",
        type: "GET",
        data: {
            ddFecha: txtFecha
           
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "1") {

                MensajeAdvertencia("No existen registros para esa linea");
                $("#spinnerCargando").prop("hidden", true);

            } else {
                CargarReporteAvanceKPI();
                var bitacora = $('#DivTableReporteControlAvance');
                $("#spinnerCargando").prop("hidden", true);
                bitacora.html(resultado);
                config.opcionesDT.pageLength = 50;
                config.opcionesDT.order = [[0, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);
                $("#DivTable").prop("hidden", false);
                $("#kpi2").prop("hidden", false); 


            }
            $('#btnConsultar').prop("disabled", true);

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });

}

function CargarAvanceKPI2() {

    $("#chartPorLinea").html("");
    if ($("#selectLinea").val() == '') {

        return;
    }
    var Horas = [];
    var Horas2 = [];
    var AvanceLinea= [];
   
    var Lineas = [];
 //  
    $.each(ListadoGeneral, function (i, item) {
        // console.log(item.Hora);
        Lineas[i] = item.Linea;
    });
    Lineas = Lineas.unique();
    $.each(ListadoGeneral, function (i, item) {
        Horas[i] = item.Hora.Hours;  
        Horas2[i] = item.Hora.Hours + ":00";  
        
    });
    Horas = Horas.unique();
    Horas2 = Horas2.unique();
   
    $.each(Horas, function (h, hora) { 
        var TotalAvance = 0;
      
        $.each(ListadoGeneral, function (j, item) {
            if ($("#selectLinea").val() == item.Linea && hora == item.Hora.Hours) {
                    TotalAvance = parseFloat(TotalAvance) + parseFloat(item.Avance);
                }              
        });     
        AvanceLinea[h] = parseFloat(TotalAvance);          
        
    });   
 

    var options = {
        series: [
            //{
            //name: 'Net Profit',
            //data: AvanceLinea1
            //},
            {

                name: 'Avance',
                data: AvanceLinea
            },
        //    {
        //    name: 'Free Cash Flow',
        //    data: AvanceLinea3
        //}, {
        //    name: 'Revenue',
        //    data:AvanceLinea4
        //},{
        //    name: 'Revenue',
        //    data:AvanceLinea5
        //    }
        ],
        chart: {
            type: 'bar',
            height: 350
        },
        plotOptions: {
            bar: {
                horizontal: false,
                columnWidth: '55%',
                endingShape: 'rounded'
            },
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            show: true,
            width: 2,
            colors: ['transparent']
        },
        xaxis: {
            categories: Horas2,
        },
        yaxis: {
            title: {
                text: 'Avance'
            }
        },
        fill: {
            opacity: 1
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return "" + val + "%"
                }
            }
        }
    };

    var chart = new ApexCharts(document.querySelector("#chartPorLinea"), options);
    chart.render();

 

    // console.log(ListadoGeneral);
}

function CargarReporteAvanceKPI() {
    var txtFecha = $('#txtFecha').val();
    if (txtFecha == "") {
        
        return;
    }
   
    $.ajax({
        url: "../Hueso/ConsultaControlAvanceDiarioGeneral",
        type: "GET",
        data: {
            ddFecha: txtFecha
        },
        success: function (resultado) {
            ListadoGeneral = resultado;
           // console.log(resultado);   
            var Lineas = []; 
            var Avance = []; 
            $.each(resultado, function (i, item) {
               ///console.log($.inArray(item.Linea, Lineas));
                if ($.inArray(item.Linea, Lineas) == -1)
                { Lineas[i] = item.Linea; }
            });
            ///console.log(Lineas);
            //Lineas = Lineas.unique();
            //console.log(Lineas);

            $.each(Lineas, function (i, linea) {
                var TotalAvance = 0;
                var cont = 0;
                $.each(resultado, function (j, item) {
                    if (linea == item.Linea) {
                        TotalAvance = parseFloat(TotalAvance) +parseFloat(item.Avance);   

                        if (parseFloat(item.Avance) > 0)
                            cont++;
                    }
                });  
                //console.log(linea+': '+TotalAvance);
                //console.log(cont);
                Avance[i] = parseFloat(TotalAvance) / parseFloat(cont);
                Avance[i] = Avance[i].toFixed(2);
                //Avance[i] = TotalAvance / cont;
            });
           // console.log(Avance);
            var options = {
                series: [{
                    name: 'Avance',
                    data: Avance
                }],
                chart: {
                    height: 350,
                    type: 'bar',
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            position: 'top', // top, center, bottom
                        },
                    }
                },
                dataLabels: {
                    enabled: true,
                    formatter: function (val) {
                        return val + "%";
                    },
                    offsetY: -20,
                    style: {
                        fontSize: '12px',
                        colors: ["#304758"]
                    }
                },

                xaxis: {
                    categories: Lineas,
                    position: 'top',
                    labels: {
                        offsetY: -18,

                    },
                    axisBorder: {
                        show: false
                    },
                    axisTicks: {
                        show: false
                    },
                    crosshairs: {
                        fill: {
                            type: 'gradient',
                            gradient: {
                                colorFrom: '#D8E3F0',
                                colorTo: '#BED1E6',
                                stops: [0, 100],
                                opacityFrom: 0.4,
                                opacityTo: 0.5,
                            }
                        }
                    },
                    tooltip: {
                        enabled: true,
                        offsetY: -35,

                    }
                },
                fill: {
                    gradient: {
                        shade: 'light',
                        type: "horizontal",
                        shadeIntensity: 0.25,
                        gradientToColors: undefined,
                        inverseColors: true,
                        opacityFrom: 1,
                        opacityTo: 1,
                        stops: [50, 0, 100, 100]
                    },
                },
                yaxis: {
                    axisBorder: {
                        show: false
                    },
                    axisTicks: {
                        show: false,
                    },
                    labels: {
                        show: false,
                        formatter: function (val) {
                            return val + "%";
                        }
                    }

                },
                title: {
                    text: 'Avance general por linea',
                    floating: true,
                    offsetY: 320,
                    align: 'center',
                    style: {
                        color: '#444'
                    }
                }
            };

            var chart = new ApexCharts(document.querySelector("#chart"), options);
            chart.render();
            $("#kpi").prop("hidden", false);
          

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });

}


Array.prototype.unique = function (a) {
    return function () { return this.filter(a) }
}(function (a, b, c) {
    return c.indexOf(a, b + 1) < 0
});