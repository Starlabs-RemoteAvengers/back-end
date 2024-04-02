
using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Identity;
using AppointEase.Data.Contracts.Interfaces;
using AppointEase.Data.Contracts.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Frozen;

namespace AppointEase.Application.Services
{
    public class ConnectionRequestService : IConnectionRequestService
    {
        private readonly IRepository<ConnectionRequests> _connectionRequestsRepository;
        private readonly IMapper _mapper;
        private readonly IOperationResult _operationResult;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConnectionUserService _connectionUserService;

        public ConnectionRequestService(IRepository<ConnectionRequests> repository,IMapper mapper,IOperationResult operationResult,UserManager<ApplicationUser> userManager,IConnectionUserService connectionUserService)
        {
            this._connectionRequestsRepository = repository;
            this._mapper = mapper;
            this._operationResult = operationResult;
            this._userManager = userManager;
            this._connectionUserService = connectionUserService;
        }
        private enum TypeConnection
        {
            Pending,
            Accepted,
            NotExist,
            Waiting
        }


        public async Task<OperationResult> AcceptRequest(string Id)
        {
            try
            {
                var getAllConnections = await GetAllConnections();
                var getRequest = getAllConnections.Where(x => x.RequestId == Id).First();
                var mappedFirstRequest = _mapper.Map<ConnectionUserRequest>(getRequest);
                await _connectionUserService.AddConnection(mappedFirstRequest);


                //After firts Connection added then gona added connection for scond user to first user
                var IdTo = getRequest.FromId;
                var fromId = getRequest.ToId;

                var secondConnectionRequest = new ConnectionRequests()
                {
                    FromId = fromId,
                    ToId = IdTo,
                    dateTimestamp = DateTime.Now
                };
                var mappedSecondRequest = _mapper.Map<ConnectionUserRequest>(secondConnectionRequest);
                var result = await _connectionUserService.AddConnection(mappedSecondRequest);
                await DeleteRequest(getRequest.RequestId);

                return result;

            }
            catch (Exception ex)
            {
                return _operationResult.ErrorResult("Error: ", new[] { ex.Message }) ;
            }
        }

        public async Task<object> AddNewConnectionRequest(SendConnectionRequest sendConnectionRequest)
        {
            try
            {
                var mapped = _mapper.Map<ConnectionRequests>(sendConnectionRequest);
                await _connectionRequestsRepository.AddAsync(mapped);

                var getAll = await _connectionRequestsRepository.GetAllAsync();
                var getId = getAll.AsEnumerable().Where(r => r.FromId == sendConnectionRequest.FromId && r.ToId == sendConnectionRequest.ToId).First().RequestId;
                return new
                {
                    success = true,
                    message = "Your request connection has sent successfully!",
                    IdRequest = getId

                };
            }
            catch (Exception ex)
            {
                return _operationResult.ErrorResult("Error while sent connection:", new[] {ex.Message});
            }
        }

        public async Task<OperationResult> CancelRequest(string Id)
        {
            try
            {
                var allConnections = await GetAllConnections();
                var ifExist = allConnections.Where(x => x.RequestId == Id).Any();
                if (!ifExist)
                    return _operationResult.ErrorResult("Connection id: ", new[] { "This conneciton id doesn't exist, please check again!" });

                await _connectionRequestsRepository.DeleteAsync(Id);

                return _operationResult.SuccessResult("Your request has cancel successfully!");
            }
            catch (Exception ex)
            {
                return _operationResult.ErrorResult("Error while cancel connection:", new[] { ex.Message });
            }


        }

