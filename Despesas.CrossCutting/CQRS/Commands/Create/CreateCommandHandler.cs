﻿using AutoMapper;
using Despesas.Repository.UnitOfWork.Abstractions;
using MediatR;

namespace CrossCutting.CQRS.Commands.Create;

public sealed class CreateCommandHandler<T> : IRequestHandler<CreateCommand<T>, T> where T : class, new()
{
    private readonly IUnitOfWork<T> _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCommandHandler(IUnitOfWork<T> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<T> Handle(CreateCommand<T> request, CancellationToken cancellationToken)
    {
        var newEntity = _mapper.Map<T>(request);
        await _unitOfWork.Repository.Insert(newEntity);
        await _unitOfWork.CommitAsync();
        return newEntity;
    }
}
