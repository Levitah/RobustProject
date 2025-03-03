using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RobustProject.Services.Entities;
using RobustProject.Services.Enums;
using RobustProject.Services.Models;
using RobustProject.Services.Models.Response;
using RobustProject.Services.Specifications;

namespace RobustProject.Services.Repositories.SqlServer;

public abstract class Repository<TModel, TId, TEntity, TRepository> : IRepository<TModel, TId>
    where TRepository : Repository<TModel, TId, TEntity, TRepository>, IRepository<TModel, TId>
    where TModel : IModel<TId>
    where TEntity : Entity<TId>
{
    protected readonly IMapper Mapper;
    protected readonly IEntityRepository<TEntity, TId> EntityRepository;
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly ILogger<TRepository> Logger;
    protected Repository(IMapper mapper, IEntityRepository<TEntity, TId> entityRepository, IUnitOfWork unitOfWork, ILogger<TRepository> logger)
    {
        Mapper = mapper;
        EntityRepository = entityRepository;
        UnitOfWork = unitOfWork;
        Logger = logger;
    }

    public virtual async Task<IResponse<TModel>> AddAsync(TModel model, CancellationToken cancellationToken = default)
    {
        var response = new Response<TModel>();
        var entity = Mapper.Map<TEntity>(model);
        try
        {
            entity = await EntityRepository.AddAsync(entity, cancellationToken);
            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return response.AddError(ErrorCode.DataPersistenceError, Logger, ex, ex.Message);
        }
        response.Content = Mapper.Map<TEntity, TModel>(entity);
        return response;
    }

    public virtual async Task<IResponse<bool>> AddAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default)
    {
        var response = new Response<bool>(true);
        var entities = Mapper.Map<IEnumerable<TEntity>>(models);

        try
        {
            await EntityRepository.AddAsync(entities, cancellationToken);
            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            response.Content = false;
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

        response.Content = models;
        return response;
    }

    public virtual async Task<IResponse<TModel>> GetAsync(TId id, CancellationToken cancellationToken = default)
    {
        var response = new Response<TModel>();
        var entity = await EntityRepository.FindByIdAsync(id, cancellationToken);
        if (entity is null)
            return response.AddError(ErrorCode.NotFound, Logger);

        response.Content = Mapper.Map<TEntity, TModel>(entity);
        return response;
    }

    public virtual async Task<IResponse<IEnumerable<TModel>>> GetAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default)
    {
        var response = new Response<IEnumerable<TModel>>();
        var idList = ids.ToList();
        var entities = await EntityRepository.FindByIdAsync(idList, cancellationToken);

        if (!idList.All(id => entities.Select(x => x!.Id).Contains(id)))
            return response.AddError(ErrorCode.NotFound, Logger);

        response.Content = Mapper.Map<IEnumerable<TEntity>, IEnumerable<TModel>>(entities!);
        return response;
    }

    protected virtual async Task<IResponse<TModel>> FindOneBySpecificationAsync(Specification<TEntity> specification)
    {
        var entity = await EntityRepository.Where(specification).FirstOrDefaultAsync();
        return new Response<TModel>(Mapper.Map<TEntity, TModel>(entity!));
    }

    protected virtual async Task<IResponse<IEnumerable<TModel>>> FindBySpecificationAsync(Specification<TEntity> specification)
    {
        var entities = await EntityRepository.Where(specification).ToListAsync();
        return new Response<IEnumerable<TModel>>(Mapper.Map<IEnumerable<TEntity>, IEnumerable<TModel>>(entities!));
    }

    public virtual async Task<IResponse<bool>> DeleteAsync(TId id, CancellationToken cancellationToken = default)
    {
        var response = new Response<bool>() { Content = false };
        var entity = await EntityRepository.FindByIdAsync(id, cancellationToken);

        if (entity is null)
            return response.AddError(ErrorCode.NotFound, Logger);

        await EntityRepository.RemoveAsync(id, cancellationToken);
        try
        {
            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return response.AddError(ErrorCode.DataPersistenceError, Logger, ex, ex.Message);
        }

        response.Content = true;
        return response;
    }

    public virtual async Task<IResponse<bool>> DeleteAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default)
    {
        var response = new Response<bool>() { Content = true };
        var idList = ids.ToList();
        var entities = await EntityRepository.FindByIdAsync(idList, cancellationToken);

        if (!idList.All(id => entities.Select(e => e!.Id).Contains(id)))
            return response.AddError(ErrorCode.NotFound, Logger);

        await EntityRepository.RemoveAsync(idList, cancellationToken);
        try
        {
            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return response.AddError(ErrorCode.DataPersistenceError, Logger, ex, ex.Message);
        }
        return response;
    }

    public virtual async Task<IResponse<TModel>> UpdateAsync(TModel model, CancellationToken cancellationToken = default)
    {
        var response = new Response<TModel>();
        var entity = await EntityRepository.FindByIdAsync(model.Id, cancellationToken);
        if (entity is null) return response.AddError(ErrorCode.NotFound, Logger);
        entity = Mapper.Map(model, entity);
        try
        {
            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return response.AddError(ErrorCode.DataPersistenceError, Logger, ex, ex.Message);
        }

        response.Content = Mapper.Map<TEntity, TModel>(entity);
        return response;
    }

    public virtual async Task<IResponse<bool>> UpdateAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default)
    {
        var response = new Response<bool>() { Content = false };

        var modelList = models.ToList();

        var entities = await EntityRepository.FindByIdAsync(modelList.Select(x => x.Id), cancellationToken);

        if (!modelList.Select(x => x.Id).All(id => entities.Select(x => x!.Id).Contains(id)))
            return response.AddError(ErrorCode.NotFound, Logger);

        foreach (var entity in entities)
        {
            var model = modelList.First(m => m.Id!.Equals(entity!.Id));
            Mapper.Map(model, entity);
        }

        await UnitOfWork.SaveChangesAsync(cancellationToken);
        response.Content = true;
        return response;
    }
}