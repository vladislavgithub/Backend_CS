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
    public class RequestsController : ControllerBase
    {
        private readonly TableContext _context;

        public RequestsController(TableContext context)
        {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
          if (_context.Requests == null)
          {
              return NotFound();
          }
            return await _context.Requests.Include(rd => rd.RequestData).ToListAsync();
        }

        // GET: api/Requests/5

        [HttpGet("GetRequestsByTitle")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequest(string title)
        {
            if (_context.Requests == null)
            {
                return NotFound();
            }
            var request = await _context.Requests
                .Where(r => r.RequestData.Title == title)
                .Include(rd => rd.RequestData)
                .ToListAsync();

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        [HttpGet("GetRequestsByStatus")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequest(int statusId)
        {
            if (_context.Requests == null)
            {
                return NotFound();
            }
            var request = await _context.Requests
                .Where(r => r.RequestData.StatusId == statusId)
                .Include (rd => rd.RequestData)
                .ToListAsync();

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("EditRequest")]
        public async Task<IActionResult> PutRequest(int id, EditRequestDTO editRequestData)
        {
            var requestData = await _context.RequestDatas.FirstOrDefaultAsync(rd => rd.Id == id);

            if (requestData == null)
            {
                return Problem("You don't have request with this id");
            }

            requestData.ChangeData = DateTime.Now;
            requestData.Title = editRequestData.Title;
            requestData.Description = editRequestData.Description;
            requestData.StatusId = editRequestData.statusId;

            _context.Entry(requestData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!RequestExists(id))
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

        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(PostRequestDto request)
        {
            if (_context.Requests == null)
            {
                return Problem("Entity set 'TableContext.Requests'  is null.");
            }

            if (_context.Persons.FirstOrDefault(p => p.id == request.PersonId) == null)
            {
                return Problem("You don't have Person with this id");
            }
            var Request1 = new Request(request.Title, request.Description, request.PersonId, request.StatusId);

            _context.Requests.Add(Request1);
            await _context.SaveChangesAsync();

            var person = _context.Persons.Include(rd => rd.PersonRequests).FirstOrDefault(p => p.id == request.PersonId);
            if (person != null)
            {
                person.PersonRequests.Add(Request1.RequestData);
                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = Request1.id }, Request1);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            if (_context.Requests == null)
            {
                return NotFound();
            }
            var request = await _context.Requests.FindAsync(id);
            var requestData = await _context.RequestDatas.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            _context.RequestDatas.Remove(requestData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return (_context.Requests?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
