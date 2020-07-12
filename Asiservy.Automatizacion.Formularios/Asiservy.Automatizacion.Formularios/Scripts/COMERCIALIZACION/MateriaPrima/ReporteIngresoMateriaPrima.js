$(document).ready(function () {
    ComboAnio();
    Pivot();
});
function ComboAnio() {
    var n = (new Date()).getFullYear()
    var select = document.getElementById("selectAnio");
    for (var i = n; i >= 2015; i--) {
        select.options.add(new Option(i, i));
    }
}

function customizeToolbar(toolbar) {
    var tabs = toolbar.getTabs(); // get all tabs from the toolbar
    toolbar.getTabs = function () {
        delete tabs[0]; // delete the first tab
        delete tabs[1];
        delete tabs[2];
        delete tabs[3].menu[1]; // borrar exportar html
        return tabs;
    }
}

function Pivot() {
    var resultado = [{ "Barco": "Tuna I", "Grupo": "Grupo Natoli I", "Origen": "NO", "ZC": "FAO-87", "Calificacion": "Buena" }, { "Barco": "Tuna II", "Grupo": "Grupo Natoli II", "Origen": "NO", "ZC": "FAO-87", "Calificacion": "Buena" } ];

    var _fechas = [];
    $.each(resultado, function (i, rowObj) {
        //if (jQuery.inArray(rowObj.Barco, _fechas) === -1) {
            _fechas.push(rowObj.Barco);
        //}
    });

    var tuplesColum = [];
    $.each(_fechas, function (i, _rowFecha) {
        tuplesColum.push({ "tuple": ["Barco." + _rowFecha] });
    });


    console.log(tuplesColum);
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
                        "uniqueName": "Barco"
                    },
                    {
                        "uniqueName": "Grupo"
                    },
                    {
                        "uniqueName": "ZC"
                    }
                ],
                "columns": [
                    {
                        "uniqueName": "Measures"
                    },
                    {
                        "uniqueName": "Barco"
                    },
                    {
                        "uniqueName": "Grupo",
                        "sort": "desc"
                    }, {
                        "uniqueName": "ZC",
                        "sort": "desc"
                    }
                ],
                "measures": [
                    {
                        "uniqueName": "Barco",
                        "aggregation": "count"
                    }
                ],
                "expands": {
                    "rows": [
                        {
                            "tuple": [
                                "CentroBarco.Limpieza de pescado"
                            ]
                        }
                    ],
                    "columns": tuplesColum
                }
            },
            "options": {
                "grid": {
                    "showTotals": "rows",
                    "showGrandTotals": "columns"
                }
            }
        },
        global: {
            // replace this path with the path to your own translated file
            localization: config.baseUrl + "Content/webdatarocks/es.json"
        }
    });
}