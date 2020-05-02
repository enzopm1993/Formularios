using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers.PRODUCCION
{
    [Authorize]
    public class PesajeController : Controller
    {
        // GET: Pesaje
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AgregarPeso()
        {
            
            ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

            return View();
        }

        [HttpPost]
        public ActionResult CrearDocumentoPesaje(ParamPesajeCab parametros)
        {
            
            ClsReturn resp = new ClsReturn();
            resp.codigo = 0;
            try
            {
                if (string.IsNullOrEmpty(parametros.LIMPIEZA))
                {
                    resp.Mensaje = "Seleccione un tipo de limpieza";
                }
                else
                {
                    if (string.IsNullOrEmpty(parametros.LOTE))
                    {
                        resp.Mensaje = "Seleccione un lote";
                    }
                    else
                    {
                        if (parametros.ORDEN == 0)
                        {
                            resp.Mensaje = "Seleccione una orden de fabricación";
                        }
                        else
                        {
                            if (parametros.TURNO == 0)
                            {
                                resp.Mensaje = "Seleccione un turno";
                            }
                            else
                            {
                                AccesoDatos.clsDEmpleado clsDEmpleado = new AccesoDatos.clsDEmpleado();


                                using (ASIS_PRODEntities db = new ASIS_PRODEntities())
                                {
                                    PRY_PESAJE objPesaje = new PRY_PESAJE();
                                    objPesaje.FECHA_INGRESO = DateTime.Now;
                                    objPesaje.USUARIO_INGRESO = User.Identity.Name.Split('_')[0];
                                    objPesaje.LINEA = clsDEmpleado.ConsultaEmpleado(User.Identity.Name.Split('_')[1]).FirstOrDefault().CODIGOLINEA;
                                    objPesaje.LIMPIEZA = parametros.LIMPIEZA;
                                    objPesaje.ORDEN_FABRICACION = parametros.ORDEN;
                                    objPesaje.TURNO = parametros.TURNO;
                                    objPesaje.LOTE = parametros.LOTE;

                                    db.PRY_PESAJE.Add(objPesaje);
                                    db.SaveChanges();
                                    resp.codigo = objPesaje.ID;
                                }
                            }
                        }
                    }
                }

                
            }
            catch (Exception ex)
            {
                resp.codigo = 0;
                resp.Mensaje = ex.Message;
            }
           

            return Json(resp);
        }

        [HttpPost]
        public ActionResult IngresarPeso(ParamPesajeDet parametros)
        {

            ClsReturn resp = new ClsReturn();
            try
            {
                resp.codigo = 0;
                if (parametros.PESO_BRUTO == 0)
                {
                    resp.Mensaje = "Debe ingresar un peso";
                }
                else
                {
                    if ( string.IsNullOrEmpty( parametros.TIPO_TARA))
                    {
                        resp.Mensaje = "Debe seleccionar un tipo de bandeja o este no está asignando un peso de tara";
                    }
                    else
                    {
                        if (parametros.ID_CABECERA == 0)
                        {
                            resp.Mensaje = "No existe una relación con un documento cabecera";
                        }
                        else{
                            AccesoDatos.clsDEmpleado clsDEmpleado = new AccesoDatos.clsDEmpleado();
                            decimal PESO_TARA = Convert.ToDecimal( parametros.TIPO_TARA.Split('|')[1]);

                            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
                            {
                                PRY_PESAJE_DETALLE objPesaje = new PRY_PESAJE_DETALLE();
                                objPesaje.FECHA_INGRESO = DateTime.Now;
                                objPesaje.USUARIO_INGRESO = User.Identity.Name.Split('_')[0];
                                objPesaje.ID_PESAJE = parametros.ID_CABECERA;
                                objPesaje.PESO_BRUTO = parametros.PESO_BRUTO;
                                objPesaje.PESO_TARA = PESO_TARA;
                                objPesaje.TIPO_TARA = parametros.TIPO_TARA.Split('|')[0];
                                objPesaje.PESO_NETO = parametros.PESO_BRUTO - PESO_TARA;
                                objPesaje.ESTADO_REGISTRO = true;
                                db.PRY_PESAJE_DETALLE.Add(objPesaje);
                                db.SaveChanges();
                                resp.codigo = objPesaje.ID;


                                RetornoInsertPeso retorno = new RetornoInsertPeso();
                                var results = db.PRY_PESAJE_DETALLE.Where(c => c.ID_PESAJE == parametros.ID_CABECERA && c.ESTADO_REGISTRO)
                                  .GroupBy(x => x.ID_PESAJE)
                                  .Select(g => new
                                  {
                                      Cabecera = g.Key,
                                      Peso = g.Sum(x => x.PESO_NETO),
                                      Registros = g.Count()
                                  }).ToList();
                                retorno.Totales = results;
                                retorno.Ultimos = db.PRY_PESAJE_DETALLE.Where(c => c.ID_PESAJE == parametros.ID_CABECERA && c.ESTADO_REGISTRO).OrderByDescending(c => c.FECHA_INGRESO).Take(5).ToList();

                                resp.Lista = retorno;
                            }
                        }
                    }
                }

              
            }
            catch (Exception ex)
            {
                resp.codigo = 0;
                resp.Mensaje = ex.Message;
            }


            return Json(resp);
        }
        [HttpGet]
        public ActionResult Eliminar(int idDetalle)
        {

            ClsReturn resp = new ClsReturn();
            try
            {
                resp.codigo = 0;
                if (idDetalle == 0)
                {
                    resp.Mensaje = "Debe ingresar seleccionar un registro";
                }
                else
                {
                    AccesoDatos.clsDEmpleado clsDEmpleado = new AccesoDatos.clsDEmpleado();


                    using (ASIS_PRODEntities db = new ASIS_PRODEntities())
                    {
                        var objPesaje = db.PRY_PESAJE_DETALLE.Find(idDetalle);

                        objPesaje.FECHA_INGRESO = DateTime.Now;
                        objPesaje.USUARIO_INGRESO = User.Identity.Name.Split('_')[0];
                        objPesaje.ESTADO_REGISTRO = false;
                        db.SaveChanges();
                        resp.codigo = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                resp.codigo = 0;
                resp.Mensaje = ex.Message;
            }
            
            return Json(resp, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ObtenerUltimosPesos(int idCabecera, int cantidad)
        {
            
            ClsReturn resp = new ClsReturn();
            try
            {
                resp.codigo = 0;
                if (idCabecera == 0)
                {
                    resp.Mensaje = "No existe una relación con un documento cabecera";
                }
                else
                {
                    AccesoDatos.clsDEmpleado clsDEmpleado = new AccesoDatos.clsDEmpleado();


                    using (ASIS_PRODEntities db = new ASIS_PRODEntities())
                    {
                        List<PRY_PESAJE_DETALLE> resultsPesos;
                        if (cantidad == -1)
                        {
                            resultsPesos = db.PRY_PESAJE_DETALLE.Where(c => c.ID_PESAJE == idCabecera && c.ESTADO_REGISTRO).OrderByDescending(c => c.FECHA_INGRESO).ToList();
                        }
                        else
                        {
                            resultsPesos = db.PRY_PESAJE_DETALLE.Where(c => c.ID_PESAJE == idCabecera && c.ESTADO_REGISTRO).OrderByDescending(c => c.FECHA_INGRESO).Take(cantidad).ToList();
                        }
                        resp.codigo = 1;
                        resp.Lista = resultsPesos;


                    }
                }
            }
            catch (Exception ex)
            {
                resp.codigo = 0;
                resp.Mensaje = ex.Message;
                resp.Lista = null;
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerTotalesCabecera(int idCabecera)
        {

            ClsReturn resp = new ClsReturn();
            try
            {
                resp.codigo = 0;
                if (idCabecera == 0)
                {
                    resp.Mensaje = "No existe una relación con un documento cabecera";
                }
                else
                {
                    AccesoDatos.clsDEmpleado clsDEmpleado = new AccesoDatos.clsDEmpleado();


                    using (ASIS_PRODEntities db = new ASIS_PRODEntities())
                    {

                        var results = db.PRY_PESAJE_DETALLE.Where(c => c.ID_PESAJE == idCabecera && c.ESTADO_REGISTRO)
                           .GroupBy(x => x.ID_PESAJE)
                           .Select(g => new 
                           {
                               Cabecera = g.Key,
                               Peso = g.Sum(x=> x.PESO_NETO),
                               Registros = g.Count()
                           }).ToList();
                        resp.codigo = 1;
                        resp.Lista = results;


                    }
                }
            }
            catch (Exception ex)
            {
                resp.codigo = 0;
                resp.Mensaje = ex.Message;
                resp.Lista = null;
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerDatosCabecera(int idCabecera)
        {

            ClsReturn resp = new ClsReturn();
            try
            {
                resp.codigo = 0;
                if (idCabecera == 0)
                {
                    resp.Mensaje = "No existe una relación con el documento cabecera";
                }
                else
                {
                    AccesoDatos.clsDEmpleado clsDEmpleado = new AccesoDatos.clsDEmpleado();


                    using (ASIS_PRODEntities db = new ASIS_PRODEntities())
                    {
                         resp.codigo = 1;

                        RetornoDatosCabecera retorno = new RetornoDatosCabecera();
                        retorno.Datos = db.PRY_PESAJE.Find(idCabecera);
                        retorno.Totales = db.PRY_PESAJE_DETALLE.Where(c => c.ID_PESAJE == idCabecera && c.ESTADO_REGISTRO)
                                 .GroupBy(x => x.ID_PESAJE)
                                 .Select(g => new
                                 {
                                     Cabecera = g.Key,
                                     Peso = g.Sum(x => x.PESO_NETO),
                                     Registros = g.Count()
                                 }).ToList();
                        retorno.Ultimos = db.PRY_PESAJE_DETALLE.Where(c => c.ID_PESAJE == idCabecera && c.ESTADO_REGISTRO).OrderByDescending(c => c.FECHA_INGRESO).Take(5).ToList();

                        resp.Lista = retorno;

                    }
                }
            }
            catch (Exception ex)
            {
                resp.codigo = 0;
                resp.Mensaje = ex.Message;
                resp.Lista = null;
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ObtieneTaras()
        {

            ClsReturn resp = new ClsReturn();
            try
            {
                resp.codigo = 0;

                using (ASIS_PRODEntities db = new ASIS_PRODEntities())
                {
                    var results = db.CLASIFICADOR.Where(c=> c.Grupo == "038" && c.Codigo != "0").ToList();
                    List<ClsKeyValue> clsKeyValue = new List<ClsKeyValue>();
                    foreach (CLASIFICADOR item in results)
                    {
                        clsKeyValue.Add(new ClsKeyValue { Codigo = item.Codigo + "|" + item.codigoExtra , Descripcion = item.Descripcion});
                    }
                    resp.codigo = 1;
                    resp.Lista = clsKeyValue;

                }
            }
            catch (Exception ex)
            {
                resp.codigo = 0;
                resp.Mensaje = ex.Message;
                resp.Lista = null;
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }
    }
    public class ClsReturn
    {
        public int codigo { get; set; }
        public string Mensaje { get; set; }
        public object Lista { get; set; }
    }
    public class ParamPesajeCab
    {
        public string LIMPIEZA { get; set; }
        public string LOTE { get; set; }
        public int ORDEN { get; set; }
        public int TURNO { get; set; }
    }

    public class ParamPesajeDet
    {
        public int ID_CABECERA { get; set; }
        public decimal PESO_BRUTO { get; set; }
        public string TIPO_TARA { get; set; }
    }
    public class RetornoInsertPeso
    {
        public object Totales { get; set; }
        public object Ultimos { get; set; }
    }

    public class RetornoDatosCabecera
    {
        public object Datos { get; set; }
        public object Totales { get; set; }
        public object Ultimos { get; set; }
    }
}