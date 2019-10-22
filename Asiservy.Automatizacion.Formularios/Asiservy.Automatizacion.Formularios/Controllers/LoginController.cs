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
namespace ProyectoWeb.Controllers
{
    public class LoginController : Controller
    {
        clsDError clsDError = null;
     
        // GET: Login
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("home", "home");
            }
            return View();
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
            catch (Exception ex)
            {

                //SetErrorMessage(ex.Message);
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
                //clsDLogin clsDLogin = new clsDLogin();
                //string psCodigoUsuario = clsDLogin.ConsultarUsuarioExiste(usuario, password);
                if (!string.IsNullOrEmpty(psCodigoUsuario))
                {
                    FormsAuthentication.SetAuthCookie(usuario+"_"+psCodigoUsuario, false);

                    
                    return Json(1);

                }

                else
                    return Json(0);
            }
            catch (Exception ex)
            {

                //SetErrorMessage(ex.Message);
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
                return Json(ex.Message);
            }

        }
        
        
    }
}