using AppointEase.Data.Contracts.Models;
using AppointEase.Data.Data;

namespace AppointEase.Data.Repositories
{
    public class ClinicRepository : Repository<TblClinic>
    {
        public ClinicRepository(AppointEaseContext context) : base(context)
        {
        }
    }
}
