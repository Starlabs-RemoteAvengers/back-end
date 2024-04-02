using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface IConnectionRequestService
    {
        Task<object> AddNewConnectionRequest(SendConnectionRequest sendConnectionRequest);
        Task<OperationResult> CancelRequest(string Id);
        Task<OperationResult> AcceptRequest(string Id);
        Task<IEnumerable<object>> GetAllRequestForSpecificUser(string Id);
        Task<IEnumerable<SendConnectionRequest>> GetAllConnections();
        Task<IEnumerable<object>> GetOtherUsers(string currentUser);
        Task<OperationResult> DeleteRequest(string Id);
        Task<object> CheckConnectionIfExist(string UserId,string DoctorId);
    }

}
