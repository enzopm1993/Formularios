function MensajeClose() {
    //location.reload();
    //$(this).modal('hide');
}

function MensajeCloseReload() {
    location.reload();
}

function MensajeCorrecto(mensaje,r) {
    $.ajax({
        url: "../Mensaje/Correcto",
        type: "Get",
        data: { reload: r },
        success: function (resultado) {
            var m = document.getElementById("ModalMensaje");
            m.innerHTML = resultado;
            //var modal = document.getElementById("ModalError");
            $("#ModalCorrecto").modal("show");
            document.getElementById('mensajeError').innerHTML = mensaje;
               console.log(r);
        }
    });
}


function MensajeError(mensaje, r) {
    $.ajax({
        url: "../Mensaje/Error",
        type: "Get",
        data: { reload: r },
        success: function (resultado) {
            var m = document.getElementById("ModalMensaje");
            m.innerHTML = resultado;
            //var modal = document.getElementById("ModalError");
            $("#ModalError").modal("show");
            document.getElementById('mensajeError').innerHTML = mensaje;
               console.log(mensaje);
        }
    });
}

function CargarEmpleados() {
    $.ajax({
        url: "../Mensaje/EmpleadoBuscar",
        type: "Get",
        success: function (resultado) {
            $('#ModelCargarEmpleados').html(resultado);
            $("#ModalEmpleado").modal("show");
            
        }
    });

}

