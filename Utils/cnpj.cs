using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace teste.Utils
{
    public class cnpj
    {
        public bool Validate(string cnpj)
        {
            //inicializa os multiplicadores
            int[] multiplicador1 = new int[12] {5,4,3,2,9,8,7,6,5,4,3,2};
			int[] multiplicador2 = new int[13] {6,5,4,3,2,9,8,7,6,5,4,3,2};
            List<int> cnpj_num = new List<int>();

            if (cnpj.Length != 18 && cnpj.Length != 14)
                return false;

            if (cnpj.Length == 18)
            {
                cnpj = cnpj.Trim();
                cnpj = cnpj.Replace(".", "").Replace("-","").Replace("/", "");

            }else if (cnpj.Length == 14)
            {
                cnpj = cnpj.Trim();
            }
            int soma = 0;
            int resto = 0;
            string digito;
            string AuxCnpj;

            if (cnpj.Length != 14)
                return false;
            
            //first digit
            AuxCnpj = cnpj.Substring(0,12);
            for (int i = 0; i < 12; i++)
                soma += int.Parse(AuxCnpj[i].ToString())*multiplicador1[i];
            
            resto = (soma % 11);
            if (resto  < 2)
                resto = 0;
            else
                resto = 11-resto;
            
            digito = resto.ToString();
            AuxCnpj += digito;

            soma = 0;
            //last digit
            for (int i = 0; i < 13; i++)
                soma += int.Parse(AuxCnpj[i].ToString())*multiplicador2[i];
            resto = (soma % 11);
            if (resto  < 2)
                resto = 0;
            else
                resto = 11-resto;
            digito = resto.ToString();
            AuxCnpj += digito;

            if(AuxCnpj.ToString() != cnpj)
                return false;
            else
                return true;
        }
    }
}