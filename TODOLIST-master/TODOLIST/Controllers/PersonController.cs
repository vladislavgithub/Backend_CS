using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TODOLIST.Models;
using TODOLIST.assets;
using TODOLIST.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace TODOLIST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly TableContext _context;

        public PersonController(TableContext context)
        {
            _context = context;
        }

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
          if (_context.Persons == null)
          {
              return NotFound();
          }
            return await _context.Persons.Include(rd => rd.PersonRequests).ToListAsync();
        }

        // GET: api/People/5



        [HttpGet("GetPersonRequests")]
        public async Task<ActionResult<IEnumerable<Request>>> GetPerson(int id)
        {
            if (_context.Persons == null)
            {
                return NotFound();
            }
            var workerRequests = _context.Requests.Include(rd => rd.RequestData).Where(r => r.PersonId == id).ToList();

            if (workerRequests == null)
            {
                return NotFound();
            }

            return workerRequests;
        }

        [HttpGet("GetPersonWithMoreRequests")]
        public async Task<ActionResult<Person>> GetPerson()
        {
            if (_context.Persons == null)
            {
                return NotFound();
            }
            var person = _context.Persons
                .Include(pr => pr.PersonRequests)
                .OrderByDescending(p => p.PersonRequests.Count())
                .ToList()[0];



            return person;
        }

        [HttpGet("GetPersonNumberOfRequests")]
        public async Task<ActionResult<int>> GetNumberOfRequest(int personId)
        {
            if (_context.Persons == null && _context.Persons.Where(p => p.id == personId) == null)
            {
                return NotFound();
            }
            var requests = _context.Requests.Where(r => r.PersonId == personId).ToList();


            return requests.Count();
        }



        // PUT: api/People/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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

        // POST: api/People
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(PersonDTO personDTO)
        {
            if (_context.Persons == null)
            {
                return Problem("Entity set 'TableContext.Workers'  is null.");
            }
            var person = new Person(personDTO.name, personDTO.password);
            person.PersonRequests = new List<RequestData> ();
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.id }, person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            if (_context.Persons == null)
            {
                return NotFound();
            }
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(int id)
        {
            return (_context.Persons?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
