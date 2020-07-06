var DatosCabecera = [];
var model = [];
$(document).ready(function () {
    ConsultarMonitoreoDescongelado();
    //console.log(Tipo);
    $('#txtTemperaturaAgua').inputmask({
        'alias': 'integer',
        'groupSeparator': ',',
        'autoGroup': true,
        'digitsOptional': true,
        'max': '100',
        'min': '-100'
    });


    Muestra.forEach(function (x, values) {
        $('#txtMuestra-' + x.IdMuestra).inputmask({
            'alias': 'decimal',
            'groupSeparator': ',',
            'digits': 2,
            'autoGroup': true,
            'digitsOptional': true,
            'max': '100.00'
        });
    });


    $.fn.datetimepicker.Constructor.Default = $.extend({}, $.fn.datetimepicker.Constructor.Default, {
        icons: {
            time: 'far fa-clock',
            date: 'far fa-calendar-alt',
            up: 'fas fa-caret-up',
            down: 'fas fa-caret-down',
            previous: 'fas fa-backward',
            next: 'fas fa-forward',
            today: 'fas fa-calendar-day',
            clear: 'fas fa-trash-alt',
            close: 'fas fa-window-close'
        }
    });

    $('#datetimepicker1').datetimepicker(
        {
            date: moment().format("YYYY-MM-DD HH:mm"),
            format: "DD-MM-YYYY HH:mm",
            minDate: moment($('#txtFecha').val(), "YYYY-MM-DD HH:mm"),
            maxDate: moment().add(1, 'days').format("YYYY-MM-DD"),
            ignoreReadonly: true
        });




});

function ValidaEstadoReporte(Fecha){
    $.ajax({
        url: "../MonitoreoDescongelado/ValidaEstadoReporte",
        type: "GET",
        data: {
            Fecha: Fecha,
            Turno: $("#selectTurno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
           // console.log(resultado);
           // console.log(resultado == 0);
            if (resultado == 0) {
                $("#lblAprobadoPendiente").html("");

            } else if (resultado==1) {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);

            } else {
                $("#lblAprobadoPendiente").removeClass("badge-info").addClass("badge-danger");
                $("#lblAprobadoPendiente").html(Mensajes.Pendiente);
            }
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}

function ConsultarMonitoreoDescongelado() {
    $("#chartCabecera2").html('');
    if ($("#txtFecha").val() == '' || $("#selectTurno").val()=='') {
        $("#divCabecera2").prop("hidden", true);
        return;
    }
    if (moment($("#txtFecha").val()).format("YYYY-MM-DD") > moment().add(1, 'days').format("YYYY-MM-DD")) {
        $("#txtFecha").val("");
        MensajeAdvertencia("Fecha no permitida");
        return;
    }
    ValidaEstadoReporte($("#txtFecha").val());
    $("#divCabecera2").prop("hidden", false);
    $("#spinnerCargando").prop("hidden", false);
  //  ConsultarPeliduvios();
    MostrarModalCargando();
    $.ajax({
        url: "../MonitoreoDescongelado/MonitoreoDescongeladoPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val(),
            Turno: $("#selectTurno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#chartCabecera2").html('<div class="text-center"><h4 class="text-warning">' + Mensajes.SinRegistros + '</h4></div>');
                $("#spinnerCargando").prop("hidden", true);
            } else {
                $("#spinnerCargando").prop("hidden", true);
                $("#chartCabecera2").html(resultado);
                config.opcionesDT.pageLength = -1;
                config.opcionesDT.order = [[2, "asc"]];;

                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            //  $('#btnConsultar').prop("disabled", true);
            CerrarModalCargando();
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            $("#spinnerCargando").prop("hidden", true);
            CerrarModalCargando();

        }
    });
}

function nuevoControl(){
    $("#txtHora").css('borderColor', '#ced4da');
}

function SeleccionarControl(model) {
    nuevoControl();
    DatosCabecera = model;
    //console.log(DatosCabecera);
    $("#txtTanque").val(DatosCabecera.U_SYP_TANQUE);
    $("#txtLote").val(DatosCabecera.U_SYP_LOTE);
    $("#txtTipo").val($("#selectTipo option:selected").text());
    $("#txtTemperaturaAgua").val('');

    var TA = Enumerable.From(Tipo)
        .Where(function (x) { return x.IdTipoMonitoreo == $("#selectTipo").val() })
        .Select(function (x) { return x.TemperaturaAgua })
        .ToArray();

   if (TA[0]) {
        $("#divTemperaturaAgua").prop("hidden",false);
    } else {
        $("#divTemperaturaAgua").prop("hidden", true);
    }
  
    if ($("#selectTurno").val() == "") {
        $("#selectTurno").css('borderColor', '#FA8072');
        return;
    } else {
        $("#selectTurno").css('borderColor', '#ced4da');
    }
      
    ConsultarMonitoreoDetalle();
    
}

function ConsultarMonitoreoDetalle() {
    MostrarModalCargando();
    $.ajax({
        url: "../MonitoreoDescongelado/MonitoreoDescongeladoDetallePartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val(),
            Turno: $("#selectTurno").val(),
            Tanque: DatosCabecera.U_SYP_TANQUE,
            Lote: DatosCabecera.U_SYP_LOTE,
            Tipo: $("#selectTipo").val()
        },
        success: function (resultado) {
            //console.log(resultado);
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#txtIdControl").val(0);
                $("#txtHora").val(moment().format("DD-MM-YYYY HH:mm"));
                Muestra.forEach(function (x) {
                    $("#txtMuestra-" + x.IdMuestra).val("");
                });

                $("#txtObservacion").val('');
                $("#ModalMonitoreo").modal("show");
                $("#btnEliminar").prop("hidden", true);
                model = [];
            } else {
                $("#txtHora").val(moment(resultado[0].Hora).format("DD-MM-YYYY HH:mm"));
               // console.log(resultado);
                Muestra.forEach(function (y) {

                    var queryResult = Enumerable.From(resultado)
                        .Where(function (x) { return x.IdMuestra == y.IdMuestra})
                        .Select(function (x) { return x.Cantidad })
                        .ToArray();

                    $("#txtMuestra-" + y.IdMuestra).val(queryResult[0]);


                  //  console.log(queryResult);

                });
                $("#txtIdControl").val(resultado[0].IdMonitoreoDescongelado);
                $("#txtTemperaturaAgua").val(resultado[0].TemperaturaAgua);
                $("#txtObservacion").val(resultado[0].Observacion);
                $("#ModalMonitoreo").modal("show");
                $("#btnEliminar").prop("hidden", false);
                model = resultado[0];
            }
            $("#btnGuardarMonitoreo").prop("disabled", false);
            CerrarModalCargando();

            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            //console.log(resultado);
            MensajeError("Error: Comuníquese con sistemas", false);         
            CerrarModalCargando();

        }
    });
}


