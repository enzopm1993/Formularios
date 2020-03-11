using Asiservy.Automatizacion.Formularios.AccesoDatos;
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
    public class MemorandumController : Controller
    {
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

            modeloVistaMemos.Memorandum = new AccesoDatos.Memorandum.ModelMemorandum();

            return View(modeloVistaMemos);
        }

        public ActionResult Generar()
        {
            return View();
        }

    }
}