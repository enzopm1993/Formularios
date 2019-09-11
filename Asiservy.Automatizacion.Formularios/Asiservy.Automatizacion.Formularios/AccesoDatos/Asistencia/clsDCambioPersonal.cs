using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia
{
    public class clsDCambioPersonal
    {
       
        public string GuardarCambioDePersonal(List<CAMBIO_PERSONAL> pListCambioPersonal,List<BITACORA_CAMBIO_PERSONAL> Bitacora, string tipo)
        {
            CAMBIO_PERSONAL CambioPersonal;
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                int ContadorEmpleados = pListCambioPersonal.Count;
                if (tipo == "prestar")
                {
                    foreach (var item in pListCambioPersonal.ToArray())
                    {
                        if (db.CAMBIO_PERSONAL.Any(x => x.Cedula == item.Cedula))
                        {
                            pListCambioPersonal.Remove(item);
                            Bitacora.Remove(Bitacora.Where(x=>x.Cedula==item.Cedula).FirstOrDefault());
                        }
                    }
                    if (ContadorEmpleados > 0 && pListCambioPersonal.Count == 0)
                    {
                        return "Todos los empleados seleccionados ya habian sido movidos";
                    }
                    else
                    {
                        db.CAMBIO_PERSONAL.AddRange(pListCambioPersonal);
                        db.BITACORA_CAMBIO_PERSONAL.AddRange(Bitacora);
                        db.SaveChanges();
                        return "Empleado(s) movido(s) con éxito";
                    }
                    
                }
                else if (tipo == "regresar")
                {
                    foreach (var item in pListCambioPersonal.ToArray())
                    {
                        CambioPersonal = db.CAMBIO_PERSONAL.Where(x => x.Cedula == item.Cedula).FirstOrDefault();
                        CambioPersonal.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                        CambioPersonal.FechaModificacionLog = DateTime.Now;
                        CambioPersonal.TerminalModificacionLog = item.TerminalIngresoLog;
                        CambioPersonal.UsuarioModificacionLog = item.UsuarioIngresoLog;
                        db.SaveChanges();
                    }
                    db.BITACORA_CAMBIO_PERSONAL.AddRange(Bitacora);
                    db.SaveChanges();
                    return "Empleado(s) regresado(s) con éxito";
                }
                return "";
            }
            
        }
    }
}