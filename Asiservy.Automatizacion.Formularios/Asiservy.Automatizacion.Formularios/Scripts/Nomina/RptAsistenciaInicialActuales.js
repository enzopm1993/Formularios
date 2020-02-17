$(function () {


    var iconLoader = "fa-spinner fa-pulse";
    var iconSearch = "fa-search"
       
    $("#generarAsistencia").click(function () {
        var fechaIni = $("#fechaIni").val();
        $("#wdr-component").empty();

        $("#generarAsistencia").attr('href', "javascript:void(0)");
        $("#iconSearch").removeClass(iconSearch);
        $("#iconSearch").addClass(iconLoader);
        $("#generarAsistencia").addClass("btnWait");
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: "../Nomina/GenerarAsistenciaInicialVsActual",
            type: "GET",
            data:{
                'fecha': fechaIni
            },
            success: function (resultado) {
                $("#generarAsistencia").attr('href', "#");
                $("#iconSearch").removeClass(iconLoader);
                $("#iconSearch").addClass(iconSearch);
                $("#generarAsistencia").removeClass("btnWait");
               
                console.log(resultado);
                var pivot = new WebDataRocks({
                    container: "#wdr-component",
                    toolbar: true,
                    beforetoolbarcreated: customizeToolbar,
                    report: {
                        dataSource: {
                            data: resultado
                        },
                        "slice": {
                            "rows": [
                                {
                                    "uniqueName": "CentroCosto"
                                },
                                {
                                    "uniqueName": "Linea"
                                },
                                {
                                    "uniqueName": "Cargo"
                                },
                                {
                                    "uniqueName": "Nombre"
                                }
                            ],
                            "columns": [
                                {
                                    "uniqueName": "Measures"
                                },
                                {
                                    "uniqueName": "Asistencia"
                                }
                            ],
                            "measures": [
                                {
                                    "uniqueName": "Cedula",
                                    "aggregation": "count"
                                }
                            ],
                            "expands": {
                                "rows": [
                                    {
                                        "tuple": [
                                            "CentroCosto.Limpieza de pescado"
                                        ]
                                    }
                                ]
                            }
                        },
                        "options": {
                            "grid": {
                                "showGrandTotals": "columns"
                            }
                        }
                    },
                    global: {
                        // replace this path with the path to your own translated file
                        localization: config.baseUrl + "Content/webdatarocks/es.json"
                    }
                });

            },
            error: function (resultado) {
                console.log(resultado);
                $("#generarAsistencia").attr('href', "#");
                $("#iconSearch").removeClass(iconLoader);
                $("#iconSearch").addClass(iconSearch);
                $("#generarAsistencia").removeClass("btnWait");
                MensajeError(resultado.statusText, false);

            }
        });

        return false;

    });
    function customizeToolbar(toolbar) {
        var tabs = toolbar.getTabs(); // get all tabs from the toolbar
        toolbar.getTabs = function () {
            console.log(tabs);
            delete tabs[0]; // delete the first tab
            delete tabs[1];
            delete tabs[2];
            delete tabs[3].menu[1]; // borrar exportar html
            return tabs;
        }
    }
});