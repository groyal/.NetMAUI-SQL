using BeebopNoteApp.Domain.Interfaces;
using BeebopNoteApp.Domain.Models;
using BeebopNoteApp.ViewModels;
using BeebopNoteApp.Views.UrgentMessage;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BeebopNoteApp.Infrastructure.Services;

public class UrgentMessageViewFactory
{
    private readonly IServiceProvider _serviceProvider;

    public UrgentMessageViewFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public UrgentMessageView Create(AppNotificationModel notification)
    {
        return new UrgentMessageView(
            notification,
            _serviceProvider.GetRequiredService<UrgentMessageViewModelFactory>(),
            _serviceProvider.GetRequiredService<IAppLogger>());
    }
}