using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.BLZ;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    [Authorize]
    public class BLZController : Controller
    {
        string[] lsUsuario;
        clsDError clsDError = null;
        // GET: BLZ
        public ActionResult ReporteDescongeladoEmparrilladoMP()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.Title = "Reporte de descongelado y emparrillado de Materia Prima";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

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

        [HttpGet]
        public JsonResult GeneraDescongeladoEmparrilladoMP(string fechaPrd)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                string Cedula = lsUsuario[1];


                var client = new RestClient(clsAtributos.BASE_URL_WS);

                string URL = "/api/Produccion/ControlDescongeladoEmparrilladoMP/" + fechaPrd;

                var request = new RestRequest(URL, Method.GET);
                IRestResponse response = client.Execute(request);
                var content = response.Content;
                var datos = JsonConvert.DeserializeObject<List<RegistroDescongeladoEmparrilladoMP>>(content);

                JsonResult result = Json(datos, JsonRequestBehavior.AllowGet);

                result.MaxJsonLength = 50000000;
                return result;
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }


        }

        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
    }
}