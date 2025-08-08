using Serilog;

namespace CarMarketplace.Contracts.Logging
{
    public static class LogException
    {
        public static void LogExceptions(Exception ex)
        {
            LogToFile(ex.ToString());
            LogToConsole(ex.ToString());
            LogToDebugger(ex.ToString());
        }

        public static void LogToFile(string message) => Log.Information(message);
        public static void LogToConsole(string message) => Log.Warning(message);
        public static void LogToDebugger(string message) => Log.Debug(message);
    }
}