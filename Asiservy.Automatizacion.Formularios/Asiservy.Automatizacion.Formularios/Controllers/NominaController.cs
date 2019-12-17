using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.App;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Nomina;
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
        public ActionResult Asistencia()
        {
            try
            {
                ViewBag.Apexcharts = "1";
                ViewBag.Handsontable = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                ModeloVistaAsistencia dataView = new ModeloVistaAsistencia();

                var client = new RestClient(clsAtributos.BASE_URL_WS);
                string URL = "/api/Nomina/Empresas/";
                
                var request = new RestRequest(URL, Method.GET);
                IRestResponse response = client.Execute(request);
                var content = response.Content;
                var datos = JsonConvert.DeserializeObject<List<ClsKeyValue>>(content);

                dataView.ListaEmpresas = datos;
                return View(dataView);
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

        [HttpPost]
        public JsonResult GenerarAsistencial(ParamAsistencia parametros)
        {
            try
            {
                var client = new RestClient(clsAtributos.BASE_URL_WS);
                string URL = "/api/Nomina/AsistenciaGeneral/" + parametros.fechaIni + "/" + parametros.fechaFin + "/" + parametros.empresa;
                if (!string.IsNullOrEmpty(parametros.cedula))
                {
                    URL = URL + "/" + parametros.cedula;
                }
                var request = new RestRequest(URL, Method.GET);
                IRestResponse response = client.Execute(request);
                var content = response.Content;
                var datos = JsonConvert.DeserializeObject<List<ClsRegistroAsistencia>>(content);



               var resultGeneros = datos
                    .GroupBy(x => x.GENERO)
                    .Select(g => new ClsKpiGenero
                    {
                        Genero = g.Key,
                        Presentes = g.Sum(x => x.PRESENTE  ? 1 : 0),
                        Ausentes = g.Sum(x => x.AUSENTE ? 1 : 0),
                        AusentesConPermiso = g.Sum(x => x.AUSENTE && x.CON_PERMISO ? 1 : 0),
                        AusentesSinPermiso = g.Sum(x => x.AUSENTE && !x.CON_PERMISO ? 1 : 0),

                    }).ToList();

                var resultPermisos = datos.Where(c => c.CON_PERMISO )
                    .GroupBy(x => x.NOVEDAD)                           
                    .Select(g => new ClsKpiDescripcionTotal
                    {
                        Descripcion = g.Key,
                        Total = g.Sum(x => x.CON_PERMISO ? 1 : 0)

                    }).OrderByDescending(c=>c.Total).ToList();

                var resultDiasAusentismo = datos.Where(c => c.AUSENTE && c.DIA_ESPECIAL )
                    .GroupBy(x => x.DIA)
                    .Select(g => new ClsKpiDescripcionTotal
                    {
                        Descripcion = g.Key,
                        Total = g.Count()

                    }).ToList();


                ModeloVistaRptAsistencia resultadoJS = new ModeloVistaRptAsistencia();

                resultadoJS.dataGeneral = datos;
                resultadoJS.TotalGeneros = resultGeneros;
                resultadoJS.TotalPermisos = resultPermisos;
                resultadoJS.TotalDias = resultDiasAusentismo;


                resultadoJS.TotalPersonas = datos.Count();
                resultadoJS.TotalAsistentes = datos.Where(c => c.PRESENTE).Count();  
                resultadoJS.TotalAusentes = datos.Where(c => c.AUSENTE).Count();
                resultadoJS.TotalConPermiso = datos.Where(c => c.AUSENTE && c.CON_PERMISO).Count();
                resultadoJS.TotalSinPermiso = datos.Where(c => c.AUSENTE && !c.CON_PERMISO).Count();


                JsonResult result = Json(resultadoJS, JsonRequestBehavior.AllowGet);
                result.MaxJsonLength = 50000000;
                return result;
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }

           
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
    public class ParamAsistencia
    {
        public string fechaIni { get; set; }
        public string fechaFin { get; set; }
        public string empresa { get; set; }
        public string cedula { get; set; }
    }
    public class ClsRegistroAsistencia
    {
        public string DIA { get; set; }
        public string NOMBRE_DIA { get; set; }
        public int NUMERO_DIA { get; set; }
        public string EMPRESA { get; set; }
        public string CEDULA { get; set; }
        public string CODIGO { get; set; }
        public string ESTADO_EMPLEADO { get; set; }
        public string GENERO_CODIGO { get; set; }
        public string GENERO { get; set; }
        public string NOMBRES { get; set; }
        public string TIPO_ROL { get; set; }
        public string AREA { get; set; }
        public string CARGO { get; set; }
        public string LINEA { get; set; }
        public string RECURSO { get; set; }
        public string INGRESO { get; set; }
        public string ALMUERZO { get; set; }
        public string CENA { get; set; }
        public string SALIDA { get; set; }
        public bool DIA_FERIADO { get; set; }
        public string DESC_DIA_FERIADO { get; set; }
        public bool DIA_ESPECIAL { get; set; }
        public string DESC_MODALIDAD { get; set; }
        public bool PRESENTE { get; set; }
        public bool AUSENTE { get; set; }
        public bool CON_PERMISO { get; set; }
        public string NOVEDAD { get; set; }
        public string OBSERVACION { get; set; }
        public string DIA_INICIA_PERMISO { get; set; }
        public string DIA_FIN_PERMISO { get; set; }
        public string HORA_INICIA_PERMISO { get; set; }
        public string HORA_FIN_PERMISO { get; set; }
    }
}
