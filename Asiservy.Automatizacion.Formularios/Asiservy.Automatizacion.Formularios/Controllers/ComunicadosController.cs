using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class ComunicadosController : Controller
    {
        // GET: Comunicados
        public ActionResult Index()
        {
            return View();
        }

        // GET: Comunicados/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Comunicados/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comunicados/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Comunicados/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Comunicados/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Comunicados/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Comunicados/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
