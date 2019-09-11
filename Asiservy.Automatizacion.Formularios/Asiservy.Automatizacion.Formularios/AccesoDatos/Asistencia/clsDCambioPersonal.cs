using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Asistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia
{
    public class clsDCambioPersonal
    {
        public List<BitacoraCambioPersonalModelView>  ConsultarBitacoraCambioPersonal (BitacoraCambioPersonalModelView filtros)
        {
            using(ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<BitacoraCambioPersonalModelView> ListadoCambioPersonal = new List<BitacoraCambioPersonalModelView>();

                IEnumerable<BITACORA_CAMBIO_PERSONAL> Listado = entities.BITACORA_CAMBIO_PERSONAL;
                if (!string.IsNullOrEmpty(filtros.CodLinea))
                {
                    Listado = Listado.Where(x=> x.CodLinea== filtros.CodLinea);
                }
                if (!string.IsNullOrEmpty(filtros.CodArea))
                {
                    Listado = Listado.Where(x => x.CodArea == filtros.CodArea);
                }
                if (!string.IsNullOrEmpty(filtros.Cedula))
                {
                    Listado = Listado.Where(x => x.Cedula == filtros.Cedula);
                }

                var fechaHasta = filtros.FechaHasta.Date.AddDays(1);
                 Listado = Listado.Where(x => x.FechaIngresoLog.Value.Date >= filtros.FechaDesde.Date);
                 Listado = Listado.Where(x => x.FechaIngresoLog.Value.Date <= fechaHasta.Date);


                foreach(var x in Listado.ToList())
                {
                    var Linea = entities.spConsultaLinea(x.CodLinea).FirstOrDefault();
                    var Area = entities.spConsultaArea(x.CodArea).FirstOrDefault();
                    //var Cargo = entities.spConsultaCargos(x.Cedula).FirstOrDefault();

                    ListadoCambioPersonal.Add(new BitacoraCambioPersonalModelView
                    {
                        Cedula = x.Cedula,
                        //CodCargo = x.CodCargo,
                        //Cargo = Cargo.Descripcion,
                        Area = Area!=null? Area.Descripcion:"",
                        CodArea = x.CodArea,
                        CodLinea = x.CodLinea,
                        Linea = Linea != null ? Linea.Descripcion:"",
                        FechaIngresoLog = x.FechaIngresoLog,
                        Tipo = x.Tipo,
                        IdBitacoraCambioPersonal = x.IdBitacoraCambioPersonal,
                        TerminalIngresoLog = x.TerminalIngresoLog,
                        UsuarioIngresoLog = x.UsuarioIngresoLog
                    });
                }

                //ListadoCambioPersonal = Listado.Select(x => new BitacoraCambioPersonalModelView
                //{
                //    Cedula=x.Cedula,
                //    CodCargo=x.CodCargo,
                //    CodArea=x.CodArea,
                //    CodLinea=x.CodLinea,
                //    FechaIngresoLog =x.FechaIngresoLog,
                //    Tipo=x.Tipo,
                //    IdBitacoraCambioPersonal = x.IdBitacoraCambioPersonal,
                //    TerminalIngresoLog=x.TerminalIngresoLog,
                //    UsuarioIngresoLog=x.UsuarioIngresoLog
                //}).ToList();

                return ListadoCambioPersonal;
            }

        }

    }
}