function Validar() {
    var valida = true;
    if ($("#txtHora").val() == "") {
        $("#txtHora").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHora").css('borderColor', '#ced4da');
    }
       
    return valida;
}

function GuardarMonitoreoDescongelado() {
    if (!Validar()) {
        return;
    }
    var obj = [];
    Muestra.forEach(function (x) {
        if ($("#txtMuestra-" + x.IdMuestra).val() != '') {
            obj.push({ IdMuestra: x.IdMuestra, Cantidad: $("#txtMuestra-" + x.IdMuestra).val() });
        }
    });
    if (obj.length == 0) {
        MensajeAdvertencia("Ingrese al menos una muestra.");
        return;
    }
    MostrarModalCargando();
    //console.log($('#datetimepicker1').datetimepicker('viewDate').format("YYYY-MM-DD HH:mm"));
    //return;
    $.ajax({
        url: "../MonitoreoDescongelado/MonitoreoDescongelado",
        type: "POST",
        data: {
            IdMonitoreoDescongelado: $("#txtIdControl").val(),
            Fecha: $("#txtFecha").val(),
            Turno: $("#selectTurno").val(),
            Tanque: DatosCabecera.U_SYP_TANQUE,
            Lote: DatosCabecera.U_SYP_LOTE,
            Especie: DatosCabecera.U_SYP_ESPECIE,
            Talla: DatosCabecera.U_SYP_TALLA,
            Hora: $('#datetimepicker1').datetimepicker('viewDate').format("YYYY-MM-DD HH:mm"),
            IdTipoMonitoreo: $("#selectTipo").val(),
            TemperaturaAgua: $("#txtTemperaturaAgua").val(),
            //Muestra1: $("#txtMuestra1").val(),
            //Muestra2: $("#txtMuestra2").val(),
            //Muestra3: $("#txtMuestra3").val(),
            Observacion: $("#txtObservacion").val(),
            Detalle: obj

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
            } else {
                MensajeCorrecto(resultado);    
                ConsultarMonitoreoDescongelado();
            }
            $("#ModalMonitoreo").modal("hide");
            CerrarModalCargando();

        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            CerrarModalCargando();

        }
    });
}


function EliminarControl() {
    $("#ModalMonitoreo").modal("hide");
    $("#modalEliminarControl").modal("show");
   // console.log(model);
    //$("#txtModalTipo").val(model.Tipo=='D'?'Descongelado':model.tipo=='E'?'Emparrillado':'Ingreso a Cocina');
    if (model.Tipo == 'D') {
        $("#txtModalTipo").val('Descongelado');
    } else if (model.Tipo=='E') {
        $("#txtModalTipo").val('Emparrillado');

    } else {
        $("#txtModalTipo").val('Ingreso a Cocina');

    }

    $("#txtModalTanque").val(DatosCabecera.U_SYP_TANQUE);
    $("#txtModalLote").val(DatosCabecera.U_SYP_LOTE);
    $("#txtModalHora").val(moment(model.Hora).format("HH:mm"));
}


$("#modal-detalle-si").on("click", function () {
    InactivarControl();
    $("#modalEliminarControl").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControl").modal('hide');
});


function InactivarControl() {
    $.ajax({
        url: "../MonitoreoDescongelado/EliminarMonitoreoDescongelado",
        type: "POST",
        data: {
            IdMonitoreoDescongelado: $("#txtIdControl").val(),
            Fecha: $("#txtFecha").val(),
            Turno: $("#selectTurno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            }else if (resultado == 1) {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
                return;
            } else if (resultado == 0) {
                MensajeAdvertencia("Faltan Parametros");
            } else {
                MensajeCorrecto(resultado);
            }
            ConsultarMonitoreoDescongelado();
            NuevoControl();
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

