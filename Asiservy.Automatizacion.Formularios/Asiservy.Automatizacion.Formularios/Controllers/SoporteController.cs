using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class SoporteController : Controller
    {
        // GET: Soporte
        public ActionResult Reporte()
        {
            ViewBag.dataTableJS = "1";
            ViewBag.Apexcharts = "1";
            ViewBag.DateRangePicker = "1";
            ViewBag.Pivot = "1";
            ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
            return View();
        }
        [HttpGet]
        public JsonResult ObtenerSoportesReporte(string fechaIni, string fechaFin)
        {
            try
            {
                var client = new RestClient("http://192.168.0.4/");

                string URL = "/osticket/scp/report.php?tipo=todos&fechaini=" + fechaIni + "&fechafin="+ fechaFin;
                var request = new RestRequest(URL, Method.GET);
                IRestResponse response = client.Execute(request);
                var content = response.Content;
                List<ItemTicket> dataView = JsonConvert.DeserializeObject<List<ItemTicket>>(content);
                
                ReturnVista envioVista = new ReturnVista();


                TkTotales tkTotales = new TkTotales();
                tkTotales.Totales = dataView.Count();
                tkTotales.Abiertos = dataView.Where(c => c.Estado == "Abierto").Count();
                tkTotales.Cerrados = dataView.Where(c => c.Estado == "Cerrado").Count();
                tkTotales.SinAsignar = dataView.Where(c => string.IsNullOrEmpty(c.AgenteAsignado)).Count();


                var DepColores = dataView
                    .GroupBy(x => new { x.Departamento, x.Color })
                    .Select(g => new ClsKeyValue
                    {
                        Codigo = g.Key.Departamento,
                        Descripcion = g.Key.Color

                    }).ToList();

                var resultTicketsPorDep = dataView
                    .GroupBy(x => x.Departamento)
                    .Select(g => new ClsKpiDescripcionTotal
                    {
                        Descripcion = g.Key,
                        Total = g.Count()

                    }).OrderByDescending(c => c.Total).ToList();

                envioVista.DataPlana = dataView;
                envioVista.Totales = tkTotales;
                envioVista.TotalTicketsPorDep = resultTicketsPorDep;
                envioVista.DepartamentoColores = DepColores;
                JsonResult result = Json(envioVista, JsonRequestBehavior.AllowGet);

                result.MaxJsonLength = 50000000;
                return result;
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ObtenerSoportesReporteCerrados(string fechaIni, string fechaFin)
        {
            try
            {
                var client = new RestClient("http://192.168.0.4/");

                string URL = "/osticket/scp/report.php?tipo=cerrados&fechaini=" + fechaIni + "&fechafin=" + fechaFin;
                var request = new RestRequest(URL, Method.GET);
                IRestResponse response = client.Execute(request);
                var content = response.Content;
                List<ItemTicket> dataView = JsonConvert.DeserializeObject<List<ItemTicket>>(content);

                ReturnVista envioVista = new ReturnVista();


                TkTotales tkTotales = new TkTotales();
                tkTotales.Totales = dataView.Count();

                var DepColores = dataView
                    .GroupBy(x => new { x.Departamento, x.Color })
                    .Select(g => new ClsKeyValue
                    {
                        Codigo = g.Key.Departamento,
                        Descripcion = g.Key.Color

                    }).ToList();

                var resultTicketsPorDep = dataView
                    .GroupBy(x => x.Departamento)
                    .Select(g => new ClsKpiDescripcionTotal
                    {
                        Descripcion = g.Key,
                        Total = g.Count()

                    }).OrderByDescending(c => c.Total).ToList();

                envioVista.DataPlana = dataView;
                envioVista.Totales = tkTotales;
                envioVista.TotalTicketsPorDep = resultTicketsPorDep;
                envioVista.DepartamentoColores = DepColores;
                JsonResult result = Json(envioVista, JsonRequestBehavior.AllowGet);

                result.MaxJsonLength = 50000000;
                return result;
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

    }



    public class ReturnVista
    {
        public bool Estado { get; set; }
        public string Mensaje { get; set; }
        public List<ItemTicket> DataPlana { get; set; }
        public TkTotales Totales { get; set; }
        public List<ClsKpiDescripcionTotal> TotalTicketsPorDep { get; set; }
        public List<ClsKeyValue> DepartamentoColores { get; set; }
    }

    public class TkTotales
    {
        public int Totales { get; set; }
        public int Abiertos { get; set; }
        public int Cerrados { get; set; }
        public int SinAsignar { get; set; }
    }

    public class ItemTicket
    {
        public int ID { get; set; }
        public string Departamento { get; set; }
        public string Usuario { get; set; }
        public string Ticket { get; set; }
        public string Asunto { get; set; }
        public string Estado { get; set; }
        public string AgenteAsignado { get; set; }
        public string FechaCreacion { get; set; }
        public string HoraCreacion { get; set; }
        public string FechaAsignacion { get; set; }
        public string HoraAsignacion { get; set; }
        public string FechaCierre { get; set; }
        public string HoraCierre { get; set; }
        public string FechaInicioSoporte { get; set; }
        public string HoraInicioSoporte { get; set; }
        public string FechaFinSoporte { get; set; }
        public string HoraFinSoporte { get; set; }
        public string TiempoTicket { get; set; }
        public string TiempoAsignacionCierre { get; set; }
        public string TiempoSoporte { get; set; }
        public string TiempoCreacionFinSoporte { get; set; }
        public string TiempoTicketLetras { get; set; }
        public int SoporteMinutos { get; set; }
        public decimal SoporteHoras { get; set; }
        public decimal SoporteDias { get; set; }
        public int TicketMinutos { get; set; }
        public decimal TicketHoras { get; set; }
        public decimal TicketDias { get; set; }
        public int AsignacionCierreMinutos { get; set; }
        public decimal AsignacionCierreHoras { get; set; }
        public decimal AsignacionCierreDias { get; set; }
        public int CreacionFinSoporteMinutos { get; set; }
        public decimal CreacionFinSoporteHoras { get; set; }
        public decimal CreacionFinSoporteDias { get; set; }
        public string Color { get; set; }
    }
    
}