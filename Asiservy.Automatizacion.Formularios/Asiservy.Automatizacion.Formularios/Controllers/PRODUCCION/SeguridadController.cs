using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.Models;
using RestSharp;
using Asiservy.Automatizacion.Formularios.Models.Seguridad;
using Newtonsoft.Json;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using System.Net;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class SeguridadController : Controller
    {
        clsDOpcion clsDopcion = null;
        clsDRol clsDRol = null;
        clsDError clsDError = null;
        clsDUsuarioRol clsDUsuarioRol = null;
        clsApiUsuario clsApiUsuario = null;
        clsDNivelUsuario clsDNivelUsuario = null;
        clsDOpcionRol OpcionesRol = null;
        clsDClasificador clsDClasificador = null;
        clsDModulo clsDModulo = null;
        clsDParametro clsDParametro = null;
        string[] lsUsuario;
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
        // GET: Seguridad
        #region MODULOS
        [Authorize]
        public ActionResult Modulo()
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


        [Authorize]
        [HttpPost]
        public ActionResult Modulo(MODULO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');

                if (model == null || string.IsNullOrEmpty(model.Nombre))
                    return Json("0", JsonRequestBehavior.AllowGet);

                clsDModulo = new clsDModulo();
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                model.FechaIngresoLog = DateTime.Now;
                var mensaje = clsDModulo.GuardarModificarModulo(model);
                return Json(mensaje, JsonRequestBehavior.AllowGet);

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
        [Authorize]
        public ActionResult ModuloPartial()
        {
            try
            {
                clsDModulo = new clsDModulo();
                var model = clsDModulo.ConsultarModulos(new MODULO()).OrderByDescending(x => x.IdModulo);
                return PartialView(model);

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

        #endregion


        #region OPCION
        [Authorize]
        public ActionResult Opcion()
        {
            try
            {

                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.Select2 = "1";

                ConsultaOpciones();
                return View();

            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home","Home");
            }
        }
        
        [HttpPost]
        [Authorize]
        public ActionResult Opcion(OPCION model)
        {
            try
            {
               // var errors = ModelState
               //.Where(x => x.Value.Errors.Count > 0)
               //.Select(x => new { x.Key, x.Value.Errors })
               //.ToArray();
               
                if (string.IsNullOrEmpty(model.Nombre))
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                
                if (string.IsNullOrEmpty(model.Clase))
                {
                    return Json("0", JsonRequestBehavior.AllowGet);

                }
                if (model.Clase == "0" && (model.Padre == null || model.Padre == 0))
                {
                    return Json("0", JsonRequestBehavior.AllowGet);

                }
                if (model.Clase == "0" && string.IsNullOrEmpty(model.Url))
                {
                    return Json("0", JsonRequestBehavior.AllowGet);

                }
                if (string.IsNullOrEmpty(model.Formulario) && model.Clase=="0")
                {
                    return Json("0", JsonRequestBehavior.AllowGet);

                }             

                clsDopcion = new clsDOpcion();
                model.Clase = model.Clase == "0" ? "H" : "P";
                model.EstadoRegistro = model.EstadoRegistro == "A" ? clsAtributos.EstadoRegistroActivo : clsAtributos.EstadoRegistroInactivo;
                string[] Usuario = User.Identity.Name.Split('_');
                model.FechaCreacionLog = DateTime.Now;
                model.UsuarioCreacionLog = Usuario[0];
                model.TerminalCreacionLog = Request.UserHostAddress;
                if (model.Clase == "H")
                    model.IdModulo = null; 
                var respuesta = clsDopcion.GuardarModificarOpcion(model);

                return Json(respuesta, JsonRequestBehavior.AllowGet);


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

        [Authorize]
        public ActionResult OpcionPartial(int idModulo, string Clase)
        {
            try
            {
                clsDopcion = new clsDOpcion();
                List<OPCION> opciones = clsDopcion.ConsultarOpciones(new OPCION {IdModulo= idModulo}).OrderByDescending(x=> x.IdOpcion).ToList();
                var hijos = clsDopcion.ConsultarOpciones(new OPCION { Clase= Clase });
                if (Clase == "H") { 
                foreach (var x in hijos)
                    {
                        bool padre = opciones.Any(y => y.IdOpcion == x.Padre);
                        if (x.Clase == "H" && padre)
                            opciones.Add(x);
                    }
                }

                return PartialView(opciones);

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
        [Authorize]
        public JsonResult ConsultarPadres(int idModulo)
        {
            try
            {
                clsDopcion = new clsDOpcion();
                clsDModulo = new clsDModulo();
               
                List<ClasificadorGenerico> opciones = clsDopcion.ConsultaOpcionModulo().Select(x=>new ClasificadorGenerico {
                    codigo=x.IdOpcion,
                    descripcion = x.Opcion+" ("+x.Modulo+")"
                }).ToList();              

                return Json(opciones,JsonRequestBehavior.AllowGet);

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

        public void ConsultaOpciones()
        {
            clsDopcion = new clsDOpcion();
            clsDModulo = new clsDModulo();
            var modulos = clsDModulo.ConsultarModulos(new MODULO { EstadoRegistro = clsAtributos.EstadoRegistroActivo}).Select(x=> new ClasificadorGenerico {codigo=x.IdModulo,descripcion= x.Nombre});
            var opciones = clsDopcion.ConsultarOpciones(new OPCION {EstadoRegistro=clsAtributos.EstadoRegistroActivo, Clase="P"}).Select(x => new ClasificadorGenerico { codigo= x.IdOpcion,descripcion= x.Nombre });
            ViewBag.opciones = opciones;
            ViewBag.modulos = modulos;
            List<ClasificadorGenerico> ClasificadorClase = new List<ClasificadorGenerico>();
            ClasificadorClase.Add(new ClasificadorGenerico { codigo = 0, descripcion = "Hijo" });
            ClasificadorClase.Add(new ClasificadorGenerico { codigo = 1, descripcion = "Padre" });
            ViewBag.Clase = new SelectList(ClasificadorClase, "codigo", "descripcion");
        }

        #endregion

        #region ROL
        [Authorize]
        public ActionResult Rol()
        {
            try {
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
        [Authorize]
        public ActionResult ConsultaRoles()
        {
            try
            {
                clsDRol Opciones = new clsDRol();
                var ListaRoles = Opciones.ConsultarRoles(string.Empty);
                return PartialView(ListaRoles);
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Rol(ROL poRol)
        {
            lsUsuario = User.Identity.Name.Split('_');
            try
            {
                if (string.IsNullOrEmpty(poRol.Descripcion))
                {
                    ModelState.AddModelError("ErrorDescripcion", "El Campo Descripción es obligatorio");
                    
                }
                if (string.IsNullOrEmpty(poRol.EstadoRegistro))
                {
                    ModelState.AddModelError("ErrorEstado", "El Campo Estado es obligatorio");

                }
                if (ModelState.IsValid)
                {
                    clsDRol poOpcion = new clsDRol();
                    string psMensaje = poOpcion.GuardarRol(poRol, lsUsuario[0], Request.UserHostAddress);
                    SetSuccessMessage(psMensaje);
                    return RedirectToAction("Rol");
                }
                else
                {
                    return View(poRol);
                }
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Rol");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Rol");
            }          
        }

        #endregion

        #region OPCIONROL
        public void CargarCombosOpcionRol()
        {
            clsDopcion = new clsDOpcion();
            //var plistOpciones = clsDopcion.ConsultarOpciones(new OPCION { EstadoRegistro = clsAtributos.EstadoRegistroActivo }).Select(x => new { x.IdOpcion, x.Nombre});
            var plistOpciones = clsDopcion.ConsultarOpciones(new OPCION { EstadoRegistro = clsAtributos.EstadoRegistroActivo });

            foreach (var item in plistOpciones)
            {
                item.Nombre = item.Nombre + "(" + item.Clase + ")";
            }
            ViewBag.OpcionesOr = plistOpciones;
            clsDRol = new clsDRol();
            var plistRoles = clsDRol.ConsultarRoles(clsAtributos.EstadoRegistroActivo).Select(x => new { x.IdRol, x.Descripcion });
            ViewBag.RolesOr = plistRoles;
        }
        [Authorize]
        public ActionResult OpcionRol()
        {
            lsUsuario = User.Identity.Name.Split('_');
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.Select2 = "1";
                CargarCombosOpcionRol();
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
        [HttpPost]
        public ActionResult OpcionRol(OPCION_ROL OpcionRol, string IdRolh, string IdOpcionh)
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                lsUsuario = User.Identity.Name.Split('_');
                CargarCombosOpcionRol();
                if (string.IsNullOrEmpty(IdRolh))
                {
                    ModelState.AddModelError("ErrorIdRol", "El Campo Rol es obligatorio");
                }
                else
                {
                    OpcionRol.IdRol = Convert.ToInt32(IdRolh);
                }
                if (string.IsNullOrEmpty(IdOpcionh))
                {
                    ModelState.AddModelError("ErrorIdOpcion", "El Campo Opción es obligatorio");
                }
                else
                {
                    OpcionRol.IdOpcion = Convert.ToInt32(IdOpcionh);
                }
                if (string.IsNullOrEmpty(OpcionRol.EstadoRegistro))
                {
                    ModelState.AddModelError("ErrorEstado", "El Campo Estado es obligatorio");
                }
                if (ModelState.IsValid)
                {
                    OpcionesRol = new clsDOpcionRol();
                    string psRespuesta = OpcionesRol.GuardarOpcionRol(OpcionRol, lsUsuario[0], Request.UserHostAddress);
                    SetSuccessMessage(psRespuesta);
                    return RedirectToAction("OpcionRol");
                }
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario[0]
                });
                return RedirectToAction("OpcionRol");
            }
        }
        [Authorize]
        public ActionResult ConsultaOpcionRol()
        {
            try
            {
                OpcionesRol = new clsDOpcionRol();
                var ListaOpcionesRoles = OpcionesRol.ConsultarOpcionRol();
                return PartialView(ListaOpcionesRoles);
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        #region USUARIO_ROL
        [Authorize]
        public ActionResult UsuarioRol()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.Select2 = "1";
                ConsultaCombos();
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
        [Authorize]
        [HttpPost]
        public ActionResult UsuarioRol(UsuarioRolViewModel model)
        {
            try
            {                
                clsDUsuarioRol = new clsDUsuarioRol();
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.Select2 = "1";
                if (model.IdRol != null  && model.IdUsuario != null )
                {
                    string[] Usuario = User.Identity.Name.Split('_');
                    model.EstadoRegistro = model.EstadoRegistro == "true" ? "A" : "I";
                    model.FechaCreacionlog = DateTime.Now;
                    model.UsuarioCreacionlog = Usuario[0];
                    model.TerminalCreacionlog = Request.UserHostAddress;
                    ConsultaCombos();
                    string respuesta= clsDUsuarioRol.GuardarModificarUsuarioRol(model);
                    SetSuccessMessage(respuesta);
                    return View();
                }
                else
                {
                    ConsultaCombos();                    
                    return View(model);
                }
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("UsuarioRol");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("UsuarioRol");
            }          
        }

        [Authorize]
        public ActionResult UsuarioRolPartial()
        {
            try
            {
                clsDUsuarioRol = new clsDUsuarioRol();
                var model = clsDUsuarioRol.ConsultaUsuarioRol(null);
                return PartialView(model);
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

        public void ConsultaCombos()
        {
            clsDRol =new clsDRol();
            var roles = clsDRol.ConsultarRoles(clsAtributos.EstadoRegistroActivo);
            ViewBag.Roles = roles.Select(x=> new {x.IdRol, x.Descripcion});
            clsApiUsuario = new clsApiUsuario();
            ViewBag.Usuarios = clsApiUsuario.ConsultaUsuariosSap();

        }
       
        #endregion


        #region NIVEL_USUARIO
        [Authorize]
        public ActionResult NivelUsuario()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDNivelUsuario = new clsDNivelUsuario();
                ViewBag.ListaUsuarios = clsDNivelUsuario.ConsultarNivelUsuario(null).Where(X=> X.Nivel==clsAtributos.NivelJefe || X.Nivel == clsAtributos.NivelSubGerencia);
                ConsultarComboNivelUsuario();
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

        [Authorize]
        [HttpPost]
        public ActionResult NivelUsuario(NivelUsuarioViewModel model)
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                clsDNivelUsuario = new clsDNivelUsuario();
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.ListaUsuarios = clsDNivelUsuario.ConsultarNivelUsuario(null);
                if (ModelState.IsValid)
                {
                   
                    string[] Usuario = User.Identity.Name.Split('_');
                    model.EstadoRegistro = model.EstadoRegistro == "true" ? "A" : "I";
                    NIVEL_USUARIO NivelUsuario = new NIVEL_USUARIO
                    {
                        IdNivelUsuario= model.IdNivelUsuario??0,
                        IdUsuario = model.IdUsuario,
                        Nivel = model.Nivel,
                        CedulaAprueba = model.UsuarioAprueba,
                        EstadoRegistro =model.EstadoRegistro,
                        FechaIngresoLog = DateTime.Now,
                        UsuarioIngresoLog = Usuario[0],
                        TerminalIngresoLog = Request.UserHostAddress
                    };
                    
                   
                    string respuesta = clsDNivelUsuario.GuardarModificarNivelUsuario(NivelUsuario);
                    SetSuccessMessage(respuesta);
                    ConsultarComboNivelUsuario();
                    return View();
                }
                else
                {
                    ConsultarComboNivelUsuario();
                    return View(model);
                }
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

        [Authorize]
        public ActionResult NivelUsuarioPartial()
        {
            try
            {
                clsDNivelUsuario = new clsDNivelUsuario();
                var Lista = clsDNivelUsuario.ConsultarNivelUsuario(null);
                return PartialView(Lista);
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

        public void ConsultarComboNivelUsuario()
        {
            clsApiUsuario = new clsApiUsuario();
            clsDClasificador = new clsDClasificador();
            ViewBag.Usuarios = clsApiUsuario.ConsultaUsuariosSap();
            
            ViewBag.Nivel = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador {
                Grupo = clsAtributos.CodigoGrupoNivelUsuario,
                EstadoRegistro = clsAtributos.EstadoRegistroActivo
            });
        }

        #endregion



        #region CLASIFICADOR
        [Authorize]
        public ActionResult Clasificador()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                lsUsuario = User.Identity.Name.Split('_');
                clsDClasificador = new clsDClasificador();
                List<Clasificador> Grupos = new List<Clasificador>();
                //List<Clasificador> Clasificador = new List<Clasificador>();

                Grupos = clsDClasificador.ConsultaClasificadorGrupos();
                //Clasificador = clsDClasificador.ConsultaClasificador(new Clasificador());
                ViewBag.Grupos = Grupos;
                //ViewBag.Clasificador = Clasificador;
                
                return View();

            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                lsUsuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario[0]
                });
                return View();
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult Clasificador(Clasificador model,bool ValidaGrupo)
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                lsUsuario = User.Identity.Name.Split('_');             
                clsDClasificador = new clsDClasificador();
                List<Clasificador> Grupos = new List<Clasificador>();
             
                if (!ModelState.IsValid || (!ValidaGrupo &&  string.IsNullOrEmpty(model.Grupo)))
                {
                    if (!ValidaGrupo && string.IsNullOrEmpty(model.Grupo))
                        ModelState.AddModelError("Grupo","Campo Requerido");
                    Grupos = clsDClasificador.ConsultaClasificadorGrupos();
                    ViewBag.Grupos = Grupos;
                    return View(model);
                }
                if (ValidaGrupo)
                {
                    model.Grupo = model.Codigo;
                    model.Codigo = "0";
                }
                model.EstadoRegistro = model.EstadoRegistro == "true" ? "A" : "I";
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress+"";
                Grupos = clsDClasificador.ConsultaClasificadorGrupos();
                var mensaje = clsDClasificador.GuardarModificarClasificador(model);
                SetSuccessMessage(mensaje);
                ViewBag.Grupos = Grupos;               

                return View();

            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario[0]
                });
                return RedirectToAction("Clasificador");
            }
        }

  
        public ActionResult ClasificadorPartial()
        {
            try
            {
                clsDClasificador = new clsDClasificador();
                 List<spConsultaClasificador> Clasificador = new List<spConsultaClasificador>();
                Clasificador = clsDClasificador.ConsultarClasificador().OrderBy(x=> x.Grupo).ToList();               
                return PartialView(Clasificador);

            }
            catch (Exception ex)
            {
                //SetErrorMessage(ex.Message);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario[0]
                });
                return Json(ex.Message,JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region PARAMETROS
        [Authorize]
        public ActionResult Parametro()
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
                return View();
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult Parametro(PARAMETRO model)
        {
            try
            {
                RespuestaGeneral Respuesta = new RespuestaGeneral();
                if (string.IsNullOrEmpty(model.Codigo))
                {
                    Respuesta.Codigo = 0;
                    Respuesta.Mensaje = "Ingrese un Codigo";
                    return Json(Respuesta, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.Descripcion))
                {
                    Respuesta.Codigo = 0;
                    Respuesta.Mensaje = "Ingrese una Descripcion";
                    return Json(Respuesta, JsonRequestBehavior.AllowGet);
                }

                //if (model.Valor<0)
                //{
                //    Respuesta.Codigo = 0;
                //    Respuesta.Mensaje = "Ingrese un Valor";
                //    return Json(Respuesta, JsonRequestBehavior.AllowGet);
                //}

                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                lsUsuario = User.Identity.Name.Split('_');
                clsDParametro = new clsDParametro();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.FechaIngresoLog = DateTime.Now;
               // model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                string Mensaje = clsDParametro.GuardarModificarParametro(model);
                Respuesta.Codigo = 1;
                Respuesta.Mensaje = Mensaje;
                return Json(Respuesta, JsonRequestBehavior.AllowGet);

            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);               
                return Json(Mensaje , JsonRequestBehavior.AllowGet);
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

        [Authorize]
        public ActionResult ParametroPartial()
        {
            try
            {
                clsDParametro = new clsDParametro();
                List<PARAMETRO> Parametros = new List<PARAMETRO>();
                Parametros = clsDParametro.ConsultaParametros(new PARAMETRO()).ToList();
                return PartialView(Parametros);

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

        #endregion




    }
}