using AppointEase.Data.Contracts.Models;
using AppointEase.Data.Data;

namespace AppointEase.Data.Repositories
{
    public class DoctorReporsitory : Repository<Doctor>
    {
        public DoctorReporsitory(AppointEaseContext context) : base(context)
        {
        }
    }
}
