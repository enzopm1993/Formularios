﻿function Mensaje(mensaje) {
    $.ajax({
        url: "../Mensaje/Error",
        type: "Get",
        success: function (resultado) {
            var m = document.getElementById("ModalMensaje");
            m.innerHTML = resultado;
            //var modal = document.getElementById("ModalError");
            $("#ModalError").modal("show");
            document.getElementById('mensajeError').innerHTML = mensaje;
            //   console.log(mensaje);
        }
    });
}