﻿using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Asistencia;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia
{
    public class clsDCambioPersonal
    {
        public static bool Between(DateTime entrada, DateTime fecha1, DateTime fecha2)
        {
            return (entrada > fecha1 && entrada < fecha2);
        }
        public List<BitacoraCambioPersonalModelView> ConsultarBitacoraCambioPersonal(BitacoraCambioPersonalModelView filtros)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<BitacoraCambioPersonalModelView> ListadoCambioPersonal = new List<BitacoraCambioPersonalModelView>();

                IEnumerable<BITACORA_CAMBIO_PERSONAL> Listado = entities.BITACORA_CAMBIO_PERSONAL;
                if (!string.IsNullOrEmpty(filtros.CodLinea))
                {
                    Listado = Listado.Where(x => x.CodLinea == filtros.CodLinea);
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


                foreach (var x in Listado.ToList())
                {
                    var Linea = entities.spConsultaLinea(x.CodLinea).FirstOrDefault();
                    var Area = entities.spConsultaArea(x.CodArea).FirstOrDefault();
                    var Empleado = entities.spConsutaEmpleados(x.Cedula).FirstOrDefault();
                    //var Cargo = entities.spConsultaCargos(x.Cedula).FirstOrDefault();

                    ListadoCambioPersonal.Add(new BitacoraCambioPersonalModelView
                    {
                        Cedula = x.Cedula,
                        Nombre = Empleado != null ? Empleado.NOMBRES : "",
                        //CodCargo = x.CodCargo,
                        //Cargo = Cargo.Descripcion,
                        Area = Area != null ? Area.Descripcion : "",
                        CodArea = x.CodArea,
                        CodLinea = x.CodLinea,
                        Linea = Linea != null ? Linea.Descripcion : "",
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


        public string GuardarCambioDePersonal(List<CAMBIO_PERSONAL> pListCambioPersonal/*,List<BITACORA_CAMBIO_PERSONAL> Bitacora*/, string tipo)
        {
            List<String> listCedulas = new List<String>();
            string psLinea = pListCambioPersonal.FirstOrDefault().CodLinea;
            string psCentroCosto = pListCambioPersonal.FirstOrDefault().CentroCosto;
            string psRecurso = pListCambioPersonal.FirstOrDefault().Recurso;
            string psCargo = pListCambioPersonal.FirstOrDefault().CodCargo;
            DateTime? psfecha = pListCambioPersonal.FirstOrDefault().Fecha;
            TimeSpan? psHora = pListCambioPersonal.FirstOrDefault().HoraInicio;
            string psusuario = pListCambioPersonal.FirstOrDefault().UsuarioIngresoLog;
            string psterminal = pListCambioPersonal.FirstOrDefault().TerminalIngresoLog;
            List<RespuestaGeneral> RespuestaGeneral = new List<RespuestaGeneral>();
            DateTime FechaActual = DateTime.Now;
            List<string> NoSePudieornMover = new List<string>();
            List<string> NoSePudieornRegresar = new List<string>();
            string psTurno = string.Empty;
            EMPLEADO_TURNO poTurnoEmpleado = null;
            spConsutaEmpleados Empleado = null;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    if (tipo == "prestar")
                    {
                        //**Buscar En Asistencia EstadoFalta para no permitir mover
                        ASISTENCIA BuscarEnAsistencia = null;
                        foreach (var item in pListCambioPersonal.ToArray())
                        {
                            BuscarEnAsistencia = db.ASISTENCIA.Where(x => x.Cedula == item.Cedula && x.Fecha == item.Fecha && x.EstadoAsistencia == clsAtributos.EstadoFalta && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                            if (BuscarEnAsistencia != null)
                            {
                                pListCambioPersonal.Remove(item);
                                Empleado = db.spConsutaEmpleados(item.Cedula).FirstOrDefault();
                                NoSePudieornMover.Add(Empleado.NOMBRES+" con cédula N°:  "+item.Cedula + ": No se puede mover, por que su asistencia ya fue generada en la fecha indicada y tiene estado falta.");
                            }

                        }
                        //**
                        if (pListCambioPersonal.Count > 0)//si es un CAMBIO_PERSONAL(nuevo) que no está en la tabla CAMBIO_PERSONAL, hay que crearlo 
                        {
                            //**verificar si esta en movimiento_personal_diario

                            MOVIMIENTO_PERSONAL_DIARIO BuscarMovimientoPersonalDiario;
                            CAMBIO_PERSONAL BuscarPerCambioPersonal;
                            CAMBIO_PERSONAL BuscarCPFalse;
                            DateTime? FechaIngresada;
                            DateTime? ptFechaInicioCP=null;
                            foreach (var item in pListCambioPersonal.ToArray())
                            {
                                Empleado = db.spConsutaEmpleados(item.Cedula).FirstOrDefault();
                                FechaIngresada = item.HoraInicio == null ? item.Fecha : item.Fecha.Value.Add(item.HoraInicio.Value);
                                //busco si la persona esta en CAMBIO_PERSONAL con Vigente false(osea que ya ha sido regresado)
                                BuscarCPFalse = db.CAMBIO_PERSONAL.Where(z => z.Cedula == item.Cedula && z.EstadoRegistro == clsAtributos.EstadoRegistroActivo).OrderByDescending(x=>x.IdCambioPersonal).FirstOrDefault();

                                BuscarPerCambioPersonal = db.CAMBIO_PERSONAL.Where(x => x.Cedula == item.Cedula && !x.Vigente.Value&& x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).OrderByDescending(z=>z.IdCambioPersonal).FirstOrDefault();

                                if (BuscarPerCambioPersonal != null && !BuscarCPFalse.Vigente.Value &&  (FechaIngresada <= BuscarPerCambioPersonal.FechaFin.Value.Add(BuscarPerCambioPersonal.Horafin.Value)))
                                {
                                        pListCambioPersonal.Remove(item);
                                        NoSePudieornMover.Add(Empleado.NOMBRES + " con cédula N°:  " + item.Cedula + ": No se puede mover, por que ya habia sido movido en una fecha menor a la indicada.");
                                }
                                //**agregado 04-06-2020 requerimiento de prestar personal sin necesidad de regresar
                                
                                BuscarPerCambioPersonal = db.CAMBIO_PERSONAL.Where(x => x.Cedula == item.Cedula && x.Vigente.Value &&x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).OrderByDescending(x => x.IdCambioPersonal).FirstOrDefault();
                                if (BuscarPerCambioPersonal!=null)
                                {
                                    ptFechaInicioCP = BuscarPerCambioPersonal.HoraInicio != null ? BuscarPerCambioPersonal.Fecha.Value.Add(BuscarPerCambioPersonal.HoraInicio.Value) : BuscarPerCambioPersonal.Fecha.Value;
                                }
                                if (BuscarPerCambioPersonal != null && (FechaIngresada <= ptFechaInicioCP))
                                {
                                        pListCambioPersonal.Remove(item);
                                        NoSePudieornMover.Add(Empleado.NOMBRES + " con cédula N°:  " + item.Cedula + ": No se puede mover, por que ya habia sido movido en una fecha menor a la indicada.");
                                }
                                if(BuscarPerCambioPersonal!=null && (BuscarPerCambioPersonal.RecursoOrigen == psRecurso&&BuscarPerCambioPersonal.CodLineaOrigen==psLinea&&BuscarPerCambioPersonal.CentroCostoOrigen==psCentroCosto&&BuscarPerCambioPersonal.CodCargo==psCargo))
                                {
                                    pListCambioPersonal.Remove(item);
                                    NoSePudieornMover.Add(Empleado.NOMBRES + " con cédula N°:  " + item.Cedula + ": No puede ser prestado al mismo centro de costo, recurso, línea y cargo al que pertenece, debe ir a la opción regresar empleados.");
                                }else if (BuscarPerCambioPersonal != null && (FechaIngresada > ptFechaInicioCP))
                                {
                                    BuscarPerCambioPersonal.Vigente = false;
                                    BuscarPerCambioPersonal.FechaFin = FechaIngresada;
                                    BuscarPerCambioPersonal.Horafin = psHora == null ? TimeSpan.Parse("00:00") : psHora;
                                    BuscarPerCambioPersonal.FechaModificacionLog = DateTime.Now;
                                    BuscarPerCambioPersonal.UsuarioModificacionLog = psusuario;
                                    BuscarPerCambioPersonal.TerminalModificacionLog = psterminal;
                                    db.SaveChanges();
                                }
                                //**
                                if (item.HoraInicio == null)//si la hora ingresada es "Inicio Jornada(null)", entonces busco si esa persona no tiene estado presente de asistencia
                                {
                                    BuscarMovimientoPersonalDiario = db.MOVIMIENTO_PERSONAL_DIARIO.Where(x => x.Cedula == item.Cedula && x.FechaInicio == psfecha&&x.EstadoRegistro==clsAtributos.EstadoRegistroActivo /*&& x.Asistencia.Value*/).OrderByDescending(x => x.IdCambioPersonal).FirstOrDefault();//busco el registro en movimiento_personal_diario
                                    if (BuscarMovimientoPersonalDiario != null)
                                    {
                                        pListCambioPersonal.Remove(item);
                                        NoSePudieornMover.Add(Empleado.NOMBRES + " con cédula N°:  " + item.Cedula + ": No se pudo mover a Inicio de Jornada por que la asistencia ya fue marcada en la Línea donde pertenece.");
                                    }
                                    //validar que no haya sido generada la asistencia en la linea a la que pertenece el empleado en el dia que se lo piensa mover(por que es a inicio de jornada
                                    var buscarasistencia = db.ASISTENCIA.Where(x => x.Cedula == item.Cedula&&x.Fecha==item.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                                    if (buscarasistencia.Count > 0)
                                    {
                                        pListCambioPersonal.Remove(item);
                                        NoSePudieornMover.Add(Empleado.NOMBRES + " con cédula N°:  " + item.Cedula + ": No se pudo mover a inicio de jornada por que la asistencia ya habia sido generada en su linea. ");
                                    }
                                }
                                else//significa que el usuario si ingreso una hora para mover al empleado
                                {
                                    //BuscarMovimientoPersonalDiario = db.MOVIMIENTO_PERSONAL_DIARIO.Where(x => x.Cedula == item.Cedula && x.FechaInicio == psfecha
                                    //                                                                     && x.Asistencia.Value).FirstOrDefault();
                                    BuscarMovimientoPersonalDiario = db.MOVIMIENTO_PERSONAL_DIARIO.Where(x => x.Cedula == item.Cedula && x.FechaInicio == psfecha
                                                                                                         &&x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).OrderByDescending(x=>x.IdCambioPersonal).FirstOrDefault();
                                    //Si la hora ingresada es menor a la hora de la asistencia(presente) de ese empleado entonces no se lo puede mover a otra línea
                                    if (BuscarMovimientoPersonalDiario != null)
                                    {
                                        //si la hora de Cambio de Personal es Mejor a la HoraInicio de Movimiento_Personal entonces no se puede mover al empleado
                                        if (BuscarMovimientoPersonalDiario.HoraFin!=null)//valido si se finalizo asistencia, que no lo pueda mover en una hora menor a la que se finalizo(siempre deberá ser mayor a la hora de finalización de la asistencia)
                                        {
                                            if (psHora <= BuscarMovimientoPersonalDiario.HoraFin)
                                            {
                                                pListCambioPersonal.Remove(item);
                                                NoSePudieornMover.Add(Empleado.NOMBRES + " con cédula N°:  " + item.Cedula + ": No se pudo mover por que la hora ingresada es menor a la hora en que ya marco asistencia en la Línea a la que pertenece.");
                                            }
                                        }else if (psHora < BuscarMovimientoPersonalDiario.HoraInicio)// && psHora>x.HoraInicio
                                        {
                                            pListCambioPersonal.Remove(item);
                                            NoSePudieornMover.Add(Empleado.NOMBRES + " con cédula N°:  " + item.Cedula + ": No se pudo mover por que la hora ingresada es menor a la hora en que ya marco asistencia en la Línea a la que pertenece.");
                                        }
                                        else
                                        {
                                            BuscarMovimientoPersonalDiario.HoraFin = psHora;
                                            BuscarMovimientoPersonalDiario.FechaFin = psfecha;
                                            BuscarMovimientoPersonalDiario.TerminalModificacionLog = psterminal;
                                            BuscarMovimientoPersonalDiario.FechaModificacionLog = DateTime.Now;
                                            BuscarMovimientoPersonalDiario.UsuarioModificacionLog = psusuario;

                                            poTurnoEmpleado = db.EMPLEADO_TURNO.Where(x => x.Cedula == item.Cedula).FirstOrDefault();
                                            psTurno = poTurnoEmpleado == null ? clsAtributos.TurnoUno : poTurnoEmpleado.Turno;//consulto el turno al que pertenece el empleado

                                            db.MOVIMIENTO_PERSONAL_DIARIO.Add(new MOVIMIENTO_PERSONAL_DIARIO { Cedula=item.Cedula,CodLinea=psLinea,CentroCosto=psCentroCosto,
                                            CodCargo=psCargo,Recurso=psRecurso,FechaInicio=psfecha,HoraInicio=psHora,EstadoRegistro=clsAtributos.EstadoRegistroActivo,
                                            FechaIngresoLog=DateTime.Now,UsuarioIngresoLog=psusuario,TerminalIngresoLog=psterminal, Asistencia=false, Turno=psTurno});
                                            db.SaveChanges();
                                        }
                                    }
                                }


                            }
                            //**
                            db.CAMBIO_PERSONAL.AddRange(pListCambioPersonal);
                        }
                        db.SaveChanges();
                        transaction.Commit();
                        if (NoSePudieornMover.Count > 0)
                        {           
                            string Mensaje = string.Empty;
                            foreach (var item in NoSePudieornMover)
                            {
                                Mensaje += item + "<br>";
                            }
                            //return "Los empleados: " + Mensaje + " No se pudieron mover a la fecha indicada por que la asistencia fue generada";

                            return Mensaje;

                        }
                        if (NoSePudieornMover.Count == 0)
                        {
                            return "1000"; //Empleados movidos con éxito

                        }

                    }
                    else if (tipo == "regresar")
                    {
                        CAMBIO_PERSONAL BuscarEnCambioPersonal = null;
                        string LineaDesc = string.Empty;
                        DateTime ValidaFecha;
                        
                        //**CONSULTO LA ASISTENCIA, si tiene asistencia falta, no le permito regresarlo(tiene que ir a marcarle presente y de ahi le permitira regresarlo) agregado 2020-01-15
                        ASISTENCIA buscaEnAsistencia = null;
                        foreach (var item in pListCambioPersonal.ToArray())
                        {
                            buscaEnAsistencia = db.ASISTENCIA.Where(x => x.Cedula == item.Cedula&&x.Fecha==item.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo && x.EstadoAsistencia == clsAtributos.EstadoFalta).FirstOrDefault();
                            if (buscaEnAsistencia != null)
                            {
                                pListCambioPersonal.Remove(item);
                                Empleado = db.spConsutaEmpleados(item.Cedula).FirstOrDefault();
                                LineaDesc = db.spConsultaLinea(buscaEnAsistencia.Linea).FirstOrDefault().Descripcion;
                                NoSePudieornRegresar.Add(Empleado.NOMBRES+" con cédula N°: "+ item.Cedula+", No se pudo regresar por que tiene asistencia con estado de falta en la fecha indicada en la línea: "+ LineaDesc + ". <br>");

                            }
                           
                        }
                        //*
                        foreach (var item in pListCambioPersonal.ToArray())
                        {
                            BuscarEnCambioPersonal = db.CAMBIO_PERSONAL.Where(x => x.Cedula == item.Cedula && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo&&x.Vigente.Value).FirstOrDefault();//correcion 2020-02-20 traer solo Vigente=1
                            //Para validar que la fechafin y hora fin de regreso no pueda ser menor o igual a la fecha y hora inicio
                            ValidaFecha = BuscarEnCambioPersonal.HoraInicio == null ? BuscarEnCambioPersonal.Fecha.Value : BuscarEnCambioPersonal.Fecha.Value.Add(BuscarEnCambioPersonal.HoraInicio.Value);
                            if (ValidaFecha < (psfecha.Value.Add(psHora.Value)))
                            {
                                listCedulas.Add(item.Cedula);
                            }
                            else
                            {
                                Empleado = db.spConsutaEmpleados(item.Cedula).FirstOrDefault();
                                NoSePudieornRegresar.Add(Empleado.NOMBRES+" con cédula N°: "+ item.Cedula+ ":  No se pudo regresar por que la fecha de Fin debe ser mayor a la Fecha de inicio. <br>");
                            }
                            

                        }
                        //**
                        foreach (var item in listCedulas.ToArray())
                        {
                            buscaEnAsistencia = db.ASISTENCIA.Where(x => x.Cedula == item && x.Fecha == psfecha && x.FechaFin != null).FirstOrDefault();
                            if(buscaEnAsistencia!=null)
                            {
                                if (buscaEnAsistencia.FechaFin.Value.Add(buscaEnAsistencia.HoraSalida.Value) >= psfecha.Value.Add(psHora.Value))
                                {
                                    listCedulas.Remove(item);
                                    Empleado = db.spConsutaEmpleados(item).FirstOrDefault();
                                    NoSePudieornRegresar.Add(Empleado.NOMBRES + " con cédula N°: " + item + ": No se pudo regresar por que la hora de finalización de asistencia es mayor o igual a la fecha en la que se lo va a regresar. <br>");

                                }
                            }
                        }
                        //**
                        //verifica si esta en movimiento: personal: diario
                        MOVIMIENTO_PERSONAL_DIARIO BuscarMovimientoPersonalDiario;
                        clsDEmpleado ClsdEmpleado = new clsDEmpleado();
                        spConsultaEspecificaEmpleadosxCedula BuscarEmpleadoDataL;
                        
                        //List<MOVIMIENTO_PERSONAL_DIARIO> ListMovimientopersonalDiario = new List<MOVIMIENTO_PERSONAL_DIARIO>();

                        foreach (var item in listCedulas.ToArray())
                        {
                            BuscarMovimientoPersonalDiario = db.MOVIMIENTO_PERSONAL_DIARIO.Where(x => x.Cedula == item && x.FechaInicio == psfecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo /*&& !x.Asistencia.Value*/).OrderByDescending(x => x.IdCambioPersonal).FirstOrDefault();
                            BuscarEmpleadoDataL = ClsdEmpleado.ConsultarEmpleadoxCedula(item);
                            
                            if (BuscarMovimientoPersonalDiario != null)
                            {
                                if (BuscarMovimientoPersonalDiario.FinalizaAsistencia==null)
                                {
                                    //modifico el registro de asistencia, lo actualizo con fecha y hora fin
                                    BuscarMovimientoPersonalDiario.HoraFin = psHora;
                                    BuscarMovimientoPersonalDiario.FechaFin = psfecha;
                                    BuscarMovimientoPersonalDiario.UsuarioModificacionLog = psusuario;
                                    BuscarMovimientoPersonalDiario.FechaModificacionLog = DateTime.Now;
                                    BuscarMovimientoPersonalDiario.TerminalModificacionLog = psterminal;
                                    //Creo un nuevo registro en movimiento_personal para poder generar la finalizacion de la asistencia de la linea a la que retorno el empleado
                                    poTurnoEmpleado = db.EMPLEADO_TURNO.Where(x => x.Cedula == item).FirstOrDefault();
                                    psTurno = poTurnoEmpleado == null ? clsAtributos.TurnoUno : poTurnoEmpleado.Turno;//consulto el turno al que pertenece el empleado
                                    db.MOVIMIENTO_PERSONAL_DIARIO.Add(new MOVIMIENTO_PERSONAL_DIARIO
                                    {
                                        Cedula = item,
                                        CodLinea = BuscarEmpleadoDataL.LINEA,//le pongo la linea, recurso, cargo y centro de costo del registro que encontro en Movimiento personal por que esta regresando a donde pertenecia
                                        CentroCosto = BuscarEmpleadoDataL.CENTRO_COSTOS,
                                        CodCargo = BuscarEmpleadoDataL.CARGO,
                                        Recurso = BuscarEmpleadoDataL.RECURSO,
                                        FechaInicio = psfecha,
                                        HoraInicio = psHora,
                                        EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                                        FechaIngresoLog = DateTime.Now,
                                        UsuarioIngresoLog = psusuario,
                                        TerminalIngresoLog = psterminal,
                                        Asistencia = false,
                                        Turno = psTurno
                                    });

                                    //, Recurso=psRecurso});
                                    //ListMovimientopersonalDiario.Add(new MOVIMIENTO_PERSONAL_DIARIO {Cedula=item,CodLinea=psLinea, CentroCosto=psCentroCosto, CodCargo=psCargo
                                    //, Recurso=psRecurso});
                                }

                            }
                            db.SaveChanges();
                        }
                        
                        //**
                        string[] CedulasArray = listCedulas.ToArray();
                        //Traigo la lista de Cambio_personal que hay que actualizar para regresar
                        var ActualizarCambioPersonal = db.CAMBIO_PERSONAL.Where(p => CedulasArray.Contains(p.Cedula) && p.Vigente.Value && p.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                        foreach (var item in ActualizarCambioPersonal)
                        {
                            //if (psfecha.Value.Add(psHora.Value) > item.Fecha.Value.Add(item.HoraInicio.Value))
                            //{
                            //item.Fecha = psfecha;
                            //item.HoraInicio = psHora;
                            item.Vigente = false;
                            item.FechaFin = psfecha;
                            item.Horafin = psHora;
                            item.FechaModificacionLog = DateTime.Now;
                            item.UsuarioModificacionLog = psusuario;
                            item.TerminalModificacionLog = psterminal;
                            //}
                            //else
                            //{
                            //    item.Fecha = item.Fecha;
                            //    item.Vigente = false;
                            //    item.UsuarioModificacionLog = psusuario;
                            //    item.TerminalModificacionLog = psterminal;
                            //}

                        }
                        db.SaveChanges();
                        transaction.Commit();
                        if (NoSePudieornRegresar.Count == 0)
                        {
                            return "Empleado(s) regresado(s) con éxito";
                        }
                        else
                        {
                            string mensaje = string.Empty;
                            foreach (var item in NoSePudieornRegresar)
                            {
                                //mensaje += item + " No se pudo regresar por que la fecha de Fin debe ser mayor a la Fecha de inicio \n";
                                mensaje += item;
                            }
                            return mensaje;
                        }
                    }
                    return "Debe seleccionar, prestar o regresar";
                }

            }
        }

        public sp_ConsultaEmpleadosMovidos ConsultarCambioPersonal(string cedula)
        {
            sp_ConsultaEmpleadosMovidos poCambioPersonal = null;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                poCambioPersonal = db.sp_ConsultaEmpleadosMovidos(cedula).FirstOrDefault();
                return poCambioPersonal;
            }
        }
        public List<spConsultarCambioPersonalxLineaxTurno> ConsultarCambioPersonalxLinea(string CodLinea, string Turno,DateTime Fecha,TimeSpan Hora)
        {

            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                //return db.CAMBIO_PERSONAL.Where(x => x.CodLinea == CodLinea).ToList();
                return db.spConsultarCambioPersonalxLineaxTurno(CodLinea, Turno, Fecha, Hora).ToList();
            }
        }

        public List<spReporteCambioPersonal> ReporteCambioPersonal(string CodLinea, DateTime? FechaInicio, DateTime? FechaFin)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spReporteCambioPersonal(CodLinea, FechaInicio, FechaFin).ToList();
            }
        }
    }
}