using AppointEase.Data.Contracts.Models;
using AppointEase.Data.Data;

namespace AppointEase.Data.Repositories
{
    public class UserRepository : Repository<TblPacient>
    {
        public UserRepository(AppointEaseContext context) : base(context)
        {
        }
    }
}
