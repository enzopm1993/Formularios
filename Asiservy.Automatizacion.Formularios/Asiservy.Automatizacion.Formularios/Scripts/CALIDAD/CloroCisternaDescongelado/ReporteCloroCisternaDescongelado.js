var listaDatos = [];
$(document).ready(function () { 
    CargarReporteControlCloro();    
});
function CargarReporteControlCloroCabecera() {
    listaDatos = null;  
    var fecha = $('#txtFecha').val();    
    if (fecha == '') {
        return;
    }
     else {
        $.ajax({
            url: "../CloroCisternaDescongelado/ValidarCloroCisternaDescongelado",
            type: "GET",
            data: {
                Fecha: fecha
            },
            success: function (resultado) {
                listaDatos = resultado;
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
            }
        });
    }
}
function CargarReporteControlCloro() {
    CargarReporteControlCloroCabecera();
    var fecha = $('#txtFecha').val();  
    var table = $("#tblDataTableReporte");
    table.DataTable().destroy();
    table.DataTable().clear();
    table.DataTable().draw();   
    if (fecha == '') {
        MensajeAdvertencia("Ingrese la FECHA correctamente");
        $("#txtEstado").html("");    
        //$("#txtEstado").addClass('text-danger');
        $("#DivReporteControlCloro").hide();
        return;
    } else {
        $("#spinnerCargando").prop("hidden", false);
        $.ajax({
            url: "../CloroCisternaDescongelado/ReporteCloroCisternaDescongeladoCabecera",
            type: "GET",
            data: {
                Fecha: fecha
            },
            success: function (resultado) {
                if (listaDatos.EstadoReporte == null || listaDatos.EstadoReporte == false) {
                    $("#txtEstado").html("(PENDIENTE)");
                } else {
                    $("#txtEstado").html("");
                }
                $("#spinnerCargando").prop("hidden", true);
                if (resultado == "101") {
                    window.location.reload();
                }
                if (resultado == "0") {
                    $('#MensajeRegistros').prop("hidden", false);
                    $("#DivReporteControlCloro").hide();
                    $("#txtEstado").html("");
                } else {
                    console.log(listaDatos);
                    $("#tblDataTableReporte tbody").empty();
                    $("#DivReporteControlCloro").show();
                    $('#MensajeRegistros').prop("hidden", true);
                    config.opcionesDT.order = [];

                    config.opcionesDT.columns = [
                        { data: 'Hora' },
                        { data: 'Ppm_Cloro' },
                        { data: 'Cisterna' },
                        { data: 'UsuarioIngresoLog' },
                        { data: 'Observaciones' }
                    ];

                    config.opcionesDT.aoColumnDefs = [{
                        "aTargets": [1], // Column to target
                        "mRender": function (data, type, full) {
                            var clscolor = "badge-danger";
                            if (data >= 0.3 && data <= 1.5) {
                                clscolor = "badge-success";
                            }
                            return '<span class="badge ' + clscolor + '">' + data + '</span>';
                        }
                    }];
                    table.DataTable().destroy();
                    table.DataTable(config.opcionesDT);
                    table.DataTable().clear();
                    table.DataTable().rows.add(resultado);
                    table.DataTable().draw();
                    $("#txtObservacionesCabecera").html(listaDatos.Observaciones);
                    $("#ModalApruebaCntrolCloro").modal("show");
                }
            },
            error: function (resultado) {
                MensajeError(resultado, false);
                $("#spinnerCargando").prop("hidden", true);
            }
        });
    }
}