using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RobustProject.Services.Entities;
using RobustProject.Services.Enums;
using RobustProject.Services.Models;
using RobustProject.Services.Models.Response;
using RobustProject.Services.Specifications;

namespace RobustProject.Services.Repositories.NoDatabase;

public class Repository<TModel, TEntity, TRepository> : IRepository<TModel>
    where TRepository : Repository<TModel, TEntity, TRepository>, IRepository<TModel>
    where TModel : IModel
    where TEntity : IEntity
{
    protected readonly IMapper Mapper;
    protected readonly IEntityRepository<TEntity> EntityRepository;
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly ILogger<TRepository> Logger;

    protected Repository(IMapper mapper, IEntityRepository<TEntity> entityRepository, IUnitOfWork unitOfWork, ILogger<TRepository> logger)
    {
        Mapper = mapper;
        EntityRepository = entityRepository;
        UnitOfWork = unitOfWork;
        Logger = logger;
    }

    public async Task<IResponse<TModel>> AddAsync(TModel model, CancellationToken cancellationToken = default)
    {
        var response = new Response<TModel>();
        var entity = Mapper.Map<TEntity>(model);

        try
        {
            entity = await EntityRepository.AddAsync(entity, cancellationToken);
            await UnitOfWork.SaveChangeAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return response.AddError(ErrorCode.DataPersistenceError, Logger, ex, ex.Message);
        }

        response.Content = Mapper.Map<TEntity, TModel>(entity);
        return response;
    }

    public async Task<IResponse<bool>> AddAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default)
    {
        var response = new Response<bool>(true);
        var entities = Mapper.Map<IEnumerable<TEntity>>(models);

        try
        {
            await EntityRepository.AddAsync(entities, cancellationToken);
            await UnitOfWork.SaveChangeAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return response.AddError(ErrorCode.DataPersistenceError, Logger, ex, ex.Message);
        }

        return response;
    }

    public async Task<IResponse<IEnumerable<TModel>>> GetPagedAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        var response = new Response<IEnumerable<TModel>>();
        IEnumerable<TModel> models;

        try
        {
            var entities = (await EntityRepository.GetAllAsync(skip, take, cancellationToken)).ToList();
            if (!entities.Any()) return response.AddError(ErrorCode.NotFound, Logger);
            models = Mapper.Map<IEnumerable<TEntity>, IEnumerable<TModel>>(entities);
        }
        catch (Exception ex)
        {
            return response.AddError(ErrorCode.DataPersistenceError, Logger, ex, ex.Message);
        }

        return response;
    }

    public async Task<IResponse<bool>> DeleteAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var response = new Response<bool>(true);

        try
        {
            EntityRepository.Delete(specification);
            await UnitOfWork.SaveChangeAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            response.Content = false;
            return response.AddError(ErrorCode.DataPersistenceError, Logger, ex, ex.Message);
        }

        return response;
    }

    public async Task<IResponse<bool>> UpdateAsync(TModel model, Specification<TEntity> specification, CancellationToken cancellationToken)
    {
        var response = new Response<bool>(true);

        try
        {
            var entity = await EntityRepository.Where(specification).FirstOrDefaultAsync();
            if (entity is null) return response.AddError(ErrorCode.NotFound, Logger);
            entity = Mapper.Map(model, entity);
            await UnitOfWork.SaveChangeAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            response.Content = false;
            return response.AddError(ErrorCode.DataPersistenceError, Logger, ex, ex.Message);
        }

        return response;
    }

    protected async Task<IResponse<TModel>> FindOneBySpecificationAsync(Specification<TEntity> specification)
    {
        var entity = await EntityRepository.Where(specification).FirstOrDefaultAsync();
        return new Response<TModel>(Mapper.Map<TEntity, TModel>(entity!));
    }

    protected async Task<IResponse<IEnumerable<TModel>>> FindBySpecificationAsync(Specification<TEntity> specification)
    {
        var entities = await EntityRepository.Where(specification).ToListAsync();
        return new Response<IEnumerable<TModel>>(Mapper.Map<IEnumerable<TEntity>, IEnumerable<TModel>>(entities!));
    }
}