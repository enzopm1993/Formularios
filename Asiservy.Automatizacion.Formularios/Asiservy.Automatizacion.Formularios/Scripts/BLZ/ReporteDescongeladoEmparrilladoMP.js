$(function () {

    var iconLoader = "fa-spinner fa-pulse";
    var iconSearch = "fa-search"
   

    var table = $("#tblDataTable");

    $("#generarReporte").click(function () {
        table.DataTable().destroy();
        table.DataTable().clear();
        table.DataTable().draw();

        var fechaPrd = $("#fechaPrd").val();


        $("#generarReporte").attr('href', "javascript:void(0)");
        $("#iconSearch").removeClass(iconSearch);
        $("#iconSearch").addClass(iconLoader);
        $("#generarReporte").addClass("btnWait");
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: "../BLZ/GeneraDescongeladoEmparrilladoMP",
            type: "GET",
            data: {
                'fechaPrd': fechaPrd
            },
            success: function (resultado) {
                $("#generarReporte").attr('href', "#");
                $("#iconSearch").removeClass(iconLoader);
                $("#iconSearch").addClass(iconSearch);
                $("#generarReporte").removeClass("btnWait");

                $("#tblDataTable tbody").empty();
                config.opcionesDT.pageLength = 15; // -1;
                config.opcionesDT.order = [];
                config.opcionesDT.columns = [
                    { data: 'U_SYP_LOTE' },
                    { data: 'ORDEN' },
                    { data: 'U_SYP_ESPECIE' },
                    { data: 'U_SYP_TALLA' },
                    { data: 'U_SYP_BARCO' },
                    { data: 'U_SYP_CLIENTE' },
                    { data: 'U_SYP_TANQUE' },
                    { data: 'VOLTEO' },
                    { data: 'U_SYP_DUCHA' },
                    { data: 'U_SYP_TC_AGUA' },
                    { data: 'HORA_INI_DESC' },
                    { data: 'U_SYP_TC_INI_DES' },
                    { data: 'HORA_FIN_DESC' },
                    { data: 'U_SYP_TC_FIN_DES' },
                    { data: 'U_SYP_TIEMPO_DES' },
                    { data: 'HORA_INI_EMPA' },
                    { data: 'HORA_FIN_EMPA' },
                    { data: 'U_SYP_RAQUEO_EMPA' },
                    { data: 'OBSERVACIONES' },
                    { data: 'U_SYP_USUARIO' }
                ];
                table.DataTable().destroy();
                table.DataTable(config.opcionesDT);
                //table.DataTable().draw();
                table.DataTable().clear();
                table.DataTable().rows.add(resultado);
                table.DataTable().draw();
            },
            error: function (resultado) {
                console.log(resultado);
                $("#generarReporte").attr('href', "#");
                $("#iconSearch").removeClass(iconLoader);
                $("#iconSearch").addClass(iconSearch);
                $("#generarReporte").removeClass("btnWait");
                MensajeError(resultado.statusText, false);

            }
        });

        return false;

    });
});