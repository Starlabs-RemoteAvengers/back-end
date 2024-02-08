using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Models
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }
        public string? Name { get; set; }
        public string? Mbiemri { get; set; }
        public int Mosha { get; set; }
    }
}
