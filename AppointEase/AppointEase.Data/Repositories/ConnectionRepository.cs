using AppointEase.Data.Contracts.Models;
using AppointEase.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Data.Repositories
{
    public class ConnectionRepository : Repository<Connections>
    {
        public ConnectionRepository(AppointEaseContext context) : base(context)
        {
        }
    }
}
