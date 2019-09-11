using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class AsistenciaController : Controller
    {

        clsDGeneral clsDGeneral = null;
        clsDEmpleado clsDEmpleado = null;
        clsDCambioPersonal clsDCambioPersonal = null;
        string[] liststring;
        #region Métodos
        public void ConsultaCombosGeneral()
        {
            clsDGeneral = new clsDGeneral();
            ViewBag.Lineas = clsDGeneral.ConsultaLineas();
            //ViewBag.Areas = clsDGeneral.ConsultaAreas("0");
            //ViewBag.Cargos = clsDGeneral.ConsultaCargos("0");
        }
        #endregion

        // GET: Asistencia
        [Authorize]
        public ActionResult Asistencia()
        {
            return View();
        }

        [Authorize]
        public ActionResult RptAsistencia()
        {
            return View();
        }

        [Authorize]
        public ActionResult EditarAsistencia()
        {
            return View();
        }

        #region Cambio_PersonaldeÁrea
        [Authorize]
        public ActionResult CambiarPersonalDeArea()
        {
            try
            {
                ConsultaCombosGeneral();
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        [Authorize]
        public ActionResult BitacoraCambioPersonal()
        {
            return View();
        }
        #endregion


        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult Cuchillo()
        {
            return View();
        }
        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult RptCuchillo()
        {
            return View();
        }

        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult ReporteDistribucion()
        {
            return View();
        }
        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult PersonalNomina()
        {
            return View();
        }
        public ActionResult EmpleadosCambioPersonalPartial(string pslinea, string psarea, string pscargo,string tipo)
        {
            try
            {
                List<spConsutaEmpleadosFiltro> ListaEmpleados=new List<spConsutaEmpleadosFiltro>();
                clsDEmpleado = new clsDEmpleado();
                if (tipo == "prestar")
                {
                    ListaEmpleados = clsDEmpleado.ConsultaEmpleadosFiltroCambioPersonal(pslinea, psarea, pscargo, clsAtributos.TipoPrestar);

                }
                else
                {
                    ListaEmpleados = clsDEmpleado.ConsultaEmpleadosFiltroCambioPersonal(pslinea, psarea, pscargo, clsAtributos.TipoRegresar);
                }
                return PartialView(ListaEmpleados);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        public JsonResult MoverEmpleados(string[] dCedulas, string dlinea, string darea, string tipo)
        {
            try
            {
                List<CAMBIO_PERSONAL> pListCambioPersonal = new List<CAMBIO_PERSONAL>();
                List<BITACORA_CAMBIO_PERSONAL> pListBitacoraCambioPersonal = new List<BITACORA_CAMBIO_PERSONAL>();
                liststring = User.Identity.Name.Split('_');
                string psRespuesta = string.Empty;
                if (dCedulas != null && dCedulas.Length > 0)
                {
                    foreach (var pscedulas in dCedulas)
                    {
                        if (!string.IsNullOrEmpty(pscedulas))
                        {
                            pListCambioPersonal.Add(new CAMBIO_PERSONAL {
                                Cedula = pscedulas,
                                CodLinea = dlinea,
                                CodArea = darea,
                                FechaIngresoLog = DateTime.Now,
                                UsuarioIngresoLog = liststring[0],
                                TerminalIngresoLog = Request.UserHostAddress,
                                EstadoRegistro = "A"
                            });
                            pListBitacoraCambioPersonal.Add(new BITACORA_CAMBIO_PERSONAL {
                            Cedula= pscedulas,
                            Tipo=tipo=="prestar"?"P":"R",
                            CodLinea= dlinea,
                            CodArea= darea,
                            FechaIngresoLog = DateTime.Now,
                            UsuarioIngresoLog = liststring[0],
                            TerminalIngresoLog = Request.UserHostAddress,
                            });
                        }
                    }
                    clsDCambioPersonal = new clsDCambioPersonal();
                    psRespuesta = clsDCambioPersonal.GuardarCambioDePersonal(pListCambioPersonal, pListBitacoraCambioPersonal, tipo);
                    return Json(psRespuesta, JsonRequestBehavior.AllowGet);
                }
                return Json("Error, no se ha seleccionado ningún empleado", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
