using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace teste.Models
{
    public class ErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public ErrorResponse InnerError { get; set; }

        public static ErrorResponse From(Exception e)
        {
            if (e == null)
                return null;
                
            return new ErrorResponse
            {
                Code = e.HResult,
                Message = e.Message,
                InnerError = From(e.InnerException)
            };
        }
    }
}