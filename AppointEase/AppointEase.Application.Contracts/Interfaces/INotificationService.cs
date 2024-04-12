using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface INotificationService
    {
        Task<object> CreateNotification(NotificationRequest request);
        Task<object> GetAllNotifications();
        Task<bool> CheckIfExist(NotificationRequest notificationRequest);
        Task<IEnumerable<object>> GetSpecificNotifications(string id);
        Task<OperationResult> DeleteNotification(IEnumerable<string> ids);
        Task<OperationResult> UpdateNotification(string id,NotificationRequest notificationRequest);
        Task<string> GetSpecificNotification(NotificationRequest notificationRequest);
    }
}
