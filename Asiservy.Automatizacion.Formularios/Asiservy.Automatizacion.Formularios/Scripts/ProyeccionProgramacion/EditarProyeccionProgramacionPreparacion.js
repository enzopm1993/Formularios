$(document).ready(function () {
    ValidaProyeccion();
});



function ValidaProyeccion() {
   
    $("#DivTablePreparacion").html('');
    $("#DivMensaje").html("");
    if ($('#txtFechaProduccion').val() == "" || $('#selectTurno').val() == "") {
        return;
    }
    $.ajax({
        url: "../ProyeccionProgramacion/ValidarProyeccionProgramacionPreparacion",
        type: "GET",
        data:
        {
            Fecha: $('#txtFechaProduccion').val(),
            Turno: $("#selectTurno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
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
            else if (resultado.Codigo == 2 || resultado.Codigo == 4) //Esta siendo Editado o ha sido cerrado
            { 
                $("#DivButtons").prop("hidden", true);
                $("#btnGenerarProyecion").prop("hidden", true);
                $("#IdProyeccion").val(resultado.Observacion);
               // MensajeAdvertencia(resultado.Mensaje);
                $("#DivMensaje").html("<h3 class'text-center'>" + resultado.Mensaje + " </h3> ");
                $("#txtValidaEditar").val("0");
                CargarProyeccionProgramacion();
            } else if (resultado.Codigo ==3) //ha sido finalizado
            {
                $("#DivButtons").prop("hidden", true);
                $("#btnGenerarProyecion").prop("hidden", true);
                $("#IdProyeccion").val(resultado.Observacion);
                $("#btnHabilitar").prop("hidden", false);      
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
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == 0) {
                $("#DivMensaje").html("<h3 class'text-center'> No existen registros </h3> ");
            } else {
                $("#DivTablePreparacion").html(resultado);
                config.opcionesDT.pageLength = 50;
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
    if (!validaHora("#txtFechaCoccionInicio"))
        valida = false;
    if (!validaHora("#txtFechaCoccionFin"))
        valida = false;
    if (!validaHora("#txtFechaEviceradoInicio"))
        valida = false;
    if (!validaHora("#txtFechaEviceradoFin"))
        valida = false;
    if (!validaHora("#txtFechaDescongeladoInicio"))
        valida = false;
    if (!validaHora("#txtFechaDescongeladoFin"))
        valida = false;
    if (!validaHora("#txtFechaRequerimiento"))
        valida = false;
    if (!validaHora("#txtReceta"))
        valida = false;

    if (!validaCocina())
        valida = false;

    if ($("#txtLote").val() == '') {
        valida = false;
        $("#txtLote").css('border-color', '#DC143C');
    } else {
        $("#txtLote").css('border-color', '#858796');  
    }

    
   
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
    cocinas = cocinas.slice(0, -1);
   

    $.ajax({
        url: "../ProyeccionProgramacion/GuardarModificarProyeccionProgramacionDetalle",
        type: "POST",
        data:
        {
            IdProyeccionProgramacionDetalle: $('#txtIdProgramacion').val(),
            TemperaturaFinal: $('#txtTemperatura').val(),
            HoraCoccionInicio: $('#txtFechaCoccionInicio').val(),
            HoraCoccionFin: $('#txtFechaCoccionFin').val(),
            TotalCoches: $('#txtCoches').val(),
            Cocina: cocinas,
            HoraEviceradoInicio: $('#txtFechaEviceradoInicio').val(),
            HoraEviceradoFin: $('#txtFechaEviceradoFin').val(),
            HoraDescongeladoInicio: $('#txtFechaDescongeladoInicio').val(),
            HoraDescongeladoFin: $('#txtFechaDescongeladoFin').val(),
            Requerimiento: $('#txtFechaRequerimiento').val(),
            Observacion: $('#txtObservacion').val(),
            RecetaRoceado: $('#txtReceta').val(),            
            Lote: $("#txtLote").val(),
            proceso: 3
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
         //  console.log(resultado);
            if (resultado.Codigo == 1) {
                $("#validaCocina").prop("hidden", false);
                $("#validaCocina").text(resultado.Mensaje);
                $("#btnCocina").css('color', '#DC143C'); 
                return;
            }
            if (resultado.Codigo == 2) {
                $("#validaCocina").prop("hidden", false);
                $("#txtFechaCoccionInicio").css('border-color', '#DC143C');
                $("#txtFechaCoccionFin").css('border-color', '#DC143C');
                 $("#validaCocina").text(resultado.Mensaje);
               
                return;
            }
            if (resultado.Codigo == 3) {
                $("#txtFechaEviceradoInicio").css('border-color', '#DC143C');
                $("#txtFechaEviceradoFin").css('border-color', '#DC143C');
                $("#validaCocina").prop("hidden", false);
                $("#validaCocina").text(resultado.Mensaje);
             
                return;
            }
            if (resultado.Codigo == 4) {
                $("#txtFechaDescongeladoInicio").css('border-color', '#DC143C');
                $("#txtFechaDescongeladoFin").css('border-color', '#DC143C');
                $("#validaCocina").prop("hidden", false);
                $("#validaCocina").text(resultado.Mensaje);
               
                return;
            }
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

        if (model.HoraCoccionFin != null) {
            var FechaCoccionFin = moment(model.HoraCoccionFin).format("YYYY-MM-DDTHH:mm");
        } else {
            var coccionFin = CalculoHora(model.HoraProcesoInicio, enfriado);
            var FechaCoccionFin = moment(coccionFin).format("YYYY-MM-DDTHH:mm");
        }
        $("#txtFechaCoccionFin").val(FechaCoccionFin);

        if (model.HoraCoccionInicio != null) {
            var FechaCoccionInicio = moment(model.HoraCoccionInicio).format("YYYY-MM-DDTHH:mm");          
        } else {
            var coccionInicio = fechaCalculoHora(FechaCoccionFin, moment(model.HoraCoccionFin).format("HH:mm"), coccion);
            var FechaCoccionInicio = moment(coccionInicio).format('YYYY-MM-DDTHH:mm')        
        }       
        $("#txtFechaCoccionInicio").val(FechaCoccionInicio);  
    
        
        $("#txtFechaProd").val($("#txtFechaProduccion").val());

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
        $("#txtCoches").val(model.TotalCoches);      
        $("#txtCocina").val(model.Cocina);      
        $("#txtPeso").val(model.Toneladas);      
        $("#txtEspecie").val(model.Especie);      
        $("#txtTalla").val(model.Talla);      
        $('#txtReceta').val(model.CodRecetaRoceado);
        
        $("#txtFechaEviceradoInicio").val(moment(model.HoraEviceradoInicio).format("YYYY-MM-DDTHH:mm"));
        $("#txtFechaEviceradoFin").val(moment(model.HoraEviceradoFin).format("YYYY-MM-DDTHH:mm"));      

        $("#txtFechaDescongeladoInicio").val(moment(model.HoraDescongeladoInicio).format("YYYY-MM-DDTHH:mm"));
        $("#txtFechaDescongeladoFin").val(moment(model.HoraDescongeladoFin).format("YYYY-MM-DDTHH:mm"));           
      
        $("#txtFechaRequerimiento").val(model.Requerimiento);      
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
    $("#txtFechaCoccionInicio").val('00:00');
    $("#txtFechaCoccionInicio").css('border-color', '#d1d3e2');
    $("#txtFechaCoccionFin").val('00:00');
    $("#txtFechaCoccionFin").css('border-color', '#d1d3e2');
    $("#txtFechaEviceradoInicio").val('00:00');
    $("#txtFechaEviceradoInicio").css('border-color', '#d1d3e2');
    $("#txtFechaEviceradoFin").val('00:00');
    $("#txtFechaEviceradoFin").css('border-color', '#d1d3e2');
    $("#txtFechaDescongeladoInicio").val('00:00');
    $("#txtFechaDescongeladoInicio").css('border-color', '#d1d3e2');
    $("#txtFechaDescongeladoFin").val('00:00');
    $("#txtFechaDescongeladoFin").css('border-color', '#d1d3e2');
    $("#txtRequerimiento").val();
    $("#txtFechaRequerimiento").val('00:00');
    $("#txtFechaRequerimiento").css('border-color', '#d1d3e2');
    $("#txtLote").val('');
    $("#txtLote").css('border-color', '#d1d3e2');
    $("#txtObservacion").val('');
    $('#txtReceta').prop("selectIndex",0);
    $("#txtReceta").css('border-color', '#d1d3e2');
    $("#btnCocina").css('color', '#858796');  
    $("#validaCocina").prop("hidden", true);
    $("input[type=checkbox]").each(function () {
        if ($(this).prop('checked')) {
            $(this).prop('checked', false);
        }
    });

}
function CalculoHora(hora1, hora2) {       
    time1 = hora1.split(/:/);
    time2 = hora2.split(/:/);  
    var fecha1 = new moment($("#txtFechaProduccion").val());    
    fecha1.set({ hour: time1[0], minute: time1[1], second: 0, millisecond: 0 });   
    var fecha = fecha1.subtract(time2[0], 'hours');
    fecha = fecha.subtract(time2[1], 'minutes');   
    return fecha;
}

function fechaCalculoHora(fecha,hora1, hora2) {

   // console.log(fecha);
    time1 = hora1.split(/:/);
    time2 = hora2.split(/:/);
  
    if (fecha != '') {
        var fecha1 = new moment(fecha);
    } else {
        var fecha1 = new moment($("#txtFechaProduccion").val());
    }
    fecha1.set({ hour: time1[0], minute: time1[1], second: 0, millisecond: 0 });
    var fecha = fecha1.subtract(time2[0], 'hours');
    fecha = fecha.subtract(time2[1], 'minutes');
    return fecha;
}


function CalcularEnfriadoCoccion() {
    var enfriado = $("#txtHoraEnfriado").val();
    var coccion = $("#txtHoraCoccion").val();
    if (enfriado != '') {
        var tiempo = CalculoHora($("#txtHoraProcesoInicio").val(), enfriado);
        $("#txtFechaCoccionFin").val(moment(tiempo).format("YYYY-MM-DDTHH:mm"));          
        if (coccion != '') {
            var FechaCoccionFinal = moment($("#txtFechaCoccionFin").val()).format("YYYY-MM-DDTHH:mm");
            var HoraCoccionFinal = moment($("#txtFechaCoccionFin").val()).format("HH:mm");           
            var tiempococcion = fechaCalculoHora(FechaCoccionFinal, HoraCoccionFinal, coccion);
    //        $("#txtFechaCoccionInicio").val(moment(tiempococcion).format("HH:mm"));
          //  console.log(tiempococcion);
            $("#txtFechaCoccionInicio").val(moment(tiempococcion).format("YYYY-MM-DDTHH:mm"));
        }
    } else {
    //    $("#txtFechaCoccionFin").val('');
        $("#txtFechaCoccionInicio").val('');
    }
    CalcularDescongeladoFin();
}

function CambiarHoraEnfriado(valor) {
    $("#txtHoraEnfriado").val(valor);
    CalcularEnfriadoCoccion()
}
function CambiarHoraCoccion(valor) {
    $("#txtHoraCoccion").val(valor);
    CalcularEnfriadoCoccion()
}


function CalcularEviceradoFin() {

}


function CalcularDescongeladoFin() {
  //  console.log($("#txtFechaEviceradoInicio").val());
    if ($("#txtFechaEviceradoInicio").val() == '') {
        $("#txtFechaEviceradoInicio").val('');
        console.log("borrar");
        return;
    }   
  //  eviceradoinicio = $("#txtFechaEviceradoInicio").val();
    Fechaeviceradoinicio = $("#txtFechaEviceradoInicio").val();
    if (Fechaeviceradoinicio != '') {
      //  $("#txtFechaDescongeladoFin").val(eviceradoinicio);        
        $("#txtFechaDescongeladoFin").val(Fechaeviceradoinicio);  
        CalcularDescongeladoInicio();
    } else {
     //   $("#txtFechaDescongeladoFin").val('');
        $("#txtFechaDescongeladoFin").val('');
    }
}


function CalcularDescongeladoInicio() {
    var descongeladofin = $("#txtFechaDescongeladoFin").val();
    var enfriado = $("#txtHoraEnfriado").val();
    if (descongeladofin != '' && enfriado != '') {
        var FechaDesconFinal = moment(descongeladofin).format("YYYY-MM-DDTHH:mm");
        var HoraDesconFinal = moment(descongeladofin).format("HH:mm");    
        var tiempo = fechaCalculoHora(FechaDesconFinal, HoraDesconFinal, enfriado);
       
        $("#txtFechaDescongeladoInicio").val(moment(tiempo).format("YYYY-MM-DDTHH:mm"));
        CalcularRequerimiento();
    } else {    
        $("#txtFechaDescongeladoInicio").val('');
    }
}

function CalcularRequerimiento() {
    var minutos = $("#txtRequerimiento").val();
    var descongeladoinicio = moment($("#txtFechaDescongeladoInicio").val()).format("HH:mm");  
    if (minutos > 0 && descongeladoinicio != '') {
        time1 = descongeladoinicio.split(/:/);
        var fecha1 = new moment($("#txtFechaDescongeladoInicio").val());
        fecha1.set({ hour: time1[0], minute: time1[1], second: 0, millisecond: 0 });       
        var fecha = fecha1.subtract(minutos, 'minutes');           
        $("#txtFechaRequerimiento").val(moment(fecha).format("YYYY-MM-DDTHH:mm"));
    } else {
        $("#txtFechaRequerimiento").val('');
     
    }
}


function FinalizarProyeccionProgramacion() {
    $.ajax({
        url: "../ProyeccionProgramacion/FinalizarIngresoProyeccionProgramacion",
        type: "GET",
        data:
        {
            id: $('#IdProyeccion').val(),
            proceso: 3
        },
        success: function (resultado) {
            ValidaProyeccion();
            MensajeCorrecto(resultado);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}

function HabilitarProyeccionProgramacion() {
    $.ajax({
        url: "../ProyeccionProgramacion/HabilitarIngresoProyeccionProgramacion",
        type: "GET",
        data:
        {
            id: $('#IdProyeccion').val(),
            proceso: 3
        },
        success: function (resultado) {
            ValidaProyeccion();
            MensajeCorrecto(resultado);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}

