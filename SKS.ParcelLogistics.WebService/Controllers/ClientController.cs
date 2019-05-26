using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SKS.ParcelLogistics.WebSerice.Controllers;
using SKS.ParcelLogistics.BusinessLogic;

namespace SKS.ParcelLogistics.WebService.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
