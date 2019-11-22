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
            var client = new RestClient("http://192.168.0.31:8870");
            var request = new RestRequest("/api/Admin/SolicitudesPendientes", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var solicitudes = JsonConvert.DeserializeObject<List<AccesoDatos.App.SolicitudesPendientes>>(content);


            return View(solicitudes);
        }

        public ActionResult InfoSolicitudDatos(int id)
        {
            var client = new RestClient("http://192.168.0.31:8003");
            var request = new RestRequest("/api/Admin/InfoDatos/"+id.ToString(), Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var datos = JsonConvert.DeserializeObject<AccesoDatos.App.InfoCambioDatos>(content);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActualizaEstadoSolicitud(ParamCambioEstado parametros)
        {
            if (parametros.estado == "A")
            {
                //EJECUTAR WS DATALIFE
            }

            var client = new RestClient("http://192.168.0.31:8003");
            var request = new RestRequest("/api/Admin/ActualizaEstadoSolicitud", Method.POST);
            request.AddParameter("id", parametros.id);
            request.AddParameter("estado", parametros.estado);
            request.AddParameter("observacion", parametros.observacion);
            request.AddParameter("username", parametros.username);
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
    }
    public class ClsKeyValue
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
    }

}
