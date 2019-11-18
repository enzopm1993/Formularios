using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    [Authorize]
    public class NominaController : Controller
    {

       
        clsDError clsDError = null;
        string[] lsUsuario;

        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }

        public ActionResult EmpleadosClientes()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                return View();

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

        public ActionResult ListaEmpleadosPartial()
        {
            try
            {

                var client = new RestClient("http://192.168.0.31:8870");
                var request = new RestRequest("/api/Empleado/SapClientes", Method.GET);
                IRestResponse response = client.Execute(request);
                var content = response.Content;
                var ListaEmpleados = JsonConvert.DeserializeObject<List<clsEmpleadoCliente>>(content);
                return PartialView(ListaEmpleados);

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

    public class clsEmpleadoCliente
    {
        public string CODEMPLEADO { get; set; }
        public string NOMBRE_EMPLEADO { get; set; }
        public string CEDULA { get; set; }
        public string GENERO { get; set; }
        public string DEPARTAMENTO { get; set; }
        public string CARGO { get; set; }
        public string LINEA { get; set; }
        public string RECURSO { get; set; }
        public string CODIGO_SAP { get; set; }
        public bool EXISTE_SAP { get; set; }
    }
}
