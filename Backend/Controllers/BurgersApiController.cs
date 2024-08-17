using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Burgermania.Data;
using Burgermania.Models;
using Microsoft.AspNetCore.Authorization;

namespace Burgermania.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BurgersApiController : ControllerBase
    {
        private readonly BurgerDbContext _context;

        public BurgersApiController(BurgerDbContext context)
        {
            _context = context;
        }

        // GET: api/BurgersApi
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Burger>>> GetBurgers()
        {
            // Retrieve the JWT token from the Authorization header
            //var authHeader = HttpContext.Request.Headers["Authorization"];
            //if (string.IsNullOrEmpty(authHeader))
            //{
            //    return Unauthorized(); // No token provided
            //}

            // Extract the token from the header
            //var token = authHeader.ToString().Split(' ')[1];

            // Validate the token (optional, since ASP.NET Core handles this automatically)
            // You can add additional validation logic if needed

            // Fetch the burgers from the database
            var burgers = await _context.Burgers.ToListAsync();
            if (burgers == null || !burgers.Any())
            {
                return NotFound(); // or return an empty list
            }

            return Ok(burgers);
        }

        // GET: api/BurgersApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Burger>> GetBurger(int id)
        {
            var burger = await _context.Burgers.FindAsync(id);

            if (burger == null)
            {
                return NotFound();
            }

            return burger;
        }

        // PUT: api/BurgersApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        
        public async Task<IActionResult> PutBurger(int id, Burger burger)
        {
          
            if (id != burger.BurgerId)
            {
                return BadRequest(); 
            }

         
            _context.Entry(burger).State = EntityState.Modified;

            try
            {
                
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
                if (!BurgerExists(id))
                {
                    return NotFound(); 
                }
                else
                {
                    throw; 
                }
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent(); 
        }

       

        // POST: api/BurgersApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        public async Task<ActionResult<Burger>> PostBurger(Burger burger)
        {
            _context.Burgers.Add(burger);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBurger", new { id = burger.BurgerId }, burger);
        }

        // DELETE: api/BurgersApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBurger(int id)
        {
            var burger = await _context.Burgers.FindAsync(id);
            if (burger == null)
            {
                return NotFound();
            }

            _context.Burgers.Remove(burger);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BurgerExists(int id)
        {
            return _context.Burgers.Any(e => e.BurgerId == id);
        }
    }
}
