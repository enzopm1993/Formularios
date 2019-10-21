(function ($) {
    "use strict"; // Start of use strict
    var indexColumE1 = 10;
    var indexColumS1 = 11;
    var indexColumCedula = 8;
    var indexColumFecha = 0;

    var $container = $("#resultsMacro");
 

    $('#txtRangfecha').daterangepicker({
        autoUpdateInput: false,
        locale: {
            cancelLabel: 'Cancelar'
        }
    });

    $('#txtRangfecha').on('apply.daterangepicker', function (ev, picker) {
        var fechaIni = picker.startDate.format('YYYY-MM-DD');
        var fechaFin = picker.endDate.format('YYYY-MM-DD');
        $(this).val(fechaIni + ' - ' + fechaFin);
        $("#cargarDatos").attr("fechaIni", fechaIni);
        $("#cargarDatos").attr("fechaFin", fechaFin);
    });

    $('#txtRangfecha').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $("#cargarDatos").attr("fechaIni", "");
        $("#cargarDatos").attr("fechaFin", "");
    });

    $("#cargarDatos").click(function () {
        var fechaIni = $(this).attr("fechaIni");
        var fechaFin = $(this).attr("fechaFin");
       
        $.ajax({
            url: config.baseUrl + config.ajaxurl + "cargarReporteMacro/" + fechaIni + "/" + fechaFin,
            dataType: "json",
            success: function (response) {
                var dataObject = response;
                $(".tituloRango").text("Datos del " + fechaIni + ' al ' + fechaFin);      
                var hotSettings = {
                    data: dataObject,
                    columns: [
                        {
                            data: 'FECHA',
                            type: 'date',
                            dateFormat: 'DD/MM/YYYY',
                            readOnly: true
                        },
                        {
                            data: 'DIA',
                            type: 'text',
                            readOnly: true
                        },                        
                        {
                            data: 'ESTADO_EMPLEADO',
                            type: 'text',
                            readOnly: true
                        },
                        {
                            data: 'EMPRESA',
                            type: 'text',
                            readOnly: true
                        },
                        {
                            data: 'LINEA',
                            type: 'text',
                            readOnly: true
                        },
                        {
                            data: 'AREA',
                            type: 'text',
                            readOnly: true
                        },
                        {
                            data: 'CARGO',
                            type: 'text',
                            readOnly: true
                        },
                        {
                            data: 'GENERO',
                            type: 'text',
                            readOnly: true
                        },
                        {
                            data: 'CEDULA',
                            type: 'text',
                            readOnly: true
                        },
                        {
                            data: 'NOMBRES',
                            type: 'text',
                            readOnly: true
                        },                        
                        {
                            data: 'E1',
                            type: 'time',
                            timeFormat: 'HH:mm',
                            width: '70px',
                            correctFormat: true
                        },
                        {
                            data: 'S1',
                            type: 'time',
                            timeFormat: 'HH:mm',
                            width: '70px',
                            correctFormat: true
                        },
                        {
                            data: 'ENT_RELOJ',
                            type: 'time',
                            timeFormat: 'HH:mm',
                            correctFormat: true
                        },
                        {
                            data: 'SAL_RELOJ',
                            type: 'time',
                            timeFormat: 'HH:mm',
                            correctFormat: true
                        },
                        {
                            data: 'ALMUERZO',
                            type: 'time',
                            readOnly: true,
                            timeFormat: 'HH:mm'
                        },
                        {
                            data: 'CENA',
                            type: 'time',
                            readOnly: true,
                            timeFormat: 'HH:mm'
                        },
                        {
                            data: 'HORAS_TRABAJADAS',
                            type: 'numeric',
                            numericFormat: {
                                pattern: '0.00'
                            },
                            readOnly: true
                        },
                        {
                            data: 'PRESENTE',
                            type: 'checkbox',
                            readOnly: true
                        },
                        {
                            data: 'PERMISO',
                            type: 'text',
                            readOnly: true
                        },
                        {
                            data: 'DIA_INICIA_PERMISO',
                            type: 'date',
                            dateFormat: 'DD/MM/YYYY',
                            readOnly: true
                        },
                        {
                            data: 'DIA_FIN_PERMISO',
                            type: 'date',
                            dateFormat: 'DD/MM/YYYY',
                            readOnly: true
                        },
                        {
                            data: 'HORA_INICIA_PERMISO',
                            type: 'time',
                            readOnly: true,
                            timeFormat: 'HH:mm'
                        },
                        {
                            data: 'HORA_FIN_PERMISO',
                            type: 'time',
                            readOnly: true,
                            timeFormat: 'HH:mm'
                        }
                    ],
                    stretchH: 'all',                   
                    autoWrapRow: true,
                    height: 400,
                    rowHeaders: true,
                    colHeaders: [
                        'FECHA',       
                        'DIA',
                        'ESTADO_EMPLEADO',
                        'EMPRESA',
                        'LINEA',
                        'AREA',
                        'CARGO',
                        'GENERO',
                        'CEDULA',
                        'NOMBRES',
                        'E1',
                        'S1',
                        'ENT_RELOJ',
                        'SAL_RELOJ',
                        'ALMUERZO',
                        'CENA',
                        'HORAS_TRABAJADAS',
                        'PRESENTE',
                        'PERMISO',
                        'DIA_INICIA_PERMISO',
                        'DIA_FIN_PERMISO',
                        'HORA_INICIA_PERMISO',
                        'HORA_FIN_PERMISO'
                    ],
                    fixedColumnsLeft: 2,
                    licenseKey: 'non-commercial-and-evaluation'
                };
                $("#resultsMacro").handsontable(hotSettings);                
            },
            error: function (error) {
            }
        });
        return false;
    });

    
    $("#getData").click(function () {    
        var cedula = '1717698821';
        var fecha = '07/10/2019';
        var handsontable = $container.data('handsontable');
        var datosExcel = getJsonExcel();
        //$.each(handsontable.getData(), function (index, value) {
        //   // console.log(value[8]);
        //    if (value[indexColumCedula] == cedula && value[indexColumFecha] == fecha ) {
        //        handsontable.setDataAtCell(index, indexColumE1, '05:00');
        //        handsontable.setDataAtCell(index, indexColumS1, '05:00');
        //    }
        //    // Will stop running after "three"
        //    //return (value !== 'three');
        //});
        
        //console.log(dataSpread);
     
        //console.log(handsontable.getData());
        return false;
    });

    function getJsonExcel() {
        var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.xlsx|.xls)$/;
        /*Checks whether the file is a valid excel file*/
        if (regex.test($("#excelfile").val().toLowerCase())) {
            var xlsxflag = false; /*Flag for checking whether excel is .xls format or .xlsx format*/
            if ($("#excelfile").val().toLowerCase().indexOf(".xlsx") > 0) {
                xlsxflag = true;
            }
            /*Checks whether the browser supports HTML5*/
            if (typeof (FileReader) != "undefined") {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var data = e.target.result;
                    /*Converts the excel data in to object*/
                    if (xlsxflag) {
                        var workbook = XLSX.read(data, { type: 'binary' });
                    }
                    else {
                        var workbook = XLS.read(data, { type: 'binary' });
                    }
                    /*Gets all the sheetnames of excel in to a variable*/
                    var sheet_name_list = workbook.SheetNames;

                    var cnt = 0; /*This is used for restricting the script to consider only first sheet of excel*/
                    sheet_name_list.forEach(function (y) { /*Iterate through all sheets*/
                        /*Convert the cell value to Json*/
                        if (xlsxflag) {
                            var exceljson = XLSX.utils.sheet_to_json(workbook.Sheets[y]);
                        }
                        else {
                            var exceljson = XLS.utils.sheet_to_row_object_array(workbook.Sheets[y]);
                        }
                        if (exceljson.length > 0 && cnt == 0) {
                            return exceljson;
                            cnt++;
                        }
                    });
                }
                if (xlsxflag) {/*If excel file is .xlsx extension than creates a Array Buffer from excel*/
                    reader.readAsArrayBuffer($("#excelfile")[0].files[0]);
                }
                else {
                    reader.readAsBinaryString($("#excelfile")[0].files[0]);
                }
            }
            else {
                alert("Sorry! Your browser does not support HTML5!");
            }
        }
        else {
            alert("Please upload a valid Excel file!");
        }
    }    

})(jQuery); // End of use strict

