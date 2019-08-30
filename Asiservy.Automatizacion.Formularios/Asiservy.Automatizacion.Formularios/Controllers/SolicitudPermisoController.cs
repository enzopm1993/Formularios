using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.Controllers
{

    public class SolicitudPermisoController : Controller
    {
        clsDClasificador clsDClasificador = null; 
        clsDSolicitudPermiso clsDSolicitudPermiso = null;
        clsDGeneral clsDGeneral = null;

        [Authorize]
        // GET: SolicitudPermiso
        public ActionResult BandejaAprobacion()
        {
            List<Solicitud> solicitud = new List<Solicitud>();

            solicitud.Add(new Solicitud { codigo = "1", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Santiago Jose" });
            solicitud.Add(new Solicitud { codigo = "2", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Morales Victor" });
            solicitud.Add(new Solicitud { codigo = "3", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Cazares Julio" });
            solicitud.Add(new Solicitud { codigo = "4", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Vera Jose" });
            solicitud.Add(new Solicitud { codigo = "5", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Chavez Jorge" });
            solicitud.Add(new Solicitud {codigo="6",fecha="16/08/2019" ,Motivo="Vacaciones",Area="Proceso",Empleado="Santiago Emilio" });


            return View(solicitud);
        }
        [Authorize]
        public ActionResult SolicitudPermiso()
        { 
            return View();
        }

        public void ConsultaCombos()
        {
            clsDClasificador = new clsDClasificador();
            clsDSolicitudPermiso = new clsDSolicitudPermiso();
            clsDGeneral = new clsDGeneral();
            ViewBag.ClasificaroMedico = clsDClasificador.ConsultarClasificador("001", 0);
            ViewBag.MotivosPermiso = clsDSolicitudPermiso.ConsultarMotivos("1");
            ViewBag.Lineas = clsDGeneral.ConsultaLineas();
            ViewBag.Areas = clsDGeneral.ConsultaAreas();
            ViewBag.CodigosEnfermedad = clsDGeneral.ConsultaCodigosEnfermedad();
            ViewBag.Cargos = clsDGeneral.ConsultaCargos();
        }


        [Authorize]
        public ActionResult SolicitudPermisoDispensario()
        {           
            ConsultaCombos();
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult SolicitudPermisoDispensario(SolicitudPermisoViewModel model)
        {
            try
            {
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();
                if ((model.FechaSalida == null || model.FechaRegreso == null) && model.FechaSalidaEntrada == null)
                {
                    ConsultaCombos();
                    ModelState.AddModelError("CustomError", "Debe Ingresar un rango de horas o fechas");
                    return View(model);
                }
                else if (model.FechaSalidaEntrada != null && (model.HoraRegreso == null || model.HoraSalida == null))
                {
                    ConsultaCombos();
                    ModelState.AddModelError("CustomError", "Debe Ingresar un rango de horas");
                    return View(model);
                }

                if (ModelState.IsValid)
                {
                    SOLICITUD_PERMISO solicitudPermiso = new SOLICITUD_PERMISO();
                    clsDSolicitudPermiso = new clsDSolicitudPermiso();
                    solicitudPermiso.CodigoLinea = model.CodigoLinea;
                    solicitudPermiso.CodigoArea = model.CodigoArea;
                    solicitudPermiso.CodigoCargo = model.CodigoCargo;
                    solicitudPermiso.Identificacion = model.Identificacion;
                    solicitudPermiso.CodigoMotivo = model.CodigoMotivo;
                    solicitudPermiso.Observacion = model.Observacion;

                    if (model.FechaSalidaEntrada == null)
                    {
                        solicitudPermiso.FechaSalida = model.FechaSalida ?? DateTime.MinValue;
                        solicitudPermiso.FechaRegreso = model.FechaRegreso ?? DateTime.MinValue;
                    }
                    else
                    {
                        solicitudPermiso.FechaSalida = new DateTime(
                            model.FechaSalidaEntrada.Value.Year
                            , model.FechaSalidaEntrada.Value.Month
                            , model.FechaSalidaEntrada.Value.Day
                            , model.HoraSalida.Value.Hour
                            , model.HoraSalida.Value.Minute
                            , model.HoraSalida.Value.Second
                            );

                        solicitudPermiso.FechaRegreso = new DateTime(
                            model.FechaSalidaEntrada.Value.Year
                            , model.FechaSalidaEntrada.Value.Month
                            , model.FechaSalidaEntrada.Value.Day
                            , model.HoraRegreso.Value.Hour
                            , model.HoraRegreso.Value.Minute
                            , model.HoraRegreso.Value.Second
                            );
                    }
                    solicitudPermiso.EstadoSolicitud = clsAtributos.EstadoSolicitudPendiente;
                    solicitudPermiso.FechaBiometrico = DateTime.Now;
                    solicitudPermiso.Origen = clsAtributos.SolicitudOrigenMedico;
                    solicitudPermiso.CodigoDiagnostico = model.CodigoDiagnostico;
                    solicitudPermiso.CodigoClasificador = int.Parse(model.CodigoClasificador);
                    solicitudPermiso.EstadoRegistro = clsAtributos.EstadoRegistroActivo;

                    solicitudPermiso.FechaIngresoLog = DateTime.Now;
                    solicitudPermiso.UsuarioIngresoLog = "Prueba VC";
                    solicitudPermiso.TerminalIngresoLog = Request.UserHostAddress;
                    string psRespuesta = clsDSolicitudPermiso.GuargarModificarSolicitud(solicitudPermiso);
                    SetSuccessMessage(string.Format(psRespuesta));
                }
                else
                {
                    ConsultaCombos();
                    return View(model);
                }

                return RedirectToAction("SolicitudPermisoDispensario");
            }catch(Exception ex)
            {
                SetSuccessMessage(ex.Message);
                return RedirectToAction("SolicitudPermisoDispensario");
            }
        }
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        [Authorize]
        public ActionResult ReporteSolicitud()
        {
            return View();
        }

        [Authorize]
        public ActionResult BitacoraSolicitud()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BandejaProduccion(string buscar, string SelectPermiso)
        {
            List<Solicitud> solicitud = new List<Solicitud>();

            solicitud.Add(new Solicitud { codigo = "1", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Santiago Jose" });
            solicitud.Add(new Solicitud { codigo = "2", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Morales Victor" });
            solicitud.Add(new Solicitud { codigo = "3", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Cazares Julio" });
            solicitud.Add(new Solicitud { codigo = "4", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Vera Jose" });
            solicitud.Add(new Solicitud { codigo = "5", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Chavez Jorge" });
            solicitud.Add(new Solicitud { codigo = "6", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Santiago Emilio" });
            if(SelectPermiso=="1")
                return View(solicitud.Where(x=>x.codigo.Contains(buscar)).ToList());
            if(SelectPermiso=="2")
                return View(solicitud.Where(x => x.Area.Contains(buscar)).ToList());
            if (SelectPermiso == "3")
                return View(solicitud.Where(x => x.Empleado.Contains(buscar)).ToList());

            return View(solicitud.ToList());


        }


        [Authorize]
        public ActionResult BandejaRRHH()
        {
            List<Solicitud> solicitud = new List<Solicitud>();

            solicitud.Add(new Solicitud { codigo = "1", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Santiago Jose" });
            solicitud.Add(new Solicitud { codigo = "2", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Morales Victor" });
            solicitud.Add(new Solicitud { codigo = "3", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Cazares Julio" });
            solicitud.Add(new Solicitud { codigo = "4", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Vera Jose" });
            solicitud.Add(new Solicitud { codigo = "5", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Chavez Jorge" });
            solicitud.Add(new Solicitud { codigo = "6", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Santiago Emilio" });


            return View(solicitud);
        }

        [Authorize]
        [HttpPost]
        public ActionResult BandejaRRHH(string buscar, string SelectPermiso)
        {
            List<Solicitud> solicitud = new List<Solicitud>();

            solicitud.Add(new Solicitud { codigo = "1", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Santiago Jose" });
            solicitud.Add(new Solicitud { codigo = "2", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Morales Victor" });
            solicitud.Add(new Solicitud { codigo = "3", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Cazares Julio" });
            solicitud.Add(new Solicitud { codigo = "4", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Vera Jose" });
            solicitud.Add(new Solicitud { codigo = "5", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Chavez Jorge" });
            solicitud.Add(new Solicitud { codigo = "6", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Santiago Emilio" });
            if (SelectPermiso == "1")
                return View(solicitud.Where(x => x.codigo.Contains(buscar)).ToList());
            if (SelectPermiso == "2")
                return View(solicitud.Where(x => x.Area.Contains(buscar)).ToList());
            if (SelectPermiso == "3")
                return View(solicitud.Where(x => x.Empleado.Contains(buscar)).ToList());

            return View(solicitud.ToList());
        }
        
    }
}
