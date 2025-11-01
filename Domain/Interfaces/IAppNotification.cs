using System.Threading.Tasks;

namespace BeebopNoteApp.Domain.Interfaces;

public interface IAppNotification
{
    Task Show(Models.AppNotificationModel notification);
}