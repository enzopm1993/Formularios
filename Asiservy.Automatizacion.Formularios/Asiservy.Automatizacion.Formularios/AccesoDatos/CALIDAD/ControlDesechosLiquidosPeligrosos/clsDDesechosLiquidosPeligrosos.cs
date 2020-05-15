using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ControlDesechosLiquidosPeligrosos
{
    public class clsDDesechosLiquidosPeligrosos
    {
        public List<sp_Control_Desechos_Liquidos_Peligrosos> ConsultarDesechosLiquidos(int anioBusqueda, int mesBusqueda, int idDesechosLiquidos, int op)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.sp_Control_Desechos_Liquidos_Peligrosos(anioBusqueda, mesBusqueda, idDesechosLiquidos, op).ToList();
                return lista;
            }
        }

        public List<CC_DESECHOS_LIQUIDOS_PELIGROSOS> ConsultarDesechosLiquidosCabecera(int anioBusqueda, bool estadoReporte)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_DESECHOS_LIQUIDOS_PELIGROSOS.Where(x => x.FechaMES.Year == anioBusqueda && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo && x.EstadoReporte == estadoReporte).ToList();
                List<CC_DESECHOS_LIQUIDOS_PELIGROSOS> listaCabecera = new List<CC_DESECHOS_LIQUIDOS_PELIGROSOS>();
                CC_DESECHOS_LIQUIDOS_PELIGROSOS itemCabecera;
                foreach (var item in lista)
                {// SI NO HAGO ESTO ME DA UN ERROR DE QUE LA CONEXION SE PERDIO A PESAR QUE SI ME TRAIA DATOS
                    itemCabecera = new CC_DESECHOS_LIQUIDOS_PELIGROSOS();
                    itemCabecera.FechaMES = item.FechaMES;
                    itemCabecera.EstadoReporte = item.EstadoReporte;
                    itemCabecera.FechaIngresoLog = item.FechaIngresoLog;
                    itemCabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    itemCabecera.IdDesechosLiquidos = item.IdDesechosLiquidos;
                    listaCabecera.Add(itemCabecera);
                }
                return listaCabecera;
            }
        }

        public int GuardarModificarDesechosLiquidos(CC_DESECHOS_LIQUIDOS_PELIGROSOS guardarmodificar, int siAprobar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_DESECHOS_LIQUIDOS_PELIGROSOS.FirstOrDefault(x => x.IdDesechosLiquidos == guardarmodificar.IdDesechosLiquidos && x.EstadoRegistro == guardarmodificar.EstadoRegistro);
                if (model != null)
                {
                    if (siAprobar == 0)
                    {
                        model.FechaMES = guardarmodificar.FechaMES;
                    }
                    else if (siAprobar == 1)
                    {
                        model.EstadoReporte = guardarmodificar.EstadoReporte;
                        model.AprobadoPor = guardarmodificar.UsuarioIngresoLog;
                        model.FechaAprobacion = guardarmodificar.FechaAprobacion;
                    }
                    model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_DESECHOS_LIQUIDOS_PELIGROSOS.Add(guardarmodificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarDesechosLiquidos(CC_DESECHOS_LIQUIDOS_PELIGROSOS registroEliminar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_DESECHOS_LIQUIDOS_PELIGROSOS.FirstOrDefault(x => x.IdDesechosLiquidos == registroEliminar.IdDesechosLiquidos);
                if (model != null)
                {
                    model.EstadoRegistro = registroEliminar.EstadoRegistro;
                    model.FechaModificacionLog = registroEliminar.FechaIngresoLog;
                    model.TerminalModificacionLog = registroEliminar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = registroEliminar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }
                return valor;
            }
        }

        public int GuardarModificarDesechosLiquidosDetalle(CC_DESECHOS_LIQUIDOS_PELIGROSOS_DETALLE guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_DESECHOS_LIQUIDOS_PELIGROSOS_DETALLE.FirstOrDefault(x => x.IdDesechosLiquidosDetalle == guardarmodificar.IdDesechosLiquidosDetalle && x.EstadoRegistro == guardarmodificar.EstadoRegistro);
                if (model != null)
                {

                    model.FechaDIA = guardarmodificar.FechaDIA;
                    model.Laboratorio = guardarmodificar.Laboratorio;
                    model.Otros = guardarmodificar.Otros;
                    model.Observaciones = guardarmodificar.Observaciones;
                    model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_DESECHOS_LIQUIDOS_PELIGROSOS_DETALLE.Add(guardarmodificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarDesechosLiquidosDetalle(CC_DESECHOS_LIQUIDOS_PELIGROSOS_DETALLE registroEliminar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_DESECHOS_LIQUIDOS_PELIGROSOS_DETALLE.FirstOrDefault(x => x.IdDesechosLiquidosDetalle == registroEliminar.IdDesechosLiquidosDetalle);
                if (model != null)
                {
                    model.EstadoRegistro = registroEliminar.EstadoRegistro;
                    model.FechaModificacionLog = registroEliminar.FechaIngresoLog;
                    model.TerminalModificacionLog = registroEliminar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = registroEliminar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }
                return valor;
            }
        }

        public List<CC_DESECHOS_LIQUIDOS_PELIGROSOS> ConsultarReporteCabecera(int anioBusqueda)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = (from c in db.CC_DESECHOS_LIQUIDOS_PELIGROSOS
                             where (c.FechaMES.Year == anioBusqueda && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                             orderby c.FechaMES descending
                             select new { c.IdDesechosLiquidos, c.FechaMES, c.EstadoReporte, c.FechaIngresoLog, c.UsuarioIngresoLog, c.FechaAprobacion, c.AprobadoPor }).ToList();
                List<CC_DESECHOS_LIQUIDOS_PELIGROSOS> listacabecera = new List<CC_DESECHOS_LIQUIDOS_PELIGROSOS>();
                CC_DESECHOS_LIQUIDOS_PELIGROSOS cabecera;
                foreach (var item in lista)
                {
                    cabecera = new CC_DESECHOS_LIQUIDOS_PELIGROSOS();
                    cabecera.IdDesechosLiquidos = item.IdDesechosLiquidos;
                    cabecera.FechaMES = item.FechaMES;
                    cabecera.EstadoReporte = item.EstadoReporte;
                    cabecera.FechaIngresoLog = item.FechaIngresoLog;
                    cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    cabecera.FechaAprobacion = item.FechaAprobacion;
                    cabecera.AprobadoPor = item.AprobadoPor;
                    listacabecera.Add(cabecera);
                }
                return listacabecera;
            }
        }
    }
}
    