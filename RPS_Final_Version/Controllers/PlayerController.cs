using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPS_Final_Version.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RPS_Final_Version.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly rock_paper_scissorsContext _context;
        
        public IConfiguration Configuration { get; }

        public PlayerController(rock_paper_scissorsContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }


        // GET: api/<PlayerController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var output = await _context.Players.ToListAsync();
                if (output == null)
                {
                    return NotFound();
                }
                return Ok(output);
            }
            catch (Exception ex)
            {
               return BadRequest($"{BadRequest().StatusCode} : {ex.Message}");
            }
            
            
        }

        // GET api/<PlayerController>/loady
        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string id)
        {
            
            try
            {
                //check if the player exists
                var player = await _context.Players.FirstOrDefaultAsync(p => p.Username == id);
                if (player == null)
                {
                    return Ok("Player not found");
                }
                else
                {
                    return Ok(player.Username);  
                }
               
            }
            catch (Exception ex)
            {
                return BadRequest($"{BadRequest().StatusCode} : {ex.Message}");
            }
           
            
        }

        // POST api/<PlayerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PlayerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PlayerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
