$(document).ready(function () {
    ValidaProyeccion();
});



function ValidaProyeccion() {
    if ($('#txtFechaProduccion').val() == "") {
        return;
    }
    $("#DivTablePreparacion").html('');
    $("#DivMensaje").html("");
    $.ajax({
        url: "../ProyeccionProgramacion/ValidarProyeccionProgramacionPreparacion",
        type: "GET",
        data:
        {
            Fecha: $('#txtFechaProduccion').val()
        },
        success: function (resultado) {

            $("#btnEliminar").prop("hidden", true);
            $("#btnFinalizar").prop("hidden", true);
            $("#btnHabilitar").prop("hidden", true);           
           
            if (resultado.Codigo == 0) //no se existen registros
            {
                $("#DivButtons").prop("hidden", true);
                $("#btnGenerarProyecion").prop("hidden", false);
                $("#IdProyeccion").val(0);
                $("#DivMensaje").html("<h3 class'text-center'>" + resultado.Mensaje + " </h3> ");
                $("#txtValidaEditar").val("1");

            }
            else if (resultado.Codigo == 2 || resultado.Codigo == 4) //Esta siendo Editado o ha sido finalizado
            { 
                $("#DivButtons").prop("hidden", true);
                $("#btnGenerarProyecion").prop("hidden", true);
                $("#IdProyeccion").val(resultado.Observacion);
               // MensajeAdvertencia(resultado.Mensaje);
                $("#DivMensaje").html("<h3 class'text-center'>" + resultado.Mensaje + " </h3> ");
                $("#txtValidaEditar").val("0");
                CargarProyeccionProgramacion();
            } else {
                $("#DivButtons").prop("hidden", false);
                $("#btnGenerarProyecion").prop("hidden", true);
                $("#IdProyeccion").val(resultado.Observacion);
                $("#btnEliminar").prop("hidden", false);
                $("#btnFinalizar").prop("hidden", false);
                CargarProyeccionProgramacion();
                $("#txtValidaEditar").val("1");
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);

        }
    });
}



