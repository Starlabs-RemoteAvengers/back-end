using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Interfaces;
using AppointEase.Data.Contracts.Models;
using AppointEase.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointEase.Data.Repositories
{
    public class Repository <T>: IRepository<T> where T : class
    {
        private readonly AppointEaseContext _context;

        public Repository(AppointEaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<string> GetIdByEmailAndPasswordAsync(string email, string password)
        {
            var user = await _context.Set<T>().FirstOrDefaultAsync(u =>
            EF.Property<string>(u, "Email") == email && EF.Property<string>(u, "Password") == password);

            if (user != null)
            {
                var userIdProperty = user.GetType().GetProperty("UserId");
                if (userIdProperty != null)
                {
                    var userId = userIdProperty.GetValue(user, null)?.ToString();
                    return userId;
                }
            }

            return null;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<OperationResult> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();

            return new OperationResult(true, "Added Successfully");
        }

        public async Task<OperationResult> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return new OperationResult(true, "Updated Successfully");
        }

        public async Task<OperationResult> DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();

           return new OperationResult(true, "Entity deleted successfully");
        }
        public async Task<IEnumerable<T>> GetDoctorsByClinicId(string clinicId)
        {
            // Assuming you have a DbSet<Doctor> named Doctors in your DbContext
            return (IEnumerable<T>)await Task.FromResult(_context.Doctor.Where(d => d.ClinicId == clinicId).ToList());
        }
    }
}
