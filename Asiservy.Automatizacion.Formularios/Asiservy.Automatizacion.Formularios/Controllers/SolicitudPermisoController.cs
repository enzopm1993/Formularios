using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class SolicitudPermisoController : Controller
    {
        [Authorize]
        // GET: SolicitudPermiso
        public ActionResult BandejaProduccion()
        {
            return View();
        }

        [Authorize]
        public ActionResult BandejaRRHH()
        {
            return View();
        }
        // GET: SolicitudPermiso/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SolicitudPermiso/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SolicitudPermiso/Create
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

        // GET: SolicitudPermiso/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SolicitudPermiso/Edit/5
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

        // GET: SolicitudPermiso/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SolicitudPermiso/Delete/5
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
