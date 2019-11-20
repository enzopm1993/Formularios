using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;



namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsDGeneral
    {
       
        public List<spConsultaCargos> ConsultaCargos(string dsCodigo)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaCargos(dsCodigo).ToList();
            }
        }

        public List<sp_GrupoEnfermedades> ConsultaCodigosGrupoSubEnfermedad(string tipo, string Grupo, string SubGrupo)
        {
            
                //entities = new ASIS_PRODEntities();
                //return entities.spConsultaCodigosEnfermedad("0").ToList();
            using(ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                
                return db.sp_GrupoEnfermedades(tipo, Grupo, SubGrupo).ToList();
            }
            
        }
        public List<spConsultaArea> ConsultaAreas(string dsCodigo)
        {

            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaArea(dsCodigo).ToList();

            }
            
        }
        public List<spConsultaRecurso> ConsultaRecursos(string dsCodigo)
        {

            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaRecurso(dsCodigo).ToList();

            }

        }
        public List<spConsultaCargosXRecursoLinea> ConsultaCargosxRecursoyLinea(string dsCodigoRecurso, string dsLinea)
        {

            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaCargosXRecursoLinea(dsCodigoRecurso, dsLinea).ToList();

            }

        }
        public List<spConsultaLineaXRecursoyCentroCosto> ConsultaLineasxCCyRecurso(string dsCodigoCC,string dsCodigoRecurso)
        {

            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaLineaXRecursoyCentroCosto(dsCodigoRecurso, dsCodigoCC).ToList();

            }

        }
        public List<spConsultaLinea> ConsultaLineas(string dsCodigo)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaLinea(dsCodigo).ToList();
            }
        }
        public List<spConsultaCentroCostos> ConsultaCentroCostos()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaCentroCostos().ToList();
            }
        }
        public String ConsultarLineaUsuario(string Identificacion)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                string Linea = string.Empty;
                var Empleado =  entities.spConsutaEmpleados(Identificacion).FirstOrDefault();
                if (Empleado != null)
                {
                    var poLinea= entities.spConsultaLinea(Empleado.CODIGOLINEA).FirstOrDefault();
                    if (poLinea != null)
                    {
                        Linea = poLinea.Descripcion ?? "";
                    }
                }

                return Linea;
            }
        }

        public List<CLASIFICADOR> ConsultarEstadosSolicitudSelect()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var Estados = entities.ESTADO_SOLICITUD.Where(x=> x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).ToList();
                Estados.Add(new ESTADO_SOLICITUD { Estado = "000", Descripcion = "Todos" });
                var Lista = Estados.OrderBy(x => x.Estado).Select(x => new CLASIFICADOR {Codigo= x.Estado, Descripcion=x.Descripcion });
                
                return Lista.ToList();
            }
        }
        public Boolean ConsultarSiMarcoBiometrico(string cedula)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                DateTime FechaActual = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                var resultado = db.spConsultaUltimaMarcacionBiometrico(cedula).FirstOrDefault();
                if (resultado == null){
                    return false;
                }
                if (resultado.Marcacion.Value >= FechaActual)
                    return true;
                else
                    return false;
            }
        }

        public string EnvioCorreo(string destinatario, string asunto, string mensaje)
        {
            try
            {
                string Correo = System.Configuration.ConfigurationSettings.AppSettings["Correo"];
                string Clave = System.Configuration.ConfigurationSettings.AppSettings["CorreoClave"];


              //  clsDParametro clsDParametro = new clsDParametro();
              //  string text = "MENSAJE DEL SISTEMA\n\n"+ mensaje;

              //  AlternateView plainView =
              //      AlternateView.CreateAlternateViewFromString(text,
              //                              Encoding.UTF8,
              //                              MediaTypeNames.Text.Plain);


              //  string html = "<H3>MENSAJE DEL SISTEMA</H3>" + mensaje+
              //"<img src='cid:imagen' />";
              //  AlternateView htmlView =
              //  AlternateView.CreateAlternateViewFromString(html,
              //              Encoding.UTF8,
              //              MediaTypeNames.Text.Html);

              //  LinkedResource img = new LinkedResource(@"C:\Desarrollo\Asiservy.Automatizacion.Formularios\Asiservy.Automatizacion.Formularios\Content\images\asilogo.jpg",
              //          MediaTypeNames.Image.Jpeg);
              //  img.ContentId = "imagen";
              //  htmlView.LinkedResources.Add(img);
                


              //  MailMessage correo = new MailMessage(Correo, destinatario);
              //  correo.Subject = asunto;
              //  correo.AlternateViews.Add(htmlView);
              //  correo.AlternateViews.Add(plainView);
                


              //  SmtpClient servidor = new SmtpClient("smtp.office365.com", 587);
              //  NetworkCredential credenciales = new NetworkCredential(Correo, Clave);
              //  servidor.Credentials = credenciales;
              //  servidor.EnableSsl = true;
                //servidor.Send(correo);
                return "Correo Enviado con Éxito";
            }
            catch (Exception ex)
            {
               return ex.Message;
                
            }
        }


        public List<MANTENIMIENTO_PREPARACION> ConsultarMantenimientoPreparacion()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {               
                return entities.MANTENIMIENTO_PREPARACION.ToList();
            }
        }



    }
}