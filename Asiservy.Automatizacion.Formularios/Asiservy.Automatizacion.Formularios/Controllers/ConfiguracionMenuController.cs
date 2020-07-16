using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class ConfiguracionMenuController : Controller
    {

        private string[] lsUsuario { get; set; } = null;
        private clsDError clsDError { get; set; } = null;
        private ClsdConfiguracionMenu ClsdConfiguracionMenu { get; set; } = null;
       

        // GET: ConfiguracionMenu
        [Authorize]
        public ActionResult FondoLogin()
        {
            try
            {
                ViewBag.JavaScrip =  RouteData.Values["controller"] + "/" + RouteData.Values["action"];
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


        public ActionResult FondoLoginPartial()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsdConfiguracionMenu = new ClsdConfiguracionMenu();
                var model = ClsdConfiguracionMenu.ConsultaLoginFondos();
                if (!model.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                return PartialView(model);
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
        public ActionResult FondoLogin(FONDO_LOGIN control, HttpPostedFileBase dataImg)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsdConfiguracionMenu = new ClsdConfiguracionMenu();
                string path = string.Empty;
                string NombreImg = string.Empty;
                if (dataImg != null)
                {
                    path = Server.MapPath(clsAtributos.UrlImagen + "FondoLogin/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var date = DateTime.Now;
                    long n = long.Parse(date.ToString("yyyyMMddHHmmss"));
                    var ext2 = dataImg.FileName.Split('.');
                    var cont = ext2.Length;
                    NombreImg = "FondoLogin/FondoLogin" + n.ToString() + "." + ext2[cont - 1];
                    control.Imagen = NombreImg;
                }


                control.UsuarioIngresoLog = lsUsuario[0];
                control.FechaIngresoLog = DateTime.Now;
                control.TerminalIngresoLog = Request.UserHostAddress;
                control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                ClsdConfiguracionMenu.GuardarModificarLoginFondo(control);
                if (dataImg != null)
                {
                    dataImg.SaveAs(path + Path.GetFileName(NombreImg));
                }
                return Json("Registro Exitoso", JsonRequestBehavior.AllowGet);
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
        public ActionResult ActivarFondoLogin(FONDO_LOGIN control)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsdConfiguracionMenu = new ClsdConfiguracionMenu();               
                control.UsuarioIngresoLog = lsUsuario[0];
                control.FechaIngresoLog = DateTime.Now;
                control.TerminalIngresoLog = Request.UserHostAddress;
                control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                ClsdConfiguracionMenu.ActivarLoginFondo(control);              
                return Json("Registro Exitoso", JsonRequestBehavior.AllowGet);
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



        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }

    }
}