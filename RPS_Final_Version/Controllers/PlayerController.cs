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
        public async Task<IActionResult> Get()
        {
            try
            {
                var players = await _context.Players.ToListAsync();
                if (players == null)
                {
                    return NotFound();
                }
                ///get the username from each player in the list 
                ///and return it as a list of strings
                var output = players.Select(x => x.Username).ToList();
                
                return Ok(output);
            }
            catch (Exception ex)
            {
               return BadRequest($"{BadRequest().StatusCode} : {ex.Message}");
            }
            
            
        }

        // GET api/<PlayerController>/Login
        [HttpGet("Login/{playerName}")]
        public async Task<IActionResult> Get(string playerName)
        {
            
            try
            {
                //check if the player exists
                var player = await _context.Players.FirstOrDefaultAsync(p => p.Username == playerName);
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
        public async Task<IActionResult> Post([FromBody] string value)
        {
            try
            {
                 //check if player name already exists

                var player = await _context.Players.FirstOrDefaultAsync(p => p.Username == value);

                if (player == null)
                {
                    //create a new player
                    var newPlayer = new Player
                    {
                        Username = value
                    };

                    //add the new player to the database
                    _context.Players.Add(newPlayer);
                    _context.SaveChanges();
                    return Ok("Player created");
                }
                else
                {
                    //player already exists
                    return Ok("Player already exists");
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"{BadRequest().StatusCode} : {ex.Message}");
            }
        }

        // ... im not sure if i need this
        // // PUT api/<PlayerController>/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody] string value)
        // {

        // }

        // DELETE api/<PlayerController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string playerName)
        {
            try
            {
                var player = await _context.Players.FirstOrDefaultAsync(p => p.Username == playerName);

                if (player == null)
                {
                    return Ok("Player not found");
                }
                else
                {
                    _context.Players.Remove(player);
                    _context.SaveChanges();
                    return Ok("Player deleted");
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"{BadRequest().StatusCode} : {ex.Message}");
            }
          
        }
    }
}
