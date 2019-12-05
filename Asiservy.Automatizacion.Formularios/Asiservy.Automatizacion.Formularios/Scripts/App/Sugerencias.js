$(document).ready(function () {


    $(".cargaDatos").click(function () {

        var comentario = $(this).data("comentario");
        var fecha = $(this).data("fecha");
        $("#fechaSugerencia").html("Fecha de envío: " + fecha);
        $("#txtContentSugerencia").html(comentario);
        $("#ModalSugerencia").modal({ backdrop: 'static', keyboard: false, show: true });

        return false;
    });






});
