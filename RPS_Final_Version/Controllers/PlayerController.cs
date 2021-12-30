using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PlayerController>/loady
        [HttpGet("{username}")]
        public string Get(int id)
        {
            //get a player from the database
            
            return "value";
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
