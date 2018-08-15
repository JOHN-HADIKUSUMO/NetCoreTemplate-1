using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BasicAuth.Models;

namespace BasicAuth.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Female")]
        public IActionResult Female()
        {
            ViewData["Message"] = "Female Section";

            return View();
        }

        [Authorize(Policy = "Male")]
        public IActionResult Male()
        {
            ViewData["Message"] = "Male Section";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
