using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.App;
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


        public ActionResult Certificados()
        {
            try
            {
                ViewBag.summernote = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                var client = new RestClient(clsAtributos.BASE_URL_WS);

                var request = new RestRequest("/api/Certificados/Todos", Method.GET);
                IRestResponse response = client.Execute(request);
                var content = response.Content;
                var ListaCertificados = JsonConvert.DeserializeObject<List<Certificados>>(content);

                request = new RestRequest("/api/Certificados/Tags", Method.GET);
                response = client.Execute(request);
                content = response.Content;
                var listaTags = JsonConvert.DeserializeObject<List<ClsKeyValue>>(content);

                ModeloVistaCertificados modeloVistaCertificados = new ModeloVistaCertificados();
                modeloVistaCertificados.Certificados = ListaCertificados;
                modeloVistaCertificados.TagsPlantilla = listaTags;

                return View(modeloVistaCertificados);

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

        [HttpPost]
        public ActionResult ProcesaCertificado(ParamCertificado parametros)
        {
           
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            var request = new RestRequest("/api/Certificados/Procesar", Method.POST);
            request.AddParameter("Id", parametros.Id);
            request.AddParameter("Descripcion", parametros.Descripcion);
            request.AddParameter("Estado", parametros.Estado);
            request.AddParameter("ConPlantilla", parametros.ConPlantilla);
            request.AddParameter("Plantilla", parametros.Plantilla);
            request.AddParameter("usuario", parametros.usuario);
            request.AddParameter("Opcion", parametros.Opcion);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var datos = JsonConvert.DeserializeObject<ClsKeyValue>(content);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActualizaInformacionDataLife(ParamCambioDatos parametros)
        {
            parametros.compania = "1";
            ClsKeyValue obReturn = new ClsKeyValue();
            using ( DataLifeService.ServicioAsiservySoapClient servicio = new DataLifeService.ServicioAsiservySoapClient())
            {
                var content = servicio.actualizarDatosEmpleados(parametros.cedula, parametros.compania, parametros.direccion, parametros.barrio, parametros.telefono, parametros.celular, parametros.correoPersonal);
                var dt = content.Tables[0];
                var codigoReturn = dt.Rows[0]["iRetCode"].ToString();
                var msgReturn = dt.Rows[0]["sErrMsg"].ToString();
                obReturn.Descripcion = msgReturn;
                obReturn.Codigo = codigoReturn;
                if (codigoReturn =="0")
                {
                    var client = new RestClient(clsAtributos.BASE_URL_WS);
                    var request = new RestRequest("/api/Admin/ActualizaEstadoSolicitud", Method.POST);
                    request.AddParameter("id", parametros.id_solicitud);
                    request.AddParameter("estado", "A");
                    request.AddParameter("observacion", "");
                    request.AddParameter("username", parametros.username);
                    request.AddParameter("tipo", "datos");
                    IRestResponse response = client.Execute(request);
                    var content2 = response.Content;
                    var datos = JsonConvert.DeserializeObject<ClsKeyValue>(content2);
                    obReturn.Codigo = "1";                  
                }
                else
                {
                    obReturn.Codigo = "0";
                    obReturn.Descripcion = msgReturn;
                }
                return Json(obReturn, JsonRequestBehavior.AllowGet);
            }          
          
        }



        [HttpPost]
        public ActionResult ObtenerCertificado(int id)
        {

            var client = new RestClient(clsAtributos.BASE_URL_WS);
            var request = new RestRequest("/api/Certificados/"+ id.ToString(), Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var datos = JsonConvert.DeserializeObject<List<Certificados>>(content);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ObtenerCertificadoGenerado(int id)
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            var request = new RestRequest("/api/Empleado/GeneraSolicitudCertificado/" + id.ToString(), Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var datos = JsonConvert.DeserializeObject<string>(content);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ListaEmpleadosPartial()
        {
            try
            {

                var client = new RestClient(clsAtributos.BASE_URL_WS);
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

    public class ParamCertificado
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public bool ConPlantilla { get; set; }
        public string Plantilla { get; set; }
        public string usuario { get; set; }
        public string Opcion { get; set; }
    }
    public class ParamCambioDatos
    {
        public string cedula { get; set; }
        public string compania { get; set; }
        public string direccion { get; set; }
        public string barrio { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string correoPersonal { get; set; }
        public int id_solicitud { get; set; }
        public string username { get; set; }
    }
}
