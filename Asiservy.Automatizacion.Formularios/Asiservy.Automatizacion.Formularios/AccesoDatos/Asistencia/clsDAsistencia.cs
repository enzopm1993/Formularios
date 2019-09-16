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
        
        public ControlDeAsistenciaViewModel ObtenerAsistenciaDiaria(string cedula)
        {

            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                spConsutaEmpleados BuscarControlador = null;
                List<ASISTENCIA> ControlAsistencia = null;
                List<sp_ConsultaAsistenciaDiaria> pListAsistencia = null;
                // var a = db.sp_ConsultaAsistenciaDiaria("01");
                ControlDeAsistenciaViewModel ControlAsistenciaViewModel = null;
                BuscarControlador = db.spConsutaEmpleados(cedula).ToList().FirstOrDefault();

                //IEnumerable<ASISTENCIA> pListAsistencia= db.ASISTENCIA.AsEnumerable().Where(x => x.Fecha.Value.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"));
                //pListAsistencia = pListAsistencia.Where(x => x.Linea == BuscarControlador.CODIGOLINEA);
                pListAsistencia = db.sp_ConsultaAsistenciaDiaria(BuscarControlador.CODIGOLINEA).ToList();
                if (pListAsistencia.ToList().Count == 0)
                {
                    List<spConsutaEmpleadosFiltro> ListaEmpleados = db.spConsutaEmpleadosFiltro("0", BuscarControlador.CODIGOLINEA, "0").Where(x=>x.CEDULA!=cedula).ToList();
                    ControlAsistencia = new List<ASISTENCIA>();
                    foreach (var item in ListaEmpleados)
                    {
                        ControlAsistencia.Add(new ASISTENCIA { Cedula=item.CEDULA,Fecha=DateTime.Now,EstadoAsistencia=clsAtributos.EstadoFalta,Linea=item.CODIGOLINEA});

                    }
                    db.ASISTENCIA.AddRange(ControlAsistencia);
                    db.SaveChanges();
                    pListAsistencia= db.sp_ConsultaAsistenciaDiaria(BuscarControlador.CODIGOLINEA).ToList();
                    ControlAsistenciaViewModel = new ControlDeAsistenciaViewModel
                    {
                        //ControlAsistencia = ControlAsistencia.OrderBy(z=>z.Nombres).ToList()
                        ControlAsistencia = pListAsistencia.OrderBy(z => z.NOMBRES).ToList()
                    };
                    
                }
                else
                {
                    ControlAsistenciaViewModel = new ControlDeAsistenciaViewModel
                    {
                        ControlAsistencia = pListAsistencia.OrderBy(x => x.NOMBRES).ToList()
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