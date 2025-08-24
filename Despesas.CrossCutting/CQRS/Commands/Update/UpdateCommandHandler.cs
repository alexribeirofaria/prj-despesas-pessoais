﻿using AutoMapper;
using Domain.Core.Aggreggates;
using MediatR;
using Repository.Persistency.UnitOfWork.Abstractions;

namespace CrossCutting.CQRS.Commands;

public sealed class UpdateCommandHandler<T> : IRequestHandler<UpdateCommand<T>, T> where T : BaseDomain, new()
{
    private readonly IUnitOfWork<T> _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCommandHandler(IUnitOfWork<T> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<T> Handle(UpdateCommand<T> request, CancellationToken cancellationToken)
    {
        var entityToUpdate = _mapper.Map<T>(request);

        var existingEntity = await _unitOfWork.Repository.GetById(entityToUpdate.Id);
        if (existingEntity is null)
            throw new InvalidOperationException($"{ nameof(existingEntity) } not found !");

        await _unitOfWork.Repository.Update(entityToUpdate);
        await _unitOfWork.CommitAsync();
        return entityToUpdate;        
    }
}
