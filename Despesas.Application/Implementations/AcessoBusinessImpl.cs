﻿using AutoMapper;
using Despesas.Application.Abstractions;
using Despesas.Application.Authentication;
using Despesas.Application.Authentication.Abstractions;
using Despesas.Application.Dtos;
using Despesas.Application.Dtos.Abstractions;
using Despesas.Application.Dtos.Core;
using Despesas.Infrastructure.Email.Abstractions;
using Domain.Entities;
using EasyCryptoSalt;
using Repository.Persistency.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static Domain.Core.ValueObject.PerfilUsuario;

namespace Despesas.Application.Implementations;
public class AcessoBusinessImpl<DtoCa, DtoLogin> : IAcessoBusiness<DtoCa, DtoLogin> where DtoCa : AcessoDto where DtoLogin : LoginDto, new()
{
    private readonly ICrypto _crypto;
    private readonly IMapper _mapper;
    private readonly IAcessoRepositorioImpl _repositorio;
    private readonly IEmailSender _emailSender;
    private readonly ISigningConfigurations _singingConfiguration;
    

    public AcessoBusinessImpl(IMapper mapper, IAcessoRepositorioImpl repositorio, SigningConfigurations singingConfiguration, IEmailSender emailSender, ICrypto crypto)
    {
        _mapper = mapper;
        _repositorio = repositorio;
        _singingConfiguration = singingConfiguration;
        _emailSender = emailSender;
        _crypto = crypto;
    }

    public async Task Create(DtoCa acessoDto)
    {
        var usuario = _mapper.Map<Usuario>(acessoDto);
        usuario = new Usuario().CreateUsuario(usuario);
        Acesso acesso = acessoDto != null ? _mapper.Map<Acesso>(acessoDto) : new Acesso();
        acesso.CreateAccount(usuario, _crypto.Encrypt(acessoDto.Senha));
        await Task.FromResult(_repositorio.Create(acesso));
    }

