using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia
{
    public class clsDCuchillo
    {
        public List<CUCHILLO> ConsultarCuchillos(CUCHILLO filtros)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                IEnumerable<CUCHILLO> Cuchillos = entities.CUCHILLO;

                if (filtros.NumeroCuchillo > 0)
                {
                    Cuchillos = Cuchillos.Where(x => x.NumeroCuchillo == filtros.NumeroCuchillo);
                }

                if (!string.IsNullOrEmpty(filtros.ColorCuchillo))
                {
                    Cuchillos = Cuchillos.Where(x => x.ColorCuchillo == filtros.ColorCuchillo);
                }

                IEnumerable<CUCHILLO> Listado = (from c in Cuchillos
                                                 join color in entities.CLASIFICADOR on c.ColorCuchillo equals color.Codigo
                                                 where color.Grupo==clsAtributos.CodigoGrupoColorCuchillo
                                                 select new CUCHILLO {
                                                     ColorCuchillo = color.Descripcion,
                                                     EstadoRegistro =c.EstadoRegistro,
                                                     FechaIngresoLog=c.FechaIngresoLog,
                                                     FechaModificacionLog=c.FechaModificacionLog,
                                                     NumeroCuchillo=c.NumeroCuchillo,
                                                     TerminalIngresoLog = c.TerminalIngresoLog,
                                                     TerminalModificacionLog = c.TerminalModificacionLog,
                                                     UsuarioIngresoLog=c.UsuarioIngresoLog,
                                                     UsuarioModificacionLog=c.UsuarioModificacionLog
                                                 }
                                                 );

              


                return Listado.ToList();
            }

        }

        public  string GuardarModificarCuchillo(CUCHILLO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {

              var Listado = entities.CUCHILLO.FirstOrDefault(x=> x.NumeroCuchillo == model.NumeroCuchillo && x.ColorCuchillo == model.ColorCuchillo);
                if (Listado != null)
                {
                    Listado.EstadoRegistro = model.EstadoRegistro;
                    Listado.FechaModificacionLog = model.FechaIngresoLog;
                    Listado.TerminalModificacionLog = model.TerminalIngresoLog;
                    Listado.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    
                }
                else
                {
                    entities.CUCHILLO.Add(model);
                }
                entities.SaveChanges();

             return clsAtributos.MsjRegistroGuardado;
            }

        }
    }
}