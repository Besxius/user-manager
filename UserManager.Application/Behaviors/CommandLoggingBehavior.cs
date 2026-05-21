using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;

namespace UserManager.Application.Behaviors
{
    public class CommandLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly ILogger<CommandLoggingBehavior<TRequest, TResponse>> _logger;

        public CommandLoggingBehavior(ILogger<CommandLoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var commandName = typeof(TRequest).Name;

            _logger.LogInformation("[START] Executing command: {CommandName}", commandName);

            var timer = Stopwatch.StartNew();

            try
            {
                var response = await next();
                timer.Stop();

                _logger.LogInformation("[SUCCESS] Command {CommandName} executed successfully in {ElapsedMilliseconds}ms",
                    commandName, timer.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                timer.Stop();
                _logger.LogError(ex, "[FAILED] Command {CommandName} failed after {ElapsedMilliseconds}ms",
                    commandName, timer.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
