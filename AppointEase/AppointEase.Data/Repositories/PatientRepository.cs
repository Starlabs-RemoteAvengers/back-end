using AppointEase.Application.Contracts.ModelsRespond;
using AppointEase.Data.Contracts.Models;
using AppointEase.Data.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace AppointEase.Data.Repositories
{
    public class PatientRepository : Repository<Patient>
    {
        private readonly AppointEaseContext _context;
        public PatientRepository(AppointEaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(PatientResponse entity)
        {
            // Implement the update logic here using your database context
            // You may need to map the properties from PatientResponse to Patient

            var patientToUpdate = await _context.Set<Patient>().FindAsync(entity.Id);

            if (patientToUpdate != null)
            {
                // Update patient properties using data from PatientResponse
                patientToUpdate.Gender = entity.Gender;
                patientToUpdate.Description = entity.Description;

                _context.Entry(patientToUpdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

    }
}
