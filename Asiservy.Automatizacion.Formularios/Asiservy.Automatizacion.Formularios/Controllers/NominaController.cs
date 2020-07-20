using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.App;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.AccesoDatos.MoverPersonal;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Nomina;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
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
        clsDAsistencia ClsdAsistencia = null;
        ClsDMoverPersonal clsdMoverPersonal=null;
        ClsNomina ClsNomina = new ClsNomina();
        string[] lsUsuario;

        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }


        public ActionResult RptAsistenciaInicialActuales()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.Pivot = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.DateRangePicker = "1";
                ViewBag.Apexcharts = "1";
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


        public ActionResult ReporteHorasHombre()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.Pivot = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.DateRangePicker = "1";
                               
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

        [HttpGet]
        public JsonResult GenerarReporteHorasHombre(string fechaIni, string fechaFin)
        {
            try
            {

                ClsNomina clsNomina = new ClsNomina();

                List<ModeloVistaHorasHombre> modeloVistaTablasPersonalPresente = clsNomina.ObtenerReporteHorasHombre(Convert.ToDateTime(fechaIni), Convert.ToDateTime(fechaFin));


                JsonResult result = Json(modeloVistaTablasPersonalPresente, JsonRequestBehavior.AllowGet);

                result.MaxJsonLength = 50000000;
                return result;
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }


        }



        public ActionResult DatosPersonales()
        {
            try
            {
                //lsUsuario = User.Identity.Name.Split('_');
                //var client = new RestClient(clsAtributos.BASE_URL_WS);
                //var request = new RestRequest("/api/Empleado/"+ lsUsuario[1], Method.GET);
                //IRestResponse response = client.Execute(request);
                //var content = response.Content;
                //ClsEmpleado retornar = JsonConvert.DeserializeObject<ClsEmpleado>(content);
                //DatosEmpleadosViewModel datosEmpleadosViewModel = new DatosEmpleadosViewModel();
                //datosEmpleadosViewModel.DatosEmpleado = retornar;
                //request = new RestRequest("/api/Empleado/CambioDatosPendientes/" + lsUsuario[1], Method.GET);
                //response = client.Execute(request);
                //content = response.Content;
                //var idPendiente = JsonConvert.DeserializeObject<Int32>(content);
                //if (idPendiente > 0)
                //{
                //    datosEmpleadosViewModel.idSolicitud = idPendiente;
                //    datosEmpleadosViewModel.MensajeMuestra = "Existe una solicitud pendiente de revisión, no es posible actualizar los datos en este momento. Anule la solicitud pendiente para enviar una nueva o bien espere hasta que el área encargada valide su solicitud de actualización de datos.";
                //}
                //ViewBag.Title = "Datos personales";
                //ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                //return View(datosEmpleadosViewModel);
                ViewBag.dataTableJS = "1";

                List<ModelViewDatosEmpleados> modelViewDatosEmpleados = new List<ModelViewDatosEmpleados>();
                modelViewDatosEmpleados = ClsNomina.ListaEmpleadosDatosPersonales();
                               
                ViewBag.Title = "Datos personales";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                return View(modelViewDatosEmpleados);
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
        public ActionResult ActualizaDatosEmpleadoDataLife(ParamDatosEmpleadoDataLife parametros)
        {

            ClsKeyValue obReturn = new ClsKeyValue();
            try
            {
                List<ModelViewDatosEmpleados> modelViewDatosEmpleados = new List<ModelViewDatosEmpleados>();
                modelViewDatosEmpleados = ClsNomina.ListaEmpleadosDatosPersonales();

                ModelViewDatosEmpleados datosEmpleadoOld = modelViewDatosEmpleados.Where(c => c.CODTRA == parametros.codigo_data).FirstOrDefault();

                using (DataLifeService.ServicioAsiservySoapClient servicio = new DataLifeService.ServicioAsiservySoapClient())
                {

                    if (string.IsNullOrEmpty(parametros.direccion))
                    {
                        parametros.direccion = "*";
                    }
                    if (string.IsNullOrEmpty(parametros.barrio))
                    {
                        parametros.barrio = "*";
                    }
                    if (string.IsNullOrEmpty(parametros.telefono))
                    {
                        parametros.telefono = "*";
                    }
                    if (string.IsNullOrEmpty(parametros.celular))
                    {
                        parametros.celular = "*";
                    }
                    if (string.IsNullOrEmpty(parametros.correoPersonal))
                    {
                        parametros.correoPersonal = "*";
                    }
                    var content = servicio.actualizarDatosEmpleados(parametros.cedula, "1", parametros.direccion, parametros.barrio, parametros.telefono, parametros.celular, parametros.correoPersonal);
                    var dt = content.Tables[0];
                    var codigoReturn = dt.Rows[0]["iRetCode"].ToString();
                    var msgReturn = dt.Rows[0]["sErrMsg"].ToString();
                   
                    if (codigoReturn == "0")
                    {
                        ModelViewDatosEmpleados datosEmpleadosLog = new ModelViewDatosEmpleados();
                        datosEmpleadosLog.CODTRA = parametros.cedula;
                        datosEmpleadosLog.DIRECCION = string.Empty;
                        datosEmpleadosLog.BARRIO = string.Empty;
                        datosEmpleadosLog.TELEFONO = string.Empty;
                        datosEmpleadosLog.CELULAR = string.Empty;
                        datosEmpleadosLog.CORREO = string.Empty;

                        if (datosEmpleadoOld.DIRECCION.Trim() != parametros.direccion.Trim())
                        {
                            datosEmpleadosLog.DIRECCION = parametros.direccion;
                        }
                        if (datosEmpleadoOld.BARRIO.Trim() != parametros.barrio.Trim())
                        {
                            datosEmpleadosLog.BARRIO = parametros.barrio;
                        }
                        if (datosEmpleadoOld.TELEFONO.Trim() != parametros.telefono.Trim())
                        {
                            datosEmpleadosLog.TELEFONO = parametros.telefono;
                        }
                        if (datosEmpleadoOld.CELULAR.Trim() != parametros.celular.Trim())
                        {
                            datosEmpleadosLog.CELULAR = parametros.celular;
                        }
                        if (datosEmpleadoOld.CORREO.Trim() != parametros.correoPersonal.Trim())
                        {
                            datosEmpleadosLog.CORREO = parametros.correoPersonal;
                        }
                        lsUsuario = User.Identity.Name.Split('_');
                        var respLog = ClsNomina.insertarLogCambio(datosEmpleadosLog, lsUsuario[0], Request.UserHostAddress);                        
                        obReturn.Descripcion = "Registro actualizado";
                        obReturn.Codigo = "1";
                    }
                    else
                    {
                        obReturn.Codigo = "0";
                        obReturn.Descripcion = msgReturn;
                    }

                }
            }
            catch (Exception ex)
            {
                obReturn.Descripcion = ex.Message;
                obReturn.Codigo = "0";
            }

            return Json(obReturn, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActualizaInformacionDataLife(ParamCambioDatos parametros)
        {
            parametros.compania = "1";
            ClsKeyValue obReturn = new ClsKeyValue();
            using (DataLifeService.ServicioAsiservySoapClient servicio = new DataLifeService.ServicioAsiservySoapClient())
            {
                var content = servicio.actualizarDatosEmpleados(parametros.cedula, parametros.compania, parametros.direccion, parametros.barrio, parametros.telefono, parametros.celular, parametros.correoPersonal);
                var dt = content.Tables[0];
                var codigoReturn = dt.Rows[0]["iRetCode"].ToString();
                var msgReturn = dt.Rows[0]["sErrMsg"].ToString();
                obReturn.Descripcion = msgReturn;
                obReturn.Codigo = codigoReturn;
                if (codigoReturn == "0")
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
                ViewBag.Pivot = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                ModeloVistaAsistencia dataView = new ModeloVistaAsistencia();

                var client = new RestClient(clsAtributos.BASE_URL_WS);
                string URL = "/api/Nomina/Empresas/";
                
                var request = new RestRequest(URL, Method.GET);
                IRestResponse response = client.Execute(request);
                var content = response.Content;
                var datos = JsonConvert.DeserializeObject<List<ClsKeyValue>>(content);

                clsDLogin clsDLogin  = new clsDLogin();
                lsUsuario = User.Identity.Name.Split('_');
                List<int?> roles = clsDLogin.ConsultaRolesUsuario(lsUsuario[1]);
                int piRolOC = 0;
                if (roles.Any())
                {
                    piRolOC = roles.FirstOrDefault(x => x.Value == clsAtributos.RolControlOC) ?? 0;
                }
                List<ClsKeyValue> resu = new List<ClsKeyValue>();
                if (piRolOC == 0)
                {
                    resu = datos.Where(c => c.Codigo == "2").ToList();
                }else
                {
                    resu = datos;
                }


                dataView.ListaEmpresas = resu;
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
        [HttpGet]
        public JsonResult GenerarAsistenciaInicialVsActual(string fechaIni, string fechaFin)
        {
            try
            {
                
                ClsNomina clsNomina = new ClsNomina();

                List<ModeloVistaPersonalPresente> modeloVistaTablasPersonalPresente = clsNomina.ObtenerTablasPersonalAsistente(Convert.ToDateTime(fechaIni), Convert.ToDateTime(fechaFin));


                ModeloVistaRetornaAsistencia objRetorna = new ModeloVistaRetornaAsistencia();
                objRetorna.modeloVistaPersonalPresentes = modeloVistaTablasPersonalPresente;


                List<ModeloVistaPersonalPresenteBiometrico> modeloVistaTablasPersonalPresenteBiometrico = clsNomina.ObtenerAsistenciaBiomentrico(Convert.ToDateTime(fechaIni), Convert.ToDateTime(fechaFin));
                objRetorna.modeloVistaPersonalPresentesBiometrico = modeloVistaTablasPersonalPresenteBiometrico;


                var resultLineasAsistentesTotales = modeloVistaTablasPersonalPresenteBiometrico.Where(c=>c.TIPO_PROCESO == "PRODUCCIÓN")
                   .GroupBy(x => x.LINEA)                  
                   .Select(g => new ClsKpiLineasASistentes
                   {
                       Linea = g.Key,
                       Presentes = g.Sum(x => x.ESTADO_ASISTENCIA == "PRESENTE" ? 1 : 0),
                       Ausentes = g.Sum(x => x.ESTADO_ASISTENCIA == "AUSENTE" ? 1 : 0),

                   }).ToList();
                objRetorna.LineasAsistentesTotales = resultLineasAsistentesTotales;

                JsonResult result = Json(objRetorna, JsonRequestBehavior.AllowGet);

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
                var listFiltrada = ListaEmpleados.Where(c => !c.EXISTE_SAP).ToList();
                return PartialView(listFiltrada);

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
        [HttpPost]
        public ActionResult ActualizaEmpleadosArea(int IdMovimientoPersonal)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                string Compania = "1";
                ClsKeyValue obReturn = new ClsKeyValue();
                //clsDMovimientoPersonalNomina ClsMovimientoPersonalNomina = new clsDMovimientoPersonalNomina();
                using (DataLifeService.ServicioAsiservySoapClient servicio = new DataLifeService.ServicioAsiservySoapClient())
                {
                    List<string> arrCedulas = new List<string>();
                    string[] liststring = User.Identity.Name.Split('_');
                    DataSet content;
                    DataTable dt;
                    string codigoReturn;
                    string msgReturn;
                    //List<Respuesta> Respuesta = new List<Respuesta>();
                    Respuesta Respuesta = new Respuesta();
                    ClsdAsistencia = new clsDAsistencia();
                    clsdMoverPersonal = new ClsDMoverPersonal();
                    var personaAMover = clsdMoverPersonal.ConsultarMoverPersonalPorId(IdMovimientoPersonal);
                    if (ClsdAsistencia.CosultarAsistenciaEmpleado(personaAMover.Cedula, DateTime.Now))
                    {
                        //remover cedula y agregar msjerror
                        Respuesta=new Respuesta { cedula = personaAMover.Cedula, Codigo = "999", Descripcion = "No se puede mover, Ya tiene asistencia Presente en su línea" };
                    }
                    else
                    {
                        //arrCedulas.Add(personaAMover.Cedula);
                        content = servicio.actualizarCodigosEmpleados(personaAMover.Cedula, Compania, personaAMover.CentroCosto, personaAMover.Cargo, personaAMover.Linea, personaAMover.Recurso);
                        dt = content.Tables[0];
                        codigoReturn = dt.Rows[0]["iRetCode"].ToString();
                        msgReturn = dt.Rows[0]["sErrMsg"].ToString();
                        obReturn.Descripcion = msgReturn;
                        obReturn.Codigo = codigoReturn;
                        Respuesta=new Respuesta { cedula = personaAMover.Cedula, Codigo = obReturn.Codigo, Descripcion = obReturn.Descripcion };
                        
                        if (obReturn.Codigo == "0")
                        {
                            string GuardarBitacora = clsdMoverPersonal.GuardarBitacoraMovimientoPersonalNomina(personaAMover.Cedula, liststring[0], Request.UserHostAddress, personaAMover.CentroCosto, personaAMover.Recurso, personaAMover.Linea, personaAMover.Cargo);
                            string ActualizarEstadoAprobado = clsdMoverPersonal.ActualizarEstadoMoverPersonal(IdMovimientoPersonal, lsUsuario[0], Request.UserHostAddress, clsAtributos.EstadoAprobadoMoverPersonalN);

                        }

                    }
                    
                    return Json(Respuesta, JsonRequestBehavior.AllowGet);
                }
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

        [HttpPost]
        public ActionResult ActualizaEmpleadosAreaMas(string[] IdMovimientoPersonal)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                string Compania = "1";
                ClsKeyValue obReturn = new ClsKeyValue();
                //clsDMovimientoPersonalNomina ClsMovimientoPersonalNomina = new clsDMovimientoPersonalNomina();
                using (DataLifeService.ServicioAsiservySoapClient servicio = new DataLifeService.ServicioAsiservySoapClient())
                {
                    List<string> arrCedulas = new List<string>();
                    string[] liststring = User.Identity.Name.Split('_');
                    DataSet content;
                    DataTable dt;
                    string codigoReturn;
                    string msgReturn;
                    List<Respuesta> Respuesta = new List<Respuesta>();
                    //Respuesta Respuesta = new Respuesta();
                    ClsdAsistencia = new clsDAsistencia();
                    clsdMoverPersonal = new ClsDMoverPersonal();
                    
                    var personaAMover = clsdMoverPersonal.ConsultarMoverPersonalPorIdMas(Array.ConvertAll(IdMovimientoPersonal,s=>int.Parse(s)));
                    
                    foreach (var item in personaAMover.ToArray())
                    {
                        if (ClsdAsistencia.CosultarAsistenciaEmpleado(item.Cedula, DateTime.Now))
                        {
                            //remover cedula y agregar msjerror
                            Respuesta.Add(new Respuesta { cedula = item.Cedula, Codigo = "001", Descripcion = "No se puede mover, Tiene asistencia con estado Presente" });
                            personaAMover.Remove(personaAMover.Single(x => x.IdMoverPersonal == item.IdMoverPersonal));
                        }
                        else
                        {
                            arrCedulas.Add(item.Cedula);
                        }
                    }
                    //foreach (var cedula in parametros.Cedula)
                    foreach (var item in personaAMover)
                    {
                        content = servicio.actualizarCodigosEmpleados(item.Cedula, Compania, item.CentroCosto, item.Cargo, item.Linea, item.Recurso);
                        dt = content.Tables[0];
                        codigoReturn = dt.Rows[0]["iRetCode"].ToString();
                        msgReturn = dt.Rows[0]["sErrMsg"].ToString();
                        obReturn.Descripcion = msgReturn;
                        obReturn.Codigo = codigoReturn;
                        Respuesta.Add(new Respuesta { cedula = item.Cedula, Codigo = obReturn.Codigo, Descripcion = obReturn.Descripcion });
                        if (obReturn.Codigo == "0")
                        {
                            string GuardarBitacora = clsdMoverPersonal.GuardarBitacoraMovimientoPersonalNomina(item.Cedula, liststring[0], Request.UserHostAddress, item.CentroCosto, item.Recurso, item.Linea, item.Cargo);
                            string ActualizarEstadoAprobado = clsdMoverPersonal.ActualizarEstadoMoverPersonal(item.IdMoverPersonal, lsUsuario[0], Request.UserHostAddress, clsAtributos.EstadoAprobadoMoverPersonalN);

                        }
                    }

                    //return Json(obReturn, JsonRequestBehavior.AllowGet);
                    return Json(Respuesta, JsonRequestBehavior.AllowGet);
                }
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


        [HttpPost]
        public ActionResult EnviaSolicitudCambioDeDatos(ParamCambioDatos datosEmpleado)
        {

            var client = new RestClient(clsAtributos.BASE_URL_WS);
            var request = new RestRequest("/api/Empleado/CambiarDatos", Method.POST);
            request.AddParameter("username", datosEmpleado.username);
            request.AddParameter("cedula", datosEmpleado.cedula);
            request.AddParameter("direccion", datosEmpleado.direccion);
            request.AddParameter("barrio", datosEmpleado.barrio);
            request.AddParameter("telefono", datosEmpleado.telefono);
            request.AddParameter("celular", datosEmpleado.celular);
            request.AddParameter("correo", datosEmpleado.correoPersonal);

            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var datos = JsonConvert.DeserializeObject<ClsKeyValue>(content);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ProcesarEnvioEmpleados(parametrosEnvioSAP ParametrosEnvio)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            ServicePointManager.Expect100Continue = true;
            var client = new RestClient("https://192.168.0.30:50000");
            var request = new RestRequest("/b1s/v1/Login", Method.POST);
            request.AddHeader("Content-Type", "application/json;odata=minimalmetadata;charset=utf-8");

            EnvioSapLogin obLogin = new EnvioSapLogin();
            obLogin.CompanyDB = "SBO_ASISERVY_PROD";
            obLogin.UserName = "gintriago";
            obLogin.Password = "Agia1991*";
            request.AddJsonBody(obLogin);
            IRestResponse response = client.Execute(request);
            var content = response.Content;

            ReturnProcesoEnvioSAP returnSapLogin = new ReturnProcesoEnvioSAP();
            dynamic respResponse = JsonConvert.DeserializeObject(content);
            if (HttpStatusCode.OK == response.StatusCode)
            {

                string SessionId = respResponse.SessionId;
               
                List<string> listaCedulas = ParametrosEnvio.Cedulas.Split(',').ToList();

                ClsNomina clsNomina = new ClsNomina();
                EnvioClienteSAP obJsonCliente;
                foreach (string _cedula in listaCedulas)
                {

                    var datosEmpleado = clsNomina.ObtenerInfoEmpleadoParaSAP(_cedula);
                    obJsonCliente = new EnvioClienteSAP();

                    obJsonCliente.CardCode = datosEmpleado.CardCode;
                    obJsonCliente.CardName = datosEmpleado.CardName;
                    obJsonCliente.CardType = datosEmpleado.CardType;
                    obJsonCliente.GroupCode = Convert.ToInt32(datosEmpleado.GroupCode);
                    obJsonCliente.FederalTaxID = datosEmpleado.FederalTaxID;
                    obJsonCliente.Currency = datosEmpleado.Currency;
                    obJsonCliente.Phone1 = datosEmpleado.Phone1;
                    obJsonCliente.Cellular = datosEmpleado.Cellular;
                    obJsonCliente.MailAddress = datosEmpleado.MailAddress;
                    obJsonCliente.EmailAddress = datosEmpleado.EmailAddress;
                    obJsonCliente.ContactPerson = datosEmpleado.ContactPerson;
                    obJsonCliente.SalesPersonCode = datosEmpleado.SalesPersonCode;
                    obJsonCliente.DebitorAccount = datosEmpleado.DebitorAccount;
                    obJsonCliente.Properties13 = datosEmpleado.Properties13;
                    obJsonCliente.U_SYP_BPTD = datosEmpleado.U_SYP_BPTD;
                    obJsonCliente.U_SYP_PARTREL = datosEmpleado.U_SYP_PARTREL;
                    obJsonCliente.U_SYP_TIPPROV = datosEmpleado.U_SYP_TIPPROV;
                    obJsonCliente.U_SYP_CONTABILIDAD = datosEmpleado.U_SYP_CONTABILIDAD;
                    obJsonCliente.U_SYP_TCONTRIB = datosEmpleado.U_SYP_TCONTRIB;
                    obJsonCliente.U_SYP_FPAGO = datosEmpleado.U_SYP_FPAGO;
                    obJsonCliente.U_SYP_TIPOPAGO = datosEmpleado.U_SYP_TIPOPAGO;
                    obJsonCliente.U_SYP_TIPOREGI = datosEmpleado.U_SYP_TIPOREGI;
                    obJsonCliente.U_SYP_PAISPAGOGEN = datosEmpleado.U_SYP_PAISPAGOGEN;
                    obJsonCliente.U_SYP_PAISPAGO = datosEmpleado.U_SYP_PAISPAGO;
                    obJsonCliente.U_SYP_BPNO = datosEmpleado.U_SYP_BPNO;
                    obJsonCliente.U_SYP_BPN2 = datosEmpleado.U_SYP_BPN2;
                    obJsonCliente.U_SYP_BPAP = datosEmpleado.U_SYP_BPAP;
                    obJsonCliente.U_SYP_BPAM = datosEmpleado.U_SYP_BPAM;
                    obJsonCliente.U_SYP_FNACIM = datosEmpleado.U_SYP_FNACIM;
                    obJsonCliente.U_SYP_GENERO = datosEmpleado.U_SYP_GENERO;
                    obJsonCliente.U_SYP_EST_CIVIL = datosEmpleado.U_SYP_EST_CIVIL;
                    obJsonCliente.U_SYP_ORIGEN_INGRESO = datosEmpleado.U_SYP_ORIGEN_INGRESO;
                    obJsonCliente.U_SYP_ADTPAGO = datosEmpleado.U_SYP_ADTPAGO;
                    obJsonCliente.U_SYP_PESRET = datosEmpleado.U_SYP_PESRET;
                    obJsonCliente.U_SYP_BPAT = datosEmpleado.U_SYP_BPAT;
                    obJsonCliente.U_SYP_BROKER = datosEmpleado.U_SYP_BROKER;
                    obJsonCliente.U_SYP_COMI_BROKER = Convert.ToDecimal(datosEmpleado.U_SYP_COMI_BROKER);
                    obJsonCliente.U_SYP_TIPBROKER = datosEmpleado.U_SYP_TIPBROKER;
                    List<ClienteContactEmployees>  ContactEmployees = new List<ClienteContactEmployees>();
                    ContactEmployees.Add(new ClienteContactEmployees
                    {
                        CardCode = datosEmpleado.CardCode, Name = "DOCELECTRONICOS" , E_Mail = datosEmpleado.EmailAddress
                    });
                    obJsonCliente.ContactEmployees = ContactEmployees;

                    var requestEmp = new RestRequest("/b1s/v1/BusinessPartners", Method.POST);
                    requestEmp.AddHeader("Content-Type", "application/json;odata=minimalmetadata;charset=utf-8");
                    requestEmp.AddHeader("Authorization", "Bearer " + SessionId);
                    requestEmp.AddCookie("B1SESSION", SessionId);
                    requestEmp.AddJsonBody(obJsonCliente);
                    IRestResponse responseEmp = client.Execute(requestEmp);
                    var contentEmp = responseEmp.Content;
                    dynamic respResponseEmp = JsonConvert.DeserializeObject(content);

                    if (responseEmp.StatusCode != HttpStatusCode.Created)
                    {
                        returnSapLogin.Estado = -1;
                        returnSapLogin.StatusCodeDescription = "No se terminó de procesar el envío por un problema interno con el registro: "+ datosEmpleado.FederalTaxID + " - " + datosEmpleado.CardName + ", intente nuevamente";
                        break;
                    }
                    else
                    {
                        returnSapLogin.StatusCodeDescription = "Proceso completado con éxito";
                    }
                    //statusCode = Created
                }


                var requestLogout = new RestRequest("/b1s/v1/Logout", Method.POST);
                requestLogout.AddCookie("B1SESSION", SessionId);
                IRestResponse responseLogout = client.Execute(requestLogout);
                
            }
            else if (HttpStatusCode.Unauthorized == response.StatusCode)
            {
                returnSapLogin.Estado = 0;
                returnSapLogin.StatusCodeDescription = respResponse.error.message.value;
            }
            else 
            {
                returnSapLogin.Estado = -1;
                returnSapLogin.StatusCodeDescription = response.StatusDescription;
            }
            

            return Json(returnSapLogin);

        }
    }

    public class Respuesta
    {
        public string cedula { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
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
    public class ParamCambioPersonal
    {
        public string[] Cedula { get; set; }
        public string Compania { get; set; }
        public string CentroCostos { get; set; }
        public string Recurso { get; set; }
        public string Linea { get; set; }
        public string Cargo { get; set; }
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

    class EnvioSapLogin
    {        
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    class ReturnProcesoEnvioSAP
    {
        public int Estado { get; set; }
        public string StatusCodeDescription { get; set; }
        public string Mensaje { get; set; }
    }

    public class parametrosEnvioSAP
    {
        public string Cedulas { get; set; }
    }
    public class EnvioClienteSAP {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string CardType { get; set; }
        public int GroupCode { get; set; }
        public string FederalTaxID { get; set; }
        public string Currency { get; set; }
        public string Phone1 { get; set; }
        public string Cellular { get; set; }
        public string MailAddress { get; set; }
        public string EmailAddress { get; set; }
        public string ContactPerson { get; set; }
        public string SalesPersonCode { get; set; }
        public string DebitorAccount { get; set; }
        public string Properties13 { get; set; }
        public string U_SYP_BPTD { get; set; }
        public string U_SYP_PARTREL { get; set; }
        public string U_SYP_TIPPROV { get; set; }
        public string U_SYP_CONTABILIDAD { get; set; }
        public string U_SYP_TCONTRIB { get; set; }
        public string U_SYP_FPAGO { get; set; }
        public string U_SYP_TIPOPAGO { get; set; }
        public string U_SYP_TIPOREGI { get; set; }
        public string U_SYP_PAISPAGOGEN { get; set; }
        public string U_SYP_PAISPAGO { get; set; }
        public string U_SYP_BPNO { get; set; }
        public string U_SYP_BPN2 { get; set; }
        public string U_SYP_BPAP { get; set; }
        public string U_SYP_BPAM { get; set; }
        public string U_SYP_FNACIM { get; set; }
        public string U_SYP_GENERO { get; set; }
        public string U_SYP_EST_CIVIL { get; set; }
        public string U_SYP_ORIGEN_INGRESO { get; set; }
        public string U_SYP_ADTPAGO { get; set; }
        public string U_SYP_PESRET { get; set; }
        public string U_SYP_BPAT { get; set; }
        public string U_SYP_BROKER { get; set; }
        public decimal U_SYP_COMI_BROKER { get; set; }
        public string U_SYP_TIPBROKER { get; set; }

        public List<ClienteContactEmployees> ContactEmployees { get; set; }
    }
    public class ClienteContactEmployees
    {
        public string CardCode { get; set; }
        public string Name { get; set; }
        public string E_Mail { get; set; }
    }

    public class ParamDatosEmpleadoDataLife
    {
        public string cedula { get; set; }
        public string direccion { get; set; }
        public string barrio { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string correoPersonal { get; set; }
        public string codigo_data { get; set; }
    }
}
