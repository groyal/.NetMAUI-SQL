using BeebopNoteApp.Domain.Interfaces;
using BeebopNoteApp.Domain.Models;
using BeebopNoteApp.Infrastructure.Services;
using BeebopNoteApp.Views.UrgentMessage;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeebopNoteApp.ViewModels
{
    public class UrgentMessageViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public UrgentMessageViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public UrgentMessageViewModel Create(Window owner, AppNotificationModel notification)
        {
            return new UrgentMessageViewModel(
                owner,
                notification,
                _serviceProvider.GetRequiredService<IRepository>(),
                _serviceProvider.GetRequiredService<NovuService>(),
                _serviceProvider.GetRequiredService<IAppLogger>());
        }
    }
}
