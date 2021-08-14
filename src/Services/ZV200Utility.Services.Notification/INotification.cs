using System.Threading.Tasks;
using Notifications.Wpf.Core;

namespace ZV200Utility.Services.Notification
{
    /// <summary>
    /// Предоставляет сервис для возможности отправки клиенту уведомлений.
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// Показывает всплывающие уведомление.
        /// </summary>
        /// <param name="title">Заголовок.</param>
        /// <param name="message">Текст уведомления.</param>
        /// <param name="notificationType">Тип уведомления.</param>
        Task ShowAsync(string title, string message, NotificationType notificationType);
    }
}