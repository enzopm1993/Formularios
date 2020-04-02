using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Nomina
{
    public class ModeloVistaRetornaAsistencia
    {
        public List<ModeloVistaPersonalPresente> modeloVistaPersonalPresentes { get; set; }

        public List<ModeloVistaPersonalPresenteBiometrico> modeloVistaPersonalPresentesBiometrico { get; set; }

        public List<App.ClsKpiLineasASistentes> LineasAsistentesTotales { get; set; }
    }
}