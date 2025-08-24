﻿using System.ComponentModel.DataAnnotations;

namespace Despesas.Application.Dtos;
public class ChangePasswordDto : IValidatableObject
{
    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    public string? Senha { get; set; }

    [Required(ErrorMessage = "O campo Confirma Senha é obrigatório.")]
    public string? ConfirmaSenha { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Senha) || string.IsNullOrWhiteSpace(Senha))
            yield return new ValidationResult("Campo Senha não pode ser em branco ou nulo!");

        if (string.IsNullOrEmpty(ConfirmaSenha) || string.IsNullOrWhiteSpace(ConfirmaSenha))
            yield return new ValidationResult("Campo Confirma Senha não pode ser em branco ou nulo!");
    }
}
