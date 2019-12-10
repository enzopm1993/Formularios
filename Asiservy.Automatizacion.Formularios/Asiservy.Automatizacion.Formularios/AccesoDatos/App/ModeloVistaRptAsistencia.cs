using Asiservy.Automatizacion.Formularios.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.App
{
    public class ModeloVistaRptAsistencia
    {
        public List<ClsRegistroAsistencia> dataGeneral { get; set; }
        public List<ClsKpiGenero> TotalGeneros { get; set; }

        public List<ClsKpiDescripcionTotal> TotalPermisos { get; set; }
        public List<ClsKpiDescripcionTotal> TotalDias { get; set; }
    }
    public class ClsKpiGenero
    {
        public String Genero { get; set; }
        public Int32 Presentes { get; set; }
        public Int32 Ausentes { get; set; }
        public Int32 AusentesConPermiso { get; set; }
        public Int32 AusentesSinPermiso { get; set; }
    }
    public class ClsKpiDescripcionTotal
    {
        public String Descripcion { get; set; }
        public Int32 Total { get; set; }
    }
    
}