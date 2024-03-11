using AppointEase.Data.Contracts.Models;
using AppointEase.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Data.Repositories
{
    public class BookAppointmentRepository : Repository<BookAppointment>
    {
        public BookAppointmentRepository(AppointEaseContext context) : base(context)
        {
        }
    }
}