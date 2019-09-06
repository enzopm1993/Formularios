using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;

namespace ProyectoWeb.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
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
            catch (Exception)
            {

                throw;
            }


        }
        [HttpPost]
        public JsonResult LogIn(string usuario, string password)
        {
            try
            {
                //clsApiUsuario a = new clsApiUsuario();
                //var b = a.ConsultaUsuarioEspecificoSap(usuario, password);
                clsDLogin clsDLogin = new clsDLogin();
                string psCodigoUsuario = clsDLogin.ConsultarUsuarioExiste(usuario, password);
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

                throw ex;
            }

        }
        
        
    }
}