using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using teste.Data;
using teste.Models;
using teste.Services;
using teste.ViewModel;

namespace teste.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class ClientController : ControllerBase
    {
        private AppDbContext _context;
        private CorreioService _correioService = new CorreioService();

        public ClientController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "employee,manager")]
        public async Task<IActionResult> GetClientAsync()
        {
            var clients = await _context
                .Clients
                .AsNoTracking()
                .ToListAsync();
            return Ok(clients);
        }
        
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "employee,manager")]
        public async Task<IActionResult> GetClientById([FromRoute] int id)
        {
            var client = await _context
                .Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (client == null)
                return NotFound();
            
            return Ok(client);
        }

        [HttpPost]
        [Authorize(Roles = "employee,manager")]
        public async Task<IActionResult> PostAsync([FromBody] CreateClientViewModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest();
            
            var address = _correioService.GetAddressAsync(model.Cep);
            var client = new Client{
                Nome = model.Nome,
                Cep = model.Cep,

                Logradouro = address.Logradouro,
                Complemento = address.Complemento,
                Bairro = address.Bairro,
                Localidade = address.Localidade,
                Ibge = address.Ibge,
                Gia = address.Gia,
                Ddd = address.Ddd,
                Siafi = address.Siafi
            };

            try
            {
                await _context
                    .Clients
                    .AddAsync(client);
                await _context.SaveChangesAsync();
                return Created(uri:"v1/clients/{client.Id}", client);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "employee,manager")]
        public async Task<IActionResult> PutAsync(
            [FromBody] CreateClientViewModel model,
            [FromRoute] int id
        ){
            if(!ModelState.IsValid)
                return BadRequest();
            
            var client = await _context
                .Clients
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (client == null)
                return NotFound();
            
            try
            {
                client.Nome = model.Nome;
                client.Cep = model.Cep;

                var address = _correioService.GetAddressAsync(model.Cep);
                client.Logradouro = address.Logradouro;
                client.Complemento = address.Complemento;
                client.Bairro = address.Bairro;
                client.Localidade = address.Localidade;
                client.Ibge = address.Ibge;
                client.Gia = address.Gia;
                client.Ddd = address.Ddd;
                client.Siafi = address.Siafi;

                _context.Update(client);
                await _context.SaveChangesAsync();

                return Ok(client);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "employee,manager")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id
        ){
            var client = await _context
            .Clients
            .FirstOrDefaultAsync(x => x.Id == id);

            if(client == null)
                return NotFound();

            try
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}