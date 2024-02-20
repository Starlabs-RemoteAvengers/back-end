using AppointEase.Data.Contracts.Models;
using AppointEase.Data.Data;

namespace AppointEase.Data.Repositories
{
    public class AdminRepository : Repository<TblAdmin>
    {
        public AdminRepository(AppointEaseContext context) : base(context)
        {
        }
    }
}
