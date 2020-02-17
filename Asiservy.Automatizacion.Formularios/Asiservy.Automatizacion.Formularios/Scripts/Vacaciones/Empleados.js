config.opcionesDT.order = [[0, "asc"]];
var table = $("#tblDataTable");
table.DataTable().destroy()
table.DataTable(config.opcionesDT);
