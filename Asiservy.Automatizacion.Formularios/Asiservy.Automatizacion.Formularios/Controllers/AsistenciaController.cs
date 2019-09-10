using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.Models;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class AsistenciaController : Controller
    {

        clsDGeneral clsDGeneral = null;
        clsDError clsDError = null;
        clsDCambioPersonal clsDCambioPersonal = null;

        #region Métodos
        public void ConsultaCombosGeneral()
        {
            clsDGeneral = new clsDGeneral();
            ViewBag.Lineas = clsDGeneral.ConsultaLineas();
            //ViewBag.Areas = clsDGeneral.ConsultaAreas("0");
            //ViewBag.Cargos = clsDGeneral.ConsultaCargos("0");
        }
        #endregion

        // GET: Asistencia
        [Authorize]
        public ActionResult Asistencia()
        {
            return View();
        }

        [Authorize]
        public ActionResult RptAsistencia()
        {
            return View();
        }

        [Authorize]
        public ActionResult EditarAsistencia()
        {
            return View();
        }

        #region Cambio_PersonaldeÁrea
        [Authorize]
        public ActionResult CambiarPersonalDeArea()
        {
            try
            {
                ConsultaCombosGeneral();
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        [Authorize]
        public ActionResult BitacoraCambioPersonal()
        {
            try
            {
                clsDGeneral = new clsDGeneral();
                ViewBag.Lineas = clsDGeneral.ConsultaLineas();
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
        public ActionResult BitacoraCambioPersonalPartial(string dsLinea, string dsArea, string dsCargo, string dsCedula, DateTime ddFechaDesde, DateTime ddFechaHasta)
        {
            try
            {
                clsDCambioPersonal = new clsDCambioPersonal();
               var model= clsDCambioPersonal.ConsultarBitacoraCambioPersonal(new Models.Asistencia.BitacoraCambioPersonalModelView {
                    CodLinea=dsLinea,
                    CodArea=dsArea,
                    CodCargo=dsCargo,
                    Cedula=dsCedula,
                    FechaDesde=ddFechaDesde,
                    FechaHasta = ddFechaHasta
                });
                return PartialView(model);

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

        #endregion


        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult Cuchillo()
        {
            return View();
        }
        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult RptCuchillo()
        {
            return View();
        }

        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult ReporteDistribucion()
        {
            return View();
        }
        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult PersonalNomina()
        {
            return View();
        }
        public ActionResult Empleados()
        {
            
            List<Empleado> Empleados = new List<Empleado>
            {
                new Empleado { Cedula = "0940203406", Nombre = "Juan Maldonado", Area="Procesos", Cargo="Limpiador" },
                new Empleado { Cedula = "1188888456", Nombre = "Pedro Suarez", Area="Procesos", Cargo="Despellejador" },
                new Empleado { Cedula = "2723626161", Nombre = "Alejandro Sánchez", Area="Procesos", Cargo="Limpiador" },
                new Empleado { Cedula = "3635261617", Nombre = "María Perez", Area="Procesos", Cargo="Limpiador" },
                new Empleado { Cedula = "1188888456", Nombre = "Andrea Bejarano", Area="Procesos", Cargo="Despellejador" },
                new Empleado { Cedula = "2345789123", Nombre = "Juan Peña", Area="Procesos", Cargo="Despellejador" },
            };
            return PartialView("Empleados",Empleados);

        }


        #region METODOS GENERICOS
        public JsonResult ConsultaListadoAreas(string CodLinea)
        {
            try
            {
                clsDGeneral = new clsDGeneral();
                var areas = clsDGeneral.ConsultaAreas(CodLinea);
                return Json(areas, JsonRequestBehavior.AllowGet);
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

        public JsonResult ConsultaListadoCargos(string CodArea)
        {
            try
            {
                clsDGeneral = new clsDGeneral();
                var areas = clsDGeneral.ConsultaCargos(CodArea);
                return Json(areas, JsonRequestBehavior.AllowGet);
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
        #endregion

    }
}
