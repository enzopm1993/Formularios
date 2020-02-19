﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using System.Data.Entity.Validation;
using Asiservy.Automatizacion.Formularios.Models;
using RestSharp;
using Newtonsoft.Json;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    [Authorize]
    public class MarcacionesController : Controller
    {
        string[] lsUsuario;
        clsDError clsDError = null;
        public ActionResult PorEmpleado()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.Title = "Estado de mis marcaciones";
               
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
        public ActionResult PorLinea()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.Title = "Registro de marcaciones";

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
        public JsonResult ObtenerMarcacionesEmpleados(string fechaIni, string fechaFin, string desde)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                string Cedula = lsUsuario[1];

                MarcacionesEmpleadoLineaViewModel dataView = new MarcacionesEmpleadoLineaViewModel();

                var client = new RestClient(clsAtributos.BASE_URL_WS);
             
                string URL = "/api/Marcaciones/"+ fechaIni + "/"+ fechaFin + "/" + Cedula + "/"+ desde;

                var request = new RestRequest(URL, Method.GET);
                IRestResponse response = client.Execute(request);
                var content = response.Content;
                var datos = JsonConvert.DeserializeObject<List<MarcacionesEmpleadoLineaViewModel>>(content);

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