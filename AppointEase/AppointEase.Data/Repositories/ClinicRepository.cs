using AppointEase.Data.Contracts.Models;
using AppointEase.Data.Data;

namespace AppointEase.Data.Repositories
{
    public class ClinicRepository : Repository<Clinic>
    {
        public ClinicRepository(AppointEaseContext context) : base(context)
        {
        }
    }
}
