using AppointEase.Application.Contracts.Models.DbModels;
using AppointEase.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Data.Repositories
{
    public class ClinicRepository : Repository<TblClinic>
    {
        public ClinicRepository(AppointEaseContext context) : base(context)
        {
        }
    }
}
