
$(document).ready(function () {
    ValidaProyeccion();
});

function Validar() {
    var valida = true;
    if ($("#txtLote").val() == "") {
        $("#ValidaLote").prop("hidden", false);
        valida = false;
    } else {
        $("#ValidaLote").prop("hidden", true);
    }

    if ($("#txtFechaProduccion").val() == "") {
        $("#ValidaFecha").prop("hidden", false);
        valida = false;
    } else {
        $("#ValidaFecha").prop("hidden", true);
    }

    if ($("#txtTonelada").val() == "") {
        $("#ValidaTonelada").prop("hidden", false);
        valida = false;
    } else {
        $("#ValidaTonelada").prop("hidden", true);
    }

    if ($("#txtDestino").val() == "") {
        $("#ValidaDestino").prop("hidden", false);
        valida = false;
    } else {
        $("#ValidaDestino").prop("hidden", true);
    }

    if ($("#txtTipoLimpieza").val() == "") {
        $("#ValidaLimpieza").prop("hidden", false);
        valida = false;
    } else {
        $("#ValidaLimpieza").prop("hidden", true);
    }

    if ($("#txtEspecie").val() == "") {
        $("#ValidaEspecie").prop("hidden", false);
        valida = false;
    } else {
        $("#ValidaEspecie").prop("hidden", true);
    }
    if ($("#txtTalla").val() == "") {
        $("#ValidaTalla").prop("hidden", false);
        valida = false;
    } else {
        $("#ValidaTalla").prop("hidden", true);
    }

    return valida;
}

