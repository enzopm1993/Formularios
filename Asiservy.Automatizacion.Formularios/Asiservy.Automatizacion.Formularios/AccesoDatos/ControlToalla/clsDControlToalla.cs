using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.ControlToalla
{
    public class clsDControlToalla
    {
        public List<CONTROL_TOALLA> ConsultarCabToalla(DateTime Fecha, string Linea, string Turno)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.CONTROL_TOALLA.Where(x => x.Fecha==Fecha&&x.Linea==Linea&&x.Turno==Turno&&x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).ToList();
            }
        }
        public string GuardarControlToallaCab(int? Id,string Turno, DateTime? Fecha, TimeSpan? Hora, string Linea, string Observacion, string psterminal, string psusuario, string estadoRegistro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (estadoRegistro == clsAtributos.EstadoRegistroActivo)
                {
                    var buscarregistroToalla = db.CONTROL_TOALLA.Where(x => x.Fecha == Fecha && x.Turno == Turno && x.Hora == Hora).FirstOrDefault();
                    if (buscarregistroToalla == null)
                    {
                        db.CONTROL_TOALLA.Add(new CONTROL_TOALLA
                        {
                            FechaCreacionLog = DateTime.Now,
                            Fecha = Fecha,
                            EstadoRegistro = estadoRegistro,
                            Hora = Hora,
                            Linea = Linea,
                            Observacion = Observacion,
                            TerminalCreacionLog = psterminal,
                            Turno = Turno,
                            UsuarioCreacionLog = psusuario
                        });
                        db.SaveChanges();
                        return "Registro guardado con éxito";
                    }
                    else
                    {
                        return "999"; //ya existe el registro
                    }
                    
                }
                else//significa que se va a desactivar el registro
                {
                    var buscarControlToallaCab= db.CONTROL_TOALLA.Find(Id);
                    buscarControlToallaCab.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    buscarControlToallaCab.FechaModificacionLog = DateTime.Now;
                    buscarControlToallaCab.UsuarioModificacionLog = psusuario;
                    buscarControlToallaCab.TerminalModificacionLog = psterminal;
                    return "Registro Inactivado con éxito";
                }
                
            }
        }
    }
}