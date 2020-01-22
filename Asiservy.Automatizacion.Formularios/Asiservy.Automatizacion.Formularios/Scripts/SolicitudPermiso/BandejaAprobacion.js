
$(document).ready(function () {
     CargarBandejaAprobacion();
});

function CargarBandejaAprobacion() {  
    $("#spinnerCargando").prop("hidden", false);
    $("#divTable").html('');
    $.ajax({
        url: "../SolicitudPermiso/BandejaAprobacionPartial",
        type: "GET",      
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }           
         
            if (resultado == "0") {
                $("#divTable").html("No existen registros");
                $("#spinnerCargando").prop("hidden", true);
            } else {
                $("#spinnerCargando").prop("hidden", true);
                $("#divTable").html(resultado);
                config.opcionesDT.pageLength = 10;
                //      config.opcionesDT.order = [[0, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);
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

function AprobarSolitudes() {
    var result = new Array();    
    i = 0;
    $("input[type=checkbox]:checked").each(function (resultado) {
        id = $(this).attr("id");
        this.id = id.replace('solicitud-', '');
        result.push(this.id);
        i++;
    });
    //console.log(result);
    Aprobar(result);    
}
function AprobarSolicitud(valor, id) {
   // console.log(valor);
    $("#" + id).prop("disabled", true);
    var solicitud=[];
    solicitud[0] = valor;
    Aprobar(solicitud, id);  
}

function Aprobar(result) {   
    $("#tdAcciones").prop("hidden", true);
    $("#btnAprobar").prop("hidden", true);
    $("#btnArpobarEspera").prop("hidden", false);
    var resultado = JSON.stringify(result)
    var resultado2 = JSON.parse(resultado)
  
    $.ajax({
        url: '../SolicitudPermiso/AprobarSolicitud',
        type: 'POST',
        dataType: "json",
        data: {
            diIdSolicitud: resultado2           
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }   
            CargarBandejaAprobacion();
            MensajeCorrecto(resultado);
            $("#btnAprobar").prop("hidden", false);
            $("#tdAcciones").prop("hidden", false);
            $("#btnArpobarEspera").prop("hidden", true);
        }
        ,
        error: function () {
            MensajeError("No se ha podido obtener la información");
            $("#btnAprobar").prop("hidden", false);
            $("#tdAcciones").prop("hidden", false);
            $("#btnArpobarEspera").prop("hidden", true);
            $("#" + id).prop("disabled", false);

        }
    });
}

function Anular() {
    valor = document.getElementById("txtIdSolicitud").value;
    Observacion = document.getElementById("txtObservaccionAnulacion").value;
    //console.log(Observacion);
    if (!Observacion || Observacion == undefined || Observacion == "" || Observacion.length == 0)
    {
        MensajeCorrecto("Debe ingresar un motivo");
    } else {
        MostrarModalCargando();
        $.ajax({
            url: '../SolicitudPermiso/AnularSolicitud',
            type: 'GET',
            data: {
                diIdSolicitud: valor,
                dsObservacion: " -Anulación: "+Observacion
            },
            success: function (resultado) {
                CerrarModalCargando();
                CargarBandejaAprobacion();
                MensajeCorrecto(resultado + "\n Solicitud Anulada");
            }
            ,
            error: function () {
                CerrarModalCargando();
                MensajeError("No se ha podido obtener la información");
            }
        });
    }
}

function ObservacionModalAnular(valor) {
   // console.log(valor);
    document.getElementById("txtObservaccionAnulacion").value = "";
    document.getElementById("txtIdSolicitud").value = valor;
    $('#ModalObservacion').modal("show");
}

function Mostrar(valor) {
    //console.log(valor);
    MostrarModalCargando();
    var sPath = window.location.pathname;
    var sPage = sPath.substring(sPath.lastIndexOf('/') + 1);
    $.ajax({
        url: '../SolicitudPermiso/SolicitudPermisoEdit',
        type: 'GET',
        data: {
            dsSolicitud: valor,
            frm: sPage
        },
        success: function (resultado) {
            CerrarModalCargando();
            document.getElementById("modal_body").innerHTML = resultado;
            $('#ModalAprobacion').modal('toggle');
        }
        ,
        error: function () {
            CerrarModalCargando();

            MensajeError("No se ha podido obtener la información",false);
        }
    });
}

