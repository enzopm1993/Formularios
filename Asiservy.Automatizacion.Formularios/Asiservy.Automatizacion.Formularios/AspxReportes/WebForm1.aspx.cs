using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.CondicionPersonal;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Asiservy.Automatizacion.Formularios.Views.AspxReportes
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        clsDCondicionPersonal clsDCondicionPersonal = new clsDCondicionPersonal();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {

            if (!IsPostBack)
            {
                //int accionistaId = int.Parse(Request.QueryString["AccionistaID"].ToString());
                //string accionista = Request.QueryString["Accionista"].ToString();
                //int AccionistaIndirectoID = int.Parse(Request.QueryString["AccionistaIndirectoID"].ToString());
                //string nombre = Request.QueryString["Nombre"].ToString() == string.Empty ? null : Request.QueryString["Nombre"].ToString();
                //char estado = char.Parse(Request.QueryString["Estado"].ToString());
                var dt = clsDCondicionPersonal.ConsultaCondicionPersonal(DateTime.Now);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/AspxReportesRdlc/Report1.rdlc");
                //ReportViewer1.PageCountMode = PageCountMode.Actual;
                //ReportViewer1.LocalReport.SetParameters(new ReportParameter("Fecha", DateTime.Now.ToShortDateString()));
                //ReportViewer1.LocalReport.SetParameters(new ReportParameter("Accionista", accionista));
                //ReportViewer1.LocalReport.SetParameters(new ReportParameter("Codigo", AccionistaIndirectoID.ToString()));
                //ReportViewer1.LocalReport.SetParameters(new ReportParameter("Nombre", nombre));
                //ReportViewer1.LocalReport.SetParameters(new ReportParameter("Estado", estado == 'O' ? "" : estado == 'A' ? "ACTIVO" : "INACTIVO"));


                ReportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource DsReporte = new ReportDataSource("DataSet1", dt);
                ReportViewer1.LocalReport.DataSources.Add(DsReporte);
                ReportViewer1.LocalReport.Refresh();
            }

        }
    }
}