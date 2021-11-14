using System;

namespace Recruiter.Infrastructure.Logger
{
    /// <summary>
    /// This type eliminates the need to depend directly on the ASP.NET Core logging types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAppLogger<T>
    {
        void LogError(string message, params object[] args);
        void LogError(Exception exception, string message, params object[] args);
        void Log(string message, params object[] args);
    }
}
