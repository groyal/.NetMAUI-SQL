using BeebopNoteApp.Domain.Interfaces;

namespace BeebopNoteApp.Infrastructure.Services
{
    public interface IPlatformServiceFactory
    {
        IPlatformFileService CreateFileService();
        // Add other platform services as needed
    }
}
