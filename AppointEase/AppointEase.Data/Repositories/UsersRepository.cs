using AppointEase.Data.Contracts.Identity;
using AppointEase.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Data.Repositories
{
    public class UsersRepository : Repository<ApplicationUser>
    {
        public UsersRepository(AppointEaseContext context) : base(context)
        { }
    }
}
