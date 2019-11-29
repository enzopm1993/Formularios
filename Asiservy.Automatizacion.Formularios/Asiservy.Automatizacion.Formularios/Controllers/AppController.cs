using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    [Authorize]
    public class AppController : Controller
    {
        // GET: App
        public ActionResult Index()
        {
            return View();
        }

        // GET: App/Details/5
        public ActionResult SolicitudesPendientes()
        {
          
            ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            var request = new RestRequest("/api/Admin/SolicitudesPendientes", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var solicitudes = JsonConvert.DeserializeObject<List<AccesoDatos.App.SolicitudesPendientes>>(content);


            return View(solicitudes);
        }

        public ActionResult Sugerencias()
        {

            ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            var request = new RestRequest("/api/Admin/Sugerencias", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var sugerencias = JsonConvert.DeserializeObject<List<AccesoDatos.App.Sugerencias>>(content);


            return View(sugerencias);
        }


        public ActionResult InfoSolicitudDatos(int id)
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            var request = new RestRequest("/api/Admin/InfoDatos/"+id.ToString(), Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var datos = JsonConvert.DeserializeObject<AccesoDatos.App.InfoCambioDatos>(content);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActualizaEstadoSolicitud(ParamCambioEstado parametros)
        {
           
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            var request = new RestRequest("/api/Admin/ActualizaEstadoSolicitud", Method.POST);
            request.AddParameter("id", parametros.id);
            request.AddParameter("estado", parametros.estado);
            request.AddParameter("observacion", parametros.observacion);
            request.AddParameter("username", parametros.username);
            request.AddParameter("tipo", parametros.tipo);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var datos = JsonConvert.DeserializeObject<ClsKeyValue>(content);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }


    }
    public class ParamCambioEstado
    {
        public int id { get; set; }
        public string estado { get; set; }
        public string observacion { get; set; }
        public string username { get; set; }
        public string tipo { get; set; }
    }
    public class ClsKeyValue
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
    }

}
