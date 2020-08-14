var config2 = {
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
var config3 = {
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
var modal_lv = 0;
var DatosOrdenes=null;
$('.modal').on('shown.bs.modal', function (e) {
    $('.modal-backdrop:last').css('zIndex', 1051 + modal_lv);
    $(e.currentTarget).css('zIndex', 1052 + modal_lv);
    modal_lv++
});

$('.modal').on('hidden.bs.modal', function (e) {
    modal_lv--
});
$(document).ready(function () {
    //$('#txtTemperaturaTermDigital').mask('9?9.99');
    $('#txtTemperaturaTermDigital').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 1, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.9' });
    $('#txtTemperaturaProductoSalida').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 1, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.9' });
    $('#txtTemperaturaInicial').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 1, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.9' });

    $('#txtPanelInicio').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 1, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.9' });
    $('#txtPanelMedio').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 1, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.9' });
    $('#txtPanelFinal').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 1, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.9' });

    $('#txtChartInicio').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 1, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.9' });
    $('#txtChartMedio').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 1, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.9' });
    $('#txtChartFinal').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 1, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.9' });

    $('#txtTermometroDigitalInicio').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 1, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.9' });
    $('#txtTermometroDigitalMedio').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 1, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.9' });
    $('#txtTermometroDigitalFinal').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 1, 'digitsOptional': false, 'max': '999.9' });


    $('#txtPresionManometroInicio').inputmask({
        'alias': 'integer',
        'min': '0',
        'max': '9999',
        'allowMinus': 'false',
        'allowPlus': 'false'
    });
    $('#txtPresionManometroMedio').inputmask({
        'alias': 'integer',
        'min': '0',
        'max': '9999',
        'allowMinus': 'false',
        'allowPlus': 'false'
    });
    $('#txtPresionManometroFinal').inputmask({
        'alias': 'integer',
        'min': '0',
        'max': '9999',
        'allowMinus': 'false',
        'allowPlus': 'false'
    });
    //--
    $('#txtm3h1Inicio').inputmask({
        'alias': 'integer',
        'min': '0',
        'max': '9999',
        'allowMinus': 'false',
        'allowPlus': 'false'
    });
    $('#txtm3h2Inicio').inputmask({
        'alias': 'integer',
        'min': '0',
        'max': '9999',
        'allowMinus': 'false',
        'allowPlus': 'false'
    });
    $('#txtm3h1Medio').inputmask({
        'alias': 'integer',
        'min': '0',
        'max': '9999',
        'allowMinus': 'false',
        'allowPlus': 'false'
    });
    $('#txtm3h2Medio').inputmask({
        'alias': 'integer',
        'min': '0',
        'max': '9999',
        'allowMinus': 'false',
        'allowPlus': 'false'
    });
    $('#txtm3h1Final').inputmask({
        'alias': 'integer',
        'min': '0',
        'max': '9999',
        'allowMinus': 'false',
        'allowPlus': 'false'
    });
    $('#txtm3h2Final').inputmask({
        'alias': 'integer',
        'min': '0',
        'max': '9999',
        'allowMinus': 'false',
        'allowPlus': 'false'
    });

    //--
    $('#Turno').prop('selectedIndex', 1);
    //$('#Linea').prop('selectedIndex', 1);
    //ConsultarCabControl();
    LLenarComboOrdenes();
});
function ValidarCabecera() {
    var valida = true;
    if ($('#Fecha').val() == '') {
        $('#Fecha').css('borderColor', '#FA8072');
        valida = false;
        //$('#msjerrorIngreso').prop('hidden', false);

    } else {
        $('#Fecha').css('borderColor', '#ced4da');
        //$('#msjerrorIngreso').prop('hidden', true);
    }
    if ($('#Turno').prop('selectedIndex') == 0) {
        $('#Turno').css('borderColor', '#FA8072');
        valida = false;
        //$('#msjerrorIngreso').prop('hidden', false);

    } else {
        $('#Turno').css('borderColor', '#ced4da');
        //$('#msjerrorIngreso').prop('hidden', true);
    }
    if ($('#Linea').prop('selectedIndex') == 0) {
        $('#Linea').css('borderColor', '#FA8072');
        valida = false;
        //$('#msjerrorIngreso').prop('hidden', false);

    } else {
        $('#Linea').css('borderColor', '#ced4da');
        //$('#msjerrorIngreso').prop('hidden', true);
    }
    if ($('#cmbOrdeneFabricacion').val() == 0) {
        $('#cmbOrdeneFabricacion').css('borderColor', '#FA8072');
        valida = false;
        //$('#msjerrorIngreso').prop('hidden', false);

    } else {
        $('#cmbOrdeneFabricacion').css('borderColor', '#ced4da');
        //$('#msjerrorIngreso').prop('hidden', true);
    }
    return valida;
}
function ValidaVacio(input) {
    //console.log(input.value);
    if (input.value != '') {
        $(input).css('borderColor', '#ced4da');
    } else {
        $('#' + input.id).css('borderColor', '#FA8072');
    }
    //if($('#'+input.id).)
}
function GuardarCabEsterilizacion() {
    if (!ValidarCabecera()) {
        return;
    }
    var UnidadPrresion = false;
    var AutoclaveConv = false;
    if ($('#ckbunidadpresion').prop('checked')) {
        UnidadPrresion = true;
    }
    if ($('#ckbconvencionales').prop('checked')) {
        AutoclaveConv = true;
    }
    $('#btnCargando').prop('hidden', false);
    $('#btnConsultar').prop('hidden', true);
    $('#btnLimpiar').prop('hidden', true);
    $('#btnEliminarCabeceraControl').prop('hidden', true);
    $('#btnGuardar').prop('hidden', true);
    $.ajax({
        url: "../EsterilizacionConserva/GuardarModificarCabeceraEsterilizacion",
        type: "POST",
        data: {
            IdCabControlEsterilizado: $('#CabeceraControl').val(),
            Fecha: $("#Fecha").val(),
            Turno: $("#Turno").val(),
            TipoLinea: $("#Linea").val(),
            Observacion: $("#Observacion").val(),
            UnidadPresion: UnidadPrresion,
            AutoclaveConvencional: AutoclaveConv,
            OrdenFabircacion: $('#cmbOrdeneFabricacion').val(),
            Pcc: $('#cmbpcc').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
            $('#btnLimpiar').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('hidden', false);
            $('#btnGuardar').prop('hidden', false);
            $('#CabeceraControl').val(resultado[2].IdCabControlEsterilizado);
            if (resultado[0] == "000") {
                MensajeCorrecto("Registro ingresado con éxito");
                $('#btnEliminarCabeceraControl').prop('disabled', false);
                $('#Fecha').prop('disabled', true);
                $('#Turno').prop('disabled', true);
                $('#Linea').prop('disabled', true);
            }
            if (resultado[0] == "001") {
                MensajeCorrecto("Registro actualizado con éxito");
                $('#btnEliminarCabeceraControl').prop('disabled', false);
                $('#Fecha').prop('disabled', true);
                $('#Turno').prop('disabled', true);
                $('#Linea').prop('disabled', true);
            }
            if (resultado[0] == "002") {
                MensajeAdvertencia("Error, el registro ya existe");
            }
            if (resultado[0] != "002") {
                ConsultarCoches();
            }
            
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
            $('#btnLimpiar').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('hidden', false);
            $('#btnGuardar').prop('hidden', false)
            //$('#btnConsultar').prop("disabled", false);
            //$("#spinnerCargando").prop("hidden", true);
        }
    });
}
function ConsultarCabControl() {
    //if ($('#Fecha').val() == '') {
    //    $('#msjerrorfecha').prop('hidden', false);
    //    return false;
    //} else {
    //    $('#msjerrorfecha').prop('hidden', true);
    //}
    //if ($('#Turno').prop('selectedIndex') == 0) {
    //    $('#msjerrorturno').prop('hidden', false);
    //    return false;
    //} else {
    //    $('#msjerrorturno').prop('hidden', true);
    //}
    //if ($('#Linea').prop('selectedIndex') == 0) {
    //    $('#msjerrorLinea').prop('hidden', false);
    //    return false;
    //} else {
    //    $('#msjerrorLinea').prop('hidden', true);
    //}
    //if ($('#cmbOrdeneFabricacion').val() == '') {
    //    $('#msjerrorOrdeneFabricacion').prop('hidden', false);
    //    return false;
    //} else {
    //    $('#msjerrorOrdeneFabricacion').prop('hidden', true);
    //}
    if (!ValidarCabecera()) {
        return;
    }
    LimpiarControlesDetalle();
    $('#btnCargando').prop('hidden', false);
    $('#btnConsultar').prop('hidden', true);
    $('#btnLimpiar').prop('hidden', true);
    $('#btnEliminarCabeceraControl').prop('hidden', true);
    $('#btnGuardar').prop('hidden', true);
    //$.ajax({
    //    url: "../EsterilizacionConserva/ConsultarCabeceraEsterilizacion",
    //    type: "POST",
    //    data: {
    //        Fecha: $("#Fecha").val(),
    //        Turno: $("#Turno").val(),
    //        TipoLinea: $("#Linea").val()
    //    },
    //    success: function (resultado) {
    //        if (resultado == "101") {
    //            window.location.reload();
    //        }
    //        $('#btnCargando').prop('hidden', true);
    //        $('#btnConsultar').prop('hidden', false);
    //        $('#btnLimpiar').prop('hidden', false);
    //        $('#btnEliminarCabeceraControl').prop('hidden', false);
    //        $('#btnGuardar').prop('hidden', false);
    //        $('#Fecha').prop('disabled', true);
    //        $('#Turno').prop('disabled', true);
    //        $('#Linea').prop('disabled', true);
    //        $('#btnEliminarCabeceraControl').prop('disabled', false);
    //        if (resultado == "0") {
    //            $("#MensajeRegistros").html('No se encontraron registros');
    //            $('#btnEliminarCabeceraControl').prop('disabled',true)
    //        } else {
    //            $("#MensajeRegistros").html('');
    //            $('#CabeceraControl').val(resultado.IdCabControlEsterilizado);
    //            $('#Observacion').val(resultado.Observacion);
    //            ConsultarDetalleControl();
    //            ConsultarCoches();
    //        }
    //    },
    //    error: function (resultado) {
    //        MensajeError(resultado.responseText, false);
    //        $('#btnCargando').prop('hidden', true);
    //        $('#btnConsultar').prop('hidden', false);
    //        $('#btnLimpiar').prop('hidden', false);
    //        $('#btnEliminarCabeceraControl').prop('hidden', false);
    //        $('#btnGuardar').prop('hidden', false)
         
    //    }
    //});

    //--FETCH
    const data = new FormData();
    data.append('Fecha', $("#Fecha").val());
    data.append('Turno', $("#Turno").val());
    data.append('TipoLinea', $("#Linea").val());
    data.append('OrdenFabircacion', $("#cmbOrdeneFabricacion").val());
    
    fetch("../EsterilizacionConserva/ConsultarCabeceraEsterilizacion", {
        method: 'POST',
        body: data
    }).then(function (resultado) { 
        return resultado.json();
        }) 
        .then(function(resultado){
            console.log(resultado);
            if (resultado == "101") {
                window.location.reload();
            }
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
            $('#btnLimpiar').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('hidden', false);
            $('#btnGuardar').prop('hidden', false);
            $('#Fecha').prop('disabled', true);
            $('#Turno').prop('disabled', true);
            $('#Linea').prop('disabled', true);
            $('#btnEliminarCabeceraControl').prop('disabled', false);
            if (resultado == "0") {
                $("#MensajeRegistros").html('No se encontraron registros');
                $('#btnEliminarCabeceraControl').prop('disabled', true)
            } else {
                console.log(resultado.Pcc);
                $("#MensajeRegistros").html('');
                $('#CabeceraControl').val(resultado.IdCabControlEsterilizado);
                $('#Observacion').val(resultado.Observacion);
                $('#cmbpcc').val(resultado.Pcc);
                if (resultado.UnidadPresion == true) {
                    $('#ckbunidadpresion').prop('checked', true);
                } else {
                    $('#ckbunidadpresion').prop('checked', false);
                }
                if (resultado.AutoclaveConvencional == true) {
                    $('#ckbconvencionales').prop('checked', true);
                } else {
                    $('#ckbconvencionales').prop('checked', false);
                }
                ConsultarDetalleControl();
                ConsultarCoches();
            }
        })
        .catch(function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
            $('#btnLimpiar').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('hidden', false);
            $('#btnGuardar').prop('hidden', false)
        })
    //
}
function ConsultarCoches() {
   
    $.ajax({
        url: "../EsterilizacionConserva/PartialCocheAutoclave",
        type: "GET",
        data: {
            //Fecha: $("#Fecha").val(),
            OrdenFabricacion:$('#cmbOrdeneFabricacion').val(),
            Turno: $("#Turno").val(),
            CabControl: $('#CabeceraControl').val(),
            Linea: $('#Linea').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $('#DivCoches').html('');
                $('#MensajeRegistros').html('No hay Coches Autoclave disponibles');
                return;
            } else {
                $('#MensajeRegistros').html('');
            }
            
            $('#DivCoches').html(resultado);
            config.opcionesDT.pageLength = 5;
            config.opcionesDT.buttons = [];
            $('#tblDataTable').DataTable(config.opcionesDT);

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
    
        }
    });

    //fetch("../EsterilizacionConserva/PartialCocheAutoclave", {
    //    method: 'POST',
    //    body: {
    //        Fecha: $("#Fecha").val(),
    //        Turno: $("#Turno").val(),
    //        CabControl: $('#CabeceraControl').val()
    //    }
    //}).then(function (response) {
    //    console.log(response.json)
    //    })
    //    .then(function (resultado) {
    //        if (resultado == "101") {
    //            window.location.reload();
    //        }
    //        $('#DivCoches').html(resultado);
    //        config.opcionesDT.pageLength = 5;
    //        config.opcionesDT.buttons = [];
    //        $('#tblDataTable').DataTable(config.opcionesDT);
    //    })
    //    .catch(function (error) {
    //        console.log(error);
    //        MensajeError(error.responseText, false);
    //    })
}
function AbrirRegion(evt, NombreTab) {
    //console.log(evt);
    //console.log(NombreTab);
   // $('#btnipro').css('background', 'transparent');

    var i, tabcontent, tablinks;
    //$('#btnipro').css('background', 'transparent');
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(NombreTab).style.display = "block";
    evt.currentTarget.className += " active";
}

