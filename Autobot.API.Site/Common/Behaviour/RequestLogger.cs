﻿using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.API.Site.Common.Behaviour
{
    public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        //private readonly ICurrentUserService _currentUserService;

        public RequestLogger(ILogger<TRequest> logger)//, ICurrentUserService currentUserService)
        {
            _logger = logger;
            //_currentUserService = currentUserService;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var name = typeof(TRequest).Name;

            _logger.LogInformation("Northwind Request: {Name} {@UserId} {@Request}",
                name, "", request);// _currentUserService.UserId, request);

            return Task.CompletedTask;
        }
    }
}
