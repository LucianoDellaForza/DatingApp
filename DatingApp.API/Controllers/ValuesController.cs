using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize] //everything in this controller has to be an autohorized request (with token)
    //  http://localhost:5000/api/values
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;
        public ValuesController(DataContext context)
        {
            this._context = context;
        }

        //CONTROLLER METHODS SHOULD BE ASYNCHRONIOUS, ali iz nekog razloga nece da mi da da ih napravim takve
        [AllowAnonymous] 
        // GET api/values
        [HttpGet]
        public IActionResult GetValues()    //IActionResult interfejs nudi i da se vrati Ok code za http odgovor
        {
            var values = _context.Values.ToList();  //ToList() da bi se executovala metoda i da bi uzeo iz db-a listu svih elemenata

            return Ok(values);  //http 200 ok response
        }

        [AllowAnonymous]    //ne treba token da bi se poslao zahtev za ovo
        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult GetValue(int id)
        {
            var value =  _context.Values.FirstOrDefault(x => x.Id == id); //nalazi element u db sa datim id-jem, a ako ne nadje, vraca null

            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
