using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventOrganizer.Utils.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        private readonly IServiceScopeFactory serviceProvider;

        public CustomLoggerProvider(IServiceScopeFactory serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>  
        /// Creates a new instance of the db logger.  
        /// </summary>  
        /// <param name="categoryName"></param>  
        /// <returns></returns>  
        public ILogger CreateLogger(string categoryName)
        {
            return new CustomLogger(serviceProvider, categoryName);
        }

        public void Dispose()
        {
        }
    }
}
