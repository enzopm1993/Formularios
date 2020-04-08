using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.AuditoriaSangre;
using Asiservy.Automatizacion.Formularios.Models.Seguridad;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class AuditoriaSangreController : Controller
    {
        clsDEmpleado clsDEmpleado = null;
        clsDError clsDError = null;
        clsDClasificador clsDClasificador = null;
        string[] lsUsuario;
        clsDAuditoriaSangre clsDAuditoriaSangre = null;
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
        // GET: AuditoriaSangre
        [Authorize]
        public ActionResult ControlAuditoriaSangre()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDClasificador = new clsDClasificador();
                clsDAuditoriaSangre = new clsDAuditoriaSangre();
                ViewBag.TipoAuditoria = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoAuditoria, "0");
                ViewBag.Lineas = clsDClasificador.ConsultarClasificador(clsAtributos.CodGrupoLineaProduccion, "0");
                ViewBag.AuditoriaSangre = clsDAuditoriaSangre.ConsultarAuditoriaSangreDiaria(DateTime.Now,clsAtributos.TurnoUno);
                
                return View();
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home","Home");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }
        }
        [Authorize]
        public ActionResult ReporteAuditoriaSangrePArtial(string CodLinea, DateTime Fecha, string Tipo, string Turno)
        {
            try
            {
                clsDAuditoriaSangre = new clsDAuditoriaSangre();
                var ReporteAuditoriaSangre = clsDAuditoriaSangre.ConsultarReporteAuditoriaSangre(CodLinea,Fecha, Tipo,Turno);
                if (!ReporteAuditoriaSangre.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);                
                }
                return PartialView(ReporteAuditoriaSangre);
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }
     
        public ActionResult EmpleadoBuscar(DateTime Fecha, TimeSpan Hora,string dsLinea)
        {
            try
            {                
                clsDEmpleado = new clsDEmpleado();
                List<spConsultaEmpleadosPersonalPrestadoFiltro> lista = clsDEmpleado.ConsultaEmpleadosCambioPersonalFiltro(Fecha,Hora,dsLinea, "0","0");
                return PartialView(lista);

            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }

       
        public ActionResult ControlAuditoriaSangrePartial( DateTime Fecha, string Turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDAuditoriaSangre = new clsDAuditoriaSangre();       
                return PartialView(clsDAuditoriaSangre.ConsultarAuditoriaSangreDiaria(Fecha,Turno));  
            }

            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }

       
        [HttpPost]
        public ActionResult ControlAuditoriaSangrePartial(int? IdAuditoria,string Cedula, string Porcentaje, DateTime Fecha,TimeSpan Hora, string estado,string TipoAuditoria,string Observacion, string Linea,string Turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if(string.IsNullOrEmpty(Cedula) || string.IsNullOrEmpty(Porcentaje) || string.IsNullOrEmpty(estado) || string.IsNullOrEmpty(TipoAuditoria) || string.IsNullOrEmpty(Linea))
                {
                    return Json("0", JsonRequestBehavior.AllowGet);

                }

                clsDAuditoriaSangre = new clsDAuditoriaSangre();
                clsDEmpleado = new clsDEmpleado();
             
                    int IdAuditoriaS = IdAuditoria == null ? 0 : Convert.ToInt32(IdAuditoria);
                    lsUsuario = User.Identity.Name.Split('_');
                    DateTime FechaCreacion = DateTime.Now;
                    //var Linea = clsDEmpleado.ConsultaEmpleado(Cedula).FirstOrDefault();

                    clsDAuditoriaSangre.GuardarActualizarAuditoriaSangre(new CONTROL_AUDITORIASANGRE
                    {
                        Cedula = Cedula,
                        Porcentaje = Convert.ToDecimal(Porcentaje),
                        FechaCreacionLog = FechaCreacion,
                        EstadoRegistro = estado,
                        TerminalCreacionLog = Request.UserHostAddress,
                        UsuarioCreacionLog = lsUsuario[0],
                        Hora = Hora,
                        FechaAuditoria = Fecha,
                        IdControlAuditoriaSangre = IdAuditoriaS,
                        Linea= Linea,
                        TipoAuditoria= TipoAuditoria,
                        Observacion= Observacion,
                        Turno=Turno
                    });

                   return Json("Registro Éxitoso", JsonRequestBehavior.AllowGet);



            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EliminarControlAuditoriaSangrePartial(int IdAuditoria,string estado,string Observacion)        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(estado) )
                {
                    return Json("0", JsonRequestBehavior.AllowGet);

                }
                clsDAuditoriaSangre = new clsDAuditoriaSangre();
                clsDEmpleado = new clsDEmpleado();              
                lsUsuario = User.Identity.Name.Split('_');
                DateTime FechaCreacion = DateTime.Now;               

                clsDAuditoriaSangre.EliminarAuditoriaSangre(new CONTROL_AUDITORIASANGRE
                {
                  
                    FechaCreacionLog = FechaCreacion,
                    EstadoRegistro = estado,
                    TerminalCreacionLog = Request.UserHostAddress,
                    UsuarioCreacionLog = lsUsuario[0],                   
                    IdControlAuditoriaSangre = IdAuditoria,                   
                    Observacion = Observacion
                });

                return Json("Registro Éxitoso", JsonRequestBehavior.AllowGet);



            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult ReporteAuditoriaSangre()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                clsDClasificador = new clsDClasificador();
                var ListLineas= clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });

                ViewBag.TipoAuditoria = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoAuditoria, "0");
                ViewBag.Lineas = new SelectList(ListLineas, "codigo", "descripcion");
                return View();
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }
        }
    }
}