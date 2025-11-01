using BeebopNoteApp.Domain.Interfaces;
using BeebopNoteApp.Domain.Models;
using BeebopNoteApp.ViewModels;
using log4net;
namespace BeebopNoteApp.Views.UrgentMessage;

public partial class UrgentMessageView : Window
{
    private readonly IAppLogger _logger;

    public UrgentMessageView(
        AppNotificationModel notification,
        UrgentMessageViewModelFactory viewModelFactory,
        IAppLogger logger)
    {
        //InitializeComponent();
        _logger = logger;

       // DataContext = viewModelFactory.Create(this, notification);
        _logger.Debug<UrgentMessageView>($"UrgentMessageView constructor called with message: {notification.Message}");
    }
}