function GuardarProyeccionDetalle() {
    if (!Validar()) {
        return;
    }
    $.ajax({
        url: "../ProyeccionProgramacion/GuardarModificarProyeccionProgramacionDetalle",
        type: "GET",
        data:
        {
            IdProyeccionProgramacion: $('#IdProyeccion').val(),
            IdProyeccionProgramacionDetalle: $('#IdProyeccionDetalle').val(),
            Lote: $("#txtLote").val(),
            Toneladas: $("#txtTonelada").val(),
            OrdenFabricacion: "",
            Destino: $("#txtDestino").val(),
            TipoLimpieza: $("#txtTipoLimpieza").val(),
            Especie: $("#txtEspecie").val(),
            Talla: $("#txtTalla").val(),
            Observacion: $("#txtObservacion").val()

        },
        success: function (resultado) {           
            CargarProyeccionProgramacion();
            Limpiar();
            MensajeCorrecto(resultado);

        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });

}

function GenerarProyeccionProgramacion() {  
    if (!Validar()) {
        return;
    }
    $.ajax({
        url: "../ProyeccionProgramacion/GenerarProyeccionProgramacion",
        type: "GET",
        data:
        {
            IdProyeccionProgramacion: $('#IdProyeccion').val(),
            FechaProduccion: $('#txtFechaProduccion').val()
        },
        success: function (resultado) {
            if (resultado > 0) {
                $("#IdProyeccion").val(resultado);
                GuardarProyeccionDetalle();
                $("#DivMensaje").html("");
                $("#DivButtons").prop("hidden", false);
                $("#btnGenerarProyecion").prop("hidden", true);
                $("#btnEliminar").prop("hidden", false);
                $("#btnFinalizar").prop("hidden", false);

            }
         
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}

function ValidaProyeccion() {
    if ($('#txtFechaProduccion').val() == "") {
        return;
    }
    $("#DivProyeccion").html("");
    $.ajax({
        url: "../ProyeccionProgramacion/ValidarProyeccionProgramacion",
        type: "GET",
        data:
        {
            Fecha: $('#txtFechaProduccion').val()
        },
        success: function (resultado) {

            $("#btnEliminar").prop("hidden", true);  
            $("#btnFinalizar").prop("hidden", true);  
            $("#btnHabilitar").prop("hidden", true);  
            $("#DivMensaje").html("");
            if (resultado.Codigo == 0) //no se existen registros
            {
                $("#DivButtons").prop("hidden", true);
                $("#btnGenerarProyecion").prop("hidden", false);
                $("#IdProyeccion").val(0);
                $("#DivMensaje").html("<h3 class'text-center'>" + resultado.Mensaje + " </h3> ");

            } else if (resultado.Codigo == 1) //proyeccion se encuentra en estado finalzado
            {               
                $("#DivButtons").prop("hidden", true);
                $("#btnGenerarProyecion").prop("hidden", true);
                $("#IdProyeccion").val(resultado.Observacion);
                MensajeAdvertencia(resultado.Mensaje);
                $("#DivMensaje").html("<h3 class'text-center'>" + resultado.Mensaje + " </h3> ");
                $("#btnHabilitar").prop("hidden", false);               
                CargarProyeccionProgramacion();
            } else {
                $("#DivButtons").prop("hidden", false);
                $("#btnGenerarProyecion").prop("hidden", true);
                $("#IdProyeccion").val(resultado.Observacion);
                $("#btnEliminar").prop("hidden", false);
                $("#btnFinalizar").prop("hidden", false);
                CargarProyeccionProgramacion();
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
            IdProgramacion: $('#IdProyeccion').val()
        },
        success: function (resultado) {
            if (resultado == 0) {
                $("#DivMensaje").html("<h3 class'text-center'> No existen registros </h3> ");              
            } else {
                $("#DivProyeccion").html(resultado);
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
function Limpiar() {
    $('#txtLote').val("");
    //$('#FechaProduccion').val("");
    $('#txtTonelada').val("");
    $('#txtDestino').prop('selectedIndex', 0);
    $('#txtTipoLimpieza').prop('selectedIndex', 0);
    $('#txtEspecie').prop('selectedIndex', 0);
    $('#txtTalla').prop('selectedIndex', 0);
    $('#Observacion').val("");
   // $('#IdProyeccion').val(0);
    $('#IdProyeccionDetalle').val(0);
    //var d = new Date();

    //var dia = d.getDate()+1;

    //var mes = (d.getMonth() + 1) < 10 ? ("0" + (d.getMonth() + 1)) : d.getMonth() + 1;
    //var anio = d.getFullYear();

    //var fechatotal = anio + "-" + mes + "-" + dia
    //$('#FechaProduccion').val(fechatotal);

    //ConsultaProyProgramacion();
}

function SeleccionarProyeccionProgramacion(id,idDetalle,lote,orden,toneladas,destino,limpieza,observacion,especie,talla) {
    $('#txtLote').val(lote);
    //$('#FechaProduccion').val("");
    $('#txtTonelada').val(toneladas);
    $('#txtDestino').val(destino);
    $('#txtTipoLimpieza').val(limpieza);
    $('#txtEspecie').val(especie);
    $('#txtTalla').val(talla);
    $('#Observacion').val(observacion);
    $('#IdProyeccion').val(id);
    $('#IdProyeccionDetalle').val(idDetalle);
}

function InactivarRegistro(){
    $.ajax({
        url: "../ProyeccionProgramacion/InactivarProyeccionProgramacionDetalle",
        type: "GET",
        data:
        {
            id: $('#IdProyeccion').val()
        },
        success: function (resultado) {
            Limpiar();
            ValidaProyeccion();
            MensajeCorrecto(resultado);

        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}


function FinalizarProyeccionProgramacion() {
    $.ajax({
        url: "../ProyeccionProgramacion/FinalizarIngresoProyeccionProgramacion",
        type: "GET",
        data:
        {
            id: $('#IdProyeccion').val()
        },
        success: function (resultado) {
            Limpiar();
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
            id: $('#IdProyeccion').val()
        },
        success: function (resultado) {
            Limpiar();
            ValidaProyeccion();
            MensajeCorrecto(resultado);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}

var modalConfirm = function (callback) {

    $("#btnEliminar").on("click", function () {
        $("#mi-modal").modal('show');
    });

    $("#modal-btn-si").on("click", function () {
        callback(true);
        $("#mi-modal").modal('hide');
    });

    $("#modal-btn-no").on("click", function () {
        callback(false);
        $("#mi-modal").modal('hide');
    });
};


modalConfirm(function (confirm) {
    if (confirm) {
        //Acciones si el usuario confirma
        InactivarRegistro();

    }
});

//function ConsultaProyProgramacion(){
//    $.ajax({
//        url: "../ProyeccionProgramacion/ProyeccionProgramacionPartial",
//        type: "POST",
//        data:
//        {
//            FechaProduccion: $('#FechaProduccion').val()
//        },
//        success: function (resultado) {
          
//            $('#DivProyeccion').empty();
//            $('#DivProyeccion').html(resultado);
           
//        },
//        error: function (resultado) {
//            MensajeError(JSON.stringify(resultado), false);

//        }
//    });
//}

//function LimpiarBoton() {
//    Limpiar();
//    var d = new Date();

//    var dia = d.getDate()+1;

//    var mes = (d.getMonth() + 1) < 10 ? ("0" + (d.getMonth() + 1)) : d.getMonth() + 1;
//    var anio = d.getFullYear();

//    var fechatotal = anio + "-" + mes + "-" + dia
//    $('#FechaProduccion').val(fechatotal);

//    ConsultaProyProgramacion();

//}
//function EditarProyeccion(IdProyeccion, Lote, Fecha, Toneladas, Destino, TipoLimpieza, Observacion, Especie, Talla) {
//   // alert(Fecha);
//    var FechaD = new Date(Fecha);
//    var mes = FechaD.getMonth()+1; //obteniendo mes
//    var dia = FechaD.getDate(); //obteniendo dia
//    var ano = FechaD.getFullYear(); //obteniendo año
//    if (dia < 10)
//        dia = '0' + dia; //agrega cero si el menor de 10
//    if (mes < 10)
//        mes = '0' + mes; //agrega cero si el menor de 10
//    $('#IdProyeccion').val(IdProyeccion);
//    $('#Lote').val(Lote);
//    $('#FechaProduccion').val(ano + "-" + mes + "-" + dia);
//    $('#Toneladas').val(Toneladas);
//    $('#Destino').val(Destino);
//    $('#TipoLimpieza').val(TipoLimpieza);
//    $('#Observacion').val(Observacion);
//    $('#Talla').val(Talla);
//    $('#Especie').val(Especie);
//    console.log(Especie);
//    console.log(Talla);

//}
//function IngresarProyeccionProgramacion() {
//    var idPro;
//    if ($('#IdProyeccion').val() == "") {
//        idPro = 0;
//    } else {
//        idPro = $('#IdProyeccion').val();
//    }
//    if ($('#Lote').val() == "" || $('#FechaProduccion').val() == "" || $('#Toneladas').val() == "" || $('#Destino').prop('selectedIndex') == 0 || $('#TipoLimpieza').prop('selectedIndex') == 0
//        || $('#Especie').prop('selectedIndex') == 0 || $('#Talla').prop('selectedIndex') == 0) {
//        MensajeError("Debe ingresar los campos requeridos", false);
//    } else {
//        $.ajax({
//            url: "../ProyeccionProgramacion/ProyeccionProgramacionPartial",
//            type: "POST",
//            data:
//            {
//                IdProyeccionProgramacion: idPro,
//                Lote: $('#Lote').val(),
//                FechaProduccion: $('#FechaProduccion').val(),
//                Toneladas: $('#Toneladas').val(),
//                Destino: $('#Destino').val(),
//                TipoLimpieza: $('#TipoLimpieza').val(),
//                Observacion: $('#Observacion').val(),
//                Especie: $('#Especie').val(),
//                Talla: $('#Talla').val()
//            },
//            success: function (resultado) {
//                Limpiar();
//                $('#DivProyeccion').empty();
//                $('#DivProyeccion').html(resultado);
//                MensajeCorrecto("Registro ingresado con éxito", false);
//            },
//            error: function (resultado) {
//                MensajeError(JSON.stringify(resultado), false);

//            }
//        });
//    }
    
//}
