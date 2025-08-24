﻿using Despesas.Application.Dtos.Core;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Despesas.Application.Dtos;
public class ReceitaDto : BaseDto
{
    [Required(ErrorMessage = "O campo Data é obrigatório.")]
    public DateTime? Data { get; set; }

    [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "O campo Valor é obrigatório.")]
    public decimal Valor { get; set; }

    [JsonIgnore]
    public Guid? IdCategoria
    {
        get
        {
            return Categoria?.Id;
        }
        set
        {
            if (value != null && Categoria != null)
            {
                Categoria.Id = value.Value;
            }
        }
    }

    [Required(ErrorMessage = "A Categoria é obrigatória.")]
    public CategoriaDto? Categoria { get; set; } = new();
}