    public async Task<AuthenticationDto> ValidateCredentials(DtoLogin loginDto)
    {
        Acesso baseLogin = await _repositorio.Find(c => c.Login.Equals(loginDto.Email)) ?? throw new ArgumentException("Usuário Inexistente!");

        if (baseLogin?.Usuario?.StatusUsuario == StatusUsuario.Inativo)
            return AuthenticationException("Usuário Inativo!");

        if (!_crypto.Verify(loginDto?.Senha ?? "", baseLogin?.Senha ?? ""))
            return AuthenticationException("Senha inválida!");

        bool credentialsValid = baseLogin is not null && loginDto?.Email == baseLogin.Login;
        if (credentialsValid && baseLogin is not null)
        {
            baseLogin.RefreshToken = _singingConfiguration.GenerateRefreshToken();
            baseLogin.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_singingConfiguration.TokenConfiguration.DaysToExpiry);
            await _repositorio.RefreshTokenInfo(baseLogin);
            return await AuthenticationSuccess(baseLogin);
        }
        return AuthenticationException("Usuário Inválido!");
    }

    public async Task<AuthenticationDto> ValidateExternalCredentials(GoogleAuthenticationDto authenticationDto)
    {
        Acesso? baseLogin =  await _repositorio.Find(c =>
            (c.ExternalProvider == authenticationDto.ExternalProvider && c.ExternalId == authenticationDto.ExternalId) && c.Login.Equals(authenticationDto.Email)
        );

        if (baseLogin == null && !string.IsNullOrEmpty(authenticationDto.ExternalProvider))
        {
            var dtoCa = this._mapper.Map<DtoCa>(authenticationDto);
            dtoCa.Senha = Guid.NewGuid().ToString("N");
            await Create(dtoCa);

            baseLogin = await _repositorio.Find(c => c.Login.Equals(dtoCa.Email));
        }

        bool credentialsValid = baseLogin is not null && authenticationDto.Email == baseLogin.Login;
        if (credentialsValid && baseLogin is not null)
        {
            baseLogin.RefreshToken = _singingConfiguration.GenerateRefreshToken();
            baseLogin.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_singingConfiguration.TokenConfiguration.DaysToExpiry);
            await Task.FromResult(_repositorio.RefreshTokenInfo(baseLogin));
            return await AuthenticationSuccess(baseLogin);
        }
        return AuthenticationException("Usuário Inválido!");
    }

    public async Task<AuthenticationDto> ValidateCredentials(string refreshToken)
    {
        var baseLogin = await _repositorio.FindByRefreshToken(refreshToken);
        var credentialsValid =
            baseLogin is not null
            && baseLogin.RefreshTokenExpiry >= DateTime.UtcNow
            && refreshToken.Equals(baseLogin.RefreshToken)
            && _singingConfiguration.ValidateRefreshToken(refreshToken);

        if (credentialsValid && baseLogin is not null)
            return await AuthenticationSuccess(baseLogin);
        else if (baseLogin is not null)
            await this.RevokeToken(baseLogin.Id);

        return AuthenticationException("Refresh Token Inválido!");
    }

    public Task RevokeToken(Guid idUsuario)
    {
        return Task.Run(() => _repositorio.RevokeRefreshToken(idUsuario));
    }

    public async Task RecoveryPassword(string email)
    {
        string? newPassword = null;
        Acesso? acesso = null;

        try
        {
            var result = await _repositorio.Find(accout => accout.Login.Equals(email));
            CheckIfUserIsTeste(result.Id);
            IsValidEmail(email);
            newPassword = Guid.NewGuid().ToString().Substring(0, 8);
            await _repositorio.RecoveryPassword(email, newPassword);
            acesso = await _repositorio.Find(c => c.Login.Equals(email));
        }
        catch
        {
            _emailSender.SendEmailPassword(acesso?.Usuario, newPassword);
            throw new ArgumentException("Erro ao enviar email de recuperação de senha!");
        }
    }

    public async Task ChangePassword(Guid idUsuario, string password)
    {
        CheckIfUserIsTeste(idUsuario);
        await _repositorio.ChangePassword(idUsuario, _crypto.Encrypt(password));
    }

    private AuthenticationDto AuthenticationException(string message)
    {
        throw new ArgumentException(message);
    }

    private Task<AuthenticationDto> AuthenticationSuccess(Acesso acesso)
    {
        ClaimsIdentity identity = new ClaimsIdentity(new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new Claim("role",  ((Perfil)acesso.Usuario.PerfilUsuario.Id).ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, acesso.UsuarioId.ToString()),
            new Claim(JwtRegisteredClaimNames.AuthTime, acesso.RefreshTokenExpiry == null ? DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() : acesso.RefreshTokenExpiry.ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
        });

        DateTime createDate = DateTime.Now;
        DateTime expirationDate = createDate + TimeSpan.FromSeconds(_singingConfiguration.TokenConfiguration.Seconds);
        string token = _singingConfiguration.CreateAccessToken(identity);

        var dto = new AuthenticationDto
        {
            Authenticated = true,
            Created = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
            Expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
            AccessToken = token,
            RefreshToken = acesso.RefreshToken
        };

        return Task.FromResult(dto);
    }

    private static void IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email não pode ser em branco ou nulo!");

        if (email.Length > 256)
            throw new ArgumentException("Email inválido!");

        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        Regex regex = new Regex(pattern);

        if (!regex.IsMatch(email))
            throw new ArgumentException("Email inválido!");
    }

    private void CheckIfUserIsTeste(Guid userIdentity)
    {
        var isUserTeste = _repositorio.Find(accout => accout.Login.Contains("teste@teste.com") && accout.UsuarioId == userIdentity);
        if (isUserTeste is not null)
            throw new ArgumentException("A senha deste usuário não pode ser atualizada!");
    }
}