using System;
using System.Threading;
using System.Threading.Tasks;

namespace FakeRepo.Core;

public abstract class GuidRepositoryFake<TEntity> : RepositoryFake<Guid, TEntity>
    where TEntity : class
{
    protected GuidRepositoryFake(ISerializer serializer, Action<RepositoryConfiguration> configAction)
        : base(serializer, configAction)
    {
    }

    public Task InsertAsync(TEntity entity, CancellationToken ct) => base.InsertAsync(GetId(entity), entity, ct);
    public Task UpdateAsync(TEntity entity, CancellationToken ct) => base.UpdateAsync(GetId(entity), entity, ct);
    public Task UpsertAsync(TEntity entity, CancellationToken ct) => base.UpsertAsync(GetId(entity), entity, ct);

    /// <summary>
    /// Method for populating the database from unit tests
    /// </summary>
    protected TEntity Add(Func<Guid, TEntity> buildAction) => Add(buildAction(Guid.NewGuid()));
}
