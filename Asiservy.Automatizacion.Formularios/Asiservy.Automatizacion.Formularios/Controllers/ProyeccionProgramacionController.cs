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

                throw ex;
            }
        }

        [HttpPost]
        public ActionResult ProyeccionProgramacionPartial(int IdProyeccionProgramacion,string Lote,DateTime? FechaProduccion,int? Toneladas,string Destino, string TipoLimpieza,string Observacion/*,string Lineas, TimeSpan HoraInicio, TimeSpan HoraFin*/)
        {
            try
            {
                PROYECCION_PROGRAMACION ProyeccionProgramacion = null;
                //if (string.IsNullOrEmpty(Lineas))
                //{
                   ProyeccionProgramacion = new PROYECCION_PROGRAMACION()
                    {

                        IdProyeccionProgramacion = IdProyeccionProgramacion,
                        Lote = Lote,
                        FechaProduccion = FechaProduccion,
                        Toneladas = Toneladas,
                        Destino = Destino,
                        TipoLimpieza = TipoLimpieza,
                        Observacion = Observacion

                    };
                //}
                //else
                //{
                //    ProyeccionProgramacion = new PROYECCION_PROGRAMACION()
                //    {

                //        IdProyeccionProgramacion = IdProyeccionProgramacion,
                //        Lineas=Lineas,
                //        HoraInicio=HoraInicio,
                //        HoraFin=HoraFin

                //    };
                //}
               
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                var Respuesta = clsDProyeccionProgramacion.GuardarActualizarProyeccionProgramacion(ProyeccionProgramacion);
                return PartialView(Respuesta);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        public ActionResult ProyeccionProgramacionEditarPartial(int IdProyeccionProgramacion,string Lineas, TimeSpan HoraInicio, TimeSpan HoraFin)
        {
            try
            {
                PROYECCION_PROGRAMACION ProyeccionProgramacion = null;
                ProyeccionProgramacion = new PROYECCION_PROGRAMACION()
                    {

                        IdProyeccionProgramacion = IdProyeccionProgramacion,
                        Lineas = Lineas,
                        HoraInicio = HoraInicio,
                        HoraFin = HoraFin

                    };
               
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                var Respuesta = clsDProyeccionProgramacion.GuardarActualizarProyeccionProgramacion(ProyeccionProgramacion);
                return PartialView(Respuesta);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [Authorize]
        public ActionResult ProyeccionProgramacionEditar()
        {
            try
            {
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                ViewBag.Proyeccion = clsDProyeccionProgramacion.ConsultarProyeccionProgramacion();
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult ModalEditarProyeccion(int IdProyeccion)
        {
            try
            {
                clsDClasificador = new clsDClasificador();
                var ListLineas = clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                ViewBag.IdProyeccion = IdProyeccion;
                ViewBag.Lineas = ListLineas;
                return PartialView();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}