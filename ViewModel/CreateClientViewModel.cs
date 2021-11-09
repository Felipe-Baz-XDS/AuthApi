using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace teste.ViewModel
{
    public class CreateClientViewModel
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Cep { get; set; }
    }
}