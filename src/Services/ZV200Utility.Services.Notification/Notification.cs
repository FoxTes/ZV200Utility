using System.Threading.Tasks;
using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;

namespace ZV200Utility.Services.Notification
{
    /// <inheritdoc />
    public class Notification : INotification
    {
        private readonly NotificationManager _notificationManager;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NotificationManager"/>.
        /// </summary>
        public Notification()
        {
            _notificationManager = new NotificationManager(NotificationPosition.BottomRight);
        }

        /// <inheritdoc />
        public Task ShowAsync(string title, string message, NotificationType notificationType)
        {
            return _notificationManager.ShowAsync(new NotificationContent
            {
                Title = title, Message = message, Type = notificationType
            });
        }
    }
}