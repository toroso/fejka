using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FakeRepo.Core;

public abstract class IntRepositoryFake<TEntity> : RepositoryFake<int, TEntity>
    where TEntity : class
{
    private int _maxId;

    protected IntRepositoryFake(ISerializer serializer, Action<RepositoryConfiguration> configAction)
        : base(serializer, configAction)
    {
    }

    public async Task<int> InsertAsync(TEntity entity, CancellationToken ct)
    {
        var id = UniqueIntId();
        SetId(entity, id);
        await base.InsertAsync(id, entity, ct);
        return id;
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken ct)
    {
        var id = GetId(entity);
        await UpdateAsync(id, entity, ct);
    }

    public async Task<int> UpsertAsync(TEntity entity, CancellationToken ct)
    {
        var id = GetId(entity);
        await UpsertAsync(id, entity, ct);
        return id;
    }

    protected async Task UpdateByCherryPickingAsync(Expression<Func<TEntity, bool>> filterExpr, Action<TEntity> setAction, CancellationToken ct)
    {
        var entity = await GetByAsync(filterExpr, ct);
        if (entity != null)
        {
            var oldId = GetId(entity);
            setAction(entity);
            await UpdateAsync(oldId, entity, ct);
        }
    }

    protected async Task UpdateAllByCherryPickingAsync(Expression<Func<TEntity, bool>> filterExpr, Action<TEntity> setAction, CancellationToken ct)
    {
        foreach (var entity in await GetAllByAsync(filterExpr, ct))
        {
            setAction(entity);
            await UpsertAsync(GetId(entity), entity, ct);
        }
    }

    protected async Task UpsertByCherryPickingAsync(Expression<Func<TEntity, bool>> filterExpr, Func<TEntity> insertFunc, Action<TEntity> setAction, CancellationToken ct)
    {
        var entity = await GetByAsync(filterExpr, ct);
        if (entity != null)
        {
            setAction(entity);
            await UpdateAsync(GetId(entity), entity, ct);
        }
        else
        {
            entity = insertFunc();
            setAction(entity);
            await UpdateAsync(GetId(entity), entity, ct);
        }
    }

    protected TEntity Add(Func<int, TEntity> buildAction) => Add(buildAction(UniqueIntId()));

    protected int UniqueIntId() => ++_maxId;
}
