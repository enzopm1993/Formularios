$(document).ready(function () {
    $("#search").keyup(function () {
        _this = this;
        // Show only matching TR, hide rest of them
        $.each($("#WebGrid tbody tr"), function () {
            if ($(this).text().toLowerCase().indexOf($(_this).val().toLowerCase()) === -1)
                $(this).hide();
            else
                $(this).show();
        });
    });
});

function checkTodos() {
    var i = 1;
    var bool = document.getElementById("checkTodos").checked;
    $('#WebGrid tr').each(function () {       
        var desSol="solicitud-"
        var x = $(this).find("td").eq(6).html();
        if (x != null) {
            desSol += i;
            document.getElementById(desSol).checked = bool;
            i++;
        }
    });
}

function Mostrar() {
    $("#myModal").modal("show");

}

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


function LimpiarTexto() {
    $.each($("#WebGrid tbody tr"), function () {
        $(this).show();
    });
    document.getElementById("search").innerText = "";
    $("#search").val("");
}
