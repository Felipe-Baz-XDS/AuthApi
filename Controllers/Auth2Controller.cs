using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using teste.Data;
using teste.Models;
using teste.Services;
using teste.ViewModel;

namespace teste.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v2/[controller]")]
    [Produces("application/json")]
    public class Auth2Controller : ControllerBase
    {

        private AppDbContext _context;

        public Auth2Controller(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        [SwaggerOperation(Summary="Gera token de auth do usuario v2")] 
        [AllowAnonymous]
        public async Task<dynamic> Authenticate([FromBody] LoginView model)
        {
            var user = await _context
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == model.Username && x.Password == model.Password);
            
            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });
            
            var token = TokenService.GenerateToken(user);
            user.Password = "";

            return new
            {
                user = user,
                token = token
            };
        }

        [HttpGet]
        [SwaggerOperation(Summary="Mostra a lista de users")] 
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _context
                .Users
                .AsNoTracking()
                .ToListAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary="Mostra a 1 user escolhido pelo Id")] 
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] int id)
        {
            var user = await _context
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if(user == null)
                return NotFound();

            return Ok(user);
        }
        
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [SwaggerOperation(Summary="Adiciona user")]  
        [Authorize(Roles = "manager")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] CreateUserViewModel model){
            if(!ModelState.IsValid)
                return BadRequest();
            
            if(!ValidaUserService.isValidModel(model))
            {
                return BadRequest();
            }

            var user = new User{
                Username = model.Username,
                Password = model.Password,
                Role = model.Role
            };

            try
            {
                await _context
                    .Users
                    .AddAsync(user);
                await _context.SaveChangesAsync();
                return Created(uri:"v2/users/{user.Id}", user);
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(Summary="Edita user")]  
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> PutAsync(
            [FromBody] CreateUserViewModel model,
            [FromRoute] int id
        ){
            if (!ModelState.IsValid)
                return BadRequest();
            
            var user = await _context
                .Users
                .FirstOrDefaultAsync(x=>x.Id == id);
            
            if(user == null)
                return NotFound();
            
            if(!ValidaUserService.isValidModel(model))
                return BadRequest();

            try
            {
                user.Username = model.Username;
                user.Password = model.Password;
                user.Role = model.Role;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                user.Password = "";
                return Ok(user);
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(Summary="Deleta user")]  
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id
        ){
            var user = await _context
                .Users
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if(user == null)
                return NotFound();

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }
    }
}
