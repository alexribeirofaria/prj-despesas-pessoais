﻿using Despesas.Application.Dtos;

namespace Despesas.Application.Abstractions;
public interface ISaldoBusiness
{
    Task<SaldoDto> GetSaldo(Guid idUsuario);
    Task<SaldoDto> GetSaldoAnual(DateTime ano, Guid idUsuario);
    Task<SaldoDto> GetSaldoByMesAno(DateTime mesAno, Guid idUsuario);
}
