﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class MensajeController : Controller
    {
        // GET: Mensaje
        public ActionResult Error()
        {
            return PartialView();
        }
    }
}