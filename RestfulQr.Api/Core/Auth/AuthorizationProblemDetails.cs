using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RestfulQr.Api.Core.Auth
{
    public static class AuthorizationProblemDetails
    {
        public static ProblemDetails ForbiddenProblemDetails(string? details = null)
        {
            return new ProblemDetails 
            {
                Title = "Unauthorized",
                Detail = details,
                Status = 401,
                Type = "https://httpstatuses.com/401"
            };
        }

        public static ProblemDetails UnauthorizedProblemDetails(string? details = null)
        {
            return new ProblemDetails 
            {
                Title = "Unauthorized",
                Detail = details,
                Status = 401,
                Type = "https://httpstatuses.com/401"
            };
        }
    }
}