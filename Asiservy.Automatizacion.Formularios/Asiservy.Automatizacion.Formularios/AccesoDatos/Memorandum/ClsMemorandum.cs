using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Memorandum
{
    public class ClsMemorandum
    {
        public int GuardarPlantillaMemo(PlantillasMemorandum modelMemorandum)
        {
  
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (modelMemorandum.id == 0 )
                {
                    db.PlantillasMemorandum.Add(modelMemorandum);
                }
                else
                {
                    PlantillasMemorandum objBusca = db.PlantillasMemorandum.Find(modelMemorandum.id);
                    objBusca.Titulo = modelMemorandum.Titulo;
                    objBusca.Plantilla = modelMemorandum.Plantilla;
                    objBusca.Estado = modelMemorandum.Estado;
                    objBusca.UsuarioModifica = modelMemorandum.UsuarioModifica;
                    objBusca.TerminalModifica = modelMemorandum.TerminalModifica;
                    objBusca.FechaModifica = modelMemorandum.FechaModifica;
                   
                }
                db.SaveChanges();
            }
            return modelMemorandum.id;
        }
        public PlantillasMemorandum ObtenerMemo(int? idMemo)
        {
            PlantillasMemorandum objBusca = null;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                objBusca = db.PlantillasMemorandum.Find(idMemo);
            }
            return objBusca;
        }

    }
}