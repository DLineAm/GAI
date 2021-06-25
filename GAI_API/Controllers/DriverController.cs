using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAI_API.Models;
using Microsoft.EntityFrameworkCore;

namespace GAI_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : Controller
    {
        // GET: DriverController
        private static EvsyutinGAIEntities db = new EvsyutinGAIEntities();

        [HttpGet]
        public List<Drivers> GetDrivers()
        {
            return db.Drivers.ToList();
        }
    }
}
