using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.App;
using Asiservy.Automatizacion.Formularios.Models;
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

        public ActionResult MarcacionesJustificar()
        {

            ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            var request = new RestRequest("/api/Nomina/PendientesJustificarMarcacion", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var pendientesjustificarMarcacions = JsonConvert.DeserializeObject<List<AccesoDatos.App.PendientesjustificarMarcacion>>(content);


            return View(pendientesjustificarMarcacions);
        }

        public ActionResult Comunicados()
        {

            ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
            ViewBag.DateRangePicker = "1";
            ViewBag.summernote = "1";

            var client = new RestClient(clsAtributos.BASE_URL_WS);
            var request = new RestRequest("/api/Comunicados/Todos", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            
            var Comunicados = JsonConvert.DeserializeObject<List<Comunicados>>(content);
           

            request = new RestRequest("/api/Comunicados/Categorias", Method.GET);
            response = client.Execute(request);
            content = response.Content;
            var Categorias = JsonConvert.DeserializeObject<List<ClsKeyValue>>(content);

            ModeloVistaComunicados modeloVista = new ModeloVistaComunicados();
            modeloVista.Comunicados = Comunicados;
            modeloVista.Categorias = Categorias;

            return View(modeloVista);
        }


        [HttpPost]
        public ActionResult ObtieneLogMarcacion(paramBuscaMarcacion parametros)
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            var request = new RestRequest("/api/Empleado/Marcaciones/" + parametros.fechaIni + "/" + parametros.fechaFin + "/" + parametros.cedula, Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var datos = JsonConvert.DeserializeObject<AccesoDatos.App.ClsMarcaciones>(content);
            return Json(datos.LogMarcaciones, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult ProcesaComunicado(ParamComunicado parametros)
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            var request = new RestRequest("/api/Comunicados/Procesar", Method.POST);
            request.AddParameter("tipoProceso", parametros.tipoProceso);
            request.AddParameter("idComunicado", parametros.idComunicado);
            request.AddParameter("titulo", parametros.titulo);
            request.AddParameter("fechaDesde", parametros.fechaDesde);
            request.AddParameter("fechaHasta", parametros.fechaHasta);
            request.AddParameter("idCategoria", parametros.idCategoria);
            request.AddParameter("prioridad", parametros.prioridad);
            request.AddParameter("estado", parametros.estado);
            request.AddParameter("contenido", parametros.contenido);
            request.AddParameter("es_nueva_cat", parametros.es_nueva_cat);
            request.AddParameter("nombre_nueva_cat", parametros.nombre_nueva_cat);
            request.AddParameter("usuario", parametros.usuario);

            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var datos = JsonConvert.DeserializeObject<ClsKeyValue>(content);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EnviarMarcacion(ClsParamEnviarMarcacion parametros)
        {
            ClsKeyValue respuesta = new ClsKeyValue();
            respuesta.Codigo = "0";
            try
            {
                if (string.IsNullOrEmpty(parametros.cedula))
                {
                    respuesta.Descripcion = "La cédula del empleado es obligatoria";
                }
                else
                {
                    if (string.IsNullOrEmpty(parametros.tipoMarcacion))
                    {
                        respuesta.Descripcion = "Ingrese un tipo de marcación";
                    }
                    else
                    {
                        string fechaHora = parametros.diaMarcacion + " " + parametros.horaMarcacionCorrecta;
                        DateTime dateTime = DateTime.ParseExact(parametros.diaMarcacion, "dd/MM/yyyy", null);
                        DateTime fechaCompletaMarcacion = DateTime.Parse(dateTime.ToShortDateString() + " " + parametros.horaMarcacionCorrecta);

                        StatusOnlyControl resultOnlyControl;
                        using (OnlyControlService.wsrvTcontrolSoapClient service = new OnlyControlService.wsrvTcontrolSoapClient())
                        {

                            string content = service.InsertaMarcacion(clsAtributos.keyLlaveAcceso, parametros.cedula, fechaCompletaMarcacion, parametros.tipoMarcacion, "192.168.31.2");

                            resultOnlyControl = JsonConvert.DeserializeObject<StatusOnlyControl>(content);
                        }
                        respuesta.Descripcion = resultOnlyControl.mensaje;
                        if (resultOnlyControl.codigo == "0")
                        {
                            var client = new RestClient(clsAtributos.BASE_URL_WS);
                            var request = new RestRequest("/api/Admin/ActualizaEstadoSolicitudMarcacion", Method.POST);
                            request.AddParameter("id", parametros.idRegistro);
                            request.AddParameter("estado", "A");
                            request.AddParameter("observacion", "Marcación actualizada");
                            request.AddParameter("username", parametros.usuarioActualiza);
                            IRestResponse response = client.Execute(request);
                            var contentJs = response.Content;
                            var datos = JsonConvert.DeserializeObject<ClsKeyValue>(contentJs);

                            respuesta.Codigo = "1";
                        }                        
                    }
                }               
            }
            catch (Exception ex)
            {
               
                respuesta.Descripcion = ex.Message;
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);

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
    public class ParamComunicado
    {
        public string tipoProceso { get; set; }
        public Int32 idComunicado { get; set; }
        public string titulo { get; set; }
        public string fechaDesde { get; set; }
        public string fechaHasta { get; set; }
        public Int32 idCategoria { get; set; }
        public string prioridad { get; set; }
        public bool estado { get; set; }
        public string contenido { get; set; }
        public bool es_nueva_cat { get; set; }
        public string nombre_nueva_cat { get; set; }
        public string usuario { get; set; }
    }
    public class paramBuscaMarcacion
    {
        public string fechaIni { get; set; }
        public string fechaFin { get; set; }
        public string cedula { get; set; }
    }
}
