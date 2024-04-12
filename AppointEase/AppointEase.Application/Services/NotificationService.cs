using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Identity;
using AppointEase.Data.Contracts.Interfaces;
using AppointEase.Data.Contracts.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AppointEase.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notifications> _notificationRepository;
        private readonly IOperationResult _result;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationService(IRepository<Notifications> repository,IOperationResult operationResult, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this._notificationRepository = repository;
            this._result = operationResult;
            this.mapper = mapper;
            this._userManager = userManager;
        }

        public async Task<bool> CheckIfExist(NotificationRequest notificationRequest)
        {
            var allNotifications = await GetAllNotifications() as IEnumerable<NotificationRequest>;
            if (allNotifications != null)
            {
                return allNotifications.Where(n => n.FromId == notificationRequest.FromId && n.ToId == notificationRequest.ToId && n.Type == notificationRequest.Type).Any();
            }
            return false;
        }

        public async Task<object> CreateNotification(NotificationRequest request)
        {
            try
            {
                bool ifExist = await CheckIfExist(request);
                if (ifExist)
                {
                    var notificationId = await GetSpecificNotification(request);
                    var newObject = request;
                    newObject.IdNotification = notificationId;
                    newObject.dateTimestamp = DateTime.Now;
                    newObject.IsRead = false;

                    await UpdateNotification(newObject.IdNotification,newObject);
                    return _result.SuccessResult();
                }

                var notification = mapper.Map<Notifications>(request);
                await _notificationRepository.AddAsync(notification);
                return _result.SuccessResult();
            }
            catch (Exception ex)
            {
                return _result.ErrorResult($"Failed create notification:", new[] { ex.Message });
            }
        }

        public async Task<OperationResult> DeleteNotification(IEnumerable<string> ids)
        {
            try
            {
                if (ids.Count() == 0)
                    return _result.ErrorResult("Please select notification to delete!", new string[0]);

                foreach (var item in ids)
                {
                    await _notificationRepository.DeleteAsync(item);
                }
                return _result.SuccessResult();
            }
            catch (Exception ex)
            {
                return _result.ErrorResult($"Failed delete notification:", new[] { ex.Message });
            }

        }

        public async Task<object> GetAllNotifications()
        {
            try
            {
                var allNotifications = await _notificationRepository.GetAllAsync();
                var mappedNotification = allNotifications.Select(d => mapper.Map<NotificationRequest>(d));
                return mappedNotification;
            }
            catch (Exception ex)
            {
                return _result.ErrorResult($"Failed to get notification:", new[] { ex.Message });
            }
        }
        public async Task<string> GetSpecificNotification(NotificationRequest notificationRequest)
        {
            var allNotifications = await GetAllNotifications() as IEnumerable<NotificationRequest>;
            if (allNotifications != null)
            {
                return allNotifications.FirstOrDefault(n => n.FromId == notificationRequest.FromId && n.ToId == notificationRequest.ToId && n.Type == notificationRequest.Type && n.dateTimestamp == notificationRequest.dateTimestamp).IdNotification;
            }
            return "";
        }

        public async Task<IEnumerable<object>> GetSpecificNotifications(string id)
        {
            try
            {
                var allNotifications = await  GetAllNotifications() as IEnumerable<NotificationRequest>;
                if (allNotifications != null)
                {
                    var getNotificationForUser = allNotifications
                         .Where(n => n.ToId.Contains(id))
                         .Select(async x =>
                         {
                             var user = await _userManager.FindByIdAsync(id);

                             var notification = new
                             {
                                 IdNotification = x.IdNotification,
                                 FromId = x.FromId,
                                 ToId = x.ToId,
                                 Type = x.Type,
                                 TypeId = x.IdType,
                                 Subject = x.Subject,
                                 Body = x.Body,
                                 Read = x.IsRead,
                                 MessageType = x.MessageType,
                                 Date = Convert.ToDateTime(x.dateTimestamp).Date.ToString("dd MMM, yyyy"),
                                 Time = Convert.ToDateTime(x.dateTimestamp).ToString("HH:mm"),
                                 DateTimestamp = x.dateTimestamp,
                                 FullName = user.Name +" "+ user.Surname,
                             };

                             return notification;
                         });

                    var notificationList = await Task.WhenAll(getNotificationForUser);

                    return notificationList;
                }
                return null;
            }
            catch (Exception ex)
            {
                return _result.ErrorResult($"Failed to get notification:", new[] { ex.Message }).Errors;
            }
        }

        public async Task<OperationResult> UpdateNotification(string id, NotificationRequest notificationRequest)
        {
            try
            {
                var existingNotification = await _notificationRepository.GetByIdAsync(id);

                if (existingNotification == null)
                {
                    return _result.ErrorResult("Notification not found.", new string[0]);
                }

                var notification = mapper.Map(notificationRequest, existingNotification);
                await _notificationRepository.UpdateAsync(notification);
                return _result.SuccessResult();
            }
            catch (Exception ex)
            {
                return _result.ErrorResult($"Failed to update:", new[] { ex.Message });
            }
        }
    }
}
