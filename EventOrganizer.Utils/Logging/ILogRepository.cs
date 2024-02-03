namespace EventOrganizer.Utils.Logging
{
    public interface ILogRepository
    {
        Task SaveLog(LogRecord logRecord);
    }
}
