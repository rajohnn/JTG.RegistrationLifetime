using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace JTG.RegistrationLifetime.Services
{
    public class FoobarMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        private readonly IOperationTransient _transientOperation;
        private readonly IOperationSingleton _singletonOperation;

        public FoobarMiddleware(RequestDelegate next, ILogger<FoobarMiddleware> logger,
            IOperationTransient transientOperation,
            IOperationSingleton singletonOperation)
        {
            _logger = logger;
            _transientOperation = transientOperation;
            _singletonOperation = singletonOperation;
            _next = next;
        }

        /// <summary>
        /// Scoped services must be resolved in the InvokeAsync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scopedOperation"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context,
            IOperationScoped scopedOperation)
        {
            _logger.LogInformation("Transient: " + _transientOperation.OperationId);
            _logger.LogInformation("Scoped: " + scopedOperation.OperationId);
            _logger.LogInformation("Singleton: " + _singletonOperation.OperationId);

            await _next(context);
        }
    }

    public static class FoobarMiddlewareExtensions
    {
        public static IApplicationBuilder UseFoobarMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FoobarMiddleware>();
        }
    }
}