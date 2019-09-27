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
        // GET: AuditoriaSangre
        [Authorize]
        public ActionResult ControlAuditoriaSangre()
        {
            try
            {
                clsDAuditoriaSangre = new clsDAuditoriaSangre();
                ViewBag.AuditoriaSangre = clsDAuditoriaSangre.ConsultarAuditoriaSangreDiaria();
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
        public ActionResult ControlAuditoriaSangrePartial(string Cedula, string Porcentaje, string fecha,string estado)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                var hora =TimeSpan.Parse(DateTime.Now.ToString("hh:mm"));
                DateTime Fecha;
                Fecha = string.IsNullOrEmpty(fecha) ? DateTime.Now :Convert.ToDateTime(fecha);
                clsDAuditoriaSangre = new clsDAuditoriaSangre();
                
                List<spConsultarAuditoriaSangreDiaria> ListaAuditoria = clsDAuditoriaSangre.GuardarActualizarAuditoriaSangre(new CONTROL_AUDITORIASANGRE { Cedula=Cedula,Porcentaje=Convert.ToInt32(Porcentaje),
                FechaCreacionLog=Fecha,EstadoRegistro=estado,TerminalCreacionLog= Request.UserHostAddress, UsuarioCreacionLog= liststring[0],
                    Hora = hora
                });

                return PartialView(ListaAuditoria);
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