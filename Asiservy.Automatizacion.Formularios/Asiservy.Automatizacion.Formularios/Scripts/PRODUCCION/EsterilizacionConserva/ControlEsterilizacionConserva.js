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
$(document).ready(function () {
    //$('#txtTemperaturaTermDigital').mask('9?9.99');
    $('#txtTemperaturaTermDigital').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '99.99' });
    $('#txtTemperaturaProductoSalida').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '99.99' });
    $('#txtTemperaturaInicial').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '99.99' });

    $('#txtPanelInicio').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.99' });
    $('#txtPanelMedio').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.99' });
    $('#txtPanelFinal').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.99' });

    $('#txtChartInicio').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.99' });
    $('#txtChartMedio').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.99' });
    $('#txtChartFinal').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.99' });

    $('#txtTermometroDigitalInicio').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.99' });
    $('#txtTermometroDigitalMedio').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.99' });
    $('#txtTermometroDigitalFinal').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.99' });

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
});
function GuardarCabEsterilizacion() {
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
            Observacion: $("#Observacion").val()
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
    if ($('#Fecha').val() == '') {
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
                $("#MensajeRegistros").html('');
                $('#CabeceraControl').val(resultado.IdCabControlEsterilizado);
                $('#Observacion').val(resultado.Observacion);
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
            Fecha: $("#Fecha").val(),
            Turno: $("#Turno").val(),
            CabControl: $('#CabeceraControl').val()
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
    $('#DivNuevoDetalleEsterilizacion').prop('hidden', false);
    $('#txtAutoclave').val(data.Autoclave);
    $('#txtParada').val(data.Parada);
    $('#txtProducto').val(data.Producto);
    $('#idcochehide').val(data.IdCocheAutoclave);
    AbrirRegion(event, 'InicioProceso');
    $("#btnipro").addClass("tablinks active");
    
}
function GuardarDetalleEsterilizacion() {
 
    $('#btnCargandoDet').prop('hidden', false);
    $('#btnGuardarDetalleControl').prop('hidden', true);
    $('#btnLimpiarDetalleControl').prop('hidden', true);
    $('#btnEliminarDetalleControl').prop('hidden', true);
    $('#btnGuardar').prop('hidden', true);
    var Tipo = [
        {
            Panel: $('#txtPanelInicio').val(),
            Chart: $('#txtChartInicio').val(),
            TermometroDigital: $('#txtTermometroDigitalInicio').val(),
            PresionManometro: $('#txtPresionManometroInicio').val(),
            HoraChequeo: $('#txtHoraChequeoInicio').val(),
            IdDetalleControlEsterilizacion: $('#IdDetalleControl').val(),
            Tipo: 'I'
        },
        {
            Panel: $('#txtPanelMedio').val(),
            Chart: $('#txtChartMedio').val(),
            TermometroDigital: $('#txtTermometroDigitalMedio').val(),
            PresionManometro: $('#txtPresionManometroMedio').val(),
            HoraChequeo: $('#txtHoraChequeoMedio').val(),
            IdDetalleControlEsterilizacion: $('#IdDetalleControl').val(),
            Tipo: 'M'
        },
        {
            Panel: $('#txtPanelFinal').val(),
            Chart: $('#txtChartFinal').val(),
            TermometroDigital: $('#txtTermometroDigitalFinal').val(),
            PresionManometro: $('#txtPresionManometroFinal').val(),
            HoraChequeo: $('#txtHoraChequeoFinal').val(),
            IdDetalleControlEsterilizacion: $('#IdDetalleControl').val(),
            Tipo: 'F'
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
            $('#IdDetalleControl').val(resultado[2].IdDetalleControlEsterilizacionConserva);
            if (resultado[0] == '000') {
                MensajeCorrecto('Registro ingresado con éxito');
            }
            if (resultado[0] == '001') {
                MensajeCorrecto('Registro actualizado con éxito');
            }
            ConsultarDetalleControl();
            //cargar tabla de detalles de esa cabecera
            //  $('#btnConsultar').prop("disabled", true);
            $('#btnCargandoDet').prop('hidden', true);
            $('#btnGuardarDetalleControl').prop('hidden', false);
            $('#btnLimpiarDetalleControl').prop('hidden', false);
            $('#btnEliminarDetalleControl').prop('hidden', false);
            ConsultarCoches();
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
            //console.log(HoraChequeoInicio);
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

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);

        }
    });
}
function LimpiarControlesDetalle() {
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
                { data: 'HoraInicio' }
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
    $("#MensajeRegistros").html('');
    $('#DivDetallesEsterilizacion').empty();
    $('#DivCoches').empty();
    $('#btnEliminarCabeceraControl').prop('disabled', true);
    $('#Fecha').prop('disabled', false);
    $('#Turno').prop('disabled', false);
    $('#Linea').prop('disabled', false);
    $('#CabeceraControl').val(0);
    $('#Fecha').val('');
    $('#Observacion').val('');
    $('#Turno').prop('selectedIndex', 0);
    $('#Linea').prop('selectedIndex', 0);
    $('#msjerrorfecha').prop('hidden', true);
    $('#msjerrorturno').prop('hidden', true);
    $('#msjerrorLinea').prop('hidden', true);
    $('#DivNuevoDetalleEsterilizacion').prop('hidden',true);
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