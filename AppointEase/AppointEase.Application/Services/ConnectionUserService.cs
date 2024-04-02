using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Identity;
using AppointEase.Data.Contracts.Interfaces;
using AppointEase.Data.Contracts.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace AppointEase.Application.Services
{
    public class ConnectionUserService : IConnectionUserService
    {
        private readonly IRepository<Connections> _connectionRepository;
        private readonly IMapper _mapper;
        private IOperationResult operationResult;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConnectionUserService(IRepository<Connections> repository,IMapper mapper,IOperationResult operationResult,UserManager<ApplicationUser> userManager)
        {
            _connectionRepository = repository;
            _mapper = mapper;
            this.operationResult = operationResult;
            _userManager = userManager;
        }
        public async Task<OperationResult> AddConnection(ConnectionUserRequest ConnectionRequest)
        {
            try
            {
                var mapped = _mapper.Map<Connections>(ConnectionRequest);

                var result = await _connectionRepository.AddAsync(mapped);
                if (result.Succeeded)
                    return operationResult.SuccessResult("User added successfully in your connections!");
                else
                    return operationResult.ErrorResult("Error while Accept Connection");

            }
            catch (Exception ex)
            {
                return operationResult.ErrorResult("Error: ", new[] { ex.Message });
            }
        }

        public async Task<object> CheckIfExcist(string UserId, string DoctorId)
        {
            try
            {
                var getAllRequest = await _connectionRepository.GetAllAsync();
                var getSpecificRequest = getAllRequest.Where(r => (r.FromId == UserId && r.ToId == DoctorId) || (r.FromId == DoctorId && r.ToId == UserId)).Any();
                return getSpecificRequest;
            }
            catch (Exception ex)
            {
                return operationResult.ErrorResult("Error: ", new[] { ex.Message });
            }
        }

        public async Task<OperationResult> DeleteConnection(string userId,string friendId)
        {
            var getAllRequest = await _connectionRepository.GetAllAsync();
            var specificConnections = getAllRequest.AsEnumerable().Where(c => c.FromId == userId && c.ToId == friendId || c.FromId == friendId && c.ToId == userId).Select(s => s.ConnectionId).ToList();

            foreach (var connecitonItem in specificConnections)
            {
                await _connectionRepository.DeleteAsync(connecitonItem);
            }

            return operationResult.SuccessResult();
        }


        public async Task<IEnumerable<object>> GetAllConnections(string userId)
        {
            try
            {
                var getAllRequest = await _connectionRepository.GetAllAsync();
                var getUserConnections = getAllRequest.AsEnumerable()
                    .Where(r => (r.FromId == userId || r.ToId == userId))
                    .ToList();


                var getUserDetails = new List<object>();

                foreach (var connection in getUserConnections)
                {
                    string selectedUserId = (userId == connection.FromId ? connection.ToId : connection.FromId);
                    var userData = await _userManager.FindByIdAsync(selectedUserId);
                    if (!getUserDetails.Any(x => ((dynamic)x).userId == selectedUserId))
                    {
                        getUserDetails.Add(new
                        {
                            userId = userData.Id,
                            connectionId = connection.ConnectionId,
                            fullName = userData.Name + " " + userData.Surname,
                            photo = userData.PhotoData,
                            photoFormat = userData.PhotoFormat,
                            date = connection.dateTimestamp,
                            role = userData.Role
                        });
                    }

                }

                return getUserDetails;
            }
            catch (Exception ex)
            {
                return operationResult.ErrorResult("Error: ", new[] { ex.Message }).Errors;
            }
        }

        public async Task<IEnumerable<ConnectionUserRequest>> GetAllConnections()
        {
            var allObjects = await _connectionRepository.GetAllAsync();
            var mappedObjects = allObjects.Select(obj => _mapper.Map<ConnectionUserRequest>(obj)).ToList();
            return mappedObjects;
        }
    }
}
