using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Refit;
using teste.Interfaces;
using teste.Models;

namespace teste.Services
{
    public class CorreioService
    {
        public Correios GetAddressAsync(string cep){
            var cepClient = RestService.For<ICepApiService>("https://viacep.com.br");
            var address = cepClient.GetAddressAsync(cep).Result;
            return address;
        }
    }
}