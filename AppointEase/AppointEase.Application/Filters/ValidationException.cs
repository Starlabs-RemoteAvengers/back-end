using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Filters
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; set; }
        public ValidationException(List<string> errors) : base("Validation failed")
        {
            Errors = errors;
        }
    }
}
