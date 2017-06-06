using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CenterForDevelopers.Controllers
{
    public class ExampleCSharpController : Controller
    {
        public IActionResult ValueTuple()
        {
            return View(("Too lazy to create a model", 555));
        }
    }
}
