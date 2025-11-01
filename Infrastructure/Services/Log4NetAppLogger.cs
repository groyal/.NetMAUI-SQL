using log4net;
using System;
using System.Collections.Generic;
using BeebopNoteApp.Domain.Interfaces;

namespace BeebopNoteApp.Infrastructure.Services;

public class Log4NetAppLogger : IAppLogger
{
    private readonly IDictionary<Type, ILog> _loggerContainer = new Dictionary<Type, ILog>();

    public void Debug<T>(object? message, Exception? exception = null) where T : class
    {
        Logger<T>().Debug(message, exception);
    }

    public void Error<T>(object? message, Exception? exception = null) where T : class
    {
        Logger<T>().Error(message, exception);
    }

    public void Info<T>(object? message, Exception? exception = null) where T : class
    {
        Logger<T>().Info(message, exception);
    }

    public void Warn<T>(object? message, Exception? exception = null) where T : class
    {
        Logger<T>().Warn(message, exception);
    }

    private ILog Logger<T>()
    {
        Type type = typeof(T);

        if (!_loggerContainer.TryGetValue(type, out ILog? value))
        {
            value = LogManager.GetLogger(type);
            _loggerContainer[type] = value;
        }

        return value;
    }
}