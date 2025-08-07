﻿using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Despesas.WebApi.Controllers.Abstractions;


[ApiController]
[Route("v{version:apiVersion}/[controller]")]
public abstract class UnitControllerBase : ControllerBase
{
    protected UnitControllerBase() { }
    protected Guid UserIdentity
    {
        get
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = HttpContext.Request.Headers.Authorization.ToString();
                var jwtToken = tokenHandler.ReadToken(token.Replace("Bearer ", "")) as JwtSecurityToken;
                var userId = jwtToken?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value;
                return new Guid(userId);
            }
            catch
            {
                return Guid.Empty;
            }
        }
    }
}