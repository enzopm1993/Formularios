using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia
{
    public class clsDMovimientoPersonalNomina
    {
        public string GuardarBitacoraMovimientoPersonalNomina(string Cedula,string psusuario,string psterminal, string CentroCosto, string Recurso, string Linea, string Cargo)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                db.BITACORA_MOVER_EMPLEADO.Add(new BITACORA_MOVER_EMPLEADO
                {
                    Cedula = Cedula,
                    CodCargo = Cargo,
                    CodCentroCosto = CentroCosto,
                    CodLinea = Linea,
                    CodRecurso = Recurso,
                    EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = psterminal,
                    UsuarioIngresoLog = psusuario
                });
                db.SaveChanges();
                return "Registro guardado con éxito";
            }
        }
    }
}