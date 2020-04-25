using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Datos.Datos;
using System.Data.Entity.Validation;
using System.Net;
using Asiservy.Automatizacion.Formularios.Models;

namespace ProyectoWeb.Controllers
{
    public class LoginController : Controller
    {
        clsDError clsDError = null;
        clsDLogin clsDLogin = null;
        clsApiUsuario clsApiUsuario = null;
        string[] lsUsuario;
        clsDGeneral clsDGeneral = null;
        // GET: Login
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("home", "home");
            }
            clsDGeneral = new clsDGeneral();
            string BD = clsDGeneral.getDataBase();
            if (BD == clsAtributos.DesarrolloBD)
            {
               ViewBag.BD = clsAtributos.BDDesarrollo;
            }
            if (BD == clsAtributos.PreProduccionBD)
            {
                ViewBag.BD = clsAtributos.BDPreProduccion;
            }
            if (BD == clsAtributos.ProduccionBD)
            {
                ViewBag.BD = clsAtributos.BDProduccion;
            }

            return View();
        }
        public JsonResult ConsultarBD()
        {
            clsDGeneral = new clsDGeneral();
            string BD = clsDGeneral.getDataBase();
            List<RespuestaGeneral> respuesta = new List<RespuestaGeneral>();
            if (BD == clsAtributos.DesarrolloBD)
            {
                respuesta.Add(new RespuestaGeneral { Codigo = 1, Descripcion = "http://192.168.0.31:8001/", Mensaje = clsAtributos.BDProduccion });
            }
            if (BD == clsAtributos.PreProduccionBD)
            {
                respuesta.Add(new RespuestaGeneral { Codigo = 1, Descripcion = "http://192.168.0.31:8001/", Mensaje=clsAtributos.BDProduccion });
            }
            if (BD == clsAtributos.ProduccionBD)
            {
                respuesta.Add(new RespuestaGeneral { Codigo = 0, Descripcion = "http://192.168.0.31:8000/", Mensaje = clsAtributos.BDPreProduccion });
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                return RedirectToAction("Login", "Login");
                //return Json(0);
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                //SetErrorMessage(Mensaje);
                return RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
               // SetErrorMessage(Mensaje);
                return RedirectToAction("Login", "Login");
            }


        }
        [HttpPost]
        public JsonResult LogIn(string usuario, string password)
        {
            try
            {                
                clsApiUsuario poUsuario = new clsApiUsuario();
                string psCodigoUsuario = poUsuario.ConsultaUsuarioEspecificoSap(usuario, password);               
                if (!string.IsNullOrEmpty(psCodigoUsuario))
                {
                    FormsAuthentication.SetAuthCookie(usuario.ToUpper()+"_"+psCodigoUsuario, false);
                    return Json(1);
                }
                else
                    return Json(0);
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
        
        public ActionResult ValidarOpcionUsuario(string Usuario, string Opcion)
        {
            try
            {
                clsDLogin = new clsDLogin();
                var valida = clsDLogin.ValidarPermisoOpcion(Usuario, Opcion);
                return Json(valida, JsonRequestBehavior.AllowGet);

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


        public ActionResult CambiarClave(string Usuario,string claveActual ,string clave1, string clave2)
        {
            try
            {
                clsApiUsuario = new clsApiUsuario();


                if (string.IsNullOrEmpty(Usuario)|| string.IsNullOrEmpty(clave1) || string.IsNullOrEmpty(clave2) || string.IsNullOrEmpty(claveActual))
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                if (clave1 != clave2){
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
              

                var respuesta = clsApiUsuario.CambiarClaveLogin(Usuario, claveActual, clave1);
                return Json(respuesta, JsonRequestBehavior.AllowGet);

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



    }
}