using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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

            if (usuario == "admin" && password == "admin")
            {
                FormsAuthentication.SetAuthCookie(usuario, false);
               
                return Json(1);
                
            }
                
            else
                return Json(0);

        }
        
        
    }
}