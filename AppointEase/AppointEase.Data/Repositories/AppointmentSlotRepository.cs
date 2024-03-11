using AppointEase.Data.Contracts.Models;
using AppointEase.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Data.Repositories
{
    public class AppointmentSlotRepository : Repository<AppointmentSlot>
    {
        public AppointmentSlotRepository(AppointEaseContext context) : base(context)
        {
        }
    }
}
