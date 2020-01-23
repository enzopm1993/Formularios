using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.ControlToalla
{
    public class clsDControlToalla
    {
        clsDAsistencia clsDAsistencia = null;
        public List<CONTROL_TOALLA> ConsultarCabToalla(DateTime Fecha, string Linea, string Turno)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.CONTROL_TOALLA.Where(x => x.Fecha==Fecha&&x.Linea==Linea&&x.Turno==Turno&&x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).ToList();
            }
        }
        public List<spConsultaDetalleToalla> ConsultarDetToalla(int IdCabToalla)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaDetalleToalla(IdCabToalla).ToList();
            }
        }
        public string GuardarControlToallaCab(int? Id,string Turno, DateTime? Fecha, TimeSpan? Hora, string Linea, string Observacion, string psterminal, string psusuario, string estadoRegistro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (estadoRegistro == clsAtributos.EstadoRegistroActivo && Id==null)
                {
                    var buscarregistroToalla = db.CONTROL_TOALLA.Where(x => x.Fecha == Fecha && x.Turno == Turno&&x.Linea==Linea && x.Hora == Hora &&x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                    if (buscarregistroToalla == null)
                    {
                        List<DETALLE_CONTROL_TOALLA> ListEmpleado = new List<DETALLE_CONTROL_TOALLA>();
                        List<spConsultaMovimientoPersonalDiario> Empleados =new List<spConsultaMovimientoPersonalDiario>();
                        clsDAsistencia = new clsDAsistencia();
                        if (Linea == "52") //linea 52=pouch
                        {
                            Empleados = clsDAsistencia.ConsultaMovimientoPersonalDiario(Fecha.Value, Hora.Value, Linea).Where(
                            x => new string[7] { "133", "143", "258", "138", "126", "119", "147" }.Contains(x.CodCargo)).ToList();
                        }
                        else
                        {
                            Empleados = clsDAsistencia.ConsultaMovimientoPersonalDiario(Fecha.Value, Hora.Value, Linea).ToList();
                        }

                        //var Empleados = clsDAsistencia.ConsultaMovimientoPersonalDiario(Convert.ToDateTime("2020-01-10"), TimeSpan.Parse("07:00"), "05").ToList();

                        foreach (var item in Empleados)
                        {
                            ListEmpleado.Add(new DETALLE_CONTROL_TOALLA { Cedula = item.Cedula, UsuarioCreacionLog = psusuario, TerminalCreacionLog = psterminal, FechaCreacionLog = DateTime.Now, EstadoRegistro=clsAtributos.EstadoRegistroActivo });
                        }
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
                            UsuarioCreacionLog = psusuario,
                            DETALLE_CONTROL_TOALLA=ListEmpleado
                        });
                        db.SaveChanges();
                        return "Registro guardado con éxito";
                    }
                    else
                    {
                        return "999"; //ya existe el registro
                    }
                    
                }
                if(estadoRegistro==clsAtributos.EstadoRegistroInactivo &&Id!=null)//significa que se va a desactivar el registro
                {
                    var buscarControlToallaCab= db.CONTROL_TOALLA.Find(Id);
                    buscarControlToallaCab.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    buscarControlToallaCab.FechaModificacionLog = DateTime.Now;
                    buscarControlToallaCab.UsuarioModificacionLog = psusuario;
                    buscarControlToallaCab.TerminalModificacionLog = psterminal;
                    db.SaveChanges();
                    return "Registro Inactivado con éxito";
                }
                else //si se va a actualizar una cabecera de toalla
                {
                    var buscarCabToalla = db.CONTROL_TOALLA.Where(z => z.Fecha == Fecha && z.Hora == Hora && z.Turno == Turno&&z.EstadoRegistro==clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                    if (buscarCabToalla == null )
                    {
                        var RegistroCabToallaAModificar = db.CONTROL_TOALLA.Find(Id);
                        RegistroCabToallaAModificar.Turno = Turno;
                        RegistroCabToallaAModificar.Fecha = Fecha;
                        RegistroCabToallaAModificar.Hora = Hora;
                        RegistroCabToallaAModificar.Observacion = Observacion;
                        db.SaveChanges();
                        return "registro actualizado con éxito";
                    }
                    else if(buscarCabToalla.Observacion!=Observacion)//si quiere actualizar solo la observacion
                    {
                        var RegistroCabToallaAModificar = db.CONTROL_TOALLA.Find(Id);
                        RegistroCabToallaAModificar.Observacion = Observacion;
                        db.SaveChanges();
                        return "registro actualizado con éxito";
                    }
                    else
                    {
                        return "555";//ya existe un registro en esa fecha, hora y turno
                    }
                    
                }
                
                
            }
        }
        public string GuardarControlToallaDet(int IdDetalle, int? NumeroToalla, string psterminal, string psusuario)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var buscar_detalletoalla = db.DETALLE_CONTROL_TOALLA.Find(IdDetalle);
                buscar_detalletoalla.NumeroToallas = NumeroToalla;
                buscar_detalletoalla.TerminalModificacionLog = psterminal;
                buscar_detalletoalla.UsuarioModificacionLog = psusuario;
                buscar_detalletoalla.FechaModificacionLog = DateTime.Now;
                db.SaveChanges();
                return "registros actualizado con éxito";

            }
        }
        public string InactivarCOntrolToalla(int IdCabToalla, string psterminal, string psusuario)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var buscar_CabToalla = db.CONTROL_TOALLA.Find(IdCabToalla);
                buscar_CabToalla.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                var buscarDetControlToalla = db.DETALLE_CONTROL_TOALLA.Where(x => x.IdControlToalla == IdCabToalla).ToList();
                foreach (var item in buscarDetControlToalla)
                {
                    item.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                }
                db.SaveChanges();
                return "registros actualizado con éxito";

            }
        }

    }
}