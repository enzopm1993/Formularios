using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Memorandum;
using Asiservy.Automatizacion.Formularios.Models;
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
    public class MemorandumController : Controller
    {
        clsDError clsDError = null;
        string[] lsUsuario;
        ClsMemorandum clsMemorandum;
        // GET: Memorandum
        public ActionResult Gestion(int? id)
        {

            ViewBag.summernote = "1";
            ViewBag.Title = "Gestión de memorandum";
            ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

            var client = new RestClient(clsAtributos.BASE_URL_WS);

            var request = new RestRequest("/api/Memorandum/Tags", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var listaTags = JsonConvert.DeserializeObject<List<ClsKeyValue>>(content);

            ModelViewMemorandum modeloVistaMemos = new ModelViewMemorandum();
            modeloVistaMemos.TagsPlantilla = listaTags;

            if (id != null)
            {
                clsMemorandum = new ClsMemorandum();
                var existente = clsMemorandum.ObtenerMemo(id);
                if (existente == null)
                {
                    return HttpNotFound();
                }
                modeloVistaMemos.Memorandum  = existente;
            }
            else
            {
                modeloVistaMemos.Memorandum = new PlantillasMemorandum();
                
            }

            return View(modeloVistaMemos);
        }
        public ActionResult Plantillas()
        {

            try
            {
                clsMemorandum = new ClsMemorandum();
                ViewBag.Title = "Plantillas de memorandum";
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                var lista = clsMemorandum.OtenerMemos();
                return View(lista);
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
        public ActionResult Generar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ProcesaMemorandum(PlantillasMemorandum parametros)
        {
            ClsKeyValue clsKeyValue = new ClsKeyValue();
            clsKeyValue.Codigo = "0";
            try
            {
                if (string.IsNullOrEmpty(parametros.Titulo))
                {
                    clsKeyValue.Descripcion = "El título del memorandum es obligatorio";
                }
                else
                {
                    if (string.IsNullOrEmpty(parametros.Plantilla))
                    {
                        clsKeyValue.Descripcion = "La plantilla es obligatoria";
                    }
                    else
                    {
                        
                        if (parametros.id > 0)
                        {
                            parametros.UsuarioModifica = User.Identity.Name.Split('_')[0];
                            parametros.TerminalModifica = Request.UserHostAddress;
                            parametros.FechaModifica = DateTime.Now;
                            clsKeyValue.Descripcion = "Registro modificado con éxito";
                        }
                        else
                        {
                            parametros.UsuarioIngresa = User.Identity.Name.Split('_')[0];
                            parametros.TerminalImgresa = Request.UserHostAddress;
                            parametros.FechaCrea = DateTime.Now;
                            clsKeyValue.Descripcion = "Registro ingresado con éxito";
                        }
                        clsMemorandum = new ClsMemorandum();
                        clsKeyValue.Codigo = clsMemorandum.GuardarPlantillaMemo(parametros).ToString();
                        
                        
                    }
                }
            }
            catch (Exception ex)
            {
                
                clsKeyValue.Descripcion = ex.Message;
            }
            
            return Json(clsKeyValue, JsonRequestBehavior.AllowGet);
        }

        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
    }
}