function CargarProyeccionProgramacion() {

    $("#spinnerCargando").prop("hidden", false);
   
    $.ajax({
        url: "../ProyeccionProgramacion/ProyeccionProgramacionDetallePartial",
        type: "GET",
        data:
        {
            IdProgramacion: $('#IdProyeccion').val(),
            proceso: 3
        },
        success: function (resultado) {
            if (resultado == 0) {
                $("#DivMensaje").html("<h3 class'text-center'> No existen registros </h3> ");
            } else {
                $("#DivTablePreparacion").html(resultado);
                config.opcionesDT.pageLength = 10;
                $('#tblDataTable').DataTable(config.opcionesDT);

            }
            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}

function validaHora(id) {
    var valor = $(id).val();  
    if (valor != '') {
        $(id).css('border-color', '#d1d3e2');
        return true;
    } else {
        $(id).css('border-color', '#DC143C');
        return false;    
    }
}
function validaCocina() {
    var cont = 0;
    $("input[type=checkbox]").each(function () {
        if ($(this).prop('checked')) {
            cont++;
        }
    });    
    if (cont == 0) {
        $("#btnCocina").css('color', '#DC143C');       
        return false;
    } else {
        $("#btnCocina").css('color', '#858796');  
        return true;
    }
}

function Validar() {
    var valida = true;
    if (!validaHora("#txtTemperatura"))
        valida = false;
    if (!validaHora("#txtCoches"))
        valida = false;
    if (!validaHora("#txtHoraCoccionInicio"))
        valida = false;
    if (!validaHora("#txtHoraCoccionFin"))
        valida = false;
    if (!validaHora("#txtHoraEviceradoInicio"))
        valida = false;
    if (!validaHora("#txtHoraEviceradoFin"))
        valida = false;
    if (!validaHora("#txtHoraDescongeladoInicio"))
        valida = false;
    if (!validaHora("#txtHoraDescongeladoFin"))
        valida = false;
    if (!validaHora("#txtHoraRequerimiento"))
        valida = false;
    if (!validaCocina())
        valida = false;
   
    return valida;   
}

function GuardarProyeccionDetalle() {
    if (!Validar()) {
        return;
    }
    var cocinas = "";
    $("input[type=checkbox]").each(function () {
        if ($(this).prop('checked')) {
            cocinas = cocinas + this.value+",";
        }
    });
    //console.log(cocinas);
    //cocinas = cocinas.slice(0, -1);
    //console.log(cocinas);

    $.ajax({
        url: "../ProyeccionProgramacion/GuardarModificarProyeccionProgramacionDetalle",
        type: "POST",
        data:
        {
            IdProyeccionProgramacionDetalle: $('#txtIdProgramacion').val(),
            TemperaturaFinal: $('#txtTemperatura').val(),
            HoraCoccionInicio: $('#txtHoraCoccionInicio').val(),
            HoraCoccionFin: $('#txtHoraCoccionFin').val(),
            TotalCoches: $('#txtCoches').val(),
            Cocina: cocinas,
            HoraEviceradoInicio: $('#txtHoraEviceradoInicio').val(),
            HoraEviceradoFin: $('#txtHoraEviceradoFin').val(),
            HoraDescongeladoInicio: $('#txtHoraDescongeladoInicio').val(),
            HoraDescongeladoFin: $('#txtHoraDescongeladoFin').val(),
            Requerimiento: $('#txtHoraRequerimiento').val(),
            Observacion: $('#txtObservacion').val(),
            proceso: 3
        },
        success: function (resultado) {
            $("#ModalEditarProyeccion").modal("hide");
            CargarProyeccionProgramacion();
            MensajeCorrecto(resultado);

        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#ModalEditarProyeccion").modal("hide");


        }
    });
   
}


function SeleccionarProyeccionProgramacion(model, ListaEnfriado, ListaCoccion) {
 
    if ($("#txtValidaEditar").val() == "1") {
        limpiarModal();
        console.log(model.Cocina);

        if (model.Cocina != null && model.Cocina!='') {
            cocinas = model.Cocina;
            cocinas = cocinas.split(',');
            $("input[type=checkbox]").each(function () {
                if (cocinas.some(x => x == $(this).val())) {
                    $(this).prop('checked', true);
                }
            });
        }
    var enfriado = "00:00:00";
    var coccion = "00:00:00";
    $.each(ListaEnfriado, function (i, item) {
        if (item.Talla == model.Talla) {
           enfriado = item.HorasDescongelado;
        }
    });
    $.each(ListaCoccion, function (i, item) {
        if (item.Talla == model.Talla){
            coccion = item.HorasCoccion;
        }
        });

        timeEnfriado = enfriado.split(/:/);
        timeCoccion = coccion.split(/:/);
        
        var coccionfin = CalculoHora(model.HoraProcesoInicio, enfriado);
        var coccioninicio = CalculoHora(coccionfin, coccion);
        $("#txtHoraEnfriado").val(timeEnfriado[0] + ":" + timeEnfriado[1]);
        $("#selectHoraEnfriado").val(timeEnfriado[0] + ":" + timeEnfriado[1]);
        $("#txtHoraCoccion").val(timeCoccion[0] + ":" + timeCoccion[1]);
        $("#selectHoraCoccion").val(timeCoccion[0] + ":" + timeCoccion[1]);
        $("#ModalEditarProyeccion").modal("show");
        $("#txtIdProgramacion").val(model.IdProyeccionProgramacionDetalle);      
        $("#txtLote").val(model.Lote);      
        $("#txtLineas").val(model.Lineas);      
        $("#txtHoraProcesoInicio").val(model.HoraProcesoInicio);      
        $("#txtHoraProcesoFin").val(model.HoraProcesoFin);            
        $("#txtTemperatura").val(model.TemperaturaFinal);      
      //  $("#txtCoccion").val(model.HoraProcesoFin);      
        $("#txtCoches").val(model.TotalCoches);      
        $("#txtCocina").val(model.Cocina);      
        $("#txtPeso").val(model.Toneladas);      
        $("#txtEspecie").val(model.Especie);      
        $("#txtTalla").val(model.Talla);      
            
        if (model.HoraCoccionInicio != null) {
            $("#txtHoraCoccionInicio").val(model.HoraCoccionInicio);   
        } else {
            $("#txtHoraCoccionInicio").val(coccioninicio);
        }
        if (model.HoraCoccionFin != null) {
            $("#txtHoraCoccionFin").val(model.HoraCoccionFin);    
        } else {
            $("#txtHoraCoccionFin").val(coccionfin);      
        }
        $("#txtHoraEviceradoInicio").val(model.HoraEviceradoInicio);      
        $("#txtHoraEviceradoFin").val(model.HoraEviceradoFin);      
        $("#txtHoraDescongeladoInicio").val(model.HoraDescongeladoInicio);      
        $("#txtHoraDescongeladoFin").val(model.HoraDescongeladoFin);      
        $("#txtRequerimiento").val(model.Requerimiento);      
       // $("#txtHoraRequerimiento").val(model.Requerimiento);      
        $("#txtObservacion").val(model.Observacion);      

        $("#btnGuardar").prop("disabled", false);
       

    }
}

function limpiarModal() {    
    $("#txtHoraEnfriado").val(0);   
    $("#txtHoraCoccion").val(0);
    $("#txtIdProgramacion").val(0);
    $("#txtLote").val('');
    $("#txtLineas").val('');
    $("#txtHoraProcesoInicio").val('00:00');
    $("#txtHoraProcesoFin").val('00:00');
    $("#txtTemperatura").val('');
    $("#txtTemperatura").css('border-color', '#d1d3e2');
    $("#txtCoches").val(0);
    $("#txtCoches").css('border-color', '#d1d3e2');
    $("#txtPeso").val(0);
    $("#txtEspecie").val('');
    $("#txtTalla").val('');
    $("#txtHoraCoccionInicio").val('00:00');
    $("#txtHoraCoccionInicio").css('border-color', '#d1d3e2');
    $("#txtHoraCoccionFin").val('00:00');
    $("#txtHoraCoccionFin").css('border-color', '#d1d3e2');
    $("#txtHoraEviceradoInicio").val('00:00');
    $("#txtHoraEviceradoInicio").css('border-color', '#d1d3e2');
    $("#txtHoraEviceradoFin").val('00:00');
    $("#txtHoraEviceradoFin").css('border-color', '#d1d3e2');
    $("#txtHoraDescongeladoInicio").val('00:00');
    $("#txtHoraDescongeladoInicio").css('border-color', '#d1d3e2');
    $("#txtHoraDescongeladoFin").val('00:00');
    $("#txtHoraDescongeladoFin").css('border-color', '#d1d3e2');
    $("#txtRequerimiento").val(0);
    $("#txtRequerimiento").val();
    $("#txtHoraRequerimiento").val('00:00');
    $("#txtHoraRequerimiento").css('border-color', '#d1d3e2');
    $("#txtObservacion").val('');       
    $("input[type=checkbox]").each(function () {
        if ($(this).prop('checked')) {
            $(this).prop('checked', false);
        }
    });

}
function CalculoHora(hora1, hora2) {
    // var horasdiferencia = (new Date("1970-1-1 " + hora2) - new Date("1970-1-1 " + hora1)) / 1000 / 60 / 60;    
    time1 = hora1.split(/:/);
    time2 = hora2.split(/:/);
    var fecha1 = new moment();
    var fecha2 = new moment();
    fecha1.set({ hour: time1[0], minute: time1[1], second: 0, millisecond: 0 });
    fecha2.set({ hour: time2[0], minute: time2[1], second: 0, millisecond: 0 });
    var fecha = fecha1.subtract(time2[0], 'hours');
    fecha = fecha.subtract(time2[1], 'minutes');
    var Tiempo = fecha.format("HH:mm");
    return Tiempo;
}

function CalcularEnfriadoCoccion() {
    var enfriado = $("#txtHoraEnfriado").val();
    var coccion = $("#txtHoraCoccion").val();
    if (enfriado != '') {
        var tiempo = CalculoHora($("#txtHoraProcesoInicio").val(), enfriado);
        $("#txtHoraCoccionFin").val(tiempo);
        if (coccion != '') {
            var tiempococcion = CalculoHora($("#txtHoraCoccionFin").val(), coccion);
            $("#txtHoraCoccionInicio").val(tiempococcion);
        }
    } else {
        $("#txtHoraCoccionFin").val('');
    }
    CalcularDescongeladoInicio();
}

function CambiarHoraEnfriado(valor) {
    $("#txtHoraEnfriado").val(valor);
    CalcularEnfriadoCoccion()
}
function CambiarHoraCoccion(valor) {
    $("#txtHoraCoccion").val(valor);
    CalcularEnfriadoCoccion()
}


function CalcularDescongeladoFin() {
    eviceradoinicio = $("#txtHoraEviceradoInicio").val();
    if (eviceradoinicio != '') {
        $("#txtHoraDescongeladoFin").val(eviceradoinicio);        
    } else {
        $("#txtHoraDescongeladoFin").val('');
    }
    CalcularDescongeladoInicio();
}


function CalcularDescongeladoInicio() {
    var descongeladofin = $("#txtHoraDescongeladoFin").val();
    var enfriado = $("#txtHoraEnfriado").val();
    if (descongeladofin != '' && enfriado != '') {
        var tiempo = CalculoHora(descongeladofin, enfriado);
        $("#txtHoraDescongeladoInicio").val(tiempo);
        CalcularRequerimiento();
    } else {
        $("#txtHoraDescongeladoInicio").val('');
    }
}

function CalcularRequerimiento() {
    var minutos = $("#txtRequerimiento").val();
    var descongeladoinicio = $("#txtHoraDescongeladoInicio").val();
    if (minutos > 0 && descongeladoinicio != '') {
        time1 = descongeladoinicio.split(/:/);
        var fecha1 = new moment();
        fecha1.set({ hour: time1[0], minute: time1[1], second: 0, millisecond: 0 });
        //var fecha = fecha1.subtract(time2[0], 'hours');
        var fecha = fecha1.subtract(minutos, 'minutes');
        var Tiempo = fecha.format("HH:mm");
        $("#txtHoraRequerimiento").val(Tiempo);
    } else {
        $("#txtHoraRequerimiento").val('');
    }
}