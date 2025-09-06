﻿using Despesas.GlobalException.CustomExceptions.Core;
using Microsoft.AspNetCore.Http;

namespace Despesas.GlobalException.CustomExceptions.Acesso;

public class EmailException : CustomException
{
    public EmailException() : base("Erro ao enviar email de recuperação de senha!", StatusCodes.Status400BadRequest) { }
}