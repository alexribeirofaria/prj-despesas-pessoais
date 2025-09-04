﻿using Microsoft.AspNetCore.Builder;

namespace Despesas.GlobalException.CommonDependenceInject;

public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
