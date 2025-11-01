using System;

namespace BeebopNoteApp.Domain.Interfaces;

public interface IAppLogger
{
    void Debug<T>(object? message, Exception? exception = null) where T : class;
    void Info<T>(object? message, Exception? exception = null) where T : class;
    void Warn<T>(object? message, Exception? exception = null) where T : class;
    void Error<T>(object? message, Exception? exception = null) where T : class;
}