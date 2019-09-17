using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

using Asiservy.Automatizacion.Formularios.Models.Asistencia;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia
{
    public class clsDAsistencia
    {
        List<sp_ConsultaAsistenciaDiaria> pListAsistencia = null;
        spConsutaEmpleados BuscarControlador = null;
        public int ConsultarExistenciaAsistencia(string cedula)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                BuscarControlador = db.spConsutaEmpleados(cedula).ToList().FirstOrDefault();
                pListAsistencia = db.sp_ConsultaAsistenciaDiaria(BuscarControlador.CODIGOLINEA+"",1).ToList();
            }
            if (pListAsistencia.ToList().Count == 0)
                return 0;
            else
                return 1;
        }
        public ControlDeAsistenciaViewModel ObtenerAsistenciaDiaria(string CodLinea, int BanderaExiste)
        {
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                
                List<ASISTENCIA> ControlAsistencia = null;
                
                ControlDeAsistenciaViewModel ControlAsistenciaViewModel = null;
                if (BanderaExiste == 0)
                {
                    List<spConsutaEmpleadosFiltro> ListaEmpleados = db.spConsutaEmpleadosFiltro("0", CodLinea, "0").Where(x => x.CODIGOCARGO != "221").ToList();
                    ControlAsistencia = new List<ASISTENCIA>();
                    foreach (var item in ListaEmpleados)
                    {
                        ControlAsistencia.Add(new ASISTENCIA { Cedula = item.CEDULA, Fecha = DateTime.Now, EstadoAsistencia = clsAtributos.EstadoFalta, Linea = item.CODIGOLINEA, Turno="1" });
                    }
                    db.ASISTENCIA.AddRange(ControlAsistencia);
                    db.SaveChanges();
                    pListAsistencia = db.sp_ConsultaAsistenciaDiaria(CodLinea).ToList();
                    pListAsistencia.ForEach(x => x.Hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")));
                    ControlAsistenciaViewModel = new ControlDeAsistenciaViewModel
                    {
                        ControlAsistencia = pListAsistencia.OrderBy(z => z.NOMBRES).ToList()
                    };
                }
                else
                {
                    pListAsistencia = db.sp_ConsultaAsistenciaDiaria(CodLinea,1).ToList();
                    pListAsistencia.ForEach(x => x.Hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")));
                    ControlAsistenciaViewModel = new ControlDeAsistenciaViewModel
                    {
                        ControlAsistencia = pListAsistencia.OrderBy(z => z.NOMBRES).ToList()
                    };
                }
                    
               
                return ControlAsistenciaViewModel;
            }
        }
        public string ActualizarAsistencia(ASISTENCIA psAsistencia)
        {
            using(ASIS_PRODEntities db =new  ASIS_PRODEntities())
            {
                DateTime Fechainicio = DateTime.Now.AddDays(-1);
                DateTime FechaFin = DateTime.Now.AddDays(1);
                var BuscarEnAsistencia = db.ASISTENCIA.Where(x => x.Cedula == psAsistencia.Cedula && (x.Fecha > Fechainicio && x.Fecha < FechaFin)).FirstOrDefault();
                BuscarEnAsistencia.EstadoAsistencia = psAsistencia.EstadoAsistencia;
                BuscarEnAsistencia.Observacion = psAsistencia.Observacion;
                db.SaveChanges();
                return "Registro actualizado con éxito";
            }
            
        }
    }
}