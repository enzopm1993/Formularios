using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ProyeccionProgramacion;
using Asiservy.Automatizacion.Formularios.Models.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class ProyeccionProgramacionController : Controller
    {
        clsDError clsDError = null;
        clsDClasificador clsDClasificador = null;
        clsDProyeccionProgramacion clsDProyeccionProgramacion = null;
        string[] liststring;
        // GET: ProyeccionProgramacion
        [Authorize]
        public ActionResult ProyeccionProgramacion()
        {
            try
            {
                clsDClasificador = new clsDClasificador();
                var ListLimpiezaPescado = clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodigoGrupoTipoLimpiezaPescado, EstadoRegistro = clsAtributos.EstadoRegistroActivo });

                ViewBag.TipoLimpieza = new SelectList(ListLimpiezaPescado, "codigo", "descripcion");

                var ListDestinoProduccion = clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodigoGrupoDestinoProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });

                ViewBag.Destino = new SelectList(ListDestinoProduccion, "codigo", "descripcion");
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                ViewBag.Proyeccion = clsDProyeccionProgramacion.ConsultarProyeccionProgramacion();
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult ProyeccionProgramacionPartial(string Lote,DateTime FechaProduccion,int Toneladas,string Destino, string TipoLimpieza,string Observacion)
        {
            try
            {
                PROYECCION_PROGRAMACION
                return PartialView();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [Authorize]
        public ActionResult ProyeccionProgramacionEditar()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}