function AgregarCocheAControl(data) {
    $('#txtTemperaturaInicial').val('');
    $('#DivNuevoDetalleEsterilizacion').prop('hidden', false);
    $('#txtAutoclave').val(data.Autoclave);
    $('#txtParada').val(data.Parada);
    $('#txtProducto').val(data.Producto);
    $('#idcochehide').val(data.IdCocheAutoclave);
    AbrirRegion(event, 'InicioProceso');
    $("#btnipro").addClass("tablinks active");
    $('#ModalDetalle').modal('show');
}
function GuardarDetalleEsterilizacion() {
 
    $('#btnCargandoDet').prop('hidden', false);
    $('#btnGuardarDetalleControl').prop('hidden', true);
    $('#btnLimpiarDetalleControl').prop('hidden', true);
    $('#btnEliminarDetalleControl').prop('hidden', true);
    //$('#btnGuardar').prop('hidden', true);
    var Tipo = [
        {
            Panel: $('#txtPanelInicio').val(),
            Chart: $('#txtChartInicio').val(),
            TermometroDigital: $('#txtTermometroDigitalInicio').val(),
            PresionManometro: $('#txtPresionManometroInicio').val(),
            HoraChequeo: $('#txtHoraChequeoInicio').val(),
            IdDetalleControlEsterilizacion: $('#IdDetalleControl').val(),
            Tipo: 'I',
            M3H1: $('#txtm3h1Inicio').val(),
            M3H2: $('#txtm3h2Inicio').val()
        },
        {
            Panel: $('#txtPanelMedio').val(),
            Chart: $('#txtChartMedio').val(),
            TermometroDigital: $('#txtTermometroDigitalMedio').val(),
            PresionManometro: $('#txtPresionManometroMedio').val(),
            HoraChequeo: $('#txtHoraChequeoMedio').val(),
            IdDetalleControlEsterilizacion: $('#IdDetalleControl').val(),
            Tipo: 'M',
            M3H1: $('#txtm3h1Medio').val(),
            M3H2: $('#txtm3h2Medio').val()
        },
        {
            Panel: $('#txtPanelFinal').val(),
            Chart: $('#txtChartFinal').val(),
            TermometroDigital: $('#txtTermometroDigitalFinal').val(),
            PresionManometro: $('#txtPresionManometroFinal').val(),
            HoraChequeo: $('#txtHoraChequeoFinal').val(),
            IdDetalleControlEsterilizacion: $('#IdDetalleControl').val(),
            Tipo: 'F',
            M3H1: $('#txtm3h1Final').val(),
            M3H2: $('#txtm3h2Final').val()
        }
    ];
    
    $.ajax({
        url: "../EsterilizacionConserva/GuardarModificarDetalleControl",
        type: "POST",
        data: {
            TIPO_ESTERILIZACION_CONSERVA: Tipo,
            IdDetalleControlEsterilizacionConserva: $('#IdDetalleControl').val(),
            IdCabeceraCoche: $('#idcochehide').val(),
            IdCabControlEsterilizacionConservas: $('#CabeceraControl').val(),
            TemperaturaInicial: $('#txtTemperaturaInicial').val(),
            HoraInicioViento: $('#txtHoraInicioVenteoo').val(),
            HoraCierreViento: $('#txtHoraCierreVenteo').val(),
            TemperaturaTermDigital: $('#txtTemperaturaTermDigital').inputmask('unmaskedvalue'),
            HoraInicioLlenado: $('#txtHoraLlenado').val(),
            HoraInicioCalentamiento: $('#txtHoraInicioCalentamiento').val(),
            HoraInicioEsterilizacion: $('#txtHoraInicioEsterilizacion').val(),
            HoraFinalEsterilizacion: $('#txtHoraFinalEsterilizacion').val(),
            TiempoEnfriamiento: $('#txtTiempoEnfriamiento').val(),
            TemperaturaProductoSalida: $('#txtTemperaturaProductoSalida').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            //$('#IdDetalleControl').val(resultado[2].IdDetalleControlEsterilizacionConserva);
            if (resultado[0] == '000') {
                MensajeCorrecto('Registro ingresado con éxito');
            }
            if (resultado[0] == '001') {
                MensajeCorrecto('Registro actualizado con éxito');
            }
            
            //cargar tabla de detalles de esa cabecera
            //  $('#btnConsultar').prop("disabled", true);
            $('#btnCargandoDet').prop('hidden', true);
            $('#btnGuardarDetalleControl').prop('hidden', false);
            $('#btnLimpiarDetalleControl').prop('hidden', false);
            $('#btnEliminarDetalleControl').prop('hidden', false);
            ConsultarCoches();
            $('#ModalDetalle').modal('hide');
            ConsultarDetalleControl();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnCargandoDet').prop('hidden', true);
            $('#btnGuardarDetalleControl').prop('hidden', false);
            $('#btnLimpiarDetalleControl').prop('hidden', false);
            $('#btnEliminarDetalleControl').prop('hidden', false);
        }
    });
}
function ConsultarDetalleControl() {
    
    $.ajax({
        url: "../EsterilizacionConserva/PartialDetalleControl",
        type: "POST",
        data: {
            idCabecera: $('#CabeceraControl').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $('#DivDetallesEsterilizacion').empty();
            if (resultado != 0) {
                $('#DivDetallesEsterilizacion').html(resultado);
                config.opcionesDT.pageLength = 5;
                config.opcionesDT.buttons = [];
            }
            
            $('#tblDetalleControl').DataTable(config.opcionesDT);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);

        }
    });
}
function AbrirModalTipos(ideDetalle, tipo) {
    var table = $('#modaltableTipo');
    table.DataTable().destroy();
    table.DataTable().clear();
    table.DataTable().draw();
    $.ajax({
        url: "../EsterilizacionConserva/ConsultarTipoEsterilizacion",
        type: "GET",
        data: {
            idDetalle: ideDetalle,
            Tipo:tipo
        },
        success: function (resultado) {
            //console.log(resultado);
            if (resultado == "101") {
                window.location.reload();
            }
            //$('#DivDetallesEsterilizacion').html(resultado);
            //config.opcionesDT.pageLength = 5;
            //config.opcionesDT.buttons = [];
            //$('#tblDetalleControl').DataTable(config.opcionesDT);
            $("#modaltableTipo tbody").empty();


            config2.opcionesDT.order = [];
            config2.opcionesDT.buttons = [];
            config2.opcionesDT.paging = false;
            config2.opcionesDT.searching = false;
            config2.opcionesDT.ordering = false;
            config2.opcionesDT.info = false;
            config2.opcionesDT.columns = [
                { data: 'Inicio' },
                { data: 'Medio' },
                { data: 'Final' }
            ];
            table.DataTable().destroy();
            table.DataTable(config2.opcionesDT);
            //table.DataTable().draw();
            table.DataTable().clear();
            var ListTipo;
            if (tipo == 'HoraChequeo') {
                ListTipo = [{
                    Inicio: moment(resultado.Inicio).format('YYYY-MM-DD, h:mm:ss'),
                    Medio: moment(resultado.Medio).format('YYYY-MM-DD, h:mm:ss'),
                    Final: moment(resultado.Final).format('YYYY-MM-DD, h:mm:ss')
                }];
            } else {
                ListTipo = [{
                    Inicio: resultado.Inicio,
                    Medio: resultado.Medio,
                    Final: resultado.Final
                }];
            }
            
            table.DataTable().rows.add(ListTipo);
            table.DataTable().draw();
            $('#ModalTipo').modal('show');
            
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);

        }
    });
}
function AbrirModalTiposM3H(ideDetalle) {
    $.ajax({
        url: "../EsterilizacionConserva/ConsultarTipoEsterilizacionM3H",
        type: "GET",
        data: {
            idDetalle: ideDetalle
        },
        success: function (resultado) {
            //console.log(resultado);
            if (resultado == "101") {
                window.location.reload();
            }
            resultado.Inicio = resultado.Inicio == null ? '' : resultado.Inicio;
            resultado.Inicio2 = resultado.Inicio2 == null ? '' : resultado.Inicio2;
            resultado.Medio = resultado.Medio == null ? '' : resultado.Medio;
            resultado.Medio2 = resultado.Medio2 == null ? '' : resultado.Medio2;
            resultado.Final = resultado.Final == null ? '' : resultado.Final;
            resultado.Final2 = resultado.Final2 == null ? '' : resultado.Final2;
            $("#modaltableTipoM3h tbody").empty();
            $('#modaltableTipoM3h tbody').append('<tr><td>' + resultado.Inicio + '</td><td>' + resultado.Inicio2 + '</td><td>' + resultado.Medio + '</td><td>' + resultado.Medio2 + '</td><td>' + resultado.Final + '</td><td>' + resultado.Final2 +'</td></tr >');
            $('#ModalTipoM3h').modal('show');

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);

        }
    });
}
function EditarDetalleControl(data) {
    $('#idcochehide').val('');
    $('#btnEliminarDetalleControl').prop('disabled', false);
    $('#divCohesAgreg').prop('hidden', true);
    $('#DivNuevoDetalleEsterilizacion').prop('hidden', false);
    $('#txtAutoclave').val(data.Autoclave);
    $('#txtParada').val(data.Esterilizada);
    $('#txtProducto').val(data.Producto);
    $('#IdDetalleControl').val(data.IdDetalleControlEsterilizacionConserva),
    $('#txtTemperaturaInicial').val(data.TemperaturaInicial);
    $('#txtHoraInicioVenteoo').val(data.HoraInicioViento);
    $('#txtHoraCierreVenteo').val(data.HoraCierreViento);
    $('#txtTemperaturaTermDigital').val(data.TemperaturaTermDigital);
    $('#txtHoraLlenado').val(data.HoraInicioLlenado);
    $('#txtHoraInicioCalentamiento').val(data.HoraInicioCalentamiento);
    $('#txtHoraInicioEsterilizacion').val(data.HoraInicioEsterilizacion);
    $('#txtHoraFinalEsterilizacion').val(data.HoraFinalEsterilizacion);
    $('#txtTiempoEnfriamiento').val(data.TiempoEnfriamiento);
    $('#txtTemperaturaProductoSalida').val(data.TemperaturaProductoSalida);
    AbrirRegion(event, 'InicioProceso');
    $("#btnipro").addClass("tablinks active");
    
    $.ajax({
        url: "../EsterilizacionConserva/ConsultarTipoEsterilizacionTodos",
        type: "GET",
        data: {
            idDetalle: data.IdDetalleControlEsterilizacionConserva
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            //console.log(resultado);
            var PanelInicio = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo=='I' })
                .Select(function (x) { return x.Panel })
                .SingleOrDefault();
            var PanelMedio = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'M' })
                .Select(function (x) { return x.Panel })
                .SingleOrDefault();
            var PanelFinal = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'F' })
                .Select(function (x) { return x.Panel })
                .SingleOrDefault();
            //--
            var TermometroDigitalInicio = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'I' })
                .Select(function (x) { return x.TermometroDigital })
                .SingleOrDefault();
            var TermometroDigitalMedio = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'M' })
                .Select(function (x) { return x.TermometroDigital })
                .SingleOrDefault();
            var TermometroDigitalFinal = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'F' })
                .Select(function (x) { return x.TermometroDigital })
                .SingleOrDefault();
            //--
            var ChartInicio = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'I' })
                .Select(function (x) { return x.Chart })
                .SingleOrDefault();
            var ChartMedio = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'M' })
                .Select(function (x) { return x.Chart })
                .SingleOrDefault();
            var ChartFinal = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'F' })
                .Select(function (x) { return x.Chart })
                .SingleOrDefault();
            //--
            var PresionManometroInicio = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'I' })
                .Select(function (x) { return x.PresionManometro })
                .SingleOrDefault();
            var PresionManometroMedio = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'M' })
                .Select(function (x) { return x.PresionManometro })
                .SingleOrDefault();
            var PresionManometroFinal = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'F' })
                .Select(function (x) { return x.PresionManometro })
                .SingleOrDefault();
            //--
            var HoraChequeoInicio = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'I' })
                .Select(function (x) { return x.HoraChequeo })
                .SingleOrDefault();
            var HoraChequeoMedio = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'M' })
                .Select(function (x) { return x.HoraChequeo })
                .SingleOrDefault();
            var HoraChequeoFinal = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'F' })
                .Select(function (x) { return x.HoraChequeo })
                .SingleOrDefault();
            //--
            //--
            var M3h1Inicio = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'I' })
                .Select(function (x) { return x.M3H1 })
                .SingleOrDefault();
            var M3h1Medio = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'M' })
                .Select(function (x) { return x.M3H1 })
                .SingleOrDefault();
            var M3h1Final = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'F' })
                .Select(function (x) { return x.M3H1 })
                .SingleOrDefault();
            var M3h2Inicio = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'I' })
                .Select(function (x) { return x.M3H2 })
                .SingleOrDefault();
            var M3h2Medio = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'M' })
                .Select(function (x) { return x.M3H2 })
                .SingleOrDefault();
            var M3h2Final = Enumerable.From(resultado)
                .Where(function (x) { return x.Tipo == 'F' })
                .Select(function (x) { return x.M3H2 })
                .SingleOrDefault();
            //--
            //console.log(HoraChequeoInicio);
            $('#txtm3h1Inicio').val(M3h1Inicio);
            $('#txtm3h2Inicio').val(M3h2Inicio);
            $('#txtm3h1Medio').val(M3h1Medio);
            $('#txtm3h2Medio').val(M3h2Medio);
            $('#txtm3h1Final').val(M3h1Final);
            $('#txtm3h2Final').val(M3h2Final);
            $('#txtPanelInicio').val(PanelInicio);
            $('#txtPanelMedio').val(PanelMedio);
            $('#txtPanelFinal').val(PanelFinal);
            $('#txtChartInicio').val(ChartInicio);
            $('#txtChartMedio').val(ChartMedio);
            $('#txtChartFinal').val(ChartFinal);
            $('#txtTermometroDigitalInicio').val(TermometroDigitalInicio);
            $('#txtTermometroDigitalMedio').val(TermometroDigitalMedio);
            $('#txtTermometroDigitalFinal').val(TermometroDigitalFinal);
            $('#txtPresionManometroInicio').val(PresionManometroInicio);
            $('#txtPresionManometroMedio').val(PresionManometroMedio);
            $('#txtPresionManometroFinal').val(PresionManometroFinal);
            $('#txtHoraChequeoInicio').val(moment(HoraChequeoInicio).format('YYYY-MM-DDTHH:mm:ss'));
            $('#txtHoraChequeoMedio').val(moment(HoraChequeoMedio).format("YYYY-MM-DDTHH:mm:ss"));
            $('#txtHoraChequeoFinal').val(moment(HoraChequeoFinal).format("YYYY-MM-DDTHH:mm:ss"));
            $('#ModalDetalle').modal('show');
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);

        }
    });
}
function LimpiarControlesDetalle() {
    $('#txtm3h1Inicio').val('');
    $('#txtm3h2Inicio').val('');
    $('#txtm3h1Medio').val('');
    $('#txtm3h2Medio').val('');
    $('#txtm3h1Final').val('');
    $('#txtm3h2Final').val('');
    $('#idcochehide').val('');
    $('#btnEliminarDetalleControl').prop('disabled', true);
    $('#txtAutoclave').val('');
    $('#txtParada').val('');
    $('#txtProducto').val('');
    $('#IdDetalleControl').val(''),
    $('#txtTemperaturaInicial').val('');
    $('#txtHoraInicioVenteoo').val('');
    $('#txtHoraCierreVenteo').val('');
    $('#txtTemperaturaTermDigital').val('');
    $('#txtHoraLlenado').val('');
    $('#txtHoraInicioCalentamiento').val('');
    $('#txtHoraInicioEsterilizacion').val('');
    $('#txtHoraFinalEsterilizacion').val('');
    $('#txtTiempoEnfriamiento').val('');
    $('#txtTemperaturaProductoSalida').val('');
    $('#txtPanelInicio').val('');
    $('#txtPanelMedio').val('');
    $('#txtPanelFinal').val('');
    $('#txtChartInicio').val('');
    $('#txtChartMedio').val('');
    $('#txtChartFinal').val('');
    $('#txtTermometroDigitalInicio').val('');
    $('#txtTermometroDigitalMedio').val('');
    $('#txtTermometroDigitalFinal').val('');
    $('#txtPresionManometroInicio').val('');
    $('#txtPresionManometroMedio').val('');
    $('#txtPresionManometroFinal').val('');
    $('#txtHoraChequeoInicio').val('');
    $('#txtHoraChequeoMedio').val('');
    $('#txtHoraChequeoFinal').val('');
    $('#divCohesAgreg').prop('hidden', false);
    $('#DivNuevoDetalleEsterilizacion').prop('hidden', true);
    $('#ModalDetalle').modal('hide');
}
function MostrarDetalleCoche(IdCabCoche) {
    var table = $('#modaltableDetCoche');
    table.DataTable().destroy();
    table.DataTable().clear();
    table.DataTable().draw();
    $.ajax({
        url: "../EsterilizacionConserva/ConsultarCochesDet",
        type: "GET",
        data: {
            IdCabCoche: IdCabCoche
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#modaltableDetCoche tbody").empty();


            config3.opcionesDT.order = [];
            config3.opcionesDT.buttons = [];
            config3.opcionesDT.paging = false;
            config3.opcionesDT.searching = false;
            config3.opcionesDT.ordering = false;
            config3.opcionesDT.info = false;
            config3.opcionesDT.columns = [
                { data: 'Tarjeta' },
                { data: 'HoraInicio' },
                { data: 'Lote' }
            ];
            table.DataTable().destroy();
            table.DataTable(config3.opcionesDT);
            //table.DataTable().draw();
            table.DataTable().clear();
           
            $.each(resultado, function (i, item) {
                resultado[i].HoraInicio = moment(resultado[i].HoraInicio).format('YYYY-MM-DD, h:mm:ss')
            });
            table.DataTable().rows.add(resultado);
            table.DataTable().draw();
            $('#ModalDetalleCoche').modal('show');
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);

        }
    });
}
function LimpiarControles() {
    $('#SelectOrdenFabricacion').prop('selectedIndex',0)
    $('#cmbOrdeneFabricacion').val('');
    $("#MensajeRegistros").html('');
    $('#DivDetallesEsterilizacion').empty();
    $('#DivCoches').empty();
    $('#btnEliminarCabeceraControl').prop('disabled', true);
    $('#Fecha').prop('disabled', false);
    $('#Turno').prop('disabled', false);
    $('#Linea').prop('disabled', false);
    $('#CabeceraControl').val(0);
    $('#Fecha').val(moment().format("YYYY-MM-DD"));
    $('#Observacion').val('');
    $('#Turno').prop('selectedIndex', 0);
    $('#Linea').prop('selectedIndex', 0);
    $('#msjerrorfecha').prop('hidden', true);
    $('#msjerrorturno').prop('hidden', true);
    $('#msjerrorLinea').prop('hidden', true);
    $('#DivNuevoDetalleEsterilizacion').prop('hidden', true);
    $('#ckbunidadpresion').prop('checked', false);
    $('#ckbconvencionales').prop('checked', false);
}
function EliminarCabControl() {
    $('#btnsicab').prop('disabled', true);
    $.ajax({
        url: "../EsterilizacionConserva/EliminarCabControl",
        type: "POST",
        data: {
            idCabecera: $('#CabeceraControl').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $('#btnsicab').prop('disabled', false);
            MensajeCorrecto(resultado[1]);
            $('#ModalEliminarCab').modal('hide');
            LimpiarControles();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnsicab').prop('disabled', false);
        }
    });
}
function ConfirmarEliminarCab() {
    $('#ModalEliminarCab').modal('show');
}
function ConfirmarELiminarDetalleControl() {
    $('#ModalEliminarDetalle').modal('show');
}
function EliminarDetalleControl() {
    $('#btnsidet').prop('disabled', true);
    $.ajax({
        url: "../EsterilizacionConserva/EliminarDetalleControl",
        type: "POST",
        data: {
            idDetalle: $('#IdDetalleControl').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $('#btnsidet').prop('disabled', false);
            MensajeCorrecto(resultado[1]);
            $('#ModalEliminarDetalle').modal('hide');
            LimpiarControlesDetalle();
            ConsultarDetalleControl();
            ConsultarCoches();
 
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnsidet').prop('disabled', false);
        }
    });
}
$("#btnOrden").on("click", function () {
    $("#ModalOrdenes").modal('show');
});
async function LlenarComboOrdenesAjax() {
    let params = {
        Fecha: $("#txtFechaOrden").val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');
    let url = '../General/ConsultaOFNivel2?' + query;
    var promesa = fetch(url);
    return promesa;
}
async function LLenarComboOrdenes(/*orden*/) {
    try {
        $('#txtCliente').val('');
        //if ($('#txtFechaProduccion').val() == '') {
        //    $('#msjErrorFechaProduccion').prop('hidden', false);
        //    return;
        //} else {
        //    $('#msjErrorFechaProduccion').prop('hidden', true);
        //}
        $('#SelectOrdenFabricacion').empty();
        $('#SelectOrdenFabricacion').append('<option> Seleccione</option>');
        if (!$('#txtFechaOrden').val() == '') {
            var PromesaConsultar = await LlenarComboOrdenesAjax();
            if (!PromesaConsultar.ok) {
                throw "Error";
            }
            var ResultadoConsultar = await PromesaConsultar.json();
            DatosOrdenes = ResultadoConsultar;
            console.log(ResultadoConsultar);
            if (ResultadoConsultar == "101") {
                window.location.reload();
            }
            $.each(ResultadoConsultar, function (key, value) {
                $('#SelectOrdenFabricacion').append('<option value=' + value.ORDEN_FABRICACION + '>' + value.ORDEN_FABRICACION + '</option>');
            });

        }
    } catch (ex) {
        MensajeError('Error comuníquese con el departamento de Sistemas, ' + ex.message, false);
    }


}
$("#modal-orden-si").on("click", function () {
    if ($("#SelectOrdenFabricacion").prop('selectedIndex') == 0) {
        $('#validaOrden').prop("hidden", false);
        return;
    }
    $("#cmbOrdeneFabricacion").val($("#SelectOrdenFabricacion").val());
    var queryResult =
         Enumerable.From(DatosOrdenes)
        .Where(function (x) { return x.ORDEN_FABRICACION == $("#SelectOrdenFabricacion").val() })
        .Select(function (x) { return x.ItemName })
        .SingleOrDefault();
    var Linea=queryResult.charAt(0) + queryResult.charAt(1);
    //console.log(queryResult);
    //console.log(Linea);
    if (Linea == 'AT') {
        $("#Linea").val('L');
    } else {
        $("#Linea").val('P');
    }
    //CargarLotes($("#SelectOrdenFabricacion").val());
    //DatosOrdenFabricacion();

    $("#ModalOrdenes").modal('hide');
    $('#validaOrden').prop("hidden", true);

    ConsultarCabControl();

});
