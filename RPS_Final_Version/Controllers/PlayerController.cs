using Microsoft.AspNetCore.Mvc;
using RPS_Final_Version.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RPS_Final_Version.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {

         ///swagger endpoint because i always forget to add it
        /// https://localhost:7066/swagger/index.html
        private readonly rock_paper_scissorsContext _context;
        public IConfiguration Configuration { get; }

        public PlayerController(rock_paper_scissorsContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }
        

      
        // GET: api/<PlayerController>
        [HttpGet]
        public IEnumerable<string> Get()
        {

            _context.ChangeTracker.Clear(); 
            //get all the players in the database
            var players = _context.Players.AsEnumerable();

            //return the list of players
            return players.Select(p => p.Username);
            
        
        }

        // PUT api/<PlayerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PlayerController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            //delete the player from the database
            var player = _context.Players.Find(id);

            //nul check
            if (player != null)
            {
                _context.Players.Remove(player);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
            
        }
    }
}
