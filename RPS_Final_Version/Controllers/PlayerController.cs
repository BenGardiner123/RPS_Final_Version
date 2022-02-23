using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPS_Final_Version.Models;
using RPS_Final_Version.Models.ViewModels;

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

        // post a new player to the database
        // POST api/<PlayerController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlayerRegisterNameCheckRequestModel incomingPlayer)
        {
            //get platyers async
            var players = await _context.Players.ToListAsync();
            
            //check if the player already exists
            if ( players.Any(p => p.Username == incomingPlayer.Username))
            {
                return StatusCode(StatusCodes.Status409Conflict, new Response { Status="Error", Message = "Username already exists" });
            }

            //create a new player and add the username
            var player = new Player();
            player.Username = incomingPlayer.Username;
            

            //try and add add the player to the database
            try
            {
                _context.Players.Add(player);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
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
