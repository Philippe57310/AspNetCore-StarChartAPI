using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet("{id:int}",Name ="GetById")]
        public IActionResult GetById(int id)
        {
            //Name = "GetById";
            var result = _context.CelestialObjects.Find(id);
            if (result == null) return NotFound();

            List<CelestialObject> celestialObjects = _context.CelestialObjects.Where(o => o.OrbitedObjectId == id).ToList();
            result.Satellites = celestialObjects;

            return Ok(result);
        }


        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var results = _context.CelestialObjects.Where(c => c.Name == name);
            if (!results.Any()) return NotFound();

            foreach (var item in results)
            {
               
                item.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == item.Id).ToList();
            }

            return Ok(results);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var results = _context.CelestialObjects.ToList();
            foreach (var celestialObject in results)
            {
                List<CelestialObject> satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == celestialObject.Id).ToList();
                celestialObject.Satellites = satellites;
            }

            return Ok(results);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            DbSet<CelestialObject> celestialObjects = _context.CelestialObjects;
            celestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }


        [HttpPut("{id}")]
        public IActionResult Update( int id, CelestialObject celestialObject)
        {
            var result = _context.CelestialObjects.Find(id);
            if (result == null) return NotFound();
            result.Name = celestialObject.Name;
            result.OrbitalPeriod = celestialObject.OrbitalPeriod;
            result.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.CelestialObjects.Update(result);
            _context.SaveChanges();

            return NoContent(); ;
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var result = _context.CelestialObjects.Find(id);
            if (result == null) return NotFound();
            result.Name =name;
            _context.CelestialObjects.Update(result);
            _context.SaveChanges();

            return NoContent(); ;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _context.CelestialObjects.Where(c=>(c.Id == id)|| (c.OrbitedObjectId==id));
            if (!result.Any()) return NotFound();
            _context.CelestialObjects.RemoveRange(result);
            _context.SaveChanges();

            return NoContent(); ;
        }
    }


}
