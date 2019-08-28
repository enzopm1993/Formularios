using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsSolicitudPermiso
    {
        ASIS_PRODEntities entities = null;
    

        public List<SolictudPermisoViewModel> ConsultaSolicitudesPermiso()
        {
            entities = new ASIS_PRODEntities();
            List<SolictudPermisoViewModel> ListaSolicitudesPermiso = new List<SolictudPermisoViewModel>();
            var lista = entities.SOLICITUD_PERMISO.ToList();
            foreach(var x in lista)
            {
                List<JUSTICA_SOLICITUD> ListadoJustificaciones = new List<JUSTICA_SOLICITUD>();
                var detalle = entities.JUSTICA_SOLICITUD.Where(y => y.IdSolicitudPermiso == x.IdSolicitudPermiso).ToList();
                foreach(var d in detalle)
                {
                    ListadoJustificaciones.Add(new JUSTICA_SOLICITUD
                    {
                        IdJustificaSolicitud=d.IdJustificaSolicitud,
                        IdSolicitudPermiso=x.IdSolicitudPermiso,
                        CodigoMotivo=d.CodigoMotivo,
                        FechaSalida=d.FechaSalida,
                        FechaRegreso=d.FechaRegreso,
                        UsuarioIngresoLog=d.UsuarioIngresoLog,
                        FechaIngresoLog=d.FechaIngresoLog,
                        TerminalIngresoLog=d.TerminalIngresoLog,
                        UsuarioModificacionLog=d.UsuarioModificacionLog,
                        TerminalModificacionLog=d.TerminalModificacionLog,
                        FechaModificacionLog=d.FechaModificacionLog
                    });
                }
                ListaSolicitudesPermiso.Add(new SolictudPermisoViewModel
                {
                    IdSolicitudPermiso = x.IdSolicitudPermiso,
                    CodigoLinea = x.CodigoLinea,
                    CodigoArea = x.CodigoArea,
                    CodigoCargo = x.CodigoCargo,
                    Identificacion = x.Identificacion,
                    CodigoMotivo = x.CodigoMotivo,
                    Observacion = x.Observacion,
                    FechaSalida = x.FechaSalida,
                    FechaRegreso = x.FechaRegreso,
                    IdEstadoSolicitud=x.IdEstadoSolicitud??0,
                    FechaBiometrico=x.FechaBiometrico,
                    Origen=char.Parse(x.Origen),
                    CodigoDiagnostico=x.CodigoDiagnostico,
                    FechaIngresoLog=x.FechaIngresoLog,
                    UsuarioIngresoLog=x.UsuarioIngresoLog,
                    TerminalIngresoLog=x.TerminalIngresoLog,
                    UsuarioModificacionLog=x.UsuarioModificacionLog,
                    FechaModificacionLog=x.FechaModificacionLog,
                    TerminalModificacionLog=x.TerminalModificacionLog





                });

            }





            return ListaSolicitudesPermiso;
        }


    }
} 