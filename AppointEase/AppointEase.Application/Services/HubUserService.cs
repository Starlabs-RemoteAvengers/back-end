using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Hub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Services
{
    public class HubUserService : IHubUserService
    {
        private static List<HubUser> _hubUsers = new List<HubUser>();

        public HubUserService()
        {
        }

        public async Task AddUserToHub(HubUser user)
        {
            _hubUsers.Add(user);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<HubUser>> GetAllUsers()
        {
            return _hubUsers;
        }

        public async Task RemoveUser(string userId)
        {
            var userToRemove = _hubUsers.FirstOrDefault(u => u.UserId == userId);
            if (userToRemove != null)
            {
                _hubUsers.Remove(userToRemove);
            }

            await Task.CompletedTask;
        }
    }
}
