using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Refit;
using teste.Models;

namespace teste.Interfaces
{
    public interface ICepApiService
    {
        [Get("/ws/{cep}/json")]
        Task<Correios> GetAddressAsync(string cep);
    }
}