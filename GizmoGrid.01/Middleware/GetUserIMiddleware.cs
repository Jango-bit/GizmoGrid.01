﻿using System.Security.Claims;

namespace GizmoGrid._01.Middleware
{
    public class GetUserIMiddleware
    {
        private readonly ILogger<GetUserIMiddleware> _logger;
        private readonly RequestDelegate _next;
        public GetUserIMiddleware(ILogger<GetUserIMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var idclaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

                if (idclaim != null)
                {
                    _logger.LogInformation($"NameIdentifier claim found: {idclaim.Value}");
                    if (Guid.TryParse(idclaim.Value, out var userId))
                    {
                        context.Items["UserId"] = userId;
                        _logger.LogInformation($"UserId successfully parsed: {userId}");
                    }
                    else
                    {
                        _logger.LogWarning($"NameIdentifier claim value '{idclaim.Value}' is not a valid integer.");
                    }
                }
                else
                {
                    _logger.LogWarning("NameIdentifier claim not found in the token.");
                }
            }
            else
            {
                _logger.LogInformation("User is not authenticated.");
            }
            await _next(context);
        }



    }
}
