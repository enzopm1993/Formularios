using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.CalibracionPhMetro
{
    public class clsDCalibracionPhMetro
    {
        public object[] GuardarControl(CC_CALIBRACION_PHMETRO poControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscaeControl = db.CC_CALIBRACION_PHMETRO.Where(x => x.Fecha == poControl.Fecha &&
                x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (buscaeControl == null)
                {
                    db.CC_CALIBRACION_PHMETRO.Add(poControl);
                    db.SaveChanges();
                    resultado[0] = "000";
                    resultado[1] = "Registro ingresado con éxito";
                    resultado[2] = poControl;
                }
                else
                {
                    resultado[0] = "002";
                    resultado[1] = "Error, el registro ya existe";
                    resultado[2] = poControl;
                }
                return resultado;
            }
        }
        public CC_CALIBRACION_PHMETRO ConsultarControl(DateTime pdFecha)
        {
            using (var db = new ASIS_PRODEntities())
            {
                return db.CC_CALIBRACION_PHMETRO.Where(x => x.Fecha == pdFecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
            }
        }
        public object[] ActualizarControl(CC_CALIBRACION_PHMETRO poControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarControl = db.CC_CALIBRACION_PHMETRO.Find(poControl.IDPhMetro);
                BuscarControl.CodigoPhMetro = poControl.CodigoPhMetro;
                BuscarControl.Hora = poControl.Hora;
                BuscarControl.observacion = poControl.observacion;
                BuscarControl.ph10 = poControl.ph10;
                BuscarControl.Ph40 = poControl.Ph40;
                BuscarControl.ph70 = poControl.ph70;

                BuscarControl.FechaModificacionLog = poControl.FechaIngresoLog;
                BuscarControl.UsuarioModificacionLog = poControl.UsuarioIngresoLog;
                BuscarControl.TerminalIngresoLog = poControl.TerminalIngresoLog;
                db.SaveChanges();
                resultado[0] = "001";
                resultado[1] = "Registro actualizado con éxito";
                resultado[2] = poControl;
                return resultado;
            }
        }
        public object[] InactivarControl(CC_CALIBRACION_PHMETRO poControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarControl = db.CC_CALIBRACION_PHMETRO.Find(poControl.IDPhMetro);
                BuscarControl.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                BuscarControl.FechaModificacionLog = poControl.FechaIngresoLog;
                BuscarControl.UsuarioModificacionLog = poControl.UsuarioIngresoLog;
                BuscarControl.TerminalModificacionLog = poControl.TerminalIngresoLog;
                db.SaveChanges();
                resultado[0] = "002";
                resultado[1] = "Registro Inactivado con éxito";
                resultado[2] = poControl;
                return resultado;
            }
        }
        public List<CC_CALIBRACION_PHMETRO> ConsultarBandejaclsDCalibracionPhMetro(DateTime? FechaInicio, DateTime? FechaFin, bool EstadoControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                if (EstadoControl == clsAtributos.EstadoReportePendiente)
                {
                    var resultado = (from p in db.CC_CALIBRACION_PHMETRO
                                     where ((p.EstadoControl == EstadoControl || p.EstadoControl == null) && (p.EstadoRegistro == clsAtributos.EstadoRegistroActivo))
                                     select p).ToList();
                    return resultado;
                }
                else
                {
                    var resultado = (from p in db.CC_CALIBRACION_PHMETRO
                                     where (p.Fecha >= FechaInicio && p.Fecha <= FechaFin) && (p.EstadoControl == EstadoControl) && p.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                     select p).ToList();
                    return resultado;
                }
            }
        }
        public string AprobarControl(int IdControl, string usuario, string terminal)
        {
            using (var db = new ASIS_PRODEntities())
            {

                var buscarControl = db.CC_CALIBRACION_PHMETRO.Find(IdControl);
                buscarControl.FechaModificacionLog = DateTime.Now;
                buscarControl.UsuarioModificacionLog = usuario;
                buscarControl.TerminalModificacionLog = terminal;
                buscarControl.UsuarioAprobacion = usuario;
                buscarControl.FechaAprobacion = DateTime.Now;
                buscarControl.EstadoControl = true;
                db.SaveChanges();

                return "El control ha sido aprobado";
            }
        }
        public string ReversarControl(int IdControl, string usuario, string terminal)
        {
            using (var db = new ASIS_PRODEntities())
            {

                var buscarControl = db.CC_CALIBRACION_PHMETRO.Find(IdControl);
                buscarControl.FechaModificacionLog = DateTime.Now;
                buscarControl.UsuarioModificacionLog = usuario;
                buscarControl.TerminalModificacionLog = terminal;
                buscarControl.UsuarioAprobacion = usuario;
                buscarControl.FechaAprobacion = DateTime.Now;
                buscarControl.EstadoControl = false;
                db.SaveChanges();

                return "El control ha sido Reversado";
            }
        }
    }
}