using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Vacaciones
{
    public class ClsVacaciones
    {
        public List<VacacionesModelView> ConsultarVacaciones(string cedula, string tipo)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {


                var listSolicitudes = entities.sp_obtenerVacacionesEmpleados(cedula,tipo).ToList();


                List<VacacionesModelView> listaRetorna = new List<VacacionesModelView>();

                foreach (var item in listSolicitudes)
                {

                    listaRetorna.Add(new VacacionesModelView
                    {
                        Linea = "",
                        Nombres = item.Nombres,
                        TotalDias = item.TotalDias,
                        DiasTomados = item.DiasTomados,
                        Saldo = item.Saldo
                    });

                }

                return listaRetorna;
            }
        }
    }
}