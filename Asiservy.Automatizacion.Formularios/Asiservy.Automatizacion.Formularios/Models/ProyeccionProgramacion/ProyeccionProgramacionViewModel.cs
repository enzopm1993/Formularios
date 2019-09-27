﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.ProyeccionProgramacion
{
    public class ProyeccionProgramacionViewModel
    {
        public int IdProyeccion { get; set; }
        public string Lote { get; set; }
        public DateTime? FechaProduccion { get; set; }
        public int? Tonelada { get; set; }
        public string CodDestino { get; set; }
        public string Destino { get; set; }
        public  string IdTipoLimpieza { get; set; }
        public string TipoLimpieza { get; set; }
        public string Observacion { get; set; }
    }
}