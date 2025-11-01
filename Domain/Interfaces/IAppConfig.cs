namespace BeebopNoteApp.Domain.Interfaces;

public interface IAppConfig
{
    string DbConnectionString { get; }
    string NovuApiKey { get; }
    string NovuUrl { get; }
    string NovuFirstName { get; }
    string NovuLastName { get; }
    string NovuEmail { get; }
}