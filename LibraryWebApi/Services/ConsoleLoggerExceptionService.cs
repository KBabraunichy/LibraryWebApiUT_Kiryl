using LibraryWebApi.Interfaces;

namespace LibraryWebApi.Services
{
    public class ConsoleLoggerExceptionService : ILoggerException
    {
        public void ExceptionInfo(Exception exception)
        {

            Console.WriteLine($"\nException: {exception.Message}");
            if (exception.InnerException is not null)
                Console.WriteLine($"InnerException: {exception.InnerException.Message}");

            Console.WriteLine($"Method: {exception.TargetSite}");
            Console.WriteLine($"StackTrace: {exception.StackTrace}");
        }
    }
}
