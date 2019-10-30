using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.AuditoriaSangre;
using Asiservy.Automatizacion.Formularios.Models.Seguridad;
using System;
using System.Collections.Generic;
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
        string[] liststring;
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

                clsDAuditoriaSangre = new clsDAuditoriaSangre();
                ViewBag.AuditoriaSangre = clsDAuditoriaSangre.ConsultarAuditoriaSangreDiaria(DateTime.Now);
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return RedirectToAction("Home", "Home");
            }
        }
        [Authorize]
        public ActionResult ReporteAuditoriaSangrePArtial(string CodLinea, DateTime Fecha)
        {
            try
            {
                clsDAuditoriaSangre = new clsDAuditoriaSangre();
                var ReporteAuditoriaSangre = clsDAuditoriaSangre.ConsultarReporteAuditoriaSangre(CodLinea,Fecha);
                if (!ReporteAuditoriaSangre.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                
                }
                return PartialView(ReporteAuditoriaSangre);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        //public ActionResult ControlAuditoriaSangrePartial()
        //{
        //    return PartialView();
        //}
        public ActionResult EmpleadoBuscar()
        {
            try
            {
                
                clsDEmpleado = new clsDEmpleado();
                List<spConsutaEmpleadosFiltro> lista = clsDEmpleado.ConsultaEmpleadosFiltro("0", "0",clsAtributos.CargoLimpiadora);
                return PartialView(lista);

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult ControlAuditoriaSangrePartial(int? IdAuditoria,string Cedula, string Porcentaje, DateTime Fecha, string estado,int? change)
        {
            try
            {
                clsDAuditoriaSangre = new clsDAuditoriaSangre();
                if (change != 1)
                {
                    int IdAuditoriaS = IdAuditoria == null ? 0 : Convert.ToInt32(IdAuditoria);
                    liststring = User.Identity.Name.Split('_');
                    var hora = TimeSpan.Parse(DateTime.Now.ToString("hh:mm"));
                    //DateTime Fecha;
                    //Fecha = string.IsNullOrEmpty(Fecha) ? DateTime.Now :Convert.ToDateTime(Fecha);
                    DateTime FechaCreacion = DateTime.Now;
                    

                    List<spConsultarAuditoriaSangreDiaria> ListaAuditoria = clsDAuditoriaSangre.GuardarActualizarAuditoriaSangre(new CONTROL_AUDITORIASANGRE
                    {
                        Cedula = Cedula,
                        Porcentaje = Convert.ToDecimal(Porcentaje),
                        FechaCreacionLog = FechaCreacion,
                        EstadoRegistro = estado,
                        TerminalCreacionLog = Request.UserHostAddress,
                        UsuarioCreacionLog = liststring[0],
                        Hora = hora,
                        FechaAuditoria = Fecha,
                        IdControlAuditoriaSangre = IdAuditoriaS
                    });

                    return PartialView(ListaAuditoria);
                }
                else
                {
                    return PartialView(clsDAuditoriaSangre.ConsultarAuditoriaSangreDiaria(Fecha));
                }
                
            }
            catch (Exception ex)
            {

                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
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
                
                ViewBag.Lineas = new SelectList(ListLineas, "codigo", "descripcion");
                return View();
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}