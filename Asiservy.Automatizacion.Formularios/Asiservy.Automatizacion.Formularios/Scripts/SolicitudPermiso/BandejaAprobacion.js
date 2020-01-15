
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
        $.ajax({
            url: '../SolicitudPermiso/AnularSolicitud',
            type: 'GET',
            data: {
                diIdSolicitud: valor,
                dsObservacion: " -Anulación: "+Observacion
            },
            success: function (resultado) {
                CargarBandejaAprobacion();
                MensajeCorrecto(resultado + "\n Solicitud Anulada");
            }
            ,
            error: function () {
                MensajeError("No se ha podido obtener la información");
            }
        });
    }
}

function Observacion(valor) {
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



//function checkTodos() {
//    var i = 1;
//    var bool = document.getElementById("checkTodos").checked;
//        console.log('prueba');

//    $('#TableBandeja tr').each(function () {       
//        var desSol="solicitud"
//        var x = $(this).find("td").eq(1).html();
//        console.log(x);
//        if (x != null) {
//            desSol += i;
//            document.getElementById(desSol).checked = bool;
//            i++;
//        }
//    });
//}



//$(document).ready(function () {
//    $("#search").keyup(function () {
//        _this = this;
//        // Show only matching TR, hide rest of them
//        $.each($("#WebGrid tbody tr"), function () {
//            if ($(this).text().toLowerCase().indexOf($(_this).val().toLowerCase()) === -1)
//                $(this).hide();
//            else
//                $(this).show();
//        });
//    });
//});



//    $("body").on("click", ".Grid tfoot a", function () {
//        $('#WebGridForm').attr('action', $(this).attr('href')).submit();
//    return false;
//});

//$('table tbody tr  td').on('click', function () {
//    $("#myModal").modal("show");
//    $("#txtSolicitud").val($(this).closest('tr').children()[0].textContent);
//    $("#txtFecha").val($(this).closest('tr').children()[1].textContent);
//    $("#txtMotivo").val($(this).closest('tr').children()[2].textContent);
//    $("#txtArea").val($(this).closest('tr').children()[3].textContent);
//    $("#txtEmpleado").val($(this).closest('tr').children()[4].textContent);

//    $("#txtSolicitud").prop('disabled', true);
//    $("#txtFecha").prop('disabled', true);
//    $("#txtMotivo").prop('disabled', true);
//    $("#txtArea").prop('disabled', true);
//    $("#txtEmpleado").prop('disabled', true);
//});
//comboFind


//function LimpiarTexto() {
//    $.each($("#TableBandejaRRHH tbody tr"), function () {
//        $(this).show();
//    });
//    document.getElementById("search").innerText = "";
//    $("#search").val("");
//}
