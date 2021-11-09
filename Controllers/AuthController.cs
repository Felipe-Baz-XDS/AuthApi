using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using teste.Repository;
using teste.Services;
using teste.ViewModel;

namespace teste.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [SwaggerOperation(Summary="Gera token de auth do usuario v1")] 
        [AllowAnonymous]
        public ActionResult<dynamic> Authenticate([FromBody] LoginView model)
        {
            // Recupera o usuário
            var user = UserRepository.Get(model.Username, model.Password);

            // Verifica se o usuário existe
            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            // Gera o Token
            var token = TokenService.GenerateToken(user);

            // Oculta a senha
            user.Password = "";

            // Retorna os dados
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpGet]
        [Route("anonymous")]
        [SwaggerOperation(Summary="teste de conecção anonima")]  
        [AllowAnonymous]
        public string Anonymous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [SwaggerOperation(Summary="teste de conecção authenticated")]  
        [Authorize]
        public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

        [HttpGet]
        [Route("employee")]
        [SwaggerOperation(Summary="teste de conecção authenticated como employee ou superior")]  
        [Authorize(Roles = "employee,manager")]
        public string Employee() => "Funcionário";

        [HttpGet]
        [Route("manager")]
        [SwaggerOperation(Summary="teste de conecção authenticated como manager")]  
        [Authorize(Roles = "manager")]
        public string Manager() => "Gerente";
    }
}