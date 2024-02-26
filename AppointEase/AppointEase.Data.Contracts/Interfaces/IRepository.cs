using AppointEase.Application.Contracts.Models;
using AppointEase.Application.Contracts.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Data.Contracts.Interfaces
{
    public interface IRepository <T>
    {
        Task<string> GetIdByEmailAndPasswordAsync(string email, string password);
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);  
    }
}
