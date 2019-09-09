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
        string[] liststring;
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
        // GET: Seguridad
        #region OPCION
        [Authorize]
        public ActionResult Opcion()
        {
            try
            {
                ConsultaOpciones();
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
                    UsuarioIngreso = "sistemas"
                });
                return RedirectToAction("Home","Home");
            }
        }
        
        [HttpPost]
        [Authorize]
        public ActionResult Opcion(OPCION model)
        {
            try
            {
                var errors = ModelState
               .Where(x => x.Value.Errors.Count > 0)
               .Select(x => new { x.Key, x.Value.Errors })
               .ToArray();
               
                if (string.IsNullOrEmpty(model.Nombre))
                {
                    ModelState.AddModelError("Nombre", "Campo Requerido");
                }
                
                if (string.IsNullOrEmpty(model.Clase))
                {
                    ModelState.AddModelError("Clase", "Campo Requerido");
                }
                if (model.Clase == "0" && model.Padre == null || model.Padre == 0)
                {
                    ModelState.AddModelError("Padre", "Campo Requerido");
                }
                if (model.Clase == "0" && string.IsNullOrEmpty(model.Url))
                {
                    ModelState.AddModelError("Url", "Campo Requerido");
                }
                if (string.IsNullOrEmpty(model.Formulario) && model.Clase=="0")
                {
                    ModelState.AddModelError("Formulario", "Campo Requerido");
                }
                ConsultaOpciones();
                if (!ModelState.IsValid)
                    return View(model);

                clsDopcion = new clsDOpcion();
                model.Clase = model.Clase == "0" ? "H" : "P";
                model.EstadoRegistro = model.EstadoRegistro == "true" ? clsAtributos.EstadoRegistroActivo : clsAtributos.EstadoRegistroInactivo;
                string[] Usuario = User.Identity.Name.Split('_');
                model.FechaCreacionLog = DateTime.Now;
                model.UsuarioCreacionLog = Usuario[0];
                model.TerminalCreacionLog = Request.UserHostAddress;
                var respuesta = clsDopcion.GuardarModificarOpcion(model);
                SetSuccessMessage(respuesta);
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
                    UsuarioIngreso = "sistemas"
                });
                return View();
            }
        }

        [Authorize]
        public ActionResult OpcionPartial()
        {
            try
            {
                clsDopcion = new clsDOpcion();
                var opciones = clsDopcion.ConsultarOpciones(null);
                return PartialView(opciones);

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
                    UsuarioIngreso = "sistemas"
                });
                return Json(ex.Message,JsonRequestBehavior.AllowGet);
            }
        }


        public void ConsultaOpciones()
        {
            clsDopcion = new clsDOpcion();
            var opciones = clsDopcion.ConsultarOpciones(new OPCION {EstadoRegistro=clsAtributos.EstadoRegistroActivo, Clase="P"}).Select(x => new { x.IdOpcion, x.Nombre });
            ViewBag.opciones = opciones;
            List<Clasificador> ClasificadorClase = new List<Clasificador>();
            ClasificadorClase.Add(new Clasificador { codigo = 0, descripcion = "Hijo" });
            ClasificadorClase.Add(new Clasificador { codigo = 1, descripcion = "Padre" });
            ViewBag.Clase = new SelectList(ClasificadorClase, "codigo", "descripcion");
        }

        #endregion

        #region ROL
        [Authorize]
        public ActionResult Rol()
        {
            try {
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
                    UsuarioIngreso = "sistemas"
                });
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
            liststring = User.Identity.Name.Split('_');
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
                    string psMensaje = poOpcion.GuardarRol(poRol, liststring[0], Request.UserHostAddress);
                    SetSuccessMessage(psMensaje);
                    return RedirectToAction("Rol");
                }
                
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
                    UsuarioIngreso = liststring[0]
                });
                return RedirectToAction("Rol");
            }
            return View(poRol);
        }

        #endregion

        #region OPCIONROL
        public void CargarCombosOpcionRol()
        {
            clsDopcion = new clsDOpcion();
            var plistOpciones = clsDopcion.ConsultarOpciones(new OPCION { EstadoRegistro = clsAtributos.EstadoRegistroActivo }).Select(x => new { x.IdOpcion, x.Nombre });
            ViewBag.OpcionesOr = plistOpciones;
            clsDRol = new clsDRol();
            var plistRoles = clsDRol.ConsultarRoles(clsAtributos.EstadoRegistroActivo).Select(x => new { x.IdRol, x.Descripcion });
            ViewBag.RolesOr = plistRoles;
        }
        [Authorize]
        public ActionResult OpcionRol()
        {
            liststring = User.Identity.Name.Split('_');
            try
            {

                CargarCombosOpcionRol();
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                return RedirectToAction("Home", "Home");
            }
        }
        #endregion


        #region USUARIO_ROL
        [Authorize]
        public ActionResult UsuarioRol()
        {
            try
            {
                ConsultaCombos();
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
                    UsuarioIngreso = "sistemas"
                });
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
                if (ModelState.IsValid)
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
                    UsuarioIngreso = "sistemas"
                });
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
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public ActionResult OpcionRol(OPCION_ROL OpcionRol,string IdRolh, string IdOpcionh)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                CargarCombosOpcionRol();
                if(string.IsNullOrEmpty(IdRolh))
                {
                    ModelState.AddModelError("ErrorIdRol", "El Campo Rol es obligatorio");
                }
                else
                {
                    OpcionRol.IdRol =Convert.ToInt32(IdRolh);
                }
                if (string.IsNullOrEmpty(IdOpcionh))
                {
                    ModelState.AddModelError("ErrorIdOpcion", "El Campo Opción es obligatorio");
                }
                else
                {
                    OpcionRol.IdOpcion= Convert.ToInt32(IdOpcionh);
                }
                if (string.IsNullOrEmpty(OpcionRol.EstadoRegistro))
                {
                    ModelState.AddModelError("ErrorEstado", "El Campo Estado es obligatorio");
                }
                if (ModelState.IsValid)
                {
                    OpcionesRol = new clsDOpcionRol();
                    string psRespuesta = OpcionesRol.GuardarOpcionRol(OpcionRol, liststring[0], Request.UserHostAddress);
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
                    UsuarioIngreso = liststring[0]
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
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        #region NIVEL_USUARIO
        [Authorize]
        public ActionResult NivelUsuario()
        {
            try
            {
                ConsultarComboNivelUsuario();
                return View();
            }
            catch (Exception ex) {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return RedirectToAction("Home","Home");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult NivelUsuario(NivelUsuarioViewModel model)
        {
            try
            {
                clsDNivelUsuario = new clsDNivelUsuario();
                if (ModelState.IsValid)
                {
                   
                    string[] Usuario = User.Identity.Name.Split('_');
                    model.EstadoRegistro = model.EstadoRegistro == "true" ? "A" : "I";
                    NIVEL_USUARIO NivelUsuario = new NIVEL_USUARIO
                    {
                        IdNivelUsuario= model.IdNivelUsuario??0,
                        IdUsuario = model.IdUsuario,
                        Nivel = model.Nivel,
                        EstadoRegistro=model.EstadoRegistro,
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
                    UsuarioIngreso = liststring[0]
                });
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
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public void ConsultarComboNivelUsuario()
        {
            clsApiUsuario = new clsApiUsuario();
            ViewBag.Usuarios = clsApiUsuario.ConsultaUsuariosSap();
            List<Clasificador> ClasificadorNivel = new List<Clasificador>();
            ClasificadorNivel.Add(new Clasificador { codigo = 0, descripcion = "Gerencia" });
            ClasificadorNivel.Add(new Clasificador { codigo = 1, descripcion = "Sub-Gerencia" });
            ClasificadorNivel.Add(new Clasificador { codigo = 2, descripcion = "Jefe Departamento" });
            ClasificadorNivel.Add(new Clasificador { codigo = 3, descripcion = "Empleado" });
            ViewBag.Nivel = ClasificadorNivel;
        }

        #endregion

    }
}