        public async Task<object> CheckConnectionIfExist(string userId, string doctorId)
        {
            try
            {
                var connectionRequests = await _connectionRequestsRepository.GetAllAsync();
                var connections = await _connectionUserService.GetAllConnections();

                var isFriends = await _connectionUserService.CheckIfExcist(userId, doctorId);
                var isConnectionExist = isFriends is bool exist && exist;

                var hasSpecificRequest = connectionRequests.Any(r => r.FromId == userId && r.ToId == doctorId);
                var hasWaitingRequest = connectionRequests.Any(r => r.FromId == doctorId && r.ToId == userId);

                var connectionType = TypeConnection.NotExist;
                string requestId = null;

                if (isConnectionExist)
                {
                    connectionType = TypeConnection.Accepted;
                    requestId = connections.FirstOrDefault(r => r.FromId == userId && r.ToId == doctorId)?.ConnectionId;

                }
                else if (!isConnectionExist && hasSpecificRequest)
                {
                    connectionType = TypeConnection.Pending;
                    requestId = connectionRequests.FirstOrDefault(r => r.FromId == userId && r.ToId == doctorId)?.RequestId;
                }
                else if (hasWaitingRequest)
                {
                    connectionType = TypeConnection.Waiting;
                    requestId = connectionRequests.FirstOrDefault(r => r.FromId == doctorId && r.ToId == userId)?.RequestId;
                }

                if (connectionType == TypeConnection.NotExist || string.IsNullOrEmpty(requestId))
                {
                    return new
                    {
                        Type = TypeConnection.NotExist.ToString(),
                        IdRequest = string.Empty // Ose mund të lini IdRequest null
                    };
                }

                return new
                {
                    Type = connectionType.ToString(),
                    IdRequest = requestId
                };
            }
            catch (Exception ex)
            {
                // Kthimi i një mesazhi të gabimit në rast se ka një gabim në ekzekutimin e metodës
                return _operationResult.ErrorResult("Error while checking connection").Errors;
            }
        }

        public async Task<IEnumerable<object>> GetAllRequestForSpecificUser(string Id)
        {
            try
            {
                var getAllRequest = await _connectionRequestsRepository.GetAllAsync();
                var listOfUsers = getAllRequest.AsEnumerable().Where(x => x.ToId == Id).ToList();
                var tasks = listOfUsers.Select(async u =>
                {
                    var userData = await _userManager.FindByIdAsync(u.FromId);

                    return new
                    {
                        userId = userData.Id,
                        connectionId = u.RequestId,
                        fullName = userData.Name + " " + userData.Surname,
                        photo = userData.PhotoData,
                        photoFormat = userData.PhotoFormat,
                        date = u.dateTimestamp,
                        role = userData.Role
                    };
                });

                var userDetails = await Task.WhenAll(tasks);
                return userDetails.ToList();

            }
            catch (Exception ex)
            {
                return _operationResult.ErrorResult("Error while cancel connection").Errors;
            }

        }

        public async Task<OperationResult> DeleteRequest(string Id)
        {
            try
            {
                var allConnections = await GetAllConnections();
                var ifExist = allConnections.Where(x => x.RequestId == Id).Any();
                if (!ifExist)
                    return _operationResult.ErrorResult("Connection id: ",new[] {"This conneciton id doesn't exist, please check again!"});

                var result = await _connectionRequestsRepository.DeleteAsync(Id);
                return _operationResult.SuccessResult();
            }
            catch (Exception ex)
            {
                return _operationResult.ErrorResult("Error while cancel connection");
            }
        }

        public async Task<IEnumerable<object>> GetOtherUsers(string currentUser)
        {
            try
            {
                var allUsers = _userManager.Users;

                var connections = await _connectionUserService.GetAllConnections(currentUser);

                var connectedUserIds = connections.Select(connection => ((dynamic)connection).userId);
                
                var otherUsers = allUsers.Where(user => !connectedUserIds.Contains(user.Id) && user.Role != "Clinic" && user.Id != currentUser);


                return otherUsers.Select(user => new
                {
                    userId = user.Id,
                    fullName = user.Name + " " + user.Surname,
                    photo = user.PhotoData,
                    photoFormat = user.PhotoFormat,
                    role = user.Role
                });

            }
            catch (Exception ex)
            {
                return _operationResult.ErrorResult("Error while cancel connection").Errors;
            }
        }

        public async Task<IEnumerable<SendConnectionRequest>> GetAllConnections()
        {
            var allObjects = await _connectionRequestsRepository.GetAllAsync();
            var mappedObjects = allObjects.Select(obj => _mapper.Map<SendConnectionRequest>(obj)).ToList();
            return mappedObjects;
        }
    }
}
