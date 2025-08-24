﻿using Despesas.Application.Dtos.Core;
using Domain.Core.ValueObject;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Despesas.Application.Dtos;

public class UsuarioDto : BaseDto
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string? Nome { get; set; }

    public string? SobreNome { get; set; }

    [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
    public string? Telefone { get; set; }

    [EmailAddress(ErrorMessage = "O campo Email é inválido.")]
    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    public string? Email { get; set; }

    [JsonIgnore]
    public PerfilUsuario? PerfilUsuario { get; set; }
}