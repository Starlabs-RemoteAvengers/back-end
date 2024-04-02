using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface IConnectionUserService
    {
        Task<OperationResult> AddConnection(ConnectionUserRequest connectionRequest);
        Task<IEnumerable<object>> GetAllConnections(string userId);
        Task<IEnumerable<ConnectionUserRequest>> GetAllConnections();
        Task<OperationResult> DeleteConnection(string userId, string friendId);
        Task<object> CheckIfExcist(string UserId, string DoctorId);

    }
}
