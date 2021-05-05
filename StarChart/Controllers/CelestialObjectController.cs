using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext _context)
        {
            this._context = _context;
        }

        [HttpGet("{id:int}")]
        [ActionName("GetById")]
        public ActionResult<CelestialObject> GetById(int id)
        {
            //Name = "GetById";
            var result = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (result == null) return NotFound();

            List<CelestialObject> celestialObjects = _context.CelestialObjects.Where(o => o.OrbitedObjectId == result.Id).ToList();
            result.Satellites = celestialObjects;

            return Ok(result);
        }


        [HttpGet("{name}")]
        [ActionName("{name}")]
        public ActionResult<CelestialObject> GetByName(string name)
        {
            var result = _context.CelestialObjects.FirstOrDefault(c => c.Name == name);
            if (result == null) return NotFound();

            List<CelestialObject> celestialObjects = _context.CelestialObjects.Where(o => o.OrbitedObjectId == result.Id).ToList();
            result.Satellites = celestialObjects;
            return Ok(result);
        }

        [HttpGet]
        public ActionResult<IEnumerable<CelestialObject>> GetAll()
        {
            var results = _context.CelestialObjects;
            if (results == null) return NotFound();
            foreach (var celestialObject in results)
            {
                List<CelestialObject> satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == celestialObject.Id).ToList();
                celestialObject.Satellites = satellites;
            }

            return Ok(results);
        }
    }


}
