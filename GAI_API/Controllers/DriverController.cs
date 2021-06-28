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
    [Route("api/[controller]/")]
    public class DriverController : Controller
    {
        // GET: DriverController
        private static EvsyutinGAIEntities db = new EvsyutinGAIEntities();

        [HttpGet]
        public List<Drivers> GetDrivers()
        {
            using var context = new EvsyutinGAIEntities();
            return context.Drivers.ToList();
        }

        [HttpPost]
        public async Task<IActionResult> PostDriver([FromBody] Drivers driver)
        {
            db.Entry(driver).State = EntityState.Added;

            await db.Drivers.AddAsync(driver);
            await db.SaveChangesAsync();
            

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(int id, Drivers driver)
        {
            if (id != driver.id)
            {
                return BadRequest();
            }

            var local = db.Set<Drivers>()
                .Local
                .FirstOrDefault(p => p.id.Equals(id));
            if (local != null)
                db.Entry(local).State = EntityState.Detached;
            db.Entry(driver).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await db.Drivers.FindAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
    }
}
