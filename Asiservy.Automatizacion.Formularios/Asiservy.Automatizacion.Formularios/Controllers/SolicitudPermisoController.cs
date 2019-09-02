using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Datos.Datos;
using System.Net;

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
            try
            {
                List<SolicitudPermisoViewModel> ListaSolicitud;
                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                ListaSolicitud = clsDSolicitudPermiso.ConsultaSolicitudesPermiso(clsAtributos.EstadoSolicitudPendiente);
                return View(ListaSolicitud);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return View();
            }
        }
        [Authorize]
        public JsonResult AprobarSolicitud(string[] diIdSolicitud)
        {
            try
            {
                string psRespuesta = string.Empty;
                if (diIdSolicitud!=null && diIdSolicitud.Length > 0)
                {
                    foreach (var psIdSolicitud in diIdSolicitud)
                    {
                        if (!string.IsNullOrEmpty(psIdSolicitud))
                        {                           
                            clsDSolicitudPermiso = new clsDSolicitudPermiso();
                            SOLICITUD_PERMISO model = new SOLICITUD_PERMISO();
                            model.IdSolicitudPermiso = int.Parse(psIdSolicitud);
                            model.EstadoSolicitud = clsAtributos.EstadoSolicitudAprobado;
                            model.FechaModificacionLog = DateTime.Now;
                            string[] psIdUsuario = User.Identity.Name.Split('_');
                            model.UsuarioModificacionLog = psIdUsuario[0];
                            model.TerminalModificacionLog = Request.UserHostAddress;
                            psRespuesta = clsDSolicitudPermiso.CambioEstadoSolicitud(model);
                        }
                    }
                    return Json(psRespuesta, JsonRequestBehavior.AllowGet);
                }
                return Json("Error, no se ha enviado ninguna solicitud", JsonRequestBehavior.AllowGet);
                
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);                
            }
        }
        [Authorize]
        public JsonResult AnularSolicitud(string diIdSolicitud,string dsObservacion)
        {
            try
            {
                if (!string.IsNullOrEmpty(diIdSolicitud))
                {
                    clsDSolicitudPermiso = new clsDSolicitudPermiso();
                    SOLICITUD_PERMISO model = new SOLICITUD_PERMISO();
                    model.IdSolicitudPermiso = int.Parse(diIdSolicitud);
                    model.Observacion = dsObservacion;
                    model.EstadoSolicitud = clsAtributos.EstadoSolicitudAnulado;
                    model.FechaModificacionLog = DateTime.Now;
                    string[] psIdUsuario = User.Identity.Name.Split('_');
                    model.UsuarioModificacionLog = psIdUsuario[0]+"";
                    model.TerminalModificacionLog = Request.UserHostAddress;
                    string psRespuesta = clsDSolicitudPermiso.CambioEstadoSolicitud(model);
                    return Json(psRespuesta, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Error, Numero de solicitud invalida", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        public JsonResult FinalizarSolicitud(string[] diIdSolicitud)
        {
            try
            {
                string psRespuesta = string.Empty;
                if (diIdSolicitud != null && diIdSolicitud.Length > 0)
                {
                    foreach (var psIdSolicitud in diIdSolicitud)
                    {
                        if (!string.IsNullOrEmpty(psIdSolicitud))
                        {                            
                            clsDSolicitudPermiso = new clsDSolicitudPermiso();
                            SOLICITUD_PERMISO model = new SOLICITUD_PERMISO();
                            model.IdSolicitudPermiso = int.Parse(psIdSolicitud);
                            model.EstadoSolicitud = clsAtributos.EstadoSolicitudRevisado;
                            model.FechaModificacionLog = DateTime.Now;
                            string[] psIdUsuario = User.Identity.Name.Split('_');
                            model.UsuarioModificacionLog = psIdUsuario[0];
                            model.TerminalModificacionLog = Request.UserHostAddress;
                            psRespuesta = clsDSolicitudPermiso.CambioEstadoSolicitud(model);
                        }
                    }
                    return Json(psRespuesta, JsonRequestBehavior.AllowGet);
                }
                return Json("Error, no se ha enviado ninguna solicitud", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        public ActionResult SolicitudPermisoEdit(string dsSolicitud)
        {
            try
            {                
                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                SolicitudPermisoViewModel model = clsDSolicitudPermiso.ConsultaSolicitudPermiso(dsSolicitud);
                if(model.Origen==clsAtributos.SolicitudOrigenGeneral)
                {
                    ConsultaCombosGeneral();
                }
                else
                {
                    ConsultaCombosMedicos();

                }

                return PartialView(model);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return PartialView();
            }
        }
        [Authorize]
        public ActionResult ConsultaSolicitudes()
        {
            clsDSolicitudPermiso poSolicitudPermiso = new clsDSolicitudPermiso();
            var pListSolicitudPermiso =poSolicitudPermiso.ConsultaSolicitudesPermisoReporte(clsAtributos.EstadoSolicitudTodos);
            return PartialView(pListSolicitudPermiso);
        }
        [Authorize]
        [HttpPost]
        public ActionResult SolicitudPermisoEdit(SolicitudPermisoViewModel doSolicitud, string frm)
        {
            try {
                if (doSolicitud != null && doSolicitud.IdSolicitudPermiso > 0)
                {
                    clsDSolicitudPermiso = new clsDSolicitudPermiso();
                    SOLICITUD_PERMISO poSolicitudPermiso = new SOLICITUD_PERMISO();
                    poSolicitudPermiso.IdSolicitudPermiso = doSolicitud.IdSolicitudPermiso;
                    poSolicitudPermiso.Observacion = doSolicitud.Observacion;
                    poSolicitudPermiso.CodigoMotivo = doSolicitud.CodigoMotivo;
                    poSolicitudPermiso.FechaSalida = doSolicitud.FechaSalida ?? poSolicitudPermiso.FechaSalida;
                    poSolicitudPermiso.FechaRegreso = doSolicitud.FechaRegreso ?? poSolicitudPermiso.FechaRegreso;
                    poSolicitudPermiso.FechaModificacionLog = DateTime.Now;
                    string[] psIdUsuario = User.Identity.Name.Split('_');
                    poSolicitudPermiso.UsuarioModificacionLog = psIdUsuario[0] + "";
                    poSolicitudPermiso.TerminalModificacionLog = Request.UserHostAddress;
                    foreach(var detalle in doSolicitud.JustificaSolicitudes)
                    {
                        if(detalle.CodigoMotivo!=null)
                        {
                            poSolicitudPermiso.JUSTICA_SOLICITUD.Add(detalle);
                        }
                    }
                    string Respuesta = clsDSolicitudPermiso.GuargarModificarSolicitud(poSolicitudPermiso);
                    SetSuccessMessage(Respuesta);
                }
                else
                {
                    SetErrorMessage("Solicitud Invalida");
                }
                if (!string.IsNullOrEmpty(frm) && frm == "BandejaRRHH")
                    return RedirectToAction("BandejaRRHH");
                else
                    return RedirectToAction("BandejaAprobacion");
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                if (!string.IsNullOrEmpty(frm) && frm == "BandejaRRHH")
                    return RedirectToAction("BandejaRRHH");
                else
                    return RedirectToAction("BandejaAprobacion");
            }

        }


        [Authorize]
        public ActionResult SolicitudPermiso()
        {
            try
            {
                ConsultaCombosGeneral();
                return View();
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return View();
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult SolicitudPermiso( SolicitudPermisoViewModel model)
        {
            try
            {
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();
                if ((model.FechaSalida == null || model.FechaRegreso == null) && model.FechaSalidaEntrada == null)
                {
                    ConsultaCombosMedicos();
                    ModelState.AddModelError("CustomError", "Debe Ingresar un rango de horas o fechas");
                    return View(model);
                }
                else if (model.FechaSalidaEntrada != null && (model.HoraRegreso == null || model.HoraSalida == null))
                {
                    ModelState.AddModelError("CustomError", "Debe Ingresar un rango de horas correcto");

                    ConsultaCombosMedicos();
                    return View(model);
                }
                else if (model.FechaSalida != null && model.FechaRegreso != null && model.FechaSalida > model.FechaRegreso)
                {
                    ConsultaCombosMedicos();
                    ModelState.AddModelError("CustomError", "Fecha de salida no puede ser mayor a la de regreso");
                    return View(model);
                }
                else if (model.HoraRegreso != null && model.HoraSalida != null && model.HoraSalida.Value.Hour > model.HoraRegreso.Value.Hour)
                {
                    ConsultaCombosMedicos();
                    ModelState.AddModelError("CustomError", "Hora de salida no puede ser mayor a la de regreso");
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
                    solicitudPermiso.Origen = clsAtributos.SolicitudOrigenGeneral;
                    solicitudPermiso.CodigoDiagnostico = "";
                    solicitudPermiso.CodigoClasificador = 0;
                    solicitudPermiso.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    string[] psIdUsuario = User.Identity.Name.Split('_');
                    solicitudPermiso.Nivel = clsDSolicitudPermiso.ConsultarNivelUsuario(psIdUsuario[1] + "");
                    solicitudPermiso.UsuarioIngresoLog = psIdUsuario[0];
                    solicitudPermiso.FechaIngresoLog = DateTime.Now;
                    solicitudPermiso.TerminalIngresoLog = Request.UserHostAddress;
                    string psRespuesta = clsDSolicitudPermiso.GuargarModificarSolicitud(solicitudPermiso);
                    SetSuccessMessage(string.Format(psRespuesta));
                }
                else
                {
                    ConsultaCombosMedicos();
                    return View(model);
                }

                return RedirectToAction("SolicitudPermiso");
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return RedirectToAction("SolicitudPermiso");
            }
        }
      


        [Authorize]
        public ActionResult SolicitudPermisoDispensario()
        {
            try
            {
                ConsultaCombosMedicos();
                return View();
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return View();
            }
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
                if (model.CodigoClasificador == null)
                {
                    //ConsultaCombosMedicos();
                    ModelState.AddModelError("CustomError2", "Debe Ingresar un Clasificador");
                    //return View(model);
                }
                if (model.CodigoDiagnostico == null)
                {
                    //ConsultaCombosMedicos();
                    ModelState.AddModelError("CustomError3", "Debe Ingresar un Diagnóstico");
                    //return View(model);
                }
                if ((model.FechaSalida == null || model.FechaRegreso == null) && model.FechaSalidaEntrada == null)
                {
                    //ConsultaCombosMedicos();
                    ModelState.AddModelError("CustomError", "Debe Ingresar un rango de horas o fechas");
                   // return View(model);
                }
                else if (model.FechaSalidaEntrada != null && (model.HoraRegreso == null || model.HoraSalida == null))
                {           
                    ModelState.AddModelError("CustomError", "Debe Ingresar un rango de horas correcto");                 
                    
                    //ConsultaCombosMedicos();
                    //return View(model);
                }
                else if(model.FechaSalida != null && model.FechaRegreso!= null && model.FechaSalida > model.FechaRegreso)
                {
                    //ConsultaCombosMedicos();
                    ModelState.AddModelError("CustomError", "Fecha de salida no puede ser mayor a la de regreso");
                    //return View(model);
                }  else if (model.HoraRegreso!=null && model.HoraSalida != null && model.HoraSalida.Value.Hour>model.HoraRegreso.Value.Hour)
                {
                    //ConsultaCombosMedicos();
                    ModelState.AddModelError("CustomError", "Hora de salida no puede ser mayor a la de regreso");
                    //return View(model);
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
                    solicitudPermiso.EstadoSolicitud = clsAtributos.EstadoSolicitudAprobado;
                    solicitudPermiso.FechaBiometrico = DateTime.Now;
                    solicitudPermiso.Origen = clsAtributos.SolicitudOrigenMedico;
                    solicitudPermiso.CodigoDiagnostico = model.CodigoDiagnostico;
                    solicitudPermiso.CodigoClasificador = int.Parse(model.CodigoClasificador);
                    solicitudPermiso.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    string[] psIdUsuario = User.Identity.Name.Split('_');
                    solicitudPermiso.Nivel = clsDSolicitudPermiso.ConsultarNivelUsuario(psIdUsuario[1]+"");

                    solicitudPermiso.FechaIngresoLog = DateTime.Now;
                    solicitudPermiso.UsuarioIngresoLog = psIdUsuario[0]+"";
                    solicitudPermiso.TerminalIngresoLog = Request.UserHostAddress;
                    string psRespuesta = clsDSolicitudPermiso.GuargarModificarSolicitud(solicitudPermiso);
                    SetSuccessMessage(string.Format(psRespuesta));
                }
                else
                {
                    ConsultaCombosMedicos();
                    return View(model);
                }

                return RedirectToAction("SolicitudPermisoDispensario");
            }catch(Exception ex)
            {
                SetErrorMessage(ex.Message);
                return RedirectToAction("SolicitudPermisoDispensario");
            }
        }
        public void ConsultaCombosGeneral()
        {
            clsDClasificador = new clsDClasificador();
            clsDSolicitudPermiso = new clsDSolicitudPermiso();
            clsDGeneral = new clsDGeneral();
            ViewBag.MotivosPermiso = clsDSolicitudPermiso.ConsultarMotivos("G");
            ViewBag.Lineas = clsDGeneral.ConsultaLineas();
            ViewBag.Areas = clsDGeneral.ConsultaAreas();
            ViewBag.Cargos = clsDGeneral.ConsultaCargos();
        }
        public void ConsultaCombosMedicos()
        {
            clsDClasificador = new clsDClasificador();
            clsDSolicitudPermiso = new clsDSolicitudPermiso();
            clsDGeneral = new clsDGeneral();
            ViewBag.ClasificaroMedico = clsDClasificador.ConsultarClasificador("001", 0);
            ViewBag.MotivosPermiso = clsDSolicitudPermiso.ConsultarMotivos("M");
            ViewBag.Lineas = clsDGeneral.ConsultaLineas();
            ViewBag.Areas = clsDGeneral.ConsultaAreas();
            ViewBag.CodigosEnfermedad = clsDGeneral.ConsultaCodigosEnfermedad();
            ViewBag.Cargos = clsDGeneral.ConsultaCargos();
        }
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
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

      


        [Authorize]
        public ActionResult BandejaRRHH()
        {
            try
            {
                List<SolicitudPermisoViewModel> ListaSolicitud;
                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                ListaSolicitud = clsDSolicitudPermiso.ConsultaSolicitudesPermiso(clsAtributos.EstadoSolicitudAprobado);
                return View(ListaSolicitud);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return View();
            }
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
