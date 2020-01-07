using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.MoverPersonal
{
    public class ClsDMoverPersonal
    {
        public string MoverPersonal(List<string> Cedula, string CentroCostos, string Recurso, string Linea, string Cargo, string psterminal, string psUsuario)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                //var EmpleadoAreaOrigen = db.spConsultaEspecificaEmpleadosxCedula(Cedula).ToList();
                spConsultaEspecificaEmpleadosxCedula EmpleadoAreaOrigen = null;
                List<MOVER_PERSONAL> poMoverPersonal = new List<MOVER_PERSONAL>();
                foreach (var item in Cedula)
                {
                    EmpleadoAreaOrigen = db.spConsultaEspecificaEmpleadosxCedula(item).FirstOrDefault();
                    poMoverPersonal.Add(new MOVER_PERSONAL
                    {
                        Cargo = Cargo,
                        CargoOrigen = EmpleadoAreaOrigen.CARGO,
                        Cedula = item,
                        CentroCosto = CentroCostos,
                        CentroCostoOrigen = EmpleadoAreaOrigen.CENTRO_COSTOS,
                        EstadoAprobacion = clsAtributos.EstadoPendienteMoverPersonalN,
                        EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                        FechaCreacionLog = DateTime.Now,
                        Linea = Linea,
                        LineaOrigen = EmpleadoAreaOrigen.LINEA,
                        Recurso = Recurso,
                        RecursoOrigen = EmpleadoAreaOrigen.RECURSO,
                        TerminalCreacionLog = psterminal,
                        UsuarioCreacionLog = psUsuario
                    });
                }
                db.MOVER_PERSONAL.AddRange(poMoverPersonal);
                db.SaveChanges();
                return "Registros ingresados correctamente";
            }
        }
        public List<spConsultarMovimientoPersonalEnNominaPendiente> ConsultarMoverPersonalEnNominaPendiente()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultarMovimientoPersonalEnNominaPendiente().ToList();
            }

        }
        public MOVER_PERSONAL ConsultarMoverPersonalPorId(int IdMoverPersonal)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                return db.MOVER_PERSONAL.Where(x => x.IdMoverPersonal == IdMoverPersonal).FirstOrDefault();
            }
        }
        public List<MOVER_PERSONAL> ConsultarMoverPersonalPorIdMas(int[] ArrayIdMoverPersonal)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                return db.MOVER_PERSONAL.Where(x => ArrayIdMoverPersonal.Contains(x.IdMoverPersonal)).ToList();
            }
        }
        public string ActualizarEstadoMoverPersonal(int IdMoverPersonal,string psusuario, string psterminal,string psEstadoAprobacion)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var MoverPersonal=db.MOVER_PERSONAL.Find(IdMoverPersonal);
                MoverPersonal.EstadoAprobacion = psEstadoAprobacion;
                MoverPersonal.UsuarioModificacionLog = psusuario;
                MoverPersonal.TerminalModificacionLog = psterminal;
                db.SaveChanges();
                return "RegistroActualizado con éxito";
            }
        }
        public string InactivaoMoverPersonal(int IdMoverPersonal, string psusuario, string psterminal)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var MoverPersonal = db.MOVER_PERSONAL.Find(IdMoverPersonal);
                MoverPersonal.UsuarioModificacionLog = psusuario;
                MoverPersonal.TerminalModificacionLog = psterminal;
                MoverPersonal.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                db.SaveChanges();
                return "Registro inactivado con éxito";
            }
        }
        public string GuardarBitacoraMovimientoPersonalNomina(string Cedula, string psusuario, string psterminal, string CentroCosto, string Recurso, string Linea, string Cargo)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                db.BITACORA_MOVER_EMPLEADO.Add(new BITACORA_MOVER_EMPLEADO
                {
                    Cedula = Cedula,
                    CodCargo = Cargo,
                    CodCentroCosto = CentroCosto,
                    CodLinea = Linea,
                    CodRecurso = Recurso,
                    EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = psterminal,
                    UsuarioIngresoLog = psusuario
                });
                db.SaveChanges();
                return "Registro guardado con éxito";
            }
        }
    }
}