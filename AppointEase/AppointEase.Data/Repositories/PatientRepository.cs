using AppointEase.Data.Contracts.Models;
using AppointEase.Data.Data;
using Microsoft.EntityFrameworkCore;


namespace AppointEase.Data.Repositories
{
    public class PatientRepository : Repository<Patient>
    {
        public PatientRepository(AppointEaseContext context) : base(context)
        {
        }
    }
}
